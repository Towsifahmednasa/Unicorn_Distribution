# WF-M13-03 Business Summary MIS reporting (role-scoped, SP-driven)

_Module M13, reverse-engineered 2026-09-19 (documentation only). Confidence tags as in the page specs._

## Trigger and actors
Management/NSM/DZSM/AM opens one of TotalSummaryNew / TotalSummaryNew2 / _T / MIOWise* / ProductWiseSalesReport / MonitoringReport / DZSMTotalSummary.

## Preconditions
Master-page session (`Session["UserId"]`); role keys RoleTypeName/RoleTypeId/EmpInfoId (login).

## Steps
1. Page_Load: default dates, load dropdowns, narrow the level radios and selectors by role (BR-M13-029, BR-M13-033).
2. User picks level (DC/Zone/Area/Territory), Transactional or Non-transactional, market-structure scope, dates -> View.
3. `LoadInfo`: date check -> map to `@Type` (BR-M13-028) -> `TotalSummaryBLL` -> `TotalSummaryDAL` -> SP via `ClsCommonInternalDAL.GetDataTableAction(..., "SSIDB")`.
4. Bind grid of the chosen level, compute footer sums in C# (BR-M13-030).
5. Export: `btnExportToExcel_Click` renders the grid to .xls; legacy: `excelButton1_Click` -> `BusinessSummaryViewer.aspx` -> `sp_Get_BusinessSummaryReportList` -> `crpBusinessSummary.rpt` Excel (BR-M13-032).

## Data
Read-only. KPI definitions: RPT-M13-01 (inline SQL twin) - SP formulas UNKNOWN.

## Branches
No dates -> alert; no rows -> alert; DZSM without zone (New2) -> empty result.

## Related
BR-M13-021..034, 045..049; README variant table; GAP-M13-001, -012..-014.

