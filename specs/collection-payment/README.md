# M09 — Collections, Payments & Money Receipts

Folder: `specs/collection-payment/` · Generated/reviewed: 2026-09-19 · Source: `Solution.Web/SInventory_UI/*`, `Solution.Web/SInventory_RPTVIEW/*`, `Library.BLL/SInventory_BLL/CustPaymentBLL.cs` & friends, `Library.DAL/SInventory_DAL/*`, root SQL scripts (`Live_Deployment_Script_All_Updates.sql`, `SP_Audit_Sales_Accounts_Voucher_Posting.sql`, `SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql`, `SP_Fix_CustomerPayment_Live.sql`). No specification existed before this reverse-engineering. Confidence tags: CONFIRMED (read in code), INFERRED, UNKNOWN.

## 1. Purpose & business overview
After a distributor invoice is delivered and (optionally) reduced at "payment confirmation" (`PaymentPartial`), this module records the money coming back: depot users enter **customer collections** against invoices (`CustomerPayment`), optionally with **discount** and **AIT** deductions, choosing a **cash or bank ledger account**. The system splits each collection into **TP** and **VAT** parts, updates invoice payment status, **auto-posts an approved credit voucher into the accounting database** (`CSTL_ACCDB_Unicorn`), and issues a **money receipt**. Related activities: bulk Excel uploads of collections, reversal/deletion of wrongly entered collections, **bank deposit slips** (cash banked by MIO/DIC, uploaded from Excel and listed), and a family of **ledger / receivable / aging / collection reports**.

Key facts (all CONFIRMED unless stated):
- Only `CustomerPayment.aspx` posts to accounts. Excel upload, legacy PPPP/Snd, MarketWise and sub-depot pages record payments **without** any voucher (GAP-M09-012).
- Payment **allocation** is per invoice, oldest-first when the bulk "Disburse" box is used; full vs partial is decided by exact equality with the confirm amount; **there is no advance/on-account payment and no over-payment carry-forward**.
- Collections are "settlements" = pay + discount + AIT; all outstanding/aging maths add all three.
- **Aging with buckets** exists only as labels on `Aging.aspx` (1-10, 11-20, 21-30, 31-40, 41-50, 51+ days) whose SP is not in the repo; `CustomerInvoiceAgingReport` computes only "Credit Days" (no buckets).

## 2. Page catalogue
Tier A pages (9) were fully reviewed; the rest are generated Tier-B leads (regex-extracted) supplemented in this README/reports where I read the code. Two pages inside the module by name but without a Tier-B spec: `SubdeportPaymentReverse.aspx` (documented inside PAGE-335) and `BankDepositSAP.aspx` (not in the 43-page list; sibling of the deposit reports, `LoadBankDepositSAP`).

| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-299 | BatchWiseCollectionReport | Report | B | [spec](pages/PAGE-299-batchwisecollectionreport.md) |
| PAGE-300 | BatchWiseCollectionReportViewer | Report viewer | B | [spec](pages/PAGE-300-batchwisecollectionreportviewer.md) |
| PAGE-301 | CustomerLedgerViewer | Report viewer | B | [spec](pages/PAGE-301-customerledgerviewer.md) |
| PAGE-302 | CustomerPaymentViewer | Report viewer | B | [spec](pages/PAGE-302-customerpaymentviewer.md) |
| PAGE-303 | MoneyReceiptViewer2 | Report viewer | B | [spec](pages/PAGE-303-moneyreceiptviewer2.md) |
| PAGE-304 | MoneyReceiptViewerForAfterPayment | Report viewer | B | [spec](pages/PAGE-304-moneyreceiptviewerforafterpayment.md) |
| PAGE-305 | Aging | Report | B | [spec](pages/PAGE-305-aging.md) |
| PAGE-306 | CustomerInvoiceAgingReport | Report (invoice aging/outstanding) | **A** | [spec](pages/PAGE-306-customerinvoiceagingreport.md) |
| PAGE-307 | CustomerLedgerReport | Report | B | [spec](pages/PAGE-307-customerledgerreport.md) |
| PAGE-308 | CustomerLedgerReportApps | Report | B | [spec](pages/PAGE-308-customerledgerreportapps.md) |
| PAGE-309 | CustomerLedgerReportNew | Report | B | [spec](pages/PAGE-309-customerledgerreportnew.md) |
| PAGE-310 | CustomerOutstandingReport | Report | B | [spec](pages/PAGE-310-customeroutstandingreport.md) |
| PAGE-311 | CustomerPayment | Entry / Transaction (collection + voucher) | **A** | [spec](pages/PAGE-311-customerpayment.md) |
| PAGE-312 | CustomerPayment - Copy | Entry (duplicate of 311, older) | B | [spec](pages/PAGE-312-customerpayment-copy.md) |
| PAGE-313 | CustomerPaymentDelete | Corrective delete | **A** | [spec](pages/PAGE-313-customerpaymentdelete.md) |
| PAGE-314 | CustomerPaymentExcelUpload | Bulk upload of collections | **A** | [spec](pages/PAGE-314-customerpaymentexcelupload.md) |
| PAGE-315 | CustomerPaymentPPPP | Entry (legacy route-based) | **A** | [spec](pages/PAGE-315-customerpaymentpppp.md) |
| PAGE-316 | CustomerPaymentReport | Report (inline SQL) | B | [spec](pages/PAGE-316-customerpaymentreport.md) |
| PAGE-317 | CustomerPaymentReportPDF | Report (inline SQL, PDF) | B | [spec](pages/PAGE-317-customerpaymentreportpdf.md) |
| PAGE-318 | CustomerPaymentSnd | Entry (second-return invoices) | **A** | [spec](pages/PAGE-318-customerpaymentsnd.md) |
| PAGE-319 | DACollectionReport | Report | B | [spec](pages/PAGE-319-dacollectionreport.md) |
| PAGE-320 | DeliveryPaymentReport | Report | B | [spec](pages/PAGE-320-deliverypaymentreport.md) |
| PAGE-321 | DeliveryPaymentReportNew | Report | B | [spec](pages/PAGE-321-deliverypaymentreportnew.md) |
| PAGE-322 | DepositList | List / delete deposit slips | B | [spec](pages/PAGE-322-depositlist.md) |
| PAGE-323 | DepositSlipExcelUpload | Upload deposit slips | B | [spec](pages/PAGE-323-depositslipexcelupload.md) |
| PAGE-324 | DepositSlipExcelUpload_old | Upload (superseded) | B | [spec](pages/PAGE-324-depositslipexcelupload-old.md) |
| PAGE-325 | DepositSlipReport | Report | B | [spec](pages/PAGE-325-depositslipreport.md) |
| PAGE-326 | DepositSlipReport_T | Report | B | [spec](pages/PAGE-326-depositslipreport-t.md) |
| PAGE-327 | DHB_DeliveryPaymentReport | Report | B | [spec](pages/PAGE-327-dhb-deliverypaymentreport.md) |
| PAGE-328 | MarketWiseCustomerPayment | Entry (legacy, no posting) | B | [spec](pages/PAGE-328-marketwisecustomerpayment.md) |
| PAGE-329 | MoneyReceipt | Receipt search & print (pre-payment list) | B | [spec](pages/PAGE-329-moneyreceipt.md) |
| PAGE-330 | MoneyReceiptAfterPayment | Receipt search & print | B | [spec](pages/PAGE-330-moneyreceiptafterpayment.md) |
| PAGE-331 | MoneyReceiptPreview | Receipt preview (inline SQL) | B | [spec](pages/PAGE-331-moneyreceiptpreview.md) |
| PAGE-332 | NewReceiveableReport | Report | B | [spec](pages/PAGE-332-newreceiveablereport.md) |
| PAGE-333 | NewReceiveableReportApps | Report | B | [spec](pages/PAGE-333-newreceiveablereportapps.md) |
| PAGE-334 | PaymentPartial | Payment-stage delivery confirmation / returns | **A** | [spec](pages/PAGE-334-paymentpartial.md) |
| PAGE-335 | PaymentReverse (+ SubdeportPaymentReverse) | Destructive reversal | **A** | [spec](pages/PAGE-335-paymentreverse.md) |
| PAGE-336 | RptMIOWiseReceiveableReport | Receivable summary report | **A** | [spec](pages/PAGE-336-rptmiowisereceiveablereport.md) |
| PAGE-337 | Sales_Collection_Reports_Acc | Report | B | [spec](pages/PAGE-337-sales-collection-reports-acc.md) |
| PAGE-338 | SalesCollectionReport | Report | B | [spec](pages/PAGE-338-salescollectionreport.md) |
| PAGE-339 | SC_CustomerPaymentReport | Report | B | [spec](pages/PAGE-339-sc-customerpaymentreport.md) |
| PAGE-340 | SC_PaymentReport | Report | B | [spec](pages/PAGE-340-sc-paymentreport.md) |
| PAGE-341 | StockValuationLedgerReport | Report (stock valuation ledger — belongs to inventory) | B | [spec](pages/PAGE-341-stockvaluationledgerreport.md) |

## 3. Business rules index (BR-M09-nnn)
| ID | One-line rule | Page(s) |
|---|---|---|
| BR-M09-001 | Collectable invoice = has `PaymentInvoiceNo`, no `SndReturnInvoiceNo`, Due>0 (payment-stage amounts) | 311 |
| BR-M09-002 | Payable ("Confirm Amount") = Σ PaymentNetAmount + DeliveryCharge; returns not deducted | 311 |
| BR-M09-003 | Ticking a row defaults pay=Due, discount=0, AIT=0; ≥1 row required | 311 |
| BR-M09-004 | pay+discount+AIT+previous ≤ Confirm Amount (UI-event check only) | 311, 315 |
| BR-M09-005 | Bulk disburse allocates oldest-first, leftover discarded; no advance | 311 |
| BR-M09-006 | TP/VAT split: VAT first, then TP | 311, 314, 315, 318 |
| BR-M09-007 | Status Full iff pay+AIT+discount+previous == Confirm Amount, else Partial | 311 |
| BR-M09-008 | Each row needs one cash/bank ledger account; default by invoice payment type | 311 |
| BR-M09-009 | Collection-By default from role/designation (X. MIO / MIO / DIC) | 311 |
| BR-M09-010 | Duplicate guard (SP + same invoice/date/amount insert guard) | 311, 314 |
| BR-M09-011 | One tblCustomerPay header per ticked row; RefNo=cheque no | 311 |
| BR-M09-012 | Auto-posted approved voucher structure (Dr cash/bank, discount, AIT; Cr AR) | 311 |
| BR-M09-013 | Voucher type/code rules (CVB-/CVC-, yyMMdd+counter; NULL type if no account) | 311 |
| BR-M09-014 | GL resolution via chart of accounts and Table_Integration_AccountsConnectivity (PageId 163) | 311 |
| BR-M09-015 | Voucher posted only if payment date is inside a financial year; else silent skip | 311 |
| BR-M09-016 | COGS posting when delivery-stage due ≤ 0 | 311 |
| BR-M09-017 | Posting failure is swallowed; payment stays | 311 |
| BR-M09-018 | Receipt no = RefNo else `MR-`+8-digit CustPayId (report SP) | 311, 330 |
| BR-M09-019 | Payment date user-editable, drives FY/voucher date | 311 |
| BR-M09-020 | Depot and customer mandatory | 311 |
| BR-M09-021 | Payment-stage confirmation allowed once per invoice | 334 |
| BR-M09-022 | Calculate before save (flag never reset) | 334 |
| BR-M09-023 | Line status Full/Partial/Reject by DQty vs TQty; header roll-up | 334 |
| BR-M09-024 | Return reason required for Reject (typo lets Partial skip) | 334 |
| BR-M09-025 | Accepted qty ≤ confirmed qty (`<=` bug in handler) | 334 |
| BR-M09-026 | Not-accepted qty returns to DC stock | 334 |
| BR-M09-027 | Line amounts: TP, VAT, discount, NP formulas | 334 |
| BR-M09-028 | Stamps PaymentInvoiceNo "RTN-…" making invoice collectable | 334 |
| BR-M09-029 | Zero-TP lines zeroed in totals | 334 |
| BR-M09-030 | Discount % source order (fixed customer, old policy, campaigns, trade term) | 334 |
| BR-M09-031 | Excel layout InvoiceNo/Amount/CollectionBy(MIO|DIC) | 314 |
| BR-M09-032 | Bulk save validations incl. server over-payment check | 314 |
| BR-M09-033 | Only PaymentStatus NULL/Partial invoices match upload | 314 |
| BR-M09-034 | Excel payments: Cash, no accounts, no discount/AIT, **no voucher** | 314 |
| BR-M09-035 | Legacy route-based variant rules (no posting) | 315 |
| BR-M09-036 | Legacy CollectionBy from DA text | 315 |
| BR-M09-037 | Second-return invoices excluded from 311; collected only on 318 | 318 |
| BR-M09-038 | Snd save rules = legacy variant | 318 |
| BR-M09-039 | Receivable report type ↔ SP @Type; DZSM/AM UI steering | 336 |
| BR-M09-040 | Footer sums rounded to whole taka; receivable formula in SP | 336 |
| BR-M09-041 | Delete payment line: remark required, UI-only "latest date" limit | 313 |
| BR-M09-042 | Delete does not reverse voucher/status in C# | 313 |
| BR-M09-043 | Reverse: reset invoice status, delete first payment header+details | 335 |
| BR-M09-044 | Reverse is hard delete, no voucher reversal/log/permission | 335 |
| BR-M09-045 | Outstanding = Σ DeliveryNetAmount+DeliveryCharge − Σ(pay+disc+AIT ≤ as-on) >0 | 306 |
| BR-M09-046 | "Credit Days" = DATEDIFF(day, InvoiceDate, AsOn); no buckets | 306 |
| BR-M09-047 | Payments counted by payment date ≤ as-on | 306 |
| BR-M09-048 | Aging uses delivery-stage amounts (differs from collection list) | 306 |
| BR-M09-049 | `@OnlyDueInvoices` unused; sales-person via customer master; no depot security | 306 |
| BR-M09-050 | Deposit slip delete: only users 51419000000 / 50588, deposit dated within today−60 days; soft delete (`IsDelete`) enforced in UI and in the UPDATE's WHERE | 322 (`DepositList.aspx.cs:155-159`, `CompanywisebranchDal.cs:115-118`) — CONFIRMED |
| BR-M09-051 | Deposit slip upload: typed Total Amount must equal Σ Excel amounts; every row needs bank + mapped account for the user's depot (`sp_GET_BAnkInfoById_ByIdExcel`, `sp_Get_BankSAPMappingAccounTNo`) or the row is red and the save blocked; company = `Session["ComUnitId"]` | 323 (`DepositSlipExcelUpload.aspx.cs:386-524`) — CONFIRMED |
| BR-M09-052 | Deposit rows are inserted one by one via `sp_I_Diposit_New` (DepositCode from `sp_Get_DepositCodeByDIC`, MIO from SAP territory code); success message tests only the id returned for the **last** row | 323, 314 (`:238-334`) — CONFIRMED |
| BR-M09-053 | `Aging.aspx` bucket headers: "1 to 10", "11 to 20", "21 to 30", "31 to 40", "41 to 50", "51+" days, each with TP Amount + Net Amount, plus Total TP / Total Net; bucket assignment logic is in `sp_Get_AgingReceivableReport` (not in repo) — bucket boundaries as *labels* only | 305 (`Aging.aspx.cs:54-94`, `Aging.aspx:248-280`) — CONFIRMED (labels) / UNKNOWN (calc) |
| BR-M09-054 | MoneyReceiptPreview receipt no = voucher code of the payment (accounts TableID 2017) else RefNo else `RP`+7-digit CustPayId (differs from report SP numbering BR-M09-018) | 331 (`MoneyReceiptPreview.aspx.cs:66-105`) — CONFIRMED |
| BR-M09-055 | MarketWiseCustomerPayment (legacy): Σ row payments must equal the typed Payment Amount; status Full if pay+previous+adjust == invoice amount else Partial via `UpdateInvoicePaymentAmount` (writes tblInvoice.PaymentAmount = pay+previous); single-detail `SaveCustPayment` returns true even when its `Existence` check skipped the insert; no discount/AIT/posting | 328 (`:216-327`, `CustPaymentBLL.cs:55-90`) — CONFIRMED |
| BR-M09-056 | Posting audit thresholds: expected voucher amount for a payment = Σ(PaymentAmount+Discount+AIT) per CustPayId; statuses MISSING_VOUCHER, MISSING_VOUCHER_DETAILS, DR_CR_UNBALANCED (strict), MASTER_VALUE_MISMATCH / DETAIL_VALUE_MISMATCH (> 1.00 BDT) | audit script (`SP_Audit_Sales_Accounts_Voucher_Posting.sql:148-219`) — CONFIRMED |
| BR-M09-057 | CustomerPaymentReport lists all payment headers by header PaymentDate for **all depots**; voucher no = accounts voucher code else RefNo else `CT`+8-digit id; "Bank/Cash" is always the literal "General Cash"; Amount = header PaymentAmount (excludes discount/AIT) | 316 (`CustomerPaymentReport.aspx.cs:174-241`) — CONFIRMED |
| BR-M09-058 | Full payment rollback (page `SalesOperationRollback`, outside M09 list): single transaction — insert reversal vouchers (Dr/Cr swapped, dated now) for TableID 2017 vouchers of the payment, reset invoice `PaymentStatus/PaymentAmount` to NULL, log via `sp_InsertCustPayDetail_DeleteLog`, delete detail+header; located by `tblCustomerPay.CollectionNo` (column origin UNKNOWN) | `SalesOperationRollbackDAL.cs:524-688` — CONFIRMED (code) / UNKNOWN (column) |

## 4. Status lifecycles
- **Invoice payment stage** (`tblInvoice.PaymentInvoiceStatus` / `PaymentInvoiceNo`, set by PAGE-334): none → Full | Partial | Reject (with `RTN-…` number). Line-level `tblInvoiceDetail.PaymentStatus` same domain.
- **Invoice collection status** (`tblInvoice.PaymentStatus`, `PaymentAmount`, written by UNKNOWN SP `sp_Update_InvoiceFinalPayment` from PAGE-311/314/315/318 with "Full"/"Partial"): NULL → Partial → Full; reset to NULL by PAGE-335. Collectable filters elsewhere use `PaymentStatus IS NULL OR 'Partial'`.
- **Payment rows** (`tblCustomerPay`, `tblCustPayDetail`): no status column; created final; removed by PAGE-335 (hard delete, first payment only) or PAGE-313 (SP delete with log).
- **Accounts voucher**: created `Approved` immediately (CSTL_ACCDB_Unicorn `ActionStatus`); never reversed by any code in this module.
- **Deposit slip** (`tblCompanyWiseDeposit`): active (`IsDelete=0/NULL`) → deleted (`IsDelete=1`, DeleteBy/DeleteDate) within 60 days by two named users.

## 5. Roles / permissions observed
Auth is Forms + session; the master page redirects to Login when `UserId` is missing. Everything is otherwise **menu-based**; explicit code checks found:
- `Session["RoleTypeName"]/["RoleTypeId"]/["UserType"]/["DesigName"]` → default "Collection By" (PAGE-311).
- `RoleTypeName == "DIC"` → deposit list requires depot and preselects it; deposit code by DIC (`DICCompanyUnitId`).
- `RoleTypeName` "DZSM"/"AM" → UI steering on PAGE-336 (client-side only).
- Deposit deletion restricted to two hard-coded LoginNames (`51419000000`, `50588`) — PAGE-322.
- `RoleTypeId` "2"/"3" checks in DeliveryPaymentReport / SC_PaymentReport (behaviour not analysed — UNKNOWN).
- No permission check at all on PAGE-313, PAGE-335, PAGE-314, PAGE-334 (payment reversal, deletion, bulk upload).
- Depot lists come from `tblUserCompanyUnit` (PAGE-311 etc.), but the aging report lists all depots.

## 6. Data model (tables & SPs that matter)
Sales DB `SalesDisDB_Unicorn_DB`: `tblCustomerPay` (CustPayId, MarketId, CustomerMasterId, ComUnitId, PaymentDate, PaymentAmount, PayType, RefNo, RefDate, Remarks[added by SP_Fix_CustomerPayment_Live.sql], CreateBy, CreateDate, …), `tblCustPayDetail` (CustPayDetailId, CustPayId, InvoiceId, SubDeportInvoiceId, PaymentAmount, TPAmount, VATAmount, AIT, Discount, CollectionBy, DANameId, custPaymentDate, BankAccId, CashAccId), `tblInvoice` (+Payment* columns, PaymentStatus, PaymentAmount, DeliveryCharge, DeliveryTpGrandTotal), `tblInvoiceDetail` (Delivery*/Payment* amount columns), `tblReturnInvoice`, `tblCompanyWiseDeposit`, `tblDCStore`, `tblOrder`, `tblCustMaster`, `tblSubInvoiceMaster`.
Accounts DB `CSTL_ACCDB_Unicorn` (cross-DB): `tblDebitCreditVoucherMaster/Detail`, `tblFinancialYear`, `tblChartOfAccounts`, `tblSubsidiaryInfo`, `tblReferenceInfo`, `tblBankAccount`, `tblBankInformation`, `tblBankAccountUnitdtl`, `tblCashAccount`, `tblMultipleCompanySameCashAccount`. ERP DB `CSTL_ERPDB_Unicorn`: `tblCompanyInfo`, `tbl_AccIntegrationTable`, `Table_Integration_AccountsConnectivity`.
SPs with bodies in repo: `sp_GET_PaymentInvSP`, `sp_GET_PaymentInvSPTPVATAmt`, `sp_GET_PaymentInvSPPaymentAmount`, `sp_Sales_CustomerPaymentPosting`, `sp_Report_CustomerInvoiceAgingOutstanding`, `sp_Get_MoneyReceiptReportAfterPaymentList`, `sp_Audit_Sales_Accounts_Voucher_Posting`. Missing (called): `sp_Update_InvoiceFinalPayment`, `sp_Check_Duplicate_InvoiceFinalPayment`, `sp_InsertCustPayDetail_DeleteLog`, `sp_Sales_DeliveryCOGSPosting`, `sp_Get_MoneyReceiptReportList`, `sp_CustomerLedger(New)`, `sp_Get_AgingReceivableReport`, `sp_RPT_MIS_RptMIOWiseReceiveableReport`, `sp_Rep_DepopsitSlip_BusinessSummary*`, `sp_Get_NewReceiveableList(Web)`, `sp_I_Diposit_New`, and the collection/delivery report SPs (see reports/).

## 7. Integrations
Cross-database accounts posting (PAGE-311, WF-M09-01); ERP DB integration mapping; SAP-related bank mapping and SAP territory→MIO code for deposits (`SAP_IntrigationPointDAL`); OLE DB Excel import (Jet/ACE); Crystal Reports viewers (money receipt, ledger, customer payment, batch-wise collection).

## 8. Reports
See [reports/](reports/) — RPT-M09-01 … RPT-M09-07.

## 9. Workflows
- [WF-M09-01 customer payment collection & accounts posting](workflows/WF-M09-01-customer-payment-collection.md)
- [WF-M09-02 payment-stage confirmation (partial/return)](workflows/WF-M09-02-payment-stage-confirmation.md)
- [WF-M09-03 bulk payment upload](workflows/WF-M09-03-bulk-payment-excel-upload.md)
- [WF-M09-04 payment reversal / deletion](workflows/WF-M09-04-payment-reversal-and-deletion.md)
- [WF-M09-05 deposit slip upload & list](workflows/WF-M09-05-deposit-slip-upload-and-list.md)
- [WF-M09-06 money receipt issue & print](workflows/WF-M09-06-money-receipt-printing.md)
- [WF-M09-07 accounts voucher posting audit](workflows/WF-M09-07-accounts-posting-audit.md)

## 10. Known gaps summary
See [GAPS-M09.md](GAPS-M09.md). Headlines: shared detail list bug and re-posting in `CustomerPayment`; no transaction and silent posting failure; several payment entry pages bypass accounts; SQL injection points (`GenerateParam`, `Parm`, Excel cells → dynamic SQL); reversal/deletion without accounting reversal or authorisation; aging buckets not implemented in the invoice aging report; many SP bodies missing (including invoice-status update and duplicate check).

## 11. What is still unverified
- Effect of `sp_Update_InvoiceFinalPayment` (which invoice columns change, whether `PaymentAmount` is cumulative) and `sp_Check_Duplicate_InvoiceFinalPayment` criteria.
- `sp_Sales_DeliveryCOGSPosting`, `sp_InsertCustPayDetail_DeleteLog`, second-return list SP, ledger/receivable/aging (bucketed) SPs, deposit SPs, Crystal `.rpt` internals (column/formula definitions are binary).
- Whether vouchers are reversed when collections are reversed/deleted (only C# was examined).
- Exact menu paths and role→page assignments (DB-driven).
- Tier-B pages: Crystal viewers, DeliveryPaymentReport family, DACollection, SC_* reports and MoneyReceipt/AfterPayment grids were only skimmed (parameter builders and SP names verified, filters not exhaustively read).
- `IVMarketSTForZone.ascx` behaviour (market-structure security).
- Whether `PrecompiledWeb/` reflects the deployed build (it contains copies of these pages).
