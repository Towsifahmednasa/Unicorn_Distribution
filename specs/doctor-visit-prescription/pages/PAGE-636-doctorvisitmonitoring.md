---
page_id: PAGE-636
module: M16
page: DoctorVisitMonitoring
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-636 — DoctorVisitMonitoring

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-636 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | DoctorVisitMonitoring |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Dashboard_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Dashboard_UI_DoctorVisitMonitoring |
| aspx size | 15553 bytes |
| Code-behind LOC | 284 |
| Inline script size | 2975 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DoctorVisitMonitoring** (Report / Dashboard). Page title/heading text: `Details`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/Dashboard_UI/DoctorVisitMonitoring.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx |
| Code-behind | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/SInventory_UI/IVMarketSTForZone.ascx |
| DAL class | DoctorMonitoringDAL (Library.DAL/Doctor_Monitoring_DAL/DoctorMonitoringDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→EmpCetegoryAddImageButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| FromDate | TextBox | Select Date | no validator found |  |  |  |  |
| ToDate | TextBox | Select Date | no validator found |  |  |  |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | OnClick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnRowDataBound→loadGridView_RowDataBound, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfEmpInfoId | HiddenField |  | no validator found |  |  |  |  |
| hfRoleType | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | ID | no validator found |  |  |  |  |
| (no id) | TemplateField | Territory | no validator found |  |  |  |  |
| lblTerritory | Label | <%#Eval("Territory")%> | no validator found |  |  |  |  |
| (no id) | BoundField | Name Of the Employee | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | TemplateField | Base HQ | no validator found |  |  |  |  |
| hfTerritoryCode | Label | <%#Eval("TerritoryCode")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | DCP | no validator found |  |  |  |  |
| lnkDCP | LinkButton | <%#Eval("DCP")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | DCR | no validator found |  |  |  |  |
| lnkDCR | LinkButton | <%#Eval("DCR")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | RX | no validator found |  |  |  |  |
| lnkRX | LinkButton | <%#Eval("RX")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Coverage | no validator found |  |  |  |  |
| lnkCustomerCoveragenn | LinkButton | <%#Eval("CustomerCoverageNew")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Visit Plan (CVP) | no validator found |  |  |  |  |
| lnkCustomerCoverage | LinkButton | <%#Eval("CustomerCoverage")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Customer Visit Report (CVR) | no validator found |  |  |  |  |
| lnkCustomerCoverage | LinkButton | <%#Eval("CVRCount")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | GP Sales (Net TP) | no validator found |  |  |  |  |
| lnkGPSales | LinkButton | <%#Eval("GPSales")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Total Sales (Net TP) | no validator found |  |  |  |  |
| lnkTSales | LinkButton | <%#Eval("TSalesNetTP")%> | no validator found |  |  |  |  |
| hndModalTrigger | HiddenField |  | no validator found |  |  |  |  |
| btnClosePopup | Button | X | no validator found |  |  |  | OnClick→btnClosePopup_Click |
| lblModalContent | Label | Details will load here... | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton | — | EmpCetegoryAddImageButton_Click | — | — | — | — | UNKNOWN (handler not resolved) |
| btnSearch | LinkButton click | (postback) | Dashboard_UI_DoctorVisitMonitoring.btnSearch_Click | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:265 | — | DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList | sp_Get_AllDoctorVisitMonitoringApprovalList | SP executed — effect on tables UNKNOWN (body not in repo) |
| resetBtn | LinkButton click | (postback) | Dashboard_UI_DoctorVisitMonitoring.resetBtn_Click | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:269 | — | — | — | read-only / none detected |
| btnClosePopup | Button click | (postback) | Dashboard_UI_DoctorVisitMonitoring.btnClosePopup_Click | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:170 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: yes; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Dashboard_UI_DoctorVisitMonitoring.Page_Load | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:26 | object sender, EventArgs e | Dashboard_UI_DoctorVisitMonitoring.Page_Load → SeedDataDAL.GetApprovalStatusList → SP: sp_GET_ActionStatusList | yes | no |
| Dashboard_UI_DoctorVisitMonitoring.loadGridView_RowDataBound | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:56 | object sender, GridViewRowEventArgs e | Dashboard_UI_DoctorVisitMonitoring.loadGridView_RowDataBound | no | no |
| Dashboard_UI_DoctorVisitMonitoring.LoadInitialInfo | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:76 |  | Dashboard_UI_DoctorVisitMonitoring.LoadInitialInfo → SeedDataDAL.GetApprovalStatusList → SP: sp_GET_ActionStatusList | yes | no |
| Dashboard_UI_DoctorVisitMonitoring.LoadData | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:115 |  | Dashboard_UI_DoctorVisitMonitoring.LoadData → DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList → SP: sp_Get_AllDoctorVisitMonitoringApprovalList | no | no |
| Dashboard_UI_DoctorVisitMonitoring.btnClosePopup_Click | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:170 | object sender, EventArgs e | Dashboard_UI_DoctorVisitMonitoring.btnClosePopup_Click | no | no |
| Dashboard_UI_DoctorVisitMonitoring.loadGridView_RowCommand | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:178 | object sender, GridViewCommandEventArgs e | Dashboard_UI_DoctorVisitMonitoring.loadGridView_RowCommand | no | no |
| Dashboard_UI_DoctorVisitMonitoring.btnSearch_Click | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:265 | object sender, EventArgs e | Dashboard_UI_DoctorVisitMonitoring.btnSearch_Click → DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList → SP: sp_Get_AllDoctorVisitMonitoringApprovalList | no | no |
| Dashboard_UI_DoctorVisitMonitoring.resetBtn_Click | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:269 | object sender, EventArgs e | Dashboard_UI_DoctorVisitMonitoring.resetBtn_Click | no | no |
| Dashboard_UI_DoctorVisitMonitoring.gv_DocumentUpload_PreRender | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs:274 | object sender, EventArgs e | Dashboard_UI_DoctorVisitMonitoring.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList | Library.DAL/Doctor_Monitoring_DAL/DoctorMonitoringDAL.cs:16 | sp_Get_AllDoctorVisitMonitoringApprovalList | @AreaId, @GroupId, @TerrId, @ZonId, @frmDate, @param, @toDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AllDoctorVisitMonitoringApprovalList | DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler GetDataByType) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Invalid type. | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 17 methods.
- Redirect targets: DoctorVisitMonitoring.aspx
- User messages: Invalid type.; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: DoctorMonitoringDAL, SeedDataDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnSearch with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSearch with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click resetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click resetBtn with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnClosePopup with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnClosePopup with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx`, `Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 45 controls parsed | n/a | PARTIAL | Solution.Web/Dashboard_UI/DoctorVisitMonitoring.aspx |
| Data access | This document §15–17 | 2 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: PAGE-167, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-463, PAGE-548, PAGE-549, PAGE-562, PAGE-563, PAGE-564, PAGE-571, PAGE-582 …
- Changing DAL methods DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList, SeedDataDAL.GetApprovalStatusList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
