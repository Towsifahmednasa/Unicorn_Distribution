---
page_id: PAGE-189
module: M05
page: CustomerCreditLimitApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-189 — CustomerCreditLimitApproval

> **Evidence level:** Tier A. `.aspx`, `.aspx.cs`, `CreditLimitDAL`, and the enforcement SP `sp_LoadOrderListForOrderCreationbyTerri` (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1225-1365`), `InvoiceDAL` invoice-print SQL, `InvoiceCreationByOrder.aspx.cs` were read. `tblCustomerCreditLimit` DDL not in repo.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-189 |
| Module | M05 |
| Page | CustomerCreditLimitApproval |
| Kind | Approval (batch approve/reject) |
| Folder / class | `SInventory_UI` / `SInventory_UI_CustomerCreditLimitApproval` |

## 2. Purpose
List all credit limits waiting in status `Posted` and let an approver approve or reject one or many at once. CONFIRMED (`CustomerCreditLimitApproval.aspx.cs:36-158`).

## 3. Business Objective
Make credit limits effective only after a second person confirms them (maker/checker), because approved limits directly block or allow invoice generation. INFERRED from status flow and enforcement SP.

## 4. Navigation
Menu DB-driven (UNKNOWN). No inbound code links found. Cancel button reloads the list.

## 5. Route
`~/SInventory_UI/CustomerCreditLimitApproval.aspx`.

## 6. Source Files
`Solution.Web/SInventory_UI/CustomerCreditLimitApproval.aspx(.cs)`, `Library.DAL/SInventory_DAL/CreditLimitDAL.cs`, `Library.DAO/SInventory_Entities/CreditLimitDAO.cs`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| `statusRadioButtonList` | Operation | yes | none selected | items `Approve` (value 1), `Reject` (value 0) |
| `loadGridView` | list | — | bound on first load | columns Company Name, Customer Name, Limit Amount, Entry User (EntryBy), Entry Date (`dd-MMM-yyyy`), select checkbox; `DataKeys[0]` = CreditLimitId; header checkbox `chkSelectAll` (AutoPostBack) |
| `chkSelectAll` / `chkSelect` | select | at least one row | unchecked | `chkSelectAll_CheckedChanged` ticks all rows |
| `submitButton` | Submit | — | — | → `btnSubmit0_Click` |
| `cancelButton` | Cancel | — | — | reload list |
UpdatePanel + progress image. Grid header forced to `<thead>` by `gv_DocumentUpload_PreRender`.

## 8. Field Mapping
| UI | DAO | SQL param | Column |
|---|---|---|---|
| DataKeys[i][0] | CreditLimitId | @CreditLimitId | `tblCustomerCreditLimit.CreditLimitId` (WHERE) |
| radio text (Reject vs other) | ActionStatus `"Approved"` or `"Reject"` | @ActionStatus | ActionStatus |
| Session["LoginName"] | ApprovedBy | @ApprovedBy | ApprovedBy |
| `DateTime.Today` | ApprovedDate | @ApprovedDate | ApprovedDate |
`CreditLimitDAL.ApprovalUpdateDAL` `:36-47`: `UPDATE tblCustomerCreditLimit SET ActionStatus=@ActionStatus,ApprovedBy=@ApprovedBy,ApprovedDate=@ApprovedDate WHERE CreditLimitId=@CreditLimitId` (parameterised).

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
| Button | Handler | DAL | SQL | Effect |
|---|---|---|---|---|
| submitButton | `btnSubmit0_Click` `:63` | `ApprovalUpdateDAL` per ticked row | UPDATE above | status change; then list reloads |
| cancelButton | `cancelButton_Click` | `LoadCustomerCreditLimitApproval("")` | SELECT | reload |

## 11. Frontend Flow
Page load → `LoadCustomerCreditLimitInformation` binds grid. User picks operation + ticks rows (or select-all) → Submit → server. After processing the grid is re-queried so decided rows disappear. Alerts via `alert()`.

## 12. Backend Flow
`btnSubmit0_Click`: `Validation()` — if no operation ⇒ alert "Please select an operation!!"; else counts ticked rows; false ⇒ (extra) alert "Please select at least one row!!!". `SubmitApproval()`: if radio text ≠ "Reject" ⇒ for each ticked row `ApprovalUpdateDAL(Approved)` and alert "Credit Limit Approved Successfully!!"; else same loop with `"Reject"` and alert "Credit Limit Rejected Successfully!!!". Each UPDATE is an independent statement (no transaction; return values ignored, so success messages are shown even if an UPDATE failed). List query: `SELECT * FROM tblCustomerCreditLimit INNER JOIN tblCompanyInfo … INNER JOIN tblCustMaster … WHERE tblCustomerCreditLimit.ActionStatus='Posted'`.

## 13. API Mapping
None.

## 14. Service Mapping
None (page → `CreditLimitDAL`). `LoadApprovalControlDAL` / `ClsApprovalAction` (role-based approval-control loader) exist in the DAL but are **not called** by this page.

## 15. Repository Mapping
`CreditLimitDAL.LoadCustomerCreditLimitApproval`, `ApprovalUpdateDAL`.

## 16. Stored Procedure Mapping
Page: none (inline SQL; reads via generic `ExecuteAllSqlQueryByStoreProcedure`, UPDATE via `UpdateDataByUpdateCommand(query, params, "SSIDB")`). Enforcement consumer: `sp_LoadOrderListForOrderCreationbyTerri` (**body read**, see BR-M05-025).

## 17. Database Mapping
Read: `tblCustomerCreditLimit`, `tblCompanyInfo`, `tblCustMaster`. Write: `tblCustomerCreditLimit.ActionStatus/ApprovedBy/ApprovedDate`. Enforcement reads (SP): `tblOrder`, `tblCompanyUnit`, `tblMarket`, `tblCustomerType`, `tblInvoice`, `tblInvoiceDetail`, `tblCustPayDetail`, `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster/Detail` (cross-database, returns).

## 18. Validation Matrix
| Rule | Where | Message |
|---|---|---|
| operation chosen | `Validation` | Please select an operation!! |
| ≥1 row ticked | `Validation` | Please select at least one row!!! |
| (none) | approver identity, self-approval, amount thresholds | not validated |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-022 | Approval queue = limits with `ActionStatus='Posted'` joined to company and customer (rows whose company or customer is missing are hidden). | `CreditLimitDAL.cs:48-55` | `LoadCustomerCreditLimitApproval` | CONFIRMED |
| BR-M05-023 | Approve/Reject is a batch operation on ticked rows: status `Approved` or `Reject` (literal, singular), `ApprovedBy`=login name, `ApprovedDate`=today; no comment field, no per-row reason, no role/limit check, no separation-of-duty check (creator can approve own entry). | `CustomerCreditLimitApproval.aspx.cs:108-158`; `CreditLimitDAL.cs:36-47` | `SubmitApproval` | CONFIRMED |
| BR-M05-024 | Enforcement counts only limits with `IsActive=1` AND `UPPER(TRIM(ActionStatus))='APPROVED'`; if several rows exist for the same company+customer their `LimitAmount` and `DayLimit` are **summed**. Rejected, Posted, inactive or missing limits ⇒ no limit (customer treated as unlimited). | `LIVE_DB…sql:1312-1319` | `sp_LoadOrderListForOrderCreationbyTerri` | CONFIRMED |
| BR-M05-025 | Invoice permission flag: `Permission=1` when (`LimitAmount=0`, i.e. no limit) OR (`GrossValue + Due <= LimitAmount` AND `DATEDIFF(DAY, oldestUnpaidInvoiceDate, today) <= DayLimit`); otherwise 0 ("Blocked"). `GrossValue` is the order's gross value; note a customer with no unpaid invoice has days=0. | `LIVE_DB…sql:1252-1265` | SP | CONFIRMED |
| BR-M05-026 | `Due` = SUM(`DeliveryTpGrandTotal`) − (SUM(PaymentAmount)+SUM(AIT)+SUM(DiscountOnPayment)) − SUM(return amount from accounts voucher `TableID=2018`) per customer+delivery-unit, only groups with positive delivered total; "oldest unpaid invoice" = MIN(`InvoiceDate`) among invoices with delivered total >0 and outstanding > 1 (per customer+company); company of the order's DC unit must equal the limit's `CompanyId`. | `LIVE_DB…sql:1292-1341` | SP | CONFIRMED |
| BR-M05-027 | The block is a UI rule: `InvoiceCreationByOrder.aspx` renders the "Generate Invoice" button as red "Blocked" with client alert "Credit Limit Exceeded! Invoice creation is blocked for this customer." when `Permission != "1"`. No server-side re-check exists in the C# invoice-creation code (grep of `CreditLimit` shows only `InvoiceDAL` print queries), so a direct URL/postback bypasses it (INFERRED; downstream SPs not visible). | `InvoiceCreationByOrder.aspx.cs:622-645` | page | CONFIRMED (UI) / INFERRED (no server check) |
| BR-M05-028 | Invoice print shows `RemainingCreditDays = MAX(0, SUM(DayLimit) − days since oldest unpaid invoice)` using the same approved+active limits. | `InvoiceDAL.cs:1343-1347,1485-1489` | inline SQL | CONFIRMED |
| BR-M05-029 | "Delete" on the credit list is soft: `IsActive=0` (status unchanged); soft-deleted limits stop being enforced and no longer block a new active limit. | `CreditLimitDAL.cs:57-61`; `CustomerCreditView.aspx.cs:92-105` | PAGE-191 | CONFIRMED |

## 20. Permission Matrix
Only `Session["LoginName"]` is read. No role check, so any user who can open the page (menu) can approve any limit. The role-based approval-control helper (`ClsApprovalAction.LoadActionControlByUser`) is present but unused here. CONFIRMED.

## 21. Status Lifecycle
`ActionStatus`: `Posted` → **Approved** | **Reject** (terminal for this page; a rejected row stays `Reject` until edited, which sets `Posted` again in PAGE-190). `Approved` → `Posted` on edit. Enforcement uses `Approved`+`IsActive=1` only. Code setting values: `CustomerCreditLimitApproval.aspx.cs:124,148`, `CustomerCreditLimitEntry.aspx.cs:120`, `CreditLimitDAL.cs:32,119`.

## 22. Error Handling
DAL update result ignored; no try/catch; UpdateDataByUpdateCommand catches SQL errors and returns false ⇒ UI still says "Approved Successfully" (GAP-M05-012). Messages use `alert('…')` (`ShowMessageBox` escapes `'`).

## 23. Audit
`ApprovedBy` (login string) and `ApprovedDate` (date only — time lost). Previous approver/date are overwritten on re-approval; no log table.

## 24. Integration
Consumed by order-to-invoice (`InvoiceCreationByOrder`, order-management module), invoice print (`RemainingCreditDays` column of Crystal dataset `dsInvoiceForCustomer`), and `CustomerCreditView` list (PAGE-191).

## 25. Reports
Invoice print (Crystal) shows `TotalDue` and `RemainingCreditDays` computed in `InvoiceDAL.InvoiceMainDatainvNo/InvoiceMainDataForReport` — outside this module's scope but driven by this page's data.

## 26. Dependencies
`CreditLimitDAL`, `ClsCommonInternalDAL`, `tblCompanyInfo`, cross-DB `CSTL_ACCDB_Unicorn`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Load page | only `Posted` rows listed | 022 |
| TC-02 | Submit with no radio | "Please select an operation!!" | — |
| TC-03 | Radio Approve, no rows ticked | "Please select at least one row!!!" | — |
| TC-04 | Approve 3 ticked rows | 3 rows `Approved`, ApprovedBy=login, ApprovedDate=today, rows leave list | 023 |
| TC-05 | Reject a row | ActionStatus `Reject`; customer unlimited again | 023,024 |
| TC-06 | Order gross 60k, dues 30k, limit 80k, oldest unpaid 10 days, DayLimit 30 | Permission=0 (Blocked) | 025 |
| TC-07 | Same but dues 10k | Permission=1 | 025 |
| TC-08 | Oldest unpaid 40 days, DayLimit 30, amount within limit | Blocked | 025 |
| TC-09 | Customer without approved limit | Permission=1 | 024,025 |
| TC-10 | Two approved limits (50k/30d + 20k/15d) same company | enforced as 70k/45d | 024 |
| TC-11 | Creator approves own limit | allowed (no SoD) | 023 |
| TC-12 | Edit approved limit then open invoice list | customer unlimited until re-approval | 021,024 |

## 28. Reverse Engineering References
Files in §6; `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1225-1365`; `Library.DAL/SInventory_DAL/InvoiceDAL.cs:1340-1557`; `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs:600-645`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Maker/checker | None existed | no role/SoD enforcement | GAP |
| Limit enforcement | None existed | UI-level block only | INFO |

## 30. Known Gaps
- No authorisation beyond menu; no self-approval prevention; no reject reason.
- Rejected/Posted/edited limits release the customer from control (unlimited) — business risk.
- Success messages shown even if an UPDATE fails (results ignored).
- Enforcement only in the order-list SP/UI; not server-verified at invoice creation.
- `LimitAmount` is not shown in invoice print; only days are.
- Amounts summed across multiple rows (no de-dup by validity window; `StartDate/EndDate` ignored by enforcement).
- Cross-database dependency on `CSTL_ACCDB_Unicorn` in a SP makes environments non-portable.
GAP-M05-012/013.

## 31. Change Impact
`sp_LoadOrderListForOrderCreationbyTerri` (also used by `OrderInfoDAL.cs:220`), `InvoiceDAL` print SQL and `dsInvoiceForCustomer` all depend on status literal `'Approved'` — changing the literal (e.g. to "Approve") silently disables enforcement. `CreditLimitDAL` shared with PAGE-190/191.
