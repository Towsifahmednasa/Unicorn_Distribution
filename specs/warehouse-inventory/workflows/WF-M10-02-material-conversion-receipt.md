# WF-M10-02 Material conversion receipt (production output -> warehouse stock-in)

**Module:** M10 | **Confidence:** CONFIRMED (entry side); approval SP effect UNKNOWN.

## Trigger and actors
Warehouse operator receives finished goods produced in the manufacturing ERP into the warehouse (`MaterialConversionReceive.aspx`); approver uses `WarehouseStockInApproval.aspx`.

## Preconditions
Production database `CSTL_ERPDB_Unicorn` contains conversion masters/details and batch stock for the company/production unit; warehouses exist per company (`tblWearhouse`).

## Steps
| # | Page | Handler -> DAL -> SQL |
|---|---|---|
| 1 | `MaterialConversionReceive.aspx` | company change -> `MaterialConversionReceiveDal.GetProductionUnit` (`CSTL_ERPDB_Unicorn..tbl_MC_ProductionUnit`) and `WarehouseStockInBll.LoadWhInformation` (`tblWearhouse`) |
| 2 | same | production-unit change -> `LoadPendingMaterialConversionList` -> `LoadMaterialConversionList(param)`: conversion detail + batch stock, excluding lines already in a non-rejected `tblWHStockInDetail` (BR-M10-016/017) |
| 3 | same | select lines; qty/cost read-only; mfg edit -> exp = mfg + 2 years (BR-M10-006); totals for selected rows (BR-M10-021) |
| 4 | same | `submitButton_Click` -> `Validation` (BR-M10-018) -> `SaveAllData`: `SaveStockInMasterInfoForMaterialConversion` (INSERT `tblWHStockInMaster` incl. `WarehouseId`, `MaterialConversionMasterId`, Status `Posted`) + `SaveWarehouseStockInDetailWithMaterialConversion` xN (INSERT `tblWHStockInDetail` incl. `MaterialConversionDetailId`) (BR-M10-019) |
| 5 | `MaterialConversionReceiveView.aspx` | list `Posted`/`Reject` with `MaterialConversionMasterId<>0` (`GetMaterialConversionReceiveData`); edit -> `Session["IDInformation"]` -> step 1 (edit: BR-M10-020); print -> `WHStockInReportViewer` |
| 6 | `WarehouseStockInApproval.aspx` | Approve -> `ApproveWarehouseStockInMaster` -> SP `sp_Acc_MaterialConversionReceive(@ApprovalStatus,@MasterId,@ApprovalBy,@ApprovalDate)` then UPDATE Status `Approved` (BR-M10-014) |

## Data changed
`tblWHStockInMaster`, `tblWHStockInDetail`; unknown tables inside the SP (probably stock/accounting).

## Branches
- Reject: status `Reject`; the conversion lines become selectable again (anti-join ignores `Reject`).
- FG with several raw materials may list duplicate rows (GAP-M10-008).
- Parse errors default silently (`TryParse`), EntryBy falls back to `Admin`.

## Postconditions
Conversion output linked to an approved stock-in; cannot be received twice.

## Related
BR-M10-016..021, 014; PAGE-370, 371, 386; GAP-M10-008.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF02-T1 | List after selecting production unit | only unreceived lines |
| WF02-T2 | Save two lines | master + 2 details with MaterialConversionDetailId |
| WF02-T3 | Try to receive same line again | not listed |
| WF02-T4 | Reject, re-list | line listed again |
| WF02-T5 | Approve | SP invoked; verify DB effect (UNKNOWN) |
