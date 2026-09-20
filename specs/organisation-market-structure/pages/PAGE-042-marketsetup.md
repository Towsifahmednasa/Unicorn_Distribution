---
page_id: PAGE-042
module: M03
page: MarketSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-042 — MarketSetup

> **Evidence level:** Tier A. `MarketSetup.aspx` (1033 lines; ≈600 are inline JS remnants), `.aspx.cs` (584 lines), user control `IVMarketStructureMarket.ascx(.cs)`, `Setup2DAL.SaveMarket` (1198-1310), `GetMarketEditDataDAL`, `GetMarketCountDataDAL`, `CommonDataLoad` division/district/thana loaders read. SP bodies absent (`UNKNOWN`). Shared control: README **[MS-CTRL]**; process: `workflows/WF-M03-04-hierarchy-node-setup-and-deactivation.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-042 / M03 |
| Name | MarketSetup ("Market Setup", `.aspx:25`) — sub-heading "User Role Wise Station Type" |
| Kind | Entry / Transaction — create/edit the lowest node of the sales hierarchy (Market) |
| Folder / master | DoctorModule_UI / NewMasterPage.master |
| Class | `DoctorModule_UI_MarketSetup`; `static CommonDataLoad _dataLoad` (`.cs:15`) |

## 2. Purpose
CONFIRMED: create/edit a **Market** (a named trading locality) under a Sub-Territory, geolocated by Division→District→Thana, with an *active/inactive date* and a mandatory list of **station types per user role** (MIO, AM, DZSM).

## 3. Business Objective
INFERRED: markets are the unit at which customers (pharmacies/doctors), routes and field-force travel allowances are organised; the station type (per role: e.g. HQ/ex-HQ/outstation — values from `sp_GET_StationTypeList`) drives allowances/tour rules for each of MIO, AM, DZSM visiting that market. Active/inactive control with dependants check prevents orphaning customers.

## 4. Navigation
List page `MarketRecords.aspx` (PAGE-041) links `MarketSetup.aspx?id=` (`MarketRecords.aspx:16,180`). Success → `MarketRecords.aspx` (`.cs:325`). Reset → self.

## 5. Route
`~/DoctorModule_UI/MarketSetup.aspx[?id=<MarketId>]`

## 6. Source Files
`Solution.Web/DoctorModule_UI/MarketSetup.aspx(.cs)`; `Solution.Web/MasterSetup_UI/IVMarketStructureMarket.ascx(.cs)` (Group…Sub-Territory visible, Market row `visible="false"` `.ascx:95`); `Library.DAL/DoctorModule_DAL/Setup2DAL.cs` (`SaveMarket` 1198, `GetMarketEditDataDAL` 2682, `GetMarketCountDataDAL` 2702, `Get_UserTypeInfo` 4765, `Get_StationTypeInfoDAL` 4784); `CommonDataLoad` (`GetDivision_Active`, `GetDistrict_ByDivision_Active`, `GetThana_ByDistrict_Active`); DAO `Library.DAO/DoctorModule_DAO/Market.cs`, `MarketStationDetailDao`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| structure control Group / Zone | Group / Zone | no | placeholder | role-locked ([MS-CTRL]) | cascade SPs | – |
| `AreaSelect` | Area | **Yes** (`Validation()` `.cs:135-255`) | placeholder | – | `sp_CS_GetArea_ByZoneId_*` | tooltip "please fill out this field" |
| `TeritorySelect` | Territory | **Yes** | placeholder | – | – | same |
| `SubTeritory` | Sub-Territory | **Yes** | placeholder | – | – | same |
| `mainName` | Market Name | **Yes** | empty | – | – | same |
| `DivisionSelect` (AutoPostBack) | Division | no (cascade only) | placeholder | – | `GetDivision_Active` | – |
| `DistrictSelect` (AutoPostBack) | District | no | – | items cleared on division change | `GetDistrict_ByDivision_Active` → `sp_Get_DistrictList_OnlyActive_ByDivisionId` | – |
| `ThanaSelect` | Thana | **Yes** | – | – | `GetThana_ByDistrict_Active` → `sp_Get_ThanaList_OnlyActive_Bydistrict_id` | same |
| `UserRoleSelect` | User Type | required to add row | placeholder | – | `Get_UserTypeInfo` → `sp_GET_RoleType` (`RoleTypeId`,`RoleType`) | tooltip |
| `StationTypeSelect` | Station Type | required to add row | placeholder | – | `Get_StationTypeInfoDAL` → `sp_GET_StationTypeList` | tooltip |
| `addButton` | Add to list | – | – | – | – | – |
| `gv_UserRole` | role/station rows with hidden `hfRoleTypeId`,`hfStationTypeId`, delete button | ≥1 (and exactly one each of MIO/AM/DZSM) | empty | – | in-memory | alerts (§18) |
| `chkIsActive` | Active | – | checked | label of date switches "Active Date"/"Inactive Date" | – | – |
| `acDate` | Active/Inactive Date | **Yes** | empty | – | – | same |
| `gv_Count` | "Parti" / "MarketIdCount" dependants count (edit only, `divShowHide`) | – | – | shown only with `?id` | `sp_check_Count_MarketStructure(@MasterId,@PageName='Market')` | – |
| `btnSave`/`btnUpdate`, `restbtn` | Submit/Update, Reset | – | – | new vs edit visibility | – | sweetalert confirm |
`#isStrucChange` ("Is Structure Change") is a client-only, non-server, `display:none` checkbox — no effect.

## 8. Field Mapping
| UI | DTO (`Market`) | Code | SP param |
|---|---|---|---|
| `id_mastetID` | `MarketId` | `.cs` `Convert.ToInt32` or 0 | `@id` |
| `F_SubTeritory` | `SubTerritoryId` | `SelectedIndex>0 ? int.Parse : null` | `@SubTerritoryId` |
| `mainName` | `MarketName` | – | `@Name` |
| `ThanaSelect` | `ThanaId` | – | `@ThanaId` |
| `acDate` | `AcOrInAcDate` | `DateTime.Parse` | `@acInAcDate` |
| `chkIsActive` | `IsActive` | – | `@isActive` |
| (never set) | `Remarks` | null | `@remarks` (null Value on a `SqlParameter` — ADO.NET omits it; SP default UNKNOWN) |
| `Session["UserId"]` | – | – | `@createdBy` |
| role/station grid rows | `MarketStationDetailDao{UserRoleID, StationTypeId}` | one per row | `sp_Save_MarketStationDetail(@StationTypeId,@MarketId,@UserRoleID)` |
Group/Zone/Area/Territory are navigation only; only `SubTerritoryId` is persisted.

## 9. Dropdown Mapping
See §7; hierarchy dropdowns preloaded in edit from `sp_Get_MarketData_ByMarketid` (`GroupId, RegionId, AreaId, TerritoryId, SubTerritoryId, MarketId`) through hidden fields (BR-M03-003); Division/District/Thana selected in edit by cascading calls (`.cs` `GetOneRecord`). Role type ids are hard-coded in validation as MIO=1, AM=2, DZSM=3.

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Add to list | `addButton_Click` | `Add()` (rebuilds a DataTable from the grid and appends role+station) | – | in-memory |
| delete row | `deleteImageButton_Click` → `Remove(row)` | – | – | in-memory |
| Submit/Update | `btnSave_Click` | `Validation()` → `Setup2DAL.SaveMarket(market, list, userId)` | see §16 | write |
| Reset | `restbtn_Click` | redirect self | – | – |

## 11. Frontend Flow
Server-driven UpdatePanel form. `IsActiveChange()` (`.aspx:605-613`) relabels the date. The remaining ≈600 lines of inline JS are the legacy jQuery/`Setup.aspx` implementation (e.g. `GetGroupInfo`, `_GetGroupInfo_Active`) that targets non-existent element ids — inert (INFERRED).

## 12. Backend Flow
`Page_Load` (first load): `LoadInitialInfo()` fills Division, StationType, UserRole lists and an empty `gv_UserRole`; if `?id` → show `btnUpdate`, `divShowHide`, `GetOneRecord(id)`: `sp_Get_MarketData_ByMarketid` → name, date, ids into hidden fields, `chkIsActive` (`IsActive=="1"`), Division/District/Thana cascade, role rows into `gv_UserRole` (only if both `RoleTypeId` and `StationTypeId` non-empty in the first row — the SP returns one row per role, but the grid binds the whole DataTable), dependants count grid. All wrapped in `try/catch{}`.
`btnSave_Click` → `Validation()` → `SaveMarket` (Setup2DAL.cs:1198-1310), *single implicit transaction*:
1. **Edit** (`MarketId>0`): `sp_check_MarketInfo(@id,@SubTerritoryId,@Name)` – rows>0 → `isSuccess=false` (→ UI "Already Exist!"). Else if `IsActive==false` → `sp_check_Vali_MarketStructure(@MasterId,@PageName="Market")`; rows>0 → `isValiCheck=true`; rows=0 → `sp_Update_MarketData`. If `IsActive==true` → `sp_Update_MarketData` and set `upTime=true`.
2. **New**: `pk = SaveDataReturnPrimaryKey("sp_Save_MarketData")`; `pk>0` → `isSuccess=true`.
3. If `isSuccess` and `pk>0`: for each grid row `SaveDataReturnPrimaryKey("sp_Save_MarketStationDetail")` (whether old rows are cleared first: UNKNOWN); then if `upTime` → `UpdateData("sp_Update_CustomerInfoForMarketData",{@MasterId=pk})` (propagates market changes to customer rows; semantics UNKNOWN) and `isSuccess` takes its result.
Exception → `SqlConnectionClose(true)` rollback, rethrow (page: no try/catch).
Result → page: success → `successalert(…,'MarketRecords.aspx')`; `isDuplicateCheck` → "Already Exist!"; `isValiCheck` → "Data cannot be deactivated!"; else → "Already Exist!" (also used for plain failures).

## 13. API Mapping
No `[WebMethod]` on this page.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
| DAL | Source | SP |
|---|---|---|
| `Setup2DAL.SaveMarket` | Setup2DAL.cs:1198 | `sp_check_MarketInfo`, `sp_check_Vali_MarketStructure`, `sp_Update_MarketData`, `sp_Save_MarketData`, `sp_Save_MarketStationDetail`, `sp_Update_CustomerInfoForMarketData` |
| `Setup2DAL.GetMarketEditDataDAL` | :2682 | `sp_Get_MarketData_ByMarketid(@id)` |
| `Setup2DAL.GetMarketCountDataDAL` | :2702 | `sp_check_Count_MarketStructure(@MasterId,@PageName)` |
| `Get_UserTypeInfo` / `Get_StationTypeInfoDAL` | :4765 / :4784 | `sp_GET_RoleType` / `sp_GET_StationTypeList` |
| `CommonDataLoad.GetDivision_Active`, `GetDistrict_ByDivision_Active`, `GetThana_ByDistrict_Active` | CommonDataLoad.cs | `sp_Get_Division_Active`, `sp_Get_DistrictList_OnlyActive_ByDivisionId`, `sp_Get_ThanaList_OnlyActive_Bydistrict_id` |

## 16. Stored Procedure Mapping
All bodies **NOT in repository**: `sp_Save_MarketData`, `sp_Update_MarketData`, `sp_check_MarketInfo`, `sp_check_Vali_MarketStructure`, `sp_check_Count_MarketStructure`, `sp_Save_MarketStationDetail`, `sp_Update_CustomerInfoForMarketData`, `sp_Get_MarketData_ByMarketid`, `sp_GET_RoleType`, `sp_GET_StationTypeList`, division/district/thana list SPs. Parameter lists as §8. Note: `sp_Save_CustomerMaster`/`sp_Update_CustomerMaster` bodies exist in the live update script but are customer pages (other module).

## 17. Database Mapping
No inline SQL. Tables UNKNOWN. Column names surfaced by edit SP: `MarketName, AcOrInAcDate, GroupId, RegionId, AreaId, TerritoryId, SubTerritoryId, MarketId, DivisionId, DistrictId, ThanaId, IsActive, RoleTypeId, StationTypeId`.

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Area / Territory / Sub-Territory | required (`SelectedValue == ""`) | `Validation()` | tooltip "please fill out this field" | BR-M03-032 |
| Market name | required | same | same | BR-M03-032 |
| Thana | required | same | same | BR-M03-032 |
| Active date | required | same | same | BR-M03-032 |
| Role grid | ≥1 row | same | alert "please Add to List User Role Wise Station Type!" | BR-M03-033 |
| MIO row | exactly 1 (`RoleTypeId==1`) | same | "MIO must be added in list!!!" | BR-M03-033 |
| AM row | exactly 1 (`RoleTypeId==2`) | same | "AM must be added in list!!!" | BR-M03-033 |
| DZSM row | exactly 1 (`RoleTypeId==3`) | same | "DZSM must be added in list!!!" | BR-M03-033 |
| Add row | role and station both chosen | `addButton_Click` | tooltip on the missing one | BR-M03-034 |
| Duplicate (edit only) | `sp_check_MarketInfo` rows | DAL | "Already Exist!" | BR-M03-035 |
| Deactivate with dependants | `sp_check_Vali_MarketStructure` rows | DAL | "Data cannot be deactivated!" | BR-M03-036 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-032 | Sub-Territory (hence Area, Territory), Name, Thana and Active/Inactive date are mandatory; Division/District only drive the Thana list. | MarketSetup.aspx.cs `Validation()` | page | CONFIRMED |
| BR-M03-033 | A market must carry **exactly one** station-type row each for MIO (role id 1), AM (2) and DZSM (3); no other roles are counted or forbidden, and duplicates of one role fail the "must be added" message. | MarketSetup.aspx.cs `Validation()` | page (hard-coded ids) | CONFIRMED |
| BR-M03-034 | A role/station row is added only when both selectors are chosen; the same role may be added twice in the grid (no add-time duplicate check) but Validation later rejects it. | `addButton_Click`, `Add()` | page | CONFIRMED |
| BR-M03-035 | On edit, a market name must be unique under its Sub-Territory (`sp_check_MarketInfo`); create has no C# duplicate check. | Setup2DAL.cs:1213-1263 | DAL | CONFIRMED (call) / UNKNOWN (semantics) |
| BR-M03-036 | A market cannot be deactivated while `sp_check_Vali_MarketStructure(@PageName='Market')` returns rows. | Setup2DAL.cs:1232-1246 | DAL | CONFIRMED (call) |
| BR-M03-037 | Saving an **active** market on edit triggers `sp_Update_CustomerInfoForMarketData` for that market (customer rows refresh; not run for create or for deactivation). | Setup2DAL.cs:1250-1257,1290-1295 | DAL | CONFIRMED (call) / UNKNOWN (effect) |
| BR-M03-038 | Station-type rows are inserted one-by-one after the market row inside the same transaction; old rows are not removed by C# (SP behaviour UNKNOWN). | Setup2DAL.cs:1278-1289 | DAL | CONFIRMED / UNKNOWN |
| BR-M03-039 | Edit form shows dependants counts (`sp_check_Count_MarketStructure`) so the user can see why deactivation is blocked. | MarketSetup.aspx.cs `GetOneRecord` | page | CONFIRMED |
| BR-M03-040 | Active flag on load is read as `IsActive == "1"` (string compare), whereas SubTerritory/others use `Convert.ToBoolean` — a bit column serialised as "True" would load unchecked. | MarketSetup.aspx.cs `GetOneRecord` | page | CONFIRMED (code) / INFERRED (impact) |
Also: BR-M03-001/002/003.

## 20. Permission Matrix
`Session["UserId"]` unguarded; control role/employee session keys. No role gate; menu-only; no `<authorization>`. Role only locks selectors (BR-M03-002).

## 21. Status Lifecycle
`IsActive` (bit) + `AcOrInAcDate`: Active ⇄ Inactive; Active→Inactive blocked by dependants (BR-M03-036). No delete.

## 22. Error Handling
`GetOneRecord`, all `LoadInitialInfo` loaders and cascades swallow exceptions; save exceptions unhandled; "Already Exist!" is the catch-all failure text.

## 23. Audit
`@createdBy` UserId (insert and update alike); no update-by parameter; timestamps SP-side UNKNOWN.

## 24. Integration
`sp_Update_CustomerInfoForMarketData` touches customer master rows (other module). No external integration.

## 25. Reports
None.

## 26. Dependencies
`Setup2DAL`, `CommonDataLoad` (static instance), structure control; consumers: PAGE-041 (list), PAGE-062 route (market picker), PAGE-096/098 (transfer/import), customer master.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Save with Area empty / Territory empty / Sub-Territory empty | first missing highlighted | BR-M03-032 |
| TC-02 | Empty name / Thana / date | highlighted | BR-M03-032 |
| TC-03 | No role rows | alert "please Add to List…" | BR-M03-033 |
| TC-04 | Only MIO+AM rows | alert "DZSM must be added in list!!!" | BR-M03-033 |
| TC-05 | Two MIO rows | "MIO must be added in list!!!" | BR-M03-033 |
| TC-06 | Add row with role but no station | station highlighted | BR-M03-034 |
| TC-07 | Create valid market | success → MarketRecords | BR-M03-038 |
| TC-08 | Edit rename to existing sibling name | "Already Exist!" | BR-M03-035 |
| TC-09 | Deactivate market with customers | "Data cannot be deactivated!" | BR-M03-036 |
| TC-10 | Edit active market | `sp_Update_CustomerInfoForMarketData` invoked | BR-M03-037 |
| TC-11 | Edit inactive→active with bit stored True | check box state on reload | BR-M03-040 |
| TC-12 | Division change | district/thana lists reset | §9 |

## 28. Reverse Engineering References
`Solution.Web/DoctorModule_UI/MarketSetup.aspx(.cs)`, `MasterSetup_UI/IVMarketStructureMarket.ascx(.cs)`, `Library.DAL/DoctorModule_DAL/Setup2DAL.cs:1198-1310,2682-2722,4765-4800`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Role station types | None existed | exactly MIO/AM/DZSM | absent | CONFIRMED (code) | BR-M03-033 |

## 30. Known Gaps
Hard-coded role ids 1/2/3; generic "Already Exist!"; create has no duplicate check; station rows re-insert semantics unknown; `IsActive=="1"` inconsistency; dead 600-line legacy JS; static `CommonDataLoad`; swallowed exceptions; `@remarks` never set; SP bodies absent (GAP-M03-001); market re-parenting by edit bypasses transfer approval (`SubTerritoryId` is editable in edit mode — controls are not disabled: commented-out `Enabled=false` lines `MarketSetup.aspx.cs:48-54`) — GAP-M03-010.

## 31. Change Impact
`SaveMarket` (Setup2DAL.cs:1198) is also referenced by an overloaded `SaveMarket(Market,string)` at :2939 (older signature). `sp_check_Vali_MarketStructure` shared (see PAGE-057). Market feeds route master (PAGE-062, `sp_check_RouteInformationMArket`), router (PAGE-052), transfer pages, bulk import (PAGE-098).
