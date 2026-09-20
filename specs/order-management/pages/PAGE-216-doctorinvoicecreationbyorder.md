---
page_id: PAGE-216
module: M06
page: DoctorInvoiceCreationByOrder
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-216 — DoctorInvoiceCreationByOrder

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-216 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | DoctorInvoiceCreationByOrder |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_InvoiceCreationByOrder |
| aspx size | 20468 bytes |
| Code-behind LOC | 269 |
| Inline script size | 1189 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DoctorInvoiceCreationByOrder** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `invoicecreationbyorder.aspx`, `invoicecreationforcustomerbyorder.aspx`, `proformareportprintviewer.aspx`

## 5. Route
`~/SInventory_UI/DoctorInvoiceCreationByOrder.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx |
| Code-behind | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→salesCenterDropDownList_SelectedIndexChanged |
| rootDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→rootDropDownList_OnSelectedIndexChanged |
| manufacDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→manufacDropDownList_SelectedIndexChanged |
| marketDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→marketDropDownList_SelectedIndexChanged |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→Button1_Click |
| reportButton | LinkButton |  | no validator found |  |  |  | OnClick→viewRptButton_Click |
| batchno | TextBox |  Batch NO | no validator found |  |  |  |  |
| invoiceButton | LinkButton |  | no validator found |  |  |  | onclick→invoiceButton_Click |
| orderGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Order No | no validator found |  |  |  |  |
| (no id) | BoundField | Order Date | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Code | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Name | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Code | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Value(TP) | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
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
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| batchno | pram | SInventory_UI_InvoiceCreationByOrder.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| rootDropDownList | batchn | SInventory_UI_InvoiceCreationByOrder.invoiceButton_Click | OrderInfoBLL.LoadDoctorOrderForOrderCreation, OrderInfoBLL.LoadInvoiceBatchId | OrderInfoDAL.LoadDoctorOrderForOrderCreation, OrderInfoDAL.LoadInvoiceBatchId | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | aTable | SInventory_UI_InvoiceCreationByOrder.GridView | OrderInfoBLL.LoadDoctorOrderForOrderCreation | OrderInfoDAL.LoadDoctorOrderForOrderCreation | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| rootDropDownList | salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadDisRouteforInvoice | inline SQL | DistributionRouteName/DistributionRouteId | tblOrder, tblRouteInformationMaster |
| manufacDropDownList | DropDownlist → OrderInfoBLL.LoadManufac | inline SQL | ManufacName/ManufacId | tblManufacturer |
| marketDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_InvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:266 | OrderInfoBLL.LoadDoctorOrderForOrderCreation | OrderInfoDAL.LoadDoctorOrderForOrderCreation | — | read-only / none detected |
|  | LinkButton click | (postback) | SInventory_UI_InvoiceCreationByOrder.cancelButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:113 | — | — | — | read-only / none detected |
| reportButton | LinkButton click | (postback) | SInventory_UI_InvoiceCreationByOrder.viewRptButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:37 | — | — | — | read-only / none detected |
| gotoinvoiceButton | Button click | (postback) | SInventory_UI_InvoiceCreationByOrder.gotoinvoiceButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:189 | — | — | — | read-only / none detected |
| Button1 | Button click | (postback) | SInventory_UI_InvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:266 | OrderInfoBLL.LoadDoctorOrderForOrderCreation | OrderInfoDAL.LoadDoctorOrderForOrderCreation | — | read-only / none detected |

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
| SInventory_UI_InvoiceCreationByOrder.Page_Load | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.Page_Load → OrderInfoBLL.LoadDisRouteforInvoice / OrderInfoBLL.LoadManufac / OrderInfoBLL.LoadSC → OrderInfoDAL.LoadDisRouteforInvoice / OrderInfoDAL.LoadManufac / OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_InvoiceCreationByOrder.GenerateParameter | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:30 |  | SInventory_UI_InvoiceCreationByOrder.GenerateParameter | no | no |
| SInventory_UI_InvoiceCreationByOrder.viewRptButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:37 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.viewRptButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.invoiceButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:59 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.invoiceButton_Click → OrderInfoBLL.LoadDoctorOrderForOrderCreation / OrderInfoBLL.LoadInvoiceBatchId → OrderInfoDAL.LoadDoctorOrderForOrderCreation / OrderInfoDAL.LoadInvoiceBatchId | yes | no |
| SInventory_UI_InvoiceCreationByOrder.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:95 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_InvoiceCreationByOrder.cancelButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:113 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.cancelButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:117 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_InvoiceCreationByOrder.rootDropDownList_OnSelectedIndexChanged | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:129 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.rootDropDownList_OnSelectedIndexChanged | yes | no |
| SInventory_UI_InvoiceCreationByOrder.DropDownlist | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:141 |  | SInventory_UI_InvoiceCreationByOrder.DropDownlist → OrderInfoBLL.LoadDisRouteforInvoice / OrderInfoBLL.LoadManufac / OrderInfoBLL.LoadSC → OrderInfoDAL.LoadDisRouteforInvoice / OrderInfoDAL.LoadManufac / OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_InvoiceCreationByOrder.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:150 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadDisRouteforInvoice → OrderInfoDAL.LoadDisRouteforInvoice | yes | no |
| SInventory_UI_InvoiceCreationByOrder.manufacDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:177 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.manufacDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_InvoiceCreationByOrder.marketDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:181 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.marketDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_InvoiceCreationByOrder.gotoinvoiceButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:189 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.gotoinvoiceButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.GeneratetoinvoiceButton_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:200 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.GeneratetoinvoiceButton_Click | no | no |
| SInventory_UI_InvoiceCreationByOrder.GridView | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:238 |  | SInventory_UI_InvoiceCreationByOrder.GridView → OrderInfoBLL.LoadDoctorOrderForOrderCreation → OrderInfoDAL.LoadDoctorOrderForOrderCreation | yes | no |
| SInventory_UI_InvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:266 | object sender, EventArgs e | SInventory_UI_InvoiceCreationByOrder.Button1_Click → OrderInfoBLL.LoadDoctorOrderForOrderCreation → OrderInfoDAL.LoadDoctorOrderForOrderCreation | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.LoadDisRouteforInvoice | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:104 |
| OrderInfoBLL.LoadInvoiceBatchId | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:75 |
| OrderInfoBLL.LoadDoctorOrderForOrderCreation | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:30 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadManufac | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2219 | inline SQL: tblManufacturer |  |
| OrderInfoDAL.LoadDisRouteforInvoice | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2007 | inline SQL: tblOrder, tblRouteInformationMaster | @ComUnitId |
| OrderInfoDAL.LoadInvoiceBatchId | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:529 | inline SQL: tblInvoiceBatch |  |
| OrderInfoDAL.LoadDoctorOrderForOrderCreation | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:183 | inline SQL: tblCompanyUnit, tblOrder_Doctorrequirement | @ComUnitId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblInvoice, tblInvoiceBatch, tblManufacturer, tblOrder, tblOrder_Doctorrequirement, tblRouteInformationMaster, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: MarketId, OrderId, RouteId, UserId, paydetailId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 5 of 18 methods.
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
BLL: OrderInfoBLL; DAL: OrderInfoDAL; DAO: —; Tables (inline): tblCompanyUnit, tblInvoice, tblInvoiceBatch, tblManufacturer, tblOrder, tblOrder_Doctorrequirement, tblRouteInformationMaster, tblUserCompanyUnit

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
| TC-07 | Click reportButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click reportButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click gotoinvoiceButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click gotoinvoiceButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click Button1 with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click Button1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx`, `Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 36 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx |
| Data access | This document §15–17 | 0 SP names, 8 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Inline SQL in code-behind without SqlParameter (possible string-built SQL — verify): GenerateParameter
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadDisRouteforInvoice, OrderInfoDAL.LoadDoctorOrderForOrderCreation, OrderInfoDAL.LoadInvoiceBatchId, OrderInfoDAL.LoadManufac, OrderInfoDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
