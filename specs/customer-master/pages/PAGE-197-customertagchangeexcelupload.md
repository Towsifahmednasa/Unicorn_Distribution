---
page_id: PAGE-197
module: M05
page: CustomerTagChangeExcelUpload
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-197 — CustomerTagChangeExcelUpload

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-197 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerTagChangeExcelUpload |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CustomerTagChangeExcelUpload |
| aspx size | 12597 bytes |
| Code-behind LOC | 331 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerTagChangeExcelUpload** (Upload / Import). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `areaview.aspx`, `homepage.aspx`

## 5. Route
`~/SInventory_UI/CustomerTagChangeExcelUpload.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ExcelUpForCustTagChangeBLL (Library.BLL/SInventory_BLL/ExcelUpForCustTagChangeBLL.cs) |
| DAL class | ExcelUpForCustTagChangeDAL (Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs) |
| DAO/entity class | MigoMasterDAO (Library.DAO/SInventory_Entities/MigoMasterDAO.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
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
| (no id) | BoundField | BRANCH | no validator found |  |  |  |  |
| (no id) | BoundField | BRANCHDES | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | CUSTOMER NAME | no validator found |  |  |  |  |
| (no id) | BoundField | ADDRESS-1 | no validator found |  |  |  |  |
| (no id) | BoundField | ADDRESS-2 | no validator found |  |  |  |  |
| (no id) | BoundField | CITY | no validator found |  |  |  |  |
| (no id) | BoundField | CONTACT PERSON | no validator found |  |  |  |  |
| (no id) | BoundField | CONTACT NUMBER | no validator found |  |  |  |  |
| (no id) | BoundField | MIO-Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIOName | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | FE Code | no validator found |  |  |  |  |
| (no id) | BoundField | FE Name | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Code | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Name | no validator found |  |  |  |  |
| (no id) | BoundField | SHIPPING COND. | no validator found |  |  |  |  |
| (no id) | BoundField | SHIPPING POINT | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | TERM OF PAYMENT | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | OnClick→submitButton_Click, OnClientClick→return confirm('Are you sure you want to Save ?'); |
| cancelUploadListButton | Button | Cancel Upload | no validator found |  |  |  | OnClick→cancelUploadListButton_Click, OnClientClick→return confirm('Are you sure you want to Re-Upload ?'); |
| refreshButton | Button | Refresh | no validator found |  |  |  | OnClick→refreshButton_Click |
| HomeButton | Button | Home | no validator found |  |  |  | OnClick→HomeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| manufacturerDropDownList | MigoMasterDAO.ManufacId | SInventory_UI_CustomerTagChangeExcelUpload.submitButton_Click | ExcelUpForCustTagChangeBLL.CustomerMasterCodeGenerator, ExcelUpForCustTagChangeBLL.CustomerXLDataGridToDbByRow, ExcelUpF | ExcelUpForCustTagChangeDAL.MigoMasterParameters | @ManufacId | UNKNOWN (SP body not in repo) | UNKNOWN |
| documentDateTextBox | MigoMasterDAO.MogoDocumentDate | SInventory_UI_CustomerTagChangeExcelUpload.submitButton_Click | ExcelUpForCustTagChangeBLL.CustomerMasterCodeGenerator, ExcelUpForCustTagChangeBLL.CustomerXLDataGridToDbByRow, ExcelUpF | ExcelUpForCustTagChangeDAL.MigoMasterParameters | @MogoDocumentDate | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → ExcelUpForCustTagChangeBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnUpload | Button click | (postback) | SInventory_UI_CustomerTagChangeExcelUpload.btnUpload_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:256 | — | — | — | read-only / none detected |
| submitButton | Button click | return confirm('Are you sure you want to Save ?'); | SInventory_UI_CustomerTagChangeExcelUpload.submitButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:215 | ExcelUpForCustTagChangeBLL.CustomerMasterCodeGenerator, ExcelUpForCustTagChangeBLL.CustomerXLDataGri | ExcelUpForCustTagChangeDAL.CustomerXLDataGridToDbByRow, ExcelUpForCustTagChangeDAL.MigoMasterParamet | — | writes tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster |
| cancelUploadListButton | Button click | return confirm('Are you sure you want to Re-Upload ?'); | SInventory_UI_CustomerTagChangeExcelUpload.cancelUploadListButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:319 | — | — | — | read-only / none detected |
| refreshButton | Button click | (postback) | SInventory_UI_CustomerTagChangeExcelUpload.refreshButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:324 | — | — | — | read-only / none detected |
| HomeButton | Button click | (postback) | SInventory_UI_CustomerTagChangeExcelUpload.HomeButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:328 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: yes; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_CustomerTagChangeExcelUpload.Page_Load | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.Page_Load → ExcelUpForCustTagChangeBLL.LoadmanufacturerName → ExcelUpForCustTagChangeDAL.LoadmanufacturerName | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.LoadDropDown | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:32 |  | SInventory_UI_CustomerTagChangeExcelUpload.LoadDropDown → ExcelUpForCustTagChangeBLL.LoadmanufacturerName → ExcelUpForCustTagChangeDAL.LoadmanufacturerName | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.XLDataGridToDbByRow | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:41 | int MigoMasterID | SInventory_UI_CustomerTagChangeExcelUpload.XLDataGridToDbByRow → ExcelUpForCustTagChangeBLL.CustomerXLDataGridToDbByRow → ExcelUpForCustTagChangeDAL.CustomerXLDataGridToDbByRow | yes | no |
| SInventory_UI_CustomerTagChangeExcelUpload.loadGridView_RowDataBound | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:93 | object sender, GridViewRowEventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.loadGridView_RowDataBound | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.submitButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:215 | object sender, EventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.submitButton_Click → ExcelUpForCustTagChangeBLL.CustomerMasterCodeGenerator / ExcelUpForCustTagChangeBLL.CustomerXLDataGridToDbByRow / ExcelUpForCustTagChangeBLL.SaveCustomerUploadMasterDAL → ExcelUpForCustTagChangeDA | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.areaImageButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:252 | object sender, ImageClickEventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.areaImageButton_Click | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.btnUpload_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:256 | object sender, EventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.btnUpload_Click | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.cancelUploadListButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:319 | object sender, EventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.cancelUploadListButton_Click | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.refreshButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:324 | object sender, EventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.refreshButton_Click | no | no |
| SInventory_UI_CustomerTagChangeExcelUpload.HomeButton_Click | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs:328 | object sender, EventArgs e | SInventory_UI_CustomerTagChangeExcelUpload.HomeButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ExcelUpForCustTagChangeBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/ExcelUpForCustTagChangeBLL.cs:133 |
| ExcelUpForCustTagChangeBLL.CustomerXLDataGridToDbByRow | Library.BLL/SInventory_BLL/ExcelUpForCustTagChangeBLL.cs:27 |
| ExcelUpForCustTagChangeBLL.SaveCustomerUploadMasterDAL | Library.BLL/SInventory_BLL/ExcelUpForCustTagChangeBLL.cs:57 |
| ExcelUpForCustTagChangeBLL.CustomerMasterCodeGenerator | Library.BLL/SInventory_BLL/ExcelUpForCustTagChangeBLL.cs:93 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ExcelUpForCustTagChangeDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:110 | inline SQL: tblManufacturer |  |
| ExcelUpForCustTagChangeDAL.CustomerXLDataGridToDbByRow | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:53 | inline SQL: tblCustomerMasterTagChangeExcelFileDetail | @ADDRESS1, @ADDRESS2, @BRANCH, @BRANCHDES, @CITY, @CONTACTNUMBER, @CONTACTPERSON, @CUSTOMERNAME, @Cu |
| ExcelUpForCustTagChangeDAL.SaveCustomerUploadMasterDAL | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:92 | inline SQL: tblCustomerMasterTagChangeExcelFileMaster | @VerifyedAll |
| ExcelUpForCustTagChangeDAL.MigoMasterParameters | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:280 | delegates / no SQL literal | @EntryBy, @EntryDate, @ManufacId, @MigoMasterID, @MogoCode, @MogoDocumentDate, @StockUpload |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblManufacturer
- Tables WRITTEN (inline SQL reachable): tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCustomerMasterTagChangeExcelFileDetail | ADDRESS1 | @ADDRESS1 |
| tblCustomerMasterTagChangeExcelFileDetail | ADDRESS2 | @ADDRESS2 |
| tblCustomerMasterTagChangeExcelFileDetail | BRANCH | @BRANCH |
| tblCustomerMasterTagChangeExcelFileDetail | BRANCHDES | @BRANCHDES |
| tblCustomerMasterTagChangeExcelFileDetail | CITY | @CITY |
| tblCustomerMasterTagChangeExcelFileDetail | CONTACTNUMBER | @CONTACTNUMBER |
| tblCustomerMasterTagChangeExcelFileDetail | CONTACTPERSON | @CONTACTPERSON |
| tblCustomerMasterTagChangeExcelFileDetail | CUSTOMERNAME | @CUSTOMERNAME |
| tblCustomerMasterTagChangeExcelFileDetail | CustomerCode | @CustomerCode |
| tblCustomerMasterTagChangeExcelFileDetail | DZSMCode | @DZSMCode |
| tblCustomerMasterTagChangeExcelFileDetail | DZSMName | @DZSMName |
| tblCustomerMasterTagChangeExcelFileDetail | FECode | @FECode |
| tblCustomerMasterTagChangeExcelFileDetail | FEName | @FEName |
| tblCustomerMasterTagChangeExcelFileDetail | MIOCode | @MIOCode |
| tblCustomerMasterTagChangeExcelFileDetail | MIOName | @MIOName |
| tblCustomerMasterTagChangeExcelFileDetail | MarketName | @MarketName |
| tblCustomerMasterTagChangeExcelFileDetail | MasterID | @MasterID |
| tblCustomerMasterTagChangeExcelFileDetail | SHIPPINGCOND | @SHIPPINGCOND |
| tblCustomerMasterTagChangeExcelFileDetail | SHIPPINGPOINT | @SHIPPINGPOINT |
| tblCustomerMasterTagChangeExcelFileDetail | TERMOFPAYMENT | @TERMOFPAYMENT |
| tblCustomerMasterTagChangeExcelFileDetail | TerritoryCode | @TerritoryCode |
| tblCustomerMasterTagChangeExcelFileDetail | Verifyed | @Verifyed |
| tblCustomerMasterTagChangeExcelFileMaster | CustomerTagChangeExcelFileCode | @MogoCode |
| tblCustomerMasterTagChangeExcelFileMaster | CustomerTagChangeExcelFileDocumentDate | @MogoDocumentDate |
| tblCustomerMasterTagChangeExcelFileMaster | CustomerTagChangeExcelFileMasterID | @MigoMasterID |
| tblCustomerMasterTagChangeExcelFileMaster | EntryBy | @EntryBy |
| tblCustomerMasterTagChangeExcelFileMaster | EntryDate | @EntryDate |
| tblCustomerMasterTagChangeExcelFileMaster | ManufacId | @ManufacId |
| tblCustomerMasterTagChangeExcelFileMaster | Transfer | @StockUpload |
| tblCustomerMasterTagChangeExcelFileMaster | VerifyedAll | @VerifyedAll |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler CheckValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Field Cannot be Blank !! | UNKNOWN |
| (handler CheckValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select DocumentDate!! | UNKNOWN |
| (handler CheckValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Select Manufacturer !! | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Error Uploading.... | UNKNOWN |
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select mandatory Field!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 14 methods.
- Redirect targets: ../CommonUI/HomePage.aspx, ../SInventory_UI/CustomerTagChangeExcelUpload.aspx, AreaView.aspx
- User messages: Error Uploading....; Field Cannot be Blank !!; Please Select mandatory Field!!; Select DocumentDate!!; Select Manufacturer !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: EntryBy, EntryDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ExcelUpForCustTagChangeBLL; DAL: ExcelUpForCustTagChangeDAL; DAO: MigoMasterDAO; Tables (inline): tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster, tblManufacturer

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnUpload with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnUpload with valid data | Handler runs; rows written to tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster |
| TC-07 | Click cancelUploadListButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click cancelUploadListButton with valid data | Handler runs; rows written to tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster |
| TC-09 | Click refreshButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click refreshButton with valid data | Handler runs; rows written to tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster |
| TC-11 | Click HomeButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click HomeButton with valid data | Handler runs; rows written to tblCustomerMasterTagChangeExcelFileDetail, tblCustomerMasterTagChangeExcelFileMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx`, `Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 37 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerTagChangeExcelUpload.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ExcelUpForCustTagChangeDAL.CustomerXLDataGridToDbByRow, ExcelUpForCustTagChangeDAL.LoadmanufacturerName, ExcelUpForCustTagChangeDAL.MigoMasterParameters, ExcelUpForCustTagChangeDAL.SaveCustomerUploadMasterDAL affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
