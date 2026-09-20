---
page_id: PAGE-367
module: M10
page: DepotStockAdjustmentsVoucher
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "PARTIAL", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-367 — DepotStockAdjustmentsVoucher

> **Evidence level:** Tier A - reviewed 2026-09-19 against `.aspx`, `.aspx.cs`, BLL, DAL and DAO source. Confidence tags: CONFIRMED (read directly in code), INFERRED, UNKNOWN. Stored-procedure bodies are NOT in the repository unless stated; SP effects are UNKNOWN. Sections not rewritten below were mechanically extracted and re-checked where noted.
## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-367 |
| Module | M10 — Warehouse, DC Stock & Inventory (INFERRED from folder + file name) |
| Page name | DepotStockAdjustmentsVoucher |
| Kind | Entry / Transaction (INFERRED from name) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_DerectStockOut |
| aspx size | 47407 bytes |
| Code-behind LOC | 474 |
| Inline script size | 985 chars |
| Suspected legacy/duplicate | no evidence |

## 2. Purpose
**Purpose (CONFIRMED from labels/messages):** Creates a **Depot Stock Adjustment Voucher** (stock-out document, table `tblDeStockOutMaster`) for a Distribution Center: choose DC, optional customer code, optional invoice (`N/A` allowed), stock-out date, reason and product; search that DC's stock rows for the product, enter stock-out quantities, stage rows and submit. The voucher is saved `Posted` and goes to `DcStockOutApproval.aspx`. Source: `Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucher.aspx.cs` (class `SInventory_UI_DerectStockOut`).

## 3. Business Objective
Formal, approved write-off/adjustment of DC stock (damage, loss, invoice-related corrections) with reason, optional invoice/customer reference and checker sign-off. The actual quantity effect happens at approval (SP `sp_UD_DcStockOutApproval`, body absent) - the entry page does not change stock.

## 4. Navigation
Menu placement is **DB-driven** (`tblMainMenu`, `tblMenuDistribution`, loaded by `sp_GET_MainMenuRole2` in `NewMasterPage.master.cs`); the menu rows are not in the repository, so the exact menu path is `UNKNOWN`.

Inbound references (other pages whose code/markup mention this page): `SInventory_UI/DepotStockAdjustmentsVoucherView.aspx`

Outbound references: `depotstockadjustmentsvoucherview.aspx`

## 5. Route
`~/SInventory_UI/DepotStockAdjustmentsVoucher.aspx` (file-based WebForms route; no `MapPageRoute` alias).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucher.aspx |
| Code-behind | Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucher.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL class | DeStockOutBLL (Library.BLL/SInventory_BLL/DeStockOutBLL.cs) |
| BLL class | SubDepotChalanBLL (Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs) |
| DAL class | DeStockOutDal (Library.DAL/SInventory_DAL/DeStockOutDal.cs) |
| DAL class | SubDepotChalanDAL (Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs) |
| DAO/entity class | DcStockOutDetailsDao (Library.DAO/SInventory_Entities/DcStockOutDetailsDao.cs) |
| DAO/entity class | DcStockOutMasterDao (Library.DAO/SInventory_Entities/DcStockOutMasterDao.cs) |

## 7. UI Control Inventory
No validators. Controls: `DistributioncenterDropDownList1` (Distribution Center, required; also fills `ComUnitCodeTextBox`), `customerTextBox` (Customer Code, optional), `StockOutTextBox` (Stock Out Date, CalendarExtender, required), `txtReason` (Reason, required), `txtproformaInvoice` (Invoice No; `ReadOnly` until a DC is chosen; AutoCompleteExtender -> `SInventoryWebService.GetProformaInvoiceNoNew`, returns `id:invoiceNo`, plus `0:N/A`; required via hidden `HiddenField1`), `productDropDownList` (required), `Button2` `Search Product`, `DerectStoctOutGridView` (chk, `lbl_PCode`, `lbl_PName`, `lbl_StockQty`, `lbl_BatchNo`, `lbl_ExpDate`, `lbl_ReceiveDate`, `transferQtyTextBox` digits-only), `addButton`, `chalanGridView` (staged; DataKeys `DCStoreId, PackSize`), `submitButton` `Submit`, `Button2` `Print`, view link -> `DepotStockAdjustmentsVoucherView.aspx`.

## 8. Field Mapping
| UI | DTO | Handler | DAL | Table.Column |
|---|---|---|---|---|
| DistributioncenterDropDownList1 | `DcStockOutMasterDao.ComUnitId` | `submitButton_Click1 :240-255` | `DeStockOutDal.SaveDataForDcStockOutMaster :44-61` | `tblDeStockOutMaster.ComUnitId` |
| HiddenField1 (invoice id) | `InvoiceId` | same | same | `InvoiceId` (0 = N/A) |
| StockOutTextBox | `StockOutDate` | same | same | `StockOutDate` |
| txtReason | `Reason` | same | same | `Reason` |
| LoginName / Now / `Posted` | `EntryBy` / `EntryDate` / `Status` | same | same | `EntryBy`, `EntryDate`, `Status` |
| customerTextBox | `CustomerCode` | same | same | `CustomerCode` |
| (generated) | `DcStockOutMasterId` = MAX+1 | `DeStockOutBLL.SaveDataForDcStockOutMasterBll` | ClsPrimaryKeyFind | `DcStockOutMasterId` |
| chalanGridView row | `DcStoreId`, `PackSize`, `ProductCode`, `ProductName`, `BatchNo`, `StackOutQty` (Int32), `ExpDate`, `ReceiveDate` | `:265-287` | `SaveDataForStockOutDetailDal :63-81` | `tblDeStockOutDetails.*` |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL |
|---|---|---|
| DistributioncenterDropDownList1 | `DeStockOutDal.DistributionCenterLoadDal :24-29` | `select ComUnitId,(ComUnitCode+':'+ComUnitName) as Com from tblCompanyUnit` |
| productDropDownList | `ProductLoadDal :18-23` | `SELECT (ProductCode+':'+ProductName)Pro,* FROM tblProduct` |
| stock rows | `SubDepotChalanDAL.GetProductDcStoreSubdeport` (`Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:327-340`) | `tblDCStore LEFT JOIN tblProduct LEFT JOIN tblCompanyUnit LEFT JOIN tblUnitPrice ... WHERE ComUnitCode='<code>' AND tblProduct.ProductId='<id>' AND tblUnitPrice.IsActive='True'` (string-concatenated; the `StockQty>0` filter is commented out) |
| invoice autocomplete | `SInventoryWebService.GetProformaInvoiceNoNew` (`Solution.Web/App_Code/SInventoryWebService.cs:236-258`) | `tblInvoice WHERE ComUnitId='<Session["ComUnitId"]>' AND InvoiceDate >= 2020/07/01 AND InvoiceNo LIKE @prefix` plus `0:N/A` |

## 10. Button Action Mapping
| Button | Handler | Effect | DB |
|---|---|---|---|
| Button2 Search Product | `Button1_Click :115` | bind DC stock rows | none |
| addButton | `addButton_Click :325` | stage ticked rows with qty | none |
| DeleteImageButton | `:402` | unstage | none |
| submitButton | `submitButton_Click1 :240` | `Validation()`, master insert, detail inserts, success popup then redirect to view | INSERT `tblDeStockOutMaster`, `tblDeStockOutDetails` |
| DC dropdown change | `:130` | sets `Session["ComUnitId"]`, `ComUnitCodeTextBox`, enables invoice box | none |

## 11. Frontend Flow
Init: product + DC dropdowns. `ProformaInvoice_TextChanged` splits `id:invoiceNo` from the autocomplete, storing id in `HiddenField1` and text in the box; invalid text clears both and alerts `Please Enter valid Data!!....`. `dQtyTextBox_TextChanged` (`:72-112`) compares transfer qty with `lbl_StockQty` for ticked rows and clears the box with `Stock Out Qty. cantbe more then Stock Quantity`. Success via `successalert(...,'DepotStockAdjustmentsVoucherView.aspx')` JS.

## 12. Backend Flow
`submitButton_Click1`: `Validation()` -> master (`SaveDataForDcStockOutMasterBll`) -> if true build details from `chalanGridView` labels (`Convert.ToInt32(lbl_StackOutQty.Text)`, `Convert.ToDateTime(lbl_ExpDate2.Text)` on text formatted `dd MMM, yyyy` by SQL `format()`) -> `SaveDataForStockOutDetailBll` (foreach insert with `MAX+1` ids) -> popup. No stock table is touched. No try/catch/transaction.

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.

## 14. Service Mapping
| BLL method | Source |
|---|---|
| DeStockOutBLL.ProductLoadBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:21 |
| DeStockOutBLL.DistributionCenterLoadBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:27 |
| SubDepotChalanBLL.GetProductDcStoreSubdeport | Library.BLL/SubDepot_BLL/SubDepotChalanBLL.cs:157 |
| DeStockOutBLL.SaveDataForDcStockOutMasterBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:39 |
| DeStockOutBLL.SaveDataForStockOutDetailBll | Library.BLL/SInventory_BLL/DeStockOutBLL.cs:47 |

## 15. Repository Mapping
| DAL method | Source | SP / SQL | SQL parameters |
|---|---|---|---|
| DeStockOutDal.ProductLoadDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:19 | inline SQL: tblProduct |  |
| DeStockOutDal.DistributionCenterLoadDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:25 | inline SQL: tblCompanyUnit |  |
| SubDepotChalanDAL.GetProductDcStoreSubdeport | Library.DAL/SubDepot_DAL/SubDepotChalanDAL.cs:328 | inline SQL: tblCompanyUnit, tblDCStore, tblProduct, tblUnitPrice |  |
| DeStockOutDal.SaveDataForDcStockOutMaster | Library.DAL/SInventory_DAL/DeStockOutDal.cs:45 | inline SQL: tblDeStockOutMaster | @ComUnitId, @CustomerCode, @DcStockOutMasterId, @EntryBy, @EntryDate, @InvoiceId, @Reason, @Status,  |
| DeStockOutDal.SaveDataForStockOutDetailDal | Library.DAL/SInventory_DAL/DeStockOutDal.cs:64 | inline SQL: tblDeStockOutDetails | @BatchNo, @DCStoreId, @DcStockOutDetailsId, @DcStockOutMasterId, @ExpDate, @PackSize, @ProductCode,  |

## 16. Stored Procedure Mapping
SP `sp_UD_DcStockOutApproval` is called from the approval page (PAGE-363), not here. Nothing else.

## 17. Database Mapping
| Table | R/W |
|---|---|
| tblDeStockOutMaster | W (DcStockOutMasterId, ComUnitId, InvoiceId, StockOutDate, Reason, EntryBy, EntryDate, CustomerCode, Status) |
| tblDeStockOutDetails | W (DcStockOutDetailsId, DcStockOutMasterId, DCStoreId, ProductCode, ProductName, StackOutQty, PackSize, BatchNo, ExpDate, ReceiveDate) |
| tblDCStore, tblProduct, tblCompanyUnit, tblUnitPrice, tblInvoice | R |

## 18. Validation Matrix
| Field | Rule | Where | Message | BR |
|---|---|---|---|---|
| DC | required | `Validation :203` | Please Select Distribution center!! | BR-M10-043 |
| Invoice | hidden id non-empty (N/A = 0 accepted) | `:208` | Please Select Invoice Number!! | BR-M10-043 |
| Date | required | `:213` | Please Insert Stock Out Date!! | BR-M10-043 |
| Reason | required | `:219` | Please Insert Reason!! | BR-M10-043 |
| Product | required | `:225` | Please Select Product!! | BR-M10-043 |
| Staged rows | >=1 | `:233` | Please Add Product and Stoct Out Qty..!! | BR-M10-043 |
| Qty <= stock | postback compare | `dQtyTextBox_TextChanged` | Stock Out Qty. cantbe more then Stock Quantity | BR-M10-045 |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M10-043 | A voucher needs a DC, an invoice choice (an explicit `N/A` counts), a stock-out date, a reason, a product and at least one staged line. Customer code is optional. | `DepotStockAdjustmentsVoucher.aspx.cs:199-239` | `Validation` | CONFIRMED |
| BR-M10-044 | Candidate stock rows are the DC's `tblDCStore` rows for the selected product joined to the active unit price (`IsActive`), with no minimum stock filter (`StockQty>0` commented out); joined by DC **code** text. | `SubDepotChalanDAL.cs:327-340` | `GetProductDcStoreSubdeport` | CONFIRMED |
| BR-M10-045 | Stock-out quantity may not exceed the row's displayed stock (checked in the textbox-change postback only; not repeated at save). Digits only. | `DepotStockAdjustmentsVoucher.aspx.cs:72-112` | `dQtyTextBox_TextChanged` | CONFIRMED |
| BR-M10-046 | Saving creates a `Posted` voucher (master + details); it does **not** change `tblDCStore`. The stock effect is deferred to approval (PAGE-363). A voucher that is not `Approved` may be deleted from the list page (`Can not Delete Data!!!....` for approved ones). | `DepotStockAdjustmentsVoucher.aspx.cs:240-301`; `DepotStockAdjustmentsVoucherView.aspx.cs:42-60`; `DeStockOutDal.cs:44-81` | `submitButton_Click1` | CONFIRMED |

## 20. Permission Matrix
`Session["LoginName"]` (EntryBy), `Session["ComUnitId"]` (invoice autocomplete; NullReference if no DC chosen first). All company units are selectable; no user-to-DC restriction.

## 21. Status Lifecycle
`Posted` (create) -> `Approved` / `Reject` (PAGE-363). View page blocks deletion of `Approved` vouchers only.

## 22. Error Handling
No try/catch; alerts via `successalert` JS helper or `alert()`; the success popup fires even if detail inserts silently failed (BLL returns true unconditionally).

## 23. Audit
`EntryBy`, `EntryDate` on master; `ApprovedBy`, `ApprovedDate` set at approval.

## 24. Integration
None.

## 25. Reports
Report `DcStockOutReportViewer.aspx?DcStockOutMasterId=` -> `crpDcStockOut.rpt` (RPT-M10-04); opened from the list/approval pages.

## 26. Dependencies
`DeStockOutBLL/Dal`, `SubDepotChalanBLL/DAL`, `SInventoryWebService`, DAOs `DcStockOutMasterDao`, `DcStockOutDetailsDao`.

## 27. Test Cases
| ID | Scenario | Expected | BR |
|---|---|---|---|
| TC-367-01 | Save without reason | `Please Insert Reason!!` | BR-M10-043 |
| TC-367-02 | Choose `N/A` invoice | accepted (InvoiceId 0) | BR-M10-043 |
| TC-367-03 | Type free text in invoice box | cleared with `Please Enter valid Data!!....` | BR-M10-043 |
| TC-367-04 | Qty > stock | textbox cleared | BR-M10-045 |
| TC-367-05 | Valid save | voucher `Posted`, tblDCStore unchanged | BR-M10-046 |
| TC-367-06 | Delete approved voucher (view page) | blocked message | BR-M10-046 |
| TC-367-07 | Pick invoice before DC | NullReference in autocomplete service | - |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucher.aspx`, `Solution.Web/SInventory_UI/DepotStockAdjustmentsVoucher.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).

## 29. Spec vs Actual
| Area | Specification | Actual | Status | Reference |
|---|---|---|---|---|
| Prior spec | None existed | - | MISSING | - |
| Stock effect | None existed | deferred to SP at approval (UNKNOWN) | UNKNOWN | PAGE-363 |

## 30. Known Gaps
- GAP-M10-011: SP `sp_UD_DcStockOutApproval` body missing - whether/how DC stock (and valuation rows) are reduced is UNKNOWN.
- Stock rows joined by DC code and product id via string concatenation (SQL injection surface).
- Invoice list limited to invoices dated from 2020-07-01; `Session["ComUnitId"]` dependency; no server re-check of quantity; page class `SInventory_UI_DerectStockOut` reused by `SubDepot` pages (name clash risk UNKNOWN).

## 31. Change Impact
`DeStockOutBLL/Dal` shared with `DcStockOutApproval`, `DepotStockAdjustmentsVoucherView`, `DcStockOutReportViewer`; `SubDepotChalanDAL.GetProductDcStoreSubdeport` shared with other sub-depot pages.

