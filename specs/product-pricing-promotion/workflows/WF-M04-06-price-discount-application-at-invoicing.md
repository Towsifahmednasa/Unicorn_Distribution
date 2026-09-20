# WF-M04-06 Price, trade policy, special discount and campaign application at invoicing (cross-module consumption)

Owned by the invoice/order modules; documented here because it consumes M04 masters. Evidence: `Solution.Web/SInventory_UI/DelivaryInvoiceCreationForCustomerByOrderAuto.aspx.cs:225-360`, `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:758-960`, `InvoiceBLL.cs:455,680`, `InvoiceDAL.cs:914`. The order-time campaign engine is not in this repo (UNKNOWN, GAP-M04-011).

## Steps (delivery invoice, per invoice)
1. Unit price and VAT: `OrderInfoBll.ProductVat(productCode)` = `SELECT * FROM tblUnitPrice WHERE ProductCode` (no IsActive filter; expects one row) (BR-M04-055).
2. Offer/flags: `LoadOffer(invoiceNo)` -> `ProductOffer`, `OldTradePolicy` from `tblInvoice`.
3. Trade-policy percentage (BR-M04-071): if `ProductOffer == "Esomium20"` -> 0. Else if invoice `FixedCustomer` true -> 0. Else if `OldTradePolicy` true and offer not `True1`/`FlexidolParagesicTrue` -> `tblTradePolicy` row where `total BETWEEN MinAmount AND MaxAmount` -> `DiscountPerc`. Else (new policy): order PaymentType, CustTypeId, SubmissionDate from `tblOrder/tblOrderDetail`; if `SubmissionDate <= 13-Dec-2024` -> `tblTradePolicyNew` (amount range only) else `tblTradePolicyNew2` (amount range + PaymentType + CustomerType) -> `DiscountPerc`.
4. Special discount (BR-M04-070): `tblProductDiscount WHERE CustomerMasterId AND ProductCode AND invoiceDate BETWEEN ActiveDate AND InactiveDate` -> `DiscountPercentage`.
5. Formulas (BR-M04-072): `totalDiscount = percentage * tpTotal / 100`; per line `dpAmt = tp * (percentage/100)` (format "F", 2 dp); `sd = tp * (specialPct/100)`; `net = tp - dpAmt - sd + tpVat` (VAT amount column, not recomputed here); header `TpDiscount = disTotal`.
6. Campaign: order lines carry `Campaign`, `CampaignName`, `IsCampaignProduct`; invoice code looks up campaign percentage by trimmed name (`tbl_BonusCampaignNewDetail` join master on name) and three campaign names are hard-coded (`Ezevent Flat Rate Campaign | Dec-25`, `Esomium 20 Flat Rate Campaign | Mar-26`, `Seacoral D Flat Rate Campaign | Apr-26`) (BR-M04-073).

## Precedence summary
Trade policy (tiered % on invoice total, unless fixed customer / Esomium20) and special product discount (customer+product+date) are both subtracted from trade price; campaign discount/bonus is decided at order time and travels on the order line. No single rule in this repo makes one override another.

## Notes / tests
`ProductDiscountEntry`/`ProductBonusEntry` cannot populate `tblProductDiscount` (BR-M04-074, GAP-M04-028). Tests: invoice total on each tier boundary; date 13-Dec-2024 vs 14-Dec-2024; fixed customer; product with and without special discount.
