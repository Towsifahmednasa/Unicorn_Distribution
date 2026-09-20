---
page_id: PAGE-231
module: M06
page: PendingOrder
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-231 — PendingOrder

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-231 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | PendingOrder |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_PendingOrder |
| aspx size | 12713 bytes |
| Code-behind LOC | 72 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **PendingOrder** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/PendingOrder.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/PendingOrder.aspx |
| Code-behind | Solution.Web/SInventory_UI/PendingOrder.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| BLL class | PendingOrderBll (Library.BLL/SInventory_BLL/PendingOrderBll.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |
| DAL class | PendingOrderDal (Library.DAL/SInventory_DAL/PendingOrderDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| cusMasterNewImageButton | ImageButton |  | no validator found |  |  |  | onclick→CustMasterNewImageButton_Click |
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| cusMasterNewImageButton | ImageButton |  | no validator found |  |  |  | onclick→CustMasterNewImageButton_Click |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| fromDate | CalendarExtender |  | no validator found |  |  |  |  |
| imgDateFrom | ImageButton |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| toDate | CalendarExtender |  | no validator found |  |  |  |  |
| imgDateTo | ImageButton |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| viewRptButton | Button | Search | no validator found |  |  |  | onclick→viewRptButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand |
| (no id) | BoundField | Sales Centre Code | no validator found |  |  |  |  |
| (no id) | BoundField | Order Code | no validator found |  |  |  |  |
| (no id) | BoundField | Submission Date | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | TemplateField | Process | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | aTable | SInventory_UI_PendingOrder.LoadPendingOrderInfo | PendingOrderBll.LoadPendingOrderInformation | PendingOrderDal.GetPendingOrderInformation | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | Page_Load → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
No buttons with server handlers found.

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
| SInventory_UI_PendingOrder.Page_Load | Solution.Web/SInventory_UI/PendingOrder.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_PendingOrder.Page_Load → OrderInfoBLL.LoadSC → OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_PendingOrder.LoadPendingOrderInfo | Solution.Web/SInventory_UI/PendingOrder.aspx.cs:25 |  | SInventory_UI_PendingOrder.LoadPendingOrderInfo → PendingOrderBll.LoadPendingOrderInformation → PendingOrderDal.GetPendingOrderInformation | no | no |
| SInventory_UI_PendingOrder.loadGridView_RowCommand | Solution.Web/SInventory_UI/PendingOrder.aspx.cs:46 | object sender, GridViewCommandEventArgs e | SInventory_UI_PendingOrder.loadGridView_RowCommand → PendingOrderBll.LoadOrderExistOrNotInfo / PendingOrderBll.LoadPendingOrderInformation → PendingOrderDal.GetOrderDetailsInfo / PendingOrderDal.GetOrderExistOrNotInfo / PendingOrderDal.GetPendingOrderInformati | no | no |
| SInventory_UI_PendingOrder.CustMasterReloadImageButton_Click | Solution.Web/SInventory_UI/PendingOrder.aspx.cs:58 | object sender, ImageClickEventArgs e | SInventory_UI_PendingOrder.CustMasterReloadImageButton_Click → PendingOrderBll.LoadPendingOrderInformation → PendingOrderDal.GetPendingOrderInformation | no | no |
| SInventory_UI_PendingOrder.viewRptButton_Click | Solution.Web/SInventory_UI/PendingOrder.aspx.cs:62 | object sender, EventArgs e | SInventory_UI_PendingOrder.viewRptButton_Click → PendingOrderBll.LoadPendingOrderInformation → PendingOrderDal.GetPendingOrderInformation | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| PendingOrderBll.LoadPendingOrderInformation | Library.BLL/SInventory_BLL/PendingOrderBll.cs:15 |
| PendingOrderBll.LoadOrderExistOrNotInfo | Library.BLL/SInventory_BLL/PendingOrderBll.cs:20 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| PendingOrderDal.GetPendingOrderInformation | Library.DAL/SInventory_DAL/PendingOrderDal.cs:16 | inline SQL: tblCompanyUnit, tblOrder, tblOrderListDetail | @ComUnitId, @FromDate, @ToDate |
| PendingOrderDal.GetOrderExistOrNotInfo | Library.DAL/SInventory_DAL/PendingOrderDal.cs:32 | inline SQL: tblOrder | @OrderCode |
| PendingOrderDal.GetOrderDetailsInfo | Library.DAL/SInventory_DAL/PendingOrderDal.cs:41 | inline SQL: tblOrderListDetail | @OrderCode |
| PendingOrderDal.SaveOrderGenerationInfo | Library.DAL/SInventory_DAL/PendingOrderDal.cs:52 | sp_OrderGenerationFromUploadOrder_SingleOrder | @IsApiData, @OrderCode, @OrderMasterID_In |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_OrderGenerationFromUploadOrder_SingleOrder | PendingOrderDal.SaveOrderGenerationInfo | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblOrder, tblOrderListDetail, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler viewRptButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select sales Center and Date Range | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 6 methods.
- Redirect targets: none
- User messages: Please Select sales Center and Date Range
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OrderInfoBLL, PendingOrderBll; DAL: OrderInfoDAL, PendingOrderDal; DAO: —; Tables (inline): tblCompanyUnit, tblOrder, tblOrderListDetail, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/PendingOrder.aspx`, `Solution.Web/SInventory_UI/PendingOrder.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/PendingOrder.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 23 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/PendingOrder.aspx |
| Data access | This document §15–17 | 1 SP names, 4 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods OrderInfoDAL.LoadSC, PendingOrderDal.GetOrderDetailsInfo, PendingOrderDal.GetOrderExistOrNotInfo, PendingOrderDal.GetPendingOrderInformation, PendingOrderDal.SaveOrderGenerationInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
