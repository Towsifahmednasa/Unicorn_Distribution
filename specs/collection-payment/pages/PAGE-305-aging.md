---
page_id: PAGE-305
module: M09
page: Aging
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-305 — Aging

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-305 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | Aging |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_Aging |
| aspx size | 20367 bytes |
| Code-behind LOC | 282 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **Aging** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `businesssummaryviewer.aspx`

## 5. Route
`~/SInventory_UI/Aging.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/Aging.aspx |
| Code-behind | Solution.Web/SInventory_UI/Aging.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| dcDropDownList2 | DropDownList |  | no validator found |  |  | see §9 |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| territoryDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| fromDate | CalendarExtender |  | no validator found |  |  |  |  |
| imgDateFrom | ImageButton |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| toDate | CalendarExtender |  | no validator found |  |  |  |  |
| imgDateTo | ImageButton |  | no validator found |  |  |  |  |
| viewRptButton | Button | Search | no validator found |  |  |  | OnClick→viewRptButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Order No | no validator found |  |  |  |  |
| (no id) | BoundField | Order Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | FE Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO CODE | no validator found |  |  |  |  |
| (no id) | BoundField | MIO NAME | no validator found |  |  |  |  |
| (no id) | BoundField | Mia Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | Intransit Day | no validator found |  |  |  |  |
| (no id) | BoundField | Type | no validator found |  |  |  |  |
| (no id) | BoundField | TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total TP Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total Net Amount | no validator found |  |  |  |  |
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
| dcDropDownList2 | aDataTable | SInventory_UI_Aging.LoadInfo | TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_Aging.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_Aging.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | headerTable | SInventory_UI_Aging.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rptTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| dcDropDownList2 | DropDownlist → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| salesCenterDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| territoryDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewRptButton | Button click | (postback) | SInventory_UI_Aging.viewRptButton_Click | Solution.Web/SInventory_UI/Aging.aspx.cs:112 | TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | sp_Get_AgingReceivableReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| excelButton1 | Button click | (postback) | SInventory_UI_Aging.btnExportToExcel_Click | Solution.Web/SInventory_UI/Aging.aspx.cs:212 | — | — | — | read-only / none detected |

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
| SInventory_UI_Aging.Page_Load | Solution.Web/SInventory_UI/Aging.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_Aging.Page_Load → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_Aging.loadGridView_OnRowCreated | Solution.Web/SInventory_UI/Aging.aspx.cs:26 | object sender, GridViewRowEventArgs e | SInventory_UI_Aging.loadGridView_OnRowCreated | no | no |
| SInventory_UI_Aging.DropDownlist | Solution.Web/SInventory_UI/Aging.aspx.cs:102 |  | SInventory_UI_Aging.DropDownlist → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_Aging.viewRptButton_Click | Solution.Web/SInventory_UI/Aging.aspx.cs:112 | object sender, EventArgs e | SInventory_UI_Aging.viewRptButton_Click → TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew → TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew → SP: sp_Get_AgingReceivableReport | no | no |
| SInventory_UI_Aging.LoadInfo | Solution.Web/SInventory_UI/Aging.aspx.cs:117 |  | SInventory_UI_Aging.LoadInfo → TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew → TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew → SP: sp_Get_AgingReceivableReport | yes | no |
| SInventory_UI_Aging.excelButton1_Click | Solution.Web/SInventory_UI/Aging.aspx.cs:168 | object sender, EventArgs e | SInventory_UI_Aging.excelButton1_Click | no | no |
| SInventory_UI_Aging.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/Aging.aspx.cs:190 | object sender, EventArgs e | SInventory_UI_Aging.rptTypeDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_Aging.btnExportToExcel_Click | Solution.Web/SInventory_UI/Aging.aspx.cs:212 | object sender, EventArgs e | SInventory_UI_Aging.btnExportToExcel_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| TotalSummaryBLL.LoadSummaryProductcodewiseGyashNew | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:181 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:256 | sp_Get_AgingReceivableReport | @districtId, @fromdate, @todate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_AgingReceivableReport | TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler excelButton1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 11 methods.
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
BLL: OtherStockActionBLL, TotalSummaryBLL; DAL: OtherStockActionDAL, TotalSummaryDAL; DAO: —; Tables (inline): tblCompanyUnit, tblUserCompanyUnit

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

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/Aging.aspx`, `Solution.Web/SInventory_UI/Aging.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/Aging.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 81 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/Aging.aspx |
| Data access | This document §15–17 | 1 SP names, 2 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: PAGE-398, PAGE-406
- Changing DAL methods OtherStockActionDAL.DCLoad, TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
