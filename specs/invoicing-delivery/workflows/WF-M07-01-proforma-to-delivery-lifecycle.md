# WF-M07-01 - Proforma -> confirmation -> delivery (customer invoice lifecycle)

**Trigger / actors:** DC operator creates invoices (M06 by-order pages or bulk proforma); DC logistics user prepares loading paperwork and confirms delivery; accounts/collection user confirms payment (M09).

**Preconditions:** order exists (`tblOrder.IsInvoice=0`, M06); DC stock exists as `tblDCStore` Receive rows; user mapped to the DC in `tblUserCompanyUnit`.

## Steps
| # | Step | Page -> handler -> BLL -> DAL -> SP/SQL | Data / status effect | Confidence |
|---|---|---|---|---|
| 1 | Create proforma invoice | M06 `InvoiceCreationForCustomerByOrder` / `InvoiceCreationByOrder` (`../../order-management/pages/PAGE-217-invoicecreationbyorder.md`, `PAGE-218-...`); or bulk `ProformaInvoiceCreation.btnSubmit0_Click` -> `InvoiceBLL.SaveProformaInvoice` -> `InvoiceDAL.SaveProformaInvoice` -> `sp_AutoInvoiceGeneration` (PAGE-272, dead grid) | `tblInvoice` (DelivaryInvoiceNo NULL), `tblInvoiceBatch`, `tblInvoiceDetail` per batch, `tblDCStore` Issue rows, `tblOrder.IsInvoice=1` (INFERRED); rules BR-M07-011..017 | CONFIRMED for InvoiceBLL path; UNKNOWN for the SP |
| 2 | (optional) Delete / auto-clean | see WF-M07-03 | proforma removed, stock returned | CONFIRMED |
| 3 | Print invoice | `InvoicePrintingList` / `ProformaPrintList` / `TopSheetGenerate` -> `InvoiceReportViewer` -> `rptInvoiceForCustomer_Uncr.rpt` | none | CONFIRMED |
| 4 | DC picking and top sheet | WF-M07-05 | picking rows, top-sheet code | CONFIRMED |
| 5 | Loading summary / sales confirm | `LoadingSummary.btnFinalSubmit_Click` (PAGE-271): `InvoiceBLL.SaveFullInvoice` -> `sp_DeliveryConformationFull_New`; `UP_LoadingSummaryInvoice` -> `sp_UP_LoadingSummary`; `PostDeliveryConfFullJournal` -> `sp_Sales_DeliverySalesPosting` | `LoadingSummaryStatus`='Full Dues'; delivery invoice fields (INFERRED); vouchers TableID 2014 (INFERRED); BR-M07-029 | CONFIRMED order / UNKNOWN effects |
| 5b | Cash / Rejection | same page, only `sp_UP_LoadingSummary` (BR-M07-030) | status only | CONFIRMED |
| 5c | Partial | "Go to Partial" -> `DelivaryInvoiceCreationForCustomerByOrderAuto_Loading.aspx` (M06 PAGE-215) | quantities/dues edited there | CONFIRMED redirect |
| 6 | Payment confirmation worklist | `DelivaryInvoiceCreationAfterSalesConfirm` (PAGE-264): lists `PaymentInvoiceNo IS NULL`; only "Go to Partial" works -> `PaymentPartial.aspx` (M09 PAGE-334); Full path dead (BR-M07-023) | `PaymentInvoiceNo/PaymentInvoiceStatus/PaymentStatus` set by M09 | CONFIRMED |
| 7 | Old-data catch-up | `DeliveryExcelUploadOldData` (see PAGE-266) -> `sp_DeliveryConformationFull_OldData` | delivery confirmation without journal | CONFIRMED call |

## Branches / exceptions
- Any SP failure mid-chain leaves partial state (no transactions): BR-M07-018, 029, 033. Journal failure invisible (BR-M07-029).
- Delivery-confirmed invoices cannot be deleted by `sp_Delete_ProformaInvoice` (DelivaryInvoiceNo not null): BR-M07-037.

## Postconditions
Invoice confirmed (delivery), stock stays as Issue rows in `tblDCStore`, revenue/COGS vouchers in accounting DB (INFERRED), invoice appears in payment worklist.

**Related BR:** 001-005, 011-019, 020-033, 037. **Related pages:** PAGE-272, 267, 271, 264, 266, 277, 244, 268. **Tests:** TC-271-07/08/09/10, TC-264-06, TC-272-*.
