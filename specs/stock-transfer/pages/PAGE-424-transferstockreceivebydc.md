---
page_id: PAGE-424
module: M11
page: TransferStockReceiveByDC
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-424 — TransferStockReceiveByDC

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-424 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | TransferStockReceiveByDC |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_TransferStockReceiveByDC |
| aspx size | 12595 bytes |
| Code-behind LOC | 88 |
| Inline script size | 984 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TransferStockReceiveByDC** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx`, `SInventory_UI/TransferReceiveProductByChalanByDC.aspx`

Outbound references: `login.aspx`

## 5. Route
`~/SInventory_UI/TransferStockReceiveByDC.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx |
| Code-behind | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ChalanBLL (Library.BLL/SInventory_BLL/ChalanBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | ChalanDAL (Library.DAL/SInventory_DAL/ChalanDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| UpdateProgress1d | UpdateProgress |  | no validator found |  |  |  |  |
| imgWaitd | Image |  | no validator found |  |  |  |  |
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | LinkButton |  | no validator found |  |  |  | onclick→searchButton_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | onclick→cancelButton_Click |
| stockInTraGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Chalan No | no validator found |  |  |  |  |
| (no id) | BoundField | ChalanDate | no validator found |  |  |  |  |
| (no id) | BoundField | Truck No | no validator found |  |  |  |  |
| (no id) | BoundField | Driver Name | no validator found |  |  |  |  |
| searchButton | Button | Search | no validator found |  |  |  | onclick→searchButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList | Page_Load → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_TransferStockReceiveByDC.Page_Load | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs:13 | object sender, EventArgs e | SInventory_UI_TransferStockReceiveByDC.Page_Load → RequisitionBLL.DCLoad → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad | no | no |
| SInventory_UI_TransferStockReceiveByDC.cancelButton_Click | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs:35 | object sender, EventArgs e | SInventory_UI_TransferStockReceiveByDC.cancelButton_Click | no | no |
| SInventory_UI_TransferStockReceiveByDC.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs:40 | object sender, EventArgs e | SInventory_UI_TransferStockReceiveByDC.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_TransferStockReceiveByDC.LoadGrid | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs:52 | string comUnitId | SInventory_UI_TransferStockReceiveByDC.LoadGrid → ChalanBLL.ChalanLoadInReceive → ChalanDAL.ChalanLoadInReceive | no | no |
| SInventory_UI_TransferStockReceiveByDC.searchButton_Click | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs:77 | object sender, EventArgs e | SInventory_UI_TransferStockReceiveByDC.searchButton_Click → ChalanBLL.ChalanLoadInReceive → ChalanDAL.ChalanLoadInReceive | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| ChalanBLL.ChalanLoadInReceive | Library.BLL/SInventory_BLL/ChalanBLL.cs:67 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| ChalanDAL.ChalanLoadInReceive | Library.DAL/SInventory_DAL/ChalanDAL.cs:260 | inline SQL: tblChalanInfo, tblCompanyUnit | @ComUnitId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblChalanInfo, tblCompanyUnit, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ComUnitId, UserId, UserType
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 6 methods.
- Redirect targets: Login.aspx, TransferStockReceiveByDC.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ChalanBLL, RequisitionBLL; DAL: ChalanDAL, RequisitionDAL; DAO: —; Tables (inline): tblChalanInfo, tblCompanyUnit, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx`, `Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 18 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/TransferStockReceiveByDC.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ChalanDAL.ChalanLoadInReceive, RequisitionDAL.BindDropDown, RequisitionDAL.DCLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
