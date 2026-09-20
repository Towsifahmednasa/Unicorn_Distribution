# RPT-M08-01 — Sales Return report family (market returns)

| Item | Detail |
|---|---|
| List / launcher pages | `SInventory_UI/AllSalesReturnReport.aspx` (PAGE-283: DC, from/to date; "National" button), `SInventory_UI/SalesReturnReport.aspx` (PAGE-297: on-screen grid + total "Return Amount (TP+VAT)") |
| Viewer pages | `SInventory_RPTVIEW/AllSalesReturnreportViewer.aspx` (PAGE-279), `SInventory_RPTVIEW/SalesReturnViewer.aspx` (PAGE-282, calls the *sales* query `SalesReportDAl`, see gap) |
| .rpt file | `crpAllSalesReturnReport.rpt` (loaded through `ReportPath(...)`, folder `Solution.Web/Reports/CrystalReports`) — both viewers |
| Dataset/table name | `DataSet` holding the query result table (`mainDataTable`) |
| Request parameters | QueryString `fromDate`, `toDate`, `districtId` (DC id), `NationalReport` (=1 for all DCs); `Session["SalesReport"]` (0 = Sales Return list, 1 = other) |
| Data source (viewer) | `CompanySalesReportBLL.SalesReturnReportDAl(districtId, from, to)` / `SalesReturnReportNationalDAl(from, to)` → `Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:26-66` (inline parameterised SQL) |
| Query summary | `tblReturnInvoice` ⨝ `tblReturnInvoiceDetail` ⨝ `tblCompanyUnit` ⨝ `tblCustMaster` ⟕ `tblDCStore` ⟕ `tblProduct` ⟕ `tblProductSQ` and **INNER JOIN `tblInvoice IV ON I.ReturnInvoiceNo = IV.AdjustInvoiceNo_ReturnInvoiceNo`**, filtered by DC and date range |
| List page data (PAGE-297) | `_DAL.GetSalesReturnReportListDAL(Parm(), Parm2())` → `CmnCrystaltoView.cs:317` → SP `sp_Get_SalesReturnReport` (body **not in repo**); `Parm()` concatenates `AND I.ComUnitId='<DC>'` and a `CONVERT(date, ISNULL(I.PaymentDate, ISNULL(I.UpdateDate, I.InvoiceDate))) BETWEEN '<from>' AND '<to>'` fragment (SQL-injection surface); start date floored to 01-Apr-2022 (`SalesReturnReport.aspx.cs:32-36`) |
| Output / export | Excel (`ExportFormatType.ExcelRecord`) streamed by the viewer (`AllSalesReturnreportViewer.aspx.cs:43,65`); grid on PAGE-297 |
| Permissions | menu-based; `SalesReturnReport` loads DCs by `Session["UserId"]` (`LoadSC(ddl,UserId)`), `AllSalesReturnReport` loads all DCs via `OtherStockActionBLL.DCLoad` |
| Confidence | viewer/query CONFIRMED; SP body UNKNOWN |

## Notes / gaps
- The viewer query joins returns to `tblInvoice` via `AdjustInvoiceNo_ReturnInvoiceNo`: only returns that were **adjusted against an invoice** can appear; market returns from PAGE-294 that were never adjusted are excluded (INFERRED from the INNER JOIN; GAP-M08-013).
- Columns are aliased misleadingly (`IV.InvoiceDate as OrderNo`, `IV.ReceivableAmount as OrderDate`, `I.Remarks as Brand`); the `.rpt` is bound to these aliases.
- `SalesReturnViewer.aspx` invokes `SalesReportDAl` (sales, not returns) while loading the same `.rpt` name — copy/paste risk (GAP-M08-013).
- Deduction charge and net return amount are not columns of the SELECT.
