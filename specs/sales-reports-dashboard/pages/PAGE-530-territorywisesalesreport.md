---
page_id: PAGE-530
module: M13
page: TerritoryWiseSalesReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-530 — TerritoryWiseSalesReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-530 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | TerritoryWiseSalesReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_TerritoryWiseSalesReport |
| aspx size | 24261 bytes |
| Code-behind LOC | 675 |
| Inline script size | 1422 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TerritoryWiseSalesReport** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `dzsmtotalsummary.aspx`

## 5. Route
`~/SInventory_UI/TerritoryWiseSalesReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/Reports_UI/IVMarketStructureMarket.ascx |
| JS include | ../VerticalAsset/jquery.tabletoCSV.js |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | DoctorInfoReportDal (Library.DAL/DoctorInfo_DAL/DoctorInfoReportDal.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| hfRegionId | HiddenField |  | no validator found |  |  |  |  |
| hfGroupID | HiddenField |  | no validator found |  |  |  |  |
| hfMarket | HiddenField |  | no validator found |  |  |  |  |
| hfSubTeritory | HiddenField |  | no validator found |  |  |  |  |
| hfTeritory | HiddenField |  | no validator found |  |  |  |  |
| hfArea | HiddenField |  | no validator found |  |  |  |  |
| hfZone | HiddenField |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox | Select  From Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| lblInfo | Label |  | no validator found |  |  |  |  |
| lblNextDate | Label |  | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| LinkButton2 | LinkButton |  | no validator found |  | Visible=false |  | OnClick→excelButton1_Click |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Name | no validator found |  |  |  |  |
| (no id) | BoundField | Number of  Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Invoice Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Sales Confirmation Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Sales Confirmation Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Confirmation Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net Return Amount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Return Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Confirmation Chemist Coverage | no validator found |  |  |  |  |
| (no id) | BoundField | FCB Sales Confirmation(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | NCOD Sales Confirmation  (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Sales Confirmation (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | General Sales Confirmation  (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | NCOD Proforma (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | FCB Proforma (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Proforma (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | General Sales (TP) (Invoice) | no validator found |  |  |  |  |
| (no id) | BoundField | Collection Chemist Coverage | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Undelivered Invoice  | no validator found |  |  |  |  |
| (no id) | BoundField | Sum of Net UndeliveredAmount(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Undelivered Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | BSP Net Invoice Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | GSP Net Invoice Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | BSP Net Sales Confirmation Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | GSP Net Sales Confirmation (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Sales confiramation BSP Coverage | no validator found |  |  |  |  |
| (no id) | BoundField | Sales confiramation GSP Coverage | no validator found |  |  |  |  |
| (no id) | BoundField | Collection BSP Coverage  | no validator found |  |  |  |  |
| (no id) | BoundField | Collection GSP Coverage  | no validator found |  |  |  |  |
| (no id) | BoundField | Net Sales (Total Sales - Return) | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Collection (TP+VAT) | no validator found |  |  |  |  |
| (no id) | BoundField | Collection TP | no validator found |  |  |  |  |
| (no id) | BoundField | Collection VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Total OutStanding | no validator found |  |  |  |  |
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| dzsmDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→excelButton1_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | aDataTable | SInventory_UI_TerritoryWiseSalesReport.LoadInfo | — | DoctorInfoReportDal.GetTerritoryWiseLastProessDate, TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | aDataTable | SInventory_UI_TerritoryWiseSalesReport.excelButton1_Click | TotalSummaryBLL.DZSMwiseLoadSummaryBLL | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rptTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| dzsmDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| salesCenterDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton click | (postback) | SInventory_UI_TerritoryWiseSalesReport.viewRptButton_Click | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:96 | — | DoctorInfoReportDal.GetTerritoryWiseLastProessDate, TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | sp_GET_DZSMProcessDate, sp_GET_DZSMwiseReportParam_ByProcess_Area, sp_GET_DZSMwiseReportParam_ByProcess_Terri, sp_GET_DZ | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | SInventory_UI_TerritoryWiseSalesReport.OnClick | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:672 | — | — | — | read-only / none detected |
| LinkButton2 | LinkButton click | (postback) | SInventory_UI_TerritoryWiseSalesReport.excelButton1_Click | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:535 | TotalSummaryBLL.DZSMwiseLoadSummaryBLL | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | sp_GET_DZSMwiseNAtionalReport, sp_GET_DZSMwiseReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| excelButton1 | Button click | (postback) | SInventory_UI_TerritoryWiseSalesReport.excelButton1_Click | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:535 | TotalSummaryBLL.DZSMwiseLoadSummaryBLL | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | sp_GET_DZSMwiseNAtionalReport, sp_GET_DZSMwiseReport | SP executed — effect on tables UNKNOWN (body not in repo) |

## 11. Frontend Flow
- Client script functions in page: exporttocsv, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_TerritoryWiseSalesReport.Page_Load | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:35 | object sender, EventArgs e | SInventory_UI_TerritoryWiseSalesReport.Page_Load → DoctorInfoReportDal.GetTerritoryWiseLastProessDate → SP: sp_GET_DZSMProcessDate | yes | no |
| SInventory_UI_TerritoryWiseSalesReport.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:91 | object sender, GridViewPageEventArgs e | SInventory_UI_TerritoryWiseSalesReport.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_TerritoryWiseSalesReport.viewRptButton_Click | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:96 | object sender, EventArgs e | SInventory_UI_TerritoryWiseSalesReport.viewRptButton_Click → DoctorInfoReportDal.GetTerritoryWiseLastProessDate / TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv → SP: sp_GET_DZSMProcessDate, sp_GET_DZSMwiseReportParam_ByProcess_Area, sp_GET_DZSMwiseReportParam_B | yes | no |
| SInventory_UI_TerritoryWiseSalesReport.LoadInfo | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:179 |  | SInventory_UI_TerritoryWiseSalesReport.LoadInfo → DoctorInfoReportDal.GetTerritoryWiseLastProessDate / TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv → SP: sp_GET_DZSMProcessDate, sp_GET_DZSMwiseReportParam_ByProcess_Area, sp_GET_DZSMwiseReportParam_ByProcess_Te | yes | no |
| SInventory_UI_TerritoryWiseSalesReport.excelButton1_Click | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:535 | object sender, EventArgs e | SInventory_UI_TerritoryWiseSalesReport.excelButton1_Click → TotalSummaryBLL.DZSMwiseLoadSummaryBLL → TotalSummaryDAL.DZSMwiseLoadSummaryBLL → SP: sp_GET_DZSMwiseNAtionalReport, sp_GET_DZSMwiseReport | no | no |
| SInventory_UI_TerritoryWiseSalesReport.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs:654 | object sender, EventArgs e | SInventory_UI_TerritoryWiseSalesReport.rptTypeDropDownList_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| TotalSummaryBLL.DZSMwiseLoadSummaryBLL | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:190 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DoctorInfoReportDal.GetTerritoryWiseLastProessDate | Library.DAL/DoctorInfo_DAL/DoctorInfoReportDal.cs:67 | sp_GET_DZSMProcessDate | @id |
| TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:800 | sp_GET_DZSMwiseReportParam_ByProcess_Area, sp_GET_DZSMwiseReportParam_ByProcess_ | @AreaSelect, @TeritorySelect, @ZoneSelect, @fromdate, @todate |
| TotalSummaryDAL.DZSMwiseLoadSummaryBLL | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:715 | sp_GET_DZSMwiseReport | @Dzsm, @fromdate, @todate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_DZSMProcessDate | DoctorInfoReportDal.GetTerritoryWiseLastProessDate | body NOT in repository | write-like name |
| sp_GET_DZSMwiseNAtionalReport | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DZSMwiseReport | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_DZSMwiseReportParam_ByProcess_Area | TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | body NOT in repository | write-like name |
| sp_GET_DZSMwiseReportParam_ByProcess_Terri | TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | body NOT in repository | write-like name |
| sp_GET_DZSMwiseReportParam_ByProcess_vv | TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validatea) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Area! | UNKNOWN |
| (handler Validatea) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Zone! | UNKNOWN |
| (handler LoadInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Mandatory Field!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: EmpInfoId, RoleTypeId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 3 of 12 methods.
- Redirect targets: DZSMTotalSummary.aspx
- User messages: Please Select Area!; Please Select Mandatory Field!!; Please Select Zone!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: TotalSummaryBLL; DAL: DoctorInfoReportDal, TotalSummaryDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click LinkButton2 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click LinkButton2 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx`, `Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 72 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/TerritoryWiseSalesReport.aspx |
| Data access | This document §15–17 | 6 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 6 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M13 (Sales Reports, Analytics & Dashboards).
- Other pages sharing this page's stored procedures: PAGE-506
- Changing DAL methods DoctorInfoReportDal.GetTerritoryWiseLastProessDate, TotalSummaryDAL.DZSMwiseLoadSummaryBLL, TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
