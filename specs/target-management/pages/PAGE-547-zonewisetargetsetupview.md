---
page_id: PAGE-547
module: M14
page: ZoneWiseTargetSetupView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-547 — ZoneWiseTargetSetupView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-547 |
| Module | M14 — Target & DWSP Management (INFERRED from folder + file name) |
| Page name | ZoneWiseTargetSetupView |
| Kind | List / View (INFERRED from name) |
| Web folder | DWSP |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | DWSP_ZoneWiseTargetSetupView |
| aspx size | 15243 bytes |
| Code-behind LOC | 512 |
| Inline script size | 1374 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ZoneWiseTargetSetupView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: National/zone/area/territory/MIO targets, monthly and product-wise targets, DWSP, target achievement reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/AreaWiseTargetSetupView.aspx`, `DWSP/TerritoryTargetSetupView.aspx`, `DWSP/ZoneWiseTargetSetup.aspx`

Outbound references: `groupwisepromoqtyentry.aspx`, `groupwisepromoqtyview.aspx`, `zonewisetargetsetup.aspx`

## 5. Route
`~/DWSP/ZoneWiseTargetSetupView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx |
| Code-behind | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | NationalTargetSetupDal (Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs) |
| DAL class | ZoneWiseTargetSetupDal (Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |
| DAO/entity class | ZoneWiseTargetDao (Library.DAO/DWSP_DAO/ZoneWiseTargetDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| id_mastetID | HiddenField |  | no validator found |  |  |  |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| groupname | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→groupname_SelectedIndexChanged |
| chkQty | CheckBox |  Is Common QTY: | no validator found |  |  |  | OnCheckedChanged→chkQty_CheckedChanged |
| amount | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| gv_List | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| HFZoneId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Zone | no validator found |  |  |  |  |
| (no id) | TemplateField | Amount | no validator found |  |  |  |  |
| txtAmount | TextBox | <%#Eval("Amount") %> | no validator found |  | ReadOnly=true |  | OnTextChanged→txtAmount_OnTextChanged |
| FilteredTextBoxExtender8 | FilteredTextBoxExtender |  | no validator found |  |  |  |  |
| btnEdit | LinkButton |  | no validator found |  | Visible=false |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| ddlYear | ZoneWiseTargetDao.Year | DWSP_ZoneWiseTargetSetupView.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Year | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlmonth | ZoneWiseTargetDao.Month | DWSP_ZoneWiseTargetSetupView.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Month | UNKNOWN (SP body not in repo) | UNKNOWN |
| groupname | ZoneWiseTargetDao.GroupId | DWSP_ZoneWiseTargetSetupView.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @GroupId | UNKNOWN (SP body not in repo) | UNKNOWN |
| amount | ZoneWiseTargetDao.TargetAmount | DWSP_ZoneWiseTargetSetupView.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @TargetAmount | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAmount | ZoneWiseTargetDao.Amount | DWSP_ZoneWiseTargetSetupView.btnSave_Click | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |
| amount | target | DWSP_ZoneWiseTargetSetupView.txtAmount_OnTextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | param | DWSP_ZoneWiseTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | param | DWSP_ZoneWiseTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| groupname | param | DWSP_ZoneWiseTargetSetupView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| amount | target | DWSP_ZoneWiseTargetSetupView.UpDateValidation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| txtAmount | ZoneWiseTargetDao.Amount | DWSP_ZoneWiseTargetSetupView.loadGridView_RowCommand | (none — page calls DAL directly) | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | @Amount | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| groupname | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

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
| DWSP_ZoneWiseTargetSetupView.Page_Load | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:26 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.Page_Load → NationalTargetSetupDal.Get_Group_All → SP: sp_NTS_Group_Active | no | no |
| DWSP_ZoneWiseTargetSetupView.LoadInitialInfo | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:35 |  | DWSP_ZoneWiseTargetSetupView.LoadInitialInfo → NationalTargetSetupDal.Get_Group_All → SP: sp_NTS_Group_Active | yes | no |
| DWSP_ZoneWiseTargetSetupView.btnSave_Click | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:160 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.btnSave_Click → ZoneWiseTargetSetupDal.SaveZoneWiseTarget → SP: sp_Save_ZoneWiseTargetSetup | no | yes |
| DWSP_ZoneWiseTargetSetupView.Unnamed_Click | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:206 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.Unnamed_Click | no | no |
| DWSP_ZoneWiseTargetSetupView.gv_DocumentUpload_PreRender | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:209 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.gv_DocumentUpload_PreRender | no | no |
| DWSP_ZoneWiseTargetSetupView.groupname_SelectedIndexChanged | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:221 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.groupname_SelectedIndexChanged | no | no |
| DWSP_ZoneWiseTargetSetupView.txtAmount_OnTextChanged | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:256 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.txtAmount_OnTextChanged | no | no |
| DWSP_ZoneWiseTargetSetupView.btnSearch_Click | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:317 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.btnSearch_Click → ZoneWiseTargetSetupDal.Get_SaveDataAll → SP: sp_Get_ZoneWiseTargetList | no | no |
| DWSP_ZoneWiseTargetSetupView.resetBtn_Click | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:349 | object sender, EventArgs e | DWSP_ZoneWiseTargetSetupView.resetBtn_Click | no | no |
| DWSP_ZoneWiseTargetSetupView.GET_DataList | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:355 |  | DWSP_ZoneWiseTargetSetupView.GET_DataList → ZoneWiseTargetSetupDal.Get_SaveDataAll → SP: sp_Get_ZoneWiseTargetList | no | no |
| DWSP_ZoneWiseTargetSetupView.loadGridView_RowCommand | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs:461 | object sender, GridViewCommandEventArgs e | DWSP_ZoneWiseTargetSetupView.loadGridView_RowCommand → ZoneWiseTargetSetupDal.SaveZoneWiseTarget → SP: sp_Save_ZoneWiseTargetSetup | no | yes |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| NationalTargetSetupDal.Get_Group_All | Library.DAL/DWSP_DAL/NationalTargetSetupDal.cs:71 | sp_NTS_Group_Active |  |
| ZoneWiseTargetSetupDal.SaveZoneWiseTarget | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:17 | sp_Save_ZoneWiseTargetSetup | @Amount, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @Year, @ZoneWTSetupId |
| ZoneWiseTargetSetupDal.Get_SaveDataAll | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs:230 | sp_Get_ZoneWiseTargetList | @Parameter |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_ZoneWiseTargetList | ZoneWiseTargetSetupDal.Get_SaveDataAll | body NOT in repository | read-like name (INFERRED) |
| sp_NTS_Group_Active | NationalTargetSetupDal.Get_Group_All | body NOT in repository | write-like name |
| sp_Save_ZoneWiseTargetSetup | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','ZoneWiseTargetSetupView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Target Amount | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 clsDecimal is-invalid | UNKNOWN |
| (handler txtAmount_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler UpDateValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Amount must be equal with target amount | UNKNOWN |
| (handler UpDateValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm mb-3 is-invalid | UNKNOWN |
| (handler UpDateValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','ZoneWiseTargetSetupView.aspx'); | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 17 methods.
- Redirect targets: ~/PromoAlloc/GroupWisePromoQtyView.aspx
- User messages: ','Success','ZoneWiseTargetSetupView.aspx');; Already Exist!; Amount must be equal with target amount; Please Select From List; Please Select Target Amount; form-control form-control-sm mb-3 clsDecimal is-invalid
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: NationalTargetSetupDal, ZoneWiseTargetSetupDal; DAO: ResultInfo, ZoneWiseTargetDao; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx`, `Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 23 controls parsed | n/a | PARTIAL | Solution.Web/DWSP/ZoneWiseTargetSetupView.aspx |
| Data access | This document §15–17 | 3 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 3 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M14 (Target & DWSP Management).
- Other pages sharing this page's stored procedures: PAGE-538, PAGE-539, PAGE-540, PAGE-541, PAGE-542, PAGE-543, PAGE-544, PAGE-545, PAGE-546
- Changing DAL methods NationalTargetSetupDal.Get_Group_All, ZoneWiseTargetSetupDal.Get_SaveDataAll, ZoneWiseTargetSetupDal.SaveZoneWiseTarget affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
