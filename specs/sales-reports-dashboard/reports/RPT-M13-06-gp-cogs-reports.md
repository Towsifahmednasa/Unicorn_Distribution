# RPT-M13-06 GP / COGS reports

_Module M13. Documentation only, generated from source 2026-09-19. Confidence: CONFIRMED unless tagged. SP bodies are not in the repository._

| List / entry page | Viewer / output page | .rpt or output | Dataset / DataTable name | BLL -> DAL method | SP / inline SQL | Request / session parameters | List-page filters | Tables |
|---|---|---|---|---|---|---|---|---|
| COGSBasedGPReport 495 | on-screen grid + XLSX (ClosedXML, "GP_Report") | none | DataTable -> `dtExport` (15 columns) | CmnCrystaltoView.GetCOGSBasedGPReportDAL | **sp_Get_COGSBasedGPReport(@Parm)** | none (page-local) | DC, customer, customer type, product, sales executive, payment date range, market structure (Group/Zone/Area/Territory/SubTerritory/Market) | UNKNOWN (aliases CPY, INV, INVD, CMS, mas) |
| GpSalesReport 511 | on-screen grid + CSV | none | - | CmnCrystaltoView.GetTerriTorryWiseSalesReportDataDAL | **sp_Get_TerritorryWiseSalesReportList(@Parm)** | none | DC, invoice UpdateDate range, market structure | UNKNOWN |
| TerritoryWiseSalesReport 530 | see RPT-01 (DZSM family) | - | - | - | sp_GET_DZSM* | - | - | - |
| SixTenProjectReportApps 463 | grid + 4 KPI tiles | none | - | DoctorMonitoringDAL | sp_Get_AllDoctorVisitMonitoringApprovalList | QS EmpInfoId | group/zone/area/territory, dates | UNKNOWN |

## GP / COGS calculation status
- **COGS report columns** (as returned/exported): Month, Date, ComUnitName ("Sales Center"), BUWise ("BU Wise"), Particulars, SKU, Qty, OrderSenderName ("Name of Sales Executive"), RealizationAmount, VATAmount, TaxAmount, COGSAmount, NetAmount, GP, GPPercent. The only arithmetic in C# is `Total Realization Amount = SUM(RealizationAmount)` (N2) and appending "%" to GPPercent on export. **GP = ?, GP % = ?, COGS = ?, NetAmount = ? are defined inside the SP (UNKNOWN)**.
- Date basis: **payment date** `CONVERT(date, CPY.PaymentDate)`; company unit fallback chain `ISNULL(CPY.ComUnitId, ISNULL(INV.ComUnitId, mas.ComUnitId))` (BR-M13-037).
- GpSalesReport: columns GPSales and TotalSales; footer sums; label "Total Net Amount" = SUM(TotalSales); date basis `I.UpdateDate` (BR-M13-040). What "GP" means for GPSales (gross profit vs GP customer type) is UNKNOWN.
- Achievement %, target vs actual, growth %: none found in M13 code except `DiffInPercent` (RPT-05).
- Caching: COGS grid cached per user for 10 minutes and export uses it (BR-M13-039).
- Security: both pages pass a concatenated SQL predicate to the SP (GAP-M13-006).

