---
page_id: PAGE-152
module: M04
page: ProductView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-152 — ProductView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-152 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | ProductView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_ProductView |
| aspx size | 7940 bytes |
| Code-behind LOC | 108 |
| Inline script size | 1772 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ProductView** (List / View). Page title/heading text: `Product Information List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ProductEntry.aspx`

Outbound references: `productedit.aspx`, `productentry.aspx`

## 5. Route
`~/SInventory_UI/ProductView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ProductView.aspx |
| Code-behind | Solution.Web/SInventory_UI/ProductView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ProductBLL (Library.BLL/SInventory_BLL/ProductBLL.cs) |
| DAL class | ProductDAL (Library.DAL/SInventory_DAL/ProductDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| productLineDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| categoryDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| genericGroupDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | Button | Search | no validator found |  |  |  | OnClick→searchButton_Click |
| resetButton | Button | Reset | no validator found |  |  |  | OnClick→resetButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Generic Group | no validator found |  |  |  |  |
| (no id) | BoundField | Category | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Description | no validator found |  |  |  |  |
| (no id) | BoundField | UOM | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Category | no validator found |  |  |  |  |
| (no id) | BoundField | Product Group | no validator found |  |  |  |  |
| (no id) | TemplateField | Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| editImageButton | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| categoryDropDownList | categoryId | SInventory_UI_ProductView.ProductLoad | ProductBLL.LoadProductSearch | ProductDAL.LoadProductSearch | @CategoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| genericGroupDropDownList | genericGroupId | SInventory_UI_ProductView.ProductLoad | ProductBLL.LoadProductSearch | ProductDAL.LoadProductSearch | @GenericGroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| productLineDropDownList | productLineId | SInventory_UI_ProductView.ProductLoad | ProductBLL.LoadProductSearch | ProductDAL.LoadProductSearch | @ProductLineID | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| productLineDropDownList | BindDropDowns → ProductBLL.LoadProductLine | sp_GET_ProductLine | LineName/ProductLineID | UNKNOWN |
| categoryDropDownList | BindDropDowns → ProductBLL.LoadProductCategory | inline SQL | CategoryName/CategoryId | tblProCategory |
| genericGroupDropDownList | BindDropDowns → ProductBLL.LoadGenericGroup | inline SQL | GenericGroupName/GenericGroupId | tblGenericGroup |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| searchButton | Button click | (postback) | SInventory_UI_ProductView.searchButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:55 | ProductBLL.LoadProductSearch | ProductDAL.GetDataTableByText, ProductDAL.LoadProductSearch | — | read-only / none detected |
| resetButton | Button click | (postback) | SInventory_UI_ProductView.resetButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:60 | ProductBLL.LoadProductSearch | ProductDAL.GetDataTableByText, ProductDAL.LoadProductSearch | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_ProductView.Page_Load | Solution.Web/SInventory_UI/ProductView.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_ProductView.Page_Load → ProductBLL.LoadGenericGroup / ProductBLL.LoadProductCategory / ProductBLL.LoadProductLine / ProductBLL.LoadProductSearch → ProductDAL.GetDataTableByText / ProductDAL.LoadCategoryName / ProductDAL.LoadGenericGroup / Product | no | no |
| SInventory_UI_ProductView.BindDropDowns | Solution.Web/SInventory_UI/ProductView.aspx.cs:24 |  | SInventory_UI_ProductView.BindDropDowns → ProductBLL.LoadGenericGroup / ProductBLL.LoadProductCategory / ProductBLL.LoadProductLine → ProductDAL.LoadCategoryName / ProductDAL.LoadGenericGroup / SeedDataDAL.GetProLineDataTableList → SP: sp_GET_ProductLine | no | no |
| SInventory_UI_ProductView.ProductLoad | Solution.Web/SInventory_UI/ProductView.aspx.cs:36 |  | SInventory_UI_ProductView.ProductLoad → ProductBLL.LoadProductSearch → ProductDAL.GetDataTableByText / ProductDAL.LoadProductSearch | yes | no |
| SInventory_UI_ProductView.searchButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:55 | object sender, EventArgs e | SInventory_UI_ProductView.searchButton_Click → ProductBLL.LoadProductSearch → ProductDAL.GetDataTableByText / ProductDAL.LoadProductSearch | no | no |
| SInventory_UI_ProductView.resetButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:60 | object sender, EventArgs e | SInventory_UI_ProductView.resetButton_Click → ProductBLL.LoadProductSearch → ProductDAL.GetDataTableByText / ProductDAL.LoadProductSearch | no | no |
| SInventory_UI_ProductView.loadGridView_RowCommand | Solution.Web/SInventory_UI/ProductView.aspx.cs:74 | object sender, GridViewCommandEventArgs e | SInventory_UI_ProductView.loadGridView_RowCommand | no | no |
| SInventory_UI_ProductView.miaTargetReloadImageButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:85 | object sender, ImageClickEventArgs e | SInventory_UI_ProductView.miaTargetReloadImageButton_Click → ProductBLL.LoadProductSearch → ProductDAL.GetDataTableByText / ProductDAL.LoadProductSearch | no | no |
| SInventory_UI_ProductView.miaTargetNewImageButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:89 | object sender, ImageClickEventArgs e | SInventory_UI_ProductView.miaTargetNewImageButton_Click | no | no |
| SInventory_UI_ProductView.EmpCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/ProductView.aspx.cs:94 | object sender, EventArgs e | SInventory_UI_ProductView.EmpCetegoryAddImageButton_Click | no | no |
| SInventory_UI_ProductView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/ProductView.aspx.cs:98 | object sender, EventArgs e | SInventory_UI_ProductView.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ProductBLL.LoadProductCategory | Library.BLL/SInventory_BLL/ProductBLL.cs:86 |
| ProductBLL.LoadGenericGroup | Library.BLL/SInventory_BLL/ProductBLL.cs:119 |
| ProductBLL.LoadProductLine | Library.BLL/SInventory_BLL/ProductBLL.cs:140 |
| ProductBLL.LoadProductSearch | Library.BLL/SInventory_BLL/ProductBLL.cs:136 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ProductDAL.LoadCategoryName | Library.DAL/SInventory_DAL/ProductDAL.cs:209 | inline SQL: tblProCategory |  |
| ProductDAL.LoadGenericGroup | Library.DAL/SInventory_DAL/ProductDAL.cs:259 | inline SQL: tblGenericGroup |  |
| SeedDataDAL.GetProLineDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:629 | sp_GET_ProductLine |  |
| ProductDAL.LoadProductSearch | Library.DAL/SInventory_DAL/ProductDAL.cs:307 | inline SQL: tblGenericGroup, tblPackSize, tblProCategory, tblProduct | @CategoryId, @GenericGroupId, @ProductLineID |
| ProductDAL.GetDataTableByText | Library.DAL/SInventory_DAL/ProductDAL.cs:25 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ProductLine | SeedDataDAL.GetProLineDataTableList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblGenericGroup, tblPackSize, tblProCategory, tblProduct, tblProductCase, tblProductGroup, tblStockUOM
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
- try/catch present in 1 of 11 methods.
- Redirect targets: ProductEntry.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ProductBLL; DAL: ProductDAL, SeedDataDAL; DAO: —; Tables (inline): tblGenericGroup, tblPackSize, tblProCategory, tblProduct, tblProductCase, tblProductGroup, tblStockUOM

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click resetButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click resetButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ProductView.aspx`, `Solution.Web/SInventory_UI/ProductView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ProductView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 22 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ProductView.aspx |
| Data access | This document §15–17 | 1 SP names, 7 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: PAGE-111, PAGE-112, PAGE-113, PAGE-114, PAGE-115, PAGE-117, PAGE-147, PAGE-168, PAGE-613, PAGE-614
- Changing DAL methods ProductDAL.GetDataTableByText, ProductDAL.LoadCategoryName, ProductDAL.LoadGenericGroup, ProductDAL.LoadProductSearch, SeedDataDAL.GetProLineDataTableList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
