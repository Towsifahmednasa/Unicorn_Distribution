---
page_id: PAGE-218
module: M06
page: InvoiceCreationForCustomerByOrder
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-218 — InvoiceCreationForCustomerByOrder (proforma invoice from one order)

> **Evidence level:** Tier A — read in full: `Solution.Web/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx.cs` (3207 lines; lines 1-1265, 2280-2460, 2539-3207 read line-by-line, remainder = add/remove-row and per-field recalculation handlers skimmed by signature), `InvoiceBLL.cs` (SaveInvoice, SaveInvoiceDetails, UpdateDCStock, InvoiceDetailInsertBLL, InvoiceNoGenerator), `InvoiceDAL.cs` (SaveDataForInvoice, SaveDataForInvoiceDetails, UpdateDCStoreQuantity, UpdateInvoiceHeaderTotals, SaveDataForReturnAmount, UpdateOrder, ProductInfoDAL), `OrderInfoDAL.cs`. `rules` is PARTIAL because the ~2000 lines of row-level recalculation handlers (qty/dp/bQty text-change) and the aspx JS were not read line-by-line and `BatchWiseProductQty` ordering/`View_OrderCustomerInfo`/`View_DCStoreCurrentStock` definitions are not in repo. Cross-module: writes M07 invoice tables and DC stock (M03/M04 inventory).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-218 |
| Module | M06 (**cross-link M07 Invoice**, inventory `tblDCStore`) |
| Kind | Entry / Transaction (core) |
| Class | `SInventory_UI_InvoiceCreationForCustomer` |
| Code-behind LOC | 3207 (aspx 969) |
| Dead code | 8 per-product FOC helpers `Moticare()`, `Nervaid75mgCapsuleFOC()`, `FlexidolFOC()`, `Ezepain()`, `MaxiventFOC()`, `SeacoralDTablet()`, `CefimaxFOC()`, `CiprodylFOC()` (`.cs:297-517`) plus `B()…B6()`, `ChkProductOffer`; hard-coded product codes (`FGDsa02`,`AELss02`,`AIssD01`,`AIDs04`,`sOAD04`,`MNSs03`,`ANB0s85`,`ANBs098`) — **not referenced** from `saveButton_Click` (INFERRED from reading save path; not exhaustively grepped for callers inside the 2000-line middle section — GAP-M06-016) |

## 2. Purpose
Loads one accepted customer order (id from `Session["OrderId"]`) into an invoice grid, applies stock limits, trade-policy/campaign discounts, VAT, delivery charge and any customer credit balance, lets the user pick the delivery agent and save. Saving creates the proforma invoice (`tblInvoice` + `tblInvoiceDetail`), issues stock from batch-wise DC store rows, and marks the order invoiced. CONFIRMED.

## 3. Business Objective
Convert an order to a billable, stock-reserving proforma invoice: quantities never exceed available DC stock, prices/VAT/discounts follow the order snapshot and policy tables, receivable is reduced by the customer's credit (returns/advance) and the resulting document gets a unique invoice number. INFERRED objective from CONFIRMED mechanics.

## 4. Navigation
Inbound: PAGE-217 `gotoinvoiceButton_Click` (Session["OrderId"]); also `SampleInvoiceCreationByOrder`, `DoctorInvoiceCreationByOrder`, `ProformaInvoiceCreation.aspx.cs:62`. Outbound: `backLinkButton`/redirects to `InvoiceCreationByOrder.aspx` (`:187,1242,2640`); print → `../SInventory_RPTVIEW/ProformaReportPrintViewer.aspx?InvNo=` (`:3146`).

## 5. Route
`~/SInventory_UI/InvoiceCreationForCustomerByOrder.aspx` — depends on Session state, not the query string. **If `Session["OrderId"]` is null the page renders an empty form** (no guard); Session["OrderId"]/["RootNameId"] are cleared after load (`:131-132`), so refresh/back loses the order.

## 6. Source Files
aspx/aspx.cs; `Library.BLL/SInventory_BLL/InvoiceBLL.cs`, `OrderInfoBLL.cs`, `RequisitionBLL`; `Library.DAL/SInventory_DAL/InvoiceDAL.cs`, `OrderInfoDAL.cs`, `ProformaOrInvoiceReturn.cs`, `MasterSetup_DAL/RouteInformationDAL`, `BonusCampaignNewDAL`; `DoctorModule_DAL/CommonDataLoad.cs:1908`; SQL bodies `SP_Fix_Invoice_Anomaly_And_Sync.sql`, `sp_Delete_ProformaInvoice_Update.sql`.

## 7. UI Control Inventory
Customer block: `custCodeTextBox`(RO), `custNameTextBox`(RO), `custAddressTextBox`, `comUnitNameTextBox`, `miaCodeTextBox`, `miaNameTextBox`, `districtNameTextBox` (holds ASM name), `areaNameTextBox`, `marketNameTextBox`, `custCategoryTextBox` (Type e.g. "FCB FY-23-24"/INSTITUTION/RCP), `cusTypeTextBox`. Invoice block: `invDateTextBox` (today `dd-MMM-yyyy`), `orderNoTextBox`(RO), `orderDateTextBox`(RO), `payTypeDDL` (`PaymentTypeLoadBLL`, index 1 preselected), `ddlDAName` (**required**, `RouteInformationDAL.GetDANameDDL(ComUnitId)`), `remarksTextBox`, `custDeliveryAddressTextBox`, `deliverypersonNameTextBox`, `deliverypersonMobileTextBox`, order-remarks alert `divOrderRemarksAlert/lblOrderRemarksSpecial` (shown when order has remarks). Grid `gridLineItemGridView` per line: `codeTextBox`, `orderdetailIdHiddenField`, `CampaignTypeHiddenField`, `nameTextBox`(RO), `currentStockTextBox`, `unitPriceTextBox`, `upVatTextBox`, `qtyTextBox`(RO), `tpTextBox`, `dpTextBox` (discount %), `dpAmtTextBox`, `sdTextBox` (campaign flag), `tpVatTextBox`, `npTextBox`, `bQtyTextBox` (gift flag), `tQtyTextBox`, `amTextBox` (credit adjustment). Totals: `tpTptalTextBox`, `disTotalTextBox`, `pdTextBox`, `vatTotalTextBox`, `deliveryChargeTextBox` (editable, postback), `grandTotalTextBox`, `crAmountTextBox` (customer credit), `rcvAmountTextBox` (receivable), `adjustInvoiceNoTextBox`. Buttons: `submitButton` (`OnClientClick=sweetAlertConfirm_Submit`, `OnClick=saveButton_Click`), `printButton`, `invTextBox`(generated invoice no), back link. Hidden: `orderHiddenField`, `orderIdHiddenField`, `hdCustomerMasterId`, `hdComUnitId`, `hdMiaId`, `warningLabel`.

## 8. Field Mapping
| UI | DTO | Column |
|---|---|---|
| invDateTextBox | Invoice.InvoiceDate | tblInvoice.InvoiceDate |
| orderNoTextBox/orderDateTextBox | OrderNo/OrderDate | tblInvoice.OrderNo/OrderDate |
| hdCustomerMasterId, hdComUnitId, hdMiaId | CustomerMasterId, ComUnitId, MiaId | tblInvoice.* |
| payTypeDDL | PaymentTypeId | tblInvoice.PaymentTypeId |
| tpTptalTextBox/disTotalTextBox/vatTotalTextBox/grandTotalTextBox | TpTotal/TpDiscount/TpVat/TpGrandTotal | tblInvoice.TpTotal/TpDiscount/TpVat/TpGrandTotal (later recomputed from details, BR-043) |
| Session["UserId"] | UserId | tblInvoice.UserId |
| orderIdHiddenField | OrderId | tblInvoice.OrderId |
| ddlDAName | Inv_DANameId | tblInvoice.Inv_DANameId |
| pdTextBox | TotalSpecialAmount | tblInvoice.TotalSpecialAmount (always "0.00" — `sptotatl` never accumulated, `:3060,3109`) |
| cusTypeTextBox | cusType | tblInvoice.CustomerType |
| — | OldTradePolicy=false, ProductOffer="False", Createdate=Now | tblInvoice.OldTradePolicy/ProductOffer/CreateDate |
| remarks/delivery address/person/mobile | Remarks, CustomerDeliveryAddress, DpNAme, DpMob | tblInvoice.Remarks, CustomerDeliveryAddress, DeliveryPersonName, DeliveryPersonPhNo |
| customer view (`View_OrderCustomerInfo`) | MIACode, MIAName, MarketCode/Name, AreaCode, DisCode, FEName, RegionCode, DZSMName, Type | tblInvoice.MIACode … Types |
| order.FixedCustomer | FixedCustomer | tblInvoice.FixedCustomer |
| deliveryChargeTextBox | DeliveryCharge | tblInvoice.DeliveryCharge |
| crAmountTextBox | AdjustAmount; IsAdjustInvoice=(cr>0); AdjustInvoiceNo | tblInvoice.AdjustAmount/IsAdjustInvoice/AdjustInvoiceNo_ReturnInvoiceNo |
| rcvAmountTextBox | ReceivableAmount | tblInvoice.ReceivableAmount |
| line: code/name/pack | ProductCode/Name/PackSize | tblInvoiceDetail.* |
| unitPriceTextBox/upVatTextBox | UnitPrice/UnitVatAmount | tblInvoiceDetail.UnitPrice/UnitVatAmount |
| qty split by batch | Quantity/BonusQuantity/TotalQuantity | tblInvoiceDetail.Quantity/BonusQuantity/TotalQuantity (BonusQuantity always 0 from this page) |
| dpTextBox | DiscountPercentage | tblInvoiceDetail.DiscountPercentage (amount recomputed = TotalPrice × % /100 in BLL — **the UI `dpAmtTextBox` value is NOT used for the stored amount**, except for flat-rate campaigns' net) |
| amTextBox | AdjustmentAmount | tblInvoiceDetail.AdjustmentAmount |
| ProductDiscount(customer,date) | SpecialAmountPer → SpecialAmount=TotalPrice×%/100 | tblInvoiceDetail.SpecialAmount |
| orderdetailIdHiddenField | OrderDetailsId | tblInvoiceDetail.OrderDetailsId |
| sdTextBox / bQtyTextBox / CampaignTypeHiddenField | IsCampaignProduct / ISGiftProduct / Campaign type | tblInvoiceDetail.IsCampaignProduct/ISGiftProduct/Campaign (`SaveDataForInvoiceDetails` param mapping in `InvoiceDetailParameters`) |
| batch row | BatchNo, ReceiveDate, ExpDate, CostPrice, DCStoreId | tblInvoiceDetail.* |

## 9. Dropdown Mapping
`payTypeDDL` ← `InvoiceDAL.PaymentTypeLoad` (source table not read — UNKNOWN); `ddlDAName` ← `RouteInformationDAL.GetDANameDDL(comUnitId)` → `sp_Get_DDLDANameByDCID` (body UNKNOWN), text `TextField`/value `Value`; per-row reason lists n/a.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Save (`submitButton`) | `saveButton_Click:2551` | BR-028..047 |
| Print | `printButton_Click:3117` | needs invoice no; runs anomaly check; may DELETE the just-created proforma (BR-048) |
| delivery charge change | `deliveryChargeTextBox_TextChanged` | `TotalValueCalculation()` |
| qty/dp/bQty/code changes, add/remove row | many handlers | recompute only |

## 11. Frontend Flow
`Page_Load` (first load, session order id present): `LoadAllDataByOrder(orderId)` → `GetCustInfo` → for each order line `AddFunc()` (append grid row), `GetProduct` (stock, unit price, VAT for the customer's DC), override price/VAT from the order line, apply stock cap → discount logic → `TotalValueCalculation`; then DA list, `CustomerCreditAmount()`, `AdjustmentAmount()` (spread credit over rows), recompute net per row; `PaymentTypeLoad`, `payTypeDDL.SelectedIndex=1`, `Todate()`; clears the session keys. If `gridLineItemGridView.Rows.Count != order line count` → redirect to PAGE-217 (`:1236-1244`). Each edit is a full postback (no JS totals). `sweetAlertConfirm_Submit` confirms save.

## 12. Backend Flow (save)
`saveButton_Click`:
1. `ValiMethod()` — each grid line's `OrderDetailId` must still exist in `tblOrderDetail` for the order; else `Response.Redirect("InvoiceCreationByOrder.aspx")` (`:2553-2641`).
2. `Validation()` (BR-030..032).
3. `OrderExists(orderId)` = any `tblInvoice` with this `OrderId`; if yes ⇒ "Invoice already Generated !!".
4. `SaveInvoice()` → `InvoiceBLL.SaveInvoice`: `invoiceId = PrimaryKeyMax("invoiceId","tblInvoice")` (MAX+1, only used for the batch row), `invoiceNo = "INV-" + comUnitCode + MAX(InvoiceId)+1 padded to 10`, `batchn = MAX(BatchNo suffix)+1` (prefixed `Session["RouteId"]-` when set and ≠0), `SaveDataForInvoiceBatch(invoiceId, batchn)`, then `SaveDataForInvoice` (inline INSERT; InvoiceId is **not** in the column list ⇒ identity).
5. Fetch real invoice PK: `ProformaOrInvoiceReturnBLL.SelectInvoiceID(orderId)`.
6. `SaveInvoiceDetail(invoicePK)` — per line: skip (mark `tblOrderDetail.Status='Undelivered'`) when `currentStock == 0`; otherwise add; then `InvoiceBLL.SaveInvoiceDetails(list, comUnitId)` → per line `UpdateDCStock` (batch allocation, below).
7. `UpdateInvoiceHeaderTotals(invoicePK)` (header ← Σ details), `SaveReturnAmount(invoicePK)` (insert `tblReturnAmount(CustomerId, InvoiceId, Amount = −crAmount)`), `UpdateInvoiceStatus(orderId)` (`tblOrder.IsInvoice='True'`).
8. Anomaly check (result ignored — the `DeleteProforma` call is commented out at `:2611`), `Clear()`, success alert "Invoice Created Successsfully!".
9. `catch`: `aInvoiceBll.DeleteInvoice(orderId)` → `sp_Deletenvoice(@OrderID)` (body UNKNOWN) and alert "Invoice generation failed: <ex.Message>". **No SQL transaction**; each DAL call is autocommit.
`UpdateDCStock`: gets batch rows (`BatchWiseProductQty(productCode, comUnitId)`: `tblDCStore` receive rows, `StockQty = (TotalQuantity>0 ? TotalQuantity : StockQty) + Σ Issued`), walks batches in returned order, splits qty across batches, and for each batch inserts an invoice-detail row and calls `UpdateDCStoreQuantity(dcStoreId, issuedQty)` which INSERTs a negative-quantity `tblDCStore` row (`Trans_Type='Issue'`, `ReferenceDCStoreId=<receive row>`, `StockQty=-issued`, cost snapshot).

## 13. API Mapping
None.

## 14. Service Mapping
InvoiceBLL: SaveInvoice, SaveInvoiceDetails, UpdateInvoiceHeaderTotals, UpdateOrder, ProductInfo, CustomerMaster, ProductDiscount, LoadProductQty, PaymentTypeLoadBLL, DeleteInvoice; OrderInfoBLL: LoadOrderWithDetail, LoadOrderExistsBll, LoadOrderWithDetailIDCheck, GetFixedCustomer, GetCustomerCredit, GetTradeTerm, GetParcentFromOrderDetails, ProductVat, ProductDiscount, UpdateInvoiceStatus, GetInvoNOGetByInvoID, LoadOrderWithDetailFrindsHospital; ProformaOrInvoiceReturnBLL.SelectInvoiceID/DeleteProforma; CommonDataLoad.Check_anomalyInvoiceDetails.

## 15. Repository Mapping
Key SQL: `InvoiceDAL.ProductInfoDAL:588` (`tblProduct ⟕ tblUnitPrice ⟕ View_DCStoreCurrentStock(ComUnitId, ProductCode)` → UnitPrice, StockQty, VAT (`VATAmountPerUnit`), CostPrice, VATPercentage); `LoadCustomerMaster:639` (`View_OrderCustomerInfo` by OrderCode); `UpdateInvoiceHeaderTotals:507`; `SaveDataForInvoice:361`; `SaveDataForInvoiceDetails:473`; `UpdateDCStoreQuantity:666`; `SaveDataForReturnAmount:1994`; `UpdateOrder:1130` (`tblOrderDetail.Status`); `OrderInfoDAL.GetTradeTerm:815`, `GetCustomerCredit:702`, `LoadOrderExistsDal:805`, `UpdateInvoiceStatus:1127`, `GetFixedCustomer:692` (matches OrderId, CustomerCode OR OrderCode — key collision risk).

## 16. Stored Procedure Mapping
| SP | Body | Use |
|---|---|---|
| sp_Deletenvoice | UNKNOWN | rollback on exception |
| sp_Delete_ProformaInvoice | CONFIRMED | from print (BR-048) |
| sp_Check_anomalyInvoiceDetails | CONFIRMED | anomaly check |
| sp_Get_DDLDANameByDCID | UNKNOWN | DA list |

## 17. Database Mapping
Written: `tblInvoice`, `tblInvoiceBatch`, `tblInvoiceDetail`, `tblDCStore` (Issue rows), `tblReturnAmount`, `tblOrder.IsInvoice`, `tblOrderDetail.Status`. Read: tblOrder/Detail, View_OrderCustomerInfo, tblProduct, tblUnitPrice, View_DCStoreCurrentStock, tblProductDiscount, tblTradePolicyNew/New2, tblReturnAmount, tblOrder.

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| Order no non-empty | `Validation:212` | "Please Input Order Number!!" | 030 |
| Order date non-empty | `:217` | "Please Input Order Date!!" | 030 |
| Payment type | `:223` | "Please Input Payment Type!!" | 030 |
| Receivable ≥ 0 | `:228` | "Invalid Receivable Amount!!" | 032 |
| No blank line (code or total qty empty) | `:233-240` | "Please Remove Blank Row!!" | 031 |
| DA chosen | `:242` | "Please Select DA Name!!" | 030 |
| Order lines unchanged | `ValiMethod` | redirect only | 029 |
| Invoice exists for order | `OrderExists` | "Invoice already Generated !!" | 028 |
| Credit limit / stock ≥ qty / min qty | not validated at save (stock=0 lines silently dropped) | — | 033 |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M06-028 | One proforma per order: save is refused when any `tblInvoice.OrderId` matches; order also marked `IsInvoice='True'`. | .cs:2558, 2630-2633; OrderInfoDAL.cs:805, 1127 | page+SQL | CONFIRMED |
| BR-M06-029 | Order lines are re-verified against `tblOrderDetail` by id before save; any mismatch aborts back to the list. | .cs:2644-2672 | page | CONFIRMED |
| BR-M06-030 | Required: order no, order date, payment type, delivery agent. | .cs:212-247 | page | CONFIRMED |
| BR-M06-031 | No blank product row (code or total qty empty). | .cs:233-240 | page | CONFIRMED |
| BR-M06-032 | Receivable amount (grand total − customer credit) must not be negative. | .cs:228-232 | page | CONFIRMED |
| BR-M06-033 | Stock cap: invoiced qty = min(order qty, DC current stock); rows where stock < ordered are painted red and excluded from totals (`cstock >= tqty` only) and from credit spreading; at save lines with stock 0 get `tblOrderDetail.Status='Undelivered'` and no invoice line. | .cs:594-605, 838-865, 3075-3094, 2860-2863; AdjustmentAmount:141-147 | page | CONFIRMED |
| BR-M06-034 | Line net = (TP − (discount amount + credit adjustment)) + VAT, where TP = unit price × qty and VAT = unit VAT × qty; price and VAT come from `tblUnitPrice` for the DC but **are overridden by the order line's TradePrice/UnitVatAmount when present**. On save, `TotalPrice = qty × UnitPrice`, `DiscountAmount = TotalPrice × DiscountPercentage/100`, `TotalPriceVatAmount = UnitVat × qty`, `NetAmount = TotalPrice − DiscountAmount − AdjustmentAmount + VAT`. | .cs:572-591, 1141-1218; InvoiceBLL.cs:389-433 | page+BLL | CONFIRMED |
| BR-M06-035 | Header totals: TP total, VAT total, discount total = Σ over in-stock rows; grand total = Σ line net + delivery charge; receivable = grand total − customer credit. After details are saved the header is recomputed from details: `TpGrandTotal = ΣNet + DeliveryCharge`, `ReceivableAmount = ΣNet + DeliveryCharge − AdjustAmount`. | .cs:3054-3115; InvoiceDAL.cs:507-531 | page+SQL | CONFIRMED |
| BR-M06-036 | Customer credit (`Σ tblReturnAmount.Amount` for the customer) is shown, deducted from receivable, distributed **equally** across non-red lines as per-line `AdjustmentAmount`, and consumed by inserting a negative `tblReturnAmount` row for this invoice (even when the amount is 0). `IsAdjustInvoice` = credit>0. Note `AdjustmentAmount()` is only called at first load, so in-page qty changes do not re-spread it. | .cs:169-184, 136-167, 2539-2550, 3001-3009 | page+SQL | CONFIRMED |
| BR-M06-037 | Discount % source: if the customer type is "INSTITUTION" or "RCP", % is taken from the order line; if category is "FCB FY-23-24" and a trade-policy row matches, % forced to 0; otherwise % = trade-policy `DiscountPerc` from `tblTradePolicyNew` (order dated ≤ 13-Dec-2024) or `tblTradePolicyNew2` (later; keyed by amount band, PaymentType, CustomerType) using the invoice's order total minus special-rate lines; special-discount orders (`IsSpDis`) keep the discount amount stored on the order line. Gift or campaign lines get 0 %. | .cs:660-1000; OrderInfoDAL.cs:815-846 | page+SQL | CONFIRMED (structure) / INFERRED (policy semantics) |
| BR-M06-038 | Campaign lines are recognised by **hard-coded campaign-name strings** (Special Rate [Triforce 500 / 1g IM / 1g IV / Aminobost] Feb-24/Apr-24 with and without "Bonus Campaign |" prefix; "Ezevent Flat Rate Campaign | Dec-25", "Esomium 20 Flat Rate Campaign | Mar-26", "Seacoral D Flat Rate Campaign | Apr-26"): % and amount come from the order line (flat-rate campaigns: amount from order, % derived = amount/TP×100). New campaigns need a code change. | .cs:667-760, 949-1127 | page | CONFIRMED |
| BR-M06-039 | Gift lines (`tblOrderDetail.ISGiftProduct=1`) are invoiced with TotalPrice, VAT and Net = 0. | InvoiceBLL.cs:391-433 | BLL | CONFIRMED |
| BR-M06-040 | Stock is issued batch-wise from `tblDCStore`: each batch consumed produces an invoice-detail row and a negative `Issue` row (`ReferenceDCStoreId` = receive row, cost snapshot). Available per batch = (TotalQuantity>0 ? TotalQuantity : StockQty) + Σ issued(negatives). Batch ordering = order of `BatchWiseProductQty` result (rest of query not read). | InvoiceBLL.cs:167-255; InvoiceDAL.cs:666-701, 921-964 | BLL+SQL | CONFIRMED (mechanics) / UNKNOWN (ordering) |
| BR-M06-041 | Invoice number `INV-<ComUnitCode><10-digit MAX(InvoiceId)+1>`; not sequence/lock protected (race window between MAX and INSERT). | InvoiceBLL.cs:495-501; InvoiceDAL.cs:1197-1203 | BLL | CONFIRMED |
| BR-M06-042 | Batch tagging: each saved invoice gets a `tblInvoiceBatch` row with `BatchNo = [RouteId-]MAX(numeric suffix)+1`. | InvoiceBLL.cs:93-104 | BLL | CONFIRMED |
| BR-M06-043 | Voucher-safe totals: header totals are always recomputed from saved detail rows after save. | .cs:2573; InvoiceDAL.cs:507 | SQL | CONFIRMED |
| BR-M06-044 | Delivery charge (order value default) is added to grand total and receivable but carried in `tblInvoice.DeliveryCharge`. | .cs:544-559, 3098-3103 | page | CONFIRMED |
| BR-M06-045 | Fixed-customer flag copied from order to invoice (`GetFixedCustomer(orderId)`). | .cs:2883, 2977-2982 | page | CONFIRMED |
| BR-M06-046 | Failure compensation: on any exception `sp_Deletenvoice(orderId)` is invoked and the user sees the raw exception message; no transaction. If failure happens after stock issue but the SP does not undo `tblDCStore` Issue rows the stock stays reserved (SP body UNKNOWN). | .cs:2622-2627; InvoiceDAL.cs:1988-1993 | page | CONFIRMED / UNKNOWN |
| BR-M06-047 | A proforma may be created only from orders with details; the page redirects to the list if the grid could not be rebuilt to the same number of lines as the order. | .cs:1236-1244 | page | CONFIRMED |
| BR-M06-048 | Print button: if the invoice fails the anomaly check (|(TpGrandTotal−AdjustAmount)−ΣNet|>10) the invoice is **deleted** (`sp_Delete_ProformaInvoice`) and the user told "Please generate Invoice again!"; otherwise opens the proforma print viewer. | .cs:3117-3163 | page+SP | CONFIRMED |

## 20. Permission Matrix
Session keys: `OrderId` (handoff), `RootNameId`, `RouteId` (via `InvoiceBLL.SaveInvoice`), `UserId` (invoice creator), `LoginName` (deleter). No role/permission check; **no credit-limit re-check** (PAGE-217 only advises). Any authenticated user with the URL and a forged Session["OrderId"]-producing path can invoice any order. CONFIRMED.

## 21. Status Lifecycle
Order: `IsInvoice 0 → 'True'` (`UpdateInvoiceStatus`, after successful detail save); order detail `Status NULL → 'Undelivered'` for zero-stock lines; delete-proforma resets both. Invoice: created with `DelivaryInvoiceNo=NULL` (proforma) → later delivery confirmation (PAGE-214/215) sets `DelivaryInvoiceNo`, `DeliveryInvoiceStatus`.

## 22. Error Handling
try/catch only around the save with compensation; `AdjustmentAmount`, totals loops use bare `catch{}`; many `Convert.ToDecimal` without TryParse in row handlers (FormatException surfaces as unhandled page error); alert text with apostrophe breaks JS (`Replace("'","\'")` no-op).

## 23. Audit
`tblInvoice.UserId`, `CreateDate` (server `DateTime.Now`), `Remarks`; deletion logs (see PAGE-217). No modification history.

## 24. Integration
Reads `View_OrderCustomerInfo`; accounts DB used later (delivery posting `sp_Sales_DeliverySalesPosting`, M07/M08). No direct SAP call.

## 25. Reports
`ProformaReportPrintViewer.aspx?InvNo=` — see `reports/RPT-M06-01-order-invoice-print.md`.

## 26. Dependencies
Tables in §17; PAGE-217 (entry), PAGE-214/215 (delivery), sp_Delete_ProformaInvoice; hard-coded campaign names; `tblTradePolicyNew(2)`.

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-01 | Positive | Order 2 lines, stock ample, no policy, DA chosen | invoice `INV-<DC>000000nnnn`; details = order lines; `IsInvoice=1`; Issue rows in tblDCStore; header net = Σ | 028,034,035,040 |
| TC-02 | Edge | Ordered qty 10, stock 6 | qty 6 invoiced, row red, excluded from totals | 033 |
| TC-03 | Edge | Ordered qty 10, stock 0 | line skipped, `tblOrderDetail.Status='Undelivered'` | 033 |
| TC-04 | Negative | Save twice (double click) | second: "Invoice already Generated !!" | 028 |
| TC-05 | Negative | No DA | "Please Select DA Name!!" | 030 |
| TC-06 | Edge | Customer credit 300, 3 in-stock rows | 100 per row adjustment; receivable = grand−300; tblReturnAmount −300 | 036 |
| TC-07 | Edge | Credit > grand total | "Invalid Receivable Amount!!" | 032 |
| TC-08 | Positive | Campaign line "Ezevent Flat Rate Campaign | Dec-25" | discount amount from order, % derived | 038 |
| TC-09 | Negative | Force exception after invoice insert (e.g. detail with missing DC store) | sp_Deletenvoice invoked; verify stock/header cleaned | 046 |
| TC-10 | Edge | Print when header≠details by >10 | invoice deleted, alert | 048 |
| TC-11 | Negative | Open page directly (no Session order) | empty form; save attempts fail (verify) | — |
| TC-12 | Edge | Gift line | line total/VAT/net 0 | 039 |

## 28. Reverse Engineering References
`InvoiceCreationForCustomerByOrder.aspx(.cs)`, `InvoiceBLL.cs`, `InvoiceDAL.cs`, `OrderInfoDAL.cs`, `SP_Fix_Invoice_Anomaly_And_Sync.sql`, `sp_Delete_ProformaInvoice_Update.sql`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Credit limit at invoicing | None existed | not enforced here | GAP-M06-005 |
| Transactionality | None existed | none; compensating SP | GAP-M06-010 |
| Campaign handling | None existed | hard-coded names | GAP-M06-011 |

## 30. Known Gaps
No transaction; hard-coded campaign strings/product codes/cut-off date; UI discount % overridden by BLL recompute; credit spread not recalculated; `pdTextBox` special amount always 0; `Session` handoff without guard; MAX+1 numbering races; `GetFixedCustomer` key collision; duplicated business logic with PAGE-214/215/Sample/Doctor variants; raw exception message to user; 8 dead FOC helpers.

## 31. Change Impact
`InvoiceBLL.SaveInvoice/UpdateDCStock/InvoiceDetailInsertBLL` are shared with other invoice-creation pages (Sample, Doctor, ProformaInvoiceCreation, DelivaryInvoice* via `UpdateInvoiceDetail`/`ReturnToDCStore`). `tblDCStore` Issue/Receive semantics are shared with stock reports (`sp_report_dcstore_stockvaluationledger`), `SP_Adjust_DCStore_Stock_Only`, `sp_Check_DCStore_StockAnomalies`. Changing `UpdateInvoiceHeaderTotals` affects every invoice page and `sp_Check_anomalyInvoiceDetails`.
