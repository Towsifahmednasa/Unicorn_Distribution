# WF-M14-01 - Target cascade setup (National -> Zone -> Area -> Territory)

Module: M14. Status: traced from code (CONFIRMED wiring; SP internals UNKNOWN). Related BR: BR-M14-001..012, 014.

## Trigger and actors
- Planner at head office (national/zone level) or DZSM/AM (area/territory level) sets monthly targets. Which roles are allowed to open each page is decided by the DB-driven menu (UNKNOWN); code only scopes DZSM/AM drop-downs.
- Cadence: monthly (Year + Month selector; month is stored as name text).

## Preconditions
- Master data exists: groups (`sp_NTS_Group_Active`), zones, areas, territories, employees.
- Parent level saved before child level (each child page reads the parent amount; if missing the label is empty and the page refuses to save).

## Steps
1. **National** - `DWSP/NationalTargetSetup.aspx` `btnSave_Click` (`NationalTargetSetup.aspx.cs:179`) -> `Validation()` (Year, Month, Group, Amount) -> `NationalTargetSetupDal.SaveNationalTargetSetup` -> `sp_Save_NationaltargetSetup(@NatargetSpId=0,@GroupId,@Year,@Month(name),@Amount,@EntryBy=Session UserId)`; success only when returned id > 0. Redirect `NationalTargetSetupView.aspx`.
2. **Zone** - `ZoneWiseTargetSetup.aspx`: group change -> `Get_ZoneActive_All_ByGroup` (`sp_Get_Zone_All_Active_ZoneTarget`) fills grid; `NewMethod` -> `sp_Get_NationalTargetAmount` fills national label; amounts edited with running-sum cap (BR-M14-006); `btnSave_Click` (`:228`) -> `SaveZoneWiseTarget` -> `sp_Save_ZoneWiseTargetSetup` per zone, one transaction (BR-M14-012).
3. **Area** - `AreaWiseTargetSetup.aspx`: zone chosen (DZSM locked to own zone) -> `GetArea_ByZoneId_All` fills grid; `sp_Get_ZoneTargetAmount` fills zone target; `btnSave_Click` (`:317`) -> `SaveZoneWiseTarget` (misnamed, saves areas) -> `sp_Save_AreaWiseTargetSetup` per area.
4. **Territory** - either `TerritoryWiseTargetSetup.aspx` (menu, session-based scoping, `Session UserId` audit) or `TerritoryWiseTargetSetupApps.aspx?EmpId=` (no login, `@EntryBy` blank). Area chosen -> `GetTeritory_ByAreaId_All` fills grid; `sp_Get_AreaTargetAmount` provides parent; save -> `SaveTerritoryWiseTarget` -> `sp_Save_TerritoryWiseTargetSetup` per territory.
5. **Amend** - `*TargetSetupView.aspx` pages list saved rows (`sp_Get_*TargetList` with a concatenated `@Parameter` filter) and re-save changed rows through the same DAL/SP (`UpdateData` row command with an existing id).
6. Continue with WF-M14-02 (territory -> day plan) or reports (RPT-M14-02).

## Data created / changed
Rows in the tables behind `sp_Save_NationaltargetSetup`, `sp_Save_ZoneWiseTargetSetup`, `sp_Save_AreaWiseTargetSetup`, `sp_Save_TerritoryWiseTargetSetup` (names of tables/columns UNKNOWN). No status transitions.

## Branches / exceptions
- Parent missing -> child page blocks Save (amount label empty).
- Running total above parent -> row cleared with "Amount must be equal with target amount"; equality is *not* required on Save.
- Duplicate (same key) -> handled inside SPs: national and zone pages surface `Already Exist!` only when `pk = 0`; area/territory pages never see failure (BR-M14-012).
- DAL exception -> rollback, exception propagates (no page catch).

## Postconditions
Target rows exist for every level saved; child amounts may not sum to parent (unchecked).

## Tests
TC-541-01/05, TC-546-01/03/04, TC-539-01/02/05, TC-545-01/04 (see the page specs).

## Related pages
PAGE-541, 542, 546, 547, 539, 540, 544, 543, 545.
