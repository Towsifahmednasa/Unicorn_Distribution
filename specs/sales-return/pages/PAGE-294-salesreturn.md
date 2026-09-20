---
page_id: PAGE-294
module: M08
page: SalesReturn
tier: A
generated: false
reviewed: 2026-09-19
coverage: {"ui": "COMPLETE", "frontend": "COMPLETE", "backend": "COMPLETE", "api": "COMPLETE", "database": "PARTIAL", "rules": "COMPLETE", "permission": "COMPLETE", "status": "COMPLETE", "tests": "PARTIAL"}
---
# PAGE-294 — SalesReturn (Market Sales Return, "Return From Market")

> **Evidence level:** Tier A — read in full: `SalesReturn.aspx`, `SalesReturn.aspx.cs` (2,826 lines; ~1,200 lines are dead FOC/legacy code, listed in §30), `ReturnInvoiceBLL`, `ReturnInvoiceDAL`, `BonusCampaignNewDAL.SaveReturnSalesMaster`, and the SP bodies `spInsertReturnInvoice_new` and `sp_SalesReturnPosting` (repo scripts). `database` stays PARTIAL because `tblReturnInvoiceDetail`, `tblReturnAmount`, `tblUnitPrice`, `tblProductDiscount` DDL and the `sp_GET/LoadSC` lookups are not in the repo, and there are **two conflicting bodies of `spInsertReturnInvoice_new`** (see §16 / GAP-M08-003).

## 1. Page Information
| Attribute | Value |
|---|---|
| Page ID | PAGE-294 |
| Module | M08 — Sales Return & Invoice Return |
| Page name | SalesReturn |
| Kind | Entry / Transaction (posts stock + accounting) |
| Web folder | SInventory_UI |
| Master page | ~/MasterPages/NewMasterPage.master |
| Page class | SInventory_UI_SalesReturn (`SalesReturn.aspx.cs:15`) |
| aspx / code-behind size | 647 lines / 2,826 lines |
| Legacy / duplicate | Yes — `SalesReturnList.aspx` is the list page; `SalesReturnList_new.aspx` belongs to the second-time flow (PAGE-296). ~1,200 lines of unused FOC-offer code remain (CONFIRMED dead: no aspx binding, no caller) |

## 2. Purpose
CONFIRMED (title/labels `SalesReturn.aspx:149-150,183`): lets a depot user record goods physically returned **from the market/customer without referencing an original invoice** ("Return From Market"). The user picks a Distribution Center (DC), identifies the customer, selects returned products, enters quantity, batch, manufacturing and expiry date, optional delivery-charge deduction, and submits. One click creates the return invoice, receives the goods back into the DC stock ledger, and auto-posts the accounting reversal vouchers.

## 3. Business Objective
INFERRED from labels and posting code: (a) give credit to the customer (accounts receivable reduced by *Net Return Amount*), (b) put saleable/returned stock back on the depot shelf under its batch/expiry, (c) reverse cost of goods sold at cost price, (d) recover any return-handling cost from the customer via "Deduction Charge" booked as delivery income.

## 4. Navigation
Menu placement is DB-driven (`sp_GET_MainMenuRole2`, `NewMasterPage.master.cs`), exact path UNKNOWN. Inbound: `SalesReturnList.aspx` (back/list link). Outbound: `SalesReturnList.aspx` (`ListImageButton_Click`, `SalesReturn.aspx.cs:92`), `../SInventory_RPTVIEW/ReturnInvoiceReportViewer.aspx?InvNo=` (print, `:707`).

## 5. Route
`~/SInventory_UI/SalesReturn.aspx` (file route, no `MapPageRoute`).

## 6. Source Files
| Type | Path |
|---|---|
| View | Solution.Web/SInventory_UI/SalesReturn.aspx |
| Code-behind | Solution.Web/SInventory_UI/SalesReturn.aspx.cs |
| Master | Solution.Web/MasterPages/NewMasterPage.master |
| BLL | Library.BLL/SInventory_BLL/ReturnInvoiceBLL.cs; OrderInfoBLL.cs (LoadSC, trade-term lookups) |
| DAL | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs; Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:441-539 (`SaveReturnSalesMaster`); OrderInfoDAL.cs (`LoadSC`, `GetFixedCustomerffff`) |
| DAO | Library.DAO/SInventory_Entities/Invoice.cs (`SalesReturnDaoMas`), InvoiceDetail.cs, ReturnAmountDAO.cs |
| SQL scripts | `SP_SalesReturn_Related.sql:18-163`, `Live_Deployment_Script_All_Updates.sql:857-944` (`spInsertReturnInvoice_new`), `:949-1240` (`sp_SalesReturnPosting`) |
| Web service | Solution.Web/SInventory_UI/SInventoryWebService.asmx `GetCustomer` (autocomplete; not read) |

## 7. UI Control Inventory
Required = server-side check in `Validation()` (`SalesReturn.aspx.cs:276-373`); there are **no ASP.NET validator controls** on the page.

| Control | Type / Label | Required | Default | Read-only / visibility | Data source / notes | Error message |
|---|---|---|---|---|---|---|
| salesCenterDropDownList | DropDownList "Distribution Center *" (AutoPostBack, select2) | Yes (unless `hdComUnitId` set) | Session `DICCompanyUnitId` → `ComUnitId` → `SalesCenter`, else first real item (`:49-64`) | Enabled | `OrderInfoBLL.LoadSC(ddl,userId)` = DCs in `tblUserCompanyUnit` for user; falls back to all `tblCompanyUnit` if ≤1 item (`:36-46`) | "Please Select Distribution Center (DC)!!" |
| custCodeTextBox | TextBox "Customer Code *" (AutoPostBack + AutoComplete `GetCustomer`) | Yes | empty | — | `ReturnInvoiceDAL.LoadCustomerMaster` matches `CustomerCode` OR `CustomerName` | "Please Select Customer !!" |
| custNameTextBox | TextBox "Customer Name" (AutoPostBack + AutoComplete) | via code | empty | — | "code : name" split at `:` (`:185-192`) | — |
| custAddressTextBox | TextBox (readonly) | — | — | ReadOnly | customer master `Address` | — |
| lblMioInfo | Label "Sales Person Name *" | Yes | empty | display | `EmpName` from customer master (direct salesperson else MIO of territory else `MIAName` else 'N/A') | "MIO Name can not be empty !!" |
| orderDateTextBox | TextBox "Return Date *" (datepicker, ReadOnly) | Yes | today `dd-MMM-yyyy` | ReadOnly (picker sets value) | becomes `InvoiceDate`+`OrderDate` | "Please Input Order Date!!" |
| orderNoTextBox | TextBox "Remarks *" | Yes | "Return From Market" | editable | saved as `Remarks` (and `OrderNo` is overwritten by "No order", `:606` vs `:638`) | "Please Input Order Number!!" |
| hdCustomerMasterId, hdComUnitId, hdMiaId, hfMIOId, hfMioEmpId, hfTerriId, hfSapCode, orderIdHiddenField, orderHiddenField | HiddenField | — | — | hidden | filled by `GetCustInfo` (`:200-253`) | — |
| comUnitNameTextBox, miaCodeTextBox, miaNameTextBox, districtNameTextBox, areaNameTextBox, marketNameTextBox, custCategoryTextBox, fixedcustomerCheckBox1, rbTypePrice | legacy controls | — | rbTypePrice "New Price" selected | `<div runat=server visible=false>` (never rendered) | kept "for code-behind compatibility" | — |
| productGridView | GridView "Available Products For Return" | ≥1 selected | filled at load by `GetRectinProduct` (`tblProduct.GroupId=1` joined to `tblUnitPrice`) | scroll box 350px | columns ProductCode, ProductName, CostPrice, Sales Price, Return Qty | "Please select at least one product to return!" |
| chkSelectAll / chkSelect | CheckBox | — | unchecked | AutoPostBack (header) | | — |
| transferQtyTextBox | TextBox "Return Qty" (digits only via FilteredTextBox `0123456789`) | Yes if row checked, >0 | empty | — | | "Please enter a valid Return Quantity for selected products!" |
| addButton (LinkButton5) | "Add Selected Products To Return List" | — | — | — | `addButton_Click` (`:712-844`) | — |
| gridLineItemGridView | GridView "Return Line Items List" | rows | empty | — | template controls below | — |
| codeTextBox / nameTextBox | TextBox | — | product code / "name:pack" | ReadOnly | | — |
| currentStockTextBox | TextBox "CStock" | — | `SUM(StockQty)` of `tblDCStore` Trans_Type IN ('Receive','Issue') for DC | ReadOnly | `ProductInfoDAL` | — |
| unitPriceTextBox | TextBox "Unit Price" (AutoPostBack) | — | active `tblUnitPrice.UnitPrice` | **editable** | digits + '.' | — |
| cogsTextBox | TextBox "Cost Price" | — | active `tblUnitPrice.CostPrice` | **editable** | drives COGS reversal (BR-M08-009) | — |
| upVatTextBox | TextBox "Unit VAT" | — | `VATAmountPerUnit` | ReadOnly | | — |
| qtyTextBox | TextBox "Return Qty" (AutoPostBack) | — | from selection | editable (decimals allowed by filter) | `RecalculateRow` | — |
| tpTextBox / tpVatTextBox / npTextBox / tQtyTextBox | TextBox "Total TP / Total VAT / Net Price / Total Qty" | — | computed | ReadOnly | | — |
| dpTextBox, dpAmtTextBox, sdTextBox, bQtyTextBox | discount %, discount amt, special discount, bonus qty | — | | `Visible="False"` columns | still populated/read in code (`:2429-2447`) | — |
| mfgDateTextBox | TextBox "Mfg Date" (datepicker) | No | empty | editable | stored as `MfgDate` (parsed with TryParse) and also put in `PackSize` (`:481`) | — |
| expDateTextBox | TextBox "Expiry Date" (datepicker) | **Yes** | empty | editable | | "Please Input Expry Date!!!" |
| batchNoTextBox | TextBox "Batch No" | **Yes** | empty | editable | | "Please Input Batch No!!!" |
| tpTptalTextBox, vatTotalTextBox, grandTotalTextBox ("Return Total"), pdTextBox, disTotalTextBox | TextBox summary | — | 0.00 | ReadOnly (pd/dis hidden) | `TotalValueCalculation` | — |
| txtDeductionCharge | TextBox "Deduction Charge" (AutoPostBack, digits + '.') | No | 0.00 | editable | JS `calcNetReturn()` + server `txtDeductionCharge_TextChanged` | — |
| txtNetReturnAmount | TextBox "Net Return Amount" | — | 0.00 | ReadOnly | | — |
| invTextBox + Button1 | Return Invoice No + Print | needed for print | empty | editable | `printButton_Click` | "Please Enter or Generate a Return Invoice No to Print!" |
| LinkButton3 (submit) / LinkButton4 (reset) | "Submit Return Information" / "Reset Information" | — | — | — | `submitButton_Click` / `cancelButton_Click`→`Clear()` | — |

## 8. Field Mapping
| UI control | DTO / variable | Handler | BLL | DAL | SP param / SQL param | Table.column |
|---|---|---|---|---|---|---|
| orderDateTextBox | `SalesReturnDaoMas.InvoiceDate`, `OrderDate` | `SaveInvoiceNew` :605-607 | — | `BonusCampaignNewDAL.SaveReturnSalesMaster` | `@InvoiceDate`, `@OrderDate` | tblReturnInvoice.ReturnInvoiceDate, OrderDate |
| hdCustomerMasterId | `CustomerMasterId` | :608 | — | same | `@CustomerMasterId` | tblReturnInvoice.CustomerMasterId |
| DC dropdown/session | `ComUnitId` (dropdown→Session DICCompanyUnitId→hdComUnitId, :579-591) | :609 | — | same | `@ComUnitId` | tblReturnInvoice.ComUnitId |
| (const 0 / 1) | `MiaId=0`, `PaymentTypeId=1` | :610-611 | — | same | `@MiaId`,`@PaymentTypeId` | MiaId, PaymentTypeId |
| tpTptalTextBox | `TpTotal` | :614,628 | — | same | `@TpTotal` | TpTotal |
| disTotalTextBox | `TpDiscount` | :615,629 | — | same | `@TpDiscount` | TpDiscount |
| vatTotalTextBox | `TpVat` | :616,630 | — | same | `@TpVat` | TpVat |
| grandTotalTextBox | `TpGrandTotal` | :617,631 | — | same | `@TpGrandTotal` | TpGrandTotal (Live version of SP only) |
| txtDeductionCharge | `DeductionCharge` | :619,632 | — | same | `@DeductionCharge` | DeductionCharge (Live version only) |
| txtNetReturnAmount | `NetReturnAmount` (recomputed if ≤0 and grand>0) | :620-626,633 | — | same | `@NetReturnAmount` | NetReturnAmount (Live version only) |
| Session UserId / LoginName | `UserId`, `createBy`; `Createdate=Now` | :634,658-659 | — | same | `@createBy`,`@Createdate` | CreateBy, CreateDate (UserId not sent to SP) |
| orderNoTextBox | `Remarks` | :639 | — | same | `@Remarks` | Remarks |
| const | `OrderNo="No order"`, `OrderId=0`, `InvoiceId=0`, `IsSalesReturnWithoutOrder="Return From Market"` (SP hard-codes 'No order') | :636-640 | — | same | `@OrderNo`,`@OrderId`,`@invoiceid` | OrderNo, OrderId, InvoiceId, IsSalesReturnWithoutOrder |
| const | `FixedCustomer=true`, `Type=true`, `ProductOffer=false`, MIA*/Market*/Area*/Region* = "" | :642-663 | — | same | `@FixedCustomer`,`@Type`… | FixedCustomer, Types, … |
| hfMIOId / hfTerriId / hfMioEmpId / hfSapCode | `MIOId_new`, `Terri_Id_new`, `MioEmpId_new`, `Mio_SapCode_New` | :666-669 | — | same | `@MIOId_new`… | MIOId_new, Terri_Id_new, MioEmpId_new, Mio_SapCode_New; SP derives RegionId_Rtn, AreaId_Rtn from territory |
| grandTotalTextBox | `ReturnAmountDAO.Amount`, CustomerId, InvoiceId=new return id | `SaveReturnAmount` :2464-2475 | `ReturnInvoiceBLL.SaveDataForReturnAmount` :139 | `ReturnInvoiceDAL.SaveDataForReturnAmount` :295-304 | inline INSERT | tblReturnAmount(CustomerId, ReturnInvoiceId, Amount) |
| line: code/name | `InvoiceDetail.ProductCode`, `ProductName` (before ':') | `SaveInvoiceDetailNew` :430-438 | `ReturnInvoiceBLL.SaveReturnInvoice(detail)` :144-156 | `SaveDataForInvoiceDetails` :265-279, params :1374-1403 | inline INSERT | tblReturnInvoiceDetail.ProductCode, ProductName |
| line: unitPrice / cogs / unitVat | `UnitPrice`, `CostPrice`, `UnitVatAmount` | :461-463 | same | same | `@UnitPrice`,`@CostPrice`,`@UnitVatAmount` | UnitPrice, CostPrice, UnitVatAmount |
| line: qty / bonus / total qty | `Quantity`, `BonusQuantity`, `TotalQuantity = tQty>0?tQty:qty` | :464-469 | same | same | | Quantity, BonusQuantity, TotalQuantity |
| line: tp / tpVat / dp / dpAmt / np | `TotalPrice`, `TotalPriceVatAmount`, `DiscountPercentage`, `DiscountAmount`, `NetAmount = np>0?np:tp+tpVat` | :465-472 | same | same | | TotalPrice … NetAmount |
| line: mfgDate | `PackSize` = raw mfg text (!) | :481 | same | same | `@PackSize` | tblReturnInvoiceDetail.PackSize |
| line: batch/exp | `BatchNo`, `ExpDate` (fallback today), `ReceiveDate=today` | :440-446,482 | same | same | | BatchNo, ExpDate, ReceiveDate |
| (const) | `DCStoreId=0`, `SpecialAmount=0`, `OrderDetailsId=hidden`, `Campaign=null`, `InvoiceDetailId(@InvoiceDetailId)=ReturnDetailsId=0` | :470-480 | same | same | | DCStoreId, SpecialAmount, OrderDetailsId, InvoiceDetailId |
| line → stock | product, name, batch, exp, mfg, qty, comUnitId, return invoice no, cogs, detailId | :518-523 | `InsertDCStoreStockSalesReturn` :158-161 | :306-366 | INSERT…VALUES | tblDCStore (see §17) |

## 9. Dropdown Mapping
| Dropdown | Source | Value / text | Notes |
|---|---|---|---|
| salesCenterDropDownList | `OrderInfoDAL.LoadSC(ddl, userId)` — `tblCompanyUnit` filtered by `tblUserCompanyUnit(UserId)`; fallback all `tblCompanyUnit` (`OrderInfoDAL.cs:972-993`) | ComUnitId / ComUnitName | Selection is written back to Session `DICCompanyUnitId` (`:70,80,224`) |
| rbTypePrice | static (Old Price/New Price) | — | Hidden; New Price selected ⇒ `ProductInfoDAL` (active `tblUnitPrice`); "Old Price" branch (`tblUnitPrice_Old`) unreachable from UI |

## 10. Button Action Mapping
| Button | Handler | Effect |
|---|---|---|
| viewLinkButton | `ListImageButton_Click` | redirect `SalesReturnList.aspx` |
| LinkButton5 "Add Selected Products…" | `addButton_Click` | validates selection, rebuilds line grid via `AddFunc`+`GetProduct`+`GetQty`, applies trade-policy discount, totals |
| chkSelectAll | `chkSelectAll_CheckedChanged` | tick/untick all |
| qty/unitPrice/deduction textboxes | `qtyTextBox_TextChanged`, `unitPriceTextBox_TextChanged`, `txtDeductionCharge_TextChanged` | server recalculation (postback) |
| LinkButton3 "Submit Return Information" | `submitButton_Click` `:375-400` | validation → save master → return amount → details+stock → auto-post vouchers → clear (keep invoice no) |
| LinkButton4 "Reset" | `cancelButton_Click`→`Clear()` `:2697-2741` | reset form, reload DCs/products |
| Button1 "Print" | `printButton_Click` `:698-710` | opens `ReturnInvoiceReportViewer.aspx?InvNo=` in new window |

## 11. Frontend Flow
1. `Page_Load` (first load): load user DCs + default DC (`LoadDistributionCenter`), load returnable product catalogue (`LoadAllProduct`→`GetRectinProduct`), date=today, remarks="Return From Market" (`:23-32`).
2. Type customer code/name (AutoComplete → `GetCustomer` asmx) → postback `GetCustInfo` fills hidden IDs, address, MIO label; if no DC selected the customer's own `ComUnitId` is selected (`:211-218`).
3. Tick products, key integer Return Qty (`FilteredTextBoxExtender` digits only), click *Add Selected Products* → line grid built; discount % chosen (§12), row recomputed.
4. Per line: user must enter batch and expiry (mfg optional), may change unit price/cost price/qty (each edit is an AutoPostBack).
5. Deduction charge edit → JS `calcNetReturn()` (`aspx:614-625`, net = grand − deduction, floor 0) and server recalculation.
6. Submit → server flow §12. On success an alert shows the generated Return Invoice No; the invoice no stays in the print box.
JS on page: datepicker/select2 init in `pageLoad`, `calcNetReturn`.

## 12. Backend Flow
`submitButton_Click` (`:375-400`), inside `try { } catch (Exception) { alert("Some Networks Error blocked Your Transaction Please Contract Administrator!!") }` (exception swallowed, no logging, **no DB transaction**):
1. `Validation()` (§18).
2. `SaveInvoiceNew()` `:571-697`: resolve `comUnitId` (dropdown → Session `DICCompanyUnitId` → `hdComUnitId`); parse totals; if `netVal<=0 && grand>0` then `netVal = max(grand − deduction, 0)`; build `SalesReturnDaoMas` (constants in §8); `BonusCampaignNewDAL.SaveReturnSalesMaster` → `accessManager.SaveDataReturnPrimaryKey("spInsertReturnInvoice_new")` → returns `SCOPE_IDENTITY()` = ReturnInvoiceId; re-read `ReturnInvoiceNo` (`getInvo`) into `invTextBox`.
3. `SaveReturnAmount(invId)` inserts `tblReturnAmount(CustomerId, ReturnInvoiceId, Amount=grandTotal)`.
4. `SaveInvoiceDetailNew(invId)` `:402-528`: for every line with a code: build detail; `ReturnInvoiceBLL.SaveReturnInvoice` (id = `PrimaryKeyMax("ReturnInvoiceDetailId")` then INSERT); if `retQty>0 && comUnitId>0` → `InsertDCStoreStockSalesReturn(...)` receives the stock row (BR-M08-011).
5. `ReturnInvoiceBLL.PostSalesReturn(invId)` → `EXEC dbo.sp_SalesReturnPosting @ReturnInvoiceId` via `SInventorySql.Execute` (BR-M08-012…016). A failure here throws after stock and master were already committed (no rollback).
6. `Clear(keepInvoiceNo:true)`.
`addButton_Click` `:712-844`: validates; per checked row `AddFunc`, `GetProduct` (`ProductInfoDAL(comUnit, code)`), `GetQty`→`RecalculateRow` (`:2402-2448`); discount %: if `GetFixedCustomerffff(customerCode).FixedCustomer` → 0; else `GetParcentFromOrderDetailsMasterID(orderIdHiddenField)` (empty on this page) → `GetTradeTerm(totalTP, CustTypeId, PaymentType, SubmissionDate)` → `DiscountPerc`, any exception → 0; discount amount per line = TP × % / 100; net per line = TP − disc + VAT.

## 13. API Mapping
No REST API. ASMX autocomplete `SInventoryWebService.asmx/GetCustomer` (not read) is used by the two customer boxes. Print opens report viewer page (§25).

## 14. Service Mapping
| Layer method | Purpose |
|---|---|
| `ReturnInvoiceBLL.CustomerMaster/getInvo/ProductInfo/ProductInfo_Old/ProductDiscount/GetRectinProduct` | lookups |
| `ReturnInvoiceBLL.SaveReturnInvoice(InvoiceDetail)` (`ReturnInvoiceBLL.cs:144-156`) | detail PK + insert |
| `ReturnInvoiceBLL.SaveDataForReturnAmount`, `InsertDCStoreStockSalesReturn`, `PostSalesReturn` (`:139,158,163`) | return amount / stock / posting |
| `OrderInfoBLL.LoadSC`, `GetParcentFromOrderDetailsMasterID`, `GetTradeTerm` | DC list, discount policy |

## 15. Repository Mapping
| DAL method | SQL |
|---|---|
| `BonusCampaignNewDAL.SaveReturnSalesMaster` :441-539 | `spInsertReturnInvoice_new` (40+ named params) |
| `ReturnInvoiceDAL.SaveDataForInvoiceDetails` :265-279 | inline `INSERT INTO dbo.tblReturnInvoiceDetail` (ReturnInvoiceDetailId, ProductCode, …, InvoiceDetailId) |
| `ReturnInvoiceDAL.SaveDataForReturnAmount` :295-304 | inline `INSERT INTO dbo.tblReturnAmount` |
| `ReturnInvoiceDAL.InsertDCStoreStockSalesReturn` :306-366 | inline `INSERT INTO dbo.tblDCStore … VALUES` |
| `ReturnInvoiceDAL.PostSalesReturn` :367-370 | `EXEC dbo.sp_SalesReturnPosting` |
| `ProductInfoDAL` :403-427 (+`_Old` :429-453) | `tblProduct` ⟕ `tblUnitPrice(IsActive=1)` ⟕ SUM(`tblDCStore.StockQty` Receive+Issue for DC) — parameterised |
| `LoadCustomerMaster` :515-546 | `tblCustMaster` ⟕ `tblCompanyUnit`, `tblEmpGeneralInfo`, `tblMIOInfo` — parameterised |
| `DiscountofProduct` :705-710 | `TOP 1 tblProductDiscount` where Status Active/'1'/NULL (qty ignored) |
| `GetRectinProduct` :1291-1302 | `tblProduct` ⟕ `tblUnitPrice` WHERE GroupId=1 |

## 16. Stored Procedure Mapping
| SP | Body | Read/Write | What it does |
|---|---|---|---|
| `spInsertReturnInvoice_new` | **in repo, two versions** — `Live_Deployment_Script_All_Updates.sql:857-944` (has `@TpGrandTotal,@DeductionCharge,@NetReturnAmount`) and `SP_SalesReturn_Related.sql:19-163` (lacks them) | W tblReturnInvoice (R tblTerritory, tblArea, tblRegion) | Generates `ReturnInvoiceNo='RIN--'+yymmdd+(MAX(ReturnInvoiceId of same day, default 10000)+1001)`, resolves `RegionId_Rtn/AreaId_Rtn` from `@Terri_Id_new`, inserts with `IsSalesReturnWithoutOrder='No order'`, `SubInvoiceId=0`, `Types=@Type`, returns `SCOPE_IDENTITY()`. Live version: `TpGrandTotal=ISNULL(@TpGrandTotal,@TpTotal)`, `NetReturnAmount=ISNULL(@NetReturnAmount, grand − deduction)` |
| `sp_SalesReturnPosting` | in repo `Live_Deployment_Script_All_Updates.sql:949-1240` | R tblReturnInvoice, tblReturnInvoiceDetail, tblDCStore, tblCustMaster; W `CSTL_ACCDB_Unicorn.dbo.tblSubsidiaryInfo, tblReferenceInfo, tblDebitCreditVoucherMaster/Detail`; W tblReturnInvoice.IsPosting | see BR-M08-012…016 and §24 |
| `sp_GET_*`/lookup for `LoadSC`, `GetTradeTerm` | not SPs (inline) / not read | — | — |

## 17. Database Mapping
- **tblReturnInvoice** (W): columns written by SP: ReturnInvoiceNo, ReturnInvoiceDate, CreateBy, CreateDate, OrderNo, OrderDate, CustomerMasterId, ComUnitId, MiaId, PaymentTypeId, TpTotal, TpDiscount, TpVat, TpGrandTotal, DeductionCharge, NetReturnAmount, OrderId, TotalSpecialAmount, OldTradePolicy, ProductOffer, Remarks, MIACode, MIAName, MarketCode, MarketName, AreaCode, DisCode, FEName, RegionCode, DZSMName, FixedCustomer, InvoiceId, IsSalesReturnWithoutOrder, SubInvoiceId, Types, MIOId_new, Terri_Id_new, MioEmpId_new, Mio_SapCode_New, RegionId_Rtn, AreaId_Rtn; later IsPosting=1. DeductionCharge/NetReturnAmount/IsPosting added by `Live_Deployment_Script_All_Updates.sql:41-76` (DECIMAL(18,2)/BIT default 0).
- **tblReturnInvoiceDetail** (W): ReturnInvoiceDetailId, ProductCode, ProductName, PackSize(=mfg text), BatchNo, ReceiveDate, ExpDate, CostPrice, UnitPrice, UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, TotalPriceVatAmount, DiscountPercentage, DiscountAmount, NetAmount, ReturnInvoiceId, DCStoreId(0), OrderDetailsId, Campaign, SpecialAmount, InvoiceDetailId(0).
- **tblReturnAmount** (W): CustomerId, ReturnInvoiceId, Amount (= grand total, i.e. *before* deduction).
- **tblDCStore** (W, **stock effect** — one new row per line): DCStoreId=MAX+1, StorageLocation='Depot Return Store', ProductCode, ProductName, PackSize (from tblProduct), BatchNo, TotalQuantity=retQty, ExpDate, MfgDate, ReceiveDate=GETDATE(), ChalanNo=return invoice no, ChalanDate=GETDATE(), ComUnitId, StockQty=retQty, DamageQty=0, StockRcvDate, ReqId=0, ReqChildId=0, StockCondition='Non-Transcational Sales Return', StockInTransfarId=0, Trans_Type='Receive', CostPerUnit (= user cost if >0 else latest CostPerUnit>0 of same product+DC, else any DC, else 0), TotalCostAmount=qty×CostPerUnit, ReferenceDCStoreId=0, ReferenceTransactionDate=GETDATE(), ReturnInvoiceDetailId (new column, `Live_Deployment_Script…:78-89`).
- **CSTL_ACCDB_Unicorn** tables (W by `sp_SalesReturnPosting`): see §24. R `CSTL_ERPDB_Unicorn.Table_Integration_AccountsConnectivity`.
- R: tblCustMaster, tblCompanyUnit, tblEmpGeneralInfo, tblMIOInfo, tblProduct, tblUnitPrice, tblProductDiscount, tblUserCompanyUnit.

## 18. Validation Matrix
| # | Check | Where | Message (verbatim) |
|---|---|---|---|
| 1 | DC dropdown index >0 or `hdComUnitId` ≠ '' / '0' | `Validation` :278-286 | Please Select Distribution Center (DC)!! |
| 2 | Remarks text non-empty | :288-293 | Please Input Order Number!! |
| 3 | Return date non-empty | :294-300 | Please Input Order Date!! |
| 4 | Customer code non-empty | :308-313 | Please Select Customer !! |
| 5 | MIO label non-empty | :315-319 | MIO Name can not be empty !! |
| 6 | Each line: expiry non-empty | :338-343 | Please Input Expry Date!!! |
| 7 | Each line: batch non-empty | :346-355 | Please Input Batch No!!! |
| 8 | Add-to-list: each checked product has numeric qty >0 | `addButton_Click` :716-730 | Please enter a valid Return Quantity for selected products! |
| 9 | Add-to-list: ≥1 product checked | :732-736 | Please select at least one product to return! |
| 10 | Product code unknown / no price row | `GetProduct` :2335 | No Any Stock or Product of <code> |
| — | NOT validated | — | line qty >0 in the grid, quantity upper bound, batch/expiry consistency, expiry ≥ today, deduction ≤ total, `hdCustomerMasterId` numeric (a blank customer ID throws `FormatException` in `Convert.ToInt32` :608 → generic "Network Error" alert), duplicate submit (double click creates a second return) |

## 19. Business Rules
| BR | Rule | Source | Implementation | Confidence |
|---|---|---|---|---|
| BR-M08-001 | A market return is created **without any link to an original invoice/order**: `InvoiceId=0`, `OrderId=0`; `OrderNo` forced to "No order"; SP writes `IsSalesReturnWithoutOrder='No order'`, `SubInvoiceId=0`. The list page filters on `IsSalesReturnWithoutOrder='No order'` to show these. | SalesReturn.aspx.cs:636-640; SP_SalesReturn_Related.sql:150-151; ReturnInvoiceDAL.cs:488 | code-behind + SP | CONFIRMED |
| BR-M08-002 | DC is mandatory; DC choice order: dropdown → Session `DICCompanyUnitId` → `hdComUnitId`. Stock and price are looked up for the chosen DC only. | SalesReturn.aspx.cs:278-286,579-591,2287-2291 | code-behind | CONFIRMED |
| BR-M08-003 | The DC list is the user's DCs (`tblUserCompanyUnit`); if the user has ≤1 mapped DC (Items.Count ≤ 1 incl. placeholder) the list falls back to **all** company units. | SalesReturn.aspx.cs:36-46; OrderInfoDAL.cs:972-993 | code-behind/DAL | CONFIRMED |
| BR-M08-004 | Customer, sales-person (MIO) name, return date and remarks are mandatory. | SalesReturn.aspx.cs:288-319 | `Validation` | CONFIRMED |
| BR-M08-005 | Every return line requires an **expiry date and batch no**; manufacturing date is optional; there is no relation to an existing batch. | SalesReturn.aspx.cs:334-356 | `Validation` | CONFIRMED |
| BR-M08-006 | Only products with `tblProduct.GroupId = 1` are offered for return; selected quantity must be a positive whole number at selection time. | ReturnInvoiceDAL.cs:1291-1299; SalesReturn.aspx.cs:716-736, aspx:348 | DAL + code-behind | CONFIRMED |
| BR-M08-007 | Line calculation: TP = unitPrice×qty; VAT = unitVAT×qty; discount % = trade-policy % (non-fixed customers; 0 if `FixedCustomer` or lookup fails) replacing the product-discount % computed in `RecalculateRow`; discount amt = TP×%/100; **Net = TP − discount + VAT**; TotalQty = qty + bonus. Header: TP total, VAT total, discount total, **Return Total = Σ Net**. | SalesReturn.aspx.cs:2423-2446,766-843,2743-2780 | code-behind | CONFIRMED |
| BR-M08-008 | **Net Return Amount = max(Return Total − Deduction Charge, 0)**; if the posted net is ≤0 while grand>0 it is recomputed the same way. Deduction has no upper validation. | SalesReturn.aspx.cs:622-626,2775-2790; aspx:614-625 | code-behind + JS | CONFIRMED |
| BR-M08-009 | Unit price **and cost price** shown in the grid are user-editable; the entered cost price (`cogsTextBox`) becomes `CostPrice` on the detail, `CostPerUnit` in `tblDCStore`, and the base of the COGS reversal voucher. | aspx:419-431; SalesReturn.aspx.cs:450,462,522 | UI + code-behind | CONFIRMED |
| BR-M08-010 | Return invoice number = `'RIN--'+yymmdd+(MAX(ReturnInvoiceId of tblReturnInvoice rows dated same day, default 10000)+1001)` — uses the global identity value, not a per-day counter. | Live_Deployment_Script_All_Updates.sql:904-910 | SP | CONFIRMED |
| BR-M08-011 | **Stock effect:** for each line with returned qty>0 and a DC, a **new `tblDCStore` 'Receive' row** is inserted (location "Depot Return Store", StockCondition "Non-Transcational Sales Return", TotalQuantity=StockQty=qty, ChalanNo=return invoice no, ReturnInvoiceDetailId linked, ReferenceDCStoreId=0). Qty used = `TotalQuantity` if >0 else `Quantity`. Cost per unit = entered cost >0, else last known DC cost for product+DC, else last known any DC, else 0. | SalesReturn.aspx.cs:518-523; ReturnInvoiceDAL.cs:306-366 | code-behind + inline SQL | CONFIRMED |
| BR-M08-012 | **Posting is automatic** after save (`sp_SalesReturnPosting`): idempotent per return (skips if a voucher with `TableID=2018` (AR) / `2023` (COGS, IsInventoryValuation=1) and `CommonPK_ID=ReturnInvoiceId` exists). | SalesReturn.aspx.cs:386; Live_Deployment_Script…:1064-1067,1165-1168 | SP | CONFIRMED |
| BR-M08-013 | AR-reversal voucher (Journal, category 2, status 'Approved', by 'Auto Process', posting date = return date): **Dr Sales Return GL** (PageId 23/FlagId 1020, fallback 4170) = ReturnNetAmount; **Cr Delivery/Deduction Income** (23/1032, fallback 5968) = deduction if >0; **Cr Accounts Receivable** (163/2, fallback 2112) = NetReturnAmount on customer subsidiary ledger. `ReturnNetAmount = TpGrandTotal` (else Σ detail NetAmount). Posted only if ReturnNetAmount>0. | Live_Deployment_Script…:974-1160 | SP | CONFIRMED |
| BR-M08-014 | COGS/inventory voucher: **Dr Finished-Goods Inventory** (23/1018, fb 4156) / **Cr COGS** (23/1019, fb 4162) for `Σ (TotalQuantity or Quantity) × (CostPrice, else latest tblDCStore.CostPerUnit>0 of the product in any DC)`; `IsInventoryValuation=1`; posted only if cost>0. | Live_Deployment_Script…:1005-1019,1165-1235 | SP | CONFIRMED |
| BR-M08-015 | Customer subsidiary (type 1) and reference info (type 1018) rows are **created on the fly** in the accounts DB if missing. Financial year = active year of company 1 (else the year containing posting date, else hard-coded id 2). | Live_Deployment_Script…:1022-1053 | SP | CONFIRMED |
| BR-M08-016 | Voucher code = `'JVP-'+yymmdd+(max of last 5 digits of same-day JVP codes, default 10000)+1`; `IsPosting=1` set on `tblReturnInvoice` at the end. | Live_Deployment_Script…:1069-1073,1238-1239 | SP | CONFIRMED |
| BR-M08-017 | No VAT-liability reversal is posted: VAT is inside the Sales Return debit (gross) and the receivable credit; deduction is booked as income without VAT. | Live_Deployment_Script…:1098-1159 | SP | CONFIRMED |
| BR-M08-018 | `tblReturnAmount.Amount` = Return Total (gross, before deduction). | SalesReturn.aspx.cs:2464-2475 | code-behind | CONFIRMED |

## 20. Permission Matrix
| Check | Enforced in code? |
|---|---|
| Login / Forms auth | via master page / Web.config only; page itself does not test `Session["UserId"]` until save (`Session["UserId"].ToString()` at :634 throws `NullReferenceException` if session lost → generic alert) |
| Role/menu | menu-based only (`sp_GET_MainMenuRole2`); **no role check in this page** |
| DC scope | DC dropdown limited to `tblUserCompanyUnit` mapping (soft; falls back to all DCs, BR-M08-003); customer/DC mismatch not checked |
| Audit user | `Session["LoginName"]` → `CreateBy`; `Session["UserId"]` used only as the DC filter and int cast |

## 21. Status Lifecycle
No status column set by the page. Implicit states: (1) saved (`tblReturnInvoice` row exists, `IsPosting` default 0) → (2) posted (`IsPosting=1` after `sp_SalesReturnPosting`). If posting throws, the return remains `IsPosting=0/NULL` with stock already received (no automatic retry). `Types`(=1 from `@Type`), `IsSalesReturnWithoutOrder='No order'` identify the flow.

## 22. Error Handling
Single `try/catch(Exception)` around the whole submit → alert "Some Networks Error blocked Your Transaction Please Contract Administrator!!"; exception details discarded (commented `throw`). `SaveReturnSalesMaster` catches, closes connection and rethrows. `Validation()` messages via `alert()`. `ShowMessageBox`/`showMessageBox` do `message.Replace("'", "\'")` (no-op in C#, so an apostrophe in a message breaks the JS). No logging, no transaction, no rollback.

## 23. Audit
`CreateBy` (LoginName), `CreateDate` (DateTime.Now) on tblReturnInvoice; vouchers stamped EntryBy=1 ('admin' for delivery flow), ApprovedBy='Auto Process'. No update/delete audit for returns. `tblReturnInvoice.UserId` is not populated by the SP.

## 24. Integration
Cross-database: `CSTL_ACCDB_Unicorn` (vouchers, financial year, subsidiary, reference) and `CSTL_ERPDB_Unicorn.Table_Integration_AccountsConnectivity` (GL mapping PageId 23/163, FlagIds 1018,1019,1020,1032,2). SAP: none. Stock ledger `tblDCStore` feeds `sp_Report_DCStore_StockValuationLedger`/valuation reports (`SP_Update_StockValuation_TransactionDate_Logic.sql`, not analysed here).

## 25. Reports
`ReturnInvoiceReportViewer.aspx` → `rptReturnInvoiceForCustomer.rpt` (see RPT-M08-03). No other report from this page.

## 26. Dependencies
Pages: SalesReturnList.aspx, ReturnInvoiceReportViewer.aspx. Tables/SPs in §16-17. Libraries: AjaxControlToolkit (AutoComplete, FilteredTextBox), jQuery pickadate/select2 (master). Session: UserId, LoginName, DICCompanyUnitId, ComUnitId, SalesCenter.

## 27. Test Cases
| # | Type | Steps | Expected | BR |
|---|---|---|---|---|
| T1 | Positive | DC=Dhaka, customer with MIO, tick 1 product qty 10, batch B1, expiry future, submit | RIN number shown; tblReturnInvoice + detail + tblReturnAmount rows; tblDCStore Receive row qty 10 Depot Return Store; 2 vouchers (AR & COGS) with IsPosting=1 | 001,011,012-016 |
| T2 | Positive | Deduction 100 on grand 1,000 | Net 900; voucher Dr 1,000 / Cr income 100 / Cr AR 900; tblReturnInvoice.DeductionCharge=100 | 008,013 |
| T3 | Negative | Submit without batch on a line | alert "Please Input Batch No!!!", nothing saved | 005 |
| T4 | Negative | Tick product, leave qty blank, Add | "Please enter a valid Return Quantity…" | 006 |
| T5 | Edge | Deduction > Return Total | Net shows 0.00; posting Dr total / Cr income deduction / Cr AR 0 → **unbalanced** voucher | 008,013 |
| T6 | Edge | Cost price edited to 0 | CostPerUnit falls back to last DC cost; COGS voucher uses fallback (or none if 0) | 009,011,014 |
| T7 | Negative | Session expired then submit | generic Network Error alert, possible partial data | — |
| T8 | Edge | Double-click submit | two returns and two stock rows (no idempotency at page level) | 011 |
| T9 | Regression | Return for DC without mapped user DC | list shows all DCs | 003 |
| T10 | Integration | Delete SP_SalesReturn_Related version conflict: run against DB deployed with older `spInsertReturnInvoice_new` | `TpGrandTotal/DeductionCharge/NetReturnAmount` params rejected ("not a parameter") → save fails | — (GAP-M08-003) |

## 28. Reverse Engineering References
`SalesReturn.aspx(.cs)`; `ReturnInvoiceBLL.cs:124-176`; `ReturnInvoiceDAL.cs:232-370,403-453,515-546,705-710,1291-1302,1374-1403`; `BonusCampaignNewDAL.cs:441-539`; `SP_SalesReturn_Related.sql`; `Live_Deployment_Script_All_Updates.sql:41-89,857-1240`.

## 29. Spec vs Actual
| Item | Specification | Actual |
|---|---|---|
| Whole page | None existed | Behaviour as documented above (no spec to compare) |
| SP deployment | None existed | Two divergent scripts define `spInsertReturnInvoice_new`; which is live UNKNOWN |

## 30. Known Gaps
- No DB transaction across master/detail/stock/posting; failure leaves partial data (master without details, stock without voucher).
- Posting relies on user-editable cost/unit price; no cap against sales price or any original invoice ⇒ arbitrary credit possible (BR-M08-009).
- Deduction unbounded → voucher unbalanced when deduction > total (BR-M08-008/013).
- Same-table-ID collision with delivery-return vouchers (TableID 2018/2023, CommonPK_ID) — see GAP-M08-005.
- `mfgDateTextBox` text stored in `PackSize` (data-quality defect).
- ~1,200 lines dead code (FOC bonus checks `Moticare`, `Nervaid75mgCapsuleFOC`, `Qty*`, `addImageButton_Click2-6`, `SaveInvoice` stub, `SaveInvoiceDetail`) with hard-coded product codes.
- `Convert.ToInt32(Session["UserId"])`, `Convert.ToInt32(hdCustomerMasterId.Value)` unguarded.
- Hidden `Visible=False` template columns (`dp`, `dpAmt`, `sd`, `bQty`) are indexed by position (`Cells[10]`, `Cells[12]`) in legacy helpers — fragile.
- JS alert message escaping no-op.
- `Old Price` branch unreachable.

## 31. Change Impact
`ReturnInvoiceDAL.InsertDCStoreStockSalesReturn/PostSalesReturn/SaveDataForInvoiceDetails` are also used by any caller of `ReturnInvoiceBLL` (grep: only `SalesReturn.aspx.cs`). `sp_SalesReturnPosting` posts `TableID 2018/2023` vouchers shared with `sp_DeliverySalesReturnPosting` (PAGE-287). `tblReturnInvoice` is read by `SalesReturnList.aspx`, return reports (RPT-M08-01..03) and `spInsertReturnInvoice_new` callers. `tblDCStore` Receive rows created here feed stock reports and are subject to `sp_Adjust_DCStore_Stock_Only` (safe here because TotalQuantity=StockQty). Changing `Table_Integration_AccountsConnectivity` rows (PageId 23/163) changes GL accounts for both return flows.
