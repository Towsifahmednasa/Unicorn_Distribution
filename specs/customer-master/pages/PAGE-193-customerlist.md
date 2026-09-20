---
page_id: PAGE-193
module: M05
page: CustomerList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-193 — CustomerList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-193 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerList |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CustomerList |
| aspx size | 12189 bytes |
| Code-behind LOC | 198 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerList** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `custverunverexcelviewer.aspx`, `migolist.aspx`, `productedit.aspx`

## 5. Route
`~/SInventory_UI/CustomerList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerList.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerList.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ExcelUpForMIGOBLL (Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs) |
| DAL class | ExcelUpForMIGODAL (Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| submitButton | Button | Search | no validator found |  |  |  | onclick→submitButton_Click |
| refreshButton | Button | Refresh | no validator found |  |  |  | OnClick→refreshButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Manufacture Name | no validator found |  |  |  |  |
| (no id) | BoundField | Document Upload Date | no validator found |  |  |  |  |
| (no id) | BoundField | Transfer to Customer Master | no validator found |  |  |  |  |
| (no id) | BoundField | Entry By | no validator found |  |  |  |  |
| (no id) | BoundField | Entry Date | no validator found |  |  |  |  |
| (no id) | TemplateField | View Detail | no validator found |  |  |  |  |
| viewImageButton | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?'); |
| (no id) | TemplateField | Verify | no validator found |  |  |  |  |
| ImageButton1 | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Verified | no validator found |  |  |  |  |
| verifiedLinkButton | LinkButton |  | no validator found |  |  |  | onclick→verifiedLinkButton_Click |
| (no id) | TemplateField | Unverified | no validator found |  |  |  |  |
| unverifiedLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→unverifiedLinkButton_OnClick |
| (no id) | TemplateField | Transfer to Customer Master | no validator found |  |  |  |  |
| transferImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Transfer ?');, onclick→transferImageButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| manufacturerDropDownList | paramter | SInventory_UI_CustomerList.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| refreshButton | Button click | (postback) | SInventory_UI_CustomerList.refreshButton_Click | Solution.Web/SInventory_UI/CustomerList.aspx.cs:110 | ExcelUpForMIGOBLL.GetUnVerifyedData, ExcelUpForMIGOBLL.GetVerifyedData, ExcelUpForMIGOBLL.LoadCustom | ExcelUpForMIGODAL.ExtractFilterValue, ExcelUpForMIGODAL.GetUnVerifyedData, ExcelUpForMIGODAL.GetVeri | — | read-only / none detected |
| editImageButton | ImageButton | return confirm('Are you sure you want to Delete ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |
| unverifiedLinkButton | LinkButton click | (postback) | SInventory_UI_CustomerList.unverifiedLinkButton_OnClick | Solution.Web/SInventory_UI/CustomerList.aspx.cs:190 | — | — | — | read-only / none detected |
| transferImageButton | ImageButton | return confirm('Are you sure you want to Transfer ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_CustomerList.Page_Load | Solution.Web/SInventory_UI/CustomerList.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_CustomerList.Page_Load → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_CustomerList.LoadDropDown | Solution.Web/SInventory_UI/CustomerList.aspx.cs:26 |  | SInventory_UI_CustomerList.LoadDropDown → ExcelUpForMIGOBLL.LoadmanufacturerName → ExcelUpForMIGODAL.LoadmanufacturerName | no | no |
| SInventory_UI_CustomerList.LoadInitialGrid | Solution.Web/SInventory_UI/CustomerList.aspx.cs:30 | GridView gridView | SInventory_UI_CustomerList.LoadInitialGrid → ExcelUpForMIGOBLL.GetUnVerifyedData / ExcelUpForMIGOBLL.GetVerifyedData / ExcelUpForMIGOBLL.LoadCustomer → ExcelUpForMIGODAL.ExtractFilterValue / ExcelUpForMIGODAL.ExtractQuotedValues / ExcelUpForMIGODAL.GetUnVerify | no | no |
| SInventory_UI_CustomerList.MigoLoad | Solution.Web/SInventory_UI/CustomerList.aspx.cs:36 |  | SInventory_UI_CustomerList.MigoLoad → ExcelUpForMIGOBLL.LoadMigo → ExcelUpForMIGODAL.LoadMigo | no | no |
| SInventory_UI_CustomerList.loadGridView_RowCommand | Solution.Web/SInventory_UI/CustomerList.aspx.cs:68 | object sender, GridViewCommandEventArgs e | SInventory_UI_CustomerList.loadGridView_RowCommand → ExcelUpForMIGOBLL.DeleteCustomerData / ExcelUpForMIGOBLL.DeleteCustomerDetailData / ExcelUpForMIGOBLL.GetUnVerifyedData / ExcelUpForMIGOBLL.GetVerifyedData / ExcelUpForMIGOBLL.LoadCustomer / ExcelUpFo → Exce | no | no |
| SInventory_UI_CustomerList.refreshButton_Click | Solution.Web/SInventory_UI/CustomerList.aspx.cs:110 | object sender, EventArgs e | SInventory_UI_CustomerList.refreshButton_Click → ExcelUpForMIGOBLL.GetUnVerifyedData / ExcelUpForMIGOBLL.GetVerifyedData / ExcelUpForMIGOBLL.LoadCustomer → ExcelUpForMIGODAL.ExtractFilterValue / ExcelUpForMIGODAL.GetUnVerifyedData / ExcelUpForMIGODAL.GetVerify | no | no |
| SInventory_UI_CustomerList.submitButton_Click | Solution.Web/SInventory_UI/CustomerList.aspx.cs:126 | object sender, EventArgs e | SInventory_UI_CustomerList.submitButton_Click → ExcelUpForMIGOBLL.GetUnVerifyedData / ExcelUpForMIGOBLL.GetVerifyedData / ExcelUpForMIGOBLL.LoadCustomer → ExcelUpForMIGODAL.ExtractFilterValue / ExcelUpForMIGODAL.GetUnVerifyedData / ExcelUpForMIGODAL.GetVerifye | no | no |
| SInventory_UI_CustomerList.PlaceVerfiyUnverify | Solution.Web/SInventory_UI/CustomerList.aspx.cs:138 |  | SInventory_UI_CustomerList.PlaceVerfiyUnverify → ExcelUpForMIGOBLL.GetUnVerifyedData / ExcelUpForMIGOBLL.GetVerifyedData → ExcelUpForMIGODAL.GetUnVerifyedData / ExcelUpForMIGODAL.GetVerificationCount / ExcelUpForMIGODAL.GetVerifyedData | no | no |
| SInventory_UI_CustomerList.transferImageButton_Click | Solution.Web/SInventory_UI/CustomerList.aspx.cs:148 | object sender, ImageClickEventArgs e | SInventory_UI_CustomerList.transferImageButton_Click → ExcelUpForMIGOBLL.GetUnVerifyedData / ExcelUpForMIGOBLL.GetVerifyedData / ExcelUpForMIGOBLL.LoadCustomer / ExcelUpForMIGOBLL.TransfarCustomer_BLL → ExcelUpForMIGODAL.ExtractFilterValue / ExcelUpForMIGODAL. | no | no |
| SInventory_UI_CustomerList.verifiedLinkButton_Click | Solution.Web/SInventory_UI/CustomerList.aspx.cs:179 | object sender, EventArgs e | SInventory_UI_CustomerList.verifiedLinkButton_Click | no | no |
| SInventory_UI_CustomerList.unverifiedLinkButton_OnClick | Solution.Web/SInventory_UI/CustomerList.aspx.cs:190 | object sender, EventArgs e | SInventory_UI_CustomerList.unverifiedLinkButton_OnClick | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ExcelUpForMIGOBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:125 |
| ExcelUpForMIGOBLL.LoadCustomer | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:121 |
| ExcelUpForMIGOBLL.GetVerifyedData | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:176 |
| ExcelUpForMIGOBLL.GetUnVerifyedData | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:180 |
| ExcelUpForMIGOBLL.LoadMigo | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:109 |
| ExcelUpForMIGOBLL.DeleteCustomerData | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:168 |
| ExcelUpForMIGOBLL.DeleteCustomerDetailData | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:172 |
| ExcelUpForMIGOBLL.VerifyCustomerBLL | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:53 |
| ExcelUpForMIGOBLL.TransfarCustomer_BLL | Library.BLL/SInventory_BLL/ExcelUpForMIGOBLL.cs:161 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ExcelUpForMIGODAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:108 | inline SQL: tblManufacturer |  |
| ExcelUpForMIGODAL.LoadCustomer | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:210 | inline SQL: tblCustomerMasterExcelFileMaster, tblManufacturer | @ManufacId |
| ExcelUpForMIGODAL.ExtractFilterValue | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:349 | delegates / no SQL literal |  |
| ExcelUpForMIGODAL.ExtractQuotedValues | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:355 | delegates / no SQL literal |  |
| ExcelUpForMIGODAL.GetVerifyedData | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:260 | delegates / no SQL literal |  |
| ExcelUpForMIGODAL.GetVerificationCount | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:300 | inline SQL: tblCustomerMasterExcelFileDetail | @MasterID, @Verifyed |
| ExcelUpForMIGODAL.GetUnVerifyedData | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:264 | delegates / no SQL literal |  |
| ExcelUpForMIGODAL.LoadMigo | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:114 | inline SQL: tblMIGOMaster, tblManufacturer |  |
| ExcelUpForMIGODAL.DeleteCustomerData | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:238 | inline SQL: tblCustomerMasterExcelFileMaster, tblCustomerMasterExcelFileMaster | @CustomerMasterExcelFileMasterID |
| ExcelUpForMIGODAL.DeleteCustomerDetailData | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:246 | inline SQL: tblCustomerMasterExcelFileDetail, tblCustomerMasterExcelFileDetail | @MasterID |
| ExcelUpForMIGODAL.VerifyCustomerDAL | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:99 | sp_VerifyCustomer | @CustomerMasterExcelFileMasterID |
| ExcelUpForMIGODAL.TransfarCustomer_DAL | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:254 | sp_CustomerTransfer | @CustomerMasterExcelFileMasterID |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CustomerTransfer | ExcelUpForMIGODAL.TransfarCustomer_DAL | body NOT in repository | read-like name (INFERRED) |
| sp_VerifyCustomer | ExcelUpForMIGODAL.VerifyCustomerDAL | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCustomerMasterExcelFileDetail, tblCustomerMasterExcelFileMaster, tblMIGODetail, tblMIGOMaster, tblManufacturer
- Tables WRITTEN (inline SQL reachable): tblCustomerMasterExcelFileDetail, tblCustomerMasterExcelFileMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler submitButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Manufacturer !! | UNKNOWN |
| (handler transferImageButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Verify all Customer!!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 17 methods.
- Redirect targets: none
- User messages: Please Select Manufacturer !!; Please Verify all Customer!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ExcelUpForMIGOBLL; DAL: ExcelUpForMIGODAL; DAO: —; Tables (inline): tblCustomerMasterExcelFileDetail, tblCustomerMasterExcelFileMaster, tblMIGODetail, tblMIGOMaster, tblManufacturer

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click refreshButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click refreshButton with valid data | Handler runs; rows written to tblCustomerMasterExcelFileDetail, tblCustomerMasterExcelFileMaster |
| TC-05 | Click unverifiedLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click unverifiedLinkButton with valid data | Handler runs; rows written to tblCustomerMasterExcelFileDetail, tblCustomerMasterExcelFileMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerList.aspx`, `Solution.Web/SInventory_UI/CustomerList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerList.aspx |
| Data access | This document §15–17 | 2 SP names, 5 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ExcelUpForMIGODAL.DeleteCustomerData, ExcelUpForMIGODAL.DeleteCustomerDetailData, ExcelUpForMIGODAL.ExtractFilterValue, ExcelUpForMIGODAL.ExtractQuotedValues, ExcelUpForMIGODAL.GetUnVerifyedData affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
