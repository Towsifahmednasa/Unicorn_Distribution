# M08 - Sales Return & Invoice Return

> Reverse-engineered specification (documentation only). Reviewed 2026-09-19. Evidence tags: CONFIRMED / INFERRED / UNKNOWN. Tier A pages: PAGE-287, 288, 290, 294, 296, 298. All other pages are Tier B (mechanically generated).

## 1. Purpose & business overview
M08 covers every way goods and money come back from customers in the distribution ERP:
1. **Market sales return** ("Return From Market", PAGE-294): return without an original invoice; stock is received into the depot ledger and two vouchers are auto-posted.
2. **Delivery invoice return** (PAGE-287): return against a delivered invoice; stock re-enters at the original batch; vouchers posted; an approval step exists in the data model but is bypassed (status 'Posted').
3. **Second-time (post-payment) partial return** (PAGE-296 -> PAGE-298): after payment confirmation, the accepted quantity per line is corrected; stock is restored in place; **no posting**.
4. **Invoice roll-back** (PAGE-288): undelivered invoices are deleted and the stock reservation is released.
5. **Legacy return invoice with freeze stock** (PAGE-290).
6. Report/list pages for all of the above (sales return, delivery return, rejection, replace note).
The **deduction charge** (return-delivery charge recovered from the customer) exists in both the delivery and market flows and is booked as income (GL flag 1032).

## 2. Page catalogue
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-279 | AllSalesReturnreportViewer - Crystal viewer for sales-return report (RPT-M08-01) | Report viewer | B | [spec](pages/PAGE-279-allsalesreturnreportviewer.md) |
| PAGE-280 | DeliveryReturnReportViewer - Crystal viewer for delivery-return shortfall report (RPT-M08-02) | Report viewer | B | [spec](pages/PAGE-280-deliveryreturnreportviewer.md) |
| PAGE-281 | SalesRejectionReportViewer - Viewer for sales-rejection report (RPT-M08-05) | Report viewer | B | [spec](pages/PAGE-281-salesrejectionreportviewer.md) |
| PAGE-282 | SalesReturnViewer - Viewer that reuses the sales-return .rpt with the sales query (RPT-M08-01, see GAP-M08-013) | Report viewer | B | [spec](pages/PAGE-282-salesreturnviewer.md) |
| PAGE-283 | AllSalesReturnReport - DC/date filter that opens the sales-return viewer (RPT-M08-01) | Report launcher | B | [spec](pages/PAGE-283-allsalesreturnreport.md) |
| PAGE-284 | BacktoReturnPage - Order-level rollback helper (uses sp_DeleteOrder, sp_GetOrderInvoiceIsZero, sp_Get_OrderNoforReturn*); logic not reviewed (GAP-M08-025) | Entry / order rollback | B | [spec](pages/PAGE-284-backtoreturnpage.md) |
| PAGE-285 | DeliveryReturnReport - DC/date filter for the delivery-return report (RPT-M08-02) | Report launcher | B | [spec](pages/PAGE-285-deliveryreturnreport.md) |
| PAGE-286 | DeliveryReturnReport_new - On-screen delivery-return list via sp_Get_DeliveryReturnReport (RPT-M08-02) | Report grid | B | [spec](pages/PAGE-286-deliveryreturnreport-new.md) |
| PAGE-287 | DeliverySalesReturn - Return against a delivery invoice: stock receipt + accounting posting (WF-M08-02) | Entry / transaction | A | [spec](pages/PAGE-287-deliverysalesreturn.md) |
| PAGE-288 | ProformaorInvoiceReturn - Delete an undelivered invoice: releases stock, reopens order (WF-M08-04) | Entry / destructive | A | [spec](pages/PAGE-288-proformaorinvoicereturn.md) |
| PAGE-289 | ReplaceNoteReport - Replace-note report via sp_Get_ReplaceNoteReport (RPT-M08-05) | Report grid | B | [spec](pages/PAGE-289-replacenotereport.md) |
| PAGE-290 | ReturnInvoiceCreationForCustomer - Legacy return invoice from a delivery invoice with freeze stock (WF-M08-04 part B) | Entry / legacy | A | [spec](pages/PAGE-290-returninvoicecreationforcustomer.md) |
| PAGE-291 | ReturnInvoiceView - List/delete/print return invoices for a DC and date range (tblReturnInvoice) | List | B | [spec](pages/PAGE-291-returninvoiceview.md) |
| PAGE-292 | SalesRejecionReport - Sales-rejection report launcher (RPT-M08-05) | Report launcher | B | [spec](pages/PAGE-292-salesrejecionreport.md) |
| PAGE-293 | SalesRejectionReport - Sales-rejection grid via sp_Get_SalesRejectionReportListLAtest (RPT-M08-05) | Report grid | B | [spec](pages/PAGE-293-salesrejectionreport.md) |
| PAGE-294 | SalesReturn - Market sales return, stock receipt and auto-posting (WF-M08-01) | Entry / transaction | A | [spec](pages/PAGE-294-salesreturn.md) |
| PAGE-295 | SalesReturnList - List of market returns (IsSalesReturnWithoutOrder = 'No order'); delete/print; DC locked for role DIC | List | B | [spec](pages/PAGE-295-salesreturnlist.md) |
| PAGE-296 | SalesReturnList_new - Worklist for second-time returns; leads to PAGE-298 (WF-M08-03) | List / worklist | A | [spec](pages/PAGE-296-salesreturnlist-new.md) |
| PAGE-297 | SalesReturnReport - Sales-return grid via sp_Get_SalesReturnReport with total (RPT-M08-01) | Report grid | B | [spec](pages/PAGE-297-salesreturnreport.md) |
| PAGE-298 | SndTimesSalesReturnPartial - Second-time partial return: header/detail update + stock (WF-M08-03) | Entry / transaction | A | [spec](pages/PAGE-298-sndtimessalesreturnpartial.md) |

## 3. Business rules index
| BR | Rule (abridged) | Page |
|---|---|---|
| BR-M08-001 | A market return is created without any link to an original invoice/order: InvoiceId=0, OrderId=0; OrderNo forced to "No order"; SP writes IsSalesReturnWithoutOrder='No order', SubInvoiceId=0. The list page filters on IsSalesReturn... | PAGE-294 |
| BR-M08-002 | DC is mandatory; DC choice order: dropdown → Session DICCompanyUnitId → hdComUnitId. Stock and price are looked up for the chosen DC only. | PAGE-294 |
| BR-M08-003 | The DC list is the user's DCs (tblUserCompanyUnit); if the user has ≤1 mapped DC (Items.Count ≤ 1 incl. placeholder) the list falls back to all company units. | PAGE-294 |
| BR-M08-004 | Customer, sales-person (MIO) name, return date and remarks are mandatory. | PAGE-294 |
| BR-M08-005 | Every return line requires an expiry date and batch no; manufacturing date is optional; there is no relation to an existing batch. | PAGE-294 |
| BR-M08-006 | Only products with tblProduct.GroupId = 1 are offered for return; selected quantity must be a positive whole number at selection time. | PAGE-294 |
| BR-M08-007 | Line calculation: TP = unitPrice×qty; VAT = unitVAT×qty; discount % = trade-policy % (non-fixed customers; 0 if FixedCustomer or lookup fails) replacing the product-discount % computed in RecalculateRow; discount amt = TP×%/100; N... | PAGE-294 |
| BR-M08-008 | Net Return Amount = max(Return Total − Deduction Charge, 0); if the posted net is ≤0 while grand>0 it is recomputed the same way. Deduction has no upper validation. | PAGE-294 |
| BR-M08-009 | Unit price and cost price shown in the grid are user-editable; the entered cost price (cogsTextBox) becomes CostPrice on the detail, CostPerUnit in tblDCStore, and the base of the COGS reversal voucher. | PAGE-294 |
| BR-M08-010 | Return invoice number = 'RIN--'+yymmdd+(MAX(ReturnInvoiceId of tblReturnInvoice rows dated same day, default 10000)+1001) — uses the global identity value, not a per-day counter. | PAGE-294 |
| BR-M08-011 | Stock effect: for each line with returned qty>0 and a DC, a new tblDCStore 'Receive' row is inserted (location "Depot Return Store", StockCondition "Non-Transcational Sales Return", TotalQuantity=StockQty=qty, ChalanNo=return invo... | PAGE-294 |
| BR-M08-012 | Posting is automatic after save (sp_SalesReturnPosting): idempotent per return (skips if a voucher with TableID=2018 (AR) / 2023 (COGS, IsInventoryValuation=1) and CommonPK_ID=ReturnInvoiceId exists). | PAGE-294 |
| BR-M08-013 | AR-reversal voucher (Journal, category 2, status 'Approved', by 'Auto Process', posting date = return date): Dr Sales Return GL (PageId 23/FlagId 1020, fallback 4170) = ReturnNetAmount; Cr Delivery/Deduction Income (23/1032, fallb... | PAGE-294 |
| BR-M08-014 | COGS/inventory voucher: Dr Finished-Goods Inventory (23/1018, fb 4156) / Cr COGS (23/1019, fb 4162) for Σ (TotalQuantity or Quantity) × (CostPrice, else latest tblDCStore.CostPerUnit>0 of the product in any DC); IsInventoryValuati... | PAGE-294 |
| BR-M08-015 | Customer subsidiary (type 1) and reference info (type 1018) rows are created on the fly in the accounts DB if missing. Financial year = active year of company 1 (else the year containing posting date, else hard-coded id 2). | PAGE-294 |
| BR-M08-016 | Voucher code = 'JVP-'+yymmdd+(max of last 5 digits of same-day JVP codes, default 10000)+1; IsPosting=1 set on tblReturnInvoice at the end. | PAGE-294 |
| BR-M08-017 | No VAT-liability reversal is posted: VAT is inside the Sales Return debit (gross) and the receivable credit; deduction is booked as income without VAT. | PAGE-294 |
| BR-M08-018 | tblReturnAmount.Amount = Return Total (gross, before deduction). | PAGE-294 |
| BR-M08-019 | The flow is only for invoices already payment-confirmed (PaymentInvoiceNo not null) and not yet second-returned (SndReturnInvoiceNo null) — enforced by the list query, plus a server duplicate guard on SndReturnInvoiceNo. | PAGE-298 |
| BR-M08-020 | The user must press Calculate after any DQty change and before saving. | PAGE-298 |
| BR-M08-021 | DQty (accepted quantity) defaults to the previously paid quantity and cannot exceed it (values ≥ previous are reset to previous). | PAGE-298 |
| BR-M08-022 | Line status: DQty = TQty ⇒ Full; 0 < DQty < TQty ⇒ Partial; DQty = 0 (or DQty > TQty, reset to 0) ⇒ Reject. Reason dropdown enabled for Partial/Reject. | PAGE-298 |
| BR-M08-023 | A reason is mandatory only for lines whose status literal equals "Reject" or the misspelt "Parial" — Partial lines effectively skip the check. | PAGE-298 |
| BR-M08-024 | Header status: every line Full ⇒ Full; every line Reject ⇒ Reject; otherwise Partial. Stored in tblInvoice.sndReturnInvoiceStatus; SndReturnInvoiceNo='SRN-'+InvoiceNo. | PAGE-298 |
| BR-M08-025 | Stock effect: for Partial/Reject lines add (PaymentQuantity − DQty) back to the original tblDCStore batch row (StockQty = StockQty + qty); Full lines: none. Bonus quantity is forced to 0. | PAGE-298 |
| BR-M08-026 | Line snapshot stored in tblInvoiceDetailReturn with PreviousQuantity = payment qty and sndReturnQuantity/TotalQuantity = DQty (i.e. the *retained* quantity, not the returned quantity). | PAGE-298 |
| BR-M08-027 | Line amounts recomputed on DQty: TP = unit price × DQty; VAT = unit VAT × DQty; Net = TP − discount + VAT; lines with zero TP get VAT/discount/net = 0; campaign/trade-term discount % recomputed from totals (old-trade-policy vs new... | PAGE-298 |
| BR-M08-028 | This page posts no accounting entry; whether posting occurs downstream from tblInvoice.sndReturn* is UNKNOWN. | PAGE-298 |
| BR-M08-029 | Print number shown is "RTN-"+InvoiceNo while the stored return number is "SRN-"+InvoiceNo. | PAGE-298 |
| BR-M08-030 | Product duplicates within the grid are rejected by ProductCodeValidation (returns false → "No: Product Already Inserted!!!"), only for manual product edits. | PAGE-298 |
| BR-M08-031 | Only delivery invoices of non-direct orders (tblOrder.IsDirect = 0/NULL) are searchable, and an invoice that already has a tblCustomerReturnMaster row can never be returned again (whole-invoice, one-time). | PAGE-287 |
| BR-M08-032 | Available quantity = DeliveryTotalQuantity − Σ prior ReturnQty per invoice detail; return qty is an integer 0…available; total must be >0. | PAGE-287 |
| BR-M08-033 | Line return amount = Return Qty × (UnitPrice + UnitVatAmount) where UnitPrice DataKey is the alias ROUND(DeliveryNetAmount/Quantity,4) (first of two UnitPrice columns) — VAT is added again on top of a delivery net amount. | PAGE-287 |
| BR-M08-034 | Deduction ≤ Total Return Amount (blocked at save, reset on edit); Net = max(Total − Deduction, 0). | PAGE-287 |
| BR-M08-035 | Return is saved with ActionStatus='Posted' — no approval step in this flow. | PAGE-287 |
| BR-M08-036 | Stock effect: returned qty is added as a new 'Receive' ledger row (StockCondition='SalesReturn', TotalQuantity=0, StockQty=qty, cost = original cost) referencing the original batch row. | PAGE-287 |
| BR-M08-037 | Voucher 1 (Journal, 'Approved', by 'admin'/'Auto Process', date=today, TableID 2018, CommonPK_ID=CustReturnId): Dr Sales Return (23/1020, fb 4170) = TotalAmount; Cr Delivery Charge Income (23/1032, fb 5968) = deduction if >0; Cr A... | PAGE-287 |
| BR-M08-038 | Voucher 2 (TableID 2023) posts only if StockValue>0: Dr Finished Goods (23/1016, fb 4156) / Cr COGS (23/1021, fb 4162) where StockValue = Σ (tblCentralStore.UnitPrice + VATPerUnit) × ReturnQty, traced invoice detail → tblDCStore.T... | PAGE-287 |
| BR-M08-039 | IsPosting=1 on tblCustomerReturnMaster after posting; sp_DeliverySalesReturnPosting has no idempotency check — re-executing creates duplicate vouchers. | PAGE-287 |
| BR-M08-040 | Only ticked lines are returned; a line's cumulative returned qty (PRQty = Σ earlier tblReturnInvoiceDetail.TotalQuantity for that original invoice detail + this RQty) may not exceed the line's total delivered qty (TQty). | PAGE-290 |
| BR-M08-041 | Return-line VAT is recomputed as TP × product VAT % / 100 and discount % is re-derived from the *order's* trade terms (GetTradeTerm), instead of using the invoiced unit VAT/discount. | PAGE-290 |
| BR-M08-042 | Amount defect: the saved detail TotalPrice/TotalPriceVatAmount are computed from qtyTextBox (delivered quantity) × unit price/VAT, while Quantity/TotalQuantity are the returned qty (dQty) — saved amounts do not match saved quantit... | PAGE-290 |
| BR-M08-043 | Returned stock is not re-admitted to tblDCStore; a tblDCStoreFreeze row (StockCondition='ReturnStock', StockQty=dQty) records it. Nothing is posted to accounts from this page. | PAGE-290 |
| BR-M08-044 | Return Invoice No = 'INV-' + <ComUnitCode> + '-' + zero-padded (COUNT of that unit's tblReturnInvoice rows + 1) (8 digits) — same INV- prefix as sales invoices; count-based so deletions create duplicates. | PAGE-290 |
| BR-M08-045 | ProformaorInvoiceReturn (PAGE-288) refuses to delete a delivery invoice when its lines already have freeze stock consumed (tblDCStoreFreeze.StockQty <> TotalQuantity) — the freeze rows created here are the trigger. | PAGE-290 |
| BR-M08-046 | The worklist lists invoices of the chosen DC + route + territory that are payment-confirmed (PaymentInvoiceNo not null) and not yet second-returned (SndReturnInvoiceNo null), sorted by invoice no; optional partial invoice-no searc... | PAGE-296 |
| BR-M08-047 | Only status "Partial Dues" exists in the row dropdown (disabled); it exposes *Go to Partial*; an invoice flagged IsAdjustInvoice=True forces index 0 and disables the dropdown. | PAGE-296 |
| BR-M08-048 | "ALL Submit" only acts on ticked rows whose status equals "Full", which cannot occur — the bulk full-confirmation path is dead. | PAGE-296 |
| BR-M08-049 | The DC list is restricted to the user's DCs (tblUserCompanyUnit); first DC is auto-selected; routes come from the "second-times sales return" route SP for that DC. | PAGE-296 |
| BR-M08-050 | Only invoices without a delivery invoice number can be deleted through sp_Delete_ProformaInvoice; other invoices are silently ignored by the SP (page still says "Delete successfully"). | PAGE-288 |
| BR-M08-051 | Stock reversal is by deleting the invoice's negative 'Issue' ledger row (matched by ReferenceDCStoreId, Trans_Type='Issue', StockQty = −TotalQuantity, one row per detail) — the Receive row's StockQty is *not* increased; over-infla... | PAGE-288 |
| BR-M08-052 | Audit trail: each detail is copied into tblInvoiceDetail_DeleterRecord, a tblProInvoiceReturnTrack row is written, and a tblInvoiceDeleteLog row records who/when. | PAGE-288 |
| BR-M08-053 | Deleting re-opens the source order (tblOrder.IsInvoice=0, tblOrderDetail.Status=NULL) so it can be re-invoiced; payments/returns tied to the invoice in tblReturnAmount are removed. | PAGE-288 |
| BR-M08-054 | "Delivery" deletion is refused if any of the invoice's freeze-stock rows has been partially consumed (StockQty<>TotalQuantity). | PAGE-288 |

## 4. Status lifecycles
| Object | States | Set by |
|---|---|---|
| tblReturnInvoice (market return) | created (IsPosting NULL/0) -> posted (IsPosting=1) | `sp_SalesReturnPosting` (PAGE-294) |
| tblCustomerReturnMaster (delivery return) | ActionStatus 'Posted' at creation (the design also has pending/approved: `ApprovalUpdateDAL`, unused); IsPosting 0/NULL -> 1 | PAGE-287 / `sp_DeliverySalesReturnPosting` |
| tblInvoice second-time return | `sndReturnInvoiceStatus` NULL -> Full / Partial / Reject; `SndReturnInvoiceNo` set (`SRN-`+InvoiceNo) | PAGE-298 |
| tblInvoiceDetailReturn.sndReturnStatus | Full / Partial / Reject | PAGE-298 |
| tblDCStoreFreeze.StockCondition | 'ReturnStock' (legacy return) | PAGE-290 |
| Invoice deletion | invoice row deleted; log rows written; `tblOrder.IsInvoice` 1 -> 0 | `sp_Delete_ProformaInvoice` (PAGE-288) |
| Worklist gate | shown while `PaymentInvoiceNo` not null and `SndReturnInvoiceNo` null | PAGE-296 |

## 5. Roles / permissions observed
Menu access is DB-driven (`sp_GET_MainMenuRole2`). In code: PAGE-294/296 limit the DC dropdown to `tblUserCompanyUnit`; `SalesReturnList` (PAGE-295) locks the DC to `Session["DICCompanyUnitId"]` when `Session["RoleTypeName"] == "DIC"`; PAGE-287/290 load every DC; no page checks a role before a destructive or posting action. `Session["LoginName"]` is the audit user; `Session["UserId"]` is dereferenced without null checks (GAP-M08-021).

## 6. Data model (tables and SPs that matter)
- Market return: `tblReturnInvoice`, `tblReturnInvoiceDetail`, `tblReturnAmount`, `tblDCStore`; SPs `spInsertReturnInvoice_new`, `sp_SalesReturnPosting`.
- Delivery return: `tblCustomerReturnMaster`, `tblCustomerReturnDetail`, `tblDCStore`, `tblCentralStore` (valuation); SP `sp_DeliverySalesReturnPosting`.
- Second-time return: `tblInvoice` (`sndReturn*`), `tblInvoiceDetailReturn`, `tblInvoiceDetail.PaymentQuantity`, `tblDCStore`; SPs `sp_UpdateAndInsertInvoiceDetailSalesReturn`, `sp_BeforesndSalesReturnList`, `sp_GET_DelivaryInvoiceNoCheckById`.
- Invoice roll-back: `sp_Delete_ProformaInvoice`, `tblInvoiceDetail_DeleterRecord`, `tblInvoiceDeleteLog`, `tblProInvoiceReturnTrack`.
- Accounts (other DBs): `CSTL_ACCDB_Unicorn.tblDebitCreditVoucherMaster/Detail/tblSubsidiaryInfo/tblReferenceInfo/tblFinancialYear`, `CSTL_ERPDB_Unicorn.Table_Integration_AccountsConnectivity` (PageId 23 flags 1016, 1018, 1019, 1020, 1021, 1032, 2; PageId 163 flag 2).
- **Stock ledger contract** (`tblDCStore`): `Trans_Type` 'Receive' rows carry stock; 'Issue' rows are negative and reference the Receive row (`ReferenceDCStoreId`); available = sum of `StockQty` over Receive+Issue. Return flows add Receive rows (PAGE-294, 287) or update in place (PAGE-298). See GAP-M08-006.

## 7. Integrations
Accounts DB voucher posting (cross-database, auto-approved journals). No SAP calls in M08 pages. Central-store price used for valuation in the delivery return.

## 8. Reports
See `reports/README.md`: RPT-M08-01 to RPT-M08-05.

## 9. Workflows
- [WF-M08-01 Market sales return](workflows/WF-M08-01-market-sales-return-entry-and-posting.md)
- [WF-M08-02 Delivery invoice return](workflows/WF-M08-02-delivery-invoice-return.md)
- [WF-M08-03 Second-time partial return](workflows/WF-M08-03-second-time-partial-return.md)
- [WF-M08-04 Invoice roll-back and legacy return](workflows/WF-M08-04-invoice-rollback-delete.md)

## 10. Known gaps summary
See [GAPS-M08.md](GAPS-M08.md) (26 items). Headline items: no transactions (001); SQL injection (002); conflicting `spInsertReturnInvoice_new` scripts (003); voucher TableID collision between market and delivery returns (005); stock-ledger inconsistency versus the adjust/anomaly scripts (006); unbalanced voucher when deduction exceeds total (007); deleting a market return leaves stock and vouchers (008); print/report mismatches (012, 013).

## 11. What is still unverified
- Bodies of the SPs listed in GAP-M08-010 and DDL of the return tables; which SP variant is live (GAP-M08-003, 004).
- Tier-B pages (284, 286, 289, 291, 292, 293, 295, 297, viewers 279-282): only structure and query shape reviewed.
- Whether downstream jobs react to `tblInvoice.sndReturn*` (accounting/AR).
- Whether `DeliveryNetAmount` includes VAT (double-VAT risk, GAP-M08-014).
- Menu placement and role mapping (DB-driven).
