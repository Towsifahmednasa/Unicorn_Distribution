---
page_id: PAGE-423
module: M11
page: TransferReceiveProductByChalanByDC
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-423 — TransferReceiveProductByChalanByDC

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-423 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | TransferReceiveProductByChalanByDC |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ReceiveProductByChalanByDC |
| aspx size | 12182 bytes |
| Code-behind LOC | 193 |
| Inline script size | 867 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TransferReceiveProductByChalanByDC** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `transferstockreceivebydc.aspx`, `transferstockreceivereportviewer.aspx`

## 5. Route
`~/SInventory_UI/TransferReceiveProductByChalanByDC.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx |
| Code-behind | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ChalanBLL (Library.BLL/SInventory_BLL/ChalanBLL.cs) |
| DAL class | ChalanDAL (Library.DAL/SInventory_DAL/ChalanDAL.cs) |
| DAL class | SAP_IntrigationPointDAL (Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs) |
| DAO/entity class | DCStockNew (Library.DAO/SInventory_Entities/DCStockNew.cs) |
| DAO/entity class | DCStoreFreezeDAO (Library.DAO/SInventory_Entities/DCStoreFreezeDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  |  |
| clnNoTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| clnDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| rcvDateTextBox | TextBox |  | no validator found |  |  |  |  |
| truckTextBox | TextBox |  | no validator found |  |  |  |  |
| driverNameTextBox | TextBox |  | no validator found |  |  |  |  |
| rcvGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | Exp Date | no validator found |  |  |  |  |
| (no id) | BoundField | Receive Date | no validator found |  |  |  |  |
| (no id) | TemplateField | Rcv Qty | no validator found |  |  |  |  |
| rcvQtyTextBox | TextBox | <%# Eval("Quantity")%> | no validator found |  | ReadOnly=True |  | OnTextChanged→rcvQtyTextBox_TextChanged |
| FilteredTextBoxExtender3 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | Un Rcv Qty | no validator found |  | Visible=False |  |  |
| damageTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→damageTextBox_TextChanged |
| FilteredTexstBoxExtender3 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | BoundField | Mfg date | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→submitButton_Click |
| ResetBtn | LinkButton |  | no validator found |  |  |  | OnClick→ResetBtn_Click |
| hdComUnitId | HiddenField |  | no validator found |  |  |  |  |
| hdReqId | HiddenField |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| rcvQtyTextBox | DCStockNew.StockQty | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @StockQty | UNKNOWN (SP body not in repo) | UNKNOWN |
| damageTextBox | DCStockNew.DamageQty | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @DamageQty | UNKNOWN (SP body not in repo) | UNKNOWN |
| rcvDateTextBox | DCStockNew.StockRcvDate | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @StockRcvDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| clnNoTextBox | DCStockNew.ChalanNo | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.ChalanUpdate, ChalanDAL.DCStockInDALMain, ChalanDAL.SaveDCStoreFreeze2, SAP_IntrigationPointDAL.MakeRESTReques | UNKNOWN | UNKNOWN | UNKNOWN |
| clnDateTextBox | DCStockNew.ChalanDate | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @ChalanDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| hdComUnitId | DCStockNew.ComUnitId | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @ComUnitId | UNKNOWN (SP body not in repo) | UNKNOWN |
| damageTextBox | DCStockNew.TotalQuantity | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @TotalQuantity | UNKNOWN (SP body not in repo) | UNKNOWN |
| damageTextBox | DCStockNew.StockQty | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.SaveDCStoreFreeze2 | @StockQty | UNKNOWN (SP body not in repo) | UNKNOWN |
| clnNoTextBox | Res | SInventory_UI_ReceiveProductByChalanByDC.Save | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.ChalanUpdate, ChalanDAL.DCStockInDALMain, ChalanDAL.SaveDCStoreFreeze2, SAP_IntrigationPointDAL.MakeRESTReques | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_ReceiveProductByChalanByDC.submitButton_Click | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:138 | ChalanBLL.DCStockIn2, ChalanBLL.SaveDCStoreFreeze2 | ChalanDAL.ChalanUpdate, ChalanDAL.DCStockInDALMain, ChalanDAL.SaveDCStoreFreeze2, SAP_IntrigationPoi | MakeRESTRequest | writes tblChalanInfo, tblDCStore, tblDCStoreFreeze |
| ResetBtn | LinkButton click | (postback) | SInventory_UI_ReceiveProductByChalanByDC.ResetBtn_Click | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:189 | — | — | — | read-only / none detected |

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
| SInventory_UI_ReceiveProductByChalanByDC.Page_Load | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.Page_Load → ChalanBLL.GetChalanReceieve → ChalanDAL.GetChalanReceieve | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.LoadInfo | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:32 | string reqId | SInventory_UI_ReceiveProductByChalanByDC.LoadInfo → ChalanBLL.GetChalanReceieve → ChalanDAL.GetChalanReceieve | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.LoadGrid | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:43 | string reqId | SInventory_UI_ReceiveProductByChalanByDC.LoadGrid → ChalanBLL.GetChalanReceieve → ChalanDAL.GetChalanReceieve | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:49 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.Save | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:60 |  | SInventory_UI_ReceiveProductByChalanByDC.Save → ChalanBLL.DCStockIn2 / ChalanBLL.SaveDCStoreFreeze2 → ChalanDAL.ChalanUpdate / ChalanDAL.DCStockInDALMain / ChalanDAL.SaveDCStoreFreeze2 / SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan → SP: MakeRESTRe | yes | yes |
| SInventory_UI_ReceiveProductByChalanByDC.submitButton_Click | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:138 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.submitButton_Click → ChalanBLL.DCStockIn2 / ChalanBLL.SaveDCStoreFreeze2 → ChalanDAL.ChalanUpdate / ChalanDAL.DCStockInDALMain / ChalanDAL.SaveDCStoreFreeze2 / SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan →  | no | yes |
| SInventory_UI_ReceiveProductByChalanByDC.damageTextBox_TextChanged | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:162 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.damageTextBox_TextChanged | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.rcvQtyTextBox_TextChanged | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:173 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.rcvQtyTextBox_TextChanged | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.backLinkButton_Click | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:184 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.backLinkButton_Click | no | no |
| SInventory_UI_ReceiveProductByChalanByDC.ResetBtn_Click | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs:189 | object sender, EventArgs e | SInventory_UI_ReceiveProductByChalanByDC.ResetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ChalanBLL.GetChalanReceieve | Library.BLL/SInventory_BLL/ChalanBLL.cs:76 |
| ChalanBLL.DCStockIn2 | Library.BLL/SInventory_BLL/ChalanBLL.cs:26 |
| ChalanBLL.SaveDCStoreFreeze2 | Library.BLL/SInventory_BLL/ChalanBLL.cs:48 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ChalanDAL.GetChalanReceieve | Library.DAL/SInventory_DAL/ChalanDAL.cs:276 | inline SQL: tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblDCStore | @ChalanId |
| ChalanDAL.DCStockInDALMain | Library.DAL/SInventory_DAL/ChalanDAL.cs:123 | inline SQL: tblDCStore, tblChalanDetail, tblDCStore, tblProduct | @ChalanDate, @ChalanDetailsId, @ChalanNo, @ComUnitId, @StockQty |
| ChalanDAL.ChalanUpdate | Library.DAL/SInventory_DAL/ChalanDAL.cs:63 | inline SQL: tblChalanInfo | @ChalanId |
| ChalanDAL.SaveDCStoreFreeze2 | Library.DAL/SInventory_DAL/ChalanDAL.cs:79 | inline SQL: tblDCStoreFreeze | @BatchNo, @ChalanDate, @ChalanDetailsId, @ChalanNo, @ComUnitId, @DCStoreFreezeId, @DCStoreId, @Damag |
| SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:549 | MakeRESTRequest | @RcvBy, @StoNo |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| MakeRESTRequest | SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblDCStore, tblProduct, tblUnitPrice
- Tables WRITTEN (inline SQL reachable): tblChalanInfo, tblDCStore, tblDCStoreFreeze
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblDCStoreFreeze | BatchNo | @BatchNo |
| tblDCStoreFreeze | ChalanDate | @ChalanDate |
| tblDCStoreFreeze | ChalanDetailsId | @ChalanDetailsId |
| tblDCStoreFreeze | ChalanNo | @ChalanNo |
| tblDCStoreFreeze | ComUnitId | @ComUnitId |
| tblDCStoreFreeze | DCStoreFreezeId | @DCStoreFreezeId |
| tblDCStoreFreeze | DCStoreId | @DCStoreId |
| tblDCStoreFreeze | DamageQty | @DamageQty |
| tblDCStoreFreeze | ExpDate | @ExpDate |
| tblDCStoreFreeze | PackSize | @PackSize |
| tblDCStoreFreeze | ProductCode | @ProductCode |
| tblDCStoreFreeze | ProductName | @ProductName |
| tblDCStoreFreeze | ReceiveDate | @ReceiveDate |
| tblDCStoreFreeze | StockCondition | @StockCondition |
| tblDCStoreFreeze | StockQty | @StockQty |
| tblDCStoreFreeze | StockRcvDate | @StockRcvDate |
| tblDCStoreFreeze | StorageLocation | @StorageLocation |
| tblDCStoreFreeze | TotalQuantity | @TotalQuantity |

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 13 methods.
- Redirect targets: TransferStockReceiveByDC.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
SAP_IntrigationPointDAL

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ChalanBLL; DAL: ChalanDAL, SAP_IntrigationPointDAL; DAO: DCStockNew, DCStoreFreezeDAO, ResultInfo; Tables (inline): tblChalanDetail, tblChalanInfo, tblCompanyUnit, tblDCStore, tblDCStoreFreeze, tblProduct, tblUnitPrice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tblChalanInfo, tblDCStore, tblDCStoreFreeze |
| TC-05 | Click ResetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click ResetBtn with valid data | Handler runs; rows written to tblChalanInfo, tblDCStore, tblDCStoreFreeze |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx`, `Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 30 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/TransferReceiveProductByChalanByDC.aspx |
| Data access | This document §15–17 | 1 SP names, 7 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: PAGE-413
- Changing DAL methods ChalanDAL.ChalanUpdate, ChalanDAL.DCStockInDALMain, ChalanDAL.GetChalanReceieve, ChalanDAL.SaveDCStoreFreeze2, SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
