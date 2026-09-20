---
page_id: PAGE-243
module: M07
page: InvoicePrintingList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-243 — InvoicePrintingList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-243 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | InvoicePrintingList |
| Kind | Report viewer (INFERRED from name) |
| Web folder | SInventory_RPTVIEW |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_InvoicePrintingList |
| aspx size | 7719 bytes |
| Code-behind LOC | 97 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **InvoicePrintingList** (Report viewer). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Proforma and invoice creation (manual/auto/by-order), challans, top sheets, picking, loading, delivery assistants, invoice printing.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `invoicereportviewer.aspx`, `login.aspx`

## 5. Route
`~/SInventory_RPTVIEW/InvoicePrintingList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx |
| Code-behind | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | InvoiceBLL (Library.BLL/SInventory_BLL/InvoiceBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| dcDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| dateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| searchButton | Button | Search | no validator found |  |  |  | onclick→searchButton_Click |
| printButton | Button | Print | no validator found |  |  |  | onclick→printButton_Click |
| reportListGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Select | no validator found |  |  |  |  |
| printCheckBox | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | InvoiceNo | no validator found |  |  |  |  |
| (no id) | BoundField | InvoiceDate | no validator found |  |  |  |  |
| (no id) | BoundField | OrderNo | no validator found |  |  |  |  |
| (no id) | BoundField | OrderDate | no validator found |  |  |  |  |
| (no id) | BoundField | CustomerCode | no validator found |  |  |  |  |
| (no id) | BoundField | CustomerName | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| dcDropDownList | aDataTable | SInventory_UI_InvoicePrintingList.searchButton_Click | InvoiceBLL.AllInvoiceForPrintingBLL | InvoiceDAL.AllInvoiceForPrintingDAL, InvoiceDAL.Param | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList | Page_Load → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
No buttons with server handlers found.

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
| SInventory_UI_InvoicePrintingList.Page_Load | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_InvoicePrintingList.Page_Load → RequisitionBLL.DCLoad → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad | no | no |
| SInventory_UI_InvoicePrintingList.printButton_Click | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx.cs:37 | object sender, EventArgs e | SInventory_UI_InvoicePrintingList.printButton_Click | no | no |
| SInventory_UI_InvoicePrintingList.searchButton_Click | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx.cs:64 | object sender, EventArgs e | SInventory_UI_InvoicePrintingList.searchButton_Click → InvoiceBLL.AllInvoiceForPrintingBLL → InvoiceDAL.AllInvoiceForPrintingDAL / InvoiceDAL.Param | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| InvoiceBLL.AllInvoiceForPrintingBLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs:779 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| InvoiceDAL.AllInvoiceForPrintingDAL | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1087 | inline SQL: tblCustMaster, tblInvoice |  |
| InvoiceDAL.Param | Library.DAL/SInventory_DAL/InvoiceDAL.cs:25 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblCustMaster, tblInvoice, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler printButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select First!! | UNKNOWN |
| (handler searchButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select DC & Date !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ComUnitId, UserType
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 4 methods.
- Redirect targets: Login.aspx
- User messages: Select DC & Date !!; Select First!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: InvoiceBLL, RequisitionBLL; DAL: InvoiceDAL, RequisitionDAL; DAO: —; Tables (inline): tblCompanyUnit, tblCustMaster, tblInvoice, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx`, `Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 18 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_RPTVIEW/InvoicePrintingList.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M07 (Proforma, Invoicing & Delivery).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods InvoiceDAL.AllInvoiceForPrintingDAL, InvoiceDAL.Param, RequisitionDAL.BindDropDown, RequisitionDAL.DCLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
