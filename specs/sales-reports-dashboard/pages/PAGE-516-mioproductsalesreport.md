---
page_id: PAGE-516
module: M13
page: MIOProductSalesReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-516 — MIOProductSalesReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-516 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | MIOProductSalesReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_AllSalesReport |
| aspx size | 15938 bytes |
| Code-behind LOC | 145 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MIOProductSalesReport** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `mioproductsalesreportviewer.aspx`

## 5. Route
`~/SInventory_UI/MIOProductSalesReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| rptTypeDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→rptTypeDropDownList_SelectedIndexChanged |
| CheckBox1 | CheckBox |  | no validator found |  |  |  | oncheckedchanged→CheckBox1_CheckedChanged1 |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→dcDropDownList1_SelectedIndexChanged |
| areacodeTextBox | TextBox |  | no validator found |  |  |  |  |
| productCodeTextBox | TextBox |  | no validator found |  |  |  |  |
| districtTextBox | TextBox |  | no validator found |  |  |  |  |
| regionTextBox | TextBox |  | no validator found |  |  |  |  |
| InvoiceDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDatse | ImageButton |  | no validator found |  |  |  |  |
| todateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDatsea | ImageButton |  | no validator found |  |  |  |  |
| SearchButton | Button | View Report | no validator found |  |  |  | onclick→SearchButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| areacodeTextBox | parameter | SInventory_UI_AllSalesReport.ReportParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | parameter | SInventory_UI_AllSalesReport.ReportParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| districtTextBox | parameter | SInventory_UI_AllSalesReport.ReportParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| regionTextBox | parameter | SInventory_UI_AllSalesReport.ReportParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_AllSalesReport.SearchButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_AllSalesReport.SearchButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| productCodeTextBox | product | SInventory_UI_AllSalesReport.SearchButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| rptTypeDropDownList | url | SInventory_UI_AllSalesReport.SearchButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rptTypeDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_AllSalesReport.Page_Load | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:12 | object sender, EventArgs e | SInventory_UI_AllSalesReport.Page_Load → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_AllSalesReport.LoadDropDown | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:26 |  | SInventory_UI_AllSalesReport.LoadDropDown → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_AllSalesReport.SearchButton_Click | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:69 | object sender, EventArgs e | SInventory_UI_AllSalesReport.SearchButton_Click | no | no |
| SInventory_UI_AllSalesReport.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:84 | object sender, EventArgs e | SInventory_UI_AllSalesReport.dcDropDownList1_SelectedIndexChanged | no | no |
| SInventory_UI_AllSalesReport.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:88 | object sender, EventArgs e | SInventory_UI_AllSalesReport.CheckBox1_CheckedChanged | no | no |
| SInventory_UI_AllSalesReport.CheckBox1_CheckedChanged1 | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:92 | object sender, EventArgs e | SInventory_UI_AllSalesReport.CheckBox1_CheckedChanged1 | no | no |
| SInventory_UI_AllSalesReport.rptTypeDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs:103 | object sender, EventArgs e | SInventory_UI_AllSalesReport.rptTypeDropDownList_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: RptParam
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL; DAL: OtherStockActionDAL; DAO: —; Tables (inline): tblCompanyUnit, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MIOProductSalesReport.aspx`, `Solution.Web/SInventory_UI/MIOProductSalesReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 20 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MIOProductSalesReport.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M13 (Sales Reports, Analytics & Dashboards).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OtherStockActionDAL.DCLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
