# RPT-M06-01 — Proforma / Order-invoice print (ProformaReportPrintViewer)

| Item | Value |
|---|---|
| Launch pages | PAGE-217 `InvoiceCreationByOrder.aspx` "Report" button (`viewRptButton_Click`, `.cs:141-155`) — batch print; PAGE-218 `InvoiceCreationForCustomerByOrder.aspx` "Print" (`printButton_Click`, `.cs:3117-3163`) — single invoice |
| Viewer | `Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx(.cs)` (242 lines), `Page_Init` |
| .rpt | `Solution.Web/Reports/CrystalReports/rptInvoiceForCustomer.rpt` (batch path via Session) and `rptInvoiceForCustomer_Uncr.rpt` (single path via `?InvNo=`); loaded with `ReportPath(...)` = `~\Reports\CrystalReports\` |
| Dataset tables | `mainDataTable`, `detailDataTable`, `dtReceiveable`, `companyInfoDataTable` (`.cs:69-76,140-147`) |
| Output | viewer bound (`crvInvoiceReport`, `SeparatePages=false`) **and** immediate PDF export to response (`ExportToHttpResponse(PortableDocFormat, …, "ProformaInvoice-")`, `.cs:94, 165`) |

## Request parameters
- Batch mode: `Session["paydetailId"]` = a SQL fragment built by `InvoiceCreationByOrder.GenerateParameter()`: `WHERE IV.InvoiceNo IN (SELECT InvoiceNo FROM dbo.tblInvoiceBatch LEFT JOIN dbo.tblInvoice ON tblInvoice.InvoiceId = tblInvoiceBatch.InvoiceId WHERE BatchNo = '<batchno textbox>')` (`InvoiceCreationByOrder.aspx.cs:114-116`). The **user-typed batch number is concatenated into SQL** and the whole WHERE fragment is stored in Session and re-injected by the viewer (`InvoiceDAL.InvoiceMainDataForReport2(invNo)` / `InvoiceDetailDataForReport2` place it into the query).
- Single mode: `Request.QueryString["InvNo"]` (URL-encoded proforma number) → `InvoiceMainDatainvNo`, `InvoiceDetailDataInvoID` (parametrised or concatenated — `InvoiceDAL.cs:1461, 1639`; concatenation of `invNo` seen in the commented legacy variants, current variants not fully read).
- Note `if (invNo != null)` (`.cs:109`): when the batch branch ran, the single branch also runs if `InvNo` absent → `Server.UrlDecode(null)` returns null so it is skipped; when both are supplied both reports are streamed.

## Data sources
| Table | SQL | Notes |
|---|---|---|
| mainDataTable | `InvoiceDAL.InvoiceMainDataForReport2` (`:1425`) / `InvoiceMainDatainvNo` (`:1461`) | tblInvoice ⟕ tblOrder ⟕ customer/MIA/DA data. Column aliases are **repurposed**: `ReturnInvoiceId` = order sender/MIO name (COALESCE of `tblOrder.OrderSenderName`, `tblOrder.MIOName`, `IV.OrderSenderName`, `IV.MIAName`, `MIA.EmpName`); `TpTotal` = `IV.CustomerType`; `TpDiscount` = `TpDiscount + TotalSpecialAmount`; `TpGrandTotal` (single mode) = `ΣNetAmount + DeliveryCharge` |
| detailDataTable | `InvoiceDetailDataForReport2` (`:1617`) / `InvoiceDetailDataInvoID` (`:1639`) | tblInvoiceDetail ⟕ tblDCStore(BatchNo, ExpDate) ⟕ tblProductDiscount; `BonusQuantity` column carries **PackSize**; `DiscountPercentage` = line % + customer/product discount %; `DiscountAmount` = discount + SpecialAmount |
| dtReceiveable | `InvoiceDAL.GetNewReceiveableforInvoiceDAl(" AND mas.[CustomerMasterId] in (<ids>)", null, null)` → `sp_Get_NewReceiveableList`-family SP (body UNKNOWN) | customer previous dues block |
| companyInfoDataTable | `StockTransportOrderReportBLL.CompanyInfoBLL()` | header |

## Filters / access
No permission check on the viewer (only whatever the caller stored in Session). Anyone who can reach the URL with `?InvNo=` can print any proforma (IDOR-style). Errors swallowed by a try/catch in `Page_Init`.

## Business logic in report path
`PAGE-217.GenerateParameter()` and `PAGE-218.printButton_Click` **delete** proformas whose header and detail totals differ by >10 before printing (BR-M06-027, BR-M06-048) — printing is not read-only.

## Gaps
GAP-M06-023 (SQL fragment in Session/injection), GAP-M06-024 (print may delete), GAP-M06-025 (repurposed columns/.rpt field mapping unverified — `.rpt` binary not inspected).
