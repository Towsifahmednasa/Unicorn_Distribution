---
page_id: PAGE-096
module: M03
page: Market_Transfer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-096 — Market_Transfer

> **Evidence level:** Tier A. `Market_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToTranser.ascx(.cs)`, `MarketStructureTransferDAL.UpdateMArket_Transfer` read; diffed against PAGE-102. SP bodies absent (`UNKNOWN`). See README **[MS-CTRL]**, `workflows/WF-M03-01-market-structure-transfer-request.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-096 / M03 |
| Name | Market_Transfer ("Market Transfer") |
| Kind | Entry / Transaction — Market re-parenting request (Market moves to another Sub-Territory) |
| Folder / master | TransferUI / NewMasterPage.master |
| Class | `TransferUI_Market_Transfer`; code-behind 294 lines |
| Note | This is the page all other transfer pages' *Reset* button redirects to |

## 2. Purpose
CONFIRMED: move one or more **Markets** (Group→Zone→Area→Territory→Sub-Territory→Market; the finest level) from a source **Sub-Territory** to a destination **Sub-Territory**. Source & destination selectors: Group, Zone, Area, Territory, Sub-Territory (Market row hidden).

## 3. Business Objective
INFERRED: correct/re-assign the market (a physical trading locality) a customer/doctor/route belongs to, with approval on PAGE-099 (`Type="Market"`).

## 4. Navigation
Menu path UNKNOWN. Reset reloads itself (`Market_Transfer.aspx`, `.cs:278`). Success reloads `Market_Transfer.aspx`.

## 5. Route
`~/TransferUI/Market_Transfer.aspx`

## 6. Source Files
`Solution.Web/TransferUI/Market_Transfer.aspx(.cs)`; `IVMarketStructureFrom_ToTranser.ascx(.cs)` (Group…Sub-Territory visible, Market `visible="false"`); `MarketStructureTransferDAL.cs:236-302` (`UpdateMArket_Transfer`); `CommonDataLoad.GetMarket_BySubTerritoryId_Active` (CommonDataLoad.cs:1667).

## 7. UI Control Inventory
| Control | Label | Required | State | Source | Message |
|---|---|---|---|---|---|
| From Group…Sub-Territory | Group/Zone/Area/Territory/Sub-Territory | Sub-Territory required to Search (`.cs:199`) | role locks per [MS-CTRL] | cascade SPs | empty → alert "Please Select Sub-Territory!" |
| To Group…Sub-Territory | same | **To Sub-Territory required** (`.cs:99-105`) | same | same | tooltip "please fill out this field" |
| `btnSearch` | Search | – | – | `sp_CS_GetMarket_BySubTerritoryId_Active(@id)` | – |
| `chkMarket` / `chkListMarket` | heading "Market List"; value `MarketId`, text `MarketName` | ≥1 ticked | 4-column | | "Please Select at least one Market !!!" |
| `btnSave`/`btnReset` | Submit / Reset | – | sweetalert confirm | | |

## 8. Field Mapping
From selectors → `FGroupId,FRegionId,FAreaId,FTerritoryId,FSubTerritoryId` (`.cs:161-165`); To selectors → `TGroupId,TRegionId,TAreaId,TTerritoryId,TSubTerritoryId` (`.cs:168-172`); ticked market id → `@TMarketId` (DAL:269); `@Type`="Market" (`.cs:119`); `@EntryBy`=`Session["UserId"]`. DAL sends all F*/T* + `@TMarketId`.

## 9. Dropdown Mapping
Cascade per [MS-CTRL]; `chkListMarket` ← `GetMarket_BySubTerritoryId_Active(F_SubTeritory)` (`.cs:204`) → `sp_CS_GetMarket_BySubTerritoryId_Active(@id)`; value `MarketId`, text `MarketName`.

## 10. Button Action Mapping
Search `.cs:195`; Submit `btnSave_Click` `.cs:110` → `Validation()` → `UpdateMArket_Transfer(dto,"Market",csv,user)` → `sp_Update_MarketStructure_Transfer` × N; Reset `.cs:277`; Select-all `.cs:281`.

## 11. Frontend Flow
As PAGE-102.

## 12. Backend Flow
`Validation()` (≥1 market ticked; To Sub-Territory selected) → CSV of MarketIds → DAL loop (one `SaveData("sp_Update_MarketStructure_Transfer")` per market, all F*/T* ids, `@TMarketId=item`, `@Type="Market"`), single implicit transaction (BR-M03-101); no try/catch in page.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`MarketStructureTransferDAL.UpdateMArket_Transfer` (:236) → `sp_Update_MarketStructure_Transfer`; `CommonDataLoad.GetMarket_BySubTerritoryId_Active` → `sp_CS_GetMarket_BySubTerritoryId_Active`.

## 16. Stored Procedure Mapping
`sp_Update_MarketStructure_Transfer` (write; body absent); `sp_CS_GetMarket_BySubTerritoryId_Active` (read; body absent).

## 17. Database Mapping
No inline SQL; tables UNKNOWN.

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Ticked markets | ≥1 | `.cs:90-94` | "Please Select at least one Market !!!" | BR-M03-016 |
| To Sub-Territory | required | `.cs:99-105` | tooltip | BR-M03-017 |
| From Sub-Territory | required to search | `.cs:199` | "Please Select Sub-Territory!" | BR-M03-016 |
| Validation of pending orders on ticked market | commented out (`CheckCustInOrder` body) | `.cs:241-274` | – | BR-M03-010 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-016 | Source list = **active** markets of chosen source Sub-Territory; at least one must be ticked (message names "Market"). | Market_Transfer.aspx.cs:90-94,199-213 | Validation/Search | CONFIRMED |
| BR-M03-017 | Destination **Sub-Territory** is mandatory; destination Group/Zone/Area/Territory are sent as context. | .cs:99-105; DAL:255-267 | Validation/DAL | CONFIRMED |
Also: BR-M03-001/002, BR-M03-007/009/010 with `@Type="Market"`, `@TMarketId=item`.

## 20. Permission Matrix
As PAGE-102 (unguarded `Session["UserId"]`; menu-only; no `<authorization>`).

## 21. Status Lifecycle
Request → approved on PAGE-099 `Type="Market"` (INFERRED).

## 22. Error Handling
As PAGE-102.

## 23. Audit
`@EntryBy`.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
DAL classes above; PAGE-099. Markets are referenced by customers (`sp_Update_CustomerInfoForMarketData` on MarketSetup update), routes (PAGE-062) — effect of transfer UNKNOWN.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search without Sub-Territory | alert "Please Select Sub-Territory!" | BR-M03-016 |
| TC-02 | Search valid | active markets | BR-M03-016 |
| TC-03 | Submit none | alert "…Market !!!" | BR-M03-016 |
| TC-04 | Submit, To Sub-Territory empty | highlighted | BR-M03-017 |
| TC-05 | 5 markets | 5 SP calls one txn | BR-M03-007 |
| TC-06 | Market with pending orders | accepted (check commented out) | BR-M03-010 |

## 28. Reverse Engineering References
`Solution.Web/TransferUI/Market_Transfer.aspx(.cs)`, `IVMarketStructureFrom_ToTranser.ascx(.cs)`, `MarketStructureTransferDAL.cs:236-302`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Market move | None existed | market id in `@TMarketId` | SP absent | UNKNOWN | §16 |

## 30. Known Gaps
SP body absent (GAP-M03-001); order-pending validation commented out (dead `CheckCustInOrder`, `.cs:241-274`) — the same check exists on PAGE-099 only for a hidden/never-called handler; no try/catch; static DAL; last-result-only.

## 31. Change Impact
Shared SP with PAGE-095/100/101/102; MarketSetup (PAGE-042) and MarketInfoImport (PAGE-098) also create/alter markets.
