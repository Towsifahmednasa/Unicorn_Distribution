---
page_id: PAGE-001
module: M01
page: Login
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-001 — Login

> **Evidence level:** Tier A — `Login.aspx`, `Login.aspx.cs`, `PanalBLL.Login`, `PanalClsDAL.Login` (inline SQL, fully read), `UserSessionTrackingManager` (fully read) and `NewMasterPage.master(.cs)` were read directly. `database` is PARTIAL because table DDL (tblUser, tblUserCompanyUnit, tblEmpGeneralInfo, tblDAInfo, tblDesignation, tbl_UserRoleInfo, tblRoleType, tblUserSessionTracking) is not in the repository.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-001 |
| Module | M01 — Authentication, Session & Navigation |
| Page name | Login |
| Kind | Entry point / authentication form (CONFIRMED) |
| Web folder | Solution.Web (root) |
| Master page | none (stand-alone HTML page, `Solution.Web/Login.aspx:1`) |
| Page class | `Login` (`Solution.Web/Login.aspx.cs:11`) |
| aspx / code-behind size | 331 / 139 lines |
| Auth model | Forms authentication is configured (`Web.config:80-82`, `loginUrl="~/Login.aspx"`) but **no code calls `FormsAuthentication.SetAuthCookie`** (repo-wide grep, no hit). Effective login state is `Session["UserId"]` only (InProc session, `Web.config:83`). |

## 2. Purpose
Sign a user in with a login name and password, build the server-side session (identity, role, company/DC scope, employee display data) that every other page relies on, record a session-tracking row, and send the user to the correct landing dashboard (CONFIRMED, `Login.aspx.cs:18-125`).

## 3. Business Objective
Give ERP staff (admins, sales hierarchy, depot/DC users, DA users and mobile-app users share the same `tblUser`) a single gateway; the role, unit and dashboard preference stored on the user decide what they see afterwards. INFERRED for the business framing; the mechanics are CONFIRMED.

## 4. Navigation
- Inbound: `NewMasterPage.master.cs` redirects here on missing session (`:52`, `:61`) and on logout (`:94`); `MainMasterPage.master.cs:18`. `Web.config` `loginUrl`. Direct URL.
- Outbound (all `Response.Redirect`): `Dashboard_UI/AdminDashboard.aspx` (when `IsMainDashboard=true`), the URL in query string `aspxerrorpath` (if it does not start with `http`), otherwise `Dashboard_UI/DashboardOne.aspx` (`Login.aspx.cs:93-118`).
- Menu placement: not in the menu (public entry). The application menu itself is built by `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` → inline SQL `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (`Library.DAL/SInventory_DAL/UserDAL.cs:33-41`). **Correction to the generated Tier-B text:** the menu is *not* loaded by `sp_GET_MainMenuRole2` (that SP is used only by `MenuDAL` for the role-menu admin page, PAGE-027 in M02).

## 5. Route
`~/Login.aspx` (file route; no `MapPageRoute`, `Global.asax:9-13` maps only `user-records`). Optional query `?aspxerrorpath=<relative url>`.

## 6. Source Files
| Type | Path |
|---|---|
| View | `Solution.Web/Login.aspx` |
| Code-behind | `Solution.Web/Login.aspx.cs` |
| BLL | `Library.BLL/Panal_BLL/PanalBLL.cs` (`Login`, `LoginLog`) |
| DAL | `Library.DAL/PanalCls/PanalClsDAL.cs` (`Login` :15-52, `LoginLog` :69-80) |
| Session tracking | `Solution.Web/App_Code/UserSessionTrackingManager.cs` |
| Assets | `Solution.Web/VerticalAsset/*`, `Solution.Web/APK_File/Unicorn_vr(1.0.0).apk` (download link on the page) |

## 7. UI Control Inventory
| Control | Type | Label | Required | Default | Notes |
|---|---|---|---|---|---|
| userNameTextBox | TextBox | User Name | server-side only (`Login.aspx.cs:24`); no validator | empty | `Login.aspx:290`; trimmed before use |
| passwordTextBox | TextBox `TextMode=Password` | Enter Password | server-side only (`:27`) | empty | `Login.aspx:296`; show/hide eye toggles `type` via jQuery (`Login.aspx:32-46`); trimmed before use |
| loginButton | LinkButton "Sign in" | – | – | – | `OnClick=loginButton_Click`; form `defaultbutton="loginButton"` (Enter key submits) |
| msgLabel | Label | – | – | empty | CSS `alert alert-warning` on error |
| APK link | anchor | Download APK File (1.0.0) | – | – | static download `APK_File/Unicorn_vr(1.0.0).apk` (unauthenticated) |

No client-side validators; no CAPTCHA; no "remember me"; no forgot-password.

## 8. Field Mapping
| UI control | Variable | Handler | BLL | DAL | SQL parameter | Table.column |
|---|---|---|---|---|---|---|
| userNameTextBox | `loginName` | `loginButton_Click` | `PanalBLL.Login` | `PanalClsDAL.Login` | `@LoginName` (VarChar 100) | `tblUser.LoginName` |
| passwordTextBox | `passwordText` | same | same | same | `@Password` (VarChar 100) | `tblUser.Password` (compared as stored text — see BR-M01-002) |

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Sign in | `loginButton_Click` (`Login.aspx.cs:18`) | validate non-empty → `PanalBLL.Login` → build session → redirect |

## 11. Frontend Flow
Static page. JS only toggles password visibility. Full postback (no UpdatePanel/AJAX). External resources: Google Fonts CSS (`Login.aspx:21`).

## 12. Backend Flow
1. `loginButton_Click` clears `msgLabel`. If trimmed user name empty → "Input Use Name Please!!!" (`:130`). If password empty → "Input Password Please!!!".
2. `PanalBLL.Login(loginName,password)` → `PanalClsDAL.Login` (parameterised inline SQL; opens `DataBase.SalesDB`, `GetDataTableByText`, closes connection in `finally`). No try/catch in the page: a DB error surfaces as an unhandled exception.
3. SQL (paraphrased from `PanalClsDAL.cs:17-35`): `SELECT TOP 1` user row + `IsPasswordChange` (null→0) + `EmpMasterCode` (DA code for UserTypeId 6/7, employee code, or UserName if no employee) + `EmpName` + `DesigName` (UserType text for UserTypeId 6) + role type name + comma list of the user's `CompanyUnitId`s (`CompanyUnitIdList`) + first company unit by `UserComId` (`DICCompanyUnitId`) + `*` (all `tblUser`/`tblUserCompanyUnit`/… columns) `WHERE LoginName=@LoginName AND Password=@Password AND UserStatus='active'`. There is **no ORDER BY** on the outer query.
4. 0 rows → "User name or password incourect !!!!". 1 row → set session keys (`Login.aspx.cs:34-69`): `UserId, UserRoleID, RoleTypeId, RoleTypeName, LoginName, UserType, CentralWareHouse`; `ComUnitId=CompanyUnitId` **only when `UserType != "Admin"`**; `UserTime` (now, "f"); `EmpInfoId, EmpName, EmpMasterCode, DesigName, DICCompanyUnitId`; `IsPasswordChange` (column value, or literal `"True"` if the column is absent); `DICUnitId=CompanyUnitIdList` (exceptions swallowed).
5. `UserSessionTrackingManager.StartTracking(HttpContext.Current)` (:48) — see §23/§24.
6. Redirect precedence (:93-118): `IsMainDashboard` (parsed with swallowed exception) → `AdminDashboard.aspx`; else `aspxerrorpath` (UrlDecoded; if it starts with `http` → `DashboardOne.aspx`, otherwise `Response.Redirect(target)`); else `DashboardOne.aspx`.

No transaction; single read query.

## 13. API Mapping
No `[WebMethod]`, no handler.

## 14. Service Mapping
| BLL | Source |
|---|---|
| `PanalBLL.Login` | `Library.BLL/Panal_BLL/PanalBLL.cs` |
| `PanalBLL.LoginLog` (defined, **not called** — call commented at `Login.aspx.cs:46`) | same |

## 15. Repository Mapping
| DAL | Source | SQL |
|---|---|---|
| `PanalClsDAL.Login` | `Library.DAL/PanalCls/PanalClsDAL.cs:15` | inline SELECT, parameterised |
| `PanalClsDAL.LoginLog` | `PanalClsDAL.cs:69` | `INSERT INTO dbo.tblLoginLog (UserId, LoginName, LoginTime)` — unused |
| `UserSessionTrackingManager.*` | `Solution.Web/App_Code/UserSessionTrackingManager.cs` | direct ADO.NET, connection string name `SolutionConnectionStringSSIDB` |

## 16. Stored Procedure Mapping
None for login. (The menu function `dbo.MainMenu2` is used by the master page, body not in repository — UNKNOWN.)

## 17. Database Mapping
| Table | Access | Columns known (CONFIRMED from SQL text) |
|---|---|---|
| tblUser | READ | UserId, LoginName, Password, UserStatus, UserTypeId, UserType, UserRoleID, EmpInfoId, daInfoId, UserName, IsPasswordChange, CentralWareHouse, IsMainDashboard (from `*`) |
| tblUserCompanyUnit | READ | UserId, CompanyUnitId, UserComId |
| tblEmpGeneralInfo | READ | EmpInfoId, EmpMasterCode, EmpName, DesignationId |
| tblDAInfo | READ | DAId, DACode, Name |
| tblDesignation | READ | DesignationId, DesigName |
| tbl_UserRoleInfo | READ | UserRoleID, RoleTypeId |
| tblRoleType | READ | RoleTypeId, RoleType |
| tblUserSessionTracking | INSERT/UPDATE | UserId, LoginDateTime, LogoutDateTime, IPAddress, Country, RegionState, City, BrowserName, OSName, DeviceType, SessionId, LastActivityTime, IsActiveSession (identity `TrackingId`) |
| tblLoginLog | none (dead) | UserId, LoginName, LoginTime |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| User Name | non-empty after Trim | no | yes | `Input Use Name Please!!!` | BR-M01-001 |
| Password | non-empty after Trim | no | yes | `Input Password Please!!!` | BR-M01-001 |
| Credentials | match + active | no | yes (SQL) | `User name or password incourect !!!!` | BR-M01-002 |
| Redirect target | not starting with `http` | no | yes | (fallback to DashboardOne) | BR-M01-008 |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M01-001 | Both user name and password must be non-blank (after trimming) before any DB lookup; user name is checked first. Leading/trailing spaces in the password are silently removed. | `Login.aspx.cs:24-29,126-141` | code-behind | CONFIRMED |
| BR-M01-002 | A login succeeds only if a `tblUser` row has `LoginName = @LoginName AND Password = @Password AND UserStatus='active'`. The password is compared **as stored text (no hashing/salting)**; case sensitivity follows the DB collation (UNKNOWN, default SQL Server collations are case-insensitive → INFERRED). | `PanalClsDAL.cs:35,39-40` | SQL | CONFIRMED (logic) / INFERRED (case rule) |
| BR-M01-003 | Only the first row (`TOP 1`) is used. Because of the LEFT JOIN to `tblUserCompanyUnit` and no ORDER BY, `Session["ComUnitId"]` (= `CompanyUnitId` column of the joined row) is **not deterministic** for users with several company units; `DICCompanyUnitId` is deterministic (lowest `UserComId`). | `PanalClsDAL.cs:17-34` | SQL | CONFIRMED |
| BR-M01-004 | On success the following session keys are set: UserId, UserRoleID, RoleTypeId, RoleTypeName, LoginName, UserType, CentralWareHouse, UserTime, EmpInfoId, EmpName, EmpMasterCode, DesigName, DICCompanyUnitId, IsPasswordChange, DICUnitId (+ ComUnitId, see BR-M01-005). | `Login.aspx.cs:34-69` | code-behind | CONFIRMED |
| BR-M01-005 | `Session["ComUnitId"]` is set only when `UserType` is not `"Admin"`; Admin sessions have no ComUnitId (a stale value from a previous login in the same browser session would survive). | `Login.aspx.cs:42-45` | code-behind | CONFIRMED |
| BR-M01-006 | `IsPasswordChange` defaults to 0 (=must change) when null in DB (`isnull(IsPasswordChange,0)`), and to `"True"` when the column is absent from the result. It is stored in session as the string `"True"`/`"False"`. | `PanalClsDAL.cs:17`, `Login.aspx.cs:55-62` | SQL + code | CONFIRMED |
| BR-M01-007 | Landing page precedence: (1) user flag `IsMainDashboard=true` → `AdminDashboard.aspx`; (2) `aspxerrorpath` deep link; (3) `DashboardOne.aspx`. `IsDepotDashboard` is not read here. | `Login.aspx.cs:93-118` | code-behind | CONFIRMED |
| BR-M01-008 | Deep-link guard: a decoded `aspxerrorpath` beginning with `http` (any case) is ignored; anything else is redirected to as-is (e.g. `//host/path`, `javascript:` are not blocked). | `Login.aspx.cs:99-113` | code-behind | CONFIRMED |
| BR-M01-009 | There is no failed-attempt counter, lockout, delay, CAPTCHA or audit of failed logins; `tblLoginLog` write is commented out so **successful logins are not written to `tblLoginLog` either**. | `Login.aspx.cs:46`; grep `LoginLog(` shows no live caller | code-behind | CONFIRMED |
| BR-M01-010 | After a successful login `StartTracking` inserts a `tblUserSessionTracking` row (IP, geo, browser, OS, device, SessionId, IsActiveSession=1) and stores the identity id in session key `__UserSessionTrackingId`. Errors are swallowed ("Audit logging should not break the login flow"). | `UserSessionTrackingManager.cs:18-64` | App_Code | CONFIRMED |
| BR-M01-011 | Before inserting, any still-active tracking row with the same ASP.NET SessionId is closed (LogoutDateTime=now, IsActiveSession=0). | `UserSessionTrackingManager.cs:39,276-295` | App_Code | CONFIRMED |
| BR-M01-012 | Client IP is taken from the first non-private public address among X-Forwarded-For, X-Real-IP, CF-Connecting-IP, Client-IP, REMOTE_ADDR, UserHostAddress; public IPs are geolocated through an external HTTPS call to `ipapi.co` (3 s timeout, failure ignored); private/loopback IPs are labelled "Private Network/LAN" or "Local". Headers are client-supplied and trusted. | `UserSessionTrackingManager.cs:314-356,479-536` | App_Code | CONFIRMED |

## 20. Permission Matrix
- Anonymous access by design. Session keys read: none (query string `aspxerrorpath` only).
- Nothing on this page prevents an already-logged-in user from logging in again as someone else; old session keys not overwritten (e.g. `ComUnitId` for Admin) remain (BR-M01-005).
- Rate limiting / lockout: none (BR-M01-009).

## 21. Status Lifecycle
Only account-status coupling: user must have `UserStatus='active'` (lower-case literal in SQL; other pages write `'Active'`/`'Inactive'` — comparison relies on DB collation, INFERRED case-insensitive). Session lifecycle: created at successful login (`Session["UserId"]` set) → alive until IIS session timeout (`sessionState timeout="24000"` minutes ≈ 16.7 days, `Web.config:83`) or logout (M01 WF-04). Tracking row: IsActiveSession 1 → 0 only via `CloseExistingActiveSession` at next login on same SessionId (BR-M01-011); `MarkCurrentSessionLoggedOut` is never called.

## 22. Error Handling
No try/catch in page. `StartTracking`, `Session["DICUnitId"]` and `IsMainDashboard` parsing swallow exceptions. A SQL failure in `PanalClsDAL.Login` propagates as an unhandled exception (no `customErrors` element configured; `compilation debug="true"`, `Web.config:63`). No central `Application_Error` (root `Global.asax` has only `Application_Start`).

## 23. Audit
`tblUserSessionTracking` (login time, IP, geo, UA). `LastActivityTime` is never refreshed because `TouchCurrentSession` has no callers; logout time is never written because `MarkCurrentSessionLoggedOut` has no callers (repo-wide grep). `tblLoginLog` is not written. No page reads `tblUserSessionTracking` (no viewer in repo).

## 24. Integration
Outbound HTTPS GET `https://ipapi.co/{ip}/json/` (third-party geo-IP) for every non-private login IP; sets `ServicePointManager.SecurityProtocol` process-wide (`UserSessionTrackingManager.cs:494`).

## 25. Reports
None.

## 26. Dependencies
`PanalBLL`, `PanalClsDAL`, `DataAccessManager`/`DataBase.SalesDB` (`Library.DAL/DataManager/DataBase.cs:7` = `SalesDisDB_Unicorn_DB`), `UserSessionTrackingManager`, `Dashboard_UI/AdminDashboard.aspx`, `Dashboard_UI/DashboardOne.aspx`. DB credentials are hard-coded in `Library.DAL/DataManager/SqlUserAccess.cs` and `Web.config` (values not reproduced here).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Valid active user, `IsMainDashboard=1` | session set, redirect AdminDashboard | 004, 007 |
| TC-02 | Valid active user, no dashboard flag | redirect DashboardOne | 007 |
| TC-03 | Blank user name | "Input Use Name Please!!!" | 001 |
| TC-04 | Blank password / spaces only | "Input Password Please!!!" | 001 |
| TC-05 | Wrong password | "User name or password incourect !!!!", no session | 002 |
| TC-06 | `UserStatus='Inactive'` | login refused | 002 |
| TC-07 | 20 consecutive bad passwords | no lockout occurs (documents current behaviour) | 009 |
| TC-08 | `?aspxerrorpath=%2FSInventory_UI%2FX.aspx` after login | redirected to that path | 007, 008 |
| TC-09 | `?aspxerrorpath=https://evil.example` | fallback DashboardOne | 008 |
| TC-10 | `?aspxerrorpath=//evil.example` | redirected to external host (current behaviour) | 008 |
| TC-11 | User with 3 company units | `ComUnitId` may vary between logins; `DICCompanyUnitId` = lowest UserComId | 003 |
| TC-12 | Admin user after a non-admin logged in on same browser session | stale `ComUnitId` remains | 005 |
| TC-13 | Login from public IP, ipapi.co unreachable | login still succeeds, geo blank | 010, 012 |
| TC-14 | Successful login | `tblUserSessionTracking` +1 row; `tblLoginLog` unchanged | 009, 010 |

## 28. Reverse Engineering References
`Solution.Web/Login.aspx`, `Solution.Web/Login.aspx.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs`, `Library.BLL/Panal_BLL/PanalBLL.cs`, `Solution.Web/App_Code/UserSessionTrackingManager.cs`, `Solution.Web/MasterPages/NewMasterPage.master(.cs)`, `Solution.Web/Web.config:80-83`.

## 29. Spec vs Actual
| Area | Specification | Actual code | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Password storage | None existed | plaintext equality in SQL (BR-M01-002) | GAP-M01-001 |
| Lockout / throttling | None existed | absent | GAP-M01-003 |
| Login audit | tblLoginLog exists in code | call commented out | GAP-M01-005 |
| Forms auth | `Web.config` declares Forms auth | never issues auth cookie; Session drives auth | GAP-M01-002 |
| Menu source (generated Tier-B said `sp_GET_MainMenuRole2`) | – | `dbo.MainMenu2` scalar function | corrected here |

## 30. Known Gaps
See `specs/authentication-navigation/GAPS-M01.md`: GAP-M01-001 (plaintext passwords), 002 (Forms auth unused), 003 (no lockout), 004 (session not regenerated / long timeout), 005 (login audit dead), 006 (open redirect variants), 007 (non-deterministic ComUnitId), 008 (tracking never closed, external geo call), 009 (hard-coded DB credentials).

## 31. Change Impact
Every page reads the session keys created here (`Session["UserId"]` alone has 475 code references in `Solution.Web`). Changing the SQL column list of `PanalClsDAL.Login` (uses `*`) affects `Session[...]` population; `IsPasswordChange` also feeds `NewMasterPage.master.cs` (forced-change modal). `UserSessionTrackingManager` is only referenced from this page.
