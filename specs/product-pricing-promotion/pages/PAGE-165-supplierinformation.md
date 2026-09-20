---
page_id: PAGE-165
module: M04
page: SupplierInformation
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-165 — SupplierInformation

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-165 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | SupplierInformation |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_SupplierInformation |
| aspx size | 9869 bytes |
| Code-behind LOC | 184 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SupplierInformation** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/SupplierInfoView.aspx`

Outbound references: `supplierinfoview.aspx`

## 5. Route
`~/SInventory_UI/SupplierInformation.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SupplierInformation.aspx |
| Code-behind | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| DAL class | SupplierInfoDal (Library.DAL/SInventory_DAL/SupplierInfoDal.cs) |
| DAO/entity class | SupplierInformation (Library.DAO/SInventory_Entities/SupplierInformation.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| companyInfotListImageButton | ImageButton |  | no validator found |  |  |  | onclick→CompanyListImageButton_Click |
| companynameTextBox | TextBox |  | no validator found |  |  |  |  |
| productidHiddenField | HiddenField |  | no validator found |  |  |  |  |
| companyAddressTextBox | TextBox |  | no validator found |  |  |  |  |
| contactTextBox | TextBox |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click1 |
| clearButton | Button | Clear | no validator found |  |  |  | OnClick→clearButton_OnClick |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| companynameTextBox | SupplierInformation.SupplierName | SInventory_UI_SupplierInformation.submitButton_Click1 | — | SupplierInfoDal.CheckDuplicate, SupplierInfoDal.CompanyCodeGenerator, SupplierInfoDal.HasCompanyName, SupplierInfoDal.Sa | UNKNOWN | UNKNOWN | UNKNOWN |
| companyAddressTextBox | SupplierInformation.Address | SInventory_UI_SupplierInformation.submitButton_Click1 | — | SupplierInfoDal.CheckDuplicate, SupplierInfoDal.CompanyCodeGenerator, SupplierInfoDal.HasCompanyName, SupplierInfoDal.Sa | UNKNOWN | UNKNOWN | UNKNOWN |
| contactTextBox | SupplierInformation.ContactNo | SInventory_UI_SupplierInformation.submitButton_Click1 | — | SupplierInfoDal.CheckDuplicate, SupplierInfoDal.CompanyCodeGenerator, SupplierInfoDal.HasCompanyName, SupplierInfoDal.Sa | UNKNOWN | UNKNOWN | UNKNOWN |
| productidHiddenField | SupplierInformation.SupplierId | SInventory_UI_SupplierInformation.submitButton_Click1 | (none — page calls DAL directly) | SupplierInfoDal.CheckDuplicate | @SupplierId | UNKNOWN (SP body not in repo) | UNKNOWN |
| productidHiddenField | SupplierInformation.SupplierId | SInventory_UI_SupplierInformation.submitButton_Click1 | (none — page calls DAL directly) | SupplierInfoDal.UpdateCompanyInfoData | @SupplierId | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| clearButton | Button click | (postback) | SInventory_UI_SupplierInformation.clearButton_OnClick | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs:158 | — | — | — | read-only / none detected |

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
| SInventory_UI_SupplierInformation.Page_Load | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_SupplierInformation.Page_Load → SupplierInfoDal.LoadSupplierInfoById | no | no |
| SInventory_UI_SupplierInformation.LoadDataForUpdate | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs:32 | int supplierid | SInventory_UI_SupplierInformation.LoadDataForUpdate → SupplierInfoDal.LoadSupplierInfoById | no | no |
| SInventory_UI_SupplierInformation.submitButton_Click1 | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs:92 | object sender, EventArgs e | SInventory_UI_SupplierInformation.submitButton_Click1 → SupplierInfoDal.CheckDuplicate / SupplierInfoDal.CompanyCodeGenerator / SupplierInfoDal.HasCompanyName / SupplierInfoDal.SaveCompanyInfoData / SupplierInfoDal.SaveDataForCompanyInfo / SupplierInfoDal. | no | no |
| SInventory_UI_SupplierInformation.CompanyListImageButton_Click | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs:153 | object sender, ImageClickEventArgs e | SInventory_UI_SupplierInformation.CompanyListImageButton_Click | no | no |
| SInventory_UI_SupplierInformation.clearButton_OnClick | Solution.Web/SInventory_UI/SupplierInformation.aspx.cs:158 | object sender, EventArgs e | SInventory_UI_SupplierInformation.clearButton_OnClick | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SupplierInfoDal.LoadSupplierInfoById | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:96 | inline SQL: tblSupplierInformation | @SupplierId |
| SupplierInfoDal.SaveCompanyInfoData | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:49 | delegates / no SQL literal |  |
| SupplierInfoDal.CompanyCodeGenerator | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:75 | delegates / no SQL literal |  |
| SupplierInfoDal.HasCompanyName | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:39 | inline SQL: tblSupplierInformation | @SupplierName |
| SupplierInfoDal.SaveDataForCompanyInfo | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:18 | inline SQL: tblSupplierInformation | @ContactNo, @EntryDate, @Entryby, @SupplierAddress, @SupplierCode, @SupplierName |
| SupplierInfoDal.CheckDuplicate | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:119 | inline SQL: tblSupplierInformation | @SupplierId, @SupplierName |
| SupplierInfoDal.UpdateCompanyInfoData | Library.DAL/SInventory_DAL/SupplierInfoDal.cs:105 | inline SQL: tblSupplierInformation | @ContactNo, @SupplierAddress, @SupplierId, @SupplierName, @UpdateDate, @Updateby |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblSupplierInformation
- Tables WRITTEN (inline SQL reachable): tblSupplierInformation
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblSupplierInformation | ContactNo | @ContactNo |
| tblSupplierInformation | EntryDate | @EntryDate |
| tblSupplierInformation | Entryby | @Entryby |
| tblSupplierInformation | SupplierAddress | @SupplierAddress |
| tblSupplierInformation | SupplierCode | @SupplierCode |
| tblSupplierInformation | SupplierName | @SupplierName |
| tblSupplierInformation | UpdateDate | @UpdateDate |
| tblSupplierInformation | Updateby | @Updateby |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input  Address!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Contact Number!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Supplier Name!! | UNKNOWN |
| (handler submitButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Company Code already exist | UNKNOWN |
| (handler submitButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Supplier already exist !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: SupplierInfoView.aspx
- User messages: Company Code already exist; Please Input  Address!!; Please Input Contact Number!!; Please Input Supplier Name!!; Supplier already exist !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: EntryDate, Entryby, UpdateDate, Updateby. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: SupplierInfoDal; DAO: SupplierInformation; Tables (inline): tblSupplierInformation

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click clearButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click clearButton with valid data | Handler runs; rows written to tblSupplierInformation |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SupplierInformation.aspx`, `Solution.Web/SInventory_UI/SupplierInformation.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SupplierInformation.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 12 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SupplierInformation.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods SupplierInfoDal.CheckDuplicate, SupplierInfoDal.CompanyCodeGenerator, SupplierInfoDal.HasCompanyName, SupplierInfoDal.LoadSupplierInfoById, SupplierInfoDal.SaveCompanyInfoData affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
