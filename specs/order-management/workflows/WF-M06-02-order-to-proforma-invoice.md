# WF-M06-02 — Order → proforma invoice (credit check, stock issue, batch tag)

**Pages:** PAGE-217 `InvoiceCreationByOrder`, PAGE-218 `InvoiceCreationForCustomerByOrder` (+ clones `DoctorInvoiceCreationByOrder`, `SampleInvoiceCreationByOrder`, `AutoInvoiceCreationByOrder` [broken]). **BR:** BR-M06-019…048. **Cross-module:** M07 invoice tables, inventory `tblDCStore`, accounts DB (credit calc).

## Trigger & actors
Sales-admin/dispatcher at a DC (user restricted to DCs in `tblUserCompanyUnit` for the list; not restricted for PAGE-218).

## Preconditions
Order: `IsInvoice=0`, `ActionStatus` ∈ (`2`,`Accepted`,`Approved`), `IsPrepareforInvoice` 1/NULL, `IsSubDepo` 0/NULL, has a DC (`ComUnitId`) — else assign via WF-M06-06. Customer credit permission = 1 (advisory only).

## Steps
1. PAGE-217 `DropDownlist()`: DCs of user → route list (`LoadDisRouteforInvoice`: only `ActionStatus='2'`) → DA/territory lists → `GridView()`.
2. `GridView()` → `OrderInfoDAL.LoadOrderForOrderCreation` → **`sp_LoadOrderListForOrderCreationbyTerri(@comunitId,@manufacId[=route],@TerritoryId)`** → rows with `Due`, `LimitAmount`, `DayLimit`, `Permission`.
3. Rows with `Permission=1` get "Generate Invoice" (confirm), others "Blocked" (alert only).
4. Click → `Session["OrderId"]=<id>`, `Session["RootNameId"]=0` → `Response.Redirect("InvoiceCreationForCustomerByOrder.aspx")`.
5. PAGE-218 `Page_Load`: `LoadAllDataByOrder`: order + customer (`View_OrderCustomerInfo`), each line → `AddFunc`/`GetProduct` (stock from `View_DCStoreCurrentStock`, price/VAT from `tblUnitPrice`, override by order line price/VAT), stock cap `min(order qty, stock)`, discount by customer category/trade policy/campaign, `TotalValueCalculation`; DA dropdown (`sp_Get_DDLDANameByDCID`); customer credit (`Σ tblReturnAmount`) spread over rows.
6. User picks DA (required), optionally edits delivery charge/remarks/delivery person → **Save**.
7. `saveButton_Click`: `ValiMethod` → `Validation` → `OrderExists` (none) → `InvoiceBLL.SaveInvoice` (insert `tblInvoiceBatch`, insert `tblInvoice`) → fetch invoice id → `SaveInvoiceDetail` → `InvoiceBLL.SaveInvoiceDetails` → per line `UpdateDCStock`: batch-wise allocation, insert `tblInvoiceDetail` row(s) and negative `tblDCStore` 'Issue' rows → `UpdateInvoiceHeaderTotals` → `SaveReturnAmount` (`tblReturnAmount` −credit) → `UpdateInvoiceStatus` (`tblOrder.IsInvoice='True'`).
8. Success alert + form cleared; user may press **Print** → RPT-M06-01 (anomaly check may delete the invoice, BR-048).

## Data changed
`tblInvoiceBatch`+, `tblInvoice`+, `tblInvoiceDetail`+ (n per batch), `tblDCStore`+ (Issue rows), `tblReturnAmount`+, `tblOrder.IsInvoice`, `tblOrderDetail.Status='Undelivered'` for zero-stock lines.

## Alternative path — bulk generation (PAGE-217)
Select orders + DA → `sp_Process_ProformaInvoiceByOrderId(@OrderId,@UserId,@BatchNo1,@DANameId)` per order (body UNKNOWN) → anomaly check → auto-delete bad proformas. Currently unreachable from the UI (selection column hidden — BR-024).

## Branches / exceptions
- Stock 0 ⇒ line dropped (marked Undelivered); qty > stock ⇒ capped; red rows.
- Invoice already exists ⇒ "Invoice already Generated !!".
- Exception during save ⇒ `sp_Deletenvoice(orderId)` compensation (body UNKNOWN) + error alert; **no DB transaction**.
- Anomaly (>10 header/detail difference) ⇒ `sp_Delete_ProformaInvoice`: deletes Issue rows, archives detail (`tblInvoiceDetail_DeleterRecord`), resets `tblOrder.IsInvoice=0`, `tblOrderDetail.Status=NULL`, deletes `tblReturnAmount`/`tblInvoice`, logs `tblInvoiceDeleteLog`.
- Credit block only in PAGE-217 UI.

## Postconditions
Proforma invoice (`DelivaryInvoiceNo NULL`) ready for loading/delivery (WF-M06-03, M07).

## Tests
See PAGE-217 TC-01..08, PAGE-218 TC-01..12. End-to-end: create manual order (WF-01) → list shows → invoice → stock reduced by issue rows → delete proforma → order re-appears in list.
