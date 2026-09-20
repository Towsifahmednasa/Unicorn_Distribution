---
page_id: PAGE-615
module: M15
page: EmployeeSetupForNewJoiner
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-615 — EmployeeSetupForNewJoiner

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-615 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmployeeSetupForNewJoiner |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_EmployeeSetupForNewJoiner |
| aspx size | 48242 bytes |
| Code-behind LOC | 1297 |
| Inline script size | 1891 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED (file diff against `EmployeeSetup.aspx.cs`): the **earlier / simpler variant of the onboarding screen** (PAGE-613). Same one-click creation of employee + login + optional field-force hierarchy row, but with a **single shift drop-down** (legacy `EmployeeInformation.ShiftId`), **no depot (`CompanyUnitId`) and no product-line selection**, the **user role is optional**, and the **active date defaults to today**. After a successful save it reloads itself (ready for the next joiner) instead of going to the list. Page code is a near copy of PAGE-613 (`Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs`, 1296 lines vs 1379).

## 3. Business Objective
INFERRED from name and behaviour: fast data-entry of **new joiners** in bulk (page returns to a blank form after each save) - HR enters employee, gives the default login (= employee code) and, for sales staff, assigns the market node. Because role is optional an employee can be entered without a working login (menu access is decided by `UserRoleID`, which is then NULL - behaviour of login with null role UNKNOWN).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `employeerecords.aspx`, `userroleentry.aspx`

## 5. Route
`~/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStructure.ascx |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | EmployeeInformationDaL (Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs) |
| DAL class | RSMSetupDal (Library.DAL/DoctorModule_DAL/RSMSetupDal.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | UserInfoDAL (Library.DAL/DoctorModule_DAL/UserInfoDAL.cs) |
| DAO/entity class | ASMInfo (Library.DAO/DoctorModule_DAO/ASMInfo.cs) |
| DAO/entity class | EmployeeInformation (Library.DAO/DoctorModule_DAO/EmployeeInformation.cs) |
| DAO/entity class | MIOInfo (Library.DAO/DoctorModule_DAO/MIOInfo.cs) |
| DAO/entity class | NSMInfo (Library.DAO/DoctorModule_DAO/NSMInfo.cs) |
| DAO/entity class | RSMInfo (Library.DAO/DoctorModule_DAO/RSMInfo.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | UserDAO (Library.DAO/DoctorModule_DAO/UserDAO.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| hfRoleType | HiddenField |  | no validator found |  |  |  |  |
| txtEmpName | TextBox | Enter employee name  | no validator found |  |  |  |  |
| txtEmpCode | TextBox | Enter employee Code  | no validator found |  |  |  | OnTextChanged→txtEmpCode_TextChanged |
| chkIsTempEmployeeCode | CheckBox | Is Temp Employee Code | no validator found |  |  |  |  |
| txtEmpFatherName | TextBox | Enter father's name | no validator found |  |  |  |  |
| txtEmpMotherName | TextBox | Enter mother's name | no validator found |  |  |  |  |
| txtEmpdobDate | TextBox | Select date of birth | no validator found |  |  |  |  |
| txtEmergencyContactNo | TextBox | Emergency contact no | no validator found |  |  |  |  |
| FilteredTextBoxExtender3 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtEmrgContactNoRelaton | TextBox | Emergency Contact Person & Relarionship | no validator found |  |  |  |  |
| txtEmpAddress | TextBox | Enter present address | no validator found |  |  |  |  |
| txtEmpPresentAddress | TextBox | Enter permanent address | no validator found |  |  |  |  |
| GenderSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ReligionSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| Nationality | DropDownList |  | no validator found |  |  | see §9 |  |
| BloodGroupSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| MaritalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| txtNIDNO | TextBox | Enter NID No | no validator found |  |  |  |  |
| FilnidteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtEmail | TextBox | Employee email | no validator found |  |  |  |  |
| txtEmpContactNo | TextBox | Employee contact no | no validator found |  |  |  |  |
| FilteredTextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtDateofjoin | TextBox | Select from date | no validator found |  |  |  |  |
| DepartmentSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| DesignationSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ShiftSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ReferencePersonName | TextBox | Enter reference Person Name | no validator found |  |  |  |  |
| ReferenceContactNo | TextBox | Enter reference contact no | no validator found |  |  |  |  |
| FilteredTextBoxExtender2 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtLastCompanyName | TextBox | Last Company Name  | no validator found |  |  |  |  |
| txtLastJobLocation | TextBox | Last Company Name  | no validator found |  |  |  |  |
| txtJobLeftDate | TextBox | Select date of birth | no validator found |  |  |  |  |
| ddlMonthlyAllawance | ListBox |  | no validator found |  |  |  |  |
| chkIsProbation | input |  | no validator found |  |  |  |  |
| txtProbationEndDate | TextBox | Select date of birth | no validator found |  |  |  |  |
| chkIsActive | input |  | no validator found |  |  |  | onchange→IsActiveChange() |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| hfNSMId | HiddenField |  | no validator found |  |  |  |  |
| hfRSMId | HiddenField |  | no validator found |  |  |  |  |
| hfAMID | HiddenField |  | no validator found |  |  |  |  |
| hfMIOId | HiddenField |  | no validator found |  |  |  |  |
| hfUserId | HiddenField |  | no validator found |  |  |  |  |
| txtLoginName | TextBox |  | no validator found |  |  |  |  |
| txtPassword | TextBox |  | no validator found |  |  |  |  |
| (no id) | a | Go to this Page | no validator found |  |  |  |  |
| ddlUserRole | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ddlUserRole_SelectedIndexChanged |
| loadUserRole | LinkButton |  | no validator found |  |  |  | OnClick→loadUserRole_Click |
| chkMobileAccess | CheckBox | Is Mobile Access | no validator found |  |  |  | OnCheckedChanged→chkMobileAccess_CheckedChanged |
| txtImei1 | TextBox |  | no validator found |  |  |  |  |
| txtImei2 | TextBox |  | no validator found |  |  |  |  |
| rbDashboard | RadioButtonList |  | no validator found |  |  |  |  |
| GroupSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→GroupSelect_SelectedIndexChanged |
| ZoneSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→ZoneSelect_SelectedIndexChanged |
| AreaSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→AreaSelect_SelectedIndexChanged |
| TeritorySelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlDistributionCenter | ListBox |  | no validator found |  |  |  |  |
| txtacDate | TextBox | Select Date | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→btnSave_Click |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Update(this);, OnClick→btnSave_Click |
| btnReset | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| GroupSelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.GroupSelect_SelectedIndexChanged | — | CommonDataLoad.GetZone_byGroupId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.ZoneSelect_SelectedIndexChanged | — | CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.AreaSelect_SelectedIndexChanged | — | CommonDataLoad.GetTerritory_ByAreaId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| txtEmpCode | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| TeritorySelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| id_mastetID | EmployeeInformation.EmpInfoId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpInfoId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | EmployeeInformation.EmpInfoId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @id | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpCode | EmployeeInformation.EmpMasterCode | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpMasterCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpCode | EmployeeInformation.EmpMasterCode | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Name | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpName | EmployeeInformation.EmpName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpFatherName | EmployeeInformation.FatherName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @FatherName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpMotherName | EmployeeInformation.MotherName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @MotherName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpdobDate | EmployeeInformation.DateOfBirth | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DateOfBirth | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpAddress | EmployeeInformation.AddressPresent | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @AddressPresent | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpPresentAddress | EmployeeInformation.AddressPermanent | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @AddressPermanent | UNKNOWN (SP body not in repo) | UNKNOWN |
| GenderSelect | EmployeeInformation.Gender | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Gender | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReligionSelect | EmployeeInformation.Religion | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Religion | UNKNOWN (SP body not in repo) | UNKNOWN |
| Nationality | EmployeeInformation.Nationality | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Nationality | UNKNOWN (SP body not in repo) | UNKNOWN |
| BloodGroupSelect | EmployeeInformation.BloodGroup | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @BloodGroup | UNKNOWN (SP body not in repo) | UNKNOWN |
| MaritalStatusSelect | EmployeeInformation.MaritalStatus | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @MaritalStatus | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtNIDNO | EmployeeInformation.NationalIdNo | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @NationalIdNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtDateofjoin | EmployeeInformation.JoiningDate | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @JoiningDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtJobLeftDate | EmployeeInformation.JobLeftDate | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @JobLeftDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtProbationEndDate | EmployeeInformation.ProbitionEndDate | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @ProbitionEndDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLastCompanyName | EmployeeInformation.LastCompanyName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @LastCompanyName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLastJobLocation | EmployeeInformation.LastJobLocation | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @LastJobLocation | UNKNOWN (SP body not in repo) | UNKNOWN |
| DepartmentSelect | EmployeeInformation.DepartmentId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DepartmentId | UNKNOWN (SP body not in repo) | UNKNOWN |
| DesignationSelect | EmployeeInformation.DesignationId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DesignationId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ShiftSelect | EmployeeInformation.ShiftId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @ShiftId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmail | EmployeeInformation.Email | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Email | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpContactNo | EmployeeInformation.CellNumber | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @CellNumber | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReferencePersonName | EmployeeInformation.RefName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @RefName | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReferenceContactNo | EmployeeInformation.RefContactNo | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @RefContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmergencyContactNo | EmployeeInformation.EmrgContactNo | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmrgContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmrgContactNoRelaton | EmployeeInformation.EmrgContactNoRelaton | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmrgContactNoRelaton | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsProbation | EmployeeInformation.IsProbition | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @IsProbition | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsTempEmployeeCode | EmployeeInformation.IsTempEmployeeCode | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @IsTempEmployeeCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfUserId | UserDAO.UserId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @UserId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpName | UserDAO.UserName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @UserName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtPassword | UserDAO.Password | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @Password | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLoginName | UserDAO.LoginName | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @LoginName | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkMobileAccess | UserDAO.IsAppsUser | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @IsAppsUser | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtImei1 | UserDAO.IMEI_One | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @IMEI_One | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtImei2 | UserDAO.IMEI_Two | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @IMEI_Two | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlUserRole | UserDAO.UserRoleID | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @UserRoleID | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtacDate | UserDAO.ActiveInActiveDate | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @ActiveInActiveDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfMIOId | MIOInfo.MIOId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @MIOId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TeritorySelect | MIOInfo.TerritoryId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TeritorySelect | MIOInfo.TerritoryId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | ASMInfo.IsActive | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @isActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | ASMInfo.IsActive | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @isActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtacDate | ASMInfo.ActiveDate | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @acInAcDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtacDate | ASMInfo.ActiveDate | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @acInAcDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfAMID | ASMInfo.ASMId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @ASMId | UNKNOWN (SP body not in repo) | UNKNOWN |
| AreaSelect | ASMInfo.AreaId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| AreaSelect | ASMInfo.AreaId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfRSMId | RSMInfo.RSMId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_RSMInfo | @RSMId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ZoneSelect | ASMInfo.RegionId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ZoneSelect | ASMInfo.RegionId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_RSMInfo | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfNSMId | NSMInfo.NSMId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_NSMInfo | @NSMId | UNKNOWN (SP body not in repo) | UNKNOWN |
| GroupSelect | ASMInfo.GroupId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| GroupSelect | ASMInfo.GroupId | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_NSMInfo | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlUserRole | dt | MasterSetup_UI_EmployeeSetupForNewJoiner.ddlUserRole_SelectedIndexChanged | — | SeedDataDAL.GetUserRoleTypeListById | UNKNOWN | UNKNOWN | UNKNOWN |
| txtEmpCode | Text | MasterSetup_UI_EmployeeSetupForNewJoiner.txtEmpCode_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| GenderSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ReligionSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| Nationality | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| BloodGroupSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| MaritalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| DepartmentSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| DesignationSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ShiftSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlMonthlyAllawance | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlUserRole | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbDashboard | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| GroupSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ZoneSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| AreaSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| TeritorySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlDistributionCenter | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| loadUserRole | LinkButton click | (postback) | MasterSetup_UI_EmployeeSetupForNewJoiner.loadUserRole_Click | Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs:1187 | — | SeedDataDAL.GetUserRoleList | sp_Get_UserRoleInfo | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs:945 | — | EmployeeInformationDaL.SaveEmployeeInformation, EmployeeInformationDaL.SyncEmployeeProductLineDeclar | sp_Save_ASMInfo, sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Save_MIOInfo, sp_Save_NSMInfo, sp_Save | writes tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | MasterSetup_UI_EmployeeSetupForNewJoiner.btnSave_Click | Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs:945 | — | EmployeeInformationDaL.SaveEmployeeInformation, EmployeeInformationDaL.SyncEmployeeProductLineDeclar | sp_Save_ASMInfo, sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Save_MIOInfo, sp_Save_NSMInfo, sp_Save | writes tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare |
| btnReset | LinkButton click | (postback) | MasterSetup_UI_EmployeeSetupForNewJoiner.btnReset_Click | Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs:1182 | — | — | — | read-only / none detected |

## 11. Frontend Flow
Identical to PAGE-613 (see that spec section 11) except: `Page_Load` sets `txtacDate.Text = DateTime.Now.ToString("dd MMMM, yyyy")` on first load (`EmployeeSetupForNewJoiner.aspx.cs:30`); `ShiftSelect` is a single-select `DropDownList` with first item `Please Select From List` (`:502-503`); no depot / product-line controls; success redirects to `EmployeeSetupForNewJoiner.aspx` (`:1167`). Confirm dialog `sweetAlertConfirm_Submit`.

## 12. Backend Flow
Same chain as PAGE-613 with these differences (all in `btnSave_Click` `:944-1180`):
1. `aMaster.ShiftId = ShiftSelect.SelectedIndex > 0 ? int.Parse(...) : null`; `aMaster.CompanyUnitId` not set.
2. `_DAL.SaveEmployeeInformation(aMaster, MonthlyArray, Session["UserId"], 0)` -> `shiftIds`/`productLineIds` are `null`, so inside the DAL the shift list falls back to `new[]{employee.ShiftId}` (single row in `tblEmpwiseShiftDeclare`) and the **product-line list becomes empty -> any existing rows for the employee are deleted** (`EmployeeInformationDaL.cs:100-110`). For a brand-new employee nothing exists so this is harmless; the edit path of this page is not reachable with a Save button (see BR-M15-013).
3. User + hierarchy saved exactly as in PAGE-613 (`SaveUserInfo`, `Save_MIOInfo/ASMInfo/RSMInfo/NSMInfo`), results ignored, non-atomic.
4. `UserRoleID = ddlUserRole.SelectedIndex > 0 ? int.Parse(...) : null` (`:1051`).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| EmployeeInformationDaL.GetEmployeeInfoForEdit | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:619 | sp_Get_EmployeeInformation_ById | @id |
| UserInfoDAL.GetUserSetupByEmpId | Library.DAL/DoctorModule_DAL/UserInfoDAL.cs:218 | sp_GET_UserMaster_ByEmpId | @id |
| RSMSetupDal.GetMIOetupEditDataByEmpId | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:677 | sp_GET_MIOInfo_ByEmpId | @id |
| RSMSetupDal.GetAMSetupEditDataByEmpId | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:699 | sp_GET_ASMInfo_ByEmpId | @id |
| RSMSetupDal.GetDZSMSetupEditDataByEmpId | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:721 | sp_GET_DZSMInfo_ByEmpId | @id |
| RSMSetupDal.GetNSMSetupEditDataByEmpId | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:743 | sp_GET_NSMInfo_ByEMPId | @id |
| CommonDataLoad.GetTerritory_ByAreaId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1540 | sp_CS_GetTerritory_ByAreaId_Active | @id |
| CommonDataLoad.GetZone_byGroupId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1213 | sp_Get_Zone_All_Active | @GroupId |
| CommonDataLoad.GetArea_ByZoneId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1131 | sp_CS_GetArea_ByZoneId_Active | @id |
| SeedDataDAL.GetUserRoleTypeListById | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:725 | sp_GET_UserRoleInfoRoleType_ById | @id |
| SeedDataDAL.GetDistributionCenterDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:669 | sp_GET_DistributionCenter |  |
| CommonDataLoad.GetGroupInfo_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:374 | sp_CS_Group_All |  |
| SeedDataDAL.GetUserRoleList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:707 | sp_Get_UserRoleInfo |  |
| SeedDataDAL.GetlMonthlyAllawanceList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:689 | sp_GET_MonthlyAllowanceList |  |
| CommonDataLoad.GetDepartment_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:559 | sp_CS_Department_Active |  |
| CommonDataLoad.GetDesignation_Active_Emp | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:579 | sp_CS_Designation_Active |  |
| CommonDataLoad.GetShift_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:598 | sp_CS_Shift_Active |  |
| UserInfoDAL.check_UserInfoByEmpCode | Library.DAL/DoctorModule_DAL/UserInfoDAL.cs:288 | sp_check_Vali_EmployeeInfoEntry | @MasterId, @PageName |
| EmployeeInformationDaL.SaveEmployeeInformation | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:19 | sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Update_Employee | @AddressPermanent, @AddressPresent, @AllowanceId, @BloodGroup, @CellNumber, @CompanyId, @CompanyUnit |
| EmployeeInformationDaL.SyncEmployeeProductLineDeclareInternal | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:252 | inline SQL: tblEmpwiseProductLineDeclare, tblEmpwiseProductLineDeclare | @EmpInfoId, @ProductLineID |
| EmployeeInformationDaL.SyncEmployeeShiftDeclareInternal | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:212 | inline SQL: tblEmpwiseShiftDeclare, tblEmpwiseShiftDeclare | @EmpInfoId, @ShiftId |
| UserInfoDAL.SaveUserInfo | Library.DAL/DoctorModule_DAL/UserInfoDAL.cs:331 | sp_Save_UserCompanyUnit, sp_Save_UserMarketDetail, sp_Save_UserMaster_New, sp_Up | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsA |
| RSMSetupDal.Save_MIOInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1107 | sp_Save_MIOInfo, sp_UD_MIOInfo, sp_check_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Save_ASMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:22 | sp_Save_ASMInfo, sp_UD_ASMInfo, sp_check_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Save_RSMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1035 | sp_Save_RSMInfo, sp_UD_RSMInfo, sp_check_RSMInfo | @CompanyId, @EmployeeId, @RSMId, @RegionId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Save_NSMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:347 | sp_Save_NSMInfo, sp_UD_NSMInfo, sp_check_NSMInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_Department_Active | CommonDataLoad.GetDepartment_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Designation_Active | CommonDataLoad.GetDesignation_Active_Emp | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_Active | CommonDataLoad.GetTerritory_ByAreaId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Group_All | CommonDataLoad.GetGroupInfo_All | body NOT in repository | write-like name |
| sp_CS_Shift_Active | CommonDataLoad.GetShift_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ASMInfo_ByEmpId | RSMSetupDal.GetAMSetupEditDataByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DZSMInfo_ByEmpId | RSMSetupDal.GetDZSMSetupEditDataByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DistributionCenter | SeedDataDAL.GetDistributionCenterDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MIOInfo_ByEmpId | RSMSetupDal.GetMIOetupEditDataByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MonthlyAllowanceList | SeedDataDAL.GetlMonthlyAllawanceList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_NSMInfo_ByEMPId | RSMSetupDal.GetNSMSetupEditDataByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_GET_UserMaster_ByEmpId | UserInfoDAL.GetUserSetupByEmpId | body NOT in repository | read-like name (INFERRED) |
| sp_GET_UserRoleInfoRoleType_ById | SeedDataDAL.GetUserRoleTypeListById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeInformation_ById | EmployeeInformationDaL.GetEmployeeInfoForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | SeedDataDAL.GetUserRoleList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active | CommonDataLoad.GetZone_byGroupId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Save_ASMInfo | RSMSetupDal.Save_ASMInfo | body NOT in repository | write-like name |
| sp_Save_EmployeeAllowanceDetail | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | write-like name |
| sp_Save_EmployeeInformation | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | write-like name |
| sp_Save_MIOInfo | RSMSetupDal.Save_MIOInfo | body NOT in repository | write-like name |
| sp_Save_NSMInfo | RSMSetupDal.Save_NSMInfo | body NOT in repository | write-like name |
| sp_Save_RSMInfo | RSMSetupDal.Save_RSMInfo | body NOT in repository | write-like name |
| sp_Save_UserCompanyUnit | UserInfoDAL.SaveUserInfo | body NOT in repository | write-like name |
| sp_Save_UserMarketDetail | UserInfoDAL.SaveUserInfo | body NOT in repository | write-like name |
| sp_Save_UserMaster_New | UserInfoDAL.SaveUserInfo | body NOT in repository | write-like name |
| sp_UD_ASMInfo | RSMSetupDal.Save_ASMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_MIOInfo | RSMSetupDal.Save_MIOInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_NSMInfo | RSMSetupDal.Save_NSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_RSMInfo | RSMSetupDal.Save_RSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Update_EmployeeInformation | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | write-like name |
| sp_Update_UserMaster_new | UserInfoDAL.SaveUserInfo | body NOT in repository | write-like name |
| sp_check_ASMInfo | RSMSetupDal.Save_ASMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_EmployeeInfo | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | read-like name (INFERRED) |
| sp_check_MIOInfo | RSMSetupDal.Save_MIOInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_NSMInfo | RSMSetupDal.Save_NSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_RSMInfo | RSMSetupDal.Save_RSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_UserInfo_Save | UserInfoDAL.SaveUserInfo | body NOT in repository | write-like name |
| sp_check_Vali_EmployeeInfoEntry | UserInfoDAL.check_UserInfoByEmpCode | body NOT in repository | read-like name (INFERRED) |
| sp_check_da_UserInfo | UserInfoDAL.SaveUserInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_da_UserInfo_Save | UserInfoDAL.SaveUserInfo | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare
- Tables WRITTEN (inline SQL reachable): tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblEmpwiseProductLineDeclare | EmpInfoId | @EmpInfoId |
| tblEmpwiseProductLineDeclare | ProductLineID | @ProductLineID |
| tblEmpwiseShiftDeclare | EmpInfoId | @EmpInfoId |
| tblEmpwiseShiftDeclare | ShiftId | @ShiftId |

## 18. Validation Matrix
Same rules/messages as PAGE-613 (BR-M15-001..008) with two differences: (a) shift is required as `ShiftSelect.SelectedIndex == 0` -> tooltip (`:647`); (b) the user-role required check is **commented out** (`:751-757`). Everything else - NID 17, phones 11, code uniqueness (`check_UserInfoByEmpCode(...,"UserEmpCode")`), login/password/active-date required, vacancy checks per role type, DIC depot required - is identical (`:600-943`).

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-015 | Exactly one shift must be chosen for a new joiner; it is stored as legacy `ShiftId` and mirrored as one row in `tblEmpwiseShiftDeclare`. | `EmployeeSetupForNewJoiner.aspx.cs:647,985`; `EmployeeInformationDaL.cs:100-106` | code-behind + DAL | CONFIRMED |
| BR-M15-016 | User role is optional on this page: without a role no hierarchy row is written (`hfRoleType` is empty) and `UserRoleID` is sent as NULL, yet the login/password are still required. | `EmployeeSetupForNewJoiner.aspx.cs:751-757,1051` | code-behind | CONFIRMED |
| BR-M15-017 | Active date defaults to today (`dd MMMM, yyyy`) so new joiners are activated from the entry date unless changed. | `EmployeeSetupForNewJoiner.aspx.cs:30` | code-behind | CONFIRMED |
| BR-M15-018 | Product-line and depot are not captured for new joiners; on save the DAL replaces the employee's product-line set with an empty set. | `EmployeeSetupForNewJoiner.aspx.cs:1023`; `EmployeeInformationDaL.cs:107-108` | DAL | CONFIRMED |
| (shared) | BR-M15-001..014 apply unchanged (name/code/join date required, NID/phone lengths, code uniqueness, code -> login+password default, user creation with `EmpInfoId` link, role-type driven hierarchy, vacancy checks, DIC depots, single Active flag, non-atomic saves, page read-only with `?MID`). Source lines are the same logic at `:596-1170`. | `EmployeeSetupForNewJoiner.aspx.cs` | code-behind | CONFIRMED |

## 20. Permission Matrix
Same as PAGE-613: only `Session["UserId"]` used; no role/permission check; no `<authorization>` in `Web.config`. Menu-level access only (menu rows UNKNOWN).

## 21. Status Lifecycle
Same columns as PAGE-613 (`EmployeeStatus`, `UserStatus`, hierarchy `IsActive/ActiveDate`); creation only.

## 22. Error Handling
As PAGE-613: field validation via `is-invalid`/`showpop6`, single generic `Already Exist!` on failure (`:1174`), DAL exceptions unhandled at page level, read paths swallow exceptions.

## 23. Audit
`Session["UserId"]` stamped as `@EntryBy` (employee, user) and `@entryBy` (hierarchy). No history table referenced.

## 24. Integration
None external; same downstream consumers as PAGE-613.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, EmployeeInformationDaL, RSMSetupDal, SeedDataDAL, UserInfoDAL; DAO: ASMInfo, EmployeeInformation, MIOInfo, NSMInfo, RSMInfo, ResultInfo, UserDAO; Tables (inline): tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-615-01 | Enter all required fields, one shift, no role | Employee + user (role NULL) created; page reloads blank | BR-M15-015, 016 |
| TC-615-02 | Shift left at "Please Select From List" | Shift box flagged, no write | BR-M15-015 |
| TC-615-03 | Role MIO + territory chosen | MIO row created with `EmployeeId=<new PK>` | BR-M15-006 |
| TC-615-04 | Open page | Active date shows today | BR-M15-017 |
| TC-615-05 | Same employee code as existing | `Employee Code Already Exist in User!` | BR-M15-003 |
| TC-615-06 | Save then verify product lines (edge) | Product-line set empty | BR-M15-018 |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx`, `Solution.Web/MasterSetup_UI/EmployeeSetupForNewJoiner.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | EmployeeSetupForNewJoiner.aspx(.cs) | n/a | MISSING | - |
| Relationship to PAGE-613 | None existed | Older copy; diverged (multi-shift/depot/product line absent) | n/a | DUPLICATE/LEGACY | GAP-M15-006 |
| Atomicity / result handling | None existed | same defects as PAGE-613 | UNKNOWN | ACTUAL-DEFECT | GAP-M15-002 |

## 30. Known Gaps
- **GAP-M15-006** Two near-identical onboarding pages (PAGE-613, PAGE-615) with different capabilities; risk of employees created without depot/product lines/multi-shift and with NULL role.
- Inherits GAP-M15-001..005 from PAGE-613.
- Success redirect points to itself, not to a list; no confirmation of what was created.

## 31. Change Impact
Shares `EmployeeInformationDaL`, `UserInfoDAL`, `RSMSetupDal`, `sp_check_Vali_EmployeeInfoEntry`, `sp_Save_EmployeeInformation` with PAGE-613/614. Changing the DAL fallback (`shiftIds ?? ShiftId`) affects this page only.

