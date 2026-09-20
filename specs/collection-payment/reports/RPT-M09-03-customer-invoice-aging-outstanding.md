# RPT-M09-03 — Customer Invoice Aging & Outstanding (invoice-wise)

Page: `Solution.Web/SInventory_UI/CustomerInvoiceAgingReport.aspx(.cs)` (PAGE-306, Tier A — see its spec for full detail). No Crystal file; GridView + HTML `.xls` export.

| Item | Value |
|---|---|
| Data source | `sp_Report_CustomerInvoiceAgingOutstanding` — body **in repo**: `Live_Deployment_Script_All_Updates.sql:129-205` |
| BLL/DAL | `CustomerInvoiceAgingBLL` → `CustomerInvoiceAgingDAL.GetCustomerInvoiceAging` (`Library.DAL/SInventory_DAL/CustomerInvoiceAgingDAL.cs:14-28`) |
| Params | `@AsOnDate` (required by page), `@FromDate`, `@ToDate` (invoice-date window), `@ComUnitId`, `@CustomerMasterId`, `@SalesPersonId`, `@OnlyDueInvoices` (always false; ignored by SP) |
| Output columns | SL, Customer Name, Invoice No, Invoice Date (`dd Mon yyyy`), Invoice Amount, Paid Amount, Credit Days, Outstanding, Responsible Person + grid footer totals |
| Export | `Customer_Invoice_Aging_Report_<yyyyMMdd_HHmmss>.xls` (HTML table with title + "As On Date") |

## Calculations exactly as coded
- Invoice Amount = Σ `tblInvoiceDetail.DeliveryNetAmount` + `tblInvoice.DeliveryCharge`.
- Paid Amount = Σ (`PaymentAmount` + `Discount` + `AIT`) of `tblCustPayDetail` where `CAST(ISNULL(custPaymentDate, header.PaymentDate) AS DATE) <= @AsOnDate`.
- Outstanding = Invoice Amount − Paid Amount; only rows with Outstanding > 0.00 are returned.
- **Credit Days = `DATEDIFF(DAY, CAST(InvoiceDate AS DATE), @AsOnDate)`**.
- **Bucket boundaries: none.** The report neither buckets nor sorts by age; ordering is `InvoiceDate ASC, InvoiceNo ASC`. (For bucket labels see RPT-M09-04.)
- Invoices with `InvoiceDate > @AsOnDate` excluded; customer inner join required.

## Reconciliation warning
Collections list (PAGE-311) uses `PaymentNetAmount` (payment-stage) whereas this report uses `DeliveryNetAmount` (delivery-stage) and ignores `tblReturnInvoice` and payment-stage returns → figures can differ (BR-M09-048, GAP-M09-002).
