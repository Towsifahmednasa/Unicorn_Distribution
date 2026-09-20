# System inventory

Counts computed with shell tools on 2026-09-19. File-type histogram excludes `PrecompiledWeb/`, `packages/`, `graphify-out/`, `.git/`, `obj/`, `bin/`, `docs/`, `specs/`. Details and evidence: [../architecture.md](../architecture.md).

## 1. Solution and projects

| Project | Path | Type | Framework | .cs files | LOC (.cs) | Notes |
|---|---|---|---|---|---|---|
| Solution.Web | `Solution.Web/` | Web Site (no csproj) | 4.0 | 749 (706 `.aspx.cs`, 24 `.ascx.cs`, 3 `.master.cs`, 4 App_Code, ...) | 194,415 (aspx.cs) + 11,795 (master/ascx/App_Code); 233,411 LOC in `.aspx` markup | 706 aspx, 24 ascx, 3 master, 3 ashx, 3 asmx |
| Library.BLL | `Library.BLL/` | class library | 4.0 | 104 | 12,950 | SInventory_BLL 96, SubDepot_BLL 6, Panal_BLL 1 |
| Library.DAL | `Library.DAL/` | class library | 4.0 | 205 | 69,389 | SInventory_DAL 112, DoctorModule_DAL 22, MasterSetup_DAL 18, DoctorMaster_DAL 9, ... |
| Library.DAO | `Library.DAO/` | class library | 4.0 | 195 | 6,324 | DoctorModule_DAO 80, SInventory_Entities 75, MasterSetup_DAO 15 |
| Library.CrystalReports | `Library.CrystalReports/` | class library | 4.0 | 124 | 103,717 (mostly generated) | 40 `.rpt`, 58 typed datasets (`.xsd`) |
| MVC_TreeGrid_Template | `MVC_TreeGrid_Template/` | orphan stub (ASP.NET Core code) | n/a | 2 (+1 cshtml) | 447 total | not in solution |
| Root SQL | `*.sql` | scripts | n/a | n/a | 5,463 (17 files) | plus 3 in `Library.DAL/SAP_IntegrationDAL/sql/` |

## 2. Technologies and package versions

| Technology | Version | Source |
|---|---|---|
| .NET Framework / C# / VS | 4.0 / C# / Visual Studio 2010 | csproj, sln |
| ClosedXML | 0.95.4 | `Solution.Web/packages.config` |
| DocumentFormat.OpenXml | 2.7.2 | same |
| EPPlus | 4.1.0 declared; 6.0.3 in `Bin/` and Web.config redirect | packages.config, `Bin/EPPlus.dll` |
| ExcelNumberFormat | 1.0.10 | packages.config |
| Newtonsoft.Json / .Schema / .JsonResult | 13.0.1 / 3.0.14 / 1.0.5778.10762 | packages.config |
| JSON (Json.dll) | 1.0.1 | packages.config |
| Microsoft.AspNet.Mvc / Razor / WebPages | 4.0.20710.0 / 2.0.20710.0 / 2.0.20710.0 | packages.config (unused by WebForms pages) |
| Microsoft.Bcl / .Async / .Build | 1.1.8 / 1.0.168 / 1.0.14 | packages.config |
| Microsoft.Web.Infrastructure | 1.0.0.0 | packages.config |
| OfficeOpenXml.Extends | 1.0.6.0 | packages.config |
| System.Web.Http.Common | 4.0.20126.16343 | all packages.config |
| Enterprise Library Common/Data/ObjectBuilder2 | 4.1.0.0 / 4.1.0.0 / 2.2.0.0 | `DLL/`, Library.DAL.csproj |
| SAP Crystal Reports for .NET | 13.0.2000.0 (config), 13.0.16.1954 (DLLs) | Web.config, `Bin/` |
| BouncyCastle.Crypto | 1.8.9 (no real usage) | `Bin/` |
| AjaxControlToolkit / DropDownCheckBoxes | 3.0.20820 / 1.1.0 | `Bin/` |
| log4net | 1.2.10.0 referenced in Web.config only; DLL absent | Web.config |
| SQL Server | instance name editions 2014/2019 in config comments; DB scripts need 2016+ (`CREATE OR ALTER`) | Web.config, root SQL |
| Excel import providers | Jet 4.0 / ACE 12.0 OLEDB | `CustomerExcelUpload.aspx.cs:272` |

## 3. Web folders and page counts (`.aspx`)

| Folder | Pages | Folder | Pages |
|---|---|---|---|
| SInventory_UI | 325 | Reports_UI | 16 |
| DoctorModule_UI | 101 | DoctorMaster_UI | 16 |
| SInventory_RPTVIEW (Crystal viewers) | 93 | TransferUI | 13 |
| MasterSetup_UI | 42 | Approval_UI | 12 |
| SubDepot_UI | 19 | DWSP | 10 |
| DoctorVisit_UI | 10 | PromoAlloc | 6 |
| Target_UI | 5 | Thana_UI | 5 |
| LeaveProcess_UI | 4 | NoticeBoard_UI | 4 |
| SAP_Integration | 4 | CommonUI | 7 |
| Dashboard_UI | 3 | SettingPanel_UI | 2 |
| UserPermission | 2 | UserProfile_UI | 2 |
| Money_Receipt_UI | 1 | UserTracking | 1 |
| eProgram_UI | 1 | root (Login, SixTenProjectReportApps) | 2 |
| **Total** | **706** | | |

Master page use: NewMasterPage 435, MainMasterPage 130, MasterPage 8, none 133. Disabled pages: 59 `.exclude` files.

## 4. Static assets

| Folder | Content |
|---|---|
| `Solution.Web/VerticalAsset/` | current Bootstrap 5 admin theme, jQuery 3.5.1, DataTables (+buttons, jszip, pdfmake), SweetAlert, toastr, jsPDF, fancybox; plugins apexcharts, chartjs, highcharts, select2, fullcalendar, gmaps, vectormap, datetimepickers, metismenu, simplebar |
| `Solution.Web/assets/` | second template set: jquery-3.6.0, jquery-ui, ~40 page scripts (ace, dropzone, tinymce, cropper, fullcalendar...), plugins, vendors |
| `Solution.Web/scripts/`, `css/` | legacy theme (jQuery 1.7.2, jQuery UI 1.8.19, flexslider, layerslider, isotope), `GV.css`, `custom.css` |
| `Solution.Web/crystalreportviewers13/` | Crystal viewer runtime, 23 language string files, ActiveX print `.cab` |
| `Solution.Web/CustomScript/` | `_myCusGen_Func.js`, `_QuickDataAccess.js` |
| Totals (excl. bin/obj/PrecompiledWeb) | 512 `.js`, 159 `.css`, 516 `.svg`, 294 `.gif`, 260 `.png`, 58 `.jpg`, 4 `.ttf`, 3 `.woff`, 2 `.woff2`, 3 `.eot` |

## 5. Web.config keys (values redacted)

`appSettings`: `CrystalImageCleaner-AutoStart`, `-Sleep`, `-Age`, `UnobtrusiveJavaScriptEnabled` (optional, read in code but absent: `DatabaseBackupConnectionStringName`, `DatabaseBackupJobName`). `connectionStrings`: `SolutionConnectionStringSSIDB` = `<redacted>` (+14 commented). Sections: `authentication` (Forms, timeout 2880), `sessionState` (InProc, 24000), `httpRuntime`, `compilation` (debug true, 13 assembly entries), `membership/profile/roleManager` (default providers, `ApplicationServices` undefined), `httpHandlers`/`handlers` (`CrystalImageHandler.aspx`), `staticContent` (`.apk`), `requestFiltering`, `businessObjects/crystalReports`, `assemblyBinding` (Newtonsoft.Json, System.Runtime, System.Threading.Tasks, EPPlus), `jsonSerialization maxJsonLength`.

## 6. External DLLs (`DLL/` and `Solution.Web/Bin/`)

`DLL/` (3): Microsoft.Practices.EnterpriseLibrary.Common, .Data, ObjectBuilder2. `Solution.Web/Bin/` (54 DLLs): the four Library.* assemblies, 14 CrystalDecisions assemblies, ClosedXML, DocumentFormat.OpenXml, EPPlus (+Interfaces, System.Drawing), ExcelNumberFormat, BouncyCastle.Crypto, Newtonsoft.Json (+Schema, JsonResult), AjaxControlToolkit, DropDownCheckBoxes, Json, OfficeOpenXml.Extends, Microsoft.Threading.Tasks(+Extensions), System.IO/Runtime/Net.Http/Threading.Tasks, Microsoft.Web.Infrastructure. `PrecompiledWeb/Solution.Web/bin/`: 184 files.

## 7. Handlers, services, routes

| Item | Path | Purpose |
|---|---|---|
| `PictureHandler.ashx` | root | JPEG from `Session["ImageBytes"]` |
| `SignatureHandler.ashx` | root | JPEG from `Session["SigImageBytes"]` (tests wrong key) |
| `HandlerDocCV.ashx` | `SInventory_UI/` | product image upload to `~/UploadFile/` |
| `SInventoryWebService.asmx` (x3) | `SInventory_UI/`, `MasterSetup_UI/`, `SubDepot_UI/` | all point to `App_Code/SInventoryWebService.cs` (23 autocomplete methods) |
| `[WebMethod]` | 119 files | 486 attributes (402 without session, 84 with) |
| Route | `user-records` | -> `~/DoctorModule_UI/UserRecords.aspx` |
| `CrystalImageHandler.aspx` | Web.config | Crystal viewer images |

## 8. Reports

112 `.rpt`: 72 in `Solution.Web/Reports/CrystalReports/` (loaded by path), 40 in `Library.CrystalReports/SInventory_RPT/` (compiled wrappers); 58 typed datasets; 93 viewer pages (`SInventory_RPTVIEW`); 72 code-behind files call `ReportDocument.Load`; 137 export calls; EPPlus Excel export on 8 pages, ClosedXML on 18 files; 72 build artefacts in `Reports/CrystalReports/TempPE/`.

## 9. File-type histogram (4,648 files)

| Ext | Count | Ext | Count | Ext | Count |
|---|---|---|---|---|---|
| cs | 1,379 | js | 512 | xls | 38 |
| aspx | 707 | svg | 516 | xlsx | 18 |
| gif | 294 | png | 260 | cab | 23 |
| css | 159 | dll | 130 | rpt | 112 |
| jpg | 58 | exclude | 59 | xsd | 58 |
| xss / xsc | 57 / 57 | html | 44 | refresh | 25 |
| ascx | 24 | xml | 21 | sql | 20 |
| pdb | 7 | config | 6 | apk | 3 |
| csproj | 4 | master / asmx / ashx / asax | 3 each | others | < 6 each |

Other: 45 files in `Solution.Web/ExcelFiles/`, 33 in `UploadFile/`, 9 in `PrescriptionImages/`, 3 APKs, 9 sample files in `Pro_Info/`. Git tracks 7,359 files, 2,830 of them build output (`PrecompiledWeb`, `bin`, `obj`).

## 10. Data-access and code metrics

| Metric | Value |
|---|---|
| Distinct stored-procedure name literals in code | about 1,023 (`sp_Get*` 495, `sp_Save*` 123, `sp_Update*` 63, `sp_Delete*` 47, `sp_check*` 55) |
| DAL files by access style | DataAccessManager 76, ClsCommonInternalDAL 119, SInventorySql 84, DB_Manager 8 (overlapping) |
| `PrimaryKeyMax` (MAX+1) call sites | 150 in 59 files |
| `new SqlParameter(` in DAL | 5,801 |
| SQL-shaped concatenation lines | DAL 334 (28 files), web 556 (111 files) |
| Pages using `Library.DAL` directly / `Library.BLL` | 274 / 429 |
| Static DAL/BLL fields on pages | 366 |
| Distinct `Session["..."]` keys | 93 |
| Cross-db references | `CSTL_ACCDB_Unicorn` 39, `CSTL_ERPDB_Unicorn` 17 |
| Automated tests | 0 |
