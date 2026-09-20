---
page_id: PAGE-417
module: M11
page: StockReceiveByDC
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-417 — StockReceiveByDC

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-417 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | StockReceiveByDC |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_StockReceiveByDC |
| aspx size | 11350 bytes |
| Code-behind LOC | 89 |
| Inline script size | 971 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockReceiveByDC** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ReceiveProductByChalanByDC.aspx`

Outbound references: `login.aspx`

## 5. Route
`~/SInventory_UI/StockReceiveByDC.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockReceiveByDC.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| viewLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→viewLinkButton_OnClick |
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| stockInTraGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Chalan No | no validator found |  |  |  |  |
| (no id) | BoundField | Chalan Date | no validator found |  |  |  |  |
| (no id) | BoundField | Truck No | no validator found |  |  |  |  |
| (no id) | BoundField | Driver Name | no validator found |  |  |  |  |
| searchButton | Button | Search | no validator found |  |  |  | OnClick→searchButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList | Page_Load → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton | — | viewLinkButton_OnClick | — | — | — | — | UNKNOWN (handler not resolved) |
| submitButton | LinkButton click | (postback) | SInventory_UI_StockReceiveByDC.searchButton_Click | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:78 | RequisitionBLL.GetAllStockRcvByDc | RequisitionDAL.GetAllStockRcvByDcDAL | — | read-only / none detected |
| LinkButton1 | LinkButton click | (postback) | SInventory_UI_StockReceiveByDC.cancelButton_Click | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:37 | — | — | — | read-only / none detected |
| searchButton | Button click | (postback) | SInventory_UI_StockReceiveByDC.searchButton_Click | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:78 | RequisitionBLL.GetAllStockRcvByDc | RequisitionDAL.GetAllStockRcvByDcDAL | — | read-only / none detected |

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
| SInventory_UI_StockReceiveByDC.Page_Load | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_StockReceiveByDC.Page_Load → RequisitionBLL.DCLoad → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad | no | no |
| SInventory_UI_StockReceiveByDC.cancelButton_Click | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:37 | object sender, EventArgs e | SInventory_UI_StockReceiveByDC.cancelButton_Click | no | no |
| SInventory_UI_StockReceiveByDC.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:42 | object sender, EventArgs e | SInventory_UI_StockReceiveByDC.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_StockReceiveByDC.LoadGrid | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:53 | string comUnitId | SInventory_UI_StockReceiveByDC.LoadGrid → RequisitionBLL.GetAllStockRcvByDc → RequisitionDAL.GetAllStockRcvByDcDAL | no | no |
| SInventory_UI_StockReceiveByDC.searchButton_Click | Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs:78 | object sender, EventArgs e | SInventory_UI_StockReceiveByDC.searchButton_Click → RequisitionBLL.GetAllStockRcvByDc → RequisitionDAL.GetAllStockRcvByDcDAL | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| RequisitionBLL.GetAllStockRcvByDc | Library.BLL/SInventory_BLL/RequisitionBLL.cs:401 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| RequisitionDAL.GetAllStockRcvByDcDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:489 | inline SQL: tblRequisition | @ComUnitId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblRequisition, tblUserCompanyUnit
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
- Redirect targets: Login.aspx, StockReceiveByDC.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: RequisitionBLL; DAL: RequisitionDAL; DAO: —; Tables (inline): tblCompanyUnit, tblRequisition, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewLinkButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockReceiveByDC.aspx`, `Solution.Web/SInventory_UI/StockReceiveByDC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockReceiveByDC.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 17 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockReceiveByDC.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods RequisitionDAL.BindDropDown, RequisitionDAL.DCLoad, RequisitionDAL.GetAllStockRcvByDcDAL affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
