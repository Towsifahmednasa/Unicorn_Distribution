---
page_id: PAGE-203
module: M05
page: CustomerImport
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-203 — CustomerImport ("Customer Updation")

> **Evidence level:** Tier A. `.aspx`, `.aspx.cs`, `BonusCampaignNewDAL.SaveCustomerInfoForExcel`, DAOs, and the approval chain (`CustomerProviderTypeApprove` PAGE-204 → `OrderTrackingDAL`) were read. **Bodies of `sp_Save_CustomerPropUpdateMaster`, `sp_Save_CustomerPropUpdateDetail`, `sp_GetCustomerProviderTypeApproveList`, `sp_Update_CustPropUpdate` are NOT in the repo (UNKNOWN)**; what the approval applies to `tblCustMaster` is INFERRED from the radio labels only. Despite the folder name "TransferUI", the page updates customer *properties*; it does not transfer stock.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-203 |
| Module | M05 |
| Page | CustomerImport (breadcrumb "Customer Updation") |
| Kind | Upload / Import → staging batch (stage 1 of 2: import → approve) |
| Folder / master | `TransferUI` / `~/MasterPages/NewMasterPage.master` |
| Class | `TransferUI_CustomerImport` |
| Twin | `DoctorImport.aspx` (PAGE-205) — same code, doctor SPs |
| Note | ~60 % of the code-behind (requisition grid, product autocomplete, e-mail senders) is copy-pasted dead code from a stock requisition page; the live path is `InitialGrid` → `btnUpload_Click/ExcelToGrid` → `submitButton_Click` |

## 2. Purpose
Bulk-change four master-data properties of existing customers from an Excel sheet — the customer's **market**, **provider (program) type**, **customer type** or **pharma platform (SMC type)** — by uploading customer codes with the new codes, previewing them, and saving a pending batch that an approver applies later. CONFIRMED for staging (`CustomerImport.aspx.cs:519-582`); the apply step is INFERRED.

## 3. Business Objective
Let head-office users re-classify or re-territory many customers in one step (e.g. after a market restructuring) under a review step, instead of editing each customer in `CustomerEntry`. INFERRED.

## 4. Navigation
Menu DB-driven (UNKNOWN). "Back to List" → `CustomerProviderTypeApprove.aspx` (PAGE-204). Download link `../ExcelFiles/CustomerFormat.xls` (format template). Success alert redirects to `CustomerImport.aspx`; Reset → same page.

## 5. Route
`~/TransferUI/CustomerImport.aspx[?MasID=…]` (`MasID` triggers `GetOneRecord` which loads a *requisition* — legacy dead path, GAP-M05-016).

## 6. Source Files
`Solution.Web/TransferUI/CustomerImport.aspx(.cs)`; `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:939-1013`; `Library.DAO/DoctorModule_DAO/BonusCampaignNewMasterDAO.cs:74-86` (`CustomerPropUpdateMasterDAO`); `BonusCampaignNewDetailDAO.cs:38-58` (`CustomerPropUpdateDetailDAO`); `Library.DAL/DataManager/DataAccessManager.cs`; template `Solution.Web/ExcelFiles/CustomerFormat.xls`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| `rbType` (RadioButtonList) | update type | implicit (always one selected) | "Market Update" | values: `Market Update`, `Provider Type` (label "Provider Type Update"), `Customer Type`, `Pharma Platform Type` |
| `id_fu` (FileUpload), `btnUpload` | Upload File | file must be chosen ("Please select an Excel file !!") | — | `.xls` Jet / `.xlsx` ACE |
| `IsFileUploaded` hidden, `lbl_up_status` | "File Name:… [ n records Found!]" | — | — | — |
| `productGridView` (name reused from requisition page) | preview | — | one blank row after `InitialGrid` | template labels: `lblCustomerCode`, `lblProviderTypeCode`, `lblCustomerTypeCode`, `lblMarketCode`, `lblPharmaPlatformCode` bound with `Eval("CustomerCode")`, `Eval("ProviderTypeCode")`, `Eval("CustomerTypeCode")`, `Eval("MarketCode")`, `Eval("PharmaPlatformCode")` |
| `submitButton` | Submit | — | — | client confirm `sweetAlertConfirm_Submit` → `submitButton_Click` |
| `LinkButton5` | Reset | — | — | redirect self |
| `viewLinkButton` | Back to List | — | — | → PAGE-204 |
A second block of requisition controls (autocomplete product grid, `ImageButton1/2`) is inside a `<%-- … --%>` comment (`CustomerImport.aspx:290-518`) — inactive.

## 8. Field Mapping
| Grid label / Excel column | DAO | SP param | Table.column (per SP name; bodies UNKNOWN) |
|---|---|---|---|
| CustomerCode | `CustCode` (empty ⇒ null) | `@CustCode` | detail table of `sp_Save_CustomerPropUpdateDetail` |
| ProviderTypeCode | `ProviderType` | `@ProviderType` | same |
| CustomerTypeCode | `CustTypeCode` | `@CustTypeCode` | same |
| MarketCode | `MarketCode` | `@MarketCode` | same |
| PharmaPlatformCode | `PharmaPlatformCode` | `@PharmaPlatformCode` | same |
| — | detail id | `@CustPropUpdateDetailId` (null) | PK |
| — | master id returned | `@CustPropMasterId` = pk from master SP | FK |
| rbType.SelectedValue | `CustomerPropUpdateMasterDAO.ConvertType` | `@ConvertType` | master table |
| constant `1` | `TypeId` | `@TypeId` | master table |
| Session["UserId"] | — | `@EntryBy` | master table |
| — | `@CustPropMasterId` (0 on create) | master SP input | — |
Source `CustomerImport.aspx.cs:525-563`, `BonusCampaignNewDAL.cs:939-1013`. All five columns are always sent whichever radio is chosen; only the radio value tells the approver/SP which column is meant.

## 9. Dropdown Mapping
None (radio list only).

## 10. Button Action Mapping
| Button | Handler | DAL | SP | Effect |
|---|---|---|---|---|
| btnUpload | `btnUpload_Click` → `ExcelToGrid` `:638-712` | — | — | file saved to `~/ExcelFiles/<name>`; preview |
| submitButton | `submitButton_Click` `:519` | `BonusCampaignNewDAL.SaveCustomerInfoForExcel` | `sp_Save_CustomerPropUpdateMaster` (returns id) then `sp_Save_CustomerPropUpdateDetail` per row | new pending batch (UNKNOWN table names) |
| Reset / Back | redirects | — | — | — |

## 11. Frontend Flow
Page load: `InitialGrid()` shows one empty template row. Choose type, pick file → Upload → grid shows rows (labels read-only). Submit shows SweetAlert confirm, then postback. Result: `successalert('Operation successful!','Success','CustomerImport.aspx')` or `faildalert('Already Exist!','Faild')` (generic text for every failure). `Download Excel Format` link is a static file.

## 12. Backend Flow
`ExcelToGrid` (`try/catch` → "Something went wrong!"; empty-file check → "Please select an Excel file !!"): saves the upload first (`id_fu.SaveAs`), picks the OLE-DB provider by extension (any other extension ⇒ null connection ⇒ NRE caught ⇒ "Something went wrong!"), reads first sheet, drops rows whose first column is empty, binds the grid, label class `alert alert-info`.
`submitButton_Click`: `Validation()` (only alerts when zero grid rows and **still returns true**) → loop grid rows reading the five labels (blank ⇒ null) → `CustomerPropUpdateMasterDAO{TypeId=1, ConvertType=rbType}` → `SaveCustomerInfoForExcel(master, details, Session["UserId"])`: opens connection; `SaveDataReturnPrimaryKey("sp_Save_CustomerPropUpdateMaster")`; if `pk>0` `SaveData("sp_Save_CustomerPropUpdateDetail")` per row; commit on close, rollback + rethrow on exception (`SqlConnectionClose(true)`) — batch is atomic (CONFIRMED, `DataAccessManager` lazy transaction).

## 13. API Mapping
None (no `[WebMethod]`).

## 14. Service Mapping
No BLL: DAL `BonusCampaignNewDAL` (class shared with campaign pages — misnamed for this use). Unused: `RequisitionBLL`, `DCStoreBLL`.

## 15. Repository Mapping
`BonusCampaignNewDAL.SaveCustomerInfoForExcel` (line 939). Dead: `RequisitionBLL.GetDataInfoByIdBll/Dtls`, `SaveRequsition`, `SaveRequsitionChild`, `UpdateManufacturerInfo`.

## 16. Stored Procedure Mapping
| SP | Direction | Body |
|---|---|---|
| `sp_Save_CustomerPropUpdateMaster` | INSERT batch header, returns id | NOT in repo |
| `sp_Save_CustomerPropUpdateDetail` | INSERT row | NOT in repo |
| (approval, PAGE-204) `sp_GetCustomerProviderTypeApproveList(@Parm)`, `sp_Update_CustPropUpdate(@CustPropMasterId,@UpdateBy)` | list / apply | NOT in repo |

## 17. Database Mapping
Tables UNKNOWN (SP bodies absent). Approver later affects live customer data (likely `tblCustMaster`: MarketId/ProgramTypeId/CustomerTypeId/SMCTypeId — INFERRED from radio labels and from column names listed in `sp_Get_CustMasterList_Approve`).

## 18. Validation Matrix
| Rule | FE | BE | Message |
|---|---|---|---|
| file chosen | — | yes | "Please select an Excel file !!" |
| extension `.xls/.xlsx` | — | implicit (null connection) | "Something went wrong!" |
| grid not empty | — | shows "Tabel Can not be Empty!" but does not stop | (GAP-016) |
| code existence / format / duplicates | none | none (page) | UNKNOWN (SP) |
| DB failure | — | yes | "Already Exist!" |

## 19. Business Rules
| ID | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M05-035 | An import is a *pending batch*: header (type + user) plus one detail row per Excel row; nothing is applied to customers at import time. | `CustomerImport.aspx.cs:525-563`; `BonusCampaignNewDAL.cs:939-1013` | staging SPs | CONFIRMED (staging) / INFERRED (no live write) |
| BR-M05-036 | The update kind is chosen by radio (`Market Update`, `Provider Type`, `Customer Type`, `Pharma Platform Type`) and stored as `ConvertType`; `TypeId` is hard-coded 1; the sheet always carries the five columns CustomerCode, ProviderTypeCode, CustomerTypeCode, MarketCode, PharmaPlatformCode and blanks become NULL. | `CustomerImport.aspx:147-151`; `.cs:542-559` | page | CONFIRMED |
| BR-M05-037 | The batch (master + all rows) is saved atomically in one transaction; any exception rolls everything back. | `BonusCampaignNewDAL.cs:998-1010`; `DataAccessManager.cs:410-437` | DAL | CONFIRMED |
| BR-M05-038 | No validation of codes at upload (customer exists? code valid? duplicate rows?) — only the SPs (UNKNOWN) could reject; a submit with no upload saves an empty batch containing one all-null detail row (the initial blank grid row) because `Validation()` never blocks. | `CustomerImport.aspx.cs:83-114,390-400` | page | CONFIRMED |
| BR-M05-039 | Approval (PAGE-204): the approver sees only batch headers (Transfer Type, Entry By, Entry Date) — not the rows — and can only Approve; approval calls `sp_Update_CustPropUpdate(@CustPropMasterId,@UpdateBy=Session UserId)`; there is no reject/comment; failure text is always "Already Exist!". | `CustomerProviderTypeApprove.aspx:43-54`; `.aspx.cs:39-63`; `OrderTrackingDAL.cs:658-686` | PAGE-204 | CONFIRMED |

## 20. Permission Matrix
Uses `Session["UserId"]` (NRE if missing). No role check on this page or on PAGE-204; access by menu only. CONFIRMED.

## 21. Status Lifecycle
Batch: **Pending** (created here; `IsTransfer` flag exists on `CustomerPropUpdateMasterDAO`, `BonusCampaignNewMasterDAO.cs:83`) → **Approved/Transferred** (set by `sp_Update_CustPropUpdate` — INFERRED). Status column names/values UNKNOWN.

## 22. Error Handling
Upload: try/catch with generic alert (no detail, no log). Save: DAL rethrows after rollback; the page does **not** catch ⇒ unhandled exception page; a `false` result ⇒ "Already Exist!". Dead code contains SMTP sending helpers (`email()`, `email2()`) with a hard-coded mailbox credential (redacted; `CustomerImport.aspx.cs:465-517`) — never called (GAP-M05-016).

## 23. Audit
`EntryBy` (UserId) and entry date (SP) on the master; approver `UpdateBy`. Row-level audit UNKNOWN. Uploaded workbook is left in `~/ExcelFiles/`.

## 24. Integration
None external. Excel files exchanged manually (template `CustomerFormat.xls`).

## 25. Reports
None.

## 26. Dependencies
OLE-DB Jet/ACE, `~/ExcelFiles` write access, SweetAlert helper JS, `BonusCampaignNewDAL`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Choose "Provider Type", upload sheet with 3 rows, Submit | 1 master (ConvertType "Provider Type", TypeId 1) + 3 detail rows, "Operation successful!" | 035,036 |
| TC-02 | Click Upload without file | "Please select an Excel file !!" | — |
| TC-03 | Upload `.txt` | "Something went wrong!" | — |
| TC-04 | Blank cells in sheet | stored as NULL | 036 |
| TC-05 | Submit immediately (no upload) | empty batch with one blank detail row is saved | 038 |
| TC-06 | Force detail SP error on row 2 | whole batch rolled back | 037 |
| TC-07 | Approve batch in PAGE-204 | list refreshes, success alert; effect on customers UNKNOWN — verify in DB | 039 |
| TC-08 | Unknown customer code in sheet | accepted at upload; outcome at approval UNKNOWN | 038 |

## 28. Reverse Engineering References
Files in §6; `Solution.Web/TransferUI/CustomerProviderTypeApprove.aspx(.cs)`; `Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:364-383,658-686`.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | — | MISSING |
| Import validation | None existed | none in page | GAP |
| Approval semantics | None existed | unknown SP | UNKNOWN |

## 30. Known Gaps
Dead requisition/e-mail code with embedded credential (redacted); `Validation()` cannot block; no row-level preview for approver; no reject; generic error text; page name/folder ("TransferUI") and DAL name (`BonusCampaignNewDAL`) misleading; all SP bodies absent. GAP-M05-016/017.

## 31. Change Impact
Twin page PAGE-205 and approvals PAGE-204/206 share `CustPropMasterId` conventions; `BonusCampaignNewDAL` is shared with campaign pages; `sp_Update_CustPropUpdate` affects customer master data (`tblCustMaster`), hence PAGE-168/173 lists.
