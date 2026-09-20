# Navigation

## How navigation works (verified)
- **Menu is DB-driven.** `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` executes `SELECT dbo.MainMenu2(@UserId,@UserRoleID)`; the returned HTML is put in `menu.InnerHtml`. The function body and menu rows are not in the repo → exact menu tree per role is **UNKNOWN**. The static menu markup in master pages is commented-out template code. Older `MainMasterPage.master.cs` builds a menu from `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`.
- **Master pages:** `NewMasterPage.master` (current), `MainMasterPage.master`, `MasterPage.master`. 89 pages use none of them (stand-alone, incl. most report viewers).
- **Routing:** file-based (`~/Folder/Page.aspx`); the only alias is `user-records` (`Global.asax`).
- **Landing:** after login → `Dashboard_UI/AdminDashboard.aspx` if `IsMainDashboard`, else a validated `aspxerrorpath` return URL, else `Dashboard_UI/DashboardOne.aspx` (`Login.aspx.cs`).
- **In-app links:** Entry/View/Edit pages link to each other via `Response.Redirect`; per-page inbound/outbound references are listed in each page spec §4. Menu maintenance pages: `UserPermission`, `AdminMenuPanal`.

## Folder → module map
| Web folder | Pages | Module(s) |
|---|---|---|
| (root) | 2 | M01, M13 |
| Approval_UI | 12 | M18 |
| CommonUI | 7 | M01 |
| DWSP | 10 | M14 |
| Dashboard_UI | 3 | M13, M16 |
| DoctorMaster_UI | 16 | M16 |
| DoctorModule_UI | 101 | M02, M03, M04, M15, M16, M17 |
| DoctorVisit_UI | 10 | M16 |
| LeaveProcess_UI | 4 | M15 |
| MasterSetup_UI | 42 | M02, M03, M04, M05, M06, M07, M15, M16 |
| Money_Receipt_UI | 1 | M09 |
| NoticeBoard_UI | 4 | M15 |
| PromoAlloc | 6 | M04 |
| Reports_UI | 16 | M02, M10, M14, M15, M16 |
| SAP_Integration | 4 | M17 |
| SInventory_RPTVIEW | 93 | M04, M05, M06, M07, M08, M09, M10, M11, M12, M13, M15, M17 |
| SInventory_UI | 325 | M02, M03, M04, M05, M06, M07, M08, M09, M10, M11, M12, M13, M14, M15, M17 |
| SettingPanel_UI | 2 | M02 |
| SubDepot_UI | 19 | M12 |
| Target_UI | 5 | M14 |
| Thana_UI | 5 | M03 |
| TransferUI | 13 | M03, M05 |
| UserPermission | 2 | M02 |
| UserProfile_UI | 2 | M01 |
| UserTracking | 1 | M01 |
| eProgram_UI | 1 | M17 |

Details: `specs/authentication-navigation/README.md` (login, session, menu), `docs/reverse-engineering/page-inventory.md`.
