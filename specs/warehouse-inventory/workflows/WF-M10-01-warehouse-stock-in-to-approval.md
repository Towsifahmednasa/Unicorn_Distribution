# WF-M10-01 Warehouse stock-in: entry -> review -> approval -> stock-out list

**Module:** M10 | **Confidence:** CONFIRMED for the status flow; the *stock posting after approval* is UNKNOWN (see GAP-M10-003).

## Trigger and actors
- Warehouse operator (maker) opens `WarehouseStockIn.aspx` after receiving goods against a supplier challan.
- Approver (checker) opens `WarehouseStockInApproval.aspx`. No role is enforced in code for either (BR-M10-015).

## Preconditions
Manufacturer, supplier and product masters exist (`tblManufacturer`, `tblSupplierInformation`, `tblProduct`, `tblUnitPrice`, `tblStockUOM`).

## Steps
| # | Page | Handler -> BLL -> DAL -> SQL | Result |
|---|---|---|---|
| 1 | `WarehouseStockIn.aspx` | `Page_Load` -> `InitialGrid/DropDownLoad/PermissionForShowBtn` (SP `sp_GET_btnShow`) | blank form; Save hidden if SP returns rows (BR-M10-008) |
| 2 | same | product autocomplete `code:name` -> `GetProductInGrid` -> `WarehouseStockInBll.ProductInfo` -> `WarehouseStockInDal.LoadProductCode` (`tblProduct/tblUnitPrice/tblStockUOM`) | price=CostPrice, VAT=VATAmountPerUnit (BR-M10-005) |
| 3 | same | qty/price/vat change -> `CalculateTotalValue`, `CalculateQuantityVatAndValue` | line/total amounts (BR-M10-004) |
| 4 | same | `submitButton_Click` -> `Validation` -> `SaveAllData` -> `WarehouseStockInBll.SaveStockInMasterInfo` (id `MAX+1`, code `WHS-nnnnnnn`) -> `WarehouseStockInDal.SaveStockInMasterInformation` (INSERT `tblWHStockInMaster`, Status `Posted`) -> `SaveWarehouseStockInDetail` xN (INSERT `tblWHStockInDetail`) | Posted document (BR-M10-001..003). **Header INSERT text is invalid SQL as committed (GAP-M10-001)** |
| 5 | `WarehouseStockInView.aspx` | `LoadWarehouseStockInData` -> `SELECT * FROM tblWHStockInMaster WHERE Status='Posted' OR 'Reject'`; row `EditData` -> `Session["IDInformation"]` -> redirect to step 1 in edit mode; row `DeleteData` -> `DeleteWhStockInInfoById` (hard delete master+details, no status check) | edit = `UpdateAllData` (update master, delete/insert lines, Status `Posted`; BR-M10-007) |
| 6 | `WarehouseStockInApproval.aspx` | `Page_Load` -> `WarehouseStockInApprovalDal.GetWarehouseStockInInformation` (`Status='Posted'`) | queue (BR-M10-012) |
| 7 | same | `btnSubmit0_Click` -> `Validation` -> `SubmitApproval`: Approve -> (if `MaterialConversionMasterId>0` SP `sp_Acc_MaterialConversionReceive`) -> `ApprovalUpdateDal` UPDATE Status `Approved`, ApproveBy, ApproveDate; Reject -> UPDATE Status `Reject` | BR-M10-013..015 |
| 8 | `WHStockInList.aspx` | `LoadWHStock` -> `Status='Approved'`; row link -> `Session["WHId"]` -> `WarehouseStockOut.aspx` (WF-M10-03) | approved documents available for stock-out |

## Data created / changed
`tblWHStockInMaster` (INSERT; UPDATE Status/UpdateBy/UpdateDate/ApproveBy/ApproveDate; DELETE), `tblWHStockInDetail` (INSERT; DELETE+INSERT on edit; DELETE). No stock table is written by the application code in this flow.

## Status transitions (`tblWHStockInMaster.Status`)
`(new)` -> `Posted` -> `Approved` | `Reject`; `Reject` -> (edit) `Posted`. `WarehouseStockInEdit.aspx` (legacy) sets a blank status (BR-M10-011).

## Branches / exceptions
- Missing header/line data: alerts, no write (BR-M10-001/002).
- Non-numeric qty: unhandled exception (no try/catch).
- Approval of a regular stock-in: only status changes (BR-M10-014); possible `FormatException` on NULL `MaterialConversionMasterId` (GAP-M10-003).
- Any step failing midway leaves partial data (no transactions).

## Postconditions
Approved document visible to stock-out. Availability of the stock in `tblCentralStore` (needed by WF-M10-03) depends on a mechanism outside this code (UNKNOWN).

## Related BR / pages
BR-M10-001..015; PAGE-385, 387, 388, 386, 395. Related: WF-M10-02, WF-M10-03.

## Tests
| ID | Case | Expected |
|---|---|---|
| WF01-T1 | Save valid stock-in | Posted master + details (subject to GAP-M10-001) |
| WF01-T2 | Approve | Status Approved, appears in `WHStockInList` |
| WF01-T3 | Reject then edit and resubmit | back to Posted, reappears in approval |
| WF01-T4 | Delete a Posted document | rows removed |
| WF01-T5 | Same user creates and approves | allowed (BR-M10-015) |
