# WF-M09-04 — Reversing or deleting a customer payment

Two independent, unlinked mechanisms exist. Confidence: CONFIRMED for the C# and inline SQL; UNKNOWN for `sp_InsertCustPayDetail_DeleteLog`.

## A. Payment Reverse (by order code) — PAGE-335 (and SubdeportPaymentReverse for sub-depot invoices)
**Trigger/actor:** any logged-in user with menu access; types an order code.
**Preconditions:** order has an invoice with `DeliveryInvoiceStatus` non-null and not 'Reject'.
**Steps:** `PaymentReverse.aspx.cs` `submitButton_Click1` → `PaymentReverseDal.GetOrderInformationByOrderNo` (first invoice) → `ResetInvoicePaymentStatus` (`UPDATE tblInvoice SET PaymentStatus=NULL, PaymentAmount=NULL`) → `GetPaymentInfoByInvoiceId` (all detail rows) → `DeletePaymentMaster(firstCustPayId)` then `DeletePaymentDetail(firstCustPayId)`.
**Data changed:** tblInvoice (status/amount cleared), tblCustomerPay (1 header deleted), tblCustPayDetail (all detail rows of that header deleted). **Not** changed: accounts vouchers, `PaymentInvoiceNo`/Payment* totals, deposits.
**Branches:** order/invoice not found → "No Information Found !!!"; no payments → status reset with no message; several payments → only the first is removed while status is reset (inconsistent).
**Postcondition:** invoice re-collectable; accounting still shows the original receipt (BR-M09-044).

## B. Customer Payment Delete (by invoice, line-wise, logged) — PAGE-313
**Trigger/actor:** user selects depot + (dates | route) → invoice → clicks Delete on a payment line with remark.
**Steps:** `CustomerPaymentDelete.aspx.cs` `invoiceGridView_RowCommand` → `CustomerPaymentDeleteBLL.DeleteCustomerPaymentDetail(id, remarks, LoginName)` → `sp_InsertCustPayDetail_DeleteLog` (`UpdateAction`, errors swallowed).
**Rules:** remark mandatory (silent no-op if empty); Delete enabled only for lines dated the invoice's latest payment date (UI-only). BR-M09-041/042.
**Data changed:** tblCustPayDetail row (removed/logged — SP body UNKNOWN); no visible change to tblCustomerPay, tblInvoice status or vouchers.

## C. Sales Operation Rollback (transactional, with accounts reversal) — outside the listed M09 pages
`Solution.Web/SInventory_UI/SalesOperationRollback.aspx(.cs)` (session `LoginName` required, :66) → `SalesOperationRollbackBLL.RollbackCustomerPayment(moneyReceiptNo, user, remarks)` → `SalesOperationRollbackDAL.cs:524-688` (single SQL transaction, rolls back on any error): (1) finds the header by **`tblCustomerPay.CollectionNo = @CollectionNo`** (a column that no C# or SQL script in the repo writes or creates — the only ADD COLUMN script adds `Remarks`; existence/population of `CollectionNo` is UNKNOWN, so this lookup may never match receipt numbers shown by the receipt pages); (2) loads detail rows; (3) for every accounts voucher with `TableID=2017 AND CommonPK_ID=CustPayId` inserts a **reversal voucher** (same prefix + yyMMdd + counter, PostingDate = now, `FinancialYearId` copied from the original, narration "Reversal of <code> : …", EntryBy = login) and detail lines with Debit/Credit swapped; (4) per detail: `UPDATE tblInvoice SET PaymentStatus=NULL, PaymentAmount=NULL` and `sp_InsertCustPayDetail_DeleteLog(@Remarks,@CustPayDetailId,@LoginName)` (inside the transaction); (5) `DELETE tblCustPayDetail` and `DELETE tblCustomerPay` for the CustPayId; commit, or rollback and return the error text. This is the only reversal path that reverses accounting (BR-M09-058); it is reachable only if the menu exposes that page.

## Exceptions & risks
No transaction; neither path reverses the voucher created by WF-M09-01 → audit (WF-M09-07) will show orphan vouchers for deleted payments (`MISSING`-type checks run from the payment side; the audit starts from tblCustomerPay so deleted headers are simply absent — orphaned vouchers are NOT detected by that script). Neither path checks that a deposit slip or money receipt was issued.

## Related BR
BR-M09-041…044.

## Related pages
PAGE-313, PAGE-335, PAGE-311, PAGE-306 (aging changes after reversal).

## Tests
TC-313-01…06, TC-335-01…07.
