---
page_id: PAGE-190
module: M05
page: CustomerCreditLimitEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-190 — CustomerCreditLimitEntry

> **Evidence level:** Tier A. `.aspx.cs`, `CreditLimitDAL`, `CreditLimitDAO`, autocomplete web method, consumer SP `sp_LoadOrderListForOrderCreationbyTerri` (body in `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1225-1365`) and `InvoiceCreationByOrder.aspx.cs` were read. DDL of `tblCustomerCreditLimit` is not in the repo (columns inferred from INSERT/UPDATE/SELECT text). All persistence is inline SQL.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-190 |
| Module | M05 |
| Page | CustomerCreditLimitEntry (create + edit) |
| Kind | Entry / Edit (`?ID=<CreditLimitId>` switches to edit) |
| Folder / class | `SInventory_UI` / `SInventory_UI_CustomerCreditLimitEntry` |
| Related | PAGE-191 `CustomerCreditView` (list/edit/delete), PAGE-189 `CustomerCreditLimitApproval` (approve/reject) |

## 2. Purpose
Define, per **company** and **customer**, a credit limit (maximum outstanding amount) and a validity period in days, or change an existing one. New/edited limits go to `Posted` (pending approval). CONFIRMED (`CustomerCreditLimitEntry.aspx.cs:111-136,173-217`).

## 3. Business Objective
Control credit sales: once a limit is Approved, invoice generation for that customer is blocked in the order-to-invoice screen when the customer's dues plus the new order exceed the limit or when its oldest unpaid invoice is older than the allowed days (see BR-M05-025). CONFIRMED via `sp_LoadOrderListForOrderCreationbyTerri`.

## 4. Navigation
Menu DB-driven (UNKNOWN). "View" link → `CustomerCreditView.aspx`; after save/update → `CustomerCreditView.aspx` (`successalert(...,'CustomerCreditView.aspx')`). Edit entered from PAGE-191 grid (`CustomerCreditView.aspx.cs:88`).

## 5. Route
`~/SInventory_UI/CustomerCreditLimitEntry.aspx[?ID=n]`.

## 6. Source Files
`Solution.Web/SInventory_UI/CustomerCreditLimitEntry.aspx(.cs)`; `Library.DAL/SInventory_DAL/CreditLimitDAL.cs`; `Library.DAO/SInventory_Entities/CreditLimitDAO.cs`; `Solution.Web/App_Code/SInventoryWebService.cs:33` (`GetCustomernew`, autocomplete, `SInventoryWebService.asmx` in the page folder); consumer `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1389-1398,1523-1532`, `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs:622-645`.

## 7. UI Control Inventory
| Control | Label | Required (server) | Default | Visible / enabled | Data source | Error |
|---|---|---|---|---|---|---|
| `companyDropDownList` | Company | yes (`SelectedIndex>0`) | first item | — | `SELECT CompanyId, CompanyName FROM tblCompanyInfo` (`CreditLimitDAL.cs:63-67`) | "Please select a company." |
| `customerTextBox` (+ AutoComplete, hidden `customeridHiddenField`) | Customer Name | yes (hidden id must be set) | empty | AutoPostBack | web method `GetCustomernew(prefix)` (top 10, code/name LIKE, returns `Code - Name:Id`) | "Please select a valid customer." |
| `limitAmntTextBox` | Limit Amount | yes, decimal >0 | — | client filter digits and "." | user | "Please input a valid credit limit amount." |
| `limitDaysTextBox` | Validity (Days) | yes, integer >0 | — | client filter digits | user | "Please input a valid day limit." |
| `isActiveCheckBox` | Is Active | — | checked on create | — | user | — |
| `saveButton` | Save | — | — | visible when no `ID` | — | `saveButton_Click` |
| `updateButton` | Update | — | — | visible when `ID` present | — | `updateButton_Click` |
| `resetButton` | Reset | — | — | — | — | with `ID` ⇒ redirect to blank entry; else `ClearForm()` |
| `viewLinkButton` | View list | — | — | — | — | redirect |
| `creditHiddenField` | — | — | `QueryString["ID"]` | — | — | — |

## 8. Field Mapping
| UI control | DAO property | Insert/Update param | `tblCustomerCreditLimit` column |
|---|---|---|---|
| companyDropDownList | CompanyId | literal in SQL | CompanyId |
| customeridHiddenField | CustomerMasterId | literal | CustomerMasterId |
| limitAmntTextBox | LimitAmount (decimal) | literal | LimitAmount |
| limitDaysTextBox | DayLimit (int) | literal | DayLimit |
| isActiveCheckBox | IsActive | literal (`'True'/'False'`) | IsActive |
| Session["LoginName"] | EntryBy / UpdateBy | literal | EntryBy / UpdateBy |
| DateTime.Now | EntryDate / UpdateDate | literal (culture string) | EntryDate / UpdateDate |
| constant | ActionStatus `"Posted"` | literal `'Posted'` | ActionStatus |
Source `CreditLimitDAL.SaveCreditLimit` `:17-35`, `UpdateCreditLimit` `:115-121`. Not written here: ApprovedBy, ApprovedDate, StartDate, EndDate (columns exist per `GetCustomerCreditInfo` select).

## 9. Dropdown Mapping
`companyDropDownList` ← `CreditLimitDAL.GetCompanyDropdownList` via `ClsCommonInternalDAL.LoadDropDownValue(…,"SSIDB")` (generic SQL proc `ExecuteAllSqlQueryByStoreProcedure`, body UNKNOWN); text `CompanyName`, value `CompanyId`; table `tblCompanyInfo`.

## 10. Button Action Mapping
| Button | Handler | DAL | SQL | DB effect |
|---|---|---|---|---|
| saveButton | `saveButton_Click` `:173` | `CheckExistorNot`, `SaveCreditLimit` | `SELECT … tblCustomerCreditLimit` join; `INSERT INTO dbo.tblCustomerCreditLimit(CompanyId,CustomerMasterId,LimitAmount,DayLimit,IsActive,EntryBy,EntryDate,ActionStatus) VALUES(…,'Posted')` | new pending row |
| updateButton | `updateButton_Click` `:196` | `CheckExistorNotUpdate`, `UpdateCreditLimit` | `UPDATE tblCustomerCreditLimit SET CompanyId,CustomerMasterId,LimitAmount,DayLimit,IsActive,UpdateBy,UpdateDate,ActionStatus='Posted' WHERE CreditLimitId=…` | row back to pending |

## 11. Frontend Flow
On first load: company list; if `ID` → `LoadCreditInfo` (`GetCustomerCreditInfoById`, fills company, customer name, hidden id, amount, days, active) and swaps buttons. Customer field: AjaxControlToolkit autocomplete → user picks `Code - Name:Id`; `TextChanged` splits on `:` — `parts[0]`→ text, `parts[1]`→ hidden id (`:219-233`); text without `:` clears the id ⇒ validation "Please select a valid customer." (a customer name containing `:` would break parsing — INFERRED). Client filters restrict numeric input. select2 init in `pageLoad`.

## 12. Backend Flow
Save: `ValidateForm()` → `IsDuplicate(false)` (`CheckExistorNot`: any row with `IsActive=1` for same company+customer) → `SaveCreditLimit(BuildModel(false))` → success/`Operation failed.` Update: `ValidateForm()` → `IsDuplicate(true)` (`CheckExistorNotUpdate`: same but `CreditLimitId != id`) → `UpdateCreditLimit`. Statement execution via `ClsCommonInternalDAL.SaveDataByInsertCommand/UpdateDataByUpdateCommand` (generic `ExecuteAllSqlQueryByStoreProcedure`, single statement, no explicit transaction; catch blocks return false). CONFIRMED.

## 13. API Mapping
Consumes ASMX `SInventoryWebService.GetCustomernew` (`[WebMethod(EnableSession=true)]`): `SELECT TOP 10 (CustomerCode+' - '+CustomerName+':'+CustomerMasterId) FROM tblCustMaster WHERE CustomerCode IS NOT NULL AND (CustomerCode LIKE @p OR CustomerName LIKE @p)` plus raw `Session["MOParam"]` appended when set (injection surface, GAP-M05-011).

## 14. Service Mapping
No BLL: page → `CreditLimitDAL` directly.

## 15. Repository Mapping
`CreditLimitDAL.{GetCompanyDropdownList, GetCustomerCreditInfoById, CheckExistorNot, CheckExistorNotUpdate, SaveCreditLimit, UpdateCreditLimit}`.

## 16. Stored Procedure Mapping
No named SP. Generic executor `ExecuteAllSqlQueryByStoreProcedure` (body NOT in repo) runs the concatenated SQL text.

## 17. Database Mapping
Read: `tblCustomerCreditLimit`, `tblCompanyInfo`, `tblCustMaster`. Write: `tblCustomerCreditLimit` (INSERT/UPDATE). Downstream readers: `sp_LoadOrderListForOrderCreationbyTerri`, `InvoiceDAL` invoice-print queries (`RemainingCreditDays`), PAGE-191 list.

## 18. Validation Matrix
| Field | Rule | FE | BE | Message | BR |
|---|---|---|---|---|---|
| company | selected (index>0) | — | yes | Please select a company. | 019 |
| customer | hidden id present | autocomplete | yes | Please select a valid customer. | 019 |
| amount | decimal, >0 | digit filter | yes | Please input a valid credit limit amount. | 019 |
| days | int, >0 | digit filter | yes | Please input a valid day limit. | 019 |
| duplicate | active limit for same company+customer exists (excluding self on edit) | — | yes | Data already exists. | 020 |
| DB write | returns false | — | yes | Operation failed. | — |
Errors shown with `faildalert('<msg>','Faild')`.

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-019 | A limit needs company, customer (chosen from autocomplete), amount > 0 and days > 0 (whole number). | `CustomerCreditLimitEntry.aspx.cs:76-109` | `ValidateForm` | CONFIRMED |
| BR-M05-020 | Only one **active** limit per company+customer: create is rejected if any `IsActive=1` row exists; edit is rejected if another active row exists. (Inactive rows do not block.) | `:138-166`; `CreditLimitDAL.cs:123-141` | `IsDuplicate` | CONFIRMED |
| BR-M05-021 | Every create and every edit stores `ActionStatus='Posted'` (pending). Editing an already Approved limit sends it back to Posted; `ApprovedBy/ApprovedDate` are left untouched. | `CreditLimitDAL.cs:20-32,115-121`; `CustomerCreditLimitEntry.aspx.cs:120` | DAL | CONFIRMED |
Related approval/enforcement rules BR-M05-022…029 are documented in PAGE-189 §19 and `workflows/WF-M05-03-credit-limit-lifecycle.md`.

## 20. Permission Matrix
Code reads only `Session["LoginName"]` (null ⇒ empty string stored as EntryBy/UpdateBy). No role check; access is menu-based. CONFIRMED.

## 21. Status Lifecycle
`tblCustomerCreditLimit.ActionStatus`: (none) → **Posted** (this page: create/update) → **Approved** or **Reject** (PAGE-189: `ApprovalUpdateDAL`). Edit ⇒ back to **Posted**. Separate `IsActive` bit: soft delete = `IsActive=0` (PAGE-191 `DeleteCreditLimit`), not an ActionStatus change.

## 22. Error Handling
Validation ⇒ `faildalert`; DAL returns false ⇒ "Operation failed."; SQL exceptions are swallowed by the `ClsCommonInternalDAL` command helpers (return false). `IsDuplicate` uses `Convert.ToInt32` on posted values (no try/catch) — tampered posts throw.

## 23. Audit
`EntryBy` = LoginName (string, not UserId), `EntryDate`, `UpdateBy`, `UpdateDate`, `ApprovedBy`, `ApprovedDate`. No history of previous limits (edit overwrites).

## 24. Integration
None external. Internal: consumed by order-to-invoice (PAGE `InvoiceCreationByOrder`, another module) and invoice print (Crystal dataset `dsInvoiceForCustomer.RemainingCreditDays`).

## 25. Reports
Invoice print shows `RemainingCreditDays` (see PAGE-189 §25/BR-M05-028).

## 26. Dependencies
`CreditLimitDAL`, `ClsCommonInternalDAL`, `SInventoryWebService`, AjaxControlToolkit (AutoComplete, FilteredTextBox), select2.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Save with company, valid customer, 50000, 30 | row Posted, IsActive=1, EntryBy=login | 019,021 |
| TC-02 | Save amount 0 / days 0 / blank | respective message | 019 |
| TC-03 | Save second active limit same company+customer | "Data already exists." | 020 |
| TC-04 | Same customer, other company | allowed | 020 |
| TC-05 | Edit an Approved limit | ActionStatus becomes Posted | 021 |
| TC-06 | Edit to values equal to another active row | "Data already exists." | 020 |
| TC-07 | Type customer text without choosing suggestion | "Please select a valid customer." | 019 |
| TC-08 | Amount `1,000` / negative | rejected by `decimal.TryParse` culture rules — verify | 019 |

## 28. Reverse Engineering References
Files in §6; `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1225-1365`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Amount enforcement | none | enforced only in order-list SP + UI button | INFO |
| Approval | none | separate page, no role check | INFO |

## 30. Known Gaps
- SQL built by string concatenation with culture-dependent `DateTime`/`bool` text (`CreditLimitDAL.cs:20-32,117-119`); customer id/company id are ints converted server-side so injection risk is low here but pattern is unsafe; `Session["MOParam"]` injection surface in the autocomplete.
- Duplicate check is not atomic (check then insert) — two concurrent saves can create two active limits.
- Editing an approved limit removes enforcement until re-approved ⇒ customer treated as unlimited meanwhile (see PAGE-189 BR-M05-024/025).
- `StartDate/EndDate` selected by the list but never set here (UNKNOWN use).
- `DayLimit` semantic ("validity days") is actually used as an overdue tolerance from the oldest unpaid invoice, not a limit validity window.
- Details in GAP-M05-011/012.

## 31. Change Impact
`CreditLimitDAL` is shared by PAGE-189/190/191. `tblCustomerCreditLimit` is read by `sp_LoadOrderListForOrderCreationbyTerri`, `InvoiceDAL.InvoiceMainDataForReport*` (invoice print). `InvoiceCreationByOrder.aspx` relies on the SP column `Permission`.
