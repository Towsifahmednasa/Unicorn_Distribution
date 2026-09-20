---
page_id: PAGE-224
module: M06
page: OrderList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-224 — OrderList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-224 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderList |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_OrderList |
| aspx size | 13294 bytes |
| Code-behind LOC | 139 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderList** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/ViewOrderDetailList.aspx`

Outbound references: `vieworderdetaillist.aspx`

## 5. Route
`~/SInventory_UI/OrderList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OrderList.aspx |
| Code-behind | Solution.Web/SInventory_UI/OrderList.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ExcelUpForOrderListBLL (Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs) |
| DAL class | ExcelUpForOrderListDAL (Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| manufacturerDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDatse | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| toTextBox | TextBox |  | no validator found |  |  |  |  |
| imgDaste | ImageButton |  | no validator found |  |  |  |  |
| Date2 | CalendarExtender |  | no validator found |  |  |  |  |
| submitButton | Button | Search | no validator found |  |  |  | onclick→submitButton_Click1 |
| refreshButton | Button | Refresh | no validator found |  |  |  | OnClick→refreshButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Manufacture Name | no validator found |  |  |  |  |
| (no id) | BoundField | Document Upload Date | no validator found |  |  |  |  |
| (no id) | BoundField | Generate Order | no validator found |  |  |  |  |
| (no id) | BoundField | Entry By | no validator found |  |  |  |  |
| (no id) | BoundField | Entry Date | no validator found |  |  |  |  |
| (no id) | TemplateField | View Detail | no validator found |  |  |  |  |
| viewImageButton | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?'); |
| (no id) | TemplateField | Generate Order | no validator found |  |  |  |  |
| OrderImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Generate Order ?');, onclick→OrderImageButton_Click |
| viewButton | Button | Start Process | no validator found |  |  |  | OnClick→viewButton_Click |
| UpdateProgress2 | UpdateProgress |  | no validator found |  |  |  |  |
| Img2 | Image |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| manufacturerDropDownList | paramter | SInventory_UI_OrderList.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| fromDateTextBox | paramter | SInventory_UI_OrderList.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| manufacturerDropDownList | LoadDropDown → ExcelUpForOrderListBLL.LoadmanufacturerName | inline SQL | ManufacName/ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| refreshButton | Button click | (postback) | SInventory_UI_OrderList.refreshButton_Click | Solution.Web/SInventory_UI/OrderList.aspx.cs:86 | — | — | — | read-only / none detected |
| editImageButton | ImageButton | return confirm('Are you sure you want to Delete ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |
| OrderImageButton | ImageButton | return confirm('Are you sure you want to Generate Order ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |
| viewButton | Button | — | viewButton_Click | — | — | — | — | UNKNOWN (handler not resolved) |

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
| SInventory_UI_OrderList.Page_Load | Solution.Web/SInventory_UI/OrderList.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_OrderList.Page_Load → ExcelUpForOrderListBLL.LoadmanufacturerName → ExcelUpForOrderListDAL.LoadmanufacturerName | no | no |
| SInventory_UI_OrderList.LoadDropDown | Solution.Web/SInventory_UI/OrderList.aspx.cs:25 |  | SInventory_UI_OrderList.LoadDropDown → ExcelUpForOrderListBLL.LoadmanufacturerName → ExcelUpForOrderListDAL.LoadmanufacturerName | no | no |
| SInventory_UI_OrderList.Load | Solution.Web/SInventory_UI/OrderList.aspx.cs:30 |  | SInventory_UI_OrderList.Load → ExcelUpForOrderListBLL.LoadOrder → ExcelUpForOrderListDAL.LoadOrder | no | no |
| SInventory_UI_OrderList.loadGridView_RowCommand | Solution.Web/SInventory_UI/OrderList.aspx.cs:45 | object sender, GridViewCommandEventArgs e | SInventory_UI_OrderList.loadGridView_RowCommand → ExcelUpForOrderListBLL.DeleteData / ExcelUpForOrderListBLL.DeleteDetailData / ExcelUpForOrderListBLL.LoadMigobyID / ExcelUpForOrderListBLL.LoadOrder → ExcelUpForOrderListDAL.DeleteData / ExcelUpForOrderListDAL. | no | no |
| SInventory_UI_OrderList.refreshButton_Click | Solution.Web/SInventory_UI/OrderList.aspx.cs:86 | object sender, EventArgs e | SInventory_UI_OrderList.refreshButton_Click | no | no |
| SInventory_UI_OrderList.submitButton_Click1 | Solution.Web/SInventory_UI/OrderList.aspx.cs:108 | object sender, EventArgs e | SInventory_UI_OrderList.submitButton_Click1 → ExcelUpForOrderListBLL.LoadMigoDate → ExcelUpForOrderListDAL.BuildOrderDateFilter / ExcelUpForOrderListDAL.LoadMigoDate | no | no |
| SInventory_UI_OrderList.LoadInitialGrid | Solution.Web/SInventory_UI/OrderList.aspx.cs:119 | GridView gridView | SInventory_UI_OrderList.LoadInitialGrid → ExcelUpForOrderListBLL.LoadMigoDate → ExcelUpForOrderListDAL.BuildOrderDateFilter / ExcelUpForOrderListDAL.ExtractQuotedValues / ExcelUpForOrderListDAL.LoadMigoDate | no | no |
| SInventory_UI_OrderList.OrderImageButton_Click | Solution.Web/SInventory_UI/OrderList.aspx.cs:124 | object sender, ImageClickEventArgs e | SInventory_UI_OrderList.OrderImageButton_Click → ExcelUpForOrderListBLL.LoadOrder / ExcelUpForOrderListBLL.TransfarOrderID_BLL → ExcelUpForOrderListDAL.LoadOrder / ExcelUpForOrderListDAL.TransfarOrderID_DAL → SP: sp_OrderGenerationFromUploadOrder | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ExcelUpForOrderListBLL.LoadmanufacturerName | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:17 |
| ExcelUpForOrderListBLL.LoadOrder | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:54 |
| ExcelUpForOrderListBLL.LoadMigobyID | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:62 |
| ExcelUpForOrderListBLL.DeleteData | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:66 |
| ExcelUpForOrderListBLL.DeleteDetailData | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:70 |
| ExcelUpForOrderListBLL.LoadMigoDate | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:75 |
| ExcelUpForOrderListBLL.TransfarOrderID_BLL | Library.BLL/SInventory_BLL/ExcelUpForOrderListBLL.cs:79 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ExcelUpForOrderListDAL.LoadmanufacturerName | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:17 | inline SQL: tblManufacturer |  |
| ExcelUpForOrderListDAL.LoadOrder | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:56 | inline SQL: tblManufacturer, tblOrderListMaster |  |
| ExcelUpForOrderListDAL.LoadMigobyID | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:65 | inline SQL: tblManufacturer, tblOrderListMaster | @OrderMasterID |
| ExcelUpForOrderListDAL.DeleteData | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:76 | inline SQL: tblOrderListMaster, tblOrderListMaster | @OrderMasterID |
| ExcelUpForOrderListDAL.DeleteDetailData | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:84 | inline SQL: tblOrderListDetail, tblOrderListDetail | @OrderMasterID |
| ExcelUpForOrderListDAL.LoadMigoDate | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:104 | inline SQL: tblManufacturer, tblOrderListMaster |  |
| ExcelUpForOrderListDAL.BuildOrderDateFilter | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:132 | delegates / no SQL literal | @FromDate, @ManufacId, @ToDate |
| ExcelUpForOrderListDAL.ExtractQuotedValues | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:161 | delegates / no SQL literal |  |
| ExcelUpForOrderListDAL.TransfarOrderID_DAL | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:112 | sp_OrderGenerationFromUploadOrder | @OrderMasterID_In |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_OrderGenerationFromUploadOrder | ExcelUpForOrderListDAL.TransfarOrderID_DAL | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblManufacturer, tblOrderListDetail, tblOrderListMaster
- Tables WRITTEN (inline SQL reachable): tblOrderListDetail, tblOrderListMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler submitButton_Click1) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Manufacturer !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: OrderID
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
- Redirect targets: ../SInventory_UI/OrderList.aspx, ../SInventory_UI/ViewOrderDetailList.aspx
- User messages: Please Select Manufacturer !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ExcelUpForOrderListBLL; DAL: ExcelUpForOrderListDAL; DAO: —; Tables (inline): tblManufacturer, tblOrderListDetail, tblOrderListMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click refreshButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click refreshButton with valid data | Handler runs; rows written to tblOrderListDetail, tblOrderListMaster |
| TC-05 | Click viewButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click viewButton with valid data | Handler runs; rows written to tblOrderListDetail, tblOrderListMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OrderList.aspx`, `Solution.Web/SInventory_UI/OrderList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/OrderList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 30 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/OrderList.aspx |
| Data access | This document §15–17 | 1 SP names, 3 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ExcelUpForOrderListDAL.BuildOrderDateFilter, ExcelUpForOrderListDAL.DeleteData, ExcelUpForOrderListDAL.DeleteDetailData, ExcelUpForOrderListDAL.ExtractQuotedValues, ExcelUpForOrderListDAL.LoadMigoDate affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
