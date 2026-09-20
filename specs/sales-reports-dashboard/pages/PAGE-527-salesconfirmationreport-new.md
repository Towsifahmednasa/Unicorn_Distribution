---
page_id: PAGE-527
module: M13
page: SalesConfirmationReport_New
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-527 — SalesConfirmationReport_New

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-527 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | SalesConfirmationReport_New |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SalesConfirmationReport_New |
| aspx size | 17369 bytes |
| Code-behind LOC | 531 |
| Inline script size | 2286 chars |
| Suspected legacy/duplicate | YES (name pattern; INFERRED) |

## 2. Purpose
INFERRED from name/markup: **SalesConfirmationReport_New** (Report / Dashboard). Page title/heading text: `Details List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales/business/summary/trend reports, Crystal report viewers and dashboards not owned by a transactional module.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `deliverypaymentreport.aspx`, `proformareportviewer.aspx`

## 5. Route
`~/SInventory_UI/SalesConfirmationReport_New.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx |
| Code-behind | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | CmnCrystaltoView (Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| dcDropDownList1 | DropDownList |  | no validator found |  |  | see §9 |  |
| InvoiceDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| ddlProgramType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlCustomer | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlChemisType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProduct | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlOrderSender | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| lblTP | Label | Total TP : 0 | no validator found |  |  |  |  |
| lblVat | Label | Total VAT : 0 | no validator found |  |  |  |  |
| lblDiscount | Label | Total Discount : 0 | no validator found |  |  |  |  |
| lblCount | Label | Total Net Amount : 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPageIndexChanging→loadGridView_PageIndexChanging, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center  | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Number | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Confirmation Number | no validator found |  |  |  |  |
| (no id) | BoundField | Confirmation Date | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No | no validator found |  |  |  |  |
| (no id) | BoundField | Exp Date | no validator found |  |  |  |  |
| (no id) | BoundField | Sold Qty | no validator found |  |  |  |  |
| (no id) | BoundField | TP | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Special Discount | no validator found |  |  |  |  |
| (no id) | BoundField | Exp. Adjustment | no validator found |  |  |  |  |
| (no id) | BoundField |  Net Payment | no validator found |  |  |  |  |
| (no id) | BoundField | Order Sender Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order Sender Name | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | Sub-Territory | no validator found |  |  |  |  |
| (no id) | BoundField | MIO CODE | no validator found |  |  |  |  |
| (no id) | BoundField | Brand | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Name | no validator found |  |  |  |  |
| (no id) | BoundField | Campaign Category | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Type | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| InvoiceDateTextBox | inputDateTime | SInventory_UI_SalesConfirmationReport_New.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | param | SInventory_UI_SalesConfirmationReport_New.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_SalesConfirmationReport_New.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlProgramType | param | SInventory_UI_SalesConfirmationReport_New.Parm_2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlChemisType | param | SInventory_UI_SalesConfirmationReport_New.Parm_2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlCustomer | param | SInventory_UI_SalesConfirmationReport_New.Parm_2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlProduct | param | SInventory_UI_SalesConfirmationReport_New.Parm_2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlOrderSender | param | SInventory_UI_SalesConfirmationReport_New.Parm_2 | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | Text | SInventory_UI_SalesConfirmationReport_New.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_SalesConfirmationReport_New.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_SalesConfirmationReport_New.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | districtId | SInventory_UI_SalesConfirmationReport_New.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| ddlProgramType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlCustomer | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlChemisType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProduct | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlOrderSender | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_SalesConfirmationReport_New.SearchButton_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:200 | — | CmnCrystaltoView.GetSalesConfirmationReport_newDAL | sp_Get_AllSalesConfirmationReport_new | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| cancelButton | LinkButton click | (postback) | SInventory_UI_SalesConfirmationReport_New.cancelButton_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:134 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_SalesConfirmationReport_New.btnExport_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:329 | — | CmnCrystaltoView.GetSalesConfirmationReport_newDAL | sp_Get_AllSalesConfirmationReport_new | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- Client script functions in page: exportTableToExcel, exportToExcel, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_SalesConfirmationReport_New.Page_Load | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:25 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.Page_Load → OtherStockActionBLL.DCLoad → CmnCrystaltoView.GetCustomerListDAL / CmnCrystaltoView.GetOrderSenderListDAL / CmnCrystaltoView.GetProductListDAL / OtherStockActionDAL.DCLoad / SeedDataDAL.GetChemistTypeListAL | no | no |
| SInventory_UI_SalesConfirmationReport_New.LoadDropDown | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:53 |  | SInventory_UI_SalesConfirmationReport_New.LoadDropDown → OtherStockActionBLL.DCLoad → CmnCrystaltoView.GetCustomerListDAL / CmnCrystaltoView.GetOrderSenderListDAL / CmnCrystaltoView.GetProductListDAL / OtherStockActionDAL.DCLoad / SeedDataDAL.GetChemistTypeLis | yes | no |
| SInventory_UI_SalesConfirmationReport_New.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:129 | object sender, GridViewPageEventArgs e | SInventory_UI_SalesConfirmationReport_New.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_SalesConfirmationReport_New.cancelButton_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:134 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.cancelButton_Click | no | no |
| SInventory_UI_SalesConfirmationReport_New.LoadData | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:139 |  | SInventory_UI_SalesConfirmationReport_New.LoadData → CmnCrystaltoView.GetSalesConfirmationReport_newDAL → SP: sp_Get_AllSalesConfirmationReport_new | no | no |
| SInventory_UI_SalesConfirmationReport_New.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:191 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.fromDateTextBox_TextChanged | no | no |
| SInventory_UI_SalesConfirmationReport_New.SearchButton_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:200 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.SearchButton_Click → CmnCrystaltoView.GetSalesConfirmationReport_newDAL → SP: sp_Get_AllSalesConfirmationReport_new | no | no |
| SInventory_UI_SalesConfirmationReport_New.Parm_2 | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:234 |  | SInventory_UI_SalesConfirmationReport_New.Parm_2 | no | no |
| SInventory_UI_SalesConfirmationReport_New.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:301 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.dcDropDownList1_SelectedIndexChanged | no | no |
| SInventory_UI_SalesConfirmationReport_New.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:305 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.CheckBox1_CheckedChanged → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_SalesConfirmationReport_New._DAL_GetSalesConfirmationDAL | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:312 |  | SInventory_UI_SalesConfirmationReport_New._DAL_GetSalesConfirmationDAL → CmnCrystaltoView.GetSalesConfirmationReport_newDAL → SP: sp_Get_AllSalesConfirmationReport_new | yes | no |
| SInventory_UI_SalesConfirmationReport_New.btnExport_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:329 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.btnExport_Click → CmnCrystaltoView.GetSalesConfirmationReport_newDAL → SP: sp_Get_AllSalesConfirmationReport_new | no | no |
| SInventory_UI_SalesConfirmationReport_New.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:487 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_SalesConfirmationReport_New.viewRptButton_Click | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs:500 | object sender, EventArgs e | SInventory_UI_SalesConfirmationReport_New.viewRptButton_Click | no | no |

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
| SeedDataDAL.GetProgramTypeListAll | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:78 | sp_GET_ProgramTypeListAll |  |
| SeedDataDAL.GetChemistTypeListALL | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:97 | sp_GET_CustomerTypeAll |  |
| CmnCrystaltoView.GetCustomerListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1826 | inline SQL: tblCustMaster |  |
| CmnCrystaltoView.GetProductListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1862 | inline SQL: tblProduct |  |
| CmnCrystaltoView.GetOrderSenderListDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:1880 | inline SQL: tblEmpGeneralInfo |  |
| CmnCrystaltoView.GetSalesConfirmationReport_newDAL | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:438 | sp_Get_AllSalesConfirmationReport_new | @Parm, @Parm2 |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_CustomerTypeAll | SeedDataDAL.GetChemistTypeListALL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramTypeListAll | SeedDataDAL.GetProgramTypeListAll | body NOT in repository | read-like name (INFERRED) |
| sp_Get_AllSalesConfirmationReport_new | CmnCrystaltoView.GetSalesConfirmationReport_newDAL | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblProduct, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Customer --- | UNKNOWN |
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Order Sender --- | UNKNOWN |
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Product --- | UNKNOWN |
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ProformaReport, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 21 methods.
- Redirect targets: DeliveryPaymentReport.aspx
- User messages: --- Select Customer ---; --- Select Order Sender ---; --- Select Product ---; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL; DAL: CmnCrystaltoView, OtherStockActionDAL, SeedDataDAL; DAO: —; Tables (inline): tblCompanyUnit, tblCustMaster, tblEmpGeneralInfo, tblProduct, tblUserCompanyUnit

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
| TC-07 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx`, `Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 57 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SalesConfirmationReport_New.aspx |
| Data access | This document §15–17 | 3 SP names, 5 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 3 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- Name suggests a legacy/duplicate/test variant; confirm whether it is still reachable from the menu.

## 31. Change Impact
- Module: M13 (Sales Reports, Analytics & Dashboards).
- Other pages sharing this page's stored procedures: PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-320, PAGE-321, PAGE-339, PAGE-340
- Changing DAL methods CmnCrystaltoView.GetCustomerListDAL, CmnCrystaltoView.GetOrderSenderListDAL, CmnCrystaltoView.GetProductListDAL, CmnCrystaltoView.GetSalesConfirmationReport_newDAL, OtherStockActionDAL.DCLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
