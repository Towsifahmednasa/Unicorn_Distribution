# RPT-M11-02 — Stock Transport Order (STO) report

| Item | Detail |
|---|---|
| List / launcher | `SInventory_UI/StockTransportOrderReportList.aspx` (PAGE-422: lists requisitions, opens viewer per `reqId`, `.aspx.cs:53-55`); `StockTransferReport.aspx` (PAGE-421 "Stock Transfer Report", Tier B) |
| Viewer | `SInventory_RPTVIEW/StockTransfarOrderReportViewer.aspx(.cs)` (PAGE-404) |
| Report file | `rptStockTransfarOrder.rpt` (`StockTransfarOrderReportViewer.aspx.cs:35`) |
| Dataset tables | `mainDataTable`, `detailDataTable`, `companyInfoDataTable` |
| Request parameters | QueryString `reqId` |
| Data source | `StockTransportOrderReportBLL.StockTransportOrderReportMainDataBLL(reqId)` / `...DetailDataBLL(reqId)` → `StockTransportOrderReportDAL` (bodies not opened; requisition + child data), `OrderReportBLL.CompanyInfoBLL()`; list page `RequisitionDAL.StockTransportOrderGridDataDAL(date)`: `SELECT * FROM tblRequisition WHERE ReqDate=@ReqDate ORDER BY ReqId DESC` |
| Output | Crystal viewer |
| Confidence | viewer CONFIRMED; DAL/query text of the STO report main/detail UNKNOWN; Tier-B list pages not reviewed in depth |
