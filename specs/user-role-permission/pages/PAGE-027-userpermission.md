---
page_id: PAGE-027
module: M02
page: UserPermission (role menu permission)
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-027 — UserPermission (Role → Menu permission: Permission / Add / View / Edit / Delete, Web & App)

> **Evidence level:** Tier A — aspx (all JS), code-behind, `MenuDAL`, `MenuRoleDAO`, `MenuRoleMasterDAO` read. SPs `sp_GET_UserRoleDropDown`, `sp_GET_MainMenuRole2`, `sp_GET_MainMenuRoleIsApp`, `sp_Delete_ByRoleIDTypeId`, `sp_Save_MenuRole`, `sp_Delete_PreviousmenuByRoleID` are **not in the repository** (database PARTIAL). Do not confuse with the legacy per-user page PAGE-008 (`CommonUI/UserPermission.aspx`, M01).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-027 |
| Module | M02 |
| Kind | Admin maintenance (AJAX single-page form) |
| Folder | `Solution.Web/UserPermission` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `UserPermission_UserPermission` (all logic in 4 static WebMethods, `Page_Load` empty) |
| Size | aspx 696 lines (lines ≈140-230 are a commented/legacy duplicate UI), code-behind 82 |

## 2. Purpose
For a chosen **user role** and channel (**Web** or **App**) show every menu row (with its parent name) and let the administrator tick which menus the role may access (`Permission`) and, for each, whether Add / View / Edit / Delete are allowed. Save replaces the role's permission set.

## 3. Business Objective
Role-based access control: decide, per role, which screens appear in the navigation and which operations are allowed on them. This is the writer of the role-menu data that the menu builder (`dbo.MainMenu2`) and the page-level check (`sp_GET_MainPermissionByUserRoleandPageUrl`, 17 pages) read. INFERRED for consumers (bodies missing); writer behaviour CONFIRMED.

## 4. Navigation
No inbound link in code; reached via DB-driven menu (UNKNOWN row) or URL. Comment in aspx points to `../DoctorModule_UI/UserRoleEntry.aspx` for creating roles (`:63`, commented). Success → `successalert(...,'UserPermission.aspx')` (reload). Menu placement itself is DB data — the earlier generated statement about `sp_GET_MainMenuRole2` in the master page is incorrect; here `sp_GET_MainMenuRole2` is the *data source of this page's grid*.

## 5. Route
`~/UserPermission/UserPermission.aspx`; AJAX endpoints `UserPermission.aspx/{GetUserRole,LoadMenu,LoadMenuIsApp,SaveMenu}`.

## 6. Source Files
`Solution.Web/UserPermission/UserPermission.aspx(.cs)`, `Library.DAL/UserRoleDAL/MenuDAL.cs` (namespace `SalesSolution.Web.DataLayer`), `Library.DAO/UserRoleDAO/MenuRoleDAO.cs`, `MenuRoleMasterDAO.cs`, `Solution.Web/CustomScript/_myCusGen_Func.js` (`SelectOption_ListObjectReturns_Async_True`).

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| `#roleSelect` (select2) | Role Type (actually **user role**: options `UserRoleID`/`RoleName`) | client: value must not be `0`/empty | none | filled by `GetUserRole` → `sp_GET_UserRoleDropDown`; `onchange=GetRoleMenuData()` |
| `#typeSelect` | Type | – | `Web` (value 1); options 0 "Select One", 1 Web, 2 App | `readonly-select` CSS class only; `onchange=GetRoleMenuData()` |
| Save button | Save | – | – | `onclick=Save()` |
| Reset button | Reset | – | – | `ResetLink()` |
| `#dtTble` (rows) | columns: SL, **Permission**, Parent menu, Menu, Add, View, Edit, Delete | – | – | one row per menu; hidden `slid<i>` = menu SL; checkboxes `permission<i>`, `add<i>`, `view<i>`, `edit<i>`, `delete<i>` |
| coverScreen | busy overlay | – | – | shown during AJAX |
Row rendering rules (JS `GetRoleMenuWebData`/`GetRoleMenuAppData`): `Permission` ticked when `Permission=="1"`; a flag checkbox is **disabled** when the menu row's capability field (`Add/View/Edit/Delete`) is `"0"`, and **checked** when the role's current value (`RAdd/RView/REdit/RDelete`) is `"1"`. For the **App** type the four flag checkboxes are hidden (`display:none`) — only Permission is visible. Parent name is printed only on the first row of each parent group. `CheckRowCheck(i)`: ticking Permission ticks every enabled flag; unticking clears all four.

## 8. Field Mapping
| UI | JSON field (`MenuRoleDAO`) | WebMethod → DAL | SP param | Column (INFERRED) |
|---|---|---|---|---|
| `#roleSelect` | `RoleId` | `SaveMenu` → `MenuDAL.SaveMenuRole` | `@RoleId` | role-menu table `RoleId` |
| `slid<i>` | `SL` | same | `@SL` | menu id (`tblMainMenu.SL`) |
| `add<i>`/`view<i>`/`delete<i>`/`edit<i>` | `Add/View/Delete/Edit` (bool) | same | `@Add,@View,@Delete,@Edit` | flags |
| `permission<i>` | `Permission` (bool) | same | `@Permission` | menu access flag |
| `#roleSelect`, `#typeSelect` | `roleSelectid`, `typeSelectId` (extra WebMethod args) | `MenuDAL.Delete_PrevoiusmenuAll` | `@RoleId`, `@TypeId` (`sp_Delete_ByRoleIDTypeId`) | delete scope |
`MenuRoleDAO.MenuRoleId` is never sent. The insert SP has **no type parameter**: `@TypeId` is used only for the delete and for loading; how Web/App rows are distinguished in the stored data is UNKNOWN (SP `sp_Save_MenuRole` body absent).

## 9. Dropdown Mapping
`roleSelect` ← `GetUserRole` → `MenuDAL.GetUserRoleDropDown` → `sp_GET_UserRoleDropDown` (fields `UserRoleID`, `RoleName`).

## 10. Button Action Mapping
| Action | JS | Server |
|---|---|---|
| role/type change | `GetRoleMenuData()` (`type==1` → `LoadMenu`, `type==2` → `LoadMenuIsApp`; type 0 does nothing) | `MenuDAL.LoadMenu(roleId,typeId)` → `sp_GET_MainMenuRole2(@RoleId,@TypeId)`; `LoadMenuIsApp` → `sp_GET_MainMenuRoleIsApp(@RoleId,@TypeId=true)` (bool constant, not the selector) |
| Save | `Save()` → `Validation()` → `FinalSave()` | `SaveMenu(MenuRoleMasterDAO arole, string roleSelectid, string typeSelectId)` |

## 11. Frontend Flow
`ready`: `GetRole(0)`. `FinalSave` iterates `#dtTble > tbody > tr` (index `i` maps to element ids `slid<i>` etc.), builds `{Id:0, MenuRoles:[…]}` **including only rows whose Permission box is ticked**, and POSTs `{arole, roleSelectid, typeSelectId}` as JSON. Success callback ignores the returned string and shows `successalert('Operation successful!','Success','UserPermission.aspx')`; the `error` callback is empty (failures are silent).
Client-side validation: role must not be `0`.

## 12. Backend Flow (`UserPermission.aspx.cs:42-62`)
`SaveMenu`: (1) `MenuDAL.Delete_PrevoiusmenuAll(roleSelectid, typeSelectId)` → `sp_Delete_ByRoleIDTypeId(@RoleId,@TypeId)`; (2) for each `MenuRoleDao` in `arole.MenuRoles`: `MenuDAL.SaveMenuRole` → `SaveDataReturnPrimaryKey("sp_Save_MenuRole", @SL,@RoleId,@Add,@View,@Delete,@Edit,@Permission)` (result `id` ignored); (3) returns the string `"True"` unconditionally. There is **no transaction** — delete and inserts are separate calls on separate connections (each DAL method opens/closes) — so a failure after the delete leaves the role with a partial or empty permission set. There is no verification that `roleSelectid` equals each row's `RoleId`, that the SL exists, or that a capability flag was disabled in the UI.
Read side: `LoadMenu`/`LoadMenuIsApp` return `accessManager.Serialize(dr)` (dynamic list serialised by the page-method JSON handler).

## 13. API Mapping
| WebMethod | Params | Returns | Session/role check |
|---|---|---|---|
| `GetUserRole()` | – | dynamic list | none |
| `LoadMenu(roleId,typeId)` | strings | dynamic list | none |
| `LoadMenuIsApp(roleId,typeId)` | strings | dynamic list | none |
| `SaveMenu(arole, roleSelectid, typeSelectId)` | `MenuRoleMasterDAO`, strings | `"True"` | **none** |
(An older `SaveMenu(List<MenuRoleDAO>)` variant is commented out, `:22-41`.)

## 14. Service Mapping
None (static `MenuDAL aMenuDal` used directly).

## 15. Repository Mapping (`Library.DAL/UserRoleDAL/MenuDAL.cs`)
`LoadMenu` (:14), `LoadMenuIsApp` (:44), `GetUserRoleDropDown` (:72), `SaveMenuRole` (:99), `Delete_Prevoiusmenu` (:133, unused: `sp_Delete_PreviousmenuByRoleID`), `Delete_PrevoiusmenuAll` (:165). All open `DataBase.SalesDB`, close in `finally`; the `Delete_*` methods set `isSuccess=true` in `finally` (so the returned `ResultInfo` always says success) but rethrow on exception.

## 16. Stored Procedure Mapping
| SP | Params | Purpose | Body in repo |
|---|---|---|---|
| `sp_GET_UserRoleDropDown` | – | roles for dropdown | no |
| `sp_GET_MainMenuRole2` | `@RoleId,@TypeId` | menu tree with capability flags (`Add/View/Edit/Delete/Permission`, `ParentName`, `ManuName`, `SL`) and the role's current values (`RAdd/RView/REdit/RDelete`) | no |
| `sp_GET_MainMenuRoleIsApp` | `@RoleId,@TypeId` (always `true`) | same for app menus | no |
| `sp_Delete_ByRoleIDTypeId` | `@RoleId,@TypeId` | delete role's rows for that type | no |
| `sp_Save_MenuRole` | `@SL,@RoleId,@Add,@View,@Delete,@Edit,@Permission` | insert one role-menu row (returns key) | no |

## 17. Database Mapping
UNKNOWN table names (role-menu table). Column names for the result set are known from JS: `SL, ParentName, ManuName, Permission, Add, View, Edit, Delete, RAdd, RView, REdit, RDelete`.

## 18. Validation Matrix
| Field | Rule | FE | BE | Message | BR |
|---|---|---|---|---|---|
| Role | not `0` | yes (`Validation()`) | **no** | tooltip `Please fill out of this field!` | BR-M02-033 |
| Type | `0`="Select One" allowed | no | no | – (Save with type 0 sends `typeSelectId=0`) | BR-M02-030 |
| Flags | disabled when capability=0 | yes (disabled attr) | no | – | BR-M02-031 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-028 | A role's access to a menu is the `Permission` flag; only rows with Permission ticked are sent and stored. Add/View/Edit/Delete of a row with Permission unticked are discarded. | `UserPermission.aspx` `FinalSave` (inner `if ($('#permission'+i).is(":checked"))`) | CONFIRMED |
| BR-M02-029 | Ticking Permission ticks all enabled Add/View/Edit/Delete boxes; unticking clears them. | `CheckRowCheck` | CONFIRMED |
| BR-M02-030 | Saving replaces the role's set for the selected **type** (Web=1/App=2): delete-all then insert-ticked, no transaction; a failure after the delete leaves the role with fewer/no permissions. | `UserPermission.aspx.cs:43-62`; `MenuDAL.cs:99-129,165-195` | CONFIRMED |
| BR-M02-031 | Add/View/Edit/Delete can only be granted where the menu declares that capability (UI disables the box when the capability value is `"0"`); **the server does not re-check**. | `UserPermission.aspx` row builders | CONFIRMED (UI) |
| BR-M02-032 | App-type editing shows only the Permission box; the hidden flag boxes keep their initial checked/unchecked state, and `FinalSave` still sends their `.is(":checked")` values. | `GetRoleMenuAppData` | CONFIRMED |
| BR-M02-033 | A role must be selected to save (client-side only). | `Validation()` | CONFIRMED |
| BR-M02-034 | The WebMethods perform **no authentication or authorization**: any HTTP client that can reach the URL can list roles/menus and overwrite any role's permission set. | `UserPermission.aspx.cs:42-82` (no `Session` reference) | CONFIRMED |
| BR-M02-035 | The `Add/Edit/Delete` flags are stored but the only UI enforcement code in the repository is `RAdd/REdit` on 3 list pages (`UserRecords`, `CustomerView`, `DoctorView`); elsewhere the reading code is commented out (10 approval pages, `AdminDashboard`, `AppMonitoringList`). `Delete` and `View` flags are never consumed in C#. | grep of `["RAdd"]`, `["REdit"]`, `["RDelete"]`, `["RView"]` | CONFIRMED |

## 20. Permission Matrix
Master-page first-GET login check protects only the HTML page. The four WebMethods are static page methods (no page lifecycle, no master page) and contain no `Session` test → **unauthenticated callers can modify role permissions** (CONFIRMED absence of checks; runtime not executed). Even for authenticated users there is no check that the caller is an administrator (role `2` bypass logic exists elsewhere but not here).

## 21. Status Lifecycle
None (no status; rows are replaced).

## 22. Error Handling
DAL rethrows; WebMethod has no try/catch; the client `error` handler is empty → silent failure; the success handler runs on any HTTP 200 regardless of content.

## 23. Audit
None: no EntryBy/EntryDate parameters are sent; no history of who changed which role's permissions (the SP might stamp — UNKNOWN).

## 24. Integration
_Sections 24–25 are documented together in this block._
None.



## 25. Reports
See §24 block above (combined §24–25).

## 26. Dependencies
`MenuDAL`, `DataAccessManager.Serialize`, jQuery/select2, `_myCusGen_Func.js`, `sp_*` above, role table maintained by PAGE-013.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Choose role + Web | grid of all menus with role's current ticks | 028 |
| TC-02 | Tick Permission on row | A/V/E/D enabled boxes auto-ticked | 029 |
| TC-03 | Save with 3 rows ticked | role's previous Web rows removed, 3 inserted | 030 |
| TC-04 | Save with none ticked | role's Web set becomes empty | 030 |
| TC-05 | Role not chosen | client tooltip, no call | 033 |
| TC-06 | Type = App | only Permission column visible | 032 |
| TC-07 | POST `SaveMenu` JSON with no cookie | server processes it (verify) | 034 |
| TC-08 | Tamper `RoleId` in one row vs `roleSelectid` | rows for another role inserted | 030, 034 |
| TC-09 | Interrupt after delete (DB error) | partial permissions | 030 |

## 28. Reverse Engineering References
`Solution.Web/UserPermission/UserPermission.aspx(.cs)`, `Library.DAL/UserRoleDAL/MenuDAL.cs`, `Library.DAO/UserRoleDAO/MenuRoleDAO.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Server-side authorization of the admin function | none | none | GAP-M02-001 |
| Add/Edit/Delete/View semantics | none | flags stored; largely unenforced | GAP-M02-003 |

## 30. Known Gaps
GAP-M02-001, 003, 004 (no transaction), 005 (SP bodies), 012 (type semantics/`@TypeId` unknown in insert), 013 (no audit).

## 31. Change Impact
Data written here is read by the menu function `dbo.MainMenu2` (M01 WF-M01-02) and by `sp_GET_MainPermissionByUserRoleandPageUrl` (17 list/approval/dashboard pages). `MenuDAL.LoadMenu` (`sp_GET_MainMenuRole2`) has no other C# caller; `ApprovalStepMap.aspx` (PAGE-026) contains a leftover JS function that also posts to `UserPermission.aspx/LoadMenu`.
