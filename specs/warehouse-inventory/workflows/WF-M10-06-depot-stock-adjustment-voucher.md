# WF-M10-06 Depot stock adjustment voucher (DC stock-out) with approval

**Module:** M10 | **Confidence:** CONFIRMED for entry and status flow; stock effect of approval UNKNOWN (SP body missing).

## Trigger and actors
DC/depot staff raise a voucher to remove stock (reason, optional invoice/customer) on `DepotStockAdjustmentsVoucher.aspx`; approver uses `DcStockOutApproval.aspx`; `DepotStockAdjustmentsVoucherView.aspx` lists/prints/deletes.

## Preconditions
DC (`tblCompanyUnit`), product, and DC stock rows in `tblDCStore` with an active `tblUnitPrice`.

## Steps
| # | Page | Handler -> BLL -> DAL -> SQL |
|---|---|---|
| 1 | `DepotStockAdjustmentsVoucher.aspx` | choose DC -> `Session["ComUnitId"]`, code text; invoice autocomplete `SInventoryWebService.GetProformaInvoiceNoNew` (`id:no`, `0:N/A`) |
| 2 | same | product + `Search Product` -> `SubDepotChalanBLL.GetProductDcStoreSubdeport` -> `SubDepotChalanDAL` (`tblDCStore` by DC code and ProductId, active price) (BR-M10-044) |
| 3 | same | qty per ticked row (BR-M10-045) -> Add -> staged `chalanGridView` |
| 4 | same | `submitButton_Click1` -> `Validation` (BR-M10-043) -> `DeStockOutBll.SaveDataForDcStockOutMasterBll` (INSERT `tblDeStockOutMaster`, Status `Posted`) -> `SaveDataForStockOutDetailBll` (INSERT `tblDeStockOutDetails` xN) |
| 5 | `DcStockOutApproval.aspx` | queue `Status='Posted'`; Approve -> `UpdateStockOutMasterDataForApprovalBll` -> SP `sp_UD_DcStockOutApproval`; Reject -> `UPDATE tblDeStockOutMaster SET Status='Reject',...` (BR-M10-047) |
| 6 | `DepotStockAdjustmentsVoucherView.aspx` | delete allowed unless `Status='Approved'`: `DcStockOutMasterDelete` + `DcStockOutDetailsDelete` (BR-M10-046); print `DcStockOutReportViewer` (`crpDcStockOut.rpt`, RPT-M10-04) |

## Data changed
`tblDeStockOutMaster`, `tblDeStockOutDetails`; `tblDCStore` only through the approval SP (UNKNOWN).

## Branches
Invoice `N/A` allowed; all DCs selectable; only `Approved` vouchers are protected from deletion (rejected vouchers can be deleted); the creator can approve their own voucher.

## Postconditions
Approved voucher (stock deducted if SP does so - UNKNOWN); printable report.

## Related
BR-M10-043..048; PAGE-367, 368, 363; GAP-M10-011.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF06-T1 | Save voucher | Posted, tblDCStore unchanged |
| WF06-T2 | Approve | Status Approved; check `tblDCStore` effect |
| WF06-T3 | Delete approved | blocked |
| WF06-T4 | Reject then delete | deleted |
