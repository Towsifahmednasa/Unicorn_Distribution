---
page_id: PAGE-064
module: M03
page: TerritoryWiseDepotSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-064 — TerritoryWiseDepotSetup

> **Evidence level:** Tier A. `TerritoryWiseDepotSetup.aspx(.cs)` (355 lines), `IVMarketStructureArea.ascx(.cs)`, `TerritoryWiseDepotSetupDAL`, `DepotWiseAreaSetupDal.GetDepotList/GetSubDepotList`, `CommonDataLoad.GetTerritory_ByAreaId_ActiveDepo/GetValidationCheck` read. SP bodies absent. Process: `workflows/WF-M03-06-territory-wise-depot-mapping.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-064 / M03 |
| Name | TerritoryWiseDepotSetup ("Territory Wise Depot Setup", `.aspx:37`) |
| Kind | Entry / Transaction — depot ↔ territory mapping |
| Folder / master | MasterSetup_UI / NewMasterPage.master |
| Class | `MasterSetup_UI_TerritoryWiseDepotSetup`; static `TerritoryWiseDepotSetupDAL _DAL`, static `DepotWiseAreaSetupDal _aRepo` (`.cs:19-20`) |

## 2. Purpose
CONFIRMED: assign the set of **Territories** (within a chosen Area) that a **Depot (DC)** — optionally a **Sub-Depot** — serves for distribution. The user selects Area → *Search* lists the area's depot-eligible territories → picks a Depot → existing mapping is pre-ticked → ticks/unticks territories → Submit.

## 3. Business Objective
INFERRED: orders/invoices originating in a territory are fulfilled by the depot mapped to it (DC = distribution center); this page is the authority for that mapping (also consumed by order/delivery pages outside this module — UNKNOWN which).

## 4. Navigation
List `TerritoryWiseDepotView.aspx` (PAGE-065) links here (`?MID=` sets `btnUpdate` visible but the load-by-id call is commented out, `.cs:50`). Success → self. Reset → self (`.cs:322`).

## 5. Route
`~/MasterSetup_UI/TerritoryWiseDepotSetup.aspx[?MID=<id>]`

## 6. Source Files
`Solution.Web/MasterSetup_UI/TerritoryWiseDepotSetup.aspx(.cs)`; user control `Solution.Web/MasterSetup_UI/IVMarketStructureArea.ascx(.cs)` (Group, Zone, Area visible; Territory/SubTerritory/Market hidden); `Library.DAL/MasterSetup_DAL/TerritoryWiseDepotSetupDAL.cs`; `Library.DAL/DoctorModule_DAL/DepotWiseAreaSetupDal.cs` (`GetDepotList` :169, `GetSubDepotList` :191); `CommonDataLoad.cs` (`GetTerritory_ByAreaId_ActiveDepo` :1560, `GetValidationCheck` :1581); DAO `DcWiseTerritoryMasterDAO`, `DcWiseTerritoryDetailDAO`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| structure control Group / Zone / Area | Group / Zone / Area | **Area required** (`.cs:139,203`) | placeholder | role-locked ([MS-CTRL]) | cascade SPs | tooltip "please fill out this field" |
| `bnSearch` | Search | – | – | clears depot/sub-depot/territory list first (`.cs:134-137`) | – | – |
| `ddlDepotName` (AutoPostBack) | Depot Name | **Yes** (`.cs:212`) | placeholder | – | `sp_GET_DepotByCompanyId(@CompanyId=1)` → `ComUnitId`/`UnitName` | tooltip |
| `ddlsubDepotName` (AutoPostBack, empty handler) | Sub-Depot Name | no (commented validation `.cs:221-227`) | – | reloaded on depot change | `sp_GET_SubDepotByComUnitId(@ComUnitId)` → `SubDepotId`/`SubDepotName` | – |
| `chkTerritoryList` (CheckBoxList, AutoPostBack, 1 column) | territories | ≥1 checked (`.cs:228`) | none | – | `sp_CS_GetTerritory_ByAreaId_ActiveForDepo(@id)` → `TerritoryId`/`TerritoryName` | alert "Please select at least one Territory!" |
| `chkTerritoryAll` | select all | – | – | `Visible="false"` (`.aspx:127`) — feature disabled | – | – |
| `btnSave`/`btnUpdate` | Submit/Update | – | – | both call `btnSave_Click`; `btnSave` visible unless `?MID` | – | sweetalert confirm |
| `btnRest` | Reset | – | – | – | – | – |
| `id_mastetID` (hidden) | – | – | "" | set by depot load | – | – |
The company id for the depot list is hard-coded `1` (`_aRepo.GetDepotList(1)`, `.cs:31`).

## 8. Field Mapping
| UI | DTO | Code | SP param |
|---|---|---|---|
| `id_mastetID` | `DcWiseTerritoryMasterDAO.DcWiseTerritoryMasterId` | 0 if empty (`.cs:274`) | `@DcWiseTerritoryMasterId` |
| `ddlDepotName` | `DCId` | `.cs:276` | `@DCId` |
| `ddlsubDepotName` | `SubDepotId` | `.cs:277` | `@SubDepotId` |
| Group / Zone / Area | `GroupId` / `RegionId` / `AreaId` | `.cs:278-280` | `@GroupId,@RegionId,@AreaId` |
| each ticked territory | `DcWiseTerritoryDetailDAO.TerritoryId` | `.cs:253-270` | `sp_Save_DcWiseTerritoryDetail(@DcWiseTerritoryMasterId,@TerritoryId)` |
| Session UserId | – | `.cs:288` | `@EntryBy` (insert) / `@UpdateBy` (update) |

## 9. Dropdown Mapping
Depot ← `DepotWiseAreaSetupDal.GetDepotList(1)`; Sub-Depot ← `GetSubDepotList(ddlDepotName.SelectedValue)`; territory checklist ← `CommonDataLoad.GetTerritory_ByAreaId_ActiveDepo(AreaSelect)`; hierarchy per [MS-CTRL]. Existing mapping pre-ticks via `MethodGetSetupDetailById(DCId, "0")` (`.cs:88-130`).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Search | `bnSearch_Click` `.cs:132` | `GetTerritory_ByAreaId_ActiveDepo` | `sp_CS_GetTerritory_ByAreaId_ActiveForDepo` | read |
| Depot change | `ddlDepotName_SelectedIndexChanged` `.cs:59` | `GetSubDepotList`, `TerritoryWiseDepotSetupDAL.GetSetupDetailById(DC,"0")` | `sp_GET_SubDepotByComUnitId`, `sp_GET_lDcWiseTerritoryDetail_ByAreaId` | read |
| Territory tick/untick | `chkTerritoryList_SelectedIndexChanged` `.cs:332` | untick → `CommonDataLoad.GetValidationCheck(id,"TerritoryWiseDepotSetupUncheck")` | `sp_check_Vali_MarketStructure` | read |
| Submit/Update | `btnSave_Click` `.cs:246` | `Validation()` → `TerritoryWiseDepotSetupDAL.SaveBonusCampaign(master, details, UserId)` | `sp_Save/Update_DcWiseTerritoryMaster`, `sp_Save_DcWiseTerritoryDetail` | write |
| Reset | `btnRest_Click` `.cs:320` | redirect self | – | – |

## 11. Frontend Flow
UpdatePanel form. Every tick/untick on `chkTerritoryList` causes a full async postback (`AutoPostBack=true`). The index of the toggled box is recovered from `Request.Form["__EVENTTARGET"]` by taking its **last character** (`.cs:334-335`) — items 10 and above therefore map to index 0-9 (wrong item) (CONFIRMED bug, BR-M03-047).

## 12. Backend Flow
1. `Page_Load` first load: depot list (`GetDepotList(1)`), `?MID` → `btnUpdate` visible else `btnSave`.
2. Search: reset depot/sub-depot/list; if Area chosen → bind territories (exceptions swallowed) else highlight Area.
3. Depot change: reload sub-depots; unset all ticks; `GetSetupDetailById(DC, "0")` → if rows: set `id_mastetID` = first row `DcWiseTerritoryMasterId`, tick territories present in result; else untick all. (The sub-depot selection has an empty handler, so mapping shown is always for SubDepotId `"0"`.)
4. Untick guard: `GetValidationCheck(value,"TerritoryWiseDepotSetupUncheck")`; any row → item re-ticked + `faildalert('Order Pending!')`.
5. Submit: `Validation()` → collect ticked territories → `SaveBonusCampaign`: `master.Id>0` → `UpdateData("sp_Update_DcWiseTerritoryMaster")` (+`@UpdateBy`), else `SaveDataReturnPrimaryKey("sp_Save_DcWiseTerritoryMaster")` (+`@EntryBy`); `pk>0` → `SaveData("sp_Save_DcWiseTerritoryDetail")` for every ticked territory (no delete of removed territories in C#; SP-side handling UNKNOWN). One implicit transaction; exceptions rollback + rethrow (BR-M03-101).
6. Page shows success alert → self; any non-success → `faildalert('Already Exist!')`.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL (DAL direct).

## 15. Repository Mapping
| DAL method | Source | SP |
|---|---|---|
| `TerritoryWiseDepotSetupDAL.SaveBonusCampaign` (name copied from campaign page) | TerritoryWiseDepotSetupDAL.cs:16-80 | `sp_Update_DcWiseTerritoryMaster`, `sp_Save_DcWiseTerritoryMaster`, `sp_Save_DcWiseTerritoryDetail` |
| `GetSetupDetailById` | TerritoryWiseDepotSetupDAL.cs:119 | `sp_GET_lDcWiseTerritoryDetail_ByAreaId(@DCId,@SubDepotId)` |
| `GetDepotList`/`GetSubDepotList` | DepotWiseAreaSetupDal.cs:169/191 | `sp_GET_DepotByCompanyId`, `sp_GET_SubDepotByComUnitId` |
| `GetTerritory_ByAreaId_ActiveDepo` | CommonDataLoad.cs:1560 | `sp_CS_GetTerritory_ByAreaId_ActiveForDepo` |
| `GetValidationCheck` | CommonDataLoad.cs:1581 | `sp_check_Vali_MarketStructure` |

## 16. Stored Procedure Mapping
`sp_Save_DcWiseTerritoryMaster` (write, returns pk; params `@DcWiseTerritoryMasterId,@DCId,@SubDepotId,@GroupId,@RegionId,@AreaId,@EntryBy`), `sp_Update_DcWiseTerritoryMaster` (same +`@UpdateBy`), `sp_Save_DcWiseTerritoryDetail` (`@DcWiseTerritoryMasterId,@TerritoryId`), read SPs listed above. All bodies NOT in repository.

## 17. Database Mapping
No inline SQL. Tables UNKNOWN (names suggest `DcWiseTerritoryMaster`/`…Detail` — INFERRED from SP names only).

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| Area required | `.cs:203` | tooltip | BR-M03-041 |
| Depot required | `.cs:212` | tooltip | BR-M03-041 |
| ≥1 territory ticked (`chkTerritoryList.SelectedValue == ""`) | `.cs:228` | "Please select at least one Territory!" | BR-M03-042 |
| Sub-depot | optional (validation commented) | – | BR-M03-041 |
| Untick blocked when check SP returns rows | `.cs:341-350` | "Order Pending!" | BR-M03-045 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-041 | Area and Depot are mandatory; Sub-Depot is optional; Group/Zone are not validated. | TerritoryWiseDepotSetup.aspx.cs:203-218,221-227 | page | CONFIRMED |
| BR-M03-042 | At least one territory must be ticked to save (so a mapping cannot be emptied through this page). | .cs:228 | page | CONFIRMED |
| BR-M03-043 | Eligible territories = active territories of the Area returned by `sp_CS_GetTerritory_ByAreaId_ActiveForDepo` (the "ForDepo" filter, e.g. excluding already-assigned — semantics UNKNOWN). | .cs:146; CommonDataLoad.cs:1560 | page | CONFIRMED (call) |
| BR-M03-044 | Choosing a depot loads that depot's existing mapping (for SubDepotId "0" only) and pre-ticks it; the master id from that mapping makes the next save an UPDATE. | .cs:59-130 | page | CONFIRMED |
| BR-M03-045 | Unticking a territory is refused (re-ticked, "Order Pending!") when `sp_check_Vali_MarketStructure(@PageName='TerritoryWiseDepotSetupUncheck')` returns any row (presumably pending orders on that depot/territory — UNKNOWN). | .cs:332-354 | page | CONFIRMED (call) |
| BR-M03-046 | Save is master + detail rows in one transaction; master's Group/Region/Area come from the *current* selectors, not from the loaded mapping; removed territories are not deleted by C#. | TerritoryWiseDepotSetupDAL.cs:16-80 | DAL | CONFIRMED / UNKNOWN (SP) |
| BR-M03-047 | Toggled-item index is parsed from the last char of `__EVENTTARGET`; lists with ≥10 territories mis-map items 10+. | .cs:334-336 | page | CONFIRMED (defect) |
| BR-M03-048 | Depot list is restricted to company id 1 (hard-coded). | .cs:31 | page | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]` (unguarded `.cs:288`); control reads role keys. No role gate; menu-only; no `<authorization>`.

## 21. Status Lifecycle
No status column handled in C#. Mapping exists/not exists (INFERRED). Unticking blocked when pending orders (BR-M03-045).

## 22. Error Handling
Territory bind and sub-depot bind swallow exceptions; depot list swallowed; `GetSetupDetailById` unguarded (exception → yellow screen); Save unhandled. Failure text is always "Already Exist!".

## 23. Audit
`@EntryBy` / `@UpdateBy` = UserId; dates SP-side (UNKNOWN).

## 24. Integration
None.

## 25. Reports
None (list PAGE-065).

## 26. Dependencies
`IVMarketStructureArea` control; `sp_check_Vali_MarketStructure` (shared); consumers of depot-territory mapping outside module UNKNOWN.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search without Area | Area highlighted | BR-M03-041 |
| TC-02 | Search with Area | territory checklist | BR-M03-043 |
| TC-03 | Submit without depot | depot highlighted | BR-M03-041 |
| TC-04 | Submit with none ticked | alert | BR-M03-042 |
| TC-05 | Select depot with existing mapping | boxes pre-ticked, master id set | BR-M03-044 |
| TC-06 | Untick territory having pending orders | re-ticked + "Order Pending!" | BR-M03-045 |
| TC-07 | 12 territories, untick #11 | wrong item toggled/validated | BR-M03-047 |
| TC-08 | Submit new mapping | success | BR-M03-046 |
| TC-09 | Choose sub-depot | no reload of mapping | §12 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/TerritoryWiseDepotSetup.aspx(.cs)`, `IVMarketStructureArea.ascx(.cs)`, `Library.DAL/MasterSetup_DAL/TerritoryWiseDepotSetupDAL.cs`, `Library.DAL/DoctorModule_DAL/DepotWiseAreaSetupDal.cs:169-210`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Sub-depot handling | None existed | selectable but ignored on load; saved on submit | absent | PARTIAL | BR-M03-044 |

## 30. Known Gaps
Index-parsing defect (BR-M03-047); sub-depot handler empty; hard-coded company 1; can't empty a mapping; `?MID` edit load commented out; static DAL fields; swallowed exceptions; misnamed `SaveBonusCampaign`; SP bodies absent (GAP-M03-001).

## 31. Change Impact
`sp_check_Vali_MarketStructure` shared (~20 callers). `GetTerritory_ByAreaId_ActiveDepo` only used by this page (grep). PAGE-035 DepotWiseAreaSetup (area-level mapping) and PAGE-065 read the same mapping domain.
