# WF-M02-01 — User provisioning (create / edit login account)

**Trigger/actors:** an administrator opens `UserSetup.aspx` (new, `?MID=`, or `?EMPMID=`) — PAGE-015 (PAGE-016 is the older variant). **Preconditions:** employee/DA/role master data exist; no server-side check on who the admin is (GAP-M02-001).

| # | Step | Evidence |
|---|---|---|
| 1 | `Page_Load`: load dropdowns (`sp_Get_EmployeeList`, `tblDAInfo` active, `sp_Get_UserRoleInfo`, `sp_Get_UserTypeInfo`, `sp_GET_DistributionCenter`); mode by query string | `UserSetup.aspx.cs:23-70,236-320` |
| 2 | Choose user type → employee panel (1/3) or DA panel (6) | `:583-597` |
| 3 | DA chosen → login and password prefilled with DA code; existing type-6 user for that DA loaded for edit | `:599-651` |
| 4 | Choose role → DC list box for roles 29/14/25/18/20/"DA ASSISTANCE" | `:859-890` |
| 5 | Submit → `Validation()` (BR-M02-001…003) → `UserInfoDAL.SaveUserInfo` | `:435-563` |
| 6 | Duplicate-check SP → `sp_Save_UserMaster_New` (create) or `sp_Update_UserMaster_new` (edit) → per-market `sp_Save_UserMarketDetail` → per-DC `sp_Save_UserCompanyUnit` | `UserInfoDAL.cs:330-470` |
| 7 | success → redirect `UserRecords.aspx` (or EmployeeRecords when `EMPMID`); failure/duplicate → "Already Exist!" | `:543-561` |

**Data:** `tblUser` (+ company units, user-market rows). **Status:** `UserStatus` Active/Inactive; `IsPasswordChange` set by SP (UNKNOWN) — drives forced change in M01 WF-M01-03. **Exceptions:** no transaction; last call's flag wins; swallowed load errors; null `EmpInfoId` parameter risk (GAP-M02-028); PAGE-016 `EmpID` defect (GAP-M02-011). **Postcondition:** user can log in (`UserStatus='active'`, plaintext password) with role/DC scope at next login.
**BR:** BR-M02-001…016. **Tests:** PAGE-015 §27, PAGE-016 §27.
