---
page_id: PAGE-633
module: M15
page: MiaInfoEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-633 — MiaInfoEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-633 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | MiaInfoEdit |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_UI_MiaInfoEdit |
| aspx size | 17263 bytes |
| Code-behind LOC | 115 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **MiaInfoEdit** (Edit form). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/MiaInfoView.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_UI/MiaInfoEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MiaInfoEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| BLL class | MIAInformationBLL (Library.BLL/SInventory_BLL/MIAInformationBLL.cs) |
| BLL class | RegionInfoBLL (Library.BLL/SInventory_BLL/RegionInfoBLL.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAL class | MiaInformationDAL (Library.DAL/SInventory_DAL/MiaInformationDAL.cs) |
| DAL class | RegionDAL (Library.DAL/SInventory_DAL/RegionDAL.cs) |
| DAO/entity class | MiaInformation (Library.DAO/SInventory_Entities/MiaInformation.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| miaInfoIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| MessageLabel | Label |  | no validator found |  |  |  |  |
| refCodeTextBox | TextBox |  | no validator found |  |  |  |  |
| refNameTextBox | TextBox |  | no validator found |  |  |  |  |
| companyNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→companyNameDropDownList_SelectedIndexChanged |
| regionNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→regionNameDropDownList_SelectedIndexChanged |
| comUnitNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→comUnitNameDropDownList_SelectedIndexChanged |
| districtNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→districtNameDropDownList_SelectedIndexChanged |
| areaNameDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| manufacDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| updateButton | Button | Update | no validator found |  |  |  | onclick→updateButton_Click |
| closeButton | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| miaInfoIdHiddenField | MiaInformation.MiaId | SInventory_UI_MiaInfoEdit.updateButton_Click | MIAInformationBLL.UpdateDataForMiaInformation | MiaInformationDAL.UpdateaMiaInformation | @MiaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| refCodeTextBox | MiaInformation.MiaCode | SInventory_UI_MiaInfoEdit.updateButton_Click | MIAInformationBLL.UpdateDataForMiaInformation | MiaInformationDAL.UpdateaMiaInformation | UNKNOWN | UNKNOWN | UNKNOWN |
| refNameTextBox | MiaInformation.MiaName | SInventory_UI_MiaInfoEdit.updateButton_Click | MIAInformationBLL.UpdateDataForMiaInformation | MiaInformationDAL.UpdateaMiaInformation | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| companyNameDropDownList | LoadDropDown → RegionInfoBLL.CompanyNameLoad | inline SQL | CompanyName/CompanyId | tblCompanyInfo |
| regionNameDropDownList | companyNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadRegionname | inline SQL | RegionName/RegionCode, RegionName/RegionId | tblRegion |
| comUnitNameDropDownList | regionNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadCompanyUnit | inline SQL | ComUnitName/ComUnitCode | tblCompanyUnit |
| districtNameDropDownList | comUnitNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadDistrictName | inline SQL | DistrictName/DistrictCode | tblDistrict |
| areaNameDropDownList | districtNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadAreaName | inline SQL | AreaName/AreaCode | tblArea |
| manufacDropDownList | LoadDropDown → MIAInformationBLL.LoadManfac | inline SQL | ManufacName/ManufacId | tblManufacturer |

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
| SInventory_UI_MiaInfoEdit.Page_Load | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.Page_Load → MIAInformationBLL.LoadManfac / MIAInformationBLL.MiaInformationEditLoad / RegionInfoBLL.CompanyNameLoad → MiaInformationDAL.LoadManfac / MiaInformationDAL.MiaInformationEditLoad / RegionDAL.LoadCompanyName | no | no |
| SInventory_UI_MiaInfoEdit.LoadDropDown | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:27 |  | SInventory_UI_MiaInfoEdit.LoadDropDown → MIAInformationBLL.LoadManfac / RegionInfoBLL.CompanyNameLoad → MiaInformationDAL.LoadManfac / RegionDAL.LoadCompanyName | no | no |
| SInventory_UI_MiaInfoEdit.updateButton_Click | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:34 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.updateButton_Click → MIAInformationBLL.UpdateDataForMiaInformation → MiaInformationDAL.UpdateaMiaInformation | no | no |
| SInventory_UI_MiaInfoEdit.MiaInformationLoad | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:66 | string miaId | SInventory_UI_MiaInfoEdit.MiaInformationLoad → MIAInformationBLL.MiaInformationEditLoad → MiaInformationDAL.MiaInformationEditLoad | no | no |
| SInventory_UI_MiaInfoEdit.regionNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:85 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.regionNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadCompanyUnit → CustomerMasterDAL.LoadCompanyUnit | no | no |
| SInventory_UI_MiaInfoEdit.comUnitNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:89 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.comUnitNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadDistrictName → CustomerMasterDAL.LoadDistrictName | no | no |
| SInventory_UI_MiaInfoEdit.districtNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:93 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.districtNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadAreaName → CustomerMasterDAL.LoadAreaName | no | no |
| SInventory_UI_MiaInfoEdit.miaNameTextBox_TextChanged | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:98 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.miaNameTextBox_TextChanged | no | no |
| SInventory_UI_MiaInfoEdit.miaCodeTextBox_TextChanged | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:103 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.miaCodeTextBox_TextChanged | no | no |
| SInventory_UI_MiaInfoEdit.companyNameDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:107 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.companyNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadRegionname → CustomerMasterDAL.LoadRegionname / CustomerMasterDAL.SingleParameter | no | no |
| SInventory_UI_MiaInfoEdit.closeButton_Click | Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs:112 | object sender, EventArgs e | SInventory_UI_MiaInfoEdit.closeButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RegionInfoBLL.CompanyNameLoad | Library.BLL/SInventory_BLL/RegionInfoBLL.cs:74 |
| MIAInformationBLL.LoadManfac | Library.BLL/SInventory_BLL/MIAInformationBLL.cs:41 |
| MIAInformationBLL.MiaInformationEditLoad | Library.BLL/SInventory_BLL/MIAInformationBLL.cs:61 |
| MIAInformationBLL.UpdateDataForMiaInformation | Library.BLL/SInventory_BLL/MIAInformationBLL.cs:51 |
| CustomerMasterBLL.LoadCompanyUnit | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:139 |
| CustomerMasterBLL.LoadDistrictName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:144 |
| CustomerMasterBLL.LoadAreaName | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:131 |
| CustomerMasterBLL.LoadRegionname | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:148 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RegionDAL.LoadCompanyName | Library.DAL/SInventory_DAL/RegionDAL.cs:40 | inline SQL: tblCompanyInfo |  |
| MiaInformationDAL.LoadManfac | Library.DAL/SInventory_DAL/MiaInformationDAL.cs:106 | inline SQL: tblManufacturer |  |
| MiaInformationDAL.MiaInformationEditLoad | Library.DAL/SInventory_DAL/MiaInformationDAL.cs:64 | inline SQL: tblMIAInfo | @MiaId |
| MiaInformationDAL.UpdateaMiaInformation | Library.DAL/SInventory_DAL/MiaInformationDAL.cs:89 | inline SQL: tblMIAInfo | @MiaCode, @MiaId, @MiaName |
| CustomerMasterDAL.LoadCompanyUnit | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:184 | inline SQL: tblCompanyUnit |  |
| CustomerMasterDAL.LoadDistrictName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:197 | inline SQL: tblDistrict |  |
| CustomerMasterDAL.LoadAreaName | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:178 | inline SQL: tblArea |  |
| CustomerMasterDAL.LoadRegionname | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:203 | inline SQL: tblRegion |  |
| CustomerMasterDAL.SingleParameter | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:386 | delegates / no SQL literal |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblArea, tblCompanyInfo, tblCompanyUnit, tblDistrict, tblMIAInfo, tblManufacturer, tblRegion
- Tables WRITTEN (inline SQL reachable): tblMIAInfo
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblMIAInfo | MiaCode | @MiaCode |
| tblMIAInfo | MiaName | @MiaName |

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
- try/catch present in 0 of 11 methods.
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
BLL: CustomerMasterBLL, MIAInformationBLL, RegionInfoBLL; DAL: CustomerMasterDAL, MiaInformationDAL, RegionDAL; DAO: MiaInformation; Tables (inline): tblArea, tblCompanyInfo, tblCompanyUnit, tblDistrict, tblMIAInfo, tblManufacturer, tblRegion

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MiaInfoEdit.aspx`, `Solution.Web/SInventory_UI/MiaInfoEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/MiaInfoEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 15 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/MiaInfoEdit.aspx |
| Data access | This document §15–17 | 0 SP names, 7 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterDAL.LoadAreaName, CustomerMasterDAL.LoadCompanyUnit, CustomerMasterDAL.LoadDistrictName, CustomerMasterDAL.LoadRegionname, CustomerMasterDAL.SingleParameter affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
