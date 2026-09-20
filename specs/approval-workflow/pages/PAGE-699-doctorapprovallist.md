---
page_id: PAGE-699
module: M18
page: DoctorApprovalList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-699 — DoctorApprovalList

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs` (430 LOC), `DoctorDAL.GetDoctor_ApplogDAL` (`Library.DAL/MasterSetup_DAL/DoctorDAL.cs:384`) and `SaveDoctor_ApplogDAL` (:321) read. SP bodies `sp_Get_DoctorClaimApp`, `sp_webapi_SaveDoctorAppLog` NOT in repository → `UNKNOWN`. (`sp_Save_DoctorMaster`/`sp_Update_DoctorMaster` bodies exist in `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:672/735` and inform the lifecycle, §21.)

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-699 / M18 |
| Page name | DoctorApprovalList (heading "Doctor List") |
| Kind | Approval list (master-data approval, single + bulk, no filters) |
| Folder / master | Approval_UI / NewMasterPage.master |
| Class | Approval_UI_DoctorApprovalList |
| Related | `MasterSetup_UI/DoctorEntry.aspx` (PAGE-669), `TransferUI/DoctorUpdationApprove.aspx` |

## 2. Purpose
Approve ("Verify") or reject doctor/provider master records created for the field force (name, code, sales person, designation, degree, speciality, entry by/date). INFERRED from columns and `Type="Doctor"`.

## 3. Business Objective
Keep the doctor (provider) master clean by having managers in the configured chain vet new records before they are used in call plans/reports. INFERRED.

## 4. Navigation
DB menu. Edit button opens `../MasterSetup_UI/DoctorEntry.aspx?MID={DoctorId}` in a new window (`.cs:89`). Hidden New Entry → `DoctorEntry.aspx`. No Reset/Search (no filter UI).

## 5. Route
`~/Approval_UI/DoctorApprovalList.aspx`.

## 6. Source Files
View, code-behind, `DoctorDAL`, `CommonDataLoad` (permission), DAO `SaveDoctorAppLog_DAO`, `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Notes |
|---|---|---|
| hfEmpTerrId/AreaId/RegionId/GroupId | hidden | never populated (market lookup commented out :344-380) → empty → sent as 0 |
| btnBulkApprove / btnBulkReject | Approve Selected / Reject Selected | confirm() |
| loadGridView | Doctor List | keys `DoctorId,DoctorApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep`; visible columns Doctor Code, Doctor Name, Sales Person Name, Designation, Degree, Doctor Speciality, Approval Status (ApprovalStatusWeb), Waiting For (WaitingForRole), Entry By (EmpMasterCode), Entry Date; hidden columns Pharma Platform, Provider Type, Zone, Area, Territory, Market Code, Market Name |
| lbEdit / lbApprove / lbReject / lbMsg | row actions | gating in §12 |
There are no filter controls, no date range, no Search/Reset, no DC dropdown on this page.

## 8. Field Mapping
`SaveDoctorAppLog_DAO` → `DoctorDAL.SaveDoctor_ApplogDAL` (:321) → `sp_webapi_SaveDoctorAppLog` via `SaveData`.
| UI | DTO | Value | SP param |
|---|---|---|---|
| hfDoctorApprovalId | DoctorApprovalId | :107 | `@DoctorApprovalId` |
| hfDoctorId | TableId | :109 | `@TableId` |
| hierarchy hidden fields | Group/Region/Area/Territory | `""`→0 | `@GroupId..@TerritoryId` |
| Session EmpInfoId / hfToEmpId | FromEmpId / ToEmpId | :116-117 | `@FromEmpId,@ToEmpId` |
| hfStep + 1 | Step | :118-119 | `@Step` |
| literal | Status "Verified" (approve :97) / "Rejected" (:146) | — | `@Status` |
| literal | Type "Doctor" | :120,:170,:225,:289 | `@Type` |
| literal | MenuId 303 | :127,:177,:232,:296 | `@MenuId` |
| now / UserId | Date, EntryDateS, ApproveDateS / EntryByS, EntryByApp | :122-126 | as named |
Table.column UNKNOWN.

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB |
|---|---|---|---|---|
| Approve | `RowCommand ApproveData` :95 | `SaveDoctor_ApplogDAL` | `sp_webapi_SaveDoctorAppLog` | approval-log step (UNKNOWN) |
| Reject | :144 | same | same | same |
| Approve/Reject Selected | :194 / :258 | same | same | per row |
| Edit | :83 | — | — | none |

## 11. Frontend Flow
Inline `SelectAllRows` toggles checkboxes; postbacks only.

## 12. Backend Flow
`Page_Load` :29-48: Session reads; first load → `UserPersmissionValidation` → `LoadData`; any exception → Dashboard. `LoadData` :338-422: no market lookup, no filters: `GetDoctor_ApplogDAL("", "", null,null,null,null)` → `sp_Get_DoctorClaimApp` (SP name says "Claim" but the grid is doctors; body absent, GAP-M18-001). Gate (:392-421): roles {5,4,14,9} see all actions; else rows whose `ToRoleTypeId != Session RoleTypeId` get "Waiting for Another Employee" (different text from other pages). Approve always writes "Verified" (even for last-step roles); reject "Rejected". Save: `SaveData` returns true unless exception; on exception rollback and rethrow (no page catch). Bulk approve/reject count success/failure from `Res.isSuccess` (:234-242).

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`DoctorDAL.GetDoctor_ApplogDAL` :384 → `sp_Get_DoctorClaimApp(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId)`; `DoctorDAL.SaveDoctor_ApplogDAL` :321 → `sp_webapi_SaveDoctorAppLog`; `DoctorDAL.GetDoctorList` :416 → `sp_Get_DoctorList` (body in repo, not called by this page).

## 16. Stored Procedure Mapping
`sp_Get_DoctorClaimApp`, `sp_webapi_SaveDoctorAppLog`, `sp_GET_MainPermissionByUserRoleandPageUrl` — bodies absent.

## 17. Database Mapping
Tables UNKNOWN via SPs. Lifecycle-relevant table from repo SPs: `tblDoctorMaster` (`ApprovalStatus`, `IsFromApp`, `DoctorCode`) — see §21.

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Bulk | none ticked/visible → "No doctors were approved/rejected. Please select records and ensure you have permission to approve/reject them." | verbatim |
| Single ok | "Operation successful!" | — |
| Single fail | "Already Exist!" (only if `isSuccess` false; effectively unreachable, BR-M18-013) | — |
| Bulk ok | "{n} doctor(s) approved/rejected successfully!" [+ " {m} failed."] | — |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-001 | Step = Step + 1 | DoctorApprovalList.aspx.cs:118-119 | CONFIRMED |
| BR-M18-002 | Waiting gate (message "Waiting for Another Employee") | :396-418 | CONFIRMED |
| BR-M18-003 | Exempt roles 5,4,14,9 | :392 | CONFIRMED |
| BR-M18-005 | MenuId 303 | :127 | CONFIRMED |
| BR-M18-009 | Audit fields | :122-126 | CONFIRMED |
| BR-M18-010 | Bulk semantics with counts | :200-255 | CONFIRMED |
| BR-M18-011 | Menu-permission check | :50-80 | CONFIRMED |
| BR-M18-013 | Save "success" = no exception | DataAccessManager.cs:447 | CONFIRMED |
| BR-M18-014 | Vocabulary "Verified"/"Rejected" | :97,:146 | CONFIRMED |
| BR-M18-021 | Approve is always "Verified" — a doctor is never marked "Accepted" by this page, whichever role acts | :97,:196 | CONFIRMED |
| BR-M18-022 | No scope/filters: every approver sees all doctors returned by the SP; hierarchy ids not sent | :344-380 | CONFIRMED |
| BR-M18-023 | Waiting message for doctors is "Waiting for Another Employee" | :416 | CONFIRMED |

## 20. Permission Matrix
Menu-permission row check unless UserRoleID==2; UI gating; no server re-check.

## 21. Status Lifecycle
`tblDoctorMaster.ApprovalStatus`: doctors inserted by the repo's `sp_Save_DoctorMaster` (LIVE_DB...sql:672-735) are stored with `ApprovalStatus='2'` and `IsFromApp=0` and a generated code `D<max+1>` (max over approved rows) — i.e. web-created doctors are approved at insert and are not in this queue. Records in the queue (approval log step with `Step`, `ToRoleTypeId`) come from another path (mobile app `IsFromApp` INFERRED). This page writes "Verified"/"Rejected" to the log; the SP's effect on `ApprovalStatus` is UNKNOWN.

## 22. Error Handling
Page_Load catch → Dashboard; no catch around save.

## 23. Audit
BR-M18-009.

## 24. Integration
`sp_webapi_*` naming: shared with API/mobile (INFERRED).

## 25. Reports
None.

## 26. Dependencies
DoctorDAL, CommonDataLoad, DAO `SaveDoctorAppLog_DAO`, `ResultInfo`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Approve as ToRole user | Status "Verified", Step+1, MenuId 303, Type "Doctor" | 001,005,021 |
| TC-02 | Role 5 approves | still "Verified" | 021 |
| TC-03 | Non-ToRole, non-exempt user | "Waiting for Another Employee" | 002,023 |
| TC-04 | Bulk approve, none ticked | error text §18 | 010 |
| TC-05 | Reject | "Rejected" | 014 |
| TC-06 | Load page | no filters shown | 022 |

## 28. Reverse Engineering References
`Solution.Web/Approval_UI/DoctorApprovalList.aspx(.cs)`, `DoctorDAL.cs:321,384`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:672-735`, WF-M18-03.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | §12 | MISSING |
| Final state | None existed | never "Accepted" | UNKNOWN (GAP-M18-026) |
| Scope | None existed | none | RISK |

## 30. Known Gaps
GAP-M18-001, 003, 005, 006, 015 (zero hierarchy), 022 (web-created records bypass approval), 026.

## 31. Change Impact
`DoctorDAL.SaveDoctor_ApplogDAL`/`GetDoctor_ApplogDAL`: only this page calls them (grep of `Solution.Web` verified for the `Save*` name pattern; `TransferUI/DoctorUpdationApprove` uses different DAL calls).
