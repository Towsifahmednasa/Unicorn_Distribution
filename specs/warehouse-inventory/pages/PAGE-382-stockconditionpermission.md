---
page_id: PAGE-382
module: M10
page: StockConditionPermission
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-382 — StockConditionPermission

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-382 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | StockConditionPermission |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_StockConditionPermission |
| aspx size | 6908 bytes |
| Code-behind LOC | 165 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **StockConditionPermission** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/StockConditionPermission.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/StockConditionPermission.aspx |
| Code-behind | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | StockConditionPermissionBll (Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs) |
| DAL class | StockConditionPermissionDal (Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs) |
| DAO/entity class | StockConditionPermissionDao (Library.DAO/SInventory_Entities/StockConditionPermissionDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| userDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnTextChanged→userDropDownList_OnTextChanged |
| stockConditionCheckBoxList | CheckBoxList |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click1 |
| clearButton | Button | Clear | no validator found |  |  |  | onclick→clearButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| userDropDownList | StockConditionPermissionDao.UserId | SInventory_UI_StockConditionPermission.submitButton_Click1 | StockConditionPermissionBll.CheckInfoAlreadyExistOrNot, StockConditionPermissionBll.LoadStockPermissionDataForSave | StockConditionPermissionDal.CheckPermissionInfoAlreadyExistOrNot, StockConditionPermissionDal.DeletePermissionInfo, Stoc | UNKNOWN | UNKNOWN | UNKNOWN |
| userDropDownList | aDataTable | SInventory_UI_StockConditionPermission.userDropDownList_OnTextChanged | StockConditionPermissionBll.LoadStockConditionByUserId | StockConditionPermissionDal.GetStockConditionByUserId | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| userDropDownList | LoadUserOnDropdownList → StockConditionPermissionBll.LoadUserInfo | inline SQL | UserName/UserId | tblUser |
| stockConditionCheckBoxList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

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
| SInventory_UI_StockConditionPermission.Page_Load | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_StockConditionPermission.Page_Load → StockConditionPermissionBll.LoadStockConditionList / StockConditionPermissionBll.LoadUserInfo → StockConditionPermissionDal.GetStockConditionList / StockConditionPermissionDal.GetUserInfoOnDropdownList | no | no |
| SInventory_UI_StockConditionPermission.LoadStockCondition | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs:26 |  | SInventory_UI_StockConditionPermission.LoadStockCondition → StockConditionPermissionBll.LoadStockConditionList → StockConditionPermissionDal.GetStockConditionList | no | no |
| SInventory_UI_StockConditionPermission.LoadUserOnDropdownList | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs:45 |  | SInventory_UI_StockConditionPermission.LoadUserOnDropdownList → StockConditionPermissionBll.LoadUserInfo → StockConditionPermissionDal.GetUserInfoOnDropdownList | no | no |
| SInventory_UI_StockConditionPermission.submitButton_Click1 | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs:87 | object sender, EventArgs e | SInventory_UI_StockConditionPermission.submitButton_Click1 → StockConditionPermissionBll.CheckInfoAlreadyExistOrNot / StockConditionPermissionBll.LoadStockPermissionDataForSave → StockConditionPermissionDal.CheckPermissionInfoAlreadyExistOrNot / StockCondition | no | no |
| SInventory_UI_StockConditionPermission.userDropDownList_OnTextChanged | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs:123 | object sender, EventArgs e | SInventory_UI_StockConditionPermission.userDropDownList_OnTextChanged → StockConditionPermissionBll.LoadStockConditionByUserId → StockConditionPermissionDal.GetStockConditionByUserId | no | no |
| SInventory_UI_StockConditionPermission.clearButton_Click | Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs:156 | object sender, EventArgs e | SInventory_UI_StockConditionPermission.clearButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| StockConditionPermissionBll.LoadStockConditionList | Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs:22 |
| StockConditionPermissionBll.LoadUserInfo | Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs:17 |
| StockConditionPermissionBll.LoadStockPermissionDataForSave | Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs:32 |
| StockConditionPermissionBll.CheckInfoAlreadyExistOrNot | Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs:47 |
| StockConditionPermissionBll.LoadStockConditionByUserId | Library.BLL/SInventory_BLL/StockConditionPermissionBll.cs:27 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| StockConditionPermissionDal.GetStockConditionList | Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs:24 | inline SQL: tblStockCondition |  |
| StockConditionPermissionDal.GetUserInfoOnDropdownList | Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs:18 | inline SQL: tblUser |  |
| StockConditionPermissionDal.SaveStockConditionPermissionInfo | Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs:39 | inline SQL: tblStockConditionPermission | @Permission, @StockConId, @UserId |
| StockConditionPermissionDal.CheckPermissionInfoAlreadyExistOrNot | Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs:50 | inline SQL: tblStockConditionPermission | @StockConId, @UserId |
| StockConditionPermissionDal.DeletePermissionInfo | Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs:60 | inline SQL: tblStockConditionPermission, tblStockConditionPermission | @StockConId, @UserId |
| StockConditionPermissionDal.GetStockConditionByUserId | Library.DAL/SInventory_DAL/StockConditionPermissionDal.cs:30 | inline SQL: tblStockConditionPermission | @UserId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblStockCondition, tblStockConditionPermission, tblUser
- Tables WRITTEN (inline SQL reachable): tblStockConditionPermission
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblStockConditionPermission | Permission | @Permission |
| tblStockConditionPermission | StockConId | @StockConId |
| tblStockConditionPermission | UserId | @UserId |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | You have to select an user !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | You have to select at least one condition !!! | UNKNOWN |

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
- Redirect targets: none
- User messages: You have to select an user !!!; You have to select at least one condition !!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: UserId. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: StockConditionPermissionBll; DAL: StockConditionPermissionDal; DAO: StockConditionPermissionDao; Tables (inline): tblStockCondition, tblStockConditionPermission, tblUser

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/StockConditionPermission.aspx`, `Solution.Web/SInventory_UI/StockConditionPermission.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/StockConditionPermission.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 7 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/StockConditionPermission.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods StockConditionPermissionDal.CheckPermissionInfoAlreadyExistOrNot, StockConditionPermissionDal.DeletePermissionInfo, StockConditionPermissionDal.GetStockConditionByUserId, StockConditionPermissionDal.GetStockConditionList, StockConditionPermissionDal.GetUserInfoOnDropdownList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
