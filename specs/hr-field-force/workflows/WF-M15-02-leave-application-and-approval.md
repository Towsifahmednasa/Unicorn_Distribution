# WF-M15-02 Leave set-up, application and approval

**Actors:** HR (masters), employee/HR clerk (application), approvers (managers / role type 5). **Pages:** PAGE-577 `Leave.aspx` (leave type + days), PAGE-578 `LeaveConfig.aspx` (rules), PAGE-609 `LeaveApplicationEntry.aspx`, PAGE-608 `LeaveApplicationCode.aspx` (web-methods), PAGE-610 `LeaveApplications.aspx` (list), PAGE-611 (yearly process, see WF-M15-03), `Approval_UI/LeaveApproveList.aspx` (cross-module), PAGE-622 (report), holidays PAGE-575.

## A. Masters
1. `Leave.aspx` -> `Leave.aspx/Save_Leaveinfo` -> `LeaveDal.Save_LeaveInfo`: id 0 -> `sp_Save_Employe_LeaveType_Infos(@LeaveTypeName,@LeaveDays,@IsActive,@EntryBy)`; id>0 -> `sp_check_LeaveInfo(@id,@Name)` must be empty then `sp_Update_Employee_Leave_info(@UpdateBy)` (`Library.DAL/DoctorModule_DAL/LeaveDal.cs:18-76`). Client messages: `Data cannot be deactivated!` (isValiCheck), duplicate (BR-M15-045).
2. `LeaveConfig.aspx` -> `BonusCampaignNewDAL.SaveLeaveConfig`: `sp_Save_LeaveConfigMaster` / `sp_Up_LeaveConfigMaster` (LeaveName, CountGovtLeave, CountEmployeeHoliday, DayNameId, EligbleforProbationEmployee, LeaveTypeId (`LeaveConType`), IsActive) + `sp_Save_LeaveConfigDtl(@LeaveConfigId,@JoiningDateCountId,@DaysPerMonthly)` per grid row + `sp_Save_LeaveConfigFroenEmp(@LeaveConfigId,@EmployeeId)` per employee (only when type text is `Foreign`) (BR-M15-041..044). **Accrual/balance formula: UNKNOWN** (SPs).
3. Holidays: PAGE-575 `Holiday.aspx/Save_Holiday` -> `HolidayDal.Save_LeaveInfo` (`sp_check_Holiday`, `sp_Save_Employee_Holiday`/`sp_Update_Employee_Holiday`; fields FiscalYear (always sent 0 by JS), HolidayDate, HolidayToDate, DayName). Whether leave day-count skips holidays is decided by config flags `CountGovtLeave`/`CountEmployeeHoliday` inside SPs (UNKNOWN).

## B. Application (PAGE-609 -> PAGE-608)
1. Employee code typed -> `LeaveApplicationCode.aspx/GetEmployeeLeaveBalance` -> `LeaveApplicationDal.GetEmployeeLeaveBalanceByCode` -> `sp_GET_EmployeeLeaveBalance(@Parameter=" AND EGI.EmpMasterCode = '<code>'")`; JS fills name, EmpInfoId, leave-type drop-down (`LeaveBalanceId`/`LeaveTypeName`) and a table (`YearlyLeaveBalance`, `LeaveBalance`) (BR-M15-061).
2. Submit -> client validation: type, code, days, from, to, reason all required (BR-M15-058) -> `SaveLeaveApplication` -> `LeaveApplicationDal.SaveLeaveApplication`: if `LeaveApplicationId>0` and `IsEditable` false (approval row found by `sp_Webapi_Get_LeaveApproveById`) -> `The leave has already been approved. Can't edit right now` (BR-M15-059); else `sp_Webapi_Save_LeaveInfo(@leaveAppId,@typeId=LeaveBalanceId,@startDate,@endDate,@reason,@empId)`. **`StartDate` is set to the To date by JS and `Days`/From date are not sent** (BR-M15-060, GAP-M15-015).
3. List: `LeaveApplications.aspx` -> `sp_Get_AllLeaveRecords(@Parameter)` with filters on `A.EntryDate` (default last 7 days), `A.ApprovalStatus`, role, employee, leave type (BR raw-SQL, GAP-M15-012).

## C. Approval (two independent paths)
- Path 1 (page 608): `LeaveApplicationCode.aspx/LeaveApplicationApprove(id, ApprovalStatus)` -> `sp_Webapi_Update_LeaveData(@id,@approvalType,@empId = Session UserId)` - no role check (BR-M15-062, 076). The UI that used to call it in `LeaveApplications.aspx` is commented out (`<%-- --%>`), so it is reachable only by direct POST.
- Path 2 (Approval_UI, cross-module): `LeaveApproveList.aspx` approve -> status `Accepted` when `Session RoleTypeId` is 5 or 9 (or `EmpInfoId==496`), else `Verified`; reject -> `Rejected`; `Type="Leave"` (`"Order"` on reject!), `MenuId=381`, `Step = current+1`, `FromEmpId`=approver, `ToEmpId`= row's next approver -> `SaveLeave_ApplogDAL` (`Approval_UI/LeaveApproveList.aspx.cs:185-290`). Who the next approver is: computed in the SP feeding the grid (UNKNOWN) (BR-M15-075).
- Statuses shown in list JS: `Pending`, `Approved`, other = Rejected (`LeaveApplicationEntry.aspx` legacy JS).

## Data / postconditions
Leave application row (+ approval log rows); balance deduction on approval: SP-internal (UNKNOWN). Report PAGE-622 reads eligibility/taken/balance (RPT-M15-01).

**Branches:** edit locked after approval; balance shortfall check: none in C#/JS (SP may). **Related BR:** 041-045, 058-062, 075, 076, 079. **Tests:** apply leave with To<From (expect none - GAP), edit approved application (locked), approve twice via both paths (double effect possible).
