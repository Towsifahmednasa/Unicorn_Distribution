---
page_id: PAGE-192
module: M05
page: CustomerExcelUpload
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-192 — CustomerExcelUpload ("Customer Data Upload")

> **Evidence level:** Tier A. `.aspx`, `.aspx.cs`, `ExcelUpForMIGOBLL`, `ExcelUpForMIGODAL`, `SInventorySql`, `ClsCommonInternalDAL.SaveAction`, the verify/transfer pages `CustomerList.aspx.cs` (PAGE-193) and the tag-change clone (PAGE-197/198) were read. **Bodies of `sp_I_Customer`, `sp_VerifyCustomer`, `sp_CustomerTransfer` are NOT in the repo (UNKNOWN)** — what "verify" and "transfer" do to live customers is INFERRED only from names/usages. Tables `tblCustomerMasterExcelFileMaster` / `…Detail` DDL not in repo.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-192 |
| Module | M05 |
| Page | CustomerExcelUpload (heading "Customer Data Upload") |
| Kind | Upload / Import (stage 1 of 3: upload → verify → transfer) |
| Folder / class | `SInventory_UI` / `SInventory_UI_CustomerExcelUpload` |
| Master page | `~/MasterPages/MainMasterPage.master` (legacy layout) |
| Clones | `CustomerTagChangeExcelUpload.aspx` (PAGE-197: same 20-column layout, tag-change tables) |

## 2. Purpose
Read an Excel file containing customer master rows (SAP-format: branch, customer code/name, addresses, MIO/FE/DZSM codes, market, term of payment), preview it in a grid, and save it as a staged upload batch (master + detail rows, flagged *unverified*) for later verification and transfer into live customers. CONFIRMED (`CustomerExcelUpload.aspx.cs:217-252,262-319`).

## 3. Business Objective
Bulk-load or refresh customers from SAP/ERP extracts without typing each one, with a verification step before anything reaches the live customer master. INFERRED (staging design confirmed; live effect UNKNOWN).

## 4. Navigation
Menu DB-driven (UNKNOWN). Batch continues on `CustomerList.aspx` (PAGE-193: verify / verified-unverified export / transfer / delete). Buttons: Refresh (self), Home (`../CommonUI/HomePage.aspx`); an unused `areaImageButton_Click` redirects to `AreaView.aspx`.

## 5. Route
`~/SInventory_UI/CustomerExcelUpload.aspx`.

## 6. Source Files
`Solution.Web/SInventory_UI/CustomerExcelUpload.aspx(.cs)`; `Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs` (also a duplicate `ExcelUpForCustTagChangeBLL.cs`); `Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs`; `SInventorySql.cs`; `ClsCommonInternalDAL.cs` (`SaveAction`); `ClsPrimaryKeyFind.cs`; `Library.DAO/SInventory_Entities/MigoMasterDAO`; sample formats in `Solution.Web/ExcelFiles/` (`CustomerFormat.xls`, `CUSTOMER UPLOAD FORMAT.xlsx`, …).

## 7. UI Control Inventory
| Control | Label | Required (server) | Notes |
|---|---|---|---|
| `documentDateTextBox` (+ calendar `dd-MMM-yyyy`) | Document Upload Date | yes | "Select DocumentDate!!" |
| `manufacturerDropDownList` | Manufacturer | yes (`SelectedIndex>0`) | `select * from tblManufacturer` (`ManufacName`/`ManufacId`) |
| `id_fu` + `btnUpload` | file chooser / Upload | file must exist (no explicit check — see §22) | `.xls` (Jet 4.0) or `.xlsx` (ACE 12.0) |
| `IsFileUploaded` hidden | — | — | set "true" after preview |
| `lbl_up_status` | "n record Found!" | — | — |
| `loadGridView` | preview | ≥1 row | 20 bound columns: BRANCH, BRANCHDES, CustomerCode, CUSTOMERNAME, ADDRESS-1, ADDRESS-2, CITY, CONTACTPERSON, CONTACTNUMBER, MIOCode, MIOName, TerritoryCode, FECode, FEName, DZSMCode, DZSMName, SHIPPINGCOND, SHIPPINGPOINT, MarketName, TERMOFPAYMENT (`HtmlEncode=False`); empty cells coloured orange in `RowDataBound` |
| `submitButton` | Submit | — | confirm "Are you sure you want to Save ?" |
| `cancelUploadListButton` | Cancel Upload | — | confirm "Are you sure you want to Re-Upload ?" — clears grid |
| `refreshButton`, `HomeButton` | Refresh / Home | — | redirects |

## 8. Field Mapping (grid cell → SP/column)
Excel column → grid cell index → `sp_I_Customer` parameter (`ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow` `:50-79`) → staged column (table `tblCustomerMasterExcelFileDetail`, per `GetVerificationReport` select `:302-308`):
1 BRANCH→@BRANCH→BRANCH (later reported as ComUnitCode); 2 BRANCHDES→@BRANCHDES→BRANCHDES (ComUnitName); 3 CustomerCode→@CustomerCode; 4 CUSTOMERNAME→@CUSTOMERNAME; 5 ADDRESS1→@ADDRESS1 (Address); 6 ADDRESS2→@ADDRESS2 (Addrees2); 7 CITY→@CITY; 8 CONTACTPERSON→@CONTACTPERSON (ConPerson); 9 CONTACTNUMBER→@CONTACTNUMBER (CellNo); 10 MIOCode→@MIOCode (MiaCode); 11 MIOName→@MIOName; 12 TerritoryCode→@TerritoryCode (AreaCode); 13 FECode→@FECode (DistrictCode); 14 FEName→@FEName; 15 DZSMCode→@DZSMCode (RegionCode); 16 DZSMName→@DZSMName; 17 SHIPPINGCOND→@SHIPPINGCOND; 18 SHIPPINGPOINT→@SHIPPINGPOINT (MarketCode); 19 MarketName→@MarketName; 20 TERMOFPAYMENT→@TERMOFPAYMENT. Extra params: `@Migo` (= batch master id → column `MasterID`), `@Verifyed`='False'. Header: `tblCustomerMasterExcelFileMaster(CustomerMasterExcelFileMasterID, CustomerMasterExcelFileCode, ManufacId, CustomerMasterExcelFileDocumentDate, Transfer, EntryBy, EntryDate, VerifyedAll)` (`:89-97`).
Mapping of the staged columns onto `tblCustMaster` happens inside `sp_CustomerTransfer` — UNKNOWN.

## 9. Dropdown Mapping
`manufacturerDropDownList` ← `ExcelUpForMIGODAL.LoadmanufacturerName` (`:107-112`): `select * from tblManufacturer` via `LoadDropDownValueWithoutDataBase`; text `ManufacName`, value `ManufacId`.

## 10. Button Action Mapping
| Button | Handler | Calls | DB effect |
|---|---|---|---|
| btnUpload | `btnUpload_Click` → `ExcelToGrid` | file saved to `~/ExcelFiles/<name>`; OLE-DB read | none (preview) |
| submitButton | `submitButton_Click` `:217` | `SaveCustomerUploadMasterDAL` then `XLDataGridToDbByRow` → `CustomerXLDataGridToDbByRow` per row | INSERT header, `sp_I_Customer` per row |
| cancelUploadListButton | clear grid | — | none |

## 11. Frontend Flow
User selects date + manufacturer + file → Upload → server reads sheet → grid + status label "n record Found!". Orange cells mark blanks. Submit asks confirmation (`confirm`) then postbacks. No AJAX; no client validation.

## 12. Backend Flow
`ExcelToGrid` (`:262-319`): `id_fu.SaveAs(MapPath("~/ExcelFiles/"+id_fu.FileName))` **before** any type check; extension `.xls` ⇒ `Microsoft.Jet.OLEDB.4.0`, `.xlsx` ⇒ `Microsoft.ACE.OLEDB.12.0` (other extensions leave `oledbConn` null ⇒ NullReferenceException, unhandled); first sheet name from schema table row 0; `SELECT * From [sheet]`; rows whose first column is empty are dropped; grid bound.
`submitButton_Click` (`:217-252`): `CheckValidation()` (§18) → build `MigoMasterDAO{ManufacId, MogoDocumentDate, StockUpload=false, EntryBy=Session["LoginName"], EntryDate=now}` → `SaveCustomerUploadMasterDAL`: `CustomerMasterExcelFileMasterID = MAX+1` (`PrimaryKeyMax`), code `CMEF-<id padded to ≥4 digits when <3 digits>` (1 digit → `000n`, 2 → `00nn`, 3+ → no padding: `CMEF-100`), INSERT master with `Transfer=false`, `VerifyedAll=false` → for every grid row call `sp_I_Customer` through `ClsCommonInternalDAL.SaveAction` (`ExecuteNonQueryAction`) **which swallows exceptions and returns 0**; the BLL/page ignore the returned id ⇒ any row that fails to insert is silently dropped. Then grid cleared, "Upload Successful..." shown. Each row and the header are separate auto-commit statements (no transaction). CONFIRMED.

## 13. API Mapping
None.

## 14. Service Mapping
`ExcelUpForMIGOBLL.{LoadmanufacturerName, SaveCustomerUploadMasterDAL, CustomerXLDataGridToDbByRow, CustomerMasterCodeGenerator}` — note the class is the MIGO (goods receipt) uploader reused for customers; MIGO methods live in the same class.

## 15. Repository Mapping
`ExcelUpForMIGODAL.{SaveCustomerUploadMasterDAL, CustomerXLDataGridToDbByRow, VerifyCustomerDAL, LoadCustomer, DeleteCustomerData, DeleteCustomerDetailData, TransfarCustomer_DAL, GetVerifyedData, GetUnVerifyedData, ReportVerifyedData, ReportUnVerifyedData}`.

## 16. Stored Procedure Mapping
| SP | Called from | Body |
|---|---|---|
| `sp_I_Customer` (out `@DetailID`) | `CustomerXLDataGridToDbByRow` (this page) | NOT in repo — presumably INSERT into `tblCustomerMasterExcelFileDetail` (INFERRED) |
| `sp_VerifyCustomer(@CustomerMasterExcelFileMasterID)` | PAGE-193 "VerifyData" via text `exec` | NOT in repo |
| `sp_CustomerTransfer(@CustomerMasterExcelFileMasterID)` | PAGE-193 transfer via `RunStoreProcedure` | NOT in repo |
| generic `ExecuteAllSqlQueryByStoreProcedure` | list/drop-down SQL | NOT in repo |

## 17. Database Mapping
Written by this page: `tblCustomerMasterExcelFileMaster` (INSERT), `tblCustomerMasterExcelFileDetail` (via `sp_I_Customer`, INFERRED). Read: `tblManufacturer`. Later stages: verify updates `Verifyed`/`VerifyedAll` (INFERRED); transfer writes live customers (UNKNOWN); delete removes master+detail only while `Transfer=false`.

## 18. Validation Matrix
`CheckValidation()` (`:100-216`), first failure wins:
| Order | Rule | Message |
|---|---|---|
| 1 | any of 20 cells equals `&nbsp;` (blank cell) | " Field Cannot be Blank !!" |
| 2 | any cell text `""` | " Field Cannot be Blank !!" |
| 3 | any cell `&nbsp;&nbsp;` (only checked for cells 1-4 exactly; later cells use malformed `&nbsp&nbsp;`) | " Field Cannot be Blank !!" |
| 4 | date empty | "Select DocumentDate!!" |
| 5 | manufacturer index 0 | "Select Manufacturer !!" |
| 6 | grid has 0 rows | " Upload File !!" |
| result | `XLDataGridToDbByRow` returns false / exception | "Error Uploading...." ; success "Upload Successful..." |
No checks for: numeric/length formats, existence of customer/market/MIO/territory codes, duplicate customer codes within the file or vs live master, file size/extension whitelist, header names (mismatched header ⇒ `BoundField` shows blanks ⇒ flagged blank by rule 1), or user role.

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-030 | Only `.xls` and `.xlsx` are parsed (by extension); the first worksheet is read; rows with an empty first column are ignored; columns are matched by header name (CustomerCode, CUSTOMERNAME, ADDRESS-1 …). | `CustomerExcelUpload.aspx.cs:262-311`; `CustomerExcelUpload.aspx:142-161` | `ExcelToGrid` | CONFIRMED |
| BR-M05-031 | Submission requires: a document date, a manufacturer, at least one row, and all 20 columns non-blank in every row. | `:100-216` | `CheckValidation` | CONFIRMED |
| BR-M05-032 | Each upload is a batch: header code `CMEF-<n>` with `n=MAX(id)+1` (padding rule in §12), `Transfer=false`, `VerifyedAll=false`; entered by `Session["LoginName"]` on `DateTime.Now`. | `ExcelUpForMIGOBLL.cs:56-100`; `ExcelUpForMIGODAL.cs:89-97` | BLL/DAL | CONFIRMED |
| BR-M05-033 | Detail rows are staged with `Verifyed='False'`; a row insert failure is swallowed (returns 0) and does not stop or fail the batch. | `ExcelUpForMIGODAL.cs:76-78`; `ClsCommonInternalDAL.cs:591-604` | DAL | CONFIRMED |
| BR-M05-034 | Batch lifecycle after upload (PAGE-193): list shows only batches with `Transfer=0` (optionally by manufacturer); **Verify** runs `sp_VerifyCustomer`; **Transfer** allowed only if `VerifyedAll=true` (else "Please Verify all Customer!!!") and runs `sp_CustomerTransfer`; **Delete** allowed only while `Transfer=false` (else "Can't Delete !!!") and removes master and detail rows; counts of verified/unverified rows are shown and exportable to Excel (`CustVerUnverExcelViewer`). What verify checks and what transfer writes: UNKNOWN. | `CustomerList.aspx.cs:67-98,147-177`; `ExcelUpForMIGODAL.cs:98-106,209-258` | PAGE-193 | CONFIRMED (gates) / UNKNOWN (SP effects) |

## 20. Permission Matrix
Only `Session["LoginName"]` read (throws NullReference ⇒ error page if session expired). No role check; menu-based access only. CONFIRMED.

## 21. Status Lifecycle
Batch: **Uploaded** (`Transfer=0`, `VerifyedAll=0`) → **Verified** (`VerifyedAll=1`, set by `sp_VerifyCustomer` — INFERRED) → **Transferred** (`Transfer=1`, set by `sp_CustomerTransfer` — INFERRED) | **Deleted** (only before transfer). Row: `Verifyed` false → true (verify). Setting code visible only for the initial values (`ExcelUpForMIGODAL.cs:76,95`).

## 22. Error Handling
`ExcelToGrid` has no try/catch (missing file ⇒ `id_fu.PostedFile` null ⇒ exception; bad extension ⇒ NRE) while the newer TransferUI clones wrap it. `XLDataGridToDbByRow` catches and shows `ex.ToString()` in an alert (exposes stack trace to user). Per-row DB errors are hidden (BR-M05-033). No logging.

## 23. Audit
Batch: `EntryBy` (login name), `EntryDate`. Rows: none known. Uploaded files remain on disk in `Solution.Web/ExcelFiles/` (and are present in the repo) — not purged, name-collision overwrites.

## 24. Integration
SAP-format Excel extracts (branch/customer/MIO/FE/DZSM/shipping columns) — manual file exchange. No API.

## 25. Reports
Verified / unverified rows per batch exported via `SInventory_RPTVIEW/CustVerUnverExcelViewer.aspx?status=1|2&id=` (report doc RPT-M05-02).

## 26. Dependencies
OLE-DB providers Jet 4.0 / ACE 12.0 must be installed on the web server; `ExcelFiles/` write permission; AjaxControlToolkit calendar.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Upload valid `.xlsx` with 20 filled columns | grid shows "n record Found!"; Submit → "Upload Successful..." | 030,031 |
| TC-02 | File with one blank cell | Submit ⇒ " Field Cannot be Blank !!"; cell orange | 031 |
| TC-03 | Submit without date / manufacturer / file | messages per §18 | 031 |
| TC-04 | Upload `.csv`/`.txt` | unhandled error (NullReference) | 030 |
| TC-05 | Two rows with same CustomerCode | both staged (no dup check) | 033 |
| TC-06 | Force `sp_I_Customer` failure on 1 row | batch still reports success, row missing | 033 |
| TC-07 | Upload batch #1, #2, #100 | codes `CMEF-0001`, `CMEF-0002`, `CMEF-100` | 032 |
| TC-08 | Transfer before verify (PAGE-193) | "Please Verify all Customer!!!" | 034 |
| TC-09 | Delete after transfer | "Can't Delete !!!" | 034 |
| TC-10 | Upload `evil.aspx` renamed?? (extension `.aspx`) | file is written to web folder before type check — security test | GAP-014 |

## 28. Reverse Engineering References
Files in §6; `Solution.Web/SInventory_UI/CustomerList.aspx.cs`; `Solution.Web/SInventory_RPTVIEW/CustVerUnverExcelViewer.aspx.cs`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| "Validation pipeline" | None existed | grid-level blank check only; real validation (if any) inside unseen SPs | UNKNOWN |

## 30. Known Gaps
- Arbitrary file type is written into the web-accessible `ExcelFiles/` folder before validation (extension check happens after `SaveAs`).
- Row failures silently dropped; no transaction over batch; no duplicate detection; no code existence validation.
- Verify/transfer logic unknown (SP bodies absent) — cannot state which customer fields are created/overwritten.
- Class name reuse (MIGO uploader) and cloned tag-change tables cause maintenance risk; magic strings (`&nbsp;`).
- Unhandled exceptions expose `ex.ToString()`.
GAP-M05-014/015.

## 31. Change Impact
`ExcelUpForMIGOBLL/DAL` also serves MIGO (goods receipt) upload pages (other module) — edits to shared methods affect them. PAGE-193 (verify/transfer/delete) and PAGE-181 (viewer) depend on the same tables; PAGE-197/198 duplicate the flow against `tblCustomerMasterTagChangeExcelFile*`.
