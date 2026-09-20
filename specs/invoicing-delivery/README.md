# M07 - Proforma, Invoicing & Delivery (`specs/invoicing-delivery`)

Reviewed 2026-09-19. Evidence: `.aspx/.aspx.cs`, BLL/DAL methods reached, and the SP bodies present in the repo (`sp_Delete_ProformaInvoice_Update.sql`, `SP_Fix_Invoice_Anomaly_And_Sync.sql`, `SP_Fix_CentralStoreStoIssue*.sql`, `SP_Adjust_DCStore_Stock_Only.sql`, `sp_Check_DCStore_StockAnomalies.sql`). All other SP bodies are absent (see `GAPS-M07.md`). Confidence tags: CONFIRMED / INFERRED / UNKNOWN.

## 1. Purpose and business overview
Covers everything between a customer order and goods delivered at a Distribution Center (DC): proforma invoice creation (bulk/manual; the live by-order creation pages belong to **M06** `../order-management/`), invoice printing, DC picking slips, top sheets per Delivery Assistant (DA), loading summary / sales confirmation (delivery confirmation + accounting posting), payment-confirmation worklist, requisition based warehouse picking and issue challans (central warehouse -> DC), challan reports, and old-data delivery confirmation by Excel. Partial payment (`PaymentPartial`) is **M09** `../collection-payment/pages/PAGE-334-paymentpartial.md`; sales return is a separate module; `LogisticeRollBackOperation` is documented under `../sales-reports-dashboard/pages/PAGE-514-logisticerollbackoperation.md`.

Two distinct "challan" meanings exist: (a) **requisition/STO issue challan** `CLN-...` (central store -> DC, `tblRequisition`, `tblStockInTransfar`) handled by PAGE-259/256/257/258/261; (b) customer **delivery invoice** ("DelivaryInvoiceNo") created at delivery confirmation (PAGE-271/264/266).

## 2. Page catalogue (43 pages; spec = `pages/PAGE-nnn-*.md`)
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-236 | DAList | List (DA master, `sp_GET_DAInfo`) | B | [pages/PAGE-236-dalist.md](pages/PAGE-236-dalist.md) |
| PAGE-237 | DASetup | Entry (DA master: `sp_Save_DAInfo`/`sp_Update_DAInfo`) | B | [pages/PAGE-237-dasetup.md](pages/PAGE-237-dasetup.md) |
| PAGE-238 | ChallanReportViewer | Report viewer (`crpChallan.rpt`) | B | [pages/PAGE-238-challanreportviewer.md](pages/PAGE-238-challanreportviewer.md) |
| PAGE-239 | DCPickingReportViewer | Report viewer (`rptDcPickingSlip`) | B | [pages/PAGE-239-dcpickingreportviewer.md](pages/PAGE-239-dcpickingreportviewer.md) |
| PAGE-240 | DeleteInvoiceReportViewer | Report viewer (`crpDeleteInvoiceReport.rpt`) | B | [pages/PAGE-240-deleteinvoicereportviewer.md](pages/PAGE-240-deleteinvoicereportviewer.md) |
| PAGE-241 | DelivaryInvoiceReturnViewer | Report viewer (`rptInvoiceForCustomerDelivary.rpt`) | B | [pages/PAGE-241-delivaryinvoicereturnviewer.md](pages/PAGE-241-delivaryinvoicereturnviewer.md) |
| PAGE-242 | DelivaryTopSheetInvoiceReturnViewer | Report viewer (`rptDelivaryTopSheet.rpt`) | B | [pages/PAGE-242-delivarytopsheetinvoicereturnviewer.md](pages/PAGE-242-delivarytopsheetinvoicereturnviewer.md) |
| PAGE-243 | InvoicePrintingList (RPTVIEW copy) | Report viewer | B | [pages/PAGE-243-invoiceprintinglist.md](pages/PAGE-243-invoiceprintinglist.md) |
| PAGE-244 | InvoiceReportViewer | Report viewer (`rptInvoiceForCustomer_Uncr.rpt`) | B | [pages/PAGE-244-invoicereportviewer.md](pages/PAGE-244-invoicereportviewer.md) |
| PAGE-245 | InvoiceReturnViewer | Report viewer (return invoice) | B | [pages/PAGE-245-invoicereturnviewer.md](pages/PAGE-245-invoicereturnviewer.md) |
| PAGE-246 | InvoiceWiseDetailsSalesReport | Report viewer (`rptProformaReport.rpt`) | B | [pages/PAGE-246-invoicewisedetailssalesreport.md](pages/PAGE-246-invoicewisedetailssalesreport.md) |
| PAGE-247 | LoadingReportViewer | Report viewer (`rptLoadingReport.rpt`) | B | [pages/PAGE-247-loadingreportviewer.md](pages/PAGE-247-loadingreportviewer.md) |
| PAGE-248 | MarketWisePicking | Report viewer (`rptMarketwisePicking.rpt`) | B | [pages/PAGE-248-marketwisepicking.md](pages/PAGE-248-marketwisepicking.md) |
| PAGE-249 | ProformaAndSalesReportViewer | Report viewer (`ProformaandsalesReport.rpt`, `crpAllSalesReport.rpt`) | B | [pages/PAGE-249-proformaandsalesreportviewer.md](pages/PAGE-249-proformaandsalesreportviewer.md) |
| PAGE-250 | ProformaReportPrintViewer | Report viewer (`rptInvoiceForCustomer(_Uncr).rpt`) | B | [pages/PAGE-250-proformareportprintviewer.md](pages/PAGE-250-proformareportprintviewer.md) |
| PAGE-251 | ProformaReportViewer | Report viewer (`rptProformaReport.rpt`) | B | [pages/PAGE-251-proformareportviewer.md](pages/PAGE-251-proformareportviewer.md) |
| PAGE-252 | ReturnInvoiceReportViewer | Report viewer (`rptReturnInvoiceForCustomer.rpt`) | B | [pages/PAGE-252-returninvoicereportviewer.md](pages/PAGE-252-returninvoicereportviewer.md) |
| PAGE-253 | ReturnInvoiceReportViewer2 | Report viewer | B | [pages/PAGE-253-returninvoicereportviewer2.md](pages/PAGE-253-returninvoicereportviewer2.md) |
| PAGE-254 | TopSheetReportViewer | Report viewer (`rptTopSheet.rpt`) | B | [pages/PAGE-254-topsheetreportviewer.md](pages/PAGE-254-topsheetreportviewer.md) |
| PAGE-255 | WarehousePickingReportViewer | Report viewer (`rptPickingForWarehouse.rpt`) | B | [pages/PAGE-255-warehousepickingreportviewer.md](pages/PAGE-255-warehousepickingreportviewer.md) |
| PAGE-256 | ChallanReportList | List / report (requisition challans) | B | [pages/PAGE-256-challanreportlist.md](pages/PAGE-256-challanreportlist.md) |
| PAGE-257 | ChallanReportView | List + edit launcher + Excel (requisition challans) | A | [pages/PAGE-257-challanreportview.md](pages/PAGE-257-challanreportview.md) |
| PAGE-258 | ChallanVIewEditNew | Edit form (challan date/truck/driver) | B (reviewed in PAGE-257 and WF-M07-04) | [pages/PAGE-258-challanvieweditnew.md](pages/PAGE-258-challanvieweditnew.md) |
| PAGE-259 | CreatePickingDetailForWareHouse | Entry / transaction (STO issue + challan) | A | [pages/PAGE-259-createpickingdetailforwarehouse.md](pages/PAGE-259-createpickingdetailforwarehouse.md) |
| PAGE-260 | CreatePickingOnDC | Entry / transaction (DC picking slip) | A | [pages/PAGE-260-createpickingondc.md](pages/PAGE-260-createpickingondc.md) |
| PAGE-261 | CreatePickingOnWareHouse | List of un-picked requisitions -> PAGE-259 | B | [pages/PAGE-261-createpickingonwarehouse.md](pages/PAGE-261-createpickingonwarehouse.md) |
| PAGE-262 | DCPickingPrintingList | List / print (DC picking slips) | B | [pages/PAGE-262-dcpickingprintinglist.md](pages/PAGE-262-dcpickingprintinglist.md) |
| PAGE-263 | DeleteInvoiceReport | Report filter (deleted invoices) | B | [pages/PAGE-263-deleteinvoicereport.md](pages/PAGE-263-deleteinvoicereport.md) |
| PAGE-264 | DelivaryInvoiceCreationAfterSalesConfirm | Entry / transaction (payment-confirmation worklist) | A | [pages/PAGE-264-delivaryinvoicecreationaftersalesconfirm.md](pages/PAGE-264-delivaryinvoicecreationaftersalesconfirm.md) |
| PAGE-265 | DelivaryTopSheetGenerate | Entry (delivery top sheet) | B | [pages/PAGE-265-delivarytopsheetgenerate.md](pages/PAGE-265-delivarytopsheetgenerate.md) |
| PAGE-266 | DeliveryExcelUploadOldData | Upload / import (old-data delivery confirmation) | A | [pages/PAGE-266-deliveryexceluploadolddata.md](pages/PAGE-266-deliveryexceluploadolddata.md) |
| PAGE-267 | InvoiceCreationForCustomer | Entry / transaction (manual invoice, legacy) | A | [pages/PAGE-267-invoicecreationforcustomer.md](pages/PAGE-267-invoicecreationforcustomer.md) |
| PAGE-268 | InvoicePrintingList | List / print (invoices by DC/date) | B | [pages/PAGE-268-invoiceprintinglist.md](pages/PAGE-268-invoiceprintinglist.md) |
| PAGE-269 | InvoiceSummaryReport | Report filter (`sp_Get_InvoiceSummaryReport`) | B | [pages/PAGE-269-invoicesummaryreport.md](pages/PAGE-269-invoicesummaryreport.md) |
| PAGE-270 | LoadingReport | Report filter (loading report) | B | [pages/PAGE-270-loadingreport.md](pages/PAGE-270-loadingreport.md) |
| PAGE-271 | LoadingSummary | Entry / transaction (sales confirm / loading summary) | A | [pages/PAGE-271-loadingsummary.md](pages/PAGE-271-loadingsummary.md) |
| PAGE-272 | ProformaInvoiceCreation | Entry (bulk proforma via SP; grid dead) | A | [pages/PAGE-272-proformainvoicecreation.md](pages/PAGE-272-proformainvoicecreation.md) |
| PAGE-273 | ProformaPrintList | List / print (proforma invoices) | B | [pages/PAGE-273-proformaprintlist.md](pages/PAGE-273-proformaprintlist.md) |
| PAGE-274 | ProformaReport | Report filter (`sp_Get_ProformaInvoiceReportList`) | B | [pages/PAGE-274-proformareport.md](pages/PAGE-274-proformareport.md) |
| PAGE-275 | RptBussinessSummary_Loading | Report (`sp_RPT_MIS_BusinessSummary`; zone/area/territory grids + Excel) | B | [pages/PAGE-275-rptbussinesssummary-loading.md](pages/PAGE-275-rptbussinesssummary-loading.md) |
| PAGE-276 | SCPickingGenerate | Entry (SC picking / top-sheet variant) | B | [pages/PAGE-276-scpickinggenerate.md](pages/PAGE-276-scpickinggenerate.md) |
| PAGE-277 | TopSheetGenerate | Entry / transaction (top sheet per DA; Search auto-deletes anomalous proformas) | A | [pages/PAGE-277-topsheetgenerate.md](pages/PAGE-277-topsheetgenerate.md) |
| PAGE-278 | WarehousePickingReportList | List / print (requisition picking) | B | [pages/PAGE-278-warehousepickingreportlist.md](pages/PAGE-278-warehousepickingreportlist.md) |

Tier-A count: 9 (candidate list had 6; RptBussinessSummary_Loading was swapped for the challan/picking transaction pages CreatePickingDetailForWareHouse and ChallanReportView, plus ProformaInvoiceCreation and CreatePickingOnDC). Tier-B specs remain mechanically generated; their SP/table lists for `sp_Delete_ProformaInvoice` / `sp_Check_anomalyInvoiceDetails` saying "body NOT in repository" are outdated - bodies are in the root `.sql` scripts.

## 3. Business rules index (BR-M07-nnn, all sequential)
| BR | One-liner | Page |
|---|---|---|
| 001-005 | Proforma bulk creation calls `sp_AutoInvoiceGeneration` per ticked order; order grid never populated; unconditional success; SC/market scoping; defaults | PAGE-272 |
| 006-010 | Manual invoice header from order code, dup-product rule, product info, line calc (TP, VAT, discount, net), header sums | PAGE-267 |
| 011 | Invoice no `INV-<ComUnitCode>` + global MAX(InvoiceId)+1 padded 10 | PAGE-267 |
| 012 | tblInvoiceBatch batch number (route prefix) | PAGE-267 |
| 013-014 | FEFO stock allocation on tblDCStore Receive rows, Issue rows negative, bonus handling, per-batch detail split | PAGE-267 |
| 015-017 | Gift zero-pricing; persisted detail formulas; header recomputed from details (+DeliveryCharge, -AdjustAmount) | PAGE-267 |
| 018-019 | No transaction; manual page fails at Isgift (no OrderDetailsId) leaving orphan header | PAGE-267 |
| 020-025 | Payment-confirmation worklist (`PaymentInvoiceNo IS NULL`), single-item status dropdown, Go-to-Partial, dead ALL Submit, total label, dead redirect | PAGE-264 |
| 026-033 | Loading summary: filters, status lists, common status, Sales Confirm 3-call chain, Cash/Rejection, Partial redirect, success flag, mid-loop rebind | PAGE-271 |
| 034-036 | Top sheet search per DA; Search auto-deletes anomalous proformas; anomaly criterion (>10, after 2022-11-16) | PAGE-277 |
| 037-039 | `sp_Delete_ProformaInvoice`: only proforma, stock reversal (delete Issue row with exact qty), bookkeeping + order reset | PAGE-277 |
| 040-041 | Top-sheet code per click; report parameters ids vs numbers | PAGE-277 |
| 042-046 | DC picking: DC scoping bug, search excludes picked, create without validation, DPK numbering, slip totals | PAGE-260 |
| 047-053 | Warehouse picking (issue) rules | PAGE-259 |
| 054-057 | Challan list/edit/export/print rules | PAGE-257 |
| 058-061 | Excel old-data upload and confirmation rules | PAGE-266 |

## 4. Status lifecycles
**Order/invoice (proforma -> delivery):**
1. Order created (M06) `tblOrder.IsInvoice = 0`.
2. Invoice creation (M06 by-order pages, or `sp_AutoInvoiceGeneration`): `tblInvoice` row with `DelivaryInvoiceNo IS NULL` (proforma), `tblInvoiceDetail` batch rows, `tblDCStore` Issue rows (stock reserved), `tblOrder.IsInvoice = 1` (INFERRED from the reset in `sp_Delete_ProformaInvoice`).
3. Proforma deletion (PAGE-277, M06, `ProformaorInvoiceReturn`): row deleted, Issue rows removed, `IsInvoice = 0`, `tblOrderDetail.Status = NULL`, logged.
4. Loading summary (PAGE-271): `LoadingSummaryStatus` = `Full Dues` | `Cash` | `Rejection` (`sp_UP_LoadingSummary`); `Partial Dues` -> partial screen. "Full Dues" also runs `sp_DeliveryConformationFull_New` (delivery invoice: `DelivaryInvoiceNo`, `DeliveryInvoiceStatus` 'Full'/'Partial', `DeliveryTpGrandTotal` - INFERRED) and `sp_Sales_DeliverySalesPosting` (voucher TableID 2014).
5. Payment confirmation (PAGE-264 -> PaymentPartial M09): `PaymentInvoiceNo` (NULL until confirmed; `RTN-`+delivery no on partial), `PaymentInvoiceStatus` ('Reject'), `PaymentStatus` ('Partial').
**DC picking:** presence in `tblDCPickingDetail` = picked (no status column).
**Requisition/STO challan:** `tblRequisition.CreatePicking` NULL/'' -> 'OK' and `Submit` = 'OK' at PAGE-259 save; `tblRequsitionChild.IsPicking/IsIssue` = 'OK'; `tblStockInTransfar.IsIssue` = 'OK'; later DC receipt (other module) moves stock into `tblDCStore` Receive rows.
Details per page in §21 of each Tier-A spec.

## 5. Roles / permissions observed
No page in M07 checks a role. Only `CreatePickingOnDC`, `InvoicePrintingList`, `DCPickingPrintingList` read `Session["UserType"]=="Admin"` (all DCs) and misuse `Session["ComUnitId"]` as a user id for others. DC scoping elsewhere relies on `tblUserCompanyUnit` via `OrderInfoBLL.LoadSC(ddl, UserId)` / `OtherStockActionBLL.DCLoad(ddl, UserId)`. `LoginName` is stamped into SP `@UpdateBy`/`@User`. Menu access is DB-driven (M01).

## 6. Data model (tables + SPs that matter)
Tables: `tblOrder`, `tblOrderDetail`, `tblInvoice` (header incl. TpTotal/TpDiscount/TpVat/TpGrandTotal/ReceivableAmount/AdjustAmount/DeliveryCharge/Inv_DANameId/DelivaryInvoiceNo/LoadingSummaryStatus/PaymentInvoiceNo), `tblInvoiceDetail`, `tblInvoiceBatch`, `tblDCStore` (Trans_Type Receive/Issue, ReferenceDCStoreId, StockQty), `tblDCPicking`, `tblDCPickingDetail`, `tblInvoiceDeleteLog`, `tblInvoiceDetail_DeleterRecord`, `tblProInvoiceReturnTrack`, `tblReturnAmount`, `tblRequisition`, `tblRequsitionChild`, `tblStockInTransfar`, `tblCentralStore`, `tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock`, `tblDAInfo`, views `View_DCStoreCurrentStock`, `View_OrderCustomerInfo`, `View_CentralStoreCurrentStock` (bodies absent).
SP bodies in repo: `sp_Delete_ProformaInvoice`, `sp_Check_anomalyInvoiceDetails`, `tbl_INV_CentralStoreStoIssue` (two script versions), `sp_Adjust_DCStore_Stock_Only`, `sp_Check_DCStore_StockAnomalies`. Absent: `sp_AutoInvoiceGeneration`, `sp_DeliveryConformationFull_New/_OldData`, `sp_UP_LoadingSummary`, `sp_Sales_DeliverySalesPosting`, `sp_LoadingSummary`, `sp_DeliveryInvoiceCreationList_New`, `sp_PaymentConformationFull`, `sp_Save_TopSheetGenReportCode`, etc.

## 7. Integrations
Accounting DB `CSTL_ACCDB_Unicorn` (delivery posting vouchers TableID 2014; invoice-return vouchers TableID 2018; read in invoice/top-sheet queries for dues and credit days). External host `13.76.141.111:58122` "Update Old Data" link (PAGE-264/271). Central weighted-average ledger (`tbl_INV_CentralStoreStoIssue`). No SAP call in M07 pages reviewed. Crystal Reports viewers.

## 8. Reports
See [reports/README.md](reports/README.md) and RPT-M07-01 (invoice/proforma prints and reports), RPT-M07-02 (top sheet, picking, loading, deleted invoice, challan).

## 9. Workflows
- [WF-M07-01 proforma to delivery lifecycle](workflows/WF-M07-01-proforma-to-delivery-lifecycle.md)
- [WF-M07-02 manual invoice and stock allocation](workflows/WF-M07-02-manual-invoice-and-stock-allocation.md)
- [WF-M07-03 proforma delete and anomaly auto-clean](workflows/WF-M07-03-proforma-delete-and-anomaly-clean.md)
- [WF-M07-04 requisition picking, issue and challan](workflows/WF-M07-04-requisition-picking-issue-challan.md)
- [WF-M07-05 top sheet and DC picking](workflows/WF-M07-05-top-sheet-and-dc-picking.md)

## 10. Known gaps summary
Full table in [GAPS-M07.md](GAPS-M07.md). Headlines: search-triggered auto-deletion of invoices; no transactions around multi-step stock/invoice writes; dead/legacy pages (ProformaInvoiceCreation grid, InvoiceCreationForCustomer save, AfterSalesConfirm submit); SQL fragments concatenated into SPs; hard-coded SMTP and external-host details; unauthenticated-by-role delivery confirmation; most SP bodies absent.

## 11. Still unverified
Bodies of `sp_AutoInvoiceGeneration`, delivery confirmation/posting SPs, `sp_LoadingSummary`; live vs script versions of the five SPs read; view definitions; `.rpt` internals (fields/formulas); Tier-B pages not opened (DA setup/list, print lists, DelivaryTopSheetGenerate, SCPickingGenerate, RptBussinessSummary_Loading, reports filters); runtime behaviour (nothing executed).
