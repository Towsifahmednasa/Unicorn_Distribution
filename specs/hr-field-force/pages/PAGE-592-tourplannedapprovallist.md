---
page_id: PAGE-592
module: M15
page: TourPlannedApprovalList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-592 — TourPlannedApprovalList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-592 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | TourPlannedApprovalList |
| Kind | Approval (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_TourPlannedApprovalList |
| aspx size | 13040 bytes |
| Code-behind LOC | 135 |
| Inline script size | 5274 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TourPlannedApprovalList** (Approval). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerentry.aspx`, `tourplandetailsview.aspx`

## 5. Route
`~/DoctorModule_UI/TourPlannedApprovalList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | TourTypeDal (Library.DAL/DoctorModule_DAL/TourTypeDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| chkAction | RadioButtonList |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfCustomerMasterId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Employee ID | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Designation | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | BoundField | Year | no validator found |  |  |  |  |
| (no id) | BoundField | Month | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | TemplateField | View | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| chkAction | rbValue | DoctorModule_UI_TourPlannedApprovalList.btnSave_Click | (none — page calls DAL directly) | TourTypeDal.Approve_TourPlanList | @Status | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfCustomerMasterId | Res | DoctorModule_UI_TourPlannedApprovalList.btnSave_Click | — | TourTypeDal.Approve_TourPlanList | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| chkAction | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_TourPlannedApprovalList.btnSave_Click | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:79 | — | TourTypeDal.Approve_TourPlanList | sp_ApproveTourPlanInformation | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- Client script functions in page: GetPrescription, IsActiveChange, SaveApproval, editClick, validation
- jQuery/DOM bindings: #CheckAll:click
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_TourPlannedApprovalList.Page_Load | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:16 | object sender, EventArgs e | DoctorModule_UI_TourPlannedApprovalList.Page_Load → TourTypeDal.GetTourPlanList → SP: sp_Get_TourPlanMasteList | no | no |
| DoctorModule_UI_TourPlannedApprovalList.chkSelectAll_CheckedChanged | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:24 | object sender, EventArgs e | DoctorModule_UI_TourPlannedApprovalList.chkSelectAll_CheckedChanged | no | no |
| DoctorModule_UI_TourPlannedApprovalList.gv_DocumentUpload_PreRender | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:34 | object sender, EventArgs e | DoctorModule_UI_TourPlannedApprovalList.gv_DocumentUpload_PreRender | no | no |
| DoctorModule_UI_TourPlannedApprovalList.EmpCetegoryAddImageButton_Click | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:46 | object sender, EventArgs e | DoctorModule_UI_TourPlannedApprovalList.EmpCetegoryAddImageButton_Click | no | no |
| DoctorModule_UI_TourPlannedApprovalList.LoadData | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:51 |  | DoctorModule_UI_TourPlannedApprovalList.LoadData → TourTypeDal.GetTourPlanList → SP: sp_Get_TourPlanMasteList | no | no |
| DoctorModule_UI_TourPlannedApprovalList.btnSave_Click | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:79 | object sender, EventArgs e | DoctorModule_UI_TourPlannedApprovalList.btnSave_Click → TourTypeDal.Approve_TourPlanList → SP: sp_ApproveTourPlanInformation | no | yes |
| DoctorModule_UI_TourPlannedApprovalList.loadGridView_RowCommand | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs:122 | object sender, GridViewCommandEventArgs e | DoctorModule_UI_TourPlannedApprovalList.loadGridView_RowCommand | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| TourTypeDal.GetTourPlanList | Library.DAL/DoctorModule_DAL/TourTypeDal.cs:117 | sp_Get_TourPlanMasteList | @param |
| TourTypeDal.Approve_TourPlanList | Library.DAL/DoctorModule_DAL/TourTypeDal.cs:350 | sp_ApproveTourPlanInformation | @ApprovedBy, @Status, @TPMaster |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_ApproveTourPlanInformation | TourTypeDal.Approve_TourPlanList | body NOT in repository | write-like name |
| sp_Get_TourPlanMasteList | TourTypeDal.GetTourPlanList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TourPlannedApprovalList.aspx'); | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: CustomerEntry.aspx
- User messages: ','Success','TourPlannedApprovalList.aspx');; please fill out this field
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: TourTypeDal; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx`, `Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 25 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/TourPlannedApprovalList.aspx |
| Data access | This document §15–17 | 2 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-593, PAGE-594, PAGE-679
- Changing DAL methods TourTypeDal.Approve_TourPlanList, TourTypeDal.GetTourPlanList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
