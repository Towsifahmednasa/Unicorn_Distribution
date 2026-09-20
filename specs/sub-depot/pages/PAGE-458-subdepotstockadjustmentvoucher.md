---
page_id: PAGE-458
module: M12
page: SubDepotStockAdjustmentVoucher
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-458 — SubDepotStockAdjustmentVoucher

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-458 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubDepotStockAdjustmentVoucher |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SubDepotStockAdjustmentVoucher |
| aspx size | 18014 bytes |
| Code-behind LOC | 423 |
| Inline script size | 1173 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SubDepotStockAdjustmentVoucher** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sub-depot info, stock in/out/transfer, invoicing, payments, top sheets, adjustments and freeze.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx`

Outbound references: `subdepotstockadjustmentvoucherview.aspx`

## 5. Route
`~/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | SubDepotStockAdjustmentsVoucherBll (Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs) |
| DAL class | SubDepotStockAdjustmentsVoucherDal (Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs) |
| DAO/entity class | SubdepotStockOutDetailsDao (Library.DAO/SubDepot_DAO/SubdepotStockOutDetailsDao.cs) |
| DAO/entity class | SubdepotStockOutMasterDao (Library.DAO/SubDepot_DAO/SubdepotStockOutMasterDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| DistributioncenterDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→DistributioncenterDropDownList1_OnSelectedIndexChanged |
| txtSubDepotInvoice | TextBox |  | no validator found |  |  |  | ontextchanged→SubDepotInvoice_TextChanged |
| SubDepotInvoice_AutoCompleteExtender | AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| ProformaInvoiceNumDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| StockOutTextBox | TextBox |  | no validator found |  |  |  |  |
| txtReason | TextBox |  | no validator found |  |  |  |  |
| productDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| Button2 | LinkButton |  | no validator found |  |  |  | OnClick→Button1_Click |
| DerectStoctOutGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Stock Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | ReceiveDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Stoct Out Qty | no validator found |  |  |  |  |
| transferQtyTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→dQtyTextBox_TextChanged |
| FilteredTextBoxExtenderconvRate | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| addButton | LinkButton |  | no validator found |  |  |  | onclick→addButton_Click |
| ProductGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Stock Out Qty | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | ReceiveDate | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove Item | no validator found |  |  |  |  |
| DeleteImageButton | ImageButton |  | no validator found |  |  |  | onclick→DeleteImageButton_Click |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→LinkButton1_Click |
| ComUnitCodeTextBox | TextBox |  | no validator found |  | Visible=false |  |  |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→submitButton_Click1 |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| txtSubDepotInvoice | Perticular | SubDepot_UI_SubDepotStockAdjustmentVoucher.SubDepotInvoice_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| HiddenField1 | id | SubDepot_UI_SubDepotStockAdjustmentVoucher.SubDepotInvoice_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| productDropDownList | dtdata | SubDepot_UI_SubDepotStockAdjustmentVoucher.Button1_Click | SubDepotStockAdjustmentsVoucherBll.GetProductDcStoreSubdeport | SubDepotStockAdjustmentsVoucherDal.GetProductDcStoreSubdeport | UNKNOWN | UNKNOWN | UNKNOWN |
| DistributioncenterDropDownList1 | unitCode | SubDepot_UI_SubDepotStockAdjustmentVoucher.DistributioncenterDropDownList1_OnSelectedIndexChanged | SubDepotStockAdjustmentsVoucherBll.ProformaInvoiceNumberBll | SubDepotStockAdjustmentsVoucherDal.ProformaInvoiceNumberDal | UNKNOWN | UNKNOWN | UNKNOWN |
| DistributioncenterDropDownList1 | SubdepotStockOutMasterDao.ComUnitId | SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 | SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll, SubDepotStockAdjustmentsVoucherBll.SaveDataForSubS | SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal, SubDepotStockAdjustmentsVoucherDal.SaveDataForSubDcStoc | UNKNOWN | UNKNOWN | UNKNOWN |
| ProformaInvoiceNumDropDownList | SubdepotStockOutMasterDao.InvoiceId | SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 | SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll, SubDepotStockAdjustmentsVoucherBll.SaveDataForSubS | SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal, SubDepotStockAdjustmentsVoucherDal.SaveDataForSubDcStoc | UNKNOWN | UNKNOWN | UNKNOWN |
| StockOutTextBox | SubdepotStockOutMasterDao.StockOutDate | SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 | SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll, SubDepotStockAdjustmentsVoucherBll.SaveDataForSubS | SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal, SubDepotStockAdjustmentsVoucherDal.SaveDataForSubDcStoc | UNKNOWN | UNKNOWN | UNKNOWN |
| txtReason | SubdepotStockOutMasterDao.Reason | SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 | SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll, SubDepotStockAdjustmentsVoucherBll.SaveDataForSubS | SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal, SubDepotStockAdjustmentsVoucherDal.SaveDataForSubDcStoc | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| DistributioncenterDropDownList1 | Page_Load → SubDepotStockAdjustmentsVoucherBll.DistributionCenterLoadBll | inline SQL | Com/ComUnitId | tblCompanyUnit |
| ProformaInvoiceNumDropDownList | DistributioncenterDropDownList1_OnSelectedIndexChanged → SubDepotStockAdjustmentsVoucherBll.ProformaInvoiceNumberBll | inline SQL | InvoiceNo/InvoiceId | tblSubInvoiceMaster |
| productDropDownList | Page_Load → SubDepotStockAdjustmentsVoucherBll.ProductLoadBll | inline SQL | Pro/ProductId | tblProduct |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| Button2 | LinkButton click | (postback) | SubDepot_UI_SubDepotStockAdjustmentVoucher.Button1_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:215 | SubDepotStockAdjustmentsVoucherBll.GetProductDcStoreSubdeport | SubDepotStockAdjustmentsVoucherDal.GetProductDcStoreSubdeport | — | read-only / none detected |
| LinkButton1 | LinkButton click | (postback) | SubDepot_UI_SubDepotStockAdjustmentVoucher.LinkButton1_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:384 | — | — | — | read-only / none detected |
| submitButton | LinkButton click | return sweetAlertConfirm_Submit(this); | SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:324 | SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll, SubDepotStockAdjustmentsVouche | SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal, SubDepotStockAdjustmentsVoucherDal. | — | writes tblSubDepotStockOutDetails, tblSubDepotStockOutMaster |
| btnReset | LinkButton click | (postback) | SubDepot_UI_SubDepotStockAdjustmentVoucher.btnReset_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:379 | — | — | — | read-only / none detected |

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
| SubDepot_UI_SubDepotStockAdjustmentVoucher.Page_Load | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:21 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.Page_Load → SubDepotStockAdjustmentsVoucherBll.DistributionCenterLoadBll / SubDepotStockAdjustmentsVoucherBll.ProductLoadBll → SubDepotStockAdjustmentsVoucherDal.DistributionCenterLoadDal / SubDepotStockAdjustmentsVou | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.ListImageButton_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:31 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.ListImageButton_Click | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.addButton_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:50 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.addButton_Click | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.DeleteImageButton_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:106 | object sender, ImageClickEventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.DeleteImageButton_Click | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.SubDepotInvoice_TextChanged | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:148 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.SubDepotInvoice_TextChanged | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.dQtyTextBox_TextChanged | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:177 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.dQtyTextBox_TextChanged | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.Button1_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:215 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.Button1_Click → SubDepotStockAdjustmentsVoucherBll.GetProductDcStoreSubdeport → SubDepotStockAdjustmentsVoucherDal.GetProductDcStoreSubdeport | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.DistributioncenterDropDownList1_OnSelectedIndexChanged | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:230 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.DistributioncenterDropDownList1_OnSelectedIndexChanged → SubDepotStockAdjustmentsVoucherBll.ProformaInvoiceNumberBll → SubDepotStockAdjustmentsVoucherDal.ProformaInvoiceNumberDal | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.chkSelectAll_CheckedChanged | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:247 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.chkSelectAll_CheckedChanged | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:324 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.submitButton_Click1 → SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll / SubDepotStockAdjustmentsVoucherBll.SaveDataForSubStockOutDetailBll / SubDepotStockAdjustmentsVoucherBll.SubStockOutMasterCod | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.btnReset_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:379 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.btnReset_Click | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucher.LinkButton1_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs:384 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucher.LinkButton1_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotStockAdjustmentsVoucherBll.ProductLoadBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:23 |
| SubDepotStockAdjustmentsVoucherBll.DistributionCenterLoadBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:29 |
| SubDepotStockAdjustmentsVoucherBll.GetProductDcStoreSubdeport | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:157 |
| SubDepotStockAdjustmentsVoucherBll.ProformaInvoiceNumberBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:35 |
| SubDepotStockAdjustmentsVoucherBll.SaveDataForSubDcStockOutMasterBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:41 |
| SubDepotStockAdjustmentsVoucherBll.SubStockOutMasterCodeGenerator | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:48 |
| SubDepotStockAdjustmentsVoucherBll.SaveDataForSubStockOutDetailBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:86 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotStockAdjustmentsVoucherDal.ProductLoadDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:20 | inline SQL: tblProduct |  |
| SubDepotStockAdjustmentsVoucherDal.DistributionCenterLoadDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:26 | inline SQL: tblCompanyUnit |  |
| SubDepotStockAdjustmentsVoucherDal.GetProductDcStoreSubdeport | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:161 | inline SQL: tblProduct |  |
| SubDepotStockAdjustmentsVoucherDal.ProformaInvoiceNumberDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:33 | inline SQL: tblSubInvoiceMaster |  |
| SubDepotStockAdjustmentsVoucherDal.SaveDataForSubDcStockOutMaster | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:42 | inline SQL: tblSubDepotStockOutMaster |  |
| SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:52 | inline SQL: tblSubDepotStockOutDetails |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblProduct, tblSubInvoiceMaster
- Tables WRITTEN (inline SQL reachable): tblSubDepotStockOutDetails, tblSubDepotStockOutMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler SubDepotInvoice_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Enter valid Data!!.... | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Add product and Stock Out Qty | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Insert Reason!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Insert Stock Out Date!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Distribution center!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Product!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Proforma Invoice Number!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ComUnitId, LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 16 methods.
- Redirect targets: SubDepotStockAdjustmentVoucher.aspx, SubDepotStockAdjustmentVoucherView.aspx
- User messages: Please Add product and Stock Out Qty; Please Enter valid Data!!....; Please Insert Reason!!; Please Insert Stock Out Date!!; Please Select Distribution center!!; Please Select Product!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: SubDepotStockAdjustmentsVoucherBll; DAL: SubDepotStockAdjustmentsVoucherDal; DAO: SubdepotStockOutDetailsDao, SubdepotStockOutMasterDao; Tables (inline): tblCompanyUnit, tblProduct, tblSubDepotStockOutDetails, tblSubDepotStockOutMaster, tblSubInvoiceMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click Button2 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click Button2 with valid data | Handler runs; rows written to tblSubDepotStockOutDetails, tblSubDepotStockOutMaster |
| TC-05 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click LinkButton1 with valid data | Handler runs; rows written to tblSubDepotStockOutDetails, tblSubDepotStockOutMaster |
| TC-07 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click submitButton with valid data | Handler runs; rows written to tblSubDepotStockOutDetails, tblSubDepotStockOutMaster |
| TC-09 | Click btnReset with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnReset with valid data | Handler runs; rows written to tblSubDepotStockOutDetails, tblSubDepotStockOutMaster |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx`, `Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 43 controls parsed | n/a | PARTIAL | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M12 (Sub-Depot Operations).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods SubDepotStockAdjustmentsVoucherDal.DistributionCenterLoadDal, SubDepotStockAdjustmentsVoucherDal.GetProductDcStoreSubdeport, SubDepotStockAdjustmentsVoucherDal.ProductLoadDal, SubDepotStockAdjustmentsVoucherDal.ProformaInvoiceNumberDal, SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
