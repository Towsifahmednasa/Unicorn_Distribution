# WF-M04-04 Product creation / edit and DC availability

**Trigger / actors:** catalogue administrator, from `ProductView.aspx` (PAGE-152). **Preconditions:** classification masters exist (category, manufacturer, pack size, UOM with SAP code, type, brand, carton, generic/therapeutic/product group, product line) - created on their Entry pages (PAGE-103..165) and refreshed with the per-drop-down refresh buttons.

## Steps (PAGE-147)
1. Fill required fields (BR-M04-040); optional image (JS check, POST to `HandlerDocCV.ashx`, BR-M04-043); select DCs (`ddlDistributionCenter`); Product Group 1 selects all DCs (BR-M04-044).
2. Submit -> `ProductBLL.SaveProduct`: `HasProductName` by ProductCode (BR-M04-041) -> `ProductId = MAX+1` -> inline `INSERT tblProduct` (own connection, auto-commit) (BR-M04-047).
3. `CustomerInfoDAL.SaveProductDist`: `sp_Delete_ProductDCDetails` then `sp_Save_ProductDCDetails` per DC in one transaction (BR-M04-046).
4. Redirect `ProductView.aspx`.
Edit (`?ID`): `ProductEditLoad` (product + DC list), `UpdateProduct` inline UPDATE (no PackSizeId/CaseId, no validation, no uniqueness, BR-M04-042) then DC replace.

## Branches / postconditions
Duplicate code -> "Already Exist!"; DC failure leaves product without DCs; inactivation only via IsActive (BR-M04-048). Product then needs a price (WF-M04-03). **Tests:** duplicate code, pack-size edit ignored, group 1 all-DC, non-image upload accepted by handler.
