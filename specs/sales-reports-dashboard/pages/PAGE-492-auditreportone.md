---
page_id: PAGE-492
module: M13
page: AuditReportOne
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-492 — AuditReportOne

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-492 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | AuditReportOne |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_AuditReportOne |
| aspx size | 17827 bytes |
| Code-behind LOC | 409 |
| Inline script size | 751 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **AuditReportOne** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `auditreportonereportviewer.aspx`, `deleteorderreportviewer.aspx`

## 5. Route
`~/SInventory_UI/AuditReportOne.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/AuditReportOne.aspx |
| Code-behind | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | AuditReportOneBll (Library.BLL/SInventory_BLL/AuditReportOneBll.cs) |
| DAL class | AuditReportOneDal (Library.DAL/SInventory_DAL/AuditReportOneDal.cs) |
| DAL class | OrderTrackingDAL (Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| nationalCheckBox | CheckBox |  | no validator found |  |  |  | oncheckedchanged→nationalCheckBox_OnCheckedChanged |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox | Select From Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select  To Date | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| excelButton | Button | Excel | no validator found |  | Visible=False |  | OnClick→excelButton_OnClick |
| viewRptButton | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_Click1 |
| lblOrderCount | Label | 0 | no validator found |  |  |  |  |
| lblOrderAmount | Label | 0 | no validator found |  |  |  |  |
| lblVAT | Label | 0 | no validator found |  |  |  |  |
| lblDiscount | Label | 0 | no validator found |  |  |  |  |
| lblChemistCount | Label | 0 | no validator found |  |  |  |  |
| lblAllTotal | Label | 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Order NO | no validator found |  |  |  |  |
| fff | LinkButton | <%#Eval("OrderCode") %> | no validator found |  |  |  |  |
| (no id) | BoundField | Order NO | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Center | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Payable | no validator found |  |  |  |  |
| (no id) | BoundField | Create Date | no validator found |  |  |  |  |
| (no id) | BoundField | Create By | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Name | no validator found |  |  |  |  |
| (no id) | BoundField | AM Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Group | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Route | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | BoundField | Delete By | no validator found |  |  |  |  |
| (no id) | BoundField | Delete Date | no validator found |  |  |  |  |
| (no id) | TemplateField | View | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| comUnitNameDropDownList | parameter | SInventory_UI_AuditReportOne.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | parameter | SInventory_UI_AuditReportOne.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitNameDropDownList | param | SInventory_UI_AuditReportOne.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | param | SInventory_UI_AuditReportOne.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| comUnitNameDropDownList | LoadDropdownlist → AuditReportOneBll.LoadSalesCenter | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| excelButton | Button click | (postback) | SInventory_UI_AuditReportOne.excelButton_OnClick | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:179 | — | — | — | read-only / none detected |
| viewRptButton | LinkButton click | (postback) | SInventory_UI_AuditReportOne.submitButton_Click1 | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:128 | — | — | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_AuditReportOne.Unnamed_Click | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:221 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_AuditReportOne.btnExport_Click | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:264 | — | — | — | read-only / none detected |

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
| SInventory_UI_AuditReportOne.Page_Load | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_AuditReportOne.Page_Load → AuditReportOneBll.LoadSalesCenter → AuditReportOneDal.GetSalesCenter | no | no |
| SInventory_UI_AuditReportOne.LoadDropdownlist | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:26 |  | SInventory_UI_AuditReportOne.LoadDropdownlist → AuditReportOneBll.LoadSalesCenter → AuditReportOneDal.GetSalesCenter | no | no |
| SInventory_UI_AuditReportOne.cancelButton_OnClick | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:108 | object sender, EventArgs e | SInventory_UI_AuditReportOne.cancelButton_OnClick | no | no |
| SInventory_UI_AuditReportOne.submitButton_Click1 | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:128 | object sender, EventArgs e | SInventory_UI_AuditReportOne.submitButton_Click1 | no | no |
| SInventory_UI_AuditReportOne.nationalCheckBox_OnCheckedChanged | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:166 | object sender, EventArgs e | SInventory_UI_AuditReportOne.nationalCheckBox_OnCheckedChanged | no | no |
| SInventory_UI_AuditReportOne.excelButton_OnClick | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:179 | object sender, EventArgs e | SInventory_UI_AuditReportOne.excelButton_OnClick | no | no |
| SInventory_UI_AuditReportOne.fromDateTextBox_OnTextChanged | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:208 | object sender, EventArgs e | SInventory_UI_AuditReportOne.fromDateTextBox_OnTextChanged | no | no |
| SInventory_UI_AuditReportOne.toDateTextBox_OnTextChanged | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:214 | object sender, EventArgs e | SInventory_UI_AuditReportOne.toDateTextBox_OnTextChanged | no | no |
| SInventory_UI_AuditReportOne.Unnamed_Click | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:221 | object sender, EventArgs e | SInventory_UI_AuditReportOne.Unnamed_Click | no | no |
| SInventory_UI_AuditReportOne.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:258 | object sender, GridViewPageEventArgs e | SInventory_UI_AuditReportOne.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_AuditReportOne.btnExport_Click | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:264 | object sender, EventArgs e | SInventory_UI_AuditReportOne.btnExport_Click | no | no |
| SInventory_UI_AuditReportOne.LoadData | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:347 | string parm | SInventory_UI_AuditReportOne.LoadData → OrderTrackingDAL.GetOrderDelTrackingList → SP: sp_Get_OrderDelTrackingList | yes | no |
| SInventory_UI_AuditReportOne.btnSearch_Click | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:395 | object sender, EventArgs e | SInventory_UI_AuditReportOne.btnSearch_Click → OrderTrackingDAL.GetOrderDelTrackingList → SP: sp_Get_OrderDelTrackingList | no | no |
| SInventory_UI_AuditReportOne.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/AuditReportOne.aspx.cs:399 | object sender, EventArgs e | SInventory_UI_AuditReportOne.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| AuditReportOneBll.LoadSalesCenter | Library.BLL/SInventory_BLL/AuditReportOneBll.cs:16 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| AuditReportOneDal.GetSalesCenter | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:16 | inline SQL: tblCompanyUnit |  |
| OrderTrackingDAL.GetOrderDelTrackingList | Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:541 | sp_Get_OrderDelTrackingList | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_OrderDelTrackingList | OrderTrackingDAL.GetOrderDelTrackingList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler DateValidationCheck) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select valid date!!! | UNKNOWN |
| (handler submitButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select national report or a sales center!!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select from date | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select to date | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: Excel, parameter
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 24 methods.
- Redirect targets: AuditReportOne.aspx
- User messages: Please select from date; Please select national report or a sales center!!!; Please select to date; Please select valid date!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: AuditReportOneBll; DAL: AuditReportOneDal, OrderTrackingDAL; DAO: —; Tables (inline): tblCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click excelButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click excelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click viewRptButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click viewRptButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/AuditReportOne.aspx`, `Solution.Web/SInventory_UI/AuditReportOne.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/AuditReportOne.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 59 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/AuditReportOne.aspx |
| Data access | This document §15–17 | 1 SP names, 1 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M13 (Sales Reports, Analytics & Dashboards).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods AuditReportOneDal.GetSalesCenter, OrderTrackingDAL.GetOrderDelTrackingList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
