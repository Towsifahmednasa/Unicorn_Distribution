---
page_id: PAGE-697
module: M18
page: DCPCVPApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-697 — DCPCVPApproval

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs`, `CustomerInfoDAL` methods and `DataAccessManager` write path were read. Stored-procedure bodies (`sp_Get_DCPCVPTourPlanApp`, `sp_webapi_SaveVisitPlanAppLog`, `sp_Get_EmployeeFieldForceInfo_EmpId`, `sp_GET_ActionStatusList`, `sp_GET_MainPermissionByUserRoleandPageUrl`) are NOT in the repository, so table/column effects are `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-697 |
| Module | M18 — Approval Workflow |
| Page name | DCPCVPApproval (heading "DCP/CVP List") |
| Kind | Approval list (role-chain approve/reject of a monthly DCP/CVP plan) |
| Web folder | Approval_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Approval_UI_DCPCVPApproval (`Solution.Web/Approval_UI/DCPCVPApproval.aspx.cs:14`) |
| aspx size / code-behind LOC | 17921 bytes / 596 |
| Suspected legacy/duplicate | Clone of TourPlanApproval (PAGE-706): same grid keys (`TourPlanApprovalId`), same filters, `resetBtn_Click` redirects to `TourPlanApproval.aspx` (`DCPCVPApproval.aspx.cs:590`). CONFIRMED |

## 2. Purpose
Lets a sales-hierarchy approver (AM / DZSM / NSM and admin-type roles) review monthly "DCP/CVP" (doctor/chemist call plan / visit plan, expanded by the field-force app) submitted by field employees, and approve/verify or reject each plan, singly or in bulk. Each action appends one step to the plan's approval log. INFERRED from heading `DCP/CVP List`, DAL `SaveVisitTourPlan_ApplogDAL`, SP name `sp_webapi_SaveVisitPlanAppLog`, `Type="DCP"`.

## 3. Business Objective
Give managers control over the doctor/customer visit plan of their subordinates before the month starts: only the approver whose role is next in the configured chain may act; other roles see "Waiting for Another Approver". INFERRED (chain configuration lives in `ApprovalStepMap`, see WF-M18-01).

## 4. Navigation
Menu placement is DB-driven (`sp_GET_MainMenuRole2`); path `UNKNOWN`. Page-level menu permission is re-checked in code (BR-M18-011). Outbound: `../DoctorVisit_UI/DoctorPlanDetailsView.aspx?id={TableId}` opened in a new window by the eye button (`DCPCVPApproval.aspx.cs:208`); `DoctorEntry.aspx` (hidden "New Entry" button, `:452`); `TourPlanApproval.aspx` (Reset, `:590`). Inbound: none found.

## 5. Route
`~/Approval_UI/DCPCVPApproval.aspx` (file route).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Approval_UI/DCPCVPApproval.aspx |
| Code-behind | Solution.Web/Approval_UI/DCPCVPApproval.aspx.cs |
| DAL | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs (`GetDCPCVPTourPlan_ApplogDAL` :512, `SaveVisitTourPlan_ApplogDAL` :1007), Library.DAL/DoctorModule_DAL/CommonDataLoad.cs (`GetPermissionForUserRole` :18, `GetEmpMarketStructure_Active` :2061, `GetEmployeeList_Active`), SeedDataDAL (`GetDistributionCenterDataTableList`, `GetApprovalStatusList` :868), Setup2DAL (`Get_UserRoleInfo`) |
| DAO | Library.DAO/MasterSetup_DAO/CustMasterInfoDAO.cs (`OrderSaveApprovalLogDAO`), `ResultInfo` |
| Data access core | Library.DAL/DataManager/DataAccessManager.cs (`SaveData` :491, tx begin :436, commit in `SqlConnectionClose` :122) |

## 7. UI Control Inventory
| Control | Type | Label | Required | Default | Read-only/visible | Data source | Error text |
|---|---|---|---|---|---|---|---|
| ddlDistributionCenter | DropDownList | Distribution Center | no | "Please Select From List" | Whole filter row is `runat=server visible=false` (aspx:76) so never rendered; for RoleTypeName "DIC" code preselects `DICCompanyUnitId` and disables (`.cs:50-54`) | `sp_GET_DistributionCenter` (ComUnitId/ComUnitName) | none |
| ddlmonth | DropDownList | Month | no | current month | — | 12 entries starting at current month (`GetMonthList` :125-138; names wrap into next year but Value is month number) | none |
| ddlYear | DropDownList | Year | no | current year | — | 2015..2050 (`GetYearList` :110-124) | none |
| EmployeeIdSelect | DropDownList | Employee | no | blank | — | `sp_CS_GetEmpGeneralInfo_All` (EmpInfoId/EmpName) | none |
| UserRoleSelect | DropDownList | User Role | no | blank | — | `sp_Get_UserRoleInfo` (UserRoleID/RoleName) | none |
| ApprovalStatusSelect | DropDownList | Approval Status | no | blank | — | `sp_GET_ActionStatusList` (SoftwareUseId/WebShow) | none |
| btnSearch / resetBtn | LinkButton | Search / Reset | — | — | — | `LoadData()` / redirect to `TourPlanApproval.aspx` | — |
| btnBulkApprove / btnBulkReject | LinkButton | Approve Selected / Reject Selected | — | — | client `confirm('Are you sure you want to approve/reject all selected records?')` | — | see §18 |
| loadGridView | GridView | DCP/CVP List | — | — | keys `TableId,TourPlanApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep` (aspx:217); columns SL, checkbox, Employee ID (EmpMasterCode), Employee Name, Designation, User Role (RoleName), Year (YearValue), Month (MonthValue), Remarks (FinalSubmitRemarks), Approval Status (ApprovalStatusWeb), Waiting For (WaitingForRole), Actions | `sp_Get_DCPCVPTourPlanApp` | none |
| lbEdit / lbApprove / lbReject | LinkButton (row) | eye / check / x | — | — | Set `Visible=false` for all three and `lbMsg`="Waiting for Another Approver" (badge bg-warning) when `hfToRoleTypeId != Session RoleTypeId` and the user's RoleTypeId is not in {5,4,14,9} (`.cs:546-577`) | — | "Waiting for Another Approver" |
| hf* (hfCustomerMasterId=TableId, hfFromEmpId, hfToEmpId, hfStep, hfRoleTypeId, hfCustomerApprovalId=TourPlanApprovalId, hfToRoleTypeId) | HiddenField (row) | — | — | bound from list SP row | posted back | — | — |
| hfEmpGroupId/RegionId/AreaId/TerritoryId | HiddenField (page) | — | — | filled in `LoadData` from `sp_Get_EmployeeFieldForceInfo_EmpId` row 0 (`.cs:466-469`) | — | — | — |
| EmpCetegoryAddImageButton | LinkButton | New Entry | — | — | `Visible=false` in markup | — | — |

Missing: no comment/remark textbox exists; `Comments` is never populated (CONFIRMED by absence in code-behind).

## 8. Field Mapping
| UI source | DTO property (`OrderSaveApprovalLogDAO`) | Handler | DAL | SP param | Table.column |
|---|---|---|---|---|---|
| hfCustomerApprovalId | OrderApprovalId | RowCommand `ApproveData`/`RejectData` :225/:275; bulk :331/:395 | `CustomerInfoDAL.SaveVisitTourPlan_ApplogDAL` :1007 | `@VisitPlanApprovalId` | UNKNOWN (SP body absent) |
| hfCustomerMasterId (TableId) | TableId | same | same | `@TableId` | UNKNOWN |
| hfEmpGroupId/Region/Area/Territory | GroupId/RegionId/AreaId/TerritoryId | same | same | `@GroupId,@RegionId,@AreaId,@TerritoryId` | UNKNOWN |
| Session["EmpInfoId"] | FromEmpId | same | same | `@FromEmpId` | UNKNOWN |
| hfToEmpId | ToEmpId | same | same | `@ToEmpId` | UNKNOWN |
| hfStep + 1 | Step | :236-237 | same | `@Step` | UNKNOWN |
| literal | Type = "DCP" on approve (:238, bulk :342); "Leave" on reject (:286, bulk :406) | same | same | `@Type` | UNKNOWN |
| literal | Status = "Verified" / "Rejected" | :215 / :264 | same | `@Status` | UNKNOWN |
| DateTime.Now | Date, EntryDateS, ApproveDateS | same | same | `@Date,@EntryDateS,@ApproveDateS` | UNKNOWN |
| Session["UserId"] | EntryByS, EntryByApp | same | same | `@EntryByS,@EntryByApp` | UNKNOWN |
| literal | MenuId = 377 | :245/:293/:349/:413 | same | `@MenuId` | UNKNOWN |
| (not set) | Comments, ToGroupId..ToTerritoryId, EntryTime*, Approve*By*, ApproveTime*, EntryDateApp, ApproveDateApp | — | passed as `DBNull` | `@Comments,@To*,...` | UNKNOWN |

## 9. Dropdown Mapping
See §7 (loaders are `try{...}catch{}` with silent failure: an SP error leaves an empty dropdown with no message, `.cs:59,162,178,195`). Filter values become raw SQL text (§18).

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB change |
|---|---|---|---|---|
| Search | `btnSearch_Click` → `LoadData` (:592) | `GetDCPCVPTourPlan_ApplogDAL` | `sp_Get_DCPCVPTourPlanApp(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId)` | read |
| Reset | `resetBtn_Click` :588 → `Response.Redirect("TourPlanApproval.aspx")` | — | — | none |
| Row check | `RowCommand` `ApproveData` :213 | `SaveVisitTourPlan_ApplogDAL` | `sp_webapi_SaveVisitPlanAppLog` | writes approval-log step (UNKNOWN) |
| Row x | `RowCommand` `RejectData` :262 | same | same | same |
| Row eye | `RowCommand` `EditData` :202 | — | — | none (opens `DoctorPlanDetailsView.aspx?id=TableId`) |
| Approve Selected | `btnBulkApprove_Click` :311 | same | same | one SP call per ticked visible row |
| Reject Selected | `btnBulkReject_Click` :375 | same | same | same |

## 11. Frontend Flow
Page init `pageLoad()` wires pickadate/select2 (aspx:50+). `SelectAllRows(this)` toggles all checkboxes via DataTables API or plain DOM fallback (aspx:31-48). Bulk buttons use inline `confirm()`. All actions are ASP.NET postbacks; no AJAX/WebMethods. Success/failure notification via `ShowSuccesalert(msg,'Success')` / `faildalert(msg,'Faild')` (JS helpers defined outside this page; UNKNOWN location).

## 12. Backend Flow
`Page_Load` (:28-77): reads Session `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (NullReference/any exception falls to empty catch — no redirect, page stays blank, :73-76). First load: `UserPersmissionValidation` (:79) → dropdowns → `LoadInitialInfo` → month/year → `LoadData`.
`LoadData` (:455-580): (1) `sp_Get_EmployeeFieldForceInfo_EmpId(@id=EmpInfoId)`; guard `EmpInfoId != "" || EmpInfoId != null` is always true (:461). Row 0 populates hierarchy hidden fields (try/catch swallow). (2) switch on `RoleTypeName`: `AM` → `Role="AM"`, `pram=" AND View_Webapi_EmployeeFieldForceInfo_Top1.EmpAreaId=<id>"`; `DZSM` → `View_Webapi_EmployeeFieldForceInfo.EmpRegionId`; `NSM` → `...EmpGroupId`; default no scope (:473-501). Note `dtMarket.Rows[0]` is dereferenced unguarded in the switch → exception if employee has no market row (goes to caller catch; page loads blank). (3) append year/month/status/role/employee predicates (:503-533). (4) `GetDCPCVPTourPlan_ApplogDAL(pram,Role,null,null,null,null)`. (5) bind grid; (6) gating loop (:546-577).
Approve/reject: build `OrderSaveApprovalLogDAO`, call `SaveVisitTourPlan_ApplogDAL` (:1007-1069): `SqlConnectionOpen` → `SaveData("sp_webapi_SaveVisitPlanAppLog")` (opens implicit transaction :436, executes `ExecuteNonQuery`, always returns `true`) → `SqlConnectionClose()` commits; on exception `SqlConnectionClose(true)` rolls back and rethrows. The page has no try/catch around the save, so an SP error surfaces as an unhandled exception (yellow screen). CONFIRMED.

## 13. API Mapping
None (no `[WebMethod]`, no handlers).

## 14. Service Mapping
No BLL: code-behind calls DAL directly (`static CustomerInfoDAL _DAL`, `:23`).

## 15. Repository Mapping
| DAL method | Source | SP |
|---|---|---|
| CommonDataLoad.GetPermissionForUserRole | CommonDataLoad.cs:18 | sp_GET_MainPermissionByUserRoleandPageUrl(@RoleId=Session UserRoleID,@PageName=normalised URL) |
| CommonDataLoad.GetEmpMarketStructure_Active | :2061 | sp_Get_EmployeeFieldForceInfo_EmpId(@id) |
| CustomerInfoDAL.GetDCPCVPTourPlan_ApplogDAL | CustomerInfoDAL.cs:512 | sp_Get_DCPCVPTourPlanApp |
| CustomerInfoDAL.SaveVisitTourPlan_ApplogDAL | :1007 | sp_webapi_SaveVisitPlanAppLog (31 parameters) |
| SeedDataDAL.GetApprovalStatusList | SeedDataDAL.cs:868 | sp_GET_ActionStatusList |
| SeedDataDAL.GetDistributionCenterDataTableList, CommonDataLoad.GetEmployeeList_Active, Setup2DAL.Get_UserRoleInfo | — | sp_GET_DistributionCenter, sp_CS_GetEmpGeneralInfo_All, sp_Get_UserRoleInfo |

## 16. Stored Procedure Mapping
| SP | Direction | Body | Known |
|---|---|---|---|
| sp_Get_DCPCVPTourPlanApp | read | NOT in repo | must return TableId, TourPlanApprovalId, FromEmpId, ToEmpId, Step, RoleTypeId, ToRoleTypeId, MaxStep, EmpMasterCode, EmpName, DesigName, RoleName, YearValue, MonthValue, FinalSubmitRemarks, ApprovalStatusWeb, WaitingForRole (from grid bindings). `@param` is appended SQL (pattern verified for sibling `sp_Get_CustomerApp`; here INFERRED) |
| sp_webapi_SaveVisitPlanAppLog | write | NOT in repo | UNKNOWN (log insert + parent status update presumed, INFERRED from sibling naming) |
| sp_Get_EmployeeFieldForceInfo_EmpId, sp_GET_ActionStatusList, sp_GET_MainPermissionByUserRoleandPageUrl, sp_GET_DistributionCenter, sp_CS_GetEmpGeneralInfo_All, sp_Get_UserRoleInfo | read | NOT in repo | UNKNOWN |

## 17. Database Mapping
Tables: all `UNKNOWN` (no inline SQL). Names visible in filters: `mas` (plan master: `YearValue`, `MonthValue`, `ApprovalStatus`, `EmpInfoId`), `us` (user: `UserRoleID`), view `View_Webapi_EmployeeFieldForceInfo[_Top1]` (EmpAreaId/EmpRegionId/EmpGroupId). CONFIRMED as text in `.cs:477-532`; underlying table names UNKNOWN.

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Bulk approve/reject | at least one ticked row whose action button is Visible, else "No records were approved/rejected. Please select records and ensure you have permission to approve/reject them." | server (`.cs:364-372`, `:428-436`) | verbatim | BR-M18-010 |
| Row approve | none server-side; role gating only by hiding buttons (viewstate bypass possible: RowCommand does not re-check the role) | UI only | — | BR-M18-002 |
| Single approve failure | `Res.isSuccess != true` → "Already Exist!" (reachable only via exception path, because `SaveData` returns true unless it throws) | `.cs:254,303` | "Already Exist!" | BR-M18-013 |
| Filters (year, month, status, role, employee) | concatenated into SQL fragment; no validation | — | — | GAP-M18-007 |

## 19. Business Rules
| BR | Description | Source | Implementation | Conf. |
|---|---|---|---|---|
| BR-M18-001 | Next step number = current `Step` (from list row) + 1; page never computes the approver chain itself | DCPCVPApproval.aspx.cs:236-237 | code-behind | CONFIRMED |
| BR-M18-002 | Approve/Reject/View buttons are shown only when the row's `ToRoleTypeId` equals the user's `Session["RoleTypeId"]`, otherwise the row shows badge "Waiting for Another Approver" | :552-577 | code-behind (UI only) | CONFIRMED |
| BR-M18-003 | Users with RoleTypeId 5, 4, 14 or 9 skip the gate and see actions on every row | :546 | code-behind | CONFIRMED |
| BR-M18-005 | `MenuId` sent to the log SP is hard-coded 377 for this page | :245,:293,:349,:413 | code-behind | CONFIRMED (meaning INFERRED: menu id keyed to approval map) |
| BR-M18-006 | Acting user's Group/Region/Area/Territory from `sp_Get_EmployeeFieldForceInfo_EmpId` row 0 is passed into the log row | :463-471 | code-behind | CONFIRMED |
| BR-M18-007 | Visibility scope by role: AM → own area, DZSM → own region, NSM → own group, other roles unrestricted | :473-501 | code-behind + SP `@Role/@param` | CONFIRMED (SP effect UNKNOWN) |
| BR-M18-009 | Log row audit: Date/EntryDateS/ApproveDateS = server now; EntryByS = EntryByApp = Session UserId; FromEmpId = Session EmpInfoId | :234-244 | code-behind | CONFIRMED |
| BR-M18-010 | Bulk actions process only ticked rows whose button is Visible; success/failure counted per row | :317-372 | code-behind | CONFIRMED |
| BR-M18-011 | Page access: unless `Session["UserRoleID"]=="2"`, a permission row for the page URL must exist (`sp_GET_MainPermissionByUserRoleandPageUrl`) else redirect to `../Dashboard_UI/DashboardOne.aspx` | :79-109 | code-behind | CONFIRMED |
| BR-M18-012 | For RoleTypeName "DIC" the DC dropdown is preselected to `DICCompanyUnitId` and disabled (control is hidden anyway) | :50-54 | code-behind | CONFIRMED |
| BR-M18-013 | Save result: `SaveData` returns true unless an exception is thrown, so "success" means "no SQL exception" | DataAccessManager.cs:447-448 | DAL | CONFIRMED |
| BR-M18-014 | Status vocabulary written by this page: "Verified" (approve) and "Rejected" | :215,:264 | code-behind | CONFIRMED |
| BR-M18-047 | Type written: "DCP" on approve, "Leave" on reject (inconsistent copy-paste) | :238,:286,:342,:406 | code-behind | CONFIRMED |
| BR-M18-048 | Reset returns to `TourPlanApproval.aspx`, not this page | :590 | code-behind | CONFIRMED |
| BR-M18-049 | Default filters: current year and current month | :121,:137 | code-behind | CONFIRMED |

## 20. Permission Matrix
Enforced in code: menu-permission row check unless UserRoleID==2 (BR-M18-011); approver gating by RoleTypeId (UI only, BR-M18-002/003). Not enforced: RowCommand/bulk handlers do not re-validate role, step or ownership server-side; any authenticated posted row index acts on `Rows[i]` hidden values. No anonymous check on this page other than Session reads (missing Session → exception swallowed → blank page).

## 21. Status Lifecycle
Strings sent: `Verified` → intermediate; `Rejected` → terminal (INFERRED). Numeric/parent status on the plan header (`mas.ApprovalStatus`) is changed inside the SP — UNKNOWN. Step counter increments by one per action (BR-M18-001). Transition table: [Pending, Step n] --approve (role = ToRoleTypeId)--> [Verified, Step n+1] (INFERRED; next ToRoleTypeId chosen by SP, UNKNOWN); --reject--> [Rejected, Step n+1].

## 22. Error Handling
Dropdown loaders and market read: silent catches. Save: no page-level catch (unhandled). Global `Application_Error` empty (no central logging).

## 23. Audit
Log row fields listed in BR-M18-009; times (`EntryTimeS`, `ApproveTimeS`) not set (null). No before/after values, no comments.

## 24. Integration
Shares SP naming (`sp_webapi_*`) with the field-force mobile/web API (INFERRED; consumers not in repo). `DCRList.aspx.cs:118` in DoctorVisit_UI uses the same log pattern for DCR.

## 25. Reports
None.

## 26. Dependencies
DAL: CustomerInfoDAL, CommonDataLoad, SeedDataDAL, Setup2DAL; DAO: OrderSaveApprovalLogDAO, ResultInfo; view page `DoctorVisit_UI/DoctorPlanDetailsView.aspx`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Log in as AM whose RoleTypeId equals a row's ToRoleTypeId; approve | SP called with Status "Verified", Step+1, MenuId 377; grid reloads | BR-M18-001,005,014 |
| TC-02 | Log in as role not in {5,4,14,9} and not the row's ToRoleTypeId | Row shows "Waiting for Another Approver", no buttons | BR-M18-002 |
| TC-03 | Role 5 user | Buttons on all rows | BR-M18-003 |
| TC-04 | Reject a row | `Type="Leave"` sent (defect check), Status "Rejected" | BR-M18-047 |
| TC-05 | Bulk approve with none ticked | error alert text per §18 | BR-M18-010 |
| TC-06 | Bulk approve, one row causes SP exception | unhandled exception (no catch) — verify behaviour | BR-M18-013 |
| TC-07 | Click Reset | navigates to TourPlanApproval.aspx | BR-M18-048 |
| TC-08 | User without menu permission, UserRoleID != 2 | redirect to DashboardOne | BR-M18-011 |
| TC-09 | Employee with no market row, role AM | exception in `LoadData` (`Rows[0]`) | BR-M18-007 |
| TC-10 | Post crafted RowCommand for a row waiting on another role | server accepts (no re-check) — negative security test | BR-M18-002 |

## 28. Reverse Engineering References
`Solution.Web/Approval_UI/DCPCVPApproval.aspx(.cs)`, `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:512,1007`, `Library.DAL/DataManager/DataAccessManager.cs:407-504`, WF-M18-01, WF-M18-02.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | Actions per §12 | UNKNOWN | MISSING | — |
| Role gating | None existed | UI-only hiding | UNKNOWN | UNVERIFIED-SERVER | BR-M18-002 |
| Type discriminator | None existed | "DCP" approve / "Leave" reject | UNKNOWN | CODE SMELL | BR-M18-047 |
| Reset link | None existed | goes to TourPlanApproval | n/a | CODE SMELL | BR-M18-048 |

## 30. Known Gaps
GAP-M18-001 (SP bodies), GAP-M18-003 (server-side role check missing), GAP-M18-004 (Type inconsistency), GAP-M18-005 (dead "Already Exist!" branch), GAP-M18-007 (SQL fragment concatenation), GAP-M18-008 (unguarded `Rows[0]`), GAP-M18-011 (Reset/copy-paste), GAP-M18-013 (no comments/audit). See `specs/approval-workflow/GAPS-M18.md`.

## 31. Change Impact
`CustomerInfoDAL.SaveVisitTourPlan_ApplogDAL` is used only by this page (grep verified). `GetDCPCVPTourPlan_ApplogDAL` only here. `sp_webapi_SaveVisitPlanAppLog` may also be called by the mobile API (UNKNOWN). Shared helpers (`GetPermissionForUserRole`, `GetEmpMarketStructure_Active`) affect every approval page. Sibling clone: PAGE-706 TourPlanApproval.
