---
page_id: PAGE-421
module: M11
page: StockTransferReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-421 — StockTransferReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-421 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | StockTransferReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_StockTransferReport |
| aspx size | 12470 bytes |
| Code-behind LOC | 89 |
| Inline script size | 3371 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockTransferReport** (Report / Dashboard). Page title/heading text: `Stock Transfer Report`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/StockTransferReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockTransferReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | StockTransferReportBLL (Library.BLL/SInventory_BLL/StockTransferReportBLL.cs) |
| DAL class | StockTransferReportDAL (Library.DAL/SInventory_DAL/StockTransferReportDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | TextBox | dd-mmm-yyyy | no validator found |  |  |  |  |
| toDateTextBox | TextBox | dd-mmm-yyyy | no validator found |  |  |  |  |
| fromLocationDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| toLocationDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | Button | Search | no validator found |  |  |  | OnClick→searchButton_Click |
| resetButton | Button | Reset | no validator found |  |  |  | OnClick→resetButton_Click |
| totalRecordsLabel | Label | 0 | no validator found |  |  |  |  |
| totalAmountLabel | Label | 0.00 | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_PreRender |
| (no id) | BoundField | Date | no validator found |  |  |  |  |
| (no id) | BoundField | Ref # | no validator found |  |  |  |  |
| (no id) | BoundField | STO No | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Unit Price | no validator found |  |  |  |  |
| (no id) | BoundField | VAT Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | From | no validator found |  |  |  |  |
| (no id) | BoundField | To | no validator found |  |  |  |  |
| (no id) | BoundField | User | no validator found |  |  |  |  |
| (no id) | TemplateField | STO Preview | no validator found |  |  |  |  |
| (no id) | a | STO Voucher Preview | no validator found |  |  |  | onclick→openReport('../SInventory_RPTVIEW/StockTransfarOrderReportViewer.aspx?reqId=<%# Eval( |
| (no id) | TemplateField | Challan Preview | no validator found |  |  |  |  |
| (no id) | a | Challan Voucher Preview | no validator found |  |  |  | onclick→openReport('../SInventory_RPTVIEW/ChallanReportViewer.aspx?reqId=<%# Eval( |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | fromDate | SInventory_UI_StockTransferReport.LoadReport | StockTransferReportBLL.LoadStockTransferReport | StockTransferReportDAL.LoadStockTransferReport | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_StockTransferReport.LoadReport | StockTransferReportBLL.LoadStockTransferReport | StockTransferReportDAL.LoadStockTransferReport | UNKNOWN | UNKNOWN | UNKNOWN |
| fromLocationDropDownList | fromLocationId | SInventory_UI_StockTransferReport.LoadReport | StockTransferReportBLL.LoadStockTransferReport | StockTransferReportDAL.LoadStockTransferReport | UNKNOWN | UNKNOWN | UNKNOWN |
| toLocationDropDownList | toLocationId | SInventory_UI_StockTransferReport.LoadReport | StockTransferReportBLL.LoadStockTransferReport | StockTransferReportDAL.LoadStockTransferReport | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| fromLocationDropDownList | BindDropDowns → StockTransferReportBLL.LoadCompanyUnits | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit |
| toLocationDropDownList | BindDropDowns → StockTransferReportBLL.LoadCompanyUnits | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| searchButton | Button click | (postback) | SInventory_UI_StockTransferReport.searchButton_Click | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:67 | StockTransferReportBLL.LoadStockTransferReport | StockTransferReportDAL.LoadStockTransferReport | — | read-only / none detected |
| resetButton | Button click | (postback) | SInventory_UI_StockTransferReport.resetButton_Click | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:72 | StockTransferReportBLL.LoadStockTransferReport | StockTransferReportDAL.LoadStockTransferReport | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: initDataTable, openReport, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_StockTransferReport.Page_Load | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_StockTransferReport.Page_Load → StockTransferReportBLL.LoadCompanyUnits / StockTransferReportBLL.LoadStockTransferReport → StockTransferReportDAL.LoadCompanyUnits / StockTransferReportDAL.LoadStockTransferReport | no | no |
| SInventory_UI_StockTransferReport.BindDropDowns | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:26 |  | SInventory_UI_StockTransferReport.BindDropDowns → StockTransferReportBLL.LoadCompanyUnits → StockTransferReportDAL.LoadCompanyUnits | no | no |
| SInventory_UI_StockTransferReport.LoadReport | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:35 |  | SInventory_UI_StockTransferReport.LoadReport → StockTransferReportBLL.LoadStockTransferReport → StockTransferReportDAL.LoadStockTransferReport | no | no |
| SInventory_UI_StockTransferReport.searchButton_Click | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:67 | object sender, EventArgs e | SInventory_UI_StockTransferReport.searchButton_Click → StockTransferReportBLL.LoadStockTransferReport → StockTransferReportDAL.LoadStockTransferReport | no | no |
| SInventory_UI_StockTransferReport.resetButton_Click | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:72 | object sender, EventArgs e | SInventory_UI_StockTransferReport.resetButton_Click → StockTransferReportBLL.LoadStockTransferReport → StockTransferReportDAL.LoadStockTransferReport | no | no |
| SInventory_UI_StockTransferReport.gv_PreRender | Solution.Web/SInventory_UI/StockTransferReport.aspx.cs:81 | object sender, EventArgs e | SInventory_UI_StockTransferReport.gv_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| StockTransferReportBLL.LoadCompanyUnits | Library.BLL/SInventory_BLL/StockTransferReportBLL.cs:18 |
| StockTransferReportBLL.LoadStockTransferReport | Library.BLL/SInventory_BLL/StockTransferReportBLL.cs:13 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| StockTransferReportDAL.LoadCompanyUnits | Library.DAL/SInventory_DAL/StockTransferReportDAL.cs:102 | inline SQL: tblCompanyUnit |  |
| StockTransferReportDAL.LoadStockTransferReport | Library.DAL/SInventory_DAL/StockTransferReportDAL.cs:12 | inline SQL: tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblRequisition | @FromDate, @FromLocationId, @ToDate, @ToLocationId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblRequisition, tblStockInTransfar, tblUser
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

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
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: StockTransferReportBLL; DAL: StockTransferReportDAL; DAO: —; Tables (inline): tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblRequisition, tblStockInTransfar, tblUser

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

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockTransferReport.aspx`, `Solution.Web/SInventory_UI/StockTransferReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockTransferReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 29 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockTransferReport.aspx |
| Data access | This document §15–17 | 0 SP names, 6 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods StockTransferReportDAL.LoadCompanyUnits, StockTransferReportDAL.LoadStockTransferReport affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
