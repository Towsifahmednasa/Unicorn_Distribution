---
page_id: PAGE-339
module: M09
page: SC_CustomerPaymentReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-339 — SC_CustomerPaymentReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-339 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | SC_CustomerPaymentReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SC_CustomerPaymentReport |
| aspx size | 53118 bytes |
| Code-behind LOC | 627 |
| Inline script size | 24896 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SC_CustomerPaymentReport** (Report / Dashboard). Page title/heading text: `' + reportTitle + '`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `proformareportviewer.aspx`

## 5. Route
`~/SInventory_UI/SC_CustomerPaymentReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx |
| Code-behind | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs |
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
| ddlPaymentBy | DropDownList |  | no validator found |  |  | see §9 |  |
| InvoiceDateTextBox | TextBox | Select Invoice From Date | no validator found |  |  |  | OnTextChanged→fromDateTextBox_TextChanged |
| todateTextBox | TextBox | Select Invoice To Date | no validator found |  |  |  |  |
| ddlCustomer | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlChemisType | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProgramType | DropDownList |  | no validator found |  |  | see §9 |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| submitButton | LinkButton |  | no validator found |  |  |  | OnClick→SearchButton_Click |
| btnSummaryReport | LinkButton |  | no validator found |  |  |  | OnClick→SummaryButton_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| lblCount | Label | Total Pay Amount (TP+Vat) : 0 | no validator found |  |  |  |  |
| btnExport | LinkButton |  | no validator found |  |  |  | OnClick→btnExport_Click |
| btnPrintPdfDetails | LinkButton |  | no validator found |  |  |  | OnClientClick→printReportDiv('MainGradeDiv', 'Customer Payment Details Report'); return false; |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order / Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Number | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Delivary Invoice | no validator found |  |  |  |  |
| (no id) | BoundField | Delivary Date | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Number | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Date | no validator found |  |  |  |  |
| (no id) | BoundField | Cash / Bank | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Paid Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Discount | no validator found |  |  |  |  |
| (no id) | BoundField | AIT + VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Mode | no validator found |  |  |  |  |
| (no id) | BoundField | User Name | no validator found |  |  |  |  |
| lblSummaryCount | Label | Total Collection Amount : 0 | no validator found |  |  |  |  |
| btnExportSummary | LinkButton |  | no validator found |  |  |  | OnClick→btnExportSummary_Click |
| btnPrintPdfSummary | LinkButton |  | no validator found |  |  |  | OnClientClick→printReportDiv('SummaryGradeDiv', 'Customer Payment Summary Report'); return false; |
| summaryGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Date | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Ref. No. | no validator found |  |  |  |  |
| (no id) | BoundField | Cash / Bank | no validator found |  |  |  |  |
| (no id) | BoundField | Collection Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Mode | no validator found |  |  |  |  |
| (no id) | BoundField | User Name | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| InvoiceDateTextBox | inputDateTime | SInventory_UI_SC_CustomerPaymentReport.fromDateTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | param | SInventory_UI_SC_CustomerPaymentReport.OldNewParm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_SC_CustomerPaymentReport.OldNewParm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | param | SInventory_UI_SC_CustomerPaymentReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlPaymentBy | param | SInventory_UI_SC_CustomerPaymentReport.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | Text | SInventory_UI_SC_CustomerPaymentReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| InvoiceDateTextBox | fromDate | SInventory_UI_SC_CustomerPaymentReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| todateTextBox | toDate | SInventory_UI_SC_CustomerPaymentReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| dcDropDownList1 | districtId | SInventory_UI_SC_CustomerPaymentReport.viewRptButton_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| dcDropDownList1 | LoadDropDown → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| ddlPaymentBy | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlCustomer | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlChemisType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProgramType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | LinkButton click | (postback) | SInventory_UI_SC_CustomerPaymentReport.SearchButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:244 | — | CmnCrystaltoView.AddDateParameter, CmnCrystaltoView.AppendIntFilter, CmnCrystaltoView.AppendStringFi | — | read-only / none detected |
| btnSummaryReport | LinkButton click | (postback) | SInventory_UI_SC_CustomerPaymentReport.SummaryButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:249 | — | CmnCrystaltoView.AddDateParameter, CmnCrystaltoView.AppendIntFilter, CmnCrystaltoView.AppendStringFi | — | read-only / none detected |
| cancelButton | LinkButton click | (postback) | SInventory_UI_SC_CustomerPaymentReport.cancelButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:140 | — | — | — | read-only / none detected |
| btnExport | LinkButton click | (postback) | SInventory_UI_SC_CustomerPaymentReport.btnExport_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:346 | — | — | — | read-only / none detected |
| btnPrintPdfDetails | LinkButton | printReportDiv('MainGradeDiv', 'Customer Payment Details Report'); return false; | — | — | — | — | — | UNKNOWN (handler not resolved) |
| btnExportSummary | LinkButton click | (postback) | SInventory_UI_SC_CustomerPaymentReport.btnExportSummary_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:462 | — | — | — | read-only / none detected |
| btnPrintPdfSummary | LinkButton | printReportDiv('SummaryGradeDiv', 'Customer Payment Summary Report'); return false; | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: exportTableToExcel, exportToExcel, initUIPlugins, pageLoad, printReportDiv
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_SC_CustomerPaymentReport.Page_Load | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:56 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.Page_Load → OtherStockActionBLL.DCLoad → CmnCrystaltoView.GetCustomerListDAL / OtherStockActionDAL.DCLoad / SeedDataDAL.GetChemistTypeListALL / SeedDataDAL.GetProgramTypeListAll → SP: sp_GET_CustomerTypeAll, sp_GET_Progra | no | no |
| SInventory_UI_SC_CustomerPaymentReport.LoadDropDown | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:83 |  | SInventory_UI_SC_CustomerPaymentReport.LoadDropDown → OtherStockActionBLL.DCLoad → CmnCrystaltoView.GetCustomerListDAL / OtherStockActionDAL.DCLoad / SeedDataDAL.GetChemistTypeListALL / SeedDataDAL.GetProgramTypeListAll → SP: sp_GET_CustomerTypeAll, sp_GET_Pro | yes | no |
| SInventory_UI_SC_CustomerPaymentReport.loadGridView_PageIndexChanging | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:135 | object sender, GridViewPageEventArgs e | SInventory_UI_SC_CustomerPaymentReport.loadGridView_PageIndexChanging | no | no |
| SInventory_UI_SC_CustomerPaymentReport.cancelButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:140 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.cancelButton_Click | no | no |
| SInventory_UI_SC_CustomerPaymentReport.LoadData | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:145 |  | SInventory_UI_SC_CustomerPaymentReport.LoadData → CmnCrystaltoView.AddDateParameter / CmnCrystaltoView.AppendIntFilter / CmnCrystaltoView.AppendStringFilter / CmnCrystaltoView.BuildPaymentFilter / CmnCrystaltoView.BuildReportFilter / CmnCrystaltoView | no | no |
| SInventory_UI_SC_CustomerPaymentReport.LoadSummaryData | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:199 |  | SInventory_UI_SC_CustomerPaymentReport.LoadSummaryData → CmnCrystaltoView.AddDateParameter / CmnCrystaltoView.AppendIntFilter / CmnCrystaltoView.AppendStringFilter / CmnCrystaltoView.BuildPaymentFilter / CmnCrystaltoView.BuildReportFilter / CmnCrystaltoView | no | no |
| SInventory_UI_SC_CustomerPaymentReport.fromDateTextBox_TextChanged | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:233 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.fromDateTextBox_TextChanged | no | no |
| SInventory_UI_SC_CustomerPaymentReport.SearchButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:244 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.SearchButton_Click → CmnCrystaltoView.AddDateParameter / CmnCrystaltoView.AppendIntFilter / CmnCrystaltoView.AppendStringFilter / CmnCrystaltoView.BuildPaymentFilter / CmnCrystaltoView.BuildReportFilter / CmnCrystaltoView | no | no |
| SInventory_UI_SC_CustomerPaymentReport.SummaryButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:249 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.SummaryButton_Click → CmnCrystaltoView.AddDateParameter / CmnCrystaltoView.AppendIntFilter / CmnCrystaltoView.AppendStringFilter / CmnCrystaltoView.BuildPaymentFilter / CmnCrystaltoView.BuildReportFilter / CmnCrystaltoVie | no | no |
| SInventory_UI_SC_CustomerPaymentReport.Parm_2 | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:282 |  | SInventory_UI_SC_CustomerPaymentReport.Parm_2 | no | no |
| SInventory_UI_SC_CustomerPaymentReport.dcDropDownList1_SelectedIndexChanged | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:336 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.dcDropDownList1_SelectedIndexChanged | no | no |
| SInventory_UI_SC_CustomerPaymentReport.CheckBox1_CheckedChanged | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:340 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.CheckBox1_CheckedChanged → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | no | no |
| SInventory_UI_SC_CustomerPaymentReport.btnExport_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:346 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.btnExport_Click | no | no |
| SInventory_UI_SC_CustomerPaymentReport.btnExportSummary_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:462 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.btnExportSummary_Click | no | no |
| SInventory_UI_SC_CustomerPaymentReport.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:581 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_SC_CustomerPaymentReport.viewRptButton_Click | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs:596 | object sender, EventArgs e | SInventory_UI_SC_CustomerPaymentReport.viewRptButton_Click | no | no |

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
| CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | inline SQL: CSTL_ACCDB_Unicorn, tblCompanyUnit, tblCustMaster, tblCustPayDetail |  |
| CmnCrystaltoView.BuildPaymentFilter | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:676 | delegates / no SQL literal | @CollectionBy |
| CmnCrystaltoView.AddDateParameter | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:744 | delegates / no SQL literal |  |
| CmnCrystaltoView.BuildReportFilter | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:702 | delegates / no SQL literal |  |
| CmnCrystaltoView.AppendIntFilter | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:720 | delegates / no SQL literal |  |
| CmnCrystaltoView.AppendStringFilter | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:732 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_CustomerTypeAll | SeedDataDAL.GetChemistTypeListALL | body NOT in repository | read-like name (INFERRED) |
| sp_GET_ProgramTypeListAll | SeedDataDAL.GetProgramTypeListAll | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): CSTL_ACCDB_Unicorn, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblCustomertype, tblDAInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblInvoiceDetailReturn, tblMarket, tblOrder, tblProgramType, tblRegion, tblRouteInformationMaster, tblSubTerritory, tblTerritory, tblUser, tblUserCompanyUnit, tblarea
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | --- Select Customer --- | UNKNOWN |
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: ProformaReport
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 22 methods.
- Redirect targets: SC_CustomerPaymentReport.aspx
- User messages: --- Select Customer ---; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL; DAL: CmnCrystaltoView, OtherStockActionDAL, SeedDataDAL; DAO: —; Tables (inline): CSTL_ACCDB_Unicorn, tblCompanyUnit, tblCustMaster, tblCustPayDetail, tblCustomerPay, tblCustomertype, tblDAInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblInvoiceDetailReturn, tblMarket, tblOrder, tblProgramType, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnSummaryReport with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSummaryReport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click cancelButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click cancelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnExport with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnExport with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click btnExportSummary with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click btnExportSummary with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx`, `Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 59 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/SC_CustomerPaymentReport.aspx |
| Data access | This document §15–17 | 2 SP names, 21 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: PAGE-207, PAGE-208, PAGE-209, PAGE-210, PAGE-320, PAGE-321, PAGE-340, PAGE-527
- Changing DAL methods CmnCrystaltoView.AddDateParameter, CmnCrystaltoView.AppendIntFilter, CmnCrystaltoView.AppendStringFilter, CmnCrystaltoView.BuildPaymentFilter, CmnCrystaltoView.BuildReportFilter affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
