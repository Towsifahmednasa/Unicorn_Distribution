---
page_id: PAGE-022
module: M02
page: UserEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-022 — UserEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-022 |
| Module | M02 — User, Role & Permission Administration (INFERRED from folder + file name) |
| Page name | UserEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | HRM_UI_TDLUser |
| aspx size | 12747 bytes |
| Code-behind LOC | 119 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **UserEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: User accounts, roles, menu-permission distribution, stock-condition/DC permissions, approval-step maps, settings panel.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/UserView.aspx`

Outbound references: `userview.aspx`

## 5. Route
`~/SInventory_UI/UserEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/UserEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/UserEntry.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | UserBLL (Library.BLL/SInventory_BLL/UserBLL.cs) |
| DAL class | UserDAL (Library.DAL/SInventory_DAL/UserDAL.cs) |
| DAO/entity class | UserCompanyUnit (Library.DAO/SInventory_Entities/UserCompanyUnit.cs) |
| DAO/entity class | UserInformation (Library.DAO/SInventory_Entities/UserInformation.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| UpdateProgress4 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait11 | Image |  | no validator found |  |  |  |  |
| viewListImageButton | ImageButton |  | no validator found |  |  |  | onclick→viewListImageButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| empNameTextBox | TextBox |  | no validator found |  |  |  |  |
| userTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| comUnitDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| userStatusNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| loginUserNameTextBox | TextBox |  | no validator found |  |  |  |  |
| userPasswordTextBox | TextBox |  | no validator found |  |  |  |  |
| contactNoTextBox | TextBox |  | no validator found |  |  |  |  |
| emailNameTextBox | TextBox |  | yes (RegularExpression) |  |  |  |  |
| submitButton0 | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| empNameTextBox | UserInformation.UserName | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| userTypeDropDownList | UserInformation.UserType | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| contactNoTextBox | UserInformation.ContactNo | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| loginUserNameTextBox | UserInformation.LoginName | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| userPasswordTextBox | UserInformation.Password | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| userStatusNameDropDownList | UserInformation.UserStatus | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| emailNameTextBox | UserInformation.Email | HRM_UI_TDLUser.submitButton_Click | UserBLL.SaveDataForUser, UserBLL.UserCodeGenerator | UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| userTypeDropDownList | LoadDropDown → UserBLL.LoadUserType | inline SQL | UserType/UserTypeId | tblUserType |
| comUnitDropDownList | LoadDropDown → UserBLL.LoadComUnit | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit |
| userStatusNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

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
| HRM_UI_TDLUser.Page_Load | Solution.Web/SInventory_UI/UserEntry.aspx.cs:14 | object sender, EventArgs e | HRM_UI_TDLUser.Page_Load → UserBLL.LoadComUnit / UserBLL.LoadUserType → UserDAL.LoadComUnit / UserDAL.LoadUserType | no | no |
| HRM_UI_TDLUser.submitButton_Click | Solution.Web/SInventory_UI/UserEntry.aspx.cs:76 | object sender, EventArgs e | HRM_UI_TDLUser.submitButton_Click → UserBLL.SaveDataForUser / UserBLL.UserCodeGenerator → UserDAL.SaveUserCompanyUnit / UserDAL.SaveUserInformation | no | no |
| HRM_UI_TDLUser.viewListImageButton_Click | Solution.Web/SInventory_UI/UserEntry.aspx.cs:109 | object sender, ImageClickEventArgs e | HRM_UI_TDLUser.viewListImageButton_Click | no | no |
| HRM_UI_TDLUser.LoadDropDown | Solution.Web/SInventory_UI/UserEntry.aspx.cs:114 |  | HRM_UI_TDLUser.LoadDropDown → UserBLL.LoadComUnit / UserBLL.LoadUserType → UserDAL.LoadComUnit / UserDAL.LoadUserType | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| UserBLL.LoadUserType | Library.BLL/SInventory_BLL/UserBLL.cs:109 |
| UserBLL.LoadComUnit | Library.BLL/SInventory_BLL/UserBLL.cs:114 |
| UserBLL.SaveDataForUser | Library.BLL/SInventory_BLL/UserBLL.cs:19 |
| UserBLL.UserCodeGenerator | Library.BLL/SInventory_BLL/UserBLL.cs:73 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| UserDAL.LoadUserType | Library.DAL/SInventory_DAL/UserDAL.cs:143 | inline SQL: tblUserType |  |
| UserDAL.LoadComUnit | Library.DAL/SInventory_DAL/UserDAL.cs:150 | inline SQL: tblCompanyUnit |  |
| UserDAL.SaveUserInformation | Library.DAL/SInventory_DAL/UserDAL.cs:17 | inline SQL: tblUser | @ContactNo, @Email, @LoginName, @Password, @UserCode, @UserId, @UserName, @UserStatus, @UserType |
| UserDAL.SaveUserCompanyUnit | Library.DAL/SInventory_DAL/UserDAL.cs:43 | inline SQL: tblUserCompanyUnit | @CompanyUnitId, @UserComId, @UserId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserType
- Tables WRITTEN (inline SQL reachable): tblUser, tblUserCompanyUnit
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblUser | ContactNo | @ContactNo |
| tblUser | Email | @Email |
| tblUser | LoginName | @LoginName |
| tblUser | Password | @Password |
| tblUser | UserCode | @UserCode |
| tblUser | UserId | @UserId |
| tblUser | UserName | @UserName |
| tblUser | UserStatus | @UserStatus |
| tblUser | UserType | @UserType |
| tblUserCompanyUnit | CompanyUnitId | @CompanyUnitId |
| tblUserCompanyUnit | UserComId | @UserComId |
| tblUserCompanyUnit | UserId | @UserId |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| emailNameTextBox | RegularExpressionValidator (\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*) | yes (validator) | UNKNOWN | UNKNOWN | Please enter a valid email address !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Loging Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Password!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input User Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input User Status!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input User Type!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 7 methods.
- Redirect targets: UserView.aspx
- User messages: Please Input Loging Name!!; Please Input Password!!; Please Input User Name!!; Please Input User Status!!; Please Input User Type!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: UserId. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: UserBLL; DAL: UserDAL; DAO: UserCompanyUnit, UserInformation; Tables (inline): tblCompanyUnit, tblUser, tblUserCompanyUnit, tblUserType

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/UserEntry.aspx`, `Solution.Web/SInventory_UI/UserEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/UserEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 20 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/UserEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M02 (User, Role & Permission Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods UserDAL.LoadComUnit, UserDAL.LoadUserType, UserDAL.SaveUserCompanyUnit, UserDAL.SaveUserInformation affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
