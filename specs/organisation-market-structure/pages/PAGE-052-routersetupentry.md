---
page_id: PAGE-052
module: M03
page: RouterSetupEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-052 — RouterSetupEntry

> **Evidence level:** Tier A. `RouterSetupEntry.aspx` (628 lines, ≈550 JS), `.aspx.cs` (63 lines), `Library.DAL/DoctorModule_DAL/RouterSetup.cs`, `RouterMaster` DAO read in full. SP bodies absent (`UNKNOWN`). **Legacy/likely broken page** — several defects below are certain from static reading; runtime confirmation is recommended before relying on them. Modern equivalent: PAGE-062 RouteInformationEntry.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-052 / M03 |
| Name | RouterSetupEntry ("Router Setup", `.aspx:13`; "Router" = *route*) |
| Kind | Entry / Transaction — route master + market picker (checkbox grid grouped by territory) |
| Folder / master | DoctorModule_UI / NewMasterPage.master |
| Class | `DoctorModule_UI_RouterSetupEntry`; static `RouterSetup _routerSetup` |
| Duplicate/legacy | Yes — overlaps PAGE-062 (route information) and the older "Router" concept (`RouterSetupView` PAGE-053) |

## 2. Purpose
CONFIRMED: create/edit a **Router (route) master** by name and select the **Markets** (listed under their Territory names) that belong to it.

## 3. Business Objective
INFERRED: an early/simple route definition (name + markets) used before the richer Route Information master (depot, DA, TA/DA, distances). Which downstream pages still use `tblRouter*` data is UNKNOWN.

## 4. Navigation
Back link `../DoctorModule_UI/RouterSetupView.aspx`; from list: `RouterSetupEntry.aspx?id=` (`RouterSetupView.aspx:17,133`). Success → `alert('Data saved successfully')` then `RouterSetupView.aspx` (`.aspx:474-476`).

## 5. Route
`~/DoctorModule_UI/RouterSetupEntry.aspx[?id=<RouterMasterId>]`

## 6. Source Files
`Solution.Web/DoctorModule_UI/RouterSetupEntry.aspx(.cs)`; `Library.DAL/DoctorModule_DAL/RouterSetup.cs` (namespace `SalesSolution.Web.DataLayer`); `Library.DAO/DoctorModule_DAO/RouterMaster.cs` (+`RouterDetails`); page loads jQuery 3.6, select2, jquery-ui, DataTables from `../assets`.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `#mainName` | Route Name | **Yes** (client) | empty | `required="true"` attr + JS check | – | tooltip "Please fill out of this field!" |
| `#tableDetail` (dynamic HTML table) | left cell = Territory `Name` (large font), right cell = one checkbox line per Market (`MarketName`) + hidden `Territory[i].TId`, `Market[i].MId` | ≥1 checked | unchecked (new); **all checked** (edit, `check()`) | – | `sp_Get_MarketByTerriTory` / `sp_Get_MarketByTerriTory_ByRouterMasterId` | `_errorWithMsg("Please!! Add Details")` |
| `#btnSave` | Submit / "Update Information" (edit) | – | – | – | – | – |
| Reset button | Reset | – | – | `onclick="ConfirmationClick()"` — function **not defined** in this page (no-op/JS error unless defined in shared scripts) | – | – |
Also a second, empty "Router Entry" card (`.aspx:102-124`) and a stray literal `}` before `</asp:Content>` (`.aspx:625`) that renders as text.

## 8. Field Mapping
| UI | JSON | DTO | SP param |
|---|---|---|---|
| `#masterId` | `RouterMasterId` | `RouterMaster.RouterMasterId` | `@id` |
| `#mainName` | `RouterName` | `RouterName` | `@RouterName ` (trailing space in the name, `RouterSetup.cs`) |
| checked rows | `RouterDetails[{TerritoryId,MarketId}]` | `RouterDetails.TerritoryId/MarketId` | `@TerritoryId `, `@MarketId ` (trailing spaces), `@id`=`RouterDetailsId` (always 0), `@RouterMasterId` |
| Session UserId | – | – | `@EntryBy` (new) / `@UpdateBy` (edit) |
The JS posts `{ 'masterData': jsonData }` (`.aspx:462`) but the WebMethod parameter is named **`router`** (`.aspx.cs:22`) — ASP.NET page methods bind JSON properties to parameters by name (BR-M03-084).

## 9. Dropdown Mapping
No dropdowns. The market grid is built client-side from `GetmarketByTerryTori` JSON (rows sorted by `Name` = territory; grouped when `Name` changes).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Submit | `Save()` → `Validation()` → `FinalSave()` | `$.ajax` POST `RouterSetupEntry.aspx/Save_RouterSetup` → `RouterSetup.Save_RouterSetup(router, int UserId)` | `sp_check_RouterMaster`, `sp_Update_RouterMaster`, `sp_Save_RouterMaster`, `sp_Save_RouterDetails` | write |
| Reset | `ConfirmationClick()` (not defined here) | – | – | – |

## 11. Frontend Flow
`$(function)`: `?id` → `#masterId`; `id>0` → `GetData(id)` else `GetDegree()`. **`GetDegree` is declared twice** (`.aspx:241` and `.aspx:328`); JavaScript keeps the *later* declaration, whose URL is `'RouterSetupEntry.aspx/"GetmarketByTerryTori'` (stray `"`), so the new-entry market list request targets a non-existent method and the grid is never populated (BR-M03-085). Edit path: `GetData` → `GetRouterEditData` (uses `data.RouterName` directly, not `data.d`, while all other handlers use `.d` — probable undefined values, INFERRED) → `GetRouterDetails(id)` → `GetmarketByTerryTori_ById` → renders grid then `check()` ticks **every** checkbox. `Validation()` counts unchecked boxes over `Rowcount` and rejects when none is checked. `FinalSave` collects `Territory[i].TId`/`Market[i].MId` for checked rows only; success alert + redirect; error/else branches are empty (commented out) — failures are silent.

## 12. Backend Flow
`Save_RouterSetup` (`[WebMethod]` without `EnableSession`, uses `HttpContext.Current.Session["UserId"]`, → `Convert.ToInt32`) → `RouterSetup.Save_RouterSetup`:
- **Edit** (`id>0`): `sp_check_RouterMaster(@id,@RouterName)` rows=0 → `sp_Update_RouterMaster(@id,@RouterName ,@UpdateBy)`; rows>0 → `isSuccess=false`.
- **New**: `pk = sp_Save_RouterMaster(@id,@RouterName ,@EntryBy)`; `pk>0` → success.
- If `pk>0 && isSuccess`: loop `router.RouterDetails` (NullReference if the client sent none) → `sp_Save_RouterDetails(@id=0,@RouterMasterId,@TerritoryId ,@MarketId )` per row; `isSuccess`=last result. One implicit transaction (BR-M03-101); exceptions rollback + rethrow. No delete of old details in C#.

## 13. API Mapping
| WebMethod | Notes |
|---|---|
| `Save_RouterSetup(RouterMaster router)` | JSON param name mismatch with client (`masterData`) |
| `GetmarketByTerryTori()` | all markets by territory (`sp_Get_MarketByTerriTory`) |
| `GetmarketByTerryTori_ById(int Id)` | `sp_Get_MarketByTerriTory_ByRouterMasterId(@id)` |
| `GetRouterMasterList()` | `sp_GET_RouterMasterInfo(@Parameter=" ")` (list page) |
| `GetRouterEditData(int id)` | returns `RouterMaster` via `GetSqlDataReader("sp_Get_RouterMaster_ByRouterMasterId")` (reader read but never closed explicitly; connection closed in finally) |
None uses `EnableSession=true`; only `Save_RouterSetup` touches Session.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`RouterSetup.Save_RouterSetup`, `GetRouterMasterList`, `GetRouterMasterForEdit`, `GetMarketByTerriTory`, `GetMarketByTerriToryById` (Library.DAL/DoctorModule_DAL/RouterSetup.cs). Unrelated leftover methods in the class: `ActiveInactive_DepartmentInfo`, `Delete_employeeleave` (call department SPs — copy/paste residue).

## 16. Stored Procedure Mapping
`sp_Save_RouterMaster`, `sp_Update_RouterMaster`, `sp_check_RouterMaster`, `sp_Save_RouterDetails`, `sp_Get_MarketByTerriTory`, `sp_Get_MarketByTerriTory_ByRouterMasterId`, `sp_Get_RouterMaster_ByRouterMasterId`, `sp_GET_RouterMasterInfo` — all bodies NOT in repository.

## 17. Database Mapping
No inline SQL; tables UNKNOWN (naming suggests `tblRouterMaster`/`tblRouterDetails`, INFERRED from DAO class names only).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Route name | non-empty | yes | none | "Please fill out of this field!" | BR-M03-086 |
| Markets | ≥1 checked | yes (`Validation`) | none (NullReference if none) | "Please!! Add Details" | BR-M03-086 |
| Duplicate on edit | `sp_check_RouterMaster` | – | DAL | none shown (silent) | BR-M03-087 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-084 | Save request cannot bind: client sends property `masterData`, server parameter is `router` → the WebMethod is invoked with a missing-parameter error, so **saving does not work as written** (unless a shared script rewrites the payload — not found). | RouterSetupEntry.aspx:462; RouterSetupEntry.aspx.cs:22 | JS/WebMethod | CONFIRMED (code) / INFERRED (runtime) |
| BR-M03-085 | The effective `GetDegree()` (second declaration) requests a malformed URL, so on **new** entry the territory/market grid stays empty and Validation always fails with "Please!! Add Details". | RouterSetupEntry.aspx:241 vs 328-331 | JS | CONFIRMED (code) |
| BR-M03-086 | A router needs a name and at least one ticked market (client-side only; server does not validate and dereferences `RouterDetails` unguarded). | RouterSetupEntry.aspx:193-224; RouterSetup.cs | JS/DAL | CONFIRMED |
| BR-M03-087 | On edit a duplicate router name (per `sp_check_RouterMaster`) silently returns `isSuccess=false` (no message shown). | RouterSetup.cs; RouterSetupEntry.aspx:473-481 | DAL/JS | CONFIRMED |
| BR-M03-088 | In edit mode every listed market checkbox is auto-ticked after load (`check()`), so an unchanged Update re-posts all listed markets as the route's members. | RouterSetupEntry.aspx:609-621 | JS | CONFIRMED |

## 20. Permission Matrix
Session: `UserId` only in `Save_RouterSetup` (unguarded). The other WebMethods are anonymous-callable if reachable (no `<authorization>` in Web.config; INFERRED). No role checks; menu-only.

## 21. Status Lifecycle
None handled (`RouterMaster.IsActive` exists in DAO but no code path sets/reads it on this page; commented `customSwitch1` code).

## 22. Error Handling
Client: failures silent (empty `error` and `else`). Server: exceptions rethrown → HTTP 500. No logging.

## 23. Audit
`@EntryBy`/`@UpdateBy` = UserId; timestamps SP-side UNKNOWN.

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
shared JS helpers (`_errorWithMsg`), `../assets` libraries; `RouterSetupView` (PAGE-053) list.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open new form | market grid empty (defect) | BR-M03-085 |
| TC-02 | Save with empty name | field flagged | BR-M03-086 |
| TC-03 | Save with nothing ticked | "Please!! Add Details" | BR-M03-086 |
| TC-04 | Save valid (if grid loads) | network shows missing param `router` error | BR-M03-084 |
| TC-05 | Open edit | every market pre-ticked | BR-M03-088 |
| TC-06 | Edit rename to existing | silent failure | BR-M03-087 |

## 28. Reverse Engineering References
`Solution.Web/DoctorModule_UI/RouterSetupEntry.aspx(.cs)`, `RouterSetupView.aspx:17,133`, `Library.DAL/DoctorModule_DAL/RouterSetup.cs`, `Library.DAO/DoctorModule_DAO/RouterMaster.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Working save/new flow | None existed | likely non-functional | – | DEFECT | BR-M03-084/085 |

## 30. Known Gaps
Payload/param-name mismatch; duplicated/typo'd `GetDegree`; undefined `ConfirmationClick`; `data.` vs `data.d`; trailing-space parameter names (`"@RouterName "`, `"@TerritoryId "`, `"@MarketId "`) — behaviour with SQL Server RPC UNKNOWN; leftover unrelated DAL methods; stray `}` rendered; unguarded `RouterDetails` loop; unclosed reader; SP bodies absent (GAP-M03-001).

## 31. Change Impact
`RouterSetup` DAL is used only by PAGE-052/053 (grep of `class RouterSetup` consumers within page code-behinds). Any change to `sp_GET_RouterMasterInfo` affects PAGE-053. Overlaps PAGE-062 domain (routes) — reconcile before retiring.
