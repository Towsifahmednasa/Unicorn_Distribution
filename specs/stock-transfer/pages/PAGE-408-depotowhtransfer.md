---
page_id: PAGE-408
module: M11
page: DepoToWHTransfer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-408 — DepoToWHTransfer (DC → Warehouse return challan: create)

> **Evidence level:** Tier A — `DepoToWHTransfer.aspx.cs` read in full (649 lines), `SCtoWHTransferDal.cs` lines 1-250 (all methods this page and its companions use), `DepoToWhTransferView.aspx.cs` (the page that actually moves stock), `StockRcvByWhDal.cs` (receive side), control inventory. All SQL is inline (string-concatenated); no SP bodies exist except `sp_DeleteDwpotToWhChalan` (read).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-408 |
| Module | M11 |
| Name / class | DepoToWHTransfer / `SInventory_UI_DepoToWHTransfer` (`.aspx.cs:13`) |
| Kind | Entry / transaction (document only — stock is deducted later on PAGE-409) |
| Size | aspx 453 lines, code-behind 649 lines |

## 2. Purpose
CONFIRMED (class, labels "Freeze Stock", "Purpose", chalan prefix `SDC`): a DC user prepares a **challan to send stock back to the central warehouse** (return of saleable stock or of *freeze/restricted* stock), stating the purpose per line.

## 3. Business Objective
INFERRED: return surplus, near-expiry or quarantined (freeze) goods from a DC to a warehouse with a printable challan; the warehouse then receives them (PAGE-414) into central stock or its freeze store.

## 4. Navigation
Inbound: `DepoToWhTransferView.aspx`. Outbound: `DepoToWhTransferView.aspx` (list/view button), `DepoToWHTransfer.aspx` (cancel), `../SInventory_RPTVIEW/DcToWHChalanReportViewer.aspx?chalanno=` (print).

## 5. Route
`~/SInventory_UI/DepoToWHTransfer.aspx`.

## 6. Source Files
View/code-behind `Solution.Web/SInventory_UI/DepoToWHTransfer.aspx(.cs)`; DAL `Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs` (`GetProductDcStoreSubdeport` :71-84, `GetProductDcFreezeStore` :86-99, `SaveDataForChalanInfo` :101-106, `SaveDataForChalanDetail` :108-113, `LoadChalanById` :121-125, `CheckChalanStatus` :192-196, `LoadPurposeDropDownList` :186-190); companion `DepoToWhTransferView.aspx.cs`; `ClsPrimaryKeyFind`.

## 7. UI Control Inventory
| Control | Type / label | Required | Default / state | Notes |
|---|---|---|---|---|
| chalanNoTextBox | TextBox RO | Yes | `SDC` + padded(MAX(SChalanId)+1) | |
| chalanDateTextBox | TextBox RO | Yes | `DateTime.Today.ToShortDateString()` (culture format) | parsed with `Convert.ToDateTime` at save |
| salescenterDropDownList1 | DropDownList (AutoPostBack) | Yes (implicit) | first DC | `tblCompanyUnit` as "code:name" — all DCs, no user scope; selection fills readonly from-unit fields |
| fromComUnitCodeTextBox / Name / Address | TextBox RO | Yes | from selected DC | `LoadComUnit(code)`; "Company Unit Information Not Found!!" if missing |
| subdeportDropDownList2 | DropDownList | Yes | value 1 (forced) | `tblWearhouse` "code:name"; drives to-WH fields |
| toComUnitCodeTextBox / Name / Address | TextBox RO | Yes | from selected WH | `WareHouseInfoLoad(id)` ("WH Information Not Found!!") |
| manufacDropDownList | DropDownList | Yes | value 1 (forced) | `tblManufacturer` |
| truckNoTextBox, driverNameTextBox | TextBox | No | | |
| productDropDownList | DropDownList | Yes for search | all products "code:name" | |
| Button1 "Show" / Button3 "Freeze Stock" | search | — | — | `GetProductDcStoreSubdeport` / `GetProductDcFreezeStore` (`tblDCStoreFreeze` rows `StockQty>0`) |
| productGridView `DataKeyNames=DCStoreId,VATAmountPerUnit,UnitPrice,DCStoreFreeze…` | GridView | — | — | select, code, name, stock, batch, expiry, `transferQtyTextBox` (digits, AutoPostBack → `dQtyTextBox_TextChanged`) |
| addButton "Add" | Button | — | — | rebuilds `chalanGridView` including existing rows |
| chalanGridView | GridView | ≥1 row | — | code, name, qty, batch, expiry, `purposeDropDownList` (`tblPurpose`), hidden `hfPurposeId`, delete |
| Save (`submitButton_Click1`) | LinkButton + `sweetAlertConfirm_Submit` | — | — | |
| printChalanNoTextBox + Button2 | print | — | filled after save | |

## 8. Field Mapping
| UI | Variable | Handler | DAL | SQL (concatenated) | Table.column |
|---|---|---|---|---|---|
| chalanDate/No | `DepoToWHTransferDao.ChalanDate/ChalanNo` (`SDC`+id from `PrimaryKeyMax("SChalanId","tblDepotToWHChalanInfo")`) | `SaveDataForChalanInfo` :562-568 | `SCtoWHTransferDal.SaveDataForChalanInfo` :101-106 | INSERT | tblDepotToWHChalanInfo.SChalanId, ChalanDate, ChalanNo |
| truck/driver | TrackNo, DriverName | :411-412 | same | | TrackNo, DriverName |
| from unit | FromComUnitCode (upper), Name, Address | :412-414 | same | | FromComUnitCode/Name/Address |
| WH | WHCode (upper), WHName, WHAddress | :415-417 | same | | WHCode, WHName, WHAddress |
| manufacturer | ManufacId | :418 | same | | ManufacId |
| totals | TotalValue/TotalVat/GrandTotal | :420-441 | same | | TotalValue, TotalVat, GrandTotal |
| const false | IsDeliver | DAL | | `'False'` | IsDeliver |
| line DataKeys | `DCStoreId`, `DCStoreFreezeId` | :463-464 | `SaveDataForChalanDetail` :108-113 | INSERT | tblDepotToWHChalanDetail.DCStoreId, DCStoreFreezeId |
| line | ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, PurposeId, SChalanId, SChalanDetailsId (`PrimaryKeyMax`) | :462-486; :519-540 | same | | same names |

## 9. Dropdown Mapping
DC: `select ComUnitId, (ComUnitCode+':'+ComUnitName) from tblCompanyUnit`; WH: `select WearhouseId, (WearhouseCode+':'+WearhouseName) from tblWearhouse`; manufacturer: `tblManufacturer`; product: `SELECT (ProductCode+':'+ProductName)Pro,* FROM tblProduct` (Value ProductId); purpose: `SELECT PurposeId,Purpose FROM tblPurpose`.

## 10. Button Action Mapping
Show → `Button1_Click` (needs from code and product) else "Please select product & sales center!!"; Freeze Stock → `Button3_Click`; Add → `addButton_Click` :212-291; Delete → `DeleteImageButton_Click`; Save → `submitButton_Click1` :402-498 (`Validation` first); Print → `Button2_Click`; view/cancel redirects.

## 11. Frontend Flow
Load: chalan no, date, dropdowns (manufacturer and WH forced to id 1). Choose DC → from fields; choose product → Show (available DC stock rows) or Freeze Stock → tick + qty (postback check vs stock cell) → Add (purpose default 1, changeable) → Save → print.

## 12. Backend Flow
`submitButton_Click1`: `Validation()`; totals from chalan grid; `SaveDataForChalanInfo` (id, number, insert `IsDeliver='False'`); reload by `LoadChalanById` (**reads `tblSubDepotChalanInfo`**, so the printed number usually stays empty); per line insert `tblDepotToWHChalanDetail`. **Stock is not changed here** — the code that would decrement `tblDCStore`/`tblDCStoreFreeze` is commented out (`.aspx.cs:526-535`). No try/catch, no transaction.
Companion (PAGE-409, `TransferStock`): for each detail: if `DCStoreId != 0 && DCStoreFreezeId == 0` → `UPDATE tblDCStore SET StockQty = <current StockQty − Quantity>` on that Receive row; else `UPDATE tblDCStoreFreeze SET StockQty = current − Quantity`; if the **last** update succeeded → `UPDATE tblDepotToWHChalanInfo SET IsTransfer='OK'`; message "Stock Transfer Complete !!!". Pending challans can be deleted (`sp_DeleteDwpotToWhChalan`) — deletes info and detail rows, no stock restore needed because nothing was deducted yet.

## 13. API Mapping
None.

## 14. Service Mapping
No BLL: the page calls `SCtoWHTransferDal` and `ClsPrimaryKeyFind` directly.

## 15. Repository Mapping
`GetProductDcStoreSubdeport`: `SELECT 0 AS DCStoreFreezeId, VATAmountPerUnit, VATPercentage, UnitPrice, *, PCode, PName FROM tblDCStore ⟕ tblProduct ⟕ tblCompanyUnit ⟕ tblUnitPrice WHERE ComUnitCode='<unit>' AND tblProduct.ProductId='<id>' AND tblUnitPrice.IsActive='True' AND StockQty>0` — **concatenated**; shows Receive-row balances (`StockQty>0`) without netting negative Issue rows. `GetProductDcFreezeStore`: same over `tblDCStoreFreeze`. `CheckChalanStatus`: `SELECT * FROM tblDepotToWHChalanInfo WHERE IsTransfer IS NULL AND FromComUnitCode='<code>'`. Inserts are concatenated `VALUES (...)`.

## 16. Stored Procedure Mapping
`sp_DeleteDwpotToWhChalan(@masterId INT)` — `SP_NewStockTransferDcToDc_Related.sql:72-79`: `DELETE tblDepotToWHChalanInfo WHERE SChalanId=@masterId; DELETE tblDepotToWHChalanDetail WHERE SChalanId=@masterId` (no transaction). No other SP.

## 17. Database Mapping
- **tblDepotToWHChalanInfo** (W): SChalanId, ChalanDate, ChalanNo, TrackNo, DriverName, FromComUnitCode/Name/Address, WHCode/Name/Address, TotalValue, TotalVat, GrandTotal, ManufacId, IsDeliver ('False' → 'OK' at WH receive), IsTransfer (NULL → 'OK' at transfer).
- **tblDepotToWHChalanDetail** (W): SChalanDetailsId, ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, SChalanId, DCStoreId, DCStoreFreezeId, PurposeId.
- **Stock effect at transfer (PAGE-409):** in-place `tblDCStore.StockQty` (or `tblDCStoreFreeze.StockQty`) **decrement** of the chosen source row. At WH receipt (PAGE-414): new `tblCentralStore` row (or `tblWhStoreFreeze`) — see PAGE-414.
- R: tblDCStore, tblDCStoreFreeze, tblProduct, tblCompanyUnit, tblWearhouse, tblUnitPrice, tblPurpose, tblManufacturer.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| chalan no / date | `Validation` :357-366 | Please Insert chalanNo!! / chalanDate!! |
| manufacturer | :367-371 | Please Select Manufacturer!! |
| from / to code | :372-381 | Please Select fromComUnit!! / toComUnit!! |
| ≥1 line | :383-387 | Please Add Product!! |
| pending challan exists for the from-unit | :389-395 | Operation not possible because Challan exist for transfer!! |
| qty > stock (postback) | `dQtyTextBox_TextChanged` :580-611 | Transfer Qty. cantbe more then Stock Quantity |
| search without unit/product | :160-176 | Please select product & sales center!! |
| Not validated | | qty numeric/>0; stock at save or at transfer time (PAGE-409 does not use its own `QuantityText` check); same DCStoreId in two challans; SQL text |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M11-022 | Only **one open (untransferred) DC→WH challan per DC** at a time: a new one is refused while any challan of that from-unit has `IsTransfer IS NULL`. | DepoToWHTransfer.aspx.cs:389-395; SCtoWHTransferDal.cs:192-196 | code-behind + SQL | CONFIRMED |
| BR-M11-023 | Challan number `SDC`+id (`MAX(SChalanId)+1`; zero-padded to 6 only for 1-3 digit ids). | DepoToWHTransfer.aspx.cs:49-77 | code-behind | CONFIRMED |
| BR-M11-024 | Lines may come from normal DC stock (`tblDCStore`, `StockQty>0`) or freeze stock (`tblDCStoreFreeze`, `DCStoreFreezeId>0`); each line carries a purpose (`tblPurpose`, default 1). | .aspx.cs:157-179,212-291,614-633 | code-behind | CONFIRMED |
| BR-M11-025 | Challan value = Σ qty × unit price (active `tblUnitPrice`), VAT = Σ qty × VAT/unit. | .aspx.cs:420-441 | code-behind | CONFIRMED |
| BR-M11-026 | **Saving the challan does not reduce stock**; stock is decremented only when a user presses *Transfer* on PAGE-409 (`IsTransfer='OK'`): `StockQty := StockQty − Quantity` on the Receive row (or freeze row) — an in-place decrement, not an Issue ledger row. | .aspx.cs:526-535 (commented); DepoToWhTransferView.aspx.cs:57-90 | code-behind | CONFIRMED |
| BR-M11-027 | Transfer (PAGE-409) does not verify quantity ≤ current stock (result may be negative) and marks the challan transferred based on the **last** row's update result only. | DepoToWhTransferView.aspx.cs:65-89 | code-behind | CONFIRMED |
| BR-M11-028 | Warehouse and manufacturer are pre-forced to id 1 on load (defaults), still changeable. | .aspx.cs:33-42 | code-behind | CONFIRMED |

## 20. Permission Matrix
No role/session checks in the page (no `Session` use). Menu-based only. All DCs and warehouses selectable by anyone.

## 21. Status Lifecycle
`IsTransfer`: NULL (created) → 'OK' (stock transferred, PAGE-409). `IsDeliver`: 'False' (created) → 'OK' (received at WH, `StockRcvByWhDal.updateChallanStatus`). Note `GetChallanDetailByReqId` compares `IsDeliver = @IsDeliver` with a **bit** parameter `false` while the column also holds the text 'OK' / 'False' (type UNKNOWN; possible conversion error) — GAP-M11-009.

## 22. Error Handling
None (no try/catch); SQL errors surface as ASP.NET errors; `Convert.ToDecimal` on grid text throws for blank.

## 23. Audit
No user/date columns on the challan insert (only challan date). No delete audit.

## 24. Integration
None (no SAP). Challan print `DcToWHChalanReportViewer` (RPT-M11-01).

## 25. Reports
`DcToWHChalanReportViewer.aspx` → `crpDCToWhChalan.rpt` (data `SCtoWHTransferDal.ChalanReport`, RPT-M11-01).

## 26. Dependencies
PAGE-409 (transfer/delete), PAGE-414 (WH receive), `tblPurpose`, `tblWearhouse`.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | DC-A, WH1, product with stock 40, qty 10, purpose 1, Save | challan info/detail rows, IsTransfer NULL; **DC stock unchanged** | 026 |
| T2 | Positive | PAGE-409 Transfer | Receive row StockQty 30; IsTransfer 'OK' | 026 |
| T3 | Negative | Second challan for same DC before transfer | blocked message | 022 |
| T4 | Negative | qty > stock (postback) | alert | — |
| T5 | Edge | Transfer after other sales consumed stock | StockQty negative, no error | 027 |
| T6 | Edge | Print after save | printChalanNo empty (reads wrong table) | GAP-M11-008 |
| T7 | Security | Product/unit filters with quote characters | concatenated SQL breaks/injects | GAP-M11-005 |
| T8 | Edge | Stock for a batch with prior Issue rows | Show displays Receive row's positive `StockQty` ignoring Issue rows (overstated) | 024 |

## 28. Reverse Engineering References
`DepoToWHTransfer.aspx.cs`; `SCtoWHTransferDal.cs:19-250`; `DepoToWhTransferView.aspx.cs:25-121`; `StockRcvByWhDal.cs`.

## 29. Spec vs Actual
None existed.

## 30. Known Gaps
String-concatenated SQL throughout (`SCtoWHTransferDal`); no server stock check; stock deduction deferred and not transactional; `LoadChalanById` targets `tblSubDepotChalanInfo`; `IsDeliver` typed inconsistently; forced default ids; no audit user.

## 31. Change Impact
`SCtoWHTransferDal` is shared with PAGE-409, PAGE-414, PAGE-415 and the Sub Depot module; `tblDCStore` in-place decrement conflicts with ledger-based scripts (`sp_Adjust_DCStore_Stock_Only`, `sp_Check_DCStore_StockAnomalies`) — see GAP-M11-003.
