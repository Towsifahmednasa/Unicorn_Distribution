# INT-07 - MIGO (SAP goods-movement) Excel import -> central store

| Item | Detail |
|---|---|
| Direction | SAP export file (Excel, manual) -> ERP |
| Transport | File upload on `SInventory_UI/MIGO_Upload.aspx` (PAGE-688): saved to `~/ExcelFiles/<client name>`, read via OLEDB (Jet `.xls` / ACE `.xlsx`, first sheet, `HDR=YES`). |
| Trigger | Manual: choose Document Date + Manufacturer, upload, Submit; later `MIGOList.aspx` (PAGE-689) Transfer. |
| Payload (23 columns by position) | ShipToParty, PONo, PODate, ItemNo, OrderDocNo, OrderDocDate, DeliveryDocNo, DeliveryDocDate, LMID, LMIDDescription, Batch, ExpDate, MfgDate, Qty, VATChallan, BilltoParty, InvoiceNo, InvoiceDate, CaseNoofShipper, VAT, Amount, Total, TransportNo (`MIGO_Upload.aspx.cs:37-64`). |
| Tables / SPs | `tblMIGOMaster` (`MigoMasterID` = MAX+1, `MogoCode` generated, `StockUpload=0`, EntryBy=LoginName), `tblMIGODetail` (one INSERT per row via `SInventorySql.Execute`, no shared transaction); transfer `sp_StockInMIGOtoCentralStore(@MigoMasterID_In)` (body UNKNOWN; result > 0 = success); delete allowed only when `StockUpload=false` (deletes master then detail); reports `MIGOReportViewer` (`crpMigoDetailsReport.rpt`, `crpOrderDetailsReport.rpt`, `crpMainMigoReport.rpt`). |
| Status / retry | `StockUpload` flag prevents deletion after transfer; no retry, re-upload creates a new master. |
| Authentication | Session login (`LoginName`). |
| Error handling | `ShowMessageBox(ex.ToString())` (full exception text to user); row conversion (`Convert.ToDateTime/Decimal`) failures abort mid-way leaving partial detail rows. |
| Risks | Client-controlled file name saved in web folder, string-concatenated filter in `MIGOList.Parameter()` (also missing closing quote), non-atomic detail load. BR-M17-017. |
