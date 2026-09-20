---
page_id: PAGE-364
module: M10
page: DCStockReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-364 — DCStockReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-364 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | DCStockReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DCStockReport |
| aspx size | 11382 bytes |
| Code-behind LOC | 301 |
| Inline script size | 1180 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DCStockReport** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `dcstockreportviewer.aspx`, `login.aspx`

## 5. Route
`~/SInventory_UI/DCStockReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DCStockReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/DCStockReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DCStockReportBLL (Library.BLL/SInventory_BLL/DCStockReportBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | DCStockReportDAL (Library.DAL/SInventory_DAL/DCStockReportDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| nationalCheckBox | CheckBox |  National Stock Report  | no validator found |  |  |  | oncheckedchanged→nationalCheckBox_CheckedChanged |
| centalWHCheckBox | CheckBox |  | no validator found |  | Enabled=False |  | OnCheckedChanged→centalWHCheckBox_CheckedChanged |
| whDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→whDropDownList_SelectedIndexChanged |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→dcDropDownList1_SelectedIndexChanged |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→reportButton_OnClick |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Available Qty | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList1 | ComUnitId | SInventory_UI_DCStockReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | comUnitDetailDataTable | SInventory_UI_DCStockReport.reportButton_OnClick | — | CmnCrystaltoView.GetDCReportListDAL | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | param | SInventory_UI_DCStockReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| whDropDownList | centalWHCheckBox_CheckedChanged → RequisitionBLL.WareHouseLoad | inline SQL | WearhouseName/WearhouseId | tblWearhouse |
| dcDropDownList1 | Page_Load → DCStockReportBLL.LoadCompanyUnit | inline SQL | ComInfo/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_DCStockReport.reportButton_OnClick | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:146 | — | CmnCrystaltoView.GetDCReportListDAL | sp_Get_DCStockReportListNew | SP executed — effect on tables UNKNOWN (body not in repo) |
|  | LinkButton click | (postback) | SInventory_UI_DCStockReport.cancelButton_Click | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:70 | — | — | — | read-only / none detected |
| LinkButton1 | LinkButton click | (postback) | SInventory_UI_DCStockReport.viewRptButton_Click | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:77 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_DCStockReport.btnExport_Click | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:221 | — | — | — | read-only / none detected |

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
| SInventory_UI_DCStockReport.Page_Load | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_DCStockReport.Page_Load → DCStockReportBLL.LoadCompanyUnit → DCStockReportDAL.LoadCompanyUnit / DCStockReportDAL.SingleParameter | no | no |
| SInventory_UI_DCStockReport.centalWHCheckBox_CheckedChanged | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:49 | object sender, EventArgs e | SInventory_UI_DCStockReport.centalWHCheckBox_CheckedChanged → RequisitionBLL.WareHouseLoad → RequisitionDAL.WareHouseLoad | no | no |
| SInventory_UI_DCStockReport.whDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:67 | object sender, EventArgs e | SInventory_UI_DCStockReport.whDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_DCStockReport.cancelButton_Click | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:70 | object sender, EventArgs e | SInventory_UI_DCStockReport.cancelButton_Click | no | no |
| SInventory_UI_DCStockReport.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:74 | object sender, EventArgs e | SInventory_UI_DCStockReport.dcDropDownList1_SelectedIndexChanged | no | no |
| SInventory_UI_DCStockReport.viewRptButton_Click | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:77 | object sender, EventArgs e | SInventory_UI_DCStockReport.viewRptButton_Click | no | no |
| SInventory_UI_DCStockReport.nationalCheckBox_CheckedChanged | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:128 | object sender, EventArgs e | SInventory_UI_DCStockReport.nationalCheckBox_CheckedChanged | no | no |
| SInventory_UI_DCStockReport.reportButton_OnClick | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:146 | object sender, EventArgs e | SInventory_UI_DCStockReport.reportButton_OnClick → CmnCrystaltoView.GetDCReportListDAL → SP: sp_Get_DCStockReportListNew | no | no |
| SInventory_UI_DCStockReport.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:210 | object sender, EventArgs e | SInventory_UI_DCStockReport.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_DCStockReport.btnExport_Click | Solution.Web/SInventory_UI/DCStockReport.aspx.cs:221 | object sender, EventArgs e | SInventory_UI_DCStockReport.btnExport_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DCStockReportBLL.LoadCompanyUnit | Library.BLL/SInventory_BLL/DCStockReportBLL.cs:16 |
| RequisitionBLL.WareHouseLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:134 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DCStockReportDAL.LoadCompanyUnit | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:16 | inline SQL: tblCompanyUnit |  |
| DCStockReportDAL.SingleParameter | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:555 | delegates / no SQL literal |  |
| RequisitionDAL.WareHouseLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:25 | inline SQL: tblWearhouse |  |
| CmnCrystaltoView.GetDCReportListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:82 | sp_Get_DCStockReportListNew | @comunit |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_DCStockReportListNew | CmnCrystaltoView.GetDCReportListDAL | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit, tblWearhouse
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DC, ReportView
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 13 methods.
- Redirect targets: DCStockReport.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DCStockReportBLL, RequisitionBLL; DAL: CmnCrystaltoView, DCStockReportDAL, RequisitionDAL; DAO: —; Tables (inline): tblCompanyUnit, tblUserCompanyUnit, tblWearhouse

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DCStockReport.aspx`, `Solution.Web/SInventory_UI/DCStockReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DCStockReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 19 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DCStockReport.aspx |
| Data access | This document §15–17 | 1 SP names, 3 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CmnCrystaltoView.GetDCReportListDAL, DCStockReportDAL.LoadCompanyUnit, DCStockReportDAL.SingleParameter, RequisitionDAL.WareHouseLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
