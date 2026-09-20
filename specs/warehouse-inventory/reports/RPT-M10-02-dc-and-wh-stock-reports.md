# RPT-M10-02 DC / warehouse stock position reports (Crystal + list pages)

**List pages:** `SInventory_UI/DCStockReport.aspx` (PAGE-364), `SInventory_UI/StockReport.aspx` (PAGE-383), `SInventory_UI/BatchLifecycleReport.aspx` (PAGE-359), `SInventory_UI/CountryStockReport.aspx` (PAGE-361), `SInventory_UI/DCWiseCountryStockReport.aspx` (PAGE-366).
**Viewers:** `SInventory_RPTVIEW/DCStockReportViewer.aspx` (PAGE-349), `CountryStockReportViewer.aspx` (344), `AllProductCountryStockReportViewer.aspx` (343), `DCwiseCounrtyStockReportViewer.aspx` (350), `CurrentStockRpt.aspx` (345).
**Crystal files:** `Solution.Web/Reports/CrystalReports/rptDCStock.rpt`, `rptWHDCStock.rpt`, `rptWHDCStockExcel.rpt` (also `Library.CrystalReports/SInventory_RPT/rptDCStock.rpt`). Country/current-stock viewers use typed report classes (`aCountryStock`, `aWithoutProductCodeCountrySales`, `aDCwiseCountryStock`, `aRptCustomerMaster`) - .rpt names not resolved (UNKNOWN).

## DC / WH stock report (main family)
- List page sets `Session["DC"]` (1 = DC report, 0 = warehouse report) and `Session["ReportView"]`, opens `DCStockReportViewer.aspx?comUnitId=<id>` (`DCStockReport.aspx.cs:80-100`; `StockReport.aspx.cs:68-94`).
- Viewer: `DCStockReportBLL.DCReportDetailDataDAL(comUnitId)` (SQL in `DCStockReportDAL.cs:36-300`), `DCReportMainDataDAL` (`select * from tblCompanyUnit where ComUnitId=@ComUnitId`), company header via `OrderReportBLL.CompanyInfoBLL`; WH branch `WHReportDetailDataDAL()` (`DCStockReportDAL.cs:310-385`, with product variant `:386-`) -> `rptWHDCStock.rpt`; Excel export `ExportFormatType.ExcelRecord`.
- **DC detail query columns:** ProductCode, ProductName, PackSize, BatchNo, MfgDate, ExpDate, `AQty` (Available), `BookFDel` (booked for delivery), `Tqty` (in transit), `RQty` (Restricted), `BQty` (Blocked), per DC. Built as UNION ALL of: available `tblDCStore` rows (`StockQty>0 AND StockCondition='Available'`) joined through three provenance paths (WH stock-in chain `tblStockInTransfar -> tblCentralStore -> tblWHStockInDetail`, `tblChalanDetail`, `tblSubDepotChalanDetail`); undelivered invoices (`tblInvoice.DelivaryInvoiceNo IS NULL`) and undelivered challans (`tblChalanInfo.IsDeliver IS NULL`) as BookFDel; `tblDCStoreFreeze` rows with `StockQty>0` and `StockCondition='Restricted'` / `'Blocked'`; requisition stock in transit from `tblStockInTransfar`.
- Stock model implication: available quantity is taken from `tblDCStore.StockQty>0` rows only (see GAP-M10-007 note on ledger-style negative Issue rows).

## Other reports in the family
| Page | Logic | Source |
|---|---|---|
| `BatchLifecycleReport.aspx` | user must be logged in (`Session["UserId"]`); DC filter; calls SP `sp_Get_BatchLifecycleReport` directly with a `SqlCommand` (`BatchLifecycleReport.aspx.cs:184`); body absent | UNKNOWN columns |
| `CountryStockReport.aspx` -> `CountryStockReportViewer` | `productCode` query string; `CountryStockReportDAL`: `select * from View_TotalCurrentStockofCompanyWithStockInTransfar where ProductCode=@ProductCode`; all-products variant `View_CentralStoreCurrentStock` | views not in repo |
| `DCWiseCountryStockReport.aspx` -> `DCwiseCounrtyStockReportViewer` | `DCStockReportBLL.DCWiseCountryReportDetailDataDAL()`; `View_ProDCStock` (`DCStockReportDAL.cs:307`) | view body absent |
| `CurrentStockRpt` viewer | `CustomerMasterBLL.CustMasterReport(ComUnitId)` (customer-master data used as "current stock" - naming mismatch) | mismatch |

## Parameters
`comUnitId` (QueryString), `productCode` (QueryString), Session `DC`, `ReportView`, `ProductCode`, `Excel`, `ReportType`.

## Known issues
Very large hand-written UNION queries with commented legacy joins; INNER JOIN chains drop stock that cannot be traced to a WH stock-in or challan; views/SPs absent; `CurrentStockRpt` viewer binds customer-master data.
