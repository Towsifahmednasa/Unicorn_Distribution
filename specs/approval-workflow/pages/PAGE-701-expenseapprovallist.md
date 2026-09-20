---
page_id: PAGE-701
module: M18
page: ExpenseApprovalList
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-701 — ExpenseApprovalList

> **Evidence level:** Tier A — `.aspx`, `.aspx.cs` (514 LOC), `CustomerInfoDAL.GetExpenseClaim_Applog` (:578) and `SaveExpenseClaim_ApplogDAL` (:1201) read. SP bodies `sp_Get_ExpanseClaimApp`, `sp_web_SaveExpanseAppLog` NOT in repository → `UNKNOWN`.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID / module | PAGE-701 / M18 |
| Page name | ExpenseApprovalList |
| Kind | Approval list (single-row only; no bulk) |
| Folder / master | Approval_UI / NewMasterPage.master |
| Class | Approval_UI_ExpenseApprovalList |
| Related | `DoctorModule_UI/ExpenseClaim.aspx` (PAGE-570, claim entry/edit), `ExpenseClaimView` (PAGE-571) |

## 2. Purpose
Managers review field-staff daily expense claims (date, type, amount, comments, attached receipt image) and verify/accept or reject them. INFERRED from columns and DAL/SP names.

## 3. Business Objective
Control payout of expense claims through a multi-level chain; last-level (RoleTypeId 5) or the special employee 496 finalises as "Accepted". INFERRED (`.cs:191-202`).

## 4. Navigation
DB menu. Outbound: eye/edit button opens `../DoctorModule_UI/ExpenseClaim.aspx?id={ExpenseClaimID}&Rid={Session RoleTypeId}` in a new window (`.cs:183`; the role id lets the claim page know the approver's role — meaning in that page UNKNOWN). Hidden `DoctorEntry.aspx` (:310). Reset → same page (:512).

## 5. Route
`~/Approval_UI/ExpenseApprovalList.aspx`.

## 6. Source Files
View/code-behind; CustomerInfoDAL; CommonDataLoad (`GetExpenseType`, market lookup, permission, employees); SeedDataDAL; Setup2DAL; DAO `OrderSaveApprovalLogDAO`, `ResultInfo`.

## 7. UI Control Inventory
| Control | Label | Required | Default | Notes |
|---|---|---|---|---|
| FromDate / ToDate | date range | no | both = today, format `dd MMMM, yyyy` (:40-41) | filter on `mas.ExpenseDate` |
| EmployeeIdSelect | Employee | no | blank | `sp_CS_GetEmpGeneralInfo_All` |
| UserRoleSelect | User Role | no | blank | `sp_Get_UserRoleInfo` |
| ddlExpesneType | Expense Type | no | blank | `GetExpenseType` (ExpenseTypeId/ExpenseTypeName) |
| ApprovalStatusSelect | Approval Status | no | blank | `sp_GET_ActionStatusList` |
| ddlDistributionCenter | Distribution Center | no | blank / DIC preselect | hidden row (aspx:81) |
| btnSearch / resetBtn | Search / Reset | — | — | search button row is visible (not in hidden block) |
| loadGridView | (list) | — | — | keys `TableId,ExpanseApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep`; columns Employee ID (EmpMasterCode), Employee Name, Designation, User Role, Expense Date, Expense Type (TypeName), Expense Amount, Comments (Remarks), Approval Status, Waiting For, receipt thumbnail/link (`ImageString`), Actions |
| lbEdit/lbApprove/lbReject/lbMsg | — | — | — | gating below; all visible by default (no `Enabled` expressions unlike Leave) |
| hfExpenseClaimID | hidden | — | — | `Eval("ExpenseClaimID")` used by Edit |
Commented-out code (:414-443) once inlined receipt images from server file paths; dead.

## 8. Field Mapping
DAL `SaveExpenseClaim_ApplogDAL` :1201; SP `sp_web_SaveExpanseAppLog`; success = `SaveDataReturnPrimaryKey > 0`.
| UI | DTO | Value | SP param |
|---|---|---|---|
| (constant) | OrderApprovalId | **0 always** (:213,:263) — the row's `ExpanseApprovalId` (hfCustomerApprovalId) is read but not used | `@ExpanseApprovalId` |
| hfCustomerMasterId (TableId) | TableId | :215 | `@TableId` |
| hierarchy hidden fields | Group/Region/Area/Territory | from market lookup :324-327 | `@GroupId...` |
| Session EmpInfoId / hfToEmpId | FromEmpId / ToEmpId | :222-223 | `@FromEmpId,@ToEmpId` |
| hfStep + 1 | Step | :224-225 | `@Step` |
| computed | Status | :191-202 ("Accepted"/"Verified"), :252 ("Rejected") | `@Status` |
| literal | Type "ExpanseClaim" (approve :226) / "Expense" (reject :274) | — | `@Type` |
| literal | MenuId 356 | :233,:281 | `@MenuId` |
| now / UserId | Date, EntryDateS, ApproveDateS / EntryByS, EntryByApp | :228-232 | as named |
Table.column UNKNOWN.

## 9. Dropdown Mapping
See §7.

## 10. Button Action Mapping
| Button | Handler | DAL | SP | DB |
|---|---|---|---|---|
| Search | :504 → `LoadData` | `GetExpenseClaim_Applog` | `sp_Get_ExpanseClaimApp` | read |
| Approve | `RowCommand ApproveData` :188 | `SaveExpenseClaim_ApplogDAL` | `sp_web_SaveExpanseAppLog` | approval-log step (UNKNOWN) |
| Reject | `RejectData` :250 | same | same | same |
| Edit/View | `EditData` :176 | — | — | none |
| Reset | :510 | — | — | none |

## 11. Frontend Flow
Standard postback; DataTable/select2 init on `pageLoad`; no bulk JS.

## 12. Backend Flow
`Page_Load` :28-138 (all lookups silent-catch; any outer exception → Dashboard redirect). `LoadData` :313-495: market lookup (:321-329), role switch (:331-359): AM → `View_Webapi_EmployeeFieldForceInfo.EmpAreaId=<id>`; DZSM → `EmpRegionId`; NSM → `EmpGroupId`; each **plus** `and DATEDIFF(DAY, ExpenseDate, GETDATE()) <= 7` (claims older than 7 days are hidden from these roles); default no scope/no age limit. Then date range (:363-370), status (`mas.ApprovalStatus='v'`), role (`us.UserRoleID= v`), employee (`mas.EmpInfoId= v`), expense type (`mas.ExpenseTypeId= v`) — numeric ones unquoted. Note: if the AM/DZSM/NSM predicate is set and the user then supplies From/To dates, both apply (the 7-day limit cannot be widened by the user). DAL → grid → gating (:445-492): roles {5,4,14} see all buttons; `EmpInfoId=="496"` → only edit hidden; else standard waiting gate.
Save path as PAGE-702 (`SaveDataReturnPrimaryKey`, pk>0).

## 13. API Mapping
None.

## 14. Service Mapping
No BLL.

## 15. Repository Mapping
`GetExpenseClaim_Applog` (:578) → `sp_Get_ExpanseClaimApp`; `SaveExpenseClaim_ApplogDAL` (:1201) → `sp_web_SaveExpanseAppLog`; `CommonDataLoad.GetExpenseType`.

## 16. Stored Procedure Mapping
`sp_Get_ExpanseClaimApp` (read), `sp_web_SaveExpanseAppLog` (write; must return a positive scalar) — bodies absent (GAP-M18-001).

## 17. Database Mapping
Tables UNKNOWN. Filter text references `mas.ExpenseDate`, `mas.ApprovalStatus`, `mas.EmpInfoId`, `mas.ExpenseTypeId`, `us.UserRoleID`, view `View_Webapi_EmployeeFieldForceInfo`.

## 18. Validation Matrix
| Field | Rule | Message |
|---|---|---|
| Save fail | pk ≤ 0 → "Already Exist!" | "Already Exist!" |
| Save ok | "Operation successful!" | — |
| Dates | none | — |
| Amount | not editable here | — |

## 19. Business Rules
| BR | Description | Source | Conf. |
|---|---|---|---|
| BR-M18-001 | Step = Step + 1 | ExpenseApprovalList.aspx.cs:224-225 | CONFIRMED |
| BR-M18-002 | Standard gate | :467-492 | CONFIRMED |
| BR-M18-003 | Gate-exempt roles 5,4,14 | :445 | CONFIRMED |
| BR-M18-004 | EmpInfoId 496 special: "Accepted" on approve; edit hidden; approve/reject visible on all rows | :195,:449-466 | CONFIRMED |
| BR-M18-005 | MenuId 356 | :233,:281 | CONFIRMED |
| BR-M18-006 | Actor hierarchy passed | :321-329 | CONFIRMED |
| BR-M18-007 | AM/DZSM/NSM scope filter | :331-359 | CONFIRMED |
| BR-M18-009 | Audit fields | :228-232 | CONFIRMED |
| BR-M18-011 | Menu-permission check | :139-171 | CONFIRMED |
| BR-M18-013 | Success = scalar > 0 | CustomerInfoDAL.cs:1245-1254 | CONFIRMED |
| BR-M18-014 | Status vocabulary | :191-202,:252 | CONFIRMED |
| BR-M18-037 | Approve status "Accepted" iff RoleTypeId = 5 or EmpInfoId = 496, else "Verified" (role 9 is not final here, unlike Leave) | :191-202 | CONFIRMED |
| BR-M18-038 | `OrderApprovalId`/`@ExpanseApprovalId` always 0 — the SP must locate the log by TableId | :213,:263 | CONFIRMED (SP behaviour UNKNOWN) |
| BR-M18-039 | Type strings differ: approve "ExpanseClaim", reject "Expense" | :226,:274 | CONFIRMED |
| BR-M18-040 | Edit opens ExpenseClaim with `id=ExpenseClaimID` and `Rid=Session RoleTypeId` | :181-183 | CONFIRMED |
| BR-M18-041 | Default date range today..today | :40-41 | CONFIRMED |
| BR-M18-042 | AM/DZSM/NSM only see claims ≤ 7 days old | :335,:341,:346 | CONFIRMED |
| BR-M18-043 | No bulk approval on this page | aspx/cs | CONFIRMED |

## 20. Permission Matrix
Menu-permission row check unless UserRoleID==2; UI-only gating; special user 496; no server-side re-check.

## 21. Status Lifecycle
Written: "Verified" → intermediate; "Accepted" → final (role 5/496); "Rejected". Header effect UNKNOWN (payment integration none observed in repo). Step +1 per action.

## 22. Error Handling
Silent lookups; save unhandled; Page_Load → Dashboard redirect.

## 23. Audit
BR-M18-009; no comment text (`Comments` never set even though the grid has a "Comments" column showing the claimant's remarks).

## 24. Integration
None observed; claim entry in DoctorModule_UI.

## 25. Reports
None here.

## 26. Dependencies
CustomerInfoDAL, CommonDataLoad, SeedDataDAL, Setup2DAL, DAO `OrderSaveApprovalLogDAO`, `ResultInfo`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Role 5 approves | Status "Accepted", MenuId 356, Type "ExpanseClaim", `@ExpanseApprovalId`=0 | 037,038,039 |
| TC-02 | Role 9 approves | "Verified" | 037 |
| TC-03 | AM sees claim dated 10 days ago | not listed | 042 |
| TC-04 | Reject | Type "Expense", Status "Rejected" | 039 |
| TC-05 | Edit click | new window ExpenseClaim.aspx?id=..&Rid=.. | 040 |
| TC-06 | Employee 496 | approve "Accepted", edit hidden | 004 |
| TC-07 | Default load | From=To=today | 041 |
| TC-08 | Expense-type filter | unquoted `mas.ExpenseTypeId= n` | GAP-007 |
| TC-09 | SP returns 0 | "Already Exist!" | 013 |

## 28. Reverse Engineering References
`Solution.Web/Approval_UI/ExpenseApprovalList.aspx(.cs)`, `CustomerInfoDAL.cs:578,1201`, WF-M18-02.

## 29. Spec vs Actual
| Area | Spec | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | §12 | MISSING |
| Log id | None existed | always 0 | UNKNOWN |
| 7-day cap | None existed | hard-coded for AM/DZSM/NSM | RISK (business rule undocumented) |

## 30. Known Gaps
GAP-M18-001, 003, 004, 007, 008 (`Rows[0]`), 009, 010, 013, 026; unquoted numeric filters increase injection surface.

## 31. Change Impact
`SaveExpenseClaim_ApplogDAL`/`GetExpenseClaim_Applog` used only here. `ExpenseClaim.aspx` reads `Rid` (PAGE-570). MenuId 356 tied to approval map (INFERRED).
