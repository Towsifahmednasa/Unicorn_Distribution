---
page_id: PAGE-385
module: M10
page: WarehouseStockIn
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-385 — WarehouseStockIn

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-385 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | WarehouseStockIn |
| Kind | Page (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_WarehouseStockIn |
| aspx size | 29327 bytes |
| Code-behind LOC | 978 |
| Inline script size | 1253 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED from code/labels):** Data-entry form where warehouse staff record goods received into the Central Warehouse (CW) against a supplier challan: header (manufacturer, supplier, stock-in date, challan no/date, reference no/date, remarks) plus product lines (product, batch, mfg/exp date, quantity, unit price, VAT per unit). Saving creates a `Posted` stock-in document that must later be approved on `WarehouseStockInApproval.aspx`. The same page is re-used as the edit form when opened from `WarehouseStockInView.aspx` (session key `IDInformation`).
Source: `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs:22-45,443-513,614-705`.

## 3. Business Objective
Give the warehouse a controlled, auditable record of every inbound receipt (what, which batch, expiry, cost and VAT) before it becomes available central stock, with a maker/checker split (entry here, approval elsewhere). INFERRED from the `Status='Posted'` -> `Approved` workflow (`WarehouseStockIn.aspx.cs:469`, `WarehouseStockInApprovalDal.cs:75-76`). The page itself does **not** change any stock quantity (see BR-M10-009).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/WarehouseStockInView.aspx`

Outbound references: `warehousestockinview.aspx`

## 5. Route
`~/SInventory_UI/WarehouseStockIn.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/WarehouseStockIn.aspx |
| Code-behind | Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | WarehouseStockInBll (Library.BLL/SInventory_BLL/WarehouseStockInBll.cs) |
| DAL class | CommonDataLoad (Library.DAL/DoctorModule_DAL/CommonDataLoad.cs) |
| DAL class | WarehouseStockInDal (Library.DAL/SInventory_DAL/WarehouseStockInDal.cs) |
| DAO/entity class | WarehouseStockInMasterDao (Library.DAO/SInventory_Entities/WarehouseStockInMasterDao.cs) |
| DAO/entity class | WharehouseStockInDetailsDao (Library.DAO/SInventory_Entities/WharehouseStockInDetailsDao.cs) |

## 7. UI Control Inventory
Only controls with behaviour verified in code are listed; purely cosmetic controls and extender companions are omitted (re-derive from the `.aspx`). **No ASP.NET validators exist on the page** (0 `*Validator` tags); all validation is server-side in `Validation()` (`WarehouseStockIn.aspx.cs:515-611`) or in `OnTextChanged` handlers.

| Control | Label | Required (server) | Default | Read-only / visible | Data source | Error message |
|---|---|---|---|---|---|---|
| manufacturerDropDownList | Manufacturer | Yes (`SelectedValue==""`) | `SelectedIndex=1` on first load (`:43`) | - | `select * from tblManufacturer` (`WarehouseStockInDal.cs:16-20`) | `Please Select Manufacturer !!!` |
| supplierDropDownList | Supplier | Yes | first item | - | `SELECT * FROM dbo.tblSupplierInformation` | `Please Select Supplier !!!` |
| stockInDateTextBox | Stock In Date | Yes | empty | OnTextChanged validates date (`TryParse`) | user | `Please insert stock in date !!!` / `Please insert valid date` |
| challanNoTextBox | Challan No | Yes | empty | - | user | `Please insert challan number !!!` |
| challanDateTextBox | Challan Date | Yes | empty | date check on change | user | `Please insert challan date !!!` |
| referenceNoTextBox / referenceDateTextBox | Reference No/Date | No | empty | date check on change | user | `Please insert valid date` |
| remarksTextBox | Remarks | No | empty | - | user | - |
| stockInIdHiddenField | (hidden) | - | "" (new) or master id (edit) | - | `Session["IDInformation"]` | - |
| productGridView row: productCodeTextBox | P.Code | Yes | empty | `ReadOnly=true` (aspx) | autocomplete "code:name" -> `productCodeTextBox_TextChanged` splits on `:` | `Please fill out product code!!` |
| productNameTextBox | Product Name | - | - | AutoComplete; on change without `:` shows `Input Correct Data!!` | `GetProductInGrid` | `Input Correct Data!!` |
| uomTextBox / packSizeTextBox | UOM / PackSize | - | filled from product | ReadOnly | `tblStockUOM.StockUOMName`, `tblProduct.PackSize` | - |
| batchTextBox | Batch | Yes | empty | - | user | `Please fill out batch!!` |
| mfgDateTextBox | Mfg.Date | Yes | empty | on change: exp date := mfg + 2 years | user | `Please fill out Mfg. Date!!` |
| expDateDateTextBox | Exp.Date | Yes | auto | on change must be >= mfg | user | `Please fill out exp. Date!!`, `Date Error!!`, `Please Insert Manufacturing Date!!` |
| reqQtyTextBox | Qty | Yes | empty | FilteredTextBoxExtender | user | `Please fill out Req.Qty!!` |
| costPriceTextBox | Price | **No** (check commented out `:594-599`) | `tblUnitPrice.CostPrice` | ReadOnly=True | product lookup | - |
| vatTextBox | Vat (per unit) | Yes | `tblUnitPrice.VATAmountPerUnit` | ReadOnly=True | product lookup | `Please fill out vat!!` |
| totalValueTextBox | Total Amt | - | qty*price+qty*vat | ReadOnly | computed | - |
| totalQtyTextBox / totalVatTextBox / grandTotalTextBox | totals | - | computed | ReadOnly | computed | - |
| submitButton | Save / Update | confirm dialog `Are you sure you want to Save ?` | text switches to `Update` in edit mode (`:143`) | **hidden if `sp_GET_btnShow` returns rows** (`:56-60`) | - | - |

## 8. Field Mapping
Field-to-column mapping (CONFIRMED; DAL builds inline SQL by string concatenation and sends it through the SP `ExecuteAllSqlQueryByStoreProcedure`, body absent).

| UI control | DTO property | Handler | BLL | DAL | Table.Column |
|---|---|---|---|---|---|
| (generated) | `WHStockInMasterID` = MAX+1 | `SaveAllData` | `WarehouseStockInBll.StockInId` -> `ClsPrimaryKeyFind.PrimaryKeyMax` | `SaveStockInMasterInformation` | `tblWHStockInMaster.WHStockInMasterID` |
| (generated) | `WHStockInCode` = `WHS-`+7-digit | `SaveAllData` | `WHStockInCode()` | `WarehouseStockInCodeId` | `tblWHStockInMaster.WHStockInCode` |
| stockInDateTextBox | `WHStockInDate` | `SaveAllData` `:449` | - | `SaveStockInMasterInformation` | `WHStockInDate` |
| manufacturerDropDownList | `ManufacId` | `:450` | - | same | `ManufacId` |
| supplierDropDownList | `SupplierId` | `:451` | - | same | `SupplierId` |
| totalVatTextBox / totalQtyTextBox / grandTotalTextBox | `TotalVat` / `TotalQuantity` (Int32) / `TotalValue` | `:452-454` | - | same | `TotalVat`, `TotalQuantity`, `TotalValue` |
| challanNoTextBox / challanDateTextBox | `ChallanNo` / `ChallanDate` | `:455-456` | - | same | `ChallanNo`, `ChallanDate` |
| referenceNoTextBox / referenceDateTextBox | `ReferenceNo` / `ReferenceDate` (string) | `:457-466` | - | same | `ReferenceNo`, `ReferenceDate` |
| remarksTextBox | `Remarks` | `:468` | - | same | `Remarks` |
| (constant) | `Status="Posted"` | `:469` | - | same | `Status` |
| Session["LoginName"] / DateTime.Now | `EntryBy` / `EntryDate` | `:470-471` | - | same | `EntryBy`, `EntryDate` |
| productidHiddenField | `ProductId` | `:496` | `SaveWarehouseStockInDetail` | `SaveWarehouseStockInDetail` (`:136-153`) | `tblWHStockInDetail.ProductId` |
| batchTextBox / expDate / mfgDate | `Batch` / `ExpDate` / `MfgDate` | `:497-499` | - | same | `Batch`, `ExpDate`, `MfgDate` |
| reqQtyTextBox / costPrice / vat / totalValue | `Qty` / `Price` / `VAT` / `TotalAmount` | `:500-503` | - | same | `Qty`, `Price`, `VAT`, `TotalAmount` |
| (edit) stockInIdHiddenField | `WHStockInMasterID` | `UpdateAllData` `:634` | `UpdateStockInMasterInfo` | `UpdateStockInMasterInfo` (`:236-254`) | `WHStockInMasterID` (WHERE) |
| (edit) Session LoginName / Now | `UpdateBy` / `UpdateDate` | `:664-665` | - | same | `UpdateBy`, `UpdateDate` |

Note: the DAL `UpdateStockInMasterInfo` starts with `SET ManufacId ='" + "' ,ChallanNo = ...` (`WarehouseStockInDal.cs:238-239`), i.e. ManufacId is set to an empty string and `WHStockInDate` is never written by the update (see GAP-M10-004).

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Text / Value | Table |
|---|---|---|---|---|
| manufacturerDropDownList | `DropDownLoad` -> `WarehouseStockInBll.LoadmanufacturerName` -> `WarehouseStockInDal.LoadmanufacturerName` (`:16-20`) | `select * from tblManufacturer` (via `ClsCommonInternalDAL.LoadDropDownValueWithoutDataBase`) | `ManufacName` / `ManufacId` | tblManufacturer |
| supplierDropDownList | `WarehouseStockInBll.LoadSupplier` -> `WarehouseStockInDal.LoadSupplier` (`:21-25`) | `SELECT * FROM dbo.tblSupplierInformation` | `SupplierName` / `SupplierId` | tblSupplierInformation |
| (autocomplete) product code/name | `productNameTextBox_AutoCompleteExtender` (service path in `.aspx`); lookup on select = `WarehouseStockInDal.LoadProductCode` (`:31-38`): `SELECT * FROM tblProduct LEFT JOIN tblUnitPrice ON ProductId LEFT JOIN tblStockUOM ... WHERE LTRIM(RTRIM(ProductCode))='<code>'` - **no `IsActive` filter on tblUnitPrice**, string-concatenated | code:name | tblProduct, tblUnitPrice, tblStockUOM |

## 10. Button Action Mapping
| Button | Handler (file:line) | What it does | DB change |
|---|---|---|---|
| ImageButton1 (add row) | `ImageButton1_Click` `:299` -> `AddRowInGrid` `:197` | Rebuilds the grid from current textbox values + one blank row; recalculates totals | none |
| ImageButton2 (remove row) | `ImageButton2_Click` `:303` | Rebuilds grid without the clicked row; re-inits grid if empty | none |
| submitButton | `submitButton_Click` `:614` -> `Validation()` -> `SaveAllData` (new) or `UpdateAllData` (when `stockInIdHiddenField` non-empty) | Insert master then loop-insert detail rows (or update master, delete all details, re-insert) | INSERT `tblWHStockInMaster`, INSERT `tblWHStockInDetail` x N; edit: UPDATE master, DELETE details |
| cancelButton | `cancelButton_Click` `:974` -> `Clear()` | resets form, reloads dropdowns, button text `Save` | none |
| viewLinkButton | `viewLinkButton_OnClick` `:853` | redirect `WarehouseStockInView.aspx` | none |

## 11. Frontend Flow
- **Init (`!IsPostBack`)**: `InitialGrid()` (one empty row), `DropDownLoad()`, `PermissionForShowBtn()`, then if `Session["IDInformation"]` is set -> load master+details into the form (`LoadWarehouseStockInInfo`), button text `Update`, session key cleared; finally `manufacturerDropDownList.SelectedIndex = 1` (`:24-44`).
- **Page-local JS**: `pageLoad()` (jQuery/date-picker bootstrap, cosmetic). No AJAX endpoints/PageMethods.
- **Product pick**: user types in code or name box (AutoCompleteExtender returns `code:name`); `OnTextChanged` postback splits on `:` and calls `GetProductInGrid` to fill product id (also `Session["ProductId"]`), name, pack size, UOM, price (= `CostPrice`) and VAT (= `VATAmountPerUnit`) (`:390-440,732-757`).
- **Calculations (server postback, no JS)**: `CalculateTotalValue(row)` = `qty*vat + qty*price`; `CalculateQuantityVatAndValue()` sums Int32 qty, `qty*vat`, and line totals (`:779-846`). Non-integer quantity throws `FormatException` in the Int32 parse (`:820`).
- **Date helpers**: Mfg change sets Exp = Mfg + 2 years formatted `dd MMMM, yyyy` (`:919-920`); Exp change must be >= Mfg else cleared with `Date Error!!` (`:956-965`).
- Update model: full postbacks inside UpdatePanel/ScriptManager; alerts via `ScriptManager.RegisterStartupScript` (`showMessageBox`).

## 12. Backend Flow
1. `Page_Load` (`:22-45`) - described in section 11.
2. `submitButton_Click` (`:614`): `Validation()`; if OK and `stockInIdHiddenField==""` -> `SaveAllData()` else `UpdateAllData()`.
3. `SaveAllData` (`:443-513`): build `WarehouseStockInMasterDao` (conversions `Convert.ToDateTime/Int32/Decimal` - **unguarded**), `Status="Posted"`, `EntryBy=Session["LoginName"]`; `WarehouseStockInBll.SaveStockInMasterInfo` -> `StockInId()` (`MAX(WHStockInMasterID)+1`) + `WHStockInCode()` (`MAX numeric suffix of 'WHS-%' + 1`, padded to 7 digits) -> `WarehouseStockInDal.SaveStockInMasterInformation`; then per grid row build `WharehouseStockInDetailsDao` and `SaveWarehouseStockInDetail` (foreach INSERT); `Clear()`; alert `Data Saved Successfully`.
4. `UpdateAllData` (`:630-705`): `UpdateStockInMasterInfo` -> `DeleteWhStockInDetailById` -> foreach `SaveWarehouseStockInDetail`; alert `Data Updated Successfully`.
5. **Transactions:** none - each statement is an independent call via `ClsCommonInternalDAL` (`SaveDataByInsertCommand`, `UpdateDataByUpdateCommand`, `DeleteDataByDeleteCommand` all execute the SP `ExecuteAllSqlQueryByStoreProcedure`). A failure mid-way leaves a master without (all) details or details deleted without re-insert.
6. **try/catch:** only `PermissionForShowBtn` (empty catch). Save/Update have none -> unhandled exception page.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WarehouseStockInBll.LoadmanufacturerName | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:19 |
| WarehouseStockInBll.LoadSupplier | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:23 |
| WarehouseStockInBll.LoadWarehouseStockInMasterInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:122 |
| WarehouseStockInBll.LoadWarehouseStockInDetailInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:128 |
| WarehouseStockInBll.ProductInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:32 |
| WarehouseStockInBll.SaveStockInMasterInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:37 |
| WarehouseStockInBll.StockInId | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:55 |
| WarehouseStockInBll.WHStockInCode | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:61 |
| WarehouseStockInBll.OrdnNoGenerator | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:67 |
| WarehouseStockInBll.SaveWarehouseStockInDetail | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:99 |
| WarehouseStockInBll.UpdateStockInMasterInfo | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:133 |
| WarehouseStockInBll.DeleteWhStockInDetailById | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:152 |
| WarehouseStockInBll.UpdateWarehouseStockInDetail | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:143 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| WarehouseStockInDal.LoadmanufacturerName | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:17 | inline SQL: tblManufacturer |  |
| WarehouseStockInDal.LoadSupplier | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:22 | inline SQL: tblSupplierInformation |  |
| WarehouseStockInDal.GetWarehouseStockInMasterInfo | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:199 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.GetWarehouseStockInDetailInfo | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:206 | inline SQL: tblProduct, tblStockUOM, tblWHStockInDetail |  |
| CommonDataLoad.GetPermissionForShowBtn | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:65 | sp_GET_btnShow | @PageName |
| WarehouseStockInDal.LoadProductCode | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:32 | inline SQL: tblProduct, tblStockUOM, tblUnitPrice |  |
| WarehouseStockInDal.SaveStockInMasterInformation | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:41 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.WarehouseStockInCodeId | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:176 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.SaveWarehouseStockInDetail | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:137 | inline SQL: tblWHStockInDetail |  |
| WarehouseStockInDal.UpdateStockInMasterInfo | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:237 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.DeleteWhStockInDetailById | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:280 | inline SQL: tblWHStockInDetail, tblWHStockInDetail |  |

## 16. Stored Procedure Mapping
| Stored procedure | Called from | Definition | Effect |
|---|---|---|---|
| `sp_GET_btnShow` (`@PageName`) | `CommonDataLoad.GetPermissionForShowBtn` (`Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:64-84`) | body NOT in repository | If it returns >=1 row, Save button is hidden. It receives only the page path (no user id) - how it scopes to a user/role is UNKNOWN. |
| `ExecuteAllSqlQueryByStoreProcedure` (`@Query`) | every `ClsCommonInternalDAL.*(string queryString, dbName)` call | body NOT in repository | Executes the concatenated SQL string. Effects = the SQL shown in DAL; error propagation UNKNOWN. |

## 17. Database Mapping
| Table | R/W | Columns / notes |
|---|---|---|
| tblWHStockInMaster | W (INSERT/UPDATE; DELETE via View page), R | WHStockInMasterID, WHStockInCode, ManufacId, SupplierId, WHStockInDate, TotalQuantity, TotalVat, TotalValue, ChallanNo, ChallanDate, ReferenceNo, ReferenceDate, Remarks, Status, EntryBy, EntryDate, UpdateBy, UpdateDate (approval: ApproveBy, ApproveDate; material conversion: WarehouseId, MaterialConversionMasterId) |
| tblWHStockInDetail | W, R | WHStockInDetailID (identity - UNKNOWN), WHStockInMasterID, ProductId, Batch, ExpDate, MfgDate, Qty, Price, VAT, TotalAmount (MC: MaterialConversionDetailId) |
| tblManufacturer, tblSupplierInformation | R | dropdowns |
| tblProduct, tblUnitPrice, tblStockUOM | R | product lookup (CostPrice, VATAmountPerUnit, PackSize, StockUOMName) |

No stock table (tblCentralStore, tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock) is written by this page.

## 18. Validation Matrix
| Field | Rule | Frontend | Backend | DB | Message | BR |
|---|---|---|---|---|---|---|
| Manufacturer | required | no | `Validation :517` | UNKNOWN | Please Select Manufacturer !!! | BR-M10-001 |
| Supplier | required | no | `:524` | UNKNOWN | Please Select Supplier !!! | BR-M10-001 |
| Stock-in date | required + parseable | no | `:531`; change handler `:868` | UNKNOWN | Please insert stock in date !!! / Please insert valid date | BR-M10-001 |
| Challan no / date | required | no | `:539,547` | UNKNOWN | Please insert challan number !!! / Please insert challan date !!! | BR-M10-001 |
| Line: product code, batch, exp, mfg, qty, vat | required per row | no | `:559-606` | UNKNOWN | Please fill out product code!! / batch!! / exp. Date!! / Mfg. Date!! / Req.Qty!! / vat!! | BR-M10-002 |
| Line: price | NOT required (commented out `:594-599`) | no | - | - | - | BR-M10-002 |
| Exp >= Mfg | date order | no | `expDateDateTextBox_OnTextChanged :956` | - | Date Error!! | BR-M10-006 |
| Numeric format of qty/vat/price/dates | none | FilteredTextBoxExtender for qty (client) | none server-side -> `FormatException` | - | (unhandled) | - |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-001 | A stock-in header cannot be saved without manufacturer, supplier, stock-in date, challan number and challan date. | `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs:515-553` | `Validation()` | CONFIRMED |
| BR-M10-002 | Every grid row must have product code, batch, expiry date, manufacturing date, quantity and VAT; unit price is optional (validation commented out). | `WarehouseStockIn.aspx.cs:555-609` (price check `:594-599` commented) | `Validation()` | CONFIRMED |
| BR-M10-003 | A new stock-in is stored with `Status='Posted'`; master id = `MAX(WHStockInMasterID)+1`; code = `WHS-` + 7-digit zero-padded (`MAX numeric suffix of codes LIKE 'WHS-%'` + 1). Not concurrency-safe (read-then-insert, no lock). | `WarehouseStockIn.aspx.cs:469`; `Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:36-96`; `Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:175-189`; `Library.DAL/InternalCls/ClsPrimaryKeyFind.cs:12-27` | `SaveAllData` | CONFIRMED |
| BR-M10-004 | Line total = qty x price + qty x VAT-per-unit; header TotalQuantity = sum of integer quantities; TotalVat = sum(qty x vat); TotalValue = sum of line totals. No rounding is applied. | `WarehouseStockIn.aspx.cs:779-846` | `CalculateTotalValue`, `CalculateQuantityVatAndValue` | CONFIRMED |
| BR-M10-005 | Selecting a product defaults unit price to `tblUnitPrice.CostPrice` and VAT to `tblUnitPrice.VATAmountPerUnit`, UOM from `tblStockUOM`. Lookup does not filter inactive price rows (first row in result order wins). | `WarehouseStockIn.aspx.cs:390-421`; `WarehouseStockInDal.cs:31-38` | `GetProductInGrid`, `LoadProductCode` | CONFIRMED |
| BR-M10-006 | Expiry date defaults to manufacturing date + 2 years and may not precede the manufacturing date; expiry cannot be entered before manufacturing date. | `WarehouseStockIn.aspx.cs:901-972` | `mfgDateTextBox_OnTextChanged`, `expDateDateTextBox_OnTextChanged` | CONFIRMED |
| BR-M10-007 | Editing an existing stock-in (opened from the View page via `Session["IDInformation"]`) updates the master, deletes ALL detail rows and re-inserts them, and resets status to `Posted` (re-approval needed). The update SQL blanks `ManufacId` and does not update `WHStockInDate`. | `WarehouseStockIn.aspx.cs:35-40,630-705`; `WarehouseStockInDal.cs:236-254,279-284` | `UpdateAllData` | CONFIRMED |
| BR-M10-008 | The Save/Update button is hidden when the SP `sp_GET_btnShow(@PageName)` returns at least one row for the page path. SP body and its per-user logic are not in the repository. | `WarehouseStockIn.aspx.cs:47-71`; `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:64-84` | `PermissionForShowBtn` | INFERRED (SP body UNKNOWN) |
| BR-M10-009 | Saving/updating a regular stock-in does NOT change any stock quantity; stock tables are untouched by this page. | whole file; DAL writes only `tblWHStockInMaster/Detail` | - | CONFIRMED |

## 20. Permission Matrix
- Forms authentication + master page redirect only. No `Session["UserRoleID"]`/role test in code. Session keys read: `LoginName` (EntryBy/UpdateBy), `IDInformation`, `ProductId` (written).
- The only per-page control is the DB-driven `sp_GET_btnShow` check that hides the Save button (BR-M10-008); the menu itself is DB-driven (`sp_GET_MainMenuRole2`). Direct URL access is not blocked by this page.

## 21. Status Lifecycle
Field `tblWHStockInMaster.Status` (string):
- `(none)` -> **`Posted`** : `SaveAllData :469` (new) or `UpdateAllData :663` (edit, re-post).
- `Posted` -> `Approved` / `Reject` : `WarehouseStockInApproval.aspx.cs:187,227` (see PAGE-386). `Reject` items reappear in the View list (`WarehouseStockInDal.GetWarehouseStockInData :191-196` = `Posted` OR `Reject`).
- `Approved` -> consumed by `WHStockInList`/`WarehouseStockOut` (`WHStockListDAL.LoadWHStock`: `Status='Approved'`).
- Delete from View page hard-deletes master+details with no status check - `WarehouseStockInDal.DeleteWhStockInInfoById :286-293`.

## 22. Error Handling
No try/catch around save/update; conversion errors (`Convert.ToDateTime("")`, non-integer qty) surface as unhandled exceptions. `showMessageBox` writes `alert('<msg>')`; the C# `message.Replace("'", "\'")` is effectively a no-op (escape is not a JS escape in a normal C# string), so apostrophes in a message would break the script. `PermissionForShowBtn` swallows all exceptions. No logging; `Global.asax` `Application_Error` is empty.

## 23. Audit
Audit columns written: `EntryBy` (=`Session["LoginName"]`), `EntryDate` (`DateTime.Now`) on insert; `UpdateBy`, `UpdateDate` on update; approval adds `ApproveBy`, `ApproveDate`. No history table; detail rows are deleted and re-inserted on edit, so line-level history is lost.

## 24. Integration
No integration from this page (SAP MIGO import is a separate flow: `ExcelUpForMIGODAL`, outside M10 scope). UNKNOWN whether SAP-received stock reaches `tblCentralStore` through the same stock-in tables.

## 25. Reports
The View/Approval pages open `SInventory_RPTVIEW/WHStockInReportViewer.aspx?reqId=<id>` (`crpWHStockIn.rpt`) - see RPT-M10-03. Not opened from this page.

## 26. Dependencies
BLL `WarehouseStockInBll`, `ClsPrimaryKeyFind`; DAL `WarehouseStockInDal`, `ClsCommonInternalDAL`, `CommonDataLoad`; DAO `WarehouseStockInMasterDao`, `WharehouseStockInDetailsDao`; DB SP `ExecuteAllSqlQueryByStoreProcedure`, `sp_GET_btnShow`; tables in section 17; peers `WarehouseStockInView`, `WarehouseStockInApproval`, `WarehouseStockInEdit`.

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-385-01 | Positive save | Fill manufacturer, supplier, dates, challan, one product with batch/mfg/qty/vat, Save + confirm | New master `Posted`, code `WHS-nnnnnnn` = previous max+1, N detail rows; alert "Data Saved Successfully" (**see GAP-M10-001: the header INSERT text is syntactically invalid; the real outcome may be a SQL exception**) | BR-M10-001/003 |
| TC-385-02 | Missing supplier | leave supplier blank | alert "Please Select Supplier !!!", no DB write | BR-M10-001 |
| TC-385-03 | Missing batch on row 2 | blank batch | alert "Please fill out batch!!" | BR-M10-002 |
| TC-385-04 | Price blank | product without CostPrice, price left blank | validation passes; `Convert.ToDecimal("")` in `SaveAllData :501` throws FormatException | BR-M10-002 |
| TC-385-05 | Expiry before mfg | mfg 01-Jan-2026, exp 01-Jan-2025 | exp cleared, "Date Error!!" | BR-M10-006 |
| TC-385-06 | Mfg auto-expiry | enter mfg 15-Mar-2026 | exp = 15 March, 2028 | BR-M10-006 |
| TC-385-07 | Totals | qty 10, price 5, vat 1 | line 60, TotalVat 10, grand 60 | BR-M10-004 |
| TC-385-08 | Edit round-trip | View -> Edit -> change qty -> Update | master updated, details replaced, status `Posted`, ManufacId blanked (defect) | BR-M10-007 |
| TC-385-09 | Stock unaffected | compare tblCentralStore before/after | no change | BR-M10-009 |
| TC-385-10 | Non-numeric qty | type `1.5` | totals postback throws (Int32 parse) | BR-M10-004 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/WarehouseStockIn.aspx`, `Solution.Web/SInventory_UI/WarehouseStockIn.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | - | - | MISSING | - |
| Save header | None existed | INSERT column list duplicated -> invalid SQL text | UNKNOWN (cannot be executed as written) | GAP | `WarehouseStockInDal.cs:42-70` |
| Approval effect on stock | None existed | Approval flips status only; central-store insert commented out | UNKNOWN whether trigger/SP posts stock | UNKNOWN | `WarehouseStockInApprovalDal.cs:77`, `WarehouseStockInApproval.aspx.cs:197-204` |
| Business rules | None existed | BR-M10-001..009 | n/a | DOCUMENTED | section 19 |

## 30. Known Gaps
- **GAP-M10-001 (High):** `SaveStockInMasterInformation` INSERT statement contains the column list twice (`WarehouseStockInDal.cs:42-70`, `... ReferenceNo,` then `( WHStockInMasterID, ...`), which is not valid T-SQL; the primary save path appears unable to succeed as committed. Runtime failure INFERRED (not executed).
- **GAP-M10-004:** `UpdateStockInMasterInfo` sets `ManufacId=''` and omits `WHStockInDate` (`WarehouseStockInDal.cs:236-254`); date values written with culture-dependent `DateTime.ToString()`.
- SQL injection surface: every DAL statement concatenates user text (`challanNoTextBox`, `remarksTextBox`, `batchTextBox`, product code) into SQL executed through `ExecuteAllSqlQueryByStoreProcedure` (`WarehouseStockInDal.cs:31-38,40-90,136-153,236-254`).
- No transaction across master + N detail statements; no evidence of a duplicate-challan check.
- IDs from `MAX()+1` (race).
- Page class name `SInventory_UI_WarehouseStockIn` is reused by `WarehouseStockOut.aspx` (`Inherits`), see GAP-M10-021.
- Stock effect after approval is not visible in code (BR-M10-009/-014).

## 31. Change Impact
- Shares DAL/BLL with `WarehouseStockOut` (product lookup `ProductInfo`), `MaterialConversionReceive` (`SaveStockInMasterInformationForMaterialConversion`, `DeleteWhStockInDetailById`), `WarehouseStockInEdit`, `WarehouseStockInView` (`GetWarehouseStockInData`, `DeleteWhStockInInfoById`).
- Tables `tblWHStockInMaster/Detail` are read by approval (PAGE-386), stock-out (PAGE-389), `WHStockInList`, stock-in reports (`WHStockInReportViewer`, `crpWHStockIn.rpt`), `DCStockReportDAL` joins (`MD.WHStockInDetailID = CS.MigoDetailID`, `DCStockReportDAL.cs:64,119,...`) and `ChallanReportViewDAL.cs:455`.
- Changing `WarehouseStockInCodeId` / code format affects both stock-in and (by copy) stock-out code generation (`WHStockListDAL.cs:37-41`).

