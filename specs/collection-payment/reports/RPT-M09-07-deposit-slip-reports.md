# RPT-M09-07 — Deposit list and deposit-slip reports

| Page | Data | SP / SQL | Params | Notes |
|---|---|---|---|---|
| `DepositList.aspx(.cs)` (PAGE-322) | `CompanywisebranchDal.LoadDepositList(from,to,companyId)` (`:82-104`) | inline SQL: `tblCompanyWiseDeposit DP ⨝ tblCompanyUnit CI ⟕ tblEmpGeneralInfo emp (EmpMasterCode) ⟕ tblBankInfo BNK WHERE ISNULL(IsDelete,0)=0` + `DepositDate` range + depot | From/To (default today), depot (DIC role must choose) | Columns EmpMasterCode, EmpName, EntryDate, AIT, DepositId, ComUnitName, BankName, BranchName, DepositDate, Amount, DepositType, AccountName, CheckNumber, CheckDate, Remarks; "Total Amount"; Excel `Deposit.xls`; delete button governed by BR-M09-050 |
| `DepositSlipReport.aspx` (PAGE-325) / `DepositSlipReport_T.aspx` (PAGE-326) | `TotalSummaryDAL.LoadDepositSlipSummary(from,to)` (:200-208) | `sp_Rep_DepopsitSlip_BusinessSummary(@fromdate,@todate)` — body NOT in repo | From/To; PAGE-325 also has a branch that runs `LoadDepositSlipSummaryProcess` → `sp_Rep_DepopsitSlip_BusinessSummaryClosingReport` (returns int; a "closing/process" step that presumably materialises data) | Excel export copy of the standard template; territory/zone/sales-center dropdowns loaded from user but not used in the call (INFERRED) |
| `BankDepositSAP.aspx` | `TotalSummaryBLL.LoadBankDepositSAP(from,to)` | SP not traced (UNKNOWN) | From/To | SAP bank-deposit view (page not in the 43-page list) |

## Reconciliation observations
- The deposit reports read `tblCompanyWiseDeposit` (uploaded slips), not `tblCustomerPay`; whether `sp_Rep_DepopsitSlip_BusinessSummary` compares collections with deposits is UNKNOWN. No in-repo code matches collection amounts to deposit amounts, so "deposit slip reconciliation" is limited to slip-total = Σ rows at upload time (BR-M09-051).
- Deleted deposits (soft delete) disappear from the list but the report SPs' handling of `IsDelete` is UNKNOWN.
