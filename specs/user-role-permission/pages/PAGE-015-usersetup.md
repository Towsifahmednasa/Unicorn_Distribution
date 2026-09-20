---
page_id: PAGE-015
module: M02
page: UserSetup
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "PARTIAL", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-015 — UserSetup (create / edit application user)

> **Evidence level:** Tier A. `UserSetup.aspx(.cs)`, `UserInfoDAL` (all methods for this page), `SeedDataDAL` loaders read. The stored procedures `sp_Save_UserMaster_New`, `sp_Update_UserMaster_new`, `sp_check_UserInfo_Save`, `sp_check_da_UserInfo_Save`, `sp_check_da_UserInfo`, `sp_GET_UserMaster_ById`, `sp_GET_UserMaster_ByEmpId`, `sp_GET_UserDetailMarket_ById`, `sp_Save_UserMarketDetail`, `sp_Save_UserCompanyUnit`, `sp_Get_UserRoleInfo`, `sp_Get_UserTypeInfo`, `sp_GET_DistributionCenter`, `sp_Get_EmployeeList` are **not in the repository** → database PARTIAL. Frontend PARTIAL: the market-structure user control `IVMarketStructure.ascx` was not read (its section is invisible, see §7).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-015 |
| Module | M02 — User, Role & Permission Administration |
| Kind | Entry/edit form (master record with detail rows) |
| Folder | `Solution.Web/DoctorModule_UI` |
| Master | `~/MasterPages/NewMasterPage.master` |
| Class | `DoctorModule_UI_UserSetup` |
| Size | aspx 523 lines, code-behind 911 lines |

## 2. Purpose
Create or edit a login account (`tblUser`) and bind it to a person (employee or DA), a role, a dashboard preference, an active/inactive status, optional mobile-app access (IMEI), and — for certain roles — the distribution centres (DCs) the user may operate in.

## 3. Business Objective
Provision and maintain who can log in and what scope (role, DC set) they get; this is the only current place where user roles are attached to users. CONFIRMED for behaviour; business framing INFERRED.

## 4. Navigation
Inbound: `UserRecords.aspx` "New"/edit (`UserRecords.aspx:15`, `UserRecords.aspx.cs:126` → `UserSetup.aspx?MID=<UserId>`), `AppMonitoringList.aspx.cs:147` (`?MID=`), `MasterSetup_UI/EmployeeRecords.aspx:398` (`?EMPMID=<EmpInfoId>`). Outbound: success → `UserRecords.aspx` (or `../MasterSetup_UI/EmployeeRecords.aspx` when opened with `EMPMID`); role label links to `UserRoleEntry.aspx` (new tab); Reset → `UserSetup.aspx`. Menu path is DB data (UNKNOWN).

## 5. Route
`~/DoctorModule_UI/UserSetup.aspx[?MID=<UserId>|?EMPMID=<EmpInfoId>]`.

## 6. Source Files
`UserSetup.aspx`, `UserSetup.aspx.cs`, `Library.DAL/DoctorModule_DAL/UserInfoDAL.cs` (page calls DAL directly, no BLL), `Library.DAL/DoctorModule_DAL/SeedDataDAL.cs` (dropdown loaders), `Solution.Web/MasterSetup_UI/IVMarketStructure.ascx` (not read), `Library.DAO.MasterSetup_DAO.UserDAO`, `BonusCampaignMarketDetailDAO`.

## 7. UI Control Inventory
| Control | Label | Required (server) | Default / visibility | Data source / notes |
|---|---|---|---|---|
| `ddlUserType` (autopostback) | User Type | yes (`SelectedValue==""` → invalid) | "Please Select From List" | `sp_Get_UserTypeInfo` (`UserTypeId`/`UserType`); change shows `DivEmp` (types 1 and 3) or `DivDA` (type 6) |
| `ddlEmployeeName` | Employee Name | required when type 1 or 3 | `DivEmp` hidden until type 1/3 | `sp_Get_EmployeeList` (`EmpInfoId`/`EmployeeName`) |
| `ddlDaList` (autopostback) | DA Name | required when type 6 | `DivDA` hidden until type 6 | inline `select DAId, DACode+' : '+Name DaName from tblDAInfo where IsActive=1`; change → autofill login/password (BR-M02-005) |
| `txtUserName` | User Name | yes | text `N/A` | free text |
| `txtLoginName` | Login Name | yes | empty | free text |
| `txtPassword` | Password | yes | empty; **rendered as plain text box (`type="text"`)**; on edit the stored password is loaded into it | free text, no policy |
| `ddlUserRole` (autopostback) + refresh `loadUserRole` | User Role | yes | – | `sp_Get_UserRoleInfo` (`UserRoleID`/`RoleName`); label links to UserRoleEntry |
| `ddlProductLine` | Product Line | – | inside `visible=false` div → never rendered | unused |
| `chkMobileAccess` (autopostback) | Is Mobile Access | – | unchecked; ticking shows `divMei` | – |
| `txtImei1`, `txtImei2` | Mobile IMEI 1/2 | **not** required here (checks commented out `:406-423`) | hidden until mobile access | free text |
| `rbDashboard` | (radio) Main Dashboard / Depot Dashboard | – | Main (value 0) selected | – |
| Market block: `IVMarketStructure`, `btnAddtoListMarket`, `gv_Market` | Market Access | – | **wrapped in `<div runat="server" visible="false">`** (`UserSetup.aspx:306`) so it is never rendered even though `ddlUserRole_SelectedIndexChanged` sets `divmrkAccess.Visible=true` | grid rows built from hidden fields (GroupId…MarketId) |
| `dcDiv` / `ddlDistributionCenter` | DC Permission / Distribution Center | – | `dcDiv` visible only for DC-type roles (BR-M02-008); multi-select ListBox | `sp_GET_DistributionCenter` (`ComUnitId`/`ComUnitName`) |
| `chkIsActive` (autopostback) | Is Active | – | checked; label `pacinTxt` toggles "Active Date:"/"Inactive Date:" | – |
| `txtacDate` | Active/Inactive date | yes | empty, datepicker | parsed with `DateTime.Parse` |
| `btnSave` / `btnUpdate` | Submit / Update | – | `btnSave` visible for new; `btnUpdate` for edit (with SweetAlert confirm) — both call `btnSave_Click` | – |
| `btnReset`, `btnBtL` | Reset, Back to List | – | – | – |
| `hfEmpID`, `id_mastetID` (hidden) | – | – | `id_mastetID` = UserId being edited ("" = create) | – |

## 8. Field Mapping
| UI | DAO (`UserDAO`) | SP parameter | Table.column (INFERRED from names; SP body unknown) |
|---|---|---|---|
| `id_mastetID` | `UserId` (0 if empty) | `@UserId` | `tblUser.UserId` |
| `ddlUserType` text/value | `UserType`, `UserTypeId` | `@UserType`, `@UserTypeId` | `tblUser.UserType`, `UserTypeId` |
| `ddlDaList` (type 6) or `ddlEmployeeName` | `EmpInfoId` | `@EmpInfoId` | `tblUser.EmpInfoId` (holds the **DA id** for type 6 — confirmed by `GetDaUserSetupByDaId`: `WHERE EmpInfoId=@DAId AND UserTypeId=6`) |
| `txtUserName`, `txtLoginName`, `txtPassword` | `UserName`, `LoginName`, `Password` | `@UserName`, `@LoginName`, `@Password` | `tblUser.*` |
| `chkMobileAccess`, IMEIs | `IsAppsUser`, `IMEI_One`, `IMEI_Two` | same names | `tblUser.*` |
| `ddlUserRole` | `UserRoleID` | `@UserRoleID` | `tblUser.UserRoleID` |
| `txtacDate` | `ActiveInActiveDate` | `@ActiveInActiveDate` | `tblUser.ActiveInActiveDate` |
| `rbDashboard` | `IsMainDashboard` / `IsDepotDashboard` | same | `tblUser.IsMainDashboard/IsDepotDashboard` |
| `chkIsActive` | `UserStatus` = "Active"/"Inactive" | `@UserStatus` | `tblUser.UserStatus` |
| `Session["UserId"]` | – | `@EntryBy` | audit column UNKNOWN |
| selected DCs (CSV) | – | `sp_Save_UserCompanyUnit(@UserId,@ComUnitId)` per DC | `tblUserCompanyUnit` |
| `gv_Market` rows | `BonusCampaignMarketDetailDAO` | `sp_Save_UserMarketDetail(@UserId,@GroupId,@RegionId,@AreaId,@TerritoryId,@SubTerritoryId,@MarketId)` | user-market table UNKNOWN |

## 9. Dropdown Mapping
As in §7. Loaders in `LoadInitialInfo` (`UserSetup.aspx.cs:236-320`), each wrapped in `try{}catch{}` (failure = silently empty list).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Submit / Update | `btnSave_Click` (:435) | `Validation()` → build DAO/market list/DC CSV → `UserInfoDAL.SaveUserInfo` → popup + redirect |
| Reset | `btnReset_Click` | `Response.Redirect("UserSetup.aspx")` |
| Add To List (hidden) | `btnAddtoListMarket_Click` | requires `GroupSelect` |
| user type / DA / role / mobile / active | autopostback handlers | toggle panels, autofill, reset DC/market |

## 11. Frontend Flow
Server-rendered postbacks, select2/multiselect styling, SweetAlert (`successalert`, `faildalert`, `sweetAlertConfirm_Submit`) from master. No AJAX/PageMethods on this page.

## 12. Backend Flow
`Page_Load` (first load): resolve user-control dropdowns; `LoadInitialInfo()`; `Market_gv_Initial()`. If `MID` present → `btnUpdate` visible, `GetOneRecord(MID)`. Else if `EMPMID` → `GetUserSetupByEmpId` → `UserId` from row 0 → `GetOneRecord`; `btnBtL` retargeted to EmployeeRecords. Else `btnSave` visible.
`GetOneRecord(userId)`: `sp_GET_UserMaster_ById` → fill all fields (including the stored **password**), set panels by type (`ddlUserType_SelectedIndexChanged(null,null)`), role (`ddlUserRole_SelectedIndexChanged`), load market rows, pre-select DCs from CSV column `UserDCID`. Entire body in `try{}catch{}` — errors ignored.
`btnSave_Click`: after `Validation()` builds `UserDAO` (`EmpInfoId` = DA id when type 6 else employee id; `UserStatus` "Active"/"Inactive"; dashboard flags: Main radio → `IsMainDashboard=true` else `IsDepotDashboard=true`) and the CSV of selected DCs, then `SaveUserInfo(aMaster, MarketList, DisArray, Session["UserId"])`.
`UserInfoDAL.SaveUserInfo` (`UserInfoDAL.cs:330-492`), one DB connection, **no transaction**:
1. `UserId > 0` (edit): run `sp_check_da_UserInfo(@UserId,@LoginName,@EmpInfoId)` (same SP for type 6 and others); if it returns **0 rows** → `sp_Update_UserMaster_new`, `pk=UserId`; else `isValiCheck=true` (UI: "Already Exist!").
2. `UserId == 0` (create): `sp_check_da_UserInfo_Save` (type 6) or `sp_check_UserInfo_Save`; if 0 rows → `pk = sp_Save_UserMaster_New` (return value); `pk>0` → `isSuccess=true`, else `isValiCheck`.
3. If `pk>0 && isSuccess`: for each market row `sp_Save_UserMarketDetail`; then, if CSV non-empty, for each DC id `sp_Save_UserCompanyUnit(@UserId,@ComUnitId)`; `isSuccess` is overwritten by the **last** call, so an earlier detail failure can be masked. Whether the SPs first delete the user's previous market/DC rows on edit is UNKNOWN.
4. exceptions: connection closed with error flag, `isSuccess=false`, rethrown (`throw exception;`).
UI: success → alert "Operation successful!" then redirect; `isSuccess` false (incl. duplicate) → "Already Exist!".

## 13. API Mapping
None (no `[WebMethod]`).

## 14. Service Mapping
None — code-behind calls DAL directly (`UserInfoDAL`, `SeedDataDAL` static instances).

## 15. Repository Mapping
| DAL method | SP |
|---|---|
| `UserInfoDAL.GetUserSetupById` | `sp_GET_UserMaster_ById(@id)` |
| `GetUserSetupByEmpId` | `sp_GET_UserMaster_ByEmpId(@id)` |
| `GetDaUserSetupByDaId` | inline `SELECT TOP 1 UserId FROM dbo.tblUser WITH (NOLOCK) WHERE EmpInfoId=@DAId AND UserTypeId=6 ORDER BY UserId DESC` |
| `GetUserDetailMarketById` | `sp_GET_UserDetailMarket_ById(@id)` |
| `SaveUserInfo` | see §12 |
| `SeedDataDAL.GetEmployee_All/GetActiveDAList/GetUserRoleList/GetUserTypeList/GetDistributionCenterDataTableList` | `sp_Get_EmployeeList`, inline `tblDAInfo`, `sp_Get_UserRoleInfo`, `sp_Get_UserTypeInfo`, `sp_GET_DistributionCenter` |

## 16. Stored Procedure Mapping
All listed in §15/§12. **None has a body in the repository** (searched `*.sql`): effects UNKNOWN. Notable unknowns: whether `sp_Save_UserMaster_New` sets `IsPasswordChange=0`, how `daInfoId` is filled for type 6 (login query reads `U.daInfoId`), audit columns from `@EntryBy`, whether duplicates are keyed on LoginName, EmpInfoId or both.

## 17. Database Mapping
`tblUser` (create/update via SP), `tblUserCompanyUnit` (via `sp_Save_UserCompanyUnit`), user-market detail table (UNKNOWN name), reads: `tblDAInfo`, `tblUserType`/role info via SPs.

## 18. Validation Matrix (server-side only; no validators; failure shows red border/tooltip `please fill out this field`, no popup)
| Order | Field | Rule | BR |
|---|---|---|---|
| 1 | User Type | required | BR-M02-001 |
| 2 | Employee | required if type 1 or 3 | BR-M02-002 |
| 3 | DA | required if type 6 | BR-M02-002 |
| 4 | User Name | non-empty | BR-M02-003 |
| 5 | Login Name | non-empty | BR-M02-003 |
| 6 | Password | non-empty (no length/complexity) | BR-M02-003 |
| 7 | User Role | required | BR-M02-003 |
| 8 | Active/Inactive date | non-empty | BR-M02-003 |
| – | DB duplicate check | SP returns rows → "Already Exist!" | BR-M02-010 |
IMEI is *not* validated (commented out). Note that `Validation()` uses `ddlUserType.SelectedValue == "3" || "1"`; other type ids (e.g. 7 seen in `PanalClsDAL.Login`) get no person link and `EmpInfoId=null` (see §22).

## 19. Business Rules
| ID | Rule | Source | Confidence |
|---|---|---|---|
| BR-M02-001 | A user type must be chosen. | `UserSetup.aspx.cs:340-346` | CONFIRMED |
| BR-M02-002 | Types 1 and 3 must be linked to an employee; type 6 (DA) to a DA; other types have no person link (`EmpInfoId=null`). | `:349-371,478-491,583-597` | CONFIRMED |
| BR-M02-003 | User name, login name, password, role and active/inactive date are mandatory; the password has **no strength rule** and no confirmation field. | `:373-431` | CONFIRMED |
| BR-M02-004 | For DA users the DA id is stored in `tblUser.EmpInfoId`; the lookup `GetDaUserSetupByDaId` finds the newest user (highest UserId) of type 6 with that id. | `:484-487`; `UserInfoDAL.cs:246-250` | CONFIRMED |
| BR-M02-005 | Selecting a DA pre-fills LoginName **and Password** with the DA code (text before ":" in the DA list item) and, if a type-6 user already exists for that DA, switches the page to edit mode and **loads that user's saved data over the typed values**. So the default DA password equals the login name (= DA code). | `:599-651` | CONFIRMED |
| BR-M02-006 | Dashboard preference is exclusive: "Main Dashboard" → `IsMainDashboard=true`, else `IsDepotDashboard=true` (never both false). Login only reads `IsMainDashboard`. | `:508-519`; `Login.aspx.cs:71-96` | CONFIRMED |
| BR-M02-007 | Status flag: Is Active → `"Active"` else `"Inactive"`; label toggles Active/Inactive date; login accepts `'active'` (case per DB collation). | `:521-529,653-664` | CONFIRMED |
| BR-M02-008 | DC (distribution-centre) multi-select is shown **only** when the selected role id is one of **29, 14, 25, 18, 20** or its name equals "DA ASSISTANCE" (case-insensitive); role ids are hard-coded. Changing role clears DC selection and market rows. (Market-access block would show for other roles but is invisible — dead.) | `:859-890` | CONFIRMED |
| BR-M02-009 | Selected DCs are sent as a comma list and stored as user company units via `sp_Save_UserCompanyUnit` (these become `CompanyUnitIdList`/`DICUnitId` at login). | `:530-541`; `UserInfoDAL.cs:453-470` | CONFIRMED |
| BR-M02-010 | Create is refused when `sp_check_UserInfo_Save`/`sp_check_da_UserInfo_Save` returns rows; edit is refused when `sp_check_da_UserInfo` returns rows; both surface as "Already Exist!". What is considered a duplicate is UNKNOWN (SP bodies missing). | `UserInfoDAL.cs:361-429` | CONFIRMED (flow) / UNKNOWN (rule) |
| BR-M02-011 | Create vs update is decided only by hidden `id_mastetID` > 0 (edit link `MID`, `EMPMID`, or DA lookup). `@EntryBy` = `Session["UserId"]`. | `:480,542`; `UserInfoDAL.cs:360-361` | CONFIRMED |
| BR-M02-012 | Mobile access flag `IsAppsUser` with two IMEIs is stored, IMEI not mandatory on this page. | `:497-500` | CONFIRMED |
| BR-M02-013 | Password is displayed and edited in clear text and loaded back into the form from the DB on edit. | `:142`; `UserSetup.aspx:171` (`type="text"`) | CONFIRMED |

## 20. Permission Matrix
- Master-page login check (first GET) only. The code-behind reads `Session["UserId"]` only to stamp `@EntryBy`. **No role/user-type check and no `UserPersmissionValidation()` on this page** (that pattern exists on `UserRecords.aspx`, the list page, which hides the New/Edit buttons via `RAdd/REdit`). A user who is not shown the menu row can still open `UserSetup.aspx` and create users — including one with any role (menu-hiding only).
- Any user reaching the page can change any user's role, status and password (`MID` is a client-controlled id).

## 21. Status Lifecycle
`tblUser.UserStatus`: `Active` ⇄ `Inactive` by checkbox (no other states; login requires `active`). Dates: `ActiveInActiveDate` = the date typed (not set by code). `IsPasswordChange` not touched here (UNKNOWN inside SPs).

## 22. Error Handling
Dropdown loaders and `GetOneRecord` swallow exceptions (silent blanks). `SaveUserInfo` rethrows → unhandled error page. **Latent defect:** the duplicate-check calls on the edit path (`UserInfoDAL.cs:368`) and create path (`:403`) build `new SqlParameter("@EmpInfoId", master.EmpInfoId)` with a null `int?` for user types without a person link → the parameter is omitted by ADO.NET and the duplicate-check SP call is expected to fail with a missing-parameter error (INFERRED; SP unknown). `Session["UserId"].ToString()` throws if session lost.

## 23. Audit
`@EntryBy` (= session UserId) passed to save/update SPs; resulting columns UNKNOWN. No password-change history. No log of role changes.

## 24. Integration
None (mobile app consumes `IsAppsUser/IMEI` elsewhere — UNKNOWN).

## 25. Reports
None.

## 26. Dependencies
`UserInfoDAL`, `SeedDataDAL`, `DataAccessManager`, `IVMarketStructure.ascx`, `UserRoleEntry.aspx` (roles), `UserRecords.aspx`, `EmployeeRecords.aspx`, Global.asax route `user-records`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | New employee user (type 1) with all mandatory fields | user created, redirect to UserRecords | 001-003, 011 |
| TC-02 | Type 1 without employee | employee dropdown flagged, no save | 002 |
| TC-03 | Type 6, pick DA with no existing user | login & password = DA code; Save visible | 004, 005 |
| TC-04 | Type 6, DA already has a user | page switches to Update and loads saved login/password | 005 |
| TC-05 | Role id 29 chosen | DC list box shown; others hide it | 008 |
| TC-06 | Save with 3 DCs ticked | three `sp_Save_UserCompanyUnit` calls | 009 |
| TC-07 | Duplicate login (per SP) | "Already Exist!" | 010 |
| TC-08 | Password `1` accepted | saved (no policy) | 003 |
| TC-09 | Open `?MID=<id>` | password shown in clear text | 013 |
| TC-10 | Untick Is Active | UserStatus "Inactive"; login refused | 007 |
| TC-11 | Low-privilege user opens URL directly | page usable | – |
| TC-12 | Type without person link on edit | expected failure (null `@EmpInfoId`) — verify | – |

## 28. Reverse Engineering References
`Solution.Web/DoctorModule_UI/UserSetup.aspx(.cs)`, `Library.DAL/DoctorModule_DAL/UserInfoDAL.cs`, `Library.DAL/DoctorModule_DAL/SeedDataDAL.cs`, `Library.DAL/PanalCls/PanalClsDAL.cs` (consumer of the created row).

## 29. Spec vs Actual
| Area | Specification | Actual | Status |
|---|---|---|---|
| Prior spec | None existed | – | MISSING |
| Password policy | none | none at creation (policy only at self-change, M01 BR-M01-018) | GAP-M02-002 |
| Market access | UI suggests per-user market access | block hidden (dead) | GAP-M02-009 |
| Role-dependent DC scope | – | hard-coded role ids | GAP-M02-006 |

## 30. Known Gaps
GAP-M02-001 (no server-side authorization), 002 (plaintext admin-visible passwords, default password = login), 004 (no transaction, masked failures), 005 (SP bodies missing), 006 (hard-coded role ids), 007 (DA id stored in EmpInfoId), 009 (dead market block), 010 (swallowed exceptions).

## 31. Change Impact
Feeds login (`PanalClsDAL.Login`: `UserRoleID`, `IsMainDashboard`, `UserStatus`, company units), the master menu (role id), every page reading `Session["UserRoleID"/"RoleTypeId"/"ComUnitId"/"DICUnitId"]`. Shares `SaveUserInfo` with PAGE-016. `SeedDataDAL` loaders are shared by many pages. Changing hard-coded role ids in `IsDcPermissionRole` alters DC assignment UI only; assigned company units drive DC-scoped data everywhere.
