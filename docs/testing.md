# Testing (current state, evident QA practice, recommended manual approach)

Documentation only. Evidence-based; nothing was executed. Tags: `CONFIRMED`, `INFERRED`, `UNKNOWN`.

Related: [architecture.md](architecture.md), [security.md](security.md), [deployment.md](deployment.md), module specs under `../specs/<slug>/README.md` (module ids M01-M18, see [system-overview](../specs/00-system/system-overview.md)).

## 1. Automated tests: none

| Check | Result | Evidence |
|---|---|---|
| Test projects in `Solution.sln` | None (5 projects: Solution.Web, Library.DAL/BLL/DAO/CrystalReports) | `Solution.sln` |
| NUnit / xUnit / MSTest references, `[Test]`, `[TestMethod]`, `[Fact]`, `Microsoft.VisualStudio.TestTools` | 0 hits in `*.cs`, `*.csproj`, `*.config`, `packages.config` | grep CONFIRMED (the only "test" text hits are generated dataset designer files and `MaterialConversionReceive` naming) |
| CI configuration (`.github`, `azure-pipelines`, `Jenkinsfile`, build scripts) | none in repo root | `ls` |
| Mocking seams | none: DAL classes are concrete, created with `new` inside pages/BLL, no interfaces, static fields, `HttpContext.Current.Session` used inside BLL/DAL (`Library.BLL/SInventory_BLL/InvoiceBLL.cs:97`, `CommonDataLoad.cs:23`), so unit testing needs either a real database or refactoring | code |
| Code-coverage / analyzers | none configured | csproj |
| Stray artefacts | `Library.DAL/bin/Debug/test_fixed_chalandal.exe` (an ad-hoc console check of `ChalanDal`, built output, not source-controlled as a project) | file listing (INFERRED purpose) |

## 2. How QA is evidently done

1. **Manual UI testing against a separate test database.** `DataBase.cs` keeps commented alternates such as `SalesDisDB_SMC_NEWDB_InvoiceTest` and `SalesDisDB_SMC_NEWDB_TerritoryStucture` (`Library.DAL/DataManager/DataBase.cs:6-8`); `Web.config` carries commented connection strings for developer machines (`PROGRAMMER25-PC`, `CSTL-PC-7`, `.` local) (`INFERRED`: testers point the site at a copy DB by hand-editing these files, see [deployment.md](deployment.md)). `PrecompiledWeb/Solution.Web/Web.config` targets a developer workstation host (CONFIRMED by diff of the active entry).
2. **`SInventory_UI/Testing_OrderStatus.aspx`** (+ `.cs`, class `SInventory_UI_Testing_OrderStatus`): a diagnostic page that looks up an order number and shows sales centre, MIO, customer, gross value, submission date and existing invoice/proforma information for the order. `OrderStatusDal.LoadOrderStatusInfoTesting` queries a **different database `SystemTest_Testing..OrderListDetail`** (`Library.DAL/SInventory_DAL/OrderStatusDal.cs:22-30`), i.e. a dedicated test database exists or existed on the server (existence `UNKNOWN`). It uses `MainMasterPage` (login gate from the master only) and is not excluded from the build.
3. **Hand-run SQL verification and repair scripts** in the repository root, each self-describing and re-runnable: `sp_Check_DCStore_StockAnomalies.sql` (finds orphan issue lines / excess receive stock), `SP_Audit_Sales_Accounts_Voucher_Posting.sql` ("1-click" read-only audit of voucher existence, debit/credit balance and value accuracy across 5 operations: material-conversion receive, delivery confirmation, customer payment, sales return, non-transactional return; uses `WITH (NOLOCK)`), `SP_Fix_Invoice_Anomaly_And_Sync.sql` (syncs `tblInvoice` header totals with `tblInvoiceDetail`), `SP_Adjust_DCStore_Stock_Only.sql`. These are de-facto data-integrity regression checks executed manually after releases.
4. **Data-fix scripts as test-data generators**: `Create_Order_ODR-20260901134.sql` inserts (or resets) one order in a transaction; useful as a pattern for repeatable test data (order code format `ODR-yyyymmddNNN`, customer code `C0000134`-style, MIO code `EMP000055`-style, warehouse/company-unit ids).
5. **Upgrade/migration evidence**: `UpgradeLog.htm` is a Visual Studio migration report (solution needs no migration; `Solution.Web` project type could not be opened by that Visual Studio installation: 1 error, 0 warnings). It is not a test report.
6. **Compilation as the smoke test**: `_compilecheck.dll` (root, 19,968 bytes, stray compile check), `PrecompiledWeb` produced by `aspnet_compiler` (a successful precompile validates every `.aspx`/`.cs` compiles, which is the only systematic automated check available).
7. **Excel upload templates** are committed under `Solution.Web/ExcelFiles/` (customer, deposit slip, order-list, STO, target formats such as `CustomerFormat.xls`, `Customer_Import_Format.xlsx`, `Deposit Slip Format Bkash.xlsx`, `Deposit Slip Format DBBL.xlsx`) and `Pro_Info/` holds sample outputs (`Customer Ledger Report ...xlsx`, `Collection Ledger.xlsx`, `Dynamic Sales Summary Report.xlsx`, `MarketTranferFormat (Market Shift).xls`). They double as functional test inputs/expected outputs (INFERRED).

## 3. Manual test approach recommended for this stack

The application is database-centric (business rules live mostly in stored procedures not stored in the repo), so testing must be **black-box through the UI plus SQL assertions**.

### 3.1 Environment
- A restorable copy of `SalesDisDB_Unicorn_DB` plus its siblings `CSTL_ACCDB_Unicorn` (vouchers) and `CSTL_ERPDB_Unicorn` (material conversion, batch stock) on the same SQL Server instance, because SPs use cross-database names ([architecture.md](architecture.md) section 7). Otherwise posting SPs fail.
- Point all four connection places at the test instance in step: `Solution.Web/Web.config`, `Library.DAL/DataManager/SqlUserAccess.cs`, `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs` (used by `DB_Manager`), and confirm `DataBase.SalesDB`. Note `DB_Manager` paths open a database literally named `SalesDisDB_New3` ([architecture.md](architecture.md) section 6.5); it must exist or the invoice / return / sub-depot paths using it will fail.
- Install the Crystal Reports 13 runtime and the ACE 12.0 OLEDB provider (Excel import) on the test server; see [deployment.md](deployment.md).
- Use `IIS Express` or IIS with the same 4.0 app pool; InProc session, so each test restart clears sessions.

### 3.2 Technique
1. **Role matrix**: create one user per role type (role names are data, `UNKNOWN`) and per company unit, and test the menu (DB-built by `dbo.MainMenu2`), then attempt **direct URLs** for pages the role should not see (menu-only authorization; only 17 pages self-check, see [security.md](security.md)). Include unauthenticated requests to `*Viewer.aspx`, `*Edit.aspx`, `HandlerDocCV.ashx` and `*.asmx` as negative tests to document current behaviour.
2. **State-transition tests** on the order-to-cash chain with SQL snapshots before/after each step (`tblOrder`, `tblInvoice`, `tblInvoiceDetail`, `tblDCStore`, `tblCustomerPay`, voucher tables): use the flags/columns named in the specs (`IsInvoice`, `IsPrepareforInvoice`, `ActionStatus`, `LoadingSummaryStatus`).
3. **Atomicity/failure tests**: delivery confirmation (`LoadingSummary.aspx`) executes three separate autocommit calls; interrupt or force a failure (e.g. break the posting SP in the test DB) and check for half-applied state (invoice confirmed, journal not posted).
4. **Concurrency tests** for `MAX+1` keys: two browsers saving the same entity at the same moment (Area, Customer, Product, Invoice) and check duplicate key errors or duplicate codes; also two users on the same `static`-DAL page (e.g. `DoctorMaster_UI/ChamberType.aspx`).
5. **Import tests**: run each Excel importer (15 pages) with valid file, wrong extension, empty sheet, wrong column names, duplicates, large file; confirm the temporary file left in `~/ExcelFiles/`.
6. **Report parity tests**: compare Crystal viewer output with the corresponding SQL/SP result and with the Excel export for the same filter (dates via `fromDate/toDate` QueryString); cover blank data, single row, multi-page, PDF export.
7. **Date/culture tests**: many pages parse `dd-MMM-yyyy` strings (`LoadingSummary.aspx.cs:624` passes `DateTime.Now.ToString("dd-MMM-yyyy")`); run under the server culture used in production and one different culture.
8. **Session expiry**: session timeout is 24,000 min, so test by manually abandoning the session cookie (or recycling the pool) while a page with an open form posts back (master check is skipped on postback).
9. **Browser matrix**: NewMaster theme (Bootstrap 5) vs MainMaster theme (older); popup Edit pages open via `window.open` (allow popups).
10. **Mobile/webview**: `*Apps.aspx` pages with `EmpId`/`EmpInfoId` in QueryString from the Android APK (`Solution.Web/APK_File/`).

### 3.3 Evidence to capture
SQL snapshots (before/after), the query used, screenshots of messages (many messages are the literal strings "Data Save Successfully", "Data Not Update!!!", "Operation successful!"), and the SQL Server profiler trace of SP calls for pages whose SP bodies are missing.

## 4. Test-data hints

- Identifier formats visible in code/scripts: order `ODR-yyyymmddNNN` (`Create_Order_ODR-20260901134.sql`), customer codes `C0000134`, employee/MIO codes `EMP000055`, warehouse/company-unit codes such as `BD02`; `CustomerCode`, `EmpMasterCode`, `DACode` (login for DA users equals DA code, see [security.md](security.md)).
- Login prerequisites: `tblUser.UserStatus = 'active'`, a role in `tbl_UserRoleInfo`, `tblUserCompanyUnit` rows (non-admin users need a company unit; admin has none), `IsPasswordChange` (0/false forces the password modal on `NewMasterPage` pages).
- Minimal master data chain for ordering: company -> company unit (DC) -> region/zone/area/territory/market (`tblCompanyInfo`, `tblCompanyUnit`, `tblRegion`, `tblArea`, `tblMarket`), customer master (`tblCustMaster`), product/pack/unit price (`tblUnitPrice`), stock in `tblDCStore`.
- Excel formats: use the committed templates in `Solution.Web/ExcelFiles/`.
- Date rules: financial-year tables (`tblFinancialYear`) drive archive and closing screens (`SettingPanel_UI/ArchiveDbConnect.aspx`); avoid running the `FinancialYearDeleteTableEntry` archive/delete pages on any DB that is not a disposable copy (they call `sp_DeleteArchive*Data` SPs, bodies `UNKNOWN`).
- Never run root SQL scripts against a shared DB without a backup; several are `ALTER`/`DELETE` patches (see [deployment.md](deployment.md)).

## 5. Regression checklist skeleton per module

Legend: page paths are under `Solution.Web/`; spec folder is `../specs/<slug>/`. Tick after each release. `BR-Mxx-nnn` ids are defined in each module's README; add rows as new rules are documented.

| Module (spec) | Core regression checks (skeleton) |
|---|---|
| **M01** Authentication and navigation (`authentication-navigation`) | Login valid/invalid/empty; inactive user rejected; admin vs non-admin session keys; redirect to `AdminDashboard` when `IsMainDashboard`; `aspxerrorpath` redirect (relative and `http://` value); menu renders for each role; forced password-change modal; `ChangePassword` policy (12-20 chars, mixed classes, differs from current, confirm matches); logout then back button; `UserTracking` page shows session row |
| **M02** User, role, permission (`user-role-permission`) | Create/edit user (clear-text field behaviour noted), role entry, menu distribution grants, DC user permission, `UserRecords`; unauthorised URL access for a role without the menu |
| **M03** Organisation and market structure (`organisation-market-structure`) | Area/Region/District/CompanyUnit Entry-View-Edit trios (duplicate code check, `MAX+1` key), territory/market setup, transfer pages (`TransferUI`), IV market-structure cascading pickers |
| **M04** Product, pricing, promotion (`product-pricing-promotion`) | Product entry incl. image upload (`HandlerDocCV.ashx`), unit-price entry and rollback on inactivate (`tblUnitPriceRollback`), VAT/MRP fields, campaign/promo allocation |
| **M05** Customer and provider master (`customer-master`) | `CustMasterEntry/Edit`, customer Excel upload/tag change, approval list (`Approval_UI/CustomerApproveList`), credit limit fields, API-customer import |
| **M06** Order management (`order-management`) | Manual order, order upload (`OrderListUpload`), requisition (`OrderRequisitionCreation`), approval, monitor (`OrderMonitor`), DC change, delete/edit; status/flag transitions |
| **M07** Proforma, invoicing, delivery (`invoicing-delivery`) | `ProformaInvoiceCreation` -> invoice by order (manual/auto) -> challan -> picking -> `LoadingSummary` final submit (Full Dues / Cash / Rejection) -> delivery confirmation and journal posting; delete-invoice reports; printing |
| **M08** Sales return (`sales-return`) | `SalesReturn` and return-invoice flows, delivery return, stock re-entry (`ReturnToDCStore`), account posting; scripts `SP_SalesReturn_Related.sql` objects |
| **M09** Collections and payments (`collection-payment`) | `CustomerPayment` (discount, AIT, delivery-charge settlement per `Live_Deployment_Script_All_Updates.sql`), money receipt (before/after payment), deposit-slip Excel upload, reversal/partial, customer ledger, aging (`Aging`, `CustomerInvoiceAgingReport`) |
| **M10** Warehouse and DC stock (`warehouse-inventory`) | Stock in/out, adjustment, freeze, opening balance, material conversion receive, valuation ledger with transaction-date logic, `sp_Check_DCStore_StockAnomalies` shows no orphans |
| **M11** Stock transfer and chalan (`stock-transfer`) | `NewStockTransferDcToDc` issue, receive, delete/rollback, B2B transfer, STO (`StockTransferOrder`), central-store STO issue, in-transit aging |
| **M12** Sub-depot (`sub-depot`) | Product receive, stock out, DC-to-sub-depot transfer, invoice list/top sheet, picking generate; note `Sub_InvoiceDAL` uses `DB_Manager` |
| **M13** Sales reports and dashboards (`sales-reports-dashboard`) | Each Crystal viewer opens with parameters, matches SQL; Excel exports; `AdminDashboard` page methods (30 `[WebMethod]`); no-data behaviour |
| **M14** Target and DWSP (`target-management`) | National/zone/area/territory targets (Entry/View), `TargetExcelUpload`, DWSP setup (`DWSP/AMDayWiseDWSPSetup` app page), achievement reports |
| **M15** HR, field force (`hr-field-force`) | Employee master/`EmployeeSetup`, leave application and yearly process (`LeaveProcess_UI`), attendance, holiday, tour plan, expense/mileage claims, notice board |
| **M16** Doctor, visits, prescriptions (`doctor-visit-prescription`) | Doctor master trio (category, chamber, degree, speciality), DCR/DCP/CVR, prescription (RX) list and images (`PrescriptionImages`), `sp_MIS_GetSalesmanCustomerVisitReport` output |
| **M17** SAP and external integration (`sap-external-integration`) | `MIGO_Upload`, `SAP_Integration/*` pages, provider-dropout integration list and approval (`Library.DAL/SAP_IntegrationDAL/sql/*.sql`), API-customer import, mobile-app web methods |
| **M18** Approval workflow (`approval-workflow`) | Each `Approval_UI` list: filters (market-structure pickers), approve/reject/step actions, `ClsApprovalAction` action-step enable/disable per user, permission gate (`UserPersmissionValidation`, role id 2 bypass) |

Cross-cutting checks to repeat every release: build (`aspnet_compiler`) succeeds; login page loads; DB connection is to the intended instance (four places); Crystal viewer renders one report; Excel import of one template succeeds; one Entry/View/Edit trio round-trips; no new `.exclude` or `Copy of` artefacts were added to the deployed tree.
