# WF-M08-01 — Market sales return: entry, stock receipt and accounting posting

**Confidence:** CONFIRMED for the write path (code + SP bodies in repo); INFERRED for business intent.

## Trigger & actors
- Depot/DC user (menu access to `SalesReturn.aspx`) records goods returned from a customer/market outlet with no invoice reference ("Return From Market").
- Downstream: accounts (vouchers), stock reports, print viewer.

## Preconditions
- Customer exists in `tblCustMaster` (found by code or name); user has a DC (or all DCs visible, BR-M08-003).
- Products exist with `tblProduct.GroupId=1` and an active `tblUnitPrice` (cost, sales price, VAT).
- GL mapping rows exist (or fallbacks apply) in `CSTL_ERPDB_Unicorn.Table_Integration_AccountsConnectivity` PageId 23/163.

## Steps
| # | Step | Page → handler → BLL → DAL → SQL |
|---|---|---|
| 1 | Open page; DCs, product catalogue, defaults load | PAGE-294 `Page_Load` → `LoadDistributionCenter`, `LoadAllProduct` → `OrderInfoDAL.LoadSC`, `ReturnInvoiceDAL.GetRectinProduct` |
| 2 | Choose DC; enter customer → hidden IDs, MIO, territory filled | `custCodeTextBox_TextChanged` → `GetCustInfo` → `ReturnInvoiceDAL.LoadCustomerMaster` |
| 3 | Tick products + integer qty → *Add Selected Products* | `addButton_Click` → `AddFunc/GetProduct/GetQty/RecalculateRow` → `ProductInfoDAL`, `DiscountofProduct`, trade-term lookup |
| 4 | Enter batch, expiry (mandatory), mfg (optional); adjust price/cost/qty; deduction | grid events; `txtDeductionCharge_TextChanged` |
| 5 | Submit | `submitButton_Click` → `Validation()` |
| 6 | Create master | `SaveInvoiceNew` → `BonusCampaignNewDAL.SaveReturnSalesMaster` → `spInsertReturnInvoice_new` → `tblReturnInvoice` (returns id, RIN-number) |
| 7 | Store return amount | `SaveReturnAmount` → `tblReturnAmount(CustomerId, ReturnInvoiceId, Amount=Return Total)` |
| 8 | Per line: detail + stock | `SaveInvoiceDetailNew` → `SaveReturnInvoice` → `tblReturnInvoiceDetail`; `InsertDCStoreStockSalesReturn` → new `tblDCStore` **Receive** row (Depot Return Store) |
| 9 | Post | `PostSalesReturn` → `sp_SalesReturnPosting` → 2 vouchers in `CSTL_ACCDB_Unicorn`; `tblReturnInvoice.IsPosting=1` |
| 10 | Print (optional) | `printButton_Click` → `ReturnInvoiceReportViewer.aspx?InvNo=` (RPT-M08-03) |

## Data created / changed
| Table | Change |
|---|---|
| tblReturnInvoice | 1 row (Types, IsSalesReturnWithoutOrder='No order', Deduction/Net columns); later IsPosting=1 |
| tblReturnInvoiceDetail | 1 row per line (InvoiceDetailId=0, DCStoreId=0) |
| tblReturnAmount | 1 row |
| **tblDCStore** | +1 Receive row per line: StockCondition 'Non-Transcational Sales Return', TotalQuantity=StockQty=qty, CostPerUnit/TotalCostAmount, ReturnInvoiceDetailId |
| CSTL_ACCDB_Unicorn.tblSubsidiaryInfo / tblReferenceInfo | created if missing (types 1 / 1018) |
| CSTL_ACCDB_Unicorn.tblDebitCreditVoucherMaster/Detail | Voucher 1 (AR reversal, TableID 2018) and Voucher 2 (COGS/inventory, TableID 2023, IsInventoryValuation=1) |

## Stock effects (which rows change)
Only **inserts** into `tblDCStore` at the selected DC (`ComUnitId`); no existing row is updated. Available stock for the product at that DC rises by the returned qty immediately (stock queries sum `StockQty` of Receive+Issue rows).

## Valuation impact
Inventory re-enters at `CostPerUnit` = user-entered cost, else last known cost (BR-M08-011); the COGS voucher uses `Σ qty × (CostPrice else latest CostPerUnit>0)` (BR-M08-014). Receipts carry `TotalCostAmount = qty × CostPerUnit`, so the valuation ledger and the GL entry agree only if the fallbacks resolve the same cost.

## Accounting posting (voucher) implications
Voucher 1: Dr Sales Return GL (23/1020→4170) = gross return (TpGrandTotal); Cr Delivery Charge Income (23/1032→5968) = deduction; Cr AR (163/2→2112) = net, on customer subsidiary. Voucher 2: Dr FG Inventory (23/1018→4156) / Cr COGS (23/1019→4162). Both 'Journal', 'Approved', 'Auto Process', date = return date, codes `JVP-yymmdd#####`. VAT is not separated. If deduction > gross, Voucher 1 is unbalanced (BR-M08-008/013; GAP-M08-007).

## Branches / exceptions
- Validation failure → alert, nothing saved.
- Any exception after step 6 → generic alert, **partial data remains** (no transaction): master without details; stock without vouchers; return with `IsPosting` NULL.
- Posting is idempotent only against voucher `TableID/CommonPK_ID`; collides with delivery-return vouchers of the same numeric id (GAP-M08-005).
- Deleting the return later from `SalesReturnList.aspx` (`ReturnInvoiceDAL.DeleteData`) removes the return rows and freeze rows but **not** the `tblDCStore` receipt nor the vouchers (GAP-M08-008).

## Postconditions
Return invoice printable; stock ledger increased; two approved vouchers exist; `tblReturnInvoice.IsPosting=1`.

## Related BR ids
BR-M08-001…018.

## Related pages
PAGE-294 (entry), PAGE-295 SalesReturnList (list/delete/print — Tier B), PAGE-291 ReturnInvoiceView (Tier B), PAGE-282/283/297 reports.

## Tests
See PAGE-294 §27 (T1-T10); add: (a) delete return then verify stock and vouchers still present; (b) run against DB where only the older `spInsertReturnInvoice_new` exists.
