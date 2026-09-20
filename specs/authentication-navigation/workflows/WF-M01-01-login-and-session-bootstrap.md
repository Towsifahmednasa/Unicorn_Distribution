# WF-M01-01 — Login and session bootstrap

**Trigger & actors:** any ERP user (admin, sales hierarchy, DC/depot, DA, mobile-app users) opens `Login.aspx` and clicks "Sign in". System actors: `PanalClsDAL`, `UserSessionTrackingManager`, external geo-IP service.

**Preconditions:** a `tblUser` row exists with `UserStatus='active'`, matching `LoginName` and `Password` (plaintext); DB reachable.

## Steps
| # | Layer | Detail | Evidence |
|---|---|---|---|
| 1 | Page | `loginButton_Click` trims and checks user name then password non-empty (BR-M01-001) | `Login.aspx.cs:18-29` |
| 2 | BLL | `PanalBLL.Login` pass-through | `PanalBLL.cs` |
| 3 | DAL | `PanalClsDAL.Login`: parameterised inline SELECT TOP 1 over `tblUser` + company units + employee/DA + designation + role type; `WHERE LoginName=@LoginName AND Password=@Password AND UserStatus='active'` (BR-M01-002/003) | `PanalClsDAL.cs:15-52` |
| 4 | Page | 0 rows → red/yellow message `User name or password incourect !!!!`; end | `Login.aspx.cs:120-124` |
| 5 | Page | set session keys (BR-M01-004/005/006) | `:34-69` |
| 6 | App_Code | `StartTracking`: read UserId, resolve client IP (header priority list), geo-IP via `https://ipapi.co/{ip}/json/` for public IPs (3 s), parse browser/OS/device from User-Agent, close active rows for the same SessionId, `INSERT INTO tblUserSessionTracking … SELECT SCOPE_IDENTITY()`, store `__UserSessionTrackingId` and `__UserSessionTrackingLastActivity` in session; all exceptions swallowed (BR-M01-010/011/012) | `UserSessionTrackingManager.cs:18-64` |
| 7 | Page | redirect: `IsMainDashboard` → `Dashboard_UI/AdminDashboard.aspx`; else `aspxerrorpath` (not starting with `http`) ; else `Dashboard_UI/DashboardOne.aspx` (BR-M01-007/008) | `:93-118` |
| 8 | Master | target page loads `NewMasterPage`: guard, header text, menu, forced-password modal (see WF-M01-02, WF-M01-03) | `NewMasterPage.master.cs` |

## Data created/changed
- Session (InProc): keys listed in BR-M01-004 (+ tracking keys).
- `tblUserSessionTracking` +1 row (`IsActiveSession=1`, `LogoutDateTime NULL`).
- `tblLoginLog`: **not written** (call commented out, `Login.aspx.cs:46`).
- No change to `tblUser`.

## Branches / exceptions
- Empty field → message, no DB call. Wrong credentials / inactive user → generic message (no distinction), no counter/lockout.
- Geo-IP failure/timeout, tracking insert failure → ignored; login continues.
- DB failure in `Login` → unhandled exception (yellow screen or IIS error).
- Missing `IsPasswordChange` column → session flag `"True"` (no forced change).

## Postconditions
Authenticated session; tracking row open forever (never closed by logout/timeout, see WF-M01-05).

## Related BR
BR-M01-001 … 012. **Related pages:** PAGE-001. **Related gaps:** GAP-M01-001, 002, 003, 004, 005, 006, 007, 008.

## Tests
See PAGE-001 §27 (TC-01…TC-14).
