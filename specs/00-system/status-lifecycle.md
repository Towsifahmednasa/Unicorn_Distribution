# Status Lifecycle (system level)

Compiled from each module README. Status columns/values are code-derived; where stored procedures set states (bodies absent) transitions are UNKNOWN.

## M01 — Authentication, Session & Navigation

- **User account** (as seen by login): `tblUser.UserStatus = 'active'` required (M02 edits it: 'Active'/'Inactive').
- **Password-change flag:** `IsPasswordChange` 0/NULL → (ChangePassword/UserProfile) 1. Who resets to 0: UNKNOWN.
- **Session:** none → (login) authenticated → (logout: keys blanked, session object survives) → (timeout 24000 min / recycle) gone. Tracking row: `IsActiveSession` 1 → 0 only when a later login reuses the same SessionId.

## M02 — User, Role & Permission Administration

- **User:** `UserStatus` Active ⇄ Inactive (PAGE-015); login requires `'active'`. `IsPasswordChange` 0/NULL → 1 (M01). No delete UI except legacy `UserBLL.DeleteUserInfo` (physical `DELETE FROM tblUser`, legacy pages).
- **Role:** `IsActive` true/false with date; block rule UNKNOWN.
- **Role permission / approval chain / approver list / DC set:** replace-all sets, no status.
- **Order delegation:** time window From–To.

## M03 — Organisation & Market Structure Master

- Node active flag (all hierarchy levels): `IsActive` + `AcOrInAcDate`; deactivation guarded by `sp_check_Vali_MarketStructure`.
- Structure transfer request: `Requested → Approved` (INFERRED); no reject.
- Bulk market import: `Staged → Applied` (approve via `sp_Save_MarketPropToTable`).
- Financial-year processing: `(none) → Completed` opening (`tblOpeningBalanceFinancialYearLog.Status='Completed'`) gates purge.
- Deposit: `IsDelete` soft flag (other page, 60-day window).

## M04 — Product, Pricing & Promotion Master

- Campaign: `IsActive` flag + `FromDate..Todate`; no transition code; edit replaces the row (live page) - PAGE-111 sec 21.
- Unit price: Active row -> archived (IsActive 0, InActiveDate) in `tblUnitPriceRollback` + new active row - PAGE-157 sec 21.
- Product: `IsActive` bit only; no delete.
- Quoted price / promo tag / promo qty: no status column handled in C#.

## M05 — Customer & Provider Master

- **Customer** `tblCustMaster.ActionStatus`: `0` Pending → `1` Verified → `2` Approved / `3` Rejected (labels CONFIRMED in `sp_Get_CustMasterList_Approve`); web create ⇒ `2`; transitions of 0/1/3 by approval flow (UNKNOWN SP). `IsActive` separate.
- **Credit limit**: `Posted` → `Approved` | `Reject`; edit ⇒ `Posted`; delete ⇒ `IsActive=0`.
- **Excel batch**: Uploaded → Verified (`VerifyedAll`) → Transferred (`Transfer`) | Deleted (before transfer).
- **Property import batch**: Pending → Approved (INFERRED).

## M06 — Order Management

| State carrier | Values seen | Set by |
|---|---|---|
| `tblOrder.ActionStatus` | `'2'` (web manual, invoiceable), `'Accepted'`, `'Approved'` (approval text), other approval texts (UNKNOWN) | `sp_Save/Update_ManualOrderMaster` ('2'); `OrderListDAL.ApprovalUpdateDAL` (action text); app orders: UNKNOWN |
| `tblOrder.IsSpecialApproval` | 1 = needs special approval | UNKNOWN producer |
| `tblOrder.IsInvoice` | 0 → 1 ('True') on invoice creation; 1 → 0 on proforma delete | `OrderInfoDAL.UpdateInvoiceStatus`; `sp_Delete_ProformaInvoice`; `sp_Process_ProformaInvoiceByOrderId` (UNKNOWN) |
| `tblOrder.IsPrepareforInvoice` | 1/NULL = ready | manual SPs |
| `tblOrder.ComUnitId` NULL → set | DC assignment | `sp_Update_OrderDC` (UNKNOWN), OrderEdit |
| `tblOrderDetail.Status` | NULL, `'Undelivered'` | PAGE-218 (`InvoiceDAL.UpdateOrder`), delete-proforma resets NULL |
| `tblInvoice.DelivaryInvoiceNo`/`DeliveryInvoiceStatus` | NULL → `DEL-…`; Full / Partial / Reject | PAGE-215/214 |
Flow: `[created ActionStatus='2' | app order (approval?) ] → [special approval → 'Approved'] → [DC assigned] → [credit ok] → IsInvoice=1 (proforma) → delivery Full/Partial/Reject`; reverse: delete proforma → `IsInvoice=0`.

## M07 — Proforma, Invoicing & Delivery

**Order/invoice (proforma -> delivery):**
1. Order created (M06) `tblOrder.IsInvoice = 0`.
2. Invoice creation (M06 by-order pages, or `sp_AutoInvoiceGeneration`): `tblInvoice` row with `DelivaryInvoiceNo IS NULL` (proforma), `tblInvoiceDetail` batch rows, `tblDCStore` Issue rows (stock reserved), `tblOrder.IsInvoice = 1` (INFERRED from the reset in `sp_Delete_ProformaInvoice`).
3. Proforma deletion (PAGE-277, M06, `ProformaorInvoiceReturn`): row deleted, Issue rows removed, `IsInvoice = 0`, `tblOrderDetail.Status = NULL`, logged.
4. Loading summary (PAGE-271): `LoadingSummaryStatus` = `Full Dues` | `Cash` | `Rejection` (`sp_UP_LoadingSummary`); `Partial Dues` -> partial screen. "Full Dues" also runs `sp_DeliveryConformationFull_New` (delivery invoice: `DelivaryInvoiceNo`, `DeliveryInvoiceStatus` 'Full'/'Partial', `DeliveryTpGrandTotal` - INFERRED) and `sp_Sales_DeliverySalesPosting` (voucher TableID 2014).
5. Payment confirmation (PAGE-264 -> PaymentPartial M09): `PaymentInvoiceNo` (NULL until confirmed; `RTN-`+delivery no on partial), `PaymentInvoiceStatus` ('Reject'), `PaymentStatus` ('Partial').
**DC picking:** presence in `tblDCPickingDetail` = picked (no status column).
**Requisition/STO challan:** `tblRequisition.CreatePicking` NULL/'' -> 'OK' and `Submit` = 'OK' at PAGE-259 save; `tblRequsitionChild.IsPicking/IsIssue` = 'OK'; `tblStockInTransfar.IsIssue` = 'OK'; later DC receipt (other module) moves stock into `tblDCStore` Receive rows.
Details per page in §21 of each Tier-A spec.

## M08 — Sales Return & Invoice Return

| Object | States | Set by |
|---|---|---|
| tblReturnInvoice (market return) | created (IsPosting NULL/0) -> posted (IsPosting=1) | `sp_SalesReturnPosting` (PAGE-294) |
| tblCustomerReturnMaster (delivery return) | ActionStatus 'Posted' at creation (the design also has pending/approved: `ApprovalUpdateDAL`, unused); IsPosting 0/NULL -> 1 | PAGE-287 / `sp_DeliverySalesReturnPosting` |
| tblInvoice second-time return | `sndReturnInvoiceStatus` NULL -> Full / Partial / Reject; `SndReturnInvoiceNo` set (`SRN-`+InvoiceNo) | PAGE-298 |
| tblInvoiceDetailReturn.sndReturnStatus | Full / Partial / Reject | PAGE-298 |
| tblDCStoreFreeze.StockCondition | 'ReturnStock' (legacy return) | PAGE-290 |
| Invoice deletion | invoice row deleted; log rows written; `tblOrder.IsInvoice` 1 -> 0 | `sp_Delete_ProformaInvoice` (PAGE-288) |
| Worklist gate | shown while `PaymentInvoiceNo` not null and `SndReturnInvoiceNo` null | PAGE-296 |

## M09 — Collections, Payments & Money Receipts

- **Invoice payment stage** (`tblInvoice.PaymentInvoiceStatus` / `PaymentInvoiceNo`, set by PAGE-334): none → Full | Partial | Reject (with `RTN-…` number). Line-level `tblInvoiceDetail.PaymentStatus` same domain.
- **Invoice collection status** (`tblInvoice.PaymentStatus`, `PaymentAmount`, written by UNKNOWN SP `sp_Update_InvoiceFinalPayment` from PAGE-311/314/315/318 with "Full"/"Partial"): NULL → Partial → Full; reset to NULL by PAGE-335. Collectable filters elsewhere use `PaymentStatus IS NULL OR 'Partial'`.
- **Payment rows** (`tblCustomerPay`, `tblCustPayDetail`): no status column; created final; removed by PAGE-335 (hard delete, first payment only) or PAGE-313 (SP delete with log).
- **Accounts voucher**: created `Approved` immediately (CSTL_ACCDB_Unicorn `ActionStatus`); never reversed by any code in this module.
- **Deposit slip** (`tblCompanyWiseDeposit`): active (`IsDelete=0/NULL`) → deleted (`IsDelete=1`, DeleteBy/DeleteDate) within 60 days by two named users.

## M10 — Warehouse, DC Stock & Inventory

- Stock-in (`tblWHStockInMaster.Status`): new -> Posted -> Approved | Reject; Reject -> (edit) Posted; blank via WarehouseStockInEdit (defect).
- Depot voucher (`tblDeStockOutMaster.Status`): Posted -> Approved (SP) | Reject; Approved not deletable.
- WH adjustment (`ActionStatus`): Approved at creation.
- Sample conversion (`tblSampleStockForDcMaster.Status`): Posted only.
- Stock condition: Available -> (freeze) condition in freeze table -> (release) qty returned; no status column.
- tblDCStore rows: Trans_Type Receive / Issue; StockCondition Available / B2BReceive / B2BIssue / TransferReceive / TransferIssue / SalesReturn / Return / Restricted / Blocked (values seen in code).

## M11 — Stock Transfer & Chalan Receive

| Object | States | Set by |
|---|---|---|
| tblChalanInfo.IsDeliver (DC-DC) | 'False' (created, in transit) -> 'True' (received) | PAGE-412 insert / `ChalanDAL.ChalanUpdate` (PAGE-423) |
| tblChalanInfo SAP flags | SAP_ChallanSend 0->1; SAP_Challan_ConfirmationSend 0->1 | `sp_SAP_Up_SAP_ChallanSendByChalanId` / `...ConfirmByChalanId` (external caller) |
| tblDepotToWHChalanInfo | IsTransfer NULL -> 'OK' (stock deducted, PAGE-409); IsDeliver 'False' -> 'OK' (WH received, PAGE-414) | PAGE-409, PAGE-414 |
| STO chain | tblRequisition Submit 'OK' (issued) -> ReceiveIssue 'OK' (DC received); tblStockInTransfar IsIssue 'OK' -> IsTransfared 'OK' | PAGE-219/259, PAGE-413 |
| tblDCStore ledger | Issue row (B2BIssue, negative) at DC-DC send; Receive row 'B2BReceive'/'Available' at receipt | PAGE-412 / PAGE-423 / PAGE-413 |
| Freeze stock | tblDCStoreFreeze / tblWhStoreFreeze StockCondition 'Restricted' | receive pages |

## M12 — Sub-Depot Operations

**DC -> Sub-depot chalan** (`tblSubDepotChalanInfo.IsDeliver`): `[none] --PAGE-450 submit--> IsDeliver=false (in transit; DC tblDCStore.StockQty already reduced)` `--PAGE-446 first line saved--> IsDeliver=true (sub-depot tblSubDepotStore rows Available; damage -> tblSubDepotStoreFreeze Restricted)`. No reject/cancel/partial state (BR-M12-010,013,014,016,017).
**Sub-depot -> DC return** (`tblSubDepotChalanReturnInfo.IsDeliver`): `PAGE-451 submit -> false (sub-depot StockQty already reduced)` `--PAGE-441 first line--> true (new tblDCStore row, ComUnitId hard-coded 6; damage -> tblDCStoreFreeze Restricted)` (BR-M12-011,018,019).
**Freeze row** (`tblSubDepotStoreFreeze.StockCondition`): created `Restricted` (receive damage) or `ReturnStock` (sales return, `ReturnInvoiceDAL.cs:1212`); qty released back to `Available` store row by PAGE-453 (Restricted only by login 21900); `Blocked` counted by report but never created by M12 code (BR-M12-028..031).
**Adjustment voucher** (`tblSubDepotStockOutMaster.Status`): `Posted -> Approved` (SP `sp_UD_SubDcStockOutApproval`, stock effect UNKNOWN); `Reject` wrongly updates the DC table (BR-M12-023..026). Direct adjustment (PAGE-448) has no status; stock reduced at once (BR-M12-021).
**Sub invoice**: order `IsInvoice 0` -> proforma (`DelivaryInvoiceNo` NULL) -> delivery `DeliveryInvoiceStatus Full/Partial/Reject` (SPs UNKNOWN) -> `PaymentStatus NULL/Partial/Full` (BR-M12-038..041,035).

## M13 — Sales Reports, Analytics & Dashboards

Report pages are read-only. Statuses they read: `tblInvoice.DeliveryInvoiceStatus` (NULL = undelivered, Partial, Full), `tblInvoiceDetail.DeliveryStatus` (Full, Partial, Reject), `tblInvoice.PaymentStatus/PaymentAmount`, `tblOrder.IsInvoice`, `tblRequisition.CreatePicking/Submit/ReceiveIssue`. Statuses **modified** only by the two rollback pages: order invoiced -> not invoiced; delivery Full/Partial -> NULL; payment settled -> NULL; requisition challan-issued -> picking -> STO -> deleted (BR-M13-002..014). Forward transitions are set by other modules.

## M14 — Target & DWSP Management

No status/approval flags are set by any M14 page (CONFIRMED). Data states are only "row exists for (level, year, month, entity)". Inline **UpdateData** on the four `*View` pages re-saves an existing row (id > 0 -> update path in the same SP). The DWSP monthly report has an *Approval Status* filter (values from `SeedDataDAL.GetApprovalStatusList`) that is produced elsewhere (UNKNOWN who sets it; no M14 code writes it).

## M15 — HR, Field-Force & Employee Administration

- Employee/User/Hierarchy: Active/Inactive (BR-M15-009).
- Approval log: `Verified` -> `Accepted` (final) or `Rejected` (WF-M15-05, WF-M15-06). Approval-status master `sp_GET_ActionStatusList` (`SoftwareUseId`, `WebShow`) - values UNKNOWN.
- Tour plan: `ApprovalStatus` `2` approved, `3` rejected, other pending (BR-M15-070).
- Leave application: `Pending / Approved / Rejected` labels seen in JS (`LeaveApplicationEntry.aspx`), SP-driven (UNKNOWN).

## M16 — Doctor / Provider Master, Visits & Prescriptions

1. **Doctor master** (`tblDoctorMaster.ApprovalStatus`): 0 Pending, 1 Verified, 2 Approved, 3 Rejected (decode in `sp_Get_DoctorList`). Web save inserts '2'. Verify/reject by `Approval_UI/DoctorApprovalList` (other module). Separate `IsActive` flag with Activedate/InactiveDate.
2. **Call plan (DCP) -> DCR -> approval**: monthly plan headers (per employee, `ApprovalStatus`) created outside web; DCP approval page non-functional; DCR (`tbl_DCRInfo`) created by app; web list shows status text; Approved <-> Rejected toggle writes `tblDCRApprovalLog` (Type 'DCR', Status Accepted/Rejected, Step+1). Second flag `IsApproved` (0/1) in Daily Visit list. Reports filter by approval status. See WF-M16-03.
3. **Prescription (RX)**: capture (Prescription.aspx) -> ApprovalStatus (domain UNKNOWN; '2' = approved/locked in UI) -> approval page cannot submit (JS defect) -> RX reports. See WF-M16-04.

## M17 — SAP, Mobile-App & External Integration

- SAP challan: staged (`action`) -> received by `sp_SAP_StockReceive` -> hidden (INFERRED). ERP challan: `SAP_ChallanSend` 0/1, `SAP_Challan_ConfirmationSend` 0/1.
- Provider dropout: `IsApprove` 0 -> 1.
- API customer: `AddtoMainCustomer` 0 -> 1. MIGO: `StockUpload` 0 -> 1.
- Bank deposit: intended staged -> sent (sent step disabled).
- Customer app approval ActionStatus 0 Pending / 1 Verified / 2 Approved / 3 Rejected.

## M18 — Approval Workflow

- Log statuses written by pages: `Verified` (intermediate), `Accepted` (final; page-specific rule), `Rejected` — with `Step` incremented per action (WF-M18-01/02).
- Customer header `tblCustMaster.ActionStatus`: '0'/NULL Pending → '1' Verified → '2' Approved | '3' Rejected (list SP mapping, CONFIRMED). Doctor `ApprovalStatus` '2' = approved (repo SP inserts '2'). Others: UNKNOWN.
- Transfer request: pending → approved (SP-defined); no reject in live path.
- Numeric ↔ word mapping (`Accepted`→2?) is inside unavailable SPs (GAP-M18-026).

