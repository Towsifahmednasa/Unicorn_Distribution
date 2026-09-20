---
page_id: PAGE-654
module: M16
page: PrescriptionApprovalList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-654 — PrescriptionApprovalList

> **Evidence level:** Tier A — `PrescriptionApprovalList.aspx` (all JS), `.aspx.cs` (empty), `Setup.aspx.cs`, `Setup2DAL.cs`. SP bodies absent. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-654 |
| Module | M16 |
| Page | PrescriptionApprovalList — heading "Prescription List Approval" |
| Folder | DoctorModule_UI |
| Code-behind | 13 lines, empty `Page_Load` |

## 2. Purpose
Let an approver bulk-approve or reject not-yet-approved prescriptions: pick rows with checkboxes, choose Approve/Reject radio, press "Submit Information". CONFIRMED (UI); **the submit is broken by a JS defect** (BR-M16-047).

## 3. Business Objective
Second step of the RX lifecycle: only approved prescriptions should count in RX reports (reports have an `@ApprovalStatus` filter, PAGE-675) — INFERRED.

## 4. Navigation
Menu placement `UNKNOWN`. Success message redirects to itself (`PrescriptionApprovalList.aspx`). A legacy `editClick(id)` function (unused by rows) points to an MVC `@Url.Action("DoctorPlanDetailsView","DoctorVisit")` which WebForms never evaluates (dead code, `:303-307`).

## 5. Route
`~/DoctorModule_UI/PrescriptionApprovalList.aspx`.

## 6. Source Files
`Solution.Web/DoctorModule_UI/PrescriptionApprovalList.aspx`, `.aspx.cs`, `Solution.Web/DoctorModule_UI/Setup.aspx.cs:72-77,1103-1109`, `Library.DAL/DoctorModule_DAL/Setup2DAL.cs:993-1025` (`Approve_PrescriptionList`), `:3477-3490` (`Get_PrescriptionList`). (A commented-out copy of `Approve_PrescriptionList` exists in `SetupDAL.cs:625-650`.)

## 7. UI Control Inventory
| Control | Label | Behaviour |
|---|---|---|
| radio `#Approve` (checked by default, value `Approve`) / `#Reject` (value `Reject`) | "Approval Status *" | selects `radioValue` |
| button "Submit Information" | – | `SaveApproval()` |
| table `#dtTble` columns | SL, Prescription Date, Prescription Type, Doctor, Approval Status, checkbox | rows built in JS from `Get_PrescriptionList` JSON (`PrescriptionDate, PrescriptionType, DoctorName, ApprovalStatus, PrescriptionId`) |
| `#CheckAll` | header checkbox | checks/unchecks all rows |
Note: table `#dtTb` referenced in the DataTable init (`:290-297`) does not exist (table id is `dtTble`) ⇒ DataTable paging/search never initialised.

## 8. Field Mapping
| UI | Request field | Server param | SP param |
|---|---|---|---|
| checked rows' hidden `DoctorList[n].TPMaster` (= PrescriptionId) | `MyArry` = comma-joined ids (trailing comma trimmed via `lastIndexOf`) | `Setup.aspx/Approve_PrescriptionList(string MyArry, string rbValue)` | `@TPMaster` (CSV) |
| radio value | intended `rbValue` | `rbValue` | `@Status` ("Approve"/"Reject") |
| session UserId | – | `HttpContext.Current.Session["UserId"]` | `@ApprovedBy` |

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
`Submit Information` → `SaveApproval()` (`:163-239`).

## 11. Frontend Flow
On ready → `GetPrescription()` POSTs `Setup.aspx/Get_PrescriptionList` with `param = " and PM.ApprovalStatus<>'Approved' "` (raw SQL fragment built in the browser) and renders rows. `SaveApproval()` → `validation()` counts unchecked rows; if none checked → `alert("Please select at least one row from List!!!")`. Then builds `srt` = ids CSV and reads `radioValue`. The AJAX payload is `JSON.stringify({ 'MyArry': srt, 'rbValue': rbValue })` — **`rbValue` is not declared anywhere in the page or master page** (the local variable is `radioValue`) ⇒ `ReferenceError: rbValue is not defined` thrown inside `SaveApproval()` before `$.ajax`; nothing is sent (BR-M16-047). Success handler (never reached) would show `successalert(...,'PrescriptionApprovalList.aspx')`.

## 12. Backend Flow
`Setup.aspx.cs:72-77`: `[WebMethod] Approve_PrescriptionList(MyArry, rbValue)` → `Setup2DAL.Approve_PrescriptionList(Id=MyArry, rbValue, SessionUser)` → `UpdateData("sp_ApprovePrescriptionInformation", @TPMaster, @ApprovedBy, @Status)`; returns `ResultInfo`. Errors: rollback + rethrow. `Get_PrescriptionList(param)` → `Setup2DAL.Get_PrescriptionList` → `sp_Get_PrescriptionList(@param)` → JSON.

## 13. API Mapping
`Setup.aspx/Get_PrescriptionList` (POST JSON, param = raw SQL), `Setup.aspx/Approve_PrescriptionList` (POST JSON). Neither checks roles.

## 14. Service Mapping
None (page → `Setup2DAL` static).

## 15. Repository Mapping
`Setup2DAL.Get_PrescriptionList`, `Setup2DAL.Approve_PrescriptionList`.

## 16. Stored Procedure Mapping
`sp_Get_PrescriptionList` (body absent; alias `PM` = prescription master, filters seen from PrescriptionView: `PM.PrescriptionId, PM.PrescriptionDate, PM.ApprovalStatus, PM.SmcTypeId_RX, PM.RegionId, PM.AreaId, PM.TerritoryId, us.UserRoleID, em.EmpInfoId, View_Webapi_EmployeeFieldForceInfo.*`), `sp_ApprovePrescriptionInformation` (body absent).

## 17. Database Mapping
Reads prescription master joined to users/employees/field-force view (UNKNOWN details). Writes approval columns of prescription master (UNKNOWN).

## 18. Validation Matrix
| Rule | Where | Message |
|---|---|---|
| ≥1 row selected | JS `validation()` | "Please select at least one row from List!!!" |
| Value of radio | none | – |
| Role of caller | none | – |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-044 | Approval list shows prescriptions whose `ApprovalStatus` text is not `'Approved'` (filter built in the browser and passed to SP) | `Solution.Web/DoctorModule_UI/PrescriptionApprovalList.aspx:243-250` | JS | CONFIRMED |
| BR-M16-045 | At least one row must be ticked before submit | `PrescriptionApprovalList.aspx:133-161` | JS `validation()` | CONFIRMED |
| BR-M16-046 | Approval is bulk: comma-separated prescription ids + a status text ("Approve"/"Reject") + approver = session UserId are sent to `sp_ApprovePrescriptionInformation` | `Setup.aspx.cs:72-77`; `Setup2DAL.cs:993-1012` | WebMethod + DAL | CONFIRMED |
| BR-M16-047 | DEFECT: JS references undefined variable `rbValue` (should be `radioValue`), so the approve/reject request is never sent; the page cannot approve anything as coded | `PrescriptionApprovalList.aspx:201,211` | JS | CONFIRMED |
| BR-M16-048 | Status vocabulary is inconsistent: list filter uses `'Approved'`, radio posts `Approve`/`Reject`, `Prescription.aspx` treats `"2"` as approved, `DCRList`/`PrescriptionView` filter by `SoftwareUseId` from `sp_GET_ActionStatusList`; actual stored values are decided in SP bodies (UNKNOWN) | `PrescriptionApprovalList.aspx:243,41-43`; `Prescription.aspx:632`; `PrescriptionView.aspx.cs:82-86` | – | INFERRED |

## 20. Permission Matrix
None in code. Anyone reaching the URL (or POSTing to the WebMethods) can list and (if the JS defect is bypassed) approve. Role hierarchy (MIO/AM/DZSM/NSM scoping) that DCRList/PrescriptionView apply is **absent** here: the approval list is unscoped.

## 21. Status Lifecycle
Intended: (new/pending) → Approve → Approved, or Reject → Rejected, per selected row. Set by `sp_ApprovePrescriptionInformation` (body absent; transitions UNKNOWN). Reject/re-approve flow (as DCR toggles) not present here.

## 22. Error Handling
JS `faildalert('Operation Faild!')`; server rethrow; no logging. The JS ReferenceError surfaces only in the browser console.

## 23. Audit
`@ApprovedBy` = session UserId (stored by SP; UNKNOWN). No approval log table for prescriptions (contrast `tblDCRApprovalLog`).

## 24. Integration
None.

## 25. Reports
None.

## 26. Dependencies
jQuery, `successalert/faildalert`, `_open_LoadingPopUp_WithMsg` (master), `Setup.aspx`.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Open page | lists non-approved prescriptions | 044 |
| T2 | Submit with none ticked | alert message | 045 |
| T3 | Tick a row, Approve, Submit | as coded: JS ReferenceError, no request, no change | 047 |
| T4 | (after defect fixed, for regression) POST `Approve_PrescriptionList` with ids "1,2", "Approve" | rows change status; verify value stored | 046,048 |
| T5 | Call WebMethod as any logged-in user | succeeds — no role check | §20 |

## 28. Reverse Engineering References
Files in §6.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | – | – | MISSING | – |
| Approve action | None existed | cannot execute (JS defect) | – | defect | BR-M16-047 |

## 30. Known Gaps
GAP-M16-018 (undefined `rbValue`), GAP-M16-019 (unscoped list, no permission), GAP-M16-020 (raw SQL fragment from client; SQL injection surface), GAP-M16-021 (status vocabulary), GAP-M16-022 (`#dtTb` DataTable never initialised), GAP-M16-023 (dead MVC `editClick`).

## 31. Change Impact
`Setup.aspx` web methods shared with tour-plan pages (`TourPlanApproveList`, etc., other module). `sp_Get_PrescriptionList` shared with PrescriptionView (PAGE-657). `Setup2DAL.Approve_PrescriptionList` only used here.
