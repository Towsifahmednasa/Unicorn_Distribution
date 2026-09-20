---
page_id: PAGE-682
module: M17
page: SAP_StockReceive
tier: A
generated: false   # upgraded by manual code review (Tier A)
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-682 — SAP_StockReceive

> **Evidence level:** Tier A - reviewed manually against `.aspx`, `.aspx.cs`, and the BLL/DAL code they reach (review date 2026-09-19). Claims are tagged CONFIRMED / INFERRED / UNKNOWN. Stored-procedure bodies are NOT in the repository unless a section says so; column-level effects of such SPs stay `UNKNOWN`. Sections 1, 4-10, 13-15 and 28 still carry the mechanically extracted content (re-checked where noted); all other sections were rewritten.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-682 |
| Module | M17 — SAP, Mobile-App & External Integration (INFERRED from folder + file name) |
| Page name | SAP_StockReceive |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SAP_Integration |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SAP_Integration_SAP_StockReceive |
| aspx size | 9558 bytes |
| Code-behind LOC | 210 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
Detail/approval screen for **one SAP stock-movement challan** (from PAGE-680's Preview). Shows challan no/date/status/from-warehouse/to-plant and the product lines (code, name, pack, batch, unit price, quantity [editable], current stock). The user may correct quantities and press **Approve** to receive the stock into the ERP. CONFIRMED: `Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs`.

## 3. Business Objective
Receive stock shipped by SAP (plant-to-plant / central-to-DC challan) into ERP inventory after confirming quantities and price/stock sanity, so that ERP stock matches SAP.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SAP_Integration/SAP_IntrigationPoint.aspx`

Outbound references: `sap_intrigationpoint.aspx`

## 5. Route
`~/SAP_Integration/SAP_StockReceive.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SAP_Integration/SAP_StockReceive.aspx |
| Code-behind | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs |
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
| txtquantity | TextBox | <%#Eval("quantity") %> | no validator found |  |  |  |  |
| (no id) | BoundField | Current Stock | no validator found |  |  |  |  |
| ApproveButton | LinkButton |  | no validator found |  |  |  | OnClick→ApproveButton_Click |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

## 8. Field Mapping
| UI Control | DTO.Property / variable | Controller (handler) | Service (BLL) | Repository (DAL) | SP / SQL parameter | Table | Column |
|---|---|---|---|---|---|---|---|
| hdfStockMovementMasterId | dtTable | SAP_Integration_SAP_StockReceive.LoadSapDataById | — | SAP_IntrigationPointDAL.GetSAPStockDataById | UNKNOWN | UNKNOWN | UNKNOWN |
| hfStockMovementDetailId | StockMovementDetailId | SAP_Integration_SAP_StockReceive.ApproveButton_Click | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | UNKNOWN | UNKNOWN | UNKNOWN |
| txtquantity | quantity | SAP_Integration_SAP_StockReceive.ApproveButton_Click | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | UNKNOWN | UNKNOWN | UNKNOWN |
| lblChallanNo | Res | SAP_Integration_SAP_StockReceive.ApproveButton_Click | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | UNKNOWN | UNKNOWN | UNKNOWN |
| txtquantity | quantity | SAP_Integration_SAP_StockReceive.DataValidation | — | — | UNKNOWN | UNKNOWN | UNKNOWN |

Mapping extracted from assignments of the form `Target = control.Text|SelectedValue|Checked` inside handlers, then followed through `SqlParameter("@x", obj.Prop)` in the resolved DAL method. Column is filled only when an inline `INSERT/UPDATE` in the DAL names it; for stored-procedure saves it is `UNKNOWN`.

## 9. Dropdown Mapping
No dropdown controls.

## 10. Button Action Mapping
| Button | Event | JS | Controller.Action | Source | Service (BLL) | Repository (DAL) | Stored procedure | DB change |
|---|---|---|---|---|---|---|---|---|
|  | LinkButton click | (postback) | SAP_Integration_SAP_StockReceive.BackToListButton_Click | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs:207 | — | — | — | read-only / none detected |
| ApproveButton | LinkButton click | (postback) | SAP_Integration_SAP_StockReceive.ApproveButton_Click | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs:120 | — | SAP_IntrigationPointDAL.SaveStockReceive, SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | sp_SAP_StockReceive, sp_SAP_Up_StockReceiveQty | SP executed — effect on tables UNKNOWN (body not in repo); write-like name |

## 11. Frontend Flow
- `Page_Load` (first load): clears `hdfStockMovementMasterId`; if `Session["StockMovementMasterId"]` set -> `LoadSapDataById()` (whole call in try/catch, errors ignored).
- `LoadSapDataById`: `sp_Get_SAP_StockReceivePendingDataById(@StockMovementMasterId)` -> header labels from row 0 (`challan_code, challan_date, action, FromWH, to_plant_code`), grid bound to all rows; for each row: `ReceiveType`, `ProductCode`, `StockQuantity`, `UnitPrice`, `quantity`, `StockQuantityChk` are read from `DataKeys`. Rules in BR-M17-004.
- `ApproveButton_Click` -> `DataValidation()` then per-row quantity update then `SaveStockReceive` (BR-M17-005). Success -> `successalert(...,'SAP_IntrigationPoint.aspx')`; failure `Already Exist!`.
- Back button -> `SAP_IntrigationPoint.aspx`.

## 12. Backend Flow
| Handler | Source | Params | Call chain (page → BLL → DAL → SP) | try/catch | Txn |
|---|---|---|---|---|---|
| SAP_Integration_SAP_StockReceive.Page_Load | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs:16 | object sender, EventArgs e | SAP_Integration_SAP_StockReceive.Page_Load → SAP_IntrigationPointDAL.GetSAPStockDataById → SP: sp_Get_SAP_StockReceivePendingDataById | yes | no |
| SAP_Integration_SAP_StockReceive.LoadSapDataById | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs:38 |  | SAP_Integration_SAP_StockReceive.LoadSapDataById → SAP_IntrigationPointDAL.GetSAPStockDataById → SP: sp_Get_SAP_StockReceivePendingDataById | yes | no |
| SAP_Integration_SAP_StockReceive.ApproveButton_Click | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs:120 | object sender, EventArgs e | SAP_Integration_SAP_StockReceive.ApproveButton_Click → SAP_IntrigationPointDAL.SaveStockReceive / SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive → SP: sp_SAP_StockReceive, sp_SAP_Up_StockReceiveQty | no | yes |
| SAP_Integration_SAP_StockReceive.BackToListButton_Click | Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs:207 | object sender, EventArgs e | SAP_Integration_SAP_StockReceive.BackToListButton_Click | no | no |

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
| (handler ApproveButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | ','Success','SAP_IntrigationPoint.aspx'); | UNKNOWN |
| (handler ApproveButton_Click) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | Already Exist! | UNKNOWN |
| (handler DataValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | form-control form-control-sm is-invalid | UNKNOWN |
| (handler DataValidation) | message literal in code-behind (condition not extracted) | no | yes (server-side message) | UNKNOWN | please fill out this field | UNKNOWN |

## 19. Business Rules
| ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M17-004 | **Line sanity flags.** A row is painted red and the Approve button hidden (`ApproveButton.Visible=false`) when `ProductCode` is empty or `quantity == 0`; for `ReceiveType == "B2B"` also when `StockQuantityChk == 0`. The unit-price-zero test is commented out (`//|| unitPrice == 0`). For non-B2B receives the column with index 7 (the "Current Stock" column by markup order, INFERRED) is hidden. `unitPrice`, `currentStockQuantity` are read but unused otherwise. | `SAP_StockReceive.aspx.cs:71-104` | code-behind | CONFIRMED |
| BR-M17-005 | **Approve.** Guards: challan label non-empty (`There is no challan number for approval !!`), grid has rows (`There is no product for approval !!`), and `DataValidation()`: every `txtquantity` non-empty and `> 0` (invalid box gets `is-invalid` + tooltip; first failure stops). Then for every line `sp_SAP_Up_StockReceiveQty(@StockMovementDetailId,@quantity)` **overwrites the staged SAP quantity** with the value typed, followed by `sp_SAP_StockReceive(@ChallanNo,@ApproveBy=Session UserId)`. The line updates and the receive are separate DAL calls on separate connections/transactions - not atomic; the result of each `UpdateSAPQtyStockReceive` is ignored. There is no check that typed quantity `<=` shipped quantity or `<=` available stock. | `SAP_StockReceive.aspx.cs:106-208`; DAL `:307-373` | code-behind + SP | CONFIRMED |
| BR-M17-002 | (challan visibility) as PAGE-680. | - | - | CONFIRMED |

## 20. Permission Matrix
Session keys: `StockMovementMasterId` (navigation state; never cleared -> stale after use), `UserId` (in DAL as `@ApproveBy`). No role check; anyone with the menu entry can receive stock. Page can be opened directly without the list (then only the previous `StockMovementMasterId` in session is loaded).

## 21. Status Lifecycle
Challan status shown = staged `action` value; ERP-side transition is executed inside `sp_SAP_StockReceive` (body UNKNOWN). After success the challan is expected to disappear from the pending list / be flagged by `sp_Get_SAP_HideChallanByChallanNo` (INFERRED).

## 22. Error Handling
- `Page_Load` swallow-all try/catch (blank page on failure).
- `LoadSapDataById` failures -> popup `No data found!`.
- Approve: SP exceptions bubble (DAL rollback+rethrow) - not caught; `UpdateSAPQtyStockReceive` and `SaveStockReceive` each rethrow.
- Messages: `There is no product for approval !!`, `There is no challan number for approval !!`, `Operation successful!`, `Already Exist!`.

## 23. Audit
`@ApproveBy` = UserId; whether SP stamps date/time UNKNOWN. Quantity edits are audit-free (the staged quantity is overwritten in place - the original SAP quantity is lost unless the SP archives it, UNKNOWN). SP bodies are NOT in the repository (only `sp_SAP_B2B*`, `sp_SAP_ChallanSend*`, `sp_SAP_Up_SAP_Challan*` in `SP_NewStockTransferDcToDc_Related.sql` and the two provider-dropout SPs in `Library.DAL/SAP_IntegrationDAL/sql/`).

## 24. Integration
SAP -> ERP stock movement acceptance (INT-02). The **B2B** variants of the SPs (`sp_SAP_B2BChallanMasterInsert`, `sp_SAP_B2BChallanDetailInsert`, `sp_SAP_B2BStockReceiveByDc`) whose bodies are in `SP_NewStockTransferDcToDc_Related.sql` have no caller in C# and are probably invoked from inside `sp_SAP_StockReceive` (INFERRED).

## 25. Reports
None on screen; after save the user is returned to the list.

## 26. Dependencies
BLL: —; DAL: SAP_IntrigationPointDAL; DAO: ResultInfo; Tables (inline): —

## 27. Test Cases
| ID | Type | Scenario | Expected | BR |
|---|---|---|---|---|
| TC-682-01 | Positive | Preview a challan with all lines valid, keep quantities, Approve | per-line `sp_SAP_Up_StockReceiveQty`, then `sp_SAP_StockReceive`; success popup, redirect to list | BR-M17-005 |
| TC-682-02 | Negative | Empty a quantity box | box invalid, no DB call | BR-M17-005 |
| TC-682-03 | Negative | Quantity 0 or negative | invalid, no DB call | BR-M17-005 |
| TC-682-04 | Negative | Line with empty ProductCode (SAP code not mapped) | row red, Approve hidden | BR-M17-004 |
| TC-682-05 | Negative | B2B line with `StockQuantityChk = 0` | row red, Approve hidden | BR-M17-004 |
| TC-682-06 | Edge | Type quantity greater than shipped | accepted (no upper bound) | BR-M17-005 |
| TC-682-07 | Failure | Second call fails after line updates succeeded | staged quantities changed, challan not received (partial) | BR-M17-005 |
| TC-682-08 | Edge | Open page directly with stale session id | shows last previewed challan | permission |

## 28. Reverse Engineering References
- `Solution.Web/SAP_Integration/SAP_StockReceive.aspx`, `Solution.Web/SAP_Integration/SAP_StockReceive.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `SAP_StockReceive.aspx(.cs)` | n/a | MISSING | - |
| Quantity limits | None existed | none | UNKNOWN | RISK | BR-M17-005 |
| Atomicity | None existed | 1 call per line + 1 receive | UNKNOWN | RISK | BR-M17-005 |

## 30. Known Gaps
- Non-atomic quantity overwrite + receive; no max quantity/stock validation; original SAP quantity overwritten.
- Session-carried id never cleared; direct URL reuse.
- Swallow-all catch in `Page_Load`.
- Unused variables (`currentStockQuantity`, `unitPrice`); price validation disabled.
- `sp_SAP_B2B*` script defects (BR-M17-009) affect the B2B path that this page feeds.
- SP bodies for `sp_Get_SAP_StockReceivePendingDataById`, `sp_SAP_Up_StockReceiveQty`, `sp_SAP_StockReceive` missing (GAP-M17-001).

## 31. Change Impact
Same DAL/SPs as PAGE-680/681/683. Changing `sp_SAP_StockReceive` affects DC/central stock (tables `tblDCStore`, `tblChalanInfo/Detail`, `tblWHStockInMaster` per the B2B script) - INFERRED.

