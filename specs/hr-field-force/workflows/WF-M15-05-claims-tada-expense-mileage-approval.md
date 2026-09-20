# WF-M15-05 Field-force claims (DA/TADA, expense, mileage) - entry, approval, reporting

**Actors:** field employee/HR clerk (entry), approver chain (AM -> DZSM -> NSM/HR final, per `RoleTypeId`), HR (reports). **Set-up pages:** ExpenseType PAGE-572, MonthlyAllowance PAGE-583, Transport PAGE-605, TADA market rules PAGE-589, Tour set-up (WF-M15-04). **Entry:** ExpenseClaim PAGE-570, MileageClaim PAGE-581, TADAClaimEdit PAGE-587 (claims are also created by the mobile app - `sp_Webapi_*`, UNKNOWN). **Lists:** PAGE-571, 582, 588. **Approval (cross-module `Approval_UI`)**: `ExpenseApprovalList`, `MillageApprovalList`, `DAApprovalList`. **Reports:** RPT-M15-02.

## Entry
- **Expense** (`ExpenseClaim.aspx`): pick expense type -> JS loads its custom fields (`GetExpenseField_ByExpenseType` -> `_dataLoad.GetExpenseField_ByExpenseType`), user adds field/value rows; required: date, type, employee, amount, >=1 row (client only). `Save_ExpenseClaim` -> `Setup2DAL.Save_ExpenseClaim`: `sp_Save_ExpenseClaimMaster` or `sp_Update_ExpenseClaim`, image (base64) written as `<prefix><id>.jpg` to `AppPrimaryDAL.GetImagePath("Expense")` (errors swallowed), `sp_Save_ExpenseClaimDetails` per field (BR-M15-065..068). Edit lock in JS: if `ApprovalStatus=="2"` the Save button hides unless `Rid==5`.
- **Mileage** (`MileageClaim.aspx`): `Save_MileageClaim` -> `sp_Save_MileageClaim`/`sp_Update_MileageClaim` with KM, meter reading, tour type, transport id, date, market ids; image file "Mileage" (BR-M15-074). Amount = km x transport rate is **not computed in C#**; the rate field `AllowedMilagePerKM` is on Transport (PAGE-605); report shows `AllowedMileageInKM`, `mileageAmount`.
- **DA/TADA** (`TADAClaimEdit.aspx`): prefilled from tour plan, `sp_Save_TadaClaimMaster` (BR-M15-034..040).

## Approval chain (as coded in Approval_UI pages and PAGE-564)
1. Grid rows come from SPs returning current `Step`, `ToEmpId`, `ToRoleTypeId` (routing computed in SP - UNKNOWN).
2. Buttons: role types 5 (and 4, 14, 9, 3 on some pages) see actions on all rows; other roles only rows whose `ToRoleTypeId == own RoleTypeId`, otherwise badge `Waiting for Another Approver`.
3. Approve: `Accepted` if `RoleTypeId==5` (leave/DA/expense/mileage pages also if `EmpInfoId=="496"` or role 9) else `Verified`; reject `Rejected`. Written via `sp_webapi_SaveAppLog(@ApprovalId=0,@Date,@FromEmpId=approver EmpInfoId,@ToEmpId,@TableId=claim id,@Status,@Type,@Step=current+1,@MenuId,...)`. Types/MenuIds: `ExpanseClaim`/356, `Millege`/372, `DA Claim`/376, Leave/381, TourPlan/371 (typed "Leave"), Attendance (PAGE-564: typed "Expense"/356).
4. Claim master `ApprovalStatus` column (value `2` = final approved in ExpenseClaim JS; list status filters use `sp_GET_ActionStatusList`).
(BR-M15-075; GAP-M15-013, 014.)

## Reporting
Monthly claim: `sp_RPT_MonthlyExpense` (list), `sp_Get_MonthlyExpenseEmpWiseMaster_Final` (per-day rows: date, market, tour type, `daAmount`, `AllowedMileageInKM`, `mileageAmount`, `expenseAmount`, `totalAmount`), `sp_Get_MonthlyExpenseEmpWiseTotal`, `sp_Webapi_Get_EmpAllawance_MonthYear` (monthly allowances of the employee), `sp_Webapi_Get_TourPlanBalanceEmp` -> Crystal `crpEmpExpense.rpt` (BR-M15-078). Monthly allowance assignment: PAGE-583 (name, amount, role, employees) and employee record allowance list (PAGE-613).

## Calculation summary
Amounts are **not calculated in web code**: DA = plan/rule value (editable), mileage = SP (KM x rate), expense = typed amount, total = SP. All formulae UNKNOWN (GAP-M15-025).

**Tests:** approver role 5 approves (status Accepted); AM approves (Verified, row now waits for next role); reject; claim edit after approval (allowed in DA page).
