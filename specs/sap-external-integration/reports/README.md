# M17 reports

M17 has report-type pages but no logic-heavy report family; details are captured in the integration files.

| Report | Page | Data / output | Where documented |
|---|---|---|---|
| MIGO reports (STO, Order Detail, MIGO) | `SInventory_UI/MIGOReport.aspx` -> `SInventory_RPTVIEW/MIGOReportViewer.aspx?fromDate&toDate&rpttype=sto/OD/MIGO&custcode&orderno` | Crystal `crpMigoDetailsReport.rpt`, `crpOrderDetailsReport.rpt`, `crpMainMigoReport.rpt` in `Solution.Web/Reports/CrystalReports/`; datasets `StoDataTable`, `orderDetailsDataTable`, `migoDataTable`, `companyInfoDataTable`; inline SQL in `ExcelUpForMIGODAL.LoadMigoReport/LoadOrderDetail/LoadMainMigoReport` (`Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:134-172`); exported straight to Excel (`ExportFormatType.ExcelRecord`); "No Data Found!!" label when empty | INT-07 |
| Sales Return Report (SAP) | `SInventory_UI/SalesReturnReportSAP.aspx` | `sp_LoadSalesReturnReportSAP(@fromdate,@todate)`, `.xls` export | INT-05 |
| SAP Dashboard | `SInventory_UI/SAPDashboard.aspx` | three grids from `sp_RPT_*StatusByDate` | INT-05 |
| Bank deposit CSV | `BankDepositSAP.aspx` | `sp_SAP_BankDepositPosting` | INT-04 |

No dedicated `RPT-M17-nn` files were created (no report has independent rules beyond these).
