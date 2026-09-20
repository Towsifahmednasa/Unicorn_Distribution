# RPT-M08-03 — Return Invoice print (market return / legacy return invoice)

| Item | Detail |
|---|---|
| Launched from | `SalesReturn.aspx` (PAGE-294 `printButton_Click`, `:698-710`), `SalesReturnList.aspx` and `ReturnInvoiceView.aspx` (`reportData` command → `ReturnInvoiceReportViewer2.aspx`), `ReturnInvoiceCreationForCustomer.aspx` (PAGE-290 print) |
| Viewer | `SInventory_RPTVIEW/ReturnInvoiceReportViewer.aspx(.cs)` (print from entry pages); `ReturnInvoiceReportViewer2.aspx(.cs)` (list pages — not read) |
| .rpt file | `rptReturnInvoiceForCustomer.rpt` (`ReturnInvoiceReportViewer.aspx.cs:50`) |
| Request parameters | QueryString `InvNo` (return invoice **number**, url-decoded; `InvoiceBLL.InvoiceNoCollectionFormate` builds an IN list). The list pages pass `DataKeys[0]` (the key column) instead of the number — which key is bound is not verified |
| Data source | `InvoiceBLL.RInvoiceMainDataForReportBLL`, `RInvoiceDetailDataForReportBLL` → `RInvoiceMainDataForReport/RInvoiceDetailDataForReport` (same shape as `ReturnInvoiceDAL.ReturnInvoiceMainDataForReport` :774-790 and `ReturnInvoiceDetailDataForReport` :792-803); `OrderReportBLL.CompanyInfoBLL()` supplies company header |
| Query summary | main: `tblReturnInvoice ⟕ tblCompanyUnit ⟕ tblCustMaster ⟕ tblPaymentType ⟕ tblMIAInfo ⟕ tblUser ⟕ tblCustCategory` filtered `ReturnInvoiceNo IN (@params)` (parameterised via `BuildInClause`); detail: `tblReturnInvoiceDetail ⟕ tblReturnInvoice` |
| Dataset tables | `mainDataTable`, `detailDataTable`, `companyInfoDataTable` |
| Output | Crystal viewer (PDF export commented out) |
| Notes | The visible SELECT column list contains TpTotal/TpVat/TpDiscount/TpGrandTotal; whether `DeductionCharge`/`NetReturnAmount` are also selected is UNKNOWN (rest of the SELECT was not reviewed). `IV.UserId` join gives a blank user for PAGE-294 returns because `spInsertReturnInvoice_new` does not set `UserId`. |
