# M15 - HR, Field-Force & Employee Administration

Reviewed 2026-09-19 (reverse-engineering; documentation only). Evidence tags: CONFIRMED / INFERRED / UNKNOWN. **No stored-procedure body of this module exists in the repository** (`sp_bodies.json` has none of them), so leave accrual, yearly leave processing, TADA/mileage amounts, approval routing (who is the next approver) and duplicate predicates are `UNKNOWN`; only the C# that calls them is documented.

## 1. Purpose & business overview
Employee master and login creation, sales-force hierarchy assignment (MIO/AM/DZSM/NSM), attendance and shift set-up, leave (types, configuration, applications, yearly balance), holidays, tour-plan set-up and approval, expense / TADA (daily allowance) / mileage claims and their rule tables, monthly allowances, training, transport modes, notice board, department/designation masters, and HR reports. Users: HR/admin (masters, employees), field managers (approvals), field staff (mostly via the mobile app - attendance, tour plans and claims are *created* by the app; the web pages here list, correct, approve and report).

### Employee <-> user <-> MIO/MIA <-> market hierarchy (CONFIRMED unless noted)
- Employee master = table `tblEmpGeneralInfo` (key `EmpInfoId`, code `EmpMasterCode`) - inline SQL `Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:272-296`, `Library.DAL/SInventory_DAL/EmpGeneralInfoDAL.cs:18`. New pages write it through `sp_Save_EmployeeInformation` (body UNKNOWN; table INFERRED same).
- User = `tblUser` (`UserId`, `UserRoleID`, `UserTypeId`, `EmpInfoId`, `UserStatus`...), linked **1:1 by `EmpInfoId`** for employee users (`UserTypeId=3`); for DA users (`UserTypeId=6`) the same column holds `tblDAInfo.DAId`. Created together with the employee on PAGE-613/615; Session gets `UserId`, `UserRoleID`, `RoleTypeId/Name`, `EmpInfoId` at login (`Solution.Web/Login.aspx.cs:34-65`).
- Role -> role type: `sp_GET_UserRoleInfoRoleType_ById` returns `RoleType` (`MIO`, `AM`, `DZSM`, `NSM`, `DIC`, others) which decides the hierarchy record.
- Hierarchy rows: `MIOInfo(EmployeeId=EmpInfoId, TerritoryId)`, `ASMInfo(EmployeeId, AreaId)` (AM/ASM/FE), `RSMInfo(EmployeeId, RegionId)` (DZSM/RSM), `NSMInfo(EmployeeId, GroupId)`; chain Group > Region(Zone) > Area > Territory > SubTerritory > Market. DIC users get depots via `UserCompanyUnit`. Write endpoints: `FieldForce.aspx/*` (PAGE-574) and PAGE-613/615.
- Resolving a person's manager/scope: `sp_Get_EmployeeFieldForceInfo_EmpId(@id=EmpInfoId)` returns `MIOEmpId/ASMEMPId/RSMEMPId/NSMEMPId` and `EmpGroupId/EmpRegionId/EmpAreaId/EmpTerrId` (used by PAGE-562, 588); reports filter on view `View_Webapi_EmployeeFieldForceInfo` (body UNKNOWN).
- **MIA** (legacy, `SInventory_UI`): `tblMIAInfo(MiaId, AreaId, MiaCode, MiaName)`; the MIA code is looked up in `tblEmpGeneralInfo.EmpMasterCode`; saving calls `sp_MioUpdateInCustomerInfo(@ter,@mio,@name)` to stamp the MIO on customers by area *name* (PAGE-634, `MiaInformationDAL.cs`). Not linked to `MIOInfo`. The Web hierarchy (MIOInfo) is the current model; MIA pages are legacy.

## 2. Page catalogue (74 pages, tier A = upgraded)
| Page ID | Page (folder) | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-562 | AttendanceInfoList (DoctorModule_UI) | List / View | B | [pages/PAGE-562-attendanceinfolist.md](pages/PAGE-562-attendanceinfolist.md) |
| PAGE-563 | AttendanceInfoReport (DoctorModule_UI) | Report | B | [pages/PAGE-563-attendanceinforeport.md](pages/PAGE-563-attendanceinforeport.md) |
| PAGE-564 | AttendanceListApproval (DoctorModule_UI) | Approval | A | [pages/PAGE-564-attendancelistapproval.md](pages/PAGE-564-attendancelistapproval.md) |
| PAGE-565 | defaltmypage (DoctorModule_UI) | Page (empty) | B | [pages/PAGE-565-defaltmypage.md](pages/PAGE-565-defaltmypage.md) |
| PAGE-566 | Department (DoctorModule_UI) | Master entry | B | [pages/PAGE-566-department.md](pages/PAGE-566-department.md) |
| PAGE-567 | DepartmentView (DoctorModule_UI) | List | B | [pages/PAGE-567-departmentview.md](pages/PAGE-567-departmentview.md) |
| PAGE-568 | Designation (DoctorModule_UI) | Master entry | B | [pages/PAGE-568-designation.md](pages/PAGE-568-designation.md) |
| PAGE-569 | DesignationView (DoctorModule_UI) | List | B | [pages/PAGE-569-designationview.md](pages/PAGE-569-designationview.md) |
| PAGE-570 | ExpenseClaim (DoctorModule_UI) | Entry (claim) | A | [pages/PAGE-570-expenseclaim.md](pages/PAGE-570-expenseclaim.md) |
| PAGE-571 | ExpenseClaimView (DoctorModule_UI) | List | B | [pages/PAGE-571-expenseclaimview.md](pages/PAGE-571-expenseclaimview.md) |
| PAGE-572 | ExpenseType (DoctorModule_UI) | Master entry | A | [pages/PAGE-572-expensetype.md](pages/PAGE-572-expensetype.md) |
| PAGE-573 | ExpenseTypeView (DoctorModule_UI) | List | B | [pages/PAGE-573-expensetypeview.md](pages/PAGE-573-expensetypeview.md) |
| PAGE-574 | FieldForce (DoctorModule_UI) | WebMethod host (35 endpoints) | A | [pages/PAGE-574-fieldforce.md](pages/PAGE-574-fieldforce.md) |
| PAGE-575 | Holiday (DoctorModule_UI) | Master entry | B | [pages/PAGE-575-holiday.md](pages/PAGE-575-holiday.md) |
| PAGE-576 | HolidayView (DoctorModule_UI) | List | B | [pages/PAGE-576-holidayview.md](pages/PAGE-576-holidayview.md) |
| PAGE-577 | Leave (DoctorModule_UI) | Master entry (leave type + days) | B | [pages/PAGE-577-leave.md](pages/PAGE-577-leave.md) |
| PAGE-578 | LeaveConfig (DoctorModule_UI) | Master entry (leave rules) | A | [pages/PAGE-578-leaveconfig.md](pages/PAGE-578-leaveconfig.md) |
| PAGE-579 | LeaveConfigList (DoctorModule_UI) | List | B | [pages/PAGE-579-leaveconfiglist.md](pages/PAGE-579-leaveconfiglist.md) |
| PAGE-580 | LeaveView (DoctorModule_UI) | List | B | [pages/PAGE-580-leaveview.md](pages/PAGE-580-leaveview.md) |
| PAGE-581 | MileageClaim (DoctorModule_UI) | Entry (claim) | B | [pages/PAGE-581-mileageclaim.md](pages/PAGE-581-mileageclaim.md) |
| PAGE-582 | MileageClaimView (DoctorModule_UI) | List | B | [pages/PAGE-582-mileageclaimview.md](pages/PAGE-582-mileageclaimview.md) |
| PAGE-583 | MonthlyAllowance (DoctorModule_UI) | Master entry | B | [pages/PAGE-583-monthlyallowance.md](pages/PAGE-583-monthlyallowance.md) |
| PAGE-584 | MonthlyAllowanceView (DoctorModule_UI) | List | B | [pages/PAGE-584-monthlyallowanceview.md](pages/PAGE-584-monthlyallowanceview.md) |
| PAGE-585 | ShiftInfoEntry (DoctorModule_UI) | Master entry | B | [pages/PAGE-585-shiftinfoentry.md](pages/PAGE-585-shiftinfoentry.md) |
| PAGE-586 | ShiftInfoList (DoctorModule_UI) | List | B | [pages/PAGE-586-shiftinfolist.md](pages/PAGE-586-shiftinfolist.md) |
| PAGE-587 | TADAClaimEdit (DoctorModule_UI) | Edit (DA claim) | A | [pages/PAGE-587-tadaclaimedit.md](pages/PAGE-587-tadaclaimedit.md) |
| PAGE-588 | TADAClaimView (DoctorModule_UI) | List / CSV export | B | [pages/PAGE-588-tadaclaimview.md](pages/PAGE-588-tadaclaimview.md) |
| PAGE-589 | TADAMarketRuleConfiguration (DoctorModule_UI) | Rule master | B | [pages/PAGE-589-tadamarketruleconfiguration.md](pages/PAGE-589-tadamarketruleconfiguration.md) |
| PAGE-590 | TADAMarketRuleConfigurationView (DoctorModule_UI) | List | B | [pages/PAGE-590-tadamarketruleconfigurationview.md](pages/PAGE-590-tadamarketruleconfigurationview.md) |
| PAGE-591 | TourPlanDetailsView (DoctorModule_UI) | View | B | [pages/PAGE-591-tourplandetailsview.md](pages/PAGE-591-tourplandetailsview.md) |
| PAGE-592 | TourPlannedApprovalList (DoctorModule_UI) | Approval | B | [pages/PAGE-592-tourplannedapprovallist.md](pages/PAGE-592-tourplannedapprovallist.md) |
| PAGE-593 | TourPlannedReport (DoctorModule_UI) | Report | B | [pages/PAGE-593-tourplannedreport.md](pages/PAGE-593-tourplannedreport.md) |
| PAGE-594 | TourPlannedUserList (DoctorModule_UI) | List | B | [pages/PAGE-594-tourplanneduserlist.md](pages/PAGE-594-tourplanneduserlist.md) |
| PAGE-595 | TourPurposeOtherSetup (DoctorModule_UI) | Master entry | A | [pages/PAGE-595-tourpurposeothersetup.md](pages/PAGE-595-tourpurposeothersetup.md) |
| PAGE-596 | TourPurposeRecords (DoctorModule_UI) | List | B | [pages/PAGE-596-tourpurposerecords.md](pages/PAGE-596-tourpurposerecords.md) |
| PAGE-597 | TourPurposeSetup (DoctorModule_UI) | Master entry | B | [pages/PAGE-597-tourpurposesetup.md](pages/PAGE-597-tourpurposesetup.md) |
| PAGE-598 | TourPurposeSetupNew (DoctorModule_UI) | Master entry | B | [pages/PAGE-598-tourpurposesetupnew.md](pages/PAGE-598-tourpurposesetupnew.md) |
| PAGE-599 | TourTypeRecords (DoctorModule_UI) | List | B | [pages/PAGE-599-tourtyperecords.md](pages/PAGE-599-tourtyperecords.md) |
| PAGE-600 | TourTypeSetup (DoctorModule_UI) | Master entry | B | [pages/PAGE-600-tourtypesetup.md](pages/PAGE-600-tourtypesetup.md) |
| PAGE-601 | Trainning (DoctorModule_UI) | Entry (training + audience) | B | [pages/PAGE-601-trainning.md](pages/PAGE-601-trainning.md) |
| PAGE-602 | TrainningDetails (DoctorModule_UI) | View | B | [pages/PAGE-602-trainningdetails.md](pages/PAGE-602-trainningdetails.md) |
| PAGE-603 | TrainningDetailsApp (DoctorModule_UI) | View (app) | B | [pages/PAGE-603-trainningdetailsapp.md](pages/PAGE-603-trainningdetailsapp.md) |
| PAGE-604 | TrainningView (DoctorModule_UI) | List | B | [pages/PAGE-604-trainningview.md](pages/PAGE-604-trainningview.md) |
| PAGE-605 | Transport (DoctorModule_UI) | Master (mode + per-km rate) | B | [pages/PAGE-605-transport.md](pages/PAGE-605-transport.md) |
| PAGE-606 | TransportView (DoctorModule_UI) | List | B | [pages/PAGE-606-transportview.md](pages/PAGE-606-transportview.md) |
| PAGE-607 | VisitPlannedApprovalList (DoctorModule_UI) | Approval (doctor visit plan) | B | [pages/PAGE-607-visitplannedapprovallist.md](pages/PAGE-607-visitplannedapprovallist.md) |
| PAGE-608 | LeaveApplicationCode (LeaveProcess_UI) | WebMethod host (leave save/approve/balance) | A | [pages/PAGE-608-leaveapplicationcode.md](pages/PAGE-608-leaveapplicationcode.md) |
| PAGE-609 | LeaveApplicationEntry (LeaveProcess_UI) | Entry (leave application) | B | [pages/PAGE-609-leaveapplicationentry.md](pages/PAGE-609-leaveapplicationentry.md) |
| PAGE-610 | LeaveApplications (LeaveProcess_UI) | List | B | [pages/PAGE-610-leaveapplications.md](pages/PAGE-610-leaveapplications.md) |
| PAGE-611 | YearlyLeaveProcess (LeaveProcess_UI) | Batch process | A | [pages/PAGE-611-yearlyleaveprocess.md](pages/PAGE-611-yearlyleaveprocess.md) |
| PAGE-612 | EmployeeRecords (MasterSetup_UI) | List (permission-checked) | B | [pages/PAGE-612-employeerecords.md](pages/PAGE-612-employeerecords.md) |
| PAGE-613 | EmployeeSetup (MasterSetup_UI) | Onboarding (employee + login + hierarchy) | A | [pages/PAGE-613-employeesetup.md](pages/PAGE-613-employeesetup.md) |
| PAGE-614 | EmployeeSetupEdit (MasterSetup_UI) | Edit | A | [pages/PAGE-614-employeesetupedit.md](pages/PAGE-614-employeesetupedit.md) |
| PAGE-615 | EmployeeSetupForNewJoiner (MasterSetup_UI) | Onboarding (legacy variant) | A | [pages/PAGE-615-employeesetupfornewjoiner.md](pages/PAGE-615-employeesetupfornewjoiner.md) |
| PAGE-616 | TourSetupForEmployee (MasterSetup_UI) | Rule master (tour quota) | B | [pages/PAGE-616-toursetupforemployee.md](pages/PAGE-616-toursetupforemployee.md) |
| PAGE-617 | TourSetupForEmployeeList (MasterSetup_UI) | List | B | [pages/PAGE-617-toursetupforemployeelist.md](pages/PAGE-617-toursetupforemployeelist.md) |
| PAGE-618 | NoticeBoard (NoticeBoard_UI) | View | B | [pages/PAGE-618-noticeboard.md](pages/PAGE-618-noticeboard.md) |
| PAGE-619 | NoticeDetailsApp (NoticeBoard_UI) | View (app) | B | [pages/PAGE-619-noticedetailsapp.md](pages/PAGE-619-noticedetailsapp.md) |
| PAGE-620 | NoticeRecords (NoticeBoard_UI) | List | B | [pages/PAGE-620-noticerecords.md](pages/PAGE-620-noticerecords.md) |
| PAGE-621 | NoticeSetup (NoticeBoard_UI) | Entry (notice + audience) | A | [pages/PAGE-621-noticesetup.md](pages/PAGE-621-noticesetup.md) |
| PAGE-622 | Employee_YearlyLeaveBalanceRpt (Reports_UI) | Report | B | [pages/PAGE-622-employee-yearlyleavebalancerpt.md](pages/PAGE-622-employee-yearlyleavebalancerpt.md) |
| PAGE-623 | EmpMonthlyExpenseRpt (Reports_UI) | Report | B | [pages/PAGE-623-empmonthlyexpenserpt.md](pages/PAGE-623-empmonthlyexpenserpt.md) |
| PAGE-624 | EmpMonthlyExpenseRptMultiple (Reports_UI) | Report | B | [pages/PAGE-624-empmonthlyexpenserptmultiple.md](pages/PAGE-624-empmonthlyexpenserptmultiple.md) |
| PAGE-625 | OrganogramReport (Reports_UI) | Report | B | [pages/PAGE-625-organogramreport.md](pages/PAGE-625-organogramreport.md) |
| PAGE-626 | TourPlanReportNew (Reports_UI) | Report | B | [pages/PAGE-626-tourplanreportnew.md](pages/PAGE-626-tourplanreportnew.md) |
| PAGE-627 | TourPlanSummaryReport (Reports_UI) | Report | B | [pages/PAGE-627-tourplansummaryreport.md](pages/PAGE-627-tourplansummaryreport.md) |
| PAGE-628 | EmployeeExpenseReportViewer (SInventory_RPTVIEW) | Crystal viewer | B | [pages/PAGE-628-employeeexpensereportviewer.md](pages/PAGE-628-employeeexpensereportviewer.md) |
| PAGE-629 | TourPlanReportViwer (SInventory_RPTVIEW) | Crystal viewer | B | [pages/PAGE-629-tourplanreportviwer.md](pages/PAGE-629-tourplanreportviwer.md) |
| PAGE-630 | EmpGeneralEdit (SInventory_UI) | Legacy edit | B | [pages/PAGE-630-empgeneraledit.md](pages/PAGE-630-empgeneraledit.md) |
| PAGE-631 | EmpGeneralInfoEntry (SInventory_UI) | Legacy entry | B | [pages/PAGE-631-empgeneralinfoentry.md](pages/PAGE-631-empgeneralinfoentry.md) |
| PAGE-632 | EmpGenInfView (SInventory_UI) | Legacy list | B | [pages/PAGE-632-empgeninfview.md](pages/PAGE-632-empgeninfview.md) |
| PAGE-633 | MiaInfoEdit (SInventory_UI) | Legacy edit | B | [pages/PAGE-633-miainfoedit.md](pages/PAGE-633-miainfoedit.md) |
| PAGE-634 | MiaInfoEntry (SInventory_UI) | Legacy entry | B | [pages/PAGE-634-miainfoentry.md](pages/PAGE-634-miainfoentry.md) |
| PAGE-635 | MiaInfoView (SInventory_UI) | Legacy list | B | [pages/PAGE-635-miainfoview.md](pages/PAGE-635-miainfoview.md) |

## 3. Business-rule index
| BR | One-liner | Page |
|---|---|---|
| BR-M15-001 | Employee name, code, joining date, >=1 shift required | 613 (614) |
| BR-M15-002 | NID 17 chars; phones 11 chars when supplied | 613 |
| BR-M15-003 | Employee code unique (SP check) | 613 |
| BR-M15-004 | Employee code auto-fills login name + password | 613 |
| BR-M15-005 | Employee gets user (`UserTypeId=3`, `EmpInfoId` link); login/password/role/date required | 613 |
| BR-M15-006 | Role type drives MIO/AM/DZSM/NSM/DIC assignment | 613 |
| BR-M15-007 | One holder per territory/area/zone/group on entry | 613 |
| BR-M15-008 | DIC user needs >=1 depot | 613 |
| BR-M15-009 | One Active flag drives employee, user, hierarchy status | 613 |
| BR-M15-010 | Shifts/product lines diff-sync; allowances per item | 613 |
| BR-M15-011 | Update blocked if `sp_check_EmployeeInfo` returns rows | 613 |
| BR-M15-012 | Main vs Depot dashboard flag | 613 |
| BR-M15-013 | Page 613/615 read-only with `?MID`; no M15 page updates user/hierarchy | 613 |
| BR-M15-014 | Hierarchy save pattern (insert / check+update) | 613 |
| BR-M15-015..018 | New-joiner variant: single shift, optional role, default active date, product lines cleared | 615 |
| BR-M15-019..022 | Edit page: code check on update, employee-only edit, shift/product replace, create-without-MID | 614 |
| BR-M15-023..028 | FieldForce endpoints: one holder per node, session stamping, reactivation branch, inactivation, NSM-Head mis-route, vacant lists | 574 |
| BR-M15-029..033 | Tour-purpose applicability setup rules, edit not implemented, role stored as text | 595 |
| BR-M15-034..040 | DA claim: required fields, market unless "(Other Visit)", prefill from tour plan, save routing, free amount, list scoping, purpose not saved | 587 (588) |
| BR-M15-041..044 | Leave configuration rules (no validation, count grid, type-driven panels, weekly-holiday day) | 578 |
| BR-M15-045 | Leave type master: name + days, deactivate/duplicate checks (SP) | 577 |
| BR-M15-046..049 | Expense type: required, fixed amount toggle, duplicate/deactivation guard, unique field names | 572 |
| BR-M15-050..054 | Attendance approval: status by role type, button visibility, bulk, permission check, copy-paste Type/Menu/zero hierarchy | 564 |
| BR-M15-055..057 | Notice: title+dates, audience, image file | 621 |
| BR-M15-058..062 | Leave application: required fields, edit lock, StartDate defect, balance display, approve endpoint unguarded | 608 |
| BR-M15-063..064 | Yearly leave process: global no-arg call, no repeat guard | 611 |
| BR-M15-065..068 | Expense claim: required, client approval lock, image file, free amount | 570 |
| BR-M15-069 | Attendance list role scoping (MIO/AM/DZSM/NSM) | 562 |
| BR-M15-070 | Tour-plan approval: Approve=2 / Reject=3, pending = status<>2, last result reported | 592/607 |
| BR-M15-071 | Tour quota per role or employee per tour type | 616 |
| BR-M15-072 | TADA market rule: TA/DA amounts by role / market / both per tour type | 589 |
| BR-M15-073 | Monthly allowance: named amount for a role, assigned to selected employees | 583 |
| BR-M15-074 | Mileage claim captures KM + meter reading + image; per-km rate lives on Transport master | 581/605 |
| BR-M15-075 | Approval chain constants (Types, MenuIds, Accepted/Verified/Rejected, RoleTypeId 5, EmpInfoId 496) | Approval_UI (cross-module) |
| BR-M15-076 | Leave approved through two independent paths | 608 / Approval_UI |
| BR-M15-077 | Legacy employee code `EMP-nnnn` from max(id)+1; MIA code unique | 631/634 |
| BR-M15-078 | Monthly expense report columns (DA, mileage, expense, total) from SPs | 623/628 |
| BR-M15-079 | Yearly leave report: eligible/taken/absent/balance for Casual, Sick, Annual + previous annual | 622 |
| BR-M15-080 | Employee list requires permission row unless UserRoleID=2 | 612 |

**Sources for rules on Tier-B pages (BR-M15-069..080, read directly, Confidence CONFIRMED unless noted):** 069 `Solution.Web/DoctorModule_UI/AttendanceInfoList.aspx.cs:230-251`; 070 `DoctorModule_UI/TourPlannedApprovalList.aspx.cs:53-110` + `TourPlannedApprovalList.aspx:60-63` + `Library.DAL/DoctorModule_DAL/TourTypeDal.cs:349-394`; 071 `MasterSetup_UI/TourSetupForEmployee.aspx.cs:78-165` + `Library.DAL/MasterSetup_DAL/TourSetupForEmployeeDAL.cs:38-114`; 072 `Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4608-4700` + JS `TADAMarketRuleConfiguration.aspx:546-598`; 073 `Setup2DAL.cs:4259-4360` + `MonthlyAllowance.aspx.cs:158-245`; 074 `Setup2DAL.cs` `Save_MileageClaim` + `Transport.aspx:246` (`AllowedMilagePerKM`; use as rate INFERRED); 075 `Approval_UI/{LeaveApproveList,DAApprovalList,ExpenseApprovalList,MillageApprovalList,TourPlanApproval}.aspx.cs`; 076 `LeaveProcess_UI/LeaveApplicationCode.aspx.cs` vs `Approval_UI/LeaveApproveList.aspx.cs:185-290`; 077 `Library.BLL/SInventory_BLL/EmpGeneralInfoBLL.cs:16-52`, `MIAInformationBLL.cs`, `MiaInformationDAL.cs:28-46`; 078 `SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs`, `ExpenseDal.cs:143-186`; 079 `Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:98-135`; 080 `MasterSetup_UI/EmployeeRecords.aspx.cs:17-58`.

## 4. Status lifecycles (see workflows)
- Employee/User/Hierarchy: Active/Inactive (BR-M15-009).
- Approval log: `Verified` -> `Accepted` (final) or `Rejected` (WF-M15-05, WF-M15-06). Approval-status master `sp_GET_ActionStatusList` (`SoftwareUseId`, `WebShow`) - values UNKNOWN.
- Tour plan: `ApprovalStatus` `2` approved, `3` rejected, other pending (BR-M15-070).
- Leave application: `Pending / Approved / Rejected` labels seen in JS (`LeaveApplicationEntry.aspx`), SP-driven (UNKNOWN).

## 5. Roles / permissions observed
Only two M15 pages enforce a code-level permission (`UserRoleID != "2"` -> `GetPermissionForUserRole(path)`, else redirect to `Dashboard_UI/DashboardOne.aspx`): PAGE-612 and PAGE-564. All other pages rely on the DB-driven menu (`sp_GET_MainMenuRole2`) and master-page login redirect. `RoleTypeId` 5 is the final approver in every approval page; `RoleTypeId` 4, 14, 9 also see action buttons; role types MIO/AM/DZSM/NSM scope list visibility to own/area/region/group. `Web.config` has no `<authorization>`; page-methods (`FieldForce.aspx/*`, `LeaveApplicationCode.aspx/*`, `YearlyLeaveProcess.aspx/*`) are callable without a session for reads.

## 6. Data model (what is confirmed)
Tables named in code: `tblEmpGeneralInfo`, `tblUser`, `tbl_UserRoleInfo`, `tblUserType`, `tblDAInfo`, `tblEmpwiseShiftDeclare`, `tblEmpwiseProductLineDeclare`, `tblMIAInfo`, `tblMarketAttendance_Master_webapi` (attendance list SQL), `tblDepartment`, `tblDesignation`, `tblManufacturer`; view `View_Webapi_EmployeeFieldForceInfo`. Everything else is behind SPs: MIO/ASM/RSM/NSM info, tour plan master, TADA claim master, expense claim master/detail, mileage claim, leave type/config/application/balance, holiday, shift, notice + audience, training + audience, allowance, transport, approval log (`sp_webapi_SaveAppLog`).

## 7. Integrations
None external. Mobile app (webapi SPs `sp_Webapi_*`) creates attendance, tour plans, claims and leave; web pages consume them. Report viewers: Crystal (`crpEmpExpense.rpt`, `rptTourPlan.rpt` in `Solution.Web/Reports/CrystalReports`). Image files (expense, mileage, notice) are written to a server path fetched by `AppPrimaryDAL.GetImagePath(type)`.

## 8. Workflows
[WF-M15-01 employee onboarding](workflows/WF-M15-01-employee-onboarding.md) - [WF-M15-02 leave application & approval](workflows/WF-M15-02-leave-application-and-approval.md) - [WF-M15-03 yearly leave process](workflows/WF-M15-03-yearly-leave-process.md) - [WF-M15-04 tour plan set-up & approval](workflows/WF-M15-04-tour-plan-setup-and-approval.md) - [WF-M15-05 claims (TADA/expense/mileage) & approval](workflows/WF-M15-05-claims-tada-expense-mileage-approval.md) - [WF-M15-06 attendance & approval](workflows/WF-M15-06-attendance-and-approval.md)

## 9. Reports
[reports/](reports/): RPT-M15-01 yearly leave balance, RPT-M15-02 monthly expense (Crystal), RPT-M15-03 tour plan reports, RPT-M15-04 attendance, RPT-M15-05 employee list / organogram.

## 10. Known gaps summary
See [GAPS-M15.md](GAPS-M15.md). Headline: leave accrual/yearly process and claim amounts are entirely inside undocumented SPs; unauthenticated page-method endpoints; onboarding not atomic and ignores user/hierarchy save results; default password = employee code; leave application never sends the From date; approval pages contain copy-paste Type/MenuId values and a hard-coded approver (`EmpInfoId 496`).

## 11. Still unverified
SP bodies (all); menu rows per role; approval-status code table; real approver routing (`ToEmpId/ToRoleTypeId` are produced by `sp_Get_AttendanceInformation` etc.); table names behind SPs; Tier-B pages (61 of 74) verified only by static extraction (a few read directly: PAGE-562, 588, 592/607, 616, 622-629 - facts recorded in workflows/reports).
