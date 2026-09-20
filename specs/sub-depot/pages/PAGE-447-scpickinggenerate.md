---
page_id: PAGE-447
module: M12
page: SCPickingGenerate
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-447 — SCPickingGenerate

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-447 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SCPickingGenerate |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SCPickingGenerate |
| aspx size | 11209 bytes |
| Code-behind LOC | 133 |
| Inline script size | 1189 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SCPickingGenerate** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sub-depot info, stock in/out/transfer, invoicing, payments, top sheets, adjustments and freeze.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `subdeportmarketwisepicking.aspx`

## 5. Route
`~/SubDepot_UI/SCPickingGenerate.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | StockConditionFreezeBLL (Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | StockConditionFreezeDAL (Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→dcDropDownList1_SelectedIndexChanged |
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→manufacturerDropDownList_SelectedIndexChanged |
| MarketDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→MarketDropDownList1_SelectedIndexChanged |
| InvoiceDateTextBox | TextBox |  | no validator found |  |  |  |  |
| SearchButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | InvoiceNo | no validator found |  |  |  |  |
| (no id) | BoundField | InvoiceDate | no validator found |  |  |  |  |
| (no id) | BoundField | OrderNo | no validator found |  |  |  |  |
| (no id) | BoundField | OrderDate | no validator found |  |  |  |  |
| (no id) | BoundField | Total Amount | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList1 | dt | SubDepot_UI_SCPickingGenerate.SearchButton_Click | StockConditionFreezeBLL.LoadInvoiceSubdeport | StockConditionFreezeDAL.InvoiceFilterParameters, StockConditionFreezeDAL.LoadInvoiceSubdeport | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | url | SubDepot_UI_SCPickingGenerate.ViewButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacturerDropDownList | LoadDropDown → OtherStockActionBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |
| MarketDropDownList1 | dcDropDownList1_SelectedIndexChanged → OrderInfoBLL.LoadMarket | inline SQL | MarketName/MarketId | View_CustomerMaster, tblCompanyUnit, tblMarket |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| SearchButton | LinkButton click | (postback) | SubDepot_UI_SCPickingGenerate.SearchButton_Click | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:61 | StockConditionFreezeBLL.LoadInvoiceSubdeport | StockConditionFreezeDAL.InvoiceFilterParameters, StockConditionFreezeDAL.LoadInvoiceSubdeport | — | read-only / none detected |
|  | LinkButton click | (postback) | SubDepot_UI_SCPickingGenerate.Unnamed_Click | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:130 | — | — | — | read-only / none detected |

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
| SubDepot_UI_SCPickingGenerate.Page_Load | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:13 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.Page_Load → OtherStockActionBLL.DCLoad / OtherStockActionBLL.LoadmanufacturerName → OtherStockActionDAL.DCLoad / OtherStockActionDAL.LoadmanufacturerName | no | no |
| SubDepot_UI_SCPickingGenerate.LoadDropDown | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:20 |  | SubDepot_UI_SCPickingGenerate.LoadDropDown → OtherStockActionBLL.DCLoad / OtherStockActionBLL.LoadmanufacturerName → OtherStockActionDAL.DCLoad / OtherStockActionDAL.LoadmanufacturerName | no | no |
| SubDepot_UI_SCPickingGenerate.chkSelectAll_CheckedChanged | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:27 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.chkSelectAll_CheckedChanged | no | no |
| SubDepot_UI_SCPickingGenerate.dcDropDownList1_SelectedIndexChanged | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:44 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.dcDropDownList1_SelectedIndexChanged → OrderInfoBLL.LoadMarket → OrderInfoDAL.LoadMarket | no | no |
| SubDepot_UI_SCPickingGenerate.manufacturerDropDownList_SelectedIndexChanged | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:51 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.manufacturerDropDownList_SelectedIndexChanged | no | no |
| SubDepot_UI_SCPickingGenerate.MarketDropDownList1_SelectedIndexChanged | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:56 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.MarketDropDownList1_SelectedIndexChanged | no | no |
| SubDepot_UI_SCPickingGenerate.SearchButton_Click | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:61 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.SearchButton_Click → StockConditionFreezeBLL.LoadInvoiceSubdeport → StockConditionFreezeDAL.InvoiceFilterParameters / StockConditionFreezeDAL.LoadInvoiceSubdeport | no | no |
| SubDepot_UI_SCPickingGenerate.ViewButton_Click | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:97 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.ViewButton_Click | no | no |
| SubDepot_UI_SCPickingGenerate.Unnamed_Click | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs:130 | object sender, EventArgs e | SubDepot_UI_SCPickingGenerate.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:20 |
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |
| OrderInfoBLL.LoadMarket | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:310 |
| StockConditionFreezeBLL.LoadInvoiceSubdeport | Library.BLL/SInventory_BLL/StockConditionFreezeBLL.cs:112 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:33 | inline SQL: tblManufacturer |  |
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| OrderInfoDAL.LoadMarket | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2261 | inline SQL: View_CustomerMaster, tblCompanyUnit, tblMarket | @ComUnitId |
| StockConditionFreezeDAL.LoadInvoiceSubdeport | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:183 | inline SQL: View_CustomerMaster, tblMarket, tblProduct, tblSubInvoiceDetail |  |
| StockConditionFreezeDAL.InvoiceFilterParameters | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:251 | delegates / no SQL literal | @ComUnitId, @InvoiceDate, @ManufacId, @MarketId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): View_CustomerMaster, tblCompanyUnit, tblManufacturer, tblMarket, tblProduct, tblSubInvoiceDetail, tblSubInvoiceMaster, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: Manufac, Market, MrktRptParameter, UserId, invoicedate
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 11 methods.
- Redirect targets: SCPickingGenerate.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL, OtherStockActionBLL, StockConditionFreezeBLL; DAL: OrderInfoDAL, OtherStockActionDAL, StockConditionFreezeDAL; DAO: —; Tables (inline): View_CustomerMaster, tblCompanyUnit, tblManufacturer, tblMarket, tblProduct, tblSubInvoiceDetail, tblSubInvoiceMaster, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click SearchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click SearchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SCPickingGenerate.aspx`, `Solution.Web/SubDepot_UI/SCPickingGenerate.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 22 controls parsed | n/a | PARTIAL | Solution.Web/SubDepot_UI/SCPickingGenerate.aspx |
| Data access | This document §15–17 | 0 SP names, 8 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M12 (Sub-Depot Operations).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadMarket, OtherStockActionDAL.DCLoad, OtherStockActionDAL.LoadmanufacturerName, StockConditionFreezeDAL.InvoiceFilterParameters, StockConditionFreezeDAL.LoadInvoiceSubdeport affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
