# WF-M03-06 — Territory-wise depot mapping

Traced from `MasterSetup_UI/TerritoryWiseDepotSetup.aspx(.cs)` (PAGE-064, detail there), `TerritoryWiseDepotSetupDAL.cs`, `DepotWiseAreaSetupDal.cs`. SP bodies absent.

**Trigger/actors:** distribution admin assigning territories to a depot (DC) / sub-depot.
**Preconditions:** area with depot-eligible active territories (`sp_CS_GetTerritory_ByAreaId_ActiveForDepo`); depots for company 1 (`sp_GET_DepotByCompanyId`).

**Steps**
1. Pick Group/Zone/Area (structure control) → Search → territory checklist (BR-M03-041, 043).
2. Pick Depot → sub-depots loaded; existing mapping for (DC, SubDepot "0") fetched via `sp_GET_lDcWiseTerritoryDetail_ByAreaId`, boxes pre-ticked, master id captured (BR-M03-044).
3. Untick a territory → `sp_check_Vali_MarketStructure(@PageName='TerritoryWiseDepotSetupUncheck')`; rows ⇒ re-ticked, "Order Pending!" (BR-M03-045).
4. Submit → need Area, Depot, ≥1 territory (BR-M03-041, 042) → `SaveBonusCampaign`: master insert/update (`sp_Save/Update_DcWiseTerritoryMaster`) + `sp_Save_DcWiseTerritoryDetail` per ticked territory, one transaction (BR-M03-046, 101).

**Data changed:** depot-territory master/detail (UNKNOWN tables). **Status:** none. **Exceptions:** ≥10 territories mis-indexed (BR-M03-047); sub-depot choice doesn't reload mapping; removed territories not deleted in C#.
**Tests:** untick territory with pending orders; 12-territory list; new vs existing mapping; missing depot.
