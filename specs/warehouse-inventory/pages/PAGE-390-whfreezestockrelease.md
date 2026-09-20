---
page_id: PAGE-390
module: M10
page: WhFreezeStockRelease
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-390 — WhFreezeStockRelease

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-390 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WhFreezeStockRelease |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WhFreezeStockRelease |
| aspx size | 20347 bytes |
| Code-behind LOC | 297 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **WhFreezeStockRelease** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/OrderRequisitionView.aspx`, `SInventory_UI/SampleStockforWarehouse.aspx`, `SInventory_UI/WarehouseStockOut.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/WhFreezeStockRelease.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx |
| Code-behind | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | WhFreezeStockReleaseBll (Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs) |
| DAL class | WhFreezeStockReleaseDal (Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs) |
| DAO/entity class | WhStoreFreezeDao (Library.DAO/SInventory_Entities/WhStoreFreezeDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| whDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| submitButton0 | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_OnClick |
| LinkButton4 | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | ProductCode | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | StockQty | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | StockCondition | no validator found |  |  |  |  |
| (no id) | TemplateField | Return Qty | no validator found |  |  |  |  |
| returnQtyTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→returnQtyTextBox_OnTextChanged |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| whDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | Button | Search | no validator found |  |  |  | OnClick→searchButton_Click |
| submitButton0 | Button | Submit | no validator found |  |  |  | OnClick→submitButton_OnClick |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | ProductCode | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | StockQty | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | StockCondition | no validator found |  |  |  |  |
| (no id) | TemplateField | Return Qty | no validator found |  |  |  |  |
| returnQtyTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→returnQtyTextBox_OnTextChanged |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| whDropDownList | LoadDropdownList → WhFreezeStockReleaseBll.LoadWhInfoOnDropDownList | inline SQL | WearhouseName/WearhouseId | tblWearhouse |
| whDropDownList | LoadDropdownList → WhFreezeStockReleaseBll.LoadWhInfoOnDropDownList | inline SQL | WearhouseName/WearhouseId | tblWearhouse |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_WhFreezeStockRelease.searchButton_Click | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:287 | WhFreezeStockReleaseBll.LoadWhFreezeStockData | WhFreezeStockReleaseDal.GetWhFreezeStockData | — | read-only / none detected |
| submitButton0 | LinkButton click | (postback) | SInventory_UI_WhFreezeStockRelease.submitButton_OnClick | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:163 | WhFreezeStockReleaseBll.LoadCentralStoreData, WhFreezeStockReleaseBll.LoadStockReleaseData, WhFreeze | WhFreezeStockReleaseDal.GetCentralStoreDataByReceiveId, WhFreezeStockReleaseDal.GetStockReleaseData, | — | writes tblCentralStore, tblWhStoreFreeze |
| LinkButton4 | LinkButton click | (postback) | SInventory_UI_WhFreezeStockRelease.cancelButton_Click | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:36 | — | — | — | read-only / none detected |
| submitButton | Button click | (postback) | SInventory_UI_WhFreezeStockRelease.searchButton_Click | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:287 | WhFreezeStockReleaseBll.LoadWhFreezeStockData | WhFreezeStockReleaseDal.GetWhFreezeStockData | — | read-only / none detected |
| submitButton0 | Button click | (postback) | SInventory_UI_WhFreezeStockRelease.submitButton_OnClick | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:163 | WhFreezeStockReleaseBll.LoadCentralStoreData, WhFreezeStockReleaseBll.LoadStockReleaseData, WhFreeze | WhFreezeStockReleaseDal.GetCentralStoreDataByReceiveId, WhFreezeStockReleaseDal.GetStockReleaseData, | — | writes tblCentralStore, tblWhStoreFreeze |

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
| SInventory_UI_WhFreezeStockRelease.Page_Load | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.Page_Load → WhFreezeStockReleaseBll.LoadWhInfoOnDropDownList → WhFreezeStockReleaseDal.GetWhInfoOnDropDownList | no | no |
| SInventory_UI_WhFreezeStockRelease.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:24 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_WhFreezeStockRelease.cancelButton_Click | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:36 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.cancelButton_Click | no | no |
| SInventory_UI_WhFreezeStockRelease.LoadDropdownList | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:40 |  | SInventory_UI_WhFreezeStockRelease.LoadDropdownList → WhFreezeStockReleaseBll.LoadWhInfoOnDropDownList → WhFreezeStockReleaseDal.GetWhInfoOnDropDownList | no | no |
| SInventory_UI_WhFreezeStockRelease.LoadWhFreezeStockInfo | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:45 |  | SInventory_UI_WhFreezeStockRelease.LoadWhFreezeStockInfo → WhFreezeStockReleaseBll.LoadWhFreezeStockData → WhFreezeStockReleaseDal.GetWhFreezeStockData | no | no |
| SInventory_UI_WhFreezeStockRelease.LoadWhFreezeStockInfo2 | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:61 |  | SInventory_UI_WhFreezeStockRelease.LoadWhFreezeStockInfo2 → WhFreezeStockReleaseBll.LoadWhFreezeStockData → WhFreezeStockReleaseDal.GetWhFreezeStockData | no | no |
| SInventory_UI_WhFreezeStockRelease.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:84 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_WhFreezeStockRelease.submitButton_OnClick | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:163 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.submitButton_OnClick → WhFreezeStockReleaseBll.LoadCentralStoreData / WhFreezeStockReleaseBll.LoadStockReleaseData / WhFreezeStockReleaseBll.LoadWhFreezeStockData / WhFreezeStockReleaseBll.UpdateCentalStoreInfo / WhFreezeSt → | no | no |
| SInventory_UI_WhFreezeStockRelease.returnQtyTextBox_OnTextChanged | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:222 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.returnQtyTextBox_OnTextChanged | no | no |
| SInventory_UI_WhFreezeStockRelease.searchButton_Click | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs:287 | object sender, EventArgs e | SInventory_UI_WhFreezeStockRelease.searchButton_Click → WhFreezeStockReleaseBll.LoadWhFreezeStockData → WhFreezeStockReleaseDal.GetWhFreezeStockData | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WhFreezeStockReleaseBll.LoadWhInfoOnDropDownList | Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs:42 |
| WhFreezeStockReleaseBll.LoadWhFreezeStockData | Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs:17 |
| WhFreezeStockReleaseBll.LoadStockReleaseData | Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs:22 |
| WhFreezeStockReleaseBll.UpdateWhStoreFreezeInfo | Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs:27 |
| WhFreezeStockReleaseBll.LoadCentralStoreData | Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs:32 |
| WhFreezeStockReleaseBll.UpdateCentalStoreInfo | Library.BLL/SInventory_BLL/WhFreezeStockReleaseBll.cs:37 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WhFreezeStockReleaseDal.GetWhInfoOnDropDownList | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:68 | inline SQL: tblWearhouse |  |
| WhFreezeStockReleaseDal.GetWhFreezeStockData | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:18 | inline SQL: tblProduct, tblUnitPrice, tblWhStoreFreeze |  |
| WhFreezeStockReleaseDal.GetStockReleaseData | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:27 | inline SQL: tblProduct, tblUnitPrice, tblWhStoreFreeze | @WhStoreFreezeId |
| WhFreezeStockReleaseDal.UpdateWhStoreFreezeStockQuantity | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:39 | inline SQL: tblWhStoreFreeze | @StockQty, @WhStoreFreezeId |
| WhFreezeStockReleaseDal.GetCentralStoreDataByReceiveId | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:49 | inline SQL: tblCentralStore | @ReceiveId |
| WhFreezeStockReleaseDal.UpdateCentalStoreQuantity | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:58 | inline SQL: tblCentralStore | @Quantity, @ReceiveId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCentralStore, tblProduct, tblUnitPrice, tblWearhouse, tblWhStoreFreeze
- Tables WRITTEN (inline SQL reachable): tblCentralStore, tblWhStoreFreeze
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCentralStore | Quantity | @Quantity |
| tblWhStoreFreeze | StockQty | @StockQty |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select a return quantity!!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | You must check at least one row!!! | UNKNOWN |
| (handler returnQtyTextBox_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select a valid value for return quantity!! | UNKNOWN |
| (handler returnQtyTextBox_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Return quantity must be equal or less than current stock quantity!! | UNKNOWN |
| (handler returnQtyTextBox_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Return quantity must be greater than zero(0) !! | UNKNOWN |
| (handler searchButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select an Warehouse!!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 14 methods.
- Redirect targets: WhFreezeStockRelease.aspx
- User messages: Please select a return quantity!!!; Please select a valid value for return quantity!!; Please select an Warehouse!!!; Return quantity must be equal or less than current stock quantity!!; Return quantity must be greater than zero(0) !!; You must check at least one row!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: WhFreezeStockReleaseBll; DAL: WhFreezeStockReleaseDal; DAO: WhStoreFreezeDao; Tables (inline): tblCentralStore, tblProduct, tblUnitPrice, tblWearhouse, tblWhStoreFreeze

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tblCentralStore, tblWhStoreFreeze |
| TC-05 | Click submitButton0 with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton0 with valid data | Handler runs; rows written to tblCentralStore, tblWhStoreFreeze |
| TC-07 | Click LinkButton4 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click LinkButton4 with valid data | Handler runs; rows written to tblCentralStore, tblWhStoreFreeze |
| TC-09 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click submitButton with valid data | Handler runs; rows written to tblCentralStore, tblWhStoreFreeze |
| TC-11 | Click submitButton0 with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click submitButton0 with valid data | Handler runs; rows written to tblCentralStore, tblWhStoreFreeze |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx`, `Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 43 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/WhFreezeStockRelease.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods WhFreezeStockReleaseDal.GetCentralStoreDataByReceiveId, WhFreezeStockReleaseDal.GetStockReleaseData, WhFreezeStockReleaseDal.GetWhFreezeStockData, WhFreezeStockReleaseDal.GetWhInfoOnDropDownList, WhFreezeStockReleaseDal.UpdateCentalStoreQuantity affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
