# RPT-M13-08 Operational and audit reports

_Module M13. Documentation only, generated from source 2026-09-19. Confidence: CONFIRMED unless tagged. SP bodies are not in the repository._

| List / entry page | Viewer / output page | .rpt or output | Dataset / DataTable name | BLL -> DAL method | SP / inline SQL | Request / session parameters | List-page filters | Tables |
|---|---|---|---|---|---|---|---|---|
| PendingSalesConfirmationReport 521 | grid + Excel via InTransitReportViewer (Session["Excel"]="Y") | (viewer in another module) | - | InvoiceDAL (`sp_GET_PendingSalesConfirmationReport`) | sp_GET_PendingSalesConfirmationReport(@Parm) | Session Excel; QS districtId / NationalReport | DC (LoadSC by UserId), dates | UNKNOWN |
| SalesConfirmationReport_New 527 | grid + CSV | none | - | CmnCrystaltoView.GetSalesConfirmationReport_newDAL | sp_Get_AllSalesConfirmationReport_new(@Parm,@Parm2); dropdowns sp_GET_CustomerTypeAll, sp_GET_ProgramTypeListAll | none | DC, market structure, program type, chemist type, customer, product, order sender, dates | UNKNOWN |
| AuditReportOne 492 (deleted orders) | grid via sp_Get_OrderDelTrackingList; popups AuditReportOneReportViewer / DeleteOrderReportViewer | crpAuditReportOne.rpt / crpDelOrder.rpt | AuditReportOneDataTable | OrderTrackingDAL.GetOrderDelTrackingList; AuditReportOneDal.GetDeleteOrderReport/NationalReport | inline SELECT: ProductCode, ProductName, Quantity, TradePrice, TotalTradePrice, OrderCode, SalesCenter, MIO, Territory, FE/DZSM (RSMId/ASMId/MIOId joins on tblEmpGeneralInfo), CustomerCode/Name, GrossValue, SubmissionDate, DeleteBy (tblUser.LoginName), DelDate + `Session["parameter"]` | Session Excel, parameter; QS rptType | ComUnit/national, dates | tblOrderDel, tblOrderDetailDel, tblCustMaster, tblUser, tblEmpGeneralInfo, tblmarket, tblSubTerritory, tblTerritory, tblArea, tblRegion, tbl_Group |
| DoctorApps 504 (app web-view) | grid + export | none | - | CustomerMasterInfoBll.CustomerLedgerBll | sp_CustomerLedger | QS EMPID | customer, dates | UNKNOWN |
| DailyBusinessReport 501 / DashboardOne 465 | empty pages (14 LOC) | - | - | - | - | - | - | - |

Rules: BR-M13-061, BR-M13-066, BR-M13-067. Note AuditReportOne shows who deleted an order and when (audit), but delete is performed in the order module.

