---
page_id: PAGE-171
module: M05
page: CustomerTypeSetup
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-171 — CustomerTypeSetup

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-171 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerTypeSetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_CustomerTypeSetup |
| aspx size | 380 bytes |
| Code-behind LOC | 65 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerTypeSetup** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/MasterSetup_UI/CustomerTypeSetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CustomerTypeDal (Library.DAL/MasterSetup_DAL/CustomerTypeDal.cs) |
| DAO/entity class | CustomerType (Library.DAO/DoctorModule_DAO/CustomerType.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
No server/HTML controls with an ID were extracted (viewer-only or code-generated page).

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

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
| MasterSetup_UI_CustomerTypeSetup.Page_Load | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:16 | object sender, EventArgs e | MasterSetup_UI_CustomerTypeSetup.Page_Load | no | no |
| MasterSetup_UI_CustomerTypeSetup.Save_DepartmentInfo | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:26 | CustomerType department | MasterSetup_UI_CustomerTypeSetup.Save_DepartmentInfo → CustomerTypeDal.Save_DepartmentInfo → SP: sp_Save_CustomerTypeInfo, sp_UD_CustomerTypeInfo, sp_checkIsDefault_CustomerType, sp_check_CustomerType | no | yes |
| MasterSetup_UI_CustomerTypeSetup.GetDepartmentList | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:37 |  | MasterSetup_UI_CustomerTypeSetup.GetDepartmentList → CustomerTypeDal.GetDepartmentList → SP: sp_GET_CustomerInfo | no | no |
| MasterSetup_UI_CustomerTypeSetup.ActiveInactive_departmentInfo | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:48 | int Id | MasterSetup_UI_CustomerTypeSetup.ActiveInactive_departmentInfo → CustomerTypeDal.ActiveInactive_DepartmentInfo → SP: sp_ActiveInactive_customerType_ById | no | yes |
| MasterSetup_UI_CustomerTypeSetup.GetDepartmentEditData | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:54 | int id | MasterSetup_UI_CustomerTypeSetup.GetDepartmentEditData → CustomerTypeDal.GetEmployeeLeaveForEdit → SP: sp_GET_CustomerType_ById | no | no |
| MasterSetup_UI_CustomerTypeSetup.Delete_EmployeeDepartment | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:61 | int Id | MasterSetup_UI_CustomerTypeSetup.Delete_EmployeeDepartment → CustomerTypeDal.Delete_employeeleave → SP: sp_Delete_DepartmnetInfo | no | yes |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| MasterSetup_UI_CustomerTypeSetup.Save_DepartmentInfo | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:26 | CustomerType department | ~/MasterSetup_UI/CustomerTypeSetup.aspx/Save_DepartmentInfo | via BLL |
| MasterSetup_UI_CustomerTypeSetup.GetDepartmentList | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:37 |  | ~/MasterSetup_UI/CustomerTypeSetup.aspx/GetDepartmentList | via BLL |
| MasterSetup_UI_CustomerTypeSetup.ActiveInactive_departmentInfo | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:48 | int Id | ~/MasterSetup_UI/CustomerTypeSetup.aspx/ActiveInactive_departmentInfo | via BLL |
| MasterSetup_UI_CustomerTypeSetup.GetDepartmentEditData | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:54 | int id | ~/MasterSetup_UI/CustomerTypeSetup.aspx/GetDepartmentEditData | via BLL |
| MasterSetup_UI_CustomerTypeSetup.Delete_EmployeeDepartment | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs:61 | int Id | ~/MasterSetup_UI/CustomerTypeSetup.aspx/Delete_EmployeeDepartment | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerTypeDal.Save_DepartmentInfo | Library.DAL/MasterSetup_DAL/CustomerTypeDal.cs:18 | sp_Save_CustomerTypeInfo, sp_UD_CustomerTypeInfo, sp_checkIsDefault_CustomerType | @CustomerCategoryId, @CustomerType, @EntryBy, @IsActive, @IsCampaign, @IsDefault, @UpdateBy, @id |
| CustomerTypeDal.GetDepartmentList | Library.DAL/MasterSetup_DAL/CustomerTypeDal.cs:107 | sp_GET_CustomerInfo | @Parameter |
| CustomerTypeDal.ActiveInactive_DepartmentInfo | Library.DAL/MasterSetup_DAL/CustomerTypeDal.cs:129 | sp_ActiveInactive_customerType_ById | @CustomerTypeId, @InactiveBy |
| CustomerTypeDal.GetEmployeeLeaveForEdit | Library.DAL/MasterSetup_DAL/CustomerTypeDal.cs:162 | sp_GET_CustomerType_ById | @id |
| CustomerTypeDal.Delete_employeeleave | Library.DAL/MasterSetup_DAL/CustomerTypeDal.cs:209 | sp_Delete_DepartmnetInfo | @DeptId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_ActiveInactive_customerType_ById | CustomerTypeDal.ActiveInactive_DepartmentInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Delete_DepartmnetInfo | CustomerTypeDal.Delete_employeeleave | body NOT in repository | write-like name |
| sp_GET_CustomerInfo | CustomerTypeDal.GetDepartmentList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CustomerType_ById | CustomerTypeDal.GetEmployeeLeaveForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Save_CustomerTypeInfo | CustomerTypeDal.Save_DepartmentInfo | body NOT in repository | write-like name |
| sp_UD_CustomerTypeInfo | CustomerTypeDal.Save_DepartmentInfo | body NOT in repository | read-like name (INFERRED) |
| sp_checkIsDefault_CustomerType | CustomerTypeDal.Save_DepartmentInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_CustomerType | CustomerTypeDal.Save_DepartmentInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
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
- try/catch present in 0 of 6 methods.
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
BLL: —; DAL: CustomerTypeDal; DAO: CustomerType, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx`, `Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 2 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/CustomerTypeSetup.aspx |
| Data access | This document §15–17 | 8 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 8 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: PAGE-567
- Changing DAL methods CustomerTypeDal.ActiveInactive_DepartmentInfo, CustomerTypeDal.Delete_employeeleave, CustomerTypeDal.GetDepartmentList, CustomerTypeDal.GetEmployeeLeaveForEdit, CustomerTypeDal.Save_DepartmentInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
