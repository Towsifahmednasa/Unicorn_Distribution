# WF-M02-03 — Role menu permission (Permission / Add / View / Edit / Delete)

**Trigger:** admin opens `UserPermission/UserPermission.aspx` — PAGE-027.
1. `GetUserRole` → `sp_GET_UserRoleDropDown`; choose role and type (Web 1 / App 2).
2. `LoadMenu` → `sp_GET_MainMenuRole2(@RoleId,@TypeId)` or `LoadMenuIsApp` → `sp_GET_MainMenuRoleIsApp` fill the grid with capability flags and current values.
3. Admin ticks Permission (auto-ticks enabled flags) and flags.
4. `Save()` → POST `SaveMenu(arole, roleSelectid, typeSelectId)` with **only Permission-ticked rows**.
5. Server: `sp_Delete_ByRoleIDTypeId(@RoleId,@TypeId)` then `sp_Save_MenuRole` per row; returns "True" (no transaction, no auth) — `UserPermission.aspx.cs:43-62`.

**Effect (INFERRED):** the menu function `dbo.MainMenu2(UserId,UserRoleID)` and `sp_GET_MainPermissionByUserRoleandPageUrl` (17 pages) read this data; bodies UNKNOWN. **Exceptions:** mid-way failure leaves a partial set; tampered `RoleId`; silent client errors. **BR:** 028-035. **Gaps:** 001, 003, 004, 012, 013, 027.
