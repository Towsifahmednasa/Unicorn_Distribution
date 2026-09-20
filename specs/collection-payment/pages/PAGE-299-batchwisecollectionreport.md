---
page_id: PAGE-299
module: M09
page: BatchWiseCollectionReport
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-299 — BatchWiseCollectionReport

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-299 |
| Module | M09 — Collections, Payments & Money Receipts (INFERRED from folder + file name) |
| Page name | BatchWiseCollectionReport |
| Kind | Report / Dashboard (INFERRED from name) |
| Web folder | Money_Receipt_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | Money_Receipt_UI_BatchWiseCollectionReport |
| aspx size | 24496 bytes |
| Code-behind LOC | 243 |
| Inline script size | 816 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **BatchWiseCollectionReport** (Report / Dashboard). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer payments, money receipts, deposit slips, payment reversal/partial, customer ledger, aging and receivables.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `batchwisecollectionreportviewer.aspx`

## 5. Route
`~/Money_Receipt_UI/BatchWiseCollectionReport.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx |
| Code-behind | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | BatchWiseCollectionReportDal (Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton |  | no validator found |  |  |  | OnClick→LinkButton1_Click |
| delDropDownList | DropDownList |  | no validator found |  |  | see §9 | OnSelectedIndexChanged→delDropDownList_OnTextChanged |
| dateTextBox | TextBox |  | no validator found |  |  |  |  |
| batchDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| singleCustomerTextBox | TextBox |  | no validator found |  | Enabled=False |  | OnTextChanged→singleCustomerTextBox_OnTextChanged |
| customerTextBox_AutoCompleteExtender | AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| hdCustomerId | HiddenField |  | no validator found |  |  |  |  |
| Button1 | LinkButton |  | no validator found |  |  |  | onclick→searchButton_Click |
| clearButton | LinkButton |  | no validator found |  |  |  | onclick→clearButton_OnClick |
| totalReceiveableLabel | Label |  | no validator found |  |  |  |  |
| imgDate | ImageButton |  | no validator found |  |  |  |  |
| Date | CalendarExtender |  | no validator found |  |  |  |  |
| paymentAmountTextBox | Label |  | no validator found |  | ReadOnly=True |  |  |
| customerCheckBox | CheckBox |  | no validator found |  |  |  | OnCheckedChanged→customerCheckBox_OnCheckedChanged |
| nameTextBox | Label |  | no validator found |  | ReadOnly=True |  |  |
| submitButton | Button | View Report | no validator found |  |  |  | OnClick→submitButton_OnClick |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Batch Creation Date | no validator found |  |  |  |  |
| (no id) | BoundField | Batch | no validator found |  |  |  |  |
| (no id) | BoundField | Payment Date | no validator found |  |  |  |  |
| (no id) | BoundField | Paid Amount | no validator found |  |  |  |  |
| (no id) | TemplateField | Action | no validator found |  |  |  |  |
| invoiceButton | LinkButton | Report &gt; &gt; &gt; | no validator found |  |  |  | OnClick→invoiceButton_Click |
| UpdateProgress1 | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| singleCustomerTextBox | data | Money_Receipt_UI_BatchWiseCollectionReport.singleCustomerTextBox_OnTextChanged | — | BatchWiseCollectionReportDal.GetCustomerInfoByCode | UNKNOWN | UNKNOWN | UNKNOWN |
| singleCustomerTextBox | aTable | Money_Receipt_UI_BatchWiseCollectionReport.singleCustomerTextBox_OnTextChanged | — | BatchWiseCollectionReportDal.GetCustomerInfoByCode | UNKNOWN | UNKNOWN | UNKNOWN |
| batchDropDownList | aTable | Money_Receipt_UI_BatchWiseCollectionReport.searchButton_Click | — | BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionSingle | UNKNOWN | UNKNOWN | UNKNOWN |
| singleCustomerTextBox | aTable | Money_Receipt_UI_BatchWiseCollectionReport.searchButton_Click | — | BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionSingle | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| delDropDownList | DropDownList → BatchWiseCollectionReportDal.LoadDeliveryMan | inline SQL | DeliveryManName/DeliveryManId | tblDeliveryManInfo, tblEmpGeneralInfo |
| batchDropDownList | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| LinkButton1 | LinkButton | — | LinkButton1_Click | — | — | — | — | UNKNOWN (handler not resolved) |
| submitButton | Button click | (postback) | Money_Receipt_UI_BatchWiseCollectionReport.submitButton_OnClick | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:85 | — | — | — | read-only / none detected |
| invoiceButton | LinkButton click | (postback) | Money_Receipt_UI_BatchWiseCollectionReport.invoiceButton_Click | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:228 | — | — | — | read-only / none detected |

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
| Money_Receipt_UI_BatchWiseCollectionReport.Page_Load | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:18 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.Page_Load → BatchWiseCollectionReportDal.LoadDeliveryMan | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.DropDownList | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:26 |  | Money_Receipt_UI_BatchWiseCollectionReport.DropDownList → BatchWiseCollectionReportDal.LoadDeliveryMan | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.delDropDownList_OnTextChanged | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:38 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.delDropDownList_OnTextChanged | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.submitButton_OnClick | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:85 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.submitButton_OnClick | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.dateTextBox_TextChanged | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:108 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.dateTextBox_TextChanged | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.clearButton_OnClick | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:113 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.clearButton_OnClick | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.singleCustomerTextBox_OnTextChanged | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:127 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.singleCustomerTextBox_OnTextChanged → BatchWiseCollectionReportDal.GetCustomerInfoByCode | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.customerCheckBox_OnCheckedChanged | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:164 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.customerCheckBox_OnCheckedChanged | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.searchButton_Click | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:183 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.searchButton_Click → BatchWiseCollectionReportDal.BatchWiseCollection / BatchWiseCollectionReportDal.BatchWiseCollectionSingle | no | no |
| Money_Receipt_UI_BatchWiseCollectionReport.invoiceButton_Click | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs:228 | object sender, EventArgs e | Money_Receipt_UI_BatchWiseCollectionReport.invoiceButton_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| BatchWiseCollectionReportDal.LoadDeliveryMan | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:17 | inline SQL: tblDeliveryManInfo, tblEmpGeneralInfo |  |
| BatchWiseCollectionReportDal.GetCustomerInfoByCode | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:24 | inline SQL: tblCustMaster, tblInvoice | @CustomerCode |
| BatchWiseCollectionReportDal.BatchWiseCollection | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:36 | inline SQL: tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice | @BatchId |
| BatchWiseCollectionReportDal.BatchWiseCollectionSingle | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:124 | inline SQL: tblCustMaster, tblCustPayDetail, tblCustomerPay, tblInvoice | @CustomerCode |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCustMaster, tblCustPayDetail, tblCustomerPay, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceBatch
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler FindBatchInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Batch Creation Date Is Required !!!! | UNKNOWN |
| (handler FindBatchInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select Delivery Man !!!! | UNKNOWN |
| (handler submitButton_OnClick) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | BatchNo is required !!!! | UNKNOWN |
| (handler singleCustomerTextBox_OnTextChanged) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Invalid Customer Info !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserType, paydetailId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 14 methods.
- Redirect targets: none
- User messages: Batch Creation Date Is Required !!!!; BatchNo is required !!!!; Invalid Customer Info !!; Please Select Delivery Man !!!!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: BatchWiseCollectionReportDal; DAO: —; Tables (inline): tblCustMaster, tblCustPayDetail, tblCustomerPay, tblDeliveryManInfo, tblEmpGeneralInfo, tblInvoice, tblInvoiceBatch

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click LinkButton1 with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click LinkButton1 with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-05 | Click submitButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click submitButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |
| TC-07 | Click invoiceButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click invoiceButton with valid data | Handler runs; rows written to tables written by the SPs in §16 (UNKNOWN) |

## 28. Reverse Engineering References
- `Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx`, `Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 32 controls parsed | n/a | PARTIAL | Solution.Web/Money_Receipt_UI/BatchWiseCollectionReport.aspx |
| Data access | This document §15–17 | 0 SP names, 7 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M09 (Collections, Payments & Money Receipts).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionSingle, BatchWiseCollectionReportDal.GetCustomerInfoByCode, BatchWiseCollectionReportDal.LoadDeliveryMan affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
