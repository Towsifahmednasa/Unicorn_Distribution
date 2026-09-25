# ✅ Test Execution Results — `saveButton_Click` TransactionScope (Delivery Confirmation)

Page: `DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx`
Executed: 2026-09-24 (TransactionScope) + 2026-09-24 (Duplicate Prevention, section নিচে) · Server: `TOWSIF\MSSQLSERVER2019` (SQL Server 2019 RTM 15.0.2000.5) · DBs: `SalesDisDB_Unicorn_DB` + `CSTL_ACCDB_Unicorn` (dev, local)
Sibling plan/results: [testing-invoice-transactionscope.md](testing-invoice-transactionscope.md), [testing-invoice-transactionscope-results.md](testing-invoice-transactionscope-results.md)

## 1. Changed file + method

Changed files (duplicate-prevention addition সহ):

| File | Change |
|---|---|
| `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx.cs` | `saveButton_Click`: TransactionScope + duplicate gate; `SaveInvoiceDetail`: `ReturnToDCStore` return check; `using System.Transactions;` |
| `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx` | `Button1`: double-click UI protection (`UseSubmitBehavior="false"`, confirm → disable → 'Saving...') |
| `Library.DAL/SInventory_DAL/InvoiceDAL.cs` | **additive**: `IsDeliveryAlreadyConfirmed(int)` — UPDLOCK gate query |
| `Library.BLL/SInventory_BLL/InvoiceBLL.cs` | **additive**: `IsDeliveryAlreadyConfirmed(int)` passthrough |

DAL/BLL-এ শুধু নতুন method যোগ — কোনো existing method/behavior বদলায়নি, অন্য page unaffected। `System.Transactions` Web.config-এ আগে থেকেই ছিল (line 65)।

## 2. What changed

`saveButton_Click`-এ duplicate-check-এর পরে পুরো save flow এখন এক `TransactionScope`-এ (ReadCommitted, 5 min timeout):

```
using (scope = TransactionScope(Required, ReadCommitted, 5min))
{
    SaveInvoice();                                  // tblInvoice: Delivery* + Payment* UPDATE
    if (!SaveInvoiceDetail(invId))
        throw "Invoice detail save failed";         // tblInvoiceDetail x2 + tblDCStore return rows
    if (!PostDeliveryConfPartialJournal(invoiceId)
        && grandTotal > 0)
        throw "Journal posting failed";             // accounting voucher SP
    scope.Complete();
}
Clear(); + success alert                            // scope-এর বাইরে
```

- `catch (Exception)` message বদলেছে: আগের generic "Some Networks Error…" → **`Delivery confirmation failed: <ex.Message>`** (আসল কারণ আর লুকায় না)।
- `SaveInvoiceDetail`-এর ভিতরে: `ReturnToDCStore(...)` false দিলে (0 row insert = stock ফেরত হয়নি) এখন `throw` → rollback। আগে silently ignore হতো।
- **Journal-false conditional (ইচ্ছাকৃত):** `PostDeliveryConfPartialJournal` false দিলে **শুধু delivered grand total > 0 হলে** throw। কারণ: SP-র body-তে `@DeliveryNetAmount <= 0` হলে designed early-`RETURN` আছে (all-Reject delivery = বৈধ no-op, 0 rows affected → DAL false return করে)। Blind throw করলে all-Reject confirmation চিরতরে fail করত। এটা business logic change না — শুধু SP-র designed skip-কে failure হিসেবে গোনা থেকে বাঁচানো।

## 3. Journal call chain

```
page → InvoiceBLL.PostDeliveryConfPartialJournal(invoiceId)     [InvoiceBLL.cs:711]
     → InvoiceDAL.PostDeliveryConfPartialJournal                 [InvoiceDAL.cs:833]
     → ClsCommonInternalDAL.UpdateAction("sp_Sales_DeliverySalesPosting", @InvoiceId)  [ClsCommonInternalDAL.cs:450]
     → ExecuteNonQueryAction: EntLib Database("SolutionConnectionStringSSIDB"), ExecuteNonQuery
        · rows > 0 → true
        · rows = 0 → false
        · SQL exception → UpdateAction-এর catch গিলে ফেলে → false   ← এই জন্যই return-check জরুরি
```

## 4. Journal SP details (`sp_Sales_DeliverySalesPosting`, body dev DB থেকে dump করা)

- **নিজের কোনো `BEGIN TRAN`/`COMMIT`/`ROLLBACK` নেই** → ambient TransactionScope-এ enlist হয়, page-এর rollback-এ SP-র সব লেখাও rollback হয় (Test 2-তে প্রমাণিত)।
- Writes:
  - `CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster` — sales voucher (JVP-YYMMDDxxxxx, VoucherType='Journal', ActionStatus='Approved', TableID=2014, CommonPK_ID=InvoiceId)
  - `CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherDetail` — Dr: Accounts Receivable (total), Cr: Sales Revenue (DeliveryNetAmount), Cr: Delivery Charge Income (থাকলে)
  - `CSTL_ACCDB_Unicorn.dbo.tblSubsidiaryInfo` / `tblReferenceInfo` — না থাকলে insert
  - `SalesDisDB_Unicorn_DB.dbo.tblInvoice` — `IsPosting=1`
- GL id গুলো `CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity` (PageId=20) থেকে, fallback hard-coded (4/14/5968)।
- Guards: `@DeliveryNetAmount <= 0` → early RETURN (0 rows → **false**); voucher আগে থেকে থাকলে (duplicate) main block skip।
- শেষে `sp_Sales_DeliveryCOGSPosting` call — **নিজস্ব TRY/CATCH-এ wrapped** (COGS fail করলেও sales posting fail হয় না)। COGS SP-ও একই ACCDB টেবিলে লেখে, `BEGIN TRAN` নেই।

## 5. Connection strings used (save flow-এ)

| Path | Connection source | String |
|---|---|---|
| `SInventorySql.Execute` (UpdateInvoice, UpdateInvoiceDetail, ReturnToDCStore…) | EntLib ← Web.config `SolutionConnectionStringSSIDB` | `Data Source=TOWSIF\MSSQLSERVER2019;Initial Catalog=SalesDisDB_Unicorn_DB;User ID=sa;…` |
| `UpdateAction` (journal SP) | EntLib ← একই config string | (একই) |
| `SInventorySql.GetDataTable` (`DCInfoWithDCId`) | `DataAccessManager.ConnectionString()` ← `SqlUserAccess` statics, নিজে বানানো | একই server/DB কিন্তু **`Encrypt=True;TrustServerCertificate=True` সহ — আলাদা string → আলাদা pool** |

## 6. MSDTC promotion status

**Promotion ঘটে (Distributed=True)** — Test 1, 2, 4-এ `Transaction.Current.TransactionInformation.DistributedIdentifier != Guid.Empty` observed। Root cause আগের report-এর মতোই: `DCInfoWithDCId`-এর `GetDataTable` দ্বিতীয় connection pool থেকে connection আনে, দ্বিতীয় pool scope-এ ঢোকামাত্র MSDTC-তে promote। Cross-database (ACCDB) write একই connection দিয়ে হয়, ওটা promotion-এর কারণ না।

Instruction অনুযায়ী shared `DataAccessManager` **modify করা হয়নি** — শুধু report। Fix হলে (দুই string এক করা) promotion যাওয়ার কথা; আগের report-এর Recommended fix এখানেও প্রযোজ্য।

## 7-10. Test results

Execution method: ASPX browser-এ চালানো হয়নি; scratchpad harness (`DelivTxHarness.exe`) **real BLL/DAL/DB** দিয়ে `saveButton_Click`-এর exact call sequence replay করেছে — same TransactionScope, same conditional, same throw গুলো। Grid-এর UI value গুলো DB-র `tblInvoiceDetail` থেকে নেওয়া (grid ওখান থেকেই bind হয়)। Site precompile (`aspnet_compiler`) exit 0 — পরিবর্তিত page compile হয় ✅

| # | Test | Invoice | Result |
|---|------|---------|--------|
| 1 | Happy path (full delivery, commit) | 1099 (1 line, qty 2, ৳2200) | ✅ PASS |
| 2 | Failure **after** journal → full rollback | 1098 (1 line, qty 10, ৳8000) | ✅ PASS |
| 3 | Partial delivery (6/10) → DeliveryReturn row → rollback | 1098 | ✅ PASS |
| 4 | Journal false → throw → rollback (journal rows সহ) | 1098 | ✅ PASS |

### Test 1 — Happy path
- `tblInvoice`: `DelivaryInvoiceNo='DEL-INV-BD010000001099'`, `PaymentInvoiceNo='RTN-…'`, `DeliveryInvoiceStatus='Full'`, `DeliveryTpGrandTotal=2200.00`, `IsPosting=1` ✅
- `tblInvoiceDetail` (1640): DeliveryQuantity=2, DeliveryStatus=Full, DeliveryNetAmount=2200, Payment* একই ✅
- `tblDCStore`: unchanged (full delivery-তে return row হয় না — expected) ✅
- ACCDB: voucher master **+2** (sales + COGS), voucher detail **+4**, reference +1 ✅
- `journalOk=True`

### Test 2 — Failure after journal
- Journal চলার পরে tx-এর ভিতরে ২টা voucher master দৃশ্যমান ছিল (`inTx voucherMaster rows=2`), তারপর throw।
- Before/after snapshot **IDENTICAL**: tblInvoice (Delivery*/Payment*/IsPosting), tblInvoiceDetail, tblDCStore (count+MAX), ACCDB voucher master/detail/subsidiary/reference — সব rollback ✅ **Cross-database rollback কাজ করে।**

### Test 3 — Partial delivery
- dQty=6/10 → `ReturnToDCStore(4, …)` চলেছে; tx-এর ভিতরে `StockCondition='DeliveryReturn', StockQty=4, ReturnFromInvoiceDetailId=1639` row **পাওয়া গেছে** (`returnRowSeenInTx=True`), তারপর controlled throw।
- After: `dcCount`/`dcMax` unchanged, `dcReturn=0` — DeliveryReturn row rollback ✅

### Test 4 — Journal returns false
- Real journal চালিয়ে (voucher rows tx-এ insert হয়ে) result-কে false ধরে page-এর exact conditional চালানো হয়েছে → `exception=Journal posting failed`, `scope.Complete()` চলেনি।
- Before/after **IDENTICAL** — invoice/detail/DCStore/journal সব rollback ✅

## 11-12. Verification method + rollback

প্রতি test-এ before/after snapshot (scope-এর বাইরে, Suppress-এ): target invoice-এর Delivery*/Payment*/IsPosting column, per-detail Delivery*/Payment*, `tblDCStore` COUNT+MAX+ReturnFromInvoiceDetailId match, ACCDB voucher master (TableID=2014, CommonPK_ID=invoiceId) + detail + total counts + subsidiary/reference counts। Rollback test-গুলোতে প্রতিটি key **byte-for-byte identical**।

Test শেষে দুই DB-ই COPY_ONLY backup থেকে **restore করা হয়েছে** — verified: invoice 1099-এর DelivaryInvoiceNo আবার NULL, voucher 0, SP definition অক্ষত (9339 chars)। Backups এখনো আছে: `…\MSSQL\Backup\SalesDisDB_Unicorn_DB_PreDelivTxTest.bak`, `CSTL_ACCDB_Unicorn_PreDelivTxTest.bak`।

## 13. Limitations / blockers

- **SP-কে সত্যিকারের mid-flight RAISERROR করানো যায়নি** — `ALTER PROCEDURE` sandbox-এ blocked। Test 4 তাই "false path" simulate করেছে: real journal insert + false treatment। `UpdateAction`-এ SP-error আর 0-rows দুটোই একই `false` হয়ে ফেরে, তাই page-এর দিক থেকে path অভিন্ন; আর SQL error-এ ambient tx এমনিতেই doomed হয়। তবু চাইলে SSMS-এ SP-তে সাময়িক `RAISERROR` বসিয়ে UI থেকে এক বার manual verify করা যায়।
- Browser/UI-level test হয়নি (login/postback automation নেই) — call sequence-level replay, আগের page-এর মতোই।
- Multi-line invoice test হয়নি (eligible invoice গুলো সব 1-line); per-line loop logic single-line-এই exercised।
- Timeout boundary test এবার আলাদাভাবে চালানো হয়নি — আগের page-এর Test 5-এ একই pattern-এ প্রমাণিত।

## Duplicate / Double Save Prevention

### Existing mechanism (inspection findings)

| Layer | Existing | যথেষ্ট? |
|---|---|---|
| Pre-check | `saveButton_Click` শুরুতে `sp_GET_DelivaryInvoiceNoCheckById` — `SELECT … WHERE InvoiceId=@id AND ISNULL(DelivaryInvoiceNo,'')<>''` → "Already Exist!" | ❌ check-then-act, tx-এর বাইরে — concurrent দুই request দুটোই empty দেখে দুটোই save করত |
| Journal SP | `IF NOT EXISTS (voucher WHERE TableID=2014 AND CommonPK_ID=@InvoiceId AND IsInventoryValuation=0)` — নিজস্ব guard; COGS SP-তেও একই ধাঁচের guard (`IsInventoryValuation=1`) | ✅ voucher duplicate আটকায়, কিন্তু `tblDCStore` return row বা Delivery* overwrite আটকায় না |
| DB constraint | `DelivaryInvoiceNo`-তে **কোনো unique index নেই** (শুধু non-unique covering index); `UQ_tblIssnvoice_InvoiceNo` আছে কিন্তু delivery হলো existing row-র UPDATE, নতুন insert না — unique constraint এখানে ভুল tool | — |
| UI | কিছুই ছিল না (দুটো button-ই শুধু confirm) | ❌ |

Duplicate-এর আসল ক্ষতি ছিল: Partial/Reject-এ `tblDCStore`-এ **DeliveryReturn row দুইবার insert** (stock দুইবার ফেরত) + Delivery*/Payment* overwrite।

### Identified business key (invent করা হয়নি, schema থেকে)

**`tblInvoice.InvoiceId` + processed marker = `DelivaryInvoiceNo`** (NULL/'' = unprocessed, `DEL-…` = processed)। Marker টা `UpdateInvoice` transaction-এর **ভিতরে** লেখা হয় → rollback হলে marker-ও rollback (Test 8-এ প্রমাণিত) — idempotency state কখনো ভুলভাবে committed হয় না।

### Server-side protection (নতুন)

`saveButton_Click`-এ scope-এর প্রথম statement — race-safe atomic gate:

```sql
SELECT ISNULL(DelivaryInvoiceNo,'') FROM tblInvoice WITH (UPDLOCK, ROWLOCK) WHERE InvoiceId=@InvoiceId
```

(`InvoiceDAL.IsDeliveryAlreadyConfirmed` → `InvoiceBLL` → page)। কেন এটা:
- **UPDLOCK** transaction শেষ পর্যন্ত ধরে রাখে → concurrent Request B এই SELECT-এই **block** হয়, A commit করলে B committed `DEL-…` দেখে → "This delivery has already been processed." দেখিয়ে **কিছু না লিখে** বেরিয়ে যায় (scope Complete হয় না)। A rollback করলে B স্বাভাবিকভাবে এগোয়।
- `Serializable`/app-lock/নতুন schema লাগেনি — এক row-র U-lock, দুই তরফই একই row আগে নেয় বলে deadlock-ordering সমস্যাও নেই।
- Existing "Already Exist!" pre-check (tx-এর বাইরে, fast path) অপরিবর্তিত।

### UI protection (secondary)

`Button1`: `UseSubmitBehavior="false"` + `OnClientClick`-এ confirm → `this.disabled=true; this.value='Saving...'`। Async postback শেষে UpdatePanel re-render-এ button আবার enabled (failure-এর পরে retry সম্ভব)। `submitButton` (LinkButton, sweetAlert confirm flow) অপরিবর্তিত — ওর ভরসা server gate। UI এখানে convenience only; আসল protection server/DB-তে।

### Test results (Tests 5–8) — harness, real BLL/DAL/DB, gate সহ replay

| # | Test | Invoice | Result |
|---|------|---------|--------|
| 5 | Double click (পরপর দুই save) | 1093 | ✅ PASS — ১ম committed, ২য় `duplicate` (gate-এ থেমেছে), ২য়-র পরে state byte-for-byte unchanged |
| 6 | Concurrent double request (2 thread, একই মুহূর্তে, partial 6/10) | 1098 | ✅ PASS — **exactly 1 committed, 1 duplicate**; `DeliveryReturn` row = 1টা, voucher set = 1টা (master 2: sales+COGS, detail 4) |
| 7 | Retry after success | 1099 | ✅ PASS — commit-এর পরে আবার save → `RESULT: DUPLICATE`, নতুন কিছু লেখা হয়নি |
| 8 | Retry after rollback | 996 (nonzero) + 1087 (zero-amount) | ✅ PASS — attempt 1: journal post-এর পরে throw → সব rollback **marker সহ** (`DelivaryInvoiceNo` আবার NULL); attempt 2: সফল commit |

Regression: Tests 1–4 gate-সহ আবার চালানো হয়েছে — সব ✅ PASS (T2/T4: 996, T3: 1098, T1: 1099)।

Bonus finding (1087): UnitPrice=0 invoice-এ journal SP designed-ভাবে skip করে false দেয় — conditional throw-এর কারণে save ঠিকই commit হয়েছে; blind `throw` হলে এই invoice কখনোই confirm করা যেত না।

### Database verification (প্রতি test-এ before/after)

Business-key ভিত্তিক snapshot: `tblInvoice` (DelivaryInvoiceNo, DeliveryInvoiceStatus, DeliveryTpGrandTotal, PaymentInvoiceNo, IsPosting) `WHERE InvoiceId=@id`; `tblInvoiceDetail` Delivery*/Payment* per line; `tblDCStore` COUNT+MAX + `COUNT(*) WHERE ReturnFromInvoiceDetailId IN (details of @id)`; ACCDB `tblDebitCreditVoucherMaster WHERE TableID=2014 AND CommonPK_ID=@id` + joined detail + total counts। T6-এর after: `dcReturn=1`, `vMaster=2`, `vDetail=4` — একটাই set।

### Remaining race-condition risk

- Gate-এ blocked Request B-র wait A-র transaction duration পর্যন্ত (worst case command/scope timeout → user error দেখে retry করবে, তখন duplicate message পাবে) — duplicate তৈরি হয় না, শুধু দেরি।
- Gate-টা **এই page-এর** save path-এ। অন্য কোনো page/SP যদি `DelivaryInvoiceNo` bypass করে একই invoice-এর delivery data লেখে, সেটা এই gate-এর বাইরে (PAGE-214 sibling flow-তে একই gate বসানো যায়, চাইলে বলবেন)।
- `sp_DeleteDeliveryInvoice` (reversal) marker মুছে দিলে invoice আবার confirmable হবে — এটা design অনুযায়ীই।

## 14. Production deployment requirements

1. **MSDTC লাগবে** (promotion ঘটে): production web+DB server-এ MSDTC service Running + (আলাদা machine হলে দুই পাশেই) Network DTC Access enabled। নাহলে প্রথম save-এই "MSDTC on server … is unavailable"। বিকল্প: deploy-এর আগে `DataAccessManager.ConnectionString()`-কে config string-এর সাথে এক করা (আগের report-এর fix) — তাহলে promotion থাকার কথা না; ওই fix সব page-কে ছোঁয় বলে আলাদা regression pass দরকার।
2. Failure message এখন আসল exception দেখায় — production-এ DB error text user দেখবে; চাইলে log+generic-এ নামানো যায়।
3. All-Reject delivery flow অপরিবর্তিত (journal skip হলে fail হয় না) — conditional-টার কারণ section 2-এ; zero-amount invoice-এও একই path (1087-এ live verified)।
4. Deploy-এ **Library.DAL.dll + Library.BLL.dll rebuild করে সাইটের Bin-এ দিতে হবে** (নতুন `IsDeliveryAlreadyConfirmed` method) — শুধু aspx/aspx.cs কপি করলে MissingMethod error হবে।
