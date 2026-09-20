---
page_id: PAGE-293
module: M08
page: SalesRejectionReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-293 — SalesRejectionReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-293 |
| Module | M08 — Sales Return & Invoice Return (INFERRED from folder + file name) |
| Page name | SalesRejectionReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SalesRejectionReport |
| aspx size | 17391 bytes |
| Code-behind LOC | 456 |
| Inline script size | 2411 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SalesRejectionReport** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales returns, return invoices, delivery returns, replacement notes, return posting.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `proformareportviewer.aspx`

## 5. Route
`~/SInventory_UI/SalesRejectionReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SalesRejectionReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 |  |
| InvoiceDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| lblCount | Label | Total Net Amount : 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Provider Type | no validator found |  |  |  |  |
| (no id) | BoundField | Pharma Platform | no validator found |  |  |  |  |
| (no id) | BoundField |  Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Number | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Rejection Date | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Exp Date | no validator found |  |  |  |  |
| (no id) | BoundField | Rejection Qty | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Special Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Exp. Adjustment | no validator found |  |  |  |  |
| (no id) | BoundField |  Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO  EMP Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO EMP Name | no validator found |  |  |  |  |
| (no id) | BoundField | Area Code | no validator found |  |  |  |  |
| (no id) | BoundField | Area Name | no validator found |  |  |  |  |
| (no id) | BoundField | Zone Code | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Type | no validator found |  |  |  |  |
| (no id) | BoundField | Reject Type | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Name | no validator found |  |  |  |  |
| (no id) | BoundField | AM Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Group | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Type | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList1 | param | SInventory_UI_SalesRejectionReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_SalesRejectionReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | param | SInventory_UI_SalesRejectionReport.Parm2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_SalesRejectionReport.Parm2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | Text | SInventory_UI_SalesRejectionReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_SalesRejectionReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_SalesRejectionReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | districtId | SInventory_UI_SalesRejectionReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | inputDateTime | SInventory_UI_SalesRejectionReport.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_SalesRejectionReport.SearchButton_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:63 | — | CmnCrystaltoView.GetSalesRejectionReportDAL | sp_Get_SalesRejectionReportListLAtest | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | SInventory_UI_SalesRejectionReport.cancelButton_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:59 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_SalesRejectionReport.btnExport_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:247 | — | — | — | read-only / none detected |

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
| SInventory_UI_SalesRejectionReport.Page_Load | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.Page_Load → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_SalesRejectionReport.LoadDropDown | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:45 |  | SInventory_UI_SalesRejectionReport.LoadDropDown → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | yes | no |
| SInventory_UI_SalesRejectionReport.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:54 | object sender, GridViewPageEventArgs e | SInventory_UI_SalesRejectionReport.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_SalesRejectionReport.cancelButton_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:59 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.cancelButton_Click | no | no |
| SInventory_UI_SalesRejectionReport.SearchButton_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:63 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.SearchButton_Click → CmnCrystaltoView.GetSalesRejectionReportDAL → SP: sp_Get_SalesRejectionReportListLAtest | no | no |
| SInventory_UI_SalesRejectionReport.LoadData | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:98 |  | SInventory_UI_SalesRejectionReport.LoadData → CmnCrystaltoView.GetSalesRejectionReportDAL → SP: sp_Get_SalesRejectionReportListLAtest | no | no |
| SInventory_UI_SalesRejectionReport.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:239 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.CheckBox1_CheckedChanged → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_SalesRejectionReport.btnExport_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:247 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.btnExport_Click | no | no |
| SInventory_UI_SalesRejectionReport.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:398 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_SalesRejectionReport.viewRptButton_Click | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:415 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.viewRptButton_Click | no | no |
| SInventory_UI_SalesRejectionReport.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs:448 | object sender, EventArgs e | SInventory_UI_SalesRejectionReport.fromDateTextBox_TextChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| CmnCrystaltoView.GetSalesRejectionReportDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:219 | sp_Get_SalesRejectionReportListLAtest | @Parm, @Parm2 |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_SalesRejectionReportListLAtest | CmnCrystaltoView.GetSalesRejectionReportDAL | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ProformaReport
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 15 methods.
- Redirect targets: SalesRejectionReport.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL; DAL: CmnCrystaltoView, OtherStockActionDAL; DAO: —; Tables (inline): tblCompanyUnit, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SalesRejectionReport.aspx`, `Solution.Web/SInventory_UI/SalesRejectionReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SalesRejectionReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 62 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SalesRejectionReport.aspx |
| Data access | This document §15–17 | 1 SP names, 2 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M08 (Sales Return & Invoice Return).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CmnCrystaltoView.GetSalesRejectionReportDAL, OtherStockActionDAL.DCLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
