# WF-M15-01 Employee onboarding (employee + login + market assignment)

**Trigger / actors:** HR/admin opens `MasterSetup_UI/EmployeeRecords.aspx` (PAGE-612) -> "New Entry" -> `EmployeeSetup.aspx` (PAGE-613; legacy variant PAGE-615 `EmployeeSetupForNewJoiner.aspx`). Later corrections: PAGE-614.
**Preconditions:** masters exist - departments, designations, shifts (PAGE-566/568/585), user roles (with `RoleType`), depots, groups/zones/areas/territories, allowances (PAGE-583), product lines. Session `UserId` present. (PAGE-612 itself requires a permission row unless `UserRoleID=2`, BR-M15-080.)

## Steps
1. PAGE-613 `Page_Load` -> `LoadInitialInfo()` fills lookups (`CommonDataLoad`, `SeedDataDAL`).
2. User types employee code -> `txtEmpCode_TextChanged` copies it to login name and password (BR-M15-004).
3. Picks role -> `ddlUserRole_SelectedIndexChanged` -> `SeedDataDAL.GetUserRoleTypeListById` (`sp_GET_UserRoleInfoRoleType_ById`) -> `hfRoleType`; shows Group/Zone/Area/Territory or DIC depot panel (BR-M15-006, 008). Cascades: `GetZone_byGroupId_Active` -> `GetArea_ByZoneId_Active` -> `GetTerritory_ByAreaId_Active`.
4. Save -> `Validation()` (BR-M15-001..008; SP checks `sp_check_Vali_EmployeeInfoEntry` for code and for market-node vacancy).
5. `EmployeeInformationDaL.SaveEmployeeInformation` -> `sp_Save_EmployeeInformation` (returns `EmpInfoId`) -> sync `tblEmpwiseShiftDeclare` / `tblEmpwiseProductLineDeclare` (inline SQL) -> `sp_Save_EmployeeAllowanceDetail` per allowance. (Transaction 1.)
6. `UserInfoDAL.SaveUserInfo` -> `sp_check_UserInfo_Save` (0 rows) -> `sp_Save_UserMaster_New` with `EmpInfoId`, `UserTypeId=3`; DIC: `sp_Save_UserCompanyUnit` per depot. (Transaction 2; result ignored.)
7. By role type: `RSMSetupDal.Save_MIOInfo` / `Save_ASMInfo` / `Save_RSMInfo` / `Save_NSMInfo` -> `sp_Save_*Info(@EmployeeId=EmpInfoId, @TerritoryId|@AreaId|@RegionId|@GroupId, @isActive, @acInAcDate, @entryBy)`. (Transaction 3; result ignored.)
8. Alert `Operation successful!` -> `EmployeeRecords.aspx` (PAGE-613) or reload of the form (PAGE-615).

## Data created / changed
`tblEmpGeneralInfo` (INFERRED via `sp_Save_EmployeeInformation`), `tblEmpwiseShiftDeclare`, `tblEmpwiseProductLineDeclare`, employee-allowance detail, `tblUser` (+ user-depot), one of MIOInfo/ASMInfo/RSMInfo/NSMInfo. Status: `EmployeeStatus`/`UserStatus`/`IsActive` = Active or Inactive from one checkbox (BR-M15-009).

## Branches / exceptions
Duplicate code -> `Employee Code Already Exist in User!`; node already held -> `Employee  Already Exist in Territory/Area/Zone/Group!`; `SaveEmployeeInformation` returns 0 -> `Already Exist!`; login duplicate -> employee committed, user missing, still success message (GAP-M15-002). Post-creation edit: PAGE-614 edits employee data only (BR-M15-019..022); hierarchy/user changes only via FieldForce endpoints (PAGE-574) for hierarchy rows, none for user.

## Postconditions
Employee can log in (`Session["EmpInfoId"]` = new `EmpInfoId`, role from `UserRoleID`), appears in `GetEmployeeList_Active` pick-lists across the module, and (MIO/AM/DZSM/NSM) in the market-scoped lists/approvals.

**Related BR:** BR-M15-001..022, 080. **Pages:** 612, 613, 614, 615, 574. **Tests:** TC-613-01..13, TC-614-01..06, TC-615-01..06.
