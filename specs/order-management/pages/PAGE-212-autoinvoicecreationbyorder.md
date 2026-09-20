---
page_id: PAGE-212
module: M06
page: AutoInvoiceCreationByOrder
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-212 — AutoInvoiceCreationByOrder

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-212 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | AutoInvoiceCreationByOrder |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_AutoInvoiceCreationByOrder |
| aspx size | 10067 bytes |
| Code-behind LOC | 102 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **AutoInvoiceCreationByOrder** (Entry / Transaction). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `invoicecreationforcustomerbyorder.aspx`

## 5. Route
`~/SInventory_UI/AutoInvoiceCreationByOrder.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx |
| Code-behind | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | InvoiceBLL (Library.BLL/SInventory_BLL/InvoiceBLL.cs) |
| BLL class | OrderInfoBLL (Library.BLL/SInventory_BLL/OrderInfoBLL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | OrderInfoDAL (Library.DAL/SInventory_DAL/OrderInfoDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| salesCenterDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→salesCenterDropDownList_SelectedIndexChanged |
| manufacDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→manufacDropDownList_SelectedIndexChanged |
| marketDropDownList | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→marketDropDownList_SelectedIndexChanged |
| Button1 | Button | Search | no validator found |  |  |  | onclick→Button1_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| orderGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Order No | no validator found |  |  |  |  |
| (no id) | BoundField | Order Date | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Code | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Market | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Code | no validator found |  |  |  |  |
| (no id) | BoundField | Sales Center Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Code | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | TemplateField | Go To Invoice | no validator found |  |  |  |  |
| gotoinvoiceButton | Button | Go To Invoice | no validator found |  |  |  | onclick→gotoinvoiceButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| salesCenterDropDownList | DropDownlist → OrderInfoBLL.LoadSC | inline SQL | ComUnitName/ComUnitId | tblCompanyUnit, tblUserCompanyUnit |
| manufacDropDownList | DropDownlist → OrderInfoBLL.LoadManufac | inline SQL | ManufacName/ManufacId | tblManufacturer |
| marketDropDownList | salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadMarketOrderWise | inline SQL | MarketName/MarketId | tblCompanyUnit, tblMarket, tblOrder |

## 10. Button Action Mapping
No buttons with server handlers found.

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
| SInventory_UI_AutoInvoiceCreationByOrder.Page_Load | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:14 | object sender, EventArgs e | SInventory_UI_AutoInvoiceCreationByOrder.Page_Load → OrderInfoBLL.LoadManufac / OrderInfoBLL.LoadMarketOrderWise / OrderInfoBLL.LoadSC → OrderInfoDAL.LoadManufac / OrderInfoDAL.LoadMarketOrderWise / OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_AutoInvoiceCreationByOrder.DropDownlist | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:30 |  | SInventory_UI_AutoInvoiceCreationByOrder.DropDownlist → OrderInfoBLL.LoadManufac / OrderInfoBLL.LoadSC → OrderInfoDAL.LoadManufac / OrderInfoDAL.LoadSC | no | no |
| SInventory_UI_AutoInvoiceCreationByOrder.salesCenterDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:37 | object sender, EventArgs e | SInventory_UI_AutoInvoiceCreationByOrder.salesCenterDropDownList_SelectedIndexChanged → OrderInfoBLL.LoadMarketOrderWise → OrderInfoDAL.LoadMarketOrderWise | no | no |
| SInventory_UI_AutoInvoiceCreationByOrder.manufacDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:43 | object sender, EventArgs e | SInventory_UI_AutoInvoiceCreationByOrder.manufacDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_AutoInvoiceCreationByOrder.marketDropDownList_SelectedIndexChanged | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:47 | object sender, EventArgs e | SInventory_UI_AutoInvoiceCreationByOrder.marketDropDownList_SelectedIndexChanged | no | no |
| SInventory_UI_AutoInvoiceCreationByOrder.gotoinvoiceButton_Click | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:55 | object sender, EventArgs e | SInventory_UI_AutoInvoiceCreationByOrder.gotoinvoiceButton_Click → InvoiceBLL.SaveFullProformaInvoice → InvoiceDAL.SaveFullProformaInvoice → SP: sp_DeliveryConformationFull | no | no |
| SInventory_UI_AutoInvoiceCreationByOrder.Button1_Click | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs:99 | object sender, EventArgs e | SInventory_UI_AutoInvoiceCreationByOrder.Button1_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| OrderInfoBLL.LoadSC | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:91 |
| OrderInfoBLL.LoadManufac | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:117 |
| OrderInfoBLL.LoadMarketOrderWise | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:79 |
| InvoiceBLL.SaveFullProformaInvoice | Library.BLL/SInventory_BLL/InvoiceBLL.cs:71 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| OrderInfoDAL.LoadSC | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1935 | inline SQL: tblCompanyUnit |  |
| OrderInfoDAL.LoadManufac | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2219 | inline SQL: tblManufacturer |  |
| OrderInfoDAL.LoadMarketOrderWise | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:2287 | inline SQL: tblCompanyUnit, tblMarket, tblOrder | @ComUnitId |
| InvoiceDAL.SaveFullProformaInvoice | Library.DAL/SInventory_DAL/InvoiceDAL.cs:279 | sp_DeliveryConformationFull | @InvoiceNo, @UpdateBy, @UpdateDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_DeliveryConformationFull | InvoiceDAL.SaveFullProformaInvoice | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyUnit, tblManufacturer, tblMarket, tblOrder, tblUserCompanyUnit
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: LoginName, MarketId, UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 9 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: InvoiceBLL, OrderInfoBLL; DAL: InvoiceDAL, OrderInfoDAL; DAO: —; Tables (inline): tblCompanyUnit, tblManufacturer, tblMarket, tblOrder, tblUserCompanyUnit

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx`, `Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 21 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/AutoInvoiceCreationByOrder.aspx |
| Data access | This document §15–17 | 1 SP names, 5 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods InvoiceDAL.SaveFullProformaInvoice, OrderInfoDAL.LoadManufac, OrderInfoDAL.LoadMarketOrderWise, OrderInfoDAL.LoadSC affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
