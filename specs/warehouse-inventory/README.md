# M10 - Warehouse, DC Stock and Inventory

Reviewed 2026-09-19 (reverse-engineered from source; **documentation only**). Evidence tags: CONFIRMED / INFERRED / UNKNOWN. SP bodies are absent from the repo except the scripts listed in section 6.

## 1. Purpose and business overview
M10 covers stock held in the **Central Warehouse (CW)** and in **Distribution Centers (DC/depots)**: receiving goods (manual stock-in, production output via material conversion), maker/checker approval, issue/write-off, adjustments, quarantine (freeze by stock condition) and release, sample conversions, batch correction, opening balances, and the stock, valuation and monitoring reports. Pages live in `Solution.Web/SInventory_UI`, `SInventory_RPTVIEW` (Crystal viewers) and one in `Reports_UI`; logic in `Library.BLL/DAL/SInventory_*`. Most DAL code builds SQL by string concatenation and runs it through the generic SP `ExecuteAllSqlQueryByStoreProcedure`.

## 2. Page catalogue (56 pages)
| Page ID | Page | Kind | Tier | Spec |
|---|---|---|---|---|
| PAGE-342 | SampleStockAllocationRpt (`Reports_UI`) | Report / Dashboard | B | [PAGE-342-samplestockallocationrpt.md](pages/PAGE-342-samplestockallocationrpt.md) |
| PAGE-343 | AllProductCountryStockReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-343-allproductcountrystockreportviewer.md](pages/PAGE-343-allproductcountrystockreportviewer.md) |
| PAGE-344 | CountryStockReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-344-countrystockreportviewer.md](pages/PAGE-344-countrystockreportviewer.md) |
| PAGE-345 | CurrentStockRpt (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-345-currentstockrpt.md](pages/PAGE-345-currentstockrpt.md) |
| PAGE-346 | CWMonthlyInventoryReport (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-346-cwmonthlyinventoryreport.md](pages/PAGE-346-cwmonthlyinventoryreport.md) |
| PAGE-347 | DcOpeningStockReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-347-dcopeningstockreportviewer.md](pages/PAGE-347-dcopeningstockreportviewer.md) |
| PAGE-348 | DcStockOutReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-348-dcstockoutreportviewer.md](pages/PAGE-348-dcstockoutreportviewer.md) |
| PAGE-349 | DCStockReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-349-dcstockreportviewer.md](pages/PAGE-349-dcstockreportviewer.md) |
| PAGE-350 | DCwiseCounrtyStockReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-350-dcwisecounrtystockreportviewer.md](pages/PAGE-350-dcwisecounrtystockreportviewer.md) |
| PAGE-351 | DistributionCenterStockMonitoringReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-351-distributioncenterstockmonitoringreportviewer.md](pages/PAGE-351-distributioncenterstockmonitoringreportviewer.md) |
| PAGE-352 | WhOpeningStockReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-352-whopeningstockreportviewer.md](pages/PAGE-352-whopeningstockreportviewer.md) |
| PAGE-353 | WHStockAdjustmentListReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-353-whstockadjustmentlistreportviewer.md](pages/PAGE-353-whstockadjustmentlistreportviewer.md) |
| PAGE-354 | WHStockInReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-354-whstockinreportviewer.md](pages/PAGE-354-whstockinreportviewer.md) |
| PAGE-355 | WhStockMonitoringReportViewer (`SInventory_RPTVIEW`) | Report viewer | B | [PAGE-355-whstockmonitoringreportviewer.md](pages/PAGE-355-whstockmonitoringreportviewer.md) |
| PAGE-356 | AdjustmentEntry (`SInventory_UI`) | Entry / Transaction | B | [PAGE-356-adjustmententry.md](pages/PAGE-356-adjustmententry.md) |
| PAGE-357 | AdjustmentTypeView (`SInventory_UI`) | List / View | B | [PAGE-357-adjustmenttypeview.md](pages/PAGE-357-adjustmenttypeview.md) |
| PAGE-358 | AreaWiseMonthlyInventoryReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-358-areawisemonthlyinventoryreport.md](pages/PAGE-358-areawisemonthlyinventoryreport.md) |
| PAGE-359 | BatchLifecycleReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-359-batchlifecyclereport.md](pages/PAGE-359-batchlifecyclereport.md) |
| PAGE-360 | CountryStockEntry (`SInventory_UI`) | Entry / Transaction | B | [PAGE-360-countrystockentry.md](pages/PAGE-360-countrystockentry.md) |
| PAGE-361 | CountryStockReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-361-countrystockreport.md](pages/PAGE-361-countrystockreport.md) |
| PAGE-362 | DcOpeningStockReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-362-dcopeningstockreport.md](pages/PAGE-362-dcopeningstockreport.md) |
| PAGE-363 | DcStockOutApproval (`SInventory_UI`) | Approval | A | [PAGE-363-dcstockoutapproval.md](pages/PAGE-363-dcstockoutapproval.md) |
| PAGE-364 | DCStockReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-364-dcstockreport.md](pages/PAGE-364-dcstockreport.md) |
| PAGE-365 | DCStockValuationReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-365-dcstockvaluationreport.md](pages/PAGE-365-dcstockvaluationreport.md) |
| PAGE-366 | DCWiseCountryStockReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-366-dcwisecountrystockreport.md](pages/PAGE-366-dcwisecountrystockreport.md) |
| PAGE-367 | DepotStockAdjustmentsVoucher (`SInventory_UI`) | Entry / Transaction | A | [PAGE-367-depotstockadjustmentsvoucher.md](pages/PAGE-367-depotstockadjustmentsvoucher.md) |
| PAGE-368 | DepotStockAdjustmentsVoucherView (`SInventory_UI`) | List / View | B | [PAGE-368-depotstockadjustmentsvoucherview.md](pages/PAGE-368-depotstockadjustmentsvoucherview.md) |
| PAGE-369 | DistributionCenterStockMonitoring (`SInventory_UI`) | Report / Dashboard | B | [PAGE-369-distributioncenterstockmonitoring.md](pages/PAGE-369-distributioncenterstockmonitoring.md) |
| PAGE-370 | MaterialConversionReceive (`SInventory_UI`) | Entry / Transaction | A | [PAGE-370-materialconversionreceive.md](pages/PAGE-370-materialconversionreceive.md) |
| PAGE-371 | MaterialConversionReceiveView (`SInventory_UI`) | List / View | B | [PAGE-371-materialconversionreceiveview.md](pages/PAGE-371-materialconversionreceiveview.md) |
| PAGE-372 | MonthlyInventoryReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-372-monthlyinventoryreport.md](pages/PAGE-372-monthlyinventoryreport.md) |
| PAGE-373 | MonthlyWarehouseReportCW (`SInventory_UI`) | Report / Dashboard | B | [PAGE-373-monthlywarehousereportcw.md](pages/PAGE-373-monthlywarehousereportcw.md) |
| PAGE-374 | NationalStockValuationReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-374-nationalstockvaluationreport.md](pages/PAGE-374-nationalstockvaluationreport.md) |
| PAGE-375 | OtherStockAction (`SInventory_UI`) | Page | A | [PAGE-375-otherstockaction.md](pages/PAGE-375-otherstockaction.md) |
| PAGE-376 | SampleStockforWarehouse (`SInventory_UI`) | Page | B | [PAGE-376-samplestockforwarehouse.md](pages/PAGE-376-samplestockforwarehouse.md) |
| PAGE-377 | SampleStockForWareHouseView (`SInventory_UI`) | List / View | B | [PAGE-377-samplestockforwarehouseview.md](pages/PAGE-377-samplestockforwarehouseview.md) |
| PAGE-378 | SampleTypeConventionView (`SInventory_UI`) | List / View | B | [PAGE-378-sampletypeconventionview.md](pages/PAGE-378-sampletypeconventionview.md) |
| PAGE-379 | SampleTypeConvertion (`SInventory_UI`) | Page | A | [PAGE-379-sampletypeconvertion.md](pages/PAGE-379-sampletypeconvertion.md) |
| PAGE-380 | StockBatchUpdate (`SInventory_UI`) | Page | B | [PAGE-380-stockbatchupdate.md](pages/PAGE-380-stockbatchupdate.md) |
| PAGE-381 | StockConditionFreeze (`SInventory_UI`) | Page | A | [PAGE-381-stockconditionfreeze.md](pages/PAGE-381-stockconditionfreeze.md) |
| PAGE-382 | StockConditionPermission (`SInventory_UI`) | Page | B | [PAGE-382-stockconditionpermission.md](pages/PAGE-382-stockconditionpermission.md) |
| PAGE-383 | StockReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-383-stockreport.md](pages/PAGE-383-stockreport.md) |
| PAGE-384 | WareHouseChalanReportList (`SInventory_UI`) | Report / Dashboard | B | [PAGE-384-warehousechalanreportlist.md](pages/PAGE-384-warehousechalanreportlist.md) |
| PAGE-385 | WarehouseStockIn (`SInventory_UI`) | Page | A | [PAGE-385-warehousestockin.md](pages/PAGE-385-warehousestockin.md) |
| PAGE-386 | WarehouseStockInApproval (`SInventory_UI`) | Approval | A | [PAGE-386-warehousestockinapproval.md](pages/PAGE-386-warehousestockinapproval.md) |
| PAGE-387 | WarehouseStockInEdit (`SInventory_UI`) | Edit form | A | [PAGE-387-warehousestockinedit.md](pages/PAGE-387-warehousestockinedit.md) |
| PAGE-388 | WarehouseStockInView (`SInventory_UI`) | List / View | B | [PAGE-388-warehousestockinview.md](pages/PAGE-388-warehousestockinview.md) |
| PAGE-389 | WarehouseStockOut (`SInventory_UI`) | Page | A | [PAGE-389-warehousestockout.md](pages/PAGE-389-warehousestockout.md) |
| PAGE-390 | WhFreezeStockRelease (`SInventory_UI`) | Page | B | [PAGE-390-whfreezestockrelease.md](pages/PAGE-390-whfreezestockrelease.md) |
| PAGE-391 | WhOpeningStockReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-391-whopeningstockreport.md](pages/PAGE-391-whopeningstockreport.md) |
| PAGE-392 | WHStockAdjustmentEntry (`SInventory_UI`) | Entry / Transaction | A | [PAGE-392-whstockadjustmententry.md](pages/PAGE-392-whstockadjustmententry.md) |
| PAGE-393 | WHStockAdjustmentView (`SInventory_UI`) | List / View | B | [PAGE-393-whstockadjustmentview.md](pages/PAGE-393-whstockadjustmentview.md) |
| PAGE-394 | WhStockConditionFreeze (`SInventory_UI`) | Page | B | [PAGE-394-whstockconditionfreeze.md](pages/PAGE-394-whstockconditionfreeze.md) |
| PAGE-395 | WHStockInList (`SInventory_UI`) | List / View | B | [PAGE-395-whstockinlist.md](pages/PAGE-395-whstockinlist.md) |
| PAGE-396 | WHStockInReportList (`SInventory_UI`) | Report / Dashboard | B | [PAGE-396-whstockinreportlist.md](pages/PAGE-396-whstockinreportlist.md) |
| PAGE-397 | WhStockMonitoringReport (`SInventory_UI`) | Report / Dashboard | B | [PAGE-397-whstockmonitoringreport.md](pages/PAGE-397-whstockmonitoringreport.md) |

Tier A = fully reviewed (11 pages: 363, 367, 370, 375, 379, 381, 385, 386, 387, 389, 392). Tier B = mechanically generated, only the business facts recorded in sections 4-7 below were verified.

## 3. Workflows and reports
Workflows: [WF-M10-01 stock-in to approval](workflows/WF-M10-01-warehouse-stock-in-to-approval.md), [02 material conversion receipt](workflows/WF-M10-02-material-conversion-receipt.md), [03 stock-out](workflows/WF-M10-03-warehouse-stock-out.md), [04 freeze and release](workflows/WF-M10-04-stock-condition-freeze-and-release.md), [05 WH adjustment](workflows/WF-M10-05-warehouse-stock-adjustment.md), [06 depot adjustment voucher](workflows/WF-M10-06-depot-stock-adjustment-voucher.md), [07 sample conversion](workflows/WF-M10-07-sample-stock-conversion.md), [08 valuation reporting](workflows/WF-M10-08-stock-valuation-reporting.md), [09 central weighted-average issue](workflows/WF-M10-09-central-store-picking-weighted-average-issue.md), [10 anomaly check/reconcile](workflows/WF-M10-10-dc-stock-anomaly-check-and-reconcile.md).
Reports: `reports/RPT-M10-01` .. `RPT-M10-10` (valuation, DC/WH stock, stock-in, depot voucher, adjustment, WH monitoring/CW monthly, DC monitoring, opening stock, monthly inventory, sample allocation). Gaps: [GAPS-M10.md](GAPS-M10.md).

## 4. Business rules index
Tier-A page rules (details in each page spec section 19):

| BR | One-line rule | Page |
|---|---|---|
| BR-M10-001 | Stock-in header needs manufacturer, supplier, date, challan no/date | 385 |
| BR-M10-002 | Each line needs code, batch, exp, mfg, qty, VAT (price optional) | 385 |
| BR-M10-003 | New stock-in Status=Posted; id MAX+1; code WHS-nnnnnnn | 385 |
| BR-M10-004 | Line = qty*price + qty*VAT; header totals summed | 385 |
| BR-M10-005 | Product pick defaults price=CostPrice, VAT=VATAmountPerUnit | 385 |
| BR-M10-006 | Exp = Mfg + 2 years; exp >= mfg | 385/370 |
| BR-M10-007 | Edit deletes/reinserts lines, resets Posted, blanks ManufacId | 385 |
| BR-M10-008 | Save hidden when sp_GET_btnShow returns rows | 385/389/392 |
| BR-M10-009 | Stock-in save does not touch stock tables | 385 |
| BR-M10-010 | Edit page requires price too | 387 |
| BR-M10-011 | Edit page blanks Status/ManufacId (drops out of queues) | 387 |
| BR-M10-012 | Approval queue = Status Posted | 386 |
| BR-M10-013 | Approve/Reject needs action + row; stamps ApproveBy/Date | 386 |
| BR-M10-014 | MC receipts call sp_Acc_MaterialConversionReceive; regular ones status only | 386 |
| BR-M10-015 | No approver entitlement / maker!=checker check | 386 |
| BR-M10-016 | MC list = unreceived production output lines (anti-join, non-Reject) | 370 |
| BR-M10-017 | MC list filtered by company/production unit; WH by company | 370 |
| BR-M10-018 | MC save needs warehouse, dates, challan, selected lines | 370 |
| BR-M10-019 | MC save Posted with MaterialConversion ids; cost = CostPerUnit, VAT 0 | 370 |
| BR-M10-020 | MC edit replaces lines; Reject frees lines | 370 |
| BR-M10-021 | MC totals over selected rows | 370 |
| BR-M10-022 | Stock-out driven from Approved stock-in via Session WHId | 389 |
| BR-M10-023 | Stock-out validations (reason, lines, non-zero qty) | 389 |
| BR-M10-024 | Out qty <= C.Stock only on postback | 389 |
| BR-M10-025 | Stock-out inserts master/detail, tblCentralStore.Quantity -= qty by MigoDetailID | 389 |
| BR-M10-026 | Stock-out bypasses weighted-average/batch ledgers | 389 |
| BR-M10-027 | Freeze only Available stock (qty>0 for DC) | 381 |
| BR-M10-028 | Conditions limited by tblStockConditionPermission per user | 381 |
| BR-M10-029 | Central-WH mode only if Session CentralWareHouse=True; DC list per user | 381 |
| BR-M10-030 | Freeze needs qty !=0, condition, qty <= stock (integer compare) | 381 |
| BR-M10-031 | DC freeze: header + tblDCStoreFreeze + tblDCStore.StockQty -= qty | 381 |
| BR-M10-032 | WH freeze via this page writes tblDCStoreFreeze rows nobody can release | 381 |
| BR-M10-033 | Bulk freeze reports success regardless of skipped rows | 381 |
| BR-M10-034 | Release list = tblDCStoreFreeze qty>0 by DC/manufacturer | 375 |
| BR-M10-035 | Release qty non-zero and <= freeze qty | 375 |
| BR-M10-036 | Release: freeze -= qty then tblDCStore.StockQty += qty (2 commits) | 375 |
| BR-M10-037 | Release rows staged, no duplicates | 375 |
| BR-M10-038 | Sample conversion needs SC, action, date, product, rows | 379 |
| BR-M10-039 | Source rows tblDCStore StockQty>0 (any condition) | 379 |
| BR-M10-040 | Qty <= stock only on postback | 379 |
| BR-M10-041 | Save decrements tblDCStore for both actions; no counterpart | 379 |
| BR-M10-042 | Delete reverses (adds back) | 379 |
| BR-M10-043 | Depot voucher needs DC, invoice/N-A, date, reason, product, rows | 367 |
| BR-M10-044 | Voucher stock rows by DC code + product + active price | 367 |
| BR-M10-045 | Voucher qty <= stock only on postback | 367 |
| BR-M10-046 | Voucher save Posted, no stock change; approved not deletable | 367 |
| BR-M10-047 | Approval: Posted queue; Approve via SP, Reject via UPDATE | 363 |
| BR-M10-048 | No approver entitlement check | 363 |
| BR-M10-049 | WH adjustment needs date, store, remarks, ticked qty | 392 |
| BR-M10-050 | Adjust rows Quantity>0; qty <= stock on postback | 392 |
| BR-M10-051 | Transaction no WHA-yymm + running sequence from 10001 | 392 |
| BR-M10-052 | Adjustment auto-Approved and always decreases stock | 392 |
| BR-M10-053 | Delete adjustment restores stock | 392/393 |

Rules verified in Tier-B pages and SP scripts (recorded here, sources given):

| BR | Rule | Source |
|---|---|---|
| BR-M10-054 | Saving condition permissions upserts only checked conditions (Permission=true); unchecking never revokes | `StockConditionPermission.aspx.cs:76-96`; `StockConditionPermissionBll.cs:31-56` |
| BR-M10-055 | WH condition freeze (PAGE-394): rows tblCentralStore Available and Quantity>0; needs selection, condition, qty; inserts tblWhStockConditionFreeze + tblWhStoreFreeze, tblCentralStore.Quantity -= qty; no server stock-limit check (only integer postback compare) | `WhStockConditionFreeze.aspx.cs`; `WhStockConditionFreezeDal.cs:22-66` |
| BR-M10-056 | WH freeze release (PAGE-390): list tblWhStoreFreeze StockQty>0; qty integer, >0, <= displayed stock (postback); submit: freeze StockQty -= qty, tblCentralStore.Quantity += qty by ReceiveId | `WhFreezeStockRelease.aspx.cs`; `WhFreezeStockReleaseDal.cs:17-65` |
| BR-M10-057 | Stock batch update (PAGE-380): rows tblDCStore StockQty>0 per DC; ticked rows need batch, mfg, exp; SP `sp_UD_StockBatch_new(@dcStoreId,@batch,@mfgdate,@expDate,@updateBy,@StockQty)`; unparsable StockQty silently becomes 0 | `StockBatchUpdate.aspx.cs`; `StockBatchUpdateDal.cs:17-36` |
| BR-M10-058 | Country/opening stock entry (PAGE-360) is legacy: inserts tblCurrentStock/tblDCStore with old columns (ReceiveId, Quantity); validation messages are copy-paste ("Please Input Company Name!!"); BLL returns false on the normal path | `CountryStockEntry.aspx.cs`; `DCStoreBLL.cs:17-68`; `DCStoreDAL.cs:16-30` |
| BR-M10-059 | Sample stock for warehouse (PAGE-376) changes tblCentralStore.StockInQty (not Quantity); rows StockInQty>0; delete re-adds | `SampleStockForWHDal.cs:33-40,86-101` |
| BR-M10-060 | DC transaction-date rule: rows with StockQty<0 or Trans_Type='Issue' dated ISNULL(ReferenceTransactionDate, StockRcvDate); others StockRcvDate | `SP_Update_StockValuation_TransactionDate_Logic.sql:90,147-148,158,230,296,319` |
| BR-M10-061 | ToDate = end of day (DATEADD(SECOND,86399,date)); FromDate used as supplied | `...sql:30,205,364` |
| BR-M10-062 | DC ledger opening (dated < FromDate): qty=SUM(StockQty), value=SUM(TotalCostAmount), rate=SUM(cost)/SUM(qty) if qty>0 else AVG(CostPerUnit); fallback tblUnitPrice.CostPrice (IsActive=1) | `...sql:44-46,83-92` |
| BR-M10-063 | Movement classes: Receive From WH (Receive/NULL/Available, no return id, condition not B2BReceive/TransferReceive/SalesReturn/Return, chalan not IDT%/RIN%/RINV%, qty>0); B2B Receive (condition B2BReceive/TransferReceive or chalan IDT%, qty>0); Return Rcv (ReturnFromInvoiceDetailId<>0 or condition SalesReturn/Return or chalan RIN%/RINV%); Sales (negative rows not B2BIssue/TransferIssue, ABS; or Issue/Sales rows); B2B Issue (condition B2BIssue/TransferIssue, ABS) | `...sql:95-150` |
| BR-M10-064 | Closing Qty = Opening + RcvFromWH + B2BRcv + ReturnRcv - Sales - B2BIssue; Closing Rate = latest CostPerUnit>0 (order StockRcvDate DESC, DCStoreId DESC, dated <= ToDate) else CostPrice; Closing Value = Closing Qty x Closing Rate | `...sql:56-66,152-159` |
| BR-M10-065 | Rows with zero opening and zero movements are excluded; DC/category/product filters optional | `...sql:163-176` |
| BR-M10-066 | National valuation, central store: latest tbl_INV_CentralStore_WeightedAvg row per Item_Id (max Weighted_Avg_Id) with Trans_Date <= ToDate gives Close_Qty, Avg_Rate, Close_Amt; opening from latest row with Trans_Date < FromDate | `...sql:283-307` |
| BR-M10-067 | National valuation, DC part: SUM(StockQty) and SUM(TotalCostAmount) per product as of ToDate; one dynamic pivot column per tblCompanyUnit name; National Total = central value + depot value; only products with positive opening/closing qty | `...sql:207-337` |
| BR-M10-068 | Batch drill-down by location: Central, Opening National, Total Depots, National, or one depot; SUM per batch, only positive | `...sql:366-481` |
| BR-M10-069 | Central STO issue valued at pre-issue average rate (Close_Amt/Close_Qty); new average = closing amt/qty; idempotent per (Mst_Trans_Id, Ref_No, Company, Item); batch row negative qty; no insufficient-stock test | `SP_Fix_CentralStoreStoIssue_Corrected.sql:99-199` |
| BR-M10-070 | Anomaly definitions: orphan Issue row = Issue with no tblInvoiceDetail (DCStoreId=ReferenceDCStoreId, TotalQuantity=ABS(StockQty)); corrupted Receive = StockQty > TotalQuantity | `sp_Check_DCStore_StockAnomalies.sql:20-91` |
| BR-M10-071 | sp_Adjust_DCStore_Stock_Only deletes matching Issue rows for an invoice and resets Receive StockQty to TotalQuantity (general step for every Receive row with StockQty <> TotalQuantity) in one transaction | `SP_Adjust_DCStore_Stock_Only.sql:30-113` |
| BR-M10-072 | Ledger semantics of tblDCStore: a sales issue inserts a new negative row (Trans_Type='Issue', ReferenceDCStoreId=source, ReferenceTransactionDate=now, cost carried) instead of updating the Receive row; DC receipts insert Receive rows with CostPerUnit/TotalCostAmount; B2B receipts StockCondition='B2BReceive' | `InvoiceDAL.cs:667-702`; `RequisitionDAL.cs:515-548`; `ChalanDAL.cs:126-165`; `sp_Delete_ProformaInvoice_Update.sql:108-140` |
| BR-M10-073 | M10 pages (freeze, sample conversion, stock batch update, other stock action) and B2B challan SP still change Receive-row StockQty in place, mixing two models | `StockConditionFreezeDAL.cs:90-98`; `SampleTypeConventionDal.cs:128-136`; `OtherStockActionDAL.cs:125-132`; `SP_NewStockTransferDcToDc_Related.sql:325-341` |
| BR-M10-074 | Inactivated unit price rows move to tblUnitPriceRollback (RollbackBy, RollbackDate) inside one transaction | `SP_Create_tblUnitPriceRollback.sql`; `ProductUnitPriceDAL.cs:220-245` |
| BR-M10-075 | WH monitoring: closing = opening(snapshot at exact fromDate) + StockIn(ReceiveDate range) - Issue(requisition picking); amount not computed | `WhStockMonitoringReportDal.cs:17` |
| BR-M10-076 | DC opening report reads tblDCStore_OpeningBalance at an exact DCOpeningBalanceDate | `DcOpeningStockReportDal.cs:24-45` |
| BR-M10-077 | Central lot availability = inbound batch rows (Trans_Qty>0) less outbound rows by Issue_Weighted_Avg_Id+batch; picking order ExpDate, ReceiveDate (FEFO) | `RequisitionDAL.cs:300-350` |

## 5. Status lifecycles
- Stock-in (`tblWHStockInMaster.Status`): new -> Posted -> Approved | Reject; Reject -> (edit) Posted; blank via WarehouseStockInEdit (defect).
- Depot voucher (`tblDeStockOutMaster.Status`): Posted -> Approved (SP) | Reject; Approved not deletable.
- WH adjustment (`ActionStatus`): Approved at creation.
- Sample conversion (`tblSampleStockForDcMaster.Status`): Posted only.
- Stock condition: Available -> (freeze) condition in freeze table -> (release) qty returned; no status column.
- tblDCStore rows: Trans_Type Receive / Issue; StockCondition Available / B2BReceive / B2BIssue / TransferReceive / TransferIssue / SalesReturn / Return / Restricted / Blocked (values seen in code).

## 6. Roles and permissions observed
No role-based checks in any M10 page: forms auth + DB-driven menu only. Observed session use: `UserId` (condition permissions, DC list via `tblUserCompanyUnit`, valuation/batch pages redirect to login if absent), `LoginName` (EntryBy), `CentralWareHouse` (enables central WH freeze), `IDInformation`/`WHId` (navigation state), `ComUnitId` (invoice autocomplete). `sp_GET_btnShow(@PageName)` hides Save buttons (PAGE-385/389/392) - body absent. Approval pages have a commented-out entitlement check (`tblAppSetup`). Per-user stock conditions: `tblStockConditionPermission` (PAGE-382).

## 7. Data model observable from code
| Table / view | Role | Notes |
|---|---|---|
| `tblCentralStore` | Legacy central stock by receive lot | ReceiveId, ProductId/Code, BatchNo, Quantity (current), StockInQty, MigoDetailID (= tblWHStockInDetail id), StockCondition, UnitPrice, VATPerUnit, Mfg/ExpDate. Mutated in place by stock-out, adjustment, freeze, release, sample-WH (StockInQty), requisition picking (`UpdateCentralStockStockOut`), rollback |
| `tbl_INV_CentralStore_WeightedAvg` | Central weighted-average ledger | Weighted_Avg_Id, Mst_Trans_Id, Item_Id/Code, Ref_No, Trans_Date, Company_Id, OP/Curr/Close Qty, Rate, Amt, Avg_Rate; source for national valuation and requisition CStock |
| `tbl_INV_Central_BatchWiseStock` | Central batch ledger | Batch_Stock_Id, Weighted_Avg_Id, Batch_No, Mfg/Exp, Trans_Qty (+in/-out), Issue_Weighted_Avg_Id, StockInTransfarId |
| `tblDCStore` | DC stock | DCStoreId, ComUnitId, ProductCode, BatchNo, TotalQuantity, StockQty, StockRcvDate, ChalanNo, StockCondition, Trans_Type (Receive/Issue), CostPerUnit, TotalCostAmount, ReferenceDCStoreId, ReferenceTransactionDate, ReturnFromInvoiceDetailId, StockInTransfarId, ChalanDetailsId |
| `tblDCStoreFreeze`, `tblWhStoreFreeze` | Quarantined qty (DC / central) | StockQty, StockCondition, Remarks, links to source row and freeze header |
| `tblStockConditionFreeze`, `tblWhStockConditionFreeze` | Freeze headers | FreezeQty, EntryBy/Date |
| `tblStockCondition`, `tblStockConditionPermission` | Condition master, per-user permission | |
| `tblDCStore_OpeningBalance`, `tblCentralStore_OpeninigBalance` (sic) | Opening snapshots keyed by date | populated outside M10 pages (UNKNOWN) |
| `tblCurrentStock`, `tblDCStoreTransaction` | legacy country stock, DC transaction log | written by CountryStockEntry / RequisitionDAL |
| `tblWHStockInMaster/Detail`, `tblWHStockOutMaster/Detail`, `tblWHStockAdjustment`, `tblWHAdjustmentDetail`, `tblDeStockOutMaster/Details`, `tblSampleStockForDcMaster/Details`, `tblSampleStockForWareHouseMaster/Details` | Documents | see page specs |
| `tblUnitPrice`, `tblUnitPriceRollback` | Cost/price master and archive | multiple rows per product (IsActive) |
| Views `View_CentralStoreCurrentStock`, `View_DCStoreCurrentStock`, `View_ProDCStock`, `View_TotalCurrentStockofCompanyWithStockInTransfar` | Current-stock views | bodies absent (UNKNOWN) |

Effect of each movement:
| Movement | tblCentralStore | Weighted-avg + batch ledger | tblDCStore |
|---|---|---|---|
| Manual stock-in / approval (PAGE-385/386) | no code write (UNKNOWN downstream) | no | - |
| Material conversion approval | via SP (UNKNOWN) | UNKNOWN | - |
| Warehouse stock-out (389) | Quantity -= qty | not touched | - |
| WH adjustment (392) | Quantity -= qty (+ on delete) | not touched | - |
| Freeze WH (381 legacy / 394) | Quantity -= qty | not touched | - |
| Release WH (390) | Quantity += qty | not touched | - |
| Picking/STO issue (requisition) | Quantity set to 0 per lot | SP writes issue rows at avg rate | DC receive inserts Receive row |
| DC freeze (381) | - | - | StockQty -= qty (in place) |
| DC release (375) | - | - | StockQty += qty (in place) |
| Sample conversion (379) | - | - | StockQty -= qty (both actions) |
| Sample stock WH (376) | StockInQty -= qty | - | - |
| Batch update (380) | - | - | SP sp_UD_StockBatch_new (UNKNOWN) |
| Depot voucher (367/363) | - | - | via SP at approval (UNKNOWN) |
| Sales issue / return (other modules) | - | - | new Issue row (negative) / return rows |

**Negative stock enforcement:** none in the database that is visible in code (no constraints/triggers in repo). Enforced only by UI postback comparisons (stock-out, adjustment, freeze, sample, voucher) which are not repeated at save; the SP for STO issue has no check; sales rollback checks stock explicitly (`SalesOperationRollbackDAL.cs:295-310`). **Freeze enforcement:** quantity removed from the stock row (in-place) and only rows with `StockCondition='Available'` and `StockQty>0` are offered/selected by stock and sales lists; reports show frozen qty as Restricted/Blocked.

## 8. Valuation method (as coded)
- **Central store:** moving **weighted average** ledger (`tbl_INV_CentralStore_WeightedAvg`): each issue is valued at the pre-issue average (Close_Amt/Close_Qty), average recomputed after each movement. Batches are tracked for traceability and FEFO allocation, not for costing. Not FIFO.
- **DC store:** each `tblDCStore` row carries `CostPerUnit`/`TotalCostAmount`. Valuation report: opening = sum of row cost amounts / sum of quantities (weighted average of cost layers incl. issues at their row cost); closing = closing quantity x **latest unit cost** (last row with CostPerUnit>0 up to ToDate) - a last-cost basis, inconsistent with the opening basis; fallback `tblUnitPrice.CostPrice`. National DC value = SUM(TotalCostAmount).
- **Transaction-date logic:** receipts by `StockRcvDate`; issues/negative rows by `ReferenceTransactionDate` (fallback `StockRcvDate`); ToDate end-of-day. See BR-M10-060..068.
- Caveat: in-place `StockQty` changes (BR-M10-073) do not adjust `TotalCostAmount`, so quantity and value can diverge.
- Cost rollback: `tblUnitPriceRollback` archives inactivated price rows (BR-M10-074).

## 9. Integrations
Cross-database reads of the production ERP `CSTL_ERPDB_Unicorn` (material conversion, batch stock; STO issue SP also references `CSTL_ERPDB..Table_Inventory_Types`); SAP-related stock (B2B challan SPs read `SAP_API_Data..tblSAP_StockMovement*`, other module); `ExcelUpForMIGODAL` imports MIGO (outside scope).

## 10. Known gaps summary
See [GAPS-M10.md](GAPS-M10.md). Top: invalid stock-in header INSERT (GAP-M10-001), edit page blanks status (002), approval posts no stock in code (003), two diverging central stock models (006), sp_Adjust_DCStore_Stock_Only resets legitimate in-place adjustments (005), valuation basis mix (007), SP bodies missing (013), universal SQL concatenation (014).

## 11. Still unverified
Bodies of `sp_GET_btnShow`, `ExecuteAllSqlQueryByStoreProcedure`, `sp_Acc_MaterialConversionReceive`, `sp_UD_DcStockOutApproval`, `sp_UD_StockBatch_new`, report SPs and views; how central stock rows (`tblCentralStore.MigoDetailID`) are created after approval; Tier-B page details (opening stock, sample stock for warehouse, AdjustmentEntry types, list/view pages, viewers other than those cited); live-database versions of the valuation SPs; DB constraints/triggers on stock tables; runtime behaviour (nothing was executed).
