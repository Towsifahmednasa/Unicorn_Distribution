# M04 - Product, Pricing & Promotion Master

Folder: `specs/product-pricing-promotion/` - reviewed 2026-09-19. Evidence: code in `Solution.Web`, `Library.BLL/DAL/DAO`. **No campaign / quoted-price / promo stored-procedure body exists in the repo**; SP-side behaviour is `UNKNOWN` throughout (see `GAPS-M04.md`).

## 1. Purpose and business overview
M04 holds the master data that every sale rests on: the **product catalogue** and its classification (category, type, brand, generic/therapeutic group, product line, pack size, UOM, shipping carton, manufacturer, supplier), the **unit price** record (cost, trade price, VAT, MRP) with an archive of superseded prices, **campaigns** (bonus / discount / trade-policy / FCFS offers targeted by customer type, product line, validity and payment mode), **quoted prices** (customer-specific discounted prices), and **promo allocation** (promo groups, MIO membership, monthly promo quantities). The *application* of prices, trade policy, special discounts and campaigns to orders/invoices happens in the order/invoice modules and (for campaigns) outside this repo; the consumption points found are documented in WF-M04-06.

## 2. Page catalogue (64 pages)
Tier A = upgraded, verified spec; Tier B = generated spec (not semantically reviewed).

| Page ID | Page | Folder | Kind | Purpose | Tier | Spec |
|---|---|---|---|---|---|---|
| PAGE-103 | GenericGroupEntry | DoctorModule_UI | Entry | Generic (drug) group master - entry, WebMethod-driven; SPs sp_Save/UD_GenericGroup, sp_check_GenericGroup | B | [PAGE-103-genericgroupentry.md](pages/PAGE-103-genericgroupentry.md) |
| PAGE-104 | GenericGroupView | DoctorModule_UI | List | Generic group list | B | [PAGE-104-genericgroupview.md](pages/PAGE-104-genericgroupview.md) |
| PAGE-105 | GroupSetupEntry | DoctorModule_UI | Entry | Market-hierarchy "Group/Region" setup (National/Group code+name); sp_Save_GroupInfo. Conceptually market structure, filed under M04 by folder (DoctorModule_UI) | B | [PAGE-105-groupsetupentry.md](pages/PAGE-105-groupsetupentry.md) |
| PAGE-106 | GroupSetupView | DoctorModule_UI | List | Group list with activate/inactivate (sp_ActiveInactive_GroupInfo) | B | [PAGE-106-groupsetupview.md](pages/PAGE-106-groupsetupview.md) |
| PAGE-107 | ProductLineEntry | DoctorModule_UI | Entry | Product line master entry (UI calls TharapeuticGroupEntry.aspx/Save_ProductLine) | B | [PAGE-107-productlineentry.md](pages/PAGE-107-productlineentry.md) |
| PAGE-108 | ProductLineView | DoctorModule_UI | List | Product line list (UI calls TharapeuticGroupView.aspx/GetProductLineList) | B | [PAGE-108-productlineview.md](pages/PAGE-108-productlineview.md) |
| PAGE-109 | TharapeuticGroupEntry | DoctorModule_UI | Entry | Therapeutic group entry; also hosts the WebMethods for product line (Save_ProductLine) | B | [PAGE-109-tharapeuticgroupentry.md](pages/PAGE-109-tharapeuticgroupentry.md) |
| PAGE-110 | TharapeuticGroupView | DoctorModule_UI | List | Therapeutic group list; hosts GetProductLineList WebMethod | B | [PAGE-110-tharapeuticgroupview.md](pages/PAGE-110-tharapeuticgroupview.md) |
| PAGE-111 | CampaignSetup | MasterSetup_UI | Entry/Transaction | LIVE campaign editor (create/edit) | A | [PAGE-111-campaignsetup.md](pages/PAGE-111-campaignsetup.md) |
| PAGE-112 | CampaignSetup_final | MasterSetup_UI | Entry/Transaction | LEGACY campaign editor with Multiple-Product and Manual-Ration structures (only persisting variant) | A | [PAGE-112-campaignsetup-final.md](pages/PAGE-112-campaignsetup-final.md) |
| PAGE-113 | CampaignSetup_Mult | MasterSetup_UI | Entry/Transaction | LEGACY 2025-05 experiment: multi-select main product (broken) | A | [PAGE-113-campaignsetup-mult.md](pages/PAGE-113-campaignsetup-mult.md) |
| PAGE-114 | CampaignSetup_new | MasterSetup_UI | Entry/Transaction | LEGACY 2023-02 campaign editor (multi-product stub not persisted) | A | [PAGE-114-campaignsetup-new.md](pages/PAGE-114-campaignsetup-new.md) |
| PAGE-115 | CampaignSetupPT | MasterSetup_UI | Entry/Transaction | LEGACY 2024-11 prototype with unbound payment radio | A | [PAGE-115-campaignsetuppt.md](pages/PAGE-115-campaignsetuppt.md) |
| PAGE-116 | CampaignView | MasterSetup_UI | List + Sync action | Campaign list, filter, edit gateway, "Sync Now" (sp_CampaignUpdateFromPage) | A | [PAGE-116-campaignview.md](pages/PAGE-116-campaignview.md) |
| PAGE-117 | OLdCampaignSetup | MasterSetup_UI | Entry/Transaction | LEGACY 2024-12 snapshot (identical code to PAGE-115) | A | [PAGE-117-oldcampaignsetup.md](pages/PAGE-117-oldcampaignsetup.md) |
| PAGE-118 | QuotedPriceSetup | MasterSetup_UI | Entry/Transaction | Quoted (customer-specific) price setup with discount calculation | A | [PAGE-118-quotedpricesetup.md](pages/PAGE-118-quotedpricesetup.md) |
| PAGE-119 | QuotedPriceView | MasterSetup_UI | List | Quoted price list; edit opens PAGE-118 | B | [PAGE-119-quotedpriceview.md](pages/PAGE-119-quotedpriceview.md) |
| PAGE-120 | GroupWisePromoQtyEntry | PromoAlloc | Entry/Transaction | Allocate promo product quantity to group MIOs per month | A | [PAGE-120-groupwisepromoqtyentry.md](pages/PAGE-120-groupwisepromoqtyentry.md) |
| PAGE-121 | GroupWisePromoQtyList | PromoAlloc | List | Group-wise promo quantity list (inline SQL over tblGroupWisePromoQty) | B | [PAGE-121-groupwisepromoqtylist.md](pages/PAGE-121-groupwisepromoqtylist.md) |
| PAGE-122 | GroupWisePromoQtyView | PromoAlloc | List + inline edit | Group-wise promo quantity view; inline qty edit via sp_Update_PromoEmployeeQty | B | [PAGE-122-groupwisepromoqtyview.md](pages/PAGE-122-groupwisepromoqtyview.md) |
| PAGE-123 | PromoGroup | PromoAlloc | Entry | Promo group master entry (code PG-<1000+max id+1>, inline SQL) | B | [PAGE-123-promogroup.md](pages/PAGE-123-promogroup.md) |
| PAGE-124 | PromoGroupList | PromoAlloc | List | Promo group list | B | [PAGE-124-promogrouplist.md](pages/PAGE-124-promogrouplist.md) |
| PAGE-125 | PromoMIOTag | PromoAlloc | Entry/Transaction | Tag MIOs to a promo group | A | [PAGE-125-promomiotag.md](pages/PAGE-125-promomiotag.md) |
| PAGE-126 | DiscountReportViewer | SInventory_RPTVIEW | Report viewer | Special-discount report (rptSpecialDiscount) | B | [PAGE-126-discountreportviewer.md](pages/PAGE-126-discountreportviewer.md) |
| PAGE-127 | ProductCostPriceViewer | SInventory_RPTVIEW | Report viewer | Product cost price report (rptProductCostPrice) | B | [PAGE-127-productcostpriceviewer.md](pages/PAGE-127-productcostpriceviewer.md) |
| PAGE-128 | ProductTreadPriceViewer | SInventory_RPTVIEW | Report viewer | Product trade price / price report (rptProductTradePrice, rptProductPriceReport) | B | [PAGE-128-producttreadpriceviewer.md](pages/PAGE-128-producttreadpriceviewer.md) |
| PAGE-129 | ConditionalPurposeSetting | SInventory_UI | Entry | Stock conditional-purpose setting (tblPurpose <- tblStockCondition); inventory-related, filed under M04 | B | [PAGE-129-conditionalpurposesetting.md](pages/PAGE-129-conditionalpurposesetting.md) |
| PAGE-130 | DiscountSpecialReport | SInventory_UI | Report launcher | Special discount report criteria (date range) -> PAGE-126 | B | [PAGE-130-discountspecialreport.md](pages/PAGE-130-discountspecialreport.md) |
| PAGE-131 | IngridentsEdit | SInventory_UI | Entry (edit) | Ingredient edit (tblIngridents) | B | [PAGE-131-ingridentsedit.md](pages/PAGE-131-ingridentsedit.md) |
| PAGE-132 | IngridentsEntry | SInventory_UI | Entry | Ingredient entry | B | [PAGE-132-ingridentsentry.md](pages/PAGE-132-ingridentsentry.md) |
| PAGE-133 | IngridentsView | SInventory_UI | List | Ingredient list | B | [PAGE-133-ingridentsview.md](pages/PAGE-133-ingridentsview.md) |
| PAGE-134 | ManufacturerEdit | SInventory_UI | Entry (edit) | Manufacturer edit | B | [PAGE-134-manufactureredit.md](pages/PAGE-134-manufactureredit.md) |
| PAGE-135 | ManufacturerEntry | SInventory_UI | Entry | Manufacturer entry (tblManufacturer) | B | [PAGE-135-manufacturerentry.md](pages/PAGE-135-manufacturerentry.md) |
| PAGE-136 | ManufacturerView | SInventory_UI | List | Manufacturer list | B | [PAGE-136-manufacturerview.md](pages/PAGE-136-manufacturerview.md) |
| PAGE-137 | PackSizeEdit | SInventory_UI | Entry (edit) | Pack size edit | B | [PAGE-137-packsizeedit.md](pages/PAGE-137-packsizeedit.md) |
| PAGE-138 | PackSizeEntry | SInventory_UI | Entry | Pack size entry (tblPackSize) | B | [PAGE-138-packsizeentry.md](pages/PAGE-138-packsizeentry.md) |
| PAGE-139 | PackSizeView | SInventory_UI | List | Pack size list | B | [PAGE-139-packsizeview.md](pages/PAGE-139-packsizeview.md) |
| PAGE-140 | ProCategoryEdit | SInventory_UI | Entry (edit) | Product category edit | B | [PAGE-140-procategoryedit.md](pages/PAGE-140-procategoryedit.md) |
| PAGE-141 | ProCategoryEntry | SInventory_UI | Entry | Product category entry (tblProCategory) | B | [PAGE-141-procategoryentry.md](pages/PAGE-141-procategoryentry.md) |
| PAGE-142 | ProCategoryView | SInventory_UI | List | Product category list | B | [PAGE-142-procategoryview.md](pages/PAGE-142-procategoryview.md) |
| PAGE-143 | ProductBonusEntry | SInventory_UI | Entry (dead) | Product bonus entry - save handler commented out, no data path | B | [PAGE-143-productbonusentry.md](pages/PAGE-143-productbonusentry.md) |
| PAGE-144 | ProductCostPriceReport | SInventory_UI | Report launcher | Product cost price report launcher -> PAGE-127 | B | [PAGE-144-productcostpricereport.md](pages/PAGE-144-productcostpricereport.md) |
| PAGE-145 | ProductDiscountEntry | SInventory_UI | Entry (dead) | Product discount entry - save handler commented out, no data path | B | [PAGE-145-productdiscountentry.md](pages/PAGE-145-productdiscountentry.md) |
| PAGE-146 | ProductEdit | SInventory_UI | Entry (edit, legacy pop-up) | Legacy product editor (unreferenced by any live flow) | B | [PAGE-146-productedit.md](pages/PAGE-146-productedit.md) |
| PAGE-147 | ProductEntry | SInventory_UI | Entry/Transaction | Product master create/edit | A | [PAGE-147-productentry.md](pages/PAGE-147-productentry.md) |
| PAGE-148 | ProductPriceReport | SInventory_UI | Report launcher | Product price report launcher -> PAGE-128 | B | [PAGE-148-productpricereport.md](pages/PAGE-148-productpricereport.md) |
| PAGE-149 | ProductSQEdit | SInventory_UI | Entry (edit) | Product brand (ProductSQ) edit | B | [PAGE-149-productsqedit.md](pages/PAGE-149-productsqedit.md) |
| PAGE-150 | ProductSQEntry | SInventory_UI | Entry | Product brand (ProductSQ) entry | B | [PAGE-150-productsqentry.md](pages/PAGE-150-productsqentry.md) |
| PAGE-151 | ProductSQView | SInventory_UI | List | Product brand list | B | [PAGE-151-productsqview.md](pages/PAGE-151-productsqview.md) |
| PAGE-152 | ProductView | SInventory_UI | List | Product master list (tblProduct with lookups) | B | [PAGE-152-productview.md](pages/PAGE-152-productview.md) |
| PAGE-153 | ProTypeEdit | SInventory_UI | Entry (edit) | Product type edit | B | [PAGE-153-protypeedit.md](pages/PAGE-153-protypeedit.md) |
| PAGE-154 | ProTypeEntry | SInventory_UI | Entry | Product type entry (tblProType) | B | [PAGE-154-protypeentry.md](pages/PAGE-154-protypeentry.md) |
| PAGE-155 | ProTypeView | SInventory_UI | List | Product type list | B | [PAGE-155-protypeview.md](pages/PAGE-155-protypeview.md) |
| PAGE-156 | ProUnitPriceEdit | SInventory_UI | Entry (edit, dead) | Unit price pop-up editor (never updates: see PAGE-157 section 31) | B | [PAGE-156-prounitpriceedit.md](pages/PAGE-156-prounitpriceedit.md) |
| PAGE-157 | ProUnitPriceEntry | SInventory_UI | Entry/Transaction | Unit price create / supersede with archive | A | [PAGE-157-prounitpriceentry.md](pages/PAGE-157-prounitpriceentry.md) |
| PAGE-158 | ProUnitPriceView | SInventory_UI | List | Active unit price list (TPVat computed), edit hands off to PAGE-157 | B | [PAGE-158-prounitpriceview.md](pages/PAGE-158-prounitpriceview.md) |
| PAGE-159 | ShippingCartonSizeEdit | SInventory_UI | Entry (edit) | Shipping carton size edit | B | [PAGE-159-shippingcartonsizeedit.md](pages/PAGE-159-shippingcartonsizeedit.md) |
| PAGE-160 | ShippingCartonSizeEntry | SInventory_UI | Entry | Shipping carton size entry (tblProductCase: PcsPerCase / CaseQty) | B | [PAGE-160-shippingcartonsizeentry.md](pages/PAGE-160-shippingcartonsizeentry.md) |
| PAGE-161 | ShippingCartonSizeView | SInventory_UI | List | Shipping carton size list | B | [PAGE-161-shippingcartonsizeview.md](pages/PAGE-161-shippingcartonsizeview.md) |
| PAGE-162 | StockUOMEdit | SInventory_UI | Entry (edit) | Stock UOM edit | B | [PAGE-162-stockuomedit.md](pages/PAGE-162-stockuomedit.md) |
| PAGE-163 | StockUOMEntry | SInventory_UI | Entry | Stock UOM entry (name only; UOMSAPCode not maintained) | B | [PAGE-163-stockuomentry.md](pages/PAGE-163-stockuomentry.md) |
| PAGE-164 | StockUOMView | SInventory_UI | List | Stock UOM list | B | [PAGE-164-stockuomview.md](pages/PAGE-164-stockuomview.md) |
| PAGE-165 | SupplierInformation | SInventory_UI | Entry | Supplier information entry/edit (tblSupplierInformation) | B | [PAGE-165-supplierinformation.md](pages/PAGE-165-supplierinformation.md) |
| PAGE-166 | SupplierInfoView | SInventory_UI | List | Supplier list | B | [PAGE-166-supplierinfoview.md](pages/PAGE-166-supplierinfoview.md) |

## 3. Campaign page variants (live vs legacy)
Six editors exist for one concept. Live = **CampaignSetup.aspx** (only page linked from `CampaignView.aspx` `:117,:195`; newest code 2025-09-06, aspx 2026-07-15). None of the others has an inbound link (grep of cs/aspx/master); DB menu rows are not in the repo, so residual menu use is `UNKNOWN`. All are also present in `PrecompiledWeb`. Every variant has the identical breadcrumb "Campaign Setup", so users cannot tell them apart.

| Aspect | CampaignSetup (111, LIVE) | _final (112) | _new (114) | PT (115) | OLd (117) | _Mult (113) |
|---|---|---|---|---|---|---|
| Files dated (cs) | 2025-09-06 | 2023-03-01 | 2023-02-26 | 2024-11-23 | 2024-12-14 | 2025-05-07 |
| Customer type | multi-select, one master per type | single | single | single | single | multi-select |
| Save DAL method | SaveBonusCampaign | **SaveBonusCampaign_final** | SaveBonusCampaign | SaveBonusCampaign | SaveBonusCampaign | SaveBonusCampaign |
| Edit semantics | delete + reinsert (new id) | **in-place update** (`_final` SPs, `_Up` details) | delete + reinsert | delete + reinsert | delete + reinsert | delete + reinsert |
| Category | yes | no | no | yes | yes | yes |
| Trade Policy / FCFS | both | Trade Policy only | Trade Policy only | both | both | both |
| Ratio-wise increment | yes | no | no | yes | yes | yes |
| Payment type COD/NCOD/Both | **persisted** | no | no | radio shown, **unbound** | no | persisted |
| Multiple product add | no | **persisted** (modal grid) | UI stub, not persisted | no | no | no (multi-select ListBox, broken) |
| Manual ration bands | no | **persisted** | no | no | no | no |
| Market/customer sections | **hidden** (`Visible=False`) | visible | visible | visible | visible | visible |
| Session map id 4 call | yes | no | no | no (null) | no (null) | yes |
| Success e-mail | built, send commented out | **sends** | **sends** | **sends** | **sends** | send commented out |
| Outer try/catch on save | yes | yes | yes | yes | yes | **no** |
| Status | LIVE | legacy, unique features | legacy oldest | legacy prototype | legacy twin of PT (code byte-identical) | abandoned experiment |

Key implication: because market/customer entry is hidden on the live page while its validation still requires one of them (BR-M04-004), creating a *new* campaign there is blocked as deployed (INFERRED; confirm at runtime).

## 4. Business rules index (BR-M04)
| BR | One-liner | Page |
|---|---|---|
| 001 | Campaign header mandatory fields (name, customer type, campaign type, category, product line, dates) | 111 |
| 002 | Campaign type 1/2/3 decides mandatory trigger inputs (product+qty / product+min+max / min+max) | 111 |
| 003 | >= 1 offer row | 111 |
| 004 | >= 1 market or customer row (unsatisfiable on live page for new records) | 111 |
| 005 | Offer type to column mapping (percent / amount / qty) per campaign type | 111 |
| 006 | Offer-row entry validation | 111 |
| 007 | Detail DTO mapping (Quantity, BonusQuantity, DiscountPercentage, QuantityDteail) | 111 |
| 008 | One campaign master per selected customer type | 111 |
| 009 | Payment type COD/NCOD/Both -> IsPTforCOD/IsPTforOther | 111 |
| 010 | Trade Policy vs FCFS exclusive on client only | 111 |
| 011 | Ratio-wise increment flag per detail with bonus product | 111 |
| 012 | Edit = delete old master + insert new id | 111 |
| 013 | Session map id 4 passed to sp_Update_BonusCampaignpkCampaignSetupId | 111 |
| 014 | Save atomic per call; per-type loop not atomic overall | 111 |
| 015 | No date-order/overlap/duplicate validation | 111 |
| 016 | E-mail notification (disabled on live page); hard-coded SMTP secret | 111 |
| 017 | Edit-load customer-type restore (CustomerTypeIdNew list) | 111 |
| 018 | Campaign list filter (today's validity unless status chosen) | 116 |
| 019 | List de-duplicates by name+type (keeps last row) | 116 |
| 020 | New Entry hidden for EmpInfoId 496 | 116 |
| 021 | Sync Now -> sp_CampaignUpdateFromPage | 116 |
| 022 | _final: in-place update, children re-inserted | 112 |
| 023 | _final: multiple-product vs manual-ration exclusive | 112 |
| 024 | _final: manual ration bands seeded/persisted, unvalidated | 112 |
| 025 | _final: multiple product rows | 112 |
| 026 | _final: category/FCFS/ratio/payment not persisted | 112 |
| 027 | _final: real SMTP send | 112 |
| 028 | _new: multi-product UI is a non-persistent stub | 114 |
| 029 | PT/OLd identical code; payment flags null; PT radio unbound | 115, 117 |
| 030 | _Mult: multi-select product not implemented, offer product list empty | 113 |
| 031 | Quoted price header required fields | 118 |
| 032 | Quoted price customer rule (create list / edit single) | 118 |
| 033 | >=1 product ticked with final amount | 118 |
| 034 | Create fans out one master per customer; cumulative detail-list bug | 118 |
| 035 | Quoted price arithmetic (see section 7) | 118 |
| 036 | UnitPrice=final amount, Vat=discount % storage | 118 |
| 037 | Edit re-saves details without visible delete | 118 |
| 040 | Product create mandatory fields | 147 |
| 041 | Product code uniqueness (create only) | 147 |
| 042 | Update omits PackSizeId/CaseId, no validation | 147 |
| 043 | Image upload client-only type check | 147 |
| 044 | Product group 1 selects all DCs; other value clears | 147 |
| 045 | UOM list needs UOMSAPCode (never set by app) | 147 |
| 046 | DC mapping replaced wholesale; separate transaction | 147 |
| 047 | ProductId = MAX+1 | 147 |
| 048 | No inactivation guard | 147 |
| 050 | Price fields required, no VAT formula or range checks | 157 |
| 051 | One price row per product on create | 157 |
| 052 | Price replace flow via edit + Inactive | 157 |
| 053 | Old price archived to tblUnitPriceRollback and deleted; two transactions | 157 |
| 054 | Archive never displayed | 157 |
| 055 | TPVat = UnitPrice + VATAmountPerUnit; downstream lookups assume one row | 157 |
| 056 | Edit mode via Session["UnitPrice"], cleared on load | 157 |
| 058 | MIO can be in one promo group (SP check on tick only) | 125 |
| 059 | Promo MIO tag save requirements; removal depends on SP | 125 |
| 060 | Active MIOs only; promo products ProductGroupId IN (2,3) | 125 |
| 061 | Possible duplicate tag master; dead WebMethod path | 125 |
| 062 | Promo group code = 'PG-' + (1000 + MAX(id) + 1); name unrestricted (PromoGroupDAL.cs:17-40, PAGE-123 Tier B) | 123 |
| 063 | Group-wise promo qty mandatory inputs | 120 |
| 064 | Stock guard checks only first ticked row | 120 |
| 065 | Stock = total central stock, no duplicate check | 120 |
| 066 | Common quantity fill | 120 |
| 067 | Month name vs date representation | 120 |
| 068 | Edit not implemented; Update re-saves | 120 |
| 069 | Dead legacy AJAX path | 120 |
| 070 | Special discount lookup tblProductDiscount by customer+product+date (invoice pages) | WF-06 |
| 071 | Trade-policy tier table chosen by date (13-Dec-2024) and OldTradePolicy flag | WF-06 |
| 072 | Invoice line discount/net formulas | WF-06 |
| 073 | Hard-coded campaign names in order DAL | WF-06 |
| 074 | ProductDiscountEntry / ProductBonusEntry saves disabled | 143, 145 |

## 5. Status lifecycles
- Campaign: `IsActive` flag + `FromDate..Todate`; no transition code; edit replaces the row (live page) - PAGE-111 sec 21.
- Unit price: Active row -> archived (IsActive 0, InActiveDate) in `tblUnitPriceRollback` + new active row - PAGE-157 sec 21.
- Product: `IsActive` bit only; no delete.
- Quoted price / promo tag / promo qty: no status column handled in C#.

## 6. Roles and permissions observed
No page in M04 enforces a role or UserType in code. Access is DB-menu-based (`sp_GET_MainMenuRole2`). Session keys read: `UserId` (EntryBy), `LoginName` (price archive), `EmpInfoId` (CampaignView hides New Entry for "496"), `CampgainMasterMapId` (written by campaign pages). Static `[WebMethod]` endpoints (PromoMIOTag, GroupWisePromoQtyEntry, promo group, therapeutic/product line pages) and `HandlerDocCV.ashx` have no in-code authorisation.

## 7. Calculations (exact)
1. **Quoted price** (PAGE-118, server-side, `decimal`, `Math.Round(x,3)` banker's): TP = list price, P = discount %, A = discount amount. P->price: `A = round(TP*P/100,3)`, `Final = round(TP-(TP-(TP-A)),3) = round(TP - TP*P/100,3)`. Amount->price: `P = round(A/TP*100,3)` (TP=0 -> exception swallowed), `Final = round(TP-A,3)`. Stored: `UnitPrice=Final`, `Vat=P`. No range limits.
2. **Unit price** (PAGE-157): no calculation; cost, unit price, VAT %, VAT amount, MRP typed. List: `TPVat = UnitPrice + VATAmountPerUnit` (SQL).
3. **Campaign** (PAGE-111..117): the pages compute nothing; they store thresholds (`ProductQty`, `Amount`, `MaxAmount`), per-row `DiscountPercentage`, `QuantityDteail` (amount), `BonusQuantity`, ratio flag. Evaluation/rounding of a campaign on an order is **not in this repo**.
4. **Trade policy and special discount at invoicing** (cross-module; WF-M04-06): `percentage` = `DiscountPerc` of the tier row matching the invoice total; `totalDiscount = percentage * total / 100`; line `dpAmt = tp * (percentage/100)` (F2 string), line special discount `sd = tp * (productDiscount%/100)`; `net = tp - dpAmt - sd + tpVat`. Precedence: fixed customers get 0% trade policy; product offer "Esomium20" skips trade policy; special discount (`tblProductDiscount`) is applied *in addition to* trade policy, not instead of it; campaign discount is carried on the order line (`Campaign`, `DiscountPercentage`) and looked up by campaign name.
5. **Promo stock guard** (PAGE-120): `stock < qty(first ticked row)` - defect BR-M04-064.

## 8. Data model
| Table | Role |
|---|---|
| tblProduct, tblProductDCDetails | product master, DC availability |
| tblProCategory, tblProType, tblProductSQ, tblManufacturer, tblPackSize, tblStockUOM, tblProductCase, tblIngridents, tblGenericGroup, tblTherapeuticGroup, tblProductGroup, tblSupplierInformation | classification masters |
| tblUnitPrice, tblUnitPriceRollback (DDL in `SP_Create_tblUnitPriceRollback.sql`), tblUnitPriceUpdate (legacy) | prices |
| tbl_BonusCampaignNewMaster / Detail (+ market, customer, map, manual-ration, multi-product child tables, names UNKNOWN) | campaigns |
| Quoted price master/detail (names UNKNOWN, SP-only) | quoted prices |
| tblProductDiscount, tblTradePolicy / tblTradePolicyNew / tblTradePolicyNew2 | discount / trade-policy inputs read at invoicing |
| tblPromoGroup, tblPromoMIOTagMaster/Detail, tblGroupWisePromoQty | promo allocation |
SPs that matter: sp_Save/Update/Del_BonusCampaign*, sp_GET_Campaign*, sp_Get_BonusCampaignNewMasterList, sp_CampaignUpdateFromPage, sp_Save_QuotedPrice*, sp_Save_ProductDCDetails, sp_Save_PromoMIOTag*, sp_check_Vali_PromoMIOTag, sp_Save_GroupWisePromoQty, generic `ExecuteAllSqlQueryByStoreProcedure` (drop-down loader). All bodies UNKNOWN.

## 9. Integrations
SMTP (Gmail) mail on campaign save (active in 4 legacy pages, disabled in live); mobile sync via `sp_CampaignUpdateFromPage` (INFERRED); SAP: `UOMSAPCode` on UOM feeds SAP challan SP; campaign engine / mobile order capture outside repo.

## 10. Reports
See `reports/README.md` (RPT-M04-01 price reports, RPT-M04-02 special discount report).

## 11. Workflows
- [WF-M04-01 Campaign setup and maintenance](workflows/WF-M04-01-campaign-setup-and-maintenance.md)
- [WF-M04-02 Quoted price setup](workflows/WF-M04-02-quoted-price-setup.md)
- [WF-M04-03 Unit price change with archive](workflows/WF-M04-03-unit-price-change-with-archive.md)
- [WF-M04-04 Product creation and DC availability](workflows/WF-M04-04-product-creation-dc-availability.md)
- [WF-M04-05 Promo group tagging and quantity allocation](workflows/WF-M04-05-promo-group-tagging-and-quantity-allocation.md)
- [WF-M04-06 Price, trade policy and discount application at invoicing (cross-module)](workflows/WF-M04-06-price-discount-application-at-invoicing.md)

## 12. Known gaps summary
See `GAPS-M04.md` (33 rows). Highlights: new campaigns blocked on the live page; hard-coded SMTP credential and SQL login in source; unrestricted file upload; campaign edit replaces ids; two-transaction price swap; stock guard defect; product update ignores pack size/carton; campaign SPs not in repo; five legacy campaign pages still deployed; SQL injection surfaces in list filter and promo inline SQL.

## 13. Still unverified
All SP bodies; runtime behaviour of campaign creation (BR-M04-004); which menu rows point to legacy campaign pages; how/where campaigns are evaluated on orders and precedence versus trade policy at order time; downstream use of quoted prices; Tier-B pages (masters, lists, report launchers) are not semantically reviewed.
