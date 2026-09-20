---
page_id: PAGE-008
module: M01
page: UserPermission (CommonUI, legacy per-user menu grant)
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-008 — UserPermission (`CommonUI`, legacy)

> **Evidence level:** Tier A — aspx, code-behind, `PanalBLL`, `PanalDAL` (all inline SQL read), `MasterPage.master(.cs)` read. Do not confuse with PAGE-027 `UserPermission/UserPermission.aspx` (M02, role-based Add/View/Edit/Delete menu permission).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-008 |
| Module | M01 (INFERRED from folder) — functionally an access-administration page |
| Kind | Admin maintenance (legacy) |
| Folder | `Solution.Web/CommonUI` |
| Master page | `~/MasterPages/MasterPage.master` (**the broken legacy master** — see §22/§30) |
| Class | `CommonUI_UserPermission` |
| Size | aspx 446 lines (about half is a commented-out earlier UI, `<%-- ... --%>` from line 231), code-behind 279 |

## 2. Purpose
Grant or revoke, per user, visibility of menu rows in the legacy `tblMainMenu` tree: choose a user, tick main menus and child menus, Save (grant) or Delete (revoke with cascade).

## 3. Business Objective
Older, per-user (not per-role) access control for navigation: decide which screens a given user sees. INFERRED from names and SQL.

## 4. Navigation
No inbound links found. Whether it is reachable through DB menu rows is UNKNOWN. Because it uses `MasterPage.master`, opening it (GET) executes `MasterPage.master.cs` `Page_Load → MenuHtml()` which reads `aDataTable.Rows[0][0]` from an **empty `DataTable`** (`LoadMenu` call commented out, `MasterPage.master.cs:37-38`) → `IndexOutOfRangeException` on first load. So the page cannot render in its current state (CONFIRMED by code; not executed).

## 5. Route
`~/CommonUI/UserPermission.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | `Solution.Web/CommonUI/UserPermission.aspx`, `.aspx.cs` |
| Master | `Solution.Web/MasterPages/MasterPage.master(.cs)` |
| BLL / DAL | `Library.BLL/Panal_BLL/PanalBLL.cs`, `Library.DAL/Panal_DAL/PanalDAL.cs` |

## 7. UI Control Inventory
| Control | Type | Behaviour |
|---|---|---|
| userDropDownList | DropDownList (autopostback) | filled from `select * from tblUser` (`UserName`/`UserId`), first load only; change → loads main-menu grid, filters `mainMenuDropDownList`, clears `menuGridView` |
| mainMenuDropDownList | DropDownList (autopostback) | main menus already granted to the user; change → loads sub-level grid |
| mainMenuGridView | GridView | rows: `mainMenuCheckBox` (checked+disabled if granted), Save (`CommandName=SaveData`), Delete (`Remove`) |
| menuGridView | GridView | rows `menuCheckBox` (+ header `menuAllCheckBox`), per-row Save/Delete, header Save (`allsaveMenuImageButton`) |
| msgLabel | Label | "Data Save" (SeaGreen), "Permission Delete", "Select First" (Red), "Eroor!!" (Red, sic) |
Whole content is inside an UpdatePanel (`UpdatePanel2`).

## 8. Field Mapping
| UI | Handler | BLL → DAL | SQL param | Table.column |
|---|---|---|---|---|
| userDropDownList.SelectedValue | all handlers | `PanalBLL.SaveMainMenu/SaveMenuBll/MenuPermissionRemove` | `@UserId` | `tblMenuDistribution.UserId` |
| GridView DataKey SL | `SaveData`/`Remove` | same | `@MenuSL` / `@SL` | `tblMenuDistribution.MenuSL` |
| constant `true` | – | `PanalDAL.SaveMainMenu` | `@Status` | `tblMenuDistribution.Status` |
| generated `MAX(SL)+1` | – | `ClsPrimaryKeyFind.PrimaryKeyMax("SL","tblMenuDistribution","SSIDB")` | `@SL` | `tblMenuDistribution.SL` |

## 9. Dropdown Mapping
`PanalDAL.UserDdl` (`select * from tblUser`); `PanalDAL.MainMenu(userId)` (granted main menus: `tblMainMenu` INNER JOIN `tblMenuDistribution` where parent null/empty).

## 10. Button Action Mapping
| Action | Handler | Chain |
|---|---|---|
| Save main-menu row | `mainMenuGridView_RowCommand` "SaveData" | tick required and checkbox enabled → `PanalBLL.SaveMainMenu` → `PanalDAL.SaveMainMenu` (INSERT grant) |
| Delete main-menu row | "Remove" | tick required → `PanalBLL.MenuPermissionRemove` (4 DELETEs, cascade) |
| Save sub-level row | `menuGridView_RowCommand` "SaveData" | `PanalBLL.SaveMenuBll` (grant + up to 2 ancestors) |
| Delete sub-level row | "Remove" | `MenuPermissionRemove` |
| Header "Save all" | `allsaveMenuImageButton_Click` | loops rows, for every enabled+checked row calls `PanalBLL.SaveMainMenu(sl,user)` — **note: `SaveMainMenu`, not `SaveMenuBll`, so ancestors are not auto-granted here** |
| Header "select all" | `menuAllCheckBox_CheckedChanged` | sets all enabled row checkboxes |

## 11. Frontend Flow
UpdatePanel partial postbacks only; no custom JS beyond markup.

## 12. Backend Flow
`SaveMenuBll(sl,userId)` (`PanalBLL.cs:37-56`): insert grant for `sl`; then twice: read `ParantId` of the current SL (`PanalDAL.GetParantId`, `Rows[0]` unguarded); if non-empty, move to parent and insert a grant for the parent if none exists (`CheckMenuSl`). Always returns `true`. `MenuPermissionRemove` executes four DELETEs on `tblMenuDistribution`: the row itself, children, grand-children, great-grand-children (nested `ParantId IN (...)`), always returns `true` regardless of results. Each statement is autocommit; no transaction.

## 13. API Mapping
None.

## 14. Service Mapping
`PanalBLL`: `UserDdl`, `MainMenuLoad`, `OtherMenuLoad`, `MainMenu`, `SaveMainMenu`, `SaveMenuBll`, `MenuPermissionRemove`.

## 15. Repository Mapping
`PanalDAL.cs`: `UserDdl` (:111), `MainMenuLoad` (:125, union of granted/ungranted top-level rows), `OtherMenuLoad` (:143, three-level parent search, granted vs `Status=0`), `MainMenu` (:117), `SaveMainMenu` (:94), `CheckMenuSl` (:63), `GetParantId` (:80), `MenuPermissionRemove` (:30). All parameterised.

## 16. Stored Procedure Mapping
None.

## 17. Database Mapping
`tblMenuDistribution` (SL, UserId, MenuSL, Status) INSERT/DELETE; `tblMainMenu` READ; `tblUser` READ.

## 18. Validation Matrix
| Rule | Message | BR |
|---|---|---|
| Row tick required to save/delete | `Select First` | BR-M01-026 |
| Disabled (already granted) checkbox cannot be saved again | (silently skipped) | BR-M01-026 |
| No user-selected check (empty `userDropDownList.SelectedValue` → `Convert.ToInt32("")` throws) | unhandled exception | – |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M01-026 | A legacy menu grant is a row `(UserId, MenuSL, Status=true)`; already-granted rows are shown checked+disabled and cannot be re-saved. | `PanalDAL.cs:94-107`, `UserPermission.aspx.cs:34-43,164-176` | CONFIRMED |
| BR-M01-027 | Granting a sub-level menu also grants its parent and grandparent if not already granted (two upward hops). Bulk "Save all" does not do this. | `PanalBLL.cs:37-56`; `UserPermission.aspx.cs:75` | CONFIRMED |
| BR-M01-028 | Revoking a menu removes the grants of that menu and of all descendants down to three levels. | `PanalDAL.cs:30-44` | CONFIRMED |
| BR-M01-029 | In the legacy master (`MainMasterPage`) the user whose `UserId==1` sees the whole `tblMainMenu` (no grant lookup); everyone else sees only granted rows. | `MainMasterPage.master.cs:48` | CONFIRMED |

## 20. Permission Matrix
No session or role read in the page (`Session` not referenced). `MasterPage.master` has **no login guard** at all. Menu-based hiding only; no server-side role check. (The page is unusable anyway, see §4.)

## 21. Status Lifecycle
`tblMenuDistribution.Status` is always written `true`; rows are deleted to revoke. No other state.

## 22. Error Handling
None (no try/catch). Unhandled `IndexOutOfRange` in `MasterPage.master.cs:39` on GET.

## 23. Audit
None (no who/when columns written).

## 24. Integration
_Integration and Reports are documented together in this block._

None.

## 25. Reports
See §24 block above (combined Integration / Reports).

## 26. Dependencies
`PanalBLL`, `PanalDAL`, `ClsPrimaryKeyFind`, `MasterPage.master`, AjaxControlToolkit reference.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | GET page | server error (master `Rows[0]` on empty table) | – |
| TC-02 | (if master were fixed) grant a level-3 menu | row + parent + grandparent rows | 027 |
| TC-03 | revoke a level-1 menu | rows of levels 1-4 removed for that user | 028 |
| TC-04 | Save a checked+disabled row | ignored | 026 |
| TC-05 | Save with no user chosen | exception | – |

## 28. Reverse Engineering References
`Solution.Web/CommonUI/UserPermission.aspx(.cs)`, `Solution.Web/MasterPages/MasterPage.master(.cs)`, `Library.DAL/Panal_DAL/PanalDAL.cs`, `Library.BLL/Panal_BLL/PanalBLL.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Intended use | – | page cannot load; live permission admin is PAGE-027 | GAP-M01-017 |

## 30. Known Gaps
GAP-M01-017 (broken master, dead page), GAP-M01-019 (two permission systems), GAP-M01-018 (no authorization/audit).

## 31. Change Impact
Shares `PanalDAL`/`PanalBLL` with PAGE-002. `tblMenuDistribution` is read by `MainMasterPage.master.cs` via `PanalClsDAL.MainMenu/SubItem/...`. `MasterPage.master` is also the master of 7 other pages (`DoctorModule_UI/AsmRecords`, `AsmSetup`, `CommonDataLoad`, `Setup`, `SInventory_UI/SampleInvoiceCreationByOrder`, `SubDepotStockOutApproval`, `Target_UI/CommonDataLoad`).
