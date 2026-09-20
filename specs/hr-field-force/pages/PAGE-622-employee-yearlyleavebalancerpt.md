---
page_id: PAGE-622
module: M15
page: Employee_YearlyLeaveBalanceRpt
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-622 — Employee_YearlyLeaveBalanceRpt

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-622 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | Employee_YearlyLeaveBalanceRpt |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Reports_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Reports_UI_Employee_YearlyLeaveBalanceRpt |
| aspx size | 24237 bytes |
| Code-behind LOC | 484 |
| Inline script size | 4079 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **Employee_YearlyLeaveBalanceRpt** (Report / Dashboard). Page title/heading text: `Day Wise Leave`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerentry.aspx`, `groupwisepromoqtyentry.aspx`

## 5. Route
`~/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx |
| Code-behind | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/2.0.5/FileSaver.min.js |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| rbReportTypeName | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbReportTypeName_SelectedIndexChanged |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlMIO | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | OnClick→resetBtn_Click |
| export-btn | button |  | no validator found |  |  |  |  |
| btnExportToExcel | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| (no id) | a |  | no validator found |  |  |  |  |
| lblEmpId | Label |  | no validator found |  |  |  |  |
| lblEmpName | Label |  | no validator found |  |  |  |  |
| lbluR | Label |  | no validator found |  |  |  |  |
| ElliCasual | Label |  | no validator found |  |  |  |  |
| abCasual | Label |  | no validator found |  |  |  |  |
| TKCasual | Label |  | no validator found |  |  |  |  |
| CasualBlnc | Label |  | no validator found |  |  |  |  |
| ElliSick | Label |  | no validator found |  |  |  |  |
| abSick | Label |  | no validator found |  |  |  |  |
| TKSick | Label |  | no validator found |  |  |  |  |
| SickBlnc | Label |  | no validator found |  |  |  |  |
| ElliAnnual | Label |  | no validator found |  |  |  |  |
| abAnnual | Label |  | no validator found |  |  |  |  |
| PreviousAL | Label |  | no validator found |  |  |  |  |
| TKAnnual | Label |  | no validator found |  |  |  |  |
| AnnualBlnc | Label |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Employee ID | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | BoundField | Leave From Date | no validator found |  |  |  |  |
| (no id) | BoundField | Leave To Date | no validator found |  |  |  |  |
| (no id) | BoundField | Day(s) | no validator found |  |  |  |  |
| (no id) | BoundField | Leave Type | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| gv_Summary | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Emp. ID | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Name of the Employee | no validator found |  |  |  |  |
| (no id) | BoundField | Desig. | no validator found |  |  |  |  |
| (no id) | BoundField | Base HQ. | no validator found |  |  |  |  |
| (no id) | BoundField | DoJ | no validator found |  |  |  |  |
| (no id) | BoundField | LoS [Y-M-D] | no validator found |  |  |  |  |
| (no id) | BoundField | Month Count | no validator found |  |  |  |  |
| (no id) | BoundField | Previous Leave (AL) | no validator found |  |  |  |  |
| (no id) | BoundField |  Casual Leave | no validator found |  |  |  |  |
| (no id) | BoundField | Sick Leave | no validator found |  |  |  |  |
| (no id) | BoundField | Annual Leave | no validator found |  |  |  |  |
| (no id) | BoundField | Annual Leave | no validator found |  |  |  |  |
| (no id) | BoundField | Eligible (AL) | no validator found |  |  |  |  |
| (no id) | BoundField | Balance (AL) | no validator found |  |  |  |  |
| (no id) | BoundField | Annual Leave | no validator found |  |  |  |  |
| (no id) | BoundField | Leave Encash | no validator found |  |  |  |  |
| (no id) | BoundField | Accumulated Leave (AL) | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlMIO | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlMIO | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm3 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlMIO | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlMIO | param | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ_NoDate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | SubTi | Reports_UI_Employee_YearlyLeaveBalanceRpt.btnExport_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | title | Reports_UI_Employee_YearlyLeaveBalanceRpt.btnExport_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbReportTypeName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlMIO | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSearch | LinkButton click | (postback) | Reports_UI_Employee_YearlyLeaveBalanceRpt.btnSearch_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:233 | — | CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList, CmnCrystaltoView.GetEmployee_YearlySummaryLi | sp_Webapi_LeaveReport_Details, sp_Webapi_LeaveReport_Summary | SP executed — effect on tables UNKNOWN (body not in repo) |
| resetBtn | LinkButton click | (postback) | Reports_UI_Employee_YearlyLeaveBalanceRpt.resetBtn_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:381 | — | — | — | read-only / none detected |
| btnExportToExcel | LinkButton click | (postback) | Reports_UI_Employee_YearlyLeaveBalanceRpt.btnExport_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:387 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad, processPrint
- jQuery/DOM bindings: #btnExport:click
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Reports_UI_Employee_YearlyLeaveBalanceRpt.Page_Load | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:24 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.Page_Load → CommonDataLoad.GetEmployeeList_All → SP: sp_CS_GetEmpGeneralInfo_All | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.LoadInitialInfo | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:34 |  | Reports_UI_Employee_YearlyLeaveBalanceRpt.LoadInitialInfo → CommonDataLoad.GetEmployeeList_All → SP: sp_CS_GetEmpGeneralInfo_All | yes | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.gv_DocumentUpload_PreRender | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:104 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.gv_DocumentUpload_PreRender | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.EmpCetegoryAddImageButton_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:115 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.EmpCetegoryAddImageButton_Click | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.LoadData | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:121 | string parm, string par2, string _year | Reports_UI_Employee_YearlyLeaveBalanceRpt.LoadData → CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList → SP: sp_Webapi_LeaveReport_Details | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.LoadSummaryData | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:182 | string parm, string par2, string Year | Reports_UI_Employee_YearlyLeaveBalanceRpt.LoadSummaryData → CmnCrystaltoView.GetEmployee_YearlySummaryList → SP: sp_Webapi_LeaveReport_Summary | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.rbReportTypeName_SelectedIndexChanged | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:201 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.rbReportTypeName_SelectedIndexChanged | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.loadGridView_RowCommand | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:220 | object sender, GridViewCommandEventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.loadGridView_RowCommand | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.btnSearch_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:233 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.btnSearch_Click → CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList / CmnCrystaltoView.GetEmployee_YearlySummaryList → SP: sp_Webapi_LeaveReport_Details, sp_Webapi_LeaveReport_Summary | yes | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:337 |  | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ_NoDate | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:362 |  | Reports_UI_Employee_YearlyLeaveBalanceRpt.Parm_Summ_NoDate | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.resetBtn_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:381 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.resetBtn_Click | no | no |
| Reports_UI_Employee_YearlyLeaveBalanceRpt.btnExport_Click | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs:387 | object sender, EventArgs e | Reports_UI_Employee_YearlyLeaveBalanceRpt.btnExport_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetEmployeeList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1761 | sp_CS_GetEmpGeneralInfo_All |  |
| CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1775 | sp_Webapi_LeaveReport_Details | @Parm, @Parm2, @Year |
| CmnCrystaltoView.GetEmployee_YearlySummaryList | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1802 | sp_Webapi_LeaveReport_Summary | @Parm, @Parm2, @Year |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_All | body NOT in repository | read-like name (INFERRED) |
| sp_Webapi_LeaveReport_Details | CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList | body NOT in repository | read-like name (INFERRED) |
| sp_Webapi_LeaveReport_Summary | CmnCrystaltoView.GetEmployee_YearlySummaryList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler LoadData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Year && Employee !! | UNKNOWN |
| (handler LoadSummaryData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Year !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 20 methods.
- Redirect targets: CustomerEntry.aspx, Employee_YearlyLeaveBalanceRpt.aspx
- User messages: Please Select From List; Please Select Year !!; Please Select Year && Employee !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
sp_Webapi_LeaveReport_Details, sp_Webapi_LeaveReport_Summary

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CmnCrystaltoView, CommonDataLoad; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnSearch with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnSearch with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click resetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click resetBtn with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click btnExportToExcel with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnExportToExcel with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx`, `Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 62 controls parsed | n/a | PARTIAL | Solution.Web/Reports_UI/Employee_YearlyLeaveBalanceRpt.aspx |
| Data access | This document §15–17 | 3 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 3 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-548, PAGE-549, PAGE-555, PAGE-557, PAGE-562, PAGE-563, PAGE-564, PAGE-570, PAGE-571, PAGE-578, PAGE-582, PAGE-587, PAGE-588, PAGE-610, PAGE-616 …
- Changing DAL methods CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList, CmnCrystaltoView.GetEmployee_YearlySummaryList, CommonDataLoad.GetEmployeeList_All affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
