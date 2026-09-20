# RPT-M14-01 - DWSP Monthly Report

| Attribute | Value |
|---|---|
| Page | `Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx` (+ `.aspx.cs`), PAGE-548 |
| Type | ASP.NET GridView report (no Crystal `.rpt`, no dataset file) |
| Master page | `NewMasterPage.master` (login enforced by master) |
| Data source | `sp_Process_DWSPReport_Territory` via `CmnCrystaltoView.GetDWSPMonthlyList_Mew` (`Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1568-1596`); body NOT in repo |
| Request parameters | none (no QueryString); all filters are form fields |
| Filters | Month (12 rolling months), Year (2015-2050), Approval Status (`SeedDataDAL.GetApprovalStatusList`, blank => `Select`), market-structure user control Zone/Area/Territory (`IVMarketStructureMarket.ascx`; Group, SubTerritory, Market controls exist but are not passed to the SP). Employee filter is hidden/dead. |
| SP parameters | `@Month` (name), `@Year`, `@MonthValue` (number), `@ApprovalStatus`, `@RegionId`, `@AreaId`, `@TrId` |
| Session used | `RoleTypeName` (DZSM must select a zone), `EmpInfoId`, `RoleTypeId` |
| Output columns | `EmpMasterCode, TerritoryCode, EmpName, DesigName, RoleName`, "Target Set", then for d = 1..31: `{d}_Gen, {d}_Cam, {d}_FCB, {d}_Total` and `Grand_Total` (`DWSPMonthlyRpt.aspx:273-446`); a second header row with day numbers is added in `loadGridView_OnRowCreated` |
| Paging / export | GridView paging; export = rendered grid as `DWSP_Report_<dd_MMM_yyyy_hh_mm_tt>.xls` (`application/ms-excel`) with heading "DWSP Report Month of <month> <year>" and print date |
| Business rules | BR-M14-019, BR-M14-020, BR-M14-002 |
| Known issues | dead `param()`, hidden employee filter, reset button -> `EmpMonthlyExpenseRpt.aspx`, SP logic/rounding/target-set column source UNKNOWN |
