---
page_id: PAGE-173
module: M05
page: CustomerView
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-173 — CustomerView ("Customer List")

> **Evidence level:** Tier A. `.aspx`, `.aspx.cs` (all 639 lines), `CustomerInfoDAL.GetCustomerList`, `CommonDataLoad.GetPermissionForUserRole`, and the list SP `sp_Get_CustMasterList_Approve` (body read, `LIVE_DB…sql:821-874`) were read. The approval-queue SP `sp_Get_CustomerApp` (body read, `:222-393`) is documented here because it is the other half of the customer status picture, even though it is called from `Approval_UI/CustomerApproveList.aspx` (PAGE-695, approval-workflow module).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-173 |
| Module | M05 |
| Page | CustomerView (heading "Customer List") |
| Kind | Searchable list + CSV export + edit launcher |
| Folder / master | `MasterSetup_UI` / `~/MasterPages/NewMasterPage.master` |
| Class | `MasterSetup_UI_CustomerView` |
| Siblings | `CustomerListPending.aspx` (PAGE-169: same list, edit opens `CustomerEntry.aspx?MID=` in same window); legacy lists `CustomerMasterView`, `CustMasterView`, `ActiveCustomerMasterList` (SInventory_UI) |

## 2. Purpose
Search and browse customers (with approval status, sales persons, geography, provider type, platform) and open a customer for editing in `CustomerEntry`; export the current filter to CSV. CONFIRMED (`CustomerView.aspx.cs:308-328,425-445,580-630`).

## 3. Business Objective
Give sales-admin users one place to find any customer (by code/name/mobile/location/salesperson) and its verification status, and to reach the edit form. INFERRED from columns and filters.

## 4. Navigation
Menu DB-driven (UNKNOWN). "Add" (`EmpCetegoryAddImageButton`) → `CustomerEntry.aspx`; row edit → `window.open('CustomerEntry.aspx?mid=<id>','_blank')`; Reset → self. Inbound: `CustomerEntry` redirects here after save.

## 5. Route
`~/MasterSetup_UI/CustomerView.aspx`.

## 6. Source Files
`Solution.Web/MasterSetup_UI/CustomerView.aspx(.cs)`; `IVMarketStructure.ascx` (kept **invisible**: `IVMarketStructure.Visible=false`, `:64`); `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:405-424`; `SeedDataDAL`, `CommonDataLoad`, `BonusCampaignNewDAL.GetMobileSyncStatus`; autocomplete `SInventoryWebService.GetCustomer_ALL_new`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| `ddlChemisType` | Customer Type | no | first | `GetChemistTypeList` |
| `ddlSalesPerson` | Sales Person | no | first | `CommonDataLoad.GetDoctorSalesPerson_forSearch` → `sp_GetDoctor_SalesPerson_forSearch` |
| `ddlStatus` | Status | no | **Active** (items: Select Status / Active=1 / Inactive=0) | filters `mas.IsActive` |
| `custNameTextBox` + `hfCustomerId` | Customer | no | — | autocomplete text `Code : Name : Cell |Id`; `TextChanged` extracts id after last `|` |
| `ddlDistributionCenter` | DC | no | first; DIC role preset+disabled; DZSM and AM roles disabled (`:192-207`) | filters `dcMas.DCId` |
| `ddlProgramType` | Provider Type | no | first | `GetProgramTypeListParam(" and GRP.IsCustomer=1")` (bound twice) |
| `ddlPharmaPlatform` | Pharma Platform | no | first | `GetSMCTypeListParam(" and GRP.forCustomer=1")` |
| `ddlApprovalStatus` | Approval Status | no | first | `GetApprovalStatusList` (`SoftwareUseId`/`WebShow`) |
| `frmDate` / `toDate` | Create From/To Date | no | last 30 days → today (`dd MMMM, yyyy`) | filters `CONVERT(date,mas.CreateDate)` |
| `IVMarketStructure` | market structure | — | invisible | its dropdowns never post ⇒ geography filters never apply |
| `btnSearch` / `resetBtn` / `btnExportToExcel` | Search / Reset / Export | — | — | — |
| `EmpCetegoryAddImageButton` | Add | — | visible unless permission says otherwise | — |
| `lblCount` | "Total : n" | — | — | — |
| `loadGridView` (paged) | Customer Code, Customer Name, Salaes Person Name (sic), Owner Name, Mobile NO, Address, Approval Status, Entry Date + hidden columns (BSP/GSP code, Zone/Area/Territory/Market, Division/District/Thana, Customer Type, Provider Type, Pharma Platform, MIO Tour Type, Dept Tagging) + Edit column | — | — | `LinkButton1` `CommandName="EditData"`, `CommandArgument=CustomerMasterId` |
| sync modal `#syncModal` + `btnSyncNow` | mobile sync notice | — | — | shown when `usp_CheckCampaignEntryDate` returns `Result="yes"`; `btnSyncNow_Click` only hides modal (no sync logic) |

## 8. Field Mapping
Read-only page. Filter → SQL fragment appended to `@Parm` (`Parm()` `:457-562`):
| Control | Fragment |
|---|---|
| ddlDistributionCenter | `AND dcMas.DCId='v'` |
| Group/Zone/Area/Territory/SubTerritory/Market (invisible) | `gr.GroupId`, `rg.RegionId`, `Ar.AreaId`, `Tr.TerritoryId`, `subTr.SubTerritoryId`, `mas.MarketId` |
| ddlProgramType | `mas.ProgramTypeId='v'` |
| ddlPharmaPlatform | `mas.SMCTypeId='v'` |
| ddlChemisType | `mas.CustomerTypeId='v'` |
| ddlSalesPerson | `(mas.SalesPersonId='v' OR (','+ISNULL(mas.TaggedSalesPersonIds,'')+',') LIKE '%,v,%')` |
| hfCustomerId | `mas.CustomerMasterId='v'` |
| else custNameTextBox | contains ':' ⇒ `(mas.CustomerCode LIKE '%code%' OR mas.CustomerName LIKE '%rest%')`; otherwise same with the whole text (**free text concatenated**) |
| ddlApprovalStatus | `mas.ActionStatus='v'` |
| ddlStatus | `mas.IsActive=v` |
| frmDate/toDate | `CONVERT(date,mas.CreateDate) BETWEEN 'a' AND 'b'` / `>=` / `<=` (text from textbox concatenated) |
Grid columns come from the SP result (see §16).

## 9. Dropdown Mapping
As §7. All list SPs' bodies are NOT in repo except `sp_Get_CustMasterList_Approve`. Approval-status text: `sp_GET_ActionStatusList` (UNKNOWN body) — the SP itself shows Pending/Verified/Approved/Rejected for `ActionStatus` 0/1/2/3.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | Effect |
|---|---|---|---|---|
| btnSearch | `btnSearch_Click` `:448` | `GetCustomerList(Parm())` | `sp_Get_CustMasterList_Approve` | grid |
| Edit (row) | `loadGridView_RowCommand "EditData"` `:425` | — | — | opens `CustomerEntry.aspx?mid=` |
| btnExportToExcel | `:580` | `LoadData(Parm())` with paging off | same | CSV `Customer_List_<timestamp>.csv` (visible columns only; requires the grid to already contain rows) |
| Add | redirect | — | — | `CustomerEntry.aspx` |

## 11. Frontend Flow
First load: `CheckSyncStatus` (modal), `UserPersmissionValidation`, default dates, bind dropdowns (each in `try/catch`). Search → postback; `PerVali()` always returns true (role-based mandatory filters DZSM/AM/MIO are commented out `:365-423`), so a blank search returns **all customers created in the date window**. Paging via `loadGridView_PageIndexChanging` re-runs the query. Export disables paging and re-runs, writes CSV to the response (`Response.End()`).

## 12. Backend Flow
`LoadData` → `_DAL.GetCustomerList(parm)` (`DataAccessManager.GetDataTable("sp_Get_CustMasterList_Approve",@Parm)`) → bind. Permission (first load only, non-super-admin): `Session["UserRoleID"] != "2"` ⇒ `CommonDataLoad.GetPermissionForUserRole(<page url>)` (`sp_GET_MainPermissionByUserRoleandPageUrl(@RoleId,@PageName)`): row present ⇒ Add button visibility = `RAdd`, last grid column (Edit) visibility = `REdit`; no row or any exception ⇒ `Response.Redirect("../Dashboard_UI/DashboardOne.aspx")`.

## 13. API Mapping
Autocomplete web method `GetCustomer_ALL_new` (see PAGE-167 §13). No page methods.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`CustomerInfoDAL.GetCustomerList`; `SeedDataDAL.*`; `CommonDataLoad.{GetDoctorSalesPerson_forSearch,GetPermissionForUserRole}`; `BonusCampaignNewDAL.GetMobileSyncStatus` (`usp_CheckCampaignEntryDate`).

## 16. Stored Procedure Mapping
**`sp_Get_CustMasterList_Approve(@Parm)` (READ `LIVE_DB…sql:821-874`)** — dynamic SQL, `'… from dbo.tblCustMaster mas … where mas.CustomerMasterId is not null '+@Parm+' order by CustomerCode desc'`. Output columns added to `mas.*`: `CustomerBsPCode`, `Dept` (`ComUnitCode : ComUnitName` of the market's route DC), `StationTypeName` (MIO station type), Division/District/Thana names, `SMCType`, `Address` cleaned by `dbo.Remove_SpecialCharacters`, `CustomerSearch` = `Code : Name[ (Inactive)] : Cell`, `EmpMasterCode` (creator's `Code : Name` via `CreateBy`→`tblUser`→`tblEmpGeneralInfo`), **`SalaesPersonName`** = comma list of all `EmpMasterCode : EmpName` whose `EmpInfoId` is in `TaggedSalesPersonIds` (fallback: `SalesPersonId` employee), Region/Area/Territory codes, `ApprovalStatus` label (`0 Pending, 1 Verified, 2 Approved, 3 Rejected`, else raw), `ProgramTypeName`, `MarketCode/MarketName`, `CustomerType`, `DistributionRouteName` (= route master `RouteName`), `EntryDate` (`dd MMM yyyy hh:mm tt`).
**`sp_Get_CustomerApp` (READ `:222-393`, used by Approval_UI list):** dynamic SQL; always excludes `ActionStatus IN ('2','3')`; `@AppStatus` matches latest approval-log `Status` or (no log) `ActionStatus`; date filter on log entry date or `CreateDate`; optional `TerritoryId`; `DupCell` CTE flags `ExistenceStatus='Already Exists'` when the same non-empty `CellNo` occurs on more than one non-rejected (`ActionStatus<>'3'`) customer; joins latest `tblCustomerApprovalLog` step (`MaxStep`) for `Status/Step/WaitingForRole/…`; `@Role`, `@param`, `@AppStatus` are concatenated into the SQL string.
Other SPs: `sp_GET_MainPermissionByUserRoleandPageUrl`, `usp_CheckCampaignEntryDate`, seed SPs — bodies NOT in repo.

## 17. Database Mapping
Read only. `tblCustMaster` plus `tblCustomerType`, `tblProgramType`, `tblUser`, `tblEmpGeneralInfo`, `tblMarket`, `tblMarketStationDetail`/`tblStationType`, `tblRouteInformationMarketDetail`/`tblRouteInformationMaster`/`tblCompanyUnit`, `tbl_Thana/District/Division`, `tblSubTerritory/Territory/Area/Region`, `tbl_Group`, `tblSMCType` (+ `tblCustomerApprovalLog` for the approval SP).

## 18. Validation Matrix
No validation. `PerVali()` returns true unconditionally. Autocomplete: text without `|` clears `hfCustomerId` (falls back to LIKE search). Export with empty grid ⇒ "No Data Found!".

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-045 | Default window = customers created in the last 30 days (`frmDate = today-30`, `toDate = today`); a search with no other filter returns every customer created in that window (no mandatory scope by role/territory). | `CustomerView.aspx.cs:80-81,448-455`; `:365-423` (commented rules) | page | CONFIRMED |
| BR-M05-046 | Add/Edit visibility follows role permissions (`RAdd`, `REdit` from `sp_GET_MainPermissionByUserRoleandPageUrl`) except for `UserRoleID==2` (treated as full admin); missing permission row ⇒ redirect to dashboard. Evaluated on first load only. | `:332-364` | `UserPersmissionValidation` | CONFIRMED |
| BR-M05-047 | Customer status text is derived from `tblCustMaster.ActionStatus`: `'0'`→Pending, `'1'`→Verified, `'2'`→Approved, `'3'`→Rejected; the list shows all statuses (incl. rejected) and filters by the raw code. | `LIVE_DB…sql:839-840` | `sp_Get_CustMasterList_Approve` | CONFIRMED |
| BR-M05-048 | Duplicate-customer detection is only informational and only in the approval queue: same non-empty mobile number on >1 customer with `ActionStatus<>'3'` ⇒ "Already Exists" flag; pending queue excludes statuses 2 and 3. | `LIVE_DB…sql:272-278,235,281` | `sp_Get_CustomerApp` | CONFIRMED |
| BR-M05-049 | The sales-person filter matches customers whose primary `SalesPersonId` equals the selection **or** whose `TaggedSalesPersonIds` comma list contains it; the displayed salesperson column lists all tagged persons (fallback: primary). | `CustomerView.aspx.cs:512-516`; `LIVE_DB…sql:830-838` | page + SP | CONFIRMED |

## 20. Permission Matrix
Enforced: `Session["UserRoleID"]` (==2 bypass; otherwise `RAdd`/`REdit` from DB) and redirect to dashboard when no permission row (first load only). `Session["RoleTypeName"]` DIC/DZSM/AM only disables the DC dropdown — data is **not** scoped by role/territory/DC (`PerVali()` and DC-scoping are neutralised). `Session["EmpInfoId"]`/`["RoleTypeId"]` are read into fields but unused. CONFIRMED.

## 21. Status Lifecycle
Read-only presentation of `ActionStatus` 0/1/2/3 and `IsActive`. Transitions are performed elsewhere (see PAGE-168 §21; approval flow PAGE-695).

## 22. Error Handling
Each initialiser has `catch (Exception ex) {}`; permission failure ⇒ dashboard redirect; SP failure on search ⇒ unhandled exception (no try/catch in `LoadData`). Export writes CSV and `Response.End()`.

## 23. Audit
None (read-only). Column `EmpMasterCode` shows the creator.

## 24. Integration
`usp_CheckCampaignEntryDate` drives a "mobile sync" modal (semantics UNKNOWN); no sync action implemented.

## 25. Reports
CSV export (RPT-M05-03).

## 26. Dependencies
`IVMarketStructure`, AjaxControlToolkit, select2/SweetAlert JS, `Newtonsoft.Json` (imported, unused here).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open page as super-admin (UserRoleID=2), Search | customers created in last 30 days | 045,046 |
| TC-02 | Open as role without a permission row | redirect to `DashboardOne.aspx` | 046 |
| TC-03 | Role with `REdit=false` | Edit column hidden | 046 |
| TC-04 | Filter Approval Status = Approved | only `ActionStatus='2'` rows; label "Approved" | 047 |
| TC-05 | Filter Sales Person X | customers where X is primary or tagged | 049 |
| TC-06 | Enter `%' OR 1=1--` in customer text box, Search | unsafe concatenation: verify injection (security test) | GAP-009 |
| TC-07 | DIC user | DC dropdown disabled, list not DC-filtered | 046 |
| TC-08 | Export with paging | all rows exported (paging disabled) | — |
| TC-09 | Two customers same mobile (pending) in approval queue | flagged "Already Exists" | 048 |
| TC-10 | Customer with 2 tagged salespersons | `SalaesPersonName` shows both | 049 |

## 28. Reverse Engineering References
Files in §6; `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:222-393,821-874`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Role scoping | UI implies (DIC/DZSM/AM disabling) | not applied to data | MISMATCH |
| Market-structure filter | UI control exists | hidden ⇒ never applied | MISMATCH |

## 30. Known Gaps
- Free-text customer search and date textboxes concatenated into SQL (`:531,535,550-558`) ⇒ injection; SP appends `@Parm` raw (`sp_Get_CustMasterList_Approve`) and `sp_Get_CustomerApp` concatenates `@AppStatus/@Role/@param`.
- Role scoping and mandatory filters disabled (commented) ⇒ any user can list all customers.
- `ORDER BY CustomerCode desc` is a string sort (`C99` > `C100`).
- List SP double-joins market→route tables (possible duplicate rows) and returns duplicate column names (`MarketName` alias vs `*`).
- Export requires the grid to have rows (uses current filter only); `Response.End()` in handler.
- Mobile sync modal has no action. GAP-M05-009/010/026.

## 31. Change Impact
`sp_Get_CustMasterList_Approve` is used by PAGE-167/168 (link)/169/173 and `MarketStructureTransferDAL`; changing its columns breaks their BoundFields. `CustomerView` links to `CustomerEntry`. `GetPermissionForUserRole` is shared by many list pages.
