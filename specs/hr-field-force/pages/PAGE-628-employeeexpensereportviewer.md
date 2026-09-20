---
page_id: PAGE-628
module: M15
page: EmployeeExpenseReportViewer
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-628 — EmployeeExpenseReportViewer

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-628 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | EmployeeExpenseReportViewer |
| Kind | Report viewer (INFERRED from name) |
| Web folder | SInventory_RPTVIEW |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_RPTVIEW_AuditReportOneReportViewer |
| aspx size | 4606 bytes |
| Code-behind LOC | 194 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **EmployeeExpenseReportViewer** (Report viewer). Page title/heading text: `Employee Monthly Expense List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `Reports_UI/EmpMonthlyExpenseRptMultiple.aspx`, `Reports_UI/OrderPermission.aspx`, `Reports_UI/TourPlanReportNew.aspx`, `Reports_UI/TourPlanSummaryReport.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx |
| Code-behind | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| DAL class | ExpenseDal (Library.DAL/MasterSetup_DAL/ExpenseDal.cs) |
| Report (.rpt) | crpEmpExpense.rpt |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| crvSalesRpt | CR:CrystalReportViewer |  | no validator found |  |  |  |  |

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
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_RPTVIEW_AuditReportOneReportViewer.Page_Init | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs:17 | object sender, EventArgs e | SInventory_RPTVIEW_AuditReportOneReportViewer.Page_Init → ExpenseDal.GetExpenseMasterById / ExpenseDal.GetExpenseMasterTotalById / ExpenseDal.GetGet_TourPlanBalanceMasterById / ExpenseDal.Get_EmpAllawance → SP: sp_Get_MonthlyExpenseEmpWiseMaster_Final, sp_Get_ | yes | no |
| SInventory_RPTVIEW_AuditReportOneReportViewer.closeButton_Click | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs:172 | object sender, EventArgs e | SInventory_RPTVIEW_AuditReportOneReportViewer.closeButton_Click | no | no |
| SInventory_RPTVIEW_AuditReportOneReportViewer.crvCustMasterRpt_Unload | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs:177 | object sender, EventArgs e | SInventory_RPTVIEW_AuditReportOneReportViewer.crvCustMasterRpt_Unload | no | no |
| SInventory_RPTVIEW_AuditReportOneReportViewer.crvCustMasterRpt_Disposed | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs:186 | object sender, EventArgs e | SInventory_RPTVIEW_AuditReportOneReportViewer.crvCustMasterRpt_Disposed | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ExpenseDal.GetExpenseMasterById | Library.DAL/MasterSetup_DAL/ExpenseDal.cs:144 | sp_Get_MonthlyExpenseEmpWiseMaster_Final | @EmpId, @Month, @Year |
| ExpenseDal.GetExpenseMasterTotalById | Library.DAL/MasterSetup_DAL/ExpenseDal.cs:168 | sp_Get_MonthlyExpenseEmpWiseTotal | @EmpId, @Month, @Year |
| ExpenseDal.Get_EmpAllawance | Library.DAL/MasterSetup_DAL/ExpenseDal.cs:77 | sp_Webapi_Get_EmpAllawance_MonthYear | @EmpInfoId, @Month, @year |
| ExpenseDal.GetGet_TourPlanBalanceMasterById | Library.DAL/MasterSetup_DAL/ExpenseDal.cs:101 | sp_Webapi_Get_TourPlanBalanceEmp | @EmpInfoId, @Month, @year |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_MonthlyExpenseEmpWiseMaster_Final | ExpenseDal.GetExpenseMasterById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MonthlyExpenseEmpWiseTotal | ExpenseDal.GetExpenseMasterTotalById | body NOT in repository | read-like name (INFERRED) |
| sp_Webapi_Get_EmpAllawance_MonthYear | ExpenseDal.Get_EmpAllawance | body NOT in repository | read-like name (INFERRED) |
| sp_Webapi_Get_TourPlanBalanceEmp | ExpenseDal.GetGet_TourPlanBalanceMasterById | body NOT in repository | read-like name (INFERRED) |

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
- try/catch present in 1 of 5 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
sp_Webapi_Get_EmpAllawance_MonthYear, sp_Webapi_Get_TourPlanBalanceEmp

## 25. Reports
crpEmpExpense.rpt

## 26. Dependencies
BLL: —; DAL: ExpenseDal; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx`, `Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 6 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_RPTVIEW/EmployeeExpenseReportViewer.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ExpenseDal.GetExpenseMasterById, ExpenseDal.GetExpenseMasterTotalById, ExpenseDal.GetGet_TourPlanBalanceMasterById, ExpenseDal.Get_EmpAllawance affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
