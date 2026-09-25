# ✅ Test Execution Results — Customer Payment TransactionScope

Page: `SInventory_UI/CustomerPayment.aspx`
Executed: 2026-09-24 · Server: `TOWSIF\MSSQLSERVER2019` (SQL Server 2019) · DBs: `SalesDisDB_Unicorn_DB` + `CSTL_ACCDB_Unicorn` (dev, local)
Sibling reports: [testing-delivery-invoice-transactionscope-results.md](testing-delivery-invoice-transactionscope-results.md), [testing-invoice-transactionscope-results.md](testing-invoice-transactionscope-results.md)

## 1. Scope

`saveButton_Click` (ALL Submit) payment flow: invoice payment update (customer ledger equivalent) + payment header + payment detail/allocation + journal posting — এক atomic transaction; duplicate/double-save prevention (server gate + pre-check + UI); rollback/retry/concurrency tests dev DB-তে।

## 2. Existing flow (inspection findings, আগের অবস্থা)

- **কোনো transaction ছিল না** — প্রতিটি ধাপ আলাদা connection/auto-commit; journal fail করলে payment রয়ে যেত।
- `PaymentJournalPosting`-এর return value **সম্পূর্ণ ignore** হতো।
- Loop-এ `aCustPaymentDetails` list **accumulate** হতো — row N-এর header-এর সাথে আগের সব row-র detail re-insert + re-journal attempt হতো (detail-এর SQL guard আর journal cursor-এর CustPayId filter কাকতালীয়ভাবে ক্ষতি আটকাত)।
- `save` flag শুধু শেষ row-র ফলাফল বহন করত — mixed success/dup-এ ভুল message।
- Duplicate check ছিল শুধু check-then-act (`sp_Check_Duplicate_InvoiceFinalPayment`) — concurrent request-এ race।

## 3. Exact call chain (নতুন, actual method names)

```
CustomerPayment.aspx (ALL Submit, masterButton)
  └ saveButton_Click
      ├ Validation()
      ├ PASS 1 (tx-এর বাইরে, per checked row):
      │   ├ OrderInfoDAL.CheckInvoiceCustpayment → sp_Check_Duplicate_InvoiceFinalPayment   [fast dup pre-check, DataAccessManager]
      │   └ header/detail build → PaymentWorkItem list
      └ PASS 2 — TransactionScope(Required, ReadCommitted, 5min):
          per work item:
          ├ CustPaymentBLL.IsPaymentAlreadyProcessed        → tblInvoice UPDLOCK gate + tblCustPayDetail exists   [নতুন, EntLib]
          ├ CustPaymentBLL.UpdateInvoiceFinalPaymentTx      → sp_Update_InvoiceFinalPayment (tblInvoice UPDATE)   [নতুন route, EntLib UpdateAction]
          ├ CustPaymentBLL.SaveCustPaymentTx                → PrimaryKeyMax + tblCustomerPay INSERT + tblCustPayDetail INSERT (return-checked) [নতুন, EntLib]
          ├ CustPaymentBLL.PaymentJournalPosting            → sp_Sales_CustomerPaymentPosting (ACCDB vouchers + শেষে COGS)  [existing, EntLib]
          ├ CustPaymentBLL.IsPaymentJournalPosted           → ACCDB voucher existence (TableID=2017, CommonPK_ID=CustPayId) [নতুন, EntLib]
          └ scope.Complete() → তারপর money-receipt modal (scope-এর বাইরে)
```

## 4. Changed files

| File | Change |
|---|---|
| `Solution.Web/SInventory_UI/CustomerPayment.aspx.cs` | `saveButton_Click` দুই pass-এ restructure + TransactionScope + gate + return checks + journal verify; `PaymentWorkItem` private class; `using System.Transactions;` catch message: **`Customer payment failed: <ex.Message>`** |
| `Solution.Web/SInventory_UI/CustomerPayment.aspx` | `paySubmitOnce()` JS (double-click lock) + `masterButton` OnClientClick বদল |
| `Library.DAL/SInventory_DAL/CustPaymentDAL.cs` | **additive**: `IsPaymentAlreadyProcessed`, `UpdateInvoiceFinalPaymentTx`, `IsPaymentJournalPosted` |
| `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` | **additive**: ৩টি passthrough + `SaveCustPaymentTx` (return-checked single header+detail save) |

কোনো existing method/SP/schema বদলায়নি — অন্য ১৭টি caller page unaffected।

## 5. TransactionScope implementation

`TransactionScopeOption.Required`, `IsolationLevel.ReadCommitted`, `Timeout=5min` — delivery page-এর pattern হুবহু। `scope.Complete()` শুধু সব work item-এর সব mandatory operation সফল হলে। Success modal/`hfCustPayId` scope-এর বাইরে। Gate-এ duplicate ধরা পড়লে `return` (Complete ছাড়া) → পুরো batch rollback।

## 6. Critical failure/boolean checks

| Call | False মানে | Action |
|---|---|---|
| `UpdateInvoiceFinalPaymentTx` | SP-র dup-guard-এ 0 rows / SQL error (UpdateAction গিলে false দেয়) | `throw "Invoice payment update failed"` |
| `SaveCustPaymentTx` | header/detail INSERT 0 rows (detail-এর নিজস্ব dup-guard skip সহ) | `throw "Payment save failed"` |
| `PaymentJournalPosting` | **সবসময় false** — SP-তে `SET NOCOUNT ON`, তাই rows-affected boolean অর্থহীন | return ignore, নিচের DB-verify দিয়ে check |
| `IsPaymentJournalPosted` | ACCDB-তে voucher নেই | `totalSettled > 0` হলে `throw "Payment journal posting failed"` |

Journal-verify conditional (`totalSettled > 0`): cursor-ই ভাউচার বানায় শুধু (Payment+Discount+AIT) > 0 হলে — zero-amount item-এ voucher না থাকা designed no-op, failure না।

## 7. Customer ledger flow

আলাদা customer-ledger table এই flow-তে নেই; ledger-equivalent হলো `tblInvoice`-এর `PaymentAmount` (increment), `PaymentStatus` (Full/Partial), `FinalPaymentNo` ('PYT-'+InvoiceNo), `FinalPaymentBy/Date` — সবই `sp_Update_InvoiceFinalPayment`-এ, transaction-এর ভিতরে। Test 2-তে rollback প্রমাণিত। Customer outstanding সব report/grid এই কলাম + `tblCustPayDetail` SUM থেকে derive হয়।

## 8. Invoice allocation flow

Allocation = per-invoice `tblCustPayDetail` row (PaymentAmount, TP/VAT split, AIT, Discount, Bank/CashAccId, CollectionBy, DANameId)। প্রতি selected invoice-এর নিজস্ব header+detail (আগের committed behavior-ই, list-accumulation bug ছাড়া)। Multi-invoice atomicity Test 9-এ প্রমাণিত।

## 9. Cash/Bank flow

আলাদা cash/bank ledger table নেই — cash/bank effect হলো voucher-এর **Debit GL**: `tblChartOfAccounts`-এ CashAccountId/BankAccountId → AccountId। Cash→`CVC-` voucher, Bank→`CVB-` voucher। Test 10-এ দুটোই verified (cash acc 1→GL 5925, bank acc 1→GL 6168)। Voucher rollback হলে cash/bank effect-ও rollback।

## 10. Receipt flow

Money receipt = **print/report artifact only** (`MoneyReceiptViewerForAfterPayment.aspx`, Crystal Report) — আলাদা কোনো receipt DB row নেই। Receipt no = payment header-ই (CustPayId), transaction-এর ভিতরে। তাই "receipt rollback" = header rollback (Test 2/3/4-এ প্রমাণিত)। Modal-এর print button save-এর পরে চলে, DB ছোঁয় না।

## 11. Journal flow

`sp_Sales_CustomerPaymentPosting` (dev DB থেকে dump):
- Writes: `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster` (VoucherType CVB/CVC, TableID=2017, CommonPK_ID=CustPayId) + `tblDebitCreditVoucherDetail` — Dr: Cash/Bank GL (+ Discount GL 4168 FlagId 1023, AIT GL 4165 FlagId 1024 থাকলে), Cr: Accounts Receivable (GL 2112, PageId 163 FlagId 2, subsidiary=customer)।
- ভাউচার-duplicate guard **নেই** (V-এর LEFT JOIN আছে কিন্তু WHERE-এ ব্যবহার হয়নি) — একই CustPayId-তে দ্বিতীয় call দিলে duplicate voucher হতো; নতুন flow-তে প্রতি header ঠিক একবারই post হয়, আর gate re-submit আটকায়।
- শেষে full payment (Due≤0) হলে `sp_Sales_DeliveryCOGSPosting` — ওটার নিজস্ব dup-guard আছে (`IsInventoryValuation=1` voucher check), Test 10b-তে প্রমাণিত।

## 12. Stored procedure analysis

| SP | TRAN/TRY/XACT | Guards | Note |
|---|---|---|---|
| `sp_Update_InvoiceFinalPayment` | কিছু নেই | নিজস্ব dup check (invoice+today+amount) → skip + RETURN 0 | UPDATE 1 row → UpdateAction true; dup→0 rows→false |
| `sp_Sales_CustomerPaymentPosting` | কিছু নেই; `SET NOCOUNT ON` | payment-voucher dup guard **নেই**; amount>0 filter | NOCOUNT-এর কারণে rows-affected সবসময় -1 → UpdateAction false even on success |
| `sp_Sales_DeliveryCOGSPosting` | কিছু নেই | নিজস্ব dup guard আছে | payment SP-র ভিতর থেকে **TRY/CATCH ছাড়া** call হয় |
| `sp_Check_Duplicate_InvoiceFinalPayment` | — | SELECT only | pre-check |

কোনো SP-তে নিজস্ব BEGIN TRAN নেই → সবগুলো ambient TransactionScope-এ enlist করে; cross-DB rollback Test 2/3/9-এ প্রমাণিত। **Nested transaction issue নেই।**

## 13. Tables affected (save flow)

`SalesDisDB_Unicorn_DB`: `tblInvoice` (UPDATE), `tblCustomerPay` (INSERT), `tblCustPayDetail` (INSERT)।
`CSTL_ACCDB_Unicorn`: `tblDebitCreditVoucherMaster`, `tblDebitCreditVoucherDetail` (INSERT; COGS হলে আরও এক set)।
Reads: `CSTL_ERPDB_Unicorn` integration/GL tables।

## 14. Connection strings

| Path | Pool |
|---|---|
| Gate, sp_Update (নতুন route), header/detail INSERT, PrimaryKeyMax, journal SP, journal verify — **transaction-এর ভিতরের সবকিছু** | EntLib ← Web.config `SolutionConnectionStringSSIDB` (একটাই connection) |
| Pre-check `CheckInvoiceCustpayment` (tx-এর **বাইরে**) | `DataAccessManager` (SqlUserAccess statics, Encrypt=True — আলাদা pool) |

**গুরুত্বপূর্ণ:** পুরনো `UpdateInvoiceFinalPayment` (DataAccessManager.UpdateData) scope-এর ভিতরে চালানো **সম্ভবই না** — `ExecuteNonQueryVoid` নিজে `BeginTransaction()` করে, enlisted connection-এ যেটা exception। এজন্যই `UpdateInvoiceFinalPaymentTx` (একই SP, EntLib route) — পুরনো method অক্ষত।

## 15. MSDTC promotion status

**NOT REQUIRED — promotion ঘটে না।** সব test-এ `Transaction.Current.TransactionInformation.DistributedIdentifier = 00000000-…` (empty)। কারণ: scope-এর ভিতরের প্রতিটি call একই EntLib connection string ব্যবহার করে (দ্বিতীয় pool ঢোকে না; DataAccessManager pre-check tx-এর বাইরে)। Cross-database (ACCDB) write একই connection-এ three-part name দিয়ে — promotion trigger করে না। Delivery page-এর মতো MSDTC dependency এখানে **নেই**।

## 16. Payment business/idempotency key

```
PAYMENT IDEMPOTENCY KEY:
InvoiceId + CONVERT(DATE, custPaymentDate) [= আজকের তারিখ] + PaymentAmount
```
Invent করা হয়নি — এই key-ই তিন জায়গায় বিদ্যমান business rule: `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_Update_InvoiceFinalPayment`-এর guard, `SaveCustDetail`-এর INSERT guard। সীমাবদ্ধতা: একই invoice-এ **একই দিনে একই amount**-এর দ্বিতীয় বৈধ payment সম্ভব না (এটা আগের থেকেই সিস্টেমের রুল); ভিন্ন amount হলে নতুন payment হয়।

## 17. Payment/receipt numbering mechanism

```
PAYMENT NUMBER: CustPayId = ClsPrimaryKeyFind.PrimaryKeyMax → SELECT MAX(CustPayId)+1 FROM tblCustomerPay
RECEIPT NUMBER: আলাদা নেই — CustPayId-ই receipt reference; voucher code = CVC/CVB-yyMMdd + MAX(substring)+1 (SP-র ভিতরে)
```
MAX+1 pattern concurrency-unsafe, **কিন্তু fail-safe**: `PK_tblCustomerPay` (CustPayId PK) থাকায় দুই ভিন্ন invoice-এর payment একই মুহূর্তে হলে দ্বিতীয়টা PK violation → exception → full rollback → retry-তে সফল। একই invoice-এর race UPDLOCK gate-এই serialize হয়। Numbering rewrite করা হয়নি (instruction অনুযায়ী); voucher code-ও SP-র নিজস্ব logic, অক্ষত।

## 18. Existing unique indexes/constraints

```
DB UNIQUE CONSTRAINT (business key-র জন্য): NO
```
- `tblCustomerPay`: `PK_tblCustomerPay` (CustPayId) — আছে ✅
- `tblCustPayDetail`: **কোনো PK/unique index নেই** — ১৩টা index সবই non-unique। Business key-র unique index যোগ করা হয়নি (production schema change এই task-এর বাইরে); চাইলে future hardening হিসেবে `UNIQUE (InvoiceId, custPaymentDate, PaymentAmount)` filtered/computed-date index বিবেচনা করা যায়।

## 19. Server-side duplicate protection

`IsPaymentAlreadyProcessed` — scope-এর প্রথম statement:
```sql
SELECT CASE WHEN EXISTS (SELECT 1 FROM tblCustPayDetail
       WHERE InvoiceId=INV.InvoiceId AND CONVERT(DATE,custPaymentDate)=CONVERT(DATE,GETDATE())
         AND ISNULL(PaymentAmount,0)=ISNULL(@PaymentAmount,0)) THEN 1 ELSE 0 END
FROM tblInvoice AS INV WITH (UPDLOCK, ROWLOCK) WHERE INV.InvoiceId=@InvoiceId
```
UPDLOCK invoice row-তে transaction শেষ পর্যন্ত থাকে → concurrent Request B এখানে block হয়, A commit হলে B committed detail দেখে "Already Exist!" দিয়ে **কিছু না লিখে** বেরোয় (Complete হয় না)। A rollback করলে B স্বাভাবিক এগোয় (retry-after-rollback তাই কাজ করে)। Existing pre-check (fast path) তার আগেই সাধারণ double-click ধরে।

## 20. UI double-click protection (secondary)

`masterButton` → `paySubmitOnce(this)`: confirm-এর পর button-এ `dataset.busy` + pointer-events lock; UpdatePanel re-render-এ (success/failure দুটোতেই) button ফ্রেশ হয় → failure-এর পরে retry সম্ভব। Primary protection সবসময় server gate।

## 21. Concurrency protection summary

Same key race → UPDLOCK gate serialize (Test 6)। Different-invoice race → শুধু CustPayId MAX+1 collision সম্ভব → PK violation → rollback+retry (fail-safe, silent corruption না)। `SERIALIZABLE`/app-lock/schema change লাগেনি।

## 22–31. Test results

Execution: ASPX browser-এ না; scratchpad harness (`PayTxHarness.exe`) **real BLL/DAL/DB** দিয়ে `saveButton_Click`-এর Pass 1+2 exact call sequence replay করেছে (same TransactionScope options, same gate, same throw-conditions)। Site precompile (aspnet_compiler) exit 0 ✅

| # | Test | Invoice(s) | Result |
|---|------|-----------|--------|
| 1 | Happy path (cash 500, commit) | 1126 | ✅ PASS — INV PA=500/Partial/PYT-no, header 777, detail 1, voucher CVC Dr 5925 / Cr 2112 |
| 2 | Failure **after** ledger update → rollback | 1125 | ✅ PASS — before/after **IDENTICAL** |
| 2b | Failure after **সব** operation (journal সহ) → rollback | 1125 | ✅ PASS — IDENTICAL (cross-DB voucher সহ) |
| 3 | Journal failure (posting হয়নি → verify throw) | 1125 | ✅ PASS — `Payment journal posting failed`, IDENTICAL |
| 4 | Critical bool false → throw → rollback | 1125 | ✅ PASS — `Invoice payment update failed`, IDENTICAL |
| 5 | Double click (sequential) | 1125 | ✅ PASS — 1st COMMITTED, 2nd PRECHECK-DUPLICATE, 3rd (pre-check bypass) gate-DUPLICATE; state unchanged |
| 6 | Concurrent double payment (2 thread, pre-check bypass, একই মুহূর্ত) | 1123 | ✅ PASS — **exactly 1 COMMITTED, 1 DUPLICATE**, detail rows=1 |
| 7 | Retry after success | 1125 | ✅ PASS — (Test 5-এর click 2/3) নতুন কিছুই লেখা হয়নি |
| 8 | Retry after rollback | 1122 | ✅ PASS — attempt1 rollback verified, attempt2 COMMITTED |
| 9 | Multi-invoice (3 invoice), শেষটায় fail → **ALL rollback**; তারপর commit | 1121,1119,1118 | ✅ PASS — fail run: ৩টা invoice-ই IDENTICAL (আগের ২টার voucher সহ rollback); clean run: ৩টাই atomic commit |
| 10a | Bank payment mode | 1119 | ✅ PASS — `CVB-` voucher, Dr GL 6168 (bank acc 1) |
| 10b | Full payment (due শোধ) → Status=Full + COGS path | 1126 | ✅ PASS — PaymentStatus=Full, COGS dup-guard-এ duplicate হয়নি |

## 32–33. DB verification + rollback

প্রতি test-এ before/after snapshot (scope-এর বাইরে, Suppress-এ): `tblInvoice` (PaymentAmount, PaymentStatus, FinalPaymentNo, FinalPaymentBy) per invoice; `tblCustomerPay`/`tblCustPayDetail` COUNT+MAX; per-invoice today's detail count; ACCDB voucher master (TableID=2017) + detail counts; COGS voucher count। Rollback test-গুলোতে প্রতিটি field **byte-for-byte identical**। Voucher Dr/Cr line-level verify: Dr=Cash/Bank GL, Cr=AR 2112, amounts exact।

Test শেষে দুই DB-ই COPY_ONLY backup থেকে **restore + verified** (payCnt=770, payMax=776, detCnt=753, inv1126 PA=0, vouchers=753, SP definition অক্ষত)। Backups: `D:\SqlBackups\SalesDisDB_Unicorn_DB_PreCustPayTest.bak`, `D:\SqlBackups\CSTL_ACCDB_Unicorn_PreCustPayTest.bak` (C: drive-এ জায়গা ছিল না)।

## 34. Build/regression

- `Library.DAL` + `Library.BLL` msbuild ✅ (warnings pre-existing); DLL দুটো `Solution.Web/Bin`-এ কপি করা হয়েছে।
- পুরো site `aspnet_compiler` precompile **exit 0** ✅ — কোনো page ভাঙেনি।
- Shared method গুলোতে শুধু **addition**; `SaveCustPayment`(list overload, ১৭টা caller), `UpdateInvoiceFinalPayment`, `PaymentJournalPosting` — সব অক্ষত।
- Payment/allocation calculation (TP/VAT split, AIT, Discount, Full/Partial logic) হুবহু আগের expression — শুধু জায়গা বদলেছে (PASS 1-এ build, PASS 2-এ write)।

## 35. Production deployment requirements

1. **MSDTC লাগবে না** — promotion হয় না (section 15)। তবু production-এ প্রথম payment-এর পরে log/verify করা ভালো।
2. Deploy-এ **Library.DAL.dll + Library.BLL.dll rebuild করে Bin-এ দিতে হবে** (নতুন method) — শুধু aspx/aspx.cs দিলে MissingMethodException।
3. Failure message এখন আসল exception দেখায় (`Customer payment failed: …`) — চাইলে log+generic করা যায়।
4. Excel upload / Snd / SubDepot / delivery-auto-payment page গুলো এখনো পুরনো unguarded path-এ — একই pattern চাইলে বসানো যায়।

## 36. Remaining risks / limitations

- **COGS error edge:** journal SP-র শেষে COGS call TRY/CATCH ছাড়া; COGS-এ SQL error হলে UpdateAction false দেয় কিন্তু payment voucher তখন存在 → verify pass → payment commit হবে COGS ছাড়া (COGS-এর নিজস্ব guard-এ পরের full payment-এ আর আসবে না)। Existing SP behavior, SP বদলানো task-এর বাইরে।
- Browser/UI-level automation test হয়নি (আগের page-গুলোর মতোই call-sequence replay)। `paySubmitOnce` JS manually exercise করা হয়নি।
- একই দিনে একই invoice-এ **একই amount**-এর দ্বিতীয় বৈধ payment idempotency rule-এই আটকায় — business-সম্মত কিনা সেটা বিদ্যমান রুল, নতুন না।
- `tblCustPayDetail`-এ DB-level unique constraint নেই — শেষ রক্ষা এখন server gate; DB constraint চাইলে আলাদা migration।
- Backdated PaymentDate-এ gate-এর GETDATE() rule আর detail-এর PDate rule ভিন্ন দিনে পড়ে — mismatch হলে detail-guard 0-rows → `Payment save failed` → rollback (fail-safe, silent dup না)।
- MarketId/RouteId harness-এ 0 দেওয়া হয়েছিল (grid-hidden field গুলো optional metadata; INSERT columns同一)।
