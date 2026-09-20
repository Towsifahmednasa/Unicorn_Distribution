---
page_id: PAGE-219
module: M06
page: IssueRequisitionProducts
tier: B
generated: true   # mechanically extracted from source by static analysis; NOT semantically reviewed unless tier: A
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "PARTIAL", "api": "COMPLETE", "database": "PARTIAL", "rules": "UNKNOWN", "permission": "UNKNOWN", "status": "UNKNOWN", "tests": "MISSING"}
---
# PAGE-219 — IssueRequisitionProducts

> **Evidence level:** Tier B — facts below were extracted by static analysis of the `.aspx` markup and `.aspx.cs` code-behind and by resolving calls through `Library.BLL` / `Library.DAL`. Items marked `UNKNOWN` were not derivable from the repository. Stored-procedure bodies are NOT in the repository (except where §16 says otherwise), so column-level mapping is `UNKNOWN` for SP-based saves.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-219 |
| Module | M06 — Order Management (INFERRED from folder + file name) |
| Page name | IssueRequisitionProducts |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SInventory_UI_IssueRequisitionProducts |
| aspx size | 13369 bytes |
| Code-behind LOC | 263 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
INFERRED from name/markup: **IssueRequisitionProducts** (Page). Page title/heading text: `none`. Detailed purpose: UNKNOWN (Tier B).

## 3. Business Objective
UNKNOWN for Tier B. Module context: Manual/imported orders, requisitions, order approval, monitoring, tracking, DC change, deletion/edit.

## 4. Navigation
Menu placement is **DB-driven**: `NewMasterPage.master.cs` → `UserBLL.LoadMenu` → `UserDAL.LoadMenu` runs `SELECT dbo.MainMenu2(@UserId,@UserRoleID)` (a DB scalar function returning menu HTML; body not in the repository; older `MainMasterPage` uses `tblMainMenu`/`tblMenuDistribution` via `PanalClsDAL`). The menu rows/function are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: `viewrequisitionforissue.aspx`

## 5. Route
`~/SInventory_UI/IssueRequisitionProducts.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx |
| Code-behind | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | ProductBLL (Library.BLL/SInventory_BLL/ProductBLL.cs) |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| DAL class | ProductDAL (Library.DAL/SInventory_DAL/ProductDAL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAO/entity class | Requesition (Library.DAO/SInventory_Entities/Requesition.cs) |
| DAO/entity class | StockInTransfar (Library.DAO/SInventory_Entities/StockInTransfar.cs) |

## 7. UI Control Inventory
| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| reqNoTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| reqDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| truckNoTextBox | TextBox |  | no validator found |  |  |  |  |
| clnNoTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| clnDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| driverNameTextBox | TextBox |  | no validator found |  |  |  |  |
| pikNoTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| pikDateTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| issueGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Pro. Code | no validator found |  |  |  |  |
| (no id) | BoundField | Product Name | no validator found |  |  |  |  |
| (no id) | BoundField | Pack Size | no validator found |  |  |  |  |
| (no id) | BoundField | Batch No. | no validator found |  |  |  |  |
| (no id) | BoundField | Pick. Qty. | no validator found |  |  |  |  |
| (no id) | BoundField | Unit Price | no validator found |  |  |  |  |
| (no id) | TemplateField | Issue Qty. | no validator found |  |  |  |  |
| issueQtyTextBox | TextBox |  | no validator found |  | Enabled=False |  |  |
| (no id) | TemplateField | T. Price | no validator found |  |  |  |  |
| priceTextBox | TextBox |  | no validator found |  |  |  |  |
| (no id) | TemplateField | VAT | no validator found |  |  |  |  |
| vatTextBox | TextBox |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Tk.Value | no validator found |  |  |  |  |
| totalPriceTextBox | TextBox |  | no validator found |  |  |  |  |
| (no id) | TemplateField | Issue | no validator found |  |  |  |  |
| issueCheckBox | CheckBox |  | no validator found |  |  |  | oncheckedchanged→issueCheckBox_CheckedChanged |
| LinkButton1 | LinkButton |  | no validator found |  |  |  | onclick→LinkButton1_Click |
| totalAllPriceTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| vatAllPriceTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| grandTotalTextBox | TextBox |  | no validator found |  | ReadOnly=True |  |  |
| hdReqId | HiddenField |  | no validator found |  |  |  |  |
| bmitButton | Button | Submit | no validator found |  |  |  | onclick→bmitButton_Click, OnClientClick→return confirm('Are you sure you want Save ?'); |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| reqNoTextBox | Text | SInventory_UI_IssueRequisitionProducts.LoadMainInfo | RequisitionBLL.ChalanNo, RequisitionBLL.GetRequisitionInfoByReqId | RequisitionDAL.GetRequisitionInfoByReqIdDAL | UNKNOWN | UNKNOWN | UNKNOWN |
| issueQtyTextBox | issueQty | SInventory_UI_IssueRequisitionProducts.PriceCalculation | ProductBLL.ProductPriceDetailWithCaseBLL | ProductDAL.GetDataTableByText, ProductDAL.ProductPriceDetailWithCase, ProductDAL.ToDbValue | UNKNOWN | UNKNOWN | UNKNOWN |
| priceTextBox | Text | SInventory_UI_IssueRequisitionProducts.PriceCalculation | ProductBLL.ProductPriceDetailWithCaseBLL | ProductDAL.GetDataTableByText, ProductDAL.ProductPriceDetailWithCase, ProductDAL.ToDbValue | UNKNOWN | UNKNOWN | UNKNOWN |
| hdReqId | Requesition.ReqId | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| clnNoTextBox | Requesition.IssueChalanNo | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| clnDateTextBox | Requesition.IssuChalanDate | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| truckNoTextBox | Requesition.TruckNo | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| driverNameTextBox | Requesition.DriverName | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| clnDateTextBox | Requesition.SubmitDate | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| totalAllPriceTextBox | Requesition.TotalPrice | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| vatAllPriceTextBox | Requesition.TotalVAT | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| grandTotalTextBox | Requesition.GrandTotalPrice | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| issueQtyTextBox | StockInTransfar.Quantity | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| priceTextBox | StockInTransfar.PriceAmount | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| vatTextBox | StockInTransfar.VATAmount | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |
| totalPriceTextBox | StockInTransfar.TotalPriceAmount | SInventory_UI_IssueRequisitionProducts.SaveData | RequisitionBLL.ReqQtyUpdateInReqDetail, RequisitionBLL.UpdateIssueInformationOnRequisition, RequisitionBLL.UpdateStockTr | RequisitionDAL.ReqDetailUpdateAfterIssue, RequisitionDAL.RequisitionIssueParameters, RequisitionDAL.UpdateIssueInformati | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
| bmitButton | Button | return confirm('Are you sure you want Save ?'); | — | — | — | — | — | UNKNOWN (handler not resolved) |

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
| SInventory_UI_IssueRequisitionProducts.Page_Load | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:18 | object sender, EventArgs e | SInventory_UI_IssueRequisitionProducts.Page_Load → ProductBLL.ProductPriceDetailWithCaseBLL / RequisitionBLL.ChalanNo / RequisitionBLL.ChallanExistsBll / RequisitionBLL.GetRequisitionDetailByReqId / RequisitionBLL.GetRequisitionInfoByReqId → ProductDAL.Product | no | no |
| SInventory_UI_IssueRequisitionProducts.LoadMainInfo | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:39 | string reqId | SInventory_UI_IssueRequisitionProducts.LoadMainInfo → RequisitionBLL.ChalanNo / RequisitionBLL.GetRequisitionInfoByReqId → RequisitionDAL.GetRequisitionInfoByReqIdDAL | no | no |
| SInventory_UI_IssueRequisitionProducts.LoadGrid | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:59 | string reqId | SInventory_UI_IssueRequisitionProducts.LoadGrid → ProductBLL.ProductPriceDetailWithCaseBLL / RequisitionBLL.GetRequisitionDetailByReqId → ProductDAL.GetDataTableByText / ProductDAL.ProductPriceDetailWithCase / ProductDAL.ToDbValue / RequisitionDAL.GetRequisiti | no | no |
| SInventory_UI_IssueRequisitionProducts.issueCheckBox_CheckedChanged | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:87 | object sender, EventArgs e | SInventory_UI_IssueRequisitionProducts.issueCheckBox_CheckedChanged → ProductBLL.ProductPriceDetailWithCaseBLL → ProductDAL.GetDataTableByText / ProductDAL.ProductPriceDetailWithCase / ProductDAL.ToDbValue | no | no |
| SInventory_UI_IssueRequisitionProducts.issueQtyTextBox_TextChanged | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:105 | object sender, EventArgs e | SInventory_UI_IssueRequisitionProducts.issueQtyTextBox_TextChanged → ProductBLL.ProductPriceDetailWithCaseBLL → ProductDAL.GetDataTableByText / ProductDAL.ProductPriceDetailWithCase / ProductDAL.ToDbValue | no | no |
| SInventory_UI_IssueRequisitionProducts.PriceCalculation | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:110 | int rowIndex | SInventory_UI_IssueRequisitionProducts.PriceCalculation → ProductBLL.ProductPriceDetailWithCaseBLL → ProductDAL.GetDataTableByText / ProductDAL.ProductPriceDetailWithCase / ProductDAL.ToDbValue | no | no |
| SInventory_UI_IssueRequisitionProducts.SaveData | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:168 |  | SInventory_UI_IssueRequisitionProducts.SaveData → RequisitionBLL.ReqQtyUpdateInReqDetail / RequisitionBLL.UpdateIssueInformationOnRequisition / RequisitionBLL.UpdateStockTransfarInfoUpdate → RequisitionDAL.ReqDetailUpdateAfterIssue / RequisitionDAL.Requisition | no | no |
| SInventory_UI_IssueRequisitionProducts.bmitButton_Click | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:241 | object sender, EventArgs e | SInventory_UI_IssueRequisitionProducts.bmitButton_Click → RequisitionBLL.ReqQtyUpdateInReqDetail / RequisitionBLL.UpdateIssueInformationOnRequisition / RequisitionBLL.UpdateStockTransfarInfoUpdate → RequisitionDAL.ReqDetailUpdateAfterIssue / RequisitionDAL.Req | no | no |
| SInventory_UI_IssueRequisitionProducts.LinkButton1_Click | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs:260 | object sender, EventArgs e | SInventory_UI_IssueRequisitionProducts.LinkButton1_Click | no | no |

Controller layer = ASP.NET code-behind class; there is no separate MVC controller for this page (WebForms).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| RequisitionBLL.ChallanExistsBll | Library.BLL/SInventory_BLL/RequisitionBLL.cs:206 |
| RequisitionBLL.GetRequisitionDetailByReqId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:186 |
| ProductBLL.ProductPriceDetailWithCaseBLL | Library.BLL/SInventory_BLL/ProductBLL.cs:132 |
| RequisitionBLL.GetRequisitionInfoByReqId | Library.BLL/SInventory_BLL/RequisitionBLL.cs:196 |
| RequisitionBLL.ChalanNo | Library.BLL/SInventory_BLL/RequisitionBLL.cs:113 |
| RequisitionBLL.UpdateIssueInformationOnRequisition | Library.BLL/SInventory_BLL/RequisitionBLL.cs:211 |
| RequisitionBLL.UpdateStockTransfarInfoUpdate | Library.BLL/SInventory_BLL/RequisitionBLL.cs:221 |
| RequisitionBLL.ReqQtyUpdateInReqDetail | Library.BLL/SInventory_BLL/RequisitionBLL.cs:233 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| RequisitionDAL.ChallanExistsDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:256 | inline SQL: tblRequisition | @ReqId |
| RequisitionDAL.GetRequisitionDetailByReqIdDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:191 | inline SQL: tblStockInTransfar | @ReqId |
| ProductDAL.ProductPriceDetailWithCase | Library.DAL/SInventory_DAL/ProductDAL.cs:278 | inline SQL: tblProduct, tblProductCase, tbl_INV_CentralStore_WeightedAvg | @ProductCode |
| RequisitionDAL.GetRequisitionInfoByReqIdDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:234 | inline SQL: tblRequisition | @ReqId |
| ProductDAL.GetDataTableByText | Library.DAL/SInventory_DAL/ProductDAL.cs:25 | delegates / no SQL literal |  |
| ProductDAL.ToDbValue | Library.DAL/SInventory_DAL/ProductDAL.cs:20 | delegates / no SQL literal |  |
| RequisitionDAL.UpdateIssueInformationOnRequisitionDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:269 | inline SQL: tblRequisition |  |
| RequisitionDAL.RequisitionIssueParameters | Library.DAL/SInventory_DAL/RequisitionDAL.cs:782 | delegates / no SQL literal | @DriverName, @GrandTotalPrice, @IssuChalanDate, @IssueChalanNo, @ReqId, @Submit, @SubmitDate, @Total |
| RequisitionDAL.UpdateStockTransfarInfoUpdateDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:465 | inline SQL: tblStockInTransfar | @IsIssue, @PriceAmount, @Quantity, @StockInTransfarId, @TotalPriceAmount, @VATAmount |
| RequisitionDAL.UpdateReqDetailIssueStatusDAL | Library.DAL/SInventory_DAL/RequisitionDAL.cs:480 | inline SQL: tblRequsitionChild | @ReqChildId |
| RequisitionDAL.ReqDetailUpdateAfterIssue | Library.DAL/SInventory_DAL/RequisitionDAL.cs:453 | inline SQL: tblRequsitionChild | @IssueQty, @PriceAmount, @ReqChildId, @TotalPrice, @VATAmount |

## 16. Stored Procedure Mapping
No stored-procedure name resolved from this page (inline SQL or no DB access).

## 17. Database Mapping
- Tables/views READ (inline SQL reachable): tblProduct, tblProductCase, tblRequisition, tblStockInTransfar, tbl_INV_CentralStore_WeightedAvg
- Tables WRITTEN (inline SQL reachable): tblRequisition, tblRequsitionChild, tblStockInTransfar
- Tables touched by stored procedures listed in §16: `UNKNOWN` unless the SP body exists in the repo (see `docs/database/stored-procedures.md`).

| Table | Column | Bound parameter |
|---|---|---|
| tblRequisition | DriverName | @DriverName |
| tblRequisition | GrandTotalPrice | @GrandTotalPrice |
| tblRequisition | IssuChalanDate | @IssuChalanDate |
| tblRequisition | IssueChalanNo | @IssueChalanNo |
| tblRequisition | Submit | @Submit |
| tblRequisition | SubmitDate | @SubmitDate |
| tblRequisition | TotalPrice | @TotalPrice |
| tblRequisition | TotalVAT | @TotalVAT |
| tblRequisition | TruckNo | @TruckNo |
| tblRequsitionChild | IssueQty | @IssueQty |
| tblRequsitionChild | PriceAmount | @PriceAmount |
| tblRequsitionChild | TotalPrice | @TotalPrice |
| tblRequsitionChild | VATAmount | @VATAmount |
| tblStockInTransfar | IsIssue | @IsIssue |
| tblStockInTransfar | PriceAmount | @PriceAmount |
| tblStockInTransfar | Quantity | @Quantity |
| tblStockInTransfar | TotalPriceAmount | @TotalPriceAmount |
| tblStockInTransfar | VATAmount | @VATAmount |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Database | Error message | BR ID |
|---|---|---|---|---|---|---|
| (handler Page_Load) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Challan Already Generated  !! | UNKNOWN |

## 19. Business Rules
None verified at Tier B. Candidate rules must be read from the handlers listed in §12 and recorded here with `BR-<module>-NNN`, source and confidence (CONFIRMED/INFERRED/UNKNOWN). See `specs/SPEC-GAPS.md`.

## 20. Permission Matrix
- Session keys read: none
- Authentication: Forms auth (`Web.config` `<authentication mode="Forms">`, loginUrl `~/Login.aspx`); master page redirects to Login when `Session["UserId"]` is null (only when the master is used).
- Authorization: menu-based — a role sees the page only if its URL is in the menu HTML produced by `dbo.MainMenu2` for that user/role (role-menu rows managed via `sp_Save_MenuRole`/`tblMenuDistribution`). Direct URL access is not additionally checked in this page's code unless `Session` keys above are tested. Per-role matrix: UNKNOWN (role data lives in the database).

## 21. Status Lifecycle
UNKNOWN at Tier B (no status transition extracted). See module README for module-level lifecycle where verified.

## 22. Error Handling
- try/catch present in 0 of 11 methods.
- Redirect targets: ViewRequisitionForIssue.aspx
- User messages: Challan Already Generated  !!
- Global handler: `Application_Error` in the app-level `Global.asax` is empty (no central logging).

## 23. Audit
Audit-like columns bound in inline SQL: none detected (SP-based writes: UNKNOWN). Login/tracking: `tblLoginLog` / `tblUserSessionTracking` (see M01).

## 24. Integration
No integration-related classes or SPs detected from this page.

## 25. Reports
No report file referenced from this page.

## 26. Dependencies
BLL: ProductBLL, RequisitionBLL; DAL: ProductDAL, RequisitionDAL; DAO: Requesition, StockInTransfar; Tables (inline): tblProduct, tblProductCase, tblRequisition, tblRequsitionChild, tblStockInTransfar, tbl_INV_CentralStore_WeightedAvg

## 27. Test Cases
GENERATED TEMPLATE — none executed. No automated tests exist in the repository.

| ID | Scenario | Expected (to be confirmed) |
|---|---|---|
| TC-01 | Open page as an authorised user | Page renders without server error; controls in §7 present |
| TC-02 | Open page with no session | Redirected to Login.aspx (if master page in use); verify per page |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx`, `Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed before this reverse-engineering (no specs/ directory) | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx | n/a | MISSING | — |
| UI controls | This document §7 | 34 controls parsed | n/a | PARTIAL | Solution.Web/SInventory_UI/IssueRequisitionProducts.aspx |
| Data access | This document §15–17 | 0 SP names, 6 inline-SQL tables | n/a | PARTIAL | §16 |
| Business rules | none | not extracted | not extracted | UNKNOWN | §19 |

## 30. Known Gaps
- Business objective, business rules and status lifecycle not reviewed (Tier B).

## 31. Change Impact
- Module: M06 (Order Management).
- Other pages sharing this page's stored procedures: none detected
- Changing DAL methods ProductDAL.GetDataTableByText, ProductDAL.ProductPriceDetailWithCase, ProductDAL.ToDbValue, RequisitionDAL.ChallanExistsDAL, RequisitionDAL.GetRequisitionDetailByReqIdDAL affects every page that calls them; check `docs/reverse-engineering/dependency-map.md`.
