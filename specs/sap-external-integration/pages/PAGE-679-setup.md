---
page_id: PAGE-679
module: M17
page: Setup
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-679 — Setup

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-679 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | Setup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/MasterPage.master |
| Page class | DoctorModule_UI_Setup |
| aspx size | 354 bytes |
| Code-behind LOC | 1650 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Empty master-page shell whose code-behind (1,649 lines) hosts **43 active `[WebMethod]` page methods** used as the JSON back-end of the doctor-module / market-structure set-up screens (Zone, Area, Territory, Sub-Territory, Market edit data, prescription, TA/DA, allowances, training, tour-plan and prescription approvals). Roughly three quarters of the file is commented-out legacy methods. CONFIRMED: `Solution.Web/DoctorModule_UI/Setup.aspx.cs`.

## 3. Business Objective
Serve create/edit/delete/list operations for setup masters and approval actions to client-side pages through AJAX (`Setup.aspx/<Method>`), so those pages need no postbacks.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/DoctorModule_UI/Setup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/Setup.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/Setup.aspx.cs |
| Master | Solution.Web/MasterPages/MasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAL class | TourTypeDal (Library.DAL/DoctorModule_DAL/TourTypeDal.cs) |
| DAL class | ZoneSetupDAL (Library.DAL/DoctorModule_DAL/ZoneSetupDAL.cs) |
| DAO/entity class | Area (Library.DAO/DoctorModule_DAO/Area.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | SubTerritoryDAO (Library.DAO/DoctorModule_DAO/SubTerritoryDAO.cs) |
| DAO/entity class | TADAMarketruleConfig (Library.DAO/DoctorModule_DAO/TADAMarketruleConfig.cs) |
| DAO/entity class | Territory (Library.DAO/DoctorModule_DAO/Territory.cs) |
| DAO/entity class | Trainning (Library.DAO/DoctorModule_DAO/Trainning.cs) |
| DAO/entity class | Zone (Library.DAO/DoctorModule_DAO/Zone.cs) |

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
- No UI. About 75 client references to `Setup.aspx/<Method>` exist under `Solution.Web` (grep): e.g. `Get_UserRoleInfo` (7), `GetThana_WitTagDetails_forEditPage` (6), `Get_CapturedBy_For_ddl` (4), `TourPlanApproveList`, `Approve_TourPlanList`, `Save_TourPurpose`, `Save_MonthlyAllowance`, `Delete_Trainning` (2 each) and single-use `SaveZone`, `SaveArea`, `SaveTerritory`, `SaveSubTerritory`.
- **Broken client references:** these are called by pages but are *not* defined (commented out or missing) in `Setup.aspx.cs`: `Save_TourPurpose`, `Save_TourType`, `Save_MonthlyAllowance`, `Save_DoctorSpeacialDay`, `Save_DepotWiseAreaInfo`, `Save_DepartmentInfo`, `SaveMarket`, `LoadDepotlist`, `LoadCompany`, `LoadAreaByDepotId`, `GetDepartmentList`, `GetDepartmentEditData`, `GetTourPurposeEditData`, `GetTourTypeEditData`, `GetMonthlyAllowanceEditData`, `GetDoctorSpecialDayForEdit`, `Delete_TADAMarketRuleConfiguration`. (Method bodies present only as `//` comments.)
- `Approve_TourPlanList` (line 136) is an **instance** (non-static) `[WebMethod]`; ASP.NET page methods must be static, so the client call `Setup.aspx/Approve_TourPlanList` cannot succeed (INFERRED).

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_Setup.Page_Load | Solution.Web/DoctorModule_UI/Setup.aspx.cs:31 | object sender, EventArgs e | DoctorModule_UI_Setup.Page_Load | no | no |
| DoctorModule_UI_Setup.Approve_PrescriptionList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:73 | string MyArry, string rbValue | DoctorModule_UI_Setup.Approve_PrescriptionList → Setup2DAL.Approve_PrescriptionList → SP: sp_ApprovePrescriptionInformation | no | yes |
| DoctorModule_UI_Setup.TourPlanApproveList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:82 | string param | DoctorModule_UI_Setup.TourPlanApproveList → TourTypeDal.GetTourPlanList → SP: sp_Get_TourPlanMasteList | no | no |
| DoctorModule_UI_Setup.GetZoneList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:120 |  | DoctorModule_UI_Setup.GetZoneList → ZoneSetupDAL.GetZoneList → SP: sp_Get_ZoneList | no | no |
| DoctorModule_UI_Setup.SaveZone | Solution.Web/DoctorModule_UI/Setup.aspx.cs:131 | Zone Zone | DoctorModule_UI_Setup.SaveZone → ZoneSetupDAL.SaveZoneInfo → SP: sp_Save_ZoneInfo, sp_Update_ZoneInfo, sp_check_Vali_MarketStructure, sp_check_ZoneInfo | no | yes |
| DoctorModule_UI_Setup.Approve_TourPlanList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:137 | string MyArry, string rbValue | DoctorModule_UI_Setup.Approve_TourPlanList → TourTypeDal.Approve_TourPlanList → SP: sp_ApproveTourPlanInformation | no | yes |
| DoctorModule_UI_Setup.GetZoneEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:146 | int id | DoctorModule_UI_Setup.GetZoneEditData → ZoneSetupDAL.GetEditData → SP: sp_Get_ZoneData_ByZoneId | no | no |
| DoctorModule_UI_Setup.SaveArea | Solution.Web/DoctorModule_UI/Setup.aspx.cs:232 | Area masterData | DoctorModule_UI_Setup.SaveArea → Setup2DAL.SaveAreaInfo → SP: sp_Save_AreaInfo, sp_Update_AreaInfo, sp_check_AreaInfo, sp_check_Vali_MarketStructure | no | yes |
| DoctorModule_UI_Setup.GetAreaEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:257 | int id | DoctorModule_UI_Setup.GetAreaEditData → Setup2DAL.GetEditData_Area → SP: sp_Get_AreaData_ByAreaId | no | no |
| DoctorModule_UI_Setup.GetAreaList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:266 | int RegionId | DoctorModule_UI_Setup.GetAreaList → Setup2DAL.GetAreaList → SP: sp_Get_AreaList | no | no |
| DoctorModule_UI_Setup.SaveTerritory | Solution.Web/DoctorModule_UI/Setup.aspx.cs:319 | Territory masterData | DoctorModule_UI_Setup.SaveTerritory → Setup2DAL.SaveTerritory → SP: sp_Save_TerritoryInfo, sp_Update_TerritoryData, sp_check_TerritoryInfo, sp_check_Vali_MarketStructure | no | yes |
| DoctorModule_UI_Setup.SaveSubTerritory | Solution.Web/DoctorModule_UI/Setup.aspx.cs:329 | SubTerritoryDAO masterData | DoctorModule_UI_Setup.SaveSubTerritory → Setup2DAL.SaveSubTerritory → SP: sp_Save_SubTerritoryInfo, sp_Update_SubTerritoryData, sp_check_SubTerritoryInfo, sp_check_Vali_MarketStructure | no | yes |
| DoctorModule_UI_Setup.GetTerritoryList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:352 | int RegionId, int areaId | DoctorModule_UI_Setup.GetTerritoryList → Setup2DAL.GetTerritoryList → SP: sp_Get_TerritoryList | no | no |
| DoctorModule_UI_Setup.GetSubTerritoryList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:361 |  | DoctorModule_UI_Setup.GetSubTerritoryList → CommonDataLoad.GetHigharcyInfoByEmployeeId / Setup2DAL.GetSubTerritoryList → SP: sp_Get_SubTerritoryList, sp_HigharchyInfoByEmployeeId | yes | no |
| DoctorModule_UI_Setup.GetTerrritoryEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:465 | int id | DoctorModule_UI_Setup.GetTerrritoryEditData → Setup2DAL.GetEditData_Territory → SP: sp_Get_TerritoryData_ByTerritoryId | no | no |
| DoctorModule_UI_Setup.GetSubTerrritoryEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:472 | int id | DoctorModule_UI_Setup.GetSubTerrritoryEditData → Setup2DAL.GetEditData_SubTerritory → SP: sp_Get_subTerritoryData_BySubTerritoryId | no | no |
| DoctorModule_UI_Setup.GetThana_WitTagDetails_forEditPage | Solution.Web/DoctorModule_UI/Setup.aspx.cs:638 | int id | DoctorModule_UI_Setup.GetThana_WitTagDetails_forEditPage → Setup2DAL.GetThana_All_WithTagInfo_ForEditPage → SP: sp_Get_Thana_WithTagInfo_TerritoryEdit | no | no |
| DoctorModule_UI_Setup.GetMarketEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:673 | int id | DoctorModule_UI_Setup.GetMarketEditData → Setup2DAL.GetMarketEditDataDAL → SP: sp_Get_MarketData_ByMarketid | no | no |
| DoctorModule_UI_Setup.Get_CapturedBy_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:990 |  | DoctorModule_UI_Setup.Get_CapturedBy_For_ddl → Setup2DAL.GetCapturedBy_For_ddl → SP: sp_Get_CapturedBY_For_DDL | no | no |
| DoctorModule_UI_Setup.Get_Doctor_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:998 |  | DoctorModule_UI_Setup.Get_Doctor_For_ddl → Setup2DAL.GetDFoctorlist_For_ddl → SP: sp_Get_Doctor_For_DDL | no | no |
| DoctorModule_UI_Setup.Get_ProductList_List_New | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1006 |  | DoctorModule_UI_Setup.Get_ProductList_List_New → Setup2DAL.Get_ProductList_List → SP: sp_Get_Product_List | no | no |
| DoctorModule_UI_Setup.Get_PrescriptionType_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1016 |  | DoctorModule_UI_Setup.Get_PrescriptionType_For_ddl → Setup2DAL.GetPrescriptionTypelist_For_ddl → SP: sp_Get_PrescriptionType_For_DDL | no | no |
| DoctorModule_UI_Setup.Get_ProductList_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1024 |  | DoctorModule_UI_Setup.Get_ProductList_For_ddl → Setup2DAL.Get_ProductList_For_Ddl → SP: sp_Get_Product_For_DDl | no | no |
| DoctorModule_UI_Setup.Delete_Prescription | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1055 | int Id | DoctorModule_UI_Setup.Delete_Prescription → Setup2DAL.Delete__Prescription → SP: sp_Delete_Prescription_All | no | yes |
| DoctorModule_UI_Setup.Get_ApprovalStatus_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1069 |  | DoctorModule_UI_Setup.Get_ApprovalStatus_ddl → SeedDataDAL.GetApprovalStatusList → SP: sp_GET_ActionStatusList | no | no |
| DoctorModule_UI_Setup.Get_PrescriptionList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1104 | string param | DoctorModule_UI_Setup.Get_PrescriptionList → Setup2DAL.Get_PrescriptionList → SP: sp_Get_PrescriptionList | no | no |
| DoctorModule_UI_Setup.Get_TADAList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1183 | string param | DoctorModule_UI_Setup.Get_TADAList → Setup2DAL.Get_TADAList → SP: sp_Get_TadaClaimList_new | no | no |
| DoctorModule_UI_Setup.GetTransportList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1203 |  | DoctorModule_UI_Setup.GetTransportList → Setup2DAL.GetTransportList → SP: sp_Get_Transport | no | no |
| DoctorModule_UI_Setup.Delete_MonthlyAllowance | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1235 | int Id | DoctorModule_UI_Setup.Delete_MonthlyAllowance → Setup2DAL.DeleteMonthlyAllowance → SP: sp_Delete_MonthlyAllowance | no | yes |
| DoctorModule_UI_Setup.GetMonthlyAllowanceList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1251 |  | DoctorModule_UI_Setup.GetMonthlyAllowanceList → Setup2DAL.GetMonthlyAllowanceList → SP: sp_Get_MonthlyAllowance | no | no |
| DoctorModule_UI_Setup.Get_AllowanceName_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1260 |  | DoctorModule_UI_Setup.Get_AllowanceName_For_ddl → Setup2DAL.GetAllowance_For_ddl → SP: sp_Get_Allowance_For_DDL | no | no |
| DoctorModule_UI_Setup.Save_TADAMarketRuleConfiguration | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1451 | TADAMarketruleConfig tADAMarketrule | DoctorModule_UI_Setup.Save_TADAMarketRuleConfiguration → Setup2DAL.SaveTADAMarketRuleConfiguration → SP: sp_Save_TADAMarketRulesConfig, sp_Update_TADAMarketRulesConfig, sp_check_TADAMarketRuleConfiguration | no | yes |
| DoctorModule_UI_Setup.GetTADAMarketRuleConfigurationDataById | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1456 | int id | DoctorModule_UI_Setup.GetTADAMarketRuleConfigurationDataById → Setup2DAL.GeTADAMarketRuleConfigurationForEdit → SP: sp_Get_TADAMarketRulesConfig_ByID | no | no |
| DoctorModule_UI_Setup.GetTADAMarketRuleConfigurationList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1461 |  | DoctorModule_UI_Setup.GetTADAMarketRuleConfigurationList → Setup2DAL.GetTADAMarketRuleConfigurationList → SP: sp_Get_TADAMarketRulesConfig | no | no |
| DoctorModule_UI_Setup.Get_TADAMarketRuleConfiguration_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1470 |  | DoctorModule_UI_Setup.Get_TADAMarketRuleConfiguration_For_ddl → Setup2DAL.GetTADAMarketRuleConfiguration_For_ddl → SP: sp_Get_TADAMarketRuleConfig_For_DDL | no | no |
| DoctorModule_UI_Setup.Get_UserRoleInfo | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1478 |  | DoctorModule_UI_Setup.Get_UserRoleInfo → Setup2DAL.Get_UserRoleInfo → SP: sp_Get_UserRoleInfo | no | no |
| DoctorModule_UI_Setup.GetCustomerCategory | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1487 |  | DoctorModule_UI_Setup.GetCustomerCategory → Setup2DAL.GetCustomerCategory → SP: sp_Get_CustomerCategory | no | no |
| DoctorModule_UI_Setup.GetEmployeeList_Active_Neww | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1498 |  | DoctorModule_UI_Setup.GetEmployeeList_Active_Neww → CommonDataLoad.GetEmployeeList_Active → SP: sp_CS_GetEmpGeneralInfo_All | no | no |
| DoctorModule_UI_Setup.GetEmployeeDesignation | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1507 |  | DoctorModule_UI_Setup.GetEmployeeDesignation → CommonDataLoad.GetDesignation_Active_Emp → SP: sp_CS_Designation_Active | no | no |
| DoctorModule_UI_Setup.Get_UserTypeInfo | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1516 |  | DoctorModule_UI_Setup.Get_UserTypeInfo → Setup2DAL.Get_UserTypeInfo → SP: sp_GET_RoleType | no | no |
| DoctorModule_UI_Setup.Get_StationTypeInfo | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1526 |  | DoctorModule_UI_Setup.Get_StationTypeInfo → Setup2DAL.Get_StationTypeInfoDAL → SP: sp_GET_StationTypeList | no | no |
| DoctorModule_UI_Setup.GetTrainningList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1630 |  | DoctorModule_UI_Setup.GetTrainningList → Setup2DAL.GetTranningList → SP: sp_Get_Trainninglist | no | no |
| DoctorModule_UI_Setup.Delete_Trainning | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1639 | int Id | DoctorModule_UI_Setup.Delete_Trainning → Setup2DAL.Delete_trainning → SP: sp_Delete_Trainning | no | yes |
| DoctorModule_UI_Setup.GetTrainningEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1644 | int id | DoctorModule_UI_Setup.GetTrainningEditData → Setup2DAL.GetTrainningForEdit → SP: sp_Get_Trainning_ById | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_Setup.Approve_PrescriptionList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:73 | string MyArry, string rbValue | ~/DoctorModule_UI/Setup.aspx/Approve_PrescriptionList | via BLL |
| DoctorModule_UI_Setup.TourPlanApproveList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:82 | string param | ~/DoctorModule_UI/Setup.aspx/TourPlanApproveList | via BLL |
| DoctorModule_UI_Setup.GetZoneList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:120 |  | ~/DoctorModule_UI/Setup.aspx/GetZoneList | via BLL |
| DoctorModule_UI_Setup.SaveZone | Solution.Web/DoctorModule_UI/Setup.aspx.cs:131 | Zone Zone | ~/DoctorModule_UI/Setup.aspx/SaveZone | via BLL |
| DoctorModule_UI_Setup.Approve_TourPlanList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:137 | string MyArry, string rbValue | ~/DoctorModule_UI/Setup.aspx/Approve_TourPlanList | via BLL |
| DoctorModule_UI_Setup.GetZoneEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:146 | int id | ~/DoctorModule_UI/Setup.aspx/GetZoneEditData | via BLL |
| DoctorModule_UI_Setup.SaveArea | Solution.Web/DoctorModule_UI/Setup.aspx.cs:232 | Area masterData | ~/DoctorModule_UI/Setup.aspx/SaveArea | via BLL |
| DoctorModule_UI_Setup.GetAreaEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:257 | int id | ~/DoctorModule_UI/Setup.aspx/GetAreaEditData | via BLL |
| DoctorModule_UI_Setup.GetAreaList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:266 | int RegionId | ~/DoctorModule_UI/Setup.aspx/GetAreaList | via BLL |
| DoctorModule_UI_Setup.SaveTerritory | Solution.Web/DoctorModule_UI/Setup.aspx.cs:319 | Territory masterData | ~/DoctorModule_UI/Setup.aspx/SaveTerritory | via BLL |
| DoctorModule_UI_Setup.SaveSubTerritory | Solution.Web/DoctorModule_UI/Setup.aspx.cs:329 | SubTerritoryDAO masterData | ~/DoctorModule_UI/Setup.aspx/SaveSubTerritory | via BLL |
| DoctorModule_UI_Setup.GetTerritoryList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:352 | int RegionId, int areaId | ~/DoctorModule_UI/Setup.aspx/GetTerritoryList | via BLL |
| DoctorModule_UI_Setup.GetSubTerritoryList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:361 |  | ~/DoctorModule_UI/Setup.aspx/GetSubTerritoryList | via BLL |
| DoctorModule_UI_Setup.GetTerrritoryEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:465 | int id | ~/DoctorModule_UI/Setup.aspx/GetTerrritoryEditData | via BLL |
| DoctorModule_UI_Setup.GetSubTerrritoryEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:472 | int id | ~/DoctorModule_UI/Setup.aspx/GetSubTerrritoryEditData | via BLL |
| DoctorModule_UI_Setup.GetThana_WitTagDetails_forEditPage | Solution.Web/DoctorModule_UI/Setup.aspx.cs:638 | int id | ~/DoctorModule_UI/Setup.aspx/GetThana_WitTagDetails_forEditPage | via BLL |
| DoctorModule_UI_Setup.GetMarketEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:673 | int id | ~/DoctorModule_UI/Setup.aspx/GetMarketEditData | via BLL |
| DoctorModule_UI_Setup.Get_CapturedBy_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:990 |  | ~/DoctorModule_UI/Setup.aspx/Get_CapturedBy_For_ddl | via BLL |
| DoctorModule_UI_Setup.Get_Doctor_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:998 |  | ~/DoctorModule_UI/Setup.aspx/Get_Doctor_For_ddl | via BLL |
| DoctorModule_UI_Setup.Get_ProductList_List_New | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1006 |  | ~/DoctorModule_UI/Setup.aspx/Get_ProductList_List_New | via BLL |
| DoctorModule_UI_Setup.Get_PrescriptionType_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1016 |  | ~/DoctorModule_UI/Setup.aspx/Get_PrescriptionType_For_ddl | via BLL |
| DoctorModule_UI_Setup.Get_ProductList_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1024 |  | ~/DoctorModule_UI/Setup.aspx/Get_ProductList_For_ddl | via BLL |
| DoctorModule_UI_Setup.Delete_Prescription | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1055 | int Id | ~/DoctorModule_UI/Setup.aspx/Delete_Prescription | via BLL |
| DoctorModule_UI_Setup.Get_ApprovalStatus_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1069 |  | ~/DoctorModule_UI/Setup.aspx/Get_ApprovalStatus_ddl | via BLL |
| DoctorModule_UI_Setup.Get_PrescriptionList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1104 | string param | ~/DoctorModule_UI/Setup.aspx/Get_PrescriptionList | via BLL |
| DoctorModule_UI_Setup.Get_TADAList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1183 | string param | ~/DoctorModule_UI/Setup.aspx/Get_TADAList | via BLL |
| DoctorModule_UI_Setup.GetTransportList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1203 |  | ~/DoctorModule_UI/Setup.aspx/GetTransportList | via BLL |
| DoctorModule_UI_Setup.Delete_MonthlyAllowance | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1235 | int Id | ~/DoctorModule_UI/Setup.aspx/Delete_MonthlyAllowance | via BLL |
| DoctorModule_UI_Setup.GetMonthlyAllowanceList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1251 |  | ~/DoctorModule_UI/Setup.aspx/GetMonthlyAllowanceList | via BLL |
| DoctorModule_UI_Setup.Get_AllowanceName_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1260 |  | ~/DoctorModule_UI/Setup.aspx/Get_AllowanceName_For_ddl | via BLL |
| DoctorModule_UI_Setup.Save_TADAMarketRuleConfiguration | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1451 | TADAMarketruleConfig tADAMarketrule | ~/DoctorModule_UI/Setup.aspx/Save_TADAMarketRuleConfiguration | via BLL |
| DoctorModule_UI_Setup.GetTADAMarketRuleConfigurationDataById | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1456 | int id | ~/DoctorModule_UI/Setup.aspx/GetTADAMarketRuleConfigurationDataById | via BLL |
| DoctorModule_UI_Setup.GetTADAMarketRuleConfigurationList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1461 |  | ~/DoctorModule_UI/Setup.aspx/GetTADAMarketRuleConfigurationList | via BLL |
| DoctorModule_UI_Setup.Get_TADAMarketRuleConfiguration_For_ddl | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1470 |  | ~/DoctorModule_UI/Setup.aspx/Get_TADAMarketRuleConfiguration_For_ddl | via BLL |
| DoctorModule_UI_Setup.Get_UserRoleInfo | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1478 |  | ~/DoctorModule_UI/Setup.aspx/Get_UserRoleInfo | via BLL |
| DoctorModule_UI_Setup.GetCustomerCategory | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1487 |  | ~/DoctorModule_UI/Setup.aspx/GetCustomerCategory | via BLL |
| DoctorModule_UI_Setup.GetEmployeeList_Active_Neww | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1498 |  | ~/DoctorModule_UI/Setup.aspx/GetEmployeeList_Active_Neww | via BLL |
| DoctorModule_UI_Setup.GetEmployeeDesignation | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1507 |  | ~/DoctorModule_UI/Setup.aspx/GetEmployeeDesignation | via BLL |
| DoctorModule_UI_Setup.Get_UserTypeInfo | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1516 |  | ~/DoctorModule_UI/Setup.aspx/Get_UserTypeInfo | via BLL |
| DoctorModule_UI_Setup.Get_StationTypeInfo | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1526 |  | ~/DoctorModule_UI/Setup.aspx/Get_StationTypeInfo | via BLL |
| DoctorModule_UI_Setup.GetTrainningList | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1630 |  | ~/DoctorModule_UI/Setup.aspx/GetTrainningList | via BLL |
| DoctorModule_UI_Setup.Delete_Trainning | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1639 | int Id | ~/DoctorModule_UI/Setup.aspx/Delete_Trainning | via BLL |
| DoctorModule_UI_Setup.GetTrainningEditData | Solution.Web/DoctorModule_UI/Setup.aspx.cs:1644 | int id | ~/DoctorModule_UI/Setup.aspx/GetTrainningEditData | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.Approve_PrescriptionList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:994 | sp_ApprovePrescriptionInformation | @ApprovedBy, @Status, @TPMaster |
| TourTypeDal.GetTourPlanList | Library.DAL/DoctorModule_DAL/TourTypeDal.cs:117 | sp_Get_TourPlanMasteList | @param |
| ZoneSetupDAL.GetZoneList | Library.DAL/DoctorModule_DAL/ZoneSetupDAL.cs:79 | sp_Get_ZoneList |  |
| ZoneSetupDAL.SaveZoneInfo | Library.DAL/DoctorModule_DAL/ZoneSetupDAL.cs:98 | sp_Save_ZoneInfo, sp_Update_ZoneInfo, sp_check_Vali_MarketStructure, sp_check_Zo | @CodeStr, @GroupId, @MasterId, @Name, @PageName, @acInAcDate, @createdBy, @id, @isActive, @remarks,  |
| TourTypeDal.Approve_TourPlanList | Library.DAL/DoctorModule_DAL/TourTypeDal.cs:350 | sp_ApproveTourPlanInformation | @ApprovedBy, @Status, @TPMaster |
| ZoneSetupDAL.GetEditData | Library.DAL/DoctorModule_DAL/ZoneSetupDAL.cs:20 | sp_Get_ZoneData_ByZoneId | @id |
| Setup2DAL.SaveAreaInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:212 | sp_Save_AreaInfo, sp_Update_AreaInfo, sp_check_AreaInfo, sp_check_Vali_MarketStr | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaName, @createdBy, @id, @isActive, @remarks, |
| Setup2DAL.GetEditData_Area | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:536 | sp_Get_AreaData_ByAreaId | @id |
| Setup2DAL.GetAreaList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:321 | sp_Get_AreaList | @RegionId |
| Setup2DAL.SaveTerritory | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:614 | sp_Save_TerritoryInfo, sp_Update_TerritoryData, sp_check_TerritoryInfo, sp_check | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaId, @createdBy, @id, @isActive, @remarks |
| Setup2DAL.SaveSubTerritory | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:723 | sp_Save_SubTerritoryInfo, sp_Update_SubTerritoryData, sp_check_SubTerritoryInfo, | @MasterId, @Name, @PageName, @TerritoryId, @acInAcDate, @createdBy, @id, @isActive |
| Setup2DAL.GetTerritoryList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:1150 | sp_Get_TerritoryList | @RegionId, @areaId |
| CommonDataLoad.GetHigharcyInfoByEmployeeId | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1866 | sp_HigharchyInfoByEmployeeId | @EmployeeId, @RoleId |
| Setup2DAL.GetSubTerritoryList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:1173 | sp_Get_SubTerritoryList | @Parameter |
| Setup2DAL.GetEditData_Territory | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:825 | sp_Get_TerritoryData_ByTerritoryId | @id |
| Setup2DAL.GetEditData_SubTerritory | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:869 | sp_Get_subTerritoryData_BySubTerritoryId | @id |
| Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2917 | sp_Get_Thana_WithTagInfo_TerritoryEdit | @id |
| Setup2DAL.GetMarketEditDataDAL | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2683 | sp_Get_MarketData_ByMarketid | @id |
| Setup2DAL.GetCapturedBy_For_ddl | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:25 | sp_Get_CapturedBY_For_DDL |  |
| Setup2DAL.GetDFoctorlist_For_ddl | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:62 | sp_Get_Doctor_For_DDL |  |
| Setup2DAL.Get_ProductList_List | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:192 | sp_Get_Product_List |  |
| Setup2DAL.GetPrescriptionTypelist_For_ddl | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:43 | sp_Get_PrescriptionType_For_DDL |  |
| Setup2DAL.Get_ProductList_For_Ddl | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:170 | sp_Get_Product_For_DDl |  |
| Setup2DAL.Delete__Prescription | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:3351 | sp_Delete_Prescription_All | @PrescriptionTypeId |
| SeedDataDAL.GetApprovalStatusList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:869 | sp_GET_ActionStatusList |  |
| Setup2DAL.Get_PrescriptionList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:3478 | sp_Get_PrescriptionList | @param |
| Setup2DAL.Get_TADAList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:100 | sp_Get_TadaClaimList_new | @param |
| Setup2DAL.GetTransportList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:3152 | sp_Get_Transport |  |
| Setup2DAL.DeleteMonthlyAllowance | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4139 | sp_Delete_MonthlyAllowance | @MonthlyAllowanceId |
| Setup2DAL.GetMonthlyAllowanceList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4170 | sp_Get_MonthlyAllowance |  |
| Setup2DAL.GetAllowance_For_ddl | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4440 | sp_Get_Allowance_For_DDL |  |
| Setup2DAL.SaveTADAMarketRuleConfiguration | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4609 | sp_Save_TADAMarketRulesConfig, sp_Update_TADAMarketRulesConfig, sp_check_TADAMar | @AreaId, @DAAmount, @EntryBy, @GroupId, @IsActive, @IsBoth, @IsMarketWise, @IsRoleWise, @MarketId, @ |
| Setup2DAL.GeTADAMarketRuleConfigurationForEdit | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4511 | sp_Get_TADAMarketRulesConfig_ByID | @TADAMarketRuleConfigId |
| Setup2DAL.GetTADAMarketRuleConfigurationList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4493 | sp_Get_TADAMarketRulesConfig |  |
| Setup2DAL.GetTADAMarketRuleConfiguration_For_ddl | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4708 | sp_Get_TADAMarketRuleConfig_For_DDL |  |
| Setup2DAL.Get_UserRoleInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4727 | sp_Get_UserRoleInfo |  |
| Setup2DAL.GetCustomerCategory | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4747 | sp_Get_CustomerCategory |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| CommonDataLoad.GetDesignation_Active_Emp | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:579 | sp_CS_Designation_Active |  |
| Setup2DAL.Get_UserTypeInfo | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4766 | sp_GET_RoleType |  |
| Setup2DAL.Get_StationTypeInfoDAL | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4785 | sp_GET_StationTypeList |  |
| Setup2DAL.GetTranningList | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5021 | sp_Get_Trainninglist |  |
| Setup2DAL.Delete_trainning | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5042 | sp_Delete_Trainning | @TrainningId |
| Setup2DAL.GetTrainningForEdit | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5175 | sp_Get_Trainning_ById | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_ApprovePrescriptionInformation | Setup2DAL.Approve_PrescriptionList | body NOT in repository | write-like name |
| sp_ApproveTourPlanInformation | TourTypeDal.Approve_TourPlanList | body NOT in repository | write-like name |
| sp_CS_Designation_Active | CommonDataLoad.GetDesignation_Active_Emp | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Delete_MonthlyAllowance | Setup2DAL.DeleteMonthlyAllowance | body NOT in repository | write-like name |
| sp_Delete_Prescription_All | Setup2DAL.Delete__Prescription | body NOT in repository | write-like name |
| sp_Delete_Trainning | Setup2DAL.Delete_trainning | body NOT in repository | write-like name |
| sp_GET_ActionStatusList | SeedDataDAL.GetApprovalStatusList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_RoleType | Setup2DAL.Get_UserTypeInfo | body NOT in repository | read-like name (INFERRED) |
| sp_GET_StationTypeList | Setup2DAL.Get_StationTypeInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Allowance_For_DDL | Setup2DAL.GetAllowance_For_ddl | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaData_ByAreaId | Setup2DAL.GetEditData_Area | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AreaList | Setup2DAL.GetAreaList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CapturedBY_For_DDL | Setup2DAL.GetCapturedBy_For_ddl | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CustomerCategory | Setup2DAL.GetCustomerCategory | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Doctor_For_DDL | Setup2DAL.GetDFoctorlist_For_ddl | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MarketData_ByMarketid | Setup2DAL.GetMarketEditDataDAL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_MonthlyAllowance | Setup2DAL.GetMonthlyAllowanceList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_PrescriptionList | Setup2DAL.Get_PrescriptionList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_PrescriptionType_For_DDL | Setup2DAL.GetPrescriptionTypelist_For_ddl | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Product_For_DDl | Setup2DAL.Get_ProductList_For_Ddl | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Product_List | Setup2DAL.Get_ProductList_List | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SubTerritoryList | Setup2DAL.GetSubTerritoryList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TADAMarketRuleConfig_For_DDL | Setup2DAL.GetTADAMarketRuleConfiguration_For_ddl | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TADAMarketRulesConfig | Setup2DAL.GetTADAMarketRuleConfigurationList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TADAMarketRulesConfig_ByID | Setup2DAL.GeTADAMarketRuleConfigurationForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TadaClaimList_new | Setup2DAL.Get_TADAList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryData_ByTerritoryId | Setup2DAL.GetEditData_Territory | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TerritoryList | Setup2DAL.GetTerritoryList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Thana_WithTagInfo_TerritoryEdit | Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | body NOT in repository | read-like name (INFERRED) |
| sp_Get_TourPlanMasteList | TourTypeDal.GetTourPlanList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Trainning_ById | Setup2DAL.GetTrainningForEdit | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Trainninglist | Setup2DAL.GetTranningList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Transport | Setup2DAL.GetTransportList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | Setup2DAL.Get_UserRoleInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneData_ByZoneId | ZoneSetupDAL.GetEditData | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ZoneList | ZoneSetupDAL.GetZoneList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_subTerritoryData_BySubTerritoryId | Setup2DAL.GetEditData_SubTerritory | body NOT in repository | read-like name (INFERRED) |
| sp_HigharchyInfoByEmployeeId | CommonDataLoad.GetHigharcyInfoByEmployeeId | body NOT in repository | read-like name (INFERRED) |
| sp_Save_AreaInfo | Setup2DAL.SaveAreaInfo | body NOT in repository | write-like name |
| sp_Save_SubTerritoryInfo | Setup2DAL.SaveSubTerritory | body NOT in repository | write-like name |
| sp_Save_TADAMarketRulesConfig | Setup2DAL.SaveTADAMarketRuleConfiguration | body NOT in repository | write-like name |
| sp_Save_TerritoryInfo | Setup2DAL.SaveTerritory | body NOT in repository | write-like name |
| sp_Save_ZoneInfo | ZoneSetupDAL.SaveZoneInfo | body NOT in repository | write-like name |
| sp_Update_AreaInfo | Setup2DAL.SaveAreaInfo | body NOT in repository | write-like name |
| sp_Update_SubTerritoryData | Setup2DAL.SaveSubTerritory | body NOT in repository | write-like name |
| sp_Update_TADAMarketRulesConfig | Setup2DAL.SaveTADAMarketRuleConfiguration | body NOT in repository | write-like name |
| sp_Update_TerritoryData | Setup2DAL.SaveTerritory | body NOT in repository | write-like name |
| sp_Update_ZoneInfo | ZoneSetupDAL.SaveZoneInfo | body NOT in repository | write-like name |
| sp_check_AreaInfo | Setup2DAL.SaveAreaInfo | body NOT in repository | read-like name (INFERRED) |
| sp_check_SubTerritoryInfo | Setup2DAL.SaveSubTerritory | body NOT in repository | read-like name (INFERRED) |
| sp_check_TADAMarketRuleConfiguration | Setup2DAL.SaveTADAMarketRuleConfiguration | body NOT in repository | read-like name (INFERRED) |
| sp_check_TerritoryInfo | Setup2DAL.SaveTerritory | body NOT in repository | read-like name (INFERRED) |
| sp_check_Vali_MarketStructure | Setup2DAL.SaveAreaInfo, Setup2DAL.SaveSubTerritory, Setup2DAL.SaveTerritory, ZoneSetupDAL.SaveZoneIn | body NOT in repository | read-like name (INFERRED) |
| sp_check_ZoneInfo | ZoneSetupDAL.SaveZoneInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-019 | **Page-method endpoints carry no authentication/authorization logic.** `[WebMethod] public static` methods on an `.aspx` are invoked by POST `<page>.aspx/<Method>` (ASP.NET page-method protocol) and execute without running the master page (`NewMasterPage.master.cs:99` / `MasterPage.master` login redirect happens only when the page *renders*). Forms authentication is configured (`Web.config:80-82`) but there is no `<authorization>` element, so nothing denies anonymous callers. Methods that read `HttpContext.Current.Session["UserId"]` (all writes/approvals) fail with NullReferenceException for anonymous users, but pure lookups and any method that does not touch Session do not. INFERRED for reachability; CONFIRMED that the methods contain no checks. | ``DoctorModule_UI/Setup.aspx.cs:72-1650`` | code-behind | CONFIRMED (no checks) / INFERRED (anonymous reachability) |
| BR-M17-020 | **Sub-territory list scoping by role.** `GetSubTerritoryList()` reads Session `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (errors swallowed), calls `sp_HigharchyInfoByEmployeeId(@EmployeeId,@RoleId)` and builds a `WHERE` fragment by string concatenation: `AM` -> `and ar.AreaId in (<AreaIds>)`; `DZSM` -> `and R.RegionId in (<RegionIds>)`; `NSM` -> `and G.GroupId in (<GroupIds>)`; any other role (incl. Admin, unknown) -> no filter. If no ids were collected the fallback uses `Rows[0]` of the hierarchy table and, for NSM, mistakenly uses `RegionId` instead of `GroupId`. Values originate from the DB, not the user. If Session is missing the try/catch leaves `EmpInfoId=""`, `RoleTypeName=""` -> falls to the *default* (unfiltered) branch (`EmpInfoId != "" || EmpInfoId != null` is always true) -> **unauthenticated callers receive the full sub-territory list**. | `Setup.aspx.cs:360-462` | code-behind | CONFIRMED |
| BR-M17-025 | **Approvals/deletes.** `Approve_PrescriptionList(MyArry, rbValue)` -> `Setup2DAL.Approve_PrescriptionList` -> `sp_ApprovePrescriptionInformation(@TPMaster=<id list>, @ApprovedBy=Session UserId, @Status=rbValue)`; tour-plan approve (`TourTypeDal.Approve_TourPlanList`) -> `sp_ApproveTourPlanInformation(@TPMaster,@ApprovedBy,@Status)` (unreachable, see §11). Status values/semantics live in the SPs (UNKNOWN). `SaveZone/SaveArea/SaveTerritory/SaveSubTerritory/Save_TADAMarketRuleConfiguration/Delete_MonthlyAllowance` pass `Session["UserId"]` (throws when anonymous). **`Delete_Prescription(int Id)` -> `sp_Delete_Prescription_All` and `Delete_Trainning(int Id)` -> `sp_Delete_Trainning` read no Session value and perform deletes** - reachable anonymously (INFERRED). | `Setup.aspx.cs:72-80,130-143,231,318-332,1054,1234,1450,1638`; `Setup2DAL.cs:993-1010,3350-3361,5041-5053` | code-behind + DAL | CONFIRMED |

## 20. Permission Matrix
Session keys read: `UserId` (Approve_PrescriptionList, SaveZone/Area/Territory/SubTerritory, Delete_MonthlyAllowance, Save_TADAMarketRuleConfiguration), `RoleTypeName`, `EmpInfoId`, `RoleTypeId` (GetSubTerritoryList). Roles are scoped only in `GetSubTerritoryList` (BR-M17-020). All other list/edit-data methods and the two destructive methods above have **no** session/role test. Menu/UI pages that call these methods are menu-permissioned but the endpoints themselves are not.

## 21. Status Lifecycle
Approval status values are passed through (`rbValue`) to `sp_ApprovePrescriptionInformation` / `sp_ApproveTourPlanInformation`; allowed values and transitions UNKNOWN (SP bodies absent). Approval status list source: `SeedDataDAL.GetApprovalStatusList` (`SoftwareUseId`/`WebShow`), used by reports (e.g. PAGE-548).

## 22. Error Handling
No try/catch in the page methods; DAL layer rolls back and rethrows (writes) -> HTTP 500 to the caller. Session-less calls to write methods raise NullReferenceException (HTTP 500).

## 23. Audit
Audit: `Session["UserId"]` passed as `sessionUser`/`@ApprovedBy`; storage columns UNKNOWN.

## 24. Integration
Internal AJAX API. See INT-12. Note the approval methods act on records (prescriptions, tour plans) that originate from the field-force mobile app (INFERRED, INT-09).

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, SeedDataDAL, Setup2DAL, TourTypeDal, ZoneSetupDAL; DAO: Area, ResultInfo, SubTerritoryDAO, TADAMarketruleConfig, Territory, Trainning, Zone; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-679-01 | Positive | Logged in AM calls `GetSubTerritoryList` | only sub-territories under AM's areas | BR-M17-020 |
| TC-679-02 | Positive | DZSM calls it | filtered by RegionId list | BR-M17-020 |
| TC-679-03 | Security | No cookie calls `GetSubTerritoryList` | returns unfiltered list (verify) | BR-M17-020 |
| TC-679-04 | Positive | `Approve_PrescriptionList` with ids `1,2`, status `A` | `sp_ApprovePrescriptionInformation` with UserId | BR-M17-025 |
| TC-679-05 | Security | No cookie calls `Delete_Prescription {"Id":1}` | executes delete (verify) | BR-M17-025 |
| TC-679-06 | Negative | Call `Approve_TourPlanList` | not callable (instance method) | BR-M17-025 |
| TC-679-07 | Negative | Call `Save_TourPurpose` | HTTP 500/unknown method (commented out) | - |
| TC-679-08 | Failure | `SaveZone` with DB error | rollback, HTTP 500 | - |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/Setup.aspx`, `Solution.Web/DoctorModule_UI/Setup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | 43 live page methods | n/a | MISSING | - |
| Client vs server API | None existed | ~17 client-referenced methods do not exist | n/a | DEFECT | §11 |

## 30. Known Gaps
- Anonymous-reachable destructive methods (`Delete_Prescription`, `Delete_Trainning`) and unfiltered list fallbacks (BR-M17-019/020/025) - INFERRED reachability.
- ~75% of the file is commented-out legacy code (dozens of `//[WebMethod]` blocks); ~17 client calls point at missing methods.
- Instance `[WebMethod] Approve_TourPlanList` cannot be called as a page method.
- String-concatenated `IN (...)` filters (`GetSubTerritoryList`), NSM fallback uses RegionId.
- Static DAL singletons (`Setup2DAL`, `TourTypeDal`, `ZoneSetupDAL`, `CommonDataLoad`, `SeedDataDAL`) shared across requests.
- `using System.Web.Mvc; System.Threading.Tasks` unused.
- SP bodies missing (GAP-M17-001).

## 31. Change Impact
Consumers: setup/tour/prescription/TADA/training pages under `DoctorModule_UI`, `MasterSetup_UI`. `Setup2DAL` (5k+ lines) is shared widely; SP names in §16 are contracts for those pages.

