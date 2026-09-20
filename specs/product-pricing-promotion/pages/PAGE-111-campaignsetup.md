---
page_id: PAGE-111
module: M04
page: CampaignSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "PARTIAL", "database": "PARTIAL", "rules": "PARTIAL", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-111 - CampaignSetup (LIVE campaign editor)

> **Evidence level:** Tier A. Read in full: `Solution.Web/MasterSetup_UI/CampaignSetup.aspx` (945 lines), `CampaignSetup.aspx.cs` (2006 lines), `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs`, `Library.DAL/DataManager/DataAccessManager.cs` (transaction semantics), `Library.DAO/DoctorModule_DAO/BonusCampaignNewMasterDAO.cs`. **No campaign stored-procedure body exists in the repository**, so every SP-side effect is `UNKNOWN`. This page is the reference implementation for the campaign family; the five sibling pages (PAGE-112..115, 117) document only their deltas and link back here. Variant comparison: see `specs/product-pricing-promotion/README.md` section "Campaign page variants".

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-111 |
| Module | M04 - Product, Pricing & Promotion Master |
| Page name | CampaignSetup |
| Kind | Entry / Transaction (create + edit of a bonus/discount campaign) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_CampaignSetup |
| Code-behind | 2006 physical lines (about 1425 non-blank) |
| Live/legacy verdict | **LIVE** - the only campaign entry page linked from `CampaignView.aspx` (`CampaignView.aspx.cs:117` New Entry, `:195` Edit `CampaignSetup.aspx?MID=`). Newest code-behind (2025-09-06) and aspx (2026-07-15) of the family. CONFIRMED (references) / INFERRED (that the DB menu also points here; menu rows are not in the repo). |
| Suspected legacy/duplicate | NO (see 5 legacy siblings in README) |

## 2. Purpose
Lets a sales-promotion administrator define a **trade campaign**: which customer type(s) it targets, campaign type and category, product line, optional main product with a trigger quantity or a min/max order amount, validity window, payment mode (COD / NCOD / Both), whether it is the *Trade Policy* or *FCFS* (first come first served) kind, and a list of **offers** (percentage discount, flat amount, or bonus quantity of a product). CONFIRMED from labels and code (`CampaignSetup.aspx.cs:1377-1537`).

## 3. Business Objective
Give the field sales / order-capture process (mobile app, order pages) a maintained table of active offers so that eligible orders receive the configured discount or free goods. The engine that *applies* a campaign to an order is **not in this repository** (see GAPS-M04); only the master data entry is. INFERRED from `tbl_BonusCampaignNewMaster/Detail` consumers (`Library.DAL/SInventory_DAL/OrderInfoDAL.cs:778`) and hard-coded campaign names in `OrderInfoDAL.cs:875,889,903`.

## 4. Navigation
- Menu placement is DB-driven (`sp_GET_MainMenuRole2`); exact path UNKNOWN.
- Inbound: `CampaignView.aspx` ("New Entry" `EmpCetegoryAddImageButton_Click` -> `CampaignSetup.aspx`; row Edit -> `CampaignSetup.aspx?MID=<id>`), the page's own Reset (`Unnamed_Click`, `:2002-2005`).
- Outbound: on success redirects (via `successalert(...,'CampaignView.aspx')`) to the list; "Back to List" link `CampaignView.aspx` (aspx:92).

## 5. Route
`~/MasterSetup_UI/CampaignSetup.aspx` - file route, optional `?MID=<CampgainMasterId>` switches to edit mode (`Page_Load` `:37-43`).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/CampaignSetup.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/CampaignSetup.aspx.cs |
| User control | Solution.Web/MasterSetup_UI/IVMarketStructure.ascx (market cascade; `Visible="False"` on this page, aspx:676) |
| DAL (direct from page, no BLL) | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs, Library.DAL/MasterSetup_DAL/QuotedPriceSetupDAL.cs (`GetProductListActiveForALl`), Library.DAL/DoctorModule_DAL/SeedDataDAL.cs (drop-down seeds) |
| DAO | Library.DAO/DoctorModule_DAO/BonusCampaignNewMasterDAO.cs (Master, Detail, CampaignCustomerDetailDAO), Library.DAO/MasterSetup_DAO/BonusCampaignMarketDetailDAO.cs, Library.DAO/SInventory_Entities/ResultInfo.cs |
| Web service | Solution.Web/App_Code/SInventoryWebService.cs `GetCustomer_New` (customer auto-complete, currently hidden) |

## 7. UI Control Inventory
Required flag = enforced by server-side `Validation()` (`:1357-1537`); the red `*` in the markup is cosmetic. No ASP validators are used.

| Control (ID) | Label | Type | Required | Default / visibility | Data source | Error / behaviour |
|---|---|---|---|---|---|---|
| id_mastetID | - | HiddenField | - | set from `?MID` | QueryString | drives create vs edit (`Convert.ToInt32` at `:1684`) |
| txtCampaignName | Campaign Name | TextBox | YES (`:1377`) | empty | user | CSS `is-invalid` + tooltip "please fill out this field" |
| ddlChemistType | Customer Type | **ListBox multiple** (select2) | YES (`:1386`, `SelectedValue==""`) | none selected | `SeedDataDAL.GetChemistTypeList` -> `sp_GET_CustomerType` (`CustomerTypeId`,`CustomerType`) | one campaign master is saved **per selected type** (BR-M04-008) |
| ddlCampaignType | Campaign Type | DropDownList AutoPostBack | YES (`:1393`) | "Please Select From List" | `sp_GET_CampaignType` (`CampainTypeId`,`TypeName`) | postback resets qty/amount/offers (`:403-466`) |
| ddlCampaignCategory | Campaign Category | DropDownList | YES (`:1401`) | placeholder | `sp_GET_CampaignCategory` | - |
| rbPaymentOptions | Payment Type | RadioButtonList COD/NCOD/Both | no server check | new: "Both" preselected (`:46`) | static | mapped in BR-M04-009 |
| ddlProLine | Product Line | DropDownList | YES (`:1410`) | placeholder | `sp_GET_ProductLine` (`ProductLineID`,`LineName`) | - |
| divProduct / ddlProduct | Product (main trigger product) | DropDownList | YES when campaign type 1 or 2 (`:1443-1471`) | hidden until type 1/2 | `BonusCampaignNewDAL.GetProductForDDL` -> `sp_Get_Product_ForTargetSetup_DDL` (`ProductId`,`ProductName`) | - |
| DivPrdQty / txtProductQty | Quantity (type 1) / Amount label (type 2 label only, div hidden) | TextBox, FilteredTextBox `0-9.` | YES for type 1 (`:1454`) | hidden | user | - |
| divAmount / txtAmount | Min Amount | TextBox `0-9.` | YES for type 2 and 3 (`:1474`,`:1497`) | hidden until type 2/3 | user | - |
| txtMaxAmount | Max Amount | TextBox `0-9.` | YES for type 2 and 3 (`:1483`,`:1506`) | hidden | user | - |
| chkIsRatioWiseIncrement | Ratio wise Increment | CheckBox (in divAmount) | no | unchecked | - | BR-M04-011 |
| frmDate | Valid From Date | `datetime-local` TextBox | YES (`:1419`, alert "Please Select Valid From Date!") | empty | user | stored with time |
| toDate | Valid To Date | `datetime-local` TextBox (css `datepickess`) | YES (`:1429`, alert "Please Select Valid To Date!") | empty | user | no From<=To check (BR-M04-015) |
| chkTradePolicy / chkFCFS | Trade Policy / FCFS | CheckBox x2 | no | unchecked | - | JS `MutExChkList` makes them mutually exclusive on the client only (aspx:924-933) |
| ddlOfferType | Offer Type | DropDownList AutoPostBack | YES to add a row (`:632`) | placeholder | `SeedDataDAL.GetOfferTypeInfo(campaignTypeId)` -> `sp_Get_OfferTypeInfo` (`BonusTypeId`,`TypeName`) | reveals fields per BR-M04-005 |
| divProductList / ddlProductList | Offer Product | DropDownList | YES for offer types needing a product | hidden | `QuotedPriceSetupDAL.GetProductListActiveForALl` -> `sp_Get_Product_All` | - |
| divDisType / txtQtyList | label set in code (`Amount (%):`, `Amount:`, `Quantity:`) | TextBox `0-9.` | YES (`:660..`) | hidden | user | - |
| bnAddList | Add to list | LinkButton | - | shown with divBtn | - | `ValiDeltList()` then `AddOffer()` |
| gv_ProductOffer | offer grid: SL, Offer Type, Product, Qty, Amount, Percent Amount, Remove | GridView (ViewState) | >=1 row (`:1521`) | empty | in-memory DataTable | hidden fields `hfBonusTypeId`,`CampaignDetailId`,`hfProductId` |
| IVMarketStructure, btnAddtoListMarket, gv_Market | Market Structure | user control + button + grid | see BR-M04-004 | **all `Visible="False"`** (aspx:676-778) | - | cannot be operated by the user |
| divCus / custNameTextBox / btnCustomerAdd / gv_Customer | Customer list | AutoComplete + grid | see BR-M04-004 | **`Visible="False"`** (aspx:786-874) | `SInventoryWebService.GetCustomer_New` | cannot be operated by the user |
| chkIsActive | Active | checkbox (`input runat=server`, markup `checked`) | no | checked | - | -> `IsActive` |
| btnSave / btnUpdate | Submit / Update | LinkButton (`sweetAlertConfirm_*`) | - | exactly one visible: Update when `?MID`, else Submit | - | both call `btnSave_Click` |

## 8. Field Mapping
UI -> DTO (`BonusCampaignNewMasterDAO`) -> SP parameter -> column (columns UNKNOWN; SP bodies not in repo). Source `CampaignSetup.aspx.cs:1682-1742`, `BonusCampaignNewDAL.cs:207-229`.

| UI field | DTO | SP param (`sp_Save_BonusCampaignNewMaster`) | Column |
|---|---|---|---|
| id_mastetID | CampgainMasterId | @CampgainMasterId | tbl_BonusCampaignNewMaster.CampgainMasterId (confirmed table by `OrderInfoDAL.cs:779`) |
| txtCampaignName | CampaignName | @CampaignName | CampaignName (matched by name in `OrderInfoDAL.LoadCampaignDtls`) |
| each selected ddlChemistType item | CustomerTypeId | @CustomerTypeId | UNKNOWN |
| ddlCampaignType | CampainTypeId | @CampainTypeId | UNKNOWN |
| ddlCampaignCategory | CampaignCategoryId | @CampaignCategoryId | UNKNOWN |
| txtAmount | Amount | @Amount | UNKNOWN |
| txtProductQty | ProductQty | @ProductQty | UNKNOWN |
| txtMaxAmount | MaxAmount | @MaxAmount | UNKNOWN |
| frmDate / toDate | FromDate / Todate | @FromDate / @Todate | UNKNOWN (`Convert DateTime.Parse`, time kept) |
| chkIsActive | IsActive | @IsActive | UNKNOWN |
| chkTradePolicy / chkFCFS | IsTradePolicy / IsFCFS | @IsTradePolicy / @IsFCFS | UNKNOWN |
| rbPaymentOptions | IsPTforCOD / IsPTforOther | @IsPTforCOD / @IsPTforOther | UNKNOWN |
| ddlProLine | ProductLineID | @ProductLineID | UNKNOWN |
| ddlProduct | BonusProductId (header) | @BonusProductId | UNKNOWN (naming: header "main product" stored in BonusProductId) |
| chkIsRatioWiseIncrement | IsRatioWiseIncrement | @IsRatioWiseIncrement | UNKNOWN |
| Session["UserId"] | - | @EntryBy | UNKNOWN |

Detail rows (`BonusCampaignNewDetailDAO`, built `:1609-1655`) -> `sp_Save_BonusCampaignNewDetail`: `CampaignMasterId`=new master id; `DiscountPercentage`=lbl_PercentAmount; `ProductId`=**header** ddlProduct; `Quantity`=header txtProductQty; `BonusProductId`=hfProductId (offer product); `BonusQuantity`=lbl_Qty; `QuantityDteail`=lbl_Amount; `BonusTypeId`=hfBonusTypeId; `CampaignName`; `IsRatioWiseIncrementPro`. Market rows -> `sp_Save_BonusCampaignMarketDetail(GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId,MarketId)`. Customer rows -> `sp_Save_BonusCampaignCustomerDetail(CustomerMasterId)`.

## 9. Dropdown Mapping
| Control | Source | Value/Text | Notes |
|---|---|---|---|
| ddlChemistType | sp_GET_CustomerType | CustomerTypeId / CustomerType | blank-item insert `Items.Insert(-1,"")` throws and is swallowed (`:248-253`) - no placeholder in a multi-select |
| ddlProLine | sp_GET_ProductLine | ProductLineID / LineName | placeholder at 0 |
| ddlCampaignType | sp_GET_CampaignType | CampainTypeId / TypeName | values 1,2,3 have code meaning |
| ddlCampaignCategory | sp_GET_CampaignCategory | CampaignCategoryId / CampaignCategory | - |
| ddlProduct | sp_Get_Product_ForTargetSetup_DDL | ProductId / ProductName | index 0 = "none" (`SelectedIndex > 0` at `:1702`) |
| ddlProductList | sp_Get_Product_All | ProductId / ProductName | `ProductId` is `SelectedValue` |
| ddlOfferType | sp_Get_OfferTypeInfo(@id = campaign type) | BonusTypeId / TypeName | ids used in code: 1,2,3,5,6,7 |

## 10. Button Action Mapping
| Button / event | Handler | Effect |
|---|---|---|
| ddlCampaignType change | `ddlCampaignType_SelectedIndexChanged` `:403` | hides/reset fields; type 1 -> product+qty; type 2 -> product+amount(+min/max block); type 3 -> amount block; reloads offer types |
| ddlOfferType change | `ddlOfferType_SelectedIndexChanged` `:468` | shows product/value inputs and label |
| bnAddList | `bnAddList_Click` `:602` | `ValiDeltList` -> `AddOffer` (append row to `gv_ProductOffer`, clear inputs) |
| BtnProductOffer (grid) | `BtnProductOffer_Click` `:861` | `RemoveOffer(rowIndex)` |
| btnAddtoListMarket / deleteImageButton / MarketdeleteImageButton | `:1275`, `:1293`, `:1203` | market list ops (hidden UI) |
| custNameTextBox change / btnCustomerAdd / deleteCustomer | `:1858`, `:1885`, `:1992` | customer list ops (hidden UI); parse `Name : ... \|Id` |
| btnSave / btnUpdate | `btnSave_Click` `:1539` | validate -> loop selected customer types -> `SaveBonusCampaign` -> (disabled) mail -> success/fail alert |
| Reset (`Unnamed_Click`) | `:2002` | `Response.Redirect("CampaignSetup.aspx")` |

## 11. Frontend Flow
- Page init: `pageLoad()` (aspx:149-171) initialises select2 for `.multiple-select` and `.mySelect2`; `$('.datepickess').datepicker("setDate", new Date())` runs once at load (aspx:173-175).
- Confirmation dialogs `sweetAlertConfirm_Submit/Update` (shared script, not in this page) gate the postback; results via `successalert(msg,'Success',url)` / `faildalert`.
- `MutExChkList(chk)` (aspx:924-933): unchecks the other checkboxes in the same container (TradePolicy vs FCFS).
- Everything else is server postback inside one `UpdatePanel` (no AJAX endpoints, no client-side calculation).

## 12. Backend Flow
1. `Page_Load` (`:23-50`): bind IVMarketStructure child controls; on first load `LoadInitialInfo()` (each seed in its own try/catch that **swallows every exception**), then `?MID` => `btnUpdate` visible + `GetOneRecord`, else `rbPaymentOptions[2]` (Both) selected + `btnSave` visible.
2. `GetOneRecord(Id)` (`:52-231`): `sp_GET_CampaignMaster_ById` -> header; if `CustomerTypeIdNew` (comma list) is empty single `CustomerTypeId` is selected else every listed id is selected (`:61-84`); dates rendered `yyyy-MM-ddTHH:mm:ss`; booleans parsed with try/catch defaulting false; payment radio restored: COD only -> item0, other only -> item1, both -> item2 (item0/1 also set - radio list ends with last selected); then detail/market/customer grids from `sp_GET_CampaignDetail_ById`, `sp_GET_CampaignDetailMarket_ById`, `sp_GET_CampaignDetailCustomer_ById`. Whole method wrapped in `catch (Exception ex) { }` (silent).
3. `btnSave_Click` (`:1539-1854`): `Validation()`; `Session["CampgainMasterMapId"]=null`; try `GetCampaignSetupMapById("4")` -> `Session["CampgainMasterMapId"]=BonusCampaignDetailsCustTypeId` (hard-coded id "4"); for **each selected customer type** build Market/Detail/Customer lists and a new master DTO then call `BonusCampaignNewDAL.SaveBonusCampaign(master, details, markets, customers, Session["UserId"])`; Session key cleared; on `isSuccess` builds a MailMessage (send **commented out**, `:1821`), sleeps 100 ms twice, shows "Operation successful!" then redirect to `CampaignView.aspx`; else "Already Exist!" (`:1841`); any exception -> `faildalert(ex.Message)`.
4. `BonusCampaignNewDAL.SaveBonusCampaign` (`:180-374`) - see section 16 for the SP sequence. One `DataAccessManager` instance: first write begins a transaction, `SqlConnectionClose()` commits, `SqlConnectionClose(true)` in `catch` rolls back (`DataAccessManager.cs:122-149`, `:410-414`), so one call = one atomic unit.

## 13. API Mapping
No page-local AJAX/WebMethod. The auto-complete extender calls ASMX `SInventoryWebService.asmx/GetCustomer_New` (`App_Code/SInventoryWebService.cs:145`, parameterised query `tblCustMaster` top 10 by name/code/cell where `IsActive=1`); reachable only when the customer block is visible (it is not on this page).

## 14. Service Mapping
No BLL layer: the page calls `BonusCampaignNewDAL` / `SeedDataDAL` / `QuotedPriceSetupDAL` directly (static instances `:18-20`; DAL objects hold a `DataAccessManager` field, so the static `_BonusCampaignNewDAL` is shared across all requests - concurrent saves share one connection/transaction object; see GAP-M04-013).

## 15. Repository Mapping
`BonusCampaignNewDAL`: `GetProductForDDL`, `GetCampaignSetupById`, `GetCampaignSetupDetailById`, `GetCampaignSetupDetailMarketById`, `GetCampaignSetupDetailCustomerById`, `GetCampaignSetupMapById`, `SaveBonusCampaign`. `SeedDataDAL`: `GetChemistTypeList`, `GetProLineDataTableList`, `GetCampaignTypeList`, `GetCampaignCategory`, `GetOfferTypeInfo`. `QuotedPriceSetupDAL.GetProductListActiveForALl`.

## 16. Stored Procedure Mapping
No body in the repo for any of these (`sp_bodies.json` has none). Sequence in `SaveBonusCampaign` (`BonusCampaignNewDAL.cs`):

| Order | SP | When | Params | Body |
|---|---|---|---|---|
| 1 | sp_Del_BonusCampaignNewMaster | `master.CampgainMasterId > 0` (edit) `:191-196` | @CampgainMasterId | UNKNOWN (soft or hard delete unknown) |
| 2 | (id forced to 0) | edit: `master.CampgainMasterId = 0` `:198` | - | after this line the update branch (`sp_Update_BonusCampaignNewMaster`, `sp_Save_BonusCampaignNewDetail_Up`) is **unreachable** |
| 3 | sp_Save_BonusCampaignNewMaster (ExecuteScalar -> new PK) | always (`:283`) | header params + @EntryBy | UNKNOWN |
| 4 | sp_Update_BonusCampaignpkCampaignSetupId | `pk>0` `:291` | @CampaignMasterId, @CampgainMasterMapId (session), @CustomerTypeId | UNKNOWN |
| 5 | sp_Save_BonusCampaignNewDetail (per offer row) | `:311` | see section 8 | UNKNOWN |
| 6 | sp_Save_BonusCampaignMarketDetail (per market) | `:338` | 6 ids | UNKNOWN |
| 7 | sp_Save_BonusCampaignCustomerDetail (per customer) | `:351` | @CustomerMasterId | UNKNOWN |

Readers: sp_GET_CampaignMaster_ById, sp_GET_CampaignDetail_ById, sp_GET_CampaignDetailMarket_ById, sp_GET_CampaignDetailCustomer_ById, sp_GET_CampaignMasterMap_ById, sp_Get_OfferTypeInfo, sp_GET_CampaignType, sp_GET_CampaignCategory, sp_GET_CustomerType, sp_GET_ProductLine, sp_Get_Product_All, sp_Get_Product_ForTargetSetup_DDL.

## 17. Database Mapping
- Tables confirmed by other repo code: `tbl_BonusCampaignNewMaster` (PK `CampgainMasterId`, `CampaignName`), `tbl_BonusCampaignNewDetail` (`CampaignMasterId`, `DiscountPercentage`) (`OrderInfoDAL.cs:778-780`). Order lines carry `CampaignName`, `IsCampaignProduct` (`Create_Order_ODR-20260901134.sql:69`).
- All other columns / tables (market detail, customer detail, map table) UNKNOWN - SP bodies absent.
- Identity: master PK from `ExecuteScalar` of the SP (`SaveDataReturnPrimaryKey`, `DataAccessManager.cs:475`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| txtCampaignName | required | no | `:1377` | tooltip "please fill out this field" | BR-M04-001 |
| ddlChemistType | at least one | no | `:1386` | same | BR-M04-001 |
| ddlCampaignType / Category / ProLine | required | no | `:1393-1416` | same | BR-M04-001 |
| frmDate | required | no | `:1419` | alert "Please Select Valid From Date!" | BR-M04-001 |
| toDate | required | no | `:1429` | alert "Please Select Valid To Date!" | BR-M04-001 |
| type 1: ddlProduct, txtProductQty | required | no | `:1443-1461` | tooltip | BR-M04-002 |
| type 2: ddlProduct, txtAmount, txtMaxAmount | required | no | `:1463-1491` | tooltip | BR-M04-002 |
| type 3: txtAmount, txtMaxAmount | required | no | `:1493-1514` | tooltip | BR-M04-002 |
| gv_ProductOffer | >=1 row | no | `:1521` | alert "please Add to List Product Offer" | BR-M04-003 |
| gv_Market / gv_Customer | at least one of the two | no | `:1529` | alert "please Add to List Market Structure or Customer!" | BR-M04-004 |
| ddlOfferType / ddlProductList / txtQtyList | per offer type table in BR-M04-006 | no | `:613-853` | tooltip | BR-M04-006 |
| From/To ordering, past dates, numeric range, Amount<=MaxAmount, duplicate campaign name | **not checked** | - | - | - | BR-M04-015 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-001 | Header is mandatory: campaign name, at least one customer type, campaign type, category, product line, valid-from and valid-to date. Validation is server-side only and stops at the first failing field. | CampaignSetup.aspx.cs:1377-1435 | `Validation()` | CONFIRMED |
| BR-M04-002 | Campaign type controls which trigger inputs are mandatory. **Type 1** (quantity trigger): main product + `txtProductQty`. **Type 2** (amount trigger on a product): main product + Min Amount + Max Amount. **Type 3** (order-amount trigger): Min Amount + Max Amount. (Type names come from `sp_GET_CampaignType`, UNKNOWN; the numeric ids 1/2/3 are hard-coded.) | CampaignSetup.aspx.cs:1436-1514, 440-464 | `Validation()`, `ddlCampaignType_SelectedIndexChanged` | CONFIRMED |
| BR-M04-003 | At least one offer row must exist in `gv_ProductOffer` before saving. | CampaignSetup.aspx.cs:1521 | `Validation()` | CONFIRMED |
| BR-M04-004 | At least one market row **or** customer row must exist (both grids empty => "please Add to List Market Structure or Customer!"). On this live page the market/customer UI is `Visible="False"` (aspx:676-778,786-874) and nothing else fills the grids on a new record, so the check can only pass in edit mode where `GetOneRecord` pre-loads rows. **Effectively a new campaign cannot be created from this page as deployed** - INFERRED from static analysis, needs runtime confirmation (GAP-M04-001). | CampaignSetup.aspx.cs:1529, 213-225; CampaignSetup.aspx:676,679,696,786,834 | `Validation()` | INFERRED |
| BR-M04-005 | Offer-type to stored column mapping (`AddOffer`). Campaign type 1 or 2: offer 1 -> **Percent** (`DiscountPercentage`), offer 2 -> **Amount** (`QuantityDteail`), offer 3 -> Amount, offer 5 -> **Qty** (`BonusQuantity`), each with an offer product. Campaign type 3: offer 1 -> **Amount** with product (label says "Amount (%)" but it is stored in the Amount column), offer 6 -> Percent with no product, offer 7 -> Amount with no product, offers 2/3 -> Amount with product, offer 5 -> Qty with product. Offer names are DB data (`sp_Get_OfferTypeInfo`); the semantics above are derived from UI labels, INFERRED (5 = bonus/free quantity, 1/6 = discount percent, 2/3/7 = flat amount). | CampaignSetup.aspx.cs:468-598, 935-1069 | `ddlOfferType_SelectedIndexChanged`, `AddOffer` | CONFIRMED (mapping) / INFERRED (business meaning) |
| BR-M04-006 | Adding an offer row requires a campaign type, an offer type, the offer product where the mapping in BR-M04-005 uses a product, and a non-empty value in `txtQtyList`. Offer types 6 and 7 (type 3 campaigns) need no product. Duplicate offers are not prevented. | CampaignSetup.aspx.cs:613-853 | `ValiDeltList` | CONFIRMED |
| BR-M04-007 | Detail-row DTO: `Quantity` = header `txtProductQty` (repeated on every row); `ProductId` = header product; `BonusProductId` = the row's offer product; `BonusQuantity` = row Qty; `DiscountPercentage` = row percent; `QuantityDteail` = row amount. Header Min/Max amount live only on the master. All numeric values parsed with `decimal.Parse` (culture-dependent, no try/catch except the global one). | CampaignSetup.aspx.cs:1609-1655 | `btnSave_Click` | CONFIRMED |
| BR-M04-008 | Multiple customer types selected => the same campaign (all offers, markets, customers) is saved once **per type**, each with its own new master row (`aMaster.CustomerTypeId = item.Value`). List page collapses these into one row (BR-M04-019). | CampaignSetup.aspx.cs:1564-1566,1691,1748 | loop over `ddlChemistType.Items` | CONFIRMED |
| BR-M04-009 | Payment type: COD -> `IsPTforCOD=true`; NCOD -> `IsPTforOther=true`; Both -> both true; none selected -> both false. New campaign defaults to "Both". No server check that an option was chosen although the label shows `*`. Radio restore on edit: both flags -> "Both". | CampaignSetup.aspx.cs:46,164-196,1719-1739 | `Page_Load`,`GetOneRecord`,`btnSave_Click` | CONFIRMED |
| BR-M04-010 | Trade Policy and FCFS are mutually exclusive **on the client only** (`MutExChkList`); the server saves whatever is posted, both false is allowed. | CampaignSetup.aspx:492-494,924-933; CampaignSetup.aspx.cs:1715-1716 | JS | CONFIRMED |
| BR-M04-011 | "Ratio wise Increment": header flag saved to master; each detail row gets `IsRatioWiseIncrementPro = true` only if the header box is ticked **and** the row has an offer product id > 0 (bonus-product rows only). Meaning of "ratio wise increment" (bonus scales in multiples of the trigger quantity) is INFERRED from the name; the scaling itself is applied outside this repo. | CampaignSetup.aspx.cs:1626-1640,1741 | `btnSave_Click` | CONFIRMED (flag) / INFERRED (meaning) |
| BR-M04-012 | **Edit = replace.** For an existing id the DAL calls `sp_Del_BonusCampaignNewMaster(oldId)`, zeroes the id and inserts a brand-new master/detail set with a **new id**; the old id disappears from the edit URL and any downstream reference to the old `CampgainMasterId` is orphaned unless the delete SP is a soft delete. The `Update` branch of the DAL is dead code for this page. Because edit repeats the loop per selected type, the delete is invoked once per iteration. | BonusCampaignNewDAL.cs:191-204,231-320 | `SaveBonusCampaign` | CONFIRMED |
| BR-M04-013 | After the master is inserted the DAL calls `sp_Update_BonusCampaignpkCampaignSetupId(@CampaignMasterId, @CampgainMasterMapId, @CustomerTypeId)`; `@CampgainMasterMapId` comes from Session, populated by `GetCampaignSetupMapById("4")` (`BonusCampaignDetailsCustTypeId`), i.e. a **hard-coded map id 4**. If the lookup fails the session value stays null. Purpose (linking a campaign to a customer-type map) is INFERRED; effect UNKNOWN. | CampaignSetup.aspx.cs:1547-1561; BonusCampaignNewDAL.cs:244-248,287-291 | `btnSave_Click`,`SaveBonusCampaign` | CONFIRMED (call) / UNKNOWN (effect) |
| BR-M04-014 | Save is atomic per `SaveBonusCampaign` call (one transaction: delete, master, mapping, details, markets, customers). Across the per-customer-type loop each call is its own transaction, so a failure on type N leaves types 1..N-1 committed. Errors are re-thrown and shown as `faildalert(ex.Message)`. | BonusCampaignNewDAL.cs:186,359-371; DataAccessManager.cs:122-149,410-414 | DAL + DataAccessManager | CONFIRMED |
| BR-M04-015 | No date-range logic: From <= To, past-dated campaigns, overlapping campaigns of the same name/type/customer type, Amount <= MaxAmount and duplicate names are **not validated** on this page ("Already Exist!" is only the generic failure message when `isSuccess` is false). | CampaignSetup.aspx.cs:1419-1435,1756-1843 | absence | CONFIRMED (absence in C#); SP-side checks UNKNOWN |
| BR-M04-016 | Notification e-mail: after a successful save a `MailMessage` is built (subject "SMC Campaign Setup", body names campaign, type and date range; To `towsifcreatrix@...`, Bcc a gmail address) but **`smtp.Send` is commented out** (`// smtp.Send(mail);`), so no mail is sent by this page. The SMTP account credentials are hard-coded in source (`<redacted>`). The four legacy variants still call `smtp.Send`. | CampaignSetup.aspx.cs:1762-1831 | `btnSave_Click` | CONFIRMED |
| BR-M04-017 | Edit-load rule for customer type: if `CustomerTypeIdNew` (comma-separated) is present every id is re-selected, otherwise the single `CustomerTypeId` is selected; on save each selected type again produces a separate master (BR-M04-008/012). | CampaignSetup.aspx.cs:61-84 | `GetOneRecord` | CONFIRMED |

## 20. Permission Matrix
- Session keys read: `UserId` (used as `EntryBy`/`UpdateBy`; `Session["UserId"].ToString()` throws NullReference => caught => `faildalert(ex.Message)` if session expired), `CampgainMasterMapId` (written/cleared by this page).
- No role/UserType/RoleTypeId check in this page; access = DB menu only. Any authenticated user who knows the URL can open it; the list page hides "New Entry" only for `EmpInfoId=="496"` (BR-M04-020 in PAGE-116). Role matrix UNKNOWN.

## 21. Status Lifecycle
Campaign "status" is (a) `IsActive` flag set by the Active switch and (b) the validity window `FromDate..Todate`. No transition code exists: the flag is simply overwritten on every save. Because edit deletes and re-creates the master (BR-M04-012), a campaign's lifecycle is create -> (replace)* -> logically ended by unticking Active or by letting `Todate` pass. The list page treats "current" as `today BETWEEN FromDate AND Todate` when no Active filter is chosen (PAGE-116). There is no delete/inactivate button. CONFIRMED (code) / UNKNOWN (SP-side `sp_CampaignUpdateFromPage` effect).

## 22. Error Handling
- Nearly every reader is wrapped in `try { } catch (Exception ex) { }` (empty) - seed failures leave empty drop-downs silently (`:240-353`), `GetOneRecord` failure leaves a half-filled edit form silently (`:230`).
- `btnSave_Click` outer `catch` -> `faildalert(ex.Message)` (may expose SQL error text to the user).
- Failure path message is always "Already Exist!" when `Res.isSuccess` false (misleading).
- Global `Application_Error` is empty.

## 23. Audit
`@EntryBy` = `Session["UserId"]` on insert. No update-by/date on this path (update branch dead). Audit columns and any history of replaced campaigns UNKNOWN (delete SP). No application log.

## 24. Integration
- Mobile/order-capture consumption UNKNOWN (no code in repo). Consumers found: `OrderInfoDAL.LoadCampaignDtls` (looks up `DiscountPercentage` by trimmed `CampaignName`), `OrderInfoDAL.getEzeventCampaignCheck/getEsomiumCampaignCheck/getEsomiumCampaignCheckSeacoral` (three campaign names hard-coded, see BR-M04-073).
- `CampaignView` "Sync Now" calls `sp_CampaignUpdateFromPage` (mobile sync per method name; INFERRED).
- SMTP (disabled, BR-M04-016).

## 25. Reports
None from this page. List: PAGE-116.

## 26. Dependencies
BLL: none. DAL: BonusCampaignNewDAL, SeedDataDAL, QuotedPriceSetupDAL, DataAccessManager. DAO: BonusCampaignNewMasterDAO, BonusCampaignNewDetailDAO, BonusCampaignMarketDetailDAO, CampaignCustomerDetailDAO, ResultInfo. UI: IVMarketStructure.ascx, AjaxControlToolkit (FilteredTextBox, AutoComplete), select2, sweetalert helpers. SMTP (System.Net.Mail).

## 27. Test Cases
Derived from the rules; none exist in the repo. Live DB required (SP bodies unknown).

| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | New campaign, all header fields valid, type 1 with product+qty, one offer, no market/customer (UI hidden) | Blocked with "please Add to List Market Structure or Customer!" (if confirmed defect) | BR-M04-004 |
| TC-02 | Edit an existing campaign (`?MID=`) and save unchanged | New master id created, old id deleted; list shows one row | BR-M04-012 |
| TC-03 | Select 3 customer types, save | 3 masters created with identical offers; list shows 1 row | BR-M04-008/019 |
| TC-04 | Campaign type 2, leave Max Amount empty | Max Amount tooltip, no save | BR-M04-002 |
| TC-05 | Type 3, offer 7 (amount, no product) | Row added with empty product | BR-M04-005/006 |
| TC-06 | Add offer with empty value box | Tooltip on `txtQtyList`, row not added | BR-M04-006 |
| TC-07 | Tick Trade Policy then FCFS | Trade Policy unticks (client) | BR-M04-010 |
| TC-08 | Payment "COD" | IsPTforCOD=1, IsPTforOther=0 | BR-M04-009 |
| TC-09 | Ratio wise ON with offer that has no product | detail flag false | BR-M04-011 |
| TC-10 | To date earlier than From date | Saved without error (defect) | BR-M04-015 |
| TC-11 | Force SP failure on 2nd of 3 types | Type 1 committed, 2 rolled back, 3 not attempted | BR-M04-014 |
| TC-12 | Session expired then Submit | "Object reference..." alert | section 20 |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/CampaignSetup.aspx`, `.aspx.cs`; `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs`; `Library.DAL/DataManager/DataAccessManager.cs`; `Library.DAO/DoctorModule_DAO/BonusCampaignNewMasterDAO.cs`; `Solution.Web/App_Code/SInventoryWebService.cs`; `Solution.Web/MasterSetup_UI/CampaignView.aspx.cs`.
- File timestamps used for live/legacy judgement: `CampaignSetup.aspx.cs` 2025-09-06, `.aspx` 2026-07-15; `PrecompiledWeb/.../CampaignSetup.aspx` built 2026-09-16.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | full page read | SP bodies absent | MISSING | - |
| New campaign creation | None existed | blocked by BR-M04-004 unless UI un-hidden | n/a | CONFLICT (page vs its own validation) | aspx:676-874 vs .cs:1529 |
| Edit semantics | None existed | delete + re-insert with new id | UNKNOWN | UNKNOWN | BR-M04-012 |
| E-mail notification | None existed | built but not sent | n/a | DEAD CODE | BR-M04-016 |

## 30. Known Gaps
- New-campaign validation vs hidden market/customer UI (GAP-M04-001). Edit replaces ids (GAP-M04-002). Hard-coded SMTP credential in source, mail To/Bcc personal addresses (GAP-M04-003). Hard-coded map id 4 and unknown `sp_Update_BonusCampaignpkCampaignSetupId` (GAP-M04-004). No date/overlap validation (GAP-M04-005). Static shared DAL/DataAccessManager instance across requests (GAP-M04-013). All swallowed exceptions (`catch { }`) hide seed/load failures. 17 campaign SPs without bodies (GAP-M04-009). Five near-duplicate legacy pages still deployed (GAP-M04-006).

## 31. Change Impact
- Shares `BonusCampaignNewDAL.SaveBonusCampaign` with PAGE-113, 114, 115, 117 (and `SaveBonusCampaign_final` only with PAGE-112). Any change to the SP set affects all.
- `tbl_BonusCampaignNewMaster/Detail` are read by invoice/sales-return flows (`OrderInfoDAL`, `OrderInfoDALSalesReturn`, `SalesReturnNewDAL`); changing campaign name semantics (edit-replace) may break the name lookup (`LoadCampaignDtls` joins by name).
- `SeedDataDAL` seeds and `sp_GET_CampaignType/Category/OfferTypeInfo` are shared with every campaign page and `CampaignView`.
