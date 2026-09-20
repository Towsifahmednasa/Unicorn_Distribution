# M12 reports
| ID | Report | Viewer | List/launcher page |
|---|---|---|---|
| RPT-M12-01 | DC->Sub-depot chalan | DcToSubdepotChalanReportViewer (PAGE-426) | PAGE-450 (also 448/451 misuse) |
| RPT-M12-02 | Sub-depot receive | SubDeportTransferStockReceiveReportViewer (PAGE-435) | PAGE-446 |
| RPT-M12-03 | Proforma invoice PDF | SubDeportInvoiceReportViewer (431) / SubDeportInvoicePrintReportViewer (430) | PAGE-456, 457, 462 |
| RPT-M12-04 | Delivery invoice + top sheet | SubDeportDelivaryInvoiceReturnViewer (428), ...TopSheet... (429) | PAGE-445 |
| RPT-M12-05 | Proforma top sheet, market picking | SubdeportTopSheetReportViewer (434), SubdeportMarketwisePicking (432) | PAGE-457/462, 447 |
| RPT-M12-06 | Sub-depot stock (Excel) | SubDeportStockReportViewer (433) | PAGE-454 |
| RPT-M12-07 | Stock-out voucher | SubDepotStockOutReportViewer (436) | PAGE-459, 443/460 |
| RPT-M12-08 | Area-wise monthly inventory | SubdeportAreaWiseMonthlySalesReportViewer (427) | PAGE-438 |
Viewers live in `Solution.Web/SInventory_RPTVIEW`; `.rpt` in `Solution.Web/Reports/CrystalReports` or `Library.CrystalReports/SInventory_RPT`. Viewers do no auth check and several read raw SQL fragments from Session (GAP-M12-023).
