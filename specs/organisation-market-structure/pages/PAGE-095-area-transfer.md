---
page_id: PAGE-095
module: M03
page: Area_Transfer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-095 — Area_Transfer

> **Evidence level:** Tier A. `Area_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToAreaTranfer.ascx(.cs)`, `MarketStructureTransferDAL.UpdateArea_Transfer` read in full; diffed line-by-line against PAGE-102 (identical structure). SP bodies absent (`UNKNOWN`). Shared control docs: README **[MS-CTRL]**; the full transfer process is in `workflows/WF-M03-01-market-structure-transfer-request.md`. Only differences from PAGE-102 are spelled out.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-095 / M03 |
| Page name | Area_Transfer ("Area Transfer", `Area_Transfer.aspx` breadcrumb) |
| Kind | Entry / Transaction — creates an **Area re-parenting request** (Area moves to another Zone) |
| Web folder / master | TransferUI / NewMasterPage.master |
| Page class | `TransferUI_Area_Transfer` |
| Code-behind LOC | 295 |
| Legacy/duplicate | Copy of PAGE-102/096/100/101 |

## 2. Purpose
CONFIRMED: move one or more **Areas** from a source **Zone** to a destination **Zone**. Source: Group + Zone (Search lists active areas of the zone). Destination: Group + Zone.

## 3. Business Objective
INFERRED: reorganise the Zone→Area hierarchy (e.g. an area is re-assigned to another DZSM's zone) with the move staged for approval on PAGE-099 (`Type="Area"`).

## 4. Navigation
Menu DB-driven (path UNKNOWN). Reset → `Market_Transfer.aspx` (`Area_Transfer.aspx.cs:278`). Success reloads `Area_Transfer.aspx` (`.cs:182`).

## 5. Route
`~/TransferUI/Area_Transfer.aspx`.

## 6. Source Files
View `Solution.Web/TransferUI/Area_Transfer.aspx`; code-behind `.aspx.cs`; user control `Solution.Web/TransferUI/IVMarketStructureFrom_ToAreaTranfer.ascx(.cs)` (Group + Zone rows visible; Area/Territory/SubTerritory/Market rows `visible="false"`, `.ascx:46,59,79,95`); DAL `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs` (`UpdateArea_Transfer` 445-511); `CommonDataLoad.GetArea_ByZoneId_Active`; DAO `MarketStructureTranferDAO`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| From `GroupSelect` | Group | no (not validated) | placeholder | locked for NSM | `sp_CS_Group_Active` | – |
| From `ZoneSelect` | Zone | **Yes to Search** (`.cs:199`) | placeholder | locked for DZSM/AM (see [MS-CTRL]) | `sp_Get_Zone_All_Active` | empty → alert "Please Select Area!" (message says Area, field is Zone) |
| To `GroupSelect` | Group | no | placeholder | as above | | – |
| To `ZoneSelect` | Zone | **Yes** (`.cs:99-105`) | placeholder | | | tooltip "please fill out this field", `is-invalid`, focus |
| `btnSearch` | Search | – | – | – | `sp_CS_GetArea_ByZoneId_Active(@id)` | – |
| `chkMarket` / `chkListMarket` | "Territory List" heading (wrong; items are **areas**), value `AreaId`, text `AreaName` | ≥1 ticked | empty | 4-column list | | "Please Select at least one Territory !!!" |
| `btnSave` / `btnReset` | Submit / Reset | – | – | sweetalert confirm | | |

## 8. Field Mapping
| UI | DTO | Code | SP param |
|---|---|---|---|
| From Group | `FGroupId` | `.cs:161` | `@FGroupId` |
| From Zone | `FRegionId` | `.cs:162` | `@FRegionId` |
| From Area/Territory/SubTerritory (hidden) | `FAreaId,FTerritoryId,FSubTerritoryId` | `.cs:163-165` | `@FAreaId,@FTerritoryId,@FSubTerritoryId` |
| To Group | `TGroupId` | `.cs:168` | `@TGroupId` |
| To Zone | `TRegionId` | `.cs:169` | **NOT sent**: DAL sends `@TRegionId = aMaster.TAreaId` (MarketStructureTransferDAL.cs:472) — the value of the *hidden* To-Area dropdown, normally NULL |
| ticked area id | – | `.cs:125-139` | `@TAreaId` = item (DAL:473) |
| constant "Area" | – | `.cs:119` | `@Type` |
| Session UserId | – | `.cs:179` | `@EntryBy` |
`@TTerritoryId,@TSubTerritoryId,@TMarketId` = NULL.

## 9. Dropdown Mapping
Group/Zone from user control ([MS-CTRL]); `chkListMarket` ← `GetArea_ByZoneId_Active(Convert.ToInt32(F_ZoneSelect.SelectedValue))` (`.cs:204`), `sp_CS_GetArea_ByZoneId_Active(@id)`, value `AreaId`, text `AreaName`. Tables: UNKNOWN.

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Search | `btnSearch_Click` `.cs:195` | `CommonDataLoad.GetArea_ByZoneId_Active` | `sp_CS_GetArea_ByZoneId_Active` | read |
| Submit | `btnSave_Click` `.cs:110` | `Validation()` → `MarketStructureTransferDAL.UpdateArea_Transfer(dto,"Area",csv,user)` | `sp_Update_MarketStructure_Transfer` × N | write (UNKNOWN) |
| Reset | `btnReset_Click` `.cs:277` | redirect Market_Transfer.aspx | – | – |
| Select-all | `chkMarket_CheckedChanged` `.cs:281` | in-memory | – | – |

## 11. Frontend Flow
Same as PAGE-102: UpdatePanel partial postbacks, `pageLoad()` re-inits select2/pickadate, sweetalert confirm on Submit, success alert `successalert('Operation successful!','Success','Area_Transfer.aspx')`.

## 12. Backend Flow
`Validation()` → build CSV of ticked AreaIds → `UpdateArea_Transfer`: single connection, one `SaveData("sp_Update_MarketStructure_Transfer")` per area with `@FGroupId,@FRegionId,@FAreaId,@FTerritoryId,@FSubTerritoryId` (source), `@TGroupId` (dest group), **`@TRegionId = aMaster.TAreaId`**, `@TAreaId=item`, T-territory/subterritory/market NULL, `@EntryBy`, `@Type="Area"`. Implicit transaction commit/rollback by `DataAccessManager` (BR-M03-101). No try/catch in the page.

## 13. API Mapping
None (no WebMethod).

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`MarketStructureTransferDAL.UpdateArea_Transfer` (MarketStructureTransferDAL.cs:445) → `sp_Update_MarketStructure_Transfer`; `CommonDataLoad.GetArea_ByZoneId_Active` (CommonDataLoad.cs:1130) → `sp_CS_GetArea_ByZoneId_Active(@id)`.

## 16. Stored Procedure Mapping
`sp_Update_MarketStructure_Transfer` (write, body NOT in repo); `sp_CS_GetArea_ByZoneId_Active` (read, body NOT in repo); control SPs per [MS-CTRL].

## 17. Database Mapping
No inline SQL. Tables UNKNOWN (transfer-request table implied but unnamed).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Ticked areas | ≥1 | `.cs:90-94` | "Please Select at least one Territory !!!" (wrong noun) | BR-M03-004 |
| To Zone | required | `.cs:99-105` | tooltip only | BR-M03-011 |
| From Zone | required to search | `.cs:199,216-219` | "Please Select Area!" (wrong noun) | BR-M03-013 |
| From≠To / duplicates | not validated | – | – | BR-M03-010 |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M03-004 | ≥1 item must be ticked (shared rule, defined on PAGE-102). | Area_Transfer.aspx.cs:90-94 | Validation | CONFIRMED |
| BR-M03-010 | No From≠To/duplicate/pending check (shared). | Area_Transfer.aspx.cs | – | CONFIRMED |
| BR-M03-011 | Destination **Zone** is mandatory for an area transfer (not the Group). | Area_Transfer.aspx.cs:99-105 | Validation | CONFIRMED |
| BR-M03-012 | The destination Zone chosen in the UI is **not transmitted**: DAL fills `@TRegionId` from the hidden To-Area dropdown (`aMaster.TAreaId`) — normally NULL; the SP receives only dest group + area id. Whether the SP can still resolve the target zone is UNKNOWN. | MarketStructureTransferDAL.cs:472 | DAL defect | CONFIRMED (code) / impact UNKNOWN |
| BR-M03-013 | Source list = active areas of the chosen source Zone; source Zone mandatory to search. | Area_Transfer.aspx.cs:199-213 | btnSearch_Click | CONFIRMED |
Also applies: BR-M03-001/002 (control), BR-M03-007/009 (one SP call per item, single transaction, last result reported) with `@Type="Area"`.

## 20. Permission Matrix
As PAGE-102: only `Session["UserId"]` read in page (unguarded); no role check; menu-only authorisation; no `<authorization>` in Web.config. AM/DZSM users have From/To Group+Zone (and Area for AM) locked to their own hierarchy → they can only pick their own zone as *both* source and destination (INFERRED consequence of [MS-CTRL]).

## 21. Status Lifecycle
No status set by page; request → approval on PAGE-099 (`Type="Area"`). See WF-M03-02.

## 22. Error Handling
Alerts for validation; `btnSearch_Click` swallows exceptions; DAL exceptions unhandled.

## 23. Audit
`@EntryBy` only (SP-side dating UNKNOWN).

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
`MarketStructureTransferDAL`, `CommonDataLoad`; approval PAGE-099.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search, From Zone empty | alert "Please Select Area!" | BR-M03-013 |
| TC-02 | Search with zone | active areas listed | BR-M03-013 |
| TC-03 | Submit nothing ticked | alert, no SP call | BR-M03-004 |
| TC-04 | Submit, To Zone empty | To Zone highlighted | BR-M03-011 |
| TC-05 | Submit 2 areas, valid To Zone | 2 SP calls, one txn; inspect `@TRegionId` on the wire = NULL | BR-M03-012 |
| TC-06 | AM login | Group/Zone/Area selectors locked | BR-M03-002 |
| TC-07 | Source zone = destination zone | accepted | BR-M03-010 |

## 28. Reverse Engineering References
`Solution.Web/TransferUI/Area_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToAreaTranfer.ascx(.cs)`, `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:445-511`.

## 29. Spec vs Actual
| Area | Specification | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Destination zone transmission | None existed | dropped (BR-M03-012) | UNKNOWN | UNKNOWN/DEFECT-SUSPECT | §8 |

## 30. Known Gaps
- Destination Zone not passed to the SP (BR-M03-012) — GAP-M03-002.
- Wrong nouns in messages/heading; no try/catch; last-result-only; static DAL; Reset goes to Market_Transfer; SP body absent (GAP-M03-001).

## 31. Change Impact
Shares `sp_Update_MarketStructure_Transfer` with PAGE-096/100/101/102. `GetArea_ByZoneId_Active` is used by all `IVMarketStructure*.ascx` (cascade).
