# WF-M15-03 Yearly leave balance processing

**Trigger / actor:** HR clicks "Process Leave" on `LeaveProcess_UI/YearlyLeaveProcess.aspx` (PAGE-611). Page text: "This process will set employee yearly leave balance based on leave type and yearly leave allocation."
**Preconditions:** leave types with days (PAGE-577), leave configuration (PAGE-578) and employees exist. No precondition is checked in code.

## Steps (exactly as coded)
1. `Save()` -> `FinalSave()` in page JS (`YearlyLeaveProcess.aspx`): shows spinner, POST `YearlyLeaveProcess.aspx/SaveYearlyLeave` with empty body (a `jsonData["year"]='2022'` variable is built but never sent).
2. Page-method `SaveYearlyleave()` (`YearlyLeaveProcess.aspx.cs`) -> `YearlyLeaveprocessDal.SaveYearlyleave(Convert.ToInt32(Session["UserId"]))` -> `accessManager.SaveData("sp_Process_YearlyLeaveBalance", @ProcessBy=UserId)` (`Library.DAL/MasterSetup_DAL/YearlyLeaveprocessDal.cs`).
3. Return `ResultInfo.isSuccess` -> `successalert('Operation successful!')` reload, else `faildalert('Operation Faild!')`. Exceptions rolled back and rethrown (client `error` handler shows failure).

## What is NOT known
Everything the SP does: which year/fiscal-year, per-employee accrual (monthly `DaysPerMonthly` by joining-date band? proration by joining date? carry-forward of previous annual leave - the report has a `PreviousAL` column), probation eligibility, handling of holidays, re-run behaviour. All UNKNOWN (GAP-M15-001, 016). Leave balance readers: `sp_GET_EmployeeLeaveBalance` (columns `YearlyLeaveBalance`, `LeaveBalance`) and report SPs `sp_Webapi_LeaveReport_Details/Summary` (columns Elli*/TK*/ab*/*Blnc, `PreviousAL`, FiscalYear).

## Branches / risks
Run twice -> effect unknown; no confirmation dialog; no audit besides `@ProcessBy`; case mismatch `SaveYearlyLeave` vs `SaveYearlyleave` (works only if page-method lookup is case-insensitive - INFERRED).

**BR:** BR-M15-063, 064. **Pages:** 611, 577, 578, 622. **Tests:** run once (expect balances set), run twice (verify no doubling), non-admin invocation via direct POST.
