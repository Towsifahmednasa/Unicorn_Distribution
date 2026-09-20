# M03 — Organisation & Market Structure Master

Reviewed 2026-09-19. Evidence: source in `Solution.Web`, `Library.DAL`, `Library.DAO`. **No stored-procedure body for this module exists in the repo**, so table/column effects are `UNKNOWN` throughout; `INFERRED` marks conclusions drawn from call shapes. Tier-A pages (14): 038, 042, 052, 057, 062, 064, 075, 094, 095, 096, 098, 099, 101, 102.

## 1. Purpose & business overview
Maintains the organisational backbone used by the field-force/distribution ERP:
- **Sales hierarchy** (as coded): `Group` (screens titled "National") → `Zone` (DB/code name `Region`; same level) → `Area` → `Territory` → `Sub-Territory` → `Market`. Roles bound to levels (INFERRED from control scoping): NSM=Group, DZSM=Zone, AM=Area, MIO=Territory. There is no extra "Region" tier between Zone and Area.
- **Geography**: Division → District → Thana/Upazila (each Market points to a Thana).
- **Distribution structure**: Sales Center / Depot (= `tblCompanyUnit`, "DC"), routes (`RouteInformation*`, legacy `Router*`), territory-wise depot mapping.
- **Structure change control**: transfer requests (5 levels) + approval; bulk market import + approval.
- **Financial year** master and year-end opening/purge.
- Legacy `SInventory_UI` company/region/zone/area/district/market/department pages (older BLL+inline-SQL model).

## 2. Market-structure user controls [MS-CTRL]
Documented once here; pages reference "[MS-CTRL]".

### 2.1 Control family
All controls expose the same server IDs so pages fetch them with `FindControl`: DropDownLists `GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect` (all `AutoPostBack`, CSS `mySelect2`) and HiddenFields `hfGroupId, hfZone, hfArea, hfTeritory, hfSubTeritory, hfMarket` (used by a host page to preselect values). Labels: Group, Zone, Area, Territory, Sub-Territory, Market. Placeholder item "Please Select From List" (value "").

| Control (Solution.Web/…) | Used by (M03) | Visible rows | Role scoping |
|---|---|---|---|
| `MasterSetup_UI/IVMarketStructure.ascx` | PAGE-062 route entry | all 6 | none |
| `MasterSetup_UI/IVMarketStructureArea.ascx` | PAGE-064 | Group, Zone, Area (Territory/Sub/Market `visible=false`) | none |
| `MasterSetup_UI/IVMarketStructureMarket.ascx` | PAGE-042 | Group…Sub-Territory (Market hidden) | AM/DZSM/NSM |
| `MasterSetup_UI/IVMarketStructureMarketForSubTeriSetup.ascx` | PAGE-057 | Group…Territory | AM/DZSM/NSM |
| `MasterSetup_UI/IVMarketStructureFrom_To.ascx`, `IVMarketStructureTo.ascx` (adds `ddlDistributionCenter`, `rootDropDownList` "Route") | PAGE-099 (hidden block) | – | AM/MIO/DZSM/NSM |
| `MasterSetup_UI/IVMarketStructureSearch/ForTeritory/InvoSearch/FS/StforAll.ascx` | other modules (reports, filters) | varies | InvoSearch/FS add NSM/DZSM/AM/MIO name pickers via `*_Rpt` loaders |
| `TransferUI/IVMarketStructureFrom_ToZoneTranfer.ascx` | PAGE-102 | Group only | AM/DZSM/NSM |
| `…FromToAreaTranfer.ascx` | PAGE-095 | Group, Zone | same |
| `…FromToTeriiTransfer.ascx` | PAGE-101 | Group, Zone, Area | same |
| `…FromToSubTeriiTransfer.ascx` | PAGE-100 | Group…Territory | same |
| `…FromToTranser.ascx` | PAGE-096 | Group…Sub-Territory | same |
Each transfer page registers the same `.ascx` twice (`IVMarketStructure` = From, `IVMarketStructureTo` = To).

### 2.2 Cascade logic (BR-M03-001)
`Page_Load` (first load) → `LoadInitialDDL()`: `GroupSelect` ← `CommonDataLoad.GetGroupInfo_Active` (`sp_CS_Group_Active`; on exception only the placeholder). Selecting a level (`*_SelectedIndexChanged`) binds the next level with **Active-only** loaders and clears all lower levels:

| Change | Loader → SP (params) | Value/Text |
|---|---|---|
| Group | `GetZone_byGroupId_Active` → `sp_Get_Zone_All_Active(@GroupId)` | RegionId/RegionName |
| Zone | `GetArea_ByZoneId_Active` → `sp_CS_GetArea_ByZoneId_Active(@id)` | AreaId/AreaName |
| Area | `GetTerritory_ByAreaId_Active` → `sp_CS_GetTerritory_ByAreaId_Active(@id)` | TerritoryId/TerritoryName |
| Territory | `GetSubTerritory_ByTerritoryId_Active` → `sp_CS_GetSubTerritory_ByTerritoryId_Active(@id)` | SubTerritoryId/SubTerritoryName |
| Sub-Territory | `GetMarket_BySubTerritoryId_Active` → `sp_CS_GetMarket_BySubTerritoryId_Active(@id)` | MarketId/MarketName |
Errors are swallowed (empty `catch`). Hidden rows still exist (`visible=false` removes rendering only).

### 2.3 Preload (edit) mode (BR-M03-003)
If the host page set `hfGroupId` (page `Page_Load` runs before the control's), the control binds **All** (incl. inactive) lists: `sp_CS_Group_All`, `sp_Get_Zone_AllByGroup`, `sp_CS_GetArea_ByZoneId_All`, `sp_CS_GetTerritory_ByAreaId_All`, `sp_CS_GetSubTerritory_ByTerritoryId_All`, `sp_CS_GetMarket_BySubTerritoryId_All`, then sets `SelectedValue` from each hidden field (throws if value absent from list — not caught).

### 2.4 Role scoping (BR-M03-002)
Session keys `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (try/catch on read). `GetHigharcyInfoByEmployeeId` → `sp_HigharchyInfoByEmployeeId(@EmployeeId,@RoleId)`; `if (EmpInfoId != "" || EmpInfoId != null)` is always true. Transfer/Setup variants: `AM` → hf Group, Zone, Area set and those three dropdowns `Enabled=false`; `DZSM` → Group, Zone locked; `NSM` → Group locked; other roles unrestricted. `From_To`/`StforAll` variants also `MIO` (locks Group, Zone, Area; presets Territory) and, for DZSM/AM/NSM/MIO, build id lists (`areaId` CSV) and load restricted lists via `GetZone_byGroupId_forDSM` / `GetArea_ByZoneId_AllForAMOnly`. `dtMarket.Rows[0]` is read unguarded → a scoped role with no hierarchy row breaks the host page. Consequence: an AM/DZSM/NSM user can only choose their own hierarchy for both From and To in transfer pages (INFERRED).

## 3. Transfer semantics (hierarchy re-parenting)
Workflow detail: `workflows/WF-M03-01-…`, `WF-M03-02-…`. Summary:

| Page | Moves | From → list | To required | Data sent to `sp_Update_MarketStructure_Transfer` |
|---|---|---|---|---|
| Zone_Transfer (102) | Zones → new Group | Group → active zones | Group | `@Type=Zone,@TRegionId=id,@TGroupId` |
| Area_Transfer (095) | Areas → new Zone | Zone → active areas | Zone | `@Type=Area,@TAreaId=id,@TGroupId,@TRegionId=<hidden To-Area>` (**dest Zone dropped**) |
| Territory_Transfer (101) | Territories → new Area | Area → active territories | Area | `@TTerritoryId=id,@TGroupId,@TRegionId,@TAreaId` |
| SubTerritory_Transfer (100) | Sub-Territories → new Territory | Territory → active subs | Territory | `@TSubTerritoryId=id` + Group…Territory |
| Market_Transfer (096) | Markets → new Sub-Territory | Sub-Territory → active markets | Sub-Territory | `@TMarketId=id` + Group…SubTerritory |
| MarketStructure_TransferApprove (099) | approves per type (Market/Sub-Territory/Territory/Area/Zone) | `sp_Get_MSList_Approve(@Parm=type)` | – | `sp_Update_MarketStructure_Approve(@MasterId,@ApprovedBy,@Type)` per row |
Common: one SP call per ticked item, all in one implicit DB transaction; source ids (`@F*`) always sent; `@EntryBy`=UserId. Status lifecycle (INFERRED): `Requested → Approved`; no reject; requester may approve; "Effective Date" on approval page is unused. What data is actually re-parented (customers, doctors, MIO tags, routes, orders) is **UNKNOWN**.

## 4. Page catalogue (75 pages)
Kind from Tier-B extraction. Tier B specs remain mechanically generated (`generated: true`) except as noted.

| Page ID | Page | Folder | Kind | Tier | Spec |
|---|---|---|---|---|---|
| PAGE-028 | AMRecords | DoctorModule_UI | List (AM) | B | [spec](pages/PAGE-028-amrecords.md) |
| PAGE-029 | AMSetup | DoctorModule_UI | Setup: AM ↔ area | B | [spec](pages/PAGE-029-amsetup.md) |
| PAGE-030 | AMSetupEdit | DoctorModule_UI | Edit | B | [spec](pages/PAGE-030-amsetupedit.md) |
| PAGE-031 | AreaRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-031-arearecords.md) |
| PAGE-032 | AreaSetup | DoctorModule_UI | Setup (JS→Setup.aspx/SaveArea) | B | [spec](pages/PAGE-032-areasetup.md) |
| PAGE-033 | AsmRecords | DoctorModule_UI | List (ASM/FE) | B | [spec](pages/PAGE-033-asmrecords.md) |
| PAGE-034 | AsmSetup | DoctorModule_UI | Setup | B | [spec](pages/PAGE-034-asmsetup.md) |
| PAGE-035 | DepotWiseAreaSetup | DoctorModule_UI | Setup: depot↔area | B | [spec](pages/PAGE-035-depotwiseareasetup.md) |
| PAGE-036 | DZSMRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-036-dzsmrecords.md) |
| PAGE-037 | DZSMSetup | DoctorModule_UI | Setup | B | [spec](pages/PAGE-037-dzsmsetup.md) |
| PAGE-038 | FinancialYearDeleteTableEntry | DoctorModule_UI | Year-end opening/purge | **A** | [spec](pages/PAGE-038-financialyeardeletetableentry.md) |
| PAGE-039 | FinancialYearEntry | DoctorModule_UI | Entry | B | [spec](pages/PAGE-039-financialyearentry.md) |
| PAGE-040 | FinancialYearView | DoctorModule_UI | List/toggle | B | [spec](pages/PAGE-040-financialyearview.md) |
| PAGE-041 | MarketRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-041-marketrecords.md) |
| PAGE-042 | MarketSetup | DoctorModule_UI | Market master | **A** | [spec](pages/PAGE-042-marketsetup.md) |
| PAGE-043 | MioRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-043-miorecords.md) |
| PAGE-044 | MioSetup | DoctorModule_UI | Setup | B | [spec](pages/PAGE-044-miosetup.md) |
| PAGE-045 | MioUpdateInfo | DoctorModule_UI | Page | B | [spec](pages/PAGE-045-mioupdateinfo.md) |
| PAGE-046 | NationalSetup | DoctorModule_UI | Group setup | B | [spec](pages/PAGE-046-nationalsetup.md) |
| PAGE-047 | NationalSetupView | DoctorModule_UI | List | B | [spec](pages/PAGE-047-nationalsetupview.md) |
| PAGE-048 | NSMHeadRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-048-nsmheadrecords.md) |
| PAGE-049 | NSMHeadSetup | DoctorModule_UI | Setup | B | [spec](pages/PAGE-049-nsmheadsetup.md) |
| PAGE-050 | NSMRecords | DoctorModule_UI | List (region head) | B | [spec](pages/PAGE-050-nsmrecords.md) |
| PAGE-051 | NSMSetup | DoctorModule_UI | Setup (region head) | B | [spec](pages/PAGE-051-nsmsetup.md) |
| PAGE-052 | RouterSetupEntry | DoctorModule_UI | Legacy route (likely broken) | **A** | [spec](pages/PAGE-052-routersetupentry.md) |
| PAGE-053 | RouterSetupView | DoctorModule_UI | List | B | [spec](pages/PAGE-053-routersetupview.md) |
| PAGE-054 | RsmRecords | DoctorModule_UI | List (DZSM) | B | [spec](pages/PAGE-054-rsmrecords.md) |
| PAGE-055 | RsmSetup | DoctorModule_UI | Setup (DZSM) | B | [spec](pages/PAGE-055-rsmsetup.md) |
| PAGE-056 | SubTerritoryRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-056-subterritoryrecords.md) |
| PAGE-057 | SubTerritorySetup | DoctorModule_UI | Sub-Territory master | **A** | [spec](pages/PAGE-057-subterritorysetup.md) |
| PAGE-058 | TerritoryRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-058-territoryrecords.md) |
| PAGE-059 | TerritorySetup | DoctorModule_UI | Setup | B | [spec](pages/PAGE-059-territorysetup.md) |
| PAGE-060 | ZoneRecords | DoctorModule_UI | List | B | [spec](pages/PAGE-060-zonerecords.md) |
| PAGE-061 | ZoneSetup | DoctorModule_UI | Setup | B | [spec](pages/PAGE-061-zonesetup.md) |
| PAGE-062 | RouteInformationEntry | MasterSetup_UI | Route master | **A** | [spec](pages/PAGE-062-routeinformationentry.md) |
| PAGE-063 | RouteInformationList | MasterSetup_UI | List (role-scoped) | B | [spec](pages/PAGE-063-routeinformationlist.md) |
| PAGE-064 | TerritoryWiseDepotSetup | MasterSetup_UI | Depot↔territory | **A** | [spec](pages/PAGE-064-territorywisedepotsetup.md) |
| PAGE-065 | TerritoryWiseDepotView | MasterSetup_UI | List | B | [spec](pages/PAGE-065-territorywisedepotview.md) |
| PAGE-066 | AreaEdit | SInventory_UI | Legacy edit | B | [spec](pages/PAGE-066-areaedit.md) |
| PAGE-067 | AreaEntry | SInventory_UI | Legacy entry | B | [spec](pages/PAGE-067-areaentry.md) |
| PAGE-068 | AreaView | SInventory_UI | Legacy list | B | [spec](pages/PAGE-068-areaview.md) |
| PAGE-069 | CompanyInfoEdit | SInventory_UI | Edit (tblCompanyInfo) | B | [spec](pages/PAGE-069-companyinfoedit.md) |
| PAGE-070 | CompanyInfoEntry | SInventory_UI | Entry | B | [spec](pages/PAGE-070-companyinfoentry.md) |
| PAGE-071 | CompanyInfoView | SInventory_UI | List | B | [spec](pages/PAGE-071-companyinfoview.md) |
| PAGE-072 | CompanyUnitEdit | SInventory_UI | Edit sales center | B | [spec](pages/PAGE-072-companyunitedit.md) |
| PAGE-073 | CompanyUnitEntry | SInventory_UI | Entry sales center | B | [spec](pages/PAGE-073-companyunitentry.md) |
| PAGE-074 | CompanyUnitView | SInventory_UI | List | B | [spec](pages/PAGE-074-companyunitview.md) |
| PAGE-075 | CompanyWiseBranch | SInventory_UI | Deposit slip entry (misfiled) | **A** | [spec](pages/PAGE-075-companywisebranch.md) |
| PAGE-076 | DepartmentEdit | SInventory_UI | Edit | B | [spec](pages/PAGE-076-departmentedit.md) |
| PAGE-077 | DepartmentEntry | SInventory_UI | Entry | B | [spec](pages/PAGE-077-departmententry.md) |
| PAGE-078 | DepartmentView | SInventory_UI | List | B | [spec](pages/PAGE-078-departmentview.md) |
| PAGE-079 | DistrictEdit | SInventory_UI | Legacy edit | B | [spec](pages/PAGE-079-districtedit.md) |
| PAGE-080 | DistrictEntry | SInventory_UI | Legacy entry | B | [spec](pages/PAGE-080-districtentry.md) |
| PAGE-081 | DistrictView | SInventory_UI | Legacy list | B | [spec](pages/PAGE-081-districtview.md) |
| PAGE-082 | MarketInfoEdit | SInventory_UI | Legacy edit | B | [spec](pages/PAGE-082-marketinfoedit.md) |
| PAGE-083 | MarketInfoEntry | SInventory_UI | Legacy entry | B | [spec](pages/PAGE-083-marketinfoentry.md) |
| PAGE-084 | MarketInfoView | SInventory_UI | Legacy list | B | [spec](pages/PAGE-084-marketinfoview.md) |
| PAGE-085 | RegionEdit | SInventory_UI | Legacy edit | B | [spec](pages/PAGE-085-regionedit.md) |
| PAGE-086 | RegionEntry | SInventory_UI | Legacy entry | B | [spec](pages/PAGE-086-regionentry.md) |
| PAGE-087 | RegionView | SInventory_UI | Legacy list | B | [spec](pages/PAGE-087-regionview.md) |
| PAGE-088 | ZoneEntry | SInventory_UI | Legacy entry (tblZone) | B | [spec](pages/PAGE-088-zoneentry.md) |
| PAGE-089 | ZoneView | SInventory_UI | Legacy list | B | [spec](pages/PAGE-089-zoneview.md) |
| PAGE-090 | District_Entry | Thana_UI | District setup | B | [spec](pages/PAGE-090-district-entry.md) |
| PAGE-091 | District_View | Thana_UI | List | B | [spec](pages/PAGE-091-district-view.md) |
| PAGE-092 | Division_View | Thana_UI | List | B | [spec](pages/PAGE-092-division-view.md) |
| PAGE-093 | Thana_View | Thana_UI | List | B | [spec](pages/PAGE-093-thana-view.md) |
| PAGE-094 | ThanaEntry | Thana_UI | Thana setup | **A** | [spec](pages/PAGE-094-thanaentry.md) |
| PAGE-095 | Area_Transfer | TransferUI | Transfer request | **A** | [spec](pages/PAGE-095-area-transfer.md) |
| PAGE-096 | Market_Transfer | TransferUI | Transfer request | **A** | [spec](pages/PAGE-096-market-transfer.md) |
| PAGE-097 | MarketInfoApprove | TransferUI | Approval (import batches) | B | [spec](pages/PAGE-097-marketinfoapprove.md) |
| PAGE-098 | MarketInfoImport | TransferUI | Excel import | **A** | [spec](pages/PAGE-098-marketinfoimport.md) |
| PAGE-099 | MarketStructure_TransferApprove | TransferUI | Approval | **A** | [spec](pages/PAGE-099-marketstructure-transferapprove.md) |
| PAGE-100 | SubTerritory_Transfer | TransferUI | Transfer request | B (documented in WF-M03-01) | [spec](pages/PAGE-100-subterritory-transfer.md) |
| PAGE-101 | Territory_Transfer | TransferUI | Transfer request | **A** | [spec](pages/PAGE-101-territory-transfer.md) |
| PAGE-102 | Zone_Transfer | TransferUI | Transfer request | **A** | [spec](pages/PAGE-102-zone-transfer.md) |

## 5. Business-rule index
| IDs | Topic | Where |
|---|---|---|
| BR-M03-001…003 | structure-control cascade, role scoping, preload | §2; PAGE-102 |
| BR-M03-004…010 | Zone transfer (≥1 item, dest Group, active list, one SP call per item, last-result flag, no From≠To check) | PAGE-102 |
| BR-M03-011…013 | Area transfer (dest Zone; dest Zone dropped in DAL; source list) | PAGE-095 |
| BR-M03-014, 015 | Territory transfer | PAGE-101 |
| BR-M03-016, 017 | Market transfer | PAGE-096 |
| BR-M03-018 | Sub-Territory transfer (dest Territory; wording defects) | WF-M03-01 (`SubTerritory_Transfer.aspx.cs:99,199`) |
| BR-M03-019…026 | Transfer approval (≥1 row, empty list, per-row SP, unused effective date, no SoD, type radio, load rule, no reject) | PAGE-099 |
| BR-M03-027…031 | Sub-Territory setup | PAGE-057 |
| BR-M03-032…040 | Market setup (MIO/AM/DZSM station triple, dup, deactivation guard, customer refresh, `IsActive=="1"`) | PAGE-042 |
| BR-M03-041…048 | Territory-wise depot (area+depot required, pre-tick, "Order Pending!", index bug, company 1) | PAGE-064 |
| BR-M03-049…067 | Route information (required fields, distances, market uniqueness, list scoping) | PAGE-062 |
| BR-M03-068…077 | Financial-year opening/purge | PAGE-038 |
| BR-M03-078…083 | Thana setup | PAGE-094 |
| BR-M03-084…088 | Legacy router setup | PAGE-052 |
| BR-M03-089…094 | Deposit slip entry | PAGE-075 |
| BR-M03-095…100 | Market Excel import/approval | PAGE-098 |
| BR-M03-101 | `DataAccessManager`: one implicit `SqlTransaction` per DAL call (begins at first write, commits in `SqlConnectionClose()`, rollback+rethrow via `SqlConnectionClose(true)`) | `Library.DAL/DataManager/DataAccessManager.cs:122-170,404-445` — CONFIRMED |
| BR-M03-102 | No `<authorization>` in `Solution.Web/Web.config`; module pages enforce no role check — access = menu visibility + master-page login redirect | Web.config:80-81 (only `<authentication>`) — CONFIRMED |
| BR-M03-103 | Transfer/approve/depot/financial-year pages hold DAL objects in `static` fields while `DataAccessManager` keeps connection/transaction in instance fields → concurrent requests share connection state | e.g. `Zone_Transfer.aspx.cs:21`, `TerritoryWiseDepotSetup.aspx.cs:19`, `FinancialYearDeleteTableEntry.aspx.cs:8-9` — CONFIRMED (declaration) / INFERRED (impact) |

## 6. Status lifecycles
- Node active flag (all hierarchy levels): `IsActive` + `AcOrInAcDate`; deactivation guarded by `sp_check_Vali_MarketStructure`.
- Structure transfer request: `Requested → Approved` (INFERRED); no reject.
- Bulk market import: `Staged → Applied` (approve via `sp_Save_MarketPropToTable`).
- Financial-year processing: `(none) → Completed` opening (`tblOpeningBalanceFinancialYearLog.Status='Completed'`) gates purge.
- Deposit: `IsDelete` soft flag (other page, 60-day window).

## 7. Roles/permissions observed
Only Session keys `UserId` (most pages), `LoginName` (PAGE-075 EntryBy), `RoleTypeName/EmpInfoId/RoleTypeId` (structure controls), `RoleTypeId=="5"` and `DICUnitId` (PAGE-063 list scoping). Data scoping: `tblUserCompanyUnit` (depot pickers). No page checks that the caller may perform the action; PAGE-038 reads no session at all. Web-method endpoints on Thana/Router pages are anonymous-reachable for reads (INFERRED).

## 8. Data model & SPs that matter
Tables named in code: `tblCompanyUnit`, `tblUserCompanyUnit`, `tblCompanyInfo`, `tblRegion`, `tblZone`, `tblArea`, `tblDistrict`, `tblMarket`, `tblDepartment` (legacy), `tblFinancialYear`, `tblOpeningBalanceFinancialYearLog`, `tblBankInfoNew`, `tblCompanyWiseDeposit`. Modern hierarchy/route/transfer tables are SP-only and unnamed. Key SPs: cascade (`sp_CS_*`, `sp_Get_Zone_*`, `sp_HigharchyInfoByEmployeeId`), `sp_check_Vali_MarketStructure` (shared deactivation/pending guard), `sp_Update_MarketStructure_Transfer/_Approve`, `sp_Get_MSList_Approve`, `sp_Save/Update_*Info|*Data` per level, `sp_*RouteInformation*`, `sp_*DcWiseTerritory*`, `sp_Save_MarketProp*`, `sp_DeleteArchive*`, `sp_opeingBalanceCreate`.

## 9. Integrations
None external except Excel/OLEDB import (PAGE-098) and archive databases for year-end (PAGE-038). No SAP calls in this module.

## 10. Reports
None — see `reports/README.md`.

## 11. Workflows
- [WF-M03-01 transfer request](workflows/WF-M03-01-market-structure-transfer-request.md) · [WF-M03-02 approval](workflows/WF-M03-02-market-structure-transfer-approval.md) · [WF-M03-03 bulk market import](workflows/WF-M03-03-bulk-market-creation-upload-and-approval.md) · [WF-M03-04 node setup & deactivation](workflows/WF-M03-04-hierarchy-node-setup-and-deactivation.md) · [WF-M03-05 route setup](workflows/WF-M03-05-route-information-setup.md) · [WF-M03-06 depot mapping](workflows/WF-M03-06-territory-wise-depot-mapping.md) · [WF-M03-07 financial-year opening & purge](workflows/WF-M03-07-financial-year-opening-and-data-purge.md)

## 12. Known-gap summary
See [GAPS-M03.md](GAPS-M03.md). Headlines: all SP bodies missing; Area transfer drops destination Zone; no authorization/SoD; destructive year-end purge without confirmation/audit; arbitrary file save on Excel import; legacy Router page likely non-functional; `CompanyWiseBranch` is a misfiled deposit page; setup-page edits can re-parent nodes outside the transfer workflow.

## 13. Still unverified
Effect of every SP (esp. transfer/approve, route, depot, purge, opening); table names; Tier-B pages (60 pages, incl. AM/MIO/NSM/DZSM/ASM setup, legacy SInventory_UI pages, list pages) were not reviewed; PAGE-100 documented only via diff (WF-M03-01); relationship of legacy `tblRegion/tblZone/tblArea` to the SP-based hierarchy; menu placement and role grants (DB data); shared JS helpers (`sweetAlertConfirm_*`, `successalert`).
