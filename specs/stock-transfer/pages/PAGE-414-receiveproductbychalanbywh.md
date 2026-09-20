---
page_id: PAGE-414
module: M11
page: ReceiveProductByChalanByWh
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-414 — ReceiveProductByChalanByWh (warehouse receives a DC → WH return challan)

> **Evidence level:** Tier A — `.aspx.cs` (312 lines) read in full, control/DataKey inventory, `StockRcvByWhDal.cs` in full, `WhStockConditionFreezeDal.SaveforWh/SaveWhStoreFreeze`, `SCtoWHTransferDal.DCStoreWIthTransfer/GetRcvId`. Opened from `StockRcvByWH.aspx` (`?ReqId=<SChalanId>`). No SP bodies involved (inline SQL).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-414 |
| Module | M11 |
| Name / class | ReceiveProductByChalanByWh / `SInventory_UI_ReceiveProductByChalanByWh` (`.aspx.cs:12`) |
| Kind | Entry / transaction — stock in at central store or WH freeze store |
| Size | code-behind 312 lines, aspx 254 lines |

## 2. Purpose
CONFIRMED (labels "UnRcvQty", Purpose/UnitPrice/VatPerUnit columns): the warehouse confirms receipt of a challan sent by a DC (PAGE-408/409) and books the goods into central stock (`Available`, `Regular`) or into WH freeze stock when the line came from freeze stock or is short/damaged.

## 3. Business Objective
INFERRED: return depot stock to the central warehouse with correct batch, price and VAT, quarantine problem quantities, and close the DC→WH challan.

## 4. Navigation
Inbound `StockRcvByWH.aspx`; outbound `StockRcvByWH.aspx` (back).

## 5. Route
`~/SInventory_UI/ReceiveProductByChalanByWh.aspx?ReqId=<SChalanId>`.

## 6. Source Files
View/code-behind `Solution.Web/SInventory_UI/ReceiveProductByChalanByWh.aspx(.cs)`; DAL `StockRcvByWhDal.cs` (`LoadMasterInfo` :16-25, `GetChallanDetailByReqId` :27-45, `CentralStorStockIn` :47-77, `updateChallanStatus` :79-87), `SCtoWHTransferDal.cs` (`DCStoreWIthTransfer` :197-202, `DCStoreCentralStore` :213-219, `GetRcvId` :221-234), `WhStockConditionFreezeDal.cs` :48-61 + `WhStockConditionFreezeBll`.

## 7. UI Control Inventory
| Control | Type | Required | Default / state | Notes |
|---|---|---|---|---|
| clnNoTextBox, clnDateTextBox, rcvDateTextBox, truckTextBox, driverNameTextBox | TextBox RO | — | challan header (`LoadMasterInfo`), receive date = today | header via `tblDepotToWHChalanInfo ⨝ tblCompanyUnit(FromComUnitCode)` |
| hdComUnitId, hdReqId | HiddenField | — | sending DC id, SChalanId | |
| rcvGridView `DataKeyNames=SChalanId,SChalanDetailsId,DCStoreFreezeId,DCStoreId,ProductId,StockConditionId` | GridView | rows | `GetChallanDetailByReqId` (`IsDeliver = false`) | cols: ProductCode, Name, PackSize, BatchNo, Quantity, MfgDate, ExpDate, ReceiveDate, rcvQty (`rcvQtyTextBox`), damage (`damageTextBox`), UnitPrice, VatPerUnit, Purpose, TotalPrice, TotalVat, TotalAmount |
| damageTextBox "UnRcvQty" | TextBox (AutoPostBack) | Yes (non-empty) | — | `damageTextBox_TextChanged` bounds vs issued qty |
| rcvQtyTextBox | TextBox | not validated | — | `rcvQtyTextBox_TextChanged` sets damage = issued − received |
| saveButton (`submitButton_Click`) | Button + `confirm('Are you sure you want to Save ?')` | — | — | `Save()` |
| backLinkButton | LinkButton | — | — | |

## 8. Field Mapping
| UI | Variable | Handler | DAL | SQL | Table.column |
|---|---|---|---|---|---|
| grid cells | `CentralStoreDao` ProductCode, ProductName, PackSize, BatchNo | `Save` :157-163 | `StockRcvByWhDal.CentralStorStockIn` :47-77 | INSERT | tblCentralStore.ProductCode, ProductName, PackSize, BatchNo |
| rcvQty | `Quantity`, `StockInQty` | :164-165 | | | Quantity, StockInQty |
| Mfg/Exp | `MfgDate` (cell 5), `ExpDate` (cell 6) | :166-167 | | | MfgDate, ExpDate |
| now | `ReceiveDate = today` | :168 | | | ReceiveDate |
| header | `ChalanNo`, `ChalanDate` | :169-170 | | | ChalanNo, ChalanDate |
| UnitPrice / VatPerUnit | `UnitPrice`, `VATPerUnit` (cells 10/11) | :171-172 | | | UnitPrice, VATPerUnit |
| **issued qty** × price/VAT | `TotalPrice`, `TotalVAT`, `TotalAmount` (cell 4 = issued Quantity) | :173-180 | | | TotalPrice, TotalVAT, TotalAmount |
| DataKeys[1] | `MigoDetailID` = **SChalanDetailsId** | :181 | | | tblCentralStore.MigoDetailID |
| DataKeys[3], [2] | `DCStoreId`, `DCStoreFreezeId` | :182-183 | | | DCStoreId, DCStoreFreezeId |
| const | `StockCondition='Available'`, `ProductStockType='Regular'` | :184-185 | | | StockCondition, ProductStockType |
| freeze branch | `WhStockConditionFreezeDao(ReceiveId, FreezeQty, EntryBy=LoginName, EntryDate=Now)` → `tblWhStockConditionFreeze`; `WhStoreFreezeDao` → `tblWhStoreFreeze` (StockCondition 'Restricted') | :197-236,245-284 | `WhStockConditionFreezeDal` :48-61 | INSERT (concatenated) | tblWhStockConditionFreeze, tblWhStoreFreeze |
| completion | `updateChallanStatus(SChalanId)` | :288-291 | :79-87 | `UPDATE tblDepotToWHChalanInfo SET IsDeliver=@IsDeliver('OK')` | tblDepotToWHChalanInfo.IsDeliver |

## 9. Dropdown Mapping
None.

## 10. Button Action Mapping
Save → `Save()`; damage/rcv textboxes → server recalculation (`damage = issued − rcv`; damage > issued → alert "Quantity Must be equal or less then Issue Qty.").

## 11. Frontend Flow
First load: `LoadInfo` + `LoadGrid` (alert "Stock Already Received !!" if no pending rows) → user reviews per line received vs unreceived → Save → alert "Data Save Successfully!!", form cleared.

## 12. Backend Flow
`Save` (:139-298): `Validation()`; per row: read `stockConditionId`, `dcstorreId`, `freezeaId`.
1. **Branch A** — `rcvQty>0 && dcstorreId>0 && freezeaId==0`: build `CentralStoreDao` and `CentralStorStockIn` → **new `tblCentralStore` row** (Quantity = received) → `DCStoreId=true`.
2. **Branch B** — otherwise (line from freeze stock, or no DC store id): if `rcvQty>0`: insert `tblWhStockConditionFreeze` (`ReceiveId = DataKeys[1]` i.e. SChalanDetailsId!) and resolve the origin central `ReceiveId` via `DCStoreWIthTransfer` → (`tblDCStore ⟕ tblStockInTransfar` on `StockInTransfarId`) or recursively `GetRcvId` through `tblChalanDetail` chain; insert `tblWhStoreFreeze` (Restricted, StockQty=rcvQty).
3. If `damage>0` (either branch): another `tblWhStockConditionFreeze` + `tblWhStoreFreeze` (Restricted) for the damaged quantity.
4. If any central-store insert succeeded (`DCStoreId`) or a freeze id > 0: `updateChallanStatus(DataKeys[0][0])` → `IsDeliver='OK'` for the whole challan (based on **row 0's** SChalanId).
No transaction; no try/catch.

## 13. API Mapping
None.

## 14. Service Mapping
`StockRcvByWhDal`, `SCtoWHTransferDal`, `WhStockConditionFreezeBll` (`SaveforWh`, `SaveWhStoreFreeze`) used directly from the page.

## 15. Repository Mapping
`GetChallanDetailByReqId`: `SELECT PD.ProductId, CLND.SChalanDetailsId, CLND.SChalanId, CLND.DCStoreFreezeId, CLND.DCStoreId, CLND.ProductCode, CLND.ProductName, DS.PackSize, P.Purpose, ISNULL(P.StockConditionId,0), ST.UnitPrice, CAST(ST.VATAmount/ST.Quantity AS decimal(10,2)) VatPerUnit, CLND.Quantity*ST.UnitPrice TotalPrice, …, CLND.BatchNo, CLND.Quantity, DS.ExpDate, DS.ReceiveDate, DS.MfgDate FROM tblDepotToWHChalanDetail CLND ⨝ tblDepotToWHChalanInfo CLN ⨝ tblProduct PD ⨝ tblDCStore DS ⟕ tblStockInTransfar ST (on DS.StockInTransfarId) ⟕ tblPurpose P WHERE CLN.IsDeliver=@IsDeliver(false) AND CLND.SChalanId=@SChalanId` (parameterised). Price/VAT therefore come from the **original STO transfer line** of the DC row (`ST.UnitPrice`, `VATAmount/Quantity`), not from the current price list; a DC row without `StockInTransfarId` (e.g. B2B-received or returned stock) yields NULL prices → `Convert.ToDecimal("")`-style failures on the page.

## 16. Stored Procedure Mapping
None.

## 17. Database Mapping
- **tblCentralStore** (W, Branch A): ProductId, ProductCode, ProductName, PackSize, BatchNo, Quantity, MfgDate, ExpDate, ReceiveDate, ChalanNo, ChalanDate, StockInQty, UnitPrice, TotalPrice, VATPerUnit, TotalVAT, TotalAmount, StockCondition, MigoDetailID, ProductStockType, DCStoreFreezeId, DCStoreId. **TotalPrice/TotalVAT/TotalAmount use the issued quantity**, Quantity uses the received quantity.
- **tblWhStockConditionFreeze / tblWhStoreFreeze** (W, Branch B and damage): freeze headers/lines.
- **tblDepotToWHChalanInfo** (W): `IsDeliver='OK'`.
- The **weighted-average / batch ledger** (`tbl_INV_CentralStore_WeightedAvg`, `tbl_INV_Central_BatchWiseStock`) is **not updated** by this page — central stock increases via `tblCentralStore` only (valuation ledger impact UNKNOWN/none).
- R: tblDepotToWHChalanInfo/Detail, tblDCStore, tblStockInTransfar, tblProduct, tblPurpose, tblChalanDetail.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| UnRcvQty non-empty on every row | `Validation` :122-136 | Please fill out UnRcvQty !! |
| damage > issued | `damageTextBox_TextChanged` :82-86 | Quantity Must be equal or less then Issue Qty. |
| no pending rows | `LoadGrid` :47-55 | Stock Already Received !! |
| Not validated | | received qty numeric/bounded; received + damage = issued; duplicate submit (IsDeliver flips only after success); user/warehouse authority |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M11-036 | Only challans with `IsDeliver = false` are receivable; `IsDeliver` becomes `'OK'` once at least one line was stocked in or frozen; the challan is then closed for all lines. | StockRcvByWhDal.cs:27-45,79-87; .aspx.cs:288-291 | DAL + code-behind | CONFIRMED |
| BR-M11-037 | **Stock effect (branch A):** a line sourced from normal DC stock (`DCStoreId>0`, no freeze id) is added to central stock as a new `tblCentralStore` row `Available/Regular` with `Quantity = received qty`; lines sourced from freeze stock are added only to WH **freeze** stock (`Restricted`). | .aspx.cs:154-190,191-237 | code-behind | CONFIRMED |
| BR-M11-038 | Unreceived/damaged quantity always goes to WH freeze stock (`tblWhStockConditionFreeze` + `tblWhStoreFreeze`, `Restricted`). | .aspx.cs:242-285 | code-behind | CONFIRMED |
| BR-M11-039 | Valuation: price and VAT per unit are those of the DC row's original STO line; totals are computed on the **issued** quantity, so `TotalPrice/TotalVAT/TotalAmount` overstate value when qty is short. | StockRcvByWhDal.cs:29-33; .aspx.cs:173-180 | SQL + code-behind | CONFIRMED |
| BR-M11-040 | `tblCentralStore.MigoDetailID` is populated with the **challan detail id** (`SChalanDetailsId`), and `tblWhStockConditionFreeze.ReceiveId` with the same id, not with a MIGO or central `ReceiveId` (the freeze store's `ReceiveId` is separately resolved by walking the DC row back to its central lot). | .aspx.cs:181,199,247 | code-behind | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]` used as EntryBy for freeze records (NullReference if absent). No role check; `StockRcvByWH.aspx` list is menu-controlled. Any user can open any `?ReqId`.

## 21. Status Lifecycle
`tblDepotToWHChalanInfo.IsDeliver`: 'False' → 'OK'. Central lots: `StockCondition='Available'` (A) / freeze rows `Restricted` (B). Whole-challan close on first success (partial receipt of remaining lines is impossible afterwards).

## 22. Error Handling
None; failures throw. A mid-loop failure leaves some lines stocked and `IsDeliver` unchanged → re-opening shows all lines again → **duplicate receipt of the already stocked lines** (no idempotency guard on `tblCentralStore` insert).

## 23. Audit
`tblWhStockConditionFreeze.EntryBy/EntryDate` only; central-store insert has no user column.

## 24. Integration
None (no SAP call on this page). Freeze/central store consumed by warehouse pages and reports.

## 25. Reports
None launched (popup commented). Challan print: `DcToWHChalanReportViewer` (RPT-M11-01).

## 26. Dependencies
PAGE-408/409 (source), `StockRcvByWH.aspx`, freeze release pages (`WhFreezeStockReleaseDal`, not analysed).

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Pending challan line from normal stock, rcv 10, damage 0 | new tblCentralStore row Quantity 10 Available; IsDeliver OK | 036,037 |
| T2 | Positive | rcv 8, damage 2 | central row 8 (TotalPrice on 10); freeze rows 2 Restricted | 037,038,039 |
| T3 | Positive | Line from DC freeze stock | only tblWhStoreFreeze row | 037 |
| T4 | Negative | damage blank | message | — |
| T5 | Edge | Failure after first line | re-open: first line receivable again → duplicate central row | GAP-M11-010 |
| T6 | Edge | DC row without StockInTransfarId | NULL price/VAT in grid → conversion error | 039 |
| T7 | Integration | Report joining `tblCentralStore.MigoDetailID` to `tblMIGODetail` (ChalanDAL.ChalanReport) | mismatched join for returned stock | 040 |

## 28. Reverse Engineering References
`ReceiveProductByChalanByWh.aspx.cs`; `StockRcvByWhDal.cs`; `SCtoWHTransferDal.cs:197-234`; `WhStockConditionFreezeDal.cs:48-61`.

## 29. Spec vs Actual
None existed.

## 30. Known Gaps
No transaction/idempotency; valuation on issued qty; `MigoDetailID` misuse; weighted-average ledger not updated for returned stock (INFERRED: central valuation reports may miss returns); `IsDeliver` bit vs text; concatenated SQL in freeze DAL; whole-challan close after first success; NullReference for null-price rows.

## 31. Change Impact
`tblCentralStore` rows written here feed STO picking (PAGE-420 chain) and warehouse stock reports; `IsDeliver='OK'` removes the challan from `StockRcvByWH` and `DepoToWhTransferView` behaviours; `WhStockConditionFreezeDal` shared with warehouse freeze/release pages.
