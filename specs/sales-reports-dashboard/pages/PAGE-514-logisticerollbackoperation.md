---
page_id: PAGE-514
module: M13
page: LogisticeRollBackOperation
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "COMPLETE", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---
# PAGE-514 — LogisticeRollBackOperation

> **Evidence level:** Tier A - `.aspx`, `.aspx.cs` and the reachable BLL/DAL code were read; SP bodies are NOT in the repository (except where stated), so SP-internal behaviour is `UNKNOWN`. Confidence tags: CONFIRMED / INFERRED / UNKNOWN. Reviewed 2026-09-19.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-514 |
| Module | M13 — Sales Reports, Analytics & Dashboards (INFERRED from folder + file name) |
| Page name | LogisticeRollBackOperation |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_LogisticeRollBackOperation |
| aspx size | 22804 bytes |
| Code-behind LOC | 186 |
| Inline script size | 1000 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**LogisticeRollBackOperation** ("Logistic Rollback") undoes stages of the central-warehouse -> distribution-centre requisition chain: STO generation, picking generation and challan generation. It is a data-modifying admin screen.

## 3. Business Objective
Allow logistics staff to step a requisition back one stage (delete an STO, cancel a picking, cancel an issue challan) and return the central stock, so it can be re-processed. INFERRED from labels ("STO Generate", "Picking Generate", "Challan Generate") and the SQL effects (CONFIRMED).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): none found

Outbound references: none found

## 5. Route
`~/SInventory_UI/LogisticeRollBackOperation.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/LogisticeRollBackOperation.aspx |
| Code-behind | Solution.Web/SInventory_UI/LogisticeRollBackOperation.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | RollBackBLL (Library.BLL/SInventory_BLL/RollBackBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | RollBackDAL (Library.DAL/SInventory_DAL/RollBackDAL.cs) |

## 7. UI Control Inventory
Generated inventory:

| Control ID | Type | Label / Text | Required | Default | Read-only / Enabled / Visible | Data source | Events → handler |
|---|---|---|---|---|---|---|---|
| DropDownList1 | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→DropDownList1_SelectedIndexChanged |
| loadGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Req. No | no validator found |  |  |  |  |
| (no id) | BoundField | Req. Date | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Name | no validator found |  |  |  |  |
| (no id) | BoundField | Total Qty. | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?');, OnClick→ImageButton2_Click |
| viewReqGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | Requision No | no validator found |  |  |  |  |
| (no id) | BoundField | Req Date | no validator found |  |  |  |  |
| (no id) | BoundField | Picking No | no validator found |  |  |  |  |
| (no id) | BoundField | Picking Date | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Name | no validator found |  |  |  |  |
| (no id) | BoundField | Manufacturer | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?');, OnClick→editImageButton_OnClick |
| stockInTraGridView | GridView |  | no validator found |  |  |  | OnPreRender→gv_DocumentUpload_PreRender |
| (no id) | BoundField | ChalanNo | no validator found |  |  |  |  |
| (no id) | BoundField | ChalanDate | no validator found |  |  |  |  |
| (no id) | BoundField | TruckNo | no validator found |  |  |  |  |
| (no id) | BoundField | DriverName | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?');, OnClick→edit2ImageButton_OnClick |
| DropDownList1 | DropDownList |  | no validator found |  |  | see §9 | onselectedindexchanged→DropDownList1_SelectedIndexChanged |
| loadGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Req. No | no validator found |  |  |  |  |
| (no id) | BoundField | Req. Date | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Name | no validator found |  |  |  |  |
| (no id) | BoundField | Total Qty. | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?');, OnClick→ImageButton2_Click |
| viewReqGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | Requision No | no validator found |  |  |  |  |
| (no id) | BoundField | Req Date | no validator found |  |  |  |  |
| (no id) | BoundField | Picking No | no validator found |  |  |  |  |
| (no id) | BoundField | Picking Date | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Code | no validator found |  |  |  |  |
| (no id) | BoundField | D.C. Name | no validator found |  |  |  |  |
| (no id) | BoundField | Manufacturer | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?');, OnClick→editImageButton_OnClick |
| stockInTraGridView | GridView |  | no validator found |  |  |  |  |
| (no id) | BoundField | ChalanNo | no validator found |  |  |  |  |
| (no id) | BoundField | ChalanDate | no validator found |  |  |  |  |
| (no id) | BoundField | TruckNo | no validator found |  |  |  |  |
| (no id) | BoundField | DriverName | no validator found |  |  |  |  |
| (no id) | TemplateField | Delete | no validator found |  |  |  |  |
| editImageButton | ImageButton |  | no validator found |  |  |  | OnClientClick→return confirm('Are you sure you want to Delete ?');, OnClick→edit2ImageButton_OnClick |

Attributes not listed above (Display field, Value field where set only in code, Disabled/Visible conditions set in code-behind, Error message text shown at runtime, Permission per control) are `UNKNOWN` at Tier B; see §9 for code-bound dropdown fields and §18 for validators.

**Verified**: DropDownList1 (options blank / SG "STO Generate" / PG "Picking Generate" / CG "Challan Generate", AutoPostBack, select2) chooses which of three panels/grids is visible (`sto`, `picking`, `challan` divs are hidden/shown in `DropDownList1_SelectedIndexChanged`). Grids: `loadGridView` (DataKey ReqId; Req.No, Req.Date, D.C. Code/Name, Total Qty, Delete), `viewReqGridView` (DataKeys ReqId, ComUnitId; Requision No, Req Date, Picking No, Picking Date, D.C., Manufacturer, Delete), `stockInTraGridView` (DataKey ReqId; ChalanNo, ChalanDate, TruckNo, DriverName, Delete). Every Delete icon has `confirm('Are you sure you want to Delete ?')`. No validators, no required fields.

## 8. Field Mapping
| UI | Key | Handler -> BLL -> DAL | Effect |
|---|---|---|---|
| loadGridView row delete | ReqId | `ImageButton2_Click` -> `RequisitionBLL.DeleteRequisition` -> `RequisitionDAL.DeleteRequisition` | `DELETE tblRequisition; DELETE tblRequsitionChild` WHERE ReqId |
| viewReqGridView row delete | ReqId | `editImageButton_OnClick` -> `RollBackBLL` (5 calls) | see BR-M13-012 |
| stockInTraGridView row delete | ReqId | `edit2ImageButton_OnClick` -> `RollBackBLL` (7 calls) | see BR-M13-013 |

## 9. Dropdown Mapping
| Dropdown | Source | Value/text |
|---|---|---|
| DropDownList1 | static ListItems | SG/PG/CG; each selection loads a grid: SG `RequisitionBLL.GetRequisitionView()`, PG `GetAllNonSubmitReq()`, CG `RollBackBLL.GetAllStockRcvByDcDAL()` |

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| delete icon (SG) | ImageButton2_Click | deletes requisition; alert "RollBack Successfull!!" / "Error!!" |
| delete icon (PG) | editImageButton_OnClick | picking rollback |
| delete icon (CG) | edit2ImageButton_OnClick | challan rollback |

## 11. Frontend Flow
Choose stage -> postback fills the corresponding grid -> click delete icon (confirm) -> postback -> handler -> alert, grid reloaded. `showMessageBox` uses `message.Replace("'", "\'")` (no effect on the alert text).

## 12. Backend Flow
**SG**: `DeleteRequisition(reqId)` (one batch `DELETE` of header then children, not in a transaction) -> bool -> reload SG grid.

**PG** (`editImageButton_OnClick`): `GetStockInTransfer(reqId)` (SELECT * FROM tblStockInTransfar WHERE ReqId) -> `UpdatePickingInformationOnRequisitionDAL(reqId)` -> for each stock-in-transfer row: `UpdateIssueInformationOnRequisitionChildDAL(ReqChildId)` and `UpdateCentralStockStockOut(Quantity, ReceiveId)` (tblCentralStore.Quantity += Quantity) -> `DeleteStockInTransfer(reqId)` (3 deletes in one batch) -> success only if `UPsts && Delsts`.

**CG** (`edit2ImageButton_OnClick`): same as PG plus `UpdateIssueInformationOnRequisition(reqId)` (Submit=NULL, IssueChalanNo=NULL) and `UpdateStockTransfarInfoUpdate(reqId)` (tblRequsitionChild.IsIssue=NULL) executed after the delete regardless of outcome.

No transaction spans the steps; each `SInventorySql.Execute` auto-commits (BR-M13-014).

## 13. API Mapping
No `[WebMethod]`, ASMX, WCF or Web API endpoint on this page; all interaction is WebForms postback (ScriptManager/UpdatePanel). CONFIRMED (no `[WebMethod]` in the code-behind).

## 14. Service Mapping
| BLL | Methods |
|---|---|
| RequisitionBLL | GetRequisitionView, GetAllNonSubmitReq, DeleteRequisition |
| RollBackBLL | GetStockInTransfer, UpdatePickingInformationOnRequisitionDAL, UpdateIssueInformationOnRequisitionChildDAL, UpdateCentralStockStockOut, DeleteStockInTransfer, GetAllStockRcvByDcDAL, UpdateIssueInformationOnRequisition, UpdateStockTransfarInfoUpdate |

## 15. Repository Mapping
| DAL | Methods |
|---|---|
| RequisitionDAL | GetRequisitionView, GetAllNonSubmitReqDAL, DeleteRequisition |
| RollBackDAL | all inline SQL via `SInventorySql` (parameterised) |

## 16. Stored Procedure Mapping
None. All SQL is inline and parameterised (`RollBackDAL.cs`, `RequisitionDAL.cs`).

## 17. Database Mapping
| Table | Op | Detail |
|---|---|---|
| tblRequisition | R, U, D | list filters (see BR-M13-011..013); U clears CreatePicking, PickingDate, PickingNo, TruckNo, DriverName, TotalPrice, TotalVAT, GrandTotalPrice; CG also Submit=NULL, IssueChalanNo=NULL; SG deletes the row |
| tblRequsitionChild | U, D | clears IssueQty, UnitPrice, PriceAmount, VATAmount, TotalPrice, IsPicking, CaseQty, MusakVATAmount, MusakTotalPrice (per ReqChildId); CG IsIssue=NULL (per ReqId); SG deletes children |
| tblStockInTransfar | R, D | rows of the requisition |
| tbl_INV_Central_BatchWiseStock, tbl_INV_CentralStore_WeightedAvg | D | rows keyed by StockInTransfarId |
| tblCentralStore | U | Quantity = Quantity + rolled-back Quantity (by ReceiveId) |
| tblManufacturer | R | join in list query |

## 18. Validation Matrix
No input validation beyond DataKey lookups and browser confirm. Messages: "RollBack Successfull!!" / "Error!!". No check that the stage is actually the latest one for the requisition, or that the DC has not already received stock (CG list only excludes rows where ReceiveIssue is set).

## 19. Business Rules
| BR id | Rule | Source | Implementation location | Confidence |
|---|---|---|---|---|
| BR-M13-011 | "STO Generate" (SG) rollback lists tblRequisition rows with CreatePicking IS NULL (grouped with SUM(ReqQty)) and the delete icon physically deletes the requisition header and its tblRequsitionChild rows. | `Library.DAL/SInventory_DAL/RequisitionDAL.cs:150` | LogisticeRollBackOperation.ImageButton2_Click -> RequisitionBLL.DeleteRequisition -> RequisitionDAL.DeleteRequisition | CONFIRMED |
| BR-M13-012 | "Picking Generate" (PG) rollback lists requisitions with Submit NULL/empty and CreatePicking set. Rollback: clear picking columns on tblRequisition (CreatePicking, PickingDate, PickingNo, TruckNo, DriverName, TotalPrice, TotalVAT, GrandTotalPrice), clear issue columns on each tblRequsitionChild (IssueQty, UnitPrice, PriceAmount, VATAmount, TotalPrice, IsPicking, CaseQty, Musak*), add each tblStockInTransfar.Quantity back to tblCentralStore.Quantity (by ReceiveId) and delete tbl_INV_Central_BatchWiseStock, tbl_INV_CentralStore_WeightedAvg and tblStockInTransfar rows of the requisition. | `Library.DAL/SInventory_DAL/RollBackDAL.cs:29` | LogisticeRollBackOperation.editImageButton_OnClick -> RollBackBLL -> RollBackDAL | CONFIRMED |
| BR-M13-013 | "Challan Generate" (CG) rollback lists tblRequisition with Submit='OK' and ReceiveIssue NULL/empty (issued but not yet received by DC) and performs the picking rollback steps plus tblRequisition.Submit=NULL, IssueChalanNo=NULL and tblRequsitionChild.IsIssue=NULL. | `Library.DAL/SInventory_DAL/RollBackDAL.cs:78` | LogisticeRollBackOperation.edit2ImageButton_OnClick | CONFIRMED |
| BR-M13-014 | The logistics rollback steps are separate auto-committed SQL calls (SInventorySql.Execute) with no transaction; the success alert depends only on the picking-reset and delete calls (UPsts && Delsts), so a mid-way failure can leave a partly rolled-back requisition. | `Solution.Web/SInventory_UI/LogisticeRollBackOperation.aspx.cs:122` | LogisticeRollBackOperation.aspx.cs | CONFIRMED |

## 20. Permission Matrix
- Enforced in code: `NewMasterPage.master.cs:24-56` redirects to `Login.aspx` when `Session["UserId"]` is null on the first (non-postback) load; postbacks are not re-checked.
- Everything else is menu-based only: the page has no role check of its own unless listed above (CONFIRMED by reading the code-behind).
- No role check in code for a destructive stock operation (GAP-M13-004).

## 21. Status Lifecycle
Requisition life-cycle as inferred from the three list filters (CONFIRMED filters, INFERRED order): requisition created -> STO generated (`CreatePicking IS NULL`) -> picking created (`CreatePicking` set, `Submit` empty) -> challan issued (`Submit='OK'`, `ReceiveIssue` empty) -> received by DC (`ReceiveIssue` set). This page moves a requisition one step backwards per action: CG -> PG state, PG -> STO state; SG deletes the requisition. The forward transitions are set in other modules.

## 22. Error Handling
No try/catch; the DAL helpers return `bool`; the page shows "Error!!" when a flag is false. Exceptions from `SInventorySql` propagate as unhandled.

## 23. Audit
No audit/log rows are written (no user, no date, no reason). Rolled-back stock-in-transfer rows are hard-deleted.

## 24. Integration
No external integration on this page; SAP STO/MIGO flows (other modules) are not reversed here - UNKNOWN whether an SAP document may exist for a rolled-back challan.

## 25. Reports
None.

## 26. Dependencies
RequisitionBLL/RequisitionDAL, RollBackBLL/RollBackDAL, SInventorySql; tables listed in §17.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-01 | Select SG | grid lists requisitions with CreatePicking NULL | BR-M13-011 |
| TC-02 | Delete an SG row | requisition + children deleted, alert success | BR-M13-011 |
| TC-03 | Select PG, delete row | picking fields cleared, central stock +qty, stock-in-transfer rows deleted | BR-M13-012 |
| TC-04 | Select CG, delete row | as PG plus Submit/IssueChalanNo NULL, IsIssue NULL | BR-M13-013 |
| TC-05 | Simulate failure between UpdateCentralStock and DeleteStockInTransfer | partial rollback persists (no transaction) | BR-M13-014 |
| TC-06 | Roll back a challan already received by DC | not listed (ReceiveIssue set) | BR-M13-013 |
| TC-07 | User without logistics role opens URL | (gap) all actions available | GAP-M13-004 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/LogisticeRollBackOperation.aspx(.cs)`
- `Library.BLL/SInventory_BLL/RollBackBLL.cs`, `Library.DAL/SInventory_DAL/RollBackDAL.cs`, `RequisitionDAL.cs`

## 29. Spec vs Actual
| Area | Specification | Actual code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | see §2-§26 | n/a | MISSING | - |
| Business rules | none | BR-M13-011..014 | inline SQL | CONFIRMED | §19 |

## 30. Known Gaps
- Non-transactional multi-step rollback (BR-M13-014); success message ignores several step results.
- No audit trail, no role check, no reason capture.
- `edit2ImageButton_OnClick` runs `UpdateIssueInformationOnRequisition`/`UpdateStockTransfarInfoUpdate` even when earlier steps failed.
- Central stock is restored using the stock-in-transfer quantity; batch-wise/weighted-average tables are deleted, not adjusted; DC-side stock (if the challan was already picked/issued to DC) is not addressed.
- Large commented-out legacy markup remains in the .aspx (lines 175+).

## 31. Change Impact
- Module M13. `RollBackDAL/BLL` are used only here. `RequisitionDAL.DeleteRequisition/GetRequisitionView` are shared with the requisition pages of the inventory module.
- Tables shared with central-store/requisition/challan pages (tblRequisition, tblRequsitionChild, tblStockInTransfar, tblCentralStore).
