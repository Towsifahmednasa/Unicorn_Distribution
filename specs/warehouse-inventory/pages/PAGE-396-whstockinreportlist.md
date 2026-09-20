---
page_id: PAGE-396
module: M10
page: WHStockInReportList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-396 — WHStockInReportList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-396 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WHStockInReportList |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WHStockInReportList |
| aspx size | 19787 bytes |
| Code-behind LOC | 174 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **WHStockInReportList** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `whstockinreportviewer.aspx`

## 5. Route
`~/SInventory_UI/WHStockInReportList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WHStockInReportList.aspx |
| Code-behind | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | WHStockInReportBll (Library.BLL/SInventory_BLL/WHStockInReportBll.cs) |
| DAL class | WHStockInReportDal (Library.DAL/SInventory_DAL/WHStockInReportDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| reportDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnTextChanged→reportDropDownList_OnTextChanged |
| stockInDateTextBox | TextBox |  | no validator found |  |  |  |  |
| productDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | LinkButton |  | no validator found |  | Visible=True |  | OnClick→searchButton_Click |
| reportButton | LinkButton |  | no validator found |  | Visible=False |  | OnClick→reportButton_OnClick |
| excelButton | LinkButton |  | no validator found |  | Visible=False |  | OnClick→excelButton_OnClick |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Code | no validator found |  |  |  |  |
| (no id) | BoundField | Challan No | no validator found |  |  |  |  |
| (no id) | BoundField | Challan Date | no validator found |  |  |  |  |
| (no id) | BoundField | Total Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amt | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| printButton | ImageButton |  | no validator found |  |  |  | OnClick→printButton_Click |
| reportDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnTextChanged→reportDropDownList_OnTextChanged |
| stockInDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| productDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | Button | Search | no validator found |  | Visible=True |  | OnClick→searchButton_Click |
| reportButton | Button | View Report | no validator found |  | Visible=False |  | OnClick→reportButton_OnClick |
| excelButton | Button | Excel | no validator found |  | Visible=False |  | OnClick→excelButton_OnClick |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Code | no validator found |  |  |  |  |
| (no id) | BoundField | Challan No | no validator found |  |  |  |  |
| (no id) | BoundField | Challan Date | no validator found |  |  |  |  |
| (no id) | BoundField | Total Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amt | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| printButton | ImageButton |  | no validator found |  |  |  | OnClick→printButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| stockInDateTextBox | aDataTable | SInventory_UI_WHStockInReportList.LoadGridView | WHStockInReportBll.LoadWarehouseStockInData | WHStockInReportDal.GetWarehouseStockInData | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| reportDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| productDropDownList | reportDropDownList_OnTextChanged → WHStockInReportBll.LoadProductInformation | inline SQL | ProductName/ProductCode | tblProduct |
| reportDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| productDropDownList | reportDropDownList_OnTextChanged → WHStockInReportBll.LoadProductInformation | inline SQL | ProductName/ProductCode | tblProduct |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| searchButton | LinkButton click | (postback) | SInventory_UI_WHStockInReportList.searchButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:55 | WHStockInReportBll.LoadWarehouseStockInData | WHStockInReportDal.GetWarehouseStockInData | — | read-only / none detected |
| reportButton | LinkButton click | (postback) | SInventory_UI_WHStockInReportList.reportButton_OnClick | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:125 | — | — | — | read-only / none detected |
| excelButton | LinkButton click | (postback) | SInventory_UI_WHStockInReportList.excelButton_OnClick | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:151 | — | — | — | read-only / none detected |
| LinkButton1 | LinkButton click | (postback) | SInventory_UI_WHStockInReportList.cancelButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:23 | — | — | — | read-only / none detected |
| printButton | ImageButton click | (postback) | SInventory_UI_WHStockInReportList.printButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:60 | — | — | — | read-only / none detected |
| searchButton | Button click | (postback) | SInventory_UI_WHStockInReportList.searchButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:55 | WHStockInReportBll.LoadWarehouseStockInData | WHStockInReportDal.GetWarehouseStockInData | — | read-only / none detected |
| reportButton | Button click | (postback) | SInventory_UI_WHStockInReportList.reportButton_OnClick | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:125 | — | — | — | read-only / none detected |
| excelButton | Button click | (postback) | SInventory_UI_WHStockInReportList.excelButton_OnClick | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:151 | — | — | — | read-only / none detected |
| printButton | ImageButton click | (postback) | SInventory_UI_WHStockInReportList.printButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:60 | — | — | — | read-only / none detected |

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
| SInventory_UI_WHStockInReportList.Page_Load | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.Page_Load | no | no |
| SInventory_UI_WHStockInReportList.cancelButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.cancelButton_Click | no | no |
| SInventory_UI_WHStockInReportList.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:27 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_WHStockInReportList.LoadGridView | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:46 |  | SInventory_UI_WHStockInReportList.LoadGridView → WHStockInReportBll.LoadWarehouseStockInData → WHStockInReportDal.GetWarehouseStockInData | no | no |
| SInventory_UI_WHStockInReportList.searchButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:55 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.searchButton_Click → WHStockInReportBll.LoadWarehouseStockInData → WHStockInReportDal.GetWarehouseStockInData | no | no |
| SInventory_UI_WHStockInReportList.printButton_Click | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:60 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.printButton_Click | no | no |
| SInventory_UI_WHStockInReportList.reportDropDownList_OnTextChanged | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:83 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.reportDropDownList_OnTextChanged → WHStockInReportBll.LoadProductInformation → WHStockInReportDal.GetProductInformation | no | no |
| SInventory_UI_WHStockInReportList.reportButton_OnClick | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:125 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.reportButton_OnClick | no | no |
| SInventory_UI_WHStockInReportList.excelButton_OnClick | Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs:151 | object sender, EventArgs e | SInventory_UI_WHStockInReportList.excelButton_OnClick | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WHStockInReportBll.LoadWarehouseStockInData | Library.BLL/SInventory_BLL/WHStockInReportBll.cs:16 |
| WHStockInReportBll.LoadProductInformation | Library.BLL/SInventory_BLL/WHStockInReportBll.cs:36 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WHStockInReportDal.GetWarehouseStockInData | Library.DAL/SInventory_DAL/WHStockInReportDal.cs:16 | inline SQL: tblWHStockInMaster |  |
| WHStockInReportDal.GetProductInformation | Library.DAL/SInventory_DAL/WHStockInReportDal.cs:46 | inline SQL: tblProduct |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblProduct, tblWHStockInMaster
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: Excel, ProductCode, ReportType
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
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
BLL: WHStockInReportBll; DAL: WHStockInReportDal; DAO: —; Tables (inline): tblProduct, tblWHStockInMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click reportButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click reportButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click excelButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click excelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click printButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click printButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-13 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-14 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WHStockInReportList.aspx`, `Solution.Web/SInventory_UI/WHStockInReportList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/WHStockInReportList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 41 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/WHStockInReportList.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods WHStockInReportDal.GetProductInformation, WHStockInReportDal.GetWarehouseStockInData affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
