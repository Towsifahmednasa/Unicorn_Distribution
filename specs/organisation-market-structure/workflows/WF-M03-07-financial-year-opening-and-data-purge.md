# WF-M03-07 — Financial-year opening balance and data purge/archive

Traced from `DoctorModule_UI/FinancialYearDeleteTableEntry.aspx(.cs)` (PAGE-038, detail there), `FinancialYearDeleteTableDAL.cs`, `UserInfoDAL.cs:39-160`. SP bodies absent; tables for FY defined by PAGE-039/040 (Tier B).

**Trigger/actors:** finance/IT admin at year end. No role check; no session user used.

**Steps**
1. Select financial year (`sp_GET_ArchiveFinancialYearList`); page checks `tblOpeningBalanceFinancialYearLog` for `Status='Completed'` → enables **Opening** (none) or **Delete/Archive** (exists) (BR-M03-068, 069).
2. **Opening**: date range from `tblFinancialYear` (end ≥ start) → `sp_opeingBalanceCreate(@FinancialYear=<id>,@FromDate,@ToDate)`; success reloads page (BR-M03-070, 072, 076).
3. **Production Table** mode: table (Order, Invoice, RX, DCR, TourPlan, Attendance, EXPENSE) → `sp_DeleteArchive<Table>Data(@FromDate,@ToDate)` on SalesDB (BR-M03-074). **Archive Table** mode: needs archive DB → `sp_DeleteArchiveDB<Table>Data(@DatabaseName,@FromDate,@ToDate)` (BR-M03-075). DA/Leave/Mileage Claim rejected (BR-M03-073).
4. Errors: rollback; SQL text shown to user (BR-M03-076).

**Data changed:** opening balances + log (UNKNOWN); transactional tables purged (per success text: archived to `…DeleteArchive` then deleted, or deleted from archive DB) — behaviour UNKNOWN.
**Status:** Opening `Completed` gates purge. **No confirmation / audit** (BR-M03-077).
**Tests:** FY with/without completed opening; unsupported table; archive without DB; SP failure rollback; forged postback to Delete without opening (server has no re-check).
