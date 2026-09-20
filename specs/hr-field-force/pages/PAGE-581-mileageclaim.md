---
page_id: PAGE-581
module: M15
page: MileageClaim
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "PARTIAL", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-581 — MileageClaim

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-581 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | MileageClaim |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_MileageClaim |
| aspx size | 34655 bytes |
| Code-behind LOC | 43 |
| Inline script size | 18583 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MileageClaim** (Page). Page title/heading text: `Market Structure`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/MileageClaimView.aspx`, `Reports_UI/CVRDoctoriseMonthlypt.aspx`, `Reports_UI/DWSPMonthlyRpt.aspx`, `Reports_UI/DcpDoctoriseMonthlypt.aspx`, `Reports_UI/DcrDoctoriseMonthlypt.aspx`, `Reports_UI/DoctorInfoReport.aspx`, `Reports_UI/EmpMonthlyExpenseRpt.aspx`, `Reports_UI/EmpMonthlyExpenseRptMultiple.aspx`, `Reports_UI/OrderPermission.aspx`, `Reports_UI/RXDoctoriseMonthlypt.aspx`, `Reports_UI/TourPlanSummaryReport.aspx`

Outbound references: `mileageclaimview.aspx`

## 5. Route
`~/DoctorModule_UI/MileageClaim.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/MileageClaim.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | AppPrimaryDAL (Library.DAL/DoctorModule_DAL/AppPrimaryDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAO/entity class | ImagePath (Library.DAO/DoctorModule_DAO/ImagePath.cs) |
| DAO/entity class | MileageClaimDAO (Library.DAO/DoctorModule_DAO/MileageClaimDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| txtMileage | input |  | no validator found |  |  |  |  |
| EntryBySelect | select |  | no validator found |  |  |  |  |
| txtNote | textarea |  | no validator found |  |  |  |  |
| ddlTourType | select |  | no validator found |  |  |  |  |
| txtClaimDate | input | Select Date | no validator found |  |  |  |  |
| ddlTransport | select |  | no validator found |  |  |  |  |
| GroupSelect | select |  | no validator found |  |  |  |  |
| ZoneSelect | select |  | no validator found |  |  |  |  |
| AreaSelect | select |  | no validator found |  |  |  |  |
| TeritorySelect | select |  | no validator found |  |  |  |  |
| SubterritorySelect | select |  | no validator found |  |  |  |  |
| MarketSelect | select |  | no validator found |  |  |  |  |
| txtMeterReadding | input |  | no validator found |  |  |  |  |
| imageUploadForm | input |  | no validator found |  |  |  | onchange→ImageToBase64(this) |
| btnSave | button |  | no validator found |  |  |  | onclick→Save() |
| masterId | input | 0 | no validator found |  |  |  |  |
| imgeBase64Str | input |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| EntryBySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlTourType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlTransport | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| GroupSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ZoneSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| AreaSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| TeritorySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| SubterritorySelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| MarketSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: FinalSave, GetArea_All_ByZone, GetArea_ByZone, GetData, GetEmpList, GetEmpList_All, GetGroupAllInfo, GetGroupInfo, GetMarket_BySubTerritoryIdNew, GetMarket_BySubTerritoryIdNew_All, GetMarket_BySubTerritoryId_All, GetMarket_ByTerritoryId, GetMarket_ByTerritoryId_All, GetSubTerritory_ByTerritoryId, GetSubTerritory_ByTerritoryId_All_new, GetTerritory_ByAreaId, GetTerritory_ByAreaId_All, GetThana_ET, GetTourTypeList, GetTourTypeList_all, GetTransportList, GetTransportList_All, GetZone, GetZone_All, ImageToBase64, RemoveValidationTooltip, ResetClick, Save, Validation, ValidationTooltip
- jQuery/DOM bindings: #AreaSelect:change, #GroupSelect:change, #SubterritorySelect:change, #TeritorySelect:change, #ZoneSelect:change, #txtMeterReadding:keypress, #txtMileage:keypress
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: yes; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| DoctorModule_UI_MileageClaim.Page_Load | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:20 | object sender, EventArgs e | DoctorModule_UI_MileageClaim.Page_Load | no | no |
| DoctorModule_UI_MileageClaim.GetThana_WitTagDetails_forEditPage | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:25 | int id | DoctorModule_UI_MileageClaim.GetThana_WitTagDetails_forEditPage → Setup2DAL.GetThana_All_WithTagInfo_ForEditPage → SP: sp_Get_Thana_WithTagInfo_TerritoryEdit | no | no |
| DoctorModule_UI_MileageClaim.GetMileageClaimEditData | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:33 | int id | DoctorModule_UI_MileageClaim.GetMileageClaimEditData → Setup2DAL.GetMileageClaimEditData → SP: sp_Get_MileageClaim_ById | no | no |
| DoctorModule_UI_MileageClaim.Save_MileageClaim | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:40 | MileageClaimDAO degree | DoctorModule_UI_MileageClaim.Save_MileageClaim → AppPrimaryDAL.GetImagePath / Setup2DAL.Save_MileageClaim → SP: sp_Save_MileageClaim, sp_Update_MileageClaim, sp_Webapi_Get_ImagePath | no | yes |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
| [WebMethod] / static page method | Source | Parameters | Endpoint (PageMethod URL) | Direct data access |
|---|---|---|---|---|
| DoctorModule_UI_MileageClaim.GetThana_WitTagDetails_forEditPage | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:25 | int id | ~/DoctorModule_UI/MileageClaim.aspx/GetThana_WitTagDetails_forEditPage | via BLL |
| DoctorModule_UI_MileageClaim.GetMileageClaimEditData | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:33 | int id | ~/DoctorModule_UI/MileageClaim.aspx/GetMileageClaimEditData | via BLL |
| DoctorModule_UI_MileageClaim.Save_MileageClaim | Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs:40 | MileageClaimDAO degree | ~/DoctorModule_UI/MileageClaim.aspx/Save_MileageClaim | via BLL |

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:2917 | sp_Get_Thana_WithTagInfo_TerritoryEdit | @id |
| Setup2DAL.GetMileageClaimEditData | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:1025 | sp_Get_MileageClaim_ById | @id |
| Setup2DAL.Save_MileageClaim | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:1585 | sp_Save_MileageClaim, sp_Update_MileageClaim | @AreaId, @EmpInfoId, @EntryBy, @GroupId, @MarketId, @MeterReading, @MileageDate, @RegionId, @SubTerr |
| AppPrimaryDAL.GetImagePath | Library.DAL/DoctorModule_DAL/AppPrimaryDAL.cs:58 | sp_Webapi_Get_ImagePath | @type |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_MileageClaim_ById | Setup2DAL.GetMileageClaimEditData | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Thana_WithTagInfo_TerritoryEdit | Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | body NOT in repository | read-like name (INFERRED) |
| sp_Save_MileageClaim | Setup2DAL.Save_MileageClaim | body NOT in repository | write-like name |
| sp_Update_MileageClaim | Setup2DAL.Save_MileageClaim | body NOT in repository | write-like name |
| sp_Webapi_Get_ImagePath | AppPrimaryDAL.GetImagePath | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 4 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
sp_Webapi_Get_ImagePath

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: AppPrimaryDAL, Setup2DAL; DAO: ImagePath, MileageClaimDAO, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/MileageClaim.aspx`, `Solution.Web/DoctorModule_UI/MileageClaim.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/MileageClaim.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 22 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/MileageClaim.aspx |
| Data access | This document §15–17 | 5 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 5 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-570, PAGE-621, PAGE-653, PAGE-679
- Changing DAL methods AppPrimaryDAL.GetImagePath, Setup2DAL.GetMileageClaimEditData, Setup2DAL.GetThana_All_WithTagInfo_ForEditPage, Setup2DAL.Save_MileageClaim affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
