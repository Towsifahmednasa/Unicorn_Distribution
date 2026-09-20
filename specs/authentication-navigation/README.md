# M01 — Authentication, Session & Navigation

Reviewed 2026-09-19 (reverse-engineered; documentation only, no prior specification existed). Confidence tags: CONFIRMED / INFERRED / UNKNOWN.

## 1. Purpose & business overview
This module is the front door of the ERP: it authenticates a user against `tblUser`, builds the ASP.NET session that all other modules read (user, role, role type, company unit, employee display data, dashboard preference), renders the role-driven navigation menu, forces a password change when flagged, records a best-effort login-session audit row, and lets a user change their own password. It also contains a few legacy/utility pages (legacy menu admin, demo/template pages, a field-employee location viewer).

Facts about the platform that every reader needs (all CONFIRMED):
- ASP.NET WebForms, .NET 4.0; `Web.config:80-83`: `authentication mode="Forms"` with `loginUrl="~/Login.aspx"`, `sessionState mode="InProc" timeout="24000"`. **No code issues a Forms auth cookie**; effective authentication is `Session["UserId"]` (see GAP-M01-002).
- Three master pages: `NewMasterPage` (435 pages; guard on first GET only), `MainMasterPage` (130 pages; guard on every request; legacy per-user menu), `MasterPage.master` (8 pages; no guard; broken). 133 `.aspx` files have no master page (Login, 132 without any `Session["UserId"]` reference incl. 92 report viewers).
- Menu for `NewMasterPage` comes from **`SELECT dbo.MainMenu2(@UserId,@UserRoleID)`** (`Library.DAL/SInventory_DAL/UserDAL.cs:33-41`), body not in repo. (Coordinator correction: the generated Tier-B pages wrongly say `sp_GET_MainMenuRole2`; that SP belongs to the role-menu admin path in M02.)

## 2. Page catalogue
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-001 | `Login.aspx` | Authentication entry | A | [pages/PAGE-001-login.md](pages/PAGE-001-login.md) |
| PAGE-002 | `CommonUI/AdminMenuPanal.aspx` | Legacy menu admin (tblMainMenu) | A | [pages/PAGE-002-adminmenupanal.md](pages/PAGE-002-adminmenupanal.md) |
| PAGE-003 | `CommonUI/Default.aspx` | UI demo (hard-coded sample grid) | B | [pages/PAGE-003-default.md](pages/PAGE-003-default.md) |
| PAGE-004 | `CommonUI/HomePage.aspx` | Empty landing template; redirect target of 3 Excel-upload pages | B | [pages/PAGE-004-homepage.md](pages/PAGE-004-homepage.md) |
| PAGE-005 | `CommonUI/JqueryAccordian.aspx` | UI template | B | [pages/PAGE-005-jqueryaccordian.md](pages/PAGE-005-jqueryaccordian.md) |
| PAGE-006 | `CommonUI/JqueryTab.aspx` | UI template | B | [pages/PAGE-006-jquerytab.md](pages/PAGE-006-jquerytab.md) |
| PAGE-007 | `CommonUI/TemplatePage.aspx` | UI template | B | [pages/PAGE-007-templatepage.md](pages/PAGE-007-templatepage.md) |
| PAGE-008 | `CommonUI/UserPermission.aspx` | Legacy per-user menu grant (page cannot load) | A | [pages/PAGE-008-userpermission.md](pages/PAGE-008-userpermission.md) |
| PAGE-009 | `UserProfile_UI/ChangePassword.aspx` | Self-service password change | A | [pages/PAGE-009-changepassword.md](pages/PAGE-009-changepassword.md) |
| PAGE-010 | `UserProfile_UI/UserProfile.aspx` | Legacy weak duplicate of ChangePassword | A | [pages/PAGE-010-userprofile.md](pages/PAGE-010-userprofile.md) |
| PAGE-011 | `UserTracking/UserTrackingList.aspx` | Employee GPS trail map/list | A | [pages/PAGE-011-usertrackinglist.md](pages/PAGE-011-usertrackinglist.md) |

Shared, non-page components documented here: `MasterPages/NewMasterPage.master(.cs)`, `MainMasterPage.master(.cs)`, `MasterPage.master(.cs)`, `App_Code/UserSessionTrackingManager.cs`, `Global.asax`.

## 3. Business rules index
| ID | One-liner | Documented in |
|---|---|---|
| BR-M01-001 | User name then password must be non-blank (trimmed) | PAGE-001 |
| BR-M01-002 | Login = `LoginName` + plaintext `Password` equality + `UserStatus='active'` | PAGE-001 |
| BR-M01-003 | `TOP 1` without ORDER BY; `ComUnitId` non-deterministic for multi-unit users | PAGE-001 |
| BR-M01-004 | Session keys created at login | PAGE-001 |
| BR-M01-005 | `ComUnitId` set only for non-Admin `UserType` | PAGE-001 |
| BR-M01-006 | `IsPasswordChange` null→0 (must change); missing column→"True" | PAGE-001 |
| BR-M01-007 | Landing: IsMainDashboard → AdminDashboard; deep-link; else DashboardOne | PAGE-001 |
| BR-M01-008 | Deep-link ignored only if it starts with `http` | PAGE-001 |
| BR-M01-009 | No lockout/attempt counter; `tblLoginLog` write disabled | PAGE-001 |
| BR-M01-010 | Login inserts `tblUserSessionTracking` row, errors swallowed | PAGE-001 |
| BR-M01-011 | Active tracking rows with same SessionId closed at login | PAGE-001 |
| BR-M01-012 | Client IP resolution from headers; public IPs geolocated via ipapi.co | PAGE-001 |
| BR-M01-013 | `NewMasterPage`: on first load (not postback) missing `Session["UserId"]` → blank keys, redirect `../Login.aspx`; any exception in header/menu code also redirects | WF-M01-02 (`NewMasterPage.master.cs:19-62`) — CONFIRMED |
| BR-M01-014 | Menu HTML = `dbo.MainMenu2(UserId, UserRoleID)` injected into `menu.InnerHtml` | WF-M01-02 (`NewMasterPage.master.cs:69-75`, `UserDAL.cs:33`) — CONFIRMED (function body UNKNOWN) |
| BR-M01-015 | Forced-change modal shown when `IsPasswordChange` is false/"false"/"0" on all pages except ChangePassword; UI-only | PAGE-009, WF-M01-03 |
| BR-M01-016 | Logout blanks 4 session keys and redirects; no Abandon, no tracking close | WF-M01-05 (`NewMasterPage.master.cs:88-95`) — CONFIRMED |
| BR-M01-017 | Header shows `EmpName (EmpMasterCode)` and `DesigName` | WF-M01-02 (`NewMasterPage.master.cs:28-39`) — CONFIRMED |
| BR-M01-018 | New password 12–20 chars with lower, upper, digit, special | PAGE-009 |
| BR-M01-019 | New password must differ from current (plaintext) | PAGE-009 |
| BR-M01-020 | Confirm must equal new | PAGE-009 |
| BR-M01-021 | Update sets Password (raw), IsPasswordChange=1, UpdateBy/Date; no old-password check | PAGE-009 |
| BR-M01-022 | UserProfile accepts any non-empty matching password; no policy | PAGE-010 |
| BR-M01-023 | Legacy menu row = name + `../folder/page` or `#` + parent | PAGE-002 |
| BR-M01-024 | Menu/grant `SL = MAX+1` | PAGE-002, PAGE-008 |
| BR-M01-025 | At most 4 menu levels in the admin UI | PAGE-002 |
| BR-M01-026 | Legacy grant row = (UserId, MenuSL, Status=true); granted rows locked | PAGE-008 |
| BR-M01-027 | Granting a child auto-grants parent and grandparent (not for "save all") | PAGE-008 |
| BR-M01-028 | Revoking removes descendants to 3 levels | PAGE-008 |
| BR-M01-029 | `MainMasterPage`: `UserId==1` sees whole tblMainMenu | PAGE-008, WF-M01-02 (`MainMasterPage.master.cs:49`) |
| BR-M01-030 | Location trail needs employee+date; blanks dropped; sorted by time; start/end labelled | PAGE-011 |
| BR-M01-031 | Excel export is client-side HTML `.xls` | PAGE-011 |

## 4. Status lifecycles
- **User account** (as seen by login): `tblUser.UserStatus = 'active'` required (M02 edits it: 'Active'/'Inactive').
- **Password-change flag:** `IsPasswordChange` 0/NULL → (ChangePassword/UserProfile) 1. Who resets to 0: UNKNOWN.
- **Session:** none → (login) authenticated → (logout: keys blanked, session object survives) → (timeout 24000 min / recycle) gone. Tracking row: `IsActiveSession` 1 → 0 only when a later login reuses the same SessionId.

## 5. Roles / permissions observed
Session carries `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType` (`Admin` or other), `CentralWareHouse`, company-unit ids. In M01 code only these are tested: `UserType=="Admin"` (ComUnitId), `IsPasswordChange`, `UserId==1` (legacy menu). Role-based checks elsewhere are documented in M02 README §"Authorization model as implemented".

## 6. Data model
| Table | Used by | Notes |
|---|---|---|
| `tblUser` | Login, ChangePassword, UserProfile | LoginName, Password (plain), UserStatus, UserTypeId, UserRoleID, EmpInfoId, daInfoId, IsPasswordChange, UpdateBy, UpdateDate, CentralWareHouse, IsMainDashboard |
| `tblUserCompanyUnit` | Login | user↔company unit(s); `CompanyUnitIdList`, `DICCompanyUnitId` |
| `tblEmpGeneralInfo`, `tblDAInfo`, `tblDesignation`, `tbl_UserRoleInfo`, `tblRoleType`, `tblUserType` | Login/profile display | – |
| `tblUserSessionTracking` | `UserSessionTrackingManager` (write only) | written, never read |
| `tblLoginLog` | none (dead) | – |
| `tblMainMenu`, `tblMenuDistribution` | legacy menu (PAGE-002/008, MainMasterPage) | – |
| `dbo.MainMenu2` (function) | NewMasterPage | body UNKNOWN |

## 7. Integrations
`https://ipapi.co/{ip}/json/` at login (`UserSessionTrackingManager.cs:495`); Leaflet/unpkg and OSM tiles in PAGE-011; Google Fonts on Login.

## 8. Reports
See [reports/RPT-M01-01-user-location-tracking.md](reports/RPT-M01-01-user-location-tracking.md). No login-audit report exists.

## 9. Workflows
- [WF-M01-01 Login and session bootstrap](workflows/WF-M01-01-login-and-session-bootstrap.md)
- [WF-M01-02 Page guard and DB-driven menu rendering](workflows/WF-M01-02-db-driven-menu-rendering.md)
- [WF-M01-03 Forced password change / change password](workflows/WF-M01-03-forced-password-change-and-change-password.md)
- [WF-M01-04 Legacy menu administration](workflows/WF-M01-04-legacy-menu-administration.md)
- [WF-M01-05 Logout and session end](workflows/WF-M01-05-logout-and-session-end.md)

## 10. Known-gaps summary
Full list in [GAPS-M01.md](GAPS-M01.md) (25 items). Headlines: plaintext passwords (001); Forms auth unused and no server-side authorization on WebMethods/no-master pages (002, 012); no lockout (003); weak session/cookie/config posture (004, 009); logout does not invalidate (010); forced-change is cosmetic (013); ChangePassword lacks old-password check and UserProfile bypasses policy (014, 015); dead legacy pages/master (017, 018); two menu systems (019); menu function body missing (020).

## 11. What is still unverified
- Body of `dbo.MainMenu2`; therefore exactly which table (user grants vs role flags) decides menu visibility.
- Database collation (case sensitivity of login/password) and whether `tblUser.Password` is ever transformed by a trigger.
- `sp_Get_UserLocationTracking`, and the source table of GPS points.
- Who sets `IsPasswordChange=0` (user-creation/reset SPs not in repo).
- Runtime confirmation of post-logout rendering and of page-method access without session (derived from code and config, not executed).
- Tier-B pages PAGE-003…007 were only skimmed (templates).
