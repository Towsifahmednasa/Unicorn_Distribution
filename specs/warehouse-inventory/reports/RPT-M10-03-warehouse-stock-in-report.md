# RPT-M10-03 Warehouse stock-in document / report

**List page:** `SInventory_UI/WHStockInReportList.aspx` (PAGE-396); also print icons on `WarehouseStockInView`, `WarehouseStockInApproval`, `MaterialConversionReceiveView`.
**Viewer:** `SInventory_RPTVIEW/WHStockInReportViewer.aspx` (PAGE-354). **Crystal file:** `Solution.Web/Reports/CrystalReports/crpWHStockIn.rpt`; product-wise variant uses `rptWHDCStock.rpt` / `rptWHDCStockExcel.rpt`.

## Flow
1. `WHStockInReportList`: date filter -> `WHStockInReportBll.LoadWarehouseStockInData(date)` -> `WHStockInReportDal.GetWarehouseStockInData`: `SELECT * FROM tblWHStockInMaster WHERE Status='Approved' AND WHStockInDate='<date>'` (string-concatenated). Row print sets `Session["ReportType"]="STD"`, `Session["Excel"]=0` and opens `WHStockInReportViewer.aspx?reqId=<WHStockInMasterID>` (`WHStockInReportList.aspx.cs:60-75`). A product-wise button sets `Session["ReportType"]="CSR"`, `Session["ProductCode"]`.
2. Viewer (`STD`): `WHStockInReportBll.LoadWarehouseStockInMasterData(reqId)` / `LoadWarehouseStockInDetailData(reqId)` (`WHStockInReportDal.cs:21-37`: master joined to manufacturer/supplier, detail joined to `tblProduct` and `tblStockUOM`) + `CompanyInfoBLL` (`tblCompanyInfo WHERE CompanyId='1'`) -> dataset -> `crpWHStockIn.rpt`; Excel when `Session["Excel"]` is non-zero (`ExportFormatType.ExcelRecord`, file name `Stock Detail Report`).
3. Viewer (`CSR`): `DCStockReportBLL.WHReportDetailDataDAL(Session["ProductCode"])` -> `rptWHDCStock.rpt` / Excel variant.

## Parameters
QueryString `reqId`; Session `ReportType`, `Excel`, `ProductCode`; list filters: stock-in date, product.

## Notes
Only `Approved` documents appear on the list page; the print icons on the View/Approval pages allow printing `Posted` ones. Company header hard-coded to `CompanyId='1'`.
