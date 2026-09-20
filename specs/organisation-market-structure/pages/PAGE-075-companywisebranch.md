---
page_id: PAGE-075
module: M03
page: CompanyWiseBranch
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-075 — CompanyWiseBranch (actually "Deposit slip entry")

> **Evidence level:** Tier A. `CompanyWiseBranch.aspx` (636 lines), `.aspx.cs` (410 lines), `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs`, `ClsCommonInternalDAL.SaveAction`, `SeedDataDAL.GetBAnkInfoById`, `CommonDataLoad.GetMIOInfo_SC_Rpt` read. **Mis-classified page:** despite the file name (`CompanyWiseBranch`) and its folder placement in the organisation module, it is a **bank deposit-slip entry** (finance/collection). Menu label in the legacy master page: "Deposit Slip Entry" (`Solution.Web/MasterPages/MasterPage.master:1214`). Kept here because the module scope list contains it.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-075 / M03 (by file name; functionally finance) |
| Name | CompanyWiseBranch (breadcrumb "Deposit slip entry", `.aspx:56`) |
| Kind | Entry / Transaction — records a sales-center bank deposit |
| Folder / master | SInventory_UI / NewMasterPage.master |
| Class | `SInventory_UI_CompanyWiseBranch` |
| Legacy | Contains a second, hidden UpdatePanel (`UpdatePanel1` `Visible="False"`, `.aspx:313`) with an older Button-based form (`submitButton`, `clearButton`) and hidden placeholder dropdowns `DropDownList1/2/4`, `ddlBranch` |

## 2. Purpose
CONFIRMED: record that a **MIO** (field officer) of a **Sales Center (company unit / depot)** deposited an amount (plus AIT — advance income tax withheld) into a company bank account, by deposit type (Cash, Online, Cheque, Market Adjustment, DD/Payorder), with cheque details where applicable.

## 3. Business Objective
INFERRED: capture collections so customer/MIO ledgers can be settled against bank credits; deposits are later listed/deleted on `DepositList.aspx` (delete allowed only within the last 60 days — `UpdateDepositInfo`, see PAGE list owner).

## 4. Navigation
"View" button and success → `DepositList.aspx` (`.cs:187,~177`); `cancelButton` → self (`.cs:182`); legacy menu link `../SInventory_UI/CompanyWiseBranch.aspx` (`MasterPage.master:1214`).

## 5. Route
`~/SInventory_UI/CompanyWiseBranch.aspx`

## 6. Source Files
`Solution.Web/SInventory_UI/CompanyWiseBranch.aspx(.cs)`; `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs`; `Library.DAL/InternalCls/ClsCommonInternalDAL.cs` (`SaveAction` :591); `Library.DAL/DoctorModule_DAL/SeedDataDAL.cs` (`GetBAnkInfoById` :35); `CommonDataLoad.GetMIOInfo_SC_Rpt` (:318); DAO `Library.DAO/SInventory_Entities/CompanyWiseDepositDao`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `companyNameDropDownList` (AutoPostBack) | Sales Center | **Yes** (`.cs:196`) | index 1 (first real unit) selected on load (`.cs:27-38`) | – | inline SQL `tblCompanyUnit` ∩ `tblUserCompanyUnit(UserId)` (`CompanywisebranchDal.LoadCompany`) | tooltip |
| `CheckBoxList1` (AutoPostBack, JS mutual exclusion `MutExChkList`) | Deposit Type: Cash / Online / Cheque (value `Check`) / Market Adjustment / DD/Payorder (value `DD`) | ≥1 (`.cs:233`) | none | choosing Cash/Online hides Branch, Cheque No, Cheque Date rows (`.cs:373-388`) | static | alert "You should select deposit type !!!" |
| `ddlMIO` | MIO Name | **Yes** (`.cs:~240`) | placeholder | reloaded when company changes | `sp_CS_MIOInfo_BySC(@DCId)` → `ValueId`/`TextName` | tooltip |
| `ddlBank` (AutoPostBack) | Bank Name | **Yes** (all types) | placeholder | reloaded on company change | inline `SELECT BankId,BankName FROM tblBankInfoNew WHERE IsActive=1 AND ComUnitId=@ComUnitId` | tooltip |
| `accNameTextBox` | Account Name | Yes (readonly) | filled on bank select | `ReadOnly=true` | `sp_GET_BAnkInfoById_ById(@id)` → `BankAccountNumber` | tooltip |
| `branchTextBox` | Branch Name | Yes except Cash/Online | filled from bank (`BranchName`) | editable | same | tooltip |
| `chkTextBox` | Check Number | Yes except Cash/Online | filled from bank (`chkAccNo`) | editable | same | tooltip |
| `chkDateTextBox` | Check Date | Yes except Cash/Online | today ("dd MMMM, yyyy") | datepicker | – | tooltip |
| `dateTextBox` | Deposit Date | **Yes** | today | datepicker; `dateTextBox_TextChanged` is **not wired** (no `OnTextChanged`) | – | tooltip |
| `amount` | Amount | **Yes** | empty | – | – | tooltip |
| `AITTextBox1` | AIT | **Yes** (non-empty) | empty | – | – | tooltip |
| `remarksTextBox` | Remarks | no | empty | multiline | – | – |
| `btnSave` | Submit | – | – | `sweetAlertConfirm_Submit` | – | – |
| `cancelButton` / `viewLinkButton` | Reset / View | – | – | – | – | – |

## 8. Field Mapping
| UI | DTO (`CompanyWiseDepositDao`) | Code | SP param (`sp_I_Diposit_New`) |
|---|---|---|---|
| company | `CompanyId` | `Convert.ToInt32` (`.cs:139`) | `@CompanyId` |
| amount | `Amount` | `Convert.ToDecimal(amount.Text.Trim())` | `@Amount` |
| AIT | `AIT` | `Convert.ToDecimal(AITTextBox1.Text.Trim())` | `@AIT` |
| `Session["LoginName"]` | `EntryBy` (**login name**, not user id) | `.cs:144` | `@EntryBy` |
| now | `EntryDate` | `DateTime.Now` | `@EntryDate` |
| deposit date | `DepositDate` | `Convert.ToDateTime(dateTextBox.Text.Trim())` | `@DepositDate` |
| – | `IsDelete=false`, `IsExcelUpload` (default) | – | `@IsDelete`, `@IsExcelUpload` |
| MIO | `MIOId` (int?) | `ddlMIO.SelectedIndex>0` | `@MIOId` |
| remarks | `Remarks` | – | `@Remarks` |
| deposit type | `DepositType` = ticked item **Text** trimmed ("Cash","Online","Cheque","Market Adjustment","DD/Payorder") | `.cs:~151` | `@DepositType` |
| bank / account | `BankId`, `AccountName` | always set | `@BankId`, `@AccountName` |
| branch / cheque no / cheque date | `BranchName`, `CheckNumber`, `CheckDate` | only when type ∉ {Cash, Online} | `@BranchName`, `@CheckNumber`, `@CheckDate` (NULL otherwise) |
| – | `DepositCode` | not set | `@DepositCode` NULL |
Output parameter `@DepositId` (int) is returned as the save result.

## 9. Dropdown Mapping
See §7. Note the Cash/Online branch condition compares `DepositType == "Cash" || "Online"` on the *Text*, while Cheque's Text is `"Cheque"` (Value "Check").

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Submit | `submitButton_Click` `.cs:135` | `Validattion()` → `CompanywisebranchDal.SaveDepositInfo` → `ClsCommonInternalDAL.SaveAction("sp_I_Diposit_New", params, "@DepositId")` | `sp_I_Diposit_New` | insert (UNKNOWN tables; list reads `tblCompanyWiseDeposit`) |
| Company change | `companyNameDropDownList_SelectedIndexChanged` `.cs:390` | `GetMIOInfo_SC_Rpt`, `LoadBank` | `sp_CS_MIOInfo_BySC`, inline | read |
| Bank change | `ddlBank_OnSelectedIndexChanged` `.cs:86` | `SeedDataDAL.GetBAnkInfoById` | `sp_GET_BAnkInfoById_ById` | read |
| Deposit type change | `CheckBoxList1_SelectedIndexChanged` `.cs:373` | toggles rows | – | UI |
| Reset | `cancelButton_Click` `.cs:182` | redirect self | – | – |

## 11. Frontend Flow
UpdatePanel form; JS `MutExChkList` makes the CheckBoxList behave like radio buttons (defined `.aspx:29`, body not analysed further); server `CheckBoxList1_SelectedIndexChanged` hides/shows Branch, Cheque No, Cheque Date rows. Success alert → `DepositList.aspx`.

## 12. Backend Flow
`submitButton_Click`: `Validattion()`; build DAO (`EntryBy = Session["LoginName"]`); `DepositType` = the last ticked item's Text; if type is Cash or Online → `BankId`,`AccountName` only; otherwise also `BranchName`, `CheckNumber`, `CheckDate`; `SaveDepositInfo(...) > 0` → success alert. **No else branch**: when the DAL returns 0 the user sees nothing (SaveAction catches every exception and returns 0 — `ClsCommonInternalDAL.cs:591-604`, uses Enterprise-Library connection "SolutionConnectionStringSSIDB", 60 s timeout, own transaction semantics UNKNOWN). Numeric/date conversions (`Convert.ToDecimal`, `Convert.ToDateTime`) are unguarded → invalid input raises an unhandled exception.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL (page → DAL). `CommonDataLoad`, `SeedDataDAL` static instances.

## 15. Repository Mapping
| DAL | Source | SQL/SP |
|---|---|---|
| `CompanywisebranchDal.LoadCompany` | CompanywisebranchDal.cs | inline `select ComUnitId, ComUnitName from tblCompanyUnit WHERE ComUnitId IN (SELECT CompanyUnitId FROM dbo.tblUserCompanyUnit WHERE UserId=@UserId)` |
| `LoadBank` | same | inline `SELECT BankId,BankName FROM tblBankInfoNew where IsActive=1 and ComUnitId=@ComUnitId` |
| `SaveDepositInfo` | same | `sp_I_Diposit_New` (17 params, out `@DepositId`) |
| `GetBAnkInfoById` | SeedDataDAL.cs:35 | `sp_GET_BAnkInfoById_ById(@id)` |
| `GetMIOInfo_SC_Rpt` | CommonDataLoad.cs:318 | `sp_CS_MIOInfo_BySC(@DCId)` |
Same DAL also has `LoadDepositList` (inline join `tblCompanyWiseDeposit`, `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblBankInfo` — note `tblBankInfo` vs `tblBankInfoNew` in the entry page), `UpdateDepositInfo` (soft delete within last 60 days), `SaveTargetValueInfo` (`usp_InsertTerritoryData`) used by other pages.

## 16. Stored Procedure Mapping
`sp_I_Diposit_New` (write; body NOT in repo), `sp_GET_BAnkInfoById_ById`, `sp_CS_MIOInfo_BySC` (read; bodies NOT in repo).

## 17. Database Mapping
Reads: `tblCompanyUnit`, `tblUserCompanyUnit`, `tblBankInfoNew` (inline SQL). Writes: via `sp_I_Diposit_New` — table UNKNOWN (the list DAL reads `tblCompanyWiseDeposit`, INFERRED as target).

## 18. Validation Matrix
| Field | Rule | Message | BR |
|---|---|---|---|
| Sales Center | required | tooltip | BR-M03-089 |
| AIT | non-empty (numeric parse unguarded) | tooltip | BR-M03-089 |
| Deposit Type | ≥1 | alert "You should select deposit type !!!" | BR-M03-089 |
| MIO | required | tooltip | BR-M03-089 |
| Bank + Account | required (all types) | tooltip | BR-M03-090 |
| Branch, Cheque No, Cheque Date | required unless Cash/Online | tooltip | BR-M03-090 |
| Deposit Date, Amount | required | tooltip | BR-M03-089 |
| Amount > 0 / numeric / date ≤ today | **not checked** | – | BR-M03-093 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-089 | Mandatory: Sales Center, AIT (may be 0 but not blank), Deposit Type, MIO, Deposit Date, Amount. Validation order: company → AIT → type → MIO → bank block → date → amount. | CompanyWiseBranch.aspx.cs:191-329 | page | CONFIRMED |
| BR-M03-090 | Cash/Online deposits need bank + account only; Cheque, Market Adjustment and DD/Payorder additionally need branch, cheque number and cheque date. Account name is read-only, auto-filled from the selected bank; branch/cheque no default from the bank record. | .cs:86-105,135-180,246-329 | page | CONFIRMED |
| BR-M03-091 | Sales Center list = user's assigned units (`tblUserCompanyUnit`); index 1 is auto-selected on load; MIO list and bank list are re-scoped to the chosen unit (`sp_CS_MIOInfo_BySC`, `tblBankInfoNew.IsActive=1`). | .cs:27-38,390-410; CompanywisebranchDal.cs | page/DAL | CONFIRMED |
| BR-M03-092 | `DepositType` persisted = ticked item's Text trimmed; if several ticked the last wins (JS enforces single selection). | .cs:~151-158 | page | CONFIRMED |
| BR-M03-093 | No numeric/range/date validation: negative or zero amount, future deposit date, non-numeric text → exception (unhandled) or accepted; save failure returns 0 silently. The "current month only" date rule (`dateTextBox_TextChanged`) exists but is not wired. | .cs:44-83,135-180; ClsCommonInternalDAL.cs:591-604 | page/DAL | CONFIRMED |
| BR-M03-094 | `EntryBy` stores the **login name** (`Session["LoginName"]`), unlike most module pages that store `UserId`. | .cs:144 | page | CONFIRMED |

## 20. Permission Matrix
Session read: `UserId` (company list), `LoginName` (EntryBy). Data scoping only through `tblUserCompanyUnit`. No role checks; menu-only; no `<authorization>`.

## 21. Status Lifecycle
DAO `IsDelete=false` on create. Soft delete (`IsDelete`, `DeleteBy`, `DeleteDate`) is done elsewhere, permitted only for deposits dated within the last 60 days (`UpdateDepositInfo`). No approval status on this page.

## 22. Error Handling
`SaveAction` swallows all SQL errors (returns 0) → silent no-op; other conversions unhandled; loaders in `try/catch{}`.

## 23. Audit
`EntryBy` (login name), `EntryDate` (server now); delete audit fields on the list side.

## 24. Integration
Feeds the collection/ledger side (out of module). Uses a different connection string name (`SolutionConnectionStringSSIDB`) via Enterprise Library — see GAPS.

## 25. Reports
None here (list page `DepositList.aspx`).

## 26. Dependencies
`tblBankInfoNew`, MIO master, company-unit master, shared JS.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Load page | first company auto-selected, MIO+bank lists loaded | BR-M03-091 |
| TC-02 | Submit with no type | alert "You should select deposit type !!!" | BR-M03-089 |
| TC-03 | Cash deposit, bank chosen | saves without branch/cheque data | BR-M03-090 |
| TC-04 | Cheque deposit, empty cheque no | Check Number flagged | BR-M03-090 |
| TC-05 | Amount "abc" | unhandled exception | BR-M03-093 |
| TC-06 | Negative amount | accepted by page | BR-M03-093 |
| TC-07 | Force SP failure | no message shown | BR-M03-093 |
| TC-08 | Deposit date next year | accepted | BR-M03-093 |
| TC-09 | Select bank | account, branch, cheque no auto-filled | BR-M03-090 |
| TC-10 | Check DB record | `EntryBy` = login name | BR-M03-094 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CompanyWiseBranch.aspx(.cs)`, `Library.DAL/SInventory_DAL/CompanywisebranchDal.cs`, `Library.DAL/InternalCls/ClsCommonInternalDAL.cs:591-640`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Page identity | Name says "Company wise branch" | it is deposit slip entry | – | CONFLICT (naming) | §Header |

## 30. Known Gaps
Misnamed/misfiled page (belongs with finance); hidden legacy form; unwired month rule; unguarded parsing; silent failure; `tblBankInfoNew` vs `tblBankInfo` inconsistency between entry and list; EntryBy = login name; SP body absent (GAP-M03-001).

## 31. Change Impact
`CompanywisebranchDal` is shared with `DepositList.aspx` and the target-excel upload (`SaveTargetValueInfo`). `sp_I_Diposit_New` feeds the deposit list and collection reports (outside module).
