# INT-09 - Field-force mobile app (shared database + WebView pages)

The repository contains **no mobile-app code and no REST controller for it**. What is provable is the shared-database surface and app-oriented pages (mobile-app existence itself is INFERRED from names: `Webapi`, `AppLog`, `...Apps.aspx`).

| Item | Detail |
|---|---|
| Direction | Bidirectional via SQL Server (app writes its data; web app approves/reads) |
| Transport | (1) Shared DB objects `sp_Webapi_*`, `sp_webapi_Save*AppLog`, view `View_Webapi_EmployeeFieldForceInfo` (used inside `sp_Get_CustomerApp`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:221-402`). (2) Stand-alone URL pages `DWSP/TerritoryWiseTargetSetupApps.aspx?EmpId=` and `AMDayWiseDWSPSetup.aspx?EmpId=` (no master page, no session check) - likely WebViews (INT-09 / M14). |
| Web-side callers | Read: `sp_Webapi_Get_OrderApp`, `sp_Get_CustomerApp`, `sp_Webapi_Get_AttendanceInformation`, `sp_Webapi_Get_DoctorClaimApp`, `sp_Webapi_Get_TourPlanBalance*`, `sp_Webapi_LeaveReport_*`, `sp_Webapi_Get_EmpAllawance*`, `sp_Webapi_Get_DayName`, `sp_Webapi_Get_LeaveConType`, `sp_WebApi_GetVersion` and `sp_Webapi_Get_ImagePath` (`AppPrimaryDAL.cs`: app version + image path - only the DAL exists, no page uses it). Write: `sp_webapi_Save{Customer,Order,TourPlan,VisitPlan,DCR,Prescription,Doctor,Leave}AppLog`, `sp_webapi_SaveAppLog` (approval-step logs from `CustomerInfoDAL`, `OrderTrackingDAL`, `DoctorDAL`, `MarketStructureTransferDAL`), `sp_Webapi_Save_LeaveInfo`, `sp_Webapi_Update_LeaveData` (`LeaveApplicationDal.cs:130,179`). |
| Monitoring | `DoctorModule_UI/AppMonitoringList.aspx` (PAGE-676): `RSMSetupDal.Get_AppMonitoringList_new(param)` -> SP `AppMonitoringViewList_new(@Parameter)`; filter fragments (`us.UserRoleID`, `emp.EmployeeStatus`, `emp.EmpInfoId`) are string-concatenated. Non-admin (`UserRoleID != 2`) must have the page in role permission (`GetPermissionForUserRole`) or is redirected to `Dashboard_UI/DashboardOne.aspx` (BR-M17-024). |
| Status flags | Approval logs (`tblCustomerApprovalLog`: Status, Step, FromEmpId/ToEmpId, RoleTypeId) - customer ActionStatus 0 Pending, 1 Verified, 2 Approved, 3 Rejected (from `sp_Get_CustomerApp` CASE). |
| Authentication | Web side session; app authentication UNKNOWN. Apps pages: none. |
| Risks | Unauthenticated `?EmpId` pages (GAP-M14-002); SP bodies for `sp_Webapi_*` absent (GAP-M17-001). |
