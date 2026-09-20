---
page_id: PAGE-122
module: M04
page: GroupWisePromoQtyView
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "PARTIAL", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-122 — GroupWisePromoQtyView

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-122 |
| Module | M04 — Product, Pricing & Promotion Master (INFERRED from folder + file name) |
| Page name | GroupWisePromoQtyView |
| Kind | List / View (INFERRED from name) |
| Web folder | PromoAlloc |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | PromoAlloc_GroupWisePromoQtyView |
| aspx size | 15737 bytes |
| Code-behind LOC | 245 |
| Inline script size | 2952 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **GroupWisePromoQtyView** (List / View). Page title/heading text: ` Group Wise Product Allocation List`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Products and classification, units/pack sizes, manufacturers/suppliers, unit prices, discounts/bonus/campaigns, promo allocation groups.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `DWSP/AreaWiseTargetSetupView.aspx`, `DWSP/NationalTargetSetupView.aspx`, `DWSP/TerritoryTargetSetupView.aspx`, `DWSP/ZoneWiseTargetSetupView.aspx`, `MasterSetup_UI/TourSetupForEmployeeList.aspx`, `PromoAlloc/GroupWisePromoQtyEntry.aspx`

Outbound references: `customerentry.aspx`, `groupwisepromoqtyentry.aspx`

## 5. Route
`~/PromoAlloc/GroupWisePromoQtyView.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx |
| Code-behind | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | GroupWisePromoQtyDAL (Library.DAL/PromoAllocDAL/GroupWisePromoQtyDAL.cs) |
| DAL class | PromoMITagDAL (Library.DAL/PromoAllocDAL/PromoMITagDAL.cs) |
| DAO/entity class | ResultInfo (Library.DAO/SInventory_Entities/ResultInfo.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| (no id) | a |  | no validator found |  |  |  |  |
| progress | UpdateProgress |  | no validator found |  |  |  |  |
| imgWait | Image |  | no validator found |  |  |  |  |
| ddlmonth | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlYear | DropDownList |  | no validator found |  |  | see §9 |  |
| groupname | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlProduct | DropDownList |  | no validator found |  |  | see §9 |  |
| ddlMIO | DropDownList |  | no validator found |  |  | see §9 |  |
| btnSearch | LinkButton |  | no validator found |  |  |  | onclick→btnSearch_Click |
| resetBtn | LinkButton |  | no validator found |  |  |  | onclick→resetBtn_Click |
| loadGridView | GridView |  | no validator found |  |  |  | onrowcommand→loadGridView_RowCommand, OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Year | no validator found |  |  |  |  |
| (no id) | BoundField | Month | no validator found |  |  |  |  |
| (no id) | BoundField | Promo Group Name | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | MIO Name | no validator found |  |  |  |  |
| (no id) | BoundField | Territory Name | no validator found |  |  |  |  |
| (no id) | TemplateField | QTY | no validator found |  |  |  |  |
| txtQty | TextBox | <%#Eval("Qty") %> | no validator found |  | ReadOnly=true |  |  |
| FilteredTssextBoxExtender1 | FilteredTextBoxExtender |  | no validator found |  | Enabled=True |  |  |
| lbtUpDate | LinkButton |  | no validator found |  | Visible=false |  |  |
| (no id) | TemplateField | Edit | no validator found |  | Visible=false |  |  |
| LinkButton1 | LinkButton |  | no validator found |  |  |  |  |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| txtQty | CountNo | PromoAlloc_GroupWisePromoQtyView.loadGridView_RowCommand | — | GroupWisePromoQtyDAL.Update___Info | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlYear | param | PromoAlloc_GroupWisePromoQtyView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlmonth | param | PromoAlloc_GroupWisePromoQtyView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| groupname | param | PromoAlloc_GroupWisePromoQtyView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |
| ddlMIO | param | PromoAlloc_GroupWisePromoQtyView.Parm | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlmonth | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlYear | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| groupname | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlProduct | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |
| ddlMIO | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
No buttons with server handlers found.

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
| PromoAlloc_GroupWisePromoQtyView.Page_Load | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:24 | object sender, EventArgs e | PromoAlloc_GroupWisePromoQtyView.Page_Load → CommonDataLoad.GetMIOInfo_Rpt / GroupWisePromoQtyDAL.GetList / PromoMITagDAL.LoadGroup / PromoMITagDAL.LoadProduct → SP: sp_CS_MIOInfo_Rpt, sp_Get_GroupWisePromoQtyList | no | no |
| PromoAlloc_GroupWisePromoQtyView.LoadInitialInfo | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:34 |  | PromoAlloc_GroupWisePromoQtyView.LoadInitialInfo → CommonDataLoad.GetMIOInfo_Rpt / PromoMITagDAL.LoadGroup / PromoMITagDAL.LoadProduct → SP: sp_CS_MIOInfo_Rpt | yes | no |
| PromoAlloc_GroupWisePromoQtyView.gv_DocumentUpload_PreRender | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:134 | object sender, EventArgs e | PromoAlloc_GroupWisePromoQtyView.gv_DocumentUpload_PreRender | no | no |
| PromoAlloc_GroupWisePromoQtyView.EmpCetegoryAddImageButton_Click | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:145 | object sender, EventArgs e | PromoAlloc_GroupWisePromoQtyView.EmpCetegoryAddImageButton_Click | no | no |
| PromoAlloc_GroupWisePromoQtyView.LoadData | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:150 | string parm | PromoAlloc_GroupWisePromoQtyView.LoadData → GroupWisePromoQtyDAL.GetList → SP: sp_Get_GroupWisePromoQtyList | no | no |
| PromoAlloc_GroupWisePromoQtyView.loadGridView_RowCommand | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:166 | object sender, GridViewCommandEventArgs e | PromoAlloc_GroupWisePromoQtyView.loadGridView_RowCommand → GroupWisePromoQtyDAL.Update___Info → SP: sp_Update_PromoEmployeeQty | no | yes |
| PromoAlloc_GroupWisePromoQtyView.btnSearch_Click | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:208 | object sender, EventArgs e | PromoAlloc_GroupWisePromoQtyView.btnSearch_Click → GroupWisePromoQtyDAL.GetList → SP: sp_Get_GroupWisePromoQtyList | no | no |
| PromoAlloc_GroupWisePromoQtyView.resetBtn_Click | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs:242 | object sender, EventArgs e | PromoAlloc_GroupWisePromoQtyView.resetBtn_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
No BLL class is reached from this page (page calls DAL directly or has no data access).

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| CommonDataLoad.GetMIOInfo_Rpt | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:300 | sp_CS_MIOInfo_Rpt |  |
| PromoMITagDAL.LoadGroup | Library.DAL/PromoAllocDAL/PromoMITagDAL.cs:151 | inline SQL: tblPromoGroup |  |
| PromoMITagDAL.LoadProduct | Library.DAL/PromoAllocDAL/PromoMITagDAL.cs:157 | inline SQL: tblProduct |  |
| GroupWisePromoQtyDAL.GetList | Library.DAL/PromoAllocDAL/GroupWisePromoQtyDAL.cs:27 | sp_Get_GroupWisePromoQtyList | @Parm |
| GroupWisePromoQtyDAL.Update___Info | Library.DAL/PromoAllocDAL/GroupWisePromoQtyDAL.cs:49 | sp_Update_PromoEmployeeQty | @CountNo, @TourSetupEmployeeId, @UpdateBy, @UpdateDate |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Nature (from name) |
|---|---|---|---|
| sp_CS_MIOInfo_Rpt | CommonDataLoad.GetMIOInfo_Rpt | body NOT in repository | read-like name (INFERRED) |
| sp_Get_GroupWisePromoQtyList | GroupWisePromoQtyDAL.GetList | body NOT in repository | read-like name (INFERRED) |
| sp_Update_PromoEmployeeQty | GroupWisePromoQtyDAL.Update___Info | body NOT in repository | write-like name |

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblProduct, tblPromoGroup
- Tables WRITTEN (inline SQL reachable): none detected
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler LoadInitialInfo) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Please Select From List | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','GroupWisePromoQtyView.aspx'); | UNKNOWN |
| (handler loadGridView_RowCommand) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: UserId
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 1 of 11 methods.
- Redirect targets: CustomerEntry.aspx, GroupWisePromoQtyView.aspx
- User messages: ','Success','GroupWisePromoQtyView.aspx');; Already Exist!; Please Select From List
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: —; DAL: CommonDataLoad, GroupWisePromoQtyDAL, PromoMITagDAL; DAO: ResultInfo; Tables (inline): tblProduct, tblPromoGroup

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx`, `Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 26 controls parsed | n/a | PARTIAL | Solution.Web/PromoAlloc/GroupWisePromoQtyView.aspx |
| Data access | This document §15–17 | 3 SP names, 2 inline-SQL tables | SP bodies not in repo | UNKNOWN | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).
- 3 stored procedure(s) called; SP bodies are absent from the repository, so validations/transactions/columns inside them are UNKNOWN.
- Page code-behind calls a DAL class directly (bypasses BLL).

## 31. Change Impact
- Module: M04 (Product, Pricing & Promotion Master).
- Other pages sharing this page's stored procedures: PAGE-342
- Changing DAL methods CommonDataLoad.GetMIOInfo_Rpt, GroupWisePromoQtyDAL.GetList, GroupWisePromoQtyDAL.Update___Info, PromoMITagDAL.LoadGroup, PromoMITagDAL.LoadProduct affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
