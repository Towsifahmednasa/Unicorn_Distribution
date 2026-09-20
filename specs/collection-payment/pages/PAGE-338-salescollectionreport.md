---
page_id: PAGE-338
module: M09
page: SalesCollectionReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-338 — SalesCollectionReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-338 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | SalesCollectionReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SalesCollectionReport |
| aspx size | 13261 bytes |
| Code-behind LOC | 309 |
| Inline script size | 816 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SalesCollectionReport** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/SalesCollectionReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SalesCollectionReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| txtDateFrom | TextBox | Select Start Date | no validator found |  |  |  |  |
| txtDateTo | TextBox | Select End Date | no validator found |  |  |  |  |
| ddlDA | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlCustomer | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlSalesPerson | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlPaymentChannel | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlRegion | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProduct | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPageIndexChanging→loadGridView_PageIndexChanging, OnPreRender→gv_PreRender |
| (no id) | BoundField | Month | no validator found |  |  |  |  |
| (no id) | BoundField | Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Narration | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Address | no validator found |  |  |  |  |
| (no id) | BoundField | Region | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | Product Rate | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Bank Amount | no validator found |  |  |  |  |
| (no id) | BoundField | bKash Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Hand Cash | no validator found |  |  |  |  |
| (no id) | BoundField | Delivery Agent (DA) | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Person | no validator found |  |  |  |  |
| lblTotalBank | Label | 0.00 | no validator found |  |  |  |  |
| lblTotalbKash | Label | 0.00 | no validator found |  |  |  |  |
| lblTotalHandCash | Label | 0.00 | no validator found |  |  |  |  |
| lblGrandTotal | Label | 0.00 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlPaymentChannel | channel | SInventory_UI_SalesCollectionReport.BuildParmString | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlDA | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlCustomer | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlSalesPerson | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlPaymentChannel | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlRegion | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProduct | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_SalesCollectionReport.SearchButton_Click | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:215 | — | CmnCrystaltoView.GetSalesCollectionReportDAL | sp_Get_SalesCollectionReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| cancelButton | LinkButton click | (postback) | SInventory_UI_SalesCollectionReport.cancelButton_Click | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:221 | — | CmnCrystaltoView.GetSalesCollectionReportDAL | sp_Get_SalesCollectionReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnExport | LinkButton click | (postback) | SInventory_UI_SalesCollectionReport.btnExport_Click | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:249 | — | CmnCrystaltoView.GetSalesCollectionReportDAL | sp_Get_SalesCollectionReport | SP executed — effect on tables UNKNOWN (body not in repo) |

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
| SInventory_UI_SalesCollectionReport.Page_Load | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_SalesCollectionReport.Page_Load → CmnCrystaltoView.GetCustomerListDAL / CmnCrystaltoView.GetDAListDAL / CmnCrystaltoView.GetOrderSenderListDAL / CmnCrystaltoView.GetProductListDAL / CmnCrystaltoView.GetRegionListDAL | no | no |
| SInventory_UI_SalesCollectionReport.LoadDropDowns | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:26 |  | SInventory_UI_SalesCollectionReport.LoadDropDowns → CmnCrystaltoView.GetCustomerListDAL / CmnCrystaltoView.GetDAListDAL / CmnCrystaltoView.GetOrderSenderListDAL / CmnCrystaltoView.GetProductListDAL / CmnCrystaltoView.GetRegionListDAL | yes | no |
| SInventory_UI_SalesCollectionReport.LoadData | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:76 |  | SInventory_UI_SalesCollectionReport.LoadData → CmnCrystaltoView.GetSalesCollectionReportDAL → SP: sp_Get_SalesCollectionReport | yes | no |
| SInventory_UI_SalesCollectionReport.BuildParmString | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:154 |  | SInventory_UI_SalesCollectionReport.BuildParmString | no | no |
| SInventory_UI_SalesCollectionReport.SearchButton_Click | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:215 | object sender, EventArgs e | SInventory_UI_SalesCollectionReport.SearchButton_Click → CmnCrystaltoView.GetSalesCollectionReportDAL → SP: sp_Get_SalesCollectionReport | no | no |
| SInventory_UI_SalesCollectionReport.cancelButton_Click | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:221 | object sender, EventArgs e | SInventory_UI_SalesCollectionReport.cancelButton_Click → CmnCrystaltoView.GetSalesCollectionReportDAL → SP: sp_Get_SalesCollectionReport | no | no |
| SInventory_UI_SalesCollectionReport.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:234 | object sender, GridViewPageEventArgs e | SInventory_UI_SalesCollectionReport.loadGridView_PageIndexChanging → CmnCrystaltoView.GetSalesCollectionReportDAL → SP: sp_Get_SalesCollectionReport | no | no |
| SInventory_UI_SalesCollectionReport.gv_PreRender | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:240 | object sender, EventArgs e | SInventory_UI_SalesCollectionReport.gv_PreRender | no | no |
| SInventory_UI_SalesCollectionReport.btnExport_Click | Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs:249 | object sender, EventArgs e | SInventory_UI_SalesCollectionReport.btnExport_Click → CmnCrystaltoView.GetSalesCollectionReportDAL → SP: sp_Get_SalesCollectionReport | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CmnCrystaltoView.GetDAListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1930 | inline SQL: tblDAInfo |  |
| CmnCrystaltoView.GetCustomerListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1826 | inline SQL: tblCustMaster |  |
| CmnCrystaltoView.GetOrderSenderListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1880 | inline SQL: tblEmpGeneralInfo |  |
| CmnCrystaltoView.GetRegionListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1948 | inline SQL: tblRegion |  |
| CmnCrystaltoView.GetProductListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1862 | inline SQL: tblProduct |  |
| CmnCrystaltoView.GetSalesCollectionReportDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1911 | sp_Get_SalesCollectionReport | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_SalesCollectionReport | CmnCrystaltoView.GetSalesCollectionReportDAL | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCustMaster, tblCustPayDetail, tblDAInfo, tblEmpGeneralInfo, tblProduct, tblRegion
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadDropDowns) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error loading filters: | UNKNOWN |
| (handler LoadData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error loading data: | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 10 methods.
- Redirect targets: none
- User messages: Error loading data:; Error loading filters:
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CmnCrystaltoView; DAO: —; Tables (inline): tblCustMaster, tblCustPayDetail, tblDAInfo, tblEmpGeneralInfo, tblProduct, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click cancelButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click cancelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SalesCollectionReport.aspx`, `Solution.Web/SInventory_UI/SalesCollectionReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SalesCollectionReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 39 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SalesCollectionReport.aspx |
| Data access | This document §15–17 | 1 SP names, 6 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Inline SQL in code-behind without SqlParameter (possible string-built SQL — verify): BuildParmString

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CmnCrystaltoView.GetCustomerListDAL, CmnCrystaltoView.GetDAListDAL, CmnCrystaltoView.GetOrderSenderListDAL, CmnCrystaltoView.GetProductListDAL, CmnCrystaltoView.GetRegionListDAL affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
