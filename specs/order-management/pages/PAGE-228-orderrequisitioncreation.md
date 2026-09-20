---
page_id: PAGE-228
module: M06
page: OrderRequisitionCreation
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-228 — OrderRequisitionCreation

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-228 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderRequisitionCreation |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_OrderRequisitionCreation |
| aspx size | 34123 bytes |
| Code-behind LOC | 781 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderRequisitionCreation** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/OrderRequisitionView.aspx`, `SInventory_UI/StockTransferOrder.aspx`

Outbound references: `orderrequisitionview.aspx`

## 5. Route
`~/SInventory_UI/OrderRequisitionCreation.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx |
| Code-behind | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DCStoreBLL (Library.BLL/SInventory_BLL/DCStoreBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | DCStoreDAL (Library.DAL/SInventory_DAL/DCStoreDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAO/entity class | Requesition (Library.DAO/SInventory_Entities/Requesition.cs) |
| DAO/entity class | RequsitionChild (Library.DAO/SInventory_Entities/RequsitionChild.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→ListImageButton_Click |
| msgLabel | Label |  | no validator found |  |  |  |  |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| Label2 | Label |  | no validator found |  |  |  |  |
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| reqNoTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| wareHouseDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| reqDateTextBox | TextBox |  | no validator found |  |  |  |  |
| Date | ajaxToolkit:CalendarExtender |  | no validator found |  |  |  |  |
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| id_fu | FileUpload |  | no validator found |  |  |  |  |
| btnUpload | Button | Upload | no validator found |  |  |  | OnClick→btnUpload_Click |
| lbl_up_status | Label |  | no validator found |  |  |  |  |
| IsFileUploaded | HiddenField |  | no validator found |  |  |  |  |
| productGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Product Code | no validator found |  |  |  |  |
| productCodeTextBox | TextBox | <%# Eval("ProductCode")%> | no validator found |  | ReadOnly=true |  | OnTextChanged→productCodeTextBox_TextChanged |
| productCodeTextBox1_AutoCompleteExtender | ajaxToolkit:AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | Product Name | no validator found |  |  |  |  |
| productNameTextBox | TextBox | <%# Eval("ProductName")%> | no validator found |  |  |  | OnTextChanged→productNameTextBox_TextChanged |
| productNameTextBox_AutoCompleteExtender | AutoCompleteExtender |  | no validator found |  |  |  |  |
| productNameTextBox_AutoCompleteExtender | ajaxToolkit:AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | PackSize | no validator found |  |  |  |  |
| packSizeTextBox | TextBox | <%# Eval("PackSize")%> | no validator found |  | ReadOnly=True |  |  |
| (no id) | TemplateField | CStock | no validator found |  |  |  |  |
| cstockTextBox | TextBox | <%# Eval("CStock")%> | no validator found |  | ReadOnly=True |  |  |
| (no id) | TemplateField | Qty | no validator found |  |  |  |  |
| reqQtyTextBox | TextBox | <%# Eval("Quantity")%> | no validator found |  |  |  | OnTextChanged→reqQtyTextBox_OnTextChanged |
| FilteredTextBoxExtender2 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | Add | no validator found |  |  |  |  |
| ImageButton1 | ImageButton |  | no validator found |  |  |  | OnClick→ImageButton1_Click |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| ImageButton2 | ImageButton |  | no validator found |  |  |  | OnClick→ImageButton2_Click |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_Click, OnClientClick→return confirm('Are you sure you want to Create Stock Transfer Order ?'); |
| LinkButton5 | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| hiddenField | HiddenField |  | no validator found |  |  |  |  |
| msgLabel | Label |  | no validator found |  |  |  |  |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| HyperLink1 | HyperLink |  | no validator found |  |  |  |  |
| Label2 | Label |  | no validator found |  |  |  |  |
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| Label1 | Label |  | no validator found |  |  |  |  |
| reqNoTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| reqDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | ajaxToolkit:CalendarExtender |  | no validator found |  |  |  |  |
| wareHouseDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| id_fu | FileUpload |  | no validator found |  |  |  |  |
| btnUpload | Button | Upload | no validator found |  |  |  | OnClick→btnUpload_Click |
| lbl_up_status | Label |  | no validator found |  |  |  |  |
| IsFileUploaded | HiddenField |  | no validator found |  |  |  |  |
| productGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Product Code | no validator found |  |  |  |  |
| productCodeTextBox | TextBox | <%# Eval("ProductCode")%> | no validator found |  |  |  | OnTextChanged→productCodeTextBox_TextChanged |
| productCodeTextBox1_AutoCompleteExtender | ajaxToolkit:AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | Product Name | no validator found |  |  |  |  |
| productNameTextBox | TextBox | <%# Eval("ProductName")%> | no validator found |  |  |  | OnTextChanged→productNameTextBox_TextChanged |
| productNameTextBox_AutoCompleteExtender | ajaxToolkit:AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField | PackSize | no validator found |  |  |  |  |
| packSizeTextBox | TextBox | <%# Eval("PackSize")%> | no validator found |  | ReadOnly=True |  |  |
| (no id) | TemplateField | CStock | no validator found |  |  |  |  |
| cstockTextBox | TextBox | <%# Eval("CStock")%> | no validator found |  | ReadOnly=True |  |  |
| (no id) | TemplateField | Qty | no validator found |  |  |  |  |
| reqQtyTextBox | TextBox | <%# Eval("Quantity")%> | no validator found |  |  |  |  |
| fcurrentStockTextBox | ajaxToolkit:FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Add | no validator found |  |  |  |  |
| ImageButton1 | ImageButton |  | no validator found |  |  |  | OnClick→ImageButton1_Click |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| ImageButton2 | ImageButton |  | no validator found |  |  |  | OnClick→ImageButton2_Click |
| submitButton | Button | Submit | no validator found |  |  |  | OnClick→submitButton_Click, OnClientClick→return confirm('Are you sure you want to Create Stock Transfer Order ?'); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList | companyInfo | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| reqNoTextBox | Requesition.ReqNo | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| reqDateTextBox | Requesition.ReqDate | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| wareHouseDropDownList | Requesition.WarehouseId | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| wareHouseDropDownList | Requesition.WearhouseName | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList | Requesition.ComUnitId | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| manufacturerDropDownList | Requesition.ManufacId | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | RequsitionChild.ProductCode | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| productNameTextBox | RequsitionChild.ProductName | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| packSizeTextBox | RequsitionChild.PackSize | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| hiddenField | Requesition.ReqId | SInventory_UI_OrderRequisitionCreation.SaveAllData | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, RequisitionBLL.ReqChil | RequisitionDAL.DCLoad, RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.LoadmanufacturerName, RequisitionDAL.SaveReu | UNKNOWN | UNKNOWN | UNKNOWN |
| productNameTextBox | productName | SInventory_UI_OrderRequisitionCreation.productNameTextBox_TextChanged | DCStoreBLL.GetProductStock, DCStoreBLL.ProductInfo | DCStoreDAL.GetProductStock, DCStoreDAL.LoadProductCode, DCStoreDAL.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | productCode | SInventory_UI_OrderRequisitionCreation.productNameTextBox_TextChanged | DCStoreBLL.GetProductStock, DCStoreBLL.ProductInfo | DCStoreDAL.GetProductStock, DCStoreDAL.LoadProductCode, DCStoreDAL.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | DropDownLoad → RequisitionBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |
| wareHouseDropDownList | DropDownLoad → RequisitionBLL.WareHouseLoad | inline SQL | WearhouseName/WearhouseId | tblWearhouse |
| dcDropDownList | DropDownLoad → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacturerDropDownList | DropDownLoad → RequisitionBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |
| wareHouseDropDownList | DropDownLoad → RequisitionBLL.WareHouseLoad | inline SQL | WearhouseName/WearhouseId | tblWearhouse |
| dcDropDownList | DropDownLoad → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton click | (postback) | SInventory_UI_OrderRequisitionCreation.ListImageButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:67 | — | — | — | read-only / none detected |
| btnUpload | Button click | (postback) | SInventory_UI_OrderRequisitionCreation.btnUpload_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:700 | — | — | — | read-only / none detected |
| ImageButton1 | ImageButton click | (postback) | SInventory_UI_OrderRequisitionCreation.ImageButton1_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:222 | — | — | — | read-only / none detected |
| ImageButton2 | ImageButton click | (postback) | SInventory_UI_OrderRequisitionCreation.ImageButton2_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:226 | — | — | — | read-only / none detected |
| submitButton | LinkButton click | return confirm('Are you sure you want to Create Stock Transfer Order ?'); | SInventory_UI_OrderRequisitionCreation.submitButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:634 | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, Re | RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.SaveReuqisitionChildDAL, RequisitionDAL.SaveReu | — | writes tblRequisition, tblRequsitionChild |
| LinkButton5 | LinkButton click | (postback) | SInventory_UI_OrderRequisitionCreation.cancelButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:71 | — | — | — | read-only / none detected |
| btnUpload | Button click | (postback) | SInventory_UI_OrderRequisitionCreation.btnUpload_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:700 | — | — | — | read-only / none detected |
| ImageButton1 | ImageButton click | (postback) | SInventory_UI_OrderRequisitionCreation.ImageButton1_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:222 | — | — | — | read-only / none detected |
| ImageButton2 | ImageButton click | (postback) | SInventory_UI_OrderRequisitionCreation.ImageButton2_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:226 | — | — | — | read-only / none detected |
| submitButton | Button click | return confirm('Are you sure you want to Create Stock Transfer Order ?'); | SInventory_UI_OrderRequisitionCreation.submitButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:634 | RequisitionBLL.DCLoad, RequisitionBLL.DeleteRequisitionDtls, RequisitionBLL.LoadmanufacturerName, Re | RequisitionDAL.DeleteRequisitionDtls, RequisitionDAL.SaveReuqisitionChildDAL, RequisitionDAL.SaveReu | — | writes tblRequisition, tblRequsitionChild |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: yes; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_OrderRequisitionCreation.Page_Load | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.Page_Load → RequisitionBLL.DCLoad / RequisitionBLL.GetDataInfoByIdBll / RequisitionBLL.GetDataInfoByIdBllDtls / RequisitionBLL.LoadmanufacturerName / RequisitionBLL.ReqNo / RequisitionBLL.ReqnNoGenerator / Requis → Common | no | no |
| SInventory_UI_OrderRequisitionCreation.PermissionForShowBtn | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:41 |  | SInventory_UI_OrderRequisitionCreation.PermissionForShowBtn → CommonDataLoad.GetPermissionForShowBtn → SP: sp_GET_btnShow | yes | no |
| SInventory_UI_OrderRequisitionCreation.ListImageButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:67 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.ListImageButton_Click | no | no |
| SInventory_UI_OrderRequisitionCreation.cancelButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:71 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.cancelButton_Click | no | no |
| SInventory_UI_OrderRequisitionCreation.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:75 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_OrderRequisitionCreation.GetOneRecord | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:86 | Int32 id | SInventory_UI_OrderRequisitionCreation.GetOneRecord → RequisitionBLL.GetDataInfoByIdBll / RequisitionBLL.GetDataInfoByIdBllDtls → RequisitionDAL.GetDataInfoByIdBllDtls / RequisitionDAL.GetDataInfoByIdDAL | no | no |
| SInventory_UI_OrderRequisitionCreation.DropDownLoad | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:115 |  | SInventory_UI_OrderRequisitionCreation.DropDownLoad → RequisitionBLL.DCLoad / RequisitionBLL.LoadmanufacturerName / RequisitionBLL.WareHouseLoad → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad / RequisitionDAL.LoadmanufacturerName / RequisitionDAL.WareHo | no | no |
| SInventory_UI_OrderRequisitionCreation.ImageButton1_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:222 | object sender, ImageClickEventArgs e | SInventory_UI_OrderRequisitionCreation.ImageButton1_Click | no | no |
| SInventory_UI_OrderRequisitionCreation.ImageButton2_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:226 | object sender, ImageClickEventArgs e | SInventory_UI_OrderRequisitionCreation.ImageButton2_Click | no | no |
| SInventory_UI_OrderRequisitionCreation.GetProductInGrid | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:282 | int rowindex, string productCode | SInventory_UI_OrderRequisitionCreation.GetProductInGrid → DCStoreBLL.GetProductStock / DCStoreBLL.ProductInfo → DCStoreDAL.GetProductStock / DCStoreDAL.LoadProductCode / DCStoreDAL.SingleParameter | no | no |
| SInventory_UI_OrderRequisitionCreation.productCodeTextBox_TextChanged | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:321 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.productCodeTextBox_TextChanged | no | no |
| SInventory_UI_OrderRequisitionCreation.SaveAllData | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:358 |  | SInventory_UI_OrderRequisitionCreation.SaveAllData → RequisitionBLL.DCLoad / RequisitionBLL.DeleteRequisitionDtls / RequisitionBLL.LoadmanufacturerName / RequisitionBLL.ReqChildId / RequisitionBLL.ReqId / RequisitionBLL.ReqNo / RequisitionBLL.ReqnNoGene → Requ | no | no |
| SInventory_UI_OrderRequisitionCreation.submitButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:634 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.submitButton_Click → RequisitionBLL.DCLoad / RequisitionBLL.DeleteRequisitionDtls / RequisitionBLL.LoadmanufacturerName / RequisitionBLL.ReqChildId / RequisitionBLL.ReqId / RequisitionBLL.ReqNo / RequisitionBLL.ReqnNoGene | no | no |
| SInventory_UI_OrderRequisitionCreation.Clear | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:657 |  | SInventory_UI_OrderRequisitionCreation.Clear → RequisitionBLL.DCLoad / RequisitionBLL.LoadmanufacturerName / RequisitionBLL.ReqNo / RequisitionBLL.ReqnNoGenerator / RequisitionBLL.WareHouseLoad → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad / Requisitio | no | no |
| SInventory_UI_OrderRequisitionCreation.productNameTextBox_TextChanged | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:665 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.productNameTextBox_TextChanged → DCStoreBLL.GetProductStock / DCStoreBLL.ProductInfo → DCStoreDAL.GetProductStock / DCStoreDAL.LoadProductCode / DCStoreDAL.SingleParameter | no | no |
| SInventory_UI_OrderRequisitionCreation.miaImageButton_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:692 | object sender, ImageClickEventArgs e | SInventory_UI_OrderRequisitionCreation.miaImageButton_Click | no | no |
| SInventory_UI_OrderRequisitionCreation.manufacturerDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:696 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.manufacturerDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_OrderRequisitionCreation.btnUpload_Click | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:700 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.btnUpload_Click | no | no |
| SInventory_UI_OrderRequisitionCreation.reqQtyTextBox_OnTextChanged | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs:763 | object sender, EventArgs e | SInventory_UI_OrderRequisitionCreation.reqQtyTextBox_OnTextChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/RequisitionBLL.cs:138 |
| RequisitionBLL.WareHouseLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:134 |
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| RequisitionBLL.GetDataInfoByIdBll | Library.BLL/SInventory_BLL/RequisitionBLL.cs:49 |
| RequisitionBLL.GetDataInfoByIdBllDtls | Library.BLL/SInventory_BLL/RequisitionBLL.cs:54 |
| RequisitionBLL.ReqNo | Library.BLL/SInventory_BLL/RequisitionBLL.cs:73 |
| RequisitionBLL.ReqnNoGenerator | Library.BLL/SInventory_BLL/RequisitionBLL.cs:81 |
| DCStoreBLL.ProductInfo | Library.BLL/SInventory_BLL/DCStoreBLL.cs:90 |
| DCStoreBLL.GetProductStock | Library.BLL/SInventory_BLL/DCStoreBLL.cs:98 |
| RequisitionBLL.SaveRequsition | Library.BLL/SInventory_BLL/RequisitionBLL.cs:20 |
| RequisitionBLL.ReqId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:67 |
| RequisitionBLL.SaveRequsitionChild | Library.BLL/SInventory_BLL/RequisitionBLL.cs:33 |
| RequisitionBLL.ReqChildId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:59 |
| RequisitionBLL.UpdateManufacturerInfo | Library.BLL/SInventory_BLL/RequisitionBLL.cs:28 |
| RequisitionBLL.DeleteRequisitionDtls | Library.BLL/SInventory_BLL/RequisitionBLL.cs:182 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/RequisitionDAL.cs:30 | inline SQL: tblManufacturer |  |
| RequisitionDAL.WareHouseLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:25 | inline SQL: tblWearhouse |  |
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| RequisitionDAL.GetDataInfoByIdDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:160 | inline SQL: tblRequisition | @ReqId |
| RequisitionDAL.GetDataInfoByIdBllDtls | Library.DAL/SInventory_DAL/RequisitionDAL.cs:171 | inline SQL: tblRequsitionChild, tbl_INV_CentralStore_WeightedAvg | @ReqId |
| CommonDataLoad.GetPermissionForShowBtn | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:65 | sp_GET_btnShow | @PageName |
| DCStoreDAL.LoadProductCode | Library.DAL/SInventory_DAL/DCStoreDAL.cs:80 | inline SQL: tblProduct, tblUnitPrice |  |
| DCStoreDAL.SingleParameter | Library.DAL/SInventory_DAL/DCStoreDAL.cs:151 | delegates / no SQL literal |  |
| DCStoreDAL.GetProductStock | Library.DAL/SInventory_DAL/DCStoreDAL.cs:91 | inline SQL: tbl_INV_CentralStore_WeightedAvg |  |
| RequisitionDAL.SaveReuqisitionDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:86 | inline SQL: tblRequisition | @ComUnitCode, @ComUnitId, @ComUnitName, @EntryBy, @EntryDate, @IsFromBatch, @ManufacId, @ReqDate, @R |
| RequisitionDAL.SaveReuqisitionChildDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:124 | inline SQL: tblRequsitionChild | @BatchNO, @PackSize, @ProductCode, @ProductName, @ReqChildId, @ReqId, @ReqQty |
| RequisitionDAL.UpdateManufacturerInfo | Library.DAL/SInventory_DAL/RequisitionDAL.cs:68 | inline SQL: tblRequisition | @ComUnitCode, @ComUnitId, @ComUnitName, @ReqDate, @ReqId, @ReqNo, @UpdateBy, @UpdateDate, @Warehouse |
| RequisitionDAL.DeleteRequisitionDtls | Library.DAL/SInventory_DAL/RequisitionDAL.cs:115 | inline SQL: tblRequsitionChild, tblRequsitionChild | @ReqId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_btnShow | CommonDataLoad.GetPermissionForShowBtn | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblManufacturer, tblProduct, tblRequisition, tblRequsitionChild, tblUnitPrice, tblUserCompanyUnit, tblWearhouse, tbl_INV_CentralStore_WeightedAvg
- Tables WRITTEN (inline SQL reachable): tblRequisition, tblRequsitionChild
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblRequisition | ComUnitCode | @ComUnitCode |
| tblRequisition | ComUnitId | @ComUnitId |
| tblRequisition | ComUnitName | @ComUnitName |
| tblRequisition | EntryBy | @EntryBy |
| tblRequisition | EntryDate | @EntryDate |
| tblRequisition | IsFromBatch | @IsFromBatch |
| tblRequisition | ManufacId | @ManufacId |
| tblRequisition | ReqDate | @ReqDate |
| tblRequisition | ReqId | @ReqId |
| tblRequisition | ReqNo | @ReqNo |
| tblRequisition | UpdateBy | @UpdateBy |
| tblRequisition | UpdateDate | @UpdateDate |
| tblRequisition | WarehouseId | @WarehouseId |
| tblRequisition | WearhouseName | @WearhouseName |
| tblRequsitionChild | BatchNO | @BatchNO |
| tblRequsitionChild | PackSize | @PackSize |
| tblRequsitionChild | ProductCode | @ProductCode |
| tblRequsitionChild | ProductName | @ProductName |
| tblRequsitionChild | ReqChildId | @ReqChildId |
| tblRequsitionChild | ReqId | @ReqId |
| tblRequsitionChild | ReqQty | @ReqQty |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler GetProductInGrid) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Input Correct Data!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Distribution Center  !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Manufacturer !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select WareHouse !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill out Req.Qty!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill out productCode!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill out productName!! | UNKNOWN |
| (handler email) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | A Stock Transfer Order has been generated. Please complete Challan | UNKNOWN |
| (handler email2) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | A Stock Transfer Order has been generated. Please complete Challan | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select Manufacturer Name!! | UNKNOWN |
| (handler productNameTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Input Correct Data!! | UNKNOWN |
| (handler reqQtyTextBox_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Quantity cannot exceed Current Stock! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: AppPass, EmailID, LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 4 of 29 methods.
- Redirect targets: OrderRequisitionCreation.aspx, OrderRequisitionView.aspx
- User messages: A Stock Transfer Order has been generated. Please complete Challan; Input Correct Data!!; Please Select Distribution Center  !!!; Please Select Manufacturer !!!; Please Select WareHouse !!!; Please fill out Req.Qty!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: EntryBy, EntryDate, UpdateBy, UpdateDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DCStoreBLL, RequisitionBLL; DAL: CommonDataLoad, DCStoreDAL, RequisitionDAL; DAO: Requesition, RequsitionChild; Tables (inline): tblCompanyUnit, tblManufacturer, tblProduct, tblRequisition, tblRequsitionChild, tblUnitPrice, tblUserCompanyUnit, tblWearhouse, tbl_INV_CentralStore_WeightedAvg

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewLinkButton with valid data | Handler runs; rows written to tblRequisition, tblRequsitionChild |
| TC-05 | Click btnUpload with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnUpload with valid data | Handler runs; rows written to tblRequisition, tblRequsitionChild |
| TC-07 | Click ImageButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click ImageButton1 with valid data | Handler runs; rows written to tblRequisition, tblRequsitionChild |
| TC-09 | Click ImageButton2 with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click ImageButton2 with valid data | Handler runs; rows written to tblRequisition, tblRequsitionChild |
| TC-11 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click submitButton with valid data | Handler runs; rows written to tblRequisition, tblRequsitionChild |
| TC-13 | Click LinkButton5 with required fields empty | Validator/server message shown; no DB write |
| TC-14 | Click LinkButton5 with valid data | Handler runs; rows written to tblRequisition, tblRequsitionChild |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx`, `Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 79 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/OrderRequisitionCreation.aspx |
| Data access | This document §15–17 | 1 SP names, 9 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: PAGE-385, PAGE-389, PAGE-392, PAGE-412, PAGE-420
- Changing DAL methods CommonDataLoad.GetPermissionForShowBtn, DCStoreDAL.GetProductStock, DCStoreDAL.LoadProductCode, DCStoreDAL.SingleParameter, RequisitionDAL.BindDropDown affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
