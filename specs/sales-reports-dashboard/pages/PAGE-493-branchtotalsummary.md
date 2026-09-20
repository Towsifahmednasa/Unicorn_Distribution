---
page_id: PAGE-493
module: M13
page: BranchTotalSummary
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-493 — BranchTotalSummary

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-493 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | BranchTotalSummary |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_BranchTotalSummary |
| aspx size | 14196 bytes |
| Code-behind LOC | 183 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **BranchTotalSummary** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `businesssummaryviewer2.aspx`, `businesssummaryviewer3.aspx`, `businesssummaryviewer4.aspx`, `businesssummaryviewer5.aspx`

## 5. Route
`~/SInventory_UI/BranchTotalSummary.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/BranchTotalSummary.aspx |
| Code-behind | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| dzsmDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| fromDate | CalendarExtender |  | no validator found |  |  |  |  |
| imgDateFrom | ImageButton |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| toDate | CalendarExtender |  | no validator found |  |  |  |  |
| imgDateTo | ImageButton |  | no validator found |  |  |  |  |
| viewRptButton | Button | Search | no validator found |  |  |  | OnClick→viewRptButton_Click |
| excelButton1 | Button | Export to Excel(Sales) | no validator found |  |  |  | OnClick→excelButton1_Click |
| proformaButton1 | Button | Export to Excel(Proforma) | no validator found |  |  |  | OnClick→excelButton1_Click2 |
| marketwiseButton | Button | Export to Excel(Market wise Proforma Summary) | no validator found |  |  |  | OnClick→excelButton_Click3 |
| salesButton | Button | Export to Excel(Market wise Sales Summary) | no validator found |  |  |  | OnClick→excelButton_Click4 |
| loadGridView | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name  | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Order | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Order Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Proforma Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Proforma Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Chemist Coverage(%) | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Sales Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Return Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Return Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Undelivered Invoice  | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net UndeliveredAmount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Undelivered Total Vat | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | aDataTable | SInventory_UI_BranchTotalSummary.viewRptButton_Click | TotalSummaryBLL.BranchwiseLoadSummaryBLL | TotalSummaryDAL.BranchwiseLoadSummaryBLL, TotalSummaryDAL.GetReportTable | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_BranchTotalSummary.excelButton1_Click2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_BranchTotalSummary.excelButton1_Click2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | url | SInventory_UI_BranchTotalSummary.excelButton1_Click2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_BranchTotalSummary.excelButton_Click3 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_BranchTotalSummary.excelButton_Click3 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | url | SInventory_UI_BranchTotalSummary.excelButton_Click3 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_BranchTotalSummary.excelButton_Click4 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_BranchTotalSummary.excelButton_Click4 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | url | SInventory_UI_BranchTotalSummary.excelButton_Click4 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_BranchTotalSummary.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_BranchTotalSummary.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | url | SInventory_UI_BranchTotalSummary.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rptTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| dzsmDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewRptButton | Button click | (postback) | SInventory_UI_BranchTotalSummary.viewRptButton_Click | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:30 | TotalSummaryBLL.BranchwiseLoadSummaryBLL | TotalSummaryDAL.BranchwiseLoadSummaryBLL, TotalSummaryDAL.GetReportTable | — | read-only / none detected |
| excelButton1 | Button click | (postback) | SInventory_UI_BranchTotalSummary.excelButton1_Click | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:159 | — | — | — | read-only / none detected |
| proformaButton1 | Button click | (postback) | SInventory_UI_BranchTotalSummary.excelButton1_Click2 | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:105 | — | — | — | read-only / none detected |
| marketwiseButton | Button click | (postback) | SInventory_UI_BranchTotalSummary.excelButton_Click3 | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:123 | — | — | — | read-only / none detected |
| salesButton | Button click | (postback) | SInventory_UI_BranchTotalSummary.excelButton_Click4 | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:141 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_BranchTotalSummary.Page_Load | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.Page_Load → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_BranchTotalSummary.DropDownlist | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:24 |  | SInventory_UI_BranchTotalSummary.DropDownlist → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_BranchTotalSummary.viewRptButton_Click | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:30 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.viewRptButton_Click → TotalSummaryBLL.BranchwiseLoadSummaryBLL → TotalSummaryDAL.BranchwiseLoadSummaryBLL / TotalSummaryDAL.GetReportTable | no | no |
| SInventory_UI_BranchTotalSummary.excelButton1_Click2 | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:105 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.excelButton1_Click2 | no | no |
| SInventory_UI_BranchTotalSummary.excelButton_Click3 | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:123 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.excelButton_Click3 | no | no |
| SInventory_UI_BranchTotalSummary.excelButton_Click4 | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:141 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.excelButton_Click4 | no | no |
| SInventory_UI_BranchTotalSummary.excelButton1_Click | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:159 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.excelButton1_Click | no | no |
| SInventory_UI_BranchTotalSummary.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs:181 | object sender, EventArgs e | SInventory_UI_BranchTotalSummary.rptTypeDropDownList_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| TotalSummaryBLL.BranchwiseLoadSummaryBLL | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:186 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| TotalSummaryDAL.BranchwiseLoadSummaryBLL | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:679 | inline SQL: tblCompanyUnit, tblCustMaster, tblInvoice, tblInvoiceDetail |  |
| TotalSummaryDAL.GetReportTable | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:19 | delegates / no SQL literal | @Branch, @FromDate, @ToDate, @Zone |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblCustMaster, tblInvoice, tblInvoiceDetail, tblOrder, tblSubInvoiceDetail, tblSubInvoiceMaster, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler viewRptButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler excelButton1_Click2) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler excelButton_Click3) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler excelButton_Click4) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler excelButton1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
- Redirect targets: none
- User messages: Please Select Date Range!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL, TotalSummaryBLL; DAL: OrderInfoDAL, TotalSummaryDAL; DAO: —; Tables (inline): tblCompanyUnit, tblCustMaster, tblInvoice, tblInvoiceDetail, tblOrder, tblSubInvoiceDetail, tblSubInvoiceMaster, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewRptButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewRptButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click proformaButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click proformaButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click marketwiseButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click marketwiseButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click salesButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click salesButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/BranchTotalSummary.aspx`, `Solution.Web/SInventory_UI/BranchTotalSummary.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/BranchTotalSummary.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 41 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/BranchTotalSummary.aspx |
| Data access | This document §15–17 | 0 SP names, 8 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M13 (Sales Reports, Analytics & Dashboards).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadSC, TotalSummaryDAL.BranchwiseLoadSummaryBLL, TotalSummaryDAL.GetReportTable affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
