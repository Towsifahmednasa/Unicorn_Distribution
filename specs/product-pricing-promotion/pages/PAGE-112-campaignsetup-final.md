---
page_id: PAGE-112
module: M04
page: CampaignSetup_final
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-112 - CampaignSetup_final (2023 generation: multiple-product and manual-ratio campaigns; NOT linked)

> **Evidence level:** Tier A. Read: full `CampaignSetup_final.aspx.cs` (2648 physical lines; read as a line-by-line diff against `CampaignSetup_new.aspx.cs` plus full reads of the changed regions), key regions of the aspx (modals, checkboxes, grids), `BonusCampaignNewDAL.SaveBonusCampaign_final` (`BonusCampaignNewDAL.cs:541-737`). Shared behaviour (header validation, offer mapping, draft grids) is identical to PAGE-111 and is not repeated; see PAGE-111 and README "Campaign page variants". SP bodies absent (UNKNOWN).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-112 |
| Module | M04 |
| Page name | CampaignSetup_final |
| Kind | Entry / Transaction |
| Web folder / master | MasterSetup_UI / ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_CampaignSetup_final |
| Files | aspx 1234 lines (last modified 2023-03-01), aspx.cs 2648 lines (2023-03-01) |
| Live/legacy verdict | **LEGACY / SUPERSEDED BUT DEPLOYED.** No inbound link from any page (the only outbound-to-list flow is its own success redirect to `CampaignView.aspx`, whose Edit/New buttons target `CampaignSetup.aspx`). It is the **only** variant that persists "Multiple Product Add" and "Manual Ration Setup" campaign structures - those features are absent from the newer live page, so campaigns created here (if any menu row still points to it) cannot be edited on PAGE-111. Menu use UNKNOWN. INFERRED. |

## 2. Purpose
Same as PAGE-111 plus two extra campaign shapes: (a) **Multiple Product Add** - the customer must buy several listed products, each with a quantity; (b) **Manual Ration Setup** - a hand-entered ladder of "from main qty / to main qty / bonus qty" bands for a bonus-quantity offer, instead of a single fixed ratio. CONFIRMED from labels/code.

## 3. Business Objective
Allow finer control of free-goods campaigns (combo purchase; stepped free-quantity slabs) than the single trigger-quantity / bonus-quantity pair. INFERRED from names; downstream engine outside repo.

## 4. Navigation
Inbound: none found (grep of repo). Outbound: `CampaignView.aspx` (success redirect and "Back to List"), `CampaignSetup.aspx` (Reset `:1945`). Menu path UNKNOWN.

## 5. Route
`~/MasterSetup_UI/CampaignSetup_final.aspx[?MID=id]`.

## 6. Source Files
View/code-behind as above; user control IVMarketStructure.ascx (visible on this page); DAL `BonusCampaignNewDAL` (`SaveBonusCampaign_final`, `GetCampaignSetupDetailMulProById`, `GetCampaignSetupDetailMannualRatioById`), `SeedDataDAL`, `QuotedPriceSetupDAL`; DAO `BonusCampaignNewMasterDAO` (+ `ManualRationSetupCampDAO`, `MultipleProductAddCampDAO`).

## 7. UI Control Inventory
Deltas versus PAGE-111 (all others as PAGE-111 unless stated):

| Control | Label | Type | Required / rule | Notes |
|---|---|---|---|---|
| ddlChemistType | Customer Type | **single** DropDownList | header required | `aMaster.CustomerTypeId = SelectedIndex>0 ? ... : null` (`:1686`); only one master per save |
| ddlCampaignCategory, rbPaymentOptions, chkFCFS, chkIsRatioWiseIncrement | - | **absent** | - | category/FCFS/ratio/payment not part of this page |
| chkTradePolicy | Trade Policy | CheckBox | optional | in the mutually-exclusive JS group with the two new checkboxes |
| divrbType (visible only for campaign type 1) | - | Row | - | shows chkMultipleProductAdd |
| chkMultipleProductAdd | Multiple Product Add | CheckBox AutoPostBack | requires main product + qty (`Validationfor_multi`) | opens modal `mpe_1` (`pnl_1`) with grid `gv_MultipleProductAdd` (Product ddl per row + Quantity + plus/minus) |
| chkManualRationSetup | Manual Ration Setup | CheckBox AutoPostBack, `Visible=false` until offer type 5 chosen | requires main product, qty and >=1 offer row (`Validationfor_Ratio`) | opens modal `mp_2` (`Panel1`) with grid `gv_ManualRationSetup` (From Main Quantity, To Main Quantity, Bonus Quantity, plus/minus), `lblInfo`, `OferPro`, `hfOferProId` |
| gv_MultipleProductAdd | grid | GridView | rows saved only when flag true | first row remove button hidden |
| gv_ManualRationSetup | grid | GridView | rows saved only when flag true | first row remove button hidden |
| Trade Policy/Multiple/Manual JS | `MutExChkList` (aspx:93) | - | - | client-only exclusivity |

## 8. Field Mapping
Header -> `sp_Save_BonusCampaignNewMaster_final` / `sp_Update_BonusCampaignNewMaster_final` params: CampgainMasterId, CampaignName, CustomerTypeId, CampainTypeId, Amount, ProductQty, MaxAmount, FromDate, Todate, IsActive, IsTradePolicy, ProductLineID, BonusProductId, **IsMultipleProductAdd, IsManualRationSetup**, EntryBy/UpdateBy (`BonusCampaignNewDAL.cs:551-577,615`). **Not sent:** CampaignCategoryId, IsFCFS, IsRatioWiseIncrement(+Pro), IsPTforCOD/Other. Detail rows as PAGE-111 minus `IsRatioWiseIncrementPro`. Manual ratio row -> `sp_Save_ManualRationSetupCampDetail(@CampaignMasterId,@ProductId=header product,@BounsProductId=hfOferProId,@MainQuantity_From,@MainQuantity_ManualRationSetup,@BonusQuantity_ManualRationSetup)`. Multiple product row -> `sp_Save_MultipleProductAddCampCampDetail(@CampaignMasterId,@ProductId,@ProQty_MultipleProductAdd)`.

## 9. Dropdown Mapping
As PAGE-111 except: no category drop-down; `ddlProduct_MultipleProductAdd` per grid row bound from `GetProductForDDL` (ProductId/ProductName).

## 10. Button Action Mapping
`chkMultipleProductAdd_CheckedChanged` (`:2422`): edit mode loads `GetCampaignSetupDetailMulProById` and shows modal; if grid already has rows just shows modal; else `Validationfor_multi()` then shows modal (else unticks and hides). `chkManualRationSetup_CheckedChanged`: edit mode loads `GetCampaignSetupDetailMannualRatioById`, sets `hfOferProId`/labels; else `Validationfor_Ratio()` then `AddManualRationSetup()` seeds the first row. `plus_/rmv_MultipleProductAdd`, `plus_/rmv_ManualRationSetup` add/remove rows; `rmvModalManualRationSetup_Click` closes. `btnSave_Click` -> `SaveBonusCampaign_final`.

## 11. Frontend Flow
As PAGE-111 plus AJAX-toolkit modal popups. Selecting offer type 5 for a type 1 or 2 campaign makes `chkManualRationSetup` visible (`ddlOfferType_SelectedIndexChanged`). On edit-load, if any offer row has `BonusTypeId==5` the offer type is re-selected to 5 and the manual-ratio checkbox shown (`:147-158`, test at `:151`). When neither structure is checked on an existing record both checkboxes are disabled; when one is checked the other is disabled (`:79-97`). `ddlCampaignType` change on a new record clears both flags (`:371-376`).

## 12. Backend Flow
Save (`btnSave_Click`, `:1518-1780`): validation -> single master DTO (no per-type loop) -> `SaveBonusCampaign_final`. DAL: **edit** (`CampgainMasterId>0`): `sp_Update_BonusCampaignNewMaster_final`, then each existing offer row via `sp_Save_BonusCampaignNewDetail_Up`; **new**: `sp_Save_BonusCampaignNewMaster_final` -> PK, offers via `sp_Save_BonusCampaignNewDetail`. Then for both paths (`pk>0`): market rows, customer rows, then `if IsManualRationSetup` ratio rows, `if IsMultipleProductAdd` product rows. One transaction (DataAccessManager) - rollback on exception. Success mail code is **active** (`smtp.Send(mail)` `:1769`) with hard-coded SMTP credentials.

## 13. API Mapping
None page-local; `GetCustomer_New` web service for customer picker (visible here).

## 14. Service Mapping
No BLL; DAL direct (static instance shared, see GAP-M04-013).

## 15. Repository Mapping
`BonusCampaignNewDAL.SaveBonusCampaign_final`, `GetCampaignSetupDetailMulProById`, `GetCampaignSetupDetailMannualRatioById`, plus the common readers.

## 16. Stored Procedure Mapping
sp_Save_BonusCampaignNewMaster_final, sp_Update_BonusCampaignNewMaster_final, sp_Save_BonusCampaignNewDetail, sp_Save_BonusCampaignNewDetail_Up, sp_Save_BonusCampaignMarketDetail, sp_Save_BonusCampaignCustomerDetail, sp_Save_ManualRationSetupCampDetail, sp_Save_MultipleProductAddCampCampDetail, sp_Get_BonusCampaigndtlList (called with id 0 and result unused, `:573-575`), readers sp_GET_CampaignDetailMulPro_ById, sp_GET_CampaignDetailMaanualRatio_ById and the common ones. All bodies UNKNOWN.

## 17. Database Mapping
Master/Detail tables as PAGE-111; new child tables implied by SP names (manual-ratio detail, multiple-product detail) - names and columns UNKNOWN.

## 18. Validation Matrix
Header validation as PAGE-111 (`:1401-1520`) except the missing category rule; notable differences: empty offer list and empty market/customer produce a popup via `showpop6('please Add to List Product Offer!')` / `'please Add to List Market Structure or Customer!'` (no alert); market/customer UI is visible so the rule is satisfiable. New: both `chkManualRationSetup` and `chkMultipleProductAdd` ticked => popup "please select Multiple Product Add or Manual Ration Setup!" (BR-M04-023). `Validationfor_multi`: main product + qty. `Validationfor_Ratio`: main product + qty + >=1 offer row ("Add to List Offer Product!"). **No validation of the ladder bands** (overlap, from<=to, bonus>0) and no check that grids are non-empty when the flag is set.

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-022 | **Update is in-place**, not replace: `sp_Update_BonusCampaignNewMaster_final` then `sp_Save_BonusCampaignNewDetail_Up` per offer row (row keyed by `CampaignDetailId`); market and customer rows are inserted again through the same `Save` SPs on every update without a preceding delete visible in C#, so duplicate market/customer rows are possible depending on the SPs (UNKNOWN). Rows added during edit carry `CampaignDetailId = NULL` into the `_Up` SP (behaviour UNKNOWN). | BonusCampaignNewDAL.cs:570-612,647-678 | `SaveBonusCampaign_final` | CONFIRMED (calls) / UNKNOWN (SP effect) |
| BR-M04-023 | Multiple Product Add and Manual Ration Setup are mutually exclusive: JS `MutExChkList` on the client; server check on save rejects both true; on an existing record the other box is disabled. | CampaignSetup_final.aspx:93-100; .cs:79-97,1505-1512 (msg :1508) | JS + `Validation()` | CONFIRMED |
| BR-M04-024 | Manual Ration Setup is offered only for offer type 5 (bonus quantity) of campaign type 1 or 2; the ladder's first row is seeded with `From = To = header Quantity` and `Bonus = the (last) offer row's Qty`; the offer product of the **last** offer row is stored as the bonus product (`hfOferProId`); rows saved only if `IsManualRationSetup==true`. Bands are free text numbers (`0-9.`), unvalidated. Manual rows are keyed on header product + the single offer product. | CampaignSetup_final.aspx.cs:492,2221-2300,1623-1642; BonusCampaignNewDAL.cs:680-700 | `AddManualRationSetup`,`btnSave_Click` | CONFIRMED |
| BR-M04-025 | Multiple Product Add is offered only for campaign type 1 (`divrbType`) and needs a main product + trigger quantity before the modal opens; each modal row = product + quantity; rows saved only if `IsMultipleProductAdd==true`; the first row cannot be removed; each row's drop-down is rebound from the full product list on every postback. Products are not de-duplicated and a row with no product saves `ProductId=NULL`. | CampaignSetup_final.aspx.cs:1968-2060,2129-2200,2586-2613; BonusCampaignNewDAL.cs:702-716 | `chkMultipleProductAdd_CheckedChanged`,`Validationfor_multi` | CONFIRMED |
| BR-M04-026 | Master flags `IsMultipleProductAdd/IsManualRationSetup` are persisted and restored on edit; category, FCFS, ratio-wise-increment and payment-type flags of the live page are **not persisted** by this variant, so a campaign saved here loses those properties. | BonusCampaignNewDAL.cs:551-568 (parameter list) | `SaveBonusCampaign_final` | CONFIRMED |
| BR-M04-027 | Success e-mail is really sent from this variant (`smtp.Send(mail)` active) to the developer's address with the Bcc; hard-coded credential in code. | CampaignSetup_final.aspx.cs:1718-1775 | `btnSave_Click` | CONFIRMED |

Shared rules that apply unchanged: BR-M04-001..007 (with market/customer visible so BR-M04-004 is satisfiable), BR-M04-014 (atomic), BR-M04-015.

## 20. Permission Matrix
Session `UserId` only. No role check. Menu-based. UNKNOWN roles.

## 21. Status Lifecycle
`IsActive` flag + date window as PAGE-111; in-place update keeps the same `CampgainMasterId` across edits (BR-M04-022) - the only variant where campaign ids are stable.

## 22. Error Handling
Outer try/catch shows `faildalert(ex.Message)`; many silent `catch (Exception ex) { }` in modal binding loops.

## 23. Audit
`@EntryBy` / `@UpdateBy` = `Session["UserId"]` (update path sets UpdateBy). Column names UNKNOWN.

## 24. Integration
Active SMTP send (BR-M04-027). No other.

## 25. Reports
None.

## 26. Dependencies
As PAGE-111 + AjaxControlToolkit ModalPopupExtender, DAO `ManualRationSetupCampDAO`, `MultipleProductAddCampDAO`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Type 1 campaign, tick Multiple Product Add with no main product | Product tooltip; box unticked | BR-M04-025 |
| TC-02 | Tick both boxes (bypass JS) and submit | popup "please select Multiple Product Add or Manual Ration Setup!" | BR-M04-023 |
| TC-03 | Offer type 5, tick Manual Ration | Modal opens with first row From=To=qty, Bonus=offer qty | BR-M04-024 |
| TC-04 | Edit existing campaign and save | Same `CampgainMasterId`, offers updated, market rows possibly duplicated (verify in DB) | BR-M04-022 |
| TC-05 | Save with manual ladder rows overlapping (1-10, 5-20) | Saved unvalidated | BR-M04-024 |
| TC-06 | Save then read `tbl` for category/FCFS | not stored | BR-M04-026 |
| TC-07 | Success save | mail attempted via Gmail SMTP (network needed) | BR-M04-027 |
| TC-08 | Multiple product row without product | ProductId NULL persisted | BR-M04-025 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/CampaignSetup_final.aspx(.cs)`, `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:541-737`, `Library.DAO/DoctorModule_DAO/BonusCampaignNewMasterDAO.cs`. Diff basis: `CampaignSetup_new.aspx.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | SP bodies absent | MISSING | - |
| Feature parity with live page | None existed | lacks category, FCFS, ratio, payment | UNKNOWN | DIVERGENT | BR-M04-026 |
| Reachability | None existed | no inbound link | menu UNKNOWN | UNKNOWN | section 1 |

## 30. Known Gaps
Only variant with multi-product/manual-ratio (feature orphaned, GAP-M04-006/007). Real SMTP send with embedded credential (GAP-M04-003). Update duplicates children (GAP-M04-007). Ratio bands unvalidated. `sp_Get_BonusCampaigndtlList` called with 0 for nothing. Dead commented blocks (`:178-217`).

## 31. Change Impact
`SaveBonusCampaign_final` and the two child SPs are used only here. Sharing with the other variants: common readers `GetCampaignSetup*ById`, `SeedDataDAL`. Removing this page would strand any existing multi-product/manual-ratio campaigns for editing.
