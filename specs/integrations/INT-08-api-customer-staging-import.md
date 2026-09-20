# INT-08 - API customer staging -> customer master

| Item | Detail |
|---|---|
| Direction | External/mobile capture -> ERP customer master (INFERRED source) |
| Transport | Database staging table `dbo.tbltempCustMaster` (columns incl. `tempCustomerMasterId, CustomerCode, CustomerName, Address, Addrees2, CellNo, City, ConPerson, MarketCode/Name, MIACode/MIAName, AreaCode, DisCode, FEName, ComUnitCode/Name, RegionCode, DZSMName, TermOfPayment, FixedCustomer, AddtoMainCustomer`). Writer not in repo (name "ApiCustomer" suggests an API). |
| Trigger | Manual: `ApiCustomerList.aspx` (PAGE-685) lists rows `WHERE AddtoMainCustomer='False'`; `Edit` opens `ImportedApiCustomer.aspx?ID=` (PAGE-687, updates via `sp_UD_ApiCustomerMaster`); `Add` promotes. |
| Rules (BR-M17-021) | Add validates all of Code, Name, Address, Address2, CellNo, City, ConPerson, MIACode, MiaName, AreaCode, DisCode, FEName, ComUnitCode/Name, RegionCode, DZSMName, TermOfPayment non-empty (else "Please Edit customer data !!!"); duplicate `CustomerCode` in `tblCustMaster` -> "Sorry! Customer already exist!!!"; inserts `tblCustMaster` with `ShippingCond='N/A'`, `CategoryId=1`, key via `ClsPrimaryKeyFind.PrimaryKeyMax`; then `UPDATE tbltempCustMaster SET AddtoMainCustomer=1`. The two statements are separate (not atomic). Code: `Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs`, `Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs`. |
| Status / retry | `AddtoMainCustomer` 0->1; failed add stays listed. |
| Authentication | Session login. Error handling: `showMessageBox` alerts; BLL rethrows. |
