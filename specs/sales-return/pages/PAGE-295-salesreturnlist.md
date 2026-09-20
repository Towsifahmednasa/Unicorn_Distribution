---
page_id: PAGE-295
module: M08
page: SalesReturnList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-295 — SalesReturnList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-295 |
| Module | M08 — Sales Return & Invoice Return (INFERRED from folder + file name) |
| Page name | SalesReturnList |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SalesReturnList |
| aspx size | 20187 bytes |
| Code-behind LOC | 99 |
| Inline script size | 2701 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SalesReturnList** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales returns, return invoices, delivery returns, replacement notes, return posting.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/SalesReturn.aspx`

Outbound references: `returninvoicereportviewer2.aspx`, `salesreturn.aspx`

## 5. Route
`~/SInventory_UI/SalesReturnList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SalesReturnList.aspx |
| Code-behind | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | ReturnInvoiceBLL (Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | ReturnInvoiceDAL (Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→EmpCetegoryAddImageButton_Click |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| LinkButton2 | LinkButton |  | no validator found |  |  |  | OnClick→Button1_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | ReturnInvoiceNo | no validator found |  |  |  |  |
| (no id) | BoundField | CustomerCode | no validator found |  |  |  |  |
| (no id) | BoundField | TpTotal | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| rptImageButton | ImageButton |  | no validator found |  |  |  |  |
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
| (no id) | BoundField | CustomerCode | no validator found |  |  |  |  |
| (no id) | BoundField | TpTotal | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| rptImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | dtdata | SInventory_UI_SalesReturnList.LoadDataa | ReturnInvoiceBLL.LoadSalesReuturnInvoice | ReturnInvoiceDAL.GetDataTable, ReturnInvoiceDAL.LoadSalesReuturnInvoice, ReturnInvoiceDAL.Parameter | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | Page_Load → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| salesCenterDropDownList | Page_Load → RequisitionBLL.DCLoad | inline SQL | Com/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | SInventory_UI_SalesReturnList.EmpCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:44 | — | — | — | read-only / none detected |
| LinkButton2 | LinkButton click | (postback) | SInventory_UI_SalesReturnList.Button1_Click | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:96 | ReturnInvoiceBLL.LoadSalesReuturnInvoice | ReturnInvoiceDAL.GetDataTable, ReturnInvoiceDAL.LoadSalesReuturnInvoice, ReturnInvoiceDAL.Parameter | — | read-only / none detected |

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
| SInventory_UI_SalesReturnList.Page_Load | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_SalesReturnList.Page_Load → RequisitionBLL.DCLoad / ReturnInvoiceBLL.LoadSalesReuturnInvoice → RequisitionDAL.BindDropDown / RequisitionDAL.DCLoad / ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadSalesReuturnInvoice / ReturnInvoiceDAL.Param | no | no |
| SInventory_UI_SalesReturnList.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:33 | object sender, EventArgs e | SInventory_UI_SalesReturnList.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_SalesReturnList.EmpCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:44 | object sender, EventArgs e | SInventory_UI_SalesReturnList.EmpCetegoryAddImageButton_Click | no | no |
| SInventory_UI_SalesReturnList.loadGridView_RowCommand | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:48 | object sender, GridViewCommandEventArgs e | SInventory_UI_SalesReturnList.loadGridView_RowCommand → ReturnInvoiceBLL.DeleteData / ReturnInvoiceBLL.LoadSalesReuturnInvoice → ReturnInvoiceDAL.DeleteData / ReturnInvoiceDAL.Execute / ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadSalesReuturnInvoice / | no | no |
| SInventory_UI_SalesReturnList.LoadDataa | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:72 |  | SInventory_UI_SalesReturnList.LoadDataa → ReturnInvoiceBLL.LoadSalesReuturnInvoice → ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadSalesReuturnInvoice / ReturnInvoiceDAL.Parameter | no | no |
| SInventory_UI_SalesReturnList.Button1_Click | Solution.Web/SInventory_UI/SalesReturnList.aspx.cs:96 | object sender, EventArgs e | SInventory_UI_SalesReturnList.Button1_Click → ReturnInvoiceBLL.LoadSalesReuturnInvoice → ReturnInvoiceDAL.GetDataTable / ReturnInvoiceDAL.LoadSalesReuturnInvoice / ReturnInvoiceDAL.Parameter | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| ReturnInvoiceBLL.LoadSalesReuturnInvoice | Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs:849 |
| ReturnInvoiceBLL.DeleteData | Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs:85 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| ReturnInvoiceDAL.LoadSalesReuturnInvoice | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:485 | inline SQL: tblCustMaster, tblReturnInvoice |  |
| ReturnInvoiceDAL.GetDataTable | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:73 | delegates / no SQL literal |  |
| ReturnInvoiceDAL.Parameter | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:21 | delegates / no SQL literal |  |
| ReturnInvoiceDAL.DeleteData | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:373 | inline SQL: tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail |  |
| ReturnInvoiceDAL.Execute | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:68 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblCustMaster, tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DICCompanyUnitId, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 7 methods.
- Redirect targets: SalesReturn.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: RequisitionBLL, ReturnInvoiceBLL; DAL: RequisitionDAL, ReturnInvoiceDAL; DAO: —; Tables (inline): tblCompanyUnit, tblCustMaster, tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail |
| TC-05 | Click LinkButton2 with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click LinkButton2 with valid data | Handler runs; rows written to tblDCStoreFreeze, tblReturnAmount, tblReturnInvoice, tblReturnInvoiceDetail |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SalesReturnList.aspx`, `Solution.Web/SInventory_UI/SalesReturnList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SalesReturnList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 33 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SalesReturnList.aspx |
| Data access | This document §15–17 | 0 SP names, 7 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M08 (Sales Return & Invoice Return).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods RequisitionDAL.BindDropDown, RequisitionDAL.DCLoad, ReturnInvoiceDAL.DeleteData, ReturnInvoiceDAL.Execute, ReturnInvoiceDAL.GetDataTable affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
