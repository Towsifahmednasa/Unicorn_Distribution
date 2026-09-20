# RPT-M15-03 Tour plan reports

| Report | List page | Viewer / output | Data |
|---|---|---|---|
| Tour plan (monthly, per employee) | PAGE-626 `Reports_UI/TourPlanReportNew.aspx` (employee, month, year) | Excel (HTML, `application/ms-excel`); PDF via popup PAGE-629 `SInventory_RPTVIEW/TourPlanReportViwer.aspx?EmployeeId=&Month=&Year=` -> `Reports/CrystalReports/rptTourPlan.rpt` (DataSet `dtTPDetail`, `dtBal`), exported as PDF `Tourplant-` | `EmployeeInformationDaL.GetTourPlanReport__` -> `sp_Get_TourPlanReportList__n(@EmpInfoId,@Month,@Year)`; balance `GetTourPlanReportBal` -> `sp_Webapi_Get_TourPlanBalanceWithEmpInfo`; list variant `GetTourPlanReportNew` -> `sp_Get_TourPlanReportList(@param)` |
| Tour plan summary | PAGE-627 `TourPlanSummaryReport.aspx` (month, year, employee, role) | grid / Excel | `GetTourPlanSummaryReportReport` -> `sp_Get_TourPlanSummaryReport(@EmpInfoId,@UserRoleId,@Month,@Year)`; role filter `usR.UserRoleId` |
| Planned tours list | PAGE-593 `TourPlannedReport`, PAGE-594 `TourPlannedUserList` | HTML | Tier B (not read in depth) |

Notes: all calculations (tour balance = quota `CountNo` from PAGE-616 minus used days, INFERRED) are in SPs (UNKNOWN). Viewer 629 calls `ExportToHttpResponse` before binding the viewer (double output). No permission checks; URL parameters unvalidated. Monthly expense viewer (PAGE-628) is linked from the tour report page (popup URL) - see RPT-M15-02.
