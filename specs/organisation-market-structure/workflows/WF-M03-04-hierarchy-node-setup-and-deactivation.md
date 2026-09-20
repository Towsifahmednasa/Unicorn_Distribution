# WF-M03-04 — Hierarchy node setup (Group → Zone → Area → Territory → Sub-Territory → Market) and deactivation

> Traced from `Library.DAL/DoctorModule_DAL/GroupSetupDal.cs` (`Save_DepartmentInfo`, name copied from another page), `ZoneSetupDAL.cs:97-190` (`SaveZoneInfo`), `Setup2DAL.cs` (`SaveAreaInfo` :211, `SaveTerritory` :613, `SaveSubTerritory` :722, `SaveMarket` :1198), the setup pages `NationalSetup`, `ZoneSetup`, `AreaSetup`, `TerritorySetup`, `SubTerritorySetup` (PAGE-057, Tier A), `MarketSetup` (PAGE-042, Tier A) and their `*Records` list pages. Host WebMethods for Zone/Area/Territory/SubTerritory/Group pages: `Solution.Web/DoctorModule_UI/Setup.aspx.cs` (`SaveZone` :130, `SaveArea` :231, `SaveTerritory` :318, `SaveSubTerritory` :328) and `NationalSetup.aspx.cs` (`Save_groupSetupInfo`). SP bodies are absent (`UNKNOWN`).

## Trigger & actors
Master-data administrator creates or edits a node or (de)activates it. Access is menu-only (no role check in code). Users of role AM/DZSM/NSM only see their own parents in the selectors (BR-M03-002).

## Parent keys (from save parameters)
| Level | Save DAL | Parent param | Key/name |
|---|---|---|---|
| Group ("National Setup") | `GroupSetupDal.Save_DepartmentInfo` | `@NationalId` (semantic UNKNOWN) | `@GroupName`, `@CodeStr`, `@IsActive` |
| Zone | `ZoneSetupDAL.SaveZoneInfo` | `@GroupId` | `@zoneId` (=RegionId), `@zoneName`, `@CodeStr` |
| Area | `Setup2DAL.SaveAreaInfo` | `@zoneId` | `@areaName`, `@CodeStr` |
| Territory | `Setup2DAL.SaveTerritory` | `@areaId` | `@Name`, `@CodeStr` |
| Sub-Territory | `Setup2DAL.SaveSubTerritory` | `@TerritoryId` | `@Name` |
| Market | `Setup2DAL.SaveMarket` | `@SubTerritoryId`, `@ThanaId` (+ station-type rows per role) | `@Name` |
All carry `@isActive`, `@acInAcDate` (active/inactive date), `@createdBy`.

## Steps (common pattern)
1. Page loads (edit: `?id=` → edit SP returns the node and its ancestors; ancestors are put in hidden fields and the structure control preloads full lists incl. inactive — BR-M03-003).
2. Page-level validation (parent + name + date required; Market: Thana and role triple — BR-M03-027, 032, 033).
3. DAL (one implicit transaction — BR-M03-101):
   - **Edit** (`id>0`): `sp_check_<Level>Info` (duplicate name under parent). Rows ⇒ refuse. Otherwise if the node is being **deactivated** (`IsActive==false`) → `sp_check_Vali_MarketStructure(@MasterId=id,@PageName=<Level>)`; rows ⇒ `isValiCheck` ("Data cannot be deactivated!"); no rows ⇒ update SP. If staying active ⇒ update SP.
   - **New**: `SaveDataReturnPrimaryKey(sp_Save_<Level>Info)`; success test differs by level (see table).
   - Market only: station-type child rows inserted; on edit-active also `sp_Update_CustomerInfoForMarketData`.
4. Page maps `ResultInfo` flags to alerts: success → list page; `isDuplicateCheck` → "Already Exist!"; `isValiCheck` → "Data cannot be deactivated!"; anything else → "Already Exist!".

| Level | Edit duplicate ⇒ | New success test | `PageName` for deactivation check |
|---|---|---|---|
| Group | `isDuplicateCheck=true` | `pk>0` else `isDuplicateCheck=true` | `Group` |
| Zone | `isDuplicateCheck=true` | `pk>0` else `isDuplicateCheck=true` | `Zone` |
| Area | `isDuplicateCheck=false` (explicitly), `isSuccess` stays false | `pk>0` else `isDuplicateCheck=false` | `Area` |
| Territory | `isDuplicateCheck=true` | `pk>0` else `isDuplicateCheck=true` | `Territory` |
| Sub-Territory | `isSuccess=false` only | **forced `isSuccess=true`** (BR-M03-030) | `SubTerritory` |
| Market | `isSuccess=false` only | `pk>0` | `Market` |
(Inconsistent flag use is a code smell, GAP-M03-014; user-visible text is nearly always "Already Exist!".)

## Deactivation guard (shared)
`sp_check_Vali_MarketStructure(@MasterId,@PageName)` is the single dependants check for Group, Zone, Area, Territory, SubTerritory, Market (and also Department, Designation, GenericGroup, ExpenseType… pages). Body absent; semantic (children? customers? MIO tags?) UNKNOWN. Also reused with `@PageName='CustomerTransfer'` and `'TerritoryWiseDepotSetupUncheck'` (pending-order guards, PAGE-064).

## Data created / changed
Node rows in the level's master table (names UNKNOWN); Market: station-type detail rows; customer rows refreshed via `sp_Update_CustomerInfoForMarketData` (UNKNOWN effect).

## Status transitions
`IsActive` true ⇄ false with `AcOrInAcDate`. Active→Inactive requires no dependants (guard). Inactive nodes drop out of Active-only pickers (transfer pages, route entry) but remain visible in "All" loaders used when editing. No delete anywhere.

## Branches / exceptions
Edit that changes the **parent** (e.g. SubTerritory `@TerritoryId`, Market `@SubTerritoryId`) is possible from the setup pages without the transfer/approval workflow (GAP-M03-010); whether the update SPs honour the changed parent is UNKNOWN.

## Postconditions
Node saved/updated; list pages (`*Records`, `NationalSetupView`) show it. Structure controls everywhere show it in cascades if Active.

## Related BR / pages
BR-M03-001…003, 027…040, 101. PAGE-046/047 (National), 060/061 (Zone), 031/032 (Area), 058/059 (Territory), 056/057 (Sub-Territory), 041/042 (Market), and legacy SInventory_UI Area/Region/Zone/District/MarketInfo entry pages (PAGE-066…089; inline-SQL/BLL based, different tables `tblArea, tblRegion, tblZone, tblDistrict, tblMarket` — relationship to the SP-based model UNKNOWN, GAP-M03-013).

## Tests
| ID | Scenario | Expected |
|---|---|---|
| WF4-T1 | Create Area under a Zone | success → list |
| WF4-T2 | Rename Territory to sibling's name (edit) | "Already Exist!" |
| WF4-T3 | Deactivate Zone with active Areas | "Data cannot be deactivated!" |
| WF4-T4 | Deactivate empty Sub-Territory | saved |
| WF4-T5 | Re-activate an inactive Market | saved; customers refreshed |
| WF4-T6 | Create duplicate Sub-Territory | page shows success even if SP returned 0 (verify) |
| WF4-T7 | Edit Market moving to another Sub-Territory | verify SP applies (bypasses transfer approval) |
