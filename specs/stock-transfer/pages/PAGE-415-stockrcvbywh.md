---
page_id: PAGE-415
module: M11
page: StockRcvByWH
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-415 — StockRcvByWH

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-415 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | StockRcvByWH |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_StockRcvByWH |
| aspx size | 7150 bytes |
| Code-behind LOC | 54 |
| Inline script size | 1192 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockRcvByWH** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ReceiveProductByChalanByWh.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/StockRcvByWH.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockRcvByWH.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | SCtoWHTransferDal (Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| stockInTraGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | ChalanNo | no validator found |  |  |  |  |
| (no id) | BoundField | ChalanDate | no validator found |  |  |  |  |
| (no id) | BoundField | TruckNo | no validator found |  |  |  |  |
| (no id) | BoundField | DriverName | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList | aTable | SInventory_UI_StockRcvByWH.searchButton_Click | — | SCtoWHTransferDal.GetWarehouseRcvStock | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList | LoadWarhehouse → SCtoWHTransferDal.WareHouseLoad | inline SQL | WearhouseName/WearhouseId | tblWearhouse |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| searchButton | LinkButton click | (postback) | SInventory_UI_StockRcvByWH.searchButton_Click | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:30 | — | SCtoWHTransferDal.GetWarehouseRcvStock | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_StockRcvByWH.Unnamed_Click | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:51 | — | — | — | read-only / none detected |

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
| SInventory_UI_StockRcvByWH.Page_Load | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_StockRcvByWH.Page_Load → SCtoWHTransferDal.WareHouseLoad | no | no |
| SInventory_UI_StockRcvByWH.LoadWarhehouse | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:24 |  | SInventory_UI_StockRcvByWH.LoadWarhehouse → SCtoWHTransferDal.WareHouseLoad | no | no |
| SInventory_UI_StockRcvByWH.searchButton_Click | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:30 | object sender, EventArgs e | SInventory_UI_StockRcvByWH.searchButton_Click → SCtoWHTransferDal.GetWarehouseRcvStock | no | no |
| SInventory_UI_StockRcvByWH.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:39 | object sender, EventArgs e | SInventory_UI_StockRcvByWH.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_StockRcvByWH.Unnamed_Click | Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs:51 | object sender, EventArgs e | SInventory_UI_StockRcvByWH.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SCtoWHTransferDal.WareHouseLoad | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:20 | inline SQL: tblWearhouse |  |
| SCtoWHTransferDal.GetWarehouseRcvStock | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:134 | inline SQL: tblDepotToWHChalanInfo |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblDepotToWHChalanInfo, tblWearhouse
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
- try/catch present in 0 of 5 methods.
- Redirect targets: StockRcvByWH.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: SCtoWHTransferDal; DAO: —; Tables (inline): tblDepotToWHChalanInfo, tblWearhouse

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockRcvByWH.aspx`, `Solution.Web/SInventory_UI/StockRcvByWH.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockRcvByWH.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 14 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockRcvByWH.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods SCtoWHTransferDal.GetWarehouseRcvStock, SCtoWHTransferDal.WareHouseLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
