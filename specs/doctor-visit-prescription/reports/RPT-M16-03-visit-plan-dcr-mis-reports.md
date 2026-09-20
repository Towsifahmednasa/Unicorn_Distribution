# RPT-M16-03 — Visit plan / DCR / MIS attendance & DCR reports

All pages live in `Solution.Web/DoctorVisit_UI/`; they are JS-driven grids over WebMethods returning `JsonConvert.SerializeObject(DataTable)`. **No Crystal Reports**; exports are client-side (DataTables buttons / XLSX / CSV).

| Report | Page (ID, tier) | WebMethod → DAL → SP | Params | Filters / notes |
|---|---|---|---|---|
| Doctor Call Plan list | `DoctorVisit.aspx` (PAGE-662, A) | `GetDoctorVisitList(param)` → `DoctorVisitDAL.GetDoctorVisitList` → `sp_Get_DoctorTourPlanMasteList` | `@param` raw SQL fragment | year, month, created-date range, approval status, creator, role, employee, group/zone/area/territory/market; default current month; Excel via XLSX (`Doctor Call Plan List.xlsx`) |
| Call plan detail | `DoctorPlanDetailsView.aspx` (PAGE-661, B) | `GetDoctorPlanDetailsViewDatabyID(id)` → `sp_Get_DoctorPlanDetailsById` | `@id` | shows `DoctorName`, `TourPlanDate`, `Type_DV` |
| Doctor Visit (plan) pivot | `DoctorVisitReport.aspx` (PAGE-664, B) | `GetDynamicPivotDoctorWiseDoctorVisitPlan(param)` → `DynamicPivotDoctorWiseDoctorVisitPlan`; also `GetDoctorVisitList` | `@param` raw SQL fragment (filters `tpMas.YearValue/MonthValue/ApprovalStatus/CreatedBy/EmpInfoId`, `tpdtl.CreatedDate`, `us.UserRoleID`, hierarchy) | columns DoctorCode, DoctorName, Degree, Speciality, ProgramTypeName, DoctorTypeName, DoctorCount |
| Doctor daily visit list | `DoctorDailyVisit.aspx` (PAGE-660, B) | `GetDoctorDailyVisitList(param)` → `sp_Get_DCRInfoList` | `@param` raw | filters incl. `CONVERT(date,DCR.EntryDate)` range, **`DCR.IsApproved`** (options All / 0 Pending / 1 Approved), `dtl.EmpInfoId`, `us.UserRoleID`, `gp/zn/ar/tr/mr` |
| DCR list (with approval) | `DCRList.aspx` (PAGE-658, A) | `GetDCRList` → `sp_Get_DCRInfoListForView` | `@param` raw | see PAGE-658 |
| DCR detail | `DCRReport.aspx?id=` (PAGE-659, B) | `GetDCRReportDataById(id)` → `sp_Rpt_DCRInfo_ById` | `@id` | header `empInfo, DesigName, DoctorName, DcrDate, TourTypeName, ChamberName`; rows `EmpName`, `Type`, `ProductName` (with-employee and product/sample lines) |
| MIS Attendance & DCR | `MISAttendanceAndDCRReport.aspx` (PAGE-665, B) | `GetMISAttendanceAndDCRReportData(startDate,endDate,empInfoId)` → `DoctorVisitDAL.GetMISAttendanceAndDCRReport` → inline `SELECT EmpMasterCode FROM tblEmpGeneralInfo WHERE EmpInfoId=@EmpInfoId` then `sp_MIS_AttendanceAndDCRReport` | `@StartDate,@EndDate,@EmpMasterCode` | default From = 1st of month, To = today; employee dropdown (`--- Select Employee ---` value `0`); date parsing accepts 10 formats then invariant `DateTime.Parse`; **any exception returns `"[]"`** |
| Salesman customer visit | `SalesmanCustomerVisitReport.aspx` (PAGE-666, B) | `GetSalesmanCustomerVisitReportData(startDate,endDate,empInfoId,filterStatus)` → `sp_MIS_GetSalesmanCustomerVisitReport` | `@StartDate,@EndDate,@SalesPersonMasterCode (NULL if empInfoId<=0),@FilterStatus (NULL for All)` | DCR Status filter All / Visited / Not Visited; columns Salesman ID/Name, Customer ID/Name, Total Visited (DCR Count), DCR Status, Total Sales Amount (BDT) |
| Salesman customer coverage summary | `SalesmanCustomerVisitSummeryReport.aspx` (PAGE-667, B) | `GetSalesmanCustomerCoverageSummaryData(...)` → `SP_GetSalesmanCustomerCoverageSummary` | `@StartDate,@EndDate,@SalesPersonMasterCode` | try/catch returns `"[]"` |

## Common notes
- Filters are built in the browser as SQL text (`AND mas.YearValue='…'`, dates concatenated) and posted to WebMethods with no server validation or role scoping (except DCRList).
- The hierarchy (Group/Zone/Area/Territory/Market) ids in `DoctorVisit`/`DoctorDailyVisit`/`DoctorVisitReport` use alias names `gp, zn, ar, tr, mr` and `ZoneId` (vs `RegionId` used elsewhere) — SP alias mapping UNKNOWN.
- Report SP bodies: none in the repository.

## Business rules / gaps
BR-M16-026..036; GAP-M16-024, 034, 035, 059, 060.

## Tests
Filter each report by current month; compare counts between DoctorDailyVisit (IsApproved) and DCRList (ApprovalStatus) for the same day; MIS report with an employee lacking `EmpMasterCode` (empty code passed); date input "2026-09-19" and "19 September, 2026" both parse.
