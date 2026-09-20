---
page_id: PAGE-322
module: M09
page: DepositList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-322 — DepositList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-322 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | DepositList |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DepositList |
| aspx size | 34084 bytes |
| Code-behind LOC | 371 |
| Inline script size | 3556 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DepositList** (List / View). Page title/heading text: `Proforma Invoice Print`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CompanyWiseBranch.aspx`, `SInventory_UI/DepositSlipExcelUpload.aspx`, `SInventory_UI/DepositSlipExcelUpload_old.aspx`

Outbound references: `companywisebranch.aspx`, `depositslipexcelupload.aspx`

## 5. Route
`~/SInventory_UI/DepositList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DepositList.aspx |
| Code-behind | Solution.Web/SInventory_UI/DepositList.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CompanywisebranchDal (Library.DAL/SInventory_DAL/CompanywisebranchDal.cs) |
| DAO/entity class | CompanyWiseDepositDao (Library.DAO/SInventory_Entities/CompanyWiseDepositDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  |  |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait7 | Image |  | no validator found |  |  |  |  |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| companyNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| SearchButton | LinkButton |  | no validator found |  |  |  | onclick→Button_Click |
| reportButton | LinkButton |  | no validator found |  |  |  | onclick→clearButton_OnClick |
| excelButton1 | LinkButton |  | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| lblCount | Label | Total Amount: 0 | no validator found |  |  |  |  |
| invoiceGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, onrowcommand→loadGridView_RowCommand |
| (no id) | BoundField | Unit Name | no validator found |  |  |  |  |
| (no id) | BoundField | Deposit Type | no validator found |  |  |  |  |
| (no id) | BoundField | Bank Name | no validator found |  |  |  |  |
| (no id) | BoundField | Account Number | no validator found |  |  |  |  |
| (no id) | BoundField | Branch Name | no validator found |  |  |  |  |
| (no id) | BoundField | Instrument Number | no validator found |  |  |  |  |
| (no id) | BoundField | Instrument Date | no validator found |  |  |  |  |
| (no id) | BoundField | Deposit Date | no validator found |  |  |  |  |
| (no id) | BoundField | Emp. ID | no validator found |  |  |  |  |
| (no id) | BoundField | Emp. Name | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | AIT | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirmDepositDelete(); |
| ListImageButton | ImageButton |  | no validator found |  |  |  | onclick→ListImageButton_Click |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| companyNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| ImageButton1 | ImageButton |  | no validator found |  |  |  |  |
| CalendarExtender12 | CalendarExtender |  | no validator found |  |  |  |  |
| Button1 | Button | Search | no validator found |  |  |  | OnClick→Button_Click |
| clearButton | Button | Clear | no validator found |  |  |  | OnClick→clearButton_OnClick |
| excelButton1 | Button | Export to Excel | no validator found |  |  |  | OnClick→btnExportToExcel_Click |
| invoiceGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand |
| (no id) | BoundField | Unit Name | no validator found |  |  |  |  |
| (no id) | BoundField | Deposit Type | no validator found |  |  |  |  |
| (no id) | BoundField | Bank Name | no validator found |  |  |  |  |
| (no id) | BoundField | Account Name | no validator found |  |  |  |  |
| (no id) | BoundField | Branch Name | no validator found |  |  |  |  |
| (no id) | BoundField | Check Number | no validator found |  |  |  |  |
| (no id) | BoundField | Check Date | no validator found |  |  |  |  |
| (no id) | BoundField | Deposit Date | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | AIT | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | fromDate | SInventory_UI_DepositList.LoadInfo | — | CompanywisebranchDal.LoadDepositList | UNKNOWN | UNKNOWN | UNKNOWN |
| toDateTextBox | toDate | SInventory_UI_DepositList.LoadInfo | — | CompanywisebranchDal.LoadDepositList | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | headerTable | SInventory_UI_DepositList.btnExportToExcel_Click | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| companyNameDropDownList | Loaddropdownlist → CompanywisebranchDal.LoadCompany | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| companyNameDropDownList | Loaddropdownlist → CompanywisebranchDal.LoadCompany | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| excelButton1 | LinkButton click | (postback) | SInventory_UI_DepositList.btnExportToExcel_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:296 | — | — | — | read-only / none detected |
| editImageButton | ImageButton | return confirmDepositDelete(); | — | — | — | — | — | UNKNOWN (handler not resolved) |
| Button1 | Button click | (postback) | SInventory_UI_DepositList.Button_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:162 | — | CompanywisebranchDal.LoadDepositList | — | read-only / none detected |
| clearButton | Button click | (postback) | SInventory_UI_DepositList.clearButton_OnClick | Solution.Web/SInventory_UI/DepositList.aspx.cs:64 | — | CompanywisebranchDal.LoadCompany, CompanywisebranchDal.LoadDropDownValue | — | read-only / none detected |
| excelButton1 | Button click | (postback) | SInventory_UI_DepositList.btnExportToExcel_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:296 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: confirmDepositDelete, pageLoad, restoreDepositListScrollPosition, saveDepositListScrollPosition
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_DepositList.Page_Load | Solution.Web/SInventory_UI/DepositList.aspx.cs:27 | object sender, EventArgs e | SInventory_UI_DepositList.Page_Load → CompanywisebranchDal.LoadCompany / CompanywisebranchDal.LoadDropDownValue | no | no |
| SInventory_UI_DepositList.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/DepositList.aspx.cs:36 | object sender, EventArgs e | SInventory_UI_DepositList.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_DepositList.Loaddropdownlist | Solution.Web/SInventory_UI/DepositList.aspx.cs:47 |  | SInventory_UI_DepositList.Loaddropdownlist → CompanywisebranchDal.LoadCompany / CompanywisebranchDal.LoadDropDownValue | yes | no |
| SInventory_UI_DepositList.clearButton_OnClick | Solution.Web/SInventory_UI/DepositList.aspx.cs:64 | object sender, EventArgs e | SInventory_UI_DepositList.clearButton_OnClick → CompanywisebranchDal.LoadCompany / CompanywisebranchDal.LoadDropDownValue | no | no |
| SInventory_UI_DepositList.LoadInfo | Solution.Web/SInventory_UI/DepositList.aspx.cs:76 |  | SInventory_UI_DepositList.LoadInfo → CompanywisebranchDal.LoadDepositList | no | no |
| SInventory_UI_DepositList.Button_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:162 | object sender, EventArgs e | SInventory_UI_DepositList.Button_Click → CompanywisebranchDal.LoadDepositList | no | no |
| SInventory_UI_DepositList.loadGridView_RowCommand | Solution.Web/SInventory_UI/DepositList.aspx.cs:204 | object sender, GridViewCommandEventArgs e | SInventory_UI_DepositList.loadGridView_RowCommand → CompanywisebranchDal.UpdateDepositInfo | no | no |
| SInventory_UI_DepositList.ListImageButton_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:278 | object sender, EventArgs e | SInventory_UI_DepositList.ListImageButton_Click | no | no |
| SInventory_UI_DepositList.ListImageButton_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:291 | object sender, ImageClickEventArgs e | SInventory_UI_DepositList.ListImageButton_Click | no | no |
| SInventory_UI_DepositList.btnExportToExcel_Click | Solution.Web/SInventory_UI/DepositList.aspx.cs:296 | object sender, EventArgs e | SInventory_UI_DepositList.btnExportToExcel_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CompanywisebranchDal.LoadCompany | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:16 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| CompanywisebranchDal.LoadDropDownValue | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:123 | delegates / no SQL literal |  |
| CompanywisebranchDal.LoadDepositList | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:83 | inline SQL: tblBankInfo, tblCompanyUnit, tblCompanyWiseDeposit, tblEmpGeneralInfo | @ComUnitId, @FromDate, @ToDate |
| CompanywisebranchDal.UpdateDepositInfo | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:107 | inline SQL: tblCompanyWiseDeposit | @DeleteBy, @DeleteDate, @DepositId, @IsDelete |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblBankInfo, tblCompanyUnit, tblCompanyWiseDeposit, tblEmpGeneralInfo, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): tblCompanyWiseDeposit
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCompanyWiseDeposit | DeleteBy | @DeleteBy |
| tblCompanyWiseDeposit | DeleteDate | @DeleteDate |
| tblCompanyWiseDeposit | IsDelete | @IsDelete |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler ApplyDeleteButtonState) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | You can not delete this deposit slip. | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select from date !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select sales center !! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select to date !! | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | You can not delete deposit slip !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName, RoleTypeName, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 20 methods.
- Redirect targets: CompanyWiseBranch.aspx
- User messages: Please select from date !!; Please select sales center !!; Please select to date !!; You can not delete deposit slip !!; You can not delete this deposit slip.
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: DeleteBy, DeleteDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CompanywisebranchDal; DAO: CompanyWiseDepositDao; Tables (inline): tblBankInfo, tblCompanyUnit, tblCompanyWiseDeposit, tblEmpGeneralInfo, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click excelButton1 with valid data | Handler runs; rows written to tblCompanyWiseDeposit |
| TC-05 | Click Button1 with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click Button1 with valid data | Handler runs; rows written to tblCompanyWiseDeposit |
| TC-07 | Click clearButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click clearButton with valid data | Handler runs; rows written to tblCompanyWiseDeposit |
| TC-09 | Click excelButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click excelButton1 with valid data | Handler runs; rows written to tblCompanyWiseDeposit |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DepositList.aspx`, `Solution.Web/SInventory_UI/DepositList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DepositList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 62 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DepositList.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CompanywisebranchDal.LoadCompany, CompanywisebranchDal.LoadDepositList, CompanywisebranchDal.LoadDropDownValue, CompanywisebranchDal.UpdateDepositInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
