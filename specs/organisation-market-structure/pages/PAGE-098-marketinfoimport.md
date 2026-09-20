---
page_id: PAGE-098
module: M03
page: MarketInfoImport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-098 — MarketInfoImport

> **Evidence level:** Tier A. `MarketInfoImport.aspx` (288 lines), `.aspx.cs` (259 lines), `Library.DAL/MarketUpload_DAL/MarketUploadDAL.cs`, and the approval side `MarketInfoApprove.aspx.cs` + `OrderTrackingDAL.GetMarketInfoApprovalList/SaveMarket_ApplogDAL` read. SP bodies absent (`UNKNOWN`) — the actual creation/update of live Market/Territory rows happens inside `sp_Save_MarketPropToTable` (unread). Process: `workflows/WF-M03-03-bulk-market-creation-upload-and-approval.md`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / Module | PAGE-098 / M03 |
| Name | MarketInfoImport (breadcrumb "Market Creation", `.aspx:21`) |
| Kind | Upload / Import — Excel → staging |
| Folder / master | TransferUI / NewMasterPage.master |
| Class | `TransferUI_MarketInfoImport`; instance `MarketUploadDAL` |

## 2. Purpose
CONFIRMED: bulk-create or bulk-update **Markets** (and territory assignment) from an Excel sheet: upload → preview in a grid → choose *Market Insert*, *Market Update* or *Territory Update* → Submit to **stage** the rows for approval on `MarketInfoApprove.aspx` (PAGE-097).

## 3. Business Objective
INFERRED: onboard many markets (with Division/District/Thana and per-role station types) in one go without keying each on MarketSetup; approval gate before data reaches live master tables.

## 4. Navigation
"Go to Approval List" → `MarketInfoApprove.aspx` (`.cs:257`); template download `../Approval_UI/MarketTranferFormat.xls` (`.aspx:71`, file exists in repo); Reset (`cancelButton_Click`) → **`CustomerImport.aspx`** (`.cs:148`, another page); success → self.

## 5. Route
`~/TransferUI/MarketInfoImport.aspx`

## 6. Source Files
`Solution.Web/TransferUI/MarketInfoImport.aspx(.cs)`; `Library.DAL/MarketUpload_DAL/MarketUploadDAL.cs`; DAO `Library.DAO/MarketUpload_DAO/MarketUpload`, `MarketUploadDetail`; approval `Solution.Web/TransferUI/MarketInfoApprove.aspx(.cs)` + `Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs` (`GetMarketInfoApprovalList` :408, `SaveMarket_ApplogDAL` :723); server folder `Solution.Web/ExcelFiles/` (upload target, contains many previously uploaded `.xls`); OLEDB providers Jet 4.0 / ACE 12.0.

## 7. UI Control Inventory
| Control | Label | Required | Default | State | Source | Message |
|---|---|---|---|---|---|---|
| `id_fu` (FileUpload) | Upload File | Yes to preview | – | – | – | "Excel file is not a correct format !" (no file or any error) |
| `btnUpload` | Upload | – | – | PostBackTrigger (full postback) | – | – |
| `lbl_up_status` | status "File Name:… [ N records Found!]" | – | – | shown after upload | – | – |
| `IsFileUploaded` (hidden) | – | – | – | set "true" on upload; **never read** | – | – |
| `rbType` | (unlabelled radio) Market Insert / Market Update / Territory Update | one | Market Insert | – | static values `MarketInsert`, `MarketUpdate`, `TerritoryUpdate` | – |
| `dataGridView` | #SL, Territory Code, Market Code, Market Name, Division, District, Thana, Regional Head Station Type, DZSM Station Type, AM Station Type, MIO Station Type (labels bound with `Eval`) | – | one blank row initially | read-only labels | Excel | – |
| `submitButton` | Submit | – | – | `sweetAlertConfirm_Submit` | – | – |
| `LinkButton5` | Reset | – | – | – | – | – |
| `viewLinkButton` | Go to Approval List | – | – | – | – | – |
| `Button2` "Transfer" (in `runat=server visible=false` row `.aspx:48`) | – | – | hidden | would post `mainid` (never set) | – | – |

## 8. Field Mapping
| Excel column (grid) | DTO (`MarketUploadDetail`) | SP param (`sp_Save_MarketPropDetail`) |
|---|---|---|
| territorycode | `TerritoryCode` | `@TerritoryCode` |
| marketcode | `MarketCode` | `@MarketCode` |
| MarketName | `MarketName` | `@MarketName` |
| Division / District / Thana | `DivisionName`, `DistrictName`, `ThanaName` | `@DivisionName`, `@DistrictName`, `@ThanaName` |
| RegionalHeadStationType / DZSMStationType / AMStationType / MIOStationType | same names | `@RegionalHeadStationType`, `@DZSMStationType`, `@AMStationType`, `@MIOStationType` |
Master (`sp_Save_MarketPropMaster`): `@TypeId="0"`, `@EntryBy `(trailing space) = Session UserId, `@EntryDate`=now, `@ConvertType` = `rbType.SelectedValue`, `@MarketPropMasterId `="0". Blank cells become `NULL` (trimmed).

## 9. Dropdown Mapping
None (radio list static).

## 10. Button Action Mapping
| Button | Handler | Chain | SP | Change |
|---|---|---|---|---|
| Upload | `btnUpload_OnClick` `.cs:150` | `ExcelToGrid()` | none | file saved to server; grid bound |
| Submit | `submitButton_Click` `.cs:189` | `Validation()` → build `MarketUploadDetail` list from grid labels → `MarketUploadDAL.SaveMarketInfoForExcel` | `sp_Save_MarketPropMaster`, `sp_Save_MarketPropDetail` × rows | staging insert |
| Go to Approval List | `viewLinkButton_Click` `.cs:255` | redirect | – | – |
| (hidden) Transfer | `Button2_OnClick` `.cs:179` | `Convert.ToInt32(mainid.Value)` → `SaveMarketUploadTransfer` | `sp_Save_MarketPropToTable` | – (unreachable: hidden and `mainid` empty ⇒ FormatException) |

## 11. Frontend Flow
Upload is a full postback (UpdatePanel with `PostBackTrigger` on `btnUpload`). Grid rows are rendered as read-only `Label`s inside TemplateFields and re-read in `submitButton_Click` via `FindControl` (values survive through ViewState).

## 12. Backend Flow
1. `Page_Load` first load: `InitialGrid()` binds one blank row to the grid.
2. **Upload**: (`ExcelToGrid`) `id_fu.SaveAs(MapPath("~/ExcelFiles/" + id_fu.FileName))` **before** any extension check (BR-M03-098); `.xls` → Jet 4.0 (`Excel 8.0;HDR=Yes;IMEX=2`), `.xlsx` → ACE 12.0 (`Excel 12.0;HDR=YES;IMEX=1`), any other extension leaves the connection null → `NullReferenceException` → caught → "Excel file is not a correct format !". Reads the **first** sheet name from the schema table, `SELECT * FROM [sheet]`, keeps rows whose first column is non-empty, binds to `dataGridView`, sets `lbl_up_status` "File Name:{f} [ n records Found!]".
3. **Submit**: `Validation()` (shows "Tabel Can not be Empty!" if zero rows **but always returns true**) → loop grid rows → DAL `SaveMarketInfoForExcel`: `sp_Save_MarketPropMaster` (returns pk) → if pk>0 one `sp_Save_MarketPropDetail` per row (all in one implicit transaction; `isSuccess`=last row's result) → exceptions rollback + rethrow (page has no try/catch).
4. Result: success → `successalert('Operation successful!','Success','MarketInfoImport.aspx')`; else `faildalert('Already Exist!')`.
5. **Approval** (other page): `MarketInfoApprove` loads `sp_GetMarketInfoApprovalList("")`; row command `ApproveData` → `OrderTrackingDAL.SaveMarket_ApplogDAL(MarketPropMasterId)` → `sp_Save_MarketPropToTable(@MarketPropMasterId,@EntryBy,@EntryDate)` which is the step that presumably creates/updates live `Market`/`Territory` rows according to `ConvertType` (UNKNOWN).

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
| DAL | Source | SP |
|---|---|---|
| `MarketUploadDAL.SaveMarketInfoForExcel` | MarketUploadDAL.cs | `sp_Save_MarketPropMaster`, `sp_Save_MarketPropDetail` |
| `MarketUploadDAL.SaveMarketUploadTransfer` (hidden button) | same | `sp_Save_MarketPropToTable` (`isBigData=true`) |
| `OrderTrackingDAL.GetMarketInfoApprovalList` / `SaveMarket_ApplogDAL` | OrderTrackingDAL.cs:408 / 723 | `sp_GetMarketInfoApprovalList(@Parm)` / `sp_Save_MarketPropToTable` |
Unused DAL methods: `SaveMarketUploadMaster`, `SaveMarketUploadDetail`.

## 16. Stored Procedure Mapping
`sp_Save_MarketPropMaster` (staging header, returns pk), `sp_Save_MarketPropDetail` (staging rows), `sp_Save_MarketPropToTable` (applies staged data — UNKNOWN), `sp_GetMarketInfoApprovalList` (read). Bodies NOT in repository. Parameter names in DAL have trailing spaces for the master (`"@EntryBy "`, `"@MarketPropMasterId "`) — SQL Server acceptance UNKNOWN.

## 17. Database Mapping
No inline SQL. Staging tables inferred as `MarketProp*` (from SP/DAO names) — UNKNOWN. Excel is read through OLEDB (external file, not a DB table).

## 18. Validation Matrix
| Rule | Where | Message | BR |
|---|---|---|---|
| File chosen and readable as .xls/.xlsx with a first sheet | `btnUpload_OnClick` | "Excel file is not a correct format !" | BR-M03-097 |
| Grid non-empty | `Validation()` | "Tabel Can not be Empty!" (informational — does not stop the save) | BR-M03-099 |
| Column names must match the template headers (`territorycode, marketcode, MarketName, Division, District, Thana, RegionalHeadStationType, DZSMStationType, AMStationType, MIOStationType`) | `Eval()` in grid | binding exception if a header is missing | BR-M03-097 |
| Territory/Market/Division existence, duplicates | not validated in C# | – | BR-M03-095 |

## 19. Business Rules
| ID | Rule | Source | Impl | Conf |
|---|---|---|---|---|
| BR-M03-095 | Import is **two-phase**: this page only stages (master + detail rows); no live Market/Territory row is written here; content (existence of territory codes, duplicate market codes/names) is not validated in C#. | MarketInfoImport.aspx.cs:189-253; MarketUploadDAL.cs | page/DAL | CONFIRMED |
| BR-M03-096 | Import mode is one of `MarketInsert` (default), `MarketUpdate`, `TerritoryUpdate`, stored on the staging master as `@ConvertType`; what each mode does is defined in the SP (UNKNOWN). | MarketInfoImport.aspx:100-104; .cs:235 | page | CONFIRMED (value) / UNKNOWN (effect) |
| BR-M03-097 | Excel rule: first worksheet only; first row = headers; rows with an empty first column are discarded; `.xls` via Jet 4.0, `.xlsx` via ACE 12.0; other extensions rejected (after being saved). | .cs:83-144,150-168 | page | CONFIRMED |
| BR-M03-098 | The uploaded file is saved to `~/ExcelFiles/<client file name>` before the extension check (overwrites same-named files; no name sanitising, no size/type limit, retained afterwards). | .cs:87-90 | page | CONFIRMED |
| BR-M03-099 | The "empty table" check does not block submission (`Validation()` always returns true); with no upload the single blank row from `InitialGrid()` is staged as a row of NULLs. | .cs:72-82,30-71 | page | CONFIRMED |
| BR-M03-100 | Live data changes only when an approver clicks *Approve* on PAGE-097 (`sp_Save_MarketPropToTable`); the same SP is bound to the hidden *Transfer* button here, which cannot work (`mainid` never set). | MarketInfoApprove.aspx.cs; OrderTrackingDAL.cs:723; MarketInfoImport.aspx.cs:179-187 | approval page | CONFIRMED |

## 20. Permission Matrix
Session: `UserId` (EntryBy; unguarded). No role check on upload or approval; same user can upload and approve (no segregation, no check in C#). Menu-only; no `<authorization>`. Upload target folder inside the web root.

## 21. Status Lifecycle
Staging batch: *Staged* (master+details saved) → *Approved/Applied* (`sp_Save_MarketPropToTable`); listing SP presumably hides applied batches (UNKNOWN). No reject.

## 22. Error Handling
Upload: all exceptions → "Excel file is not a correct format !" (hides real cause: OLEDB provider missing, locked file, missing headers). Submit: unhandled DAL exceptions; generic failure "Already Exist!". `showMessageBox` sanitising is a no-op (`Replace("'", "\'")`).

## 23. Audit
`@EntryBy` (UserId), `@EntryDate` (now) on master; the uploaded file remains on disk (`ExcelFiles`) as the only source trace.

## 24. Integration
Excel/OLEDB (Jet 4.0 32-bit + ACE 12.0 must be installed on the server); template file `Approval_UI/MarketTranferFormat.xls`.

## 25. Reports
None.

## 26. Dependencies
OLEDB drivers; `ExcelFiles` folder write permission; PAGE-097 approval; `sp_Save_MarketPropToTable` (shared with approval).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Upload valid .xlsx with template headers | N records preview | BR-M03-097 |
| TC-02 | Upload .txt renamed | file saved to ExcelFiles, "Excel file is not a correct format !" | BR-M03-097/098 |
| TC-03 | Upload with missing header column | binding error / format message | BR-M03-097 |
| TC-04 | Rows with blank first cell | dropped | BR-M03-097 |
| TC-05 | Submit without upload | one blank row staged; "Operation successful!" | BR-M03-099 |
| TC-06 | Submit after upload | master + N detail rows; success | BR-M03-095 |
| TC-07 | Approver approves batch | `sp_Save_MarketPropToTable` executed | BR-M03-100 |
| TC-08 | Upload same file name twice | overwritten | BR-M03-098 |
| TC-09 | Reset | goes to CustomerImport.aspx | §4 |
| TC-10 | Same user uploads and approves | allowed | §20 |

## 28. Reverse Engineering References
`Solution.Web/TransferUI/MarketInfoImport.aspx(.cs)`, `MarketInfoApprove.aspx.cs`, `Library.DAL/MarketUpload_DAL/MarketUploadDAL.cs`, `Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:408,723`, `Solution.Web/Approval_UI/MarketTranferFormat.xls`.

## 29. Spec vs Actual
| Area | Spec | Actual | DB | Status | Ref |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Insert vs update semantics | None existed | delegated to SP | absent | UNKNOWN | BR-M03-096 |

## 30. Known Gaps
Arbitrary file save into web root (security); no content validation; empty-table check ineffective; hidden broken Transfer button; Reset → unrelated page; unused DAL methods; trailing-space param names; OLEDB dependency; SP bodies absent (GAP-M03-001).

## 31. Change Impact
`sp_Save_MarketPropToTable` shared by PAGE-097 approve and the hidden button; `MarketUploadDAL` only used here (grep of `MarketUploadDAL`). Creating markets in bulk bypasses MarketSetup rules (station-type triple MIO/AM/DZSM, dependants) unless the SP re-implements them (UNKNOWN).
