---
page_id: PAGE-631
module: M15
page: EmpGeneralInfoEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-631 — EmpGeneralInfoEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-631 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmpGeneralInfoEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | HRM_UI_EmpGeneralInfo |
| aspx size | 25992 bytes |
| Code-behind LOC | 276 |
| Inline script size | 1170 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **EmpGeneralInfoEntry** (Entry / Transaction). Page title/heading text: `Employee General Information Part-1`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/EmpGenInfView.aspx`

Outbound references: `empgeninfview.aspx`

## 5. Route
`~/SInventory_UI/EmpGeneralInfoEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| JS include | ../scripts/jquery.min.js |
| JS include | ../scripts/jquery-ui.js |
| JS include | ../scripts/jquery.js |
| JS include | ../css/Date/jquery.datepick.js |
| CSS include | ../css/jquery-ui.css |
| CSS include | ../css/Date/jquery.datepick.css |
| BLL class | EmpGeneralInfoBLL (Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs) |
| DAL class | EmpGeneralInfoDAL (Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs) |
| DAO/entity class | EmpGeneralInfo (Library.DAO/SInventory_Entities/EmpGeneralInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| empViewImageButton | ImageButton |  | no validator found |  |  |  | onclick→empViewImageButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| hidAccordionIndex | HiddenField |  | no validator found |  |  |  |  |
| pictureImage | Image |  | no validator found |  |  |  |  |
| previewButton | Button | Preview | no validator found |  |  |  | onclick→previewButton_Click |
| empNameTextBox | TextBox |  | no validator found |  |  |  |  |
| shortNameTextBox | TextBox |  | no validator found |  |  |  |  |
| fatherNameTextBox | TextBox |  | no validator found |  |  |  |  |
| motherNameTextBox | TextBox |  | no validator found |  |  |  |  |
| pictureFileUpload | FileUpload |  | no validator found |  |  |  |  |
| religionTextBox | TextBox |  | no validator found |  |  |  |  |
| signatureImage | Image |  | no validator found |  |  |  |  |
| sigpreviewButton | Button | Preview | no validator found |  |  |  | onclick→sigpreviewButton_Click |
| nationalityTextBox | TextBox |  | no validator found |  |  |  |  |
| dateOfBirthTextBox | TextBox |  | no validator found |  |  |  |  |
| sigFileUpload | FileUpload |  | no validator found |  |  |  |  |
| placeOfBirthTextBox | TextBox |  | no validator found |  |  |  |  |
| prtAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| permAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| nationalIdNoTextBox | TextBox |  | no validator found |  |  |  |  |
| designationDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| genderDropDown | DropDownList |  | no validator found |  |  | see §9 |  |
| departmentDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| bloodGroupDropDown | DropDownList |  | no validator found |  |  | see §9 |  |
| mobileNoTextBox | TextBox |  | no validator found |  |  |  |  |
| maritalStatusDropDown | DropDownList |  | no validator found |  |  | see §9 |  |
| phoneNoTextBox | TextBox |  | no validator found |  |  |  |  |
| medicalTextBox | TextBox |  | no validator found |  |  |  |  |
| emailTextBox | TextBox |  | no validator found |  |  |  |  |
| referanceNameTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→referanceNameTextBox_TextChanged |
| refranceCellNoTextBox | TextBox |  | no validator found |  |  |  |  |
| refranceAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| joiningDateTextBox | TextBox |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| empNameTextBox | EmpGeneralInfo.EmpName | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| shortNameTextBox | EmpGeneralInfo.ShortName | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| fatherNameTextBox | EmpGeneralInfo.FatherName | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| motherNameTextBox | EmpGeneralInfo.MotherName | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| religionTextBox | EmpGeneralInfo.Religion | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| nationalityTextBox | EmpGeneralInfo.Nationality | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| dateOfBirthTextBox | EmpGeneralInfo.DateOfBirth | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| placeOfBirthTextBox | EmpGeneralInfo.PlaceOfBirth | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| bloodGroupDropDown | EmpGeneralInfo.BloodGroup | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| genderDropDown | EmpGeneralInfo.Gender | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| prtAddressTextBox | EmpGeneralInfo.AddressPresent | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| permAddressTextBox | EmpGeneralInfo.AddressPermanent | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| medicalTextBox | EmpGeneralInfo.MedicalInformation | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| phoneNoTextBox | EmpGeneralInfo.PhoneNo | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| mobileNoTextBox | EmpGeneralInfo.CellNumber | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| emailTextBox | EmpGeneralInfo.Email | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| maritalStatusDropDown | EmpGeneralInfo.MaritalStatus | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| nationalIdNoTextBox | EmpGeneralInfo.NationalIdNo | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| referanceNameTextBox | EmpGeneralInfo.ReferanceName | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| refranceAddressTextBox | EmpGeneralInfo.ReferanceAddress | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| refranceCellNoTextBox | EmpGeneralInfo.ReferanceCellNo | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| departmentDropDownList | EmpGeneralInfo.DepartmentId | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters | @DepartmentId | UNKNOWN (SP body not in repo) | UNKNOWN |
| joiningDateTextBox | EmpGeneralInfo.JoiningDate | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| designationDropDownList | EmpGeneralInfo.Designation | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| designationDropDownList | EmpGeneralInfo.DesignationId | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters | @DesignationId | UNKNOWN (SP body not in repo) | UNKNOWN |
| departmentDropDownList | EmpGeneralInfo.DeptName | HRM_UI_EmpGeneralInfo.submitButton_Click | EmpGeneralInfoBLL.EmpMasterCodeGenerator, EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.SaveEmployeeInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| designationDropDownList | DesignationNameLoad → EmpGeneralInfoBLL.LoadDesignationToDropDownBLL | inline SQL | DesigName/DesignationId | tblDesignation |
| genderDropDown | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| departmentDropDownList | DepartmentNameLoad → EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL | inline SQL | DeptName/DeptId | tblDepartment |
| bloodGroupDropDown | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| maritalStatusDropDown | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: yes; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| HRM_UI_EmpGeneralInfo.Page_Load | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:16 | object sender, EventArgs e | HRM_UI_EmpGeneralInfo.Page_Load → EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL / EmpGeneralInfoBLL.LoadDesignationToDropDownBLL → EmpGeneralInfoDAL.LoadDepartmentName / EmpGeneralInfoDAL.LoadDesignationName | no | no |
| HRM_UI_EmpGeneralInfo.previewButton_Click | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:26 | object sender, EventArgs e | HRM_UI_EmpGeneralInfo.previewButton_Click | no | no |
| HRM_UI_EmpGeneralInfo.sigpreviewButton_Click | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:33 | object sender, EventArgs e | HRM_UI_EmpGeneralInfo.sigpreviewButton_Click | no | no |
| HRM_UI_EmpGeneralInfo.DesignationNameLoad | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:80 |  | HRM_UI_EmpGeneralInfo.DesignationNameLoad → EmpGeneralInfoBLL.LoadDesignationToDropDownBLL → EmpGeneralInfoDAL.LoadDesignationName | no | no |
| HRM_UI_EmpGeneralInfo.DepartmentNameLoad | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:84 |  | HRM_UI_EmpGeneralInfo.DepartmentNameLoad → EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL → EmpGeneralInfoDAL.LoadDepartmentName | no | no |
| HRM_UI_EmpGeneralInfo.submitButton_Click | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:207 | object sender, EventArgs e | HRM_UI_EmpGeneralInfo.submitButton_Click → EmpGeneralInfoBLL.EmpMasterCodeGenerator / EmpGeneralInfoBLL.SaveDataFoEmployeeInfo → EmpGeneralInfoDAL.EmployeeParameters / EmpGeneralInfoDAL.SaveEmployeeInfo | no | no |
| HRM_UI_EmpGeneralInfo.empViewImageButton_Click | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:266 | object sender, ImageClickEventArgs e | HRM_UI_EmpGeneralInfo.empViewImageButton_Click | no | no |
| HRM_UI_EmpGeneralInfo.referanceNameTextBox_TextChanged | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs:272 | object sender, EventArgs e | HRM_UI_EmpGeneralInfo.referanceNameTextBox_TextChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| EmpGeneralInfoBLL.LoadDepartmentToDropDownBLL | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:63 |
| EmpGeneralInfoBLL.LoadDesignationToDropDownBLL | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:59 |
| EmpGeneralInfoBLL.SaveDataFoEmployeeInfo | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:17 |
| EmpGeneralInfoBLL.EmpMasterCodeGenerator | Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:35 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| EmpGeneralInfoDAL.LoadDepartmentName | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:49 | inline SQL: tblDepartment |  |
| EmpGeneralInfoDAL.LoadDesignationName | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:42 | inline SQL: tblDesignation |  |
| EmpGeneralInfoDAL.SaveEmployeeInfo | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:17 | inline SQL: tblEmpGeneralInfo |  |
| EmpGeneralInfoDAL.EmployeeParameters | Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:141 | delegates / no SQL literal | @AddressPermanent, @AddressPresent, @BloodGroup, @CellNumber, @DateOfBirth, @DepartmentId, @DeptName |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblDepartment, tblDesignation
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
| tblEmpGeneralInfo | DeptName | @DeptName |
| tblEmpGeneralInfo | Designation | @Designation |
| tblEmpGeneralInfo | DesignationId | @DesignationId |
| tblEmpGeneralInfo | Email | @Email |
| tblEmpGeneralInfo | EmpImage | @EmpImage |
| tblEmpGeneralInfo | EmpInfoId | @EmpInfoId |
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
| (handler Clear) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ----Select---- | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input  Employee Short Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Blood Group!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Date Of Birth!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Department!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Designation!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Email!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Employee Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Father Name!! | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Data In All TextBox! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ImageBytes, SigImageBytes
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 12 methods.
- Redirect targets: EmpGenInfView.aspx
- User messages: ----Select----; Please Input  Employee Short Name!!; Please Input Blood Group!!; Please Input Data In All TextBox!; Please Input Date Of Birth!!; Please Input Department!!
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
- `Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx`, `Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 50 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/EmpGeneralInfoEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods EmpGeneralInfoDAL.EmployeeParameters, EmpGeneralInfoDAL.LoadDepartmentName, EmpGeneralInfoDAL.LoadDesignationName, EmpGeneralInfoDAL.SaveEmployeeInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
