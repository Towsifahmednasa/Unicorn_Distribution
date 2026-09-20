# INT-04 - Bank deposit posting to SAP - outbound (currently disabled)

| Item | Detail |
|---|---|
| Direction | ERP -> SAP finance |
| Transport | Intended: HTTPS POST, JSON, HTTP Basic auth, to a SAP PI/PO RESTAdapter endpoint (`.../RESTAdapter/eph_mio`, external host `smc-bd.org` port 42223) - `Solution.Web/SInventory_UI/BankDepositSAP.aspx.cs:276-291`. The whole `HttpClient.PostAsync` block is **commented out**. Working path today: CSV export. |
| Trigger | PAGE-686: View (runs `sp_SAP_BankDeposit_SAP_Process(@fromdate,@todate)` then reads `sp_SAP_BankDepositPosting(@FrmDate,@ToDate)`); Export CSV; Final Submit (builds JSON, sends nothing). |
| Payload | `{"Posting":[{"CustomerCode","Amount","ValueDate","BankAccountNo","Reference","CQNumber"}]}` (all strings; `ValueDate` = `DepositDate`). |
| Tables / SPs | `sp_SAP_BankDeposit_SAP_Process`, `sp_SAP_BankDepositPosting`, `sp_SAP_BankDepositSendtoSAP(@Ids)` (would mark rows sent; commented out). Bodies UNKNOWN. |
| Status flags / retry | Sent flag never set (call disabled) -> same rows re-listed; no retry. |
| Authentication | Basic auth username/password **hard-coded in the source** (`BankDepositSAP.aspx.cs:277`) - `<redacted>`; treat as leaked. |
| Error handling | `LoadInfo` empty catch; Final Submit catch shows raw message in `alert`. |
| Related | Excel uploads map territory/bank to SAP codes (`sp_Get_SAP_EmpSApCodebyTerritory`, `sp_Get_BankSAPMappingAccounTNo`) in `DepositSlipExcelUpload`/`CustomerPaymentExcelUpload` - see INT-13. BR-M17-013/014. |
