# WF-M01-02 — Page guard and DB-driven menu rendering (NewMasterPage)

**Trigger:** any GET of a page whose `MasterPageFile` is `~/MasterPages/NewMasterPage.master` (435 of 706 `.aspx` files). Actor: logged-in user.

**Correction of generated text:** the menu HTML is produced by the **scalar function `dbo.MainMenu2(@UserId,@UserRoleID)`**, not by `sp_GET_MainMenuRole2`.

## Steps
| # | Layer | Detail | Evidence |
|---|---|---|---|
| 1 | Master `Page_Load` | `ShowForcePasswordChangeModal = ShouldShowForcePasswordChangeModal()` on every request (see WF-M01-03) | `NewMasterPage.master.cs:17` |
| 2 | Master | **only if `!IsPostBack`**: if `Session["UserId"] != null` → continue; else set `UserId/LoginName/UserType/ComUnitId` to `""` and `Response.Redirect("../Login.aspx")` (BR-M01-013) | `:19-62` |
| 3 | Master | header: `lblName = EmpName + " (" + EmpMasterCode + ")"` (code omitted if empty), `lblDgs = DesigName` (BR-M01-017) | `:28-39` |
| 4 | Master | `MenuHtml()` → `UserBLL.LoadMenu(UserId, UserRoleID)` → `UserDAL.LoadMenu` → `SInventorySql.GetDataTable("SELECT dbo.MainMenu2(@UserId,@UserRoleID) AS MenuHTML")`; `menu.InnerHtml = Rows[0][0]` (BR-M01-014) | `:69-75`, `UserDAL.cs:33-41` |
| 5 | Any exception in 2–4 (e.g. `Session["EmpMasterCode"]` null, DB error, missing row) | `catch` → blank the four session keys, redirect to `../Login.aspx` | `:51-61` |
| 6 | Page content renders inside `ContentPlaceHolder1` | – | master |

## What the function returns / how permissions feed it
`MainMenu2` body is **not in the repository (UNKNOWN)**. Its inputs are the user id and role id, and the role menu is maintained by M02 PAGE-027 (`UserPermission/UserPermission.aspx`, `sp_Save_MenuRole`, `MenuRoleDAO` flags Add/View/Edit/Delete/Permission). Whether the function uses `tblMenuDistribution` (user grants), the role table filled by `sp_Save_MenuRole`, or both is UNKNOWN. The static menu markup in the master (`NewMasterPage.master:918-…`) is commented-out template leftovers.

## Legacy master (`MainMasterPage`, 130 pages)
Guard on **every** request (`Session["UserId"]` null/empty → redirect). Menu built in C#: `Convert.ToInt32(Session["UserId"])==1` → `PanalClsDAL.MainMenu()/SubItem()/…` (whole `tblMainMenu`), else `MainMenu(userId)` joins `tblMenuDistribution` (BR-M01-029).

## Data
Read-only (function/SQL); no writes.

## Branches / exceptions
- Postback or page-method call: no guard.
- Empty menu string → blank menu (no error).
- After "logout" (WF-M01-05) `Session["UserId"]` is `""` (not null) → step 2 passes.

## Related BR: BR-M01-013, 014, 017, 029. **Pages:** all NewMasterPage pages. **Gaps:** GAP-M01-002, 010, 011, 019, 020, 025.

## Tests
| ID | Scenario | Expected |
|---|---|---|
| T1 | GET a page with no session | redirect `../Login.aspx` (through catch: `Session["UserId"]` becomes `""`) |
| T2 | second GET in the same never-logged-in session | `Session["UserId"]` is `""` (non-null) → passes step 2 → `Session["EmpMasterCode"].ToString()` NRE → catch → redirect |
| T3 | POST to a page method of any NewMasterPage page without session | executes (no guard) |
| T4 | user with role lacking a menu row | menu hides it; direct URL still opens unless the page has its own check (only 17 pages do — see M02 README) |
