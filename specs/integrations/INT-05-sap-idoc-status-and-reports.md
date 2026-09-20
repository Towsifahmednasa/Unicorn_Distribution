# INT-05 - SAP status dashboard and SAP-format reports - ERP-side monitoring

| Item | Detail |
|---|---|
| Direction | Read-only view of ERP->SAP hand-off state |
| Transport | Database only. `SInventory_UI/SAPDashboard.aspx` (PAGE-692): month/year (2022-2030) -> `CmnCrystaltoView.GetSalesConfirmStatusByDateReportDAL` (`sp_RPT_SalesConfirmStatusByDate @Month,@Year`), `GetSalesReturnReportDAL` (`sp_RPT_SalesReturnStatusByDate`), `GetChallanStatusByDateDAL` (`sp_RPT_ChallanStatusByDate`) - `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:352-415`. |
| Fields shown | Sales / return: `SalesDocDate, HasIdoc ("IDoc Received?"), SendDate, Qty, Discount, TP, VAT`; challan: `ChalanDate, ChalanNo, SAP_ChallanSend ("Send to SAP"), SAP_ChallanRecevie ("Confirmation to SAP")`. The IDoc flag implies SAP returns IDoc acknowledgements that are stored by an external process (INFERRED). |
| Other | `SalesReturnReportSAP.aspx` (PAGE-691): `sp_LoadSalesReturnReportSAP(@fromdate,@todate)`, `.xls` export. Sales-return SPs carry `Mio_SapCode_New` (repo scripts `SP_SalesReturn_Related.sql`). |
| Trigger | Manual search. Retry/resend: none from this page. |
| Authentication / errors | Session login only; `LoadData` catch shows alert with message. Bodies of the three status SPs UNKNOWN. BR-M17-022. |
