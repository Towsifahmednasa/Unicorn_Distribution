---
page_id: PAGE-077
module: M03
page: DepartmentEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-077 — DepartmentEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-077 |
| Module | M03 — Organisation & Market Structure Master (INFERRED from folder + file name) |
| Page name | DepartmentEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_DepartmentEntry |
| aspx size | 5566 bytes |
| Code-behind LOC | 42 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DepartmentEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Company/branch/unit, geography (division/district/thana), sales hierarchy (national/zone/region/area/territory/market), routes, depot-territory mapping and structure transfers.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/DepartmentView.aspx`

Outbound references: `departmentview.aspx`

## 5. Route
`~/SInventory_UI/DepartmentEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DepartmentEntry.aspx |
| Code-behind | Solution.Web/SInventory_UI/DepartmentEntry.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | DepartmentBLL (Library.BLL/SInventory_BLL/DepartmentBLL.cs) |
| DAL class | DepartmentDAL (Library.DAL/SInventory_DAL/DepartmentDAL.cs) |
| DAO/entity class | Department (Library.DAO/DoctorModule_DAO/Department.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| departmentListImageButton | ImageButton |  | no validator found |  |  |  | onclick→departmentListImageButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| DepartmentNameTextBox | TextBox |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | onclick→submitButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| DepartmentNameTextBox | Department.DepartmentName | SInventory_UI_DepartmentEntry.submitButton_Click | DepartmentBLL.DepartmentCodeGenerator, DepartmentBLL.SaveDataForDepartment | DepartmentDAL.HasDeptName, DepartmentDAL.SaveDepartmentInfo | UNKNOWN | UNKNOWN | UNKNOWN |

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
| SInventory_UI_DepartmentEntry.Page_Load | Solution.Web/SInventory_UI/DepartmentEntry.aspx.cs:13 | object sender, EventArgs e | SInventory_UI_DepartmentEntry.Page_Load | no | no |
| SInventory_UI_DepartmentEntry.submitButton_Click | Solution.Web/SInventory_UI/DepartmentEntry.aspx.cs:22 | object sender, EventArgs e | SInventory_UI_DepartmentEntry.submitButton_Click → DepartmentBLL.DepartmentCodeGenerator / DepartmentBLL.SaveDataForDepartment → DepartmentDAL.HasDeptName / DepartmentDAL.SaveDepartmentInfo | no | no |
| SInventory_UI_DepartmentEntry.departmentListImageButton_Click | Solution.Web/SInventory_UI/DepartmentEntry.aspx.cs:39 | object sender, ImageClickEventArgs e | SInventory_UI_DepartmentEntry.departmentListImageButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DepartmentBLL.SaveDataForDepartment | Library.BLL/SInventory_BLL/DepartmentBLL.cs:16 |
| DepartmentBLL.DepartmentCodeGenerator | Library.BLL/SInventory_BLL/DepartmentBLL.cs:43 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DepartmentDAL.HasDeptName | Library.DAL/SInventory_DAL/DepartmentDAL.cs:29 | inline SQL: tblDepartment | @DeptName |
| DepartmentDAL.SaveDepartmentInfo | Library.DAL/SInventory_DAL/DepartmentDAL.cs:16 | inline SQL: tblDepartment | @DeptCode, @DeptId, @DeptName |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblDepartment
- Tables WRITTEN (inline SQL reachable): tblDepartment
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblDepartment | DeptCode | @DeptCode |
| tblDepartment | DeptId | @DeptId |
| tblDepartment | DeptName | @DeptName |

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
- Redirect targets: DepartmentView.aspx
- User messages: Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DepartmentBLL; DAL: DepartmentDAL; DAO: Department; Tables (inline): tblDepartment

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DepartmentEntry.aspx`, `Solution.Web/SInventory_UI/DepartmentEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DepartmentEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 7 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DepartmentEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M03 (Organisation & Market Structure Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DepartmentDAL.HasDeptName, DepartmentDAL.SaveDepartmentInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
