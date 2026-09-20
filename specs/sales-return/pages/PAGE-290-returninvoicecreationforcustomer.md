---
page_id: PAGE-290
module: M08
page: ReturnInvoiceCreationForCustomer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "PARTIAL", "frontend": "PARTIAL", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-290 — ReturnInvoiceCreationForCustomer (legacy "Return Invoice" against a delivery invoice, freeze-stock flavour)

> **Evidence level:** Tier A (focused on the write path). Read: `.aspx.cs` lines 1-120, 660-1131 (validation, save, calculations, dQty/select handlers) and control inventory of `.aspx`; `InvoiceBLL`/`InvoiceDAL` save methods; `OrderInfoDAL.LoadDelivaryInvoice/GetQty`. Lines 120-660 (legacy `LoadAllDataByOrder`, `InitialGrid`, `AddFunc`, add/remove row, `GetProduct`) were skimmed only (dead/legacy per no-caller analysis) → UI/frontend PARTIAL. No SP bodies are involved (all inline SQL). The class name is `SInventory_UI_InvoiceCreationForCustomer` — copy of the invoice-creation page.

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-290 |
| Module | M08 |
| Name / class | ReturnInvoiceCreationForCustomer / `SInventory_UI_InvoiceCreationForCustomer` (`.aspx.cs:13`) |
| Kind | Entry / Transaction (legacy) |
| Size | aspx 557 lines, code-behind 1,131 lines |
| Legacy | Yes — predecessor of PAGE-287/294; writes to `tblDCStoreFreeze` (freeze stock) and never to `tblDCStore`; no accounting posting |

## 2. Purpose
INFERRED from labels ("Invoice Generate", "Return Invoice Quantity Must Be Smaller…") and code: create a **Return Invoice** for selected lines of a delivery invoice found by delivery invoice number, capturing the returned quantity per line and parking the returned goods in the *freeze (quarantine) store*.

## 3. Business Objective
INFERRED: document a return against a delivered invoice (printable return invoice) and hold the returned stock in `tblDCStoreFreeze` (status "ReturnStock") until it is inspected/re-admitted by another process (freeze-stock pages, not in M08).

## 4. Navigation
Menu-driven (UNKNOWN). Outbound: `../SInventory_RPTVIEW/ReturnInvoiceReportViewer.aspx?InvNo=` (print, `printButton_Click` :919-925).

## 5. Route
`~/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx`.

## 6. Source Files
| Type | Path |
|---|---|
| View / code-behind | Solution.Web/SInventory_UI/ReturnInvoiceCreationForCustomer.aspx(.cs) |
| BLL | InvoiceBLL.cs (`SaveReturnInvoice` :108-121, `SaveDCStoreFreeze` :122-126, `ReturnInvoiceNoGenerator` :553-596); OrderInfoBLL (`LoadDelivaryInvoice`, `GetQty`, `ProductVat`, `GetTradeTerm`); RequisitionBLL |
| DAL | InvoiceDAL.cs (`SaveDataForReturnInvoice` :441-471, `SaveDataForReturnInvoiceDetails` :488-505, `DCStockInDAL` :1864-1900, `ReturnInvoiceNoCount` :1204-1211, `DCInfoWithDCId`); OrderInfoDAL.cs:570-591 |

## 7. UI Control Inventory
| Control | Type | Required | Default / state | Notes |
|---|---|---|---|---|
| delinvoicenoTextBox | TextBox (AutoPostBack, AutoComplete `GetCustomer`) | Yes (search) | empty | delivery invoice no |
| salesCenterDropDownList | DropDownList (AutoPostBack) | Yes (search filter `ComUnitId`) | `LoadSC` (all DCs, no user filter) | |
| Button1 "Search" | Button | — | — | `Button1_Click` :1067-1096 |
| custCodeTextBox, custNameTextBox (AutoComplete), custAddressTextBox, comUnitNameTextBox, miaCodeTextBox, miaNameTextBox, districtNameTextBox, areaNameTextBox, marketNameTextBox, custCategoryTextBox | TextBox | no | filled by `GetCustInfo(customerCode)` — which queries `CustomerMaster(orderNoTextBox.Text)` i.e. by **order no** | readonly except code/name |
| invDateTextBox | TextBox | — | today | ReadOnly |
| orderNoTextBox, orderDateTextBox (+CalendarExtender) | TextBox | Yes | from invoice | `Validation` :42-51 |
| payTypeDDL | DropDownList | no | index 1 | unused in save |
| gridLineItemGridView | GridView `DataKeyNames=InvoiceDetailId,DCStoreId` | rows | bound to `LoadDelivaryInvoice` | cols Code, Name, UP, UVAT, Qty (RO), TP, TPVAT, DP, DAmt, NP, BQty, TQty, **PRQty** (RO, previous returned qty), **RQty** (`dQtyTextBox`, RO until row ticked), checkbox `chkSelect` |
| chkSelect / chkSelectAll | CheckBox | ≥1 (implicit) | unchecked | `chkSelect_OnCheckedChanged` :1114-1131 toggles dQty ReadOnly and recalculates totals |
| tpTptalTextBox, disTotalTextBox, vatTotalTextBox, grandTotalTextBox | TextBox | — | ReadOnly | totals over **ticked** rows |
| saveButton "Invoice Generate" | Button | — | — | `saveButton_Click` :666-675 |
| invTextBox + printButton | TextBox/Button | — | generated return invoice no | print |

## 8. Field Mapping
| UI | Variable | Handler | BLL | DAL | SQL param | Table.column |
|---|---|---|---|---|---|---|
| invDateTextBox | `Invoice.InvoiceDate` | `SaveInvoice` :827 | `InvoiceBLL.SaveReturnInvoice(Invoice)` :108-116 | `InvoiceDAL.SaveDataForReturnInvoice` :441-471 | `@ReturnInvoiceDate` | tblReturnInvoice.ReturnInvoiceDate |
| new key | `ReturnInvoiceId = PrimaryKeyMax("ReturnInvoiceId","tblReturnInvoice")` | BLL :110 | | | `@ReturnInvoiceId` | ReturnInvoiceId |
| generated | `ReturnInvoiceNo = 'INV-' + <unit code> + '-' + 8-digit (COUNT(ReturnInvoiceNo of unit)+1)` | BLL :112,553-596 | | | `@ReturnInvoiceNo` | ReturnInvoiceNo |
| orderNoTextBox / orderDateTextBox | OrderNo / OrderDate | :828-829 | | | `@OrderNo`,`@OrderDate` | OrderNo, OrderDate |
| hdCustomerMasterId / hdComUnitId / hdMiaId | CustomerMasterId / ComUnitId / MiaId | :830-832 | | | | CustomerMasterId, ComUnitId, MiaId |
| totals | TpTotal, TpDiscount, TpVat, TpGrandTotal | :834-837 | | | | same names (**no DeductionCharge / NetReturnAmount / IsPosting**) |
| Session UserId | UserId | :838 | | | `@UserId` | UserId |
| orderIdHiddenField | OrderId | :840 | | | | OrderId |
| invoiceHiddenField | `ReturnInvoiceid` = original InvoiceId | :841 | | | `@InvoiceId` | tblReturnInvoice.InvoiceId (original delivery invoice) |
| line InvoiceDetailId | `ReturnDetailsId` | `SaveInvoiceDetail` :697-698 | `SaveReturnInvoice(InvoiceDetail)` :117-121 | `SaveDataForReturnInvoiceDetails` :488-505 | `@InvoiceDetailId` | tblReturnInvoiceDetail.InvoiceDetailId (original detail) |
| new key | `InvoiceDetailId = PrimaryKeyMax("ReuturnInvoiceDetailId","tblReturnInvoiceDetail")` | BLL :119 | | | `@ReuturnInvoiceDetailId` | **ReuturnInvoiceDetailId** (typo'd column name) |
| DC row (`DCInfoWithDCId`) | ProductName, PackSize, BatchNo, ExpDate, ReceiveDate | :703-707 | | | | detail columns |
| product cost | `CostPrice = tblUnitPrice.CostPrice` (`ProductInfo(comUnit, code)`) | :692,708 | | | | CostPrice |
| RQty (dQty) | `Quantity = dQty`; `TotalQuantity = dQty + bonus` | :739-751 | | | | Quantity, TotalQuantity |
| **Qty (delivered)** × UP / × UVAT | `TotalPrice`, `TotalPriceVatAmount` | :752-759 | | | | TotalPrice, TotalPriceVatAmount |
| computed | `NetAmount = TP − DiscountAmount + VAT` | :762-763 | | | | NetAmount |
| DC freeze | `DCStockNew` (StockQty=dQty, TotalQuantity=dQty, …) | :770-801 | `SaveDCStoreFreeze` :122-126 (`PrimaryKeyMax("DCStoreFreezeId","tblDCStoreFreeze")`) | `InvoiceDAL.DCStockInDAL` :1864-1900 | `@DCStoreId,@DCStoreFreezeId,@InvoiceDetailId,…,@StockCondition='ReturnStock'` | tblDCStoreFreeze.* |

## 9. Dropdown Mapping
`salesCenterDropDownList` ← `OrderInfoBLL.LoadSC(ddl)` (all `tblCompanyUnit`, no per-user filter). `payTypeDDL` ← `tblPaymentType` (unused).

## 10. Button Action Mapping
Search (`Button1_Click`): `OrderInfoBLL.LoadDelivaryInvoice(delinvoiceno, comUnitId)` → binds grid, fills header, then per row `GetQty(InvoiceDetailId)` → PRQty = `SUM(TotalQuantity)` from `tblReturnInvoiceDetail` for that original detail. `dQtyTextBox_TextChanged` (:968-1066) validates and recomputes. `saveButton_Click`: `Validation` → `SaveInvoice` → `SaveInvoiceDetail` → `Clear` → alert "Create Invoice".

## 11. Frontend Flow
Select DC + delivery invoice no → Search → tick lines → enter RQty (only editable when ticked) → postback recomputes TP/VAT/discount (trade-term %) and totals over ticked rows → "Invoice Generate". No client JS beyond controls.

## 12. Backend Flow
`saveButton_Click` (:666-675): `Validation()`; `SaveInvoice()` → `ReturnInvoiceBLL`-style number/PK generation then INSERT `tblReturnInvoice`; `SaveInvoiceDetail(invId)` per **ticked** row: read product/DC info, INSERT `tblReturnInvoiceDetail` (`ReuturnInvoiceDetailId`), INSERT `tblDCStoreFreeze` (StockCondition 'ReturnStock', StockQty=dQty); then `Clear()` and alert. No try/catch, no transaction, no posting. `invTextBox` = the generated number.
`dQtyTextBox_TextChanged`: if `dQty + PRQty ≤ TQty`: TP = unit price × dQty; discount % from `GetTradeTerm(tpTotal, CustTypeId, PaymentType, SubmissionDate)` (from the original order); VAT = TP × product VAT% / 100 (`ProductVat`); NP = TP − discount + VAT; else untick, clear dQty, alert.

## 13. API Mapping
None (ASMX autocomplete only).

## 14. Service Mapping
`InvoiceBLL.SaveReturnInvoice` ×2 overloads, `SaveDCStoreFreeze`, `ReturnInvoiceNoGenerator`, `ProductInfo`, `DCInfoWithDCId`, `CustomerMaster`, `PaymentTypeLoadBLL`; `OrderInfoBLL.LoadDelivaryInvoice`, `GetQty`, `ProductVat`, `GetParcentFromOrderDetailsMasterID`, `GetTradeTerm`.

## 15. Repository Mapping
Inline SQL in `InvoiceDAL` (parameterised INSERTs) and `OrderInfoDAL.LoadDelivaryInvoice`: `SELECT … FROM tblInvoice ⟕ tblInvoiceDetail ⟕ tblProduct ⟕ tblCustMaster WHERE DelivaryInvoiceNo=@DelivaryInvoiceNo AND ComUnitId=@ComUnitId` (delivery-* columns aliased as Quantity/TotalPrice/VAT/NetPrice/TotalQty); `GetQty`: `SELECT SUM(TotalQuantity) FROM tblReturnInvoiceDetail WHERE InvoiceDetailId=@InvoiceDetailId`.

## 16. Stored Procedure Mapping
None (inline SQL). `sp_SalesReturnPosting` is **not** called here.

## 17. Database Mapping
- **tblReturnInvoice** (W): ReturnInvoiceId, ReturnInvoiceNo, ReturnInvoiceDate, OrderNo, OrderDate, CustomerMasterId, ComUnitId, MiaId, PaymentTypeId(0), TpTotal, TpDiscount, TpVat, TpGrandTotal, UserId, OrderId, InvoiceId, TotalSpecialAmount.
- **tblReturnInvoiceDetail** (W): ReuturnInvoiceDetailId, ProductCode, ProductName, PackSize, BatchNo, ReceiveDate, ExpDate, CostPrice, UnitPrice, UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, TotalPriceVatAmount, DiscountPercentage, DiscountAmount, NetAmount, ReturnInvoiceId, DCStoreId(0/blank), OrderDetailsId, InvoiceDetailId, SpecialAmount. NB `ReturnInvoiceDAL` (PAGE-294) writes the correctly spelt `ReturnInvoiceDetailId` — the two DALs target differently named columns of the same table (GAP-M08-004).
- **tblDCStoreFreeze** (W — **stock effect: freeze store**): DCStoreId (original), DCStoreFreezeId (MAX+1), InvoiceDetailId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo, TotalQuantity=dQty, ExpDate, ReceiveDate, ChalanNo, ChalanDate, ComUnitId, StockQty=dQty, DamageQty 0, StockRcvDate, ReqId, ReqChildId, StockInTransfarId, StockCondition='ReturnStock', ChalanDetailsId. **`tblDCStore` is NOT changed** by this page.
- R: tblInvoice, tblInvoiceDetail, tblProduct, tblCustMaster, tblUnitPrice, tblOrder/tblOrderDetail, tblCompanyUnit.

## 18. Validation Matrix
| Check | Where | Message |
|---|---|---|
| Order no non-empty | `Validation` :42-46 | Please Input Order Number!! |
| Order date non-empty | :47-51 | Please Input Order Date!! |
| No grid row with blank code or blank TQty | :58-66 | Please Remove Blank Row!! |
| dQty + previous returned ≤ TQty | `dQtyTextBox_TextChanged` :981,1058-1063 | Return Quantity Must Be Smaller or equel then Pre Qty (row unticked, dQty cleared) |
| Not validated | — | no check that at least one row is ticked (a save with none creates an empty master), dQty numeric (Convert.ToDecimal on blank throws), stock, payment |

## 19. Business Rules
| BR | Rule | Source | Impl. | Conf. |
|---|---|---|---|---|
| BR-M08-040 | Only ticked lines are returned; a line's cumulative returned qty (PRQty = Σ earlier `tblReturnInvoiceDetail.TotalQuantity` for that original invoice detail + this RQty) may not exceed the line's total delivered qty (TQty). | .aspx.cs:981,1085-1091; OrderInfoDAL.cs:583-591 | code-behind | CONFIRMED |
| BR-M08-041 | Return-line VAT is recomputed as `TP × product VAT % / 100` and discount % is re-derived from the *order's* trade terms (`GetTradeTerm`), instead of using the invoiced unit VAT/discount. | .aspx.cs:1001-1054 | code-behind | CONFIRMED |
| BR-M08-042 | **Amount defect:** the saved detail `TotalPrice`/`TotalPriceVatAmount` are computed from `qtyTextBox` (delivered quantity) × unit price/VAT, while `Quantity/TotalQuantity` are the returned qty (`dQty`) — saved amounts do not match saved quantities; header totals come from the screen (return-based). | .aspx.cs:727-763,834-837 | code-behind | CONFIRMED |
| BR-M08-043 | Returned stock is **not** re-admitted to `tblDCStore`; a `tblDCStoreFreeze` row (`StockCondition='ReturnStock'`, `StockQty=dQty`) records it. Nothing is posted to accounts from this page. | .aspx.cs:768-802; InvoiceDAL.cs:1864-1900 | code-behind + SQL | CONFIRMED |
| BR-M08-044 | Return Invoice No = `'INV-' + <ComUnitCode> + '-' + zero-padded (COUNT of that unit's tblReturnInvoice rows + 1)` (8 digits) — same `INV-` prefix as sales invoices; count-based so deletions create duplicates. | InvoiceBLL.cs:553-596; InvoiceDAL.cs:1204-1211 | BLL | CONFIRMED |
| BR-M08-045 | `ProformaorInvoiceReturn` (PAGE-288) refuses to delete a delivery invoice when its lines already have freeze stock consumed (`tblDCStoreFreeze.StockQty <> TotalQuantity`) — the freeze rows created here are the trigger. | ProformaOrInvoiceReturn.cs:156-163; ProformaorInvoiceReturn.aspx.cs saveButton_Click | code-behind | CONFIRMED |

## 20. Permission Matrix
`Session["UserId"]` used at save; no role check; DC list unfiltered (`LoadSC(ddl)`), any user can return any DC's invoice.

## 21. Status Lifecycle
No status fields. `tblDCStoreFreeze.StockCondition='ReturnStock'` marks quarantined return stock; consumption elsewhere lowers `StockQty` below `TotalQuantity`.

## 22. Error Handling
None (no try/catch); malformed decimals surface as unhandled `FormatException`. `showMessageBox` alerts.

## 23. Audit
`UserId` on the return invoice; no CreateBy/CreateDate set by this insert (columns not in this INSERT).

## 24. Integration
None (no accounts, no SAP).

## 25. Reports
`ReturnInvoiceReportViewer.aspx` → `rptReturnInvoiceForCustomer.rpt` (RPT-M08-03) reads `tblReturnInvoice`/`Detail` by return invoice number.

## 26. Dependencies
`SInventoryWebService.asmx` (GetCustomer, GetProduct2), CalendarExtender, tables above.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | Search delivered invoice, tick one line, RQty ≤ TQty, generate | header + detail + freeze row (ReturnStock) | 040,043 |
| T2 | Negative | RQty + PRQty > TQty | row unticked; alert | 040 |
| T3 | Edge | Two returns for same line summing to TQty | second allowed if within remainder | 040 |
| T4 | Edge | Save with no ticked line | empty return header saved | — (GAP) |
| T5 | Defect | Return 2 of 5 delivered | detail TotalPrice = 5 × price while Quantity = 2 | 042 |
| T6 | Edge | Deleted return earlier then create new for same unit | return no may repeat (count-based) | 044 |
| T7 | Integration | Compare with PAGE-294 schema | insert fails if only one of `ReturnInvoiceDetailId`/`ReuturnInvoiceDetailId` exists | 040 |

## 28. Reverse Engineering References
`.aspx.cs:18-116,660-1131`; `InvoiceBLL.cs:108-126,553-596`; `InvoiceDAL.cs:441-505,1204-1211,1864-1900`; `OrderInfoDAL.cs:570-591`.

## 29. Spec vs Actual
None existed — behaviour above is the only definition.

## 30. Known Gaps
Amount/quantity mismatch (BR-M08-042); column-name typo/dual DAL (`ReuturnInvoiceDetailId`); no accounting and no DCStore reinstatement (stock only frozen) — returns via this page do not affect available stock or GL; `GetCustInfo` resolves the customer by order no; no ticked-row check; ~500 lines legacy code; no transaction; PK by `MAX+1` (race).

## 31. Change Impact
`InvoiceBLL.SaveReturnInvoice*`, `InvoiceDAL.SaveDataForReturnInvoice*` are used only by this page among M08 pages (grep). `tblDCStoreFreeze` rows created here are consumed by freeze-stock pages and the "delete delivery invoice" guard (PAGE-288). Replacing this page with PAGE-287/294 flows changes GL impact (they post vouchers).
