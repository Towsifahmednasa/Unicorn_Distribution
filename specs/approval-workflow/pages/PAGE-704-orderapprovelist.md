---
page_id: PAGE-704
module: M18
page: OrderApproveList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-704 — OrderApproveList

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs` (330 LOC), `CustomerInfoDAL.GetOrderList_ApplogDAL` (:447) and `SaveOrder_ApplogDAL` (:881) read. SP bodies `sp_Webapi_Get_OrderApp`, `sp_webapi_SaveOrderAppLog` NOT in repository → `UNKNOWN`. **Do not confuse with M06 `ManualOrderApproval`** (PAGE-220, `specs/order-management/pages/PAGE-220-manualorderapproval.md`), which approves manual orders through the separate `tblAction*` engine (WF-M18-05) and writes via `orderListBll.ApprovalUpdateDAL`; this page approves field-force (app) orders through the role-chain approval log.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-704 / M18 |
| Page name | OrderApproveList |
| Kind | Approval list (single-row only; no bulk) |
| Folder / master | Approval_UI / NewMasterPage.master |
| Class | Approval_UI_OrderApproveList |
| Cross-link | M06 PAGE-220 ManualOrderApproval (different mechanism, different order source) |

## 2. Purpose
Approvers in the sales hierarchy (AM/DZSM/NSM) and DC in-charge (DIC) verify or accept orders placed by field staff (order code, customer, market, net amount, creator, date) before they become regular orders. INFERRED from columns (`OrderCode`, `TotalNetPayable`, `EntryByWeb`) and `mas.ActionStatus not in (2,3)`.

## 3. Business Objective
Add a managerial check to field-taken orders: intermediate approvers "Verify", the RoleTypeId 5 approver "Accepts". Only recent (≤ 7 days) orders are shown to AM/DZSM/NSM/DIC. INFERRED (`.cs:111-118`, `:252-269`).

## 4. Navigation
DB menu. Edit action would open `../MasterSetup_UI/CustomerEntry.aspx?MID={TableId}` (`.cs:104` — a customer page for an order record: copy-paste) but `lbEdit` has `Visible="false"` in markup (aspx:130) and is hidden again in the gate loop, so never shown. Hidden New Entry → `DoctorEntry.aspx`.

## 5. Route
`~/Approval_UI/OrderApproveList.aspx`.

## 6. Source Files
View/code-behind; CustomerInfoDAL; CommonDataLoad; SeedDataDAL; DAO `OrderSaveApprovalLogDAO`, `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Notes |
|---|---|---|
| ddlDistributionCenter | Distribution Center | in a hidden row (aspx:59); bound on first load; DIC role: preselected to `Session["DICCompanyUnitId"]` and disabled (`.cs:55-59`); used in the DIC filter (BR-M18-027) |
| hf page hidden fields | — | filled from market lookup (:241-244) |
| loadGridView | (order list) | keys `TableId,OrderApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep`; columns Order Code, Customer Code, Customer Name, Market, Net Amount (TotalNetPayable), Approval Status (ApprovalStatusWeb), Create By (EntryByWeb), Create Date (EntryDate) (commented-out Doctor columns at aspx:114-116 are dead) |
| lbApprove / lbReject / lbMsg | row actions | standard gate |
No filters, no Search, no bulk buttons.

## 8. Field Mapping
`OrderSaveApprovalLogDAO` → `SaveOrder_ApplogDAL` (:881) → `SaveData("sp_webapi_SaveOrderAppLog")` (:925).
| UI | DTO | Value | SP param |
|---|---|---|---|
| hfCustomerApprovalId (OrderApprovalId) | OrderApprovalId | :128 | `@OrderApprovalId` |
| hfCustomerMasterId (TableId = order id) | TableId | :130 | `@TableId` |
| hidden hierarchy fields | Group/Region/Area/Territory | :132-135 | `@GroupId..` |
| Session EmpInfoId / hfToEmpId | FromEmpId / ToEmpId | :137-138 | `@FromEmpId,@ToEmpId` |
| hfStep + 1 | Step | :139-140 | `@Step` |
| computed | Status: "Accepted" if RoleTypeId = 5 else "Verified" (:111-118); "Rejected" (:168) | — | `@Status` |
| literal | Type "Order" | :141,:190 | `@Type` |
| literal | MenuId 381 (same value as LeaveApproveList) | :148,:197 | `@MenuId` |
| now / UserId | Date, EntryDateS, ApproveDateS / EntryByS, EntryByApp | :143-147 | as named |
Table.column UNKNOWN.

## 9. Dropdown Mapping
Only `ddlDistributionCenter` (`sp_GET_DistributionCenter`).

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB |
|---|---|---|---|---|
| Approve | `RowCommand ApproveData` :109 | `SaveOrder_ApplogDAL` | `sp_webapi_SaveOrderAppLog` | approval-log step (UNKNOWN) |
| Reject | :166 | same | same | same |

## 11. Frontend Flow
DataTable/select2 init (aspx:40-55); postbacks only.

## 12. Backend Flow
`Page_Load` :27-69: **explicit login check** — `Session["UserId"] == null` → `Response.Redirect("../Login.aspx")` (:29-33), the only Approval_UI page with it; Session keys read defensively (null → ""); any outer exception swallowed silently (catch empty :66-68).
`UserPersmissionValidation` (:71-94): only for `UserRoleID != "2"` **and** `UserType != "Admin"`; permission row missing → Dashboard redirect; exceptions swallowed (no redirect on error, unlike other pages).
`LoadData` (:230-322): market lookup (`GetEmpMarketStructure_Active`), role switch: AM → `View_Webapi_EmployeeFieldForceInfo.EmpAreaId=<id>`; DZSM → `EmpRegionId`; NSM → `EmpGroupId`; **DIC → `dcMas.DCId=<ddlDistributionCenter.SelectedValue>`** (Role "DIC"; the only page with a DIC branch); each with `DATEDIFF(DAY, mas.EntryDate, GETDATE()) <= 7`; default no scope. Then always append `and mas.ActionStatus not in (2,3)` (:278) (approved/rejected orders excluded; note that for the default branch this string starts with " and" which is fine). `dtMarket.Rows[0]` unguarded in the switch. DAL `GetOrderList_ApplogDAL` → `sp_Webapi_Get_OrderApp`. Gate loop (:290-319): roles {5,4,14} full; others standard waiting gate.
Save path uses `SaveData` (always true unless exception; rollback+rethrow; no page catch), so the failure text "Already Exist!" is unreachable.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`GetOrderList_ApplogDAL` :447 → `sp_Webapi_Get_OrderApp`; `SaveOrder_ApplogDAL` :881 → `sp_webapi_SaveOrderAppLog`; duplicates: `MarketStructureTransferDAL.cs:800,961` (no callers in Solution.Web).

## 16. Stored Procedure Mapping
`sp_Webapi_Get_OrderApp`, `sp_webapi_SaveOrderAppLog`, lookups — bodies absent (GAP-M18-001). Whether approving here sets `tblOrder.ActionStatus` (which `sp_Update_ManualOrderMaster` sets to '2' for M06 manual orders, `LIVE_DB...sql:1190`) is UNKNOWN.

## 17. Database Mapping
Tables UNKNOWN (SP). Filter text references `mas.EntryDate`, `mas.ActionStatus`, `dcMas.DCId`, view `View_Webapi_EmployeeFieldForceInfo`.

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Session | `UserId` null → Login | — |
| Save fail | `!isSuccess` → "Already Exist!" | unreachable in practice |
| Save ok | "Operation successful!" | — |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-001 | Step = Step + 1 | OrderApproveList.aspx.cs:139-140 | CONFIRMED |
| BR-M18-002 | Standard waiting gate | :294-319 | CONFIRMED |
| BR-M18-003 | Exempt roles 5,4,14 | :290 | CONFIRMED |
| BR-M18-005 | MenuId 381 | :148,:197 | CONFIRMED |
| BR-M18-006 | Actor hierarchy passed | :238-246 | CONFIRMED |
| BR-M18-007 | Scope by AM/DZSM/NSM/DIC | :248-276 | CONFIRMED |
| BR-M18-009 | Audit fields | :143-147 | CONFIRMED |
| BR-M18-011 | Menu-permission check only for non-role-2 non-Admin users | :71-94 | CONFIRMED |
| BR-M18-012 | DIC DC preselect/disabled | :55-59 | CONFIRMED |
| BR-M18-013 | Success = no exception | DataAccessManager.cs:447 | CONFIRMED |
| BR-M18-014 | Vocabulary Verified/Accepted/Rejected | :111-118,:168 | CONFIRMED |
| BR-M18-024 | Approve = "Accepted" iff RoleTypeId = 5 else "Verified" | :111-118 | CONFIRMED |
| BR-M18-025 | Orders with ActionStatus 2 or 3 excluded | :278 | CONFIRMED |
| BR-M18-026 | AM/DZSM/NSM/DIC see only orders ≤ 7 days old | :252,:258,:263,:269 | CONFIRMED |
| BR-M18-027 | DIC scope = selected/preselected DC (`dcMas.DCId`) | :267-271 | CONFIRMED |
| BR-M18-028 | Missing Session UserId → redirect Login | :29-33 | CONFIRMED |
| BR-M18-029 | MenuId 381 identical to LeaveApproveList (either a copy-paste error or a shared approval-map row) | :148 vs LeaveApproveList.aspx.cs:236 | CONFIRMED (cause UNKNOWN) |

## 20. Permission Matrix
Explicit Session check + menu-permission (non-role-2, non-Admin). UI gating; no server re-check.

## 21. Status Lifecycle
Log statuses Verified → Accepted / Rejected; `mas.ActionStatus` codes 2/3 (approved/rejected, INFERRED from the exclusion) are set by SP (UNKNOWN). Step +1 per action.

## 22. Error Handling
Outer catch empty (:66); permission-validation catch empty; save unhandled.

## 23. Audit
BR-M18-009.

## 24. Integration
Order data source: field-force app orders (INFERRED from `sp_Webapi_*` and `View_Webapi_*`); no SAP call here. M06 cross-link above.

## 25. Reports
None.

## 26. Dependencies
CustomerInfoDAL, CommonDataLoad, SeedDataDAL, DAO.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | No session | redirect Login | 028 |
| TC-02 | Role 5 approves | Status "Accepted", MenuId 381, Type "Order" | 024,005 |
| TC-03 | AM approves | "Verified" | 024 |
| TC-04 | AM sees 9-day-old order | not shown | 026 |
| TC-05 | DIC role | filter `dcMas.DCId=<DICCompanyUnitId>` | 027 |
| TC-06 | Order with ActionStatus 2 | not listed | 025 |
| TC-07 | Employee without market row (AM) | exception in `Rows[0]` (swallowed; blank page) | 007 |
| TC-08 | Reject | "Rejected", Type "Order" | 014 |

## 28. Reverse Engineering References
`OrderApproveList.aspx(.cs)`, `CustomerInfoDAL.cs:447,881`, cross-ref `specs/order-management/pages/PAGE-220-manualorderapproval.md`, WF-M18-02, WF-M18-05.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | §12 | MISSING |
| Menu id | None existed | 381 shared with Leave | RISK |

## 30. Known Gaps
GAP-M18-001, 003, 005, 007, 008, 010, 011 (edit link), 025 (MenuId clash), 026.

## 31. Change Impact
`SaveOrder_ApplogDAL`/`GetOrderList_ApplogDAL` used only by this page (grep verified). Orders approved here may be consumed by invoicing/M06 flows via status columns (UNKNOWN).
