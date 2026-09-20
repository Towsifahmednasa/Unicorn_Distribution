# WF-M03-02 — Market-structure transfer approval

> Traced from `Solution.Web/TransferUI/MarketStructure_TransferApprove.aspx(.cs)`, `Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs` (`GetList` :18, `UpdateMarketStructure_TransferApprove` :153). `sp_Get_MSList_Approve` and `sp_Update_MarketStructure_Approve` bodies are not in the repo — the *effect* of approval is `UNKNOWN`.

## Trigger & actors
- Trigger: requests exist from WF-M03-01 (Type ∈ Market, Sub-Territory, Territory, Area, Zone).
- Actor: any user with the page in their menu (no role/level check; the requester can approve their own request — BR-M03-023).

## Preconditions
Pending requests returned by `sp_Get_MSList_Approve(@Parm=<type>)`. Session `UserId`.

## Steps
1. **Open page** → `Page_Load` (first load) → `btnSearch_Click(null,null)` (`.cs:41-44`) → `GetList("Market")` (default radio) → `gv_market` shows `MarketName` per pending row (key `MarketStructureTranferId`). [CONFIRMED]
2. **Choose type** via `rbType` (AutoPostBack) → `rbType_SelectedIndexChanged` → `btnSearch_Click` → clears all five grids → `GetList(<radio value>)` → binds only the matching grid (`Sub-Territory`→`gv_subTerri`/`TSubTerritoryName`, `Territory`→`gv_Terrritory`/`TTerritoryName`, `Area`→`gv_Area`/`TAreaName`, `Zone`→`gv_Zone`/`TRegionName`). Only the destination-level name is displayed — no source/destination parent, no requester/date. [CONFIRMED]
3. **Tick rows** (row `chkSelect`; header select-all handlers per grid). The "Effective Date" input is never read (BR-M03-022). The From/To selector block is `visible=false` (not usable). [CONFIRMED]
4. **Submit** (`btnSave_Click`, `.cs:279`) → JS confirm → `Validation()`: grid has rows ("Table can not be Empty!"), ≥1 ticked ("Please Select at least one row !!!") (BR-M03-019/020). [CONFIRMED]
5. Collect ticked `MarketStructureTranferId`s → CSV → `UpdateMarketStructure_TransferApprove(Type, CSV, UserId)`: for each id `UpdateData("sp_Update_MarketStructure_Approve", {@MasterId=id,@ApprovedBy=UserId,@Type=Type})`; all in one implicit transaction (BR-M03-021, BR-M03-101); rollback + rethrow on exception; `isSuccess` = last call. [CONFIRMED]
6. Success → `successalert(…,'MarketStructure_TransferApprove.aspx')` (reload → approved rows disappear if the list SP filters pending — INFERRED); failure → "Operation Faild!". [CONFIRMED]

## Data changed
Approval SP effect: **UNKNOWN**. Reasonable INFERENCE (not proven): marks the request approved (ApprovedBy/date) and applies the parent change to the hierarchy master tables; the master tables are `UNKNOWN`.

## Status transitions
`Requested` → `Approved` (only). No reject, no partial approval per level, no approver hierarchy, no expiry (BR-M03-026).

## Branches / exceptions
- Empty grid → alert; nothing ticked → alert.
- `GetList` failure → empty grid (swallowed `catch`).
- Hidden From/To controls still run `Page_Load` with `GetHigharcyInfoByEmployeeId`; for AM/DZSM/NSM/MIO users with no hierarchy row they throw `IndexOutOfRangeException`/`ArgumentOutOfRange` and break the whole page (INFERRED, `IVMarketStructureFrom_To.ascx.cs:113-138`).
- Reset → `Customer_Doctor_Transfer.aspx` (unrelated page).
- Dead safety net: `CheckCustInOrder` ("Order Pending!" via `sp_check_Vali_MarketStructure`/`CustomerTransfer`) exists but is not wired to any control on this page.

## Postconditions
Selected requests approved; page reloads.

## Related BR / pages
BR-M03-019…026, 101…103. PAGE-099 (+ PAGE-095/096/100/101/102 upstream).

## Tests
| ID | Scenario | Expected |
|---|---|---|
| WF2-T1 | Approve 3 Area requests | 3 approve calls in one txn; success |
| WF2-T2 | Approve with none ticked | alert |
| WF2-T3 | Switch radio to Zone | only zone grid |
| WF2-T4 | Requester approves own request | allowed |
| WF2-T5 | DB error mid-batch | all rolled back |
| WF2-T6 | Effective date entered | ignored |
