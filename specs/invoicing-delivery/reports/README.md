# M07 reports

Two report-family documents:
- [RPT-M07-01-invoice-and-proforma-print.md](RPT-M07-01-invoice-and-proforma-print.md) - invoice/proforma/delivery/return prints and proforma/sales lifecycle reports.
- [RPT-M07-02-top-sheet-picking-challan.md](RPT-M07-02-top-sheet-picking-challan.md) - top sheet, market-wise picking, DC picking slip, challan, loading, deleted-invoice reports.

All viewers live in `Solution.Web/SInventory_RPTVIEW/` and load `.rpt` files from `Solution.Web/Reports/CrystalReports/` (some also in `Library.CrystalReports/SInventory_RPT/`). Not opened: `.rpt` field layouts and formulas (UNKNOWN). Report SQL is mostly inline in `Library.DAL/SInventory_DAL/InvoiceDAL.cs`.
