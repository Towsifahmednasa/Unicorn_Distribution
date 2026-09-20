---
page_id: PAGE-100
module: M03
page: SubTerritory_Transfer
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-100 — SubTerritory_Transfer

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-100 |
| Module | M03 — Organisation & Market Structure Master (INFERRED from folder + file name) |
| Page name | SubTerritory_Transfer |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | TransferUI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | TransferUI_SubTerritory_Transfer |
| aspx size | 9351 bytes |
| Code-behind LOC | 295 |
| Inline script size | 993 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SubTerritory_Transfer** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Company/branch/unit, geography (division/district/thana), sales hierarchy (national/zone/region/area/territory/market), routes, depot-territory mapping and structure transfers.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `market_transfer.aspx`

## 5. Route
`~/TransferUI/SubTerritory_Transfer.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/TransferUI/SubTerritory_Transfer.aspx |
| Code-behind | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| User control | ~/TransferUI/IVMarketStructureFrom_ToSubTeriiTransfer.ascx |
| User control | ~/TransferUI/IVMarketStructureFrom_ToSubTeriiTransfer.ascx |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | MarketStructureTransferDAL (Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs) |
| DAO/entity class | MarketStructureTranferDAO (Library.DAO/Transer_DAO/MarketStructureTranferDAO.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| IVMarketStructure | uc1:IVMarketStructure |  | no validator found |  |  |  |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| IVMarketStructureTo | uc1:IVMarketStructureTo |  | no validator found |  |  |  |  |
| chkMarket | CheckBox |  Select All / Unselect All | no validator found |  |  |  | OnCheckedChanged→chkMarket_CheckedChanged |
| chkListMarket | CheckBoxList |  | no validator found |  |  |  |  |
| btnSave | LinkButton |  | no validator found |  |  |  | OnClick→btnSave_Click, OnClientClick→return sweetAlertConfirm_Submit(this); |
| btnReset | LinkButton |  | no validator found |  |  |  | OnClick→btnReset_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| chkListMarket | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| btnSave | LinkButton click | return sweetAlertConfirm_Submit(this); | TransferUI_SubTerritory_Transfer.btnSave_Click | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:111 | — | MarketStructureTransferDAL.UpdateSubTerritory_Transfer | sp_Update_MarketStructure_Transfer | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |
| btnReset | LinkButton click | (postback) | TransferUI_SubTerritory_Transfer.btnReset_Click | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:275 | — | — | — | read-only / none detected |

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
| TransferUI_SubTerritory_Transfer.Page_Load | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:24 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.Page_Load | no | no |
| TransferUI_SubTerritory_Transfer.gv_DocumentUpload_PreRender | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:46 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.gv_DocumentUpload_PreRender | no | no |
| TransferUI_SubTerritory_Transfer.rbType_SelectedIndexChanged | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:58 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.rbType_SelectedIndexChanged | no | no |
| TransferUI_SubTerritory_Transfer.btnSave_Click | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:111 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.btnSave_Click → MarketStructureTransferDAL.UpdateSubTerritory_Transfer → SP: sp_Update_MarketStructure_Transfer | no | yes |
| TransferUI_SubTerritory_Transfer.btnSearch_Click | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:196 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.btnSearch_Click → CommonDataLoad.GetSubTerritory_ByTerritoryId_Active → SP: sp_CS_GetSubTerritory_ByTerritoryId_Active | yes | no |
| TransferUI_SubTerritory_Transfer.chkSelect_CheckedChanged | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:225 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.chkSelect_CheckedChanged | yes | no |
| TransferUI_SubTerritory_Transfer.btnReset_Click | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:275 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.btnReset_Click | no | no |
| TransferUI_SubTerritory_Transfer.chkMarket_CheckedChanged | Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs:280 | object sender, EventArgs e | TransferUI_SubTerritory_Transfer.chkMarket_CheckedChanged | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| MarketStructureTransferDAL.UpdateSubTerritory_Transfer | Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:305 | sp_Update_MarketStructure_Transfer | @EntryBy, @FAreaId, @FGroupId, @FRegionId, @FSubTerritoryId, @FTerritoryId, @TAreaId, @TGroupId, @TM |
| CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1625 | sp_CS_GetSubTerritory_ByTerritoryId_Active | @id |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_GetSubTerritory_ByTerritoryId_Active | CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | body NOT in repository | read-like name (INFERRED) |
| sp_Update_MarketStructure_Transfer | MarketStructureTransferDAL.UpdateSubTerritory_Transfer | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select at least one Territory !!! | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-select form-select-sm mb-3 mySelect2 is-invalid | UNKNOWN |
| (handler Validation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |
| (handler btnSave_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','SubTerritory_Transfer.aspx'); | UNKNOWN |
| (handler btnSearch_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Sub-Territory! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 3 of 11 methods.
- Redirect targets: Market_Transfer.aspx
- User messages: ','Success','SubTerritory_Transfer.aspx');; Please Select Sub-Territory!; Please Select at least one Territory !!!; form-select form-select-sm mb-3 mySelect2; form-select form-select-sm mb-3 mySelect2 is-invalid; please fill out this field
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, MarketStructureTransferDAL; DAO: MarketStructureTranferDAO, ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click btnSave with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click btnSave with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click btnReset with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click btnReset with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/TransferUI/SubTerritory_Transfer.aspx`, `Solution.Web/TransferUI/SubTerritory_Transfer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/TransferUI/SubTerritory_Transfer.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 12 controls parsed | n/a | PARTIAL | Solution.Web/TransferUI/SubTerritory_Transfer.aspx |
| Data access | This document §15–17 | 2 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 2 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M03 (Organisation & Market Structure Master).
- Other pages sharing this page's stored procedures: PAGE-095, PAGE-096, PAGE-101, PAGE-102, PAGE-557, PAGE-587, PAGE-677
- Changing DAL methods CommonDataLoad.GetSubTerritory_ByTerritoryId_Active, MarketStructureTransferDAL.UpdateSubTerritory_Transfer affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
