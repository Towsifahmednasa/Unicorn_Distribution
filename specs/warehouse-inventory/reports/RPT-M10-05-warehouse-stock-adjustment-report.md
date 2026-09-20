# RPT-M10-05 Warehouse stock adjustment list report

**Opener:** `WHStockAdjustmentView.aspx` (`reportData` command) -> `../SInventory_RPTVIEW/WHStockAdjustmentListReportViewer.aspx?WHStockAdjId=<id>` (PAGE-353).
**Crystal file:** `Solution.Web/Reports/CrystalReports/crpWhStockAdjustmentListInfo.rpt`.
**Data:** `WHStockAdjDAL.GetWhStockAdjustmentListReportInformation(id)` (`WHStockAdjDAL.cs:28-37`): `tblWHStockAdjustment LEFT JOIN tblAdjustmentType, tblWearhouse, tblWHAdjustmentDetail dtl, tblCentralStore CSS ON CSS.ReceiveId = dtl.ReceiveId WHERE WHStockAdjId=<id>` -> ProductName, ProductCode, Quantity, AdjustmentType, TransactionNo, TransactionDate, Remarks and `(CSS.UnitPrice*dtl.Quantity)` aliased as `WearhouseName` (amount stored in a misnamed column).
**Parameters:** QueryString `WHStockAdjId` (string concatenated into SQL). Company header via `OrderReportBLL.CompanyInfoBLL`. Excel export line commented out.
