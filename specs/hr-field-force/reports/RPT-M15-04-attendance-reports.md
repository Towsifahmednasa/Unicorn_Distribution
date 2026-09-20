# RPT-M15-04 Attendance list / report

- **PAGE-562** `AttendanceInfoList.aspx` and **PAGE-563** `AttendanceInfoReport.aspx` (`Solution.Web/DoctorModule_UI`): HTML grids, filters date range, approval status (`sp_GET_ActionStatusList`), user role (`Get_UserRoleInfo`), employee (`sp_CS_GetEmpGeneralInfo_All`), scoped by viewer role type (MIO/AM/DZSM/NSM) using `sp_Get_EmployeeFieldForceInfo_EmpId` (BR-M15-069).
- **Data:** `AttendanceDAL.Get_Emp_AttendanceInfoList(param)` -> `sp_Get_Emp_AttendanceInfoList(@param)`; day-row layout `Get_Emp_AttendanceInfoListRowDay` -> `sp_Get_Emp_AttendanceInfoDayRow(@param)`. `param` uses aliases `att1/att2`, `View_Webapi_EmployeeFieldForceInfo` (raw SQL fragment, GAP-M15-012). Tables in SP: UNKNOWN (approval page shows `tblMarketAttendance_Master_webapi`).
- **Output:** on-screen grid (export controls in PAGE-563 - Tier B). No `.rpt`.
- **Approval variant:** PAGE-564 (WF-M15-06).
