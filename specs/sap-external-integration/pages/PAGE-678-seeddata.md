---
page_id: PAGE-678
module: M17
page: SeedData
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-678 — SeedData

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-678 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | SeedData |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_SeedData |
| aspx size | 363 bytes |
| Code-behind LOC | 140 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Empty page hosting 13 static `[WebMethod]` JSON endpoints (`DoctorModule_UI/SeedData.aspx/<Method>`) that return "seed"/reference lists from `SeedDataDAL`: `GetDivisionList`, `GetDivisionList_NotInAnyTagWithZone`, `GetZoneList_Active`, `GetBrandNameALL`, `GetDistrictList_Active`, `GetThana_All`, `GetEmployeeList`, `GetEmployee_AllFieldForceEmployeeList`, `GetThana_WitTagDetails`, `GetGroupList`, `GetOfferTypeInfo(id)`, `GetChemistTypeList`, `GetCampaignTypeList`. CONFIRMED: `Solution.Web/DoctorModule_UI/SeedData.aspx.cs:1-139`. Backing SPs (e.g. `sp_Get_ALlDIvision`, `sp_Get_Zone_All_Active`, `sp_Get_Thana_Active`, `sp_Get_EmployeeListFieldForce`, `sp_GET_GroupInfo`, `sp_Get_OfferTypeInfo`) are in `Library.DAL/DoctorModule_DAL/SeedDataDAL.cs`.

## 3. Business Objective
Provide reference data (geography, brands, groups, field-force employees, offer/chemist/campaign types) to the set-up and customer/doctor screens.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/DoctorModule_UI/SeedData.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/SeedData.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/SeedData.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

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
No UI. Most-called endpoints (client references): `SeedData.aspx/GetGroupList` (10 pages), `GetEmployee_AllFieldForceEmployeeList` (7), `GetZoneList_Active`, `GetThana_WitTagDetails`, `GetEmployeeList`, `GetDistrictList_Active` (2 each).

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_SeedData.Page_Load | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:16 | object sender, EventArgs e | DoctorModule_UI_SeedData.Page_Load | no | no |
| DoctorModule_UI_SeedData.GetDivisionList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:24 |  | DoctorModule_UI_SeedData.GetDivisionList → SeedDataDAL.GetDivisions_All → SP: sp_Get_ALlDIvision | no | no |
| DoctorModule_UI_SeedData.GetDivisionList_NotInAnyTagWithZone | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:32 |  | DoctorModule_UI_SeedData.GetDivisionList_NotInAnyTagWithZone → SeedDataDAL.GetDivisionList_NotInAnyTagWithZone → SP: sp_Get_Division_NoTagWithZone | no | no |
| DoctorModule_UI_SeedData.GetZoneList_Active | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:41 |  | DoctorModule_UI_SeedData.GetZoneList_Active → SeedDataDAL.GetZone_ActiveOnly → SP: sp_Get_Zone_All_Active | no | no |
| DoctorModule_UI_SeedData.GetBrandNameALL | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:51 |  | DoctorModule_UI_SeedData.GetBrandNameALL → SeedDataDAL.GetBrandNameALL → SP: sp_Get_BrandName_All | no | no |
| DoctorModule_UI_SeedData.GetDistrictList_Active | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:59 |  | DoctorModule_UI_SeedData.GetDistrictList_Active → SeedDataDAL.GetDistrict_ActiveOnly → SP: sp_Get_District_All_Active_NoTag | no | no |
| DoctorModule_UI_SeedData.GetThana_All | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:69 |  | DoctorModule_UI_SeedData.GetThana_All → SeedDataDAL.GetThana_All_Active → SP: sp_Get_Thana_Active | no | no |
| DoctorModule_UI_SeedData.GetEmployeeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:78 |  | DoctorModule_UI_SeedData.GetEmployeeList → SeedDataDAL.GetEmployee_All → SP: sp_Get_EmployeeList | no | no |
| DoctorModule_UI_SeedData.GetEmployee_AllFieldForceEmployeeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:87 |  | DoctorModule_UI_SeedData.GetEmployee_AllFieldForceEmployeeList → SeedDataDAL.GetEmployee_AllFieldForceEmployeeList → SP: sp_Get_EmployeeListFieldForce | no | no |
| DoctorModule_UI_SeedData.GetThana_WitTagDetails | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:97 |  | DoctorModule_UI_SeedData.GetThana_WitTagDetails → SeedDataDAL.GetThana_All_WithTagInfo → SP: sp_Get_Thana_WithTagInfo | no | no |
| DoctorModule_UI_SeedData.GetGroupList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:107 |  | DoctorModule_UI_SeedData.GetGroupList → SeedDataDAL.GetGroup_All → SP: sp_GET_GroupInfo | no | no |
| DoctorModule_UI_SeedData.GetOfferTypeInfo | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:116 | int id | DoctorModule_UI_SeedData.GetOfferTypeInfo → SeedDataDAL.GetOfferTypeInfo → SP: sp_Get_OfferTypeInfo | no | no |
| DoctorModule_UI_SeedData.GetChemistTypeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:125 |  | DoctorModule_UI_SeedData.GetChemistTypeList → SeedDataDAL.GetChemistTypeList → SP: sp_GET_CustomerType | no | no |
| DoctorModule_UI_SeedData.GetCampaignTypeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:134 |  | DoctorModule_UI_SeedData.GetCampaignTypeList → SeedDataDAL.GetCampaignTypeList → SP: sp_GET_CampaignType | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_SeedData.GetDivisionList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:24 |  | ~/DoctorModule_UI/SeedData.aspx/GetDivisionList | via BLL |
| DoctorModule_UI_SeedData.GetDivisionList_NotInAnyTagWithZone | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:32 |  | ~/DoctorModule_UI/SeedData.aspx/GetDivisionList_NotInAnyTagWithZone | via BLL |
| DoctorModule_UI_SeedData.GetZoneList_Active | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:41 |  | ~/DoctorModule_UI/SeedData.aspx/GetZoneList_Active | via BLL |
| DoctorModule_UI_SeedData.GetBrandNameALL | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:51 |  | ~/DoctorModule_UI/SeedData.aspx/GetBrandNameALL | via BLL |
| DoctorModule_UI_SeedData.GetDistrictList_Active | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:59 |  | ~/DoctorModule_UI/SeedData.aspx/GetDistrictList_Active | via BLL |
| DoctorModule_UI_SeedData.GetThana_All | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:69 |  | ~/DoctorModule_UI/SeedData.aspx/GetThana_All | via BLL |
| DoctorModule_UI_SeedData.GetEmployeeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:78 |  | ~/DoctorModule_UI/SeedData.aspx/GetEmployeeList | via BLL |
| DoctorModule_UI_SeedData.GetEmployee_AllFieldForceEmployeeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:87 |  | ~/DoctorModule_UI/SeedData.aspx/GetEmployee_AllFieldForceEmployeeList | via BLL |
| DoctorModule_UI_SeedData.GetThana_WitTagDetails | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:97 |  | ~/DoctorModule_UI/SeedData.aspx/GetThana_WitTagDetails | via BLL |
| DoctorModule_UI_SeedData.GetGroupList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:107 |  | ~/DoctorModule_UI/SeedData.aspx/GetGroupList | via BLL |
| DoctorModule_UI_SeedData.GetOfferTypeInfo | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:116 | int id | ~/DoctorModule_UI/SeedData.aspx/GetOfferTypeInfo | via BLL |
| DoctorModule_UI_SeedData.GetChemistTypeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:125 |  | ~/DoctorModule_UI/SeedData.aspx/GetChemistTypeList | via BLL |
| DoctorModule_UI_SeedData.GetCampaignTypeList | Solution.Web/DoctorModule_UI/SeedData.aspx.cs:134 |  | ~/DoctorModule_UI/SeedData.aspx/GetCampaignTypeList | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetDivisions_All | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:16 | sp_Get_ALlDIvision |  |
| SeedDataDAL.GetDivisionList_NotInAnyTagWithZone | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:292 | sp_Get_Division_NoTagWithZone |  |
| SeedDataDAL.GetZone_ActiveOnly | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:322 | sp_Get_Zone_All_Active |  |
| SeedDataDAL.GetBrandNameALL | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:364 | sp_Get_BrandName_All |  |
| SeedDataDAL.GetDistrict_ActiveOnly | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:342 | sp_Get_District_All_Active_NoTag |  |
| SeedDataDAL.GetThana_All_Active | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:385 | sp_Get_Thana_Active |  |
| SeedDataDAL.GetEmployee_All | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:207 | delegates / no SQL literal |  |
| SeedDataDAL.GetEmployee_AllFieldForceEmployeeList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:274 | sp_Get_EmployeeListFieldForce |  |
| SeedDataDAL.GetThana_All_WithTagInfo | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:406 | sp_Get_Thana_WithTagInfo |  |
| SeedDataDAL.GetGroup_All | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:428 | sp_GET_GroupInfo |  |
| SeedDataDAL.GetOfferTypeInfo | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:446 | sp_Get_OfferTypeInfo | @id |
| SeedDataDAL.GetChemistTypeList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:610 | sp_GET_CustomerType |  |
| SeedDataDAL.GetCampaignTypeList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:1047 | sp_GET_CampaignType |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_CampaignType | SeedDataDAL.GetCampaignTypeList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_CustomerType | SeedDataDAL.GetChemistTypeList | body NOT in repository | read-like name (INFERRED) |
| sp_GET_GroupInfo | SeedDataDAL.GetGroup_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_ALlDIvision | SeedDataDAL.GetDivisions_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_BrandName_All | SeedDataDAL.GetBrandNameALL | body NOT in repository | read-like name (INFERRED) |
| sp_Get_District_All_Active_NoTag | SeedDataDAL.GetDistrict_ActiveOnly | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Division_NoTagWithZone | SeedDataDAL.GetDivisionList_NotInAnyTagWithZone | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeList | SeedDataDAL.GetEmployee_All | body NOT in repository | read-like name (INFERRED) |
| sp_Get_EmployeeListFieldForce | SeedDataDAL.GetEmployee_AllFieldForceEmployeeList | body NOT in repository | read-like name (INFERRED) |
| sp_Get_OfferTypeInfo | SeedDataDAL.GetOfferTypeInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Thana_Active | SeedDataDAL.GetThana_All_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Thana_WithTagInfo | SeedDataDAL.GetThana_All_WithTagInfo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Zone_All_Active | SeedDataDAL.GetZone_ActiveOnly | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-019 | **Page-method endpoints carry no authentication/authorization logic.** `[WebMethod] public static` methods on an `.aspx` are invoked by POST `<page>.aspx/<Method>` (ASP.NET page-method protocol) and execute without running the master page (`NewMasterPage.master.cs:99` / `MasterPage.master` login redirect happens only when the page *renders*). Forms authentication is configured (`Web.config:80-82`) but there is no `<authorization>` element, so nothing denies anonymous callers. Methods that read `HttpContext.Current.Session["UserId"]` (all writes/approvals) fail with NullReferenceException for anonymous users, but pure lookups and any method that does not touch Session do not. INFERRED for reachability; CONFIRMED that the methods contain no checks. | ``DoctorModule_UI/SeedData.aspx.cs:16-139` (no Session use)` | code-behind | CONFIRMED (no checks) / INFERRED (anonymous reachability) |
| BR-M17-027 | `GetEmployee_AllFieldForceEmployeeList` -> `sp_Get_EmployeeListFieldForce` and `GetEmployeeList` -> `sp_Get_EmployeeList` return the employee/field-force roster to unauthenticated callers (columns UNKNOWN). | `SeedData.aspx.cs:64-80`; `SeedDataDAL.cs:221,279` | code-behind | CONFIRMED (no check) |

## 20. Permission Matrix
No Session key read (CONFIRMED). No role checks. Reachability as BR-M17-019.

## 21. Status Lifecycle
Not applicable.

## 22. Error Handling
No error handling; DAL exceptions -> HTTP 500.

## 23. Audit
None.

## 24. Integration
Internal AJAX JSON API. See INT-12.

## 25. Reports
None.

## 26. Dependencies
BLL: —; DAL: SeedDataDAL; DAO: —; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-678-01 | Positive | POST `{}` to `GetGroupList` (logged in) | JSON list of groups | BR-M17-019 |
| TC-678-02 | Security | Same without cookie | returns data (verify) | BR-M17-019 |
| TC-678-03 | Positive | `GetOfferTypeInfo {"id":N}` | offer type row(s) | - |
| TC-678-04 | Negative | wrong param name | HTTP 500 | - |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/SeedData.aspx`, `Solution.Web/DoctorModule_UI/SeedData.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | 13 page methods | n/a | MISSING | - |

## 30. Known Gaps
- Unauthenticated JSON endpoints incl. employee rosters.
- `SeedDataDAL` is a 1106-line class mixing unrelated lookups (bank info, invoice status, route info) - the page exposes only 13 of them.
- Shared static `SeedDataDAL`.
- SP bodies missing (GAP-M17-001).

## 31. Change Impact
Consumers: ~10 pages call `SeedData.aspx/GetGroupList` etc. `SeedDataDAL` is also used directly by many code-behinds (AppMonitoringList, DWSPMonthlyRpt, TargetExcelUploadList).

