---
page_id: PAGE-464
module: M13
page: AdminDashboard
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-464 — AdminDashboard

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-464 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | AdminDashboard |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Dashboard_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Dashboard_UI_AdminDashboard |
| aspx size | 202586 bytes |
| Code-behind LOC | 1418 |
| Inline script size | 132652 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**AdminDashboard** is the landing dashboard shown after login (`Login.aspx.cs:96` redirects here when `IsMainDashboard`). It renders KPI tiles (orders, invoices, delivery, payment, rejection, DCR, Rx, attendance, customer coverage, leave) and Chart.js/Highcharts-style charts (sales/collection by month, order by month, returns, customer coverage, GMP/non-GMP visits and Rx, brand/product order, attendance, expense/DA, depot/zone-wise order and invoice). All numbers come from 28 AJAX `[WebMethod]`s (catalogue in §13).

## 3. Business Objective
Give management a role-scoped (national / NSM group / DZSM zone) view of sales performance and field-force activity without opening individual reports. INFERRED from tile/chart names; role scoping is CONFIRMED (`ChartDAL.GetTotalInfoByCurrentDateDAL`).

## 4. Navigation
Landing target from `Login.aspx.cs` (CONFIRMED). Non-permitted users are redirected to `DashboardOne.aspx` (an empty page with the master page, `Dashboard_UI/DashboardOne.aspx`, 14 LOC code-behind, referenced by many approval pages as a fallback). Menu placement is DB-driven (`tblMainMenu`/`sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`), rows not in repository -> exact menu path `UNKNOWN`.

## 5. Route
`~/Dashboard_UI/AdminDashboard.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Dashboard_UI/AdminDashboard.aspx |
| Code-behind | Solution.Web/Dashboard_UI/AdminDashboard.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | ../VerticalAsset/plugins/vectormap/jquery-jvectormap-2.0.2.min.js |
| JS include | ../VerticalAsset/plugins/vectormap/jquery-jvectormap-world-mill-en.js |
| JS include | ../VerticalAsset/plugins/highcharts/js/highcharts.js |
| JS include | ../VerticalAsset/plugins/highcharts/js/exporting.js |
| JS include | ../VerticalAsset/plugins/highcharts/js/variable-pie.js |
| JS include | ../VerticalAsset/plugins/highcharts/js/export-data.js |
| JS include | ../VerticalAsset/plugins/highcharts/js/accessibility.js |
| JS include | ../VerticalAsset/plugins/apexcharts-bundle/js/apexcharts.min.js |
| JS include | ../VerticalAsset/js/index.js |
| CSS include | ../VerticalAsset/plugins/highcharts/css/highcharts.css |
| CSS include | ../VerticalAsset/plugins/vectormap/jquery-jvectormap-2.0.2.css |
| DAL class | ChartDAL (Library.DAL/ChartDAL/ChartDAL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAO/entity class | TotalInfoDAO (Library.DAO/MasterSetup_DAO/TotalInfoDAO.cs) |

## 7. UI Control Inventory
The page is 5 204 lines of markup: master-page content + inline jQuery/Ajax + chart code (no server controls of interest; `Page_Load` only runs the permission check). Generated inventory:

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| TodayOrderDepot | input | Depot | no validator found |  |  |  |  |
| TodayInvoiceDepot | input | Depot | no validator found |  |  |  |  |
| GroupNameSelect | select |  | no validator found |  |  |  |  |
| zoneSelect | select |  | no validator found |  |  |  |  |
| areaSelect | select |  | no validator found |  |  |  |  |
| territorySelect | select |  | no validator found |  |  |  |  |
| SubTerritory | select |  | no validator found |  |  |  |  |
| MarketSelect | select |  | no validator found |  |  |  |  |
| OrderMonth | select |  | no validator found |  |  |  |  |
| OrderYear | select |  | no validator found |  |  |  |  |
| CustomerTypeSelect | select |  | no validator found |  |  |  |  |
| OrderDepot | input | Depot | no validator found |  |  |  |  |
| orderfromdate | input |  | no validator found |  |  |  |  |
| ordertodate | input |  | no validator found |  |  |  |  |
| btnSaveorder | button |  | no validator found |  |  |  | onclick→GetData(2) |
| SalesMonthNew | select |  | no validator found |  |  |  |  |
| SalesYearNew | select |  | no validator found |  |  |  |  |
| SalesCustomerTypeSelect | select |  | no validator found |  |  |  |  |
| salesfromdate | input |  | no validator found |  |  |  | onload→getDate() |
| salestodate | input |  | no validator found |  |  |  |  |
| SalesCheckInvoice | input | Invoice | no validator found |  |  |  |  |
| SalesWiseCheckPayment | input | Payment | no validator found |  |  |  |  |
| btnSavesales | button |  | no validator found |  |  |  | onclick→GetData(1) |
| returnfromdate | input |  | no validator found |  |  |  |  |
| returntodate | input |  | no validator found |  |  |  |  |
| btnSavereturn | button |  | no validator found |  |  |  | onclick→GetData(3) |
| brandMonthSelect | select |  | no validator found |  |  |  |  |
| brandYear | select |  | no validator found |  |  |  |  |
| brandNameSelect | select |  | no validator found |  |  |  |  |
| BrandWiseCheckInvoice | input | Invoice | no validator found |  |  |  |  |
| BrandWiseCheckPayment | input | Payment | no validator found |  |  |  |  |
| chkDayWsie | input | DayWise | no validator found |  |  |  |  |
| chkBrandWise | input | BrandWise | no validator found |  |  |  |  |
| chkProductWise | input | ProductWise | no validator found |  |  |  |  |
| brandwisefromdate | input |  | no validator found |  |  |  |  |
| brandwisetodate | input |  | no validator found |  |  |  |  |
| btnSavebrandwise | button |  | no validator found |  |  |  | onclick→LoadBrandWiseOrder() |
| CovMonthNew | select |  | no validator found |  |  |  |  |
| CovYearNew | select |  | no validator found |  |  |  |  |
| CovCustomerTypeSelect | select |  | no validator found |  |  |  |  |
| CovCheckInvoice | input | Invoice | no validator found |  |  |  |  |
| CovWiseCheckPayment | input | Payment | no validator found |  |  |  |  |
| customerfromdate | input |  | no validator found |  |  |  |  |
| customertodate | input |  | no validator found |  |  |  |  |
| btnSavecustomer | button |  | no validator found |  |  |  | onclick→GetData(4) |
| VRMonth | select |  | no validator found |  |  |  |  |
| VRYear | select |  | no validator found |  |  |  |  |
| VRApprovalStatusSelect | select |  | no validator found |  |  |  |  |
| VRDayWiseCheck | input | Day | no validator found |  |  |  |  |
| gmpfromdate | input |  | no validator found |  |  |  |  |
| gmptodate | input |  | no validator found |  |  |  |  |
| btnSavegmp | button |  | no validator found |  |  |  | onclick→LoadVRChart() |
| nongmpfromdate | input |  | no validator found |  |  |  |  |
| nongmptodate | input |  | no validator found |  |  |  |  |
| btnSavenongmp | button |  | no validator found |  |  |  | onclick→GetData(6) |
| RxMonth | select |  | no validator found |  |  |  |  |
| RxYear | select |  | no validator found |  |  |  |  |
| RxApprovalStatusSelect | select |  | no validator found |  |  |  |  |
| RXGMPCheck | input | GMP | no validator found |  |  |  |  |
| RXNONGMPCheck | input | NONGMP | no validator found |  |  |  |  |
| RXDayWiseCheck | input | Day | no validator found |  |  |  |  |
| RXZoneWiseCheck | input | Zone | no validator found |  |  |  |  |
| gmprxfromdate | input |  | no validator found |  |  |  |  |
| gmprxtodate | input |  | no validator found |  |  |  |  |
| btnSavegmprx | button |  | no validator found |  |  |  | onclick→LoadRXChart() |
| nongmprxfromdate | input |  | no validator found |  |  |  |  |
| nongmprxtodate | input |  | no validator found |  |  |  |  |
| btnSavenongmprx | button |  | no validator found |  |  |  | onclick→GetData(8) |
| AttMonth | select |  | no validator found |  |  |  |  |
| AttYear | select |  | no validator found |  |  |  |  |
| AttInCheck | input | AttIN | no validator found |  |  |  |  |
| AttOutCheck | input | AttOut | no validator found |  |  |  |  |
| AttApprovalStatusSelect | select |  | no validator found |  |  |  |  |
| btnSaveatt | button |  | no validator found |  |  |  | onclick→LoadAtt() |
| attfromdate | input |  | no validator found |  |  |  |  |
| atttodate | input |  | no validator found |  |  |  |  |
| MonthSelect | select |  | no validator found |  |  |  |  |
| FiscalYearSelect | select |  | no validator found |  |  |  |  |
| ExpenseTypeIdSelect | select |  | no validator found |  |  |  |  |
| ApprovalStatusSelect | select |  | no validator found |  |  |  |  |
| ExpenseCheck | input | Expense | no validator found |  |  |  |  |
| DACheck | input | DA | no validator found |  |  |  |  |
| btnSavetotalexp | button |  | no validator found |  |  |  | onclick→LoadExense() |
| totalexpfromdate | input |  | no validator found |  |  |  |  |
| totalexptodate | input |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified**: filter widgets (`SalesCustomerTypeSelect`, `GroupNameSelect`, `zoneSelect`, `areaSelect`, `territorySelect`, `SubTerritory`, `MarketSelect`, `brandNameSelect`, month/year selectors, `SalesdWiseCheck` Invoice/Payment radio, date pickers) are plain HTML/JS controls populated by JS; their option lists come from other pages' WebMethods (e.g. `../DoctorModule_UI/SeedData.aspx/GetBrandNameALL`). Tiles updated by `GetTotalData(1)`: TotalOrder, TotalInvoice, totalDelivery, totalPayment, TotalRejection, TotalOrderPer, TotalInvoicerPer, totalDCR/TotalDcrType, TotalRX/TotalRXType, totalCustomerCoverage, TotalAttandence, TotalLeave.

## 8. Field Mapping
No form fields are submitted to the server as a postback. JS fields map to WebMethod args: month/year selectors -> `SSMonthNew`/`SSYearNew`; Invoice/Payment radio -> `fromdt`; customer type -> `todt`; market-structure selects -> concatenated `param` SQL fragment (BR-M13-019); date pickers -> `fromdt`/`todt`.

## 9. Dropdown Mapping
Dropdown data is fetched client-side via WebMethods of other pages (brand list from `SeedData.aspx/GetBrandNameALL`; market structure/customer types via `GetCustomerTypeAllInfo` and helper functions in the same script). Detailed loaders other than brand: not traced (UNKNOWN).

## 10. Button Action Mapping
Buttons/links invoke JS `GetData(type)` (type 1 sales, 2 order, 4 customer coverage ... commented-out types 3,5,8-11), `LoadBrandWiseOrder()`, `LoadAtt()`, `LoadRXChart()`, `LoadVRChart()`, `LoadExense()`, `LoadDeptoWiseInvoice(param)`, `LoadDeptoWiseOrder(param)`.

## 11. Frontend Flow
`$(document).ready` (aspx:1461) calls `GetTotalData(1)` and the 10 top-bar chart loaders (`LoadTotalOrderchart` ... `LoadTotalInvoicechart`) plus brand-wise order; each loader does `$.ajax POST AdminDashboard.aspx/<method>` with `contentType application/json`, reads `data.d`, `JSON.parse` for string results and feeds the chart routine (`SalesChartAdd`, `OrderChartAdd`, `ReturnChartAdd`, ...). The `GetData(type)` function builds `param` by appending " AND tblOrder.<Col>=<value>" for each filled selector (BR-M13-019) and calls the matching endpoint. No calculations besides chart formatting.

## 12. Backend Flow
`Page_Load` (first load): `UserPersmissionValidation()` in try/catch -> BR-M13-015. WebMethods are `public static`, use one shared static `ChartDAL` instance (`aChartDal`) and serialize DataTables with `JsonConvert.SerializeObject`. Multi-month endpoints (`GetSalesChartData`, `GetOrderChartData`, `GetCustomerReportChartData`, `GetSalesRetrunChartData`, `GetGMP*ChartData`) loop month by month, call the SP once per month, pivot rows by `Criteria` into columns `MMM-yyyy` and add `Total` (BR-M13-017). Errors: `ChartDAL` re-throws; the WebMethod does not catch (client receives HTTP 500).

## 13. API Mapping
**28 `[WebMethod]` endpoints** in `Solution.Web/Dashboard_UI/AdminDashboard.aspx.cs` (all `public static`, POST/JSON, no `[ScriptMethod]` attribute needed because ScriptManager page methods are used by jQuery `contentType: application/json`). None checks the session; the Forms cookie is the only gate (see GAP-M13-005).

| # | WebMethod (`AdminDashboard.aspx/<name>`) | Parameters | ChartDAL method | SP(s) called | Result shape |
|---|---|---|---|---|---|
| 1 | Get_DeptoWiseOrder | param | ChartDAL.GetDeptoWiseOrderDAL | param=="Depot" ? sp_Get_DashboardOrder_DeptoWise : sp_Get_DashboardDeptoWiseOrder (no args) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 2 | Get_DeptoWiseInvoice | param | Get_DeptoWiseInvoiceDAL | param=="Depot" ? sp_Get_DashboardInvoice_DeptoWise : sp_Get_DashboardDeptoWiseInvoice | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 3 | Get_TopBarChartOrder | param (ignored) | Get_TopBarChartOrderDAL | sp_Get_DashboardTopBarChartOrder | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 4 | Get_TopBarChartDeliveryAmount | param (ignored) | Get_TopBarChartDeliveryAmountDAL | sp_Get_DashboardTopBarChartDeliveryAmount | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 5 | Get_TopBarChartRejectionAmount | param (ignored) | Get_TopBarChartRejectionAmountDAL | sp_Get_DashboardTopBarChartRejectionAmount | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 6 | Get_TopBarChartDCR | param (ignored) | Get_TopBarChartDCRDAL | sp_Get_DashboardTopBarChartTotalDCR | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 7 | Get_TopBarChartTotalRX | param (ignored) | Get_TopBarChartTotalRXDAL | sp_Get_DashboardTopBarChartTotalRX | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 8 | Get_TopBarChartTotalAttandence | param (ignored) | Get_TopBarChartTotalAttandenceDAL | sp_Get_DashboardTopBarChartTotalAttandence | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 9 | Get_TopBarChartCustomerCoverage | param (ignored) | Get_TopBarChartCustomerCoverageDAL | sp_Get_DashboardTopBarChartCustomerCoverage | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 10 | Get_TopBarChartTotalLeave | param (ignored) | Get_TopBarChartTotalLeaveDAL | sp_Get_DashboardTopBarChartTotalLeave | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 11 | Get_TopBarChartOrderCount | param (ignored) | Get_TopBarChartOrderCountDAL | sp_Get_DashboardTopBarChartOrderCount | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 12 | Get_TopBarChartTotalInvoice | param (ignored) | Get_TopBarChartTotalInvoiceDAL | sp_Get_DashboardTopBarChartTotalInvoiceAmount | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 13 | GetTotalInfoByCurrentDate (returns TotalInfoDAO, not JSON string) | int id (ignored) | GetTotalInfoByCurrentDateDAL | role-scoped: sp_HigharchyInfoByEmployeeId then sp_Get_DashboardTopBarData_Zone / _NSM / sp_Get_DashboardTopBarData (BR-M13-016) | TotalInfoDAO JSON object (14 string props) |
| 14 | GetBrandWiseOrderReport | fromdt, todt (dates), param | GetBrandWiseOrderReportDAL | sp_Get_BrandWiseOrderDashboard_new(@FrmDate,@ToDate,@param) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 15 | GetBrandWiseOrderReportDayWise | fromdt ("Invoice"/"Payment"), todt (date), param, Brand | GetBrandWiseOrderReportDayWiseDAL | Invoice: DayWise sp_Get_BrandWiseOrderDashboard_new / BrandWise sp_Get_BrandWiseOrderDashboard / ProductWise sp_Get_ProductWiseOrderDashboard; Payment: sp_Get_PaymentBrandWiseOrderDashboard / sp_Get_BrandWiseOrderPaymentDashboard / sp_Get_ProductWiseOrderPaymentDashboard | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 16 | GetAttandenceMonthlyReport | fromdt (=month), todt (=year), param | GetAttandenceMonthlyReportDAL | sp_Get_AttandenceMonthlyDashboard_new(@Month,@Year,@param) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 17 | GetGMPRXReportChartDataDayWise | fromdt ("Day"/"Zone"), todt, param | GetGMPRXReportChartDataDayWiseDAL | Day: sp_Get_DoctorGMPxRecordMonthlyDashboardDayWise; Zone: ..ZoneWise (@FrmDate,@ToDate,@param) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 18 | GetGMPVisitReportChartDataDayWise | fromdt ("Day"/"Zone"), todt, param | GetGMPVisitReportChartDataDayWiseDAL | Day: sp_Get_DoctorGMPRecordMonthlyDashboardDayWise; Zone: ..ZoneWise | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 19 | GetSalesChartData | fromdt ("Invoice"/"Payment"), todt (customer-type id), param, SSMonthNew ("m1,m2,.."), SSYearNew | GetPerMonthAmount per month | Invoice: sp_Get_SalesRecordMonthlyDayWsie_New; Payment: sp_Get_SalesRecordMonthlyDayWsiePayment (@Month,@Year,@param,@CustomerTypeId); pivot Criteria x month; Total = sum/#months (BR-M13-017) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 20 | GetSalesRetrunChartData | fromdt, todt (dates), param | GetSalesRetrunChartData per month | sp_Get_SalesReturnRecordMonthly_New(@Month,@Year,@param); Total = sum | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 21 | GetOrderChartData | fromdt, todt ("Zone"/"Depot"), param, SSMonthNew, SSYearNew | GetOrderChartDataDAL per month | Zone: sp_Get_OrderRecordMonthlyDashboard_New; Depot: sp_Get_OrderRecordMonthlyDashboard_Depo; Total = sum/#months | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 22 | GetCustomerReportChartData | fromdt ("Invoice"/"Payment" used as Type via CustomerTypeId arg order - see note), todt, param, SSMonthNew, SSYearNew | GetCustomerReportChartDataDAL per month | Invoice: sp_Get_CustomerCoverageRecordMonthlyDashboard_New; Payment: sp_Get_CustomerCoverageRecordPaymentDashboard_New; Total = sum | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 23 | GetGMPVisitReportChartData | fromdt, todt (dates), param | GetGMPVisitReportChartDataDAL | sp_Get_DoctorGMPRecordMonthlyDashboard_New(@FrmDate,@ToDate,@param); Total = sum | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 24 | GetNONGMPVisitReportChartData | fromdt, todt | GetNONGMPVisitReportChartDataDAL(month,year) | sp_Get_DoctorNGMPRecordMonthlyDashboard(@Month,@Year) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 25 | GetGMPRXReportChartData | fromdt, todt, param | GetGMPRXReportChartDataDAL | sp_Get_DoctorGMPxRecordMonthlyDashboard_new | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 26 | GetNONGMPRXReportChartData | fromdt, todt | GetNONGMPRXReportChartDataDAL | sp_Get_DoctorNGMPxRecordMonthlyDashboard(@Month,@Year) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 27 | GetExpanseClaimMonthlyChartData | fromdt (=month), todt (=year), param | GetExpanseClaimMonthlyChartDataDAL | sp_Get_ExpanseClaimMonthlyDashboard(@Month,@Year,@param) | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |
| 28 | GetExpanseClaimMonthlyChartDataDayWise | fromdt ("Expense"/"DA" as month arg), todt (year), param | GetExpanseClaimMonthlyChartDataDayWiseDAL | Expense: sp_Get_ExpanseClaimMonthlyDashboard_DayWise; DA: sp_Get_DAMonthlyDashboard_DayWise | JSON string of a DataTable: array of row objects `{col: value}` (columns defined by the SP; pivoted month columns for the multi-month charts); read on the client as `JSON.parse(response.d)` |

Notes: (1) the "TopBarChart*" endpoints ignore their `param` argument and return an SP-defined series (likely last N days - SP bodies UNKNOWN). (2) `GetCustomerReportChartData` passes `(Month, Year, param, CustomerTypeId=fromdt, Type=todt)` to `ChartDAL.GetCustomerReportChartDataDAL(...)` whose parameter order is (Month, Year, param, CustomerTypeId, Type) - the WebMethod call is `GetCustomerReportChartDataDAL(month, year, param, fromdt, todt)`, so `fromdt` lands in `CustomerTypeId` and `todt` in `Type`, opposite to `GetSalesChartData` where `fromdt` is the type selector. Whether the JS supplies them in that order was not traced (INFERRED defect risk). (3) `GetGMPRXReportChartDataDayWise` and `GetExpanseClaimMonthlyChartData` also exist commented-out at lines 1156/1341.

## 14. Service Mapping
No BLL layer: the code-behind calls `Library.DAL.ChartDAL.ChartDAL` directly (`CommonDataLoad` for the permission lookup).

## 15. Repository Mapping
`ChartDAL` (Library.DAL/ChartDAL/ChartDAL.cs): every method opens `DataAccessManager` on `DataBase.SalesDB`, executes an SP, closes in `finally`. `GetTotalInfoByCurrentDateDAL` reads `HttpContext.Current.Session["RoleTypeName"/"EmpInfoId"/"RoleTypeId"]` (try/catch: missing session silently means national scope).

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_MainPermissionByUserRoleandPageUrl | CommonDataLoad.GetPermissionForUserRole | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AttandenceMonthlyDashboard_new | ChartDAL.GetAttandenceMonthlyReportDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_BrandWiseOrderDashboard | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_BrandWiseOrderDashboard_new | ChartDAL.GetBrandWiseOrderReportDAL, ChartDAL.GetBrandWiseOrderReportDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_BrandWiseOrderPaymentDashboard | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CustomerCoverageRecordMonthlyDashboard_New | ChartDAL.GetCustomerReportChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CustomerCoverageRecordPaymentDashboard_New | ChartDAL.GetCustomerReportChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DAMonthlyDashboard_DayWise | ChartDAL.GetExpanseClaimMonthlyChartDataDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardDeptoWiseInvoice | ChartDAL.Get_DeptoWiseInvoiceDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardDeptoWiseOrder | ChartDAL.GetDeptoWiseOrderDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardInvoice_DeptoWise | ChartDAL.Get_DeptoWiseInvoiceDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardOrder_DeptoWise | ChartDAL.GetDeptoWiseOrderDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartCustomerCoverage | ChartDAL.Get_TopBarChartCustomerCoverageDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartDeliveryAmount | ChartDAL.Get_TopBarChartDeliveryAmountDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartOrder | ChartDAL.Get_TopBarChartOrderDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartOrderCount | ChartDAL.Get_TopBarChartOrderCountDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartRejectionAmount | ChartDAL.Get_TopBarChartRejectionAmountDAL | body NOT in repository | write-like name |
| sp_Get_DashboardTopBarChartTotalAttandence | ChartDAL.Get_TopBarChartTotalAttandenceDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartTotalDCR | ChartDAL.Get_TopBarChartDCRDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartTotalInvoiceAmount | ChartDAL.Get_TopBarChartTotalInvoiceDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartTotalLeave | ChartDAL.Get_TopBarChartTotalLeaveDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarChartTotalRX | ChartDAL.Get_TopBarChartTotalRXDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarData | ChartDAL.GetTotalInfoByCurrentDateDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarData_NSM | ChartDAL.GetTotalInfoByCurrentDateDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DashboardTopBarData_Zone | ChartDAL.GetTotalInfoByCurrentDateDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorGMPRecordMonthlyDashboardDayWise | ChartDAL.GetGMPVisitReportChartDataDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorGMPRecordMonthlyDashboardZoneWise | ChartDAL.GetGMPVisitReportChartDataDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorGMPRecordMonthlyDashboard_New | ChartDAL.GetGMPVisitReportChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorGMPxRecordMonthlyDashboardDayWise | ChartDAL.GetGMPRXReportChartDataDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorGMPxRecordMonthlyDashboardZoneWise | ChartDAL.GetGMPRXReportChartDataDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorGMPxRecordMonthlyDashboard_new | ChartDAL.GetGMPRXReportChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorNGMPRecordMonthlyDashboard | ChartDAL.GetNONGMPVisitReportChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorNGMPxRecordMonthlyDashboard | ChartDAL.GetNONGMPRXReportChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpanseClaimMonthlyDashboard | ChartDAL.GetExpanseClaimMonthlyChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpanseClaimMonthlyDashboard_DayWise | ChartDAL.GetExpanseClaimMonthlyChartDataDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_OrderRecordMonthlyDashboard_Depo | ChartDAL.GetOrderChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_OrderRecordMonthlyDashboard_New | ChartDAL.GetOrderChartDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_PaymentBrandWiseOrderDashboard | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ProductWiseOrderDashboard | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ProductWiseOrderPaymentDashboard | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SalesRecordMonthlyDayWsiePayment | ChartDAL.GetPerMonthAmount | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SalesRecordMonthlyDayWsie_New | ChartDAL.GetPerMonthAmount | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SalesReturnRecordMonthly_New | ChartDAL.GetSalesRetrunChartData | body NOT in repository | read-like name (INFERRED) |
| sp_HigharchyInfoByEmployeeId | ChartDAL.GetTotalInfoByCurrentDateDAL | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
No inline SQL. Tables touched by the 45 SPs are UNKNOWN (SP bodies absent). Table names visible in the client-built `param` predicates: `tblOrder` (CustTypeId, GroupId, RegionId, AreaId, TerritoryId, SubterritoryId, MarketId) and `mas.ExpenseDate` (commented). Everything on this page is READ-ONLY by construction (no write SP; SP names are read-like - INFERRED; `sp_Get_DashboardTopBarChartRejectionAmount` was flagged "write-like" only because of the word "Rejection").

## 18. Validation Matrix
No validation: JSON arguments are not checked; `Convert.ToDateTime`/`Convert.ToInt32` failures raise exceptions (HTTP 500). Selector strings ("Invoice", "Zone", ...) that do not match produce empty tables (BR-M13-018).

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-015 | Dashboard access: UserRoleID "2" is admitted without a permission lookup; any other role must return >=1 row from GetPermissionForUserRole(<page path>) (sp_GET_MainPermissionByUserRoleandPageUrl) else it is redirected to DashboardOne.aspx; a missing session key redirects to Login.aspx. WebMethods (static) perform no such check. | `Solution.Web/Dashboard_UI/AdminDashboard.aspx.cs:37` | AdminDashboard.aspx.cs UserPersmissionValidation | CONFIRMED |
| BR-M13-016 | KPI header scope by RoleTypeName (Session): DZSM -> hierarchy RegionIds (sp_HigharchyInfoByEmployeeId) joined by comma -> sp_Get_DashboardTopBarData_Zone(@ZoneId); NSM -> GroupIds -> sp_Get_DashboardTopBarData_NSM(@GroupId); every other role (and a missing session) -> unscoped sp_Get_DashboardTopBarData. The 14 result columns are copied 1:1 into TotalInfoDAO. | `Library.DAL/ChartDAL/ChartDAL.cs:40` | ChartDAL.GetTotalInfoByCurrentDateDAL | CONFIRMED |
| BR-M13-017 | GetSalesChartData and GetOrderChartData build one column per selected month (MMM-yyyy) and a "Total" column that is the SUM of the month values DIVIDED by the number of selected months (i.e. a monthly average); GetSalesRetrunChartData and GetCustomerReportChartData use a plain SUM. | `Solution.Web/Dashboard_UI/AdminDashboard.aspx.cs:474` | AdminDashboard.aspx.cs GetSalesChartData / GetOrderChartData | CONFIRMED |
| BR-M13-018 | Several endpoints re-use their date parameters as selectors: for GetSalesChartData fromdt = "Invoice"\|"Payment" (SP choice) and todt = customer-type id; for GetOrderChartData todt = "Zone"\|"Depot"; for GetBrandWiseOrderReportDayWise fromdt = "Invoice"\|"Payment" and Brand = "DayWise"\|"BrandWise"\|"ProductWise"; GetGMP*DayWise fromdt = "Day"\|"Zone"; GetExpanseClaimMonthlyChartDataDayWise month = "Expense"\|"DA". An unknown selector returns an empty table (no SP is called). | `Library.DAL/ChartDAL/ChartDAL.cs:133` | ChartDAL.* | CONFIRMED |
| BR-M13-019 | Drill-down filters are assembled in browser JS as a SQL fragment (" AND tblOrder.CustTypeId=.. AND tblOrder.RegionId=.. ...") and posted as the "param" argument that ChartDAL passes unchanged to the SPs as @param; whether the SPs concatenate it into dynamic SQL is UNKNOWN (bodies absent) but the contract invites SQL injection. | `Solution.Web/Dashboard_UI/AdminDashboard.aspx:4105` | AdminDashboard.aspx GetData(); ChartDAL @param | INFERRED |
| BR-M13-020 | Month-loop start/end dates are built with new DateTime(year, month, DateTime.Now.Day); on days 29-31 this throws for shorter months, and the loop guard (Month<=Month && Year<=Year) does not iterate across a year boundary. | `Solution.Web/Dashboard_UI/AdminDashboard.aspx.cs:363` | AdminDashboard.aspx.cs GetSalesChartData / GetOrderChartData / GetCustomerReportChartData | CONFIRMED |

## 20. Permission Matrix
- Enforced: `UserPersmissionValidation` (BR-M13-015): role id `2` bypasses; others need a permission row for `../Dashboard_UI/AdminDashboard.aspx`; failure -> `DashboardOne.aspx`; missing `UserRoleID` -> `Login.aspx`.
- Data scoping by role (`RoleTypeName` DZSM/NSM) only for the KPI header (BR-M13-016); all chart endpoints take scope from the client `param`, i.e. NOT enforced server-side.
- The 28 WebMethods have no session/role check.

## 21. Status Lifecycle
No status lifecycle; read-only dashboard. Status semantics live inside the SPs (UNKNOWN).

## 22. Error Handling
Server: no try/catch in WebMethods; DAL `catch(Exception e){ throw; }`, `finally` closes the connection. Page_Load: nested try/catch redirects. Client: `error:` callbacks not analysed.

## 23. Audit
No audit. No logging.

## 24. Integration
None (Chart library JS from local `Chart` folder; no external service calls traced).

## 25. Reports
Not a report page; dashboards are documented in `workflows/WF-M13-04-admin-dashboard-data-load.md`.

## 26. Dependencies
ChartDAL, DataAccessManager, TotalInfoDAO (`Library.DAO/MasterSetup_DAO/TotalInfoDAO.cs`), CommonDataLoad, Newtonsoft.Json, jQuery, chart scripts, master page.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Login as role 2 | dashboard opens without permission row | BR-M13-015 |
| TC-02 | Login as non-2 role without permission row | redirect to DashboardOne.aspx | BR-M13-015 |
| TC-03 | DZSM user | header tiles from sp_Get_DashboardTopBarData_Zone with own RegionIds | BR-M13-016 |
| TC-04 | NSM user | ..._NSM with GroupIds | BR-M13-016 |
| TC-05 | Sales chart Jan-Mar, 3 selected months | Total column = sum/3 | BR-M13-017 |
| TC-06 | Sales return chart Jan-Mar | Total = sum | BR-M13-017 |
| TC-07 | Sales chart on the 31st, month Feb selected | exception (invalid date) | BR-M13-020 |
| TC-08 | POST GetTotalInfoByCurrentDate without session | returns national totals (gap) | GAP-M13-005 |
| TC-09 | Filter `param` containing a quote/SQL | verify SP behaviour (injection test) | BR-M13-019 |
| TC-10 | fromdt="Bogus" on GetSalesChartData | empty table | BR-M13-018 |

## 28. Reverse Engineering References
- `Solution.Web/Dashboard_UI/AdminDashboard.aspx`, `.aspx.cs`
- `Library.DAL/ChartDAL/ChartDAL.cs`
- `Library.DAO/MasterSetup_DAO/TotalInfoDAO.cs`
- `Solution.Web/Login.aspx.cs:96`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| Endpoints | none | 28 WebMethods catalogued (§13) | SPs absent | PARTIAL | §13 |
| Business rules | none | BR-M13-015..020 | n/a | PARTIAL | §19 |

## 30. Known Gaps
- 28 unauthenticated-by-code WebMethods (GAP-M13-005); `param` SQL fragment contract (GAP-M13-006).
- Month loops use today's day-of-month and break across years (BR-M13-020).
- Overloaded parameter meaning (BR-M13-018) makes the API easy to misuse and hard to test.
- 45 SP bodies not in repository - all KPI definitions UNKNOWN.
- Static shared `ChartDAL` instance holds one `DataAccessManager` connection field: concurrent requests can interleave `SqlConnectionOpen/Close` on the same instance (INFERRED thread-safety risk).
- 5 204-line page with duplicated chart code; commented-out endpoints.

## 31. Change Impact
- Module M13. `ChartDAL` is used by this page only (grep). `Login.aspx.cs` redirects here; `DashboardOne.aspx` is the shared fallback for approval pages.
- The SPs are shared with the mobile/field APIs? UNKNOWN.
