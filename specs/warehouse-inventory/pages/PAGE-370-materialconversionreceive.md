---
page_id: PAGE-370
module: M10
page: MaterialConversionReceive
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-370 — MaterialConversionReceive

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-370 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | MaterialConversionReceive |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_MaterialConversionReceive |
| aspx size | 34967 bytes |
| Code-behind LOC | 885 |
| Inline script size | 7890 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED):** Receives **finished goods produced in the manufacturing ERP** (material conversion / production batches) into the warehouse. The user picks a company (business unit) and production unit, the page lists un-received conversion output batches from the production database `CSTL_ERPDB_Unicorn`, the user selects lines, sets stock-in date, warehouse, challan and saves a `Posted` warehouse stock-in linked to the conversion (`MaterialConversionMasterId`, per-line `MaterialConversionDetailId`). Also used to edit (`?ID=` or `Session["IDInformation"]`). Source: `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs`, `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs`.

## 3. Business Objective
Bridge production output into distribution stock: avoid re-keying production batches, guarantee that a conversion line is received at most once (unless rejected), and pass through the same approval as other stock-ins (PAGE-386). INFERRED from the anti-join on `tblWHStockInDetail.MaterialConversionDetailId` (BR-M10-016).

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/MaterialConversionReceiveView.aspx`

Outbound references: `materialconversionreceiveview.aspx`

## 5. Route
`~/SInventory_UI/MaterialConversionReceive.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/MaterialConversionReceive.aspx |
| Code-behind | Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| JS include | ../assets/vendors/sweetalert2/sweetalert2.min.js |
| CSS include | ../assets/vendors/sweetalert2/sweetalert2.min.css |
| BLL class | WarehouseStockInBll (Library.BLL/SInventory_BLL/WarehouseStockInBll.cs) |
| DAL class | MaterialConversionReceiveDal (Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs) |
| DAL class | WarehouseStockInDal (Library.DAL/SInventory_DAL/WarehouseStockInDal.cs) |
| DAO/entity class | WarehouseStockInMasterDao (Library.DAO/SInventory_Entities/WarehouseStockInMasterDao.cs) |
| DAO/entity class | WharehouseStockInDetailsDao (Library.DAO/SInventory_Entities/WharehouseStockInDetailsDao.cs) |

## 7. UI Control Inventory
No ASP.NET validators; JS helpers (`showMaterialAlert`, `showMaterialSuccess`, SweetAlert-style dialogs, date picker/select2 init) are cosmetic. Messages come from `Validation()` (`:471-564`).

| Control | Label | Required | Default / state | Source |
|---|---|---|---|---|
| ddlCompany | Company | No (filter) | list from `tblCompanyInfo` | `MaterialConversionReceiveDal.LoadCompany :195` |
| ddlProductionUnit | Production Unit | No (filter) but list loads only when selected | filled after company change | `CSTL_ERPDB_Unicorn..tbl_MC_ProductionUnit WHERE BusinessUnitId = <company>` (`:201-205`, concatenated) |
| warehouseDropDownList | Warehouse | Yes (`Please Select a Warehouse !!!`) | filled after company change | `SELECT * FROM tblWearhouse WHERE CompanyId='<company>'` (`WarehouseStockInDal.GetWhInformation`) |
| stockInDateTextBox | Stock In Date | Yes (`Please insert stock in date !!!`) | empty | user, TryParse check |
| challanNoTextBox / challanDateTextBox | Challan | Yes | empty | user |
| referenceNoTextBox / referenceDateTextBox / remarksTextBox | Reference / Remarks | No | empty | user |
| ddlSupplier | (hidden, `Visible=False`) | No | `SupplierId` parsed with `TryParse`, default 0 | - |
| loadGridView | lines | at least one selected (`Please select at least one item.`) | bound from `LoadMaterialConversionList` | DataKeys `MaterialConversionDetailId, MaterialConversionMasterId` |
| chkSelect / chkSelectAll | select | - | unchecked | totals count selected rows only |
| conversionCodeTextBox, productCodeTextBox, productNameTextBox, packSizeTextBox | read-only info | - | - | ReadOnly |
| batchTextBox, mfgDateTextBox, expDateDateTextBox | Batch / Mfg / Exp | required for selected rows (`Please fill out batch!!`, `Mfg. Date!!`, `exp. Date!!`) | from production batch stock | mfg change sets exp = mfg + 2 years (`:328-329`) |
| reqQtyTextBox | Qty | required for selected rows | `FinalOutputQuantityInt` | ReadOnly |
| costPriceTextBox / vatTextBox / totalValueTextBox | Price / Vat / Total | - | `CostPerUnit` / `0` / `TotalCostOfThisBatch` | ReadOnly |
| totalQtyTextBox / totalVatTextBox / grandTotalTextBox | totals | - | computed for selected rows | ReadOnly |
| submitButton | Submit Information / Update Information | - | postback control registered with ScriptManager (`:23`) | - |

## 8. Field Mapping
| UI | DTO | Handler | BLL | DAL | Table.Column |
|---|---|---|---|---|---|
| warehouseDropDownList | `WarehouseId` | `SaveAllData :577-579` | `SaveStockInMasterInfoForMaterialConversion` | `SaveStockInMasterInformationForMaterialConversion :92-134` | `tblWHStockInMaster.WarehouseId` |
| ddlSupplier | `SupplierId` | `:581-583` | same | same | `SupplierId` |
| stockInDate/challan/reference/remarks | `WHStockInDate`, `ChallanNo`, `ChallanDate`, `ReferenceNo`, `ReferenceDate`, `Remarks` | `:573-605` | same | same | same-named columns |
| totals | `TotalQuantity` (Int32 of decimal), `TotalVat`, `TotalValue` | `:585-595` | same | same | same-named |
| constant | `Status="Posted"`; `EntryBy` = LoginName -> UserId -> "Admin"; `EntryDate` | `:606-608` | same | same | `Status`, `EntryBy`, `EntryDate` |
| first selected row DataKey | `MaterialConversionMasterId` | `GetSelectedMaterialConversionMasterId :768-787` | same | same | `tblWHStockInMaster.MaterialConversionMasterId` |
| selected rows | `ProductId`, `Batch`, `ExpDate`, `MfgDate`, `Qty`, `Price`, `VAT`, `TotalAmount`, `MaterialConversionDetailId` | `GetSelectedDetails :699-766` | `SaveWarehouseStockInDetailWithMaterialConversion` | `SaveWarehouseStockInDetailWithMaterialConversion :155-173` | `tblWHStockInDetail.*` incl. `MaterialConversionDetailId` |
Source-list columns (read): production DB `tbl_MC_MaterialConversionMaster` (`MaterialConversionCode`, `FGItemId`, `CostPerUnit`, `FinalOutputQuantity`, `BusinessUnitId`, `ProductionUnitId`, `RecipeSetupMasterId`), `tbl_MC_MaterialConversionDetails`, `tbl_INV_BatchWiseStock` (`Batch_No`, `Mfg_Date`, `Exp_Date`, `Trans_Qty`), local `tbl_MC_ProductRawMaterialDetails`, `tblProduct`, `tblPackSize`.

## 9. Dropdown Mapping
| Dropdown | Loader (handler → BLL/DAL call) | SP / SQL source | Display / Value field | Table(s) read |
|---|---|---|---|---|
| ddlCompany | LoadDropdownlist → MaterialConversionReceiveDal.LoadCompany | inline SQL | CompanyName/CompanyId | tblCompanyInfo |
| warehouseDropDownList | LoadExistingStockInData → WarehouseStockInBll.LoadWhInformation | inline SQL | WearhouseName/WearhouseId | tblWearhouse |
| ddlProductionUnit | LoadExistingStockInData → MaterialConversionReceiveDal.GetProductionUnit | inline SQL | ProductionUnitName/ProductionUnitId | CSTL_ERPDB_Unicorn |
| ddlSupplier | no loader call found passing this control (static ListItems or bound indirectly) | UNKNOWN | UNKNOWN/UNKNOWN | UNKNOWN |

## 10. Button Action Mapping
| Button | Handler | Effect | DB change |
|---|---|---|---|
| submitButton | `submitButton_Click :451` -> `Validation` -> `SaveAllData` (new) / `UpdateAllData` (hidden id present) | master + details insert (or update master, delete details, insert details); wrapped in try/catch with SweetAlert messages | `tblWHStockInMaster`, `tblWHStockInDetail` |
| cancelButton | `cancelButton_Click :466` -> `Clear` | reset form | none |
| ddlCompany / ddlProductionUnit (autopostback) | `:855-873` | reload dependent lists / pending list | none |
| chkSelect / chkSelectAll / qty/cost/vat text changes | recalc totals | none |
| viewLinkButton | redirect `MaterialConversionReceiveView.aspx` | none |

## 11. Frontend Flow
Init: `LoadDropdownlist` (companies); if `?ID=` or `Session["IDInformation"]` present, `LoadExistingStockInData` fills master, production unit, warehouse, and marks saved lines checked (`MarkAndPopulateSavedRows`); submit button text -> `Update Information` (`:26-43,237-293`). JS: SweetAlert wrappers (`showMaterialAlert/Success`), loading overlay on postbacks (`showMaterialLoading/closeMaterialLoading`, `hookUpdatePanelEndRequest`), date picker and select2 initialisation (`initMaterialConversionDatePicker`, `initMaterialConversionSelect`).

## 12. Backend Flow
1. Company change -> `GetProductionUnit` + `LoadWhInformation`; production-unit change -> `LoadPendingMaterialConversionList` -> `LoadMaterialConversionList(GenerateParameter())` where `GenerateParameter` concatenates `AND CM.BusinessUnitId='<c>' AND CM.ProductionUnitId='<p>'` (`:814-829`).
2. `SaveAllData` (`:566-628`): master via `SaveStockInMasterInfoForMaterialConversion` (returns false -> `Failed to save stock-in master information.`); details via `SaveWarehouseStockInDetailWithMaterialConversion`; `Clear()`; success alert `Data Saved Successfully`; catch -> `Error saving stock-in: <ex.Message>`.
3. `UpdateAllData` (`:630-697`): `UpdateStockInMasterInfoForMaterialConversion` (WarehouseId, WHStockInDate, ..., `Status='Posted'`, `MaterialConversionMasterId`), `DeleteWhStockInDetailById`, re-insert selected details.
4. No transaction. Numeric/date parsing uses `TryParse` (invalid -> 0 / today) - silently defaults instead of failing.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| WarehouseStockInBll.LoadWhInformation | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:27 |
| WarehouseStockInBll.SaveStockInMasterInfoForMaterialConversion | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:46 |
| WarehouseStockInBll.StockInId | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:55 |
| WarehouseStockInBll.WHStockInCode | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:61 |
| WarehouseStockInBll.OrdnNoGenerator | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:67 |
| WarehouseStockInBll.SaveWarehouseStockInDetailWithMaterialConversion | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:108 |
| WarehouseStockInBll.UpdateStockInMasterInfoForMaterialConversion | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:138 |
| WarehouseStockInBll.DeleteWhStockInDetailById | Library.BLL/SInventory_BLL/WarehouseStockInBll.cs:152 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| MaterialConversionReceiveDal.LoadCompany | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:196 | inline SQL: tblCompanyInfo |  |
| MaterialConversionReceiveDal.LoadStockInMasterForEdit | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:136 | inline SQL: CSTL_ERPDB_Unicorn, tblWHStockInDetail, tblWHStockInMaster, tblWearhouse |  |
| MaterialConversionReceiveDal.GetProductionUnit | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:202 | inline SQL: CSTL_ERPDB_Unicorn |  |
| WarehouseStockInDal.GetWhInformation | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:27 | inline SQL: tblWearhouse |  |
| MaterialConversionReceiveDal.LoadStockInDetailForEdit | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:168 | inline SQL: CSTL_ERPDB_Unicorn, tblProduct, tblWHStockInDetail, tblWHStockInMaster |  |
| MaterialConversionReceiveDal.LoadMaterialConversionListForEdit | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:82 | inline SQL: CSTL_ERPDB_Unicorn, tblPackSize, tblProduct, tblWHStockInDetail |  |
| MaterialConversionReceiveDal.LoadMaterialConversionList | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:26 | inline SQL: CSTL_ERPDB_Unicorn, tblPackSize, tblProduct, tblWHStockInDetail | @Param |
| WarehouseStockInDal.SaveStockInMasterInformationForMaterialConversion | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:93 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.WarehouseStockInCodeId | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:176 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.SaveWarehouseStockInDetailWithMaterialConversion | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:156 | inline SQL: tblWHStockInDetail |  |
| WarehouseStockInDal.UpdateStockInMasterInfoForMaterialConversion | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:257 | inline SQL: tblWHStockInMaster |  |
| WarehouseStockInDal.DeleteWhStockInDetailById | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:280 | inline SQL: tblWHStockInDetail, tblWHStockInDetail |  |

## 16. Stored Procedure Mapping
None directly. Approval later calls `sp_Acc_MaterialConversionReceive` (PAGE-386). `ExecuteAllSqlQueryByStoreProcedure` generic runner for all SQL.

## 17. Database Mapping
| Table | R/W | Notes |
|---|---|---|
| CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionMaster / ..tbl_MC_MaterialConversionDetails / ..tbl_MC_RecipeMaster / ..tbl_MC_ProductionUnit / ..tblCompanyInfo / ..tbl_INV_BatchWiseStock | R | production ERP (cross-database, name hard-coded) |
| tbl_MC_ProductRawMaterialDetails (local DB) | R | INNER JOIN on `FGItemId = ProductId` |
| tblWHStockInMaster | R/W | `WarehouseId`, `MaterialConversionMasterId` are MC-specific |
| tblWHStockInDetail | R/W | `MaterialConversionDetailId` |
| tblProduct, tblPackSize, tblWearhouse, tblCompanyInfo | R | lookups |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| Warehouse | required | `Validation :473` | Please Select a Warehouse !!! | BR-M10-018 |
| Stock-in date, challan no, challan date | required | `:481-503` | Please insert stock in date !!! / challan number !!! / challan date !!! | BR-M10-018 |
| Selected line code, batch, exp, mfg, qty | required | `:520-553` | Please fill out product code!! / batch!! / exp. Date!! / Mfg. Date!! / Req.Qty!! | BR-M10-018 |
| At least one selected line | count>0 | `:557-561` | Please select at least one item. | BR-M10-018 |
| Exp >= Mfg | date order | `expDateDateTextBox_OnTextChanged :333-364` | Date Error!! | BR-M10-006 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-016 | The selectable list contains production output lines (`tbl_MC_MaterialConversionDetails` joined to batch stock) with `FinalOutputQuantity>0` that are **not** already linked (`WD.MaterialConversionDetailId`) to a warehouse stock-in whose status is not `Reject`; when editing, lines of the current stock-in are excluded from that exclusion. Quantity = sum of `ABS(Trans_Qty)` of the batch rows; cost per unit = `CM.CostPerUnit`; VAT = 0. | `Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:25-133` | `LoadMaterialConversionList(ForEdit)` | CONFIRMED |
| BR-M10-017 | The list is filtered by company (`CM.BusinessUnitId`) and production unit (`CM.ProductionUnitId`); the warehouse list is restricted to the company (`tblWearhouse.CompanyId`). The list is shown only after a production unit is chosen. | `MaterialConversionReceive.aspx.cs:295-312,814-829,855-873` | `GenerateParameter` | CONFIRMED |
| BR-M10-018 | Save/Update needs warehouse, stock-in date, challan number and date, and at least one selected line with product code, batch, exp date, mfg date and quantity. | `MaterialConversionReceive.aspx.cs:471-564` | `Validation` | CONFIRMED |
| BR-M10-019 | New receipts are saved `Status='Posted'`, master `MaterialConversionMasterId` = the master id of the first selected row; each detail stores `MaterialConversionDetailId`. Amounts: line total = qty x cost per unit (rounded to 2 dp in SQL) (+ qty x VAT, VAT=0). Numeric/date fields that fail to parse silently become 0 / today. | `MaterialConversionReceive.aspx.cs:566-628,699-787`; `WarehouseStockInDal.cs:92-173` | `SaveAllData` | CONFIRMED |
| BR-M10-020 | Editing replaces all detail rows and re-posts the document (`Status='Posted'`); a rejected document releases its conversion lines for re-receipt (anti-join ignores `Reject`). | `MaterialConversionReceive.aspx.cs:630-697`; `MaterialConversionReceiveDal.cs:63-66` | `UpdateAllData` | CONFIRMED |
| BR-M10-021 | Header totals count selected rows only: TotalQuantity = sum(int qty), TotalVat = sum(qty x vat), grand total = sum(line totals). | `MaterialConversionReceive.aspx.cs:408-449` | `CalculateQuantityVatAndValue` | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]` else `Session["UserId"]` else literal `"Admin"` is used as EntryBy/UpdateBy (`:607,675`) - an unauthenticated context would be recorded as `Admin`. No role checks.

## 21. Status Lifecycle
`Posted` on save/update (`:606,674`); `Approved`/`Reject` by PAGE-386 (calls `sp_Acc_MaterialConversionReceive` when `MaterialConversionMasterId>0`). View list = `Posted`/`Reject` with a non-zero `MaterialConversionMasterId` (`GetMaterialConversionReceiveData :207-213`).

## 22. Error Handling
try/catch in `SaveAllData`/`UpdateAllData` -> SweetAlert `Operation failed` with raw exception message (leaks internals). No transaction, so a failure after the master insert leaves a header without lines.

## 23. Audit
`EntryBy/EntryDate`, `UpdateBy/UpdateDate` on `tblWHStockInMaster`; approval fields set later.

## 24. Integration
Reads the production ERP database `CSTL_ERPDB_Unicorn` directly by cross-database query (hard-coded name); approval SP may write back (UNKNOWN). Local table `tbl_MC_ProductRawMaterialDetails` joined per raw material.

## 25. Reports
None from this page; View page prints `WHStockInReportViewer` (RPT-M10-03).

## 26. Dependencies
`MaterialConversionReceiveDal`, `WarehouseStockInBll/Dal`, `WarehouseStockInMasterDao`, `WharehouseStockInDetailsDao`, production DB `CSTL_ERPDB_Unicorn`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-370-01 | Select company then production unit | pending lines listed | BR-M10-016/017 |
| TC-370-02 | Line already in a Posted/Approved stock-in | not listed | BR-M10-016 |
| TC-370-03 | Line only in a Rejected stock-in | listed again | BR-M10-016/020 |
| TC-370-04 | Submit with no warehouse | `Please Select a Warehouse !!!` | BR-M10-018 |
| TC-370-05 | No line selected | `Please select at least one item.` | BR-M10-018 |
| TC-370-06 | Valid save of 2 lines | master `Posted` with `MaterialConversionMasterId`, 2 details; totals = selected only | BR-M10-019/021 |
| TC-370-07 | FG with 2 raw materials | multiple list rows per detail/batch (duplicate risk) | GAP-M10-008 |
| TC-370-08 | Edit and untick a line | line removed from document, re-listed | BR-M10-020 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx`, `Solution.Web/SInventory_UI/MaterialConversionReceive.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Effect on stock | None existed | only via approval SP (UNKNOWN) | UNKNOWN | PAGE-386 |

## 30. Known Gaps
- GAP-M10-008: `INNER JOIN tbl_MC_ProductRawMaterialDetails MAP ON CM.FGItemId = MAP.ProductId` with `MAP.RawMaterialId` in GROUP BY can return one row per raw material for the same conversion detail/batch, allowing the same output to be ticked several times (INFERRED).
- Hard-coded cross-database names `CSTL_ERPDB_Unicorn`, string-concatenated filters (`GenerateParameter`, `GetProductionUnit`), raw exception text shown to users, silent `TryParse` defaults, EntryBy fallback `Admin`, no transaction.
- Anti-join excludes any non-Rejected receipt including `Posted`; a stuck `Posted` document blocks the conversion.

## 31. Change Impact
Shares `tblWHStockInMaster/Detail` and `WarehouseStockInDal` with PAGE-385/387; approval PAGE-386 branches on `MaterialConversionMasterId`. `MaterialConversionReceiveView` lists the same documents.

