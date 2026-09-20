# RPT-M09-01 — Money Receipt (PDF) and receipt search grids

| Item | Value |
|---|---|
| List/search pages | `Solution.Web/SInventory_UI/MoneyReceiptAfterPayment.aspx(.cs)` (PAGE-330), `MoneyReceipt.aspx(.cs)` (PAGE-329) |
| Viewer pages | `Solution.Web/SInventory_RPTVIEW/MoneyReceiptViewerForAfterPayment.aspx(.cs)` (PAGE-304), `MoneyReceiptViewer2.aspx(.cs)` (PAGE-303); HTML preview `SInventory_UI/MoneyReceiptPreview.aspx(.cs)` (PAGE-331) |
| Crystal file | `Solution.Web/Reports/CrystalReports/rptMoneyReceipt.rpt` (both viewers); binary — field list UNKNOWN |
| Datasets | `mainDataTable` (SP result) + `companyInfoDataTable` (`StockTransportOrderReportBLL.CompanyInfoBLL()`) |
| Feeding SP | After payment: `sp_Get_MoneyReceiptReportAfterPaymentList(@Parm)` — body in `SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql`. Older list: `sp_Get_MoneyReceiptReportList(@Parm)` — body NOT in repo. Preview: inline SQL (no SP). |
| Request params | Viewer: `Session["InvoiceIds"]` (SQL fragment beginning with `AND …`, built by the caller). Preview: QueryString `ReceiptNo`, `CustPayId`, `InvoiceId`, `CopyType`. |

## Filters on the list page (MoneyReceiptAfterPayment)
Depot (`INV.ComUnitId`), Route (`ord.DistributionRouteId`), payment date range (`CONVERT(date, custPay.custPaymentDate) BETWEEN from AND to`; blank To → to "now"), market-structure pickers (`ord.GroupId`, `ord.RegionId`, `ord.AreaId`, `ord.TerritoryId`, `ord.SubTerritoryId`, `ord.MarketId`). All appended as string concatenation to `@Parm` (`Parm()` :123-183) — injection-prone (GAP-M09-007).

## Output columns (after-payment SP)
DISTINCT: CustPayDetailId, CustPayId, InvoiceId, OrderNo, CustomerCode, CustomerName, InvoiceNo, InvoiceDate (dd/mm/yyyy), DelivaryInvoiceNo, DelivaryInvoiceDate (= `ISNULL(INV.UpdateDate, InvoiceDate)`), PaymentNo, PaymentDate (custPaymentDate), PaymentAmount, TPAmount, VatAmount, ReceiptNo, ComUnitName ("Code : Name"), Address, OrderBy (order sender). Ordered `CustPayDetailId DESC`.
- **ReceiptNo / PaymentNo:** `masPay.RefNo` if non-empty else `MR-`+8-digit padded CustPayId (ReceiptNo) / plain CustPayId (PaymentNo) (BR-M09-018).
- Amount shown = `tblCustPayDetail.PaymentAmount` (excludes discount and AIT).

## Output / export
- Viewer exports **PDF directly** via `ExportToHttpResponse(PortableDocFormat, Response, true, "Money Receipt No-<ReceiptNo>")` and also binds the viewer; if no rows: script alert "No Money Receipt Data Found!".
- Search page: grid with "Total Payment Amount" (sum of PaymentAmount) and Excel export (`btnExport_Click`, ClosedXML/EPPlus usings).
- Preview page: HTML rendering with copy-type label; receipt number = accounts voucher code → RefNo → `RP`+7 digits (BR-M09-054); uses its own `SqlConnection` from configuration (bypasses DAL).

## Notes
Money receipt is per payment *detail*; several invoices paid in one cheque produce several rows with the same ReceiptNo. `Session["InvoiceIds"]` is a shared session slot — concurrent printing in two tabs can overwrite each other.
