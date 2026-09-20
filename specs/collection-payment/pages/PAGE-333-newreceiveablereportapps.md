---
page_id: PAGE-333
module: M09
page: NewReceiveableReportApps
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-333 — NewReceiveableReportApps

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-333 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | NewReceiveableReportApps |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_NewReceiveableReportApps |
| aspx size | 45496 bytes |
| Code-behind LOC | 294 |
| Inline script size | 16064 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **NewReceiveableReportApps** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `intransitreportviewer.aspx`

## 5. Route
`~/SInventory_UI/NewReceiveableReportApps.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx |
| Code-behind | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs |
| JS include | ../VerticalAsset/js/pace.min.js |
| JS include | ../VerticalAsset/js/bootstrap.bundle.min.js |
| JS include | ../VerticalAsset/js/jquery.min.js |
| JS include | ../VerticalAsset/plugins/simplebar/js/simplebar.min.js |
| JS include | ../VerticalAsset/plugins/metismenu/js/metisMenu.min.js |
| JS include | ../VerticalAsset/plugins/perfect-scrollbar/js/perfect-scrollbar.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/legacy.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/picker.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/picker.time.js |
| JS include | ../VerticalAsset/plugins/datetimepicker/js/picker.date.js |
| CSS include | https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css |
| CSS include | ../VerticalAsset/plugins/simplebar/css/simplebar.css |
| CSS include | ../VerticalAsset/plugins/perfect-scrollbar/css/perfect-scrollbar.css |
| CSS include | ../VerticalAsset/plugins/metismenu/css/metisMenu.min.css |
| CSS include | ../VerticalAsset/css/pace.min.css |
| CSS include | ../VerticalAsset/plugins/datatable/css/dataTables.bootstrap5.min.css |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| InvoiceDateTextBox | TextBox |  | no validator found |  |  |  |  |
| todateTextBox | TextBox |  | no validator found |  |  |  |  |
| SearchButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| marketDropDownList | DropDownList |  | no validator found |  | Visible=false | see §9 |  |
| lblCount | Label | Total : 0 | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Center Code | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Mobile No. | no validator found |  |  |  |  |
| (no id) | BoundField | Order NO | no validator found |  |  |  |  |
| (no id) | BoundField | Order Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice NO | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | Number of Days In-Transit | no validator found |  |  |  |  |
| (no id) | BoundField | Net Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Paid Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Receivable Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Territory  | no validator found |  |  |  |  |
| (no id) | BoundField | AM Emp Code | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Emp Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Emp Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Emp Name | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| InvoiceDateTextBox | inputDateTime | SInventory_UI_NewReceiveableReportApps.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| id_mastetID | param | SInventory_UI_NewReceiveableReportApps.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_NewReceiveableReportApps.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| marketDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| SearchButton | LinkButton click | (postback) | SInventory_UI_NewReceiveableReportApps.SearchButton_Click | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:250 | — | InvoiceDAL.GetNewReceiveableDAl | sp_Get_NewReceiveableList | SP executed — effect on tables UNKNOWN (body not in repo) |

## 11. Frontend Flow
- Client script functions in page: ImageToBase64ShopImg, ShowSuccesalert, exportTableToExcel, exportToExcel, faildalert, pageLoad, showpop5, showpop6, successalert, sweetAlertConfirm_Delete, sweetAlertConfirm_Submit, sweetAlertConfirm_Update
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_NewReceiveableReportApps.Page_Load | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:20 | object sender, EventArgs e | SInventory_UI_NewReceiveableReportApps.Page_Load | no | no |
| SInventory_UI_NewReceiveableReportApps.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:39 | object sender, EventArgs e | SInventory_UI_NewReceiveableReportApps.fromDateTextBox_TextChanged | no | no |
| SInventory_UI_NewReceiveableReportApps.btnExport_Click | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:56 | object sender, EventArgs e | SInventory_UI_NewReceiveableReportApps.btnExport_Click | no | no |
| SInventory_UI_NewReceiveableReportApps.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:210 | object sender, GridViewPageEventArgs e | SInventory_UI_NewReceiveableReportApps.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_NewReceiveableReportApps.LoadData | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:215 |  | SInventory_UI_NewReceiveableReportApps.LoadData → InvoiceDAL.GetNewReceiveableDAl → SP: sp_Get_NewReceiveableList | no | no |
| SInventory_UI_NewReceiveableReportApps.SearchButton_Click | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:250 | object sender, EventArgs e | SInventory_UI_NewReceiveableReportApps.SearchButton_Click → InvoiceDAL.GetNewReceiveableDAl → SP: sp_Get_NewReceiveableList | no | no |
| SInventory_UI_NewReceiveableReportApps.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs:288 | object sender, EventArgs e | SInventory_UI_NewReceiveableReportApps.dcDropDownList1_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| InvoiceDAL.GetNewReceiveableDAl | Library.DAL/SInventory_DAL/InvoiceDAL.cs:93 | sp_Get_NewReceiveableList | @districtId, @fromDate, @toDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_NewReceiveableList | InvoiceDAL.GetNewReceiveableDAl | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
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
BLL: —; DAL: InvoiceDAL; DAO: —; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click SearchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click SearchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx`, `Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 35 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/NewReceiveableReportApps.aspx |
| Data access | This document §15–17 | 1 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: PAGE-217, PAGE-244, PAGE-402, PAGE-505
- Changing DAL methods InvoiceDAL.GetNewReceiveableDAl affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
