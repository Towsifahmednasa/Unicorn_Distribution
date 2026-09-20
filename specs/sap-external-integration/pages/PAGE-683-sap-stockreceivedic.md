---
page_id: PAGE-683
module: M17
page: SAP_StockReceiveDIC
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-683 — SAP_StockReceiveDIC

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-683 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | SAP_StockReceiveDIC |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SAP_Integration |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SAP_Integration_SAP_StockReceiveDIC |
| aspx size | 9583 bytes |
| Code-behind LOC | 210 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **SAP_StockReceiveDIC** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: SAP hand-off pages, MIGO, provider dropout integration, API-customer import, mobile-app data-load web methods.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SAP_Integration/SAP_IntrigationPointDIC.aspx`

Outbound references: `sap_intrigationpoint.aspx`, `sap_intrigationpointdic.aspx`

## 5. Route
`~/SAP_Integration/SAP_StockReceiveDIC.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx |
| Code-behind | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | SAP_IntrigationPointDAL (Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| hdfStockMovementMasterId | HiddenField |  | no validator found |  |  |  |  |
| lblChallanNo | Label |  | no validator found |  |  |  |  |
| lblChallanDate | Label |  | no validator found |  |  |  |  |
| lblStatus | Label |  | no validator found |  |  |  |  |
| lblFrom | Label |  | no validator found |  |  |  |  |
| lblTo | Label |  | no validator found |  |  |  |  |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | TemplateField | #SL | no validator found |  |  |  |  |
| LabelSL | Label | <%# Container.DataItemIndex + 1 %> | no validator found |  |  |  |  |
| hfStockMovementDetailId | HiddenField |  | no validator found |  |  |  |  |
| (no id) | BoundField | Product Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No. | no validator found |  |  |  |  |
| (no id) | BoundField | Unit Price | no validator found |  |  |  |  |
| (no id) | TemplateField | Quantity | no validator found |  |  |  |  |
| txtquantity | TextBox | <%#Eval("quantity") %> | no validator found |  | ReadOnly=true |  |  |
| (no id) | BoundField | Current Stock | no validator found |  |  |  |  |
| ApproveButton | LinkButton |  | no validator found |  |  |  | OnClick→ApproveButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| hdfStockMovementMasterId | dtTable | SAP_Integration_SAP_StockReceiveDIC.LoadSapDataById | — | SAP_IntrigationPointDAL.GetSAPStockDataById | UNKNOWN | UNKNOWN | UNKNOWN |
| hfStockMovementDetailId | StockMovementDetailId | SAP_Integration_SAP_StockReceiveDIC.ApproveButton_Click | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | UNKNOWN | UNKNOWN | UNKNOWN |
| txtquantity | quantity | SAP_Integration_SAP_StockReceiveDIC.ApproveButton_Click | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | UNKNOWN | UNKNOWN | UNKNOWN |
| lblChallanNo | Res | SAP_Integration_SAP_StockReceiveDIC.ApproveButton_Click | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | UNKNOWN | UNKNOWN | UNKNOWN |
| txtquantity | quantity | SAP_Integration_SAP_StockReceiveDIC.DataValidation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
|  | LinkButton click | (postback) | SAP_Integration_SAP_StockReceiveDIC.BackToListButton_Click | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs:207 | — | — | — | read-only / none detected |
| ApproveButton | LinkButton click | (postback) | SAP_Integration_SAP_StockReceiveDIC.ApproveButton_Click | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs:120 | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | sp_SAP_StockReceive, sp_SAP_Up_StockReceiveQty | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- Client script functions in page: none
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SAP_Integration_SAP_StockReceiveDIC.Page_Load | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs:16 | object sender, EventArgs e | SAP_Integration_SAP_StockReceiveDIC.Page_Load → SAP_IntrigationPointDAL.GetSAPStockDataById → SP: sp_Get_SAP_StockReceivePendingDataById | yes | no |
| SAP_Integration_SAP_StockReceiveDIC.LoadSapDataById | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs:38 |  | SAP_Integration_SAP_StockReceiveDIC.LoadSapDataById → SAP_IntrigationPointDAL.GetSAPStockDataById → SP: sp_Get_SAP_StockReceivePendingDataById | yes | no |
| SAP_Integration_SAP_StockReceiveDIC.ApproveButton_Click | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs:120 | object sender, EventArgs e | SAP_Integration_SAP_StockReceiveDIC.ApproveButton_Click → SAP_IntrigationPointDAL.SaveStockReceive / SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive → SP: sp_SAP_StockReceive, sp_SAP_Up_StockReceiveQty | no | yes |
| SAP_Integration_SAP_StockReceiveDIC.BackToListButton_Click | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs:207 | object sender, EventArgs e | SAP_Integration_SAP_StockReceiveDIC.BackToListButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SAP_IntrigationPointDAL.GetSAPStockDataById | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:375 | sp_Get_SAP_StockReceivePendingDataById | @StockMovementMasterId |
| SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:343 | sp_SAP_Up_StockReceiveQty | @StockMovementDetailId, @quantity |
| SAP_IntrigationPointDAL.SaveStockReceive | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:308 | sp_SAP_StockReceive | @ApproveBy, @ChallanNo |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_SAP_StockReceivePendingDataById | SAP_IntrigationPointDAL.GetSAPStockDataById | body NOT in repository | read-like name (INFERRED) |
| sp_SAP_StockReceive | SAP_IntrigationPointDAL.SaveStockReceive | body NOT in repository | read-like name (INFERRED) |
| sp_SAP_Up_StockReceiveQty | SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): none detected
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler ApproveButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','SAP_IntrigationPointDIC.aspx'); | UNKNOWN |
| (handler ApproveButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler DataValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler DataValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: StockMovementMasterId, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 2 of 6 methods.
- Redirect targets: SAP_IntrigationPoint.aspx
- User messages: ','Success','SAP_IntrigationPointDIC.aspx');; Already Exist!; form-control form-control-sm is-invalid; please fill out this field
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
SAP_IntrigationPointDAL, sp_Get_SAP_StockReceivePendingDataById, sp_SAP_StockReceive, sp_SAP_Up_StockReceiveQty

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: SAP_IntrigationPointDAL; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click  with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click  with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click ApproveButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click ApproveButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx`, `Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/SAP_Integration/SAP_StockReceiveDIC.aspx |
| Data access | This document §15–17 | 3 SP names, 0 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 3 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M17 (SAP, Mobile-App & External Integration).
- Other pages sharing this page's stored procedures: PAGE-680, PAGE-681, PAGE-682
- Changing DAL methods SAP_IntrigationPointDAL.GetSAPStockDataById, SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
