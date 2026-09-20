---
page_id: PAGE-123
module: M04
page: PromoGroup
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-123 — PromoGroup

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-123 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | PromoGroup |
| Kind | Page (INFERRED from name) |
| Web folder | PromoAlloc |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | PromoAlloc_PromoGroup |
| aspx size | 10071 bytes |
| Code-behind LOC | 67 |
| Inline script size | 5587 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **PromoGroup** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `PromoAlloc/PromoGroupList.aspx`

Outbound references: `promogrouplist.aspx`

## 5. Route
`~/PromoAlloc/PromoGroup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/PromoAlloc/PromoGroup.aspx |
| Code-behind | Solution.Web/PromoAlloc/PromoGroup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | ../assets/year-select.js |
| DAL class | PromoGroupDAL (Library.DAL/PromoAllocDAL/PromoGroupDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| groupname | input |  Promo Group Name | no validator found |  |  |  |  |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | TextBox |  | no validator found |  |  |  |  |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: ConfirmationClick, FinalSave, GetData, RemoveValidationTooltip, Save, Validation, ValidationTooltip, getUrlVars
- jQuery/DOM bindings: #customSwitch1:change
- AJAX endpoints (`url:`): PromoGroup.aspx/LoadPromoGroupById, PromoGroup.aspx/SaveGroup
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| PromoAlloc_PromoGroup.Page_Load | Solution.Web/PromoAlloc/PromoGroup.aspx.cs:21 | object sender, EventArgs e | PromoAlloc_PromoGroup.Page_Load | no | no |
| PromoAlloc_PromoGroup.SaveGroup | Solution.Web/PromoAlloc/PromoGroup.aspx.cs:27 | GroupDAO aTargetDao | PromoAlloc_PromoGroup.SaveGroup → PromoGroupDAL.SavePromoGroup / PromoGroupDAL.UpdatePromoGroup | no | no |
| PromoAlloc_PromoGroup.LoadPromoGroupById | Solution.Web/PromoAlloc/PromoGroup.aspx.cs:53 | string id | PromoAlloc_PromoGroup.LoadPromoGroupById → PromoGroupDAL.LoadPromoGroupById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| PromoAlloc_PromoGroup.SaveGroup | Solution.Web/PromoAlloc/PromoGroup.aspx.cs:27 | GroupDAO aTargetDao | ~/PromoAlloc/PromoGroup.aspx/SaveGroup | via BLL |
| PromoAlloc_PromoGroup.LoadPromoGroupById | Solution.Web/PromoAlloc/PromoGroup.aspx.cs:53 | string id | ~/PromoAlloc/PromoGroup.aspx/LoadPromoGroupById | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| PromoGroupDAL.UpdatePromoGroup | Library.DAL/PromoAllocDAL/PromoGroupDAL.cs:40 | inline SQL: tblPromoGroup |  |
| PromoGroupDAL.SavePromoGroup | Library.DAL/PromoAllocDAL/PromoGroupDAL.cs:17 | inline SQL: tblPromoGroup, tblPromoGroup |  |
| PromoGroupDAL.LoadPromoGroupById | Library.DAL/PromoAllocDAL/PromoGroupDAL.cs:60 | inline SQL: tblPromoGroup |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblPromoGroup
- Tables WRITTEN (inline SQL reachable): tblPromoGroup
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 3 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: PromoGroupDAL; DAO: ResultInfo; Tables (inline): tblPromoGroup

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/PromoAlloc/PromoGroup.aspx`, `Solution.Web/PromoAlloc/PromoGroup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/PromoAlloc/PromoGroup.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 8 controls parsed | n/a | PARTIAL | Solution.Web/PromoAlloc/PromoGroup.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods PromoGroupDAL.LoadPromoGroupById, PromoGroupDAL.SavePromoGroup, PromoGroupDAL.UpdatePromoGroup affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
