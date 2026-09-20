---
page_id: PAGE-376
module: M10
page: SampleStockforWarehouse
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-376 — SampleStockforWarehouse

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-376 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | SampleStockforWarehouse |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SampleStockforWarehouse |
| aspx size | 33327 bytes |
| Code-behind LOC | 335 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SampleStockforWarehouse** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/SampleStockForWareHouseView.aspx`

Outbound references: `samplestockforwarehouseview.aspx`, `whfreezestockrelease.aspx`

## 5. Route
`~/SInventory_UI/SampleStockforWarehouse.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx |
| Code-behind | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | SampleStockForWHBll (Library.BLL/SInventory_BLL/SampleStockForWHBll.cs) |
| DAL class | SampleStockForWHDal (Library.DAL/SInventory_DAL/SampleStockForWHDal.cs) |
| DAO/entity class | SampleStockForWHDetails (Library.DAO/SInventory_Entities/SampleStockForWHDetails.cs) |
| DAO/entity class | SampleStockForWareHouseMaster (Library.DAO/SInventory_Entities/SampleStockForWareHouseMaster.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→viewLinkButton_OnClick |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| DistributioncenterDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→DistributioncenterDropDownList1_OnSelectedIndexChanged |
| ActionDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| DateTextBox | TextBox |  | no validator found |  |  |  |  |
| productDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| Button2 | LinkButton |  | no validator found |  |  |  | OnClick→Button1_Click |
| DerectStoctGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Stock Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | ReceiveDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Convention Stock | no validator found |  |  |  |  |
| ConventionTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→dQtyTextBox_TextChanged |
| FilteredTextBoxExtenderconvRate | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| addButton | LinkButton |  | no validator found |  |  |  | OnClick→addButton_Click |
| ProductGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Receive Date | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Expiry Date | no validator found |  |  |  |  |
| (no id) | BoundField | Current Stock | no validator found |  |  |  |  |
| (no id) | BoundField | Convention Stock | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove Item | no validator found |  |  |  |  |
| DeleteImageButton | ImageButton |  | no validator found |  |  |  | onclick→DeleteImageButton_Click |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_Click1 |
| LinkButton4 | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| ImageButton3 | ImageButton |  | no validator found |  |  |  | onclick→ListImageButton_Click |
| DistributioncenterDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→DistributioncenterDropDownList1_OnSelectedIndexChanged |
| ActionDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| DateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| productDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| Button2 | Button | Search Product | no validator found |  |  |  | onclick→Button1_Click |
| DerectStoctGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Stock Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | ReceiveDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Convention Stock | no validator found |  |  |  |  |
| ConventionTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→dQtyTextBox_TextChanged |
| FilteredTextBoxExtenderconvRate | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| addButton | Button | Add | no validator found |  |  |  | onclick→addButton_Click |
| ProductGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Receive Date | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Expiry Date | no validator found |  |  |  |  |
| (no id) | BoundField | Current Stock | no validator found |  |  |  |  |
| (no id) | BoundField | Convention Stock | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove Item | no validator found |  |  |  |  |
| DeleteImageButton | ImageButton |  | no validator found |  |  |  | onclick→DeleteImageButton_Click |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click1 |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| productDropDownList | dtdata | SInventory_UI_SampleStockforWarehouse.Button1_Click | SampleStockForWHBll.GetProductDcStore | SampleStockForWHDal.GetProductDcStore | UNKNOWN | UNKNOWN | UNKNOWN |
| DistributioncenterDropDownList1 | SampleStockForWareHouseMaster.WareHouseId | SInventory_UI_SampleStockforWarehouse.submitButton_Click1 | SampleStockForWHBll.SaveDataForSubDcStockOutMasterBll, SampleStockForWHBll.SaveDataForSubStockOutDetailBll, SampleStockF | SampleStockForWHDal.SaveDataForStockOutDetailDal, SampleStockForWHDal.SaveDataForSubDcStockOutMaster, SampleStockForWHDa | UNKNOWN | UNKNOWN | UNKNOWN |
| ActionDropDownList | SampleStockForWareHouseMaster.Action | SInventory_UI_SampleStockforWarehouse.submitButton_Click1 | SampleStockForWHBll.SaveDataForSubDcStockOutMasterBll, SampleStockForWHBll.SaveDataForSubStockOutDetailBll, SampleStockF | SampleStockForWHDal.SaveDataForStockOutDetailDal, SampleStockForWHDal.SaveDataForSubDcStockOutMaster, SampleStockForWHDa | UNKNOWN | UNKNOWN | UNKNOWN |
| DateTextBox | SampleStockForWareHouseMaster.Date | SInventory_UI_SampleStockforWarehouse.submitButton_Click1 | SampleStockForWHBll.SaveDataForSubDcStockOutMasterBll, SampleStockForWHBll.SaveDataForSubStockOutDetailBll, SampleStockF | SampleStockForWHDal.SaveDataForStockOutDetailDal, SampleStockForWHDal.SaveDataForSubDcStockOutMaster, SampleStockForWHDa | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| DistributioncenterDropDownList1 | Page_Load → SampleStockForWHBll.SalesCenterLoadBll | inline SQL | Com/WearhouseId | tblWearhouse |
| ActionDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| productDropDownList | Page_Load → SampleStockForWHBll.ProductLoadBll | inline SQL | Pro/ProductId | tblProduct |
| DistributioncenterDropDownList1 | Page_Load → SampleStockForWHBll.SalesCenterLoadBll | inline SQL | Com/WearhouseId | tblWearhouse |
| ActionDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| productDropDownList | Page_Load → SampleStockForWHBll.ProductLoadBll | inline SQL | Pro/ProductId | tblProduct |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton | — | viewLinkButton_OnClick | — | — | — | — | UNKNOWN (handler not resolved) |
| Button2 | LinkButton click | (postback) | SInventory_UI_SampleStockforWarehouse.Button1_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:196 | SampleStockForWHBll.GetProductDcStore | SampleStockForWHDal.GetProductDcStore | — | read-only / none detected |
| addButton | LinkButton click | (postback) | SInventory_UI_SampleStockforWarehouse.addButton_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:62 | — | — | — | read-only / none detected |
| submitButton | LinkButton click | (postback) | SInventory_UI_SampleStockforWarehouse.submitButton_Click1 | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:288 | SampleStockForWHBll.SaveDataForSubDcStockOutMasterBll, SampleStockForWHBll.SaveDataForSubStockOutDet | SampleStockForWHDal.SaveDataForStockOutDetailDal, SampleStockForWHDal.SaveDataForSubDcStockOutMaster | — | writes tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster |
| LinkButton4 | LinkButton click | (postback) | SInventory_UI_SampleStockforWarehouse.cancelButton_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:38 | — | — | — | read-only / none detected |

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
| SInventory_UI_SampleStockforWarehouse.Page_Load | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.Page_Load → SampleStockForWHBll.ProductLoadBll / SampleStockForWHBll.SalesCenterLoadBll → SampleStockForWHDal.ProductLoadDal / SampleStockForWHDal.SalesCenterLoadDal | no | no |
| SInventory_UI_SampleStockforWarehouse.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:26 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_SampleStockforWarehouse.cancelButton_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:38 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.cancelButton_Click | no | no |
| SInventory_UI_SampleStockforWarehouse.ListImageButton_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:43 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.ListImageButton_Click | no | no |
| SInventory_UI_SampleStockforWarehouse.addButton_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:62 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.addButton_Click | no | no |
| SInventory_UI_SampleStockforWarehouse.DeleteImageButton_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:114 | object sender, ImageClickEventArgs e | SInventory_UI_SampleStockforWarehouse.DeleteImageButton_Click | no | no |
| SInventory_UI_SampleStockforWarehouse.dQtyTextBox_TextChanged | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:158 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.dQtyTextBox_TextChanged | no | no |
| SInventory_UI_SampleStockforWarehouse.Button1_Click | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:196 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.Button1_Click → SampleStockForWHBll.GetProductDcStore → SampleStockForWHDal.GetProductDcStore | no | no |
| SInventory_UI_SampleStockforWarehouse.DistributioncenterDropDownList1_OnSelectedIndexChanged | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:211 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.DistributioncenterDropDownList1_OnSelectedIndexChanged | no | no |
| SInventory_UI_SampleStockforWarehouse.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:219 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_SampleStockforWarehouse.submitButton_Click1 | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs:288 | object sender, EventArgs e | SInventory_UI_SampleStockforWarehouse.submitButton_Click1 → SampleStockForWHBll.SaveDataForSubDcStockOutMasterBll / SampleStockForWHBll.SaveDataForSubStockOutDetailBll / SampleStockForWHBll.StockOutDetail / SampleStockForWHBll.SubStockOutMasterCodeGenerator →  | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SampleStockForWHBll.ProductLoadBll | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:22 |
| SampleStockForWHBll.SalesCenterLoadBll | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:28 |
| SampleStockForWHBll.GetProductDcStore | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:33 |
| SampleStockForWHBll.SaveDataForSubDcStockOutMasterBll | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:39 |
| SampleStockForWHBll.SubStockOutMasterCodeGenerator | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:46 |
| SampleStockForWHBll.SaveDataForSubStockOutDetailBll | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:84 |
| SampleStockForWHBll.StockOutDetail | Library.BLL/SInventory_BLL/SampleStockForWHBll.cs:95 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SampleStockForWHDal.ProductLoadDal | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:27 | inline SQL: tblProduct |  |
| SampleStockForWHDal.SalesCenterLoadDal | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:21 | inline SQL: tblWearhouse |  |
| SampleStockForWHDal.GetProductDcStore | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:33 | inline SQL: tblCentralStore, tblProduct | @ProductId |
| SampleStockForWHDal.SaveDataForSubDcStockOutMaster | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:47 | inline SQL: tblSampleStockForWareHouseMaster | @Action, @Date, @EntryBy, @EntryDate, @SampleStockForWHMasterId, @SampleStockForWareHouseMstCode, @S |
| SampleStockForWHDal.SaveDataForStockOutDetailDal | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:65 | inline SQL: tblSampleStockForWareHouseDetails | @BatchNo, @ExpDate, @ProductCode, @ProductName, @ReceiveDate, @ReceiveId, @SampleStock, @SampleStock |
| SampleStockForWHDal.StockOutCentral | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:85 | inline SQL: tblCentralStore | @ReceiveId, @SampleStock |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCentralStore, tblProduct, tblWearhouse
- Tables WRITTEN (inline SQL reachable): tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblSampleStockForWareHouseDetails | BatchNo | @BatchNo |
| tblSampleStockForWareHouseDetails | ExpDate | @ExpDate |
| tblSampleStockForWareHouseDetails | ProductCode | @ProductCode |
| tblSampleStockForWareHouseDetails | ProductName | @ProductName |
| tblSampleStockForWareHouseDetails | ReceiveDate | @ReceiveDate |
| tblSampleStockForWareHouseDetails | ReceiveId | @ReceiveId |
| tblSampleStockForWareHouseDetails | SampleStock | @SampleStock |
| tblSampleStockForWareHouseDetails | SampleStockForWHDetailsId | @SampleStockForWHDetailsId |
| tblSampleStockForWareHouseDetails | SampleStockForWHMasterId | @SampleStockForWHMasterId |
| tblSampleStockForWareHouseMaster | Action | @Action |
| tblSampleStockForWareHouseMaster | Date | @Date |
| tblSampleStockForWareHouseMaster | EntryBy | @EntryBy |
| tblSampleStockForWareHouseMaster | EntryDate | @EntryDate |
| tblSampleStockForWareHouseMaster | SampleStockForWHMasterId | @SampleStockForWHMasterId |
| tblSampleStockForWareHouseMaster | SampleStockForWareHouseMstCode | @SampleStockForWareHouseMstCode |
| tblSampleStockForWareHouseMaster | Status | @Status |
| tblSampleStockForWareHouseMaster | WareHouseId | @WareHouseId |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Add product and Qty | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Insert  Date!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Action !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Distribution center!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Product!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ComUnitId, LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 15 methods.
- Redirect targets: WhFreezeStockRelease.aspx, ~/SInventory_UI/SampleStockForWareHouseView.aspx
- User messages: Please Add product and Qty; Please Insert  Date!!; Please Select Action !!; Please Select Distribution center!!; Please Select Product!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: EntryBy, EntryDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: SampleStockForWHBll; DAL: SampleStockForWHDal; DAO: SampleStockForWHDetails, SampleStockForWareHouseMaster; Tables (inline): tblCentralStore, tblProduct, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster, tblWearhouse

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewLinkButton with valid data | Handler runs; rows written to tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster |
| TC-05 | Click Button2 with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click Button2 with valid data | Handler runs; rows written to tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster |
| TC-07 | Click addButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click addButton with valid data | Handler runs; rows written to tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster |
| TC-09 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click submitButton with valid data | Handler runs; rows written to tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster |
| TC-11 | Click LinkButton4 with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click LinkButton4 with valid data | Handler runs; rows written to tblCentralStore, tblSampleStockForWareHouseDetails, tblSampleStockForWareHouseMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx`, `Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 77 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SampleStockforWarehouse.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods SampleStockForWHDal.GetProductDcStore, SampleStockForWHDal.ProductLoadDal, SampleStockForWHDal.SalesCenterLoadDal, SampleStockForWHDal.SaveDataForStockOutDetailDal, SampleStockForWHDal.SaveDataForSubDcStockOutMaster affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
