# WF-M09-03 — Bulk customer payment upload from Excel

Confidence: CONFIRMED (page and repo SP body `sp_GET_PaymentInvSPPaymentAmount`); UNKNOWN for `sp_Update_InvoiceFinalPayment`, `sp_Check_Duplicate_InvoiceFinalPayment`.

## Trigger & actors
Back-office/DA user with a collection sheet (Excel `.xls`/`.xlsx`, first sheet): columns A Invoice No, B Amount, C Collection By (`MIO`/`DIC`).

## Preconditions
Invoices are payment-confirmed and `PaymentStatus IS NULL OR 'Partial'`; OLE DB Jet/ACE drivers on the server; write access to `~/ExcelFiles/`; a route and DA selected (DA mandatory).

## Steps
1. `CustomerPaymentExcelUpload.aspx` `btnUpload_Click` → `ExcelToGrid` (:516): saves upload under `~/ExcelFiles/<clientFileName>`, reads sheet, skips rows with empty column A.
2. For each row: `OrderInfoDAL.LoadPaymentInvSPWithPaymentAmount(" and  InvoiceNo='<A>'", "<B F2>", "<C>")` → `sp_GET_PaymentInvSPPaymentAmount` (dynamic SQL; amount and collection-by concatenated) → 0/1 grid row with `PayNewAmount`, Due, Confirm Amount, Previous Pay.
3. Rows merged/bound (all ticked), `PayAmountChange(i)` on each row → `sp_GET_PaymentInvSPTPVATAmt` → hidden TP/VAT; total label.
4. User reviews, may untick/change amounts; **ALL Submit** → `Validation` (DA, ≥1 row, non-zero, MIO/DIC, pay+previous ≤ confirm) → `saveButton_Click`: per ticked row header (PayType "Cash", date today, ComUnitId from invoice) → duplicate SP → `sp_Update_InvoiceFinalPayment` → shared list → `SaveCustPayment` (tblCustomerPay, tblCustPayDetail).
5. Success → SweetAlert → reload page.

## Data created / changed
tblCustomerPay, tblCustPayDetail (no AIT/discount/bank/cash ids), tblInvoice (via SP). **No accounts voucher** (BR-M09-034).

## Branches / exceptions
Invoice not found/ineligible → silently no grid row; upload parse errors → alert; duplicate → "Already Exist!"; injection risk in sheet cells (GAP-M09-007).

## Postconditions
Invoice dues reduced; accounting has no matching voucher, so posting audit (WF-M09-07) will flag `MISSING_VOUCHER` for these payments unless posted elsewhere (UNKNOWN).

## Related BR
BR-M09-031…034, 006, 010.

## Related pages
PAGE-314, PAGE-311.

## Tests
TC-314-01…08.
