# RPT-M13-02 Business Summary and Branch/Market Excel exports (BusinessSummaryViewer 1-5)

_Module M13. Documentation only, generated from source 2026-09-19. Confidence: CONFIRMED unless tagged. SP bodies are not in the repository._

All five viewers are stand-alone pages (no master page, no session check) whose `Page_Init` reads `fromDate`/`toDate` (and `Branch`) from the QueryString, builds one DataTable, loads a .rpt from `Solution.Web/Reports/CrystalReports/` (`Server.MapPath("~\\Reports\\CrystalReports\\"+name)`) and calls `ExportToHttpResponse(ExportFormatType.ExcelRecord, ...)` - i.e. **the popup only downloads an .xls; nothing is rendered**. (`crvSalesRpt` viewer control is declared but unused.)

| List / entry page | Viewer / output page | .rpt or output | Dataset / DataTable name | BLL -> DAL method | SP / inline SQL | Request / session parameters | List-page filters | Tables |
|---|---|---|---|---|---|---|---|---|
| TotalSummary 531 (`excelButton1`) | BusinessSummaryViewer 469 | crpBusinessSummary.rpt (`Solution.Web/Reports/CrystalReports/crpBusinessSummary.rpt`) | BsummaryDataTable | CmnCrystaltoView.BusinessSummaryReportDAl | SP sp_Get_BusinessSummaryReportList | fromDate,toDate | dates only | UNKNOWN (SP) |
| BranchTotalSummary 493 | BusinessSummaryViewer2 470 | rptBranchSales.rpt | Bsales1 | TotalSummaryDAL.BranchwiseTotalSalesBLL(from,to[,Branch]) | inline SQL: delivered lines (Partial/Full) per depot ('Kustia' literal) and sub-depot ('SubDepo') per UpdateDate: NumberofInvoiceSold, SumofNetSalesAmount = SUM(DeliveryNetAmount-DeliveryTotalPriceVatAmount), DataColumn2 = SUM(DeliveryTotalPriceVatAmount) | fromDate,toDate,Branch(ComUnitId) | dates, sales centre | tblCompanyUnit, tblInvoice/Detail, tblSubInvoiceMaster/Detail |
| BranchTotalSummary 493 | BusinessSummaryViewer3 471 | rptBranchSales.rpt | Bsales1 | BranchwiseTotalProformaBLL | inline SQL: proforma per InvoiceDate: SUM(NetAmount)-SUM(TotalPriceVatAmount), VAT | same | same | same |
| BranchTotalSummary 493 | BusinessSummaryViewer4 472 | rptMarketwiseBusinessSummary.rpt (`Solution.Web/Reports/CrystalReports/rptMarketwiseBusinessSummary.rpt`, +.cs typed report) | MarketwiseSummaryDS | MarketBranchwiseTotalProformaBLL | inline SQL as Viewer3 grouped by MarketName, MarketCode, MIACode, MIAName, AreaCode, RegionCode, DisCode | same | same | same |
| BranchTotalSummary 493 | BusinessSummaryViewer5 473 | rptMarketwiseBusinessSummary.rpt | MarketwiseSummaryDS | saleshwiseTotalProformaBLL(from,to,Branch) / salesBranchwiseTotalProformaBLL(from,to) | inline SQL: delivered net (UpdateDate) by market/MIO | same | same | same |

Notes: (1) `Branch != ""` test - a missing Branch is null and takes the "with branch" path with a NULL parameter (`SInventorySql.DbValue`), returning no rows (INFERRED). (2) Viewer2-5 use `TotalSummaryDAL` (the class in `SInventory_DAL`), not the near-identical `MIOTotalSummaryDAL` copy. (3) Depot/sub-depot literals 'Kustia'/'SubDepo' are hard-coded labels. (4) Rules: BR-M13-032, BR-M13-060, BR-M13-068. (5) Response is `Content-Disposition` download named `BusinessSummary_Report` / `MarketwiseBusinessSummary_Report`.

