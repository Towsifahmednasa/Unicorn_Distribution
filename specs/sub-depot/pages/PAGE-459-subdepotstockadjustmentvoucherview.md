---
page_id: PAGE-459
module: M12
page: SubDepotStockAdjustmentVoucherView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-459 — SubDepotStockAdjustmentVoucherView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-459 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | SubDepotStockAdjustmentVoucherView |
| Kind | List / View (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_SubDepotStockAdjustmentVoucherView |
| aspx size | 6592 bytes |
| Code-behind LOC | 104 |
| Inline script size | 2043 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SubDepotStockAdjustmentVoucherView** (List / View). Page title/heading text: `Sub Depot Stock Adjustments Voucher List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Sub-depot info, stock in/out/transfer, invoicing, payments, top sheets, adjustments and freeze.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SubDepot_UI/SubDepotStockAdjustmentVoucher.aspx`

Outbound references: `subdepotstockadjustmentvoucher.aspx`, `subdepotstockoutreportviewer.aspx`

## 5. Route
`~/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx |
| Code-behind | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | SubDepotStockAdjustmentsVoucherBll (Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs) |
| DAL class | SubDepotStockAdjustmentsVoucherDal (Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton |  | no validator found |  |  |  | OnClick→EmpCetegoryAddImageButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender, onrowcommand→loadGridView_RowCommand |
| (no id) | BoundField | Com Unit Name | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Reason | no validator found |  |  |  |  |
| (no id) | BoundField | StockOut Date  | no validator found |  |  |  |  |
| (no id) | BoundField | Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | LinkButton |  | no validator found |  |  |  | OnClientClick→return GetConfirmation(); |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| reportImageButton | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| EmpCetegoryAddImageButton | LinkButton click | (postback) | SubDepot_UI_SubDepotStockAdjustmentVoucherView.EmpCetegoryAddImageButton_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:100 | — | — | — | read-only / none detected |
| editImageButton | LinkButton | return GetConfirmation(); | — | — | — | — | — | UNKNOWN (handler not resolved) |

## 11. Frontend Flow
- Client script functions in page: GetConfirmation
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SubDepot_UI_SubDepotStockAdjustmentVoucherView.Page_Load | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:16 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucherView.Page_Load → SubDepotStockAdjustmentsVoucherBll.SubDcStockOutBll → SubDepotStockAdjustmentsVoucherDal.SubDcStockOutViewDal | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucherView.SubDcStockOutView | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:32 |  | SubDepot_UI_SubDepotStockAdjustmentVoucherView.SubDcStockOutView → SubDepotStockAdjustmentsVoucherBll.SubDcStockOutBll → SubDepotStockAdjustmentsVoucherDal.SubDcStockOutViewDal | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucherView.loadGridView_RowCommand | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:39 | object sender, GridViewCommandEventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucherView.loadGridView_RowCommand → SubDepotStockAdjustmentsVoucherBll.SubDcStockOutBll / SubDepotStockAdjustmentsVoucherBll.SubDcStockOutDetailsDelete / SubDepotStockAdjustmentsVoucherBll.SubDcStockOutMasterDelete → SubDep | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucherView.DcStockOutAddImageButton_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:82 | object sender, ImageClickEventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucherView.DcStockOutAddImageButton_Click | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucherView.gv_DocumentUpload_PreRender | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:88 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucherView.gv_DocumentUpload_PreRender | no | no |
| SubDepot_UI_SubDepotStockAdjustmentVoucherView.EmpCetegoryAddImageButton_Click | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs:100 | object sender, EventArgs e | SubDepot_UI_SubDepotStockAdjustmentVoucherView.EmpCetegoryAddImageButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotStockAdjustmentsVoucherBll.SubDcStockOutBll | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:96 |
| SubDepotStockAdjustmentsVoucherBll.SubDcStockOutMasterDelete | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:106 |
| SubDepotStockAdjustmentsVoucherBll.SubDcStockOutDetailsDelete | Library.BLL/SubDepot_BLL/SubDepotStockAdjustmentsVoucherBll.cs:101 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotStockAdjustmentsVoucherDal.SubDcStockOutViewDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:62 | inline SQL: tblCompanyUnit, tblSubDepotStockOutMaster, tblSubInvoiceMaster |  |
| SubDepotStockAdjustmentsVoucherDal.SubDcStockOutMasterDeleteDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:73 | inline SQL: tblSubDepotStockOutMaster, tblSubDepotStockOutMaster |  |
| SubDepotStockAdjustmentsVoucherDal.SubDcStockOutDetailsDeleteDal | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:81 | inline SQL: tblSubDepotStockOutDetails, tblSubDepotStockOutDetails |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblSubDepotStockOutDetails, tblSubDepotStockOutMaster, tblSubInvoiceMaster
- Tables WRITTEN (inline SQL reachable): tblSubDepotStockOutDetails, tblSubDepotStockOutMaster
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Can not Delete Data!!!.... | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: Status, SubDcStockOutMasterId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 7 methods.
- Redirect targets: SubDepotStockAdjustmentVoucher.aspx
- User messages: Can not Delete Data!!!....
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: SubDepotStockAdjustmentsVoucherBll; DAL: SubDepotStockAdjustmentsVoucherDal; DAO: —; Tables (inline): tblCompanyUnit, tblSubDepotStockOutDetails, tblSubDepotStockOutMaster, tblSubInvoiceMaster

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tblSubDepotStockOutDetails, tblSubDepotStockOutMaster |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx`, `Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 14 controls parsed | n/a | PARTIAL | Solution.Web/SubDepot_UI/SubDepotStockAdjustmentVoucherView.aspx |
| Data access | This document §15–17 | 0 SP names, 4 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M12 (Sub-Depot Operations).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods SubDepotStockAdjustmentsVoucherDal.SubDcStockOutDetailsDeleteDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutMasterDeleteDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutViewDal affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
