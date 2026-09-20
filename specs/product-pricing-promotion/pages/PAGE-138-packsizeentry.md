---
page_id: PAGE-138
module: M04
page: PackSizeEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-138 — PackSizeEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-138 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | PackSizeEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_PackSizeEntry |
| aspx size | 5002 bytes |
| Code-behind LOC | 119 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **PackSizeEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/PackSizeView.aspx`, `SInventory_UI/ProductEntry.aspx`

Outbound references: `packsizeview.aspx`

## 5. Route
`~/SInventory_UI/PackSizeEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/PackSizeEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | PackSizeBLL (Library.BLL/SInventory_BLL/PackSizeBLL.cs) |
| DAL class | PackSizeDAL (Library.DAL/SInventory_DAL/PackSizeDAL.cs) |
| DAO/entity class | PackSize (Library.DAO/SInventory_Entities/PackSize.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| detailsViewButton | LinkButton |  | no validator found |  |  |  | OnClick→detailsViewButton_Click |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait7 | Image |  | no validator found |  |  |  |  |
| packsizeNameTextBox | TextBox |  | no validator found |  |  |  |  |
| packSizeIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→submitButton_Click |
| updateButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→submitButton_Click |
| ResetBtn | LinkButton |  | no validator found |  |  |  | OnClick→ResetBtn_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| packSizeIdHiddenField | PackSize.PackSizeId | SInventory_UI_PackSizeEntry.submitButton_Click | PackSizeBLL.SavePackSize, PackSizeBLL.UpdatePackSizeInfo | PackSizeDAL.HasPackSizeNameUp | @PackSizeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| packSizeIdHiddenField | PackSize.PackSizeId | SInventory_UI_PackSizeEntry.submitButton_Click | PackSizeBLL.SavePackSize, PackSizeBLL.UpdatePackSizeInfo | PackSizeDAL.UpdatePackSizeInfo | @PackSizeId | UNKNOWN (SP body not in repo) | UNKNOWN |
| packsizeNameTextBox | PackSize.PackSizeName | SInventory_UI_PackSizeEntry.submitButton_Click | PackSizeBLL.SavePackSize, PackSizeBLL.UpdatePackSizeInfo | PackSizeDAL.HasPackSizeName, PackSizeDAL.HasPackSizeNameUp, PackSizeDAL.SavePackSize, PackSizeDAL.UpdatePackSizeInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| detailsViewButton | LinkButton click | (postback) | SInventory_UI_PackSizeEntry.detailsViewButton_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:109 | — | — | — | read-only / none detected |
| submitButton | LinkButton click | (postback) | SInventory_UI_PackSizeEntry.submitButton_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:46 | PackSizeBLL.SavePackSize, PackSizeBLL.UpdatePackSizeInfo | PackSizeDAL.HasPackSizeName, PackSizeDAL.HasPackSizeNameUp, PackSizeDAL.SavePackSize, PackSizeDAL.Up | — | writes tblPackSize |
| updateButton | LinkButton click | (postback) | SInventory_UI_PackSizeEntry.submitButton_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:46 | PackSizeBLL.SavePackSize, PackSizeBLL.UpdatePackSizeInfo | PackSizeDAL.HasPackSizeName, PackSizeDAL.HasPackSizeNameUp, PackSizeDAL.SavePackSize, PackSizeDAL.Up | — | writes tblPackSize |
| ResetBtn | LinkButton click | (postback) | SInventory_UI_PackSizeEntry.ResetBtn_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:114 | — | — | — | read-only / none detected |

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
| SInventory_UI_PackSizeEntry.Page_Load | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_PackSizeEntry.Page_Load → PackSizeBLL.PackSizeEditLoad → PackSizeDAL.PackSizeEditLoad | no | no |
| SInventory_UI_PackSizeEntry.PackSizeLoad | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:33 | string departmentId | SInventory_UI_PackSizeEntry.PackSizeLoad → PackSizeBLL.PackSizeEditLoad → PackSizeDAL.PackSizeEditLoad | no | no |
| SInventory_UI_PackSizeEntry.submitButton_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:46 | object sender, EventArgs e | SInventory_UI_PackSizeEntry.submitButton_Click → PackSizeBLL.SavePackSize / PackSizeBLL.UpdatePackSizeInfo → PackSizeDAL.HasPackSizeName / PackSizeDAL.HasPackSizeNameUp / PackSizeDAL.SavePackSize / PackSizeDAL.UpdatePackSizeInfo | no | no |
| SInventory_UI_PackSizeEntry.detailsViewButton_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:109 | object sender, EventArgs e | SInventory_UI_PackSizeEntry.detailsViewButton_Click | no | no |
| SInventory_UI_PackSizeEntry.ResetBtn_Click | Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs:114 | object sender, EventArgs e | SInventory_UI_PackSizeEntry.ResetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| PackSizeBLL.PackSizeEditLoad | Library.BLL/SInventory_BLL/PackSizeBLL.cs:49 |
| PackSizeBLL.UpdatePackSizeInfo | Library.BLL/SInventory_BLL/PackSizeBLL.cs:54 |
| PackSizeBLL.SavePackSize | Library.BLL/SInventory_BLL/PackSizeBLL.cs:16 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| PackSizeDAL.PackSizeEditLoad | Library.DAL/SInventory_DAL/PackSizeDAL.cs:56 | inline SQL: tblPackSize | @PackSizeId |
| PackSizeDAL.HasPackSizeNameUp | Library.DAL/SInventory_DAL/PackSizeDAL.cs:35 | inline SQL: tblPackSize | @PackSizeId, @PackSizeName |
| PackSizeDAL.UpdatePackSizeInfo | Library.DAL/SInventory_DAL/PackSizeDAL.cs:74 | inline SQL: tblPackSize | @PackSizeId, @PackSizeName |
| PackSizeDAL.HasPackSizeName | Library.DAL/SInventory_DAL/PackSizeDAL.cs:25 | inline SQL: tblPackSize | @PackSizeName |
| PackSizeDAL.SavePackSize | Library.DAL/SInventory_DAL/PackSizeDAL.cs:14 | inline SQL: tblPackSize | @PackSizeName |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblPackSize
- Tables WRITTEN (inline SQL reachable): tblPackSize
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblPackSize | PackSizeName | @PackSizeName |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','PackSizeView.aspx'); | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist | UNKNOWN |
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
- Redirect targets: PackSizeEntry.aspx, PackSizeView.aspx
- User messages: ','Success','PackSizeView.aspx');; Already Exist; Already Exist!; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: PackSizeBLL; DAL: PackSizeDAL; DAO: PackSize; Tables (inline): tblPackSize

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click detailsViewButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click detailsViewButton with valid data | Handler runs; rows written to tblPackSize |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tblPackSize |
| TC-07 | Click updateButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click updateButton with valid data | Handler runs; rows written to tblPackSize |
| TC-09 | Click ResetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click ResetBtn with valid data | Handler runs; rows written to tblPackSize |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/PackSizeEntry.aspx`, `Solution.Web/SInventory_UI/PackSizeEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/PackSizeEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 11 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/PackSizeEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods PackSizeDAL.HasPackSizeName, PackSizeDAL.HasPackSizeNameUp, PackSizeDAL.PackSizeEditLoad, PackSizeDAL.SavePackSize, PackSizeDAL.UpdatePackSizeInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
