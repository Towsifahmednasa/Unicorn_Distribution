# ✅ Test Execution Results — `saveButton_Click` TransactionScope

Executed: 2026-09-24 · Server: `TOWSIF\MSSQLSERVER2019` (SQL Server 2019 RTM 15.0.2000.5) · DB: `SalesDisDB_Unicorn_DB` (dev, local)
Plan: [testing-invoice-transactionscope.md](testing-invoice-transactionscope.md)

## Execution method (important scope note)

ASPX page-টা browser-এ click kore chalano hoyni (login/postback automation chara). Er bodole:
- **Build check:** puro website `aspnet_compiler` diye precompile — exit 0, শুধু unrelated warning। TransactionScope + Web.config `System.Transactions` change compile hoy ✅
- **Flow execution:** ekta test harness (scratchpad-e, repo-r baire) `saveButton_Click`-er **exact BLL call sequence** replay koreche — `CustomerMaster` → `GetFixedCustomer` → `InvoiceBLL.SaveInvoice` → `SelectInvoiceID` → `ProductDiscount` → `SaveInvoiceDetails` → `UpdateInvoiceHeaderTotals` → `UpdateInvoiceStatus` — same `TransactionScope(ReadCommitted, 5min)`-er bhitore, **real BLL/DAL/DB diye**। UI control-read gulo same DB query diye replace (grid jeta theke load hoy)। Fake `HttpContext` + `Session["RouteId"]="0"` set kora chilo.
- Test-er age COPY_ONLY backup, test-er por **DB restore** — dev DB ekhon exact pre-test state-e (verified: 1074 invoices, 1388 details, test invoice 0)।

## Summary

| # | Test | Result |
|---|------|--------|
| 0 | Environment + site precompile | ✅ PASS |
| 1 | Happy path commit (Order A=912) | ✅ PASS |
| 2 | Forced rollback after `UpdateInvoiceStatus` (Order B=916) | ✅ PASS |
| 3 | Promotion / MSDTC | ⚠️ PASS with finding: **promotion=True in every run** |
| 4 | `dtl==false` branch | ✅ PASS |
| 5 | Timeout abort (10s scope + 15s sleep) | ✅ PASS |
| 6 | Post-rollback consistency + retry | ✅ PASS |

## Test 0 — Environment
- `Microsoft SQL Server 2019 (RTM) 15.0.2000.5 (X64) Enterprise`, server `TOWSIF\MSSQLSERVER2019`, DB ONLINE/MULTI_USER
- Required objects present: `ExecuteAllSqlQueryByStoreProcedure`, `sp_Deletenvoice`, সব টেবিল
- Identity confirmed: `tblInvoice.InvoiceId`=IDENTITY, `tblInvoiceDetail.InvoiceDetailId`=IDENTITY, `tblDCStore.DCStoreId`=MAX+1 (plan-এর A2/A3 সঠিক)
- Backup: `C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER2019\MSSQL\Backup\SalesDisDB_Unicorn_DB_PreTxTest.bak` (COPY_ONLY, এখনো আছে)
- MSDTC service test শুরুতে **Stopped** ছিল (StartType=Manual)

## Test 1 — Happy Path (Order 912, ODR-260923912, 2 lines)
Committed: `InvoiceId=1127`, `InvoiceNo=INV-BD020000001127`

| Check | Result |
|---|---|
| tblInvoice / tblInvoiceBatch | +1 / +1 ✅ |
| tblInvoiceDetail | +3 ✅ (1টা line FIFO-তে 2 batch-এ split: DCStore 2077+2078, আরেকটা 1653) |
| tblDCStore Issue rows | +3, negative StockQty, `ReferenceDCStoreId` linked ✅ |
| Header totals | TpTotal=SumTotalPrice=17400.00, Grand=ExpectedGrand=17400.00, Recv ✅ |
| IssuedQty = InvoicedQty | 4.00 = 4.00 ✅ |
| tblOrder.IsInvoice | 0→1 ✅ |
| tblReturnAmount | unchanged ✅ (`SaveReturnAmount` off) |
| Orphans | 0 ✅ |

(Cosmetic: `tblInvoiceBatch`-এ পুরনো DB-র stale row ছিল যার InvoiceId=1127 — নতুন invoice আসায় orphan count 1 কমেছে। এই টেবিলে 736K stale row আছে, invoice মাত্র ~1K।)

## Test 2 — Forced Rollback (Order 916)
সব 4 step সফলভাবে চলেছে (invId=1128 পর্যন্ত insert হয়েছিল), তারপর `throw new Exception("TEST_TRANSACTION_ROLLBACK")` — `UpdateInvoiceStatus`-এর **পরে**, plan অনুযায়ী।
- Before/after full state snapshot: **IDENTICAL** — tblInvoice, tblInvoiceBatch, tblInvoiceDetail, tblDCStore, tblOrder.IsInvoice, tblOrderDetail.Status সব rollback ✅
- `sp_Deletenvoice` execution_count: 0 → 0 (call হয়নি) ✅

## Test 3 — Promotion / MSDTC ⚠️ (main finding)

**প্রতিটা run-এ transaction distributed-এ promote হয়েছে** — `DistributedIdentifier` কখনো empty ছিল না (যেমন Test 1: `Local=cbbb0505-...:1`, `Distributed=34c1bc8a-...`)।

**Root cause (code-confirmed):** save flow **দুইটা আলাদা connection string** ব্যবহার করে →
1. `SInventorySql.Execute` → `ClsCommonInternalDAL` → Enterprise Library → config-এর `SolutionConnectionStringSSIDB`
2. `SInventorySql.GetDataTable` → `DataAccessManager.ConnectionString()` → `SqlUserAccess` থেকে নিজে বানানো string (**`Encrypt=True;TrustServerCertificate=True` সহ — config string-এ নেই**)

আলাদা string = আলাদা connection pool = এক scope-এ দ্বিতীয় pool ঢোকামাত্র MSDTC promotion।

**Observed behavior:** MSDTC Stopped ছিল, promotion-এর সময় **on-demand auto-start** হয়ে গেছে (এখন Running)। কোনো MSDTC exception আসেনি; commit এবং rollback দুটোই DTC-র মধ্য দিয়ে সঠিক কাজ করেছে।

**Production risk:**
- Production server-এ MSDTC disabled/blocked থাকলে প্রথম save-এই `"MSDTC on server ... is unavailable"` আসবে
- Web server আর DB server আলাদা machine হলে **দুই পাশেই Network DTC Access** লাগবে
- প্রতি invoice save এখন 2-phase commit — সামান্য slow, বেশি fragile

**Recommended fix (Plan B-র আগে, অনেক ছোট):** `DataAccessManager.ConnectionString()`-এর string-টা config-এর `SolutionConnectionStringSSIDB`-র সাথে **হুবহু এক** করা (এক pool → sequential open/close-এ promotion হওয়ার কথা না)। তারপর এই harness দিয়ে re-test করে `Distributed=empty` verify করা। কাজ না করলে তখন Plan B (`SqlTransaction`)।

## Test 4 — `dtl==false` Branch (Order 916)
`SaveInvoiceDetails` **চলার পরে** (detail + DCStore rows insert হয়ে যাওয়ার পরে) `throw "Please generate Invoice again!"`:
- State IDENTICAL — already-inserted rows সহ সব rollback ✅
- Exception message ঠিক page-এর মতোই ✅

## Test 5 — Timeout (10s scope, 15s sleep)
- Exception: `System.Transactions.TransactionException: The operation is not valid for the state of the transaction.` / inner: `System.TimeoutException: Transaction Timeout`
- State IDENTICAL — timeout abort-এও full rollback ✅

## Test 6 — Consistency + Retry (Order 916, ৩টা rollback-এর পরে)
- Half-saved headers: 0, orphan Issue rows: 0, orphan details: 0 ✅
- Retry commit সফল: invoice created, `IsInvoice=1`, detail present, "already generated" আসেনি ✅
- InvoiceNo retry-তে সেই একই `INV-BD020000001128` (plan A5 সঠিক) ✅
- ℹ️ **Note (pre-existing, fail না):** rollback-এ IDENTITY পুড়ে যাওয়ায় retry-র `InvoiceId=1131` কিন্তু InvoiceNo suffix `1128` (নম্বর আসে `MAX(InvoiceId)+1` থেকে)। Collision হয় না, কিন্তু id আর no-র মিল আর থাকে না। এটা TransactionScope-এর আগেও সম্ভব ছিল, rollback এটাকে দৃশ্যমান করে।

## Cleanup verification
- [x] DB restored from backup — 1074 invoices / 1388 details / order 912+916-এর test invoice 0, ONLINE, MULTI_USER
- [x] Repo-তে test-এর কোনো temporary code নেই (harness scratchpad-এ ছিল); `git status`-এ শুধু আগের intended changes
- [x] Backup file রাখা আছে (উপরের path)
- [ ] MSDTC service এখন Running (আগে Stopped) — বন্ধ করতে চাইলে: `Stop-Service MSDTC`

## Verdict

TransactionScope change টা **কাজ করছে**: commit, rollback (মাঝপথে ও শেষ ধাপের পরে), timeout — সব ক্ষেত্রে all-or-nothing আচরণ verified। একটাই action item: **connection string unify করে promotion বন্ধ করা**, না হলে production-এ MSDTC dependency যাচ্ছে।
