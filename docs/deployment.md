# Deployment (as evidenced by the repository)

Documentation only. All statements are derived from files in the repo; the actual production topology and procedures are `UNKNOWN` unless a file proves them. Connection targets and credentials are redacted (`<redacted>`). Tags: `CONFIRMED`, `INFERRED`, `UNKNOWN`.

Related: [architecture.md](architecture.md) (sections 2, 14, 16), [security.md](security.md), [testing.md](testing.md).

## 1. Build

| Step | Evidence |
|---|---|
| Toolchain: Visual Studio 2010 solution (`Format Version 11.00`, `# Visual Studio 2010`), all projects target .NET Framework 4.0 | `Solution.sln:1-2`, csproj `TargetFrameworkVersion v4.0` |
| 4 class libraries built by MSBuild: `Library.DAO` -> `Library.DAL` -> `Library.BLL`; `Library.CrystalReports` builds `.rpt` + typed datasets (needs the Crystal Reports for Visual Studio runtime, references from the GAC without hint paths) | csproj files |
| Website (`Solution.Web`) is a **Web Site project**: no .csproj; the sln stores `ProjectReferences` to the three DLLs and the `aspnet_compiler` settings: virtual path `/Solution.Web`, physical path `Solution.Web\`, target `PrecompiledWeb\Solution.Web\`, `Updateable=true`, `ForceOverwrite=true`, `FixedNames=false`, Debug: `Debug=True`, Release: `Debug=False` (note: Release website configuration maps to `Debug|Any CPU` in the sln, so the website is always compiled with the Debug configuration) | `Solution.sln:3-24`, configuration mapping lines |
| Output `PrecompiledWeb/Solution.Web/` (updatable precompile: `.aspx` shells kept, code in `bin/App_Code.dll` plus `*.compiled` markers; `PrecompiledApp.config` `version="2" updatable="true"`) | `PrecompiledWeb/Solution.Web/PrecompiledApp.config` |
| Publish profile `Solution.Web/App_Data/PublishProfiles/FolderProfile.pubxml`: FileSystem publish, `LastUsedBuildConfiguration=Release`, `DeleteExistingFiles=False` (**publishing does not remove stale files**), `ExcludeApp_Data=False`, target a developer's Desktop folder (developer-specific path) | pubxml (CONFIRMED) |
| `Solution.Web/website.publishproj` (VS 10.5 publish targets, source path is a developer's machine path `...param_ePharmaWeb_Main\Solution.Web`, metabase `/IISExpress/7.5/LM/W3SVC/7/ROOT`) | file |
| NuGet: `Solution.Web/packages.config` (17 packages); libraries carry only System.Web.Http.Common. `packages/` folder is git-ignored; restore is needed before building. `DLL/` holds the three Enterprise Library 4.1 DLLs and is required by `Library.DAL` | `.gitignore`, `Library.DAL.csproj` |
| Dependency-version mismatch to watch: `packages.config` says EPPlus 4.1.0, `Solution.Web/Bin` contains EPPlus 6.0.3 with Web.config binding redirect to 6.0.3.0; `log4net` is listed in Web.config `<compilation><assemblies>` but no `log4net.dll` exists in the repo (a clean server may fail to compile the site unless the assembly is present in the GAC - `INFERRED`) | Web.config, `Solution.Web/Bin/` |
| Build outputs are committed: `PrecompiledWeb/` (3,015 files), `Solution.Web/Bin/` (54 DLLs), `Library.*/bin`, `obj` (2,830 tracked files under those folders) so **the repo can be deployed by copying folders**, but source and binaries can drift (`PrecompiledWeb` DLLs dated 2026-09-16, `Solution.Web/Bin/Library.DAL.dll` dated 2026-09-17) | `git ls-files`, file dates |
| Stray files: `_compilecheck.dll`, `CSC19F16BFD3D31408799D462CF15AEA679.TMP` (compiler temp), `Solution.sln.DotSettings.user` | root |

## 2. Configuration handling

- **No config transforms**: there is no `Web.Debug.config`/`Web.Release.config` and no build-time substitution. Environment switching is done by **commenting/uncommenting `<add name="SolutionConnectionStringSSIDB">` lines in `Web.config`** (14 commented alternates) and editing static fields in `Library.DAL/DataManager/SqlUserAccess.cs` and `Library.DAL/MAIN_FUNCTION/DB_Authentication.cs`, and `DataBase.SalesDB` in `DataBase.cs` (`INFERRED` from the alternates present). Because two of these are compiled into `Library.DAL.dll`, **a connection change needs a rebuild of Library.DAL** (the Web.config entry alone is not enough): four places must agree ([architecture.md](architecture.md) section 6.10).
- `Web.config` is git-ignored (comment "Contains DB credentials" in `.gitignore`) and therefore not versioned; copies exist in `Solution.Web/` and `PrecompiledWeb/Solution.Web/` and differ only in which connection entry is active.
- Settings that matter on the server: `compilation debug="true"` (should be reviewed for production), `httpRuntime executionTimeout=36000 maxRequestLength=4194303`, `system.webServer` `maxAllowedContentLength=4294967295`, `sessionState InProc timeout=24000`, `.apk` MIME map, `runAllManagedModulesForAllRequests=true`.

## 3. IIS / server requirements (inferred)

| Requirement | Evidence |
|---|---|
| Windows Server with IIS 7.5 or later, ASP.NET 4.0 (Framework 4.x) app pool, **integrated pipeline** (handlers under `system.webServer`, `validateIntegratedModeConfiguration=false`) | `Web.config`, `website.publishproj` metabase path |
| Application pool must be **32/64-bit compatible with the OleDb provider used for Excel import**: Jet 4.0 (`.xls`, 32-bit only) and ACE 12.0 (`.xlsx`) providers must be installed (15 upload pages) | `SInventory_UI/CustomerExcelUpload.aspx.cs:272-282` |
| SAP Crystal Reports runtime for .NET 13.0 (Bin has 13.0.16.1954 assemblies, Web.config pins 13.0.2000.0 via config sections, implying the runtime/redirect policy is installed on the server). Viewer static files served from `crystalreportviewers13/`; `CrystalImageHandler.aspx` registered; `CrystalImageCleaner-*` settings enabled | Web.config, `Bin/`, `Solution.Web/crystalreportviewers13/` |
| Write permission for the app-pool identity on `Solution.Web/ExcelFiles/`, `UploadFile/`, `Reports/CrystalReports/TempPE/` (Crystal temp assemblies), and read of `PrescriptionImages/` | code (SaveAs paths) |
| Outbound HTTPS to `ipapi.co` (login IP geolocation, 3 s timeout, failure ignored) | `UserSessionTrackingManager.cs:ResolveLocation` |
| Network path to SQL Server (named instance) and permission to run `msdb.dbo.sp_start_job` for the archive backup job | `ArchiveDbConnectRepository.cs` |
| InProc session: single web server or sticky sessions; recycle/deploy clears all sessions; long timeout (24,000 min) keeps memory in use | Web.config |
| A virtual application root `/Solution.Web` is assumed by tooling (`VirtualPath=/Solution.Web`); route `user-records` is mapped at app start (`Global.asax`). Static references use `~/` paths and relative `../` paths, so hosting under a different virtual path is supported for most pages (`INFERRED`) | `Solution.sln`, `Global.asax` |

## 4. Database deployment practice (root SQL scripts)

The repository root holds 17 SQL scripts (5,463 lines) that are applied **manually with SSMS (or sqlcmd) to the live database**; there is no migration tool (no Flyway/EF/DbUp), no `database/` folder, and stored-procedure bodies are otherwise not versioned (SP source exists only in these scripts; about 41 procedures in total).

| Script | Purpose (from header) | Style |
|---|---|---|
| `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql` (1,430 lines) | "single script" live update: 1) table schema updates (`IF NOT EXISTS (... sys.columns ...) ALTER TABLE ... ADD`, e.g. `tblCustMaster.TaggedSalesPersonIds`), 2) 13 `CREATE/ALTER` procedure statements | idempotent, `PRINT` progress |
| `Live_Deployment_Script_All_Updates.sql` (1,248 lines, dated 06-Sep-2026) | modules: customer payment (discount, AIT, delivery-charge settlement), customer aging and outstanding report, delivery sales return (delivery-charge deduction and account posting). STEP 1 schema changes, STEP 2 accounts connectivity rows in `CSTL_ERPDB_Unicorn` (`Table_Integration_AccountsConnectivity` flag/GL mapping), STEP 3 procedures via `CREATE OR ALTER PROCEDURE`, STEP 4 repeated schema guards | idempotent; needs SQL Server 2016 SP1+ for `CREATE OR ALTER` (instance names in config comments mention 2014 and 2019 editions: the target must be 2016+ for these scripts - `INFERRED`) |
| `SP_Fix_CentralStoreStoIssue.sql`, `SP_Fix_CentralStoreStoIssue_Corrected.sql` | rewrite of `tbl_INV_CentralStoreStoIssue` (cursor mapping fix). The existence of a "Corrected" second file shows fixes are iterated by hand | `DROP PROCEDURE IF EXISTS` then `CREATE` |
| `SP_Fix_STO_Chalan_Process.sql`, `SP_NewStockTransferDcToDc_Related.sql` | STO / challan / B2B transfer issue, receive, delete-rollback fixes (contain `DELETE FROM` statements) | mix of DDL and data repair |
| `SP_SalesReturn_Related.sql`, `SP_Update_StockValuation_TransactionDate_Logic.sql`, `sp_Delete_ProformaInvoice_Update.sql` | procedure updates for sales return, stock-valuation ledger, proforma delete | `ALTER PROCEDURE` / `CREATE` |
| `SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql`, `SP_Create_tblUnitPriceRollback.sql`, `SP_Fix_CustomerPayment_Live.sql` | new procedure/table/column (`tblCustomerPay.Remarks`) | guarded creation |
| `SP_Adjust_DCStore_Stock_Only.sql`, `sp_Check_DCStore_StockAnomalies.sql`, `SP_Audit_Sales_Accounts_Voucher_Posting.sql`, `SP_Fix_Invoice_Anomaly_And_Sync.sql`, `Create_Order_ODR-20260901134.sql` | data repair and audit (transactional for the order/adjust scripts) | one-off operations |

Practice observed: (1) date/name-stamped scripts named after the release ("LIVE_DB_UPDATE_<from>_TO_<to>", "Live_Deployment_Script_All_Updates"), (2) idempotent guards (`IF NOT EXISTS`, `IF OBJECT_ID`), (3) `USE [SalesDisDB_Unicorn_DB]` header, (4) comments partly in Bengali, (5) no version table, no rollback scripts (except `tblUnitPriceRollback` table for unit-price inactivation), (6) SP changes are applied **ahead of or together with** the web deployment since the code calls procedures by name; if the web build deploys before the SP script, pages fail at runtime (`INFERRED`), (7) cross-database objects (`CSTL_ACCDB_Unicorn`, `CSTL_ERPDB_Unicorn`) must exist and be reachable with the same login.

`Library.DAL/SAP_IntegrationDAL/sql/` holds 3 more scripts for the provider-dropout integration (`alter_tblProviderDropoutIntrigration_AddApprovalColumns.sql`, `sp_Get_ProviderDropoutIntrigrationList.sql`, `sp_Up_ProviderDropoutIntrigrationApprove.sql`).

## 5. Environments evidenced (targets redacted)

| Environment (inferred class) | Evidence | Target |
|---|---|---|
| Live/current | active entries in `Web.config` and `SqlUserAccess.cs`; DB name `SalesDisDB_Unicorn_DB` | SQL Server named instance on a private-LAN address, `<redacted>` |
| Developer workstation | `PrecompiledWeb/Solution.Web/Web.config` active entry; commented `PROGRAMMER25-PC\MSSQLSERVER2019`; publish profile path on a user Desktop | machine name, `<redacted>` |
| Other servers/VMs (commented) | 14 commented Web.config lines: named hosts (VM and "E-Program" DB app servers, `SERVER-01`), two public-IP hosts (one with a custom port), local `.`; instance names show `MSSQLSERVER2014` and `MSSQLSERVER2019` | `<redacted>` |
| Historical databases | `SalesDisDB_SMC`, `SalesDisDB_SMC_NEWDB`, `..._NewModule`, `..._InvoiceTest`, `..._TerritoryStucture`, `SalesDisDB_New3`, `SystemTest_Testing` | names only; the application lineage is a reused product (`SMC` customer -> `Unicorn`); the backup job default name `Society_DB_Backup_Job` and the tracker user-agent `SMC-Session-Tracker` are leftovers |
| Accounts / ERP DBs | `CSTL_ACCDB_Unicorn`, `CSTL_ERPDB_Unicorn` referenced by name in SQL and the audit script | same instance (cross-db names without linked servers) `INFERRED` |
| Login | live login is `sa` for the whole application; two commented entries use a lower-privileged web login | `<redacted>` |

Number of environments the code can distinguish at runtime: 1 (no environment flag; `SqlUserAccess.LiveServer` exists but is never read).

## 6. Android APK downloads

`Solution.Web/APK_File/` (tracked in git, 3 files, plus a copy under `PrecompiledWeb`): `E-Pharma.apk` (8.3 MB, dated 2021-12-15), `click-pharma.apk` (8.3 MB, 2021-12-15), `Unicorn_vr(1.0.0).apk` (14.1 MB, 2026-06-17). `Web.config` adds MIME `.apk = application/vnd.android.package-archive`; `Login.aspx:314` links `APK_File/Unicorn_vr(1.0.0).apk` as "Download APK File (1.0.0)". Deploying a new app version = replace the file in the site folder and edit the link (`INFERRED`). The old APKs stay downloadable by URL. The apps talk to `*Apps.aspx` pages and WebMethods; the mobile API server is not part of this repository (`UNKNOWN`).

## 7. Deployment procedure as best inferred (not documented in repo)

1. Backup the live database and the two sibling databases (`UNKNOWN` whether automated; the app can start an Agent backup job from `SettingPanel_UI/ArchiveDbConnect.aspx`).
2. Run the dated SQL script(s) from the repo root in SSMS on the live instance (idempotent).
3. Build in Visual Studio (Debug/Release both compile as Debug), publish to a folder or run `aspnet_compiler` to `PrecompiledWeb/Solution.Web`.
4. Copy the output to the IIS site folder (publish profile does not delete old files; `ExcelFiles/`, `UploadFile/`, `PrescriptionImages/` live inside the site folder and must not be overwritten or lost).
5. Verify `Web.config` connection entry and that `Library.DAL.dll` contains the intended `SqlUserAccess` values.
6. Recycle the app pool (sessions lost) and smoke-test login, one report, one import.

## 8. Known operational hazards

| # | Hazard | Evidence |
|---|---|---|
| 1 | Credentials and target duplicated in four places, two compiled into DLLs; changing environment requires editing code and rebuilding | [architecture.md](architecture.md) 6.10 |
| 2 | Site runs as `sa`; a leaked Web.config/DLL grants full instance control | [security.md](security.md) SEC-02 |
| 3 | Session state InProc + 24,000-minute timeout: every deploy/recycle logs everyone out and pending page state (report parameters kept in Session) is lost | Web.config |
| 4 | `compilation debug="true"` and updatable precompile: slower, larger memory, source `.aspx` markup is deployed | Web.config, `PrecompiledApp.config` |
| 5 | Publishing does not delete files (`DeleteExistingFiles=False`); removed pages remain live; 59 `.exclude` files and `Copy of` pages sit in the tree | `FolderProfile.pubxml` |
| 6 | Uploaded data (Excel, product images, prescriptions) lives inside the site folder: redeploying by wiping the folder loses it; it is also in git in part | file listing |
| 7 | Order of deployment: SP scripts vs code; SP bodies are not versioned other than these scripts, so the repository cannot rebuild the database (`UNKNOWN` baseline schema) | root scripts |
| 8 | Missing runtime prerequisites break specific features silently: no Crystal runtime (all 93 viewers), no ACE/Jet provider (15 imports), no `log4net` assembly (compile), no outbound HTTPS (geolocation is best-effort only) | code |
| 9 | Very long timeouts: command timeout 8,000 s (`DataAccessManager`), up to 99,000,000 s in `ClsCommonInternalDAL` older methods, `executionTimeout` 36,000 s: a blocked query can hold worker threads for hours | `DataAccessManager.cs:17`, `ClsCommonInternalDAL.cs` |
| 10 | 4 GB request limits allow very large uploads into the web root | Web.config |
| 11 | `DB_Manager` code paths use database `SalesDisDB_New3`; if absent on a new server, invoice / return / sub-depot flows using `CreateConnection_DAL()` fail | `InvoiceDAL.cs:207` |
| 12 | Single point of failure: one IIS server, one SQL instance for three databases, no health check or job monitoring in the code (`Application_Error` empty, no logging) | [architecture.md](architecture.md) 13 |
| 13 | Crystal image cleaner and temp assemblies in `Reports/CrystalReports/TempPE` are build artefacts committed to the tree; a stale `TempPE` can conflict with a rebuilt report assembly (`INFERRED`) | `Solution.Web/Reports/CrystalReports/TempPE/` |
| 14 | Git hygiene: `.gitignore` excludes `bin/`, `obj/` and `Web.config` but binaries (2,830 files) were already committed; `.gitignore` itself has an uncommitted change in the working tree | `git status` at start of review |
