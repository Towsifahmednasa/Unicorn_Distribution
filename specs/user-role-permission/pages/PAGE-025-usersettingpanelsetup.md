---
page_id: PAGE-025
module: M02
page: UserSettingPanelSetup
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-025 — UserSettingPanelSetup

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-025 |
| Module | M02 — User, Role & Permission Administration (INFERRED from folder + file name) |
| Page name | UserSettingPanelSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SettingPanel_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SettingPanel_UI_UserSettingPanelSetup |
| aspx size | 9568 bytes |
| Code-behind LOC | 161 |
| Inline script size | 2541 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **UserSettingPanelSetup** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: User accounts, roles, menu-permission distribution, stock-condition/DC permissions, approval-step maps, settings panel.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SettingPanel_UI/UserSettingPanelSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx |
| Code-behind | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | UserSettingPanelDal (Library.DAL/SettingPanel_DAL/UserSettingPanelDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | UserSettingPanelDao (Library.DAO/SettingPanel_DAO/UserSettingPanelDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| gv_List | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| menuAllCheckBox | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→menuAllCheckBox_CheckedChanged |
| (no id) | TemplateField | Select | no validator found |  |  |  |  |
| ckCheckBox | CheckBox |  | no validator found |  |  |  |  |
| ckAllCheckBox | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→ckAllCheckBox_OnCheckedChanged |
| (no id) | BoundField | Criteria | no validator found |  |  |  |  |
| (no id) | TemplateField | Active From Date | no validator found |  |  |  |  |
| txtFromDate | TextBox | <%#Eval("FromDate") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Active To Date | no validator found |  |  |  |  |
| txtToDate | TextBox | <%#Eval("Todate") %> | no validator found |  |  |  |  |
| btnUpdate | LinkButton |  | no validator found |  |  |  | OnClick→btnUpdate_OnClick, OnClientClick→return sweetAlertConfirm_Update(this); |
| btn_Reset | LinkButton |  | no validator found |  |  |  | OnClick→btn_Reset_OnClick |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | SettingPanel_UI_UserSettingPanelSetup.btnUpdate_OnClick | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:100 | — | UserSettingPanelDal.SaveUserPanelSettting | sp_UpDate_UserSettingPanel | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btn_Reset | LinkButton click | (postback) | SettingPanel_UI_UserSettingPanelSetup.btn_Reset_OnClick | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:138 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SettingPanel_UI_UserSettingPanelSetup.Page_Load | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:19 | object sender, EventArgs e | SettingPanel_UI_UserSettingPanelSetup.Page_Load → UserSettingPanelDal.Get_UserSettingPanel → SP: sp_GET_UserSettingPanel | no | no |
| SettingPanel_UI_UserSettingPanelSetup.loadUserSettingData | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:28 |  | SettingPanel_UI_UserSettingPanelSetup.loadUserSettingData → UserSettingPanelDal.Get_UserSettingPanel → SP: sp_GET_UserSettingPanel | no | no |
| SettingPanel_UI_UserSettingPanelSetup.gv_DocumentUpload_PreRender | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:52 | object sender, EventArgs e | SettingPanel_UI_UserSettingPanelSetup.gv_DocumentUpload_PreRender | no | no |
| SettingPanel_UI_UserSettingPanelSetup.loadGridView_RowCommand | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:62 | object sender, GridViewCommandEventArgs e | SettingPanel_UI_UserSettingPanelSetup.loadGridView_RowCommand | no | no |
| SettingPanel_UI_UserSettingPanelSetup.btnUpdate_OnClick | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:100 | object sender, EventArgs e | SettingPanel_UI_UserSettingPanelSetup.btnUpdate_OnClick → UserSettingPanelDal.SaveUserPanelSettting → SP: sp_UpDate_UserSettingPanel | no | yes |
| SettingPanel_UI_UserSettingPanelSetup.btn_Reset_OnClick | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:138 | object sender, EventArgs e | SettingPanel_UI_UserSettingPanelSetup.btn_Reset_OnClick | no | no |
| SettingPanel_UI_UserSettingPanelSetup.ckAllCheckBox_OnCheckedChanged | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs:143 | object sender, EventArgs e | SettingPanel_UI_UserSettingPanelSetup.ckAllCheckBox_OnCheckedChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| UserSettingPanelDal.Get_UserSettingPanel | Library.DAL/SettingPanel_DAL/UserSettingPanelDal.cs:50 | sp_GET_UserSettingPanel |  |
| UserSettingPanelDal.SaveUserPanelSettting | Library.DAL/SettingPanel_DAL/UserSettingPanelDal.cs:18 | sp_UpDate_UserSettingPanel | @FromDate, @Todate, @UserSettingPanelId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_UserSettingPanel | UserSettingPanelDal.Get_UserSettingPanel | body NOT in repository | read-like name (INFERRED) |
| sp_UpDate_UserSettingPanel | UserSettingPanelDal.SaveUserPanelSettting | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Minimum One | UNKNOWN |
| (handler btnUpdate_OnClick) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','UserSettingPanelSetup.aspx'); | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
- Redirect targets: UserSettingPanelSetup.aspx
- User messages: ','Success','UserSettingPanelSetup.aspx');; Please Select Minimum One
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: UserSettingPanelDal; DAO: ResultInfo, UserSettingPanelDao; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnUpdate with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnUpdate with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btn_Reset with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btn_Reset with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx`, `Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 16 controls parsed | n/a | PARTIAL | Solution.Web/SettingPanel_UI/UserSettingPanelSetup.aspx |
| Data access | This document §15–17 | 2 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M02 (User, Role & Permission Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods UserSettingPanelDal.Get_UserSettingPanel, UserSettingPanelDal.SaveUserPanelSettting affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
