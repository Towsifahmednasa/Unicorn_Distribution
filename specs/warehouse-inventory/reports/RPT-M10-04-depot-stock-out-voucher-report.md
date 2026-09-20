# RPT-M10-04 Depot stock adjustment voucher print

**Openers:** `DepotStockAdjustmentsVoucherView.aspx` (`ReportView` command), `DcStockOutApproval.aspx` (`View` command), `SampleTypeConventionView.aspx` (`ReportView`, wrong document type).
**Viewer:** `SInventory_RPTVIEW/DcStockOutReportViewer.aspx` (PAGE-348). **Crystal file:** `Solution.Web/Reports/CrystalReports/crpDcStockOut.rpt`.
**Parameter:** QueryString `DcStockOutMasterId` (also `Session["DcStockOutMasterId"]`).
**Data:** `DeStockOutBLL.DcStockOutReportViewBll(id)` -> `DeStockOutDal.DcStockOutReportDal` (`DeStockOutDal.cs:163-180`): `tblDeStockOutMaster LEFT JOIN tblDeStockOutDetails, tblCompanyUnit, tblInvoice, tblCustMaster (on CustomerCode), tblunitprice (on ProductCode)`; columns `Total = UnitPrice*StackOutQty`, VATAmountPerUnit, UnitPrice, CustomerCode/Name/Address, ComUnitName, Invoice (`N/A` when `InvoiceId=0`), BatchNo, StackOutQty. **Defect:** the SELECT list is missing a comma between `... 'N/A' else tblInvoice.InvoiceNo end  Invoice` and `tblDeStockOutDetails.BatchNo` in the repository text (`DeStockOutDal.cs:165`) - the column list as committed reads `... end Invoice tblDeStockOutDetails.BatchNo,...` (alias glitch, may return an error/odd alias) (INFERRED from source text; not executed). Unit price join is unfiltered (`IsActive` not tested) so duplicates possible.
**Output:** Crystal viewer.
