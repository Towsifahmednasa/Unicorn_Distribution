---
page_id: PAGE-368
module: M10
page: DepotStockAdjustmentsVoucherView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-368 — DepotStockAdjustmentsVoucherView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-368 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | DepotStockAdjustmentsVoucherView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DirectStockOutView |
| aspx size | 7024 bytes |
| Code-behind LOC | 121 |
| Inline script size | 2175 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DepotStockAdjustmentsVoucherView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/DepotStockAdjustmentsVoucher.aspx`

Outbound references: `custmasterentry.aspx`, `customermasteredit.aspx`, `customermasterviewer.aspx`, `dcstockoutreportviewer.aspx`, `depotstockadjustmentsvoucher.aspx`

## 5. Route
`~/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx |
| Code-behind | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DeStockOutBLL (Library.BLL/SInventory_BLL/DeStockOutBLL.cs) |
| DAL class | DeStockOutDal (Library.DAL/SInventory_DAL/DeStockOutDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | ComUnit Name | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Reason | no validator found |  |  |  |  |
| (no id) | BoundField | StockOutDate  | no validator found |  |  |  |  |
| (no id) | BoundField | Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return GetConfirmation(); |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| reportImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| editImageButton | ImageButton | return GetConfirmation(); | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: GetConfirmation
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_DirectStockOutView.Page_Load | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_DirectStockOutView.Page_Load → DeStockOutBLL.DcStockOutBll → DeStockOutDal.DcStockOutViewDal | no | no |
| SInventory_UI_DirectStockOutView.DcStockOutgrid | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:34 |  | SInventory_UI_DirectStockOutView.DcStockOutgrid → DeStockOutBLL.DcStockOutBll → DeStockOutDal.DcStockOutViewDal | no | no |
| SInventory_UI_DirectStockOutView.loadGridView_RowCommand | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:41 | object sender, GridViewCommandEventArgs e | SInventory_UI_DirectStockOutView.loadGridView_RowCommand → DeStockOutBLL.DcStockOutBll / DeStockOutBLL.DcStockOutDetailsDelete / DeStockOutBLL.DcStockOutMasterDelete → DeStockOutDal.DcStockOutDetailsDeleteDal / DeStockOutDal.DcStockOutMasterDeleteDal / DeStock | no | no |
| SInventory_UI_DirectStockOutView.CustMasterNewImageButton_Click | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:90 | object sender, ImageClickEventArgs e | SInventory_UI_DirectStockOutView.CustMasterNewImageButton_Click | no | no |
| SInventory_UI_DirectStockOutView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:94 | object sender, EventArgs e | SInventory_UI_DirectStockOutView.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_DirectStockOutView.DcStockOutAddImageButton_Click | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:105 | object sender, ImageClickEventArgs e | SInventory_UI_DirectStockOutView.DcStockOutAddImageButton_Click | no | no |
| SInventory_UI_DirectStockOutView.rptImageButton_Click | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:110 | object sender, ImageClickEventArgs e | SInventory_UI_DirectStockOutView.rptImageButton_Click | no | no |
| SInventory_UI_DirectStockOutView.Button1_Click | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs:118 | object sender, EventArgs e | SInventory_UI_DirectStockOutView.Button1_Click → DeStockOutBLL.DcStockOutBll → DeStockOutDal.DcStockOutViewDal | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DeStockOutBLL.DcStockOutBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:57 |
| DeStockOutBLL.DcStockOutMasterDelete | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:67 |
| DeStockOutBLL.DcStockOutDetailsDelete | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:62 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DeStockOutDal.DcStockOutViewDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:84 | inline SQL: tblCompanyUnit, tblDeStockOutMaster, tblInvoice |  |
| DeStockOutDal.DcStockOutMasterDeleteDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:95 | inline SQL: tblDeStockOutMaster, tblDeStockOutMaster | @DcStockOutMasterId |
| DeStockOutDal.DcStockOutDetailsDeleteDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:106 | inline SQL: tblDeStockOutDetails, tblDeStockOutDetails | @DcStockOutMasterId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblDeStockOutDetails, tblDeStockOutMaster, tblInvoice
- Tables WRITTEN (inline SQL reachable): tblDeStockOutDetails, tblDeStockOutMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Can not Delete Data!!!.... | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DcStockOutMasterId, Status
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
- Redirect targets: CustMasterEntry.aspx, DepotStockAdjustmentsVoucher.aspx
- User messages: Can not Delete Data!!!....
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DeStockOutBLL; DAL: DeStockOutDal; DAO: —; Tables (inline): tblCompanyUnit, tblDeStockOutDetails, tblDeStockOutMaster, tblInvoice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx`, `Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 16 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucherView.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DeStockOutDal.DcStockOutDetailsDeleteDal, DeStockOutDal.DcStockOutMasterDeleteDal, DeStockOutDal.DcStockOutViewDal affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
