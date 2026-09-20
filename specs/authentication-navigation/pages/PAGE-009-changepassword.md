---
page_id: PAGE-009
module: M01
page: ChangePassword
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-009 — ChangePassword

> **Evidence level:** Tier A — aspx (incl. all inline JS), code-behind, `ChangePasswordDAL` (all queries read), `NewMasterPage.master(.cs)` read.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-009 |
| Module | M01 |
| Kind | Self-service form (own password) |
| Folder | `Solution.Web/UserProfile_UI` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `UserProfile_UI_ChangePassword` |
| Size | aspx 674 lines (≈380 lines CSS), code-behind 175 |

## 2. Purpose
Let the signed-in user set a new password that meets a strength policy; it is also the destination of the forced-password-change modal shown by the master page.

## 3. Business Objective
Enforce first-login / periodic credential hygiene for all users (the modal text reads "Your account requires a password update before continuing"). CONFIRMED for the flow, INFERRED for the business motive.

## 4. Navigation
Inbound: user drop-down "Change Password" (`NewMasterPage.master:1598`, `../UserProfile_UI/ChangePassword.aspx`); forced modal button "Change Password Now" (`NewMasterPage.master:1643`); the page's own success redirect. Outbound: success popup re-opens `ChangePassword.aspx`. The master page suppresses the modal on this page only (`NewMasterPage.master.cs:103-107`).

## 5. Route
`~/UserProfile_UI/ChangePassword.aspx`.

## 6. Source Files
`ChangePassword.aspx(.cs)`, `Library.DAL/UserProfileDAL/ChangePasswordDAL.cs` (page calls the DAL directly — no BLL), `NewMasterPage.master(.cs)`.

## 7. UI Control Inventory
| Control | Type | Label | Required | Notes |
|---|---|---|---|---|
| lblshortName, lblDesignation, lblID, lblRoleName | Label | name, designation, "ID" (= LoginName), "Role" | – | filled on first load from `GetUserPassInfoDAL` |
| txt_Password | TextBox `TextMode=Password`, MaxLength 20 | New Password * | client + server | strength regex; eye toggle button switches `type` |
| txtConfirm | TextBox Password, MaxLength 20 | Confirm Password * | client + server | must equal new password |
| revPasswordStrength | RegularExpressionValidator | – | – | `^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,20}$`, "Password is not strong enough" |
| CompareValidator1 | CompareValidator | – | – | "Password is not matched" |
| lblPasswordError | Label | – | – | server-side error text |
| btn_Save | LinkButton "Update Password" | – | – | `OnClientClick="return validateChangePasswordClient() && sweetAlertConfirm_Update(this);"` |
| 5 rule tiles | divs `data-rule=length/lower/upper/number/special` | live checklist | – | turned green by `updateRules()` on keyup/change |

No old-password field exists.

## 8. Field Mapping
| UI | Chain | SQL param | Column |
|---|---|---|---|
| txt_Password (trimmed) | `btn_Save_OnClick` → `ChangePasswordDAL.UpdatePass(int UserId, string Pass)` | `@Password` | `tblUser.Password` (stored as typed) |
| Session["UserId"] | `_userId` | `@UserId` (twice) | `tblUser.UserId`, `tblUser.UpdateBy` |
| – | – | `GETDATE()` | `tblUser.UpdateDate` |
| – | – | literal `1` | `tblUser.IsPasswordChange` |

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
`btn_Save` → `btn_Save_OnClick` (`ChangePassword.aspx.cs:84`).

## 11. Frontend Flow
IIFE at end of aspx: live rule ticks, eye toggles, `validateChangePasswordClient()` (same regex; confirm non-empty and equal; also calls `Page_ClientValidate()`), re-bound after every UpdatePanel async postback. On server success: `successalert('Password updated successfully!','Success','ChangePassword.aspx')`; on failure `faildalert('Password update failed!','Failed')` (helpers from master scripts, not part of this page).

## 12. Backend Flow
`Page_Load`: `_userId = Session["UserId"]` if not null; first load and `Session["EmpInfoId"]` non-null/non-empty (`!= ""` on an object → reference comparison, effectively always true when non-null) → `GetOneRecordPassword(_userId)`. `btn_Save_OnClick`: `Validat()` → `UpdatePass(Convert.ToInt32(_userId), txt_Password.Text.Trim())` → on true: `Session["IsPasswordChange"]="True"` + success alert.
`Validat()` order: (1) empty → "Please enter new password"; else (2) fails `IsStrongPassword` → "Password is not strong enough"; else (3) equals current password (`GetUserCurrentPasswordDAL` returns `tblUser.Password`, compared with `!=` on plaintext) → "New password cannot be the same as current password"; then confirm empty or different → red border (no text). All checks are repeated server-side. `UpdatePass` returns `true` if `UserId<=0` **without updating anything** (falls out of the `if`).

## 13. API Mapping
None.

## 14. Service Mapping
None (page → DAL).

## 15. Repository Mapping
| DAL | SQL |
|---|---|
| `GetUserPassInfoDAL` | `select u.LoginName,'' Password,emp.EmpName,uRR.RoleName,uType.UserType,dgs.DesigName from tblUser u left join tblEmpGeneralInfo ... where u.UserId=` + **string concatenation** of `Session["UserId"]` |
| `GetUserCurrentPasswordDAL` | `select u.Password from tblUser u with (nolock) where u.UserId=` + concatenated id |
| `UpdatePass` | `update tblUser set Password=@Password,IsPasswordChange=1, UpdateBy=@UserId,UpdateDate= GETDATE() where UserId=@UserId` (parameterised, DB "SSIDB") |

## 16. Stored Procedure Mapping
None.

## 17. Database Mapping
`tblUser` READ (LoginName, Password, EmpInfoId, UserRoleID, UserTypeId) / UPDATE (Password, IsPasswordChange, UpdateBy, UpdateDate); `tblEmpGeneralInfo`, `tbl_UserRoleInfo`, `tblUserType`, `tblDesignation` READ.

## 18. Validation Matrix
| Field | Rule | FE | BE | Message | BR |
|---|---|---|---|---|---|
| New password | required | yes | yes | `Please enter new password` | BR-M01-018 |
| New password | 12–20 chars, ≥1 lower, ≥1 upper, ≥1 digit, ≥1 non-alphanumeric | yes (JS + validator) | yes (`Regex.IsMatch`) | `Password is not strong enough` | BR-M01-018 |
| New password | ≠ current | no | yes | `New password cannot be the same as current password` | BR-M01-019 |
| Confirm | required, equal | yes | yes | validator `Password is not matched` / red border | BR-M01-020 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M01-015 | The forced-change modal (master page) appears on every page except `ChangePassword.aspx` while `Session["IsPasswordChange"]` is `false`, `"false"` or `"0"`; the modal is a static Bootstrap dialog (no close/ESC) but contains only a link; it is UI-only. | `NewMasterPage.master.cs:97-116`, `NewMasterPage.master:1628-1668` | CONFIRMED |
| BR-M01-018 | Password policy: length 12–20, at least one lowercase, uppercase, digit and one non-alphanumeric character. | `ChangePassword.aspx.cs:171-174`; JS in `ChangePassword.aspx` | CONFIRMED |
| BR-M01-019 | New password must differ from the current stored password (plaintext comparison). No history beyond the current one. | `ChangePassword.aspx.cs:158-169` | CONFIRMED |
| BR-M01-020 | Confirmation must equal the new password. | `ChangePassword.aspx.cs:145-154` | CONFIRMED |
| BR-M01-021 | On success `tblUser.Password` is overwritten with the plain text, `IsPasswordChange=1`, `UpdateBy=UserId`, `UpdateDate=GETDATE()`, and the session flag becomes `"True"` so the modal disappears without re-login. **The current password is not asked for or verified.** | `ChangePasswordDAL.cs:63-92`, `ChangePassword.aspx.cs:88-92` | CONFIRMED |

## 20. Permission Matrix
Reads `Session["UserId"]` and `Session["EmpInfoId"]`. Runs for any logged-in user on their own account only (id from session, never from the request). Guard is the master page's first-load check only. If the session has expired and the user posts the form, `_userId` is null: `Validat()` reaches `GetUserCurrentPasswordDAL(null)` → SQL `where u.UserId=` → SQL syntax error (unhandled exception). If `IsStrongPassword` fails first the function returns before the query. INFERRED runtime behaviour, CONFIRMED code path.

## 21. Status Lifecycle
`tblUser.IsPasswordChange`: `0/NULL` → must change (modal) ; `1` → satisfied. Set to 1 here (`ChangePasswordDAL.cs:78`); who sets it back to 0 (admin reset / new user) is UNKNOWN (SP bodies `sp_Save_UserMaster_New`, `sp_Update_UserMaster_new` not in repo). Session copy `IsPasswordChange` refreshed only at login or by this page.

## 22. Error Handling
`UpdatePass` wraps in try/catch that rethrows (`throw exception;` resets stack). Page has no try/catch; SQL errors become unhandled exceptions. Failure of update (0 rows) → `faildalert('Password update failed!')`.

## 23. Audit
`tblUser.UpdateBy` (user id) and `UpdateDate` only; no password-change history table; no notification.

## 24. Integration
_Sections 24–25 are documented together in this block._
None.



## 25. Reports
See §24 block above (combined §24–25).

## 26. Dependencies
`ChangePasswordDAL`, `ClsCommonInternalDAL`, master page scripts (`successalert`, `faildalert`, `sweetAlertConfirm_Update`).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | New = `Abcdefg1234!` (12 chars) equal confirm | success; DB updated; modal gone | 018, 021 |
| TC-02 | 11 chars / 21 chars | "Password is not strong enough" | 018 |
| TC-03 | no special char | not strong enough | 018 |
| TC-04 | new equals stored current | "New password cannot be the same…" | 019 |
| TC-05 | confirm differs | validator "Password is not matched" | 020 |
| TC-06 | POST with valid data, no old password | succeeds (no re-authentication) | 021 |
| TC-07 | User with IsPasswordChange=0 opens Dashboard | modal shown, page reachable behind it via URL | 015 |
| TC-08 | Session expired then submit | unhandled SQL error | – |
| TC-09 | Strong password with leading/trailing spaces | trimmed before storing/comparing | 018 |

## 28. Reverse Engineering References
`Solution.Web/UserProfile_UI/ChangePassword.aspx(.cs)`, `Library.DAL/UserProfileDAL/ChangePasswordDAL.cs`, `Solution.Web/MasterPages/NewMasterPage.master(.cs)`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Old-password verification | none | absent | GAP-M01-014 |
| Storage | none | plaintext | GAP-M01-001 |

## 30. Known Gaps
GAP-M01-001, 013 (enforcement is UI-only), 014 (no old password, no history/expiry), 016 (string-concatenated SQL on session id, session-expiry failure mode).

## 31. Change Impact
`ChangePasswordDAL` is shared with PAGE-010. `IsPasswordChange` session flag is read by `NewMasterPage.master.cs`. Password column consumed by `PanalClsDAL.Login` and displayed by UserSetup pages (M02).
