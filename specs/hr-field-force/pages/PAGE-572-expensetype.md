---
page_id: PAGE-572
module: M15
page: ExpenseType
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-572 — ExpenseType

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-572 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | ExpenseType |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_ExpenseType |
| aspx size | 15747 bytes |
| Code-behind LOC | 596 |
| Inline script size | 882 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: master screen for **expense types** used by expense claims (PAGE-570): a type has a name, an optional amount (only editable when `Fixed` is ticked), whether a receipt image is required, an Active flag, the **role types and/or individual employees allowed to claim it**, and a list of **custom detail fields** (field name + required Yes/No) that the claim form then asks for. List page: `ExpenseTypeView.aspx` (PAGE-573).

## 3. Business Objective
CONFIRMED by code: HR defines which categories of field expense exist, who may claim them, whether a fixed reimbursement amount applies and what supporting data (fields, image) each claim must carry.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/ExpenseTypeView.aspx`

Outbound references: `expensetypeview.aspx`

## 5. Route
`~/DoctorModule_UI/ExpenseType.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/ExpenseType.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | SetupDAL (Library.DAL/DoctorModule_DAL/SetupDAL.cs) |
| DAO/entity class | ExpenseTypeDetails (Library.DAO/DoctorModule_DAO/ExpenseTypeDetails.cs) |
| DAO/entity class | ExpenseTypeMaster (Library.DAO/DoctorModule_DAO/ExpenseTypeMaster.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| TxtName | TextBox |  | no validator found |  |  |  |  |
| ddlRoleType | ListBox |  | no validator found |  |  |  |  |
| EmployeeIdSelect | ListBox |  | no validator found |  |  |  |  |
| chkFixed | CheckBox | is Fixed Amount | no validator found |  |  |  | OnCheckedChanged→chkFixed_CheckedChanged |
| txtAmount | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| rbImgType | RadioButtonList |  | no validator found |  |  |  |  |
| txtFieldName | TextBox |  | no validator found |  |  |  |  |
| IsRequired | DropDownList |  | no validator found |  |  | see §9 |  |
| hfForMe | HiddenField |  | no validator found |  |  |  |  |
| addButtonDA | LinkButton |  | no validator found |  |  |  | OnClick→addButtonDA_Click |
| gv_DA | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfExpenseTypDetailsId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Field Name | no validator found |  |  |  |  |
| lbl_FieldName | Label | <%# Eval("FieldName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Required | no validator found |  |  |  |  |
| lbl_IsRequied | Label | <%# Eval("IsRequied") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| deleteImageButton | LinkButton |  | no validator found |  |  |  | OnClick→deleteImageButton_Click |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| customSwitch1 | input |  | no validator found |  |  |  | onchange→IsActiveChange() |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| txtFieldName | newFieldName | DoctorModule_UI_ExpenseType.Add | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| hfExpenseTypDetailsId | dt | DoctorModule_UI_ExpenseType.Remove | — | SetupDAL.checkFroDelete, SetupDAL.delExpensDetls | UNKNOWN | UNKNOWN | UNKNOWN |
| hfExpenseTypDetailsId | Res | DoctorModule_UI_ExpenseType.Remove | — | SetupDAL.checkFroDelete, SetupDAL.delExpensDetls | UNKNOWN | UNKNOWN | UNKNOWN |
| hfExpenseTypDetailsId | ExpenseTypeDetails.ExpenseTypDetailsId | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @ExpenseTypDetailsId | UNKNOWN (SP body not in repo) | UNKNOWN |
| lbl_FieldName | ExpenseTypeDetails.FieldName | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @FieldName | UNKNOWN (SP body not in repo) | UNKNOWN |
| lbl_IsRequied | ExpenseTypeDetails.IsRequied | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @IsRequied | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | ExpenseTypeDetails.ExpenseTypeId | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @ExpenseTypeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| TxtName | ExpenseTypeMaster.ExpenseTypeName | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @ExpenseTypeName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | ExpenseTypeMaster.ExpenseAmount | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @ExpenseAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| customSwitch1 | ExpenseTypeMaster.IsActive | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @IsActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkFixed | ExpenseTypeMaster.isFixed | DoctorModule_UI_ExpenseType.btnSave_Click | (none — page calls DAL directly) | SetupDAL.SaveExpenseTypeMaster | @isFixed | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlRoleType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbImgType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| IsRequired | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| addButtonDA | LinkButton click | (postback) | DoctorModule_UI_ExpenseType.addButtonDA_Click | Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs:207 | — | — | — | read-only / none detected |
| deleteImageButton | LinkButton click | (postback) | DoctorModule_UI_ExpenseType.deleteImageButton_Click | Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs:285 | — | — | — | read-only / none detected |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_ExpenseType.btnSave_Click | Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs:461 | — | SetupDAL.SaveExpenseTypeMaster | sp_Save_ExpenseTypeDetails, sp_Save_ExpenseTypeMaster, sp_Update_ExpenseType, sp_check_ExpenseType, sp_check_Vali_Market | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_ExpenseType.btnSave_Click | Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs:461 | — | SetupDAL.SaveExpenseTypeMaster | sp_Save_ExpenseTypeDetails, sp_Save_ExpenseTypeMaster, sp_Update_ExpenseType, sp_check_ExpenseType, sp_check_Vali_Market | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | DoctorModule_UI_ExpenseType.btnReset_Click | Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs:583 | — | — | — | read-only / none detected |

## 11. Frontend Flow
`Page_Load` (`ExpenseType.aspx.cs:25-88`): fills role types (`GetRoleTypeInfoDALFFS`) and active field-staff employees (`GetEmployeeList_ActiveFS`) as multi-selects, empty detail grid; `?id=` -> `GetOneRecord` (`:90-205`, `SetupDAL.GetEditDataForId`, `Get_ExpenseTypeDetailsByExpenseId`, fills fixed flag, name, amount, image flag, active, role/employee CSVs). Detail rows: `addButtonDA_Click` (`:206`) -> `Add()` (`:236`): rebuilds a `ViewState["CurrentTable"]` from the grid, adds a row unless the same field name (exact match) exists. `chkFixed_CheckedChanged` (`:587`) clears the amount and makes it read-only unless `Fixed` ticked. Save -> `btnSave_Click` (`:460`); success -> `ExpenseTypeView.aspx`.

## 12. Backend Flow
`btnSave_Click` -> `Validation()` -> `ExpenseTypeMaster` (`RoleType_xp=0`, `RoleTypeMult` and `EmpNameMult` = CSVs of selected ids, `ImageRequired` = radio Yes, `isFixed`, `IsActive`, `ExpenseAmount` decimal or null) + list of `ExpenseTypeDetails` -> `SetupDAL.SaveExpenseTypeMaster` (`Library.DAL/DoctorModule_DAL/SetupDAL.cs:4281`): id>0 -> `sp_check_ExpenseType(@ExpenseTypeId,@ExpenseTypeName)` rows -> `isDuplicateCheck`; else if `IsActive==false` -> `sp_check_Vali_MarketStructure(@MasterId,@PageName='ExpenseTypeName')` rows -> `isValiCheck` (in use, cannot deactivate) else `sp_Update_ExpenseType(@UpdateBy)`; id==0 -> `sp_Save_ExpenseTypeMaster(@EntryBy)`. Then each detail row -> `sp_Save_ExpenseTypeDetails(@ExpenseTypDetailsId,@ExpenseTypeId,@FieldName,@IsRequied)` (**always the save SP, also for existing ids** - the update SP call is commented out). Transaction implicit, committed at close. Messages: success; `Already Exist!` (duplicate/other); `Data cannot be deactivated!`.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetRoleTypeInfoDALFFS | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:356 | sp_CS_RoleTypeFSS |  |
| CommonDataLoad.GetEmployeeList_ActiveFS | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:484 | sp_CS_GetEmpGeneralInfo_AllFSS |  |
| SetupDAL.GetEditDataForId | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5130 | sp_Get_ExpenseTypeData_ByExpenseTypeId | @id |
| SetupDAL.Get_ExpenseTypeDetailsByExpenseId | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5199 | sp_Get_ExpenseDetails_ByExpenseId | @id |
| SetupDAL.checkFroDelete | Library.DAL/DoctorModule_DAL/SetupDAL.cs:5174 | sp_check_Vali_MarketStructure | @MasterId, @PageName |
| SetupDAL.delExpensDetls | Library.DAL/DoctorModule_DAL/SetupDAL.cs:4253 | sp_ExpenseTypeDtl | @ExpenseTypeId |
| SetupDAL.SaveExpenseTypeMaster | Library.DAL/DoctorModule_DAL/SetupDAL.cs:4282 | sp_Save_ExpenseTypeDetails, sp_Save_ExpenseTypeMaster, sp_Update_ExpenseType, sp | @EmpNameMult, @EntryBy, @ExpenseAmount, @ExpenseTypDetailsId, @ExpenseTypeId, @ExpenseTypeName, @Fie |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_AllFSS | CommonDataLoad.GetEmployeeList_ActiveFS | body NOT in repository | read-like name (INFERRED) |
| sp_CS_RoleTypeFSS | CommonDataLoad.GetRoleTypeInfoDALFFS | body NOT in repository | read-like name (INFERRED) |
| sp_ExpenseTypeDtl | SetupDAL.delExpensDetls | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpenseDetails_ByExpenseId | SetupDAL.Get_ExpenseTypeDetailsByExpenseId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpenseTypeData_ByExpenseTypeId | SetupDAL.GetEditDataForId | body NOT in repository | read-like name (INFERRED) |
| sp_Save_ExpenseTypeDetails | SetupDAL.SaveExpenseTypeMaster | body NOT in repository | write-like name |
| sp_Save_ExpenseTypeMaster | SetupDAL.SaveExpenseTypeMaster | body NOT in repository | write-like name |
| sp_Update_ExpenseType | SetupDAL.SaveExpenseTypeMaster | body NOT in repository | write-like name |
| sp_check_ExpenseType | SetupDAL.SaveExpenseTypeMaster | body NOT in repository | read-like name (INFERRED) |
| sp_check_Vali_MarketStructure | SetupDAL.SaveExpenseTypeMaster, SetupDAL.checkFroDelete | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| TxtName | required | `Validation()` `:379` | tooltip | BR-M15-046 |
| ddlRoleType / EmployeeIdSelect | at least one role type **or** employee | `:379-440` | tooltip on both | BR-M15-046 |
| detail grid | >= 1 row | `:442` | `please Add to List One Row` | BR-M15-046 |
| txtFieldName (add) | required | `addButtonDA_Click` | tooltip | BR-M15-049 |
| field name | unique in grid (case-sensitive) | `Add()` | silently ignored | BR-M15-049 |
| txtAmount | decimal parse (`decimal.Parse`), read-only unless Fixed | `btnSave_Click` | unhandled FormatException on bad text | BR-M15-047 |
| duplicate name / deactivate-in-use | SP checks | `SetupDAL.cs:4300-4335` | `Already Exist!` / `Data cannot be deactivated!` | BR-M15-048 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-046 | An expense type needs a name, at least one allowed role type or employee, and at least one detail field. | `ExpenseType.aspx.cs:379-445` | code-behind | CONFIRMED |
| BR-M15-047 | Amount is only enterable when `Fixed` is ticked (otherwise cleared/read-only and saved as NULL); image requirement is Yes/No. Code does not enforce the fixed amount on claims (PAGE-570 amount is free-typed). | `ExpenseType.aspx.cs:587-595,460-520` | code-behind | CONFIRMED |
| BR-M15-048 | Renaming to an existing name is rejected (`sp_check_ExpenseType`); an expense type in use cannot be deactivated (`sp_check_Vali_MarketStructure` with `PageName='ExpenseTypeName'`). SP predicates UNKNOWN. | `SetupDAL.cs:4300-4335` | DAL + SP | CONFIRMED (flow), UNKNOWN (SP) |
| BR-M15-049 | Detail field names must be unique within the type as typed (exact string compare); each has a Required flag persisted as `IsRequied` (sic). | `ExpenseType.aspx.cs:236-270`; `SetupDAL.cs:4370-4392` | code-behind + DAL | CONFIRMED |

## 20. Permission Matrix
Session: `UserId` only. No role check; menu-only. Employee/role restrictions are stored data (who may claim), enforced in the claim form/SPs (UNKNOWN).

## 21. Status Lifecycle
`IsActive` (`customSwitch1`) only; deactivation guarded by BR-M15-048.

## 22. Error Handling
Lookup loads swallow exceptions; DAL rollback + rethrow; single generic `Already Exist!` for unspecific failure.

## 23. Audit
`@EntryBy` / `@UpdateBy` = `Session["UserId"]` on master; detail rows no audit param.

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, SetupDAL; DAO: ExpenseTypeDetails, ExpenseTypeMaster, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-572-01 | Name, one role type, one field row, Save | Created; redirect to list | BR-M15-046 |
| TC-572-02 | No role type and no employee | Both boxes flagged | BR-M15-046 |
| TC-572-03 | Add field `Km` twice | Second ignored | BR-M15-049 |
| TC-572-04 | Untick Fixed with amount typed | Amount cleared | BR-M15-047 |
| TC-572-05 | Deactivate a type used in claims | `Data cannot be deactivated!` | BR-M15-048 |
| TC-572-06 | Edit existing type and Save (edge) | Detail rows re-sent with existing ids -> check duplicate detail rows | GAP-M15-017 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/ExpenseType.aspx`, `Solution.Web/DoctorModule_UI/ExpenseType.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | ExpenseType.aspx(.cs) | n/a | MISSING | - |
| Fixed amount enforcement | None existed | not enforced at claim | UNKNOWN | ACTUAL-GAP | GAP-M15-018 |
| Detail update | None existed | update SP call commented out | UNKNOWN | ACTUAL-DEFECT | GAP-M15-017 |

## 30. Known Gaps
- **GAP-M15-017** Detail rows always re-saved with save SP; unused `Remove()` (uses `checkFroDelete`/`delExpensDetls`); `decimal.Parse` unguarded; `RoleType_xp` legacy constant 0.

## 31. Change Impact
Feeds PAGE-570/571 (expense claim form and list, `GetExpenseType`, `GetExpenseField_ByExpenseType`) and mobile claims; `SetupDAL` shared with Transport/TourPurpose pages.

