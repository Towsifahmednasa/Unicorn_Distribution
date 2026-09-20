# WF-M09-07 — Sales-to-accounts voucher posting audit (customer payment portion)

Confidence: CONFIRMED (script `SP_Audit_Sales_Accounts_Voucher_Posting.sql`, read-only). No web page calls it (grep: none) — it is a DBA/finance run-on-demand SP.

## Trigger & actors
DBA / finance controller runs `EXEC dbo.sp_Audit_Sales_Accounts_Voucher_Posting @OperationType='CUSTOMER_PAYMENT' [, @FromDate, @ToDate, @ShowOnlyMismatches=1]` on the live sales DB (`ALL` covers material conversion, delivery confirmation, customer payment, sales return, destroy).

## Customer-payment audit logic (lines 148-219)
1. Base: every `tblCustomerPay` row in the date range (by `PaymentDate`, `@ToDate` extended to 23:59:59).
2. Expected amount = Σ over `tblCustPayDetail` of `PaymentAmount + Discount + AIT` for the CustPayId.
3. Voucher = `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster` with `TableID = 2017 AND CommonPK_ID = CustPayId`; detail totals = Σ DebitAmountBDT / Σ CreditAmountBDT.
4. Status (first match wins): `MISSING_VOUCHER` → `MISSING_VOUCHER_DETAILS` → `DR_CR_UNBALANCED` (Dr ≠ Cr exactly) → `MASTER_VALUE_MISMATCH` (|expected − TotalAmountBDT| > 1.00) → `DETAIL_VALUE_MISMATCH` (|expected − Σ Credit| > 1.00) → `OK`.
5. Outputs: per-payment grid, and a KPI summary (total, vouchers found, missing, missing details, unbalanced, value mismatches, OK, Health % = OK/Total).

## Interpretation for M09
- Payments from Excel upload / PPPP / Snd / MarketWise / sub-depot will show `MISSING_VOUCHER` (they never post) — GAP-M09-012.
- Payments whose date is outside any financial year or failed posting show `MISSING_VOUCHER` (BR-M09-015/017).
- Because `sp_Sales_CustomerPaymentPosting` creates one voucher **per (CustPayId, InvoiceId) call** and `CommonPK_ID = CustPayId`, a header with several details could yield several vouchers joined to one row (row multiplication in the audit) — INFERRED; in practice each header has one detail.
- Orphan vouchers (payment deleted/reversed via WF-M09-04) are **not** detected because the audit starts from tblCustomerPay.

## Related BR / pages
BR-M09-056; PAGE-311, PAGE-314; GAP-M09-012, GAP-M09-022.

## Tests
Run after a two-invoice submission and expect 100 %; after an Excel upload expect MISSING_VOUCHER; after PAGE-335 reverse expect no row for the payment and an unmatched voucher remaining in the accounts DB.
