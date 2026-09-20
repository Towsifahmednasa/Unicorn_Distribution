---
page_id: PAGE-160
module: M04
page: ShippingCartonSizeEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-160 — ShippingCartonSizeEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-160 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | ShippingCartonSizeEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ShippingCartonSizeEntry |
| aspx size | 6715 bytes |
| Code-behind LOC | 119 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ShippingCartonSizeEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ProductEntry.aspx`, `SInventory_UI/ShippingCartonSizeView.aspx`

Outbound references: `shippingcartonsizeview.aspx`

## 5. Route
`~/SInventory_UI/ShippingCartonSizeEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ShippingCartonSizeBLL (Solution.Web/SInventory_UI/ShippingCartonSizeBLL.cs) |
| DAL class | ShippingCartonSizeDAL (Solution.Web/SInventory_UI/ShippingCartonSizeDAL.cs) |
| DAO/entity class | ShippingCartonSize (Library.DAO/SInventory_Entities/ShippingCartonSize.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| detailsViewButton | LinkButton |  | no validator found |  |  |  | OnClick→detailsViewButton_Click |
| shippingcartoonIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait7 | Image |  | no validator found |  |  |  |  |
| caseQtyTextBox | TextBox |  | no validator found |  |  |  |  |
| pcsPerCaseTextBox | TextBox |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→submitButton_Click |
| updateButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→submitButton_Click |
| ResetBtn | LinkButton |  | no validator found |  |  |  | OnClick→ResetBtn_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| shippingcartoonIdHiddenField | ShippingCartonSize.CaseId | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | ShippingCartonSizeBLL.SaveShippingCartonSize, ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | @CaseId | UNKNOWN (SP body not in repo) | UNKNOWN |
| shippingcartoonIdHiddenField | ShippingCartonSize.CaseId | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | ShippingCartonSizeBLL.SaveShippingCartonSize, ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | ShippingCartonSizeDAL.SaveShippingCartonSize | @CaseId | UNKNOWN (SP body not in repo) | UNKNOWN |
| caseQtyTextBox | ShippingCartonSize.CaseQty | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | ShippingCartonSizeBLL.SaveShippingCartonSize, ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | ShippingCartonSizeDAL.SaveShippingCartonSize, ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| pcsPerCaseTextBox | ShippingCartonSize.PcsPerCase | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | ShippingCartonSizeBLL.SaveShippingCartonSize, ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | ShippingCartonSizeDAL.SaveShippingCartonSize, ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| detailsViewButton | LinkButton click | (postback) | SInventory_UI_ShippingCartonSizeEntry.detailsViewButton_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:110 | — | — | — | read-only / none detected |
| submitButton | LinkButton click | (postback) | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:56 | ShippingCartonSizeBLL.SaveShippingCartonSize, ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | ShippingCartonSizeDAL.SaveShippingCartonSize, ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | — | writes tblProductCase |
| updateButton | LinkButton click | (postback) | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:56 | ShippingCartonSizeBLL.SaveShippingCartonSize, ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | ShippingCartonSizeDAL.SaveShippingCartonSize, ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | — | writes tblProductCase |
| ResetBtn | LinkButton click | (postback) | SInventory_UI_ShippingCartonSizeEntry.ResetBtn_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:115 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_ShippingCartonSizeEntry.Page_Load | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_ShippingCartonSizeEntry.Page_Load → ShippingCartonSizeBLL.ShippingCartonSizeEditLoad → ShippingCartonSizeDAL.ShippingCartonSizeEditLoad | no | no |
| SInventory_UI_ShippingCartonSizeEntry.ShippingCartonSizeLoad | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:32 | string ID | SInventory_UI_ShippingCartonSizeEntry.ShippingCartonSizeLoad → ShippingCartonSizeBLL.ShippingCartonSizeEditLoad → ShippingCartonSizeDAL.ShippingCartonSizeEditLoad | no | no |
| SInventory_UI_ShippingCartonSizeEntry.submitButton_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:56 | object sender, EventArgs e | SInventory_UI_ShippingCartonSizeEntry.submitButton_Click → ShippingCartonSizeBLL.SaveShippingCartonSize / ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo → ShippingCartonSizeDAL.SaveShippingCartonSize / ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | no | no |
| SInventory_UI_ShippingCartonSizeEntry.detailsViewButton_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:110 | object sender, EventArgs e | SInventory_UI_ShippingCartonSizeEntry.detailsViewButton_Click | no | no |
| SInventory_UI_ShippingCartonSizeEntry.ResetBtn_Click | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs:115 | object sender, EventArgs e | SInventory_UI_ShippingCartonSizeEntry.ResetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ShippingCartonSizeBLL.ShippingCartonSizeEditLoad | Library.BLL/SInventory_BLL/ShippingCartonSizeBLL.cs:50 |
| ShippingCartonSizeBLL.UpdateShippingCartonSizeInfo | Library.BLL/SInventory_BLL/ShippingCartonSizeBLL.cs:55 |
| ShippingCartonSizeBLL.SaveShippingCartonSize | Library.BLL/SInventory_BLL/ShippingCartonSizeBLL.cs:17 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ShippingCartonSizeDAL.ShippingCartonSizeEditLoad | Library.DAL/SInventory_DAL/ShippingCartonSizeDAL.cs:54 | inline SQL: tblProductCase | @CaseId |
| ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo | Library.DAL/SInventory_DAL/ShippingCartonSizeDAL.cs:74 | inline SQL: tblProductCase | @CaseId, @CaseQty, @PcsPerCase, @ProductCode |
| ShippingCartonSizeDAL.SaveShippingCartonSize | Library.DAL/SInventory_DAL/ShippingCartonSizeDAL.cs:17 | inline SQL: tblProductCase | @CaseId, @CaseQty, @PcsPerCase, @ProductCode |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblProductCase
- Tables WRITTEN (inline SQL reachable): tblProductCase
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblProductCase | CaseId | @CaseId |
| tblProductCase | CaseQty | @CaseQty |
| tblProductCase | PcsPerCase | @PcsPerCase |
| tblProductCase | ProductCode | @ProductCode |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','ShippingCartonSizeView.aspx'); | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

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
- Redirect targets: ShippingCartonSizeEntry.aspx, ShippingCartonSizeView.aspx
- User messages: ','Success','ShippingCartonSizeView.aspx');; Already Exist!; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ShippingCartonSizeBLL; DAL: ShippingCartonSizeDAL; DAO: ShippingCartonSize; Tables (inline): tblProductCase

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click detailsViewButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click detailsViewButton with valid data | Handler runs; rows written to tblProductCase |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tblProductCase |
| TC-07 | Click updateButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click updateButton with valid data | Handler runs; rows written to tblProductCase |
| TC-09 | Click ResetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click ResetBtn with valid data | Handler runs; rows written to tblProductCase |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx`, `Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 12 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ShippingCartonSizeEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ShippingCartonSizeDAL.SaveShippingCartonSize, ShippingCartonSizeDAL.ShippingCartonSizeEditLoad, ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
