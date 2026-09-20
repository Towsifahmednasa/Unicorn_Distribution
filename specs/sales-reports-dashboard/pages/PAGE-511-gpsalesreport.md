---
page_id: PAGE-511
module: M13
page: GpSalesReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-511 — GpSalesReport

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-511 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | GpSalesReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_GpSalesReport |
| aspx size | 13237 bytes |
| Code-behind LOC | 268 |
| Inline script size | 2424 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**GpSalesReport** (grid title "Territory wise Sales Report List") shows territory-wise sales with a GPSales and a TotalSales column and CSV export; it also has a legacy "view" button that opens `ProformaReportViewer` (another module).

## 3. Business Objective
Show, per territory/customer scope, how much of the delivered sales is GP (gross-profit/general-program - meaning of "GP" is UNKNOWN; INFERRED as the GP customer segment) versus total sales. Column meanings come from the SP (`sp_Get_TerritorryWiseSalesReportList`, not in repo).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `proformareport.aspx`, `proformareportviewer.aspx`

## 5. Route
`~/SInventory_UI/GpSalesReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/GpSalesReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/GpSalesReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |

## 7. UI Control Inventory
Generated inventory:

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 |  |
| InvoiceDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| lblCount | Label | Total Net Amount : 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Date | no validator found |  |  |  |  |
| (no id) | BoundField | GP Sales | no validator found |  |  |  |  |
| (no id) | BoundField | Total Sales | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified**: InvoiceDateTextBox/todateTextBox default today; dcDropDownList1 via `OtherStockActionBLL.DCLoad`; `IVMarketStructure` user control (6 selectors). No required fields.

## 8. Field Mapping
UI -> `Parm()` predicate: dc -> `CU.ComUnitId`; dates -> `CONVERT(date,I.UpdateDate) BETWEEN`; group/zone/area/territory/subterritory/market -> `mas.GroupId/RegionId/AreaId/TerritoryId/SubTerritoryId/MarketId` (all string concatenation) -> `CmnCrystaltoView.GetTerriTorryWiseSalesReportDataDAL(@Parm)` -> `sp_Get_TerritorryWiseSalesReportList`.

## 9. Dropdown Mapping
DC list: `OtherStockActionBLL.DCLoad(dcDropDownList1)`; `CheckBox1_CheckedChanged` reloads it. Market structure: user control.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| SearchButton | LoadData | grid + totals |
| cancelButton | Response.Redirect("ProformaReport.aspx") | leaves the module |
| btnExport | btnExport_Click | CSV `TerriTorryWise_Sales_Report_List_<timestamp>.csv` (re-runs LoadData with paging off; header cell text + comma-separated cells) |
| viewRptButton | viewRptButton_Click | sets Session["ProformaReport"]=1 and opens `ProformaReportViewer.aspx?fromDate&toDate&districtId` (requires date range and DC) |

## 11. Frontend Flow
Load: defaults + DC list. Date change (`fromDateTextBox_TextChanged`): From < 01-Apr-2022 is reset to "01 April, 2022". Search binds and totals. Export streams CSV.

## 12. Backend Flow
`LoadData`: SP result; if rows: footer cells [5]="Total", [6]=SUM(GPSales) `N2`, [7]=SUM(TotalSales) `N2`; label "Total Net Amount : " + SUM(TotalSales). Else empty grid, 0.00.

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
OtherStockActionBLL only; DAL called directly.

## 15. Repository Mapping
CmnCrystaltoView.GetTerriTorryWiseSalesReportDataDAL.

## 16. Stored Procedure Mapping
| SP | Called from | Definition |
|---|---|---|
| sp_Get_TerritorryWiseSalesReportList(@Parm) | CmnCrystaltoView.cs:19-27 | body NOT in repo (GPSales/TotalSales definitions UNKNOWN) |

## 17. Database Mapping
Tables inside SP UNKNOWN (predicate aliases: CU = tblCompanyUnit, I = tblInvoice, mas = customer/market master). No writes.

## 18. Validation Matrix
No validation except the From-date floor (01-Apr-2022). Empty To date -> `DateTime.Now` string appended.

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-040 | GpSalesReport (Territory-wise GP sales) filters by CONVERT(date, I.UpdateDate) BETWEEN from AND to, ComUnit and market-structure levels via a concatenated @Parm to sp_Get_TerritorryWiseSalesReportList; it totals GPSales and TotalSales in the footer and shows "Total Net Amount" = sum(TotalSales). Minimum From date is forced to 01 April 2022 on change. CSV export re-runs the query without paging. | `Solution.Web/SInventory_UI/GpSalesReport.aspx.cs:79` | GpSalesReport.aspx.cs LoadData/Parm; CmnCrystaltoView.GetTerriTorryWiseSalesReportDataDAL | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).

## 21. Status Lifecycle
Read-only.

## 22. Error Handling
No try/catch around the SP call (exceptions propagate to the ASP.NET error page); export shows "No Data Found!" via `faildalert` JS when the grid is empty.

## 23. Audit
None.

## 24. Integration
None.

## 25. Reports
On-screen + CSV. See `reports/RPT-M13-06-gp-cogs-reports.md`.

## 26. Dependencies
CmnCrystaltoView, OtherStockActionBLL, IVMarketStructure control.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Default search | rows by UpdateDate=today | BR-M13-040 |
| TC-02 | From date 01-Jan-2020 | reset to 01 April 2022 | BR-M13-040 |
| TC-03 | Export with grid empty | "No Data Found!" | BR-M13-040 |
| TC-04 | Totals | footer GPSales/TotalSales equal column sums | BR-M13-040 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/GpSalesReport.aspx(.cs)`; `CmnCrystaltoView.cs:19`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| GP definition | none | in SP | not in repo | UNKNOWN | GAP-M13-001 |

## 30. Known Gaps
- Copy-paste page: cancel button and viewer target belong to the proforma-report page (`ProformaReport.aspx`, `ProformaReportViewer.aspx`) - the view button does not show GP data.
- Predicate string concatenation (GAP-M13-006); `DateTime.Now` culture format used in SQL literal.
- GP meaning undocumented in code.

## 31. Change Impact
- Same SP is used only by this page and `TerritoryWiseSalesReport` family? (grep: `GetTerriTorryWiseSalesReportDataDAL` callers) - verify before change.
