---
page_id: PAGE-026
module: M02
page: ApprovalStepMap
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-026 — ApprovalStepMap (approval chain by role type per menu)

> **Evidence level:** Tier A for page/JS/DAL. SPs `sp_GET_RoleType`, `sp_GET_MainMenuByType`, `sp_GET_ApprovalMapLoad`, `sp_Save_ApprovalMapMaster`, `sp_Save_ApprovalMapDetail` are **not in the repository** (database PARTIAL). The consuming side (how the chain is used by approval SPs/pages) is largely UNKNOWN (rules PARTIAL).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-026 |
| Module | M02 |
| Kind | Approval configuration (AJAX form) |
| Folder | `Solution.Web/UserPermission` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `UserPermission_ApprovalStepMap` (static WebMethods only) |
| Size | aspx 733 lines (roughly half legacy/copy-paste JS), code-behind 58 |

## 2. Purpose
For one **menu (approval page)** and one **originating role type** define the ordered chain of *role types* to which an item moves for approval (e.g. role A → order 1 role B → order 2 role C).

## 3. Business Objective
Configure multi-step approval routing without code: which role type approves after which, per menu/page. INFERRED (labels "Role Forward", "Order"; DAL names `FromRoleId`, `ToRoleId`, `Order`).

## 4. Navigation
No inbound link in code; DB-driven menu (UNKNOWN). Success → `ApprovalStepMap.aspx` reload.

## 5. Route
`~/UserPermission/ApprovalStepMap.aspx`; endpoints `ApprovalStepMap.aspx/{SaveMenu,GetUserRole,GetMenu,LoadApprovalMap}`.

## 6. Source Files
`ApprovalStepMap.aspx(.cs)`, `Library.DAL/UserRoleDAL/ApprovalMapDAL.cs`, `Library.DAO/UserRoleDAO/ApprovalMapMaster.cs` (contains `ApprovalMapMaster` and `ApprovalMapDetail`), `_myCusGen_Func.js`.

## 7. UI Control Inventory
| Control | Label | Required | Default / behaviour |
|---|---|---|---|
| `#typeSelect` (inside `display:none` row) | Type | – | fixed `App` (value 2); options Select One/Web/App; on change reloads menu list (the server ignores it) |
| `#menuSelect` | Menu | not validated | options from `GetMenu` (`SL`/`ManuName`); change clears the chain table and reloads the From-role list |
| `#roleSelect` | From role (role type) | client: must not be `0` | options from `GetUserRole` (`RoleTypeId`/`RoleType`); change → `GetAppMapData()` loads existing chain |
| `#roleFSelect` | Role Forward (To role) | required for add (`roleid != "0"`, else alert `Please Select Role Froward!`) | same list |
| `#ordertxt` (class `clsDecimal`) | Order | required for add (alert `Please Select Order!`) | numeric keypress filter |
| `#addtolistbtn` | Add | – | validates uniqueness then appends a row |
| `#dtTble` rows | Role, Order, Remove button | – | hidden `roleid<i>`, cell `order<i>` |
| Save button | Save | – | `Save()` |

## 8. Field Mapping
| UI | JSON (`ApprovalMapMaster`) | DAL | SP param | Column (UNKNOWN) |
|---|---|---|---|---|
| `#menuSelect` | `MenuId` | `SaveApprovalMapMaster` | `@MenuId` | approval-map master |
| `#roleSelect` | `FromRoleId` | same | `@FromRoleId` | master |
| (none — never sent) | `MenuName` | same | `@MenuName` = DBNull | master |
| row `roleid<i>` | `ApprovalMapDetails[].ToRoleId` | `SaveApprovalMapDetail` | `@ToRoleId` | detail |
| row `order<i>` | `ApprovalMapDetails[].Order` | same | `@Order` | detail |
| (server) | `ApprovalMapMasterId` = pk returned by master save | same | `@ApprovalMapMasterId` | detail FK |

## 9. Dropdown Mapping
`GetUserRole` → `ApprovalMapDAL.GetRoleTypeDropDown` → `sp_GET_RoleType`; `GetMenu` → `GetMainMenuDropDown` → `sp_GET_MainMenuByType(@TypeId="")` (type is hard-coded empty string; the UI's Type selector has no effect).

## 10. Button Action Mapping
Add → client validation `validationForOrder(order, roleid)`: **order must be unused and role must not already be in the list** (alerts `Order Already Exist!!!`, `Role Already Exist!!!`). Save → `Validation()` (role ≠ 0) → `FinalSave()`.

## 11. Frontend Flow
`GetAppMapData` (on From-role change): POST `LoadApprovalMap {roleId, typeId: <menuId>}`; if rows exist they are listed; **if none, the From role itself is pre-inserted as order 1**. `FinalSave` builds `{MenuId, FromRoleId, ApprovalMapDetails:[{ToRoleId, Order}]}` by iterating rows with `#roleid<i>`/`#order<i>` for `i = 0..n-1` while row ids were created as `roleid<index of last row + 1>`; after removing a middle row (Remove button) the ids are no longer contiguous, so later rows read `undefined` (CONFIRMED by reading `AddData`/`DelU`/`FinalSave`). Success callback always shows `successalert('Operation successful!','Success','ApprovalStepMap.aspx')`; `error` handler empty.

## 12. Backend Flow (`ApprovalStepMap.aspx.cs:102-116`)
`SaveMenu(approvalMapMaster)`: `mainid = ApprovalMapDal.SaveApprovalMapMaster(m)` (`sp_Save_ApprovalMapMaster`, `SaveDataReturnPrimaryKey`); then for each detail set `ApprovalMapMasterId = mainid` and call `SaveApprovalMapDetail` (`sp_Save_ApprovalMapDetail`); returns `"True"`. No transaction, no result check, no delete of previous details in C# (whether the master SP upserts and clears old details is UNKNOWN — if it does not, re-saving duplicates the chain).

## 13. API Mapping
| WebMethod | Returns | Auth |
|---|---|---|
| `SaveMenu(ApprovalMapMaster)` | `"True"` | none |
| `GetUserRole()` | role types | none |
| `GetMenu()` | menus | none |
| `LoadApprovalMap(roleId,typeId)` | `sp_GET_ApprovalMapLoad(@FromRoleId=roleId,@MenuId=typeId)` rows `RoleType, RoleTypeId, Order` | none |

## 14. Service Mapping
_Sections 14–15 are documented together in this block._
No BLL. `ApprovalMapDAL`: `GetRoleTypeDropDown`, `LoadApprovalMap`, `GetMainMenuDropDown`, `SaveApprovalMapMaster`, `SaveApprovalMapDetail` (all: `DataBase.SalesDB`, close in `finally`, rethrow).



## 15. Repository Mapping
See §14 block above (combined §14–15).

## 16. Stored Procedure Mapping
| SP | Params | Body |
|---|---|---|
| `sp_GET_RoleType` | – | not in repo |
| `sp_GET_MainMenuByType` | `@TypeId` | not in repo |
| `sp_GET_ApprovalMapLoad` | `@FromRoleId,@MenuId` | not in repo |
| `sp_Save_ApprovalMapMaster` | `@MenuId,@FromRoleId,@MenuName` | not in repo |
| `sp_Save_ApprovalMapDetail` | `@ApprovalMapMasterId,@Order,@ToRoleId` | not in repo |

## 17. Database Mapping
UNKNOWN table names (approval-map master/detail). The other approval tables in the repo (`tblAction`, `tblActionSteps`, `tblActionStepDetail`, `tblActionPageWiseStep`, `tblActionUserWiseApproval`, read by `ClsApprovalAction` and `OrderListDAL`) are a **different, user-based mechanism** and are not written by this page (grep: no writer in C#).

## 18. Validation Matrix
| Field | Rule | FE | BE | Message | BR |
|---|---|---|---|---|---|
| From role | ≠ 0 | yes | no | tooltip `Please fill out of this field!` | BR-M02-024 |
| To role | selected | yes (alert) | no | `Please Select Role Froward!` | BR-M02-024 |
| Order | non-empty, unique, numeric-only keypress | yes | no | `Please Select Order!`, `Order Already Exist!!!` | BR-M02-024 |
| To role | unique in list | yes | no | `Role Already Exist!!!` | BR-M02-024 |
| Menu | none | – | – | – | – |

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-024 | A mapping is (menu, from-role type) → ordered list of to-role types; within one mapping each order value and each role may appear once (client-side only). | `ApprovalStepMap.aspx` `validationForOrder` | CONFIRMED |
| BR-M02-025 | When a (menu, from-role) has no saved chain the UI proposes the from-role itself as step 1. | `GetAppMapData` | CONFIRMED |
| BR-M02-026 | Save = insert master (`MenuId`, `FromRoleId`, `MenuName` null) then one detail row per step, without transaction; result is not checked. | `ApprovalStepMap.aspx.cs:102-116` | CONFIRMED |
| BR-M02-027 | The "roles" here are **role types** (`RoleTypeId`, e.g. those also held in `Session["RoleTypeId"]`), not user roles; the Type selector is fixed to App and ignored server-side. Downstream approval list pages compare a row's `ToRoleTypeId` to `Session["RoleTypeId"]` (e.g. `CustomerApproveList.aspx.cs:548`), so this mapping is what makes an item visible/actionable to a role type — consumption code is in other modules. | `ApprovalMapDAL.cs`; `CustomerApproveList.aspx.cs:532-548` | INFERRED |

## 20. Permission Matrix
No `Session` use; WebMethods unauthenticated and unauthorised (same as PAGE-027). Anyone can rewrite an approval chain.

## 21. Status Lifecycle
None on this page (configuration). Approval item status lifecycles are in other modules.

## 22. Error Handling
DAL rethrows; page has no try/catch; client error callback empty; success always announced.

## 23. Audit
None (no EntryBy/date).

## 24. Integration
_Sections 24–25 are documented together in this block._
None.



## 25. Reports
See §24 block above (combined §24–25).

## 26. Dependencies
`ApprovalMapDAL`, `SalesSolution.Web.Models.ResultInfo`, `_myCusGen_Func.js`, role-type master (`tblRoleType`, referenced by login SQL).

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Pick menu + from role, no chain exists | one row = from role, order 1 | 025 |
| TC-02 | Add role B order 2, role C order 2 | `Order Already Exist!!!` | 024 |
| TC-03 | Add role B twice | `Role Already Exist!!!` | 024 |
| TC-04 | Save chain of 3 | master + 3 details | 026 |
| TC-05 | Remove middle row then Save | later rows send `undefined` role/order (verify) | – |
| TC-06 | Save twice for same menu/role | duplicate chain or upsert — depends on SP (verify) | 026 |
| TC-07 | POST `SaveMenu` with no session | processed | – |

## 28. Reverse Engineering References
`Solution.Web/UserPermission/ApprovalStepMap.aspx(.cs)`, `Library.DAL/UserRoleDAL/ApprovalMapDAL.cs`, `Library.DAO/UserRoleDAO/ApprovalMapMaster.cs`.

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Consumption of the map | – | not found in C# (only SP-side, UNKNOWN) | GAP-M02-014 |

## 30. Known Gaps
GAP-M02-001, 004, 005, 013, 014 (consumer unknown, row-index bug, ignored Type selector), 015 (two parallel approval mechanisms).

## 31. Change Impact
Approval list pages in `Approval_UI` (10 pages call `UserPersmissionValidation` and use `ToRoleTypeId` from `Session["RoleTypeId"]`) depend on the chain data — verify with DB before changing. `ApprovalMapDAL` has no other C# caller.
