# WF-M09-01 — Customer payment collection, TP/VAT split and accounts voucher posting

Confidence: CONFIRMED for the C# path and repo SP bodies; UNKNOWN for `sp_Update_InvoiceFinalPayment`, `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_Sales_DeliveryCOGSPosting`.

## Trigger & actors
Depot user (DIC/MIO staff; role not enforced) opens `CustomerPayment.aspx` after cash/cheque/bank money is received from a customer. Downstream actor: accounting (reads posted vouchers), customer (receives money receipt).

## Preconditions
- Invoice delivered and payment-confirmed: `tblInvoice.PaymentInvoiceNo IS NOT NULL` (PAGE-334 / WF-M09-02), `SndReturnInvoiceNo IS NULL`, Due > 0 (BR-M09-001).
- User has the depot in `tblUserCompanyUnit`; a cash or bank ledger account exists for the invoice's company in the accounts DB; the payment date lies inside a `tblFinancialYear` of the company (otherwise no voucher, BR-M09-015).

## Steps
1. **Search** — `CustomerPayment.aspx` `submitButton_Click` → `LoadGridView` (:814) → `OrderInfoDAL.LoadPaymentInvSP(GenerateParam())` → `sp_GET_PaymentInvSP` (dynamic `@param`, oldest first). Grid shows Confirm Amount / Previous Pay / Due.
2. **Select rows** — `chkSelect_CheckedChanged` (:898): pay=Due, discount=0.00, AIT=0.00; cash/bank flag by invoice PaymentTypeName; account list from `CustPaymentDAL.LoadBankAccount/LoadCashAccount(CompanyId)` (accounts DB). Optional bulk **Disburse** amount (JS/`ApplyDisbursement`) fills oldest first (BR-M09-005).
3. **Per-edit TP/VAT** — `PayAmountChange` (:599) → `sp_GET_PaymentInvSPTPVATAmt(@InvoiceId,@PayAmount)`; hidden `hfTP_Pay`/`hfVat_Pay` set by VAT-first rule (BR-M09-006); over-payment reset (BR-M09-004).
4. **Submit** — `saveButton_Click` (:354) after `Validation()` (depot, customer, date, ≥1 row, pay amount present, bank/cash chosen).
5. Per ticked row: `sp_Check_Duplicate_InvoiceFinalPayment` → status Full/Partial → **`sp_Update_InvoiceFinalPayment(invoice, pay+AIT+discount, status, login)`** → detail added to shared list → `CustPaymentBLL.SaveCustPayment` → `INSERT tblCustomerPay` (new id = MAX+1) then `SaveCustDetail` for each list entry (`INSERT tblCustPayDetail` with TP, VAT, AIT, discount, bank/cash id, DA, collection-by, payment date; skipped if same invoice+date+amount exists).
6. **Accounts posting** — for every detail in the list `CustPaymentBLL.PaymentJournalPosting(custPayId, invoiceId)` → `CustPaymentDAL` → `sp_Sales_CustomerPaymentPosting` (cross-DB):
   a. cursor over `tblCustPayDetail` rows where pay+discount+AIT > 0 for that CustPayId+InvoiceId joined to company (ERP DB), financial year (accounts DB, by payment date), customer subsidiary ledger (`SubsidiaryTypeId=1`), reference info (`ReferenceTypeId=1016` = invoice);
   b. voucher code `CVB-/CVC-` + yyMMdd + counter; type `CreditVoucherBank` or `CreditVoucherCash` (BR-M09-013);
   c. INSERT `tblDebitCreditVoucherMaster` (approved, "Auto Posting", EntryBy `admin`, CommonPK_ID=CustPayId, TableID from `tbl_AccIntegrationTable` for `tblCustomerPay`);
   d. detail lines: **Dr** bank/cash GL (pay) · **Dr** discount GL (163/1023) · **Dr** AIT GL (163/1024) · **Cr** receivable GL (163/2, with customer subsidiary id) for pay+discount+AIT (BR-M09-012/014);
   e. after the cursor: `DueAmount = DeliveryTpGrandTotal + DeliveryCharge − Σ(pay+disc+AIT)`; if ≤ 0 → `EXEC sp_Sales_DeliveryCOGSPosting @InvoiceId` (BR-M09-016).
7. **Outcome** — if last processed row's `save` true: `hfCustPayId` = ids; modal "Payment Saved Successfully" → **Print Money Receipt** (WF-M09-06) or Close. Else alert "Already Exist!".

## Data created / changed
| Table | Change |
|---|---|
| tblCustomerPay | +1 row per ticked invoice row |
| tblCustPayDetail | +1 row per invoice payment (TP/VAT/AIT/discount/account/collection-by) |
| tblInvoice | payment status/amount updated by `sp_Update_InvoiceFinalPayment` (columns UNKNOWN) |
| CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster/Detail | +1 voucher master and 3-4 detail lines per detail (if FY/company/mapping exist) |
| (COGS SP) | UNKNOWN |
Status transitions: invoice collection status NULL/Partial → Partial/Full (UNKNOWN storage); voucher: created Approved.

## Branches / exceptions
- Duplicate detected → row skipped, "Already Exist!" if the *last* row's result is false (earlier successes not reported).
- Posting SP error → swallowed by `ClsCommonInternalDAL.UpdateAction`; payment stays without voucher (BR-M09-017).
- Payment date outside financial year → no voucher, no message (BR-M09-015).
- No bank/cash id (tampered) → voucher type NULL, code without prefix (BR-M09-013).
- Shared detail list: header *k* receives details 0..*k*; earlier ones skipped by guard; posting loop re-runs for earlier invoices (no new voucher because CustPayId differs) but COGS check re-runs (GAP-M09-004).
- No DB transaction anywhere.

## Postconditions
Invoice Due reduced by pay+discount+AIT; receipt available; accounts DB holds a balanced approved voucher per detail (Dr = pay+discount+AIT = Cr). Verify with WF-M09-07.

## Related BR
BR-M09-001…020 (see PAGE-311 §19).

## Related pages
PAGE-311, PAGE-334 (upstream), PAGE-330/303/304/331 (receipts), PAGE-306/336/316 (reports), PAGE-313/335 (undo).

## Tests
TC-311-01 … TC-311-15 (PAGE-311 §27); add: reconcile `sp_Audit_Sales_Accounts_Voucher_Posting @OperationType='CUSTOMER_PAYMENT'` = 100 % OK after a multi-invoice submit.
