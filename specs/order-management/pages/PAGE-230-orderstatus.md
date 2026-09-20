---
page_id: PAGE-230
module: M06
page: OrderStatus
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-230 — OrderStatus

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-230 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderStatus |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_OrderStatus |
| aspx size | 58188 bytes |
| Code-behind LOC | 317 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderStatus** (Page). Page title/heading text: `Order Information`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/OrderDCChange.aspx`, `MasterSetup_UI/OrderTrackingList.aspx`, `MasterSetup_UI/OrderTrackingListDBH.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/OrderStatus.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OrderStatus.aspx |
| Code-behind | Solution.Web/SInventory_UI/OrderStatus.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderStatusBll (Library.BLL/SInventory_BLL/OrderStatusBll.cs) |
| DAL class | OrderStatusDal (Library.DAL/SInventory_DAL/OrderStatusDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→LinkButton1_Click |
| orderNoTextBox | TextBox |  | no validator found |  | ReadOnly=true |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_OnClick |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| salesCenterCodeTextBox | Label |  | no validator found |  |  |  |  |
| mioCodeTextBox | Label |  | no validator found |  |  |  |  |
| salesCenterNameTextBox | Label |  | no validator found |  |  |  |  |
| mioNameTextBox | Label |  | no validator found |  |  |  |  |
| customerCodeTextBox | Label |  | no validator found |  |  |  |  |
| grossValueTextBox | Label |  | no validator found |  |  |  |  |
| CustomerNameTextBox | Label |  | no validator found |  |  |  |  |
| submissionDateTextBox | Label |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | onrowdatabound→GridView1_RowDataBound, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | Total TP | no validator found |  |  |  |  |
| (no id) | BoundField | Unit Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Dis. Percent | no validator found |  |  |  |  |
| (no id) | BoundField | Dis. Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Name | no validator found |  |  |  |  |
| (no id) | BoundField | Gift/Bonus Product | no validator found |  |  |  |  |
| proformaMessageLabel | Label |  | no validator found |  |  |  |  |
| invoiceNumberLabel | Label |  | no validator found |  |  |  |  |
| invoiceDateLabel | Label |  | no validator found |  |  |  |  |
| tpTotalLabel | Label |  | no validator found |  |  |  |  |
| tpDiscountLabel | Label |  | no validator found |  |  |  |  |
| tpVatLabel | Label |  | no validator found |  |  |  |  |
| tpGrandTotalLabel | Label |  | no validator found |  |  |  |  |
| proformaGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Percentage | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| deliveryInvoiceMsgLabel | Label |  | no validator found |  |  |  |  |
| deliveryInvoiceNumberLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPTotalLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPDiscountLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPVATLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPGrandTotalLabel | Label |  | no validator found |  |  |  |  |
| deliveryStatusLabel | Label |  | no validator found |  |  |  |  |
| deliveryGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Percentage | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| paymentMsgLabel | Label |  | no validator found |  |  |  |  |
| paymentAmountLabel | Label |  | no validator found |  |  |  |  |
| paymentStatusLabel | Label |  | no validator found |  |  |  |  |
| orderNoTextBox | TextBox |  | no validator found |  |  |  |  |
| searchButton | Button | Search | no validator found |  |  |  | OnClick→searchButton_OnClick |
| salesCenterCodeTextBox | Label |  | no validator found |  |  |  |  |
| mioCodeTextBox | Label |  | no validator found |  |  |  |  |
| salesCenterNameTextBox | Label |  | no validator found |  |  |  |  |
| mioNameTextBox | Label |  | no validator found |  |  |  |  |
| customerCodeTextBox | Label |  | no validator found |  |  |  |  |
| grossValueTextBox | Label |  | no validator found |  |  |  |  |
| CustomerNameTextBox | Label |  | no validator found |  |  |  |  |
| submissionDateTextBox | Label |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | Trade Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Trade Price | no validator found |  |  |  |  |
| proformaMessageLabel | Label |  | no validator found |  |  |  |  |
| invoiceNumberLabel | Label |  | no validator found |  |  |  |  |
| invoiceDateLabel | Label |  | no validator found |  |  |  |  |
| tpTotalLabel | Label |  | no validator found |  |  |  |  |
| tpDiscountLabel | Label |  | no validator found |  |  |  |  |
| tpVatLabel | Label |  | no validator found |  |  |  |  |
| tpGrandTotalLabel | Label |  | no validator found |  |  |  |  |
| proformaGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Percentage | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| deliveryInvoiceMsgLabel | Label |  | no validator found |  |  |  |  |
| deliveryInvoiceNumberLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPTotalLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPDiscountLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPVATLabel | Label |  | no validator found |  |  |  |  |
| deliveryTPGrandTotalLabel | Label |  | no validator found |  |  |  |  |
| deliveryStatusLabel | Label |  | no validator found |  |  |  |  |
| deliveryGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price | no validator found |  |  |  |  |
| (no id) | BoundField | Total Price Vat Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount Percentage | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| paymentMsgLabel | Label |  | no validator found |  |  |  |  |
| paymentAmountLabel | Label |  | no validator found |  |  |  |  |
| paymentStatusLabel | Label |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| orderNoTextBox | aTable | SInventory_UI_OrderStatus.searchButton_OnClick | OrderStatusBll.CheckDeliveryInvoiceExistOrNot, OrderStatusBll.CheckInvoiceExistOrNot, OrderStatusBll.CheckPaymentStatus, | OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot, OrderStatusDal.GetDeliveryInvoiceInfo, OrderStatusDal.GetInvoiceExistO | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton | — | LinkButton1_Click | — | — | — | — | UNKNOWN (handler not resolved) |
| submitButton | LinkButton click | (postback) | SInventory_UI_OrderStatus.searchButton_OnClick | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:48 | OrderStatusBll.CheckDeliveryInvoiceExistOrNot, OrderStatusBll.CheckInvoiceExistOrNot, OrderStatusBll | OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot, OrderStatusDal.GetDeliveryInvoiceInfo, OrderStatus | — | read-only / none detected |
| cancelButton | LinkButton click | (postback) | SInventory_UI_OrderStatus.cancelButton_Click | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:31 | — | — | — | read-only / none detected |
| searchButton | Button click | (postback) | SInventory_UI_OrderStatus.searchButton_OnClick | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:48 | OrderStatusBll.CheckDeliveryInvoiceExistOrNot, OrderStatusBll.CheckInvoiceExistOrNot, OrderStatusBll | OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot, OrderStatusDal.GetDeliveryInvoiceInfo, OrderStatus | — | read-only / none detected |

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
| SInventory_UI_OrderStatus.Page_Load | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_OrderStatus.Page_Load → OrderStatusBll.CheckDeliveryInvoiceExistOrNot / OrderStatusBll.CheckInvoiceExistOrNot / OrderStatusBll.LoadDeliveryInvoiceInfo / OrderStatusBll.LoadOrderStatusInfo / OrderStatusBll.LoadProformaInvoice → OrderStatusDal.GetI | no | no |
| SInventory_UI_OrderStatus.cancelButton_Click | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:31 | object sender, EventArgs e | SInventory_UI_OrderStatus.cancelButton_Click | no | no |
| SInventory_UI_OrderStatus.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:36 | object sender, EventArgs e | SInventory_UI_OrderStatus.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_OrderStatus.searchButton_OnClick | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:48 | object sender, EventArgs e | SInventory_UI_OrderStatus.searchButton_OnClick → OrderStatusBll.CheckDeliveryInvoiceExistOrNot / OrderStatusBll.CheckInvoiceExistOrNot / OrderStatusBll.CheckPaymentStatus / OrderStatusBll.LoadDeliveryInvoiceInfo / OrderStatusBll.LoadOrderStatusInfo  → OrderSta | yes | no |
| SInventory_UI_OrderStatus.PaymentStatusOperation | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:127 | string orderNo | SInventory_UI_OrderStatus.PaymentStatusOperation → OrderStatusBll.CheckPaymentStatus → OrderStatusDal.GetCheckPaymentStatus | no | no |
| SInventory_UI_OrderStatus.GridView1_RowDataBound | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:151 | object sender, GridViewRowEventArgs e | SInventory_UI_OrderStatus.GridView1_RowDataBound | no | no |
| SInventory_UI_OrderStatus.DeliveryInvoiceOperation | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:163 | string orderNo | SInventory_UI_OrderStatus.DeliveryInvoiceOperation → OrderStatusBll.CheckDeliveryInvoiceExistOrNot / OrderStatusBll.CheckPaymentStatus / OrderStatusBll.LoadDeliveryInvoiceInfo → OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot / OrderStatusDal.GetCheckPaymentS | no | no |
| SInventory_UI_OrderStatus.ProformaInvoiceOperation | Solution.Web/SInventory_UI/OrderStatus.aspx.cs:213 | string orderNo | SInventory_UI_OrderStatus.ProformaInvoiceOperation → OrderStatusBll.CheckDeliveryInvoiceExistOrNot / OrderStatusBll.CheckInvoiceExistOrNot / OrderStatusBll.CheckPaymentStatus / OrderStatusBll.LoadDeliveryInvoiceInfo / OrderStatusBll.LoadProformaInvoiceI → Orde | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderStatusBll.LoadOrderStatusInfo | Library.BLL/SInventory_BLL/OrderStatusBll.cs:15 |
| OrderStatusBll.CheckInvoiceExistOrNot | Library.BLL/SInventory_BLL/OrderStatusBll.cs:23 |
| OrderStatusBll.LoadProformaInvoiceInfo | Library.BLL/SInventory_BLL/OrderStatusBll.cs:28 |
| OrderStatusBll.CheckDeliveryInvoiceExistOrNot | Library.BLL/SInventory_BLL/OrderStatusBll.cs:33 |
| OrderStatusBll.LoadDeliveryInvoiceInfo | Library.BLL/SInventory_BLL/OrderStatusBll.cs:38 |
| OrderStatusBll.CheckPaymentStatus | Library.BLL/SInventory_BLL/OrderStatusBll.cs:43 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderStatusDal.GetOrderStatusInfo | Library.DAL/SInventory_DAL/OrderStatusDal.cs:13 | inline SQL: tblOrder, tblOrderDetail | @OrderNumber |
| OrderStatusDal.GetInvoiceExistOrNot | Library.DAL/SInventory_DAL/OrderStatusDal.cs:32 | inline SQL: tblInvoice | @OrderNo |
| OrderStatusDal.GetProformaInvoiceInfo | Library.DAL/SInventory_DAL/OrderStatusDal.cs:42 | inline SQL: tblInvoice, tblInvoiceDetail | @OrderNo |
| OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot | Library.DAL/SInventory_DAL/OrderStatusDal.cs:53 | inline SQL: tblInvoice, tblInvoiceDetail | @OrderNo |
| OrderStatusDal.GetDeliveryInvoiceInfo | Library.DAL/SInventory_DAL/OrderStatusDal.cs:65 | inline SQL: tblInvoice, tblInvoiceDetail | @OrderNo |
| OrderStatusDal.GetCheckPaymentStatus | Library.DAL/SInventory_DAL/OrderStatusDal.cs:76 | inline SQL: tblInvoice | @OrderNo |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblInvoice, tblInvoiceDetail, tblOrder, tblOrderDetail
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler searchButton_OnClick) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Search by Order Number !!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 13 methods.
- Redirect targets: none
- User messages: Please Search by Order Number !!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderStatusBll; DAL: OrderStatusDal; DAO: —; Tables (inline): tblInvoice, tblInvoiceDetail, tblOrder, tblOrderDetail

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click cancelButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click cancelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OrderStatus.aspx`, `Solution.Web/SInventory_UI/OrderStatus.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/OrderStatus.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 115 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/OrderStatus.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderStatusDal.GetCheckDeliveryInvoiceExistOrNot, OrderStatusDal.GetCheckPaymentStatus, OrderStatusDal.GetDeliveryInvoiceInfo, OrderStatusDal.GetInvoiceExistOrNot, OrderStatusDal.GetOrderStatusInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
