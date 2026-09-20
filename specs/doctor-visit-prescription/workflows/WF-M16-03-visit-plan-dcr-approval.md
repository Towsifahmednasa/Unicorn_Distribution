# WF-M16-03 — Doctor visit: call plan (DCP) → call report (DCR) → approval → reporting

Scope note: **no web page in this repository creates a DCP or a DCR.** Both are created outside the web app (INFERRED: mobile app / `webapi` SPs — the approval log SP is `sp_webapi_SaveDCRAppLog`, the DCR list shows `POutLoc` GPS location and `EntryByApp`; there is no INSERT/`sp_Save_DCR*` call in any M16 page). The web application **views, approves/rejects and reports** them. The parts that are in code are documented below; the creation side is `UNKNOWN`.

## Trigger & actors
- Field user (MIO/MIA, role type `MIO`) submits a monthly plan and daily call reports in the mobile app (creation channel INFERRED).
- Line managers (AM, DZSM, NSM) and HO users review lists and approve/reject DCRs in the web app.

## Preconditions
Doctors exist with tagged sales persons and chambers (WF-M16-01); employee field-force hierarchy exists (`View_Webapi_EmployeeFieldForceInfo` with `MIOEmpId, ASMEMPId, RSMEMPId, NSMEMPId`).

## Steps (web-app side)
### A. Call plan (DCP) monitoring
1. `DoctorVisit.aspx` (PAGE-662) → `GetDoctorVisitList(param)` → `sp_Get_DoctorTourPlanMasteList` → list of monthly plan headers (`DocTPMaster`, `YearValue`, `MonthValue`, `FinalSubmitRemarks`, `ApprovalStatus`).
2. Drill-down: `DoctorPlanDetailsView.aspx?id=<DocTPMaster>` (PAGE-661) → `sp_Get_DoctorPlanDetailsById` → planned doctors per date (`TourPlanDate`, `DoctorName`, `Type_DV`).
3. Approval of plans: `DoctorVisit_UI/DoctorVisitApprovalList.aspx` (PAGE-663) is **non-functional** (Razor URLs in WebForms; `DoctorVisitDAL.Approve_DoctorPlanList → sp_ApproveDoctorTourPlanMaster` unreferenced). A separate working approval exists in another module (`DoctorModule_UI/VisitPlannedApprovalList.aspx` → `Setup.aspx/Approve_TourPlanList`).
4. Pivot: `DoctorVisitReport.aspx` (PAGE-664) → `DynamicPivotDoctorWiseDoctorVisitPlan(@param)`.

### B. DCR review and approval
5. `DCRList.aspx` (PAGE-658): `LoadData` → `DoctorVisitDAL.GetDCRList(param)` → `sp_Get_DCRInfoListForView(@param)`; filters by date (default today), approval status, role, employee, pharma platform; visibility scoped by session `RoleTypeName` (MIO/AM/DZSM/NSM) via `View_Webapi_EmployeeFieldForceInfo` (BR-M16-027).
6. View: `DCRReport.aspx?id=<DcrId>` (PAGE-659) → `GetDCRReportDataById` → `sp_Rpt_DCRInfo_ById` (doctor, date, visit type, chamber, employees present, products/samples by type).
7. Approve/Reject click (`loadGridView_RowCommand`): read latest `tblDCRApprovalLog` row for the DCR (inline SQL) → `CustomerInfoDAL.SaveDCR_Applog_DAL` → `sp_webapi_SaveDCRAppLog` with `Type='DCR'`, `Status='Accepted'|'Rejected'`, `Step=previous+1`, `FromEmpId=session EmpInfoId`, `ToEmpId=previous ToEmpId`, `MenuId=382`, group/region/area/territory from log or DCR (BR-M16-029).
8. Buttons toggle: Approve only for currently "Rejected" rows, Reject only for currently "Approved" rows (BR-M16-028).

### C. Daily visit list and MIS
9. `DoctorDailyVisit.aspx` (PAGE-660) → `sp_Get_DCRInfoList(@param)`; its Approval Status filter is `DCR.IsApproved` ('0' Pending / '1' Approved), a **different flag** from `DCR.ApprovalStatus` (BR-M16-032).
10. `MISAttendanceAndDCRReport.aspx` (PAGE-665) → `sp_MIS_AttendanceAndDCRReport(@StartDate,@EndDate,@EmpMasterCode)`; `SalesmanCustomerVisitReport.aspx` (666) → `sp_MIS_GetSalesmanCustomerVisitReport(@StartDate,@EndDate,@SalesPersonMasterCode,@FilterStatus)`; `SalesmanCustomerVisitSummeryReport.aspx` (667) → `SP_GetSalesmanCustomerCoverageSummary`.
11. Dashboard `Dashboard_UI/DoctorVisitMonitoring.aspx` (PAGE-636) → `sp_Get_AllDoctorVisitMonitoringApprovalList` per employee with DCP/DCR/RX/CVP/CVR/sales columns; drill-down modal is a stub (BR-M16-035).
12. Monthly pivots (PAGE-671/672/673/675) consume DCR/DCP/RX by approval status (see reports RPT-M16-01).

## Data created / changed (by the web app)
| Table | Change |
|---|---|
| `tblDCRApprovalLog` | one new row per approve/reject click (via `sp_webapi_SaveDCRAppLog`; exact columns UNKNOWN beyond the parameter list) |
| `tbl_DCRInfo.ApprovalStatus` | presumably updated by that SP (UNKNOWN) |
| DCP tables | not changed by web pages |

## Status lifecycle (DCR)
`(created by app: Pending?/Approved?)` → web: `Approved --Reject--> Rejected --Approve--> Approved ("Accepted" written to log)`. Two flags exist: `ApprovalStatus` (list, filters, log) and `IsApproved` (DoctorDailyVisit filter). Full state machine `UNKNOWN`.

## Branches / exceptions
- Row without buttons: any status other than "Approved"/"Rejected" (e.g. pending) cannot be actioned from the web.
- Employee without a field-force row → `IndexOutOfRangeException` in `param()`.
- Replay of a stale POST approves again (no server status re-check).
- No latest log row → no action, no message (BR-M16-030).

## Postconditions
DCR reflects last decision; log has an additional `Step`.

## Related BR ids
BR-M16-026..036.

## Related pages
PAGE-636, 658, 659, 660, 661, 662, 663, 664, 665, 666, 667.

## Tests
As in PAGE-658 §27 plus: DCP header list for the current month shows rows; Reject then Approve a DCR and verify `tblDCRApprovalLog` has two new rows with increasing `Step`; MIO user cannot see other MIO's DCR; filter `DoctorDailyVisit` by Approved(1)/Pending(0) and compare with `DCRList` status filter for the same DCR (expect divergence risk, GAP-M16-059).
