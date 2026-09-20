---
page_id: PAGE-601
module: M15
page: Trainning
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-601 — Trainning

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-601 |
| Module | M15 — HR, Field-Force & Employee Administration (INFERRED from folder + file name) |
| Page name | Trainning |
| Kind | Page (INFERRED from name) |
| Web folder | DoctorModule_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DoctorModule_UI_Trainning |
| aspx size | 20475 bytes |
| Code-behind LOC | 439 |
| Inline script size | 2929 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **Trainning** (Page). Page title/heading text: `Market Structure`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Employee master, attendance, leave, holiday, shifts, tour plan, expense/TADA/mileage claims, training, notice board, employee-related reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DoctorModule_UI/TrainningView.aspx`

Outbound references: `trainningview.aspx`

## 5. Route
`~/DoctorModule_UI/Trainning.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DoctorModule_UI/Trainning.aspx |
| Code-behind | Solution.Web/DoctorModule_UI/Trainning.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStructure.ascx |
| JS include | //cdn.ckeditor.com/4.14.1/standard/ckeditor.js |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAL class | Setup2DAL (Library.DAL/DoctorModule_DAL/Setup2DAL.cs) |
| DAO/entity class | BonusCampaignMarketDetailDAO (Library.DAO/MasterSetup_DAO/BonusCampaignMarketDetailDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | Trainning (Library.DAO/DoctorModule_DAO/Trainning.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| txtTitle | TextBox |  | no validator found |  |  |  |  |
| txtDescription | TextBox |  | no validator found |  |  |  |  |
| txtMailBody | FTB:FreeTextBox |  | no validator found |  |  |  |  |
| TrainningMeterial | TextBox |  | no validator found |  |  |  |  |
| FromDate | TextBox | Select  From date | no validator found |  |  |  |  |
| Todate | TextBox | Select  To date | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnAddtoListMarket | LinkButton |  | no validator found |  |  |  | OnClick→btnAddtoListMarket_Click |
| gv_Market | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfGroupId | HiddenField |  | no validator found |  |  |  |  |
| hfRegionId | HiddenField |  | no validator found |  |  |  |  |
| hfAreaId | HiddenField |  | no validator found |  |  |  |  |
| hfTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| hfSubTerritoryId | HiddenField |  | no validator found |  |  |  |  |
| hfMarketId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Group | no validator found |  |  |  |  |
| lbl_GroupName | Label | <%#Eval("GroupName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Zone | no validator found |  |  |  |  |
| lbl_RegionName | Label | <%#Eval("RegionName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | AreaName | no validator found |  |  |  |  |
| lbl_AreaName | Label | <%#Eval("AreaName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Territory | no validator found |  |  |  |  |
| lbl_TerritoryName | Label | <%#Eval("TerritoryName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Sub-Territory | no validator found |  |  |  |  |
| lbl_SubTerritoryName | Label | <%#Eval("SubTerritoryName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Market | no validator found |  |  |  |  |
| lbl_MarketName | Label | <%#Eval("MarketName") %> | no validator found |  |  |  |  |
| (no id) | TemplateField | Remove | no validator found |  |  |  |  |
| MarketdeleteImageButton | LinkButton |  | no validator found |  |  |  | OnClick→MarketdeleteImageButton_Click |
| UserRoleSelect | ListBox |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Submit(this);, OnClick→btnSave_Click |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClientClick→return sweetAlertConfirm_Update(this);, OnClick→btnSave_Click |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |
| masterId | input | 0 | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| hfGroupId | BonusCampaignMarketDetailDAO.GroupId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfRegionId | BonusCampaignMarketDetailDAO.RegionId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @RegionId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfAreaId | BonusCampaignMarketDetailDAO.AreaId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @AreaId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfTerritoryId | BonusCampaignMarketDetailDAO.TerritoryId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @TerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfSubTerritoryId | BonusCampaignMarketDetailDAO.SubTerritoryId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @SubTerritoryId | UNKNOWN (SP body not in repo) | UNKNOWN |
| hfMarketId | BonusCampaignMarketDetailDAO.MarketId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @MarketId | UNKNOWN (SP body not in repo) | UNKNOWN |
| id_mastetID | Trainning.TrainningId | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @TrainningId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtTitle | Trainning.Title | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @Title | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtDescription | Trainning.Description | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @Description | UNKNOWN (SP body not in repo) | UNKNOWN |
| TrainningMeterial | Trainning.TrainningMeterial | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @TrainningMeterial | UNKNOWN (SP body not in repo) | UNKNOWN |
| FromDate | Trainning.FromDate | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @FromDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| Todate | Trainning.ToDate | DoctorModule_UI_Trainning.btnSave_Click | (none — page calls DAL directly) | Setup2DAL.Save_Trainning | @ToDate | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| UserRoleSelect | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnAddtoListMarket | LinkButton click | (postback) | DoctorModule_UI_Trainning.btnAddtoListMarket_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:418 | — | — | — | read-only / none detected |
| MarketdeleteImageButton | LinkButton click | (postback) | DoctorModule_UI_Trainning.MarketdeleteImageButton_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:245 | — | — | — | read-only / none detected |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | DoctorModule_UI_Trainning.btnSave_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:162 | — | Setup2DAL.Save_Trainning | sp_Save_TrainingMarketDetail, sp_Save_TrainingUserRoleDetail, sp_Save_Trainning, sp_Update_Trainning | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | DoctorModule_UI_Trainning.btnSave_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:162 | — | Setup2DAL.Save_Trainning | sp_Save_TrainingMarketDetail, sp_Save_TrainingUserRoleDetail, sp_Save_Trainning, sp_Update_Trainning | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | DoctorModule_UI_Trainning.btnReset_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:436 | — | — | — | read-only / none detected |

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
| DoctorModule_UI_Trainning.Page_Load | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:19 | object sender, EventArgs e | DoctorModule_UI_Trainning.Page_Load → SeedDataDAL.GetUserRoleList / Setup2DAL.GetTrainningDetailMarketById / Setup2DAL.GetTrainningSetupById → SP: sp_GET_TrainingDetailMarket_ById, sp_Get_Trainning_ById, sp_Get_UserRoleInfo | yes | no |
| DoctorModule_UI_Trainning.GetOneRecord | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:60 | string value | DoctorModule_UI_Trainning.GetOneRecord → Setup2DAL.GetTrainningDetailMarketById / Setup2DAL.GetTrainningSetupById → SP: sp_GET_TrainingDetailMarket_ById, sp_Get_Trainning_ById | yes | no |
| DoctorModule_UI_Trainning.btnSave_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:162 | object sender, EventArgs e | DoctorModule_UI_Trainning.btnSave_Click → Setup2DAL.Save_Trainning → SP: sp_Save_TrainingMarketDetail, sp_Save_TrainingUserRoleDetail, sp_Save_Trainning, sp_Update_Trainning | no | yes |
| DoctorModule_UI_Trainning.MarketdeleteImageButton_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:245 | object sender, EventArgs e | DoctorModule_UI_Trainning.MarketdeleteImageButton_Click | no | no |
| DoctorModule_UI_Trainning.btnAddtoListMarket_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:418 | object sender, EventArgs e | DoctorModule_UI_Trainning.btnAddtoListMarket_Click | no | no |
| DoctorModule_UI_Trainning.btnReset_Click | Solution.Web/DoctorModule_UI/Trainning.aspx.cs:436 | object sender, EventArgs e | DoctorModule_UI_Trainning.btnReset_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetUserRoleList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:707 | sp_Get_UserRoleInfo |  |
| Setup2DAL.GetTrainningSetupById | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5135 | sp_Get_Trainning_ById | @id |
| Setup2DAL.GetTrainningDetailMarketById | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:5073 | sp_GET_TrainingDetailMarket_ById | @id |
| Setup2DAL.Save_Trainning | Library.DAL/DoctorModule_DAL/Setup2DAL.cs:4923 | sp_Save_TrainingMarketDetail, sp_Save_TrainingUserRoleDetail, sp_Save_Trainning, | @AreaId, @Description, @EntryBy, @FromDate, @GroupId, @IsActive, @MarketId, @RegionId, @SubTerritory |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_TrainingDetailMarket_ById | Setup2DAL.GetTrainningDetailMarketById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_Trainning_ById | Setup2DAL.GetTrainningSetupById | body NOT in repository | read-like name (INFERRED) |
| sp_Get_UserRoleInfo | SeedDataDAL.GetUserRoleList | body NOT in repository | read-like name (INFERRED) |
| sp_Save_TrainingMarketDetail | Setup2DAL.Save_Trainning | body NOT in repository | write-like name |
| sp_Save_TrainingUserRoleDetail | Setup2DAL.Save_Trainning | body NOT in repository | write-like name |
| sp_Save_Trainning | Setup2DAL.Save_Trainning | body NOT in repository | write-like name |
| sp_Update_Trainning | Setup2DAL.Save_Trainning | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 datepicker is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please Add to List Market Structure or User Role!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TrainningView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler btnAddtoListMarket_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler btnAddtoListMarket_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler btnAddtoListMarket_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 10 methods.
- Redirect targets: Trainning.aspx
- User messages: ','Success','TrainningView.aspx');; Already Exist!; form-control form-control-sm is-invalid; form-control form-control-sm mb-3 datepicker is-invalid; form-select form-select-sm mb-3 mySelect2; form-select form-select-sm mb-3 mySelect2 is-invalid
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: SeedDataDAL, Setup2DAL; DAO: BonusCampaignMarketDetailDAO, ResultInfo, Trainning; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnAddtoListMarket with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnAddtoListMarket with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click MarketdeleteImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click MarketdeleteImageButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnUpdate with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnUpdate with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-11 | Click btnReset with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click btnReset with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/DoctorModule_UI/Trainning.aspx`, `Solution.Web/DoctorModule_UI/Trainning.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DoctorModule_UI/Trainning.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 42 controls parsed | n/a | PARTIAL | Solution.Web/DoctorModule_UI/Trainning.aspx |
| Data access | This document §15–17 | 7 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 7 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M15 (HR, Field-Force & Employee Administration).
- Other pages sharing this page's stored procedures: PAGE-012, PAGE-015, PAGE-016, PAGE-549, PAGE-562, PAGE-563, PAGE-564, PAGE-571, PAGE-582, PAGE-583, PAGE-588, PAGE-610, PAGE-613, PAGE-615, PAGE-621 …
- Changing DAL methods SeedDataDAL.GetUserRoleList, Setup2DAL.GetTrainningDetailMarketById, Setup2DAL.GetTrainningSetupById, Setup2DAL.Save_Trainning affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
