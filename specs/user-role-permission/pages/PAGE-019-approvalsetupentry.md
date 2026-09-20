---
page_id: PAGE-019
module: M02
page: ApprovalSetupEntry
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-019 — ApprovalSetupEntry (approver users per approval page, table `tblAppSetup`)

> **Evidence level:** Tier A — code-behind, `AppSetupDAL` (all inline SQL), consumers `DcStockOutApproval`, `SubDepot_UI/SubDepotStockOutApproval`, `DoctorCustomerTransferApproval`, `WarehouseStockInApprovalDal`, `DeStockOutDal`, `SubDepotStockAdjustmentsVoucherDal` read. Markup skimmed (ui PARTIAL). Table DDL not in repo.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-019 |
| Module | M02 |
| Kind | Entry grid (legacy) |
| Folder | `Solution.Web/SInventory_UI` |
| Master | `~/MasterPages/MainMasterPage.master` (guard on every request) |
| Class | `SInventory_UI_ApprovalSetupEntry`; size aspx ≈190 / code-behind 200 |
| Inbound links | none found |

## 2. Purpose
For a menu row flagged as an approval page (`tblMainMenu.IsApprovalPage='1'`) maintain the list of users (with e-mail) who are its approvers.

## 3. Business Objective
Restrict who may approve legacy stock documents (DC stock-out, warehouse stock-in, sub-depot adjustments, customer transfer) to named users. INFERRED from names and the consumers' `GetAssignedAppUser`.

## 4. Navigation
None found. Menu row UNKNOWN.

## 5. Route
`~/SInventory_UI/ApprovalSetupEntry.aspx`.

## 6. Source Files
`ApprovalSetupEntry.aspx(.cs)`, `Library.DAL/SInventory_DAL/AppSetupDAL.cs`, `AppSetupDAO`.

## 7. UI Control Inventory
`menuDropDownList` (autopostback; `SELECT * FROM dbo.tblMainMenu where IsApprovalPage='1'`, text `ManuName`, value `SL`); `loadGridView` rows: `userDropDownList` (autopostback; `SELECT * FROM dbo.tblUser WHERE UserStatus='Active'`, `UserName`/`UserId`), `emailTextBox`, `ImageButton1` (add row), `ImageButton2` (delete row); `submitButton`. Messages via `alert()`.

## 8. Field Mapping
| UI | DAO (`AppSetupDAO`) | SQL param | Column |
|---|---|---|---|
| `menuDropDownList` | `SL` | `@SL` | `tblAppSetup.SL` (= `tblMainMenu.SL`) |
| row `userDropDownList` | `UserId` | `@UserId` | `tblAppSetup.UserId` |
| row `emailTextBox` | `Email` | `@Email` | `tblAppSetup.Email` |
| `Session["LoginName"]` | `EntryBy` | `@EntryBy` | `tblAppSetup.EntryBy` |
| `DateTime.Now` | `EntryDate` | `@EntryDate` | `tblAppSetup.EntryDate` |

## 9. Dropdown Mapping
_Sections 9–11 are documented together in this block._
As §7. `ImageButton1_Click` rebuilds the grid from current rows plus one blank row; `ImageButton2_Click` removes the clicked row; UpdatePanel postbacks only.



## 10. Button Action Mapping
See §9 block above (combined §9–11).

## 11. Frontend Flow
See §9 block above (combined §9–11).

## 12. Backend Flow
Menu selected → `LoadAppSetup(" WHERE tblAppSetup.SL='" + menu + "'")` (**string-concatenated filter**; the query also `LEFT JOIN tblUser ON tblUser.UserId = tblAppSetup.SL`, i.e. joined on the wrong column, but only `tblAppSetup` columns are consumed) → rows bound and per-row user dropdown selected (`Rows[i]["UserId"]`; empty result → blank grid). Submit: `DeleteAreaInfo(menu)` (`DELETE FROM tblAppSetup WHERE SL=@SL`) then, for every grid row, `SaveAppSetup` (`INSERT INTO tblAppSetup (SL,UserId,Email,EntryBy,EntryDate)`); then `Clear()` and alert `Data Saved Successfully`. No transaction, no result checks, no validation (blank user → `Convert.ToInt32("")` throws after the delete has already committed).

## 13. API Mapping
_Sections 13–16 are documented together in this block._
None / none / `AppSetupDAL` (`SaveAppSetup`, `LoadAppSetup`, `DeleteAreaInfo`, `LoadMenuName`, `LoadUser`; `UpdateAreaInfo` unused) / no SP.



## 14. Service Mapping
See §13 block above (combined §13–16).

## 15. Repository Mapping
See §13 block above (combined §13–16).

## 16. Stored Procedure Mapping
See §13 block above (combined §13–16).

## 17. Database Mapping
`tblAppSetup(AppSetupId, SL, UserId, Email, EntryBy, EntryDate)` R/W/D; `tblMainMenu`, `tblUser` R.

## 18. Validation Matrix
None (no server or client validation).

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-046 | Approvers are stored per approval menu row (`SL`) as (user, e-mail); saving replaces all rows of that menu. | `ApprovalSetupEntry.aspx.cs:163-184`; `AppSetupDAL.cs:16,51` | CONFIRMED |
| BR-M02-047 | Only menus with `IsApprovalPage='1'` and users with `UserStatus='Active'` are selectable. | `AppSetupDAL.cs:63,69` | CONFIRMED |
| BR-M02-048 | Consumers determine a user's approver status with `GetMenuIdByMenuName(page file name)` (`tblMainMenu.URL LIKE '%name%'`) then `GetAssignedAppUser(SL, Session["UserId"])`. The check (`DataTrue()`) is **live only** in `SubDepot_UI/SubDepotStockOutApproval.aspx.cs:29` (and only hides the initial list on first load; postback actions are unguarded); in `DcStockOutApproval`, `WarehouseStockInApproval`, `DoctorCustomerTransferApproval` the call is commented out (`//if (DataTrue())`). | `SubDepotStockOutApproval.aspx.cs:29`; `DcStockOutApproval.aspx.cs:22`; `WarehouseStockInApproval.aspx.cs:20`; `DoctorCustomerTransferApproval.aspx.cs:29` | CONFIRMED |
| BR-M02-049 | `WarehouseStockInApprovalDal.GetAssignedAppUser/GetMenuIdByMenuName` build SQL by string concatenation (`SL='"+menuid+"'`, `URL like '%"+menuname+"%'`); `LoadApprovalControlBLL` etc. are separate. | `WarehouseStockInApprovalDal.cs:48-56` | CONFIRMED |

## 20. Permission Matrix
`MainMasterPage` login guard only; no role check; any logged-in user can appoint approvers (menu-hiding only). Effectively the mapping is largely unenforced (BR-M02-048).

## 21. Status Lifecycle
None.

## 22. Error Handling
None (exceptions unhandled); `showMessageBox` no-op escape as in PAGE-020.

## 23. Audit
`EntryBy` (LoginName), `EntryDate`.

## 24. Integration
_Sections 24–25 are documented together in this block._
None.



## 25. Reports
See §24 block above (combined §24–25).

## 26. Dependencies
`AppSetupDAL`, `SInventorySql`, `ClsCommonInternalDAL`, `tblMainMenu` (legacy menu, M01 PAGE-002).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Choose menu, add 2 users, Submit | old rows deleted, 2 inserted | 046 |
| TC-02 | Blank user row | exception after delete (approvers lost) | 046 |
| TC-03 | Non-approver opens DcStockOutApproval | list still shown (check commented out) | 048 |
| TC-04 | Non-approver opens SubDepot_UI SubDepotStockOutApproval | list not loaded (postback actions unguarded) | 048 |

## 28. Reverse Engineering References
`Solution.Web/SInventory_UI/ApprovalSetupEntry.aspx(.cs)`, `Library.DAL/SInventory_DAL/AppSetupDAL.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Approver enforcement | – | mostly disabled | GAP-M02-025 |

## 30. Known Gaps
GAP-M02-001, 004, 015 (parallel mechanisms), 024 (string-concatenated SQL, wrong join), 025 (approver check disabled).

## 31. Change Impact
`tblAppSetup` is read by 3 DALs listed above; `tblMainMenu.URL` values (M01 PAGE-002) are matched with `LIKE`, so URL edits change approver resolution.
