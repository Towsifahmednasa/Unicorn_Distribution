---
page_id: PAGE-232
module: M06
page: SampleInvoiceCreationByOrder
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-232 — SampleInvoiceCreationByOrder

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-232 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | SampleInvoiceCreationByOrder |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MasterPage.master |
| Page class | SInventory_UI_InvoiceCreationByOrder |
| aspx size | 15839 bytes |
| Code-behind LOC | 198 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SampleInvoiceCreationByOrder** (Entry / Transaction). Page title/heading text: `Sample Invoice Creation`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `invoicecreationbyorder.aspx`, `invoicecreationforcustomerbyorder.aspx`

## 5. Route
`~/SInventory_UI/SampleInvoiceCreationByOrder.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx |
| Code-behind | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs |
| Master | Solution.Web/MasterPages/MasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait7 | Image |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→salesCenterDropDownList_SelectedIndexChanged |
| manufacDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→manufacDropDownList_SelectedIndexChanged |
| marketDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→marketDropDownList_SelectedIndexChanged |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→Button1_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| invoiceButton | Button | Generate Invoice | no validator found |  |  |  | onclick→invoiceButton_Click |
| orderGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Order No | no validator found |  |  |  |  |
| (no id) | BoundField | Order Date | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Code | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Value(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Go To Invoice | no validator found |  |  |  |  |
| gotoinvoiceButton | Button | Go To Invoice | no validator found |  |  |  | OnClick→gotoinvoiceButton_Click |
| (no id) | TemplateField | Generate Invoice | no validator found |  |  |  |  |
| GenerateinvoiceButton | Button | Generate | no validator found |  |  |  | onclick→GeneratetoinvoiceButton_Click |
| Button1 | Button | Search | no validator found |  |  |  | OnClick→Button1_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacDropDownList | DropDownlist → OrderInfoBLL.LoadManufac | inline SQL | ManufacName/ManufacId | tblManufacturer |
| marketDropDownList | salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadMarketOrderWise | inline SQL | MarketName/MarketId | tblCompanyUnit, tblMarket, tblOrder |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_InvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:178 | — | — | — | read-only / none detected |
| cancelButton | LinkButton click | (postback) | SInventory_UI_InvoiceCreationByOrder.cancelButton_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:183 | — | — | — | read-only / none detected |
| gotoinvoiceButton | Button click | (postback) | SInventory_UI_InvoiceCreationByOrder.gotoinvoiceButton_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:104 | — | — | — | read-only / none detected |
| Button1 | Button click | (postback) | SInventory_UI_InvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:178 | — | — | — | read-only / none detected |

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
| SInventory_UI_InvoiceCreationByOrder.Page_Load | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.Page_Load → OrderInfoBLL.LoadManufac / OrderInfoBLL.LoadMarketOrderWise / OrderInfoBLL.LoadSC → OrderInfoDAL.LoadManufac / OrderInfoDAL.LoadMarketOrderWise / OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_InvoiceCreationByOrder.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:30 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_InvoiceCreationByOrder.invoiceButton_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:48 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.invoiceButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.DropDownlist | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:79 |  | SInventory_UI_InvoiceCreationByOrder.DropDownlist → OrderInfoBLL.LoadManufac / OrderInfoBLL.LoadSC → OrderInfoDAL.LoadManufac / OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_InvoiceCreationByOrder.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:86 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadMarketOrderWise → OrderInfoDAL.LoadMarketOrderWise | no | no |
| SInventory_UI_InvoiceCreationByOrder.manufacDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:92 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.manufacDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_InvoiceCreationByOrder.marketDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:96 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.marketDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_InvoiceCreationByOrder.gotoinvoiceButton_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:104 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.gotoinvoiceButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.GeneratetoinvoiceButton_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:115 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.GeneratetoinvoiceButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:178 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.Button1_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.cancelButton_Click | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:183 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.cancelButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs:188 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.LoadMarketOrderWise | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:79 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadManufac | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2219 | inline SQL: tblManufacturer |  |
| OrderInfoDAL.LoadMarketOrderWise | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2287 | inline SQL: tblCompanyUnit, tblMarket, tblOrder | @ComUnitId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblManufacturer, tblMarket, tblOrder, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: MarketId, OrderId, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 15 methods.
- Redirect targets: InvoiceCreationByOrder.aspx, InvoiceCreationForCustomerByOrder.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL; DAL: OrderInfoDAL; DAO: —; Tables (inline): tblCompanyUnit, tblManufacturer, tblMarket, tblOrder, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click cancelButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click cancelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click gotoinvoiceButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click gotoinvoiceButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click Button1 with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click Button1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx`, `Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 32 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SampleInvoiceCreationByOrder.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadManufac, OrderInfoDAL.LoadMarketOrderWise, OrderInfoDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
