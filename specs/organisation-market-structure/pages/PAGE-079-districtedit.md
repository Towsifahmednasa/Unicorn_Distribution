---
page_id: PAGE-079
module: M03
page: DistrictEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-079 — DistrictEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-079 |
| Module | M03 — Organisation & Market Structure Master (INFERRED from folder + file name) |
| Page name | DistrictEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_DistrictEdit |
| aspx size | 17033 bytes |
| Code-behind LOC | 95 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DistrictEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Company/branch/unit, geography (division/district/thana), sales hierarchy (national/zone/region/area/territory/market), routes, depot-territory mapping and structure transfers.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/DistrictView.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/DistrictEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DistrictEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| BLL class | DistrictInfoBLL (Library.BLL/SInventory_BLL/DistrictBLL.cs) |
| BLL class | RegionInfoBLL (Library.BLL/SInventory_BLL/RegionInfoBLL.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAL class | DistrictDAL (Library.DAL/SInventory_DAL/DistrictDAL.cs) |
| DAL class | RegionDAL (Library.DAL/SInventory_DAL/RegionDAL.cs) |
| DAO/entity class | DistrictInfo (Library.DAO/SInventory_Entities/DistrictInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| districtIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| companyNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→companyNameDropDownList_SelectedIndexChanged |
| regionDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→regionDropDownList_SelectedIndexChanged |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→comUnitNameDropDownList_SelectedIndexChanged |
| districtNameTextBox | TextBox |  | no validator found |  |  |  |  |
| districtCodeTextBox | TextBox |  | no validator found |  |  |  |  |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| districtIdHiddenField | DistrictInfo.DistrictId | SInventory_UI_DistrictEdit.updateButton_Click | DistrictInfoBLL.UpdateDataForDistrictInfo | DistrictDAL.UpdateDistrictInfo | @DistrictId | UNKNOWN (SP body not in repo) | UNKNOWN |
| districtCodeTextBox | DistrictInfo.DistrictCode | SInventory_UI_DistrictEdit.updateButton_Click | DistrictInfoBLL.UpdateDataForDistrictInfo | DistrictDAL.UpdateDistrictInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| districtNameTextBox | DistrictInfo.DistrictName | SInventory_UI_DistrictEdit.updateButton_Click | DistrictInfoBLL.UpdateDataForDistrictInfo | DistrictDAL.UpdateDistrictInfo | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| companyNameDropDownList | LoadDropDown → RegionInfoBLL.CompanyNameLoad | inline SQL | CompanyName/CompanyId | tblCompanyInfo |
| regionDropDownList | companyNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadRegionname | inline SQL | RegionName/RegionCode, RegionName/RegionId | tblRegion |
| comUnitNameDropDownList | regionDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadCompanyUnit | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |

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
| SInventory_UI_DistrictEdit.Page_Load | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_DistrictEdit.Page_Load → DistrictInfoBLL.DistrictInfoEditLoad / RegionInfoBLL.CompanyNameLoad → DistrictDAL.DistrictInfoEditLoad / RegionDAL.LoadCompanyName | no | no |
| SInventory_UI_DistrictEdit.LoadDropDown | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:23 |  | SInventory_UI_DistrictEdit.LoadDropDown → RegionInfoBLL.CompanyNameLoad → RegionDAL.LoadCompanyName | no | no |
| SInventory_UI_DistrictEdit.updateButton_Click | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:30 | object sender, EventArgs e | SInventory_UI_DistrictEdit.updateButton_Click → DistrictInfoBLL.UpdateDataForDistrictInfo → DistrictDAL.UpdateDistrictInfo | no | no |
| SInventory_UI_DistrictEdit.DistrictLoad | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:60 | string DistrictInfoId | SInventory_UI_DistrictEdit.DistrictLoad → DistrictInfoBLL.DistrictInfoEditLoad → DistrictDAL.DistrictInfoEditLoad | no | no |
| SInventory_UI_DistrictEdit.closeButton_Click | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:78 | object sender, EventArgs e | SInventory_UI_DistrictEdit.closeButton_Click | no | no |
| SInventory_UI_DistrictEdit.comUnitNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:82 | object sender, EventArgs e | SInventory_UI_DistrictEdit.comUnitNameDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_DistrictEdit.companyNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:86 | object sender, EventArgs e | SInventory_UI_DistrictEdit.companyNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadRegionname → CustomerMasterDAL.LoadRegionname / CustomerMasterDAL.SingleParameter | no | no |
| SInventory_UI_DistrictEdit.regionDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/DistrictEdit.aspx.cs:91 | object sender, EventArgs e | SInventory_UI_DistrictEdit.regionDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadCompanyUnit → CustomerMasterDAL.LoadCompanyUnit | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DistrictInfoBLL.DistrictInfoEditLoad | Library.BLL/SInventory_BLL/DistrictBLL.cs:70 |
| RegionInfoBLL.CompanyNameLoad | Library.BLL/SInventory_BLL/RegionInfoBLL.cs:74 |
| DistrictInfoBLL.UpdateDataForDistrictInfo | Library.BLL/SInventory_BLL/DistrictBLL.cs:60 |
| CustomerMasterBLL.LoadRegionname | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:148 |
| CustomerMasterBLL.LoadCompanyUnit | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:139 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DistrictDAL.DistrictInfoEditLoad | Library.DAL/SInventory_DAL/DistrictDAL.cs:50 | inline SQL: tblCompanyInfo, tblCompanyUnit, tblDistrict, tblRegion | @DistrictId |
| RegionDAL.LoadCompanyName | Library.DAL/SInventory_DAL/RegionDAL.cs:40 | inline SQL: tblCompanyInfo |  |
| DistrictDAL.UpdateDistrictInfo | Library.DAL/SInventory_DAL/DistrictDAL.cs:72 | inline SQL: tblDistrict | @DistrictCode, @DistrictId, @DistrictName |
| CustomerMasterDAL.LoadRegionname | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:203 | inline SQL: tblRegion |  |
| CustomerMasterDAL.SingleParameter | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:386 | delegates / no SQL literal |  |
| CustomerMasterDAL.LoadCompanyUnit | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:184 | inline SQL: tblCompanyUnit |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyInfo, tblCompanyUnit, tblDistrict, tblRegion
- Tables WRITTEN (inline SQL reachable): tblDistrict
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblDistrict | DistrictCode | @DistrictCode |
| tblDistrict | DistrictName | @DistrictName |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler updateButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Data Update Successfully!!! Please Reload | UNKNOWN |
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
- try/catch present in 0 of 8 methods.
- Redirect targets: none
- User messages: Data Update Successfully!!! Please Reload; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterBLL, DistrictInfoBLL, RegionInfoBLL; DAL: CustomerMasterDAL, DistrictDAL, RegionDAL; DAO: DistrictInfo; Tables (inline): tblCompanyInfo, tblCompanyUnit, tblDistrict, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DistrictEdit.aspx`, `Solution.Web/SInventory_UI/DistrictEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/DistrictEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 12 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/DistrictEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M03 (Organisation & Market Structure Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterDAL.LoadCompanyUnit, CustomerMasterDAL.LoadRegionname, CustomerMasterDAL.SingleParameter, DistrictDAL.DistrictInfoEditLoad, DistrictDAL.UpdateDistrictInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
