# ✅ Test Execution Results — `submitButton_Click` TransactionScope (Sales Return / Return From Market)

Page: `SInventory_UI/SalesReturn.aspx`
Executed: 2026-09-24 · Server: `TOWSIF\MSSQLSERVER2019` (SQL Server 2019) · DBs: `SalesDisDB_Unicorn_DB` + `CSTL_ACCDB_Unicorn` (dev, local)
Sibling reports: [testing-delivery-invoice-transactionscope-results.md](testing-delivery-invoice-transactionscope-results.md), [testing-invoice-transactionscope-results.md](testing-invoice-transactionscope-results.md)

## 1. Scope

`SalesReturn.aspx`-এর active submit event (`submitButton_Click` — `saveButton_Click` পুরোটাই commented-out dead code) delivery-invoice page-এর মতো harden করা হয়েছে: এক TransactionScope, সব critical bool check, journal-verify, duplicate/double-save protection (server-side one-time token + UI disable)। Production DB ছোঁয়া হয়নি; সব test dev DB-তে, শেষে দুই DB-ই backup থেকে restore করা হয়েছে।

## 2-3. Existing flow + exact call chain (inspect করা, অনুমান নয়)

```
SalesReturn.aspx (LinkButton3)
    ↓
submitButton_Click                                        [SalesReturn.aspx.cs]
    ↓
SaveInvoiceNew()
    → BonusCampaignNewDAL.SaveReturnSalesMaster()          [BonusCampaignNewDAL.cs:441]
      → DataAccessManager.SaveDataReturnPrimaryKey("spInsertReturnInvoice_new")
        · INSERT tblReturnInvoice (ReturnInvoiceNo generate, SCOPE_IDENTITY return)
    → ReturnInvoiceBLL.getInvo() → SELECT ReturnInvoiceNo  [ReturnInvoiceDAL.cs:549]
    ↓
SaveReturnAmount(invId)
    → ReturnInvoiceBLL.SaveDataForReturnAmount()
      → INSERT tblReturnAmount                             [ReturnInvoiceDAL.cs:295]
    ↓
SaveInvoiceDetailNew(invId)   — প্রতি grid row:
    → ReturnInvoiceBLL.SaveReturnInvoice(detail)
      · ClsPrimaryKeyFind.PrimaryKeyMax(MAX+1) → INSERT tblReturnInvoiceDetail
    → ReturnInvoiceBLL.InsertDCStoreStockSalesReturn()
      · INSERT tblDCStore (Trans_Type='Receive', StockCondition='Non-Transcational Sales Return',
        DCStoreId = MAX+1 inline, ChalanNo = ReturnInvoiceNo)                 ← stock update
    ↓
ReturnInvoiceBLL.PostSalesReturn(invId)
    → EXEC dbo.sp_SalesReturnPosting                       [ReturnInvoiceDAL.cs:367]
      · CSTL_ACCDB_Unicorn: tblDebitCreditVoucherMaster/Detail (AR reversal 2018 + COGS reversal 2023),
        tblSubsidiaryInfo, tblReferenceInfo · SalesDisDB: tblReturnInvoice.IsPosting=1   ← journal
    ↓
Success (Clear + message)
```

## 4. Changed files

| File | Change |
|---|---|
| `Solution.Web/SInventory_UI/SalesReturn.aspx.cs` | `using System.Transactions;` · `submitButton_Click` rewritten (token gate + TransactionScope + bool checks + journal verify) · `IssueSaveToken()` new · `SaveReturnAmount` void→bool · `SaveInvoiceDetailNew` hardened (detailid≤0/stock-false/empty-grid → throw/false) · `SaveInvoiceNew`-এর getInvo silent-catch → throw |
| `Solution.Web/SInventory_UI/SalesReturn.aspx` | Submit LinkButton-এ `OnClientClick` double-click guard (disable + "Saving...") |
| `Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs` | নতুন method `IsSalesReturnPosted` (additive; কিছু refactor হয়নি) |
| `Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs` | `IsSalesReturnPosted` passthrough |

Shared helper (`DataAccessManager`, `SInventorySql`, `ClsCommonInternalDAL`), Web.config, DB schema — **কিছুই বদলানো হয়নি**। Build: Library.DAL + Library.BLL msbuild OK, পুরো site `aspnet_compiler` precompile exit 0 ✅

## 5. TransactionScope implementation

```
token gate (consumed? → "already submitted", DB untouched)
   ↓
using (TransactionScope(Required, ReadCommitted, 5min))
{
    invId = SaveInvoiceNew();            if (invId <= 0) throw;
    if (!SaveReturnAmount(invId))        throw "Return amount save failed";
    if (!SaveInvoiceDetailNew(invId))    throw "Return invoice detail save failed";
    aInvoiceBll.PostSalesReturn(invId);                       // bool অবিশ্বস্ত (section 7)
    if (!aInvoiceBll.IsSalesReturnPosted(invId)) throw "Journal posting failed";
    scope.Complete();
}
   ↓
token consume + নতুন token issue → Clear(keepInvoiceNo) → success message   // সব scope-এর বাইরে
catch(Exception ex) → showMessageBox("Sales return failed: " + ex.Message)  // token থেকে যায় → retry allowed
```

## 6. Critical boolean return checks

| Method | আগে | এখন |
|---|---|---|
| `SaveReturnSalesMaster` (header) | pk unchecked | `invId <= 0 → throw` |
| `getInvo` (ReturnInvoiceNo) | empty catch, silent skip | rows==0 → throw |
| `SaveDataForReturnAmount` | `bool status` ignored | false → throw |
| `SaveReturnInvoice` (detail) | `detailid` unchecked (0=fail) | ≤0 → throw (product code সহ) |
| `InsertDCStoreStockSalesReturn` (stock) | bool ignored; comUnitId=0 হলে silently skip | false → throw; retQty>0 কিন্তু DC unresolved → throw |
| empty grid | header-only return তৈরি হতো | 0 detail saved → false → throw |
| `PostSalesReturn` (journal) | bool ignored | নিচে দেখো — bool অবিশ্বস্ত, তাই `IsSalesReturnPosted` verify |

## 7. Journal posting flow + কেন verify-query

```
page → ReturnInvoiceBLL.PostSalesReturn → ReturnInvoiceDAL.PostSalesReturn
     → SInventorySql.Execute("EXEC dbo.sp_SalesReturnPosting …")
     → ClsCommonInternalDAL.UpdateDataByUpdateCommandNew (EntLib, catch নেই → SQL error propagate ✅)
     → return ExecuteNonQuery(...) > 0
```

**প্রমাণিত (Test 1):** SP-র `SET NOCOUNT ON`-এর কারণে `ExecuteNonQuery` সফল হলেও **-1** দেয় → `PostSalesReturn` **সবসময় false** (`postRaw=False` on success)। তাই `if(!PostSalesReturn) throw` করলে প্রতিটা save fail করত। Fix: SP call-এর পরে নতুন `IsSalesReturnPosted(invId)` — SP-র নিজের guard-এর হুবহু আয়না: `IsPosting=1` এবং (return amount>0 হলে) ACCDB-তে AR-reversal voucher (TableID 2018) exists। Exception / no-op / 0-row — সব failure এতে ধরা পড়ে।

## 8. Stored procedure analysis (dev DB থেকে dump করা)

**`spInsertReturnInvoice_new`** — writes: `tblReturnInvoice` INSERT only (SalesDisDB)। `BEGIN TRAN`/`COMMIT`/`ROLLBACK`/`TRY-CATCH`/`XACT_ABORT` **নেই** → ambient TransactionScope-এ enlist, rollback-এ সব যায়। Return: `SELECT SCOPE_IDENTITY()`। ReturnInvoiceNo = `'RIN--'+yyMMdd+(same-date MAX(ReturnInvoiceId)+1001)` — MAX-based, race-প্রবণ (section 29)।

**`sp_SalesReturnPosting`** — nested transaction **নেই**, TRY/CATCH নেই, `SET NOCOUNT ON`। Writes:
- `CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster` — AR reversal (TableID 2018, IsValuation 0, @ReturnNetAmount>0 হলে) + COGS reversal (TableID 2023, IsValuation 1, @ReturnCostAmount>0 হলে); দুটোরই NOT-EXISTS duplicate guard (TableID+CommonPK_ID)
- `CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail` — 2018: Dr Sales Return (GL 4170 fallback), Cr Deduction Income (5968, deduction>0 হলে), Cr AR (2112); 2023: Dr FG Inventory (4156), Cr COGS (4162); GL id `CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity` (PageId 23/163) থেকে
- `tblSubsidiaryInfo` / `tblReferenceInfo` — না থাকলে insert
- `SalesDisDB.dbo.tblReturnInvoice` — `IsPosting=1` (সবশেষে, সবসময়)
- Invoice না পেলে early `RETURN` (কিছুই লেখে না)

## 9. Tables affected (save flow)

`tblReturnInvoice` (header, IDENTITY PK) · `tblReturnInvoiceDetail` (PK MAX+1 via ClsPrimaryKeyFind) · `tblReturnAmount` · `tblDCStore` (stock receive, PK MAX+1 inline) · ACCDB: `tblDebitCreditVoucherMaster`, `tblDebitCreditVoucherDetail`, `tblSubsidiaryInfo`, `tblReferenceInfo`

## 10. Connection strings

| Path | Source | String |
|---|---|---|
| `SInventorySql.Execute` (detail, stock, amount, journal SP) | EntLib 4.1 ← Web.config `SolutionConnectionStringSSIDB` | `Data Source=TOWSIF\MSSQLSERVER2019;Initial Catalog=SalesDisDB_Unicorn_DB;User ID=sa;…` |
| Header save (`DataAccessManager.SaveDataReturnPrimaryKey`) + সব `GetDataTable` (getInvo, IsSalesReturnPosted) | `DataAccessManager.ConnectionString()` ← `SqlUserAccess` statics | একই server/DB কিন্তু **`Encrypt=True;TrustServerCertificate=True` সহ → আলাদা string → আলাদা pool** |

## 11. MSDTC promotion status

**Promotion ঘটে (Distributed)** — Test 1/3b/4A/5/6/8/9-এ scope-এর ভিতরে `Transaction.Current.TransactionInformation.DistributedIdentifier` non-empty GUID observed (যেমন T1: `c24461b4-…`)। Root cause delivery page-এর মতোই: header save-ই প্রথম operation এবং সেটা দ্বিতীয় pool (DataAccessManager) থেকে যায়, বাকি সব EntLib pool থেকে → দ্বিতীয় connection ঢোকামাত্র MSDTC promote। Instruction অনুযায়ী shared helper **modify করা হয়নি** — শুধু report (production requirement: section 28)।

## 12. Duplicate / idempotency business key

এই page **"Return From Market"** — কোনো source order/invoice reference নেই (`OrderId=0`, `OrderNo='No order'`), তাই delivery page-এর মতো natural business-key gate (invoice row + UPDLOCK) সম্ভব না; একই customer একই দিনে একই পণ্য বৈধভাবে দুবার ফেরত দিতে পারে। Schema+business logic inspect করে সিদ্ধান্ত:

```
IDEMPOTENCY / BUSINESS KEY:
Per-form one-time save token (GUID) — প্রতিটি rendered form-এর একক submission identity।
Session-এ "SRTOKEN_<guid>" হিসেবে issue হয় (Page_Load !IsPostBack), ViewState-এ round-trip,
শুধুমাত্র সফল COMMIT-এর পরে consume। Natural DB business key: নেই (unique constraint: NO)।
```

## 13. Existing unique constraints/indexes

- `tblReturnInvoice`: শুধু `PK_tblReturnInvoice (ReturnInvoiceId, IDENTITY)` — **ReturnInvoiceNo বা কোনো business combination-এ unique index নেই → NO (report only, schema change করা হয়নি)**
- `tblReturnInvoiceDetail`: `PK_tblReturnInvoiceDetail (ReturnInvoiceDetailId)` — MAX+1 collision হলে PK violation → এখন scope-এ পুরো rollback (আগে partial থেকে যেত)
- `tblDCStore`: `PK_tblDCStore (DCStoreId)` — একই সুরক্ষা
- `tblReturnAmount`: `PK_tblReturnAmount (ReturnAmountId)`

## 14-16. Duplicate protection layers

**Server-side (primary):** one-time token — consume-only-after-commit।
```
Request → token consumed? ── YES → "already submitted" (কোনো DB write নেই)
                          └─ NO  → TransactionScope → সব save → COMMIT → token consume
```
- Double-click/browser retry/network retry/slow-response re-click: same token re-post → blocked
- **Race-safety:** একই session-এর concurrent requests ASP.NET session state lock-এ serialize হয় (page-এ session RW), তাই SELECT-then-INSERT-জাতীয় race এখানে নেই; token-check + consume কার্যত atomic। ভিন্ন session/user = ভিন্ন token = ব্যবসায়িকভাবে ভিন্ন return (source doc নেই বলে এটাই সঠিক boundary)।
- Rollback interaction (section 15 of task): token consume হয় **শুধু** `scope.Complete()`-এর পরে — failed transaction-এ token বেঁচে থাকে → retry allowed (Test 8 প্রমাণ); success-এর পরে consumed → re-post blocked (Test 5/7 প্রমাণ)। "Permanently processed after rollback" অসম্ভব।

**UI (secondary):** LinkButton `OnClientClick` — প্রথম click-এ disable + "Saving..." spinner; postback response-এ button স্বাভাবিকভাবে re-render (failure-এ আবার active)।

**Error handling:** Clear + success message scope-এর বাইরে, commit-এর পরে; failure-এ `"Sales return failed: <ex.Message>"` (existing `showMessageBox`/`showSuccessMessageBox` convention preserved), success message failure-এর আগে কখনো নয়।

## 17-25. Test results

Execution: scratchpad harness (`SRHarness.exe`) — **real BLL/DAL/dev-DB** দিয়ে `submitButton_Click`-এর হুবহু call sequence (same TransactionScope options, same ordering, same throw condition, token-gate-এর একই semantics; Dictionary+lock = Session+ASP.NET session-lock stand-in)। প্রতি test-এ before/after full snapshot (Suppress scope-এ): দুই DB-র ৮ টেবিলের count/MAX + per-invoice rows। Test data: customer 966, DC 3 (BD01), products 0112650/0112680।

| # | Test | Result |
|---|------|--------|
| 1 | Happy path → COMMIT | ✅ PASS — header/detail/amount/stock +1, IsPosting=1, AR+COGS voucher, `postRaw=False` প্রমাণ, DTC id logged |
| 2 | Controlled failure **after stock update** | ✅ PASS — in-tx এ dtl=1, dcStore=1, amt=1 দৃশ্যমান ছিল; after == before **byte-identical** |
| 3 | **Real journal failure** — `sp_SalesReturnPosting`-এ সাময়িক `RAISERROR` inject (পরে restore, body-match verified) | ✅ PASS — SQL error propagate → সব rollback, snapshot identical |
| 3b | Journal no-op (SP আদৌ চলেনি) → `IsSalesReturnPosted=false` → throw | ✅ PASS — rollback identical |
| 4 | Boolean false — (A) journal-verify false treatment: real voucher rows tx-এ লেখা হয়ে গিয়েছিল, তবু rollback; (B) detail save false | ✅ PASS ×2 — উভয়েই identical snapshot; **cross-database rollback প্রমাণিত** |
| 5 | Double click (same token ×2, sequential) | ✅ PASS — 1 SUCCESS + 1 DUPLICATE; DB-তে ঠিক ১টা return/detail/stock/amount, ১ set voucher |
| 6 | Concurrent double request (same token, 2 thread) | ✅ PASS — SUCCESS=1, DUPLICATE=1; ঠিক ১টা return |
| 7 | Retry after success (তৃতীয়বার same token) | ✅ PASS — DUPLICATE, কোনো নতুন row নেই (snapshot unchanged) |
| 8 | Retry after rollback | ✅ PASS — fail → identical rollback → same token retry → SUCCESS |
| 9 | Multi-item (2 product, 2 batch, ভিন্ন qty) + failure | ✅ PASS — in-tx dtl=2 dcStore=2 → সব rollback identical; তারপর same token-এ 2-item commit OK |

## 26-27. DB verification + rollback verification

প্রতি test-এ snapshot fields: `hdrCount/hdrMax` (tblReturnInvoice), `dtlCount`, `amtCount`, `dcCount/dcMax` (tblDCStore), per-invoice `myDtl/myAmt/myDc/myPosted/myVouchers`, ACCDB `vmCount/vdCount/subCount/refCount`। প্রতিটি rollback test-এ (2, 3, 3b, 4A, 4B, 8-first, 9-first) before/after **সব field identical**। শেষে দুই DB-ই COPY_ONLY backup থেকে restore + verified: `tblReturnInvoice` আবার 11 rows, `sp_SalesReturnPosting` definition 13991 chars (original), pre-existing orphan voucher-রাও অক্ষত। Backups: SQL default backup dir-এ `SalesDisDB_Unicorn_DB_PreSalesReturnTxTest.bak`, `CSTL_ACCDB_Unicorn_PreSalesReturnTxTest.bak`।

**Dev-data artifact (test চলাকালীন পাওয়া):** ACCDB-তে আগের testing-এর **orphan AR voucher (TableID 2018, CommonPK_ID 13–22)** ছিল যাদের কোনো tblReturnInvoice row নেই — নতুন return ওই id পেলে SP-র NOT-EXISTS guard AR voucher **skip** করে (T1-এ ধরা পড়ে)। Test-এর জন্য orphan-গুলো temporarily delete করা হয়েছিল (restore-এ ফেরত এসেছে)। Production-এ এমন mismatch থাকলে একই silent-skip হবে — section 29 দেখো।

## 28. Production deployment requirements

1. **MSDTC লাগবে** — promotion প্রতি save-এই ঘটে। Production web+DB server-এ MSDTC Running + (আলাদা machine হলে দুই পাশে) Network DTC Access enabled, নাহলে প্রথম save-এই "MSDTC unavailable"। স্থায়ী বিকল্প: `DataAccessManager.ConnectionString()`-কে config string-এর সাথে এক করা (আগের report-এর fix; সব page ছোঁয় বলে আলাদা regression pass দরকার)।
2. Failure message এখন আসল exception দেখায় — চাইলে log+generic করা যায়।
3. Web farm / session state: token Session-ভিত্তিক — InProc-এর বদলে StateServer/SQL session হলে token আপনিই কাজ করবে; sessionless page করা যাবে না।

## 29. Remaining risks / limitations

- **ReturnInvoiceNo unique নয় + MAX-based generation** — দুই ভিন্ন session-এর truly-concurrent commit-এ একই নম্বর সম্ভব (id ভিন্ন হবে)। Unique index চাইলে schema change লাগবে — এই task-এ করা হয়নি (report only)।
- Cross-session duplicate (দুই user/browser-এ same customer-এর same return) token আটকায় না — source document নেই বলে কোনো server-side natural key-ও নেই; এটা business boundary।
- ACCDB orphan voucher (CommonPK_ID overlap) থাকলে SP AR voucher silently skip করে এবং `IsSalesReturnPosted` সেটাকে posted ধরবে (SP-র নিজের guard-এর সাথে সঙ্গতিপূর্ণ, কিন্তু ভুল amount-এর পুরনো voucher-ই খাতায় থাকে)। Dev DB-তে 13–22 range-এ এখনো orphan আছে (restore-এ ফেরত এসেছে) — চাইলে আলাদা cleanup task।
- Browser/UI-level automation হয়নি (login/postback automation নেই) — call-sequence-level replay, delivery page-এর মতোই; token gate-এর ASP.NET session-lock serialization framework guarantee, harness-এ lock দিয়ে represent করা হয়েছে।
- `ClsPrimaryKeyFind.PrimaryKeyMax` (detail) ও tblDCStore-র inline MAX+1 concurrent ভিন্ন-return save-এ PK collision ঘটাতে পারে — এখন পুরো tx rollback হয়ে user retry করতে পারবে (আগে partial data থাকত), কিন্তু retry-র প্রয়োজনটা থেকে যায়।
