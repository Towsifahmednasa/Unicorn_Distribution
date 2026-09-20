---
page_id: PAGE-214
module: M06
page: DelivaryInvoiceCreationForCustomerByOrderAuto
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-214 — DelivaryInvoiceCreationForCustomerByOrderAuto (older variant of PAGE-215)

> **Evidence level:** Tier A — `DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs` lines 14-192, 883-1330, 1464-1583 read; same DAL methods as PAGE-215. Differences from PAGE-215 are listed; read PAGE-215 for the shared mechanics. Database PARTIAL (no SP bodies). **Legacy candidate:** the only link to this page (`LoadingSummary.aspx.cs:273`) is commented out and replaced by PAGE-215 (`:272`); a further redirect from `SubDepot_UI/DelivaryInvoiceList.aspx.cs:97` targets a *different* page (`DelivaryInvoiceCreationForCustomerAuto.aspx`). Menu exposure is DB-driven (UNKNOWN), so the page may still be reachable directly — INFERRED.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-214 |
| Module | M06 by classifier; functionally M07 |
| Kind | Entry / Transaction (delivery confirmation with optional auto-payment) |
| LOC | 1846 |
| Suspected legacy | **Yes** (no live inbound link; superseded by PAGE-215; a `…AutoForSalesCnfrm.aspx.exclude` copy also exists) |

## 2. Purpose
Same as PAGE-215 (per-line Full/Partial/Reject delivery confirmation of a proforma from `Session["InvoiceId"]`), plus an "Auto Payment" checkbox (`autopayment`, **Checked=true, Enabled=false** — `aspx:544`) that immediately records a full cash payment for the delivered grand total. CONFIRMED.

## 3. Business Objective
Confirm delivery and, for cash deliveries, settle the invoice in the same step. INFERRED.

## 4. Navigation
Inbound: none live. Back link → `DelivaryInvoiceCreationAuto.aspx` (`.cs:121`).

## 5. Route
`~/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx`.

## 6. Source Files
aspx(.cs) above; DAL/BLL as PAGE-215; `CustPaymentBLL` (`UpdateInvoicePaymentAmount`, `SaveCustPayment`).

## 7. UI Control Inventory
As PAGE-215 (grid columns, `dQtyTextBox`, `statusLabel`, `reasonReturnDropDownList`) plus `autopayment` (always checked & disabled) and `payTypeDDL`. Header customer block via `View_OrderCustomerInfo`.

## 8. Field Mapping
As PAGE-215 §8 for `UpdateInvoice`/`UpdateInvoiceDetail`/`ReturnToDCStore`, **except** there is no `PaymentUpdateInvoice*` call and `PaymentTypeId` forced 0. Auto-payment: `CustPayment{CustomerMasterId, MarketId=0, ComUnitId, PaymentDate=today, PaymentAmount=grandTotal, PayType="Cash", CreateBy=LoginName, CreateDate}` + `CustPaymentDetail{InvoiceId, PaymentAmount}` → `CustPaymentBLL.SaveCustPayment` (customer payment tables; SP/columns UNKNOWN), and `UpdateInvoicePaymentAmount(total,"Full",invoiceId)`.

## 9. Dropdown Mapping
As PAGE-215.

## 10. Button Action Mapping
`saveButton_Click:883`: `Validation()` → `SaveInvoice()` → `SaveInvoiceDetail()` → `if (autopayment.Checked) SavePayment()` → `Clear()` → alert "Create Invoice". catch ⇒ "Some Networks Error blocked Your Transaction Please Contract Administrator!!".

## 11. Frontend Flow
Page_Load: `Session["InvoiceId"]` → `LoadInvoice`; `payTypeDDL.SelectedIndex = 1` executed **before** the list is loaded (`:27-28`) so it has no effect on the loaded items; date today. `LoadAllDataByOrder` (`:194-364`) exists but is not called from Page_Load (dead). Grid recalculation via `DqtyCalculation` (`:1464`).

## 12. Backend Flow
As PAGE-215 with these differences: (a) **no duplicate-confirmation guard**; (b) `aInvoice.DelivaryInvoiceNo = "DEL-" + invoiceNoHiddenField` (`:1213`) and `InvoiceDAL.UpdateInvoice` prepends `"DEL-"` again (`InvoiceDAL.cs:784`) ⇒ stored value `DEL-DEL-INV-…` while the screen shows `DEL-INV-…`; (c) **no `sp_Sales_DeliverySalesPosting` call** — accounting posting is skipped; (d) optional auto full-cash payment.

## 13. API Mapping
None.

## 14. Service Mapping
As PAGE-215 + `CustPaymentBLL`.

## 15. Repository Mapping
As PAGE-215.

## 16. Stored Procedure Mapping
None called directly from this page (UNKNOWN inside `SaveCustPayment`/`UpdateInvoicePaymentAmount`).

## 17. Database Mapping
Written: tblInvoice (Delivery*), tblInvoiceDetail (Delivery*), tblDCStore (Receive), customer payment tables, invoice payment amount/status columns.

## 18. Validation Matrix
As PAGE-215 (`Validation:39-76`): order no, order date, no blank code/tQty/dQty ("Please Input Valid Data!!!"), reason for `"Reject"`/`"Parial"` ("Please Select Reason !!").

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-057 | Delivery status per line and invoice-level roll-up are identical to BR-M06-049/050: dQty==tQty Full; 0<dQty<tQty Partial; dQty>tQty ⇒ 0/Reject; dQty==0 Reject; all Full ⇒ Full, all Reject ⇒ Reject, else Partial. | .cs:1464-1518, 1221-1245 | page | CONFIRMED |
| BR-M06-058 | Auto payment: unconditionally records a full "Cash" payment equal to the delivered grand total (checkbox is permanently checked and disabled), regardless of payment terms or partial delivery status. | .cs:891-894, 1254-1297; aspx:544 | page | CONFIRMED |
| BR-M06-059 | Delivery number is double-prefixed (`DEL-DEL-<InvoiceNo>`) in `tblInvoice.DelivaryInvoiceNo` while the on-screen number is single-prefixed. | .cs:1213, 1216; InvoiceDAL.cs:784; InvoiceBLL.cs:691-697 | page+SQL | CONFIRMED |
| BR-M06-060 | This variant has no repeat-confirmation guard and no journal posting call (contrast BR-M06-051/054). | .cs:883-905 | page | CONFIRMED |
| BR-M06-061 | Short-delivered quantity returns to stock (same rule as BR-M06-053). | .cs:1026-1037 | page+SQL | CONFIRMED |
| BR-M06-062 | Reason check misspells "Partial" (as in PAGE-215). | .cs:63-64 | page | CONFIRMED |

## 20. Permission Matrix
Session `UserId`, `LoginName` only; no role checks.

## 21. Status Lifecycle
Same fields as PAGE-215 (`DelivaryInvoiceNo`, `DeliveryInvoiceStatus`, line `DeliveryStatus`) plus payment status set "Full" by `UpdateInvoicePaymentAmount`.

## 22. Error Handling
Generic error message; no transaction; header saved before details.

## 23. Audit
`UpdateBy`, `UpdateDatetime`; payment `CreateBy/CreateDate`.

## 24. Integration
None visible.

## 25. Reports
None.

## 26. Dependencies
As PAGE-215.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | Full delivery | invoice Full; auto Cash payment = grand total | 057,058 |
| TC-02 | Edge | Partial delivery | Partial; payment still equals grand total of delivered value | 058 |
| TC-03 | Negative | Confirm same invoice twice | second run re-applies updates (no guard) and adds another payment | 060 |
| TC-04 | Edge | Inspect stored delivery no. | `DEL-DEL-INV-…` | 059 |
| TC-05 | Negative | Partial line no reason | accepted (typo) | 062 |

## 28. Reverse Engineering References
`…Auto.aspx(.cs)`, `LoadingSummary.aspx.cs:271-273`, `InvoiceDAL.cs:772-790`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Live use | None existed | no live link | GAP-M06-018 |
| Delivery no. format | None existed | double prefix | GAP-M06-019 |

## 30. Known Gaps
Legacy/orphan variant; double prefix; no accounting posting; no duplicate guard; forced auto-cash payment; dead `LoadAllDataByOrder`; typo; hard-coded codes.

## 31. Change Impact
See PAGE-215 §31. Removing/hiding this page has no code-level dependents (no inbound references found).
