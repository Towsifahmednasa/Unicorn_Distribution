# WF-M04-03 Unit price creation and change with archive

**Trigger / actors:** pricing administrator via `ProUnitPriceView.aspx` (PAGE-158) or direct to `ProUnitPriceEntry.aspx` (PAGE-157). **Preconditions:** product exists (WF-M04-04).

## A. First price
1. Select product (`sp_GET_ProductNameList`), enter cost, unit price, VAT %, VAT amount, MRP, active date (BR-M04-050).
2. Submit -> `UnitPriceBLL.SaveUnitPrice` -> `HasProductName` (`select * from tblUnitPrice where ProductId`) -> if none: `UnitPriceId = MAX+1`, `INSERT tblUnitPrice` (IsActive=1) (BR-M04-051). Else "Product Price Already Exist !!".

## B. Change price
1. `ProUnitPriceView` Edit sets `Session["UnitPrice"]` and redirects; `ProUnitPriceEntry.Page_Load` loads the row and clears the key (BR-M04-056).
2. Choose "Inactive", enter old price inactive date and the new price fields (BR-M04-052).
3. Submit -> `MoveUnitPriceToRollback(inactiveDate, oldId, LoginName)`: transaction 1 = `INSERT tblUnitPriceRollback ... SELECT` (IsActive 0, InActiveDate, RollbackBy, RollbackDate=GETDATE()) + `DELETE tblUnitPrice`.
4. `SaveUnitPriceEdit`: transaction 2 = `INSERT tblUnitPrice` new row (`MAX+1`, IsActive 1) (BR-M04-053).

## Exceptions
Step 3 failure -> "Inactive Failed: ..." nothing changed. Step 4 failure after step 3 -> product has no price (GAP-M04-020). Empty new fields -> FormatException after archive. Archive never shown (BR-M04-054).

## Postconditions / related
Exactly one live `tblUnitPrice` row per product, joined by product code across stock, challan, invoice, reports (PAGE-157 sec 31). List shows `TPVat = UnitPrice + VATAmountPerUnit` (BR-M04-055). BRs 050-056. **Tests:** duplicate create; replace then query rollback table; force insert failure; refresh in edit mode.
