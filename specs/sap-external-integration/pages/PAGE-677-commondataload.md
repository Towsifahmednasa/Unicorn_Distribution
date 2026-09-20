---
page_id: PAGE-677
module: M17
page: CommonDataLoad
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-677 — CommonDataLoad

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-677 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | CommonDataLoad |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/MasterPage.master |
| Page class | DoctorModule_UI_CommonDataLoad |
| aspx size | 372 bytes |
| Code-behind LOC | 485 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Empty page (only master-page placeholders) that serves as a **JSON reference-data API for the browser**: ~55 static `[WebMethod]` endpoints (`DoctorModule_UI/CommonDataLoad.aspx/<Method>`) that return a `DataTable` serialised with `JsonConvert.SerializeObject`. Categories: market hierarchy (`GetNational_Active`, `GetZone_*`, `GetArea_ByZoneId_*`, `GetTerritory_ByAreaId_*`, `GetSubTerritory_*`, `GetMarket_*`), people (`GetEmployeeList_All/Active`, `GetNSM/DZSM/AM/MIOEmployee_All`, `GetUserList_Active`, `GetDesignation_*`, `GetDepartment_Active`, `GetShift_Active`), doctor/customer master (`GetDoctorType_Active`, `DoctorSpeciality_Active`, `GetDoctorBrand_Active`, `GetInstitute_Active`, `GetChamber_ByDoctorId`, `GetDegree_Active`, `GetCustomerType_All`, `GetThanaInfo_All`, `GetDivision/District/Thana`), tour/expense (`GetTourTypeList_*`, `GetTransportList_*`, `GetExpenseField_ByExpenseType`, `GetStationType_Active`), misc (`GetCompanyInfoAll`, `GetRoleTypeInfo`, `GetFiscalYearInfo_Active`, `GetContactType`, `GetSpecialDay_Active`). CONFIRMED: `Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs`.

## 3. Business Objective
Feed cascading drop-downs and pick-lists of the doctor/field-force/master-setup screens (jQuery `$.ajax`) without an ASMX proxy; the same URLs are also useable by any other client (e.g. an app WebView) that speaks the ASP.NET page-method protocol (INFERRED).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/DoctorModule_UI/CommonDataLoad.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs |
| Master | Solution.Web/MasterPages/MasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |

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
- No UI. Client scripts across the site call e.g. `CommonDataLoad.aspx/GetTerritory_ByAreaId_Active`, `GetStationType_Active`, `GetDesignation_Active`, `GetZone_byGroupId_All/Active`, `GetArea_ByZoneId_Active`, `GetSubTerritory_ByTerritoryId_*`, `GetMarket_*`, `GetDoctorType_Active`, `GetUserList_Active`, `GetTransportList_All` (grep count of referencing pages: see `docs/reverse-engineering/api-inventory.md`).
- Each method: `DataTable dt = _dataLoad.<DalMethod>(args); return JsonConvert.SerializeObject(dt);` where `_dataLoad` is `static CommonDataLoad` (`Library.DAL/DoctorModule_DAL/CommonDataLoad.cs`) - a single instance shared by all requests.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_CommonDataLoad.Page_Load | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:18 | object sender, EventArgs e | DoctorModule_UI_CommonDataLoad.Page_Load | no | no |
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:25 |  | DoctorModule_UI_CommonDataLoad.GetDesignation_Active → CommonDataLoad.GetDesignation_Active → SP: sp_Get_Designation_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorSpeciality_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:33 |  | DoctorModule_UI_CommonDataLoad.GetDoctorSpeciality_Active → CommonDataLoad.DoctorSpeciality_Active → SP: sp_Get_DoctorSpeciality_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorBrand_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:41 |  | DoctorModule_UI_CommonDataLoad.GetDoctorBrand_Active → CommonDataLoad.GetDoctorBrand_Active → SP: sp_Get_DoctorBrand_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetInstitute_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:49 |  | DoctorModule_UI_CommonDataLoad.GetInstitute_Active → CommonDataLoad.GetInstitute_Active → SP: sp_Get_CustMasterInstitution | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:60 | int id | DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_Active → CommonDataLoad.GetZone_byGroupId_Active → SP: sp_Get_Zone_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetChamber_ByDoctorId | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:70 | int id | DoctorModule_UI_CommonDataLoad.GetChamber_ByDoctorId → CommonDataLoad.GetChamber_ByDoctorId → SP: sp_GetChamber_ByDoctorId | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:77 | int id | DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_All → CommonDataLoad.GetZone_byGroupId_All → SP: sp_Get_Zone_AllByGroup | no | no |
| DoctorModule_UI_CommonDataLoad.GetNSMEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:86 | int id | DoctorModule_UI_CommonDataLoad.GetNSMEmployee_All → CommonDataLoad.GetNSMEmployee_All → SP: sp_Get_EmployeeNSMditID | no | no |
| DoctorModule_UI_CommonDataLoad.GetDZSMEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:96 | int id | DoctorModule_UI_CommonDataLoad.GetDZSMEmployee_All → CommonDataLoad.GetDZSMEmployee_All → SP: sp_Get_EmployeeDZSMditID | no | no |
| DoctorModule_UI_CommonDataLoad.GetAMEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:105 | int id | DoctorModule_UI_CommonDataLoad.GetAMEmployee_All → CommonDataLoad.GetAMEmployee_All → SP: sp_Get_EmployeeAMditID | no | no |
| DoctorModule_UI_CommonDataLoad.GetMIOEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:114 | int id | DoctorModule_UI_CommonDataLoad.GetMIOEmployee_All → CommonDataLoad.GetMIOEmployee_All → SP: sp_Get_EmployeeMIOEditID | no | no |
| DoctorModule_UI_CommonDataLoad.GetCompanyInfoAll | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:123 |  | DoctorModule_UI_CommonDataLoad.GetCompanyInfoAll → CommonDataLoad.GetCompanyInfoAll → SP: sp_GET_CompanyInfo | no | no |
| DoctorModule_UI_CommonDataLoad.GetExpenseField_ByExpenseType | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:131 | int id | DoctorModule_UI_CommonDataLoad.GetExpenseField_ByExpenseType → CommonDataLoad.GetExpenseField_ByExpenseType → SP: sp_Get_ExpenseDetails_ByExpenseId | no | no |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:141 | int id | DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_Active → CommonDataLoad.GetArea_ByZoneId_Active → SP: sp_CS_GetArea_ByZoneId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:149 | int id | DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_All → CommonDataLoad.GetArea_ByZoneId_All → SP: sp_CS_GetArea_ByZoneId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetContactType | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:157 |  | DoctorModule_UI_CommonDataLoad.GetContactType → CommonDataLoad.GetContactType → SP: sp_Get_ContactType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetSpecialDay_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:165 |  | DoctorModule_UI_CommonDataLoad.GetSpecialDay_Active → CommonDataLoad.GetSpecialDay_Active → SP: sp_Get_DoctorSpecialDay_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetStationType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:172 |  | DoctorModule_UI_CommonDataLoad.GetStationType_Active → CommonDataLoad.GetStationType_Active → SP: sp_Get_StationType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetChamberType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:180 |  | DoctorModule_UI_CommonDataLoad.GetChamberType_Active → CommonDataLoad.GetChamberType_Active → SP: sp_Get_ChamberType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDivision_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:188 |  | DoctorModule_UI_CommonDataLoad.GetDivision_Active → CommonDataLoad.GetDivision_Active → SP: sp_Get_Division_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDistrict_ByDivision_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:196 | int id | DoctorModule_UI_CommonDataLoad.GetDistrict_ByDivision_Active → CommonDataLoad.GetDistrict_ByDivision_Active → SP: sp_Get_DistrictList_OnlyActive_ByDivisionId | no | no |
| DoctorModule_UI_CommonDataLoad.GetThana_ByDistrict_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:204 | int id | DoctorModule_UI_CommonDataLoad.GetThana_ByDistrict_Active → CommonDataLoad.GetThana_ByDistrict_Active → SP: sp_Get_ThanaList_OnlyActive_Bydistrict_id | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:212 |  | DoctorModule_UI_CommonDataLoad.GetDoctorType_Active → CommonDataLoad.GetDoctorType_Active → SP: sp_Get_DoctorType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDoctorCustomer_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:220 |  | DoctorModule_UI_CommonDataLoad.GetDoctorCustomer_Active → CommonDataLoad.DoctorCustomer_Active → SP: sp_Get_DoctorCustomer_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetProgramType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:228 |  | DoctorModule_UI_CommonDataLoad.GetProgramType_Active → CommonDataLoad.DoctorProgramType_Active → SP: sp_Get_DoctorProgramType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDegree_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:236 |  | DoctorModule_UI_CommonDataLoad.GetDegree_Active → CommonDataLoad.GetDegree_Active → SP: sp_Get_Degree_All_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:244 |  | DoctorModule_UI_CommonDataLoad.GetZone_Active → CommonDataLoad.GetZone_Active → SP: sp_CS_GetZone_Active, sp_CS_GetZone_ByGroupId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:253 |  | DoctorModule_UI_CommonDataLoad.GetGroupInfo_Active → CommonDataLoad.GetGroupInfo_Active → SP: sp_CS_Group_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetNational_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:265 |  | DoctorModule_UI_CommonDataLoad.GetNational_Active → CommonDataLoad.GetNational_Active → SP: sp_CS_National_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetRoleTypeInfo | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:278 |  | DoctorModule_UI_CommonDataLoad.GetRoleTypeInfo → CommonDataLoad.GetRoleTypeInfoDAL → SP: sp_CS_RoleType | no | no |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:285 |  | DoctorModule_UI_CommonDataLoad.GetGroupInfo_All → CommonDataLoad.GetGroupInfo_All → SP: sp_CS_Group_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetCustomerType_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:294 |  | DoctorModule_UI_CommonDataLoad.GetCustomerType_All → CommonDataLoad.GetCustomerType_All → SP: sp_CS_CustomerType_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetThanaInfo_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:303 |  | DoctorModule_UI_CommonDataLoad.GetThanaInfo_All → CommonDataLoad.GetThanaInfo_All → SP: sp_CS_Thana_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetFiscalYearInfo_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:310 |  | DoctorModule_UI_CommonDataLoad.GetFiscalYearInfo_Active → CommonDataLoad.GetFiscalYearInfo_Active → SP: sp_CS_FiscalYearInfo_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetZone_ActiveById | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:318 | int id | DoctorModule_UI_CommonDataLoad.GetZone_ActiveById → CommonDataLoad.GetZone_Active → SP: sp_CS_GetZone_Active, sp_CS_GetZone_ByGroupId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:328 | int id | DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_Active → CommonDataLoad.GetTerritory_ByAreaId_Active → SP: sp_CS_GetTerritory_ByAreaId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:336 | int id | DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_Active → CommonDataLoad.GetSubTerritory_ByTerritoryId_Active → SP: sp_CS_GetSubTerritory_ByTerritoryId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:344 | int id | DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_All → CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle → SP: sp_CS_GetSubTerritory_ByTerritoryId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:351 | int id | DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_All → CommonDataLoad.GetTerritory_ByAreaId_All → SP: sp_CS_GetTerritory_ByAreaId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:359 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_Active → CommonDataLoad.GetMarket_ByTerritoryId_Active → SP: sp_CS_GetMarket_BySubTerritoryId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:367 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_All → CommonDataLoad.GetMarket_ByTerritoryId_All → SP: sp_CS_GetMarket_ByTerritoryId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:377 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_Active → CommonDataLoad.GetMarket_ByTerritoryId_Active → SP: sp_CS_GetMarket_BySubTerritoryId_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:384 | int id | DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_All → CommonDataLoad.GetMarket_BySubTerritoryId_All → SP: sp_CS_GetMarket_BySubTerritoryId_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:393 |  | DoctorModule_UI_CommonDataLoad.GetEmployeeList_All → CommonDataLoad.GetEmployeeList_All → SP: sp_CS_GetEmpGeneralInfo_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:401 |  | DoctorModule_UI_CommonDataLoad.GetEmployeeList_Active → CommonDataLoad.GetEmployeeList_Active → SP: sp_CS_GetEmpGeneralInfo_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetDepartment_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:409 |  | DoctorModule_UI_CommonDataLoad.GetDepartment_Active → CommonDataLoad.GetDepartment_Active → SP: sp_CS_Department_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active_Emp | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:417 |  | DoctorModule_UI_CommonDataLoad.GetDesignation_Active_Emp → CommonDataLoad.GetDesignation_Active_Emp → SP: sp_CS_Designation_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetShift_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:424 |  | DoctorModule_UI_CommonDataLoad.GetShift_Active → CommonDataLoad.GetShift_Active → SP: sp_CS_Shift_Active | no | no |
| DoctorModule_UI_CommonDataLoad.CompanyList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:431 |  | DoctorModule_UI_CommonDataLoad.CompanyList_Active → CommonDataLoad.CompanyList_Active → SP: sp_CS_GetEmpGeneralInfo_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:439 |  | DoctorModule_UI_CommonDataLoad.GetTourTypeList_Active → CommonDataLoad.GetTourTypeList_Active → SP: sp_CS_TourPlanType_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:448 |  | DoctorModule_UI_CommonDataLoad.GetTourTypeList_All → CommonDataLoad.GetTourTypeList_ALL → SP: sp_CS_TourPlanType_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetTransportList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:456 |  | DoctorModule_UI_CommonDataLoad.GetTransportList_Active → CommonDataLoad.GetTransportList_Active → SP: sp_CS_Transport_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetTransportList_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:464 |  | DoctorModule_UI_CommonDataLoad.GetTransportList_All → CommonDataLoad.GetTransportList_All → SP: sp_CS_Transport_All | no | no |
| DoctorModule_UI_CommonDataLoad.GetUserList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:472 |  | DoctorModule_UI_CommonDataLoad.GetUserList_Active → CommonDataLoad.GetUserList_Active → SP: sp_CS_UserInfo_Active | no | no |
| DoctorModule_UI_CommonDataLoad.GetExpenseType | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:479 |  | DoctorModule_UI_CommonDataLoad.GetExpenseType → CommonDataLoad.GetExpenseType → SP: sp_CS_GetExpenseType_Active | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:25 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDesignation_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorSpeciality_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:33 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDoctorSpeciality_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorBrand_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:41 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDoctorBrand_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetInstitute_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:49 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetInstitute_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:60 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetZone_byGroupId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetChamber_ByDoctorId | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:70 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetChamber_ByDoctorId | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_byGroupId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:77 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetZone_byGroupId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetNSMEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:86 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetNSMEmployee_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDZSMEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:96 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDZSMEmployee_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetAMEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:105 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetAMEmployee_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMIOEmployee_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:114 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetMIOEmployee_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetCompanyInfoAll | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:123 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetCompanyInfoAll | via BLL |
| DoctorModule_UI_CommonDataLoad.GetExpenseField_ByExpenseType | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:131 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetExpenseField_ByExpenseType | via BLL |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:141 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetArea_ByZoneId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetArea_ByZoneId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:149 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetArea_ByZoneId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetContactType | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:157 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetContactType | via BLL |
| DoctorModule_UI_CommonDataLoad.GetSpecialDay_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:165 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetSpecialDay_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetStationType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:172 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetStationType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetChamberType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:180 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetChamberType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDivision_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:188 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDivision_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDistrict_ByDivision_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:196 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDistrict_ByDivision_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetThana_ByDistrict_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:204 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetThana_ByDistrict_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:212 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDoctorType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDoctorCustomer_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:220 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDoctorCustomer_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetProgramType_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:228 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetProgramType_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDegree_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:236 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDegree_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:244 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetZone_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:253 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetGroupInfo_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetNational_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:265 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetNational_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetRoleTypeInfo | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:278 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetRoleTypeInfo | via BLL |
| DoctorModule_UI_CommonDataLoad.GetGroupInfo_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:285 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetGroupInfo_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetCustomerType_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:294 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetCustomerType_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetThanaInfo_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:303 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetThanaInfo_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetFiscalYearInfo_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:310 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetFiscalYearInfo_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetZone_ActiveById | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:318 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetZone_ActiveById | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:328 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetTerritory_ByAreaId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:336 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetSubTerritory_ByTerritoryId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetSubTerritory_ByTerritoryId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:344 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetSubTerritory_ByTerritoryId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTerritory_ByAreaId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:351 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetTerritory_ByAreaId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:359 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetMarket_ByTerritoryId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_ByTerritoryId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:367 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetMarket_ByTerritoryId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:377 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetMarket_BySubTerritoryId_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetMarket_BySubTerritoryId_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:384 | int id | ~/DoctorModule_UI/CommonDataLoad.aspx/GetMarket_BySubTerritoryId_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:393 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetEmployeeList_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetEmployeeList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:401 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetEmployeeList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDepartment_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:409 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDepartment_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetDesignation_Active_Emp | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:417 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetDesignation_Active_Emp | via BLL |
| DoctorModule_UI_CommonDataLoad.GetShift_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:424 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetShift_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.CompanyList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:431 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/CompanyList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:439 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetTourTypeList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTourTypeList_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:448 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetTourTypeList_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTransportList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:456 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetTransportList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetTransportList_All | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:464 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetTransportList_All | via BLL |
| DoctorModule_UI_CommonDataLoad.GetUserList_Active | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:472 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetUserList_Active | via BLL |
| DoctorModule_UI_CommonDataLoad.GetExpenseType | Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs:479 |  | ~/DoctorModule_UI/CommonDataLoad.aspx/GetExpenseType | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetDesignation_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:733 | sp_Get_Designation_All_Active |  |
| CommonDataLoad.DoctorSpeciality_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:811 | sp_Get_DoctorSpeciality_Active |  |
| CommonDataLoad.GetDoctorBrand_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:874 | sp_Get_DoctorBrand_Active |  |
| CommonDataLoad.GetInstitute_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:895 | sp_Get_CustMasterInstitution |  |
| CommonDataLoad.GetZone_byGroupId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1213 | sp_Get_Zone_All_Active | @GroupId |
| CommonDataLoad.GetChamber_ByDoctorId | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1234 | sp_GetChamber_ByDoctorId | @DoctorId |
| CommonDataLoad.GetZone_byGroupId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1254 | sp_Get_Zone_AllByGroup | @GroupId |
| CommonDataLoad.GetNSMEmployee_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1314 | sp_Get_EmployeeNSMditID | @Id |
| CommonDataLoad.GetDZSMEmployee_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1334 | sp_Get_EmployeeDZSMditID | @Id |
| CommonDataLoad.GetAMEmployee_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1355 | sp_Get_EmployeeAMditID | @Id |
| CommonDataLoad.GetMIOEmployee_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1375 | sp_Get_EmployeeMIOEditID | @Id |
| CommonDataLoad.GetCompanyInfoAll | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1412 | sp_GET_CompanyInfo |  |
| CommonDataLoad.GetExpenseField_ByExpenseType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1501 | sp_Get_ExpenseDetails_ByExpenseId | @id |
| CommonDataLoad.GetArea_ByZoneId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1131 | sp_CS_GetArea_ByZoneId_Active | @id |
| CommonDataLoad.GetArea_ByZoneId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1152 | sp_CS_GetArea_ByZoneId_All | @id |
| CommonDataLoad.GetContactType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:914 | sp_Get_ContactType_Active |  |
| CommonDataLoad.GetSpecialDay_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:933 | sp_Get_DoctorSpecialDay_All_Active |  |
| CommonDataLoad.GetStationType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:951 | sp_Get_StationType_Active |  |
| CommonDataLoad.GetChamberType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:971 | sp_Get_ChamberType_Active |  |
| CommonDataLoad.GetDivision_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:989 | sp_Get_Division_Active |  |
| CommonDataLoad.GetDistrict_ByDivision_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1008 | sp_Get_DistrictList_OnlyActive_ByDivisionId | @id |
| CommonDataLoad.GetThana_ByDistrict_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1031 | sp_Get_ThanaList_OnlyActive_Bydistrict_id | @id |
| CommonDataLoad.GetDoctorType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:830 | sp_Get_DoctorType_Active |  |
| CommonDataLoad.DoctorCustomer_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:792 | sp_Get_DoctorCustomer_Active |  |
| CommonDataLoad.DoctorProgramType_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1054 | sp_Get_DoctorProgramType_Active |  |
| CommonDataLoad.GetDegree_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1073 | sp_Get_Degree_All_Active |  |
| CommonDataLoad.GetZone_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:128 | sp_CS_GetZone_Active |  |
| CommonDataLoad.GetGroupInfo_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:187 | sp_CS_Group_Active |  |
| CommonDataLoad.GetNational_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:205 | sp_CS_National_Active |  |
| CommonDataLoad.GetRoleTypeInfoDAL | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:339 | sp_CS_RoleType |  |
| CommonDataLoad.GetGroupInfo_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:374 | sp_CS_Group_All |  |
| CommonDataLoad.GetCustomerType_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:394 | sp_CS_CustomerType_All |  |
| CommonDataLoad.GetThanaInfo_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:413 | sp_CS_Thana_All |  |
| CommonDataLoad.GetFiscalYearInfo_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:431 | sp_CS_FiscalYearInfo_Active |  |
| CommonDataLoad.GetTerritory_ByAreaId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1540 | sp_CS_GetTerritory_ByAreaId_Active | @id |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1625 | sp_CS_GetSubTerritory_ByTerritoryId_Active | @id |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1782 | sp_CS_GetSubTerritory_ByTerritoryId_All | @id |
| CommonDataLoad.GetTerritory_ByAreaId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1823 | sp_CS_GetTerritory_ByAreaId_All | @id |
| CommonDataLoad.GetMarket_ByTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1994 | sp_CS_GetMarket_BySubTerritoryId_Active | @id |
| CommonDataLoad.GetMarket_ByTerritoryId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:2015 | sp_CS_GetMarket_ByTerritoryId_All | @id |
| CommonDataLoad.GetMarket_BySubTerritoryId_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1717 | sp_CS_GetMarket_BySubTerritoryId_All | @id |
| CommonDataLoad.GetEmployeeList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1761 | sp_CS_GetEmpGeneralInfo_All |  |
| CommonDataLoad.GetEmployeeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:467 | sp_CS_GetEmpGeneralInfo_All |  |
| CommonDataLoad.GetDepartment_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:559 | sp_CS_Department_Active |  |
| CommonDataLoad.GetDesignation_Active_Emp | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:579 | sp_CS_Designation_Active |  |
| CommonDataLoad.GetShift_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:598 | sp_CS_Shift_Active |  |
| CommonDataLoad.CompanyList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:616 | sp_CS_GetEmpGeneralInfo_Active |  |
| CommonDataLoad.GetTourTypeList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:635 | sp_CS_TourPlanType_Active |  |
| CommonDataLoad.GetTourTypeList_ALL | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:654 | sp_CS_TourPlanType_All |  |
| CommonDataLoad.GetTransportList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:673 | sp_CS_Transport_Active |  |
| CommonDataLoad.GetTransportList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:693 | sp_CS_Transport_All |  |
| CommonDataLoad.GetUserList_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:713 | sp_CS_UserInfo_Active |  |
| CommonDataLoad.GetExpenseType | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:449 | sp_CS_GetExpenseType_Active |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_CustomerType_All | CommonDataLoad.GetCustomerType_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Department_Active | CommonDataLoad.GetDepartment_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Designation_Active | CommonDataLoad.GetDesignation_Active_Emp | body NOT in repository | read-like name (INFERRED) |
| sp_CS_FiscalYearInfo_Active | CommonDataLoad.GetFiscalYearInfo_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_Active | CommonDataLoad.GetArea_ByZoneId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetArea_ByZoneId_All | CommonDataLoad.GetArea_ByZoneId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_Active | CommonDataLoad.CompanyList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_Active, CommonDataLoad.GetEmployeeList_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetExpenseType_Active | CommonDataLoad.GetExpenseType | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_BySubTerritoryId_Active | CommonDataLoad.GetMarket_ByTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_BySubTerritoryId_All | CommonDataLoad.GetMarket_BySubTerritoryId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetMarket_ByTerritoryId_All | CommonDataLoad.GetMarket_ByTerritoryId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetSubTerritory_ByTerritoryId_Active | CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetSubTerritory_ByTerritoryId_All | CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_Active | CommonDataLoad.GetTerritory_ByAreaId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetTerritory_ByAreaId_All | CommonDataLoad.GetTerritory_ByAreaId_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetZone_Active | CommonDataLoad.GetZone_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_GetZone_ByGroupId_Active | CommonDataLoad.GetZone_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Group_Active | CommonDataLoad.GetGroupInfo_Active | body NOT in repository | write-like name |
| sp_CS_Group_All | CommonDataLoad.GetGroupInfo_All | body NOT in repository | write-like name |
| sp_CS_National_Active | CommonDataLoad.GetNational_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_RoleType | CommonDataLoad.GetRoleTypeInfoDAL | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Shift_Active | CommonDataLoad.GetShift_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Thana_All | CommonDataLoad.GetThanaInfo_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_TourPlanType_Active | CommonDataLoad.GetTourTypeList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_TourPlanType_All | CommonDataLoad.GetTourTypeList_ALL | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Transport_Active | CommonDataLoad.GetTransportList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_CS_Transport_All | CommonDataLoad.GetTransportList_All | body NOT in repository | read-like name (INFERRED) |
| sp_CS_UserInfo_Active | CommonDataLoad.GetUserList_Active | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CompanyInfo | CommonDataLoad.GetCompanyInfoAll | body NOT in repository | read-like name (INFERRED) |
| sp_GetChamber_ByDoctorId | CommonDataLoad.GetChamber_ByDoctorId | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ChamberType_Active | CommonDataLoad.GetChamberType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ContactType_Active | CommonDataLoad.GetContactType | body NOT in repository | read-like name (INFERRED) |
| sp_Get_CustMasterInstitution | CommonDataLoad.GetInstitute_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Degree_All_Active | CommonDataLoad.GetDegree_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Designation_All_Active | CommonDataLoad.GetDesignation_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DistrictList_OnlyActive_ByDivisionId | CommonDataLoad.GetDistrict_ByDivision_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Division_Active | CommonDataLoad.GetDivision_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorBrand_Active | CommonDataLoad.GetDoctorBrand_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorCustomer_Active | CommonDataLoad.DoctorCustomer_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorProgramType_Active | CommonDataLoad.DoctorProgramType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorSpecialDay_All_Active | CommonDataLoad.GetSpecialDay_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorSpeciality_Active | CommonDataLoad.DoctorSpeciality_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DoctorType_Active | CommonDataLoad.GetDoctorType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeAMditID | CommonDataLoad.GetAMEmployee_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeDZSMditID | CommonDataLoad.GetDZSMEmployee_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeMIOEditID | CommonDataLoad.GetMIOEmployee_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeNSMditID | CommonDataLoad.GetNSMEmployee_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ExpenseDetails_ByExpenseId | CommonDataLoad.GetExpenseField_ByExpenseType | body NOT in repository | read-like name (INFERRED) |
| sp_Get_StationType_Active | CommonDataLoad.GetStationType_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ThanaList_OnlyActive_Bydistrict_id | CommonDataLoad.GetThana_ByDistrict_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_AllByGroup | CommonDataLoad.GetZone_byGroupId_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active | CommonDataLoad.GetZone_byGroupId_Active | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-019 | **Page-method endpoints carry no authentication/authorization logic.** `[WebMethod] public static` methods on an `.aspx` are invoked by POST `<page>.aspx/<Method>` (ASP.NET page-method protocol) and execute without running the master page (`NewMasterPage.master.cs:99` / `MasterPage.master` login redirect happens only when the page *renders*). Forms authentication is configured (`Web.config:80-82`) but there is no `<authorization>` element, so nothing denies anonymous callers. Methods that read `HttpContext.Current.Session["UserId"]` (all writes/approvals) fail with NullReferenceException for anonymous users, but pure lookups and any method that does not touch Session do not. INFERRED for reachability; CONFIRMED that the methods contain no checks. | ``DoctorModule_UI/CommonDataLoad.aspx.cs` (no `Session` reference anywhere in the file - grep 2026-09-19)` | code-behind | CONFIRMED (no checks) / INFERRED (anonymous reachability) |
| BR-M17-026 | **Sensitive lookups exposed:** `GetUserList_Active` (`sp_CS_UserInfo_Active`) and `GetEmployeeList_All/Active` (`sp_CS_GetEmpGeneralInfo_All`, `sp_Get_EmployeeList`) return user/employee master rows; the returned columns are UNKNOWN (SP bodies missing) so PII exposure cannot be confirmed. | `CommonDataLoad.aspx.cs:392-400,471-478`; `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:712-718,1768` | code-behind | CONFIRMED (endpoint exists); UNKNOWN (columns) |

## 20. Permission Matrix
Session keys read: **none** in this file (CONFIRMED). No per-method role restriction. Master page `MasterPage.master` (older master) is applied to the page but not to page-method calls.

## 21. Status Lifecycle
Not applicable (lookups only).

## 22. Error Handling
No try/catch; exceptions from DAL propagate to ASP.NET which returns HTTP 500 with the JSON error envelope (message/stack trace visibility depends on `customErrors`, not reviewed).

## 23. Audit
None.

## 24. Integration
Internal AJAX JSON API (also usable by external clients). See `specs/integrations/INT-12-page-method-json-endpoints.md`.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad; DAO: —; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-677-01 | Positive | POST `{"id":1}` to `GetArea_ByZoneId_Active` with session | JSON array of areas | BR-M17-019 |
| TC-677-02 | Security | Same POST without any cookie | should be rejected; code performs no check (verify) | BR-M17-019 |
| TC-677-03 | Security | POST `GetUserList_Active` without cookie | returns user list rows if reachable | BR-M17-026 |
| TC-677-04 | Negative | non-integer `id` | HTTP 500 | - |
| TC-677-05 | Consistency | Compare `Target_UI/CommonDataLoad.aspx/*` results | identical for shared methods | - |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/CommonDataLoad.aspx`, `Solution.Web/DoctorModule_UI/CommonDataLoad.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | ~55 page methods | n/a | MISSING | - |
| Access control | None existed | none | n/a | RISK | BR-M17-019 |

## 30. Known Gaps
- No authentication/authorization on ~55 lookup endpoints (incl. user/employee lists).
- Duplicate class copy under `Target_UI/CommonDataLoad.aspx` (PAGE-557).
- Single shared static `CommonDataLoad` DAL instance across requests (DAL objects hold a connection field; thread-safety not guaranteed).
- All SP bodies missing (GAP-M17-001).
- `using System.Web.Mvc;` imported in a WebForms page.

## 31. Change Impact
Every screen that loads drop-downs through `CommonDataLoad.aspx/*` (grep: `Solution.Web/**/*.aspx`) depends on these method names and JSON shape; the DAL class is shared with PAGE-557, PAGE-679, DWSP pages and reports.

