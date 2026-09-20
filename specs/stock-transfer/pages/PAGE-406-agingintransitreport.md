---
page_id: PAGE-406
module: M11
page: AgingInTransitReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-406 — AgingInTransitReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-406 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | AgingInTransitReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_AgingInTransitReport |
| aspx size | 23318 bytes |
| Code-behind LOC | 280 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **AgingInTransitReport** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `agingintransitreportviewer.aspx`

## 5. Route
`~/SInventory_UI/AgingInTransitReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/AgingInTransitReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| UpdateProgress4 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait11 | Image |  | no validator found |  |  |  |  |
| CheckBox1 | CheckBox |  | no validator found |  |  |  | oncheckedchanged→CheckBox1_CheckedChanged |
| dcDropDownList2 | DropDownList |  | no validator found |  |  | see §9 |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→dcDropDownList1_SelectedIndexChanged |
| InvoiceDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDatse | ImageButton |  | no validator found |  |  |  |  |
| todateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDatsea | ImageButton |  | no validator found |  |  |  |  |
| SearchButton | Button | View Report | no validator found |  |  |  | onclick→SearchButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| excelButton | Button | Excel | no validator found |  |  |  | OnClick→excelButton_OnClick |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Unit Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Order No | no validator found |  |  |  |  |
| (no id) | BoundField | Order Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Area Code | no validator found |  |  |  |  |
| (no id) | BoundField | Mia Code | no validator found |  |  |  |  |
| (no id) | BoundField | District Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | Intransit Day | no validator found |  |  |  |  |
| (no id) | BoundField | Main MIO CODE | no validator found |  |  |  |  |
| (no id) | BoundField | Main MIO NAME | no validator found |  |  |  |  |
| (no id) | BoundField | Special Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | No of Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Invoice Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Return Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Sales Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Collection | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Outstanding Amt. | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList2 | aDataTable | SInventory_UI_AgingInTransitReport.SearchButton_Click | TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_AgingInTransitReport.SearchButton_Click | TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_AgingInTransitReport.SearchButton_Click | TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | headerTable | SInventory_UI_AgingInTransitReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_AgingInTransitReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_AgingInTransitReport.excelButton_OnClick | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList2 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| dcDropDownList1 | LoadDropDown → OrderInfoBLL.LoadDZSM | inline SQL | RegionName/RegionCode | tblregion |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| excelButton | Button click | (postback) | SInventory_UI_AgingInTransitReport.excelButton_OnClick | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:113 | — | — | — | read-only / none detected |

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
| SInventory_UI_AgingInTransitReport.Page_Load | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_AgingInTransitReport.Page_Load → OrderInfoBLL.LoadDZSM / OtherStockActionBLL.DCLoad → OrderInfoDAL.LoadDZSM / OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_AgingInTransitReport.LoadDropDown | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:30 |  | SInventory_UI_AgingInTransitReport.LoadDropDown → OrderInfoBLL.LoadDZSM / OtherStockActionBLL.DCLoad → OrderInfoDAL.LoadDZSM / OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_AgingInTransitReport.SearchButton_Click | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:43 | object sender, EventArgs e | SInventory_UI_AgingInTransitReport.SearchButton_Click → TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew → TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew → SP: sp_Get_AgingReceivableReport | no | no |
| SInventory_UI_AgingInTransitReport.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:95 | object sender, EventArgs e | SInventory_UI_AgingInTransitReport.dcDropDownList1_SelectedIndexChanged | no | no |
| SInventory_UI_AgingInTransitReport.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:99 | object sender, EventArgs e | SInventory_UI_AgingInTransitReport.CheckBox1_CheckedChanged | no | no |
| SInventory_UI_AgingInTransitReport.excelButton_OnClick | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:113 | object sender, EventArgs e | SInventory_UI_AgingInTransitReport.excelButton_OnClick | no | no |
| SInventory_UI_AgingInTransitReport.loadGridView_OnRowCreated | Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs:205 | object sender, GridViewRowEventArgs e | SInventory_UI_AgingInTransitReport.loadGridView_OnRowCreated | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| OrderInfoBLL.LoadDZSM | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:113 |
| TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:181 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| OrderInfoDAL.LoadDZSM | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1981 | inline SQL: tblregion |  |
| TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:256 | sp_Get_AgingReceivableReport | @districtId, @fromdate, @todate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_AgingReceivableReport | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit, tblregion
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
- try/catch present in 0 of 9 methods.
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
BLL: OrderInfoBLL, OtherStockActionBLL, TotalSummaryBLL; DAL: OrderInfoDAL, OtherStockActionDAL, TotalSummaryDAL; DAO: —; Tables (inline): tblCompanyUnit, tblUserCompanyUnit, tblregion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click excelButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click excelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/AgingInTransitReport.aspx`, `Solution.Web/SInventory_UI/AgingInTransitReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/AgingInTransitReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 76 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/AgingInTransitReport.aspx |
| Data access | This document §15–17 | 1 SP names, 3 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: PAGE-305, PAGE-398
- Changing DAL methods OrderInfoDAL.LoadDZSM, OtherStockActionDAL.DCLoad, TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
