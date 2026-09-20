---
page_id: PAGE-188
module: M05
page: CustomerCategoryEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-188 — CustomerCategoryEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-188 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerCategoryEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CustomerCategoryEntry |
| aspx size | 5072 bytes |
| Code-behind LOC | 42 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerCategoryEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CustCategoryView.aspx`

Outbound references: `custcategoryview.aspx`

## 5. Route
`~/SInventory_UI/CustomerCategoryEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | CustomerCategoryBLL (Library.BLL/SInventory_BLL/CustomerCategoryBLL.cs) |
| DAL class | CustomerCategoryDAL (Library.DAL/SInventory_DAL/CustomerCategoryDAL.cs) |
| DAO/entity class | CustomerCategory (Library.DAO/SInventory_Entities/CustomerCategory.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| empCetegoryListImageButton | ImageButton |  | no validator found |  |  |  | onclick→EmpCetegoryListImageButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| empCategoryNameTextBox | TextBox |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| empCategoryNameTextBox | CustomerCategory.CategoryName | SInventory_UI_CustomerCategoryEntry.submitButton_Click | CustomerCategoryBLL.CustomerCategoryCodeGenerator, CustomerCategoryBLL.SaveDataForCustomerCategory | CustomerCategoryDAL.HasCustCategoryName, CustomerCategoryDAL.SaveCustomerCategory | UNKNOWN | UNKNOWN | UNKNOWN |

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
| SInventory_UI_CustomerCategoryEntry.Page_Load | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx.cs:13 | object sender, EventArgs e | SInventory_UI_CustomerCategoryEntry.Page_Load | no | no |
| SInventory_UI_CustomerCategoryEntry.submitButton_Click | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx.cs:22 | object sender, EventArgs e | SInventory_UI_CustomerCategoryEntry.submitButton_Click → CustomerCategoryBLL.CustomerCategoryCodeGenerator / CustomerCategoryBLL.SaveDataForCustomerCategory → CustomerCategoryDAL.HasCustCategoryName / CustomerCategoryDAL.SaveCustomerCategory | no | no |
| SInventory_UI_CustomerCategoryEntry.EmpCetegoryListImageButton_Click | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx.cs:39 | object sender, ImageClickEventArgs e | SInventory_UI_CustomerCategoryEntry.EmpCetegoryListImageButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustomerCategoryBLL.SaveDataForCustomerCategory | Library.BLL/SInventory_BLL/CustomerCategoryBLL.cs:16 |
| CustomerCategoryBLL.CustomerCategoryCodeGenerator | Library.BLL/SInventory_BLL/CustomerCategoryBLL.cs:43 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerCategoryDAL.HasCustCategoryName | Library.DAL/SInventory_DAL/CustomerCategoryDAL.cs:30 | inline SQL: tblCustCategory | @CategoryName |
| CustomerCategoryDAL.SaveCustomerCategory | Library.DAL/SInventory_DAL/CustomerCategoryDAL.cs:17 | inline SQL: tblCustCategory | @CategoryCode, @CategoryId, @CategoryName |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCustCategory
- Tables WRITTEN (inline SQL reachable): tblCustCategory
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCustCategory | CategoryCode | @CategoryCode |
| tblCustCategory | CategoryId | @CategoryId |
| tblCustCategory | CategoryName | @CategoryName |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
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
- try/catch present in 0 of 4 methods.
- Redirect targets: CustCategoryView.aspx
- User messages: Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerCategoryBLL; DAL: CustomerCategoryDAL; DAO: CustomerCategory; Tables (inline): tblCustCategory

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx`, `Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 7 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerCategoryEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerCategoryDAL.HasCustCategoryName, CustomerCategoryDAL.SaveCustomerCategory affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
