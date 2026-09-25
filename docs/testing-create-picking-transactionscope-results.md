# ✅ Test Execution Results — `bmitButton_Click` TransactionScope (Create Picking Detail For WareHouse)

Page: `SInventory_UI/CreatePickingDetailForWareHouse.aspx?ReqId=18`
Executed: 2026-09-26 · Server: `TOWSIF\MSSQLSERVER2019` · DB: `SalesDisDB_Unicorn_DB` (dev, local)
Reference pattern: [testing-transfer-receive-product-by-chalan-by-dc-results.md](testing-transfer-receive-product-by-chalan-by-dc-results.md)

## 1. Scope

Warehouse picking/issue: requisition header update + প্রতি product-এর FEFO lot allocation (`tblStockInTransfar`) + central stock/weighted-avg SP + child update — সব এক transaction-এ। আগে transaction ছিল না; SP failure `UpdateAction`-এ swallow হতো।

## 2. Call chain

```
bmitButton_Click → SaveData()
  using TransactionScope(Required, ReadCommitted, 5min)
    RequisitionBLL.IsPickingAlreadyDone            UPDLOCK gate on tblRequisition + COUNT tblStockInTransfar   [নতুন]
    UpdatePickingInformationOnRequisition          UPDATE tblRequisition
    UpdateIssueInformationOnRequisitionChild       per child:
      UpdateCentralStockAndStockInTransfar
        GetCurrentStockofCentralStock(ByBatch)     SELECT (CTE)                       [EntLib pool-এ সরানো]
        ProductPriceDetailWithCaseBLL              SELECT                              [EntLib pool-এ সরানো]
        PrimaryKeyMax                              SELECT MAX+1
        StockInTransfarInsertDAL                   INSERT tblStockInTransfar
        UpdateCentralStockStockOut                 UPDATE tblCentralStore
        ProcessWeightedAvg                         SP tbl_INV_CentralStoreStoIssue     [এখন throw করে]
      UpdateIssueInformationOnRequisitionChildDAL  UPDATE tblRequsitionChild
    scope.Complete()
  → success: Clear + alert + print popup ; failure/duplicate: alert, Clear/print নেই
```

## 3. Changed files

| File | Change |
|---|---|
| `Solution.Web/SInventory_UI/CreatePickingDetailForWareHouse.aspx.cs` | `SaveData()` → `bool`; DB writes এক TransactionScope-এ; try/catch; duplicate gate; print শুধু success-এ |
| `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` | **additive**: `ExecuteStoredProcedure` (error propagate করে) |
| `Library.DAL/SInventory_DAL/RequisitionDAL.cs` | `ProcessWeightedAvg` → নতুন throwing call; **additive** `IsPickingAlreadyDone`; `GetCurrentStockofCentralStock`/`ByBatch` → EntLib pool |
| `Library.DAL/SInventory_DAL/ProductDAL.cs` | `ProductPriceDetailWithCase` → EntLib pool (একই query/result) |
| `Library.BLL/SInventory_BLL/RequisitionBLL.cs` | **additive**: `IsPickingAlreadyDone` |

## 4. Bug found by the test: MSDTC promotion

প্রথম run-এ সব rollback test PASS, কিন্তু `Transaction.Current.TransactionInformation.DistributedIdentifier` → **PROMOTED-TO-MSDTC**। কারণ: `SInventorySql.GetDataTable` (`DataAccessManager`) আলাদা connection string ব্যবহার করে (`Encrypt=True;TrustServerCertificate=True`) → আলাদা pool → scope-এর ভিতরে দ্বিতীয় connection। Fix: scope-এর ভিতরের তিনটি read (`GetCurrentStockofCentralStock`, `...ByBatch`, `ProductPriceDetailWithCase`) `ClsCommonInternalDAL.DataContainerDataTable(..., "SSIDB")`-এ সরানো (Execute-এর সাথে এক pool)। দ্বিতীয় run: `msdtc=local` — **MSDTC লাগে না**।

## 5. Results (harness: real BLL/DAL/dev DB, page-এর exact scope block replay)

| # | Test | Result |
|---|---|---|
| T1 | সব write-এর পরে throw (Complete-এর আগে): mid-flight dirty read-এ 11 `tblStockInTransfar` row দেখা গেছে; rollback-এর পরে snapshot before==after | ✅ PASS |
| T2 | **আসল SP failure** (SP rename): exception surface হয় ("Could not find stored procedure…"); header+stock সব rollback; SP restored | ✅ PASS (আগে swallow হতো) |
| T3 | **আসল row-2 failure** (temporary trigger `A1411`-এর weighted-avg insert-এ RAISERROR): row 1 আগেই লেখা হয়েছিল (dirty read = 2), তারপরও rollback-এ header/row 1/row 2 সব ফেরত; trigger removed | ✅ PASS |
| T4 | 2 thread একই requisition একসাথে: exactly 1 SUCCESS + 1 DUPLICATE; issued একবারই (sit qty 300 = 100+200); `msdtc=local` | ✅ PASS |
| T5 | success-এর পরে retry → DUPLICATE, state unchanged | ✅ PASS |

**PASS=12 FAIL=0** (fix-এর পরে)। Snapshot: header (`CreatePicking/Submit/PickingNo/TotalPrice`), child sum/count, `tblStockInTransfar` (req ও global count/max), `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock` count/max, `tblCentralStore` sum।

## 6. Build

- `Library.DAL`/`Library.BLL` msbuild: success
- Site precompile `aspnet_compiler` (fresh DLL সহ): exit 0, error 0

## 7. DB restore

Restore point `D:\Projects\Unicorn\SalesDisDB_Unicorn_DB_PrePickingTxTest.bak` (COPY_ONLY)। Test শেষে DB restore করা হয়েছে: `CreatePicking` NULL, `tblStockInTransfar` (ReqId 18) 0 row, test trigger নেই, SP আছে। Production DB স্পর্শ করা হয়নি।

## 8. Deployment

- **Library.DAL.dll + Library.BLL.dll site Bin-এ দিতে হবে** (নতুন `IsPickingAlreadyDone`/`ExecuteStoredProcedure`) — শুধু aspx.cs কপি করলে MissingMethodException।
- MSDTC লাগবে না (dev-এ verified)। ভবিষ্যতে কেউ scope-এর ভিতরে `SInventorySql.GetDataTable` ঢোকালে promotion ফিরবে।

## 9. Remaining risks / not covered

- **UI-level test হয়নি** — page-এর grid/UpdatePanel/print popup browser দিয়ে চালানো হয়নি; scope block-এর replay + site compile।
- **`PrimaryKeyMax` MAX+1 race** (অন্য requisition-এর সাথে concurrent হলে) — পুরো codebase-এর pre-existing pattern; PK collision হলে এখন exception → rollback।
- **`UpdateCentralStockStockOut` ID mismatch (pre-existing, এই কাজের বাইরে):** `GetCurrentStockofCentralStock`-এর `ReceiveId` আসে `tbl_INV_Central_BatchWiseStock.Batch_Stock_Id` থেকে, কিন্তু update হয় `tblCentralStore.ReceiveId`-এ। Test-এ 11 lot-এর মধ্যে ReceiveId 1427 (2N2704X)-এর `tblCentralStore`-এ কোনো row নেই; ফলে picking-এর পরে `tblCentralStore` sum অপরিবর্তিত (314 → 314)। ভুল row update হচ্ছে কিনা আলাদা করে verify করা দরকার।
- `UpdateIssueInformationOnRequisitionChildDAL`/`UpdatePickingInformationOnRequisitionDAL`-এর bool return এখনো ignored (0-row update silent)।
- Blocked concurrent request A-র transaction শেষ পর্যন্ত অপেক্ষা করে (duplicate হয় না, শুধু দেরি)।
