---
page_id: PAGE-057
module: M03
page: SubTerritorySetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-057 — SubTerritorySetup

> **Evidence level:** Tier A. `SubTerritorySetup.aspx` (612 lines, incl. ≈400 lines of leftover JS), `.aspx.cs` (220 lines), user control `IVMarketStructureMarketForSubTeriSetup.ascx(.cs)`, `Setup2DAL.SaveSubTerritory/GetEditData_SubTerritoryOne` read. SP bodies absent (`UNKNOWN`). Shared control docs: README **[MS-CTRL]**. Sibling setup pages (National/Zone/Area/Territory/Market) follow the same save pattern — see `workflows/WF-M03-04-hierarchy-node-setup-and-deactivation.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-057 / M03 |
| Name | SubTerritorySetup ("Sub-Territory Setup", `.aspx:26`) |
| Kind | Entry / Transaction — create/edit a Sub-Territory master row |
| Folder / master | DoctorModule_UI / NewMasterPage.master |
| Class | `DoctorModule_UI_SubTerritorySetup` |
| Legacy notes | Inline JS still contains a full jQuery/`Setup.aspx/SaveSubTerritory` implementation from an earlier MVC-style version (`.aspx:257-575`); the server `LinkButton`s are what actually save. |

## 2. Purpose
CONFIRMED: create or edit a **Sub-Territory** (level below Territory, above Market) under a chosen Territory, with an active/inactive flag and its active/inactive date.

## 3. Business Objective
INFERRED: maintain the 5th level of the sales hierarchy so markets can be grouped (e.g. a Sub-Territory = a MIO's working cluster) and so deactivation of obsolete clusters is blocked while dependants exist.

## 4. Navigation
List page `SubTerritoryRecords.aspx` (PAGE-056) links here with `?id=` (`SubTerritoryRecords.aspx:16,148`). Success → `SubTerritoryRecords.aspx` (`.cs:127`). Reset → self (`.cs:154`).

## 5. Route
`~/DoctorModule_UI/SubTerritorySetup.aspx[?id=<SubTerritoryId>]`

## 6. Source Files
`Solution.Web/DoctorModule_UI/SubTerritorySetup.aspx(.cs)`; `Solution.Web/MasterSetup_UI/IVMarketStructureMarketForSubTeriSetup.ascx(.cs)`; `Library.DAL/DoctorModule_DAL/Setup2DAL.cs` (`SaveSubTerritory` 722-822, `GetEditData_SubTerritoryOne` 909); DAO `Library.DAO/DoctorModule_DAO/SubTerritoryDAO`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `IVMarketStructure.GroupSelect` / `ZoneSelect` | Group / Zone | no | placeholder | locked by role (AM/DZSM/NSM, see [MS-CTRL]) | `sp_CS_Group_*`, `sp_Get_Zone_*` | – |
| `AreaSelect` | Area | **Yes** (`.cs:172`) | placeholder | – | `sp_CS_GetArea_ByZoneId_*` | tooltip "please fill out this field", `is-invalid`, focus |
| `TeritorySelect` | Territory | **Yes** (`.cs:182`) | placeholder | – | `sp_CS_GetTerritory_ByAreaId_*` | same |
| (Sub-Territory & Market rows of the control) | – | hidden (`visible="false"` `.ascx:79,95`) | – | – | – | – |
| `mainName` | Sub-Territory Name | **Yes** (`.cs:190`) | empty | – | – | same |
| `chkIsActive` (HTML checkbox `runat=server`) | Active | – | **checked** | `onchange=IsActiveChange()` toggles label between "Active Date"/"Inactive Date" (`.aspx:277`) | – | – |
| `acDate` (pickadate) | Active/Inactive Date | **Yes** (`.cs:200`) | empty | – | – | same |
| `divShowHide` | "Is Structure Change" checkbox (`#isStrucChange`, not server-bound) | – | visible only in edit (`.cs:40`) | client-only, unused by code | – | – |
| `id_mastetID` (hidden) | – | – | "" | set from `?id=` | – | – |
| `btnSave` / `btnUpdate` | Submit / Update | – | `btnSave` visible on new, `btnUpdate` visible on edit (both call `btnSave_Click`) | – | – | `sweetAlertConfirm_*` |
| `restbtn` | Reset | – | – | – | – | – |

## 8. Field Mapping
| UI | DTO (`SubTerritoryDAO`) | Code | SP param | Column |
|---|---|---|---|---|
| `id_mastetID` | `SubTerritoryId` | `.cs:111` (0 if empty) | `@id` | UNKNOWN |
| `TeritorySelect` | `TerritoryId` (int?) | `.cs:115` | `@TerritoryId` | UNKNOWN |
| `mainName` | `SubTerritoryName` | `.cs:114` | `@Name` | UNKNOWN |
| `chkIsActive` | `IsActive` | `.cs:120` | `@isActive` | UNKNOWN |
| `acDate` | `AcOrInAcDate` (`DateTime.Parse`) | `.cs:118` | `@acInAcDate` | UNKNOWN |
| `Session["UserId"]` | – | `.cs:124` | `@createdBy` | UNKNOWN |
`Group`, `Zone`, `Area` selections are UI-only navigation aids (not persisted).

## 9. Dropdown Mapping
Group→Zone→Area→Territory from the control: **preloaded in edit** with the "All" (incl. inactive) loaders because the page sets `hfGroupId/hfZone/hfArea/hfTeritory` from `sp_Get_subTerritoryData_BySubTerritoryId` (`.cs:65-91`), then the control's `Page_Load` (which runs after the page's) binds them (BR-M03-003); interactive cascade uses Active-only loaders (BR-M03-001).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Submit/Update | `btnSave_Click` `.cs:104` | `Validation()` → `Setup2DAL.SaveSubTerritory(dto, int.Parse(UserId))` | see §16 | insert/update |
| Reset | `restbtn_Click` `.cs:152` | redirect self | – | – |

## 11. Frontend Flow
Load: `?id` → `GetOneRecord` (server) fills name, date, active flag and hidden ids (`.cs:56-102`). UpdatePanel postbacks; `pageLoad()` re-inits select2/pickadate. Legacy inline JS (`$(function…)` at `.aspx:195`) additionally runs `GetData(masterid)` → `Setup.aspx/GetSubTerrritoryEditData` when `?id` is present and `GetGroup(0)` → `SeedData.aspx/GetGroupList` on a new form; it targets element ids (`#GroupNameSelect`, `#zoneSelect`…) that do not exist on this page, so effects are none/harmless (INFERRED) but the AJAX calls are still issued.

## 12. Backend Flow
`btnSave_Click`: `Validation()` → build DTO → `SaveSubTerritory` (Setup2DAL.cs:722-822):
- **Edit (`SubTerritoryId>0`)**: `sp_check_SubTerritoryInfo(@id,@TerritoryId,@Name)`; rows>0 → `isSuccess=false` (no other flag) → UI "Already Exist!". rows=0 → if `IsActive==false`: `sp_check_Vali_MarketStructure(@MasterId,@PageName="SubTerritory")`; rows>0 → `isValiCheck=true` ("Data cannot be deactivated!"); rows=0 → `sp_Update_SubTerritoryData`. If `IsActive==true` → `sp_Update_SubTerritoryData` directly.
- **New**: `pk = SaveDataReturnPrimaryKey("sp_Save_SubTerritoryInfo")`; `isSuccess = true` **unconditionally** (no pk>0 test, no duplicate pre-check in C#).
- Implicit transaction: one DB transaction per call, committed at `SqlConnectionClose()` (BR-M03-101). Exceptions → rollback + rethrow (page has no try/catch).
Result mapping in page (`.cs:125-148`): isSuccess → success alert → `SubTerritoryRecords.aspx`; isDuplicateCheck → "Already Exist!"; isValiCheck → "Data cannot be deactivated!"; else → "Already Exist!".

## 13. API Mapping
Page has no `[WebMethod]`. (Sibling host page `Setup.aspx` exposes `SaveSubTerritory` used by the legacy inline JS; documented under its own module owner.)

## 14. Service Mapping
No BLL; `Setup2DAL` instance field `_setupDAL` (non-static).

## 15. Repository Mapping
| DAL | Source | SP |
|---|---|---|
| `Setup2DAL.SaveSubTerritory` | Setup2DAL.cs:722 | `sp_check_SubTerritoryInfo`, `sp_check_Vali_MarketStructure`, `sp_Update_SubTerritoryData`, `sp_Save_SubTerritoryInfo` |
| `Setup2DAL.GetEditData_SubTerritoryOne` | Setup2DAL.cs:909 | `sp_Get_subTerritoryData_BySubTerritoryId(@id)` |
| control loaders | CommonDataLoad.cs | see [MS-CTRL] |

## 16. Stored Procedure Mapping
| SP | Nature | Params (from DAL) | Body |
|---|---|---|---|
| `sp_Save_SubTerritoryInfo` | write (returns identity via scalar) | `@id,@TerritoryId,@Name,@createdBy,@isActive,@acInAcDate` | NOT in repo |
| `sp_Update_SubTerritoryData` | write | same | NOT in repo |
| `sp_check_SubTerritoryInfo` | read (duplicate check; rows = conflict) | `@id,@TerritoryId,@Name` | NOT in repo |
| `sp_check_Vali_MarketStructure` | read (rows = has dependants) | `@MasterId,@PageName` | NOT in repo (shared by Group/Zone/Area/Territory/SubTerritory/Market/Dept/Designation/… pages) |
| `sp_Get_subTerritoryData_BySubTerritoryId` | read | `@id` → columns used: `SubTerritoryName,AcOrInAcDate,GroupId,RegionId,IsActive,AreaId,TerritoryId` | NOT in repo |

## 17. Database Mapping
No inline SQL; tables UNKNOWN (SP-only). Column names surfaced by the edit SP: `SubTerritoryName, AcOrInAcDate, GroupId, RegionId, AreaId, TerritoryId, IsActive`.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Area | required | no | `.cs:172` | tooltip "please fill out this field" | BR-M03-027 |
| Territory | required | no | `.cs:182` | same | BR-M03-027 |
| Name | required (non-empty; no trim, no length/charset rule) | no | `.cs:190` | same | BR-M03-027 |
| Active date | required | no | `.cs:200` | same | BR-M03-027 |
| Duplicate name in territory (edit only) | SP result | – | `SaveSubTerritory` | "Already Exist!" | BR-M03-028 |
| Deactivate with dependants | SP result | – | `SaveSubTerritory` | "Data cannot be deactivated!" | BR-M03-029 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-027 | Area, Territory, Name and Active/Inactive date are mandatory; Group/Zone are not. | SubTerritorySetup.aspx.cs:172-206 | Validation | CONFIRMED |
| BR-M03-028 | On **edit**, a Sub-Territory name must be unique per parent Territory (via `sp_check_SubTerritoryInfo`); rows returned ⇒ save refused, generic "Already Exist!". The exact uniqueness key is UNKNOWN (SP body). | Setup2DAL.cs:747-780 | DAL | CONFIRMED (call) / UNKNOWN (semantics) |
| BR-M03-029 | A Sub-Territory cannot be set inactive while `sp_check_Vali_MarketStructure(@PageName='SubTerritory')` returns rows (dependants such as active markets — semantics UNKNOWN). | Setup2DAL.cs:754-769 | DAL | CONFIRMED (call) |
| BR-M03-030 | On **create** there is no C# duplicate check and `isSuccess` is forced `true`; a duplicate would have to be rejected inside `sp_Save_SubTerritoryInfo` (UNKNOWN) — if the SP returns 0/null the page still reports success. | Setup2DAL.cs:784-788 | DAL | CONFIRMED |
| BR-M03-031 | New records default to Active (checkbox pre-checked); label switches Active Date/Inactive Date. Editing preloads all levels including inactive parents. | SubTerritorySetup.aspx:122,277; .cs:73-85 | UI | CONFIRMED |
Also: BR-M03-001/002/003 (control).

## 20. Permission Matrix
`Session["UserId"]` (unguarded, `.cs:124`); control reads `RoleTypeName/EmpInfoId/RoleTypeId`. No page-level role check; menu-only; no `<authorization>` in Web.config. Role scoping only pre-locks Group/Zone/Area selectors (BR-M03-002) — an NSM/DZSM/AM user can create sub-territories only under their own visible parents, an Admin/other role anywhere.

## 21. Status Lifecycle
Flag `IsActive` (bool) + `AcOrInAcDate`: `Active` ⇄ `Inactive`. Set by `chkIsActive` (`.cs:120`). Transition Active→Inactive guarded by BR-M03-029; Inactive→Active unguarded. No delete.

## 22. Error Handling
`GetOneRecord` swallows all exceptions (page silently empty if edit SP fails). Save errors bubble unhandled. Messages via sweet-alert helpers.

## 23. Audit
`@createdBy` = UserId on both insert and update (no separate `@updatedBy` param sent for update); date columns SP-side/UNKNOWN. No change log.

## 24. Integration
None.

## 25. Reports
None (list page PAGE-056).

## 26. Dependencies
`Setup2DAL`, `CommonDataLoad`, user control; shares `sp_check_Vali_MarketStructure` with all hierarchy setup pages.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Submit with Area empty | Area highlighted, no DB write | BR-M03-027 |
| TC-02 | Submit with Territory empty | Territory highlighted | BR-M03-027 |
| TC-03 | Empty name / empty date | field highlighted | BR-M03-027 |
| TC-04 | Create valid | "Operation successful!" → list | BR-M03-030 |
| TC-05 | Edit, rename to existing name in same territory | "Already Exist!" | BR-M03-028 |
| TC-06 | Edit, untick Active for sub-territory with active markets | "Data cannot be deactivated!" | BR-M03-029 |
| TC-07 | Edit and re-activate | saved | BR-M03-031 |
| TC-08 | Create duplicate name | outcome depends on SP; page reports success even if pk=0 | BR-M03-030 |
| TC-09 | Login as AM | Group/Zone locked | BR-M03-002 |
| TC-10 | Open with `?id=` of unknown record | blank form (swallowed exception) | §22 |

## 28. Reverse Engineering References
`Solution.Web/DoctorModule_UI/SubTerritorySetup.aspx(.cs)`, `MasterSetup_UI/IVMarketStructureMarketForSubTeriSetup.ascx(.cs)`, `Library.DAL/DoctorModule_DAL/Setup2DAL.cs:722-927`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Uniqueness rule | None existed | edit-only via SP | absent | UNKNOWN | BR-M03-028/030 |
| Deactivation rule | None existed | delegated to SP | absent | UNKNOWN | BR-M03-029 |

## 30. Known Gaps
- Create path lacks duplicate check & forced success (BR-M03-030).
- Moving a sub-territory to another parent territory via edit changes `TerritoryId` param without the approval workflow of PAGE-100 (whether `sp_Update_SubTerritoryData` updates the parent is UNKNOWN) — GAP-M03-010.
- Legacy dead JS issuing AJAX calls; "Is Structure Change" checkbox unbound; `@createdBy` reused for update; swallowed edit-load errors; SP bodies absent (GAP-M03-001).

## 31. Change Impact
`Setup2DAL.SaveSubTerritory` used by this page and `Setup.aspx/SaveSubTerritory` (WebMethod host page, `Setup.aspx.cs:328`). `sp_check_Vali_MarketStructure` shared by 20+ callers (grep: CommonDataLoad.cs:1589, Setup2DAL.cs:249/651/759/1238, ZoneSetupDAL.cs:135, GroupSetupDal.cs:95, Department/Designation/GenericGroup DALs, SetupDAL.cs). Sub-Territory is a parent key for Market (PAGE-042) and a From/To level for PAGE-096/100.
