---
page_id: PAGE-267
module: M07
page: InvoiceCreationForCustomer
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "PARTIAL", "tests": "PARTIAL"}
---

# PAGE-267 — InvoiceCreationForCustomer

> **Evidence level:** Tier A - reviewed 2026-09-19 by reading the .aspx, .aspx.cs, the BLL/DAL methods reached, and every SP/SQL body present in the repository. Confidence tags: CONFIRMED (read in implementation), INFERRED (implied by several references), UNKNOWN (not derivable; SP bodies are absent from the repo except where stated). Module: M07 - Proforma, Invoicing & Delivery.


## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-267 |
| Module | M07 - Proforma, Invoicing & Delivery |
| Page name | InvoiceCreationForCustomer |
| Kind | Entry / Transaction (manual proforma invoice for one customer/order) - **legacy; runtime failure path in the current code (BR-M07-019)** |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/MainMasterPage.master (`InvoiceCreationForCustomer.aspx:1`) |
| Page class | SInventory_UI_InvoiceCreationForCustomer |
| aspx size | 448 lines; code-behind 594 lines |
| Suspected legacy/duplicate | YES (CONFIRMED): the page class name `SInventory_UI_InvoiceCreationForCustomer` is declared again by `ReturnInvoiceCreationForCustomer.aspx.cs:13` (copy/paste clone); the sibling `InvoiceCreationForCustomerManual.aspx.exclude` is excluded from the build; no inbound link found by grep. The live invoice screens are the M06 by-order pages (`../order-management/pages/PAGE-217-invoicecreationbyorder.md`, `PAGE-218-invoicecreationforcustomerbyorder.md`). |

## 2. Purpose
CONFIRMED (labels): heading "Invoice" (`InvoiceCreationForCustomer.aspx:13`). A user types/selects a customer and an **order number**, adds product lines (code or name autocomplete), the page shows stock, unit price, VAT, discount and net per line and totals, and **Invoice Generate** saves an invoice header, allocates DC stock batch by batch and writes invoice detail lines; **Print** opens the invoice PDF viewer.

## 3. Business Objective
INFERRED: allow a DC operator to raise a proforma invoice manually (outside the automatic order->invoice pipeline), e.g. for a customer/order that is not present in the automated path, and immediately print it. Business effect (if it worked): a `tblInvoice` header (no DelivaryInvoiceNo => proforma), `tblInvoiceDetail` batch lines, `tblInvoiceBatch` row and `tblDCStore` 'Issue' rows that reserve stock. See BR-M07-011..018 and `../workflows/WF-M07-02-manual-invoice-and-stock-allocation.md`. Because of BR-M07-019 the save fails after the header row is written.

## 4. Navigation
Menu DB-driven -> UNKNOWN. Inbound: none found (grep of `.aspx/.cs/.master` for `InvoiceCreationForCustomer.aspx` excluding ByOrder). Outbound: `../SInventory_RPTVIEW/InvoiceReportViewer.aspx?InvNo=<invTextBox>` in a popup (`InvoiceCreationForCustomer.aspx.cs:546-552`).

## 5. Route
`~/SInventory_UI/InvoiceCreationForCustomer.aspx` (file-based WebForms route; no `MapPageRoute` alias).


## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx |
| Code-behind | Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs |
| Master | Solution.Web/MasterPages/MainMasterPage.master |
| Web service (autocomplete) | Solution.Web/App_Code/SInventoryWebService.cs (`GetCustomer` :104, `GetProduct2` :485) via `SInventory_UI/SInventoryWebService.asmx` |
| BLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs (`SaveInvoice` :86, `SaveInvoiceDetails` :141, `UpdateDCStock` :167, `InvoiceDetailInsertBLL` :362, `InvoiceNoGenerator` :495, `ProductDiscount` :680, `CustomerMaster` :655, `ProductInfo` :635, `PaymentTypeLoadBLL` :664), Library.BLL/SInventory_BLL/OrderInfoBLL.cs (`LoadInvoiceBatchId`) |
| DAL | Library.DAL/SInventory_DAL/InvoiceDAL.cs (see §15), `SInventorySql.cs`, `InternalCls/ClsCommonInternalDAL.cs`, `ClsPrimaryKeyFind` |
| DAO | Library.DAO/SInventory_Entities/Invoice.cs, InvoiceDetail.cs |

## 7. UI Control Inventory
| Control ID | Type | Label | Required | Default | Read-only / Enabled / Visible | Data source | Events / error |
|---|---|---|---|---|---|---|---|
| custCodeTextBox | TextBox | "Customer Code" | no (only tested for non-empty inside `GetCustInfo`) | empty | editable, AutoPostBack | typed | `custCodeTextBox_TextChanged` -> `GetCustInfo` |
| custNameTextBox | TextBox | "CustomerName" | no | empty | editable, AutoPostBack; AutoCompleteExtender -> `SInventoryWebService.GetCustomer` (`code:name`, top 20, ComUnit-scoped for non-admin) | typed / autocomplete | `custNameTextBox_TextChanged`: if text contains ':' puts part[0] into custCodeTextBox and calls `GetCustInfo` |
| custAddressTextBox, comUnitNameTextBox ("Salse Center"), miaCodeTextBox, miaNameTextBox, districtNameTextBox ("Distirict"), areaNameTextBox, marketNameTextBox ("Merket"), custCategoryTextBox | TextBox | as labelled | - | empty | ReadOnly | filled by `GetCustInfo` from `View_OrderCustomerInfo` row for the typed **order code** | - |
| hdCustomerMasterId, hdComUnitId, hdMiaId | HiddenField | - | - | empty | hidden | from `View_OrderCustomerInfo` (`CustomerMasterId`, `ComUnitId`, `MiaId`) | if the lookup returns no row they stay empty -> `Convert.ToInt32("")` FormatException at save |
| invDateTextBox | TextBox | "Invoice Date" | - | today `dd-MMM-yyyy` (`Todate()`) | ReadOnly | server clock | - |
| orderNoTextBox | TextBox | "Order No" | YES: "Please Input Order Number!!" | empty | editable | typed | - |
| orderDateTextBox (+CalendarExtender dd-MMM-yyyy) | TextBox | "Order Date" | YES: "Please Input Order Date!!" | empty | editable | typed / calendar | - |
| payTypeDDL | DropDownList | "Payment Type" | YES (`payTypeDDL.Text == ""` -> "Please Input Payment Type!!") | first item | enabled | `InvoiceBLL.PaymentTypeLoadBLL` -> `select * from tblPaymentType` (PaymentTypeName / PaymentTypeId) | - |
| gridLineItemGridView (14 columns) | GridView | SL, Code, Name, CStock, UP, UVAT, Qty, TP, TPVAT, DP, DAmt, NP, BQty, TQty | at save: every row needs Code and TQty ("Please Remove Blank Row!!") | one blank row (`InitialGrid`) | UP and UVAT columns hidden (`Columns[4/5].Visible=false`); CStock/UP/UVAT/TP/TPVAT/DP/DAmt/NP/TQty ReadOnly; BQty ReadOnly (so bonus can never be typed) ; Qty editable | see below | add/remove line image buttons |
| codeTextBox (FilteredTextBox) | TextBox | Code | - | - | editable, AutoPostBack | typed | `codeTextBox_TextChanged` -> `GetProduct` |
| nameTextBox | TextBox | Name | - | - | editable; AutoComplete `GetProduct2` (`Name:Code`) | typed | `nameTextBox_TextChanged`: text must contain ':' - takes part[1] as code |
| qtyTextBox | TextBox | Qty | at save only via TQty | - | editable, AutoPostBack | typed | `qtyTextBox_TextChanged` (`Convert.ToDecimal` - FormatException on non-numeric/empty) |
| tpTptalTextBox / vatTotalTextBox / disTotalTextBox / grandTotalTextBox | TextBox | "TP Total", "VAT Total", "Discount Total", "Grand Total" | - | - | ReadOnly | `TotalValueCalculation()` | - |
| saveButton | Button | "Invoice Generate" | see §18 | - | enabled | - | `saveButton_Click` |
| invTextBox / printButton | TextBox / Button | "Print InvNo" / "Print" | none | filled after save | editable | - | `printButton_Click` opens viewer popup |

## 8. Field Mapping
| UI control | Entity property | BLL/DAL param | Table.column | Notes |
|---|---|---|---|---|
| invDateTextBox | `Invoice.InvoiceDate` | `@InvoiceDate` | tblInvoice.InvoiceDate | `Convert.ToDateTime` |
| orderNoTextBox | `Invoice.OrderNo` | `@OrderNo` | tblInvoice.OrderNo | `OrderId` is NOT set (0/NULL) |
| orderDateTextBox | `Invoice.OrderDate` | `@OrderDate` | tblInvoice.OrderDate | |
| hdCustomerMasterId | `CustomerMasterId` | `@CustomerMasterId` | tblInvoice.CustomerMasterId | |
| hdComUnitId | `ComUnitId` | `@ComUnitId` | tblInvoice.ComUnitId | also drives stock lookup |
| hdMiaId | `MiaId` | `@MiaId` | tblInvoice.MiaId | |
| payTypeDDL | `PaymentTypeId` | `@PaymentTypeId` | tblInvoice.PaymentTypeId | |
| tpTptalTextBox | `TpTotal` | `@TpTotal` | tblInvoice.TpTotal | later overwritten by `UpdateInvoiceHeaderTotals` |
| disTotalTextBox | `TpDiscount` | `@TpDiscount` | tblInvoice.TpDiscount | ditto |
| vatTotalTextBox | `TpVat` | `@TpVat` | tblInvoice.TpVat | ditto |
| grandTotalTextBox | `TpGrandTotal` | `@TpGrandTotal` | tblInvoice.TpGrandTotal | ditto |
| Session["UserId"] | `UserId` | `@UserId` | tblInvoice.UserId | |
| comUnitNameTextBox (`CODE:Name`) part[0] | `ComUnitCode` | not stored; used only in `InvoiceNoGenerator` | - | |
| line: codeTextBox | `InvoiceDetail.ProductCode` | `@ProductCode` | tblInvoiceDetail.ProductCode | |
| line: nameTextBox `Name:Pack` | `ProductName`, `PackSize` | `@ProductName`,`@PackSize` | tblInvoiceDetail.ProductName / PackSize | split on ':' - IndexOutOfRange if no ':' |
| line: unitPriceTextBox (hidden col) | `UnitPrice` | `@UnitPrice` | tblInvoiceDetail.UnitPrice | |
| line: upVatTextBox | `UnitVatAmount` | `@UnitVatAmount` | tblInvoiceDetail.UnitVatAmount | |
| line: qtyTextBox | `Quantity` (split per batch) | `@Quantity` | tblInvoiceDetail.Quantity | |
| line: dpTextBox | `DiscountPercentage` | `@DiscountPercentage` | tblInvoiceDetail.DiscountPercentage | |
| line: bQtyTextBox | `BonusQuantity` | `@BonusQuantity` | tblInvoiceDetail.BonusQuantity | always 0 (BQty forced to "0" at `.aspx.cs:409`) |
| not mapped | `OrderDetailsId`, `SpecialAmountPer`, `AdjustmentAmount`, `Campaign*`, `ISGiftProductforInv` | - | - | left at defaults (0/null) - see BR-M07-019 |

## 9. Dropdown Mapping
| Dropdown | Loader | SQL | Display / Value |
|---|---|---|---|
| payTypeDDL | `Page_Load` -> `InvoiceBLL.PaymentTypeLoadBLL` -> `InvoiceDAL.PaymentTypeLoad` (`InvoiceDAL.cs:867`) | `select * from tblPaymentType` | PaymentTypeName / PaymentTypeId |

Autocomplete (not dropdowns): customer (`GetCustomer`: `tblCustMaster` active rows, `code:name`, scoped to `Session["ComUnitId"]` unless Admin), product (`GetProduct2`: `tblProduct` name prefix, `Name:Code`).

## 10. Button Action Mapping
| Button | Event | Handler | BLL | DAL | SQL/SP | DB change |
|---|---|---|---|---|---|---|
| saveButton | click | `saveButton_Click` (`.aspx.cs:427`): `Validation()`; `SaveInvoice()`; `SaveInvoiceDetail(invId)`; `Clear()`; alert "Create Invoice" | `InvoiceBLL.SaveInvoice`, `SaveInvoiceDetails` | `InvoiceDAL.SaveDataForInvoice`, `SaveDataForInvoiceBatch`, `BatchWiseProductQty`, `SaveDataForInvoiceDetails`, `UpdateDCStoreQuantity`, `UpdateInvoiceHeaderTotals` | inline SQL only (no SP) | tblInvoice (INSERT, later UPDATE totals), tblInvoiceBatch (INSERT), tblInvoiceDetail (INSERT per batch), tblDCStore (INSERT Issue rows) |
| addImageButton / removeImageButton | click | add blank row / rebuild grid without the row | - | - | - | none |
| printButton | click | `printButton_Click` -> popup `InvoiceReportViewer.aspx?InvNo=` | - | - | - | none |

## 11. Frontend Flow
UpdatePanel (no UpdateProgress). Page_Load(!PostBack): payment types, invoice date=today, one blank grid row. Typing customer name (autocomplete) or code -> postback -> `GetCustInfo(custCode)` which ignores the code and queries `View_OrderCustomerInfo WHERE OrderCode=@OrderCode` with **`orderNoTextBox.Text`** (`InvoiceCreationForCustomer.aspx.cs:65`, `InvoiceDAL.cs:639-646`) - so the user must type the order number first. Entering a product code/name triggers `GetProduct`: duplicate code check (`ProductCodeValidation`), then `ProductInfoDAL(comUnitId, code)` -> name:pack, current stock, unit price, unit VAT. Entering Qty triggers `qtyTextBox_TextChanged` which recomputes the row and grand totals server-side (no JS calculations). Grid rebuilds by copying every row into a `DataTable` (postback-heavy).

## 12. Backend Flow
| Handler | Source | Flow / branches | try/catch | Txn |
|---|---|---|---|---|
| Page_Load | `.aspx.cs:16-24` | init | no | no |
| GetCustInfo | `.aspx.cs:60-86` | fills header fields if one row found; else silently nothing | no | no |
| GetProduct | `.aspx.cs:303-342` | dup code -> alert "<code> No: Product Already Inserted!!!" and clears row; no product row -> alert "No Any Stock or Product of <code>" and clears row | no | no |
| qtyTextBox_TextChanged | `.aspx.cs:368-411` | see BR-M07-009 | no (FormatException surfaces) | no |
| saveButton_Click | `.aspx.cs:427-436` | `Validation()` -> `SaveInvoice()` (`InvoiceBLL.SaveInvoice`: `PrimaryKeyMax("invoiceId","tblInvoice")`, `InvoiceNoGenerator`, batch id, `SaveDataForInvoiceBatch`, `SaveDataForInvoice`) -> `SaveInvoiceDetail` (`InvoiceBLL.SaveInvoiceDetails`: per line `UpdateDCStock` FEFO allocation -> `InvoiceDetailInsertBLL` -> `SaveDataForInvoiceDetails` + `UpdateDCStoreQuantity`; then `UpdateInvoiceHeaderTotals`) -> `Clear()` -> alert | no | **no** - every statement is an independent auto-commit (`SInventorySql.Execute` -> `ClsCommonInternalDAL.UpdateDataByUpdateCommandNew`, `ClsCommonInternalDAL.cs:199-224`) |

## 13. API Mapping
This page exposes no `[WebMethod]`. Client AJAX targets: none.


## 14. Service Mapping
| BLL method | Source |
|---|---|
| InvoiceBLL.PaymentTypeLoadBLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs:665 |
| InvoiceBLL.CustomerMaster | Library.BLL/SInventory_BLL/InvoiceBLL.cs:656 |
| InvoiceBLL.ProductInfo | Library.BLL/SInventory_BLL/InvoiceBLL.cs:636 |
| InvoiceBLL.ProductDiscount | Library.BLL/SInventory_BLL/InvoiceBLL.cs:681 |
| InvoiceBLL.SaveInvoice | Library.BLL/SInventory_BLL/InvoiceBLL.cs:87 |
| OrderInfoBLL.LoadInvoiceBatchId | Library.BLL/SInventory_BLL/OrderInfoBLL.cs:75 |
| InvoiceBLL.InvoiceNoGenerator | Library.BLL/SInventory_BLL/InvoiceBLL.cs:496 |
| InvoiceBLL.SaveInvoiceDetails | Library.BLL/SInventory_BLL/InvoiceBLL.cs:142 |
| InvoiceBLL.UpdateDCStock | Library.BLL/SInventory_BLL/InvoiceBLL.cs:129 |
| InvoiceBLL.InvoiceDetailInsertBLL | Library.BLL/SInventory_BLL/InvoiceBLL.cs:363 |
| InvoiceBLL.UpdateInvoiceHeaderTotals | Library.BLL/SInventory_BLL/InvoiceBLL.cs:157 |


## 15. Repository Mapping
| DAL method | Source | SQL | Params |
|---|---|---|---|
| InvoiceDAL.SaveDataForInvoice | `InvoiceDAL.cs:361-421` | INSERT tblInvoice (43 columns incl. InvoiceNo, CustomerType, CreateDate, AdjustAmount, ReceivableAmount, InvoiceDate, OrderNo, OrderDate, CustomerMasterId, ComUnitId, MiaId, PaymentTypeId, TpTotal, TpDiscount, TpVat, TpGrandTotal, UserId, OrderId, TotalSpecialAmount, Inv_DANameId, MIACode/MIAName/MarketCode/MarketName/AreaCode/DisCode/FEName/RegionCode/DZSMName, FixedCustomer, DeliveryPersonName/PhNo, CustomerDeliveryAddress, DeliveryCharge ...) | `Param()` wraps null as DBNull |
| InvoiceDAL.SaveDataForInvoiceBatch | `InvoiceDAL.cs:423-435` | INSERT tblInvoiceBatch(BatchNo, Date=GETDATE(), InvoiceId) | @BatchNo, @InvoiceId |
| InvoiceDAL.BatchWiseProductQty | `InvoiceDAL.cs:921-960` | FEFO batch query on tblDCStore (see BR-M07-013) | @ProductCode, @ComUnitId |
| InvoiceDAL.SaveDataForInvoiceDetails | `InvoiceDAL.cs:473-486`, params `InvoiceDAL.cs:534-565` | INSERT tblInvoiceDetail (25 columns) | `Campaign` hard-coded "N" |
| InvoiceDAL.UpdateDCStoreQuantity | `InvoiceDAL.cs:666-701` | INSERT tblDCStore Issue row (copy of Receive row) | @IssueQty, @DCStoreId |
| InvoiceDAL.Isgift | `InvoiceDAL.cs:713-720` | `select ISGiftProduct from tblOrderDetail where OrderDetailId=@OrderDetailId` | @OrderDetailId |
| InvoiceDAL.UpdateInvoiceHeaderTotals | `InvoiceDAL.cs:507-535` | UPDATE tblInvoice from SUM(tblInvoiceDetail) | @InvoiceId |
| InvoiceDAL.ProductInfoDAL | `InvoiceDAL.cs:588-603` | tblProduct LEFT JOIN tblUnitPrice, View_DCStoreCurrentStock | @ComUnitId, @ProductCode |
| InvoiceDAL.DiscountofProduct | `InvoiceDAL.cs:912-919` | `SELECT TOP 1 * FROM tblProductDiscount WHERE ProductCode=@ProductCode AND (Status='Active' OR Status='1' OR Status IS NULL)` (no ORDER BY, quantity parameter unused) | @ProductCode |
| InvoiceDAL.InvoiceNoCount | `InvoiceDAL.cs:1197-1202` | `SELECT isnull(MAX(InvoiceId),0)+1 FROM tblInvoice` | - |
| InvoiceDAL.LoadCustomerMaster | `InvoiceDAL.cs:639-646` | `SELECT * FROM View_OrderCustomerInfo WHERE OrderCode=@OrderCode` | @OrderCode |

## 16. Stored Procedure Mapping
No stored procedure is called by this page. SPs indirectly relevant to the resulting invoice: `sp_Delete_ProformaInvoice` (body in repo: `sp_Delete_ProformaInvoice_Update.sql:7`), `sp_Check_anomalyInvoiceDetails` (`SP_Fix_Invoice_Anomaly_And_Sync.sql:42`) - see PAGE-277.

## 17. Database Mapping
- READ: tblPaymentType, tblProduct, tblUnitPrice, View_DCStoreCurrentStock (body UNKNOWN), View_OrderCustomerInfo (body UNKNOWN), tblProductDiscount, tblOrderDetail (ISGiftProduct), tblDCStore, tblInvoice (MAX id), tblCustMaster (autocomplete).
- WRITTEN: `tblInvoice` (INSERT then UPDATE totals), `tblInvoiceBatch` (INSERT), `tblInvoiceDetail` (INSERT), `tblDCStore` (INSERT Issue rows). Keys are generated by `ClsPrimaryKeyFind.PrimaryKeyMax` (max+1, non-atomic; race between concurrent users - INFERRED from name/usage, source of `ClsPrimaryKeyFind` not opened).

## 18. Validation Matrix
| Rule | Layer | Condition | Message |
|---|---|---|---|
| Order number required | server `Validation()` `.aspx.cs:33` | `orderNoTextBox.Text == ""` | "Please Input Order Number!!" |
| Order date required | server | `orderDateTextBox.Text == ""` | "Please Input Order Date!!" |
| Payment type required | server | `payTypeDDL.Text == ""` (Text of a DDL is the selected item's text; list always has items -> effectively never fails) | "Please Input Payment Type!!" |
| No blank rows | server | any row with empty codeTextBox or tQtyTextBox | "Please Remove Blank Row!!" |
| Numeric qty | FilteredTextBoxExtender (client) + `Convert.ToDecimal` (server) | non-numeric -> unhandled FormatException | - |
| Duplicate product | server `ProductCodeValidation` | same code in another row | "<code> No: Product Already Inserted!!!" |
| Unknown product | server `GetProduct` | `ProductInfo` returns no row | "No Any Stock or Product of <code>" |
| Stock sufficiency | none - stock is displayed only (CStock) | - | - |
| Customer/order lookup | none - silent when the order code is not found | - | - |
| Grid non-empty, qty > 0, ordering | none | - | - |

## 19. Business Rules
| BR ID | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M07-006 | Customer/order header comes from the **order number**: `GetCustInfo` ignores the customer code value and reads `View_OrderCustomerInfo` by `OrderCode = orderNoTextBox.Text`; save requires order number and order date, and rejects blank rows; payment type check is ineffective. | `InvoiceCreationForCustomer.aspx.cs:31-58, 60-86`; `InvoiceDAL.cs:639-646` | code-behind + inline SQL | CONFIRMED |
| BR-M07-007 | A product code may appear only once in the grid. | `.aspx.cs:344-360` | code-behind | CONFIRMED |
| BR-M07-008 | Product line info: unit price `tblUnitPrice.UnitPrice`, VAT per unit `tblUnitPrice.VATAmountPerUnit`, stock `View_DCStoreCurrentStock.TotalQty` for the order's ComUnit; product not found -> row cleared. | `InvoiceDAL.cs:588-603`; `.aspx.cs:303-342` | inline SQL | CONFIRMED (view body UNKNOWN) |
| BR-M07-009 | Line calculation (postback): TP = UnitPrice x Qty; TPVAT = UnitVAT x Qty; DP% = first active `tblProductDiscount.DiscountPercentage` for the product (0 if none; quantity slab ignored); DAmt = TP/100 x DP% (0 when DP=0); NP = (TP - DAmt) + TPVAT; TQty = Qty; BQty forced to 0. No rounding applied (raw `decimal.ToString()`). | `.aspx.cs:368-411`; `InvoiceBLL.cs:680-689`; `InvoiceDAL.cs:912-919` | code-behind | CONFIRMED |
| BR-M07-010 | Header totals shown = sums of the grid columns: TP Total = sum(TP), VAT Total = sum(TPVAT), Discount Total = sum(DAmt), Grand Total = sum(NP). | `.aspx.cs:524-545` | code-behind | CONFIRMED |
| BR-M07-011 | Invoice number = `"INV-" + ComUnitCode + (MAX(InvoiceId)+1 left-padded to 10 digits)` - the counter is the global max of tblInvoice.InvoiceId, not per DC and not atomic. `InvoiceId` itself = `PrimaryKeyMax("invoiceId","tblInvoice")`. | `InvoiceBLL.cs:86-90, 495-501`; `InvoiceDAL.cs:1197-1202` | BLL + inline SQL | CONFIRMED |
| BR-M07-012 | Every invoice gets a `tblInvoiceBatch` row (BatchNo, Date=GETDATE(), InvoiceId). BatchNo = `MAX(numeric suffix after '-' of existing BatchNo values) + 1`, prefixed with `Session["RouteId"] + "-"` when RouteId is set and not "0" (otherwise the bare number). Because only BatchNo values containing '-' are considered by the MAX, invoices created without a RouteId get a number that is not counted by later runs (INFERRED: numbers may repeat). Used by InvoiceCreationByOrder (M06) to find all invoices of a batch. | `InvoiceBLL.cs:92-105`; `OrderInfoDAL.cs:264-277`; `InvoiceDAL.cs:423-435`; `InvoiceCreationByOrder.aspx.cs:115-135` | BLL + inline SQL | CONFIRMED (query read) / INFERRED (repeat behaviour) |
| BR-M07-013 | **Stock allocation is FEFO over `tblDCStore`**: candidate batches = Receive rows of the product in the DC with available quantity > 0 where available = (TotalQuantity if > 0 else StockQty) + SUM(StockQty of Issue rows referencing it via ReferenceDCStoreId); ordered by ExpDate ASC, BatchNo ASC. For each batch consumed an `Issue` row is INSERTed (StockQty = -issued, TotalQuantity 0, Trans_Type 'Issue', ReferenceDCStoreId = receive row, CostPerUnit copied, TotalCostAmount = -issued x CostPerUnit); the Receive row is never decremented. One `tblInvoiceDetail` row is written per batch used (so one ordered line can become several detail lines). | `InvoiceDAL.cs:921-960, 666-701`; `InvoiceBLL.cs:167-256` | BLL + inline SQL | CONFIRMED |
| BR-M07-014 | Bonus quantity is allocated first (from the earliest batch(es)); if a batch cannot cover it, the overflow is booked as a detail row with Quantity 0 and bonus = overflow, then the paid quantity continues in the next batch. TotalQuantity per detail = Quantity + BonusQuantity. If total stock across batches is less than the requested quantity the loop ends silently and only the available quantity is invoiced (no error). | `InvoiceBLL.cs:167-256, 362-421` | BLL | CONFIRMED (control flow read; edge cases not executed) |
| BR-M07-015 | Gift product lines: when `tblOrderDetail.ISGiftProduct` is true for the line's OrderDetailsId, TotalPrice, TotalPriceVatAmount and NetAmount are stored as 0 (stock is still issued). | `InvoiceBLL.cs:391-420` | BLL | CONFIRMED |
| BR-M07-016 | Persisted detail formulas (per batch row): TotalPrice = qty x UnitPrice; DiscountAmount = TotalPrice x DiscountPercentage/100; SpecialAmount = TotalPrice x SpecialAmountPer/100 (stored, NOT deducted from NetAmount); TotalPriceVatAmount = UnitVatAmount x qty (VAT not reduced by discount); NetAmount = (TotalPrice - DiscountAmount - AdjustmentAmount) + TotalPriceVatAmount; CostPrice = tblUnitPrice.CostPrice; ExpDate/ReceiveDate/BatchNo from the batch row. Bonus units carry no price. No rounding. | `InvoiceBLL.cs:362-421` | BLL | CONFIRMED |
| BR-M07-017 | After all lines are saved, the header is recomputed from the details: TpTotal = SUM(TotalPrice); TpDiscount = SUM(DiscountAmount); TpVat = SUM(TotalPriceVatAmount); TpGrandTotal = SUM(NetAmount) + DeliveryCharge; ReceivableAmount = TpGrandTotal - AdjustAmount. The header numbers typed on the page are therefore overwritten. | `InvoiceDAL.cs:507-535`; `InvoiceBLL.cs:141-154` | inline SQL | CONFIRMED |
| BR-M07-018 | There is no transaction around header, batch, detail and stock-issue writes; a failure at any step leaves partial data (header without details, or Issue rows without detail). Remediation scripts exist in the repo for this class of damage (`sp_Check_DCStore_StockAnomalies.sql`, `SP_Adjust_DCStore_Stock_Only.sql`). | `SInventorySql.cs`, `ClsCommonInternalDAL.cs:199-224`; scripts | DAL | CONFIRMED |
| BR-M07-019 | **The page cannot complete a save**: `SaveInvoiceDetail` never sets `OrderDetailsId` (0); `InvoiceDetailInsertBLL` calls `Isgift(0)` and reads `dt.Rows[0]` on an empty result -> ArgumentOutOfRange/IndexOutOfRange after the header and tblInvoiceBatch rows were already inserted. Result: an orphan `tblInvoice` header (TpGrandTotal from the grid, no details) and an unhandled server error. `OrderId` is also never set on the header. | `InvoiceCreationForCustomer.aspx.cs:438-467`; `InvoiceBLL.cs:391-392`; `InvoiceDAL.cs:713-720` | BLL + page | CONFIRMED (static path) / INFERRED (runtime outcome, not executed) |

## 20. Permission Matrix
- Session keys read: `UserId` (invoice creator), `ComUnitId`/`UserType` (only in the autocomplete web service: Admin or empty ComUnitId sees all customers), `RouteId` (batch id prefix).
- No role or permission check in the page; access = menu only. `GetCustomer` is scoped to the user's ComUnit for non-admin, but the save path does not verify that the order's ComUnit equals the user's ComUnit (the order number comes from free text).

## 21. Status Lifecycle
The page creates a proforma: `tblInvoice.DelivaryInvoiceNo` is not written (NULL = proforma). No status column is set here. Later transitions (loading summary / delivery confirmation / payment) are in `PAGE-271`, `PAGE-264` and the README lifecycle. Deletion of an unconfirmed proforma is `sp_Delete_ProformaInvoice` (BR-M07-037..039).

## 22. Error Handling
No try/catch. FormatException / IndexOutOfRange surface as unhandled errors (BR-M07-019). Validation errors use `alert()`.

## 23. Audit
tblInvoice: `UserId` = session user, `CreateDate` = `aInvoice.Createdate` (not set on this page -> null/default; `CreateBy` not written by this INSERT). tblDCStore Issue rows: `ReferenceTransactionDate = GETDATE()`. No audit log table written on create.

## 24. Integration
None. (Accounting journal posting happens only at delivery confirmation, `sp_Sales_DeliverySalesPosting`, see PAGE-271.)

## 25. Reports
`InvoiceReportViewer.aspx?InvNo=<InvNo>` (Print button) -> `rptInvoiceForCustomer_Uncr.rpt`; see `../reports/RPT-M07-01-invoice-and-proforma-print.md`.

## 26. Dependencies
BLL: InvoiceBLL, OrderInfoBLL. DAL: InvoiceDAL, SInventorySql, ClsCommonInternalDAL, ClsPrimaryKeyFind. DAO: Invoice, InvoiceDetail. Web service: SInventoryWebService.GetCustomer/GetProduct2. Views: View_OrderCustomerInfo, View_DCStoreCurrentStock (definitions not in repo).

## 27. Test Cases
| ID | Scenario | Steps | Expected | BR |
|---|---|---|---|---|
| TC-267-01 | Missing order no | leave Order No empty, Generate | alert "Please Input Order Number!!" | BR-M07-006 |
| TC-267-02 | Missing order date | fill order no only | alert "Please Input Order Date!!" | BR-M07-006 |
| TC-267-03 | Blank grid row | leave code/qty blank | alert "Please Remove Blank Row!!" | BR-M07-006 |
| TC-267-04 | Duplicate product | add same code twice | second row cleared, alert "... Already Inserted!!!" | BR-M07-007 |
| TC-267-05 | Unknown product | code that does not exist | alert "No Any Stock or Product of X" | BR-M07-008 |
| TC-267-06 | Calculation | UP=100, UVAT=15, Qty=10, discount row 5% | TP=1000, TPVAT=150, DAmt=50, NP=1100, TQty=10, BQty=0 | BR-M07-009 |
| TC-267-07 | Totals | two lines | header totals equal column sums | BR-M07-010 |
| TC-267-08 | Save happy path (expected by design) | valid customer/order, stock available | invoice header + details + Issue rows | BR-M07-011..017 - **currently fails with IndexOutOfRange (BR-M07-019); assert that an orphan header remains** |
| TC-267-09 | Stock split | qty 25 with batches 10 (expiry earlier) and 20 | two detail rows 10 and 15, two Issue rows -10 and -15 | BR-M07-013 |
| TC-267-10 | Insufficient stock | qty 100, total stock 30 | only 30 invoiced silently | BR-M07-014 |
| TC-267-11 | Non-numeric qty | type "abc" | FormatException | - |
| TC-267-12 | Concurrent save | two users same second | possible duplicate ids/numbers (max+1) | BR-M07-011 |

## 28. Reverse Engineering References
- `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx`, `Solution.Web/SInventory_UI/InvoiceCreationForCustomer.aspx.cs`
- Extraction tooling: `docs/reverse-engineering/tools/` (read-only static analysis).


## 29. Spec vs Actual
| Area | Specification | Actual Code | Actual DB | Status | Reference |
|---|---|---|---|---|---|
| Prior spec | None existed | page present | n/a | MISSING | - |
| Save flow | None existed | fails at Isgift (no OrderDetailsId) | orphan header | GAP | BR-M07-019 |
| Stock effect | None existed | Issue rows in tblDCStore, FEFO | ledger style | DOCUMENTED | BR-M07-013 |
| Totals | None existed | header overwritten from details incl. DeliveryCharge | - | DOCUMENTED | BR-M07-017 |

## 30. Known Gaps
- Save path throws after header insert (orphan headers) - BR-M07-019 (GAP-M07-004).
- No transaction; no stock sufficiency check (GAP-M07-005).
- `GetCustInfo` semantic mismatch (customer code ignored, order code used) (GAP-M07-006).
- Payment-type "required" check ineffective; qty and numeric parsing unguarded.
- `DiscountofProduct` picks `TOP 1` without ORDER BY and ignores quantity; discount is by product not by customer (contrast reports which join `tblProductDiscount` by customer).
- Duplicate page class name in `ReturnInvoiceCreationForCustomer.aspx.cs` (compile-time conflict risk; GAP-M07-007).
- Autocomplete SQL is parameterised (OK); page uses no string concatenation.
- Special amount is stored but not part of NetAmount; printed discount = DiscountAmount + SpecialAmount in reports (`InvoiceDAL.cs:1598-1615`), so printed lines may not reconcile (INFERRED).

## 31. Change Impact
- `InvoiceBLL.SaveInvoice`, `SaveInvoiceDetails`, `UpdateDCStock`, `InvoiceDetailInsertBLL`, `InvoiceDAL.UpdateDCStoreQuantity`, `BatchWiseProductQty`, `UpdateInvoiceHeaderTotals` are shared with the M06 by-order pages (`InvoiceCreationByOrder`, `InvoiceCreationForCustomerByOrder`, auto-invoice pages) - any change to allocation, FEFO order or header totals changes all of them.
- `ReturnInvoiceCreationForCustomer.aspx` (sales return, another module) copies this page.
- Reports reading these tables: invoice print, proforma reports, top sheet, picking slips (`../reports/`).
- Cross-links: M06 `../order-management/pages/PAGE-217-invoicecreationbyorder.md`, `PAGE-218-invoicecreationforcustomerbyorder.md`.
