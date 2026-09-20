---
page_id: PAGE-349
module: M10
page: DCStockReportViewer
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-349 — DCStockReportViewer

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-349 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | DCStockReportViewer |
| Kind | Report viewer (INFERRED from name) |
| Web folder | SInventory_RPTVIEW |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_RPTVIEW_DCStockReportViewer |
| aspx size | 4572 bytes |
| Code-behind LOC | 259 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DCStockReportViewer** (Report viewer). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/DCStockReport.aspx`, `SInventory_UI/StockReport.aspx`, `SubDepot_UI/SubDeportStockReport.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_RPTVIEW/DCStockReportViewer.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx |
| Code-behind | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | DCStockReportBLL (Library.BLL/SInventory_BLL/DCStockReportBLL.cs) |
| BLL class | StockTransportOrderReportBLL (Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs) |
| DAL class | DCStockReportDAL (Library.DAL/SInventory_DAL/DCStockReportDAL.cs) |
| DAL class | StockTransportOrderReportDAL (Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs) |
| Report (.rpt) | rptDCStock.rpt |
| Report (.rpt) | rptWHDCStock.rpt |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| crvDCStockRpt | CR:CrystalReportViewer |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_RPTVIEW_DCStockReportViewer.Page_Init | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx.cs:21 | object sender, EventArgs e | SInventory_RPTVIEW_DCStockReportViewer.Page_Init → DCStockReportBLL.DCReportDetailDataDAL / DCStockReportBLL.DCReportMainDataDAL / DCStockReportBLL.WHReportDetailDataDAL / StockTransportOrderReportBLL.CompanyInfoBLL → DCStockReportDAL.DCReportDetailDataDAL / D | no | no |
| SInventory_RPTVIEW_DCStockReportViewer.closeButton_Click | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx.cs:217 | object sender, EventArgs e | SInventory_RPTVIEW_DCStockReportViewer.closeButton_Click | no | no |
| SInventory_RPTVIEW_DCStockReportViewer.crvDCStockRpt_Unload | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx.cs:239 | object sender, EventArgs e | SInventory_RPTVIEW_DCStockReportViewer.crvDCStockRpt_Unload | no | no |
| SInventory_RPTVIEW_DCStockReportViewer.crvDCStockRpt_Disposed | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx.cs:250 | object sender, EventArgs e | SInventory_RPTVIEW_DCStockReportViewer.crvDCStockRpt_Disposed | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DCStockReportBLL.DCReportDetailDataDAL | Library.BLL/SInventory_BLL/DCStockReportBLL.cs:31 |
| DCStockReportBLL.DCReportMainDataDAL | Library.BLL/SInventory_BLL/DCStockReportBLL.cs:26 |
| StockTransportOrderReportBLL.CompanyInfoBLL | Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs:23 |
| DCStockReportBLL.WHReportDetailDataDAL | Library.BLL/SInventory_BLL/DCStockReportBLL.cs:35 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DCStockReportDAL.DCReportDetailDataDAL | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:37 | inline SQL: tblCentralStore, tblChalanDetail, tblChalanInfo, tblCompanyUnit |  |
| DCStockReportDAL.SingleParameter | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:555 | delegates / no SQL literal |  |
| DCStockReportDAL.DCReportMainDataDAL | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:31 | inline SQL: tblCompanyUnit |  |
| StockTransportOrderReportDAL.CompanyInformation | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs:15 | inline SQL: tblCompanyInfo |  |
| DCStockReportDAL.WHReportDetailDataDAL | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:311 | inline SQL: tblCentralStore, tblStockInTransfar, tblWHStockInDetail, tblWhStoreFreeze |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCentralStore, tblChalanDetail, tblChalanInfo, tblCompanyInfo, tblCompanyUnit, tblDCStore, tblDCStoreFreeze, tblInvoice, tblInvoiceDetail, tblProduct, tblRequisition, tblStockInTransfar, tblSubDepotChalanDetail, tblWHStockInDetail, tblWhStoreFreeze
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DC, ReportView
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 5 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
rptDCStock.rpt, rptWHDCStock.rpt

## 26. Dependencies
BLL: DCStockReportBLL, StockTransportOrderReportBLL; DAL: DCStockReportDAL, StockTransportOrderReportDAL; DAO: —; Tables (inline): tblCentralStore, tblChalanDetail, tblChalanInfo, tblCompanyInfo, tblCompanyUnit, tblDCStore, tblDCStoreFreeze, tblInvoice, tblInvoiceDetail, tblProduct, tblRequisition, tblStockInTransfar, tblSubDepotChalanDetail, tblWHStockInDetail, tblWhStoreFreeze

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx`, `Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 6 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_RPTVIEW/DCStockReportViewer.aspx |
| Data access | This document §15–17 | 0 SP names, 15 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DCStockReportDAL.DCReportDetailDataDAL, DCStockReportDAL.DCReportMainDataDAL, DCStockReportDAL.SingleParameter, DCStockReportDAL.WHReportDetailDataDAL, StockTransportOrderReportDAL.CompanyInformation affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
