---
page_id: PAGE-520
module: M13
page: MonitoringReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-520 — MonitoringReport

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-520 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | MonitoringReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_MonitoringReport |
| aspx size | 36129 bytes |
| Code-behind LOC | 1114 |
| Inline script size | 921 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**MonitoringReport** - MIS monitoring view per DC/Zone/Area/Territory showing the TotalSummary block plus period roll-forward (previous-period opening, current period, total) for invoices, sales, discounts, returns, bank deposit, cash in hand and closing invoice.

## 3. Business Objective
Show management the invoiced / returned / delivered ("sales") / outstanding position by distribution centre, zone, area and territory for a date range, and export it to Excel. Business intent INFERRED from headings/columns; wiring CONFIRMED.

## 4. Navigation
Menu placement is DB-driven (`tblMainMenu`/`sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`), rows not in repository -> exact menu path `UNKNOWN`. Legacy static menu (`Solution.Web/MasterPages/MasterPage.master`): not in the legacy menu; no inbound reference.

## 5. Route
`~/SInventory_UI/MonitoringReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MonitoringReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/MonitoringReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
Generated inventory:

Generated inventory:

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| rbReportTypeName | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbReportTypeName_SelectedIndexChanged |
| fromDateTextBox | TextBox | Select  From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| rbAmount | RadioButtonList |  | no validator found |  |  |  |  |
| viewRptButton | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| excelButton1 | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| gv_DistributionCenter | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Distribution Center | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | No of Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Invoice Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Return Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Sales Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Collection | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Outstanding Amt. | no validator found |  |  |  |  |
| gv_Zone | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Previous Period (Opening) | no validator found |  |  |  |  |
| (no id) | BoundField | Current Period | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | Previous Period | no validator found |  |  |  |  |
| (no id) | BoundField | Current Period | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField |  Previous Period | no validator found |  |  |  |  |
| (no id) | BoundField | Current Period | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | Previous Period | no validator found |  |  |  |  |
| (no id) | BoundField | Current Period | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | Bank Deposit | no validator found |  |  |  |  |
| (no id) | BoundField | Cash in Hand | no validator found |  |  |  |  |
| (no id) | BoundField | Previous Period | no validator found |  |  |  |  |
| (no id) | BoundField | Current Period | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| gv_Area | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | No of Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Invoice Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Return Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Sales Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Collection | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Outstanding Amt. | no validator found |  |  |  |  |
| gv_Territory | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | No of Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Invoice Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Return Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Sales Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Collection | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  | Visible=false |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Outstanding Amt. | no validator found |  |  |  |  |
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| zoneDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| territoryDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified in code**: fromDateTextBox/toDateTextBox default today (`dd MMMM, yyyy`, first load); `rbReportTypeName` (1 Distribution Center, 2 Zone, 3 Area, 4 Territory - default and enabled items depend on role, BR-M13-029); `rbAmount` (1 Transactional [default], 2 Non-transactional); market-structure user control `IVMarketStructure` (Group/Zone/Area/Territory/SubTerritory/Market); `divMrk` visible for Area/Territory; grids `gv_DistributionCenter`, `gv_Zone`, `gv_Area`, `gv_Territory` (or `loadGridView`) with two-row headers built in `*_OnRowCreated`; buttons `viewRptButton` (View) and `excelButton1` (-> `btnExportToExcel_Click`). No validators; server checks in §18.

**Verified in code**: fromDateTextBox/toDateTextBox default today (`dd MMMM, yyyy`, first load); `rbReportTypeName` (1 Distribution Center, 2 Zone, 3 Area, 4 Territory - default and enabled items depend on role, BR-M13-029); `rbAmount` (1 Transactional [default], 2 Non-transactional); market-structure user control `IVMarketStructure` (Group/Zone/Area/Territory/SubTerritory/Market); `divMrk` visible for Area/Territory; grids `gv_DistributionCenter`, `gv_Zone`, `gv_Area`, `gv_Territory` (or `loadGridView`) with two-row headers built in `*_OnRowCreated`; buttons `viewRptButton` (View) and `excelButton1` (-> `btnExportToExcel_Click`). No validators; server checks in §18.

## 8. Field Mapping
fromDateTextBox/toDateTextBox -> `DateTime` -> the SP arguments (`@fromdate,@todate,@Type` only - Area/Zone/Territory selectors are not sent). Selector values (`F_GroupSelect`, `F_ZoneSelect`, `F_AreaSelect`, `F_TeritorySelect`) -> `Area`, `Terr`, `ZoneId` strings. Result columns bound by name (BoundField DataField): ComUnitCode, ShortName, NumberofProformaInvoice, ProTpVat, NetInvoiceAmt, NumberofReturnInvoice, SumofNetReturnAmount, DelReTpVat, NetReturnAmt, salesTP, SalesVat, SalesTotal, NumberofInvoiceSold, SumofNetSalesAmount, DelTpVat, NetSalesAmt ("Gross Collection"), Outstanding1-3, Invoice_PreviousPeriodOpening, Invoice_CurrentPeriod, Invoice_Period_Total, Sales_*, Discount_*, Return_*, BankDeposit, CashinHand, ClosingInvoice_PreviousPeriod/CurrentPeriod/_Period_Total.

## 9. Dropdown Mapping
Legacy dropdowns `salesCenterDropDownList/zoneDropDownList/territoryDropDownList` are filled by `OrderInfoBLL.LoadSC / LoadSCZoneWise / LoadZone / LoadTerritory(Session["UserId"])`; market-structure selectors by the user control (CommonDataLoad SPs, other module).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| viewRptButton | viewRptButton_Click -> LoadInfo | query + bind + footer totals |
| excelButton1 | btnExportToExcel_Click | re-runs LoadInfo with paging off, renders grid as .xls (`Content-Disposition attachment`) |
| reset (Unnamed_Click) | Unnamed_Click | clears |
| rbReportTypeName | rbReportTypeName_SelectedIndexChanged | clears all grids, shows/hides market panel |

## 11. Frontend Flow
First load: dates = today; `DropDownlist()`; role narrowing (BR-M13-029). Changing the level radio empties all grids. No JavaScript calculations (footer totals are server-side, BR-M13-030).

## 12. Backend Flow
`LoadInfo`: (1) both dates required (else alert "Please Select Date Range!!"); (2) derive `Type` from `rbReportTypeName`/`rbAmount` (BR-M13-028); (3) call `TotalSummaryBLL.rptMonitoringReport(from,to,Type)` -> sp_RPTMonitoringReport(@fromdate,@todate,@Type); (4) if DZSM and no zone selected, clear rows (New2 family only); (5) bind the grid for the chosen level and write footer sums with `Math.Round(x).ToString("#,##0")`; (6) no rows -> alert "No Data Found!!". Default level is Zone (`Value=2` preselected). Excel export builds a file per level with names "Distribution Center_Wise_", "Zoner_Wise_", "Area_Wise_", "Territory_Wise_" (lines 852-1036). BR-M13-045.

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
TotalSummaryBLL / OrderInfoBLL (TotalSummaryBLL.rptMonitoringReport(from,to,Type)).

## 15. Repository Mapping
TotalSummaryDAL (`aCommonInternalDal.GetDataTableAction(sp, params, "SSIDB")`), see `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs`.

## 16. Stored Procedure Mapping
| SP | Called from | Definition |
|---|---|---|
| sp_RPTMonitoringReport(@fromdate,@todate,@Type) | TotalSummaryBLL.rptMonitoringReport(from,to,Type) | body NOT in repository - all arithmetic UNKNOWN |

## 17. Database Mapping
Tables read inside the SP: UNKNOWN. (The inline-SQL twin `TotalSummaryDAL.LoadSummaryDAL` reads tblCompanyUnit, tblInvoice/Detail, tblSubInvoiceMaster/Detail, tblOrder, tblCustMaster - BR-M13-021..027.) No writes.

## 18. Validation Matrix
| Rule | Message |
|---|---|
| From and To date non-empty | "Please Select Date Range!!" |
| Result not empty | "No Data Found!!" |
| Any exception (TotalSummaryNew only) | "Somthing went wrong" |
Dates parsed with `Convert.ToDateTime` (culture dependent; invalid text raises).

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-024 | Derived columns (exact formulas in inline SQL): NetInvoiceAmt = SumofNetProformaAmount+ProTpVat; NetReturnAmt = SumofNetReturnAmount+DelReTpVat; NetSalesAmt = SumofNetSalesAmount+DelTpVat; salesTP = ProformaNet - ReturnNet; SalesVat = ProVat - ReturnVat; SalesTotal = salesTP+SalesVat; Outstanding1 = ProformaNet - ReturnNet - SoldNet; Outstanding2 = ProVat - ReturnVat - DelTpVat; Outstanding3 = Outstanding1+Outstanding2. (No payment/collection table is involved in this inline query: "Outstanding" here is invoiced-minus-returned-minus-delivered.) For pages whose data comes from SPs (New2, New_T, MIOWise*, Monitoring) the same column names are shown but the SP arithmetic is UNKNOWN. | `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:61` | TotalSummaryDAL.LoadSummaryDAL; column names bound in *.aspx BoundFields | CONFIRMED (inline SQL) / UNKNOWN (SP-fed pages) |
| BR-M13-028 | Report scope radio (rbReportTypeName 1 Distribution Center, 2 Zone, 3 Area, 4 Territory) is translated into the SP argument @Type: "SC", "Zone", then for Area/Territory "AreaTran"/"AreaNONTran"/"TerritoryTran"/"TerritoryNONTran" according to rbAmount (Transactional vs Non-transactional); New2 uses "AreaTranForDSM"/"AreaNONTranForDSM" for Area when Session RoleTypeId="3" (else ZoneId is blanked). Meaning of Tran/NONTran lives inside the SPs (UNKNOWN). | `Solution.Web/SInventory_UI/TotalSummaryNew2.aspx.cs:535` | *.aspx.cs LoadInfo | CONFIRMED |
| BR-M13-045 | MonitoringReport shows, per scope level (SC/Zone/Area/Territory), the invoice/return/sales/outstanding block (same columns as TotalSummary) plus period roll-forward columns Invoice_PreviousPeriodOpening / CurrentPeriod / Total, Sales_*, Discount_*, Return_*, BankDeposit, CashinHand and ClosingInvoice_* from sp_RPTMonitoringReport(@fromdate,@todate,@Type). Formulas are inside the SP (UNKNOWN); the "Zone" scope is the default selection. | `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:554` | MonitoringReport.aspx.cs LoadInfo; TotalSummaryDAL.rptMonitoringReport | CONFIRMED (wiring) / UNKNOWN (formulas) |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).
- Role narrowing by `Session["RoleTypeName"]` (BR-M13-029) - UI/argument only, not an authorization check; data scope for AM/DZSM is trusted to the SP arguments posted by the browser.

## 21. Status Lifecycle
Read-only. Reads delivery status via SP (Partial/Full/Reject semantics UNKNOWN inside SP; see BR-M13-022 for the inline twin).

## 22. Error Handling
try/catch only in TotalSummaryNew (generic alert). Others let exceptions propagate.

## 23. Audit
None.

## 24. Integration
None. Excel export is HTML-as-.xls.

## 25. Reports
On-screen MIS grid: `reports/RPT-M13-01-business-summary-mis-family.md`; legacy Excel viewer path: `RPT-M13-02`.

## 26. Dependencies
TotalSummaryBLL/DAL, OrderInfoBLL, IVMarketStructure control, sp_RPTMonitoringReport(@fromdate,@todate,@Type).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Search with both dates, level Distribution Center | one row per DC; footer = column sums | BR-M13-030 |
| TC-02 | Level Area + Non-transactional | @Type = AreaNONTran | BR-M13-028 |
| TC-03 | Login as DZSM | DC level disabled | BR-M13-029 |
| TC-04 | Login as AM | zone/area/group locked | BR-M13-029 |
| TC-05 | Empty dates | alert | - |
| TC-06 | No data | alert "No Data Found!!" | - |
| TC-07 | Export | .xls contains current grid | BR-M13-032 |
| TC-08 | Compare totals with sp_Get_BusinessSummaryReportList export for same dates | (reconciliation test; formulas differ) | GAP-M13-014 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MonitoringReport.aspx(.cs)`; `Library.BLL/SInventory_BLL/TotalSummaryBLL.cs`; `Library.DAL/SInventory_DAL/TotalSummaryDAL.cs`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| KPI formulas | none | SP-defined | not in repo | UNKNOWN | GAP-M13-001 |

## 30. Known Gaps
- SP body absent (GAP-M13-001); near-duplicate page family (GAP-M13-014): Default level is Zone (`Value=2` preselected). Excel export builds a file per level with names "Distribution Center_Wise_", "Zoner_Wise_", "Area_Wise_", "Territory_Wise_" (lines 852-1036). BR-M13-045.
- Footers rounded to integers; hidden generic error handling; legacy dropdowns/dead handler `excelButton1_Click`; file date n/a (INFERRED from mtime).

## 31. Change Impact
- Same SP used by: see README variant table. Changing `TotalSummaryDAL` affects all pages of the family and BusinessSummaryViewer 2-5.
