# RPT-M10-07 Distribution center stock monitoring

**List page:** `SInventory_UI/DistributionCenterStockMonitoring.aspx` (PAGE-369) - button opens `../SInventory_RPTVIEW/DistributionCenterStockMonitoringReportViewer.aspx?rptType=<type>`.
**Viewer:** PAGE-351. **Crystal files:** `crpDCStockMonitoringReport.rpt` (screen) and `crpDCStockMonitoringExcelReport.rpt` (Excel; file not found in the repository - referenced by `rptdoc.Load(ReportPath("crpDCStockMonitoringExcelReport.rpt"))`).
**Data:** `DistributionCenterStockMonitoringBll.LoadDcStockMonitoringInfo` -> `DistributionCenterStockMonitoringDal.GetDcStockMonitoringInfo` -> SP `sp_GET_AllStockProductwise` (no parameters; body absent). Sibling method `EmployeewiseProductSales` -> SP `sp_GET_EmployeeWiseProductSale`.
**Parameters:** QueryString `rptType` (`Excel` vs screen).
**Notes:** cannot verify columns or logic (SP absent). GAP-M10-013.
