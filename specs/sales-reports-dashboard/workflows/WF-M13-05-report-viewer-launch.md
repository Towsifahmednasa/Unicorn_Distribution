# WF-M13-05 List page -> popup viewer -> Crystal export

_Module M13, reverse-engineered 2026-09-19 (documentation only). Confidence tags as in the page specs._

## Trigger and actors
User of a list page (e.g. AllSalesReport, CompanySalesReport, SalesTrendReport, BranchTotalSummary) clicks View/Excel.

## Steps
1. List page validates dates (defaults To = From when blank) and, for national/DC reports, sets Session flags (`SalesReport`, `Excel`, `parameter`, `RptParam`, `rpt`).
2. `ScriptManager.RegisterStartupScript` runs `window.open("../SInventory_RPTVIEW/<Viewer>.aspx?fromDate=..&toDate=..&<ids>")` (950x700 popup).
3. Viewer `Page_Init` (no master page, no session check) reads QueryString/Session, calls BLL/DAL (inline SQL or SP), sets `DataTable.TableName` to the dataset table expected by the .rpt.
4. Two output modes: (a) `rptdoc.Load(Server.MapPath("~\\Reports\\CrystalReports\\<name>.rpt"))` + `ExportToHttpResponse(ExportFormatType.ExcelRecord, ...)` = immediate .xls download (BusinessSummary, AllSales, MIOProduct, Excel viewers); (b) typed report (`Library.CrystalReports/SInventory_RPT`) or .rpt bound to `crvSalesRpt` for on-screen display (entity sales, trend, day-wise, comparison, historical).
5. Unload/Disposed handlers close the ReportDocument.

## Lineage
Full page -> viewer -> .rpt -> SP tables: `reports/RPT-M13-02` .. `RPT-M13-05`.

## Exceptions
No rows -> "No Data Found!!"; missing .rpt (`crpSalesTrendExcelReport.rpt`) -> runtime error; missing Session["rpt"] -> NullReferenceException (ProductWiseNational).

## Related
BR-M13-050..068; GAP-M13-005, -016..-020.

