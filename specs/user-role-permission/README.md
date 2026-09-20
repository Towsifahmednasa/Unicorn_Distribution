# M02 — User, Role & Permission Administration

Reviewed 2026-09-19 (reverse-engineered, documentation only; no prior specification existed). Tags: CONFIRMED / INFERRED / UNKNOWN. Companion module: [M01](../authentication-navigation/README.md) (login, menu rendering, passwords).

## 1. Purpose & business overview
M02 lets administrators (a) create/maintain login accounts and tie them to employees or DAs, roles, dashboard preference, mobile access and DC scope; (b) maintain roles and role→menu permissions (Permission/Add/View/Edit/Delete, Web/App); (c) assign users to distribution centres (`tblUserCompanyUnit`); (d) define approval chains by role type per menu and approver users per approval page; (e) delegate order placement per territory for a period. Most stored procedures behind these screens are **not in the repository**, so persistence details are UNKNOWN.

## 2. Page catalogue
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-012 | `DoctorModule_UI/UserRecords.aspx` | User list (only M02 page with `UserPersmissionValidation`: role 2 bypass, else `sp_GET_MainPermissionByUserRoleandPageUrl`; applies `RAdd`/`REdit`) | B | [pages/PAGE-012-userrecords.md](pages/PAGE-012-userrecords.md) |
| PAGE-013 | `DoctorModule_UI/UserRoleEntry.aspx` | Role create/edit | A | [pages/PAGE-013-userroleentry.md](pages/PAGE-013-userroleentry.md) |
| PAGE-014 | `DoctorModule_UI/UserRoleRecords.aspx` | Role list (AJAX `FieldForce.aspx/GetUserRoleList`; inactivate call points to `/FieldForce.aspx/RsmInactiveById`, copied from RSM list — unverified) | B | [pages/PAGE-014-userrolerecords.md](pages/PAGE-014-userrolerecords.md) |
| PAGE-015 | `DoctorModule_UI/UserSetup.aspx` | User create/edit | A | [pages/PAGE-015-usersetup.md](pages/PAGE-015-usersetup.md) |
| PAGE-016 | `MasterSetup_UI/UserSetupforEMP.aspx` | Older employee-user setup (no inbound links) | A | [pages/PAGE-016-usersetupforemp.md](pages/PAGE-016-usersetupforemp.md) |
| PAGE-017 | `Reports_UI/OrderPermission.aspx` | Order-place delegation per territory | A | [pages/PAGE-017-orderpermission.md](pages/PAGE-017-orderpermission.md) |
| PAGE-018 | `SInventory_UI/AccountSettings.aspx` | Legacy self/admin user edit (plain password field; `admin` login may pick any user) | B | [pages/PAGE-018-accountsettings.md](pages/PAGE-018-accountsettings.md) |
| PAGE-019 | `SInventory_UI/ApprovalSetupEntry.aspx` | Approver users per approval page (`tblAppSetup`) | A | [pages/PAGE-019-approvalsetupentry.md](pages/PAGE-019-approvalsetupentry.md) |
| PAGE-020 | `SInventory_UI/DCUserPermission.aspx` | User ↔ DC/warehouse (`tblUserCompanyUnit`) | A | [pages/PAGE-020-dcuserpermission.md](pages/PAGE-020-dcuserpermission.md) |
| PAGE-021 | `SInventory_UI/UserEdit.aspx` | Legacy user edit (no master page) | B | [pages/PAGE-021-useredit.md](pages/PAGE-021-useredit.md) |
| PAGE-022 | `SInventory_UI/UserEntry.aspx` | Legacy user create (masked password field; success message echoes password) | B | [pages/PAGE-022-userentry.md](pages/PAGE-022-userentry.md) |
| PAGE-023 | `SInventory_UI/UserView.aspx` | Legacy user list — grid **shows the Password column** | B | [pages/PAGE-023-userview.md](pages/PAGE-023-userview.md) |
| PAGE-024 | `SettingPanel_UI/ArchiveDbConnect.aspx` | Archive DB registration + starts SQL Agent job (`msdb.dbo.sp_start_job`, job name from appSettings) — not user admin | B | [pages/PAGE-024-archivedbconnect.md](pages/PAGE-024-archivedbconnect.md) |
| PAGE-025 | `SettingPanel_UI/UserSettingPanelSetup.aspx` | Per-row FromDate/ToDate setting-panel windows (`UserSettingPanelDal`) | B | [pages/PAGE-025-usersettingpanelsetup.md](pages/PAGE-025-usersettingpanelsetup.md) |
| PAGE-026 | `UserPermission/ApprovalStepMap.aspx` | Approval chain by role type per menu | A | [pages/PAGE-026-approvalstepmap.md](pages/PAGE-026-approvalstepmap.md) |
| PAGE-027 | `UserPermission/UserPermission.aspx` | Role→menu permission | A | [pages/PAGE-027-userpermission.md](pages/PAGE-027-userpermission.md) |

## 3. Authorization model as implemented
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

## 4. Business rules index
| ID | One-liner | Page |
|---|---|---|
| BR-M02-001…013 | UserSetup: user type required; type→person link (1/3 employee, 6 DA); mandatory fields; DA id in `EmpInfoId`; DA default login=password=code and auto-edit; exclusive dashboard flag; Active/Inactive; DC block only for roles 29/14/25/18/20/"DA ASSISTANCE"; DC CSV → `sp_Save_UserCompanyUnit`; duplicate check via SPs; create vs update by hidden id, `@EntryBy`=UserId; IMEI optional; plaintext password shown | PAGE-015 |
| BR-M02-014…016 | UserSetupforEMP: IMEI mandatory, DC only role 14, no dashboard flags, `?EmpID=` copies EmpInfoId into user id | PAGE-016 |
| BR-M02-017…023 | OrderPermission: zone/area filter; candidate list; ≥1 row; validation non-blocking; EntryBy=LoginName; last-row result; no consumer in repo | PAGE-017 |
| BR-M02-024…027 | ApprovalStepMap: unique order/role per chain; default step 1; master+detail save without txn; role *types* | PAGE-026 |
| BR-M02-028…035 | Role permission: Permission flag stored only for ticked rows; auto-tick; replace-set save; capability flags; App shows Permission only; role required; endpoints unauthenticated; Add/Edit flags barely consumed | PAGE-027 |
| BR-M02-036…041 | DCUserPermission: user+DC required; replace set; warehouse id 15 → CWH flag; exactly 11 DCs → National flag; unfiltered lists; two writers | PAGE-020 |
| BR-M02-042…045 | UserRoleEntry: mandatory fields; edit blocked by check SP; no create duplicate check; entryBy | PAGE-013 |
| BR-M02-046…049 | ApprovalSetupEntry: approvers per menu; selectable filters; consumer check mostly disabled; concatenated SQL | PAGE-019 |

## 5. Status lifecycles
- **User:** `UserStatus` Active ⇄ Inactive (PAGE-015); login requires `'active'`. `IsPasswordChange` 0/NULL → 1 (M01). No delete UI except legacy `UserBLL.DeleteUserInfo` (physical `DELETE FROM tblUser`, legacy pages).
- **Role:** `IsActive` true/false with date; block rule UNKNOWN.
- **Role permission / approval chain / approver list / DC set:** replace-all sets, no status.
- **Order delegation:** time window From–To.

## 6. Roles / permissions observed
Hard-coded ids: `UserRoleID==2` = super user (bypass); roles 29,14,25,18,20 and name "DA ASSISTANCE" = DC-scoped roles; role 14 in UserSetupforEMP; UserTypeId 1/3 = employee, 6 = DA (7 also appears in login SQL); `RoleTypeId` 2/3/4/5/9/14, `RoleTypeName` "DIC". Names of these roles are DB data (UNKNOWN).

## 7. Data model
| Table / object | Role |
|---|---|
| `tblUser` | accounts (LoginName, Password[plain], UserStatus, UserTypeId, UserRoleID, EmpInfoId [holds DA id for type 6], IsAppsUser, IMEI_One/Two, IsMainDashboard, IsDepotDashboard, ActiveInActiveDate, IsPasswordChange) |
| `tbl_UserRoleInfo`, `tblRoleType`, `tblUserType` | roles, role types, user types |
| `tblUserCompanyUnit` | user↔DC (+CWHPermission, NationalReportPermission) |
| `tblAppSetup` | approver users per approval menu |
| `tblMainMenu`, `tblMenuDistribution` | legacy menu / per-user grants (M01) |
| role-menu table (name UNKNOWN) | filled by `sp_Save_MenuRole` |
| approval-map master/detail (names UNKNOWN) | `sp_Save_ApprovalMapMaster/Detail` |
| `tblActionUserWiseApproval`, `tblActionPageWiseStep`, `tblActionSteps`, `tblActionStepDetail`, `tblAction` | user-based approval actions (read only in C#) |
| order-permission table (UNKNOWN) | `sp_Save_OrderPermission` |
SPs on this module's pages: see each page §16; none has a body in the repo.

## 8. Integrations
None (ArchiveDbConnect starts a SQL Agent job; not user-related).

## 9. Reports
None: see [reports/README.md](reports/README.md).

## 10. Workflows
- [WF-M02-01 User provisioning](workflows/WF-M02-01-user-provisioning.md)
- [WF-M02-02 Role creation](workflows/WF-M02-02-role-creation.md)
- [WF-M02-03 Role menu permission](workflows/WF-M02-03-role-menu-permission.md)
- [WF-M02-04 DC access assignment](workflows/WF-M02-04-dc-access-assignment.md)
- [WF-M02-05 Approval chain mapping](workflows/WF-M02-05-approval-chain-mapping.md)
- [WF-M02-06 Approver assignment (legacy)](workflows/WF-M02-06-approver-assignment-legacy.md)
- [WF-M02-07 Order placement delegation](workflows/WF-M02-07-order-placement-delegation.md)

## 11. Known-gaps summary
See [GAPS-M02.md](GAPS-M02.md) (31 items). Headlines: no server-side authorization on any admin function (001); plaintext passwords visible and default password = login for DAs (002); role/menu flags mostly unenforced and only 17 pages check URLs (003, 027); non-atomic replace-all saves (004); missing SP bodies (005); hard-coded role/DC ids (006); wrong-id defect in UserSetupforEMP (011); disabled approver checks (025).

## 12. What is still unverified
SP bodies (all); `dbo.MainMenu2` and how role-menu rows feed it; `sp_GET_MainPermissionByUserRoleandPageUrl` semantics; whether `sp_Save_UserMaster_New` resets `IsPasswordChange`; user-market and order-permission tables; consumption of `tblAction*`/approval-map data; runtime confirmation of unauthenticated page-method calls; Tier-B pages PAGE-012, 014, 018, 021-025 only skimmed; `IVMarketStructure.ascx`.
