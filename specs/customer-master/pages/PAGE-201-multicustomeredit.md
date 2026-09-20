---
page_id: PAGE-201
module: M05
page: MultiCustomerEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-201 — MultiCustomerEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-201 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | MultiCustomerEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_MultiCustomerEdit |
| aspx size | 17813 bytes |
| Code-behind LOC | 204 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MultiCustomerEdit** (Edit form). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/MultiCustomerEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| BLL class | MultiCustomerEditBLL (Library.BLL/SInventory_BLL/MultiCustomerEditBLL.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAL class | MultiCustomerEditDAL (Library.DAL/SInventory_DAL/MultiCustomerEditDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| comUnitNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| regionNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| regionNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| districtNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| districtNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| areaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| areaNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| miaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| miaNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| marketNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| marketNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| categoryNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| categoryNameDropDownList0 | DropDownList |  | no validator found |  |  | see §9 |  |
| searchButton | Button | Search | no validator found |  |  |  | onclick→searchButton_Click |
| searchButton | Button | Search | no validator found |  |  |  | onclick→searchButton_Click |
| UpdateProgress2 | UpdateProgress |  | no validator found |  |  |  |  |
| Img21 | Image |  | no validator found |  |  |  |  |
| replaceButton | Button | Replace | no validator found |  |  |  | onclick→replaceButton_Click1 |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| Img2 | Image |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | oncheckedchanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Address | no validator found |  |  |  |  |
| (no id) | BoundField | Branch | no validator found |  |  |  |  |
| (no id) | BoundField | Shipping Point | no validator found |  |  |  |  |
| (no id) | BoundField | Territory | no validator found |  |  |  |  |
| (no id) | BoundField | FE | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM | no validator found |  |  |  |  |
| Label1 | Label | Not Updated : | no validator found |  |  |  |  |
| Label2 | Label |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| regionNameDropDownList | parameter | SInventory_UI_MultiCustomerEdit.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitNameDropDownList | parameter | SInventory_UI_MultiCustomerEdit.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| districtNameDropDownList | parameter | SInventory_UI_MultiCustomerEdit.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| areaNameDropDownList | parameter | SInventory_UI_MultiCustomerEdit.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList | parameter | SInventory_UI_MultiCustomerEdit.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | parameter | SInventory_UI_MultiCustomerEdit.Parameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitNameDropDownList0 | CUnit | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList0 | MarketName | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| regionNameDropDownList0 | parameter | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitNameDropDownList0 | parameter | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| districtNameDropDownList0 | parameter | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| areaNameDropDownList0 | parameter | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList0 | parameter | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList0 | parameter | SInventory_UI_MultiCustomerEdit.ParameterUpdate | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| comUnitNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadCompanyUnit | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |
| comUnitNameDropDownList0 | LoadDropDown → CustomerMasterBLL.LoadCompanyUnit | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |
| regionNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadRegionname | inline SQL | RegionName/RegionCode, RegionName/RegionId | tblRegion |
| regionNameDropDownList0 | LoadDropDown → CustomerMasterBLL.LoadRegionname | inline SQL | RegionName/RegionCode, RegionName/RegionId | tblRegion |
| districtNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadDistrictName | inline SQL | DistrictName/DistrictCode | tblDistrict |
| districtNameDropDownList0 | LoadDropDown → CustomerMasterBLL.LoadDistrictName | inline SQL | DistrictName/DistrictCode | tblDistrict |
| areaNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadAreaName | inline SQL | AreaName/AreaCode | tblArea |
| areaNameDropDownList0 | LoadDropDown → CustomerMasterBLL.LoadAreaName | inline SQL | AreaName/AreaCode | tblArea |
| miaNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadMiaName | inline SQL | MiaName/MiaCode | tblMIAInfo |
| miaNameDropDownList0 | LoadDropDown → CustomerMasterBLL.LoadMiaName | inline SQL | MiaName/MiaCode | tblMIAInfo |
| marketNameDropDownList | LoadDropDown → CustomerMasterBLL.LoadMarketName | inline SQL | MarketName/MarketCode | tblMarket |
| marketNameDropDownList0 | LoadDropDown → CustomerMasterBLL.LoadMarketName | inline SQL | MarketName/MarketCode | tblMarket |
| categoryNameDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| categoryNameDropDownList0 | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

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
| SInventory_UI_MultiCustomerEdit.Page_Load | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs:15 | object sender, EventArgs e | SInventory_UI_MultiCustomerEdit.Page_Load → CustomerMasterBLL.LoadAreaName / CustomerMasterBLL.LoadCompanyUnit / CustomerMasterBLL.LoadDistrictName / CustomerMasterBLL.LoadMarketName / CustomerMasterBLL.LoadMiaName / CustomerMasterBLL.LoadRegio → CustomerMaste | no | no |
| SInventory_UI_MultiCustomerEdit.LoadDropDown | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs:22 |  | SInventory_UI_MultiCustomerEdit.LoadDropDown → CustomerMasterBLL.LoadAreaName / CustomerMasterBLL.LoadCompanyUnit / CustomerMasterBLL.LoadDistrictName / CustomerMasterBLL.LoadMarketName / CustomerMasterBLL.LoadMiaName / CustomerMasterBLL.LoadRegio → CustomerMa | no | no |
| SInventory_UI_MultiCustomerEdit.chkSelectAll_CheckedChanged | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs:39 | object sender, EventArgs e | SInventory_UI_MultiCustomerEdit.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_MultiCustomerEdit.searchButton_Click | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs:127 | object sender, EventArgs e | SInventory_UI_MultiCustomerEdit.searchButton_Click → MultiCustomerEditBLL.LoadCusteomer → MultiCustomerEditDAL.BuildWhereClause / MultiCustomerEditDAL.LoadCusteomer | no | no |
| SInventory_UI_MultiCustomerEdit.replaceButton_Click1 | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs:167 | object sender, EventArgs e | SInventory_UI_MultiCustomerEdit.replaceButton_Click1 → MultiCustomerEditBLL.CHeckInvice / MultiCustomerEditBLL.UpdateDataForCustomer → MultiCustomerEditDAL.BuildSetClause / MultiCustomerEditDAL.CHeckInvice / MultiCustomerEditDAL.UpdateDataForCustomer | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustomerMasterBLL.LoadRegionname | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:148 |
| CustomerMasterBLL.LoadCompanyUnit | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:139 |
| CustomerMasterBLL.LoadMiaName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:160 |
| CustomerMasterBLL.LoadDistrictName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:144 |
| CustomerMasterBLL.LoadMarketName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:126 |
| CustomerMasterBLL.LoadAreaName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:131 |
| MultiCustomerEditBLL.LoadCusteomer | Library.BLL/SInventory_BLL/MultiCustomerEditBLL.cs:19 |
| MultiCustomerEditBLL.CHeckInvice | Library.BLL/SInventory_BLL/MultiCustomerEditBLL.cs:23 |
| MultiCustomerEditBLL.UpdateDataForCustomer | Library.BLL/SInventory_BLL/MultiCustomerEditBLL.cs:15 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerMasterDAL.LoadRegionname | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:203 | inline SQL: tblRegion |  |
| CustomerMasterDAL.SingleParameter | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:386 | delegates / no SQL literal |  |
| CustomerMasterDAL.LoadCompanyUnit | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:184 | inline SQL: tblCompanyUnit |  |
| CustomerMasterDAL.LoadMiaName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:221 | inline SQL: tblMIAInfo |  |
| CustomerMasterDAL.LoadDistrictName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:197 | inline SQL: tblDistrict |  |
| CustomerMasterDAL.LoadMarketName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:166 | inline SQL: tblMarket |  |
| CustomerMasterDAL.LoadAreaName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:178 | inline SQL: tblArea |  |
| MultiCustomerEditDAL.LoadCusteomer | Library.DAL/SInventory_DAL/MultiCustomerEditDAL.cs:52 | inline SQL: View_CustomerMaster |  |
| MultiCustomerEditDAL.BuildWhereClause | Library.DAL/SInventory_DAL/MultiCustomerEditDAL.cs:69 | delegates / no SQL literal |  |
| MultiCustomerEditDAL.CHeckInvice | Library.DAL/SInventory_DAL/MultiCustomerEditDAL.cs:60 | inline SQL: tblInvoice | @CustomerMasterId |
| MultiCustomerEditDAL.UpdateDataForCustomer | Library.DAL/SInventory_DAL/MultiCustomerEditDAL.cs:38 | inline SQL: tblCustMaster | @CustomerMasterId |
| MultiCustomerEditDAL.BuildSetClause | Library.DAL/SInventory_DAL/MultiCustomerEditDAL.cs:90 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): View_CustomerMaster, tblArea, tblCompanyUnit, tblDistrict, tblInvoice, tblMIAInfo, tblMarket, tblRegion
- Tables WRITTEN (inline SQL reachable): tblCustMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterBLL, MultiCustomerEditBLL; DAL: CustomerMasterDAL, MultiCustomerEditDAL; DAO: —; Tables (inline): View_CustomerMaster, tblArea, tblCompanyUnit, tblCustMaster, tblDistrict, tblInvoice, tblMIAInfo, tblMarket, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MultiCustomerEdit.aspx`, `Solution.Web/SInventory_UI/MultiCustomerEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 40 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MultiCustomerEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 9 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterDAL.LoadAreaName, CustomerMasterDAL.LoadCompanyUnit, CustomerMasterDAL.LoadDistrictName, CustomerMasterDAL.LoadMarketName, CustomerMasterDAL.LoadMiaName affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
