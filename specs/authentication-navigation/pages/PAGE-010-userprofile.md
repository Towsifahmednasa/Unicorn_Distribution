---
page_id: PAGE-010
module: M01
page: UserProfile
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-010 — UserProfile

> **Evidence level:** Tier A — aspx, code-behind and `ChangePasswordDAL` read. This page is an older, weaker copy of PAGE-009.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-010 |
| Module | M01 |
| Kind | Self-service profile / password page (legacy duplicate of ChangePassword) |
| Folder | `Solution.Web/UserProfile_UI` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `UserProfile_UI_UserProfile` |
| Size | aspx 399 lines, code-behind 135 |

## 2. Purpose
Shows the user's ID (login name), name, role and designation and lets them set a new password.

## 3. Business Objective
Self-service credential change. INFERRED; same objective as PAGE-009, which superseded it (the aspx `ChangePassword` carries the policy UI).

## 4. Navigation
No inbound link found in `Solution.Web` code or master (the master's drop-down links to `ChangePassword.aspx`). Success popup redirects to `ChangePassword.aspx` (`UserProfile.aspx.cs:88`). Reachable by direct URL.

## 5. Route
`~/UserProfile_UI/UserProfile.aspx`.

## 6. Source Files
`UserProfile.aspx(.cs)`, `Library.DAL/UserProfileDAL/ChangePasswordDAL.cs`.

## 7. UI Control Inventory
| Control | Type | Notes |
|---|---|---|
| lblID / lblshortName / lblRoleName / lblDesignation | Label | filled on first load by `GetUserPassInfoDAL(Session["UserId"])` (LoginName, EmpName, RoleName, DesigName) |
| txt_Password | TextBox **without `TextMode=Password`** (`UserProfile.aspx:351`) | new password shown in clear text while typing; no MaxLength, no policy validator |
| txtConfirm | TextBox **without `TextMode=Password`** (`:366`) | + `CompareValidator1` "Password is not Matched" |
| btn_Save | LinkButton "Update Password" | `OnClientClick="return sweetAlertConfirm_Update(this);"` |

## 8. Field Mapping
As PAGE-009: `txt_Password` → `ChangePasswordDAL.UpdatePass(UserId, pass)` → `tblUser.Password`, `IsPasswordChange=1`, `UpdateBy`, `UpdateDate`.

## 9. Dropdown Mapping
_Sections 9–10 are documented together in this block._
No dropdowns. `btn_Save_OnClick` → `Validat()` → `UpdatePass`.



## 10. Button Action Mapping
See §9 block above (combined §9–10).

## 11. Frontend Flow
Only the SweetAlert confirm; result via `successalert('Operation successful!','Success','ChangePassword.aspx')` or `faildalert('Already Exist!','Faild')` (wrong wording copied from a master-data page).

## 12. Backend Flow
`Page_Load` first load always calls `GetOneRecordPassword(_userId)` (no `EmpInfoId` test). `Validat()`: password non-empty; confirm non-empty; equal (fields flagged with `is-invalid` CSS, no text). Then `UpdatePass(Convert.ToInt32(_userId), trimmed)`. **Does not** update `Session["IsPasswordChange"]`.

## 13. API Mapping
_Sections 13–16 are documented together in this block._
No API, no BLL, DAL = `ChangePasswordDAL` (same queries as PAGE-009: `GetUserPassInfoDAL` concatenates the session id into SQL; `UpdatePass` parameterised). No SP.



## 14. Service Mapping
See §13 block above (combined §13–16).

## 15. Repository Mapping
See §13 block above (combined §13–16).

## 16. Stored Procedure Mapping
See §13 block above (combined §13–16).

## 17. Database Mapping
Same as PAGE-009.

## 18. Validation Matrix
| Field | Rule | FE | BE | Message | BR |
|---|---|---|---|---|---|
| New password | non-empty only | validator none | yes | tooltip `please fill out this field` | BR-M01-022 |
| Confirm | non-empty, equal | CompareValidator | yes | `Password is not Matched` | BR-M01-022 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M01-022 | On this page any non-empty password is accepted as long as the confirmation matches: **no strength policy, no "different from current" check, no old-password check**; the DB flag `IsPasswordChange` becomes 1 but the session flag stays unchanged (modal keeps showing until re-login). | `UserProfile.aspx.cs:83-134` | CONFIRMED |

## 20. Permission Matrix
Own account only (`Session["UserId"]`). Master first-load guard only. The forced-change modal is *not* suppressed on this page (only `ChangePassword.aspx` is), yet the page can be used to satisfy the DB flag with a trivial password — i.e. it bypasses BR-M01-018/019.

## 21. Status Lifecycle
Sets `tblUser.IsPasswordChange=1` (see PAGE-009 §21).

## 22. Error Handling
No try/catch (DAL rethrows). If session expired, `Convert.ToInt32(null)` = 0 → `UpdatePass` returns `true` without updating and the UI reports success ("Operation successful!") — false success (CONFIRMED by code reading of `UpdatePass` `if (UserId > 0)` fall-through).

## 23. Audit
`UpdateBy`, `UpdateDate` only.

## 24. Integration
_Sections 24–26 are documented together in this block._
None / None / `ChangePasswordDAL`.



## 25. Reports
See §24 block above (combined §24–26).

## 26. Dependencies
See §24 block above (combined §24–26).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | New = `a`, confirm `a` | accepted, stored `a` | 022 |
| TC-02 | New = current password | accepted | 022 |
| TC-03 | Confirm differs | validator error | 022 |
| TC-04 | Forced user updates here | DB flag 1, modal still shown until next login | 022 |
| TC-05 | Expired session then submit | success message but no DB change | – |
| TC-06 | Typing password | characters visible (not masked) | – |

## 28. Reverse Engineering References
`Solution.Web/UserProfile_UI/UserProfile.aspx(.cs)`, `Library.DAL/UserProfileDAL/ChangePasswordDAL.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Password policy | (PAGE-009 policy) | not applied here | GAP-M01-014 |

## 30. Known Gaps
GAP-M01-013, 014, 015 (unmasked password fields), 016 (false success on expired session), 022-note duplicate/legacy page.

## 31. Change Impact
Shares `ChangePasswordDAL` with PAGE-009. Removing or hiding this page has no inbound dependants found.
