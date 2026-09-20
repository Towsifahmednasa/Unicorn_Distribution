# WF-M06-05 — Excel order upload → order generation; pending-order re-import

**Pages:** PAGE-225 `OrderListUpload`, PAGE-224 `OrderList`, PAGE-234 `ViewOrderDetailList` (Tier B viewer), PAGE-231 `PendingOrder`. **BR:** BR-M06-069…073. Bodies of `sp_OrderGenerationFromUploadOrder` and `sp_OrderGenerationFromUploadOrder_SingleOrder` are **not in the repository**.

## Trigger & actors
Order desk uploads a spreadsheet of field orders (per manufacturer, per document date); later "generates" real orders, or re-processes orders that are present in the upload/API staging table but missing from `tblOrder`.

## Steps — upload (PAGE-225)
1. `btnUpload_Click` → `ExcelToGrid`: saves the file to `~/ExcelFiles/<filename>` (client file name used as-is), opens it with OleDb (Jet 4.0 for .xls, ACE 12.0 for .xlsx), reads the **first sheet**, keeps rows whose first column is non-empty, binds `loadGridView`.
2. **Submit** → `CheckValidation` (document date, manufacturer, rows exist) → `ExcelUpForOrderListBLL.SaveOrder`: `OrderMasterID = MAX+1` (`ClsPrimaryKeyFind`) → `tblOrderListMaster(OrderMasterID, ManufacId, DocumentDate, GenerateOrder=0, EntryBy=LoginName, EntryDate)`.
3. `XLDataGridToDbByRow`: for each grid row **by fixed column position** (SalesCentre, Name, MIO name, Territory code, FE code, DZSM code, Customer id/name, Product code/name, Qty, Gross, Order code, Submission date, MIO code) → `tblOrderListDetail` insert (parametrised). Errors show `ex.ToString()` in an alert; master row is not rolled back.

## Steps — generate (PAGE-224 OrderList)
4. Search by manufacturer/date (`LoadMigoDate` with `BuildOrderDateFilter`) lists masters with `GenerateOrder=0`.
5. Row **Generate** (`OrderImageButton_Click`) → `TransfarOrderID_BLL/DAL` → `sp_OrderGenerationFromUploadOrder(@OrderMasterID_In)` (UNKNOWN) → "Transferred successfully!!!" if return > 0.
6. Row **Delete** allowed only when `GenerateOrder = false` → deletes master and detail rows (`DeleteData`, `DeleteDetailData`); else "Cant Delete !!! ".

## Steps — pending re-import (PAGE-231)
7. Filters: sales centre (user's DCs), from/to date (all required) → `PendingOrderDal.GetPendingOrderInformation`: distinct `tblOrderListDetail` orders in the date range for that DC whose `OrderCode` is **not in** `tblOrder` within the same range.
8. Row **executeOrder** → `PendingOrderBll.LoadOrderExistOrNotInfo(orderCode)`: if the code is absent from `tblOrder` → find `OrderMasterID` in `tblOrderListDetail` → `sp_OrderGenerationFromUploadOrder_SingleOrder(@OrderMasterID_In,@OrderCode,@IsApiData=1)`. Both success and failure paths set message "Order Processed"; if the order already exists the message is empty.

## Data changed
`tblOrderListMaster`, `tblOrderListDetail` (staging); `tblOrder`/`tblOrderDetail` by the SPs (UNKNOWN details).

## Exceptions / risks
Filename-based save into web folder without extension whitelist beyond `.xls/.xlsx` branches (other extensions leave `oledbConn` null ⇒ NullReference); no per-row validation of numbers/dates (`Convert.ToDecimal/DateTime` → whole upload aborts after master exists); duplicate uploads of the same order codes are not detected on upload; `OrderList.Parameter()` builds SQL text with unclosed quotes (`… ManufacId='X`) — the DAL rewrites it through `BuildOrderDateFilter` (not read, GAP-M06-028).

## Postconditions
Generated orders are expected to satisfy the invoiceable state triple (UNKNOWN — SP body absent).

## Tests
Upload .xlsx with 3 rows → 1 master (GenerateOrder=0) + 3 detail; Generate → orders appear (verify state flags); Delete after generate → refused; Pending: order removed from `tblOrder` re-imports; wrong column order upload → wrong data mapped silently.
