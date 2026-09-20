---
page_id: PAGE-196
module: M05
page: CustomerMasterView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-196 — CustomerMasterView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-196 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerMasterView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_CustomerMasterView |
| aspx size | 13235 bytes |
| Code-behind LOC | 148 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerMasterView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CustMasterEntry.aspx`

Outbound references: `custmasteredit.aspx`, `custmasterentry.aspx`, `customermasterviewer.aspx`

## 5. Route
`~/SInventory_UI/CustomerMasterView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerMasterView.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | CustomerMasterInfoBll (Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs) |
| DAL class | CustomerMasterInfoDal (Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs) |

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
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand |
| (no id) | BoundField | Distribution Center | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Address | no validator found |  |  |  |  |
| (no id) | BoundField | Cell No | no validator found |  |  |  |  |
| (no id) | BoundField | Mio Code | no validator found |  |  |  |  |
| (no id) | BoundField | FE Code | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Code | no validator found |  |  |  |  |
| (no id) | BoundField | Market Name | no validator found |  |  |  |  |
| (no id) | BoundField | DZSM Code | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| custcodenameTextBox | parameter | SInventory_UI_CustomerMasterView.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| districtNameDropDownList | parameter | SInventory_UI_CustomerMasterView.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| areaNameDropDownList | parameter | SInventory_UI_CustomerMasterView.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| miaNameDropDownList | parameter | SInventory_UI_CustomerMasterView.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| marketNameDropDownList | parameter | SInventory_UI_CustomerMasterView.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| regionNameDropDownList | parameter | SInventory_UI_CustomerMasterView.GenerateParameter | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| districtNameDropDownList | LoadDropDownList → CustomerMasterInfoBll.LoadFEInfo | inline SQL | DistrictName/DistrictCode | tblDistrict |
| areaNameDropDownList | LoadDropDownList → CustomerMasterInfoBll.LoadTerritoryInfo | inline SQL | AreaName/AreaCode | tblArea |
| miaNameDropDownList | LoadDropDownList → CustomerMasterInfoBll.LoadMiaInfo | inline SQL | MiaName/MiaCode | tblMIAInfo |
| marketNameDropDownList | LoadDropDownList → CustomerMasterInfoBll.LoadMaketInfo | inline SQL | MarketName/MarketCode | tblMarket |
| regionNameDropDownList | LoadDropDownList → CustomerMasterInfoBll.LoadDZSMInfo | inline SQL | RegionName/RegionCode | tblRegion |
| regionNameDropDownList | LoadDropDownList → CustomerMasterInfoBll.LoadDZSMInfo | inline SQL | RegionName/RegionCode | tblRegion |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| Button2 | Button click | (postback) | SInventory_UI_CustomerMasterView.Button1_Click | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:145 | CustomerMasterInfoBll.LoadCustomerMasterM | CustomerMasterInfoDal.LoadCustomerMasterViewM | — | read-only / none detected |

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
| SInventory_UI_CustomerMasterView.Page_Load | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:17 | object sender, EventArgs e | SInventory_UI_CustomerMasterView.Page_Load → CustomerMasterInfoBll.LoadDZSMInfo / CustomerMasterInfoBll.LoadFEInfo / CustomerMasterInfoBll.LoadMaketInfo / CustomerMasterInfoBll.LoadMiaInfo / CustomerMasterInfoBll.LoadTerritoryInfo → CustomerMasterInfoDal.GetDZ | no | no |
| SInventory_UI_CustomerMasterView.LoadDropDownList | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:26 |  | SInventory_UI_CustomerMasterView.LoadDropDownList → CustomerMasterInfoBll.LoadDZSMInfo / CustomerMasterInfoBll.LoadFEInfo / CustomerMasterInfoBll.LoadMaketInfo / CustomerMasterInfoBll.LoadMiaInfo / CustomerMasterInfoBll.LoadTerritoryInfo → CustomerMasterInfoDa | no | no |
| SInventory_UI_CustomerMasterView.CustomerMasterLoad | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:42 |  | SInventory_UI_CustomerMasterView.CustomerMasterLoad → CustomerMasterInfoBll.LoadCustomerMasterM → CustomerMasterInfoDal.LoadCustomerMasterViewM | no | no |
| SInventory_UI_CustomerMasterView.loadGridView_RowCommand | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:106 | object sender, GridViewCommandEventArgs e | SInventory_UI_CustomerMasterView.loadGridView_RowCommand | no | no |
| SInventory_UI_CustomerMasterView.CustMasterNewImageButton_Click | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:129 | object sender, ImageClickEventArgs e | SInventory_UI_CustomerMasterView.CustMasterNewImageButton_Click | no | no |
| SInventory_UI_CustomerMasterView.CustMasterReloadImageButton_Click | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:133 | object sender, ImageClickEventArgs e | SInventory_UI_CustomerMasterView.CustMasterReloadImageButton_Click → CustomerMasterInfoBll.LoadCustomerMasterM → CustomerMasterInfoDal.LoadCustomerMasterViewM | no | no |
| SInventory_UI_CustomerMasterView.rptImageButton_Click | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:138 | object sender, ImageClickEventArgs e | SInventory_UI_CustomerMasterView.rptImageButton_Click | no | no |
| SInventory_UI_CustomerMasterView.Button1_Click | Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs:145 | object sender, EventArgs e | SInventory_UI_CustomerMasterView.Button1_Click → CustomerMasterInfoBll.LoadCustomerMasterM → CustomerMasterInfoDal.LoadCustomerMasterViewM | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| CustomerMasterInfoBll.LoadFEInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:33 |
| CustomerMasterInfoBll.LoadTerritoryInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:38 |
| CustomerMasterInfoBll.LoadMiaInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:43 |
| CustomerMasterInfoBll.LoadMaketInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:48 |
| CustomerMasterInfoBll.LoadDZSMInfo | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:28 |
| CustomerMasterInfoBll.LoadCustomerMasterM | Library.BLL/SInventory_BLL/CustomerMasterInfoBll.cs:113 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CustomerMasterInfoDal.GetFEInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:38 | inline SQL: tblDistrict |  |
| CustomerMasterInfoDal.GetTerritoryInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:45 | inline SQL: tblArea |  |
| CustomerMasterInfoDal.GetMiaInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:52 | inline SQL: tblMIAInfo |  |
| CustomerMasterInfoDal.GetMaketInfo | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:59 | inline SQL: tblMarket |  |
| CustomerMasterInfoDal.GetDZSMname | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:24 | inline SQL: tblRegion |  |
| CustomerMasterInfoDal.LoadCustomerMasterViewM | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:187 | inline SQL: View_CustomerForModification |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): View_CustomerForModification, tblArea, tblDistrict, tblMIAInfo, tblMarket, tblRegion
- Tables WRITTEN (inline SQL reachable): none detected
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
- try/catch present in 0 of 11 methods.
- Redirect targets: CustMasterEntry.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterInfoBll; DAL: CustomerMasterInfoDal; DAO: —; Tables (inline): View_CustomerForModification, tblArea, tblDistrict, tblMIAInfo, tblMarket, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click Button2 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click Button2 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerMasterView.aspx`, `Solution.Web/SInventory_UI/CustomerMasterView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerMasterView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 28 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerMasterView.aspx |
| Data access | This document §15–17 | 0 SP names, 6 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterInfoDal.GetDZSMname, CustomerMasterInfoDal.GetFEInfo, CustomerMasterInfoDal.GetMaketInfo, CustomerMasterInfoDal.GetMiaInfo, CustomerMasterInfoDal.GetTerritoryInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
