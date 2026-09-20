# WF-M04-05 Promo group, MIO tagging and quantity allocation

**Trigger / actors:** promotion/warehouse administrator. **Preconditions:** active MIOs (`tblMIOInfo`), promo products (ProductGroupId 2 or 3), central stock (`tblCentralStore`).

## Steps
1. `PromoGroup.aspx` (PAGE-123): create group via WebMethod `SaveGroup` -> inline INSERT `tblPromoGroup` with code `'PG-'+(1000+MAX(id)+1)`, name concatenated into SQL (BR-M04-062).
2. `PromoMIOTag.aspx` (PAGE-125): choose group -> `LoadMIO` (active MIOs not in group + members pre-ticked); tick MIO -> `sp_check_Vali_PromoMIOTag` (BR-M04-058); Save -> `sp_Save/Update_PromoMIOTagMaster` + `sp_Save_PromoMIOTagDetail` per ticked MIO (BR-M04-059, 061).
3. `GroupWisePromoQtyEntry.aspx` (PAGE-120): choose group -> `LoadMIOByGroupId`; product -> central stock `SUM(tblCentralStore.Quantity)`; month/year; common or individual qty (BR-M04-066); Save -> `Validation` (BR-M04-063/064) -> `sp_Save_GroupWisePromoQty` per ticked MIO (one transaction).
4. `GroupWisePromoQtyView.aspx` (PAGE-122): list/inline edit qty via `sp_Update_PromoEmployeeQty` (BR-M04-068).

## Data / exceptions
`tblPromoGroup`, `tblPromoMIOTagMaster/Detail`, `tblGroupWisePromoQty` + SP-side per-MIO table (UNKNOWN). Stock guard checks only first ticked row (BR-M04-064); no duplicate control (BR-M04-065). Possible duplicate tag master (BR-M04-061).

**Related:** BRs 058-069, GAP-M04-025..027. **Tests:** tick MIO of another group; 5 MIOs x 50 vs stock 100; open `?MID` and Update; untick member and save.
