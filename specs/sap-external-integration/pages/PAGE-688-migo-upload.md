---
page_id: PAGE-688
module: M17
page: MIGO_Upload
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-688 — MIGO_Upload

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-688 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | MIGO_Upload |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_MIGO_Upload |
| aspx size | 11470 bytes |
| Code-behind LOC | 230 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MIGO_Upload** (Upload / Import). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: SAP hand-off pages, MIGO, provider dropout integration, API-customer import, mobile-app data-load web methods.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `areaview.aspx`, `homepage.aspx`

## 5. Route
`~/SInventory_UI/MIGO_Upload.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MIGO_Upload.aspx |
| Code-behind | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ExcelUpForMIGOBLL (Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs) |
| DAL class | ExcelUpForMIGODAL (Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs) |
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
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | ShipToParty | no validator found |  |  |  |  |
| (no id) | BoundField | PO.No | no validator found |  |  |  |  |
| (no id) | BoundField | PO.Date | no validator found |  |  |  |  |
| (no id) | BoundField | ItemNo | no validator found |  |  |  |  |
| (no id) | BoundField | OrderDocNo | no validator found |  |  |  |  |
| (no id) | BoundField | OrderDocDate | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryDocNo | no validator found |  |  |  |  |
| (no id) | BoundField | DeliveryDocDate | no validator found |  |  |  |  |
| (no id) | BoundField | LMID | no validator found |  |  |  |  |
| (no id) | BoundField | LMIDDescription | no validator found |  |  |  |  |
| (no id) | BoundField | Batch | no validator found |  |  |  |  |
| (no id) | BoundField | ExpDate | no validator found |  |  |  |  |
| (no id) | BoundField | MfgDate | no validator found |  |  |  |  |
| (no id) | BoundField | Qty | no validator found |  |  |  |  |
| (no id) | BoundField | VATChallan | no validator found |  |  |  |  |
| (no id) | BoundField | BilltoParty | no validator found |  |  |  |  |
| (no id) | BoundField | InvoiceNo | no validator found |  |  |  |  |
| (no id) | BoundField | InvoiceDate | no validator found |  |  |  |  |
| (no id) | BoundField | CaseNoofShipper | no validator found |  |  |  |  |
| (no id) | BoundField | VAT | no validator found |  |  |  |  |
| (no id) | BoundField | Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Total | no validator found |  |  |  |  |
| (no id) | BoundField | TransportNo | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | OnClick→submitButton_Click, OnClientClick→return confirm('Are you sure you want to Save ?'); |
| cancelUploadListButton | Button | Cancel Upload | no validator found |  |  |  | OnClick→cancelUploadListButton_Click, OnClientClick→return confirm('Are you sure you want to Re-Upload ?'); |
| refreshButton | Button | Refresh | no validator found |  |  |  | OnClick→refreshButton_Click |
| HomeButton | Button | Home | no validator found |  |  |  | OnClick→HomeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| manufacturerDropDownList | MigoMasterDAO.ManufacId | SInventory_UI_MIGO_Upload.submitButton_Click | ExcelUpForMIGOBLL.MigoMasterCodeGenerator, ExcelUpForMIGOBLL.SaveMigo, ExcelUpForMIGOBLL.XLDataGridToDbByRow | ExcelUpForMIGODAL.MigoMasterParameters | @ManufacId | UNKNOWN (SP body not in repo) | UNKNOWN |
| documentDateTextBox | MigoMasterDAO.MogoDocumentDate | SInventory_UI_MIGO_Upload.submitButton_Click | ExcelUpForMIGOBLL.MigoMasterCodeGenerator, ExcelUpForMIGOBLL.SaveMigo, ExcelUpForMIGOBLL.XLDataGridToDbByRow | ExcelUpForMIGODAL.MigoMasterParameters | @MogoDocumentDate | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnUpload | Button click | (postback) | SInventory_UI_MIGO_Upload.btnUpload_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:155 | — | — | — | read-only / none detected |
| submitButton | Button click | return confirm('Are you sure you want to Save ?'); | SInventory_UI_MIGO_Upload.submitButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:114 | ExcelUpForMIGOBLL.MigoMasterCodeGenerator, ExcelUpForMIGOBLL.SaveMigo, ExcelUpForMIGOBLL.XLDataGridT | ExcelUpForMIGODAL.MigoMasterParameters, ExcelUpForMIGODAL.SaveMigoDAL, ExcelUpForMIGODAL.XLDataGridT | — | writes tblMIGODetail, tblMIGOMaster |
| cancelUploadListButton | Button click | return confirm('Are you sure you want to Re-Upload ?'); | SInventory_UI_MIGO_Upload.cancelUploadListButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:218 | — | — | — | read-only / none detected |
| refreshButton | Button click | (postback) | SInventory_UI_MIGO_Upload.refreshButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:223 | — | — | — | read-only / none detected |
| HomeButton | Button click | (postback) | SInventory_UI_MIGO_Upload.HomeButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:227 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: yes; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_MIGO_Upload.Page_Load | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_MIGO_Upload.Page_Load → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_MIGO_Upload.LoadDropDown | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:31 |  | SInventory_UI_MIGO_Upload.LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_MIGO_Upload.XLDataGridToDbByRow | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:40 | int MigoMasterID | SInventory_UI_MIGO_Upload.XLDataGridToDbByRow → ExcelUpForMIGOBLL.XLDataGridToDbByRow → ExcelUpForMIGODAL.XLDataGridToDbByRow | yes | no |
| SInventory_UI_MIGO_Upload.submitButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:114 | object sender, EventArgs e | SInventory_UI_MIGO_Upload.submitButton_Click → ExcelUpForMIGOBLL.MigoMasterCodeGenerator / ExcelUpForMIGOBLL.SaveMigo / ExcelUpForMIGOBLL.XLDataGridToDbByRow → ExcelUpForMIGODAL.MigoMasterParameters / ExcelUpForMIGODAL.SaveMigoDAL / ExcelUpForMIGODAL.XLDataGri | no | no |
| SInventory_UI_MIGO_Upload.areaImageButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:151 | object sender, ImageClickEventArgs e | SInventory_UI_MIGO_Upload.areaImageButton_Click | no | no |
| SInventory_UI_MIGO_Upload.btnUpload_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:155 | object sender, EventArgs e | SInventory_UI_MIGO_Upload.btnUpload_Click | no | no |
| SInventory_UI_MIGO_Upload.cancelUploadListButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:218 | object sender, EventArgs e | SInventory_UI_MIGO_Upload.cancelUploadListButton_Click | no | no |
| SInventory_UI_MIGO_Upload.refreshButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:223 | object sender, EventArgs e | SInventory_UI_MIGO_Upload.refreshButton_Click | no | no |
| SInventory_UI_MIGO_Upload.HomeButton_Click | Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs:227 | object sender, EventArgs e | SInventory_UI_MIGO_Upload.HomeButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ExcelUpForMIGOBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:125 |
| ExcelUpForMIGOBLL.XLDataGridToDbByRow | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:19 |
| ExcelUpForMIGOBLL.SaveMigo | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:33 |
| ExcelUpForMIGOBLL.MigoMasterCodeGenerator | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:78 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ExcelUpForMIGODAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:108 | inline SQL: tblManufacturer |  |
| ExcelUpForMIGODAL.XLDataGridToDbByRow | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:19 | inline SQL: tblMIGODetail | @Amount, @Batch, @BilltoParty, @CaseNoofShipper, @DeliveryDocDate, @DeliveryDocNo, @ExpDate, @Invoic |
| ExcelUpForMIGODAL.SaveMigoDAL | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:83 | inline SQL: tblMIGOMaster |  |
| ExcelUpForMIGODAL.MigoMasterParameters | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:277 | delegates / no SQL literal | @EntryBy, @EntryDate, @ManufacId, @MigoMasterID, @MogoCode, @MogoDocumentDate, @StockUpload |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblManufacturer
- Tables WRITTEN (inline SQL reachable): tblMIGODetail, tblMIGOMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblMIGODetail | Amount | @Amount |
| tblMIGODetail | Batch | @Batch |
| tblMIGODetail | BilltoParty | @BilltoParty |
| tblMIGODetail | CaseNoofShipper | @CaseNoofShipper |
| tblMIGODetail | DeliveryDocDate | @DeliveryDocDate |
| tblMIGODetail | DeliveryDocNo | @DeliveryDocNo |
| tblMIGODetail | ExpDate | @ExpDate |
| tblMIGODetail | InvoiceDate | @InvoiceDate |
| tblMIGODetail | InvoiceNo | @InvoiceNo |
| tblMIGODetail | ItemNo | @ItemNo |
| tblMIGODetail | LMID | @LMID |
| tblMIGODetail | LMIDDescription | @LMIDDescription |
| tblMIGODetail | MfgDate | @MfgDate |
| tblMIGODetail | MigoMasterID | @MigoMasterID |
| tblMIGODetail | OrderDocDate | @OrderDocDate |
| tblMIGODetail | OrderDocNo | @OrderDocNo |
| tblMIGODetail | PODate | @PODate |
| tblMIGODetail | PONo | @PONo |
| tblMIGODetail | Qty | @Qty |
| tblMIGODetail | ShipToParty | @ShipToParty |
| tblMIGODetail | Total | @Total |
| tblMIGODetail | TransportNo | @TransportNo |
| tblMIGODetail | VAT | @VAT |
| tblMIGODetail | VATChallan | @VATChallan |
| tblMIGOMaster | EntryBy | @EntryBy |
| tblMIGOMaster | EntryDate | @EntryDate |
| tblMIGOMaster | ManufacId | @ManufacId |
| tblMIGOMaster | MigoMasterID | @MigoMasterID |
| tblMIGOMaster | MogoCode | @MogoCode |
| tblMIGOMaster | MogoDocumentDate | @MogoDocumentDate |
| tblMIGOMaster | StockUpload | @StockUpload |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
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
- try/catch present in 1 of 13 methods.
- Redirect targets: ../CommonUI/HomePage.aspx, ../SInventory_UI/MIGO_Upload.aspx, AreaView.aspx
- User messages: Error Uploading....; Please Select mandatory Field!!; Select DocumentDate!!; Select Manufacturer !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: EntryBy, EntryDate. Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ExcelUpForMIGOBLL; DAL: ExcelUpForMIGODAL; DAO: MigoMasterDAO; Tables (inline): tblMIGODetail, tblMIGOMaster, tblManufacturer

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnUpload with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnUpload with valid data | Handler runs; rows written to tblMIGODetail, tblMIGOMaster |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tblMIGODetail, tblMIGOMaster |
| TC-07 | Click cancelUploadListButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click cancelUploadListButton with valid data | Handler runs; rows written to tblMIGODetail, tblMIGOMaster |
| TC-09 | Click refreshButton with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click refreshButton with valid data | Handler runs; rows written to tblMIGODetail, tblMIGOMaster |
| TC-11 | Click HomeButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click HomeButton with valid data | Handler runs; rows written to tblMIGODetail, tblMIGOMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MIGO_Upload.aspx`, `Solution.Web/SInventory_UI/MIGO_Upload.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MIGO_Upload.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 40 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MIGO_Upload.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M17 (SAP, Mobile-App & External Integration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ExcelUpForMIGODAL.LoadmanufacturerName, ExcelUpForMIGODAL.MigoMasterParameters, ExcelUpForMIGODAL.SaveMigoDAL, ExcelUpForMIGODAL.XLDataGridToDbByRow affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
