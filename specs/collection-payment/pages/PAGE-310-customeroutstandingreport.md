---
page_id: PAGE-310
module: M09
page: CustomerOutstandingReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-310 — CustomerOutstandingReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-310 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | CustomerOutstandingReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_CustomerOutstandingReport |
| aspx size | 14518 bytes |
| Code-behind LOC | 251 |
| Inline script size | 803 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerOutstandingReport** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerpaymentviewer.aspx`, `login.aspx`

## 5. Route
`~/SInventory_UI/CustomerOutstandingReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ../MasterSetup_UI/IVMarketStructure.ascx |
| User control | ~/SInventory_UI/IVMarketStructureInvoSearch.ascx |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| CheckBox1 | CheckBox | National Report | no validator found |  |  |  | oncheckedchanged→CheckBox1_CheckedChanged1 |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→salesCenterDropDownList_SelectedIndexChanged |
| fromDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  |  |
| todateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| paymentTermsDropdown | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| marketDropDownList | DropDownList |  | no validator found |  | Visible=false | see §9 |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Sales Center Code | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Programe Type | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Number | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Delivery Invoice Number | no validator found |  |  |  |  |
| (no id) | BoundField | Delivery Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Exp Date | no validator found |  |  |  |  |
| (no id) | BoundField | Sold Qty | no validator found |  |  |  |  |
| (no id) | BoundField |  Net Sales Amount | no validator found |  |  |  |  |
| (no id) | BoundField | VAT Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Trade Discount  | no validator found |  |  |  |  |
| (no id) | BoundField | Special Discount   | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | FE Code | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO CODE | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Type | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | fromDate | SInventory_UI_CustomerOutstandingReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_CustomerOutstandingReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| paymentTermsDropdown | url | SInventory_UI_CustomerOutstandingReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | param | SInventory_UI_CustomerOutstandingReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| paymentTermsDropdown | param | SInventory_UI_CustomerOutstandingReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | param | SInventory_UI_CustomerOutstandingReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| paymentTermsDropdown | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| marketDropDownList | salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadMarket | inline SQL | MarketName/MarketId | View_CustomerMaster, tblCompanyUnit, tblMarket |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_CustomerOutstandingReport.viewRptButton_Click | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:72 | — | — | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_CustomerOutstandingReport.cancelButton_Click | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:46 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_CustomerOutstandingReport.btnExport_Click | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:178 | — | — | — | read-only / none detected |

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
| SInventory_UI_CustomerOutstandingReport.Page_Load | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:20 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.Page_Load → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_CustomerOutstandingReport.cancelButton_Click | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:46 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.cancelButton_Click | no | no |
| SInventory_UI_CustomerOutstandingReport.CheckBox1_CheckedChanged1 | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:50 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.CheckBox1_CheckedChanged1 | no | no |
| SInventory_UI_CustomerOutstandingReport.DropDownlist | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:62 |  | SInventory_UI_CustomerOutstandingReport.DropDownlist → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_CustomerOutstandingReport.viewRptButton_Click | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:72 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.viewRptButton_Click | no | no |
| SInventory_UI_CustomerOutstandingReport.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:130 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadMarket → OrderInfoDAL.LoadMarket | no | no |
| SInventory_UI_CustomerOutstandingReport.btnExport_Click | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:178 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.btnExport_Click | no | no |
| SInventory_UI_CustomerOutstandingReport.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs:235 | object sender, EventArgs e | SInventory_UI_CustomerOutstandingReport.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadMarket | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:310 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadMarket | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2261 | inline SQL: View_CustomerMaster, tblCompanyUnit, tblMarket | @ComUnitId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): View_CustomerMaster, tblCompanyUnit, tblMarket, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler viewRptButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select Date Range and National CheckBox!! | UNKNOWN |
| (handler viewRptButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select Date Range and Sales Center Name!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: PaymentReport, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 12 methods.
- Redirect targets: CustomerOutstandingReport.aspx
- User messages: Select Date Range and National CheckBox!!; Select Date Range and Sales Center Name!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL; DAL: OrderInfoDAL; DAO: —; Tables (inline): View_CustomerMaster, tblCompanyUnit, tblMarket, tblUserCompanyUnit

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
- `Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx`, `Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 50 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerOutstandingReport.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadMarket, OrderInfoDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
