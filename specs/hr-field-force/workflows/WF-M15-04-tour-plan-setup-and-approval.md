# WF-M15-04 Tour plan set-up, approval and reporting

**Actors:** HR/admin (set-up), field staff (create plans - mobile app, not in M15 web code), approving manager. **Pages:** PAGE-600 `TourTypeSetup` (+599 list), PAGE-597/598 `TourPurposeSetup(New)` (+596), PAGE-595 `TourPurposeOtherSetup`, PAGE-616 `TourSetupForEmployee` (+617), PAGE-589 `TADAMarketRuleConfiguration` (+590), PAGE-592 `TourPlannedApprovalList`, PAGE-591 `TourPlanDetailsView`, PAGE-594 `TourPlannedUserList`, PAGE-607 `VisitPlannedApprovalList`, reports 593/626/627/629.

## Set-up
1. **Tour types / station types** (HQ=2, EX-HQ=1, OS=3 in static markup; master lists `sp_Get_TourPlanTypeDDL`, `Get_StationTypeInfo`).
2. **Tour purposes** (PAGE-597/598/600) and their applicability by role/market/station type (PAGE-595, BR-M15-029..033).
3. **Tour quota** (PAGE-616): radio Role-wise (0) / Employee-wise (1); role type or employee + tour type + `CountNo` required; `TourSetupForEmployeeDAL.SaveDAInfo`: insert only after `sp_Check_TourSetupEmployeeList(@EmpInfoId)` or `sp_Check_TourSetupEmployeeListRoleType(@RoleTypeId)` returns 0 rows -> `sp_Save_TourSetupEmployee` (else `Already Exist!`); update (`sp_Update_TourSetupEmployee`) touches only `CountNo` (BR-M15-071). Meaning of `CountNo` (allowed tour days per type per month) INFERRED; consumer: `sp_Webapi_Get_TourPlanBalanceWithEmpInfo` (tour balance shown in reports).
4. **TA/DA amounts** (PAGE-589): per tour type, scope role-wise / market-wise / both, TA and DA amounts (`SaveTADAMarketRuleConfiguration`: update guarded by `sp_check_TADAMarketRuleConfiguration`; insert `sp_Save_TADAMarketRulesConfig`) (BR-M15-072). Which rule feeds a plan's `DAAmount`: UNKNOWN.

## Plan creation (outside M15 web code)
Mobile app / webapi writes the plan master (`ApprovalStatus`) - UNKNOWN procedures. Web view: `TourPlanDetailsView.aspx?id=`.

## Approval (PAGE-592; PAGE-607 is the same for doctor visit plans)
1. `LoadData` -> `TourTypeDal.GetTourPlanList("and mas.ApprovalStatus<>'2'")` -> `sp_Get_TourPlanMasteList` (pending = everything not approved, includes previously rejected `3`).
2. Approver ticks rows, chooses radio Approve (`2`, default) / Reject (`3`) (`chkAction`), Save -> per ticked row `Approve_TourPlanList(id, status, Session UserId)` -> `sp_ApproveTourPlanInformation(@TPMaster,@ApprovedBy,@Status)`; `Res` keeps only the **last** row's result; success alert -> reload (BR-M15-070, GAP-M15-021). Doctor visit plans: `Approve_VisitPlanList` -> `sp_ApproveVisitPlanInformation`.
3. Alternative multi-step approval with approval log: `Approval_UI/TourPlanApproval.aspx` (`Verified`/`Rejected`, `MenuId 371`, `Type="Leave"` copy-paste) - cross-module.
4. Claim link: an approved plan's date/market/tour type/DA amount pre-fills the TADA claim (WF-M15-05).

## Reports
Tour plan report/summary and PDF: RPT-M15-03.

**BR:** 029-033, 070-072, 075. **Tests:** duplicate role quota (blocked), reject then re-list (still pending), approve 3 rows where the last fails (UI reports failure although others approved).
