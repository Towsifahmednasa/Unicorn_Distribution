---
page_id: PAGE-227
module: M06
page: OrderMonitor
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-227 — OrderMonitor

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-227 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderMonitor |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_OrderMonitor |
| aspx size | 18028 bytes |
| Code-behind LOC | 764 |
| Inline script size | 1341 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderMonitor** (Page). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `businesssummaryviewer.aspx`

## 5. Route
`~/SInventory_UI/OrderMonitor.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OrderMonitor.aspx |
| Code-behind | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | TotalSummaryBLL (Library.BLL/SInventory_BLL/TotalSummaryBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | TotalSummaryDAL (Library.DAL/SInventory_DAL/TotalSummaryDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox | Select   From Date | no validator found |  |  |  |  |
| toDateTextBox | TextBox | Select  To Date | no validator found |  |  |  |  |
| viewRptButton | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPageIndexChanging→OnPageIndexChanging |
| (no id) | BoundField | Depot Name | no validator found |  |  |  |  |
| (no id) | BoundField | Number of order received | no validator found |  |  |  |  |
| (no id) | BoundField | Order Value | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Proforma/Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Proforma/Invoice Value | no validator found |  |  |  |  |
| (no id) | BoundField | Rejection Vaue | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Deleted order | no validator found |  |  |  |  |
| (no id) | BoundField | Order Deleted Value | no validator found |  |  |  |  |
| (no id) | BoundField | Number of pending Order | no validator found |  |  |  |  |
| (no id) | BoundField | Pending Value | no validator found |  |  |  |  |
| zoneDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→zoneDropDownList_SelectedIndexChanged |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→salesCenterDropDownList_SelectedIndexChanged |
| territoryDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | aDataTable | SInventory_UI_OrderMonitor.LoadInfo | TotalSummaryBLL.LoadSummaryzoneBranchwiseBLL, TotalSummaryBLL.LoadSummaryzonewiseBLL, TotalSummaryBLL.OrderMonitoring_Bi | TotalSummaryDAL.GetReportTable, TotalSummaryDAL.LoadSummaryzoneBranchwiseBLL, TotalSummaryDAL.LoadSummaryzonewiseDAL, To | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_OrderMonitor.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_OrderMonitor.excelButton1_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | headerTable | SInventory_UI_OrderMonitor.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| zoneDropDownList | DropDownlist → OrderInfoBLL.LoadZone | inline SQL | Region/RegionCode | tblRegion |
| salesCenterDropDownList | zoneDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadSCZoneWise | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblInvoice |
| territoryDropDownList | salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadTerritory | inline SQL | AreaName/AreaCode | tblArea, tblInvoice |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewRptButton | LinkButton click | (postback) | SInventory_UI_OrderMonitor.viewRptButton_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:105 | TotalSummaryBLL.LoadSummaryzoneBranchwiseBLL, TotalSummaryBLL.LoadSummaryzonewiseBLL, TotalSummaryBL | TotalSummaryDAL.GetReportTable, TotalSummaryDAL.LoadSummaryzoneBranchwiseBLL, TotalSummaryDAL.LoadSu | sp_OrderMonitoringPanel_Bizmotion | SP executed — effect on tables UNKNOWN (body not in repo) |
|  | LinkButton click | (postback) | SInventory_UI_OrderMonitor.Unnamed_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:761 | — | — | — | read-only / none detected |
| excelButton1 | Button click | (postback) | SInventory_UI_OrderMonitor.btnExportToExcel_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:684 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_OrderMonitor.btnExport_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:158 | — | — | — | read-only / none detected |

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
| SInventory_UI_OrderMonitor.Page_Load | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:19 | object sender, EventArgs e | SInventory_UI_OrderMonitor.Page_Load | no | no |
| SInventory_UI_OrderMonitor.loadGridView_OnRowCreated | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:29 | object sender, GridViewRowEventArgs e | SInventory_UI_OrderMonitor.loadGridView_OnRowCreated | no | no |
| SInventory_UI_OrderMonitor.DropDownlist | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:98 |  | SInventory_UI_OrderMonitor.DropDownlist → OrderInfoBLL.LoadZone → OrderInfoDAL.LoadZone | no | no |
| SInventory_UI_OrderMonitor.viewRptButton_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:105 | object sender, EventArgs e | SInventory_UI_OrderMonitor.viewRptButton_Click → TotalSummaryBLL.LoadSummaryzoneBranchwiseBLL / TotalSummaryBLL.LoadSummaryzonewiseBLL / TotalSummaryBLL.OrderMonitoring_Bizmo → TotalSummaryDAL.GetReportTable / TotalSummaryDAL.LoadSummaryzoneBranchwiseBLL / Tot | no | no |
| SInventory_UI_OrderMonitor.btnExport_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:158 | object sender, EventArgs e | SInventory_UI_OrderMonitor.btnExport_Click | no | no |
| SInventory_UI_OrderMonitor.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:295 | object sender, EventArgs e | SInventory_UI_OrderMonitor.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_OrderMonitor.LoadInfo | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:306 |  | SInventory_UI_OrderMonitor.LoadInfo → TotalSummaryBLL.LoadSummaryzoneBranchwiseBLL / TotalSummaryBLL.LoadSummaryzonewiseBLL / TotalSummaryBLL.OrderMonitoring_Bizmo → TotalSummaryDAL.GetReportTable / TotalSummaryDAL.LoadSummaryzoneBranchwiseBLL / TotalSummaryDA | yes | no |
| SInventory_UI_OrderMonitor.excelButton1_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:641 | object sender, EventArgs e | SInventory_UI_OrderMonitor.excelButton1_Click | no | no |
| SInventory_UI_OrderMonitor.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:662 | object sender, EventArgs e | SInventory_UI_OrderMonitor.rptTypeDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_OrderMonitor.btnExportToExcel_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:684 | object sender, EventArgs e | SInventory_UI_OrderMonitor.btnExportToExcel_Click | no | no |
| SInventory_UI_OrderMonitor.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:750 | object sender, EventArgs e | SInventory_UI_OrderMonitor.salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadTerritory → OrderInfoDAL.LoadTerritory | no | no |
| SInventory_UI_OrderMonitor.zoneDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:755 | object sender, EventArgs e | SInventory_UI_OrderMonitor.zoneDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadSCZoneWise → OrderInfoDAL.LoadSCZoneWise | no | no |
| SInventory_UI_OrderMonitor.Unnamed_Click | Solution.Web/SInventory_UI/OrderMonitor.aspx.cs:761 | object sender, EventArgs e | SInventory_UI_OrderMonitor.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadZone | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:338 |
| TotalSummaryBLL.OrderMonitoring_Bizmo | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:36 |
| TotalSummaryBLL.LoadSummaryzonewiseBLL | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:40 |
| TotalSummaryBLL.LoadSummaryzoneBranchwiseBLL | Library.BLL/SInventory_BLL/TotalSummaryBLL.cs:44 |
| OrderInfoBLL.LoadTerritory | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:334 |
| OrderInfoBLL.LoadSCZoneWise | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:342 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadZone | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2613 | inline SQL: tblRegion |  |
| TotalSummaryDAL.OrderMonitoring_Bizmo | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:97 | sp_OrderMonitoringPanel_Bizmotion | @fromdate, @todate |
| TotalSummaryDAL.LoadSummaryzonewiseDAL | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:1044 | inline SQL: tblInvoice, tblInvoiceDetail, tblProduct, tblSubInvoiceDetail |  |
| TotalSummaryDAL.GetReportTable | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:19 | delegates / no SQL literal | @Branch, @FromDate, @ToDate, @Zone |
| TotalSummaryDAL.LoadSummaryzoneBranchwiseBLL | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs:1193 | inline SQL: tblInvoice, tblInvoiceDetail, tblProduct, tblSubInvoiceDetail |  |
| OrderInfoDAL.LoadTerritory | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2591 | inline SQL: tblArea, tblInvoice | @ComUnitId |
| OrderInfoDAL.LoadSCZoneWise | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2629 | inline SQL: tblCompanyUnit, tblInvoice | @RegionCode |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_OrderMonitoringPanel_Bizmotion | TotalSummaryDAL.OrderMonitoring_Bizmo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyUnit, tblInvoice, tblInvoiceDetail, tblProduct, tblRegion, tblSubInvoiceDetail, tblSubInvoiceMaster
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
- try/catch present in 1 of 16 methods.
- Redirect targets: OrderMonitor.aspx
- User messages: Please Select Date Range!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL, TotalSummaryBLL; DAL: OrderInfoDAL, TotalSummaryDAL; DAO: —; Tables (inline): tblArea, tblCompanyUnit, tblInvoice, tblInvoiceDetail, tblProduct, tblRegion, tblSubInvoiceDetail, tblSubInvoiceMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewRptButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewRptButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click excelButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OrderMonitor.aspx`, `Solution.Web/SInventory_UI/OrderMonitor.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/OrderMonitor.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 28 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/OrderMonitor.aspx |
| Data access | This document §15–17 | 1 SP names, 8 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadSCZoneWise, OrderInfoDAL.LoadTerritory, OrderInfoDAL.LoadZone, TotalSummaryDAL.GetReportTable, TotalSummaryDAL.LoadSummaryzoneBranchwiseBLL affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
