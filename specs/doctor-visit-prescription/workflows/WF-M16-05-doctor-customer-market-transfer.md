# WF-M16-05 — Doctor / customer transfer to another market

## Trigger & actors
Back-office user opens `MasterSetup_UI/Customer_Doctor_Transfer.aspx` (PAGE-668) to re-assign either customers or doctors from one market to another.

## Preconditions
Source market has active customers (`mas.IsActive=1`) or approved doctors (`DM.ApprovalStatus='2'`); target market structure (and, for customers, distribution centre/route) selected in the "To" hierarchy control `IVMarketStructureTo`.

## Steps
1. Choose type radio `rbType` (item 0 = Customer "Cust", else Doctor "Doc"). Changing it clears both grids.
2. Choose the source market in `IVMarketStructure` → Search (`btnSearch_Click`): if no market → `showMessageBox("Please Select Market!")`. Customer: `CustomerInfoDAL.GetCustomerList(" and mas.IsActive=1 and mas.MarketId=<id>")` → `sp_Get_CustMasterList_Approve`; Doctor: `DoctorDAL.GetDoctorList(" and DM.ApprovalStatus='2' and  DM.MarketId=<id>")` → `sp_Get_DoctorList` (dynamic SQL, body in `LIVE_DB_UPDATE_...sql:884-930`; only `IsDoctor=1` doctors returned).
3. Tick rows. For customers, each tick runs `CheckCustInOrder` → `CommonDataLoad.GetValidationCheck(id,"CustomerTransfer")` → `sp_check_Vali_MarketStructure`; if it returns rows the tick is reverted with "Order Pending!". No equivalent check exists for doctors.
4. Choose target market (required: `T_MarketSelect`), Save (`btnSave_Click`): `Validation()` → grid must have rows ("Table can not be Empty!"), at least one ticked ("Please Select at least one employee !!!" — text is the same for doctors), target market chosen (`is-invalid`).
5. `CustomerInfoDAL.UpdateCustomer_Doctor_Transfer(Type, "id1,id2,…", MarketId, Session UserId, DCID, RouteId)` → `sp_Update_Customer_Doctor_Transfer(@MasterId CSV, @MarketId, @ApprovedBy=session user, @Type 'Cust'|'Doc', @DCID, @RouteId)`; success ⇒ `successalert('Operation successful!', …, 'Customer_Doctor_Transfer.aspx')`.
6. Related: `ListImageButton_Click` → `../TransferUI/CustomerDocTransferApproval.aspx` (approval list in another folder) and `CustomerInfoDAL.UpdateCustomer_Doctor_TransferApprove`, `DoctorDAL.GetDoctorTransferList` → `sp_Get_DoctorTranferApprovalList` — the transfer may be a two-step process handled by SP + approval page; body absent (UNKNOWN).

## Data created / changed
`tblDoctorMaster` (MarketId and derived hierarchy) / `tblCustMaster` (MarketId, hierarchy, station types — cf. the customer-update SP in the same script `:640-660`); approval/transfer log tables UNKNOWN.

## Branches / exceptions
Order pending (customers only) blocks selection; empty selection/market blocks; DAL exception rethrown after rollback (page has no try/catch in `btnSave_Click`).

## Postconditions
Selected doctors/customers point to the target market (or are queued for approval — UNKNOWN).

## Related BR ids
BR-M16-021.

## Related pages
PAGE-668, PAGE-669/670 (doctor master/list), `TransferUI/CustomerDocTransferApproval.aspx` and `Approval_UI/DoctorCustomerTransferApproval.aspx` (other modules).

## Tests
Transfer 2 approved doctors A,B from market M1 to M2 and verify `MarketId`; try with a customer that has pending order (expect "Order Pending!"); save without ticking; save without target market; inject `'` in market id via crafted POST (SQL concatenation risk).
