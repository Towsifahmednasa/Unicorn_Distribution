# WF-M01-03 — Forced password change and self-service password change

**Trigger:** a user with `tblUser.IsPasswordChange` = 0/NULL logs in (modal), or any user opens "Change Password". Actors: end user; master page.

**Preconditions:** logged-in session (`Session["UserId"]`).

## Steps
| # | Layer | Detail | Evidence |
|---|---|---|---|
| 1 | Login | `IsPasswordChange` from SQL `isnull(...,0)` copied to session as `"True"/"False"` (or `"True"` if column missing) | `Login.aspx.cs:55-62` |
| 2 | Master | `ShouldShowForcePasswordChangeModal()`: false when no session/flag or current file is `ChangePassword.aspx`; true when flag is bool false or text `"0"`/`"false"` (BR-M01-015) | `NewMasterPage.master.cs:97-116` |
| 3 | Master markup | Bootstrap static modal `forcePasswordChangeModal` (`backdrop:'static'`, `keyboard:false`, `hide.bs.modal` prevented) with a single link to `../UserProfile_UI/ChangePassword.aspx` | `NewMasterPage.master:1628-1668` |
| 4 | ChangePassword | user types new + confirm; client regex/rules; server `Validat()` (BR-M01-018/019/020) | `ChangePassword.aspx.cs:103-174` |
| 5 | DAL | `UPDATE tblUser SET Password=@Password, IsPasswordChange=1, UpdateBy=@UserId, UpdateDate=GETDATE() WHERE UserId=@UserId` | `ChangePasswordDAL.cs:63-92` |
| 6 | Page | `Session["IsPasswordChange"]="True"`; success alert; page reloads; modal no longer rendered (BR-M01-021) | `ChangePassword.aspx.cs:88-92` |

## Data
`tblUser.Password, IsPasswordChange, UpdateBy, UpdateDate`; session `IsPasswordChange`.

## Branches / exceptions
- **The modal is cosmetic**: it only overlays the page; other URLs, page methods and postbacks are not blocked server-side (no redirect/`Response.End`). A user can type another URL (or use a WebMethod endpoint) while flagged.
- **Alternate weaker path:** `UserProfile.aspx` (PAGE-010) also sets `IsPasswordChange=1` in DB with any non-empty password and no policy; it does not clear the session flag, so the modal persists until next login.
- Expired session at submit → SQL error (PAGE-009) or false success (PAGE-010).
- Who resets `IsPasswordChange` to 0 when an admin creates/resets a user is UNKNOWN (SP bodies `sp_Save_UserMaster_New`, `sp_Update_UserMaster_new` absent). Admin-set passwords are visible in plain text to admins (M02 PAGE-015).

## Postconditions
DB password replaced; flag =1; no history, no notification, other sessions of the same user unaffected.

## Related BR: BR-M01-015, 018, 019, 020, 021, 022. **Pages:** PAGE-009, PAGE-010, master. **Gaps:** GAP-M01-001, 013, 014, 015, 016.

## Tests
PAGE-009 §27 TC-01…TC-09, PAGE-010 §27 TC-01…TC-06.
