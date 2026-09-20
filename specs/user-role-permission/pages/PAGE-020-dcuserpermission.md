---
page_id: PAGE-020
module: M02
page: DCUserPermission
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-020 — DCUserPermission (user ↔ distribution centre / warehouse access)

> **Evidence level:** Tier A — aspx markup, code-behind, `DCUserPermissionBll`, `DCUserPermissionDal` (all inline SQL, fully read), `UserCompanyUnitDao`. `tblUserCompanyUnit`, `tblCompanyUnit`, `tblWearhouse`, `tblUser` DDL not in repo (database PARTIAL).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-020 |
| Module | M02 |
| Kind | Admin maintenance (legacy layout) |
| Folder | `Solution.Web/SInventory_UI` |
| Master | `~/MasterPages/MainMasterPage.master` (guard on every request) |
| Class | `SInventory_UI_DCUserPermission` |
| Size | aspx 192 lines, code-behind 192 |
| Inbound links | none found in `Solution.Web` |

## 2. Purpose
Choose a user and tick the distribution centres (company units) — and the central warehouse — the user may work with; Save replaces the user's set.

## 3. Business Objective
Scope users to DCs: `tblUserCompanyUnit` drives `Session["ComUnitId"]`/`DICCompanyUnitId`/`DICUnitId` at login (M01 BR-M01-003/004), which downstream stock, sales and report pages use for DC filtering (the "stock condition / DC permission" area). It also carries the flags `CWHPermission` (central warehouse) and `NationalReportPermission` (national-level reports). INFERRED semantics of flags from names/usages; writes CONFIRMED.

## 4. Navigation
No inbound link found; DB menu row UNKNOWN. UserSetup (PAGE-015/016) is the newer place that also writes `tblUserCompanyUnit`.

## 5. Route
`~/SInventory_UI/DCUserPermission.aspx`.

## 6. Source Files
`DCUserPermission.aspx(.cs)`, `Library.BLL/SInventory_BLL/DCUserPermissionBll.cs`, `Library.DAL/SInventory_DAL/DCUserPermissionDal.cs`, `Library.DAO/SInventory_Entities/UserCompanyUnitDao.cs`.

## 7. UI Control Inventory
| Control | Type | Notes |
|---|---|---|
| `userDropDownList` | DropDownList, autopostback (`OnTextChanged`) | `SELECT UserId, UserCode + ':' + UserName AS UserName FROM tblUser` (all users, no status filter; blank first item added by `LoadDropDownValue` — behaviour of that helper assumed) |
| `dcCheckBoxList` | CheckBoxList | `SELECT ComUnitId, ComUnitCode+':'+ComUnitName AS CompanyUnit FROM tblCompanyUnit UNION ALL SELECT 15, WearhouseCode+':'+WearhouseName FROM tblWearhouse` — warehouse rows are given the fixed id **15** |
| `submitButton` | Button → `submitButton_Click1` | – |
| `clearButton` | Button → `clearButton_Click` | – |
Within an UpdatePanel. Messages via JS `alert(...)`.

## 8. Field Mapping
| UI | DAO (`UserCompanyUnitDao`) | SQL param | Column |
|---|---|---|---|
| `userDropDownList.SelectedValue` | `UserId` | `@UserId` | `tblUserCompanyUnit.UserId` |
| each checked item value | `CompanyUnitId` | `@CompanyUnitId` | `tblUserCompanyUnit.CompanyUnitId` |
| item value == "15" | `CWHPermission=true` | `@CWHPermission` | `tblUserCompanyUnit.CWHPermission` |
| count of checked items excluding "15" == 11 | `NationalReportPermission=true` | `@NationalReportPermission` | `tblUserCompanyUnit.NationalReportPermission` |

## 9. Dropdown Mapping
See §7 (`DCUserPermissionDal.GetUserInfo`, `GetDCList`, both inline SQL, DB "SSIDB").

## 10. Button Action Mapping
| Button | Handler | Chain |
|---|---|---|
| user changed | `userDropDownList_OnTextChanged` | clear boxes; `GetDCUserPermissionById` (`SELECT * FROM tblUserCompanyUnit WHERE UserId=@UserId`) → tick boxes whose id equals `CompanyUnitId` |
| Submit | `submitButton_Click1` | `Validation()` → build list → `DCUserPermissionBll.LoadDCUserPermissionDataForSave` |
| Clear | `clearButton_Click` | reset user + boxes |

## 11. Frontend Flow
UpdatePanel postbacks; alert popups.

## 12. Backend Flow
`Validation()`: user selected else `You have to select an user !!!`; at least one box ticked else `You have to select at least one DC !!!`. `CountDCCheck()` counts ticked items excluding value "15". For each ticked item build a DAO (`CWHPermission` only for id 15; `NationalReportPermission=true` on **all** rows iff count == 11). BLL: `DeletePermissionInfo(userId)` (`DELETE FROM tblUserCompanyUnit WHERE UserId=@UserId`) then `SaveDCUserPermissionInfo` per DAO (`INSERT INTO tblUserCompanyUnit (UserId,CompanyUnitId,CWHPermission,NationalReportPermission) VALUES (...)`), `status` = result of the **last** insert. UI: `Permission Info Saved Successfully!!! ` / `Permission Info doesn't Saved Successfully!!! `, then `Clear()` on success. No transaction; delete is committed before inserts.

## 13. API Mapping
None.

## 14. Service Mapping
_Sections 14–15 are documented together in this block._
`DCUserPermissionBll` (`LoadUserInfo`, `LoadDCList`, `LoadDCUserPermissionDataForSave`, `LoadDCUserPermissionById`); `DCUserPermissionDal` (`GetUserInfo`, `GetDCList`, `SaveDCUserPermissionInfo`, `DeletePermissionInfo`, `GetDCUserPermissionById`, unused `CheckPermissionInfoAlreadyExistOrNot`) — all parameterised.



## 15. Repository Mapping
See §14 block above (combined §14–15).

## 16. Stored Procedure Mapping
None (inline SQL through `SInventorySql` / `ClsCommonInternalDAL`).

## 17. Database Mapping
`tblUserCompanyUnit` (UserComId identity? — in the legacy `UserBLL.SaveDataForUser` the key is generated by `MAX+1`; here it is not supplied, so it is presumably an identity: UNKNOWN), READ/DELETE/INSERT; `tblUser`, `tblCompanyUnit`, `tblWearhouse` READ.

## 18. Validation Matrix
| Rule | Message | BR |
|---|---|---|
| user required | `You have to select an user !!!` | BR-M02-036 |
| ≥ 1 DC | `You have to select at least one DC !!!` | BR-M02-036 |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-036 | A user must be selected and at least one DC/warehouse ticked to save. | `DCUserPermission.aspx.cs:145-173` | CONFIRMED |
| BR-M02-037 | Saving **replaces** the user's whole `tblUserCompanyUnit` set (delete then re-insert). Deleting and inserting are not atomic. | `DCUserPermissionBll.cs:26-38`; `DCUserPermissionDal.cs:29-60` | CONFIRMED |
| BR-M02-038 | The central warehouse is represented by fixed id `15`; ticking it sets `CWHPermission=true` for that row. | `DCUserPermission.aspx.cs:100-103`; `DCUserPermissionDal.cs:25` | CONFIRMED |
| BR-M02-039 | `NationalReportPermission=true` is set on every saved row only when **exactly 11** non-warehouse DCs are ticked (hard-coded 11 = presumed "all DCs"); ticking 10 or 12 gives false. | `DCUserPermission.aspx.cs:105-108,129-142` | CONFIRMED |
| BR-M02-040 | The DC list is all `tblCompanyUnit` rows (no active/type filter) plus all warehouses; the user list has no status filter. | `DCUserPermissionDal.cs:19,25` | CONFIRMED |
| BR-M02-041 | `tblUserCompanyUnit` has two writers with different columns: this page (with CWH/National flags, inline SQL) and UserSetup (`sp_Save_UserCompanyUnit(@UserId,@ComUnitId)`, no flags); on UserSetup edit the SP's treatment of existing rows is UNKNOWN. | `UserInfoDAL.cs:464` vs `DCUserPermissionDal.cs:31` | CONFIRMED (writers) / UNKNOWN (SP) |

## 20. Permission Matrix
`MainMasterPage` login check on every request (`Session["UserId"]` non-empty); no role/user-type check in the page; no `UserPersmissionValidation`. Any logged-in user with the URL can change anyone's DC scope, including their own (menu-hiding only).

## 21. Status Lifecycle
None.

## 22. Error Handling
No try/catch; `ShowMessageBox` uses `message.Replace("'", "\'")` (no-op in C#, so an apostrophe in a message would break the JS — the two fixed messages contain "doesn't", which **does** contain an apostrophe → the failure alert script is malformed and would not display: INFERRED from the code `String.Format("alert('{0}');", message)`).

## 23. Audit
None (no EntryBy/date; `tblUserCompanyUnit` has no such columns in the INSERT).

## 24. Integration
_Sections 24–25 are documented together in this block._
None. (Results are consumed indirectly by every DC-scoped report/page.)



## 25. Reports
See §24 block above (combined §24–25).

## 26. Dependencies
`SInventorySql`, `ClsCommonInternalDAL`, `MainMasterPage`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | No user chosen, click Submit | alert `You have to select an user !!!` | 036 |
| TC-02 | User, no DC | alert `You have to select at least one DC !!!` | 036 |
| TC-03 | Tick 2 DCs, save | rows replaced by 2 | 037 |
| TC-04 | Tick warehouse (15) | `CWHPermission=1` on that row | 038 |
| TC-05 | Tick exactly 11 DCs | `NationalReportPermission=1` on all rows | 039 |
| TC-06 | Tick 10 or 12 DCs | flag 0 | 039 |
| TC-07 | Select user with existing rows | boxes pre-ticked from `tblUserCompanyUnit` | 037 |
| TC-08 | DB error on 2nd insert | earlier rows exist, later missing; failure message may not display | 037 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/DCUserPermission.aspx(.cs)`, `Library.BLL/SInventory_BLL/DCUserPermissionBll.cs`, `Library.DAL/SInventory_DAL/DCUserPermissionDal.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| "All DCs" national-report rule | – | hard-coded 11 | GAP-M02-006 |

## 30. Known Gaps
GAP-M02-001, 004, 006 (hard-coded 11 and id 15), 016 (two writers to `tblUserCompanyUnit`), 017 (user list includes inactive; no DC filter).

## 31. Change Impact
Directly changes login session scope (`Login.aspx.cs` DICUnitId/ComUnitId) for the affected user at next login; shared table with PAGE-015/016; `CWHPermission`/`NationalReportPermission` have no C# reader (grep) — consumed in SPs/reports if at all (UNKNOWN).
