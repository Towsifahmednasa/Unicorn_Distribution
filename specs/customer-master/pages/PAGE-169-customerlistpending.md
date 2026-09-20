---
page_id: PAGE-169
module: M05
page: CustomerListPending
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-169 — CustomerListPending

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-169 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerListPending |
| Kind | List / View (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_CustomerListPending |
| aspx size | 24849 bytes |
| Code-behind LOC | 352 |
| Inline script size | 2952 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerListPending** (List / View). Page title/heading text: `Customer List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerentry.aspx`, `customerview.aspx`

## 5. Route
`~/MasterSetup_UI/CustomerListPending.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/CustomerListPending.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStructureSearch.ascx |
| DAL class | CustomerInfoDAL (Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→EmpCetegoryAddImageButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| ddlStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlDistributionCenter | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProgramType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlChemisType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlApprovalStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| frmDate | TextBox | Select Date | no validator found |  |  |  |  |
| toDate | TextBox | Select Date | no validator found |  |  |  |  |
| custNameTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→custNameTextBox_TextChanged |
| at_txt_JobCirculation | AutoCompleteExtender |  | no validator found |  |  |  |  |
| hfCustomerId | HiddenField |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| lblCount | Label | Total : 0 | no validator found |  |  |  |  |
| btnExportToExcel | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Provider Type | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Mobile NO | no validator found |  |  |  |  |
| (no id) | BoundField | Address | no validator found |  |  |  |  |
| (no id) | BoundField | Group | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Zone | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Area | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Territory | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Market | no validator found |  | Visible=False |  |  |
| (no id) | BoundField | Distribution RouteName | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  | Visible=false |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| custNameTextBox | empName | MasterSetup_UI_CustomerListPending.custNameTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlDistributionCenter | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProgramType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlChemisType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlApprovalStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | MasterSetup_UI_CustomerListPending.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:153 | — | — | — | read-only / none detected |
| btnExportToExcel | LinkButton click | (postback) | MasterSetup_UI_CustomerListPending.btnExportToExcel_Click | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:277 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_CustomerListPending.Page_Load | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:25 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.Page_Load → CustomerInfoDAL.GetCustomerList / SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetChemistTypeList / SeedDataDAL.GetDistributionCenterDataTableList / SeedDataDAL.GetProgramTypeList → SP: sp_GET_ActionStatusList, | yes | no |
| MasterSetup_UI_CustomerListPending.custNameTextBox_TextChanged | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:112 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.custNameTextBox_TextChanged | no | no |
| MasterSetup_UI_CustomerListPending.gv_DocumentUpload_PreRender | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:142 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.gv_DocumentUpload_PreRender | no | no |
| MasterSetup_UI_CustomerListPending.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:153 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.EmpCetegoryAddImageButton_Click | no | no |
| MasterSetup_UI_CustomerListPending.LoadData | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:158 | string parm | MasterSetup_UI_CustomerListPending.LoadData → CustomerInfoDAL.GetCustomerList → SP: sp_Get_CustMasterList_Approve | no | no |
| MasterSetup_UI_CustomerListPending.loadGridView_RowCommand | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:174 | object sender, GridViewCommandEventArgs e | MasterSetup_UI_CustomerListPending.loadGridView_RowCommand | no | no |
| MasterSetup_UI_CustomerListPending.btnSearch_Click | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:187 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.btnSearch_Click → CustomerInfoDAL.GetCustomerList → SP: sp_Get_CustMasterList_Approve | no | no |
| MasterSetup_UI_CustomerListPending.loadGridView_PageIndexChanging | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:262 | object sender, GridViewPageEventArgs e | MasterSetup_UI_CustomerListPending.loadGridView_PageIndexChanging | no | no |
| MasterSetup_UI_CustomerListPending.resetBtn_Click | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:267 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.resetBtn_Click | no | no |
| MasterSetup_UI_CustomerListPending.btnExportToExcel_Click | Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs:277 | object sender, EventArgs e | MasterSetup_UI_CustomerListPending.btnExportToExcel_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetChemistTypeList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:610 | sp_GET_CustomerType |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| SeedDataDAL.GetProgramTypeList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:909 | sp_GET_ProgramTypeList |  |
| SeedDataDAL.GetDistributionCenterDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:669 | sp_GET_DistributionCenter |  |
| CustomerInfoDAL.GetCustomerList | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:406 | sp_Get_CustMasterList_Approve | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CustomerType | SeedDataDAL.GetChemistTypeList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DistributionCenter | SeedDataDAL.GetDistributionCenterDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramTypeList | SeedDataDAL.GetProgramTypeList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CustMasterList_Approve | CustomerInfoDAL.GetCustomerList | LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:820 | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler custNameTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Input Correct Data ! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DICCompanyUnitId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 13 methods.
- Redirect targets: CustomerEntry.aspx, CustomerEntry.aspx?MID=, CustomerView.aspx
- User messages: Input Correct Data !; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CustomerInfoDAL, SeedDataDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnExportToExcel with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnExportToExcel with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/CustomerListPending.aspx`, `Solution.Web/MasterSetup_UI/CustomerListPending.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/CustomerListPending.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 44 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/CustomerListPending.aspx |
| Data access | This document §15–17 | 5 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 5 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: PAGE-015, PAGE-016, PAGE-111, PAGE-112, PAGE-113, PAGE-114, PAGE-115, PAGE-117, PAGE-147, PAGE-167, PAGE-168, PAGE-173, PAGE-207, PAGE-208, PAGE-209 …
- Changing DAL methods CustomerInfoDAL.GetCustomerList, SeedDataDAL.GetApprovalStatusList, SeedDataDAL.GetChemistTypeList, SeedDataDAL.GetDistributionCenterDataTableList, SeedDataDAL.GetProgramTypeList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
