---
page_id: PAGE-450
module: M12
page: StockTransferDcToSubDepot
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-450 — StockTransferDcToSubDepot

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-450 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | StockTransferDcToSubDepot |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_StockTransferDcToSubDepot |
| aspx size | 22955 bytes |
| Code-behind LOC | 476 |
| Inline script size | 985 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**StockTransferDcToSubDepot** (`Stock Transfer DC to Sub-Depot`, `StockTransferDcToSubDepot.aspx:18`) issues stock from a DC to one of its sub-depots. The user chooses the sending DC (`Sales Center`), the receiving sub-depot, a product, searches the DC's batch rows, ticks the batches to send with a quantity each, adds them to a list, enters truck/driver (optional) and submits. The page creates a delivery chalan (`SDC00000n`) with values, deducts the DC stock immediately and leaves the chalan `IsDeliver=false` (in transit) until the sub-depot receives it (WF-M12-01). CONFIRMED (`StockTransferDcToSubDepot.aspx.cs:116-203`).

## 3. Business Objective
Move goods from the main depot to a sub-depot and produce the transport document (chalan) with trade value and VAT, so that the sub-depot can later confirm receipt and its own stock is created. INFERRED from names/flow; the parent-DC/sub-depot relation is CONFIRMED (BR-M12-001). The chalan value is at trade price (BR-M12-009), not cost, so this is a physical transfer document rather than an accounting posting.

## 4. Navigation
Menu placement DB-driven (path `UNKNOWN`). Inbound: none in code. Outbound: `../SInventory_RPTVIEW/DcToSubdepotChalanReportViewer.aspx?chalanno=` (Print, `.cs:379-386`); Reset -> `StockTransferDcToSubDepot.aspx` (`.cs:472-475`). Pending chalans are later opened from `StockReceiveBySubDepot.aspx` (PAGE-449).

## 5. Route
`~/SubDepot_UI/StockTransferDcToSubDepot.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx |
| Code-behind | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | SubDepotChalanBLL (Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | SubDepotChalanDAL (Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs) |
| DAO/entity class | ChalanDetail (Library.DAO/SInventory_Entities/ChalanDetail.cs) |
| DAO/entity class | ChalanInfo (Library.DAO/SInventory_Entities/ChalanInfo.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible | Data source | Event -> handler |
|---|---|---|---|---|---|---|
| chalanDateTextBox | Chalan Date : | server: non-empty (BR-M12-008) | today (`Todate()` :30-33) | ReadOnly=True | - | - |
| salescenterDropDownList1 | Sales Center : | via fromComUnitCode | - | - | `RequisitionBLL.DCLoad` all DCs (:25) | `salescenterDropDownList1_SelectedIndexChanged` (:422) |
| fromComUnitCodeTextBox / Name / Address | Sales Center Code / Name / Address | code required | from DC selection (`SubDepotChalanBLL.ComUnit` -> `tblCompanyUnit`) | ReadOnly=True; code box AutoPostBack | - | `fromComUnitCodeTextBox_TextChanged` (:210) `Company Unit Information Not Found!!` |
| truckNoTextBox | Truck Number : | no | - | - | free text | - |
| driverNameTextBox | Driver Name: | no | - | - | free text | - |
| chalanNoTextBox | Chalan No: | server: non-empty | next `SDC` number preview (`ChalanId()` :204) | ReadOnly=True | `SubDepotChalanBLL.ChalanNoForShow` | - |
| subdeportDropDownList2 | Sub-Depot : | via toComUnitCode | `--------Select---------` | - | `RequisitionBLL.SubdeportLoad(DC)` | `subdeportDropDownList2_SelectedIndexChanged` (:448) |
| toComUnitCodeTextBox / Name / Address | Sub-Depot Code / Name / Address | code required | from `tblSubDepot` | ReadOnly | `SubDepotChalanBLL.LoadSubDepot` | `toComUnitCodeTextBox_TextChanged` (:231) `Sub-Depot Information Not Found!!` |
| manufacDropDownList | Manufacturer: | server: SelectedValue non-empty | index 1 (`:24`) | - | `SubDepotChalanBLL.LoadManufac` | - |
| productDropDownList | Product: | - | first item | handler = DC handler (BR-M12-047) | `RequisitionBLL.ProductLoad` | -> `salescenterDropDownList1_SelectedIndexChanged` |
| Button1 `Search Product` | - | - | - | - | `GetProductDcStoreSubdeport(ProductId, fromComUnitCode)` | `Button1_Click` (:253) `No Product Found!!` |
| productGridView | Product Code / Name / Stock Qty / Batch No / ExpDate / ReceiveDate / Transfer Qty | - | - | DataKeys `DCStoreId,VATAmountPerUnit,UnitPrice` | tblDCStore rows | `chkSelectAll_CheckedChanged` (:34) |
| transferQtyTextBox | Transfer Qty | non-empty to be added | empty | digits only | user | `dQtyTextBox_TextChanged` (:388) |
| addButton `Add to List` | - | - | - | - | builds chalan grid | `addButton_Click` (:280) |
| chalanGridView | Product Code / Name / Transfer Qty / Batch No / ExpDate / Remove | >=1 row (BR-M12-008) | - | DataKeys `DCStoreId,VATAmountPerUnit,UnitPrice` | in-page | `DeleteImageButton_Click` (:335) |
| submitButton `Submit` | - | - | - | JS `sweetAlertConfirm_Submit` confirm (master page) | writes chalan | `submitButton_Click1` (:116) |
| printChalanNoTextBox + Button2 `Print` | Chalan No : | no | set after a successful save (:159-163) | AutoPostBack | - | `Button2_Click` (:379) |

## 8. Field Mapping
| UI control | Variable / DTO | Handler | BLL | DAL | SQL parameter | Table.column |
|---|---|---|---|---|---|---|
| chalanDateTextBox | `ChalanInfo.ChalanDate` (`Convert.ToDateTime`) | :122 | `SubDepotChalanBLL.SaveDataForChalanInfo` | `SubDepotChalanDAL.SaveDataForChalanInfo` :16 | concatenated | `tblSubDepotChalanInfo.ChalanDate` |
| chalanNoTextBox (display) / generated | `ChalanNo` = `SDC`+id (regenerated in BLL) | :123 / BLL:23 | `ChalanNoGenerator` | same | concatenated | `.ChalanNo`; `.SChalanId` = MAX+1 |
| truckNoTextBox | `TrackNo` | :124 | same | same | concatenated | `.TrackNo` |
| driverNameTextBox | `DriverName` | :125 | same | same | concatenated | `.DriverName` |
| fromComUnitCode/Name/Address | `FromComUnitCode/Name/Address` | :126-128 | same | same | concatenated | `.FromComUnitCode/.FromComUnitName/.FromComUnitAddress` |
| toComUnit* (sub-depot) | `ToComUnitCode/Name/Address` | :129-131 | same | same | concatenated | `.SubDepotCode/.SubDepotName/.SubDepotAddress` |
| manufacDropDownList | `ManufacId` | :132 | same | same | concatenated | `.ManufacId` |
| computed totals | `TotalValue/TotalVat/GrandTotal` | :134-155 | same | same | concatenated | `.TotalValue/.TotalVat/.GrandTotal` |
| (constant) | `IsDeliver` = false | DAL:18-19 | - | same | literal `'false'` | `.IsDeliver` |
| chalanGridView row | `ChalanDetail`: DCStoreId=DataKeys[0], ProductCode=cell0, ProductName=cell1, Quantity=cell2, BatchNo=cell3, UnitPrice=DataKeys[2], Value, Vat, ValueWVat | :170-190 | `SaveDataForChalanDetail` | `SubDepotChalanDAL.SaveDataForChalanDetail` :24 | concatenated | `tblSubDepotChalanDetail.DCStoreId/ProductCode/ProductName/Quantity/BatchNo/UnitPrice/Value/Vat/ValueWVat/SChalanId/SChalanDetailsId` |
| Quantity | stock deduction | BLL:82-83 | `DCInfoWithDCId` + `UpdateDCStockQuantity` | DAL:140-144,306-310 | concatenated | `tblDCStore.StockQty` (read then set to `current - qty`) |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Value/Text | Tables |
|---|---|---|---|---|
| salescenterDropDownList1 | `Page_Load` -> `RequisitionBLL.DCLoad(ddl)` (:25) | `select ComUnitId,(ComUnitCode+':'+ComUnitName) as Com from tblCompanyUnit` | ComUnitId / `Code:Name` | tblCompanyUnit |
| subdeportDropDownList2 | DC change (:424) | `... from tblSubDepot WHERE ComUnitId=@ComUnitId` | SubDepotId / `Code:Name` | tblSubDepot |
| productDropDownList | `Page_Load` (:23) | `SELECT (ProductCode+':'+ProductName)Pro,* FROM tblProduct` | ProductId | tblProduct |
| manufacDropDownList | `SubDepotChalanBLL.LoadManufac` (:22) | `SELECT * FROM dbo.tblManufacturer` | ManufacId / ManufacName | tblManufacturer |

## 10. Button Action Mapping
| Button | Handler | BLL -> DAL | SQL | DB change |
|---|---|---|---|---|
| Search Product | `Button1_Click` :253 | `GetProductDcStoreSubdeport` -> `SubDepotChalanDAL` :327 | SELECT tblDCStore LEFT JOIN tblProduct, tblCompanyUnit, tblUnitPrice WHERE `ComUnitCode='<code>' AND tblProduct.ProductId='<id>' AND tblUnitPrice.IsActive='True'` (dates via `format(...,'dd MMM, yyyy')`) | read |
| Add to List / Remove | `addButton_Click` :280 / `DeleteImageButton_Click` :335 | none | none | none |
| Submit | `submitButton_Click1` :116 (after JS confirm) | `SaveDataForChalanInfo` (id+no generation), `LoadChalanById`, `SaveDataForChalanDetail` per line | INSERT tblSubDepotChalanInfo; INSERT tblSubDepotChalanDetail; SELECT tblDCStore; UPDATE tblDCStore SET StockQty | 3 tables written (§17) |
| Print | `Button2_Click` :379 | none | viewer runs `ChalanDAL.SubDepotChalanReport` (RPT-M12-01) | none |
| Reset | `Unnamed_Click` :472 | none | none | none |

## 11. Frontend Flow
- `Page_Load` first load: next chalan no preview, date today, manufacturers, products, manufacturer index 1, DC list (:16-27).
- DC change: fills sub-depot dropdown, copies DC code to `fromComUnitCodeTextBox`, loads name/address; sub-depot change: name/address from `tblSubDepot`.
- Selecting a product re-runs the DC handler and resets the sub-depot dropdown (BR-M12-047).
- Search -> product grid; qty TextChanged validation (BR-M12-007); Add -> chalan grid; Submit JS confirm (`sweetAlertConfirm_Submit`, `NewMasterPage.master:441`) then post-back; after save `Clear()` (does NOT clear `printChalanNoTextBox`), `Todate()`, `ChalanId()`.
- Client script: `pageLoad()` select2/pickadate init. No AJAX/PageMethods.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `submitButton_Click1` | `.cs:116-203` | `Validation()` (BR-M12-008) -> build `ChalanInfo` -> compute totals (BR-M12-009) -> `SaveDataForChalanInfo(out chalanId)` [id = MAX(SChalanId)+1, no = `SDC`+id] -> `LoadChalanById` (sets `printChalanNoTextBox`) -> if status true: build `ChalanDetail` list -> `SaveDataForChalanDetail(list)` [per line: id = MAX(SChalanDetailsId)+1, INSERT detail, SELECT tblDCStore row, UPDATE tblDCStore.StockQty = current - qty] -> alert `Data save successfully`, Clear, new number | no | no |
| `addButton_Click` | :280-334 | as BR-M12-006 | no | no |
| `dQtyTextBox_TextChanged` | :388-420 | BR-M12-007 | no | no |
| look-up handlers | :210-251,422-470 | `tblCompanyUnit` / `tblSubDepot` | no | no |

There is no rollback: if a detail INSERT fails after the header INSERT, the header stays with `IsDeliver=false` and DC stock for earlier lines has already been deducted. `SaveDataByInsertCommand` rethrows (yellow error page); `UpdateDataByUpdateCommand` swallows (BR-M12-010).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepotChalanBLL.LoadManufac | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:71 |
| RequisitionBLL.ProductLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:143 |
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| SubDepotChalanBLL.ChalanNoForShow | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:104 |
| SubDepotChalanBLL.ChalanNoGenerator | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:111 |
| SubDepotChalanBLL.SaveDataForChalanInfo | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:20 |
| SubDepotChalanBLL.LoadChalanById | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:53 |
| SubDepotChalanBLL.SaveDataForChalanDetail | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:76 |
| SubDepotChalanBLL.DCInfoWithDCId | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:100 |
| SubDepotChalanBLL.UpdateDCStockQuantity | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:49 |
| SubDepotChalanBLL.ComUnit | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:140 |
| SubDepotChalanBLL.LoadSubDepot | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:144 |
| SubDepotChalanBLL.GetProductDcStoreSubdeport | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:157 |
| RequisitionBLL.SubdeportLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:157 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepotChalanDAL.LoadManufac | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:146 | inline SQL: tblManufacturer |  |
| RequisitionDAL.ProductLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:41 | inline SQL: tblProduct |  |
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| SubDepotChalanDAL.SaveDataForChalanInfo | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:17 | inline SQL: tblSubDepotChalanInfo |  |
| SubDepotChalanDAL.LoadChalanById | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:188 | inline SQL: tblSubDepotChalanInfo |  |
| SubDepotChalanDAL.SaveDataForChalanDetail | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:25 | inline SQL: tblSubDepotChalanDetail |  |
| SubDepotChalanDAL.DCInfoWithDCId | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:141 | inline SQL: tblDCStore |  |
| SubDepotChalanDAL.UpdateDCStockQuantity | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:307 | inline SQL: tblDCStore |  |
| SubDepotChalanDAL.LoadComUnit | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:287 | inline SQL: tblCompanyUnit |  |
| SubDepotChalanDAL.LoadSubDepot | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:294 | inline SQL: tblSubDepot |  |
| SubDepotChalanDAL.GetProductDcStoreSubdeport | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:328 | inline SQL: tblCompanyUnit, tblDCStore, tblProduct, tblUnitPrice |  |
| RequisitionDAL.SubdeportLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:46 | inline SQL: tblSubDepot | @ComUnitId |

## 16. Stored Procedure Mapping
| SP | Use | Body in repo |
|---|---|---|
| `ExecuteAllSqlQueryByStoreProcedure(@Query)` | executes every INSERT/UPDATE string built in `SubDepotChalanDAL` | NO - UNKNOWN |
No business SP. Page-level table effects are known from the inline SQL, but what the wrapper SP does beyond `EXEC(@Query)` is UNKNOWN.

## 17. Database Mapping
| Table | Access | Columns | Source |
|---|---|---|---|
| tblSubDepotChalanInfo | INSERT, SELECT | SChalanId, ChalanDate, ChalanNo, TrackNo, DriverName, FromComUnitCode/Name/Address, SubDepotCode/Name/Address, TotalValue, TotalVat, GrandTotal, ManufacId, IsDeliver(false) | `SubDepotChalanDAL.cs:16-21,187-191` |
| tblSubDepotChalanDetail | INSERT | SChalanDetailsId, ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, SChalanId, DCStoreId | `SubDepotChalanDAL.cs:24-29` |
| tblDCStore | SELECT, UPDATE | search (StockQty, batch, dates, ComUnitCode via tblCompanyUnit); `StockQty = current - qty` | `SubDepotChalanDAL.cs:140-144,306-310,327-340` |
| tblUnitPrice | SELECT | UnitPrice, VATAmountPerUnit, VATPercentage (IsActive) | :330-336 |
| tblCompanyUnit, tblSubDepot, tblProduct, tblManufacturer | SELECT | look-ups | - |

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Chalan No | required | ReadOnly | yes | `Please Insert chalanNo!!` | BR-M12-008 |
| Chalan Date | required | ReadOnly (today) | yes | `Please Insert chalanDate!!` | BR-M12-008 |
| Manufacturer | required | - | yes | `Please Select Manufacturer!!` | BR-M12-008 |
| From DC code | required | ReadOnly | yes | `Please Select fromComUnit!!` | BR-M12-008 |
| To sub-depot code | required | ReadOnly | yes | `Please Select toComUnit!!` | BR-M12-008 |
| Lines | >=1 | - | yes | `Please Add Product!!` | BR-M12-008 |
| Transfer Qty | digits; <= stock | FilteredTextBoxExtender; post-back | not re-checked at submit | `Transfer Qty. cantbe more then Stock Quantity` | BR-M12-007 |
| Product search | rows exist | - | yes | `No Product Found!!` | BR-M12-005 |
| DC/sub-depot code look-up | exists | - | yes | `Company Unit Information Not Found!!` / `Sub-Depot Information Not Found!!` | - |
| Truck / driver | none | - | - | - | - |
| Qty 0, decimals, apostrophes in names, date format | not validated | - | - | SQL text is concatenated: a `'` in a product name breaks the INSERT | GAP-M12-003 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId). Every sub-depot dropdown is filtered by the DC chosen just before it (`WHERE ComUnitId=@ComUnitId`, label `SubDepotCode:SubDepotName`, value SubDepotId, default item `--------Select---------`). | Library.DAL/SInventory_DAL/RequisitionDAL.cs:45-52,761-770; Library.DAL/SubDepot_DAL/SubDepotDAL.cs:15-19 | RequisitionDAL.SubdeportLoad | CONFIRMED |
| BR-M12-004 | Sub-depot documents use the text prefix `SDC` + 6-digit zero-padded (MAX(id)+1): DC->Sub-depot chalan (tblSubDepotChalanInfo.SChalanId), Sub-depot->DC return chalan (tblSubDepotChalanReturnInfo.SChalanId) and the direct stock-adjustment code (tblSubDepoStockAdjustment.SubDepoStockOutId). Padding covers ids of 1-3 digits only; ids >= 1000 are not padded (`SDC1000`). The three counters are independent, so the same `SDC00000n` text can exist in three tables (and both chalan tables). | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:19-25,103-128; Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:19-25,111-136; Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:19-25,124-142; Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:29-45 | ChalanNoGenerator / ClsPrimaryKeyFind.PrimaryKeyMax (SELECT ISNULL(MAX(col),0)+1) | CONFIRMED |
| BR-M12-005 | DC->Sub-depot product search lists tblDCStore rows of the selected DC (matched by ComUnitCode) for the selected product with an ACTIVE tblUnitPrice row. The `StockQty>0` filter is commented out and StockCondition is not filtered, so zero-stock and non-Available rows are returned. | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:327-340 (query text 330-336; `AND StockQty>0` commented at 337); Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:253-265 | SubDepotChalanDAL.GetProductDcStoreSubdeport | CONFIRMED |
| BR-M12-006 | Add-to-list keeps only rows whose checkbox is ticked AND whose Transfer Qty is non-empty; a DCStoreId already in the chalan list is silently not added again (HasDCStoreId). Previously listed lines are re-added from the grid text/DataKeys on every click. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:267-334; StockTransferSubDepottoDc.aspx.cs:267-334 | addButton_Click / HasDCStoreId | CONFIRMED |
| BR-M12-007 | Transfer quantity accepts digits only (FilteredTextBoxExtender ValidChars 0-9). A post-back on the textbox compares each row's qty to its Stock Qty cell and, if greater, clears the box with `Transfer Qty. cantbe more then Stock Quantity`. The comparison is client-triggered only - it is NOT repeated on Submit, so a stale/edited value can exceed stock. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:388-420 and .aspx:315-323; StockTransferSubDepottoDc.aspx.cs:388-420; StockOutSubDepot.aspx.cs:414-446 | dQtyTextBox_TextChanged | CONFIRMED |
| BR-M12-008 | Chalan submit validation (DC->Sub and Sub->DC): chalan no present, chalan date present, manufacturer selected, from-unit code present, to-unit code present, at least one line (`Please Insert chalanNo!!`, `Please Insert chalanDate!!`, `Please Select Manufacturer!!`, `Please Select fromComUnit!!`, `Please Select toComUnit!!`, `Please Add Product!!`). Truck no and driver name are optional. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:77-115; StockTransferSubDepottoDc.aspx.cs:77-115 | Validation() | CONFIRMED |
| BR-M12-009 | Chalan valuation: per line Value = UnitPrice x Qty; Vat = Qty x VATAmountPerUnit; ValueWVat = Value + Vat; header TotalValue/TotalVat/GrandTotal are the sums. UnitPrice/VAT come from the ACTIVE tblUnitPrice (trade price, `ISNULL(VATAmountPerUnit,0)`) joined by ProductCode - not from batch cost. No rounding is applied. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:134-155,170-190; SubDepotChalanDAL.cs:330-336 | submitButton_Click1 | CONFIRMED |
| BR-M12-010 | DC->Sub-depot send: header saved with IsDeliver=false (INSERT tblSubDepotChalanInfo), each line saved to tblSubDepotChalanDetail, and DC stock is reduced IMMEDIATELY by read-modify-write `UPDATE tblDCStore SET StockQty='<current - qty>'` (no `Trans_Type='Issue'` ledger row, no CostPerUnit, no negative-stock guard). Nothing is in a transaction; UPDATE failures are swallowed. | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:19-25,75-86; Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:16-29,306-310; Library.DAL/InternalCls/ClsCommonInternalDAL.cs:397-421,465-490 | SubDepotChalanBLL.SaveDataForChalanInfo / SaveDataForChalanDetail | CONFIRMED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)). The transfer, adjustment and invoice pages use the unfiltered DC list (`select ... from tblCompanyUnit`) so any logged-in user may pick any DC. | Solution.Web/SubDepot_UI/StockReceiveBySubDepot.aspx.cs:16-31; SubDeportStockReport.aspx.cs:11-27; Library.DAL/SInventory_DAL/RequisitionDAL.cs:35-39,53-64; StockTransferDcToSubDepot.aspx.cs:25 | Page_Load DC dropdown loaders | CONFIRMED |
| BR-M12-047 | Selecting a product on DC->Sub-depot and Stock Adjustment pages fires the DC dropdown handler (`onselectedindexchanged=salescenterDropDownList1_SelectedIndexChanged`), which rebinds and resets the Sub-depot dropdown to `--------Select---------`; the sub-depot must be chosen after the product or it is lost. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx:152-158; StockOutSubDepot.aspx:147-149; StockOutSubDepot.aspx.cs:448-473 | productDropDownList markup | CONFIRMED |

**Worked calculation (BR-M12-009)**: UnitPrice 100.00, VAT/unit 15.00, Qty 10 -> Value 1,000.00, Vat 150.00, ValueWVat 1,150.00; header totals are the sums of lines; nothing is rounded before storing (`TotalVat` etc. concatenated as decimal text).

## 20. Permission Matrix
- Session keys read by the page code: none (master page enforces `Session["UserId"]`).
- The DC list is the unfiltered `tblCompanyUnit` list (BR-M12-045): any authenticated user with menu access may issue stock from any DC to any sub-depot. No role/UserType check in code.
- Permission per control: none. Role matrix in DB (`tblMainMenu`/`tblMenuDistribution`) - UNKNOWN.

## 21. Status Lifecycle
`tblSubDepotChalanInfo.IsDeliver`: **False** at creation (DAL literal `'false'`, `SubDepotChalanDAL.cs:19`) -> set to **True** by `SubDepotChalanBLL.DCStockIn2 -> ChalanUpdate` when the sub-depot receives (WF-M12-01). NULL is tolerated by list queries (BR-M12-013) but never written by this page. No cancel/reject/edit state. DC stock is reduced at creation, i.e. stock is 'in transit' between the two ledgers; the Sub-depot Stock Report shows it as `In-transit` (BR-M12-043).

## 22. Error Handling
No try/catch. Failures: DB errors from INSERT propagate (`throw ex`); UPDATE errors swallowed; `Convert.ToDecimal/ToDateTime` exceptions on bad grid text/culture dates. Success: alert `Data save successfully`. `Global.asax` `Application_Error` is empty. Culture: `ChalanDate` is concatenated into SQL as `DateTime.ToString()` - result depends on server culture (INFERRED risk).

## 23. Audit
No audit columns are written to `tblSubDepotChalanInfo/Detail` (no CreateBy/CreateDate in the INSERT text). The session user is not recorded. Any audit columns that exist in the DB table are UNKNOWN.

## 24. Integration
None (no SAP/mobile). Compare main depot B2B transfer which has SAP challan integration (`sp_SAP_B2B*`).

## 25. Reports
Print -> `DcToSubdepotChalanReportViewer.aspx?chalanno=<no>` -> `rptDcToDcChalan.rpt` fed by `ChalanDAL.SubDepotChalanReport` (RPT-M12-01). The stock-report `In-transit` column reflects unreceived chalans (RPT-M12-06).

## 26. Dependencies
BLL: `SubDepotChalanBLL`, `RequisitionBLL`. DAL: `SubDepotChalanDAL`, `RequisitionDAL`, `ClsCommonInternalDAL`, `ClsPrimaryKeyFind`. DAO: `ChalanInfo`, `ChalanDetail`. Tables per §17. Master `NewMasterPage.master`. Toolkit `FilteredTextBoxExtender`.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Happy path | DC + sub-depot + product, search, tick batch qty 10 (stock 50), Add, Submit+confirm | chalan `SDC00000n` header IsDeliver=false, 1 detail (Value=UnitPrice x 10), DC row StockQty 40, alert `Data save successfully` | BR-M12-004/009/010 |
| TC-02 | Empty list (negative) | Submit with no lines | alert `Please Add Product!!`, no DB write | BR-M12-008 |
| TC-03 | Missing sub-depot | do not pick a sub-depot | `Please Select toComUnit!!` | BR-M12-008 |
| TC-04 | Qty > stock | enter 60 for stock 50 | box cleared + alert | BR-M12-007 |
| TC-05 | Edge: qty valid then stock changes | another user takes stock before Submit | no server re-check; DC StockQty may become negative | BR-M12-007/010 |
| TC-06 | Duplicate batch add | add same batch twice | second ignored | BR-M12-006 |
| TC-07 | Zero-stock rows | product with a zero-stock batch | row is listed (filter commented out) | BR-M12-005 |
| TC-08 | Product change after sub-depot | select sub-depot, then change product | sub-depot dropdown resets but `toComUnitCodeTextBox` keeps the old code, so Submit still succeeds with the previously selected sub-depot | BR-M12-047 |
| TC-09 | Apostrophe in product name | product name `X'Y` | INSERT fails (unhandled error) | GAP-M12-003 |
| TC-10 | Print after save | click Print | viewer opens with the new chalan | RPT-M12-01 |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx`, `Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `StockTransferDcToSubDepot.aspx(.cs)` | n/a | MISSING | - |
| Chalan creation | None existed | header+detail+immediate DC deduction | tables not in repo (DDL UNKNOWN) | CONFIRMED (code) | BR-M12-010 |
| Valuation | None existed | trade price x qty + VAT | - | CONFIRMED | BR-M12-009 |
| Transaction safety | None existed | none | - | GAP | GAP-M12-007 |
| Ledger consistency with main DC | None existed | in-place StockQty update vs main `B2BIssue` ledger row | `sp_Report_DCStore_StockValuationLedger` uses issue rows | GAP | GAP-M12-008 |

## 30. Known Gaps
- Stock deduction is immediate and non-atomic; no ledger `Issue` row/cost (main depot B2B chalan does write one: `ChalanDAL.UpdateDCStockQuantity` `Trans_Type='Issue'`, `CostPerUnit`, `ChalanDAL.cs:513-553`) - the DC stock-valuation ledger (`SP_Update_StockValuation_TransactionDate_Logic.sql:84-140`) classifies movements from negative/Issue rows, so this in-place change is not a recognised movement (INFERRED, GAP-M12-008).
- Zero/negative stock allowed, no server-side qty re-check, product/DC list unfiltered by user.
- SQL by string concatenation (injection/quoting risk), culture-dependent date text.
- Preview chalan number `ChalanId()` computed before save (race: two users see the same number; actual number is regenerated in BLL).
- Product dropdown wired to DC handler (BR-M12-047); `DeleteImageButton_Click` OK on this page (DataKeys match) but not on the Sub->DC twin.

## 31. Change Impact
- `SubDepotChalanBLL/DAL` are used by `StockReceiveBySubDepot`, `ProductReceiveBySubDeport` and this page; `SubDepotStockAdjustmentVoucher.aspx.cs:16` instantiates it (unused field); the main-depot page `Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucher.aspx.cs:18,117` calls `SubDepotChalanBLL.GetProductDcStoreSubdeport`, which despite its name reads `tblDCStore` (BR-M12-005).
- Downstream consumers of the rows written: `StockReceiveBySubDepot`/`ProductReceiveBySubDeport` (WF-M12-01), `DcToSubdepotChalanReportViewer`, `DCStockReportDAL.SubDeportStockReportDetailDataDAL` (In-transit bucket), `ChalanDAL.SubdeportDCStoreReport`.
- Changing chalan number generation affects `StockTransferSubDepottoDc` and the stock-adjustment code that share the `SDC` prefix (BR-M12-004).

