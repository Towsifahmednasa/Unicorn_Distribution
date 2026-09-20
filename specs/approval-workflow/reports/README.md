# M18 Approval Workflow — Reports

**There are no reports in this module.** No `RPT-M18-nn` documents are produced.

## Evidence
| Check | Result |
|---|---|
| Grep of `Solution.Web/Approval_UI/*.aspx`, `*.aspx.cs`, `*.ascx(.cs)` for `.rpt`, `ReportViewer`, `Crystal`, `Export`, `.xls` | Only one hit: `DoctorCustomerTransferApproval.aspx.cs` |
| That hit | `loadGridView_RowCommand` "View" opens `../SInventory_RPTVIEW/DcStockOutReportViewer.aspx?DcStockOutMasterId=<id>` (`DoctorCustomerTransferApproval.aspx.cs:142-150`). `loadGridView` is never bound (`DcStockOutViewForApproval()` at :93 is never called), so the button can never be rendered. The report belongs to the stock-out / warehouse flow (other module), not to approvals |
| `.rpt` files loaded by any M18 page | none |
| Any list page exports/prints | none (only on-screen GridView + DataTables client-side search/paging init in `.aspx`) |

## Non-report files in `Solution.Web/Approval_UI/`
The folder also holds four Excel **upload templates** that are referenced by other modules, not by approval pages (grep of `Solution.Web`):
| File | Referenced from |
|---|---|
| `Customer_Payment.xls` | `SInventory_UI/CustomerPaymentExcelUpload.aspx:184` |
| `Deposit Slip.xls` | `SInventory_UI/DepositSlipExcelUpload.aspx:110`, `DepositSlipExcelUpload_old.aspx:110` |
| `TargetEntry.xls` | `SInventory_UI/TargetExcelUpload.aspx:69` |
| `MarketTranferFormat.xls` | `TransferUI/MarketInfoImport.aspx:71` |
And a user control `IVMarketStructureInvoSearchApp.ascx(.cs)` that no page in `Solution.Web` registers (grep of `.aspx`/`.cs` finds only its own code-behind; `CustomerApproveList.aspx` uses `../SInventory_UI/IVMarketStructureInvoSearch.ascx` instead) — dead/duplicate control (GAP-M18-021).

Approval-related reports (e.g. DCR/RX status by `ApprovalStatus`, `CmnCrystaltoView.cs:901-1650`) are consumed by doctor-visit / sales-report modules and are outside M18.
