---
page_id: PAGE-002
module: M01
page: AdminMenuPanal
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-002 — AdminMenuPanal ("MAIN MENU PANAL")

> **Evidence level:** Tier A — `AdminMenuPanal.aspx`, `.aspx.cs`, `PanalBLL`, `PanalDAL`, `ClsPrimaryKeyFind`, `ClsCommonInternalDAL.SaveDataByInsertCommand` (parameterised overload) read. Table DDL for `tblMainMenu` is not in the repository (PARTIAL).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-002 |
| Module | M01 — Authentication, Session & Navigation |
| Kind | Admin entry form (legacy menu maintenance) |
| Web folder | `Solution.Web/CommonUI` |
| Master page | `~/MasterPages/MainMasterPage.master` (legacy layout; guard: `Session["UserId"]` null or empty → redirect `../Login.aspx` on **every** request, `MainMasterPage.master.cs:17-20`) |
| Page class | `CommonUI_AdminMenuPanal` |
| Size | aspx 427 lines, code-behind 396 lines |
| Legacy status | Belongs to the older `tblMainMenu`/`tblMenuDistribution` menu system (see §31); the active master `NewMasterPage` renders `dbo.MainMenu2(...)` instead. Whether this page is still reachable from the live menu is UNKNOWN (menu rows are DB data). |

## 2. Purpose
Lets an administrator add rows to the legacy navigation tree in four accordion panes: Main Menu (level 1), Menu (level 2 under a main menu), Sub-Menu (level 3), Child Menu (level 4). Each new row can optionally point to a page (`../<folder>/<page>`).

## 3. Business Objective
Extend the navigation structure without a code deployment: register a new page/menu label so it can later be granted to users (see PAGE-008 for the legacy per-user grant). INFERRED from labels and DAL SQL.

## 4. Navigation
Not linked from other code (grep for `AdminMenuPanal` in `Solution.Web` finds only the page itself). Reachable only through a DB menu row or direct URL. No outbound navigation. The generated statement "loaded by `sp_GET_MainMenuRole2` in NewMasterPage" is wrong (see PAGE-001 §4).

## 5. Route
`~/CommonUI/AdminMenuPanal.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View | `Solution.Web/CommonUI/AdminMenuPanal.aspx` (jQuery UI accordion, `hidAccordionIndex` keeps open pane) |
| Code-behind | `Solution.Web/CommonUI/AdminMenuPanal.aspx.cs` |
| BLL | `Library.BLL/Panal_BLL/PanalBLL.cs` (`MenuSaveBll`, `MainMenuDropdown`, `MenuDropdown`) |
| DAL | `Library.DAL/Panal_DAL/PanalDAL.cs` (`MenuSaveDal`, `MainMenuDropDown`, `MenuDropDown`) |
| Helpers | `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs`, `ClsCommonInternalDAL.cs` |
| DAO | `Library.DAO/Panal_Entities/ObjPanal.cs` |

## 7. UI Control Inventory
| Pane | Controls | Required (server-side) | Notes |
|---|---|---|---|
| 1 Main Menu | `mainMenuNameTextBox`, `addPageCheckBox` (autopostback shows `divAddpage`), `mainMenuDesFolderTextBox`, `mainMenuPageNameTextBox`, `saveMainMenuButton` | name always; folder+page only if checkbox ticked | `ParantId=null` |
| 2 Menu | `ddlMMenuNameMPanal` (main menu), `menuNameTextBox`, `addPageCheckBoxMenu`, `menuDisFolderTextBox`, `menuPageTextBox`, `menuSaveButton` | dropdown, name; folder+page if ticked | parent = selected main menu |
| 3 Sub-Menu | `ddlSubMMenuNameMPanal` (autopostback → fills `ddlMenuNameForSubMenu`), `ddlMenuNameForSubMenu`, `subMenuNameTextBox`, `addPageCheckBoxSubMenu`, `submenuDisFolderTextBox`, `submenuPageTextBox`, `subMenuSaveButton` | second-level dropdown, name; folder+page if ticked | parent = second-level menu |
| 4 Child Menu | `ddlChMMenuNameMPanal`, `ddlMenuNameForChildMenu` (autopostback), `ddlsubMenuName`, `childMenuNameTextBox`, `childDisFolderTextBox`, `childMenuPageTextBox`, `saveChildMenu` | all four; folder and page **always** required | URL always set |
| Shared | `msgLabel` (green success / red error, cleared on postback), `hidAccordionIndex` | – | – |

Default values: none. No validators in markup; all checks are in code-behind (§18).

## 8. Field Mapping
| UI control | DTO property | Handler | BLL → DAL | SQL param | Column |
|---|---|---|---|---|---|
| main/menu/sub/child name TextBox | `ObjPanal.ManuName` | `MainMenuSave` / `MenuSave` / `SubMenuSave` / `ChildMenuSave` | `PanalBLL.MenuSaveBll` → `PanalDAL.MenuSaveDal` | `@ManuName` | `tblMainMenu.ManuName` (sic) |
| folder + page TextBoxes | `ObjPanal.URL = "../"+folder+"/"+page` or `"#"` | same | same | `@URL` | `tblMainMenu.URL` |
| parent dropdown | `ObjPanal.ParantId` (string) | same (`null` for pane 1) | same | `@ParantId` | `tblMainMenu.ParantId` (sic) |
| (generated) | `ObjPanal.SL = PrimaryKeyMax("SL","tblMainMenu","SSIDB")` = `MAX(SL)+1` | `PanalBLL.MenuSaveBll` (`PanalBLL.cs:19-23`) | – | `@SL` | `tblMainMenu.SL` |

## 9. Dropdown Mapping
All loaded by inline SQL on `tblMainMenu` (connection "SSIDB"): `MainMenuDropdown` = `select * from tblMainMenu where ParantId is null or ParantId=''` (text `ManuName`, value `SL`) for the three first-level dropdowns (loaded in `LoadDDL`, on first load and after every save); `MenuDropdown(ddl,id)` = `... where ParantId = @ParantId` for the cascaded dropdowns.

## 10. Button Action Mapping
| Button | Handler | Chain |
|---|---|---|
| saveMainMenuButton | `saveMainMenuButton_Click` (accordion 0) | `MainMenuValidation` → `MainMenuSave` |
| menuSaveButton | `menuSaveButton_Click` (1) | `MenuValidation` → `MenuSave` |
| subMenuSaveButton | `subMenuSaveButton_Click` (2) | `SubMenuValidation` → `SubMenuSave` |
| saveChildMenu | `saveChildMenu_Click` (3) | `ChildMenuValidation` → `ChildMenuSave` |

## 11. Frontend Flow
jQuery UI accordion (`jquery-ui.js`, `../scripts/jquery*.js`); the open pane index round-trips in `hidAccordionIndex` set by every handler. Page is a plain postback form; no AJAX/PageMethods.

## 12. Backend Flow
`Page_Load`: first load `LoadDDL()`; postback clears `msgLabel`. Each save: validate → build `ObjPanal` → `PanalBLL.MenuSaveBll` (reads `MAX(SL)+1` with `SELECT (isnull(MAX(SL),0)+1) FROM tblMainMenu`, then `INSERT INTO dbo.tblMainMenu (SL, ManuName, URL, ParantId)` through the parameterised overload `ClsCommonInternalDAL.SaveDataByInsertCommand(query, params, "SSIDB")` (`ClsCommonInternalDAL.cs:424-448`, direct `GetSqlStringCommand`, `ExecuteNonQuery() > 0`)) → on `true`: success text, clear inputs, `LoadDDL()`. If save returns `false` nothing is shown (no else branch). No try/catch, no transaction.

## 13. API Mapping
None (no WebMethods).

## 14. Service Mapping
`PanalBLL.MenuSaveBll`, `MainMenuDropdown`, `MenuDropdown`.

## 15. Repository Mapping
| DAL | SQL |
|---|---|
| `PanalDAL.MenuSaveDal` (`PanalDAL.cs:16`) | `INSERT INTO dbo.tblMainMenu (SL, ManuName, URL, ParantId) VALUES (...)` parameterised, DB "SSIDB" |
| `PanalDAL.MainMenuDropDown` (:48) / `MenuDropDown` (:53) | SELECT on `tblMainMenu` |

## 16. Stored Procedure Mapping
None. (The 1-argument overload `SaveDataByInsertCommand(string)` at `ClsCommonInternalDAL.cs:178-190` wraps the SQL text in SP `ExecuteAllSqlQueryByStoreProcedure`, but this page uses the parameterised overload, which runs the text directly.)

## 17. Database Mapping
`tblMainMenu` (SL, ManuName, URL, ParantId; other columns such as `IsApprovalPage` exist per `AppSetupDAL.LoadMenuName`) — READ and INSERT. No update/delete of menu rows exists anywhere in the M01 code (no edit/delete UI).

## 18. Validation Matrix
| Pane | Rule | Message (verbatim) | BR |
|---|---|---|---|
| Main | name non-empty; if page ticked → folder and page non-empty | `Fill Required TextBox!!` | BR-M01-023 |
| Menu | parent selected; name non-empty; if ticked → folder+page | `Select Required DropDown!!` / `Fill Required TextBox!!` | BR-M01-023 |
| Sub | second-level selected; name; folder+page if ticked | same | BR-M01-023 |
| Child | sub-menu selected; name; folder; page (mandatory) | same | BR-M01-023 |
No check for duplicate names/URLs, path syntax, or that the target file exists.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M01-023 | A menu row = `ManuName` + `URL` + `ParantId`. URL is `"../" + folder + "/" + page` when the "add page" box is ticked (always for child menus), otherwise `"#"` (a pure heading). Level-1 rows have `ParantId = null`. | `AdminMenuPanal.aspx.cs:98-106,164-172,220-228,284-286` | code-behind | CONFIRMED |
| BR-M01-024 | New row id `SL` = `MAX(SL)+1` computed in a separate query before the insert (no lock/identity → race). | `PanalBLL.cs:19-23`, `ClsPrimaryKeyFind.cs:12-18` | BLL | CONFIRMED |
| BR-M01-025 | The tree has at most four levels in this UI (main → menu → sub-menu → child); dropdown cascades enforce parent-of-parent selection. | `AdminMenuPanal.aspx.cs:341-371` | code-behind | CONFIRMED |

## 20. Permission Matrix
Only the master-page login check (`Session["UserId"]` non-empty). **No role or user-type check** in this page (`Session` is never read in the code-behind) — any logged-in user who knows the URL can add menu rows. Menu-based hiding only.

## 21. Status Lifecycle
None (rows are created; there is no active/inactive flag written here).

## 22. Error Handling
No try/catch; DB exceptions propagate. A `false` result from `SaveDataByInsertCommand` produces no message.

## 23. Audit
None (no EntryBy/EntryDate written).

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
`PanalBLL`, `PanalDAL`, `ClsPrimaryKeyFind`, `ClsCommonInternalDAL`, AjaxControlToolkit registration (unused controls), jQuery UI.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Save main menu "X" without page | row (SL=max+1, URL `#`, ParantId null); "MainMenu Save" | 023, 024 |
| TC-02 | Save main menu with ticked page but blank folder | `Fill Required TextBox!!` | 023 |
| TC-03 | Save child menu with blank page | `Fill Required TextBox!!` | 023 |
| TC-04 | Child pane without picking sub-menu | `Select Required DropDown!!` | 025 |
| TC-05 | Two admins save simultaneously | possible duplicate SL (race) | 024 |
| TC-06 | Logged-in low-privilege user opens URL | page renders and saves (no role check) | – |
| TC-07 | No session | redirect `../Login.aspx` | – |

## 28. Reverse Engineering References
`Solution.Web/CommonUI/AdminMenuPanal.aspx(.cs)`, `Library.DAL/Panal_DAL/PanalDAL.cs`, `Library.BLL/Panal_BLL/PanalBLL.cs`, `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Authorization | None existed | none beyond login | GAP-M01-018 |
| Menu source | – | writes legacy `tblMainMenu`, not the role menu function used by NewMasterPage | GAP-M01-019 |

## 30. Known Gaps
GAP-M01-018 (no authorization, no duplicate check, `MAX+1` key, no audit), GAP-M01-019 (two menu systems), GAP-M01-020 (`dbo.MainMenu2` body unknown).

## 31. Change Impact
`PanalDAL/PanalBLL` menu methods are shared with PAGE-008 (`CommonUI/UserPermission`). `tblMainMenu` is also read by `MainMasterPage`, `PanalClsDAL.MainMenu/SubItem/SubSubItem/SubSubChildItem`, `AppSetupDAL.LoadMenuName`, `ApprovalSetupEntry`, and `DeStockOutDal/WarehouseStockInApprovalDal.GetMenuIdByMenuName` (URL `LIKE`), so a changed `URL` value alters approver look-ups (see M02 PAGE-019).
