---
page_id: PAGE-129
module: M04
page: ConditionalPurposeSetting
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-129 — ConditionalPurposeSetting

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-129 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | ConditionalPurposeSetting |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ConditionalPurposeSetting |
| aspx size | 6147 bytes |
| Code-behind LOC | 79 |
| Inline script size | 984 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ConditionalPurposeSetting** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/ConditionalPurposeSetting.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx |
| Code-behind | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | ConditionalPurposeDal (Library.DAL/SInventory_DAL/ConditionalPurposeDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| purposeTextBox | TextBox |  | no validator found |  |  |  |  |
| conditionDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_Click1, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| conditionDropDownList | LoadDropdownList → ConditionalPurposeDal.LoadCondition | inline SQL | StockCondition/StockConId | tblStockCondition |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_ConditionalPurposeSetting.submitButton_Click1 | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:28 | — | ConditionalPurposeDal.SaveSettings | — | writes tblPurpose |
| btnReset | LinkButton click | (postback) | SInventory_UI_ConditionalPurposeSetting.btnReset_Click | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:76 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_ConditionalPurposeSetting.Page_Load | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_ConditionalPurposeSetting.Page_Load → ConditionalPurposeDal.LoadCondition | no | no |
| SInventory_UI_ConditionalPurposeSetting.LoadDropdownList | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:23 |  | SInventory_UI_ConditionalPurposeSetting.LoadDropdownList → ConditionalPurposeDal.LoadCondition | no | no |
| SInventory_UI_ConditionalPurposeSetting.submitButton_Click1 | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:28 | object sender, EventArgs e | SInventory_UI_ConditionalPurposeSetting.submitButton_Click1 → ConditionalPurposeDal.SaveSettings | no | no |
| SInventory_UI_ConditionalPurposeSetting.viewLinkButton_Click | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:66 | object sender, EventArgs e | SInventory_UI_ConditionalPurposeSetting.viewLinkButton_Click | no | no |
| SInventory_UI_ConditionalPurposeSetting.btnSave_Click | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:71 | object sender, EventArgs e | SInventory_UI_ConditionalPurposeSetting.btnSave_Click | no | no |
| SInventory_UI_ConditionalPurposeSetting.btnReset_Click | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs:76 | object sender, EventArgs e | SInventory_UI_ConditionalPurposeSetting.btnReset_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ConditionalPurposeDal.LoadCondition | Library.DAL/SInventory_DAL/ConditionalPurposeDal.cs:19 | inline SQL: tblStockCondition |  |
| ConditionalPurposeDal.SaveSettings | Library.DAL/SInventory_DAL/ConditionalPurposeDal.cs:26 | inline SQL: tblPurpose | @Purpose, @StockConditionId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblStockCondition
- Tables WRITTEN (inline SQL reachable): tblPurpose
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblPurpose | Purpose | @Purpose |
| tblPurpose | StockConditionId | @StockConditionId |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select condition !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Pourpose is required !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 8 methods.
- Redirect targets: ConditionalPurposeSetting.aspx
- User messages: Please select condition !!; Pourpose is required !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: ConditionalPurposeDal; DAO: —; Tables (inline): tblPurpose, tblStockCondition

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tblPurpose |
| TC-05 | Click btnReset with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnReset with valid data | Handler runs; rows written to tblPurpose |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx`, `Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 7 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ConditionalPurposeSetting.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ConditionalPurposeDal.LoadCondition, ConditionalPurposeDal.SaveSettings affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
