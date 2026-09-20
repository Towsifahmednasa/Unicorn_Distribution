---
page_id: PAGE-284
module: M08
page: BacktoReturnPage
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-284 — BacktoReturnPage

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-284 |
| Module | M08 — Sales Return & Invoice Return (INFERRED from folder + file name) |
| Page name | BacktoReturnPage |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_BacktoReturnPage |
| aspx size | 9817 bytes |
| Code-behind LOC | 252 |
| Inline script size | 816 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **BacktoReturnPage** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sales returns, return invoices, delivery returns, replacement notes, return posting.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `custmasterentry.aspx`, `customermasterviewer.aspx`, `login.aspx`, `orderedit.aspx`

## 5. Route
`~/SInventory_UI/BacktoReturnPage.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/BacktoReturnPage.aspx |
| Code-behind | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→salesCenterDropDownList_SelectedIndexChanged |
| rootDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→rootDropDownList_SelectedIndexChanged |
| custcodenameTextBox | TextBox |  | no validator found |  | Visible=false |  |  |
| ddlOrder | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | LinkButton |  | no validator found |  |  |  | OnClientClick→return sweetAlertConfirm_Delete(this);, OnClick→Button1_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | SC Code | no validator found |  |  |  |  |
| (no id) | BoundField | SC Name | no validator found |  |  |  |  |
| (no id) | BoundField | Order No | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Gross Value | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| edistIasdasdmageButton | LinkButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| edistImageButton | LinkButton |  | no validator found |  |  |  | OnClientClick→return sweetAlertConfirm_Delete(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | dt | SInventory_UI_BacktoReturnPage.salesCenterDropDownList_SelectedIndexChanged | — | SeedDataDAL.GetRouteInfoforBacktoReturn | UNKNOWN | UNKNOWN | UNKNOWN |
| custcodenameTextBox | aDataTable | SInventory_UI_BacktoReturnPage.CustomerMasterLoad | CustomerMasterBLL.LoadOrderView | CustomerMasterDAL.LoadOrderView | UNKNOWN | UNKNOWN | UNKNOWN |
| custcodenameTextBox | aDataTable | SInventory_UI_BacktoReturnPage.CustomerMasterLoad2 | CustomerMasterBLL.LoadOrderView | CustomerMasterDAL.LoadOrderView | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlOrder | sts | SInventory_UI_BacktoReturnPage.Button1_Click | CustomerMasterBLL.UpdateBacktoReturnPage | CustomerMasterDAL.UpdateBacktoReturnPage | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenterDropDownList | dt | SInventory_UI_BacktoReturnPage.rootDropDownList_SelectedIndexChanged | — | SeedDataDAL.GetOrderNoforReturn | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| rootDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlOrder | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| searchButton | LinkButton click | return sweetAlertConfirm_Delete(this); | SInventory_UI_BacktoReturnPage.Button1_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:181 | CustomerMasterBLL.UpdateBacktoReturnPage | CustomerMasterDAL.UpdateBacktoReturnPage | sp_UpdateBacktoReturnPage | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
|  | LinkButton click | (postback) | SInventory_UI_BacktoReturnPage.Unnamed_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:207 | — | — | — | read-only / none detected |
| edistImageButton | LinkButton | return sweetAlertConfirm_Delete(this); | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_BacktoReturnPage.Page_Load | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:24 | object sender, EventArgs e | SInventory_UI_BacktoReturnPage.Page_Load → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC / SeedDataDAL.GetRouteInfoforBacktoReturn → SP: sp_Get_RouteInfoforBacktoReturn | no | no |
| SInventory_UI_BacktoReturnPage.DropDownlist | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:33 |  | SInventory_UI_BacktoReturnPage.DropDownlist → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC / SeedDataDAL.GetRouteInfoforBacktoReturn → SP: sp_Get_RouteInfoforBacktoReturn | yes | no |
| SInventory_UI_BacktoReturnPage.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:51 | object sender, EventArgs e | SInventory_UI_BacktoReturnPage.salesCenterDropDownList_SelectedIndexChanged → SeedDataDAL.GetRouteInfoforBacktoReturn → SP: sp_Get_RouteInfoforBacktoReturn | yes | no |
| SInventory_UI_BacktoReturnPage.LoadDropDown | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:85 |  | SInventory_UI_BacktoReturnPage.LoadDropDown → CommonDataLoad.GetOrderInvoiceIsZero → SP: sp_GetOrderInvoiceIsZero | yes | no |
| SInventory_UI_BacktoReturnPage.CustomerMasterLoad | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:113 |  | SInventory_UI_BacktoReturnPage.CustomerMasterLoad → CustomerMasterBLL.LoadOrderView → CustomerMasterDAL.LoadOrderView | no | no |
| SInventory_UI_BacktoReturnPage.CustomerMasterLoad2 | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:126 |  | SInventory_UI_BacktoReturnPage.CustomerMasterLoad2 → CustomerMasterBLL.LoadOrderView → CustomerMasterDAL.LoadOrderView | no | no |
| SInventory_UI_BacktoReturnPage.loadGridView_RowCommand | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:133 | object sender, GridViewCommandEventArgs e | SInventory_UI_BacktoReturnPage.loadGridView_RowCommand → CustomerMasterBLL.DeleteRequisition / CustomerMasterBLL.LoadOrderView → CustomerMasterDAL.DeleteRequisition / CustomerMasterDAL.LoadOrderView → SP: sp_DeleteOrder | no | no |
| SInventory_UI_BacktoReturnPage.CustMasterNewImageButton_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:165 | object sender, ImageClickEventArgs e | SInventory_UI_BacktoReturnPage.CustMasterNewImageButton_Click | no | no |
| SInventory_UI_BacktoReturnPage.areaReloadImageButton_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:169 | object sender, ImageClickEventArgs e | SInventory_UI_BacktoReturnPage.areaReloadImageButton_Click → CustomerMasterBLL.LoadOrderView → CustomerMasterDAL.LoadOrderView | no | no |
| SInventory_UI_BacktoReturnPage.rptImageButton_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:174 | object sender, ImageClickEventArgs e | SInventory_UI_BacktoReturnPage.rptImageButton_Click | no | no |
| SInventory_UI_BacktoReturnPage.Button1_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:181 | object sender, EventArgs e | SInventory_UI_BacktoReturnPage.Button1_Click → CustomerMasterBLL.UpdateBacktoReturnPage → CustomerMasterDAL.UpdateBacktoReturnPage → SP: sp_UpdateBacktoReturnPage | no | no |
| SInventory_UI_BacktoReturnPage.Unnamed_Click | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:207 | object sender, EventArgs e | SInventory_UI_BacktoReturnPage.Unnamed_Click | no | no |
| SInventory_UI_BacktoReturnPage.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:212 | object sender, EventArgs e | SInventory_UI_BacktoReturnPage.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_BacktoReturnPage.rootDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs:224 | object sender, EventArgs e | SInventory_UI_BacktoReturnPage.rootDropDownList_SelectedIndexChanged → SeedDataDAL.GetOrderNoforReturn → SP: sp_Get_OrderNoforReturn, sp_Get_OrderNoforReturnDistributionRouteId | yes | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| CustomerMasterBLL.LoadOrderView | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:94 |
| CustomerMasterBLL.DeleteRequisition | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:81 |
| CustomerMasterBLL.UpdateBacktoReturnPage | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:85 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| SeedDataDAL.GetRouteInfoforBacktoReturn | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:507 | sp_Get_RouteInfoforBacktoReturn | @id |
| CommonDataLoad.GetOrderInvoiceIsZero | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1955 | sp_GetOrderInvoiceIsZero |  |
| CustomerMasterDAL.LoadOrderView | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:90 | inline SQL: tblOrder | @OrderCode |
| CustomerMasterDAL.DeleteRequisition | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:335 | sp_DeleteOrder | @LoginName, @OrdID |
| CustomerMasterDAL.UpdateBacktoReturnPage | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:343 | sp_UpdateBacktoReturnPage | @LoginName, @OrdID |
| SeedDataDAL.GetOrderNoforReturn | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:566 | sp_Get_OrderNoforReturn | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_DeleteOrder | CustomerMasterDAL.DeleteRequisition | body NOT in repository | write-like name |
| sp_GetOrderInvoiceIsZero | CommonDataLoad.GetOrderInvoiceIsZero | body NOT in repository | read-like name (INFERRED) |
| sp_Get_OrderNoforReturn | SeedDataDAL.GetOrderNoforReturn | body NOT in repository | read-like name (INFERRED) |
| sp_Get_OrderNoforReturnDistributionRouteId | SeedDataDAL.GetOrderNoforReturn | body NOT in repository | read-like name (INFERRED) |
| sp_Get_RouteInfoforBacktoReturn | SeedDataDAL.GetRouteInfoforBacktoReturn | body NOT in repository | read-like name (INFERRED) |
| sp_UpdateBacktoReturnPage | CustomerMasterDAL.UpdateBacktoReturnPage | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblOrder, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler salesCenterDropDownList_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler LoadDropDown) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Cant delete!!Proforma Already Generated | UNKNOWN |
| (handler Button1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','BacktoReturnPage.aspx'); | UNKNOWN |
| (handler Button1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Can not delete!! | UNKNOWN |
| (handler Button1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Button1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Button1_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler rootDropDownList_SelectedIndexChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 4 of 16 methods.
- Redirect targets: BacktoReturnPage.aspx, CustMasterEntry.aspx
- User messages: ','Success','BacktoReturnPage.aspx');; Can not delete!!; Cant delete!!Proforma Already Generated; Please Select From List; form-select form-select-sm mb-3 mySelect2; form-select form-select-sm mb-3 mySelect2 is-invalid
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterBLL, OrderInfoBLL; DAL: CommonDataLoad, CustomerMasterDAL, OrderInfoDAL, SeedDataDAL; DAO: —; Tables (inline): tblCompanyUnit, tblOrder, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click searchButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/BacktoReturnPage.aspx`, `Solution.Web/SInventory_UI/BacktoReturnPage.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/BacktoReturnPage.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 22 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/BacktoReturnPage.aspx |
| Data access | This document §15–17 | 6 SP names, 3 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 6 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M08 (Sales Return & Invoice Return).
- Other pages sharing this page's stored procedures: PAGE-222
- Changing DAL methods CommonDataLoad.GetOrderInvoiceIsZero, CustomerMasterDAL.DeleteRequisition, CustomerMasterDAL.LoadOrderView, CustomerMasterDAL.UpdateBacktoReturnPage, OrderInfoDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
