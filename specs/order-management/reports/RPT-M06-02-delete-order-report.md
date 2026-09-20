# RPT-M06-02 — Delete Order report (deleted orders audit)

| Item | Value |
|---|---|
| List/filter page | `Solution.Web/SInventory_UI/DeleteOrderReport.aspx(.cs)` (PAGE-213; 173 lines) |
| Viewer | `Solution.Web/SInventory_RPTVIEW/DeleteOrderReportViewer.aspx(.cs)` (PAGE-211; 87 lines) |
| .rpt | `Solution.Web/Reports/CrystalReports/crpDelOrderReport.rpt` (on-screen, `Session["Excel"]=="N"`) and `crpDelOrder.rpt` (Excel export via `ExportFormatType.ExcelRecord`, `"DeleteOrderReport"`) |
| Dataset | `DeleteOrderDataTable` |
| BLL/DAL | `DeleteOrderReportBll.LoadDeleteOrderReport / LoadDeleteOrderNationalReport` → `DeleteOrderReportDal.GetDeleteOrderReport / GetDeleteOrderNationalReport` (`Library.DAL/SInventory_DAL/DeleteOrderReportDal.cs:28-40`) |

## Parameters
Passed through Session (not QueryString): `Session["ComUnitCode"]` (sales-center dropdown text/value), `Session["FromDate"]`, `Session["ToDate"]`, `Session["Excel"]` ("N" screen / "Y" Excel); `Request.QueryString["rptType"]` (`"1"` = regional/DC report, otherwise "national"). `Session["Excel"].ToString()` in the viewer throws if the session key is missing (direct URL access).

## SQL (parametrised)
```
SELECT OD.OrderCode, OD.ComUnitCode AS SalesCenterCode, OD.ComUnitName AS SalesCenterName, OD.MIOCode, OD.MIOName,
       VC.AreaName AS TeritoryName, VC.AreaCode AS TeritoryCode, VC.DistrictCode AS FECode, VC.DistrictName AS FEName,
       VC.RegionCode AS DZSMCode, VC.RegionName AS DZSMName, OD.CustomerCode, OD.CustomerName, OD.GrossValue,
       OD.SubmissionDate, OD.DeleteBy, OD.DeleteDate
FROM tblOrderDel OD WITH(NOLOCK) INNER JOIN dbo.View_CustomerMaster VC ON VC.CustomerCode = OD.CustomerCode WHERE 1=1
  [AND OD.ComUnitCode=@ComUnitCode] [AND OD.DeleteDate BETWEEN @FromDate AND @ToDate]
```
The regional and national methods use **identical SQL** (the only difference is the caller's `rptType`), so "national" is not actually cross-DC unless the filter is left blank. `INNER JOIN View_CustomerMaster` drops deleted orders whose customer no longer resolves. `FromDate/ToDate` are strings compared to `DeleteDate` (date-only end date excludes deletions later on the end day — INFERRED).

## Source of rows
`tblOrderDel` is populated by `sp_DeleteOrder` (PAGE-222) — INFERRED, SP body absent (GAP-M06-020).

## Output
Crystal viewer (popup 950x700) or `.xls` download.

## Gaps
GAP-M06-026 (identical national/regional queries; inner join loss; session-coupled parameters).
