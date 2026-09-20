---
page_id: PAGE-614
module: M15
page: EmployeeSetupEdit
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-614 — EmployeeSetupEdit

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-614 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmployeeSetupEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_EmployeeSetupEdit |
| aspx size | 33744 bytes |
| Code-behind LOC | 592 |
| Inline script size | 1891 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: edit screen for the **employee's personal and job data only** (no login/role/market assignment). Opened from the pencil icon in the employee list (`EmployeeRecords.aspx:389`, `EmployeeSetupEdit.aspx?MID=<EmpInfoId>`). Shows the same personal/job fields as PAGE-613 and lets HR correct them, re-assign shifts, product lines, monthly allowances and depot, and set the employee Active/Inactive.

## 3. Business Objective
CONFIRMED by code: maintain employee master data after onboarding (name, contact, joining/probation/left dates, department, designation, depot, shifts, product lines, allowances, status) without touching credentials or hierarchy. Without `?MID` the page shows a Save button and would create an employee **without** a user (code path identical to PAGE-613 minus the user block).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `employeerecords.aspx`

## 5. Route
`~/MasterSetup_UI/EmployeeSetupEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStructure.ascx |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | EmployeeInformationDaL (Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | UserInfoDAL (Library.DAL/DoctorModule_DAL/UserInfoDAL.cs) |
| DAO/entity class | EmployeeInformation (Library.DAO/DoctorModule_DAO/EmployeeInformation.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| hfRoleType | HiddenField |  | no validator found |  |  |  |  |
| txtEmpName | TextBox | Enter employee name  | no validator found |  |  |  |  |
| txtEmpCode | TextBox | Enter employee Code  | no validator found |  | ReadOnly=true |  |  |
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
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→btnSave_Click |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Update(this);, OnClick→btnSave_Click |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| id_mastetID | dt | MasterSetup_UI_EmployeeSetupEdit.Validation | — | UserInfoDAL.check_UserInfoByEmpCodeUpdate | UNKNOWN | UNKNOWN | UNKNOWN |
| id_mastetID | EmployeeInformation.EmpInfoId | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpInfoId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | EmployeeInformation.EmpInfoId | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @id | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpCode | EmployeeInformation.EmpMasterCode | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpMasterCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpCode | EmployeeInformation.EmpMasterCode | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Name | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpName | EmployeeInformation.EmpName | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmpName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpFatherName | EmployeeInformation.FatherName | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @FatherName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpMotherName | EmployeeInformation.MotherName | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @MotherName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpdobDate | EmployeeInformation.DateOfBirth | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DateOfBirth | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpAddress | EmployeeInformation.AddressPresent | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @AddressPresent | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpPresentAddress | EmployeeInformation.AddressPermanent | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @AddressPermanent | UNKNOWN (SP body not in repo) | UNKNOWN |
| GenderSelect | EmployeeInformation.Gender | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Gender | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReligionSelect | EmployeeInformation.Religion | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Religion | UNKNOWN (SP body not in repo) | UNKNOWN |
| Nationality | EmployeeInformation.Nationality | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Nationality | UNKNOWN (SP body not in repo) | UNKNOWN |
| BloodGroupSelect | EmployeeInformation.BloodGroup | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @BloodGroup | UNKNOWN (SP body not in repo) | UNKNOWN |
| MaritalStatusSelect | EmployeeInformation.MaritalStatus | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @MaritalStatus | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtNIDNO | EmployeeInformation.NationalIdNo | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @NationalIdNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtDateofjoin | EmployeeInformation.JoiningDate | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @JoiningDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtJobLeftDate | EmployeeInformation.JobLeftDate | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @JobLeftDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtProbationEndDate | EmployeeInformation.ProbitionEndDate | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @ProbitionEndDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLastCompanyName | EmployeeInformation.LastCompanyName | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @LastCompanyName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtLastJobLocation | EmployeeInformation.LastJobLocation | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @LastJobLocation | UNKNOWN (SP body not in repo) | UNKNOWN |
| DepartmentSelect | EmployeeInformation.DepartmentId | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DepartmentId | UNKNOWN (SP body not in repo) | UNKNOWN |
| DesignationSelect | EmployeeInformation.DesignationId | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @DesignationId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlDepotSelect | EmployeeInformation.CompanyUnitId | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @CompanyUnitId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmail | EmployeeInformation.Email | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @Email | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmpContactNo | EmployeeInformation.CellNumber | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @CellNumber | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReferencePersonName | EmployeeInformation.RefName | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @RefName | UNKNOWN (SP body not in repo) | UNKNOWN |
| ReferenceContactNo | EmployeeInformation.RefContactNo | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @RefContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmergencyContactNo | EmployeeInformation.EmrgContactNo | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmrgContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmrgContactNoRelaton | EmployeeInformation.EmrgContactNoRelaton | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @EmrgContactNoRelaton | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsProbation | EmployeeInformation.IsProbition | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @IsProbition | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsTempEmployeeCode | EmployeeInformation.IsTempEmployeeCode | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | (none — page calls DAL directly) | EmployeeInformationDaL.SaveEmployeeInformation | @IsTempEmployeeCode | UNKNOWN (SP body not in repo) | UNKNOWN |

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
| ProductLineSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlDepotSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx.cs:436 | — | EmployeeInformationDaL.SaveEmployeeInformation, EmployeeInformationDaL.SyncEmployeeProductLineDeclar | sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Update_EmployeeInformation, sp_check_EmployeeInfo, sp_c | writes tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | MasterSetup_UI_EmployeeSetupEdit.btnSave_Click | Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx.cs:436 | — | EmployeeInformationDaL.SaveEmployeeInformation, EmployeeInformationDaL.SyncEmployeeProductLineDeclar | sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Update_EmployeeInformation, sp_check_EmployeeInfo, sp_c | writes tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare |
| btnReset | LinkButton click | (postback) | MasterSetup_UI_EmployeeSetupEdit.btnReset_Click | Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx.cs:585 | — | — | — | read-only / none detected |

## 11. Frontend Flow
`Page_Load` (`EmployeeSetupEdit.aspx.cs:26-48`): `LoadInitialInfo()` (departments, designations, shifts, product lines, allowances, depots; no user/role/market lookups) then, with `?MID`, `btnUpdate.Visible=true` and `GetOneRecord` loads `sp_Get_EmployeeInformation_ById`, `tblEmpwiseShiftDeclare`, `tblEmpwiseProductLineDeclare` and the `AllowanceId` CSV (`:50-190`). Update link asks `sweetAlertConfirm_Update`; success -> `EmployeeRecords.aspx`. No cascades, no role panels, no client validation.

## 12. Backend Flow
`btnSave_Click` (`:435-542`): `Validation()` -> build `EmployeeInformation` exactly as PAGE-613 (`EmpInfoId = id_mastetID`) -> `EmployeeInformationDaL.SaveEmployeeInformation(aMaster, MonthlyArray, Session["UserId"], 0, shiftIds, productLineIds)` -> `MyPK>0` => `Operation successful!` else `Already Exist!`. Update path: `sp_check_EmployeeInfo(@id, @Name=EmpMasterCode)` must return no rows, then `sp_Update_EmployeeInformation` (`@UpdateBy`), then diff-sync of `tblEmpwiseShiftDeclare` / `tblEmpwiseProductLineDeclare`, then `sp_Save_EmployeeAllowanceDetail` per selected allowance (`EmployeeInformationDaL.cs:68-125`). **No user or hierarchy update**, so `EmployeeStatus` can become `Inactive` while `UserStatus` and the MIO/ASM/RSM/NSM `IsActive` remain unchanged (CONFIRMED absence of any such call in this page; effect on login UNKNOWN).

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
| SeedDataDAL.GetlMonthlyAllawanceList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:689 | sp_GET_MonthlyAllowanceList |  |
| SeedDataDAL.GetProLineDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:629 | sp_GET_ProductLine |  |
| CommonDataLoad.GetDepartment_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:559 | sp_CS_Department_Active |  |
| CommonDataLoad.GetDesignation_Active_Emp | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:579 | sp_CS_Designation_Active |  |
| CommonDataLoad.GetShift_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:598 | sp_CS_Shift_Active |  |
| SeedDataDAL.GetDistributionCenterDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:669 | sp_GET_DistributionCenter |  |
| UserInfoDAL.check_UserInfoByEmpCodeUpdate | Library.DAL/DoctorModule_DAL/UserInfoDAL.cs:310 | sp_check_Vali_EmployeeInfoUpdate | @EmpId, @EmpMasterCode |
| EmployeeInformationDaL.SaveEmployeeInformation | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:19 | sp_Save_EmployeeAllowanceDetail, sp_Save_EmployeeInformation, sp_Update_Employee | @AddressPermanent, @AddressPresent, @AllowanceId, @BloodGroup, @CellNumber, @CompanyId, @CompanyUnit |
| EmployeeInformationDaL.SyncEmployeeProductLineDeclareInternal | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:252 | inline SQL: tblEmpwiseProductLineDeclare, tblEmpwiseProductLineDeclare | @EmpInfoId, @ProductLineID |
| EmployeeInformationDaL.SyncEmployeeShiftDeclareInternal | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:212 | inline SQL: tblEmpwiseShiftDeclare, tblEmpwiseShiftDeclare | @EmpInfoId, @ShiftId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_Department_Active | CommonDataLoad.GetDepartment_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Designation_Active | CommonDataLoad.GetDesignation_Active_Emp | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Shift_Active | CommonDataLoad.GetShift_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DistributionCenter | SeedDataDAL.GetDistributionCenterDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MonthlyAllowanceList | SeedDataDAL.GetlMonthlyAllawanceList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProductLine | SeedDataDAL.GetProLineDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeInformation_ById | EmployeeInformationDaL.GetEmployeeInfoForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Save_EmployeeAllowanceDetail | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | write-like name |
| sp_Save_EmployeeInformation | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | write-like name |
| sp_Update_EmployeeInformation | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | write-like name |
| sp_check_EmployeeInfo | EmployeeInformationDaL.SaveEmployeeInformation | body NOT in repository | read-like name (INFERRED) |
| sp_check_Vali_EmployeeInfoUpdate | UserInfoDAL.check_UserInfoByEmpCodeUpdate | body NOT in repository | write-like name |

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
| Field | Rule | Backend (`Validation()` `EmployeeSetupEdit.aspx.cs:297-431`) | Message | BR |
|---|---|---|---|---|
| txtEmpName / txtEmpCode / txtDateofjoin | required | `:312-340` | tooltip `please fill out this field` | BR-M15-001 |
| ShiftSelect | >= 1 shift | `:341` | tooltip | BR-M15-001 |
| NID / employee, reference, emergency phone | length 17 / 11 when supplied | `:352-408` | `NID No must be 17 digits!` etc. | BR-M15-002 |
| txtEmpCode | unique among **other** employees: `check_UserInfoByEmpCodeUpdate(EmpId, code)` -> `sp_check_Vali_EmployeeInfoUpdate(@EmpId,@EmpMasterCode)` rows>0 rejects | `:410-424` | `Employee Code Already Exist in User!` | BR-M15-019 |
| Login, password, role, market, IMEI | not on this page | - | - | - |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-019 | On edit the employee-code uniqueness check excludes the record itself (`@EmpId` + code) and, in addition, the DAL blocks the update if `sp_check_EmployeeInfo(@id,@Name=code)` returns a row. | `EmployeeSetupEdit.aspx.cs:411-424`; `UserInfoDAL.cs:309-327`; `EmployeeInformationDaL.cs:68-95` | code-behind + DAL | CONFIRMED (calls), UNKNOWN (SP semantics) |
| BR-M15-020 | This page edits employee master data only: user account (login name, password, role, active/inactive), and MIO/ASM/RSM/NSM assignment are not touched. Changing the employee code here does not change the login name. | `EmployeeSetupEdit.aspx.cs` (no `SaveUserInfo`/`Save_*Info` calls) | code-behind | CONFIRMED |
| BR-M15-021 | Shifts and product lines are replaced by the submitted set (diff-sync: rows for removed ids are physically deleted from `tblEmpwiseShiftDeclare` / `tblEmpwiseProductLineDeclare`); allowances are re-sent one-by-one (no C# delete; SP behaviour UNKNOWN - risk of duplicate allowance rows). | `EmployeeInformationDaL.cs:98-125,211-286` | DAL | CONFIRMED / UNKNOWN |
| BR-M15-022 | Without `?MID` this page exposes Save and creates an employee without login (same DAL path). | `EmployeeSetupEdit.aspx.cs:36-44` | code-behind | CONFIRMED |
| (shared) | BR-M15-001, 002, 009 (Active flag -> `EmployeeStatus` only here), 010, 011 apply. | `EmployeeSetupEdit.aspx.cs` | code-behind | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]` only (used as `@UpdateBy`); no `UserRoleID` check and no `GetPermissionForUserRole` call on this page (the *list* page PAGE-612 has one - see that spec). Direct URL access to `EmployeeSetupEdit.aspx?MID=n` by any logged-in user is possible.

## 21. Status Lifecycle
`EmployeeStatus` Active/Inactive (from `chkIsActive`), probation flag/date, temp-code flag. Employee inactivation here does not cascade to `UserStatus` (BR-M15-020).

## 22. Error Handling
As PAGE-613: validation tooltips/popups; generic `Already Exist!`; DAL exceptions propagate; read failures swallowed (`GetOneRecord` `catch { }` leaves blank form).

## 23. Audit
`@UpdateBy = Session["UserId"]` (update) / `@EntryBy` (insert). No history rows.

## 24. Integration
None.

## 25. Reports
None (list: PAGE-612).

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, EmployeeInformationDaL, SeedDataDAL, UserInfoDAL; DAO: EmployeeInformation, ResultInfo; Tables (inline): tblEmpwiseProductLineDeclare, tblEmpwiseShiftDeclare

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-614-01 | Open `?MID=<id>`, change mobile no to 11 digits, Update | `Operation successful!`; list shows change | BR-M15-002 |
| TC-614-02 | Change code to another employee's code | `Employee Code Already Exist in User!` | BR-M15-019 |
| TC-614-03 | Untick a shift and Update | Row removed from `tblEmpwiseShiftDeclare` | BR-M15-021 |
| TC-614-04 | Untick Active and Update | `EmployeeStatus='Inactive'`; user row unchanged (edge) | BR-M15-020 |
| TC-614-05 | Open without `?MID` and Save | Employee created without user (edge) | BR-M15-022 |
| TC-614-06 | Re-save with allowances selected twice (edge) | Check for duplicate allowance rows (SP unknown) | BR-M15-021 |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx`, `Solution.Web/MasterSetup_UI/EmployeeSetupEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | EmployeeSetupEdit.aspx(.cs) | n/a | MISSING | - |
| Deactivation cascade | None existed | not implemented | UNKNOWN | ACTUAL-GAP | GAP-M15-007 |
| DB mapping | None existed | SP params known | bodies absent | UNKNOWN | GAP-M15-001 |

## 30. Known Gaps
- **GAP-M15-007** Employee inactivation does not deactivate the user login or the market assignment; conversely there is no page in M15 to inactivate a user (except FieldForce web-methods for hierarchy rows).
- Allowance re-save may duplicate rows (no delete in C#; SP unknown).
- Page duplicates 90% of PAGE-613 (copy-paste); `btnSave` visible without `MID` allows employee creation without login.
- Inherits GAP-M15-003 (static DAL) and GAP-M15-005 (no authorisation).

## 31. Change Impact
Shares `EmployeeInformationDaL`, `sp_Update_EmployeeInformation`, `tblEmpwiseShiftDeclare`, `tblEmpwiseProductLineDeclare` with PAGE-613/615. Employee list PAGE-612 links here.

