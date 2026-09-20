# WF-M16-04 — Prescription (RX) capture → list → approval

## Trigger & actors
- Capture: a back-office user (or, INFERRED, the mobile app via `Webapi` image path SPs) records a doctor's prescription with photo and products.
- Approval: a manager uses `PrescriptionApprovalList.aspx` (broken as coded).

## Preconditions
Doctor with at least one chamber (WF-M16-01), prescription types (WF-M16-02), products (`sp_Get_Product_List`), user with app access for "Captured By".

## Steps
1. `PrescriptionView.aspx` (PAGE-657) lists prescriptions: `SetupDAL.Get_PrescriptionList(param)` → `sp_Get_PrescriptionList(@param)`; default range = today; filters: date on `PM.PrescriptionDate`, `PM.ApprovalStatus`, `us.UserRoleID`, `em.EmpInfoId`, `PM.SmcTypeId_RX`, `PM.RegionId/AreaId/TerritoryId` (the hierarchy user control `DoctorVisit_UI/IVMasterStructureForDCR.ascx` is rendered on this page — unlike DCRList it is not hidden — so its Zone/Area/Territory filters work), role scoping MIO/AM/DZSM/NSM via `View_Webapi_EmployeeFieldForceInfo` (same code pattern as DCRList, `PrescriptionView.aspx.cs:132-170`). Add → `Prescription.aspx`; Edit → `Prescription.aspx?id=`.
2. `Prescription.aspx` (PAGE-653): choose Captured By, date, doctor (→ chamber list), type, add products, choose image; **only the image is validated** (BR-M16-037/038).
3. `FinalSave()` → `Prescription.aspx/Save_Prescription` → `SetupDAL.Save_Prescription` (BR-M16-040): `sp_Save_PrescriptionMaster` (new) or `sp_Update_PrescriptionMaster` (edit, `@UpdateBy`=session) → image saved to configured folder as `<prefix><PrescriptionId>.jpg` (path from `sp_Webapi_Get_ImagePath('Prescription')`) → each product `sp_Save_PrescriptionProductDetail`.
4. Approval list `PrescriptionApprovalList.aspx` (PAGE-654) loads `sp_Get_PrescriptionList` with `and PM.ApprovalStatus<>'Approved'`; user ticks rows, chooses Approve/Reject, Submit.
5. `SaveApproval()` throws `ReferenceError: rbValue is not defined` **before** the AJAX call (BR-M16-047) ⇒ nothing is submitted. (Intended path: `Setup.aspx/Approve_PrescriptionList` → `Setup2DAL.Approve_PrescriptionList` → `sp_ApprovePrescriptionInformation(@TPMaster CSV, @ApprovedBy session UserId, @Status 'Approve'|'Reject')`.)
6. Reports: RX pivots (PAGE-675) and DoctorInfoReport (PAGE-674) count prescriptions; approval status is an optional filter in PAGE-675.

## Data created / changed
| Object | Change |
|---|---|
| prescription master (`tbl_PrescriptionMaster`, alias `PM`) | insert/update via SPs (columns: PrescriptionId, PrescriptionDate, PrescriptionTypeId, DoctorId, ChemberId, ImageName, EntryBy, UpdateBy, ApprovalStatus, … UNKNOWN details) |
| prescription product detail | insert per product (update semantics UNKNOWN) |
| file system | `<ImagePathValue>/<ImagePreName><id>.jpg` written/overwritten |
| approval columns | intended `sp_ApprovePrescriptionInformation` (unreachable from the page) |

## Status lifecycle
`ApprovalStatus` (value domain UNKNOWN; `"2"` = Approved per `Prescription.aspx:632`, `'Approved'` text per approval list filter). Intended: New/Pending → Approved | Rejected. Web page cannot perform the transition. Approved rows are hidden from editing only in the UI (BR-M16-041).

## Branches / exceptions
- No image → tooltip; other fields missing → still posted.
- Image path unavailable → `Directory.CreateDirectory`/`WriteAllBytes` throws → HTTP 500 → "Operation Faild!".
- Update after approval via direct POST is possible.
- Duplicate product rows possible.

## Postconditions
Prescription with image and product list exists; not approved unless approved by another channel (mobile/DB).

## Related BR ids
BR-M16-037..048.

## Related pages
PAGE-653, 654, 655/656 (prescription type), 657, 675, 674.

## Tests
Create with image only (expect bad row / SP error), full create (2 products), edit and check for duplicated details, approve attempt (expect JS error), direct POST to `Approve_PrescriptionList` with valid ids (expect status change, no role check).
