---
page_id: PAGE-101
module: M03
page: Territory_Transfer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-101 — Territory_Transfer

> **Evidence level:** Tier A. `Territory_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToTeriiTransfer.ascx(.cs)`, `MarketStructureTransferDAL.UpdateTerritory_Transfer` read; diffed against PAGE-102. SP bodies absent (`UNKNOWN`). See README **[MS-CTRL]** and `workflows/WF-M03-01-market-structure-transfer-request.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-101 / M03 |
| Name | Territory_Transfer ("Territory Transfer") |
| Kind | Entry / Transaction — Territory re-parenting request (Territory moves to another Area) |
| Folder / master | TransferUI / NewMasterPage.master |
| Class | `TransferUI_Territory_Transfer`; code-behind 294 lines |

## 2. Purpose
CONFIRMED: move one or more **Territories** from a source **Area** to a destination **Area**. Source selectors: Group, Zone, Area; destination selectors: Group, Zone, Area (Territory/Sub-Territory/Market rows hidden).

## 3. Business Objective
INFERRED: rebalance territory ownership between areas (AM catchments) while the field-force (MIO tag), customers and routes attached to a territory follow the territory; approval on PAGE-099 (`Type="Territory"`).

## 4. Navigation
Menu path UNKNOWN. Reset → `Market_Transfer.aspx` (`.cs:278`). Success reloads `Territory_Transfer.aspx` (`.cs:182`).

## 5. Route
`~/TransferUI/Territory_Transfer.aspx`

## 6. Source Files
`Solution.Web/TransferUI/Territory_Transfer.aspx(.cs)`; `IVMarketStructureFrom_ToTeriiTransfer.ascx(.cs)` (Group/Zone/Area visible; Territory row `visible="false"` `.ascx:59`); `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:514-580`; `CommonDataLoad.GetTerritory_ByAreaId_Active` (CommonDataLoad.cs:1539).

## 7. UI Control Inventory
| Control | Label | Required | State | Source | Message |
|---|---|---|---|---|---|
| From Group / Zone / Area | Group / Zone / Area | Area required to Search (`.cs:199`) | Group locked for NSM; +Zone for DZSM; +Group,Zone,Area(enabled) for AM (see [MS-CTRL]) | cascade SPs | empty Area → alert "Please Select Area!" |
| To Group / Zone / Area | same | **To Area required** (`.cs:99-105`) | same | same | tooltip "please fill out this field" |
| `btnSearch` | Search | – | – | `sp_CS_GetTerritory_ByAreaId_Active(@id)` | – |
| `chkMarket`, `chkListMarket` | heading "Territory List"; value `TerritoryId`, text `TerritoryName` | ≥1 ticked | – | | "Please Select at least one Territory !!!" |
| `btnSave` / `btnReset` | Submit / Reset | – | sweetalert confirm | | |

## 8. Field Mapping
From Group/Zone/Area/Territory/SubTerritory → `FGroupId,FRegionId,FAreaId,FTerritoryId,FSubTerritoryId` (`.cs:161-165`) → `@F*`. To Group/Zone/Area → `TGroupId,TRegionId,TAreaId` (`.cs:168-170`) → `@TGroupId,@TRegionId,@TAreaId` (DAL:540-542). Ticked territory id → `@TTerritoryId` (DAL:543). `@TSubTerritoryId,@TMarketId`=NULL. `@Type`="Territory" (`.cs:119`). `@EntryBy`=`Session["UserId"]`.

## 9. Dropdown Mapping
Cascade per [MS-CTRL]; `chkListMarket` ← `GetTerritory_ByAreaId_Active(F_AreaSelect)` (`.cs:204`) → `sp_CS_GetTerritory_ByAreaId_Active(@id)`; value `TerritoryId`, text `TerritoryName`.

## 10. Button Action Mapping
Search `btnSearch_Click` `.cs:195` → `CommonDataLoad.GetTerritory_ByAreaId_Active`; Submit `btnSave_Click` `.cs:110` → `Validation()` → `MarketStructureTransferDAL.UpdateTerritory_Transfer(dto,"Territory",csv,user)` → `sp_Update_MarketStructure_Transfer` × N; Reset → redirect; select-all `.cs:281`.

## 11. Frontend Flow
As PAGE-102 (UpdatePanel, select2 re-init, sweetalert confirm, success alert → `Territory_Transfer.aspx`).

## 12. Backend Flow
`Validation()` (ticked ≥1; To Area selected) → CSV of TerritoryIds → DAL loops: `@FGroupId..@FSubTerritoryId` source, `@TGroupId,@TRegionId,@TAreaId` destination, `@TTerritoryId=item`, others NULL, `@EntryBy`, `@Type="Territory"`; commit at `SqlConnectionClose()`, rollback + rethrow on exception (BR-M03-101). No try/catch in page.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`MarketStructureTransferDAL.UpdateTerritory_Transfer` (MarketStructureTransferDAL.cs:514) → `sp_Update_MarketStructure_Transfer`; `CommonDataLoad.GetTerritory_ByAreaId_Active` → `sp_CS_GetTerritory_ByAreaId_Active`.

## 16. Stored Procedure Mapping
`sp_Update_MarketStructure_Transfer` (write; body absent), `sp_CS_GetTerritory_ByAreaId_Active` (read; body absent), control SPs.

## 17. Database Mapping
No inline SQL; tables UNKNOWN.

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Ticked territories | ≥1 | `.cs:90-94` | "Please Select at least one Territory !!!" | BR-M03-004 |
| To Area | required | `.cs:99-105` | tooltip | BR-M03-014 |
| From Area | required to search | `.cs:199` | "Please Select Area!" | BR-M03-015 |
| From≠To | none | – | – | BR-M03-010 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-014 | Destination **Area** is mandatory; destination Group/Zone are sent from the control (`TRegionId`, `TGroupId`) but not validated. | Territory_Transfer.aspx.cs:99-105; DAL:540-542 | Validation/DAL | CONFIRMED |
| BR-M03-015 | Source list = active territories of chosen source Area; Area mandatory to search. | .cs:199-213 | btnSearch_Click | CONFIRMED |
Also: BR-M03-001/002 (control), BR-M03-004 (≥1 ticked), BR-M03-007/009/010 semantics with `@Type="Territory"`, `@TTerritoryId=item`.

## 20. Permission Matrix
Same as PAGE-102 (UserId unguarded; menu-only; no `<authorization>`; role only locks selectors).

## 21. Status Lifecycle
Request → approved on PAGE-099 `Type="Territory"` (INFERRED); no status set here.

## 22. Error Handling
Alerts; swallowed search errors; unhandled DAL exceptions.

## 23. Audit
`@EntryBy` only.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
`MarketStructureTransferDAL`, `CommonDataLoad`, PAGE-099. Territory is also referenced by TerritoryWiseDepot (PAGE-064), Route (PAGE-062), MIO tagging (FieldForce pages) — impact UNKNOWN.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search, From Area empty | alert "Please Select Area!" | BR-M03-015 |
| TC-02 | Search valid Area | active territories | BR-M03-015 |
| TC-03 | Submit none ticked | alert | BR-M03-004 |
| TC-04 | Submit, To Area empty | highlighted | BR-M03-014 |
| TC-05 | Submit 3 territories | 3 SP calls, one txn | BR-M03-007 |
| TC-06 | Same area as destination | accepted | BR-M03-010 |

## 28. Reverse Engineering References
`Solution.Web/TransferUI/Territory_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToTeriiTransfer.ascx(.cs)`, `MarketStructureTransferDAL.cs:514-580`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Territory move | None existed | Area is parent key sent | SP absent | UNKNOWN | §16 |

## 30. Known Gaps
SP body absent (GAP-M03-001); wrong-noun messages not present here except heading reuse; no try/catch; last-result-only; static DAL; Reset → Market_Transfer; no downstream-impact check (orders/customers/routes) in C#.

## 31. Change Impact
Shares transfer SP with PAGE-095/096/100/102; `GetTerritory_ByAreaId_Active` used by every structure control.
