# WF-M01-05 — Logout and session end

**Trigger:** user clicks "Logout" in the header drop-down (`NewMasterPage.master:1607`, `LinkButton logOut`) or the session times out.

## Steps (logout)
| # | Layer | Detail | Evidence |
|---|---|---|---|
| 1 | Master | `logOut_Click`: `Session["UserId"]=""`, `LoginName=""`, `UserType=""`, `ComUnitId=""`; `Response.Redirect("../Login.aspx")` (BR-M01-016) | `NewMasterPage.master.cs:88-95` |
| 2 | – | **No** `Session.Abandon()`, no `Session.Clear()`, no clearing of `UserRoleID, RoleTypeId, RoleTypeName, EmpInfoId, EmpName, EmpMasterCode, DesigName, DICCompanyUnitId, DICUnitId, CentralWareHouse, IsPasswordChange` | same |
| 3 | – | **No** call to `UserSessionTrackingManager.MarkCurrentSessionLoggedOut` (defined, never called) → `tblUserSessionTracking.IsActiveSession` stays 1, `LogoutDateTime` stays NULL | grep |
| 4 | – | `FormsAuthentication.SignOut` not used (no auth cookie exists) | grep |
| 5 | Login | Next login with the same browser session: `CloseExistingActiveSession(SessionId)` closes the old tracking row (only then) | `UserSessionTrackingManager.cs:39` |

A second, unused handler `ImageButton1_Click` (`:78-86`) sets admin keys then `Session.Abandon()`; it is not wired in `NewMasterPage.master` (no `ImageButton1` control).

## Why this matters
`NewMasterPage` guard is `Session["UserId"] != null`; after logout it is `""`, i.e. non-null, and the other identity keys still exist, so **the master page will render for the "logged-out" user if they navigate back/forward to a URL** (menu is built from `UserId=""`, header text from surviving `EmpName`). Individual pages that call `Convert.ToInt32(Session["UserId"])` will throw; pages that only use `Session["UserRoleID"]`/`ComUnitId`-less logic keep working. (INFERRED runtime effect from CONFIRMED code.)

## Session timeout
`sessionState mode=InProc timeout=24000` minutes; `forms timeout=2880` (unused). App-pool recycle loses all sessions (InProc). No `Session_End` handler in the root `Global.asax`.

## Related BR: BR-M01-016, 010–012. **Gaps:** GAP-M01-004, 008, 010.

## Tests
| ID | Scenario | Expected |
|---|---|---|
| T1 | Click Logout, then press Back to a menu page | page may render (guard passes) |
| T2 | After logout, inspect `tblUserSessionTracking` | row still `IsActiveSession=1` |
| T3 | Log in again in the same browser | old row closed by SessionId match |
