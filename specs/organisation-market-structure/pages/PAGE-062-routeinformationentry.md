---
page_id: PAGE-062
module: M03
page: RouteInformationEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-062 — RouteInformationEntry

> **Evidence level:** Tier A. `RouteInformationEntry.aspx` (505 lines) and `.aspx.cs` (861 lines) read in full, plus `RouteInformationDAL` (all 451 lines), `RouteInformationList.aspx.cs`, `OrderInfoDAL.LoadSC`, `IVMarketStructure.ascx(.cs)`, `DataAccessManager`. SP bodies absent (`UNKNOWN`). Process: `workflows/WF-M03-05-route-information-setup.md`. The older, simpler "Router" page is PAGE-052.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-062 / M03 |
| Name | RouteInformationEntry ("Route Information", `.aspx:18`) |
| Kind | Entry / Transaction — master + 3 detail lists (create/edit) |
| Folder / master | MasterSetup_UI / NewMasterPage.master |
| Class | `MasterSetup_UI_RouteInformationEntry` |

## 2. Purpose
CONFIRMED: define a **distribution route** for a Depot (DC / optionally sub-depot): route name, type, total distance & days, TA (travel allowance) and DA (daily allowance) amounts, the weekdays it runs, the **delivery associates (DA)** assigned, and the ordered list of **markets** on the route with the distance of each stop.

## 3. Business Objective
INFERRED: routes group markets into deliverable runs so depots can plan delivery (route/DA selection appears in invoicing/delivery helper SPs `sp_GET_TerritoryByRouteInformationDDL`, `sp_GET_DelTerritoryByRouteInformationDDL` in `RouteInformationDAL`), compute route cost (TA/DA) and prevent a market from being served by two routes (BR-M03-063).

## 4. Navigation
Reached from `RouteInformationList.aspx` (PAGE-063): *New* → `RouteInformationEntry.aspx`, *Edit* → `?MID=<RouteInformationMasterId>` (`RouteInformationList.aspx.cs:36,68`). Success → `RouteInformationList.aspx` (`.cs:839`).

## 5. Route
`~/MasterSetup_UI/RouteInformationEntry.aspx[?MID=<id>]`

## 6. Source Files
`Solution.Web/MasterSetup_UI/RouteInformationEntry.aspx(.cs)`; user control `Solution.Web/MasterSetup_UI/IVMarketStructure.ascx(.cs)` (all six rows visible); `Library.DAL/MasterSetup_DAL/RouteInformationDAL.cs`; `Library.BLL/SInventory_BLL/OrderInfoBLL.LoadSC(ddl,userId)` → `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:978`; `CommonDataLoad`; DAO `RouteInformationMasterDAO`, `RouteInformationDADetailDAO`, `BonusCampaignMarketDetailDAO` (re-used campaign DAO for market rows). `DepotWiseAreaSetupDal _aRepo` is declared but unused.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `ddlDepotName` (AutoPostBack) | DC | **Yes** (`.cs:613`) | first item (list is "select"+units) | – | `OrderInfoBLL.LoadSC` → inline SQL `tblCompanyUnit WHERE ComUnitId IN (SELECT CompanyUnitId FROM tblUserCompanyUnit WHERE UserId=@UserId)` | tooltip "please fill out this field" |
| `chkIsSubDepo` | Is Sub-Depot | no | unchecked | – | – | – |
| `txtRouteName` | Route Name | **Yes** (`.cs:621`) | empty | – | – | tooltip |
| `ddlDAName` | DA Name | required to add | placeholder | reloaded on depot change | `sp_Get_DDLDANameByDCID(@depoId)` | tooltip |
| `addButtonDA` → `gv_DA` | Add To List | ≥1 row (`.cs:630`) | empty | delete link per row | in-memory | alert "Please Add to List DA Name!" |
| structure control (Group…Market) | Group/Zone/Area/Territory/Sub-Territory/Market | Market required to add (`.cs:472`) | placeholder | role-locked ([MS-CTRL]) | cascade SPs (Active) | tooltip |
| `txtDistance` (+ FilteredTextBoxExtender `0123456789.`) | Distance(km) | required to add (`.cs:474`) | empty | – | – | tooltip |
| `btnAddtoListMarket` → `gv_Market` | Add To List | ≥1 row (`.cs:639`) | empty | per row: hidden ids, editable `txtGridDistance` (`oninput=updateMarketDistanceTotal()`), Move Up/Down, Remove | in-memory | alert "Please Add to List Market!" |
| `txtTotalDistance` | Total Distance(KM) | **Yes** (`.cs:649`) | auto-filled | recomputed on every list operation and on Save | Σ grid distance | tooltip |
| `txtTotalDay` | Total Day | **Yes** (`.cs:658`) | empty | numeric filter | – | tooltip |
| `ddlRouteType` | Route Type | **Yes** (`.cs:666`) | placeholder | – | `sp_Get_RouteTypeInfo` | tooltip |
| `txtTAAmount` | TA Amount | **Yes** (`.cs:674`) | empty | numeric filter | – | tooltip |
| `txtDAAmount` | DA Amount | **Yes** (`.cs:682`) | empty | numeric filter | – | tooltip |
| `ddlRouteDay` (ListBox, multi) | Route Day | ≥1 selected (`.cs:691`) | none | – | `sp_Get_WeekNameInfo` | tooltip |
| `btnSave` / `btnUpdate` | Submit / Update | – | `btnSave` visible on new, `btnUpdate` on edit; both → `btnSave_Click` | sweetalert confirm | – | – |
| `btnReset` | Reset | – | – | handler is empty (`.cs:857-860`) — does nothing | – | – |

## 8. Field Mapping
| UI | DTO | Code | SP param |
|---|---|---|---|
| `id_mastetID` | `RouteInformationMasterId` | 0 if empty (`.cs:807`) | `@RouteInformationMasterId` |
| `ddlDepotName` | `DCId` | `.cs:808` | `@DCId` |
| `chkIsSubDepo` | `IsSubDepo` | `.cs:809` | `@IsSubDepo` |
| `txtRouteName` | `RouteName` | `.cs:811` | `@RouteName` |
| `txtTotalDay` | `TotalDay` (decimal) | `.cs:815` `decimal.Parse` | `@TotalDay` |
| `txtTotalDistance` | `TotalDistance` (decimal) | `.cs:814` | `@TotalDistance` |
| `ddlRouteType` | `RouteTypeId` | `.cs:818` | `@RouteTypeId` |
| `txtTAAmount` / `txtDAAmount` | `TAAmount` / `DAAmount` | `.cs:820-821` | `@TAAmount` / `@DAAmount` |
| Session UserId | – | DAL | `@EntryBy` (both insert and update) |
| `gv_DA` rows (`hfDANameId`) | `RouteInformationDADetailDAO.DAId` | `.cs:785-803` | `sp_Save_RouteInformationDADetail(@RouteInformationMasterId,@DAId)` |
| `gv_Market` rows (hidden ids + `txtGridDistance`) | `BonusCampaignMarketDetailDAO{GroupId,RegionId,AreaId,TerritoryId,SubTerritoryId,MarketId,Distance}` | `.cs:745-780` | `sp_Save_RouteInformationMarketDetail(@RouteInformationMasterId,@GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId,@MarketId,@Distance)` |
| `ddlRouteDay` selected values | CSV `RouteDayArray` | `.cs:824-835` | `sp_Save_RouteInformationWeekNameDetails(@RouteInformationMasterId,@WeekNameId)` per day |
| market ids CSV | `MarketIdStr` | `.cs:781` | `sp_check_RouteInformationMArket(@id,@Name,@Status)` |
Edit load maps `dt["BrandId"]` (a column carrying the route-day ids — name reused from another page) to `ddlRouteDay` selections (`.cs:125-137`).

## 9. Dropdown Mapping
Depot ← `LoadSC(userId)`; DA ← `sp_Get_DDLDANameByDCID`; Route Type ← `sp_Get_RouteTypeInfo` (`Value`/`TextField`); Route Day ← `sp_Get_WeekNameInfo` (`Value`/`TextField`); hierarchy ← structure control (Active-only cascade). `ddlRouteDay.Items.Insert(-1,"")` (`.cs:204`) is invalid for a `ListItemCollection` and throws inside the `try/catch{}` after binding, so `SelectedIndex=0` is never executed (no default selected day).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| depot change | `dc_SelectedIndexChanged` `.cs:53` | `LoadDANameDDL` → `RouteInformationDAL.GetDANameDDL` | `sp_Get_DDLDANameByDCID` | read |
| Add DA | `addButtonDA_Click` `.cs:237` | `Add()` | – | in-memory |
| remove DA | `deleteImageButton_Click` `.cs:498` | `Remove(row)` | – | in-memory |
| Add market | `btnAddtoListMarket_Click` `.cs:467` | `MarketValidation` → `AddMarket` (append, sort by distance asc) | – | in-memory |
| Move ↑/↓ | `MarketMoveUp_Click` `.cs:518` / `MarketMoveDown_Click` `.cs:540` | swap rows | – | in-memory |
| Remove market | `MarketdeleteImageButton_Click` `.cs:508` | `RemoveMarket` | – | in-memory |
| Submit/Update | `btnSave_Click` `.cs:734` | `Validation()` → `RouteInformationDAL.SaveRouteInformation` | see §16 | write |
| Reset | `btnReset_Click` `.cs:857` | empty | – | – |

## 11. Frontend Flow
Server-rendered UpdatePanel form. Client JS `updateMarketDistanceTotal()` (`.aspx:338-352`) sums all `.grid-distance` inputs (`parseFloat`, ignores NaN) and writes the total (`toFixed(2)` with trailing zeros stripped; empty when 0) into `txtTotalDistance` on each keystroke in a grid distance box. Server recomputes on every add/remove/move and once more at Save (`UpdateTotalDistanceFromGrid`, format `"0.##"`) before validating. `pageLoad()` re-inits select2 and the multi-select.

## 12. Backend Flow
1. First load: `LoadDropdownList()` (depot via `LoadSC`, weekdays, route types, empty market/DA grids); `?MID` → `btnUpdate` visible + `GetOneRecord`: `sp_GET_RouteInformationMaster_ById` (DCId, IsSubDepo, RouteName, TotalDistance, TotalDay, TAAmount, DAAmount, RouteTypeId, BrandId=route days) → `sp_GET_RouteInformationDADetail_ById` → `sp_GET_RouteInformationMarketDetail_ById` (adds an empty `Distance` column if the SP lacks one). All in `try/catch{}`.
2. `btnSave_Click`: recompute total; `Validation()`; build DTO lists; `SaveRouteInformation` (RouteInformationDAL.cs):
   - **Edit** (`id>0`): `sp_check_RouteInformationMArket(@id=id,@Name=<market csv>,@Status="Update")`; rows=0 → `UpdateData("sp_UD_RouteInformationMaster")`, `pk=id`; rows>0 → `isDuplicateCheck=true`.
   - **New**: `sp_check_RouteInformationMArket(@id=1,@Name=csv,@Status="Entry")`; rows=0 → `pk = SaveDataReturnPrimaryKey("sp_Save_RouteInformationMaster")`, `pk>0` → success else `isDuplicateCheck=true`; rows>0 → `isValiCheck=true`.
   - If `pk>0`: one `sp_Save_RouteInformationDADetail` per DA, one `sp_Save_RouteInformationMarketDetail` per market row, one `sp_Save_RouteInformationWeekNameDetails` per selected day. `isSuccess` = result of the *last* call. C# never deletes previous detail rows before re-inserting on edit (SP behaviour UNKNOWN).
   - Whole call = one implicit transaction (BR-M03-101); exception → rollback + rethrow.
3. Result handling (`.cs:837-852`): `isSuccess` → success alert → list; then, **independently**, `if (isDuplicateCheck) faildalert("Market Already Exist in Another Route!") else faildalert("Already Exist!")` — registered under the same script key "Popup" as the success alert (a later same-key registration in the same async postback is ignored — INFERRED — so on success the success alert wins). Practical effect: entry-mode conflict (`isValiCheck`) shows "Already Exist!", edit-mode conflict shows "Market Already Exist in Another Route!", and a failed insert (pk≤0) also shows the *market* message (BR-M03-064).

## 13. API Mapping
None (`[WebMethod]` absent).

## 14. Service Mapping
BLL used only for the depot list: `OrderInfoBLL.LoadSC(ddl,userId)` → `OrderInfoDAL.LoadSC` (inline parametrised SQL via `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`).

## 15. Repository Mapping
| DAL method | Source | SP |
|---|---|---|
| `SaveRouteInformation` | RouteInformationDAL.cs (method after `GetWeekNameList`) | `sp_check_RouteInformationMArket`, `sp_UD_RouteInformationMaster`, `sp_Save_RouteInformationMaster`, `sp_Save_RouteInformationDADetail`, `sp_Save_RouteInformationMarketDetail`, `sp_Save_RouteInformationWeekNameDetails` |
| `GetDANameDDL` | same | `sp_Get_DDLDANameByDCID(@depoId)` |
| `GetRouteTypeInfoList` / `GetWeekNameList` | same | `sp_Get_RouteTypeInfo` / `sp_Get_WeekNameInfo` |
| `GetRouteInformationMasterById` / `GeteRouteInformationDA_DetailById` / `GetRouteInformationDetailMarketById` | same | `sp_GET_RouteInformationMaster_ById`, `sp_GET_RouteInformationDADetail_ById`, `sp_GET_RouteInformationMarketDetail_ById` |

## 16. Stored Procedure Mapping
All bodies **NOT in repository** — nature from call pattern: `sp_Save_RouteInformationMaster` (write, returns pk), `sp_UD_RouteInformationMaster` (update), `sp_Save_RouteInformationDADetail` / `…MarketDetail` / `…WeekNameDetails` (child inserts), `sp_check_RouteInformationMArket` (read; rows = market already present in another route — INFERRED from message text), read SPs above.

## 17. Database Mapping
No inline SQL for the route tables (SP only) → tables UNKNOWN. Depot list reads `tblCompanyUnit` and `tblUserCompanyUnit` (inline SQL, `OrderInfoDAL.cs:986-987`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| DC | required | – | `.cs:613` | tooltip | BR-M03-049 |
| Route name | required | – | `.cs:621` | tooltip | BR-M03-050 |
| DA list | ≥1 | – | `.cs:630` | alert "Please Add to List DA Name!" | BR-M03-051 |
| Market list | ≥1 | – | `.cs:639` | alert "Please Add to List Market!" | BR-M03-052 |
| Total distance | non-empty | – | `.cs:649` | tooltip | BR-M03-053 |
| Total day | non-empty | numeric filter | `.cs:658` | tooltip | BR-M03-054 |
| Route type | required | – | `.cs:666` | tooltip | BR-M03-055 |
| TA / DA amount | non-empty | numeric filter | `.cs:674,682` | tooltip | BR-M03-056/057 |
| Route day | ≥1 | – | `.cs:691` | tooltip | BR-M03-058 |
| Add DA | DA selected | – | `.cs:242` | tooltip | BR-M03-059 |
| Add market | market selected; distance non-empty; market not already in grid | – | `.cs:472-484,704-733` | tooltip / "This Market is already exist in list!" | BR-M03-060 |
| Market in another route | SP result | – | DAL | "Market Already Exist in Another Route!" / "Already Exist!" | BR-M03-063 |
| Numeric parse | `decimal.Parse` of totals/amounts | filter only | unguarded | unhandled `FormatException` on e.g. "1.2.3" | BR-M03-062 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-049 | A route belongs to exactly one depot (DC), chosen from the user's assigned company units (`tblUserCompanyUnit`). | RouteInformationEntry.aspx.cs:613,191-194; OrderInfoDAL.cs:986 | page | CONFIRMED |
| BR-M03-050 | Route name mandatory (no trim/length/uniqueness rule in C#). | .cs:621 | page | CONFIRMED |
| BR-M03-051 | ≥1 DA must be attached; DA choices depend on the depot (`sp_Get_DDLDANameByDCID`). | .cs:630,53-74 | page | CONFIRMED |
| BR-M03-052 | ≥1 market must be on the route. | .cs:639 | page | CONFIRMED |
| BR-M03-053 | Total distance mandatory (auto = Σ stop distances). | .cs:649,563-594 | page | CONFIRMED |
| BR-M03-054 | Total days mandatory. | .cs:658 | page | CONFIRMED |
| BR-M03-055 | Route type mandatory (`sp_Get_RouteTypeInfo`). | .cs:666 | page | CONFIRMED |
| BR-M03-056 | TA amount mandatory. | .cs:674 | page | CONFIRMED |
| BR-M03-057 | DA (allowance) amount mandatory. | .cs:682 | page | CONFIRMED |
| BR-M03-058 | ≥1 running weekday mandatory. | .cs:691 | page | CONFIRMED |
| BR-M03-059 | Adding a DA needs a selection; the same DA can be added repeatedly (no duplicate check). | .cs:237-256,267-294 | page | CONFIRMED |
| BR-M03-060 | Each market stop needs a market chosen (deepest level) and a distance; a market may appear once per route. | .cs:467-496,704-733 | page | CONFIRMED |
| BR-M03-061 | When a stop is added the stop list is re-sorted ascending by distance (non-numeric = last); users may reorder with Up/Down and edit distances afterwards (no re-sort). | .cs:407,422-453,518-561 | page | CONFIRMED |
| BR-M03-062 | Total distance = Σ numeric stop distances (invalid → 0), format `0.##`; recomputed on each change and before save (overwrites any manual edit of the total). Amount/day fields are parsed with `decimal.Parse` unguarded. | .cs:563-594,736,814-821 | page | CONFIRMED |
| BR-M03-063 | A market may not belong to two routes: `sp_check_RouteInformationMArket` is invoked with the CSV of market ids (`@Status` Entry/Update; edit passes the route id to exclude itself). Semantics of returned rows UNKNOWN (INFERRED = conflicts). | RouteInformationDAL.cs (SaveRouteInformation) | DAL | CONFIRMED (call) |
| BR-M03-064 | Feedback mapping is inconsistent: entry-mode conflict → `isValiCheck` (UI shows "Already Exist!"); edit-mode conflict or failed insert (pk≤0) → `isDuplicateCheck` (UI shows "Market Already Exist in Another Route!"). | .cs:837-852; DAL | page/DAL | CONFIRMED |
| BR-M03-065 | Master and all child rows (DA, market stops, weekdays) are saved in one transaction; children are re-inserted on edit without C# deletion; success flag = last child call. | RouteInformationDAL.cs | DAL | CONFIRMED / UNKNOWN (SP) |
| BR-M03-066 | "Is Sub-Depot" marks a route served from a sub-depot; no other logic depends on it in this page. Route-day ids are round-tripped through a column named `BrandId`. | .cs:809,108,125 | page | CONFIRMED |
| BR-M03-067 | (List page PAGE-063) Users with `Session["RoleTypeId"]=="5"` see all routes; others see only routes whose `DCId` is in `Session["DICUnitId"]` (SQL fragment ` and A.DCId in(<DICUnitId>)` concatenated into the SP parameter). | RouteInformationList.aspx.cs:45-53 | list page | CONFIRMED |

## 20. Permission Matrix
Session read: `UserId` (depot list, save; unguarded). Depot scoping by `tblUserCompanyUnit` rows of the user is the only data-level restriction on this page; the list page adds `RoleTypeId==5` / `DICUnitId` scoping (BR-M03-067). Edit by `?MID=` is not checked against the user's depots (any id can be loaded — INFERRED: SP `sp_GET_RouteInformationMaster_ById` receives only the id). No `<authorization>` in Web.config; menu-only page access.

## 21. Status Lifecycle
No status/active flag on the route in C# (no activate/deactivate/delete on this page). Existence only.

## 22. Error Handling
Edit load and every dropdown loader in `try/catch{}` (silent). Save: no try/catch — DAL rolls back and rethrows; numeric `FormatException` unhandled. Confusing double alert logic (BR-M03-064).

## 23. Audit
`@EntryBy`=UserId on both insert and update; dates SP-side UNKNOWN; no history of changed stops.

## 24. Integration
None visible.

## 25. Reports
None.

## 26. Dependencies
Structure control; `sp_check_RouteInformationMArket`; masters DA (`sp_Get_DDLDANameByDCID`), route type, week names; consumers of route data in delivery/invoicing (`GetTerritoryByRouteInformationDA_DDLId` etc. in `RouteInformationDAL`; callers outside module).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Save empty form | DC highlighted first | BR-M03-049 |
| TC-02 | Depot+name only | alert "Please Add to List DA Name!" | BR-M03-051 |
| TC-03 | Add DA, no market | alert "Please Add to List Market!" | BR-M03-052 |
| TC-04 | Add market with empty distance | distance highlighted | BR-M03-060 |
| TC-05 | Add same market twice | "This Market is already exist in list!" | BR-M03-060 |
| TC-06 | Add stops 30, 10, 20 km | list order 10,20,30; total 60 | BR-M03-061/062 |
| TC-07 | Edit a grid distance | total updates client-side and server-side | BR-M03-062 |
| TC-08 | No route day / route type / TA / DA / days | each highlighted in order | BR-M03-054..058 |
| TC-09 | Save market that is on another route (new) | "Already Exist!" | BR-M03-063/064 |
| TC-10 | Edit route adding a market of another route | "Market Already Exist in Another Route!" | BR-M03-063/064 |
| TC-11 | Total day "1.2.3" | unhandled FormatException | BR-M03-062 |
| TC-12 | Valid save | children saved in one txn; success → list | BR-M03-065 |
| TC-13 | Click Reset | nothing happens | §10 |
| TC-14 | Non-admin opens list | only own depot routes | BR-M03-067 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/RouteInformationEntry.aspx(.cs)`, `RouteInformationList.aspx.cs`, `Library.DAL/MasterSetup_DAL/RouteInformationDAL.cs`, `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:972-994`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Route uniqueness of markets | None existed | delegated to SP | absent | UNKNOWN | BR-M03-063 |
| Detail re-save on edit | None existed | re-insert without C# delete | absent | UNKNOWN | BR-M03-065 |

## 30. Known Gaps
Reset button no-op; inconsistent duplicate messages; `Insert(-1,"")`; unguarded numeric parsing; DAO reuse (`BonusCampaignMarketDetailDAO`, `BrandId` column) — copy/paste debt; list-page SQL fragment concatenation (BR-M03-067; session-sourced but still string-built SQL); no delete/deactivate route; no per-route uniqueness of name; SP bodies absent (GAP-M03-001).

## 31. Change Impact
`RouteInformationDAL` also feeds RouteInformationList (PAGE-063) and delivery/payment dropdown helpers; `sp_check_RouteInformationMArket` only here (grep). Markets deactivated/transferred (PAGE-042/096) can leave routes pointing at inactive/moved markets — no cross-check in C#.
