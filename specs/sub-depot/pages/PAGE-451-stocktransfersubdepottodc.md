---
page_id: PAGE-451
module: M12
page: StockTransferSubDepottoDc
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-451 — StockTransferSubDepottoDc

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-451 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | StockTransferSubDepottoDc |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/MainMasterPage.master |
| Page class | SubDepot_UI_StockTransferSubDepottoDc |
| aspx size | 21827 bytes |
| Code-behind LOC | 471 |
| Inline script size | 0 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**StockTransferSubDepottoDc** creates a return chalan that sends stock from sub-depot stores back to the DC. The user chooses the DC and its sub-depot, searches a product, ticks sub-depot batch rows and quantities, adds them to a list and submits. A return chalan (`SDC00000n`, table `tblSubDepotChalanReturnInfo`) is written, the sub-depot store quantity is reduced immediately and the chalan waits in transit for DC receipt (`SubDeportTransferStockReceiveByDC` -> `SubdeportTransferReceiveProductByChalanByDC`, WF-M12-02). CONFIRMED (`StockTransferSubDepottoDc.aspx.cs:116-203`, `SubDepotChalanReturnBLL.cs:79-90`). This page is the old-style layout (plain `Button`s, no breadcrumb caption).

## 3. Business Objective
Let a sub-depot return unsold/excess/expired stock to its parent DC with a chalan document. INFERRED from names and tables. The DC-side of the flow is where the stock is re-created (BR-M12-018).

## 4. Navigation
Menu placement DB-driven (`UNKNOWN`). Outbound: `DcToSubdepotChalanReportViewer.aspx` (Print, wrong viewer - BR-M12-048). Pending returns are received by `SubDeportTransferStockReceiveByDC.aspx` (PAGE-442) in `SInventory_UI`. Master page `MainMasterPage.master` (`StockTransferSubDepottoDc.aspx:1`).

## 5. Route
`~/SubDepot_UI/StockTransferSubDepottoDc.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx |
| Code-behind | Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | SubDepotChalanReturnBLL (Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | SubDepotChalanReturnDAL (Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs) |
| DAO/entity class | ChalanDetail (Library.DAO/SInventory_Entities/ChalanDetail.cs) |
| DAO/entity class | ChalanInfo (Library.DAO/SInventory_Entities/ChalanInfo.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Handler |
|---|---|---|---|---|---|---|
| chalanDateTextBox | (old layout, no label) | yes (server) | today | ReadOnly | - | - |
| chalanNoTextBox | - | yes (server) | next `SDC` no. from `tblSubDepotChalanReturnInfo` | ReadOnly | `SubDepotChalanReturnBLL.ChalanNoForShow` | - |
| salescenterDropDownList1 | DC | via code | - | - | `RequisitionBLL.DCLoad` all DCs | `..._SelectedIndexChanged` (:422) |
| subdeportDropDownList2 | Sub-depot | via code | - | - | `RequisitionBLL.SubdeportLoad` | (:448) |
| from/to `ComUnit*TextBox` | code/name/address | codes required | from selections | ReadOnly | `tblCompanyUnit`, `tblSubDepot` | TextChanged handlers (:210,:231) |
| truckNoTextBox, driverNameTextBox | - | no | - | - | - | - |
| manufacDropDownList | - | yes (server) | index 1 | - | `LoadManufac` | - |
| productDropDownList | - | - | - | - | `ProductLoad` | -> DC handler (BR-M12-047) |
| Button1 `Search Product` | - | - | - | - | `SubDepotChalanReturnBLL.GetProductDcStoreSubdeport(ProductId, toComUnitName)` | `Button1_Click` (:253) |
| productGridView | Product Code / Name / Stock Qty / Batch / ExpDate / ReceiveDate / Transfer Qty | - | - | DataKeys `SubDCStoreId,VATAmountPerUnit,UnitPrice` | tblSubDepotStore (all sub-depots, BR-M12-012) | - |
| transferQtyTextBox | Transfer Qty | non-empty to be added | - | digits only | - | `dQtyTextBox_TextChanged` |
| addButton `Add` | - | - | - | - | - | `addButton_Click` (:280) |
| chalanGridView + DeleteImageButton | line list / remove | >=1 row | - | DataKeys `SubDCStoreId,...` | - | `DeleteImageButton_Click` (:335) - fails (BR-M12-050) |
| submitButton `Submit` | - | - | - | **no JS confirm** | writes return chalan | `submitButton_Click1` (:116) |
| printChalanNoTextBox + Button2 `Print` | - | - | set after save | - | - | `Button2_Click` (:379) wrong viewer (BR-M12-048) |

## 8. Field Mapping
| UI control | Variable / DTO | BLL | DAL | Table.column |
|---|---|---|---|---|
| chalan date/no, truck, driver, from/to units, manufacturer | `ChalanInfo` fields as on PAGE-450 (:120-133) | `SubDepotChalanReturnBLL.SaveDataForChalanInfo` (:19) | `SubDepotChalanReturnDAL.SaveDataForChalanInfo` :16 | `tblSubDepotChalanReturnInfo.ChalanDate/ChalanNo/TrackNo/DriverName/FromComUnit*/SubDepotCode/SubDepotName/SubDepotAddress/ManufacId/IsDeliver(false)/SChalanId/TotalValue/TotalVat/GrandTotal` |
| chalanGridView row | `ChalanDetail`: **DCStoreId = DataKeys[i][0] which is `SubDCStoreId`**, code, name, qty, batch, unit price, value, vat | `SaveDataForChalanDetail` (:79) | `SubDepotChalanReturnDAL.SaveDataForChalanDetail` :24 | `tblSubDepotChalanRetuenDetail.DCStoreId` (holds SubDCStoreId - BR-M12-049), `ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, SChalanId, SChalanDetailsId` |
| Quantity | stock reduction | `SubDCInfoWithDCId` then `SubUpdateDCStockQuantity` | `SubDepotChalanReturnDAL.cs:145-149,316-320` | `tblSubDepotStore.StockQty` set to `current - qty` |
Table name `tblSubDepotChalanRetuenDetail` is spelt `Retuen` in code (DB name assumed identical - UNKNOWN without DDL).

## 9. Dropdown Mapping
Same four dropdowns as PAGE-450 (DC, sub-depot, product, manufacturer) - loaders `RequisitionBLL.DCLoad/SubdeportLoad/ProductLoad`, `SubDepotChalanReturnBLL.LoadManufac` (`StockTransferSubDepottoDc.aspx.cs:20-25,422-470`).

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL | DB change |
|---|---|---|---|---|
| Search Product | `Button1_Click` :253 | `SubDepotChalanReturnBLL.GetProductDcStoreSubdeport` -> `SubDepotChalanReturnDAL` :337 | `SELECT ... FROM tblSubDepotStore LEFT JOIN tblProduct, tblUnitPrice WHERE ProductId='<id>' AND tblUnitPrice.IsActive='True' AND StockQty>0` (**no SubDepotId filter**) | read |
| Add / Remove | :280 / :335 | none | none | none |
| Submit | `submitButton_Click1` :116 | `SaveDataForChalanInfo`, `LoadChalanById`, `SaveDataForChalanDetail` (+ `SubDCInfoWithDCId`, `SubUpdateDCStockQuantity`) | INSERT tblSubDepotChalanReturnInfo; INSERT tblSubDepotChalanRetuenDetail; SELECT/UPDATE tblSubDepotStore | 3 tables |
| Print | `Button2_Click` :379 | none | wrong viewer (BR-M12-048) | none |

## 11. Frontend Flow
Same page life-cycle as PAGE-450: `Page_Load` preload (:16-27), DC/sub-depot handlers, Search, qty validation (BR-M12-007), Add, Submit, Print. Differences: no submit confirmation dialog; `DeleteImageButton_Click` rebuilds the grid with a `DCStoreId` column (BR-M12-050). Client script: none in this page (old markup); no AJAX.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `submitButton_Click1` | `.cs:116-203` | identical to PAGE-450 but through `SubDepotChalanReturnBLL`: `Validation()` -> totals -> `SaveDataForChalanInfo` (id MAX(SChalanId)+1 in `tblSubDepotChalanReturnInfo`) -> details: INSERT `tblSubDepotChalanRetuenDetail`, then `SubDCInfoWithDCId(DCStoreId)` (SELECT tblSubDepotStore) and `SubUpdateDCStockQuantity(current - qty)` | no | no |
| `Button1_Click` | :253-265 | search (unfiltered by sub-depot, BR-M12-012) | no | no |
| `DeleteImageButton_Click` | :335-378 | breaks (BR-M12-050) | no | no |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotChalanReturnBLL.LoadManufac | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:75 |
| RequisitionBLL.ProductLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:143 |
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| SubDepotChalanReturnBLL.ChalanNoForShow | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:112 |
| SubDepotChalanReturnBLL.ChalanNoGenerator | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:119 |
| SubDepotChalanReturnBLL.SaveDataForChalanInfo | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:20 |
| SubDepotChalanReturnBLL.LoadChalanById | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:57 |
| SubDepotChalanReturnBLL.SaveDataForChalanDetail | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:80 |
| SubDepotChalanReturnBLL.SubDCInfoWithDCId | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:104 |
| SubDepotChalanReturnBLL.SubUpdateDCStockQuantity | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:53 |
| SubDepotChalanReturnBLL.ComUnit | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:148 |
| SubDepotChalanReturnBLL.LoadSubDepot | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:152 |
| SubDepotChalanReturnBLL.GetProductDcStoreSubdeport | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:165 |
| RequisitionBLL.SubdeportLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:157 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotChalanReturnDAL.LoadManufac | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:151 | inline SQL: tblManufacturer |  |
| RequisitionDAL.ProductLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:41 | inline SQL: tblProduct |  |
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| SubDepotChalanReturnDAL.SaveDataForChalanInfo | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:17 | inline SQL: tblSubDepotChalanReturnInfo |  |
| SubDepotChalanReturnDAL.LoadChalanById | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:193 | inline SQL: tblSubDepotChalanReturnInfo |  |
| SubDepotChalanReturnDAL.SaveDataForChalanDetail | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:25 | inline SQL: tblSubDepotChalanRetuenDetail |  |
| SubDepotChalanReturnDAL.SubDCInfoWithDCId | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:146 | inline SQL: tblSubDepotStore |  |
| SubDepotChalanReturnDAL.SubUpdateDCStockQuantity | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:317 | inline SQL: tblSubDepotStore |  |
| SubDepotChalanReturnDAL.LoadComUnit | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:292 | inline SQL: tblCompanyUnit |  |
| SubDepotChalanReturnDAL.LoadSubDepot | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:299 | inline SQL: tblSubDepot |  |
| SubDepotChalanReturnDAL.GetProductDcStoreSubdeport | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:338 | inline SQL: tblCompanyUnit, tblProduct, tblSubDepotStore, tblUnitPrice |  |
| RequisitionDAL.SubdeportLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:46 | inline SQL: tblSubDepot | @ComUnitId |

## 16. Stored Procedure Mapping
`ExecuteAllSqlQueryByStoreProcedure` (dynamic SQL wrapper; body not in repo - UNKNOWN). No business SP.

## 17. Database Mapping
| Table | Access | Columns | Source |
|---|---|---|---|
| tblSubDepotChalanReturnInfo | INSERT, SELECT | as tblSubDepotChalanInfo (SChalanId, ChalanDate, ChalanNo, TrackNo, DriverName, From*, SubDepotCode/Name/Address, TotalValue, TotalVat, GrandTotal, ManufacId, IsDeliver=false) | `SubDepotChalanReturnDAL.cs:16-21,192-196` |
| tblSubDepotChalanRetuenDetail | INSERT | SChalanDetailsId, ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, SChalanId, DCStoreId(=SubDCStoreId) | :24-29 |
| tblSubDepotStore | SELECT, UPDATE | StockQty | :145-149,316-320,337-346 |
| tblUnitPrice, tblProduct, tblSubDepot, tblCompanyUnit, tblManufacturer | SELECT | look-ups | - |

## 18. Validation Matrix
Identical matrix to PAGE-450 §18 (BR-M12-008, BR-M12-007, `No Product Found!!`, `Company Unit Information Not Found!!`, `Sub-Depot Information Not Found!!`). Additional: none checks that the ticked stock rows belong to the chosen sub-depot (BR-M12-012).

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId). Every sub-depot dropdown is filtered by the DC chosen just before it (`WHERE ComUnitId=@ComUnitId`, label `SubDepotCode:SubDepotName`, value SubDepotId, default item `--------Select---------`). | Library.DAL/SInventory_DAL/RequisitionDAL.cs:45-52,761-770; Library.DAL/SubDepot_DAL/SubDepotDAL.cs:15-19 | RequisitionDAL.SubdeportLoad | CONFIRMED |
| BR-M12-004 | Sub-depot documents use the text prefix `SDC` + 6-digit zero-padded (MAX(id)+1): DC->Sub-depot chalan (tblSubDepotChalanInfo.SChalanId), Sub-depot->DC return chalan (tblSubDepotChalanReturnInfo.SChalanId) and the direct stock-adjustment code (tblSubDepoStockAdjustment.SubDepoStockOutId). Padding covers ids of 1-3 digits only; ids >= 1000 are not padded (`SDC1000`). The three counters are independent, so the same `SDC00000n` text can exist in three tables (and both chalan tables). | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:19-25,103-128; Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:19-25,111-136; Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:19-25,124-142; Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:29-45 | ChalanNoGenerator / ClsPrimaryKeyFind.PrimaryKeyMax (SELECT ISNULL(MAX(col),0)+1) | CONFIRMED |
| BR-M12-007 | Transfer quantity accepts digits only (FilteredTextBoxExtender ValidChars 0-9). A post-back on the textbox compares each row's qty to its Stock Qty cell and, if greater, clears the box with `Transfer Qty. cantbe more then Stock Quantity`. The comparison is client-triggered only - it is NOT repeated on Submit, so a stale/edited value can exceed stock. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:388-420 and .aspx:315-323; StockTransferSubDepottoDc.aspx.cs:388-420; StockOutSubDepot.aspx.cs:414-446 | dQtyTextBox_TextChanged | CONFIRMED |
| BR-M12-008 | Chalan submit validation (DC->Sub and Sub->DC): chalan no present, chalan date present, manufacturer selected, from-unit code present, to-unit code present, at least one line (`Please Insert chalanNo!!`, `Please Insert chalanDate!!`, `Please Select Manufacturer!!`, `Please Select fromComUnit!!`, `Please Select toComUnit!!`, `Please Add Product!!`). Truck no and driver name are optional. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:77-115; StockTransferSubDepottoDc.aspx.cs:77-115 | Validation() | CONFIRMED |
| BR-M12-009 | Chalan valuation: per line Value = UnitPrice x Qty; Vat = Qty x VATAmountPerUnit; ValueWVat = Value + Vat; header TotalValue/TotalVat/GrandTotal are the sums. UnitPrice/VAT come from the ACTIVE tblUnitPrice (trade price, `ISNULL(VATAmountPerUnit,0)`) joined by ProductCode - not from batch cost. No rounding is applied. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:134-155,170-190; SubDepotChalanDAL.cs:330-336 | submitButton_Click1 | CONFIRMED |
| BR-M12-011 | Sub-depot->DC return send mirrors BR-10 but writes tblSubDepotChalanReturnInfo/tblSubDepotChalanRetuenDetail and reduces the SUB-DEPOT store row by read-modify-write `UPDATE tblSubDepotStore SET StockQty='<current - qty>'` (looked up by SubDCStoreId). IsDeliver=false until the DC receives it. | Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:19-25,79-90; Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:16-33,145-149,316-320 | SubDepotChalanReturnBLL.SaveDataForChalanDetail | CONFIRMED |
| BR-M12-012 | Sub-depot->DC product search returns sub-depot stock (`StockQty>0`, active unit price) for the selected product from ALL sub-depots: the sub-depot argument passed by the page (`toComUnitNameTextBox.Text`) is not used in the SQL. The chosen sub-depot is only recorded on the chalan header, not enforced on the stock rows picked. | Library.DAL/SubDepot_DAL/SubDepotChalanReturnDAL.cs:337-346; Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs:253-265 | SubDepotChalanReturnDAL.GetProductDcStoreSubdeport | CONFIRMED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)). The transfer, adjustment and invoice pages use the unfiltered DC list (`select ... from tblCompanyUnit`) so any logged-in user may pick any DC. | Solution.Web/SubDepot_UI/StockReceiveBySubDepot.aspx.cs:16-31; SubDeportStockReport.aspx.cs:11-27; Library.DAL/SInventory_DAL/RequisitionDAL.cs:35-39,53-64; StockTransferDcToSubDepot.aspx.cs:25 | Page_Load DC dropdown loaders | CONFIRMED |
| BR-M12-047 | Selecting a product on DC->Sub-depot and Stock Adjustment pages fires the DC dropdown handler (`onselectedindexchanged=salescenterDropDownList1_SelectedIndexChanged`), which rebinds and resets the Sub-depot dropdown to `--------Select---------`; the sub-depot must be chosen after the product or it is lost. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx:152-158; StockOutSubDepot.aspx:147-149; StockOutSubDepot.aspx.cs:448-473 | productDropDownList markup | CONFIRMED |
| BR-M12-048 | The Print button on the Sub-depot->DC return page opens the DC->Sub-depot chalan viewer (`DcToSubdepotChalanReportViewer`), which queries tblSubDepotChalanInfo/Detail by ChalanNo. A return chalan lives in tblSubDepotChalanReturnInfo, so the print returns either no data or the DC->Sub chalan that happens to have the same `SDC00000n` text. | Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs:379-386; Library.DAL/SInventory_DAL/ChalanDAL.cs:579-594; Solution.Web/SInventory_RPTVIEW/DcToSubdepotChalanReportViewer.aspx.cs:15-22 | Button2_Click | INFERRED |
| BR-M12-049 | On the Sub-depot->DC return page `ChalanDetail.DCStoreId` (persisted to tblSubDepotChalanRetuenDetail.DCStoreId) carries the SUB-DEPOT store id (DataKey `SubDCStoreId`). The DC-side receive query joins that value to `tblDCStore.DCStoreId`, so ExpDate/MfgDate/ComUnitId shown and used at DC receive come from an unrelated DC store row with the same numeric id. | Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs:170-176 and .aspx:295; Library.DAL/SInventory_DAL/ChalanDAL.cs:288-298; Solution.Web/SInventory_UI/SubdeportTransferReceiveProductByChalanByDC.aspx.cs:40-50,74-75 | SubChalanReceieve | INFERRED |
| BR-M12-050 | The Remove (delete row) handler of the Sub-depot->DC page rebuilds the chalan grid with a `DCStoreId` column while the grid is bound with DataKeyNames `SubDCStoreId,...`; re-binding then fails (`does not contain a property with the name 'SubDCStoreId'`), so removing a line raises an error. | Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs:335-378 and .aspx:295-311 | DeleteImageButton_Click | INFERRED |

Calculation identical to BR-M12-009 (trade price x qty + VAT).

## 20. Permission Matrix
As PAGE-450: no Session role/user check in page code; DC list unfiltered; menu-based only. Any user can return stock of any sub-depot to any DC.

## 21. Status Lifecycle
`tblSubDepotChalanReturnInfo.IsDeliver`: False at creation -> True when the DC receives (`ChalanDAL.SubChalanUpdate`, `ChalanDAL.cs:70-77`, WF-M12-02). Sub-depot stock is already reduced at creation. No cancel/edit. See the state machine in `README.md`.

## 22. Error Handling
No try/catch. Delete-row failure (BR-M12-050) surfaces as an unhandled ASP.NET data-binding exception. Other behaviour as PAGE-450. No confirmation dialog before submit.

## 23. Audit
No audit columns (no user/time written). UNKNOWN whether DB defaults exist.

## 24. Integration
None.

## 25. Reports
Print opens the DC->Sub-depot viewer (RPT-M12-01) - wrong for return chalans (BR-M12-048). No dedicated return-chalan report exists in M12 (GAP-M12-011).

## 26. Dependencies
BLL: `SubDepotChalanReturnBLL`, `RequisitionBLL`; DAL: `SubDepotChalanReturnDAL` (copy of `SubDepotChalanDAL` with return tables), `ClsCommonInternalDAL`, `ClsPrimaryKeyFind`; DAO `ChalanInfo`, `ChalanDetail`.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Happy path | DC, sub-depot, product; tick sub-depot batch qty 5 (stock 20); Add; Submit | header+detail in return tables IsDeliver=false; sub-depot StockQty 15 | BR-M12-011 |
| TC-02 | No lines | Submit | `Please Add Product!!` | BR-M12-008 |
| TC-03 | Wrong sub-depot rows | choose sub-depot A, search product held by B | B's stock is listed and can be returned under A's chalan | BR-M12-012 |
| TC-04 | Qty > stock | enter more than stock | cleared + alert | BR-M12-007 |
| TC-05 | Remove a line | click remove icon | data-binding error (defect) | BR-M12-050 |
| TC-06 | Print | click Print after save | no data / wrong chalan | BR-M12-048 |
| TC-07 | Number collision | create DC->Sub chalan and return chalan with same sequence | both show `SDC00000n`; print shows DC->Sub one | BR-M12-004/048 |
| TC-08 | DC receipt data | receive the return at DC (WF-M12-02) | Exp/Mfg date/ComUnit derived from tblDCStore row with id = SubDCStoreId | BR-M12-049 |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx`, `Solution.Web/SubDepot_UI/StockTransferSubDepottoDc.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `StockTransferSubDepottoDc.aspx(.cs)` | n/a | MISSING | - |
| Search scope | None existed | not filtered by sub-depot | - | GAP | BR-M12-012 |
| Print | None existed | wrong viewer | - | GAP | BR-M12-048 |
| Remove-row | None existed | broken | - | GAP | BR-M12-050 |

## 30. Known Gaps
- Search returns stock of all sub-depots (BR-M12-012); delete-row broken (BR-M12-050); `DCStoreId` field holds SubDCStoreId (BR-M12-049); print uses wrong viewer (BR-M12-048); no confirm; same transaction/concat/SQL risks as PAGE-450; chalan numbering `SDC` shared with DC->Sub (BR-M12-004); `SubDepotChalanReturnBLL.DCStockIn2` counts `SubDCStoreId` on `tblSubDepotChalanReturnInfo` (wrong table, unused - GAP-M12-018).

## 31. Change Impact
- `SubDepotChalanReturnBLL/DAL` used only by this page. Consumers of the rows: `ChalanDAL.SubdeportChalanLoadInReceive`, `ChalanDAL.SubChalanReceieve` (DC receive pages PAGE-442/441), `ChalanDAL.SubChalanUpdate`. Stock decrement is visible in `SubDeportStockReport` (RPT-M12-06); no in-transit bucket for return chalans exists there (the report only counts DC->Sub chalans).

