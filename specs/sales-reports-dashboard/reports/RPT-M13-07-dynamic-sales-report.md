# RPT-M13-07 Dynamic Sales Report (territory scorecard + drill-downs)

_Module M13. Documentation only, generated from source 2026-09-19. Confidence: CONFIRMED unless tagged. SP bodies are not in the repository._

| List / entry page | Viewer / output page | .rpt or output | Dataset / DataTable name | BLL -> DAL method | SP / inline SQL | Request / session parameters | List-page filters | Tables |
|---|---|---|---|---|---|---|---|---|
| DynamicSalesReport 505 grid `gvSelectedItems` | on-screen (+ Excel per modal via ClosedXML/EPPlus `btnExport*`) | none | - | CmnCrystaltoView.GetDynamicSalesReportListDAL | **GetDynamicSalesReportList** (`GetDataTable(sp, params, isBigData=true)`) | @fromdate,@todate,@Type="",@Area,@Terr,@ZonId,@grpId | dates, group/zone/area/territory (month/year/DC/brand controls ignored) | UNKNOWN |
| drill-down: Invoice / FCB / General / Institution invoice | modal grid | - | - | CmnCrystaltoView.GetProformaInvoListDAL | sp_Get_ProformaInvoiceReportList(@Parm) | territory hidden id, `CustomerCategoryId` 2/1/3 | same | UNKNOWN |
| drill-down: Full / FCB / General / Institution collection | modal | - | - | GetFullPaymentDAL(NewParm,Parm,Parm2) | sp_Get_AllSalesReportListParam2 | date basis tblCustPay.custPaymentDate (and I.UpdateDate) | same | UNKNOWN |
| drill-down: Partial collection | modal | - | - | InvoiceDAL.GetNewReceiveableDAl | sp_Get_NewReceiveableList | InvoiceDate range, cc.TerritoryId | same | UNKNOWN |
| drill-down: doctor lists (Total, BSP, GSP, PSP, GMP, HQ, Ex-HQ, OS) | modal | - | - | DoctorDAL.GetDoctorList | sp_Get_DoctorList(@Parm) - **body in repo, dynamic SQL** | `Tr.TerritoryId`, `DM.EntryDate` range (Total only), program type (BSP/GSP/PSP/GMP builders) | same | tblDoctorMaster (DM), tblEmpGeneralInfo, tblMarket, tblTerritory, tblArea, tblRegion, tbl_Group, tblProgramType, tblDoctorType, ... (SP text) |
| drill-down: DCR / GMP coverage / Doctor coverage | modal | - | - | DoctorVisitDAL.GetDCRList | sp_Get_DCRInfoListForView | @Parm | same | UNKNOWN |
| drill-down: Rx covered / prescribers | modal | - | - | SetupDAL.Get_PrescriptionList | sp_Get_PrescriptionList | @Parm | same | UNKNOWN |

Grid columns (header text order): Region, Zone, Area, Territory Code, Territory, Invoice, Partial/Full Collection, FCB, Campaigns, General, Institution Invoice/Collection, Total Doctor, No of BSP/GSP/PSP/GMP/HQ/Ex-HQ/OS doctors, Sum of DCR (Total/BSP/GSP/PSP/GMP), GMP-Doctor Coverage monthly, Doctor Coverage monthly, Sum of Rx Covered (total/BSP/GSP/PSP/GMP), No of Dr Rx Prescriber, GMP Dr-Rx Prescriber, HQ/OS Invoice/Collection, Return (TP+VAT), #Chemist-Coverage Invoice/Collection, Blue/Green Star Invoice, Sales Campaign Invoice, SFD Rx Coverage, Productivity Invoice-Return, Productivity Collection. All values come from the SP; **no calculation is done on the page**. Rules: BR-M13-041, BR-M13-042, BR-M13-044. Defects: BR-M13-043 (HQ/ExHQ/OS identical, empty campaign handlers), `rbType`/`rbCalculationType` disabled and unused.

