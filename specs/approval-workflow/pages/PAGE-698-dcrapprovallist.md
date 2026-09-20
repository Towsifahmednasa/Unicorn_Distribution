---
page_id: PAGE-698
module: M18
page: DCRApprovalList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-698 — DCRApprovalList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-698 |
| Module | M18 — Approval Workflow (INFERRED from folder + file name) |
| Page name | DCRApprovalList |
| Kind | Approval (INFERRED from name) |
| Web folder | Approval_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Approval_UI_DCRApprovalList |
| aspx size | 19638 bytes |
| Code-behind LOC | 520 |
| Inline script size | 3722 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DCRApprovalList** (Approval). Page title/heading text: `DCR List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Approval list pages for customers, doctors, orders, leave, expense, tour plan, DCR, RX and related.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerentry.aspx`, `dashboardone.aspx`, `doctorentry.aspx`, `tourplandetailsview.aspx`

## 5. Route
`~/Approval_UI/DCRApprovalList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Approval_UI/DCRApprovalList.aspx |
| Code-behind | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | CustomerInfoDAL (Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAO/entity class | OrderSaveApprovalLogDAO (Library.DAO/MasterSetup_DAO/CustMasterInfoDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→EmpCetegoryAddImageButton_Click |
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
| UserRoleSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | OnClick→resetBtn_Click |
| btnBulkApprove | LinkButton |  | no validator found |  |  |  | OnClick→btnBulkApprove_Click, OnClientClick→return confirm('Are you sure you want to approve all selected DCR records?'); |
| btnBulkReject | LinkButton |  | no validator found |  |  |  | OnClick→btnBulkReject_Click, OnClientClick→return confirm('Are you sure you want to reject all selected DCR records?'); |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | onclick→SelectAllRows(this); |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Name | no validator found |  |  |  |  |
| (no id) | BoundField | DCR Date | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Person Name | no validator found |  |  |  |  |
| (no id) | BoundField | Visit Type | no validator found |  |  |  |  |
| (no id) | BoundField | Chamber | no validator found |  |  |  |  |
| (no id) | BoundField | Comments | no validator found |  |  |  |  |
| (no id) | TemplateField | Location | no validator found |  |  |  |  |
| (no id) | a | Show in map | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | BoundField | Waiting For | no validator found |  | Visible=False |  |  |
| (no id) | TemplateField | Actions | no validator found |  |  |  |  |
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
| hfCustomerApprovalId | OrderSaveApprovalLogDAO.OrderApprovalId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @DCRApprovalId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfCustomerMasterId | OrderSaveApprovalLogDAO.TableId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @TableId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpGroupId | OrderSaveApprovalLogDAO.GroupId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpRegionId | OrderSaveApprovalLogDAO.RegionId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpAreaId | OrderSaveApprovalLogDAO.AreaId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpTerrId | OrderSaveApprovalLogDAO.TerritoryId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfToEmpId | OrderSaveApprovalLogDAO.ToEmpId | Approval_UI_DCRApprovalList.loadGridView_RowCommand | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @ToEmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStep | InStep | Approval_UI_DCRApprovalList.loadGridView_RowCommand | — | CustomerInfoDAL.GetDCR_Applog, CustomerInfoDAL.SaveDCR_Applog_DAL | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerApprovalId | OrderSaveApprovalLogDAO.OrderApprovalId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @DCRApprovalId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfCustomerMasterId | OrderSaveApprovalLogDAO.TableId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @TableId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpGroupId | OrderSaveApprovalLogDAO.GroupId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpRegionId | OrderSaveApprovalLogDAO.RegionId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpAreaId | OrderSaveApprovalLogDAO.AreaId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpTerrId | OrderSaveApprovalLogDAO.TerritoryId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfToEmpId | OrderSaveApprovalLogDAO.ToEmpId | Approval_UI_DCRApprovalList.btnBulkApprove_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @ToEmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStep | InStep | Approval_UI_DCRApprovalList.btnBulkApprove_Click | — | CustomerInfoDAL.GetDCR_Applog, CustomerInfoDAL.SaveDCR_Applog_DAL | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerApprovalId | OrderSaveApprovalLogDAO.OrderApprovalId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @DCRApprovalId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfCustomerMasterId | OrderSaveApprovalLogDAO.TableId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @TableId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpGroupId | OrderSaveApprovalLogDAO.GroupId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpRegionId | OrderSaveApprovalLogDAO.RegionId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpAreaId | OrderSaveApprovalLogDAO.AreaId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfEmpTerrId | OrderSaveApprovalLogDAO.TerritoryId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfToEmpId | OrderSaveApprovalLogDAO.ToEmpId | Approval_UI_DCRApprovalList.btnBulkReject_Click | (none — page calls DAL directly) | CustomerInfoDAL.SaveDCR_Applog_DAL | @ToEmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfStep | InStep | Approval_UI_DCRApprovalList.btnBulkReject_Click | — | CustomerInfoDAL.GetDCR_Applog, CustomerInfoDAL.SaveDCR_Applog_DAL | UNKNOWN | UNKNOWN | UNKNOWN |
| FromDate | pram | Approval_UI_DCRApprovalList.LoadData | — | CustomerInfoDAL.GetDCR_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | pram | Approval_UI_DCRApprovalList.LoadData | — | CustomerInfoDAL.GetDCR_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| UserRoleSelect | pram | Approval_UI_DCRApprovalList.LoadData | — | CustomerInfoDAL.GetDCR_Applog | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | pram | Approval_UI_DCRApprovalList.LoadData | — | CustomerInfoDAL.GetDCR_Applog | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlDistributionCenter | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | Approval_UI_DCRApprovalList.EmpCetegoryAddImageButton_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:369 | — | — | — | read-only / none detected |
| btnSearch | LinkButton click | (postback) | Approval_UI_DCRApprovalList.btnSearch_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:511 | — | CustomerInfoDAL.GetDCR_Applog | sp_Get_DCRApp | SP executed — effect on tables UNKNOWN (body not in repo) |
| resetBtn | LinkButton click | (postback) | Approval_UI_DCRApprovalList.resetBtn_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:517 | — | — | — | read-only / none detected |
| btnBulkApprove | LinkButton click | return confirm('Are you sure you want to approve all selected DCR records?'); | Approval_UI_DCRApprovalList.btnBulkApprove_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:230 | — | CustomerInfoDAL.GetDCR_Applog, CustomerInfoDAL.SaveDCR_Applog_DAL | sp_Get_DCRApp, sp_webapi_SaveDCRAppLog | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnBulkReject | LinkButton click | return confirm('Are you sure you want to reject all selected DCR records?'); | Approval_UI_DCRApprovalList.btnBulkReject_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:294 | — | CustomerInfoDAL.GetDCR_Applog, CustomerInfoDAL.SaveDCR_Applog_DAL | sp_Get_DCRApp, sp_webapi_SaveDCRAppLog | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- Client script functions in page: SelectAllRows, initDataTable, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Approval_UI_DCRApprovalList.Page_Load | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:27 | object sender, EventArgs e | Approval_UI_DCRApprovalList.Page_Load → CommonDataLoad.GetEmployeeList_Active / CustomerInfoDAL.GetDCR_Applog / SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetDistributionCenterDataTableList / Setup2DAL.Get_UserRoleInfo → SP: sp_CS_GetEmpGeneralInfo_All, s | yes | no |
| Approval_UI_DCRApprovalList.loadGridView_RowCommand | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:119 | object sender, GridViewCommandEventArgs e | Approval_UI_DCRApprovalList.loadGridView_RowCommand → CustomerInfoDAL.GetDCR_Applog / CustomerInfoDAL.SaveDCR_Applog_DAL → SP: sp_Get_DCRApp, sp_webapi_SaveDCRAppLog | no | yes |
| Approval_UI_DCRApprovalList.btnBulkApprove_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:230 | object sender, EventArgs e | Approval_UI_DCRApprovalList.btnBulkApprove_Click → CustomerInfoDAL.GetDCR_Applog / CustomerInfoDAL.SaveDCR_Applog_DAL → SP: sp_Get_DCRApp, sp_webapi_SaveDCRAppLog | no | yes |
| Approval_UI_DCRApprovalList.btnBulkReject_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:294 | object sender, EventArgs e | Approval_UI_DCRApprovalList.btnBulkReject_Click → CustomerInfoDAL.GetDCR_Applog / CustomerInfoDAL.SaveDCR_Applog_DAL → SP: sp_Get_DCRApp, sp_webapi_SaveDCRAppLog | no | yes |
| Approval_UI_DCRApprovalList.gv_DocumentUpload_PreRender | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:358 | object sender, EventArgs e | Approval_UI_DCRApprovalList.gv_DocumentUpload_PreRender | no | no |
| Approval_UI_DCRApprovalList.EmpCetegoryAddImageButton_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:369 | object sender, EventArgs e | Approval_UI_DCRApprovalList.EmpCetegoryAddImageButton_Click | no | no |
| Approval_UI_DCRApprovalList.LoadData | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:374 |  | Approval_UI_DCRApprovalList.LoadData → CustomerInfoDAL.GetDCR_Applog → SP: sp_Get_DCRApp | no | no |
| Approval_UI_DCRApprovalList.btnSearch_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:511 | object sender, EventArgs e | Approval_UI_DCRApprovalList.btnSearch_Click → CustomerInfoDAL.GetDCR_Applog → SP: sp_Get_DCRApp | no | no |
| Approval_UI_DCRApprovalList.resetBtn_Click | Solution.Web/Approval_UI/DCRApprovalList.aspx.cs:517 | object sender, EventArgs e | Approval_UI_DCRApprovalList.resetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| Setup2DAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4727 | sp_Get_UserRoleInfo |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| SeedDataDAL.GetDistributionCenterDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:669 | sp_GET_DistributionCenter |  |
| CustomerInfoDAL.GetDCR_Applog | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:678 | sp_Get_DCRApp | @AppStatus, @AreaId, @EmpId, @FromDt, @GroupId, @Role, @TerritoryId, @ToDt, @ZoneId, @param |
| CustomerInfoDAL.SaveDCR_Applog_DAL | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:1342 | sp_webapi_SaveDCRAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DistributionCenter | SeedDataDAL.GetDistributionCenterDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DCRApp | CustomerInfoDAL.GetDCR_Applog | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | Setup2DAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |
| sp_webapi_SaveDCRAppLog | CustomerInfoDAL.SaveDCR_Applog_DAL | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler btnBulkApprove_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler btnBulkApprove_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | faildalert('No records were approved. Please select records and ensure you have permission to approve them.','Faild'); | UNKNOWN |
| (handler btnBulkReject_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success'); | UNKNOWN |
| (handler btnBulkReject_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | faildalert('No records were rejected. Please select records and ensure you have permission to reject them.','Faild'); | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DICCompanyUnitId, EmpInfoId, RoleTypeId, RoleTypeName, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 10 methods.
- Redirect targets: ../Dashboard_UI/DashboardOne.aspx, DCRApprovalList.aspx, DoctorEntry.aspx
- User messages: ','Success');; Already Exist!; Please Select From List; faildalert('No records were approved. Please select records and ensure you have permission to approve them.','Faild');; faildalert('No records were rejected. Please select records and ensure you have permission to reject them.','Faild');
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
sp_Get_DCRApp, sp_webapi_SaveDCRAppLog

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, CustomerInfoDAL, SeedDataDAL, Setup2DAL; DAO: OrderSaveApprovalLogDAO, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnSearch with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSearch with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click resetBtn with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click resetBtn with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnBulkApprove with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnBulkApprove with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click btnBulkReject with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click btnBulkReject with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/Approval_UI/DCRApprovalList.aspx`, `Solution.Web/Approval_UI/DCRApprovalList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Approval_UI/DCRApprovalList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 48 controls parsed | n/a | PARTIAL | Solution.Web/Approval_UI/DCRApprovalList.aspx |
| Data access | This document §15–17 | 6 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 6 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M18 (Approval Workflow).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-147, PAGE-167, PAGE-168, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-327, PAGE-548, PAGE-549 …
- Changing DAL methods CommonDataLoad.GetEmployeeList_Active, CustomerInfoDAL.GetDCR_Applog, CustomerInfoDAL.SaveDCR_Applog_DAL, SeedDataDAL.GetApprovalStatusList, SeedDataDAL.GetDistributionCenterDataTableList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
