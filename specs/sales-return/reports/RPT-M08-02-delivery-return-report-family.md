# RPT-M08-02 — Delivery Return report family

| Item | Detail |
|---|---|
| List / launcher pages | `SInventory_UI/DeliveryReturnReport.aspx` (PAGE-285: DC + dates; national), `SInventory_UI/DeliveryReturnReport_new.aspx` (PAGE-286: grid via `sp_Get_DeliveryReturnReport`) |
| Viewer | `SInventory_RPTVIEW/DeliveryReturnReportViewer.aspx` (PAGE-280) |
| .rpt file | `crpDeliveryReturnReport.rpt` |
| Dataset/table name | `deliveryDataTable` (`DeliveryReturnReportViewer.aspx.cs:38`) |
| Request parameters | QueryString `fromDate`, `toDate`, `districtId`, `NationalReport`; `Session["DeliveryReturnRpt"]` (0/1) |
| Data source | `CompanySalesReportBLL.DeliveryReturnReportDAl(districtId, from, to)` / `(from, to)` → `CompanySalesReportDAL.cs:115-152` inline SQL; list page PAGE-286: `CmnCrystaltoView.cs:295` → SP `sp_Get_DeliveryReturnReport` (body not in repo) |
| Query summary | `tblInvoice ⨝ tblInvoiceDetail ⨝ tblCompanyUnit ⨝ View_CustomerMaster ⨝ tblDCStore` **WHERE `ID.DeliveryStatus IN ('Reject','Partial')`**, `UNION ALL` the sub-depot equivalent; `Quantity = ID.Quantity − ID.DeliveryQuantity`, `Amount = ID.NetAmount − ID.DeliveryNetAmount`, `VatAmount = ID.TotalPriceVatAmount − ID.DeliveryTotalPriceVatAmount`, `ReturnReason` |
| Output / export | Excel (`ExportToHttpResponse ExcelRecord`, `DeliveryReturnReportViewer.aspx.cs:43,60`) or Crystal view |
| Confidence | viewer/query CONFIRMED; SP UNKNOWN |

## Notes / gaps
- This report measures the **delivery-confirmation shortfall** (`Quantity − DeliveryQuantity`), not the rows of `tblCustomerReturnMaster` created by PAGE-287 or `tblInvoiceDetailReturn` created by PAGE-298. Whether `sp_Get_DeliveryReturnReport` (PAGE-286) includes them is UNKNOWN (GAP-M08-013).
- The date filter compares `I.UpdateDate` only.
