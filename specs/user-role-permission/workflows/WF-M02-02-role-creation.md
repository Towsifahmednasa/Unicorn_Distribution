# WF-M02-02 — Role creation / edit

**Trigger:** admin opens `UserRoleEntry.aspx` (from `UserRoleRecords.aspx` or the UserSetup role label) — PAGE-013.
1. Load role types (`CommonDataLoad.aspx/GetRoleTypeInfo`); in edit mode `FieldForce.aspx/GetUserRoleEditData` → `sp_GET_UserRoleInfo_ById`.
2. Client validation: name, role type, date (BR-M02-042).
3. POST `FieldForce.aspx/Save_UserRoleInfo` → `RSMSetupDal.Save_UserRoleInfo`: edit → `sp_check_UserRoleInfo` (0 rows) → `sp_UD_UserRoleInfo`; create → `sp_Save_UserRoleInfo` (BR-M02-043/044). `@entryBy` = `Session["UserId"]`.
4. UI: success → `UserRoleRecords.aspx`; check-hit → "Data cannot be deactivated!".

**Data:** `tbl_UserRoleInfo`. **Next:** grant menus (WF-M02-03), assign to users (WF-M02-01). **Exceptions:** no session → NRE / HTTP 500; no create duplicate check. **BR:** 042-045. **Gaps:** 001, 005, 022.
