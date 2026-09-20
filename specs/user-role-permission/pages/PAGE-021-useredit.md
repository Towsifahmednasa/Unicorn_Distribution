---
page_id: PAGE-021
module: M02
page: UserEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-021 — UserEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-021 |
| Module | M02 — User, Role & Permission Administration (INFERRED from folder + file name) |
| Page name | UserEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_UserEdit |
| aspx size | 11440 bytes |
| Code-behind LOC | 113 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **UserEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: User accounts, roles, menu-permission distribution, stock-condition/DC permissions, approval-step maps, settings panel.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/UserView.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/UserEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/UserEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/UserEdit.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | UserBLL (Library.BLL/SInventory_BLL/UserBLL.cs) |
| DAL class | UserDAL (Library.DAL/SInventory_DAL/UserDAL.cs) |
| DAO/entity class | UserInformation (Library.DAO/SInventory_Entities/UserInformation.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| userIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| userNameTextBox | TextBox |  | no validator found |  |  |  |  |
| userTypeTextBox | TextBox |  | no validator found |  |  |  |  |
| loginNameTextBox | TextBox |  | no validator found |  |  |  |  |
| passwordTextBox | TextBox |  | no validator found |  |  |  |  |
| userStatusTextBox | TextBox |  | no validator found |  |  |  |  |
| emailTextBox | TextBox |  | no validator found |  |  |  |  |
| contactTextBox | TextBox |  | no validator found |  |  |  |  |
| closeButton2 | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click1 |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| userIdHiddenField | UserInformation.UserId | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | @UserId | UNKNOWN (SP body not in repo) | UNKNOWN |
| userNameTextBox | UserInformation.UserName | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| userTypeTextBox | UserInformation.UserType | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| loginNameTextBox | UserInformation.LoginName | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| passwordTextBox | UserInformation.Password | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| userStatusTextBox | UserInformation.UserStatus | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| emailTextBox | UserInformation.Email | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| contactTextBox | UserInformation.ContactNo | SInventory_UI_UserEdit.updateButton_Click1 | UserBLL.UpdateDataForUser | UserDAL.UpdateUserInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_UserEdit.Page_Load | Solution.Web/SInventory_UI/UserEdit.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_UserEdit.Page_Load → UserBLL.UserEditLoad → UserDAL.UserInformationEditLoad | no | no |
| SInventory_UI_UserEdit.closeButton_Click | Solution.Web/SInventory_UI/UserEdit.aspx.cs:24 | object sender, EventArgs e | SInventory_UI_UserEdit.closeButton_Click | no | no |
| SInventory_UI_UserEdit.updateButton_Click1 | Solution.Web/SInventory_UI/UserEdit.aspx.cs:63 | object sender, EventArgs e | SInventory_UI_UserEdit.updateButton_Click1 → UserBLL.UpdateDataForUser → UserDAL.UpdateUserInfo | no | no |
| SInventory_UI_UserEdit.UserLoad | Solution.Web/SInventory_UI/UserEdit.aspx.cs:100 | string userId | SInventory_UI_UserEdit.UserLoad → UserBLL.UserEditLoad → UserDAL.UserInformationEditLoad | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| UserBLL.UserEditLoad | Library.BLL/SInventory_BLL/UserBLL.cs:104 |
| UserBLL.UpdateDataForUser | Library.BLL/SInventory_BLL/UserBLL.cs:90 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| UserDAL.UserInformationEditLoad | Library.DAL/SInventory_DAL/UserDAL.cs:89 | inline SQL: tblUser | @UserId |
| UserDAL.UpdateUserInfo | Library.DAL/SInventory_DAL/UserDAL.cs:116 | inline SQL: tblUser | @ContactNo, @Email, @LoginName, @Password, @UserId, @UserName, @UserStatus, @UserType |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblUser
- Tables WRITTEN (inline SQL reachable): tblUser
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblUser | ContactNo | @ContactNo |
| tblUser | Email | @Email |
| tblUser | LoginName | @LoginName |
| tblUser | Password | @Password |
| tblUser | UserName | @UserName |
| tblUser | UserStatus | @UserStatus |
| tblUser | UserType | @UserType |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Contact Number!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Loging Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Password!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input User Name!! | UNKNOWN |
| (handler updateButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Data Update Successfully!!! Please Reload | UNKNOWN |
| (handler updateButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 6 methods.
- Redirect targets: none
- User messages: Data Update Successfully!!! Please Reload; Please Input Contact Number!!; Please Input Loging Name!!; Please Input Password!!; Please Input User Name!!; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: UserBLL; DAL: UserDAL; DAO: UserInformation; Tables (inline): tblUser

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/UserEdit.aspx`, `Solution.Web/SInventory_UI/UserEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/UserEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 14 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/UserEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M02 (User, Role & Permission Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods UserDAL.UpdateUserInfo, UserDAL.UserInformationEditLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
