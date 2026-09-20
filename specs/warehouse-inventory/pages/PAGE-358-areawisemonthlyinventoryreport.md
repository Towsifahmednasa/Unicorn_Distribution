---
page_id: PAGE-358
module: M10
page: AreaWiseMonthlyInventoryReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-358 — AreaWiseMonthlyInventoryReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-358 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | AreaWiseMonthlyInventoryReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_AreaWiseMonthlyInventoryReport |
| aspx size | 8641 bytes |
| Code-behind LOC | 96 |
| Inline script size | 803 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **AreaWiseMonthlyInventoryReport** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Warehouse/DC stock in/out, adjustments, freezes, opening balances, sample stock, material conversion, stock valuation and monitoring.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `areawisemonthlysalesreportviewer.aspx`

## 5. Route
`~/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| CheckBox1 | CheckBox |  | no validator found |  |  |  | oncheckedchanged→CheckBox1_CheckedChanged |
| branchDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| viewRptButton | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| marketDropDownList | DropDownList |  | no validator found |  | Visible=false | see §9 |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | inputDateTime | SInventory_UI_AreaWiseMonthlyInventoryReport.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | fromDate | SInventory_UI_AreaWiseMonthlyInventoryReport.ReportPopUp | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_AreaWiseMonthlyInventoryReport.ReportPopUp | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| branchDropDownList | branchId | SInventory_UI_AreaWiseMonthlyInventoryReport.ReportPopUp | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| CheckBox1 | national | SInventory_UI_AreaWiseMonthlyInventoryReport.ReportPopUp | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| branchDropDownList | LoadDropdownList → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| marketDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewRptButton | LinkButton click | (postback) | SInventory_UI_AreaWiseMonthlyInventoryReport.viewRptButton_Click | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:81 | — | — | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_AreaWiseMonthlyInventoryReport.cancelButton_Click | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:35 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_AreaWiseMonthlyInventoryReport.Page_Load | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_AreaWiseMonthlyInventoryReport.Page_Load → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_AreaWiseMonthlyInventoryReport.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:26 | object sender, EventArgs e | SInventory_UI_AreaWiseMonthlyInventoryReport.fromDateTextBox_TextChanged | no | no |
| SInventory_UI_AreaWiseMonthlyInventoryReport.cancelButton_Click | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:35 | object sender, EventArgs e | SInventory_UI_AreaWiseMonthlyInventoryReport.cancelButton_Click | no | no |
| SInventory_UI_AreaWiseMonthlyInventoryReport.LoadDropdownList | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:39 |  | SInventory_UI_AreaWiseMonthlyInventoryReport.LoadDropdownList → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_AreaWiseMonthlyInventoryReport.viewRptButton_Click | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:81 | object sender, EventArgs e | SInventory_UI_AreaWiseMonthlyInventoryReport.viewRptButton_Click | no | no |
| SInventory_UI_AreaWiseMonthlyInventoryReport.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs:85 | object sender, EventArgs e | SInventory_UI_AreaWiseMonthlyInventoryReport.CheckBox1_CheckedChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler ReportPopUp) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select adate range!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 8 methods.
- Redirect targets: AreaWiseMonthlyInventoryReport.aspx
- User messages: Please Select adate range!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL; DAL: OrderInfoDAL; DAO: —; Tables (inline): tblCompanyUnit, tblUserCompanyUnit

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

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx`, `Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 13 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/AreaWiseMonthlyInventoryReport.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M10 (Warehouse, DC Stock & Inventory).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
