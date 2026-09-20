---
page_id: PAGE-237
module: M07
page: DASetup
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-237 — DASetup

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-237 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | DASetup |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | MasterSetup_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | MasterSetup_UI_DASetup |
| aspx size | 15869 bytes |
| Code-behind LOC | 402 |
| Inline script size | 1453 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **DASetup** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Proforma and invoice creation (manual/auto/by-order), challans, top sheets, picking, loading, delivery assistants, invoice printing.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `MasterSetup_UI/DAList.aspx`, `MasterSetup_UI/TerritoryWiseDepotView.aspx`

Outbound references: `campaignview.aspx`, `dalist.aspx`

## 5. Route
`~/MasterSetup_UI/DASetup.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/MasterSetup_UI/DASetup.aspx |
| Code-behind | Solution.Web/MasterSetup_UI/DASetup.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | OtherStockActionBLL (Library.BLL/SInventory_BLL/OtherStockActionBLL.cs) |
| DAL class | DAInfoDal (Library.DAL/MasterSetup_DAL/DAInfoDal.cs) |
| DAL class | OtherStockActionDAL (Library.DAL/SInventory_DAL/OtherStockActionDAL.cs) |
| DAO/entity class | DAInfoDao (Library.DAO/MasterSetup_DAO/DAInfoDao.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlDepotName | DropDownList |  | no validator found |  |  | see §9 |  |
| txtNID | TextBox | Delivery Man NID | no validator found |  |  |  |  |
| FilteredTextBoxExtenderunitValue | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| hiddenField | HiddenField |  | no validator found |  |  |  |  |
| txtName | TextBox | Delivery Man Name | no validator found |  |  |  |  |
| txtJoiningDate | TextBox | Select Date of Joining | no validator found |  |  |  |  |
| txtAddress | TextBox | Permanent Address | no validator found |  |  |  |  |
| txtPhone | TextBox | Delivery Man Phone | no validator found |  |  |  |  |
| FilteredTssextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtEmergencyContactNo | TextBox | Emergency Contact No | no validator found |  |  |  |  |
| FilteresssdTextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| txtReferenceName | TextBox | Reference Name | no validator found |  |  |  |  |
| txtReferencePhone | TextBox | Reference Phone | no validator found |  |  |  |  |
| FiltertxtReferencePhoneedTextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| chkIsActive | CheckBox |  | no validator found | true |  |  | onclick→IsActiveChange() |
| txtActiveDate | TextBox | Select Date | no validator found |  |  |  |  |
| txtRemarks | TextBox | Remarks | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnUpdate | LinkButton |  | no validator found |  | Visible=false |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Update(this); |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| txtNID | DAInfoDao.NID | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @NID | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtName | DAInfoDao.Name | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @Name | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtPhone | DAInfoDao.PhoneNo | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @PhoneNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtAddress | DAInfoDao.Address | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @Address | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtEmergencyContactNo | DAInfoDao.EmergencyContactNo | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @EmergencyContactNo | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtReferenceName | DAInfoDao.ReferenceName | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @ReferenceName | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtReferencePhone | DAInfoDao.ReferencePhone | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @ReferencePhone | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtRemarks | DAInfoDao.Remarks | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @Remarks | UNKNOWN (SP body not in repo) | UNKNOWN |
| ddlDepotName | DAInfoDao.ComUnitId | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @ComUnitId | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtJoiningDate | DAInfoDao.JoiningDate | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @JoiningDate | UNKNOWN (SP body not in repo) | UNKNOWN |
| chkIsActive | DAInfoDao.IsActive | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @IsActive | UNKNOWN (SP body not in repo) | UNKNOWN |
| txtActiveDate | activeInactiveDate | MasterSetup_UI_DASetup.btnSave_Click | — | DAInfoDal.SaveDAInfo | UNKNOWN | UNKNOWN | UNKNOWN |
| hiddenField | DAInfoDao.DAId | MasterSetup_UI_DASetup.btnSave_Click | (none — page calls DAL directly) | DAInfoDal.SaveDAInfo | @DAId | UNKNOWN (SP body not in repo) | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlDepotName | LoadDepotList → OtherStockActionBLL.DCLoad | inline SQL | Com/ComUnitId, ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | MasterSetup_UI_DASetup.btnSave_Click | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:289 | — | DAInfoDal.SaveDAInfo | sp_Save_DAInfo, sp_Update_DAInfo | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnUpdate | LinkButton click | return sweetAlertConfirm_Update(this); | MasterSetup_UI_DASetup.btnSave_Click | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:289 | — | DAInfoDal.SaveDAInfo | sp_Save_DAInfo, sp_Update_DAInfo | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | MasterSetup_UI_DASetup.btnReset_Click | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:398 | — | — | — | read-only / none detected |

## 11. Frontend Flow
- Client script functions in page: IsActiveChange, pageLoad
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: no; File upload: no; Grid/table markup: no
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| MasterSetup_UI_DASetup.Page_Load | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:16 | object sender, EventArgs e | MasterSetup_UI_DASetup.Page_Load → OtherStockActionBLL.DCLoad → DAInfoDal.GetDAInfoById / OtherStockActionDAL.DCLoad → SP: sp_GET_DAInfoById | no | no |
| MasterSetup_UI_DASetup.LoadDepotList | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:38 |  | MasterSetup_UI_DASetup.LoadDepotList → OtherStockActionBLL.DCLoad → OtherStockActionDAL.DCLoad | yes | no |
| MasterSetup_UI_DASetup.GetOneRecord | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:78 | int daId | MasterSetup_UI_DASetup.GetOneRecord → DAInfoDal.GetDAInfoById → SP: sp_GET_DAInfoById | no | no |
| MasterSetup_UI_DASetup.btnSave_Click | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:289 | object sender, EventArgs e | MasterSetup_UI_DASetup.btnSave_Click → DAInfoDal.SaveDAInfo → SP: sp_Save_DAInfo, sp_Update_DAInfo | no | yes |
| MasterSetup_UI_DASetup.btnReset_Click | Solution.Web/MasterSetup_UI/DASetup.aspx.cs:398 | object sender, EventArgs e | MasterSetup_UI_DASetup.btnReset_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OtherStockActionBLL.DCLoad | Library.BLL/SInventory_BLL/OtherStockActionBLL.cs:28 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DAInfoDal.GetDAInfoById | Library.DAL/MasterSetup_DAL/DAInfoDal.cs:98 | sp_GET_DAInfoById | @Parameter |
| OtherStockActionDAL.DCLoad | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:94 | inline SQL: tblCompanyUnit, tblUserCompanyUnit | @UserId |
| DAInfoDal.SaveDAInfo | Library.DAL/MasterSetup_DAL/DAInfoDal.cs:19 | sp_Save_DAInfo, sp_Update_DAInfo | @ActiveDate, @Address, @ComUnitId, @DAId, @EmergencyContactNo, @EntryBy, @EntryDate, @InactiveDate,  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_GET_DAInfoById | DAInfoDal.GetDAInfoById | body NOT in repository | read-like name (INFERRED) |
| sp_Save_DAInfo | DAInfoDal.SaveDAInfo | body NOT in repository | write-like name |
| sp_Update_DAInfo | DAInfoDal.SaveDAInfo | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Delivery Man NID must be 17 digits! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Emergency Contact No must be 11 digits! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Phone No must be 11 digits! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Reference Phone must be 11 digits! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm datepicker is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','CampaignView.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','DAList.aspx'); | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: DAEdit, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 10 methods.
- Redirect targets: DASetup.aspx
- User messages: ','Success','CampaignView.aspx');; ','Success','DAList.aspx');; Already Exist!; Delivery Man NID must be 17 digits!; Emergency Contact No must be 11 digits!; Phone No must be 11 digits!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: OtherStockActionBLL; DAL: DAInfoDal, OtherStockActionDAL; DAO: DAInfoDao; Tables (inline): tblCompanyUnit, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnUpdate with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnUpdate with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click btnReset with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click btnReset with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/MasterSetup_UI/DASetup.aspx`, `Solution.Web/MasterSetup_UI/DASetup.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/MasterSetup_UI/DASetup.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/MasterSetup_UI/DASetup.aspx |
| Data access | This document §15–17 | 3 SP names, 2 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 3 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M07 (Proforma, Invoicing & Delivery).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods DAInfoDal.GetDAInfoById, DAInfoDal.SaveDAInfo, OtherStockActionDAL.DCLoad affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
