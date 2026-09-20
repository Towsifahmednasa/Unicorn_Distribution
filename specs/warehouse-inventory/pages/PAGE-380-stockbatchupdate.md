---
page_id: PAGE-380
module: M10
page: StockBatchUpdate
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-380 — StockBatchUpdate

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-380 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | StockBatchUpdate |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_StockConditionFreeze |
| aspx size | 11510 bytes |
| Code-behind LOC | 182 |
| Inline script size | 1192 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockBatchUpdate** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/StockBatchUpdate.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockBatchUpdate.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | StockBatchUpdateDal (Library.DAL/SInventory_DAL/StockBatchUpdateDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→dcDropDownList1_SelectedIndexChanged |
| submitButton0 | LinkButton |  | no validator found |  |  |  | OnClick→submitButton0_OnClick |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | TemplateField | Batch No | no validator found |  |  |  |  |
| batchTextBox | TextBox | <%# Eval("BatchNo")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Mfg. Date | no validator found |  |  |  |  |
| mfgDateTextBox | TextBox | <%# Eval("MfgDate")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Exp. Date | no validator found |  |  |  |  |
| expDateDateTextBox | TextBox | <%# Eval("ExpDate")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | C.Stock Qty | no validator found |  |  |  |  |
| txtStockQty | TextBox | <%# Eval("StockQty")%> | no validator found |  |  |  |  |
| (no id) | BoundField | C.Stock Qty | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList1 | aTable | SInventory_UI_StockConditionFreeze.dcDropDownList1_SelectedIndexChanged | — | StockBatchUpdateDal.LoadStockByDcId | UNKNOWN | UNKNOWN | UNKNOWN |
| txtStockQty | sQ | SInventory_UI_StockConditionFreeze.submitButton0_OnClick | — | StockBatchUpdateDal.LoadStockByDcId, StockBatchUpdateDal.UpdateStockBatch | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → OtherStockActionBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton0 | LinkButton click | (postback) | SInventory_UI_StockConditionFreeze.submitButton0_OnClick | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:62 | — | StockBatchUpdateDal.LoadStockByDcId, StockBatchUpdateDal.UpdateStockBatch | sp_UD_StockBatch_new | SP executed — effect on tables UNKNOWN (body not in repo) |
|  | LinkButton click | (postback) | SInventory_UI_StockConditionFreeze.Unnamed_Click | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:179 | — | — | — | read-only / none detected |

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
| SInventory_UI_StockConditionFreeze.Page_Load | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_StockConditionFreeze.Page_Load → OtherStockActionBLL.DCLoad / OtherStockActionBLL.LoadmanufacturerName → OtherStockActionDAL.DCLoad / OtherStockActionDAL.LoadmanufacturerName | no | no |
| SInventory_UI_StockConditionFreeze.LoadDropDown | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:26 |  | SInventory_UI_StockConditionFreeze.LoadDropDown → OtherStockActionBLL.DCLoad / OtherStockActionBLL.LoadmanufacturerName → OtherStockActionDAL.DCLoad / OtherStockActionDAL.LoadmanufacturerName | no | no |
| SInventory_UI_StockConditionFreeze.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:34 | object sender, EventArgs e | SInventory_UI_StockConditionFreeze.dcDropDownList1_SelectedIndexChanged → StockBatchUpdateDal.LoadStockByDcId | no | no |
| SInventory_UI_StockConditionFreeze.submitButton0_OnClick | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:62 | object sender, EventArgs e | SInventory_UI_StockConditionFreeze.submitButton0_OnClick → StockBatchUpdateDal.LoadStockByDcId / StockBatchUpdateDal.UpdateStockBatch → SP: sp_UD_StockBatch_new | yes | no |
| SInventory_UI_StockConditionFreeze.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:160 | object sender, EventArgs e | SInventory_UI_StockConditionFreeze.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_StockConditionFreeze.Unnamed_Click | Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs:179 | object sender, EventArgs e | SInventory_UI_StockConditionFreeze.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:20 |
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:33 | inline SQL: tblManufacturer |  |
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| StockBatchUpdateDal.LoadStockByDcId | Library.DAL/SInventory_DAL/StockBatchUpdateDal.cs:18 | inline SQL: tblDCStore | @ComUnitId |
| StockBatchUpdateDal.UpdateStockBatch | Library.DAL/SInventory_DAL/StockBatchUpdateDal.cs:27 | sp_UD_StockBatch_new | @StockQty, @batch, @dcStoreId, @expDate, @mfgdate, @updateBy |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_UD_StockBatch_new | StockBatchUpdateDal.UpdateStockBatch | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblDCStore, tblManufacturer, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select MFG Date !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select batch no !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select EXP Date !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | You must check at least one row!!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 8 methods.
- Redirect targets: StockBatchUpdate.aspx
- User messages: Please Select MFG Date !!!; Please Select batch no !!!; Please select EXP Date !!!; You must check at least one row!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL; DAL: OtherStockActionDAL, StockBatchUpdateDal; DAO: —; Tables (inline): tblCompanyUnit, tblDCStore, tblManufacturer, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton0 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton0 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockBatchUpdate.aspx`, `Solution.Web/SInventory_UI/StockBatchUpdate.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockBatchUpdate.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockBatchUpdate.aspx |
| Data access | This document §15–17 | 1 SP names, 4 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OtherStockActionDAL.DCLoad, OtherStockActionDAL.LoadmanufacturerName, StockBatchUpdateDal.LoadStockByDcId, StockBatchUpdateDal.UpdateStockBatch affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
