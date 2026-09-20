---
page_id: PAGE-413
module: M11
page: ReceiveProductByChalanByDC
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-413 — ReceiveProductByChalanByDC (DC receives an STO shipment from the warehouse)

> **Evidence level:** Tier A — `.aspx.cs` (247 lines) read in full, control/DataKey inventory of the `.aspx`, `RequisitionBLL.DCStockIn2/SaveDCStoreFreeze2`, `RequisitionDAL` (`DCStockInDAL`, `DCStockInTransDAL`, `StockInTransfarStatusUpdate`, `UpdateReceiveIssueStatus`, `GetStockInTransfarByReqIdDAL`, `SaveDCStoreFreeze2`), `SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan`. The SAP SP `MakeRESTRequest` body is not in the repo. Opened from `StockReceiveByDC.aspx` (PAGE-417) with `?ReqId=`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-413 |
| Module | M11 |
| Name / class | ReceiveProductByChalanByDC / `SInventory_UI_ReceiveProductByChalanByDC` (`.aspx.cs:13`) |
| Kind | Entry / transaction — **stock in at DC**, closes the STO |
| Size | code-behind 247 lines, aspx 375 lines |

## 2. Purpose
CONFIRMED (labels "UnRcvQty", "Stock Already Received!!"): the destination DC confirms physical receipt of the goods issued against a Stock Transport Order (challan `CLN-…`), entering per line the received quantity or the unreceived/damaged quantity.

## 3. Business Objective
INFERRED: bring warehouse-issued stock into the DC ledger, segregate damaged/short quantities into restricted (freeze) stock, close the STO, and notify SAP that the delivery arrived.

## 4. Navigation
Inbound: `StockReceiveByDC.aspx` (list of `tblRequisition` with `Submit='OK' AND ReceiveIssue` empty for the user's DC). Outbound: `StockReceiveByDC.aspx` (back), `../SInventory_RPTVIEW/StockReceiveReportViewer.aspx?reqId=` (popup after save).

## 5. Route
`~/SInventory_UI/ReceiveProductByChalanByDC.aspx?ReqId=<n>`.

## 6. Source Files
View/code-behind `Solution.Web/SInventory_UI/ReceiveProductByChalanByDC.aspx(.cs)`; `RequisitionBLL.cs` (`GetRequisitionInfoByReqId` :195, `GetStockInTransfarByReqId` :404, `DCStockIn2` :433-448, `SaveDCStoreFreeze2` :465-475); `RequisitionDAL.cs` (`GetStockInTransfarByReqIdDAL` :498-505, `DCStockInDAL` :507-541, `DCStockInTransDAL` :544-572, `StockInTransfarStatusUpdate` :574-581, `UpdateReceiveIssueStatus` :582-592, `SaveDCStoreFreeze2` :627-652, `DcStockParameters` :822-846); `SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:548-577`.

## 7. UI Control Inventory
| Control | Type / label | Required | Default / state | Source |
|---|---|---|---|---|
| clnNoTextBox | TextBox RO | — | `tblRequisition.IssueChalanNo` | `LoadInfo` :42-52 |
| clnDateTextBox | TextBox RO | — | `IssuChalanDate` | |
| rcvDateTextBox | TextBox RO | — | today | becomes `StockRcvDate` |
| driverNameTextBox, truckTextBox | TextBox RO | — | `DriverName`, `TruckNo` | |
| hdComUnitId, hdReqId | HiddenField | — | requisition's `ComUnitId`, ReqId | |
| rcvGridView `DataKeyNames=ReqId,ReqChildId,StockInTransfarId` | GridView | rows | `GetStockInTransfarByReqId` (`tblStockInTransfar WHERE IsTransfared IS NULL AND ReqId`) | cols 0-9: ProductCode, ProductName, PackSize, BatchNo, Quantity (issued), ExpDate, ReceiveDate, `rcvQtyTextBox`, `damageTextBox` (AutoPostBack), MfgDate |
| rcvQtyTextBox | TextBox | (not validated) | issued qty | `rcvQtyTextBox_TextChanged` sets damage = issue − rcv |
| damageTextBox | TextBox "UnRcvQty" | **Yes** (non-empty) | 0/blank | `damageTextBox_TextChanged`: if damage > issue qty → alert "Quantity Must be equal or less then Issue Qty." and blank; else rcv = issue − damage |
| submit (`submitButton_Click`) | LinkButton + `sweetAlertConfirm_Submit` | — | — | `Save()` |
| backLinkButton | LinkButton | — | — | redirect |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | SQL param | Table.column |
|---|---|---|---|---|---|---|
| grid cells 0-3 | ProductCode, ProductName, PackSize, BatchNo | `Save` :92-113 | `DCStockIn2` :433-448 | `DCStockInDAL` :507-541 | `@ProductCode…@BatchNo` | tblDCStore.ProductCode, ProductName, PackSize, BatchNo |
| Quantity (cell 4) | `TotalQuantity` | | | | `@TotalQuantity` | TotalQuantity (**issued** qty) |
| rcvQtyTextBox | `StockQty` | | | | `@StockQty` | StockQty (**received** qty) |
| damageTextBox | `DamageQty` | | | | `@DamageQty` | DamageQty |
| ExpDate / Mfgdate / ReceiveDate | dates | | | | `@ExpDate,@MfgDate,@ReceiveDate` | ExpDate, MfgDate, ReceiveDate (= STO pick lot's receive date) |
| rcvDateTextBox | `StockRcvDate` | | | | `@StockRcvDate` | StockRcvDate |
| clnNo/clnDate | `ChalanNo`, `ChalanDate` | | | | | ChalanNo, ChalanDate (issue challan) |
| hdComUnitId | `ComUnitId` | | | | | ComUnitId |
| DataKeys | `ReqId`, `ReqChildId`, `StockInTransfarId` | :110-112 | | | | ReqId, ReqChildId, StockInTransfarId |
| DAL constants | `StorageLocation` (from DCStockNew, blank), `StockCondition='Available'`, `Trans_Type='Receive'`, `CostPerUnit = tblStockInTransfar.UnitPrice`, `TotalCostAmount = tblStockInTransfar.TotalPriceAmount`, `ReferenceDCStoreId = own DCStoreId`, `ReferenceTransactionDate = ReceiveDate` | | | :527-537 | | same |
| damage | freeze row: `DCStoreId`, product/batch/exp/dates, `TotalQuantity=StockQty=damage`, `DamageQty=0`, `StockCondition='Restricted'` | `Save` :117-140 | `SaveDCStoreFreeze2` (`DCStoreFreezeId = MAX+1`) | `SaveDCStoreFreeze2` :627-652 | | tblDCStoreFreeze.* |

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
Submit → `submitButton_Click` → `Save()`; Back → `StockReceiveByDC.aspx`; quantity edits → server recalculation only.

## 11. Frontend Flow
Page_Load (first only): `ReqId` from QueryString → `LoadInfo` (requisition → challan header) and `LoadGrid` (pending `tblStockInTransfar` rows, else alert "Stock Already Received!!"). User adjusts UnRcvQty or Received qty per line (each edit posts back) → Submit (SweetAlert) → alert "Data Save Successfully!!" + report popup.

## 12. Backend Flow
`Save` (:85-159): `Validation()` (every damage box non-empty). Per grid row: build `DCStockNew`; **`RequisitionBLL.DCStockIn2`**: `DCStoreId = MAX(DCStoreId)+1` → `DCStockInTransDAL(DCStoreId, StockInTransfarId, 'ReceiveProductFromWH', StockQty)` (insert into `tblDCStoreTransaction` unless the same (DCStoreId, Id, Type) exists) → `DCStockInDAL` (**insert only if no `tblDCStore` row exists for the `StockInTransfarId`** — idempotent) → `StockInTransfarStatusUpdate` (`tblStockInTransfar.IsTransfared='OK'`) → `UpdateReceiveIssueStatus` (`tblRequisition.ReceiveIssue='OK', ReceiveIssueDate=StockRcvDate`). If damage>0: `SaveDCStoreFreeze2` (restricted freeze). After the loop: `_DAL.MakeRESTRequestWithUpdateChallan(clnNoTextBox.Text)` inside try/catch **that swallows every error** (executes SP `MakeRESTRequest(@StoNo, @RcvBy=Session["UserId"])`, opening its own SQL connection). Alert, report popup, `Clear()`. No transaction; the `DCStoreId` returned is a pre-computed MAX+1 even when the guarded insert is skipped.

## 13. API Mapping
Outbound SAP call is inside SP `MakeRESTRequest` (name suggests an HTTP REST request issued from SQL Server; body UNKNOWN).

## 14. Service Mapping
`RequisitionBLL.GetRequisitionInfoByReqId`, `GetStockInTransfarByReqId`, `DCStockIn2`, `SaveDCStoreFreeze2`; `SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan`.

## 15. Repository Mapping
See §6 line refs: `GetStockInTransfarByReqIdDAL`: `SELECT * FROM tblStockInTransfar WHERE IsTransfared IS NULL AND ReqId=@ReqId`; `DCStockInDAL`: `IF NOT EXISTS(SELECT 1 FROM tblDCStore WHERE StockInTransfarId=@StockInTransfarId) BEGIN INSERT … END`.

## 16. Stored Procedure Mapping
| SP | Body | Effect |
|---|---|---|
| `MakeRESTRequest(@StoNo, @RcvBy)` | not in repo | UNKNOWN — SAP notification of STO receipt |
The related ledger SP `tbl_INV_CentralStoreStoIssue` (issue side, see PAGE-420 §16) has two conflicting definitions; this page is the receiving end of that chain.

## 17. Database Mapping
- **tblDCStore — stock effect (one row per received line):** `DCStoreId=MAX+1`; `TotalQuantity`=issued qty; `StockQty`=**received** qty; `DamageQty`=entered damage; `StockCondition='Available'`; `Trans_Type='Receive'`; `CostPerUnit`=`tblStockInTransfar.UnitPrice` (trade price excl. VAT); `TotalCostAmount`=`tblStockInTransfar.TotalPriceAmount` (**incl. VAT**, so ≠ qty × CostPerUnit); `ReferenceDCStoreId`=own id; `ReferenceTransactionDate`=lot receive date; ReqId, ReqChildId, StockInTransfarId.
- **tblDCStoreFreeze** (W when damage>0): StorageLocation '', product fields, TotalQuantity=StockQty=damage, `StockCondition='Restricted'`, ComUnitId, DCStoreId of the new receive row.
- **tblDCStoreTransaction** (W): DCStoreId, Date, Id (StockInTransfarId), Type 'ReceiveProductFromWH', Quantity (Quantity = received qty; insert guarded by NOT EXISTS on DCStoreId+Id+Type).
- **tblStockInTransfar** (W): `IsTransfared='OK'`. **tblRequisition** (W): `ReceiveIssue='OK'`, `ReceiveIssueDate`.
- No `tblCentralStore` change here (issued at pick). The weighted-average/central batch ledger already reduced at issue.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| UnRcvQty (damage) non-empty on every row | `Validation` :69-83 | Please fill out UnRcvQty !! |
| damage ≤ issued | `damageTextBox_TextChanged` :213-216 | Quantity Must be equal or less then Issue Qty. |
| pending rows exist | `LoadGrid` :57-65 | Stock Already Received!! |
| Not validated | | received qty numeric / ≥0 / ≤ issued (only damage is bounded; a user can key rcv > issued in `rcvQtyTextBox`); received + damage = issued; user's DC vs requisition DC; `ReqId` missing (NullReference) |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M11-029 | Only STO lines not yet transferred (`tblStockInTransfar.IsTransfared IS NULL`) of the requisition are shown; when none remain the page reports "Stock Already Received!!". | RequisitionDAL.cs:498-505; .aspx.cs:57-65 | DAL | CONFIRMED |
| BR-M11-030 | Received qty defaults to issued qty; unreceived (damaged) qty is entered, bounded by issued qty, and required on every row (0 allowed). | .aspx.cs:69-83,192-243 | code-behind | CONFIRMED |
| BR-M11-031 | **Stock effect:** one `tblDCStore` Receive row per line, `StockQty` = received qty (not issued), `TotalQuantity` = issued qty, guarded so a `StockInTransfarId` can create only one row. | RequisitionDAL.cs:507-541 | inline SQL | CONFIRMED |
| BR-M11-032 | Unreceived/damaged qty is **not** added to available stock: it is written to `tblDCStoreFreeze` as 'Restricted' (freeze stock) at the new receive row. | .aspx.cs:117-140 | code-behind | CONFIRMED |
| BR-M11-033 | Valuation: `CostPerUnit` = STO unit price (excl. VAT); `TotalCostAmount` = STO total incl. VAT — the DC ledger cost is the transfer price, not the warehouse weighted-average cost, and is internally inconsistent. | RequisitionDAL.cs:531-534 | inline SQL | CONFIRMED |
| BR-M11-034 | Completion flags: `tblStockInTransfar.IsTransfared='OK'` per line and `tblRequisition.ReceiveIssue='OK'` (removes the STO from the DC's receive list). | RequisitionBLL.cs:441-444 | BLL | CONFIRMED |
| BR-M11-035 | SAP hand-off: after saving, the challan/STO number is passed to `MakeRESTRequest` with the user id; any failure is ignored, so the ERP receipt stands even if SAP is not notified; no state flag is set by this page. | .aspx.cs:146-153; SAP_IntrigationPointDAL.cs:548-577 | code-behind | CONFIRMED (SP body UNKNOWN) |

## 20. Permission Matrix
No role checks in this page. `StockReceiveByDC.aspx` (list) filters by user: Admin sees all DCs, others `Session["UserId"]`-mapped DCs (`DCLoad(ddl, UserId)`), redirects to Login if `Session["UserType"]` is empty — but a user can open this page directly with any `?ReqId=`. `Session["UserId"]` is dereferenced inside the SAP DAL (throws → swallowed).

## 21. Status Lifecycle
`tblStockInTransfar.IsTransfared`: NULL → 'OK'. `tblRequisition.ReceiveIssue`: NULL/'' → 'OK' (+ `ReceiveIssueDate`). Freeze: `StockCondition='Restricted'`. Stock ledger: Receive row 'Available'.

## 22. Error Handling
No try/catch except the swallowed SAP call. Any failure mid-loop leaves earlier lines received (no transaction) — re-opening the page then shows the remaining lines (idempotent per StockInTransfarId).

## 23. Audit
`StockRcvDate` user-visible; no receiving-user column written by this page (`tblDCStoreTransaction` stores no user). SAP call carries user id.

## 24. Integration
SAP REST notification via SQL `MakeRESTRequest`. Report popup `StockReceiveReportViewer` (RPT-M11-01).

## 25. Reports
`StockReceiveReportViewer.aspx` → data `RequisitionBLL.DCStoreReport(reqId)` (tblDCStore ⟕ … ⟕ tblCentralStore ⟕ tblMIGODetail, `WHERE tblDCStore.ReqId=@ReqId`) (RPT-M11-01).

## 26. Dependencies
PAGE-420/259/219 (STO chain), PAGE-417 (list), tables above, SAP DB link.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Receive full issued qty, damage 0 | Receive row StockQty = issued; IsTransfared OK; ReceiveIssue OK | 031,034 |
| T2 | Positive | Issued 100, damage 10 (rcv 90) | Receive row TotalQuantity 100, StockQty 90, DamageQty 10; freeze 10 Restricted | 031,032 |
| T3 | Negative | Damage 120 > 100 | alert, blank | 030 |
| T4 | Negative | Leave damage blank | "Please fill out UnRcvQty !!" | 030 |
| T5 | Edge | Key received 150 > issued 100 | accepted (no bound); StockQty 150 | 030 |
| T6 | Edge | Re-submit after success | grid empty "Stock Already Received!!"; DAL guard prevents second row | 029,031 |
| T7 | Edge | SAP unreachable | receipt saved; error swallowed | 035 |
| T8 | Integration | Run `sp_Adjust_DCStore_Stock_Only` afterwards | receive rows with damage (StockQty<TotalQuantity) reset to TotalQuantity → damage qty double-counted | GAP-M11-003 |

## 28. Reverse Engineering References
`ReceiveProductByChalanByDC.aspx.cs`; `RequisitionBLL.cs:404-475`; `RequisitionDAL.cs:498-652,822-846`; `SAP_IntrigationPointDAL.cs:548-577`.

## 29. Spec vs Actual
None existed.

## 30. Known Gaps
No transaction; received qty unbounded; valuation inconsistency; SAP failure invisible; `DCStoreId` returned even when insert skipped (freeze row may point to a non-existing DCStoreId); no receiver audit; direct URL access by `ReqId`; `sp_Adjust_DCStore_Stock_Only` conflict.

## 31. Change Impact
`RequisitionBLL.DCStockIn2/DCStockInDAL` also used by sub-depot and other receive pages (`StockReceive`, `TransferStockReceiveByDC` use `ChalanBLL` variant); `tblStockInTransfar.IsTransfared` and `tblRequisition.ReceiveIssue` drive STO reports and the receive list.
