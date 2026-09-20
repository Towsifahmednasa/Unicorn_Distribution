---
module: M12
name: Sub-Depot Operations
reviewed: 2026-09-19
---
# M12 - Sub-Depot Operations

## 1. Purpose and business overview
A **sub-depot** is a satellite stock point under one parent DC (`tblSubDepot.ComUnitId`, BR-M12-001). M12 is a **parallel stack** to the main DC stack: the DC sends stock to a sub-depot (chalan), the sub-depot receives it into its own store (`tblSubDepotStore`), invoices customers of sub-depot routes (`tblSubInvoiceMaster/Detail`), collects payments, quarantines/releases damaged and returned stock (`tblSubDepotStoreFreeze`), adjusts stock (direct or by approved voucher) and can return stock to the DC. Orders/routes are flagged `IsSubDepo` so main-depot lists exclude them (BR-M12-038).
Code: `Solution.Web/SubDepot_UI` (19 pages), sub-depot pages in `SInventory_UI` (7) and viewers in `SInventory_RPTVIEW` (11); `Library.BLL/DAL/DAO` `SubDepot_*` (plus shared `OrderInfoDAL`, `CustPaymentDAL`, `ChalanDAL`, `OtherStockActionDAL`, `DCStockReportDAL`). All `SubDepot_DAL` SQL is string-concatenated and executed through the wrapper SP `ExecuteAllSqlQueryByStoreProcedure` (`Library.DAL/InternalCls/ClsCommonInternalDAL.cs:397-421`).

## 2. Page catalogue (37 pages)
Tier A = reviewed against code (this run); Tier B = generated, unreviewed. Tier-A picks: candidates 448, 453, 450, 451, 452 plus 446, 441, 456 (write/transaction pages missed by the candidate list).

| Page ID | Page | Folder | Kind | Purpose | Tier | Spec |
|---|---|---|---|---|---|---|
| PAGE-426 | DcToSubdepotChalanReportViewer | SInventory_RPTVIEW | Report viewer | Chalan print viewer (DC->Sub-depot) | B | [PAGE-426-dctosubdepotchalanreportviewer.md](pages/PAGE-426-dctosubdepotchalanreportviewer.md) |
| PAGE-427 | SubdeportAreaWiseMonthlySalesReportViewer | SInventory_RPTVIEW | Report viewer | Area-wise monthly inventory viewer | B | [PAGE-427-subdeportareawisemonthlysalesreportviewer.md](pages/PAGE-427-subdeportareawisemonthlysalesreportviewer.md) |
| PAGE-428 | SubDeportDelivaryInvoiceReturnViewer | SInventory_RPTVIEW | Report viewer | Delivery invoice viewer | B | [PAGE-428-subdeportdelivaryinvoicereturnviewer.md](pages/PAGE-428-subdeportdelivaryinvoicereturnviewer.md) |
| PAGE-429 | SubDeportDelivaryTopSheetInvoiceReturnViewer | SInventory_RPTVIEW | Report viewer | Delivery top-sheet viewer | B | [PAGE-429-subdeportdelivarytopsheetinvoicereturnviewer.md](pages/PAGE-429-subdeportdelivarytopsheetinvoicereturnviewer.md) |
| PAGE-430 | SubDeportInvoicePrintReportViewer | SInventory_RPTVIEW | Report viewer | Proforma invoice PDF viewer (from list) | B | [PAGE-430-subdeportinvoiceprintreportviewer.md](pages/PAGE-430-subdeportinvoiceprintreportviewer.md) |
| PAGE-431 | SubDeportInvoiceReportViewer | SInventory_RPTVIEW | Report viewer | Proforma invoice PDF viewer (from creation) | B | [PAGE-431-subdeportinvoicereportviewer.md](pages/PAGE-431-subdeportinvoicereportviewer.md) |
| PAGE-432 | SubdeportMarketwisePicking | SInventory_RPTVIEW | Report viewer | Market-wise picking PDF | B | [PAGE-432-subdeportmarketwisepicking.md](pages/PAGE-432-subdeportmarketwisepicking.md) |
| PAGE-433 | SubDeportStockReportViewer | SInventory_RPTVIEW | Report viewer | Sub-depot stock report (Excel) | B | [PAGE-433-subdeportstockreportviewer.md](pages/PAGE-433-subdeportstockreportviewer.md) |
| PAGE-434 | SubdeportTopSheetReportViewer | SInventory_RPTVIEW | Report viewer | Proforma top-sheet viewer | B | [PAGE-434-subdeporttopsheetreportviewer.md](pages/PAGE-434-subdeporttopsheetreportviewer.md) |
| PAGE-435 | SubDeportTransferStockReceiveReportViewer | SInventory_RPTVIEW | Report viewer | Sub-depot receive report viewer | B | [PAGE-435-subdeporttransferstockreceivereportviewer.md](pages/PAGE-435-subdeporttransferstockreceivereportviewer.md) |
| PAGE-436 | SubDepotStockOutReportViewer | SInventory_RPTVIEW | Report viewer | Stock-out voucher viewer | B | [PAGE-436-subdepotstockoutreportviewer.md](pages/PAGE-436-subdepotstockoutreportviewer.md) |
| PAGE-437 | SalesReturnListSubdeport | SInventory_UI | List / View | Sub-depot sales-return list / delete | B | [PAGE-437-salesreturnlistsubdeport.md](pages/PAGE-437-salesreturnlistsubdeport.md) |
| PAGE-438 | SubdeportMonthlyInventoryReport | SInventory_UI | Report / Dashboard | Monthly inventory report launcher | B | [PAGE-438-subdeportmonthlyinventoryreport.md](pages/PAGE-438-subdeportmonthlyinventoryreport.md) |
| PAGE-439 | SubdeportPaymentReverse | SInventory_UI | Entry / Transaction | Sub-depot payment reverse | B | [PAGE-439-subdeportpaymentreverse.md](pages/PAGE-439-subdeportpaymentreverse.md) |
| PAGE-440 | SubDeportProformaorInvoiceReturn | SInventory_UI | Page | Delete sub-depot proforma / delivery invoice | B | [PAGE-440-subdeportproformaorinvoicereturn.md](pages/PAGE-440-subdeportproformaorinvoicereturn.md) |
| PAGE-441 | SubdeportTransferReceiveProductByChalanByDC | SInventory_UI | Entry / Transaction | DC receives Sub-depot return chalan | A | [PAGE-441-subdeporttransferreceiveproductbychalanbydc.md](pages/PAGE-441-subdeporttransferreceiveproductbychalanbydc.md) |
| PAGE-442 | SubDeportTransferStockReceiveByDC | SInventory_UI | Entry / Transaction | DC list of pending Sub-depot returns | B | [PAGE-442-subdeporttransferstockreceivebydc.md](pages/PAGE-442-subdeporttransferstockreceivebydc.md) |
| PAGE-443 | SubDepotStockOutApproval | SInventory_UI | Approval | Voucher approval (SInventory_UI copy) | B | [PAGE-443-subdepotstockoutapproval.md](pages/PAGE-443-subdepotstockoutapproval.md) |
| PAGE-444 | DelivaryInvoiceList | SubDepot_UI | List / View | Delivery confirmation (Full/Reject/Partial) | B | [PAGE-444-delivaryinvoicelist.md](pages/PAGE-444-delivaryinvoicelist.md) |
| PAGE-445 | DelivaryTopSheetGenerate | SubDepot_UI | Entry / Transaction | Delivery invoice/top-sheet print list | B | [PAGE-445-delivarytopsheetgenerate.md](pages/PAGE-445-delivarytopsheetgenerate.md) |
| PAGE-446 | ProductReceiveBySubDeport | SubDepot_UI | Entry / Transaction | Sub-depot receives DC chalan | A | [PAGE-446-productreceivebysubdeport.md](pages/PAGE-446-productreceivebysubdeport.md) |
| PAGE-447 | SCPickingGenerate | SubDepot_UI | Entry / Transaction | Market-wise picking launcher | B | [PAGE-447-scpickinggenerate.md](pages/PAGE-447-scpickinggenerate.md) |
| PAGE-448 | StockOutSubDepot | SubDepot_UI | Page | Direct stock adjustment (write-off) | A | [PAGE-448-stockoutsubdepot.md](pages/PAGE-448-stockoutsubdepot.md) |
| PAGE-449 | StockReceiveBySubDepot | SubDepot_UI | Entry / Transaction | Sub-depot pending chalan list | B | [PAGE-449-stockreceivebysubdepot.md](pages/PAGE-449-stockreceivebysubdepot.md) |
| PAGE-450 | StockTransferDcToSubDepot | SubDepot_UI | Entry / Transaction | DC -> Sub-depot stock transfer (send) | A | [PAGE-450-stocktransferdctosubdepot.md](pages/PAGE-450-stocktransferdctosubdepot.md) |
| PAGE-451 | StockTransferSubDepottoDc | SubDepot_UI | Entry / Transaction | Sub-depot -> DC return transfer (send) | A | [PAGE-451-stocktransfersubdepottodc.md](pages/PAGE-451-stocktransfersubdepottodc.md) |
| PAGE-452 | SubDeportCustomerPayment | SubDepot_UI | Entry / Transaction | Sub-depot customer payment | A | [PAGE-452-subdeportcustomerpayment.md](pages/PAGE-452-subdeportcustomerpayment.md) |
| PAGE-453 | SubDeportStockFreez | SubDepot_UI | Page | Freeze stock release | A | [PAGE-453-subdeportstockfreez.md](pages/PAGE-453-subdeportstockfreez.md) |
| PAGE-454 | SubDeportStockReport | SubDepot_UI | Report / Dashboard | Sub-depot stock report launcher | B | [PAGE-454-subdeportstockreport.md](pages/PAGE-454-subdeportstockreport.md) |
| PAGE-455 | SubdepotInfoEntry | SubDepot_UI | Entry / Transaction | Sub-depot master entry/edit | B | [PAGE-455-subdepotinfoentry.md](pages/PAGE-455-subdepotinfoentry.md) |
| PAGE-456 | SubDepotInvoiceCreation | SubDepot_UI | Entry / Transaction | Proforma invoice creation from orders | A | [PAGE-456-subdepotinvoicecreation.md](pages/PAGE-456-subdepotinvoicecreation.md) |
| PAGE-457 | SubDepotProformaList | SubDepot_UI | List / View | Proforma print/top-sheet list | B | [PAGE-457-subdepotproformalist.md](pages/PAGE-457-subdepotproformalist.md) |
| PAGE-458 | SubDepotStockAdjustmentVoucher | SubDepot_UI | Entry / Transaction | Stock adjustment voucher (Posted) | B | [PAGE-458-subdepotstockadjustmentvoucher.md](pages/PAGE-458-subdepotstockadjustmentvoucher.md) |
| PAGE-459 | SubDepotStockAdjustmentVoucherView | SubDepot_UI | List / View | Adjustment voucher list/delete/print | B | [PAGE-459-subdepotstockadjustmentvoucherview.md](pages/PAGE-459-subdepotstockadjustmentvoucherview.md) |
| PAGE-460 | SubDepotStockOutApproval | SubDepot_UI | Approval | Voucher approval (SubDepot_UI copy) | B | [PAGE-460-subdepotstockoutapproval.md](pages/PAGE-460-subdepotstockoutapproval.md) |
| PAGE-461 | SubdepotView | SubDepot_UI | List / View | Sub-depot list | B | [PAGE-461-subdepotview.md](pages/PAGE-461-subdepotview.md) |
| PAGE-462 | TopSheetGenerate | SubDepot_UI | Entry / Transaction | Proforma top-sheet/print list | B | [PAGE-462-topsheetgenerate.md](pages/PAGE-462-topsheetgenerate.md) |

## 3. Status lifecycles / transfer state machine
**DC -> Sub-depot chalan** (`tblSubDepotChalanInfo.IsDeliver`): `[none] --PAGE-450 submit--> IsDeliver=false (in transit; DC tblDCStore.StockQty already reduced)` `--PAGE-446 first line saved--> IsDeliver=true (sub-depot tblSubDepotStore rows Available; damage -> tblSubDepotStoreFreeze Restricted)`. No reject/cancel/partial state (BR-M12-010,013,014,016,017).
**Sub-depot -> DC return** (`tblSubDepotChalanReturnInfo.IsDeliver`): `PAGE-451 submit -> false (sub-depot StockQty already reduced)` `--PAGE-441 first line--> true (new tblDCStore row, ComUnitId hard-coded 6; damage -> tblDCStoreFreeze Restricted)` (BR-M12-011,018,019).
**Freeze row** (`tblSubDepotStoreFreeze.StockCondition`): created `Restricted` (receive damage) or `ReturnStock` (sales return, `ReturnInvoiceDAL.cs:1212`); qty released back to `Available` store row by PAGE-453 (Restricted only by login 21900); `Blocked` counted by report but never created by M12 code (BR-M12-028..031).
**Adjustment voucher** (`tblSubDepotStockOutMaster.Status`): `Posted -> Approved` (SP `sp_UD_SubDcStockOutApproval`, stock effect UNKNOWN); `Reject` wrongly updates the DC table (BR-M12-023..026). Direct adjustment (PAGE-448) has no status; stock reduced at once (BR-M12-021).
**Sub invoice**: order `IsInvoice 0` -> proforma (`DelivaryInvoiceNo` NULL) -> delivery `DeliveryInvoiceStatus Full/Partial/Reject` (SPs UNKNOWN) -> `PaymentStatus NULL/Partial/Full` (BR-M12-038..041,035).

## 4. How sub-depot stock is valued and frozen
- **Quantity ledger, no cost**: `tblSubDepotStore` rows carry StockQty/DamageQty/TotalQuantity, batch, dates and `StockCondition='Available'`; no CostPerUnit/Trans_Type/TotalCostAmount is ever written (BR-M12-014; `SubDepotChalanDAL.cs:42-90`). Stock is changed **in place** (`StockQty = current -/+ qty`) by transfers, adjustments and freeze release - not by ledger rows.
- **Valuation is at trade price only**: chalan Value = active `tblUnitPrice.UnitPrice` x qty + VAT (BR-M12-009); freeze list Amount = UnitPrice x StockQty (BR-M12-028). The stock report (RPT-M12-06) is quantity-only with five buckets Available / Booked-for-delivery / In-transit / Blocked / Restricted (BR-M12-043). No sub-depot equivalent of `sp_Report_DCStore_StockValuationLedger` exists.
- **Frozen** = quantity moved out of the Available store row into `tblSubDepotStoreFreeze` (damage at receive, customer returns); frozen qty is excluded from `Available` and released only via PAGE-453.
- **Cross-effects on DC valuation (INFERRED, GAP-M12-008/030)**: DC->Sub issue is an in-place `UPDATE tblDCStore` (no negative `Issue` row) and Sub->DC receipt inserts a row with NULL `Trans_Type`, which the DC valuation SP (`SP_Update_StockValuation_TransactionDate_Logic.sql:84-140`) reads as "Receive from WH".

## 5. Sub-depot vs main-depot differences (from code read)
| Area | Main depot | Sub-depot | Evidence |
|---|---|---|---|
| Stock table | `tblDCStore` with ledger rows (`Trans_Type` Receive/Issue, `CostPerUnit`, `TotalCostAmount`, `ReferenceDCStoreId`) | `tblSubDepotStore` plain qty rows, no cost | `ChalanDAL.cs:122-160,513-553`; `InvoiceDAL.cs:666-700`; `SubDepotChalanDAL.cs:42-90` |
| Stock out (transfer/sale) | INSERT negative `Issue` row w/ cost, parameterised | in-place `StockQty = cur - qty`, concatenated SQL, no txn, no negative guard | same; `SubDepotChalanBLL.cs:75-86` |
| Stock in (receive) | `DCStockInDALMain` INSERT..SELECT `B2BReceive`, cost from source/`tblUnitPrice` | plain INSERT `Available`; DC-side return receipt uses `DCStockInDALMainSub` (no cost, ComUnitId 6) | `ChalanDAL.cs:107-160`; `ProductReceiveBySubDeport.aspx.cs:46-73` |
| Transfer document | `tblChalanInfo/Detail` (+`FromComUnitId`, SAP `sp_SAP_B2B*`), `NewStockTransferDcToDc` has try/catch, TryParse | `tblSubDepotChalanInfo/Detail` and `...ReturnInfo/RetuenDetail`; no SAP, no try/catch, `Convert.To*` | `NewStockTransferDcToDc.aspx.cs:164-249`; `StockTransferDcToSubDepot.aspx.cs:116-203` |
| Transfer state | `IsDeliver` flag, filtered by receiving DC | same flag; Sub->DC pending list not filtered by DC | `ChalanDAL.cs:269-273` |
| Invoice creation | `sp_Process_ProformaInvoiceByOrderId` + DA name, route-id batch prefix, post-check `Check_anomalyInvoiceDetails` auto-delete | `sp_Process_SubDepoProformaInvoiceByOrderId`, batch `06-`, route list DC 6, empty catch | `InvoiceCreationByOrder.aspx.cs:200-260`; `SubDepotInvoiceCreation.aspx.cs:52-123` |
| Invoice tables | `tblInvoice/tblInvoiceDetail` | `tblSubInvoiceMaster/tblSubInvoiceDetail/tblSubInvoiceMasterBatch` (`SubDCStoreId`) | `Sub_InvoiceDAL.cs:111-262` |
| Delivery confirmation | `sp_DeliveryConformationFull/Reject` | `sp_SubdeportDeliveryConformationFull`, `sp_SubDeportDeliveryConformationReject`; Partial page excluded | `InvoiceDAL.cs:269-313`; `DelivaryInvoiceList.aspx.cs:87-124` |
| Payment | dedupe (`CheckInvoiceCustpayment`), AIT/discount/collection-by/bank/cash, return-adjusted due, `sp_Sales_CustomerPaymentPosting` | no dedupe/posting, due ignores returns, UI binds hidden controls | `CustomerPayment.aspx.cs:435-500`; `SubDeportCustomerPayment.aspx.cs:221-295`; `CustPaymentDAL.cs:205-229` |
| Freeze release | `OtherStockAction` - Restricted test commented out (any user) | `SubDeportStockFreez` - Restricted only login `21900` (hard-coded), wrong-row check | `OtherStockAction.aspx.cs:284`; `SubDeportStockFreez.aspx.cs:384-433` |
| Stock adjustment | voucher (`tblDeStockOutMaster`, label-based grid, parameterised DAL) + approval `sp_UD_DcStockOutApproval` | voucher (`tblSubDepotStockOutMaster`, index-based grid) + `sp_UD_SubDcStockOutApproval`, **plus** un-approved direct write-off page; reject hits DC table | `DepotStockAdjustmentsVoucher.aspx.cs:240-280`; `DeStockOutDal.cs:138,189`; BR-M12-021..026 |
| Stock valuation report | `sp_Report_DCStore_StockValuationLedger` (qty, rate, value) | quantity buckets only | `DCStockReportDAL.cs:462-545,~560` |
| Order routing | main lists filter `IsSubDepo=0/NULL` | `IsSubDepo=1` + route `IsSubDepo=1` | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1363`; `OrderInfoDAL.cs:244-262` |
| Access control | DC lists restricted by `tblUserCompanyUnit` on most pages | transfer/adjust/invoice pages list all DCs | BR-M12-045 |
| SQL style | mostly parameterised `SInventorySql` | `SubDepot_DAL` concatenated | GAP-M12-003 |

## 6. Roles / permissions observed
Menu-driven only (`tblMainMenu`/`tblMenuDistribution`, not in repo). Code-level: `Session UserType=='Admin'` (receive list, stock report), `UserId` DC restriction (freeze, payment, receive list), `LoginName=='21900'` special user (freeze release), `tblAppSetup` assignment gate (voucher approval, BR-M12-027). Full role matrix UNKNOWN.

## 7. Data model (tables and SPs that matter)
Tables: tblSubDepot, tblSubDepotStore, tblSubDepotStoreFreeze, tblSubDepotChalanInfo/Detail, tblSubDepotChalanReturnInfo/RetuenDetail, tblSubDepoStockAdjustment/AdjustDetail, tblSubDepotStockOutMaster/Details, tblSubInvoiceMaster/Detail/MasterBatch, tblCustomerPay + tblCustPayDetail.SubDeportInvoiceId, tblOrder.IsSubDepo, tblRouteInformationMaster.IsSubDepo, tblReturnInvoice(ISSubdeport, SubInvoiceId), tblDCStore/tblDCStoreFreeze (DC side).
SPs (bodies NOT in repo): `ExecuteAllSqlQueryByStoreProcedure`, `sp_Process_SubDepoProformaInvoiceByOrderId`, `sp_SubdeportDeliveryConformationFull`, `sp_SubDeportDeliveryConformationReject`, `sp_Delete_ProformaInvoice_SubDeport`, `sp_DeleteDeliveryInvoiceSubdepo`, `sp_UD_SubDcStockOutApproval`, `sp_SubdeportAreawiseDailyOpeningClosingStockNational`.

## 8. Integrations
None (no SAP/mobile/API from sub-depot pages). `SInventoryWebService.GetSubDepotInvoiceNo` (autocomplete for voucher page, `Solution.Web/App_Code/SInventoryWebService.cs:189`).

## 9. Reports
See [reports/README.md](reports/README.md) (RPT-M12-01..08).

## 10. Workflows
[WF-M12-01 DC->Sub-depot transfer](workflows/WF-M12-01-dc-to-subdepot-transfer.md) | [WF-M12-02 Sub-depot->DC return](workflows/WF-M12-02-subdepot-to-dc-return.md) | [WF-M12-03 Stock out / adjustment & approval](workflows/WF-M12-03-stock-out-adjustment-approval.md) | [WF-M12-04 Freeze & release](workflows/WF-M12-04-freeze-and-release.md) | [WF-M12-05 Sub-depot invoicing, delivery, payment](workflows/WF-M12-05-subdepot-invoicing-payment.md) | [WF-M12-06 Reversals](workflows/WF-M12-06-invoice-payment-reversal.md)

## 11. Business rules index
| BR ID | Rule (summary) | Confidence |
|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId) | CONFIRMED |
| BR-M12-002 | Sub-depot registration requires name, address, code, mobile no and parent DC (messages `Please Input Sales Center Name!!`, `Please Input Address!!`, `Please Input Code!!` | CONFIRMED |
| BR-M12-003 | Sub-depot code is proposed as a hard-coded 3-letter prefix by parent-DC id (1 BAR, 2 BOG, 3 CHA, 4 CUM, 6 KUS, 7 KHU, 8 MYM, 9 RAJ, 10 RAN, 11 SYL, 12 DHA) + zero-padded  | CONFIRMED |
| BR-M12-004 | Sub-depot documents use the text prefix `SDC` + 6-digit zero-padded (MAX(id)+1): DC->Sub-depot chalan (tblSubDepotChalanInfo.SChalanId), Sub-depot->DC return chalan (tblS | CONFIRMED |
| BR-M12-005 | DC->Sub-depot product search lists tblDCStore rows of the selected DC (matched by ComUnitCode) for the selected product with an ACTIVE tblUnitPrice row | CONFIRMED |
| BR-M12-006 | Add-to-list keeps only rows whose checkbox is ticked AND whose Transfer Qty is non-empty; a DCStoreId already in the chalan list is silently not added again (HasDCStoreId | CONFIRMED |
| BR-M12-007 | Transfer quantity accepts digits only (FilteredTextBoxExtender ValidChars 0-9) | CONFIRMED |
| BR-M12-008 | Chalan submit validation (DC->Sub and Sub->DC): chalan no present, chalan date present, manufacturer selected, from-unit code present, to-unit code present, at least one  | CONFIRMED |
| BR-M12-009 | Chalan valuation: per line Value = UnitPrice x Qty; Vat = Qty x VATAmountPerUnit; ValueWVat = Value + Vat; header TotalValue/TotalVat/GrandTotal are the sums | CONFIRMED |
| BR-M12-010 | DC->Sub-depot send: header saved with IsDeliver=false (INSERT tblSubDepotChalanInfo), each line saved to tblSubDepotChalanDetail, and DC stock is reduced IMMEDIATELY by r | CONFIRMED |
| BR-M12-011 | Sub-depot->DC return send mirrors BR-10 but writes tblSubDepotChalanReturnInfo/tblSubDepotChalanRetuenDetail and reduces the SUB-DEPOT store row by read-modify-write `UPD | CONFIRMED |
| BR-M12-012 | Sub-depot->DC product search returns sub-depot stock (`StockQty>0`, active unit price) for the selected product from ALL sub-depots: the sub-depot argument passed by the  | CONFIRMED |
| BR-M12-013 | Sub-depot receive list shows chalans of the selected sub-depot with IsDeliver='False' OR NULL (list) - the receive screen itself reloads only IsDeliver='False' (a NULL fl | CONFIRMED |
| BR-M12-014 | Sub-depot receive creates one NEW tblSubDepotStore row per chalan line: StockQty = received qty, TotalQuantity = quantity sent, DamageQty = damaged qty, MfgDate/ExpDate f | CONFIRMED |
| BR-M12-015 | Received qty and damaged/un-received qty textboxes drive each other on post-back (rcv = sent - damage; damage = sent - rcv) | CONFIRMED |
| BR-M12-016 | Damaged/un-received qty > 0 on receive creates a freeze row (INSERT tblSubDepotStoreFreeze; StockCondition 'Restricted'; StockQty = TotalQuantity = damage qty; SubDCStore | CONFIRMED |
| BR-M12-017 | A chalan is flagged delivered (IsDeliver='True') as a side-effect of saving the FIRST received line (ChalanUpdate is called inside DCStockIn2 for every line) | CONFIRMED |
| BR-M12-018 | DC receipt of a Sub-depot->DC return inserts a new tblDCStore row per line (plain INSERT with SChalanDetailsId, StockCondition 'Available', no Trans_Type/CostPerUnit/Tota | CONFIRMED |
| BR-M12-019 | The DC-side list of pending Sub-depot->DC returns is not filtered by DC or sub-depot: `SubdeportChalanLoadInReceive(comunitId)` ignores its argument and returns every ret | CONFIRMED |
| BR-M12-020 | DC-side damaged qty on a return receipt is stored in tblDCStoreFreeze as 'Restricted' (StockQty = damage), with ComUnitId taken from the hidden `hdComUnitId` (= ComUnitId | INFERRED |
| BR-M12-021 | Direct sub-depot stock adjustment (Stock Adjustment / StockOutSubDepot): product search reads tblSubDepotStore of the chosen sub-depot with StockQty>0 and active unit pri | CONFIRMED |
| BR-M12-022 | StockOutSubDepot Submit does not call its own Validation(): with an empty list a header row is still inserted; the header id is `MAX+1` so `id>0` is always true and `Stoc | CONFIRMED |
| BR-M12-023 | Sub-depot Stock Adjustment Voucher: required DC, proforma invoice, stock-out date, reason, product and at least one line (`Please Select Distribution center!!`, `Please S | CONFIRMED |
| BR-M12-024 | Voucher approval: only vouchers with Status='Posted' are listed; Approve sets Status='Approved', ApprovedBy=LoginName, ApprovedDate=today via SP sp_UD_SubDcStockOutApprov | INFERRED |
| BR-M12-025 | Voucher rejection is routed through the DC stock-out method: it updates `tblDeStockOutMaster` (the main-depot DC stock-out master) WHERE DcStockOutMasterId = <sub-depot v | CONFIRMED |
| BR-M12-026 | A voucher can be deleted only while Status != 'Approved' (master row deleted, then details). | CONFIRMED |
| BR-M12-027 | Access to the voucher approval list is granted only if the page's menu row (tblMainMenu.URL like the file name) has an assignment in tblAppSetup for the session UserId; o | CONFIRMED |
| BR-M12-028 | Freeze list (Freeze Stock Release): rows of tblSubDepotStoreFreeze with StockQty>0 for the chosen sub-depot and manufacturer, ordered by ExpDate, with Amount = tblUnitPri | CONFIRMED |
| BR-M12-029 | Release quantity rules: return qty must be non-empty and not 0 (`Insert Return Quantity !!`) and <= the row's StockQty (`Return Quantity must be Less then Stock Qty`) | CONFIRMED |
| BR-M12-030 | Release effect: tblSubDepotStoreFreeze.StockQty := StockQty - qty and tblSubDepotStore.StockQty := StockQty + qty on the ORIGINAL store row (SubDCStoreId), by read-modify | CONFIRMED |
| BR-M12-031 | Rows whose StockCondition is 'Restricted' can be released only when Session LoginName = '21900' (hard-coded user) | CONFIRMED |
| BR-M12-032 | Sub-depot payable invoices: tblSubInvoiceMaster with DeliveryTpGrandTotal>0, matching ComUnitId and MarketId, PaymentStatus NULL or 'Partial', DelivaryInvoiceNo NOT NULL  | CONFIRMED |
| BR-M12-033 | Payment save validation: at least one invoice ticked (`Please Select Invoice from List!!`); each ticked row has a pay amount (`Please fill out Pay Amount !!`); payment am | CONFIRMED |
| BR-M12-034 | Over-payment guard: on pay-amount change, if (entered + previous payment) > invoice amount the box is reset to 0 with `Cannot Be Greater then Invoice Quantity ` (sic) | CONFIRMED |
| BR-M12-035 | Payment status: if (pay + previous) equals the Del-invoice amount (grid cell 7) status='Full' else 'Partial'; tblSubInvoiceMaster.PaymentAmount is overwritten with (pay + | CONFIRMED |
| BR-M12-036 | Payment persistence: per ticked invoice one tblCustomerPay row (MarketId, ComUnitId, date, amount, PayType text, RefNo, RefDate, CreateBy=LoginName, CreateDate) and one t | CONFIRMED |
| BR-M12-037 | Sub-depot payment reverse (by order code): finds the sub invoice via tblOrder.OrderCode, sets tblSubInvoiceMaster.PaymentStatus/PaymentAmount to NULL and deletes ONLY the | CONFIRMED |
| BR-M12-038 | Sub-depot proforma candidates: tblOrder with IsSubDepo=1, IsInvoice=0, chosen DC, manufacturer and distribution route; the route list only offers routes flagged tblRouteI | CONFIRMED |
| BR-M12-039 | Batch id for a proforma run = literal `06-` + (max numeric suffix of tblSubInvoiceMasterBatch.BatchNo + 1); the `06` prefix and the route-list DC id (6) are hard-coded ra | CONFIRMED |
| BR-M12-040 | Proforma generation per ticked order calls SP sp_Process_SubDepoProformaInvoiceByOrderId(@OrderId,@UserId,@BatchNo1) (body not in repo) | CONFIRMED |
| BR-M12-041 | Delivery confirmation of a sub-depot proforma: status Full -> SP sp_SubdeportDeliveryConformationFull(@InvoiceNo,@UpdateBy,@UpdateDate); Reject -> sp_SubDeportDeliveryCon | CONFIRMED |
| BR-M12-042 | Deleting a sub-depot proforma or delivery invoice calls sp_Delete_ProformaInvoice_SubDeport / sp_DeleteDeliveryInvoiceSubdepo | INFERRED |
| BR-M12-043 | Sub-depot stock report is quantity-only and is built from five buckets per sub-depot/product/batch: Available (tblSubDepotStore.StockQty>0 & StockCondition='Available'),  | CONFIRMED |
| BR-M12-044 | Deleting a sub-depot sales-return from the list runs the shared return delete: ReturnInvoice, ReturnInvoiceDetail, ReturnAmount rows and every tblDCStoreFreeze row linked | INFERRED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)) | CONFIRMED |
| BR-M12-046 | On DC change the Stock Adjustment Voucher page overwrites the login session value `Session["ComUnitId"]` with the selected DC id, altering the identity used by later page | CONFIRMED |
| BR-M12-047 | Selecting a product on DC->Sub-depot and Stock Adjustment pages fires the DC dropdown handler (`onselectedindexchanged=salescenterDropDownList1_SelectedIndexChanged`), wh | CONFIRMED |
| BR-M12-048 | The Print button on the Sub-depot->DC return page opens the DC->Sub-depot chalan viewer (`DcToSubdepotChalanReportViewer`), which queries tblSubDepotChalanInfo/Detail by  | INFERRED |
| BR-M12-049 | On the Sub-depot->DC return page `ChalanDetail.DCStoreId` (persisted to tblSubDepotChalanRetuenDetail.DCStoreId) carries the SUB-DEPOT store id (DataKey `SubDCStoreId`) | INFERRED |
| BR-M12-050 | The Remove (delete row) handler of the Sub-depot->DC page rebuilds the chalan grid with a `DCStoreId` column while the grid is bound with DataKeyNames `SubDCStoreId,...`; | INFERRED |

## 12. Known gaps (summary) - see [GAPS-M12.md](GAPS-M12.md)
Top risks: no transactions + in-place stock updates; DC->Sub issue bypasses DC ledger; Sub->DC search unfiltered by sub-depot and DC receipt hard-coded to DC 6; payment page bound to hidden controls; approval reject updates the wrong table; hard-coded user 21900; delivery/invoice/approval SP bodies missing; empty catch on proforma generation; three copies of the same DAL.

## 13. Still unverified
SP bodies listed in section 7 (stock effects of proforma, delivery confirm, delete, approval); DB DDL/constraints/defaults; menu/role data; behaviour of Tier-B pages 426-440, 442-445, 447, 449, 454, 455, 457, 459-462 beyond what is cited here (viewers/lists read for reports/workflows only); Crystal `.rpt` internals.
