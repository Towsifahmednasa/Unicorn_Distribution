# RPT-M10-06 Warehouse stock monitoring and Central Warehouse monthly inventory

**List pages:** `SInventory_UI/WhStockMonitoringReport.aspx` (PAGE-397), `SInventory_UI/MonthlyWarehouseReportCW.aspx` (PAGE-373), `SInventory_UI/WareHouseChalanReportList.aspx` (PAGE-384, picking chalan list).
**Viewers:** `SInventory_RPTVIEW/WhStockMonitoringReportViewer.aspx` (PAGE-355) -> `crpWhStockMonitoring.rpt`; `SInventory_RPTVIEW/CWMonthlyInventoryReport.aspx` (PAGE-346) -> `crpCWMonthlyInventoryReport.rpt` (`Solution.Web/Reports/CrystalReports/`, source copy `Library.CrystalReports/SInventory_RPT/`); chalan list opens `WareHouseChalanReportViewer.aspx?reqId=` (viewer not in M10 folder).

## Warehouse stock monitoring
`WhStockMonitoringReport.aspx.cs` -> `WhStockMonitoringReportBll.LoadWhStokMonitoringInformation(from,to)` -> `WhStockMonitoringReportDal.GetWhStokMonitoringInformation` (`Library.DAL/SInventory_DAL/WhStockMonitoringReportDal.cs:15-19`), one concatenated query per product:
- `OpeningStockQty`, `OpAmount` = `tblCentralStore_OpeninigBalance` (sic) rows where `CSOpeninigBalanceDate = '<fromDate>'` (**exact date match only**, `SUM(Quantity)`, `OpAmount = SUM(Quantity)*UnitPrice`);
- `StockInQty` = `SUM(tblCentralStore.StockInQty)` where `ReceiveDate BETWEEN from AND to`;
- `IssueQty` = `SUM(tblStockInTransfar.PickingQty)` for requisitions with `PickingDate BETWEEN from AND to`;
- `ClosingBal` = Opening + StockIn - Issue; `saleQty`, `saleAmount` from `tblInvoiceDetail` with `DeliveryStatus IN ('Full','Partial')` and `tblInvoice.UpdateDate BETWEEN`; `ClosingAmount` hard-coded `0`.
Viewer parameters: QueryString `fromDate`, `toDate`; report shows the same rows; Excel export supported.
Limitations: no roll-forward of opening (needs an opening-balance row on the from date), warehouse stock-outs / adjustments / freezes are not deducted (they are not in the formula), amounts not computed.

## CW monthly inventory
`MonthlyWarehouseReportCW.aspx.cs:44` opens `CWMonthlyInventoryReport.aspx?fromDate=&toDate=` -> `CWMonthlyInventoryReportBll.LoadCWMonthlyInventoryReport` -> `CWMonthlyInventoryReportDal.GetCWMonthlyInventoryReport` -> SP `sp_WHBeenCard(@fromDate,@toDate)` (body absent). The older inline query (commented, `CWMonthlyInventoryReportDal.cs`) shows the intended columns: ReceiveFromProduction, OpeningStock, TotalIssued, ClosingStock, per-DC issue columns (hard-coded ComUnitId 1-12), OtherIssueSample (`tblWHStockOutDetail`), OtherIssueDamage (`tblWhStoreFreeze`).

## Chalan list
`WareHouseChalanReportList.aspx` -> `WarehousePickingBll.StockTransportPickingGridDataBLL(date)`; opens `WareHouseChalanReportViewer.aspx?reqId=` (`.rpt` not resolved).
