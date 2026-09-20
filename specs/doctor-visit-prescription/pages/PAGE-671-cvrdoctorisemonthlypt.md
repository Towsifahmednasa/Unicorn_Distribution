---
page_id: PAGE-671
module: M16
page: CVRDoctoriseMonthlypt
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-671 — CVRDoctoriseMonthlypt

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-671 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | CVRDoctoriseMonthlypt |
| Kind | Page (INFERRED from name) |
| Web folder | Reports_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Reports_UI_CVRDoctoriseMonthlypt |
| aspx size | 38162 bytes |
| Code-behind LOC | 654 |
| Inline script size | 16458 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CVRDoctoriseMonthlypt** (Page). Page title/heading text: `Customer Visit Report`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `dcrdoctorisemonthlypt.aspx`, `mileageclaim.aspx`

## 5. Route
`~/Reports_UI/CVRDoctoriseMonthlypt.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx |
| Code-behind | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/Reports_UI/IVMarketStructureMarket.ascx |
| JS include | ../VerticalAsset/jquery.tabletoCSV.js |
| BLL class | ProductBLL (Library.BLL/SInventory_BLL/ProductBLL.cs) |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | ProductDAL (Library.DAL/SInventory_DAL/ProductDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| rbType | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbType_SelectedIndexChanged |
| EmployeeIdSelect | DropDownList |  | no validator found |  | Visible=false | see §9 |  |
| rbReportTypeName | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbReportTypeName_SelectedIndexChanged |
| fromDateTextBox | TextBox | Select  From Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| ddlBrand | DropDownList |  | no validator found |  |  | see §9 |  |
| DoctorTypeSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProgramType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlPharmaPlatform | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlApprovalStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| ddlMIO | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | OnClick→resetBtn_Click |
| btnExportToExcel | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Name | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Code | no validator found |  |  |  |  |
| (no id) | BoundField | Degree | no validator found |  |  |  |  |
| (no id) | BoundField | Speciality | no validator found |  |  |  |  |
| (no id) | BoundField | Degree | no validator found |  |  |  |  |
| (no id) | BoundField | Provider Type | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Type | no validator found |  |  |  |  |
| (no id) | BoundField | National | no validator found |  |  |  |  |
| (no id) | BoundField | National Code | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Zone Code | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Area Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | 1 | no validator found |  |  |  |  |
| (no id) | BoundField | 2 | no validator found |  |  |  |  |
| (no id) | BoundField | 3 | no validator found |  |  |  |  |
| (no id) | BoundField | 4 | no validator found |  |  |  |  |
| (no id) | BoundField | 5 | no validator found |  |  |  |  |
| (no id) | BoundField | 6 | no validator found |  |  |  |  |
| (no id) | BoundField | 7 | no validator found |  |  |  |  |
| (no id) | BoundField | 8 | no validator found |  |  |  |  |
| (no id) | BoundField | 9 | no validator found |  |  |  |  |
| (no id) | BoundField | 10 | no validator found |  |  |  |  |
| (no id) | BoundField | 11 | no validator found |  |  |  |  |
| (no id) | BoundField | 12 | no validator found |  |  |  |  |
| (no id) | BoundField | 13 | no validator found |  |  |  |  |
| (no id) | BoundField | 14 | no validator found |  |  |  |  |
| (no id) | BoundField | 15 | no validator found |  |  |  |  |
| (no id) | BoundField | 16 | no validator found |  |  |  |  |
| (no id) | BoundField | 17 | no validator found |  |  |  |  |
| (no id) | BoundField | 18 | no validator found |  |  |  |  |
| (no id) | BoundField | 19 | no validator found |  |  |  |  |
| (no id) | BoundField | 20 | no validator found |  |  |  |  |
| (no id) | BoundField | 21 | no validator found |  |  |  |  |
| (no id) | BoundField | 22 | no validator found |  |  |  |  |
| (no id) | BoundField | 23 | no validator found |  |  |  |  |
| (no id) | BoundField | 24 | no validator found |  |  |  |  |
| (no id) | BoundField | 25 | no validator found |  |  |  |  |
| (no id) | BoundField | 26 | no validator found |  |  |  |  |
| (no id) | BoundField | 27 | no validator found |  |  |  |  |
| (no id) | BoundField | 28 | no validator found |  |  |  |  |
| (no id) | BoundField | 29 | no validator found |  |  |  |  |
| (no id) | BoundField | 30 | no validator found |  |  |  |  |
| (no id) | BoundField | 31 | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlApprovalStatus | aPP | Reports_UI_CVRDoctoriseMonthlypt.LoadData | — | CmnCrystaltoView.GetCVRDoctorWiseDayList, CmnCrystaltoView.GetDCRUserWiseList, CmnCrystaltoView.GetDoctorBrandWiseList,  | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlProgramType | ProviderType | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetCVRDoctorWiseDayList | @ProviderType | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlProgramType | ProviderType | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetDynamicVisitStatusReport | @ProviderType | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlPharmaPlatform | PharmaPlatform | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetCVRDoctorWiseDayList | @PharmaPlatform | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlPharmaPlatform | PharmaPlatform | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetDynamicVisitStatusReport | @PharmaPlatform | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlMIO | EmpID | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetDynamicVisitStatusReport | @EmpID | UNKNOWN (SP body not in repo) | UNKNOWN |
| DoctorTypeSelect | DCType | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetDynamicVisitStatusReport | @DCType | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlBrand | Brand | Reports_UI_CVRDoctoriseMonthlypt.LoadData | (none — page calls DAL directly) | CmnCrystaltoView.GetDynamicVisitStatusReport | @Brand | UNKNOWN (SP body not in repo) | UNKNOWN |
| fromDateTextBox | dtDate | Reports_UI_CVRDoctoriseMonthlypt.LoadData | — | CmnCrystaltoView.GetCVRDoctorWiseDayList, CmnCrystaltoView.GetDCRUserWiseList, CmnCrystaltoView.GetDoctorBrandWiseList,  | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | param | Reports_UI_CVRDoctoriseMonthlypt.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbReportTypeName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlBrand | Page_Load → ProductBLL.LoadProductSQ | inline SQL | ProductSQName/ProductBrandId | tblProductSQ |
| DoctorTypeSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProgramType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlPharmaPlatform | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlApprovalStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlMIO | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSearch | LinkButton click | (postback) | Reports_UI_CVRDoctoriseMonthlypt.btnSearch_Click | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:403 | — | CmnCrystaltoView.GetCVRDoctorWiseDayList, CmnCrystaltoView.GetDCRUserWiseList, CmnCrystaltoView.GetD | DynamicDateByDateRange, DynamicPivotBrandWiseDCR, DynamicPivotDoctorWiseCVR_New_ForSearch, DynamicPivotProductdWiseDCR,  | SP executed — effect on tables UNKNOWN (body not in repo) |
| resetBtn | LinkButton click | (postback) | Reports_UI_CVRDoctoriseMonthlypt.resetBtn_Click | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:632 | — | — | — | read-only / none detected |
| btnExportToExcel | LinkButton click | (postback) | Reports_UI_CVRDoctoriseMonthlypt.btnExportToExcel_Click | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:451 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: ExportToPdf, GetApprovalStatusList, GetAreaList, GetEmpList, GetUserRoleInfo, ResetClick, editClick, exportTableToExcel, exportToExcel, exporttocsv, pageLoad, un
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Reports_UI_CVRDoctoriseMonthlypt.Page_Load | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:38 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.Page_Load → ProductBLL.LoadProductSQ → CommonDataLoad.GetDoctorType_Active / CommonDataLoad.GetEmployeeList_Active / ProductDAL.LoadProductSQ / SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetProgramTypeListParam / SeedDataD | yes | no |
| Reports_UI_CVRDoctoriseMonthlypt.LoadInitialInfo | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:106 |  | Reports_UI_CVRDoctoriseMonthlypt.LoadInitialInfo → CommonDataLoad.GetDoctorType_Active / CommonDataLoad.GetEmployeeList_Active / SeedDataDAL.GetApprovalStatusList / SeedDataDAL.GetProgramTypeListParam / SeedDataDAL.GetSMCTypeListParam → SP: sp_CS_GetEmpGeneral | yes | no |
| Reports_UI_CVRDoctoriseMonthlypt.LoadData | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:208 |  | Reports_UI_CVRDoctoriseMonthlypt.LoadData → CmnCrystaltoView.GetCVRDoctorWiseDayList / CmnCrystaltoView.GetDCRUserWiseList / CmnCrystaltoView.GetDoctorBrandWiseList / CmnCrystaltoView.GetDoctorProductWiseList / CmnCrystaltoView.GetDynamicDateBy → SP: DynamicDa | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.btnSearch_Click | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:403 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.btnSearch_Click → CmnCrystaltoView.GetCVRDoctorWiseDayList / CmnCrystaltoView.GetDCRUserWiseList / CmnCrystaltoView.GetDoctorBrandWiseList / CmnCrystaltoView.GetDoctorProductWiseList / CmnCrystaltoView.GetDynamicDateBy → SP: Dy | yes | no |
| Reports_UI_CVRDoctoriseMonthlypt.btnExportToExcel_Click | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:451 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.btnExportToExcel_Click | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.GetMileageClaimList | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:562 | string param | Reports_UI_CVRDoctoriseMonthlypt.GetMileageClaimList → Setup2DAL.GetMileageClaimList → SP: sp_Get_MileageClaimList | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.gv_DocumentUpload_PreRender | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:571 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.gv_DocumentUpload_PreRender | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.loadGridView_RowCommand | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:583 | object sender, GridViewCommandEventArgs e | Reports_UI_CVRDoctoriseMonthlypt.loadGridView_RowCommand | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.resetBtn_Click | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:632 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.resetBtn_Click | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.rbType_SelectedIndexChanged | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:637 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.rbType_SelectedIndexChanged | no | no |
| Reports_UI_CVRDoctoriseMonthlypt.rbReportTypeName_SelectedIndexChanged | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:643 | object sender, EventArgs e | Reports_UI_CVRDoctoriseMonthlypt.rbReportTypeName_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| Reports_UI_CVRDoctoriseMonthlypt.GetMileageClaimList | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs:562 | string param | ~/Reports_UI/CVRDoctoriseMonthlypt.aspx/GetMileageClaimList | via BLL |

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ProductBLL.LoadProductSQ | Library.BLL/SInventory_BLL/ProductBLL.cs:110 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ProductDAL.LoadProductSQ | Library.DAL/SInventory_DAL/ProductDAL.cs:245 | inline SQL: tblProductSQ |  |
| CommonDataLoad.GetDoctorType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:830 | sp_Get_DoctorType_Active |  |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| SeedDataDAL.GetProgramTypeListParam | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:928 | sp_GET_ProgramTypeListParm | @Parm |
| SeedDataDAL.GetSMCTypeListParam | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:949 | sp_GET_SMCTypeListParm | @Parm |
| CmnCrystaltoView.GetDynamicDateByDateRange | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1698 | DynamicDateByDateRange | @frmDate, @toDate |
| CmnCrystaltoView.GetCVRDoctorWiseDayList | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1053 | DynamicPivotDoctorWiseCVR_New_ForSearch | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType,  |
| CmnCrystaltoView.GetDoctorBrandWiseList | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1602 | DynamicPivotBrandWiseDCR | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType,  |
| CmnCrystaltoView.GetDoctorProductWiseList | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1241 | DynamicPivotProductdWiseDCR | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType,  |
| CmnCrystaltoView.GetDCRUserWiseList | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1275 | DynamicPivotUserWiseDCR | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType,  |
| CmnCrystaltoView.GetDynamicVisitStatusReport | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1091 | DynamicVisitStatusReport | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @DCType, @EmpID, @ListToPivot, @Month, @PharmaPlatform |
| Setup2DAL.GetMileageClaimList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2379 | sp_Get_MileageClaimList | @param |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| DynamicDateByDateRange | CmnCrystaltoView.GetDynamicDateByDateRange | body NOT in repository | read-like name (INFERRED) |
| DynamicPivotBrandWiseDCR | CmnCrystaltoView.GetDoctorBrandWiseList | body NOT in repository | read-like name (INFERRED) |
| DynamicPivotDoctorWiseCVR_New_ForSearch | CmnCrystaltoView.GetCVRDoctorWiseDayList | body NOT in repository | read-like name (INFERRED) |
| DynamicPivotProductdWiseDCR | CmnCrystaltoView.GetDoctorProductWiseList | body NOT in repository | read-like name (INFERRED) |
| DynamicPivotUserWiseDCR | CmnCrystaltoView.GetDCRUserWiseList | body NOT in repository | read-like name (INFERRED) |
| DynamicVisitStatusReport | CmnCrystaltoView.GetDynamicVisitStatusReport | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramTypeListParm | SeedDataDAL.GetProgramTypeListParam | body NOT in repository | read-like name (INFERRED) |
| sp_GET_SMCTypeListParm | SeedDataDAL.GetSMCTypeListParam | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorType_Active | CommonDataLoad.GetDoctorType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MileageClaimList | Setup2DAL.GetMileageClaimList | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblProductSQ
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validatea) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Area! | UNKNOWN |
| (handler Validatea) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Zone! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 3 of 18 methods.
- Redirect targets: DcrDoctoriseMonthlypt.aspx
- User messages: Please Select Area!; Please Select From List; Please Select Zone!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ProductBLL; DAL: CmnCrystaltoView, CommonDataLoad, ProductDAL, SeedDataDAL, Setup2DAL; DAO: —; Tables (inline): tblProductSQ

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
- `Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx`, `Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 92 controls parsed | n/a | PARTIAL | Solution.Web/Reports_UI/CVRDoctoriseMonthlypt.aspx |
| Data access | This document §15–17 | 12 SP names, 1 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 12 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: PAGE-017, PAGE-167, PAGE-168, PAGE-169, PAGE-173, PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-548, PAGE-549, PAGE-555, PAGE-557, PAGE-562, PAGE-563 …
- Changing DAL methods CmnCrystaltoView.GetCVRDoctorWiseDayList, CmnCrystaltoView.GetDCRUserWiseList, CmnCrystaltoView.GetDoctorBrandWiseList, CmnCrystaltoView.GetDoctorProductWiseList, CmnCrystaltoView.GetDynamicDateByDateRange affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
