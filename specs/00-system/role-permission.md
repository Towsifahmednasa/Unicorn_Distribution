# Roles & Permissions (system level)

**Roles are database data; the repository contains no role list.** Login yields `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType`; the DB function `dbo.MainMenu2(@UserId,@UserRoleID)` (body not in repo) renders the menu; role→menu rows are managed by `UserPermission`/`AdminMenuPanal` (`sp_Save_MenuRole`, `tblMenuDistribution`). Authentication: Forms mode configured but the app authenticates via Session only; `Web.config` has no `<authorization>` element (`docs/security.md`).

## Authorization model as implemented (M02)

This section states precisely what the code enforces (CONFIRMED by reading code/config unless tagged).

**Layers that exist**
1. **Authentication gate = session, not Forms.** `Web.config` declares Forms auth but no code issues an auth cookie and there is no `<authorization>`/`<location>`; access control is `Session["UserId"]` (M01 GAP-M01-002).
2. **Master-page guard.** `NewMasterPage`: first GET only (redirect to Login if `Session["UserId"] == null`); `MainMasterPage`: every request; `MasterPage.master`: none. Postbacks to NewMasterPage pages, static `[WebMethod]` page methods, `.ashx`, and 132 no-master `.aspx` are outside this guard.
3. **Menu hiding (the main "authorization").** The navigation HTML comes from `dbo.MainMenu2(UserId, UserRoleID)` (body UNKNOWN); pages are shown or hidden by menu row only. **The menu does not stop direct URL access.**
4. **Per-page server-side URL permission — 17 pages only.** `UserPersmissionValidation()` in: 10 `Approval_UI` list pages, `Dashboard_UI/AdminDashboard`, `DoctorModule_UI/{AppMonitoringList, AttendanceListApproval, UserRecords}`, `MasterSetup_UI/{CustomerView, DoctorView, EmployeeRecords}`. Logic: `Session["UserRoleID"] != "2"` → call `sp_GET_MainPermissionByUserRoleandPageUrl(@RoleId, @PageName)` (`CommonDataLoad.cs:17-38`, path normalised); **no row or any exception → redirect to `DashboardOne.aspx`**; role id **2 is a hard-coded bypass** (32 code sites test `!= "2"`). It runs only when `!IsPostBack`. `RAdd`/`REdit` are applied only in `UserRecords`, `CustomerView`, `DoctorView`; elsewhere the lines are commented out; `RView`/`RDelete` are never consumed in C#.
5. **Approver mapping.** `tblAppSetup` check (`DataTrue()`) live in 1 page only (PAGE-019 BR-M02-048). The user-based action mapping (`tblActionUserWiseApproval`/`tblActionPageWiseStep`/`tblActionSteps`/`tblActionStepDetail`/`tblAction`, `ClsApprovalAction`, `OrderListDAL.LoadApprovalControlDAL`) enables/disables approval radio buttons on `ManualOrderApproval` by `Session["LoginName"]` and page file name — UI control state only (the class builds SQL by concatenating login name/page name). No page in the repo writes those tables.
6. **Role-type gating in code.** `Session["RoleTypeId"]/RoleTypeName` are compared to literals in approval/dashboard pages (RoleTypeId 2/3/4/5/9/14; `RoleTypeName=="DIC"` in 20 places); `Session["UserType"]=="Admin"` in 19 places; approval-list rows compare `ToRoleTypeId` (from the approval chain data) with the session role type before showing action controls (e.g. `CustomerApproveList.aspx.cs:532-548`) — UI-level.
7. **Data scoping by session.** `ComUnitId`, `DICCompanyUnitId`, `DICUnitId`, `CentralWareHouse` (from `tblUserCompanyUnit`/`tblUser`) parameterise SPs/queries in other modules; whether each page applies them is outside M02 (note `CWHPermission`, `NationalReportPermission` have no C# reader).

**What is enforced server-side vs only hidden**
| Control | Server-side enforced? |
|---|---|
| Being logged in for NewMasterPage GET | Yes (first load only) |
| Being logged in for POST/WebMethod/no-master pages | No (unless a method reads `Session`) |
| Role may open a given page | **Only for the 17 pages above** (and role 2 always); all other pages: menu-hiding only |
| Add / Edit flag | 3 pages (UI visibility of New button/Edit column); otherwise not |
| View / Delete flag | Not consumed anywhere in C# |
| Who may administer users, roles, permissions, DC scope, approvers, approval chains | **No check at all** (PAGE-015/016/013/027/026/020/019/017): any logged-in user with the URL — and, for the AJAX endpoints of PAGE-027/026, any HTTP client without a session |
| Approver identity when approving | Mostly not (check commented out in 3 of 4 pages) |
| Password strength at admin creation | No (only self-change on ChangePassword, M01) |

**Weaknesses (documented, not fixed):** plaintext passwords visible in admin screens (GAP-M02-002); `SaveMenu` role-permission and approval-chain endpoints unauthenticated (GAP-M02-001); role 2 magic bypass; hard-coded role ids (GAP-M02-006/023); permission model has three overlapping mechanisms (role-menu, per-user legacy grants, approver tables) with unknown SP semantics (GAP-M02-015).

## M01 — Authentication, Session & Navigation: permissions observed

Session carries `UserRoleID`, `RoleTypeId`, `RoleTypeName`, `UserType` (`Admin` or other), `CentralWareHouse`, company-unit ids. In M01 code only these are tested: `UserType=="Admin"` (ComUnitId), `IsPasswordChange`, `UserId==1` (legacy menu). Role-based checks elsewhere are documented in M02 README §"Authorization model as implemented".

## M02 — User, Role & Permission Administration: permissions observed

Hard-coded ids: `UserRoleID==2` = super user (bypass); roles 29,14,25,18,20 and name "DA ASSISTANCE" = DC-scoped roles; role 14 in UserSetupforEMP; UserTypeId 1/3 = employee, 6 = DA (7 also appears in login SQL); `RoleTypeId` 2/3/4/5/9/14, `RoleTypeName` "DIC". Names of these roles are DB data (UNKNOWN).

## M03 — Organisation & Market Structure Master: permissions observed

Only Session keys `UserId` (most pages), `LoginName` (PAGE-075 EntryBy), `RoleTypeName/EmpInfoId/RoleTypeId` (structure controls), `RoleTypeId=="5"` and `DICUnitId` (PAGE-063 list scoping). Data scoping: `tblUserCompanyUnit` (depot pickers). No page checks that the caller may perform the action; PAGE-038 reads no session at all. Web-method endpoints on Thana/Router pages are anonymous-reachable for reads (INFERRED).

## M04 — Product, Pricing & Promotion Master: permissions observed

No page in M04 enforces a role or UserType in code. Access is DB-menu-based (`sp_GET_MainMenuRole2`). Session keys read: `UserId` (EntryBy), `LoginName` (price archive), `EmpInfoId` (CampaignView hides New Entry for "496"), `CampgainMasterMapId` (written by campaign pages). Static `[WebMethod]` endpoints (PromoMIOTag, GroupWisePromoQtyEntry, promo group, therapeutic/product line pages) and `HandlerDocCV.ashx` have no in-code authorisation.

## M05 — Customer & Provider Master: permissions observed

Almost no in-code enforcement: menu-based only. Exceptions: PAGE-173 (`UserRoleID`, `RAdd/REdit`), DIC/DZSM/AM DC-dropdown locks (cosmetic), `CustomerMasterReport` reads `Session["UserType"]`. Credit approval, imports, approvals: none.

## M06 — Order Management: permissions observed

No page in this module checks `Session["RoleTypeName"]`/`RoleTypeId` for authorisation. Enforcement = menu placement (DB-driven) + DC scoping via `tblUserCompanyUnit` on some lists (PAGE-217/231) + `tblActionUserWiseApproval` for approval options (PAGE-220). `Session["UserId"]` / `["LoginName"]` are recorded as creator/deleter/approver. Credit limit is not a permission but an advisory UI block.

## M07 — Proforma, Invoicing & Delivery: permissions observed

No page in M07 checks a role. Only `CreatePickingOnDC`, `InvoicePrintingList`, `DCPickingPrintingList` read `Session["UserType"]=="Admin"` (all DCs) and misuse `Session["ComUnitId"]` as a user id for others. DC scoping elsewhere relies on `tblUserCompanyUnit` via `OrderInfoBLL.LoadSC(ddl, UserId)` / `OtherStockActionBLL.DCLoad(ddl, UserId)`. `LoginName` is stamped into SP `@UpdateBy`/`@User`. Menu access is DB-driven (M01).

## M08 — Sales Return & Invoice Return: permissions observed

Menu access is DB-driven (`sp_GET_MainMenuRole2`). In code: PAGE-294/296 limit the DC dropdown to `tblUserCompanyUnit`; `SalesReturnList` (PAGE-295) locks the DC to `Session["DICCompanyUnitId"]` when `Session["RoleTypeName"] == "DIC"`; PAGE-287/290 load every DC; no page checks a role before a destructive or posting action. `Session["LoginName"]` is the audit user; `Session["UserId"]` is dereferenced without null checks (GAP-M08-021).

## M09 — Collections, Payments & Money Receipts: permissions observed

Auth is Forms + session; the master page redirects to Login when `UserId` is missing. Everything is otherwise **menu-based**; explicit code checks found:
- `Session["RoleTypeName"]/["RoleTypeId"]/["UserType"]/["DesigName"]` → default "Collection By" (PAGE-311).
- `RoleTypeName == "DIC"` → deposit list requires depot and preselects it; deposit code by DIC (`DICCompanyUnitId`).
- `RoleTypeName` "DZSM"/"AM" → UI steering on PAGE-336 (client-side only).
- Deposit deletion restricted to two hard-coded LoginNames (`51419000000`, `50588`) — PAGE-322.
- `RoleTypeId` "2"/"3" checks in DeliveryPaymentReport / SC_PaymentReport (behaviour not analysed — UNKNOWN).
- No permission check at all on PAGE-313, PAGE-335, PAGE-314, PAGE-334 (payment reversal, deletion, bulk upload).
- Depot lists come from `tblUserCompanyUnit` (PAGE-311 etc.), but the aging report lists all depots.

## M10 — Warehouse, DC Stock & Inventory: permissions observed

No role-based checks in any M10 page: forms auth + DB-driven menu only. Observed session use: `UserId` (condition permissions, DC list via `tblUserCompanyUnit`, valuation/batch pages redirect to login if absent), `LoginName` (EntryBy), `CentralWareHouse` (enables central WH freeze), `IDInformation`/`WHId` (navigation state), `ComUnitId` (invoice autocomplete). `sp_GET_btnShow(@PageName)` hides Save buttons (PAGE-385/389/392) - body absent. Approval pages have a commented-out entitlement check (`tblAppSetup`). Per-user stock conditions: `tblStockConditionPermission` (PAGE-382).

## M11 — Stock Transfer & Chalan Receive: permissions observed

Menu-driven. Code-level: `GetPermissionForShowBtn(path)` hides Save on PAGE-412/420 when the permission SP returns rows; `StockReceiveByDC` scopes DC list by `Session["UserType"]`/`UserId`; all other checks absent (no role checks before stock-changing actions; typed unit codes unrestricted). `Session["LoginName"]`/`["UserId"]` used for audit/SAP calls without null guards.

## M12 — Sub-Depot Operations: permissions observed

Menu-driven only (`tblMainMenu`/`tblMenuDistribution`, not in repo). Code-level: `Session UserType=='Admin'` (receive list, stock report), `UserId` DC restriction (freeze, payment, receive list), `LoginName=='21900'` special user (freeze release), `tblAppSetup` assignment gate (voucher approval, BR-M12-027). Full role matrix UNKNOWN.

## M13 — Sales Reports, Analytics & Dashboards: permissions observed

- Menu is DB-driven (`sp_GET_MainMenuRole2`); `NewMasterPage.master.cs:24-56` sends anonymous first-loads to `Login.aspx`.
- Session keys used: UserId, UserType ("Admin" sees all DCs, others only `ComUnitId`), ComUnitId, RoleTypeName (DZSM/NSM/AM/MIO), RoleTypeId, EmpInfoId, LoginName, UserRoleID.
- Enforced checks: AdminDashboard page permission (BR-M13-015); DC lists restricted by `UserType`; MIS pages narrow radios/selectors by role (UI only, BR-M13-029). **No role check** on either rollback page; viewers and SixTenProjectReportApps have no session check; the 28 dashboard WebMethods have none; Web.config has no `<authorization>` rules.

## M14 — Target & DWSP Management: permissions observed

| Page group | Session keys used | Enforcement |
|---|---|---|
| National / Zone entry | `UserId` | none beyond master-page login + DB menu |
| Area / Territory entry, views | `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (3 = DZSM, 2 = AM, 5 = higher level) | UI scoping only (locked drop-downs) |
| DWSP Apps pages (538, 545) | none | **no login check at all**; identity = `?EmpId` |
| Reports 548/549/550 | `RoleTypeName`, `EmpInfoId`, `RoleTypeId` | DZSM must pick zone; user-control scoping |
| Excel upload | `LoginName` | none |
| Target_UI/CommonDataLoad | none | none |

## M15 — HR, Field-Force & Employee Administration: permissions observed

Only two M15 pages enforce a code-level permission (`UserRoleID != "2"` -> `GetPermissionForUserRole(path)`, else redirect to `Dashboard_UI/DashboardOne.aspx`): PAGE-612 and PAGE-564. All other pages rely on the DB-driven menu (`sp_GET_MainMenuRole2`) and master-page login redirect. `RoleTypeId` 5 is the final approver in every approval page; `RoleTypeId` 4, 14, 9 also see action buttons; role types MIO/AM/DZSM/NSM scope list visibility to own/area/region/group. `Web.config` has no `<authorization>`; page-methods (`FieldForce.aspx/*`, `LeaveApplicationCode.aspx/*`, `YearlyLeaveProcess.aspx/*`) are callable without a session for reads.

## M16 — Doctor / Provider Master, Visits & Prescriptions: permissions observed

Session keys used: UserId, EmpInfoId, RoleTypeName (MIO, AM, DZSM, NSM, Admin), RoleTypeId, UserRoleID. Enforced in code: DoctorView page permission (UserRoleID != "2" needs permission row); role-scoped row visibility in DCRList and PrescriptionView; AM must pick Area / DZSM Zone in monthly pivots; hierarchy-control prefill by role. All other pages and WebMethods: menu-only gating, no checks.

## M17 — SAP, Mobile-App & External Integration: permissions observed

Session keys: `UserId`, `RoleTypeName`, `DICCompanyUnitId`, `EmpInfoId`, `RoleTypeId`, `ComUnitId`, `UserType`, `LoginName`, `UserRoleID`. Enforced role logic: DIC unit filter, Setup sub-territory scoping, AppMonitoring page permission, ASMX customer scoping. Everything else is menu-based; page methods and ASMX have no checks.

## M18 — Approval Workflow: permissions observed

Session keys: `RoleTypeName` (AM, DZSM, NSM, DIC), `RoleTypeId` (5, 4, 14, 9, 3 special), `EmpInfoId` (496 special), `UserId`, `UserRoleID` (2 = full access), `UserType`, `DICCompanyUnitId`. Access: menu-permission row via `sp_GET_MainPermissionByUserRoleandPageUrl` (skipped for UserRoleID 2); approval eligibility by `ToRoleTypeId` match in UI only; no server-side re-check (GAP-M18-003). PAGE-700 has no checks.

