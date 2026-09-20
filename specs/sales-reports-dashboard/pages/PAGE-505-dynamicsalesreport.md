---
page_id: PAGE-505
module: M13
page: DynamicSalesReport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-505 — DynamicSalesReport

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-505 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | DynamicSalesReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DynamicSalesReport |
| aspx size | 211594 bytes |
| Code-behind LOC | 3784 |
| Inline script size | 2411 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**DynamicSalesReport** ("Details List") - a configurable territory scorecard: per territory it shows invoice and collection amounts by customer category (FCB, Campaign, General, Institution, HQ, OS), doctor counts by program type, DCR and Rx counts, coverage and productivity, with a header-selection checklist and about 30 drill-down modals that list the underlying invoices/collections/doctors/DCRs/prescriptions (each with Excel export). 3 784 lines of code-behind.

## 3. Business Objective
Give sales management one page that joins sales/collection with doctor-call and prescription activity per territory to judge field productivity ("Productivity Invoice-Return", "Productivity Collection", "SFD Rx Coverage"). INFERRED from column names; wiring CONFIRMED.

## 4. Navigation
Inbound references: none in code (menu DB-driven -> UNKNOWN). Outbound: opens `ProformaReportViewer.aspx` (other module) from `viewRptButton_Click`; `cancelButton` -> `ProformaReport.aspx`.

## 5. Route
`~/SInventory_UI/DynamicSalesReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DynamicSalesReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/DynamicSalesReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| BLL class | ProductBLL (Library.BLL/SInventory_BLL/ProductBLL.cs) |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | DoctorDAL (Library.DAL/MasterSetup_DAL/DoctorDAL.cs) |
| DAL class | DoctorVisitDAL (Library.DAL/DoctorVisit_DAL/DoctorVisitDAL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | ProductDAL (Library.DAL/SInventory_DAL/ProductDAL.cs) |
| DAL class | SetupDAL (Library.DAL/DoctorModule_DAL/SetupDAL.cs) |

## 7. UI Control Inventory
Generated inventory (top of table = filter controls; the ~100 grid rows list every column):

Generated inventory (top of table = filter controls; the ~100 grid rows list every column):

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| rbReportType | RadioButtonList |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| rbType | RadioButtonList |  | no validator found |  | Enabled=false |  |  |
| rbCalculationType | RadioButtonList |  | no validator found |  | Enabled=false |  |  |
| rbDTWise | RadioButtonList |  | no validator found |  | Enabled=false |  | OnSelectedIndexChanged→rbDTWise_SelectedIndexChanged |
| dcDropDownList1 | DropDownList |  | no validator found |  | Visible=false | see §9 |  |
| ddlBrandName | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlMonth | DropDownList |  | no validator found |  |  | see §9 |  |
| InvoiceDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| chkRpt | CheckBox | Report Header | no validator found |  |  |  | OnCheckedChanged→chkRpt_OnCheckedChanged |
| cblHeader | CheckBoxList |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→btnShowGrid_Click |
| lblCount | Label | Total Net Amount : 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| _hfTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| gvSelectedItems | GridView |  | no validator found |  |  |  |  |
| gvSelectedItems | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Region | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | BoundField | Area | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | TemplateField | Invoice | no validator found |  |  |  |  |
| lbInvoice | LinkButton | <%#Eval("Invoice") %> | no validator found |  |  |  | OnClick→lbInvoice_Click |
| hfTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Partial Collection | no validator found |  |  |  |  |
| lblPartialCollection | LinkButton | <%#Eval("Partial Collection") %> | no validator found |  |  |  | OnClick→lblPartialCollection_Click |
| (no id) | TemplateField | Full Collection | no validator found |  |  |  |  |
| lblFullCollection | LinkButton | <%#Eval("Full Collection") %> | no validator found |  |  |  | OnClick→lblFullCollection_Click |
| (no id) | TemplateField | FCB Invoice | no validator found |  |  |  |  |
| lblFCBInvoice | LinkButton | <%#Eval("FCB Invoice") %> | no validator found |  |  |  | OnClick→lblFCBInvoice_Click |
| (no id) | TemplateField | FCB Collection | no validator found |  |  |  |  |
| lblFCBCollection | LinkButton | <%# Eval("FCB Collection") %> | no validator found |  |  |  | OnClick→lblFCBCollection_Click |
| (no id) | TemplateField | Campaigns Invoice | no validator found |  |  |  |  |
| lblCampaignsInvoice | LinkButton | <%# Eval("Campaigns Invoice") %> | no validator found |  |  |  | OnClick→lblCampaignsInvoice_Click |
| (no id) | TemplateField | Campaigns Collection | no validator found |  |  |  |  |
| lblCampaignsCollection | LinkButton | <%# Eval("Campaigns Collection") %> | no validator found |  |  |  | OnClick→lblCampaignsCollection_Click |
| (no id) | TemplateField | General Invoice | no validator found |  |  |  |  |
| lblGeneralInvoice | LinkButton | <%# Eval("General Invoice") %> | no validator found |  |  |  | OnClick→lblGeneralInvoice_Click |
| (no id) | TemplateField | General Collection | no validator found |  |  |  |  |
| lblGeneralCollection | LinkButton | <%# Eval("General Collection") %> | no validator found |  |  |  | OnClick→lblGeneralCollection_Click |
| (no id) | TemplateField | Institution Invoice | no validator found |  |  |  |  |
| lblInstitutionInvoice | LinkButton | <%# Eval("Institution Invoice") %> | no validator found |  |  |  | OnClick→lblInstitutionInvoice_Click |
| (no id) | TemplateField | Institution Collection | no validator found |  |  |  |  |
| lblInstitutionCollection | LinkButton | <%# Eval("Institution Collection") %> | no validator found |  |  |  | OnClick→lblInstitutionCollection_Click |
| (no id) | TemplateField | Total Doctor | no validator found |  |  |  |  |
| lblTotalDoctor | LinkButton | <%# Eval("Total Doctor") %> | no validator found |  |  |  | OnClick→lblTotalDoctor_Ckick |
| (no id) | TemplateField | No of BSP | no validator found |  |  |  |  |
| lnkNoOfBSP | LinkButton | <%# Eval("No of BSP") %> | no validator found |  |  |  | OnClick→lblNoofBSP_Ckick |
| (no id) | TemplateField | No of GSP | no validator found |  |  |  |  |
| lnkNoOfGSP | LinkButton | <%# Eval("No of GSP") %> | no validator found |  |  |  | OnClick→lblNoofGSP_Ckick |
| (no id) | TemplateField | No of PSP | no validator found |  |  |  |  |
| lnkNoOfPSP | LinkButton | <%# Eval("No of PSP") %> | no validator found |  |  |  | OnClick→lblNoofPSP_Ckick |
| (no id) | TemplateField | No of GMP | no validator found |  |  |  |  |
| lnkNoOfGMP | LinkButton | <%# Eval("No of GMP") %> | no validator found |  |  |  | OnClick→lblNoofGMP_Ckick |
| (no id) | TemplateField | No of HQ Doctor | no validator found |  |  |  |  |
| lnkNoOfHQDoctor | LinkButton | <%# Eval("No of HQ Doctor") %> | no validator found |  |  |  | OnClick→lblNoOfHQDoctor_Ckick |
| (no id) | TemplateField | No of Ex. HQ Doctor | no validator found |  |  |  |  |
| lnkNoOfExHQDoctor | LinkButton | <%# Eval("No of Ex HQ Doctor") %> | no validator found |  |  |  | OnClick→lblNoOfExHQDoctor_Ckick |
| (no id) | TemplateField | No of OS Doctor | no validator found |  |  |  |  |
| lnkNoOfOSDoctor | LinkButton | <%# Eval("No of OS Doctor") %> | no validator found |  |  |  | OnClick→lblNoOfOsDoctor_Ckick |
| (no id) | TemplateField | Sum of DCR-Total | no validator found |  |  |  |  |
| lnkSumOfDCRTotal | LinkButton | <%# Eval("Sum of DCR-Total") %> | no validator found |  |  |  | OnClick→lblDcr_Ckick |
| (no id) | TemplateField | Sum of DCR-BSP | no validator found |  |  |  |  |
| lnkSumOfDCRBSP | LinkButton | <%# Eval("Sum of DCR-BSP") %> | no validator found |  |  |  | OnClick→lblDcrBsp_Ckick |
| (no id) | TemplateField | Sum of DCR-GSP | no validator found |  |  |  |  |
| lnkSumOfDCRGSP | LinkButton | <%# Eval("Sum of DCR-GSP") %> | no validator found |  |  |  | OnClick→lblDcrGsp_Ckick |
| (no id) | TemplateField | Sum of DCR-PSP | no validator found |  |  |  |  |
| lnkSumOfDCRPSP | LinkButton | <%# Eval("Sum of DCR-PSP") %> | no validator found |  |  |  | OnClick→lblDcrPsp_Ckick |
| (no id) | TemplateField | Sum of GMP-DCR | no validator found |  |  |  |  |
| lnkSumOfGMPDCR | LinkButton | <%# Eval("Sum of GMP-DCR") %> | no validator found |  |  |  | OnClick→lblDcrGmp_Ckick |
| (no id) | TemplateField | GMP-Doctor Coverage monthly | no validator found |  |  |  |  |
| lnkGMPDoctorCoverageMonthly | LinkButton | <%# Eval("GMP-Doctor Coverage monthly") %> | no validator found |  |  |  | OnClick→lblGmpDocotorCoverageMonthly_Ckick |
| (no id) | TemplateField | Doctor Coverage monthly | no validator found |  |  |  |  |
| lnkDoctorCoverageMonthly | LinkButton | <%# Eval("Doctor Coverage monthly") %> | no validator found |  |  |  | OnClick→lblDocotorCoverageMonthly_Ckick |
| (no id) | TemplateField | Sum of Rx Covered | no validator found |  |  |  |  |
| lnkSumOfRxCovered | LinkButton | <%# Eval("Sum of Rx Covered") %> | no validator found |  |  |  | OnClick→lblRxCovered_Ckick |
| (no id) | TemplateField | Sum of Rx Covered-BSP | no validator found |  |  |  |  |
| lnkSumOfRxCoveredBSP | LinkButton | <%# Eval("Sum of Rx Covered-BSP") %> | no validator found |  |  |  | OnClick→lblRxCoveredBSP_Ckick |
| (no id) | TemplateField | Sum of Rx Covered-GSP | no validator found |  |  |  |  |
| lnkSumOfRxCoveredGSP | LinkButton | <%# Eval("Sum of Rx Covered-GSP") %> | no validator found |  |  |  | OnClick→lblRxCoveredGSP_Ckick |
| (no id) | TemplateField | Sum of Rx Covered-PSP | no validator found |  |  |  |  |
| lnkSumOfRxCoveredPSP | LinkButton | <%# Eval("Sum of Rx Covered-PSP") %> | no validator found |  |  |  | OnClick→lblRxCoveredPSP_Ckick |
| (no id) | TemplateField | Sum of Rx Covered-GMP | no validator found |  |  |  |  |
| lnkSumOfRxCoveredGMP | LinkButton | <%# Eval("Sum of Rx Covered-GMP") %> | no validator found |  |  |  | OnClick→lblRxCoveredGMP_Ckick |
| (no id) | TemplateField | No of Dr Rx Prescriber | no validator found |  |  |  |  |
| lnkNoOfDrRxPrescriber | LinkButton | <%# Eval("No of Dr Rx Prescriber") %> | no validator found |  |  |  | OnClick→lblRxPrescriber_Ckick |
| (no id) | TemplateField | GMP Dr.-Rx Prescriber | no validator found |  |  |  |  |
| lnkGMPDrRxPrescriber | LinkButton | <%# Eval("GMP Dr-Rx Prescriber") %> | no validator found |  |  |  | OnClick→lblRxPrescriberGMP_Ckick |
| (no id) | TemplateField | HQ Invoice | no validator found |  |  |  |  |
| lblHQInvoice | Label | <%# Eval("HQ_Invoice") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | HQ Collection | no validator found |  |  |  |  |
| lblHQCollection | Label | <%# Eval("HQ_Collection") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | OS Invoice | no validator found |  |  |  |  |
| lblOSInvoice | Label | <%# Eval("OS_Invoice") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | OS Collection | no validator found |  |  |  |  |
| lblOSCollection | Label | <%# Eval("OS_Collection") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Return (TP+VAT) | no validator found |  |  |  |  |
| lblReturnVAT | Label | <%# Eval("ReturnTP_VAT") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | #Chemist-Coverage-Invoice | no validator found |  |  |  |  |
| lblChemistInvoice | Label | <%# Eval("#Chemist-Coverage-Invoice") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | #Chemist-Coverage-Collection | no validator found |  |  |  |  |
| lblChemistCollection | Label | <%# Eval("#Chemist-Coverage-Collection") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Blue Star Invoice | no validator found |  |  |  |  |
| lblBlueStarInvoice | Label | <%# Eval("Blue Star Invoice") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Green Star Invoice | no validator found |  |  |  |  |
| lblGreenStarInvoice | Label | <%# Eval("Green Star Invoice") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Sales Campaign Invoice | no validator found |  |  |  |  |
| lblSalesCampaignInvoice | Label | <%# Eval("Sales Campaign Invoice") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | No of Dr Rx Prescriber | no validator found |  |  |  |  |
| lblRxPrescriber | Label | <%# Eval("No of Dr Rx Prescriber") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | SFD Rx Coverage | no validator found |  |  |  |  |
| lblSFDRxCoverage | Label | <%# Eval("SFD Rx Coverage") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Productivity Invoice-Return | no validator found |  |  |  |  |
| lblProductivityInvoiceReturn | Label | <%# Eval("Productivity Invoice-Return") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Productivity Collection | no validator found |  |  |  |  |
| lblProductivityCollection | Label | <%# Eval("Productivity Collection") %> | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnPageIndexChanging→loadGridView_PageIndexChanging |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Provider Type | no validator found |  |  |  |  |
| (no id) | BoundField | Pharma Platform | no validator found |  |  |  |  |
| (no id) | BoundField |  Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Number | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Exp Date | no validator found |  |  |  |  |
| (no id) | BoundField | Order Qty | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Special Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Exp. Adjustment | no validator found |  |  |  |  |
| (no id) | BoundField |  Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |

_Truncated at 150 rows; 1218 controls parsed._

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified defaults/conditions**: InvoiceDateTextBox/todateTextBox = today; `rbDTWise` (Date Range Wise default / Month Wise) toggles `divdtRange`/`divmonth`; ddlMonth = current month, ddlYear = current year-10 .. +5; `gvSelectedItems.Visible=false` until Show; `rbType` and `rbCalculationType` are `Enabled=false` (TerritoryWise/UserWise, NetTP/NetTPVAT) and unused by code; `rbReportType` (Summary default / Details), `ddlBrandName` (`ProductBLL.LoadProductSQ`) and `dcDropDownList1` (Visible=false, `DCLoad`) are loaded but not used by the grid query (BR-M13-041). No validators.

**Verified defaults/conditions**: InvoiceDateTextBox/todateTextBox = today; `rbDTWise` (Date Range Wise default / Month Wise) toggles `divdtRange`/`divmonth`; ddlMonth = current month, ddlYear = current year-10 .. +5; `gvSelectedItems.Visible=false` until Show; `rbType` and `rbCalculationType` are `Enabled=false` (TerritoryWise/UserWise, NetTP/NetTPVAT) and unused by code; `rbReportType` (Summary default / Details), `ddlBrandName` (`ProductBLL.LoadProductSQ`) and `dcDropDownList1` (Visible=false, `DCLoad`) are loaded but not used by the grid query (BR-M13-041). No validators.

## 8. Field Mapping
| UI | Used in | Mapped to |
|---|---|---|
| InvoiceDateTextBox / todateTextBox | grid (parsed `DateTime`) and drill-down predicates (text) | @fromdate/@todate; `CONVERT(date,I.InvoiceDate) BETWEEN ..` |
| GroupSelect / ZoneSelect / AreaSelect / TeritorySelect | grid | @grpId, @ZonId, @Area, @Terr |
| SubTeritory / MarketSelect | drill-down predicates only | `mas.SubTerritoryId`, `mas.MarketId` |
| cblHeader items | column visibility | matched by `HeaderText` |
| _hfTerritoryId (row hidden field) | drill-down | `mas.TerritoryId` / `Tr.TerritoryId` / `cc.TerritoryId` |

## 9. Dropdown Mapping
ddlYear/ddlMonth built in code; DC list `OtherStockActionBLL.DCLoad`; brand list `ProductBLL.LoadProductSQ`; market structure user control.

## 10. Button Action Mapping
| Button | Handler |
|---|---|
| submitButton | btnShowGrid_Click (main grid) |
| SearchButton | LoadData -> proforma list `loadGridView` (legacy) |
| viewRptButton | ProformaReportViewer popup |
| btnExport | Excel of legacy list (line 1271) |
| ~30 LinkButtons in grid | `lbInvoice_Click`, `lblTotalDoctor_Ckick`, `lblNoofBSP_Ckick` ..., `lblFullCollection_Click` etc.: read row hidden TerritoryId, show ModalPopupExtender, load list |
| btnExport* (26) | per-modal Excel exports |
| btnClose*/btnCloseModal* | hide modal |

## 11. Frontend Flow
Load defaults; Show builds visible columns then calls SP; click on a number opens an AJAX modal (`mpe_1`, `TotalDoctorModalPopupExtender`, ...) filled server-side (postback in UpdatePanel). `fromDateTextBox_TextChanged` floors the date at 01-Apr-2021 (BR-M13-044).

## 12. Backend Flow
`btnShowGrid_Click` -> `_DAL.GetDynamicSalesReportListDAL(from,to,"",grp,zone,area,terr)` (SP `GetDynamicSalesReportList`, `isBigData=true`) -> bind; empty -> hide grid. Drill-down `Load*` methods: territory id from hidden field + `*Parm` builder -> DAL -> bind modal grid (BR-M13-042). Builders reference dates, group/zone/area/subterritory/market from the live selectors (not from the clicked row), except the territory which is the clicked row's.

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
ProductBLL, OtherStockActionBLL; DAL classes called directly: CmnCrystaltoView, InvoiceDAL, DoctorDAL, SetupDAL, DoctorVisitDAL.

## 15. Repository Mapping
| DAL method | SP |
|---|---|
| CmnCrystaltoView.GetDynamicSalesReportListDAL | GetDynamicSalesReportList |
| GetProformaInvoListDAL | sp_Get_ProformaInvoiceReportList |
| GetFullPaymentDAL | sp_Get_AllSalesReportListParam2 |
| InvoiceDAL.GetNewReceiveableDAl | sp_Get_NewReceiveableList |
| DoctorDAL.GetDoctorList | sp_Get_DoctorList (body in repo) |
| DoctorVisitDAL.GetDCRList | sp_Get_DCRInfoListForView |
| SetupDAL.Get_PrescriptionList | sp_Get_PrescriptionList |

## 16. Stored Procedure Mapping
See §15. Only `sp_Get_DoctorList` is in the repository (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:883`): it builds `SELECT ... WHERE DM.DoctorId is not null and isnull(DM.IsDoctor,0)=1 [+ @Parm +] ORDER BY DM.DoctorCode DESC` and runs it with `sp_executesql` - the fragment is concatenated (injection). All other bodies absent.

## 17. Database Mapping
From the doctor SP (CONFIRMED): tblDoctorMaster (DM), tblEmpGeneralInfo, tblDoctorChemberDetail, tblDoctorContactDetail, tblDoctorDesignation, tblProgramType, tblDoctorType, tblMarket, tblMarketStationDetail, tblStationType, tblRouteInformationMarketDetail/Master, tblCompanyUnit, tblSubTerritory, tblTerritory, tblArea, tblRegion, tbl_Group, tblSMCType, tblUser. Others UNKNOWN. Read-only.

## 18. Validation Matrix
No validation: `Convert.ToDateTime(InvoiceDateTextBox.Text.Trim())` throws on empty/invalid text (btnShowGrid_Click). Date floor 01-Apr-2021 on change.

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-041 | The main DynamicSalesReport grid calls SP GetDynamicSalesReportList(@fromdate,@todate,@Type="",@Area,@Terr,@ZonId,@grpId) using ONLY the two dates and the group/zone/area/territory selectors; rbReportType, rbType, rbCalculationType, rbDTWise, month/year, dcDropDownList1 and ddlBrandName are not passed. Result is hidden when empty. Column visibility is toggled from cblHeader (chkRpt selects all). | `Solution.Web/SInventory_UI/DynamicSalesReport.aspx.cs:103` | DynamicSalesReport.aspx.cs btnShowGrid_Click; CmnCrystaltoView.GetDynamicSalesReportListDAL | CONFIRMED |
| BR-M13-042 | Each numeric cell is a LinkButton that stores the row's hidden TerritoryId in _hfTerritoryId and opens an AJAX modal loaded with a per-metric list: invoices (GetProformaInvoListDAL, InvoiceDate), FCB/General/Institution invoices (CustomerCategoryId 2/1/3), full/partial collections (GetFullPaymentDAL on tblCustPay.custPaymentDate / GetNewReceiveableDAl on InvoiceDate), doctor lists (sp_Get_DoctorList), DCR (GetDCRList) and prescriptions (Get_PrescriptionList). Every filter is a concatenated SQL fragment passed as @Parm. | `Solution.Web/SInventory_UI/DynamicSalesReport.aspx.cs:2041` | DynamicSalesReport.aspx.cs Load*/*Parm | CONFIRMED (page) / UNKNOWN (most SPs) |
| BR-M13-043 | Defect: TotalNoOfHqDoctorParam hard-codes ProgramTypeId="4" but tests `if (ProgramTypeId == "")`, so the program-type predicate is never added; HQ, Ex-HQ and OS doctor drill-downs (LoadNoOfHqDoctor / LoadNoOfExHqDoctor / LoadNoOfOsDoctor) all use this builder and return the same territory doctor list. The Campaigns Invoice/Collection links have empty handlers, and the GMP/Doctor Coverage and Rx Prescriber drill-downs reuse the DCR/Rx builders. | `Solution.Web/SInventory_UI/DynamicSalesReport.aspx.cs:588` | DynamicSalesReport.aspx.cs | CONFIRMED |
| BR-M13-044 | Entering an invoice From-date before 01-Apr-2021 resets it to "01 April, 2021". | `Solution.Web/SInventory_UI/DynamicSalesReport.aspx.cs:1484` | DynamicSalesReport.aspx.cs fromDateTextBox_TextChanged | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).
- No role scoping: any user sees all territories they can select (market-structure control may restrict per role - other module).

## 21. Status Lifecycle
Read-only.

## 22. Error Handling
No try/catch in `btnShowGrid_Click`; `LoadDropDown` swallows errors. Excel handlers: not traced individually.

## 23. Audit
None.

## 24. Integration
None (uses doctor-visit/prescription data of the field-force module).

## 25. Reports
See `reports/RPT-M13-07-dynamic-sales-report.md` and `workflows/WF-M13-06-dynamic-sales-report-drilldown.md`.

## 26. Dependencies
CmnCrystaltoView, InvoiceDAL, DoctorDAL, DoctorVisitDAL, SetupDAL, ClosedXML, EPPlus, AjaxControlToolkit ModalPopupExtender, IVMarketStructure.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Show with default dates | grid rows per territory | BR-M13-041 |
| TC-02 | Change ddlMonth/ddlBrandName only | result unchanged | BR-M13-041 |
| TC-03 | Click Invoice link of a row | modal lists that territory's invoices | BR-M13-042 |
| TC-04 | Click No of HQ / Ex-HQ / OS Doctor | identical lists | BR-M13-043 |
| TC-05 | Click Campaigns Invoice | nothing happens | BR-M13-043 |
| TC-06 | From date 2019 | reset to 01 April 2021 | BR-M13-044 |
| TC-07 | Empty date | exception | GAP-M13-006 |
| TC-08 | Modal export | file equals modal grid | - |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DynamicSalesReport.aspx(.cs)`, `CmnCrystaltoView.cs`, `InvoiceDAL.cs`, `DoctorDAL.cs`, `SetupDAL.cs`, `DoctorVisitDAL.cs`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:883`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| Business rules | none | BR-M13-041..044 | partly SP | PARTIAL | §19 |

## 30. Known Gaps
- Unused controls (month/year, brand, type, calculation type) suggest unfinished features.
- HQ/ExHQ/OS identical; empty campaign handlers; 26 near-identical export handlers and 30 near-identical load methods (duplication).
- Concatenated predicates everywhere (GAP-M13-006); `sp_Get_DoctorList` dynamic SQL.
- `DateTime.Now` appended in SQL text.

## 31. Change Impact
- `sp_Get_DoctorList`, `sp_Get_DCRInfoListForView`, `sp_Get_PrescriptionList` are shared with doctor-master/DCR/prescription pages of other modules.
