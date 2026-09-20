# WF-M14-03 - Territory target Excel upload

Module: M14. Related BR: BR-M14-021, 022, 023, 028. CONFIRMED (SP internals UNKNOWN).

## Trigger and actors
Sales-admin user with menu access to "Monthly Target Upload" (`SInventory_UI/TargetExcelUpload.aspx`). Input: Excel file with columns `TerritoryCode, TargetValue, MonthValue, YearValue, FinancialYear`.

## Preconditions
- Territory code must map to an employee (`sp_Get_Chk_EmpCode`).
- No existing target for (territory, month, year) (`sp_Get_Chk_EMPDataExistCheck`).
- Jet/ACE OLEDB provider installed on the web server.

## Steps
1. `btnUpload_Click` -> `ExcelToGrid` (`TargetExcelUpload.aspx.cs:368`): save file to `~/ExcelFiles/<name>`, read first sheet via OLEDB, show in grid; rows unmapped or already existing are painted `#ff9999`.
2. `submitButton_Click` (`:228`) -> `Validattion()` (`:277`): required fields, employee-mapped, not-duplicate. First failure stops with a message.
3. For each row: `CompanywisebranchDal.SaveTargetValueInfo` -> `ClsCommonInternalDAL.SaveAction("usp_InsertTerritoryData", ..., "@SL")` (own connection per row, no shared transaction, exceptions swallowed to 0).
4. `id` (last row) > 0 -> `successalert(..., 'TargetExcelUpload.aspx')`; else `Already Exist!`.
5. Review/correct on `TargetExcelUploadList.aspx` (`sp_Get_TerritoryTargetList @Parm` filtered by fiscal year) -> Edit (`TargetExcelUploadEdit.aspx?MID=`) or Delete (`Delete_Target` -> `sp_DEL_TargetInfo @SL`).

## Data created / changed
Rows behind `usp_InsertTerritoryData` (territory monthly target with EntryBy = LoginName). Uploaded file left in `~/ExcelFiles/`.

## Branches / exceptions
- Non-Excel / unreadable file -> `Please fill up all required data !!`.
- Partial failure invisible (BR-M14-022).
- Rows are only validated one-by-one at submit; the grid colouring at upload time is advisory.

## Postconditions
Territory targets stored for the month; blocked from re-upload for same month.

## Tests
TC-554-01..08.

## Related pages
PAGE-554, 555, 556.
