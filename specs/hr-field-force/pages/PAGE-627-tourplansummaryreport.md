---
page_id: PAGE-627
module: M15
page: TourPlanSummaryReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-627 — TourPlanSummaryReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-627 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | TourPlanSummaryReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Reports_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Reports_TourPlanSummaryReport |
| aspx size | 21391 bytes |
| Code-behind LOC | 383 |
| Inline script size | 10215 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TourPlanSummaryReport** (Report / Dashboard). Page title/heading text: `  Tour Plan Summary Report`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `employeeexpensereportviewer.aspx`, `empmonthlyexpenserpt.aspx`, `mileageclaim.aspx`

## 5. Route
`~/Reports_UI/TourPlanSummaryReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx |
| Code-behind | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | EmployeeInformationDaL (Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| EmployeeIdSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| UserRoleSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlEmployeeStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | SL | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Role | no validator found |  |  |  |  |
| (no id) | BoundField | Day of HQ | no validator found |  |  |  |  |
| (no id) | BoundField | No of Ex. HQ | no validator found |  |  |  |  |
| (no id) | BoundField | No of OS | no validator found |  |  |  |  |
| (no id) | BoundField | No of OS-DCC | no validator found |  |  |  |  |
| (no id) | BoundField | No of Days Work | no validator found |  |  |  |  |
| (no id) | BoundField | No of Days Leave | no validator found |  |  |  |  |
| (no id) | BoundField | #Days Work Team Mate | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlmonth | aDataTable | Reports_TourPlanSummaryReport.LoadData | — | EmployeeInformationDaL.GetTourPlanSummaryReportReport | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | Month | Reports_TourPlanSummaryReport.PopUp | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | Year | Reports_TourPlanSummaryReport.PopUp | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlEmployeeStatus | param | Reports_TourPlanSummaryReport.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | param | Reports_TourPlanSummaryReport.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| UserRoleSelect | param | Reports_TourPlanSummaryReport.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlEmployeeStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: GetApprovalStatusList, GetAreaList, GetEmpList, GetUserRoleInfo, ResetClick, editClick, pageLoad, un
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Reports_TourPlanSummaryReport.Page_Load | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:27 | object sender, EventArgs e | Reports_TourPlanSummaryReport.Page_Load → CommonDataLoad.GetEmployeeList_All / Setup2DAL.Get_UserRoleInfo → SP: sp_CS_GetEmpGeneralInfo_All, sp_Get_UserRoleInfo | no | no |
| Reports_TourPlanSummaryReport.LoadInitialInfo | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:114 |  | Reports_TourPlanSummaryReport.LoadInitialInfo → CommonDataLoad.GetEmployeeList_All / Setup2DAL.Get_UserRoleInfo → SP: sp_CS_GetEmpGeneralInfo_All, sp_Get_UserRoleInfo | yes | no |
| Reports_TourPlanSummaryReport.LoadData | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:160 |  | Reports_TourPlanSummaryReport.LoadData → EmployeeInformationDaL.GetTourPlanSummaryReportReport → SP: sp_Get_TourPlanSummaryReport | no | no |
| Reports_TourPlanSummaryReport.chkSelectAll_CheckedChanged | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:175 | object sender, EventArgs e | Reports_TourPlanSummaryReport.chkSelectAll_CheckedChanged | no | no |
| Reports_TourPlanSummaryReport.btnSearch_Click | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:192 | object sender, EventArgs e | Reports_TourPlanSummaryReport.btnSearch_Click → EmployeeInformationDaL.GetTourPlanSummaryReportReport → SP: sp_Get_TourPlanSummaryReport | no | no |
| Reports_TourPlanSummaryReport.btnExportToExcel_Click | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:227 | object sender, EventArgs e | Reports_TourPlanSummaryReport.btnExportToExcel_Click | no | no |
| Reports_TourPlanSummaryReport.GetMileageClaimList | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:320 | string param | Reports_TourPlanSummaryReport.GetMileageClaimList → Setup2DAL.GetMileageClaimList → SP: sp_Get_MileageClaimList | no | no |
| Reports_TourPlanSummaryReport.gv_DocumentUpload_PreRender | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:329 | object sender, EventArgs e | Reports_TourPlanSummaryReport.gv_DocumentUpload_PreRender | no | no |
| Reports_TourPlanSummaryReport.loadGridView_RowCommand | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:341 | object sender, GridViewCommandEventArgs e | Reports_TourPlanSummaryReport.loadGridView_RowCommand | no | no |
| Reports_TourPlanSummaryReport.resetBtn_Click | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:353 | object sender, EventArgs e | Reports_TourPlanSummaryReport.resetBtn_Click | no | no |
| Reports_TourPlanSummaryReport.btnPrint_OnClick | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:358 | object sender, EventArgs e | Reports_TourPlanSummaryReport.btnPrint_OnClick | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| Reports_TourPlanSummaryReport.GetMileageClaimList | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs:320 | string param | ~/Reports_UI/TourPlanSummaryReport.aspx/GetMileageClaimList | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4727 | sp_Get_UserRoleInfo |  |
| CommonDataLoad.GetEmployeeList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1761 | sp_CS_GetEmpGeneralInfo_All |  |
| EmployeeInformationDaL.GetTourPlanSummaryReportReport | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs:388 | sp_Get_TourPlanSummaryReport | @EmpInfoId, @Month, @UserRoleId, @Year |
| Setup2DAL.GetMileageClaimList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2379 | sp_Get_MileageClaimList | @param |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MileageClaimList | Setup2DAL.GetMileageClaimList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPlanSummaryReport | EmployeeInformationDaL.GetTourPlanSummaryReportReport | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | Setup2DAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation2) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select at least one employee! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 19 methods.
- Redirect targets: ../DoctorModule_UI/MileageClaim.aspx?id=, EmpMonthlyExpenseRpt.aspx
- User messages: Please Select From List; Please Select at least one employee!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, EmployeeInformationDaL, Setup2DAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/Reports_UI/TourPlanSummaryReport.aspx`, `Solution.Web/Reports_UI/TourPlanSummaryReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 32 controls parsed | n/a | PARTIAL | Solution.Web/Reports_UI/TourPlanSummaryReport.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-017, PAGE-548, PAGE-549, PAGE-555, PAGE-557, PAGE-562, PAGE-563, PAGE-564, PAGE-570, PAGE-571, PAGE-578, PAGE-582 …
- Changing DAL methods CommonDataLoad.GetEmployeeList_All, EmployeeInformationDaL.GetTourPlanSummaryReportReport, Setup2DAL.GetMileageClaimList, Setup2DAL.Get_UserRoleInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
