---
page_id: PAGE-288
module: M08
page: ProformaorInvoiceReturn
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-288 — ProformaorInvoiceReturn (delete / roll back an undelivered invoice)

> **Evidence level:** Tier A (added to the candidate list because it is the only M08 page whose SP body is in the repo and that reverses stock). Read: `.aspx.cs` (122 lines), control inventory, `ProformaOrInvoiceReturn` DAL (all `Has*`, `Delete*`, `LoadDetailID`, `LoadStock`), and SP body `sp_Delete_ProformaInvoice` (`sp_Delete_ProformaInvoice_Update.sql:1-206`, read fully). `sp_DeleteDeliveryInvoice` body is **not** in the repo (database PARTIAL).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-288 |
| Module | M08 |
| Name / class | ProformaorInvoiceReturn / `SInventory_UI_ProformaorInvoiceReturn` (`.aspx.cs:10`) |
| Kind | Entry / destructive action |
| Size | code-behind 122 lines |

## 2. Purpose
CONFIRMED (checkbox labels "Invoice" / "Delivery", button "Delete"): remove an **invoice that has not been delivery-confirmed** (checkbox "Invoice", DB call `sp_Delete_ProformaInvoice`) or — when the hidden "Delivery" checkbox is used — a delivery invoice created today (`sp_DeleteDeliveryInvoice`), returning the goods to stock.

## 3. Business Objective
INFERRED: undo a mistaken invoice: release the stock that the invoice reserved (issue lines), reopen the source order for re-invoicing, and leave an audit trail (delete log + delete-record copy of detail rows).

## 4. Navigation
Menu-driven; outbound: self.

## 5. Route
`~/SInventory_UI/ProformaorInvoiceReturn.aspx`.

## 6. Source Files
`Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx(.cs)`; `Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs`; `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs`; `sp_Delete_ProformaInvoice_Update.sql`.

## 7. UI Control Inventory
| Control | Type | Required | Default | State |
|---|---|---|---|---|
| ProformaCheckBox | CheckBox text "Invoice" (AutoPostBack) | one of the two | unchecked | mutually exclusive with InvoiceCheckBox |
| InvoiceCheckBox | CheckBox text "Delivery" | — | unchecked | **`Visible=false`** in the first UpdatePanel (a second, duplicate block of the same IDs exists in the markup) |
| proformaInvoiceTextBox | TextBox (AutoPostBack) | Yes | empty | invoice no (or delivery invoice no) |
| submitButton / saveButton "Delete" | LinkButton/Button with confirm | — | — | `saveButton_Click` |
| LinkButton4 | cancel | — | — | `cancelButton_Click` → reload page |

## 8. Field Mapping
| UI | Handler | BLL | DAL | SP param | Table |
|---|---|---|---|---|---|
| proformaInvoiceTextBox (Invoice mode) | `saveButton_Click` :48-53 | `DeleteProforma` | `DeleteProformaDal` :116-122 | `@InvoiceCode`, `@User = Session["LoginName"]` | `sp_Delete_ProformaInvoice` |
| proformaInvoiceTextBox (Delivery mode) | :54-79 | `DeleteDeliveyInvoice` | `DeleteDeliveyInvoiceDal` :130-137 | `@DelivaryInvoiceNo`, `@User` | `sp_DeleteDeliveryInvoice` (body UNKNOWN) |

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
`ProformaCheckBox_CheckedChanged` / `InvoiceCheckBox_CheckedChanged` (exclusive, clear text); `proformaInvoiceTextBox_TextChanged` (existence check); `saveButton_Click` (delete); `cancelButton_Click`.

## 11. Frontend Flow
Tick "Invoice" → type invoice no → on blur `chkProforma` (`HasProforma`) must find an undelivered, non-adjust invoice, else alert "No Proforma Found or Invoice Created!!" and clear → Delete (confirm dialog) → alert "Delete successfully !!".

## 12. Backend Flow
Invoice mode: `DeleteProforma(text)` → `sp_Delete_ProformaInvoice` (below) → `Clear()` → alert. Delivery mode (hidden): `LoadDetailID(delivery no)`; for each detail `LoadStock(InvoiceDetailId)` = `SELECT * FROM tblDCStoreFreeze WHERE InvoiceDetailId=@id AND StockQty<>TotalQuantity`; if any freeze stock already consumed → alert "Stock Already Returned From Freeze Stock, Cant't Delete !!"; else `sp_DeleteDeliveryInvoice`. No try/catch.

## 13. API Mapping
None.

## 14. Service Mapping
`ProformaOrInvoiceReturnBLL.DeleteProforma/DeleteDeliveyInvoice/LoadDetailID/LoadStock/chkProforma/chkInvoiceForToday`.

## 15. Repository Mapping
`HasProforma`: `SELECT InvoiceNo FROM tblInvoice WHERE DelivaryInvoiceNo IS NULL AND (IsAdjustInvoice IS NULL OR IsAdjustInvoice=0) AND InvoiceNo=@InvoiceNo`. `HasInvoiceForToday`: `tblInvoice.DelivaryInvoiceNo=@ AND CONVERT(date,UpdateDate)=today`. `LoadDetailID`, `LoadStock` as above. All parameterised.

## 16. Stored Procedure Mapping
| SP | Body | Effect |
|---|---|---|
| `sp_Delete_ProformaInvoice(@InvoiceCode,@User)` | in repo, read fully | see BR-M08-050…053 |
| `sp_DeleteDeliveryInvoice(@DelivaryInvoiceNo,@User)` | **not in repo** | UNKNOWN (per comment in `SalesOperationRollbackDAL.cs:313-329` it inserts negative 'Issue'-style rows into `tblDCStore`) |
| `sp_Delete_ProformaInvoice_SubDeport`, `sp_DeleteDeliveryInvoiceSubdepo` | not in repo | sub-depot variants (M12) |

## 17. Database Mapping
For `sp_Delete_ProformaInvoice`: precondition `tblInvoice.InvoiceNo=@InvoiceCode AND DelivaryInvoiceNo IS NULL`. Per invoice detail (cursor ordered by InvoiceDetailId): **W tblDCStore** — `DELETE TOP (1)` the ledger row with `ReferenceDCStoreId=@DCStoreId AND Trans_Type='Issue' AND StockQty = −TotalQuantity` (releases the reservation), then `UPDATE tblDCStore SET StockQty=TotalQuantity WHERE DCStoreId=@DCStoreId AND Trans_Type='Receive' AND StockQty > TotalQuantity` (clamps a previously inflated receive row); **W tblProInvoiceReturnTrack** (ReturnTotalQuantity, DCStoreId, ReturnDate, ReturnExecutionDateTime, InvoiceDetailId, InvoiceId); **W tblInvoiceDetail_DeleterRecord** (full copy of 35 detail columns); **D tblInvoiceDetail**. After the loop: W `tblOrderDetail.[Status]=NULL` and `tblOrder.IsInvoice=0` (order re-opened, using the *last* cursor row's OrderId only); D `tblReturnAmount` for the invoice; D `tblInvoice`; W `tblInvoiceDeleteLog(InvoiceId, InvoiceNo, DeleteDateTime, DeleteBy)`.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| Invoice mode: not found / already delivered / adjust invoice | `proformaInvoiceTextBox_TextChanged` :92-100 | No Proforma Found or Invoice Created!! |
| Delivery mode: no invoice today | :102-121 | No Invoice Found!! |
| Delivery mode: freeze stock consumed | `saveButton_Click` :74-78 | Stock Already Returned From Freeze Stock, Cant't Delete !! |
| Neither checkbox / empty text | none: nothing happens (no message) | — |
| SP internal | `IF EXISTS(... DelivaryInvoiceNo IS NULL)` else silent no-op | — |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M08-050 | Only invoices **without a delivery invoice number** can be deleted through `sp_Delete_ProformaInvoice`; other invoices are silently ignored by the SP (page still says "Delete successfully"). | sp_Delete_ProformaInvoice_Update.sql:15; .aspx.cs:48-53 | SP + page | CONFIRMED |
| BR-M08-051 | **Stock reversal is by deleting the invoice's negative 'Issue' ledger row** (matched by `ReferenceDCStoreId`, `Trans_Type='Issue'`, `StockQty = −TotalQuantity`, one row per detail) — the Receive row's `StockQty` is *not* increased; over-inflated Receive rows are clamped to `TotalQuantity`. This fix replaced an earlier version that added quantity back to the receive row (see comments at `:112-123` and `sp_Adjust_DCStore_Stock_Only`). | sp_Delete_ProformaInvoice_Update.sql:112-130 | SP | CONFIRMED |
| BR-M08-052 | Audit trail: each detail is copied into `tblInvoiceDetail_DeleterRecord`, a `tblProInvoiceReturnTrack` row is written, and a `tblInvoiceDeleteLog` row records who/when. | sp_Delete_ProformaInvoice_Update.sql:132-165,201-202 | SP | CONFIRMED |
| BR-M08-053 | Deleting re-opens the source order (`tblOrder.IsInvoice=0`, `tblOrderDetail.Status=NULL`) so it can be re-invoiced; payments/returns tied to the invoice in `tblReturnAmount` are removed. | sp_Delete_ProformaInvoice_Update.sql:189-198 | SP | CONFIRMED |
| BR-M08-054 | "Delivery" deletion is refused if any of the invoice's freeze-stock rows has been partially consumed (`StockQty<>TotalQuantity`). | ProformaOrInvoiceReturn.cs:156-163; .aspx.cs:57-78 | code-behind | CONFIRMED |

## 20. Permission Matrix
Uses only `Session["LoginName"]` as delete-by. No role check; menu-based. Destructive, no second approver.

## 21. Status Lifecycle
Invoice: proforma (no `DelivaryInvoiceNo`) → deleted (row removed; log + delete-record retained). Order: `IsInvoice` 1→0.

## 22. Error Handling
None in page; SP has no TRY/CATCH or transaction — a mid-loop failure leaves partially deleted details (each detail is deleted individually before the header).

## 23. Audit
`tblInvoiceDeleteLog`, `tblInvoiceDetail_DeleterRecord`, `tblProInvoiceReturnTrack` (see BR-M08-052). Log insert uses `SELECT ... FROM tblInvoice` executed *before* the delete (`:181-183`).

## 24. Integration
None. (Accounting reversal of the deleted invoice, if any posting existed, is **not** performed here.)

## 25. Reports
None.

## 26. Dependencies
`tblInvoice`, `tblInvoiceDetail`, `tblDCStore`, `tblOrder`, `tblOrderDetail`, `tblReturnAmount`, `tblDCStoreFreeze`, audit tables above; `sp_Adjust_DCStore_Stock_Only`/`sp_Check_DCStore_StockAnomalies` are the repair/diagnostic companions.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Delete an undelivered invoice with 2 lines | Issue rows removed; detail copied to DeleterRecord; invoice removed; order re-opened; delete log row | 050-053 |
| T2 | Negative | Invoice already delivery-confirmed | text box cleared with "No Proforma Found or Invoice Created!!" | 050 |
| T3 | Edge | Two invoice lines from the same batch with equal qty | `DELETE TOP (1)` removes one row per line — OK; if quantities differ from issue rows, no row deleted and stock stays reserved | 051 |
| T4 | Edge | Receive row previously inflated (`StockQty>TotalQuantity`) | clamped to TotalQuantity | 051 |
| T5 | Negative | Delivery mode with consumed freeze stock | blocked message | 054 |
| T6 | Edge | Order with several invoices | only last detail's order id is reset | 053 |

## 28. Reverse Engineering References
`ProformaorInvoiceReturn.aspx.cs`; `ProformaOrInvoiceReturn.cs:14-165`; `sp_Delete_ProformaInvoice_Update.sql`; `SalesOperationRollbackDAL.cs:105-190,313-360` (parallel rollback logic in C#).

## 29. Spec vs Actual
None existed. SP header comments (Bengali) state intent: "do not ADD StockQty back to the Receive row; delete the Issue line instead" — matches implementation.

## 30. Known Gaps
No transaction/TRY-CATCH in SP; silent no-op message ("Delete successfully") when nothing was deleted; `DELETE TOP(1)` matching on exact quantity can miss rows; order reset uses only the last cursor row's OrderId; no accounting reversal; `sp_DeleteDeliveryInvoice` unavailable for review; duplicate control IDs in the aspx markup.

## 31. Change Impact
`sp_Delete_ProformaInvoice` shares its ledger contract (Issue rows referencing Receive rows) with `sp_Adjust_DCStore_Stock_Only`, `sp_Check_DCStore_StockAnomalies`, `SalesOperationRollbackDAL`, `InvoiceDAL.UpdateDCStoreQuantity` and the stock-transfer flows (M11). Any change to how issues are recorded must be mirrored here.
