---
page_id: PAGE-448
module: M12
page: StockOutSubDepot
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-448 — StockOutSubDepot

> **Evidence level:** Tier A - reviewed 2026-09-19 against the .aspx, .aspx.cs and the BLL/DAL/DAO code they reach. Stored-procedure bodies are NOT in the repository unless stated; such effects are marked `UNKNOWN`. Confidence tags: CONFIRMED (read in code), INFERRED (implied by several references), UNKNOWN.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-448 |
| Module | M12 — Sub-Depot Operations (INFERRED from folder + file name) |
| Page name | StockOutSubDepot |
| Kind | Page (INFERRED from name) |
| Web folder | SubDepot_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SubDepot_UI_StockTransferDcToSubDepot |
| aspx size | 33675 bytes |
| Code-behind LOC | 541 |
| Inline script size | 1189 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**StockOutSubDepot** (breadcrumb caption `Stock Adjustment`, `StockOutSubDepot.aspx:14`) lets a user remove stock from a sub-depot directly. The user picks a DC, one of that DC's sub-depots and a product, ticks the batch rows to write off, enters a quantity per batch, adds them to a list and submits. On Submit the quantities are deducted at once from the sub-depot's stock rows (`tblSubDepotStore`) and an adjustment record (`tblSubDepoStockAdjustment` + `tblSubDepoStockAdjustDetail`) is stored. CONFIRMED (code: `StockOutSubDepot.aspx.cs:117-143`, `SubDepoAdjustBLL.cs:19-39`).

Do not confuse with `SubDepotStockAdjustmentVoucher.aspx` (PAGE-458, voucher + approval, no immediate stock effect) - this page has **no approval, no reason and no date field** (BR-M12-021).

## 3. Business Objective
Keep the physical sub-depot stock record in line with reality (breakage, expiry, loss, count difference) without waiting for a voucher approval. INFERRED - no reason text exists on the form or in the tables; the intent is deduced from the caption and the effect (stock decrement).

The page also carries an unused chalan header block (hidden `chalanNoTextBox`, `driverNameTextBox`, `truckNoTextBox`, `manufacDropDownList`) and a `Print` button that opens `DcToSubdepotChalanReportViewer.aspx?chalanno=<printChalanNoTextBox>`; both are copy-paste remnants of `StockTransferDcToSubDepot.aspx` (the print box is never filled, see GAP-M12-005).

## 4. Navigation
Menu placement is DB-driven (`tblMainMenu`, `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the row is not in the repo, so the exact path is `UNKNOWN`.

Inbound references: none found in code. Outbound: `../SInventory_RPTVIEW/DcToSubdepotChalanReportViewer.aspx` (Print button, `StockOutSubDepot.aspx.cs:405-412`).
Master page `NewMasterPage.master` (hosts `sweetAlertConfirm_Submit`, but this page does NOT attach it to Submit - there is no confirmation prompt).

## 5. Route
`~/SubDepot_UI/StockOutSubDepot.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SubDepot_UI/StockOutSubDepot.aspx |
| Code-behind | Solution.Web/SubDepot_UI/StockOutSubDepot.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | RequisitionBLL (Library.BLL/SInventory_BLL/RequisitionBLL.cs) |
| BLL class | SubDepoAdjustBLL (Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs) |
| DAL class | RequisitionDAL (Library.DAL/SInventory_DAL/RequisitionDAL.cs) |
| DAL class | SubDepoAdjustDAL (Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs) |
| DAO/entity class | SubDepoAdjustDetailDAO (Library.DAO/SubDepot_DAO/SubDepoAdjustDetailDAO.cs) |
| DAO/entity class | SubDepoAdjustMentDAO (Library.DAO/SubDepot_DAO/SubDepoAdjustMentDAO.cs) |

## 7. UI Control Inventory
| Control ID | Label | Required | Default | Read-only / visible (set where) | Data source | Event -> handler |
|---|---|---|---|---|---|---|
| salescenterDropDownList1 | DC: | No validator; must be chosen before sub-depot (BR-M12-045/047) | none (empty until DC list is bound) | markup | `RequisitionBLL.DCLoad` (all `tblCompanyUnit`, `Code:Name`) `StockOutSubDepot.aspx.cs:26` | SelectedIndexChanged -> `salescenterDropDownList1_SelectedIndexChanged` (:448) |
| subdeportDropDownList2 | Sub-Depot: | none (required for Search - empty value => `SubDepotId=''` => no rows) | `--------Select---------` after DC change | markup | `RequisitionBLL.SubdeportLoad(DC)` (:450) | SelectedIndexChanged (:474) |
| toComUnitCodeTextBox / toComUnitNameTextBox / toComUnitAddressTextBox | Sub-Depot Code / Name / Address | no | filled from `tblSubDepot` on sub-depot change | ReadOnly=True (markup); message `Sub-Depot Information Not Found!!` if not found | `SubDepoAdjustBLL.LoadSubDepot` | `toComUnitCodeTextBox_TextChanged` (:257) |
| productDropDownList | Product: | none | first item | markup; **its handler is the DC handler** (BR-M12-047) | `RequisitionBLL.ProductLoad` (`ProductCode:ProductName`, value ProductId) (:24) | -> `salescenterDropDownList1_SelectedIndexChanged` |
| Button1 (`Search`) | - | - | - | - | `SubDepoAdjustBLL.GetProductDcStoreSubdeport(ProductId, SubDepotId)` | Click (:279) - `No Product Found!!` when empty |
| productGridView | Product Code / Name / Stock Qty / Batch / ExpDate / ReceiveDate / Transfer Qty | - | - | DataKeys `DCStoreId` (= aliased SubDCStoreId), `VATAmountPerUnit`, `UnitPrice` | rows of `tblSubDepotStore` | - |
| chkSelectAll / chkSelect | (header / row check) | row must be ticked to be added | unchecked | - | - | `chkSelectAll_CheckedChanged` (:35) |
| transferQtyTextBox | Transfer Qty | non-empty to be added (BR-M12-006) | empty | digits only (FilteredTextBoxExtender) | user | TextChanged -> `dQtyTextBox_TextChanged` (:414): qty > stock => cleared + `Transfer Qty. cantbe more then Stock Quantity` |
| addButton (`Add to List`) | - | - | - | - | builds `chalanGridView` | Click (:306) |
| chalanGridView | Product Code / Name / Transfer Qty / Batch No / ExpDate / Remove | - | - | DataKeys `DCStoreId,VATAmountPerUnit,UnitPrice` | in-page DataTable (postback state) | `DeleteImageButton_Click1` (:498) |
| submitButton (`Submit`) | - | server checks: none (Validation() not called) | - | no confirm dialog | writes adjustment | Click -> `submitButton_Click1` (:117) |
| chalanNoTextBox, chalanDateTextBox, fromComUnit*TextBox, driverNameTextBox, truckNoTextBox, prodctCodeTextBox, manufacDropDownList | (hidden remnants) | not used on submit | date = today, manufacturer index 1 (`Page_Load` :23-25) | Visible=False (markup) | - | - |
| printChalanNoTextBox + Button2 (`Print`) | Chalan No | no | empty | visible | - | `Button2_Click` (:405) opens chalan viewer |

## 8. Field Mapping
| UI control | Variable / DTO | Handler | BLL | DAL | SQL parameter | Table.column |
|---|---|---|---|---|---|---|
| Session["LoginName"] | `SubDepoAdjustMentDAO.EntryBy` | `submitButton_Click1` :121 | `SubDepoAdjustBLL.SaveDataForSubDepoAdjust` | `SubDepoAdjustDAL.SaveDataForSubDepoAdjust` :22 | concatenated literal | `tblSubDepoStockAdjustment.EntryBy` |
| (system) `DateTime.Now` | `EntryDate` | :122 | same | same | concatenated | `tblSubDepoStockAdjustment.EntryDate` |
| (generated) MAX(SubDepoStockOutId)+1 | `SubDepoStockOutId` / `SubDepoStockCode` (`SDC`+6 digits) | `SubDepoAdjustBLL.cs:21-23` | `PrimaryKeyMax` | same | concatenated | `tblSubDepoStockAdjustment.SubDepoStockOutId`, `.SubDepoStockCode` |
| chalanGridView DataKeys[i][0] | `SubDepoAdjustDetailDAO.SubDCStoreId` | :134 | `SaveDataForSubDepoAdjustDetail` | `SubDepoAdjustDAL.SaveDataForSubDepoAdjustDetail` :28 | concatenated | `tblSubDepoStockAdjustDetail.SubDCStoreId` |
| chalanGridView cell[2] (Transfer Qty) | `Quantity` | :135 | same | same | concatenated | `tblSubDepoStockAdjustDetail.Quantity`; also `tblSubDepotStore.StockQty = StockQty - Quantity` (`UpdateSubDepoStockQuantity` :327) |
| header id | `SubDepoStockOutId` | :136 | - | - | - | `tblSubDepoStockAdjustDetail.SubDepoStockOutId` |
| subdeportDropDownList2 (sub-depot) | - | not persisted | - | - | - | none - the sub-depot is only used to filter the product search |

Columns of `tblSubDepoStockAdjustment` written: SubDepoStockOutId, SubDepoStockCode, EntryBy, EntryDate. Columns of `tblSubDepoStockAdjustDetail` written: SubDCStoreId, SubDepoStockOutId, Quantity (`SubDepoAdjustDAL.cs:24-25,30-37`). No reason/status/approval columns are written.

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Display / value | Tables |
|---|---|---|---|---|
| salescenterDropDownList1 | `Page_Load` -> `RequisitionBLL.DCLoad(ddl)` `StockOutSubDepot.aspx.cs:26` | `select ComUnitId,(ComUnitCode+':'+ComUnitName) as Com from tblCompanyUnit` (`RequisitionDAL.cs:35-39`) | Com / ComUnitId | tblCompanyUnit (no user filter) |
| subdeportDropDownList2 | DC change -> `RequisitionBLL.SubdeportLoad` :450 | `select SubDepotId,(SubDepotCode+':'+SubDepotName) as Com from tblSubDepot WHERE ComUnitId=@ComUnitId` (`RequisitionDAL.cs:45-52`) | Com / SubDepotId (+ default item) | tblSubDepot |
| productDropDownList | `Page_Load` -> `RequisitionBLL.ProductLoad` :24 | `SELECT (ProductCode+':'+ProductName)Pro,* FROM tblProduct` (`RequisitionDAL.cs:40-44`) | Pro / ProductId | tblProduct |
| manufacDropDownList (hidden) | `SubDepoAdjustBLL.LoadManufac` :23; SelectedIndex=1 :25 | `SELECT * FROM tblManufacturer` (`SubDepoAdjustDAL.cs:161-165`) | ManufacName / ManufacId | tblManufacturer |

## 10. Button Action Mapping
| Button | Event / JS | Handler (line) | BLL -> DAL | SQL | DB change |
|---|---|---|---|---|---|
| Button1 `Search` | postback | `Button1_Click` :279 | `SubDepoAdjustBLL.GetProductDcStoreSubdeport` -> `SubDepoAdjustDAL.GetProductDcStoreSubdeport` :348 | inline SELECT `tblSubDepotStore` LEFT JOIN `tblProduct`,`tblSubDepot`,`tblUnitPrice` WHERE `SubDepotId='<id>' AND ProductId='<id>' AND tblUnitPrice.IsActive='True' AND StockQty>0 order by ProductCode` (string concatenation) | read only |
| addButton `Add to List` | postback | `addButton_Click` :306 | none | none | none (in-page DataTable) |
| DeleteImageButton `Remove Item` | postback | `DeleteImageButton_Click1` :498 | none | none | none |
| submitButton `Submit` | postback (no JS confirm) | `submitButton_Click1` :117 | `SubDepoAdjustBLL.SaveDataForSubDepoAdjust` -> `SubDepoAdjustDAL.SaveDataForSubDepoAdjust`; then per line `SaveDataForSubDepoAdjustDetail` -> `SubDepoAdjustDAL.SaveDataForSubDepoAdjustDetail` + `UpdateSubDepoStockQuantity` | INSERT tblSubDepoStockAdjustment; INSERT tblSubDepoStockAdjustDetail; `UPDATE tblSubDepotStore SET StockQty=StockQty-<qty> WHERE SubDCStoreId='<id>'` | 3 tables written (see §17) |
| Button2 `Print` | postback + `window.open` | `Button2_Click` :405 | none | none (viewer runs `ChalanDAL.SubDepotChalanReport`) | none |

## 11. Frontend Flow
- `Page_Load` (first load only): `ChalanId()` shows next `SDC` number (unused), `Todate()`, loads manufacturer (hidden), product list, selects manufacturer index 1, loads DC list (:17-29).
- DC change -> re-binds sub-depot list and copies the DC code into hidden `fromComUnitCodeTextBox` (looks up `tblCompanyUnit`); sub-depot change -> fills name/address text boxes from `tblSubDepot` (message `Sub-Depot Information Not Found!!` and clears boxes if missing).
- Product change fires the DC handler (BR-M12-047) so the sub-depot list is reset.
- `Search` fills `productGridView`; `chkSelectAll` toggles rows; qty TextChanged validates qty <= stock (BR-M12-007); `Add to List` copies ticked + qty rows into `chalanGridView` (BR-M12-006).
- Page script: only `pageLoad()` (select2 initialisation, `StockOutSubDepot.aspx:63-75`). `showMessageBox` emits `alert('...')` via `ScriptManager.RegisterStartupScript`. No AJAX endpoints, PageMethods or modals.
- No client-side totals are computed.

## 12. Backend Flow
| Handler | Source | Flow | try/catch | Txn |
|---|---|---|---|---|
| `submitButton_Click1` | `StockOutSubDepot.aspx.cs:117-143` | new DAO (EntryBy=Session LoginName, EntryDate=Now) -> `SaveDataForSubDepoAdjust(dao,out id)` (id = MAX+1) -> **if id>0 (always)**: loop `chalanGridView` rows -> `SubDepoAdjustDetailDAO{SubDCStoreId=DataKeys[i][0], Quantity=Cells[2].Text (Convert.ToDecimal), SubDepoStockOutId=id}` -> `SaveDataForSubDepoAdjustDetail(list)` -> for each detail: INSERT then, if insert returned true, `UpdateSubDepoStockQuantity` -> `Clear()` -> alert `Stock Out Successfully` | no | no (each statement autocommits; `SaveDataByInsertCommand` rethrows, `UpdateDataByUpdateCommand` swallows) |
| `Button1_Click` | :279-291 | search -> bind or alert `No Product Found!!` | no | no |
| `addButton_Click` / `DeleteImageButton_Click1` | :306-360 / :498-540 | rebuild `chalanGridView` from DataTable | no | no |
| `dQtyTextBox_TextChanged` | :414-446 | loops ALL product rows (the `chkSelect` test is commented out), qty>stock => clear + alert | no | no |
| DC / sub-depot / code TextChanged handlers | :236-277,448-496 | look-ups in `tblCompanyUnit` / `tblSubDepot` | no | no |

All SQL goes through `ClsCommonInternalDAL.SaveDataByInsertCommand/UpdateDataByUpdateCommand`, which wrap the statement text in the stored procedure `ExecuteAllSqlQueryByStoreProcedure(@Query)` (`ClsCommonInternalDAL.cs:397-421,465-490`). Its body is not in the repo (UNKNOWN).

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| SubDepoAdjustBLL.LoadManufac | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:85 |
| RequisitionBLL.ProductLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:143 |
| RequisitionBLL.DCLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:152 |
| SubDepoAdjustBLL.ChalanNoForShow | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:118 |
| SubDepoAdjustBLL.ChalanNoGenerator | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:125 |
| SubDepoAdjustBLL.SaveDataForSubDepoAdjust | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:20 |
| SubDepoAdjustBLL.SaveDataForSubDepoAdjustDetail | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:27 |
| SubDepoAdjustBLL.ComUnit | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:154 |
| SubDepoAdjustBLL.LoadSubDepot | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:158 |
| SubDepoAdjustBLL.GetProductDcStoreSubdeport | Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:171 |
| RequisitionBLL.SubdeportLoad | Library.BLL/SInventory_BLL/RequisitionBLL.cs:157 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| SubDepoAdjustDAL.LoadManufac | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:162 | inline SQL: tblManufacturer |  |
| RequisitionDAL.ProductLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:41 | inline SQL: tblProduct |  |
| RequisitionDAL.DCLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:36 | inline SQL: tblCompanyUnit |  |
| RequisitionDAL.BindDropDown | Library.DAL/SInventory_DAL/RequisitionDAL.cs:761 | delegates / no SQL literal |  |
| SubDepoAdjustDAL.SaveDataForSubDepoAdjust | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:23 | inline SQL: tblSubDepoStockAdjustment |  |
| SubDepoAdjustDAL.SaveDataForSubDepoAdjustDetail | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:29 | inline SQL: tblSubDepoStockAdjustDetail |  |
| SubDepoAdjustDAL.UpdateSubDepoStockQuantity | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:328 | inline SQL: tblSubDepotStore |  |
| SubDepoAdjustDAL.LoadComUnit | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:303 | inline SQL: tblCompanyUnit |  |
| SubDepoAdjustDAL.LoadSubDepot | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:310 | inline SQL: tblSubDepot |  |
| SubDepoAdjustDAL.GetProductDcStoreSubdeport | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:349 | inline SQL: tblProduct, tblSubDepot, tblSubDepotStore, tblUnitPrice |  |
| RequisitionDAL.SubdeportLoad | Library.DAL/SInventory_DAL/RequisitionDAL.cs:46 | inline SQL: tblSubDepot | @ComUnitId |

## 16. Stored Procedure Mapping
| SP | Purpose | Read/Write | Body in repo |
|---|---|---|---|
| `ExecuteAllSqlQueryByStoreProcedure` (param `@Query`) | executes the inline INSERT/UPDATE strings built in `SubDepoAdjustDAL` (dynamic SQL wrapper) | write | NO - UNKNOWN (`sp_bodies.json` has no entry) |

No business SP is called by this page.

## 17. Database Mapping
| Table | Access | Columns / effect | Source |
|---|---|---|---|
| tblSubDepotStore | READ (search) / WRITE | `StockQty = StockQty - <qty>` per adjusted batch row (`SubDCStoreId` key) | `SubDepoAdjustDAL.cs:327-331,348-361` |
| tblSubDepoStockAdjustment | WRITE (INSERT) | SubDepoStockOutId, SubDepoStockCode, EntryBy, EntryDate | `SubDepoAdjustDAL.cs:22-27` |
| tblSubDepoStockAdjustDetail | WRITE (INSERT) | SubDCStoreId, SubDepoStockOutId, Quantity | `SubDepoAdjustDAL.cs:28-39` |
| tblSubDepot | READ | code, name, address, parent | `SubDepoAdjustDAL.cs:309-315` |
| tblCompanyUnit, tblProduct, tblManufacturer, tblUnitPrice | READ | look-ups, price join | as above |

Not written: any freeze table, cost/ledger row, journal. Column list of the three adjustment/store tables in the DB is otherwise UNKNOWN (no DDL in repo).

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | Message | BR |
|---|---|---|---|---|---|
| Transfer Qty | digits only | FilteredTextBoxExtender | no | (keystroke filtered) | BR-M12-007 |
| Transfer Qty | <= Stock Qty | TextChanged post-back | **no re-check on submit** | `Transfer Qty. cantbe more then Stock Quantity` | BR-M12-007 |
| Product search | at least one row | - | yes | `No Product Found!!` | BR-M12-021 |
| Add list | ticked row with non-empty qty | - | yes (silent skip) | - | BR-M12-006 |
| Whole form | chalanNo/date/manufacturer/from/to/list count | - | `Validation()` exists (:78-116) but is **never called** | `Please Insert chalanNo!!`, `Please Insert chalanDate!!`, `Please Select Manufacturer!!`, `Please Select fromComUnit!!`, `Please Select toComUnit!!`, `Please Add Product!!` | BR-M12-022 |
| Sub-depot / DC look-up | code must exist | - | yes | `Company Unit Information Not Found!!` / `Sub-Depot Information Not Found!!` | - |
| Positive/zero qty, decimals, negative resulting stock | not validated | no | no | - | BR-M12-022 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M12-001 | A sub-depot is registered under exactly one parent DC (tblSubDepot.ComUnitId). Every sub-depot dropdown is filtered by the DC chosen just before it (`WHERE ComUnitId=@ComUnitId`, label `SubDepotCode:SubDepotName`, value SubDepotId, default item `--------Select---------`). | Library.DAL/SInventory_DAL/RequisitionDAL.cs:45-52,761-770; Library.DAL/SubDepot_DAL/SubDepotDAL.cs:15-19 | RequisitionDAL.SubdeportLoad | CONFIRMED |
| BR-M12-004 | Sub-depot documents use the text prefix `SDC` + 6-digit zero-padded (MAX(id)+1): DC->Sub-depot chalan (tblSubDepotChalanInfo.SChalanId), Sub-depot->DC return chalan (tblSubDepotChalanReturnInfo.SChalanId) and the direct stock-adjustment code (tblSubDepoStockAdjustment.SubDepoStockOutId). Padding covers ids of 1-3 digits only; ids >= 1000 are not padded (`SDC1000`). The three counters are independent, so the same `SDC00000n` text can exist in three tables (and both chalan tables). | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:19-25,103-128; Library.BLL/SubDepot_BLL/SubDepotChalanReturnBLL.cs:19-25,111-136; Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:19-25,124-142; Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:29-45 | ChalanNoGenerator / ClsPrimaryKeyFind.PrimaryKeyMax (SELECT ISNULL(MAX(col),0)+1) | CONFIRMED |
| BR-M12-006 | Add-to-list keeps only rows whose checkbox is ticked AND whose Transfer Qty is non-empty; a DCStoreId already in the chalan list is silently not added again (HasDCStoreId). Previously listed lines are re-added from the grid text/DataKeys on every click. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:267-334; StockTransferSubDepottoDc.aspx.cs:267-334 | addButton_Click / HasDCStoreId | CONFIRMED |
| BR-M12-007 | Transfer quantity accepts digits only (FilteredTextBoxExtender ValidChars 0-9). A post-back on the textbox compares each row's qty to its Stock Qty cell and, if greater, clears the box with `Transfer Qty. cantbe more then Stock Quantity`. The comparison is client-triggered only - it is NOT repeated on Submit, so a stale/edited value can exceed stock. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx.cs:388-420 and .aspx:315-323; StockTransferSubDepottoDc.aspx.cs:388-420; StockOutSubDepot.aspx.cs:414-446 | dQtyTextBox_TextChanged | CONFIRMED |
| BR-M12-021 | Direct sub-depot stock adjustment (Stock Adjustment / StockOutSubDepot): product search reads tblSubDepotStore of the chosen sub-depot with StockQty>0 and active unit price; Submit writes a header (tblSubDepoStockAdjustment: id, code `SDC`+id, EntryBy = Session LoginName, EntryDate = now), one tblSubDepoStockAdjustDetail per line (SubDCStoreId, qty) and immediately reduces `tblSubDepotStore.StockQty = StockQty - qty`. There is no approval step, no reason field and the chosen sub-depot is not stored on the header. | Solution.Web/SubDepot_UI/StockOutSubDepot.aspx.cs:117-143; Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:19-39; Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:22-39,327-331,348-361 | StockOutSubDepot.submitButton_Click1 -> SubDepoAdjustBLL | CONFIRMED |
| BR-M12-022 | StockOutSubDepot Submit does not call its own Validation(): with an empty list a header row is still inserted; the header id is `MAX+1` so `id>0` is always true and `Stock Out Successfully` is shown even when a detail/UPDATE silently fails (UPDATE errors are swallowed). Qty can exceed stock at submit (see BR-7); `StockQty` can go negative. | Solution.Web/SubDepot_UI/StockOutSubDepot.aspx.cs:78-116 (unused),117-143; Library.BLL/SubDepot_BLL/SubDepoAdjustBLL.cs:26-39 | submitButton_Click1 | CONFIRMED |
| BR-M12-045 | Sub-depot receive and stock-report pages branch on Session UserType: Admin sees all DCs, others only DCs assigned in tblUserCompanyUnit (DCLoad(ddl,userId)). The transfer, adjustment and invoice pages use the unfiltered DC list (`select ... from tblCompanyUnit`) so any logged-in user may pick any DC. | Solution.Web/SubDepot_UI/StockReceiveBySubDepot.aspx.cs:16-31; SubDeportStockReport.aspx.cs:11-27; Library.DAL/SInventory_DAL/RequisitionDAL.cs:35-39,53-64; StockTransferDcToSubDepot.aspx.cs:25 | Page_Load DC dropdown loaders | CONFIRMED |
| BR-M12-047 | Selecting a product on DC->Sub-depot and Stock Adjustment pages fires the DC dropdown handler (`onselectedindexchanged=salescenterDropDownList1_SelectedIndexChanged`), which rebinds and resets the Sub-depot dropdown to `--------Select---------`; the sub-depot must be chosen after the product or it is lost. | Solution.Web/SubDepot_UI/StockTransferDcToSubDepot.aspx:152-158; StockOutSubDepot.aspx:147-149; StockOutSubDepot.aspx.cs:448-473 | productDropDownList markup | CONFIRMED |

Calculations: none on this page (no price/VAT/totals are used; `VATAmountPerUnit`/`UnitPrice` DataKeys are carried but never read on submit).

## 20. Permission Matrix
- Session keys read: `LoginName` (`StockOutSubDepot.aspx.cs:121`). No `UserId`/role/`UserType` check in code.
- Authentication: Forms auth; `NewMasterPage.master` redirects to Login when `Session["UserId"]` is null.
- Authorization: menu-based only (`tblMainMenu`/`tblMenuDistribution` per role; not in repo). Any authenticated user who reaches the URL can adjust the stock of any DC's sub-depot (DC list is not user-restricted - BR-M12-045).
- No approval gate (contrast `SubDepotStockOutApproval`).

## 21. Status Lifecycle
There is no status column on this page's records. Lifecycle of an adjustment: **created = applied** in one click:
`(none) -> header+details inserted -> tblSubDepotStore.StockQty reduced`. No reversal, edit, cancel, approval or status flag exists in `tblSubDepoStockAdjustment` as written by the code (columns SubDepoStockOutId/Code/EntryBy/EntryDate only). Stock quantity thresholds are not checked, so `StockQty` can become negative (BR-M12-022).

## 22. Error Handling
- No try/catch in any handler of the page. `SaveDataByInsertCommand` rethrows DB errors (`throw ex`) => unhandled ASP.NET error page; `UpdateDataByUpdateCommand` returns false and swallows the exception, so a failed stock UPDATE is invisible.
- `Convert.ToInt32/ToDecimal` on grid text (`Cells[2].Text`) throws `FormatException` for non-numeric text or `&nbsp;`.
- User messages via JS `alert`. `Application_Error` in `Global.asax` is empty (no central logging).
- The success message `Stock Out Successfully` is displayed whenever `id>0`, which is always true (BR-M12-022).

## 23. Audit
Audit columns written: `tblSubDepoStockAdjustment.EntryBy` (Session LoginName) and `EntryDate` (server time). The detail table has no user/time. No `tblLoginLog`-style trace of the stock change and no before/after quantity is stored. Other audit columns in the tables (if any) are UNKNOWN.

## 24. Integration
None. No SAP/mobile/email integration. Uses the shared DB helper `ExecuteAllSqlQueryByStoreProcedure` (see §12).

## 25. Reports
No report is produced by the adjustment itself. The `Print` button opens `SInventory_RPTVIEW/DcToSubdepotChalanReportViewer.aspx?chalanno=<printChalanNoTextBox>` (chalan report `rptDcToDcChalan.rpt`, see RPT-M12-01) but `printChalanNoTextBox` is never populated by this page, so the viewer receives an empty `chalanno` and shows `No Data Found!!`.

## 26. Dependencies
BLL: `RequisitionBLL`, `SubDepoAdjustBLL`. DAL: `RequisitionDAL`, `SubDepoAdjustDAL`, `ClsCommonInternalDAL`, `ClsPrimaryKeyFind`. DAO: `SubDepoAdjustMentDAO`, `SubDepoAdjustDetailDAO`. Tables: see §17. Controls: AjaxControlToolkit `FilteredTextBoxExtender`, select2/pickadate JS. Code duplicated in `SubDepotChalanDAL`/`SubDepotChalanReturnDAL` (same query text) - GAP-M12-018.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-01 | Search stock | DC, sub-depot chosen, product chosen, Search | grid lists only `tblSubDepotStore` rows of that sub-depot with StockQty>0 | BR-M12-021 |
| TC-02 | Product change resets sub-depot | choose sub-depot then re-select a product | sub-depot dropdown returns to `--------Select---------`; Search says `No Product Found!!` | BR-M12-047 |
| TC-03 | Qty greater than stock | tick a row, enter qty > stock | box cleared + alert `Transfer Qty. cantbe more then Stock Quantity` | BR-M12-007 |
| TC-04 | Positive adjust | tick row, qty 5 (stock 20), Add, Submit | header+detail inserted, store row StockQty 15, alert `Stock Out Successfully`, code `SDC00000n` | BR-M12-004, 021 |
| TC-05 | Empty list submit (negative) | Submit with nothing added | header row still inserted, no detail, success alert shown (defect) | BR-M12-022 |
| TC-06 | Edge: same batch added twice | tick same row after it is already listed | not added twice | BR-M12-006 |
| TC-07 | Edge: stale qty after concurrent stock change | qty valid at entry, stock reduced elsewhere before Submit | no server check - stock can go negative | BR-M12-022 |
| TC-08 | Non-numeric/zero qty | type 0 or leave blank on ticked row | blank rows skipped; 0 accepted and written | BR-M12-006 |
| TC-09 | Failed UPDATE | simulate DB error on stock UPDATE | no error shown; success alert (swallowed) | BR-M12-022 |
| TC-10 | No session | open without login | redirected to Login (master page) | - |

## 28. Reverse Engineering References
- `Solution.Web/SubDepot_UI/StockOutSubDepot.aspx`, `Solution.Web/SubDepot_UI/StockOutSubDepot.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | `StockOutSubDepot.aspx(.cs)` | n/a | MISSING | - |
| Purpose | None existed | direct write-off, no approval | tables not in repo | INFERRED | S2 |
| Validation | None existed | `Validation()` defined but not called | - | GAP | BR-M12-022 |
| Stock effect | None existed | `StockQty=StockQty-qty` in place | UNKNOWN (no DDL) | CONFIRMED (code) | BR-M12-021 |
| Approval | None existed | absent (vs voucher page) | - | GAP | GAP-M12-006 |

## 30. Known Gaps
- Copy of `StockTransferDcToSubDepot.aspx` (same partial class name `SubDepot_UI_StockTransferDcToSubDepot` - two pages `Inherits` the same class; GAP-M12-004) with an unrelated chalan block left on the page.
- `Validation()` not wired; no reason; no approval; qty can exceed stock or go negative; header created even when no lines (BR-M12-022).
- Header does not record sub-depot; only derivable through the detail's SubDCStoreId.
- SQL built by string concatenation (SQL-injection surface: only DropDown ids and grid values); `SubDepoAdjustDAL` duplicated in two other DAL classes.
- Hard-coded default: manufacturer index 1 (hidden).
- Product dropdown wired to the DC handler (BR-M12-047).

## 31. Change Impact
- Shared code: `SubDepoAdjustBLL`/`SubDepoAdjustDAL` are used only by this page (grep `SubDepoAdjust`); `RequisitionBLL.DCLoad/ProductLoad/SubdeportLoad` are shared by all M12 transfer/receive pages and many main-depot pages (`RequisitionDAL.cs:35-52`).
- Changing `SubDepoAdjustDAL.UpdateSubDepoStockQuantity` changes every stock deduction of this page only.
- `tblSubDepotStore` is also decremented by `SubDepotChalanReturnBLL` (Sub->DC), read by the stock report (`DCStockReportDAL.cs:485-490`) and by the freeze-release page - any quantity semantic change affects them (WF-M12-02/04).

