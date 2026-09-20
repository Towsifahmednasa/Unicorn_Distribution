---
page_id: PAGE-213
module: M06
page: DeleteOrderReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-213 — DeleteOrderReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-213 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | DeleteOrderReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_DeleteOrderReport |
| aspx size | 12129 bytes |
| Code-behind LOC | 174 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DeleteOrderReport** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `deleteorderreportviewer.aspx`

## 5. Route
`~/SInventory_UI/DeleteOrderReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DeleteOrderReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | DeleteOrderReportBll (Library.BLL/SInventory_BLL/DeleteOrderReportBll.cs) |
| DAL class | DeleteOrderReportDal (Library.DAL/SInventory_DAL/DeleteOrderReportDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| MessageLabel | Label |  | no validator found |  |  |  |  |
| nationalCheckBox | CheckBox |  | no validator found |  |  |  | oncheckedchanged→nationalCheckBox_OnCheckedChanged |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→fromDateTextBox_OnTextChanged |
| imgDatse | ImageButton |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→toDateTextBox_OnTextChanged |
| imgDatsea | ImageButton |  | no validator found |  |  |  |  |
| viewRptButton | Button | View Report | no validator found |  |  |  | onclick→submitButton_Click1 |
| excelButton | Button | Excel | no validator found |  |  |  | OnClick→excelButton_OnClick |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| comUnitNameDropDownList | LoadDropdownlist → DeleteOrderReportBll.LoadSalesCenter | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| excelButton | Button click | (postback) | SInventory_UI_DeleteOrderReport.excelButton_OnClick | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:134 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_DeleteOrderReport.Page_Load | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:11 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.Page_Load → DeleteOrderReportBll.LoadSalesCenter → DeleteOrderReportDal.GetSalesCenter | no | no |
| SInventory_UI_DeleteOrderReport.LoadDropdownlist | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:19 |  | SInventory_UI_DeleteOrderReport.LoadDropdownlist → DeleteOrderReportBll.LoadSalesCenter → DeleteOrderReportDal.GetSalesCenter | no | no |
| SInventory_UI_DeleteOrderReport.cancelButton_OnClick | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:63 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.cancelButton_OnClick | no | no |
| SInventory_UI_DeleteOrderReport.submitButton_Click1 | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:83 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.submitButton_Click1 | no | no |
| SInventory_UI_DeleteOrderReport.nationalCheckBox_OnCheckedChanged | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:121 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.nationalCheckBox_OnCheckedChanged | no | no |
| SInventory_UI_DeleteOrderReport.excelButton_OnClick | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:134 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.excelButton_OnClick | no | no |
| SInventory_UI_DeleteOrderReport.fromDateTextBox_OnTextChanged | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:162 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.fromDateTextBox_OnTextChanged | no | no |
| SInventory_UI_DeleteOrderReport.toDateTextBox_OnTextChanged | Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs:168 | object sender, EventArgs e | SInventory_UI_DeleteOrderReport.toDateTextBox_OnTextChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DeleteOrderReportBll.LoadSalesCenter | Library.BLL/SInventory_BLL/DeleteOrderReportBll.cs:16 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DeleteOrderReportDal.GetSalesCenter | Library.DAL/SInventory_DAL/DeleteOrderReportDal.cs:17 | inline SQL: tblCompanyUnit |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler DateValidationCheck) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select valid date!!! | UNKNOWN |
| (handler submitButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select national report or a sales center!!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select from date | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select to date | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ComUnitCode, Excel, FromDate, ToDate
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 15 methods.
- Redirect targets: none
- User messages: Please select from date; Please select national report or a sales center!!!; Please select to date; Please select valid date!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: DeleteOrderReportBll; DAL: DeleteOrderReportDal; DAO: —; Tables (inline): tblCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click excelButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click excelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DeleteOrderReport.aspx`, `Solution.Web/SInventory_UI/DeleteOrderReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DeleteOrderReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 12 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DeleteOrderReport.aspx |
| Data access | This document §15–17 | 0 SP names, 1 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DeleteOrderReportDal.GetSalesCenter affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
