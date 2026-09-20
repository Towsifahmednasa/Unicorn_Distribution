---
page_id: PAGE-191
module: M05
page: CustomerCreditView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-191 — CustomerCreditView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-191 |
| Module | M05 — Customer & Provider Master (INFERRED from folder + file name) |
| Page name | CustomerCreditView |
| Kind | Edit form (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_CustomerCreditView |
| aspx size | 13497 bytes |
| Code-behind LOC | 118 |
| Inline script size | 1708 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **CustomerCreditView** (Edit form). Page title/heading text: `Customer Credit List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Customer/provider entry, categories/types, credit limits, tagging, Excel uploads/imports, approvals of customer changes.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/CustomerCreditLimitEntry.aspx`

Outbound references: `customercreditlimitentry.aspx`

## 5. Route
`~/SInventory_UI/CustomerCreditView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/CustomerCreditView.aspx |
| Code-behind | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CreditLimitDAL (Library.DAL/SInventory_DAL/CreditLimitDAL.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| newEntryLinkButton | LinkButton |  | no validator found |  |  |  | OnClick→newEntryLinkButton_Click |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| companyDropDownList | DropDownList |  | no validator found |  |  | see §9 |  |
| customerTextBox | TextBox |  | no validator found |  |  |  | OnTextChanged→customerTextBox_TextChanged |
| customerTextBox_AutoCompleteExtender | AutoCompleteExtender |  | no validator found |  | Enabled=True |  |  |
| customerIdHiddenField | HiddenField |  | no validator found |  |  |  |  |
| searchButton | LinkButton |  | no validator found |  |  |  | OnClick→searchButton_Click |
| resetButton | LinkButton |  | no validator found |  |  |  | OnClick→resetButton_Click |
| loadGridView | GridView |  | no validator found |  |  |  | OnRowCommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Company | no validator found |  |  |  |  |
| (no id) | BoundField | Customer ID | no validator found |  |  |  |  |
| (no id) | BoundField | Customer Name | no validator found |  |  |  |  |
| (no id) | BoundField | Limit Amount | no validator found |  |  |  |  |
| (no id) | BoundField | Validity (Day) | no validator found |  |  |  |  |
| (no id) | BoundField | Status | no validator found |  |  |  |  |
| (no id) | BoundField | Overdue Since (Days) | no validator found |  |  |  |  |
| (no id) | BoundField | Oldest Due Invoice No | no validator found |  |  |  |  |
| (no id) | BoundField | Oldest Due Invoice Date | no validator found |  |  |  |  |
| (no id) | BoundField | Active Status | no validator found |  |  |  |  |
| (no id) | BoundField | Approved By | no validator found |  |  |  |  |
| (no id) | BoundField | Approved Date | no validator found |  |  |  |  |
| (no id) | TemplateField | Edit | no validator found |  |  |  |  |
| editButton | LinkButton |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| deleteButton | LinkButton | <%# Eval("ActiveStatus").ToString() == "Active" ? "Deactivate" : "Already Inactive" %> | no validator found |  | Enabled=<%# Eval("ActiveStatus").ToString() == "Active" %> |  | OnClientClick→return sweetAlertConfirm_Delete(this); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| customerTextBox | parts | SInventory_UI_CustomerCreditView.customerTextBox_TextChanged | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| companyDropDownList | LoadDropdowns → CreditLimitDAL.GetCompanyDropdownList | inline SQL | CompanyName/CompanyId | tblCompanyInfo |

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| newEntryLinkButton | LinkButton click | (postback) | SInventory_UI_CustomerCreditView.newEntryLinkButton_Click | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:55 | — | — | — | read-only / none detected |
| searchButton | LinkButton click | (postback) | SInventory_UI_CustomerCreditView.searchButton_Click | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:45 | — | CreditLimitDAL.GetCustomerCreditInfo | — | read-only / none detected |
| resetButton | LinkButton click | (postback) | SInventory_UI_CustomerCreditView.resetButton_Click | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:50 | — | — | — | read-only / none detected |
| deleteButton | LinkButton | return sweetAlertConfirm_Delete(this); | — | — | — | — | — | UNKNOWN (handler not resolved) |

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
| SInventory_UI_CustomerCreditView.Page_Load | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:13 | object sender, EventArgs e | SInventory_UI_CustomerCreditView.Page_Load → CreditLimitDAL.GetCompanyDropdownList / CreditLimitDAL.GetCustomerCreditInfo | no | no |
| SInventory_UI_CustomerCreditView.LoadDropdowns | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:22 |  | SInventory_UI_CustomerCreditView.LoadDropdowns → CreditLimitDAL.GetCompanyDropdownList | no | no |
| SInventory_UI_CustomerCreditView.LoadGrid | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:27 |  | SInventory_UI_CustomerCreditView.LoadGrid → CreditLimitDAL.GetCustomerCreditInfo | no | no |
| SInventory_UI_CustomerCreditView.searchButton_Click | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:45 | object sender, EventArgs e | SInventory_UI_CustomerCreditView.searchButton_Click → CreditLimitDAL.GetCustomerCreditInfo | no | no |
| SInventory_UI_CustomerCreditView.resetButton_Click | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:50 | object sender, EventArgs e | SInventory_UI_CustomerCreditView.resetButton_Click | no | no |
| SInventory_UI_CustomerCreditView.newEntryLinkButton_Click | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:55 | object sender, EventArgs e | SInventory_UI_CustomerCreditView.newEntryLinkButton_Click | no | no |
| SInventory_UI_CustomerCreditView.customerTextBox_TextChanged | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:60 | object sender, EventArgs e | SInventory_UI_CustomerCreditView.customerTextBox_TextChanged | no | no |
| SInventory_UI_CustomerCreditView.loadGridView_RowCommand | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:77 | object sender, GridViewCommandEventArgs e | SInventory_UI_CustomerCreditView.loadGridView_RowCommand → CreditLimitDAL.DeleteCreditLimit / CreditLimitDAL.GetCustomerCreditInfo | no | no |
| SInventory_UI_CustomerCreditView.gv_DocumentUpload_PreRender | Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs:109 | object sender, EventArgs e | SInventory_UI_CustomerCreditView.gv_DocumentUpload_PreRender | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CreditLimitDAL.GetCompanyDropdownList | Library.DAL/SInventory_DAL/CreditLimitDAL.cs:64 | inline SQL: tblCompanyInfo |  |
| CreditLimitDAL.GetCustomerCreditInfo | Library.DAL/SInventory_DAL/CreditLimitDAL.cs:81 | inline SQL: tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustomerCreditLimit |  |
| CreditLimitDAL.DeleteCreditLimit | Library.DAL/SInventory_DAL/CreditLimitDAL.cs:58 | inline SQL: tblCustomerCreditLimit |  |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustomerCreditLimit, tblInvoice
- Tables WRITTEN (inline SQL reachable): tblCustomerCreditLimit
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ShowSuccesalert('Operation successful!','Success'); | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 10 methods.
- Redirect targets: CustomerCreditLimitEntry.aspx, CustomerCreditLimitEntry.aspx?ID=, CustomerCreditView.aspx
- User messages: ShowSuccesalert('Operation successful!','Success');
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CreditLimitDAL; DAO: —; Tables (inline): tblCompanyInfo, tblCompanyUnit, tblCustMaster, tblCustomerCreditLimit, tblInvoice

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |
| TC-03 | Click newEntryLinkButton with required fields empty | Validator/server message shown; no DB write |
| TC-04 | Click newEntryLinkButton with valid data | Handler runs; rows written to tblCustomerCreditLimit |
| TC-05 | Click searchButton with required fields empty | Validator/server message shown; no DB write |
| TC-06 | Click searchButton with valid data | Handler runs; rows written to tblCustomerCreditLimit |
| TC-07 | Click resetButton with required fields empty | Validator/server message shown; no DB write |
| TC-08 | Click resetButton with valid data | Handler runs; rows written to tblCustomerCreditLimit |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/CustomerCreditView.aspx`, `Solution.Web/SInventory_UI/CustomerCreditView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/CustomerCreditView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 29 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/CustomerCreditView.aspx |
| Data access | This document §15–17 | 0 SP names, 5 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M05 (Customer & Provider Master).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods CreditLimitDAL.DeleteCreditLimit, CreditLimitDAL.GetCompanyDropdownList, CreditLimitDAL.GetCustomerCreditInfo affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
