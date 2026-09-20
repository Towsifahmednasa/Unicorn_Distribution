---
page_id: PAGE-455
module: M12
page: SubdepotInfoEntry
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-455 — SubdepotInfoEntry

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-455 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubdepotInfoEntry |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SubdepotInfoEntry |
| aspx size | 10565 bytes |
| Code-behind LOC | 249 |
| Inline script size | 1180 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SubdepotInfoEntry** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sub-depot info, stock in/out/transfer, invoicing, payments, top sheets, adjustments and freeze.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SubDepot_UI/SubdepotView.aspx`

Outbound references: `subdepotview.aspx`

## 5. Route
`~/SubDepot_UI/SubdepotInfoEntry.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | CustomerMasterBLL (Library.BLL/SInventory_BLL/CustomerMasterBLL.cs) |
| BLL class | SubDepotBLL (Library.BLL/SubDepot_BLL/SubDepotBLL.cs) |
| DAL class | CustomerMasterDAL (Library.DAL/SInventory_DAL/CustomerMasterDAL.cs) |
| DAL class | SubDepotDAL (Library.DAL/SubDepot_DAL/SubDepotDAL.cs) |
| DAO/entity class | CompanyUnit (Library.DAO/SInventory_Entities/CompanyUnit.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| companyNameDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→companyNameDropDownList_SelectedIndexChanged1 |
| comUnitCodeTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| salesCenternameTextBox | TextBox |  | no validator found |  |  |  |  |
| addressTextBox | TextBox |  | no validator found |  |  |  |  |
| phoneNoTextBox | TextBox |  | no validator found |  |  |  |  |
| mobileNoTextBox | TextBox |  | no validator found |  |  |  |  |
| faxNoTextBox | TextBox |  | no validator found |  |  |  |  |
| LinksubmitButtonButton1 | LinkButton |  | no validator found |  |  |  | OnClick→submitButton_Click1 |
| regionDropDownList | DropDownList |  | no validator found |  | Visible=false | see §9 |  |
| subdepotIdHiddenField | HiddenField |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| comUnitCodeTextBox | CompanyUnit.ComUnitCode | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenternameTextBox | CompanyUnit.ComUnitName | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| addressTextBox | CompanyUnit.Address | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| phoneNoTextBox | CompanyUnit.PhoneNo | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| mobileNoTextBox | CompanyUnit.MobileNo | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| faxNoTextBox | CompanyUnit.FaxNo | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| companyNameDropDownList | CompanyUnit.ComUnitId | SubDepot_UI_SubdepotInfoEntry.Save | SubDepotBLL.SaveSalesCenter | SubDepotDAL.SaveDataForSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| comUnitCodeTextBox | CompanyUnit.ComUnitCode | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| salesCenternameTextBox | CompanyUnit.ComUnitName | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| addressTextBox | CompanyUnit.Address | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| phoneNoTextBox | CompanyUnit.PhoneNo | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| mobileNoTextBox | CompanyUnit.MobileNo | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| faxNoTextBox | CompanyUnit.FaxNo | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| companyNameDropDownList | CompanyUnit.ComUnitId | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |
| subdepotIdHiddenField | CompanyUnit.RegionId | SubDepot_UI_SubdepotInfoEntry.Update | SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.UpdateSalesCenter | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| companyNameDropDownList | LoadDropDown → SubDepotBLL.CompanyNameLoad | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit |
| regionDropDownList | companyNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadRegionname | inline SQL | RegionName/RegionCode, RegionName/RegionId | tblRegion |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| LinksubmitButtonButton1 | LinkButton click | (postback) | SubDepot_UI_SubdepotInfoEntry.submitButton_Click1 | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:165 | SubDepotBLL.SaveSalesCenter, SubDepotBLL.UpdateDataForSalesCenter | SubDepotDAL.SaveDataForSalesCenter, SubDepotDAL.UpdateSalesCenter | — | writes tblSubDepot |
|  | LinkButton click | (postback) | SubDepot_UI_SubdepotInfoEntry.Unnamed_Click | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:246 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SubDepot_UI_SubdepotInfoEntry.Page_Load | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:16 | object sender, EventArgs e | SubDepot_UI_SubdepotInfoEntry.Page_Load → SubDepotBLL.CompanyNameLoad / SubDepotBLL.LoadSubDepotById → SubDepotDAL.LoadCompanyName / SubDepotDAL.LoadSubDepotById | no | no |
| SubDepot_UI_SubdepotInfoEntry.GetData | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:30 | string id | SubDepot_UI_SubdepotInfoEntry.GetData → SubDepotBLL.LoadSubDepotById → SubDepotDAL.LoadSubDepotById | no | no |
| SubDepot_UI_SubdepotInfoEntry.LoadDropDown | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:47 |  | SubDepot_UI_SubdepotInfoEntry.LoadDropDown → SubDepotBLL.CompanyNameLoad → SubDepotDAL.LoadCompanyName | no | no |
| SubDepot_UI_SubdepotInfoEntry.Save | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:105 |  | SubDepot_UI_SubdepotInfoEntry.Save → SubDepotBLL.SaveSalesCenter → SubDepotDAL.SaveDataForSalesCenter | no | no |
| SubDepot_UI_SubdepotInfoEntry.Update | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:135 |  | SubDepot_UI_SubdepotInfoEntry.Update → SubDepotBLL.UpdateDataForSalesCenter → SubDepotDAL.UpdateSalesCenter | no | no |
| SubDepot_UI_SubdepotInfoEntry.submitButton_Click1 | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:165 | object sender, EventArgs e | SubDepot_UI_SubdepotInfoEntry.submitButton_Click1 → SubDepotBLL.SaveSalesCenter / SubDepotBLL.UpdateDataForSalesCenter → SubDepotDAL.SaveDataForSalesCenter / SubDepotDAL.UpdateSalesCenter | no | no |
| SubDepot_UI_SubdepotInfoEntry.unitViewImageButton_Click | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:178 | object sender, ImageClickEventArgs e | SubDepot_UI_SubdepotInfoEntry.unitViewImageButton_Click | no | no |
| SubDepot_UI_SubdepotInfoEntry.companyNameDropDownList_SelectedIndexChanged | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:182 | object sender, EventArgs e | SubDepot_UI_SubdepotInfoEntry.companyNameDropDownList_SelectedIndexChanged → CustomerMasterBLL.LoadRegionname → CustomerMasterDAL.LoadRegionname / CustomerMasterDAL.SingleParameter | no | no |
| SubDepot_UI_SubdepotInfoEntry.companyNameDropDownList_SelectedIndexChanged1 | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:187 | object sender, EventArgs e | SubDepot_UI_SubdepotInfoEntry.companyNameDropDownList_SelectedIndexChanged1 → SubDepotBLL.CodeGenerator → SubDepotDAL.GetId | no | no |
| SubDepot_UI_SubdepotInfoEntry.Unnamed_Click | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs:246 | object sender, EventArgs e | SubDepot_UI_SubdepotInfoEntry.Unnamed_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotBLL.LoadSubDepotById | Library.BLL/SubDepot_BLL/SubDepotBLL.cs:43 |
| SubDepotBLL.CompanyNameLoad | Library.BLL/SubDepot_BLL/SubDepotBLL.cs:72 |
| SubDepotBLL.SaveSalesCenter | Library.BLL/SubDepot_BLL/SubDepotBLL.cs:18 |
| SubDepotBLL.UpdateDataForSalesCenter | Library.BLL/SubDepot_BLL/SubDepotBLL.cs:77 |
| CustomerMasterBLL.LoadRegionname | Library.BLL/SInventory_BLL/CustomerMasterBLL.cs:148 |
| SubDepotBLL.CodeGenerator | Library.BLL/SubDepot_BLL/SubDepotBLL.cs:48 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotDAL.LoadSubDepotById | Library.DAL/SubDepot_DAL/SubDepotDAL.cs:57 | inline SQL: tblCompanyUnit, tblSubDepot |  |
| SubDepotDAL.LoadCompanyName | Library.DAL/SubDepot_DAL/SubDepotDAL.cs:97 | inline SQL: tblCompanyUnit |  |
| SubDepotDAL.SaveDataForSalesCenter | Library.DAL/SubDepot_DAL/SubDepotDAL.cs:16 | inline SQL: tblSubDepot |  |
| SubDepotDAL.UpdateSalesCenter | Library.DAL/SubDepot_DAL/SubDepotDAL.cs:91 | inline SQL: tblSubDepot |  |
| CustomerMasterDAL.LoadRegionname | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:203 | inline SQL: tblRegion |  |
| CustomerMasterDAL.SingleParameter | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs:386 | delegates / no SQL literal |  |
| SubDepotDAL.GetId | Library.DAL/SubDepot_DAL/SubDepotDAL.cs:52 | inline SQL: tblSubDepot |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblRegion, tblSubDepot
- Tables WRITTEN (inline SQL reachable): tblSubDepot
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Address!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Code!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Mobile No!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Input Sales Center Name!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Distribution Center!! | UNKNOWN |
| (handler Save) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |
| (handler Update) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please input data in all Textbox | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: SubDepot
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 13 methods.
- Redirect targets: SubdepotInfoEntry.aspx, SubdepotView.aspx
- User messages: Please Input Address!!; Please Input Code!!; Please Input Mobile No!!; Please Input Sales Center Name!!; Please Select Distribution Center!!; Please input data in all Textbox
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: CustomerMasterBLL, SubDepotBLL; DAL: CustomerMasterDAL, SubDepotDAL; DAO: CompanyUnit; Tables (inline): tblCompanyUnit, tblRegion, tblSubDepot

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click LinksubmitButtonButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click LinksubmitButtonButton1 with valid data | Handler runs; rows written to tblSubDepot |
| TC-05 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click  with valid data | Handler runs; rows written to tblSubDepot |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx`, `Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 17 controls parsed | n/a | PARTIAL | Solution.Web/SubDepot_UI/SubdepotInfoEntry.aspx |
| Data access | This document §15–17 | 0 SP names, 3 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M12 (Sub-Depot Operations).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CustomerMasterDAL.LoadRegionname, CustomerMasterDAL.SingleParameter, SubDepotDAL.GetId, SubDepotDAL.LoadCompanyName, SubDepotDAL.LoadSubDepotById affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
