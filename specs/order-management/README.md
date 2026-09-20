# M06 — Order Management (`specs/order-management`)

Reverse-engineered 2026-09-19 from `Solution.Web/SInventory_UI`, `Solution.Web/MasterSetup_UI`, `Solution.Web/SInventory_RPTVIEW`, `Library.BLL/DAL/DAO` and the SQL scripts in the repo root. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Evidence for every rule is in the Tier-A page specs; this file indexes them.

## 1. Purpose & business overview
The module covers the life of a customer sales order **before** delivery: order capture (manual web entry, Excel upload, mobile/app orders that arrive as `tblOrder` rows), optional exception approval, assignment to a distribution centre (DC) / route, credit-controlled conversion to a proforma invoice, and confirmation of delivery (Full/Partial/Reject) which the classifier also placed here (functionally M07). Monitoring/tracking pages, requisition pages (DC → warehouse stock requests) and a delete/audit report complete the module.

Core entity: `tblOrder` (+ `tblOrderDetail`). Key flags: `ActionStatus`, `IsInvoice`, `IsPrepareforInvoice`, `IsManual`, `IsFromApp`, `IsSubDepo`, `IsSpecialApproval`, `OrderType`, `OrderSenderType`, `ComUnitId`, `DistributionRouteId`.

## 2. Page catalogue
Tier A = read in depth (this pass). Tier B = generated spec only (not semantically reviewed — treat as leads).
| Page ID | Page (folder) | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-207 | OrderDCChange (MasterSetup_UI) | Bulk assign DC/route | **A** | [PAGE-207](pages/PAGE-207-orderdcchange.md) |
| PAGE-208 | OrderTrackingList (MasterSetup_UI) | List/export | B | [PAGE-208](pages/PAGE-208-ordertrackinglist.md) |
| PAGE-209 | OrderTrackingListDBH (MasterSetup_UI) | List/export | B | [PAGE-209](pages/PAGE-209-ordertrackinglistdbh.md) |
| PAGE-210 | OrderTrackingSummary (MasterSetup_UI) | Report/dashboard | B | [PAGE-210](pages/PAGE-210-ordertrackingsummary.md) |
| PAGE-211 | DeleteOrderReportViewer (SInventory_RPTVIEW) | Report viewer | B (see RPT-M06-02) | [PAGE-211](pages/PAGE-211-deleteorderreportviewer.md) |
| PAGE-212 | AutoInvoiceCreationByOrder | Entry — **broken/legacy** (grid load commented out; click calls `sp_DeliveryConformationFull`) | B | [PAGE-212](pages/PAGE-212-autoinvoicecreationbyorder.md) |
| PAGE-213 | DeleteOrderReport | Report filter | B (see RPT-M06-02) | [PAGE-213](pages/PAGE-213-deleteorderreport.md) |
| PAGE-214 | DelivaryInvoiceCreationForCustomerByOrderAuto | Delivery confirmation — legacy | **A** | [PAGE-214](pages/PAGE-214-delivaryinvoicecreationforcustomerbyorderauto.md) |
| PAGE-215 | DelivaryInvoiceCreationForCustomerByOrderAuto_Loading | Delivery confirmation | **A** | [PAGE-215](pages/PAGE-215-delivaryinvoicecreationforcustomerbyorderauto-loading.md) |
| PAGE-216 | DoctorInvoiceCreationByOrder | Entry (clone of 217) | B | [PAGE-216](pages/PAGE-216-doctorinvoicecreationbyorder.md) |
| PAGE-217 | InvoiceCreationByOrder | Order list + proforma entry | **A** | [PAGE-217](pages/PAGE-217-invoicecreationbyorder.md) |
| PAGE-218 | InvoiceCreationForCustomerByOrder | Proforma invoice form (core) | **A** | [PAGE-218](pages/PAGE-218-invoicecreationforcustomerbyorder.md) |
| PAGE-219 | IssueRequisitionProducts | Requisition issue | B | [PAGE-219](pages/PAGE-219-issuerequisitionproducts.md) |
| PAGE-220 | ManualOrderApproval | Approval (M18 engine) | **A** | [PAGE-220](pages/PAGE-220-manualorderapproval.md) |
| PAGE-221 | ManualOrderCreation | Order entry/edit | **A** | [PAGE-221](pages/PAGE-221-manualordercreation.md) |
| PAGE-222 | OrderDeleteorEdit | Delete order | **A** | [PAGE-222](pages/PAGE-222-orderdeleteoredit.md) |
| PAGE-223 | OrderEdit | Popup: change order DC | B (read: see WF-M06-06 §B) | [PAGE-223](pages/PAGE-223-orderedit.md) |
| PAGE-224 | OrderList | Upload master list / generate / delete | B (read: WF-M06-05) | [PAGE-224](pages/PAGE-224-orderlist.md) |
| PAGE-225 | OrderListUpload | Excel upload | B (read: WF-M06-05) | [PAGE-225](pages/PAGE-225-orderlistupload.md) |
| PAGE-226 | OrderListView | List/preview un-invoiced orders (class name `SInventory_UI_AreaView`) | B (read: WF-M06-01) | [PAGE-226](pages/PAGE-226-orderlistview.md) |
| PAGE-227 | OrderMonitor | Monitoring panel | B | [PAGE-227](pages/PAGE-227-ordermonitor.md) |
| PAGE-228 | OrderRequisitionCreation | DC→warehouse requisition entry | B | [PAGE-228](pages/PAGE-228-orderrequisitioncreation.md) |
| PAGE-229 | OrderRequisitionView | Requisition list | B | [PAGE-229](pages/PAGE-229-orderrequisitionview.md) |
| PAGE-230 | OrderStatus | Order detail (`?MID=`) | B | [PAGE-230](pages/PAGE-230-orderstatus.md) |
| PAGE-231 | PendingOrder | Re-import staged orders | B (read: WF-M06-05) | [PAGE-231](pages/PAGE-231-pendingorder.md) |
| PAGE-232 | SampleInvoiceCreationByOrder | Entry (clone of 217; uses `MasterPage.master`) | B | [PAGE-232](pages/PAGE-232-sampleinvoicecreationbyorder.md) |
| PAGE-233 | Testing_OrderStatus | Test copy of OrderStatus | B | [PAGE-233](pages/PAGE-233-testing-orderstatus.md) |
| PAGE-234 | ViewOrderDetailList | Staged order detail viewer | B | [PAGE-234](pages/PAGE-234-vieworderdetaillist.md) |
| PAGE-235 | ViewRequisitionForIssue | Requisition view | B | [PAGE-235](pages/PAGE-235-viewrequisitionforissue.md) |

## 3. Business rules index
Ids are sequential across the module; the rule text, source lines and confidence are in the page specs named.
| BR | One-liner | Where |
|---|---|---|
| BR-M06-001 | Invoiced order cannot be edited (page-level only) | PAGE-221 |
| BR-M06-002 | Sales person mandatory | PAGE-221 |
| BR-M06-003 | Customer mandatory/resolvable | PAGE-221 |
| BR-M06-004 | DC mandatory | PAGE-221 |
| BR-M06-005 | ≥1 row, qty>0; unresolved rows silently dropped | PAGE-221 |
| BR-M06-006 | Totals: Gross=Σ TP×qty, VAT=Σ vat×qty, no discount, Net=Gross+VAT | PAGE-221 |
| BR-M06-007 | Default sales person/DC derived from customer tags and user-DC map | PAGE-221 |
| BR-M06-008 | Order-number generation (page proposal vs SP `ODR-yyyymmdd<id>`) | PAGE-221 |
| BR-M06-009 | Manual orders born approved/invoice-ready (`ActionStatus='2'`…) | PAGE-221 |
| BR-M06-010 | Market hierarchy snapshotted onto order | PAGE-221 |
| BR-M06-011 | Edit deletes and re-inserts all lines | PAGE-221 |
| BR-M06-012 | TP editable, no limits | PAGE-221 |
| BR-M06-013 | Gift/campaign flags; inverted computed campaign flag on edit | PAGE-221 |
| BR-M06-014 | Approver options driven by user+page step config | PAGE-220 |
| BR-M06-015 | Queue = `IsSpecialApproval=1` orders at the user's step status | PAGE-220 |
| BR-M06-016 | Operation and ≥1 order required | PAGE-220 |
| BR-M06-017 | Approval = overwrite ActionStatus/ApprovedBy/ApprovedDate, no re-check | PAGE-220 |
| BR-M06-018 | Approver sees customer due (two formulas) | PAGE-220 |
| BR-M06-019 | Eligibility filter for invoicing list | PAGE-217 |
| BR-M06-020 | Credit permission formula (limit + day-limit) | PAGE-217 |
| BR-M06-021 | Credit block is UI-only | PAGE-217/218 |
| BR-M06-022 | Defaults / session memory of filters | PAGE-217 |
| BR-M06-023 | Session handoff of order id | PAGE-217 |
| BR-M06-024 | Bulk generation needs selection + DA; UI column hidden | PAGE-217 |
| BR-M06-025 | Same-day duplicate check unreachable | PAGE-217 |
| BR-M06-026 | Bulk errors swallowed | PAGE-217 |
| BR-M06-027 | Auto-delete of proforma on header/detail mismatch >10 | PAGE-217 |
| BR-M06-028 | One proforma per order | PAGE-218 |
| BR-M06-029 | Order lines re-verified before save | PAGE-218 |
| BR-M06-030 | Required: order no/date, pay type, DA | PAGE-218 |
| BR-M06-031 | No blank line | PAGE-218 |
| BR-M06-032 | Receivable ≥ 0 | PAGE-218 |
| BR-M06-033 | Stock cap / zero-stock lines Undelivered | PAGE-218 |
| BR-M06-034 | Line net formula; order price overrides list price | PAGE-218 |
| BR-M06-035 | Header totals + recompute from details | PAGE-218 |
| BR-M06-036 | Customer credit spread equally, consumed via tblReturnAmount | PAGE-218 |
| BR-M06-037 | Discount % source (trade policy, INSTITUTION/RCP, FCB FY-23-24) | PAGE-218 |
| BR-M06-038 | Hard-coded campaign names | PAGE-218 |
| BR-M06-039 | Gift lines zero-priced | PAGE-218 |
| BR-M06-040 | Batch-wise stock issue (negative Issue rows) | PAGE-218 |
| BR-M06-041 | Invoice number `INV-<DC><10-digit MAX(InvoiceId)+1>` | PAGE-218 |
| BR-M06-042 | Batch tagging tblInvoiceBatch | PAGE-218 |
| BR-M06-043 | Header recomputed from details | PAGE-218 |
| BR-M06-044 | Delivery charge in grand total/receivable | PAGE-218 |
| BR-M06-045 | FixedCustomer copied | PAGE-218 |
| BR-M06-046 | Compensation `sp_Deletenvoice` on failure | PAGE-218 |
| BR-M06-047 | Redirect if grid ≠ order lines | PAGE-218 |
| BR-M06-048 | Print may delete anomalous invoice | PAGE-218 |
| BR-M06-049 | Line delivery status Full/Partial/Reject | PAGE-215 |
| BR-M06-050 | Invoice-level status roll-up | PAGE-215 |
| BR-M06-051 | One confirmation per invoice; DEL-/RTN- numbering | PAGE-215 |
| BR-M06-052 | Reason required for Reject (typo skips Partial) | PAGE-215 |
| BR-M06-053 | Undelivered qty returned to stock as Receive row | PAGE-215 |
| BR-M06-054 | Delivery figures + journal posting | PAGE-215 |
| BR-M06-055 | Payment* columns mirrored at confirmation | PAGE-215 |
| BR-M06-056 | Hard-coded product offers, `ProductCode2` defect | PAGE-215 |
| BR-M06-057 | Same status logic (legacy variant) | PAGE-214 |
| BR-M06-058 | Forced auto full-cash payment | PAGE-214 |
| BR-M06-059 | Double `DEL-DEL-` prefix | PAGE-214 |
| BR-M06-060 | No guard / no posting in legacy variant | PAGE-214 |
| BR-M06-061 | Short-delivered qty returns to stock | PAGE-214 |
| BR-M06-062 | "Parial" typo | PAGE-214 |
| BR-M06-063 | Delete offers only un-invoiced orders | PAGE-222 |
| BR-M06-064 | SP refusal reported as "Proforma Already Generated" | PAGE-222 |
| BR-M06-065 | UserId passed as `@LoginName` | PAGE-222 |
| BR-M06-066 | DC-change page lists only DC-less orders; filters inert | PAGE-207 |
| BR-M06-067 | Assignment needs selection + DC + route | PAGE-207 |
| BR-M06-068 | Routes limited to selected DC | PAGE-207 |
| BR-M06-069 | Upload needs document date, manufacturer and rows; columns mapped by fixed position; only first sheet read | PAGE-225 (`OrderListUpload.aspx.cs:41-107,153-210`) — CONFIRMED, Tier-B page |
| BR-M06-070 | Uploaded master starts `GenerateOrder=false`, `EntryBy=LoginName` | PAGE-225 (`:114-122`) — CONFIRMED |
| BR-M06-071 | Master can be deleted only while `GenerateOrder=false` | PAGE-224 (`OrderList.aspx.cs:46-62`) — CONFIRMED |
| BR-M06-072 | Pending list = staged orders of a DC/date range not present in `tblOrder` for that range; DC + dates required | PAGE-231 (`PendingOrderDal.cs:15-29`; `PendingOrder.aspx.cs:61-71`) — CONFIRMED |
| BR-M06-073 | Pending re-import runs single-order SP only if code absent from `tblOrder`; message always "Order Processed" | PAGE-231 (`PendingOrderBll.cs:20-53`) — CONFIRMED |
| BR-M06-074 | OrderEdit changes only DC id/code/name of an order by id (no invoice-state check, no route change) | PAGE-223 (`OrderEdit.aspx.cs:49-71`; `OrderListDAL.cs:270-280`) — CONFIRMED, Tier-B page |
| BR-M06-075 | OrderListView lists all orders with `IsInvoice='0'` and previews them with totals (Σ qty, TP, VAT, Net) | PAGE-226 (`OrderInfoDAL.cs:592-601`; `OrderListView.aspx.cs:87-107`) — CONFIRMED, Tier-B page |

## 4. Status lifecycle (order)
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

## 5. Roles & permissions observed
No page in this module checks `Session["RoleTypeName"]`/`RoleTypeId` for authorisation. Enforcement = menu placement (DB-driven) + DC scoping via `tblUserCompanyUnit` on some lists (PAGE-217/231) + `tblActionUserWiseApproval` for approval options (PAGE-220). `Session["UserId"]` / `["LoginName"]` are recorded as creator/deleter/approver. Credit limit is not a permission but an advisory UI block.

## 6. Data model (tables and SPs that matter)
Tables: `tblOrder`, `tblOrderDetail`, `tblOrderDel` (archive; INFERRED writer `sp_DeleteOrder`), `tblOrderListMaster`, `tblOrderListDetail` (Excel/API staging), `tblInvoice`, `tblInvoiceDetail`, `tblInvoiceBatch`, `tblInvoiceDeleteLog`, `tblInvoiceDetail_DeleterRecord`, `tblProInvoiceReturnTrack`, `tblReturnAmount`, `tblDCStore`, `tblCustomerCreditLimit`, `tblTradePolicyNew/New2`, `tblProductDiscount`, `tblCustMaster`, hierarchy tables (`tblMarket…tbl_Group`), `tblAction*` (M18), cross-DB `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster/Detail`.
SPs with bodies in repo: `sp_Save_ManualOrderMaster`, `sp_Update_ManualOrderMaster`, `sp_Save_ManualOrderDetail`, `sp_LoadOrderListForOrderCreationbyTerri` (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`), `sp_Check_anomalyInvoiceDetails` (`SP_Fix_Invoice_Anomaly_And_Sync.sql`), `sp_Delete_ProformaInvoice` (`sp_Delete_ProformaInvoice_Update.sql`). Bodies missing: see GAP-M06-015.

## 7. Integrations
Mobile/field app orders (`IsFromApp`, `OrderSenderType`) and API staging (`@IsApiData=1` in pending re-import; "Bizmotion" monitor SP) — details UNKNOWN. Accounts DB (`CSTL_ACCDB_Unicorn`) read for dues and written (INFERRED) by `sp_Sales_DeliverySalesPosting`. No SAP call in verified Tier-A pages.

## 8. Reports
See [reports/](reports/): RPT-M06-01 proforma print, RPT-M06-02 delete-order report, RPT-M06-03 tracking/monitor grids (partial).

## 9. Workflows
[WF-M06-01 manual order entry/edit](workflows/WF-M06-01-manual-order-entry-and-edit.md) · [WF-M06-02 order→proforma invoice](workflows/WF-M06-02-order-to-proforma-invoice.md) · [WF-M06-03 delivery confirmation](workflows/WF-M06-03-delivery-confirmation.md) · [WF-M06-04 special-order approval](workflows/WF-M06-04-special-order-approval.md) · [WF-M06-05 Excel upload & pending import](workflows/WF-M06-05-excel-order-upload-and-pending-order-import.md) · [WF-M06-06 DC assignment/edit/delete](workflows/WF-M06-06-order-dc-assignment-edit-and-delete.md). Related: M18 approval engine `specs/approval-workflow/`, M07 invoice/delivery.

## 10. Data-fix script `Create_Order_ODR-20260901134.sql` — what it reveals
Transactional script (`BEGIN TRANSACTION … COMMIT`) for one order: (1) order codes are `ODR-<yyyyMMdd><OrderId>` (here `ODR-20260901134`, dated 2026-09-01, i.e. numbering by `sp_Save_ManualOrderMaster`); (2) an existing order is "re-opened" for invoicing by `IsInvoice=0, IsPrepareforInvoice=1, ActionStatus='2'`, hard-coded `GrossValue/TotalNetPayable=322550.00`, and `tblOrderDetail.Status=NULL`; (3) a missing order is inserted with `OrderSenderType='Web Manual'`, `IsManual=1`, DC 12/`BD02` "Dhaka Central Warehouse", MIO `EMP000055`/"Office", customer `C0000134` "Akij Pharmacy - Banani" (CustomerMasterId 134), remark "Transfer from Akij outlate Banani", 14 lines totalling 322 550.00, VAT 0; line VAT/discount 0; (4) it omits the `TerritoryId/MarketId/DistributionRouteId/…` hierarchy columns (left NULL; SubmissionDate is set to midnight) so the order will match `sp_LoadOrderListForOrderCreationbyTerri` only via the "NULL route/territory passes" clauses); (5) production data are corrected by hand-run scripts with hard-coded values (audit risk). Comments in Bengali. The script embeds no credentials.

## 11. Known gaps (summary) — see [GAPS-M06.md](GAPS-M06.md)
No transactions across multi-step writes; credit block UI-only; unreachable bulk-invoice path and dead duplicate-invoice check; hard-coded campaign names / product codes / cut-off date; missing SP bodies (proforma creation, delete order, DC update, tracking, posting); double `DEL-` prefix; "Parial" typo; MAX+1 numbering; string-concatenated SQL fragments (`@Parm`, report Session WHERE, `ClsApprovalAction`); many cloned pages.

## 12. Still unverified
Tier-B pages (208–212, 216, 219, 223 beyond DC change, 226–235) were not read line-by-line except where noted; `.rpt` internals; SP bodies listed in GAP-M06-015; contents of `tblAction*`/`ActionStatus` vocabulary; how app orders enter `tblOrder`; the ~2000 lines of row-recalculation handlers in PAGE-218/214/215; `BatchWiseProductQty` ordering; menu/role assignments.
