---
page_id: PAGE-210
module: M06
page: OrderTrackingSummary
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-210 — OrderTrackingSummary

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-210 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderTrackingSummary |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_OrderTrackingSummary |
| aspx size | 45501 bytes |
| Code-behind LOC | 615 |
| Inline script size | 10226 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderTrackingSummary** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `customerentry.aspx`, `ordertrackinglist.aspx`

## 5. Route
`~/MasterSetup_UI/OrderTrackingSummary.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/Reports_UI/IVMarketStructureMarket.ascx |
| JS include | ../VerticalAsset/jquery.tabletoCSV.js |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | OrderTrackingDAL (Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  | Visible=false |  | OnClick→EmpCetegoryAddImageButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| rbReportTypeName | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbReportTypeName_SelectedIndexChanged |
| ddlDistributionCenter | DropDownList |  | no validator found |  |  | see §9 |  |
| frmDate | TextBox | Select Date | no validator found |  |  |  |  |
| toDate | TextBox | Select Date | no validator found |  |  |  |  |
| ddlPharmaPlatform | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProgramType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlChemisType | DropDownList |  | no validator found |  |  | see §9 |  |
| ApprovalStatusSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| custNameTextBox | TextBox |  | no validator found |  |  |  | ontextchanged→custNameTextBox_TextChanged |
| at_txt_JobCirculation | AutoCompleteExtender |  | no validator found |  |  |  |  |
| hfCustomerId | HiddenField |  | no validator found |  |  |  |  |
| ddlCampaign | DropDownList |  | no validator found |  |  | see §9 |  |
| chkIsActive | input |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| lblOrderCount | Label | 0 | no validator found |  |  |  |  |
| lblOrderAmount | Label | 0 | no validator found |  |  |  |  |
| lblVAT | Label | 0 | no validator found |  |  |  |  |
| lblDiscount | Label | 0 | no validator found |  |  |  |  |
| lblAllTotal | Label | 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnExport_Click |
| gv_Sum_Product | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Received By | no validator found |  |  |  |  |
| (no id) | BoundField | Order Count | no validator found |  |  |  |  |
| (no id) | BoundField | Product Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Order Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Count | no validator found |  |  |  |  |
| gv_Sum_Chemist | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Received By | no validator found |  |  |  |  |
| (no id) | BoundField | Order Count | no validator found |  |  |  |  |
| (no id) | BoundField | Order Amount | no validator found |  |  |  |  |
| gvOrderMaster | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | BoundField | Order NO | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Center | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Payable | no validator found |  |  |  |  |
| (no id) | BoundField | Create Date | no validator found |  |  |  |  |
| (no id) | BoundField | Create By | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Name | no validator found |  |  |  |  |
| (no id) | BoundField | AM Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Group | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Route | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| gv_OrderDetails | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Order NO | no validator found |  |  |  |  |
| (no id) | BoundField | Order NO | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Center | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Create Date | no validator found |  |  |  |  |
| (no id) | BoundField | Create By | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Name | no validator found |  |  |  |  |
| (no id) | BoundField | AM Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Group | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Route | no validator found |  |  |  |  |
| (no id) | BoundField | Approval Status | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Quantity | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | Total TP | no validator found |  |  |  |  |
| (no id) | BoundField | Unit Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Dis. Percent | no validator found |  |  |  |  |
| (no id) | BoundField | Dis. Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Name | no validator found |  |  |  |  |
| (no id) | BoundField | Gift/Bonus Product | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| custNameTextBox | empName | MasterSetup_UI_OrderTrackingSummary.custNameTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| frmDate | dtDate | MasterSetup_UI_OrderTrackingSummary.LoadData | — | OrderTrackingDAL.GetDatefromMonthYearStuff, OrderTrackingDAL.Get_ALlOrderSummaryByChemist, OrderTrackingDAL.Get_ALlOrder | UNKNOWN | UNKNOWN | UNKNOWN |
| frmDate | dtDate | MasterSetup_UI_OrderTrackingSummary.btnExport_Click | — | CommonDataLoad.GetEmpMarketStructure_Active, OrderTrackingDAL.GetDatefromMonthYearStuff, OrderTrackingDAL.Get_ALlOrderSu | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlDistributionCenter | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlProgramType | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlChemisType | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| hfCustomerId | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| frmDate | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlCampaign | param | MasterSetup_UI_OrderTrackingSummary.Parm | — | CommonDataLoad.GetEmpMarketStructure_Active | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbReportTypeName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlDistributionCenter | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProgramType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlChemisType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ApprovalStatusSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlCampaign | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | MasterSetup_UI_OrderTrackingSummary.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:205 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | MasterSetup_UI_OrderTrackingSummary.btnExport_Click | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:297 | — | CommonDataLoad.GetEmpMarketStructure_Active, OrderTrackingDAL.GetDatefromMonthYearStuff, OrderTracki | DynamicDatebyMonthByDateRange, sp_Get_ALlOrderSummaryByChemist_Pivot, sp_Get_ALlOrderSummaryByProduct, sp_Get_EmployeeFi | SP executed — effect on tables UNKNOWN (body not in repo) |

## 11. Frontend Flow
- Client script functions in page: ExportToPdf, exportTableToExcel, exportToExcel, exporttocsv, pageLoad
- jQuery/DOM bindings: #btngv:click
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_OrderTrackingSummary.Page_Load | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:29 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.Page_Load → SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetCampaignNameListALLFromOrder / SeedDataDAL.GetChemistTypeListALL / SeedDataDAL.GetDistributionCenterDataTableList / SeedDataDAL.GetProgramTypeListAll → SP: sp_GE | yes | no |
| MasterSetup_UI_OrderTrackingSummary.custNameTextBox_TextChanged | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:158 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.custNameTextBox_TextChanged | no | no |
| MasterSetup_UI_OrderTrackingSummary.loadGridView_PageIndexChanging | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:190 | object sender, GridViewPageEventArgs e | MasterSetup_UI_OrderTrackingSummary.loadGridView_PageIndexChanging | no | no |
| MasterSetup_UI_OrderTrackingSummary.gv_DocumentUpload_PreRender | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:194 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.gv_DocumentUpload_PreRender | no | no |
| MasterSetup_UI_OrderTrackingSummary.EmpCetegoryAddImageButton_Click | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:205 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.EmpCetegoryAddImageButton_Click | no | no |
| MasterSetup_UI_OrderTrackingSummary.LoadData | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:210 | string parm | MasterSetup_UI_OrderTrackingSummary.LoadData → OrderTrackingDAL.GetDatefromMonthYearStuff / OrderTrackingDAL.Get_ALlOrderSummaryByChemist / OrderTrackingDAL.Get_ALlOrderSummaryByProduct → SP: DynamicDatebyMonthByDateRange, sp_Get_ALlOrderSummaryByChemist_Pivot | no | no |
| MasterSetup_UI_OrderTrackingSummary.btnExport_Click | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:297 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.btnExport_Click → CommonDataLoad.GetEmpMarketStructure_Active / OrderTrackingDAL.GetDatefromMonthYearStuff / OrderTrackingDAL.Get_ALlOrderSummaryByChemist / OrderTrackingDAL.Get_ALlOrderSummaryByProduct → SP: DynamicDatebyMo | no | no |
| MasterSetup_UI_OrderTrackingSummary.loadGridView_RowCommand | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:424 | object sender, GridViewCommandEventArgs e | MasterSetup_UI_OrderTrackingSummary.loadGridView_RowCommand | no | no |
| MasterSetup_UI_OrderTrackingSummary.btnSearch_Click | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:430 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.btnSearch_Click → CommonDataLoad.GetEmpMarketStructure_Active / OrderTrackingDAL.GetDatefromMonthYearStuff / OrderTrackingDAL.Get_ALlOrderSummaryByChemist / OrderTrackingDAL.Get_ALlOrderSummaryByProduct → SP: DynamicDatebyMo | no | no |
| MasterSetup_UI_OrderTrackingSummary.Parm | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:476 |  | MasterSetup_UI_OrderTrackingSummary.Parm → CommonDataLoad.GetEmpMarketStructure_Active → SP: sp_Get_EmployeeFieldForceInfo_EmpId | no | no |
| MasterSetup_UI_OrderTrackingSummary.resetBtn_Click | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:601 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.resetBtn_Click | no | no |
| MasterSetup_UI_OrderTrackingSummary.rbReportTypeName_SelectedIndexChanged | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs:606 | object sender, EventArgs e | MasterSetup_UI_OrderTrackingSummary.rbReportTypeName_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetChemistTypeListALL | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:97 | sp_GET_CustomerTypeAll |  |
| SeedDataDAL.GetSMCTypeListParam | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:949 | sp_GET_SMCTypeListParm | @Parm |
| SeedDataDAL.GetCampaignNameListALLFromOrder | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:145 | sp_GET_CampaignNameFromOrderDetail |  |
| SeedDataDAL.GetProgramTypeListAll | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:78 | sp_GET_ProgramTypeListAll |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| SeedDataDAL.GetDistributionCenterDataTableList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:669 | sp_GET_DistributionCenter |  |
| OrderTrackingDAL.GetDatefromMonthYearStuff | Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:451 | DynamicDatebyMonthByDateRange | @frmDate, @toDate |
| OrderTrackingDAL.Get_ALlOrderSummaryByChemist | Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:478 | sp_Get_ALlOrderSummaryByChemist_Pivot | @ColumnToPivot, @ListToPivot, @Parm |
| OrderTrackingDAL.Get_ALlOrderSummaryByProduct | Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:501 | sp_Get_ALlOrderSummaryByProduct | @Parm |
| CommonDataLoad.GetEmpMarketStructure_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:2062 | sp_Get_EmployeeFieldForceInfo_EmpId | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| DynamicDatebyMonthByDateRange | OrderTrackingDAL.GetDatefromMonthYearStuff | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CampaignNameFromOrderDetail | SeedDataDAL.GetCampaignNameListALLFromOrder | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CustomerTypeAll | SeedDataDAL.GetChemistTypeListALL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DistributionCenter | SeedDataDAL.GetDistributionCenterDataTableList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramTypeListAll | SeedDataDAL.GetProgramTypeListAll | body NOT in repository | read-like name (INFERRED) |
| sp_GET_SMCTypeListParm | SeedDataDAL.GetSMCTypeListParam | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ALlOrderSummaryByChemist_Pivot | OrderTrackingDAL.Get_ALlOrderSummaryByChemist | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ALlOrderSummaryByProduct | OrderTrackingDAL.Get_ALlOrderSummaryByProduct | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeFieldForceInfo_EmpId | CommonDataLoad.GetEmpMarketStructure_Active | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblOrderDetail
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler custNameTextBox_TextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Input Correct Data ! | UNKNOWN |
| (handler valiReport) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Zone! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DICCompanyUnitId, EmpInfoId, RoleTypeId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 15 methods.
- Redirect targets: CustomerEntry.aspx, OrderTrackingList.aspx
- User messages: Input Correct Data !; Please Select From List; Please Select Zone!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, OrderTrackingDAL, SeedDataDAL; DAO: —; Tables (inline): tblOrderDetail

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx`, `Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 108 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/OrderTrackingSummary.aspx |
| Data access | This document §15–17 | 10 SP names, 1 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 10 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- Inline SQL in code-behind without SqlParameter (possible string-built SQL — verify): Parm

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: PAGE-015, PAGE-016, PAGE-147, PAGE-167, PAGE-168, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-320, PAGE-321, PAGE-327, PAGE-339, PAGE-340 …
- Changing DAL methods CommonDataLoad.GetEmpMarketStructure_Active, OrderTrackingDAL.GetDatefromMonthYearStuff, OrderTrackingDAL.Get_ALlOrderSummaryByChemist, OrderTrackingDAL.Get_ALlOrderSummaryByProduct, SeedDataDAL.GetApprovalStatusList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
