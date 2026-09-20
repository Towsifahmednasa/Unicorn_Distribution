---
page_id: PAGE-407
module: M11
page: B2BTransferView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-407 — B2BTransferView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-407 |
| Module | M11 — Stock Transfer & Chalan Receive (INFERRED from folder + file name) |
| Page name | B2BTransferView |
| Kind | List / View (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_B2BTransferView |
| aspx size | 18913 bytes |
| Code-behind LOC | 168 |
| Inline script size | 867 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **B2BTransferView** (List / View). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: DC-to-DC, DC-to-Warehouse, B2B, stock transport orders, chalan receive, in-transit tracking.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/NewStockTransferDcToDc.aspx`

Outbound references: `newstocktransferdctodc.aspx`, `transferstockreceivereportviewer.aspx`

## 5. Route
`~/SInventory_UI/B2BTransferView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/B2BTransferView.aspx |
| Code-behind | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | B2BTransferViewDal (Library.DAL/SInventory_DAL/B2BTransferViewDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| viewLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→custCetegoryAddImageButton_Click |
| fromDateTextBox | TextBox |  | no validator found |  |  |  |  |
| toDateTextBox | TextBox |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| cancelButton | LinkButton |  | no validator found |  |  |  | OnClick→cancelButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| (no id) | BoundField | Chalan No | no validator found |  |  |  |  |
| (no id) | BoundField | Chalan Date | no validator found |  |  |  |  |
| (no id) | BoundField | WearhouseName | no validator found |  |  |  |  |
| (no id) | BoundField | From (DC) | no validator found |  |  |  |  |
| (no id) | BoundField | To (DC) | no validator found |  |  |  |  |
| (no id) | BoundField | Total Value | no validator found |  |  |  |  |
| (no id) | BoundField | Total Vat | no validator found |  |  |  |  |
| (no id) | BoundField | Grand Total | no validator found |  |  |  |  |
| (no id) | BoundField | Status | no validator found |  |  |  |  |
| (no id) | TemplateField | Report | no validator found |  |  |  |  |
| btPrint | LinkButton |  | no validator found |  |  |  | OnClick→btPrint_Click |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | LinkButton |  | no validator found |  |  |  | OnClientClick→return sweetAlertConfirm_Delete(this);, OnClick→editImageButton_Click |
| EmpCetegoryAddImageButton | ImageButton |  | no validator found |  |  |  | OnClick→custCetegoryAddImageButton_Click |
| EmpCetegoryReloadImageButton | ImageButton |  | no validator found |  |  |  | onclick→custCetegoryReloadImageButton_Click |
| cusMasterNewImageButton | ImageButton |  | no validator found |  |  |  | onclick→CustMasterNewImageButton_Click |
| submitButton | Button | Search | no validator found |  |  |  | OnClick→searchButton_Click |
| submitButton0 | Button | Cancel | no validator found |  |  |  | OnClick→submitButton_OnClick |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| fromDateTextBox | parameter | SInventory_UI_B2BTransferView.GetPrameterList | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton click | (postback) | SInventory_UI_B2BTransferView.custCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:79 | — | — | — | read-only / none detected |
| btnSearch | LinkButton click | (postback) | SInventory_UI_B2BTransferView.searchButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:96 | — | B2BTransferViewDal.GetB2bTransferInfo | — | read-only / none detected |
| cancelButton | LinkButton click | (postback) | SInventory_UI_B2BTransferView.cancelButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:23 | — | — | — | read-only / none detected |
| btPrint | LinkButton click | (postback) | SInventory_UI_B2BTransferView.btPrint_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:143 | — | — | — | read-only / none detected |
| editImageButton | LinkButton click | return sweetAlertConfirm_Delete(this); | SInventory_UI_B2BTransferView.editImageButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:109 | — | B2BTransferViewDal.DeleteChallanDetailById, B2BTransferViewDal.DeleteChallanMasterById, B2BTransferV | sp_Del_B2BDelete | writes tblChalanInfo |
| EmpCetegoryAddImageButton | ImageButton click | (postback) | SInventory_UI_B2BTransferView.custCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:79 | — | — | — | read-only / none detected |
| submitButton | Button click | (postback) | SInventory_UI_B2BTransferView.searchButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:96 | — | B2BTransferViewDal.GetB2bTransferInfo | — | read-only / none detected |
| submitButton0 | Button click | (postback) | SInventory_UI_B2BTransferView.submitButton_OnClick | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:101 | — | B2BTransferViewDal.GetB2bTransferInfo | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_UI_B2BTransferView.Page_Load | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:16 | object sender, EventArgs e | SInventory_UI_B2BTransferView.Page_Load → B2BTransferViewDal.GetB2bTransferInfo | no | no |
| SInventory_UI_B2BTransferView.cancelButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:23 | object sender, EventArgs e | SInventory_UI_B2BTransferView.cancelButton_Click | no | no |
| SInventory_UI_B2BTransferView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:28 | object sender, EventArgs e | SInventory_UI_B2BTransferView.gv_DocumentUpload_PreRender | no | no |
| SInventory_UI_B2BTransferView.custCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:39 | object sender, EventArgs e | SInventory_UI_B2BTransferView.custCetegoryAddImageButton_Click | no | no |
| SInventory_UI_B2BTransferView.LoadB2BInformation | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:43 |  | SInventory_UI_B2BTransferView.LoadB2BInformation → B2BTransferViewDal.GetB2bTransferInfo | no | no |
| SInventory_UI_B2BTransferView.custCetegoryAddImageButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:79 | object sender, ImageClickEventArgs e | SInventory_UI_B2BTransferView.custCetegoryAddImageButton_Click | no | no |
| SInventory_UI_B2BTransferView.ImageButton2_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:84 | object sender, ImageClickEventArgs e | SInventory_UI_B2BTransferView.ImageButton2_Click | no | no |
| SInventory_UI_B2BTransferView.searchButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:96 | object sender, EventArgs e | SInventory_UI_B2BTransferView.searchButton_Click → B2BTransferViewDal.GetB2bTransferInfo | no | no |
| SInventory_UI_B2BTransferView.submitButton_OnClick | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:101 | object sender, EventArgs e | SInventory_UI_B2BTransferView.submitButton_OnClick → B2BTransferViewDal.GetB2bTransferInfo | no | no |
| SInventory_UI_B2BTransferView.editImageButton_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:109 | object sender, EventArgs e | SInventory_UI_B2BTransferView.editImageButton_Click → B2BTransferViewDal.DeleteChallanDetailById / B2BTransferViewDal.DeleteChallanMasterById / B2BTransferViewDal.GetB2bTransferInfo → SP: sp_Del_B2BDelete | no | no |
| SInventory_UI_B2BTransferView.btPrint_Click | Solution.Web/SInventory_UI/B2BTransferView.aspx.cs:143 | object sender, EventArgs e | SInventory_UI_B2BTransferView.btPrint_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| B2BTransferViewDal.GetB2bTransferInfo | Library.DAL/SInventory_DAL/B2BTransferViewDal.cs:19 | inline SQL: tblChalanInfo |  |
| B2BTransferViewDal.DeleteChallanMasterById | Library.DAL/SInventory_DAL/B2BTransferViewDal.cs:28 | inline SQL: tblChalanInfo, tblChalanInfo | @ChalanId |
| B2BTransferViewDal.DeleteChallanDetailById | Library.DAL/SInventory_DAL/B2BTransferViewDal.cs:45 | sp_Del_B2BDelete | @chalanId |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Del_B2BDelete | B2BTransferViewDal.DeleteChallanDetailById | SP_Fix_STO_Chalan_Process.sql:24; SP_NewStockTransferDcToDc_Related.sql:22 | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblChalanInfo
- Tables WRITTEN (inline SQL reachable): tblChalanInfo
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
- try/catch present in 0 of 14 methods.
- Redirect targets: NewStockTransferDcToDc.aspx
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: B2BTransferViewDal; DAO: —; Tables (inline): tblChalanInfo

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewLinkButton with valid data | Handler runs; rows written to tblChalanInfo |
| TC-05 | Click btnSearch with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSearch with valid data | Handler runs; rows written to tblChalanInfo |
| TC-07 | Click cancelButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click cancelButton with valid data | Handler runs; rows written to tblChalanInfo |
| TC-09 | Click btPrint with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btPrint with valid data | Handler runs; rows written to tblChalanInfo |
| TC-11 | Click editImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-12 | Click editImageButton with valid data | Handler runs; rows written to tblChalanInfo |
| TC-13 | Click EmpCetegoryAddImageButton with required fields empty | Validator/server message shown; no DB write |
| TC-14 | Click EmpCetegoryAddImageButton with valid data | Handler runs; rows written to tblChalanInfo |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/B2BTransferView.aspx`, `Solution.Web/SInventory_UI/B2BTransferView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/B2BTransferView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 32 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/B2BTransferView.aspx |
| Data access | This document §15–17 | 1 SP names, 1 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M11 (Stock Transfer & Chalan Receive).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods B2BTransferViewDal.DeleteChallanDetailById, B2BTransferViewDal.DeleteChallanMasterById, B2BTransferViewDal.GetB2bTransferInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
