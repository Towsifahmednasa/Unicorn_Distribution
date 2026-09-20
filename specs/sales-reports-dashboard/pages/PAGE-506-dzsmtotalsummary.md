---
page_id: PAGE-506
module: M13
page: DZSMTotalSummary
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-506 — DZSMTotalSummary

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-506 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | DZSMTotalSummary |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DZSMTotalSummary |
| aspx size | 25371 bytes |
| Code-behind LOC | 795 |
| Inline script size | 1422 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**DZSMTotalSummary** ("DZSM wise Business Summary") - one wide grid (27 columns) per DZSM/zone/area with invoice, sales, return, customer coverage, fixed/campaign/final sales and Blue/Green net sell & coverage; XLS export.

## 3. Business Objective
Let NSM/DZSM/AM see their scope's business summary including the Blue/Green (customer-type) sales and coverage split. INFERRED from column names; scoping CONFIRMED (BR-M13-033).

## 4. Navigation
Menu placement is DB-driven (`tblMainMenu`/`sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`), rows not in repository -> exact menu path `UNKNOWN`. Legacy static menu: "DZSM Wise Business Summary" (`MasterPage.master:1391`). `TerritoryWiseSalesReport.aspx.cs` redirects here on cancel (line 673).

## 5. Route
`~/SInventory_UI/DZSMTotalSummary.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DZSMTotalSummary.aspx |
| Code-behind | Solution.Web/SInventory_UI/DZSMTotalSummary.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | ../VerticalAsset/jquery.tabletoCSV.js |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
Generated inventory:

Generated inventory:

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| hfRegionId | HiddenField |  | no validator found |  |  |  |  |
| hfGroupID | HiddenField |  | no validator found |  |  |  |  |
| hfMarket | HiddenField |  | no validator found |  |  |  |  |
| hfSubTeritory | HiddenField |  | no validator found |  |  |  |  |
| hfTeritory | HiddenField |  | no validator found |  |  |  |  |
| hfArea | HiddenField |  | no validator found |  |  |  |  |
| hfZone | HiddenField |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox | Select  From Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| GroupSelect | DropDownList |  | no validator found |  | Enabled=False | see §9 | OnSelectedIndexChanged→GroupSelect_SelectedIndexChanged |
| ZoneSelect | DropDownList |  | no validator found |  |  | see §9 |  |
| AreaSelect | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→AreaSelect_SelectedIndexChanged |
| TeritorySelect | DropDownList |  | no validator found |  |  | see §9 |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| LinkButton2 | LinkButton |  | no validator found |  | Visible=false |  | OnClick→excelButton1_Click |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Name | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Proforma Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Proforma Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Sales Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Return Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Return Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Chemist Coverage | no validator found |  |  |  |  |
| (no id) | BoundField | FCB Sales (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Sales (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | General Sales (TP)  | no validator found |  |  |  |  |
| (no id) | BoundField | FCB Proforma (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Proforma (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | General Sales (TP) (Proforma) | no validator found |  |  |  |  |
| (no id) | BoundField | Chemist Coverage (Proforma) | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Undelivered Invoice  | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net UndeliveredAmount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Undelivered Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | BSP Net Proforma Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | GSP Net Proforma Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | BSP Net Sales Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | GSP Net Sales Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | BSP Coverage (Proforma) | no validator found |  |  |  |  |
| (no id) | BoundField | GSP Coverage (Proforma) | no validator found |  |  |  |  |
| (no id) | BoundField | BSP Coverage  | no validator found |  |  |  |  |
| (no id) | BoundField | GSP Coverage  | no validator found |  |  |  |  |
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| dzsmDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→excelButton1_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified**: fromDate = first day of month, toDate = last day of month (`dd MMMM, yyyy`) on first load; GroupSelect/ZoneSelect/AreaSelect cascade (`sp_Get_Zone_AllByGroup`, `sp_Get_Zone_ForDSM`, `sp_CS_GetTerritory_ByAreaId_All`), hidden fields hfGroupID/hfRegionId/hfZone lock DZSM/NSM scope; `salesCenterDropDownList` is used by the export handler only.

**Verified**: fromDate = first day of month, toDate = last day of month (`dd MMMM, yyyy`) on first load; GroupSelect/ZoneSelect/AreaSelect cascade (`sp_Get_Zone_AllByGroup`, `sp_Get_Zone_ForDSM`, `sp_CS_GetTerritory_ByAreaId_All`), hidden fields hfGroupID/hfRegionId/hfZone lock DZSM/NSM scope; `salesCenterDropDownList` is used by the export handler only.

## 8. Field Mapping
ZoneSelect.SelectedValue -> `Parm()` (raw value, no predicate text) -> `parm` argument; fromDateTextBox/toDateTextBox `.Text` -> string `fromdate/todate` (not DateTime).

## 9. Dropdown Mapping
GroupSelect: `CommonDataLoad.GetGroupInfo_All`; ZoneSelect/AreaSelect via cascade handlers (lines 265-372).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| viewRptButton | viewRptButton_Click -> LoadInfo | grid |
| excelButton1 | excelButton1_Click (line 654) | requires salesCenterDropDownList value; re-queries **DZSMwiseLoadSummaryBLL(from,to,branch)** (sp_GET_DZSMwiseReport) and streams "BusinessSummary.xls" |

## 11. Frontend Flow
Load: month range, role hierarchy (`GetHigharcyInfoByEmployeeId`), lock selectors, fill dropdowns. Search binds grid + 27 footer sums.

## 12. Backend Flow
`LoadInfo`: both dates required else alert "Please Select Mandatory Field!!" -> `DZSMwiseLoadSummaryparm` -> if rows bind + footers (BR-M13-030: sums include CustomerCoverPer, CustomerCoverPerProforma; int-typed columns cast with `Field<int?>`; a decimal/int type mismatch throws `InvalidCastException`, no try/catch) else alert "No Data Found!!".

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
TotalSummaryBLL (export path), OrderInfoBLL.LoadDZSM.

## 15. Repository Mapping
TotalSummaryDAL.DZSMwiseLoadSummaryparm (grid), DZSMwiseLoadSummaryBLL (export); CommonDataLoad.

## 16. Stored Procedure Mapping
| SP | Called from |
|---|---|
| sp_GET_DZSMwiseReportParam_ByProcess(@fromdate,@todate,@parm) | grid |
| sp_GET_DZSMwiseReport / sp_GET_DZSMwiseNAtionalReport | export / national |
| sp_HigharchyInfoByEmployeeId, sp_RSMInfoByEmployeeId, sp_Get_Zone_*, sp_CS_* | hierarchy and dropdowns |
All bodies NOT in repository.

## 17. Database Mapping
Tables UNKNOWN (SP). Grid columns: NumberofProformaInvoice, SumofNetProformaAmount, ProTpVat, NumberofInvoiceSold, SumofNetSalesAmount, DelTpVat, NumberofReturnInvoice, SumofNetReturnAmount, DelReTpVat, CustomerCoverPer, SumofNetSalesAmountFixed/Camp/Fixed2/Camp2, FinalSales, FinalSales2, CustomerCoverPerProforma, BlueNetSell, GreenNetSell, DelBlueNetSell, DelGreenNetSell, BlueCov, greenCov, DelBlueCov, DelgreenCov.

## 18. Validation Matrix
"Please Select Mandatory Field!!" (dates), "No Data Found!!". No other validation.

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-030 | Page footers are computed server-side in C# by summing the returned DataTable per column (null -> 0). Rounding is inconsistent: New/New2/T/MIOWise use Math.Round(x) shown "#,##0" (integer), TotalSummaryNew shows some totals unformatted (ToString("")), DZSMTotalSummary uses "N2" and also sums percentage-type columns (CustomerCoverPer, CustomerCoverPerProforma) and count columns cast to int. | `Solution.Web/SInventory_UI/DZSMTotalSummary.aspx.cs:504` | *.aspx.cs LoadInfo footers | CONFIRMED |
| BR-M13-033 | DZSMTotalSummary derives the caller hierarchy from Session (RoleTypeName/EmpInfoId/RoleTypeId) via GetHigharcyInfoByEmployeeId: AM -> AreaId list, DZSM -> RegionId list, NSM -> GroupId list; group/zone selectors are pre-selected and locked for DZSM/NSM; default period = first to last day of the current month. | `Solution.Web/SInventory_UI/DZSMTotalSummary.aspx.cs:82` | DZSMTotalSummary.aspx.cs Page_Load | CONFIRMED |
| BR-M13-034 | The grid is fed by TotalSummaryDAL.DZSMwiseLoadSummaryparm(from,to,parm) -> sp_GET_DZSMwiseReportParam_ByProcess(@fromdate,@todate,@parm) where parm is the raw ZoneSelect.SelectedValue (string dates are passed, not DateTime). The Excel button instead calls DZSMwiseLoadSummaryBLL(from,to,salesCenter) (sp_GET_DZSMwiseReport) so export and grid come from different SPs. | `Solution.Web/SInventory_UI/DZSMTotalSummary.aspx.cs:415` | DZSMTotalSummary.aspx.cs LoadInfo / excelButton1_Click | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).
- Role scope derived from Session (AM->AreaId, DZSM->RegionId, NSM->GroupId) and applied by locking selectors (BR-M13-033); server does not re-validate posted values.

## 21. Status Lifecycle
Read-only.

## 22. Error Handling
No try/catch in LoadInfo; `Page_Load` swallows session-read errors (`catch { }`).

## 23. Audit
None.

## 24. Integration
None.

## 25. Reports
On-screen; see RPT-M13-01.

## 26. Dependencies
TotalSummaryDAL, CommonDataLoad, TotalSummaryBLL.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | DZSM login | group+zone preselected/locked | BR-M13-033 |
| TC-02 | Search current month | 27 footer sums | BR-M13-034 |
| TC-03 | Export without sales centre | nothing exported | BR-M13-034 |
| TC-04 | Compare grid vs export totals | differ (different SP) | BR-M13-034 |
| TC-05 | Footer CustomerCoverPer | equals sum of row percentages (not a coverage %) | BR-M13-030 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DZSMTotalSummary.aspx(.cs)`; `TotalSummaryDAL.cs:755`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| KPI formulas | none | SP | not in repo | UNKNOWN | GAP-M13-001 |

## 30. Known Gaps
- Percentage columns summed in footer; export from a different SP than grid; excel button depends on a hidden/legacy dropdown (GAP-M13-013).
- `if (EmpInfoId != "" || EmpInfoId != null)` is always true (null deref possible when session missing).

## 31. Change Impact
- `sp_GET_DZSMwiseReport*` also used by TerritoryWiseSalesReport (530).
