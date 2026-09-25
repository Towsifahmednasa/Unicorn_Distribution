# ✅ Test Execution Results — `saveButton_Click` TransactionScope (Delivery Sales Return)

Page: `SInventory_UI/DeliverySalesReturn.aspx`
Executed: 2026-09-24 · Server: `TOWSIF\MSSQLSERVER2019` (SQL Server 2019) · DBs: `SalesDisDB_Unicorn_DB` + `CSTL_ACCDB_Unicorn` (dev, local)
Sibling reports: [testing-delivery-invoice-transactionscope-results.md](testing-delivery-invoice-transactionscope-results.md), [testing-sales-return-transactionscope-results.md](testing-sales-return-transactionscope-results.md), [testing-invoice-transactionscope-results.md](testing-invoice-transactionscope-results.md)

## 1. Scope

`DeliverySalesReturn.aspx`-এর save flow (delivery invoice-এর against-এ customer return) production-safe করা হয়েছে: এক TransactionScope, সব critical bool/identity check, journal verify, duplicate/double-save protection (one-time token + invoice-row UPDLOCK gate + in-tx over-return re-check + UI disable)। Production DB ছোঁয়া হয়নি; সব test dev DB-তে, শেষে দুই DB-ই backup থেকে restore করা হয়েছে (verified)।

## 2-3. Existing flow + exact call chain (inspect করা, অনুমান নয়)

```
DeliverySalesReturn.aspx (Button1 "Save Return")
    ↓
saveButton_Click                                   [DeliverySalesReturn.aspx.cs]
    ↓
CustomerReturnBLL.SaveCustomerReturn(master)       [CustomerReturnBLL.cs → CustomerReturnDAL.cs:17]
    · INSERT tblCustomerReturnMaster (IDENTITY, SCOPE_IDENTITY return)     ← header
    ↓  প্রতি grid row (ReturnQty > 0):
CustomerReturnBLL.SaveCustomerReturnDetail          [CustomerReturnDAL.cs:51]
    · INSERT tblCustomerReturnDetail (IDENTITY)                            ← detail
CustomerReturnBLL.InsertDCStoreStockReturn          [CustomerReturnDAL.cs:~220]
    · INSERT tblDCStore — নতুন Receive row, StockCondition='SalesReturn',
      DCStoreId = inline MAX+1, source = invoice detail-এর DCStoreId (Trans_Type='Receive')  ← stock
    ↓
CustomerReturnBLL.PostDeliverySalesReturn(id)       [CustomerReturnDAL.cs → UpdateAction]
    · EXEC sp_DeliverySalesReturnPosting @ReturnId
      CSTL_ACCDB: tblDebitCreditVoucherMaster/Detail (AR reversal TableID 2018 + COGS reversal 2023),
      SalesDisDB: tblCustomerReturnMaster.IsPosting=1                       ← journal
    ↓
Clear() + "Data Saved and Posted Successfully !!!"
```

আগে **কোনো transaction ছিল না, কোনো bool/identity check ছিল না, কোনো duplicate protection ছিল না** — header commit হয়ে detail/stock/journal fail করলে partial return থেকে যেত, আর double-click-এ পুরো set দুইবার হতো (SP-তেও duplicate guard নেই)।

## 4. Changed files

| File | Change |
|---|---|
| `Solution.Web/SInventory_UI/DeliverySalesReturn.aspx.cs` | `using System.Transactions;` · `IssueSaveToken()` new (Page_Load !IsPostBack) · `saveButton_Click`: token gate + TransactionScope + UPDLOCK gate + সব identity/bool check + over-return re-check + journal verify + `catch → "Delivery sales return failed: <msg>"` |
| `Solution.Web/SInventory_UI/DeliverySalesReturn.aspx` | `Button1`: `UseSubmitBehavior="false"` + confirm → disable → 'Saving...' |
| `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs` | **additive**: `LockInvoiceForReturn` (UPDLOCK gate), `HasOverReturn` (in-tx validation), `IsDeliverySalesReturnPosted` (journal verify) |
| `Library.BLL/SInventory_BLL/CustomerReturnBLL.cs` | ৩টি passthrough |

Shared helper (`ClsCommonInternalDAL`, `DataAccessManager`), Web.config, DB schema — **কিছুই বদলানো হয়নি**। অন্য কোনো page ছোঁয়া হয়নি।

## 5. TransactionScope implementation

```
token gate (consumed? → "already submitted", DB untouched)
   ↓
using (TransactionScope(Required, ReadCommitted, 5min))
{
    if (!LockInvoiceForReturn(invoiceId)) throw;        // UPDLOCK ROWLOCK — tx শেষ পর্যন্ত ধরা
    id = SaveCustomerReturn(...);   if (id <= 0) throw;
    প্রতি row: detailId = SaveCustomerReturnDetail;      if (detailId <= 0) throw;
              if (!InsertDCStoreStockReturn(...)) throw;
    if (savedDetails == 0) throw;
    if (HasOverReturn(invoiceId)) throw;                // concurrent return-এর against-এ re-check
    PostDeliverySalesReturn(id);                        // bool অবিশ্বস্ত (section 6)
    if (!IsDeliverySalesReturnPosted(id)) throw "Journal posting failed";
    scope.Complete();
}
   ↓
token consume + নতুন token → Clear() → "Data Saved and Posted Successfully !!!"   // scope-এর বাইরে
catch(Exception ex) → "Delivery sales return failed: " + ex.Message               // token বেঁচে থাকে → retry
```

## 6. Critical boolean/identity checks

| Method | আগে | এখন |
|---|---|---|
| `SaveCustomerReturn` (header, identity) | `id > 0` check ছিল কিন্তু fail-এ silent no-op | `<= 0 → throw` |
| `SaveCustomerReturnDetail` (identity) | unchecked (0 = fail) | `<= 0 → throw` |
| `InsertDCStoreStockReturn` (stock, bool) | **ignored** — source Receive row না মিললে 0 rows → false, silently stock ফেরত হতো না | `false → throw` |
| empty detail set | হতো না (Validation আছে) | তবু `savedDetails == 0 → throw` (defense) |
| `PostDeliverySalesReturn` (journal, bool) | **ignored** | bool অবিশ্বস্ত: SP-তে `SET NOCOUNT ON` → `ExecuteNonQuery` = -1 → `UpdateAction` **success-এও false** দেয় (আর SQL error swallow করে false দেয়)। তাই bool-এর বদলে `IsDeliverySalesReturnPosted` verify |

`IsDeliverySalesReturnPosted` SP-র নিজের logic-এর আয়না: `TotalAmount <= 0` হলে SP-র designed early-RETURN no-op (OK ধরা হয়); নাহলে `IsPosting = 1` — SP-র **শেষ statement**, সব voucher insert-এর পরেই কেবল set হয়। Exception / no-op / mid-flight error — সব এতে ধরা পড়ে (Test 3/3b/4 প্রমাণ)।

## 7. Stock flow

Return-এ stock ফেরত = `tblDCStore`-এ নতুন **Receive** row (StockCondition='SalesReturn', StockQty=ReturnQty, CostPerUnit source row থেকে, `ReturnFromInvoiceDetailId`+`SalesReturnDetailId` link)। Source: invoice detail-এর `DCStoreId`-এর Receive row। `DCStoreId = inline MAX+1` (PK collision হলে এখন পুরো tx rollback)। Rollback test 2/8/9-এ প্রমাণিত: failure-এ stock row থাকে না।

## 8. DC Store flow

উপরেরটাই এই page-এর DC Store update — আলাদা `UpdateDCStoreQty` এই save path-এ ব্যবহৃত হয় না (DAL-এ আছে, অন্য flow-র জন্য)।

## 9. Journal flow

Section 2-3 + 6 দেখো। `sp_DeliverySalesReturnPosting` writes: ACCDB `tblDebitCreditVoucherMaster` (AR reversal, TableID **2018**, CommonPK_ID=CustReturnId; stock value > 0 হলে COGS reversal, TableID **2023**) + `tblDebitCreditVoucherDetail` (Dr Sales Return GL 4170-fallback / Cr Delivery Income 5968 deduction থাকলে / Cr AR 2112; COGS: Dr FG 4156 / Cr COGS 4162) + SalesDisDB `tblCustomerReturnMaster.IsPosting=1`। GL id `CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity` (PageId=23) থেকে, fallback hard-coded।

## 10. Stored procedure analysis (dev DB থেকে dump করা)

`sp_DeliverySalesReturnPosting`:
- `SET NOCOUNT ON` — bool-unreliability-র কারণ (section 6)
- **নিজের `BEGIN TRAN`/`COMMIT`/`ROLLBACK`/`TRY-CATCH`/`XACT_ABORT` নেই** → ambient TransactionScope-এ enlist, page rollback-এ SP-র সব লেখা যায় (Test 3/4 প্রমাণ) — nested transaction issue নেই
- Guards: `@InvoiceId IS NULL OR @TotalReturnAmount <= 0` → early RETURN (designed no-op)
- **কোনো duplicate guard নেই** (sibling SP-গুলোর NOT-EXISTS check এটায় নেই) — একই ReturnId-তে দুইবার call = duplicate voucher; duplicate protection তাই আরও জরুরি ছিল
- VoucherCode = per-day MAX+1 substring — সেই পরিচিত race-প্রবণ pattern (দুই ভিন্ন return-এর truly-concurrent post-এ একই code সম্ভব; PK ভিন্ন হবে)
- ⚠ **Cross-flow TableID overlap:** এই SP TableID **2018/2023** ব্যবহার করে `CommonPK_ID = CustReturnId` দিয়ে; `sp_SalesReturnPosting`-ও 2018/2023 ব্যবহার করে `CommonPK_ID = ReturnInvoiceId` দিয়ে — **দুই ভিন্ন টেবিলের id এক namespace-এ**। Voucher-to-source traceability ambiguous (existing design, বদলানো হয়নি — report only)

## 11. Tables affected (save flow)

`tblCustomerReturnMaster` (header, IDENTITY) · `tblCustomerReturnDetail` (IDENTITY) · `tblDCStore` (stock receive, inline MAX+1) · ACCDB: `tblDebitCreditVoucherMaster`, `tblDebitCreditVoucherDetail` · (`tblSubsidiaryInfo`/`tblReferenceInfo` এই SP পড়ে, লেখে না)

## 12. Connection strings

Save flow-এর **সব** operation (header/detail/stock insert, gate/verify query, journal SP) `ClsCommonInternalDAL` দিয়ে EntLib `SolutionConnectionStringSSIDB` (Web.config: `Data Source=TOWSIF\MSSQLSERVER2019;Initial Catalog=SalesDisDB_Unicorn_DB;User ID=sa;…`) — **একটাই connection pool**। `DataAccessManager`-এর দ্বিতীয় pool এই page-এর save path-এ নেই (sibling page-গুলোর সাথে মূল পার্থক্য)।

## 13. MSDTC promotion status

**Promotion হয় না — `NOT REQUIRED`।** প্রতিটি সফল save-এ scope-এর ভিতরে `Transaction.Current.TransactionInformation.DistributedIdentifier == Guid.Empty` observed (harness output: `DTC=none (local tx)`)। কারণ: সব operation একই pool-এর একটাই connection reuse করে; ACCDB-র cross-database write একই connection দিয়ে হয় (promotion ঘটায় না)। Sibling page-গুলোর মতো MSDTC production requirement এখানে **নেই**। নতুন ৩টি DAL method ইচ্ছাকৃতভাবে একই EntLib pool-এ রাখা হয়েছে যাতে এটা ভবিষ্যতেও ধরে রাখে।

## 14. Business / idempotency key

```
IDEMPOTENCY / BUSINESS KEY:
tblInvoice.InvoiceId (source delivery invoice) — কিন্তু 1-return-per-invoice নয়:
partial return সমর্থিত (AvailableQty = DeliveryTotalQuantity - SUM(ReturnQty)), তাই
"processed flag" জাতীয় one-shot marker এখানে ভুল হতো। ব্যবহৃত সমন্বয়:
  (a) per-form one-time save token (GUID, Session "DSRTOKEN_<guid>", commit-এর পরেই কেবল consume)
      = একটি rendered form-এর একক submission identity (double-click/retry boundary)
  (b) InvoiceId-র উপর UPDLOCK gate + in-tx over-return re-check
      = per-invoice quantity invariant: SUM(ReturnQty) ≤ DeliveryTotalQuantity per line
কোনোটাই অনুমান নয় — LoadInvoiceNew-এর AvailableQty logic থেকেই business rule নেওয়া।
```

## 15. Existing unique indexes/constraints

- `tblCustomerReturnMaster`: শুধু `PK_tblCustomerReturnMaster (CustReturnId)` — InvoiceId-তে unique index **নেই** এবং থাকা উচিতও না (multi-partial-return বৈধ) → **DB UNIQUE CONSTRAINT: NO** (সঠিক constraint হলো quantity invariant, যেটা DB constraint-এ প্রকাশযোগ্য নয় — in-tx check দিয়ে enforce করা হয়েছে)
- `tblCustomerReturnDetail`: `PK_tblCustomerReturnDetail` · `tblDCStore`: `PK_tblDCStore` (inline MAX+1 collision → এখন full rollback)
- Schema change করা হয়নি (report only)

## 16-18. Duplicate / concurrency protection layers

**Server-side (primary):**
1. **One-time token** — consume শুধু `scope.Complete()`-এর পরে। Double-click, browser/network retry, slow-response re-click, same-request re-submit: same token → "already submitted", কোনো DB write নেই। ASP.NET-এর per-session request serialization check-then-consume-কে same-session-এ race-free করে। Rollback-এ token বেঁচে থাকে → retry সম্ভব, delivery কখনো permanently blocked হয় না (Test 8)।
2. **UPDLOCK invoice gate** — scope-এর প্রথম statement invoice row-র U-lock নেয়, tx শেষ পর্যন্ত ধরে। ভিন্ন session-এর concurrent request এখানে block হয় (serialize)।
3. **In-tx over-return re-check** — detail insert-এর পরে `HasOverReturn`: কোনো line-এ `SUM(ReturnQty) > DeliveryTotalQuantity` হলে throw → rollback। Grid-এর stale AvailableQty দিয়ে race-এ over-return অসম্ভব (Test 6b: 7+7 vs 10 → ঠিক একটাই commit)।

`SELECT-then-INSERT` race (task section 13-এর unsafe pattern) এভাবে বন্ধ: pre-check নয়, **post-insert re-validation lock-এর ভিতরে**।

**UI (secondary):** Button1 — confirm → disable → 'Saving...' (`UseSubmitBehavior="false"`); postback re-render-এ আবার সক্রিয় (failure-এ retry সম্ভব)।

**Error handling:** Clear + success message scope-এর বাইরে, commit-এর পরে; failure-এ `"Delivery sales return failed: <ex.Message>"` (existing `ShowMessageBox` convention); success message কখনো failure-এর পরে নয়।

## 19-27. Test results

Execution: scratchpad harness (`DSRHarness.exe`) — **real BLL/DAL/dev-DB** দিয়ে `saveButton_Click`-এর হুবহু call sequence (same TransactionScope options, ordering, throws; grid data page-এর নিজস্ব source `LoadInvoiceNew` থেকেই; Dictionary+lock = Session + ASP.NET session-lock stand-in)। প্রতি test-এ before/after snapshot: দুই DB-র মোট count + per-invoice header/detail/stock/posted/voucher। Build: Library.DAL + Library.BLL msbuild OK; পুরো site `aspnet_compiler` precompile **exit 0** ✅

| # | Test | Invoice | Result |
|---|------|---------|--------|
| 1 | Happy path → COMMIT | 1122 | ✅ PASS — header+detail+DCStore(SalesReturn, qty 1)+IsPosting=1+voucher; `DTC=none` |
| 2 | Controlled failure **after stock update** | 1121 | ✅ PASS — snapshot before/after **IDENTICAL**; token বেঁচে আছে |
| 3 | **Real journal failure** — SP-তে সাময়িক `RAISERROR` inject (প্রথম voucher insert-এর পরে; পরে restore, `CLEAN` verified) | 1120 | ✅ PASS — "Journal posting failed" → সব rollback (voucher rows সহ), IDENTICAL |
| 3b | Journal no-op (SP আদৌ চলেনি) → verify false | 1120 | ✅ PASS — rollback IDENTICAL |
| 4 | Boolean false — real journal চলার **পরে** verify-কে false ধরা | 1119 | ✅ PASS — voucher rows tx-এ লেখা হয়েও rollback; **cross-database rollback প্রমাণিত** |
| 5 | Double click (same token ×2 sequential) | 1118 | ✅ PASS — SUCCESS + DUPLICATE; ঠিক ১টা return/detail/stock/voucher set |
| 6a | Concurrent same token (2 thread, session-lock emulated = real ASP.NET semantics) | 1116 | ✅ PASS — 1 SUCCESS + 1 DUPLICATE; ১টাই return set |
| 6a′ | Concurrent same token **session-lock ছাড়া** (framework guarantee বাদ দিলেও কী হয়) | 1126 | DB-level backstop ধরেছে: 1 SUCCESS + 1 over-return block, **exactly 1 return** — duplicate তবু হয়নি |
| 6b | Concurrent ভিন্ন token, same invoice (qty 10, দুই request-ই 7 করে চায়) | 1125 | ✅ PASS — 1 SUCCESS + 1 blocked; returned qty **7, not 14** |
| 7 | Retry after success (consumed token আবার) | 1122 | ✅ PASS — DUPLICATE, snapshot unchanged |
| 8 | Retry after rollback | 1115 | ✅ PASS — fail → IDENTICAL rollback → same token retry → SUCCESS |
| 9 | Multi-item (2 line, deduction 500) failure → সব rollback → তারপর commit | 1124 | ✅ PASS — fail-এ IDENTICAL; commit-এ dtl=2, dc=2, posted=1 |

## 28-29. DB verification + rollback verification

Snapshot fields (প্রতি test-এ before/after): `hdrCount/hdrMax` (tblCustomerReturnMaster), `dtlCount`, `dcCount/dcMax` (tblDCStore), per-invoice `myReturns/myDetails/myDc/myReturnedQty/myPosted/myVouchers` (voucher: ACCDB TableID 2018/2023, CommonPK_ID=CustReturnId), ACCDB `vmCount/vdCount/subCount/refCount`। প্রতিটি rollback test-এ (2, 3, 3b, 4, 8-first, 9-first) **সব field byte-for-byte identical**।

Test শেষে দুই DB-ই COPY_ONLY backup থেকে **restore + verified**: `tblCustomerReturnMaster` আবার 22 rows (test-এর সব return গায়েব), test invoice range-এ 0 return, SP definition original (11601 chars)। Backups এখনো আছে: SQL default backup dir-এ `SalesDisDB_Unicorn_DB_PreDSRTest.bak`, `CSTL_ACCDB_Unicorn_PreDSRTest.bak`।

## 30. Build / regression

- `Library.DAL.csproj` + `Library.BLL.csproj` msbuild — **OK** (শুধু pre-existing warnings)
- পুরো site `aspnet_compiler` precompile — **exit 0** (পরিবর্তিত page compile হয়; অন্য কোনো page-এ নতুন error নেই)
- DAL/BLL-এ শুধু **নতুন** method — কোনো existing method-এর signature/behavior বদলায়নি, তাই অন্য caller-রা unaffected
- Search/Load/Clear flow অপরিবর্তিত

## 31. Production deployment requirements

1. **MSDTC লাগবে না** এই page-এর জন্য (section 13) — তবু sibling page-গুলোর জন্য ইতিমধ্যে MSDTC enable করা থাকলে ক্ষতি নেই।
2. Deploy-এ **Library.DAL.dll + Library.BLL.dll rebuild করে সাইটের Bin-এ দিতে হবে** (নতুন ৩টি method) — শুধু aspx/aspx.cs কপি করলে MissingMethodException হবে।
3. Failure message এখন আসল exception দেখায় — চাইলে log + generic message-এ নামানো যায়।
4. Web farm হলে session state InProc-এর বদলে StateServer/SQL — token আপনিই কাজ করবে।

## 32. Remaining risks / limitations

- **SP-তে duplicate guard নেই** (NOT-EXISTS absent) — অন্য কোনো path একই CustReturnId-তে SP আবার call করলে duplicate voucher হবে; এই page-এর path এখন duplicate-safe, কিন্তু SP নিজে idempotent নয় (SP change এই task-এর বাইরে — report only)।
- **TableID 2018/2023 দুই ভিন্ন return-টেবিলের সাথে shared** (section 10) — voucher traceability ambiguity + অন্য flow-র return-id overlap হলে ভুল join-এর ঝুঁকি; existing design।
- `tblDCStore` inline MAX+1 — ভিন্ন invoice-এর truly-concurrent save-এ PK collision সম্ভব → এখন full rollback + user retry (আগে partial data থাকত), কিন্তু retry-র প্রয়োজন থেকে যায়।
- VoucherCode per-day MAX+1 — concurrent post-এ same code সম্ভব (unique index নেই); existing pattern, সব sibling SP-তেই।
- Browser/UI-level automation হয়নি (login/postback automation নেই) — call-sequence-level replay, sibling page-গুলোর মতোই; session-lock serialization framework guarantee, harness-এ lock দিয়ে represent + lock-ছাড়া backstop-ও আলাদাভাবে প্রমাণিত (6a′)।
- Payment type radio list validate হয় কিন্তু save-এ কোথাও লেখা হয় না (pre-existing behavior, বদলানো হয়নি)।
- Page-এর `WriteLog` hard-coded অন্য user-এর path-এ লেখে (try/catch-এ গেলা) — pre-existing, এই task-এ ছোঁয়া হয়নি; cleanup চাইলে বলবেন।
