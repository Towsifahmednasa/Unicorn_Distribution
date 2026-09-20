---
page_id: PAGE-168
module: M05
page: CustomerEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-168 — CustomerEntry

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs`, `CustomerInfoDAL.SaveInfo`, `DataAccessManager` and the SP bodies of `sp_Save_CustomerMaster` / `sp_Update_CustomerMaster` (repo file `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`) were read. Bodies of `sp_GET_CustMaster_ById`, `sp_Save_CustProductLineDetail` and the dropdown seed SPs are NOT in the repo (UNKNOWN). Confidence tags: CONFIRMED / INFERRED / UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-168 |
| Module | M05 — Customer & Provider Master |
| Page name | CustomerEntry (heading "Customer Information", sub-heading "Market Structure") |
| Kind | Entry + Edit form (one page, two modes) — CONFIRMED `Solution.Web/MasterSetup_UI/CustomerEntry.aspx.cs:61-78` |
| Web folder | MasterSetup_UI |
| Master page | `~/MasterPages/NewMasterPage.master` |
| Page class | `MasterSetup_UI_CustomerEntry` |
| Modes | Create (no `MID`) shows `btnSave`; Edit (`?MID=<CustomerMasterId>`) shows `btnUpdate` — both post to `btnSave_Click` |
| Code-behind LOC | 919 |
| Legacy/duplicate | The current customer entry. Legacy twins: `SInventory_UI/CustMasterEntry.aspx` (PAGE-186) and `SInventory_UI/CustomerMasterEdit.aspx` / `CustMasterEdit.aspx` (PAGE-194 / PAGE-185) write a different column set of the same `tblCustMaster` table |

## 2. Purpose
Create a new customer (pharmacy / chemist / provider outlet) or edit an existing one in `tblCustMaster`, assigning it to a market (Group→Region→Area→Territory→SubTerritory→Market), customer type, provider (program) type, station types, distribution center/route, one or more tagged sales persons and product lines. CONFIRMED (`CustomerEntry.aspx.cs:743-858`, SP `sp_Save_CustomerMaster`).

## 3. Business Objective
Maintain the customer master used by sales orders, invoicing, field-force call planning and the mobile app: one row per outlet, with an auto-generated code (`C<n>`), the market hierarchy for reporting/targets, and the salespeople who may sell to it. INFERRED from column set and downstream joins (`sp_Get_CustMasterList_Approve`, `SInventoryWebService.GetCustomernew`).

## 4. Navigation
- Menu placement DB-driven (`sp_GET_MainMenuRole2`) — exact path UNKNOWN.
- Inbound: `MasterSetup_UI/CustomerView.aspx` (grid edit button opens `CustomerEntry.aspx?mid=<id>` in a new window, `CustomerView.aspx.cs:436`), `MasterSetup_UI/CustomerListPending.aspx` (`Response.Redirect("CustomerEntry.aspx?MID=…")`, `CustomerListPending.aspx.cs:180`), `Approval_UI/CustomerApproveList.aspx` (window.open `../MasterSetup_UI/CustomerEntry.aspx?MID=`, `CustomerApproveList.aspx.cs:114`), `CustomerChangeProgramType.aspx` (add button).
- Outbound: after successful save → `CustomerView.aspx`; Reset → `CustomerEntry.aspx`.

## 5. Route
`~/MasterSetup_UI/CustomerEntry.aspx` (file route; optional `?MID=`).

## 6. Source Files
| Type | Path |
|---|---|
| View | `Solution.Web/MasterSetup_UI/CustomerEntry.aspx` |
| Code-behind | `Solution.Web/MasterSetup_UI/CustomerEntry.aspx.cs` |
| User control | `Solution.Web/MasterSetup_UI/IVMarketStructure.ascx(.cs)` — cascading Group/Zone/Area/Territory/SubTerritory/Market dropdowns |
| DAL | `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs` (`SaveInfo`, `GetCustomerSetupById`), `Library.DAL/DoctorModule_DAL/SeedDataDAL.cs`, `CommonDataLoad.cs` |
| Transaction wrapper | `Library.DAL/DataManager/DataAccessManager.cs` |
| DAO | `Library.DAO/MasterSetup_DAO/CustMasterInfoDAO.cs`, `Library.DAO/SInventory_Entities/ResultInfo.cs` |
| SP script | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:402-489` (save), `:549-662` (update) |

## 7. UI Control Inventory
Server validation (`Validation()`, `CustomerEntry.aspx.cs:509-741`) is the only enforced validation; no ASP.NET validators are used. Red `*` marks in the markup are cosmetic for several fields that are not actually validated (noted).

| Control | Label | Required (server) | Default / initial | Enabled / visible logic | Data source | Error / behaviour |
|---|---|---|---|---|---|---|
| `id_mastetID` (hidden) | — | — | `MID` query value | set only when `MID` present | QueryString | 0/"" ⇒ create, >0 ⇒ update (`:753`) |
| `txtChemistName` | Customer Name | YES | — | — | user | invalid CSS + tooltip "please fill out this field"; returns false (`:534-540`) |
| `ddlChemisType` | Customer Type | YES (`SelectedValue==""`) | "Please Select From List" | — | `SeedDataDAL.GetChemistTypeList` → `sp_GET_CustomerType` | same tooltip (`:542-548`) |
| `ddlDistributionRoute` | Distribution Route | no (check commented out `:550-556`, `*` still shown) | blank | — | `GetDistributionRouteList` → `sp_GET_DistributionRoute` | — |
| `txtEmail` | Email | no | — | — | user | no format validation |
| `txtMobile` | Mobile | YES + exactly 11 chars | — | `MaxLength=11`; client `FilteredTextBoxExtender` allows digits only | user | empty ⇒ tooltip; length≠11 ⇒ popup "Mobile NO must be 11 digits!" via `showpop6` (`:724-736`) |
| `txtOwnerName` | Owner Name | no (check commented `:574-580`) | — | — | user | — |
| `ddlSalesPerson` | Sales Person | no | none selected | multi-select (`SelectionMode=Multiple`, select2) | `CommonDataLoad.GetDoctor_SalesPerson_Active` → `sp_GetDoctor_SalesPerson_Active` (`EmpInfoId`,`EmpName`) | see BR-M05-007 |
| `txtAddress` | Address | YES | — | multiline | user | tooltip (`:583-589`) |
| `txtVoterID` | NID NO | no (check commented) | — | `MaxLength=17`, digits only (client) | user | 17-digit rule commented out (`:710-722`) |
| `txtTradeLicense` | "Refered Doctor" (label) but stored in `TradeLicense` | no | — | — | user | label/column mismatch |
| `txtDrugLicense` | Drug License | no | — | — | user | — |
| `txtPharmacyCouncilCertificate` | Pharmacy Council Certificate | no | — | — | user | — |
| `txtBCDS` | BCDS Registration (placeholder wrongly "Drug License") | no | — | — | user | — |
| `ddlStationType` | Station Type (MIO) | no (`*` shown; check commented `:591-597`) | blank | — | `GetStationTypeList` → `sp_GET_StationTypeList` | server SP overrides from market |
| `ddlAMStationType` | Station Type (AM) | no | blank | — | same list | overridden by SP |
| `ddlDZSMStationType` | Station Type (DZSM) | no | blank | — | same list | overridden by SP |
| `ddlProgramType` | Provider Type | no (`*` shown; check commented) | blank | — | `GetProgramTypeListParam(" and GRP.IsCustomer=1")` → `sp_GET_ProgramTypeListParm` | only customer-flagged provider types offered |
| `ddlPharmaPlatform` | Pharma Platform | no | blank | — | `GetSMCTypeListParam(" and GRP.forCustomer=1")` → `sp_GET_SMCTypeListParm` | **never read on save** — not persisted (GAP-M05-004) |
| `ddlProLine` | Product Line | no (`*` shown) | blank item | multi-select | `GetProLineDataTableList` → `sp_GET_ProductLine` | saved as child rows |
| `txtRemarks` | Remarks | no | — | multiline | user | mapped to `Reamrks` |
| `ddlDistributionCenter` | Distribution Center | no (`*` shown) | blank | — | `GetDistributionCenterDataTableList` → `sp_GET_DistributionCenter` | on create stored with DC code/name; **ignored on update** (BR-M05-009) |
| `IVMarketStructure` (6 dropdowns + 6 hidden) | Group…Market | no (`MarketSelect` may be empty) | blank | edit mode: enabled and pre-selected from `hfGroupId…hfMarket` | `CommonDataLoad` `GetGroupInfo_*`, `GetZone_byGroupId_*`, … | market optional ⇒ geography stays null |
| `ddlDivision`/`ddlDistrict`/`ddlThana` | Division/District/Thana | no | blank | cascading (`AutoPostBack`) | `GetDivisionList`, `GetDistrictList(id)`, `GetThanaList(id)` | SP overrides from market's thana |
| `chkIsActive` | Active | — | checked | — | user | saved to `IsActive` |
| `ShopImgUploadForm`, `TradeLicenseimageUploadForm` (file inputs) | Shop Image / Trade License Image | — | — | client-only base64 preview | JS | **never persisted** (GAP-M05-003) |
| `btnSave` / `btnUpdate` | Save / Update | — | — | `btnSave.Visible` when no `MID`; `btnUpdate.Visible` when `MID` (`:63,77`) | `sweetAlertConfirm_Submit/Update` client confirm | → `btnSave_Click` |
| `btnReset` | Reset | — | — | — | — | `Response.Redirect("CustomerEntry.aspx")` |

## 8. Field Mapping (UI → DAO → SP parameter → table.column)
Source: `CustomerEntry.aspx.cs:751-842`, `CustomerInfoDAL.cs:258-402`, SP `sp_Save_CustomerMaster` / `sp_Update_CustomerMaster`. "C" = written on create, "U" = written on update.
| UI control | DAO property | SP param | `tblCustMaster` column | C | U |
|---|---|---|---|---|---|
| id_mastetID | CustomerMasterId | @CustomerMasterId | CustomerMasterId (key; identity on insert) | ignored | WHERE key |
| txtChemistName | CustomerName | @CustomerName | CustomerName | Y | Y |
| ddlChemisType (value/text) | CustomerTypeId / CustomerType | @CustomerTypeId / @CustomerType | CustomerTypeId / CustomerType | Y | Y |
| ddlDistributionRoute | DistributionRouteId | @DistributionRouteId | DistributionRouteId | Y | Y |
| txtEmail | Email | @Email | Email | Y | Y |
| txtMobile | CellNo | @CellNo | CellNo | Y | Y |
| txtOwnerName | OwnerName | @OwnerName | OwnerName | Y | Y |
| txtAddress | Address | @Address | Address | Y | Y |
| txtVoterID | VoterID | @VoterID | VoterID | Y | Y |
| txtTradeLicense | TradeLicense | @TradeLicense | TradeLicense | Y | Y |
| txtDrugLicense | DrugLicense | @DrugLicense | DrugLicense | Y | Y |
| txtPharmacyCouncilCertificate | PharmacyCouncilCertificate | @PharmacyCouncilCertificate | PharmacyCouncilCertificate | Y | Y |
| txtBCDS | BCDS | @BCDS | BCDS | Y | Y |
| txtRemarks | Reamrks | @Reamrks | Reamrks | Y | Y |
| chkIsActive | IsActive | @IsActive | IsActive | Y | Y |
| MarketSelect (value/text) | MarketId / MarketName | @MarketId / @MarketName | MarketId / MarketName | Y | Y |
| ddlStationType (value/text) | StationTypeId / CustomerStation | @StationTypeId / @CustomerStation | StationTypeId / CustomerStation | overwritten by market lookup | overwritten (2nd UPDATE) |
| ddlAMStationType | NSMStationTypeId | @NSMStationTypeId | NSMStationTypeId | overwritten by market lookup (UserRoleID=2) | overwritten |
| ddlDZSMStationType | DZSMStationTypeId | @DZSMStationTypeId | DZSMStationTypeId | overwritten (UserRoleID=3) | overwritten |
| ddlDistributionCenter | ComUnitId | @ComUnitId | ComUnitId, ComUnitName, ComUnitCode (looked up from `tblCompanyUnit`) | Y | **NO** (param accepted, never used) |
| ddlSalesPerson (ids) | TaggedSalesPersonIds | @TaggedSalesPersonIds | TaggedSalesPersonIds | Y | Y (as posted) |
| ddlSalesPerson (text before ':') | TaggedSalesPersonCodes | @TaggedSalesPersonCodes | TaggedSalesPersonCodes | Y | Y |
| first selected sales person | EmpInfoId | @EmpInfoId | SalesPersonId | Y | Y |
| ddlProgramType | ProgramTypeId | @ProgramTypeId | ProgramTypeId | Y | Y |
| ddlDivision (value/text) | DivisionId / Division | @DivisionId / @Division | DivisionId / Division | id overwritten from market→thana→district→division | id overwritten; text as posted |
| ddlDistrict, ddlThana | DistrictId/District, ThanaId/Thana | idem | DistrictId/District, ThanaId/Thana | ids overwritten | ids overwritten |
| ddlProLine (selected values) | (`ProLineArray` string) | `sp_Save_CustProductLineDetail(@CustomerMasterId,@ProductLineID)` per line | `tblCustProductLine` (per SP name; body UNKNOWN) | Y | delete-then-insert |
| (Session UserId) | — | @EntryBy (create) / @UpdateBy (update) | CreateBy + CreateDate=GETDATE() / UpdateBy + UpdateDate=GETDATE() | Y | Y |
| (SP-derived from MarketId) | — | — | GroupId, RegionId, AreaId, TerritoryId, SubTerritoryId | Y | Y |
| (constant in SP) | — | @ActionStatus **ignored** | ActionStatus = `'2'` | Y (hard-coded) | not touched |
| (SP-generated) | — | — | CustomerCode = `'C'+n` | Y | not touched |
Not mapped anywhere: `ddlPharmaPlatform` (SMCTypeId), image files, CategoryId, InActiveDate.

## 9. Dropdown Mapping
| Dropdown | Loader | SP | Value / Text | Filter |
|---|---|---|---|---|
| ddlChemisType | `SeedDataDAL.GetChemistTypeList` (`SeedDataDAL.cs:610`) | `sp_GET_CustomerType` | CustomerTypeId / CustomerType | UNKNOWN (SP body absent) |
| ddlDistributionRoute | `GetDistributionRouteList` (`:788`) | `sp_GET_DistributionRoute` | DistributionRouteId / DistributionRouteName | UNKNOWN |
| ddlDistributionCenter | `GetDistributionCenterDataTableList` (`:669`) | `sp_GET_DistributionCenter` | ComUnitId / ComUnitName | UNKNOWN |
| ddlDivision | `GetDivisionList` (`:828`) | `sp_GET_DivisionList` | DivisionId / DivisionName | — |
| ddlDistrict | `GetDistrictList(divisionId)` (`:1008`) | `sp_GET_DistrictList` | DistrictId / DistrictName | by division |
| ddlThana | `GetThanaList(districtId)` (`:1028`) | `sp_GET_ThanaList` | ThanaId / ThanaName | by district |
| ddlStationType / AM / DZSM | `GetStationTypeList` (`:850`) | `sp_GET_StationTypeList` | StationTypeId / StationTypeName | same table bound 3× |
| ddlProgramType | `GetProgramTypeListParam(" and GRP.IsCustomer=1")` (`:928`) | `sp_GET_ProgramTypeListParm(@Parm)` | ProgramTypeId / ProgramTypeName | customer-flagged only |
| ddlPharmaPlatform | `GetSMCTypeListParam(" and GRP.forCustomer=1")` (`:949`) | `sp_GET_SMCTypeListParm(@Parm)` | SMCTypeId / SMCType | customer-flagged only |
| ddlProLine | `GetProLineDataTableList` (`:629`) | `sp_GET_ProductLine` | ProductLineID / LineName | — |
| ddlSalesPerson | `CommonDataLoad.GetDoctor_SalesPerson_Active` (`:753`) | `sp_GetDoctor_SalesPerson_Active` | EmpInfoId / EmpName (text expected `Code : Name`) | active field staff |
| Market structure | `IVMarketStructure.ascx.cs` | `CommonDataLoad.Get*_Active/_All` | GroupId…MarketId | active-only on create, all on edit |

## 10. Button Action Mapping
| Button | Handler | DAL | SP(s) | DB effect |
|---|---|---|---|---|
| btnSave | `btnSave_Click` `CustomerEntry.aspx.cs:743` (after `Validation()`) | `CustomerInfoDAL.SaveInfo` | `sp_Save_CustomerMaster` → `sp_Save_CustProductLineDetail`×n | INSERT `tblCustMaster` (ActionStatus '2'), product-line rows |
| btnUpdate | same handler (id>0) | same | `sp_Update_CustomerMaster` → `sp_Save_CustProductLineDetail`×n | UPDATE `tblCustMaster`; DELETE+re-INSERT `tblCustProductLine` |
| btnReset | `btnReset_OnClick` | — | — | redirect only |
| ddlDivision/ddlDistrict | `…_SelectedIndexChanged` | `SeedDataDAL` | `sp_GET_DistrictList` / `sp_GET_ThanaList` | read only |

## 11. Frontend Flow
1. `Page_Load` (every request): resolves the six user-control dropdowns/hidden fields via `FindControl`; reads `Session["RoleTypeName"]`; re-applies image previews from hidden fields (`:26-53`).
2. First load: `LoadInitialInfo()` binds all dropdowns; if `MID` → `GetOneRecord` (`:83-329`) fills all controls from `sp_GET_CustMaster_ById`, pre-selects sales persons from `TaggedSalesPersonIds` (falling back to `SalesPersonId`), cascades Division→District→Thana, sets the hidden market ids for the user control, ticks product lines (one result row per product line), and tries to read the shop/trade-licence JPEG from disk path `ImagePreName+Id+".jpg"` into a base64 data-URI.
3. Client JS (`CustomerEntry.aspx:90-170`): select2 init (`pageLoad`), `ImageToBase64ShopImg` and `ImageToBase64Trade` (FileReader → hidden fields `imgeBase64Str`, `hfTradeBase64Str`, preview `<img>`). `ImageToBase64ShopImg` references an undefined variable `file` (`file["type"]`) so it throws before previewing (CONFIRMED `CustomerEntry.aspx:112-114`).
4. Save/Update buttons run `sweetAlertConfirm_*` (shared confirm helper — definition outside this page) then postback. Success ⇒ `successalert('Operation successful!','Success','CustomerView.aspx')`; failure ⇒ `faildalert('Already Exist!','Faild')`.

## 12. Backend Flow
`btnSave_Click` (`:743-858`): `Validation()` → build `CustMasterInfoDAO` (empty strings ⇒ null; `SelectedIndex>0` guards for ints) → build tagged-sales-person lists (`item.Text.Split(':')[0].Trim()` = code) → collect selected product lines → `_DAL.SaveInfo(aMaster, ProLineArray, Session["UserId"])`.
`CustomerInfoDAL.SaveInfo` (`:258-402`): opens `DataBase.SalesDB` connection; `CustomerMasterId>0` ⇒ `UpdateData("sp_Update_CustomerMaster")` + `@UpdateBy`; else `SaveDataReturnPrimaryKey("sp_Save_CustomerMaster")` + `@EntryBy` (returns `SCOPE_IDENTITY`); if `pk>0` and product lines chosen ⇒ `SaveData("sp_Save_CustProductLineDetail")` per line. `catch` ⇒ `SqlConnectionClose(true)` (rollback) + rethrow; `finally` ⇒ `SqlConnectionClose()` (commit). CONFIRMED.
Transaction: `DataAccessManager` lazily starts one `SqlTransaction` on the first write (`DataAccessManager.cs:410-415,432-437`) and commits/rolls back on close — the whole save (master + product lines) is atomic. Exceptions thrown from `SaveInfo` are **not** caught by the page (`btnSave_Click` has no try/catch) ⇒ unhandled server error page.

## 13. API Mapping
No `[WebMethod]`, no AJAX endpoint. The market-structure user control uses server postbacks only.

## 14. Service Mapping
No BLL: the page calls `CustomerInfoDAL`, `SeedDataDAL`, `CommonDataLoad` directly (CONFIRMED, `CustomerEntry.aspx.cs:16-19`).

## 15. Repository Mapping
| DAL method | Source | SP | Params |
|---|---|---|---|
| `CustomerInfoDAL.GetCustomerSetupById` | `CustomerInfoDAL.cs:19` | `sp_GET_CustMaster_ById` | @id |
| `CustomerInfoDAL.SaveInfo` | `CustomerInfoDAL.cs:258` | `sp_Save_CustomerMaster`, `sp_Update_CustomerMaster`, `sp_Save_CustProductLineDetail` | full list in §8 + @EntryBy/@UpdateBy |
| `SeedDataDAL.*` | `SeedDataDAL.cs` | see §9 | — |
| `CommonDataLoad.GetDoctor_SalesPerson_Active` | `CommonDataLoad.cs:753` | `sp_GetDoctor_SalesPerson_Active` | — |
Identical `SaveInfo` copies exist in `OrderTrackingDAL.cs:236-244` and `MarketStructureTransferDAL.cs:715-723` (same SP names) — see GAP-M05-018.

## 16. Stored Procedure Mapping
| SP | Body | Known behaviour |
|---|---|---|
| `sp_Save_CustomerMaster` | **READ** `LIVE_DB_UPDATE…sql:402-489` | see §17/§19 |
| `sp_Update_CustomerMaster` | **READ** `:549-662` | see §17/§19 |
| `sp_GET_CustMaster_ById`, `sp_Save_CustProductLineDetail`, seed SPs (§9) | NOT in repo | UNKNOWN |
`sp_Save_CustomerMaster` (create), in order: (1) derive Division/District/Thana/SubTerritory/Territory/Area/Region/Group from `tblMarket` (INNER JOIN SubTerritory→Territory→Area→Region; LEFT JOIN Thana→District→Division) `WHERE MarketId=@MarketId` (overwrites the passed ids when a row is found); (2) `@StationTypeId/@NSMStationTypeId/@DZSMStationTypeId` from `tblMarketStationDetail` where `UserRoleID` = 1 / 2 / 3; (3) `ComUnitName/ComUnitCode` from `tblCompanyUnit` by `@ComUnitId`; (4) `@CustCodeint = MAX(CONVERT(INT,SUBSTRING(CustomerCode,2,LEN+1)))+1` over `ActionStatus='2'` rows, `CustomerCode='C'+…`; (5) if `@TaggedSalesPersonIds IS NULL AND @EmpInfoId IS NOT NULL` ⇒ tagged = CAST(@EmpInfoId); (6) INSERT with `ActionStatus='2'`, `CreateBy=@EntryBy`, `CreateDate=GETDATE()`, `SalesPersonId=@EmpInfoId`; (7) `SELECT SCOPE_IDENTITY()`.
`sp_Update_CustomerMaster`: same geography derivation; UPDATE of name/address/cell/market/geo ids/`IsActive`/type/`UpdateBy`/`UpdateDate`/`SalesPersonId=@EmpInfoId`/`TaggedSalesPersonIds`/`TaggedSalesPersonCodes`/route/owner/licences/BCDS/email/remarks/`ProgramTypeId`/station types; then `DELETE FROM tblCustProductLine WHERE CustomerMasterId=…`; re-derives station types & division/district/thana and UPDATEs them again. `@ComUnitId`, `@ActionStatus` are declared but never used.

## 17. Database Mapping
- Read (in SPs): `tblMarket`, `tblSubTerritory`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Thana`, `tbl_District`, `tbl_Division`, `tblMarketStationDetail`, `tblCompanyUnit`, `tblCustMaster` (MAX code).
- Written: `tblCustMaster` (INSERT/UPDATE), `tblCustProductLine` (DELETE in update SP; INSERT via unseen SP).
- Audit columns: `CreateBy`, `CreateDate`, `UpdateBy`, `UpdateDate`.

## 18. Validation Matrix
| Field | Rule | FE | BE | DB | Message | BR |
|---|---|---|---|---|---|---|
| txtChemistName | required | no | yes (`:534`) | none known | CSS `is-invalid`, tooltip "please fill out this field" | BR-M05-001 |
| ddlChemisType | required | no | yes (`:542`) | — | same | BR-M05-001 |
| txtMobile | required | digits-only extender + MaxLength 11 | yes (`:566`) | — | tooltip | BR-M05-001 |
| txtMobile | length == 11 | MaxLength | yes (`:724`) | — | "Mobile NO must be 11 digits!" | BR-M05-002 |
| txtAddress | required | no | yes (`:583`) | — | tooltip | BR-M05-001 |
| Uniqueness (name, mobile, code) | none | — | none | none in SP | shows "Already Exist!" for any non-success | BR-M05-010 |
| Distribution route, owner, NID(17), station types, program type, DC, market, division/district/thana | check code exists but commented out | — | **not enforced** | — | — | BR-M05-001 |

## 19. Business Rules
| ID | Rule | Source | Implementation | Conf. |
|---|---|---|---|---|
| BR-M05-001 | Only Customer Name, Customer Type, Mobile and Address are mandatory; all other required-looking fields (route, owner, NID, station types, provider type, DC, market, product line) have their checks commented out. | `CustomerEntry.aspx.cs:509-741` | `Validation()` | CONFIRMED |
| BR-M05-002 | Mobile must be exactly 11 characters (length check only; digits are enforced only by the client filter). | `CustomerEntry.aspx.cs:724-736`; `CustomerEntry.aspx:225-226` | `Validation()` | CONFIRMED |
| BR-M05-003 | Same button handler creates or updates: `CustomerMasterId` from hidden field (`MID`) ⇒ 0 = create (`sp_Save_CustomerMaster`, `@EntryBy`), >0 = update (`sp_Update_CustomerMaster`, `@UpdateBy`); user = `Session["UserId"]`. | `CustomerEntry.aspx.cs:753,844`; `CustomerInfoDAL.cs:338-359` | `SaveInfo` | CONFIRMED |
| BR-M05-004 | Customer code is system-generated on create only: `'C' + (max numeric suffix of ActionStatus='2' codes + 1)`; the user cannot enter or change it. | `LIVE_DB…sql:460-463` | `sp_Save_CustomerMaster` | CONFIRMED |
| BR-M05-005 | A customer created on this page is immediately Approved (`ActionStatus='2'`); the `@ActionStatus` parameter is ignored. | `LIVE_DB…sql:483` | `sp_Save_CustomerMaster` | CONFIRMED |
| BR-M05-006 | Group/Region/Area/Territory/SubTerritory and Division/District/Thana ids are derived from the chosen market (not trusted from the form); MIO/AM/DZSM station types come from `tblMarketStationDetail` for that market (UserRoleID 1/2/3) and replace the form values. | `LIVE_DB…sql:443-456,590-599,646-660` | both SPs | CONFIRMED |
| BR-M05-007 | Tagged sales persons: every selected list item contributes its `EmpInfoId` (comma list → `TaggedSalesPersonIds`) and the part of the text before `:` (→ `TaggedSalesPersonCodes`); the first selected becomes `SalesPersonId` (`EmpInfoId`). If ids are null and a primary exists the SP sets tagged ids = primary id. On edit the list is pre-selected from `TaggedSalesPersonIds`, else `SalesPersonId`. On update the posted values overwrite the stored ones (empty selection ⇒ empty string, not null). | `CustomerEntry.aspx.cs:144-158,792-805`; `LIVE_DB…sql:465-468,601-604,627-629` | page + SPs | CONFIRMED |
| BR-M05-008 | Save is one DB transaction; on update the customer's product lines are deleted and re-inserted from the form (empty selection ⇒ all lines removed). | `DataAccessManager.cs:410-437`; `LIVE_DB…sql:644` | `SaveInfo` + `sp_Update_CustomerMaster` | CONFIRMED |
| BR-M05-009 | Update never changes `CustomerCode`, `ActionStatus`, `CreateBy/Date`, nor the Distribution Center (`ComUnitId/Name/Code`) even though the DC dropdown is editable. | `LIVE_DB…sql:606-642` | `sp_Update_CustomerMaster` | CONFIRMED |
| BR-M05-010 | There is no duplicate check (name, mobile, code) on create or update in page, DAL or SP; the message "Already Exist!" is displayed for any non-success return (which practically never happens). Duplicate mobile numbers are only flagged later in the approval list (`DupCell`). | `CustomerInfoDAL.cs:346-358`; `CustomerEntry.aspx.cs:852`; `LIVE_DB…sql:272-278` | — | CONFIRMED |
| BR-M05-011 | Intended rule "non-Admin users cannot edit master/structure fields on create" never executes: it is inside `GetOneRecord` (edit only) but guarded by `!isEditMode`. All roles can edit all fields in edit mode. | `CustomerEntry.aspx.cs:286-324` | `GetOneRecord` | CONFIRMED |
| BR-M05-012 | Provider Type dropdown lists only program types flagged for customers (`GRP.IsCustomer=1`); Pharma Platform only `GRP.forCustomer=1`. | `CustomerEntry.aspx.cs:467,485` | `SeedDataDAL` | CONFIRMED (filter text) / UNKNOWN (SP) |
| BR-M05-013 | Inactive customers keep their record; display convention appends " (Inactive)" to the name in searches. | `LIVE_DB…sql:829`; `SInventoryWebService.cs:353` | list SP / autocomplete | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: only `Session["RoleTypeName"]` is read, and only by the dead branch (BR-M05-011). No `UserRoleID`, no `RAdd/REdit` check on this page (contrast `CustomerView.aspx.cs:332-364`). Any authenticated user with the URL (menu) can create/edit any customer. CONFIRMED.
- Menu/role visibility: DB-driven, UNKNOWN.

## 21. Status Lifecycle
`tblCustMaster.ActionStatus` (string): `'0'` Pending, `'1'` Verified, `'2'` Approved, `'3'` Rejected (labels from `sp_Get_CustMasterList_Approve`, `LIVE_DB…sql:839-840`). This page: create ⇒ `'2'`; update ⇒ unchanged. Transitions to/from 0/1/3 are performed by the app-approval flow (`Approval_UI/CustomerApproveList.aspx` → `sp_webapi_SaveCustomerAppLog`, other module, PAGE-695) and by mobile-created rows (UNKNOWN SP). `IsActive` bit is independent (active/inactive).

## 22. Error Handling
`Validation()` failure ⇒ inline CSS/tooltip or `showpop6` popup, no DB call. `GetOneRecord` and `LoadInitialInfo` swallow all exceptions (`catch {}`), so a failed dropdown/record load yields a blank/partly filled form silently (GAP-M05-005). `btnSave_Click` has no try/catch; DAL rethrows after rollback ⇒ ASP.NET error page. No logging (Global.asax `Application_Error` empty).

## 23. Audit
`CreateBy` (UserId int), `CreateDate` (`GETDATE()`), `UpdateBy`, `UpdateDate`. No before/after history; overwritten values are lost.

## 24. Integration
Customers created here feed the mobile app / field-force modules through `tblCustMaster` (INFERRED). No outbound SAP call in this page (SAP customer code exists as `CustomerBsPCode` column, `LIVE_DB…sql:827`, set elsewhere — UNKNOWN).

## 25. Reports
None from this page. Listing/export: `CustomerView.aspx` (PAGE-173).

## 26. Dependencies
DAL `CustomerInfoDAL`, `SeedDataDAL`, `CommonDataLoad`, `DataAccessManager`; user control `IVMarketStructure`; JS libs select2, SweetAlert helpers (defined in master page).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Create with name, type, mobile (11 digits), address only | Saved; new row `ActionStatus='2'`, `CustomerCode='C'+n`; redirect to CustomerView | 001,004,005 |
| TC-02 | Create leaving Name empty | invalid styling, no save | 001 |
| TC-03 | Mobile with 10 digits | popup "Mobile NO must be 11 digits!" | 002 |
| TC-04 | Create with market selected | GroupId…SubTerritoryId, Division/District/Thana and station types populated from market, not form | 006 |
| TC-05 | Select 3 sales persons | `TaggedSalesPersonIds`="a,b,c", Codes="A,B,C", `SalesPersonId`=a | 007 |
| TC-06 | Create two customers with identical name+mobile | both succeed (no duplicate check) | 010 |
| TC-07 | Edit (MID) and change DC | DC unchanged in DB | 009 |
| TC-08 | Edit, untick all product lines | all `tblCustProductLine` rows removed | 008 |
| TC-09 | Edit as non-Admin | all fields editable (rule 011 not enforced) | 011 |
| TC-10 | Force SP error (e.g. bad ProgramTypeId FK) | full rollback, error page | 008 |
| TC-11 | Two users save simultaneously | possible duplicate `CustomerCode` (no lock) — verify | 004 |
| TC-12 | Upload shop image then save | image not stored | GAP-003 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/CustomerEntry.aspx(.cs)`, `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs`, `Library.DAL/DataManager/DataAccessManager.cs`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`.

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | — | — | MISSING | — |
| Required fields | None existed (asterisks imply ≥10 required) | 4 enforced | — | MISMATCH vs UI hints | BR-001 |
| Approval of new customer | None existed | auto-approved | ActionStatus '2' | INFO | BR-005 |
| Customer code | None existed | SP-generated `C<n>` | column value | INFO | BR-004 |
| Images / Pharma platform | UI implies saved | not saved | — | GAP | GAP-003/004 |

## 30. Known Gaps
1. Concurrency: code = MAX+1 without lock/serialisable isolation ⇒ duplicates possible (INFERRED). `CONVERT(INT,…)` fails for any approved code whose suffix is non-numeric; first ever code would be NULL (`'C'+NULL`).
2. Images never persisted; `ImageToBase64ShopImg` has a JS error.
3. Pharma Platform, DC (on update) not saved. Station-type dropdowns are decorative (overwritten by SP).
4. `sp_Update_CustomerMaster` runs `DELETE FROM tblCustProductLine` before knowing the form's list (all-or-nothing by design).
5. Dead role restriction; asterisks without validation; label/column mismatches (Referred Doctor→TradeLicense).
6. Swallowed exceptions on load (`catch {}`) and unhandled exceptions on save.
7. Duplicate `SaveInfo` implementations in three DAL classes.
See `GAPS-M05.md` (GAP-M05-001 … ).

## 31. Change Impact
- `sp_Save_CustomerMaster` / `sp_Update_CustomerMaster` are also called from `OrderTrackingDAL.cs` and `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs` (used by TransferUI pages) — a change affects them (grep verified).
- `sp_Get_CustMasterList_Approve` list consumers: PAGE-167, PAGE-169, PAGE-173.
- `tblCustMaster` is read by ~40 DAL classes (orders, invoices, payments, returns) — column changes have wide impact.
- `IVMarketStructure.ascx` is shared with PAGE-167/173/169 and other MasterSetup pages.
