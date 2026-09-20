# RPT-M11-03 — "In-Transit", "Aging In-Transit" and "MIO-wise In-Transit" reports (invoice receivables)

| Item | Detail |
|---|---|
| List pages | `InTransitReport.aspx` (PAGE-410), `AgingInTransitReport.aspx` (PAGE-406), `MIOwiseInTransitReport.aspx` (PAGE-411) |
| Viewers | `InTransitReportViewer.aspx` (PAGE-401), `AgingInTransitReportViewer.aspx` (PAGE-398), `MIOwiseInTransitReportViewer.aspx` (PAGE-402) in `SInventory_RPTVIEW` |
| Report files | `INTransitReport.rpt` (Excel branch; export file name "Receivable_Report"), `crpINTransitReport.rpt` (screen branch); MIO-wise: `MIOWIseReceivable.rpt` |
| Request parameters | QueryString `fromDate`, `toDate`, `districtId`, `NationalReport`, `MArket`, `MIO`; Session `Excel` ("Y"/"N"), `Param` (pre-built SQL fragment for MIO-wise) |
| Data sources | `InvoiceBLL.IntransitBll(districtId, from, to)` / `IntransitBll(from, to)` → `ReturnInvoiceDAL.IntransitReportDAl` (:1234-1256, parameterised inline SQL over `tblInvoice` etc. with `tblDetails.Net…`); `AgingIntransitReportDAl` → aging SP `sp_Get_AgingReceivableReport`; list grid SP `sp_Get_IntransitReportList`; MIO-wise `CmnCrystaltoView.Get_MIOWiseReceiveableReport` → `sp_Get_NewReceiveableList` (body not in repo); MIO/territory lookups `sp_Get_EmployeeMIO`, `sp_CS_GetTerritory_All` |
| Output | Excel export or Crystal viewer |
| **Semantic finding** | Despite the module name, these reports list **customer invoice receivables** (delivered/invoiced amounts still to be collected: "Receivable_Report", "MIOWiseReceivable"). They do **not** report stock transfers awaiting receipt. Transfer in-transit visibility exists only as Pending/Received status on `B2BTransferView` (`tblChalanInfo.IsDeliver`) and by the pending lists of the receive pages (GAP-M11-017). |
| Security | MIO-wise viewer trusts `Session["Param"]` (concatenated SQL) |
| Confidence | viewer wiring CONFIRMED; SP bodies UNKNOWN; page-level detail Tier B |
