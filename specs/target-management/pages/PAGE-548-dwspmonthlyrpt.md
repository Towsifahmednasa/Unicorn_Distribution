---
page_id: PAGE-548
module: M14
page: DWSPMonthlyRpt
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-548 — DWSPMonthlyRpt

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-548 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | DWSPMonthlyRpt |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Reports_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Reports_UI_DWSPMonthlyRpt |
| aspx size | 45935 bytes |
| Code-behind LOC | 1004 |
| Inline script size | 16118 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Grid report (not Crystal) of the **day-wise sales plan for every territory/employee for one month**: for each of up to 31 days it shows General, Campaign, FCB and Total planned value, plus a Grand Total and a "Target Set" column. Filters: Month, Year, Approval Status, and the market-structure user control (Group/Zone/Area/Territory, ..). Export to Excel (HTML table saved as `.xls`). CONFIRMED: `Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx(.cs)`.

## 3. Business Objective
Let sales managers review and audit what DWSP each field employee/territory has planned for the month (data entered through PAGE-538) and compare with the target set.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `empmonthlyexpenserpt.aspx`, `mileageclaim.aspx`

## 5. Route
`~/Reports_UI/DWSPMonthlyRpt.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx |
| Code-behind | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/Reports_UI/IVMarketStructureMarket.ascx |
| JS include | ../VerticalAsset/jquery.tabletoCSV.js |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
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
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlApprovalStatus | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | OnClick→resetBtn_Click |
| btnExportToExcel | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Emp. ID | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Emp. Name | no validator found |  |  |  |  |
| (no id) | BoundField | Designation | no validator found |  |  |  |  |
| (no id) | BoundField | Role | no validator found |  |  |  |  |
| (no id) | BoundField | Target Set | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | General | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign | no validator found |  |  |  |  |
| (no id) | BoundField | FCB | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  |  |

_Truncated at 150 rows; 168 controls parsed._

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlApprovalStatus | aPP | Reports_UI_DWSPMonthlyRpt.LoadData | — | CmnCrystaltoView.GetDWSPMonthlyList_Mew | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | aDataTable | Reports_UI_DWSPMonthlyRpt.LoadData | — | CmnCrystaltoView.GetDWSPMonthlyList_Mew | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | headerTable | Reports_UI_DWSPMonthlyRpt.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| EmployeeIdSelect | param | Reports_UI_DWSPMonthlyRpt.param | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| EmployeeIdSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| rbReportTypeName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlApprovalStatus | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSearch | LinkButton click | (postback) | Reports_UI_DWSPMonthlyRpt.btnSearch_Click | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:921 | — | CmnCrystaltoView.GetDWSPMonthlyList_Mew | sp_Process_DWSPReport_Territory | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| resetBtn | LinkButton click | (postback) | Reports_UI_DWSPMonthlyRpt.resetBtn_Click | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:985 | — | — | — | read-only / none detected |
| btnExportToExcel | LinkButton click | (postback) | Reports_UI_DWSPMonthlyRpt.btnExportToExcel_Click | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:653 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- `Page_Load`: reads `RoleTypeName`, `EmpInfoId`, `RoleTypeId` from Session (try/catch-swallowed); locates the user-control drop-downs (`GroupSelect, ZoneSelect, AreaSelect, TeritorySelect, SubTeritory, MarketSelect`) inside `IVMarketStructure` (`~/Reports_UI/IVMarketStructureMarket.ascx`, role-aware cascade, 504 lines); first load -> `LoadInitialInfo()` (Month/Year lists, Approval Status list from `SeedDataDAL.GetApprovalStatusList` [`SoftwareUseId`/`WebShow`], employee list from `sp_CS_GetEmpGeneralInfo_All`).
- `btnSearch_Click` -> `LoadData()`. Paging via `loadGridView_PageIndexChanging`. `btnExportToExcel_Click` sets `AllowPaging=false`, reloads, renders the grid to an `.xls` response with title `DWSP Report Month of <month> <year>` and print date.
- `loadGridView_OnRowCreated` adds a second header row with day numbers 1..31 (each colspan 4) over the four columns per day.
- The employee drop-down `EmployeeIdSelect` is `Visible="false"` (markup line 118) and its JavaScript loader is unused - not part of the UI.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| Reports_UI_DWSPMonthlyRpt.Page_Load | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:39 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.Page_Load → CommonDataLoad.GetEmployeeList_All / SeedDataDAL.GetApprovalStatusList → SP: sp_CS_GetEmpGeneralInfo_All, sp_GET_ActionStatusList | yes | no |
| Reports_UI_DWSPMonthlyRpt.LoadInitialInfo | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:97 |  | Reports_UI_DWSPMonthlyRpt.LoadInitialInfo → CommonDataLoad.GetEmployeeList_All / SeedDataDAL.GetApprovalStatusList → SP: sp_CS_GetEmpGeneralInfo_All, sp_GET_ActionStatusList | yes | no |
| Reports_UI_DWSPMonthlyRpt.LoadData | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:145 |  | Reports_UI_DWSPMonthlyRpt.LoadData → CmnCrystaltoView.GetDWSPMonthlyList_Mew → SP: sp_Process_DWSPReport_Territory | yes | no |
| Reports_UI_DWSPMonthlyRpt.loadGridView_PageIndexChanging | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:648 | object sender, GridViewPageEventArgs e | Reports_UI_DWSPMonthlyRpt.loadGridView_PageIndexChanging | no | no |
| Reports_UI_DWSPMonthlyRpt.btnExportToExcel_Click | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:653 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.btnExportToExcel_Click | no | no |
| Reports_UI_DWSPMonthlyRpt.loadGridView_OnRowCreated | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:724 | object sender, GridViewRowEventArgs e | Reports_UI_DWSPMonthlyRpt.loadGridView_OnRowCreated | no | no |
| Reports_UI_DWSPMonthlyRpt.btnSearch_Click | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:921 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.btnSearch_Click → CmnCrystaltoView.GetDWSPMonthlyList_Mew → SP: sp_Process_DWSPReport_Territory | no | no |
| Reports_UI_DWSPMonthlyRpt.GetMileageClaimList | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:952 | string param | Reports_UI_DWSPMonthlyRpt.GetMileageClaimList → Setup2DAL.GetMileageClaimList → SP: sp_Get_MileageClaimList | no | no |
| Reports_UI_DWSPMonthlyRpt.gv_DocumentUpload_PreRender | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:961 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.gv_DocumentUpload_PreRender | no | no |
| Reports_UI_DWSPMonthlyRpt.loadGridView_RowCommand | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:973 | object sender, GridViewCommandEventArgs e | Reports_UI_DWSPMonthlyRpt.loadGridView_RowCommand | no | no |
| Reports_UI_DWSPMonthlyRpt.resetBtn_Click | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:985 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.resetBtn_Click | no | no |
| Reports_UI_DWSPMonthlyRpt.rbType_SelectedIndexChanged | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:990 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.rbType_SelectedIndexChanged | no | no |
| Reports_UI_DWSPMonthlyRpt.rbReportTypeName_SelectedIndexChanged | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:996 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.rbReportTypeName_SelectedIndexChanged | no | no |
| Reports_UI_DWSPMonthlyRpt.ddlArea_SelectedIndexChanged | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:1001 | object sender, EventArgs e | Reports_UI_DWSPMonthlyRpt.ddlArea_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

**Verified behaviour (manual review):** `LoadData()` (lines 144-190) converts blank Approval Status to the literal `"Select"`, then: if `RoleTypeName == "DZSM"` a Zone must be selected (`ZoneSelect.SelectedValue != ""`, else popup ` Please Select Zone!`) before calling `CmnCrystaltoView.GetDWSPMonthlyList_Mew(monthNumber, monthName, year, approvalStatus, regionId, areaId, trId)`; other roles call it unconditionally. DAL: `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1568-1596` -> `sp_Process_DWSPReport_Territory` with `@Month,@Year,@MonthValue,@ApprovalStatus,@RegionId,@AreaId,@TrId` (older `sp_Process_DWSPReport` commented out). Result bound to the 31x4 column grid (`{d}_Gen, {d}_Cam, {d}_FCB, {d}_Total`, `Grand_Total`). The private `param()` filter (adds `mas.EmpInfoId`) is defined but never called (dead), and `resetBtn_Click` redirects to `EmpMonthlyExpenseRpt.aspx` (copy-paste target).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| Reports_UI_DWSPMonthlyRpt.GetMileageClaimList | Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs:952 | string param | ~/Reports_UI/DWSPMonthlyRpt.aspx/GetMileageClaimList | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| CommonDataLoad.GetEmployeeList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1761 | sp_CS_GetEmpGeneralInfo_All |  |
| CmnCrystaltoView.GetDWSPMonthlyList_Mew | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1569 | sp_Process_DWSPReport_Territory | @ApprovalStatus, @AreaId, @Month, @MonthValue, @RegionId, @TrId, @Year |
| Setup2DAL.GetMileageClaimList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2379 | sp_Get_MileageClaimList | @param |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_All | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MileageClaimList | Setup2DAL.GetMileageClaimList | body NOT in repository | read-like name (INFERRED) |
| sp_Process_DWSPReport_Territory | CmnCrystaltoView.GetDWSPMonthlyList_Mew | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler LoadData) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Zone! | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-019 | **DWSP report data source.** Rows come from `sp_Process_DWSPReport_Territory` (body not in repo) with parameters month name/number, year, approval status, region, area, territory. Blank Approval Status is sent as the sentinel string `Select`. Columns per day: General, Campaign, FCB, Total; final `Grand_Total`. | `Reports_UI/DWSPMonthlyRpt.aspx.cs:144-190`; `CmnCrystaltoView.cs:1568-1596`; `DWSPMonthlyRpt.aspx:273-446` | code-behind + SP | CONFIRMED (wiring); UNKNOWN (SP logic) |
| BR-M14-020 | **DZSM must choose a zone.** For `RoleTypeName == "DZSM"` the report refuses to run without a Zone (`Please Select Zone!`). Other roles (incl. Admin, AM, MIO) run with blank zone/area/territory = unrestricted at the page level; row-level restriction by role therefore depends on the user control's default selections and on the SP (UNKNOWN). | `DWSPMonthlyRpt.aspx.cs:144-180` | code-behind | CONFIRMED |
| BR-M14-002 | Month/Year selectors as in the entry pages (same helper code). | `DWSPMonthlyRpt.aspx.cs:65-95` | code-behind | CONFIRMED |

## 20. Permission Matrix
- Session keys read: `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (swallowed if missing). Master page enforces login.
- Only the DZSM zone requirement is a code-level restriction (BR-M14-020). The `IVMarketStructure` control has its own role scoping (reads Session RoleTypeName/EmpInfoId/RoleTypeId, calls `sp_HigharchyInfoByEmployeeId`, AM/DZSM limited lists) - not reviewed line by line (PARTIAL). Menu-based otherwise.

## 21. Status Lifecycle
No status column written. Approval Status filter uses `SeedDataDAL.GetApprovalStatusList` values (SoftwareUseId) - meaning of DWSP approval states UNKNOWN (no code in M14 sets them).

## 22. Error Handling
- Missing zone (DZSM) -> `faildalert(' Please Select Zone!')`; empty result -> Excel export shows `No Data Found!!` alert.
- `DAL` exceptions bubble (no catch in `LoadData`). Session read failures swallowed.
- `executionTimeMessage` prints a literal string (missing `$`), timing is not actually logged.

## 23. Audit
Read-only report; no audit written.

## 24. Integration
None.

## 25. Reports
Interactive GridView report + Excel export only. See `specs/target-management/reports/RPT-M14-01-dwsp-monthly-report.md`.

## 26. Dependencies
BLL: —; DAL: CmnCrystaltoView, CommonDataLoad, SeedDataDAL, Setup2DAL; DAO: —; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-548-01 | Positive | Admin selects current month/year, no filters, Search | grid shows all territories with 31 day blocks | BR-M14-019 |
| TC-548-02 | Negative | Login as DZSM, Search without Zone | popup `Please Select Zone!`, no query | BR-M14-020 |
| TC-548-03 | Positive | Export to Excel after search | `DWSP_Report_<timestamp>.xls` with heading `DWSP Report Month of ..` | BR-M14-019 |
| TC-548-04 | Edge | Export with empty grid | `No Data Found!!` | - |
| TC-548-05 | Edge | Approval Status left blank | SP receives `Select` | BR-M14-019 |
| TC-548-06 | Defect | Try to filter by employee | no employee filter available/effective | §30 |
| TC-548-07 | Edge | Month with 30 days | 31st-day columns show empty/0 (SP dependent, UNKNOWN) | BR-M14-019 |

## 28. Reverse Engineering References
- `Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx`, `Solution.Web/Reports_UI/DWSPMonthlyRpt.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `DWSPMonthlyRpt.aspx(.cs)` | n/a | MISSING | - |
| Employee filter | None existed | dead code | - | DEFECT | §30 |
| SP logic | None existed | `sp_Process_DWSPReport_Territory` | body not in repo | UNKNOWN | GAP-M14-001 |

## 30. Known Gaps
- Dead employee filter (`param()` unused, `EmployeeIdSelect` hidden).
- Reset button goes to a different report (`EmpMonthlyExpenseRpt.aspx`) - copy/paste (`DWSPMonthlyRpt.aspx.cs:984-987`).
- `using System.Windows.Media;` (WPF assembly) referenced in a web page.
- Export re-renders the whole grid to HTML `.xls`; no size limit, large data may time out (DAL default timeout).
- Report SP body unknown so totals/rounding/approval semantics cannot be verified.
- Static shared DAL/DAO fields (`static SeedDataDAL`, `CommonDataLoad`) across requests - safe only if they hold no state.

## 31. Change Impact
Depends on `sp_Process_DWSPReport_Territory` and `IVMarketStructureMarket.ascx` (also used by other reports). Data comes from entries made in PAGE-538. Changing the SP output column names (`{d}_Gen` ...) breaks the fixed 31-day grid.

