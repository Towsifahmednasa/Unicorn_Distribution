---
page_id: PAGE-119
module: M04
page: QuotedPriceView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-119 — QuotedPriceView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-119 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | QuotedPriceView |
| Kind | List / View (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_QuotedPriceView |
| aspx size | 5470 bytes |
| Code-behind LOC | 59 |
| Inline script size | 1400 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **QuotedPriceView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/QuotedPriceSetup.aspx`

Outbound references: `quotedpricesetup.aspx`

## 5. Route
`~/MasterSetup_UI/QuotedPriceView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | QuotedPriceSetupDAL (Library.DAL/MasterSetup_DAL/QuotedPriceSetupDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→EmpCetegoryAddImageButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Description | no validator found |  |  |  |  |
| (no id) | BoundField | Policy | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Market Wise | no validator found |  |  |  |  |
| (no id) | BoundField | Active From Date | no validator found |  |  |  |  |
| (no id) | BoundField | Active To Date | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | MasterSetup_UI_QuotedPriceView.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs:36 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_QuotedPriceView.Page_Load | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs:16 | object sender, EventArgs e | MasterSetup_UI_QuotedPriceView.Page_Load → QuotedPriceSetupDAL.GetQuotedPriceList → SP: sp_Get_QuotedPriceMaster | no | no |
| MasterSetup_UI_QuotedPriceView.gv_DocumentUpload_PreRender | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs:25 | object sender, EventArgs e | MasterSetup_UI_QuotedPriceView.gv_DocumentUpload_PreRender | no | no |
| MasterSetup_UI_QuotedPriceView.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs:36 | object sender, EventArgs e | MasterSetup_UI_QuotedPriceView.EmpCetegoryAddImageButton_Click | no | no |
| MasterSetup_UI_QuotedPriceView.LoadData | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs:42 |  | MasterSetup_UI_QuotedPriceView.LoadData → QuotedPriceSetupDAL.GetQuotedPriceList → SP: sp_Get_QuotedPriceMaster | no | no |
| MasterSetup_UI_QuotedPriceView.loadGridView_RowCommand | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs:49 | object sender, GridViewCommandEventArgs e | MasterSetup_UI_QuotedPriceView.loadGridView_RowCommand | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| QuotedPriceSetupDAL.GetQuotedPriceList | Library.DAL/MasterSetup_DAL/QuotedPriceSetupDAL.cs:114 | sp_Get_QuotedPriceMaster |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_QuotedPriceMaster | QuotedPriceSetupDAL.GetQuotedPriceList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 5 methods.
- Redirect targets: QuotedPriceSetup.aspx, QuotedPriceSetup.aspx?MID=
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: QuotedPriceSetupDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/QuotedPriceView.aspx`, `Solution.Web/MasterSetup_UI/QuotedPriceView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 13 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/QuotedPriceView.aspx |
| Data access | This document §15–17 | 1 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods QuotedPriceSetupDAL.GetQuotedPriceList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
