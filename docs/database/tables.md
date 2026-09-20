# Tables touched by the repository SQL scripts (Part A)

Scope: the repository contains **no table-creation DDL** except `tblUnitPriceRollback`. Table definitions are otherwise only visible through the 20 SQL scripts' `ALTER TABLE`, `CREATE INDEX`, `INSERT/UPDATE/DELETE` and `SELECT` statements (read in full for this document). Types, nullability and defaults below are given **only where a script states them** (`CONFIRMED`); columns known only from DML are listed with their usage and `type UNKNOWN`. Full column lists, primary/foreign keys and existing indexes are UNKNOWN (see `docs/database/relationships.md` for generated relationship inventories). Stored-procedure detail is in `docs/database/stored-procedures.md`; SP names below link by name.

Databases: `SalesDisDB_Unicorn_DB` (default), `CSTL_ERPDB_Unicorn`, `CSTL_ACCDB_Unicorn`, `SAP_API_Data` (cross-database, 3-part names).

## 1. Schema-change inventory (DDL executed by the scripts)

| # | Object | Change | Script:line | Idempotent? |
|---|---|---|---|---|
| 1 | `tblCustMaster` | ADD `TaggedSalesPersonIds NVARCHAR(MAX) NULL` | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:7-12` | yes (`sys.columns` guard) |
| 2 | `tblDoctorMaster` | ADD `TaggedSalesPersonIds NVARCHAR(MAX) NULL` | `LIVE_DB_UPDATE...:14-19` | yes |
| 3 | `tblSalesReturnDetail` | ALTER COLUMN `ProductName NVARCHAR(500)`, `ProductCode NVARCHAR(150)` when `ProductName.max_length < 1000` bytes | `LIVE_DB_UPDATE...:22-28` | yes (guarded by byte length) |
| 4 | `tblTempSalesReturnOrder` | ALTER COLUMN `CustomerName NVARCHAR(250)`, `ComUnitName NVARCHAR(250)`, `MIOName NVARCHAR(250)` when `CustomerName.max_length < 500` bytes | `LIVE_DB_UPDATE...:30-37` | yes |
| 5 | `tblCustomerReturnMaster` | ADD `DeductionCharge DECIMAL(18,2) NULL` constraint `DF_tblCustomerReturnMaster_DeductionCharge DEFAULT 0.00` | `Live_Deployment_Script_All_Updates.sql:17-27` | yes |
| 6 | `tblCustomerReturnMaster` | ADD `NetReturnAmount DECIMAL(18,2) NULL` constraint `DF_tblCustomerReturnMaster_NetReturnAmount DEFAULT 0.00` | `Live_Deployment...:29-39` | yes |
| 7 | `tblReturnInvoice` | ADD `DeductionCharge DECIMAL(18,2) NULL` (`DF_tblReturnInvoice_DeductionCharge DEFAULT 0.00`) | `Live_Deployment...:42-52` (repeated, unnamed default, at `:822-831`) | yes (second block is a no-op) |
| 8 | `tblReturnInvoice` | ADD `NetReturnAmount DECIMAL(18,2) NULL` (`DF_tblReturnInvoice_NetReturnAmount DEFAULT 0.00`) | `Live_Deployment...:54-64` (repeated `:833-842`) | yes |
| 9 | `tblReturnInvoice` | ADD `IsPosting BIT NULL` (`DF_tblReturnInvoice_IsPosting DEFAULT 0`) | `Live_Deployment...:66-76` | yes |
| 10 | `tblDCStore` | ADD `ReturnInvoiceDetailId INT NULL` (`DF_tblDCStore_ReturnInvoiceDetailId DEFAULT 0`) | `Live_Deployment...:79-89` (repeated `:845-853`) | yes |
| 11 | `CSTL_ERPDB_Unicorn.dbo.Table_Integration_AccountsConnectivity` | DML upsert: row (CompanyId 1, PageId 23, FlagId 1032, GLId 5968, SubGL 0, DRCR 'CR'); if a row for (PageId 23, FlagId 1032) exists it is UPDATEd to `GLId=5968, DRCR='CR'` for **all** companies | `Live_Deployment...:95-115` | yes (state-converging) |
| 12 | `tblCustomerPay` | ADD `Remarks NVARCHAR(500) NULL` (guard is `INFORMATION_SCHEMA.COLUMNS` by table name only - any schema) | `SP_Fix_CustomerPayment_Live.sql:10-24` | yes |
| 13 | `tblUnitPriceRollback` | CREATE by `SELECT TOP 0 ... INTO` from `tblUnitPrice` (types copied; `UnitPriceId` CAST to INT drops IDENTITY), then ADD `RollbackId INT IDENTITY(1,1) NOT NULL` PK `PK_tblUnitPriceRollback`, `RollbackBy NVARCHAR(100) NULL`, `RollbackDate DATETIME NOT NULL` default `GETDATE()` (`DF_tblUnitPriceRollback_RollbackDate`); CREATE INDEX `IX_tblUnitPriceRollback_ProductId (ProductId)` | `SP_Create_tblUnitPriceRollback.sql:4-21` | yes (`OBJECT_ID IS NULL` guard) |
| 14 | `tblProviderDropoutIntrigration` | ADD `IsApprove BIT NOT NULL` (`DF_tblProviderDropoutIntrigration_IsApprove DEFAULT (0)`), `IsApproveBy NVARCHAR(50) NULL`, `ApproveDate DATETIME2 NULL` | `Library.DAL/SAP_IntegrationDAL/sql/alter_tblProviderDropoutIntrigration_AddApprovalColumns.sql:1-21` | yes (`COL_LENGTH` guard) |
| 15 | `tblInvoice` | data UPDATE (header totals synced to details) - see section 8 | `SP_Fix_Invoice_Anomaly_And_Sync.sql:13-32` | re-runnable (only updates rows still differing) |
| 16 | `tblOrder` / `tblOrderDetail` | one-off data repair / insert for `ODR-20260901134` - see section 8 | `Create_Order_ODR-20260901134.sql` | re-runnable (branches on existence) |

**Only index/constraint DDL in the repo:** `IX_tblUnitPriceRollback_ProductId`, `PK_tblUnitPriceRollback` and the 8 named default constraints above. **No other `CREATE INDEX` exists**; SPs rely on undocumented existing indexes (UNKNOWN). Missing-index candidates suggested by the scripts' access paths (recommendation only, not applied): `tblDCStore(ComUnitId, ProductCode, StockRcvDate)` and `tblDCStore(ReferenceDCStoreId, Trans_Type)` (ledger queries, `sp_Delete_ProformaInvoice`), `tblCustMaster(CustomerCode)`, `tblOrder(OrderCode)`.

Caveats for the DDL (CONFIRMED in script text):
- Items 3-4: `ALTER COLUMN ... NVARCHAR(n)` without `NULL`/`NOT NULL` re-declares the column as **nullable** (SQL Server default), silently dropping a prior `NOT NULL`. The comment calls them "temp tables" (`:21`), but they are real tables that the C# code fills and `TRUNCATE`s (`Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:1306-1326`).
- Items 5/6/7/8: the two `tblReturnInvoice` blocks and the `tblDCStore` block appear twice in `Live_Deployment_Script_All_Updates.sql` (steps 1.x and 4.x).
- Item 10: the added `tblDCStore.ReturnInvoiceDetailId` is **not referenced by any SP in the scripts or by any C# statement found** (C# inserts use `ReturnFromInvoiceDetailId` / `SalesReturnDetailId`, `CustomerReturnDAL.cs:196-197`, `InvoiceDAL.cs:1836-1837`); its consumer is UNKNOWN.
- Item 11: hard-coded GL account id `5968` for "Return Delivery Charge Income" (`PageId 23`, `FlagId 1032`).
- Columns **assumed to pre-exist** (used by the scripts but created by none): `tblCustMaster.TaggedSalesPersonCodes`, `tblDoctorMaster.TaggedSalesPersonCodes`, `tblCustomerReturnMaster.IsPosting`, `tblChalanInfo.SAP_ChallanSend`, `tblChalanInfo.SAP_Challan_ConfirmationSend`, `tblDCStore.ReferenceTransactionDate`, `ReferenceDCStoreId`, `Trans_Type`, `StockCondition`, `CostPerUnit`, `TotalCostAmount`, `ReturnFromInvoiceDetailId`, `tblOrder.IsPrepareforInvoice`, `tblOrder.IsSubDepo`, `tblOrder.*_Ord` snapshot columns, `tblOrderDetail.Status`, `tblReturnInvoice.RegionId_Rtn/AreaId_Rtn/Terri_Id_new/...`, `tblVoucherMaster.SalesReturnId/DestroyId/IsInventoryValuation` (accounts DB).

## 2. Customer and doctor masters

### tblCustMaster
| Column | Definition / source | Use |
|---|---|---|
| `TaggedSalesPersonIds` | `NVARCHAR(MAX) NULL` (added `LIVE_DB_UPDATE...:9`) | comma-separated `EmpInfoId` list of tagged sales persons; written by `sp_Save_CustomerMaster` (defaults to `EmpInfoId`), `sp_Update_CustomerMaster` (overwritten), `sp_UD_CustomerMaster` (`ISNULL` keep); read with `','+ids+',' LIKE '%,'+id+',%'` in `sp_Get_CustMasterList_Approve`. Stored as text, no FK. |
| `TaggedSalesPersonCodes` | type UNKNOWN (SP params are `nvarchar(500)`; column not created by any script) | parallel comma list of employee master codes; written by save/update/UD SPs. |
| `CustomerMasterId` | PK (identity; `SCOPE_IDENTITY()` used) | key in all SPs. |
| `CustomerCode` | text; generated `'C'+n` | unique-by-convention only; generation is MAX(numeric tail of Approved rows)+1. |
| `ActionStatus` | NVARCHAR; '0' Pending, '1' Verified, '2' Approved, '3' Rejected | insert forces '2'; `sp_Get_CustomerApp` hides '2','3'. |
| `CellNo` | text | duplicate-cell detection CTE in `sp_Get_CustomerApp` (non-rejected rows). |
| `SalesPersonId` | int -> `tblEmpGeneralInfo.EmpInfoId` | set from `@EmpInfoId`; used by AR ageing (`Responsible Person`). |
| `IsActive`, `CreateBy`, `CreateDate`, `UpdateBy`, `UpdateDate` | bit / int / datetime | audit; `UpdateBy/Date` are set by `sp_Update_CustomerMaster` only. |
| `MarketId`, `MarketName`, `GroupId`, `RegionId`, `AreaId`, `TerritoryId`, `SubTerritoryId`, `DivisionId`, `DistrictId`, `ThanaId` | int / text | hierarchy re-derived from `MarketId` on every insert/update. |
| `StationTypeId`, `NSMStationTypeId`, `DZSMStationTypeId` | int | from `tblMarketStationDetail` (`UserRoleID` 1, 2, 3). |
| `CustomerName`, `Address`, `CustomerStation`, `Division`, `District`, `Thana`, `CustomerType`, `CustomerTypeId`, `DistributionRouteId`, `OwnerName`, `VoterID`, `TradeLicense`, `DrugLicense`, `PharmacyCouncilCertificate`, `BCDS`, `Email`, `Reamrks` (sic), `ProgramTypeId`, `ComUnitId`, `ComUnitName`, `ComUnitCode` | text/int | written by save; most re-written by update. |
| legacy set by `sp_UD_CustomerMaster`: `ConPerson`, `FixedCustomer`, `CategoryId`, `Addrees2` (sic), `City`, `ShippingCond`, `MarketCode`, `MIACode`, `AreaCode`, `DisCode`, `FEName`, `RegionCode`, `DZSMName`, `TermOfPayment` | text/bit | SAP/legacy attributes. |
| `SMCTypeId`, `CustomerBsPCode` | int / text | read-only in list SPs. |
Children touched: `tblCustProductLine` (DELETE by `CustomerMasterId` in `sp_Update_CustomerMaster`); read: `tblCustomerApprovalLog`, `tblCustomerCreditLimit`.

### tblDoctorMaster (+ child tables)
| Column | Definition / source | Use |
|---|---|---|
| `TaggedSalesPersonIds` | `NVARCHAR(MAX) NULL` (added `LIVE_DB_UPDATE...:16`) | as above, written by `sp_Save_DoctorMaster` / `sp_Update_DoctorMaster`. |
| `TaggedSalesPersonCodes` | type UNKNOWN, not created by scripts | as above. |
| `DoctorId` (PK), `DoctorCode` (`'D'+n`), `ApprovalStatus` ('2' on insert), `IsFromApp` (0 on insert), `IsDoctor` (list SP filters `=1`), `SalesPersonId`, `EntryBy/EntryDate`, `UpdateBy/UpdateDate`, `IsActive`, `Activedate`, `InactiveDate`, `DesignationId`, `Gender`, `DoctorTypeId`, `MarketId`, `DivisionId/DistrictId/ThanaId`, `UPCode`, `UnionName`, `StreetAddress`, `SecondaryCode`, `StationTypeId`, `ProgramTypeId`, `DoctorCategoryId`, `SMCTypeId`, `Reamrks` | mixed | written by save/update. |
Children deleted by `sp_Update_DoctorMaster`: `tblDoctorBrandDetail`, `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorSpecialDayDetail`, `tblDoctorContactDetail`, `tblDoctorChemberDetail` (except chambers referenced by `tbl_PrescriptionMaster.ChemberId` / `tbl_DCRInfo.ChemberId`). Read by `sp_Get_DoctorList`: `tblDoctorDesignation`, `tblDoctorType`, `tblDoctorChamber`, `tblDoctorDegree`, `tblDoctorSpeciality`, `tblDoctorProgramType(+Detail)`.

## 3. Orders

### tblOrder
Written by `sp_Save_ManualOrderMaster` / `sp_Update_ManualOrderMaster` (and the one-off `Create_Order_ODR-20260901134.sql`); read by `sp_LoadOrderListForOrderCreationbyTerri`, `sp_GET_PaymentInvSP*`, `sp_Get_MoneyReceiptReportAfterPaymentList`.
| Column | Meaning as used by scripts |
|---|---|
| `OrderId` (PK, identity) | `MAX(OrderId)+1` predicts the code suffix |
| `OrderCode` | `ODR-yyyymmdd<MAX(OrderId)+1>`; `ORD-000000...` placeholders are replaced |
| `ActionStatus` | '2' / 'Accepted' / 'Approved' = approved; manual orders forced '2' |
| `IsInvoice` | 0 = not yet invoiced (queue filter); set to 1 by invoicing (code not in scripts), reset to 0 by `sp_Delete_ProformaInvoice` and the one-off fix |
| `IsPrepareforInvoice` | 1 or NULL = ready for invoicing |
| `IsManual` | 1 for web-manual orders (queue includes `IsManual=1` even if `OrderType` differs) |
| `IsFromApp` | 0 for manual |
| `IsSubDepo` | 0 or NULL = main-depot order (sub-depot orders excluded from queue) |
| `OrderType` | 'Regular' (or NULL) |
| `OrderSenderType` | 'Web Manual' for web-entered; app orders use `OrderSenderCode/OrderSenderName` (not set here) |
| `ComUnitId/ComUnitCode/ComUnitName`, `MIOCode`, `MIOName`, `ManufacId`, `TerritoryCode` | ownership and sender |
| `CustomerCode`, `CustomerName`, `CustomerMasterId`, `CustTypeId`, `ProgramTypeId`, `DistributionRouteId`, `MarketId`, `TerritoryId`, `AreaId`, `RegionId`, `GroupId`, `SubTerritoryId` | snapshot of customer master at order time |
| `GroupName_Ord`, `RegionName_Ord`, `AreaName_Ord`, `TerritoryName_Ord`, `SubTerritoryName_Ord`, `MarketName_Ord`, `SmcTypeId_Ord`, `SMCType_Ord` | order-time name snapshots |
| `GrossValue`, `TotalVat`, `TotalDiscount`, `TotalNetPayable` | DECIMAL(18,4) parameters -> column types UNKNOWN |
| `SubmissionDate`, `EntryDate`, `ServerDateTime`, `DeliveryDate`, `FixedCustomer`, `Remarks`, `PaymentType` | |

### tblOrderDetail
Columns written by `sp_Save_ManualOrderDetail`: `ProductId, ProductCode, ProductName, Quantity, TradePrice, TotalTradePrice, ISGiftProduct, IsCampaignProduct, IsSpDis (0), OrderId, DiscountPercent (0), DiscountAmount (0), UnitVatAmount, TotalVatAmount, NetAmount`; `[Status]` (set NULL by `sp_Delete_ProformaInvoice` and the one-off fix - a per-line "invoiced" flag whose non-NULL values are set elsewhere; UNKNOWN), `OrderDetailsId` (PK; referenced by `tblInvoiceDetail.OrderDetailsId`). Deleted wholesale (`WHERE OrderId`) by `sp_Update_ManualOrderMaster`.

## 4. Invoices, payments, credit

### tblInvoice
Columns read/written by the scripts: `InvoiceId, InvoiceNo, InvoiceDate, OrderId, OrderNo, OrderSenderCode, OrderSenderName, CustomerMasterId, ComUnitId, PaymentTypeId, DelivaryInvoiceNo` (sic; NULL = proforma/undelivered), `DeliveryInvoiceStatus` ('Full'/'Partial'), `DeliveryTpGrandTotal`, `DeliveryCharge` (added into receivable in the June-Sept-2026 payment SPs), `TpTotal, TpDiscount, TpVat, TpGrandTotal, AdjustAmount, ReceivableAmount` (header totals, synced by `SP_Fix_Invoice_Anomaly_And_Sync.sql`), `PaymentInvoiceNo` (non-NULL = payment invoice exists), `PaymentDate`, `PaymentStatus` (NULL / 'Partial' = payable), `PaymentInvoiceStatus` ('Reject'), `SndReturnInvoiceNo` (non-NULL = second-time return), `PaymentAmount`, `AIT`, `DiscountOnPayment` (read by the credit query), `UpdateDate`, `DelivarySpecialAmount`.
Deleted by `sp_Delete_ProformaInvoice` (by `InvoiceNo`, only when `DelivaryInvoiceNo IS NULL`).

### tblInvoiceDetail
Columns: `InvoiceDetailId, InvoiceId, OrderDetailsId, DCStoreId, ProductCode, ProductName, PackSize, BatchNo, ReceiveDate, ExpDate, CostPrice, UnitPrice, UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, TotalPriceVatAmount, DiscountPercentage, DiscountAmount, NetAmount, SpecialAmount, ReturnReason` (order-time amounts); `Delivery*` set (`DeliveryQuantity, DeliveryBonusQuantity, DeliveryTotalQuantity, DeliveryTotalPrice, DeliveryTotalPriceVatAmount, DeliveryDiscountPercentage, DeliveryDiscountAmount, DeliveryNetAmount, DeliveryStatus`) and `Payment*` set (`PaymentNetAmount, PaymentTotalPrice, PaymentTotalPriceVatAmount`). **Three parallel amount sets** (order-time `NetAmount`, delivery-time `DeliveryNetAmount`, payment-time `PaymentNetAmount`) are used by different reports: the anomaly checker/sync use `NetAmount`, the AR ageing uses `DeliveryNetAmount`, the payment SPs use `PaymentNetAmount`.

### tblInvoiceDetail_DeleterRecord, tblInvoiceDeleteLog, tblProInvoiceReturnTrack, tblReturnAmount
Written by `sp_Delete_ProformaInvoice`: `_DeleterRecord` receives a full copy of every deleted detail line (`InvoiceDetailId, ProductCode, ProductName, PackSize, BatchNo, ReceiveDate, ExpDate, CostPrice, UnitPrice, UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, TotalPriceVatAmount, DiscountPercentage, DiscountAmount, NetAmount, InvoiceId, DCStoreId, Delivery* columns, DeliveryStatus, OrderDetailsId, SpecialAmount, DelivarySpecialAmount, ReturnReason`); `tblInvoiceDeleteLog(InvoiceId, InvoiceNo, DeleteDateTime, DeleteBy)` audit trail; `tblProInvoiceReturnTrack(ReturnTotalQuantity, DCStoreId, ReturnDate (text dd Mon yyyy), ReturnExecutionDateTime, InvoiceDetailId, InvoiceId)`; `tblReturnAmount` rows for the invoice are deleted. These log tables are the only audit evidence of proforma deletions and are used by `sp_Check_DCStore_StockAnomalies` and `sp_Adjust_DCStore_Stock_Only` to trace orphans.

### tblCustomerPay / tblCustPayDetail
- `tblCustomerPay`: `CustPayId` (PK), `PaymentDate`, `RefNo`, `PayType`, **`Remarks NVARCHAR(500) NULL`** (added by `SP_Fix_CustomerPayment_Live.sql`, item 12).
- `tblCustPayDetail`: `CustPayDetailId, CustPayId, InvoiceId, PaymentAmount, Discount, AIT, TPAmount, VATAmount, custPaymentDate, BankAccId, CashAccId`. Settlement rule used everywhere: **paid = PaymentAmount + Discount + AIT** (Discount and AIT count as settlement); TP/VAT parts (`TPAmount`, `VATAmount`) track how the payment is allocated to trade price vs VAT.
- Voucher creation for these rows is in `sp_Sales_CustomerPaymentPosting`.

### tblCustomerCreditLimit
Read only (`sp_LoadOrderListForOrderCreationbyTerri`): `CompanyId, CustomerMasterId, LimitAmount, DayLimit, IsActive, ActionStatus` (`'Approved'` compared trimmed/upper). Multiple active approved rows for the same customer+company are **summed**.

## 5. Returns

### tblCustomerReturnMaster (delivery-time customer return)
Columns: `CustReturnId` (PK), `InvoiceId`, `TotalAmount`, **`DeductionCharge DECIMAL(18,2) NULL DEFAULT 0.00`** and **`NetReturnAmount DECIMAL(18,2) NULL DEFAULT 0.00`** (added; item 5-6), `IsPosting` (set to 1 by `sp_DeliverySalesReturnPosting`; creation not in scripts), `ActionStatus, ApprovedBy, ApprovedDate` (updated by C# `CustomerReturnDAL.cs:109`). Detail: `tblCustomerReturnDetail(CustReturnId, InvoiceDetailId, ReturnQty)` read by the posting SP. Business rule: `NetReturnAmount = TotalAmount - DeductionCharge` (recomputed by the SP when <= 0); the deduction is the "return delivery charge" retained as income (GL 5968).

### tblReturnInvoice (market / field return)
Columns inserted by `spInsertReturnInvoice_new`: `ReturnInvoiceId` (PK identity), `ReturnInvoiceNo` (`RIN--yymmdd<n>`), `ReturnInvoiceDate, CreateBy, CreateDate, OrderNo, OrderDate, OrderId, CustomerMasterId, ComUnitId, MiaId, PaymentTypeId, TpTotal, TpDiscount, TpVat, TpGrandTotal, TotalSpecialAmount, OldTradePolicy, ProductOffer, Remarks, MIACode, MIAName, MarketCode, MarketName, AreaCode, DisCode, FEName, RegionCode, DZSMName, FixedCustomer, InvoiceId, IsSalesReturnWithoutOrder ('No order' constant), SubInvoiceId (0), Types, MIOId_new, Terri_Id_new, MioEmpId_new, Mio_SapCode_New, RegionId_Rtn, AreaId_Rtn` plus the added **`DeductionCharge`, `NetReturnAmount` (DECIMAL(18,2) NULL, default 0.00)** and **`IsPosting BIT NULL default 0`** (item 7-9; set to 1 by `sp_SalesReturnPosting`). Detail table `tblReturnInvoiceDetail(ReturnInvoiceId, ProductCode, Quantity, TotalQuantity, CostPrice, TotalPrice, NetAmount, ...)` is read by `sp_SalesReturnPosting` for the cost valuation. `TpGrandTotal` is summed as `AdjustableAmount` in the payment SPs (`ReturnTotal`).

### tblInvoiceDetailReturn (second-time return lines)
Inserted by `sp_UpdateAndInsertInvoiceDetailSalesReturn`: `InvoiceId, InvoiceDetailId, PreviousQuantity, sndReturnQuantity, sndReturnBonusQuantity, sndReturnTotalQuantity, sndReturnTotalPrice, sndReturnTotalPriceVatAmount, sndReturnDiscountPercentage, sndReturnDiscountAmount, sndReturnNetAmount, sndReturnStatus, sndReturnReason`; PK returned via `SCOPE_IDENTITY()` as `InvoiceDetailReturnId`.

### tblSalesReturnDetail / tblTempSalesReturnOrder (staging)
Staging tables **truncated wholesale** by `ReturnInvoiceDAL.DeleteTempSalesReturnInfo` (`ReturnInvoiceDAL.cs:1319-1326`) - they are global, not per-user, so concurrent users of the sales-return screen would corrupt each other's staging (INFERRED from `TRUNCATE` + no user key). Column changes by this repo's script: `tblSalesReturnDetail.ProductName -> NVARCHAR(500)`, `ProductCode -> NVARCHAR(150)`; `tblTempSalesReturnOrder.CustomerName/ComUnitName/MIOName -> NVARCHAR(250)`. Known other columns: `tblSalesReturnDetail(ProductId, ProductCode, ProductName, Quantity, TradePrice, TotalTradePrice, OrderId)`; `tblTempSalesReturnOrder(OrderCode, CustomerCode, ComUnitCode, ...)`. Truncation errors from overlong product names were the reason for the widening (INFERRED from the comment `:21`).

## 6. Stock and inventory

### tblDCStore (depot stock ledger)
The central stock table for depots (DC). Row types (CONFIRMED from SP logic and `ChalanDAL.cs`): **Receive** (`Trans_Type='Receive'`, `StockQty=TotalQuantity` when untouched), **Issue** (`Trans_Type='Issue'`, `StockQty<0`, `ReferenceDCStoreId` = source Receive row, `ReferenceTransactionDate`, `StockCondition` such as `B2BIssue`), B2B inward (`StockCondition='B2BReceive'`/`TransferReceive`), customer returns (`ReturnFromInvoiceDetailId`, `ChalanNo LIKE 'RIN%'`), rows from `sp_SAP_B2BStockReceiveByDc` (`StockCondition='Available'`, `Trans_Type='Receive'`). Stock = `SUM(StockQty)`.
| Column | Use in scripts |
|---|---|
| `DCStoreId` | PK, **not identity in practice**: `MAX+1` in `sp_SAP_B2BStockReceiveByDc` and C# inserts |
| `ComUnitId`, `ProductCode`, `ProductName`, `PackSize`, `BatchNo`, `MfgDate`, `ExpDate` | dimensions (product joined by text code) |
| `TotalQuantity` | original received quantity; anomaly test `StockQty > TotalQuantity` |
| `StockQty` | signed quantity; **in-place decrement** by `sp_SAP_B2BChallanDetailInsert`, in-place add by `sp_Del_B2BDelete` v1; reset to `TotalQuantity` by `sp_Adjust_DCStore_Stock_Only` |
| `DamageQty`, `StorageLocation` | set 0 / '' by `sp_SAP_B2BStockReceiveByDc` |
| `StockRcvDate` | ledger date for receipts (and for issues in the v-A ledger) |
| `ReceiveDate`, `ChalanNo`, `ChalanDate`, `ChalanDetailsId` | link to challan (`IDT%` prefix = inter-depot transfer, `RIN%` = return) |
| `Trans_Type` | 'Receive' / 'Issue' / 'Sales' / 'Available' / '' / NULL |
| `StockCondition` | 'Available', 'B2BReceive', 'TransferReceive', 'B2BIssue', 'TransferIssue', 'SalesReturn', 'Return' |
| `ReferenceDCStoreId`, `ReferenceTransactionDate` | issue -> source row / real transaction date (used by the v-B ledger and national reports) |
| `CostPerUnit`, `TotalCostAmount` | valuation; **not set** by the SAP receive SP |
| `ReturnFromInvoiceDetailId` | return marker; `TempReceiveId` links to `tblCentralStore.ReceiveId` (cost/price source in the return-posting SP) |
| `ReturnInvoiceDetailId` | **added by script** (`INT NULL DEFAULT 0`), consumer UNKNOWN |
| `StockInTransfarId`, `ReqId`, `ReqChildId` | STO origin (C#) |

### Central store: tbl_INV_CentralStore_WeightedAvg, tbl_INV_Central_BatchWiseStock, tblCentralStore, tblStockInTransfar
- `tbl_INV_CentralStore_WeightedAvg` (weighted-average ledger, one row per movement): `Weighted_Avg_Id` (PK), `Mst_Trans_Id, Trans_Id, Item_Code, Ref_No, Trans_Date, Actual_Date, Entry_Date, Trans_Type_Id, Inv_Type_Id, Inv_Type, Company_Id, Warehouse_Id, Item_Id, OP_Qty/OP_Rate/OP_Amt, Curr_Qty/Curr_Rate/Curr_Amt, Close_Qty/Avg_Rate/Close_Amt, Weighted_Avg_Id_Issue`. Insert-only in `tbl_INV_CentralStoreStoIssue`.
- `tbl_INV_Central_BatchWiseStock`: `Batch_Stock_Id, Weighted_Avg_Id, Issue_Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty, Remarks ('STO Picking'), StockInTransfarId`. The meaning of `Weighted_Avg_Id` vs `Issue_Weighted_Avg_Id` is **swapped between the two versions** of the SP (see stored-procedures.md).
- `tblStockInTransfar`: `StockInTransfarId, ProductCode, PickingQty, ReceiveId, ReqId, BatchNo, MfgDate, ExpDate, ReceiveDate` (read). `tblRequisition`: `ReqId, ReqNo, PickingNo, PickingDate`. `tblCentralStore`: `ReceiveId, UnitPrice, VATPerUnit` (cost source for delivery-return COGS).

### Challan tables
- `tblChalanInfo`: `ChalanId` (PK, `MAX+1`), `ChalanDate, ChalanNo, TrackNo, DriverName, FromComUnitCode/Name/Address/Id, ToComUnitCode/Name/Address, TotalValue, TotalVat, GrandTotal, ManufacId, IsDeliver ('False' string), Note ('From SAP'), SAP_ChallanSend (bit), SAP_Challan_ConfirmationSend (bit)`.
- `tblChalanDetail`: `ChalanDetailsId` (`MAX+1`), `ChalanId, ProductCode, ProductName, Quantity, BatchNo, UnitPrice, Value, Vat, ValueWVat, DCStoreId`.
- `tblDepotToWHChalanInfo` / `tblDepotToWHChalanDetail`: keyed by `SChalanId`; deleted by `sp_DeleteDwpotToWhChalan` (`IsTransfer` appears in C#).
- `tblWHStockInMaster`: `WHStockInMasterID, WHStockInCode, WHStockInDate, Status, TotalValue, ChallanNo` (read: excludes already received SAP challans; accounts audit TableID 2013).

### tblUnitPrice / tblUnitPriceRollback
`tblUnitPrice`: `UnitPriceId` (identity), `ProductId, ProductCode, ProductName, PackSize, CostPrice, UnitPrice, VATPercentage, VATAmountPerUnit, MRPPrice, IsActive, ActiveDate, InActiveDate`; read as the fallback cost (`CostPrice`, `IsActive=1`) and SAP price/VAT source. `tblUnitPriceRollback` (item 13): same columns + `RollbackId, RollbackBy, RollbackDate`. Business rule (from the header comment `SP_Create_tblUnitPriceRollback.sql:1` and `ProductUnitPriceDAL.cs:219`): deactivating a price **moves the row** to the rollback table and deletes it from `tblUnitPrice` in a single transaction (`MoveUnitPriceToRollback`). Risk: `SELECT INTO` does not copy indexes/defaults; `tblUnitPrice` rows referenced by other tables are hard-deleted.

## 7. Cross-database accounts tables (`CSTL_ACCDB_Unicorn` / `CSTL_ERPDB_Unicorn`)

Written by the posting SPs; **none of their DDL is in the repo**:
| Table | Columns used |
|---|---|
| `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster` | `VoucherMasterId (PK), VoucherCode, CompanyInfoId, FinancialYearId, PostingDate, VoucherCategoryID (2), CurrencyId (1), CurrencyCovRate (1.0), Narration, PaytoIsSupplier (0), IsInvoice (0), TotalAmountBDT, TotalAmount, Remarks, EntryBy ('admin' / 1), EntryDate, VoucherType ('Journal'/'CreditVoucherBank'/'CreditVoucherCash'), ActionStatus ('Approved'), IsCostCenter/IsBuyerCostCenter/IsOtherCostCenter (0), ApprovedBy ('Auto Process'), ApprovedDate, CommonPK_ID, TableID, IsInventoryValuation, SalesReturnId, DestroyId` |
| `..tblDebitCreditVoucherDetail` | `VoucherMasterId, AccountId, DebitorCredit ('Debit'/'Credit'), DebitAmount, CreditAmount (both 0.00), DebitAmountBDT, CreditAmountBDT, Remarks, SubsidoryLedgerId (sic), RefInfoInd` - the BDT columns carry the amounts; the un-suffixed ones are always 0 |
| `..tblSubsidiaryInfo` | `SubsidiaryInfoId, SubsidiaryTypeId (1 = customer), S_Info_Id (= CustomerMasterId), IsSelect, Description, EntryBy, EntryDate` (row auto-created by `sp_SalesReturnPosting`) |
| `..tblReferenceInfo` | `ReferenceInfoId, ReferenceTypeId (1016 = invoice, 1018 = return invoice), R_Info_Id, Description, EntryBy, EntryDate` |
| `..tblFinancialYear` | `FinancialYearId, CompanyId, StartDate, EndDate, Status ('Active')` |
| `..tblChartOfAccounts` | `AccountId, BankAccountId, CashAccountId` |
| `CSTL_ERPDB_Unicorn..tblCompanyInfo` | `CompanyId` |
| `CSTL_ERPDB_Unicorn..tbl_AccIntegrationTable` | `IntegrationTableId, Table_Name` (`tblCustomerPay`) |
| `CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity` | `CompanyId, PageId, FlagId, GLId, SubGL, DRCR` - GL routing table; flags used: PageId 23 (return posting): 1016 FG inventory (delivery return), 1018 FG inventory (market return), 1019 COGS (market return), 1020 sales return, 1021 COGS (delivery return), 1032 return delivery-charge income (added by script), 2 AR; PageId 163 (payments): 2 AR, 1023 discount, 1024 AIT |

`TableID` key convention: 2013 material-conversion receive, 2014 delivery confirmation (sales + COGS vouchers), 2016 destroy, 2017 customer payment, 2018 sales return, 2023 return inventory valuation (from `SP_Audit_Sales_Accounts_Voucher_Posting.sql`). `SAP_API_Data..tblSAP_StockMovementMaster(StockMovementMasterId, challan_code, challan_date, from_plant_code, to_plant_code)` and `..tblSAP_StockMovementDetail(StockMovementMasterId, product_code, quantity, batch_no, expiry_date, manufacturer_date)` are read by the SAP challan SPs.

### tblProviderDropoutIntrigration (SAP/eProgram provider dropout)
Columns seen: `providerIDropoutIntrigrationd` (PK, sic), `providerId, programShortName, providerCode, providerName, mobileNo, nid, email, outlet, dropoutReason, insertedAt`, plus added `IsApprove BIT NOT NULL DEFAULT 0`, `IsApproveBy NVARCHAR(50) NULL`, `ApproveDate DATETIME2 NULL` (stored as UTC via `SYSUTCDATETIME()`). No INSERT exists in the repo (rows arrive from an external integration - UNKNOWN); the base CREATE TABLE is not in the repo.

## 8. Data-fix / one-off scripts

| Script | Intent | What it changes | What it reveals (defects/anomalies) |
|---|---|---|---|
| `Create_Order_ODR-20260901134.sql` (104 lines; comments in Bengali) | Recreate or re-open a specific web-manual order so it can be invoiced (transfer to outlet "Akij Pharmacy - Banani") | In one `BEGIN TRANSACTION`: if `tblOrder.OrderCode='ODR-20260901134'` exists -> UPDATE `IsInvoice=0, IsPrepareforInvoice=1, ActionStatus='2', GrossValue=322550.00, TotalNetPayable=322550.00` and `tblOrderDetail.[Status]=NULL`; else INSERT the header (ComUnit 12 'BD02' 'Dhaka Central Warehouse', MIO 'EMP000055' 'Office', customer 'C0000134' / CustomerMasterId 134, GrossValue 322550.00, `OrderSenderType='Web Manual'`, `OrderType='Regular'`, `IsManual=1`, `SubmissionDate 2026-09-01`, `Remarks='Transfer from Akij outlate Banani'`) and 14 detail lines (ProductIds 34-55; blood-pressure monitors, cuffs, nebulizers, adapters, scale; every line UnitVat 0, discount 0; total 322,550.00, verified by summing the lines) | The order code `ODR-20260901134` is exactly the `ODR-yyyymmdd<MAX(OrderId)+1>` pattern of `sp_Save_ManualOrderMaster`, i.e. the order/its lines were lost (or an invoice was rolled back) and rebuilt by hand with hard-coded ids and prices - evidence of (1) non-atomic order save (header without details), (2) no rollback path other than SQL, (3) manual order pricing has no VAT (all `VAT 0`). The transaction has **no TRY/CATCH/XACT_ABORT**: a failure between INSERTs leaves an open transaction. The "exists" branch overwrites GrossValue/NetPayable with constants. |
| `SP_Fix_Invoice_Anomaly_And_Sync.sql` | Repair invoices whose header totals disagree with their detail totals | UPDATE `tblInvoice` SET `TpTotal=SUM(TotalPrice)`, `TpDiscount=SUM(DiscountAmount)`, `TpVat=SUM(TotalPriceVatAmount)`, `TpGrandTotal=SUM(NetAmount)`, `ReceivableAmount = SUM(NetAmount) - AdjustAmount` for invoices dated after `2022-11-16` where `ABS(TpGrandTotal - SUM(NetAmount)) > 10`; then ALTER `sp_Check_anomalyInvoiceDetails` to tolerate `AdjustAmount` | Known defect: invoice header total and detail sum diverge (details saved without header update or vice versa, possibly from the proforma edit/delete flow); the app already guards for it (`InvoiceCreationByOrder.aspx.cs:127`) and **auto-deletes** the proforma when detected. The repair is unconditional (no backup table, no transaction, no dry run); the `> 10` BDT tolerance and the 2022-11-16 cut-off are hard-coded (the cut-off suggests the defect began then); the UPDATE selects rows by `ABS(TpGrandTotal - SUM(NetAmount)) > 10` **without** `AdjustAmount`, whereas the updated checker compares `(TpGrandTotal - AdjustAmount)` with `SUM(NetAmount)`; for any invoice with a non-zero `AdjustAmount` the two rules disagree, so the sync can overwrite a legitimate header total (direction/sign of `AdjustAmount` is UNKNOWN). |
| `SP_Adjust_DCStore_Stock_Only.sql`, `sp_Check_DCStore_StockAnomalies.sql`, `sp_Delete_ProformaInvoice_Update.sql` | Repair and detect **stock inflation** after invoice rollbacks | See stored-procedures.md: deletes phantom Issue rows, resets Receive `StockQty` to `TotalQuantity`; proforma delete now deletes the Issue row instead of adding quantity back | Known defect: an earlier `sp_Delete_ProformaInvoice` **added the invoiced quantity back onto the Receive row while the negative Issue row stayed** (`SP_Adjust_DCStore_Stock_Only.sql:103`), producing Receive rows with `StockQty > TotalQuantity` (inflated stock) and orphan Issue rows whose invoice line was deleted. The data repair is a blanket reset (`<>`), risky where in-place decrements are legitimate. |
| `SP_Fix_STO_Chalan_Process.sql` (2026-09-02) + `SP_NewStockTransferDcToDc_Related.sql` (2026-09-01) | Fix B2B/STO challan issue/receive/delete | v2 `sp_Del_B2BDelete` deletes ledger rows; other SAP SPs (in-place model) unchanged | Known defect: deleting a B2B challan restored stock twice (in-place add plus remaining Issue row) and never removed destination Receive rows. The 2026-09-01 script still contains the old delete SP and the in-place-decrement SAP challan SP - the two scripts contradict each other. |
| `SP_Fix_CentralStoreStoIssue.sql` / `..._Corrected.sql` | Repair central-store STO picking posting (weighted-average and batch ledger) | Re-creates `tbl_INV_CentralStoreStoIssue` | Known defect: wrong ReceiveId -> Batch_Stock_Id mapping and duplicate/incorrect batch joins ("Fixes" list at `SP_Fix_CentralStoreStoIssue.sql:2-6`); v2 fixes idempotency and the batch/weighted-avg id semantics. **No script repairs the rows already written** by the defective versions - historical batch-wise stock for STO picking may be mis-attributed (UNKNOWN extent). |
| `SP_Update_StockValuation_TransactionDate_Logic.sql` | Change the stock reports to value issues at their transaction date | Re-creates 3 report SPs | Known defect (INFERRED from the change): reports used the ledger row's receipt/creation date for issues, so back-dated or late-posted issues fell in the wrong period. Requires `ReferenceTransactionDate` populated on Issue rows (NULL falls back to `StockRcvDate`). |
| `Live_Deployment_Script_All_Updates.sql`, `SP_SalesReturn_Related.sql` | Customer payment (discount/AIT/delivery charge), AR ageing, delivery & market sales-return posting with delivery-charge deduction | DDL items 5-11 + 7 SPs | Known gaps: return-posting SPs use different GL flags and reference types; `IsPosting` for `tblCustomerReturnMaster` is not created; delivery-return posting is not idempotent; the credit-check SP joins return vouchers on the wrong key. |
| `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql` | Multi-person tagging for customers/doctors; manual-order SPs with credit-limit check; widen return staging columns | DDL items 1-4 + 13 SPs | `TaggedSalesPersonCodes` column not created; customer/doctor codes MAX+1. |
| `SP_Create_tblUnitPriceRollback.sql`, `SP_Fix_CustomerPayment_Live.sql`, `Library.DAL/SAP_IntegrationDAL/sql/*.sql` | Small schema additions | items 12-14 | - |

## 9. Consolidated known-defect list from the scripts

1. Stock ledger integrity: two stock models (ledger vs in-place), inflated Receive rows, orphan Issue rows, blanket realignment SP.
2. Non-atomic multi-step writers without transactions (proforma delete, posting SPs, customer/doctor updates, SAP challan SPs).
3. MAX+1 key/number generation for codes, ids and voucher numbers (race conditions; predictable `ODR-` numbers).
4. Header/detail total divergence on invoices (repaired by script; root cause not visible).
5. Accounts posting: hard-coded EntryBy/GL/FY fallbacks; `TableID 2018` key collision; delivery-return posting not idempotent; credit-limit due join on the wrong voucher key.
6. Orphaned/undefined columns and objects assumed by scripts (section 1 caveats), cross-database name inconsistency (`CSTL_ERPDB` vs `CSTL_ERPDB_Unicorn`), and SPs with no C# caller (`sp_UD_CustomerMaster`, SAP challan SPs, audit/repair tools) - callers are outside the repo or manual.

<!-- GENERATED-INDEX-BEGIN: tables -->
## Generated table index (inline-SQL evidence)

Columns below are **observed** in `INSERT INTO t (cols)` and `UPDATE t SET col=` statements in the C# code and in `SqlParameter` → column bindings. They are a **lower bound** of the real schema (no DDL exists in the repo). Data types, PK/FK, defaults and indexes are UNKNOWN unless stated in the "From SQL scripts" section above.

### CSTL_ACCDB_Unicorn

- Modules (pages reaching it via inline SQL): M06, M07, M09, M13
- Read by (11): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, CustPaymentDAL.LoadBankAccount, CustPaymentDAL.LoadCashAccount, InvoiceDAL.InvoiceMainDataForReport, InvoiceDAL.InvoiceMainDatainvNo, OrderInfoDAL.GetCustomerDueHistory …
- Written by (2): SalesOperationRollbackDAL.RollbackCustomerPayment, SalesOperationRollbackDAL.RollbackDeliveryInvoice
- Columns observed (0): 
- ALTERs in repo scripts: none

### CSTL_ERPDB_Unicorn

- Modules (pages reaching it via inline SQL): M10
- Read by (5): MaterialConversionReceiveDal.GetProductionUnit, MaterialConversionReceiveDal.LoadMaterialConversionList, MaterialConversionReceiveDal.LoadMaterialConversionListForEdit, MaterialConversionReceiveDal.LoadStockInDetailForEdit, MaterialConversionReceiveDal.LoadStockInMasterForEdit
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### GetmonthlycustomerSaleDate

- Modules (pages reaching it via inline SQL): M13
- Read by (1): TotalSummaryDAL.GetMonthYearWiseSale
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### InboundLots

- Modules (pages reaching it via inline SQL): M07
- Read by (2): RequisitionDAL.GetCurrentStockofCentralStock, RequisitionDAL.GetCurrentStockofCentralStockByBatch
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### OutboundSum

- Modules (pages reaching it via inline SQL): M07
- Read by (2): RequisitionDAL.GetCurrentStockofCentralStock, RequisitionDAL.GetCurrentStockofCentralStockByBatch
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### SalesDisDB_SMC_NEWDB

- Modules (pages reaching it via inline SQL): M02
- Read by (1): PanalClsDAL.FinancialYearList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### SystemTest_Testing

- Modules (pages reaching it via inline SQL): M06
- Read by (1): OrderStatusDal.LoadOrderStatusInfoTesting
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_BonusCampaignNewDetail

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (3): OrderInfoDAL.LoadCampaignDtls, OrderInfoDALSalesReturn.LoadCampaignDtls, SalesReturnNewDAL.LoadCampaignDtls
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_BonusCampaignNewMaster

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (3): OrderInfoDAL.LoadCampaignDtls, OrderInfoDALSalesReturn.LoadCampaignDtls, SalesReturnNewDAL.LoadCampaignDtls
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_DCRInfo

- Modules (pages reaching it via inline SQL): M16
- Read by (1): DoctorVisit_UI_DCRList.GetLatestDCRApprovalLog
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_Group

- Modules (pages reaching it via inline SQL): M13, M14
- Read by (4): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, ProuctWiseSalesDAL.LoadMonthlyTarget, SInventoryWebService.GetCustomer_ALL_ForDIC
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_INV_Central_BatchWiseStock

- Modules (pages reaching it via inline SQL): M07, M13
- Read by (3): RequisitionDAL.GetCurrentStockofCentralStock, RequisitionDAL.GetCurrentStockofCentralStockByBatch, RollBackDAL.DeleteStockInTransfer
- Written by (1): RollBackDAL.DeleteStockInTransfer
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_INV_CentralStore_WeightedAvg

- Modules (pages reaching it via inline SQL): M05, M06, M07, M11, M13
- Read by (7): DCStoreDAL.GetProductStock, ProductDAL.ProductPriceDetailWithCase, RequisitionDAL.GetCurrentStockofCentralStock, RequisitionDAL.GetCurrentStockofCentralStockByBatch, RequisitionDAL.GetDataInfoByIdBllDtls, RequisitionDAL.GetPickingDataForIssueDAL …
- Written by (1): RollBackDAL.DeleteStockInTransfer
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_MC_ProductRawMaterialDetails

- Modules (pages reaching it via inline SQL): M10
- Read by (2): MaterialConversionReceiveDal.LoadMaterialConversionList, MaterialConversionReceiveDal.LoadMaterialConversionListForEdit
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_PrescriptionType

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SetupDAL.GetPrescriptionTypelist_For_ddl
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_trainingRecordDetailsEmployee

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tbl_UserRoleInfo

- Modules (pages reaching it via inline SQL): M01, M02, M15
- Read by (3): ChangePasswordDAL.GetUserPassInfoDAL, PanalClsDAL.Login, RSMSetupDal.Get_UserListInfo
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblAction

- Modules (pages reaching it via inline SQL): M06
- Read by (2): ClsApprovalAction.LoadActionControlByUser, OrderListDAL.LoadApprovalControlDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblActionPageWiseStep

- Modules (pages reaching it via inline SQL): M06
- Read by (3): ClsApprovalAction.LoadActionControlByUser, ClsApprovalAction.LoadForApprovalByUserCondition, OrderListDAL.LoadApprovalControlDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblActionStepDetail

- Modules (pages reaching it via inline SQL): M06
- Read by (3): ClsApprovalAction.LoadActionControlByUser, ClsApprovalAction.LoadForApprovalByUserCondition, OrderListDAL.LoadApprovalControlDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblActionSteps

- Modules (pages reaching it via inline SQL): M06
- Read by (3): ClsApprovalAction.LoadActionControlByUser, ClsApprovalAction.LoadForApprovalByUserCondition, OrderListDAL.LoadApprovalControlDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblActionUserWiseApproval

- Modules (pages reaching it via inline SQL): M06
- Read by (3): ClsApprovalAction.LoadActionControlByUser, ClsApprovalAction.LoadForApprovalByUserCondition, OrderListDAL.LoadApprovalControlDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblAddressDivision

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblAdjustmentType

- Modules (pages reaching it via inline SQL): M10
- Read by (5): CustomerCategoryDAL.LoadAdjustment, WHStockAdjDAL.GetWhStockAdjustmentListReportInformation, WHStockAdjDAL.LoadAdjustmentType, WHStockAdjDAL.LoadWHStockAdjustment, WHStockAdjDAL.LoadWHStockAdjustmentById
- Written by (1): CustomerCategoryDAL.SaveAdjustment
- Columns observed (1): AdjustmentType
- ALTERs in repo scripts: none

### tblAppSetup

- Modules (pages reaching it via inline SQL): M02, M10, M12, M18
- Read by (5): AppSetupDAL.DeleteAreaInfo, AppSetupDAL.LoadAppSetup, DeStockOutDal.GetAssignedAppUser, SubDepotStockAdjustmentsVoucherDal.GetAssignedAppUser, WarehouseStockInApprovalDal.GetAssignedAppUser
- Written by (3): AppSetupDAL.DeleteAreaInfo, AppSetupDAL.SaveAppSetup, AppSetupDAL.UpdateAreaInfo
- Columns observed (5): Email, EntryBy, EntryDate, SL, UserId
- ALTERs in repo scripts: none

### tblArcDBConnect

- Modules (pages reaching it via inline SQL): M02
- Read by (2): ArchiveDbConnectRepository.EntryExists, ArchiveDbConnectRepository.GetRecentEntries
- Written by (1): ArchiveDbConnectRepository.InsertEntry
- Columns observed (2): DataBaseName, FY
- ALTERs in repo scripts: none

### tblArea

- Modules (pages reaching it via inline SQL): M03, M05, M06, M07, M08, M09, M13, M14, M15, M17
- Read by (48): AreaDAL.AreaEditLoad, AreaDAL.HasAreaName, AreaDAL.LoadAreaView, AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, CompanySalesReportDAL.SalesReportDAlParameter …
- Written by (2): AreaDAL.SaveAreaInfo, AreaDAL.UpdateAreaInfo
- Columns observed (3): AreaCode, AreaId, AreaName
- ALTERs in repo scripts: none

### tblarea

- Modules (pages reaching it via inline SQL): M09
- Read by (1): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblBankInfo

- Modules (pages reaching it via inline SQL): M09
- Read by (2): CompanywisebranchDal.LoadDepositList, SInventory_UI_MoneyReceiptPreview.FetchDataFromDatabase
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblBankInfoNew

- Modules (pages reaching it via inline SQL): M03
- Read by (1): CompanywisebranchDal.LoadBank
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblBoardUniversity

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpEducationInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblBranchInfo

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): CompanywisebranchDal.LoadBranch
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblCentralStore

- Modules (pages reaching it via inline SQL): M04, M07, M10, M11, M12, M13, M17
- Read by (42): CentralStoreDAL.LoadStockReceiveView, CentralStoreDAL.StockReceiveEditLoad, ChalanDAL.CentralStoreQuantity, ChalanDAL.ChalanReport, ChalanDAL.DCStoreReport, ChalanDAL.GetProductWhStore …
- Written by (14): CentralStoreDAL.SaveDataForStockReceive, CentralStoreDAL.UpdateStockReceive, ChallanReportViewDAL.UpdateCentralStockStockOut, RequisitionDAL.UpdateCentralStockStockOut, RollBackDAL.UpdateCentralStockStockOut, SampleStockForWHDal.StockInCentral …
- Columns observed (24): BatchNo, ChalanDate, ChalanNo, DCStoreFreezeId, DCStoreId, ExpDate, InternalNoteNo, MfgDate, MigoDetailID, PackSize, ProductCode, ProductId, ProductName, ProductStockType, Quantity, ReceiveDate, ReceiveId, StockCondition, StockInQty, TotalAmount, TotalPrice, TotalVAT, UnitPrice, VATPerUnit
- ALTERs in repo scripts: none

### tblCentralStore_OpeninigBalance

- Modules (pages reaching it via inline SQL): M10
- Read by (3): WhOpeningStockReportDal.GetWhOpeningStockReport, WhStockMonitoringReportDal.GetWhStokMonitoringInformation, WhStockMonitoringReportDal.GetWhStokMonitoringReportInformation
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblChalanDetail

- Modules (pages reaching it via inline SQL): M10, M11
- Read by (19): ChalanDAL.ChalanDetailEditLoad, ChalanDAL.ChalanReport, ChalanDAL.DCStockInDALMain, ChalanDAL.DCStoreReport, ChalanDAL.GetChalanReceieve, ChalanDAL.UpdateDCStockQuantity …
- Written by (2): ChalanDAL.SaveDataForChalanDetail, ChalanDAL.UpdateaChalanDetail
- Columns observed (11): BatchNo, ChalanDetailsId, ChalanId, DCStoreId, ProductCode, ProductName, Quantity, UnitPrice, Value, ValueWVat, Vat
- ALTERs in repo scripts: none

### tblChalanInfo

- Modules (pages reaching it via inline SQL): M10, M11
- Read by (21): B2BTransferViewDal.DeleteChallanMasterById, B2BTransferViewDal.GetB2bTransferInfo, ChalanDAL.ChalanEditLoad, ChalanDAL.ChalanLoadInReceive, ChalanDAL.ChalanReport, ChalanDAL.DCStoreReport …
- Written by (10): B2BTransferViewDal.DeleteChallanMasterById, ChalanDAL.ChalanUpdate, ChalanDAL.SaveDataForChalanInfo, ChalanDAL.UpdateaChalanInfo, SubDepoAdjustDAL.UpdateaChalanDetail, SubDepoAdjustDAL.UpdateaChalanInfo …
- Columns observed (18): ChalanDate, ChalanId, ChalanNo, DriverName, FromComUnitAddress, FromComUnitCode, FromComUnitId, FromComUnitName, GrandTotal, IsDeliver, ManufacId, ProductCode, ToComUnitAddress, ToComUnitCode, ToComUnitName, TotalValue, TotalVat, TrackNo
- ALTERs in repo scripts: none

### tblCompanyInfo

- Modules (pages reaching it via inline SQL): M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M13, M15, M17
- Read by (32): AreaDAL.AreaEditLoad, AreaDAL.LoadAreaView, BatchWiseCollectionReportDal.AitInformationById, BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId, BatchWiseCollectionReportDal.LoadNonTranscationalInvoice, BatchWiseCollectionReportDal.NTInvoiceReturnReport …
- Written by (2): CompanyInfoDAL.SaveDataForCompanyInfo, CompanyInfoDAL.UpdateCompanyInfo
- Columns observed (7): Address, CompanyCode, CompanyId, CompanyName, ContactNo, FaxNo, Remarks
- ALTERs in repo scripts: none

### tblCompanyUnit

- Modules (pages reaching it via inline SQL): M02, M03, M05, M06, M07, M08, M09, M10, M11, M12, M13, M15, M17, M18
- Read by (227): AreaDAL.AreaEditLoad, AreaDAL.LoadAreaView, AreaWiseMonthlyInventoryReportDal.GetBranchList, AuditReportOneDal.GetSalesCenter, BatchWiseCollectionReportDal.AitInformationById, BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId …
- Written by (2): CompanyUnitDAL.SaveDataForSalesCenter, CompanyUnitDAL.UpdateSalesCenter
- Columns observed (7): Address, ComUnitCode, ComUnitId, ComUnitName, FaxNo, MobileNo, PhoneNo
- ALTERs in repo scripts: none

### tblCompanyWiseDeposit

- Modules (pages reaching it via inline SQL): M09
- Read by (1): CompanywisebranchDal.LoadDepositList
- Written by (1): CompanywisebranchDal.UpdateDepositInfo
- Columns observed (3): DeleteBy, DeleteDate, IsDelete
- ALTERs in repo scripts: none

### tblCountry

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblCurrentStock

- Modules (pages reaching it via inline SQL): M10, M11
- Read by (7): CentralStoreDAL.CurrentStockQty, CentralStoreDAL.CurrentStockReport, CentralStoreDAL.HasProductName, CentralStoreDAL.StockCheck, DCStoreDAL.CurrentStockQty, DCStoreDAL.HasProductcode …
- Written by (4): CentralStoreDAL.SaveDataForCurrentStock, CentralStoreDAL.UpdateCurrentStockQuantity, DCStoreDAL.SaveDataForCurrentStock, DCStoreDAL.UpdateCurrentStockQuantity
- Columns observed (7): ComUnitCode, ComUnitId, PackSize, ProductCode, ProductName, Quantity, StockId
- ALTERs in repo scripts: none

### tblCustCategory

- Modules (pages reaching it via inline SQL): M05, M07, M08, M12
- Read by (30): CustomerCategoryDAL.CustomerCategoryEditLoad, CustomerCategoryDAL.HasCustCategoryName, CustomerCategoryDAL.LoadCustCategoryView, CustomerMasterDAL.LoadCategoryName, CustomerMasterDAL.LoadCategoryNameById, CustomerMasterInfoDal.LoadCategoryName …
- Written by (2): CustomerCategoryDAL.SaveCustomerCategory, CustomerCategoryDAL.UpdateCustCategoryInfo
- Columns observed (3): CategoryCode, CategoryId, CategoryName
- ALTERs in repo scripts: none

### tblCustMaster

- Modules (pages reaching it via inline SQL): M04, M05, M06, M07, M08, M09, M10, M12, M13, M17
- Read by (138): AuditReportOneDal.GetCustomer, AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, BatchWiseCollectionReportDal.AitInformationById, BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId …
- Written by (4): CustomerMasterDAL.SaveCustometMasterInfo, CustomerMasterInfoDal.SaveCustometMasterInfo, ImportedApiCustomerDal.SaveApiCustomerInformation, MultiCustomerEditDAL.UpdateDataForCustomer
- Columns observed (25): Addrees2, Address, AreaCode, CategoryId, CellNo, City, ComUnitCode, ComUnitName, ConPerson, CustomerCode, CustomerCodeOld, CustomerName, DZSMName, DisCode, FEName, FixedCustomer, InActiveDate, IsActive, MIACode, MIAName, MarketCode, MarketName, RegionCode, ShippingCond, TermOfPayment
- ALTERs in repo scripts: ADD TaggedSalesPersonIds NVARCHAR(MAX) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:9)

### tblCustomerCreditLimit

- Modules (pages reaching it via inline SQL): M05, M07
- Read by (7): CreditLimitDAL.CheckExistorNot, CreditLimitDAL.CheckExistorNotUpdate, CreditLimitDAL.GetCustomerCreditInfo, CreditLimitDAL.GetCustomerCreditInfoById, CreditLimitDAL.LoadCustomerCreditLimitApproval, InvoiceDAL.InvoiceMainDataForReport …
- Written by (4): CreditLimitDAL.ApprovalUpdateDAL, CreditLimitDAL.DeleteCreditLimit, CreditLimitDAL.SaveCreditLimit, CreditLimitDAL.UpdateCreditLimit
- Columns observed (10): ActionStatus, ApprovedBy, ApprovedDate, CompanyId, CustomerMasterId, DayLimit, EntryBy, EntryDate, IsActive, LimitAmount
- ALTERs in repo scripts: none

### tblCustomerMasterExcelFileDetail

- Modules (pages reaching it via inline SQL): M05
- Read by (3): ExcelUpForMIGODAL.DeleteCustomerDetailData, ExcelUpForMIGODAL.GetVerificationCount, ExcelUpForMIGODAL.GetVerificationReport
- Written by (1): ExcelUpForMIGODAL.DeleteCustomerDetailData
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblCustomerMasterExcelFileMaster

- Modules (pages reaching it via inline SQL): M05
- Read by (2): ExcelUpForMIGODAL.DeleteCustomerData, ExcelUpForMIGODAL.LoadCustomer
- Written by (2): ExcelUpForMIGODAL.DeleteCustomerData, ExcelUpForMIGODAL.SaveCustomerUploadMasterDAL
- Columns observed (8): CustomerMasterExcelFileCode, CustomerMasterExcelFileDocumentDate, CustomerMasterExcelFileMasterID, EntryBy, EntryDate, ManufacId, Transfer, VerifyedAll
- ALTERs in repo scripts: none

### tblCustomerMasterTagChangeExcelFileDetail

- Modules (pages reaching it via inline SQL): M05
- Read by (3): ExcelUpForCustTagChangeDAL.DeleteCustomerDetailData, ExcelUpForCustTagChangeDAL.GetVerificationCount, ExcelUpForCustTagChangeDAL.GetVerificationReport
- Written by (2): ExcelUpForCustTagChangeDAL.CustomerXLDataGridToDbByRow, ExcelUpForCustTagChangeDAL.DeleteCustomerDetailData
- Columns observed (22): ADDRESS1, ADDRESS2, BRANCH, BRANCHDES, CITY, CONTACTNUMBER, CONTACTPERSON, CUSTOMERNAME, CustomerCode, DZSMCode, DZSMName, FECode, FEName, MIOCode, MIOName, MarketName, MasterID, SHIPPINGCOND, SHIPPINGPOINT, TERMOFPAYMENT, TerritoryCode, Verifyed
- ALTERs in repo scripts: none

### tblCustomerMasterTagChangeExcelFileMaster

- Modules (pages reaching it via inline SQL): M05
- Read by (2): ExcelUpForCustTagChangeDAL.DeleteCustomerData, ExcelUpForCustTagChangeDAL.LoadCustomer
- Written by (2): ExcelUpForCustTagChangeDAL.DeleteCustomerData, ExcelUpForCustTagChangeDAL.SaveCustomerUploadMasterDAL
- Columns observed (8): CustomerTagChangeExcelFileCode, CustomerTagChangeExcelFileDocumentDate, CustomerTagChangeExcelFileMasterID, EntryBy, EntryDate, ManufacId, Transfer, VerifyedAll
- ALTERs in repo scripts: none

### tblCustomerPay

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M12, M13
- Read by (14): BatchWiseCollectionReportDal.AitInformationById, BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionById, BatchWiseCollectionReportDal.BatchWiseCollectionSheet, BatchWiseCollectionReportDal.BatchWiseCollectionSingle, BatchWiseCollectionReportDal.LoadDueInvoice …
- Written by (3): CustPaymentDAL.SaveCustPayment, PaymentReverseDal.DeletePaymentMaster, SalesOperationRollbackDAL.RollbackCustomerPayment
- Columns observed (12): ComUnitId, CreateBy, CreateDate, CustPayId, CustomerMasterId, MarketId, PayType, PaymentAmount, PaymentDate, RefDate, RefNo, Remarks
- ALTERs in repo scripts: ADD Remarks NVARCHAR(500) (SP_Fix_CustomerPayment_Live.sql:17)

### tblCustomerReturnDetail

- Modules (pages reaching it via inline SQL): M08
- Read by (3): CustomerReturnDAL.LoadInvoiceDataApproval, CustomerReturnDAL.LoadInvoiceNew, CustomerReturnDAL.LoadInvoiceNewConsumer
- Written by (1): CustomerReturnDAL.SaveCustomerReturnDetail
- Columns observed (4): CustReturnId, InvoiceDetailId, ReturnAmount, ReturnQty
- ALTERs in repo scripts: none

### tblCustomerReturnMaster

- Modules (pages reaching it via inline SQL): M08
- Read by (3): CustomerReturnDAL.LoadCustomerReturnApproval, CustomerReturnDAL.LoadInvoiceDataApproval, CustomerReturnDAL.LoadInvoiceNew
- Written by (2): CustomerReturnDAL.ApprovalUpdateDAL, CustomerReturnDAL.SaveCustomerReturn
- Columns observed (10): ActionStatus, ApprovedBy, ApprovedDate, DeductionCharge, EntryBy, EntryDate, InvoiceId, NetReturnAmount, TotalAmount, TotalQty
- ALTERs in repo scripts: ADD DeductionCharge DECIMAL(18, 2) (Live_Deployment_Script_All_Updates.sql:23); ADD NetReturnAmount DECIMAL(18, 2) (Live_Deployment_Script_All_Updates.sql:35)

### tblCustomertype

- Modules (pages reaching it via inline SQL): M07, M09
- Read by (3): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, InvoiceDAL.InvoiceMainDataForReport, SalesReturnDAL.InvoiceMainDataForReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblCustomerType

- Modules (pages reaching it via inline SQL): M07, M13
- Read by (3): CmnCrystaltoView.GetCustomerTypeListDAL, InvoiceDAL.InvoiceDetailDataForReportNew_, SalesReturnDAL.InvoiceDetailDataForReportNew_
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblCustPayDetail

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M12, M13
- Read by (26): BatchWiseCollectionReportDal.AitInformationById, BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionById, BatchWiseCollectionReportDal.BatchWiseCollectionSheet, BatchWiseCollectionReportDal.BatchWiseCollectionSingle, BatchWiseCollectionReportDal.LoadDueInvoice …
- Written by (5): BatchWiseCollectionReportDal.UpdateIsPrint, CustPaymentDAL.SaveCustDetail, CustPaymentDAL.SubdeportSaveCustDetail, PaymentReverseDal.DeletePaymentDetail, SalesOperationRollbackDAL.RollbackCustomerPayment
- Columns observed (15): AIT, BankAccId, CashAccId, CollectionBy, CustPayDetailId, CustPayId, DANameId, Discount, InvoiceId, IsPrint, PaymentAmount, SubDeportInvoiceId, TPAmount, VATAmount, custPaymentDate
- ALTERs in repo scripts: none

### tblDAInfo

- Modules (pages reaching it via inline SQL): M01, M02, M07, M08, M09, M15
- Read by (11): CmnCrystaltoView.GetDAListDAL, CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, CustomerReturnDAL.GetDANameById, InvoiceDAL.InvoiceDetailDataForReportNew_, InvoiceDAL.InvoiceMainDataForReport, InvoiceDAL.InvoiceMainDataForReport2 …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDCPicking

- Modules (pages reaching it via inline SQL): M07
- Read by (16): InvoiceDAL.AllPickingForReportList, InvoiceDAL.DCPickingReportDetailDataDAL, InvoiceDAL.DCPickingReportMainDataDAL, InvoiceDAL.DcPickingNoCount, ReturnInvoiceDAL.AllPickingForReportList, ReturnInvoiceDAL.DCPickingReportDetailDataDAL …
- Written by (4): InvoiceDAL.DcPickingSaveDAL, ReturnInvoiceDAL.DcPickingSaveDAL, SalesReturnDAL.DcPickingSaveDAL, Sub_InvoiceDAL.DcPickingSaveDAL
- Columns observed (5): AreaId, ComUnitId, DCPicDate, DCPicId, DCPicNo
- ALTERs in repo scripts: none

### tblDCPickingDetail

- Modules (pages reaching it via inline SQL): M07
- Read by (8): InvoiceDAL.DCPickingReportDetailDataDAL, InvoiceDAL.InvoiceForDCPickingDAL, ReturnInvoiceDAL.DCPickingReportDetailDataDAL, ReturnInvoiceDAL.InvoiceForDCPickingDAL, SalesReturnDAL.DCPickingReportDetailDataDAL, SalesReturnDAL.InvoiceForDCPickingDAL …
- Written by (4): InvoiceDAL.DcPickingDetailSaveDAL, ReturnInvoiceDAL.DcPickingDetailSaveDAL, SalesReturnDAL.DcPickingDetailSaveDAL, Sub_InvoiceDAL.DcPickingDetailSaveDAL
- Columns observed (3): DCPicDetailId, DCPicId, InvoiceNo
- ALTERs in repo scripts: none

### tblDCRApprovalLog

- Modules (pages reaching it via inline SQL): M16
- Read by (1): DoctorVisit_UI_DCRList.GetLatestDCRApprovalLog
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDCStock

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (12): ChalanDAL.HasProductcode, InvoiceDAL.DCStockQuantity, InvoiceDAL.HasProductcode, ReturnInvoiceDAL.DCStockQuantity, ReturnInvoiceDAL.HasProductcode, SalesReturnDAL.DCStockQuantity …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDCStore

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M10, M11, M12, M13
- Read by (77): ChalanDAL.ChalanReport, ChalanDAL.DCInfoWithDCId, ChalanDAL.DCStockInDALMain, ChalanDAL.DCStoreReport, ChalanDAL.GetChalanReceieve, ChalanDAL.GetProductDcStore …
- Written by (26): ChalanDAL.DCStockInDALMain, ChalanDAL.DCStockInDALMainSub, ChalanDAL.UpdateDCStockQuantity, ChallanReportViewDAL.DCStockInDAL, CustomerReturnDAL.InsertDCStoreStockReturn, CustomerReturnDAL.UpdateDCStoreQty …
- Columns observed (32): BatchNo, ChalanDate, ChalanDetailsId, ChalanNo, ComUnitId, CostPerUnit, DCStoreId, DamageQty, ExpDate, MfgDate, PackSize, ProductCode, ProductName, Quantity, ReceiveDate, ReceiveId, ReferenceDCStoreId, ReferenceTransactionDate, ReqChildId, ReqId, ReturnFromInvoiceDetailId, ReturnInvoiceDetailId, SChalanDetailsId, SalesReturnDetailId, StockCondition, StockInTransfarId, StockQty, StockRcvDate, StorageLocation, TotalCostAmount, TotalQuantity, Trans_Type
- ALTERs in repo scripts: ADD ReturnInvoiceDetailId INT (Live_Deployment_Script_All_Updates.sql:85); ADD ReturnInvoiceDetailId INT (Live_Deployment_Script_All_Updates.sql:847)

### tblDCStore_OpeningBalance

- Modules (pages reaching it via inline SQL): M10
- Read by (2): DcOpeningStockReportDal.GetAllDcOpeningStockReport, DcOpeningStockReportDal.GetDcOpeningStockReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDCStoreFreeze

- Modules (pages reaching it via inline SQL): M08, M10, M11, M12, M13
- Read by (10): DCStockReportDAL.DCReportDetailDataDAL, OtherStockActionDAL.LoadData, OtherStockActionDAL.LoadStockData, ProformaOrInvoiceReturn.LoadStock, ReturnInvoiceDAL.DeleteData, SCtoWHTransferDal.DcFreezeInfoId …
- Written by (17): ChalanDAL.DCStockInDAL, ChalanDAL.SaveDCStoreFreeze2, ChallanReportViewDAL.SaveDCStoreFreeze2, InvoiceDAL.DCStockInDAL, OtherStockActionDAL.UpdateDCStoreFreezeStock, RequisitionDAL.SaveDCStoreFreeze2 …
- Columns observed (26): BatchNo, ChalanDate, ChalanDetailsId, ChalanNo, ComUnitId, DCStoreFreezeId, DCStoreId, DamageQty, ExpDate, InvoiceDetailId, PackSize, ProductCode, ProductName, ReceiveDate, ReceiveId, Remarks, ReqChildId, ReqId, ReturnInvoiceDetailId, StockCondition, StockConditionFreezeID, StockInTransfarId, StockQty, StockRcvDate, StorageLocation, TotalQuantity
- ALTERs in repo scripts: none

### tblDCStoreTransaction

- Modules (pages reaching it via inline SQL): M11
- Read by (1): RequisitionDAL.DCStockInTransDAL
- Written by (1): RequisitionDAL.DCStockInTransDAL
- Columns observed (5): DCStoreId, Date, Id, Quantity, Type
- ALTERs in repo scripts: none

### tblDeliveryManInfo

- Modules (pages reaching it via inline SQL): M09
- Read by (3): BatchWiseCollectionReportDal.BatchWiseCollectionById, BatchWiseCollectionReportDal.BatchWiseCollectionSheet, BatchWiseCollectionReportDal.LoadDeliveryMan
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDepartment

- Modules (pages reaching it via inline SQL): M01, M03, M15
- Read by (5): ChangePasswordDAL.GetEmployeeInfoDAL, DepartmentDAL.DepartmentEditLoad, DepartmentDAL.HasDeptName, DepartmentDAL.LoadDepartment, EmpGeneralInfoDAL.LoadDepartmentName
- Written by (2): DepartmentDAL.SaveDepartmentInfo, DepartmentDAL.UpdateDepartmentInfo
- Columns observed (3): DeptCode, DeptId, DeptName
- ALTERs in repo scripts: none

### tblDepotToWHChalanDetail

- Modules (pages reaching it via inline SQL): M11
- Read by (3): SCtoWHTransferDal.ChalanReport, SCtoWHTransferDal.LoadChallanDetailById, StockRcvByWhDal.GetChallanDetailByReqId
- Written by (1): SCtoWHTransferDal.SaveDataForChalanDetail
- Columns observed (13): BatchNo, DCStoreFreezeId, DCStoreId, ProductCode, ProductName, PurposeId, Quantity, SChalanDetailsId, SChalanId, UnitPrice, Value, ValueWVat, Vat
- ALTERs in repo scripts: none

### tblDepotToWHChalanInfo

- Modules (pages reaching it via inline SQL): M11
- Read by (6): SCtoWHTransferDal.ChalanReport, SCtoWHTransferDal.CheckChalanStatus, SCtoWHTransferDal.GetWarehouseRcvStock, SCtoWHTransferDal.LoadDepoToWHTransferInfo, StockRcvByWhDal.GetChallanDetailByReqId, StockRcvByWhDal.LoadMasterInfo
- Written by (3): SCtoWHTransferDal.SaveDataForChalanInfo, SCtoWHTransferDal.UpdateChalanMasterStatus, StockRcvByWhDal.updateChallanStatus
- Columns observed (17): ChalanDate, ChalanNo, DriverName, FromComUnitAddress, FromComUnitCode, FromComUnitName, GrandTotal, IsDeliver, IsTransfer, ManufacId, SChalanId, TotalValue, TotalVat, TrackNo, WHAddress, WHCode, WHName
- ALTERs in repo scripts: none

### tblDesignation

- Modules (pages reaching it via inline SQL): M01, M15
- Read by (5): ChangePasswordDAL.GetEmployeeInfoDAL, ChangePasswordDAL.GetUserPassInfoDAL, EmpGeneralInfoDAL.LoadDesignationName, PanalClsDAL.Login, SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDesignationType

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDeStockOutDetails

- Modules (pages reaching it via inline SQL): M10
- Read by (4): DeStockOutDal.DcStockOutDetailsDeleteDal, DeStockOutDal.DcStockOutReportDal, DeStockOutDal.GetDcStoreIdDal, SubDepotStockAdjustmentsVoucherDal.GetDcStoreIdDal
- Written by (2): DeStockOutDal.DcStockOutDetailsDeleteDal, DeStockOutDal.SaveDataForStockOutDetailDal
- Columns observed (10): BatchNo, DCStoreId, DcStockOutDetailsId, DcStockOutMasterId, ExpDate, PackSize, ProductCode, ProductName, ReceiveDate, StackOutQty
- ALTERs in repo scripts: none

### tblDeStockOutMaster

- Modules (pages reaching it via inline SQL): M10, M12, M18
- Read by (6): DeStockOutDal.DcStockOutAppDal, DeStockOutDal.DcStockOutMasterDeleteDal, DeStockOutDal.DcStockOutReportDal, DeStockOutDal.DcStockOutViewDal, DeStockOutDal.GetDcStoreIdDal, SubDepotStockAdjustmentsVoucherDal.GetDcStoreIdDal
- Written by (4): DeStockOutDal.ApprovalUpdateDal, DeStockOutDal.DcStockOutMasterDeleteDal, DeStockOutDal.SaveDataForDcStockOutMaster, SubDepotStockAdjustmentsVoucherDal.ApprovalUpdateDal
- Columns observed (11): ApprovedBy, ApprovedDate, ComUnitId, CustomerCode, DcStockOutMasterId, EntryBy, EntryDate, InvoiceId, Reason, Status, StockOutDate
- ALTERs in repo scripts: none

### tblDistributionRoute

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (3): OrderInfoDAL.LoadDistributionRoute, OrderInfoDALSalesReturn.LoadDistributionRoute, SalesReturnNewDAL.LoadDistributionRoute
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDistrict

- Modules (pages reaching it via inline SQL): M01, M03, M05, M07, M13, M15, M17
- Read by (22): AreaDAL.AreaEditLoad, AreaDAL.LoadAreaView, AreaDAL.LoadDistrictName, ChangePasswordDAL.GetEmployeeInfoDAL, CompanySalesReportDAL.SalesReportDAlParameter, CustomerMasterDAL.LoadDistrictName …
- Written by (2): DistrictDAL.SaveDistrictInfo, DistrictDAL.UpdateDistrictInfo
- Columns observed (3): DistrictCode, DistrictId, DistrictName
- ALTERs in repo scripts: none

### tblDivision

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDivisionWing

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDoctorDegree

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDoctorDegreeDetail

- Modules (pages reaching it via inline SQL): M16
- Read by (2): MasterSetup_UI_DoctorView.Parm, SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDoctorMaster

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (2): SInventoryWebService.GetDoctor_ALL, SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: ADD TaggedSalesPersonIds NVARCHAR(MAX) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:16)

### tblDoctorProgramType

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDoctorProgramTypeDetail

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDoctorSpeciality

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblDoctorSpecialityDetail

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SetupDAL.Get_DoctorList
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEducationalInstitution

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpEducationInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEducationName

- Modules (pages reaching it via inline SQL): M01
- Read by (2): ChangePasswordDAL.GetEmpEducationInfoDAL, ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEducationSubjectGroup

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpEducationInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpCategory

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpChildren

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpChildrenInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpEducation

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpEducationInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpExperience

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpExperienceInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpGeneralInfo

- Modules (pages reaching it via inline SQL): M01, M02, M04, M06, M07, M08, M09, M13, M15, M16
- Read by (35): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId, BatchWiseCollectionReportDal.BatchWiseCollectionById, BatchWiseCollectionReportDal.BatchWiseCollectionSheet, BatchWiseCollectionReportDal.LoadDeliveryMan …
- Written by (2): EmpGeneralInfoDAL.SaveEmployeeInfo, EmpGeneralInfoDAL.UpdateEmployeeInfo
- Columns observed (30): AddressPermanent, AddressPresent, BloodGroup, CellNumber, DateOfBirth, DepartmentId, DeptName, Designation, DesignationId, Email, EmpImage, EmpInfoId, EmpMasterCode, EmpName, FatherName, Gender, JoiningDate, MaritalStatus, MedicalInformation, MotherName, NationalIdNo, Nationality, PhoneNo, PlaceOfBirth, RefAddress, RefCellNo, RefName, Religion, ShortName, SignatureImage
- ALTERs in repo scripts: none

### tblEmpInfoTrainingType

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmployeeType

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpReference

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpReferenceInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpTraining

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpTrainingInstitution

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblEmpwiseProductLineDeclare

- Modules (pages reaching it via inline SQL): M15
- Read by (2): EmployeeInformationDaL.GetEmployeeProductLineIds, EmployeeInformationDaL.SyncEmployeeProductLineDeclareInternal
- Written by (1): EmployeeInformationDaL.SyncEmployeeProductLineDeclareInternal
- Columns observed (2): EmpInfoId, ProductLineID
- ALTERs in repo scripts: none

### tblEmpwiseShiftDeclare

- Modules (pages reaching it via inline SQL): M15
- Read by (2): EmployeeInformationDaL.GetEmployeeShiftIds, EmployeeInformationDaL.SyncEmployeeShiftDeclareInternal
- Written by (1): EmployeeInformationDaL.SyncEmployeeShiftDeclareInternal
- Columns observed (2): EmpInfoId, ShiftId
- ALTERs in repo scripts: none

### tblEvaluateTrainingMaster

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblFinancialYear

- Modules (pages reaching it via inline SQL): M03
- Read by (1): UserInfoDAL.GetArchiveFinancialYearById
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblFiscalYearInfos

- Modules (pages reaching it via inline SQL): M15
- Read by (1): HolidayDal.GetFinanCialyear
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblFocDetails

- Modules (pages reaching it via inline SQL): M06, M08
- Read by (4): InvoiceDAL.ProductFocBonusQtyDAL, ReturnInvoiceDAL.ProductFocBonusQtyDAL, SalesReturnDAL.ProductFocBonusQtyDAL, Sub_InvoiceDAL.ProductFocBonusQtyDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblFocMaster

- Modules (pages reaching it via inline SQL): M06, M08
- Read by (4): InvoiceDAL.ProductFocBonusQtyDAL, ReturnInvoiceDAL.ProductFocBonusQtyDAL, SalesReturnDAL.ProductFocBonusQtyDAL, Sub_InvoiceDAL.ProductFocBonusQtyDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblGenericGroup

- Modules (pages reaching it via inline SQL): M04
- Read by (3): ProductDAL.LoadGenericGroup, ProductDAL.LoadProduct, ProductDAL.LoadProductSearch
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblGroupWisePromoQty

- Modules (pages reaching it via inline SQL): M04
- Read by (2): GroupWisePromoQtyDAL.LoadGroupWiseQty, GroupWisePromoQtyDAL.LoadGroupWiseQtyById
- Written by (2): GroupWisePromoQtyDAL.SaveGroupWiseQty, GroupWisePromoQtyDAL.UpdateGroupWiseQty
- Columns observed (8): Date, EntryBy, EntryDate, Month, ProductId, PromoGroupId, Qty, Year
- ALTERs in repo scripts: none

### tblIngridents

- Modules (pages reaching it via inline SQL): M04
- Read by (6): IngridentsDAL.HasIngridentsName, IngridentsDAL.IngridentsEditLoad, IngridentsDAL.LoadIngridents, ProductDAL.LoadIngrident, ProductSQDAL.LoadIngrident, ProductSQDAL.LoadProductSQ
- Written by (2): IngridentsDAL.SaveIngridents, IngridentsDAL.UpdateIngridentsInfo
- Columns observed (3): IngridentsId, IngridentsName, IngridentsType
- ALTERs in repo scripts: none

### tblInvoice

- Modules (pages reaching it via inline SQL): M05, M06, M07, M08, M09, M10, M11, M12, M13, M17, M18
- Read by (224): BatchWiseCollectionReportDal.AitInformationById, BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId, BatchWiseCollectionReportDal.BatchWiseCollectionById, BatchWiseCollectionReportDal.BatchWiseCollectionSheet, BatchWiseCollectionReportDal.BatchWiseCollectionSingle …
- Written by (14): CustPaymentDAL.UpdateInvoicePaymentAmount, InvoiceDAL.DeleteInvoice, InvoiceDAL.PaymentUpdateInvoice, InvoiceDAL.SaveDataForInvoice, InvoiceDAL.UpdateInvoice, InvoiceDAL.UpdateInvoiceHeaderTotals …
- Columns observed (70): AdjustAmount, AdjustInvoiceNo_ReturnInvoiceNo, AreaCode, ComUnitId, CreateDate, CustomerDeliveryAddress, CustomerMasterId, CustomerType, DZSMName, DelivaryInvoiceNo, DelivarySpecialAmount, DeliveryCharge, DeliveryDate, DeliveryInvoiceCreateDate, DeliveryInvoiceStatus, DeliveryPersonName, DeliveryPersonPhNo, DeliveryTpDiscount, DeliveryTpGrandTotal, DeliveryTpTotal, DeliveryTpVat, DisCode, FEName, FixedCustomer, Inv_DANameId, InvoiceDate, InvoiceNo, IsAdjustInvoice, MIACode, MIAName, MarketCode, MarketName, MiaId, OldTradePolicy, OrderDate, OrderId, OrderNo, PaymentAmount, PaymentBy, PaymentDate, PaymentInvoiceNo, PaymentInvoiceStatus, PaymentStatus, PaymentTpDiscount, PaymentTpGrandTotal, PaymentTpTotal, PaymentTpVat, PaymentTypeId, ProductOffer, ReceivableAmount, RegionCode, Remarks, SndReturnInvoiceNo, SndReturnPaymentBy, SndReturnPaymentDate, TotalSpecialAmount, TpDiscount, TpGrandTotal, TpTotal, TpVat …
- ALTERs in repo scripts: none

### tblInvoiceBatch

- Modules (pages reaching it via inline SQL): M06, M07, M09
- Read by (11): BatchWiseCollectionReportDal.BatchWiseCollection, BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId, BatchWiseCollectionReportDal.BatchWiseCollectionById, BatchWiseCollectionReportDal.BatchWiseCollectionSheet, BatchWiseCollectionReportDal.BatchWiseCollectionSingle, OrderInfoDAL.GetInvoNOGetByBatchNo …
- Written by (2): InvoiceDAL.SaveDataForInvoiceBatch, SalesReturnDAL.SaveDataForInvoiceBatch
- Columns observed (3): BatchNo, Date, InvoiceId
- ALTERs in repo scripts: none

### tblInvoiceDeleteLog

- Modules (pages reaching it via inline SQL): M07, M13
- Read by (2): DeleteInvoiceReportDAL.GetDeleteOrderNationalReport, DeleteInvoiceReportDAL.GetDeleteOrderReport
- Written by (2): SalesOperationRollbackDAL.RollbackDeliveryInvoice, SalesOperationRollbackDAL.RollbackProforma
- Columns observed (7): DelInvoiceNo, DeleteBy, DeleteDateTime, DeliveryDeleteBy, DeliveryDeleteDateTime, InvoiceId, InvoiceNo
- ALTERs in repo scripts: none

### tblInvoiceDetail

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M10, M11, M12, M13
- Read by (122): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, CompanySalesReportDAL.DeliveryReturnReportDAl, CompanySalesReportDAL.SalesReportDAl, CompanySalesReportDAL.SalesReportDAlParameter, CustPaymentDAL.LoadInvoice, CustomerMasterInfoDal.GetFixedCustomerSalesReportInfo …
- Written by (10): InvoiceDAL.DeleteInvoice, InvoiceDAL.PaymentUpdateInvoiceDetail, InvoiceDAL.SaveDataForInvoiceDetails, InvoiceDAL.UpdateInvoiceDetail, SalesOperationRollbackDAL.RollbackDeliveryInvoice, SalesOperationRollbackDAL.RollbackProforma …
- Columns observed (46): AdjustmentAmount, BatchNo, BonusQuantity, Campaign, CostPrice, DCStoreId, DelivarySpecialAmount, DeliveryBonusQuantity, DeliveryDiscountAmount, DeliveryDiscountPercentage, DeliveryNetAmount, DeliveryQuantity, DeliveryStatus, DeliveryTotalPrice, DeliveryTotalPriceVatAmount, DeliveryTotalQuantity, DiscountAmount, DiscountPercentage, ExpDate, ISGiftProduct, InvoiceId, IsCampaignProduct, NetAmount, OrderDetailsId, PackSize, PaymentBonusQuantity, PaymentDiscountAmount, PaymentDiscountPercentage, PaymentNetAmount, PaymentQuantity, PaymentReturnReason, PaymentStatus, PaymentTotalPrice, PaymentTotalPriceVatAmount, PaymentTotalQuantity, ProductCode, ProductName, Quantity, ReceiveDate, ReturnReason, SpecialAmount, TotalPrice, TotalPriceVatAmount, TotalQuantity, UnitPrice, UnitVatAmount
- ALTERs in repo scripts: none

### tblInvoiceDetail_DeleterRecord

- Modules (pages reaching it via inline SQL): M07, M13
- Read by (2): DeleteInvoiceReportDAL.GetDeleteOrderNationalReport, DeleteInvoiceReportDAL.GetDeleteOrderReport
- Written by (1): SalesOperationRollbackDAL.RollbackDeliveryInvoice
- Columns observed (8): BatchNo, DeliveryQuantity, ExpDate, InvoiceDetailId, InvoiceId, ProductCode, Quantity, ReceiveDate
- ALTERs in repo scripts: none

### tblInvoiceDetailReturn

- Modules (pages reaching it via inline SQL): M09
- Read by (1): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblJobLocation

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblLoginLog

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (0): 
- Written by (1): PanalClsDAL.LoginLog
- Columns observed (3): LoginName, LoginTime, UserId
- ALTERs in repo scripts: none

### tblMainMenu

- Modules (pages reaching it via inline SQL): M01, M02, M06, M10, M12, M18
- Read by (19): AppSetupDAL.LoadAppSetup, AppSetupDAL.LoadMenuName, ClsApprovalAction.LoadActionControlByUser, ClsApprovalAction.LoadForApprovalByUserCondition, DeStockOutDal.GetMenuIdByMenuName, OrderListDAL.LoadApprovalControlDAL …
- Written by (1): PanalDAL.MenuSaveDal
- Columns observed (4): ManuName, ParantId, SL, URL
- ALTERs in repo scripts: none

### tblManufacturer

- Modules (pages reaching it via inline SQL): M04, M05, M06, M07, M08, M09, M10, M11, M12, M13, M14, M15, M17
- Read by (42): ChalanDAL.LoadManufac, ChallanReportViewDAL.GetAllNonPickReqDAL, ChallanReportViewDAL.GetAllNonSubmitReqDAL, ChallanReportViewDAL.LoadmanufacturerName, CustPaymentDAL.LoadManufac, ExcelUpForCustTagChangeDAL.LoadCustomer …
- Written by (2): ManufacturerDAL.SaveManufacturer, ManufacturerDAL.UpdateManufacturerInfo
- Columns observed (4): ManufacAddress, ManufacCode, ManufacId, ManufacName
- ALTERs in repo scripts: none

### tblMarket

- Modules (pages reaching it via inline SQL): M03, M05, M06, M07, M08, M09, M12, M13, M17
- Read by (73): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, CustPaymentDAL.LoadMarket, CustomerMasterDAL.LoadMarketName, CustomerMasterDAL.LoadMarketNameById, CustomerMasterInfoDal.GetMaketInfo, CustomerMasterInfoDal.GetMarketNameById …
- Written by (2): MarketInfoDAL.SaveMarketInfo, MarketInfoDAL.UpdateCustCategoryInfo
- Columns observed (3): MarketCode, MarketId, MarketName
- ALTERs in repo scripts: none

### tblmarket

- Modules (pages reaching it via inline SQL): M13
- Read by (2): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblMenuDistribution

- Modules (pages reaching it via inline SQL): M01
- Read by (8): PanalClsDAL.MainMenu, PanalClsDAL.SubItem, PanalClsDAL.SubSubChildItem, PanalClsDAL.SubSubItem, PanalDAL.CheckMenuSl, PanalDAL.MainMenu …
- Written by (2): PanalDAL.MenuPermissionRemove, PanalDAL.SaveMainMenu
- Columns observed (4): MenuSL, SL, Status, UserId
- ALTERs in repo scripts: none

### tblMIAInfo

- Modules (pages reaching it via inline SQL): M03, M05, M07, M12, M13, M14, M15, M17
- Read by (46): CompanySalesReportDAL.SalesReportDAlParameter, CustomerMasterDAL.LoadMiaName, CustomerMasterDAL.LoadMiaNameById, CustomerMasterInfoDal.GetMiaInfo, CustomerMasterInfoDal.GetMiaNameById, ImportedApiCustomerDal.GetMiaInfo …
- Written by (2): MiaInformationDAL.SaveDataForMiaInfo, MiaInformationDAL.UpdateaMiaInformation
- Columns observed (4): AreaId, MiaCode, MiaId, MiaName
- ALTERs in repo scripts: none

### tblMIATarget

- Modules (pages reaching it via inline SQL): M14
- Read by (3): MiaTargetDAL.HasMiaName, MiaTargetDAL.LoadMiaTargetView, MiaTargetDAL.MiaTargetEditLoad
- Written by (2): MiaTargetDAL.SaveMiaTarget, MiaTargetDAL.UpdateMiaTarget
- Columns observed (6): MiaCode, MiaName, MiaTargetAmount, MiaTargetId, Period, Year
- ALTERs in repo scripts: none

### tblMIGODetail

- Modules (pages reaching it via inline SQL): M05, M11, M12, M17
- Read by (19): ChalanDAL.ChalanReport, ChalanDAL.DCStoreReport, ChalanDAL.SubDepotChalanReport, ChalanDAL.SubdeportDCStoreReport, ChallanReportViewDAL.DCStoreReport, ExcelUpForCustTagChangeDAL.DeleteDetailData …
- Written by (4): ExcelUpForCustTagChangeDAL.DeleteDetailData, ExcelUpForCustTagChangeDAL.XLDataGridToDbByRow, ExcelUpForMIGODAL.DeleteDetailData, ExcelUpForMIGODAL.XLDataGridToDbByRow
- Columns observed (24): Amount, Batch, BilltoParty, CaseNoofShipper, DeliveryDocDate, DeliveryDocNo, ExpDate, InvoiceDate, InvoiceNo, ItemNo, LMID, LMIDDescription, MfgDate, MigoMasterID, OrderDocDate, OrderDocNo, PODate, PONo, Qty, ShipToParty, Total, TransportNo, VAT, VATChallan
- ALTERs in repo scripts: none

### tblMIGOMaster

- Modules (pages reaching it via inline SQL): M05, M17
- Read by (10): ExcelUpForCustTagChangeDAL.DeleteData, ExcelUpForCustTagChangeDAL.LoadMainMigoReport, ExcelUpForCustTagChangeDAL.LoadMigo, ExcelUpForCustTagChangeDAL.LoadMigoDate, ExcelUpForCustTagChangeDAL.LoadMigobyID, ExcelUpForMIGODAL.DeleteData …
- Written by (4): ExcelUpForCustTagChangeDAL.DeleteData, ExcelUpForCustTagChangeDAL.SaveMigoDAL, ExcelUpForMIGODAL.DeleteData, ExcelUpForMIGODAL.SaveMigoDAL
- Columns observed (7): EntryBy, EntryDate, ManufacId, MigoMasterID, MogoCode, MogoDocumentDate, StockUpload
- ALTERs in repo scripts: none

### tblMIOInfo

- Modules (pages reaching it via inline SQL): M04, M08
- Read by (6): BatchWiseCollectionReportDal.BatchWiseCollectionByBatchId, CustomerReturnDAL.LoadCustomerInfoByCode, PromoMITagDAL.LoadMIO, PromoMITagDAL.LoadMIOByGroupId, ReturnInvoiceDAL.LoadCustomerMaster, SalesReturnDAL.InvoiceMainDataForReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblMonthlyTarget

- Modules (pages reaching it via inline SQL): M14
- Read by (4): TargetDAL.CheckLoadMonthlyTarget, TargetDAL.LoadMonthlyTarget, TargetDAL.LoadMonthlyTargetById, TargetDAL.SaveMonthlyTarget
- Written by (2): TargetDAL.SaveMonthlyTarget, TargetDAL.UpdateMonthlyTarget
- Columns observed (7): Amount, Date, EntryBy, EntryDate, FinYearId, Month, Year
- ALTERs in repo scripts: none

### tblNationality

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblNonTranscationalInvoiceDetail

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): BatchWiseCollectionReportDal.NTInvoiceReturnReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblNonTranscationalInvoiceMaster

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (2): BatchWiseCollectionReportDal.LoadNonTranscationalInvoice, BatchWiseCollectionReportDal.NTInvoiceReturnReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOccupation

- Modules (pages reaching it via inline SQL): M01
- Read by (3): ChangePasswordDAL.GetEmpChildrenInfoDAL, ChangePasswordDAL.GetEmpReferenceInfoDAL, ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOfficeBranch

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOpeningBalanceFinancialYearLog

- Modules (pages reaching it via inline SQL): M03
- Read by (1): UserInfoDAL.IsOpeningBalanceExistsForFinancialYear
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOrder

- Modules (pages reaching it via inline SQL): M05, M06, M07, M08, M09, M12, M13
- Read by (94): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, CompanySalesReportDAL.CustomerPaymentDAl, CompanySalesReportDAL.SalesReectionReportDAl, CustomerMasterDAL.Customerorder, CustomerMasterDAL.LoadOrderView, CustomerPaymentDeleteDAL.LoadInvoicesByRoute …
- Written by (6): OrderInfoDAL.UpdateInvoiceStatus, OrderInfoDALSalesReturn.UpdateInvoiceStatus, OrderListDAL.ApprovalUpdateDAL, OrderListDAL.UpdateCompanyInfo, SalesOperationRollbackDAL.RollbackProforma, SalesReturnNewDAL.UpdateInvoiceStatus
- Columns observed (7): ActionStatus, ApprovedBy, ApprovedDate, ComUnitCode, ComUnitId, ComUnitName, IsInvoice
- ALTERs in repo scripts: none

### tblOrder_Doctorrequirement

- Modules (pages reaching it via inline SQL): M06
- Read by (3): OrderInfoDAL.LoadDoctorOrderForOrderCreation, OrderInfoDALSalesReturn.LoadDoctorOrderForOrderCreation, SalesReturnNewDAL.LoadDoctorOrderForOrderCreation
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOrderDel

- Modules (pages reaching it via inline SQL): M06, M13
- Read by (4): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, DeleteOrderReportDal.GetDeleteOrderNationalReport, DeleteOrderReportDal.GetDeleteOrderReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOrderDetail

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M13
- Read by (43): CompanySalesReportDAL.SalesReectionReportDAl, InvoiceDAL.Isgift, InvoiceDAL.LoadProductQty, MasterSetup_UI_OrderTrackingList.Parm, MasterSetup_UI_OrderTrackingListDBH.Parm, MasterSetup_UI_OrderTrackingSummary.Parm …
- Written by (5): InvoiceDAL.UpdateOrder, ReturnInvoiceDAL.UpdateOrder, SalesOperationRollbackDAL.RollbackProforma, SalesReturnDAL.UpdateOrder, Sub_InvoiceDAL.UpdateOrder
- Columns observed (1): Status
- ALTERs in repo scripts: none

### tblOrderDetailDel

- Modules (pages reaching it via inline SQL): M13
- Read by (2): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblOrderListDetail

- Modules (pages reaching it via inline SQL): M06, M17
- Read by (6): ExcelUpForCustTagChangeDAL.LoadOrderDetail, ExcelUpForMIGODAL.LoadOrderDetail, ExcelUpForOrderListDAL.DeleteDetailData, ExcelUpForOrderListDAL.LoadOrder, PendingOrderDal.GetOrderDetailsInfo, PendingOrderDal.GetPendingOrderInformation
- Written by (2): ExcelUpForOrderListDAL.DeleteDetailData, ExcelUpForOrderListDAL.XLDataGridToDbByRow
- Columns observed (16): CustomerID, CustomerName, DZSMCode, FECode, GrossValue, MIOCode, MIOName, OrderCode, OrderMasterID, OrderQty, ProductCode, ProductName, SalesCentre, SalesCentreName, SubmissionDate, TerritoryCode
- ALTERs in repo scripts: none

### tblOrderListMaster

- Modules (pages reaching it via inline SQL): M06, M17
- Read by (6): ExcelUpForCustTagChangeDAL.LoadOrderDetail, ExcelUpForMIGODAL.LoadOrderDetail, ExcelUpForOrderListDAL.DeleteData, ExcelUpForOrderListDAL.LoadMigoDate, ExcelUpForOrderListDAL.LoadMigobyID, ExcelUpForOrderListDAL.LoadOrder
- Written by (2): ExcelUpForOrderListDAL.DeleteData, ExcelUpForOrderListDAL.SaveOrderDAL
- Columns observed (6): DocumentDate, EntryBy, EntryDate, GenerateOrder, ManufacId, OrderMasterID
- ALTERs in repo scripts: none

### tblPackSize

- Modules (pages reaching it via inline SQL): M04, M10
- Read by (11): BatchWiseCollectionReportDal.NTInvoiceReturnReport, MaterialConversionReceiveDal.LoadMaterialConversionList, MaterialConversionReceiveDal.LoadMaterialConversionListForEdit, PackSizeDAL.HasPackSizeName, PackSizeDAL.HasPackSizeNameUp, PackSizeDAL.LoadPackSize …
- Written by (2): PackSizeDAL.SavePackSize, PackSizeDAL.UpdatePackSizeInfo
- Columns observed (1): PackSizeName
- ALTERs in repo scripts: none

### tblPaymentType

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M12
- Read by (33): CustPaymentDAL.PaymentTypeLoad, InvoiceDAL.DelivaryInvoiceMainDataForReport, InvoiceDAL.DeliveryInvoiceMainDataForReportDAL, InvoiceDAL.InvoiceMainDataForReport, InvoiceDAL.InvoiceMainDataForReport2, InvoiceDAL.InvoiceMainDatainvNo …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblProCategory

- Modules (pages reaching it via inline SQL): M04, M09, M10
- Read by (9): CentralStoreDAL.LoadCategoryDropdown, ProductCategoryDAL.HasCustCategoryName, ProductCategoryDAL.HasCustCategoryNameUp, ProductCategoryDAL.LoadCategoryView, ProductCategoryDAL.ProductCategoryEditLoad, ProductDAL.LoadCategoryName …
- Written by (2): ProductCategoryDAL.SaveProductCategory, ProductCategoryDAL.UpdateProCategoryInfo
- Columns observed (3): CategoryCode, CategoryId, CategoryName
- ALTERs in repo scripts: none

### tblProduct

- Modules (pages reaching it via inline SQL): M04, M05, M06, M07, M08, M09, M10, M11, M12, M13, M14, M17
- Read by (175): BatchWiseCollectionReportDal.NTInvoiceReturnReport, CentralStoreDAL.LoadProductDropdown, ChalanDAL.ChalanReport, ChalanDAL.DCStockInDALMain, ChalanDAL.DCStoreReport, ChalanDAL.GetChalanReceieve …
- Written by (2): ProductDAL.SaveDataForProduct, ProductDAL.UpdateProduct
- Columns observed (18): CaseId, CategoryId, Description, GenericGroupId, IsActive, ManufacId, PackSize, PackSizeId, ProTypeId, ProductBrandId, ProductCode, ProductGroupId, ProductId, ProductImage, ProductLineID, ProductName, StockUOMId, TherapueticGroupId
- ALTERs in repo scripts: none

### tblProductCase

- Modules (pages reaching it via inline SQL): M04, M06, M07
- Read by (8): ProductDAL.LoadProduct, ProductDAL.LoadProductSearch, ProductDAL.LoadShippingCartonSize, ProductDAL.ProductPriceDetailWithCase, ProductUnitPriceDAL.LoadUnitPriceView, ShippingCartonSizeDAL.HasPcsPerCase …
- Written by (2): ShippingCartonSizeDAL.SaveShippingCartonSize, ShippingCartonSizeDAL.UpdateShippingCartonSizeInfo
- Columns observed (4): CaseId, CaseQty, PcsPerCase, ProductCode
- ALTERs in repo scripts: none

### tblProductDCDetails

- Modules (pages reaching it via inline SQL): M04
- Read by (1): ProductDAL.ProductEditLoad
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblProductDiscount

- Modules (pages reaching it via inline SQL): M04, M06, M07, M08, M09, M12
- Read by (30): InvoiceDAL.DelivaryInvoiceDetailDataForReport, InvoiceDAL.DiscountofProduct, InvoiceDAL.InvoiceDetailDataForReport, InvoiceDAL.InvoiceDetailDataForReport2, InvoiceDAL.InvoiceDetailDataInvoID, InvoiceDAL.RInvoiceDetailDataForReport …
- Written by (2): ProductDiscountDAL.SaveProductDiscount, ProductDiscountDAL.UpdateProductDiscountInfo
- Columns observed (7): ActiveDate, CustomerMasterId, DiscountId, DiscountPercentage, InactiveDate, ProductCode, Status
- ALTERs in repo scripts: none

### tblProductGroup

- Modules (pages reaching it via inline SQL): M04, M10
- Read by (3): ProductDAL.LoadProduct, ProductDAL.LoadProductSearch, ProductDAL.LoadProductType_new
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblProductSQ

- Modules (pages reaching it via inline SQL): M04, M07, M08, M13, M16
- Read by (10): CompanySalesReportDAL.SalesReportDAl, CompanySalesReportDAL.SalesReturnReportDAl, CompanySalesReportDAL.SalesReturnReportNationalDAl, ProductDAL.LoadProductSQ, ProductSQDAL.DeleteProductBrand, ProductSQDAL.HasProductSQName …
- Written by (3): ProductSQDAL.DeleteProductBrand, ProductSQDAL.SaveProductSQ, ProductSQDAL.UpdateProductSQInfo
- Columns observed (3): IngridentsId, ProductBrandId, ProductSQName
- ALTERs in repo scripts: none

### tblProductWiseSalesTarget

- Modules (pages reaching it via inline SQL): M14
- Read by (2): ProuctWiseSalesDAL.LoadMonthlyTarget, ProuctWiseSalesDAL.LoadMonthlyTargetById
- Written by (2): ProuctWiseSalesDAL.SaveProductWiseSales, ProuctWiseSalesDAL.UpdateMonthlyTarget
- Columns observed (11): Amount, AreaId, Date, EntryBy, EntryDate, GroupId, Month, ProductId, RegionId, TerritoryId, Year
- ALTERs in repo scripts: none

### tblProgramType

- Modules (pages reaching it via inline SQL): M07, M09
- Read by (3): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, InvoiceDAL.InvoiceMainDataForReport, SalesReturnDAL.InvoiceMainDataForReport
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblProjectSetup

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblPromoGroup

- Modules (pages reaching it via inline SQL): M04
- Read by (6): GroupWisePromoQtyDAL.LoadGroupWiseQty, PromoGroupDAL.LoadPromoGroup, PromoGroupDAL.LoadPromoGroupById, PromoGroupDAL.LoadPromoGroupCode, PromoGroupDAL.SavePromoGroup, PromoMITagDAL.LoadGroup
- Written by (2): PromoGroupDAL.SavePromoGroup, PromoGroupDAL.UpdatePromoGroup
- Columns observed (4): EntryBy, EntryDate, PromoGroupCode, PromoGroupName
- ALTERs in repo scripts: none

### tblPromoMIOTagDetail

- Modules (pages reaching it via inline SQL): M04
- Read by (3): PromoMITagDAL.DeleteData, PromoMITagDAL.LoadMIO, PromoMITagDAL.LoadMIOByGroupId
- Written by (2): PromoMITagDAL.DeleteData, PromoMITagDAL.SaveMIOTagDetail
- Columns observed (2): MIOId, MIOTagMasterId
- ALTERs in repo scripts: none

### tblPromoMIOTagMaster

- Modules (pages reaching it via inline SQL): M04
- Read by (3): PromoMITagDAL.LoadMIO, PromoMITagDAL.LoadMIOByGroupId, PromoMITagDAL.LoadMIOTag
- Written by (1): PromoMITagDAL.SaveMIOTagMaster
- Columns observed (3): EntryBy, EntryDate, PromoGroupId
- ALTERs in repo scripts: none

### tblProType

- Modules (pages reaching it via inline SQL): M04
- Read by (6): ProTypeDAL.HasProTypeName, ProTypeDAL.HasProTypeNameUp, ProTypeDAL.LoadProType, ProTypeDAL.ProTypeEditLoad, ProductDAL.LoadType, ProductUnitPriceDAL.LoadUnitPriceView
- Written by (2): ProTypeDAL.SaveProType, ProTypeDAL.UpdateProTypeInfo
- Columns observed (2): ProTypeId, ProTypeName
- ALTERs in repo scripts: none

### tblPurpose

- Modules (pages reaching it via inline SQL): M04, M11
- Read by (2): SCtoWHTransferDal.LoadPurposeDropDownList, StockRcvByWhDal.GetChallanDetailByReqId
- Written by (1): ConditionalPurposeDal.SaveSettings
- Columns observed (2): Purpose, StockConditionId
- ALTERs in repo scripts: none

### tblRegion

- Modules (pages reaching it via inline SQL): M03, M05, M06, M07, M09, M12, M13, M14, M15, M17
- Read by (34): AreaDAL.AreaEditLoad, AreaDAL.LoadAreaView, AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, CmnCrystaltoView.GetRegionListDAL, CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new …
- Written by (2): RegionDAL.SaveRegionInfo, RegionDAL.UpdateRegionInfo
- Columns observed (3): RegionCode, RegionId, RegionName
- ALTERs in repo scripts: none

### tblregion

- Modules (pages reaching it via inline SQL): M11
- Read by (3): OrderInfoDAL.LoadDZSM, OrderInfoDALSalesReturn.LoadDZSM, SalesReturnNewDAL.LoadDZSM
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblRequisition

- Modules (pages reaching it via inline SQL): M05, M06, M07, M10, M11, M13, M17
- Read by (36): ChallanReportViewDAL.ChallanDetailReportQuery, ChallanReportViewDAL.ChallanExistsDAL, ChallanReportViewDAL.ChallanReportDAL, ChallanReportViewDAL.ChallanReportDAL2, ChallanReportViewDAL.ChallanSummaryReportQuery, ChallanReportViewDAL.DeleteRequisition …
- Written by (15): ChallanReportViewDAL.DeleteRequisition, ChallanReportViewDAL.SaveReuqisitionDAL, ChallanReportViewDAL.UpdateIssueInformationOnRequisitionDAL, ChallanReportViewDAL.UpdateManufacturerInfo, ChallanReportViewDAL.UpdatePickingInformationOnRequisitionDAL, ChallanReportViewDAL.UpdateReceiveIssueStatus …
- Columns observed (28): ComUnitCode, ComUnitId, ComUnitName, CreatePicking, DriverName, EntryBy, EntryDate, GrandTotalPrice, IsFromBatch, IssuChalanDate, IssueChalanNo, ManufacId, PickingDate, PickingNo, ReceiveIssue, ReceiveIssueDate, ReqDate, ReqId, ReqNo, Submit, SubmitDate, TotalPrice, TotalVAT, TruckNo, UpdateBy, UpdateDate, WarehouseId, WearhouseName
- ALTERs in repo scripts: none

### tblRequsitionChild

- Modules (pages reaching it via inline SQL): M05, M06, M07, M10, M11, M13
- Read by (13): ChallanReportViewDAL.DeleteRequisition, ChallanReportViewDAL.DeleteRequisitionDtls, ChallanReportViewDAL.GetDataInfoByIdBllDtls, ChallanReportViewDAL.GetPickingDataForIssueDAL, ChallanReportViewDAL.GetRequisitionView, RequisitionDAL.DeleteRequisition …
- Written by (14): ChallanReportViewDAL.DeleteRequisition, ChallanReportViewDAL.DeleteRequisitionDtls, ChallanReportViewDAL.ReqDetailUpdateAfterIssue, ChallanReportViewDAL.SaveReuqisitionChildDAL, ChallanReportViewDAL.UpdateIssueInformationOnRequisitionChildDAL, ChallanReportViewDAL.UpdateReqDetailIssueStatusDAL …
- Columns observed (17): BatchNO, CaseQty, IsIssue, IsPicking, IssueQty, MusakTotalPrice, MusakVATAmount, PackSize, PriceAmount, ProductCode, ProductName, ReqChildId, ReqId, ReqQty, TotalPrice, UnitPrice, VATAmount
- ALTERs in repo scripts: none

### tblReturnAmount

- Modules (pages reaching it via inline SQL): M06, M08, M12, M13
- Read by (5): OrderInfoDAL.GetCustomerCredit, OrderInfoDALSalesReturn.GetCustomerCredit, ReturnInvoiceDAL.DeleteData, SalesOperationRollbackDAL.RollbackProforma, SalesReturnNewDAL.GetCustomerCredit
- Written by (5): InvoiceDAL.SaveDataForReturnAmount, ReturnInvoiceDAL.DeleteData, ReturnInvoiceDAL.SaveDataForReturnAmount, SalesOperationRollbackDAL.RollbackProforma, SalesReturnDAL.SaveDataForReturnAmount
- Columns observed (4): Amount, CustomerId, InvoiceId, ReturnInvoiceId
- ALTERs in repo scripts: none

### tblReturnInvoice

- Modules (pages reaching it via inline SQL): M07, M08, M09, M12
- Read by (36): CompanySalesReportDAL.SalesReturnReportDAl, CompanySalesReportDAL.SalesReturnReportNationalDAl, CustPaymentDAL.LoadInvoice, InvoiceDAL.FindInvoiceCode, InvoiceDAL.RInvoiceDetailDataForReport, InvoiceDAL.RInvoiceMainDataForReport …
- Written by (7): InvoiceDAL.SaveDataForReturnInvoice, ReturnInvoiceDAL.DeleteData, ReturnInvoiceDAL.SaveDataForInvoice, ReturnInvoiceDAL.SaveDataForReturnInvoice, ReturnInvoiceDAL.UpdateInvoice, SalesReturnDAL.SaveDataForReturnInvoice …
- Columns observed (50): AreaCode, ComUnitId, CreateBy, CreateDate, CustomerMasterId, DZSMName, DeductionCharge, DelivaryInvoiceNo, DelivarySpecialAmount, DeliveryInvoiceStatus, DeliveryPersonName, DeliveryPersonPhNo, DeliveryTpDiscount, DeliveryTpGrandTotal, DeliveryTpTotal, DeliveryTpVat, DisCode, FEName, FixedCustomer, ISSubdeport, InvoiceId, InvoiceNo, IsSalesReturnWithoutOrder, MIACode, MIAName, MarketCode, MarketName, MiaId, NetReturnAmount, OldTradePolicy, OrderDate, OrderId, OrderNo, PaymentTypeId, ProductOffer, RegionCode, Remarks, ReturnInvoiceDate, ReturnInvoiceId, ReturnInvoiceNo, SubInvoiceId, TotalSpecialAmount, TpDiscount, TpGrandTotal, TpTotal, TpVat, Types, UpdateBy, UpdateDate, UserId
- ALTERs in repo scripts: ADD DeductionCharge DECIMAL(18, 2) (Live_Deployment_Script_All_Updates.sql:48); ADD NetReturnAmount DECIMAL(18, 2) (Live_Deployment_Script_All_Updates.sql:60); ADD IsPosting BIT (Live_Deployment_Script_All_Updates.sql:72); ADD DeductionCharge DECIMAL(18, 2) (Live_Deployment_Script_All_Updates.sql:824); ADD NetReturnAmount DECIMAL(18, 2) (Live_Deployment_Script_All_Updates.sql:835)

### tblReturnInvoiceDetail

- Modules (pages reaching it via inline SQL): M07, M08, M12
- Read by (22): CompanySalesReportDAL.SalesReturnReportDAl, CompanySalesReportDAL.SalesReturnReportNationalDAl, InvoiceDAL.RInvoiceDetailDataForReport, InvoiceDAL.ReturnInvoiceDetailDataForReport, InvoiceDAL.ReturnReturnInvoiceDetailDataForReport, OrderInfoDAL.GetQty …
- Written by (7): InvoiceDAL.SaveDataForReturnInvoiceDetails, ReturnInvoiceDAL.DeleteData, ReturnInvoiceDAL.SaveDataForInvoiceDetails, ReturnInvoiceDAL.SaveDataForReturnInvoiceDetails, ReturnInvoiceDAL.UpdateInvoiceDetail, SalesReturnDAL.SaveDataForReturnInvoiceDetails …
- Columns observed (36): BatchNo, BonusQuantity, Campaign, CostPrice, DCStoreId, DelivarySpecialAmount, DeliveryBonusQuantity, DeliveryDiscountAmount, DeliveryDiscountPercentage, DeliveryNetAmount, DeliveryQuantity, DeliveryStatus, DeliveryTotalPrice, DeliveryTotalPriceVatAmount, DeliveryTotalQuantity, DiscountAmount, DiscountPercentage, ExpDate, InvoiceDetailId, NetAmount, OrderDetailsId, PackSize, ProductCode, ProductName, Quantity, ReceiveDate, ReturnInvoiceDetailId, ReturnInvoiceId, ReturnReason, ReuturnInvoiceDetailId, SpecialAmount, TotalPrice, TotalPriceVatAmount, TotalQuantity, UnitPrice, UnitVatAmount
- ALTERs in repo scripts: none

### tblReturnReason

- Modules (pages reaching it via inline SQL): M06, M08, M09
- Read by (8): InvoiceDAL.GetReason, InvoiceDAL.ReturnReason, ReturnInvoiceDAL.GetReason, ReturnInvoiceDAL.ReturnReason, SalesReturnDAL.GetReason, SalesReturnDAL.ReturnReason …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblRoleType

- Modules (pages reaching it via inline SQL): M01
- Read by (1): PanalClsDAL.Login
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblRouteInformationMarketDetail

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): SInventoryWebService.GetCustomer_ALL_ForDIC
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblRouteInformationMaster

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M12
- Read by (17): CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, InvoiceDAL.InvoiceDetailDataForReportNew_, OrderInfoDAL.LoadDeliveryDisRouteforInvoice, OrderInfoDAL.LoadDisRoute, OrderInfoDAL.LoadDisRouteforInvoice, OrderInfoDAL.SubDepoLoadDisRouteforInvoice …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSalaryGrade

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSalaryLocation

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSalaryStep

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSalesReturnDetail

- Modules (pages reaching it via inline SQL): M08
- Read by (1): ReturnInvoiceDAL.LoadOrderWithDetail
- Written by (1): ReturnInvoiceDAL.SaveOrderDetail
- Columns observed (7): OrderId, ProductCode, ProductId, ProductName, Quantity, TotalTradePrice, TradePrice
- ALTERs in repo scripts: ALTER COLUMN ProductName NVARCHAR(500) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:24); ALTER COLUMN ProductCode NVARCHAR(150) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:25)

### tblSampleStockForDcDetails

- Modules (pages reaching it via inline SQL): M10
- Read by (2): SampleTypeConventionDal.DcStockOutDetailsDeleteDal, SampleTypeConventionDal.LoadSampleStock
- Written by (2): SampleTypeConventionDal.DcStockOutDetailsDeleteDal, SampleTypeConventionDal.SaveDataForStockOutDetailDal
- Columns observed (9): BatchNo, DCStoreId, ExpDate, ProductCode, ProductName, ReceiveDate, SampleStock, SampleStockForDcDetailsId, SampleStockForDcMasterId
- ALTERs in repo scripts: none

### tblSampleStockForDcMaster

- Modules (pages reaching it via inline SQL): M10
- Read by (2): SampleTypeConventionDal.DcStockOutMasterDeleteDal, SampleTypeConventionDal.DcStockOutViewDal
- Written by (2): SampleTypeConventionDal.DcStockOutMasterDeleteDal, SampleTypeConventionDal.SaveDataForSubDcStockOutMaster
- Columns observed (8): Action, ComUnitId, Date, EntryBy, EntryDate, SampleStockForDcMasterCode, SampleStockForDcMasterId, Status
- ALTERs in repo scripts: none

### tblSampleStockForWareHouseDetails

- Modules (pages reaching it via inline SQL): M10
- Read by (2): SampleStockForWHDal.DcStockOutDetailsDeleteDal, SampleStockForWHDal.GetStock
- Written by (2): SampleStockForWHDal.DcStockOutDetailsDeleteDal, SampleStockForWHDal.SaveDataForStockOutDetailDal
- Columns observed (9): BatchNo, ExpDate, ProductCode, ProductName, ReceiveDate, ReceiveId, SampleStock, SampleStockForWHDetailsId, SampleStockForWHMasterId
- ALTERs in repo scripts: none

### tblSampleStockForWareHouseMaster

- Modules (pages reaching it via inline SQL): M10
- Read by (2): SampleStockForWHDal.DcStockOutMasterDeleteDal, SampleStockForWHDal.DcStockOutViewDal
- Written by (2): SampleStockForWHDal.DcStockOutMasterDeleteDal, SampleStockForWHDal.SaveDataForSubDcStockOutMaster
- Columns observed (8): Action, Date, EntryBy, EntryDate, SampleStockForWHMasterId, SampleStockForWareHouseMstCode, Status, WareHouseId
- ALTERs in repo scripts: none

### tblSection

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSMCTrainingVenue

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSpecialization

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpEducationInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblStockCondition

- Modules (pages reaching it via inline SQL): M04, M10
- Read by (4): ConditionalPurposeDal.LoadCondition, StockConditionFreezeDAL.LoadStockConditionBll, StockConditionPermissionDal.GetStockConditionList, WhStockConditionFreezeDal.LoadStockConditionBll
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblStockConditionFreeze

- Modules (pages reaching it via inline SQL): M10
- Read by (0): 
- Written by (2): StockConditionFreezeDAL.SaveforDC, StockConditionFreezeDAL.SaveforWH
- Columns observed (7): DCStoreId, EntryBy, EntryDate, FreezeQty, ManufacId, ReceiveId, StockConditionFreezeID
- ALTERs in repo scripts: none

### tblStockConditionPermission

- Modules (pages reaching it via inline SQL): M10
- Read by (5): StockConditionFreezeDAL.LoadStockConditionBll, StockConditionPermissionDal.CheckPermissionInfoAlreadyExistOrNot, StockConditionPermissionDal.DeletePermissionInfo, StockConditionPermissionDal.GetStockConditionByUserId, WhStockConditionFreezeDal.LoadStockConditionBll
- Written by (2): StockConditionPermissionDal.DeletePermissionInfo, StockConditionPermissionDal.SaveStockConditionPermissionInfo
- Columns observed (3): Permission, StockConId, UserId
- ALTERs in repo scripts: none

### tblStockInTransfar

- Modules (pages reaching it via inline SQL): M06, M07, M10, M11, M12, M13, M17
- Read by (39): ChalanDAL.ChalanReport, ChalanDAL.DCStoreReport, ChalanDAL.SubDepotChalanReport, ChalanDAL.SubdeportDCStoreReport, ChallanReportViewDAL.ChallanDetailReportQuery, ChallanReportViewDAL.ChallanSummaryReportQuery …
- Written by (7): ChallanReportViewDAL.StockInTransfarInsertDAL, ChallanReportViewDAL.StockInTransfarStatusUpdate, ChallanReportViewDAL.UpdateStockTransfarInfoUpdateDAL, RequisitionDAL.StockInTransfarInsertDAL, RequisitionDAL.StockInTransfarStatusUpdate, RequisitionDAL.UpdateStockTransfarInfoUpdateDAL …
- Columns observed (19): BatchNo, ExpDate, IsIssue, IsTransfared, MfgDate, PackSize, PickingQty, PriceAmount, ProductCode, ProductName, Quantity, ReceiveDate, ReceiveId, ReqChildId, ReqId, StockInTransfarId, TotalPriceAmount, UnitPrice, VATAmount
- ALTERs in repo scripts: none

### tblStockUOM

- Modules (pages reaching it via inline SQL): M04, M07, M10, M11
- Read by (11): ProductDAL.LoadProduct, ProductDAL.LoadProductSearch, ProductDAL.LoadStockUOM, ProductUnitPriceDAL.LoadUnitPriceView, StockTransportOrderReportDAL.StockTransportOrderReportDetailData, StockUOMDAL.HasStockUOMName …
- Written by (2): StockUOMDAL.SaveStockUOM, StockUOMDAL.UpdateStockUOMInfo
- Columns observed (2): StockUOMId, StockUOMName
- ALTERs in repo scripts: none

### tblSubDepoStockAdjustDetail

- Modules (pages reaching it via inline SQL): M12
- Read by (0): 
- Written by (1): SubDepoAdjustDAL.SaveDataForSubDepoAdjustDetail
- Columns observed (3): Quantity, SubDCStoreId, SubDepoStockOutId
- ALTERs in repo scripts: none

### tblSubDepoStockAdjustment

- Modules (pages reaching it via inline SQL): M12
- Read by (0): 
- Written by (1): SubDepoAdjustDAL.SaveDataForSubDepoAdjust
- Columns observed (4): EntryBy, EntryDate, SubDepoStockCode, SubDepoStockOutId
- ALTERs in repo scripts: none

### tblSubDepot

- Modules (pages reaching it via inline SQL): M12
- Read by (20): ChalanDAL.SubdeportDCStoreReport, ChallanReportViewDAL.SubdeportCodeLoad, ChallanReportViewDAL.SubdeportLoad, DCStockReportDAL.SubDeportStockReportDetailDataDAL, RequisitionDAL.SubdeportCodeLoad, RequisitionDAL.SubdeportLoad …
- Written by (2): SubDepotDAL.SaveDataForSalesCenter, SubDepotDAL.UpdateSalesCenter
- Columns observed (7): Address, ComUnitId, FaxNo, MobileNo, PhoneNo, SubDepotCode, SubDepotName
- ALTERs in repo scripts: none

### tblSubDepotChalanDetail

- Modules (pages reaching it via inline SQL): M10, M12
- Read by (6): ChalanDAL.SubDepotChalanReport, ChalanDAL.SubdeportDCStoreReport, DCStockReportDAL.DCReportDetailDataDAL, DCStockReportDAL.SubDeportStockReportDetailDataDAL, SubDepoAdjustDAL.GetChalanReceieve, SubDepotChalanDAL.GetChalanReceieve
- Written by (2): SubDepoAdjustDAL.SaveDataForChalanDetail, SubDepotChalanDAL.SaveDataForChalanDetail
- Columns observed (11): BatchNo, DCStoreId, ProductCode, ProductName, Quantity, SChalanDetailsId, SChalanId, UnitPrice, Value, ValueWVat, Vat
- ALTERs in repo scripts: none

### tblSubDepotChalanInfo

- Modules (pages reaching it via inline SQL): M11, M12
- Read by (9): ChalanDAL.SubDepotChalanReport, DCStockReportDAL.SubDeportStockReportDetailDataDAL, SCtoWHTransferDal.LoadChalanById, SubDepoAdjustDAL.ChalanLoadInReceive, SubDepoAdjustDAL.GetChalanReceieve, SubDepoAdjustDAL.LoadChalanById …
- Written by (4): SubDepoAdjustDAL.ChalanUpdate, SubDepoAdjustDAL.SaveDataForChalanInfo, SubDepotChalanDAL.ChalanUpdate, SubDepotChalanDAL.SaveDataForChalanInfo
- Columns observed (16): ChalanDate, ChalanNo, DriverName, FromComUnitAddress, FromComUnitCode, FromComUnitName, GrandTotal, IsDeliver, ManufacId, SChalanId, SubDepotAddress, SubDepotCode, SubDepotName, TotalValue, TotalVat, TrackNo
- ALTERs in repo scripts: none

### tblSubDepotChalanRetuenDetail

- Modules (pages reaching it via inline SQL): M12
- Read by (2): ChalanDAL.SubChalanReceieve, SubDepotChalanReturnDAL.GetChalanReceieve
- Written by (1): SubDepotChalanReturnDAL.SaveDataForChalanDetail
- Columns observed (11): BatchNo, DCStoreId, ProductCode, ProductName, Quantity, SChalanDetailsId, SChalanId, UnitPrice, Value, ValueWVat, Vat
- ALTERs in repo scripts: none

### tblSubDepotChalanReturnInfo

- Modules (pages reaching it via inline SQL): M12
- Read by (5): ChalanDAL.SubChalanReceieve, ChalanDAL.SubdeportChalanLoadInReceive, SubDepotChalanReturnDAL.ChalanLoadInReceive, SubDepotChalanReturnDAL.GetChalanReceieve, SubDepotChalanReturnDAL.LoadChalanById
- Written by (3): ChalanDAL.SubChalanUpdate, SubDepotChalanReturnDAL.ChalanUpdate, SubDepotChalanReturnDAL.SaveDataForChalanInfo
- Columns observed (16): ChalanDate, ChalanNo, DriverName, FromComUnitAddress, FromComUnitCode, FromComUnitName, GrandTotal, IsDeliver, ManufacId, SChalanId, SubDepotAddress, SubDepotCode, SubDepotName, TotalValue, TotalVat, TrackNo
- ALTERs in repo scripts: none

### tblSubDepotStockOutDetails

- Modules (pages reaching it via inline SQL): M12
- Read by (2): SubDepotStockAdjustmentsVoucherDal.SubDcStockOutDetailsDeleteDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutReportDal
- Written by (2): SubDepotStockAdjustmentsVoucherDal.SaveDataForStockOutDetailDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutDetailsDeleteDal
- Columns observed (10): BatchNo, ExpDate, PackSize, ProductCode, ProductName, ReceiveDate, StockOutQty, SubDCStoreId, SubDcStockOutDetailsId, SubDcStockOutMasterId
- ALTERs in repo scripts: none

### tblSubDepotStockOutMaster

- Modules (pages reaching it via inline SQL): M12
- Read by (4): SubDepotStockAdjustmentsVoucherDal.SubDcStockOutApprovalViewDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutMasterDeleteDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutReportDal, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutViewDal
- Written by (2): SubDepotStockAdjustmentsVoucherDal.SaveDataForSubDcStockOutMaster, SubDepotStockAdjustmentsVoucherDal.SubDcStockOutMasterDeleteDal
- Columns observed (9): ComUnitId, EntryBy, EntryDate, InvoiceId, Reason, Status, StockOutDate, SubDcStockOutMasterCode, SubDcStockOutMasterId
- ALTERs in repo scripts: none

### tblSubDepotStore

- Modules (pages reaching it via inline SQL): M07, M08, M12, M13
- Read by (14): ChalanDAL.SubdeportDCStoreReport, CompanySalesReportDAL.DeliveryReturnReportDAl, CompanySalesReportDAL.SalesReportDAl, DCStockReportDAL.SubDeportStockReportDetailDataDAL, InvoiceDAL.ProformaReportDAl, OtherStockActionDAL.SubdeportLoadStockDCStockData …
- Written by (7): OtherStockActionDAL.SubdeportUpdateDCStock, SubDepoAdjustDAL.DCStockInDALMain, SubDepoAdjustDAL.UpdateSubDepoStockQuantity, SubDepotChalanDAL.DCStockInDALMain, SubDepotChalanReturnDAL.DCStockInDALMain, SubDepotChalanReturnDAL.SubUpdateDCStockQuantity …
- Columns observed (1): StockQty
- ALTERs in repo scripts: none

### tblSubDepotStoreFreeze

- Modules (pages reaching it via inline SQL): M12
- Read by (4): DCStockReportDAL.SubDeportStockReportDetailDataDAL, OtherStockActionDAL.LoadSubdeportStockData, OtherStockActionDAL.SubDeportLoadData, ReturnInvoiceDAL.SuvDCInfoWithDCId
- Written by (6): OtherStockActionDAL.SubdeportUpdateDCStoreFreezeStock, ReturnInvoiceDAL.SubDCStockInDAL, SubDepoAdjustDAL.SaveDCStoreFreeze2, SubDepotChalanDAL.SaveDCStoreFreeze2, SubDepotChalanReturnDAL.SaveDCStoreFreeze2, Sub_InvoiceDAL.DCStockInDAL
- Columns observed (23): BatchNo, ChalanDate, ChalanNo, DamageQty, ExpDate, InvoiceDetailId, PackSize, ProductCode, ProductName, ReceiveDate, ReqChildId, ReqId, ReturnInvoiceDetailId, SChalanDetailsId, SDStoreFreezeId, StockCondition, StockInTransfarId, StockQty, StockRcvDate, StorageLocation, SubDCStoreId, SubDepotId, TotalQuantity
- ALTERs in repo scripts: none

### tblSubInvoiceDetail

- Modules (pages reaching it via inline SQL): M06, M07, M08, M12, M13
- Read by (49): CompanySalesReportDAL.DeliveryReturnReportDAl, CompanySalesReportDAL.SalesReportDAl, CustomerMasterInfoDal.GetFixedCustomerSalesReportInfo2, DCStockReportDAL.SubDeportStockReportDetailDataDAL, InvoiceDAL.ProformaReportDAl, MIOTotalSummaryDAL.BranchwiseLoadSummaryBLL …
- Written by (2): Sub_InvoiceDAL.SaveDataForInvoiceDetails, Sub_InvoiceDAL.UpdateInvoiceDetail
- Columns observed (1): DeliveryQuantity
- ALTERs in repo scripts: none

### tblSubInvoiceMaster

- Modules (pages reaching it via inline SQL): M06, M07, M08, M09, M11, M12, M13
- Read by (86): CompanySalesReportDAL.CustomerPaymentDAl, CompanySalesReportDAL.DeliveryReturnReportDAl, CompanySalesReportDAL.SalesReportDAl, CustPaymentDAL.LoadSubDeportInvoice, CustomerMasterInfoDal.GetFixedCustomerSalesReportInfo2, DCStockReportDAL.SubDeportStockReportDetailDataDAL …
- Written by (4): CustPaymentDAL.SubdeportUpdateInvoicePaymentAmount, PaymentReverseDal.subResetInvoicePaymentStatus, Sub_InvoiceDAL.SaveDataForInvoice, Sub_InvoiceDAL.UpdateInvoice
- Columns observed (3): DeliveryTpTotal, PaymentAmount, PaymentStatus
- ALTERs in repo scripts: none

### tblSubInvoiceMasterBatch

- Modules (pages reaching it via inline SQL): M12
- Read by (4): OrderInfoDAL.LoadSubInvoiceBatchId, OrderInfoDALSalesReturn.LoadSubInvoiceBatchId, SalesReturnNewDAL.LoadSubInvoiceBatchId, SubDepot_UI_SubDepotInvoiceCreation.GenerateParameter
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSubSection

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSubTerritory

- Modules (pages reaching it via inline SQL): M09, M13
- Read by (4): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, SInventoryWebService.GetCustomer_ALL_ForDIC
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblSupplierInformation

- Modules (pages reaching it via inline SQL): M04, M10
- Read by (6): SupplierInfoDal.CheckDuplicate, SupplierInfoDal.HasCompanyName, SupplierInfoDal.LoadSupplierInfo, SupplierInfoDal.LoadSupplierInfoById, WHStockInReportDal.GetWarehouseStockInMasterData, WarehouseStockInDal.LoadSupplier
- Written by (2): SupplierInfoDal.SaveDataForCompanyInfo, SupplierInfoDal.UpdateCompanyInfoData
- Columns observed (8): ContactNo, EntryDate, Entryby, SupplierAddress, SupplierCode, SupplierName, UpdateDate, Updateby
- ALTERs in repo scripts: none

### tbltempCustMaster

- Modules (pages reaching it via inline SQL): M17
- Read by (4): CustomerMasterInfoDal.LoadNewCustomer, ImportedApiCustomerDal.ApiCustomerInformation, ImportedApiCustomerDal.CustomerMasterEditLoad, ImportedApiCustomerDal.LoadNewCustomer
- Written by (1): ImportedApiCustomerDal.UpdateApiCustomerInfo
- Columns observed (1): AddtoMainCustomer
- ALTERs in repo scripts: none

### tblTempSalesReturnOrder

- Modules (pages reaching it via inline SQL): M08
- Read by (2): ReturnInvoiceDAL.LoadOrderWithDetail, ReturnInvoiceDAL.OrderManualId
- Written by (1): ReturnInvoiceDAL.SaveOrderMaster
- Columns observed (14): ComUnitCode, ComUnitId, ComUnitName, CustomerCode, CustomerName, GrossValue, IsInvoice, IsManual, MIOCode, MIOName, ManufacId, OrderCode, SubmissionDate, TerritoryCode
- ALTERs in repo scripts: ALTER COLUMN CustomerName NVARCHAR(250) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:32); ALTER COLUMN ComUnitName NVARCHAR(250) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:33); ALTER COLUMN MIOName NVARCHAR(250) (LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:34)

### tblTerritory

- Modules (pages reaching it via inline SQL): M04, M06, M08, M09, M13, M14
- Read by (9): AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport, CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL_new, CustomerReturnDAL.LoadCustomerInfoByCode, OrderListDAL.GetSpecialApproveOrder, PromoMITagDAL.LoadMIO …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblThana

- Modules (pages reaching it via inline SQL): M01
- Read by (1): ChangePasswordDAL.GetEmployeeInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTherapeuticGroup

- Modules (pages reaching it via inline SQL): M04
- Read by (1): ProductDAL.LoadTherapeuticGroup
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTradePolicy

- Modules (pages reaching it via inline SQL): M06, M08, M09
- Read by (3): OrderInfoDAL.GetTradeTermOld, OrderInfoDALSalesReturn.GetTradeTermOld, SalesReturnNewDAL.GetTradeTermOld
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTradePolicyNew

- Modules (pages reaching it via inline SQL): M06, M08, M09
- Read by (3): OrderInfoDAL.GetTradeTerm, OrderInfoDALSalesReturn.GetTradeTerm, SalesReturnNewDAL.GetTradeTerm
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTradePolicyNew2

- Modules (pages reaching it via inline SQL): M06, M08, M09
- Read by (3): OrderInfoDAL.GetTradeTerm, OrderInfoDALSalesReturn.GetTradeTerm, SalesReturnNewDAL.GetTradeTerm
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTrainingOrgInfo

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTrainingRecordMaster

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblTrainingType

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (1): ChangePasswordDAL.GetEmpTrainingInfoDAL
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblUnitPrice

- Modules (pages reaching it via inline SQL): M04, M05, M06, M07, M08, M09, M10, M11, M12, M17
- Read by (56): CentralStoreDAL.LoadProduct, ChalanDAL.DCStockInDALMain, ChalanDAL.DCStoreReport, ChalanDAL.GetProductDcStore, ChalanDAL.GetProductWhStore, ChalanDAL.SubdeportDCStoreReport …
- Written by (5): ProductUnitPriceDAL.DeleteProduct, ProductUnitPriceDAL.MoveUnitPriceToRollback, ProductUnitPriceDAL.SaveProductUnitPrice, ProductUnitPriceDAL.UpdateActive, ProductUnitPriceDAL.UpdateCustCategoryInfo
- Columns observed (13): ActiveDate, CostPrice, InActiveDate, IsActive, MRPPrice, PackSize, ProductCode, ProductId, ProductName, UnitPrice, UnitPriceId, VATAmountPerUnit, VATPercentage
- ALTERs in repo scripts: none

### tblunitprice

- Modules (pages reaching it via inline SQL): M10
- Read by (1): DeStockOutDal.DcStockOutReportDal
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblUnitPrice_Old

- Modules (pages reaching it via inline SQL): M08
- Read by (1): ReturnInvoiceDAL.ProductInfoDAL_Old
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblUnitPriceRollback

- Modules (pages reaching it via inline SQL): M04
- Read by (0): 
- Written by (1): ProductUnitPriceDAL.MoveUnitPriceToRollback
- Columns observed (15): ActiveDate, CostPrice, InActiveDate, IsActive, MRPPrice, PackSize, ProductCode, ProductId, ProductName, RollbackBy, RollbackDate, UnitPrice, UnitPriceId, VATAmountPerUnit, VATPercentage
- ALTERs in repo scripts: ADD RollbackId INT (SP_Create_tblUnitPriceRollback.sql:14)

### tblUnitPriceUpdate

- Modules (pages reaching it via inline SQL): none via pages (SP-only or helper use)
- Read by (0): 
- Written by (1): ProductUnitPriceDAL.SaveProductUnitPriceUpdate
- Columns observed (11): ActiveDate, CostPrice, PackSize, ProductCode, ProductId, ProductName, UnitPrice, UnitPriceId, UnitPriceUpdateId, VATAmountPerUnit, VATPercentage
- ALTERs in repo scripts: none

### tblUser

- Modules (pages reaching it via inline SQL): M01, M02, M06, M07, M09, M10, M11, M12, M13, M15
- Read by (53): AccountSettingsDal.GetUserOnDropDownList, AccountSettingsDal.UserInformationEditLoad, AppSetupDAL.LoadAppSetup, AppSetupDAL.LoadUser, AuditReportOneDal.GetDeleteOrderNationalReport, AuditReportOneDal.GetDeleteOrderReport …
- Written by (5): AccountSettingsDal.UpdateUserInfo, ChangePasswordDAL.UpdatePass, UserDAL.DeleteUserInfo, UserDAL.SaveUserInformation, UserDAL.UpdateUserInfo
- Columns observed (12): ContactNo, Email, IsPasswordChange, LoginName, Password, UpdateBy, UpdateDate, UserCode, UserId, UserName, UserStatus, UserType
- ALTERs in repo scripts: none

### tblUserCompanyUnit

- Modules (pages reaching it via inline SQL): M01, M02, M03, M05, M06, M07, M08, M09, M10, M11, M12, M13, M17
- Read by (16): ChallanReportViewDAL.DCLoad, CompanywisebranchDal.LoadCompany, CustPaymentDAL.LoadSC, DCStockReportDAL.LoadCompanyUnit, DCUserPermissionDal.CheckPermissionInfoAlreadyExistOrNot, DCUserPermissionDal.DeletePermissionInfo …
- Written by (3): DCUserPermissionDal.DeletePermissionInfo, DCUserPermissionDal.SaveDCUserPermissionInfo, UserDAL.SaveUserCompanyUnit
- Columns observed (5): CWHPermission, CompanyUnitId, NationalReportPermission, UserComId, UserId
- ALTERs in repo scripts: none

### tblUserSessionTracking

- Modules (pages reaching it via inline SQL): M01
- Read by (0): 
- Written by (4): UserSessionTrackingManager.CloseExistingActiveSession, UserSessionTrackingManager.CloseTrackingRecord, UserSessionTrackingManager.InsertTrackingRecord, UserSessionTrackingManager.UpdateLastActivity
- Columns observed (13): BrowserName, City, Country, DeviceType, IPAddress, IsActiveSession, LastActivityTime, LoginDateTime, LogoutDateTime, OSName, RegionState, SessionId, UserId
- ALTERs in repo scripts: none

### tblUserType

- Modules (pages reaching it via inline SQL): M01, M02, M15
- Read by (3): ChangePasswordDAL.GetUserPassInfoDAL, RSMSetupDal.Get_UserListInfo, UserDAL.LoadUserType
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblWearhouse

- Modules (pages reaching it via inline SQL): M02, M06, M07, M10, M11, M12
- Read by (14): ChallanReportViewDAL.WareHouseLoad, DCUserPermissionDal.GetDCList, MaterialConversionReceiveDal.LoadStockInMasterForEdit, RequisitionDAL.WareHouseLoad, SCtoWHTransferDal.WareHouseInfoLoad, SCtoWHTransferDal.WareHouseLoad …
- Written by (0): 
- Columns observed (0): 
- ALTERs in repo scripts: none

### tblWHAdjustmentDetail

- Modules (pages reaching it via inline SQL): M10
- Read by (3): WHStockAdjDAL.DeleteStockWHAdjustment, WHStockAdjDAL.GetWhStockAdjustmentListReportInformation, WHStockAdjDAL.LoadWHStockAdjustmentById
- Written by (2): WHStockAdjDAL.DeleteStockWHAdjustment, WHStockAdjDAL.SaveWHStockAdjDetail
- Columns observed (3): Quantity, ReceiveId, WHStockAdjId
- ALTERs in repo scripts: none

### tblWHStockAdjustment

- Modules (pages reaching it via inline SQL): M10
- Read by (5): WHStockAdjDAL.DeleteStockWHAdjustment, WHStockAdjDAL.GetTransNo, WHStockAdjDAL.GetWhStockAdjustmentListReportInformation, WHStockAdjDAL.LoadWHStockAdjustment, WHStockAdjDAL.LoadWHStockAdjustmentById
- Written by (2): WHStockAdjDAL.DeleteStockWHAdjustment, WHStockAdjDAL.SaveWHStockAdjMaster
- Columns observed (10): ActionStatus, AdjustmentType, EntryBy, EntryDate, FromStore, Remarks, StockEffect, TransactionDate, TransactionNo, toStore
- ALTERs in repo scripts: none

### tblWhStockConditionFreeze

- Modules (pages reaching it via inline SQL): M10, M11
- Read by (0): 
- Written by (1): WhStockConditionFreezeDal.SaveforWh
- Columns observed (6): EntryBy, EntryDate, FreezeQty, ManufacId, ReceiveId, WhStockConditionFreezeID
- ALTERs in repo scripts: none

### tblWHStockInDetail

- Modules (pages reaching it via inline SQL): M07, M10, M17
- Read by (17): ChallanReportViewDAL.ChallanDetailReportQuery, ChallanReportViewDAL.ChallanSummaryReportQuery, DCStockReportDAL.DCReportDetailDataDAL, DCStockReportDAL.WHReportDetailDataDAL, ExcelUpForMIGODAL.LoadMigoReport, MaterialConversionReceiveDal.LoadMaterialConversionList …
- Written by (4): WarehouseStockInDal.DeleteWhStockInDetailById, WarehouseStockInDal.DeleteWhStockInInfoById, WarehouseStockInDal.SaveWarehouseStockInDetail, WarehouseStockInDal.SaveWarehouseStockInDetailWithMaterialConversion
- Columns observed (10): Batch, ExpDate, MaterialConversionDetailId, MfgDate, Price, ProductId, Qty, TotalAmount, VAT, WHStockInMasterID
- ALTERs in repo scripts: none

### tblWHStockInMaster

- Modules (pages reaching it via inline SQL): M10
- Read by (15): MaterialConversionReceiveDal.GetMaterialConversionReceiveData, MaterialConversionReceiveDal.LoadMaterialConversionList, MaterialConversionReceiveDal.LoadMaterialConversionListForEdit, MaterialConversionReceiveDal.LoadStockInDetailForEdit, MaterialConversionReceiveDal.LoadStockInMasterForEdit, WHStockInReportDal.GetWarehouseStockInData …
- Written by (6): WarehouseStockInApprovalDal.ApprovalUpdateDal, WarehouseStockInDal.DeleteWhStockInInfoById, WarehouseStockInDal.SaveStockInMasterInformation, WarehouseStockInDal.SaveStockInMasterInformationForMaterialConversion, WarehouseStockInDal.UpdateStockInMasterInfo, WarehouseStockInDal.UpdateStockInMasterInfoForMaterialConversion
- Columns observed (19): ( WHStockInMasterID, ChallanDate, ChallanNo, EntryBy, EntryDate, ManufacId, MaterialConversionMasterId, ReferenceDate, ReferenceNo, Remarks, Status, SupplierId, TotalQuantity, TotalValue, TotalVat, WHStockInCode, WHStockInDate, WHStockInMasterID, WarehouseId
- ALTERs in repo scripts: none

### tblWHStockOutDetail

- Modules (pages reaching it via inline SQL): M10
- Read by (0): 
- Written by (1): WHStockListDAL.SaveWarehouseStockOutDetail
- Columns observed (5): ProductId, Qty, ReceiveId, WHStockInDetailID, WHStockOutMasterID
- ALTERs in repo scripts: none

### tblWHStockOutMaster

- Modules (pages reaching it via inline SQL): M10
- Read by (1): WHStockListDAL.WarehouseStockInCodeId
- Written by (1): WHStockListDAL.SaveStockOutMasterInformation
- Columns observed (8): EntryBy, EntryDate, ManufacId, Reason, WHStockInMasterID, WHStockOutCode, WHStockOutDate, WHStockOutMasterID
- ALTERs in repo scripts: none

### tblWhStoreFreeze

- Modules (pages reaching it via inline SQL): M10, M11
- Read by (3): DCStockReportDAL.WHReportDetailDataDAL, WhFreezeStockReleaseDal.GetStockReleaseData, WhFreezeStockReleaseDal.GetWhFreezeStockData
- Written by (2): WhFreezeStockReleaseDal.UpdateWhStoreFreezeStockQuantity, WhStockConditionFreezeDal.SaveWhStoreFreeze
- Columns observed (14): BatchNo, DamageQty, ExpDate, PackSize, ProductId, ProductName, ReceiveDate, ReceiveId, Remarks, StockCondition, StockQty, StockRcvDate, TotalQuantity, WhStockConditionFreezeID
- ALTERs in repo scripts: none

### tblZone

- Modules (pages reaching it via inline SQL): M03, M13
- Read by (6): DistrictDAL.LoadZoneByComUnit, ZoneInfoDAL.HasZoneName, ZoneInfoDAL.LoadZoneInfo, ZoneInfoDAL.ZoneEditLoad, ZoneSalesReportDAL.LoadZone, ZoneSalesReportDAL.ZoneReportMainDataDAL
- Written by (2): ZoneInfoDAL.SaveZoneInfo, ZoneInfoDAL.UpdateZoneInfo
- Columns observed (5): ComUnitId, ComUnitName, ZoneCode, ZoneId, ZoneName
- ALTERs in repo scripts: none

<!-- GENERATED-INDEX-END: tables -->

