---
page_id: PAGE-372
module: M10
page: MonthlyInventoryReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-372 — MonthlyInventoryReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-372 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | MonthlyInventoryReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_MonthlyInventoryReport |
| aspx size | 14776 bytes |
| Code-behind LOC | 397 |
| Inline script size | 2421 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MonthlyInventoryReport** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `intransitreport.aspx`, `intransitreportviewer.aspx`

## 5. Route
`~/SInventory_UI/MonthlyInventoryReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | ProductBLL (Library.BLL/SInventory_BLL/ProductBLL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | ProductDAL (Library.DAL/SInventory_DAL/ProductDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→dcDropDownList1_SelectedIndexChanged |
| ddlProTypeNew | DropDownList |  | no validator found |  |  | see §9 |  |
| InvoiceDateTextBox | TextBox |  | no validator found |  |  |  |  |
| todateTextBox | TextBox |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| SearchButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| excelButton | LinkButton |  | no validator found |  | Visible=False |  | OnClick→excelButton_OnClick |
| marketDropDownList | DropDownList |  | no validator found |  | Visible=false | see §9 |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Center | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Base Unit | no validator found |  |  |  |  |
| (no id) | BoundField | Opening Stock | no validator found |  |  |  |  |
| (no id) | BoundField | CWH | no validator found |  |  |  |  |
| (no id) | BoundField | Area Office InterTransfer | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Depo Receive | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Sales | no validator found |  |  |  |  |
| (no id) | BoundField | Bonus | no validator found |  |  |  |  |
| (no id) | BoundField | Return | no validator found |  |  |  |  |
| (no id) | BoundField | Area Office Inter Transfer | no validator found |  |  |  |  |
| (no id) | BoundField | WH Return | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Depo Transfer | no validator found |  |  |  |  |
| (no id) | BoundField | Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Restricted Stock | no validator found |  |  |  |  |
| (no id) | BoundField | Adjustment  | no validator found |  |  |  |  |
| (no id) | BoundField | Closing Stock | no validator found |  |  |  |  |
| (no id) | BoundField | Book for Delivery Qty | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| InvoiceDateTextBox | inputDateTime | SInventory_UI_MonthlyInventoryReport.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | headerTable | SInventory_UI_MonthlyInventoryReport.btnExport_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | param | SInventory_UI_MonthlyInventoryReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_MonthlyInventoryReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | aDataTable | SInventory_UI_MonthlyInventoryReport.LoadData | — | InvoiceDAL.GetMonthlyInventoryReportDAl | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | Text | SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | districtId | SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| ddlProTypeNew | Page_Load → ProductBLL.LoadProductType_new | inline SQL | GroupName/GroupId | tblProductGroup |
| marketDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| SearchButton | LinkButton click | (postback) | SInventory_UI_MonthlyInventoryReport.SearchButton_Click | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:315 | — | InvoiceDAL.GetMonthlyInventoryReportDAl | sp_Get_MonthlyInventoryReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| excelButton | LinkButton click | (postback) | SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:358 | — | — | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_MonthlyInventoryReport.Unnamed_Click | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:394 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_MonthlyInventoryReport.btnExport_Click | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:50 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: exportTableToExcel, exportToExcel, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_MonthlyInventoryReport.Page_Load | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:21 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.Page_Load → OtherStockActionBLL.LoadSC / ProductBLL.LoadProductType_new → OtherStockActionDAL.LoadSC / ProductDAL.LoadProductType_new | no | no |
| SInventory_UI_MonthlyInventoryReport.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:33 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.fromDateTextBox_TextChanged | no | no |
| SInventory_UI_MonthlyInventoryReport.btnExport_Click | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:50 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.btnExport_Click | no | no |
| SInventory_UI_MonthlyInventoryReport.LoadDropDown | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:183 |  | SInventory_UI_MonthlyInventoryReport.LoadDropDown → OtherStockActionBLL.LoadSC → OtherStockActionDAL.LoadSC | no | no |
| SInventory_UI_MonthlyInventoryReport.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:221 | object sender, GridViewPageEventArgs e | SInventory_UI_MonthlyInventoryReport.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_MonthlyInventoryReport.LoadData | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:226 |  | SInventory_UI_MonthlyInventoryReport.LoadData → InvoiceDAL.GetMonthlyInventoryReportDAl → SP: sp_Get_MonthlyInventoryReport | no | no |
| SInventory_UI_MonthlyInventoryReport.loadGridView_OnRowCreated | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:256 | object sender, GridViewRowEventArgs e | SInventory_UI_MonthlyInventoryReport.loadGridView_OnRowCreated | no | no |
| SInventory_UI_MonthlyInventoryReport.SearchButton_Click | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:315 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.SearchButton_Click → InvoiceDAL.GetMonthlyInventoryReportDAl → SP: sp_Get_MonthlyInventoryReport | no | no |
| SInventory_UI_MonthlyInventoryReport.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:353 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.dcDropDownList1_SelectedIndexChanged | no | no |
| SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:358 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.excelButton_OnClick | no | no |
| SInventory_UI_MonthlyInventoryReport.Unnamed_Click | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs:394 | object sender, EventArgs e | SInventory_UI_MonthlyInventoryReport.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ProductBLL.LoadProductType_new | Library.BLL/SInventory_BLL/ProductBLL.cs:124 |
| OtherStockActionBLL.LoadSC | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:33 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ProductDAL.LoadProductType_new | Library.DAL/SInventory_DAL/ProductDAL.cs:266 | inline SQL: tblProductGroup |  |
| OtherStockActionDAL.LoadSC | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:63 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| InvoiceDAL.GetMonthlyInventoryReportDAl | Library.DAL/SInventory_DAL/InvoiceDAL.cs:159 | sp_Get_MonthlyInventoryReport | @CiD, @ProTypId, @fromDate, @toDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_MonthlyInventoryReport | InvoiceDAL.GetMonthlyInventoryReportDAl | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblProductGroup, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: Excel, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 14 methods.
- Redirect targets: InTransitReport.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL, ProductBLL; DAL: InvoiceDAL, OtherStockActionDAL, ProductDAL; DAO: —; Tables (inline): tblCompanyUnit, tblProductGroup, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click SearchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click SearchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click excelButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click excelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx`, `Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 40 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MonthlyInventoryReport.aspx |
| Data access | This document §15–17 | 1 SP names, 3 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods InvoiceDAL.GetMonthlyInventoryReportDAl, OtherStockActionDAL.LoadSC, ProductDAL.LoadProductType_new affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
