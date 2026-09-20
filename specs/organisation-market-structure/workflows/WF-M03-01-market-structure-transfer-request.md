# WF-M03-01 — Market-structure transfer request (Zone / Area / Territory / Sub-Territory / Market)

> Traced from: `Solution.Web/TransferUI/{Zone,Area,Territory,SubTerritory,Market}_Transfer.aspx(.cs)`, the five `IVMarketStructureFrom_To*.ascx(.cs)` controls, `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs`, `Library.DAL/DataManager/DataAccessManager.cs`. SP bodies are **not** in the repo: what `sp_Update_MarketStructure_Transfer` writes is `UNKNOWN`. Confidence tags per step.

## Trigger & actors
- Trigger: an organisation/master-data administrator restructures the sales hierarchy (moves a node under a new parent).
- Actor: any logged-in user who can open the page (menu-based only; no role check in code — BR-M03-102). Role only affects which upper-level selectors are locked (BR-M03-002). Approver: see WF-M03-02.

## Hierarchy levels (as implemented, top → bottom)
`Group` (maintained on "National Setup"; `GroupId`) → `Zone` (DB names `RegionId/RegionName`; the UI says Zone, the code says Region — they are the **same level**) → `Area` → `Territory` → `Sub-Territory` → `Market`. Roles: NSM at Group, DZSM at Zone, AM at Area, MIO at Territory (INFERRED from control role scoping, `IVMarketStructureFrom_To.ascx.cs:46-140`). There is no separate "Region" level between Zone and Area in code.

## Preconditions
- Session valid (`Session["UserId"]`). Master data exists and active (Search lists **active** children only — BR-M03-006/013/015/016).
- For AM/DZSM/NSM users, `sp_HigharchyInfoByEmployeeId` must return a row (else the control throws — `IVMarketStructureFrom_To*.ascx.cs`).

## The five variants (what each page moves and re-parents)
| Page (PAGE) | Type sent (`@Type`) | Source selectors → list shown (Search) | Item id posted as | Destination required | Destination ids sent | Message wording defects |
|---|---|---|---|---|---|---|
| Zone_Transfer (102) | `Zone` | From **Group** → active Zones (`sp_Get_Zone_All_Active(@GroupId)`) | `@TRegionId` | To Group | `@TGroupId` (others NULL) | search alert says "Area"; list heading & alert say "Territory" |
| Area_Transfer (095) | `Area` | From **Zone** → active Areas (`sp_CS_GetArea_ByZoneId_Active`) | `@TAreaId` | To Zone | `@TGroupId`; **`@TRegionId = aMaster.TAreaId` (hidden To-Area, normally NULL) — destination Zone is dropped** (BR-M03-012) | search alert "Area"; list heading/alert "Territory" |
| Territory_Transfer (101) | `Territory` | From **Area** → active Territories (`sp_CS_GetTerritory_ByAreaId_Active`) | `@TTerritoryId` | To Area | `@TGroupId,@TRegionId,@TAreaId` | correct |
| SubTerritory_Transfer (100, Tier B) | `Sub-Territory` | From **Territory** → active Sub-Territories (`sp_CS_GetSubTerritory_ByTerritoryId_Active`) | `@TSubTerritoryId` | To Territory | `@TGroupId,@TRegionId,@TAreaId,@TTerritoryId` | search alert "Sub-Territory"; validation says "Territory" (BR-M03-018) |
| Market_Transfer (096) | `Market` | From **Sub-Territory** → active Markets (`sp_CS_GetMarket_BySubTerritoryId_Active`) | `@TMarketId` | To Sub-Territory | `@TGroupId…@TSubTerritoryId` | correct |
All variants always send the *source* ids `@FGroupId,@FRegionId,@FAreaId,@FTerritoryId,@FSubTerritoryId` (only those visible in the source control are non-NULL), `@EntryBy` = `Session["UserId"]`, `@Type`. DAL methods: `UpdateZone_Transfer` (:375), `UpdateArea_Transfer` (:445), `UpdateTerritory_Transfer` (:514), `UpdateSubTerritory_Transfer` (:304), `UpdateMArket_Transfer` (:236) — five copy-pasted loops.

## Steps
1. **Open page** → page `Page_Load` (`Zone_Transfer.aspx.cs:23`) grabs the two control instances' dropdowns via `FindControl`; each control's `Page_Load` loads active Groups (`sp_CS_Group_Active`); for NSM/DZSM/AM roles it calls `GetHigharcyInfoByEmployeeId(EmpInfoId, RoleTypeId)` (`sp_HigharchyInfoByEmployeeId`) and pre-selects/locks levels (BR-M03-002). [CONFIRMED]
2. **Pick source** through the cascade (BR-M03-001): each selection change reloads the next level (active only) and clears lower ones.
3. **Search** (`btnSearch_Click`, `.cs:195`) → active children of the chosen source node bound to `chkListMarket` (4-column checkbox list). Exceptions swallowed. Empty selection → alert with the wrong noun (table above). [CONFIRMED]
4. **Pick destination** through the To control (same cascade; role locks apply equally, so an AM/DZSM/NSM user can only choose within their own hierarchy — INFERRED).
5. **Tick items** (`chkMarket` = select/unselect all).
6. **Submit** (`btnSave_Click`, `.cs:110`) → JS confirm `sweetAlertConfirm_Submit` → `Validation()`: ≥1 ticked (BR-M03-004/016); destination level selected (BR-M03-005/011/014/017/018) → build CSV → DAL loop: for each id `accessManager.SaveData("sp_Update_MarketStructure_Transfer", …)`. [CONFIRMED]
7. **Commit**: no explicit transaction call; `DataAccessManager` starts a `SqlTransaction` at the first write and commits in `SqlConnectionClose()` (called in `finally`); an exception triggers `SqlConnectionClose(true)` (rollback) then rethrow — whole batch is all-or-nothing (BR-M03-007/101). [CONFIRMED]
8. **Result**: `Res.isSuccess` = last item's result (BR-M03-009) → success alert & page reload, else "Operation Faild!". Exceptions are not caught in the page (yellow screen). [CONFIRMED]
9. **Downstream (UNKNOWN)**: the SP either applies the re-parenting immediately or stores a request row keyed by `MarketStructureTranferId`; the approval page (WF-M03-02) proves the latter exists (INFERRED). Effects on customers, doctors, MIO tagging, routes, depot mapping, targets, orders, reports: `UNKNOWN`.

## Data created / changed
| Data | Change | Evidence |
|---|---|---|
| Transfer-request rows (table name UNKNOWN) | one row per ticked item: type, from-ids, to-ids, entry user (INFERRED) | SP name + approval grid key `MarketStructureTranferId`; DAO `MarketStructureTranferDAO` |
| Hierarchy parent keys | changed by the SP at transfer or at approval time | UNKNOWN |

## Status transitions
`(none)` → `Requested` (this workflow) → `Approved` (WF-M03-02). No cancel/reject/withdraw transition exists in code (BR-M03-026).

## Branches / exceptions
- No item ticked → alert, no DB call. Destination missing → field highlighted, no DB call.
- Source = destination, or item already under destination, or duplicate pending request → **not checked** in C# (BR-M03-010).
- SP failure → rollback of the whole submit; unhandled exception page.
- Session expired → `NullReferenceException` at `Session["UserId"].ToString()`.
- Concurrency: DAL instance is `static` in the page (`private static MarketStructureTransferDAL _CustomerInfoDAL`) but `DataAccessManager` keeps connection/transaction in instance fields → two simultaneous submits share one connection/transaction object (BR-M03-103; INFERRED impact).

## Postconditions
Requests staged (or moves applied — UNKNOWN). Page reloads itself; *Reset* redirects to `Market_Transfer.aspx` on all five pages.

## Related BR
BR-M03-001, 002, 004–018, 026, 101–103. Pages: PAGE-095, 096, 100, 101, 102 (+ PAGE-099 for approval).

## Tests
| ID | Scenario | Expected |
|---|---|---|
| WF1-T1 | Zone transfer of 2 zones to another Group | 2 SP calls, one transaction, success alert |
| WF1-T2 | Area transfer: verify wire params | `@TRegionId` NULL although a To-Zone was chosen (BR-M03-012) |
| WF1-T3 | Territory transfer, no destination Area | To-Area highlighted, no DB write |
| WF1-T4 | Market transfer with SP error on item 3 | no request rows persist; error page |
| WF1-T5 | AM user opens Area_Transfer | Group/Zone locked; can only pick own zone as both source and destination |
| WF1-T6 | Two users submit simultaneously | verify no cross-contamination (BR-M03-103) |
| WF1-T7 | Same node submitted twice | both accepted (no duplicate check) — verify SP behaviour |
