---
page_id: PAGE-685
module: M17
page: ApiCustomerList
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-685 — ApiCustomerList

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-685 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | ApiCustomerList |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_ApiCustomerList |
| aspx size | 13500 bytes |
| Code-behind LOC | 238 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ApiCustomerList** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: SAP hand-off pages, MIGO, provider dropout integration, API-customer import, mobile-app data-load web methods.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `importedapicustomer.aspx`

## 5. Route
`~/SInventory_UI/ApiCustomerList.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/ApiCustomerList.aspx |
| Code-behind | Solution.Web/SInventory_UI/ApiCustomerList.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ImportedApiCustomerBll (Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs) |
| DAL class | ImportedApiCustomerDal (Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs) |
| DAO/entity class | CustomerMaster (Library.BLL/SInventory_BLL/CustomerMaster.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| cusMasterNewImageButton | ImageButton |  | no validator found |  |  |  | onclick→CustMasterNewImageButton_Click |
| custMasterReloadImageButton | ImageButton |  | no validator found |  |  |  | onclick→CustMasterReloadImageButton_Click |
| custcodenameTextBox | TextBox |  | no validator found |  |  |  |  |
| Button2 | Button | Search | no validator found |  |  |  | OnClick→Button1_Click |
| custcodenameTextBox | TextBox |  | no validator found |  |  |  |  |
| districtNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| areaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| miaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| marketNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| regionNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| regionNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Address | no validator found |  |  |  |  |
| (no id) | BoundField | Cell No | no validator found |  |  |  |  |
| (no id) | BoundField | Mio Code | no validator found |  |  |  |  |
| (no id) | BoundField | Mio Name | no validator found |  |  |  |  |
| (no id) | BoundField | FE Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Code | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Add To Customer | no validator found |  |  |  |  |
| addImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| districtNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| areaNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| miaNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| marketNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| regionNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| regionNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| Button2 | Button | — | Button1_Click | — | — | — | — | UNKNOWN (handler not resolved) |

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
| SInventory_UI_ApiCustomerList.Page_Load | Solution.Web/SInventory_UI/ApiCustomerList.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_ApiCustomerList.Page_Load → ImportedApiCustomerBll.LoadNewCustomer → ImportedApiCustomerDal.LoadNewCustomer | no | no |
| SInventory_UI_ApiCustomerList.LoadData | Solution.Web/SInventory_UI/ApiCustomerList.aspx.cs:26 |  | SInventory_UI_ApiCustomerList.LoadData → ImportedApiCustomerBll.LoadNewCustomer → ImportedApiCustomerDal.LoadNewCustomer | no | no |
| SInventory_UI_ApiCustomerList.loadGridView_RowCommand | Solution.Web/SInventory_UI/ApiCustomerList.aspx.cs:45 | object sender, GridViewCommandEventArgs e | SInventory_UI_ApiCustomerList.loadGridView_RowCommand → ImportedApiCustomerBll.ApiCustomerInformation / ImportedApiCustomerBll.LoadNewCustomer / ImportedApiCustomerBll.SaveApiCustomerInfo / ImportedApiCustomerBll.UpdateApiCustomer → ImportedApiCustomerDal.ApiC | no | no |
| SInventory_UI_ApiCustomerList.CustMasterReloadImageButton_Click | Solution.Web/SInventory_UI/ApiCustomerList.aspx.cs:235 | object sender, ImageClickEventArgs e | SInventory_UI_ApiCustomerList.CustMasterReloadImageButton_Click → ImportedApiCustomerBll.LoadNewCustomer → ImportedApiCustomerDal.LoadNewCustomer | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| ImportedApiCustomerBll.LoadNewCustomer | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:19 |
| ImportedApiCustomerBll.ApiCustomerInformation | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:95 |
| ImportedApiCustomerBll.SaveApiCustomerInfo | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:100 |
| ImportedApiCustomerBll.UpdateApiCustomer | Library.BLL/SInventory_BLL/ImportedApiCustomerBll.cs:125 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| ImportedApiCustomerDal.LoadNewCustomer | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:20 | inline SQL: tbltempCustMaster |  |
| ImportedApiCustomerDal.ApiCustomerInformation | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:196 | inline SQL: tbltempCustMaster | @TempCustomerMasterId |
| ImportedApiCustomerDal.HasCustomerMastername | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:232 | inline SQL: tblCustMaster | @CustomerCode |
| ImportedApiCustomerDal.SaveApiCustomerInformation | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:242 | inline SQL: tblCustMaster | @Addrees2, @Address, @AreaCode, @CategoryId, @CellNo, @City, @ComUnitCode, @ComUnitName, @ConPerson, |
| ImportedApiCustomerDal.UpdateApiCustomerInfo | Library.DAL/SInventory_DAL/ImportedApiCustomerDal.cs:325 | inline SQL: tbltempCustMaster | @AddtoMainCustomer, @TempCustomerMasterId |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCustMaster, tbltempCustMaster
- Tables WRITTEN (inline SQL reachable): tblCustMaster, tbltempCustMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblCustMaster | Addrees2 | @Addrees2 |
| tblCustMaster | Address | @Address |
| tblCustMaster | AreaCode | @AreaCode |
| tblCustMaster | CategoryId | @CategoryId |
| tblCustMaster | CellNo | @CellNo |
| tblCustMaster | City | @City |
| tblCustMaster | ComUnitCode | @ComUnitCode |
| tblCustMaster | ComUnitName | @ComUnitName |
| tblCustMaster | ConPerson | @ConPerson |
| tblCustMaster | CustomerCode | @CustomerCode |
| tblCustMaster | CustomerCodeOld | @CustomerCodeOld |
| tblCustMaster | CustomerName | @CustomerName |
| tblCustMaster | DZSMName | @DZSMName |
| tblCustMaster | DisCode | @DisCode |
| tblCustMaster | FEName | @FEName |
| tblCustMaster | FixedCustomer | @FixedCustomer |
| tblCustMaster | MIACode | @MIACode |
| tblCustMaster | MIAName | @MIAName |
| tblCustMaster | MarketCode | @MarketCode |
| tblCustMaster | MarketName | @MarketName |
| tblCustMaster | RegionCode | @RegionCode |
| tblCustMaster | ShippingCond | @ShippingCond |
| tblCustMaster | TermOfPayment | @TermOfPayment |
| tbltempCustMaster | AddtoMainCustomer | @AddtoMainCustomer |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Edit customer data !!! | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Sorry! Customer already exist!!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 7 methods.
- Redirect targets: none
- User messages: Please Edit customer data !!!; Sorry! Customer already exist!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
ImportedApiCustomerBll, ImportedApiCustomerDal

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ImportedApiCustomerBll; DAL: ImportedApiCustomerDal; DAO: CustomerMaster; Tables (inline): tblCustMaster, tbltempCustMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click Button2 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click Button2 with valid data | Handler runs; rows written to tblCustMaster, tbltempCustMaster |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/ApiCustomerList.aspx`, `Solution.Web/SInventory_UI/ApiCustomerList.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/ApiCustomerList.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 34 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/ApiCustomerList.aspx |
| Data access | This document §15–17 | 0 SP names, 2 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M17 (SAP, Mobile-App & External Integration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ImportedApiCustomerDal.ApiCustomerInformation, ImportedApiCustomerDal.HasCustomerMastername, ImportedApiCustomerDal.LoadNewCustomer, ImportedApiCustomerDal.SaveApiCustomerInformation, ImportedApiCustomerDal.UpdateApiCustomerInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
