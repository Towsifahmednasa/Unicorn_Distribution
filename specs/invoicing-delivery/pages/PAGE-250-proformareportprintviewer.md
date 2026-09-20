---
page_id: PAGE-250
module: M07
page: ProformaReportPrintViewer
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-250 — ProformaReportPrintViewer

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-250 |
| Module | M07 — Proforma, Invoicing & Delivery (INFERRED from folder + file name) |
| Page name | ProformaReportPrintViewer |
| Kind | Report viewer (INFERRED from name) |
| Web folder | SInventory_RPTVIEW |
| Master page | none (stand-alone HTML page) |
| Page class | SInventory_RPTVIEW_InvoiceReportViewer |
| aspx size | 4518 bytes |
| Code-behind LOC | 243 |
| Inline script size | 625 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **ProformaReportPrintViewer** (Report viewer). Page title/heading text: `Edit`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Proforma and invoice creation (manual/auto/by-order), challans, top sheets, picking, loading, delivery assistants, invoice printing.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/DoctorInvoiceCreationByOrder.aspx`, `SInventory_UI/InvoiceCreationByOrder.aspx`, `SInventory_UI/InvoiceCreationForCustomerByOrder.aspx`, `SInventory_UI/InvoiceSummaryReport.aspx`, `SInventory_UI/ProformaPrintList.aspx`, `SInventory_UI/TopSheetGenerate.aspx`

Outbound references: none found

## 5. Route
`~/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx |
| Code-behind | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx.cs |
| CSS include | ../css/custom.css |
| CSS include | ../css/style.css |
| CSS include | ../css/colors/blue.css |
| BLL class | InvoiceBLL (Library.BLL/SInventory_BLL/InvoiceBLL.cs) |
| BLL class | StockTransportOrderReportBLL (Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs) |
| DAL class | InvoiceDAL (Library.DAL/SInventory_DAL/InvoiceDAL.cs) |
| DAL class | StockTransportOrderReportDAL (Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs) |
| Report (.rpt) | rptInvoiceForCustomer.rpt |
| Report (.rpt) | rptInvoiceForCustomer_Uncr.rpt |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| btnPrint | input | Print | no validator found |  |  |  | onclick→Print() |
| Button1 | Button | Close | no validator found |  |  |  | onclick→closeButton_Click |
| crvInvoiceReport | CR:CrystalReportViewer |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
No control→property assignments were extracted (page may only list/report, or bind controls indirectly). Table: `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
No buttons with server handlers found.

## 11. Frontend Flow
- Client script functions in page: Print
- jQuery/DOM bindings: none detected
- AJAX endpoints (`url:`): none
- `PageMethods` calls: none
- Modal dialogs: no; Export/print hints: yes; File upload: no; Grid/table markup: yes
- Update model: ASP.NET WebForms postback (UpdatePanel/ScriptManager where present); request payloads are form posts (ViewState) unless an AJAX endpoint is listed.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SInventory_RPTVIEW_InvoiceReportViewer.Page_Init | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx.cs:24 | object sender, EventArgs e | SInventory_RPTVIEW_InvoiceReportViewer.Page_Init → InvoiceBLL.InvoiceDetailDataForReportBLL2 / InvoiceBLL.InvoiceDetailDataInvoID / InvoiceBLL.InvoiceMainDataForReportBLL2 / InvoiceBLL.InvoiceMainDatainvNo / StockTransportOrderReportBLL.CompanyInfoBLL → Invoic | yes | no |
| SInventory_RPTVIEW_InvoiceReportViewer.closeButton_Click | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx.cs:203 | object sender, EventArgs e | SInventory_RPTVIEW_InvoiceReportViewer.closeButton_Click | no | no |
| SInventory_RPTVIEW_InvoiceReportViewer.crvInvoiceReport_Disposed | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx.cs:225 | object sender, EventArgs e | SInventory_RPTVIEW_InvoiceReportViewer.crvInvoiceReport_Disposed | no | no |
| SInventory_RPTVIEW_InvoiceReportViewer.crvInvoiceReport_Unload | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx.cs:235 | object sender, EventArgs e | SInventory_RPTVIEW_InvoiceReportViewer.crvInvoiceReport_Unload | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| InvoiceBLL.InvoiceMainDataForReportBLL2 | Library.BLL/SInventory_BLL/InvoiceBLL.cs:897 |
| InvoiceBLL.InvoiceDetailDataForReportBLL2 | Library.BLL/SInventory_BLL/InvoiceBLL.cs:920 |
| StockTransportOrderReportBLL.CompanyInfoBLL | Library.BLL/SInventory_BLL/StockTransportOrderReportBLL.cs:23 |
| InvoiceBLL.InvoiceMainDatainvNo | Library.BLL/SInventory_BLL/InvoiceBLL.cs:903 |
| InvoiceBLL.InvoiceDetailDataInvoID | Library.BLL/SInventory_BLL/InvoiceBLL.cs:925 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| InvoiceDAL.InvoiceMainDataForReport2 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1426 | inline SQL: tblCompanyUnit, tblCustMaster, tblDAInfo, tblEmpGeneralInfo |  |
| InvoiceDAL.InvoiceDetailDataForReport2 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1618 | inline SQL: tblDCStore, tblInvoice, tblInvoiceDetail, tblProductDiscount |  |
| StockTransportOrderReportDAL.CompanyInformation | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs:15 | inline SQL: tblCompanyInfo |  |
| InvoiceDAL.GetNewReceiveableforInvoiceDAl | Library.DAL/SInventory_DAL/InvoiceDAL.cs:137 | sp_Get_NewReceiveableListforInvoice | @districtId, @fromDate, @toDate |
| InvoiceDAL.InvoiceMainDatainvNo | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1462 | inline SQL: CSTL_ACCDB_Unicorn, tblCompanyUnit, tblCustMaster, tblCustomerCreditLimit |  |
| InvoiceDAL.InvoiceDetailDataInvoID | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1640 | inline SQL: tblDCStore, tblInvoice, tblInvoiceDetail, tblProductDiscount |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_Get_NewReceiveableListforInvoice | InvoiceDAL.GetNewReceiveableforInvoiceDAl | body NOT in repository | read-like name (INFERRED) |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): CSTL_ACCDB_Unicorn, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustomerCreditLimit, tblDAInfo, tblDCStore, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblOrder, tblPaymentType, tblProductDiscount, tblUser
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
No validators or validation messages extracted.

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: paydetailId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 6 methods.
- Redirect targets: none
- User messages: none extracted
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
rptInvoiceForCustomer.rpt, rptInvoiceForCustomer_Uncr.rpt

## 26. Dependencies
BLL: InvoiceBLL, StockTransportOrderReportBLL; DAL: InvoiceDAL, StockTransportOrderReportDAL; DAO: —; Tables (inline): CSTL_ACCDB_Unicorn, tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustomerCreditLimit, tblDAInfo, tblDCStore, tblEmpGeneralInfo, tblInvoice, tblInvoiceDetail, tblOrder, tblPaymentType, tblProductDiscount, tblUser

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx`, `Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 6 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_RPTVIEW/ProformaReportPrintViewer.aspx |
| Data access | This document §15–17 | 1 SP names, 14 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 1 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).
- No client or server validation messages found for a page with action buttons.

## 31. Change Impact
- Module: M07 (Proforma, Invoicing & Delivery).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods InvoiceDAL.GetNewReceiveableforInvoiceDAl, InvoiceDAL.InvoiceDetailDataForReport2, InvoiceDAL.InvoiceDetailDataInvoID, InvoiceDAL.InvoiceMainDataForReport2, InvoiceDAL.InvoiceMainDatainvNo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
