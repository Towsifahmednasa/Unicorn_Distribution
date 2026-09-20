# RPT-M10-09 Monthly inventory (DC/area) reports

**Pages:** `SInventory_UI/MonthlyInventoryReport.aspx` (PAGE-372), `SInventory_UI/AreaWiseMonthlyInventoryReport.aspx` (PAGE-358).
**MonthlyInventoryReport:** user selects DC (`OtherStockActionBLL.LoadSC(userId)` - only the user's units), product type (`ProductBLL.LoadProductType_new`), from/to dates. `InvoiceDAL.GetMonthlyInventoryReportDAl(districtId, from, to, proTypeId)` (`InvoiceDAL.cs:158-`) executes an SP with parameters `@fromDate, @toDate, @CiD, @ProTypId` (name in spec: `sp_Get_MonthlyInventoryReport`; body absent). Export button `btnExport_Click` writes Excel. The same page also contains handlers that open `InTransitReportViewer.aspx` (in-transit/national variants) - copy of another report's code.
**AreaWiseMonthlyInventoryReport:** `OrderInfoBLL.LoadSC(branchDropDown, userId)`; opens `SInventory_RPTVIEW/AreaWiseMonthlySalesReportViewer.aspx?fromDate&toDate&branchId&national` (a *sales* viewer, not an inventory one - naming mismatch).
**Parameters:** dates as text (culture), `districtId`/`branchId`.
**Notes:** logic UNKNOWN (SP absent); spec-vs-name mismatch for the area-wise page.
