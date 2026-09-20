---
page_id: PAGE-226
module: M06
page: OrderListView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-226 — OrderListView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-226 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderListView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_AreaView |
| aspx size | 18135 bytes |
| Code-behind LOC | 224 |
| Inline script size | 998 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderListView** (List / View). Page title/heading text: `Order Details Preview`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ManualOrderCreation.aspx`

Outbound references: `areaedit.aspx`, `areaentry.aspx`, `manualordercreation.aspx`

## 5. Route
`~/SInventory_UI/OrderListView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OrderListView.aspx |
| Code-behind | Solution.Web/SInventory_UI/OrderListView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| (no id) | a |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center / DC | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Value (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | TemplateField | Action | no validator found |  |  |  |  |
| btnPreview | LinkButton |  | no validator found |  |  |  |  |
| (no id) | a | Edit Order | no validator found |  |  |  |  |
| lblModalOrderCode | Label |  | no validator found |  |  |  |  |
| lblModalOrderDate | Label |  | no validator found |  |  |  |  |
| lblModalDC | Label |  | no validator found |  |  |  |  |
| lblModalCustomer | Label |  | no validator found |  |  |  |  |
| lblModalSalesPerson | Label |  | no validator found |  |  |  |  |
| lblModalAddress | Label |  | no validator found |  |  |  |  |
| lblModalCellNo | Label |  | no validator found |  |  |  |  |
| lblModalRemarks | Label |  | no validator found |  |  |  |  |
| gvModalItems | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | # | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Unit TP (Tk) | no validator found |  |  |  |  |
| (no id) | BoundField | Total TP (Tk) | no validator found |  |  |  |  |
| (no id) | BoundField | Unit VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Total VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount (Tk) | no validator found |  |  |  |  |
| lblModalTotalQty | Label |  | no validator found |  |  |  |  |
| lblModalTotalTP | Label |  | no validator found |  |  |  |  |
| lblModalTotalVAT | Label |  | no validator found |  |  |  |  |
| lblModalNetPayable | Label |  | no validator found |  |  |  |  |
| lnkModalEdit | HyperLink |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| excelButton1 | Button click | (postback) | SInventory_UI_AreaView.btnExportToExcel_Click | Solution.Web/SInventory_UI/OrderListView.aspx.cs:155 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: initializeDataTable
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: yes; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_AreaView.Page_Load | Solution.Web/SInventory_UI/OrderListView.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_AreaView.Page_Load → OrderInfoBLL.LoadOrderWithDetail → OrderInfoDAL.LoadOrderWithDetail | no | no |
| SInventory_UI_AreaView.AreaLoad | Solution.Web/SInventory_UI/OrderListView.aspx.cs:26 |  | SInventory_UI_AreaView.AreaLoad → OrderInfoBLL.LoadOrderWithDetail → OrderInfoDAL.LoadOrderWithDetail | no | no |
| SInventory_UI_AreaView.loadGridView_RowCommand | Solution.Web/SInventory_UI/OrderListView.aspx.cs:36 | object sender, GridViewCommandEventArgs e | SInventory_UI_AreaView.loadGridView_RowCommand → OrderInfoBLL.LoadOrderWithDetail → OrderInfoDAL.LoadOrderWithDetail | no | no |
| SInventory_UI_AreaView.ShowOrderPreview | Solution.Web/SInventory_UI/OrderListView.aspx.cs:51 | string orderId | SInventory_UI_AreaView.ShowOrderPreview → OrderInfoBLL.LoadOrderWithDetail → OrderInfoDAL.LoadOrderWithDetail | no | no |
| SInventory_UI_AreaView.areaInfoNewImageButton_Click | Solution.Web/SInventory_UI/OrderListView.aspx.cs:126 | object sender, ImageClickEventArgs e | SInventory_UI_AreaView.areaInfoNewImageButton_Click | no | no |
| SInventory_UI_AreaView.areaReloadImageButton_Click | Solution.Web/SInventory_UI/OrderListView.aspx.cs:130 | object sender, ImageClickEventArgs e | SInventory_UI_AreaView.areaReloadImageButton_Click → OrderInfoBLL.LoadOrderWithDetail → OrderInfoDAL.LoadOrderWithDetail | no | no |
| SInventory_UI_AreaView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/OrderListView.aspx.cs:140 | object sender, EventArgs e | SInventory_UI_AreaView.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_AreaView.btnExportToExcel_Click | Solution.Web/SInventory_UI/OrderListView.aspx.cs:155 | object sender, EventArgs e | SInventory_UI_AreaView.btnExportToExcel_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadOrderWithDetail | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:184 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadOrderWithDetail | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1185 | inline SQL: tblCompanyUnit, tblCustMaster, tblOrder |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblCustMaster, tblMarket, tblOrder, tblOrderDetail, tblProduct
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
- try/catch present in 0 of 10 methods.
- Redirect targets: AreaEntry.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL; DAL: OrderInfoDAL; DAO: —; Tables (inline): tblCompanyUnit, tblCustMaster, tblMarket, tblOrder, tblOrderDetail, tblProduct

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OrderListView.aspx`, `Solution.Web/SInventory_UI/OrderListView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/OrderListView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 44 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/OrderListView.aspx |
| Data access | This document §15–17 | 0 SP names, 6 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadOrderWithDetail affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
