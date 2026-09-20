# WF-M01-04 — Legacy menu administration (tblMainMenu / tblMenuDistribution)

**Trigger:** an administrator wants a new legacy menu entry or to grant/revoke menu items per user. Actors: admin user; PAGE-002 and PAGE-008.
**Status:** legacy — the active master (`NewMasterPage`) renders `dbo.MainMenu2`, while these tables drive `MainMasterPage` (130 pages) and the approver look-ups (`GetMenuIdByMenuName`, M02 WF-M02-06).

## A. Create a menu row (PAGE-002)
1. Admin fills one of four panes; server validation (`Fill Required TextBox!!` / `Select Required DropDown!!`) — `AdminMenuPanal.aspx.cs`.
2. `PanalBLL.MenuSaveBll`: `SL = SELECT ISNULL(MAX(SL),0)+1 FROM tblMainMenu` then `INSERT INTO dbo.tblMainMenu (SL, ManuName, URL, ParantId)`; URL = `../folder/page` or `#` (BR-M01-023/024).
3. Dropdowns reloaded; success message. No update/delete of rows exists.

## B. Grant/revoke per user (PAGE-008 — currently cannot load)
1. Select user → main-menu grid (`PanalDAL.MainMenuLoad`), select main menu → sub grid (`OtherMenuLoad`).
2. Save row: `SaveMenuBll` inserts `tblMenuDistribution(SL=MAX+1, UserId, MenuSL, Status=1)` for the menu and (if missing) parent and grandparent (BR-M01-027); "Save all" uses `SaveMainMenu` only.
3. Delete: four cascading DELETEs by parent chain (BR-M01-028).
4. Effect: `MainMasterPage` shows only granted rows (UserId=1 sees all; BR-M01-029).

## Data
`tblMainMenu` INSERT; `tblMenuDistribution` INSERT/DELETE; reads of `tblUser`.

## Exceptions
No transactions; no audit columns; `MAX+1` race; `GetParantId` `Rows[0]` unguarded (invalid SL → exception); PAGE-008 blocked by broken `MasterPage.master` (GAP-M01-017).

## Related BR: BR-M01-023…029. **Gaps:** GAP-M01-017, 018, 019.

## Tests
PAGE-002 §27, PAGE-008 §27.
