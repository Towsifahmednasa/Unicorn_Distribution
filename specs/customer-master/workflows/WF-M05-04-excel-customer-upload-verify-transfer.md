# WF-M05-04 — Excel customer upload → verify → transfer (legacy pipeline, incl. tag-change variant)

**Scope:** file → staging master/detail tables → verification → transfer to live customers. Traced pages: `CustomerExcelUpload` (PAGE-192), `CustomerList` (PAGE-193), verified/unverified export `CustVerUnverExcelViewer` (PAGE-181); the clone for tag changes: `CustomerTagChangeExcelUpload` (PAGE-197) and `CustomerTagChangeList` (PAGE-198). **Bodies of `sp_I_Customer`, `sp_VerifyCustomer`, `sp_CustomerTransfer`, `sp_VerifyCustomerTagList`, `sp_CustomerTransferTagChange` are not in the repo**: validation and live-write logic is UNKNOWN; only the gates in C# are documented.

## Trigger & actors
- Trigger: head office receives an SAP-format customer extract (or a tag-change extract) to load.
- Actors: a user with the upload page and a user with the list page in their menu (no role checks in code).

## Preconditions
Excel `.xls`/`.xlsx` with exactly the 20 expected headers (BRANCH, BRANCHDES, CustomerCode, CUSTOMERNAME, ADDRESS-1, ADDRESS-2, CITY, CONTACTPERSON, CONTACTNUMBER, MIOCode, MIOName, TerritoryCode, FECode, FEName, DZSMCode, DZSMName, SHIPPINGCOND, SHIPPINGPOINT, MarketName, TERMOFPAYMENT); manufacturer master data; OLE-DB Jet/ACE installed.

## Steps
1. **Upload & preview** — `btnUpload_Click` → `ExcelToGrid`: file saved to `~/ExcelFiles/<name>` (before any type check), provider chosen by extension, first sheet read, rows with empty first column dropped, grid shown with orange blanks (BR-M05-030).
2. **Validate** — `CheckValidation`: all 20 cells of all rows non-blank, date, manufacturer, ≥1 row (BR-M05-031).
3. **Stage** — `SaveCustomerUploadMasterDAL` inserts `tblCustomerMasterExcelFileMaster` (`CMEF-<n>`; tag-change: `tblCustomerMasterTagChangeExcelFileMaster`, `CTME-<n>`) with `Transfer=0`, `VerifyedAll=0`, `EntryBy=LoginName`; then `sp_I_Customer` per row (tag-change: inline INSERT into `tblCustomerMasterTagChangeExcelFileDetail`) with `Verifyed=False`. Failures swallowed; no transaction (BR-M05-032/033). Message "Upload Successful...".
4. **List** — `CustomerList.aspx` (PAGE-193): `LoadCustomer(manufacturer)` shows batches with `Transfer=0`; per batch verified/unverified counts (`COUNT(*)` on detail by `Verifyed`) with links to the Excel export (`CustVerUnverExcelViewer?status=1|2` — tag change 3|4 — → `crpCustomerMaster.rpt` exported as Excel).
5. **Verify** — row command `VerifyData` → `ExcelUpForMIGOBLL.VerifyCustomerBLL(id)` → text `exec sp_VerifyCustomer @CustomerMasterExcelFileMasterID` (tag change: `sp_VerifyCustomerTagList`). Sets row `Verifyed` and presumably master `VerifyedAll` (INFERRED); the message "Verified" is shown unconditionally.
6. **Transfer** — `transferImageButton_Click`: reload batch; if `VerifyedAll` false ⇒ "Please Verify all Customer!!!"; else `TransfarCustomer_BLL(id)` → `RunStoreProcedure("sp_CustomerTransfer")` (tag change: `sp_CustomerTransferTagChange`); if result > 0 ⇒ "Transferred successfully!!!" and list refreshes (batch leaves list because `Transfer` becomes 1 — INFERRED).
7. **Delete (before transfer)** — `DeleteData`: allowed only when `Transfer=false`; deletes master then detail rows; else "Can't Delete !!!" (BR-M05-034).

## Data created / changed
| Stage | Tables |
|---|---|
| upload | `tblCustomerMasterExcelFileMaster` (+ `Detail` via SP) / tag-change equivalents |
| verify | detail `Verifyed`, master `VerifyedAll` (INFERRED) |
| transfer | live `tblCustMaster` (fields UNKNOWN), master `Transfer` |

## Branches / exceptions
- Invalid extension ⇒ NullReferenceException (unhandled).
- Blank cell ⇒ blocked at step 2. Duplicate customer codes ⇒ not detected in C#.
- Row insert failure ⇒ silently skipped; verify may then report fewer rows than the file had.
- Transfer of an unverified batch ⇒ blocked; delete after transfer ⇒ blocked.
- Verify can be repeated (a commented "verify again" exists before transfer).

## Postconditions
Verified batch transferred: customers created/updated in the live master (UNKNOWN details); batch flagged transferred and hidden.

## Related BRs
BR-M05-030 … 034, 059.

## Related pages
PAGE-192, PAGE-193, PAGE-181, PAGE-197, PAGE-198; shared BLL/DAL `ExcelUpForMIGOBLL/DAL`, `ExcelUpForCustTagChangeBLL/DAL`.

## Tests
1. Valid file, 3 rows ⇒ 1 master, 3 detail (all unverified).
2. Blank cell ⇒ blocked. 3. Verify ⇒ counts move to verified. 4. Transfer before verify ⇒ blocked message. 5. Delete after transfer ⇒ blocked. 6. Force one `sp_I_Customer` failure ⇒ detail count 2 with "Upload Successful..." (defect check).
