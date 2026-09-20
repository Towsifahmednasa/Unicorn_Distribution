---
page_id: PAGE-223
module: M06
page: OrderEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-223 — OrderEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-223 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | OrderEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_OrderEdit |
| aspx size | 5865 bytes |
| Code-behind LOC | 110 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **OrderEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/BacktoReturnPage.aspx`, `SInventory_UI/OrderDeleteorEdit.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/OrderEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/OrderEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/OrderEdit.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| BLL class | OrderListBLL (Library.BLL/SInventory_BLL/OrderListBLL.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAL class | OrderListDAL (Library.DAL/SInventory_DAL/OrderListDAL.cs) |
| DAO/entity class | CustomerMaster (Library.BLL/SInventory_BLL/CustomerMaster.cs) |
| DAO/entity class | OrderInfoMaster (Library.DAO/SInventory_Entities/OrderInfoMaster.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| custMastIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→comUnitNameDropDownList_SelectedIndexChanged |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| comUnitNameDropDownList | DC | SInventory_UI_OrderEdit.updateButton_Click | OrderListBLL.UpdateDataForCompanyInfo | OrderListDAL.Param, OrderListDAL.UpdateCompanyInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| custMastIdHiddenField | OrderInfoMaster.OrderId | SInventory_UI_OrderEdit.updateButton_Click | OrderListBLL.UpdateDataForCompanyInfo | OrderListDAL.Param, OrderListDAL.UpdateCompanyInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitNameDropDownList | OrderInfoMaster.ComUnitId | SInventory_UI_OrderEdit.updateButton_Click | OrderListBLL.UpdateDataForCompanyInfo | OrderListDAL.Param, OrderListDAL.UpdateCompanyInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| comUnitNameDropDownList | LoadDropDown → CustomerMasterDAL.LoadCompanyUnitbyID | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit |

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
| SInventory_UI_OrderEdit.Page_Load | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_OrderEdit.Page_Load → CustomerMasterDAL.LoadCompanyUnitbyID | no | no |
| SInventory_UI_OrderEdit.LoadDropDown | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:25 |  | SInventory_UI_OrderEdit.LoadDropDown → CustomerMasterDAL.LoadCompanyUnitbyID | no | no |
| SInventory_UI_OrderEdit.updateButton_Click | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:50 | object sender, EventArgs e | SInventory_UI_OrderEdit.updateButton_Click → OrderListBLL.UpdateDataForCompanyInfo → OrderListDAL.Param / OrderListDAL.UpdateCompanyInfo | no | no |
| SInventory_UI_OrderEdit.CompanyInfoLoad | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:74 | string custMastId | SInventory_UI_OrderEdit.CompanyInfoLoad → CustomerMasterBLL.CustomerMasterEditLoad → CustomerMasterDAL.CustomerMasterEditLoad | no | no |
| SInventory_UI_OrderEdit.closeButton_Click | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:83 | object sender, EventArgs e | SInventory_UI_OrderEdit.closeButton_Click | no | no |
| SInventory_UI_OrderEdit.regionNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:87 | object sender, EventArgs e | SInventory_UI_OrderEdit.regionNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_OrderEdit.areaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:91 | object sender, EventArgs e | SInventory_UI_OrderEdit.areaNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_OrderEdit.comUnitNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:96 | object sender, EventArgs e | SInventory_UI_OrderEdit.comUnitNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_OrderEdit.districtNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:101 | object sender, EventArgs e | SInventory_UI_OrderEdit.districtNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_OrderEdit.miaNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/OrderEdit.aspx.cs:106 | object sender, EventArgs e | SInventory_UI_OrderEdit.miaNameDropDownList_SelectedIndexChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderListBLL.UpdateDataForCompanyInfo | Library.BLL/SInventory_BLL/OrderListBLL.cs:118 |
| CustomerMasterBLL.CustomerMasterEditLoad | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:108 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerMasterDAL.LoadCompanyUnitbyID | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:190 | inline SQL: tblCompanyUnit |  |
| OrderListDAL.UpdateCompanyInfo | Library.DAL/SInventory_DAL/OrderListDAL.cs:271 | inline SQL: tblOrder |  |
| OrderListDAL.Param | Library.DAL/SInventory_DAL/OrderListDAL.cs:17 | delegates / no SQL literal |  |
| CustomerMasterDAL.CustomerMasterEditLoad | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:99 | inline SQL: tblCustMaster | @CustomerMasterId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblCustMaster
- Tables WRITTEN (inline SQL reachable): tblOrder
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblOrder | ComUnitCode | @ComUnitCode |
| tblOrder | ComUnitId | @ComUnitId |
| tblOrder | ComUnitName | @ComUnitName |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Distribution Center Name !! | UNKNOWN |
| (handler updateButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 12 methods.
- Redirect targets: none
- User messages: Please Input Distribution Center Name !!; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterBLL, OrderListBLL; DAL: CustomerMasterDAL, OrderListDAL; DAO: CustomerMaster, OrderInfoMaster; Tables (inline): tblCompanyUnit, tblCustMaster, tblOrder

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/OrderEdit.aspx`, `Solution.Web/SInventory_UI/OrderEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/OrderEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 7 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/OrderEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterDAL.CustomerMasterEditLoad, CustomerMasterDAL.LoadCompanyUnitbyID, OrderListDAL.Param, OrderListDAL.UpdateCompanyInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
