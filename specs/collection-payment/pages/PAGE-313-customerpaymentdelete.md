---
page_id: PAGE-313
module: M09
page: CustomerPaymentDelete
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-313 — CustomerPaymentDelete (delete a posted collection line with remarks)

> **Evidence level:** Tier A — `.aspx.cs` (235 lines), `CustomerPaymentDeleteBLL`, `CustomerPaymentDeleteDAL` and the grid markup read in full. The delete itself is performed by SP `sp_InsertCustPayDetail_DeleteLog`, whose body is NOT in the repo; whether it removes the row, logs it, adjusts the invoice status, or reverses the accounts voucher is UNKNOWN. This page was added to the module beyond the candidate list because it is the supported (logged) way to undo a collection.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-313 |
| Kind | Entry / Transaction — corrective delete (CONFIRMED) |
| Page class | SInventory_UI_CustomerPaymentDelete; LOC 235 |

## 2. Purpose
Find an invoice that has customer-payment lines (by depot + invoice date range, or by depot + route), list its payment detail lines (invoice no, amount, date), and delete a line after typing a remark. Only the payment line with the **latest payment date** for that invoice has an enabled Delete button.

## 3. Business Objective
Correct erroneous collection entries with a mandatory reason and a deletion log instead of raw database deletes. INFERRED from the SP name (`…_DeleteLog`) and the mandatory-remark rule.

## 4. Navigation
Menu path UNKNOWN. Reset → self (`CustomerPaymentDelete.aspx`).

## 5. Route
`~/SInventory_UI/CustomerPaymentDelete.aspx`.

## 6. Source Files
`CustomerPaymentDelete.aspx(.cs)`; `Library.BLL/SInventory_BLL/CustomerPaymentDeleteBLL.cs`; `Library.DAL/SInventory_DAL/CustomerPaymentDeleteDAL.cs`; `SeedDataDAL.GetRouteInfoforCustPayment`; `OrderInfoBLL.LoadSC`.

## 7. UI Control Inventory
| Control | Label | Required | Notes |
|---|---|---|---|
| DepoNameDropDownList | Depot | needed to load invoices | `LoadSC(UserId)` user depots; on change loads routes and clears invoice list |
| rootDropDownList | Route | optional path B | change → invoices for depot+route |
| fromDateTextBox / toDateTextBox | From / To Date | both + depot for path A | AutoPostBack; "From Date cannot be after To Date"; "Invalid date format" |
| InvoiceNoDropDownList | Invoice | – | path A: `InvoiceNo` of invoices in date range having payment lines; path B: `CustomerCode : CustomerName [InvoiceNo]` |
| invoiceGridView | Payment lines | – | Invoice No, Payment Amount, Payment Date, Remarks textbox, Delete button (`CommandName=DeleteRow`, argument `CustPayDetailId`) |
| remarksText | Remarks | required (blank ⇒ nothing happens, no message) | |

## 8. Field Mapping
`remarksText`, `CustPayDetailId` (command argument), `Session["LoginName"]` → SP params `@Remarks`, `@CustPayDetailId`, `@LoginName` (`CustomerPaymentDeleteDAL.cs:61-69`). Grid columns from `tblCustPayDetail ⟕ tblInvoice`: InvoiceNo, CustPayDetailId, InvoiceId, PaymentAmount, custPaymentDate.

## 9. Dropdown Mapping
Depot: tblCompanyUnit by user; Route: `sp_Get_RouteInfoforCustPayment`; Invoice list SQL: path A `tblInvoice ⨝ tblCustPayDetail WHERE InvoiceDate BETWEEN @From AND @To AND ComUnitId=@ComUnitId` (not DISTINCT ⇒ an invoice with N payment lines appears N times); path B `tblInvoice ⨝ tblOrder ⨝ tblCustPayDetail WHERE ord.ComUnitId AND ord.DistributionRouteId`.

## 10. Button Action Mapping
Invoice selected → `invoiceNoDropDownList_SelectedIndexChanged` → `LoadData(InvoiceId)`; Delete → `invoiceGridView_RowCommand("DeleteRow")`.

## 11. Frontend Flow
Choose depot → dates (or route) → pick invoice → grid lists its payment lines → type remark → click Delete (no confirmation dialog) → alert "Record deleted successfully." and grid reloads.

## 12. Backend Flow
`RowDataBound` disables Delete on rows whose `custPaymentDate` ≠ the maximum date for the invoice (`ViewState["LatestPaymentDate"]`, :127-146; date-level compare, so several lines on the latest date are all deletable). `RowCommand` (:151-192): read remarks; if non-empty → `DeleteCustomerPaymentDetail(id, remarks, LoginName)` → `sp_InsertCustPayDetail_DeleteLog` via `UpdateAction` (swallows exceptions ⇒ false ⇒ "Delete failed."). The DAL opens a `DataAccessManager` connection (`SqlConnectionOpen`) and never closes it (`CustomerPaymentDeleteDAL.cs:63`).

## 13. API Mapping
None.

## 14. Service Mapping
`CustomerPaymentDeleteBLL` (LoadInvoicesByDateRange, LoadInvoicesByRoute, LoadData, DeleteCustomerPaymentDetail); `OrderInfoBLL.LoadSC`.

## 15. Repository Mapping
`CustomerPaymentDeleteDAL` (inline SELECTs listed in §9, `sp_InsertCustPayDetail_DeleteLog` call), `SeedDataDAL.GetRouteInfoforCustPayment`.

## 16. Stored Procedure Mapping
`sp_InsertCustPayDetail_DeleteLog(@Remarks,@CustPayDetailId,@LoginName)` — body NOT in repo (UNKNOWN). `sp_Get_RouteInfoforCustPayment` (UNKNOWN).

## 17. Database Mapping
tblCustPayDetail (SELECT; delete/log by SP), tblInvoice, tblOrder (SELECT). Delete-log table name UNKNOWN. **tblCustomerPay header, tblInvoice payment status and accounts vouchers are not touched by any code on this page.**

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| From ≤ To | `DateTextBox_TextChanged` :46 | "From Date cannot be after To Date" | BR-M09-041 |
| Date parse | :43-64 | "Invalid date format" | – |
| Remarks non-empty | :161 | none (silent no-op) | BR-M09-041 |
| Only latest-date line deletable | grid state :145 | button disabled (UI-only; server does not re-check) | BR-M09-041 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M09-041 | A payment detail line may be deleted only after entering a remark, and the UI enables Delete only for lines dated the same as the invoice's latest payment (LIFO-style). The server side does not re-verify either condition (a crafted postback with the command can delete any line id). The action records `@LoginName` and `@Remarks` through `sp_InsertCustPayDetail_DeleteLog`. | CustomerPaymentDelete.aspx.cs:135-192; CustomerPaymentDeleteDAL.cs:61-69 | CONFIRMED (UI) / UNKNOWN (SP effect) |
| BR-M09-042 | Deleting a collection line, in the visible code, does **not** reverse the accounts voucher created by PAGE-311, restore the invoice's payment status, or update the header amount; any such handling would have to be inside the SP (UNKNOWN). Compare with PAGE-335 which resets the invoice status. | whole page | CONFIRMED (absence in C#) / UNKNOWN (SP) |

## 20. Permission Matrix
Session: UserId, LoginName. Menu-based only — **no role/user restriction** (contrast DepositList's hard-coded user list).

## 21. Status Lifecycle
Payment line lifecycle: exists → deleted (with log). No soft-delete flag visible in C#.

## 22. Error Handling
Try/catch around date load and delete; messages via `alert`. `invoiceNoDropDownList_SelectedIndexChanged` does `Convert.ToInt32(InvoiceNoDropDownList.SelectedValue)` with no guard (empty ⇒ FormatException).

## 23. Audit
`@LoginName` + `@Remarks` passed to the log SP (storage UNKNOWN). No date-of-deletion in C#.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
`sp_InsertCustPayDetail_DeleteLog`, tblCustPayDetail, tblInvoice, tblOrder.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-313-01 | Invoice with lines on 3 dates | only newest-date lines have enabled Delete | BR-M09-041 |
| TC-313-02 | Click Delete with empty remark | nothing happens | BR-M09-041 |
| TC-313-03 | Delete with remark | success alert; line gone; verify log row (UNKNOWN table) | BR-M09-041 |
| TC-313-04 | After delete, check invoice PaymentStatus and voucher | document actual state (voucher expected to remain) | BR-M09-042 |
| TC-313-05 | Forge RowCommand for an older line | currently accepted (defect) | BR-M09-041 |
| TC-313-06 | From > To | message | – |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/CustomerPaymentDelete.aspx(.cs)`, `Library.DAL/SInventory_DAL/CustomerPaymentDeleteDAL.cs`, `Library.BLL/SInventory_BLL/CustomerPaymentDeleteBLL.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Accounting reversal | none | not in C#; SP unknown | UNKNOWN (GAP-M09-003) |

## 30. Known Gaps
SP body missing; no server validation of "latest only"; no confirm dialog; connection leak in DAL; path A dropdown duplicates invoices; no reversal of tblCustomerPay/tblInvoice/voucher in visible code; no permission control.

## 31. Change Impact
Interacts with PAGE-311 data (tblCustPayDetail) and reports (ledger, aging, receivable). `PaymentReverseDal` (PAGE-335) deletes the same tables by different rules.
