---
page_id: PAGE-528
module: M13
page: SalesOperationRollback
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-528 — SalesOperationRollback

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-528 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | SalesOperationRollback |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SalesOperationRollback |
| aspx size | 5693 bytes |
| Code-behind LOC | 99 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**SalesOperationRollback** is a data-modifying support/admin screen that reverses one document of the sales cycle: a Proforma Invoice (removes it and makes the order invoiceable again), a Delivery Invoice (turns it back into an undelivered proforma and reverses the accounting voucher) or a Customer Payment / Money Receipt (removes the collection and re-opens the invoices it settled). Page heading text: `Rollback / Delete` (button label). CONFIRMED from `SalesOperationRollback.aspx.cs` and `SalesOperationRollbackDAL.cs`.

## 3. Business Objective
Let authorised staff correct wrongly created or wrongly confirmed sales transactions without database scripts, while keeping stock, order status and the accounting ledger consistent. The code enforces the reverse order of the sales cycle: payment first, then delivery, then proforma (BR-M13-002, BR-M13-005). Business intent is INFERRED from labels/messages; the mechanics are CONFIRMED.

## 4. Navigation
Menu placement is DB-driven (`sp_GET_MainMenuRole2`, not in repository) -> path `UNKNOWN`.

Inbound references from other code: none found. Outbound: none (no Response.Redirect). No page other than this one calls `SalesOperationRollbackBLL`/`SalesOperationRollbackDAL` (grep).

## 5. Route
`~/SInventory_UI/SalesOperationRollback.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SalesOperationRollback.aspx |
| Code-behind | Solution.Web/SInventory_UI/SalesOperationRollback.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | SalesOperationRollbackBLL (Library.BLL/SInventory_BLL/SalesOperationRollbackBLL.cs) |
| DAL class | SalesOperationRollbackDAL (Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs) |

## 7. UI Control Inventory
Generated inventory (verified against the .aspx):

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| rdoProforma | RadioButton | Proforma Invoice | no validator found | true |  |  | OnCheckedChanged→RollbackType_CheckedChanged |
| rdoDelivery | RadioButton | Delivery Invoice | no validator found |  |  |  | OnCheckedChanged→RollbackType_CheckedChanged |
| rdoPayment | RadioButton | Customer Payment | no validator found |  |  |  | OnCheckedChanged→RollbackType_CheckedChanged |
| lblCodeLabel | Label | Proforma Invoice No | no validator found |  |  |  |  |
| txtCode | TextBox |  | no validator found |  |  |  |  |
| txtRemarks | TextBox |  | no validator found |  |  |  |  |
| btnRollback | LinkButton |  | no validator found |  |  |  | OnClick→btnRollback_Click, OnClientClick→return confirm('Are you sure you want to perform this rollback/delete operation?'); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified behaviour**

| Control | Label | Required | Default | Visible/Enabled set in code | Data source / handler |
|---|---|---|---|---|---|
| rdoProforma | Proforma Invoice | one of the 3 radios (GroupName RollbackType) | Checked | - | AutoPostBack -> `RollbackType_CheckedChanged` |
| rdoDelivery | Delivery Invoice | " | unchecked | - | same |
| rdoPayment | Customer Payment | " | unchecked | - | same |
| lblCodeLabel | "Proforma Invoice No" initial; code sets "Proforma Invoice No:" / "Delivery Invoice No:" / "Money Receipt No:" | - | - | text changed on radio change | `RollbackType_CheckedChanged` (also clears txtCode) |
| txtCode | (label above) | YES - server check `string.IsNullOrEmpty(code)` -> alert "Please enter the Code/No!" (BR-M13-001) | empty | cleared by `Clear()` after success | free text, `.Trim()`; parameterised in DAL |
| txtRemarks | reason (multi-line, 3 rows) | YES - server check -> alert "Please enter the reason/remarks for rollback!" | empty | cleared after success | free text |
| btnRollback | "Rollback / Delete" | - | - | `OnClientClick="return confirm('Are you sure you want to perform this rollback/delete operation?');"` | `btnRollback_Click` |

## 8. Field Mapping
| UI field | DTO/param | Handler -> BLL -> DAL | SQL predicate / column |
|---|---|---|---|
| txtCode (Proforma) | proformaCode | `RollbackProforma` | `tblInvoice.InvoiceNo = @InvoiceNo` |
| txtCode (Delivery) | deliveryInvoiceNo | `RollbackDeliveryInvoice` | `tblInvoice.DelivaryInvoiceNo = @DelivaryInvoiceNo` |
| txtCode (Payment) | moneyReceiptNo | `RollbackCustomerPayment` | `tblCustomerPay.CollectionNo = @CollectionNo` |
| txtRemarks | remarks | same three methods | delivery/payment: voucher `Remarks` = "Reversal of <code> : <remarks>"; payment: `sp_InsertCustPayDetail_DeleteLog @Remarks`; proforma: **not persisted** |
| Session["LoginName"] | loginUser | same | `DeleteBy`, `DeliveryDeleteBy`, voucher `EntryBy`, `@LoginName` |

## 9. Dropdown Mapping
No dropdowns. Selector is the radio group `RollbackType` (Proforma / Delivery / Payment), default Proforma.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| btnRollback | `btnRollback_Click` (aspx.cs:49) | validates code/remarks/session, dispatches on the checked radio, alert with result |
| radios | `RollbackType_CheckedChanged` | relabels and clears code (no DB call) |

## 11. Frontend Flow
1. `Page_Load` (first load): `Clear()` empties txtCode and txtRemarks.
2. Radio change -> partial postback, label text swapped, txtCode cleared.
3. Click Rollback -> browser `confirm()`; on OK, postback `btnRollback_Click`.
4. Result is shown with `showMessageBox` (`alert(...)`, single quotes escaped, CR/LF replaced by space); on "SUCCESS" the inputs are cleared, on error the form keeps its values.
No page-local JavaScript calculations.

## 12. Backend Flow
`btnRollback_Click`: trim code/remarks -> empty code alert -> empty remarks alert -> `Session["LoginName"] == null` alert "User session is invalid. Please log in again." -> dispatch by radio -> `SalesOperationRollbackBLL.Rollback*` (try/catch returns `"Error: "+ex.Message`) -> `SalesOperationRollbackDAL.Rollback*` (new `SqlConnection` from `SqlUserAccess` credentials with `Encrypt=True;TrustServerCertificate=True`, `BeginTransaction`, all statements enrolled, `Commit` -> "SUCCESS"; `catch` -> `Rollback` -> "Error: <msg>").

**Proforma** (BR-M13-002..004): read invoice -> not found => "Proforma Invoice not found!" -> has DelivaryInvoiceNo => refuse -> log -> reset order -> per line stock reversal + freeze delete -> delete tblReturnAmount, tblInvoiceDetail, tblInvoice.
**Delivery** (BR-M13-005..008): read invoice by DelivaryInvoiceNo -> not found => "Delivery Invoice not found!" -> payment exists => refuse -> stock availability check -> insert negative Issue rows -> reverse vouchers (TableID 2014) -> log -> reset delivery columns.
**Payment** (BR-M13-009): read tblCustomerPay by CollectionNo -> not found => "Customer Payment Record not found!" -> reverse vouchers (TableID 2017) -> per detail reset invoice payment fields + SP log -> delete details and master.

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
| BLL | Method | Notes |
|---|---|---|
| SalesOperationRollbackBLL | RollbackProforma / RollbackDeliveryInvoice / RollbackCustomerPayment | pass-through with try/catch that converts exceptions to a string |

## 15. Repository Mapping
| DAL | Method | Notes |
|---|---|---|
| SalesOperationRollbackDAL | GetConnectionString | builds a private connection string from `SqlUserAccess` (credentials `<redacted>`, hard-coded in `Library.DAL/InternalCls/SqlUserAccess.cs`) |
| SalesOperationRollbackDAL | RollbackProforma / RollbackDeliveryInvoice / RollbackCustomerPayment | inline parameterised SQL in one transaction each (does NOT use `DataAccessManager`) |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature |
|---|---|---|---|
| sp_InsertCustPayDetail_DeleteLog(@Remarks, @CustPayDetailId, @LoginName) | `RollbackCustomerPayment` (per payment detail, inside the transaction) | body NOT in repository | write (log table `tblCustPayDetail_DeleteLog` per the SP name - INFERRED). Also called from `CustomerPaymentDeleteDAL.cs`. |

## 17. Database Mapping
| Table | Operation | By | Columns / effect |
|---|---|---|---|
| tblInvoice | R, U (delivery/payment), D (proforma) | all three | read InvoiceId, OrderId, DelivaryInvoiceNo, ComUnitId, OrderNo, CustomerMasterId; delivery: set DelivaryInvoiceNo/DeliveryInvoiceStatus/DeliveryDate/DeliveryPersonName/DeliveryPersonPhNo/DeliveryInvoiceCreateDate = NULL; payment: PaymentStatus, PaymentAmount = NULL; proforma: row deleted |
| tblInvoiceDetail | R, U, D | proforma D; delivery U | delivery: DeliveryQuantity, DeliveryBonusQuantity, DeliveryTotalQuantity, DeliveryTotalPrice, DeliveryTotalPriceVatAmount, DeliveryDiscountPercentage, DeliveryDiscountAmount, DeliveryNetAmount = 0; DeliveryStatus, ReturnReason = NULL |
| tblOrder / tblOrderDetail | U | proforma | IsInvoice = 0; Status = NULL |
| tblDCStore | R, I, D, U | proforma D/U; delivery I | negative Issue rows deleted (proforma) or inserted (delivery); Receive row StockQty capped to TotalQuantity |
| tblDCStoreFreeze | D | proforma, delivery | rows by InvoiceDetailId |
| tblReturnAmount | D | proforma | rows by InvoiceId |
| tblInvoiceDeleteLog | I | proforma, delivery | InvoiceId, InvoiceNo, DelInvoiceNo ('PROFORMA' or delivery no), DeleteDateTime, DeleteBy [, DeliveryDeleteBy, DeliveryDeleteDateTime] |
| tblInvoiceDetail_DeleterRecord | I | delivery | InvoiceDetailId, InvoiceId, ProductCode, BatchNo, Quantity, DeliveryQuantity, ExpDate, ReceiveDate |
| tblCustomerPay / tblCustPayDetail | R, D | payment | by CollectionNo / CustPayId |
| CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster / ..Detail | R, I | delivery (TableID 2014), payment (TableID 2017) | reversal vouchers (cross-database, same server) |
All column effects above are CONFIRMED from inline SQL (`SalesOperationRollbackDAL.cs`).

## 18. Validation Matrix
| Field | Rule | Message (verbatim) | Where |
|---|---|---|---|
| txtCode | non-empty | Please enter the Code/No! | aspx.cs |
| txtRemarks | non-empty | Please enter the reason/remarks for rollback! | aspx.cs |
| Session LoginName | not null | User session is invalid. Please log in again. | aspx.cs |
| Proforma exists | row found | Proforma Invoice not found! | DAL |
| No delivery yet | DelivaryInvoiceNo empty | Delivery Invoice already created! Please delete the Delivery Invoice first. | DAL |
| Delivery exists | row found | Delivery Invoice not found! | DAL |
| No payment | COUNT(tblCustPayDetail)=0 | Payment has been settled for this Delivery Invoice. Please rollback the payment first! | DAL |
| Stock | SUM(StockQty) >= returnQty | Sufficient stock not available for Product {0}, Batch {1}, Exp Date {2:dd-MMM-yyyy}. Required: {3}, Available: {4} | DAL |
| Payment exists | row found | Customer Payment Record not found! | DAL |
No length/format validation on txtCode/txtRemarks; no validators in markup.

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-001 | Rollback requires a non-empty Code/No, a non-empty reason (remarks) and a live session (Session["LoginName"] != null); otherwise an alert is shown and nothing is written. A JS confirm() precedes the postback. | `Solution.Web/SInventory_UI/SalesOperationRollback.aspx.cs:56` | SalesOperationRollback.aspx.cs btnRollback_Click; aspx OnClientClick confirm | CONFIRMED |
| BR-M13-002 | Proforma rollback is refused when the invoice already carries a DelivaryInvoiceNo ("Delivery Invoice already created! Please delete the Delivery Invoice first."). The proforma is looked up by tblInvoice.InvoiceNo = entered code. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:56` | SalesOperationRollbackDAL.RollbackProforma | CONFIRMED |
| BR-M13-003 | Proforma rollback (all in one SqlTransaction): (1) insert tblInvoiceDeleteLog row (DelInvoiceNo='PROFORMA', DeleteBy=login); (2) if OrderId>0: tblOrder.IsInvoice=0 and tblOrderDetail.Status=NULL (order becomes invoiceable again); (3) reverse stock (see stock-reversal rule); (4) delete tblDCStoreFreeze rows per invoice line; (5) delete tblReturnAmount, tblInvoiceDetail, tblInvoice. The entered remarks are NOT stored for a proforma rollback. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:61` | SalesOperationRollbackDAL.RollbackProforma | CONFIRMED |
| BR-M13-004 | Stock reversal for a proforma line with Quantity>0: Rule 1 delete TOP(1) tblDCStore row where ReferenceDCStoreId=line.DCStoreId AND Trans_Type='Issue' AND StockQty=-Quantity, then cap the linked Receive row (StockQty<=TotalQuantity); if none deleted and the line's own DCStore row is an Issue row, delete it. Rule 2 fallback delete TOP(1) Issue row matching ProductCode+BatchNo+ExpDate+ComUnitId+StockQty=-Quantity. Rule 3 (legacy): add Quantity back to the Receive row StockQty, capped at TotalQuantity. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:111` | SalesOperationRollbackDAL.RollbackProforma (loop over tblInvoiceDetail) | CONFIRMED |
| BR-M13-005 | Delivery-invoice rollback is refused when any tblCustPayDetail row exists for the invoice ("Payment has been settled for this Delivery Invoice. Please rollback the payment first!"). Lookup key: tblInvoice.DelivaryInvoiceNo. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:268` | SalesOperationRollbackDAL.RollbackDeliveryInvoice | CONFIRMED |
| BR-M13-006 | Delivery rollback first validates stock: for every line with returnQty = Quantity - DeliveryQuantity > 0, SUM(tblDCStore.StockQty) for ProductCode+BatchNo+ExpDate must be >= returnQty, else "Sufficient stock not available for Product .., Batch .., Exp Date ... Required: .., Available: ..". It then inserts a negative 'Issue' tblDCStore row (StockQty=-returnQty, CostPerUnit copied, TotalCostAmount=-returnQty*CostPerUnit, ReferenceDCStoreId = receive row) so that the quantity previously returned to stock at delivery is issued again, and deletes tblDCStoreFreeze rows. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:282` | SalesOperationRollbackDAL.RollbackDeliveryInvoice | CONFIRMED |
| BR-M13-007 | Delivery rollback reverses accounting: every voucher in CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster with CommonPK_ID=InvoiceId and TableID=2014 is copied to a NEW voucher (new VoucherCode = original prefix + yymmdd + running number for the day, PostingDate=now, Narration 'Reversal of <code> : <old narration>', Remarks 'Reversal of <code> : <remarks>', EntryBy=login) and its detail rows are copied with Debit/Credit swapped. Originals are not deleted. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:368` | SalesOperationRollbackDAL.RollbackDeliveryInvoice (cross-database write) | CONFIRMED |
| BR-M13-008 | After reversal the delivery invoice is "un-delivered", not deleted: tblInvoiceDeleteLog + tblInvoiceDetail_DeleterRecord rows are written, tblInvoice.DelivaryInvoiceNo/DeliveryInvoiceStatus/DeliveryDate/DeliveryPersonName/DeliveryPersonPhNo/DeliveryInvoiceCreateDate are set NULL, and every tblInvoiceDetail Delivery* quantity/amount column is set 0 and DeliveryStatus/ReturnReason NULL. The proforma (InvoiceNo, Quantity, NetAmount) survives. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:483` | SalesOperationRollbackDAL.RollbackDeliveryInvoice | CONFIRMED |
| BR-M13-009 | Payment (money receipt) rollback: locate tblCustomerPay.CollectionNo=code; reverse vouchers with CommonPK_ID=CustPayId and TableID=2017 (same swap-Dr/Cr copy as delivery); for each tblCustPayDetail row set tblInvoice.PaymentStatus=NULL and PaymentAmount=NULL (AdjustAmount is untouched) and call sp_InsertCustPayDetail_DeleteLog(@Remarks,@CustPayDetailId,@LoginName); finally delete tblCustPayDetail and tblCustomerPay rows. | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:561` | SalesOperationRollbackDAL.RollbackCustomerPayment | CONFIRMED |
| BR-M13-010 | Each rollback executes on its own connection inside one SqlTransaction; any exception rolls back everything and returns "Error: <message>" which the page shows verbatim in an alert; success returns "SUCCESS" ("Rollback operation successfully completed!"). | `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs:225` | SalesOperationRollbackDAL / SalesOperationRollbackBLL (try/catch returning string) | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).
- **No role check exists on this page** although it hard-deletes financial documents: any user who can browse to `SalesOperationRollback.aspx` (the menu is the only gate) can run all three rollbacks. `Session["LoginName"]` is only used as the audit name.

## 21. Status Lifecycle
Status columns changed by the page (CONFIRMED):

| Document | Before | After | Code |
|---|---|---|---|
| tblOrder.IsInvoice | 1 (invoiced) | 0 | proforma rollback |
| tblOrderDetail.Status | any | NULL | proforma rollback |
| tblInvoice.DeliveryInvoiceStatus / tblInvoiceDetail.DeliveryStatus | Full/Partial/Reject | NULL | delivery rollback |
| tblInvoice.PaymentStatus / PaymentAmount | set | NULL | payment rollback |

Allowed order (enforced by guards): Payment rolled back -> Delivery rolled back -> Proforma rolled back (order becomes invoiceable). No forward transition is set by this page.

## 22. Error Handling
BLL and DAL catch every exception and return `"Error: " + ex.Message`; DAL rolls back the transaction first. The page shows the string in a JS alert (message text can include SQL/constraint details - information exposure). No logging of failures. `showMessageBox` escapes only `'` and CR/LF.

## 23. Audit
Written logs: `tblInvoiceDeleteLog`, `tblInvoiceDetail_DeleterRecord`, `tblCustPayDetail_DeleteLog` (via SP), and reversal vouchers carrying `EntryBy`. **The remarks entered by the user are not stored in the invoice logs** (only in the voucher `Remarks` and the payment-detail log). The proforma header/detail rows are hard-deleted; only the log row keeps InvoiceId/InvoiceNo.

## 24. Integration
Cross-database write to the accounting database `CSTL_ACCDB_Unicorn` (three-part names `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster/Detail`) - same SQL Server login/transaction (CONFIRMED). No SAP/B2B integration is reversed by this page (SAP-related columns are not touched) - whether posted SAP documents need reversal is UNKNOWN.

## 25. Reports
No report. (The proforma/delivery documents it removes appear in the M13 sales reports, see `reports/RPT-M13-03-sales-detail-exports.md`.)

## 26. Dependencies
BLL: SalesOperationRollbackBLL; DAL: SalesOperationRollbackDAL (`SqlUserAccess`, `DataBase.SalesDB`); tables: see §17; SP: sp_InsertCustPayDetail_DeleteLog; external DB: CSTL_ACCDB_Unicorn.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Submit with empty code | alert, no DB change | BR-M13-001 |
| TC-02 | Submit with empty remarks | alert, no DB change | BR-M13-001 |
| TC-03 | Proforma with no delivery, valid code | invoice+lines deleted, order.IsInvoice=0, stock Issue rows removed, log row `PROFORMA` | BR-M13-002..004 |
| TC-04 | Proforma that already has DelivaryInvoiceNo | error alert, nothing changed | BR-M13-002 |
| TC-05 | Delivery invoice with a payment | error "Payment has been settled...", nothing changed | BR-M13-005 |
| TC-06 | Delivery invoice with rejected qty but batch stock below returnQty | stock error message, transaction rolled back | BR-M13-006 |
| TC-07 | Delivery invoice, no payment, stock ok | delivery columns NULL/0, negative Issue row inserted, reversal voucher (TableID 2014) created with swapped Dr/Cr, delete-log rows | BR-M13-006..008 |
| TC-08 | Money receipt no. of a 2-invoice collection | both invoices PaymentStatus/PaymentAmount NULL, 2 log-SP calls, vouchers reversed (2017), tblCustPayDetail+tblCustomerPay deleted | BR-M13-009 |
| TC-09 | Force a failure mid-way (e.g. voucher insert error) | full rollback, "Error: ..." shown | BR-M13-010 |
| TC-10 | Unknown code | "... not found!" | BR-M13-002/005/009 |
| TC-11 | Session expired (LoginName null) | session alert | BR-M13-001 |
| TC-12 | Non-admin user browses to URL | (gap) operation succeeds - verify role gating | GAP-M13-004 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SalesOperationRollback.aspx`, `.aspx.cs`
- `Library.BLL/SInventory_BLL/SalesOperationRollbackBLL.cs`
- `Library.DAL/SInventory_DAL/SalesOperationRollbackDAL.cs`
- `Library.DAL/SInventory_DAL/CustomerPaymentDeleteDAL.cs` (other caller of the log SP)

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| Business rules | none | BR-M13-001..010 extracted | inline SQL read | CONFIRMED | §19 |
| SP body | none | sp_InsertCustPayDetail_DeleteLog called | not in repo | UNKNOWN | GAP-M13-001 |

## 30. Known Gaps
- No role/permission check on a destructive page (GAP-M13-004).
- Remarks not persisted for proforma/delivery rollbacks; audit log lacks reason.
- Proforma rollback does not touch accounting vouchers or SAP-related fields; delivery rollback ignores sub-depot invoices (`tblSubInvoiceMaster`) - only `tblInvoice` is handled.
- Payment rollback does not reset `tblInvoice.AdjustAmount` and does not verify later documents referencing the payment.
- New DCStore id = `MAX(DCStoreId)+1` inside the INSERT and voucher code = `MAX(SUBSTRING(VoucherCode,11,6))+1` -> race conditions under concurrency.
- `DELETE TOP (1) ... StockQty = -Qty` can remove a different Issue row of equal quantity.
- Connection string with hard-coded SQL credentials (`<redacted>`, `SqlUserAccess.cs`) and `TrustServerCertificate=True`; ex.Message shown to user.
- Unused locals (`orderNo`, `customerMasterId`) in the delivery path.

## 31. Change Impact
- Module: M13. Only this page uses `SalesOperationRollbackBLL/DAL`.
- Tables shared with invoicing/delivery/collection modules (tblInvoice, tblInvoiceDetail, tblDCStore, tblCustomerPay, tblCustPayDetail, tblOrder): any schema change impacts this page. `sp_InsertCustPayDetail_DeleteLog` is also used by `CustomerPaymentDeleteDAL`.
- Accounting DB `CSTL_ACCDB_Unicorn` voucher tables and TableID conventions 2014 (invoice) / 2017 (customer payment) must stay in sync with the posting code (`SP_Sales_CustomerPaymentPosting` family, other modules).
