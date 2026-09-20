---
page_id: PAGE-157
module: M04
page: ProUnitPriceEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "PARTIAL", "database": "COMPLETE", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-157 - ProUnitPriceEntry (product price master: create, supersede with history)

> **Evidence level:** Tier A. Read in full: `ProUnitPriceEntry.aspx.cs` (350 lines), `Library.BLL/SInventory_BLL/UnitPriceBLL.cs`, `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs` (all inline SQL visible), `SP_Create_tblUnitPriceRollback.sql` (repo root, DDL), `ProUnitPriceView.aspx.cs`, `ProUnitPriceEdit.aspx.cs`. Database dimension COMPLETE for this page because every statement is inline SQL in the repo (the only SP is the drop-down loader `sp_GET_ProductNameList`).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-157 |
| Module | M04 |
| Page name | ProUnitPriceEntry |
| Kind | Entry / Transaction (new price; replace price with archive) |
| Web folder / master | SInventory_UI / NewMasterPage |
| Page class | SInventory_UI_ProUnitPriceEntry |
| Files | aspx 26,419 bytes; aspx.cs 350 lines |
| Suspected legacy/duplicate | NO - live. The separate pop-up editor `ProUnitPriceEdit.aspx` (PAGE-156) is effectively dead (section 31) |

## 2. Purpose
Maintain the **unit price record of a product**: cost price, unit (trade) price, VAT %, VAT amount per unit, MRP, and the date the price becomes active. A price is replaced by entering a new price together with the inactive date of the old one; the old row is archived in `tblUnitPriceRollback`. CONFIRMED.

## 3. Business Objective
Provide the single authoritative price (`tblUnitPrice` active row) that stock valuation, challans, orders and invoices join on by product code. INFERRED from the many `tblUnitPrice` joins (`ChalanDAL`, `InvoiceDAL`, `OrderInfoDAL.ProductVat`, ...). "Rollback" in the name is an archive of superseded prices, not an undo (BR-M04-055).

## 4. Navigation
Inbound: `ProUnitPriceView.aspx` (edit: sets `Session["UnitPrice"]=<UnitPriceId>` then redirects to this page without querystring, `ProUnitPriceView.aspx.cs:36-42`; new: redirect). Outbound: `ProUnitPriceView.aspx`.

## 5. Route
`~/SInventory_UI/ProUnitPriceEntry.aspx` (edit mode is entered through the session key, not the URL).

## 6. Source Files
View/code-behind above; `UnitPriceBLL`, `ProductUnitPriceDAL`, `SeedDataDAL.GetProductNameList`, `SInventorySql` helper, `ClsPrimaryKeyFind`, DAO `ProductUnitPrice`.

## 7. UI Control Inventory
| Control | Label | Required (server) | Notes |
|---|---|---|---|
| ddlProductName | Product | YES `:103` "Please Select Product!!" | `sp_GET_ProductNameList`; selecting fills code/name/pack size via `GetProductInfo` |
| productCodeTextBox | Product Code | YES `:109` | AutoPostBack; `productCodeTextBox_TextChanged` looks up `tblProduct` by **ProductId** (the method is `LoadProduct(productId)` -> `SELECT * FROM tblProduct where ProductId=@ProductId`, `ProductUnitPriceDAL.cs:269-278`) although the field is named code |
| productNameTextBox | Product Name | - | read-only; accepts "code : name" input (`Split(':')[0]`) |
| packSizeTextBox | Pack Size | - | read-only |
| costPriceTextBox | Cost Price | YES `:115` | free decimal text (FilteredTextBox) |
| unitPriceTextBox | Unit Price | YES `:122` | - |
| vatperTextBox | VAT % | YES `:130` | default "0" |
| vatAmountTextBox | VAT Amount (per unit) | YES `:138` | **entered manually, no auto calculation from VAT %** |
| txtMRP | MRP | YES `:146` | used for both new and replacement price |
| activeDtTextBox | Active Date | **not validated** | pickadate; `Convert.ToDateTime` on create throws if empty |
| RadioButtonList1 | Active / Inactive | - | "Inactive" allowed only in edit mode (BR-M04-052) |
| @new panel: newCostPriceTextBox, newUnitPriceTextBox, newvatPerceTextBox, newvatAmountTextBox, newactiveDtTextBox, inactiveTextBox | New price fields + Inactive date of old price | inactiveTextBox YES `:195`; the `new*` fields are **not validated** | shown when Inactive selected in edit mode |
| submitButton / ResetBtn / detailsViewButton | Submit / Reset / Back | - | - |

## 8. Field Mapping
UI -> `ProductUnitPrice` -> `tblUnitPrice` (CONFIRMED `ProductUnitPriceDAL.cs:19-35`):
`ProductId` (hidden), `ProductCode`, `ProductName`, `PackSize`, `CostPrice`, `UnitPrice`, `VATPercentage`, `VATAmountPerUnit`, `MRPPrice`, `IsActive` (= true always), `ActiveDate`; `UnitPriceId` = `MAX(UnitPriceId)+1`. Replacement: `new*` fields -> same columns; `MRPPrice` <- `txtMRP` (single field); archive row -> `tblUnitPriceRollback` (BR-M04-053).

## 9. Dropdown Mapping
`ddlProductName` <- `SeedDataDAL.GetProductNameList` (`sp_GET_ProductNameList`; ProductId/ProductName).

## 10. Button Action Mapping
| Event | Handler | Effect |
|---|---|---|
| ddlProductName change | `:341` | `GetProductInfo(ProductId)` |
| productCodeTextBox / productNameTextBox change | `:297`,`:303` | same lookup; not-found alert "Product Information Not Found!!" |
| RadioButtonList1 change | `:317` | Inactive + existing record => show new-price panel; Inactive without record => alert "Use Edit Mode to use that function " and clear |
| submitButton | `submitButton_Click1` `:157` | create or replace (below) |

## 11. Frontend Flow
Server-postback driven; `pageLoad()` initialises pickadate/select2. No client price calculations.

## 12. Backend Flow
`Page_Load`: bind product list; if `Session["UnitPrice"]` set => `unitPriceIdHiddenField` = it, `ProductEditLoad` (fills fields from `SELECT * FROM tblUnitPrice WHERE UnitPriceId=@`, sets radio to Active/Inactive by `IsActive`), then **clears the session key** (a refresh loses edit mode).
`submitButton_Click1`: `Validation()`; if `unitPriceIdHiddenField` empty => **create**: build `ProductUnitPrice` (IsActive=true, ActiveDate from textbox) -> `UnitPriceBLL.SaveUnitPrice`: `HasProductName` = `select * from tblUnitPrice where ProductId=@ProductId` => if any row exists (active or not) return false => alert "Product Price Already Exist !!"; else `UnitPriceId=MAX+1`, INSERT; alert "Data Save Successfully  Product Name  is :  <name> And  Product Price is :<price>", clear form.
Else **replace**: require `inactiveTextBox` ("Please Input Inactive Date!!"); build new price from `new*` fields; `MoveUnitPriceToRollback(inactiveDate, oldUnitPriceId, Session["LoginName"])` (transaction 1: INSERT..SELECT into `tblUnitPriceRollback` with IsActive=0, InActiveDate, RollbackBy, RollbackDate=GETDATE(); throws "Unit price not found!" when no source row; then DELETE from `tblUnitPrice`; commit) then `SaveUnitPriceEdit` (transaction 2: `UnitPriceId = MAX+1`, INSERT new active row). Errors of step 1 => alert "Inactive Failed: <msg>" and stop.

## 13. API Mapping
None.

## 14. Service Mapping
`UnitPriceBLL.SaveUnitPrice`, `SaveUnitPriceEdit`, `MoveUnitPriceToRollback`, `ProductUnitPriceEditLoad`, `ProductInfo`; unused `UpdateActive`, `UpdateDataForProductUnitPrice`.

## 15. Repository Mapping
`ProductUnitPriceDAL`: `SaveProductUnitPrice`, `HasProductName`, `ProductUnitPriceEditLoad`, `MoveUnitPriceToRollback`, `LoadProduct`, `LoadUnitPriceView`, `GetProductPriceReportInfo`, `UpdateActive`, `UpdateCustCategoryInfo`. `MoveUnitPriceToRollback` builds its own connection string from the hard-coded `SqlUserAccess` credentials (contains a live SQL login; not repeated here) instead of `DataAccessManager`.

## 16. Stored Procedure Mapping
Only `sp_GET_ProductNameList` (seed, body UNKNOWN). Everything else is inline SQL.

## 17. Database Mapping
| Table | Access | Detail |
|---|---|---|
| tblUnitPrice | INSERT, SELECT, DELETE (replace), UPDATE (legacy edit page) | UnitPriceId, ProductId, ProductCode, ProductName, PackSize, CostPrice, UnitPrice, VATPercentage, VATAmountPerUnit, MRPPrice, IsActive, ActiveDate, InActiveDate |
| tblUnitPriceRollback | INSERT..SELECT | UnitPriceId, ProductId, ProductCode, ProductName, PackSize, CostPrice, UnitPrice, VATPercentage, VATAmountPerUnit, MRPPrice, IsActive(0), ActiveDate, InActiveDate, RollbackBy, RollbackDate; extra columns `RollbackId` IDENTITY PK, index on ProductId (DDL `SP_Create_tblUnitPriceRollback.sql`) |
| tblProduct | SELECT | product lookup |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| product, code, cost, unit price, VAT %, VAT amount, MRP | non-empty (checked on the old-price controls even in replace mode) | `Validation()` `:100-155` | "Please Select Product!!", "Please Input Product Code!!", "...Cost Price!!", "...Unit Price!!", "...Vat!!", "...Vat Amount!!", "Please Input MRP!!" | BR-M04-050 |
| Active Date | none (create) / `newactiveDtTextBox` none (replace) | - | unhandled FormatException | BR-M04-051 |
| Inactive Date | required in replace | `:195` | "Please Input Inactive Date!!" | BR-M04-053 |
| numeric format, negative, cost > price, VAT consistency | none | - | - | BR-M04-050 |
| duplicate price per product | any existing row | `HasProductName` | "Product Price Already Exist !!" | BR-M04-051 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M04-050 | A price record needs product, cost price, unit price, VAT %, VAT amount and MRP; values are taken as typed: **there is no formula** linking unit price, VAT % and VAT amount, and no numeric-range checks (negative, cost > unit price, MRP < unit price all accepted). Values parsed with `Convert.ToDecimal`; bad text throws. In replace mode the same old-price fields are validated even though the `new*` fields drive the save. | ProUnitPriceEntry.aspx.cs:100-155,165-179,201-216 | `Validation`,`submitButton_Click1` | CONFIRMED |
| BR-M04-051 | One price row per product on create: any existing `tblUnitPrice` row for the ProductId (regardless of `IsActive`) blocks a new price ("Product Price Already Exist !!"). A new price for a product that already has one is possible only through the replace flow. The Active Date must be typed (unvalidated; empty => `Convert.ToDateTime` exception). New prices are always `IsActive = 1`; the page never writes `InActiveDate` on the active row. | ProductUnitPriceDAL.cs:57-64; UnitPriceBLL.cs:21-33; ProUnitPriceEntry.aspx.cs:161-192 | `SaveUnitPrice` | CONFIRMED |
| BR-M04-052 | Price replace (supersede) is entered from the edit view: open the row, choose "Inactive" (allowed only when a price id is loaded), give the old price's inactive date and the new price fields + new active date. A new price cannot be scheduled independently of retiring the old (same click). | ProUnitPriceEntry.aspx.cs:193-241,317-339 | radio + submit | CONFIRMED |
| BR-M04-053 | **Rollback/archive:** on replace, the old row is copied to `tblUnitPriceRollback` (IsActive forced 0, `InActiveDate` = entered date, `RollbackBy` = `Session["LoginName"]` or empty, `RollbackDate` = server time) and **deleted** from `tblUnitPrice` inside one transaction; if the source row is missing it aborts with "Unit price not found!". The new price is then inserted in a **second, independent** transaction with `UnitPriceId = MAX+1`. If the insert fails after step 1 committed the product is left with **no price row** and the old price only in the archive. The dates are not compared (Inactive date may be after the new Active Date, leaving overlap or gap; only one row is ever live). | ProductUnitPriceDAL.cs:220-260; UnitPriceBLL.cs:70-91; ProUnitPriceEntry.aspx.cs:218-241 | `MoveUnitPriceToRollback`,`SaveUnitPriceEdit` | CONFIRMED |
| BR-M04-054 | No page reads `tblUnitPriceRollback`: price history is stored but never displayed; `ProductPriceReport`/`ProductCostPriceReport` list `tblUnitPrice` only (`GetProductPriceReportInfo` lists all rows, `LoadUnitPriceView` only `IsActive='1'`). Therefore "price history/rollback" cannot be restored or viewed from the application. | grep `tblUnitPriceRollback` = ProductUnitPriceDAL.cs, ProUnitPriceEntry.aspx.cs, SP_Create_tblUnitPriceRollback.sql | - | CONFIRMED |
| BR-M04-055 | List/View price presentation: `TPVat = UnitPrice + VATAmountPerUnit` (trade price incl. VAT) computed in SQL for `ProUnitPriceView` and the cost/trade price reports; downstream sales screens take `UnitPrice` and `VATPercentage` from `tblUnitPrice WHERE ProductCode=@ProductCode` with **no IsActive filter** in `OrderInfoDAL.ProductVat` (`:952`), i.e. they rely on there being exactly one row per product (guaranteed only by BR-M04-051/053). | ProductUnitPriceDAL.cs:69-80; OrderInfoDAL.cs:950-957 | - | CONFIRMED |
| BR-M04-056 | Edit mode is passed through `Session["UnitPrice"]` and cleared on first load; a browser refresh or new tab after that opens the page in create mode. | ProUnitPriceEntry.aspx.cs:36-41; ProUnitPriceView.aspx.cs:36-42 | `Page_Load` | CONFIRMED |

## 20. Permission Matrix
Session: `LoginName` (RollbackBy) and the transient `UnitPrice` key. No role checks; **any menu user can change prices**. Roles UNKNOWN.

## 21. Status Lifecycle
`tblUnitPrice.IsActive`: created 1; never set to 0 by this page (rows are moved out instead). Lifecycle: `Active row -> (replace) -> archived in tblUnitPriceRollback (IsActive=0, InActiveDate) + new Active row`. The legacy `UpdateActive` (sets IsActive=0, InActiveDate) exists but is unused. `LoadUnitPriceView` shows only `IsActive='1'`.

## 22. Error Handling
Validation via alerts. Replace step 1 wrapped in try/catch (alert "Inactive Failed: ..."). Create/replace insert not wrapped: exceptions (bad decimal/date, PK collision) hit the default error page. `SaveProductUnitPrice` result ignored (BLL returns true).

## 23. Audit
Only `RollbackBy` (login name) and `RollbackDate` on archived rows; the live row carries no creator/updater columns. No log of who created a price.

## 24. Integration
None directly; prices are read by stock/valuation/SAP-related DALs (see section 31).

## 25. Reports
Feeds `ProductPriceReport` (PAGE-148 -> viewer PAGE-128) and `ProductCostPriceReport` (PAGE-144 -> viewer PAGE-127); see README reports index.

## 26. Dependencies
BLL `UnitPriceBLL`; DAL `ProductUnitPriceDAL`, `SeedDataDAL`, `SInventorySql`; DAO `ProductUnitPrice`; `SqlUserAccess` (hard-coded credentials); pickadate.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | New price, all fields, product without price | Row inserted IsActive=1, UnitPriceId=max+1 | BR-M04-050/051 |
| TC-02 | New price for a product that has any price row | "Product Price Already Exist !!" | BR-M04-051 |
| TC-03 | Leave Active Date empty | Unhandled FormatException | BR-M04-051 |
| TC-04 | Choose Inactive without loading an existing price | Alert "Use Edit Mode to use that function " | BR-M04-052 |
| TC-05 | Edit row, Inactive, new price + inactive date | Old row in `tblUnitPriceRollback` (RollbackBy=login), deleted live, new row inserted | BR-M04-053 |
| TC-06 | As TC-05 but new price fields empty | Old row archived+deleted then FormatException; product has no price | BR-M04-053 |
| TC-07 | Edit then refresh page | Opens in create mode | BR-M04-056 |
| TC-08 | Cost 500, unit price 100, VAT amount 999 | Saved (no checks) | BR-M04-050 |
| TC-09 | Search UI/DB for archived prices | No screen shows them | BR-M04-054 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/ProUnitPriceEntry.aspx(.cs)`, `ProUnitPriceView.aspx.cs`, `ProUnitPriceEdit.aspx.cs`, `Library.BLL/SInventory_BLL/UnitPriceBLL.cs`, `Library.DAL/SInventory_DAL/ProductUnitPriceDAL.cs`, `SP_Create_tblUnitPriceRollback.sql`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | read | DDL of rollback table in repo | MISSING | - |
| Price history | None existed | archived, not viewable | table exists (per DDL script; live presence UNKNOWN) | INCOMPLETE FEATURE | BR-M04-054 |

## 30. Known Gaps
Two-transaction price swap (GAP-M04-020). Hard-coded DB credential used to open a raw connection (GAP-M04-021). No numeric/date validation, VAT not derived (GAP-M04-022). No audit on the live row. `ProUnitPriceEdit` broken/dead (GAP-M04-023). No history viewer (GAP-M04-024).

## 31. Change Impact
`tblUnitPrice` is joined by `ChalanDAL`, `ChallanReportViewDAL`, `CompanySalesReportDAL`, `DCStoreDAL`, `CentralStoreDAL`, `InvoiceDAL`, `OrderInfoDAL(.SalesReturn)`, `OtherStockActionDAL`, `ExcelUpFor*DAL` and price/cost reports (grep `tblUnitPrice`). Changing price semantics (e.g. allowing several rows per product) breaks those joins, which take any/first row by product code. `ProUnitPriceEdit.aspx` (PAGE-156, unreachable: `ProUnitPriceView.PopUp()` is never called) sets `CostPrice = unitPriceTextBox.Text` and calls `UpdateDataForProductUnitPrice`, which returns false whenever the product already has a price (i.e. always) - so it never updates.
