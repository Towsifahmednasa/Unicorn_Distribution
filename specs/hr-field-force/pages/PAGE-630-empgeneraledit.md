---
page_id: PAGE-630
module: M15
page: EmpGeneralEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-630 — EmpGeneralEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-630 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmpGeneralEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_EmpGeneralEdit |
| aspx size | 22866 bytes |
| Code-behind LOC | 124 |
| Inline script size | 378 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **EmpGeneralEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/EmpGenInfView.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/EmpGeneralEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs |
| JS include | ../css/Date/jquery.datepick.js |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| CSS include | ../css/Date/jquery.datepick.css |
| BLL class | EmpGeneralInfoBLL (Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs) |
| DAL class | EmpGeneralInfoDAL (Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs) |
| DAO/entity class | EmpGeneralInfo (Library.DAO/SInventory_Entities/EmpGeneralInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpInfoIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| empNameTextBox | TextBox |  | no validator found |  |  |  |  |
| bloodGroupTextBox | TextBox |  | no validator found |  |  |  |  |
| shortNameTextBox | TextBox |  | no validator found |  |  |  |  |
| sposNameTextBox | TextBox |  | no validator found |  |  |  |  |
| fatherNameTextBox | TextBox |  | no validator found |  |  |  |  |
| sposBirthDtTextBox | TextBox |  | no validator found |  |  |  |  |
| motherNameTextBox | TextBox |  | no validator found |  |  |  |  |
| mobileNoTextBox | TextBox |  | no validator found |  |  |  |  |
| religionTextBox | TextBox |  | no validator found |  |  |  |  |
| phoneNoTextBox | TextBox |  | no validator found |  |  |  |  |
| nationalityTextBox | TextBox |  | no validator found |  |  |  |  |
| emailTextBox | TextBox |  | no validator found |  |  |  |  |
| dateOfBirthTextBox | TextBox |  | no validator found |  |  |  |  |
| maritalStatusTextBox | TextBox |  | no validator found |  |  |  |  |
| placeOfBirthTextBox | TextBox |  | no validator found |  |  |  |  |
| medInfoTextBox | TextBox |  | no validator found |  |  |  |  |
| prtAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| permAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| nationalIdNoTextBox | TextBox |  | no validator found |  |  |  |  |
| genderTextBox | TextBox |  | no validator found |  |  |  |  |
| designationDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| joiningDateTextBox | TextBox |  | no validator found |  |  |  |  |
| departmentDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| updateButton0 | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton0 | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| EmpInfoIdHiddenField | EmpGeneralInfo.EmpInfoId | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters | @EmpInfoId | UNKNOWN (SP body not in repo) | UNKNOWN |
| designationDropDownList | EmpGeneralInfo.DesignationId | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters | @DesignationId | UNKNOWN (SP body not in repo) | UNKNOWN |
| departmentDropDownList | EmpGeneralInfo.DepartmentId | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters | @DepartmentId | UNKNOWN (SP body not in repo) | UNKNOWN |
| joiningDateTextBox | EmpGeneralInfo.JoiningDate | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| empNameTextBox | EmpGeneralInfo.EmpName | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| shortNameTextBox | EmpGeneralInfo.ShortName | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| fatherNameTextBox | EmpGeneralInfo.FatherName | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| motherNameTextBox | EmpGeneralInfo.MotherName | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| religionTextBox | EmpGeneralInfo.Religion | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| nationalityTextBox | EmpGeneralInfo.Nationality | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| dateOfBirthTextBox | EmpGeneralInfo.DateOfBirth | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| placeOfBirthTextBox | EmpGeneralInfo.PlaceOfBirth | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| bloodGroupTextBox | EmpGeneralInfo.BloodGroup | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| genderTextBox | EmpGeneralInfo.Gender | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| prtAddressTextBox | EmpGeneralInfo.AddressPresent | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| permAddressTextBox | EmpGeneralInfo.AddressPermanent | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| medInfoTextBox | EmpGeneralInfo.MedicalInformation | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| phoneNoTextBox | EmpGeneralInfo.PhoneNo | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| mobileNoTextBox | EmpGeneralInfo.CellNumber | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| emailTextBox | EmpGeneralInfo.Email | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| maritalStatusTextBox | EmpGeneralInfo.MaritalStatus | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| nationalIdNoTextBox | EmpGeneralInfo.NationalIdNo | SInventory_UI_EmpGeneralEdit.updateButton_Click | EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.UpdateEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| designationDropDownList | DesignationNameLoad → EmpGeneralInfoBLL.LoadDesignationToDropDownBLL | inline SQL | DesigName/DesignationId | tblDesignation |
| departmentDropDownList | DepartmentNameLoad → EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL | inline SQL | DeptName/DeptId | tblDepartment |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_EmpGeneralEdit.Page_Load | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_EmpGeneralEdit.Page_Load → EmpGeneralInfoBLL.EmpGeneralInfoEditLoad / EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL / EmpGeneralInfoBLL.LoadDesignationToDropDownBLL → EmpGeneralInfoDAL.EmpInfoEditLoad / EmpGeneralInfoDAL.LoadDepartmentName / EmpG | no | no |
| SInventory_UI_EmpGeneralEdit.updateButton_Click | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs:25 | object sender, EventArgs e | SInventory_UI_EmpGeneralEdit.updateButton_Click → EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo → EmpGeneralInfoDAL.EmployeeParameters / EmpGeneralInfoDAL.UpdateEmployeeInfo | no | no |
| SInventory_UI_EmpGeneralEdit.EmpGeneralInfoLoad | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs:77 | string employeeId | SInventory_UI_EmpGeneralEdit.EmpGeneralInfoLoad → EmpGeneralInfoBLL.EmpGeneralInfoEditLoad → EmpGeneralInfoDAL.EmpInfoEditLoad | no | no |
| SInventory_UI_EmpGeneralEdit.DesignationNameLoad | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs:109 |  | SInventory_UI_EmpGeneralEdit.DesignationNameLoad → EmpGeneralInfoBLL.LoadDesignationToDropDownBLL → EmpGeneralInfoDAL.LoadDesignationName | no | no |
| SInventory_UI_EmpGeneralEdit.DepartmentNameLoad | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs:114 |  | SInventory_UI_EmpGeneralEdit.DepartmentNameLoad → EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL → EmpGeneralInfoDAL.LoadDepartmentName | no | no |
| SInventory_UI_EmpGeneralEdit.closeButton_Click | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs:121 | object sender, EventArgs e | SInventory_UI_EmpGeneralEdit.closeButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:63 |
| EmpGeneralInfoBLL.LoadDesignationToDropDownBLL | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:59 |
| EmpGeneralInfoBLL.EmpGeneralInfoEditLoad | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:78 |
| EmpGeneralInfoBLL.UpdateDataForEmpGeneralInfo | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:68 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| EmpGeneralInfoDAL.LoadDepartmentName | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:49 | inline SQL: tblDepartment |  |
| EmpGeneralInfoDAL.LoadDesignationName | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:42 | inline SQL: tblDesignation |  |
| EmpGeneralInfoDAL.EmpInfoEditLoad | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:57 | inline SQL: tblEmpGeneralInfo | @EmpInfoId |
| EmpGeneralInfoDAL.UpdateEmployeeInfo | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:100 | inline SQL: tblEmpGeneralInfo |  |
| EmpGeneralInfoDAL.EmployeeParameters | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:141 | delegates / no SQL literal | @AddressPermanent, @AddressPresent, @BloodGroup, @CellNumber, @DateOfBirth, @DepartmentId, @DeptName |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblDepartment, tblDesignation, tblEmpGeneralInfo
- Tables WRITTEN (inline SQL reachable): tblEmpGeneralInfo
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblEmpGeneralInfo | AddressPermanent | @AddressPermanent |
| tblEmpGeneralInfo | AddressPresent | @AddressPresent |
| tblEmpGeneralInfo | BloodGroup | @BloodGroup |
| tblEmpGeneralInfo | CellNumber | @CellNumber |
| tblEmpGeneralInfo | DateOfBirth | @DateOfBirth |
| tblEmpGeneralInfo | DepartmentId | @DepartmentId |
| tblEmpGeneralInfo | Designation | @Designation |
| tblEmpGeneralInfo | DesignationId | @DesignationId |
| tblEmpGeneralInfo | Email | @Email |
| tblEmpGeneralInfo | EmpImage | @EmpImage |
| tblEmpGeneralInfo | EmpMasterCode | @EmpMasterCode |
| tblEmpGeneralInfo | EmpName | @EmpName |
| tblEmpGeneralInfo | FatherName | @FatherName |
| tblEmpGeneralInfo | Gender | @Gender |
| tblEmpGeneralInfo | JoiningDate | @JoiningDate |
| tblEmpGeneralInfo | MaritalStatus | @MaritalStatus |
| tblEmpGeneralInfo | MedicalInformation | @MedicalInformation |
| tblEmpGeneralInfo | MotherName | @MotherName |
| tblEmpGeneralInfo | NationalIdNo | @NationalIdNo |
| tblEmpGeneralInfo | Nationality | @Nationality |
| tblEmpGeneralInfo | PhoneNo | @PhoneNo |
| tblEmpGeneralInfo | PlaceOfBirth | @PlaceOfBirth |
| tblEmpGeneralInfo | RefAddress | @RefAddress |
| tblEmpGeneralInfo | RefCellNo | @RefCellNo |
| tblEmpGeneralInfo | RefName | @RefName |
| tblEmpGeneralInfo | Religion | @Religion |
| tblEmpGeneralInfo | ShortName | @ShortName |
| tblEmpGeneralInfo | SignatureImage | @SignatureImage |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler updateButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Data Update Successfully!!! Please Reload | UNKNOWN |
| (handler updateButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 6 methods.
- Redirect targets: none
- User messages: Data Update Successfully!!! Please Reload; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: EmpGeneralInfoBLL; DAL: EmpGeneralInfoDAL; DAO: EmpGeneralInfo; Tables (inline): tblDepartment, tblDesignation, tblEmpGeneralInfo

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/EmpGeneralEdit.aspx`, `Solution.Web/SInventory_UI/EmpGeneralEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 30 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/EmpGeneralEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods EmpGeneralInfoDAL.EmpInfoEditLoad, EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.LoadDepartmentName, EmpGeneralInfoDAL.LoadDesignationName, EmpGeneralInfoDAL.UpdateEmployeeInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
