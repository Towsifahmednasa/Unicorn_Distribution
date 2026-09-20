# RPT-M08-04 — Delivery invoice return print (second-time return)

| Item | Detail |
|---|---|
| Launched from | `SndTimesSalesReturnPartial.aspx` (PAGE-298 `printButton_Click`, key `invTextBox` = "RTN-"+InvoiceNo) |
| Viewer | `SInventory_RPTVIEW/DelivaryInvoiceReturnViewer.aspx(.cs)` |
| .rpt file | `rptInvoiceForCustomerDelivary.rpt` (`DelivaryInvoiceReturnViewer.aspx.cs:40`) |
| Request parameters | QueryString `InvNo` |
| Data source | `InvoiceBLL.DelivaryInvoiceMainDataForReport(invColl)` / `DelivaryInvoiceDetailDataForReport` → queries with `WHERE IV.DelivaryInvoiceNo IN (...)` (`SalesReturnDAL.cs` ~1495-1502, `ReturnInvoiceDAL.cs` `DelivaryInvoice*DataForReport`) |
| Mismatch | The page stores `SndReturnInvoiceNo='SRN-'+InvoiceNo` but prints with `RTN-`+InvoiceNo, and the query filters `DelivaryInvoiceNo` — the key passed cannot match a delivery number, so the print is expected to be empty (INFERRED; GAP-M08-012). |
| Output | Crystal viewer |
