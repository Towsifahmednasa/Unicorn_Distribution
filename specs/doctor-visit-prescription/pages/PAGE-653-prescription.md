---
page_id: PAGE-653
module: M16
page: Prescription
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-653 — Prescription

> **Evidence level:** Tier A — `Prescription.aspx` (all inline JS), `Prescription.aspx.cs`, `Setup.aspx(.cs)` web methods used for dropdowns, `CommonDataLoad.aspx.cs`, `_QuickDataAccess.js`, `SetupDAL.cs`, `Setup2DAL.cs`, `PrescriptionMasterDAO.cs`, `PrescriptionMaster.cs` read. All prescription SP bodies are absent from the repository (`UNKNOWN`). Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-653 |
| Module | M16 |
| Page | Prescription (create/edit one prescription "RX" record with image and prescribed products) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Code-behind | 55 lines; logic is AJAX/JS (aspx ~700 lines) |
| Duplicate code | Same save logic exists twice: `SetupDAL.Save_Prescription` (used) and `Setup2DAL.Save_Prescription` (unused by this page) |

## 2. Purpose
Record a doctor's prescription: choose capturing user, date, doctor, prescription type, the doctor's chamber, add the products prescribed (brand/product list), upload the prescription photo, submit. CONFIRMED (labels + `FinalSave()`).

## 3. Business Objective
Prescription capture proves which products a doctor prescribes; RX counts feed the RX pivot reports (PAGE-675) and DoctorInfoReport (PAGE-674) and the approval step. INFERRED (report names/columns); the primary capture channel is likely the mobile app (`sp_Webapi_Get_ImagePath`, `IsFromApp`), the web page allows manual/back-office entry — INFERRED.

## 4. Navigation
Inbound: `PrescriptionView.aspx` (Add button → `Prescription.aspx`; Edit → `Prescription.aspx?id=<PrescriptionId>`, `PrescriptionView.aspx.cs:259-263,281-291`). After successful save JS redirects to `PrescriptionView.aspx`. Menu path `UNKNOWN`.

## 5. Route
`~/DoctorModule_UI/Prescription.aspx[?id=<PrescriptionId>]`. `id` parsed by JS `getUrlVars()` and stored in hidden `#masterId` (default `0`).

## 6. Source Files
`Solution.Web/DoctorModule_UI/Prescription.aspx`, `Prescription.aspx.cs`, `Solution.Web/DoctorModule_UI/Setup.aspx.cs` (dropdown web methods lines 985-1020), `Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:69` (`GetChamber_ByDoctorId`), `Solution.Web/CustomScript/_QuickDataAccess.js:779`, `Library.DAL/DoctorModule_DAL/SetupDAL.cs` (`Save_Prescription` :4761, `GetEditData_Prescription` :2636, `Get_PrescriptionDetailsByPrescriptionId` :2617), `Library.DAL/DoctorModule_DAL/Setup2DAL.cs` (dropdown DAL methods `GetCapturedBy_For_ddl` :24, `GetPrescriptionTypelist_For_ddl` :42, `GetDFoctorlist_For_ddl` :61, `Get_ProductList_List` :191), `Library.DAO/DoctorModule_DAO/PrescriptionMasterDAO.cs`, `PrescriptionMaster.cs`.

## 7. UI Control Inventory
All controls are plain HTML (no ASP validators); Select2 for dropdowns; pickadate for the date.

| Control | Label | Required (as intended) | Enforced? | Data source |
|---|---|---|---|---|
| `#ddlCapturedBy` | Captured By | yes (`*`) | **No** (BR-M16-038) | `Setup.aspx/Get_CapturedBy_For_ddl` → `Setup2DAL.GetCapturedBy_For_ddl` → `sp_Get_CapturedBY_For_DDL` (EmpInfoId, EmpName; posted as `EntryBy`) |
| `#PrescripotionDate` | Prescripotion Date (sic) | yes | No | pickadate text input |
| `#ddlDoctor` | Doctor | yes | No | `Setup.aspx/Get_Doctor_For_ddl` → `sp_Get_Doctor_For_DDL` (DoctorId, DoctorName) |
| `#ddlPrescriptionType` | Prescription Type | yes | No | `Setup.aspx/Get_PrescriptionType_For_ddl` → `sp_Get_PrescriptionType_For_DDL` |
| `#ddlChamber` | Chamber | yes | No | on doctor change: `CommonDataLoad.aspx/GetChamber_ByDoctorId` → `sp_GetChamber_ByDoctorId(@id)` (ChemberId, Chember) |
| `#ddlProduct` + Add to list | Product | yes at least one row | No | `Setup.aspx/Get_ProductList_List_New` → `sp_Get_Product_List` (ProductId, ProductName) |
| `#dtTble` rows | product rows with hidden `ProductDetails[i].ProductId` | ≥1 | No | – |
| `#imageUploadForm` (file, `accept=image/*`, multiple attr but only `files[0]` read) | Upload Image | **Yes** — only enforced check | Yes (BR-M16-037) | `FileReader` → base64 into `#imgeBase64Str` |
| `#btnSave` | Submit ("Update" text on edit; hidden when status is `2`) | – | – | – |
| Reset button | `ConfirmationClick()` | – | – | defined in master page (not read) |

## 8. Field Mapping
| UI | JSON property (`typeMaster`) | DAO `PrescriptionMasterDAO` | SP param | Table.column |
|---|---|---|---|---|
| `#masterId` | `PrescriptionId` | `PrescriptionId` | `@PrescriptionId` | `tbl_PrescriptionMaster.PrescriptionId` (UNKNOWN name confirmed only by SP param) |
| `#PrescripotionDate` | `PrescriptionDate` | `PrescriptionDate` (DateTime?) | `@PrescriptionDate` | `PrescriptionDate` |
| `#ddlPrescriptionType` | `PrescriptionTypeId` | int? | `@PrescriptionTypeId` | UNKNOWN col (SP body absent) |
| `#ddlDoctor` | `DoctorId` | int? | `@DoctorId` | `tbl_PrescriptionMaster.DoctorId` (column exists: referenced by `sp_Update_DoctorMaster:805`) |
| `#ddlChamber` | `ChemberId` | int? | `@ChemberId` | `tbl_PrescriptionMaster.ChemberId` (exists: `sp_Update_DoctorMaster:805`) |
| `#ddlCapturedBy` | `EntryBy` | string | `@EntryBy` | UNKNOWN |
| session UserId | – | – | `@UpdateBy` (update only) | UNKNOWN |
| (not sent by JS) | `ImageName` | string (null) | `@ImageName` (null) | UNKNOWN |
| `#imgeBase64Str` | `ImageString` | string | not an SP param — written to file | file `<ImagePathValue>/<ImagePreName><PrescriptionId>.jpg` |
| product rows | `PrescriptionProductDetailDAOs[].ProductId` | list | `sp_Save_PrescriptionProductDetail(@PrescriptionId,@ProductId)` | UNKNOWN |

Read side (`sp_Get_Prescription_ByPrescriptionId`) returns columns `PrescriptionDate, PrescriptionTypeId, PrescriptionId, ChemberId, DoctorId, ImageName, ApprovalStatus, EntryBy, ImagePreName` (from `SetupDAL.cs:2647-2665`); `sp_Get_PrescriptionDetails_ByPrescriptionId` returns `ProductId, ProductName`.

## 9. Dropdown Mapping
See §7. Dropdown loader JS `SelectOption_DtTable_Async_True(url, control, valueField, textField, setId)` (in `_QuickDataAccess.js`, not read in detail); chamber loader `_GetChamber_ByDoctorId` adds a "--- Select  ---" option with value `0` and is **synchronous** (`async:false`).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Add to list | `SaveValue()` | ignores empty/"0" product; appends row (`id` counter) and reloads product dropdown; no duplicate check |
| remove (row) | `RemoveRow(id)` | removes `#addr<id>` (on edit-loaded rows the id counter restarts at 0 so ids can collide with newly added rows — INFERRED from code: `id` var is only initialised to 0 and edit rows use their own index) |
| Submit/Update | `Save()` → `FinalSave()` | see §11/§12 |
| file input | `ImageToBase64(this)` | reads `files[0]` and stores base64 (part after comma) |

## 11. Frontend Flow
1. Ready: init pickadate. `$(function)`: read `id` from URL. If `id>0`: `GetProduct(0)` and `GetData(id)`; else load captured-by, doctor, prescription-type, product lists.
2. `GetData(id)` → `Prescription.aspx/GetPrescriptionEditData` → if `ApprovalStatus=="2"` hides the Submit button (client-side lock only), else relabels button "Update"; fills dropdowns, date (`ToJavaScriptDate_Formater`), chamber, then `GetPrescriptionDetails` → rows; shows image from `ImageString` base64 (`data:image/jpeg`).
3. Doctor change → chamber list for that doctor.
4. `Save()`: if `#imgeBase64Str` empty → invalid tooltip "Please Choose an Image!"; **otherwise `FinalSave()` is called directly**. The complete-form `Validation()` function (lines 472-547) is **never invoked** anywhere (BR-M16-038).
5. `FinalSave()` builds `jsonData` (`DoctorId, PrescriptionId, PrescriptionDate, EntryBy, PrescriptionTypeId, ChemberId, ImageString, PrescriptionProductDetailDAOs[]`) and POSTs JSON to `Prescription.aspx/Save_Prescription`; success → `successalert('Operation successful!','Success','PrescriptionView.aspx')`, else `faildalert('Operation Faild!')`.
Note: string values (e.g. `DoctorId "5"`, empty strings for unselected values) are deserialised into `int?`/`DateTime?` DAO properties by the ASP.NET JSON serializer; an empty string for an int property would fail deserialisation (INFERRED from framework behaviour, not exercised).

## 12. Backend Flow
`Prescription.aspx.cs`: `Save_Prescription(PrescriptionMasterDAO)` (static WebMethod, `Prescription.aspx.cs:37-43`) reads `Session["UserId"].ToString()` (NullReference if session gone) → `SetupDAL.Save_Prescription(typeMaster, userId)` (`SetupDAL.cs:4761-4873`):
1. Params `@PrescriptionId, @PrescriptionDate, @PrescriptionTypeId, @DoctorId, @ChemberId, @ImageName` (ImageName null ⇒ a `SqlParameter` with `null` value is **omitted** by ADO.NET → SP default/behaviour UNKNOWN).
2. `PrescriptionId>0` ⇒ add `@EntryBy` (original captured-by from UI) and `@UpdateBy`(session) → `UpdateData("sp_Update_PrescriptionMaster")`; `isSuccess=true`, `pk=PrescriptionId`. Else add `@EntryBy` → `SaveDataReturnPrimaryKey("sp_Save_PrescriptionMaster")`; `pk>0 ⇒ isSuccess=true`.
3. If `pk>0`: if `ImageString != ""` → `AppPrimaryDAL.GetImagePath("Prescription")` (`sp_Webapi_Get_ImagePath`) → `ImagePathValue` directory (created if missing) → `File.WriteAllBytes(<dir>/<ImagePreName><pk>.jpg, base64)` (server filesystem write, overwrites). Then for each product → `SaveDataReturnPrimaryKey("sp_Save_PrescriptionProductDetail",@PrescriptionId=pk,@ProductId)`.
4. Implicit transaction (see PAGE-669 §12); the image file is written **before** commit, so a later SQL failure leaves an orphan file. Exceptions: rollback + rethrow → WebMethod returns HTTP 500 → JS `faildalert`.
On update no delete of the old product rows is visible in C#; whether `sp_Update_PrescriptionMaster` or `sp_Save_PrescriptionProductDetail` removes/de-duplicates old rows is `UNKNOWN` (risk: duplicated product rows on every update).

## 13. API Mapping
| Endpoint | Method | Purpose |
|---|---|---|
| `Prescription.aspx/GetPrescriptionEditData(id)` | POST JSON | `SetupDAL.GetEditData_Prescription` (reads image file from disk into base64 with try/catch; missing file ⇒ `ImageString` = DB `ImageName` text) |
| `Prescription.aspx/GetPrescriptionDetailsListForEdit(id)` | POST | product rows JSON |
| `Prescription.aspx/Save_Prescription(typeMaster)` | POST | save |
| `Setup.aspx/Get_CapturedBy_For_ddl`, `Get_Doctor_For_ddl`, `Get_PrescriptionType_For_ddl`, `Get_ProductList_List_New` | POST | dropdowns |
| `CommonDataLoad.aspx/GetChamber_ByDoctorId(id)` | POST | chambers |
None of these WebMethods check authorization/roles in code.

## 14. Service Mapping
No BLL. Page → static `SetupDAL` (`private static SetupDAL _setupDAL`, shared instance).

## 15. Repository Mapping
See §6/§12. Dropdown data through `Setup2DAL` (Setup.aspx uses `Setup2DAL`, Prescription.aspx uses `SetupDAL`).

## 16. Stored Procedure Mapping
`sp_Save_PrescriptionMaster`, `sp_Update_PrescriptionMaster`, `sp_Save_PrescriptionProductDetail`, `sp_Get_Prescription_ByPrescriptionId`, `sp_Get_PrescriptionDetails_ByPrescriptionId`, `sp_Webapi_Get_ImagePath`, `sp_Get_CapturedBY_For_DDL`, `sp_Get_Doctor_For_DDL`, `sp_Get_PrescriptionType_For_DDL`, `sp_Get_Product_List`, `sp_GetChamber_ByDoctorId` — **all bodies absent** from repo.

## 17. Database Mapping
Written (by SP names): prescription master + product detail tables (names UNKNOWN; `tbl_PrescriptionMaster` is the master per `sp_Update_DoctorMaster:805` and `PrescriptionView` filter aliases `PM.*`). Filesystem write of `.jpg`.

## 18. Validation Matrix
| Check | Client | Server |
|---|---|---|
| Image chosen | Yes ("Please Choose an Image!") | none (a null `ImageString` passes `!= ""` and `Convert.FromBase64String(null)` throws `ArgumentNullException`; INFERRED) |
| Captured By, Date, Doctor, Chamber, Type, ≥1 product | `Validation()` exists but **not called** | none |
| Duplicate products | none | none |
| Approved lock (`ApprovalStatus==2`) | Submit hidden on edit | none (direct POST can still update) |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-037 | A prescription image is mandatory before submit (client only) | `Solution.Web/DoctorModule_UI/Prescription.aspx:432-470` | `Save()` | CONFIRMED |
| BR-M16-038 | The full-form validation (captured-by, date, doctor, chamber, type, ≥1 product) is dead code: `Validation()` is never called, so incomplete prescriptions are posted | `Prescription.aspx:472-547` vs `:432-470` | JS | CONFIRMED |
| BR-M16-039 | Chambers are restricted to the chosen doctor's chambers (`sp_GetChamber_ByDoctorId`) | `Prescription.aspx:313-325`; `_QuickDataAccess.js:779-800` | JS | CONFIRMED |
| BR-M16-040 | Save flow: insert if `PrescriptionId==0` else update (`@UpdateBy`=session user); on success write image to configured path as `<prefix><PrescriptionId>.jpg` (always .jpg) then insert each product detail | `SetupDAL.cs:4761-4855` | `Save_Prescription` | CONFIRMED |
| BR-M16-041 | An Approved prescription (`ApprovalStatus=="2"`) cannot be edited **in the UI** (button hidden); not enforced server-side | `Prescription.aspx:632-638`; `SetupDAL.cs:4789-4801` | JS only | CONFIRMED |
| BR-M16-042 | Same product can be added several times; no de-duplication client or server (server side of detail SP UNKNOWN) | `Prescription.aspx:351-385` | JS | CONFIRMED (client) |
| BR-M16-043 | `EntryBy` is the "Captured By" user chosen on the form (user with app access) and is sent on both insert and update; the session user is recorded only as `UpdateBy` on update | `Prescription.aspx:558`; `SetupDAL.cs:4791-4792,4804`; legacy inline SQL `SetupDAL.cs:25-26` (`IsAppsUser=1`) | JS + DAL | INFERRED (SP `sp_Get_CapturedBY_For_DDL` body absent) |

## 20. Permission Matrix
No server permission check (no `GetPermissionForUserRole`, no role test). Only `Session["UserId"]` used (unchecked). Menu gating only.

## 21. Status Lifecycle
Column `ApprovalStatus` on the prescription master (returned by `sp_Get_Prescription_ByPrescriptionId`; JS treats `"2"` as Approved/locked). New rows start in a state set inside `sp_Save_PrescriptionMaster` (UNKNOWN; `PrescriptionApprovalList` lists rows where `ApprovalStatus <> 'Approved'`, `PrescriptionApprovalList.aspx:243`). Value domain UNKNOWN: numeric `'2'` vs text `'Approved'` are both used by different pages (GAP-M16-021). Transitions are made by `sp_ApprovePrescriptionInformation` (PAGE-654).

## 22. Error Handling
JS: `faildalert('Operation Faild!')` on any failure/HTTP error. Server: rethrow after rollback; `GetEditData_Prescription` swallows image-read errors. No logging.

## 23. Audit
`EntryBy`, `UpdateBy` params; dates by SP (UNKNOWN). Image file overwritten with no history.

## 24. Integration
File system path from `sp_Webapi_Get_ImagePath` (type "Prescription") shared with the mobile API (INFERRED from `Webapi` name); web server must have write access to that path.

## 25. Reports
Prescription data feeds RX pivot reports (PAGE-675) and DoctorInfoReport.

## 26. Dependencies
pickadate, select2, `_QuickDataAccess.js`, `CommonDataLoad.aspx`, `Setup.aspx`, `AppPrimaryDAL.GetImagePath`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Submit without image | tooltip "Please Choose an Image!", no request | 037 |
| T2 | Image + nothing else | request is sent (validation dead) — expect server/SP failure or bad row | 038 |
| T3 | Complete new prescription with 2 products | master + 2 detail rows, `<prefix><id>.jpg` written | 040 |
| T4 | Edit approved prescription (status 2) | Submit hidden; direct POST still updates | 041 |
| T5 | Change doctor | chamber list reloads for doctor | 039 |
| T6 | Add same product twice | two rows shown/posted | 042 |
| T7 | Update existing prescription | verify (DB) whether product rows are replaced or duplicated | 040 |
| T8 | Image path not writable | exception, HTTP 500, "Operation Faild!" | 040 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Required fields | None existed | only image enforced | – | n/a | BR-M16-037/038 |
| Approved lock | None existed | client only | – | documented | BR-M16-041 |

## 30. Known Gaps
GAP-M16-011 (validation dead code), GAP-M16-012 (no server-side approved lock), GAP-M16-013 (image written before commit; hard-coded `.jpg`, no size/type validation, base64 in JSON), GAP-M16-014 (duplicate `Save_Prescription` in `Setup2DAL`), GAP-M16-015 (product detail dedupe/replace on update UNKNOWN), GAP-M16-016 (no permission check), GAP-M16-017 (`ImageName` param null).

## 31. Change Impact
Shares `SetupDAL`/`Setup2DAL` with many DoctorModule_UI pages (HR/field-force: Attendance, Mileage, Expense — other modules). `CommonDataLoad.aspx/GetChamber_ByDoctorId` shared with other pages using `_GetChamber_ByDoctorId`. `sp_Get_Product_List` shared with report pages (PAGE-675). Renaming approval values would affect PAGE-654/657.
