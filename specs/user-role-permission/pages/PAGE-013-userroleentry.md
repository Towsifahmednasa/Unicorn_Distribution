---
page_id: PAGE-013
module: M02
page: UserRoleEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-013 — UserRoleEntry (create / edit a user role)

> **Evidence level:** Tier A — aspx JS, `FieldForce.aspx.cs` WebMethods, `RSMSetupDal.Save_UserRoleInfo/GetUserRoleEditDataDAL`, `_QuickDataAccess.js`. SPs `sp_Save_UserRoleInfo`, `sp_UD_UserRoleInfo`, `sp_check_UserRoleInfo`, `sp_GET_UserRoleInfo_ById` are **not in repo**. The role-type list is served by `CommonDataLoad.aspx/GetRoleTypeInfo` (not read).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-013 |
| Module | M02 |
| Kind | Entry/edit form (pure AJAX; code-behind empty) |
| Folder | `Solution.Web/DoctorModule_UI` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `DoctorModule_UI_UserRoleEntry` (`Page_Load` empty) |

## 2. Purpose
Define a **user role** (name, role type, active flag, activation date). Roles are what users are assigned in UserSetup and what menu permissions attach to (PAGE-027).

## 3. Business Objective
Maintain the role catalogue that drives menu access and DC-related UI; each role belongs to a **role type** (category, e.g. hierarchy level / "DIC") used for approvals (`Session["RoleTypeId"]`, `RoleTypeName`). INFERRED.

## 4. Navigation
Inbound: `UserRoleRecords.aspx` "New Entry" and edit (`UserRoleEntry.aspx?id=`), link in UserSetup role label. Outbound: success → `UserRoleRecords.aspx`.

## 5. Route
`~/DoctorModule_UI/UserRoleEntry.aspx[?id=<UserRoleID>]`; endpoints `FieldForce.aspx/Save_UserRoleInfo`, `FieldForce.aspx/GetUserRoleEditData`, `CommonDataLoad.aspx/GetRoleTypeInfo`.

## 6. Source Files
`UserRoleEntry.aspx`, `Solution.Web/DoctorModule_UI/FieldForce.aspx.cs:90-94,146-150`, `Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:175-248,556-…`, `Library.DAO` `UserRoleDao`, `Solution.Web/CustomScript/_QuickDataAccess.js:501-…`.

## 7. UI Control Inventory
| Control | Label | Required | Default |
|---|---|---|---|
| `#userRole` | User Role | yes (not empty, not "0") | empty |
| `#RoleTypeSelect` | Role Type | yes (not ""/"0") | loaded by `_GetRoleTypeInfo` |
| `#customSwitch1` (checkbox) | Is Active | – | checked; `IsActiveChange()` toggles the date label |
| `#acDate` | Active/Inactive date | yes (non-empty) | pickadate |
| `#masterId` (hidden) | – | – | `0` = create; `?id=` = edit |
| Save/Update button, Reset | – | – | button text becomes "Update" in edit mode |

## 8. Field Mapping
| UI | JSON (`UserRoleDao`) | DAL param | Column |
|---|---|---|---|
| `#masterId` | `UserRoleID` | `@UserRoleID` | `tbl_UserRoleInfo.UserRoleID` |
| `#userRole` | `RoleName` | `@RoleName` | `RoleName` |
| `#RoleTypeSelect` | `RoleTypeId` | `@RoleTypeId` | `RoleTypeId` |
| `#customSwitch1` | `IsActive` | `@isActive` | `IsActive` |
| `#acDate` | `Activedate` (sic) → `ActiveDate` | `@acInAcDate` | active/inactive date |
| `Session["UserId"]` | – | `@entryBy` | audit |
(Table name `tbl_UserRoleInfo` is confirmed by the login/profile SQL; DAO fields `EntryBy/EntryDate/UpdateBy/UpdateDate/InActiveBy/ActiveInActiveDate` exist in `UserRoleDAO`.)

## 9. Dropdown Mapping
`_GetRoleTypeInfo` → `CommonDataLoad.aspx/GetRoleTypeInfo` (`RoleTypeId`/`RoleType`).

## 10. Button Action Mapping
`Save()` → `Validation()` → `FinalSave()` → POST `FieldForce.aspx/Save_UserRoleInfo {aRoleInfo}`.

## 11. Frontend Flow
`ready`: if `?id=` → `GetData(id)` → POST `GetUserRoleEditData {id}` → fill fields, switch button to "Update", set role type, date, active switch; else `GetRoleTypeInfo(0)`. Result handling: `isSuccess` → `successalert('Operation successful!','Success','UserRoleRecords.aspx')`; `isValiCheck` → `faildalert('Data cannot be deactivated!','Faild')`; otherwise `faildalert('Operation Faild!')`; HTTP error → same fail alert.

## 12. Backend Flow
`Save_UserRoleInfo(UserRoleDao)` → `RSMSetupDal.Save_UserRoleInfo(aRoleInfo, Convert.ToInt32(Session["UserId"]))`: if `UserRoleID > 0` (edit): `sp_check_UserRoleInfo(@RoleName,@UserRoleID)`; **0 rows** → `sp_UD_UserRoleInfo` (`isSuccess`, `pk=UserRoleID`); rows returned → `isValiCheck=true`. Create (`UserRoleID == 0`): `pk = sp_Save_UserRoleInfo` (no pre-check in C#). Then `pk>0` → success else `isValiCheck`. Exceptions: connection closed, `isSuccess=false`, rethrown. `Session["UserId"].ToString()` throws NRE when there is no session (so this endpoint, unlike PAGE-027, fails without a session — an implicit but unintended guard).

## 13. API Mapping
| WebMethod | Params | Returns | Auth |
|---|---|---|---|
| `FieldForce.aspx/Save_UserRoleInfo` | `UserRoleDao aRoleInfo` | `ResultInfo` | implicit (needs `Session["UserId"]`) |
| `FieldForce.aspx/GetUserRoleEditData` | `int id` | `UserRoleDao` | none |
| `FieldForce.aspx/GetUserRoleList` (list page) | – | JSON | none |

## 14. Service Mapping
_Sections 14–15 are documented together in this block._
No BLL; `RSMSetupDal` methods above.



## 15. Repository Mapping
See §14 block above (combined §14–15).

## 16. Stored Procedure Mapping
`sp_Save_UserRoleInfo(@UserRoleID,@RoleName,@RoleTypeId,@entryBy,@isActive,@acInAcDate)`; `sp_UD_UserRoleInfo` (same params); `sp_check_UserRoleInfo(@RoleName,@UserRoleID)`; `sp_GET_UserRoleInfo_ById(@id)` (returns `UserRoleID, RoleTypeId, RoleName, IsActive, ActiveDateStr`). Bodies UNKNOWN.

## 17. Database Mapping
`tbl_UserRoleInfo` (UserRoleID, RoleName, RoleTypeId, IsActive, dates) — write via SP (UNKNOWN); `tblRoleType` read via SP.

## 18. Validation Matrix (client-side only)
| Field | Rule | Message | BR |
|---|---|---|---|
| Role name | non-empty, ≠"0" | `Please fill out of this field!` | BR-M02-042 |
| Role type | non-empty, ≠"0" | same | BR-M02-042 |
| Date | non-empty | same | BR-M02-042 |
Server: name uniqueness/in-use rule on edit only (BR-M02-043).

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-042 | Name, role type and activation date are mandatory (client only); a role is active by default. | `UserRoleEntry.aspx` `Validation()`, `#customSwitch1 checked` | CONFIRMED |
| BR-M02-043 | On **edit**, the save is refused when `sp_check_UserRoleInfo(@RoleName,@UserRoleID)` returns rows (semantic — name clash and/or users assigned — UNKNOWN); the UI then says "Data cannot be deactivated!" regardless of what was being attempted. | `RSMSetupDal.cs:191-206`; `UserRoleEntry.aspx` `FinalSave` | CONFIRMED (flow) / UNKNOWN (rule) |
| BR-M02-044 | On **create** there is no duplicate-name check in C#. | `RSMSetupDal.cs:216-219` | CONFIRMED |
| BR-M02-045 | Role writes are stamped with the session **UserId** (`@entryBy`). | `FieldForce.aspx.cs:93` | CONFIRMED |

## 20. Permission Matrix
Master-page guard for the page; the Save endpoint reads `Session["UserId"]` (throws if absent) but never checks who the user is. No role restriction: any logged-in user can create/rename/deactivate roles (menu-hiding only).

## 21. Status Lifecycle
`IsActive` true/false (+ date). Deactivation may be blocked by the check SP (UNKNOWN). Effect of an inactive role on login/menu is UNKNOWN (login SQL does not test role activity).

## 22. Error Handling
Client alerts as in §11; DAL rethrows → HTTP 500 → generic fail alert.

## 23. Audit
`@entryBy`; `UserRoleDAO` shows EntryBy/EntryDate/UpdateBy/UpdateDate/InActiveBy columns (exact SP handling UNKNOWN).

## 24. Integration
_Sections 24–25 are documented together in this block._
None.



## 25. Reports
See §24 block above (combined §24–25).

## 26. Dependencies
`FieldForce.aspx` (page-methods host), `CommonDataLoad.aspx`, `_QuickDataAccess.js`, `tbl_UserRoleInfo`, `tblRoleType`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | New role "X", role type, date | created | 042 |
| TC-02 | New role with an existing name | may create a duplicate (no C# check; SP unknown) | 044 |
| TC-03 | Rename to an existing name | `Data cannot be deactivated!` | 043 |
| TC-04 | Edit with blank date | `Please fill out of this field!` | 042 |
| TC-05 | Save with no session | HTTP 500 → `Operation Faild!` | 045 |
| TC-06 | Untick active on a role with users | may be blocked (UNKNOWN) | 043 |

## 28. Reverse Engineering References
`Solution.Web/DoctorModule_UI/UserRoleEntry.aspx`, `FieldForce.aspx.cs`, `Library.DAL/DoctorModule_DAL/RSMSetupDal.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Message on block | – | always "cannot be deactivated" | GAP-M02-022 |

## 30. Known Gaps
GAP-M02-001, 005, 022 (misleading message; no create duplicate check; client-only validation), 023 (role ids hard-coded elsewhere: 2, 14, 18, 20, 25, 29).

## 31. Change Impact
Role ids and role type ids are hard-coded in code: `UserRoleID == "2"` (bypass of page permission check, 32 code sites), DC-permission roles 29/14/25/18/20 (UserSetup), 14 (UserSetupforEMP); `RoleTypeId` 2/3/4/5/9/14 and `RoleTypeName=="DIC"` in approval/dashboard pages. Renumbering or re-creating roles breaks these.
