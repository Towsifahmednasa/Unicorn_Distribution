# WF-M09-06 — Money receipt issue and print

Confidence: CONFIRMED for the flows read (`CustomerPayment`, `MoneyReceiptAfterPayment`, `MoneyReceipt` row command, viewers, `MoneyReceiptPreview`, repo SP `sp_Get_MoneyReceiptReportAfterPaymentList`); `sp_Get_MoneyReceiptReportList` body UNKNOWN; `.rpt` internals UNKNOWN.

## Trigger & actors
(a) Right after saving a collection: modal "Print Money Receipt" on `CustomerPayment.aspx`. (b) Later, a user searches receipts on `MoneyReceiptAfterPayment.aspx` (payment-date based) or `MoneyReceipt.aspx` (older list) and prints. (c) `MoneyReceiptPreview.aspx` shows an HTML receipt by querystring.

## Steps
1. **From payment page:** `btnMoneyReceiptPrint_Click` → `MoneyRecitPrint()` builds `Session["InvoiceIds"]` = SQL fragment `AND INV.InvoiceId IN (...) AND custPay.CustPayId IN (hfCustPayId) AND CONVERT(date, custPay.custPaymentDate)=CONVERT(date,GETDATE())` → popup `SInventory_RPTVIEW/MoneyReceiptViewerForAfterPayment.aspx`.
2. **Viewer** `Page_Init` reads `Session["InvoiceIds"]` → `InvoiceBLL.MoneyReceiptAfterPaymentInfo` → `InvoiceDAL` → `sp_Get_MoneyReceiptReportAfterPaymentList(@Parm)` (repo body: DISTINCT rows from tblCustPayDetail ⨝ tblCustomerPay ⨝ tblInvoice ⟕ customer/order/company unit; columns CustPayDetailId, CustPayId, InvoiceId, OrderNo, CustomerCode/Name, InvoiceNo/Date, DelivaryInvoiceNo/Date, PaymentNo, PaymentDate, PaymentAmount, TPAmount, VatAmount, ReceiptNo, ComUnitName, Address, OrderBy) + company info → DataSet {mainDataTable, companyInfoDataTable} → `rptMoneyReceipt.rpt` → **exported directly as PDF** "Money Receipt No-<ReceiptNo>" (`ExportToHttpResponse`, exception after Response.End ignored) and also bound to the CrystalReportViewer.
3. **Search page** `MoneyReceiptAfterPayment.aspx`: filters depot, route, `custPaymentDate` range, market-structure (group/region/area/territory/subterritory/market) → `CmnCrystaltoView.GetMoneyReceiptAfterPaymentDAL(Parm())` (same SP) → grid with total; row/selection → viewer (via `Session["InvoiceIds"]`). `MoneyReceipt.aspx` does the same with `sp_Get_MoneyReceiptReportList` and `MoneyReceiptViewer2.aspx` (session `InvoiceIds`, `rptMoneyReceipt.rpt`).
4. **Preview** `MoneyReceiptPreview.aspx`: inline SQL over tblCustomerPay ⟕ accounts voucher (TableID 2017) ⟕ detail ⟕ invoice ⟕ customer/market/company/bank by `ReceiptNo | CustPayId | InvoiceId` querystring; receipt no = voucher code, else RefNo, else `RP`+7 digits (BR-M09-054).

## Receipt numbering (inconsistent across pages)
| Source | Rule |
|---|---|
| Report SP (BR-M09-018) | `RefNo` if non-empty else `MR-`+8-digit CustPayId |
| Preview page (BR-M09-054) | accounts VoucherCode (CVC-/CVB-…) else RefNo else `RP`+7-digit id |
| CustomerPaymentReport (BR-M09-057) | voucher code else RefNo else `CT`+8-digit id |
Because `RefNo` = cheque number, multiple payments with the same cheque share a receipt number.

## Data created / changed
None (read-only). Session variable `InvoiceIds` carries an SQL fragment.

## Branches / exceptions
No rows → alert "No Money Receipt Data Found!" (viewer). If `Session["InvoiceIds"]` empty → blank viewer. `MoneyReceiptPreview` swallows all exceptions and reads the connection string itself.

## Related BR / pages / tests
BR-M09-018, 054, 057. Pages PAGE-311, 303, 304, 329, 330, 331. Tests: print receipt for a cheque-paid vs cash payment (numbers differ by rule); print two payments in one session (session fragment overwritten); tamper `hfCustPayId` (SQL injection risk, GAP-M09-007/024).
