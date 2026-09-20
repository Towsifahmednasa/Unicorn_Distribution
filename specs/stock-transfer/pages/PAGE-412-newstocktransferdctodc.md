---
page_id: PAGE-412
module: M11
page: NewStockTransferDcToDc
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-412 — NewStockTransferDcToDc (B2B / DC-to-DC stock transfer: challan send)

> **Evidence level:** Tier A — `NewStockTransferDcToDc.aspx.cs` read in full (488 lines); control inventory of the `.aspx`; `ChalanBLL`, `ChalanDAL` (send + receive + delete paths), `B2BTransferViewDal`; SP bodies read fully from `SP_NewStockTransferDcToDc_Related.sql` (`sp_Del_B2BDelete`, `sp_DeleteDwpotToWhChalan`, `sp_SAP_B2BStockReceiveByDc`, `sp_SAP_B2BChallanMasterInsert`, `sp_SAP_B2BChallanDetailInsert`, `sp_SAP_ChallanSendDetailByChalanId`, `sp_SAP_Up_SAP_ChallanSendByChalanId`, `sp_SAP_Up_SAP_ChallanConfirmByChalanId`) and `SP_Fix_STO_Chalan_Process.sql`. UI coverage PARTIAL (markup styling/JS not reviewed line by line). No caller of the SAP SPs exists in the C# code (external integration, UNKNOWN).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-412 |
| Module | M11 — Stock Transfer & Chalan Receive |
| Name / class | NewStockTransferDcToDc / `SInventory_UI_NewStockTransferDcToDc` (`.aspx.cs:13`) |
| Kind | Entry / transaction — **changes stock at send time** |
| Size | aspx 788 lines, code-behind 488 lines |
| Legacy | `StockTransferDcToDc.aspx` (PAGE-419) is the older variant (not Tier A) |

## 2. Purpose
CONFIRMED (labels/messages "Please Insert chalanNo", "Transfer Qty. can't be more than Stock Quantity"): a user at the sending DC creates a **transfer challan** (chalan no auto-generated `IDT…`) moving batch-wise stock from one DC (from-unit code) to another DC (to-unit code), with driver/truck details, printable challan.

## 3. Business Objective
INFERRED: rebalance inventory between depots (business-to-business / B2B), keep an auditable challan with value and VAT, put the stock "in transit" (issued at source, not yet received) until the destination DC receives it (PAGE-423).

## 4. Navigation
Inbound: `B2BTransferView.aspx` (new button). Outbound: `B2BTransferView.aspx` (`backLinkButton_Click`), `NewStockTransferDcToDc.aspx` (cancel), `../SInventory_RPTVIEW/DcToDcChalanReportViewer.aspx?chalanno=` (print, `Button2_Click` :463-468).

## 5. Route
`~/SInventory_UI/NewStockTransferDcToDc.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View/code-behind | Solution.Web/SInventory_UI/NewStockTransferDcToDc.aspx(.cs) |
| BLL | Library.BLL/SInventory_BLL/ChalanBLL.cs (`SaveDataForChalanInfo` :18-24, `SaveDataForChalanDetail` :96-109, `ChalanNoGenerator` :133-151, `GetProductDcStore`, `ComUnit`) |
| DAL | Library.DAL/SInventory_DAL/ChalanDAL.cs (`SaveDataForChalanInfo` :17-41, `SaveDataForChalanDetail` :43-61, `GetProductDcStore` :440-490, `UpdateDCStockQuantity` :513-553, `DCStockInDALMain` :122-170, `ChalanUpdate` :62-69); B2BTransferViewDal.cs |
| SQL | SP_NewStockTransferDcToDc_Related.sql; SP_Fix_STO_Chalan_Process.sql |
| Other | CommonDataLoad (`GetPermissionForShowBtn`) |

## 7. UI Control Inventory
| Control | Type / label | Required | Default / state | Notes |
|---|---|---|---|---|
| chalanNoTextBox | TextBox (readonly) | Yes | `ChalanNoForShow()` = `IDT`+padded(MAX(ChalanId)+1) | overwritten at save with the id-based number |
| chalanDateTextBox | TextBox | Yes | today `dd-MMM-yyyy` | editable |
| manufacDropDownList | DropDownList | Yes (`SelectedValue==""`) | `tblManufacturer` | |
| fromComUnitCodeTextBox | TextBox (AutoPostBack) | Yes | empty | unit code typed; `ComUnit(code)` fills name/address/id (`fromCunithiddenValue`) |
| fromComUnitNameTextBox, fromComUnitAddressTextBox | TextBox | — | readonly | |
| toComUnitCodeTextBox (+ name/address) | TextBox (AutoPostBack) | Yes | empty | name/address editable text boxes; no destination id kept (matched later by `ToComUnitCode`) |
| truckNoTextBox, driverNameTextBox | TextBox | No | | |
| prodctCodeTextBox + Button1 "Show" | TextBox/LinkButton | product code required to search | | `GetProductDcStore(productCode, fromUnitCode)` |
| productGridView `DataKeyNames=DCStoreId,VATAmountPerUnit,UnitPrice` | GridView | — | | cols: select, code, name, Stock Qty, batch, expiry, transfer qty (`transferQtyTextBox`, digits only, AutoPostBack→`dQtyTextBox_TextChanged`) |
| addButton | LinkButton "Add" | — | | builds `chalanGridView` |
| chalanGridView `DataKeyNames=DCStoreId,VATAmountPerUnit,UnitPrice` | GridView | ≥1 row | | code, name, qty, batch, expiry, delete image |
| grandTotalWordLabel | Label | — | | Σ qty × unit price (excl. VAT) |
| Save (`submitButton_Click1`) | LinkButton, `sweetAlertConfirm_Submit` | — | hidden when `GetPermissionForShowBtn(path)` returns rows (btnSave.Visible=false, `:30-54`) | |
| printChalanNoTextBox + Button2 | TextBox/LinkButton | — | filled after save | print |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | SQL param | Table.column |
|---|---|---|---|---|---|---|
| chalanDateTextBox | `ChalanInfo.ChalanDate` | `submitButton_Click1` :172 | `SaveDataForChalanInfo` | `ChalanDAL.SaveDataForChalanInfo` :17-41 | `@ChalanDate` | tblChalanInfo.ChalanDate |
| generated id | `ChalanId = PrimaryKeyMax("ChalanId","tblChalanInfo")`; `ChalanNo = ChalanNoGenerator(id)` | BLL :20-22 | | | `@ChalanId`, `@ChalanNo` | ChalanId, ChalanNo |
| truck/driver | `TrackNo`, `DriverName` | :174-175 | | | | TrackNo, DriverName |
| from/to code, name, address | `FromComUnit*`, `ToComUnit*` | :176-182 | | | | FromComUnitCode/Name/Address, ToComUnitCode/Name/Address |
| fromCunithiddenValue | `fromunitid` | :179 | | | `@FromComUnitId` | FromComUnitId |
| manufacDropDownList | `ManufacId` | :183 | | | | ManufacId |
| grid totals | `TotalValue`, `TotalVat`, `GrandTotal` | :185-206 | | | | TotalValue, TotalVat, GrandTotal |
| (const false) | `IsDeliver` | DAL :38 | | | `@IsDeliver=false` | IsDeliver (column stores 'True'/'False' text — `B2BTransferViewDal`) |
| chalanGridView DataKeys[0] | `ChalanDetail.DCStoreId` | :226-227 | `SaveDataForChalanDetail` :96-109 | `SaveDataForChalanDetail` :43-61 | `@DCStoreId` | tblChalanDetail.DCStoreId |
| row qty/batch/name/code | Quantity, BatchNo, ProductName, ProductCode | :233-242 | | | | tblChalanDetail.* |
| UnitPrice / VAT | `UnitPrice`, `Value=qty×UP`, `Vat=qty×VATperUnit`, `ValueWVat` | :243-246 | | | | UnitPrice, Value, Vat, ValueWVat |
| BLL per detail | `ChalanDetailId = PrimaryKeyMax("ChalanDetailsId","tblChalanDetail")` | BLL :100 | | | | ChalanDetailsId |
| stock issue | `UpdateDCStockQuantity(DCStoreId, qty, ChalanDetailsId)` | BLL :103-106 | | `ChalanDAL.cs:513-553` | `@IssueQty,@DCStoreId,@ChalanDetailsId` | tblDCStore (Issue row, see §17) |

## 9. Dropdown Mapping
`manufacDropDownList` ← `ChalanBLL.LoadManufac` → `SELECT * FROM dbo.tblManufacturer` (Text ManufacName, Value ManufacId). No DC dropdowns — units are typed codes (any company unit, no user scoping).

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| Show | `Button1_Click` :315-327 | binds `productGridView` or alert "No Product Found!!" |
| Add | `addButton_Click` :342-412 | appends ticked rows with qty (skips DCStoreId already in list via `HasDCStoreId`; **note:** existing chalan rows are re-appended from the grid) |
| delete image | `DeleteImageButton_Click` :413-462 | removes row, recalculates total |
| Save | `submitButton_Click1` :164-264 | validation → master → details → issue rows; alert "Data saved successfully!" |
| Print | `Button2_Click` | viewer popup |
| Cancel/Back | redirects | |
| transfer qty edit | `dQtyTextBox_TextChanged` :470-487 | if qty > row stock (`Cells[3]`) alert "Transfer Qty. can't be more than Stock Quantity (n)" and blank |

## 11. Frontend Flow
Page_Load (first): `ChalanId()`, `Todate()`, `LoadManufac`, `PermissionForShowBtn()`. Type from-unit code (postback fills name/address/id), to-unit code, product code → Show → tick batches/enter qty (each edit posts back and is checked against displayed stock) → Add → repeat for other products → Save → print. No client script beyond controls.

## 12. Backend Flow
`submitButton_Click1` (try/catch → alert "Error saving data: <message>"):
1. `Validation()`.
2. Totals: per row `qty × VATAmountPerUnit`, `qty × UnitPrice` (values from grid DataKeys = active `tblUnitPrice`).
3. `SaveDataForChalanInfo` → `tblChalanInfo` insert (IsDeliver false, FromComUnitId), `ChalanNo` regenerated from the new id.
4. `LoadChalanById` to show the printable number.
5. `SaveDataForChalanDetail(list)`: for each: insert `tblChalanDetail`; if `DCStoreId>0 && Quantity>0` → `ChalanDAL.UpdateDCStockQuantity` inserts the **issue ledger row** (below). No transaction.

## 13. API Mapping
None.

## 14. Service Mapping
`ChalanBLL`: `LoadManufac`, `ComUnit`, `GetProductDcStore`, `ChalanNoForShow`, `SaveDataForChalanInfo`, `SaveDataForChalanDetail`, `LoadChalanById`.

## 15. Repository Mapping
| DAL | SQL (parameterised) |
|---|---|
| `GetProductDcStore` :440-490 | `SELECT MIN(DS.DCStoreId) DCStoreId, P.ProductCode, P.ProductName, DS.BatchNo, DS.ExpDate, MIN(ReceiveDate), UP.UnitPrice, VATAmountPerUnit, VATPercentage, SUM(DS.StockQty) StockQty FROM tblDCStore DS ⟕ tblProduct ⟕ tblCompanyUnit CU ⟕ tblUnitPrice UP(IsActive=1) WHERE CU.ComUnitCode=@ComUnitCode AND DS.ProductCode=@ProductCode AND DS.Trans_Type IN ('Receive','Issue') GROUP BY … HAVING SUM(StockQty)>0` |
| `SaveDataForChalanInfo/Detail` | INSERT tblChalanInfo / tblChalanDetail |
| `UpdateDCStockQuantity` :513-553 | `INSERT INTO tblDCStore … SELECT TOP 1 … FROM tblDCStore DS ⟕ tblChalanDetail CD ⟕ tblChalanInfo CI WHERE DS.DCStoreId=@DCStoreId` (issue row) |

## 16. Stored Procedure Mapping
Page itself calls **no SP**. Related SP bodies (in repo) that operate on the challan it creates:
| SP | Script | Effect |
|---|---|---|
| `sp_Del_B2BDelete(@chalanId)` | `SP_NewStockTransferDcToDc_Related.sql:23-61` (**v1, 2026-09-01**) | cursor over `tblChalanDetail(ChalanId)`; for each: `tblDCStore.StockQty = StockQty + Quantity` on the source row (`DCStoreId`); then delete `tblChalanDetail`, `tblChalanInfo`. **No transaction; never removes Issue/Receive ledger rows.** |
| `sp_Del_B2BDelete(@chalanId)` | `SP_Fix_STO_Chalan_Process.sql:25-64` (**v2, 2026-09-02**, both scripts DROP+CREATE) | in TRY/transaction: DELETE `tblDCStore` rows `Trans_Type='Issue'` matching the challan's `ChalanDetailsId` **or** `ChalanNo`; DELETE `tblDCStore` rows `Trans_Type='Receive' AND StockCondition='B2BReceive'` likewise; delete details and master; ROLLBACK + `RAISERROR` on error. **Spec conflict — see GAP-M11-001.** |
| `sp_DeleteDwpotToWhChalan(@masterId)` | :72-79 | `DELETE tblDepotToWHChalanInfo WHERE SChalanId`; `DELETE tblDepotToWHChalanDetail WHERE SChalanId` (DC→WH, PAGE-409) |
| `sp_SAP_B2BChallanMasterInsert(@ChallanNo)` | :182-265 | reads `SAP_API_Data..tblSAP_StockMovementMaster/Detail` (matched to `tblCompanyUnit.SAP_Code` for from/to plant, `tblProduct.SAP_Code` = `SUBSTRING(product_code,13,…)`, active `tblUnitPrice`); excludes challans already in `tblWHStockInMaster.ChallanNo`; for each group with `SUM(quantity)>0` inserts `tblChalanInfo(ChalanId=MAX+1, ChalanDate=GETDATE(), ChalanNo, TrackNo='', DriverName='', From/To unit code/name/address, TotalValue=Σq×ISNULL(UnitPrice,1), TotalVat=Σq×ISNULL(VATAmountPerUnit,1), GrandTotal, ManufacId=1, IsDeliver='False', FromComUnitId, Note='From SAP')`; `RETURN @ReceiveIdMAX` (new ChalanId) |
| `sp_SAP_B2BChallanDetailInsert(@MasterId,@ChallanNo)` | :276-370 | for each SAP detail line (qty>0, active price): FIFO by `ExpDate` allocates `tblDCStore` rows of the **source DC** with `StockQty>0` — **`UPDATE tblDCStore SET StockQty = StockQty − alloc`** (decrement model, no Issue row); inserts `tblChalanDetail(ChalanDetailsId=MAX+1, ProductCode, ProductName, Quantity=@Qty (full line qty), BatchNo, UnitPrice, Value, Vat, ValueWVat, ChalanId, DCStoreId)` per allocation step. **Defects:** detail Quantity is the full SAP quantity every step; `@StockQuantity` is declared outside the inner loop so after stock runs out the stale value keeps the branch alive and phantom detail rows with `DCStoreId=0` are inserted until `@AdjustmentQuantity` ≤ 0; no shortage error |
| `sp_SAP_B2BStockReceiveByDc(@MasterId)` | :90-171 | for each `tblChalanDetail(ChalanId)`: INSERT `tblDCStore(DCStoreId=MAX+1, StorageLocation='', ProductCode, ProductName, PackSize, BatchNo, TotalQuantity=Quantity, ExpDate/MfgDate/ReceiveDate from the source DCStore row, ChalanNo, ChalanDate, ComUnitId=(tblCompanyUnit where ComUnitCode=ToComUnitCode), StockQty=Quantity, DamageQty=0, StockRcvDate=GETDATE(), StockCondition='Available', ChalanDetailsId, Trans_Type='Receive')` — no CostPerUnit/TotalCostAmount/ReferenceDCStoreId; does **not** set `tblChalanInfo.IsDeliver`; `RETURN` last id |
| `sp_SAP_ChallanSendDetailByChalanId(@ChalanId)` | :381-398 | returns SAP payload rows: `ChalanId, product_code (=tblProduct.SAP_Code), batch_no, quantity, UoM (=tblStockUOM.UOMSAPCode), remarks=''` |
| `sp_SAP_Up_SAP_ChallanSendByChalanId` | :409-417 | `tblChalanInfo.SAP_ChallanSend = 1` |
| `sp_SAP_Up_SAP_ChallanConfirmByChalanId` | :428-436 | `tblChalanInfo.SAP_Challan_ConfirmationSend = 1` |

## 17. Database Mapping
- **tblChalanInfo** (W): ChalanId, ChalanDate, ChalanNo, TrackNo, DriverName, FromComUnitCode/Name/Address, ToComUnitCode/Name/Address, TotalValue, TotalVat, GrandTotal, ManufacId, IsDeliver ('False' at send; 'True' set by receive), FromComUnitId, Note ('From SAP' for SAP-origin), SAP_ChallanSend, SAP_Challan_ConfirmationSend (SAP flags; DDL not in repo).
- **tblChalanDetail** (W): ChalanDetailsId, ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, ChalanId, DCStoreId.
- **tblDCStore — stock effect at SEND (per detail):** one new row: `DCStoreId=MAX+1`, StorageLocation/ProductCode/ProductName/PackSize/BatchNo copied from the source row, `TotalQuantity=0`, ExpDate, MfgDate, ReceiveDate=GETDATE(), ChalanNo=challan no, ChalanDate, ComUnitId (source), **`StockQty = −qty`**, DamageQty 0, StockRcvDate, ReqId/ReqChildId copied, `StockCondition='B2BIssue'`, `Trans_Type='Issue'`, `CostPerUnit`=source cost (0 if NULL), `TotalCostAmount=−qty×cost`, `ReferenceDCStoreId=@DCStoreId` (the **MIN(DCStoreId)** of the batch/expiry group shown in the grid), ChalanDetailsId. No Receive row is touched.
- **tblDCStore — stock effect at RECEIVE** (`ChalanDAL.DCStockInDALMain` :122-170, triggered by PAGE-423): new `Trans_Type='Receive'`, `StockCondition='B2BReceive'` row at the **destination** unit with `TotalQuantity=StockQty=received qty`, cost from source row or active `tblUnitPrice.CostPrice`, `ReferenceDCStoreId=source DCStoreId`, `ChalanDetailsId`.
- R: tblProduct, tblCompanyUnit, tblUnitPrice, tblManufacturer.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| chalan no non-empty | `Validation` :127-131 | Please Insert chalanNo!! |
| date non-empty | :132-136 | Please Insert chalanDate!! |
| manufacturer selected | :137-141 | Please Select Manufacturer!! |
| from unit code non-empty | :142-146 | Please Select fromComUnit!! |
| to unit code non-empty | :147-151 | Please Select toComUnit!! |
| ≥1 chalan row | :157-161 | Please Add Product!! |
| unit code not found | `fromComUnitCodeTextBox_TextChanged` :284-290 | Company Unit Information Not Found!! (clears) |
| transfer qty > stock | `dQtyTextBox_TextChanged` :481-486 (postback only) | Transfer Qty. can't be more than Stock Quantity (n) |
| Not validated | | from ≠ to; qty >0 at save (rows with blank qty are skipped at Add, but qty 0 allowed); stock re-check at save (race: two users issue the same batch); numeric parse (TryParse → 0 silently); date format |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M11-001 | Sender and receiver DCs are identified by **typed unit code** resolved in `tblCompanyUnit`; unknown code is rejected and cleared; no rule stops from = to and no user/DC scoping applies. | NewStockTransferDcToDc.aspx.cs:271-313,142-151 | code-behind | CONFIRMED |
| BR-M11-002 | A challan requires number, date, manufacturer, from unit, to unit and ≥1 product line. | .aspx.cs:125-163 | code-behind | CONFIRMED |
| BR-M11-003 | Available stock per batch/expiry = `SUM(StockQty)` of `tblDCStore` rows (Trans_Type Receive+Issue) of the from-unit and product, shown only if > 0; unit price/VAT come from the active `tblUnitPrice`. | ChalanDAL.cs:440-490 | DAL | CONFIRMED |
| BR-M11-004 | Transfer quantity must not exceed displayed stock — enforced only when the qty box posts back; **no server-side re-check at Save**. | .aspx.cs:470-487,164-264 | code-behind | CONFIRMED |
| BR-M11-005 | The same source DCStoreId cannot be added twice (list de-duplication by `DCStoreId`). | .aspx.cs:329-341,368 | code-behind | CONFIRMED |
| BR-M11-006 | Challan number = `IDT` + id (`MAX(ChalanId)+1`), left-padded with zeros to 6 digits only when the id has 1-3 digits; ids ≥ 1000 are unpadded. | ChalanBLL.cs:126-151 | BLL | CONFIRMED |
| BR-M11-007 | Challan value = Σ qty × active unit price; VAT = Σ qty × VAT/unit; grand = value + VAT (per detail `Value`, `Vat`, `ValueWVat`). | .aspx.cs:185-247 | code-behind | CONFIRMED |
| BR-M11-008 | **Stock leaves the sender at save time**: each detail with qty>0 inserts a negative `Trans_Type='Issue'`, `StockCondition='B2BIssue'` ledger row referencing the source row (`ReferenceDCStoreId`) and the challan detail; the goods are "in transit" until received (`tblChalanInfo.IsDeliver='False'`). | ChalanBLL.cs:103-106; ChalanDAL.cs:513-553 | BLL + inline SQL | CONFIRMED |
| BR-M11-009 | Receiving (PAGE-423) inserts a destination `Receive` row (`B2BReceive`) for the **received** quantity and flips `IsDeliver` to 'True'; the quantity difference stays as a restricted `tblDCStoreFreeze` row for damage. | ChalanDAL.cs:62-69,122-170; TransferReceiveProductByChalanByDC.aspx.cs:59-128 | BLL/DAL | CONFIRMED |
| BR-M11-010 | Deleting a B2B challan from `B2BTransferView` is allowed only when its status text is not "Received" (status = `IsDeliver='True'`); the page deletes `tblChalanInfo` first, then calls `sp_Del_B2BDelete`. | B2BTransferView.aspx.cs:108-140; B2BTransferViewDal.cs:18-51 | code-behind + DAL | CONFIRMED |
| BR-M11-011 | **`sp_Del_B2BDelete` exists in two conflicting definitions**: v1 (2026-09-01) adds `Quantity` back onto the source Receive row and leaves Issue rows; v2 (2026-09-02, "Fix STO Chalan Process") deletes the Issue (and B2BReceive) ledger rows transactionally. The live behaviour depends on which script ran last; with the ledger model of BR-M11-008 only v2 is consistent (v1 double-counts stock: Receive.StockQty inflated **and** Issue row kept). | SP_NewStockTransferDcToDc_Related.sql:23-61; SP_Fix_STO_Chalan_Process.sql:25-64 | SP | CONFIRMED (deployed version UNKNOWN) |
| BR-M11-012 | SAP-origin challans: `sp_SAP_B2BChallanMasterInsert` builds a challan header from SAP stock movement (plants mapped by `tblCompanyUnit.SAP_Code`, products by `tblProduct.SAP_Code`), skipping challans already in `tblWHStockInMaster`; missing unit price/VAT default to **1**. | SP_NewStockTransferDcToDc_Related.sql:182-265 | SP | CONFIRMED |
| BR-M11-013 | `sp_SAP_B2BChallanDetailInsert` consumes source DC stock FIFO by expiry using the **decrement model** (`StockQty -=`), not Issue rows; detail quantity = full SAP line qty per allocation step; stale-variable defect creates phantom details when stock is short (GAP-M11-004). | SP_NewStockTransferDcToDc_Related.sql:276-370 | SP | CONFIRMED |
| BR-M11-014 | SAP hand-off states on the challan: `SAP_ChallanSend` (challan payload sent: `sp_SAP_ChallanSendDetailByChalanId` supplies SAP product code, batch, qty, UoM) and `SAP_Challan_ConfirmationSend` (receipt confirmation sent) are set to 1 by the two `sp_SAP_Up_*` procedures; no repo code calls them. | :381-436 | SP | CONFIRMED (callers UNKNOWN) |

## 20. Permission Matrix
Only `GetPermissionForShowBtn(path)`: if the permission SP returns rows for this page the Save button is hidden (inverse "deny" table; `CommonDataLoad`, not read) — exceptions swallowed. No role/DC check; any user can transfer from any unit code. `Session` not used otherwise.

## 21. Status Lifecycle
`tblChalanInfo.IsDeliver`: `'False'` (created/in transit) → `'True'` (received; set by `ChalanUpdate` at PAGE-423). View status text: Pending / Received (`B2BTransferViewDal.cs:20-22`). SAP flags `SAP_ChallanSend` 0→1, `SAP_Challan_ConfirmationSend` 0→1 (external). Stock ledger: Issue row created at send (B2BIssue), Receive row at receipt (B2BReceive).

## 22. Error Handling
try/catch around Save → `showMessageBox("Error saving data: " + ex.Message)`; `TryParse` silently yields 0. No transaction: if detail 3 of 5 fails, master and earlier issue rows remain. `PermissionForShowBtn` swallows exceptions.

## 23. Audit
No created-by/updated-by on `tblChalanInfo` insert; driver/truck only. Issue rows record `ReferenceTransactionDate=GETDATE()`. No log of deletions except the SP effect.

## 24. Integration
SAP: challan payload SPs and flags (§16); SAP-origin challan creation from `SAP_API_Data..tblSAP_StockMovement*`; SAP dashboard (`SAPDashboard.aspx`, `sp_RPT_ChallanStatusByDate` — columns `SAP_ChallanSend`, `SAP_ChallanRecevie`). Cross-database read of `SAP_API_Data`.

## 25. Reports
`DcToDcChalanReportViewer.aspx` (RPT-M11-01), `TransferStockReceiveReportViewer.aspx` (RPT-M11-01).

## 26. Dependencies
`tblUnitPrice`, `tblProduct`, `tblCompanyUnit`, `tblManufacturer`; `SAP_API_Data`; PAGE-423 (receive), PAGE-407 (list/delete).

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | From=DC-A code, to=DC-B code, product with 100 in batch B1, transfer 30, Save | tblChalanInfo IsDeliver False; detail; Issue row −30 (B2BIssue) at DC-A; DC-A available 70 | 003,007,008 |
| T2 | Negative | Unknown to-unit code | alert, cleared | 001 |
| T3 | Negative | Qty 200 > stock 100 (post back) | alert, qty cleared | 004 |
| T4 | Edge | Same product twice from same DCStoreId | second ignored | 005 |
| T5 | Edge | from = to | accepted (no rule) | 001 |
| T6 | Edge | Two users transfer same batch concurrently | both succeed; available goes negative | 004 |
| T7 | Positive | Receive at PAGE-423 with 5 damaged of 30 | DC-B Receive row 25 B2BReceive; freeze 5; IsDeliver True | 009 |
| T8 | Positive | Delete pending challan (v2 SP) | Issue rows removed, stock restored | 010,011 |
| T9 | Negative | Delete with v1 SP | Receive.StockQty inflated and Issue kept → double stock | 011 |
| T10 | Negative | SAP detail insert with less stock than SAP qty | phantom detail rows DCStoreId=0 | 013 |

## 28. Reverse Engineering References
`NewStockTransferDcToDc.aspx.cs`; `ChalanBLL.cs`; `ChalanDAL.cs:17-170,440-553`; `B2BTransferViewDal.cs`; `SP_NewStockTransferDcToDc_Related.sql`; `SP_Fix_STO_Chalan_Process.sql`.

## 29. Spec vs Actual
| Item | Specification | Actual |
|---|---|---|
| All | None existed | as above |
| SP header of v2 | "Cleanly deletes a B2B Transfer Challan, removing any Issue and Receive ledger rows" | matches v2; contradicts v1 header "restores issued stock back to tblDCStore" |

## 30. Known Gaps
No transaction; no server-side stock check; `sp_Del_B2BDelete` versions; page deletes master before SP (v2 `ChalanNo IN (SELECT … tblChalanInfo)` then finds nothing, relying on `ChalanDetailsId`); MIN(DCStoreId) reference row can differ from the row whose batch actually holds stock; unit-price join to active `tblUnitPrice` duplicates rows if several active prices; SAP SP defects; challan numbering by MAX+1 (race); no from≠to rule; no audit user.

## 31. Change Impact
`ChalanBLL.SaveDataForChalanDetail`/`ChalanDAL.UpdateDCStockQuantity` are used by the DC→DC pages (412, 419); `DCStockInDALMain` by PAGE-423 and sub-depot receive; `sp_Del_B2BDelete` by `B2BTransferViewDal`. The Issue/Receive ledger contract is shared with invoice issue (M07), `sp_Delete_ProformaInvoice`, `sp_Adjust_DCStore_Stock_Only` and `sp_Check_DCStore_StockAnomalies`.
