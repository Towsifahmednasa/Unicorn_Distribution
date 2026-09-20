# RPT-M10-08 DC and warehouse opening stock reports

**List pages:** `SInventory_UI/DcOpeningStockReport.aspx` (PAGE-362), `SInventory_UI/WhOpeningStockReport.aspx` (PAGE-391).
**Viewers:** `SInventory_RPTVIEW/DcOpeningStockReportViewer.aspx` (PAGE-347) -> `crpDcOpeningStockReport.rpt`; `WhOpeningStockReportViewer.aspx` (PAGE-352) -> `WarehouseOpeningStockReport.rpt` (file not found under the searched paths - UNKNOWN location).
**Parameters:** QueryString `reportType`, `Date` (list page text box), Session `dcId` (DC page: all DCs vs one).
**Data (DC):** `DcOpeningStockReportDal.GetAllDcOpeningStockReport(date)` / `GetDcOpeningStockReport(dcId,date)` (`Library.DAL/SInventory_DAL/DcOpeningStockReportDal.cs:24-45`): `SELECT ComUnitCode, ComUnitName, ProductCode, ProductName, PackSize, BatchNo, MfgDate, ExpDate, StockQty FROM tblDCStore_OpeningBalance INNER JOIN tblCompanyUnit WHERE DCOpeningBalanceDate = @DCOpeningBalanceDate [AND ComUnitId=@ComUnitId]` (parameterised; exact-date match on a snapshot table).
**Data (WH):** `DcOpeningStockReportBll.LoadAllWhOpeningStockReport(date)` (central opening snapshot table `tblCentralStore_OpeninigBalance`, sic, per `WhStockMonitoringReportDal`) - SQL not read line-by-line (INFERRED).
**Output:** Crystal viewer + Excel (`ExportFormatType.ExcelRecord`, file name `CustomerMaster_Report`, copy-paste name).
**Notes:** the opening-balance tables are snapshots keyed by date; nothing in M10 pages creates them (populated by scripts/other modules - UNKNOWN).
