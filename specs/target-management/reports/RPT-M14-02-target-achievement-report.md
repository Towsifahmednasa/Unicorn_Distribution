# RPT-M14-02 - Target Achievement Reports (Zone / Area / Territory)

Two report pages (Tier-B specs PAGE-549, PAGE-550); read manually for this document (CONFIRMED wiring, SP formulas UNKNOWN).

| Attribute | PAGE-549 `TargetAChivementReport` | PAGE-550 `TargetAChivementReportNew` |
|---|---|---|
| Path | `Solution.Web/Reports_UI/TargetAChivementReport.aspx(.cs)` | `Solution.Web/Reports_UI/TargetAChivementReportNew.aspx(.cs)` |
| Type | GridView, CSV export | three GridViews (`gv_Zone`, `gv_Area`, `gv_Territory`) with footer totals, CSV export |
| SP | `sp_Get_TTargetAChivementReport` (`Setup2DAL.Get_TargetAChivementReport`, `Library.DAL/DoctorModule_DAL/Setup2DAL.cs:118-140`) | `sp_Get_TTargetAChivementReport_nnn` (`Setup2DAL.Get_TargetAChivementReportNew`, `:143-165`) |
| SP parameters | `@_Month,@_Year,@FromDate,@ToDate,@_Zone,@_Area,@_Tr` | `@FromDate,@ToDate,@Type(Zone/Area/Territory),@ZoneId,@Area,@Terr` |
| Filters | Month, Year, From/To date, Zone, Area, Territory (cascade via `_dataLoad.GetArea_ByZoneId_Rpt`, `GetTerritory_ByAreaId_Rpt`) | From/To date, report-type radio (Zone/Area/Territory), Zone/Area/Territory (`IVMarketStructure`-style controls; DZSM/AM scoping by Session `RoleTypeName`/`RoleTypeId`) |
| Columns | SL#, Territory Code/Name, Target, Order, Ach%, Invoice, Ach%, Sales, Ach% | same + Zone/Area names by level; footer sums of Target, Order, Invoice, Sales (rounded with `Math.Round`, format `#,##0`) - `TargetAChivementReportNew.aspx.cs:190-240` |
| Session | `RoleTypeName, EmpInfoId, RoleTypeId` | same |
| Export | CSV `Target Achievement Report_<timestamp>.csv` | CSV, one per level |
| Rules | BR-M14-026 | BR-M14-026 |
| Notes | The `param()` helper in both pages builds a `TadaDate`/`ApprovalStatus` filter copied from the TA/DA claim report and is unused (dead); leftover claim-edit popup `TADAClaimEdit.aspx` in 549 | Achievement percentages (`OrderAchiv`, `InvoiceAchiv`, `SalesAchiv`) are computed inside the SP (UNKNOWN); footer does not show Ach% totals |

Business meaning (INFERRED): compare the target loaded via M14 pages/Excel with actual orders, invoices and sales per Zone/Area/Territory for the selected period.
