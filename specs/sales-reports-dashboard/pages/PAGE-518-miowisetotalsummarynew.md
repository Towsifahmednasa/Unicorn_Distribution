---
page_id: PAGE-518
module: M13
page: MIOWiseTotalSummaryNew
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-518 — MIOWiseTotalSummaryNew

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-518 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | MIOWiseTotalSummaryNew |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_MIOWiseTotalSummaryNew |
| aspx size | 21782 bytes |
| Code-behind LOC | 333 |
| Inline script size | 920 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MIOWiseTotalSummaryNew** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/MIOWiseTotalSummaryNew_T.aspx`

Outbound references: `businesssummaryviewer.aspx`

## 5. Route
`~/SInventory_UI/MIOWiseTotalSummaryNew.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx |
| Code-behind | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | MIOTotalSummaryBLL (Library.BLL/SInventory_BLL/MIOTotalSummaryBLL.cs) |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | MIOTotalSummaryDAL (Library.DAL/SInventory_DAL/MIOTotalSummaryDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox | Select  From Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select To Date | no validator found |  |  |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| LinkButton2 | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Name | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | No of Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Invoice Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Returned Invoices | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Return Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Sales Amt | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Collection | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Invoices Sold | no validator found |  |  |  |  |
| (no id) | BoundField | Amount (TP) | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Outstanding Amt. | no validator found |  |  |  |  |
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| zoneDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| territoryDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| viewRptButton | Button | Search | no validator found |  |  |  | OnClick→viewRptButton_Click |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | aDataTable | SInventory_UI_MIOWiseTotalSummaryNew.LoadInfo | MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash | MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_MIOWiseTotalSummaryNew.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_MIOWiseTotalSummaryNew.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | headerTable | SInventory_UI_MIOWiseTotalSummaryNew.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| rptTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| zoneDropDownList | DropDownlist → OrderInfoBLL.LoadZone | inline SQL | Region/RegionCode | tblRegion |
| territoryDropDownList | DropDownlist → OrderInfoBLL.LoadTerritory | inline SQL | AreaName/AreaCode | tblArea, tblInvoice |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton click | (postback) | SInventory_UI_MIOWiseTotalSummaryNew.viewRptButton_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:103 | MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash | MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash | sp_MiowiseMISReport | SP executed — effect on tables UNKNOWN (body not in repo) |
|  | LinkButton click | (postback) | SInventory_UI_MIOWiseTotalSummaryNew.Unnamed_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:330 | — | — | — | read-only / none detected |
| LinkButton2 | LinkButton click | (postback) | SInventory_UI_MIOWiseTotalSummaryNew.btnExportToExcel_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:258 | — | — | — | read-only / none detected |
| viewRptButton | Button click | (postback) | SInventory_UI_MIOWiseTotalSummaryNew.viewRptButton_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:103 | MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash | MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash | sp_MiowiseMISReport | SP executed — effect on tables UNKNOWN (body not in repo) |
| excelButton1 | Button click | (postback) | SInventory_UI_MIOWiseTotalSummaryNew.btnExportToExcel_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:258 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_MIOWiseTotalSummaryNew.Page_Load | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.Page_Load → OrderInfoBLL.LoadSC / OrderInfoBLL.LoadTerritory / OrderInfoBLL.LoadZone → OrderInfoDAL.LoadSC / OrderInfoDAL.LoadTerritory / OrderInfoDAL.LoadZone | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.loadGridView_OnRowCreated | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:26 | object sender, GridViewRowEventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.loadGridView_OnRowCreated | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.DropDownlist | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:95 |  | SInventory_UI_MIOWiseTotalSummaryNew.DropDownlist → OrderInfoBLL.LoadSC / OrderInfoBLL.LoadTerritory / OrderInfoBLL.LoadZone → OrderInfoDAL.LoadSC / OrderInfoDAL.LoadTerritory / OrderInfoDAL.LoadZone | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.viewRptButton_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:103 | object sender, EventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.viewRptButton_Click → MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash → MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash → SP: sp_MiowiseMISReport | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.LoadInfo | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:108 |  | SInventory_UI_MIOWiseTotalSummaryNew.LoadInfo → MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash → MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash → SP: sp_MiowiseMISReport | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.excelButton1_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:214 | object sender, EventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.excelButton1_Click | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:236 | object sender, EventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.rptTypeDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.btnExportToExcel_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:258 | object sender, EventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.btnExportToExcel_Click | no | no |
| SInventory_UI_MIOWiseTotalSummaryNew.Unnamed_Click | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs:330 | object sender, EventArgs e | SInventory_UI_MIOWiseTotalSummaryNew.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadTerritory | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:334 |
| OrderInfoBLL.LoadZone | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:338 |
| MIOTotalSummaryBLL.LoadSummaryProductcodewiseGyash | Library.BLL/SInventory_BLL/MIOTotalSummaryBLL.cs:47 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadTerritory | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2591 | inline SQL: tblArea, tblInvoice | @ComUnitId |
| OrderInfoDAL.LoadZone | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2613 | inline SQL: tblRegion |  |
| MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash | Library.DAL/SInventory_DAL/MIOTotalSummaryDAL.cs:141 | sp_MiowiseMISReport | @fromDate, @toDate, @unitid |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_MiowiseMISReport | MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyUnit, tblInvoice, tblRegion, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |
| (handler excelButton1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Date Range!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 12 methods.
- Redirect targets: MIOWiseTotalSummaryNew.aspx
- User messages: Please Select Date Range!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: MIOTotalSummaryBLL, OrderInfoBLL; DAL: MIOTotalSummaryDAL, OrderInfoDAL; DAO: —; Tables (inline): tblArea, tblCompanyUnit, tblInvoice, tblRegion, tblUserCompanyUnit

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
| TC-09 | Click viewRptButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click viewRptButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx`, `Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 45 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MIOWiseTotalSummaryNew.aspx |
| Data access | This document §15–17 | 1 SP names, 5 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M13 (Sales Reports, Analytics & Dashboards).
- Other pages sharing this page's stored procedures: PAGE-519
- Changing DAL methods MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash, OrderInfoDAL.LoadSC, OrderInfoDAL.LoadTerritory, OrderInfoDAL.LoadZone affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
