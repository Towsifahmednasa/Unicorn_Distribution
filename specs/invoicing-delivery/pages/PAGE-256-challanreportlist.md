---
page_id: PAGE-256
module: M07
page: ChallanReportList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-256 — ChallanReportList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-256 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | ChallanReportList |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ChallanReportList |
| aspx size | 30409 bytes |
| Code-behind LOC | 353 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ChallanReportList** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Proforma and invoice creation (manual/auto/by-order), challans, top sheets, picking, loading, delivery assistants, invoice printing.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `challanreportviewer.aspx`

## 5. Route
`~/SInventory_UI/ChallanReportList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ChallanReportList.aspx |
| Code-behind | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| CheckBox1 | CheckBox |  | no validator found |  |  |  | oncheckedchanged→CheckBox1_CheckedChanged |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| dateTextBox | TextBox |  | no validator found |  |  |  |  |
| manufacturerDate1 | CalendarExtender |  | no validator found |  |  |  |  |
| TextBox1 | TextBox |  | no validator found |  |  |  |  |
| detailsButton | LinkButton |  | no validator found |  |  |  | OnClick→detailsButton_Click |
| summaryButton | LinkButton |  | no validator found |  |  |  | OnClick→summaryButton_Click |
| searchButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| excelButton1 | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| reportListGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | D.C.Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C.Name | no validator found |  |  |  |  |
| (no id) | BoundField | Req.No | no validator found |  |  |  |  |
| (no id) | BoundField | Req.Date | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnNo | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Print | no validator found |  |  |  |  |
| printButton | Button | PRINT | no validator found |  |  |  | onclick→printButton_Click |
| summaryGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Total Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total VAT Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amount with Vat | no validator found |  |  |  |  |
| detailsGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | D.C.Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C.Name | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Total Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | TP Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Amount | no validator found |  |  |  |  |
| (no id) | BoundField | VAT Amount Per Unit | no validator found |  |  |  |  |
| (no id) | BoundField | Total VAT Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amount with Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Req.No | no validator found |  |  |  |  |
| (no id) | BoundField | Req.Date | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnNo | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Print | no validator found |  |  |  |  |
| printButton | Button | PRINT | no validator found |  |  |  | onclick→printButton_Click |
| CheckBox1 | CheckBox |  | no validator found |  |  |  | oncheckedchanged→CheckBox1_CheckedChanged |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| dateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| TextBox1 | TextBox |  | no validator found |  |  |  |  |
| imgDate2 | ImageButton |  | no validator found |  |  |  |  |
| CalendarExtender1 | CalendarExtender |  | no validator found |  |  |  |  |
| detailsButton | Button | Details Report | no validator found |  |  |  | onclick→detailsButton_Click |
| summaryButton | Button | Summary Report | no validator found |  |  |  | onclick→summaryButton_Click |
| searchButton | Button | View List | no validator found |  |  |  | onclick→searchButton_Click |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| reportListGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | D.C.Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C.Name | no validator found |  |  |  |  |
| (no id) | BoundField | Req.No | no validator found |  |  |  |  |
| (no id) | BoundField | Req.Date | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnNo | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Print | no validator found |  |  |  |  |
| printButton | Button | PRINT | no validator found |  |  |  | onclick→printButton_Click |
| summaryGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Total Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total VAT Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amount with Vat | no validator found |  |  |  |  |
| detailsGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | D.C.Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C.Name | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Total Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | TP Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Amount | no validator found |  |  |  |  |
| (no id) | BoundField | VAT Amount Per Unit | no validator found |  |  |  |  |
| (no id) | BoundField | Total VAT Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amount with Vat | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnNo | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryClnDate | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dateTextBox | aTable | SInventory_UI_ChallanReportList.searchButton_Click | RequisitionBLL.ChallanReportDAL2 | RequisitionDAL.ChallanReportDAL2, RequisitionDAL.DateRangeParameters | UNKNOWN | UNKNOWN | UNKNOWN |
| dateTextBox | aTable | SInventory_UI_ChallanReportList.SummaryGrid | RequisitionBLL.ChallanSummaryReportDAL2 | RequisitionDAL.ChallanSummaryReportDAL2, RequisitionDAL.DateRangeParameters | UNKNOWN | UNKNOWN | UNKNOWN |
| dateTextBox | aTable | SInventory_UI_ChallanReportList.DetailsGrid | RequisitionBLL.ChallanDetailReportDAL2 | RequisitionDAL.ChallanDetailReportDAL2, RequisitionDAL.DateRangeParameters | UNKNOWN | UNKNOWN | UNKNOWN |
| dateTextBox | headerTable | SInventory_UI_ChallanReportList.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | Page_Load → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| salesCenterDropDownList | Page_Load → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| detailsButton | LinkButton click | (postback) | SInventory_UI_ChallanReportList.detailsButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:137 | RequisitionBLL.ChallanDetailReportDAL2 | RequisitionDAL.ChallanDetailReportDAL2, RequisitionDAL.DateRangeParameters | — | read-only / none detected |
| summaryButton | LinkButton click | (postback) | SInventory_UI_ChallanReportList.summaryButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:91 | RequisitionBLL.ChallanSummaryReportDAL2 | RequisitionDAL.ChallanSummaryReportDAL2, RequisitionDAL.DateRangeParameters | — | read-only / none detected |
| searchButton | LinkButton click | (postback) | SInventory_UI_ChallanReportList.searchButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:48 | RequisitionBLL.ChallanReportDAL2 | RequisitionDAL.ChallanReportDAL2, RequisitionDAL.DateRangeParameters | — | read-only / none detected |
| LinkButton1 | LinkButton | — | cancelButton_Click | — | — | — | — | UNKNOWN (handler not resolved) |
| excelButton1 | LinkButton click | (postback) | SInventory_UI_ChallanReportList.btnExportToExcel_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:217 | — | — | — | read-only / none detected |
| excelButton1 | Button click | (postback) | SInventory_UI_ChallanReportList.btnExportToExcel_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:217 | — | — | — | read-only / none detected |

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
| SInventory_UI_ChallanReportList.Page_Load | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_ChallanReportList.Page_Load → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_ChallanReportList.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:26 | object sender, EventArgs e | SInventory_UI_ChallanReportList.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_ChallanReportList.searchButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:48 | object sender, EventArgs e | SInventory_UI_ChallanReportList.searchButton_Click → RequisitionBLL.ChallanReportDAL2 → RequisitionDAL.ChallanReportDAL2 / RequisitionDAL.DateRangeParameters | no | no |
| SInventory_UI_ChallanReportList.summaryButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:91 | object sender, EventArgs e | SInventory_UI_ChallanReportList.summaryButton_Click → RequisitionBLL.ChallanSummaryReportDAL2 → RequisitionDAL.ChallanSummaryReportDAL2 / RequisitionDAL.DateRangeParameters | no | no |
| SInventory_UI_ChallanReportList.SummaryGrid | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:96 |  | SInventory_UI_ChallanReportList.SummaryGrid → RequisitionBLL.ChallanSummaryReportDAL2 → RequisitionDAL.ChallanSummaryReportDAL2 / RequisitionDAL.DateRangeParameters | no | no |
| SInventory_UI_ChallanReportList.detailsButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:137 | object sender, EventArgs e | SInventory_UI_ChallanReportList.detailsButton_Click → RequisitionBLL.ChallanDetailReportDAL2 → RequisitionDAL.ChallanDetailReportDAL2 / RequisitionDAL.DateRangeParameters | no | no |
| SInventory_UI_ChallanReportList.DetailsGrid | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:142 |  | SInventory_UI_ChallanReportList.DetailsGrid → RequisitionBLL.ChallanDetailReportDAL2 → RequisitionDAL.ChallanDetailReportDAL2 / RequisitionDAL.DateRangeParameters | no | no |
| SInventory_UI_ChallanReportList.printButton_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:190 | object sender, EventArgs e | SInventory_UI_ChallanReportList.printButton_Click | no | no |
| SInventory_UI_ChallanReportList.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:200 | object sender, EventArgs e | SInventory_UI_ChallanReportList.CheckBox1_CheckedChanged | no | no |
| SInventory_UI_ChallanReportList.btnExportToExcel_Click | Solution.Web/SInventory_UI/ChallanReportList.aspx.cs:217 | object sender, EventArgs e | SInventory_UI_ChallanReportList.btnExportToExcel_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| RequisitionBLL.ChallanReportDAL2 | Library.BLL/SInventory_BLL/RequisitionBLL.cs:458 |
| RequisitionBLL.ChallanSummaryReportDAL2 | Library.BLL/SInventory_BLL/RequisitionBLL.cs:500 |
| RequisitionBLL.ChallanDetailReportDAL2 | Library.BLL/SInventory_BLL/RequisitionBLL.cs:490 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.ChallanReportDAL2 | Library.DAL/SInventory_DAL/RequisitionDAL.cs:613 | inline SQL: tblRequisition | @ComUnitId, @FromDate, @ToDate |
| RequisitionDAL.DateRangeParameters | Library.DAL/SInventory_DAL/RequisitionDAL.cs:773 | delegates / no SQL literal | @FromDate, @ToDate |
| RequisitionDAL.ChallanSummaryReportDAL2 | Library.DAL/SInventory_DAL/RequisitionDAL.cs:729 | inline SQL: tblCentralStore, tblProduct, tblRequisition, tblStockInTransfar | @ComUnitId, @FromDate, @ToDate |
| RequisitionDAL.ChallanDetailReportDAL2 | Library.DAL/SInventory_DAL/RequisitionDAL.cs:695 | inline SQL: tblCentralStore, tblProduct, tblRequisition, tblStockInTransfar | @ComUnitId, @FromDate, @ToDate |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCentralStore, tblCompanyUnit, tblProduct, tblRequisition, tblStockInTransfar, tblUnitPrice, tblUserCompanyUnit, tblWHStockInDetail
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 13 methods.
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
BLL: OrderInfoBLL, RequisitionBLL; DAL: OrderInfoDAL, RequisitionDAL; DAO: —; Tables (inline): tblCentralStore, tblCompanyUnit, tblProduct, tblRequisition, tblStockInTransfar, tblUnitPrice, tblUserCompanyUnit, tblWHStockInDetail

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click detailsButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click detailsButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click summaryButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click summaryButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-13 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-14 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ChallanReportList.aspx`, `Solution.Web/SInventory_UI/ChallanReportList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ChallanReportList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 94 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ChallanReportList.aspx |
| Data access | This document §15–17 | 0 SP names, 8 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M07 (Proforma, Invoicing & Delivery).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadSC, RequisitionDAL.ChallanDetailReportDAL2, RequisitionDAL.ChallanReportDAL2, RequisitionDAL.ChallanSummaryReportDAL2, RequisitionDAL.DateRangeParameters affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
