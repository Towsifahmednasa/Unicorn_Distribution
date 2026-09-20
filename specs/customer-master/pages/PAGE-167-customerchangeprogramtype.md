---
page_id: PAGE-167
module: M05
page: CustomerChangeProgramType
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-167 — CustomerChangeProgramType ("Change Customer Provider Type")

> **Evidence level:** Tier A. `.aspx`, `.aspx.cs`, `CustomerInfoDAL.Update__ProgramTypeInfo` / `GetCustomerList`, and the list SP `sp_Get_CustMasterList_Approve` (body read, `LIVE_DB…sql:821-874`) were read. `sp_Update_CustomerProgramType` body is NOT in the repo (UNKNOWN). In this domain "Program Type" = "Provider Type" (label vs code naming).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-167 |
| Module | M05 |
| Page | CustomerChangeProgramType |
| Kind | Search + inline edit grid (one customer row at a time) |
| Folder / master | `MasterSetup_UI` / `~/MasterPages/NewMasterPage.master` |
| Class | `MasterSetup_UI_CustomerChangeProgramType` |

## 2. Purpose
Search customers by market structure, location, provider type, pharma platform, status and name, then change the **Provider (Program) Type** and its free-text **Provider Type Code** of any listed customer directly in the grid, saved immediately. CONFIRMED (`CustomerChangeProgramType.aspx.cs:232-311,572-627`).

## 3. Business Objective
Quickly re-classify outlets (e.g. from "General" to a strategic provider class) without opening the full customer form, so campaigns/targets keyed by program type follow the new class. INFERRED.

## 4. Navigation
Menu DB-driven (UNKNOWN). Add button → `CustomerEntry.aspx`; Reset → `CustomerView.aspx` (`resetBtn_Click`). No approval step.

## 5. Route
`~/MasterSetup_UI/CustomerChangeProgramType.aspx`.

## 6. Source Files
`Solution.Web/MasterSetup_UI/CustomerChangeProgramType.aspx(.cs)`; `IVMarketStructure.ascx`; `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs` (`Update__ProgramTypeInfo` `:41-79`, `GetCustomerList` `:405-424`); `SeedDataDAL.cs`; `CommonDataLoad.cs`; autocomplete `SInventoryWebService.GetCustomer_ALL_new` (`App_Code/SInventoryWebService.cs:349`).

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| `ddlDistributionCenter` | Distribution Center | no | first; for `RoleTypeName=="DIC"` preset to `Session["DICCompanyUnitId"]` and disabled | **filter not applied** (its clause in `Parm()` is commented out `:396-399`) |
| `ddlStationType` | (label "Provider Type") | no | never bound (no items) | adds `mas.StationTypeId=` clause only if value non-empty ⇒ practically unused |
| `ddlProgramType` | Provider Type | **yes to search** ("Please Select Program Type!") | first | `GetProgramTypeListParam(" and GRP.IsCustomer=1")` |
| `ddlPharmaPlatform` | Pharma Platform | no | first | `GetSMCTypeListParam(" and GRP.forCustomer=1")` |
| `ddlApprovalStatus` | Approval Status (twice in markup) | no | first | bound from `GetApprovalStatusList` but its clause is commented out ⇒ ignored |
| `frmDate` / `toDate` | Create From/To Date | no | today (`dd MMMM, yyyy`) | clauses commented out ⇒ ignored |
| `custNameTextBox` + AutoComplete + `hfCustomerId` | Customer | no | — | suggestion format `Code : Name : Cell |Id`; `TextChanged` splits on `|` (requires ':' present else clears and shows "Input Correct Data !") |
| `ddlStatus` | Select Status | no | Active (1) | items Select Status / Active(1) / Inactive(0) |
| `IVMarketStructure` | Group/Zone/Area/Territory/SubTerritory/Market | no | blank | cascading |
| `DivisionSelect`/`DistrictSelect`/`ThanaSelect` | Division/District/Thana | no | blank | `GetDivision_Active`, `GetDistrict_ByDivision_Active`, `GetThana_ByDistrict_Active` |
| `btnSearch` | Search | — | — | → `btnSearch_Click` |
| `lblCount` | "Total : n" | — | — | — |
| `btnExportToExcel` | Export | — | — | CSV download |
| `loadGridView` (10 per page) | Customer Code, Name, Provider Type (name), Owner, Mobile, Address, **Code** (`txtProgramTypeCode`, read-only until Edit), **Provider Type** (`ddlProgramType_G`, disabled until Edit), Update link (hidden until Edit), Edit pencil, Zone/Area/Territory/Market codes, Market name, Division/District/Thana | — | — | key `CustomerMasterId`; hidden `hfCustomerMasterId` |
| `gv_Export` | hidden export grid | — | — | includes Provider Type Code column |

## 8. Field Mapping
| UI | DAL param | SP | Column (per name; body UNKNOWN) |
|---|---|---|---|
| `hfCustomerMasterId` | `@CustomerMasterId` | `sp_Update_CustomerProgramType` | `tblCustMaster.CustomerMasterId` (WHERE) |
| `ddlProgramType_G.SelectedValue` | `@ProgramTypeId` | same | `ProgramTypeId` |
| `txtProgramTypeCode.Text` (blank ⇒ null) | `@ProgramTypeCode` | same | `ProgramTypeCode` (a column of `tblCustMaster`, visible via `*` in the list SP) |
| `Session["UserId"]` | `@UpdateBy` | same | UpdateBy |
| `DateTime.Now` | `@UpdateDate` | same | UpdateDate |
`CustomerChangeProgramType.aspx.cs:572-593`, `CustomerInfoDAL.cs:41-79`.

## 9. Dropdown Mapping
`ddlProgramType`/`ddlProgramType_G` ← `SeedDataDAL.GetProgramTypeListParam` → `sp_GET_ProgramTypeListParm(@Parm=" and GRP.IsCustomer=1")`; each grid row's dropdown is bound to the same table and pre-selected with the row's `ProgramTypeId`. `ddlPharmaPlatform` ← `sp_GET_SMCTypeListParm(" and GRP.forCustomer=1")`. `ddlApprovalStatus` ← `sp_GET_ActionStatusList` (`SoftwareUseId`/`WebShow`). Division/District/Thana from `CommonDataLoad`. Bodies UNKNOWN.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | Effect |
|---|---|---|---|---|
| btnSearch | `btnSearch_Click` `:320` | `GetCustomerList(Parm())` | `sp_Get_CustMasterList_Approve(@Parm)` | read |
| Edit pencil | `lblEditMode_Click` `:554` | — | — | unlock row: code textbox editable, dropdown enabled, Update link visible |
| Update link | `lbtUpDate_Click` `:572` (also `RowCommand "UpdateData"`) | `Update__ProgramTypeInfo` | `sp_Update_CustomerProgramType` | UPDATE one customer |
| btnExportToExcel | `btnExportToExcel_Click` `:336` | `GetCustomerList(Parm())` | same | CSV `Customer_List_dd_MMM_yyyy_hh_mm_tt.csv` |

## 11. Frontend Flow
Page load: bind dropdowns (defaults today's date, DIC lock). Search requires a provider-type filter, then grid binds and each row's dropdown is filled. User clicks pencil → row unlocks → chooses type/edits code → Update → success toast (`ShowSuccesalert`) and re-search. Export re-queries and writes CSV (quotes cells containing commas).

## 12. Backend Flow
`LoadData(parm)` → `GetCustomerList` → grid bind + count + per-row dropdown binding (try/catch swallow). `lbtUpDate_Click`: requires `ddlProgramType_G.SelectedValue != ""` (else "Please Select Program Type!" and focus) → `Update__ProgramTypeInfo` (`UpdateData` inside the lazy transaction; exception ⇒ rollback+rethrow) → success ⇒ re-run search (or clear grid if the top filter empty) else "Already Exist!". `RowCommand "EditData"` does nothing.

## 13. API Mapping
Autocomplete `GetCustomer_ALL_new(prefixText)` (`SInventoryWebService.cs:349`): `select top 25 … ISNULL(CustomerCode+' : ','')+CustomerName[+' (Inactive) ']+ISNULL(' : '+CellNo,'')+' |'+CustomerMasterId … where Customer like @prefixText` (parameterised; no active filter).

## 14. Service Mapping
No BLL (`CustomerInfoDAL`, `SeedDataDAL`, `CommonDataLoad`).

## 15. Repository Mapping
`CustomerInfoDAL.GetCustomerList`, `Update__ProgramTypeInfo`; `SeedDataDAL.GetProgramTypeListParam/GetSMCTypeListParam/GetApprovalStatusList/GetDistributionCenterDataTableList`; `CommonDataLoad.GetDivision_Active/GetDistrict_ByDivision_Active/GetThana_ByDistrict_Active`.

## 16. Stored Procedure Mapping
| SP | Body | Notes |
|---|---|---|
| `sp_Get_CustMasterList_Approve(@Parm)` | READ | dynamic SQL; `'… where mas.CustomerMasterId is not null '+@Parm+' order by CustomerCode desc'`; joins `tblCustomerType`, `tblProgramType`, `tblUser`→`tblEmpGeneralInfo`, `tblMarket`, market station type (UserRoleID=1), route/DC (`tblRouteInformationMarketDetail`→`tblRouteInformationMaster`→`tblCompanyUnit`, joined twice), sales-person (`SalesPersonId`) and tagged sales-person names (`STUFF/XML PATH` over `TaggedSalesPersonIds`), Thana/District/Division, SubTerritory→Territory→Area→Region→Group, `tblSMCType`; status label from `ActionStatus`; `Remove_SpecialCharacters(Address)`; `SELECT … , *` (all `tblCustMaster` columns) |
| `sp_Update_CustomerProgramType` | NOT in repo | UNKNOWN |

## 17. Database Mapping
Read: `tblCustMaster` + joins above. Write: `tblCustMaster` (ProgramTypeId, ProgramTypeCode, UpdateBy, UpdateDate — INFERRED from parameters).

## 18. Validation Matrix
| Rule | Where | Message |
|---|---|---|
| provider-type filter chosen | `btnSearch_Click` | "Please Select Program Type!" |
| row provider type chosen | update | "Please Select Program Type!" |
| autocomplete text must contain ":" | `custNameTextBox_TextChanged` | "Input Correct Data !" |
| no data on export | export | "No Data Found!" |
| DB failure | update | "Already Exist!" |
No format/length validation of Provider Type Code.

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-042 | A search always requires a Provider Type filter; results are further restricted by the market-structure / geography / pharma-platform / status (default Active) / customer filters (string-concatenated into `@Parm`). Date and approval-status filters are displayed but ignored. | `CustomerChangeProgramType.aspx.cs:322-333,391-495` | `Parm()` | CONFIRMED |
| BR-M05-043 | A customer's provider type and free-text provider-type code can be changed per row; the change is immediate (no approval), stamped with the user (`UserId`) and time, and must have a provider type selected. | `:572-627`; `CustomerInfoDAL.cs:41-79` | `Update__ProgramTypeInfo` | CONFIRMED (params) / UNKNOWN (SP body) |
| BR-M05-044 | Only provider types flagged for customers (`GRP.IsCustomer=1`) can be chosen. | `:68,200` | `sp_GET_ProgramTypeListParm` | CONFIRMED (filter text) |
| BR-M05-052 | DIC users get the distribution center preset/locked to their unit (`Session["DICCompanyUnitId"]`), but the list is not actually filtered by DC (clause commented). | `:129-133,396-399` | page | CONFIRMED |

## 20. Permission Matrix
`Session["RoleTypeName"]=="DIC"` locks the DC dropdown (cosmetic). `Session["UserId"]` used for `UpdateBy`. No `UserRoleID`/`RAdd/REdit` permission lookup (unlike `CustomerView`). Any user with menu access can change any customer's provider type. CONFIRMED.

## 21. Status Lifecycle
No status change. `IsActive` is only a filter; `ActionStatus` is read-only here.

## 22. Error Handling
Dropdown/data binding wrapped in empty `catch`; update failure ⇒ generic "Already Exist!"; DAL rethrows on exception ⇒ unhandled server error. `Update` success message uses `ShowSuccesalert(...,'Faild')` (title arg wrong, cosmetic).

## 23. Audit
`UpdateBy`, `UpdateDate` written (per params); no history table known.

## 24. Integration
None.

## 25. Reports
CSV export of the list (`Customer_List_<timestamp>.csv`, headers from `gv_Export`): see RPT-M05-03.

## 26. Dependencies
`IVMarketStructure` control, `SInventoryWebService`, select2, AjaxControlToolkit AutoComplete.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search without provider type | alert "Please Select Program Type!" | 042 |
| TC-02 | Search provider type X + market | only customers with X in that market, Active by default | 042 |
| TC-03 | Edit row, change type + code, Update | success; list refreshes showing new type | 043 |
| TC-04 | Update with blank dropdown | alert, no DB call | 043 |
| TC-05 | Set From/To date and Approval Status | no effect on results | 042 |
| TC-06 | DIC user searches | DC dropdown locked but customers of other DCs still listed (verify) | 052 |
| TC-07 | Free-text with `'` in customer text box | autocomplete parameterised; `hfCustomerId` value is concatenated — tamper test | GAP-023 |
| TC-08 | Export with 500 rows | CSV includes Provider Type Code column | — |

## 28. Reverse Engineering References
Files in §6; `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:821-874`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Filters | UI shows date/approval filters | ignored | MISMATCH |
| DIC scoping | UI implies | not applied | MISMATCH |

## 30. Known Gaps
SQL concatenation of dropdown values and hidden-field customer id into `@Parm` (tamperable postback values); dead filters; mislabelled `ddlStationType`; duplicate row logic (`RowCommand "UpdateData"` vs `lbtUpDate_Click`); no approval for a master-data change; `sp_Update_CustomerProgramType` unknown; the list SP can return duplicate customers when a market belongs to several route masters (double LEFT JOIN through `tblRouteInformationMarketDetail`, INFERRED). GAP-M05-009/010/023.

## 31. Change Impact
Calls `sp_Get_CustMasterList_Approve` (shared with PAGE-168 list links, 169, 173); writes `tblCustMaster.ProgramTypeId`, which drives program-type based reports, campaigns and CustomerImport "Provider Type Update" (PAGE-203). Duplicate DAL method also in `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs`.
