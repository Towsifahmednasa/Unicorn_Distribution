# WF-M09-05 — Bank deposit slip upload, listing and deletion (deposit reconciliation)

Confidence: CONFIRMED for `DepositSlipExcelUpload.aspx.cs` (lines 236-676) and `DepositList.aspx.cs`, `CompanywisebranchDal.cs`; UNKNOWN for `sp_I_Diposit_New`, `sp_Get_DepositCodeByDIC`, `sp_GET_BAnkInfoById_ByIdAcc/ByIdExcel`, `sp_Get_BankSAPMappingAccounTNo`, `sp_Get_SAP_EmpSApCodebyTerritory`, deposit-slip report SPs. Tier-B pages PAGE-322/323 were not upgraded (cap); rules recorded here as BR-M09-050…052.

## Trigger & actors
Depot/DIC finance user records the bank deposit slips of cash collected from customers/MIOs by uploading an Excel file; managers list/export deposits; two named users may delete.

## Preconditions
Bank + bank account of each row exist for the user's depot (`Session["ComUnitId"]`) and are SAP-mapped; total of the slip typed by the user.

## Steps
1. **Upload** (`DepositSlipExcelUpload.aspx`) `btnUpload_Click` → `ExcelToGrid` (:534): saves file to `~/ExcelFiles/`, OLE DB read of first sheet, binds `loadGridView`, sums column 8 (amount) into `lblCount`; for each row checks `GetBankSAPMappingAccounTNo(account)` and `GetBankAccounTNo(account, ComUnitId)` (bank id → hidden field); rows failing are coloured red.
2. **Validate** (`Validattion` :386): rows exist; cells 3-9 non-empty; bank id ≠ 0 ("Bank Name Not Found!"); account mapped in SAP mapping and for the depot ("Account Not mapped!!"); typed **Total Amount** must equal Σ grid amounts (BR-M09-051) — this is the only reconciliation control.
3. **Save** (`submitButton_Click` :238): per row: territory code → `GetSAP_EmpSApCodebyTerritory` → MIOId (0 if unknown); DepositDate via flexible parse (invalid ⇒ default date silently); company = `Session["ComUnitId"]`; bank id via `GetBAnkInfoByIdAcc` (empty catch ⇒ 0); DepositCode from `sp_Get_DepositCodeByDIC`; `CompanywisebranchDal.SaveDepositInfo` → `sp_I_Diposit_New` (tblCompanyWiseDeposit). Success message depends on the last row's id only (BR-M09-052).
4. **List** (`DepositList.aspx`) → `LoadDepositList(from,to,companyId)`: `tblCompanyWiseDeposit ⨝ tblCompanyUnit ⟕ tblEmpGeneralInfo ⟕ tblBankInfo WHERE ISNULL(IsDelete,0)=0` by `DepositDate` range and depot; grid total; Excel export. DIC role must pick a depot.
5. **Delete** — `loadGridView_RowCommand("EditData")`: allowed only for LoginName `51419000000` or `50588` and deposit dated within [today−60d, today]; DAL `UPDATE tblCompanyWiseDeposit SET IsDelete=1, DeleteBy, DeleteDate WHERE DepositId=@… AND DepositDate BETWEEN today−60 AND today` (BR-M09-050).
6. **Reports**: DepositSlipReport(_T) → `sp_Rep_DepopsitSlip_BusinessSummary` (+ `…ClosingReport` process); BankDepositSAP → deposit data for SAP.

## Data created / changed
tblCompanyWiseDeposit (Amount, AIT, DepositDate, DepositType, BankId, AccountName, MIOId, DepositCode, IsExcelUpload=true, IsDelete, Entry/Delete audit).

## Reconciliation (what exists / does not)
No code links deposits to `tblCustomerPay` collections (no join, no matching); reconciliation between collection and banked amounts is done only by reports (`sp_Rep_DepopsitSlip_BusinessSummary`, SP not in repo) — UNKNOWN. The only in-code check is slip total = Σ rows.

## Branches / exceptions
Unmapped bank/account → blocked; date parse failure → row saved with default date (`DateTime.MinValue` semantics on DAO) — INFERRED; `Console.WriteLine` used for invalid date (no user feedback); duplicate protection only inside `sp_I_Diposit_New` (UNKNOWN) so message "Already Exist!" is shown when last id ≤ 0.

## Related BR
BR-M09-050, 051, 052.

## Related pages
PAGE-322, 323, 324 (old), 325, 326, PAGE-314 (contains the same dead deposit code), BankDepositSAP.

## Tests
Upload with total mismatch (blocked); unmapped account (row red); delete by non-listed user (message); delete 61-day-old slip (message and SQL guard); upload then list totals.
