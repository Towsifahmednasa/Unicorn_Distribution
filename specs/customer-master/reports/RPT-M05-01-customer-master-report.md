# RPT-M05-01 — Customer Master report (Crystal → Excel)

## Pages
| Role | Page | Path |
|---|---|---|
| Launcher / filter page (PAGE-195) | `CustomerMasterReport.aspx` | `Solution.Web/SInventory_UI/CustomerMasterReport.aspx(.cs)` |
| Viewer / exporter (PAGE-180) | `CustomerMasterViewer.aspx` | `Solution.Web/SInventory_RPTVIEW/CustomerMasterViewer.aspx(.cs)` |

## Report file and dataset
- `.rpt`: `crpCustomerMaster.rpt` — loaded from `Solution.Web/Reports/CrystalReports/crpCustomerMaster.rpt` (source copy `Library.CrystalReports/SInventory_RPT/crpCustomerMaster.rpt`).
- Data set given to Crystal (`SetDataSource`): `custMasterDetailTable` (customer rows) + `companyInfoDataTable` (`SELECT CompanyName, Address, ContactNo, FaxNo FROM tblCompanyInfo WHERE CompanyId='1'` via `StockTransportOrderReportDAL.CompanyInformation`).
- Output: **exported directly** with `rptdoc.ExportToHttpResponse(ExportFormatType.ExcelRecord, Response, true, "CustomerMaster_Report")` — the viewer control is not used (commented code); the result is an Excel download named `CustomerMaster_Report`.

## Data source (inline SQL, `Library.DAL/SInventory_DAL/CustomerMasterDAL.cs`)
| `rptType` (QueryString) | Launcher option | BLL → DAL | SQL |
|---|---|---|---|
| `FBC` | Fixed Business Customer | `FixedCustMasterReport` → `FixedCustomerMasterReport` (`:77-81`) | `SELECT (DistrictCode+'-'+DistrictName)DistrictName,(RegionCode+'-'+RegionName)RegionName,(AreaCode+'-'+AreaName)AreaName,(MiaCode+'-'+MiaName)MiaName,(MarketCode+'-'+MarketName)MarketName,* FROM dbo.View_CustomerMaster WHERE FixedCustomer = 'true'` |
| `RC` | Regular Customer | `RegularCustMasterReport` → `RegularCustomerMasterReport` (`:83-87`) | same with `FixedCustomer = 'false'` |
| `AC` | All Customer | `CustMasterReport()` → `CustomerMasterReport()` (`:313-317`) | `SELECT * FROM dbo.View_CustomerMaster LEFT JOIN dbo.tblCustMaster ON View_CustomerMaster.CustomerMasterId = tblCustMaster.CustomerMasterId` |
(An overload `CustomerMasterReport(comUnitId)` filtering `ComUnitId=@ComUnitId` exists but is **not** called by the viewer.) `View_CustomerMaster` DDL is not in the repo (UNKNOWN columns/joins).

## Request parameters
- Launcher: `reportTypeDropDownList` value (`NULL` ⇒ alert "Please Select a report type !!!!"), opens `window.open('../SInventory_RPTVIEW/CustomerMasterViewer.aspx?rptType=<FBC|RC|AC>')`.
- Viewer: `Request.QueryString["rptType"]`; any other/empty value ⇒ empty table ⇒ page label "No Data Found!!" (no exception).
- Session: launcher reads `Session["UserType"]` (Admin ⇒ all DCs, else `Session["ComUnitId"]` for the DC dropdown; empty ⇒ redirect `Login.aspx`). **The DC dropdown and the "all" checkbox are not passed to the viewer**, so the export always contains all customers regardless of the DC selection or role.

## Filters on the list page
Only the report type. The DC selector is decorative (commented code in `viewRptButton_Click`).

## Output / export
Excel (`ExcelRecord`) of the `.rpt` layout; empty result ⇒ no file, message label.

## Business logic / notes
- No calculations; presentation concatenations `Code-Name` for district, region, area, MIA and market (legacy code-based hierarchy — customers created via the modern `CustomerEntry` may have blank legacy code columns and would appear with blank hierarchy, INFERRED).
- Fixed vs regular filter compares `FixedCustomer = 'true'/'false'` as string literals against a bit column.
- Report closes/disposes document in `Unload/Disposed` handlers.

## Gaps
No role/DC scoping applied to data (GAP-M05-024); viewer accessible by direct URL with no session check in code (`CustomerMasterViewer.aspx.cs` has none) — access relies on Forms authentication only.
