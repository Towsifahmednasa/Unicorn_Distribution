---
page_id: PAGE-613
module: M15
page: EmployeeSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-613 — EmployeeSetup

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-613 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmployeeSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_EmployeeSetup |
| aspx size | 49282 bytes |
| Code-behind LOC | 1380 |
| Inline script size | 1891 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED (heading text `Login Access`, field labels, save chain): single-screen **new-employee onboarding** that creates, in one click, (a) the employee master record, (b) the employee's shifts, product lines and monthly allowances, (c) the employee's **web/mobile login account** and (d) - when the chosen user role is a field-force role - the employee's **market-structure assignment** (MIO -> territory, AM -> area, DZSM -> zone/region, NSM -> group) or, for role type `DIC`, the depots the user may operate. With `?MID=<EmpInfoId>` the same page is a **read-only viewer** of an existing employee (no Save button is made visible - see BR-M15-013); editing of the personal/job data is done on `EmployeeSetupEdit.aspx` (PAGE-614). Entry point: `EmployeeRecords.aspx` "New Entry" button (`Solution.Web/MasterSetup_UI/EmployeeRecords.aspx:15`), eye icon -> `EmployeeSetup.aspx?MID=` (`EmployeeRecords.aspx:386`).

## 3. Business Objective
CONFIRMED by code: let HR/admin onboard a salesperson or office employee once and have every downstream module work immediately - the employee gets a `UserMaster` row linked by `EmpInfoId`, a role (`UserRoleID`) that fixes menu access and sales-hierarchy role type, and (for MIO/AM/DZSM/NSM) a territory/area/zone/group so that orders, tour plans, attendance and approvals can be scoped to the employee's market. Login name and password default to the employee code (`txtEmpCode_TextChanged`, `EmployeeSetup.aspx.cs:1374-1378`), i.e. the objective includes fast bulk onboarding with predictable credentials (security risk - see GAP-M15-004).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/EmployeeRecords.aspx`, `MasterSetup_UI/UserSetupforEMP.aspx`

Outbound references: `employeerecords.aspx`, `userroleentry.aspx`

## 5. Route
`~/MasterSetup_UI/EmployeeSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs |
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
| ShiftSelect | ListBox |  | no validator found |  |  |  |  |
| ReferencePersonName | TextBox | Enter reference Person Name | no validator found |  |  |  |  |
| ReferenceContactNo | TextBox | Enter reference contact no | no validator found |  |  |  |  |
| FilteredTextBoxExtender2 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtLastCompanyName | TextBox | Last Company Name  | no validator found |  |  |  |  |
| txtLastJobLocation | TextBox | Last Company Name  | no validator found |  |  |  |  |
| txtJobLeftDate | TextBox | Select date of birth | no validator found |  |  |  |  |
| ddlMonthlyAllawance | ListBox |  | no validator found |  |  |  |  |
| ProductLineSelect | ListBox |  | no validator found |  |  |  |  |
| chkIsProbation | input |  | no validator found |  |  |  |  |
| txtProbationEndDate | TextBox | Select date of birth | no validator found |  |  |  |  |
| chkIsActive | input |  | no validator found |  |  |  | onchange→IsActiveChange() |
| ddlDepotSelect | DropDownList |  | no validator found |  |  | see §9 |  |
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

**Verified control notes (code-behind / markup, Tier A):**
| Control | Label | Required (server) | Default | Visible / enabled | Source / error |
|---|---|---|---|---|---|
| txtEmpName | Employee name | yes | blank | always | `EmployeeSetup.aspx.cs:652` |
| txtEmpCode | Employee code | yes; unique | blank | always; AutoPostBack (fills login/password) | `:661,750,1374` |
| chkIsTempEmployeeCode | Is temp employee code | no | unchecked | always | `:1039` |
| txtEmpdobDate / txtJobLeftDate / txtProbationEndDate / txtDateofjoin | dates (pickadate `dd MMMM, yyyy`) | only join date | blank | always | parsed with `DateTime.Parse(...).Date` `:998-1011` |
| GenderSelect / ReligionSelect / Nationality / BloodGroupSelect / MaritalStatusSelect | static lists (Male/Female; Islam/Hindu/Christian; Bangladeshi/Indian/Others; A+..AB-; Married/Unmarried) | no | index 0 | always | saved as text if index>0 `:1001-1005` |
| DepartmentSelect / DesignationSelect | active departments / employee-designations | no | index 0 | always | `sp_CS_Department_Active`, `sp_CS_Designation_Active` |
| ShiftSelect (ListBox, multi) | shifts | >=1 | none | always | `sp_CS_Shift_Active` (text field `ShiftText`) |
| ProductLineSelect (ListBox, multi) | product lines | no | none | always | `sp_GET_ProductLine` |
| ddlMonthlyAllawance (ListBox, multi) | monthly allowances | no | none | always | `sp_GET_MonthlyAllowanceList` |
| ddlDepotSelect | Depot of employee (`CompanyUnitId`) | no | "Select Depot" | always | `sp_GET_DistributionCenter` |
| chkIsActive | Active | - | checked | always | `EmployeeSetup.aspx:416` |
| txtLoginName / txtPassword | login | yes | filled from employee code | always (plain text box) | `:769,777` |
| ddlUserRole | user role | yes (613) | blank | always | `sp_Get_UserRoleInfo` |
| chkMobileAccess + txtImei1/txtImei2 | mobile access | IMEI not required (rule commented out) | unchecked | IMEI panel `divMei` visible only when checked | `:794-811,1364` |
| rbDashboard | Main / Depot dashboard | - | Main | always | markup `:572-574` |
| GroupSelect/ZoneSelect/AreaSelect/TeritorySelect | market node | depends on role type | index 0 | panels `divGroup/divZone/divArea/divTerritory` shown per role type | `:1288-1338` |
| ddlDistributionCenter (multi) | depots for DIC user | DIC only | none | panel `dcDiv` only for `DIC` | `:1352-1357` |
| txtacDate | active/inactive date | yes | blank | always | `:813` |
| btnSave | Save | - | - | visible only when no `?MID`; confirm `sweetAlertConfirm_Submit` | `:43` |
| btnUpdate / btnReset | - | - | - | never visible / empty handler | `:36,1263` |

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| GroupSelect | dt | MasterSetup_UI_EmployeeSetup.GroupSelect_SelectedIndexChanged | — | CommonDataLoad.GetZone_byGroupId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | MasterSetup_UI_EmployeeSetup.ZoneSelect_SelectedIndexChanged | — | CommonDataLoad.GetArea_ByZoneId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | MasterSetup_UI_EmployeeSetup.AreaSelect_SelectedIndexChanged | — | CommonDataLoad.GetTerritory_ByAreaId_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| txtEmpCode | dt | MasterSetup_UI_EmployeeSetup.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| TeritorySelect | dt | MasterSetup_UI_EmployeeSetup.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| AreaSelect | dt | MasterSetup_UI_EmployeeSetup.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| ZoneSelect | dt | MasterSetup_UI_EmployeeSetup.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| GroupSelect | dt | MasterSetup_UI_EmployeeSetup.Validation | — | UserInfoDAL.check_UserInfoByEmpCode | UNKNOWN | UNKNOWN | UNKNOWN |
| id_mastetID | EmployeeInformation.EmpInfoId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpInfoId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | EmployeeInformation.EmpInfoId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @id | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpCode | EmployeeInformation.EmpMasterCode | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpMasterCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpCode | EmployeeInformation.EmpMasterCode | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Name | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpName | EmployeeInformation.EmpName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpFatherName | EmployeeInformation.FatherName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @FatherName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpMotherName | EmployeeInformation.MotherName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @MotherName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpdobDate | EmployeeInformation.DateOfBirth | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DateOfBirth | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpAddress | EmployeeInformation.AddressPresent | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @AddressPresent | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpPresentAddress | EmployeeInformation.AddressPermanent | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @AddressPermanent | UNKNOWN (SP body not in repo) | UNKNOWN |
| GenderSelect | EmployeeInformation.Gender | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Gender | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReligionSelect | EmployeeInformation.Religion | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Religion | UNKNOWN (SP body not in repo) | UNKNOWN |
| Nationality | EmployeeInformation.Nationality | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Nationality | UNKNOWN (SP body not in repo) | UNKNOWN |
| BloodGroupSelect | EmployeeInformation.BloodGroup | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @BloodGroup | UNKNOWN (SP body not in repo) | UNKNOWN |
| MaritalStatusSelect | EmployeeInformation.MaritalStatus | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @MaritalStatus | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtNIDNO | EmployeeInformation.NationalIdNo | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @NationalIdNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtDateofjoin | EmployeeInformation.JoiningDate | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @JoiningDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtJobLeftDate | EmployeeInformation.JobLeftDate | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @JobLeftDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtProbationEndDate | EmployeeInformation.ProbitionEndDate | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @ProbitionEndDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLastCompanyName | EmployeeInformation.LastCompanyName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @LastCompanyName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLastJobLocation | EmployeeInformation.LastJobLocation | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @LastJobLocation | UNKNOWN (SP body not in repo) | UNKNOWN |
| DepartmentSelect | EmployeeInformation.DepartmentId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DepartmentId | UNKNOWN (SP body not in repo) | UNKNOWN |
| DesignationSelect | EmployeeInformation.DesignationId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DesignationId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlDepotSelect | EmployeeInformation.CompanyUnitId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @CompanyUnitId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmail | EmployeeInformation.Email | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Email | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpContactNo | EmployeeInformation.CellNumber | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @CellNumber | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReferencePersonName | EmployeeInformation.RefName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @RefName | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReferenceContactNo | EmployeeInformation.RefContactNo | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @RefContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmergencyContactNo | EmployeeInformation.EmrgContactNo | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmrgContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmrgContactNoRelaton | EmployeeInformation.EmrgContactNoRelaton | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmrgContactNoRelaton | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsProbation | EmployeeInformation.IsProbition | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @IsProbition | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsTempEmployeeCode | EmployeeInformation.IsTempEmployeeCode | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @IsTempEmployeeCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfUserId | UserDAO.UserId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @UserId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpName | UserDAO.UserName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @UserName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtPassword | UserDAO.Password | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @Password | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLoginName | UserDAO.LoginName | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @LoginName | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkMobileAccess | UserDAO.IsAppsUser | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @IsAppsUser | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtImei1 | UserDAO.IMEI_One | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @IMEI_One | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtImei2 | UserDAO.IMEI_Two | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @IMEI_Two | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlUserRole | UserDAO.UserRoleID | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @UserRoleID | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtacDate | UserDAO.ActiveInActiveDate | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @ActiveInActiveDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfMIOId | MIOInfo.MIOId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @MIOId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TeritorySelect | MIOInfo.TerritoryId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TeritorySelect | MIOInfo.TerritoryId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | ASMInfo.IsActive | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @isActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | ASMInfo.IsActive | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @isActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtacDate | ASMInfo.ActiveDate | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_MIOInfo | @acInAcDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtacDate | ASMInfo.ActiveDate | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @acInAcDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfAMID | ASMInfo.ASMId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @ASMId | UNKNOWN (SP body not in repo) | UNKNOWN |
| AreaSelect | ASMInfo.AreaId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| AreaSelect | ASMInfo.AreaId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_ASMInfo | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfRSMId | RSMInfo.RSMId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_RSMInfo | @RSMId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ZoneSelect | ASMInfo.RegionId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ZoneSelect | ASMInfo.RegionId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_RSMInfo | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfNSMId | NSMInfo.NSMId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_NSMInfo | @NSMId | UNKNOWN (SP body not in repo) | UNKNOWN |
| GroupSelect | ASMInfo.GroupId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | UserInfoDAL.SaveUserInfo | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| GroupSelect | ASMInfo.GroupId | MasterSetup_UI_EmployeeSetup.btnSave_Click | (none — page calls DAL directly) | RSMSetupDal.Save_NSMInfo | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlUserRole | dt | MasterSetup_UI_EmployeeSetup.ddlUserRole_SelectedIndexChanged | — | SeedDataDAL.GetUserRoleTypeListById | UNKNOWN | UNKNOWN | UNKNOWN |
| txtEmpCode | Text | MasterSetup_UI_EmployeeSetup.txtEmpCode_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

**Verified employee mapping (UI -> `EmployeeInformation` -> `sp_Save_EmployeeInformation`/`sp_Update_EmployeeInformation` parameter; table/column names UNKNOWN because SP body is not in the repo):**
| UI control | DTO property | SP parameter | Read-back column in `sp_Get_EmployeeInformation_ById` |
|---|---|---|---|
| txtEmpName | EmpName | @EmpName | EmpName |
| txtEmpCode | EmpMasterCode | @EmpMasterCode | EmpMasterCode |
| txtEmpFatherName / MotherName | FatherName / MotherName | @FatherName / @MotherName | FatherName / MotherName |
| txtEmpdobDate | DateOfBirth | @DateOfBirth | DateOfBirth |
| txtEmpAddress / txtEmpPresentAddress | AddressPresent / AddressPermanent (**note: the box labelled "permanent address" is `txtEmpPresentAddress`**) | @AddressPresent / @AddressPermanent | AddressPresent / AddressPermanent |
| Gender/Religion/Nationality/BloodGroup/Marital | text | @Gender @Religion @Nationality @BloodGroup @MaritalStatus | same names |
| txtNIDNO | NationalIdNo | @NationalIdNo | NationalIdNo |
| txtDateofjoin / JobLeftDate / ProbationEndDate | JoiningDate / JobLeftDate / ProbitionEndDate | @JoiningDate @JobLeftDate @ProbitionEndDate | JoiningDate JobLeftDate ProbitionEndDate |
| txtLastCompanyName / LastJobLocation | LastCompanyName / LastJobLocation | same | same |
| DepartmentSelect / DesignationSelect | DepartmentId / DesignationId | @DepartmentId @DesignationId | DepartmentId DesignationId |
| ddlDepotSelect | CompanyUnitId | @CompanyUnitId | CompanyUnitId |
| (none) | ShiftId = null | @ShiftId | (single shift - legacy; multi via `tblEmpwiseShiftDeclare`) |
| txtEmail / txtEmpContactNo | Email / CellNumber | @Email @CellNumber | Email CellNumber |
| ReferencePersonName / ReferenceContactNo | RefName / RefContactNo | @RefName @RefContactNo | RefName RefContactNo |
| txtEmergencyContactNo / txtEmrgContactNoRelaton | EmrgContactNo / EmrgContactNoRelaton | same | same |
| chkIsProbation / chkIsTempEmployeeCode | IsProbition / IsTempEmployeeCode | @IsProbition @IsTempEmployeeCode | same |
| chkIsActive | EmployeeStatus ('Active'/'Inactive') | @EmployeeStatus | EmployeeStatus |
| (constant 1) | CompanyId | @CompanyId | - |
| ddlMonthlyAllawance | CSV -> per item | `sp_Save_EmployeeAllowanceDetail(@EmpInfoId,@AllowanceId)` | AllowanceId (CSV) |
User mapping (`UserDAO` -> `sp_Save_UserMaster_New` / `sp_Update_UserMaster_new`): UserId(hfUserId), UserType='Employee', UserTypeId=3, EmpInfoId, UserName=EmpName, LoginName, Password, IsAppsUser(chkMobileAccess), IMEI_One/Two, UserRoleID, UserStatus, ActiveInActiveDate, IsMainDashboard, IsDepotDashboard, @EntryBy. Depots: `sp_Save_UserCompanyUnit(@UserId,@ComUnitId)`.

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
| ProductLineSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlDepotSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
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
| loadUserRole | LinkButton click | (postback) | MasterSetup_UI_EmployeeSetup.loadUserRole_Click | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs:1269 | — | SeedDataDAL.GetUserRoleList | sp_Get_UserRoleInfo | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | MasterSetup_UI_EmployeeSetup.btnSave_Click | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs:979 | — | EmployeeInformationDaL.SaveEmployeeInformation, EmployeeInformationDaL.SyncEmployeeProductLineDeclar | sp_Save_ASMInfo, sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Save_MIOInfo, sp_Save_NSMInfo, sp_Save | writes tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | MasterSetup_UI_EmployeeSetup.btnSave_Click | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs:979 | — | EmployeeInformationDaL.SaveEmployeeInformation, EmployeeInformationDaL.SyncEmployeeProductLineDeclar | sp_Save_ASMInfo, sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Save_MIOInfo, sp_Save_NSMInfo, sp_Save | writes tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare |
| btnReset | LinkButton click | (postback) | MasterSetup_UI_EmployeeSetup.btnReset_Click | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs:1264 | — | — | — | read-only / none detected |

**Verified:** `btnSave` -> `btnSave_Click` (`EmployeeSetup.aspx.cs:978`) writes to: employee master (`sp_Save_EmployeeInformation`), `tblEmpwiseShiftDeclare`, `tblEmpwiseProductLineDeclare`, employee-allowance detail (`sp_Save_EmployeeAllowanceDetail`), user master (`sp_Save_UserMaster_New`), user-depot (`sp_Save_UserCompanyUnit`), and one of MIO/ASM/RSM/NSM info tables. `btnUpdate` and `btnReset` are inert (never visible / empty handler). `loadUserRole` only re-binds the role list.

## 11. Frontend Flow
1. **Init** (`Page_Load` `EmployeeSetup.aspx.cs:26-48`, first GET only): `LoadInitialInfo()` fills depots (twice: multi-select `ddlDistributionCenter` for DIC users and single `ddlDepotSelect`), groups, user roles, monthly allowances, product lines, departments, designations (employee-eligible), active shifts. If `?MID=` present -> `GetOneRecord`; else `btnSave.Visible = true`.
2. **Cascades (server postbacks, UpdatePanel)**: Group -> `GroupSelect_SelectedIndexChanged` (loads active zones; clears area+territory); Zone -> area; Area -> territory (`:553-626`).
3. **User role change** `ddlUserRole_SelectedIndexChanged` (`:1288-1361`): reads `RoleType` via `sp_GET_UserRoleInfoRoleType_ById`, stores in `hfRoleType`, shows group/zone/area/territory panels by role type (MIO: all four; AM: group/zone/area; DZSM: group/zone; NSM: group), shows the "DIC depots" panel (`dcDiv`) only for `DIC`; resets `ddlDistributionCenter` selection.
4. **Mobile access** checkbox toggles the IMEI panel (`chkMobileAccess_CheckedChanged` `:1364`).
5. **Employee code typed** (`OnTextChanged`) copies the code into login name and password.
6. **Client JS** (`EmployeeSetup.aspx:139-160`): only `pageLoad()` re-initialising `pickadate` date pickers and `select2`; `IsActiveChange()` toggles the label of the active date. No client-side validation - all validation is server-side (`Validation()`), tooltips/`is-invalid` CSS classes are set from code-behind.
7. **Confirm dialogs**: Save link uses `OnClientClick="return sweetAlertConfirm_Submit(this);"`; success shows `successalert(...,'EmployeeRecords.aspx')`, failure `faildalert('Already Exist!')`.
8. Numeric filters: `FilteredTextBoxExtender` on NID/phones; `MaxLength` 17 (NID) and 11 (phones) in markup (`EmployeeSetup.aspx:96,235,251,337`).

## 12. Backend Flow
`btnSave_Click` (`EmployeeSetup.aspx.cs:978-1223`) after `Validation()` passes:
1. Build `EmployeeInformation` (`CompanyId` hard-coded `1`; `ShiftId` null because shifts are multi-select; gender/religion/nationality/blood group/marital status stored as the **display text** of the dropdown, only when index > 0). Monthly allowance ids are joined into a CSV.
2. **Employee**: `EmployeeInformationDaL.SaveEmployeeInformation(aMaster, MonthlyArray, Session["UserId"], 0, shiftIds, productLineIds)` (`Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:18`): `EmpInfoId==0` -> `SaveDataReturnPrimaryKey("sp_Save_EmployeeInformation")` with `@EntryBy`; `EmpInfoId>0` -> `sp_check_EmployeeInfo(@id,@Name=code)`; only if it returns **no rows** -> `sp_Update_EmployeeInformation`. Then `SyncEmployeeShiftDeclareInternal` / `SyncEmployeeProductLineDeclareInternal` (inline SQL diff: DELETE removed ids, INSERT new ids into `tblEmpwiseShiftDeclare` / `tblEmpwiseProductLineDeclare`), then `sp_Save_EmployeeAllowanceDetail(@EmpInfoId,@AllowanceId)` once per selected allowance. Returns the PK; `0` means duplicate.
3. If PK > 0 -> **User**: `UserInfoDAL.SaveUserInfo(aMasterUser, MarketList(empty), DisArray, UserId)` (`Library.DAL/DoctorModule_DAL/UserInfoDAL.cs:330`): `UserType="Employee"`, `UserTypeId=3`, `EmpInfoId=PK`, `UserName=EmpName`; new user -> `sp_check_UserInfo_Save` (0 rows required) then `sp_Save_UserMaster_New`; existing user -> `sp_check_da_UserInfo` then `sp_Update_UserMaster_new`; then one `sp_Save_UserCompanyUnit(@UserId,@ComUnitId)` per selected depot (DIC only). **The returned `ResultInfo` (`Res2`) is never inspected** (`:1134`).
4. **Hierarchy row** by `hfRoleType`: `MIO` -> `Save_MIOInfo` (TerritoryId), `AM` -> `Save_ASMInfo` (AreaId), `DZSM` -> `Save_RSMInfo` (RegionId), `NSM` -> `Save_NSMInfo` (GroupId) (`RSMSetupDal.cs:1106/21/1034/346`); each with `EmployeeId=PK`, `IsActive=chkIsActive`, `ActiveDate=txtacDate`, `entryBy=Session UserId`. Results also ignored.
5. Alert `Operation successful!` -> redirect `EmployeeRecords.aspx`. If PK==0 alert `Already Exist!`.
**Transactions**: `DataAccessManager` lazily begins a transaction at the first write and commits in `SqlConnectionClose()` (`Library.DAL/DataManager/DataAccessManager.cs:122-149,407-472`). Each DAL method (`SaveEmployeeInformation`, `SaveUserInfo`, `Save_MIOInfo`...) is its own connection/transaction, so the 3-4 steps above are **not atomic**. The DAL instances are `static` fields on the page class (`:17-23`) shared by concurrent requests.
**try/catch**: DAL methods roll back and rethrow; the page has no try/catch around `btnSave_Click` (unhandled exception -> ASP.NET error page); read paths swallow all exceptions (`catch (Exception ex) { }`).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| EmployeeInformationDaL.GetEmployeeInfoForEdit | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:619 | sp_Get_EmployeeInformation_ById | @id |
| EmployeeInformationDaL.GetEmployeeShiftIds | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:154 | inline SQL: tblEmpwiseShiftDeclare | @EmpInfoId |
| EmployeeInformationDaL.GetEmployeeProductLineIds | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:183 | inline SQL: tblEmpwiseProductLineDeclare | @EmpInfoId |
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
| SeedDataDAL.GetProLineDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:629 | sp_GET_ProductLine |  |
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
| sp_GET_ProductLine | SeedDataDAL.GetProLineDataTableList | body NOT in repository | read-like name (INFERRED) |
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
- Inline SQL (CONFIRMED, `EmployeeInformationDaL.cs:129-286`): `SELECT ShiftId FROM dbo.tblEmpwiseShiftDeclare WHERE EmpInfoId=@EmpInfoId` / `DELETE ... WHERE EmpInfoId AND ShiftId` / `INSERT (EmpInfoId, ShiftId)`; same three statements for `dbo.tblEmpwiseProductLineDeclare (EmpInfoId, ProductLineID)`.
- All other writes are via SPs whose bodies are not in the repository: employee master (`sp_Save_EmployeeInformation`; the table is INFERRED to be `tblEmpGeneralInfo` because the legacy page `SInventory_UI/EmpGeneralInfoEntry` inserts the same column names `EmpInfoId, EmpMasterCode, EmpName, FatherName, ... DepartmentId, DesignationId` into it - `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:18`), user master (`sp_Save_UserMaster_New`, table INFERRED `UserMaster`/`tblUserMaster`), `sp_Save_UserCompanyUnit`, `sp_Save_MIOInfo`/`sp_Save_ASMInfo`/`sp_Save_RSMInfo`/`sp_Save_NSMInfo`, `sp_Save_EmployeeAllowanceDetail`.
- Reads: `sp_Get_EmployeeInformation_ById`, `sp_GET_UserMaster_ByEmpId`, `sp_GET_MIOInfo_ByEmpId`, `sp_GET_ASMInfo_ByEmpId`, `sp_GET_DZSMInfo_ByEmpId`, `sp_GET_NSMInfo_ByEMPId`, lookups listed in section 16.
- Relationship model (CONFIRMED from parameters): `Employee.EmpInfoId` 1-1 `User.EmpInfoId`; `MIOInfo(EmployeeId=EmpInfoId, TerritoryId)`, `ASMInfo(EmployeeId, AreaId)`, `RSMInfo(EmployeeId, RegionId)`, `NSMInfo(EmployeeId, GroupId)`; hierarchy Group > Region(Zone) > Area > Territory (> SubTerritory > Market) via the cascading loaders `GetZone_byGroupId_Active`, `GetArea_ByZoneId_Active`, `GetTerritory_ByAreaId_Active`.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend (`Validation()` `EmployeeSetup.aspx.cs:634-977`, in this order) | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| txtEmpName | required | no | `:652` -> red border + tooltip `please fill out this field`, focus | UNKNOWN | (tooltip only) | BR-M15-001 |
| txtEmpCode | required | no | `:661` | UNKNOWN | (tooltip) | BR-M15-001 |
| txtDateofjoin | required | no | `:671` | UNKNOWN | (tooltip) | BR-M15-001 |
| ShiftSelect | >= 1 shift selected | no | `:681` (`GetSelectedShiftIds().Count==0`) | UNKNOWN | (tooltip) | BR-M15-001 |
| txtNIDNO | if non-empty length must be 17 | MaxLength 17 | `:692` | - | `NID No must be 17 digits!` (`showpop6`) | BR-M15-002 |
| txtEmpContactNo | if non-empty length must be 11 | MaxLength 11 | `:706` | - | `Employee Contact No must be 11 digits!` | BR-M15-002 |
| ReferenceContactNo | if non-empty length 11 | MaxLength 11 | `:721` | - | `Reference Contact No  must be 11 digits!` | BR-M15-002 |
| txtEmergencyContactNo | if non-empty length 11 | MaxLength 11 | `:735` | - | `Emergency Contact No must be 11 digits!` | BR-M15-002 |
| txtEmpCode | must not already exist | no | `:750-766` `check_UserInfoByEmpCode(code,"UserEmpCode")` -> `sp_check_Vali_EmployeeInfoEntry`; rows>0 rejects | SP body UNKNOWN | `Employee Code Already Exist in User!` | BR-M15-003 |
| txtLoginName | required | no | `:769` | UNKNOWN | (tooltip) | BR-M15-005 |
| txtPassword | required | no | `:777` | UNKNOWN | (tooltip) | BR-M15-005 |
| ddlUserRole | required (613 only; commented out in PAGE-615) | no | `:785` | - | (tooltip) | BR-M15-005 |
| txtacDate | required (active/inactive date) | no | `:813` | - | (tooltip) | BR-M15-005 |
| TeritorySelect (role MIO) | required + vacancy check `MIObyTeriID` | no | `:824-851` | SP UNKNOWN | `Employee  Already Exist in Territory!` | BR-M15-006, BR-M15-007 |
| AreaSelect (role AM) | required + `AMbyAreaID` | no | `:855-881` | SP UNKNOWN | `Employee  Already Exist in Area!` | BR-M15-007 |
| ZoneSelect (role DZSM) | required + `DZSMbyZoneID` | no | `:886-912` | SP UNKNOWN | `Employee  Already Exist in Zone!` | BR-M15-007 |
| GroupSelect (role NSM) | required + `NSMbyGroupID` | no | `:917-943` | SP UNKNOWN | `Employee  Already Exist in Group!` | BR-M15-007 |
| ddlDistributionCenter (role DIC) | >= 1 depot | no | `:946-955` | - | (tooltip) | BR-M15-008 |
| Not validated | e-mail format, date order (join vs probation end vs job-left), IMEI (commented out `:794-811`), password strength | - | - | - | - | GAP-M15-004 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-001 | Employee name, employee code, joining date and at least one shift are mandatory to create an employee; everything else is optional (stored as NULL when blank). | `Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs:652-688` | code-behind `Validation()` | CONFIRMED |
| BR-M15-002 | NID (if given) must be exactly 17 chars; employee, reference and emergency mobile numbers (if given) exactly 11 chars. No format/digit check beyond `FilteredTextBoxExtender`/`MaxLength`. | `EmployeeSetup.aspx.cs:692-747`; markup `EmployeeSetup.aspx:96,235,251,337` | `Validation()` + markup | CONFIRMED |
| BR-M15-003 | Employee code must be unique against existing user/employee data (`sp_check_Vali_EmployeeInfoEntry` with `@PageName='UserEmpCode'`). On the edit page the update variant `sp_check_Vali_EmployeeInfoUpdate(@EmpId,@EmpMasterCode)` is used. SP bodies absent: what "exists" means (employee master vs user) is UNKNOWN. | `EmployeeSetup.aspx.cs:750-766`; `Library.DAL/DoctorModule_DAL/UserInfoDAL.cs:287-327` | code-behind + DAL + SP | INFERRED (intent), UNKNOWN (SP) |
| BR-M15-004 | Typing the employee code auto-fills login name **and password** with the same value (can be overwritten). | `EmployeeSetup.aspx.cs:1374-1378` | `txtEmpCode_TextChanged` | CONFIRMED |
| BR-M15-005 | Every employee created on this page also gets a login: login name, password, user role and active-date are required; user is created with `UserType='Employee'`, `UserTypeId=3`, `EmpInfoId=<new employee PK>`, `UserName=EmpName`. **Employee <-> user link = `UserMaster.EmpInfoId`**; the session later exposes it as `Session["EmpInfoId"]` (`Solution.Web/Login.aspx.cs:50`). | `EmployeeSetup.aspx.cs:769-819,1078-1134`; `UserInfoDAL.cs:330-420` | code-behind + `UserInfoDAL.SaveUserInfo` | CONFIRMED |
| BR-M15-006 | The user role's `RoleType` (from `sp_GET_UserRoleInfoRoleType_ById`) decides the hierarchy assignment: `MIO` -> territory required; `AM` -> area; `DZSM` -> zone (region); `NSM` -> group; `DIC` -> depots. Other role types (e.g. admin/HR/office) need no market assignment. | `EmployeeSetup.aspx.cs:821-973,1288-1361` | code-behind | CONFIRMED |
| BR-M15-007 | One active occupant per market node on entry: territory (MIO), area (AM), zone (DZSM), group (NSM) is checked with `sp_check_Vali_EmployeeInfoEntry` (`MIObyTeriID`/`AMbyAreaID`/`DZSMbyZoneID`/`NSMbyGroupID`); any returned row blocks the save. Whether the check ignores inactive holders is UNKNOWN. | `EmployeeSetup.aspx.cs:836-941` | code-behind + SP | INFERRED |
| BR-M15-008 | A `DIC` (depot in-charge) user must be tied to at least one distribution center; one `UserCompanyUnit` row per chosen depot. | `EmployeeSetup.aspx.cs:946-955,1122-1134`; `UserInfoDAL.cs:452-467` | code-behind + DAL | CONFIRMED |
| BR-M15-009 | A single "Active" checkbox drives `EmployeeStatus` ('Active'/'Inactive'), `UserStatus` and the hierarchy row `IsActive`; the same date box (`txtacDate`) is the user `ActiveInActiveDate` and the hierarchy `ActiveDate`. | `EmployeeSetup.aspx.cs:1041-1050,1113-1121,1149-1150,1168-1169,1185-1186,1203-1204` | code-behind | CONFIRMED |
| BR-M15-010 | Shifts (many), product lines (many) and monthly allowances (many) are attached to the employee: shifts/product lines by diff-sync (delete removed, insert new) in `tblEmpwiseShiftDeclare` / `tblEmpwiseProductLineDeclare`; allowances by one `sp_Save_EmployeeAllowanceDetail` call per selected id (no delete of previously-saved allowances in C#; SP behaviour UNKNOWN). | `EmployeeInformationDaL.cs:98-125,211-286` | DAL | CONFIRMED (C#), UNKNOWN (allowance SP) |
| BR-M15-011 | Duplicate guard on update path: if `sp_check_EmployeeInfo(@id,@Name=EmpMasterCode)` returns any row, `sp_Update_EmployeeInformation` is **skipped**, PK stays 0 and the UI says `Already Exist!`. On insert the SP itself returns PK 0 for duplicates (SP body UNKNOWN). | `EmployeeInformationDaL.cs:68-95` | DAL | CONFIRMED |
| BR-M15-012 | Dashboard preference: item 0 (`Main Dashboard`) -> `IsMainDashboard=true`, otherwise `IsDepotDashboard=true`. | `EmployeeSetup.aspx.cs:1100-1111` | code-behind | CONFIRMED |
| BR-M15-013 | With `?MID=` the page shows data but neither Save nor Update is visible (`btnUpdate.Visible=true` is commented out at `:36`; `btnSave` is made visible only in the `else`), so this page **cannot update** an existing employee; updates go through PAGE-614 (personal/job data only). Changing a user's role, password, or hierarchy post-creation is NOT possible from any M15 page (legacy links to `UserSetup.aspx`/`MioUpdateInfo.aspx` are commented out in `EmployeeRecords.aspx:295-318`). | `EmployeeSetup.aspx.cs:34-44`; `EmployeeSetup.aspx` btnSave/btnUpdate `Visible="false"` | code-behind | CONFIRMED |
| BR-M15-014 | Hierarchy save: `MIOId/ASMId/RSMId/NSMId == 0` -> insert (`sp_Save_*Info`, PK returned); `> 0` -> `sp_check_*Info(@TerritoryId/@AreaId/... , @Id)` must return no rows, then `sp_UD_*Info`; otherwise `ResultInfo.isValiCheck=true`. `CompanyId` on MIO/ASM DTO is never set by this page (defaults 0) while RSM/NSM use `1`. | `RSMSetupDal.cs:21-70,346-392,1034-1080,1106-1160`; `EmployeeSetup.aspx.cs:1142-1205` | DAL | CONFIRMED |

## 20. Permission Matrix
- Session keys read: `UserId` only (`:1067,1134,1151...`). **No role/permission check in this page** (no `UserPersmissionValidation`, no `GetPermissionForUserRole`, no `UserRoleID` test) - access is purely menu-based; anyone who knows the URL can open it and, if authenticated, create users with **any** role (including admin role id) because `ddlUserRole` lists all roles from `sp_Get_UserRoleInfo`.
- Master page redirects to `Login.aspx` only on first GET when `Session["UserId"]` is null (`Solution.Web/MasterPages/NewMasterPage.master.cs:26-60`); postbacks are not re-checked there, and `Session["UserId"].ToString()` in `btnSave_Click` throws `NullReferenceException` with an expired session.
- `Web.config` has no `<authorization>` block (grep) - anonymous requests reach the .aspx pipeline; only the master-page redirect protects it.
- Role-based UI: panels for hierarchy/depots depend on the selected *target* user's role type, not on the logged-in user.

## 21. Status Lifecycle
| Entity | Column | Values | Set by | Source |
|---|---|---|---|---|
| Employee | `EmployeeStatus` | `Active` / `Inactive` | `chkIsActive` -> `aMaster.EmployeeStatus` | `EmployeeSetup.aspx.cs:1041-1050` |
| User | `UserStatus` (+`ActiveInActiveDate`) | `Active` / `Inactive` | same checkbox | `:1113-1121` |
| Hierarchy row (MIO/ASM/RSM/NSM) | `IsActive`, `ActiveDate` | bool | same checkbox / `txtacDate` | `:1149-1204` |
| Probation | `IsProbition`, `ProbitionEndDate` | bool / date | checkbox + date | `:1038,1011` |
| Temporary code | `IsTempEmployeeCode` | bool | checkbox | `:1039` |
Lifecycle (INFERRED): created Active/Inactive at onboarding -> later Active<->Inactive via PAGE-614 (employee) ; user/hierarchy activation cannot be changed from M15 pages after creation except through FieldForce web-methods (PAGE-574: `MioInactiveById`, etc.).

## 22. Error Handling
- Validation errors: CSS `is-invalid` + tooltip or `showpop6('<msg>')` popup; first failing rule returns immediately.
- DAL failures: rollback + rethrow (unhandled -> yellow screen; `Global.asax` `Application_Error` is empty).
- All read/populate blocks use empty `catch (Exception ex) { }` - a failed dropdown load silently leaves an empty list; a failed `GetOneRecord` leaves the form blank without a message.
- Duplicate/failed save shows the single generic message `Already Exist!` regardless of the real cause (`:1218`).
- `showMessageBox` escapes quotes incorrectly (`message.Replace("'", "\'")` is a no-op in C#) - unused here.

## 23. Audit
- `@EntryBy` / `@UpdateBy` (= `Session["UserId"]`) passed to `sp_Save_EmployeeInformation` / `sp_Update_EmployeeInformation`, `@EntryBy` to `sp_Save_UserMaster_New`, `@entryBy` to hierarchy SPs. Column names UNKNOWN. No before/after log, no change history, no login-log write on this page.
- Passwords are read back from `sp_GET_UserMaster_ByEmpId` and put in a text box (`:205`) - stored/handled in clear text as far as this code shows (GAP-M15-004).

## 24. Integration
None external. Consumers of the data created here: menu engine (`UserRoleID`), login (`sp_...` login SP -> Session keys), mobile app users (`IsAppsUser`, `IMEI_One/Two`), order/tour/attendance modules through `EmpInfoId` and the MIO/ASM/RSM/NSM tables.

## 25. Reports
None from this page. Employee list: PAGE-612 (`EmployeeRecords.aspx` via `sp_Get_EmployeeInformationList_Prm`).

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, EmployeeInformationDaL, RSMSetupDal, SeedDataDAL, UserInfoDAL; DAO: ASMInfo, EmployeeInformation, MIOInfo, NSMInfo, RSMInfo, ResultInfo, UserDAO; Tables (inline): tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-613-01 | Save with name, code, join date, one shift, login, password, role (non-field role), active date | Employee + user created; alert `Operation successful!`; redirect to list | BR-M15-001, 005 |
| TC-613-02 | Leave name empty | Red border + focus, no DB write | BR-M15-001 |
| TC-613-03 | No shift selected | Blocked at shift box | BR-M15-001 |
| TC-613-04 | NID with 16 chars / phone with 10 chars | `NID No must be 17 digits!` / `... must be 11 digits!` | BR-M15-002 |
| TC-613-05 | Employee code already used | `Employee Code Already Exist in User!` | BR-M15-003 |
| TC-613-06 | Type code `E100` | Login and password boxes show `E100` | BR-M15-004 |
| TC-613-07 | Role type MIO, no territory | Territory box flagged | BR-M15-006 |
| TC-613-08 | Role type MIO, territory already occupied | `Employee  Already Exist in Territory!` | BR-M15-007 |
| TC-613-09 | Role type DIC, no depot | Blocked | BR-M15-008 |
| TC-613-10 | Login name that already belongs to another user (edge) | Employee saved, user NOT created, UI still says success (defect) | BR-M15-005, GAP-M15-002 |
| TC-613-11 | Open `?MID=<id>` | All data shown, no Save button | BR-M15-013 |
| TC-613-12 | Untick Active | `EmployeeStatus/UserStatus='Inactive'` and hierarchy `IsActive=false` | BR-M15-009 |
| TC-613-13 | Two concurrent saves from different browsers (edge) | Risk of shared static DAL/connection state | GAP-M15-003 |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/EmployeeSetup.aspx`, `Solution.Web/MasterSetup_UI/EmployeeSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | Solution.Web/MasterSetup_UI/EmployeeSetup.aspx(.cs) | n/a | MISSING | - |
| Atomic onboarding | None existed | 3-4 separate transactions; user/hierarchy results ignored | SP bodies UNKNOWN | ACTUAL-DEFECT | GAP-M15-002 |
| Edit existing employee | None existed | Save hidden when `?MID` | n/a | ACTUAL | BR-M15-013 |
| Role authorisation | None existed | none in code | UNKNOWN | ACTUAL-GAP | GAP-M15-005 |
| Column-level DB mapping | None existed | SP params known | SP bodies absent | UNKNOWN | GAP-M15-001 |

## 30. Known Gaps
- **GAP-M15-002** User/hierarchy save results ignored; employee can be committed without login (`EmployeeSetup.aspx.cs:1134,1151,1170,1187,1205`).
- **GAP-M15-003** DAL objects are `static` fields (`:17-23`) holding a shared `DataAccessManager` (connection + transaction state) across concurrent requests.
- **GAP-M15-004** Password default = employee code, stored/handled in clear text, read back into UI (`:205,1376-1377`); no strength/e-mail/date-order validation.
- **GAP-M15-005** No server-side authorisation; any authenticated user can create users with any role.
- **GAP-M15-001** 42 SPs called, bodies not in repo.
- Duplicate of PAGE-615 / PAGE-614 logic (copy-paste); empty `btnReset_Click`; `loadUserRole` postback link reloads roles only.
- `MIOInfo.CompanyId`/`ASMInfo.CompanyId` never set -> `0` sent to `@CompanyId`.
- Vacancy checks run on every save including edit-with-same-values scenarios (not reachable here since page is read-only with MID).

## 31. Change Impact
- Table `UserMaster` (INFERRED name from `sp_Save_UserMaster_New`) is read by the login SP, menu SP and every approval page via `Session["EmpInfoId"]`.
- `tblEmpwiseShiftDeclare` / `tblEmpwiseProductLineDeclare` written also by PAGE-614 (`EmployeeSetupEdit.aspx.cs`); PAGE-615 uses the legacy single `ShiftId`.
- DAL classes `EmployeeInformationDaL`, `UserInfoDAL`, `RSMSetupDal` are shared with PAGE-574 (FieldForce web-methods), PAGE-612, PAGE-614, PAGE-615 and the MIO/AM/DZSM/NSM setup pages in `DoctorModule_UI` (grep of `FieldForce.aspx/` callers lists 18 pages).
- Changing `sp_check_Vali_EmployeeInfoEntry` affects both this page and PAGE-615.

