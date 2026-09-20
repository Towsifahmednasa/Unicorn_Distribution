---
page_id: PAGE-574
module: M15
page: FieldForce
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-574 — FieldForce

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, reachable DAL/DAO code and page-local JS. Stored-procedure bodies are NOT in the repository (none of this module's SPs are in the root `*.sql` scripts), so SP-internal behaviour is marked `UNKNOWN`. Sections 1, 4-6, 13-15 and 28 keep the generated (static-analysis) content, which was spot-checked; sections carrying `Verified` notes were checked line by line.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-574 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | FieldForce |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_FieldForce |
| aspx size | 367 bytes |
| Code-behind LOC | 279 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
CONFIRMED: `FieldForce.aspx` has **no UI** (markup is two empty `Content` placeholders, `Solution.Web/DoctorModule_UI/FieldForce.aspx:1-7`; `Page_Load` empty). It exists only as a **host of 35 static `[WebMethod]` endpoints** (`FieldForce.aspx.cs`) that the field-force hierarchy screens call with jQuery/AJAX (`FieldForce.aspx/<Method>`). Callers found by grep (18 pages in `Solution.Web/DoctorModule_UI`): `AMRecords, AMSetup, AMSetupEdit, AsmRecords, AsmSetup, DZSMRecords, DZSMSetup, MioRecords, MioSetup, MioUpdateInfo, NSMHeadRecords, NSMHeadSetup, NSMRecords, NSMSetup, RsmRecords, RsmSetup, UserRoleEntry, UserRoleRecords`. The endpoints create/update/list/inactivate the sales-force hierarchy positions: NSM (group head), NSM-Head, RSM/DZSM (region/zone), ASM/AM (area), MIO (territory), and user roles.

## 3. Business Objective
CONFIRMED by code: maintain "who holds which market node" - one employee per group (NSM), region (RSM/DZSM), area (ASM/AM) and territory (MIO) with active/inactive dates - and list *vacant* nodes (`sp_GET_Vacent*`) so a manager can fill them. Together with `UserMaster.EmpInfoId` this is the **employee <-> market hierarchy link** consumed by orders, tour plans, attendance and approvals.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/MioRecords.aspx`, `DoctorModule_UI/MioSetup.aspx`, `DoctorModule_UI/MioUpdateInfo.aspx`

Outbound references: none found

## 5. Route
`~/DoctorModule_UI/FieldForce.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/FieldForce.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/FieldForce.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | RSMSetupDal (Library.DAL/DoctorModule_DAL/RSMSetupDal.cs) |
| DAO/entity class | ASMInfo (Library.DAO/DoctorModule_DAO/ASMInfo.cs) |
| DAO/entity class | MIOInfo (Library.DAO/DoctorModule_DAO/MIOInfo.cs) |
| DAO/entity class | NSMInfo (Library.DAO/DoctorModule_DAO/NSMInfo.cs) |
| DAO/entity class | RSMInfo (Library.DAO/DoctorModule_DAO/RSMInfo.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | UserRoleDao (Library.DAO/DoctorModule_DAO/UserRoleDao.cs) |

## 7. UI Control Inventory
No server/HTML controls with an ID were extracted (viewer-only or code-generated page).

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
No page-local JS (page is empty). Consumers call `POST FieldForce.aspx/<Method>` with `contentType: application/json` and read `result.d`; list methods return a **JSON string** (`JsonConvert.SerializeObject(DataTable)`), edit-data methods return typed DTOs (`NSMInfo`, `RSMInfo`, `ASMInfo`, `MIOInfo`, `UserRoleDao`), save/inactivate methods return `ResultInfo` (`isSuccess`, `isValiCheck`, `ErrorMessage`, `Id`).

## 12. Backend Flow
Every method delegates to the single static `RSMSetupDal _setupDAL` (`FieldForce.aspx.cs:17`). Save/inactivate methods read the acting user as `Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString())` (throws `NullReferenceException` when no session). Read methods use no session at all. Save pattern in `RSMSetupDal` (identical for NSM/RSM/ASM/MIO, lines 21/346/1034/1106): id==0 -> `SaveDataReturnPrimaryKey("sp_Save_<X>Info")`; id>0 -> `sp_check_<X>Info(@<Node>Id,@<X>Id)` must return **0 rows** else `isValiCheck=true`, then `sp_UD_<X>Info`; `pk>0` -> `isSuccess=true` else `isValiCheck=true`. `InsertUpdate_ASMInfo`/`Insert_Update_MIOInfo` (lines 95/1180): on update with `IsActive=true` call `sp_UD_Insert_ASMInfo`/`sp_UD_Insert_MIOInfo` (INFERRED: closes the old assignment and inserts a new active row so history is kept), with `IsActive=false` call `sp_UD_ASMInfo`/`sp_UD_MIOInfo`. Inactivate methods call `sp_Update_<X>ActiveStatus(@<X>Id,@InactiveBy)` (lines 926/991/1280). Each DAL method opens its own connection and commits in `finally` (`SqlConnectionClose`), rolls back and rethrows on exception.

## 13. API Mapping
All are ASP.NET Page Methods: `POST /DoctorModule_UI/FieldForce.aspx/<Method>`.
| # | WebMethod (line) | Input | Returns | DAL method -> SP |
|---|---|---|---|---|
| 1 | LoadVacentGroup (23) | - | JSON | `Get_VacentGroup` -> `sp_GET_VacentGroup` |
| 2 | LoadVacentRegion (32) | groupId | JSON | `Get_VacentRegion` -> `sp_GET_VacentRegion` |
| 3 | LoadVacentArea (41) | zoneId | JSON | `Get_VacentArea` -> `sp_GET_VacentArea` |
| 4 | Save_RSMInfo (50) | RSMInfo | ResultInfo | `Save_RSMInfo` -> `sp_check_RSMInfo`, `sp_Save_RSMInfo`, `sp_UD_RSMInfo` |
| 5 | GetRSMList (56) | - | JSON | `Get_RSMInfo` -> `sp_GET_RSMInfo` |
| 6 | RsmInactiveById (66) | rsmId | ResultInfo | `Inactive_RSMInfoById` -> `sp_Update_RSMActiveStatus` |
| 7 | AsmRecords (72) | - | "" | none (stub) |
| 8 | Save_ASMInfo (77) | ASMInfo | ResultInfo | `Save_ASMInfo` -> `sp_check_ASMInfo`, `sp_Save_ASMInfo`, `sp_UD_ASMInfo` |
| 9 | InsertUpdate_ASMInfo (83) | ASMInfo | ResultInfo | `InsertUpdate_ASMInfoDAL` -> `sp_check_ASMInfo`, `sp_UD_Insert_ASMInfo` / `sp_UD_ASMInfo` |
| 10 | Save_UserRoleInfo (90) | UserRoleDao | ResultInfo | `Save_UserRoleInfo` -> `sp_check_UserRoleInfo`, `sp_Save_UserRoleInfo`, `sp_UD_UserRoleInfo` |
| 11 | Save_NSMInfo (96) | NSMInfo | ResultInfo | `Save_NSMInfo` -> `sp_check_NSMInfo`, `sp_Save_NSMInfo`, `sp_UD_NSMInfo` |
| 12 | Save_NSMHeadInfo (100) | NSMInfo | ResultInfo | **calls `Save_NSMInfo`, not `Save_NSMHeadInfo`** (DAL method `Save_NSMHeadInfo` at `RSMSetupDal.cs:422` is never reached from here) |
| 13 | GetUserRoleList (106) | - | JSON | `Get_UserRoleInfo` -> `sp_GET_UserRoleList` |
| 14 | GetUserInfoList (118) | - | JSON | `Get_UserListInfo` -> **inline SQL** `SELECT ... * FROM tblUser UR LEFT JOIN tbl_UserRoleInfo, tblUserType, tblEmpGeneralInfo emp (UR.EmpInfoId=emp.EmpInfoId), tblDAInfo (UserTypeId=6) WHERE UR.UserId not in (1)` (`RSMSetupDal.cs:272-296`) |
| 15 | GetNSMSetupEditData (131) | id | NSMInfo | `sp_GET_NSMInfo_ById` |
| 16 | GetNSMSetupEditDataByEmpId (140) | id | NSMInfo | `sp_GET_NSMInfo_ByEMPId` |
| 17 | GetUserRoleEditData (146) | id | UserRoleDao | `sp_GET_UserRoleInfo_ById` |
| 18 | GeDZSMSetupEditData (153) | id | RSMInfo | `sp_GET_DZSMInfo_ById` |
| 19 | GeDZSMSetupEditDataByEMPID (159) | id | RSMInfo | `sp_GET_DZSMInfo_ByEmpId` |
| 20 | GeAMSetupEditData (166) | id | ASMInfo | `sp_GET_ASMInfo_ById` |
| 21 | GeMIOtupEditData (175) | id | MIOInfo | `sp_GET_MIOInfo_ById` |
| 22 | GeMIOMasterDataByEmpID (182) | id | MIOInfo | `sp_GET_MIOInfo_ByEmpId` |
| 23 | GAMMasterDataByEmpID (189) | id | ASMInfo | `sp_GET_ASMInfo_ByEmpId` |
| 24 | GetNSMList (195) | - | JSON | `Get_NSMInfo` -> `sp_GET_NSMInfo` |
| 25 | GetNSMHeadList (204) | - | JSON | `GetNSMHeadList` -> `sp_GET_NSMHeadInfo` |
| 26 | GetASMList (215) | - | JSON | `Get_ASMInfo` -> `sp_GET_ASMInfo` |
| 27 | AsmInactiveById (226) | asmId | ResultInfo | `Inactive_ASMInfoById` -> `sp_Update_ASMActiveStatus` |
| 28 | AsmSetup (231) | - | "" | stub |
| 29 | MioRecords (237) | - | "" | stub |
| 30 | MioSetup (242) | - | "" | stub |
| 31 | Save_MIOInfo (247) | MIOInfo | ResultInfo | `Save_MIOInfo` -> `sp_check_MIOInfo`, `sp_Save_MIOInfo`, `sp_UD_MIOInfo` |
| 32 | Insert_Update_MIOInfo (252) | MIOInfo | ResultInfo | `Insert_Update_MIOInfoDAL` -> `sp_check_MIOInfo`, `sp_UD_Insert_MIOInfo` / `sp_UD_MIOInfo` |
| 33 | LoadVacentTerritory (258) | areaId | JSON | `Get_VacentTerritory` -> `sp_GET_VacentTerritory` |
| 34 | GetMIOList (265) | - | JSON | `Get_MIOInfo` -> `sp_GET_MIOInfo` |
| 35 | MioInactiveById (275) | mioId | ResultInfo | `Inactive_MIOInfoById` -> `sp_Update_MIOActiveStatus` |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RSMSetupDal.Get_VacentGroup | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:888 | sp_GET_VacentGroup |  |
| RSMSetupDal.Get_VacentRegion | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1324 | sp_GET_VacentRegion | @GroupId |
| RSMSetupDal.Get_VacentArea | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1344 | sp_GET_VacentArea | @RegionId |
| RSMSetupDal.Save_RSMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1035 | sp_Save_RSMInfo, sp_UD_RSMInfo, sp_check_RSMInfo | @CompanyId, @EmployeeId, @RSMId, @RegionId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Get_RSMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:970 | sp_GET_RSMInfo | @Parameter |
| RSMSetupDal.Inactive_RSMInfoById | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:992 | sp_Update_RSMActiveStatus | @InactiveBy, @RSMId |
| RSMSetupDal.Save_ASMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:22 | sp_Save_ASMInfo, sp_UD_ASMInfo, sp_check_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.InsertUpdate_ASMInfoDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:96 | sp_UD_ASMInfo, sp_UD_Insert_ASMInfo, sp_check_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Save_UserRoleInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:176 | sp_Save_UserRoleInfo, sp_UD_UserRoleInfo, sp_check_UserRoleInfo | @RoleName, @RoleTypeId, @UserRoleID, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Save_NSMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:347 | sp_Save_NSMInfo, sp_UD_NSMInfo, sp_check_NSMInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:251 | sp_GET_UserRoleList | @Parameter |
| RSMSetupDal.Get_UserListInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:273 | inline SQL: tblDAInfo, tblEmpGeneralInfo, tblUser, tblUserType |  |
| RSMSetupDal.GetNSMSetupEditDataDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:500 | sp_GET_NSMInfo_ById | @id |
| RSMSetupDal.GetNSMSetupEditDataByEmpIdDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:530 | sp_GET_NSMInfo_ByEMPId | @id |
| RSMSetupDal.GetUserRoleEditDataDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:557 | sp_GET_UserRoleInfo_ById | @id |
| RSMSetupDal.GeDZNSMSetupEditDataDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:589 | sp_GET_DZSMInfo_ById | @id |
| RSMSetupDal.GeDZSMSetupEditDataByEMPIDDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:619 | sp_GET_DZSMInfo_ByEmpId | @id |
| RSMSetupDal.GeAMSetupEditDataDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:645 | sp_GET_ASMInfo_ById | @id |
| RSMSetupDal.GeMIOetupEditDataDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:763 | sp_GET_MIOInfo_ById | @id |
| RSMSetupDal.GeMIOMasterDataByEmpIDDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:797 | sp_GET_MIOInfo_ByEmpId | @id |
| RSMSetupDal.GAMMasterDataByEmpIDDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:823 | sp_GET_ASMInfo_ByEmpId | @id |
| RSMSetupDal.Get_NSMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:849 | sp_GET_NSMInfo | @Parameter |
| RSMSetupDal.GetNSMHeadList | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:868 | sp_GET_NSMHeadInfo | @Parameter |
| RSMSetupDal.Get_ASMInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:907 | sp_GET_ASMInfo | @Parameter |
| RSMSetupDal.Inactive_ASMInfoById | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:927 | sp_Update_ASMActiveStatus | @ASMId, @InactiveBy |
| RSMSetupDal.Save_MIOInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1107 | sp_Save_MIOInfo, sp_UD_MIOInfo, sp_check_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Insert_Update_MIOInfoDAL | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1181 | sp_Save_MIOInfo, sp_UD_Insert_MIOInfo, sp_UD_MIOInfo, sp_check_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive |
| RSMSetupDal.Get_VacentTerritory | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1364 | sp_GET_VacentTerritory | @AreaId |
| RSMSetupDal.Get_MIOInfo | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1261 | sp_GET_MIOInfo | @Parameter |
| RSMSetupDal.Inactive_MIOInfoById | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:1281 | sp_Update_MIOActiveStatus | @InactiveBy, @MIOId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_ASMInfo | RSMSetupDal.Get_ASMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ASMInfo_ByEmpId | RSMSetupDal.GAMMasterDataByEmpIDDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ASMInfo_ById | RSMSetupDal.GeAMSetupEditDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DZSMInfo_ByEmpId | RSMSetupDal.GeDZSMSetupEditDataByEMPIDDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DZSMInfo_ById | RSMSetupDal.GeDZNSMSetupEditDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MIOInfo | RSMSetupDal.Get_MIOInfo | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MIOInfo_ByEmpId | RSMSetupDal.GeMIOMasterDataByEmpIDDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_MIOInfo_ById | RSMSetupDal.GeMIOetupEditDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_NSMHeadInfo | RSMSetupDal.GetNSMHeadList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_NSMInfo | RSMSetupDal.Get_NSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_GET_NSMInfo_ByEMPId | RSMSetupDal.GetNSMSetupEditDataByEmpIdDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_NSMInfo_ById | RSMSetupDal.GetNSMSetupEditDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_RSMInfo | RSMSetupDal.Get_RSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_GET_UserRoleInfo_ById | RSMSetupDal.GetUserRoleEditDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_UserRoleList | RSMSetupDal.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |
| sp_GET_VacentArea | RSMSetupDal.Get_VacentArea | body NOT in repository | read-like name (INFERRED) |
| sp_GET_VacentGroup | RSMSetupDal.Get_VacentGroup | body NOT in repository | read-like name (INFERRED) |
| sp_GET_VacentRegion | RSMSetupDal.Get_VacentRegion | body NOT in repository | read-like name (INFERRED) |
| sp_GET_VacentTerritory | RSMSetupDal.Get_VacentTerritory | body NOT in repository | read-like name (INFERRED) |
| sp_Save_ASMInfo | RSMSetupDal.Save_ASMInfo | body NOT in repository | write-like name |
| sp_Save_MIOInfo | RSMSetupDal.Insert_Update_MIOInfoDAL, RSMSetupDal.Save_MIOInfo | body NOT in repository | write-like name |
| sp_Save_NSMInfo | RSMSetupDal.Save_NSMInfo | body NOT in repository | write-like name |
| sp_Save_RSMInfo | RSMSetupDal.Save_RSMInfo | body NOT in repository | write-like name |
| sp_Save_UserRoleInfo | RSMSetupDal.Save_UserRoleInfo | body NOT in repository | write-like name |
| sp_UD_ASMInfo | RSMSetupDal.InsertUpdate_ASMInfoDAL, RSMSetupDal.Save_ASMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_Insert_ASMInfo | RSMSetupDal.InsertUpdate_ASMInfoDAL | body NOT in repository | write-like name |
| sp_UD_Insert_MIOInfo | RSMSetupDal.Insert_Update_MIOInfoDAL | body NOT in repository | write-like name |
| sp_UD_MIOInfo | RSMSetupDal.Insert_Update_MIOInfoDAL, RSMSetupDal.Save_MIOInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_NSMInfo | RSMSetupDal.Save_NSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_RSMInfo | RSMSetupDal.Save_RSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_UD_UserRoleInfo | RSMSetupDal.Save_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Update_ASMActiveStatus | RSMSetupDal.Inactive_ASMInfoById | body NOT in repository | write-like name |
| sp_Update_MIOActiveStatus | RSMSetupDal.Inactive_MIOInfoById | body NOT in repository | write-like name |
| sp_Update_RSMActiveStatus | RSMSetupDal.Inactive_RSMInfoById | body NOT in repository | write-like name |
| sp_check_ASMInfo | RSMSetupDal.InsertUpdate_ASMInfoDAL, RSMSetupDal.Save_ASMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_MIOInfo | RSMSetupDal.Insert_Update_MIOInfoDAL, RSMSetupDal.Save_MIOInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_NSMInfo | RSMSetupDal.Save_NSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_RSMInfo | RSMSetupDal.Save_RSMInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_UserRoleInfo | RSMSetupDal.Save_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Inline SQL (CONFIRMED): `Get_UserListInfo` reads `tblUser`, `tbl_UserRoleInfo`, `tblUserType`, `tblEmpGeneralInfo`, `tblDAInfo` (`Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:272-296`). This is the direct evidence that **`tblUser.EmpInfoId` = `tblEmpGeneralInfo.EmpInfoId`** for employee users (`UserTypeId=3`) and `tblUser.EmpInfoId = tblDAInfo.DAId` for DA users (`UserTypeId=6`).
- Everything else is SP-only (bodies absent): MIO/ASM/RSM/NSM/NSM-Head info tables, user-role table (`tbl_UserRoleInfo` INFERRED from the inline SQL join).
- DTO fields (`Library.DAO/DoctorModule_DAO/MIOInfo.cs`): `MIOId, CompanyId, TerritoryId, EmployeeId, IsActive, ActiveDate, InActiveDate, InActiveBy, EntryBy, EntryDate, UpdateBy, UpdateDate, Vacant, GroupId, RegionId, AreaId, ActiveDateStr`; SP parameters actually sent: `@MIOId @CompanyId @TerritoryId @EmployeeId @entryBy @isActive @acInAcDate` (same shape for ASM/RSM/NSM with `AreaId/RegionId/GroupId`).

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| id > 0 on update | node must not already be held by another active record: `sp_check_*Info` returns rows -> `isValiCheck=true` | `RSMSetupDal.cs:48-58,372-383,1058-1070,1131-1143` | none from server (caller decides; JS text in caller pages UNKNOWN here) | BR-M15-023 |
| insert | SP returns PK 0 on duplicate -> `isValiCheck=true` | `RSMSetupDal.cs:64,388,1075,1148` | same | BR-M15-023 |
| Session | `Session["UserId"]` must exist for writes | `FieldForce.aspx.cs:53,68,79,86,93,99,103,228,250,255,277` | NullReferenceException | BR-M15-024 |
| All input DTO fields | **no server-side validation at all** (dates, ids, isActive taken as posted; `ActiveDate` non-nullable `DateTime`) | - | - | GAP-M15-008 |

## 19. Business Rules
| BR ID | Description | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M15-023 | One node = one holder: saving/updating an NSM/RSM/ASM/MIO record fails validation (`isValiCheck`) when the target group/region/area/territory is already held (`sp_check_*Info`), or the insert SP returns no key. | `Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:21-70,346-392,1034-1080,1106-1160` | DAL | CONFIRMED (flow), UNKNOWN (SP predicate) |
| BR-M15-024 | Every write is stamped with the logged-in user (`Session["UserId"]` -> `@entryBy` / `@InactiveBy`); the endpoints do not otherwise check who the caller is. | `FieldForce.aspx.cs:53-277` | web-methods | CONFIRMED |
| BR-M15-025 | Reactivating an ASM/MIO through the `Insert_Update`/`InsertUpdate` variants uses `sp_UD_Insert_*` (new active row) while deactivating uses `sp_UD_*`; regular `Save_*` always uses `sp_UD_*`. | `RSMSetupDal.cs:120-135,1203-1218` | DAL | CONFIRMED (branch), INFERRED (meaning) |
| BR-M15-026 | Inactivation is a separate call (`*InactiveById`) recording the inactivating user; it is not part of the save call. | `RSMSetupDal.cs:926,991,1280` | DAL | CONFIRMED |
| BR-M15-027 | `Save_NSMHeadInfo` web-method persists through `Save_NSMInfo` (group NSM table) - NSM-Head assignments are saved as NSM assignments. | `FieldForce.aspx.cs:100-104` | web-method | CONFIRMED |
| BR-M15-028 | Vacant-node lists (`sp_GET_VacentGroup/Region/Area/Territory`) drive which nodes can be assigned in the setup screens; the "vacant" predicate is inside the SP. | `RSMSetupDal.cs:887-905,1323-1381` | DAL | INFERRED |

## 20. Permission Matrix
- **No permission or role check on any of the 35 methods.** Only writes touch `Session["UserId"]`; all 20 read/list/edit-data methods run without any session or role check.
- `Web.config` contains `<authentication mode="Forms">` but no `<authorization>` element (grep of `Solution.Web/Web.config`), so ASP.NET does not deny anonymous requests; the master-page redirect (`NewMasterPage.master.cs:26-60`) applies only to rendering the page, not to `FieldForce.aspx/<Method>` POSTs. Result (INFERRED from framework behaviour, not executed): `POST /DoctorModule_UI/FieldForce.aspx/GetUserInfoList` can be invoked without logging in and returns every `tblUser` row (`SELECT ... *`, which includes all columns of `tblUser` - password column presence INFERRED from `sp_GET_UserMaster_ByEmpId` returning `Password`, `EmployeeSetup.aspx.cs:205`).
- Menu-based access for the screens that call these endpoints: UNKNOWN (DB).

## 21. Status Lifecycle
| Entity | Field | Values | Transition code |
|---|---|---|---|
| MIO/ASM/RSM/NSM assignment | `IsActive`, `ActiveDate` (`@isActive`, `@acInAcDate`) | true/false | set by Save_* / Insert_Update_*; `*InactiveById` -> `sp_Update_*ActiveStatus(@InactiveBy)` (`RSMSetupDal.cs:926-968,991-1032,1280-1320`) |
| Vacant | `Vacant` string on DTO | UNKNOWN | read-only, produced by SP |
Lifecycle: vacant -> assigned (insert, active) -> inactive (inactivate call) -> re-assigned (Insert_Update variant when reactivating, INFERRED).

## 22. Error Handling
DAL methods: `try { ... } catch { SqlConnectionClose(true) (rollback); isSuccess=false; ErrorMessage = ex.Message; throw }` - the exception is rethrown so page-method callers receive HTTP 500 with the raw message (`customErrors` not verified). Business failures return `ResultInfo.isValiCheck=true` without text.

## 23. Audit
`@entryBy` (save) and `@InactiveBy` (inactivate) carry the numeric `Session["UserId"]`; `acInAcDate` carries the active/inactive effective date. Column names UNKNOWN. No write to any log table from C#.

## 24. Integration
None external. Consumed by 18 UI pages (see Purpose) and by `EmployeeSetup*.aspx.cs` through the same `RSMSetupDal` (PAGE-613/615).

## 25. Reports
None. List endpoints feed record-list grids of the caller pages.

## 26. Dependencies
BLL: —; DAL: RSMSetupDal; DAO: ASMInfo, MIOInfo, NSMInfo, RSMInfo, ResultInfo, UserRoleDao; Tables (inline): tblDAInfo, tblEmpGeneralInfo, tblUser, tblUserType, tbl_UserRoleInfo

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-574-01 | POST `Save_MIOInfo` with new employee/territory, session present | `isSuccess=true`, new `MIOId` row | BR-M15-023, 024 |
| TC-574-02 | POST `Save_MIOInfo` (id>0) with a territory held by another MIO | `isValiCheck=true`, no update | BR-M15-023 |
| TC-574-03 | POST any Save_* with no session cookie | HTTP 500 / NullReference | BR-M15-024 |
| TC-574-04 | POST `Save_NSMHeadInfo` then read `GetNSMList` and `GetNSMHeadList` | Record appears in NSM list (not head list) | BR-M15-027 |
| TC-574-05 | POST `MioInactiveById` | `sp_Update_MIOActiveStatus` run with `@InactiveBy=UserId` | BR-M15-026 |
| TC-574-06 | POST `GetUserInfoList` without login (security) | Expect denial; code allows it | GAP-M15-009 |
| TC-574-07 | POST `Insert_Update_MIOInfo` with `IsActive=true`, id>0 | branch `sp_UD_Insert_MIOInfo` | BR-M15-025 |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/FieldForce.aspx`, `Solution.Web/DoctorModule_UI/FieldForce.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | FieldForce.aspx.cs | n/a | MISSING | - |
| NSM-Head save | None existed | routed to NSM save | UNKNOWN | ACTUAL-DEFECT | BR-M15-027 |
| Endpoint security | None existed | no auth on reads; session-only on writes | n/a | ACTUAL-GAP | GAP-M15-009 |
| SP bodies | None existed | 35 methods -> ~45 SPs | absent | UNKNOWN | GAP-M15-001 |

## 30. Known Gaps
- **GAP-M15-009** Unauthenticated data endpoints, incl. `GetUserInfoList` (full `tblUser` dump; grep finds **no caller** of `FieldForce.aspx/GetUserInfoList` in `.aspx/.js/.ascx/.master`, i.e. dead but still reachable).
- **GAP-M15-008** No input validation; `ActiveDate` non-nullable.
- **BR-M15-027** NSM-Head save mis-routed.
- Four stub methods returning "" (`AsmRecords, AsmSetup, MioRecords, MioSetup`) - dead code.
- Static shared `RSMSetupDal` (GAP-M15-003). Duplicate save variants (`Save_*` vs `Insert_Update_*`) with different semantics.
- `Save_RSMInfo` etc. do not verify that `EmployeeId` exists/is active.

## 31. Change Impact
Any change to `RSMSetupDal` affects PAGE-613/615 (employee onboarding) and the 18 caller pages above. SPs `sp_check_*Info`, `sp_UD_*Info`, `sp_GET_Vacent*` shared with reports/approvals that resolve managers from the hierarchy (e.g. `View_Webapi_EmployeeFieldForceInfo` used by PAGE-562/563/564 - see those specs).

