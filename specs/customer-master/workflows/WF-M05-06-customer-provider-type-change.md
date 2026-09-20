# WF-M05-06 — Change a customer's provider (program) type in place

**Scope:** `MasterSetup_UI/CustomerChangeProgramType.aspx` (PAGE-167). Evidence: `.aspx.cs`, `CustomerInfoDAL.Update__ProgramTypeInfo`, `sp_Get_CustMasterList_Approve` (body read). `sp_Update_CustomerProgramType` body absent (UNKNOWN).

## Trigger & actors
A master-data user needs to correct/reclassify a handful of customers' provider type. Any user with the page in their menu (no permission lookup).

## Preconditions
Provider types flagged `IsCustomer=1` exist; customers exist.

## Steps
1. Choose **Provider Type** (mandatory) plus optional Pharma Platform, market structure, division/district/thana, status (default Active) and customer; **Search** → `GetCustomerList(Parm())` → `sp_Get_CustMasterList_Approve`. Date and approval-status filters are ignored (BR-M05-042).
2. Grid rows bind a per-row provider-type dropdown (pre-selected with the current value); dropdown, code textbox and the Update link are locked until the row's pencil (`lblEditMode_Click`) is used.
3. Edit the type and/or free-text **Code** → **Update** (`lbtUpDate_Click`): requires a selected type (else "Please Select Program Type!") → `Update__ProgramTypeInfo(customerId, code, typeId, UserId, now)` → `sp_Update_CustomerProgramType` (within DAL transaction).
4. Success ⇒ "Operation successful!" toast and the list is re-queried with the same filters; failure ⇒ "Already Exist!".
5. Optional: **Export** current filter as CSV (`Customer_List_<timestamp>.csv`, includes Provider Type Code).

## Data created / changed
`tblCustMaster.ProgramTypeId`, `ProgramTypeCode`, `UpdateBy`, `UpdateDate` (INFERRED from SP parameters). No approval, no history.

## Branches / exceptions
- No provider-type filter ⇒ search blocked.
- Blank row dropdown ⇒ update blocked.
- DIC users: DC dropdown locked but data not filtered by DC (BR-M05-052).
- SP exception ⇒ rollback, unhandled error.

## Postconditions
New provider type visible in `CustomerView` and all reports/targets keyed by `ProgramTypeId`.

## Related BRs
BR-M05-042, 043, 044, 052.

## Related pages
PAGE-167; list PAGE-173/169; bulk alternative WF-M05-05.

## Tests
See PAGE-167 §27.
