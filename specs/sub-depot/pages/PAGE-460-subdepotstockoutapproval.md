---
page_id: PAGE-460
module: M12
page: SubDepotStockOutApproval
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-460 — SubDepotStockOutApproval

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-460 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubDepotStockOutApproval |
| Kind | Approval (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_DcStockOutApproval |
| aspx size | 10872 bytes |
| Code-behind LOC | 225 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SubDepotStockOutApproval** (Approval). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sub-depot info, stock in/out/transfer, invoicing, payments, top sheets, adjustments and freeze.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `subdepotstockoutreportviewer.aspx`

## 5. Route
`~/SubDepot_UI/SubDepotStockOutApproval.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | SubDepotStockAdjustmentsVoucherBll (Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs) |
| DAL class | SubDepotStockAdjustmentsVoucherDal (Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs) |
| DAO/entity class | DcStockOutMasterDao (Library.DAO/SInventory_Entities/DcStockOutMasterDao.cs) |
| DAO/entity class | SubdepotStockOutMasterDao (Library.DAO/SubDepot_DAO/SubdepotStockOutMasterDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| statusRadioButtonList | RadioButtonList |  | no validator found |  |  |  |  |
| submitButton | Button | Submit | no validator found |  |  |  | OnClick→btnSubmit0_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand |
| (no id) | BoundField | ComUnit Name | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Reason | no validator found |  |  |  |  |
| (no id) | BoundField | StockOut Date  | no validator found |  |  |  |  |
| (no id) | BoundField | Status | no validator found |  |  |  |  |
| (no id) | TemplateField | View | no validator found |  |  |  |  |
| reportImageButton | ImageButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| statusRadioButtonList | DcStockOutMasterDao.Status | SInventory_UI_DcStockOutApproval.SubmitApproval | SubDepotStockAdjustmentsVoucherBll.ApprovalUpdateBLL, SubDepotStockAdjustmentsVoucherBll.UpdateStockOutMasterDataForAppr | SubDepotStockAdjustmentsVoucherDal.UpdateStockOutMasterDataForApprovalDal | @Status | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| statusRadioButtonList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| submitButton | Button click | (postback) | SInventory_UI_DcStockOutApproval.btnSubmit0_Click | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:119 | SubDepotStockAdjustmentsVoucherBll.ApprovalUpdateBLL, SubDepotStockAdjustmentsVoucherBll.SubDcStockO | SubDepotStockAdjustmentsVoucherDal.ApprovalUpdateDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockO | sp_UD_SubDcStockOutApproval | writes tblDeStockOutMaster |

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
| SInventory_UI_DcStockOutApproval.Page_Load | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:24 | object sender, EventArgs e | SInventory_UI_DcStockOutApproval.Page_Load → SubDepotStockAdjustmentsVoucherBll.GetAssignedAppUser / SubDepotStockAdjustmentsVoucherBll.GetMenuIdByMenuName / SubDepotStockAdjustmentsVoucherBll.SubDcStockOutApprovalViewBll → SubDepotStockAdjustmentsVoucherDal.G | no | no |
| SInventory_UI_DcStockOutApproval.DcStockOutViewForApproval | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:40 |  | SInventory_UI_DcStockOutApproval.DcStockOutViewForApproval → SubDepotStockAdjustmentsVoucherBll.SubDcStockOutApprovalViewBll → SubDepotStockAdjustmentsVoucherDal.SubDcStockOutApprovalViewDal | no | no |
| SInventory_UI_DcStockOutApproval.DataTrue | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:50 |  | SInventory_UI_DcStockOutApproval.DataTrue → SubDepotStockAdjustmentsVoucherBll.GetAssignedAppUser / SubDepotStockAdjustmentsVoucherBll.GetMenuIdByMenuName → SubDepotStockAdjustmentsVoucherDal.GetAssignedAppUser / SubDepotStockAdjustmentsVoucherDal.GetMenuIdByM | no | no |
| SInventory_UI_DcStockOutApproval.loadGridView_RowCommand | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:85 | object sender, GridViewCommandEventArgs e | SInventory_UI_DcStockOutApproval.loadGridView_RowCommand | no | no |
| SInventory_UI_DcStockOutApproval.chkSelectAll_CheckedChanged | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:101 | object sender, EventArgs e | SInventory_UI_DcStockOutApproval.chkSelectAll_CheckedChanged | no | no |
| SInventory_UI_DcStockOutApproval.btnSubmit0_Click | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:119 | object sender, EventArgs e | SInventory_UI_DcStockOutApproval.btnSubmit0_Click → SubDepotStockAdjustmentsVoucherBll.ApprovalUpdateBLL / SubDepotStockAdjustmentsVoucherBll.SubDcStockOutApprovalViewBll / SubDepotStockAdjustmentsVoucherBll.UpdateStockOutMasterDataForApprovalBll → SubDepotSto | no | no |
| SInventory_UI_DcStockOutApproval.SubmitApproval | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs:171 |  | SInventory_UI_DcStockOutApproval.SubmitApproval → SubDepotStockAdjustmentsVoucherBll.ApprovalUpdateBLL / SubDepotStockAdjustmentsVoucherBll.UpdateStockOutMasterDataForApprovalBll → SubDepotStockAdjustmentsVoucherDal.ApprovalUpdateDal / SubDepotStockAdjustments | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotStockAdjustmentsVoucherBll.GetMenuIdByMenuName | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:112 |
| SubDepotStockAdjustmentsVoucherBll.GetAssignedAppUser | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:118 |
| SubDepotStockAdjustmentsVoucherBll.SubDcStockOutApprovalViewBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:132 |
| SubDepotStockAdjustmentsVoucherBll.UpdateStockOutMasterDataForApprovalBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:146 |
| SubDepotStockAdjustmentsVoucherBll.ApprovalUpdateBLL | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:125 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotStockAdjustmentsVoucherDal.GetMenuIdByMenuName | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:90 | inline SQL: tblMainMenu |  |
| SubDepotStockAdjustmentsVoucherDal.GetAssignedAppUser | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:96 | inline SQL: tblAppSetup |  |
| SubDepotStockAdjustmentsVoucherDal.SubDcStockOutApprovalViewDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:114 | inline SQL: tblCompanyUnit, tblInvoice, tblSubDepotStockOutMaster |  |
| SubDepotStockAdjustmentsVoucherDal.UpdateStockOutMasterDataForApprovalDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:141 | sp_UD_SubDcStockOutApproval | @ApprovedBy, @ApprovedDate, @Status, @SubDcStockOutMasterId |
| SubDepotStockAdjustmentsVoucherDal.ApprovalUpdateDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:103 | inline SQL: tblDeStockOutMaster |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_UD_SubDcStockOutApproval | SubDepotStockAdjustmentsVoucherDal.UpdateStockOutMasterDataForApprovalDal | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblAppSetup, tblCompanyUnit, tblInvoice, tblMainMenu, tblSubDepotStockOutMaster
- Tables WRITTEN (inline SQL reachable): tblDeStockOutMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler btnSubmit0_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select at least one row!!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please select an operation!! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName, SubDcStockOutMasterId, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: none
- User messages: Please select an operation!!; Please select at least one row!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: SubDepotStockAdjustmentsVoucherBll; DAL: SubDepotStockAdjustmentsVoucherDal; DAO: DcStockOutMasterDao, SubdepotStockOutMasterDao; Tables (inline): tblAppSetup, tblCompanyUnit, tblDeStockOutMaster, tblInvoice, tblMainMenu, tblSubDepotStockOutMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click submitButton with valid data | Handler runs; rows written to tblDeStockOutMaster |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx`, `Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 18 controls parsed | n/a | PARTIAL | Solution.Web/SubDepot_UI/SubDepotStockOutApproval.aspx |
| Data access | This document §15–17 | 1 SP names, 6 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.

## 31. Change Impact
- Module: M12 (Sub-Depot Operations).
- Other pages sharing this page's stored procedures: PAGE-443
- Changing DAL methods SubDepotStockAdjustmentsVoucherDal.ApprovalUpdateDal, SubDepotStockAdjustmentsVoucherDal.GetAssignedAppUser, SubDepotStockAdjustmentsVoucherDal.GetMenuIdByMenuName, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutApprovalViewDal, SubDepotStockAdjustmentsVoucherDal.UpdateStockOutMasterDataForApprovalDal affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
