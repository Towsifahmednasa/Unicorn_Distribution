# INT-11 - `SInventoryWebService` (ASMX autocomplete service)

| Item | Detail |
|---|---|
| Implementation | `Solution.Web/App_Code/SInventoryWebService.cs` (596 lines), `[WebService]`, `[ScriptService]`. Three identical `.asmx` shells (`SInventory_UI/`, `MasterSetup_UI/`, `SubDepot_UI/SInventoryWebService.asmx`) point at `~/App_Code/SInventoryWebService.cs`. |
| Direction / protocol | Browser -> ERP, SOAP/HTTP-POST/JSON (script service). Used by AjaxControlToolkit AutoComplete (`ServiceMethod=`) in ~70 references: `GetProductWithCode` (15), `GetCustomer` (12), `GetProduct2` (8), `GetCustomer_New` (7), `GetCustomer_ALL_new` (5), `GetCustomer_ALL` (4), `GetCustomernew`, `GetAllInvoice` (3 each), and others. |
| Methods (23) | Customer, product, invoice, batch, employee, doctor searches; each runs inline SQL through `SqlDataAdapter` (`SolutionConnectionStringSSIDB`), `LIKE @prefixText` parameterised (`%text%`). |
| Data scoping | `GetCustomer`: Admin or empty `ComUnitId` -> all active customers (TOP 20); else `WHERE ComUnitId=@ComUnitId` from `Session["ComUnitId"]` (BR-M17-023). `GetEmpInfo`, `GetCustomer_WithoutGeneral` also use `UserType`/`ComUnitId`. |
| Authentication | `EnableSession=true` on many, but no method rejects a missing session; `GetProductList`, `GetProductWithCode`, `GetProduct2/3`, `GetProduct`, `GetProductByMenufracturer`, `HelloWorld` do not use Session at all -> anonymous product lookups (INFERRED reachability, no `<authorization>` in `Web.config`). |
| Injection risks | Concatenated: `GetPreBatch` (`Session["ProductId"]`), `GetAllInvoice` (`Session["CompIDD"]`), `GetProductByMenufracturer` (`contextKey` parsed with `Convert.ToInt32`, safe), `GetCustomernew` appends `Session["MOParam"]` (a session-held SQL fragment). `GetPreBatch`/`GetAllInvoice` throw NullReference without Session. |
| Error handling | None (exceptions -> SOAP/HTTP 500). |
| Status/retry | n/a. |
