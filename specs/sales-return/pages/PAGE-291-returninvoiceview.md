---
page_id: PAGE-291
module: M08
page: ReturnInvoiceView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-291 — ReturnInvoiceView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-291 |
| Module | M08 — Sales Return & Invoice Return (INFERRED from folder + file name) |
| Page name | ReturnInvoiceView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_ReturnInvoiceView |
| aspx size | 10592 bytes |
| Code-behind LOC | 74 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ReturnInvoiceView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales returns, return invoices, delivery returns, replacement notes, return posting.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `returninvoicereportviewer2.aspx`

## 5. Route
`~/SInventory_UI/ReturnInvoiceView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx |
| Code-behind | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | ReturnInvoiceBLL (Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | ReturnInvoiceDAL (Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| HyperLink1 | HyperLink |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgfDate | ImageButton |  | no validator found |  |  |  |  |
| orderDate | CalendarExtender |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| ImageButton1 | ImageButton |  | no validator found |  |  |  |  |
| CalendarExtender1 | CalendarExtender |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| Button1 | Button | Search | no validator found |  |  |  | onclick→Button1_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand |
| (no id) | BoundField | ReturnInvoiceNo | no validator found |  |  |  |  |
| (no id) | BoundField | OrderNo | no validator found |  |  |  |  |
| (no id) | BoundField | TpTotal | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| rptImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | dtdata | SInventory_UI_ReturnInvoiceView.LoadDataa | ReturnInvoiceBLL.LoadReuturnInvoice | ReturnInvoiceDAL.GetDataTable, ReturnInvoiceDAL.LoadReuturnInvoice, ReturnInvoiceDAL.Parameter | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | Page_Load → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

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
| SInventory_UI_ReturnInvoiceView.Page_Load | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_ReturnInvoiceView.Page_Load → RequisitionBLL.DCLoad → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad | no | no |
| SInventory_UI_ReturnInvoiceView.loadGridView_RowCommand | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx.cs:23 | object sender, GridViewCommandEventArgs e | SInventory_UI_ReturnInvoiceView.loadGridView_RowCommand → ReturnInvoiceBLL.DeleteData / ReturnInvoiceBLL.LoadReuturnInvoice → ReturnInvoiceDAL.DeleteData / ReturnInvoiceDAL.Execute / ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadReuturnInvoice / ReturnI | no | no |
| SInventory_UI_ReturnInvoiceView.LoadDataa | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx.cs:47 |  | SInventory_UI_ReturnInvoiceView.LoadDataa → ReturnInvoiceBLL.LoadReuturnInvoice → ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadReuturnInvoice / ReturnInvoiceDAL.Parameter | no | no |
| SInventory_UI_ReturnInvoiceView.Button1_Click | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx.cs:71 | object sender, EventArgs e | SInventory_UI_ReturnInvoiceView.Button1_Click → ReturnInvoiceBLL.LoadReuturnInvoice → ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadReuturnInvoice / ReturnInvoiceDAL.Parameter | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| ReturnInvoiceBLL.DeleteData | Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs:85 |
| ReturnInvoiceBLL.LoadReuturnInvoice | Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs:95 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| ReturnInvoiceDAL.DeleteData | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:373 | inline SQL: tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail |  |
| ReturnInvoiceDAL.Execute | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:68 | delegates / no SQL literal |  |
| ReturnInvoiceDAL.Parameter | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:21 | delegates / no SQL literal |  |
| ReturnInvoiceDAL.LoadReuturnInvoice | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:476 | inline SQL: tblReturnInvoice |  |
| ReturnInvoiceDAL.GetDataTable | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:73 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail
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
- try/catch present in 0 of 5 methods.
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
BLL: RequisitionBLL, ReturnInvoiceBLL; DAL: RequisitionDAL, ReturnInvoiceDAL; DAO: —; Tables (inline): tblCompanyUnit, tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ReturnInvoiceView.aspx`, `Solution.Web/SInventory_UI/ReturnInvoiceView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 20 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ReturnInvoiceView.aspx |
| Data access | This document §15–17 | 0 SP names, 6 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M08 (Sales Return & Invoice Return).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods RequisitionDAL.BindDropDown, RequisitionDAL.DCLoad, ReturnInvoiceDAL.DeleteData, ReturnInvoiceDAL.Execute, ReturnInvoiceDAL.GetDataTable affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
