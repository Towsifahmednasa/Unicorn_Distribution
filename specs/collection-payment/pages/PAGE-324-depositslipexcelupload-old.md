---
page_id: PAGE-324
module: M09
page: DepositSlipExcelUpload_old
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-324 — DepositSlipExcelUpload_old

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-324 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | DepositSlipExcelUpload_old |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DepositSlipExcelUpload_old |
| aspx size | 19705 bytes |
| Code-behind LOC | 581 |
| Inline script size | 841 chars |
| Suspected legacy/duplicate | YES (name pattern; INFERRED) |

## 2. Purpose
INFERRED from name/markup: **DepositSlipExcelUpload_old** (Upload / Import). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CompanyWiseBranch.aspx`

Outbound references: `areaview.aspx`, `customerexcelupload.aspx`, `depositlist.aspx`, `depositslipexcelupload.aspx`, `homepage.aspx`

## 5. Route
`~/SInventory_UI/DepositSlipExcelUpload_old.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx |
| Code-behind | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ExcelUpForMIGOBLL (Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs) |
| DAL class | CompanywisebranchDal (Library.DAL/SInventory_DAL/CompanywisebranchDal.cs) |
| DAL class | ExcelUpForMIGODAL (Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs) |
| DAL class | SAP_IntrigationPointDAL (Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs) |
| DAO/entity class | CompanyWiseDepositDao (Library.DAO/SInventory_Entities/CompanyWiseDepositDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| txtDepositCode | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| txtTotalAmount | TextBox | Total Amount | no validator found |  |  |  |  |
| FilteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| (no id) | a |  | no validator found |  |  |  |  |
| id_fu | FileUpload |  | no validator found |  |  |  |  |
| IsFileUploaded | HiddenField |  | no validator found |  |  |  |  |
| lbl_up_status | Label |  | no validator found |  |  |  |  |
| btnUpload | Button | Upload | no validator found |  |  |  | OnClick→btnUpload_Click |
| lblCount | Label |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnRowDataBound→loadGridView_RowDataBound |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField | MIO Code | no validator found |  |  |  |  |
| lblTerritoryCode | Label | <%# Bind("MIOCode") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | SAP Code | no validator found |  |  |  |  |
| lblSapTerritoryCode | Label | <%# Bind("SapTerritoryCode") %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | DepositType | no validator found |  |  |  |  |
| (no id) | BoundField | Bank Name | no validator found |  |  |  |  |
| (no id) | BoundField | Account Name | no validator found |  |  |  |  |
| (no id) | BoundField | Deposit Date | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  |  |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→submitButton_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| documentDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| id_fu | FileUpload |  | no validator found |  |  |  |  |
| btnUpload | Button | Upload | no validator found |  |  |  | OnClick→btnUpload_Click |
| lbl_up_status | Label |  | no validator found |  |  |  |  |
| IsFileUploaded | HiddenField |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowDataBound→loadGridView_RowDataBound |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center | no validator found |  |  |  |  |
| (no id) | BoundField | DepositType | no validator found |  |  |  |  |
| (no id) | BoundField | Bank Name | no validator found |  |  |  |  |
| (no id) | BoundField | Account Name | no validator found |  |  |  |  |
| (no id) | BoundField | Deposit Date | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Remarks | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | OnClick→submitButton_Click, OnClientClick→return confirm('Are you sure you want to Save ?'); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| lblTerritoryCode | dtTable | SInventory_UI_DepositSlipExcelUpload_old.submitButton_Click | — | CompanywisebranchDal.SaveDepositInfo, SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_IntrigationPointDAL.GetDep | UNKNOWN | UNKNOWN | UNKNOWN |
| txtDepositCode | CompanyWiseDepositDao.DepositCode | SInventory_UI_DepositSlipExcelUpload_old.submitButton_Click | (none — page calls DAL directly) | CompanywisebranchDal.SaveDepositInfo | @DepositCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtTotalAmount | _TotalAmount | SInventory_UI_DepositSlipExcelUpload_old.Validattion | — | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo | UNKNOWN | UNKNOWN | UNKNOWN |
| lblCount | _hfAmount | SInventory_UI_DepositSlipExcelUpload_old.Validattion | — | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo | UNKNOWN | UNKNOWN | UNKNOWN |
| lblTerritoryCode | dtTable | SInventory_UI_DepositSlipExcelUpload_old.ExcelToGrid | — | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnUpload | Button click | (postback) | SInventory_UI_DepositSlipExcelUpload_old.btnUpload_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:438 | — | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerri | sp_Get_BankSAPMappingAccounTNo, sp_Get_SAP_EmpSApCodebyTerritory | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_DepositSlipExcelUpload_old.submitButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:235 | — | CompanywisebranchDal.SaveDepositInfo, SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_Intrig | sp_Get_BankSAPMappingAccounTNo, sp_Get_DepositCodeByDIC, sp_Get_SAP_EmpSApCodebyTerritory, sp_I_Diposit_New | SP executed — effect on tables UNKNOWN (body not in repo) |
| cancelButton | LinkButton click | (postback) | SInventory_UI_DepositSlipExcelUpload_old.cancelButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:54 | — | — | — | read-only / none detected |
| btnUpload | Button click | (postback) | SInventory_UI_DepositSlipExcelUpload_old.btnUpload_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:438 | — | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerri | sp_Get_BankSAPMappingAccounTNo, sp_Get_SAP_EmpSApCodebyTerritory | SP executed — effect on tables UNKNOWN (body not in repo) |
| submitButton | Button click | return confirm('Are you sure you want to Save ?'); | SInventory_UI_DepositSlipExcelUpload_old.submitButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:235 | — | CompanywisebranchDal.SaveDepositInfo, SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_Intrig | sp_Get_BankSAPMappingAccounTNo, sp_Get_DepositCodeByDIC, sp_Get_SAP_EmpSApCodebyTerritory, sp_I_Diposit_New | SP executed — effect on tables UNKNOWN (body not in repo) |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: yes; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_DepositSlipExcelUpload_old.Page_Load | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.Page_Load → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName / SAP_IntrigationPointDAL.GetDepositCodeByDIC → SP: sp_Get_DepositCodeByDIC | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.LoadDropDown | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:43 |  | SInventory_UI_DepositSlipExcelUpload_old.LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.cancelButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:54 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.cancelButton_Click | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.XLDataGridToDbByRow | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:58 | int MigoMasterID | SInventory_UI_DepositSlipExcelUpload_old.XLDataGridToDbByRow → ExcelUpForMIGOBLL.CustomerXLDataGridToDbByRow → ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow → SP: sp_I_Customer | yes | no |
| SInventory_UI_DepositSlipExcelUpload_old.loadGridView_RowDataBound | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:110 | object sender, GridViewRowEventArgs e | SInventory_UI_DepositSlipExcelUpload_old.loadGridView_RowDataBound | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.submitButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:235 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.submitButton_Click → CompanywisebranchDal.SaveDepositInfo / SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo / SAP_IntrigationPointDAL.GetDepositCodeByDIC / SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory → SP: s | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.Validattion | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:314 |  | SInventory_UI_DepositSlipExcelUpload_old.Validattion → SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo → SP: sp_Get_BankSAPMappingAccounTNo | yes | no |
| SInventory_UI_DepositSlipExcelUpload_old.areaImageButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:434 | object sender, ImageClickEventArgs e | SInventory_UI_DepositSlipExcelUpload_old.areaImageButton_Click | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.btnUpload_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:438 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.btnUpload_Click → SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo / SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory → SP: sp_Get_BankSAPMappingAccounTNo, sp_Get_SAP_EmpSApCodebyTerritory | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.ExcelToGrid | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:442 |  | SInventory_UI_DepositSlipExcelUpload_old.ExcelToGrid → SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo / SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory → SP: sp_Get_BankSAPMappingAccounTNo, sp_Get_SAP_EmpSApCodebyTerritory | yes | no |
| SInventory_UI_DepositSlipExcelUpload_old.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:557 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.cancelUploadListButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:569 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.cancelUploadListButton_Click | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.refreshButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:574 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.refreshButton_Click | no | no |
| SInventory_UI_DepositSlipExcelUpload_old.HomeButton_Click | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs:578 | object sender, EventArgs e | SInventory_UI_DepositSlipExcelUpload_old.HomeButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ExcelUpForMIGOBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:125 |
| ExcelUpForMIGOBLL.CustomerXLDataGridToDbByRow | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:27 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SAP_IntrigationPointDAL.GetDepositCodeByDIC | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:21 | sp_Get_DepositCodeByDIC | @Parm |
| ExcelUpForMIGODAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:108 | inline SQL: tblManufacturer |  |
| ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:53 | sp_I_Customer | @ADDRESS1, @ADDRESS2, @BRANCH, @BRANCHDES, @CITY, @CONTACTNUMBER, @CONTACTPERSON, @CUSTOMERNAME, @Cu |
| SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:100 | sp_Get_SAP_EmpSApCodebyTerritory | @TerritoryCode |
| CompanywisebranchDal.SaveDepositInfo | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:43 | sp_I_Diposit_New | @AIT, @AccountName, @Amount, @BankId, @BranchName, @CheckDate, @CheckNumber, @CompanyId, @DepositCod |
| SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:123 | sp_Get_BankSAPMappingAccounTNo | @BankAccNo |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_BankSAPMappingAccounTNo | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo | body NOT in repository | read-like name (INFERRED) |
| sp_Get_DepositCodeByDIC | SAP_IntrigationPointDAL.GetDepositCodeByDIC | body NOT in repository | read-like name (INFERRED) |
| sp_Get_SAP_EmpSApCodebyTerritory | SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory | body NOT in repository | read-like name (INFERRED) |
| sp_I_Customer | ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow | body NOT in repository | read-like name (INFERRED) |
| sp_I_Diposit_New | CompanywisebranchDal.SaveDepositInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblManufacturer
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler CheckValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Field Cannot be Blank !! | UNKNOWN |
| (handler CheckValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select DocumentDate!! | UNKNOWN |
| (handler CheckValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select Manufacturer !! | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','DepositSlipExcelUpload.aspx'); | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Bank Name Not Found! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please add deposit slip info !! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill up all required data !! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler ExcelToGrid) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill up all required data !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DICCompanyUnitId, LoginName, RoleTypeName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 3 of 17 methods.
- Redirect targets: ../CommonUI/HomePage.aspx, ../SInventory_UI/CustomerExcelUpload.aspx, AreaView.aspx
- User messages: ','Success','DepositSlipExcelUpload.aspx');; Already Exist!; Bank Name Not Found!; Field Cannot be Blank !!; Please add deposit slip info !!; Please fill up all required data !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
SAP_IntrigationPointDAL, sp_Get_BankSAPMappingAccounTNo, sp_Get_SAP_EmpSApCodebyTerritory

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ExcelUpForMIGOBLL; DAL: CompanywisebranchDal, ExcelUpForMIGODAL, SAP_IntrigationPointDAL; DAO: CompanyWiseDepositDao; Tables (inline): tblManufacturer

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnUpload with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnUpload with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click cancelButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click cancelButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnUpload with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnUpload with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx`, `Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 49 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DepositSlipExcelUpload_old.aspx |
| Data access | This document §15–17 | 5 SP names, 1 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 5 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- Name suggests a legacy/duplicate/test variant; confirm whether it is still reachable from the menu.

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: PAGE-075, PAGE-192, PAGE-314, PAGE-323, PAGE-554
- Changing DAL methods CompanywisebranchDal.SaveDepositInfo, ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow, ExcelUpForMIGODAL.LoadmanufacturerName, SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo, SAP_IntrigationPointDAL.GetDepositCodeByDIC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
