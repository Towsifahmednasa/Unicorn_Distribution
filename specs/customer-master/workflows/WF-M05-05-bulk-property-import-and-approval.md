# WF-M05-05 — Bulk customer / doctor property update: import → approve → apply

**Scope:** `TransferUI/CustomerImport.aspx` (PAGE-203) and `DoctorImport.aspx` (PAGE-205) stage Excel rows as a pending batch; `CustomerProviderTypeApprove.aspx` (PAGE-204) / `DoctorUpdationApprove.aspx` (PAGE-206) approve it. A sibling approval, `CustomerDocTransferApproval.aspx` (PAGE-202), approves customer/doctor *market transfers* created on `MasterSetup_UI/Customer_Doctor_Transfer.aspx` (not in this module's page list). **All staging/approval SP bodies are missing from the repo**; the effect on live data is INFERRED from labels.

## Trigger & actors
- Trigger: a market restructuring or reclassification requires changing Market, Provider Type, Customer/Doctor Type or Pharma Platform for many outlets.
- Actors: uploader (any user with the import page), approver (any user with the approval page; no role check).

## Preconditions
Excel file in the template (`ExcelFiles/CustomerFormat.xls` / `DoctorFormat.xls`) with headers CustomerCode|DoctorCode, ProviderTypeCode, CustomerTypeCode|DoctorTypeCode, MarketCode, PharmaPlatformCode.

## Steps
1. **Pick type** — radio: Market Update / Provider Type / Customer (Doctor) Type / Pharma Platform Type → stored as `ConvertType` (BR-M05-036).
2. **Upload & preview** — `ExcelToGrid`: saves the file under `~/ExcelFiles/`, reads first sheet with Jet/ACE, drops rows with empty first column, binds label grid (BR-M05-036).
3. **Submit** — `submitButton_Click`: read grid labels (blank ⇒ null) → `SaveCustomerInfoForExcel` / `SaveDoctorInfoForExcel`: `SaveDataReturnPrimaryKey(sp_Save_CustomerPropUpdateMaster | sp_Save_DoctorPropUpdateMaster)` with `@TypeId=1, @ConvertType, @EntryBy=UserId` then `SaveData(sp_Save_*PropUpdateDetail)` per row; one transaction (BR-M05-037). No validation of codes (BR-M05-038).
4. **Approver list** — `OrderTrackingDAL.GetCustomerProviderApprovalList("")` → `sp_GetCustomerProviderTypeApproveList` (doctor: `sp_GetDoctorProviderTypeApproveList`); grid shows Transfer Type (`ConvertType`), Entry By, Entry Date only.
5. **Approve** — row command `ApproveData` reads hidden `CustPropMasterId` → `SaveCustomerProvider_ApplogDAL` → `sp_Update_CustPropUpdate(@CustPropMasterId,@UpdateBy=UserId)` (doctor: `SaveDoctorProvider_ApplogDAL` → `sp_Update_DoctorPropUpdate`). Success ⇒ toast and list reload; failure ⇒ "Already Exist!" (BR-M05-039/041).
6. **Apply** — inside the update SP (UNKNOWN): presumably resolves codes to ids and updates `tblCustMaster` / `tblDoctorMaster` (ProgramTypeId, CustomerTypeId/DoctorTypeId, MarketId, SMCTypeId) for the batch rows and marks the batch transferred (`IsTransfer`).

## Data created / changed
Staging master/detail tables (names UNKNOWN); on approval, live customer/doctor master rows (INFERRED).

## Branches / exceptions
- Approver cannot reject or inspect rows in the UI; failure text is generic.
- Submit without upload saves an empty batch with one all-null detail row (BR-M05-038).
- `DoctorImport` upload has no try/catch; `CustomerImport` has.
- Hidden field id in approver grid is client-supplied (tamperable postback value, validated only by SP).

## Postconditions
Customers/doctors reclassified after approval; audit is only `EntryBy`/`UpdateBy` on the batch (UNKNOWN row-level).

## Related BRs
BR-M05-035 … 041.

## Related pages
PAGE-203, 204, 205, 206 (and PAGE-202 for transfers).

## Tests
1. Provider Type batch (3 rows) ⇒ approver sees 1 line "Provider Type". 2. Approve ⇒ verify `tblCustMaster.ProgramTypeId` changed for the 3 customers (expected, UNKNOWN). 3. Upload with an unknown customer code ⇒ upload accepted; observe approval outcome. 4. Approve twice (double-click) ⇒ check idempotence.
