---
page_id: PAGE-495
module: M13
page: COGSBasedGPReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-495 — COGSBasedGPReport

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-495 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | COGSBasedGPReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_COGSBasedGPReport |
| aspx size | 14933 bytes |
| Code-behind LOC | 318 |
| Inline script size | 1816 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**COGSBasedGPReport** ("COGS based GP Calculation Report") lists collections (realisations) with their cost of goods sold and gross profit per product/customer/sales executive, with an Excel export. Grid + "Total Realization Amount" label.

## 3. Business Objective
Let finance/management measure gross profit on the basis of amounts actually realised (collected), against COGS, by month, sales centre (BU), customer, product and sales executive. INFERRED from column names; the realised-on-payment-date basis is CONFIRMED (BR-M13-037).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/COGSBasedGPReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/COGSBasedGPReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/COGSBasedGPReport.aspx.cs |
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
| InvoiceDateTextBox | TextBox | Select From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| ddlCustomer | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlCustomerType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProduct | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlOrderSender | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| lblCount | Label | Total Realization Amount : 0.00 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | BoundField | Month | no validator found |  |  |  |  |
| (no id) | BoundField | Date | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | BU Wise | no validator found |  |  |  |  |
| (no id) | BoundField | Particulars | no validator found |  |  |  |  |
| (no id) | BoundField | SKU | no validator found |  |  |  |  |
| (no id) | BoundField | Qty | no validator found |  |  |  |  |
| (no id) | BoundField | Name of Sales Executive | no validator found |  |  |  |  |
| (no id) | BoundField | Realization Amount | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Tax | no validator found |  |  |  |  |
| (no id) | BoundField | COGS Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | GP | no validator found |  |  |  |  |
| (no id) | TemplateField | GP % | no validator found |  |  |  |  |
| lblGPPercent | Label | <%# Eval("GPPercent") + "%" %> | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified defaults**: InvoiceDateTextBox = first day of current month, todateTextBox = today (format `dd MMMM, yyyy`); dcDropDownList1 (DC list from `OtherStockActionBLL.DCLoad`), ddlCustomer ("--- Select Customer ---", value CustomerCode from `tblCustMaster WHERE IsActive=1`), ddlCustomerType (`tblCustomerType WHERE IsActive=1`), ddlProduct (`tblProduct WHERE IsActive=1`), ddlOrderSender (`tblEmpGeneralInfo WHERE EmployeeStatus='Active'`), market-structure user control `IVMarketStructure` (Group/Zone/Area/Territory/SubTerritory/Market). No required fields; no validators.

## 8. Field Mapping
| UI | -> `Parm()` predicate (concatenated) | -> SP |
|---|---|---|
| dcDropDownList1 | `AND ISNULL(CPY.ComUnitId, ISNULL(INV.ComUnitId, mas.ComUnitId))='<id>'` | `@Parm` |
| ddlCustomer | `AND CMS.CustomerCode='..'` | |
| ddlCustomerType | `AND CMS.CustomerTypeId='..'` | |
| ddlProduct | `AND INVD.ProductCode='..'` | |
| ddlOrderSender | `AND ISNULL(mas.OrderSenderCode, INV.OrderSenderCode)='..'` | |
| dates | `AND CONVERT(date, CPY.PaymentDate) BETWEEN 'from' AND 'to'` (To empty -> today `dd-MMM-yyyy`) | |
| Group/Zone/Area/Territory/SubTerritory/Market | `CMS.GroupId/RegionId/AreaId/TerritoryId/SubTerritoryId/MarketId` | |
SP: `sp_Get_COGSBasedGPReport(@Parm)` (`CmnCrystaltoView.GetCOGSBasedGPReportDAL`); aliases CPY/INV/mas/CMS/INVD are defined inside the SP (UNKNOWN).

## 9. Dropdown Mapping
Dropdown queries are inline `SELECT DISTINCT` (see §7) via `CmnCrystaltoView.GetCustomerListDAL`, `GetCustomerTypeListDAL`, `GetProductListDAL`, `GetOrderSenderListDAL`; DC list via `OtherStockActionBLL.DCLoad`.

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| SearchButton | `SearchButton_Click` -> `LoadData()` | query, bind, cache 10 min, total |
| cancelButton | `cancelButton_Click` | `Response.Redirect("COGSBasedGPReport.aspx")` |
| btnExport | `btnExport_Click` | ClosedXML workbook "GP_Report", file `COGS_based_GP_Calculation_Report.xlsx` |

## 11. Frontend Flow
First load: dates defaulted, dropdowns loaded, `LoadData()` runs immediately with the default month-to-date range. Search re-runs the SP. Paging (`loadGridView_PageIndexChanging`) re-runs `LoadData()`. `gv_DocumentUpload_PreRender` makes the header a `<thead>`.

## 12. Backend Flow
`LoadData` -> `_DAL_GetGPReportDAL()` (try/catch -> alert of `ex.Message`, empty table) -> if rows: `Cache.Insert("COGSBasedGPReportData_<UserId or Guest>", dt, absolute 10 min)` -> bind grid -> `lblCount = "Total Realization Amount : " + SUM(RealizationAmount).ToString("N2")`; no rows -> empty grid and 0.00. Export: cached table else re-query; re-shape into 15 named columns (Month, Date, Sales Center, BU Wise, Particulars, SKU, Qty, Name of Sales Executive, Realization Amount, VAT, Tax, COGS Amount, Net Amount, GP, GP %) with GP % rendered as `<GPPercent>%` string.

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
No BLL for the report (page calls the DAL directly); `OtherStockActionBLL.DCLoad` for the DC list.

## 15. Repository Mapping
CmnCrystaltoView (`Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs`): GetCOGSBasedGPReportDAL (SP), GetCustomerListDAL/GetCustomerTypeListDAL/GetProductListDAL/GetOrderSenderListDAL (inline text SQL through `DataAccessManager.GetDataTableByText`).

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition |
|---|---|---|
| sp_Get_COGSBasedGPReport(@Parm nvarchar) | CmnCrystaltoView.GetCOGSBasedGPReportDAL | body NOT in repository (GP, COGS, GP% arithmetic UNKNOWN) |

## 17. Database Mapping
Read: dropdown sources `tblCustMaster`, `tblCustomerType`, `tblProduct`, `tblEmpGeneralInfo`. Tables inside the SP (aliases CPY = customer payment, INV = invoice, INVD = invoice detail, CMS = customer master, mas = order/master) are UNKNOWN beyond alias hints. No writes.

## 18. Validation Matrix
No mandatory fields. Errors: exception message shown via `showMessageBox(ex.Message)`. No date sanity check (text is sent to the SP as a string inside the fragment; a malformed date is a SQL error).

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-037 | COGS-based GP report is realised on the PAYMENT date: the date filter is CONVERT(date, CPY.PaymentDate) BETWEEN from AND to (if only From is given, To = today); company unit filter is ISNULL(CPY.ComUnitId, ISNULL(INV.ComUnitId, mas.ComUnitId)); all filters are concatenated into a SQL fragment and passed as @Parm to sp_Get_COGSBasedGPReport. | `Solution.Web/SInventory_UI/COGSBasedGPReport.aspx.cs:184` | COGSBasedGPReport.aspx.cs Parm(); CmnCrystaltoView.GetCOGSBasedGPReportDAL | CONFIRMED |
| BR-M13-038 | GP arithmetic is not in C#: the SP returns Month, Date, ComUnitName, BUWise, Particulars, SKU, Qty, OrderSenderName, RealizationAmount, VATAmount, TaxAmount, COGSAmount, NetAmount, GP, GPPercent (UNKNOWN formulas). The page only totals RealizationAmount ("Total Realization Amount : N2") and the Excel export appends "%" to GPPercent. | `Solution.Web/SInventory_UI/COGSBasedGPReport.aspx.cs:143` | COGSBasedGPReport.aspx.cs LoadData / btnExport_Click | CONFIRMED (page) / UNKNOWN (SP) |
| BR-M13-039 | Search results are cached in ASP.NET Cache for 10 minutes under "COGSBasedGPReportData_<UserId>"; btnExport exports the cached table when present, so an export after changing filters without pressing Search returns the earlier result. Default range = first day of current month to today. | `Solution.Web/SInventory_UI/COGSBasedGPReport.aspx.cs:133` | COGSBasedGPReport.aspx.cs | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).

## 21. Status Lifecycle
Read-only. Status semantics inside SP UNKNOWN.

## 22. Error Handling
Search: try/catch around the DAL -> alert; export: try/catch -> alert (`Response.End()` inside try throws ThreadAbort, caught silently).

## 23. Audit
None.

## 24. Integration
None.

## 25. Reports
On-screen grid + XLSX export (ClosedXML). No Crystal report. See `reports/RPT-M13-06-gp-cogs-reports.md`.

## 26. Dependencies
CmnCrystaltoView, OtherStockActionBLL, IVMarketStructure user control, ClosedXML, ASP.NET Cache.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Open page | grid loaded for current month by payment date | BR-M13-037 |
| TC-02 | Payment on 31st, invoice on 25th of previous month | included when payment date in range | BR-M13-037 |
| TC-03 | Filter by customer + product | `@Parm` includes both predicates | BR-M13-037 |
| TC-04 | Search then change filters, click Export without Search | export shows old result (cache) | BR-M13-039 |
| TC-05 | Empty result | 0.00 total | BR-M13-038 |
| TC-06 | Filter text with a quote | SQL error/alert (injection surface) | GAP-M13-006 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/COGSBasedGPReport.aspx(.cs)`
- `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:169`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| GP formula | none | in SP | not in repo | UNKNOWN | GAP-M13-001 |

## 30. Known Gaps
- GP/COGS/GP% formulas live only in `sp_Get_COGSBasedGPReport` (not in repo) - cannot be verified.
- Filters concatenated into an SQL fragment (injection risk, GAP-M13-006).
- Export may not match on-screen filters because of the cache (BR-M13-039).
- Only RealizationAmount is totalled on screen; no GP/COGS totals.
- `LoadDropDown` swallows all exceptions (`catch { }`).

## 31. Change Impact
- Module M13 (a Tier-B twin of the same DAL: `GpSalesReport`, `TerritoryWise`). `CmnCrystaltoView` is used by ~43 files; changes to its dropdown helpers (`GetCustomerListDAL` etc.) also affect `SalesConfirmationReport_New`.
