---
page_id: PAGE-335
module: M09
page: PaymentReverse
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-335 — PaymentReverse (reverse all payment status for an order's invoice)

> **Evidence level:** Tier A — `PaymentReverse.aspx(.cs)` (113 + 84 lines) and `PaymentReverseDal` (all queries are inline SQL and fully read). Sibling `SubdeportPaymentReverse.aspx(.cs)` (PAGE in module list as "SubdeportPaymentReverse", 79 lines) has identical logic against `tblSubInvoiceMaster` / `SubDeportInvoiceId` and is documented here rather than separately. No SP is involved, so the database dimension is fully verified.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-335 (sibling: SubdeportPaymentReverse.aspx, no separate generated spec file in this folder — see README catalogue) |
| Kind | Entry / Transaction — destructive correction (CONFIRMED) |
| Page class | SInventory_UI_PaymentReverse; DAL `Library.DAL/SInventory_DAL/PaymentReverseDal.cs` |

## 2. Purpose
Given an **order code**, undoes the collection state of its (non-rejected) delivery invoice: sets the invoice's `PaymentStatus` and `PaymentAmount` to NULL and deletes one payment header + its detail rows.

## 3. Business Objective
Emergency correction path so an invoice can be re-collected after a wrong/duplicate payment entry. INFERRED (page button text "Return Payment" in the sub-depot variant).

## 4. Navigation
Menu path UNKNOWN. Reset → self (`Unnamed_Click`).

## 5. Route
`~/SInventory_UI/PaymentReverse.aspx` (sibling `~/SInventory_UI/SubdeportPaymentReverse.aspx`).

## 6. Source Files
`PaymentReverse.aspx(.cs)`, `PaymentReverseDal.cs`, `SInventorySql.cs`; sibling `SubdeportPaymentReverse.aspx(.cs)`.

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| orderNoTextBox | Order No | Yes (`:25`) | free text order code (`tblOrder.OrderCode`) |
| submitbutton (`submitButton_Click1`) | Search/Submit ("Return Payment" on sub-depot page) | – | **no confirmation dialog** |
| Reset link | Reset | – | redirect |

## 8. Field Mapping
`orderNoTextBox.Text.Trim()` → `@OrderNo` → `tblOrder.OrderCode`; resolved `InvoiceId` → `@InvoiceId` (UPDATE tblInvoice) and `CustPayId` (DELETE).

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
Submit → `submitButton_Click1` (:23-50).

## 11. Frontend Flow
Type order code → click → alert "Payment Reverse Succesfully Done !!!" (text box cleared) / "No Information Found !!!" / "Please Insert Order No !!!".

## 12. Backend Flow
1. `GetOrderInformationByOrderNo`: `SELECT INV.InvoiceId FROM tblOrder ODR LEFT JOIN tblInvoice INV ON ODR.OrderId=INV.OrderId WHERE (INV.DeliveryInvoiceStatus IS NOT NULL AND INV.DeliveryInvoiceStatus NOT IN ('Reject')) AND ODR.OrderCode=@OrderNo` — takes **the first row only** (`Rows[0]`).
2. `ResetInvoicePaymentStatus(invoiceId)`: `UPDATE tblInvoice SET PaymentStatus=NULL, PaymentAmount=NULL WHERE InvoiceId=@InvoiceId`.
3. `DeleteCustomerPaymentInfo`: `SELECT CustPayDetailId, CustPayId FROM tblCustPayDetail WHERE InvoiceId=@InvoiceId`; if any row: `DELETE FROM tblCustomerPay WHERE CustPayId=<first row's CustPayId>`, then `DELETE FROM tblCustPayDetail WHERE CustPayId=<same>`; `status` = result of the detail delete.
4. Message only if both `pst` and `payment` true; if reset succeeded but no payment rows existed (status false) **no message at all** is shown.
No transaction; no logging; no accounts action.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL; `PaymentReverseDal` is called directly from the code-behind.

## 15. Repository Mapping
`PaymentReverseDal`: GetOrderInformationByOrderNo, ResetInvoicePaymentStatus, GetPaymentInfoByInvoiceId, DeletePaymentMaster, DeletePaymentDetail (+ subdepo variants); all SQL parameterised via `SInventorySql`.

## 16. Stored Procedure Mapping
None (inline SQL only).

## 17. Database Mapping
tblOrder, tblInvoice (UPDATE PaymentStatus, PaymentAmount), tblCustPayDetail (SELECT/DELETE), tblCustomerPay (DELETE). Sub-depot: tblSubInvoiceMaster + `tblCustPayDetail.SubDeportInvoiceId`.

## 18. Validation Matrix
| Rule | Message | BR |
|---|---|---|
| Order no non-empty | "Please Insert Order No !!!" | BR-M09-043 |
| Invoice exists with DeliveryInvoiceStatus not null and ≠ 'Reject' | "No Information Found !!!" | BR-M09-043 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M09-043 | Reversal targets the invoice of an order whose delivery status is non-null and not 'Reject'; it resets `PaymentStatus`/`PaymentAmount` to NULL (making the invoice "uncollected" again for `PaymentStatus IS NULL OR 'Partial'` filters) and deletes the header+details of only the **first** payment id found for that invoice, even if the invoice has several payments or the header covers other invoices. | PaymentReverse.aspx.cs:23-71; PaymentReverseDal.cs:15-102 | CONFIRMED |
| BR-M09-044 | Reversal is a hard delete: no reversal voucher in `CSTL_ACCDB_Unicorn`, no delete log, no permission check, no confirmation; the payment-stage fields (`PaymentInvoiceNo`, Payment* totals) set by PAGE-334 are untouched, so the invoice immediately re-appears in the collection list with full Due only if the deleted payment was its sole payment. | same | CONFIRMED |

## 20. Permission Matrix
No Session key read; anyone who can open the page can reverse any order's payment. Menu-based only.

## 21. Status Lifecycle
tblInvoice.PaymentStatus: (Full|Partial) → NULL (this page). PaymentAmount → NULL. No other status.

## 22. Error Handling
No try/catch; SQL exceptions surface as error page; messages via alert.

## 23. Audit
None (no who/when/why). Vouchers remain.

## 24. Integration
None (does not touch accounts DB).

## 25. Reports
None.

## 26. Dependencies
`PaymentReverseDal`, tblOrder, tblInvoice, tblCustPayDetail, tblCustomerPay (+ tblSubInvoiceMaster).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-335-01 | Blank order no | "Please Insert Order No !!!" | BR-M09-043 |
| TC-335-02 | Unknown order code / Rejected invoice | "No Information Found !!!" | BR-M09-043 |
| TC-335-03 | Order with one full payment | PaymentStatus/Amount NULL, header+detail deleted, success alert | BR-M09-043 |
| TC-335-04 | Order with two payments | only first CustPayId deleted; status NULL though second payment remains (inconsistent) | BR-M09-043 |
| TC-335-05 | Reverse then check accounts voucher | voucher still exists | BR-M09-044 |
| TC-335-06 | Order whose invoice has no payment | status reset but no message | BR-M09-043 |
| TC-335-07 | Sub-depot page with sub-depot order | same via tblSubInvoiceMaster | BR-M09-043 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/PaymentReverse.aspx(.cs)`, `SubdeportPaymentReverse.aspx(.cs)`, `Library.DAL/SInventory_DAL/PaymentReverseDal.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Accounting reversal | none | none in code | GAP (GAP-M09-003) |

## 30. Known Gaps
No authorisation, confirmation, logging or accounts reversal; partial reversal when multiple payments; multiple invoices per order (only the first); ignores whether a deposit/receipt was issued; no transaction (status reset may succeed while delete fails); relies on hard delete (loses history); duplicate sub-depot page.

## 31. Change Impact
Deletes rows read by aging (PAGE-306), receivable, ledger, deposit-independent reports and money receipts; conflicts with PAGE-313's logged deletion. `PaymentReverseDal` is also used by SubdeportPaymentReverse.
