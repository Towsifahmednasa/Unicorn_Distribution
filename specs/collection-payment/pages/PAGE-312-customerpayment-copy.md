---
page_id: PAGE-312
module: M09
page: CustomerPayment - Copy
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-312 — CustomerPayment - Copy

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-312 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | CustomerPayment - Copy |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_CustomerPayment |
| aspx size | 25476 bytes |
| Code-behind LOC | 461 |
| Inline script size | 738 chars |
| Suspected legacy/duplicate | YES (name pattern; INFERRED) |

## 2. Purpose
INFERRED from name/markup: **CustomerPayment - Copy** (Entry / Transaction). Page title/heading text: `Due Invoice List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerpayment.aspx`, `customerpaymentexcelupload.aspx`

## 5. Route
`~/SInventory_UI/CustomerPayment - Copy.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | CustPaymentBLL (Library.BLL/SInventory_BLL/CustPaymentBLL.cs) |
| DAL class | CustPaymentDAL (Library.DAL/SInventory_DAL/CustPaymentDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAO/entity class | CustPayment (Library.DAO/SInventory_Entities/CustPayment.cs) |
| DAO/entity class | CustPaymentDetail (Library.DAO/SInventory_Entities/CustPaymentDetail.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→salesCenterDropDownList_SelectedIndexChanged |
| rootDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_Click |
| payTypeDDL | DropDownList |  | no validator found |  |  | see §9 |  |
| refNameTextBox | TextBox |  | no validator found |  |  |  |  |
| refDtTextBox | TextBox |  | no validator found |  | Visible=False |  |  |
| paymentDtTextBox_CalendarExtender0 | CalendarExtender |  | no validator found |  |  |  |  |
| ImageButton0 | ImageButton |  | no validator found |  | Visible=False |  |  |
| marketDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→marketDropDownList_SelectedIndexChanged |
| customerDropDownList | DropDownList |  | no validator found |  | Visible=False | see §9 | OnSelectedIndexChanged→customerDropDownList_SelectedIndexChanged |
| lblCount | Label | Total Pay Amount : 0 | no validator found |  |  |  |  |
| orderGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| hfCustomerMasterId | HiddenField |  | no validator found |  |  |  |  |
| hfMarketId | HiddenField |  | no validator found |  |  |  |  |
| hfDistributionRouteId | HiddenField |  | no validator found |  |  |  |  |
| chkSelect | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelect_CheckedChanged |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Confirm Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Confirm Invoice Date | no validator found |  |  |  |  |
| (no id) | TemplateField | Confirm  Amount | no validator found |  |  |  |  |
| lblTotalDelivery | Label | <%#Eval("TotalDelivery")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Previous Pay | no validator found |  |  |  |  |
| lblPaymentAmount | Label | <%#Eval("PaymentAmount")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Due Amount | no validator found |  |  |  |  |
| lblDue | Label | <%#Eval("Due")%> | no validator found |  |  |  |  |
| (no id) | TemplateField | Pay Amount | no validator found |  |  |  |  |
| payAmountTextBox | TextBox | <%#Eval("Due")%> | no validator found |  |  |  | OnTextChanged→payAmountTextBox_TextChanged |
| FilteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | TemplateField |  | no validator found |  | Visible=false |  |  |
| chkAdjust | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkAdjust_OnCheckedChanged |
| (no id) | BoundField | AdjustableAmount | no validator found |  | Visible=false |  |  |
| masterButton | LinkButton |  | no validator found |  |  |  | OnClick→saveButton_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | dt | SInventory_UI_CustomerPayment.salesCenterDropDownList_SelectedIndexChanged | — | SeedDataDAL.GetRouteInfoforCustPayment | UNKNOWN | UNKNOWN | UNKNOWN |
| lblTotalDelivery | TotalDelivery | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerMasterId | CustPayment.CustomerMasterId | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentParameters | @CustomerMasterId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfMarketId | CustPayment.MarketId | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| hfDistributionRouteId | CustPayment.DistributionRouteId | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | CustPayment.ComUnitId | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentParameters | @ComUnitId | UNKNOWN (SP body not in repo) | UNKNOWN |
| payAmountTextBox | CustPayment.PaymentAmount | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | OrderInfoDAL.CheckInvoiceCustpayment | @PaymentAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| payAmountTextBox | CustPayment.PaymentAmount | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.UpdateInvoiceFinalPayment | @PaymentAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| payTypeDDL | CustPayment.PayType | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| refNameTextBox | CustPayment.RefNo | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| refDtTextBox | CustPayment.RefDate | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| payAmountTextBox | totalamount | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| chkAdjust | CustPaymentDetail.IsAdjust | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPayment | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| payAmountTextBox | mainamount | SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblTotalDelivery | delamount | SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| lblDue | Dueamount | SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| payAmountTextBox | prevamount | SInventory_UI_CustomerPayment.CalculateTotal | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | paaram | SInventory_UI_CustomerPayment.GenerateParam | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownList → CustPaymentBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| rootDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| payTypeDDL | DropDownList → CustPaymentBLL.PaymentTypeLoadBLL | inline SQL | PaymentTypeName/PaymentTypeId | tblPaymentType |
| marketDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| customerDropDownList | marketDropDownList_SelectedIndexChanged → CustPaymentBLL.LoadCustomerMaster | inline SQL | CustomerName/CustomerMasterId | View_CustomerMaster, tblCustMaster |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_CustomerPayment.submitButton_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:425 | — | OrderInfoDAL.LoadPaymentInvSP | sp_GET_PaymentInvSP | SP executed — effect on tables UNKNOWN (body not in repo) |
|  | LinkButton click | (postback) | SInventory_UI_CustomerPayment.cancelButton_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:420 | — | — | — | read-only / none detected |
| masterButton | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_CustomerPayment.saveButton_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:207 | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoiceFinalPaym | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Exi | sp_Check_Duplicate_InvoiceFinalPayment, sp_GET_PaymentInvSP, sp_Update_InvoiceFinalPayment | writes tblCustPayDetail, tblCustomerPay |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_CustomerPayment.Page_Load | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:21 | object sender, EventArgs e | SInventory_UI_CustomerPayment.Page_Load → CustPaymentBLL.LoadSC / CustPaymentBLL.PaymentTypeLoadBLL → CustPaymentDAL.LoadSC / CustPaymentDAL.PaymentTypeLoad / CustPaymentDAL.SingleParameter / SeedDataDAL.GetRouteInfoforCustPayment → SP: sp_Get_RouteInfoforCust | no | no |
| SInventory_UI_CustomerPayment.DropDownList | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:50 |  | SInventory_UI_CustomerPayment.DropDownList → CustPaymentBLL.LoadSC / CustPaymentBLL.PaymentTypeLoadBLL → CustPaymentDAL.LoadSC / CustPaymentDAL.PaymentTypeLoad / CustPaymentDAL.SingleParameter / SeedDataDAL.GetRouteInfoforCustPayment → SP: sp_Get_RouteInfoforC | yes | no |
| SInventory_UI_CustomerPayment.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:72 | object sender, EventArgs e | SInventory_UI_CustomerPayment.salesCenterDropDownList_SelectedIndexChanged → SeedDataDAL.GetRouteInfoforCustPayment → SP: sp_Get_RouteInfoforCustPayment | yes | no |
| SInventory_UI_CustomerPayment.customerDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:104 | object sender, EventArgs e | SInventory_UI_CustomerPayment.customerDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_CustomerPayment.marketDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:109 | object sender, EventArgs e | SInventory_UI_CustomerPayment.marketDropDownList_SelectedIndexChanged → CustPaymentBLL.LoadCustomerMaster → CustPaymentDAL.LoadCustomerMaster / CustPaymentDAL.SingleParameter | no | no |
| SInventory_UI_CustomerPayment.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:124 | object sender, EventArgs e | SInventory_UI_CustomerPayment.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_CustomerPayment.saveButton_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:207 | object sender, EventArgs e | SInventory_UI_CustomerPayment.saveButton_Click → CustPaymentBLL.SaveCustDetail / CustPaymentBLL.SaveCustPayment / CustPaymentBLL.UpdateInvoiceFinalPayment → CustPaymentDAL.CustPaymentDetailParameters / CustPaymentDAL.CustPaymentParameters / CustPaymentDAL.Exis | no | no |
| SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:343 | object sender, EventArgs e | SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | no | no |
| SInventory_UI_CustomerPayment.chkAdjust_OnCheckedChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:401 | object sender, EventArgs e | SInventory_UI_CustomerPayment.chkAdjust_OnCheckedChanged | no | no |
| SInventory_UI_CustomerPayment.cancelButton_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:420 | object sender, EventArgs e | SInventory_UI_CustomerPayment.cancelButton_Click | no | no |
| SInventory_UI_CustomerPayment.submitButton_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:425 | object sender, EventArgs e | SInventory_UI_CustomerPayment.submitButton_Click → OrderInfoDAL.LoadPaymentInvSP → SP: sp_GET_PaymentInvSP | no | no |
| SInventory_UI_CustomerPayment.LoadGridView | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:432 |  | SInventory_UI_CustomerPayment.LoadGridView → OrderInfoDAL.LoadPaymentInvSP → SP: sp_GET_PaymentInvSP | no | no |
| SInventory_UI_CustomerPayment.Unnamed_Click | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:453 | object sender, EventArgs e | SInventory_UI_CustomerPayment.Unnamed_Click | no | no |
| SInventory_UI_CustomerPayment.chkSelect_CheckedChanged | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs:458 | object sender, EventArgs e | SInventory_UI_CustomerPayment.chkSelect_CheckedChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustPaymentBLL.LoadSC | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:167 |
| CustPaymentBLL.PaymentTypeLoadBLL | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:189 |
| CustPaymentBLL.LoadCustomerMaster | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:181 |
| CustPaymentBLL.UpdateInvoiceFinalPayment | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:24 |
| CustPaymentBLL.SaveCustPayment | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:33 |
| CustPaymentBLL.SaveCustDetail | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:127 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustPaymentDAL.LoadSC | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:137 | inline SQL: tblCompanyUnit |  |
| CustPaymentDAL.SingleParameter | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:344 | delegates / no SQL literal |  |
| CustPaymentDAL.PaymentTypeLoad | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:180 | inline SQL: tblPaymentType |  |
| SeedDataDAL.GetRouteInfoforCustPayment | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:526 | sp_Get_RouteInfoforCustPayment | @id |
| CustPaymentDAL.LoadCustomerMaster | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:174 | inline SQL: View_CustomerMaster, tblCustMaster |  |
| OrderInfoDAL.CheckInvoiceCustpayment | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:705 | sp_Check_Duplicate_InvoiceFinalPayment | @InvoiceId, @PaymentAmount |
| CustPaymentDAL.UpdateInvoiceFinalPayment | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:42 | sp_Update_InvoiceFinalPayment | @FinalPaymentBy, @InvoiceId, @PaymentAmount, @ptStatus |
| CustPaymentDAL.SaveCustPayment | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:79 | inline SQL: tblCustomerPay |  |
| CustPaymentDAL.CustPaymentParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:296 | delegates / no SQL literal | @ComUnitId, @CreateBy, @CreateDate, @CustPayId, @CustomerMasterId, @MarketId, @PayType, @PaymentAmou |
| CustPaymentDAL.SaveCustDetail | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:102 | inline SQL: tblCustPayDetail, tblCustPayDetail, tblCustomerPay |  |
| CustPaymentDAL.CustPaymentDetailParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:315 | delegates / no SQL literal | @AIT, @BankAccId, @CashAccId, @CollectionBy, @CustPayDetailId, @CustPayId, @DANameId, @Discount, @In |
| CustPaymentDAL.Existence | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:240 | inline SQL: tblCustPayDetail | @InvoiceId, @PaymentAmount |
| OrderInfoDAL.LoadPaymentInvSP | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:767 | sp_GET_PaymentInvSP | @param |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Check_Duplicate_InvoiceFinalPayment | OrderInfoDAL.CheckInvoiceCustpayment | body NOT in repository | read-like name (INFERRED) |
| sp_GET_PaymentInvSP | OrderInfoDAL.LoadPaymentInvSP | Live_Deployment_Script_All_Updates.sql:212 | read-like name (INFERRED) |
| sp_Get_RouteInfoforCustPayment | SeedDataDAL.GetRouteInfoforCustPayment | body NOT in repository | read-like name (INFERRED) |
| sp_Update_InvoiceFinalPayment | CustPaymentDAL.UpdateInvoiceFinalPayment | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): View_CustomerMaster, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblPaymentType, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): tblCustPayDetail, tblCustomerPay
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCustPayDetail | AIT | @AIT |
| tblCustPayDetail | BankAccId | @BankAccId |
| tblCustPayDetail | CashAccId | @CashAccId |
| tblCustPayDetail | CollectionBy | @CollectionBy |
| tblCustPayDetail | CustPayDetailId | @CustPayDetailId |
| tblCustPayDetail | CustPayId | @CustPayId |
| tblCustPayDetail | DANameId | @DANameId |
| tblCustPayDetail | Discount | @Discount |
| tblCustPayDetail | InvoiceId | @InvoiceId |
| tblCustPayDetail | PaymentAmount | @PaymentAmount |
| tblCustPayDetail | TPAmount | @TPAmount |
| tblCustPayDetail | VATAmount | @VATAmount |
| tblCustPayDetail | custPaymentDate | @PDate |
| tblCustomerPay | ComUnitId | @ComUnitId |
| tblCustomerPay | CreateBy | @CreateBy |
| tblCustomerPay | CreateDate | @CreateDate |
| tblCustomerPay | CustPayId | @CustPayId |
| tblCustomerPay | CustomerMasterId | @CustomerMasterId |
| tblCustomerPay | MarketId | @MarketId |
| tblCustomerPay | PayType | @PayType |
| tblCustomerPay | PaymentAmount | @PaymentAmount |
| tblCustomerPay | PaymentDate | @PaymentDate |
| tblCustomerPay | RefDate | @RefDate |
| tblCustomerPay | RefNo | @RefNo |
| tblCustomerPay | Remarks | @Remarks |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler salesCenterDropDownList_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Payment Type!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill out Pay Amount !! | UNKNOWN |
| (handler saveButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler saveButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler payAmountTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Cannot Be Greater then Invoice Quantity | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 19 methods.
- Redirect targets: CustomerPayment.aspx
- User messages: ','Success');; Already Exist!; Cannot Be Greater then Invoice Quantity; Please Select From List; Please Select Payment Type!!; Please fill out Pay Amount !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: CreateBy, CreateDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustPaymentBLL; DAL: CustPaymentDAL, OrderInfoDAL, SeedDataDAL; DAO: CustPayment, CustPaymentDetail; Tables (inline): View_CustomerMaster, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblPaymentType, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tblCustPayDetail, tblCustomerPay |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tblCustPayDetail, tblCustomerPay |
| TC-07 | Click masterButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click masterButton with valid data | Handler runs; rows written to tblCustPayDetail, tblCustomerPay |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx`, `Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 47 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerPayment - Copy.aspx |
| Data access | This document §15–17 | 4 SP names, 7 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- Name suggests a legacy/duplicate/test variant; confirm whether it is still reachable from the menu.

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: PAGE-311, PAGE-313, PAGE-314, PAGE-315, PAGE-318
- Changing DAL methods CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentDAL.LoadCustomerMaster, CustPaymentDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
