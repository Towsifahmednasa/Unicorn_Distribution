---
page_id: PAGE-663
module: M16
page: DoctorVisitApprovalList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-663 — DoctorVisitApprovalList

> **Evidence level:** Tier A — `DoctorVisitApprovalList.aspx` (all JS), empty `.aspx.cs`, `DoctorVisitDAL.cs`, repo-wide grep for callers. SP bodies absent. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-663 |
| Module | M16 |
| Page | DoctorVisitApprovalList — heading "Doctor Visit Approval List" (approval of monthly Doctor Call Plans "DCP") |
| Folder | DoctorVisit_UI |
| Code-behind | 13 lines, empty `Page_Load` |
| Status | **Non-functional legacy port** (ASP.NET MVC/Razor code inside a WebForms page) |

## 2. Purpose
Intended: list monthly doctor visit plans (DCP / "TPMaster" rows: employee, designation, role, year, month, remarks, approval status) and bulk approve/reject the ticked plans. CONFIRMED (markup/JS).

## 3. Business Objective
Plan approval by a line manager before the month's visits start (INFERRED). The working approval of tour plans lives on other pages (`DoctorModule_UI/VisitPlannedApprovalList.aspx` → `Setup.aspx/Approve_TourPlanList`, other module).

## 4. Navigation
No inbound link from `.cs`/`.aspx` in `Solution.Web` (grep for `DoctorVisitApprovalList`). The legacy `MasterPages/MasterPage.master:1126` links to an external MVC host `http://45.64.134.85:580/DoctorVisit/DoctorVisitApprovalList` (cleartext HTTP, hard-coded IP). DB menu may point here (UNKNOWN).

## 5. Route
`~/DoctorVisit_UI/DoctorVisitApprovalList.aspx`.

## 6. Source Files
`Solution.Web/DoctorVisit_UI/DoctorVisitApprovalList.aspx`, `.aspx.cs`, `Library.DAL/DoctorVisit_DAL/DoctorVisitDAL.cs:114-132` (`GetDoctorVisitList` — exists), `:581-607` (`Approve_DoctorPlanList` — exists, **no caller**).

## 7. UI Control Inventory
Radio `Approve`/`Reject` (default Approve, values `Approved`/`Rejected`), "Submit" button (`SaveApproval()`), table `#dtTble` columns SL, Employee ID, Employee Name, Designation, User Role, Year, Month, Remarks (`FinalSubmitRemarks`), Approval Status, View, checkbox; hidden `DoctorList[n].TPMaster` = `DocTPMaster`.

## 8. Field Mapping
Intended: ticked `DocTPMaster` ids → `MyArry` (CSV), radio value → `rbValue` → `sp_ApproveDoctorTourPlanMaster(@TPMaster,@ApprovedBy,@Status)` via `DoctorVisitDAL.Approve_DoctorPlanList` (which nothing calls).

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
`SaveApproval()` (`:104-`), `editClick(id)`.

## 11. Frontend Flow
On ready: `GetPrescription()` (misnamed; loads plans) calls `$.ajax({url:'@Url.Action("GetDoctorVisitList","DoctorVisit")', type:"Get", dataType:'json', data:{param:""}})`. **`@Url.Action(...)` is Razor syntax; an `.aspx` WebForms page does not evaluate it**, so the literal text `@Url.Action("GetDoctorVisitList", "DoctorVisit")` is used as the URL ⇒ 404/`error` ⇒ table stays empty (BR-M16-034). Likewise `SaveApproval()` posts to `'@Url.Action("Approve_DoctorPlanList","DoctorVisit")'` and `editClick()` navigates to `'@Url.Action("DoctorPlanDetailsView","DoctorVisit")?id=…'`. The page's own code-behind has no WebMethods.

## 12. Backend Flow
None reachable. (`DoctorVisitDAL.GetDoctorVisitList` is reachable from `DoctorVisit.aspx` and `DoctorVisitReport.aspx` WebMethods, not from here.)

## 13. API Mapping
No WebMethod on this page; JS targets non-existent MVC actions `DoctorVisit/GetDoctorVisitList`, `DoctorVisit/Approve_DoctorPlanList`, `DoctorVisit/DoctorPlanDetailsView` (no MVC controller in repo — grep for `Approve_DoctorPlanList` finds only the DAL method).

## 14. Service Mapping
None.

## 15. Repository Mapping
`DoctorVisitDAL.Approve_DoctorPlanList` → `UpdateData("sp_ApproveDoctorTourPlanMaster")` — orphan.

## 16. Stored Procedure Mapping
`sp_ApproveDoctorTourPlanMaster` (orphan, body absent), `sp_Get_DoctorTourPlanMasteList` (via other pages, body absent).

## 17. Database Mapping
Doctor tour plan master (`DocTPMaster`, `YearValue`, `MonthValue`, `FinalSubmitRemarks`, `ApprovalStatus`) — from column names in the sibling list page; table name UNKNOWN.

## 18. Validation Matrix
Only JS "Please select at least one row from List!!!" (`alert`).

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M16-034 | This approval page cannot work: all three AJAX/navigation targets use Razor `@Url.Action` in a WebForms page and no WebMethod/controller exists; `DoctorVisitDAL.Approve_DoctorPlanList` has no caller | `Solution.Web/DoctorVisit_UI/DoctorVisitApprovalList.aspx` (`@Url.Action` occurrences in `GetPrescription`, `SaveApproval`, `editClick`); `Library.DAL/DoctorVisit_DAL/DoctorVisitDAL.cs:581` | markup/JS | CONFIRMED |

## 20. Permission Matrix
None (no server code). If the DAL method were wired, it would take `SessionUser` only; no role check.

## 21. Status Lifecycle
Intended: plan `Pending → Approved | Rejected` with values `Approved`/`Rejected` passed as `@Status`. Not executable here; actual tour-plan approval is in another module (`VisitPlannedApprovalList`, radio value `2` = Approve).

## 22. Error Handling
JS `_saveError()` on failure (never reached meaningfully).

## 23. Audit
`@ApprovedBy` param (unused path).

## 24. Integration
Legacy MVC application at external host (see §4) is presumably the origin of this markup — INFERRED.

## 25. Reports
None.

## 26. Dependencies
jQuery, `jquery-confirm`, DataTables.

## 27. Test Cases
| # | Case | Expected | BR |
|---|---|---|---|
| T1 | Open page | table empty; browser console shows failed request to a literal `@Url.Action(...)` URL | 034 |
| T2 | Tick a row, Submit | request fails → `_saveError()` | 034 |
| T3 | Repo search for callers of `Approve_DoctorPlanList` | none | 034 |

## 28. Reverse Engineering References
Files in §6; `MasterPages/MasterPage.master:1126`.

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | non-functional | – | MISSING | – |

## 30. Known Gaps
GAP-M16-031 (page non-functional), GAP-M16-032 (orphan `Approve_DoctorPlanList`/`sp_ApproveDoctorTourPlanMaster`), GAP-M16-033 (legacy menu link to external IP over HTTP; `MasterPage.master:1126`).

## 31. Change Impact
Removing/repairing this page affects nothing else in code; if the DB menu links here, users reach a dead page. `DoctorVisitDAL` shared with PAGE-658/659/660/661/662/664/665/666/667.
