# WF-M04-01 Campaign setup and maintenance

**Trigger / actors:** promotion administrator opens `CampaignView.aspx`. **Preconditions:** logged in; menu access; product lines, campaign types, offer types, customer types seeded in DB.

## Steps
1. `CampaignView.aspx` (PAGE-116) `LoadData` -> `BonusCampaignNewDAL.GetBonusCampaignList(@Parm)` -> `sp_Get_BonusCampaignNewMasterList`; default = campaigns valid today (BR-M04-018); duplicates by name+type collapsed (BR-M04-019).
2. New Entry -> `CampaignSetup.aspx`; Edit -> `CampaignSetup.aspx?MID=id` (PAGE-111).
3. `GetOneRecord` (edit): `sp_GET_CampaignMaster_ById`, `..Detail_ById`, `..DetailMarket_ById`, `..DetailCustomer_ById`.
4. User sets header, campaign type (1/2/3 reveals trigger inputs), offer type; "Add to list" -> `ValiDeltList` -> `AddOffer` (BR-M04-005/006). Market/customer rows: UI hidden on live page (BR-M04-004).
5. Submit -> `Validation()` (BR-M04-001..004) -> session map id lookup (BR-M04-013) -> per selected customer type `BonusCampaignNewDAL.SaveBonusCampaign`:
   - edit: `sp_Del_BonusCampaignNewMaster(oldId)` (BR-M04-012)
   - `sp_Save_BonusCampaignNewMaster` -> new PK; `sp_Update_BonusCampaignpkCampaignSetupId`; `sp_Save_BonusCampaignNewDetail` per offer; `sp_Save_BonusCampaignMarketDetail` per market; `sp_Save_BonusCampaignCustomerDetail` per customer. One transaction per call, commit on close, rollback on exception (BR-M04-014).
6. Mail built, send disabled (BR-M04-016); success alert -> `CampaignView.aspx`.
7. Optional: "Sync Now" -> `sp_CampaignUpdateFromPage` (BR-M04-021).

## Data changed
`tbl_BonusCampaignNewMaster/Detail`, market/customer/map child tables (names UNKNOWN). Lifecycle: `IsActive` + date window; replaced on edit.

## Branches / exceptions
Validation failure -> tooltip/alert, no write. SP exception -> rollback, alert with exception text. Multi-type failure mid-loop leaves earlier types committed. Legacy variants (PAGE-112..115,117) follow the same flow with differences in README section 3; `_final` updates in place and persists multi-product/manual-ration rows (BR-M04-022..026).

## Postconditions / related
Campaign row(s) exist, consumed by invoice/order logic (WF-M04-06). BRs 001-021. Pages 111-117. **Tests:** create type-1 campaign (expect BR-004 block on live page); edit and confirm new id; 3 customer types -> 3 masters, 1 list row; date inversion accepted.
