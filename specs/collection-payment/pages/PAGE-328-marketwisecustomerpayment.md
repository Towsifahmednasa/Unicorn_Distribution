---
page_id: PAGE-328
module: M09
page: MarketWiseCustomerPayment
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-328 — MarketWiseCustomerPayment

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-328 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | MarketWiseCustomerPayment |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CustomerPayment |
| aspx size | 17941 bytes |
| Code-behind LOC | 378 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MarketWiseCustomerPayment** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/MarketWiseCustomerPayment.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx |
| Code-behind | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | CustPaymentBLL (Library.BLL/SInventory_BLL/CustPaymentBLL.cs) |
| DAL class | CustPaymentDAL (Library.DAL/SInventory_DAL/CustPaymentDAL.cs) |
| DAO/entity class | CustPayment (Library.DAO/SInventory_Entities/CustPayment.cs) |
| DAO/entity class | CustPaymentDetail (Library.DAO/SInventory_Entities/CustPaymentDetail.cs) |
| DAO/entity class | CustomerMaster (Library.BLL/SInventory_BLL/CustomerMaster.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→salesCenterDropDownList_SelectedIndexChanged |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| marketDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→marketDropDownList_SelectedIndexChanged |
| customerTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→customerTextBox_TextChanged |
| customerDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→customerDropDownList_SelectedIndexChanged |
| paymentDtTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| paymentDtTextBox_CalendarExtender | CalendarExtender |  | no validator found |  |  |  |  |
| ImageButton | ImageButton |  | no validator found |  |  |  |  |
| paymentAmountTextBox | TextBox |  | no validator found |  |  |  |  |
| FilteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| payTypeDDL | DropDownList |  | no validator found |  |  | see §9 |  |
| refNameTextBox | TextBox |  | no validator found |  |  |  |  |
| refDtTextBox | TextBox |  | no validator found |  |  |  |  |
| paymentDtTextBox_CalendarExtender0 | CalendarExtender |  | no validator found |  |  |  |  |
| ImageButton0 | ImageButton |  | no validator found |  |  |  |  |
| orderGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelect_OnCheckedChanged |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pro.Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Pro.Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Del Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Del Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Del Inv Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Previous Pay | no validator found |  |  |  |  |
| (no id) | BoundField | Due Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Adjust Amount | no validator found |  |  |  |  |
| (no id) | TemplateField | Pay Amount | no validator found |  |  |  |  |
| payAmountTextBox | TextBox |  | no validator found |  |  |  |  |
| FilteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| saveButton | Button | Save | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Save ?');, onclick→saveButton_Click |
| UpdateProgress2 | UpdateProgress |  | no validator found |  |  |  |  |
| Img2 | Image |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | dt | SInventory_UI_CustomerPayment.salesCenterDropDownList_SelectedIndexChanged | CustPaymentBLL.CustomerLoad, CustPaymentBLL.LoadCustomerMaster, CustPaymentBLL.LoadInvoice | CustPaymentDAL.CustomerLoad, CustPaymentDAL.LoadCustomerMaster, CustPaymentDAL.LoadInvoice, CustPaymentDAL.SingleParamet | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | dt | SInventory_UI_CustomerPayment.customerTextBox_TextChanged | CustPaymentBLL.CustomerLoad, CustPaymentBLL.LoadInvoice | CustPaymentDAL.CustomerLoad, CustPaymentDAL.LoadInvoice, CustPaymentDAL.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| customerTextBox | aCustomerMaster | SInventory_UI_CustomerPayment.CustomerId | CustPaymentBLL.CustomerLoad | CustPaymentDAL.CustomerLoad, CustPaymentDAL.SingleParameter | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | dt | SInventory_UI_CustomerPayment.marketDropDownList_SelectedIndexChanged | CustPaymentBLL.CustomerLoad, CustPaymentBLL.LoadCustomerMaster, CustPaymentBLL.LoadInvoice | CustPaymentDAL.CustomerLoad, CustPaymentDAL.LoadCustomerMaster, CustPaymentDAL.LoadInvoice, CustPaymentDAL.SingleParamet | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | CustPayment.ComUnitId | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentParameters | @ComUnitId | UNKNOWN (SP body not in repo) | UNKNOWN |
| paymentDtTextBox | CustPayment.PaymentDate | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentParameters | @PaymentDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| payAmountTextBox | CustPayment.PaymentAmount | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentParameters | @PaymentAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| payAmountTextBox | CustPayment.PaymentAmount | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentDetailParameters | @PaymentAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| payTypeDDL | CustPayment.PayType | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| refNameTextBox | CustPayment.RefNo | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| refDtTextBox | CustPayment.RefDate | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| payAmountTextBox | totalamount | SInventory_UI_CustomerPayment.saveButton_Click | CustPaymentBLL.SaveCustDetail, CustPaymentBLL.SaveCustPayment, CustPaymentBLL.UpdateInvoicePaymentAmount | CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.Existence, CustPaymentD | UNKNOWN | UNKNOWN | UNKNOWN |
| payAmountTextBox | mainamount | SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| payAmountTextBox | amnt | SInventory_UI_CustomerPayment.TotalPaymentAmount | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownList → CustPaymentBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| marketDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| customerDropDownList | salesCenterDropDownList_SelectedIndexChanged → CustPaymentBLL.LoadCustomerMaster | inline SQL | CustomerName/CustomerMasterId | View_CustomerMaster, tblCustMaster |
| payTypeDDL | DropDownList → CustPaymentBLL.PaymentTypeLoadBLL | inline SQL | PaymentTypeName/PaymentTypeId | tblPaymentType |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| saveButton | Button | return confirm('Are you sure you want to Save ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_CustomerPayment.Page_Load | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_CustomerPayment.Page_Load → CustPaymentBLL.LoadSC / CustPaymentBLL.PaymentTypeLoadBLL → CustPaymentDAL.LoadSC / CustPaymentDAL.PaymentTypeLoad / CustPaymentDAL.SingleParameter | no | no |
| SInventory_UI_CustomerPayment.DropDownList | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:45 |  | SInventory_UI_CustomerPayment.DropDownList → CustPaymentBLL.LoadSC / CustPaymentBLL.PaymentTypeLoadBLL → CustPaymentDAL.LoadSC / CustPaymentDAL.PaymentTypeLoad / CustPaymentDAL.SingleParameter | no | no |
| SInventory_UI_CustomerPayment.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:53 | object sender, EventArgs e | SInventory_UI_CustomerPayment.salesCenterDropDownList_SelectedIndexChanged → CustPaymentBLL.CustomerLoad / CustPaymentBLL.LoadCustomerMaster / CustPaymentBLL.LoadInvoice → CustPaymentDAL.CustomerLoad / CustPaymentDAL.LoadCustomerMaster / CustPaymentDAL.LoadInv | no | no |
| SInventory_UI_CustomerPayment.customerTextBox_TextChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:82 | object sender, EventArgs e | SInventory_UI_CustomerPayment.customerTextBox_TextChanged → CustPaymentBLL.CustomerLoad / CustPaymentBLL.LoadInvoice → CustPaymentDAL.CustomerLoad / CustPaymentDAL.LoadInvoice / CustPaymentDAL.SingleParameter | no | no |
| SInventory_UI_CustomerPayment.CustomerId | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:105 | out CustomerMaster aCustomerMaster | SInventory_UI_CustomerPayment.CustomerId → CustPaymentBLL.CustomerLoad → CustPaymentDAL.CustomerLoad / CustPaymentDAL.SingleParameter | no | no |
| SInventory_UI_CustomerPayment.customerDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:114 | object sender, EventArgs e | SInventory_UI_CustomerPayment.customerDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_CustomerPayment.marketDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:119 | object sender, EventArgs e | SInventory_UI_CustomerPayment.marketDropDownList_SelectedIndexChanged → CustPaymentBLL.CustomerLoad / CustPaymentBLL.LoadCustomerMaster / CustPaymentBLL.LoadInvoice → CustPaymentDAL.CustomerLoad / CustPaymentDAL.LoadCustomerMaster / CustPaymentDAL.LoadInvoice  | no | no |
| SInventory_UI_CustomerPayment.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:154 | object sender, EventArgs e | SInventory_UI_CustomerPayment.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_CustomerPayment.saveButton_Click | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:249 | object sender, EventArgs e | SInventory_UI_CustomerPayment.saveButton_Click → CustPaymentBLL.SaveCustDetail / CustPaymentBLL.SaveCustPayment / CustPaymentBLL.UpdateInvoicePaymentAmount → CustPaymentDAL.CustPaymentDetailParameters / CustPaymentDAL.CustPaymentParameters / CustPaymentDAL.Exi | no | no |
| SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:329 | object sender, EventArgs e | SInventory_UI_CustomerPayment.payAmountTextBox_TextChanged | no | no |
| SInventory_UI_CustomerPayment.chkSelect_OnCheckedChanged | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs:375 | object sender, EventArgs e | SInventory_UI_CustomerPayment.chkSelect_OnCheckedChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustPaymentBLL.LoadSC | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:167 |
| CustPaymentBLL.PaymentTypeLoadBLL | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:189 |
| CustPaymentBLL.LoadCustomerMaster | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:181 |
| CustPaymentBLL.LoadInvoice | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:193 |
| CustPaymentBLL.CustomerLoad | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:206 |
| CustPaymentBLL.SaveCustPayment | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:33 |
| CustPaymentBLL.SaveCustDetail | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:127 |
| CustPaymentBLL.UpdateInvoicePaymentAmount | Library.BLL/SInventory_BLL/CustPaymentBLL.cs:114 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustPaymentDAL.LoadSC | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:137 | inline SQL: tblCompanyUnit |  |
| CustPaymentDAL.SingleParameter | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:344 | delegates / no SQL literal |  |
| CustPaymentDAL.PaymentTypeLoad | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:180 | inline SQL: tblPaymentType |  |
| CustPaymentDAL.LoadCustomerMaster | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:174 | inline SQL: View_CustomerMaster, tblCustMaster |  |
| CustPaymentDAL.LoadInvoice | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:196 | inline SQL: tblCustPayDetail, tblInvoice, tblInvoiceDetail, tblReturnInvoice |  |
| CustPaymentDAL.CustomerLoad | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:253 | inline SQL: tblCustMaster |  |
| CustPaymentDAL.SaveCustPayment | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:79 | inline SQL: tblCustomerPay |  |
| CustPaymentDAL.CustPaymentParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:296 | delegates / no SQL literal | @ComUnitId, @CreateBy, @CreateDate, @CustPayId, @CustomerMasterId, @MarketId, @PayType, @PaymentAmou |
| CustPaymentDAL.SaveCustDetail | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:102 | inline SQL: tblCustPayDetail, tblCustPayDetail, tblCustomerPay |  |
| CustPaymentDAL.CustPaymentDetailParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:315 | delegates / no SQL literal | @AIT, @BankAccId, @CashAccId, @CollectionBy, @CustPayDetailId, @CustPayId, @DANameId, @Discount, @In |
| CustPaymentDAL.Existence | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:240 | inline SQL: tblCustPayDetail | @InvoiceId, @PaymentAmount |
| CustPaymentDAL.UpdateInvoicePaymentAmount | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:163 | inline SQL: tblInvoice |  |
| CustPaymentDAL.InvoicePaymentParameters | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:334 | delegates / no SQL literal | @InvoiceId, @PaymentAmount, @PaymentStatus |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): View_CustomerMaster, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice, tblInvoiceDetail, tblPaymentType, tblReturnInvoice, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): tblCustPayDetail, tblCustomerPay, tblInvoice
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
| tblInvoice | PaymentAmount | @PaymentAmount |
| tblInvoice | PaymentStatus | @PaymentStatus |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Payment Amount!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Payment Date!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Payment Type!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill out Pay Amount !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Total Invoice Payment Amount Must Be Equel To Payment Amount | UNKNOWN |
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
- try/catch present in 0 of 15 methods.
- Redirect targets: none
- User messages: Cannot Be Greater then Invoice Quantity; Please Select Payment Amount!!; Please Select Payment Date!!; Please Select Payment Type!!; Please fill out Pay Amount !!; Total Invoice Payment Amount Must Be Equel To Payment Amount
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: CreateBy, CreateDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustPaymentBLL; DAL: CustPaymentDAL; DAO: CustPayment, CustPaymentDetail, CustomerMaster; Tables (inline): View_CustomerMaster, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice, tblInvoiceDetail, tblPaymentType, tblReturnInvoice, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx`, `Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 40 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MarketWiseCustomerPayment.aspx |
| Data access | This document §15–17 | 0 SP names, 10 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustPaymentDAL.CustPaymentDetailParameters, CustPaymentDAL.CustPaymentParameters, CustPaymentDAL.CustomerLoad, CustPaymentDAL.Existence, CustPaymentDAL.InvoicePaymentParameters affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
