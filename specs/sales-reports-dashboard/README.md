# M13 - Sales Reports, Analytics & Dashboards

_Reverse-engineered 2026-09-19. Documentation only; evidence = source paths; SP bodies are absent from the repository (see GAPS-M13.md)._

## 1. Purpose and business overview
M13 holds the management-information layer of the sales & distribution ERP: 75 pages = the post-login **AdminDashboard** (28 AJAX endpoints), the **Business Summary / MIS** report family (many near-duplicate variants), sales reports by company/DC/region/zone/district/territory/MIA/MIO/customer/product, GP/COGS reports, trend/comparison reports, the field-activity **DynamicSalesReport**, 26 popup **viewer** pages (`SInventory_RPTVIEW`) that export Crystal reports to Excel/screen, an app web-view (**SixTenProjectReportApps**, DoctorApps) and two **data-modifying admin operations**: `SalesOperationRollback` (proforma / delivery / payment) and `LogisticeRollBackOperation` (STO / picking / challan).
Business value: management sees invoiced vs delivered ("sales") vs returned vs (invoice-minus-return-minus-sales) "outstanding" amounts per depot/zone/area/territory/MIO; sales-force activity (DCR/Rx) vs sales; GP on realised collections; and can undo mistakes in the order-to-cash chain.

## 2. Page catalogue (75 pages)
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-463 | SixTenProjectReportApps ((root)) | Report / Dashboard | A | [PAGE-463-sixtenprojectreportapps](pages/PAGE-463-sixtenprojectreportapps.md) |
| PAGE-464 | AdminDashboard (Dashboard_UI) | Report / Dashboard | A | [PAGE-464-admindashboard](pages/PAGE-464-admindashboard.md) |
| PAGE-465 | DashboardOne (Dashboard_UI) | Report / Dashboard | B | [PAGE-465-dashboardone](pages/PAGE-465-dashboardone.md) |
| PAGE-466 | AllSalesreportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-466-allsalesreportviewer](pages/PAGE-466-allsalesreportviewer.md) |
| PAGE-467 | AreaWiseMonthlySalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-467-areawisemonthlysalesreportviewer](pages/PAGE-467-areawisemonthlysalesreportviewer.md) |
| PAGE-468 | AuditReportOneReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-468-auditreportonereportviewer](pages/PAGE-468-auditreportonereportviewer.md) |
| PAGE-469 | BusinessSummaryViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-469-businesssummaryviewer](pages/PAGE-469-businesssummaryviewer.md) |
| PAGE-470 | BusinessSummaryViewer2 (SInventory_RPTVIEW) | Report viewer | B | [PAGE-470-businesssummaryviewer2](pages/PAGE-470-businesssummaryviewer2.md) |
| PAGE-471 | BusinessSummaryViewer3 (SInventory_RPTVIEW) | Report viewer | B | [PAGE-471-businesssummaryviewer3](pages/PAGE-471-businesssummaryviewer3.md) |
| PAGE-472 | BusinessSummaryViewer4 (SInventory_RPTVIEW) | Report viewer | B | [PAGE-472-businesssummaryviewer4](pages/PAGE-472-businesssummaryviewer4.md) |
| PAGE-473 | BusinessSummaryViewer5 (SInventory_RPTVIEW) | Report viewer | B | [PAGE-473-businesssummaryviewer5](pages/PAGE-473-businesssummaryviewer5.md) |
| PAGE-474 | CompanySalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-474-companysalesreportviewer](pages/PAGE-474-companysalesreportviewer.md) |
| PAGE-475 | ComUnitSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-475-comunitsalesreportviewer](pages/PAGE-475-comunitsalesreportviewer.md) |
| PAGE-476 | CustomerSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-476-customersalesreportviewer](pages/PAGE-476-customersalesreportviewer.md) |
| PAGE-477 | DayWiseBusinessReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-477-daywisebusinessreportviewer](pages/PAGE-477-daywisebusinessreportviewer.md) |
| PAGE-478 | DistrictSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-478-districtsalesreportviewer](pages/PAGE-478-districtsalesreportviewer.md) |
| PAGE-479 | EmployeewiseProductSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-479-employeewiseproductsalesreportviewer](pages/PAGE-479-employeewiseproductsalesreportviewer.md) |
| PAGE-480 | ExcelSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-480-excelsalesreportviewer](pages/PAGE-480-excelsalesreportviewer.md) |
| PAGE-481 | FinalSalesViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-481-finalsalesviewer](pages/PAGE-481-finalsalesviewer.md) |
| PAGE-482 | FixedCustomerSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-482-fixedcustomersalesreportviewer](pages/PAGE-482-fixedcustomersalesreportviewer.md) |
| PAGE-483 | HistoricalReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-483-historicalreportviewer](pages/PAGE-483-historicalreportviewer.md) |
| PAGE-484 | MiaWiseSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-484-miawisesalesreportviewer](pages/PAGE-484-miawisesalesreportviewer.md) |
| PAGE-485 | MIOProductSalesreportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-485-mioproductsalesreportviewer](pages/PAGE-485-mioproductsalesreportviewer.md) |
| PAGE-486 | ProductWiseNationalSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-486-productwisenationalsalesreportviewer](pages/PAGE-486-productwisenationalsalesreportviewer.md) |
| PAGE-487 | RegionSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-487-regionsalesreportviewer](pages/PAGE-487-regionsalesreportviewer.md) |
| PAGE-488 | SalesComparisoneReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-488-salescomparisonereportviewer](pages/PAGE-488-salescomparisonereportviewer.md) |
| PAGE-489 | SalesTrendReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-489-salestrendreportviewer](pages/PAGE-489-salestrendreportviewer.md) |
| PAGE-490 | ZoneSalesReportViewer (SInventory_RPTVIEW) | Report viewer | B | [PAGE-490-zonesalesreportviewer](pages/PAGE-490-zonesalesreportviewer.md) |
| PAGE-491 | AllSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-491-allsalesreport](pages/PAGE-491-allsalesreport.md) |
| PAGE-492 | AuditReportOne (SInventory_UI) | Report / Dashboard | B | [PAGE-492-auditreportone](pages/PAGE-492-auditreportone.md) |
| PAGE-493 | BranchTotalSummary (SInventory_UI) | Report / Dashboard | B | [PAGE-493-branchtotalsummary](pages/PAGE-493-branchtotalsummary.md) |
| PAGE-494 | BusinessSummary (SInventory_UI) | Report / Dashboard | B | [PAGE-494-businesssummary](pages/PAGE-494-businesssummary.md) |
| PAGE-495 | COGSBasedGPReport (SInventory_UI) | Report / Dashboard | A | [PAGE-495-cogsbasedgpreport](pages/PAGE-495-cogsbasedgpreport.md) |
| PAGE-496 | CompanySalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-496-companysalesreport](pages/PAGE-496-companysalesreport.md) |
| PAGE-497 | ComUnitSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-497-comunitsalesreport](pages/PAGE-497-comunitsalesreport.md) |
| PAGE-498 | Copy of TotalSummaryNew (SInventory_UI) | Report / Dashboard | B | [PAGE-498-copy-of-totalsummarynew](pages/PAGE-498-copy-of-totalsummarynew.md) |
| PAGE-499 | CustomerWiseSale (SInventory_UI) | Page | B | [PAGE-499-customerwisesale](pages/PAGE-499-customerwisesale.md) |
| PAGE-500 | CustomerWiseSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-500-customerwisesalesreport](pages/PAGE-500-customerwisesalesreport.md) |
| PAGE-501 | DailyBusinessReport (SInventory_UI) | Report / Dashboard | B | [PAGE-501-dailybusinessreport](pages/PAGE-501-dailybusinessreport.md) |
| PAGE-502 | DayWiseBusinessReport (SInventory_UI) | Report / Dashboard | B | [PAGE-502-daywisebusinessreport](pages/PAGE-502-daywisebusinessreport.md) |
| PAGE-503 | DistrictSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-503-districtsalesreport](pages/PAGE-503-districtsalesreport.md) |
| PAGE-504 | DoctorApps (SInventory_UI) | Page | B | [PAGE-504-doctorapps](pages/PAGE-504-doctorapps.md) |
| PAGE-505 | DynamicSalesReport (SInventory_UI) | Report / Dashboard | A | [PAGE-505-dynamicsalesreport](pages/PAGE-505-dynamicsalesreport.md) |
| PAGE-506 | DZSMTotalSummary (SInventory_UI) | Report / Dashboard | A | [PAGE-506-dzsmtotalsummary](pages/PAGE-506-dzsmtotalsummary.md) |
| PAGE-507 | EmployeewiseProductSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-507-employeewiseproductsalesreport](pages/PAGE-507-employeewiseproductsalesreport.md) |
| PAGE-508 | FinalSales (SInventory_UI) | Page | B | [PAGE-508-finalsales](pages/PAGE-508-finalsales.md) |
| PAGE-509 | FinalsalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-509-finalsalesreport](pages/PAGE-509-finalsalesreport.md) |
| PAGE-510 | FinalsalesReport_T (SInventory_UI) | Report / Dashboard | B | [PAGE-510-finalsalesreport-t](pages/PAGE-510-finalsalesreport-t.md) |
| PAGE-511 | GpSalesReport (SInventory_UI) | Report / Dashboard | A | [PAGE-511-gpsalesreport](pages/PAGE-511-gpsalesreport.md) |
| PAGE-512 | HistoricalReport (SInventory_UI) | Report / Dashboard | B | [PAGE-512-historicalreport](pages/PAGE-512-historicalreport.md) |
| PAGE-513 | InvoiceWiseDetailsSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-513-invoicewisedetailssalesreport](pages/PAGE-513-invoicewisedetailssalesreport.md) |
| PAGE-514 | LogisticeRollBackOperation (SInventory_UI) | Page | A | [PAGE-514-logisticerollbackoperation](pages/PAGE-514-logisticerollbackoperation.md) |
| PAGE-515 | MIAWiseSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-515-miawisesalesreport](pages/PAGE-515-miawisesalesreport.md) |
| PAGE-516 | MIOProductSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-516-mioproductsalesreport](pages/PAGE-516-mioproductsalesreport.md) |
| PAGE-517 | MIOWiseBusinessSummary (SInventory_UI) | Report / Dashboard | A | [PAGE-517-miowisebusinesssummary](pages/PAGE-517-miowisebusinesssummary.md) |
| PAGE-518 | MIOWiseTotalSummaryNew (SInventory_UI) | Report / Dashboard | B | [PAGE-518-miowisetotalsummarynew](pages/PAGE-518-miowisetotalsummarynew.md) |
| PAGE-519 | MIOWiseTotalSummaryNew_T (SInventory_UI) | Report / Dashboard | B | [PAGE-519-miowisetotalsummarynew-t](pages/PAGE-519-miowisetotalsummarynew-t.md) |
| PAGE-520 | MonitoringReport (SInventory_UI) | Report / Dashboard | A | [PAGE-520-monitoringreport](pages/PAGE-520-monitoringreport.md) |
| PAGE-521 | PendingSalesConfirmationReport (SInventory_UI) | Report / Dashboard | B | [PAGE-521-pendingsalesconfirmationreport](pages/PAGE-521-pendingsalesconfirmationreport.md) |
| PAGE-522 | ProductWiseNationalSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-522-productwisenationalsalesreport](pages/PAGE-522-productwisenationalsalesreport.md) |
| PAGE-523 | ProductWiseSalesReport (SInventory_UI) | Report / Dashboard | A | [PAGE-523-productwisesalesreport](pages/PAGE-523-productwisesalesreport.md) |
| PAGE-524 | ProformaandSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-524-proformaandsalesreport](pages/PAGE-524-proformaandsalesreport.md) |
| PAGE-525 | RegionSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-525-regionsalesreport](pages/PAGE-525-regionsalesreport.md) |
| PAGE-526 | SalesComparisoneReport (SInventory_UI) | Report / Dashboard | B | [PAGE-526-salescomparisonereport](pages/PAGE-526-salescomparisonereport.md) |
| PAGE-527 | SalesConfirmationReport_New (SInventory_UI) | Report / Dashboard | B | [PAGE-527-salesconfirmationreport-new](pages/PAGE-527-salesconfirmationreport-new.md) |
| PAGE-528 | SalesOperationRollback (SInventory_UI) | Page | A | [PAGE-528-salesoperationrollback](pages/PAGE-528-salesoperationrollback.md) |
| PAGE-529 | SalesTrendReport (SInventory_UI) | Report / Dashboard | B | [PAGE-529-salestrendreport](pages/PAGE-529-salestrendreport.md) |
| PAGE-530 | TerritoryWiseSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-530-territorywisesalesreport](pages/PAGE-530-territorywisesalesreport.md) |
| PAGE-531 | TotalSummary (SInventory_UI) | Report / Dashboard | B | [PAGE-531-totalsummary](pages/PAGE-531-totalsummary.md) |
| PAGE-532 | TotalSummaryNew (SInventory_UI) | Report / Dashboard | A | [PAGE-532-totalsummarynew](pages/PAGE-532-totalsummarynew.md) |
| PAGE-533 | TotalSummaryNew2 (SInventory_UI) | Report / Dashboard | A | [PAGE-533-totalsummarynew2](pages/PAGE-533-totalsummarynew2.md) |
| PAGE-534 | TotalSummaryNew_PT (SInventory_UI) | Report / Dashboard | B | [PAGE-534-totalsummarynew-pt](pages/PAGE-534-totalsummarynew-pt.md) |
| PAGE-535 | TotalSummaryNew_T (SInventory_UI) | Report / Dashboard | A | [PAGE-535-totalsummarynew-t](pages/PAGE-535-totalsummarynew-t.md) |
| PAGE-536 | TotalSummaryProductWise (SInventory_UI) | Report / Dashboard | B | [PAGE-536-totalsummaryproductwise](pages/PAGE-536-totalsummaryproductwise.md) |
| PAGE-537 | ZoneSalesReport (SInventory_UI) | Report / Dashboard | B | [PAGE-537-zonesalesreport](pages/PAGE-537-zonesalesreport.md) |

## 3. Business rules index
| BR id | Pages | Rule (short) | Confidence |
|---|---|---|---|
| BR-M13-001 | 528 | Rollback requires a non-empty Code/No, a non-empty reason (remarks) and a live session (Session["LoginName"] != null); otherwise an alert is shown and nothing is written | CONFIRMED |
| BR-M13-002 | 528 | Proforma rollback is refused when the invoice already carries a DelivaryInvoiceNo ("Delivery Invoice already created! Please delete the Delivery Invoice first.") | CONFIRMED |
| BR-M13-003 | 528 | Proforma rollback (all in one SqlTransaction): (1) insert tblInvoiceDeleteLog row (DelInvoiceNo='PROFORMA', DeleteBy=login); (2) if OrderId>0: tblOrder.IsInvoice=0 and tb | CONFIRMED |
| BR-M13-004 | 528 | Stock reversal for a proforma line with Quantity>0: Rule 1 delete TOP(1) tblDCStore row where ReferenceDCStoreId=line.DCStoreId AND Trans_Type='Issue' AND StockQty=-Quant | CONFIRMED |
| BR-M13-005 | 528 | Delivery-invoice rollback is refused when any tblCustPayDetail row exists for the invoice ("Payment has been settled for this Delivery Invoice | CONFIRMED |
| BR-M13-006 | 528 | Delivery rollback first validates stock: for every line with returnQty = Quantity - DeliveryQuantity > 0, SUM(tblDCStore.StockQty) for ProductCode+BatchNo+ExpDate must be | CONFIRMED |
| BR-M13-007 | 528 | Delivery rollback reverses accounting: every voucher in CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster with CommonPK_ID=InvoiceId and TableID=2014 is copied to a NEW vou | CONFIRMED |
| BR-M13-008 | 528 | After reversal the delivery invoice is "un-delivered", not deleted: tblInvoiceDeleteLog + tblInvoiceDetail_DeleterRecord rows are written, tblInvoice.DelivaryInvoiceNo/De | CONFIRMED |
| BR-M13-009 | 528 | Payment (money receipt) rollback: locate tblCustomerPay.CollectionNo=code; reverse vouchers with CommonPK_ID=CustPayId and TableID=2017 (same swap-Dr/Cr copy as delivery) | CONFIRMED |
| BR-M13-010 | 528 | Each rollback executes on its own connection inside one SqlTransaction; any exception rolls back everything and returns "Error: <message>" which the page shows verbatim i | CONFIRMED |
| BR-M13-011 | 514 | "STO Generate" (SG) rollback lists tblRequisition rows with CreatePicking IS NULL (grouped with SUM(ReqQty)) and the delete icon physically deletes the requisition header | CONFIRMED |
| BR-M13-012 | 514 | "Picking Generate" (PG) rollback lists requisitions with Submit NULL/empty and CreatePicking set | CONFIRMED |
| BR-M13-013 | 514 | "Challan Generate" (CG) rollback lists tblRequisition with Submit='OK' and ReceiveIssue NULL/empty (issued but not yet received by DC) and performs the picking rollback s | CONFIRMED |
| BR-M13-014 | 514 | The logistics rollback steps are separate auto-committed SQL calls (SInventorySql.Execute) with no transaction; the success alert depends only on the picking-reset and de | CONFIRMED |
| BR-M13-015 | 464 | Dashboard access: UserRoleID "2" is admitted without a permission lookup; any other role must return >=1 row from GetPermissionForUserRole(<page path>) (sp_GET_MainPermis | CONFIRMED |
| BR-M13-016 | 464 | KPI header scope by RoleTypeName (Session): DZSM -> hierarchy RegionIds (sp_HigharchyInfoByEmployeeId) joined by comma -> sp_Get_DashboardTopBarData_Zone(@ZoneId); NSM -> | CONFIRMED |
| BR-M13-017 | 464 | GetSalesChartData and GetOrderChartData build one column per selected month (MMM-yyyy) and a "Total" column that is the SUM of the month values DIVIDED by the number of s | CONFIRMED |
| BR-M13-018 | 464 | Several endpoints re-use their date parameters as selectors: for GetSalesChartData fromdt = "Invoice"\|"Payment" (SP choice) and todt = customer-type id; for GetOrderChart | CONFIRMED |
| BR-M13-019 | 464 | Drill-down filters are assembled in browser JS as a SQL fragment (" AND tblOrder.CustTypeId=. | INFERRED |
| BR-M13-020 | 464 | Month-loop start/end dates are built with new DateTime(year, month, DateTime.Now.Day); on days 29-31 this throws for shorter months, and the loop guard (Month<=Month && Y | CONFIRMED |
| BR-M13-021 | 531, 494, 493 | Invoice (proforma) KPIs: NumberofProformaInvoice = COUNT(DISTINCT InvoiceId), SumofNetProformaAmount = SUM(NetAmount) - SUM(TotalPriceVatAmount) (net of VAT), ProTpVat =  | CONFIRMED |
| BR-M13-022 | 531, 494, 493 | Sales (delivered) KPIs: only DeliveryInvoiceStatus IN ('Partial','Full') AND TpGrandTotal>0 AND UpdateDate BETWEEN dates; NumberofInvoiceSold = COUNT(DISTINCT InvoiceId), | CONFIRMED |
| BR-M13-023 | 531, 494, 493 | Return (delivery-rejection) KPIs: lines with DeliveryStatus IN ('Reject','Partial') and UpdateDate in range: SumofNetReturnAmount = (SUM(NetAmount)-SUM(TotalPriceVatAmoun | CONFIRMED |
| BR-M13-024 | 531, 536, 518, 533, 520 | Derived columns (exact formulas in inline SQL): NetInvoiceAmt = SumofNetProformaAmount+ProTpVat; NetReturnAmt = SumofNetReturnAmount+DelReTpVat; NetSalesAmt = SumofNetSal | CONFIRMED (inline SQL) / UNKNOWN (SP-fed pages) |
| BR-M13-025 | 531, 494, 493 | Undelivered invoices: DeliveryInvoiceStatus IS NULL and InvoiceDate in range -> NumberofUndelInvoice, SumofNetUnAmount (NetAmount-TotalPriceVatAmount), UnTpVat; orders: N | CONFIRMED |
| BR-M13-026 | 531, 494 | CustomerCoverPer = (number of tblCustMaster customers of the ComUnit having an invoice in the period * 100) / (all tblCustMaster customers of the ComUnit) using INTEGER d | CONFIRMED |
| BR-M13-027 | 531, 532, 533, 535, 517 | Date bases differ per measure and are therefore not comparable across columns: proforma = InvoiceDate, sales/returns/deliveries = UpdateDate, orders = SubmissionDate, col | CONFIRMED |
| BR-M13-028 | 532, 533, 535, 523, 520 | Report scope radio (rbReportTypeName 1 Distribution Center, 2 Zone, 3 Area, 4 Territory) is translated into the SP argument @Type: "SC", "Zone", then for Area/Territory " | CONFIRMED |
| BR-M13-029 | 532, 533, 535, 523 | Role narrowing on load (Session RoleTypeName) | CONFIRMED |
| BR-M13-030 | 532, 533, 535, 506, 517 | Page footers are computed server-side in C# by summing the returned DataTable per column (null -> 0) | CONFIRMED |
| BR-M13-031 | 532 | TotalSummaryNew.LoadInfo only runs when zone/sales-center/territory legacy dropdowns are all empty (otherwise message "Please Select Date Range!!"); with the current UI t | CONFIRMED |
| BR-M13-032 | 531, 469 | On TotalSummary the Excel button (excelButton1 -> excelButton1_Click) opens BusinessSummaryViewer.aspx?fromDate&toDate in a popup, which ignores every on-screen filter an | CONFIRMED |
| BR-M13-033 | 506 | DZSMTotalSummary derives the caller hierarchy from Session (RoleTypeName/EmpInfoId/RoleTypeId) via GetHigharcyInfoByEmployeeId: AM -> AreaId list, DZSM -> RegionId list,  | CONFIRMED |
| BR-M13-034 | 506 | The grid is fed by TotalSummaryDAL.DZSMwiseLoadSummaryparm(from,to,parm) -> sp_GET_DZSMwiseReportParam_ByProcess(@fromdate,@todate,@parm) where parm is the raw ZoneSelect | CONFIRMED |
| BR-M13-035 | 463 | SixTenProjectReportApps (mobile/web-view page, no master page) takes EmpInfoId from the QueryString, loads that employee's RoleTypeId/RoleType via Get_EmpGeneralInfoByEmp | CONFIRMED |
| BR-M13-036 | 463 | KPI tiles are sums over the SP result: lblKpiDCP = SUM(DCP), lblKpiDCR = SUM(DCR), lblKpiRX = SUM(RX), lblKpiSales = SUM(TSalesNetTP); non-numeric/NULL cells count as 0 ( | CONFIRMED |
| BR-M13-037 | 495 | COGS-based GP report is realised on the PAYMENT date: the date filter is CONVERT(date, CPY.PaymentDate) BETWEEN from AND to (if only From is given, To = today); company u | CONFIRMED |
| BR-M13-038 | 495 | GP arithmetic is not in C#: the SP returns Month, Date, ComUnitName, BUWise, Particulars, SKU, Qty, OrderSenderName, RealizationAmount, VATAmount, TaxAmount, COGSAmount,  | CONFIRMED (page) / UNKNOWN (SP) |
| BR-M13-039 | 495 | Search results are cached in ASP.NET Cache for 10 minutes under "COGSBasedGPReportData_<UserId>"; btnExport exports the cached table when present, so an export after chan | CONFIRMED |
| BR-M13-040 | 511 | GpSalesReport (Territory-wise GP sales) filters by CONVERT(date, I.UpdateDate) BETWEEN from AND to, ComUnit and market-structure levels via a concatenated @Parm to sp_Get | CONFIRMED |
| BR-M13-041 | 505 | The main DynamicSalesReport grid calls SP GetDynamicSalesReportList(@fromdate,@todate,@Type="",@Area,@Terr,@ZonId,@grpId) using ONLY the two dates and the group/zone/area | CONFIRMED |
| BR-M13-042 | 505 | Each numeric cell is a LinkButton that stores the row's hidden TerritoryId in _hfTerritoryId and opens an AJAX modal loaded with a per-metric list: invoices (GetProformaI | CONFIRMED (page) / UNKNOWN (most SPs) |
| BR-M13-043 | 505 | Defect: TotalNoOfHqDoctorParam hard-codes ProgramTypeId="4" but tests `if (ProgramTypeId == "")`, so the program-type predicate is never added; HQ, Ex-HQ and OS doctor dr | CONFIRMED |
| BR-M13-044 | 505 | Entering an invoice From-date before 01-Apr-2021 resets it to "01 April, 2021". | CONFIRMED |
| BR-M13-045 | 520 | MonitoringReport shows, per scope level (SC/Zone/Area/Territory), the invoice/return/sales/outstanding block (same columns as TotalSummary) plus period roll-forward colum | CONFIRMED (wiring) / UNKNOWN (formulas) |
| BR-M13-046 | 523 | ProductWiseSalesReport (MIS) calls sp_RPT_MIS_ProductWiseSalesReport(@fromdate,@todate,@Type,@Area,@Terr,@ZonId) and displays Sales (TP/Discount/VAT/Gross), Return, "Just | CONFIRMED (wiring) / UNKNOWN (formulas) |
| BR-M13-047 | 517 | MIOWiseBusinessSummary is a copy of TotalSummaryNew2 whose only functional change is the data call LoadMIOWiseBusinessSummaryDAL(salesCenter, from, to) -> sp_RPT_MIOWiseB | CONFIRMED |
| BR-M13-048 | 535 | TotalSummaryNew_T is TotalSummaryNew2 with LoadSummaryProductcodewiseGyash__New -> sp_BusinessSummaryMISReport_TT(@fromdate,@todate,@Type,@Area,@Terr,@ZoneId) and the rec | CONFIRMED |
| BR-M13-049 | 533 | TotalSummaryNew2 ("Business Summary" in the legacy static menu) calls LoadSummaryProductcodewiseGyash(from,to,Type,Area,Terr,ZoneId) -> sp_BusinessSummaryMISReport_All_Ne | CONFIRMED |
| BR-M13-050 | 466, 491, 481, 508, 485, 516... | Sales-detail Excel report (crpAllSalesReport.rpt): one row per DELIVERED invoice line (ID.DeliveryStatus IN ('Full','Partial')), UNION ALL of depot (tblInvoice/tblInvoice | CONFIRMED |
| BR-M13-051 | 485, 516 | MIO/territory/product/district/region-wise sales is produced by appending a caller-built predicate to the sales query: MIOProductSalesReport builds a predicate such as AN | CONFIRMED |
| BR-M13-052 | 474, 475, 478, 484, 476, 487... | Company/ComUnit/Region/Zone/District/MIA/Customer sales reports aggregate View_MIAWiseSalesReport by ProductCode+Product: TotalQty = SUM(TotalQuantity), TotalAmount = SUM | CONFIRMED |
| BR-M13-053 | 478 | DistrictSalesReportDAL builds both its header and detail queries by string concatenation of districtId and fromDate/toDate (date formatting depends on server culture) - S | CONFIRMED |
| BR-M13-054 | 489, 529 | SalesTrendReport period = 01-Jan to 31-Dec of the chosen year; data from sp_GET_MonthwiseSale(@FromDate,@ToDate); CRP uses crpSalesTrendReport.rpt, the Excel option loads | CONFIRMED |
| BR-M13-055 | 477, 502 | DayWiseBusinessReport period is hard-wired to 01-<month>-<year> . | CONFIRMED |
| BR-M13-056 | 488, 526 | SalesComparisone report: per district, SaleValue = SUM(DeliveryTpGrandTotal) of invoices with DeliveryInvoiceStatus IN ('Full','Partial') in the calendar month of @Report | CONFIRMED |
| BR-M13-057 | 486, 522 | ProductWiseNationalSalesReport: for @FromDate month and each of the 3 previous months per product: DeliveryTotal = SUM(DeliveryTotalQuantity), SaleValue = SUM(DeliveryNet | CONFIRMED |
| BR-M13-058 | 483, 512 | HistoricalReport: the list page passes toDate = fromDateTextBox value (defect: both ends are the From date) and the DAL selects columns of tblCentralStore relabelled as M | CONFIRMED |
| BR-M13-059 | 515 | MIAWiseSalesReport option "Total Between Date" opens MiaSalesReportViewer.aspx, which exists only as MiaSalesReportViewer.aspx.exclude (excluded from the build) - dead li | CONFIRMED |
| BR-M13-060 | 470, 471, 472, 473, 493 | BranchTotalSummary offers four Excel exports: BusinessSummaryViewer2 (delivered sales by UpdateDate per depot/day, rptBranchSales.rpt), Viewer3 (proforma by InvoiceDate,  | CONFIRMED |
| BR-M13-061 | 468, 492 | AuditReportOne lists DELETED orders (tblOrderDel + tblOrderDetailDel) with MIO/RSM/territory/zone, deleter (tblUser.LoginName) and DelDate via sp_Get_OrderDelTrackingList | CONFIRMED |
| BR-M13-062 | 467 | AreaWiseMonthlySalesReportViewer: national=false -> sp_AreawiseDailyOpeningClosingStockDepowise(@fromDate,@toDate,@CiD) else sp_AreawiseDailyOpeningClosingStockNational(@ | CONFIRMED |
| BR-M13-063 | 524 | ProformaandSalesReport exports the invoice life-cycle via sp_InvoceLifecycle(@fromDate,@toDate); the district/ComUnit argument is accepted by the DAL but never sent to th | CONFIRMED |
| BR-M13-064 | 509, 510, 508, 481 | FinalsalesReport / FinalsalesReport_T (identical code, class name only differs) show sp_GetFinalSales(@fromdate,@todate) plus a customer/invoice count panel from sp_Numbe | CONFIRMED |
| BR-M13-065 | 499 | CustomerWiseSale builds a month-by-month pivot in C# (TotalSummaryBLL.CustomerWiseSale): for each month between From and To it calls GetmonthlycustomerSaleDate(first, las | CONFIRMED |
| BR-M13-066 | 521 | PendingSalesConfirmationReport lists orders/invoices awaiting confirmation via sp_GET_PendingSalesConfirmationReport(@Parm) (predicate string incl | CONFIRMED (wiring) / UNKNOWN (SP) |
| BR-M13-067 | 527 | SalesConfirmationReport_New filters (ComUnit, market structure, program type, chemist type, customer, product, order sender) are concatenated into @Parm/@Parm2 for sp_Get | CONFIRMED |
| BR-M13-068 | 466, 481, 469, 470, 471, 472... | Viewer pages (SInventory_RPTVIEW) are stand-alone pages with no master page and no Session/login or role check; they trust QueryString (fromDate, toDate, ids, rptType, Na | INFERRED |

## 4. Status lifecycle
Report pages are read-only. Statuses they read: `tblInvoice.DeliveryInvoiceStatus` (NULL = undelivered, Partial, Full), `tblInvoiceDetail.DeliveryStatus` (Full, Partial, Reject), `tblInvoice.PaymentStatus/PaymentAmount`, `tblOrder.IsInvoice`, `tblRequisition.CreatePicking/Submit/ReceiveIssue`. Statuses **modified** only by the two rollback pages: order invoiced -> not invoiced; delivery Full/Partial -> NULL; payment settled -> NULL; requisition challan-issued -> picking -> STO -> deleted (BR-M13-002..014). Forward transitions are set by other modules.

## 5. Roles and permissions observed
- Menu is DB-driven (`sp_GET_MainMenuRole2`); `NewMasterPage.master.cs:24-56` sends anonymous first-loads to `Login.aspx`.
- Session keys used: UserId, UserType ("Admin" sees all DCs, others only `ComUnitId`), ComUnitId, RoleTypeName (DZSM/NSM/AM/MIO), RoleTypeId, EmpInfoId, LoginName, UserRoleID.
- Enforced checks: AdminDashboard page permission (BR-M13-015); DC lists restricted by `UserType`; MIS pages narrow radios/selectors by role (UI only, BR-M13-029). **No role check** on either rollback page; viewers and SixTenProjectReportApps have no session check; the 28 dashboard WebMethods have none; Web.config has no `<authorization>` rules.

## 6. Data model (what matters)
- Sales: `tblInvoice`/`tblInvoiceDetail` (depot) and `tblSubInvoiceMaster`/`tblSubInvoiceDetail` (sub-depot), `tblOrder`/`tblOrderDetail`, `tblOrderDel`/`tblOrderDetailDel` (deleted orders), `tblReturnInvoice*`, `tblCustPayDetail`/`tblCustomerPay`, `View_MIAWiseSalesReport`, `View_CustomerMaster`, `tblDCStore` (stock), `tblProduct`/`tblProductSQ` (brand).
- Market structure: `tbl_Group` > `tblRegion` (zone/DZSM) > `tblArea` > `tblTerritory` > `tblSubTerritory` > `tblMarket`; `tblMIAInfo`, `tblDistrict`, `tblCompanyUnit` (DC/branch).
- Logistics: `tblRequisition`, `tblRequsitionChild`, `tblStockInTransfar`, `tblCentralStore`, `tbl_INV_Central_BatchWiseStock`, `tbl_INV_CentralStore_WeightedAvg`.
- Logs: `tblInvoiceDeleteLog`, `tblInvoiceDetail_DeleterRecord`, `tblCustPayDetail_DeleteLog` (via SP).
- Accounting DB `CSTL_ACCDB_Unicorn` (`tblDebitCreditVoucherMaster/Detail`).
- SPs (bodies not in repo): ~75 distinct (`sp_ProductWiseBusinessSummaryMISReportByParam`, `sp_BusinessSummaryMISReport_All_New_New`, `_TT`, `sp_MiowiseMISReport`, `sp_RPT_MIOWiseBusinessSummary`, `sp_RPTMonitoringReport`, `sp_RPT_MIS_ProductWiseSalesReport`, `sp_GET_DZSMwise*`, `sp_Get_COGSBasedGPReport`, `sp_Get_TerritorryWiseSalesReportList`, `GetDynamicSalesReportList`, `sp_GET_MonthwiseSale`, `sp_GET_DatewiseSale`, `sp_InvoceLifecycle`, 45 dashboard `sp_Get_Dashboard*`...). Only `sp_Get_DoctorList` body exists (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:883) and it concatenates `@Parm` into dynamic SQL.

## 7. Integrations
Cross-database writes to the accounting DB by the delivery/payment rollback (TableID 2014/2017); Excel/PDF export via Crystal Reports (`Solution.Web/Reports/CrystalReports`, `Library.CrystalReports/SInventory_RPT`), ClosedXML/EPPlus/OpenXML for XLSX; mobile app web-views. No SAP call from M13 pages.

## 8. Variant comparison - TotalSummary / BusinessSummary families
| Page (ID) | File date (INFERRED from file mtime) | In legacy static menu `MasterPages/MasterPage.master` | Other pages referencing it | Data call (BLL -> DAL) | SP / SQL | Result columns (grid) |
|---|---|---|---|---|---|---|
| TotalSummary (531) | 2022-04-24 | no | none | TotalSummaryBLL.LoadSummaryBLL | inline SQL `TotalSummaryDAL.LoadSummaryDAL` (per-ComUnit) | ComUnitCode, ShortName, Proforma, Sold, Return counts/amounts, VAT |
| BusinessSummary (494) | 2021-12-15 | no | none | LoadSummaryBLL | same inline SQL | ComUnitCode/Name, Proforma, Sold, Return |
| BranchTotalSummary (493) | 2021-12-15 | yes ("Branch Wise Business Summary", line 1390) | none | BranchwiseLoadSummaryBLL(from,to,Branch) | inline SQL (same as LoadSummaryDAL + `WHERE C.ComUnitId=@Branch`) | + 4 Excel buttons -> BusinessSummaryViewer2..5 |
| DZSMTotalSummary (506) | 2022-10-10 | yes ("DZSM Wise Business Summary", line 1391) | TerritoryWiseSalesReport (redirect) | TotalSummaryDAL.DZSMwiseLoadSummaryparm | sp_GET_DZSMwiseReportParam_ByProcess | Proforma, Sold, Return, CustomerCoverPer, Fixed/Camp/Final sales, Blue/Green net sell & coverage (27 cols) |
| TerritoryWiseSalesReport (530) | - | no | none | DZSMwiseLoadSummaryBLL / _vvvv | sp_GET_DZSMwiseReport(+Param_ByProcess_vv/_Area/_Terri), sp_GET_DZSMwiseNAtionalReport, sp_GET_DZSMProcessDate | DZSM/territory-level |
| Copy of TotalSummaryNew (498) | 2021-12-15 | no | none | LoadSummary2BLL + LoadSummaryzone*BLL (3 branches) | sp_ProductWiseBusinessSummaryMISReport + inline zone/branch SQL | ProductCode, Proforma, Return, Sales, Discount, Collection (18 cols) - oldest product-wise copy |
| TotalSummaryProductWise (536) | 2021-12-15 | no | none | LoadSummary2BLL | sp_ProductWiseBusinessSummaryMISReport | ComUnitCode, ProductCode, Invoice/Return/Sales/Outstanding1-3 |
| **TotalSummaryNew (532)** | **2025-10-05 (newest)** | **yes ("Product wise Business Summary", line 1333)** | TotalSummaryNew_PT | LoadSummaryProductcodewise (Type,Zone,Area,Terr) | sp_ProductWiseBusinessSummaryMISReportByParam | product + zone/area/territory blocks: Invoice, Rejection, Sales, Return, JustSales, Collection, Receivable |
| TotalSummaryNew_PT (534) | 2023-10-05 | no | none | LoadBusinessSummaryProductwise | sp_Rpt_BusinessSummaryProductwise (+ legacy zone/branch inline calls) | 19 product-wise columns (Proforma, Return, Sales, Discount, Collection) |
| **TotalSummaryNew2 (533)** | 2023-08-06 | **yes ("Business Summary", line 1334)** | MIOWiseBusinessSummary, MonitoringReport, TotalSummaryNew_T (text refs) | LoadSummaryProductcodewiseGyash (6 args) | sp_BusinessSummaryMISReport_All_New_New | DC/Zone/Area/Territory: Invoice, Return, Sales (salesTP/SalesVat/SalesTotal), Collection (NetSalesAmt), Outstanding1-3 |
| TotalSummaryNew_T (535) | 2023-10-10 | no | none | LoadSummaryProductcodewiseGyash__New | sp_BusinessSummaryMISReport_TT | as New2 but "Sales & Collection" combined header, receivable = SumofRCVTP/Vat/Amount, no salesTP/SalesTotal |
| MIOWiseBusinessSummary (517) | 2023-10-04 | no | none | LoadMIOWiseBusinessSummaryDAL(Depid) | sp_RPT_MIOWiseBusinessSummary | as New2/_T layout, MIO rows, receivable SumofRCV* |
| MIOWiseTotalSummaryNew (518) | 2022-05-10 | yes ("MIO wise Business Summary", line 1335) | MIOWiseTotalSummaryNew_T | MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash(unit) | sp_MiowiseMISReport(@fromDate,@toDate,@unitid) | ComUnitCode, MiaName, Invoice/Return/Sales/Outstanding |
| MIOWiseTotalSummaryNew_T (519) | 2023-10-04 | no | none | identical | identical | **byte-identical code except class name** (2-line diff) |
| CustomerWiseSale (499) | 2021-12-15 | no | none | TotalSummaryBLL.CustomerWiseSale (C# pivot over GetmonthlycustomerSaleDate) | DB function + sp_BusinessSummaryMISReport(_All_New_New) | customer x month pivot |
| MonitoringReport (520) | - | no | none | rptMonitoringReport(Type) | sp_RPTMonitoringReport | TotalSummary block + period roll-forward, discounts, bank deposit, cash in hand |
| ProductWiseSalesReport (523) | - | no | none | ProductWiseSalesReportDAL | sp_RPT_MIS_ProductWiseSalesReport | product sales/return/just-sales/collection/receivable |
| FinalsalesReport (509) / _T (510) | - | yes (FinalsalesReport, line 1336) | _T references FinalsalesReport | FinalSales, FinalSales2 | sp_GetFinalSales, sp_NumberofInvoiceandCust | **_T is identical except class name** |

Which is "current"? Menu evidence (static legacy master + newest mtime) points to TotalSummaryNew (product-wise), TotalSummaryNew2 (business summary) and MIOWiseTotalSummaryNew as the live trio; the live DB-driven menu is `UNKNOWN`. `_T`, `_PT`, `Copy of`, `2021-12-15` files are variants/experiments (INFERRED). All variants exist in `PrecompiledWeb/Solution.Web/SInventory_UI` (a compiled snapshot), so all are deployable.

## 9. Reports (see `reports/`)
- [RPT-M13-01-business-summary-mis-family](reports/RPT-M13-01-business-summary-mis-family.md) - Business Summary / MIS on-screen report family
- [RPT-M13-02-business-summary-excel-exports](reports/RPT-M13-02-business-summary-excel-exports.md) - Business Summary and Branch/Market Excel exports (viewers 1-5)
- [RPT-M13-03-sales-detail-exports](reports/RPT-M13-03-sales-detail-exports.md) - Sales detail Excel exports (All/Final/MIO-product/Proforma-and-sales)
- [RPT-M13-04-entity-sales-reports](reports/RPT-M13-04-entity-sales-reports.md) - Company / DC / Region / Zone / District / MIA / Customer sales reports
- [RPT-M13-05-trend-comparison-periodic](reports/RPT-M13-05-trend-comparison-periodic.md) - Trend, comparison, day-wise, national product-wise, historical
- [RPT-M13-06-gp-cogs-reports](reports/RPT-M13-06-gp-cogs-reports.md) - GP / COGS reports
- [RPT-M13-07-dynamic-sales-report](reports/RPT-M13-07-dynamic-sales-report.md) - Dynamic sales report
- [RPT-M13-08-operational-audit-reports](reports/RPT-M13-08-operational-audit-reports.md) - Operational and audit reports

## 10. Workflows (see `workflows/`)
- [WF-M13-01-sales-operation-rollback](workflows/WF-M13-01-sales-operation-rollback.md) - Sales operation rollback (proforma / delivery / payment)
- [WF-M13-02-logistics-requisition-rollback](workflows/WF-M13-02-logistics-requisition-rollback.md) - Logistics requisition rollback (STO / picking / challan)
- [WF-M13-03-business-summary-mis-reporting](workflows/WF-M13-03-business-summary-mis-reporting.md) - Business Summary MIS reporting (role-scoped, SP-driven)
- [WF-M13-04-admin-dashboard-data-load](workflows/WF-M13-04-admin-dashboard-data-load.md) - Admin dashboard data load and endpoint catalogue
- [WF-M13-05-report-viewer-launch](workflows/WF-M13-05-report-viewer-launch.md) - List page -> popup viewer -> Crystal export
- [WF-M13-06-dynamic-sales-report-drilldown](workflows/WF-M13-06-dynamic-sales-report-drilldown.md) - Dynamic sales report grid and drill-down modals

## 11. AdminDashboard endpoints
The 28 `[WebMethod]`s (parameters, ChartDAL method, SP, result shape) are tabulated in `pages/PAGE-464-admindashboard.md` section 13 and summarised in `workflows/WF-M13-04-admin-dashboard-data-load.md`.

## 12. Known gaps (summary) - full list in GAPS-M13.md
- All KPI/GP/COGS arithmetic except the inline TotalSummary query lives in SPs that are not in the repository.
- SQL injection surfaces: concatenated predicates (COGS, GP, DynamicSalesReport, MIOProduct, Audit, dashboard `param`), District DAL, `sp_Get_DoctorList` dynamic SQL.
- Data-modifying rollback pages without role checks; logistics rollback non-transactional.
- Anonymous-reachable viewers, SixTen page (identity from QueryString) and dashboard WebMethods.
- Many duplicate/legacy pages; several defects (HistoricalReport to-date, missing `crpSalesTrendExcelReport.rpt`, dead `MiaSalesReportViewer` link, inverted Excel checks).

## 13. Still unverified
SP internals (all sales/GP/COGS/collection/receivable formulas outside the inline SQL), View_MIAWiseSalesReport definition, DB-driven menu/role mapping, Crystal `.rpt` internals (formula fields/groupings - binary files not opened), client-side JS of AdminDashboard chart routines beyond the AJAX layer, and Tier-B pages (61 of 75) that were only mechanically extracted.
