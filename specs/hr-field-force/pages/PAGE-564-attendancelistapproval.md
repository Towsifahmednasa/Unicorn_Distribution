---
page_id: PAGE-564
module: M15
page: AttendanceListApproval
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-564 — AttendanceListApproval

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-564 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | AttendanceListApproval |
| Kind | Approval (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_AttendanceListApproval |
| aspx size | 21170 bytes |
| Code-behind LOC | 628 |
| Inline script size | 3886 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: web approval queue for **field-force attendance submissions** (mobile attendance rows with `AttType`, date, employee): filter by date range, approval status, attendance type, user role, employee, depot; approve, reject, or bulk approve/reject; row edit opens a claim page. Two approval mechanisms coexist (grid buttons via approval log, and two WebMethods) - see BR-M15-050 and GAP-M15-013.

## 3. Business Objective
CONFIRMED by labels/messages: let the next approver in the hierarchy verify attendance (status `Verified`) and the final approver (role type 5) accept it (`Accepted`) or reject, so that attendance is payable/reportable.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerentry.aspx`, `dashboardone.aspx`, `expenseapprovallist.aspx`, `expenseclaim.aspx`

## 5. Route
`~/DoctorModule_UI/AttendanceListApproval.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | AttendanceDAL (Library.DAL/DoctorModule_DAL/AttendanceDAL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | CustomerInfoDAL (Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAO/entity class | AttendanceLog (Library.DAO/MasterSetup_DAO/CustMasterInfoDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlDistributionCenter | DropDownList |  | no validator found |  |  | see §9 |  |
| hfEmpTerrId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpAreaId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpRegionId | HiddenField |  | no validator found |  |  |  |  |
| hfEmpGroupId | HiddenField |  | no validator found |  |  |  |  |
| FromDate | TextBox | Select Date | no validator found |  |  |  |  |
| EmployeeIdSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ToDate | TextBox | Select Date | no validator found |  |  |  |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| UserRoleSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlType | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| btnBulkApprove | LinkButton |  | no validator found |  |  |  | OnClick→btnBulkApprove_Click, OnClientClick→return confirm('Are you sure you want to approve all selected attendance records?'); |
| btnBulkReject | LinkButton |  | no validator found |  |  |  | OnClick→btnBulkReject_Click, OnClientClick→return confirm('Are you sure you want to reject all selected attendance records?'); |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | onclick→SelectAllRows(this); |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Employee ID | no validator found |  |  |  |  |
| (no id) | BoundField | Employee Name | no validator found |  |  |  |  |
| (no id) | BoundField | Designation | no validator found |  |  |  |  |
| (no id) | BoundField | User Role | no validator found |  |  |  |  |
| (no id) | BoundField | Attendance Date | no validator found |  |  |  |  |
| (no id) | BoundField | In/Out Time  | no validator found |  |  |  |  |
| (no id) | BoundField | Expense Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Comments | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | BoundField | Waiting For | no validator found |  | Visible=False |  |  |
| (no id) | TemplateField | Image | no validator found |  |  |  |  |
| hpImg | a |  | no validator found |  |  |  |  |
| imgShow | Image |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Location | no validator found |  |  |  |  |
| (no id) | a | Show in map | no validator found |  |  |  |  |
| (no id) | TemplateField | Actions | no validator found |  |  |  |  |
| hfExpenseClaimID | HiddenField |  | no validator found |  |  |  |  |
| hfCustomerMasterId | HiddenField |  | no validator found |  |  |  |  |
| hfFromEmpId | HiddenField |  | no validator found |  |  |  |  |
| hfToEmpId | HiddenField |  | no validator found |  |  |  |  |
| hfStep | HiddenField |  | no validator found |  |  |  |  |
| hfRoleTypeId | HiddenField |  | no validator found |  |  |  |  |
| hfCustomerApprovalId | HiddenField |  | no validator found |  |  |  |  |
| hfToRoleTypeId | HiddenField |  | no validator found |  |  |  |  |
| lbMsg | Label |  | no validator found |  |  |  |  |
| lbEdit | LinkButton |  | no validator found |  | Visible=false |  |  |
| lbApprove | LinkButton |  | no validator found |  |  |  |  |
| lbReject | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| FromDate | pram | DoctorModule_UI_AttendanceListApproval.LoadData | — | CustomerInfoDAL.GetAttendence_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | pram | DoctorModule_UI_AttendanceListApproval.LoadData | — | CustomerInfoDAL.GetAttendence_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlType | pram | DoctorModule_UI_AttendanceListApproval.LoadData | — | CustomerInfoDAL.GetAttendence_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| UserRoleSelect | pram | DoctorModule_UI_AttendanceListApproval.LoadData | — | CustomerInfoDAL.GetAttendence_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | pram | DoctorModule_UI_AttendanceListApproval.LoadData | — | CustomerInfoDAL.GetAttendence_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerMasterId | AttendanceLog.TableId | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @TableId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpGroupId | AttendanceLog.GroupId | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpRegionId | AttendanceLog.RegionId | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpAreaId | AttendanceLog.AreaId | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpTerrId | AttendanceLog.TerritoryId | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfToEmpId | AttendanceLog.ToEmpId | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @ToEmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStep | InStep | DoctorModule_UI_AttendanceListApproval.loadGridView_RowCommand | — | CustomerInfoDAL.GetAttendence_Applog, CustomerInfoDAL.SaveAttandaceAppLog | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerMasterId | AttendanceLog.TableId | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @TableId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpGroupId | AttendanceLog.GroupId | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpRegionId | AttendanceLog.RegionId | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpAreaId | AttendanceLog.AreaId | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpTerrId | AttendanceLog.TerritoryId | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfToEmpId | AttendanceLog.ToEmpId | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @ToEmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStep | InStep | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | — | CustomerInfoDAL.GetAttendence_Applog, CustomerInfoDAL.SaveAttandaceAppLog | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerMasterId | AttendanceLog.TableId | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @TableId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpGroupId | AttendanceLog.GroupId | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpRegionId | AttendanceLog.RegionId | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpAreaId | AttendanceLog.AreaId | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpTerrId | AttendanceLog.TerritoryId | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfToEmpId | AttendanceLog.ToEmpId | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveAttandaceAppLog | @ToEmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStep | InStep | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | — | CustomerInfoDAL.GetAttendence_Applog, CustomerInfoDAL.SaveAttandaceAppLog | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlDistributionCenter | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnBulkApprove | LinkButton click | return confirm('Are you sure you want to approve all selected attendance records?'); | DoctorModule_UI_AttendanceListApproval.btnBulkApprove_Click | Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx.cs:450 | — | CustomerInfoDAL.GetAttendence_Applog, CustomerInfoDAL.SaveAttandaceAppLog | sp_Get_AttendanceInformation, sp_webapi_SaveAppLog | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnBulkReject | LinkButton click | return confirm('Are you sure you want to reject all selected attendance records?'); | DoctorModule_UI_AttendanceListApproval.btnBulkReject_Click | Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx.cs:522 | — | CustomerInfoDAL.GetAttendence_Applog, CustomerInfoDAL.SaveAttandaceAppLog | sp_Get_AttendanceInformation, sp_webapi_SaveAppLog | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
`Page_Load` (`AttendanceListApproval.aspx.cs:30-124`): reads `RoleTypeName`, `EmpInfoId`, `RoleTypeId` from Session (any failure -> redirect `Dashboard_UI/DashboardOne.aspx`); on first GET runs `UserPersmissionValidation()`, defaults dates to today, fills employees, roles, approval statuses (`sp_GET_ActionStatusList`), depots (DIC users: depot fixed to `Session["DICCompanyUnitId"]` and disabled), then `LoadData()`. Search button -> `LoadData`; reset -> redirect to `ExpenseApprovalList.aspx`. Row commands `EditData/ApproveData/RejectData`; toolbar `btnBulkApprove`, `btnBulkReject`; header select-all via `chkSelect` per row.

## 12. Backend Flow
`LoadData` (`:126-291`): builds `pram` string: date range from `FromDate/ToDate` text (` AND CONVERT(date,tblMarketAttendance_Master_webapi.AttendanceDate) BETWEEN '<from>' AND '<to>'`), status (`...ApprovalStatus='<v>'`), type (`...AttType='<v>'`), role (`us.UserRoleID`), employee (`...EmpInfoId`) -> `CustomerInfoDAL.GetAttendence_Applog(pram, "", null,...)` -> `sp_Get_AttendanceInformation(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId)` -> grid. The market-scope block is commented out (`:130-173`). After binding: unless `RoleTypeId` in {5,4,14,9}, each row whose hidden `hfToRoleTypeId != Session RoleTypeId` gets Edit/Approve/Reject hidden and badge `Waiting for Another Approver` (`:259-288`). Approve (`:343-397`): status `Accepted` if `RoleTypeId==5` else `Verified`; `AttendanceLog` = {ApprovalId 0, TableId=`hfCustomerMasterId`, Group/Region/Area/Territory = hidden fields **never populated -> 0**, FromEmpId=Session EmpInfoId, ToEmpId=`hfToEmpId`, Step=`hfStep+1`, Type=`"Expense"`, Status, Date/EntryDateS/ApproveDateS=now, EntryByS/EntryByApp=UserId, MenuId=356} -> `CustomerInfoDAL.SaveAttandaceAppLog` -> `sp_webapi_SaveAppLog` (`Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:1135`). Reject identical with status `Rejected`. Bulk methods loop ticked rows with visible action links, count success/failure, message `<n> record(s) approved successfully! <m> failed.` / `No records were approved. Please select records and ensure you have permission to approve them.` WebMethods: `Get_AttendanceList_Approval` -> `sp_Get_Emp_AttendanceInfoListPending`; `Approve_AttendanceList(MyArry, rbValue)` -> `sp_ApproveAttendanceInformation(@AttendanceId,@ApprovedBy,@Status)`.

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_AttendanceListApproval.Get_AttendanceList_Approval | Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx.cs:600 |  | ~/DoctorModule_UI/AttendanceListApproval.aspx/Get_AttendanceList_Approval | via BLL |
| DoctorModule_UI_AttendanceListApproval.Approve_AttendanceList | Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx.cs:610 | string MyArry, bool? rbValue | ~/DoctorModule_UI/AttendanceListApproval.aspx/Approve_AttendanceList | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| Setup2DAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4727 | sp_Get_UserRoleInfo |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| SeedDataDAL.GetDistributionCenterDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:669 | sp_GET_DistributionCenter |  |
| CustomerInfoDAL.GetAttendence_Applog | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:612 | sp_Get_AttendanceInformation | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param |
| CommonDataLoad.GetPermissionForUserRole | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:18 | sp_GET_MainPermissionByUserRoleandPageUrl | @PageName, @RoleId |
| CommonDataLoad.NormalizePageUrl | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:89 | delegates / no SQL literal |  |
| CustomerInfoDAL.SaveAttandaceAppLog | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:1136 | sp_webapi_SaveAppLog | @ApprovalId, @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTi |
| AttendanceDAL.Get_AttendanceList_Approval | Library.DAL/DoctorModule_DAL/AttendanceDAL.cs:228 | sp_Get_Emp_AttendanceInfoListPending |  |
| AttendanceDAL.ApprovalAttendanceListInfo | Library.DAL/DoctorModule_DAL/AttendanceDAL.cs:247 | sp_ApproveAttendanceInformation | @ApprovedBy, @AttendanceId, @Status |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_ApproveAttendanceInformation | AttendanceDAL.ApprovalAttendanceListInfo | body NOT in repository | write-like name |
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DistributionCenter | SeedDataDAL.GetDistributionCenterDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MainPermissionByUserRoleandPageUrl | CommonDataLoad.GetPermissionForUserRole | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AttendanceInformation | CustomerInfoDAL.GetAttendence_Applog | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Emp_AttendanceInfoListPending | AttendanceDAL.Get_AttendanceList_Approval | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | Setup2DAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |
| sp_webapi_SaveAppLog | CustomerInfoDAL.SaveAttandaceAppLog | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Session keys | `RoleTypeName`, `EmpInfoId`, `RoleTypeId` must exist | `Page_Load` try/catch | redirect to dashboard | BR-M15-053 |
| Permission | non-admin (`UserRoleID != "2"`) needs `GetPermissionForUserRole(<path>)` row | `:294-326` | redirect to `DashboardOne.aspx` | BR-M15-053 |
| Approve/Reject | only rows where action visible (role/step match) | `:259-288`, bulk `:468-470` | `Waiting for Another Approver` | BR-M15-051 |
| Filters | none validated; concatenated into SQL | `:176-214` | - | GAP-M15-012 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-050 | Approval status written: `Accepted` when approver's `RoleTypeId==5` (final), otherwise `Verified`; reject always `Rejected`; each action appends an approval-log row with `Step = current step + 1`, `FromEmpId` = approver, `ToEmpId` = row's designated approver. | `AttendanceListApproval.aspx.cs:343-445` | code-behind + `sp_webapi_SaveAppLog` | CONFIRMED |
| BR-M15-051 | Action buttons visible for all rows to role types 5, 4, 14, 9; other roles act only on rows addressed to their own `RoleTypeId`; others see `Waiting for Another Approver`. Who is "next" (`ToRoleTypeId/ToEmpId`, `Step`) comes from `sp_Get_AttendanceInformation` (UNKNOWN). | `:259-288` | code-behind + SP | CONFIRMED (UI), UNKNOWN (routing) |
| BR-M15-052 | Bulk approve/reject processes only ticked rows whose Approve/Reject link is visible and reports counts; per-row failures do not stop the loop. | `:449-585` | code-behind | CONFIRMED |
| BR-M15-053 | Access rule: `UserRoleID == 2` bypasses; every other user must have a permission row for this page path or is redirected to the dashboard; the fetched permission flags (`RAdd`, `REdit`) are not applied (commented). | `:294-326` | code-behind + `CommonDataLoad.GetPermissionForUserRole` | CONFIRMED |
| BR-M15-054 | Approval log is written with `Type="Expense"` and `MenuId=356` (expense menu) and zero hierarchy ids - copy of the expense approval page; edit button opens `ExpenseClaim.aspx?id=<hfExpenseClaimID>&Rid=<RoleTypeId>`. | `:338,377-384,425-432` | code-behind | CONFIRMED |

## 20. Permission Matrix
Enforced in code: BR-M15-053 (page-level, first GET only - postbacks re-run only the role/step visibility) and BR-M15-051 (row-level UI hiding; **server does not re-verify the row's `ToRoleTypeId` on approve** - a forged postback/hidden-field value could approve any row, INFERRED). WebMethods `Get_AttendanceList_Approval`/`Approve_AttendanceList` have no permission check.

## 21. Status Lifecycle
Approval-log `Status`: `Verified` (intermediate) -> `Accepted` (RoleTypeId 5) or `Rejected` at any step; `Step` increments by 1 per action. Attendance master `ApprovalStatus` value set from `sp_GET_ActionStatusList` (`SoftwareUseId` / `WebShow`, values UNKNOWN); WebMethod path uses boolean status (`sp_ApproveAttendanceInformation`).

## 22. Error Handling
Session/permission failures redirect; approval failure shows `Already Exist!` (copy-paste); bulk shows counts; DAL rollback+rethrow (unhandled in RowCommand).

## 23. Audit
Approval log stores approver EmpInfoId/UserId, entry/approve timestamps (`EntryDateS`, `ApproveDateS` = server time), Step, Status, Type, MenuId.

## 24. Integration
sp_webapi_SaveAppLog

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: AttendanceDAL, CommonDataLoad, CustomerInfoDAL, SeedDataDAL, Setup2DAL; DAO: AttendanceLog, ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-564-01 | Role 5 approves pending row | Log status `Accepted`, step+1 | BR-M15-050 |
| TC-564-02 | AM approves row addressed to AM | `Verified` | BR-M15-050 |
| TC-564-03 | AM views row addressed to DZSM | Buttons hidden, `Waiting for Another Approver` | BR-M15-051 |
| TC-564-04 | Tick 3 rows, one invalid, Bulk Approve | Counts reported | BR-M15-052 |
| TC-564-05 | Non-admin without permission row | Redirect to dashboard | BR-M15-053 |
| TC-564-06 | Approve then check log Type/MenuId (edge) | `Expense`/356 recorded | BR-M15-054 |
| TC-564-07 | Filter date `'` injection (edge) | Should be rejected; code concatenates | GAP-M15-012 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx`, `Solution.Web/DoctorModule_UI/AttendanceListApproval.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | AttendanceListApproval.aspx(.cs) | n/a | MISSING | - |
| Approval Type/Menu | None existed | Expense/356 | UNKNOWN | ACTUAL-DEFECT | BR-M15-054 |
| Server-side row check | None existed | absent | UNKNOWN | ACTUAL-GAP | GAP-M15-013 |

## 30. Known Gaps
- **GAP-M15-013**, **GAP-M15-012**, **GAP-M15-014**; unused dependencies (`Setup2DAL`, `SeedDataDAL` statics, commented market scoping); reset button goes to another page.

## 31. Change Impact
`sp_webapi_SaveAppLog` and `CustomerInfoDAL` shared by all Approval_UI pages (customer, doctor, order, leave, expense, mileage, DA, tour plan) and mobile approvals. `sp_Get_AttendanceInformation` shared with reports. MenuId 356 collides with expense approval logs.

