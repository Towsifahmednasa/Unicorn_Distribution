---
page_id: PAGE-554
module: M14
page: TargetExcelUpload
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-554 — TargetExcelUpload

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-554 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | TargetExcelUpload |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_TargetExcelUpload |
| aspx size | 17365 bytes |
| Code-behind LOC | 521 |
| Inline script size | 985 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
"Monthly Target Upload" (page heading). User uploads an Excel file whose columns are `TerritoryCode, TargetValue, MonthValue, YearValue, FinancialYear`; the sheet is shown in a grid (rows failing checks are coloured), and **Submit** stores one territory target per row. CONFIRMED: `Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:228-360,368-480`.

## 3. Business Objective
Bulk-load monthly territory (MIO) sales targets from a spreadsheet instead of keying them page by page, per financial year. Downstream use: `sp_Get_TerritoryTargetList` list (PAGE-556) and, INFERRED, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/TargetExcelUploadList.aspx`

Outbound references: `areaview.aspx`, `customerexcelupload.aspx`, `homepage.aspx`, `targetexceluploadlist.aspx`

## 5. Route
`~/SInventory_UI/TargetExcelUpload.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/TargetExcelUpload.aspx |
| Code-behind | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | ExcelUpForMIGOBLL (Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs) |
| DAL class | CompanywisebranchDal (Library.DAL/SInventory_DAL/CompanywisebranchDal.cs) |
| DAL class | ExcelUpForMIGODAL (Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs) |
| DAL class | SAP_IntrigationPointDAL (Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs) |
| DAO/entity class | TargetExcelUploadDao (Library.DAO/SInventory_Entities/CompanyWiseDepositDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| (no id) | a |  | no validator found |  |  |  |  |
| id_fu | FileUpload |  | no validator found |  |  |  |  |
| IsFileUploaded | HiddenField |  | no validator found |  |  |  |  |
| lbl_up_status | Label |  | no validator found |  |  |  |  |
| btnUpload | Button | Upload | no validator found |  |  |  | OnClick→btnUpload_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, OnRowDataBound→loadGridView_RowDataBound |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Territory ID | no validator found |  |  |  |  |
| lblTerritoryCode | Label | <%# Bind("TerritoryCode") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Target Value | no validator found |  |  |  |  |
| lblTargetValue | Label | <%# Bind("TargetValue") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Month Value | no validator found |  |  |  |  |
| lblMonthValue | Label | <%# Bind("MonthValue") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Year Value | no validator found |  |  |  |  |
| lblYearValue | Label | <%# Bind("YearValue") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Financial Year | no validator found |  |  |  |  |
| lblFinancialYear | Label | <%# Bind("FinancialYear") %> | no validator found |  |  |  |  |
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
| lblTerritoryCode | TargetExcelUploadDao.TerritoryCode | SInventory_UI_TargetExcelUpload.submitButton_Click | (none — page calls DAL directly) | CompanywisebranchDal.SaveTargetValueInfo | @TerritoryCode | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblTargetValue | TargetExcelUploadDao.Value | SInventory_UI_TargetExcelUpload.submitButton_Click | (none — page calls DAL directly) | CompanywisebranchDal.SaveTargetValueInfo | @Value | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblMonthValue | TargetExcelUploadDao.MonthName | SInventory_UI_TargetExcelUpload.submitButton_Click | (none — page calls DAL directly) | CompanywisebranchDal.SaveTargetValueInfo | @MonthName | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblYearValue | TargetExcelUploadDao.YearValue | SInventory_UI_TargetExcelUpload.submitButton_Click | (none — page calls DAL directly) | CompanywisebranchDal.SaveTargetValueInfo | @YearValue | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblYearValue | TargetExcelUploadDao.YearValue | SInventory_UI_TargetExcelUpload.submitButton_Click | (none — page calls DAL directly) | SAP_IntrigationPointDAL.GetEMPDataExistCheck | @YearValue | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblFinancialYear | TargetExcelUploadDao.FinYearValue | SInventory_UI_TargetExcelUpload.submitButton_Click | (none — page calls DAL directly) | CompanywisebranchDal.SaveTargetValueInfo | @FinYearValue | UNKNOWN (SP body not in repo) | UNKNOWN |
| lblTerritoryCode | _dtTable | SInventory_UI_TargetExcelUpload.Validattion | — | SAP_IntrigationPointDAL.GetEMPDataExistCheck, SAP_IntrigationPointDAL.GetEMPMap | UNKNOWN | UNKNOWN | UNKNOWN |
| lblTerritoryCode | _dtTable | SInventory_UI_TargetExcelUpload.ExcelToGrid | — | SAP_IntrigationPointDAL.GetEMPDataExistCheck, SAP_IntrigationPointDAL.GetEMPMap | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnUpload | Button click | (postback) | SInventory_UI_TargetExcelUpload.btnUpload_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:365 | — | SAP_IntrigationPointDAL.GetEMPDataExistCheck, SAP_IntrigationPointDAL.GetEMPMap | sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode | SP executed — effect on tables UNKNOWN (body not in repo) |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_TargetExcelUpload.submitButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:229 | — | CompanywisebranchDal.SaveTargetValueInfo, SAP_IntrigationPointDAL.GetEMPDataExistCheck, SAP_Intrigat | sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode, usp_InsertTerritoryData | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| cancelButton | LinkButton click | (postback) | SInventory_UI_TargetExcelUpload.cancelButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:48 | — | — | — | read-only / none detected |
| btnUpload | Button click | (postback) | SInventory_UI_TargetExcelUpload.btnUpload_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:365 | — | SAP_IntrigationPointDAL.GetEMPDataExistCheck, SAP_IntrigationPointDAL.GetEMPMap | sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode | SP executed — effect on tables UNKNOWN (body not in repo) |
| submitButton | Button click | return confirm('Are you sure you want to Save ?'); | SInventory_UI_TargetExcelUpload.submitButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:229 | — | CompanywisebranchDal.SaveTargetValueInfo, SAP_IntrigationPointDAL.GetEMPDataExistCheck, SAP_Intrigat | sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode, usp_InsertTerritoryData | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- `btnUpload_Click -> ExcelToGrid()` (lines 368-480): saves the posted file to `~/ExcelFiles/<client file name>` (no extension whitelist, no size check, file left on disk), opens it with OLEDB (`Microsoft.Jet.OLEDB.4.0` for `.xls`, `Microsoft.ACE.OLEDB.12.0` for `.xlsx`, `HDR=YES`), reads the first sheet (`SELECT * From [<sheet>]`), drops rows whose first column is empty, binds to `loadGridView`, then for each row calls `GetEMPMap` and `GetEMPDataExistCheck` and paints the row `#ff9999` if the territory is unmapped or the record already exists; shows "`<n>` record Found!". Any exception -> alert `Please fill up all required data !!`.
- `submitButton_Click` -> `Validattion()` then per-row `SaveTargetValueInfo` (lines 228-275).
- The page also contains a large amount of unrelated legacy code (customer-master upload: `XLDataGridToDbByRow`, `CheckValidation`, `manufacturerDropDownList`, deposit grid markup) that is not reachable from the target upload buttons - see §30.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_TargetExcelUpload.Page_Load | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.Page_Load → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_TargetExcelUpload.LoadDropDown | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:37 |  | SInventory_UI_TargetExcelUpload.LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_TargetExcelUpload.cancelButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:48 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.cancelButton_Click | no | no |
| SInventory_UI_TargetExcelUpload.XLDataGridToDbByRow | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:52 | int MigoMasterID | SInventory_UI_TargetExcelUpload.XLDataGridToDbByRow → ExcelUpForMIGOBLL.CustomerXLDataGridToDbByRow → ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow → SP: sp_I_Customer | yes | no |
| SInventory_UI_TargetExcelUpload.loadGridView_RowDataBound | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:104 | object sender, GridViewRowEventArgs e | SInventory_UI_TargetExcelUpload.loadGridView_RowDataBound | no | no |
| SInventory_UI_TargetExcelUpload.submitButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:229 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.submitButton_Click → CompanywisebranchDal.SaveTargetValueInfo / SAP_IntrigationPointDAL.GetEMPDataExistCheck / SAP_IntrigationPointDAL.GetEMPMap → SP: sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode, usp_InsertTerritoryData | no | no |
| SInventory_UI_TargetExcelUpload.Validattion | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:278 |  | SInventory_UI_TargetExcelUpload.Validattion → SAP_IntrigationPointDAL.GetEMPDataExistCheck / SAP_IntrigationPointDAL.GetEMPMap → SP: sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode | no | no |
| SInventory_UI_TargetExcelUpload.areaImageButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:361 | object sender, ImageClickEventArgs e | SInventory_UI_TargetExcelUpload.areaImageButton_Click | no | no |
| SInventory_UI_TargetExcelUpload.btnUpload_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:365 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.btnUpload_Click → SAP_IntrigationPointDAL.GetEMPDataExistCheck / SAP_IntrigationPointDAL.GetEMPMap → SP: sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode | no | no |
| SInventory_UI_TargetExcelUpload.ExcelToGrid | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:369 |  | SInventory_UI_TargetExcelUpload.ExcelToGrid → SAP_IntrigationPointDAL.GetEMPDataExistCheck / SAP_IntrigationPointDAL.GetEMPMap → SP: sp_Get_Chk_EMPDataExistCheck, sp_Get_Chk_EmpCode | yes | no |
| SInventory_UI_TargetExcelUpload.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:492 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_TargetExcelUpload.cancelUploadListButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:504 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.cancelUploadListButton_Click | no | no |
| SInventory_UI_TargetExcelUpload.refreshButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:509 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.refreshButton_Click | no | no |
| SInventory_UI_TargetExcelUpload.HomeButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:513 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.HomeButton_Click | no | no |
| SInventory_UI_TargetExcelUpload.EmpCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs:518 | object sender, EventArgs e | SInventory_UI_TargetExcelUpload.EmpCetegoryAddImageButton_Click | no | no |

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
| ExcelUpForMIGODAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:108 | inline SQL: tblManufacturer |  |
| ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:53 | sp_I_Customer | @ADDRESS1, @ADDRESS2, @BRANCH, @BRANCHDES, @CITY, @CONTACTNUMBER, @CONTACTPERSON, @CUSTOMERNAME, @Cu |
| CompanywisebranchDal.SaveTargetValueInfo | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:69 | usp_InsertTerritoryData | @EntryBy, @FinYearValue, @MonthName, @TerritoryCode, @Value, @YearValue |
| SAP_IntrigationPointDAL.GetEMPMap | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:171 | sp_Get_Chk_EmpCode | @EmpCode |
| SAP_IntrigationPointDAL.GetEMPDataExistCheck | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:196 | sp_Get_Chk_EMPDataExistCheck | @EmpCode, @MonthValue, @YearValue |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_Chk_EMPDataExistCheck | SAP_IntrigationPointDAL.GetEMPDataExistCheck | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Chk_EmpCode | SAP_IntrigationPointDAL.GetEMPMap | body NOT in repository | read-like name (INFERRED) |
| sp_I_Customer | ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow | body NOT in repository | read-like name (INFERRED) |
| usp_InsertTerritoryData | CompanywisebranchDal.SaveTargetValueInfo | body NOT in repository | write-like name |

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
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TargetExcelUpload.aspx'); | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exists!! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please add target !! | UNKNOWN |
| (handler Validattion) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill up all required data !! | UNKNOWN |
| (handler ExcelToGrid) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please fill up all required data !! | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M14-021 | **Upload row validation (on Submit).** Grid must contain rows (`Please add target !!`). For every row: TerritoryCode, TargetValue, MonthValue, YearValue, FinancialYear all non-blank (else `Please fill up all required data !!` and row coloured `#ff9999`); the territory code must map to an employee (`sp_Get_Chk_EmpCode @EmpCode=<territory code>` returns >=1 row, else `Employee Not mapped!!`); and no target may already exist for (territory code, month, year) (`sp_Get_Chk_EMPDataExistCheck` returns rows -> `Already Exists!!`, row red). The first failing row stops validation. Value/month/year formats are **not** validated (all handled as strings). | `TargetExcelUpload.aspx.cs:277-360`; `SAP_IntrigationPointDAL.cs:170-243` | code-behind + SP | CONFIRMED (checks); UNKNOWN (SP internals) |
| BR-M14-022 | **Upload save semantics.** For each row `CompanywisebranchDal.SaveTargetValueInfo` -> `usp_InsertTerritoryData(@EntryBy=Session["LoginName"], @TerritoryCode, @Value, @MonthName, @YearValue, @FinYearValue)` through `ClsCommonInternalDAL.SaveAction`, which opens its own connection/command per row (no shared transaction), swallows exceptions (`catch { pk = 0; }`) and returns the `@SL` output id. The page keeps only the **last row's** id: `id > 0` -> success popup, else `Already Exist!`. Therefore a failure on any earlier row is invisible, and partial loads are possible. | `TargetExcelUpload.aspx.cs:228-275`; `CompanywisebranchDal.cs:68-80`; `ClsCommonInternalDAL.cs:590-640` | code-behind + DAL | CONFIRMED |
| BR-M14-023 | **Territory-code-as-employee-code.** The mapping check calls the employee-code SP (`@EmpCode`) with the *territory code* - so `sp_Get_Chk_EmpCode` must accept territory codes (the SAP-integration DAL names it `GetEMPMap`). Meaning of "mapped" beyond this: UNKNOWN. | `TargetExcelUpload.aspx.cs:318-345` | code-behind | INFERRED |

## 20. Permission Matrix
- Session keys read: `LoginName` (as `@EntryBy` audit string). Master page requires login. No role check (CONFIRMED).
- Anyone with menu access may load territory targets; Which roles: UNKNOWN.

## 21. Status Lifecycle
No status column. Existing (territory, month, year) blocks re-upload (BR-M14-021); the only way to change a target is the Edit page (PAGE-555) or delete on the list page (PAGE-556, `sp_DEL_TargetInfo @SL`).

## 22. Error Handling
- Upload errors: single `try/catch` -> alert `Please fill up all required data !!` (any exception - missing file, wrong provider, bad sheet - collapses to this message).
- Submit validation messages: `Please add target !!`, `Please fill up all required data !!`, `Employee Not mapped!!`, `Already Exists!!`.
- Save errors are swallowed in `SaveAction` (pk 0) -> `Already Exist!` popup.

## 23. Audit
`@EntryBy` = LoginName string passed to `usp_InsertTerritoryData`; how/whether the SP stores entry date is UNKNOWN.

## 24. Integration
Excel -> ERP import (file upload). Detailed in `specs/integrations/INT-13-excel-file-imports.md`. Uses the SAP-integration DAL class (`SAP_IntrigationPointDAL`) only for its two lookup helpers; no SAP call is made.

## 25. Reports
None; result visible in PAGE-556.

## 26. Dependencies
BLL: ExcelUpForMIGOBLL; DAL: CompanywisebranchDal, ExcelUpForMIGODAL, SAP_IntrigationPointDAL; DAO: TargetExcelUploadDao; Tables (inline): tblManufacturer

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-554-01 | Positive | Upload valid `.xlsx` with 3 new territory rows, Submit | 3 `usp_InsertTerritoryData` calls; success popup; rows visible on list page | BR-M14-021/022 |
| TC-554-02 | Negative | Row with blank TargetValue | row coloured, `Please fill up all required data !!` | BR-M14-021 |
| TC-554-03 | Negative | Territory code not mapped | `Employee Not mapped!!` | BR-M14-021/023 |
| TC-554-04 | Negative | Same territory/month/year already loaded | `Already Exists!!`, nothing saved | BR-M14-021 |
| TC-554-05 | Edge | Row 1 fails in SP (exception) but row 2 succeeds | success popup shown, row 1 silently missing | BR-M14-022 |
| TC-554-06 | Negative | Upload `.csv` or `.txt` | `oledbConn` null -> exception -> `Please fill up all required data !!` | - |
| TC-554-07 | Security | Upload file named `../x.xlsx` / `.aspx` | saved under `~/ExcelFiles/` with client name (no whitelist) - verify | §30 |
| TC-554-08 | Edge | Two sheets in workbook | only first sheet read | - |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/TargetExcelUpload.aspx`, `Solution.Web/SInventory_UI/TargetExcelUpload.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `TargetExcelUpload.aspx(.cs)` | n/a | MISSING | - |
| Atomicity | None existed | per-row commit, last id decides result | UNKNOWN | RISK | BR-M14-022 |
| Data types | None existed | strings only | SP UNKNOWN | UNKNOWN | BR-M14-021 |

## 30. Known Gaps
- Non-atomic import, last-row success flag, swallowed SP errors (BR-M14-022).
- Uploaded file saved to a web-accessible folder with the client-supplied name, no extension/content check (`TargetExcelUpload.aspx.cs:371-376`) - risk of overwrite and file drop; also file never cleaned up.
- Depends on Jet/ACE OLEDB providers (32-bit/driver install on server).
- Dead legacy code from the customer/MIGO upload pages (manufacturer drop-down, `XLDataGridToDbByRow`, deposit grid markup lines ~250-340, `CheckValidation` with `&nbsp;` checks) and wrong redirects (`refreshButton_Click` -> `CustomerExcelUpload.aspx`, `areaImageButton_Click` -> `AreaView.aspx`).
- Month/year/financial-year are free text; duplicates rely on exact string match.
- SP bodies missing: `usp_InsertTerritoryData`, `sp_Get_Chk_EmpCode`, `sp_Get_Chk_EMPDataExistCheck`, `sp_Get_TerritoryTargetList` (GAP-M14-001).

## 31. Change Impact
Shares `SAP_IntrigationPointDAL.GetEMPMap/GetEMPDataExistCheck` with nothing else in M14; `CompanywisebranchDal` is also used by deposit upload pages. Output consumed by PAGE-555/556 and possibly `sp_Get_TTargetAChivementReport*`.

