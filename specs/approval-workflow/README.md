# M18 — Approval Workflow (`Solution.Web/Approval_UI`)

Reviewed 2026-09-19. Documentation only — no source was modified. Confidence tags: CONFIRMED (read in code), INFERRED, UNKNOWN. SP bodies are absent from the repository except `sp_Get_CustomerApp` (list SP of the customer page); everything on the SQL side of the other pages is UNKNOWN (see `GAPS-M18.md`, GAP-M18-001).

## 1. Purpose & business overview
The module gives managers in the sales hierarchy a queue per subject type from which they approve/verify or reject items raised by field staff or created in the field: customers and doctors (master data), monthly tour plans and DCP/CVP visit plans, DCRs and prescriptions (RX), leave applications, expense / DA / mileage claims and field-force orders, plus a bulk market-transfer approval of customers/doctors. Approval is multi-level: the list SP returns each subject's latest approval-log row (`Step`, `ToRoleTypeId`, `ToEmpId`); only the user whose role equals `ToRoleTypeId` (or a gate-exempt role) sees the approve/reject buttons; each action calls a per-domain "save app-log" SP with `Step = current + 1`, a status word (`Verified`, `Accepted`, `Rejected`) and the acting employee. The chain itself is configured on `UserPermission/ApprovalStepMap.aspx` (M02, PAGE-026). Next-approver computation and header-status changes happen inside SPs that are not in the repository.

A second, unrelated mechanism (per-user action assignments in `tblAction*`, `ClsApprovalAction`) exists for manual-order approval in M06 and is documented in WF-M18-05; no M18 page uses it.

## 2. Page catalogue
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-695 | CustomerApproveList | Master-data approval (single+bulk) | A | [PAGE-695](pages/PAGE-695-customerapprovelist.md) |
| PAGE-696 | DAApprovalList | DA (TA/DA) claim approval | B | [PAGE-696](pages/PAGE-696-daapprovallist.md) |
| PAGE-697 | DCPCVPApproval | DCP/CVP visit-plan approval (single+bulk) | A | [PAGE-697](pages/PAGE-697-dcpcvpapproval.md) |
| PAGE-698 | DCRApprovalList | Doctor call report approval (single+bulk) | B | [PAGE-698](pages/PAGE-698-dcrapprovallist.md) |
| PAGE-699 | DoctorApprovalList | Master-data approval (single+bulk) | A | [PAGE-699](pages/PAGE-699-doctorapprovallist.md) |
| PAGE-700 | DoctorCustomerTransferApproval | Bulk transfer approval (Submit dead) | A | [PAGE-700](pages/PAGE-700-doctorcustomertransferapproval.md) |
| PAGE-701 | ExpenseApprovalList | Expense claim approval | A | [PAGE-701](pages/PAGE-701-expenseapprovallist.md) |
| PAGE-702 | LeaveApproveList | Leave approval (single+bulk) | A | [PAGE-702](pages/PAGE-702-leaveapprovelist.md) |
| PAGE-703 | MillageApprovalList | Mileage claim approval | B | [PAGE-703](pages/PAGE-703-millageapprovallist.md) |
| PAGE-704 | OrderApproveList | Field-force order approval | A | [PAGE-704](pages/PAGE-704-orderapprovelist.md) |
| PAGE-705 | RXApprovalList | Prescription approval | B | [PAGE-705](pages/PAGE-705-rxapprovallist.md) |
| PAGE-706 | TourPlanApproval | Monthly tour-plan approval | A | [PAGE-706](pages/PAGE-706-tourplanapproval.md) |
Cross-links: OrderApproveList ↔ M06 ManualOrderApproval `specs/order-management/pages/PAGE-220-manualorderapproval.md` (different engine and order source); ApprovalStepMap `specs/user-role-permission/pages/PAGE-026-approvalstepmap.md`; working transfer approval `specs/customer-master/pages/PAGE-202-customerdoctransferapproval.md`.
Tier-B pages (DA, DCR, Mileage, RX) were only traced from grep excerpts (statuses, Types, MenuIds, gate roles — see WF-M18-01/02); their specs remain generated (GAP-M18-029).

## 3. Business rules index
| BR | One-liner |
|---|---|
| BR-M18-001 | Next `Step` = row's current `Step` + 1 (all list pages) |
| BR-M18-002 | Approve/Reject buttons visible only when row `ToRoleTypeId` = user `RoleTypeId`, else "Waiting for Another Approver" (UI only) |
| BR-M18-003 | Gate-exempt RoleTypeIds differ per page (5,4,14 [+9] [+3]) |
| BR-M18-004 | Hard-coded employee 496 approves as "Accepted" and sees all rows (Leave, Expense, DA, Mileage) |
| BR-M18-005 | Hard-coded MenuId per page (302,303,356,371,372,376,377,379,381,382) |
| BR-M18-006 | Acting user's Group/Region/Area/Territory passed into the log (where the lookup is enabled) |
| BR-M18-007 | Scope by RoleTypeName AM→area, DZSM→region, NSM→group (DIC→DC on Order) via `@Role/@param` |
| BR-M18-008 | Filters are raw SQL text appended to dynamic SQL in `@param` |
| BR-M18-009 | Audit fields: Date/EntryDateS/ApproveDateS = now, EntryByS = EntryByApp = UserId, FromEmpId = EmpInfoId |
| BR-M18-010 | Bulk processes ticked rows whose action button is visible; per-row counts |
| BR-M18-011 | Menu-permission row required for page unless UserRoleID = 2 (Order: also unless UserType Admin) |
| BR-M18-012 | DIC role: DC dropdown preselected to DICCompanyUnitId and disabled |
| BR-M18-013 | Save "success" = no exception (`SaveData`) or scalar > 0 (`SaveDataReturnPrimaryKey`) or rows affected > 0 (customer) |
| BR-M18-014 | Status vocabulary: Verified / Accepted / Rejected |
| BR-M18-015 | Customer approve always writes "Accepted" |
| BR-M18-016 | Customers with ActionStatus 2 or 3 excluded from queue |
| BR-M18-017 | Customers sharing CellNo with another non-rejected customer flagged "Already Exists" |
| BR-M18-018 | Customer filter/search UI hidden server-side |
| BR-M18-019 | Customer UI reports success even if save failed |
| BR-M18-020 | ActionStatus map 0 Pending / 1 Verified / 2 Approved / 3 Rejected; waiting role default "Pending Approval" |
| BR-M18-021 | Doctor approve always "Verified" |
| BR-M18-022 | Doctor page: no scope, no filters, no hierarchy ids |
| BR-M18-023 | Doctor waiting message "Waiting for Another Employee" |
| BR-M18-024 | Order approve = "Accepted" iff RoleTypeId 5 else "Verified" |
| BR-M18-025 | Orders with ActionStatus 2/3 excluded |
| BR-M18-026 | AM/DZSM/NSM/DIC see orders ≤ 7 days old |
| BR-M18-027 | DIC scope = selected DC (`dcMas.DCId`) |
| BR-M18-028 | Order page redirects to Login when Session UserId missing |
| BR-M18-029 | Order and Leave both use MenuId 381 |
| BR-M18-030 | Leave approve "Accepted" iff RoleTypeId ∈ {5,9} or EmpInfoId 496 else "Verified" |
| BR-M18-031 | Leave reject writes Type "Order" |
| BR-M18-032 | Leave date filter default last 7 days..today on EntryDate |
| BR-M18-033 | Leave approve link disabled if status "Approved"; reject disabled if "Rejected" |
| BR-M18-034 | Role 3 gate-exempt on Leave only |
| BR-M18-035 | Leave/Order edit button never visible |
| BR-M18-036 | Leave log hierarchy ids always 0 |
| BR-M18-037 | Expense approve "Accepted" iff RoleTypeId 5 or EmpInfoId 496 |
| BR-M18-038 | Expense sends approval id 0 (SP must find log by TableId) |
| BR-M18-039 | Expense Type "ExpanseClaim" (approve) / "Expense" (reject) |
| BR-M18-040 | Expense edit opens ExpenseClaim with id and Rid=RoleTypeId |
| BR-M18-041 | Expense date filter default today..today |
| BR-M18-042 | Expense: AM/DZSM/NSM see claims ≤ 7 days old |
| BR-M18-043 | Expense page has no bulk approval |
| BR-M18-044 | Tour plan Type "Leave" for both actions |
| BR-M18-045 | Tour plan default filters: current year/month |
| BR-M18-046 | Tour plan page has no bulk approval |
| BR-M18-047 | DCP Type "DCP" (approve) / "Leave" (reject) |
| BR-M18-048 | DCP Reset goes to TourPlanApproval |
| BR-M18-049 | DCP default filters: current year/month |
| BR-M18-050 | Customer with pending order cannot be ticked for transfer approval |
| BR-M18-051 | Transfer Submit calls `sp_Update_Customer_Doctor_TransferApprove` per ticked row; last result decides message |
| BR-M18-052 | Transfer Submit is gated by `Validation()` on an unbound grid → never executes |
| BR-M18-053 | Transfer page toggles customer/doctor lists via transfer-approval list SPs |
| BR-M18-054 | Transfer page has no Session/permission check |
| BR-M18-055 | Transfer approver recorded as Session UserId |
| BR-M18-056 | `tblAction` action list = rows with `IsShow=1` |
| BR-M18-057 | tblAction engine uses only the first matching user/page row |
| BR-M18-058 | "Cancel" action stays enabled (unless equal to the condition) |
| BR-M18-059 | Action whose text equals the row's `ActionCondition` is disabled |
| BR-M18-060 | No assignment → all actions disabled |
| BR-M18-061 | Approval chain config = master (menu, from-role) + details (to-role, order); saved by separate SP calls, no transaction, no authorisation (`ApprovalStepMap`) |
(BR-M18-056..061 sources: `ClsApprovalAction.cs:25-63`, `ApprovalMapDAL.cs:99-156`, `ApprovalStepMap.aspx.cs:21-34`; CONFIRMED.)

## 4. Status lifecycle
- Log statuses written by pages: `Verified` (intermediate), `Accepted` (final; page-specific rule), `Rejected` — with `Step` incremented per action (WF-M18-01/02).
- Customer header `tblCustMaster.ActionStatus`: '0'/NULL Pending → '1' Verified → '2' Approved | '3' Rejected (list SP mapping, CONFIRMED). Doctor `ApprovalStatus` '2' = approved (repo SP inserts '2'). Others: UNKNOWN.
- Transfer request: pending → approved (SP-defined); no reject in live path.
- Numeric ↔ word mapping (`Accepted`→2?) is inside unavailable SPs (GAP-M18-026).

## 5. Roles & permissions observed
Session keys: `RoleTypeName` (AM, DZSM, NSM, DIC), `RoleTypeId` (5, 4, 14, 9, 3 special), `EmpInfoId` (496 special), `UserId`, `UserRoleID` (2 = full access), `UserType`, `DICCompanyUnitId`. Access: menu-permission row via `sp_GET_MainPermissionByUserRoleandPageUrl` (skipped for UserRoleID 2); approval eligibility by `ToRoleTypeId` match in UI only; no server-side re-check (GAP-M18-003). PAGE-700 has no checks.

## 6. Data model
| Item | Known |
|---|---|
| `tblCustomerApprovalLog` | CustomerApprovalId, Date, FromEmpId, ToEmpId, TableId, Status, Comments, Type, Step, GroupId, RegionId, AreaId, TerritoryId, RoleTypeId, ToRoleTypeId, EntryDateApp |
| `tblCustMaster.ActionStatus`, `tblDoctorMaster.ApprovalStatus` | 0/1/2/3 (customer); '2' approved |
| other log tables | UNKNOWN (one per domain by SP naming) |
| approval map | DAO `ApprovalMapMaster/Detail`; tables UNKNOWN |
| `tblAction`, `tblActionSteps`, `tblActionStepDetail`, `tblActionPageWiseStep`, `tblActionUserWiseApproval`, `tblMainMenu` | names only (WF-M18-05) |
| Key SPs | list: `sp_Get_*App*`; save: `sp_webapi_Save*AppLog`, `sp_web_Save*AppLog`, `sp_SaveLeaveAppLog`; config: `sp_Save_ApprovalMapMaster/Detail`, `sp_GET_ApprovalMapLoad`; transfer: `sp_Update_Customer_Doctor_TransferApprove` |
| Core code | `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs`, `DoctorDAL.cs`, `Library.DAL/InternalCls/ClsApprovalAction.cs`, `Library.DAL/UserRoleDAL/ApprovalMapDAL.cs`, `Library.DAL/DataManager/DataAccessManager.cs` |

## 7. Integrations
`sp_webapi_*`/`View_Webapi_*` naming suggests a shared mobile/Web API layer (INFERRED). `DoctorVisit_UI/DCRList.aspx.cs:118` re-executes the DCR approval write. No SAP integration in this module (the `sp_Up_ProviderDropoutIntrigrationApprove` script in `Library.DAL/SAP_IntegrationDAL/sql/` belongs to the SAP integration module, not M18).

## 8. Reports
None — see [reports/README.md](reports/README.md).

## 9. Workflows
- [WF-M18-01 generic approval-log engine](workflows/WF-M18-01-generic-approval-log-engine.md)
- [WF-M18-02 field-force plan/claim/leave/order approval](workflows/WF-M18-02-field-force-plan-claim-order-approval.md)
- [WF-M18-03 customer & doctor master approval](workflows/WF-M18-03-customer-doctor-master-approval.md)
- [WF-M18-04 customer/doctor transfer approval](workflows/WF-M18-04-customer-doctor-transfer-approval.md)
- [WF-M18-05 tblAction per-user action engine](workflows/WF-M18-05-tblaction-user-action-engine.md)

## 10. Known gaps (summary; full list in [GAPS-M18.md](GAPS-M18.md), 34 items)
Most serious: missing SP bodies (001/002); UI-only approver authorisation (003); customer approval reports success on failure (012); dead transfer Submit (014); hard-coded employee/role/menu ids and inconsistent exempt-role lists (009/010/025); SQL fragment concatenation and raw-SQL executor (007/018/030); web-created customers/doctors bypass approval (022).

## 11. What is still unverified
Bodies of all list/save SPs (except `sp_Get_CustomerApp`): next-approver logic, header-status updates, side effects (leave balance, expense payment, order status), log-table DDL; how "Accepted" maps to final status; menu/role seed data and the approval-map contents; the mobile API consumers; `ExecuteAllSqlQueryByStoreProcedure` and connection-string targets; Tier-B pages (DA, DCR, Mileage, RX) beyond grep-level facts; client-side helpers `ShowSuccesalert/faildalert/successalert` (defined outside these pages); runtime behaviour (no tests exist; the proposed test cases in each spec are unexecuted).
