---
page_id: PAGE-287
module: M08
page: DeliverySalesReturn
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-287 — DeliverySalesReturn (Return against a delivery invoice)

> **Evidence level:** Tier A — read `DeliverySalesReturn.aspx` and `.aspx.cs` in full (405 lines), `CustomerReturnDAL.cs`/`CustomerReturnBLL.cs` in full, and the SP body of `sp_DeliverySalesReturnPosting` (`Live_Deployment_Script_All_Updates.sql:558-812`). Table DDL for `tblCustomerReturnMaster/Detail` is not in the repo except the three `ALTER`s at `:16-39`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-287 |
| Module | M08 |
| Name / class | DeliverySalesReturn / `SInventory_UI_DeliverySalesReturn` (`.aspx.cs:13`) |
| Kind | Entry / Transaction (stock + accounting) |
| Size | aspx 223 lines (UpdatePanel), code-behind 405 lines |
| Legacy | writes a debug log to a developer profile path (`WriteLog`, `.aspx.cs:18-26`); `protected DropDownList payTypeDDL = new DropDownList()` stub (`:404`) |

## 2. Purpose
CONFIRMED (labels "Delivery Sales Return", "Net Customer Return / Credit"): a depot user searches by **Delivery Invoice No** and returns part or all of what was delivered on that invoice, optionally deducting a return-delivery charge; the system creates the return, puts stock back and posts the reversal vouchers.

## 3. Business Objective
INFERRED: reverse a delivered invoice when goods come back (shop closed, damaged, refused) — credit the customer's receivable, restore inventory at the original batch, reverse COGS/inventory valuation, and recover a delivery charge.

## 4. Navigation
Menu-driven (UNKNOWN path). No inbound page links found. Outbound: none (stays on page).

## 5. Route
`~/SInventory_UI/DeliverySalesReturn.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | Solution.Web/SInventory_UI/DeliverySalesReturn.aspx(.cs) |
| BLL | Library.BLL/SInventory_BLL/CustomerReturnBLL.cs; OrderInfoBLL.LoadInvoice; InvoiceBLL (PaymentTypeLoad) |
| DAL | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs (whole file); OrderInfoDAL.LoadInvoice |
| DAO | CustomerReturnMasterDAO, CustomerReturnDetailDAO |
| SQL | Live_Deployment_Script_All_Updates.sql:16-39 (alter tblCustomerReturnMaster), :557-815 (`sp_DeliverySalesReturnPosting`), :98-115 (GL flag 1032 seed) |

## 7. UI Control Inventory
| Control | Type / label | Required | Default | State | Data source / notes |
|---|---|---|---|---|---|
| delInvNoTextBox0 | TextBox "Delivery Invoice No:" (AutoPostBack → `Button2_Click`) | Yes (implicit) | empty | editable | matched against `tblInvoice.DelivaryInvoiceNo` |
| Button2 | "Search" | — | — | — | `Button2_Click` :291-325 |
| paymentTypeRadioButtonList | Cash (selected)/Credit | must have selection | Cash | `style="display:none"` | validated only for "count>0" |
| uiel (div) | details panel | — | `Visible=True` | shown always | — |
| custCodeTextBox, custNameTextBox, custAddressTextBox, orderNoTextBox, orderDateTextBox, salesPersonNameLabel, invoiceNoLabel, invDateTextBox, payamntTextBox, deliveryInvoiceNoLabel, updateDateLabel, daNameLabel, comUnitNameTextBox | **Labels** | — | filled by `LoadInvoice` | read-only | payment amount = SUM(tblCustPayDetail.PaymentAmount) (informational) |
| deliverypersonMobileTextBox, deliverypersonNameTextBox, remarksTextBox | Label `Visible=false` | — | — | hidden | |
| hdCustomerMasterId, hdComUnitId, hdMiaId, invoiceNoHiddenField, invoiceHiddenField, orderIdHiddenField | HiddenField | — | — | — | `invoiceHiddenField` = InvoiceId used at save |
| loadGridView | GridView `DataKeyNames=InvoiceId,InvoiceDetailId,UnitPrice,UnitVatAmount` | — | bound to `LoadInvoiceNew` | — | columns: Product Code, Product Name, Pack Size, **Available Qty** (`AvialableQty`), Net Amount, Return Qty (`retQtyTextBox`, AutoPostBack), Return Amount (`retamountTextBox`, disabled) |
| txtTotalReturnAmount | TextBox "Total Product Return Amount" | — | 0.00 | ReadOnly | Σ line return amounts |
| txtDeductionCharge | TextBox "Return Delivery / Deduction Charge" (AutoPostBack, `ValidChars=0123456789.`) | No | 0.00 | editable | JS `calcNetReturn()` + server |
| txtNetReturnAmount | TextBox "Net Customer Return / Credit" | — | 0.00 | ReadOnly | total − deduction (≥0) |
| Button1 "Save Return" | Button, `OnClientClick=confirm('Are you sure you want to Return Invoice ?')` | — | — | — | `saveButton_Click` |
| clearButton "Clear" | Button | — | — | — | `Clear()` |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | SQL param | Table.column |
|---|---|---|---|---|---|---|
| invoiceHiddenField | `CustomerReturnMasterDAO.InvoiceId` | `saveButton_Click` :214 | `SaveCustomerReturn` | `CustomerReturnDAL.SaveCustomerReturn` :17-50 | `@InvoiceId` | tblCustomerReturnMaster.InvoiceId |
| Σ retQty | `TotalQty` | :216 | same | same | `@TotalQty` | TotalQty |
| Σ retamount | `TotalAmount` | :215 | same | same | `@TotalAmount` | TotalAmount |
| txtDeductionCharge | `DeductionCharge` | :217 | same | same | `@DeductionCharge` | DeductionCharge (added by Live script) |
| computed | `NetReturnAmount = max(total − deduction, 0)` | :209-218 | same | same | `@NetReturnAmount` | NetReturnAmount (added by Live script) |
| const "Posted" | `ActionStatus` | :219 | same | same | `@ActionStatus` | ActionStatus |
| Session LoginName / Now | `EntryBy`, `EntryDate` | :220-221 | same | same | `@EntryBy`,`@EntryDate` | EntryBy, EntryDate |
| grid DataKey InvoiceDetailId | `CustomerReturnDetailDAO.InvoiceDetailId` | :238 | `SaveCustomerReturnDetail` | `SaveCustomerReturnDetail` :51-75 | `@InvoiceDetailId` | tblCustomerReturnDetail.InvoiceDetailId |
| retQtyTextBox | `ReturnQty` | :241-242 | same | same | `@ReturnQty` | ReturnQty |
| retamountTextBox | `ReturnAmount` | :239-240 | same | same | `@ReturnAmount` | ReturnAmount |
| new master id | `CustReturnId` | :237 | same | same | `@CustReturnId` | CustReturnId (identity via `SCOPE_IDENTITY`) |
| (detail id, qty) | stock row | :247 | `InsertDCStoreStockReturn(invoiceDetailId, qty, detailId)` | `CustomerReturnDAL.cs:181-219` | `@InvoiceDetailId,@ReturnQty,@SalesReturnDetailId` | tblDCStore (see §17) |
| `custReturnId` | posting | :251 | `PostDeliverySalesReturn` | :134-141 | `@ReturnId` | `sp_DeliverySalesReturnPosting` |

## 9. Dropdown Mapping
`payTypeDDL` loaded from `tblPaymentType` (`InvoiceBLL.PaymentTypeLoadBLL`, `.aspx.cs:32`) but the control is a code-only `new DropDownList()` — not in the aspx; no visible dropdown.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Search / textbox change | `Button2_Click` | `CustomerReturnBLL.LoadInvoiceNew(text)` → if rows: `LoadInvoice(InvoiceId)` fills labels, grid bound; else alert "Already Returned or Payment Exist or Invalid Delivery Invoice No. !!!" |
| Return Qty change | `retQtyTextBox_OnTextChanged` :327-347 | per row: if qty > available → alert "Cannot be greater then Avialable Quantity", blank; else amount = qty × (UnitPrice + UnitVatAmount); totals recomputed |
| Deduction change | `txtDeductionCharge_TextChanged` :349 → `CalculateReturnTotals` | recompute; if deduction > total → alert and reset to 0.00 |
| Save Return | `saveButton_Click` :182-257 | see §12 |
| Clear | `clearButton_OnClick` :399 | reset |

## 11. Frontend Flow
Enter delivery invoice no → search → grid of delivery lines with available qty → enter Return Qty per line (postback recalculates amounts/totals) → optional deduction (JS `calcNetReturn` mirrors server) → confirm dialog → save. `UpdatePanel` partial postbacks.

## 12. Backend Flow
`saveButton_Click` (:182-257), no try/catch, no transaction:
1. `Validation()` (§18).
2. Sum `totalqty`, `totalamnt`; parse deduction; if `deduction > totalamnt && totalamnt > 0` alert "Deduction charge cannot be greater than Total Product Return Amount!" and stop; `net = max(total − deduction, 0)`.
3. If `totalqty>0`: `SaveCustomerReturn` → `INSERT tblCustomerReturnMaster (InvoiceId, TotalQty, TotalAmount, DeductionCharge, NetReturnAmount, EntryBy, EntryDate, ActionStatus)` + `SCOPE_IDENTITY()` (`ClsCommonInternalDAL.SaveDataByInsertCommandWithIdentity`).
4. For each row with ReturnQty>0: `SaveCustomerReturnDetail` then `InsertDCStoreStockReturn(invoiceDetailId, qty, detailId)`.
5. `PostDeliverySalesReturn(id)` → `sp_DeliverySalesReturnPosting @ReturnId`.
6. `Clear()`; alert "Data Saved and Posted Successfully !!!".
Search: `LoadInvoiceNew(delinvno)` builds SQL by **string concatenation** of the textbox (SQL injection, `CustomerReturnDAL.cs:79-86`).

## 13. API Mapping
None.

## 14. Service Mapping
`CustomerReturnBLL` methods (thin pass-through): `LoadInvoiceNew`, `SaveCustomerReturn`, `SaveCustomerReturnDetail`, `InsertDCStoreStockReturn`, `PostDeliverySalesReturn`, `LoadCustomerInfoByCode`, `GetCompanyUnitById`, `GetDANameById`, `GetOrderSenderNameByOrderId`.

## 15. Repository Mapping
| DAL | SQL |
|---|---|
| `LoadInvoiceNew` :77-89 | `SELECT UNT.ComUnitName, ROUND(DeliveryNetAmount/Quantity,4) AS UnitPrice, *, (DeliveryTotalQuantity − ISNULL(prev.Qty,0)) AvialableQty, PackSize, NetAmount, tblInvoice.InvoiceId FROM tblInvoice ⟕ tblInvoiceDetail ⟕ tblCompanyUnit ⟕ tblOrder ⟕ (SUM(ReturnQty) per InvoiceDetailId from tblCustomerReturnDetail) ⟕ (SUM(PaymentAmount) per invoice from tblCustPayDetail) WHERE DelivaryInvoiceNo='<text>' AND ISNULL(ODR.IsDirect,0)=0 AND tblInvoice.InvoiceId NOT IN (SELECT InvoiceId FROM tblCustomerReturnMaster)` — **concatenated** |
| `LoadCustomerInfoByCode` :143-164, `GetCompanyUnitById`, `GetDANameById`, `GetOrderSenderNameByOrderId` | concatenated SQL (injection) |
| `SaveCustomerReturn/Detail` :17-75 | parameterised INSERTs |
| `InsertDCStoreStockReturn` :181-219 | `INSERT INTO tblDCStore … SELECT … FROM tblDCStore WHERE DCStoreId=(SELECT DCStoreId FROM tblInvoiceDetail WHERE InvoiceDetailId=@InvoiceDetailId) AND Trans_Type='Receive'` |
| `PostDeliverySalesReturn` :134-141 | `sp_DeliverySalesReturnPosting` via `UpdateAction` |
| `UpdateDCStoreQty` :112-116, `ApprovalUpdateDAL` :101-111, `LoadCustomerReturnApproval` :117-124 | present but not called by this page; no caller found in `Solution.Web` |

## 16. Stored Procedure Mapping
`sp_DeliverySalesReturnPosting(@ReturnId INT)` — **in repo** `Live_Deployment_Script_All_Updates.sql:558-812`. Column-level effect:
- Reads `tblCustomerReturnMaster` (InvoiceId, TotalAmount, DeductionCharge, NetReturnAmount) ⟕ `tblInvoice` (InvoiceNo, CustomerMasterId, ComUnitId) ⟕ `tblCustMaster`.
- Exits silently (`RETURN`) if invoice not found or `TotalAmount ≤ 0`. If `NetReturnAmount ≤ 0 and Total>0` recomputes net = total − deduction.
- Company = `CSTL_ERPDB_Unicorn..tblCompanyInfo` via `tblCompanyUnit` (default 1); Financial year = `CSTL_ACCDB_Unicorn.tblFinancialYear` Active/containing today (default id 2).
- Subsidiary: `tblSubsidiaryInfo(SubsidiaryTypeId=1, S_Info_Id=CustomerMasterId)` (**not created if missing**, stays 0); reference: `tblReferenceInfo(ReferenceTypeId=1016, R_Info_Id=InvoiceId)` (0 if missing).
- Voucher 1 and Voucher 2 (§19); sets `tblCustomerReturnMaster.IsPosting=1`.

## 17. Database Mapping
- **tblCustomerReturnMaster** (W): InvoiceId, TotalQty, TotalAmount, DeductionCharge, NetReturnAmount, EntryBy, EntryDate, ActionStatus('Posted'), IsPosting (column pre-exists — not created by the script). Live script adds DeductionCharge/NetReturnAmount DECIMAL(18,2) default 0.
- **tblCustomerReturnDetail** (W): InvoiceDetailId, ReturnQty, ReturnAmount, CustReturnId.
- **tblDCStore** (W — **stock effect, one ledger row per returned line**): copy of the *original* Receive row (StorageLocation, ProductCode, ProductName, PackSize, BatchNo, ExpDate, MfgDate, ChalanNo, ChalanDate, ComUnitId, ReqId, ReqChildId, StockInTransfarId) with `DCStoreId=MAX+1`, `TotalQuantity=0`, `ReceiveDate=GETDATE()`, `StockQty=@ReturnQty`, `DamageQty=0`, `StockRcvDate=GETDATE()`, `StockCondition='SalesReturn'`, `Trans_Type='Receive'`, `CostPerUnit`=original, `TotalCostAmount=@ReturnQty×CostPerUnit`, `ReferenceDCStoreId=original DCStoreId`, `ReferenceTransactionDate=GETDATE()`, `SalesReturnDetailId=detailId`, `ReturnFromInvoiceDetailId=InvoiceDetailId`. Only inserts if the original row is `Trans_Type='Receive'` (an invoice line referencing an 'Issue' row returns **no** stock, silently).
- **CSTL_ACCDB_Unicorn**: tblDebitCreditVoucherMaster/Detail (W). **tblCentralStore** (R via `DCS.TempReceiveId = CTRS1.ReceiveId`).
- R: tblInvoice, tblInvoiceDetail, tblCompanyUnit, tblOrder, tblCustPayDetail, tblCustMaster, tblDAInfo.

## 18. Validation Matrix
| Check | Where | Message (verbatim) |
|---|---|---|
| Return qty numeric int ≥ 0 | `Validation` :140-145 | Please enter a valid return quantity! |
| Return qty ≤ Available Qty | :147-152; :331-338 | Return quantity cannot be greater than Available Quantity! / Cannot be greater then Avialable Quantity |
| Σ return qty > 0 | :158-162 | Please specify return quantity for at least one product! |
| Payment type radio has a selection | :164-177 | Please select payment type !!! |
| Deduction ≤ total return amount | :203-207 / :378-383 | Deduction charge cannot be greater than Total Product Return Amount! |
| Search: no rows | :317 | Already Returned or Payment Exist or Invalid Delivery Invoice No. !!! |
| Not validated | — | payment already collected (only labelled; query does **not** filter on payments), status of the delivery, user's DC scope, expiry, duplicate click |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M08-031 | Only delivery invoices of **non-direct orders** (`tblOrder.IsDirect = 0/NULL`) are searchable, and an invoice that already has a `tblCustomerReturnMaster` row can **never be returned again** (whole-invoice, one-time). | CustomerReturnDAL.cs:79-86 | inline SQL | CONFIRMED |
| BR-M08-032 | Available quantity = `DeliveryTotalQuantity − Σ prior ReturnQty` per invoice detail; return qty is an integer 0…available; total must be >0. | CustomerReturnDAL.cs:79-84; DeliverySalesReturn.aspx.cs:131-162 | SQL + code-behind | CONFIRMED |
| BR-M08-033 | Line return amount = Return Qty × (UnitPrice + UnitVatAmount) where `UnitPrice` DataKey is the alias `ROUND(DeliveryNetAmount/Quantity,4)` (first of two `UnitPrice` columns) — VAT is added again on top of a delivery **net** amount. | DeliverySalesReturn.aspx.cs:341-343; CustomerReturnDAL.cs:79 | code-behind + SQL | CONFIRMED (double-VAT effect INFERRED) |
| BR-M08-034 | Deduction ≤ Total Return Amount (blocked at save, reset on edit); Net = max(Total − Deduction, 0). | .aspx.cs:203-210,378-388 | code-behind | CONFIRMED |
| BR-M08-035 | Return is saved with `ActionStatus='Posted'` — no approval step in this flow. | .aspx.cs:219 | code-behind | CONFIRMED |
| BR-M08-036 | **Stock effect:** returned qty is added as a new 'Receive' ledger row (`StockCondition='SalesReturn'`, TotalQuantity=0, StockQty=qty, cost = original cost) referencing the original batch row. | CustomerReturnDAL.cs:181-219 | inline SQL | CONFIRMED |
| BR-M08-037 | Voucher 1 (Journal, 'Approved', by 'admin'/'Auto Process', date=today, TableID 2018, CommonPK_ID=CustReturnId): **Dr Sales Return** (23/1020, fb 4170) = TotalAmount; **Cr Delivery Charge Income** (23/1032, fb 5968) = deduction if >0; **Cr Accounts Receivable** (23/2, fb 2112) = Net on customer subsidiary. | Live_Deployment_Script…:631-725 | SP | CONFIRMED |
| BR-M08-038 | Voucher 2 (TableID 2023) posts only if `StockValue>0`: **Dr Finished Goods** (23/1016, fb 4156) / **Cr COGS** (23/1021, fb 4162) where `StockValue = Σ (tblCentralStore.UnitPrice + VATPerUnit) × ReturnQty`, traced invoice detail → `tblDCStore.TempReceiveId` → `tblCentralStore.ReceiveId`. Inventory is valued at central-store *transfer price incl. VAT*, not at `CostPerUnit`. | Live_Deployment_Script…:727-805 | SP | CONFIRMED |
| BR-M08-039 | `IsPosting=1` on `tblCustomerReturnMaster` after posting; `sp_DeliverySalesReturnPosting` has **no idempotency check** — re-executing creates duplicate vouchers. | Live_Deployment_Script…:807-811 | SP | CONFIRMED |

## 20. Permission Matrix
Only `Session["LoginName"].ToString()` at save (NullReference when session lost). No role, DC or user scoping; any user with menu access can return any DC's delivery invoice.

## 21. Status Lifecycle
`tblCustomerReturnMaster.ActionStatus`: created directly as **'Posted'** (other values 'Pending'/'Approved' are referenced by `LoadCustomerReturnApproval/ApprovalUpdateDAL`, but no page in `Solution.Web` calls them — dormant approval design). `IsPosting` 0/NULL → 1 in the SP.

## 22. Error Handling
`Button2_Click` try/catch → `ShowMessageBox("Error: " + ex.Message)` (exposes raw exception text). `saveButton_Click`: none — exceptions surface as ASP.NET error page; partial state possible (master saved, some stock rows inserted, no voucher). `WriteLog` swallows errors.

## 23. Audit
`EntryBy` (LoginName), `EntryDate` on master; vouchers `EntryBy='admin'`, `ApprovedBy='Auto Process'`. Debug file log to `C:\Users\Programmer25\...\debug.log` (developer machine path; fails silently on server).

## 24. Integration
Accounts: cross-DB inserts into `CSTL_ACCDB_Unicorn` vouchers; `CSTL_ERPDB_Unicorn.Table_Integration_AccountsConnectivity` (PageId 23, FlagIds 1016, 1020, 1021, 1032, 2). `tblCentralStore` (warehouse valuation).

## 25. Reports
None launched from this page. Related report family: RPT-M08-02 (Delivery Return).

## 26. Dependencies
`CSTL_ACCDB_Unicorn`, `CSTL_ERPDB_Unicorn` databases; `tblCentralStore`; AjaxControlToolkit `FilteredTextBoxExtender`.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Search valid DEL invoice, return 2 of 5 on one line, no deduction, Save | master 'Posted', detail row, 1 new Receive row qty 2, 2 vouchers, IsPosting=1 | 031-039 |
| T2 | Positive | Deduction 50 on total 500 | Net 450; voucher Dr 500 / Cr 50 / Cr 450 | 034,037 |
| T3 | Negative | Return qty > available | alert, blank | 032 |
| T4 | Negative | Deduction > total | alert, save blocked | 034 |
| T5 | Negative | Search invoice already returned | "Already Returned or Payment Exist or Invalid…" | 031 |
| T6 | Edge | Line whose invoice DCStoreId points to an Issue-type row | detail saved, **no stock row** inserted | 036 |
| T7 | Edge | Customer without subsidiary in accounts DB | voucher AR line has SubsidoryLedgerId = 0 | 037 |
| T8 | Edge | Re-run `sp_DeliverySalesReturnPosting` for same id | duplicate vouchers | 039 |
| T9 | Security | Delivery invoice no `x' OR 1=1--` | injection executes (query concatenated) | — |
| T10 | Edge | VAT-bearing invoice | return amount includes VAT twice if `DeliveryNetAmount` already includes VAT — verify against invoice | 033 |

## 28. Reverse Engineering References
`DeliverySalesReturn.aspx(.cs)`; `CustomerReturnDAL.cs`; `CustomerReturnBLL.cs`; `Live_Deployment_Script_All_Updates.sql:16-39,98-115,557-815`.

## 29. Spec vs Actual
| Item | Specification | Actual |
|---|---|---|
| All | None existed | as documented |
| Script title says "Delivery Sales Return (Delivery Charge Deduction & Account Posting)" (`Live_Deployment_Script…:1-8`) | — | Matches the code: deduction charge + posting exist |

## 30. Known Gaps
SQL injection in `LoadInvoiceNew`, `LoadCustomerInfoByCode`, `GetCompanyUnitById`, `GetDANameById`, `GetOrderSenderNameByOrderId`; no transaction; no idempotency for posting; possible double VAT; `TotalQuantity=0` on 'SalesReturn' receive rows makes them look "corrupted" to `sp_Check_DCStore_StockAnomalies` (`StockQty > TotalQuantity`) and **erasable** by `sp_Adjust_DCStore_Stock_Only`'s general branch (`StockQty <> TotalQuantity` → reset to 0) — GAP-M08-006; `TableID 2018/2023 + CommonPK_ID` collision with `sp_SalesReturnPosting` (GAP-M08-005); voucher code numbering differs (`SUBSTRING(…,11,6)` here vs `RIGHT(…,5)` there) and, lacking a `LIKE 'JVP-%'` filter, can try to CONVERT a non-numeric substring from other voucher types on the same date; hard-coded `EntryBy='admin'`; hard-coded fallback GL ids and financial year 2; debug log path; dormant approval methods; radio "payment type" validation is dead UI.

## 31. Change Impact
`CustomerReturnDAL.InsertDCStoreStockReturn` is a candidate for reuse by other return pages; `sp_DeliverySalesReturnPosting` is only invoked from `CustomerReturnDAL.PostDeliverySalesReturn`. `tblCustomerReturnMaster` is read by delivery-return reports and the "not already returned" filter of this page. Changing GL mapping rows (PageId 23) alters postings of both return flows.
