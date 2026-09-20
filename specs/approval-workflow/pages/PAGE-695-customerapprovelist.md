---
page_id: PAGE-695
module: M18
page: CustomerApproveList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-695 — CustomerApproveList

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs` (580 LOC), `CustomerInfoDAL.GetCustomer_ApplogDAL` (:785), `SaveCustomer_ApplogDAL` (:818), `ClsCommonInternalDAL.DeleteAction`/`ExecuteNonQueryAction` read. **The list SP body IS available**: `sp_Get_CustomerApp` in `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:221-395`. The save SP `sp_webapi_SaveCustomerAppLog` body is NOT in the repository (`UNKNOWN`).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-695 / M18 |
| Page name | CustomerApproveList (heading "Customer List") |
| Kind | Approval list (master-data approval, single + bulk) |
| Folder / master | Approval_UI / NewMasterPage.master |
| Class | Approval_UI_CustomerApproveList |
| Related | `MasterSetup_UI/CustomerEntry.aspx` (PAGE-168), `TransferUI/CustomerProviderTypeApprove.aspx`, `MarketInfoApprove.aspx` (other approval-style pages, other modules) |

## 2. Purpose
Approve (accept) or reject newly created customers (pharmacies/chemists/retailers) awaiting approval, with a "Already Exists" duplicate-mobile-number warning. CONFIRMED from SP (`ExistenceStatus`, `ActionStatus`).

## 3. Business Objective
Prevent unapproved or duplicate customers from being used for orders: a customer only gets status 2 (Approved) after the approval step; duplicates by `CellNo` are highlighted red so the approver can reject them. INFERRED (SP `DupCell` CTE; row CSS `bg-color-red`, `.cs:388-391`).

## 4. Navigation
DB menu. Outbound (edit button): `../MasterSetup_UI/CustomerEntry.aspx?MID={CustomerMasterId}` new window (`.cs:117`). Hidden New Entry → `DoctorEntry.aspx` (:367). Reset → self (:574).

## 5. Route
`~/Approval_UI/CustomerApproveList.aspx`.

## 6. Source Files
View `CustomerApproveList.aspx`; code-behind; user control `../SInventory_UI/IVMarketStructureInvoSearch.ascx` (registered as `IVMarketStructure`, aspx:3) whose dropdowns are read in code; DAL `CustomerInfoDAL`; `ClsCommonInternalDAL` (internal, Enterprise Library, connection key `SolutionConnectionStringSSIDB`); `CommonDataLoad`, `SeedDataDAL`; DAO `CustomerSaveAppLog_DAO`, `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| Filter block 1 (DC `ddlDistributionCenter`, `ddlStatus` Active/Inactive, `DropDownList1` "DC", `ddlProgramType` Provider Type, `ddlChemisType` Customer Type, `ddlApprovalStatus`, `frmDate`, `toDate`, `custNameTextBox`+AutoComplete, `hfCustomerId`, market-structure user control) | filters | no | blank | **All three filter rows have `runat="server" visible="false"` (aspx:62,91,241) — the filters, Search and Reset buttons are never rendered.** `ddlProgramType`, `ddlChemisType`, `ddlApprovalStatus`, `DropDownList1` are never data-bound in code (empty). Only `ddlDistributionCenter` (bound :47-60) and `ddlStatus` (static ListItems) have items |
| btnBulkApprove / btnBulkReject | Approve Selected / Reject Selected | — | — | client `confirm(...)` |
| loadGridView | Customer List | — | — | keys `CustomerMasterId,CustomerApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep,ExistenceStatus`; visible columns Customer Code, Customer Name, Sales Person Name, Customer Type, Mobile NO, Address, Approval Status (ApprovalStatusWeb), Waiting For (WaitingForRole), Entry By (EmpMasterCode), Entry Date; hidden columns Zone, Area, Territory, Market Code/Name, Pharma Platform, Provider Type, Distribution RouteName |
| lbEdit/lbApprove/lbReject/lbMsg | row actions | — | — | gating below |
| hf* | hidden row/page fields | — | — | `hfCustomerApprovalId=CustomerApprovalId`, `hfCustomerMasterId=CustomerMasterId` |

## 8. Field Mapping
`CustomerSaveAppLog_DAO` → `SaveCustomer_ApplogDAL` (:818) → `sp_webapi_SaveCustomerAppLog` via `aCommonInternalDal.DeleteAction` (:863).
| UI | DTO | Value | SP param |
|---|---|---|---|
| hfCustomerApprovalId | CustomerApprovalId | :134 | `@CustomerApprovalId` |
| hfCustomerMasterId | TableId | :136 | `@TableId` |
| hidden hierarchy fields | Group/Region/Area/TerritoryId | **always 0**: they are never populated (market lookup commented out :415-456) | `@GroupId..@TerritoryId` |
| Session EmpInfoId / hfToEmpId | FromEmpId / ToEmpId | :143-144 | `@FromEmpId,@ToEmpId` |
| hfStep + 1 | Step | :145-146 | `@Step` |
| literal | Status "Accepted" (approve :124,:244) / "Rejected" (:184,:300) | — | `@Status` |
| literal | Type "Customer" | :147,:206,:272,:328 | `@Type` |
| literal | MenuId 302 | :154,:213,:279,:335 | `@MenuId` |
| now / UserId | Date, EntryDateS, ApproveDateS / EntryByS, EntryByApp | :149-153 | as named |
Table.column for the write: UNKNOWN. From the list SP (verified): approval log table is `tblCustomerApprovalLog` (columns CustomerApprovalId, Date, FromEmpId, ToEmpId, TableId, Status, Comments, Type, Step, GroupId, RegionId, AreaId, TerritoryId, RoleTypeId, ToRoleTypeId, EntryDateApp); master `tblCustMaster.ActionStatus`.

## 9. Dropdown Mapping
Only `ddlDistributionCenter` (`sp_GET_DistributionCenter`) is loaded; others empty/static (see §7).

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB |
|---|---|---|---|---|
| Approve (row) | `RowCommand ApproveData` :122 | `SaveCustomer_ApplogDAL` | `sp_webapi_SaveCustomerAppLog` | UNKNOWN (log step; presumably `tblCustMaster.ActionStatus`) |
| Reject (row) | :182 | same | same | same |
| Approve/Reject Selected | :242 / :298 | same | same | one SP call per ticked visible row |
| Edit | :111 | — | — | none |
| Search/Reset | not rendered | — | — | — |

## 11. Frontend Flow
`SelectAllRows` (aspx:367) toggles via DataTable API; `pageLoad` pickadate/select2; UpdatePanel; DataTables init at aspx:373+.

## 12. Backend Flow
`Page_Load` :28-73: resolves user-control dropdowns via `FindControl`; Session read; first load → permission check → DC dropdown → `LoadData`. Outer catch → Dashboard redirect.
`LoadData` :409-563: the role-scope switch (:415-456) is fully commented out, so `Role=""`; filter predicates (:460-518) are built from user-control/hidden controls (dead in practice because the controls are hidden and unbound: `pram` stays empty) e.g. `AND mas.ActionStatus='..'`, `AND CONVERT(date,mas.CreateDate) BETWEEN ..`. Calls `GetCustomer_ApplogDAL(pram,"",null,null,null,null)` → `sp_Get_CustomerApp` (verified body):
- base filter `ISNULL(mas.ActionStatus,'0') NOT IN ('2','3')` (approved/rejected customers never listed);
- joins latest log row per customer: `LogMax = MAX(Step) GROUP BY TableId`, join `tblCustomerApprovalLog` on `TableId & Step=MaxStep`;
- `ExistenceStatus` = 'Already Exists' when `tblCustMaster.CellNo` is shared by another customer with `ActionStatus <> '3'` (CTE `DupCell`), else 'New';
- `ApprovalStatusWeb`: 0/NULL Pending, 1 Verified, 2 Approved, 3 Rejected; `WaitingForRole` = role of `ToRoleTypeId`, default 'Pending Approval'; `Step` default 0; `ToRoleTypeId` default 0;
- `@param` appended verbatim into dynamic SQL executed by `sp_executesql`; `@Role` (if set) hides rows whose last actor role equals it.
Then bind, `loadGridView_RowDataBound` (:375) reads column `ExistenceStatus` **by header text** `GetColumnIndexByName(...,"ExistenceStatus")` (:397-406) — the grid has no column with that header text, so the index is −1 and the red-row highlight never applies (INFERRED from aspx bound column list; `ExistenceStatus` is only a DataKey). Gate loop (:532-562): roles {5,4,14,9} unrestricted; others standard waiting gate.
Save path: `SaveCustomer_ApplogDAL` → `DeleteAction` → `ExecuteNonQueryAction` (:605-652): Enterprise Library command on `SolutionConnectionStringSSIDB`, `ExecuteNonQuery() > 0` → true; **any exception is swallowed and returns false** (`DeleteAction` :73-85, catch sets false). The page treats both true and false as success: the `else` branch also shows "Operation successful!" (:173,:232) (the `faildalert` line is commented out :174,:234). Bulk paths increment `successCount` unconditionally without reading `Res` (:281-282,:337-338). CONFIRMED.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`GetCustomer_ApplogDAL` :785 (`accessManager.GetDataTable("sp_Get_CustomerApp")`), `SaveCustomer_ApplogDAL` :818 (uses internal DAL, not `DataAccessManager`), `GetPermissionForUserRole`, `GetDistributionCenterDataTableList`. Duplicate DAL copies of `SaveCustomer_ApplogDAL` exist in `OrderTrackingDAL.cs:593` (calls `SaveData("sp_webapi_SaveCustomerAppLog")` via DataAccessManager) and `MarketStructureTransferDAL.cs:898` (not called from any page in Solution.Web).

## 16. Stored Procedure Mapping
| SP | Body | Known |
|---|---|---|
| sp_Get_CustomerApp(@param,@Role,@AppStatus,@FromDt,@ToDt,@EmpId,@TerritoryId) | IN REPO (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:221`) | read (dynamic SQL; details in §12). Note DAL passes 6 params, `@TerritoryId` default NULL |
| sp_webapi_SaveCustomerAppLog | NOT in repo | write; UNKNOWN |
| sp_GET_DistributionCenter, sp_GET_MainPermissionByUserRoleandPageUrl | NOT in repo | UNKNOWN |

## 17. Database Mapping
READ (via SP, verified): `tblCustMaster`, `tblCustomerApprovalLog`, `tblRoleType`, `tblEmpGeneralInfo`, `tblUser`, `tblCustomerType`, `tblProgramType`, `tblDistributionRoute`, `tblMarket`, `tblSubTerritory`, `tblTerritory`, `tblSMCType`, `tblRouteInformationMarketDetail/Master`, view `View_Webapi_EmployeeFieldForceInfo`. WRITTEN: UNKNOWN (approval-log table `tblCustomerApprovalLog` presumed, INFERRED).

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Bulk | no ticked visible row → "No customers were approved/rejected. Please select records and ensure you have permission to approve/reject them." | verbatim |
| Single/bulk success | "Operation successful!" / "{n} customer(s) approved/rejected successfully!" | shown even when the save failed |
| Duplicate cell | display only (red row — see §12 caveat); does not block approval | — |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-001 | Step = Step + 1 | CustomerApproveList.aspx.cs:145-146 | CONFIRMED |
| BR-M18-002 | Standard waiting gate | :536-562 | CONFIRMED |
| BR-M18-003 | Exempt roles 5,4,14,9 | :532 | CONFIRMED |
| BR-M18-005 | MenuId 302 | :154 | CONFIRMED |
| BR-M18-007 | Role-scope filter switch is commented out (no AM/DZSM/NSM restriction) | :415-456 | CONFIRMED |
| BR-M18-008 | Filters are raw SQL text appended to `@param` inside dynamic SQL | .cs:460-518; LIVE_DB...sql:389 | CONFIRMED |
| BR-M18-009 | Audit fields | :149-153 | CONFIRMED |
| BR-M18-010 | Bulk processes ticked+visible rows; no per-row success test | :247-296 | CONFIRMED |
| BR-M18-011 | Menu-permission check | :75-105 | CONFIRMED |
| BR-M18-012 | DIC preselect | :56-60 | CONFIRMED |
| BR-M18-013 | Save result on this page = `ExecuteNonQuery()>0` with exception swallowed | ClsCommonInternalDAL.cs:73-85,:625 | CONFIRMED |
| BR-M18-014 | Vocabulary "Accepted"/"Rejected" | :124,:184 | CONFIRMED |
| BR-M18-015 | Approve always writes "Accepted" (no intermediate "Verified" on this page) | :124,:244 | CONFIRMED |
| BR-M18-016 | Customers with `ActionStatus` 2 (Approved) or 3 (Rejected) are excluded from the queue | LIVE_DB...sql:235 | CONFIRMED |
| BR-M18-017 | A customer whose `CellNo` is shared with another non-rejected customer is flagged "Already Exists" | LIVE_DB...sql:272-277,:283 | CONFIRMED (highlight INFERRED non-functional) |
| BR-M18-018 | Filter/Search UI is hidden server-side | aspx:62,91,241 | CONFIRMED |
| BR-M18-019 | UI reports success even when the save failed | .cs:173,:232,:282,:338 | CONFIRMED |
| BR-M18-020 | Numeric status map 0 Pending / 1 Verified / 2 Approved / 3 Rejected; waiting role defaults "Pending Approval" | LIVE_DB...sql:288-295,:281 | CONFIRMED |

## 20. Permission Matrix
Menu-permission check (non-role-2); UI gating; no server re-check; approver can approve any listed customer regardless of territory (scope switch disabled).

## 21. Status Lifecycle
`tblCustMaster.ActionStatus`: '0'/NULL Pending → '1' Verified → '2' Approved (customer code assigned as `'C'+MAX+1` over approved rows by `sp_Save_CustomerMaster` — see below) ; '3' Rejected. Page writes log status "Accepted"/"Rejected" — how "Accepted" maps to ActionStatus '2' is inside the unavailable save SP (UNKNOWN, GAP-M18-026). Important: in the available `sp_Save_CustomerMaster` (LIVE_DB...sql:403-540) a newly inserted customer gets `ActionStatus '2'` hard-coded and code `C<n>`; so customers created through that SP never enter this queue. The queue is therefore fed by another creation path (mobile app `IsFromApp` inferred from the doctor SP; UNKNOWN for customers).

## 22. Error Handling
Save exceptions swallowed → false → success message. Outer `Page_Load` catch → Dashboard.

## 23. Audit
BR-M18-009; approval log itself is the audit trail (`Comments` never set).

## 24. Integration
SP naming `sp_webapi_*`/`View_Webapi_*` indicates shared use with a Web API (mobile). Enterprise Library connection `SolutionConnectionStringSSIDB` used for the save (may target a different DB/connection string than `DataBase.SalesDB` — UNKNOWN; config not documented here).

## 25. Reports
None.

## 26. Dependencies
CustomerInfoDAL, ClsCommonInternalDAL, CommonDataLoad, SeedDataDAL, IVMarketStructureInvoSearch.ascx, DAO `CustomerSaveAppLog_DAO`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Load page | only customers with ActionStatus not in (2,3) | 016 |
| TC-02 | Two customers with same CellNo | both "Already Exists" (verify red row fails) | 017 |
| TC-03 | Approve as gate-exempt role | log Status "Accepted", Step+1, MenuId 302 | 001,005,015 |
| TC-04 | Force SP failure (e.g. bad connection) | UI still says "Operation successful!" | 013,019 |
| TC-05 | Bulk approve 3 rows | success count 3 irrespective of SP result | 010,019 |
| TC-06 | Non-exempt role, row waits on another role | buttons hidden | 002 |
| TC-07 | Search UI | not visible | 018 |
| TC-08 | Reject | Status "Rejected", Type "Customer" | 014 |

## 28. Reverse Engineering References
`CustomerApproveList.aspx(.cs)`, `CustomerInfoDAL.cs:785,818`, `ClsCommonInternalDAL.cs:73,605`, `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:221-395`, WF-M18-03.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | §12 | MISSING |
| Duplicate highlight | None existed | intended (`bg-color-red`) but index lookup by header text fails | DEFECT (INFERRED) |
| Result feedback | None existed | always success | DEFECT |

## 30. Known Gaps
GAP-M18-001/002 (save SP, log DDL), 003, 007, 012 (false success), 015, 017 (duplicate highlight non-functional), 020 (hidden filters), 022 (creation bypasses approval), 026, 027, 031 (different connection key).

## 31. Change Impact
Grep of `Solution.Web` shows `CustomerInfoDAL.GetCustomer_ApplogDAL`/`SaveCustomer_ApplogDAL` are called only by this page. Duplicate copies (`OrderTrackingDAL.cs:561,593`, `MarketStructureTransferDAL.cs:866,898`) have no caller in `Solution.Web` (dead duplicates, GAP-M18-027). `sp_webapi_SaveCustomerAppLog` may additionally be called by the mobile API (UNKNOWN).
