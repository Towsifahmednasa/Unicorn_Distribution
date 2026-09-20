# WF-M14-02 - DWSP day-wise plan and monthly report

Module: M14. Related BR: BR-M14-013, 015..020, 002/003. Confidence: CONFIRMED wiring; SP behaviour UNKNOWN.

## Trigger and actors
- Actor: Area Manager (RoleTypeId 2) or DZSM (3), typically via a link carrying `?EmpId=` (INFERRED from the mobile app: the page has no master page and no login check). Review actor: any manager running PAGE-548.

## Preconditions
- Territory target for the month exists (WF-M14-01) - shown read-only but not required to save.
- `EmpId` resolves in `sp_Get_RoleTypeByEmpId`.

## Steps
1. Open `DWSP/TerritoryWiseTargetSetupApps.aspx?EmpId=n` (optional monthly territory split, WF-M14-01 step 4) then `btnAdd_Click` -> `AMDayWiseDWSPSetup.aspx?EmpId=n` (`TerritoryWiseTargetSetupApps.aspx.cs:696`).
2. `AMDayWiseDWSPSetup.Page_Load`/`LoadInitialInfo` (`:30,59`): groups, role resolution (`CommonDal.Get_RoleTypeEmpId` -> `sp_Get_RoleTypeByEmpId`), zone/area/group pre-selection and locking.
3. User selects Month/Year, Zone, Area, Territory (`GetTeritory_ByAreaId_IsVaccant`).
4. `ddlTerritory_SelectedIndexChanged` (`:666`): `sp_Get_FirstDate_LastDatebyYearMonth`, `sp_GetDatewithinDateRange` -> grid with one row per day; `NewMethod` -> `sp_Get_TerritoryWiseTargetSetup` -> read-only monthly target.
5. User keys General/FCB/Campaign per day; each change posts back and `calculation()` refreshes footer totals (`:730`).
6. `btnSave_Click` (`:271`): `Validation()` (BR-M14-015) -> build one `TerritoryTargetSetupDao` per day -> `AreaWiseTargetSetupDal.Save_DWSPMaster` -> `sp_Save_DWSPMaster` per row inside one transaction.
7. Success popup -> redirect `TerritoryWiseTargetSetupApps.aspx?EmpId=n`.
8. Managers run `Reports_UI/DWSPMonthlyRpt.aspx` (WF step): `btnSearch_Click` -> `GetDWSPMonthlyList_Mew` -> `sp_Process_DWSPReport_Territory` -> 31-day grid; export `.xls`.

## Data created / changed
DWSP rows per (employee, territory, date) with General/FCB/Campaign amounts (table UNKNOWN). No status transitions in code.

## Branches / exceptions
- Grand total = 0 -> `Total Target  Can not be Empty!!`; no rows -> `Table can not be Empty!`.
- Total is not compared with the monthly territory target (BR-M14-016).
- Past dates editable (lock code commented out).
- DZSM running the report without zone -> `Please Select Zone!`.
- `@EntryBy` absent in `sp_Save_DWSPMaster` call.

## Postconditions
Daily plan stored; visible in PAGE-548 for the matching month/approval status.

## Tests
TC-538-01..08, TC-548-01..07.

## Related pages
PAGE-538, 545, 548 (and hierarchy helpers `CommonDal`, `CommonDataLoad`).
