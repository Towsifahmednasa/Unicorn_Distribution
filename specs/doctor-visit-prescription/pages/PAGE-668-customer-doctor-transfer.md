---
page_id: PAGE-668
module: M16
page: Customer_Doctor_Transfer
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-668 — Customer_Doctor_Transfer

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-668 |
| Module | M16 — Doctor / Provider Master, Visits & Prescriptions (INFERRED from folder + file name) |
| Page name | Customer_Doctor_Transfer |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_Customer_Doctor_Transfer |
| aspx size | 14170 bytes |
| Code-behind LOC | 350 |
| Inline script size | 999 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **Customer_Doctor_Transfer** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Doctor master data, doctor visits/DCR/DCP, prescriptions (RX), doctor reports.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `Approval_UI/DoctorCustomerTransferApproval.aspx`, `TransferUI/MarketStructure_TransferApprove.aspx`

Outbound references: `customerdoctransferapproval.aspx`

## 5. Route
`~/MasterSetup_UI/Customer_Doctor_Transfer.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/MasterSetup_UI/IVMarketStructureFrom_To.ascx |
| User control | ~/MasterSetup_UI/IVMarketStructureTo.ascx |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | CustomerInfoDAL (Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs) |
| DAL class | DoctorDAL (Library.DAL/MasterSetup_DAL/DoctorDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→ListImageButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| rbType | RadioButtonList |  | no validator found |  |  |  | OnSelectedIndexChanged→rbType_SelectedIndexChanged |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | OnClick→btnSearch_Click |
| IVMarketStructureTo | uc1:IVMarketStructureTo |  | no validator found |  |  |  |  |
| gv_Customer_List | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfCustomerMasterId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelectAll_CheckedChanged |
| chkSelect | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkSelect_CheckedChanged |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Type | no validator found |  |  |  |  |
| (no id) | BoundField | Owner Name | no validator found |  |  |  |  |
| (no id) | BoundField | Mobile NO | no validator found |  |  |  |  |
| (no id) | BoundField | Address | no validator found |  |  |  |  |
| (no id) | BoundField | Distribution RouteName | no validator found |  |  |  |  |
| gv_Doctor_List | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | TemplateField | SL# | no validator found |  |  |  |  |
| hfDoctorId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | TemplateField |  | no validator found |  |  |  |  |
| chkDoctorSelectAll | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→chkDoctorSelectAll_CheckedChanged |
| chkDoctorSelect | CheckBox |  | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Code | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Name | no validator found |  |  |  |  |
| (no id) | BoundField | Designation | no validator found |  |  |  |  |
| (no id) | BoundField | Degree | no validator found |  |  |  |  |
| (no id) | BoundField | Doctor Speciality | no validator found |  |  |  |  |
| (no id) | BoundField | Chamber Info | no validator found |  |  |  |  |
| (no id) | BoundField | Mobile NO | no validator found |  |  |  |  |
| (no id) | BoundField | Address | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| hfCustomerMasterId | dt | MasterSetup_UI_Customer_Doctor_Transfer.CheckCustInOrder | — | CommonDataLoad.GetValidationCheck | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| rbType | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| viewLinkButton | LinkButton click | (postback) | MasterSetup_UI_Customer_Doctor_Transfer.ListImageButton_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:239 | — | — | — | read-only / none detected |
| btnSearch | LinkButton click | (postback) | MasterSetup_UI_Customer_Doctor_Transfer.btnSearch_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:243 | — | CustomerInfoDAL.GetCustomerList, DoctorDAL.GetDoctorList | sp_Get_CustMasterList_Approve, sp_Get_DoctorList | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | MasterSetup_UI_Customer_Doctor_Transfer.btnSave_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:171 | — | CustomerInfoDAL.UpdateCustomer_Doctor_Transfer | sp_Update_Customer_Doctor_Transfer | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | MasterSetup_UI_Customer_Doctor_Transfer.btnReset_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:347 | — | — | — | read-only / none detected |

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
| MasterSetup_UI_Customer_Doctor_Transfer.Page_Load | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:23 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.Page_Load | no | no |
| MasterSetup_UI_Customer_Doctor_Transfer.gv_DocumentUpload_PreRender | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:45 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.gv_DocumentUpload_PreRender | no | no |
| MasterSetup_UI_Customer_Doctor_Transfer.rbType_SelectedIndexChanged | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:57 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.rbType_SelectedIndexChanged | no | no |
| MasterSetup_UI_Customer_Doctor_Transfer.chkSelectAll_CheckedChanged | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:159 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.chkSelectAll_CheckedChanged → CommonDataLoad.GetValidationCheck → SP: sp_check_Vali_MarketStructure | no | no |
| MasterSetup_UI_Customer_Doctor_Transfer.btnSave_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:171 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.btnSave_Click → CustomerInfoDAL.UpdateCustomer_Doctor_Transfer → SP: sp_Update_Customer_Doctor_Transfer | no | yes |
| MasterSetup_UI_Customer_Doctor_Transfer.ListImageButton_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:239 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.ListImageButton_Click | no | no |
| MasterSetup_UI_Customer_Doctor_Transfer.btnSearch_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:243 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.btnSearch_Click → CustomerInfoDAL.GetCustomerList / DoctorDAL.GetDoctorList → SP: sp_Get_CustMasterList_Approve, sp_Get_DoctorList | yes | no |
| MasterSetup_UI_Customer_Doctor_Transfer.chkDoctorSelectAll_CheckedChanged | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:283 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.chkDoctorSelectAll_CheckedChanged | no | no |
| MasterSetup_UI_Customer_Doctor_Transfer.chkSelect_CheckedChanged | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:297 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.chkSelect_CheckedChanged → CommonDataLoad.GetValidationCheck → SP: sp_check_Vali_MarketStructure | yes | no |
| MasterSetup_UI_Customer_Doctor_Transfer.CheckCustInOrder | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:312 | int rowIndex | MasterSetup_UI_Customer_Doctor_Transfer.CheckCustInOrder → CommonDataLoad.GetValidationCheck → SP: sp_check_Vali_MarketStructure | yes | no |
| MasterSetup_UI_Customer_Doctor_Transfer.btnReset_Click | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs:347 | object sender, EventArgs e | MasterSetup_UI_Customer_Doctor_Transfer.btnReset_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetValidationCheck | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1582 | sp_check_Vali_MarketStructure | @MasterId, @PageName |
| CustomerInfoDAL.UpdateCustomer_Doctor_Transfer | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:136 | sp_Update_Customer_Doctor_Transfer | @ApprovedBy, @DCID, @MarketId, @MasterId, @RouteId, @Type |
| CustomerInfoDAL.GetCustomerList | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:406 | sp_Get_CustMasterList_Approve | @Parm |
| DoctorDAL.GetDoctorList | Library.DAL/MasterSetup_DAL/DoctorDAL.cs:417 | sp_Get_DoctorList | @Parm |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_CustMasterList_Approve | CustomerInfoDAL.GetCustomerList | LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:820 | write-like name |
| sp_Get_DoctorList | DoctorDAL.GetDoctorList | LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:883 | read-like name (INFERRED) |
| sp_Update_Customer_Doctor_Transfer | CustomerInfoDAL.UpdateCustomer_Doctor_Transfer | body NOT in repository | write-like name |
| sp_check_Vali_MarketStructure | CommonDataLoad.GetValidationCheck | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select at least one employee !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Table can not be Empty! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','Customer_Doctor_Transfer.aspx'); | UNKNOWN |
| (handler btnSearch_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Market! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 3 of 13 methods.
- Redirect targets: ../TransferUI/CustomerDocTransferApproval.aspx, Customer_Doctor_Transfer.aspx
- User messages: ','Success','Customer_Doctor_Transfer.aspx');; Please Select Market!; Please Select at least one employee !!!; Table can not be Empty!; form-select form-select-sm mb-3 mySelect2; form-select form-select-sm mb-3 mySelect2 is-invalid
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, CustomerInfoDAL, DoctorDAL; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click viewLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click viewLinkButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnSearch with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnSearch with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-09 | Click btnReset with required fields empty | Validator/server message shown; no DB write |
| TC-10 | Click btnReset with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx`, `Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 41 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/Customer_Doctor_Transfer.aspx |
| Data access | This document §15–17 | 4 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 4 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M16 (Doctor / Provider Master, Visits & Prescriptions).
- Other pages sharing this page's stored procedures: PAGE-042, PAGE-046, PAGE-057, PAGE-064, PAGE-099, PAGE-103, PAGE-105, PAGE-167, PAGE-169, PAGE-173, PAGE-505, PAGE-566, PAGE-568, PAGE-572, PAGE-583 …
- Changing DAL methods CommonDataLoad.GetValidationCheck, CustomerInfoDAL.GetCustomerList, CustomerInfoDAL.UpdateCustomer_Doctor_Transfer, DoctorDAL.GetDoctorList affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
