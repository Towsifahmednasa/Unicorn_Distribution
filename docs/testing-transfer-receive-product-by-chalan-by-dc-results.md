# ✅ Test Execution Results — `submitButton_Click` TransactionScope (Transfer Receive Product By Chalan By DC)

Page: `SInventory_UI/TransferReceiveProductByChalanByDC.aspx`
Executed: 2026-09-24 · Server: `TOWSIF\MSSQLSERVER2019` (SQL Server 2019) · DB: `SalesDisDB_Unicorn_DB` (dev, local)
Reference pattern: [testing-delivery-invoice-transactionscope-results.md](testing-delivery-invoice-transactionscope-results.md)

## 1. Scope

Transfer chalan receive (DC-to-DC transfer-এর destination DC-তে stock receive)। Submit-এর সব DB write এক transaction-এ, race-safe duplicate gate, DAL boolean failure surface, UI double-click protection।

## 2. Existing Submit flow (inspection findings)

`submitButton_Click` → `Save()`: প্রতি grid row-এ `ChalanBLL.DCStockIn2` (stock insert + status update) + damage>0 হলে `SaveDCStoreFreeze2` → শেষে SAP REST call (try/catch swallowed) → success alert + report popup + Clear।

আগে যা ছিল না: transaction নেই (row 3 fail করলে row 1–2 committed থেকে যেত + IsDeliver='True' হয়ে যেত — chalan list থেকে হারিয়ে যেত অথচ stock আংশিক), DAL bool return সব জায়গায় ignored, submit-time duplicate check নেই (শুধু load-time `IsDeliver` filter), `submitButton_Click`-এ কোনো try/catch নেই (DB error হলে ASP.NET error page)।

## 3. Exact call chain (actual method names)

```
TransferReceiveProductByChalanByDC.aspx  submitButton_Click
  → Save()
      per grid row:
      → ChalanBLL.DCStockIn2(DCStockNew)                          [ChalanBLL.cs]
          → ClsPrimaryKeyFind.PrimaryKeyMax("DCStoreId","tblDCStore","SSIDB")   [read, EntLib]
          → ChalanDAL.DCStockInDALMain      → INSERT tblDCStore (Trans_Type='Receive', StockCondition='B2BReceive')
          → ChalanDAL.ChalanUpdate          → UPDATE tblChalanInfo SET IsDeliver='True'
      damage > 0 হলে:
      → ChalanBLL.SaveDCStoreFreeze2Checked(DCStoreFreezeDAO)     [নতুন, নিচে]
          → PrimaryKeyMax("DCStoreFreezeId","tblDCStoreFreeze")   [read]
          → ChalanDAL.SaveDCStoreFreeze2    → INSERT tblDCStoreFreeze (StockCondition='Restricted')
  → SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan      [scope-এর বাইরে, best-effort]
      → SP MakeRESTRequest: sp_OA* দিয়ে SAP REST POST + UPDATE SAP_API_Data..tblSAP_StockMovementMaster
```

সব DAL write `SInventorySql.Execute` → `ClsCommonInternalDAL.UpdateDataByUpdateCommandNew` → EntLib `SolutionConnectionStringSSIDB`। `Execute` = `ExecuteNonQuery > 0` (0 rows → false, exception propagate)।

## 4. Changed files

| File | Change |
|---|---|
| `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs` | `Save()` দুই phase-এ ভাগ (parse tx-এর বাইরে, writes এক TransactionScope-এ + duplicate gate); `submitButton_Click`-এ try/catch; `using System.Transactions;` |
| `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` | `rcvSubmitOnce()` script + `submitButton` OnClientClick — double-click UI protection (CustomerPayment convention) |
| `Library.DAL/SInventory_DAL/ChalanDAL.cs` | **additive**: `IsChalanAlreadyReceived(string)` — UPDLOCK gate, EntLib pool |
| `Library.BLL/SInventory_BLL/ChalanBLL.cs` | `DCStockIn2`: DAL bool check → throw (এই page-ই একমাত্র caller, verify করা); **additive**: `IsChalanAlreadyReceived`, `SaveDCStoreFreeze2Checked` |

Shared connection helper (`SInventorySql`, `DataAccessManager`, `ClsCommonInternalDAL`) অপরিবর্তিত। `ChalanBLL.SaveDCStoreFreeze2` (Subdeport page-ও ব্যবহার করে) ও `DCStockInSub` অপরিবর্তিত।

## 5. TransactionScope implementation

```
Phase 1 (tx-এর বাইরে): grid parse → List<(DCStockNew, DCStoreFreezeDAO?)>   // কোনো DB write নেই
Phase 2:
using (scope = TransactionScope(Required, ReadCommitted, 5min))
{
    if (aChalanBll.IsChalanAlreadyReceived(chalanId))   // UPDLOCK gate
        { "This chalan has already been received." + Clear; return; }
    প্রতি row: DCStockIn2 → (damage হলে) SaveDCStoreFreeze2Checked
    scope.Complete();
}
SAP REST (try/catch swallowed) + success alert + Popup + Clear   // scope-এর বাইরে
```

SAP REST call ইচ্ছাকৃতভাবে scope-এর **বাইরে**: external HTTP rollback হয় না, tx-এর ভিতরে রাখলে HTTP timeout পর্যন্ত সব lock ধরে রাখত; failure আগেও swallowed ছিল, তা-ই আছে।

## 6. Critical failure / boolean checks

- `DCStockInDALMain` false (0 rows) → `throw "DC stock receive insert failed for product … batch …"` → rollback
- `ChalanUpdate` false → `throw "Chalan received-status update failed…"` → rollback
- `SaveDCStoreFreeze2` false → `throw "DC store freeze save failed…"` → rollback (নতুন `SaveDCStoreFreeze2Checked` path)
- `submitButton_Click` catch → `"Transfer receive failed: <ex.Message>"` (আগে unhandled ছিল)

## 7. Challan validation

Load-time: `GetChalanReceieve` শুধু `IsDeliver IS NULL OR IN ('False','false','0')` chalan দেখায় (existing, unchanged)। Submit-time: gate (নিচে §21)। Unknown ChalanId হলে gate `true` (block) ফেরায়। Receive quantity validation page-এ আগেও ছিল না — `rcvQtyTextBox` **ReadOnly**, সবসময় challan-এর পুরো Quantity; বদলানো হয়নি (business rule preserve)।

## 8-13. Receive/stock/batch/DC Store/ledger/status flow

এই page-এ আলাদা receive header/detail টেবিল নেই — `tblDCStore`-ই একসাথে DC store, stock ledger এবং batch stock (প্রতি row-এ BatchNo, Trans_Type='Receive'/'Issue' movement rows, SUM(StockQty) = current stock)। Flow per line: `tblDCStore` INSERT (destination ComUnitId-তে +StockQty, batch সহ, cost `tblUnitPrice`/source row থেকে) + damage হলে `tblDCStoreFreeze` INSERT (Restricted)। Status = `tblChalanInfo.IsDeliver` (nvarchar(100) marker: NULL/'0'/'False' = unreceived, 'True' = received) — একই transaction-এ update হয়, fail হলে marker সহ rollback (T2/T3/T8 প্রমাণিত)। আলাদা inventory ledger টেবিল: নেই।

## 14. Journal flow

**Journal/Accounting posting: NOT APPLICABLE** — এই flow কোনো accounting voucher তৈরি করে না (BLL→DAL→DB trace-এ কোনো ACCDB write নেই)। কিছু যোগ করা হয়নি।

## 15. Stored procedure analysis

Submit flow-এ একটাই SP: `MakeRESTRequest` (`sp_helptext` দিয়ে dump করা) — sp_OACreate/sp_OAMethod দিয়ে SAP endpoint-এ HTTP POST + `SAP_API_Data..tblSAP_StockMovementMaster` UPDATE। **নিজস্ব BEGIN TRAN/COMMIT/ROLLBACK/TRY-CATCH নেই** → nested transaction issue নেই; call scope-এর বাইরে বলে interaction-ও নেই। বাকি সব DB operation inline parameterized SQL (উপরে §3)।

## 16. Tables affected

| Table | Operation |
|---|---|
| `tblDCStore` | INSERT per line (receive movement row) |
| `tblChalanInfo` | UPDATE `IsDeliver='True'` |
| `tblDCStoreFreeze` | INSERT (শুধু damage>0 হলে) |
| `SAP_API_Data..tblSAP_StockMovementMaster` | UPDATE (scope-এর বাইরে, best-effort SAP notify) |

## 17. Connection strings

Scope-এর ভিতরের **সব** call (gate সহ) EntLib ← Web.config `SolutionConnectionStringSSIDB` (`Data Source=TOWSIF\MSSQLSERVER2019;Initial Catalog=SalesDisDB_Unicorn_DB;User ID=sa;…`) — এক pool। Gate ইচ্ছাকৃতভাবে `SInventorySql.GetDataTable` (DataAccessManager-এর আলাদা string) ব্যবহার করে **না**, `ClsCommonInternalDAL.DataContainerDataTable` ব্যবহার করে — delivery-invoice page-এ দ্বিতীয় pool-ই promotion-এর কারণ ছিল।

## 18. MSDTC promotion status

**NOT REQUIRED — promotion ঘটে না।** Committed happy path-এ `Transaction.Current.TransactionInformation.DistributedIdentifier == Guid.Empty` (harness output: `msdtc=local`)। কারণ: এই page-এর পুরো transactional flow এক connection string/pool-এ (§17)। আগের page-গুলোর মতো MSDTC production requirement এখানে **নেই**।

## 19. Business / idempotency key

```
IDEMPOTENCY / BUSINESS KEY:
tblChalanInfo.ChalanId + processed marker tblChalanInfo.IsDeliver
(NULL/'0'/'False' = unreceived; 'True' = received)
```

Schema/code থেকে identify করা (invent না): `ChalanLoadInReceive`/`GetChalanReceieve` এই marker দিয়েই receivable list filter করে, `ChalanUpdate` receive-এ এটাই set করে। Marker transaction-এর ভিতরে লেখা হয় → rollback হলে marker-ও ফেরে (retry-safe, T8)।

## 20. Existing unique indexes / constraints

`tblChalanInfo`: PK `ChalanId`, `UQ_ChalanNo (ChalanNo)`। `tblDCStore`: PK `DCStoreId` (movement row id — receive business key-তে **কোনো unique constraint নেই**)। `tblDCStoreFreeze`: PK `DCStoreFreezeId`। `tblChalanDetail`: PK `ChalanDetailsId`।

```
DB UNIQUE CONSTRAINT (receive business key-তে): NO
```

Production schema-তে নতুন constraint যোগ করা হয়নি (instruction অনুযায়ী)। চাইলে ভবিষ্যতে filtered unique index (যেমন `tblDCStore(ChalanDetailsId) WHERE Trans_Type='Receive'`) বিবেচনা করা যায় — তার আগে existing data-র duplicate audit লাগবে।

## 21. Server-side duplicate protection (নতুন)

Scope-এর প্রথম statement — race-safe atomic gate (`ChalanDAL.IsChalanAlreadyReceived`):

```sql
SELECT ISNULL(IsDeliver,'') FROM dbo.tblChalanInfo WITH (UPDLOCK, ROWLOCK) WHERE ChalanId=@ChalanId
```

UPDLOCK transaction শেষ পর্যন্ত থাকে → concurrent Request B এই SELECT-এ block হয়; A commit করলে B committed `'True'` দেখে **কিছু না লিখে** "This chalan has already been received." দেখায়; A rollback করলে B স্বাভাবিক এগোয়। SERIALIZABLE/app-lock/schema change লাগেনি। আগের check-then-act pattern-ও ছিল না — গোটা duplicate protection-ই নতুন।

## 22. UI double-submit protection (secondary)

`submitButton` (LinkButton): `OnClientClick="return rcvSubmitOnce(this);"` — sweetAlert confirm-এর পরে button lock (`pointerEvents='none'`, opacity), UpdatePanel re-render-এ আবার enabled (failure-এ retry সম্ভব)। CustomerPayment-এর `paySubmitOnce` convention-এর কপি। Primary protection server gate।

## 23. Concurrent protection

§21-এর gate-ই। T6-এ প্রমাণিত: দুই thread একই মুহূর্তে → exactly 1 SUCCESS + 1 DUPLICATE, DB-তে ঠিক একটাই receive set।

## 24-33. TEST 1–10 results

Execution: scratchpad harness (`TransferRcvHarness.exe`) — **real BLL/DAL/dev DB** দিয়ে `Save()`-এর exact phase-2 sequence replay (same gate, same TransactionScope options, same BLL calls, same throw-গুলো); row data grid-এর bind source (`tblChalanDetail`+`tblChalanInfo`, chalan 43: 2 product/2 batch) থেকে। Site precompile (`aspnet_compiler`) exit 0। প্রতি test-এ before/after snapshot Suppress scope-এ।

| # | Plan test | Harness test | Result |
|---|---|---|---|
| TEST 1 | Happy path | T1: 2-line commit → `rcvRows=2, rcvQty=2, IsDeliver=True`, freeze 0 | ✅ PASS |
| TEST 2 | Failure after stock update | T2: সব stock row লেখার পরে controlled throw → before==after byte-for-byte | ✅ PASS |
| TEST 3 | Failure after status update | T3: সব row + IsDeliver='True' হওয়ার পরে throw → সব rollback (marker সহ) | ✅ PASS |
| TEST 4 | Boolean false | T4: **আসল** DAL false (bogus ChalanDetailsId → INSERT..SELECT 0 rows) row2-তে; row1 already written → throw "DC stock receive insert failed…" → দুটোই rollback | ✅ PASS |
| TEST 5 | Double submit | T5: পরপর দুই submit → 1st SUCCESS, 2nd DUPLICATE, ঠিক এক receive set | ✅ PASS |
| TEST 6 | Concurrent double submit | T6: 2 thread simultaneous → exactly 1 SUCCESS + 1 DUPLICATE, `rcvRows=2, rcvQty=2` (একটাই set) | ✅ PASS |
| TEST 7 | Retry after success | T7: commit-এর পরে আবার → DUPLICATE, state unchanged | ✅ PASS |
| TEST 8 | Retry after rollback | T8: T4-এর rollback-এর পরে (IsDeliver আবার '0') retry → SUCCESS | ✅ PASS |
| TEST 9 | Partial receive | **NOT APPLICABLE** — `rcvQtyTextBox` ReadOnly (সবসময় full challan qty), damage column `Visible="False"`; page partial receive support করে না, chalan এক shot-এ পুরোটা receive হয়। Business rule বদলানো হয়নি। | N/A |
| TEST 10 | Multi-product/multi-batch | T1/T2/T4/T6 সবই 2-product/2-batch। T10a: damage>0 → freeze row commit (`frzChalan=1, rcvQty=1`); T10b: একই + fail-before-Complete → freeze+stock+status সব rollback | ✅ PASS |

T4 একইসাথে mid-loop failure-ও প্রমাণ করে: row1 সত্যিই insert হয়েছিল, row2 fail → row1 rollback (all-or-nothing)।

## 34-35. Before/after DB verification + rollback

Snapshot per test: `tblChalanInfo.IsDeliver` (chalan 43), `tblDCStore` COUNT+MAX + receive rows/qty `WHERE ChalanDetailsId IN (88,89) AND Trans_Type='Receive'`, `tblDCStoreFreeze` COUNT + `WHERE ChalanNo='IDT000043'`। প্রতিটি rollback test-এ before == after **identical** (harness output-এ দৃশ্যমান)। Run শেষে reset → final state == pre-test state (`IsDeliver=0, dcMax=2085, rcvRows=0, frzCount=0`)।

Restore point: `D:\Projects\Unicorn\SalesDisDB_Unicorn_DB_PreTransferRcvTest.bak` (COPY_ONLY, run-এর আগে নেওয়া, রাখা আছে)। Production DB স্পর্শ করা হয়নি।

## 36. Build / regression result

- `Library.DAL.csproj`, `Library.BLL.csproj` msbuild: **success** (শুধু pre-existing warnings)
- সাইট precompile `aspnet_compiler`: **exit 0** (এক compile error পাওয়া গিয়ে fix করা হয়েছিল — DAO `ChalanId` int? cast)
- Regression scope: `ChalanBLL.DCStockIn2`-এর একমাত্র caller এই page (grep-verified); `SaveDCStoreFreeze2`/`DCStockInSub` (Subdeport pages) অপরিবর্তিত; shared helper untouched; পুরো সাইট compile হয় — অন্য page-এ প্রভাব নেই।

## 37. Production deployment requirements

1. **MSDTC লাগবে না** — এই page-এর flow এক connection pool-এ, promotion ঘটে না (dev-এ verified)। তবে অন্য কেউ ভবিষ্যতে scope-এর ভিতরে `SInventorySql.GetDataTable` জাতীয় (DataAccessManager pool) call ঢোকালে promotion ফিরবে।
2. Deploy-এ **Library.DAL.dll + Library.BLL.dll rebuild করে সাইটের Bin-এ দিতে হবে** (নতুন `IsChalanAlreadyReceived`/`SaveDCStoreFreeze2Checked`) — শুধু aspx/aspx.cs কপি করলে MissingMethodException।
3. Failure message এখন আসল exception দেখায় (`Transfer receive failed: …`) — চাইলে log+generic-এ নামানো যায়।
4. SAP notify আগের মতোই best-effort (fail হলে receive commit থাকে, SAP-এ যায় না) — এটা pre-existing behavior, বদলানো হয়নি।

## 38. Remaining risks / limitations

- **DCStoreId MAX+1 race (pre-existing, পুরো codebase-এর pattern):** `PrimaryKeyMax`-এর MAX+1 আর `DCStockInDALMain`-এর ভিতরের MAX+1 আলাদা session-এর সাথে race করতে পারে; PK violation হলে এখন exception → পুরো rollback + retry (আগে partial state থাকত) — তাই আগের চেয়ে নিরাপদ, কিন্তু root cause (identity/sequence নয়) রয়ে গেছে। Freeze row-র `DCStoreId` link-ও এই BLL-computed id-র উপর নির্ভরশীল (same-tx-এ consistent, প্রমাণিত)।
- Gate-এ blocked concurrent request A-র transaction duration পর্যন্ত অপেক্ষা করে (worst case timeout → user retry → duplicate message) — duplicate হয় না, শুধু দেরি।
- Gate এই page-এর path-এ; অন্য কোনো page/SP `IsDeliver` bypass করে একই chalan-এর receive লিখলে সেটা gate-এর বাইরে (sibling `SubdeportTransferReceiveProductByChalanByDC` একই ধাঁচের কিন্তু আলাদা flow — চাইলে একই treatment দেওয়া যাবে)।
- Browser/UI-level automation test হয়নি (আগের page-গুলোর মতোই call-sequence-level replay); `rcvSubmitOnce` UI protection manually exercise করা হয়নি — pattern CustomerPayment-এ প্রমাণিত।
- Receive business key-তে DB unique constraint নেই (report only, §20)।
- `IsDeliver` nvarchar marker — কেউ ম্যানুয়ালি অন্য spelling লিখলে gate 'True'/'1' ছাড়া সব unreceived ধরবে (existing load query-র সাথে consistent)।
