---
page_id: PAGE-359
module: M10
page: BatchLifecycleReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-359 — BatchLifecycleReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-359 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | BatchLifecycleReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_BatchLifecycleReport |
| aspx size | 20302 bytes |
| Code-behind LOC | 377 |
| Inline script size | 993 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **BatchLifecycleReport** (Report / Dashboard). Page title/heading text: `Batch Movement & Traceability Ledger`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `login.aspx`

## 5. Route
`~/SInventory_UI/BatchLifecycleReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DCStockReportBLL (Library.BLL/SInventory_BLL/DCStockReportBLL.cs) |
| DAL class | DCStockReportDAL (Library.DAL/SInventory_DAL/DCStockReportDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→dcDropDownList_SelectedIndexChanged |
| fromDateTextBox | TextBox | Select From Date | no validator found |  |  |  |  |
| productDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→productDropDownList_SelectedIndexChanged |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| batchDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| txnTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| resetButton | LinkButton |  | no validator found |  |  |  | OnClick→resetButton_Click |
| exportButton | LinkButton |  | no validator found |  |  |  | OnClick→exportButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowDataBound→loadGridView_RowDataBound, OnPreRender→loadGridView_PreRender |
| (no id) | TemplateField | SL No | no validator found |  |  |  |  |
| (no id) | TemplateField | Txn ID | no validator found |  |  |  |  |
| (no id) | BoundField | Posting Date & Time | no validator found |  |  |  |  |
| (no id) | TemplateField | Transaction Type | no validator found |  |  |  |  |
| lblType | Label | <%# Eval("TransactionType") %> | no validator found |  |  |  |  |
| (no id) | BoundField | Reference Document | no validator found |  |  |  |  |
| (no id) | BoundField | Source / Origin | no validator found |  |  |  |  |
| (no id) | BoundField | Destination / Party Details | no validator found |  |  |  |  |
| (no id) | BoundField | Receive (+) | no validator found |  |  |  |  |
| (no id) | BoundField | Issue (-) | no validator found |  |  |  |  |
| (no id) | BoundField | Batch Balance | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| productDropDownList | productCode | SInventory_UI_BatchLifecycleReport.productDropDownList_SelectedIndexChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList | comUnitId | SInventory_UI_BatchLifecycleReport.LoadData | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| productDropDownList | filename | SInventory_UI_BatchLifecycleReport.exportButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList | LoadFilters → DCStockReportBLL.LoadCompanyUnit | inline SQL | ComInfo/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| productDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| batchDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| txnTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| searchButton | LinkButton click | (postback) | SInventory_UI_BatchLifecycleReport.searchButton_Click | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:242 | — | — | sp_Get_BatchLifecycleReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| resetButton | LinkButton click | (postback) | SInventory_UI_BatchLifecycleReport.resetButton_Click | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:247 | — | — | — | read-only / none detected |
| exportButton | LinkButton click | (postback) | SInventory_UI_BatchLifecycleReport.exportButton_Click | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:252 | — | — | — | read-only / none detected |

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
| SInventory_UI_BatchLifecycleReport.Page_Load | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:30 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.Page_Load → DCStockReportBLL.LoadCompanyUnit → DCStockReportDAL.LoadCompanyUnit / DCStockReportDAL.SingleParameter | no | no |
| SInventory_UI_BatchLifecycleReport.LoadFilters | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:44 |  | SInventory_UI_BatchLifecycleReport.LoadFilters → DCStockReportBLL.LoadCompanyUnit → DCStockReportDAL.LoadCompanyUnit / DCStockReportDAL.SingleParameter | yes | no |
| SInventory_UI_BatchLifecycleReport.LoadProductDropdown | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:59 |  | SInventory_UI_BatchLifecycleReport.LoadProductDropdown | yes | no |
| SInventory_UI_BatchLifecycleReport.productDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:94 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.productDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_BatchLifecycleReport.dcDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:108 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.dcDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_BatchLifecycleReport.LoadBatchDropdown | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:116 | string productCode | SInventory_UI_BatchLifecycleReport.LoadBatchDropdown | yes | no |
| SInventory_UI_BatchLifecycleReport.LoadData | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:161 |  | SInventory_UI_BatchLifecycleReport.LoadData → SP: sp_Get_BatchLifecycleReport | yes | no |
| SInventory_UI_BatchLifecycleReport.searchButton_Click | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:242 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.searchButton_Click → SP: sp_Get_BatchLifecycleReport | no | no |
| SInventory_UI_BatchLifecycleReport.resetButton_Click | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:247 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.resetButton_Click | no | no |
| SInventory_UI_BatchLifecycleReport.exportButton_Click | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:252 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.exportButton_Click | yes | no |
| SInventory_UI_BatchLifecycleReport.loadGridView_RowDataBound | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:295 | object sender, GridViewRowEventArgs e | SInventory_UI_BatchLifecycleReport.loadGridView_RowDataBound | no | no |
| SInventory_UI_BatchLifecycleReport.loadGridView_PreRender | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:360 | object sender, EventArgs e | SInventory_UI_BatchLifecycleReport.loadGridView_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DCStockReportBLL.LoadCompanyUnit | Library.BLL/SInventory_BLL/DCStockReportBLL.cs:16 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DCStockReportDAL.LoadCompanyUnit | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:16 | inline SQL: tblCompanyUnit |  |
| DCStockReportDAL.SingleParameter | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:555 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_BatchLifecycleReport | SInventory_UI_BatchLifecycleReport.LoadData | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblDCStore, tblProduct, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadFilters) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error loading filters: | UNKNOWN |
| (handler LoadProductDropdown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Product --- | UNKNOWN |
| (handler LoadProductDropdown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error loading Products: | UNKNOWN |
| (handler productDropDownList_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Batch --- | UNKNOWN |
| (handler LoadBatchDropdown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Batch --- | UNKNOWN |
| (handler LoadBatchDropdown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error loading Batches: | UNKNOWN |
| (handler LoadData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error retrieving report: | UNKNOWN |
| (handler LoadData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select a Batch No. | UNKNOWN |
| (handler LoadData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select a Product. | UNKNOWN |
| (handler exportButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error exporting to Excel: | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 5 of 14 methods.
- Redirect targets: BatchLifecycleReport.aspx, ~/Login.aspx
- User messages: --- Select Batch ---; --- Select Product ---; Error exporting to Excel:; Error loading Batches:; Error loading Products:; Error loading filters:
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DCStockReportBLL; DAL: DCStockReportDAL; DAO: —; Tables (inline): tblCompanyUnit, tblDCStore, tblProduct, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click resetButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click resetButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click exportButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click exportButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/BatchLifecycleReport.aspx`, `Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 33 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx |
| Data access | This document §15–17 | 1 SP names, 4 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DCStockReportDAL.LoadCompanyUnit, DCStockReportDAL.SingleParameter affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
