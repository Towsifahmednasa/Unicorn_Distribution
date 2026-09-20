---
page_id: PAGE-016
module: M02
page: UserSetupforEMP
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-016 — UserSetupforEMP (employee-oriented user setup; older variant of PAGE-015)

> **Evidence level:** Tier A for code-behind and DAL (identical `UserInfoDAL` to PAGE-015, see there for the SP list and their UNKNOWN bodies). Markup was inspected for control ids/visibility only (ui PARTIAL). No inbound link to this page exists in `Solution.Web` (grep for `UserSetupforEMP`: only its own files), so it looks superseded by `DoctorModule_UI/UserSetup.aspx?EMPMID=` (INFERRED).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-016 |
| Module | M02 |
| Kind | Entry/edit form |
| Folder | `Solution.Web/MasterSetup_UI` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `MasterSetup_UI_UserSetupforEMP` |
| Size | aspx 510 lines, code-behind 761 lines |

## 2. Purpose
Create/edit a user account for an *employee* (types 1/3): employee, user/login name, password, role, mobile access with mandatory IMEIs, active status, and either market access (all roles) or DC permission (role 14 only).

## 3. Business Objective
Same as PAGE-015 for employee accounts; reached from the employee master in the older flow (`lbemp` link goes to `EmployeeSetup.aspx?MID=`). INFERRED.

## 4. Navigation
Inbound: none found. Outbound: success → `../user-records` (route alias mapped in `Global.asax:9-13` to `~/DoctorModule_UI/UserRecords.aspx`); Reset → `UserSetup.aspx` (**the other page**, not itself); `lbemp_Click` → `../MasterSetup_UI/EmployeeSetup.aspx?MID=<id_mastetID>`.

## 5. Route
`~/MasterSetup_UI/UserSetupforEMP.aspx[?MID=<UserId>|?EmpID=<EmpInfoId>]`.

## 6. Source Files
`UserSetupforEMP.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/UserInfoDAL.cs`, `SeedDataDAL.cs`, `IVMarketStructure.ascx`.

## 7. UI Control Inventory (differences from PAGE-015)
| Control | Notes |
|---|---|
| `ddlUserType` (autopostback) | employee panel `DivEmp` for types 1/3 only; no DA branch |
| `ddlEmployeeName` | required for types 1/3 |
| `txtUserName`, `txtLoginName`, `txtPassword` | required; password `type="text"` (plain) |
| `ddlUserRole` (autopostback), `loadUserRole` | role required |
| `chkMobileAccess` → `divMei`, `txtImei1`, `txtImei2` | **IMEI 1 and 2 mandatory when mobile access is ticked** (`:350-367`) |
| `chkIsActive`, `txtacDate` | as PAGE-015; date mandatory |
| Market block (`divmrkAccess`, `gv_Market`) | **visible** here (not wrapped in an invisible parent) except when role = 14 |
| `dcDiv`, `ddlDistributionCenter` | visible only when role id **14** (`:723-727`) |
| `lbemp` LinkButton | enabled only when a user id is present |
| no `rbDashboard` | dashboard flags are never set (`IsMainDashboard/IsDepotDashboard` stay `null` → DBNull) |
| `btnSave`/`btnUpdate`, `btnReset` | as PAGE-015 |

## 8. Field Mapping
_Sections 8–10 are documented together in this block._
Identical to PAGE-015 §8–§10 except: no DA id, `IsMainDashboard`/`IsDepotDashboard` not sent as values, `EmpInfoId` always from `ddlEmployeeName`. Save call: `UserInfoDAL.SaveUserInfo(aMaster, MarketList, DisArray, Session["UserId"])` (`UserSetupforEMP.aspx.cs:465`).



## 9. Dropdown Mapping
See §8 block above (combined §8–10).

## 10. Button Action Mapping
See §8 block above (combined §8–10).

## 11. Frontend Flow
Postbacks and SweetAlert only.

## 12. Backend Flow
Page_Load first load: `MID` → edit that **user id**; else `EmpID` → `GetUserSetupByEmpId(EmpID)` and then `id_mastetID.Value = dt.Rows[0]["EmpInfoId"]` (`:57`) followed by `GetOneRecord(id_mastetID.Value)`, which calls `sp_GET_UserMaster_ById(@id)` **with an employee id as if it were a user id**. When the employee has no user, `Rows[0]` throws and is swallowed; `id_mastetID` stays empty and the form opens for create. When the employee has a user, `id_mastetID` = EmpInfoId (not UserId) and the Save button (only `btnSave` is made visible in this branch) sends `@UserId = EmpInfoId` → **update path (`UserId>0`) against the wrong user row** (CONFIRMED code; effect INFERRED because `sp_GET_UserMaster_ByEmpId` result columns are unknown and `EmpInfoId` may not even exist in its result — then the code would throw and be swallowed). `btnSave_Click`/`SaveUserInfo` as in PAGE-015 §12.

## 13. API Mapping
_Sections 13–16 are documented together in this block._
None / none / `UserInfoDAL` and `SeedDataDAL` / same SP list as PAGE-015 (bodies UNKNOWN).



## 14. Service Mapping
See §13 block above (combined §13–16).

## 15. Repository Mapping
See §13 block above (combined §13–16).

## 16. Stored Procedure Mapping
See §13 block above (combined §13–16).

## 17. Database Mapping
As PAGE-015 (`tblUser`, `tblUserCompanyUnit`, user-market detail).

## 18. Validation Matrix
Order: user type; (type 1/3) employee; user name; login name; password; role; (mobile) IMEI1, IMEI2; active/inactive date — all "required", red border + tooltip `please fill out this field`, no popup.

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-014 | Employee users: types 1/3 require an employee; mobile access requires both IMEIs; DC block appears only for role 14 (market access for all other roles). | `UserSetupforEMP.aspx.cs:305-367,715-727` | CONFIRMED |
| BR-M02-015 | Dashboard preference is not maintained by this page (flags sent as null). | `:422-452` (no assignment) | CONFIRMED |
| BR-M02-016 | Opening with `?EmpID=` copies the employee's `EmpInfoId` into the "user id" hidden field, so a subsequent Save targets `UserId = EmpInfoId`. | `:50-64` | CONFIRMED (code) / INFERRED (effect) |

Rules BR-M02-001/003/007/009/010/011/013 of PAGE-015 apply equally (same validation text, same `SaveUserInfo`).

## 20. Permission Matrix
Same as PAGE-015: master-page login check only; no role check, no `UserPersmissionValidation`; `Session["UserId"]` used for `@EntryBy`.

## 21. Status Lifecycle
`UserStatus` Active/Inactive by checkbox (note: when loading, `chkIsActive` is only set to true, never explicitly false, `:172-175`; default markup is checked → an Inactive user opened here appears Active and would be saved Active — CONFIRMED default `Checked="true"` at `UserSetupforEMP.aspx:438`; this is a defect versus PAGE-015, which also unticks the box).

## 22. Error Handling
As PAGE-015 (`try{}catch{}` in loaders and `GetOneRecord`; rethrow in DAL).

## 23. Audit
_Sections 23–26 are documented together in this block._
`@EntryBy`; none; none; same as PAGE-015 + `EmployeeSetup.aspx`, route `user-records`.



## 24. Integration
See §23 block above (combined §23–26).

## 25. Reports
See §23 block above (combined §23–26).

## 26. Dependencies
See §23 block above (combined §23–26).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Employee user, mobile access ticked, IMEI blank | IMEI field flagged | 014 |
| TC-02 | Role 14 | DC block shown, market hidden | 014 |
| TC-03 | `?EmpID=` for an employee that has a user | form loads (if column exists) and Save targets `UserId=EmpInfoId` — verify against DB before use | 016 |
| TC-04 | `?EmpID=` for employee without user | blank create form | 016 |
| TC-05 | Save | popup then redirect `../user-records` | 014 |

## 28. Reverse Engineering References
`Solution.Web/MasterSetup_UI/UserSetupforEMP.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/UserInfoDAL.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Relationship to PAGE-015 | – | duplicate/older logic with divergent rules (IMEI mandatory, role 14 only, no DA, no dashboard) | GAP-M02-008 |

## 30. Known Gaps
GAP-M02-001, 002, 004, 005, 006, 008 (duplicate divergent page), 011 (`EmpID` → wrong user id).

## 31. Change Impact
Shares `UserInfoDAL.SaveUserInfo` with PAGE-015. Because it has no inbound links, removal likely has no runtime dependants, but the DB menu may still link to it (UNKNOWN).
