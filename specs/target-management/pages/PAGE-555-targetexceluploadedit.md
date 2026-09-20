---
page_id: PAGE-555
module: M14
page: TargetExcelUploadEdit
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-555 — TargetExcelUploadEdit

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-555 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | TargetExcelUploadEdit |
| Kind | Upload / Import (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_TargetExcelUploadEdit |
| aspx size | 11035 bytes |
| Code-behind LOC | 227 |
| Inline script size | 1361 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **TargetExcelUploadEdit** (Upload / Import). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/TargetExcelUploadList.aspx`

Outbound references: `targetexceluploadlist.aspx`

## 5. Route
`~/SInventory_UI/TargetExcelUploadEdit.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx |
| Code-behind | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | BonusCampaignNewDAL (Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | SeedDataDAL (Library.DAL/DoctorModule_DAL/SeedDataDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | TargetEditDAO (Library.DAO/DoctorModule_DAO/BonusCampaignNewMasterDAO.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| HiddenField1 | HiddenField |  | no validator found |  |  |  |  |
| ddlFinancialYear | DropDownList |  | no validator found |  | Enabled=false | see §9 |  |
| ddlYear | DropDownList |  | no validator found |  | Enabled=false | see §9 |  |
| ddlMonth | DropDownList |  | no validator found |  | Enabled=false | see §9 |  |
| ddlEmployeeName | DropDownList |  | no validator found |  | Enabled=false | see §9 |  |
| txtTargetValue | TextBox | Target Value | no validator found |  |  |  |  |
| FilteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| btnSave | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| id_mastetID | TargetEditDAO.SL | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveTargetEdit | @SL | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlFinancialYear | TargetEditDAO.FYId | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveTargetEdit | @FYId | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlYear | TargetEditDAO.YearValue | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveTargetEdit | @YearValue | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlMonth | TargetEditDAO.MonthName | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveTargetEdit | @MonthName | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlEmployeeName | TargetEditDAO.EmpId | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveTargetEdit | @EmpId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtTargetValue | TargetEditDAO.Value | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | (none — page calls DAL directly) | BonusCampaignNewDAL.SaveTargetEdit | @Value | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlFinancialYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlMonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlEmployeeName | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | SInventory_UI_TargetExcelUploadEdit.btnSave_Click | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx.cs:134 | — | BonusCampaignNewDAL.SaveTargetEdit | sp_Update_TargetInfo | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_TargetExcelUploadEdit.Page_Load | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx.cs:20 | object sender, EventArgs e | SInventory_UI_TargetExcelUploadEdit.Page_Load → BonusCampaignNewDAL.GetTargetEditById / CommonDataLoad.GetEmployeeList_All / SeedDataDAL.GetFiscalYearList → SP: sp_CS_GetEmpGeneralInfo_All, sp_GET_TargetEdit_ById, sp_GET_tFiscalYearList | yes | no |
| SInventory_UI_TargetExcelUploadEdit.GetOneRecord | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx.cs:79 | string Id | SInventory_UI_TargetExcelUploadEdit.GetOneRecord → BonusCampaignNewDAL.GetTargetEditById → SP: sp_GET_TargetEdit_ById | yes | no |
| SInventory_UI_TargetExcelUploadEdit.btnSave_Click | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx.cs:134 | object sender, EventArgs e | SInventory_UI_TargetExcelUploadEdit.btnSave_Click → BonusCampaignNewDAL.SaveTargetEdit → SP: sp_Update_TargetInfo | no | yes |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SeedDataDAL.GetFiscalYearList | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs:1066 | sp_GET_tFiscalYearList |  |
| CommonDataLoad.GetEmployeeList_All | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1761 | sp_CS_GetEmpGeneralInfo_All |  |
| BonusCampaignNewDAL.GetTargetEditById | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:1161 | sp_GET_TargetEdit_ById | @id |
| BonusCampaignNewDAL.SaveTargetEdit | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:378 | sp_Update_TargetInfo | @EmpId, @FYId, @MonthName, @SL, @UpdateBy, @Value, @YearValue |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | CommonDataLoad.GetEmployeeList_All | body NOT in repository | read-like name (INFERRED) |
| sp_GET_TargetEdit_ById | BonusCampaignNewDAL.GetTargetEditById | body NOT in repository | read-like name (INFERRED) |
| sp_GET_tFiscalYearList | SeedDataDAL.GetFiscalYearList | body NOT in repository | read-like name (INFERRED) |
| sp_Update_TargetInfo | BonusCampaignNewDAL.SaveTargetEdit | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','TargetExcelUploadList.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 6 methods.
- Redirect targets: none
- User messages: ','Success','TargetExcelUploadList.aspx');; Already Exist!; Please Select From List; form-control form-control-sm is-invalid; form-select form-select-sm mb-3 mySelect2; form-select form-select-sm mb-3 mySelect2 is-invalid
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: BonusCampaignNewDAL, CommonDataLoad, SeedDataDAL; DAO: ResultInfo, TargetEditDAO; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx`, `Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 15 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/TargetExcelUploadEdit.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M14 (Target & DWSP Management).
- Other pages sharing this page's stored procedures: PAGE-548, PAGE-549, PAGE-556, PAGE-557, PAGE-562, PAGE-563, PAGE-564, PAGE-570, PAGE-571, PAGE-578, PAGE-582, PAGE-587, PAGE-588, PAGE-610, PAGE-616 …
- Changing DAL methods BonusCampaignNewDAL.GetTargetEditById, BonusCampaignNewDAL.SaveTargetEdit, CommonDataLoad.GetEmployeeList_All, SeedDataDAL.GetFiscalYearList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
