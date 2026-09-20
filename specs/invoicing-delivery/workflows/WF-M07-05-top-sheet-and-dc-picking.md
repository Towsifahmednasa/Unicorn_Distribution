# WF-M07-05 - DC picking slip and top sheet (outbound preparation)

**Actors:** DC store keeper / logistics user. **Precondition:** proforma invoices exist for the day (WF-M07-01/02).

## A. DC picking slip (`CreatePickingOnDC`, PAGE-260)
1. Select DC (Admin: all; others: defective scoping, BR-M07-042) and invoice date -> `searchButton_Click` -> `InvoiceBLL.InvoiceForDCPickingBLL` -> `InvoiceDAL.InvoiceForDCPickingDAL`: invoices of DC/date not in `tblDCPickingDetail` (BR-M07-043).
2. Choose picking date and area (areas loaded on DC change via concatenated SQL), tick invoices -> `createButton_Click` -> `Save()`: `DcPickingSaveBLL` (PK max+1, `DPK-<code>-<count+1 padded 8>`, INSERT tblDCPicking) then `DcPickingDetailSaveBLL` (INSERT tblDCPickingDetail per invoice) (BR-M07-044/045). No transaction/validation.
3. Print: `DCPickingPrintingList` -> `DCPickingReportViewer?pickNo=` -> `rptDcPickingSlip` totals of tblInvoiceDetail by product/batch (BR-M07-046).

## B. Top sheet (`TopSheetGenerate`, PAGE-277)
1. DC + date + DA -> Search (also runs anomaly auto-delete, WF-M07-03).
2. Tick invoices -> "View TopSheet Report": `Save_TopSheetGenReportCodeInfo` -> `sp_Save_TopSheetGenReportCode(@EntryBy,@DeliveryMan)` -> `sp_GET_ToSheetcode_ById` -> `TopSheetGenCode`; popup `TopSheetReportViewer?InvNo=<ids>&Code=<code>` (session keys `ProformaTopSheet=1`, `dcDropDownList1`, `Route=0`, `InvoiceDateTextBox`) -> `rptTopSheet.rpt` with detail (`InvoiceDetailDataForReportNew_`), market/DA-wise picking (`MarketPickinReportNew_`), main data (`InvoiceMainDataForReport`) (BR-M07-040/041).
3. "Print Invoice Report" -> `ProformaReportPrintViewer`.

## Data changed
tblDCPicking, tblDCPickingDetail; top-sheet code table (name UNKNOWN). No stock movement.

## Exceptions
Empty picking headers; codes generated without rows; id vs number mismatch in report queries (GAP-M07-020, 023, 019). Variants: `DelivaryTopSheetGenerate` (PAGE-265, delivery invoices), `SCPickingGenerate` (PAGE-276) - Tier B, not verified.

**Related BR:** 034-036, 040-046. **Tests:** TC-260-*, TC-277-09..12.
