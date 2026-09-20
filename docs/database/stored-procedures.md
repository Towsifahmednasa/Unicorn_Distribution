# Stored procedures - bodies present in the repository (Part A)

Scope: the **only** stored-procedure source in the repository is 20 SQL scripts (17 at the repo root, 3 in `Library.DAL/SAP_IntegrationDAL/sql/`). They define **41 distinct stored procedures**; ~1,000 other SPs called by the C# code have no body in the repo (see the generated index appended at the end of this file). Every line of all 20 scripts was read for this document. Confidence tags: CONFIRMED = read in the script, INFERRED = strongly implied by several references, UNKNOWN = not verifiable from the repo. Documentation only - nothing was modified.

Databases referenced by the scripts: `SalesDisDB_Unicorn_DB` (sales/inventory; the `USE` target), `CSTL_ERPDB_Unicorn` (ERP master: company info, GL mapping `Table_Integration_AccountsConnectivity`, `tbl_AccIntegrationTable`), `CSTL_ACCDB_Unicorn` (accounts: vouchers, chart of accounts, financial year, subsidiary/reference info), `SAP_API_Data` (SAP staging: `tblSAP_StockMovementMaster/Detail`) and, in one script, an un-suffixed `CSTL_ERPDB`. The header of `SP_SalesReturn_Related.sql` says the target is "SalesDisDB" (no `_Unicorn_DB` suffix). The scripts never define the linking, permissions or synonyms - cross-database access is by 3-part names (`db.dbo.table` / `db..table`).

## Script inventory (all 20 read)

| # | Script | Lines | Contents |
|---|---|---|---|
| 1 | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql` | 1430 | 4 ALTER TABLE blocks; 13 SPs (A.1, A.2, ledger in A.4). "Generated: 31-Aug-2026" |
| 2 | `Live_Deployment_Script_All_Updates.sql` | 1248 | schema (5 columns, 1 GL mapping row); 7 SPs + `spInsertReturnInvoice_new`. "Date: 06-Sep-2026" |
| 3 | `SP_SalesReturn_Related.sql` | 228 | `spInsertReturnInvoice_new` (older), `sp_UpdateAndInsertInvoiceDetailSalesReturn` |
| 4 | `SP_Update_StockValuation_TransactionDate_Logic.sql` | 486 | `sp_Report_DCStore_StockValuationLedger` (newer), 2 national-stock report SPs |
| 5 | `SP_NewStockTransferDcToDc_Related.sql` | 437 | 8 SPs for B2B/SAP challan (dated 2026-09-01) |
| 6 | `SP_Fix_STO_Chalan_Process.sql` | 68 | `sp_Del_B2BDelete` v2 (dated 2026-09-02) |
| 7 | `SP_Fix_CentralStoreStoIssue.sql` | 192 | `tbl_INV_CentralStoreStoIssue` v1 |
| 8 | `SP_Fix_CentralStoreStoIssue_Corrected.sql` | 215 | `tbl_INV_CentralStoreStoIssue` v2 |
| 9 | `SP_Adjust_DCStore_Stock_Only.sql` | 135 | `sp_Adjust_DCStore_Stock_Only` |
| 10 | `sp_Check_DCStore_StockAnomalies.sql` | 94 | `sp_Check_DCStore_StockAnomalies` |
| 11 | `sp_Delete_ProformaInvoice_Update.sql` | 206 | `ALTER PROCEDURE sp_Delete_ProformaInvoice` |
| 12 | `SP_Fix_Invoice_Anomaly_And_Sync.sql` | 82 | data UPDATE of `tblInvoice` + `ALTER PROCEDURE sp_Check_anomalyInvoiceDetails` |
| 13 | `SP_Audit_Sales_Accounts_Voucher_Posting.sql` | 436 | `sp_Audit_Sales_Accounts_Voucher_Posting` (03-Sep-2026) |
| 14 | `SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql` | 56 | `sp_Get_MoneyReceiptReportAfterPaymentList` |
| 15 | `SP_Create_tblUnitPriceRollback.sql` | 22 | creates table `tblUnitPriceRollback` (no SP) |
| 16 | `SP_Fix_CustomerPayment_Live.sql` | 24 | ALTER TABLE `tblCustomerPay` ADD `Remarks` (no SP) |
| 17 | `Create_Order_ODR-20260901134.sql` | 104 | one-off data repair of order `ODR-20260901134` (no SP) |
| 18 | `Library.DAL/SAP_IntegrationDAL/sql/alter_tblProviderDropoutIntrigration_AddApprovalColumns.sql` | 21 | ALTER TABLE (3 columns; no SP) |
| 19 | `Library.DAL/SAP_IntegrationDAL/sql/sp_Get_ProviderDropoutIntrigrationList.sql` | 30 | `sp_Get_ProviderDropoutIntrigrationList` |
| 20 | `Library.DAL/SAP_IntegrationDAL/sql/sp_Up_ProviderDropoutIntrigrationApprove.sql` | 22 | `sp_Up_ProviderDropoutIntrigrationApprove` |

Deployment-order hazards (CONFIRMED): the scripts have no ordering metadata; `sp_SalesReturn_Related`, `SP_NewStockTransferDcToDc_Related`, `SP_Fix_CentralStoreStoIssue*` and `SP_Update_StockValuation...` contain `DROP`+`CREATE` of objects that other, later scripts also (re)define. Running the "related"/older scripts after the "fix"/newer ones silently **downgrades** the objects (see "Duplicates and conflicts").

## Catalogue (41 SPs)

| SP | Script(s):line | Area |
|---|---|---|
| sp_Get_CustomerApp | LIVE_DB_UPDATE:222 | customer approval worklist |
| sp_Save_CustomerMaster | LIVE_DB_UPDATE:403 | customer insert |
| sp_UD_CustomerMaster | LIVE_DB_UPDATE:499 | customer legacy update (no caller) |
| sp_Update_CustomerMaster | LIVE_DB_UPDATE:550 | customer update |
| sp_Save_DoctorMaster | LIVE_DB_UPDATE:672 | doctor insert |
| sp_Update_DoctorMaster | LIVE_DB_UPDATE:735 | doctor update |
| sp_Get_CustMasterList_Approve | LIVE_DB_UPDATE:821 | customer list |
| sp_Get_DoctorList | LIVE_DB_UPDATE:884 | doctor list |
| sp_Save_ManualOrderMaster | LIVE_DB_UPDATE:940 | manual order header |
| sp_Update_ManualOrderMaster | LIVE_DB_UPDATE:1115 | manual order header update |
| sp_Save_ManualOrderDetail | LIVE_DB_UPDATE:1376 | manual order line |
| sp_LoadOrderListForOrderCreationbyTerri | LIVE_DB_UPDATE:1225 | invoice-creation queue + credit check |
| sp_Report_DCStore_StockValuationLedger | LIVE_DB_UPDATE:52 **and** SP_Update_StockValuation:19 (2 versions) | DC stock ledger |
| sp_Report_NationalStockValuation_DateRange | SP_Update_StockValuation:195 | national stock |
| sp_Report_NationalStockValuation_BatchDetails | SP_Update_StockValuation:354 | national stock drill-down |
| sp_Report_CustomerInvoiceAgingOutstanding | Live_Deployment:129 | AR ageing |
| sp_GET_PaymentInvSP | Live_Deployment:212 | payable invoices |
| sp_GET_PaymentInvSPTPVATAmt | Live_Deployment:296 | TP/VAT split |
| sp_GET_PaymentInvSPPaymentAmount | Live_Deployment:340 | payment grid |
| sp_Sales_CustomerPaymentPosting | Live_Deployment:403 | payment -> accounts voucher |
| sp_DeliverySalesReturnPosting | Live_Deployment:558 | delivery return -> vouchers |
| sp_SalesReturnPosting | Live_Deployment:949 | market return -> vouchers |
| spInsertReturnInvoice_new | Live_Deployment:857 **and** SP_SalesReturn_Related:19 (2 versions) | return header |
| sp_UpdateAndInsertInvoiceDetailSalesReturn | SP_SalesReturn_Related:174 | return detail (snd) |
| sp_Delete_ProformaInvoice | sp_Delete_ProformaInvoice_Update:8 | proforma delete |
| sp_Check_anomalyInvoiceDetails | SP_Fix_Invoice_Anomaly_And_Sync:43 | invoice sanity |
| sp_Adjust_DCStore_Stock_Only | SP_Adjust_DCStore_Stock_Only:13 | stock repair |
| sp_Check_DCStore_StockAnomalies | sp_Check_DCStore_StockAnomalies:11 | stock diagnostics |
| sp_Audit_Sales_Accounts_Voucher_Posting | SP_Audit_...:23 | accounts audit |
| sp_Get_MoneyReceiptReportAfterPaymentList | SP_Create_sp_Get_MoneyReceipt...:18 | money receipt |
| sp_Del_B2BDelete | SP_NewStockTransferDcToDc_Related:23 **and** SP_Fix_STO_Chalan_Process:25 (2 versions) | B2B challan delete |
| sp_DeleteDwpotToWhChalan | SP_NewStockTransferDcToDc_Related:72 | depot->WH challan delete |
| sp_SAP_B2BStockReceiveByDc | SP_NewStockTransferDcToDc_Related:90 | B2B receive |
| sp_SAP_B2BChallanMasterInsert | SP_NewStockTransferDcToDc_Related:182 | SAP challan header |
| sp_SAP_B2BChallanDetailInsert | SP_NewStockTransferDcToDc_Related:277 | SAP challan detail + stock deduct |
| sp_SAP_ChallanSendDetailByChalanId | SP_NewStockTransferDcToDc_Related:381 | SAP payload |
| sp_SAP_Up_SAP_ChallanSendByChalanId | SP_NewStockTransferDcToDc_Related:409 | SAP flag |
| sp_SAP_Up_SAP_ChallanConfirmByChalanId | SP_NewStockTransferDcToDc_Related:428 | SAP flag |
| tbl_INV_CentralStoreStoIssue | SP_Fix_CentralStoreStoIssue:12 **and** ..._Corrected:16 (2 versions) | central-store STO issue |
| sp_Get_ProviderDropoutIntrigrationList | SAP_IntegrationDAL/sql/...List.sql:7 | provider dropout list |
| sp_Up_ProviderDropoutIntrigrationApprove | SAP_IntegrationDAL/sql/...Approve.sql:7 | provider dropout approve |

## Duplicates and conflicts (summary)

| SP | Versions | Newer | Conflict |
|---|---|---|---|
| sp_Report_DCStore_StockValuationLedger | `LIVE_DB_UPDATE...:52` vs `SP_Update_StockValuation...:19` | second (INFERRED) | first ranks all rows by `StockRcvDate`; second dates issues by `ReferenceTransactionDate` (transaction date). Same name/params/columns; last run wins. |
| spInsertReturnInvoice_new | `SP_SalesReturn_Related.sql:19` vs `Live_Deployment...:857` | second (dated 06-Sep-2026; superset) | older lacks `@TpGrandTotal/@DeductionCharge/@NetReturnAmount`; C# `BonusCampaignNewDAL.cs:468-470` **passes** them -> the older SP would fail with "not a parameter" and lose deduction data. |
| sp_Del_B2BDelete | `SP_NewStockTransferDcToDc_Related.sql:23` (2026-09-01) vs `SP_Fix_STO_Chalan_Process.sql:25` (2026-09-02) | second (dated) | v1 adds qty back to source rows in place and has no transaction; v2 deletes ledger Issue/Receive rows in a transaction. Opposite stock models. |
| tbl_INV_CentralStoreStoIssue | `SP_Fix_CentralStoreStoIssue.sql:12` vs `..._Corrected.sql:16` | second (INFERRED from name/`USE`/idempotency) | v2 swaps the meaning of `Weighted_Avg_Id` / `Issue_Weighted_Avg_Id` in the batch row and makes re-runs idempotent. |
| sp_Delete_ProformaInvoice | body only in `sp_Delete_ProformaInvoice_Update.sql`; older body only referenced in comments (`SP_Adjust_DCStore_Stock_Only.sql:103`) | this file | older added quantity to Receive rows (inflation defect). |
| sp_Check_anomalyInvoiceDetails | body only in `SP_Fix_Invoice_Anomaly_And_Sync.sql` (ALTER) | this file | older body (no `AdjustAmount`) not in repo. |

## Cross-cutting observations (CONFIRMED unless noted)

1. **Transactions:** only 2 of 41 SPs use TRY/CATCH + transaction (`sp_Adjust_DCStore_Stock_Only`, `sp_Del_B2BDelete` v2; the one-off `Create_Order...sql` uses a bare `BEGIN TRANSACTION` with no error handling). The multi-table writers (`sp_Delete_ProformaInvoice`, `sp_Update_CustomerMaster`, `sp_Update_DoctorMaster`, the posting SPs, `tbl_INV_CentralStoreStoIssue`, the SAP challan SPs) have none.
2. **Dynamic SQL with un-parameterised concatenation** (injection surface if C# does not sanitise): `sp_Get_CustomerApp`, `sp_Get_CustMasterList_Approve`, `sp_Get_DoctorList`, `sp_GET_PaymentInvSP`, `sp_GET_PaymentInvSPPaymentAmount` (also `@CollectionBy`, `@PaymentAmount`), `sp_Get_MoneyReceiptReportAfterPaymentList`, `sp_Report_NationalStockValuation_DateRange`.
3. **Non-atomic key generation** (`MAX(id)+1`): customer/doctor codes, order code `ODR-yyyymmdd<MAX(OrderId)+1>`, return number `RIN--yymmdd<MAX(ReturnInvoiceId)+1001>`, `DCStoreId`, `ChalanId`, `ChalanDetailsId` (`sp_SAP_B2B*`), voucher codes.
4. **Two stock models coexist:** ledger (negative Issue rows) vs in-place decrement of Receive rows. Scripts that assume one break the other (`sp_SAP_B2BChallanDetailInsert`, `sp_Del_B2BDelete` v1 vs the C# `ChalanDAL`, `sp_Delete_ProformaInvoice`, `sp_Adjust_DCStore_Stock_Only`).
5. **Hard-coded IDs / fallbacks:** `EntryBy='admin'`, `CompanyId 1`, financial-year fallback `2`, GL fallbacks `4170, 5968, 2112, 4156, 4162`, mapping row `PageId 23 / FlagId 1032 / GLId 5968`, default `CompanyId 16` in `sp_GET_PaymentInvSP`, cut-off date `2022-11-16`, unit price/VAT default `1`.
6. **Voucher key collision:** `TableID=2018` is used for both `tblReturnInvoice.ReturnInvoiceId` and `tblCustomerReturnMaster.CustReturnId`.
7. **Status vocabularies seen:** customer/doctor `ActionStatus/ApprovalStatus` '0' Pending, '1' Verified, '2' Approved, '3' Rejected; order `ActionStatus` '2' (also 'Accepted','Approved'); credit-limit `ActionStatus='Approved'`; invoice `PaymentStatus` NULL/'Partial'/...; `PaymentInvoiceStatus` 'Reject'; `DeliveryInvoiceStatus` 'Full'/'Partial'; voucher `ActionStatus='Approved'` (auto-posted, already approved).

## Stored-procedure sections

## A.1 Customer / doctor master SPs (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`)

Script conventions (CONFIRMED, file-wide): every SP is `IF OBJECT_ID(...) DROP` + `CREATE` (re-runnable, but a drop/create window exists and grants on the old object are lost); none uses `SET XACT_ABORT`, `BEGIN TRAN`, `TRY/CATCH` or `RAISERROR/THROW` unless stated. Header says "Generated: 31-Aug-2026" (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:3`).

### sp_Get_CustomerApp
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:222-392` (single version). CONFIRMED.
- **Purpose:** Customer-approval worklist: lists `tblCustMaster` rows with their latest approval-log step (`tblCustomerApprovalLog`), the field-force hierarchy of the creator and a duplicate-cell-number flag.
- **Parameters:** `@param NVARCHAR(MAX)=NULL` (raw SQL fragment appended to the query), `@Role NVARCHAR(MAX)=NULL`, `@AppStatus NVARCHAR(MAX)=NULL` (comma-list of quoted statuses inserted verbatim into `IN (...)`), `@FromDt DATETIME=NULL`, `@ToDt DATETIME=NULL`, `@EmpId INT=NULL` (declared, never used), `@TerritoryId INT=NULL`.
- **Reads:** `tblCustMaster` (CustomerMasterId, CustomerName, CustomerCode, CellNo, Address, ActionStatus, CreateDate, CreateBy, SalesPersonId, CustomerTypeId, ProgramTypeId, DistributionRouteId, MarketId, SMCTypeId), `tblEmpGeneralInfo` (twice: as EGI on SalesPersonId and unaliased as creator), `tblCustomerType`, `tblProgramType`, `tblDistributionRoute`, `tblMarket`, `tblSubTerritory`, `tblTerritory`, `tblSMCType`, `tblRouteInformationMarketDetail`, `tblRouteInformationMaster`, `tblCustomerApprovalLog` (TableId, Step, Status, EntryDateApp, FromEmpId, ToEmpId, Comments, Type, GroupId/RegionId/AreaId/TerritoryId, RoleTypeId, ToRoleTypeId, CustomerApprovalId, Date), `tblUser`, `tblRoleType` (twice), view `View_Webapi_EmployeeFieldForceInfo` (TerritoryId/AreaId/RegionId/GroupId + names/codes; MIOEmpId, ASMEMPId, RSMEMPId, NSMEMPId). Writes: none.
- **Business rules:** rows with `ActionStatus` in ('2','3') (Approved/Rejected) are excluded (`:235`) - it is a pending/in-flight list. Status text map: '0'=Pending, '1'=Verified, '2'=Approved, '3'=Rejected (`:288-294`). `ExistenceStatus`='Already Exists' when the same non-empty `CellNo` occurs on more than one non-rejected customer (CTE `DupCell`, `:272-278`). Latest log step = `MAX(Step)` per `TableId` (`:364-371`); `WaitingForRole` = role of `ToRoleTypeId` else 'Pending Approval'. Date filter: one date -> equality, two dates -> BETWEEN; when `@AppStatus` is given the date applies to `ISNULL(log.EntryDateApp, mas.CreateDate)`, otherwise to `mas.CreateDate`. `@Role` excludes rows whose last log role equals `@Role`.
- **Dynamic SQL / injection:** whole query built by string concatenation and run via `sp_executesql` without parameters (`:390`). `@AppStatus`, `@Role`, `@param` are injected verbatim (`:239, :383, :385`) -> SQL-injection surface unless the C# layer sanitises. CONFIRMED (SP side).
- **Result set:** SalesPersonName, ExistenceStatus, SMCType, WaitingForRole, ApprovalStatus, CellNo, Address, ApprovalStatusWeb, ProgramTypeName, MarketCode, MarketName, CustomerType, DistributionRouteName, EmpInfoId, CustomerMasterId, CustomerName, CustomerCode, EntryDate (`dd MMM yyyy hh:mm tt`), CustomerApprovalId, Date, FromEmpId, ToEmpId, TableId, Status, Comments, Type, Step, log GroupId/RegionId/AreaId/TerritoryId/RoleTypeId, ToRoleTypeId, EmpMasterCode, EmpName, view TerritoryId/AreaId/RegionId/GroupId/TerritoryName/TerritoryCode/AreaCode/AreaName/RegionCode/RegionName/GroupName, MIOEmpId, ASMEMPId, RSMEMPId, NSMEMPId, MaxStep, WaitingRole, WatingEmployee (constant '').
- **Callers:** `Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:800` `CustomerInfoDAL.GetCustomer_ApplogDAL`. Page ids not resolved here (see `specs/*/pages/`).
- **Risks:** duplicate result column names (TerritoryId, AreaId, RegionId, GroupId returned twice: log + view); joins `tblRouteInformationMarketDetail/Master` (DCdtl, dcMas) are never selected and can multiply rows when a market maps to several routes.

### sp_Save_CustomerMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:403-488` (single version). CONFIRMED.
- **Purpose:** Insert a new customer master. New customers are created already Approved (`ActionStatus='2'` hard-coded, `:483`); the `@ActionStatus` parameter is accepted but ignored.
- **Parameters (all defaulted NULL except first):** `@CustomerMasterId int` (unused on insert), `@CustomerName, @Address, @CellNo nvarchar(max)`, `@MarketId int`, `@EmpInfoId int`, `@MarketName, @IsActive bit`, `@CustomerStation, @Division, @District, @Thana, @CustomerType nvarchar(max)`, `@CustomerTypeId, @DistrictId, @DivisionId, @ThanaId, @StationTypeId, @DistributionRouteId int`, `@OwnerName, @VoterID, @TradeLicense, @DrugLicense, @PharmacyCouncilCertificate, @BCDS, @ActionStatus, @Email, @Reamrks nvarchar(max)`, `@EntryBy int`, `@ProgramTypeId, @ComUnitId, @NSMStationTypeId, @DZSMStationTypeId int`, `@TaggedSalesPersonIds nvarchar(500)`, `@TaggedSalesPersonCodes nvarchar(500)`.
- **Reads:** `tblMarket`, `tblSubTerritory`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Thana`, `tbl_District`, `tbl_Division` (derives Division/District/Thana/SubTerritory/Territory/Area/Region/Group from `@MarketId`, overwriting caller-supplied `@DivisionId/@DistrictId/@ThanaId`), `tblMarketStationDetail` (StationTypeId per `UserRoleID` 1=MIO, 2=NSM, 3=DZSM, overwriting the station-type parameters), `tblCompanyUnit` (ComUnitName/ComUnitCode for `@ComUnitId`), `tblCustMaster` (`MAX(CustomerCode)`).
- **Writes:** INSERT `tblCustMaster`(CustomerName, Address, CellNo, MarketId, MarketName, IsActive, CustomerStation, Division, District, Thana, CustomerType, CustomerTypeId, DistrictId, DivisionId, ThanaId, StationTypeId, CreateBy=@EntryBy, CreateDate=GETDATE(), DistributionRouteId, OwnerName, VoterID, TradeLicense, DrugLicense, PharmacyCouncilCertificate, BCDS, ActionStatus='2', Email, Reamrks, ProgramTypeId, CustomerCode, GroupId, RegionId, AreaId, TerritoryId, SubTerritoryId, NSMStationTypeId, DZSMStationTypeId, ComUnitId, ComUnitName, ComUnitCode, SalesPersonId=@EmpInfoId, TaggedSalesPersonIds, TaggedSalesPersonCodes).
- **Business rules:** customer code = `'C' + (MAX(numeric tail of CustomerCode among ActionStatus='2') + 1)` (`:462-463`); if no approved customer exists MAX is NULL and the code becomes NULL. `@TaggedSalesPersonIds` defaults to `CAST(@EmpInfoId AS NVARCHAR(50))` when NULL (`:465-468`).
- **Transaction/error handling:** none. Code generation is not atomic: two concurrent saves can read the same MAX and create duplicate `CustomerCode` (no unique constraint is visible in the repo -> UNKNOWN). `CONVERT(INT, SUBSTRING(CustomerCode,2,...))` fails on any approved code with a non-numeric tail.
- **Result set:** `SELECT SCOPE_IDENTITY()` (new `CustomerMasterId`, decimal).
- **Code-format note:** the generator emits `C<n>` (e.g. `C135`) while the one-off script `Create_Order_ODR-20260901134.sql:53` references customer `C0000134` (zero-padded) - existing and newly generated customer codes evidently use different formats (INFERRED); the numeric-tail MAX therefore yields `C135` after `C0000134`.
- **Callers:** `CustomerInfoDAL.SaveInfo` (`Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs:349`, via `SaveDataReturnPrimaryKey`), `OrderTrackingDAL.SaveInfo` (`Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs:244`), `MarketStructureTransferDAL.SaveInfo` (`Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs:723`).
- **Risks:** column `TaggedSalesPersonCodes` is inserted here but no script adds it (only `TaggedSalesPersonIds` is added, `:7-19`) - the deployment depends on the column pre-existing (UNKNOWN). `@ActionStatus` ignored, so the approval workflow is bypassed on insert. No duplicate-cell-number check although `sp_Get_CustomerApp` reports duplicates.

### sp_UD_CustomerMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:499-540`. CONFIRMED. No caller in `Library.*` / `Solution.Web` -> UNKNOWN whether used by an external client/mobile API.
- **Purpose:** Update legacy/SAP-oriented customer attributes plus the tagged sales-person lists.
- **Parameters:** `@CustomerMasterId INT` (required), `@CustomerCode, @CategoryId INT, @CustomerName, @Address, @CellNo, @Addrees2, @City, @ShippingCond, @MarketCode, @MarketName, @MIACode, @MiaName, @AreaCode, @DisCode, @FEName, @ComUnitCode, @ComUnitName, @RegionCode, @DZSMName, @TermOfPayment, @ConPerson` (NVARCHAR(MAX)=NULL), `@FixedCustomer bit` (required), `@LoginName`, `@IsActive bit` (required), `@InActiveDate datetime` (required, never used), `@TaggedSalesPersonIds nvarchar(500)=NULL`, `@TaggedSalesPersonCodes nvarchar(500)=NULL`.
- **Writes:** UPDATE `tblCustMaster` SET ConPerson, FixedCustomer, IsActive, CategoryId, CustomerName, Address, CellNo, Addrees2, City, ShippingCond, MarketCode, MarketName, MIACode, AreaCode, DisCode, FEName, ComUnitCode, ComUnitName, RegionCode, DZSMName, TermOfPayment; TaggedSalesPersonIds/Codes = `ISNULL(@param, existing)` (`:536-537`). `@CustomerCode/@MiaName/@LoginName/@InActiveDate` are ignored.
- **Risks:** all non-tagged columns are overwritten unconditionally (NULL parameter wipes the column); no existence check; no transaction; no audit columns (UpdateBy/UpdateDate not set).

### sp_Update_CustomerMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:550-662`. CONFIRMED.
- **Purpose:** Update an existing customer from the customer-master page; re-derives the geographic hierarchy from `@MarketId`; clears the customer's product-line links.
- **Parameters:** same as `sp_Save_CustomerMaster` but `@UpdateBy int` replaces `@EntryBy`.
- **Reads:** `tblMarket`, `tblSubTerritory`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Thana`, `tbl_District`, `tbl_Division`, `tblMarketStationDetail` (UserRoleID 1/2/3).
- **Writes:** UPDATE `tblCustMaster` (statement 1): name/address/cell, GroupId/RegionId/AreaId/TerritoryId/SubTerritoryId (from market), MarketId/MarketName, IsActive, CustomerStation, Division, District, Thana, CustomerType(+Id), DistrictId, DivisionId, ThanaId, StationTypeId, UpdateBy, UpdateDate=GETDATE(), SalesPersonId=@EmpInfoId, TaggedSalesPersonIds/Codes (overwritten, NOT ISNULL-merged unlike `sp_UD_CustomerMaster`), DistributionRouteId, OwnerName, VoterID, TradeLicense, DrugLicense, PharmacyCouncilCertificate, BCDS, Email, Reamrks, ProgramTypeId, NSMStationTypeId, DZSMStationTypeId. **DELETE `tblCustProductLine` WHERE CustomerMasterId** (`:644`) - product-line mappings are wiped on every update and must be re-inserted by the caller. Statement 2 re-stamps UpdateBy/UpdateDate, ThanaId/DistrictId/DivisionId and StationTypeId/NSMStationTypeId/DZSMStationTypeId from `tblMarket`/`tblMarketStationDetail` (`:658-660`).
- **Rules:** `@TaggedSalesPersonIds` defaults to `@EmpInfoId` when NULL. `ActionStatus`, `CustomerCode`, `ComUnit*` are NOT updated (`@ActionStatus`, `@ComUnitId` accepted but unused).
- **Transactions/errors:** none. UPDATE + DELETE + UPDATE are not atomic; a failure after the DELETE leaves the customer without product lines.
- **Result set:** none (caller uses `UpdateData`).
- **Callers:** `CustomerInfoDAL.SaveInfo` (`CustomerInfoDAL.cs:341`), `OrderTrackingDAL.SaveInfo` (`OrderTrackingDAL.cs:236`), `MarketStructureTransferDAL.SaveInfo` (`MarketStructureTransferDAL.cs:715`).
- **Risks:** if `@MarketId` matches no market (or an inner join fails) the hierarchy variables are NULL and GroupId/RegionId/... are set to NULL.

### sp_Save_DoctorMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:672-725`. CONFIRMED.
- **Purpose:** Insert a doctor master row (auto-approved).
- **Parameters:** `@DoctorId int` (required, unused), `@SecondaryCode nvarchar(max)`, `@DesignationId int`, `@Gender`, `@DoctorName nvarchar(max)`, `@IsActive bit`, `@Activedate datetime`, `@InactiveDate datetime`, `@EntryBy int`, `@DivisionId/@DistrictId/@ThanaId int`, `@UPCode nvarchar(max)`, `@DoctorTypeId int`, `@MarketId int`, `@UnionName, @Reamrks, @StreetAddress nvarchar(max)`, `@StationTypeId, @ProgramTypeId, @DoctorCategoryId, @SMCTypeId, @SalesPersonId int`, `@TaggedSalesPersonIds nvarchar(500)`, `@TaggedSalesPersonCodes nvarchar(500)` (defaults NULL).
- **Reads:** `tblDoctorMaster` (`MAX(DoctorCode)` where `ApprovalStatus='2'`). **Writes:** INSERT `tblDoctorMaster`(DoctorName, DoctorCode, SecondaryCode, DesignationId, Gender, IsActive, Activedate, EntryBy, EntryDate=GETDATE(), DivisionId, DistrictId, ThanaId, IsFromApp=0, ApprovalStatus='2', UPCode, DoctorTypeId, MarketId, UnionName, Reamrks, StationTypeId, ProgramTypeId, DoctorCategoryId, InactiveDate, SMCTypeId, SalesPersonId, TaggedSalesPersonIds, TaggedSalesPersonCodes, StreetAddress).
- **Rules:** code = `'D' + (MAX(numeric tail where ApprovalStatus='2') + 1)`; `ApprovalStatus` hard-coded '2' and `IsFromApp=0`; tagged ids default to `@SalesPersonId`. Result: `SELECT SCOPE_IDENTITY()`.
- **Transactions:** none; same MAX+1 race as customers. Same caveat: `tblDoctorMaster.TaggedSalesPersonCodes` is not added by any script (only `TaggedSalesPersonIds`, `:14-19`).
- **Callers:** `Library.DAL/MasterSetup_DAL/DoctorDAL.cs:164` `DoctorDAL.SaveInfo` (`SaveDataReturnPrimaryKey`).

### sp_Update_DoctorMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:735-811`. CONFIRMED.
- **Purpose:** Update a doctor and wipe child collections so the caller can re-insert them.
- **Parameters:** as save minus `@EntryBy`, plus required `@UpdateBy int`; `@DoctorId int` required.
- **Writes:** UPDATE `tblDoctorMaster` (DoctorName, SecondaryCode, DesignationId, Gender, IsActive, Activedate, UpdateBy, UpdateDate, StreetAddress, DivisionId, DistrictId, ThanaId, UPCode, DoctorTypeId, MarketId, UnionName, Reamrks, StationTypeId, ProgramTypeId, DoctorCategoryId, InactiveDate, SMCTypeId, SalesPersonId, TaggedSalesPersonIds/Codes). DELETE from `tblDoctorBrandDetail`, `tblDoctorDegreeDetail`, `tblDoctorSpecialityDetail`, `tblDoctorSpecialDayDetail`, `tblDoctorContactDetail` (by DoctorId, unconditional); DELETE `tblDoctorChemberDetail` for that DoctorId except chambers referenced in `tbl_PrescriptionMaster.ChemberId` or `tbl_DCRInfo.ChemberId` for this doctor (`:802-809`) - chambers with prescription/DCR history are protected.
- **Reads:** `tbl_PrescriptionMaster`, `tbl_DCRInfo`.
- **Risks:** `NOT IN (SELECT ChemberId ...)` deletes nothing if any subquery `ChemberId` is NULL (three-valued logic); multi-statement without transaction -> a mid-way failure leaves the doctor without brands/degrees/etc. `ApprovalStatus` is untouched.
- **Callers:** `DoctorDAL.SaveInfo` (`DoctorDAL.cs:156`, `UpdateData`).

### sp_Get_CustMasterList_Approve
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:821-874`. CONFIRMED.
- **Purpose:** Customer master listing for approval/maintenance grids with market, territory, route, SMC type and all tagged sales persons.
- **Parameters:** `@Parm nvarchar(max)=null` - raw WHERE-fragment appended after `where mas.CustomerMasterId is not null` (`:870`); a NULL `@Parm` makes `@Q` NULL (no ISNULL guard, unlike `sp_Get_DoctorList`).
- **Reads:** `tblCustMaster`, `tblCustomerType`, `tblProgramType`, `tblUser`, `tblEmpGeneralInfo` (x2), `tblMarket`, `tblMarketStationDetail` + `tblStationType` (UserRoleID=1), `tblRouteInformationMarketDetail` + `tblRouteInformationMaster` + `tblCompanyUnit` (Dept = `ComUnitCode : ComUnitName` of route DC), `tbl_Thana`, `tbl_District`, `tbl_Division`, `tblSubTerritory`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`, `tblSMCType`. Function: `dbo.Remove_SpecialCharacters(Address)` (body not in repo).
- **Rules:** `SalaesPersonName` (sic) = STUFF/`FOR XML PATH` concatenation of every employee whose `EmpInfoId` appears in the comma list `mas.TaggedSalesPersonIds` (`','+ids+',' LIKE '%,'+id+',%'`), falling back to the single `SalesPersonId` employee. Status text: 0 Pending, 1 Verified, 2 Approved, 3 Rejected. Search label appends ` (Inactive) ` when `IsActive<>1`. Ordered `CustomerCode desc` (string sort). Result is many derived columns plus `*` (all `tblCustMaster` columns) - duplicate column names possible.
- **Dynamic SQL:** yes, `@Parm` injected unparameterised.
- **Callers:** `CustomerInfoDAL.GetCustomerList` (`CustomerInfoDAL.cs:413`), `MarketStructureTransferDAL.GetMarketListBySubTerritoryId` (`MarketStructureTransferDAL.cs:787`).
- **Risks:** route master/detail joins can duplicate a customer per route; string sort of codes ('C9' > 'C10').

### sp_Get_DoctorList
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:884-929`. CONFIRMED.
- **Purpose:** Doctor listing with chambers, contacts, degrees, specialties, program types and tagged sales persons.
- **Parameters:** `@Parm nvarchar(max) = ''` (raw WHERE fragment; NULL coerced to '', `:889`).
- **Reads:** `tblDoctorMaster`, `tblDoctorDesignation`, `tblEmpGeneralInfo`, `tblProgramType`, `tblDoctorType`, `tblMarket`, `tblMarketStationDetail`/`tblStationType`, `tblRouteInformationMarketDetail`/`tblRouteInformationMaster`/`tblCompanyUnit`, `tblSubTerritory`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group`, `tblSMCType`, `tblUser` (x2: entry/update), `tblDoctorChemberDetail`, `tblDoctorChamber`, `tblDoctorContactDetail`, `tblDoctorDegree`+`tblDoctorDegreeDetail`, `tblDoctorSpeciality`+`tblDoctorSpecialityDetail`, `tblDoctorProgramType`+`tblDoctorProgramTypeDetail`.
- **Rules:** only real doctors: `isnull(DM.IsDoctor,0)=1` (`:924`). ApprovalStatus map as customers. Multi-valued columns via `STUFF(... FOR XML PATH(''))` (chamber strip `1,2`, contacts `1,1`). Ordered `DoctorCode DESC` (string sort). Dynamic SQL, `@Parm` injected verbatim.
- **Callers:** `DoctorDAL.GetDoctorList` (`DoctorDAL.cs:424`), `SetupDAL.Get_DoctorList` (`Library.DAL/DoctorModule_DAL/SetupDAL.cs:189`), `Setup2DAL.Get_DoctorList` (`Library.DAL/DoctorModule_DAL/Setup2DAL.cs:86`). (`sp_Get_DoctorList_Approval`, called at `SetupDAL.cs:3985` / `Setup2DAL.cs:4814`, is a different SP whose body is not in the repo.)

## A.2 Manual order SPs (`LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql`)

### sp_Save_ManualOrderMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:940-1104`. CONFIRMED. Related one-off data fix: `Create_Order_ODR-20260901134.sql` (see `tables.md`).
- **Purpose:** Header insert for a web-entered ("Web Manual") order, pre-approved and ready to invoice.
- **Parameters:** `@OrderCode NVARCHAR(50) OUTPUT` (in/out), `@TerritoryCode NVARCHAR(50)=NULL`, `@ComUnitId INT=NULL`, `@ComUnitCode NVARCHAR(50)`, `@ComUnitName NVARCHAR(200)`, `@MIOCode NVARCHAR(50)`, `@MIOName NVARCHAR(200)`, `@MIOId INT` (unused), `@ManufacId INT=1`, `@CustomerCode NVARCHAR(50)`, `@CustomerName NVARCHAR(200)`, `@GrossValue/@TotalVat/@TotalDiscount/@TotalNetPayable DECIMAL(18,4)=0`, `@SubmissionDate DATETIME=NULL`, `@FixedCustomer BIT=0`, `@Remarks NVARCHAR(MAX)`, `@OrderId INT OUTPUT`.
- **Reads:** `tblCustMaster` (TOP 1 by `CustomerCode = @CustomerCode`), `tblSMCType`, `tblMarket`, `tblSubTerritory`, `tblTerritory`, `tblArea`, `tblRegion`, `tbl_Group` (order-time snapshot of hierarchy ids/names), `tblOrder` (`MAX(OrderId)`).
- **Writes:** INSERT `tblOrder`(OrderCode, TerritoryCode, ComUnitId/Code/Name, MIOCode, MIOName, ManufacId, CustomerCode, CustomerName, GrossValue, TotalVat, TotalDiscount, TotalNetPayable, SubmissionDate, FixedCustomer, Remarks, OrderType='Regular', IsManual=1, IsFromApp=0, IsInvoice=0, IsPrepareforInvoice=1, IsSubDepo=0, ActionStatus='2', CustomerMasterId, MarketId, TerritoryId, AreaId, RegionId, GroupId, SubTerritoryId, CustTypeId, ProgramTypeId, DistributionRouteId, GroupName_Ord, RegionName_Ord, AreaName_Ord, TerritoryName_Ord, SubTerritoryName_Ord, MarketName_Ord, SmcTypeId_Ord, SMCType_Ord, OrderSenderType='Web Manual', EntryDate, ServerDateTime).
- **Business rules:** if `@OrderCode` is NULL/''/`LIKE 'ORD-000000%'` a code is generated `'ODR-' + yyyymmdd + CAST(MAX(OrderId)+1 AS NVARCHAR)` (`:999-1004`) - the numeric suffix is the predicted next identity (global, not per-day) computed non-atomically. Manual orders start `ActionStatus='2'` (approved) so they appear at once in `sp_LoadOrderListForOrderCreationbyTerri`.
- **Transaction:** none; detail rows are inserted by separate `sp_Save_ManualOrderDetail` calls. Returns `@OrderId = SCOPE_IDENTITY()`.
- **Callers:** `Library.DAL/SInventory_DAL/OrderListDAL.cs:121` `OrderListDAL.SaveOrderMaster` (`SaveData`, output params).
- **Risks:** race: concurrent saves can generate the same `OrderCode`; predicted id != actual identity if an id is skipped/rolled back (the kind of defect patched by `Create_Order_ODR-20260901134.sql`); an unmatched `@CustomerCode` leaves all snapshot columns/CustomerMasterId NULL yet the order still saves; `TOP 1` without ORDER BY if duplicate codes exist.

### sp_Update_ManualOrderMaster
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1115-1214`. CONFIRMED.
- **Purpose:** Overwrite a manual order header and remove all its detail lines (caller re-inserts).
- **Parameters:** `@OrderId INT` (required) and the same optional header parameters as save (no `@OrderCode`, `@ManufacId`).
- **Writes:** UPDATE `tblOrder` header/amounts and forces `OrderType='Regular', ActionStatus='2', IsSubDepo=0, IsPrepareforInvoice=1`; hierarchy/snapshot columns use `ISNULL(@new, existing)`. **DELETE `tblOrderDetail` WHERE OrderId** (`:1213`).
- **Guards:** none - it does not check `IsInvoice`, so an already-invoiced order can be edited and its detail wiped; no transaction (UPDATE+DELETE not atomic; if the follow-up `sp_Save_ManualOrderDetail` calls fail the order is left empty).
- **Callers:** `OrderListDAL.UpdateOrderMaster` (`OrderListDAL.cs:175`).

### sp_Save_ManualOrderDetail
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1376-1429`. CONFIRMED.
- **Purpose:** Insert one order line.
- **Parameters:** `@ProductId INT`, `@ProductCode NVARCHAR(50)`, `@ProductName NVARCHAR(200)`, `@Quantity DECIMAL(18,4)`, `@TradePrice DECIMAL(18,4)`, `@TotalTradePrice DECIMAL(18,4)`, `@ISGiftProduct BIT=0`, `@IsCampaignProduct BIT=0`, `@OrderId INT`, `@UnitVatAmount/@TotalVatAmount/@NetAmount DECIMAL(18,4)=0`.
- **Writes:** INSERT `tblOrderDetail`(ProductId, ProductCode, ProductName, Quantity, TradePrice, TotalTradePrice, ISGiftProduct, IsCampaignProduct, IsSpDis=0, OrderId, DiscountPercent=0, DiscountAmount=0, UnitVatAmount, TotalVatAmount, NetAmount). Discount always stored 0 (manual orders carry no line discount).
- **Validation:** none (no header existence/quantity checks). **Callers:** `OrderListDAL.SaveOrderDetail` (`OrderListDAL.cs:211`).

### sp_LoadOrderListForOrderCreationbyTerri
- **Script:** `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1225-1365`. CONFIRMED. Single version.
- **Purpose:** Delivery-invoice creation queue: approved, un-invoiced orders of a depot with credit-limit / overdue diagnosis per customer.
- **Parameters:** `@manufacId int=NULL` (actually compared to `tblOrder.DistributionRouteId`, `:1359`), `@comunitId int` (required), `@TerritoryId int=NULL`.
- **Reads:** `tblOrder`, `tblCompanyUnit`, `tblMarket`, `tblCustomerType`, `tblInvoice`, `tblInvoiceDetail`, `tblCustPayDetail`, `tblCustomerCreditLimit`; cross-database `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster` + `..tblDebitCreditVoucherDetail` (sales-return vouchers: `TableID = 2018`, `IsInventoryValuation=0`, sum of `DebitAmountBDT` by `CommonPK_ID` = InvoiceId).
- **Business rules (CONFIRMED in SQL):**
  - Queue filter: `IsInvoice=0`, `(OrderType='Regular' OR OrderType IS NULL OR IsManual=1)`, `ComUnitId=@comunitId`, route filter (NULL route passes), territory filter (NULL territory passes), `IsPrepareforInvoice=1 OR NULL`, `ActionStatus IN ('2','Accepted','Approved')`, `IsSubDepo=0 OR NULL`.
  - `Due` (TS) per customer+unit = `SUM(DeliveryTpGrandTotal) - (SUM(PaymentAmount)+SUM(AIT)+SUM(DiscountOnPayment)) - SUM(sales-return debit)`; only customers with `SUM(DeliveryTpGrandTotal)>0`. `ReceiveAble = Order.GrossValue + Due`.
  - Credit limit (LM): sum of active (`IsActive=1`) `tblCustomerCreditLimit` rows with `ActionStatus='Approved'` per company+customer -> `LimitAmount`, `DayLimit`.
  - Oldest unpaid invoice (LD): min `InvoiceNo`/`InvoiceDate` among invoices with `DeliveryTpGrandTotal>0` whose net (`total - payment - AIT - discount - return`) `> 1` BDT. `DS = DATEDIFF(DAY, oldest InvoiceDate, GETDATE())`.
  - **`Permission` flag** = 1 when `LimitAmount = 0` (no limit configured -> unlimited), or (`ReceiveAble <= LimitAmount` AND `DS <= DayLimit`); else 0.
  - `DueAmount` (tblDue) = per customer sum over invoices of `(SUM(PaymentNetAmount) - SUM(TPAmount+VATAmount paid))` where the per-invoice residual `> 10` and totals differ; `DueAlert` = days since `UpdateDate` (>=30) of the latest delivered partial/unpaid invoice (`PaymentStatus IS NULL OR 'Partial'`, `PaymentInvoiceStatus<>'Reject'`).
- **Result columns:** PaymentType, DueAlert, CustomerMasterId, Remarks, ComunitId, ManufacId, OrderId, OrderCode, OrderSenderCode, OrderSenderName, SubmissionDate, DueAmount, CustomerType (twice: `tblCustomerType.CustomerType` and `tblOrder.CustomerType`), MarketName, TerritoryName (`TerritoryName_Ord`), CustomerCode, CustomerName, GrossValue, DeliveryDate, Due, ReceiveAble, LimitAmount, DayLimit, InvoiceNo (`N/A`), InvoiceDate, DS, Permission.
- **Callers:** `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:220` `OrderInfoDAL.LoadOrderForOrderCreation` (passes `@comunitId,@manufacId,@TerritoryId`; the `marketId` argument is not passed).
- **Cross-SP inconsistency (CONFIRMED by reading the posting SPs):** the return amount is joined `INV.InvoiceId = RTN.CommonPK_ID` for vouchers with `TableID = 2018`; but `sp_SalesReturnPosting` / `sp_DeliverySalesReturnPosting` (`Live_Deployment_Script_All_Updates.sql:662`, `:1093`) store `CommonPK_ID = ReturnInvoiceId` / `CustReturnId`, **not** the invoice id. Sales returns are therefore subtracted from whichever invoice happens to share that numeric id (effect on real data: UNKNOWN, depends on how legacy vouchers were keyed).
- **Risks:** hard dependency on database name `CSTL_ACCDB_Unicorn` (non-portable cross-db); unused `LEFT JOIN tblInvoice INV2` inside `LD`; "no credit limit configured" is treated as fully permitted; duplicate `CustomerType` column names; `tblDue` aggregate is not filtered by company/unit.

## A.3 Receivables, payment and sales-return posting SPs (`Live_Deployment_Script_All_Updates.sql`)

Script header (`Live_Deployment_Script_All_Updates.sql:1-8`): modules "Customer Payment (Discount, AIT & Delivery Charge Settlement)", "Customer Aging & Outstanding Report", "Delivery Sales Return", dated **06-Sep-2026** - i.e. later than `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql` (31-Aug-2026). It uses `CREATE OR ALTER` (needs SQL Server 2016 SP1+), switches database with `USE [SalesDisDB_Unicorn_DB]` (`:13`, `:121`) and `USE [CSTL_ERPDB_Unicorn]` (`:95`). The SPs read/write three databases: `SalesDisDB_Unicorn_DB` (sales), `CSTL_ERPDB_Unicorn` (ERP master, GL mapping) and `CSTL_ACCDB_Unicorn` (accounts/vouchers, referenced cross-database with 3-/4-part names).

### sp_Report_CustomerInvoiceAgingOutstanding
- **Script:** `Live_Deployment_Script_All_Updates.sql:129-205` (single version). CONFIRMED.
- **Purpose:** Customer invoice ageing / outstanding statement as on a date.
- **Parameters:** `@AsOnDate DATE` (required; NULL -> today), `@FromDate DATE=NULL`, `@ToDate DATE=NULL`, `@ComUnitId INT=NULL`, `@CustomerMasterId INT=NULL`, `@SalesPersonId INT=NULL`, `@OnlyDueInvoices BIT=1` (**declared but never used** - the SP always returns only `Outstanding > 0`).
- **Reads:** `tblInvoice` (InvoiceId, InvoiceNo, InvoiceDate, DeliveryCharge, CustomerMasterId, ComUnitId), `tblInvoiceDetail` (`DeliveryNetAmount`), `tblCustPayDetail` (PaymentAmount, Discount, AIT, InvoiceId, custPaymentDate, CustPayId), `tblCustomerPay` (PaymentDate), `tblCustMaster` (CustomerName, SalesPersonId), `tblEmpGeneralInfo` (EmpName). Writes: none.
- **Rules:** `Invoice Amount = SUM(DeliveryNetAmount) + DeliveryCharge`; `Paid Amount = SUM(PaymentAmount + Discount + AIT)` with payments dated `<= @AsOnDate` (payment date = `ISNULL(cpd.custPaymentDate, cp.PaymentDate)`); `Credit Days = DATEDIFF(DAY, InvoiceDate, @AsOnDate)`; `Outstanding = Invoice Amount - Paid Amount`; only invoices with `InvoiceDate <= @AsOnDate` and `Outstanding > 0.00`; `0`/NULL filter parameters mean "all". Discount and AIT count as settlement, not as revenue reduction. **Sales returns are NOT netted** (contrast `sp_LoadOrderListForOrderCreationbyTerri`, which subtracts return vouchers, and `sp_GET_PaymentInvSP`, which exposes `ReturnTotal` only as `AdjustableAmount`).
- **Result set:** SL (`ROW_NUMBER` by InvoiceDate, InvoiceNo), Customer Name, Invoice No, Invoice Date (`dd Mon yyyy`, style 106), Invoice Amount, Paid Amount, Credit Days, Outstanding, Responsible Person (`EmpName` of customer's SalesPersonId or 'N/A').
- **Transactions/errors:** none (read-only). CTEs: `CTE_InvoiceSummary`, `CTE_PaymentSummary`, `CTE_BaseData`.
- **Callers:** `Library.DAL/SInventory_DAL/CustomerInvoiceAgingDAL.cs:27` `CustomerInvoiceAgingDAL.GetCustomerInvoiceAging`.
- **Risks:** unused `@OnlyDueInvoices`; `DeliveryNetAmount` vs `PaymentNetAmount` (used by the other payment SPs) are different columns - reports may not reconcile; `LEFT JOIN` payment summary is date-bounded but invoice summary is not (later edits of invoice detail change history).

### sp_GET_PaymentInvSP
- **Script:** `Live_Deployment_Script_All_Updates.sql:212-289`. Name spelled `sp_GET_PaymentInvSP` (SQL Server identifiers are case-insensitive by default, so C# `sp_Get_PaymentInvSP` resolves). CONFIRMED. Single version.
- **Purpose:** List of invoices with a payable balance for the customer-payment / second-time-return screens.
- **Parameters:** `@param NVARCHAR(MAX)` (required; NULL -> ''), a raw SQL fragment appended to the WHERE.
- **Reads:** `tblInvoice`, `tblOrder` (LEFT), `tblCustMaster`, `tblCompanyUnit`, `tblPaymentType`, `tblCustPayDetail` (PaymentAmount, Discount, AIT, TPAmount, VATAmount), `tblInvoiceDetail` (PaymentNetAmount, PaymentTotalPriceVatAmount, PaymentTotalPrice), `tblReturnInvoice` (TPGrandTotal). Writes: none.
- **Rules (CONFIRMED):** `TotalDelivery = SUM(PaymentNetAmount) + tblInvoice.DeliveryCharge` (delivery charge now part of receivable); `PaymentAmount(PP) = SUM(PaymentAmount)+SUM(Discount)+SUM(AIT)`; `Due = TotalDelivery - PP`; `TP_Pay = (TotalDelivery - PaymentTotalPriceVatAmount) - SUM(TPAmount)` and `Vat_Pay = -SUM(VATAmount)` (TP part vs VAT part split); `AdjustableAmount = SUM(tblReturnInvoice.TPGrandTotal)`; only rows with `Due > 0`, `PaymentInvoiceNo IS NOT NULL` (invoice must have been delivered/finalised) and `SndReturnInvoiceNo IS NULL` (no second-time return). `CompanyId` defaults to **16** and `PaymentTypeName` to 'Credit' when missing (hard-coded, `:241-242`). Order header falls back to customer master (`CROSS APPLY` alias `ord`), and to order code 'Opening' for invoices with no order (opening-balance invoices).
- **Dynamic SQL:** `@param` concatenated into the statement (`:286`) -> injection surface. Result set columns: TerritoryName_Ord, TP_Pay, Vat_Pay, OrderNo, OrderDate (dd/mm/yyyy), CustomerCode, CustomerName, MarketId, DistributionRouteId, CustomerMasterId, InvoiceId, InvoiceNo, InvoiceDate, RtnInvoiceNo, RtnInvoiceDate, TotalDelivery, PaymentAmount, Due, AdjustableAmount, CompanyId, PaymentTypeName.
- **Callers:** `Library.DAL/SInventory_DAL/OrderInfoDAL.cs:795` `LoadPaymentInvSP` and `:857` `SndTimeReturnLoadPaymentInvSP`; `OrderInfoDALSalesReturn.cs:431` `LoadPaymentInvSP`; `SalesReturnNewDAL.cs:377` `LoadPaymentInvSP`.
- **Risks:** `RTN` join multiplies rows only via aggregation (safe); `tblInvoiceDetail` is joined twice; date strings formatted in SQL.

### sp_GET_PaymentInvSPTPVATAmt
- **Script:** `Live_Deployment_Script_All_Updates.sql:296-333`. CONFIRMED.
- **Purpose:** For one invoice, return the TP (trade price) and VAT components still payable (used to split a payment amount).
- **Parameters:** `@InvoiceId int`, `@PayAmount decimal(18,2)` (**unused**).
- **Reads:** same tables as above but **INNER JOIN `tblOrder` and `tblCustMaster`** (so invoices without an order - opening invoices - return no row here, unlike `sp_GET_PaymentInvSP` which LEFT JOINs). Rules: `TP_Pay = (SUM(PaymentNetAmount)+DeliveryCharge - PaymentTotalPriceVatAmount) - SUM(TPAmount)`, `Vat_Pay = PaymentTotalPriceVatAmount - SUM(VATAmount)`; filter `PaymentInvoiceNo IS NOT NULL AND InvoiceId=@InvoiceId`. **Inconsistent with the list SP:** there `Vat_Pay = -SUM(VATAmount)`, here `Vat_Pay = VATtotal - paid`. Result: TP_Pay, Vat_Pay.
- **Callers:** `OrderInfoDAL.cs:919` `LoadPaymentInvSPTPVATAmt`; `OrderInfoDALSalesReturn.cs:462`; `SalesReturnNewDAL.cs:408`.

### sp_GET_PaymentInvSPPaymentAmount
- **Script:** `Live_Deployment_Script_All_Updates.sql:340-396`. CONFIRMED.
- **Purpose:** Build the payment-entry grid rows for pre-typed payment amounts.
- **Parameters:** `@param nvarchar(max)`, `@PaymentAmount nvarchar(max)`, `@CollectionBy nvarchar(max)` - **all three concatenated into dynamic SQL**: `@CollectionBy` inside quotes (`:351`) and `@PaymentAmount` as a raw expression (`:355`) -> direct injection surface; `@param` appended to WHERE.
- **Rules:** invoices with `PaymentInvoiceNo IS NOT NULL AND (PaymentStatus IS NULL OR 'Partial')`; `TotalDelivery = SUM(PaymentNetAmount)+DeliveryCharge`; `Due = TotalDelivery - (payments+discount+AIT)`; `TP_Pay` and `Vat_Pay` constant 0; `PayNewAmount = @PaymentAmount`. Note: no `Due > 0` guard here, unlike the list SP.
- **Result columns:** CollectionBy, TP_Pay(0), Vat_Pay(0), ComUnitId, PayNewAmount, OrderNo, OrderDate, CustomerCode, CustomerName, MarketId, DistributionRouteId, CustomerMasterId, InvoiceId, InvoiceNo, InvoiceDate, RtnInvoiceNo, RtnInvoiceDate, TotalDelivery, PaymentAmount, Due, AdjustableAmount.
- **Callers:** `OrderInfoDAL.cs:91` `LoadPaymentInvSPWithPaymentAmount`; `OrderInfoDALSalesReturn.cs:101`; `SalesReturnNewDAL.cs:46`.

### sp_Sales_CustomerPaymentPosting
- **Script:** `Live_Deployment_Script_All_Updates.sql:403-551`. CONFIRMED. Single version.
- **Purpose:** Auto-post accounting (credit/receipt) vouchers for one customer payment line and, if the invoice becomes fully paid, trigger COGS posting.
- **Parameters:** `@CustPayId INT`, `@InvoiceId INT`.
- **Reads:** `tblCustPayDetail` (PaymentAmount, Discount, AIT, BankAccId, CashAccId, InvoiceId, CustPayId), `tblCustomerPay` (PaymentDate), `tblInvoice` (InvoiceNo, CustomerMasterId, ComUnitId, DeliveryTpGrandTotal, DeliveryCharge), `tblCompanyUnit`, `tblCustMaster`; cross-db `CSTL_ERPDB_Unicorn..tblCompanyInfo`, `CSTL_ERPDB_Unicorn..tbl_AccIntegrationTable` (IntegrationTableId for `tblCustomerPay`), `CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity` (GLId by PageId/FlagId), `CSTL_ACCDB_Unicorn..tblSubsidiaryInfo` (SubsidiaryTypeId=1), `..tblReferenceInfo` (ReferenceTypeId 1016), `..tblFinancialYear`, `..tblChartOfAccounts` (BankAccountId/CashAccountId -> AccountId), `..tblDebitCreditVoucherMaster`.
- **Writes:** INSERT `CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster` and `..tblDebitCreditVoucherDetail` per payment line (cursor `@MyCursor2`). Voucher master: VoucherType = `CreditVoucherBank` if `BankAccId<>0`, else `CreditVoucherCash` if `CashAccId<>0` (else NULL - the `'Journal'` default is overwritten by the CASE, `:453-456`), VoucherCode prefix `CVB-` / `CVC-`, `VoucherCategoryID=2`, `CurrencyId=1`, rate 1, `TotalAmountBDT = Payment+Discount+AIT`, `ActionStatus='Approved'`, `EntryBy='admin'`, `ApprovedBy='Auto Process'`, `Remarks='Auto Posting'`, `CommonPK_ID=InvoiceId` (per line), `TableID = IntegrationTableId`.
- **Voucher lines (balanced):** Dr Cash/Bank GL (`tblChartOfAccounts.AccountId` by BankAccountId/CashAccountId) = PaymentAmount (only if > 0); Dr Discount GL (PageId 163, FlagId 1023) = Discount; Dr AIT GL (PageId 163, FlagId 1024) = AIT (AIT = advance income tax withheld by customer); Cr Accounts-Receivable GL (PageId 163, FlagId 2) = PaymentAmount+AIT+Discount with customer subsidiary ledger id. Voucher code = `yyMMdd` + `MAX(SUBSTRING(VoucherCode,11,6)) + 1` (default 10000+1) per posting date (`:449-451`).
- **COGS trigger:** after the cursor: `DueAmount = (DeliveryTpGrandTotal + DeliveryCharge) - SUM(PaymentAmount+Discount+AIT)` for the invoice; if `<= 0` -> `EXEC sp_Sales_DeliveryCOGSPosting @InvoiceId` (SP **not in repo**). Cost of goods sold is therefore recognised only on full settlement.
- **Transactions/errors:** none: cursor loop with multiple inserts across databases, no TRAN/TRY/CATCH -> partial vouchers possible (master without detail, unbalanced). Not idempotent: running twice for the same `(CustPayId, InvoiceId)` posts duplicate vouchers (the join to voucher master `V` at `:433` is never used as a guard).
- **Risks/ambiguities:** hard-coded `EntryBy='admin'`; MAX+1 voucher numbering race; INNER JOINs to `tblFinancialYear` on `PaymentDate BETWEEN Start AND End` - a payment outside any FY silently posts nothing; `LEFT JOIN tblReferenceInfo ... R_Info_Id=@InvoiceId` is unqualified; `@VoucherType` can be NULL when both bank and cash ids are 0 (no prefix, NULL type); posting date = payment date; `DISTINCT` cursor may collapse identical lines.
- **Callers:** `Library.DAL/SInventory_DAL/CustPaymentDAL.cs:73` `CustPaymentDAL.PaymentJournalPosting`.

### sp_DeliverySalesReturnPosting
- **Script:** `Live_Deployment_Script_All_Updates.sql:558-812`. CONFIRMED. Single version.
- **Purpose:** Account posting for a delivery-time (customer) sales return, including the delivery-charge deduction.
- **Parameters:** `@ReturnId INT` (= `tblCustomerReturnMaster.CustReturnId`).
- **Reads:** `tblCustomerReturnMaster` (InvoiceId, TotalAmount, DeductionCharge, NetReturnAmount), `tblInvoice`, `tblCustMaster`, `tblCompanyUnit`, `tblCustomerReturnDetail` (ReturnQty, InvoiceDetailId), `tblInvoiceDetail` (DCStoreId), `tblDCStore` (TempReceiveId), `tblCentralStore` (UnitPrice, VATPerUnit; joined on `DCS.TempReceiveId = CTRS1.ReceiveId`); cross-db `CSTL_ERPDB_Unicorn..tblCompanyInfo`, `..Table_Integration_AccountsConnectivity`, `CSTL_ACCDB_Unicorn..tblFinancialYear`, `..tblSubsidiaryInfo`, `..tblReferenceInfo`, `..tblDebitCreditVoucherMaster`.
- **Writes:** two voucher masters + details in `CSTL_ACCDB_Unicorn`, then `UPDATE tblCustomerReturnMaster SET IsPosting = 1` (`:808-810`).
  - Voucher 1 (TableID 2018, "Sales Return Journal"): Dr Sales-Return GL (PageId 23, Flag 1020, fallback 4170) = `TotalAmount`; Cr Delivery-Charge Income GL (PageId 23, Flag 1032, fallback 5968) = `DeductionCharge` (only if > 0); Cr AR GL (PageId 23, Flag 2, fallback 2112) = `NetReturnAmount` with customer subsidiary ledger. Balanced when `Net = Total - Deduction`.
  - Voucher 2 (TableID 2023, inventory valuation), only if `StockValue > 0`: Dr Finished-Goods GL (PageId 23, Flag 1016, fallback 4156) and Cr COGS GL (PageId 23, Flag 1021, fallback 4162) = `SUM((CentralStore.UnitPrice + VATPerUnit) * ReturnQty)` - i.e. valued at the **central-store receive price incl. VAT**, not the DC weighted cost.
- **Rules:** exits silently (`RETURN`) when the return/invoice is not found or `TotalAmount <= 0` (`:598`); `NetReturnAmount` recomputed as `Total - Deduction` when <= 0; posting date = **today** (`CAST(GETDATE() AS DATE)`), not the return date; company id via unit -> `tblCompanyInfo` (default 1); financial year = `Status='Active'` else the FY containing today, **fallback FY id 2** (`:620`); voucher code `JVP-yyMMdd` + (MAX(SUBSTRING(code,11,6))+1).
- **Transactions/errors:** none; **no idempotency guard** (`IsPosting` is set but never tested at the start), so a re-run duplicates both vouchers. `IsPosting` on `tblCustomerReturnMaster` is **not created by any script here** (the deployment adds `IsPosting` only to `tblReturnInvoice`, `:66-76`) -> UNKNOWN whether `tblCustomerReturnMaster.IsPosting` exists.
- **Inconsistency with `sp_SalesReturnPosting`:** different GL flags for inventory/COGS (1016/1021 here vs 1018/1019 there), AR flag on PageId 23 here vs PageId 163 there, reference type 1016 (invoice) here vs 1018 (return invoice) there, and different valuation basis.
- **Callers:** `Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:140` `CustomerReturnDAL.PostDeliverySalesReturn`.

### sp_SalesReturnPosting
- **Script:** `Live_Deployment_Script_All_Updates.sql:949-1240`. CONFIRMED. Single version.
- **Purpose:** Account posting for a market (field) sales return recorded in `tblReturnInvoice` (page `SalesReturn.aspx`).
- **Parameters:** `@ReturnInvoiceId INT`.
- **Reads:** `tblReturnInvoice` (ReturnInvoiceNo, ReturnInvoiceDate, CustomerMasterId, ComUnitId, TpGrandTotal, TpTotal, DeductionCharge, NetReturnAmount), `tblCustMaster`, `tblReturnInvoiceDetail` (NetAmount, TotalPrice, TotalQuantity, Quantity, CostPrice, ProductCode), `tblDCStore` (CostPerUnit fallback: latest row with `CostPerUnit>0` by ProductCode across **all DCs**, `ORDER BY DCStoreId DESC`), `CSTL_ACCDB_Unicorn..tblFinancialYear/tblSubsidiaryInfo/tblReferenceInfo/tblDebitCreditVoucherMaster`, `CSTL_ERPDB_Unicorn..Table_Integration_AccountsConnectivity`.
- **Writes (cross-db):** `tblSubsidiaryInfo` (creates a customer subsidiary ledger row when missing: SubsidiaryTypeId=1, S_Info_Id=customer, EntryBy=1), `tblReferenceInfo` (ReferenceTypeId 1018 for the return invoice when missing), `tblDebitCreditVoucherMaster` (2 vouchers, `IsInventoryValuation` 0 and 1, `SalesReturnId`), `tblDebitCreditVoucherDetail`, and `UPDATE tblReturnInvoice SET IsPosting = 1`.
- **Rules:** `ReturnNetAmount = TpGrandTotal` (else `TpTotal`, else `SUM(NetAmount or TotalPrice)` from detail); `NetReturnAmount` = stored value, else `ReturnNetAmount - DeductionCharge` (floored at 0); `ReturnCostAmount = SUM(ISNULL(TotalQuantity, Quantity) * ISNULL(NULLIF(CostPrice,0), DCStore.CostPerUnit))`. Voucher 1 (TableID 2018, IsInventoryValuation=0): Dr Sales-Return GL (PageId 23/Flag 1020, fb 4170) = ReturnNetAmount; Cr Delivery-Charge Income (PageId 23/Flag 1032, fb 5968) = DeductionCharge; Cr AR (PageId 163/Flag 2, fb 2112) = NetReturnAmount with subsidiary id. Voucher 2 (TableID 2023, IsInventoryValuation=1, only if cost > 0): Dr Finished Goods (PageId 23/Flag 1018, fb 4156), Cr COGS (PageId 23/Flag 1019, fb 4162) = ReturnCostAmount. Posting date = `ReturnInvoiceDate` (else today). Financial year = active/covering (fb FY id 2). CompanyId fixed **1** (`:955`).
- **Idempotency:** guarded per voucher by `NOT EXISTS` on `(TableID, CommonPK_ID = ReturnInvoiceId, IsInventoryValuation)` (`:1064-1067`, `:1165-1168`) - safe to re-run, unlike the delivery-return SP.
- **Transactions/errors:** none (no TRAN/TRY); silent `RETURN` when the return invoice number is not found. Voucher number = `JVP-yyMMdd` + `MAX(RIGHT(VoucherCode,5)) + 1` (`:1069-1073`; note different parsing from the payment SP, which uses `SUBSTRING(...,11,6)`), computed at posting date -> race with concurrent postings.
- **Risks:** `ReturnNetAmount` includes the `DeductionCharge` (Dr full amount, Cr income + net) - consistent only if `NetReturnAmount = Total - Deduction`; `AccountId`s hard-coded as fallbacks; EntryBy=1.
- **Callers:** `Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:369` `ReturnInvoiceDAL.PostSalesReturn`.

### spInsertReturnInvoice_new
Two versions exist (duplicate; **conflicting parameter lists**):
| Version | Script:line | Parameters | Notes |
|---|---|---|---|
| Older | `SP_SalesReturn_Related.sql:19-162` (`DROP`+`CREATE`, `SET NOCOUNT ON`; header target DB "SalesDisDB", page `SInventory_UI/SalesReturn.aspx`) | no `@TpGrandTotal`, `@DeductionCharge`, `@NetReturnAmount` | INSERT omits TpGrandTotal/DeductionCharge/NetReturnAmount |
| Newer | `Live_Deployment_Script_All_Updates.sql:857-943` (`CREATE OR ALTER`, dated 06-Sep-2026; section "4.2 Updated Stored Procedure") | adds `@TpGrandTotal decimal(18,2)=null`, `@DeductionCharge decimal(18,2)=0`, `@NetReturnAmount decimal(18,2)=0` | inserts `TpGrandTotal = ISNULL(@TpGrandTotal,@TpTotal)`, `DeductionCharge = ISNULL(@DeductionCharge,0)`, `NetReturnAmount = ISNULL(@NetReturnAmount, ISNULL(@TpGrandTotal,@TpTotal) - ISNULL(@DeductionCharge,0))`; no `SET NOCOUNT ON` |

- **Which is newer:** the Live_Deployment version (INFERRED from the dated header and because it is the superset that `sp_SalesReturnPosting` reads `DeductionCharge/NetReturnAmount` from). **Conflict:** running `SP_SalesReturn_Related.sql` after the live script would DROP the newer SP and re-create the older one, silently losing the deduction-charge columns, and a C# call passing `@TpGrandTotal/@DeductionCharge/@NetReturnAmount` would then fail with "not a parameter" (check `BonusCampaignNewDAL.SaveReturnSalesMaster`).
- **Purpose:** Insert the market sales-return header into `tblReturnInvoice` (page SalesReturn.aspx) and return the new id.
- **Common parameters:** `@ReturnInvoiceId INT` (required, **unused**), `@InvoiceDate datetime=null`, `@OrderId int`, `@OrderNo nvarchar(50)`, `@OrderDate datetime`, `@CustomerMasterId int`, `@ComUnitId int`, `@MiaId int`, `@PaymentTypeId int`, `@TpTotal/@TpDiscount/@TpVat decimal(18,2)`, `@DpNAme nvarchar(50)` and `@DpMob` (unused), `@createBy nvarchar(50)`, `@Createdate datetime`, `@TotalSpecialAmount decimal(18,2)`, `@ProductOffer bit`, `@OldTradePolicy bit`, `@Remarks nvarchar(max)`, `@FixedCustomer bit`, `@MIACode`, `@MIAName`, `@MarketCode`, `@MarketName`, `@AreaCode`, `@DisCode`, `@FEName`, `@RegionCode`, `@DZSMName`, `@invoiceid int`, `@Type bit`, `@MIOId_new int`, `@Terri_Id_new int`, `@MioEmpId_new int`, `@Mio_SapCode_New nvarchar(100)`.
- **Reads:** `tblReturnInvoice` (`MAX(ReturnInvoiceId)`), `tblTerritory` -> `tblArea` -> `tblRegion` (AreaId/RegionId for `@Terri_Id_new`). **Writes:** INSERT `tblReturnInvoice`(ReturnInvoiceNo, ReturnInvoiceDate, CreateBy, CreateDate, OrderNo, OrderDate, CustomerMasterId, ComUnitId, MiaId, PaymentTypeId, TpTotal, TpDiscount, TpVat, [TpGrandTotal, DeductionCharge, NetReturnAmount in newer], OrderId, TotalSpecialAmount, OldTradePolicy, ProductOffer, Remarks, MIACode, MIAName, MarketCode, MarketName, AreaCode, DisCode, FEName, RegionCode, DZSMName, FixedCustomer, InvoiceId, IsSalesReturnWithoutOrder='No order' (hard-coded), SubInvoiceId=0, Types=@Type, MIOId_new, Terri_Id_new, MioEmpId_new, Mio_SapCode_New, RegionId_Rtn, AreaId_Rtn).
- **Rules:** return number = `'RIN--' + yyMMdd(@InvoiceDate) + (MAX(ReturnInvoiceId of that day) + 1001)`, default base 10000 when none that day (`:908` / `:67`) - the suffix is derived from the **primary key**, not a per-day counter, so numbers are non-sequential and can repeat if the day's max id + 1001 collides with a later id. The `RIN` prefix is later used by the stock-ledger SPs to classify return receipts (`ChalanNo LIKE 'RIN%'`).
- **Result set:** `SELECT SCOPE_IDENTITY()`. **Transactions:** none. `SUBSTRING(CONVERT(NVARCHAR(MAX),YEAR(...)),3,3)` -> 2-digit year.
- **Callers:** `Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs:505` `BonusCampaignNewDAL.SaveReturnSalesMaster`.

### sp_UpdateAndInsertInvoiceDetailSalesReturn
- **Script:** `SP_SalesReturn_Related.sql:174-227` (single version; `DROP`+`CREATE`; header says target DB "SalesDisDB"). CONFIRMED.
- **Purpose:** Insert one second-time ("snd") sales-return line for an invoice detail.
- **Parameters:** `@InvoiceDetailReturnId INT OUTPUT`, `@InvoiceId INT`, `@InvoiceDetailId INT`, `@Quantity INT`, `@PreviousQuantity INT`, `@BonusQuantity INT`, `@TotalQuantity INT`, `@TotalPrice DECIMAL(18,2)`, `@TotalPriceVatAmount DECIMAL(18,2)`, `@DiscountPercentage DECIMAL(5,2)`, `@DiscountAmount DECIMAL(18,2)`, `@NetAmount DECIMAL(18,2)`, `@DeliveryStatus NVARCHAR(50)`, `@ReturnReason NVARCHAR(250)` (all required).
- **Writes:** INSERT `tblInvoiceDetailReturn`(InvoiceId, InvoiceDetailId, PreviousQuantity, sndReturnQuantity, sndReturnBonusQuantity, sndReturnTotalQuantity, sndReturnTotalPrice, sndReturnTotalPriceVatAmount, sndReturnDiscountPercentage, sndReturnDiscountAmount, sndReturnNetAmount, sndReturnStatus=@DeliveryStatus, sndReturnReason). Despite the name, **no UPDATE** is performed. Returns id both via OUTPUT parameter and `SELECT ... AS InvoiceDetailReturnId`.
- **Validation/transactions:** none (no check that `@Quantity <= previous`, no stock effect here; stock/receipt is elsewhere).
- **Callers:** `Library.DAL/SInventory_DAL/SalesReturnDAL.cs:792` `SalesReturnDAL.InsertInvoiceDetailReturn`.

## A.4 Stock valuation / ledger reports

The DC ledger table `tblDCStore` is modelled as an append-style ledger: *Receive* rows carry `StockQty = TotalQuantity`; *Issue* rows are separate negative rows (`Trans_Type='Issue'`, `StockQty < 0`, `ReferenceDCStoreId` -> the Receive row, `ReferenceTransactionDate`, `StockCondition` e.g. `B2BIssue`), evidenced by `Library.DAL/SInventory_DAL/ChalanDAL.cs:515-545` and by `sp_Delete_ProformaInvoice` / `sp_Adjust_DCStore_Stock_Only`. Stock = `SUM(StockQty)`.

### sp_Report_DCStore_StockValuationLedger
Two versions (duplicate, **logic conflict**):
| Version | Script:line | Date basis |
|---|---|---|
| v-A (older) | `LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:52-211` | **`StockRcvDate` only** for opening, movements and last-cost (`:123`, `:180`, `:190`) |
| v-B (newer, INFERRED from file name "..._TransactionDate_Logic" and the extra column) | `SP_Update_StockValuation_TransactionDate_Logic.sql:19-179` (`USE [SalesDisDB_Unicorn_DB]`) | Receives by `StockRcvDate`; **Issues (`StockQty<0 OR Trans_Type='Issue'`) by `ISNULL(ReferenceTransactionDate, StockRcvDate)`** (`:90`, `:147-148`, `:158`) |
- Everything else (parameters, columns, CASE classification, join structure) is byte-for-byte the same apart from comments; the two scripts DROP+CREATE the same object, so **whichever ran last wins**. Nothing in the repo records which is deployed -> UNKNOWN. v-B depends on `tblDCStore.ReferenceTransactionDate`, which no script here adds (used by C# INSERTs at `ChalanDAL.cs:132`, `CustomerReturnDAL.cs:196`, `InvoiceDAL.cs:676/1836`, so it exists in the live schema).
- **Purpose:** Per depot (DC) / product stock ledger: opening, movements, closing quantity and value.
- **Parameters:** `@FromDate DATETIME`, `@ToDate DATETIME` (required; `@ToDate` is moved to 23:59:59 of its day, `:63`/`:30`), `@ComUnitId INT=NULL`, `@CategoryId INT=NULL`, `@ProductId INT=NULL` (NULL = all).
- **Reads:** `tblDCStore` (ComUnitId, ProductCode, StockQty, TotalCostAmount, CostPerUnit, StockRcvDate, Trans_Type, StockCondition, ChalanNo, ReturnFromInvoiceDetailId, DCStoreId, [ReferenceTransactionDate in v-B]), `tblCompanyUnit` (ComUnitCode, ComUnitName), `tblProduct` (ProductId, ProductCode, ProductName, PackSize, CategoryId), `tblUnitPrice` (CostPrice, IsActive=1, fallback cost), `tblProCategory` (CategoryName). Writes: none. Uses `WITH (NOLOCK)` throughout (dirty reads).
- **Classification rules (CONFIRMED, identical in both versions):**
  - *Receive From WH* = `StockQty>0` rows with `Trans_Type IN ('Receive','Available','',NULL)`, `ReturnFromInvoiceDetailId=0/NULL`, `StockCondition NOT IN ('B2BReceive','TransferReceive','SalesReturn','Return')`, `ChalanNo` not `IDT%`, `RIN%`, `RINV%`.
  - *B2B Receive* = `Trans_Type IN ('Receive','Available')` AND (`StockCondition IN ('B2BReceive','TransferReceive')` OR `ChalanNo LIKE 'IDT%'`), `StockQty>0`.
  - *Return Rcv* = `ReturnFromInvoiceDetailId<>0` OR `StockCondition IN ('SalesReturn','Return')` OR `ChalanNo LIKE 'RIN%'/'RINV%'` (no sign restriction).
  - *Sales* = `StockQty<0` and not `B2BIssue/TransferIssue` -> `ABS(StockQty)`; or `Trans_Type IN ('Issue','Sales')` (non-B2B) -> `StockQty` (positive-valued issue rows are added as-is).
  - *B2B Issue* = `StockCondition IN ('B2BIssue','TransferIssue')` -> `ABS(StockQty)`.
  - Opening = `SUM(StockQty)` before `@FromDate`, rate = `SUM(TotalCostAmount)/SUM(StockQty)` when qty > 0 else `AVG(CostPerUnit)`; Closing Qty = Opening + WH + B2B + Return - Sales - B2B Issue; **Closing Rate = latest `CostPerUnit>0` row up to `@ToDate`** (`ROW_NUMBER` by StockRcvDate DESC, DCStoreId DESC) else active `tblUnitPrice.CostPrice`; Closing Value = Closing Qty x Closing Rate (so closing value is *not* the sum of ledger cost amounts). Only rows with any non-zero opening/movement are returned.
- **Result columns:** DC Code, Distribution Center, Product Code, Product Name, Pack Size, Category Name, Opening Qty/Rate/Total Value, Receive From WH, B2B Receive, Return Rcv, Sales, B2B Issue, Closing Qty/Rate/Total Value (all DECIMAL(18,2)).
- **Callers:** `Library.DAL/SInventory_DAL/DCStockReportDAL.cs:571` `DCStockReportDAL.GetDCStockValuationLedger` -> page `Solution.Web/SInventory_UI/DCStockValuationReport.aspx.cs` (PAGE-365).
- **Risks:** the outer query iterates every `tblDCStore` row and relies on the final `GROUP BY` (over already-aggregated sub-query columns) to collapse them; because `UP.CostPrice` is part of the GROUP BY, **more than one active `tblUnitPrice` row for a product produces duplicate output lines** for the same DC/product. The *Return Rcv* bucket sums `StockQty` with no sign or `Trans_Type` restriction, so a negative row carrying a return marker (`RIN%`, `ReturnFromInvoiceDetailId`) is netted into Return Rcv **and** counted as Sales (`StockQty<0`) - double effect on the closing quantity. Products are joined by `ProductCode` text (trim/upper), not id. `NOLOCK` reads can see uncommitted ledger rows.

### sp_Report_NationalStockValuation_DateRange
- **Script:** `SP_Update_StockValuation_TransactionDate_Logic.sql:195-338` (single version). CONFIRMED.
- **Purpose:** National (central store + all depots) stock valuation between two dates with one dynamic column per company unit.
- **Parameters:** `@FromDate DATETIME`, `@ToDate DATETIME` (required; ToDate -> end of day), `@CategoryId INT=NULL`, `@ProductId INT=NULL`.
- **Dynamic SQL:** column list built with `STRING_AGG(QUOTENAME(ComUnitName), ', ')` over **all** `tblCompanyUnit` rows (needs SQL Server 2017+); date values and filters are string-concatenated into the statement (`CONVERT(VARCHAR(23), @ToDate, 121)`, `CAST(@CategoryId AS VARCHAR)`), executed with `sp_executesql` without parameters. The select-list alias is built as `'[' + ComUnitName + ']'` (not `QUOTENAME`), so a unit name containing `]` breaks the query; a `ComUnitName` used in `PIVOT ... FOR ComUnitName IN (...)` appears once per row - duplicate names across units would create duplicate columns (error). If `tblCompanyUnit` is empty the column lists are NULL and the SQL becomes NULL.
- **Reads:** `tblProduct`, `tblProCategory`, `tblCompanyUnit`, `tblDCStore` (StockQty, TotalCostAmount, Trans_Type, StockRcvDate, ReferenceTransactionDate, ProductCode, ComUnitId), `tbl_INV_CentralStore_WeightedAvg` (Item_Id, Close_Qty, Avg_Rate, Close_Amt, Trans_Date, Weighted_Avg_Id). Writes none.
- **Rules:** depot quantities are `SUM(StockQty)` with the transaction-date rule (issues by `ReferenceTransactionDate`); central store closing = latest `tbl_INV_CentralStore_WeightedAvg` row with `Trans_Date <= ToDate` (`ROW_NUMBER` by `Weighted_Avg_Id DESC`), opening = latest row with `Trans_Date < FromDate`. National Qty = Central + Total Depots; National Value = Central `Close_Amt` + `SUM(tblDCStore.TotalCostAmount)` (depot valuation uses ledger cost amounts, unlike the DC ledger report's last-rate valuation). Only products with any positive opening/closing stock are listed (negative stocks hidden).
- **Result columns:** Product Code, Product Name, Pack Size, Category, Avg Rate (central), Opening National Qty, Central Store Qty, one column per company unit (`ISNULL(...,0)`), Total Depots Qty, National Stock Qty, Central Store Value, Depots Total Value, National Total Value.
- **Callers:** `Library.DAL/SInventory_DAL/CentralStoreDAL.cs:199` `CentralStoreDAL.GetNationalStockValuation` -> `Solution.Web/SInventory_UI/NationalStockValuationReport.aspx.cs` (PAGE-374).
- **Risks:** `Pivoted_DC` is joined but its columns are referenced unqualified in the select list; the pivot uses **all** company units including non-depot units; hidden dependence on the central-store weighted-average table having one row per item and date; product/depot join by text code.

### sp_Report_NationalStockValuation_BatchDetails
- **Script:** `SP_Update_StockValuation_TransactionDate_Logic.sql:354-483` (single version). CONFIRMED.
- **Purpose:** Batch drill-down for a cell of the national valuation grid.
- **Parameters:** `@ProductCode NVARCHAR(100)`, `@LocationName NVARCHAR(150)` (must equal the grid column caption: `'Central Store Qty'`, `'Opening National Qty'`, `'Total Depots Qty'`, `'National Stock Qty'`, otherwise treated as a depot `ComUnitName`), `@FromDate`, `@ToDate DATETIME` (ToDate -> end of day).
- **Reads:** `tbl_INV_Central_BatchWiseStock` (Batch_No, Mfg_Date, Exp_Date, Trans_Qty, Weighted_Avg_Id), `tbl_INV_CentralStore_WeightedAvg` (Item_Code, Trans_Date), `tblDCStore` (BatchNo, MfgDate, ExpDate, StockQty, transaction-date rule), `tblCompanyUnit`. Static SQL (no dynamic).
- **Rules:** central batch quantity = `SUM(Trans_Qty)` of batch rows whose weighted-avg row has `Trans_Date <= @ToDate` (opening: `< @FromDate`); combined branches `UNION ALL` central + depot batches; `HAVING SUM(qty) > 0` (negative/zero batches hidden). Because batch rows are tied to a weighted-avg row by `Weighted_Avg_Id`, this report is sensitive to which weighted-avg id the STO-issue batch row references - see `tbl_INV_CentralStoreStoIssue` v1 vs v2 (v1 attributes the issue to the *parent receipt's* weighted-avg row/date).
- **Result columns:** Batch No, MFG Date, Exp Date (dd-mm-yyyy), Qty.
- **Callers:** `CentralStoreDAL.cs:210` `CentralStoreDAL.GetNationalStockValuationBatchDetails` -> `NationalStockValuationReport.aspx.cs` (PAGE-374).
- **Risks:** location matching by caption string and by `ComUnitName` (renaming a unit or a caption change silently returns an empty/wrong set); `MIN(Mfg/Exp)` per batch hides differing dates.

## A.5 DC-store integrity / rollback SPs

### sp_Delete_ProformaInvoice
- **Script:** `sp_Delete_ProformaInvoice_Update.sql:8-205` (`ALTER PROCEDURE` - assumes the SP already exists; no CREATE/DROP). Only version in the repo, but the header comments in `SP_Adjust_DCStore_Stock_Only.sql:103` say an **earlier live version added quantity back onto Receive rows** ("previously caused StockQty inflation"), so this file is the newer fix. CONFIRMED / INFERRED (older body not in repo).
- **Purpose:** Delete an undelivered proforma (pre-delivery) invoice: reverse its DC-store issues, log the deleted detail lines, re-open the order for invoicing.
- **Parameters:** `@InvoiceCode NVARCHAR(500)` (= `tblInvoice.InvoiceNo`), `@User NVARCHAR(500)`.
- **Guard:** executes only `IF EXISTS(tblInvoice WHERE InvoiceNo=@InvoiceCode AND DelivaryInvoiceNo IS NULL)` (proforma not yet delivered); otherwise it silently does nothing and returns.
- **Reads:** `tblInvoice` (InvoiceId, OrderId, DelivaryInvoiceNo, DelivarySpecialAmount), `tblInvoiceDetail` (all columns incl. DCStoreId, TotalQuantity, Delivery* columns, ReturnReason, OrderDetailsId).
- **Writes (per invoice-detail row via cursor):** DELETE TOP (1) `tblDCStore` WHERE `ReferenceDCStoreId=@DCStoreId AND Trans_Type='Issue' AND StockQty = -TotalQuantity` (removes the negative issue row that was inserted when the invoice was created); UPDATE `tblDCStore` SET `StockQty = TotalQuantity` for the referenced Receive row where `StockQty > TotalQuantity` (self-heal of the earlier inflation defect); INSERT `tblProInvoiceReturnTrack`(ReturnTotalQuantity, DCStoreId, ReturnDate, ReturnExecutionDateTime, InvoiceDetailId, InvoiceId); INSERT `tblInvoiceDetail_DeleterRecord` (full copy of the detail row); DELETE `tblInvoiceDetail`. After the loop: UPDATE `tblOrderDetail` SET `[Status]=NULL` and `tblOrder` SET `IsInvoice=0` for the order; DELETE `tblReturnAmount` WHERE InvoiceId; DELETE `tblInvoice`; INSERT `tblInvoiceDeleteLog`(InvoiceId, InvoiceNo, DeleteDateTime, DeleteBy=@User).
- **Business rules:** invoice creation consumes stock via a negative Issue ledger row; deleting a proforma removes that row (stock returns implicitly since stock = SUM(StockQty)) instead of adding to the Receive row. Order is put back to `IsInvoice=0`, so it re-enters the invoice-creation queue.
- **Transactions/errors:** **none** - no `BEGIN TRAN`, no `TRY/CATCH`, no `XACT_ABORT`, although it performs a long cursor loop of deletes/inserts across 8 tables. A failure part-way leaves detail rows deleted with stock/logs inconsistent. No result set. The caller `RunStoreProcedure` (`Library.DAL/InternalCls/ClsCommonInternalDAL.cs:551-575`) returns 1 only when `ExecuteNonQuery() > 0`; because the SP sets `SET NOCOUNT ON` the driver reports -1, so `DeleteProformaDal` returns **0 even on success** (INFERRED - callers must not treat 0 as failure).
- **Ambiguities/risks:** (1) if no issue row matches the exact quantity (`StockQty = -TotalQuantity`, e.g. after edits or partial delivery) the negative row **stays** and stock is permanently understated; (2) `@OrderIdForUpdate` is set only inside the loop, so an invoice with **no detail rows** leaves the order at `IsInvoice=1` while the invoice is deleted; (3) rows with `DCStoreId=0` still get a return-track row with `DCStoreId=0`; (4) it deletes by `InvoiceNo` (`DELETE FROM tblInvoice WHERE InvoiceNo=...`) - duplicate invoice numbers would delete several headers; (5) `@ReceiveDate/@ExpDate` etc. stored into `_DeleterRecord`, but bonus/free-product stock (BonusQuantity) is only reversed if it is inside `TotalQuantity`; (6) `sp_Delete_ProformaInvoice_SubDeport` (called from `ProformaOrInvoiceReturn.cs:128`) is a different SP not in the repo.
- **Callers:** `Library.DAL/SInventory_DAL/ProformaOrInvoiceReturn.cs:121` `ProformaOrInvoiceReturn.DeleteProformaDal` (params `@InvoiceCode`, `@User` = Session `LoginName`), via `ProformaOrInvoiceReturnBLL` (`Library.BLL/SInventory_BLL/ProformaOrInvoiceReturnBLL.cs:186`); pages `Solution.Web/SInventory_UI/ProformaorInvoiceReturn.aspx.cs` (PAGE-288), `InvoiceCreationByOrder.aspx.cs` (PAGE-217), `InvoiceCreationForCustomerByOrder.aspx.cs` (PAGE-218), `TopSheetGenerate.aspx.cs` (PAGE-277/462).

### sp_Adjust_DCStore_Stock_Only
- **Script:** `SP_Adjust_DCStore_Stock_Only.sql:13-134` (single version; comments in Bengali). No caller in the repo -> a manual DBA/ops repair tool. CONFIRMED.
- **Purpose:** (1) revert a specific invoice's stock issue and (2) realign corrupted Receive rows (per header comments `:3-6`).
- **Parameters:** `@InvoiceCode NVARCHAR(500)=NULL` (invoice to reverse), `@ProductCode NVARCHAR(100)=NULL` (limits the general realignment), `@FixCorruptedReceiveQty BIT=1`.
- **Reads:** `tblInvoice`, `tblInvoiceDetail` (DCStoreId, TotalQuantity, ProductCode, BatchNo) - falling back to `tblInvoiceDetail_DeleterRecord` + `tblInvoiceDeleteLog` (latest delete of that InvoiceNo) when the invoice is already gone.
- **Writes:** (case 1, when `@InvoiceCode` given) per detail: DELETE TOP (1) from `tblDCStore` where `ReferenceDCStoreId=@DCStoreId AND Trans_Type='Issue' AND StockQty=-TotalQty`; and, if `@FixCorruptedReceiveQty=1`, UPDATE that Receive row SET `StockQty=TotalQuantity` where `StockQty>TotalQuantity`. (case 2, whenever `@FixCorruptedReceiveQty=1`, regardless of invoice code) UPDATE **all** `tblDCStore` rows with `Trans_Type='Receive' AND StockQty<>TotalQuantity` (optionally one product) SET `StockQty=TotalQuantity` (`:104-108`).
- **Transactions/errors:** `TRY/CATCH` with `BEGIN TRANSACTION`/`ROLLBACK` and `RAISERROR(@ErrorMessage,16,1)` (original error number/line lost). Returns result set: Status ('SUCCESS'), ProcessedInvoice, ProcessedProduct, IssueRowsRemoved, ReceiveRowsAligned, Message. Also PRINTs progress.
- **Risks (important):** case 2 uses `<>` (not `>`): it also **raises** Receive rows whose `StockQty < TotalQuantity`. Under any process that legitimately decrements Receive rows in place (e.g. `sp_SAP_B2BChallanDetailInsert`, the v1 `sp_Del_B2BDelete` restore, older invoicing logic) this global reset re-inflates stock. The default (`@FixCorruptedReceiveQty=1`, `@ProductCode=NULL`) applies to the entire `tblDCStore` even when only one invoice is being reversed. Cases 1 and 2 are inconsistent (`>` vs `<>`), and the anomaly checker (`sp_Check_DCStore_StockAnomalies`) only counts `>`.

### sp_Check_DCStore_StockAnomalies
- **Script:** `sp_Check_DCStore_StockAnomalies.sql:11-93` (single version; Bengali comments). No caller in the repo (manual tool). CONFIRMED. Read-only.
- **Purpose:** Diagnose (a) orphan Issue ledger rows whose invoice line no longer exists and (b) Receive rows with inflated `StockQty`.
- **Parameters:** `@ProductCode NVARCHAR(100)=NULL`.
- **Reads:** `tblDCStore`, `tblInvoiceDetail` (DCStoreId, TotalQuantity), `tblInvoiceDetail_DeleterRecord` (InvoiceId, DCStoreId, TotalQuantity, InvoiceDetailId).
- **Rules:** an Issue row is "orphan" when no `tblInvoiceDetail` exists with `DCStoreId = Issue.ReferenceDCStoreId AND TotalQuantity = ABS(Issue.StockQty)`; a Receive row is "corrupted/inflated" when `StockQty > TotalQuantity`.
- **Result sets (3):** (1) summary: Total_Issue_Rows, Orphan_Issue_Rows_Invoice_Removed, Active_Issue_Rows_With_Valid_Invoice, Corrupted_Receive_Rows_Inflated_Stock; (2) orphan issues: DCStore_Issue_Id, Original_Receive_DCStoreId, ProductCode, ProductName, BatchNo, Orphan_Issue_Qty, Issue_Date (`ReferenceTransactionDate`), Anomalies_Reason, Possible_Deleted_InvoiceId; (3) inflated receives: Receive_DCStoreId, ProductCode, ProductName, BatchNo, ChalanNo, Actual_Received_Qty, Corrupted_StockQty, Excess_Inflated_Stock, ReceiveDate.
- **Risks/ambiguities:** matching on `(DCStoreId, quantity)` produces false "orphan" hits for Issue rows that do not originate from invoices (B2B issues, sample/destroy issues, customer-return adjustments) and false "valid" hits where two invoices share a batch and quantity.

### sp_Check_anomalyInvoiceDetails
- **Script:** `SP_Fix_Invoice_Anomaly_And_Sync.sql:43-78` (`ALTER PROCEDURE`; single version in repo; earlier live version not in repo). CONFIRMED (new body).
- **Purpose:** Detect an invoice whose header grand total disagrees with the sum of its detail net amounts, tolerant of `AdjustAmount`.
- **Parameters:** `@InvoiceId INT`, `@orderId INT` (both required).
- **Reads:** `tblInvoice` (OrderId, InvoiceId, TpGrandTotal, AdjustAmount, InvoiceNo, InvoiceDate), `tblInvoiceDetail` (NetAmount). Rule: returns a row when `ABS((TpGrandTotal - ISNULL(AdjustAmount,0)) - SUM(NetAmount)) > 10` (10 BDT tolerance) for invoices dated after `'2022-11-16'` (hard-coded cut-off). Result: TpGrandTotal, NetAmount, InvoiceNo, invoicedate.
- **Callers:** `Library.DAL/DoctorModule_DAL/CommonDataLoad.cs:1918` `CommonDataLoad.Check_anomalyInvoiceDetails` (params `@InvoiceId`, `@orderId` as strings), used by `Solution.Web/SInventory_UI/InvoiceCreationByOrder.aspx.cs:127,255`, `InvoiceCreationForCustomerByOrder.aspx.cs:2607,3136`, `TopSheetGenerate.aspx.cs:159` as a guard before confirming/using an invoice. **Behaviour in the caller:** in `InvoiceCreationByOrder.aspx.cs:127-131` (and `:255-259`) a returned anomaly row causes the page to call `aIn.DeleteProforma(InvoiceNo)` (-> `sp_Delete_ProformaInvoice`), i.e. this check's tolerance directly decides automatic deletion of proforma invoices.
- **Risks:** the previous body (pre-`AdjustAmount`) would flag every adjusted invoice; hard-coded date; ALTER fails if the SP is missing.

## A.6 Stock-transfer (B2B / STO) and SAP-challan SPs

The DC-to-DC transfer application code (C#) writes ledger rows: an Issue row `StockCondition='B2BIssue', Trans_Type='Issue', StockQty=-qty, ReferenceDCStoreId, ChalanDetailsId` (`Library.DAL/SInventory_DAL/ChalanDAL.cs:515-545`) and an Inward row `StockCondition='B2BReceive'` (`ChalanDAL.cs:122-160`). The SPs in `SP_NewStockTransferDcToDc_Related.sql` (dated 2026-09-01) implement a *different* model (in-place decrement, `StockCondition='Available'`), and `SP_Fix_STO_Chalan_Process.sql` (2026-09-02) realigns only `sp_Del_B2BDelete` to the ledger model.

### sp_Del_B2BDelete
Two versions (duplicate, **behavioural conflict**; v2 newer):
| Version | Script:line | Behaviour |
|---|---|---|
| v1 | `SP_NewStockTransferDcToDc_Related.sql:23-61` (header date 2026-09-01) | cursor over `tblChalanDetail` (DCStoreId, Quantity); for each row `UPDATE tblDCStore SET StockQty = StockQty + Quantity` (in-place restore), then DELETE `tblChalanDetail` and `tblChalanInfo`. No transaction, no TRY/CATCH. |
| v2 (**newer**) | `SP_Fix_STO_Chalan_Process.sql:25-64` (header date 2026-09-02, "Fix STO and Challan Process issues") | inside `BEGIN TRY / BEGIN TRANSACTION`: DELETE `tblDCStore` rows with `Trans_Type='Issue'` whose `ChalanDetailsId` is in the challan's details or whose `ChalanNo` equals the challan's `ChalanNo`; DELETE `tblDCStore` rows with `Trans_Type='Receive' AND StockCondition='B2BReceive'` for the same challan (destination already received); then DELETE `tblChalanDetail` and `tblChalanInfo`; `ROLLBACK` + `RAISERROR(msg,16,1)` on error. |
- **Parameter (both):** `@chalanId NVARCHAR(500)` (compared against the int `ChalanId` column -> implicit conversion; caller passes an int).
- **Why v2 exists / conflict:** v1's in-place `+Quantity` **and** the app's negative Issue ledger row that is left behind double-restore stock (inflated Receive rows - the defect found by `sp_Check_DCStore_StockAnomalies`); v1 also never removed the destination's Receive rows, so a deleted transferred challan left stock at the destination. Running `SP_NewStockTransferDcToDc_Related.sql` **after** the fix script re-creates v1 and re-introduces both defects. Which is deployed: UNKNOWN.
- **v2 risks:** the `ChalanNo IN (...)` alternative can delete Issue/Receive rows of *other* challans with the same number (or NULL/blank ChalanNo if data are dirty); `Trans_Type='Receive' AND StockCondition='B2BReceive'` does **not** match receive rows created by `sp_SAP_B2BStockReceiveByDc` (`StockCondition='Available'`), so those survive; no check that destination stock has already been sold (deleting a received challan can drive destination stock negative); no delivery/received-state guard.
- **Callers:** `Library.DAL/SInventory_DAL/B2BTransferViewDal.cs:50` `B2BTransferViewDal.DeleteChallanDetailById` (`DeleteAction`, which **swallows exceptions and returns false** - `ClsCommonInternalDAL.cs:73-84`, so v2's `RAISERROR` surfaces only as a `false` return, not a message) <- `Solution.Web/SInventory_UI/B2BTransferView.aspx.cs` (PAGE-407).

### sp_DeleteDwpotToWhChalan
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:72-79` (single version). Name spelled `sp_DeleteDwpotToWhChalan`. CONFIRMED.
- **Purpose:** Delete a Depot-to-Warehouse challan master and detail.
- **Parameter:** `@masterId INT`. **Writes:** DELETE `tblDepotToWHChalanInfo` WHERE `SChalanId=@masterId`; DELETE `tblDepotToWHChalanDetail` WHERE `SChalanId=@masterId`. **No stock reversal, no transaction, no state guard** (a transferred/received challan can be deleted; whether stock rows exist for it is UNKNOWN).
- **Callers:** `Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:165` `SCtoWHTransferDal.DeleteDepoToWHTransfer` (`DeleteAction`) <- `Solution.Web/SInventory_UI/DepoToWhTransferView.aspx.cs` (PAGE-409).

### sp_SAP_B2BStockReceiveByDc
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:90-171`. CONFIRMED. No caller in the repo (UNKNOWN: external SAP-integration job/service).
- **Purpose:** At the destination DC, receive a B2B challan into `tblDCStore`.
- **Parameter:** `@MasterId INT` (= `tblChalanInfo.ChalanId`). **Returns** the last new `DCStoreId` via `RETURN @ReceiveIdMAX` (integer return code, 0 if no rows).
- **Reads:** `tblChalanDetail`, `tblChalanInfo`, `tblDCStore` (source row: ExpDate, ReceiveDate, MfgDate), `tblProduct`, `tblCompanyUnit` (destination = `ToComUnitCode`). **Writes:** cursor -> INSERT `tblDCStore`(DCStoreId = MAX+1, StorageLocation '', ProductCode, ProductName, PackSize, BatchNo, TotalQuantity=Quantity, ExpDate, ReceiveDate, ChalanNo, ChalanDate, ComUnitId, StockQty=Quantity, DamageQty=0, StockRcvDate=GETDATE(), StockCondition='Available', MfgDate, ChalanDetailsId, Trans_Type='Receive').
- **Risks:** DCStoreId is generated with `MAX(DCStoreId)+1` (not identity) - race between concurrent inserts (duplicate/PK error); **no `CostPerUnit`/`TotalCostAmount`/`ReferenceDCStoreId` set** -> received stock carries no cost (valuation reports fall back to `tblUnitPrice.CostPrice`) and `StockCondition='Available'` (not `B2BReceive`) so `sp_Report_DCStore_StockValuationLedger` classifies it as "Receive From WH" unless `ChalanNo LIKE 'IDT%'`; **no idempotency** (calling twice doubles the stock); no transaction; `INNER` semantic missing (LEFT JOINs) so a challan whose `ToComUnitCode` matches no unit inserts rows with `ComUnitId=NULL`.

### sp_SAP_B2BChallanMasterInsert
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:182-266`. CONFIRMED. No caller in repo.
- **Purpose:** Create a `tblChalanInfo` header for a SAP stock-movement (STO) challan code.
- **Parameter:** `@ChallanNo NVARCHAR(500)`. **Returns** `RETURN @ReceiveIdMAX` (new ChalanId).
- **Reads:** cross-database `SAP_API_Data..tblSAP_StockMovementMaster` (challan_date, challan_code, from_plant_code, to_plant_code, StockMovementMasterId) and `..tblSAP_StockMovementDetail` (product_code, quantity), `tblCompanyUnit` (SAP_Code, ComUnitCode/Name/Address/Id) for both plants, `tblProduct` (SAP_Code; SAP product code is matched with `SUBSTRING(D.product_code, 13, LEN)`, i.e. the first 12 characters are stripped), `tblUnitPrice` (IsActive=1; UnitPrice, VATAmountPerUnit), `tblWHStockInMaster` (ChallanNo).
- **Rules:** only challans **not already received into the warehouse** (`challan_code NOT IN (SELECT ChallanNo FROM tblWHStockInMaster)`) with `SUM(quantity)>0`; TotalValue = `SUM(quantity * ISNULL(UnitPrice,1))`, TotalVat = `SUM(quantity * ISNULL(VATAmountPerUnit,1))` (**missing price/VAT default to 1 per unit**), GrandTotal = sum. Writes: INSERT `tblChalanInfo`(ChalanId = MAX+1, ChalanDate=GETDATE() (not the SAP challan_date), ChalanNo, TrackNo '', DriverName '', From/To ComUnitCode/Name/Address, TotalValue, TotalVat, GrandTotal, ManufacId=1, IsDeliver='False', FromComUnitId, Note='From SAP').
- **Risks:** not idempotent - a second call inserts a duplicate header (no existence check for the challan number in `tblChalanInfo`); `MAX+1` key generation; `WHERE UP.IsActive = 1` turns the LEFT JOIN into an inner join, so products without an active price are dropped; if several units share a `SAP_Code`, rows multiply.

### sp_SAP_B2BChallanDetailInsert
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:277-371`. CONFIRMED. No caller in repo.
- **Purpose:** Create challan detail rows for a SAP challan and **deduct stock from the sending DC (FIFO by expiry)**.
- **Parameters:** `@MasterId INT` (ChalanId), `@ChallanNo NVARCHAR(500)`.
- **Reads:** same SAP tables as above plus `tblDCStore` (StockQty>0, ComUnitId, ProductCode, ORDER BY ExpDate), `tblProduct`, `tblUnitPrice`, `tblCompanyUnit` (from plant). **Writes:** UPDATE `tblDCStore` SET `StockQty` (in-place decrement of the oldest-expiry Receive row(s) until the quantity is covered); INSERT `tblChalanDetail`(ChalanDetailsId=MAX+1, ProductCode, ProductName, Quantity=@Qty, BatchNo=SAP batch_no, UnitPrice, Value=@Qty*@Price, Vat, ValueWVat, ChalanId, DCStoreId).
- **Rules & defects (CONFIRMED in code, effect INFERRED):** (1) uses **in-place decrement** and no negative Issue row, i.e. it contradicts the ledger model used by the C# code and `sp_Del_B2BDelete` v2 (deleting the challan via v2 will *not* restore stock decremented here; the global "align Receive to TotalQuantity" fix would re-inflate it); (2) the detail row stores the **full `@Qty`** on every batch row it touches, so a line satisfied from two batches records 2x the quantity; (3) `@StockQuantity` is not reset when the `SELECT TOP 1` finds no row, so a stock shortage re-uses the stale value and can "issue" phantom quantity with `DCStoreId=0` (or spin until the residual is consumed); (4) `@BatchNo` from SAP is not used to pick the stock batch (FIFO by expiry only); (5) no stock-sufficiency check or error on shortage; (6) not idempotent; no transaction; (7) `DCStoreId=MAX+1` style ids for detail rows.

### sp_SAP_ChallanSendDetailByChalanId
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:381-398`. CONFIRMED. No caller in repo (consumed by an external SAP push).
- **Purpose:** Return the challan lines in SAP payload shape. **Parameter:** `@ChalanId INT`. **Reads:** `tblChalanDetail` (NOLOCK), `tblProduct` (SAP_Code), `tblStockUOM` (UOMSAPCode). **Result:** ChalanId, product_code (SAP code), batch_no, quantity, UoM, remarks ('').

### sp_SAP_Up_SAP_ChallanSendByChalanId
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:409-417`. **Parameter:** `@ChalanId INT`. **Writes:** `UPDATE tblChalanInfo SET SAP_ChallanSend = 1`. No guard/audit; no caller in repo.

### sp_SAP_Up_SAP_ChallanConfirmByChalanId
- **Script:** `SP_NewStockTransferDcToDc_Related.sql:428-436`. **Parameter:** `@ChalanId INT`. **Writes:** `UPDATE tblChalanInfo SET SAP_Challan_ConfirmationSend = 1`. No guard/audit; no caller in repo. (Both SAP flag columns are assumed to exist; no script adds them.)

### tbl_INV_CentralStoreStoIssue
(Despite the `tbl_` prefix this is a **stored procedure**, `CREATE PROCEDURE [dbo].[tbl_INV_CentralStoreStoIssue]`.) Two versions (duplicate, **semantic conflict**; "Corrected" newer):
| Version | Script:line | Behaviour |
|---|---|---|
| v1 "FIXED" | `SP_Fix_CentralStoreStoIssue.sql:12-191` (no `USE`; header lists 3 fixes) | Inserts the issue weighted-avg row only if none exists for `(Mst_Trans_Id, Ref_No, Company_Id, Item_Id)`; if it exists `@Weighted_Avg_Id_New` stays 0 and the batch row is skipped. Batch row: `Weighted_Avg_Id = @Ref_Weighted_Avg_Id` (**parent inbound** row), `Issue_Weighted_Avg_Id = @Weighted_Avg_Id_New` (the new issue row). Fallback parent lookup = first positive batch row with the same `Batch_No` (any product, no ORDER BY). |
| v2 "CORRECTED" | `SP_Fix_CentralStoreStoIssue_Corrected.sql:16-211` (`USE [SalesDisDB_Unicorn_DB]`, adds PRINT) | Re-uses an existing weighted-avg row id when present (self-healing re-run); batch row: `Weighted_Avg_Id = @Weighted_Avg_Id_New` (**the issue row**), `Issue_Weighted_Avg_Id = ISNULL(@Ref_Weighted_Avg_Id,0)` (parent) - **the two columns are swapped versus v1**; parent fallback restricted to the same product (`WA.Item_Code`) and `ORDER BY Batch_Stock_Id DESC`; duplicate guard `NOT EXISTS (Weighted_Avg_Id, StockInTransfarId, Batch_No)`. |
- **Which is newer:** v2 (name "Corrected", adds `USE`, idempotency). INFERRED. Whichever ran last is live: UNKNOWN. Because `sp_Report_NationalStockValuation_BatchDetails` reads batch rows via `Weighted_Avg_Id -> Trans_Date`, v1-created rows are dated at the original receipt (issue quantity netted against the wrong date); v2 dates them at the issue. Rows created before v2 (by v1 or by the earlier unfixed live SP) keep the old shape - **no data-migration script exists**.
- **Purpose:** Post a central-store (warehouse) STO picking issue: write a negative weighted-average ledger row and a negative batch-wise stock row per `tblStockInTransfar` line.
- **Parameter:** `@StockInTransfarId INT`.
- **Reads:** `tblStockInTransfar` (PickingQty, ReceiveDate, StockInTransfarId, ReceiveId, ReqId, BatchNo, MfgDate, ExpDate, ProductCode), `tblRequisition` (PickingNo, PickingDate, ReqNo), `Table_Inventory_TranscationTypes` (TranscationTypeId, RespectiveTable='tblStockInTransfar'), **cross-database `CSTL_ERPDB..Table_Inventory_Types`** (InventoryTypeId=1; note the database name `CSTL_ERPDB`, whereas all other scripts use `CSTL_ERPDB_Unicorn` -> UNKNOWN whether an alias/synonym exists), `tblProduct` (ProductId, ProductCode, ProductName, PackSize, CompanyId), `tblWearhouse` (WearhouseId by CompanyId), `tbl_INV_CentralStore_WeightedAvg` (latest per item/company), `tbl_INV_Central_BatchWiseStock`.
- **Writes:** INSERT `tbl_INV_CentralStore_WeightedAvg`(Mst_Trans_Id=StockInTransfarId, Trans_Id=ReceiveId, Item_Code, Ref_No=ReqNo (`@MaterialConversionCode`), Trans_Date=GETDATE(), Actual_Date=StockInTransfar.ReceiveDate, Entry_Date=PickingDate, Trans_Type_Id, Inv_Type_Id, Inv_Type, Company_Id, Warehouse_Id, Item_Id, OP_Qty/OP_Rate/OP_Amt, Curr_Qty (= -PickingQty), Curr_Rate (= opening rate), Curr_Amt, Close_Qty/Avg_Rate/Close_Amt, Weighted_Avg_Id_Issue=NULL); INSERT `tbl_INV_Central_BatchWiseStock`(Weighted_Avg_Id, Batch_No, Mfg_Date, Exp_Date, Trans_Qty=-PickingQty, Remarks='STO Picking', StockInTransfarId, Issue_Weighted_Avg_Id).
- **Business rules (weighted-average costing):** opening = latest ledger row for the item (Close_Qty, Close_Amt); opening rate = Close_Amt / Close_Qty (0 if Close_Qty <= 0); issues are costed at the opening (average) rate; closing qty/amount = opening + (-qty, -qty x rate); new average = closing amount / closing qty (0 if closing qty <= 0). `MfgDate/ExpDate` default to `GETDATE()` when NULL.
- **Transactions/errors:** none (cursor of multiple inserts, no TRAN/TRY); no check that stock is sufficient (`Close_Qty` may go negative); `INNER JOIN tblWearhouse ON PD.CompanyId = WH.CompanyId` multiplies cursor rows when a company has several warehouses (v1/v2 guard the duplicate weighted-avg row by (Mst_Trans_Id, Ref_No, Company, Item) so the first warehouse wins arbitrarily). `LEFT JOIN Table_Inventory_TranscationTypes` has no relation to the line, so multiple matching rows duplicate lines.
- **Callers:** `Library.DAL/SInventory_DAL/RequisitionDAL.cs:413` `RequisitionDAL.ProcessWeightedAvg` (`UpdateAction`) <- `Library.BLL/SInventory_BLL/RequisitionBLL.cs:344,381`.

## A.7 Accounts audit and money receipt

### sp_Audit_Sales_Accounts_Voucher_Posting
- **Script:** `SP_Audit_Sales_Accounts_Voucher_Posting.sql:23-435` (single version; header "Generated Date: 03-Sep-2026", "Read-Only Audit"). No caller in the repo (DBA/ops tool). CONFIRMED.
- **Purpose:** One-click reconciliation of ERP transactions to accounts vouchers in `CSTL_ACCDB_Unicorn`: existence, detail lines, Dr/Cr balance, value accuracy, for five operations.
- **Parameters:** `@OperationType NVARCHAR(50)='ALL'` (`'ALL','MATERIAL_CONVERSION','DELIVERY_CONFIRMATION','CUSTOMER_PAYMENT','SALES_RETURN','NON_TRANSACTIONAL_RETURN'`), `@FromDate DATETIME=NULL`, `@ToDate DATETIME=NULL` (moved to end of day), `@ShowOnlyMismatches BIT=0`.
- **Voucher key convention discovered (`TableID` / `CommonPK_ID` in `tblDebitCreditVoucherMaster`):** 2013 = material-conversion receive (`tblWHStockInMaster.WHStockInMasterID`); 2014 = delivery confirmation (`tblInvoice.InvoiceId`, sales voucher with `IsInventoryValuation=0/NULL` and COGS voucher with `=1`); 2016 = destroy / non-transactional return (`tblDestroyProduct.DestroyId`); 2017 = customer payment (`tblCustomerPay.CustPayId`); 2018 = sales return (`tblReturnInvoice.ReturnInvoiceId`); 2023 = sales-return inventory valuation.
- **Reads:** `tblWHStockInMaster` (WHStockInMasterID, WHStockInCode, WHStockInDate, Status, TotalValue), `tblInvoice` (InvoiceId, InvoiceNo, DelivaryInvoiceNo, InvoiceDate, DeliveryInvoiceStatus, DeliveryTpGrandTotal), `tblCustomerPay` (CustPayId, RefNo, PaymentDate, PayType), `tblCustPayDetail` (PaymentAmount, Discount, AIT), `tblReturnInvoice` (ReturnInvoiceId, ReturnInvoiceNo, ReturnInvoiceDate, TpTotal, TpDiscount), `tblDestroyProduct` (DestroyId, StockOutDate, StockOutQty, Reason, DcFreezeId), `tblDCStoreFreeze`, `tblDCStore` (TempReceiveId), `tblCentralStore` (UnitPrice), and cross-db `CSTL_ACCDB_Unicorn.dbo.tblDebitCreditVoucherMaster` (VoucherMasterId, VoucherCode, PostingDate, TotalAmountBDT, TableID, CommonPK_ID, IsInventoryValuation, SalesReturnId, DestroyId, VoucherType) and `..tblDebitCreditVoucherDetail` (DebitAmountBDT, CreditAmountBDT).
- **Audit statuses:** `MISSING_VOUCHER`, `MISSING_VOUCHER_DETAILS`, `DR_CR_UNBALANCED` (`SUM(Debit)<>SUM(Credit)`, exact), `MASTER_VALUE_MISMATCH` (|doc amount - master amount| > 1.00 BDT), `DETAIL_VALUE_MISMATCH`, delivery-specific `MISSING_SALES_VOUCHER/MISSING_SALES_DETAILS/SALES_DR_CR_UNBALANCED/SALES_VALUE_MISMATCH/COGS_DR_CR_UNBALANCED`, else `OK`. Delivery rows are audited only when delivered (`DelivaryInvoiceNo IS NOT NULL` or `DeliveryInvoiceStatus IN ('Full','Partial')` or a voucher exists).
- **Temp tables:** `#AuditSummary` and one temp table per operation (`#TempMatConv`, `#TempDelivery`, `#TempPayment`, `#TempSalesReturn`, `#TempDestroy`), dropped at the end of each block.
- **Result sets:** one detail grid per selected operation (per-operation column set) and a final KPI summary: Sales / Inventory Operation, Total Records, Vouchers Posted, Missing Vouchers, Missing Details, Dr/Cr Unbalanced, Value Mismatch, 100% OK Count, Health (%) Score (`OK / Total x 100`, 100 when no rows).
- **Findings about the posting SPs (CONFIRMED from cross-reading):** (1) the sales-return check compares `TpTotal` with the voucher amount, whereas `sp_SalesReturnPosting` posts `TpGrandTotal` (or detail totals) and the audit's own `ReturnNetAmount` (`TpTotal - TpDiscount`) is not the compared value -> false mismatches when discount/deduction exist; (2) `sp_DeliverySalesReturnPosting` also uses `TableID=2018` with `CommonPK_ID = CustReturnId` (a *different* table's key) - the audit's join `(V.TableID=2018 OR V.SalesReturnId=RI.ReturnInvoiceId) AND (V.CommonPK_ID=RI.ReturnInvoiceId OR V.SalesReturnId=...)` can therefore match a customer-return voucher to an unrelated return invoice with the same numeric id, and `sp_SalesReturnPosting`'s `NOT EXISTS (TableID=2018 AND CommonPK_ID=ReturnInvoiceId ...)` guard can wrongly skip posting; (3) the audit has no payment-side check of `sp_Sales_DeliveryCOGSPosting`; (4) `TableID=2017` is assumed while `sp_Sales_CustomerPaymentPosting` takes the id from `tbl_AccIntegrationTable` (value not visible in repo).
- **Risks:** `WITH (NOLOCK)` (fine for audit); cross-db object names hard-coded; `SELECT ... INTO` temp tables per operation with no error handling; `TotalDebitBDT`/`TotalCreditBDT` compared without rounding tolerance.

### sp_Get_MoneyReceiptReportAfterPaymentList
- **Script:** `SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql:18-55` (`DROP` if exists + `CREATE`; single version). CONFIRMED.
- **Purpose:** Feed the money-receipt grid/report after a customer payment.
- **Parameters:** `@Parm NVARCHAR(MAX) = ''` (raw WHERE fragment appended after `WHERE 1=1`; NULL -> '').
- **Reads:** `tblCustPayDetail` (CustPayDetailId, CustPayId, InvoiceId, custPaymentDate, PaymentAmount, TPAmount, VATAmount), `tblCustomerPay` (RefNo), `tblInvoice` (InvoiceId, OrderNo, OrderId, InvoiceNo, InvoiceDate, DelivaryInvoiceNo, UpdateDate, OrderSenderCode, OrderSenderName, CustomerMasterId, ComUnitId), `tblCustMaster` (CustomerCode, CustomerName), `tblOrder` (OrderSenderCode/Name), `tblCompanyUnit` (ComUnitCode, ComUnitName, Address).
- **Rules:** `PaymentNo = RefNo` if not blank else the CustPayId; `ReceiptNo = RefNo` if not blank else `'MR-' + 8-digit zero-padded CustPayId`; `DelivaryInvoiceDate = ISNULL(UpdateDate, InvoiceDate)`; `OrderBy = OrderSender code : name` from the order, falling back to the invoice; `SELECT DISTINCT`.
- **Dynamic SQL:** `@Parm` injected verbatim into `sp_executesql` (`:51`). Ordered by `CustPayDetailId DESC`.
- **Result columns:** CustPayDetailId, CustPayId, InvoiceId, OrderNo, CustomerCode, CustomerName, InvoiceNo, InvoiceDate (dd/mm/yyyy), DelivaryInvoiceNo, DelivaryInvoiceDate, PaymentNo, PaymentDate, PaymentAmount, TPAmount, VatAmount, ReceiptNo, ComUnitName (`code : name`), Address, OrderBy.
- **Callers:** `Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:69` `CmnCrystaltoView.GetMoneyReceiptAfterPaymentDAL` (page `Solution.Web/SInventory_UI/MoneyReceiptAfterPayment.aspx.cs`, PAGE-330), `Library.DAL/SInventory_DAL/InvoiceDAL.cs:80` and `SalesReturnDAL.cs:139` `MoneyReceiptAfterPaymentInfo` (viewer `Solution.Web/SInventory_RPTVIEW/MoneyReceiptViewerForAfterPayment.aspx.cs`, PAGE-304).
- **Risks:** `DISTINCT` on wide rows; date/format conversion in SQL; `@Parm` injection surface; "MR-" number is derived, not stored (changes if `RefNo` is later filled).

## A.8 SAP provider-dropout integration (`Library.DAL/SAP_IntegrationDAL/sql/`)

### sp_Get_ProviderDropoutIntrigrationList
- **Script:** `Library.DAL/SAP_IntegrationDAL/sql/sp_Get_ProviderDropoutIntrigrationList.sql:1-30` (create-stub-if-missing then `ALTER PROCEDURE` idiom; single version). CONFIRMED.
- **Purpose:** List provider-dropout notifications received from an external program system.
- **Parameters:** none. **Reads:** `tblProviderDropoutIntrigration` (providerIDropoutIntrigrationd, providerId, programShortName, providerCode, providerName, mobileNo, nid, email, outlet, dropoutReason, insertedAt, IsApprove, IsApproveBy, ApproveDate). **Result:** those columns, `IsApprove` coalesced with `ISNULL(IsApprove,0)`; `ORDER BY insertedAt DESC, providerIDropoutIntrigrationd DESC`. No filtering/paging.
- **Callers:** `Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs:86` `SAP_IntrigationPointDAL.GetProviderDropoutIntrigrationListDAL` <- `Solution.Web/eProgram_UI/ProviderDropoutRequestList.aspx.cs` (PAGE-694). Column spelling `providerIDropoutIntrigrationd` (sic, trailing `d`) is the actual key.

### sp_Up_ProviderDropoutIntrigrationApprove
- **Script:** `Library.DAL/SAP_IntegrationDAL/sql/sp_Up_ProviderDropoutIntrigrationApprove.sql:1-22`. CONFIRMED.
- **Purpose:** Approve one provider-dropout request. **Parameters:** `@providerIDropoutIntrigrationd BIGINT`, `@ApproveBy NVARCHAR(50)`. **Writes:** `UPDATE tblProviderDropoutIntrigration SET IsApprove=1, IsApproveBy=@ApproveBy, ApproveDate=SYSUTCDATETIME() WHERE id=@id AND ISNULL(IsApprove,0)=0` (idempotent: an already approved row is not touched; **ApproveDate is UTC** while `insertedAt` is unknown). No rows-affected result, no error if the id does not exist, no authorisation check inside (relies on the page). No transaction (single statement).
- **Callers:** `SAP_IntrigationPointDAL.cs:624` `ApproveProviderDropoutIntrigrationDAL` <- `ProviderDropoutRequestList.aspx.cs` (PAGE-694). The approval only sets flags - what triggers downstream SAP/eProgram processing after approval is UNKNOWN.

<!-- GENERATED-INDEX-BEGIN: sp -->
## Generated stored-procedure interface index (1034 names)

Source: every string literal used as a stored-procedure name in `Library.DAL`, `Solution.Web` code-behind and `App_Code` (first argument of `GetSqlDataReader/GetDataTable/GetDataSet/SaveData*/UpdateData/DeleteData/RunStoreProcedure/...`, `SqlCommand(...)`/`CommandText`, `EXEC name` in inline SQL, and literals matching `sp_*`). **Parameters** = `SqlParameter` names in the calling DAL/page method(s) (union). **Definition** = where a `CREATE/ALTER PROCEDURE` exists in the repo scripts; `NOT IN REPO` means the body could not be read. Tables/columns/validation of SPs without a body are UNKNOWN.

| Stored procedure | Parameters (from callers) | Callers (class.method) | Pages (count) | Modules | Definition | Name check |
|---|---|---|---|---|---|---|
| AppMonitoringViewList | @Parameter | RSMSetupDal.Get_AppMonitoringList | 0 | — | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| AppMonitoringViewList_new | @Parameter | RSMSetupDal.Get_AppMonitoringList_new | 1 (PAGE-676) | M17 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicDateByDateRange | @frmDate, @toDate | CmnCrystaltoView.GetDynamicDateByDateRange | 4 (PAGE-671, PAGE-672, PAGE-673, PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicDatebyMonthByDateRange | @frmDate, @toDate | OrderTrackingDAL.GetDatefromMonthYearStuff | 1 (PAGE-210) | M06 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicDatebyMonthYear | @Month, @Year | CmnCrystaltoView.GetDatefromMonthYear | 0 | — | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicDatebyMonthYearForStuff | @Month, @Year | CmnCrystaltoView.GetDatefromMonthYearStuff | 0 | — | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotBrandWiseDCR | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetDoctorBrandWiseList | 2 (PAGE-671, PAGE-673) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotBrandWiseRX_new | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @DoctorTypeSelect, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Yea | CmnCrystaltoView.GetRXDoctorBrandWiseList | 1 (PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDoctorWiseCVR_New_ForSearch | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetCVRDoctorWiseDayList | 1 (PAGE-671) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDoctorWiseDCP | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetDCPDoctorWiseDayList | 1 (PAGE-672) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDoctorWiseDCPCustomerWise | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetCusDCPDoctorWiseDayList | 1 (PAGE-672) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDoctorWiseDCR_New_ForSearch | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetDCRDoctorWiseDayList | 1 (PAGE-673) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDoctorWiseDoctorVisitPlan | @param | DoctorVisitDAL.DynamicPivotDoctorWiseDoctorVisitPlan | 1 (PAGE-664) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDoctorWiseRX_New | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @DoctorTypeSelect, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Yea | CmnCrystaltoView.GetRXDoctorWiseDayList | 1 (PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotDWSP | @ColumnToPivot, @ListToPivot, @Month, @Year | CmnCrystaltoView.GetDWSPMonthlyList | 0 | — | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotProductdWiseDCR | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetDoctorProductWiseList | 2 (PAGE-671, PAGE-673) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotProductdWiseRX_New | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @DoctorTypeSelect, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Yea | CmnCrystaltoView.GetRXDoctorProductWiseList | 1 (PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotUserandProductdWiseRX_New | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @DoctorTypeSelect, @ListToPivot, @Month, @PharmaPlatform, @ProID, @ProviderType, @TeritorySele | CmnCrystaltoView.GetRXUserProductWiseList | 1 (PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotUserWiseDCP | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetDCPUserWiseList | 1 (PAGE-672) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotUserWiseDCR | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Year, @ZoneSelect | CmnCrystaltoView.GetDCRUserWiseList | 2 (PAGE-671, PAGE-673) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicPivotUserWiseRX_New | @ApprovalStatus, @AreaSelect, @ColumnToPivot, @DoctorTypeSelect, @ListToPivot, @Month, @PharmaPlatform, @ProviderType, @TeritorySelect, @Yea | CmnCrystaltoView.GetRXUserWiseList | 1 (PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| DynamicVisitStatusReport | @ApprovalStatus, @AreaSelect, @Brand, @ColumnToPivot, @DCType, @DoctorTypeSelect, @EmpID, @ListToPivot, @Month, @PharmaPlatform, @ProviderTy | CmnCrystaltoView.GetDynamicVisitStatusReport | 2 (PAGE-671, PAGE-673) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| GetDynamicSalesReportList | @Area, @Terr, @Type, @ZonId, @fromdate, @grpId, @todate | CmnCrystaltoView.GetDynamicSalesReportListDAL | 1 (PAGE-505) | M13 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| GetDynamicVisitStatusReportRX | @ApprovalStatus, @AreaSelect, @Brand, @ColumnToPivot, @DoctorTypeSelect, @EmpID, @ListToPivot, @Month, @PharmaPlatform, @ProID, @ProviderTyp | CmnCrystaltoView.GetDynamicRXStatusReport | 1 (PAGE-675) | M16 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| MakeRESTRequest | @RcvBy, @StoNo | SAP_IntrigationPointDAL.MakeRESTRequestWithUpdateChallan | 2 (PAGE-413, PAGE-423) | M11 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| msdb | @JobName | ArchiveDbConnectRepository.StartJob | 1 (PAGE-024) | M02 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| sp_Acc_MaterialConversionReceive | @ApprovalBy, @ApprovalDate, @ApprovalStatus, @MasterId | WarehouseStockInApprovalDal.ApproveWarehouseStockInMaster | 1 (PAGE-386) | M10 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_customerType_ById | @CustomerTypeId, @InactiveBy | CustomerTypeDal.ActiveInactive_DepartmentInfo | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_Department | @DeptId, @InactiveBy | DepartmentDal.ActiveInactive_DepartmentInfo; RouterSetup.ActiveInactive_DepartmentInfo | 1 (PAGE-567) | M15 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_employeeDesignation_ById | @InactiveBy | DesignationDal.ActiveInactive_DesignationInfo | 1 (PAGE-569) | M15 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_FinancialYear | @Id, @InactiveBy | FinancialYearDal.ActiveInactive_FinancialYearInfo | 1 (PAGE-040) | M03 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_GroupInfo | @Id, @InactiveBy | GroupSetupDal.ActiveInactive_DepartmentInfo | 2 (PAGE-047, PAGE-106) | M03,M04 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_Holiday | @HolidayId, @InactiveBy | HolidayDal.ActiveInactive_emplopyeeleave | 1 (PAGE-576) | M15 | NOT IN REPO | sp-convention |
| sp_ActiveInactive_Programtype | @DeptId, @InactiveBy | ProgramDal.ActiveInactive_DepartmentInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_ActiveInactive_Stationtype | @DeptId, @InactiveBy | StationDal.ActiveInactive_DepartmentInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_ActiveInactive_Transport | @InactiveBy, @LeaveTypeId | LeaveDal.ActiveInactive_emplopyeeleave | 2 (PAGE-579, PAGE-580) | M15 | NOT IN REPO | sp-convention |
| sp_Approve_DoctorInformation | @ApprovedBy, @DoctorId | Setup2DAL.ApprovalDoctorInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_Approve_TADAClaim | @ApprovedBy, @TadaID | Setup2DAL.ApprovalTADAClaim | 0 | — | NOT IN REPO | sp-convention |
| sp_ApproveAttendanceInformation | @ApprovedBy, @AttendanceId, @Status | AttendanceDAL.ApprovalAttendanceListInfo | 1 (PAGE-564) | M15 | NOT IN REPO | sp-convention |
| sp_ApproveCustomerInformation | @ApprovedBy, @CustomerMasterId, @Status | CustomerInfoDAL.ApprovalCustomerListInfo; DoctorDAL.ApprovalCustomerListInfo; MarketStructureTransferDAL.ApprovalCustomerListInfo (+1) | 0 | — | NOT IN REPO | sp-convention |
| sp_ApproveDoctorTourPlanMaster | @ApprovedBy, @Status, @TPMaster | DoctorVisitDAL.Approve_DoctorPlanList | 0 | — | NOT IN REPO | sp-convention |
| sp_ApproveExpenseClaimInformation | @ApprovedBy, @Status, @TPMaster | Setup2DAL.Approve_ExpenseClaimList | 0 | — | NOT IN REPO | sp-convention |
| sp_ApproveMileageClaimInformation | @ApprovedBy, @Status, @TPMaster | Setup2DAL.Approve_MileageClaimList | 0 | — | NOT IN REPO | sp-convention |
| sp_ApprovePrescriptionInformation | @ApprovedBy, @Status, @TPMaster | Setup2DAL.Approve_PrescriptionList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_ApproveTourPlanInformation | @ApprovedBy, @Status, @TPMaster | TourTypeDal.Approve_TourPlanList | 2 (PAGE-592, PAGE-679) | M15,M17 | NOT IN REPO | sp-convention |
| sp_ApproveVisitPlanInformation | @ApprovedBy, @Status, @TPMaster | TourTypeDal.Approve_VisitPlanList | 1 (PAGE-607) | M15 | NOT IN REPO | sp-convention |
| sp_AreawiseDailyOpeningClosingStockDepowise | @CiD, @fromDate, @toDate | AreaWiseMonthlyInventoryReportDal.GetAreaWiseMonthlyInventoryReport | 1 (PAGE-467) | M13 | NOT IN REPO | sp-convention |
| sp_AreawiseDailyOpeningClosingStockNational | @fromDate, @toDate | AreaWiseMonthlyInventoryReportDal.GetAreaWiseMonthlyInventoryNationalReport | 1 (PAGE-467) | M13 | NOT IN REPO | sp-convention |
| sp_AutoInvoiceGeneration | @OrderCodeInPut, @UserId | InvoiceDAL.SaveProformaInvoice; SalesReturnDAL.SaveProformaInvoice | 1 (PAGE-272) | M07 | NOT IN REPO | sp-convention |
| sp_BeforesndSalesReturnList | @param | SalesReturnNewDAL.LoadOrderWithInvoiceSP | 1 (PAGE-296) | M08 | NOT IN REPO | sp-convention |
| sp_BusinessSummaryMISReport | @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewiseGyash | 2 (PAGE-499, PAGE-533) | M13 | NOT IN REPO | sp-convention |
| sp_BusinessSummaryMISReport_All_New_New | @Area, @Terr, @Type, @ZoneId, @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewiseGyash | 2 (PAGE-499, PAGE-533) | M13 | NOT IN REPO | sp-convention |
| sp_BusinessSummaryMISReport_TT | @Area, @Terr, @Type, @ZoneId, @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewiseGyash__New | 1 (PAGE-535) | M13 | NOT IN REPO | sp-convention |
| sp_Campaign_SetIsAdjustedProbationDiscount | @InvoiceId | CustPaymentDAL.UpdateAdjustment | 0 | — | NOT IN REPO | sp-convention |
| sp_CampaignUpdateFromPage | @Ids | BonusCampaignNewDAL.SYncBonusCampaign | 1 (PAGE-116) | M04 | NOT IN REPO | sp-convention |
| sp_Check_anomalyInvoiceDetails | @InvoiceId, @orderId | CommonDataLoad.Check_anomalyInvoiceDetails | 3 (PAGE-217, PAGE-218, PAGE-277) | M06,M07 | ['SP_Fix_Invoice_Anomaly_And_Sync.sql:42'] | sp-convention |
| sp_check_AreaInfo | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaName, @createdBy, @id, @isActive, @remarks, @zoneId | Setup2DAL.SaveAreaInfo | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_check_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.InsertUpdate_ASMInfoDAL; RSMSetupDal.Save_ASMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_check_Count_MarketStructure | @MasterId, @PageName | Setup2DAL.GetMarketCountDataDAL | 1 (PAGE-042) | M03 | NOT IN REPO | sp-convention |
| sp_check_CustomerType | @CustomerCategoryId, @CustomerType, @EntryBy, @IsActive, @IsCampaign, @IsDefault, @UpdateBy, @id | CustomerTypeDal.Save_DepartmentInfo | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_check_da_UserInfo | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_check_da_UserInfo_Save | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_check_Department | @DepartmentName, @EntryBy, @IsActive, @MasterId, @PageName, @UpdateBy, @id | DepartmentDal.Save_DepartmentInfo | 1 (PAGE-566) | M15 | NOT IN REPO | sp-convention |
| sp_check_Designation | @DesigName, @EntryBy, @IsActive, @MasterId, @PageName, @UpdateBy, @id | DesignationDal.Save_DesignationInfo | 1 (PAGE-568) | M15 | NOT IN REPO | sp-convention |
| sp_check_DistictInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id | ThanaDal.Save_DistictInfo | 1 (PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_check_DoctorCategory | @Activedate, @CategoryName, @EntryBy, @UpdateBy | DoctorCategoryDal.SaveDoctorCategory; Setup2DAL.SaveDoctorCategory | 1 (PAGE-639) | M16 | NOT IN REPO | sp-convention |
| sp_check_DoctorChamber | @Activedate, @ChamberName, @EntryBy, @UpdateBy | DoctorChamberDal.SaveDoctorChamber; Setup2DAL.SaveDoctorChamber | 1 (PAGE-637) | M16 | NOT IN REPO | sp-convention |
| sp_check_DoctorDegree | @Activedate, @DegreeName, @DoctorTypeId, @EntryBy, @UpdateBy | DoctorDegreeDal.SaveDoctorDegree; Setup2DAL.SaveDoctorDegree | 1 (PAGE-641) | M16 | NOT IN REPO | sp-convention |
| sp_check_DoctorDesignation | @Activedate, @DesignationName, @EntryBy, @UpdateBy | DoctorDesignationDal.SaveDoctorDesignation; Setup2DAL.SaveDoctorDesignation | 1 (PAGE-643) | M16 | NOT IN REPO | sp-convention |
| sp_check_DoctorPatientType | @Activedate, @EntryBy, @IsActive, @PatientType, @PatientTypeId, @UpdateBy | PatientTypeDal.SaveDoctorpatientType; Setup2DAL.SaveDoctorpatientType | 1 (PAGE-647) | M16 | NOT IN REPO | sp-convention |
| sp_check_DoctorSpecialDay | @Activedate, @EntryBy, @IsActive, @SpecialDay, @SpecialDayId, @UpdateBy | DoctorSpecailDayDal.SaveDoctorSpeacialDay; Setup2DAL.SaveDoctorSpeacialDay | 1 (PAGE-651) | M16 | NOT IN REPO | sp-convention |
| sp_check_DoctorSpeciality | @Activedate, @EntryBy, @IsActive, @SpecialityId, @SpecialityName, @UpdateBy | DoctorSpecialityDal.SaveDoctorSpeaciality; Setup2DAL.SaveDoctorSpeaciality | 1 (PAGE-645) | M16 | NOT IN REPO | sp-convention |
| sp_Check_Duplicate_InvoiceFinalPayment | @InvoiceId, @PaymentAmount | OrderInfoDAL.CheckInvoiceCustpayment; OrderInfoDALSalesReturn.CheckInvoiceCustpayment; SalesReturnNewDAL.CheckInvoiceCustpayment | 5 (PAGE-311, PAGE-312, PAGE-314, PAGE-315…) | M09 | NOT IN REPO | sp-convention |
| sp_check_Employee_ShiftInfos | @Activedate, @EntryBy, @ShiftInTime, @ShiftOutTime, @ShiftText, @UpdateBy | AttendanceDAL.Save_ShiftInfo | 1 (PAGE-585) | M15 | NOT IN REPO | sp-convention |
| sp_check_EmployeeAllowance | @EmpInfoId, @EntryBy, @IsActive, @MasterId, @MonthlyAllowance, @MonthlyAllowanceId, @MonthlyAllowanceName, @Name, @PageName, @UpdateBy, @Use | Setup2DAL.SaveMonthlyAllowance | 1 (PAGE-583) | M15 | NOT IN REPO | sp-convention |
| sp_check_EmployeeInfo | @AddressPermanent, @AddressPresent, @AllowanceId, @BloodGroup, @CellNumber, @CompanyId, @CompanyUnitId, @DateOfBirth, @DepartmentId, @Design | EmployeeInformationDaL.SaveEmployeeInformation | 3 (PAGE-613, PAGE-614, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_check_ExpenseType | @EmpNameMult, @EntryBy, @ExpenseAmount, @ExpenseTypDetailsId, @ExpenseTypeId, @ExpenseTypeName, @FieldName, @ImageRequired, @IsActive, @IsRe | SetupDAL.SaveExpenseTypeMaster | 1 (PAGE-572) | M15 | NOT IN REPO | sp-convention |
| sp_check_GenericGroup | @EntryBy, @GenericGroupName, @InactiveBy, @InactiveDate, @IsActive, @MasterId, @PageName, @UpdateBy, @id | GenericGroupDal.Save_GenericGroup | 1 (PAGE-103) | M04 | NOT IN REPO | sp-convention |
| sp_check_GroupInfo | @CodeStr, @EntryBy, @GroupName, @IsActive, @MasterId, @Name, @NationalId, @PageName, @UpdateBy, @id | GroupSetupDal.Save_DepartmentInfo | 2 (PAGE-046, PAGE-105) | M03,M04 | NOT IN REPO | sp-convention |
| sp_check_Holiday | @DayName, @EntryBy, @FiscalYear, @HolidayDate, @HolidayId, @HolidayToDate, @IsActive, @Name, @UpdateBy, @id | HolidayDal.Save_LeaveInfo | 1 (PAGE-575) | M15 | NOT IN REPO | sp-convention |
| sp_check_LeaveInfo | @EntryBy, @IsActive, @LeaveDays, @LeaveTypeId, @LeaveTypeName, @Name, @UpdateBy, @id | LeaveDal.Save_LeaveInfo | 2 (PAGE-577, PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_check_MarketInfo | @MarketId, @MasterId, @Name, @PageName, @StationTypeId, @SubTerritoryId, @ThanaId, @UserRoleID, @acInAcDate, @createdBy, @id, @isActive, @re | Setup2DAL.SaveMarket | 1 (PAGE-042) | M03 | NOT IN REPO | sp-convention |
| sp_check_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Insert_Update_MIOInfoDAL; RSMSetupDal.Save_MIOInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_check_NSMHeadInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_NSMHeadInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_check_NSMInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_NSMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_check_PrescriptionType | @Activedate, @EntryBy, @IsActive, @PrescriptionType, @PrescriptionTypeId, @UpdateBy, PrescriptionType, PrescriptionTypeId | PrescriptionTypeDal.Save_PrescriptionType | 1 (PAGE-649) | M16 | NOT IN REPO | sp-convention |
| sp_check_ProductLine | @EntryBy, @InactiveBy, @InactiveDate, @IsActive, @LineName, @UpdateBy, @id | TherapueticGroupDal.Save_ProductLine | 1 (PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_check_Programtype | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @UpdateBy, @id | ProgramDal.Save_DepartmentInfo | 2 (PAGE-175, PAGE-177) | M05 | NOT IN REPO | sp-convention |
| sp_check_RouteInformationMArket | @AreaId, @DAAmount, @DAId, @DCId, @Distance, @EntryBy, @GroupId, @IsSubDepo, @MarketId, @Name, @RegionId, @RouteInformationMasterId, @RouteN | RouteInformationDAL.SaveRouteInformation | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_check_RouterMaster | @EntryBy, @RouterMasterId, @RouterName, @UpdateBy, @id | RouterSetup.Save_RouterSetup | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_check_RSMInfo | @CompanyId, @EmployeeId, @RSMId, @RegionId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_RSMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_check_SMCtype | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @SMCType, @UpdateBy, @id | ProgramDal.Save_SMCTypeInfo | 1 (PAGE-175) | M05 | NOT IN REPO | sp-convention |
| sp_check_StationType | @EndTime, @EntryBy, @IsActive, @StartTime, @StationTypeName, @UpdateBy, @id | StationDal.Save_DepartmentInfo | 1 (PAGE-178) | M05 | NOT IN REPO | sp-convention |
| sp_check_SubTerritoryInfo | @MasterId, @Name, @PageName, @TerritoryId, @acInAcDate, @createdBy, @id, @isActive | Setup2DAL.SaveSubTerritory | 2 (PAGE-057, PAGE-679) | M03,M17 | NOT IN REPO | sp-convention |
| sp_check_TADAMarketRuleConfiguration | @AreaId, @DAAmount, @EntryBy, @GroupId, @IsActive, @IsBoth, @IsMarketWise, @IsRoleWise, @MarketId, @TAAmount, @TADAMarketRuleConfigId, @Terr | Setup2DAL.SaveTADAMarketRuleConfiguration | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_check_TerritoryInfo | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaId, @createdBy, @id, @isActive, @remarks | Setup2DAL.SaveTerritory | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_check_ThanaInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id | ThanaDal.SaveThanaInfo | 2 (PAGE-090, PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_check_TherapeuticGroup | @EntryBy, @InactiveBy, @InactiveDate, @IsActive, @TherapeuticGroupName, @UpdateBy, @id | TherapueticGroupDal.Save_TherapueticGroup | 2 (PAGE-107, PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_Check_TourSetupEmployeeList | @CountNo, @EmpInfoId, @EntryBy, @EntryDate, @IsEmployeeWise, @IsRoleWise, @RoleTypeId, @StationTypeId, @TourSetupEmployeeId, @UpdateBy, @Upd | TourSetupForEmployeeDAL.SaveDAInfo | 2 (PAGE-616, PAGE-617) | M15 | NOT IN REPO | sp-convention |
| sp_Check_TourSetupEmployeeListRoleType | @CountNo, @EmpInfoId, @EntryBy, @EntryDate, @IsEmployeeWise, @IsRoleWise, @RoleTypeId, @StationTypeId, @TourSetupEmployeeId, @UpdateBy, @Upd | TourSetupForEmployeeDAL.SaveDAInfo | 2 (PAGE-616, PAGE-617) | M15 | NOT IN REPO | sp-convention |
| sp_check_Transport | @AllowedMilagePerKM, @EntryBy, @IsActive, @MasterId, @PageName, @TransportId, @TransportName, @UpdateBy | Setup2DAL.SaveTransport; SetupDAL.SaveTransport | 1 (PAGE-605) | M15 | NOT IN REPO | sp-convention |
| sp_check_UserInfo_Save | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_check_UserRoleInfo | @RoleName, @RoleTypeId, @UserRoleID, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_UserRoleInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_check_Vali_EmployeeInfoEntry | @MasterId, @PageName | UserInfoDAL.check_UserInfoByEmpCode | 2 (PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_check_Vali_EmployeeInfoUpdate | @EmpId, @EmpMasterCode | UserInfoDAL.check_UserInfoByEmpCodeUpdate | 1 (PAGE-614) | M15 | NOT IN REPO | sp-convention |
| sp_check_Vali_MarketStructure | @AllowedMilagePerKM, @CodeStr, @DepartmentName, @DesigName, @EmpInfoId, @EmpNameMult, @EntryBy, @ExpenseAmount, @ExpenseTypDetailsId, @Expen | CommonDataLoad.GetValidationCheck; DepartmentDal.Save_DepartmentInfo; DesignationDal.Save_DesignationInfo (+11) | 16 (PAGE-042, PAGE-046, PAGE-057, PAGE-064…) | M15,M03,M04 | NOT IN REPO | sp-convention |
| sp_check_Vali_PromoMIOTag | @EmpInfoId, @PageName, @PromoGroupId | CommonDataLoad.GetMIOTagValidationCheck | 1 (PAGE-125) | M04 | NOT IN REPO | sp-convention |
| sp_check_ZoneInfo | @CodeStr, @GroupId, @MasterId, @Name, @PageName, @acInAcDate, @createdBy, @id, @isActive, @remarks, @zoneId, @zoneName | ZoneSetupDAL.SaveZoneInfo | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_checkIsDefault_CustomerType | @CustomerCategoryId, @CustomerType, @EntryBy, @IsActive, @IsCampaign, @IsDefault, @UpdateBy, @id | CustomerTypeDal.Save_DepartmentInfo | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_CHK_DCWiseArea | @AreaId, @DepotId, @DepotWiseAreaId, @EntryBy, @UpdateBy | DepotWiseAreaSetupDal.DepotWiseAreaInfo | 1 (PAGE-035) | M03 | NOT IN REPO | sp-convention |
| sp_Chk_SAP_EmpInfoCondition | @ActionStatus, @RoleType, @employee_code, @femployee_id | SAP_IntrigationPointDAL.CheckSAP_EmpInfoDAL; SAP_IntrigationPointDAL.CheckSAP_ProductDAL | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_CS_ASMInfo_Rpt | — | CommonDataLoad.GetAMInfo_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_CustomerType_All | — | CommonDataLoad.GetCustomerType_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_CS_Department_Active | — | CommonDataLoad.GetDepartment_Active | 5 (PAGE-557, PAGE-613, PAGE-614, PAGE-615…) | M15,M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_Designation_Active | — | CommonDataLoad.GetDesignation_Active_Emp | 6 (PAGE-557, PAGE-613, PAGE-614, PAGE-615…) | M15,M17,M14 | NOT IN REPO | sp-convention |
| sp_CS_FiscalYearInfo_Active | — | CommonDataLoad.GetFiscalYearInfo_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_GetArea_ByZoneId_Active | @id | CommonDataLoad.GetArea_ByZoneId_Active | 11 (PAGE-095, PAGE-463, PAGE-538, PAGE-543…) | M14,M15,M03 | NOT IN REPO | sp-convention |
| sp_CS_GetArea_ByZoneId_All | @id | CommonDataLoad.GetArea_ByZoneId_All | 4 (PAGE-463, PAGE-557, PAGE-587, PAGE-677) | M13,M14,M15 | NOT IN REPO | sp-convention |
| sp_CS_GetArea_ByZoneId_ForAMOnly | @id | CommonDataLoad.GetArea_ByZoneId_AllForAMOnly | 1 (PAGE-463) | M13 | NOT IN REPO | sp-convention |
| sp_CS_GetArea_ByZoneId_Rpt | @id | CommonDataLoad.GetArea_ByZoneId_Rpt | 1 (PAGE-549) | M14 | NOT IN REPO | sp-convention |
| sp_CS_GetDistributionRouteNameByMarketId | @id | CommonDataLoad.GetDistributionRouteNameByMarketId | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_GetEmpGeneralInfo_Active | — | CommonDataLoad.CompanyList_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_GetEmpGeneralInfo_All | — | CommonDataLoad.GetEmployeeList_Active; CommonDataLoad.GetEmployeeList_All | 41 (PAGE-548, PAGE-549, PAGE-555, PAGE-557…) | M15,M16,M18 | NOT IN REPO | sp-convention |
| sp_CS_GetEmpGeneralInfo_AllFSS | — | CommonDataLoad.GetEmployeeList_ActiveFS | 2 (PAGE-572, PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_CS_GetExpenseType_Active | — | CommonDataLoad.GetExpenseType | 4 (PAGE-557, PAGE-570, PAGE-677, PAGE-701) | M14,M15,M17 | NOT IN REPO | sp-convention |
| sp_CS_GetMarket_BySubTerritoryId_Active | @id | CommonDataLoad.GetMarket_BySubTerritoryId_Active; CommonDataLoad.GetMarket_ByTerritoryId_Active | 4 (PAGE-096, PAGE-557, PAGE-587, PAGE-677) | M03,M14,M15 | NOT IN REPO | sp-convention |
| sp_CS_GetMarket_BySubTerritoryId_All | @id | CommonDataLoad.GetMarket_BySubTerritoryId_All | 3 (PAGE-557, PAGE-587, PAGE-677) | M14,M15,M17 | NOT IN REPO | sp-convention |
| sp_CS_GetMarket_BySubTerritoryId_Rpt | @id | CommonDataLoad.GetMarket_BySubTerritoryId_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_GetMarket_ByTerritoryId_All | @id | CommonDataLoad.GetMarket_ByTerritoryId_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_CS_GetSubTerritory_ByTerritoryId_Active | @id | CommonDataLoad.GetSubTerritory_ByTerritoryId_Active | 4 (PAGE-100, PAGE-557, PAGE-587, PAGE-677) | M03,M14,M15 | NOT IN REPO | sp-convention |
| sp_CS_GetSubTerritory_ByTerritoryId_All | @id | CommonDataLoad.GetSubTerritory_ByTerritoryId_All; CommonDataLoad.GetSubTerritory_ByTerritoryId_Alle | 3 (PAGE-557, PAGE-587, PAGE-677) | M14,M15,M17 | NOT IN REPO | sp-convention |
| sp_CS_GetSubTerritory_ByTerritoryId_Rpt | @id | CommonDataLoad.GetSubTerritory_ByTerritoryId_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_GetTerritory_All | — | CommonDataLoad.GetTerritory_All | 1 (PAGE-411) | M11 | NOT IN REPO | sp-convention |
| sp_CS_GetTerritory_ByAreaId_Active | @id | CommonDataLoad.GetTerritory_ByAreaId_Active | 7 (PAGE-101, PAGE-463, PAGE-557, PAGE-587…) | M15,M03,M13 | NOT IN REPO | sp-convention |
| sp_CS_GetTerritory_ByAreaId_ActiveForDepo | @id | CommonDataLoad.GetTerritory_ByAreaId_ActiveDepo | 1 (PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_CS_GetTerritory_ByAreaId_All | @id | CommonDataLoad.GetTerritory_ByAreaId_All | 9 (PAGE-463, PAGE-506, PAGE-517, PAGE-533…) | M13,M14,M15 | NOT IN REPO | sp-convention |
| sp_CS_GetTerritory_ByAreaId_Rpt | @id | CommonDataLoad.GetTerritory_ByAreaId_Rpt | 1 (PAGE-549) | M14 | NOT IN REPO | sp-convention |
| sp_CS_GetTerritoryWiseDistributnCenterbyTeritoryId | @id | CommonDataLoad.GetTerritoryWiseDistributnCenter | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_GetZone_Active | — | CommonDataLoad.GetZone_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_GetZone_ByGroupId_Active | @id | CommonDataLoad.GetZone_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_Group_Active | — | CommonDataLoad.GetGroupInfo_Active | 4 (PAGE-463, PAGE-557, PAGE-669, PAGE-677) | M13,M14,M16 | NOT IN REPO | sp-convention |
| sp_CS_Group_All | — | CommonDataLoad.GetGroupInfo_All | 7 (PAGE-463, PAGE-506, PAGE-557, PAGE-587…) | M15,M13,M14 | NOT IN REPO | sp-convention |
| sp_CS_Group_Rpt | — | CommonDataLoad.GetGroupInfo_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_MIOInfo_BySC | @DCId | CommonDataLoad.GetMIOInfo_SC_Rpt | 1 (PAGE-075) | M03 | NOT IN REPO | sp-convention |
| sp_CS_MIOInfo_Rpt | — | CommonDataLoad.GetMIOInfo_Rpt | 2 (PAGE-122, PAGE-342) | M04,M10 | NOT IN REPO | sp-convention |
| sp_CS_National_Active | — | CommonDataLoad.GetNational_Active | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_CS_NSMInfo_Rpt | — | CommonDataLoad.GetNSMInfo_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_RoleType | — | CommonDataLoad.GetRoleTypeInfoDAL | 3 (PAGE-557, PAGE-616, PAGE-677) | M14,M15,M17 | NOT IN REPO | sp-convention |
| sp_CS_RoleTypeFSS | — | CommonDataLoad.GetRoleTypeInfoDALFFS | 2 (PAGE-572, PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_CS_RSMInfo_Rpt | — | CommonDataLoad.GetDZSMInfo_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_CS_Shift_Active | — | CommonDataLoad.GetShift_Active | 5 (PAGE-557, PAGE-613, PAGE-614, PAGE-615…) | M15,M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_Thana_All | — | CommonDataLoad.GetThanaInfo_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_CS_TourPlanType_Active | — | CommonDataLoad.GetTourTypeList_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_TourPlanType_All | — | CommonDataLoad.GetTourTypeList_ALL | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_Transport_Active | — | CommonDataLoad.GetTransportList_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_Transport_All | — | CommonDataLoad.GetTransportList_All | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CS_UserInfo_Active | — | CommonDataLoad.GetUserList_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_CustomerLedger | @Cust, @fromdate, @todate | CustomerLedgerDAL.CustomerLedgerDal | 3 (PAGE-301, PAGE-308, PAGE-504) | M09,M13 | NOT IN REPO | sp-convention |
| sp_CustomerLedgerNew | @Cust, @fromdate, @todate | CustomerLedgerDAL.CustomerLedgerDalNew | 1 (PAGE-309) | M09 | NOT IN REPO | sp-convention |
| sp_CustomerTransfer | @CustomerMasterExcelFileMasterID | ExcelUpForMIGODAL.TransfarCustomer_DAL | 1 (PAGE-193) | M05 | NOT IN REPO | sp-convention |
| sp_CustomerTransferTagChange | @CustomerTagChangeExcelFileMasterID | ExcelUpForCustTagChangeDAL.TransfarCustomer_DAL | 1 (PAGE-198) | M05 | NOT IN REPO | sp-convention |
| sp_Del_B2BDelete | @chalanId | B2BTransferViewDal.DeleteChallanDetailById | 1 (PAGE-407) | M11 | ['SP_Fix_STO_Chalan_Process.sql:24', 'SP_NewStockTransferDcToDc_Related.sql:22'] | sp-convention |
| sp_Del_BonusCampaignNewMaster | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusTypeId, @CampaignCategoryId, @CampaignDetailId, @CampaignMasterId, @CampaignName, @ | BonusCampaignNewDAL.SaveBonusCampaign | 5 (PAGE-111, PAGE-113, PAGE-114, PAGE-115…) | M04 | NOT IN REPO | sp-convention |
| sp_DEL_NoticeImage | @NoticeId | BonusCampaignNewDAL.Delete_NoticeImage; NoticeDal.Delete_NoticeImage | 0 | — | NOT IN REPO | sp-convention |
| sp_DEL_TargetInfo | @SL | BonusCampaignNewDAL.Delete_Target | 1 (PAGE-556) | M14 | NOT IN REPO | sp-convention |
| sp_Delete_ByRoleIDTypeId | @RoleId, @TypeId | MenuDAL.Delete_PrevoiusmenuAll | 1 (PAGE-027) | M02 | NOT IN REPO | sp-convention |
| sp_Delete_DepartmnetInfo | @DeptId | CustomerTypeDal.Delete_employeeleave; DepartmentDal.Delete_employeeleave; ProgramDal.Delete_employeeleave (+2) | 2 (PAGE-171, PAGE-567) | M05,M15 | NOT IN REPO | sp-convention |
| sp_Delete_DesignationInfo | @DesignationId | DesignationDal.Delete_DesignationInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_DoctorCategory | @DeleteBy | DoctorCategoryDal.DeleteDoctorcategory; Setup2DAL.DeleteDoctorcategory | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_DoctorChamber | @ChamberId, @DeleteBy | DoctorChamberDal.DeleteDoctorchamber | 1 (PAGE-638) | M16 | NOT IN REPO | sp-convention |
| sp_Delete_DoctorDegree | @DeleteBy | Setup2DAL.DeleteDoctorDegree | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_DoctorDesignation | @DeleteBy, @DesignationId | DoctorDesignationDal.DeleteDoctorDesignation; Setup2DAL.DeleteDoctorchamber | 1 (PAGE-644) | M16 | NOT IN REPO | sp-convention |
| sp_Delete_DoctorpatientType | @DeleteBy, @PatientTypeId | PatientTypeDal.DeleteDoctorPatientType; Setup2DAL.DeleteDoctorPatientType | 1 (PAGE-648) | M16 | NOT IN REPO | sp-convention |
| sp_Delete_DoctorSpecailDay | @DeleteBy, @SpecialDayId | DoctorSpecailDayDal.DeleteDoctorSpecialDay; Setup2DAL.DeleteDoctorSpecialDayType | 1 (PAGE-652) | M16 | NOT IN REPO | sp-convention |
| sp_Delete_DoctorSpeciality | @DeleteBy, @SpecialityId | DoctorSpecialityDal.DeleteDoctorSpeciality; Setup2DAL.DeleteDoctorSpeciality | 1 (PAGE-646) | M16 | NOT IN REPO | sp-convention |
| sp_Delete_Employee_LeaveInfo | @LeaveTypeId | HolidayDal.Delete_holiday; LeaveDal.Delete_employeeleave | 2 (PAGE-579, PAGE-580) | M15 | NOT IN REPO | sp-convention |
| sp_Delete_EmployeeInformation | @EmpInfoId | EmployeeInformationDaL.Delete_EmployeeInformation | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_ExpenseType | @ExpenseTypeId | Setup2DAL.Delete_ExpenseType; SetupDAL.Delete_ExpenseType | 1 (PAGE-573) | M15 | NOT IN REPO | sp-convention |
| sp_Delete_GroupInfo | @Id | GroupSetupDal.Delete_employeeleave | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_MonthlyAllowance | @MonthlyAllowanceId | Setup2DAL.DeleteMonthlyAllowance | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Delete_NoticeMaster | @NoticeId | BonusCampaignNewDAL.Delete_NoticeMaster; NoticeDal.Delete_NoticeMaster | 1 (PAGE-618) | M15 | NOT IN REPO | sp-convention |
| sp_Delete_Prescription_All | @PrescriptionTypeId | Setup2DAL.Delete__Prescription | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Delete_PrescriptionDetailsWhenUpdate | @PrescriptionTypeId | Setup2DAL.DeletePrescriptionDetailsWhenUpdate | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_PrescriptionType | @DeleteBy, @PrescriptionTypeId | PrescriptionTypeDal.Delete_PrescriptionType; Setup2DAL.DeletePrescription; Setup2DAL.Delete_PrescriptionType | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_PreviousmenuByRoleID | @id | MenuDAL.Delete_Prevoiusmenu | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_ProductDCDetails | @ComUnitId, @ProductId | CustomerInfoDAL.SaveProductDist; MarketStructureTransferDAL.SaveProductDist | 1 (PAGE-147) | M04 | NOT IN REPO | sp-convention |
| sp_Delete_ProformaInvoice | @InvoiceCode, @User | ProformaOrInvoiceReturn.DeleteProformaDal | 4 (PAGE-217, PAGE-218, PAGE-277, PAGE-288) | M06,M07,M08 | ['sp_Delete_ProformaInvoice_Update.sql:7'] | sp-convention |
| sp_Delete_ProformaInvoice_SubDeport | @InvoiceCode, @User | ProformaOrInvoiceReturn.DeleteProformaSub | 1 (PAGE-440) | M12 | NOT IN REPO | sp-convention |
| sp_Delete_TADAMarketRulesConfig | @TADAMarketRuleConfigId | Setup2DAL.DeleteTADAMarketRuleConfiguration | 0 | — | NOT IN REPO | sp-convention |
| sp_Delete_TourPurpose | @DeleteBy, @TourPurposeId | DoctorVisitDAL.DeleteTourPurpose; TourTypeDal.DeleteTourPurpose | 1 (PAGE-596) | M15 | NOT IN REPO | sp-convention |
| sp_Delete_TourType | @DeleteBy, @TourTypeId | DoctorVisitDAL.DeleteTourType; TourTypeDal.DeleteTourType | 1 (PAGE-599) | M15 | NOT IN REPO | sp-convention |
| sp_Delete_Trainning | @TrainningId | Setup2DAL.Delete_trainning | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Delete_Transport | @DeleteBy, @TransportId | Setup2DAL.DeleteTrasport | 0 | — | NOT IN REPO | sp-convention |
| sp_DeleteArchiveAttendanceData | — | FinancialYearDeleteTableDAL.DeleteArchiveAttendanceData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBAttendanceData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBAttendanceData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBDCRData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBDCRData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBExpenseData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBExpenseData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBInvoiceData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBInvoiceData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBOrderData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBOrderData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBRxData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBRxData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDBTourPlanData | — | FinancialYearDeleteTableDAL.DeleteArchiveDBTourPlanData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveDCRData | — | FinancialYearDeleteTableDAL.DeleteArchiveDCRData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveExpenseData | — | FinancialYearDeleteTableDAL.DeleteArchiveExpenseData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveInvoiceData | — | FinancialYearDeleteTableDAL.DeleteArchiveInvoiceData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveOrderData | — | FinancialYearDeleteTableDAL.DeleteArchiveOrderData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveRxData | — | FinancialYearDeleteTableDAL.DeleteArchiveRxData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteArchiveTourPlanData | — | FinancialYearDeleteTableDAL.DeleteArchiveTourPlanData | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_DeleteDeliveryInvoice | @DelivaryInvoiceNo, @User | ProformaOrInvoiceReturn.DeleteDeliveyInvoiceDal | 1 (PAGE-288) | M08 | NOT IN REPO | sp-convention |
| sp_DeleteDeliveryInvoiceSubdepo | @DelivaryInvoiceNo, @User | ProformaOrInvoiceReturn.SubdepoDeleteDeliveyInvoiceDal | 1 (PAGE-440) | M12 | NOT IN REPO | sp-convention |
| sp_DeleteDwpotToWhChalan | @masterId | SCtoWHTransferDal.DeleteDepoToWHTransfer | 1 (PAGE-409) | M11 | ['SP_NewStockTransferDcToDc_Related.sql:71'] | sp-convention |
| sp_Deletenvoice | @OrderID | InvoiceDAL.DeleteInvoice; ReturnInvoiceDAL.DeleteInvoice; SalesReturnDAL.DeleteInvoice (+1) | 1 (PAGE-218) | M06 | NOT IN REPO | sp-convention |
| sp_DeleteOrder | @LoginName, @OrdID | CustomerMasterDAL.DeleteRequisition | 2 (PAGE-222, PAGE-284) | M06,M08 | NOT IN REPO | sp-convention |
| sp_DeliveryConformationFull | @InvoiceNo, @UpdateBy, @UpdateDate | InvoiceDAL.SaveFullProformaInvoice; ReturnInvoiceDAL.SaveFullInvoice; ReturnInvoiceDAL.SaveFullProformaInvoice (+3) | 1 (PAGE-212) | M06 | NOT IN REPO | sp-convention |
| sp_DeliveryConformationFull_New | @InvoiceNo, @UpdateBy, @UpdateDate | InvoiceDAL.SaveFullInvoice; SalesReturnDAL.SaveFullInvoice | 1 (PAGE-271) | M07 | NOT IN REPO | sp-convention |
| sp_DeliveryConformationFull_OldData | @InvoiceNo, @UpdateBy, @UpdateDate | InvoiceDAL.SaveFullInvoiceOldData; SalesReturnDAL.SaveFullInvoiceOldData | 1 (PAGE-266) | M07 | NOT IN REPO | sp-convention |
| sp_DeliveryConformationReject | @InvoiceNo, @ReturnReason, @UpdateBy, @UpdateDate | InvoiceDAL.SaveRejectInvoice; ReturnInvoiceDAL.SaveRejectInvoice; SalesReturnDAL.SaveRejectInvoice (+1) | 0 | — | NOT IN REPO | sp-convention |
| sp_DeliveryInvoiceCreationList_New | @param | OrderInfoDAL.LoadOrderWithInvoiceSP; OrderInfoDALSalesReturn.LoadOrderWithInvoiceSP | 2 (PAGE-264, PAGE-271) | M07 | NOT IN REPO | sp-convention |
| sp_DeliverySalesReturnPosting | @ReturnId | CustomerReturnDAL.PostDeliverySalesReturn | 1 (PAGE-287) | M08 | ['Live_Deployment_Script_All_Updates.sql:558'] | sp-convention |
| sp_DICByregionid | @Area, @DCId, @EmployeeId, @RoleId, @Terr, @Type, @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewiseGyash_dzsm | 0 | — | NOT IN REPO | sp-convention |
| sp_EmpGeneralInfoByEmployeeId | @EmployeeId | CommonDataLoad.Get_EmpGeneralInfoByEmployeeId | 1 (PAGE-463) | M13 | NOT IN REPO | sp-convention |
| sp_ExpenseTypeDtl | @ExpenseTypeId | SetupDAL.delExpensDetls | 2 (PAGE-572, PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_GET_ActionStatusList | — | SeedDataDAL.GetApprovalStatusList | 33 (PAGE-167, PAGE-169, PAGE-173, PAGE-207…) | M16,M18,M15 | NOT IN REPO | sp-convention |
| sp_Get_AgingReceivableReport | @districtId, @fromDate, @fromdate, @toDate, @todate | InvoiceDAL.AgingIntransitReportDAl; SalesReturnDAL.AgingIntransitReportDAl; TotalSummaryDAL.LoadSummaryProductcodewiseGyashNew | 3 (PAGE-305, PAGE-398, PAGE-406) | M11,M09 | NOT IN REPO | sp-convention |
| sp_Get_alesReectionReportList | @Parm | CmnCrystaltoView.SalesRejecionReportDAl | 1 (PAGE-281) | M08 | NOT IN REPO | sp-convention |
| sp_Get_ALl_District_List | — | ThanaDal.Get_District_All_List | 1 (PAGE-091) | M03 | NOT IN REPO | sp-convention |
| sp_Get_ALl_Division_List | — | ThanaDal.GetDivision_All_List | 1 (PAGE-092) | M03 | NOT IN REPO | sp-convention |
| sp_Get_ALl_Thana_List | — | ThanaDal.Get_Thana_All_List | 1 (PAGE-093) | M03 | NOT IN REPO | sp-convention |
| sp_Get_AllCollectionReportListDHB | @Parm, @Parm2 | CmnCrystaltoView.GetDBH_DeliveryPaymentDAL | 1 (PAGE-327) | M09 | NOT IN REPO | sp-convention |
| sp_Get_ALlDIvision | — | SeedDataDAL.GetDivisions_All | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_Get_AllDoctorVisitMonitoringApprovalList | @AreaId, @GroupId, @TerrId, @ZonId, @frmDate, @param, @toDate | DoctorMonitoringDAL.GetDoctorVisitMonitoringApprovalList | 2 (PAGE-463, PAGE-636) | M13,M16 | NOT IN REPO | sp-convention |
| sp_Get_AllLeaveRecords | @Parameter | LeaveApplicationDal.GetAllLeaveRecords | 1 (PAGE-610) | M15 | NOT IN REPO | sp-convention |
| sp_Get_ALlOrderSummaryByChemist_Pivot | @ColumnToPivot, @ListToPivot, @Parm | OrderTrackingDAL.Get_ALlOrderSummaryByChemist | 1 (PAGE-210) | M06 | NOT IN REPO | sp-convention |
| sp_Get_ALlOrderSummaryByProduct | @Parm | OrderTrackingDAL.Get_ALlOrderSummaryByProduct | 1 (PAGE-210) | M06 | NOT IN REPO | sp-convention |
| sp_Get_Allowance_For_DDL | — | Setup2DAL.GetAllowance_For_ddl | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_AllPaymentReportList | @Parm | CmnCrystaltoView.GetAllPaymentReportListDAL | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_AllSalesConfirmationReport_new | @Parm, @Parm2 | CmnCrystaltoView.GetSalesConfirmationReport_newDAL | 1 (PAGE-527) | M13 | NOT IN REPO | sp-convention |
| sp_Get_AllSalesReportList | @Parm | CmnCrystaltoView.GetAllSalesListDAL | 1 (PAGE-491) | M13 | NOT IN REPO | sp-convention |
| sp_Get_AllSalesReportListParam2 | @NewParm, @Parm, @Parm2 | CmnCrystaltoView.GetDeliveryPaymentDAL; CmnCrystaltoView.GetFullPaymentDAL | 2 (PAGE-320, PAGE-505) | M09,M13 | NOT IN REPO | sp-convention |
| sp_Get_AllSalesReportListParamNew | @Parm, @Parm2 | CmnCrystaltoView.GetDeliveryPaymentDALNew | 1 (PAGE-321) | M09 | NOT IN REPO | sp-convention |
| sp_GET_AllStockProductwise | — | DistributionCenterStockMonitoringDal.GetDcStockMonitoringInfo | 1 (PAGE-351) | M10 | NOT IN REPO | sp-convention |
| sp_Get_AMDZSMListByTerritoryId | @TerritoryId | EmployeeInformationDaL.GetAMDZSMListByTerritoryId | 1 (PAGE-017) | M02 | NOT IN REPO | sp-convention |
| sp_GET_ApprovalMapLoad | @FromRoleId, @MenuId | ApprovalMapDAL.LoadApprovalMap | 1 (PAGE-026) | M02 | NOT IN REPO | sp-convention |
| sp_GET_ArchiveDbConnect_ByFinancialYearDesc | @FinancialYearDesc | UserInfoDAL.GetArchiveDatabaseList | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_GET_ArchiveFinancialYearList | — | UserInfoDAL.GetArchiveFinancialYearList | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_Get_AreaAll_ByEmpID | @EmpId, @id | CommonDal.GetArea_ByEmpID_All; CommonDal.GetTerritory_ByEmpID_All | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_AreaAll_ByZoneId | @id | CommonDal.GetArea_ByZoneId_All | 2 (PAGE-539, PAGE-540) | M14 | NOT IN REPO | sp-convention |
| sp_Get_AreaData_ByAreaId | @id | Setup2DAL.GetEditData_Area | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_AreaList | @RegionId | Setup2DAL.GetAreaList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_AreaList_OnlyActive | — | Setup2DAL.GetAreaList_OnlyActive | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_AreaList_OnlyActive_ByZoneId | @id | Setup2DAL.GetAreaList_OnlyActive_ByZoneId | 1 (PAGE-570) | M15 | NOT IN REPO | sp-convention |
| sp_Get_AreaListOrdPer | @RegionId | Setup2DAL.GetAreaListOrdPer | 1 (PAGE-017) | M02 | NOT IN REPO | sp-convention |
| sp_Get_AreaListOrdPerALL | — | Setup2DAL.GetAreaListOrdPerALL | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_Get_AreaTargetAmount | @RegionId, @Year, @month | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByArea | 3 (PAGE-543, PAGE-544, PAGE-545) | M14 | NOT IN REPO | sp-convention |
| sp_Get_AreaWiseTargetList | @Parameter | AreaWiseTargetSetupDal.Get_SaveDataAll | 1 (PAGE-540) | M14 | NOT IN REPO | sp-convention |
| sp_GET_ASMInfo | @Parameter | RSMSetupDal.Get_ASMInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_ASMInfo_ByEmpId | @id | RSMSetupDal.GAMMasterDataByEmpIDDAL; RSMSetupDal.GetAMSetupEditDataByEmpId | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_GET_ASMInfo_ById | @id | RSMSetupDal.GeAMSetupEditDataDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Get_AttandenceMonthlyDashboard_new | @Month, @Year, @param | ChartDAL.GetAttandenceMonthlyReportDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_AttendanceInformation | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetAttendence_Applog | 1 (PAGE-564) | M15 | NOT IN REPO | sp-convention |
| sp_GET_BAnkInfoById_ById | @id | SeedDataDAL.GetBAnkInfoById | 1 (PAGE-075) | M03 | NOT IN REPO | sp-convention |
| sp_GET_BAnkInfoById_ByIdAcc | @AccNo, @CompanyId | SeedDataDAL.GetBAnkInfoByIdAcc | 1 (PAGE-323) | M09 | NOT IN REPO | sp-convention |
| sp_GET_BAnkInfoById_ByIdExcel | @BankAccNo, @ComUnitId | SAP_IntrigationPointDAL.GetBankAccounTNo | 1 (PAGE-323) | M09 | NOT IN REPO | sp-convention |
| sp_Get_BankSAPMappingAccounTNo | @BankAccNo | SAP_IntrigationPointDAL.GetBankSAPMappingAccounTNo | 2 (PAGE-323, PAGE-324) | M09 | NOT IN REPO | sp-convention |
| sp_Get_BatchLifecycleReport | @BatchNo, @ComUnitId, @FromDate, @ProductCode, @ToDate, @TransactionType | SInventory_UI_BatchLifecycleReport.LoadData | 1 (PAGE-359) | M10 | NOT IN REPO | sp-convention |
| sp_Get_BonusCampaigndtlList | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignCategoryId, @Ca | BonusCampaignNewDAL.SaveBonusCampaign; BonusCampaignNewDAL.SaveBonusCampaign_final | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Get_BonusCampaignNewMasterList | @Parm | BonusCampaignNewDAL.GetBonusCampaignList | 1 (PAGE-116) | M04 | NOT IN REPO | sp-convention |
| sp_Get_BrandName_All | — | SeedDataDAL.GetBrandNameALL | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_Get_BrandWiseOrderDashboard | @FrmDate, @ToDate, @param | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_BrandWiseOrderDashboard_new | @FrmDate, @ToDate, @param | ChartDAL.GetBrandWiseOrderReportDAL; ChartDAL.GetBrandWiseOrderReportDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_BrandWiseOrderPaymentDashboard | @FrmDate, @ToDate, @param | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_GET_btnShow | @PageName | CommonDataLoad.GetPermissionForShowBtn | 6 (PAGE-228, PAGE-385, PAGE-389, PAGE-392…) | M10,M11,M06 | NOT IN REPO | sp-convention |
| sp_Get_BusinessSummaryReportList | @fromdate, @todate | CmnCrystaltoView.BusinessSummaryReportDAl | 2 (PAGE-469, PAGE-531) | M13 | NOT IN REPO | sp-convention |
| sp_GET_CampaignCategory | — | SeedDataDAL.GetCampaignCategory | 4 (PAGE-111, PAGE-113, PAGE-115, PAGE-117) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignDetail_ById | @id | BonusCampaignNewDAL.GetCampaignSetupDetailById | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignDetailCustomer_ById | @id | BonusCampaignNewDAL.GetCampaignSetupDetailCustomerById | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignDetailMaanualRatio_ById | @id | BonusCampaignNewDAL.GetCampaignSetupDetailMannualRatioById | 1 (PAGE-112) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignDetailMarket_ById | @id | BonusCampaignNewDAL.GetCampaignSetupDetailMarketById | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignDetailMulPro_ById | @id | BonusCampaignNewDAL.GetCampaignSetupDetailMulProById | 1 (PAGE-112) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignMaster_ById | @id | BonusCampaignNewDAL.GetCampaignSetupById | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignMasterMap_ById | @id | BonusCampaignNewDAL.GetCampaignSetupMapById | 2 (PAGE-111, PAGE-113) | M04 | NOT IN REPO | sp-convention |
| sp_GET_CampaignNameFromOrderDetail | — | SeedDataDAL.GetCampaignNameListALLFromOrder | 3 (PAGE-208, PAGE-209, PAGE-210) | M06 | NOT IN REPO | sp-convention |
| sp_GET_CampaignType | — | SeedDataDAL.GetCampaignTypeList | 8 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04,M17 | NOT IN REPO | sp-convention |
| sp_Get_CapturedBY_For_DDL | — | Setup2DAL.GetCapturedBy_For_ddl; SetupDAL.GetCapturedBy_For_ddl | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_ChamberType_Active | — | CommonDataLoad.GetChamberType_Active; DoctorInformationDal.GetChamberType_Active | 3 (PAGE-557, PAGE-669, PAGE-677) | M14,M16,M17 | NOT IN REPO | sp-convention |
| sp_GET_checkTodaysAlreadyInviceGenerateByCustId | @id | BonusCampaignNewDAL.checkTodaysAlreadyInviceGenerate | 1 (PAGE-217) | M06 | NOT IN REPO | sp-convention |
| sp_Get_Chk_EmpCode | @EmpCode | SAP_IntrigationPointDAL.GetEMPMap | 1 (PAGE-554) | M14 | NOT IN REPO | sp-convention |
| sp_Get_Chk_EMPDataExistCheck | @EmpCode, @MonthValue, @YearValue | SAP_IntrigationPointDAL.GetEMPDataExistCheck | 1 (PAGE-554) | M14 | NOT IN REPO | sp-convention |
| sp_Get_Chk_TerritoryCode | @TerritoryCode | SAP_IntrigationPointDAL.GetTerriMap | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_COGSBasedGPReport | @Parm | CmnCrystaltoView.GetCOGSBasedGPReportDAL | 1 (PAGE-495) | M13 | NOT IN REPO | sp-convention |
| sp_GET_CompanyInfo | — | CommonDataLoad.GetCompanyInfoAll; DepotWiseAreaSetupDal.GetCompanyList | 3 (PAGE-035, PAGE-557, PAGE-677) | M03,M14,M17 | NOT IN REPO | sp-convention |
| sp_Get_ContactType_Active | — | CommonDataLoad.GetContactType | 3 (PAGE-557, PAGE-669, PAGE-677) | M14,M16,M17 | NOT IN REPO | sp-convention |
| sp_GET_CustMaster_ById | @id | CustomerInfoDAL.GetCustomerSetupById; MarketStructureTransferDAL.GetCustomerSetupById; OrderTrackingDAL.GetCustomerSetupById | 1 (PAGE-168) | M05 | NOT IN REPO | sp-convention |
| sp_Get_CustMasterInstitution | — | CommonDataLoad.GetInstitute_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_Get_CustMasterList_Approve | @Parm | CustomerInfoDAL.GetCustomerList; MarketStructureTransferDAL.GetMarketListBySubTerritoryId | 4 (PAGE-167, PAGE-169, PAGE-173, PAGE-668) | M05,M16 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:820'] | sp-convention |
| sp_Get_Customer_Active | — | ExpenseDal.GetCustomerListActive; QuotedPriceSetupDAL.GetCustomerListActive | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_CustomerApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetCustomer_ApplogDAL | 1 (PAGE-695) | M18 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:221'] | sp-convention |
| sp_Get_CustomerCategory | — | Setup2DAL.GetCustomerCategory | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_CustomerCoverageRecordMonthlyDashboard_New | @CustomerTypeId, @Month, @Year, @param | ChartDAL.GetCustomerReportChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_CustomerCoverageRecordPaymentDashboard_New | @CustomerTypeId, @Month, @Year, @param | ChartDAL.GetCustomerReportChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_GET_CustomerInfo | @Parameter | CustomerTypeDal.GetDepartmentList | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_Get_CustomerTranferApprovalList | @Parm | CustomerInfoDAL.GetCustomerTranferList | 1 (PAGE-700) | M18 | NOT IN REPO | sp-convention |
| sp_GET_CustomerType | — | SeedDataDAL.GetChemistTypeList | 10 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04,M05,M17 | NOT IN REPO | sp-convention |
| sp_GET_CustomerType_ById | @id | CustomerTypeDal.GetEmployeeLeaveForEdit | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_GET_CustomerTypeAll | — | SeedDataDAL.GetChemistTypeListALL | 9 (PAGE-207, PAGE-208, PAGE-209, PAGE-210…) | M06,M09,M13 | NOT IN REPO | sp-convention |
| sp_Get_DACollectionReport | @Parm, @Parm2 | CmnCrystaltoView.GetDACollectionReportDAL | 1 (PAGE-319) | M09 | NOT IN REPO | sp-convention |
| sp_GET_DAInfo | @Parameter | DAInfoDal.GetDAInfo; TourSetupForEmployeeDAL.GetDAInfo | 1 (PAGE-236) | M07 | NOT IN REPO | sp-convention |
| sp_GET_DAInfoById | @Parameter | DAInfoDal.GetDAInfoById; TourSetupForEmployeeDAL.GetDAInfoById | 1 (PAGE-237) | M07 | NOT IN REPO | sp-convention |
| sp_Get_DAMonthlyDashboard_DayWise | @Month, @Year, @param | ChartDAL.GetExpanseClaimMonthlyChartDataDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardDeptoWiseInvoice | — | ChartDAL.Get_DeptoWiseInvoiceDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardDeptoWiseOrder | — | ChartDAL.GetDeptoWiseOrderDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardInvoice_DeptoWise | — | ChartDAL.Get_DeptoWiseInvoiceDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardOrder_DeptoWise | — | ChartDAL.GetDeptoWiseOrderDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartCustomerCoverage | — | ChartDAL.Get_TopBarChartCustomerCoverageDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartDeliveryAmount | — | ChartDAL.Get_TopBarChartDeliveryAmountDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartOrder | — | ChartDAL.Get_TopBarChartOrderDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartOrderCount | — | ChartDAL.Get_TopBarChartOrderCountDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartRejectionAmount | — | ChartDAL.Get_TopBarChartRejectionAmountDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartTotalAttandence | — | ChartDAL.Get_TopBarChartTotalAttandenceDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartTotalDCR | — | ChartDAL.Get_TopBarChartDCRDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartTotalInvoiceAmount | — | ChartDAL.Get_TopBarChartTotalInvoiceDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartTotalLeave | — | ChartDAL.Get_TopBarChartTotalLeaveDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarChartTotalRX | — | ChartDAL.Get_TopBarChartTotalRXDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarData | @EmployeeId, @GroupId, @RoleId, @ZoneId | ChartDAL.GetTotalInfoByCurrentDateDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarData_NSM | @EmployeeId, @GroupId, @RoleId, @ZoneId | ChartDAL.GetTotalInfoByCurrentDateDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DashboardTopBarData_Zone | @EmployeeId, @GroupId, @RoleId, @ZoneId | ChartDAL.GetTotalInfoByCurrentDateDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DatewiseSale | @FromDate, @ToDate | DayWiseBusinessReportDal.GetDayWiseBusinessInfo | 1 (PAGE-477) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DCPCVPTourPlanApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetDCPCVPTourPlan_ApplogDAL | 1 (PAGE-697) | M18 | NOT IN REPO | sp-convention |
| sp_Get_DCRApp | @AppStatus, @AreaId, @EmpId, @FromDt, @GroupId, @Role, @TerritoryId, @ToDt, @ZoneId, @param | CustomerInfoDAL.GetDCR_Applog | 1 (PAGE-698) | M18 | NOT IN REPO | sp-convention |
| sp_Get_DCRInfoList | @param | DoctorVisitDAL.GetDoctorDailyVisitList | 1 (PAGE-660) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DCRInfoListForView | @param | DoctorVisitDAL.GetDCRList | 2 (PAGE-505, PAGE-658) | M13,M16 | NOT IN REPO | sp-convention |
| sp_Get_DCStockReportListNew | @comunit | CmnCrystaltoView.GetDCReportListDAL | 1 (PAGE-364) | M10 | NOT IN REPO | sp-convention |
| sp_GET_DCWiseArea | @DepotId | DepotWiseAreaSetupDal.GetAreaByDepotId | 1 (PAGE-035) | M03 | NOT IN REPO | sp-convention |
| sp_Get_DDLDAName | — | RouteInformationDAL.GetDANameList | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_DDLDANameByDCID | @depoId | RouteInformationDAL.GetDANameDDL | 3 (PAGE-062, PAGE-218, PAGE-277) | M03,M06,M07 | NOT IN REPO | sp-convention |
| sp_Get_Degree_All | — | CommonDataLoad.GetDoctorDegree_All | 1 (PAGE-670) | M16 | NOT IN REPO | sp-convention |
| sp_Get_Degree_All_Active | — | CommonDataLoad.GetDegree_Active; DoctorInformationDal.GetDegree_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_Get_Degree_All_ActiveByDoctorTypeId | @id | CommonDataLoad.GetDegree_ActiveDoctorTypeId | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_GET_DelivaryInvoiceNoCheckById | @InvStatus, @id | BonusCampaignNewDAL.GetDelivaryInvoiceNoCheckById | 3 (PAGE-215, PAGE-298, PAGE-334) | M06,M08,M09 | NOT IN REPO | sp-convention |
| sp_Get_DeliveryReturnReport | @Parm | CmnCrystaltoView.GetDeliveryReturnReportListDAL | 1 (PAGE-286) | M08 | NOT IN REPO | sp-convention |
| sp_GET_DelTerritoryByRouteInformationDDL | @IvDate, @id | RouteInformationDAL.GetDelTerritoryByRouteInformationDA_DDLId | 1 (PAGE-271) | M07 | NOT IN REPO | sp-convention |
| sp_GET_DepartmentInfo | @Parameter | DepartmentDal.GetDepartmentList; GroupSetupDal.GetDepartmentList | 1 (PAGE-567) | M15 | NOT IN REPO | sp-convention |
| sp_GET_DepartmentInfo_ById | @id | DepartmentDal.GetEmployeeLeaveForEdit | 1 (PAGE-566) | M15 | NOT IN REPO | sp-convention |
| sp_Get_DepositCodeByDIC | @Parm | SAP_IntrigationPointDAL.GetDepositCodeByDIC | 3 (PAGE-314, PAGE-323, PAGE-324) | M09 | NOT IN REPO | sp-convention |
| sp_GET_DepotByCompanyId | @CompanyId | DepotWiseAreaSetupDal.GetDepotList | 2 (PAGE-035, PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_Get_Designation_All_Active | — | CommonDataLoad.GetDesignation_Active | 3 (PAGE-557, PAGE-669, PAGE-677) | M14,M16,M17 | NOT IN REPO | sp-convention |
| sp_GET_DesignationInfo | @Parameter | DesignationDal.GetDesignationList | 1 (PAGE-569) | M15 | NOT IN REPO | sp-convention |
| sp_GET_DesignationInfo_ById | @id | DesignationDal.GetDesignationForEdit | 1 (PAGE-568) | M15 | NOT IN REPO | sp-convention |
| sp_GET_DistributionCenter | — | SeedDataDAL.GetDistributionCenterDataTableList | 26 (PAGE-015, PAGE-016, PAGE-147, PAGE-167…) | M18,M05,M06 | NOT IN REPO | sp-convention |
| sp_GET_DistributionRoute | — | SeedDataDAL.GetDistributionRouteList | 3 (PAGE-168, PAGE-276, PAGE-462) | M05,M07,M12 | NOT IN REPO | sp-convention |
| sp_GET_DistributionRouteByDCID | @id | SeedDataDAL.GetDistributionRouteListByDCID | 2 (PAGE-207, PAGE-277) | M06,M07 | NOT IN REPO | sp-convention |
| sp_Get_District_Active_DDL | @id | ThanaDal.GetDistrict_All_DDL | 2 (PAGE-090, PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_Get_District_All_Active_NoTag | — | SeedDataDAL.GetDistrict_ActiveOnly | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_GET_DistrictInfo_ById | @id | ThanaDal.Get_District_By_Id | 1 (PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_GET_DistrictList | @id | SeedDataDAL.GetDistrictList | 2 (PAGE-168, PAGE-669) | M05,M16 | NOT IN REPO | sp-convention |
| sp_Get_DistrictList_OnlyActive_ByDivisionId | @id | CommonDataLoad.GetDistrict_ByDivision_Active; Setup2DAL.GetDistrict_ByDivision_Active; SetupDAL.GetDistrict_ByDivision_Active | 4 (PAGE-042, PAGE-167, PAGE-557, PAGE-677) | M03,M05,M14 | NOT IN REPO | sp-convention |
| sp_Get_Division_Active | — | CommonDataLoad.GetDivision_Active | 4 (PAGE-042, PAGE-167, PAGE-557, PAGE-677) | M03,M05,M14 | NOT IN REPO | sp-convention |
| sp_Get_Division_Active_DDL | — | ThanaDal.GetDivision_All_DDL | 2 (PAGE-090, PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_Get_Division_NoTagWithZone | — | SeedDataDAL.GetDivisionList_NotInAnyTagWithZone | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_GET_DivisionList | — | SeedDataDAL.GetDivisionList | 2 (PAGE-168, PAGE-669) | M05,M16 | NOT IN REPO | sp-convention |
| sp_Get_Doctor_For_DDL | — | Setup2DAL.GetDFoctorlist_For_ddl; SetupDAL.GetDFoctorlist_For_ddl | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_DoctorBrand_Active | — | CommonDataLoad.GetDoctorBrand_Active | 3 (PAGE-557, PAGE-669, PAGE-677) | M14,M16,M17 | NOT IN REPO | sp-convention |
| sp_Get_Doctorcategory_ById | @id | DoctorCategoryDal.GetDoctorCategoryForEdit; Setup2DAL.GetDoctorCategoryForEdit | 1 (PAGE-639) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorCategoryList | — | DoctorCategoryDal.GetDoctorCategoryList; Setup2DAL.GetDoctorCategoryList | 1 (PAGE-640) | M16 | NOT IN REPO | sp-convention |
| sp_Get_doctorCateList | — | CommonDataLoad.GetDocCat_Active | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorChamber_ById | @id | DoctorChamberDal.GetDoctorChamberForEditt; Setup2DAL.GetDoctorChamberForEdit | 1 (PAGE-637) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorChamberList | — | DoctorChamberDal.GetDoctorChamberList; Setup2DAL.GetDoctorChamberList | 1 (PAGE-638) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorClaimApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | DoctorDAL.GetDoctor_ApplogDAL | 1 (PAGE-699) | M18 | NOT IN REPO | sp-convention |
| sp_GET_DoctorContactDetail_ById | @id | DoctorDAL.GetContactById | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorCustomer_Active | — | CommonDataLoad.DoctorCustomer_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_Get_DoctorDegree_ById | @id | DoctorDegreeDal.GetDoctorDegreeEditdata; Setup2DAL.GetDoctorDegreeForEdit | 1 (PAGE-641) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorDegreeList | — | DoctorDegreeDal.GetDoctorDegreeList; Setup2DAL.GetDoctorDegreeList | 1 (PAGE-642) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorDesignation_ById | @id | DoctorDesignationDal.GetDoctorDesignationForEdit; Setup2DAL.GetDoctorDesignationForEdit | 1 (PAGE-643) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorDesignationList | — | DoctorDesignationDal.GetDoctorDesignationList; Setup2DAL.GetDoctorDesignationList | 1 (PAGE-644) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorExpenseClaim_ById | @id | Setup2DAL.GetExpenseClaimEditData | 1 (PAGE-570) | M15 | NOT IN REPO | sp-convention |
| sp_Get_DoctorGMPRecordMonthlyDashboard_New | @FrmDate, @ToDate, @param | ChartDAL.GetGMPVisitReportChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorGMPRecordMonthlyDashboardDayWise | @FrmDate, @ToDate, @param | ChartDAL.GetGMPVisitReportChartDataDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorGMPRecordMonthlyDashboardZoneWise | @FrmDate, @ToDate, @param | ChartDAL.GetGMPVisitReportChartDataDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorGMPxRecordMonthlyDashboard_new | @FrmDate, @ToDate, @param | ChartDAL.GetGMPRXReportChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorGMPxRecordMonthlyDashboardDayWise | @FrmDate, @ToDate, @param | ChartDAL.GetGMPRXReportChartDataDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorGMPxRecordMonthlyDashboardZoneWise | @FrmDate, @ToDate, @param | ChartDAL.GetGMPRXReportChartDataDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorList | @Parm | DoctorDAL.GetDoctorList; Setup2DAL.Get_DoctorList; SetupDAL.Get_DoctorList | 3 (PAGE-505, PAGE-668, PAGE-670) | M16,M13 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:883'] | sp-convention |
| sp_Get_DoctorList_Approval | — | Setup2DAL.Get_DoctorList_Approval; SetupDAL.Get_DoctorList_Approval | 0 | — | NOT IN REPO | sp-convention |
| sp_GET_DoctorMaster_ById | @id | DoctorDAL.GetDoctorSetupById | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorNGMPRecordMonthlyDashboard | @Month, @Year | ChartDAL.GetNONGMPVisitReportChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorNGMPxRecordMonthlyDashboard | @Month, @Year | ChartDAL.GetNONGMPRXReportChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_DoctorPatientList | — | PatientTypeDal.GetDoctorPatientTypeList; Setup2DAL.GetDoctorPatientTypeList | 1 (PAGE-648) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorPatientType_ById | @id | PatientTypeDal.GetDoctorPatientTypeForEdit; Setup2DAL.GetDoctorPatientTypeForEdit | 1 (PAGE-647) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorPlanDetailsById | @id | DoctorVisitDAL.GetDoctorPlanDetailsViewDatabyID | 1 (PAGE-661) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorProgramType_Active | — | CommonDataLoad.DoctorProgramType_Active; DoctorInformationDal.DoctorProgramType_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_GET_DoctorProgramTypeList | — | SeedDataDAL.GetDoctorProgramTypeList | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_DoctorSetupData_ByDoctorId | @id | Setup2DAL.GetEditData_DoctorSetup | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_DoctorSpeacialDay_ById | @id | DoctorSpecailDayDal.GetDoctorSpecialDayForEdit; Setup2DAL.GetDoctorSpecialDayForEdit | 1 (PAGE-651) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorSpecailDayList | — | DoctorSpecailDayDal.GetDoctorSpecialDayList; Setup2DAL.GetDoctorSpecialDayList | 1 (PAGE-652) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorSpecialDay_All_Active | — | CommonDataLoad.GetSpecialDay_Active | 3 (PAGE-557, PAGE-669, PAGE-677) | M14,M16,M17 | NOT IN REPO | sp-convention |
| sp_GET_DoctorSpecialDayDetail_ById | @id | DoctorDAL.GetSpecialDayById | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorSpeciality_Active | — | CommonDataLoad.DoctorSpeciality_Active | 3 (PAGE-557, PAGE-669, PAGE-677) | M14,M16,M17 | NOT IN REPO | sp-convention |
| sp_Get_DoctorSpeciality_ById | @id | DoctorSpecialityDal.GetDoctorSpecialityForEdit; Setup2DAL.GetDoctorSpecialityForEdit | 1 (PAGE-645) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorSpecialityList | — | DoctorSpecialityDal.GetDoctorSpecialityList; Setup2DAL.GetDoctorSpecialityList | 1 (PAGE-646) | M16 | NOT IN REPO | sp-convention |
| sp_Get_DoctorTourPlanMasteList | @param | DoctorVisitDAL.GetDoctorVisitList | 3 (PAGE-607, PAGE-662, PAGE-664) | M16,M15 | NOT IN REPO | sp-convention |
| sp_Get_DoctorTranferApprovalList | @Parm | DoctorDAL.GetDoctorTransferList | 1 (PAGE-700) | M18 | NOT IN REPO | sp-convention |
| sp_Get_DoctorType_Active | — | CommonDataLoad.GetDoctorType_Active | 7 (PAGE-557, PAGE-669, PAGE-670, PAGE-671…) | M16,M14,M17 | NOT IN REPO | sp-convention |
| sp_GET_DZSMInfo_ByEmpId | @id | RSMSetupDal.GeDZSMSetupEditDataByEMPIDDAL; RSMSetupDal.GetDZSMSetupEditDataByEmpId | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_GET_DZSMInfo_ById | @id | RSMSetupDal.GeDZNSMSetupEditDataDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Get_DZSMinfoByEmpId | @EmpId | CommonDal.Get_DZSMinfoByEmpId | 4 (PAGE-538, PAGE-539, PAGE-540, PAGE-545) | M14 | NOT IN REPO | sp-convention |
| sp_GET_DZSMProcessDate | @id | DoctorInfoReportDal.GetTerritoryWiseLastProessDate | 1 (PAGE-530) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseNAtionalReport | @fromdate, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | 2 (PAGE-506, PAGE-530) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseReport | @Dzsm, @fromdate, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryBLL | 2 (PAGE-506, PAGE-530) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseReportParam_ByProcess | @fromdate, @parm, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryparm | 1 (PAGE-506) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseReportParam_ByProcess_Area | @AreaSelect, @TeritorySelect, @ZoneSelect, @fromdate, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | 1 (PAGE-530) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseReportParam_ByProcess_Terri | @AreaSelect, @TeritorySelect, @ZoneSelect, @fromdate, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | 1 (PAGE-530) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseReportParam_ByProcess_vv | @AreaSelect, @TeritorySelect, @ZoneSelect, @fromdate, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryparm_vvvv | 1 (PAGE-530) | M13 | NOT IN REPO | sp-convention |
| sp_GET_DZSMwiseReportParam_new_Day | @fromdate, @parm, @todate | TotalSummaryDAL.DZSMwiseLoadSummaryparm_new | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_Emp_AttendanceInfoDayRow | @param | AttendanceDAL.Get_Emp_AttendanceInfoListRowDay | 1 (PAGE-563) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Emp_AttendanceInfoList | @param | AttendanceDAL.Get_Emp_AttendanceInfoList | 2 (PAGE-562, PAGE-563) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Emp_AttendanceInfoListPending | — | AttendanceDAL.Get_AttendanceList_Approval | 1 (PAGE-564) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Employee_ShiftInfos_ById | @id | AttendanceDAL.GetShiftInfoEditData | 1 (PAGE-585) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Employee_ShiftInfosList | — | AttendanceDAL.GetShiftList | 1 (PAGE-586) | M15 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeAMditID | @Id | CommonDataLoad.GetAMEmployee_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeDZSMditID | @Id | CommonDataLoad.GetDZSMEmployee_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeFieldForceInfo_EmpId | @id | CommonDataLoad.GetEmpMarketStructure_Active | 18 (PAGE-208, PAGE-209, PAGE-210, PAGE-327…) | M18,M15,M06 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeFieldForceInfo_EmpIdMArketInfo | @RoleTypeName, @id | CommonDataLoad.GetEmpMarketStructure_Active_forReport | 1 (PAGE-549) | M14 | NOT IN REPO | sp-convention |
| sp_GET_EmployeeInfoOrdPermission | @Parm | EmployeeInformationDaL.GetEmployeeInfoOrdPermission | 1 (PAGE-017) | M02 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeInformation_ById | @id | EmployeeInformationDaL.GetEmployeeInfoForEdit | 3 (PAGE-613, PAGE-614, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeInformationList_Prm | @Parm | EmployeeInformationDaL.GetEmployeeInformationList | 1 (PAGE-612) | M15 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeInformationListActive | @Parm | EmployeeInformationDaL.GetEmployeeInformationListForActive | 1 (PAGE-583) | M15 | NOT IN REPO | sp-convention |
| sp_GET_EmployeeLeaveBalance | @Parameter | LeaveApplicationDal.GetEmployeeLeaveBalanceByCode | 1 (PAGE-608) | M15 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeLeaveIinfo_ById | @id | LeaveDal.GetEmployeeLeaveForEdit | 2 (PAGE-577, PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeList | @EmpInfoId | SeedDataDAL.GetEmployee_All | 4 (PAGE-015, PAGE-016, PAGE-676, PAGE-678) | M02,M17 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeListFieldForce | — | SeedDataDAL.GetEmployee_AllFieldForceEmployeeList | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeMIO | — | CommonDataLoad.GetMIOEmployee_AllWithOutParm | 1 (PAGE-411) | M11 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeMIOEditID | @Id | CommonDataLoad.GetMIOEmployee_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_Get_EmployeeNSMditID | @Id | CommonDataLoad.GetNSMEmployee_All | 1 (PAGE-677) | M17 | NOT IN REPO | sp-convention |
| sp_GET_EmployeeWiseProductSale | — | DistributionCenterStockMonitoringDal.EmployeewiseProductSales | 1 (PAGE-479) | M13 | NOT IN REPO | sp-convention |
| sp_Get_EmployyeMonthlyExpense | @EmpId, @ToDate, @frmDate | CmnCrystaltoView.GetMonthlyExpenseList | 1 (PAGE-623) | M15 | NOT IN REPO | sp-convention |
| sp_Get_ExpanseClaimApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetExpenseClaim_Applog | 1 (PAGE-701) | M18 | NOT IN REPO | sp-convention |
| sp_Get_ExpanseClaimMonthlyDashboard | @Month, @Year, @param | ChartDAL.GetExpanseClaimMonthlyChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_ExpanseClaimMonthlyDashboard_DayWise | @Month, @Year, @param | ChartDAL.GetExpanseClaimMonthlyChartDataDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_ExpenseClaimList | @param | Setup2DAL.GetExpenseClaimList; SetupDAL.GetExpenseClaimList | 1 (PAGE-571) | M15 | NOT IN REPO | sp-convention |
| sp_Get_ExpenseDetails_ByExpenseId | @id | CommonDataLoad.GetExpenseField_ByExpenseType; Setup2DAL.Get_ExpenseTypeDetailsByExpenseId; SetupDAL.Get_ExpenseTypeDetailsByExpenseId | 4 (PAGE-557, PAGE-570, PAGE-572, PAGE-677) | M15,M14,M17 | NOT IN REPO | sp-convention |
| sp_Get_ExpenseTypeData_ByExpenseTypeId | @id | Setup2DAL.GetEditDataForExpenseType; SetupDAL.GetEditDataForExpenseType; SetupDAL.GetEditDataForId | 1 (PAGE-572) | M15 | NOT IN REPO | sp-convention |
| sp_Get_ExpenseTypeMaster | — | Setup2DAL.Get_ExpenseTypeMasterList; SetupDAL.Get_ExpenseTypeMasterList | 1 (PAGE-573) | M15 | NOT IN REPO | sp-convention |
| sp_GET_FinancialInfo | @Parameter | FinancialYearDal.GetDepartmentList; FinancialYearDal.GetFinancialYearList | 1 (PAGE-040) | M03 | NOT IN REPO | sp-convention |
| sp_GET_financialYear_ById | @id | FinancialYearDal.GetFinancialYearForEdit | 1 (PAGE-039) | M03 | NOT IN REPO | sp-convention |
| sp_Get_FinanCialyearforDDL | — | HolidayDal.GetFinancialYear | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_FirstDate_LastDatebyYearMonth | @EndDate, @StartDate | ZoneWiseTargetSetupDal.Get_FirstDate_LastDatebyYearMonth | 1 (PAGE-538) | M14 | NOT IN REPO | sp-convention |
| sp_GET_forPaymentTerritoryByRouteInformationDDL | @id | RouteInformationDAL.GetforPaymentTerritoryByRouteInformationDA_DDLId | 3 (PAGE-311, PAGE-315, PAGE-318) | M09 | NOT IN REPO | sp-convention |
| sp_GET_forsndReturnTerritoryByRouteInformationDDL | @id | RouteInformationDAL.GetforsndreturnTerritoryByRouteInformationDA_DDLId | 1 (PAGE-296) | M08 | NOT IN REPO | sp-convention |
| sp_GET_GenericGroup | @Parameter | GenericGroupDal.GetGenericGroupList | 1 (PAGE-104) | M04 | NOT IN REPO | sp-convention |
| sp_GET_GenericGroup_ById | @id | GenericGroupDal.GetGenericGroupForEdit | 1 (PAGE-103) | M04 | NOT IN REPO | sp-convention |
| sp_GET_GetOrderDtlCamCheckId | @id | BonusCampaignNewDAL.GetOrderDtlCamCheckId | 0 | — | NOT IN REPO | sp-convention |
| sp_GET_GetOrderDtlCamCheckIdEze | @id | BonusCampaignNewDAL.GetOrderDtlCamCheckIdCheckEzevent | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_Group_List | @Parameter | NationalTargetSetupDal.Get_GroupTarget_All | 1 (PAGE-542) | M14 | NOT IN REPO | sp-convention |
| sp_GET_GroupInfo | — | SeedDataDAL.GetGroup_All | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_GET_GroupInfo_ById | @id | GroupSetupDal.GetEmployeeLeaveForEdit | 2 (PAGE-046, PAGE-105) | M03,M04 | NOT IN REPO | sp-convention |
| sp_GET_GroupList | @Parameter | GroupSetupDal.GetGroupSetupList | 1 (PAGE-106) | M04 | NOT IN REPO | sp-convention |
| sp_Get_GroupListOrdPer | — | Setup2DAL.GetGroupListOrdPer | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_Get_GroupWisePromoQtyList | @Parm | GroupWisePromoQtyDAL.GetList | 1 (PAGE-122) | M04 | NOT IN REPO | sp-convention |
| sp_Get_HolidayInfo_ById | @id | HolidayDal.GetholidayForEdit | 1 (PAGE-575) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Holidaylist | — | HolidayDal.GetHolydayList | 1 (PAGE-576) | M15 | NOT IN REPO | sp-convention |
| sp_Get_IntransitReportList | @districtId, @fromDate, @toDate | InvoiceDAL.IntransitReportDAl; SalesReturnDAL.IntransitReportDAl | 3 (PAGE-398, PAGE-401, PAGE-410) | M11 | NOT IN REPO | sp-convention |
| sp_GET_InvoiceStatusddlAll | @IsShowforPartial, @IsShowforRejection, @IsforReturn, @isForSalesConfirm | SeedDataDAL.GettblInvoiceStatusddlALL | 1 (PAGE-271) | M07 | NOT IN REPO | sp-convention |
| sp_Get_InvoiceSummaryReport | @ComUnitId, @CustomerCode, @DeliveryNo, @FromDate, @ProformaNo, @ToDate | CmnCrystaltoView.GetInvoiceSummaryReportDAL | 1 (PAGE-269) | M07 | NOT IN REPO | sp-convention |
| sp_GET_InvoiceWiseDetailsSalesReport | @DCID, @FromDate, @ToDate | AreaDAL.LoadSummaryProductcodewiseGyash | 1 (PAGE-513) | M13 | NOT IN REPO | sp-convention |
| sp_Get_JoiningDateCountInfo | — | CommonDataLoad.Get_JoiningDateCountInfo | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_GET_lDcWiseTerritoryDetail_ByAreaId | @DCId, @SubDepotId | TerritoryWiseDepotSetupDAL.GetSetupDetailById | 1 (PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_Get_Leave_AppLog | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetLeave_AppLogDAL | 1 (PAGE-702) | M18 | NOT IN REPO | sp-convention |
| sp_GET_LeaveApplicationInfoById | @Parameter | LeaveApplicationDal.GetApplicationInfoById | 1 (PAGE-608) | M15 | NOT IN REPO | sp-convention |
| sp_Get_LeaveConfigList | — | LeaveDal.GetLeaveConfigList | 1 (PAGE-579) | M15 | NOT IN REPO | sp-convention |
| sp_GET_LeaveConfigSetupById | @id | BonusCampaignNewDAL.GetLeaveConfigSetupById | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Leavelist | — | LeaveDal.GetLeaveList | 1 (PAGE-580) | M15 | NOT IN REPO | sp-convention |
| sp_Get_LeaveType_New | — | SeedDataDAL.GetLeaveTypeList | 2 (PAGE-610, PAGE-702) | M15,M18 | NOT IN REPO | sp-convention |
| sp_Get_LoadingReportList | @Parm, @dtRange | CmnCrystaltoView.GetLoadingReportDAL | 1 (PAGE-247) | M07 | NOT IN REPO | sp-convention |
| sp_GET_MainMenuByType | @TypeId | ApprovalMapDAL.GetMainMenuDropDown | 1 (PAGE-026) | M02 | NOT IN REPO | sp-convention |
| sp_GET_MainMenuRole2 | @RoleId, @TypeId | MenuDAL.LoadMenu | 1 (PAGE-027) | M02 | NOT IN REPO | sp-convention |
| sp_GET_MainMenuRoleIsApp | @RoleId, @TypeId | MenuDAL.LoadMenuIsApp | 1 (PAGE-027) | M02 | NOT IN REPO | sp-convention |
| sp_GET_MainPermissionByUserRoleandPageUrl | @PageName, @RoleId | CommonDataLoad.GetPermissionForUserRole | 17 (PAGE-012, PAGE-173, PAGE-464, PAGE-564…) | M18,M15,M02 | NOT IN REPO | sp-convention |
| sp_Get_MarketByTerriTory | — | RouterSetup.GetMarketByTerriTory | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_Get_MarketByTerriTory_ByRouterMasterId | @id | RouterSetup.GetMarketByTerriToryById | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_Get_MarketData_ByMarketid | @id | Setup2DAL.GetEditData_Market; Setup2DAL.GetEditData_Market_New; Setup2DAL.GetMarketEditDataDAL | 2 (PAGE-042, PAGE-679) | M03,M17 | NOT IN REPO | sp-convention |
| sp_Get_MarketList | @Parameter | Setup2DAL.GetMarketList | 1 (PAGE-041) | M03 | NOT IN REPO | sp-convention |
| sp_Get_MileageAppData | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetMileageClaim_Applog | 1 (PAGE-703) | M18 | NOT IN REPO | sp-convention |
| sp_Get_MileageClaim_ById | @id | Setup2DAL.GetMileageClaimEditData | 1 (PAGE-581) | M15 | NOT IN REPO | sp-convention |
| sp_Get_MileageClaimList | @param | Setup2DAL.GetMileageClaimList | 12 (PAGE-017, PAGE-548, PAGE-582, PAGE-623…) | M15,M16,M02 | NOT IN REPO | sp-convention |
| sp_GET_MIOInfo | @Parameter | RSMSetupDal.Get_MIOInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_MIOInfo_ByEmpId | @id | RSMSetupDal.GeMIOMasterDataByEmpIDDAL; RSMSetupDal.GetMIOetupEditDataByEmpId | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_GET_MIOInfo_ById | @id | RSMSetupDal.GeMIOetupEditDataDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Get_MoneyReceiptReportAfterPaymentList | @Parm | CmnCrystaltoView.GetMoneyReceiptAfterPaymentDAL; InvoiceDAL.MoneyReceiptAfterPaymentInfo; SalesReturnDAL.MoneyReceiptAfterPaymentInfo | 2 (PAGE-304, PAGE-330) | M09 | ['SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql:17'] | sp-convention |
| sp_Get_MoneyReceiptReportList | @Parm | CmnCrystaltoView.GetMoneyReceiptDAL; InvoiceDAL.MoneyReceiptInfo; SalesReturnDAL.MoneyReceiptInfo | 2 (PAGE-303, PAGE-329) | M09 | NOT IN REPO | sp-convention |
| sp_Get_MonthlyAllowance | — | Setup2DAL.GetMonthlyAllowanceList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_MonthlyAllowance_ById | @id | Setup2DAL.GetMonthlyAllowanceForEdit | 1 (PAGE-583) | M15 | NOT IN REPO | sp-convention |
| sp_GET_MonthlyAllowanceList | — | SeedDataDAL.GetlMonthlyAllawanceList | 3 (PAGE-613, PAGE-614, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Get_MonthlyExpenseEmpWiseMaster_Final | @EmpId, @Month, @Year | ExpenseDal.GetExpenseMasterById | 1 (PAGE-628) | M15 | NOT IN REPO | sp-convention |
| sp_Get_MonthlyExpenseEmpWiseTotal | @EmpId, @Month, @Year | ExpenseDal.GetExpenseMasterTotalById | 1 (PAGE-628) | M15 | NOT IN REPO | sp-convention |
| sp_Get_MonthlyInventoryReport | @CiD, @ProTypId, @fromDate, @toDate | InvoiceDAL.GetMonthlyInventoryReportDAl; SalesReturnDAL.GetMonthlyInventoryReportDAl | 1 (PAGE-372) | M10 | NOT IN REPO | sp-convention |
| sp_GET_MonthwiseSale | @FromDate, @ToDate | SalesTrendReportDal.GetSalesTrendInfo | 1 (PAGE-489) | M13 | NOT IN REPO | sp-convention |
| sp_Get_MSList_Approve | @Parm | MarketStructureTransferDAL.GetList | 1 (PAGE-099) | M03 | NOT IN REPO | sp-convention |
| sp_GET_NationalList | @Parameter | GroupSetupDal.GetNationalSetupList | 1 (PAGE-047) | M03 | NOT IN REPO | sp-convention |
| sp_Get_NationalTargetAmount | @GroupId, @Year, @month | ZoneWiseTargetSetupDal.Get_NationalTargetAmount_All_ByGroup | 1 (PAGE-546) | M14 | NOT IN REPO | sp-convention |
| sp_Get_NewReceiveableList | @districtId, @fromDate, @toDate | CmnCrystaltoView.Get_MIOWiseReceiveableReport; InvoiceDAL.GetNewReceiveableDAl; SalesReturnDAL.GetNewReceiveableDAl | 5 (PAGE-217, PAGE-244, PAGE-333, PAGE-402…) | M06,M07,M09 | NOT IN REPO | sp-convention |
| sp_Get_NewReceiveableListforInvoice | @districtId, @fromDate, @toDate | InvoiceDAL.GetNewReceiveableforInvoiceDAl; SalesReturnDAL.GetNewReceiveableforInvoiceDAl | 1 (PAGE-250) | M07 | NOT IN REPO | sp-convention |
| sp_Get_NewReceiveableListWeb | @districtId, @fromDate, @toDate | InvoiceDAL.GetNewReceiveableDAlWeb | 1 (PAGE-332) | M09 | NOT IN REPO | sp-convention |
| sp_Get_Notice_ById | @id | Setup2DAL.GetNoticeSetupById | 2 (PAGE-619, PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_GET_NoticeDetailMarket_ById | @id | Setup2DAL.GetNoticeDetailMarketById | 1 (PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Noticedetails_By_NoticeId | @id | BonusCampaignNewDAL.Get_NoticeDetailsByNoticeId; NoticeDal.Get_NoticeDetailsByNoticeId | 1 (PAGE-618) | M15 | NOT IN REPO | sp-convention |
| sp_Get_NoticeImage_By_NoticeId | @id | BonusCampaignNewDAL.Get_NoticeImageByNoticeId; NoticeDal.Get_NoticeImageByNoticeId | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_NoticeMaster | — | BonusCampaignNewDAL.GetNoticeMasterList; NoticeDal.GetNoticeMasterList | 1 (PAGE-618) | M15 | NOT IN REPO | sp-convention |
| sp_Get_NoticeMaster_ById | @id | BonusCampaignNewDAL.GetNoticeMasterForEdit; NoticeDal.GetNoticeMasterForEdit | 1 (PAGE-618) | M15 | NOT IN REPO | sp-convention |
| sp_GET_NoticeSeen_ById | @id | Setup2DAL.GetNoticeSeenbyById | 1 (PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_GET_NSMHeadInfo | @Parameter | RSMSetupDal.GetNSMHeadList | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_NSMInfo | @Parameter | RSMSetupDal.Get_NSMInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_NSMInfo_ByEMPId | @id | RSMSetupDal.GetNSMSetupEditDataByEmpId; RSMSetupDal.GetNSMSetupEditDataByEmpIdDAL | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_GET_NSMInfo_ById | @id | RSMSetupDal.GetNSMSetupEditDataDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Get_OfferTypeInfo | @id | SeedDataDAL.GetOfferTypeInfo | 7 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04,M17 | NOT IN REPO | sp-convention |
| sp_Get_OrderDelTrackingList | @Parm | OrderTrackingDAL.GetOrderDelTrackingList | 1 (PAGE-492) | M13 | NOT IN REPO | sp-convention |
| sp_Get_OrderDetailsTrackingList | @Parm | OrderTrackingDAL.GetOrderDetailsTrackingList | 1 (PAGE-208) | M06 | NOT IN REPO | sp-convention |
| sp_Get_OrderNoforReturn | @id | SeedDataDAL.GetOrderNoforReturn | 1 (PAGE-284) | M08 | NOT IN REPO | sp-convention |
| sp_Get_OrderNoforReturnDistributionRouteId | @ComId, @rootId | SeedDataDAL.GetOrderNoforReturn | 1 (PAGE-284) | M08 | NOT IN REPO | sp-convention |
| sp_Get_OrderRecordMonthly | @Month, @Year | ChartDAL.GetOrderPerMonthAmount | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_OrderRecordMonthlyDashboard_Depo | @Month, @Year, @param | ChartDAL.GetOrderChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_OrderRecordMonthlyDashboard_New | @Month, @Year, @param | ChartDAL.GetOrderChartDataDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_OrderTrackingList_DBH | @Parm | OrderTrackingDAL.GetOrderTrackingDBH | 1 (PAGE-209) | M06 | NOT IN REPO | sp-convention |
| sp_Get_OrderTrackingList_Latest | @Parm | OrderTrackingDAL.GetOrderTrackingList | 2 (PAGE-207, PAGE-208) | M06 | NOT IN REPO | sp-convention |
| sp_Get_PaymentBrandWiseOrderDashboard | @FrmDate, @ToDate, @param | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_GET_PaymentInvSP | @param | OrderInfoDAL.LoadPaymentInvSP; OrderInfoDALSalesReturn.LoadPaymentInvSP; SalesReturnNewDAL.LoadPaymentInvSP | 3 (PAGE-311, PAGE-312, PAGE-315) | M09 | ['Live_Deployment_Script_All_Updates.sql:212'] | sp-convention |
| sp_GET_PaymentInvSPPaymentAmount | @CollectionBy, @PaymentAmount, @param | OrderInfoDAL.LoadPaymentInvSPWithPaymentAmount; OrderInfoDALSalesReturn.LoadPaymentInvSPWithPaymentAmount; SalesReturnNewDAL.LoadPaymentInvSPWithPaymentAmount | 1 (PAGE-314) | M09 | ['Live_Deployment_Script_All_Updates.sql:340'] | sp-convention |
| sp_GET_PaymentInvSPSndReturn | @param | OrderInfoDAL.SndTimeReturnLoadPaymentInvSP | 1 (PAGE-318) | M09 | NOT IN REPO | sp-convention |
| sp_GET_PaymentInvSPTPVATAmt | @InvoiceId, @PayAmount | OrderInfoDAL.LoadPaymentInvSPTPVATAmt; OrderInfoDALSalesReturn.LoadPaymentInvSPTPVATAmt; SalesReturnNewDAL.LoadPaymentInvSPTPVATAmt | 4 (PAGE-311, PAGE-314, PAGE-315, PAGE-318) | M09 | ['Live_Deployment_Script_All_Updates.sql:296'] | sp-convention |
| sp_GET_PendingSalesConfirmationReport | @districtId | InvoiceDAL.GetPendingSalesConfirmationReportDAl; SalesReturnDAL.GetPendingSalesConfirmationReportDAl | 1 (PAGE-521) | M13 | NOT IN REPO | sp-convention |
| sp_Get_Prescription_ByPrescriptionId | @id | Setup2DAL.GetEditData_Prescription; SetupDAL.GetEditData_Prescription | 1 (PAGE-653) | M16 | NOT IN REPO | sp-convention |
| sp_Get_PrescriptionApp | @AppStatus, @AreaId, @EmpId, @FromDt, @GroupId, @Role, @TerritoryId, @ToDt, @ZoneId, @param | CustomerInfoDAL.GetRX_Applog | 1 (PAGE-705) | M18 | NOT IN REPO | sp-convention |
| sp_Get_PrescriptionDetails_ByPrescriptionId | @id | SetupDAL.Get_PrescriptionDetailsByPrescriptionId | 1 (PAGE-653) | M16 | NOT IN REPO | sp-convention |
| sp_Get_PrescriptionList | @param | Setup2DAL.Get_PrescriptionList; SetupDAL.Get_PrescriptionList | 3 (PAGE-505, PAGE-657, PAGE-679) | M13,M16,M17 | NOT IN REPO | sp-convention |
| sp_Get_PrescriptionType_ById | @id | PrescriptionTypeDal.GetPrescriptionForEdit; Setup2DAL.GetPrescriptionForEdit; SetupDAL.GetPrescriptionForEdit | 2 (PAGE-649, PAGE-655) | M16 | NOT IN REPO | sp-convention |
| sp_Get_PrescriptionType_For_DDL | — | Setup2DAL.GetPrescriptionTypelist_For_ddl; SetupDAL.GetPrescriptionTypelist_For_ddl | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_PrescriptionTypeList | — | PrescriptionTypeDal.GetPrescriptionTypeList; Setup2DAL.GetPrescriptionTypeList; SetupDAL.GetPrescriptionTypeList | 2 (PAGE-650, PAGE-656) | M16 | NOT IN REPO | sp-convention |
| sp_Get_Product_Active | — | ExpenseDal.GetProductListActive; QuotedPriceSetupDAL.GetProductListActive | 1 (PAGE-118) | M04 | NOT IN REPO | sp-convention |
| sp_Get_Product_All | — | ExpenseDal.GetProductListActiveForALl; QuotedPriceSetupDAL.GetProductListActiveForALl | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Get_Product_For_DDl | — | Setup2DAL.Get_ProductList_For_Ddl; SetupDAL.Get_ProductList_For_Ddl | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_Product_ForTargetSetup_DDL | — | BonusCampaignNewDAL.GetProductForDDL | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Get_Product_List | — | CommonDataLoad.GetProduct_Active; Setup2DAL.Get_ProductList_List | 2 (PAGE-675, PAGE-679) | M16,M17 | NOT IN REPO | sp-convention |
| sp_GET_ProductLine | — | SeedDataDAL.GetProLineDataTableList | 11 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04,M15,M05 | NOT IN REPO | sp-convention |
| sp_GET_ProductLine_ById | @id | TherapueticGroupDal.GetProductLineEditData | 1 (PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_GET_ProductLineList | @Parameter | TherapueticGroupDal.GetProductLineList | 1 (PAGE-110) | M04 | NOT IN REPO | sp-convention |
| sp_GET_ProductNameList | — | SeedDataDAL.GetProductNameList | 1 (PAGE-157) | M04 | NOT IN REPO | sp-convention |
| sp_Get_ProductWiseOrderDashboard | @FrmDate, @ToDate, @param | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_ProductWiseOrderPaymentDashboard | @FrmDate, @ToDate, @param | ChartDAL.GetBrandWiseOrderReportDayWiseDAL | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_ProformaInvoiceReportList | @Parm | CmnCrystaltoView.GetProformaInvoListDAL | 2 (PAGE-274, PAGE-505) | M07,M13 | NOT IN REPO | sp-convention |
| sp_GET_ProgramType_ById | @id | ProgramDal.GetEmployeeLeaveForEdit | 2 (PAGE-175, PAGE-177) | M05 | NOT IN REPO | sp-convention |
| sp_GET_ProgramtypeInfo | @Parameter | ProgramDal.GetDepartmentList | 2 (PAGE-175, PAGE-177) | M05 | NOT IN REPO | sp-convention |
| sp_GET_ProgramTypeList | — | SeedDataDAL.GetProgramTypeList | 1 (PAGE-169) | M05 | NOT IN REPO | sp-convention |
| sp_GET_ProgramTypeListAll | — | SeedDataDAL.GetProgramTypeListAll | 9 (PAGE-207, PAGE-208, PAGE-209, PAGE-210…) | M06,M09,M13 | NOT IN REPO | sp-convention |
| sp_GET_ProgramTypeListParm | @Parm | SeedDataDAL.GetProgramTypeListParam | 9 (PAGE-167, PAGE-168, PAGE-173, PAGE-669…) | M16,M05 | NOT IN REPO | sp-convention |
| sp_GET_ProgramTypeWithoutGeneralList | — | SeedDataDAL.GetProgramTypeWithoutGeneralList | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_ProviderDropoutIntrigrationList | — | SAP_IntrigationPointDAL.GetProviderDropoutIntrigrationListDAL | 1 (PAGE-694) | M17 | ['Library.DAL/SAP_IntegrationDAL/sql/sp_Get_ProviderDropoutIntrigrationList.sql:6'] | sp-convention |
| sp_Get_QuotedPriceDetailById | @id | ExpenseDal.GetDetailById; QuotedPriceSetupDAL.GetDetailById | 1 (PAGE-118) | M04 | NOT IN REPO | sp-convention |
| sp_Get_QuotedPriceMaster | — | ExpenseDal.GetQuotedPriceList; QuotedPriceSetupDAL.GetQuotedPriceList | 1 (PAGE-119) | M04 | NOT IN REPO | sp-convention |
| sp_GET_QuotedPriceMaster_ById | @id | ExpenseDal.GetAllowanceMasterById; ExpenseDal.GetMasterById; QuotedPriceSetupDAL.GetQuotedPriceMasterById | 1 (PAGE-118) | M04 | NOT IN REPO | sp-convention |
| sp_Get_ReplaceNoteReport | @Parm | CmnCrystaltoView.GetReplaceNoteReportListDAL | 1 (PAGE-289) | M08 | NOT IN REPO | sp-convention |
| sp_GET_RoleType | — | ApprovalMapDAL.GetRoleTypeDropDown; Setup2DAL.Get_UserTypeInfo | 3 (PAGE-026, PAGE-042, PAGE-679) | M02,M03,M17 | NOT IN REPO | sp-convention |
| sp_Get_RoleTypeByEmpId | @EmpId | CommonDal.Get_RoleTypeEmpId | 2 (PAGE-538, PAGE-545) | M14 | NOT IN REPO | sp-convention |
| sp_Get_RouteInfoforBacktoReturn | @id | SeedDataDAL.GetRouteInfoforBacktoReturn | 1 (PAGE-284) | M08 | NOT IN REPO | sp-convention |
| sp_Get_RouteInfoforCustPayment | @id | SeedDataDAL.GetRouteInfoforCustPayment | 5 (PAGE-311, PAGE-312, PAGE-313, PAGE-314…) | M09 | NOT IN REPO | sp-convention |
| sp_Get_RouteInfoforCustPaymentSnd | @id | SeedDataDAL.GetRouteInfoforCustPaymentSnd | 1 (PAGE-318) | M09 | NOT IN REPO | sp-convention |
| sp_Get_RouteInfoforReturn | @id | SeedDataDAL.GetRouteInfoforReturn | 1 (PAGE-264) | M07 | NOT IN REPO | sp-convention |
| sp_Get_RouteInfoforReturn2ndTimes | @id | SeedDataDAL.GetRouteInfoforSalesReturn2ndTimes | 1 (PAGE-296) | M08 | NOT IN REPO | sp-convention |
| sp_GET_RouteInformationDADetail_ById | @id | RouteInformationDAL.GeteRouteInformationDA_DetailById | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_GET_RouteInformationDADetailDDL | @id | RouteInformationDAL.GeteRouteInformationDA_DDLId; RouteInformationDAL.GetforPaymentRouteInformationDA_DDLId | 5 (PAGE-217, PAGE-311, PAGE-314, PAGE-315…) | M09,M06 | NOT IN REPO | sp-convention |
| sp_GET_RouteInformationMarketDetail_ById | @id | RouteInformationDAL.GetRouteInformationDetailMarketById | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_GET_RouteInformationMaster_ById | @id | RouteInformationDAL.GetRouteInformationMasterById | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Get_RouteInformationMasterList | @Parameter | RouteInformationDAL.GetRouteInformationList | 1 (PAGE-063) | M03 | NOT IN REPO | sp-convention |
| sp_Get_RouterMaster_ByRouterMasterId | @id | RouterSetup.GetRouterMasterForEdit | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_GET_RouterMasterInfo | @Parameter | RouterSetup.GetRouterMasterList | 2 (PAGE-052, PAGE-053) | M03 | NOT IN REPO | sp-convention |
| sp_Get_RouteTypeInfo | — | RouteInformationDAL.GetRouteTypeInfoList | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Get_RPT_PaymentSC_Param | @Parm, @Parm2 | CmnCrystaltoView.GetSC_PaymentReportDAL | 1 (PAGE-340) | M09 | NOT IN REPO | sp-convention |
| sp_Get_RPT_SC_CustomerFinalPaymentReport | @Parm, @Parm2 | CmnCrystaltoView.GetSC_CustomerFinalPaymentReportDAL | 0 | — | NOT IN REPO | sp-convention |
| sp_GET_RSMInfo | @Parameter | RSMSetupDal.Get_RSMInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Get_SalesCollectionReport | @Parm | CmnCrystaltoView.GetSalesCollectionReportDAL | 1 (PAGE-338) | M09 | NOT IN REPO | sp-convention |
| sp_Get_SalesRecordMonthlyDayWsie_New | @CustomerTypeId, @Month, @Year, @param | ChartDAL.GetPerMonthAmount | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_SalesRecordMonthlyDayWsiePayment | @CustomerTypeId, @Month, @Year, @param | ChartDAL.GetPerMonthAmount | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_SalesRejectionReportListLAtest | @Parm, @Parm2 | CmnCrystaltoView.GetSalesRejectionReportDAL | 1 (PAGE-293) | M08 | NOT IN REPO | sp-convention |
| sp_Get_SalesReturnRecordMonthly_New | @Month, @Year, @param | ChartDAL.GetSalesRetrunChartData | 1 (PAGE-464) | M13 | NOT IN REPO | sp-convention |
| sp_Get_SalesReturnReport | @Parm, @Parm2 | CmnCrystaltoView.GetSalesReturnReportListDAL | 1 (PAGE-297) | M08 | NOT IN REPO | sp-convention |
| sp_Get_SampleStockReport | @EmpId, @Month, @Year | CmnCrystaltoView.GetSampleStockRptList | 1 (PAGE-342) | M10 | NOT IN REPO | sp-convention |
| sp_Get_SAP_DICStockReceivePendingData | @Parm | SAP_IntrigationPointDAL.GetSAP_StockReceivePendingDataDIC | 1 (PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_Get_SAP_EmpInfo | @Parm, @Parm2 | SAP_IntrigationPointDAL.GetSAP_EmpInfoDAL | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_Get_SAP_EmpSApCodebyTerritory | @TerritoryCode | SAP_IntrigationPointDAL.GetSAP_EmpSApCodebyTerritory | 3 (PAGE-314, PAGE-323, PAGE-324) | M09 | NOT IN REPO | sp-convention |
| sp_Get_SAP_HideChallanByChallanNo | @ChallanNo | SAP_IntrigationPointDAL.HideChallanByChallanNo | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_Get_SAP_IntrigationPointHeader | @Parm, @Parm2 | SAP_IntrigationPointDAL.GetSAP_IntrigationPointHeaderDAL | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_Get_SAP_ProductInfo | @Parm, @Parm2 | SAP_IntrigationPointDAL.GetSAP_ProductInfoDAL | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_Get_SAP_StockReceivePendingData | @Parm | SAP_IntrigationPointDAL.GetSAP_StockReceivePendingData | 1 (PAGE-680) | M17 | NOT IN REPO | sp-convention |
| sp_Get_SAP_StockReceivePendingDataById | @StockMovementMasterId | SAP_IntrigationPointDAL.GetSAPStockDataById | 2 (PAGE-682, PAGE-683) | M17 | NOT IN REPO | sp-convention |
| sp_GET_SMCFamilyDoctorLastProcessDate | @id | DoctorInfoReportDal.GetLastProessDate | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_GET_SMCType_ById | @id | ProgramDal.GetSMCTypeEditData | 1 (PAGE-175) | M05 | NOT IN REPO | sp-convention |
| sp_GET_SMCtypeInfo | @Parameter | ProgramDal.GetSMCTypeList | 1 (PAGE-175) | M05 | NOT IN REPO | sp-convention |
| sp_GET_SMCTypeListParm | @Parm | SeedDataDAL.GetSMCTypeListParam | 12 (PAGE-167, PAGE-168, PAGE-173, PAGE-210…) | M16,M05,M06 | NOT IN REPO | sp-convention |
| sp_Get_StationType_Active | — | CommonDataLoad.GetStationType_Active | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_GET_StationType_ById | @id | StationDal.GetEmployeeLeaveForEdit | 1 (PAGE-178) | M05 | NOT IN REPO | sp-convention |
| sp_GET_StationTypeInfo | @Parameter | StationDal.GetDepartmentList | 1 (PAGE-179) | M05 | NOT IN REPO | sp-convention |
| sp_GET_StationTypeList | — | SeedDataDAL.GetStationTypeList; Setup2DAL.Get_StationTypeInfoDAL | 5 (PAGE-042, PAGE-168, PAGE-616, PAGE-669…) | M03,M05,M15 | NOT IN REPO | sp-convention |
| sp_GET_StationTypeListAll | — | SetupDAL.Get_StationTypeInfo | 1 (PAGE-587) | M15 | NOT IN REPO | sp-convention |
| sp_GET_SubDepotByComUnitId | @ComUnitId | DepotWiseAreaSetupDal.GetSubDepotList | 1 (PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_Get_SubmarketData_ById | @id | Setup2DAL.GetEditData_SubMarket | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_SubmarketList | — | Setup2DAL.GetSubMarketList | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_subTerritoryData_BySubTerritoryId | @id | Setup2DAL.GetEditData_SubTerritory; Setup2DAL.GetEditData_SubTerritoryOne | 2 (PAGE-057, PAGE-679) | M03,M17 | NOT IN REPO | sp-convention |
| sp_Get_SubTerritoryList | @Parameter | Setup2DAL.GetSubTerritoryList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_TADAAppData | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetTADA_Applog_DAL | 1 (PAGE-696) | M18 | NOT IN REPO | sp-convention |
| sp_Get_TadaClaimList | — | SetupDAL.Get_TADAList | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TadaClaimList_Approval | — | Setup2DAL.Get_TADAList_For_Approval | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TadaClaimList_new | @param | Setup2DAL.Get_TADAList | 2 (PAGE-588, PAGE-679) | M15,M17 | NOT IN REPO | sp-convention |
| sp_Get_TadaClaimMaster_ById | @id | Setup2DAL.GetTadaForEdit | 1 (PAGE-587) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TADAMarketRuleConfig_For_DDL | — | Setup2DAL.GetTADAMarketRuleConfiguration_For_ddl | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_TADAMarketRulesConfig | — | Setup2DAL.GetTADAMarketRuleConfigurationList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_TADAMarketRulesConfig_ByID | @TADAMarketRuleConfigId | Setup2DAL.GeTADAMarketRuleConfigurationForEdit | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_GET_TargetEdit_ById | @id | BonusCampaignNewDAL.GetTargetEditById | 1 (PAGE-555) | M14 | NOT IN REPO | sp-convention |
| sp_Get_TerritorryWiseSalesReportList | @Parm | CmnCrystaltoView.GetTerriTorryWiseSalesReportDataDAL | 1 (PAGE-511) | M13 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryAll_ByAreaId | @id | CommonDal.GetTeritory_ByAreaId_All | 2 (PAGE-544, PAGE-545) | M14 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryAll_ByAreaIdIsNotVaccant | @id | CommonDal.GetTeritory_ByAreaId_IsnotVaccant | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TerritoryAll_ByAreaIdIsVaccant | @id | CommonDal.GetTeritory_ByAreaId_IsVaccant | 1 (PAGE-538) | M14 | NOT IN REPO | sp-convention |
| sp_GET_TerriToryByDCId | @DCid | SeedDataDAL.GetTerriToryByDCId | 1 (PAGE-270) | M07 | NOT IN REPO | sp-convention |
| sp_GET_TerritoryByRouteInformationDDL | @id | RouteInformationDAL.GetTerritoryByRouteInformationDA_DDLId | 2 (PAGE-217, PAGE-264) | M06,M07 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryCodeByRoleTypeEmpId | @EmpId, @RoleType | EmployeeInformationDaL.GetTerritoryCodeByRoleTypeEmpId | 1 (PAGE-624) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryCodeByRoleTypeEmpId_Active | @EmpId, @RoleType | EmployeeInformationDaL.GetTerritoryCodeByRoleTypeEmpIdActive | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TerritoryData_ByTerritoryId | @id | Setup2DAL.GetEditData_Territory | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryList | @RegionId, @areaId | Setup2DAL.GetTerritoryList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryListOrdPerALL | — | Setup2DAL.GetTerritoryListOrdPerALL | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryTargetList | @Parm | BonusCampaignNewDAL.GetTargetUploadList | 1 (PAGE-556) | M14 | NOT IN REPO | sp-convention |
| sp_GET_TerritoryWiseDepotSetupList | @Parameter | TerritoryWiseDepotSetupDAL.GetDAInfo | 1 (PAGE-065) | M03 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryWiseTargetList | @Parameter | AreaWiseTargetSetupDal.Get_TerritoryDataAll | 1 (PAGE-543) | M14 | NOT IN REPO | sp-convention |
| sp_Get_TerritoryWiseTargetSetup | @RegionId, @Year, @month | ZoneWiseTargetSetupDal.Get_TerriTargetAmount_All_ByTeritoryId | 1 (PAGE-538) | M14 | NOT IN REPO | sp-convention |
| sp_GET_tFiscalYearList | — | SeedDataDAL.GetFiscalYearList | 2 (PAGE-555, PAGE-556) | M14 | NOT IN REPO | sp-convention |
| sp_Get_Thana_Active | — | SeedDataDAL.GetThana_All_Active | 1 (PAGE-678) | M17 | NOT IN REPO | sp-convention |
| sp_Get_Thana_WithTagInfo | — | SeedDataDAL.GetThana_All_WithTagInfo | 2 (PAGE-570, PAGE-678) | M15,M17 | NOT IN REPO | sp-convention |
| sp_Get_Thana_WithTagInfo_TerritoryEdit | @id | Setup2DAL.GetThana_All_WithTagInfo_ForEditPage | 3 (PAGE-570, PAGE-581, PAGE-679) | M15,M17 | NOT IN REPO | sp-convention |
| sp_GET_ThanaInfo_ById | @id | ThanaDal.Get_Thana_By_Id_Dal | 2 (PAGE-090, PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_GET_ThanaList | @id | SeedDataDAL.GetThanaList | 2 (PAGE-168, PAGE-669) | M05,M16 | NOT IN REPO | sp-convention |
| sp_Get_ThanaList_OnlyActive_Bydistrict_id | @id | CommonDataLoad.GetThana_ByDistrict_Active | 4 (PAGE-042, PAGE-167, PAGE-557, PAGE-677) | M03,M05,M14 | NOT IN REPO | sp-convention |
| sp_GET_TherapeuticGroup_ById | @id | TherapueticGroupDal.GetTherapueticGroupForEdit | 2 (PAGE-107, PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_GET_TherapueticGroup | @Parameter | TherapueticGroupDal.GetTherapueticGroupList | 2 (PAGE-108, PAGE-110) | M04 | NOT IN REPO | sp-convention |
| sp_GET_ToSheetcode_ById | @id | SeedDataDAL.GetTopSheetCodeSetupById | 1 (PAGE-277) | M07 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetTourPlan_ApplogDAL | 1 (PAGE-706) | M18 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanByTourPlanDate | @empId, @tadaDate | Setup2DAL.Get_TourPlanByTourPlanDate | 1 (PAGE-587) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanDetailsById | @id | TourTypeDal.GetTourPlanDetailsViewDatabyID | 1 (PAGE-591) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanMasteList | @param | TourTypeDal.GetTourPlanList; TourTypeDal.GetTourPlanReport | 4 (PAGE-592, PAGE-593, PAGE-594, PAGE-679) | M15,M17 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanReportList | @param | EmployeeInformationDaL.GetTourPlanReportNew | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TourPlanReportList__n | @EmpInfoId, @Month, @Year | EmployeeInformationDaL.GetTourPlanReport__ | 2 (PAGE-626, PAGE-629) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanSummaryReport | @EmpInfoId, @Month, @UserRoleId, @Year | EmployeeInformationDaL.GetTourPlanSummaryReportReport | 1 (PAGE-627) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPlanTypeDDL | — | SetupDAL.Get_TourPlanTypeDDL | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_GET_TourPlanUserListByParm | @Parm | DoctorVisitDAL.GetPurchaseMasterInfoParam; TourTypeDal.GetPurchaseMasterInfoParam | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TourPlanYear | — | DoctorVisitDAL.GetTourPlanUserYear_Active; TourTypeDal.GetTourPlanUserYear_Active | 2 (PAGE-593, PAGE-594) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPurpose | — | DoctorVisitDAL.GetTourPurposeList | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_TourPurpose_ById | @id | DoctorVisitDAL.GetTourPurposeForEdit; TourTypeDal.GetTourPurposeForEdit | 2 (PAGE-597, PAGE-598) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPurposeDDL | — | SetupDAL.Get_TourPurposeInfo; TourTypeDal.GetTourPurposeList | 2 (PAGE-587, PAGE-596) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPurposeDDLNew | — | SetupDAL.Get_TourPurposeInfoNew | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourPurposeOtherSetupId | @id | SetupDAL.GetEditDataForTPOtherId | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourSetupEmployeeList | @Parm | TourSetupForEmployeeDAL.GetList | 1 (PAGE-617) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourType | — | DoctorVisitDAL.GetTourTypeList; TourTypeDal.GetTourTypeList | 1 (PAGE-599) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TourType_ById | @id | DoctorVisitDAL.GetTourTypeForEdit; TourTypeDal.GetTourTypeForEdit | 1 (PAGE-600) | M15 | NOT IN REPO | sp-convention |
| sp_GET_TrainingDetailMarket_ById | @id | Setup2DAL.GetTrainningDetailMarketById | 1 (PAGE-601) | M15 | NOT IN REPO | sp-convention |
| sp_Get_Trainning_ById | @id | Setup2DAL.GetTrainningForEdit; Setup2DAL.GetTrainningSetupById | 2 (PAGE-601, PAGE-679) | M15,M17 | NOT IN REPO | sp-convention |
| sp_Get_Trainninglist | — | Setup2DAL.GetTranningList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_Transport | — | Setup2DAL.GetTransportList; SetupDAL.GetTransportList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_Transport_ById | @id | Setup2DAL.GetTransportForEdit; SetupDAL.GetTransportForEdit | 1 (PAGE-605) | M15 | NOT IN REPO | sp-convention |
| sp_Get_TTargetAChivementReport | @FromDate, @ToDate, @_Area, @_Month, @_Tr, @_Year, @_Zone | Setup2DAL.Get_TargetAChivementReport | 1 (PAGE-549) | M14 | NOT IN REPO | sp-convention |
| sp_Get_TTargetAChivementReport_nnn | @Area, @FromDate, @Terr, @ToDate, @Type, @ZoneId | Setup2DAL.Get_TargetAChivementReportNew | 1 (PAGE-550) | M14 | NOT IN REPO | sp-convention |
| sp_GET_UserArchivePermission_ByUserId | @UserId | UserInfoDAL.GetUserArchivePermissionById | 0 | — | NOT IN REPO | sp-convention |
| sp_GET_UserDetailMarket_ById | @id | UserInfoDAL.GetUserDetailMarketById | 2 (PAGE-015, PAGE-016) | M02 | NOT IN REPO | sp-convention |
| sp_Get_UserLocationTracking | @empid, @trackDate | CommonDataLoad.GetUserLocationTracking | 1 (PAGE-011) | M01 | NOT IN REPO | sp-convention |
| sp_GET_UserMaster_ByEmpId | @id | UserInfoDAL.GetUserSetupByEmpId | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_GET_UserMaster_ById | @id | UserInfoDAL.GetUserSetupById | 2 (PAGE-015, PAGE-016) | M02 | NOT IN REPO | sp-convention |
| sp_GET_UserRoleDropDown | — | MenuDAL.GetUserRoleDropDown | 1 (PAGE-027) | M02 | NOT IN REPO | sp-convention |
| sp_Get_UserRoleInfo | — | SeedDataDAL.GetUserRoleList; Setup2DAL.Get_UserRoleInfo; SetupDAL.Get_UserRoleInfo | 31 (PAGE-012, PAGE-015, PAGE-016, PAGE-549…) | M15,M18,M02 | NOT IN REPO | sp-convention |
| sp_GET_UserRoleInfo_ById | @id | RSMSetupDal.GetUserRoleEditDataDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_UserRoleInfoRoleType_ById | @id | SeedDataDAL.GetUserRoleTypeListById | 2 (PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_GET_UserRoleList | @Parameter | RSMSetupDal.Get_UserRoleInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_UserSettingPanel | — | UserSettingPanelDal.Get_UserSettingPanel | 1 (PAGE-025) | M02 | NOT IN REPO | sp-convention |
| sp_Get_UserTypeInfo | — | SeedDataDAL.GetUserTypeList | 2 (PAGE-015, PAGE-016) | M02 | NOT IN REPO | sp-convention |
| sp_GET_VacentArea | @RegionId | RSMSetupDal.Get_VacentArea | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_VacentGroup | — | RSMSetupDal.Get_VacentGroup | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_VacentRegion | @GroupId | RSMSetupDal.Get_VacentRegion | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_GET_VacentTerritory | @AreaId | RSMSetupDal.Get_VacentTerritory | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Get_WeekNameInfo | — | RouteInformationDAL.GetWeekNameList | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Get_Zone_All_Active | @GroupId | CommonDataLoad.GetZone_byGroupId_Active; SeedDataDAL.GetZone_ActiveOnly | 8 (PAGE-102, PAGE-463, PAGE-557, PAGE-587…) | M15,M17,M03 | NOT IN REPO | sp-convention |
| sp_Get_Zone_All_Active_ByGroup | @GroupId | CommonDal.Get_Forddl_ZoneActive_All_ByGroup | 6 (PAGE-538, PAGE-539, PAGE-540, PAGE-543…) | M14 | NOT IN REPO | sp-convention |
| sp_Get_Zone_All_Active_ZoneTarget | @GroupId | CommonDal.Get_ZoneActive_All_ByGroup; ZoneWiseTargetSetupDal.Get_ZoneActive_All_ByGroup | 1 (PAGE-546) | M14 | NOT IN REPO | sp-convention |
| sp_Get_Zone_AllByGroup | @GroupId | CommonDataLoad.GetZone_byGroupId_All | 6 (PAGE-463, PAGE-506, PAGE-549, PAGE-557…) | M13,M14,M15 | NOT IN REPO | sp-convention |
| sp_Get_Zone_AllByGroupRpt | @GroupId | CommonDataLoad.GetZone_byGroupId_Rpt | 0 | — | NOT IN REPO | sp-convention |
| sp_Get_Zone_ForDSM | @GroupId | CommonDataLoad.GetZone_byGroupId_forDSM | 6 (PAGE-463, PAGE-506, PAGE-538, PAGE-539…) | M14,M13 | NOT IN REPO | sp-convention |
| sp_Get_ZoneData_ByZoneId | @id | ZoneSetupDAL.GetEditData | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_ZoneinfoByEmpId | @EmpId | CommonDal.Get_ZoneinfoByEmpId | 4 (PAGE-538, PAGE-543, PAGE-544, PAGE-545) | M14 | NOT IN REPO | sp-convention |
| sp_Get_ZoneList | — | ZoneSetupDAL.GetInstitutionList; ZoneSetupDAL.GetZoneList | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Get_ZoneListOrdPer | — | Setup2DAL.GetZoneListOrdPer | 2 (PAGE-017, PAGE-595) | M02,M15 | NOT IN REPO | sp-convention |
| sp_Get_ZoneTargetAmount | @RegionId, @Year, @month | ZoneWiseTargetSetupDal.Get_AreaTargetAmount_All_ByZone | 1 (PAGE-539) | M14 | NOT IN REPO | sp-convention |
| sp_Get_ZoneWiseTargetList | @Parameter | ZoneWiseTargetSetupDal.Get_SaveDataAll | 1 (PAGE-547) | M14 | NOT IN REPO | sp-convention |
| sp_GetChamber_ByDoctorId | @DoctorId | CommonDataLoad.GetChamber_ByDoctorId | 2 (PAGE-557, PAGE-677) | M14,M17 | NOT IN REPO | sp-convention |
| sp_GetCustomer_Doctor_TransferAppList | @Parm, @Type | OrderTrackingDAL.GetCustomer_Doctor_Transfer | 1 (PAGE-202) | M05 | NOT IN REPO | sp-convention |
| sp_GetCustomerProviderTypeApproveList | @Parm | OrderTrackingDAL.GetCustomerProviderApprovalList | 1 (PAGE-204) | M05 | NOT IN REPO | sp-convention |
| sp_GetDatewithinDateRange | @fDate, @tDate | ZoneWiseTargetSetupDal.GetDatewithinDateRange | 1 (PAGE-538) | M14 | NOT IN REPO | sp-convention |
| sp_GetDCRRXDoctorWiseRptView | @Month, @Type, @Year | CmnCrystaltoView.GetDoctorWiseDayList | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_GetDoctor_SalesPerson_Active | — | CommonDataLoad.GetDoctor_SalesPerson_Active | 2 (PAGE-168, PAGE-669) | M05,M16 | NOT IN REPO | sp-convention |
| sp_GetDoctor_SalesPerson_forSearch | — | CommonDataLoad.GetDoctorSalesPerson_forSearch | 2 (PAGE-173, PAGE-670) | M05,M16 | NOT IN REPO | sp-convention |
| sp_GetDoctorProviderTypeApproveList | @Parm | OrderTrackingDAL.GetDoctorProviderApprovalList | 1 (PAGE-206) | M05 | NOT IN REPO | sp-convention |
| sp_GetFinalSales | @fromdate, @todate | TotalSummaryDAL.FinalSales | 2 (PAGE-509, PAGE-510) | M13 | NOT IN REPO | sp-convention |
| sp_GetMarketInfoApprovalList | @Parm | OrderTrackingDAL.GetMarketInfoApprovalList | 1 (PAGE-097) | M03 | NOT IN REPO | sp-convention |
| sp_GetOrderInvoiceIsZero | — | CommonDataLoad.GetOrderInvoiceIsZero | 2 (PAGE-222, PAGE-284) | M06,M08 | NOT IN REPO | sp-convention |
| sp_GetOrganogramreportList | @Parm | OrderTrackingDAL.GetOrganogramreportList | 1 (PAGE-625) | M15 | NOT IN REPO | sp-convention |
| sp_GetRXDoctorWiseRpt | @ApprovalStatus, @Month, @MonthValue, @Year | CmnCrystaltoView.GetDCRDoctorWiseDayListNew; CmnCrystaltoView.GetRXDoctorWiseDayListNew | 0 | — | NOT IN REPO | sp-convention |
| SP_GetSalesmanCustomerCoverageSummary | @EmpInfoId, @EndDate, @SalesPersonMasterCode, @StartDate | DoctorVisitDAL.GetSalesmanCustomerCoverageSummary | 1 (PAGE-667) | M16 | NOT IN REPO | sp-convention |
| sp_GetWarningForCustomerPayment_new | @CustCode, @CustID | InvoiceDAL.GetWarning; SalesReturnDAL.GetWarning | 0 | — | NOT IN REPO | sp-convention |
| sp_HigharchyInfoByEmployeeId | @EmployeeId, @GroupId, @RoleId, @ZoneId | ChartDAL.GetTotalInfoByCurrentDateDAL; CommonDataLoad.GetHigharcyInfoByEmployeeId | 9 (PAGE-041, PAGE-463, PAGE-464, PAGE-506…) | M14,M13,M03 | NOT IN REPO | sp-convention |
| sp_I_Customer | @ADDRESS1, @ADDRESS2, @BRANCH, @BRANCHDES, @CITY, @CONTACTNUMBER, @CONTACTPERSON, @CUSTOMERNAME, @CustomerCode, @DZSMCode, @DZSMName, @FECod | ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow | 5 (PAGE-192, PAGE-314, PAGE-323, PAGE-324…) | M09,M05,M14 | NOT IN REPO | sp-convention |
| sp_I_DepotWiseArea | @AreaId, @DepotId, @DepotWiseAreaId, @EntryBy, @UpdateBy | DepotWiseAreaSetupDal.DepotWiseAreaInfo | 1 (PAGE-035) | M03 | NOT IN REPO | sp-convention |
| sp_I_Diposit_New | @AIT, @AccountName, @Amount, @BankId, @BranchName, @CheckDate, @CheckNumber, @CompanyId, @DepositCode, @DepositDate, @DepositType, @EntryBy, | CompanywisebranchDal.SaveDepositInfo | 4 (PAGE-075, PAGE-314, PAGE-323, PAGE-324) | M09,M03 | NOT IN REPO | sp-convention |
| sp_I_FixedCustomer | @code, @date | FixedCustomerUploadDal.SaveFixedCustomer | 1 (PAGE-200) | M05 | NOT IN REPO | sp-convention |
| sp_InsertCustPayDetail_DeleteLog | @CollectionNo, @CustPayDetailId, @CustPayId, @InvoiceId, @LoginName, @NewMasterId, @OrigId, @OrigMasterId, @Remarks, @User | CustomerPaymentDeleteDAL.DeleteCustomerPaymentDetail; SalesOperationRollbackDAL.RollbackCustomerPayment | 2 (PAGE-313, PAGE-528) | M09,M13 | NOT IN REPO | sp-convention |
| sp_InvoceLifecycle | @fromDate, @toDate | CompanySalesReportDAL.GetInvoceLifecycleReport | 1 (PAGE-249) | M07 | NOT IN REPO | sp-convention |
| sp_LoadingSummary | @param | OrderInfoDAL.LoadSummaryList; OrderInfoDALSalesReturn.LoadSummaryList; SalesReturnNewDAL.LoadSummaryList | 1 (PAGE-271) | M07 | NOT IN REPO | sp-convention |
| sp_LoadOrderListForOrderCreation | @comunitId, @manufacId | OrderInfoDALSalesReturn.LoadOrderForOrderCreation; SalesReturnNewDAL.LoadOrderForOrderCreation | 0 | — | NOT IN REPO | sp-convention |
| sp_LoadOrderListForOrderCreationbyTerri | @TerritoryId, @comunitId, @manufacId | OrderInfoDAL.LoadOrderForOrderCreation | 1 (PAGE-217) | M06 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1224'] | sp-convention |
| sp_LoadSalesReturnReportSAP | @fromdate, @todate | TotalSummaryDAL.LoadSalesReturnReportSAP | 1 (PAGE-691) | M17 | NOT IN REPO | sp-convention |
| sp_MioUpdateInCustomerInfo | @mio, @name, @ter | MiaInformationDAL.MioUpdate | 1 (PAGE-634) | M15 | NOT IN REPO | sp-convention |
| sp_MiowiseMISReport | @fromDate, @toDate, @unitid | MIOTotalSummaryDAL.LoadSummaryProductcodewiseGyash | 2 (PAGE-518, PAGE-519) | M13 | NOT IN REPO | sp-convention |
| sp_MIS_AttendanceAndDCRReport | @EmpInfoId, @EmpMasterCode, @EndDate, @StartDate | DoctorVisitDAL.GetMISAttendanceAndDCRReport | 1 (PAGE-665) | M16 | NOT IN REPO | sp-convention |
| sp_MIS_GetSalesmanCustomerVisitReport | @EmpInfoId, @EndDate, @FilterStatus, @SalesPersonMasterCode, @StartDate | DoctorVisitDAL.GetSalesmanCustomerVisitReport; ReportController.GetSalesmanCustomerVisitReportData | 1 (PAGE-666) | M16 | NOT IN REPO | sp-convention |
| sp_NTS_Group_Active | — | CommonDal.Get_Group_All; NationalTargetSetupDal.Get_Group_All; NationalTargetSetupDal.Get_SaveDataAll | 10 (PAGE-538, PAGE-539, PAGE-540, PAGE-541…) | M14 | NOT IN REPO | sp-convention |
| sp_NumberofInvoiceandCust | @fromdate, @todate | TotalSummaryDAL.FinalSales2 | 2 (PAGE-509, PAGE-510) | M13 | NOT IN REPO | sp-convention |
| sp_opeingBalanceCreate | @FinancialYear, @FromDate, @ToDate | UserInfoDAL.CreateOpeningBalance | 1 (PAGE-038) | M03 | NOT IN REPO | sp-convention |
| sp_OrderGenerationFromUploadOrder | @OrderMasterID_In | ExcelUpForOrderListDAL.TransfarOrderID_DAL | 1 (PAGE-224) | M06 | NOT IN REPO | sp-convention |
| sp_OrderGenerationFromUploadOrder_SingleOrder | @IsApiData, @OrderCode, @OrderMasterID_In | PendingOrderDal.SaveOrderGenerationInfo | 1 (PAGE-231) | M06 | NOT IN REPO | sp-convention |
| sp_OrderMonitoringPanel_Bizmotion | @fromdate, @todate | TotalSummaryDAL.OrderMonitoring_Bizmo | 1 (PAGE-227) | M06 | NOT IN REPO | sp-convention |
| sp_PaymentConformationFull | @InvoiceNo, @UpdateBy, @UpdateDate | InvoiceDAL.SavePaymentConformationFull; SalesReturnDAL.SavePaymentConformationFull | 2 (PAGE-264, PAGE-296) | M07,M08 | NOT IN REPO | sp-convention |
| sp_Process_DWSPReport_Territory | @ApprovalStatus, @AreaId, @Month, @MonthValue, @RegionId, @TrId, @Year | CmnCrystaltoView.GetDWSPMonthlyList_Mew | 1 (PAGE-548) | M14 | NOT IN REPO | sp-convention |
| sp_Process_ProformaInvoiceByOrderId | @BatchNo1, @DANameId, @OrderId, @UserId | OrderInfoDAL.GenerateInvoiceByOrderId; OrderInfoDALSalesReturn.GenerateInvoiceByOrderId; SalesReturnNewDAL.GenerateInvoiceByOrderId | 1 (PAGE-217) | M06 | NOT IN REPO | sp-convention |
| sp_Process_SubDepoProformaInvoiceByOrderId | @BatchNo1, @OrderId, @UserId | OrderInfoDAL.GenerateSubInvoiceByOrderId; OrderInfoDALSalesReturn.GenerateSubInvoiceByOrderId; SalesReturnNewDAL.GenerateSubInvoiceByOrderId | 1 (PAGE-456) | M12 | NOT IN REPO | sp-convention |
| sp_Process_YearlyLeaveBalance | @ProcessBy | YearlyLeaveprocessDal.SaveYearlyleave | 1 (PAGE-611) | M15 | NOT IN REPO | sp-convention |
| sp_ProductWiseBranchwiseBusinessSummaryMISReport | @Branch, @fromdate, @todate | TotalSummaryDAL.LoadSummaryzoneBranchTerritorywiseBLL | 1 (PAGE-534) | M13 | NOT IN REPO | sp-convention |
| sp_ProductWiseBusinessSummaryMISReport | @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewise | 2 (PAGE-498, PAGE-536) | M13 | NOT IN REPO | sp-convention |
| sp_ProductWiseBusinessSummaryMISReportByParam | @Area, @Terr, @Type, @ZonId, @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewiseNew | 1 (PAGE-532) | M13 | NOT IN REPO | sp-convention |
| sp_Rep_DepopsitSlip_BusinessSummary | @fromdate, @todate | TotalSummaryDAL.LoadDepositSlipSummary | 2 (PAGE-325, PAGE-326) | M09 | NOT IN REPO | sp-convention |
| sp_Rep_DepopsitSlip_BusinessSummaryClosingReport | @fromdate, @todate | TotalSummaryDAL.LoadDepositSlipSummaryProcess | 1 (PAGE-325) | M09 | NOT IN REPO | sp-convention |
| sp_Report_CentralStore_StockValuationLedger | @CategoryId, @FromDate, @ProductId, @ToDate | CentralStoreDAL.GetStockValuationLedger | 1 (PAGE-341) | M09 | NOT IN REPO | sp-convention |
| sp_Report_CustomerInvoiceAgingOutstanding | @AsOnDate, @ComUnitId, @CustomerMasterId, @FromDate, @OnlyDueInvoices, @SalesPersonId, @ToDate | CustomerInvoiceAgingDAL.GetCustomerInvoiceAging | 1 (PAGE-306) | M09 | ['Live_Deployment_Script_All_Updates.sql:129'] | sp-convention |
| sp_Report_DCStore_StockValuationLedger | @CategoryId, @ComUnitId, @FromDate, @ProductId, @ToDate | DCStockReportDAL.GetDCStockValuationLedger | 1 (PAGE-365) | M10 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:51', 'SP_Update_StockValuation_TransactionDate_Logic.sql:18'] | sp-convention |
| sp_Report_NationalStockValuation_BatchDetails | @FromDate, @LocationName, @ProductCode, @ToDate | CentralStoreDAL.GetNationalStockValuationBatchDetails | 1 (PAGE-374) | M10 | ['SP_Update_StockValuation_TransactionDate_Logic.sql:353'] | sp-convention |
| sp_Report_NationalStockValuation_DateRange | @CategoryId, @FromDate, @ProductId, @ToDate | CentralStoreDAL.GetNationalStockValuation | 1 (PAGE-374) | M10 | ['SP_Update_StockValuation_TransactionDate_Logic.sql:194'] | sp-convention |
| sp_Rpt_AreawiseDoctorWeek | @Area, @Parameter, @Zone, @frmDate, @toDate | DoctorInfoReportDal.Get_Dactor_Information_Area_Wise | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_RPT_BusinessSummaryMISReport | @Area, @DCId, @EmployeeId, @RoleId, @Terr, @Type, @fromdate, @todate | TotalSummaryDAL.LoadSummaryProductcodewiseGyash_dzsm | 0 | — | NOT IN REPO | sp-convention |
| sp_Rpt_BusinessSummaryProductwise | @fromdate, @todate | TotalSummaryDAL.LoadBusinessSummaryProductwise | 1 (PAGE-534) | M13 | NOT IN REPO | sp-convention |
| sp_RPT_ChallanStatusByDate | @Month, @Year | CmnCrystaltoView.GetChallanStatusByDateDAL | 1 (PAGE-692) | M17 | NOT IN REPO | sp-convention |
| sp_Rpt_DCRInfo_ById | @id | DoctorVisitDAL.GetDCRReportDataById | 1 (PAGE-659) | M16 | NOT IN REPO | sp-convention |
| sp_RPT_DoctorInfoReport | @Parameter, @frmDate, @toDate | DoctorInfoReportDal.Get_Dactor_Information_Report | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_Rpt_DoctorwiseDoctorWeek | @Area, @Parameter, @Teritory, @Zone, @frmDate, @toDate | DoctorInfoReportDal.Get_Dactor_Information_Doc_Wise | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_RPT_MIOWiseBusinessSummary | @Depid, @fromdate, @todate | TotalSummaryDAL.LoadMIOWiseBusinessSummaryDAL | 1 (PAGE-517) | M13 | NOT IN REPO | sp-convention |
| sp_Rpt_MIOwiseDoctorWeek | @Area, @Parameter, @Teritory, @Zone, @frmDate, @toDate | DoctorInfoReportDal.Get_Dactor_Information_MIO_Wise | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_RPT_MIS_BusinessSummary | @Area, @Terr, @Type, @ZonId, @fromdate, @todate | TotalSummaryDAL.LoadRptBussinessSummary_LoadingDAL | 1 (PAGE-275) | M07 | NOT IN REPO | sp-convention |
| sp_RPT_MIS_BusinessSummary_Acc | @Area, @Terr, @Type, @ZonId, @fromdate, @todate | TotalSummaryDAL.Sales_Collection_Reports_AccDAL | 1 (PAGE-337) | M09 | NOT IN REPO | sp-convention |
| sp_RPT_MIS_ProductWiseSalesReport | @Area, @Terr, @Type, @ZonId, @fromdate, @todate | TotalSummaryDAL.ProductWiseSalesReportDAL | 1 (PAGE-523) | M13 | NOT IN REPO | sp-convention |
| sp_RPT_MIS_RptMIOWiseReceiveableReport | @Area, @Terr, @Type, @ZonId, @fromdate, @todate | TotalSummaryDAL.RptMIOWiseReceiveableReport | 1 (PAGE-336) | M09 | NOT IN REPO | sp-convention |
| sp_RPT_MonthlyExpense | @Month, @Parm, @Year | EmployeeInformationDaL.GetEmployeeInformationListForReport | 1 (PAGE-624) | M15 | NOT IN REPO | sp-convention |
| sp_RPT_SalesConfirmStatusByDate | @Month, @Year | CmnCrystaltoView.GetSalesConfirmStatusByDateReportDAL | 1 (PAGE-692) | M17 | NOT IN REPO | sp-convention |
| sp_RPT_SalesReturnStatusByDate | @Month, @Year | CmnCrystaltoView.GetSalesReturnReportDAL | 1 (PAGE-692) | M17 | NOT IN REPO | sp-convention |
| sp_Rpt_SMCFamilyDoctorReport_ProcessData | @Area, @Parameter, @Teritory, @Zone, @frmDate, @toDate | DoctorInfoReportDal.Get_Dactor_Information_Details | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_Rpt_ZonewiseDoctorWeek | @Parameter, @Zone, @frmDate, @toDate | DoctorInfoReportDal.Get_Dactor_Information_Zone_Wise | 1 (PAGE-674) | M16 | NOT IN REPO | sp-convention |
| sp_RPTMonitoringReport | @Type, @fromdate, @todate | TotalSummaryDAL.rptMonitoringReport | 1 (PAGE-520) | M13 | NOT IN REPO | sp-convention |
| sp_RSMInfoByEmployeeId | @EmployeeId | CommonDataLoad.GetRSMInfoByEmployeeId | 1 (PAGE-506) | M13 | NOT IN REPO | sp-convention |
| sp_Sales_CustomerPaymentPosting | @CustPayId, @InvoiceId | CustPaymentDAL.PaymentJournalPosting | 1 (PAGE-311) | M09 | ['Live_Deployment_Script_All_Updates.sql:403'] | sp-convention |
| sp_Sales_DeliverySalesPosting | @InvoiceId | InvoiceDAL.PostDeliveryConfFullJournal; InvoiceDAL.PostDeliveryConfPartialJournal | 2 (PAGE-215, PAGE-271) | M06,M07 | NOT IN REPO | sp-convention |
| sp_SalesReturnPosting | — | ReturnInvoiceDAL.PostSalesReturn | 1 (PAGE-294) | M08 | ['Live_Deployment_Script_All_Updates.sql:949'] | sp-convention |
| sp_SAP_BankDeposit_SAP_Process | @fromdate, @todate | BonusCampaignNewDAL.BankDeposit_SAP_Process | 1 (PAGE-686) | M17 | NOT IN REPO | sp-convention |
| sp_SAP_BankDepositPosting | @FrmDate, @ToDate | TotalSummaryDAL.BankDepositPosting_SAP | 1 (PAGE-686) | M17 | NOT IN REPO | sp-convention |
| sp_SAP_BankDepositSendtoSAP | @Ids | BonusCampaignNewDAL.BankDeposit_SAP_ProcessUpdate | 0 | — | NOT IN REPO | sp-convention |
| sp_SAP_StockReceive | @ApproveBy, @ChallanNo | SAP_IntrigationPointDAL.SaveStockReceive | 4 (PAGE-680, PAGE-681, PAGE-682, PAGE-683) | M17 | NOT IN REPO | sp-convention |
| sp_SAP_Up_StockReceiveQty | @StockMovementDetailId, @quantity | SAP_IntrigationPointDAL.UpdateSAPQtyStockReceive | 2 (PAGE-682, PAGE-683) | M17 | NOT IN REPO | sp-convention |
| sp_Save_ApprovalMapDetail | @ApprovalMapMasterId, @Order, @ToRoleId | ApprovalMapDAL.SaveApprovalMapDetail | 1 (PAGE-026) | M02 | NOT IN REPO | sp-convention |
| sp_Save_ApprovalMapMaster | @FromRoleId, @MenuId, @MenuName | ApprovalMapDAL.SaveApprovalMapMaster | 1 (PAGE-026) | M02 | NOT IN REPO | sp-convention |
| sp_Save_AreaInfo | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaName, @createdBy, @id, @isActive, @remarks, @zoneId | Setup2DAL.SaveAreaInfo | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Save_AreaWiseTargetSetup | @Amount, @AreaId, @AreaWTSetupId, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @Year | AreaWiseTargetSetupDal.SaveZoneWiseTarget | 3 (PAGE-539, PAGE-540, PAGE-543) | M14 | NOT IN REPO | sp-convention |
| sp_Save_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_ASMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Save_BDoctorChemberDetail | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Save_BonusCampaignCustomerDetail | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignCategoryId, @Ca | BonusCampaignNewDAL.SaveBonusCampaign; BonusCampaignNewDAL.SaveBonusCampaign_final | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Save_BonusCampaignMarketDetail | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignCategoryId, @Ca | BonusCampaignNewDAL.SaveBonusCampaign; BonusCampaignNewDAL.SaveBonusCampaign_final | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Save_BonusCampaignNewDetail | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignCategoryId, @Ca | BonusCampaignNewDAL.SaveBonusCampaign; BonusCampaignNewDAL.SaveBonusCampaign_final | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Save_BonusCampaignNewDetail_Up | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignCategoryId, @Ca | BonusCampaignNewDAL.SaveBonusCampaign; BonusCampaignNewDAL.SaveBonusCampaign_final | 6 (PAGE-111, PAGE-112, PAGE-113, PAGE-114…) | M04 | NOT IN REPO | sp-convention |
| sp_Save_BonusCampaignNewMaster | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusTypeId, @CampaignCategoryId, @CampaignDetailId, @CampaignMasterId, @CampaignName, @ | BonusCampaignNewDAL.SaveBonusCampaign | 5 (PAGE-111, PAGE-113, PAGE-114, PAGE-115…) | M04 | NOT IN REPO | sp-convention |
| sp_Save_BonusCampaignNewMaster_final | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignDetailId, @Camp | BonusCampaignNewDAL.SaveBonusCampaign_final | 1 (PAGE-112) | M04 | NOT IN REPO | sp-convention |
| sp_Save_CustomerMaster | @Address, @BCDS, @CellNo, @ComUnitId, @CustomerMasterId, @CustomerName, @CustomerType, @CustomerTypeId, @DZSMStationTypeId, @DistributionRou | CustomerInfoDAL.SaveInfo; MarketStructureTransferDAL.SaveInfo; OrderTrackingDAL.SaveInfo | 1 (PAGE-168) | M05 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:402'] | sp-convention |
| sp_Save_CustomerPropUpdateDetail | @ConvertType, @CustCode, @CustPropMasterId, @CustPropUpdateDetailId, @CustTypeCode, @EntryBy, @MarketCode, @PharmaPlatformCode, @ProviderTyp | BonusCampaignNewDAL.SaveCustomerInfoForExcel | 1 (PAGE-203) | M05 | NOT IN REPO | sp-convention |
| sp_Save_CustomerPropUpdateMaster | @ConvertType, @CustCode, @CustPropMasterId, @CustPropUpdateDetailId, @CustTypeCode, @EntryBy, @MarketCode, @PharmaPlatformCode, @ProviderTyp | BonusCampaignNewDAL.SaveCustomerInfoForExcel | 1 (PAGE-203) | M05 | NOT IN REPO | sp-convention |
| sp_Save_CustomerTypeInfo | @CustomerCategoryId, @CustomerType, @EntryBy, @IsActive, @IsCampaign, @IsDefault, @UpdateBy, @id | CustomerTypeDal.Save_DepartmentInfo | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_Save_CustProductLineDetail | @Address, @BCDS, @CellNo, @ComUnitId, @CustomerMasterId, @CustomerName, @CustomerType, @CustomerTypeId, @DZSMStationTypeId, @DistributionRou | CustomerInfoDAL.SaveInfo; MarketStructureTransferDAL.SaveInfo; OrderTrackingDAL.SaveInfo | 1 (PAGE-168) | M05 | NOT IN REPO | sp-convention |
| sp_Save_DAInfo | @ActiveDate, @Address, @ComUnitId, @DAId, @EmergencyContactNo, @EntryBy, @EntryDate, @InactiveDate, @IsActive, @JoiningDate, @NID, @Name, @P | DAInfoDal.SaveDAInfo | 1 (PAGE-237) | M07 | NOT IN REPO | sp-convention |
| sp_Save_DcWiseTerritoryDetail | @AreaId, @DCId, @DcWiseTerritoryMasterId, @EntryBy, @GroupId, @RegionId, @SubDepotId, @TerritoryId, @UpdateBy | TerritoryWiseDepotSetupDAL.SaveBonusCampaign | 1 (PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_Save_DcWiseTerritoryMaster | @AreaId, @DCId, @DcWiseTerritoryMasterId, @EntryBy, @GroupId, @RegionId, @SubDepotId, @TerritoryId, @UpdateBy | TerritoryWiseDepotSetupDAL.SaveBonusCampaign | 1 (PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_Save_DepartmentInfo | @DepartmentName, @EntryBy, @IsActive, @MasterId, @PageName, @UpdateBy, @id | DepartmentDal.Save_DepartmentInfo | 1 (PAGE-566) | M15 | NOT IN REPO | sp-convention |
| sp_Save_DesignationInfo | @DesigName, @EntryBy, @IsActive, @MasterId, @PageName, @UpdateBy, @id | DesignationDal.Save_DesignationInfo | 1 (PAGE-568) | M15 | NOT IN REPO | sp-convention |
| sp_Save_DistictInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id | ThanaDal.Save_DistictInfo | 1 (PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_Save_DoctorBrandDetail | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorCategory | @Activedate, @CategoryName, @EntryBy, @UpdateBy | DoctorCategoryDal.SaveDoctorCategory; Setup2DAL.SaveDoctorCategory | 1 (PAGE-639) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorChamber | @Activedate, @ChamberName, @EntryBy, @UpdateBy | DoctorChamberDal.SaveDoctorChamber; Setup2DAL.SaveDoctorChamber | 1 (PAGE-637) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorContactDetail_New | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorDegree | @Activedate, @DegreeName, @DoctorTypeId, @EntryBy, @UpdateBy | DoctorDegreeDal.SaveDoctorDegree; Setup2DAL.SaveDoctorDegree | 1 (PAGE-641) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorDegreeDetail | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorDesignation | @Activedate, @DesignationName, @EntryBy, @UpdateBy | DoctorDesignationDal.SaveDoctorDesignation; Setup2DAL.SaveDoctorDesignation | 1 (PAGE-643) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorMaster | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:671'] | sp-convention |
| sp_Save_DoctorPatientType | @Activedate, @EntryBy, @IsActive, @PatientType, @PatientTypeId, @UpdateBy | PatientTypeDal.SaveDoctorpatientType; Setup2DAL.SaveDoctorpatientType | 1 (PAGE-647) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorPropUpdateDetail | @ConvertType, @CustCode, @CustPropMasterId, @CustPropUpdateDetailId, @CustTypeCode, @EntryBy, @MarketCode, @PharmaPlatformCode, @ProviderTyp | BonusCampaignNewDAL.SaveDoctorInfoForExcel | 1 (PAGE-205) | M05 | NOT IN REPO | sp-convention |
| sp_Save_DoctorPropUpdateMaster | @ConvertType, @CustCode, @CustPropMasterId, @CustPropUpdateDetailId, @CustTypeCode, @EntryBy, @MarketCode, @PharmaPlatformCode, @ProviderTyp | BonusCampaignNewDAL.SaveDoctorInfoForExcel | 1 (PAGE-205) | M05 | NOT IN REPO | sp-convention |
| sp_Save_DoctorSpecialDay | @Activedate, @EntryBy, @IsActive, @SpecialDay, @SpecialDayId, @UpdateBy | DoctorSpecailDayDal.SaveDoctorSpeacialDay; Setup2DAL.SaveDoctorSpeacialDay | 1 (PAGE-651) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorSpecialDayDetails | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorSpeciality | @Activedate, @EntryBy, @IsActive, @SpecialityId, @SpecialityName, @UpdateBy | DoctorSpecialityDal.SaveDoctorSpeaciality; Setup2DAL.SaveDoctorSpeaciality | 1 (PAGE-645) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DoctorSpecialityDetail | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | NOT IN REPO | sp-convention |
| sp_Save_DWSPMaster | @CampaignAmount, @DWSPDate, @FCBAmount, @GeneralAmount, @TerritoryId, @empId | AreaWiseTargetSetupDal.Save_DWSPMaster | 1 (PAGE-538) | M14 | NOT IN REPO | sp-convention |
| sp_Save_Employe_LeaveType_Infos | @EntryBy, @IsActive, @LeaveDays, @LeaveTypeId, @LeaveTypeName, @Name, @UpdateBy, @id | LeaveDal.Save_LeaveInfo | 2 (PAGE-577, PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Save_Employee_Holiday | @DayName, @EntryBy, @FiscalYear, @HolidayDate, @HolidayId, @HolidayToDate, @IsActive, @Name, @UpdateBy, @id | HolidayDal.Save_LeaveInfo | 1 (PAGE-575) | M15 | NOT IN REPO | sp-convention |
| sp_Save_EmployeeAllowanceDetail | @AddressPermanent, @AddressPresent, @AllowanceId, @BloodGroup, @CellNumber, @CompanyId, @CompanyUnitId, @DateOfBirth, @DepartmentId, @Design | EmployeeInformationDaL.SaveEmployeeInformation | 3 (PAGE-613, PAGE-614, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Save_EmployeeInformation | @AddressPermanent, @AddressPresent, @AllowanceId, @BloodGroup, @CellNumber, @CompanyId, @CompanyUnitId, @DateOfBirth, @DepartmentId, @Design | EmployeeInformationDaL.SaveEmployeeInformation | 3 (PAGE-613, PAGE-614, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ExpenseClaimDetails | @Amount, @EmpInfoId, @EntryBy, @ExpenseClaimID, @ExpenseDate, @ExpenseDetailId, @ExpenseTypDetailsId, @ExpenseTypeId, @Remarks, @UpdateBy, @ | Setup2DAL.SaveExpenseClaimDetails; Setup2DAL.Save_ExpenseClaim; SetupDAL.SaveExpenseClaimDetails (+1) | 1 (PAGE-570) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ExpenseClaimMaster | @Amount, @EmpInfoId, @EntryBy, @ExpenseClaimID, @ExpenseDate, @ExpenseTypDetailsId, @ExpenseTypeId, @Remarks, @UpdateBy, @ValueText | Setup2DAL.Save_ExpenseClaim; SetupDAL.Save_ExpenseClaim | 1 (PAGE-570) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ExpenseTypeDetails | @EmpNameMult, @EntryBy, @ExpenseAmount, @ExpenseTypDetailsId, @ExpenseTypeId, @ExpenseTypeName, @FieldName, @ImageRequired, @IsActive, @IsRe | Setup2DAL.SaveExpenseTypeDetails; SetupDAL.SaveExpenseTypeDetails; SetupDAL.SaveExpenseTypeMaster | 1 (PAGE-572) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ExpenseTypeMaster | @EmpNameMult, @EntryBy, @ExpenseAmount, @ExpenseTypDetailsId, @ExpenseTypeId, @ExpenseTypeName, @FieldName, @ImageRequired, @IsActive, @IsRe | Setup2DAL.SaveExpenseTypeMaster; SetupDAL.SaveExpenseTypeMaster | 1 (PAGE-572) | M15 | NOT IN REPO | sp-convention |
| sp_Save_FinancialYearInfo | @EntryBy, @IsActive, @UpdateBy, @YearFromDate, @YearTodate, @id | FinancialYearDal.Save_DepartmentInfo | 1 (PAGE-039) | M03 | NOT IN REPO | sp-convention |
| sp_Save_GenericGroup | @EntryBy, @GenericGroupName, @InactiveBy, @InactiveDate, @IsActive, @MasterId, @PageName, @UpdateBy, @id | GenericGroupDal.Save_GenericGroup | 1 (PAGE-103) | M04 | NOT IN REPO | sp-convention |
| sp_Save_GroupInfo | @CodeStr, @EntryBy, @GroupName, @IsActive, @MasterId, @Name, @NationalId, @PageName, @UpdateBy, @id | GroupSetupDal.Save_DepartmentInfo | 2 (PAGE-046, PAGE-105) | M03,M04 | NOT IN REPO | sp-convention |
| sp_Save_GroupWisePromoQty | @EmpInfoId, @EntryBy, @MIOId, @Month, @ProductId, @PromoGroupId, @Qty, @TerritoryId, @Year | GroupWisePromoQtyDAL.SaveDetail | 1 (PAGE-120) | M04 | NOT IN REPO | sp-convention |
| sp_Save_LeaveConfigDtl | @CountEmployeeHoliday, @CountGovtLeave, @DayNameId, @DaysPerMonthly, @EligbleforProbationEmployee, @EmployeeId, @EntryBy, @IsActive, @Joinin | BonusCampaignNewDAL.SaveLeaveConfig | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Save_LeaveConfigFroenEmp | @CountEmployeeHoliday, @CountGovtLeave, @DayNameId, @DaysPerMonthly, @EligbleforProbationEmployee, @EmployeeId, @EntryBy, @IsActive, @Joinin | BonusCampaignNewDAL.SaveLeaveConfig | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Save_LeaveConfigMaster | @CountEmployeeHoliday, @CountGovtLeave, @DayNameId, @DaysPerMonthly, @EligbleforProbationEmployee, @EmployeeId, @EntryBy, @IsActive, @Joinin | BonusCampaignNewDAL.SaveLeaveConfig | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ManualOrderDetail | @ISGiftProduct, @IsCampaignProduct, @NetAmount, @OrderId, @ProductCode, @ProductId, @ProductName, @Quantity, @TotalTradePrice, @TotalVatAmou | OrderListDAL.SaveOrderDetail | 1 (PAGE-221) | M06 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1375'] | sp-convention |
| sp_Save_ManualOrderMaster | @ComUnitCode, @ComUnitId, @ComUnitName, @CustomerCode, @CustomerName, @FixedCustomer, @GrossValue, @MIOCode, @MIOId, @MIOName, @ManufacId, @ | OrderListDAL.SaveOrderMaster | 1 (PAGE-221) | M06 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:939'] | sp-convention |
| sp_Save_ManualRationSetupCampDetail | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignDetailId, @Camp | BonusCampaignNewDAL.SaveBonusCampaign_final | 1 (PAGE-112) | M04 | NOT IN REPO | sp-convention |
| sp_Save_MarketData | @MarketId, @MasterId, @Name, @PageName, @StationTypeId, @SubTerritoryId, @ThanaId, @UserRoleID, @acInAcDate, @createdBy, @id, @isActive, @re | Setup2DAL.SaveMarket | 1 (PAGE-042) | M03 | NOT IN REPO | sp-convention |
| sp_Save_MarketPropDetail | @AMStationType, @ConvertType, @DZSMStationType, @DistrictName, @DivisionName, @EntryDate, @MIOStationType, @MarketCode, @MarketName, @Market | MarketUploadDAL.SaveMarketInfoForExcel; MarketUploadDAL.SaveMarketUploadDetail | 1 (PAGE-098) | M03 | NOT IN REPO | sp-convention |
| sp_Save_MarketPropMaster | @AMStationType, @ConvertType, @DZSMStationType, @DistrictName, @DivisionName, @EntryDate, @MIOStationType, @MarketCode, @MarketName, @Market | MarketUploadDAL.SaveMarketInfoForExcel; MarketUploadDAL.SaveMarketUploadMaster | 1 (PAGE-098) | M03 | NOT IN REPO | sp-convention |
| sp_Save_MarketPropToTable | @EntryBy, @EntryDate, @MarketPropMasterId | MarketUploadDAL.SaveMarketUploadTransfer; OrderTrackingDAL.SaveMarket_ApplogDAL | 2 (PAGE-097, PAGE-098) | M03 | NOT IN REPO | sp-convention |
| sp_Save_MarketStationDetail | @MarketId, @MasterId, @Name, @PageName, @StationTypeId, @SubTerritoryId, @ThanaId, @UserRoleID, @acInAcDate, @createdBy, @id, @isActive, @re | Setup2DAL.SaveMarket | 1 (PAGE-042) | M03 | NOT IN REPO | sp-convention |
| sp_Save_MenuRole | @Add, @Delete, @Edit, @Permission, @RoleId, @SL, @View | MenuDAL.SaveMenuRole | 1 (PAGE-027) | M02 | NOT IN REPO | sp-convention |
| sp_Save_MileageClaim | @AreaId, @EmpInfoId, @EntryBy, @GroupId, @MarketId, @MeterReading, @MileageDate, @RegionId, @SubTerritoryId, @TerritoryId, @TourTypeId, @Tra | Setup2DAL.Save_MileageClaim | 1 (PAGE-581) | M15 | NOT IN REPO | sp-convention |
| sp_Save_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Insert_Update_MIOInfoDAL; RSMSetupDal.Save_MIOInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Save_MonthlyAllowance | @EmpInfoId, @EntryBy, @IsActive, @MasterId, @MonthlyAllowance, @MonthlyAllowanceId, @MonthlyAllowanceName, @Name, @PageName, @UpdateBy, @Use | Setup2DAL.SaveMonthlyAllowance | 1 (PAGE-583) | M15 | NOT IN REPO | sp-convention |
| sp_Save_MonthlyAllowanceDetail | @EmpInfoId, @EntryBy, @IsActive, @MasterId, @MonthlyAllowance, @MonthlyAllowanceId, @MonthlyAllowanceName, @Name, @PageName, @UpdateBy, @Use | Setup2DAL.SaveMonthlyAllowance | 1 (PAGE-583) | M15 | NOT IN REPO | sp-convention |
| sp_Save_MultipleProductAddCampCampDetail | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignDetailId, @Camp | BonusCampaignNewDAL.SaveBonusCampaign_final | 1 (PAGE-112) | M04 | NOT IN REPO | sp-convention |
| sp_Save_NationaltargetSetup | @Amount, @EntryBy, @GroupId, @Month, @NatargetSpId | NationalTargetSetupDal.SaveNationalTargetSetup | 2 (PAGE-541, PAGE-542) | M14 | NOT IN REPO | sp-convention |
| sp_Save_Notice_MarketDetail | @Announcement, @AreaId, @EntryBy, @FromDate, @GroupId, @MarketId, @NoticeId, @NoticeTitle, @RegionId, @SubTerritoryId, @TerritoryId, @ToDate | NoticeDal.SaveNotice | 1 (PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_Save_NoticeImage | @ImageName, @ImagePath, @NoticeId, @NoticeImageId | BonusCampaignNewDAL.SaveImage; BonusCampaignNewDAL.SaveNoticeImage; NoticeDal.SaveImage (+1) | 0 | — | NOT IN REPO | sp-convention |
| sp_Save_NoticeMaster | @Announcement, @AreaId, @EntryBy, @FromDate, @GroupId, @MarketId, @NoticeId, @NoticeTitle, @RegionId, @SubTerritoryId, @TerritoryId, @ToDate | NoticeDal.SaveNotice | 1 (PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_Save_NoticeUserRoleDetail | @Announcement, @AreaId, @EntryBy, @FromDate, @GroupId, @MarketId, @NoticeId, @NoticeTitle, @RegionId, @SubTerritoryId, @TerritoryId, @ToDate | NoticeDal.SaveNotice | 1 (PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_Save_NSMHeadInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_NSMHeadInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_Save_NSMInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_NSMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Save_OrderPermission | @EntryBy, @FrmDate, @PermittedEmpId, @TerritoryId, @ToDate | Setup2DAL.Save_OrderPermission | 1 (PAGE-017) | M02 | NOT IN REPO | sp-convention |
| sp_Save_PrescriptionMaster | @ChemberId, @DoctorId, @EntryBy, @ImageName, @ImagePath, @PrescriptionDate, @PrescriptionId, @PrescriptionTypeId, @ProductId, @UpdateBy | Setup2DAL.Save_Prescription; SetupDAL.Save_Prescription; SetupDAL.Save_PrescriptionMaster | 1 (PAGE-653) | M16 | NOT IN REPO | sp-convention |
| sp_Save_PrescriptionProductDetail | @ChemberId, @DoctorId, @EntryBy, @ImageName, @PrescriptionDate, @PrescriptionId, @PrescriptionTypeId, @ProductId, @UpdateBy | Setup2DAL.Save_Prescription; SetupDAL.Save_Prescription; SetupDAL.Save_PrescriptionDetail | 1 (PAGE-653) | M16 | NOT IN REPO | sp-convention |
| sp_Save_PrescriptionType | @Activedate, @EntryBy, @IsActive, @PrescriptionType, @PrescriptionTypeId, @UpdateBy, PrescriptionType, PrescriptionTypeId | PrescriptionTypeDal.Save_PrescriptionType; Setup2DAL.SavePrescription; SetupDAL.SavePrescription | 2 (PAGE-649, PAGE-655) | M16 | NOT IN REPO | sp-convention |
| sp_Save_ProductDCDetails | @ComUnitId, @ProductId | CustomerInfoDAL.SaveProductDist; MarketStructureTransferDAL.SaveProductDist | 1 (PAGE-147) | M04 | NOT IN REPO | sp-convention |
| sp_Save_ProductLine | @EntryBy, @InactiveBy, @InactiveDate, @IsActive, @LineName, @UpdateBy, @id | TherapueticGroupDal.Save_ProductLine | 1 (PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_Save_ProgramTypeInfo | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @UpdateBy, @id | ProgramDal.Save_DepartmentInfo | 2 (PAGE-175, PAGE-177) | M05 | NOT IN REPO | sp-convention |
| sp_Save_PromoMIOTagDetail | @EmpInfoId, @EntryBy, @MIOId, @MIOTagId, @PromoGroupId | PromoMITagDAL.SaveBonusCampaign | 1 (PAGE-125) | M04 | NOT IN REPO | sp-convention |
| sp_Save_PromoMIOTagMaster | @EmpInfoId, @EntryBy, @MIOId, @MIOTagId, @PromoGroupId | PromoMITagDAL.SaveBonusCampaign | 1 (PAGE-125) | M04 | NOT IN REPO | sp-convention |
| sp_Save_QuotedPriceDetail | @ActiveFromDate, @ActiveToDate, @AreaId, @CustomerMasterId, @Description, @EntryBy, @GroupId, @IsCustomerWise, @IsMarketWise, @MarketId, @Po | ExpenseDal.SaveMasterDetals; QuotedPriceSetupDAL.SaveMasterDetals | 1 (PAGE-118) | M04 | NOT IN REPO | sp-convention |
| sp_Save_QuotedPriceMaster | @ActiveFromDate, @ActiveToDate, @AreaId, @CustomerMasterId, @Description, @EntryBy, @GroupId, @IsCustomerWise, @IsMarketWise, @MarketId, @Po | ExpenseDal.SaveMasterDetals; QuotedPriceSetupDAL.SaveMasterDetals | 1 (PAGE-118) | M04 | NOT IN REPO | sp-convention |
| sp_Save_RouteInformationDADetail | @AreaId, @DAAmount, @DAId, @DCId, @Distance, @EntryBy, @GroupId, @IsSubDepo, @MarketId, @Name, @RegionId, @RouteInformationMasterId, @RouteN | RouteInformationDAL.SaveRouteInformation | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Save_RouteInformationMarketDetail | @AreaId, @DAAmount, @DAId, @DCId, @Distance, @EntryBy, @GroupId, @IsSubDepo, @MarketId, @Name, @RegionId, @RouteInformationMasterId, @RouteN | RouteInformationDAL.SaveRouteInformation | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Save_RouteInformationMaster | @AreaId, @DAAmount, @DAId, @DCId, @Distance, @EntryBy, @GroupId, @IsSubDepo, @MarketId, @Name, @RegionId, @RouteInformationMasterId, @RouteN | RouteInformationDAL.SaveRouteInformation | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Save_RouteInformationWeekNameDetails | @AreaId, @DAAmount, @DAId, @DCId, @Distance, @EntryBy, @GroupId, @IsSubDepo, @MarketId, @Name, @RegionId, @RouteInformationMasterId, @RouteN | RouteInformationDAL.SaveRouteInformation | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_Save_RouterDetails | @EntryBy, @RouterMasterId, @RouterName, @UpdateBy, @id | RouterSetup.Save_RouterSetup | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_Save_RouterMaster | @EntryBy, @RouterMasterId, @RouterName, @UpdateBy, @id | RouterSetup.Save_RouterSetup | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_Save_RSMInfo | @CompanyId, @EmployeeId, @RSMId, @RegionId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_RSMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ShiftInfos | @Activedate, @EntryBy, @ShiftInTime, @ShiftOutTime, @ShiftText, @UpdateBy | AttendanceDAL.Save_ShiftInfo | 1 (PAGE-585) | M15 | NOT IN REPO | sp-convention |
| sp_Save_SMCTypeInfo | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @SMCType, @UpdateBy, @id | ProgramDal.Save_SMCTypeInfo | 1 (PAGE-175) | M05 | NOT IN REPO | sp-convention |
| sp_Save_StationTypeInfo | @EndTime, @EntryBy, @IsActive, @StartTime, @StationTypeName, @UpdateBy, @id | StationDal.Save_DepartmentInfo | 1 (PAGE-178) | M05 | NOT IN REPO | sp-convention |
| sp_Save_SubMarketData | @Name, @acInAcDate, @createdBy, @id, @isActive, @marketId, @remarks | Setup2DAL.SaveSubMarket | 0 | — | NOT IN REPO | sp-convention |
| sp_Save_SubTerritoryInfo | @MasterId, @Name, @PageName, @TerritoryId, @acInAcDate, @createdBy, @id, @isActive | Setup2DAL.SaveSubTerritory | 2 (PAGE-057, PAGE-679) | M03,M17 | NOT IN REPO | sp-convention |
| sp_Save_TadaClaimMaster | @AreaId, @DAAmount, @EmpInfoId, @GroupId, @HotelName, @HotelPhone, @MarketId, @RegionId, @Remarks, @SubTerritoryId, @TadaDate, @TadaID, @Ter | Setup2DAL.SaveTADADA | 1 (PAGE-587) | M15 | NOT IN REPO | sp-convention |
| sp_Save_TADAMarketRulesConfig | @AreaId, @DAAmount, @EntryBy, @GroupId, @IsActive, @IsBoth, @IsMarketWise, @IsRoleWise, @MarketId, @TAAmount, @TADAMarketRuleConfigId, @Terr | Setup2DAL.SaveTADAMarketRuleConfiguration | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Save_TerritoryInfo | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaId, @createdBy, @id, @isActive, @remarks | Setup2DAL.SaveTerritory | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Save_TerritoryWiseTargetSetup | @Amount, @AreaId, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @TerritoryId, @TerritoryWTSetupId, @Year | AreaWiseTargetSetupDal.SaveTerritoryWiseTarget | 2 (PAGE-544, PAGE-545) | M14 | NOT IN REPO | sp-convention |
| sp_Save_ThanaInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id | ThanaDal.SaveThanaInfo | 2 (PAGE-090, PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_Save_TherapeuticGroup | @EntryBy, @InactiveBy, @InactiveDate, @IsActive, @TherapeuticGroupName, @UpdateBy, @id | TherapueticGroupDal.Save_TherapueticGroup | 2 (PAGE-107, PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_Save_TopSheetGenReportCode | @DeliveryMan, @EntryBy | SeedDataDAL.Save_TopSheetGenReportCodeInfo | 1 (PAGE-277) | M07 | NOT IN REPO | sp-convention |
| sp_Save_TourPurpose | @AMAmount, @Activedate, @DZSMAmount, @EntryBy, @IsActive, @IsMarketVisit, @IsOtherVisit, @MIOAmount, @TPId, @TPName, @UpdateBy | DoctorVisitDAL.SaveTourPurpose; TourTypeDal.SaveTourPurpose | 2 (PAGE-597, PAGE-598) | M15 | NOT IN REPO | sp-convention |
| sp_Save_TourSetupEmployee | @CountNo, @EmpInfoId, @EntryBy, @EntryDate, @IsEmployeeWise, @IsRoleWise, @RoleTypeId, @StationTypeId, @TourSetupEmployeeId, @UpdateBy, @Upd | TourSetupForEmployeeDAL.SaveDAInfo | 2 (PAGE-616, PAGE-617) | M15 | NOT IN REPO | sp-convention |
| sp_Save_TourType | @Activedate, @EntryBy, @IsActive, @TourTypeId, @TourTypeName, @UpdateBy | DoctorVisitDAL.SaveTourType; TourTypeDal.SaveTourType | 1 (PAGE-600) | M15 | NOT IN REPO | sp-convention |
| sp_Save_TrainingMarketDetail | @AreaId, @Description, @EntryBy, @FromDate, @GroupId, @IsActive, @MarketId, @RegionId, @SubTerritoryId, @TerritoryId, @Title, @ToDate, @Trai | Setup2DAL.Save_Trainning | 1 (PAGE-601) | M15 | NOT IN REPO | sp-convention |
| sp_Save_TrainingUserRoleDetail | @AreaId, @Description, @EntryBy, @FromDate, @GroupId, @IsActive, @MarketId, @RegionId, @SubTerritoryId, @TerritoryId, @Title, @ToDate, @Trai | Setup2DAL.Save_Trainning | 1 (PAGE-601) | M15 | NOT IN REPO | sp-convention |
| sp_Save_Trainning | @AreaId, @Description, @EntryBy, @FromDate, @GroupId, @IsActive, @MarketId, @RegionId, @SubTerritoryId, @TerritoryId, @Title, @ToDate, @Trai | Setup2DAL.Save_Trainning | 1 (PAGE-601) | M15 | NOT IN REPO | sp-convention |
| sp_Save_Transport | @AllowedMilagePerKM, @EntryBy, @IsActive, @MasterId, @PageName, @TransportId, @TransportName, @UpdateBy | Setup2DAL.SaveTransport; SetupDAL.SaveTransport | 1 (PAGE-605) | M15 | NOT IN REPO | sp-convention |
| sp_Save_UserCompanyUnit | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_Save_UserMarketDetail | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_Save_UserMaster_New | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_Save_UserRoleInfo | @RoleName, @RoleTypeId, @UserRoleID, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_UserRoleInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Save_ZoneInfo | @CodeStr, @GroupId, @MasterId, @Name, @PageName, @acInAcDate, @createdBy, @id, @isActive, @remarks, @zoneId, @zoneName | ZoneSetupDAL.SaveZoneInfo | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Save_ZoneWiseTargetSetup | @Amount, @EntryBy, @GroupId, @Month, @RegionId, @TargetAmount, @Year, @ZoneWTSetupId | ZoneWiseTargetSetupDal.SaveZoneWiseTarget | 2 (PAGE-546, PAGE-547) | M14 | NOT IN REPO | sp-convention |
| sp_SaveLeaveAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveLeave_ApplogDAL | 1 (PAGE-702) | M18 | NOT IN REPO | sp-convention |
| sp_StockInMIGOtoCentralStore | @MigoMasterID_In | ExcelUpForCustTagChangeDAL.TransfarMigobyID_DAL; ExcelUpForMIGODAL.TransfarMigobyID_DAL | 1 (PAGE-689) | M17 | NOT IN REPO | sp-convention |
| sp_SubdeportAreawiseDailyOpeningClosingStockNational | @fromDate, @toDate | AreaWiseMonthlyInventoryReportDal.SubdeportGetAreaWiseMonthlyInventoryNationalReport | 1 (PAGE-427) | M12 | NOT IN REPO | sp-convention |
| sp_SubdeportDeliveryConformationFull | @InvoiceNo, @UpdateBy, @UpdateDate | InvoiceDAL.SubdeportSaveFullInvoice; ReturnInvoiceDAL.SubdeportSaveFullInvoice; SalesReturnDAL.SubdeportSaveFullInvoice | 1 (PAGE-444) | M12 | NOT IN REPO | sp-convention |
| sp_SubDeportDeliveryConformationReject | @InvoiceNo, @ReturnReason, @UpdateBy, @UpdateDate | InvoiceDAL.SubSaveRejectInvoice; ReturnInvoiceDAL.SubSaveRejectInvoice; SalesReturnDAL.SubSaveRejectInvoice | 1 (PAGE-444) | M12 | NOT IN REPO | sp-convention |
| sp_UD_ApiCustomerMaster | @Addrees2, @Address, @ApiCustomerId, @AreaCode, @CellNo, @City, @ComUnitCode, @ComUnitName, @ConPerson, @CustomerName, @DZSMName, @DisCode,  | ImportedApiCustomerDal.UpdateApiCustomerInformation | 1 (PAGE-687) | M17 | NOT IN REPO | sp-convention |
| sp_UD_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.InsertUpdate_ASMInfoDAL; RSMSetupDal.Save_ASMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_UD_CustomerTypeInfo | @CustomerCategoryId, @CustomerType, @EntryBy, @IsActive, @IsCampaign, @IsDefault, @UpdateBy, @id | CustomerTypeDal.Save_DepartmentInfo | 1 (PAGE-171) | M05 | NOT IN REPO | sp-convention |
| sp_UD_DcStockOutApproval | @ApprovedBy, @ApprovedDate, @DcStockOutMasterId, @Status | DeStockOutDal.UpdateStockOutMasterDataForApprovalDal | 2 (PAGE-363, PAGE-700) | M10,M18 | NOT IN REPO | sp-convention |
| sp_UD_DepartmentInfo | @DepartmentName, @EntryBy, @IsActive, @MasterId, @PageName, @UpdateBy, @id | DepartmentDal.Save_DepartmentInfo | 1 (PAGE-566) | M15 | NOT IN REPO | sp-convention |
| sp_UD_DepotWiseArea | @AreaId, @DepotId, @DepotWiseAreaId, @EntryBy, @UpdateBy | DepotWiseAreaSetupDal.DepotWiseAreaInfo | 1 (PAGE-035) | M03 | NOT IN REPO | sp-convention |
| sp_UD_DesignationInfo | @DesigName, @EntryBy, @IsActive, @MasterId, @PageName, @UpdateBy, @id | DesignationDal.Save_DesignationInfo | 1 (PAGE-568) | M15 | NOT IN REPO | sp-convention |
| sp_UD_DistictInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id | ThanaDal.Save_DistictInfo | 1 (PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_UD_FinancialYearInfo | @EntryBy, @IsActive, @UpdateBy, @YearFromDate, @YearTodate, @id | FinancialYearDal.Save_DepartmentInfo | 1 (PAGE-039) | M03 | NOT IN REPO | sp-convention |
| sp_UD_FixedCustomer | @code | FixedCustomerUploadDal.UpdateFixedCustomer | 1 (PAGE-200) | M05 | NOT IN REPO | sp-convention |
| sp_UD_GenericGroup | @EntryBy, @GenericGroupName, @InactiveBy, @InactiveDate, @IsActive, @MasterId, @PageName, @UpdateBy, @id | GenericGroupDal.Save_GenericGroup | 1 (PAGE-103) | M04 | NOT IN REPO | sp-convention |
| sp_UD_GroupInfo | @CodeStr, @EntryBy, @GroupName, @IsActive, @MasterId, @Name, @NationalId, @PageName, @UpdateBy, @id | GroupSetupDal.Save_DepartmentInfo | 2 (PAGE-046, PAGE-105) | M03,M04 | NOT IN REPO | sp-convention |
| sp_UD_Insert_ASMInfo | @ASMId, @AreaId, @CompanyId, @EmployeeId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.InsertUpdate_ASMInfoDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_UD_Insert_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Insert_Update_MIOInfoDAL | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_UD_MIOInfo | @CompanyId, @EmployeeId, @MIOId, @TerritoryId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Insert_Update_MIOInfoDAL; RSMSetupDal.Save_MIOInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_UD_NSMHeadInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_NSMHeadInfo | 0 | — | NOT IN REPO | sp-convention |
| sp_UD_NSMInfo | @CompanyId, @EmployeeId, @GroupId, @NSMId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_NSMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_UD_ProductLine | @EntryBy, @InactiveBy, @InactiveDate, @IsActive, @LineName, @UpdateBy, @id | TherapueticGroupDal.Save_ProductLine | 1 (PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_UD_ProgramTypeInfo | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @UpdateBy, @id | ProgramDal.Save_DepartmentInfo | 2 (PAGE-175, PAGE-177) | M05 | NOT IN REPO | sp-convention |
| sp_UD_RegularCustomer | @code | FixedCustomerUploadDal.UpdateRegularCustomer | 1 (PAGE-200) | M05 | NOT IN REPO | sp-convention |
| sp_UD_RouteInformationMaster | @AreaId, @DAAmount, @DAId, @DCId, @Distance, @EntryBy, @GroupId, @IsSubDepo, @MarketId, @Name, @RegionId, @RouteInformationMasterId, @RouteN | RouteInformationDAL.SaveRouteInformation | 1 (PAGE-062) | M03 | NOT IN REPO | sp-convention |
| sp_UD_RSMInfo | @CompanyId, @EmployeeId, @RSMId, @RegionId, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_RSMInfo | 3 (PAGE-574, PAGE-613, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_UD_SMCTypeInfo | @EntryBy, @IsActive, @IsCustomer, @IsDefault, @IsDoctor, @ProgramTypeName, @SMCType, @UpdateBy, @id | ProgramDal.Save_SMCTypeInfo | 1 (PAGE-175) | M05 | NOT IN REPO | sp-convention |
| sp_UD_StationTypeInfo | @EndTime, @EntryBy, @IsActive, @StartTime, @StationTypeName, @UpdateBy, @id | StationDal.Save_DepartmentInfo | 1 (PAGE-178) | M05 | NOT IN REPO | sp-convention |
| sp_UD_StockBatch_new | @StockQty, @batch, @dcStoreId, @expDate, @mfgdate, @updateBy | StockBatchUpdateDal.UpdateStockBatch | 1 (PAGE-380) | M10 | NOT IN REPO | sp-convention |
| sp_UD_SubDcStockOutApproval | @ApprovedBy, @ApprovedDate, @Status, @SubDcStockOutMasterId | SubDepotStockAdjustmentsVoucherDal.UpdateStockOutMasterDataForApprovalDal | 2 (PAGE-443, PAGE-460) | M12 | NOT IN REPO | sp-convention |
| sp_UD_ThanaInfo | @CreatedBy, @Name, @ThanaName, @UpdateBy, @district_id, @id | ThanaDal.SaveThanaInfo | 2 (PAGE-090, PAGE-094) | M03 | NOT IN REPO | sp-convention |
| sp_UD_TherapeuticGroup | @EntryBy, @InactiveBy, @InactiveDate, @IsActive, @TherapeuticGroupName, @UpdateBy, @id | TherapueticGroupDal.Save_TherapueticGroup | 2 (PAGE-107, PAGE-109) | M04 | NOT IN REPO | sp-convention |
| sp_UD_UserRoleInfo | @RoleName, @RoleTypeId, @UserRoleID, @acInAcDate, @entryBy, @isActive | RSMSetupDal.Save_UserRoleInfo | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Up_LeaveConfigMaster | @CountEmployeeHoliday, @CountGovtLeave, @DayNameId, @DaysPerMonthly, @EligbleforProbationEmployee, @EmployeeId, @EntryBy, @IsActive, @Joinin | BonusCampaignNewDAL.SaveLeaveConfig | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_UP_LoadingSummary | @InvoiceId, @LoadingSummaryStatus, @UpdateBy | InvoiceDAL.UP_LoadingSummaryInvoice; SalesReturnDAL.UP_LoadingSummaryInvoice | 1 (PAGE-271) | M07 | NOT IN REPO | sp-convention |
| sp_UP_LoadingSummaryFinal | @InvoiceId, @LoadingSummaryStatus, @UpdateBy | InvoiceDAL.UP_LoadingSummaryInvoice_Complete; SalesReturnDAL.UP_LoadingSummaryInvoice_Complete | 0 | — | NOT IN REPO | sp-convention |
| sp_Up_ProviderDropoutIntrigrationApprove | @ApproveBy, @providerIDropoutIntrigrationd | SAP_IntrigationPointDAL.ApproveProviderDropoutIntrigrationDAL | 1 (PAGE-694) | M17 | ['Library.DAL/SAP_IntegrationDAL/sql/sp_Up_ProviderDropoutIntrigrationApprove.sql:6'] | sp-convention |
| sp_Update_AreaInfo | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaName, @createdBy, @id, @isActive, @remarks, @zoneId | Setup2DAL.SaveAreaInfo | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Update_ASMActiveStatus | @ASMId, @InactiveBy | RSMSetupDal.Inactive_ASMInfoById | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Update_BonusCampaignNewMaster | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusTypeId, @CampaignCategoryId, @CampaignDetailId, @CampaignMasterId, @CampaignName, @ | BonusCampaignNewDAL.SaveBonusCampaign | 5 (PAGE-111, PAGE-113, PAGE-114, PAGE-115…) | M04 | NOT IN REPO | sp-convention |
| sp_Update_BonusCampaignNewMaster_final | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusQuantity_ManualRationSetup, @BonusTypeId, @BounsProductId, @CampaignDetailId, @Camp | BonusCampaignNewDAL.SaveBonusCampaign_final | 1 (PAGE-112) | M04 | NOT IN REPO | sp-convention |
| sp_Update_BonusCampaignpkCampaignSetupId | @Amount, @AreaId, @BonusProductId, @BonusQuantity, @BonusTypeId, @CampaignCategoryId, @CampaignDetailId, @CampaignMasterId, @CampaignName, @ | BonusCampaignNewDAL.SaveBonusCampaign | 5 (PAGE-111, PAGE-113, PAGE-114, PAGE-115…) | M04 | NOT IN REPO | sp-convention |
| sp_Update_Customer_Doctor_Transfer | @ApprovedBy, @DCID, @MarketId, @MasterId, @RouteId, @Type | CustomerInfoDAL.UpdateCustomer_Doctor_Transfer; MarketStructureTransferDAL.UpdateCustomer_Doctor_Transfer; OrderTrackingDAL.UpdateCustomer_Doctor_Transfer | 1 (PAGE-668) | M16 | NOT IN REPO | sp-convention |
| sp_Update_Customer_Doctor_TransferApprove | @ApprovedBy, @DCID, @MarketId, @MasterId, @RouteId, @Type | CustomerInfoDAL.UpdateCustomer_Doctor_TransferApprove | 1 (PAGE-700) | M18 | NOT IN REPO | sp-convention |
| sp_Update_Customer_Doctor_TransferApproveNew | @ApprovedBy, @MasterId, @Type | OrderTrackingDAL.CustomerDocTransferApprovalDAL | 1 (PAGE-202) | M05 | NOT IN REPO | sp-convention |
| sp_Update_CustomerInfoForMarketData | @MarketId, @MasterId, @Name, @PageName, @StationTypeId, @SubTerritoryId, @ThanaId, @UserRoleID, @acInAcDate, @createdBy, @id, @isActive, @re | Setup2DAL.SaveMarket | 1 (PAGE-042) | M03 | NOT IN REPO | sp-convention |
| sp_Update_CustomerMaster | @Address, @BCDS, @CellNo, @ComUnitId, @CustomerMasterId, @CustomerName, @CustomerType, @CustomerTypeId, @DZSMStationTypeId, @DistributionRou | CustomerInfoDAL.SaveInfo; MarketStructureTransferDAL.SaveInfo; OrderTrackingDAL.SaveInfo | 1 (PAGE-168) | M05 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:549'] | sp-convention |
| sp_Update_CustomerProgramType | @CustomerMasterId, @ProgramTypeCode, @ProgramTypeId, @UpdateBy, @UpdateDate | CustomerInfoDAL.Update__ProgramTypeInfo; MarketStructureTransferDAL.Update__ProgramTypeInfo | 1 (PAGE-167) | M05 | NOT IN REPO | sp-convention |
| sp_Update_CustPropUpdate | @CustPropMasterId, @UpdateBy | OrderTrackingDAL.SaveCustomerProvider_ApplogDAL | 1 (PAGE-204) | M05 | NOT IN REPO | sp-convention |
| sp_Update_DAInfo | @ActiveDate, @Address, @ComUnitId, @DAId, @EmergencyContactNo, @EntryBy, @EntryDate, @InactiveDate, @IsActive, @JoiningDate, @NID, @Name, @P | DAInfoDal.SaveDAInfo | 1 (PAGE-237) | M07 | NOT IN REPO | sp-convention |
| sp_Update_DcWiseTerritoryMaster | @AreaId, @DCId, @DcWiseTerritoryMasterId, @EntryBy, @GroupId, @RegionId, @SubDepotId, @TerritoryId, @UpdateBy | TerritoryWiseDepotSetupDAL.SaveBonusCampaign | 1 (PAGE-064) | M03 | NOT IN REPO | sp-convention |
| sp_Update_DoctorCategory | @Activedate, @CategoryName, @EntryBy, @UpdateBy | DoctorCategoryDal.SaveDoctorCategory; Setup2DAL.SaveDoctorCategory | 1 (PAGE-639) | M16 | NOT IN REPO | sp-convention |
| sp_Update_DoctorChamber | @Activedate, @ChamberName, @EntryBy, @UpdateBy | DoctorChamberDal.SaveDoctorChamber; Setup2DAL.SaveDoctorChamber | 1 (PAGE-637) | M16 | NOT IN REPO | sp-convention |
| sp_Update_DoctorDegree | @Activedate, @DegreeName, @DoctorTypeId, @EntryBy, @UpdateBy | DoctorDegreeDal.SaveDoctorDegree; Setup2DAL.SaveDoctorDegree | 1 (PAGE-641) | M16 | NOT IN REPO | sp-convention |
| sp_Update_DoctorDesignation | @Activedate, @DesignationName, @EntryBy, @UpdateBy | DoctorDesignationDal.SaveDoctorDesignation; Setup2DAL.SaveDoctorDesignation | 1 (PAGE-643) | M16 | NOT IN REPO | sp-convention |
| sp_Update_DoctorMaster | @Activedate, @Address, @BrandId, @ChamberTypeId, @Contact, @ContactType, @ContactTypeId, @DegId, @DesignationId, @DistrictId, @DivisionId, @ | DoctorDAL.SaveInfo | 1 (PAGE-669) | M16 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:734'] | sp-convention |
| sp_Update_DoctorPatientType | @Activedate, @EntryBy, @IsActive, @PatientType, @PatientTypeId, @UpdateBy | PatientTypeDal.SaveDoctorpatientType; Setup2DAL.SaveDoctorpatientType | 1 (PAGE-647) | M16 | NOT IN REPO | sp-convention |
| sp_Update_DoctorPropUpdate | @CustPropMasterId, @UpdateBy | OrderTrackingDAL.SaveDoctorProvider_ApplogDAL | 1 (PAGE-206) | M05 | NOT IN REPO | sp-convention |
| sp_Update_DoctorSpecialDay | @Activedate, @EntryBy, @IsActive, @SpecialDay, @SpecialDayId, @UpdateBy | DoctorSpecailDayDal.SaveDoctorSpeacialDay; Setup2DAL.SaveDoctorSpeacialDay | 1 (PAGE-651) | M16 | NOT IN REPO | sp-convention |
| sp_Update_DoctorSpeciality | @Activedate, @EntryBy, @IsActive, @SpecialityId, @SpecialityName, @UpdateBy | DoctorSpecialityDal.SaveDoctorSpeaciality; Setup2DAL.SaveDoctorSpeaciality | 1 (PAGE-645) | M16 | NOT IN REPO | sp-convention |
| sp_Update_Employee_Holiday | @DayName, @EntryBy, @FiscalYear, @HolidayDate, @HolidayId, @HolidayToDate, @IsActive, @Name, @UpdateBy, @id | HolidayDal.Save_LeaveInfo | 1 (PAGE-575) | M15 | NOT IN REPO | sp-convention |
| sp_Update_Employee_Leave_info | @EntryBy, @IsActive, @LeaveDays, @LeaveTypeId, @LeaveTypeName, @Name, @UpdateBy, @id | LeaveDal.Save_LeaveInfo | 2 (PAGE-577, PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Update_Employee_ShiftInfos | @Activedate, @EntryBy, @ShiftInTime, @ShiftOutTime, @ShiftText, @UpdateBy | AttendanceDAL.Save_ShiftInfo | 1 (PAGE-585) | M15 | NOT IN REPO | sp-convention |
| sp_Update_EmployeeInformation | @AddressPermanent, @AddressPresent, @AllowanceId, @BloodGroup, @CellNumber, @CompanyId, @CompanyUnitId, @DateOfBirth, @DepartmentId, @Design | EmployeeInformationDaL.SaveEmployeeInformation | 3 (PAGE-613, PAGE-614, PAGE-615) | M15 | NOT IN REPO | sp-convention |
| sp_Update_ExpenseClaim | @Amount, @EmpInfoId, @EntryBy, @ExpenseClaimID, @ExpenseDate, @ExpenseTypDetailsId, @ExpenseTypeId, @Remarks, @UpdateBy, @ValueText | Setup2DAL.Save_ExpenseClaim; SetupDAL.Save_ExpenseClaim | 1 (PAGE-570) | M15 | NOT IN REPO | sp-convention |
| sp_Update_ExpenseType | @EmpNameMult, @EntryBy, @ExpenseAmount, @ExpenseTypDetailsId, @ExpenseTypeId, @ExpenseTypeName, @FieldName, @ImageRequired, @IsActive, @IsRe | Setup2DAL.SaveExpenseTypeMaster; SetupDAL.SaveExpenseTypeMaster | 1 (PAGE-572) | M15 | NOT IN REPO | sp-convention |
| sp_Update_ExpenseTypeDetails | @ExpenseTypDetailsId, @ExpenseTypeId, @FieldName, @IsRequied | SetupDAL.SaveExpenseTypeDetails | 0 | — | NOT IN REPO | sp-convention |
| sp_Update_InvoiceFinalPayment | @FinalPaymentBy, @InvoiceId, @PaymentAmount, @ptStatus | CustPaymentDAL.UpdateInvoiceFinalPayment | 5 (PAGE-311, PAGE-312, PAGE-314, PAGE-315…) | M09 | NOT IN REPO | sp-convention |
| sp_Update_ManualOrderMaster | @ComUnitCode, @ComUnitId, @ComUnitName, @CustomerCode, @CustomerName, @FixedCustomer, @GrossValue, @MIOCode, @MIOId, @MIOName, @OrderId, @Re | OrderListDAL.UpdateOrderMaster | 1 (PAGE-221) | M06 | ['LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql:1114'] | sp-convention |
| sp_Update_MarketData | @MarketId, @MasterId, @Name, @PageName, @StationTypeId, @SubTerritoryId, @ThanaId, @UserRoleID, @acInAcDate, @createdBy, @id, @isActive, @re | Setup2DAL.SaveMarket | 1 (PAGE-042) | M03 | NOT IN REPO | sp-convention |
| sp_Update_MarketStructure_Approve | @ApprovedBy, @MasterId, @Type | MarketStructureTransferDAL.UpdateMarketStructure_TransferApprove | 1 (PAGE-099) | M03 | NOT IN REPO | sp-convention |
| sp_Update_MarketStructure_Transfer | @EntryBy, @FAreaId, @FGroupId, @FRegionId, @FSubTerritoryId, @FTerritoryId, @TAreaId, @TGroupId, @TMarketId, @TRegionId, @TSubTerritoryId, @ | MarketStructureTransferDAL.UpdateArea_Transfer; MarketStructureTransferDAL.UpdateMArket_Transfer; MarketStructureTransferDAL.UpdateSubTerritory_Transfer (+2) | 5 (PAGE-095, PAGE-096, PAGE-100, PAGE-101…) | M03 | NOT IN REPO | sp-convention |
| sp_Update_MileageClaim | @AreaId, @EmpInfoId, @EntryBy, @GroupId, @MarketId, @MeterReading, @MileageDate, @RegionId, @SubTerritoryId, @TerritoryId, @TourTypeId, @Tra | Setup2DAL.Save_MileageClaim | 1 (PAGE-581) | M15 | NOT IN REPO | sp-convention |
| sp_Update_MIOActiveStatus | @InactiveBy, @MIOId | RSMSetupDal.Inactive_MIOInfoById | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Update_MonthlyAllowance | @EmpInfoId, @EntryBy, @IsActive, @MasterId, @MonthlyAllowance, @MonthlyAllowanceId, @MonthlyAllowanceName, @Name, @PageName, @UpdateBy, @Use | Setup2DAL.SaveMonthlyAllowance | 1 (PAGE-583) | M15 | NOT IN REPO | sp-convention |
| sp_Update_NoticeMaster | @Announcement, @AreaId, @EntryBy, @FromDate, @GroupId, @MarketId, @NoticeId, @NoticeTitle, @RegionId, @SubTerritoryId, @TerritoryId, @ToDate | NoticeDal.SaveNotice | 1 (PAGE-621) | M15 | NOT IN REPO | sp-convention |
| sp_Update_OrderDC | @ApprovedBy, @DCID, @OrdId, @RouteNameId | OrderTrackingDAL.UpdateOrderDC | 1 (PAGE-207) | M06 | NOT IN REPO | sp-convention |
| sp_Update_PrescriptionMaster | @ChemberId, @DoctorId, @EntryBy, @ImageName, @PrescriptionDate, @PrescriptionId, @PrescriptionTypeId, @ProductId, @UpdateBy | Setup2DAL.Save_Prescription; SetupDAL.Save_Prescription | 1 (PAGE-653) | M16 | NOT IN REPO | sp-convention |
| sp_Update_PrescriptionType | @Activedate, @EntryBy, @IsActive, @PrescriptionType, @PrescriptionTypeId, @UpdateBy, PrescriptionType, PrescriptionTypeId | PrescriptionTypeDal.Save_PrescriptionType; Setup2DAL.SavePrescription; SetupDAL.SavePrescription | 2 (PAGE-649, PAGE-655) | M16 | NOT IN REPO | sp-convention |
| sp_Update_PromoEmployeeQty | @CountNo, @TourSetupEmployeeId, @UpdateBy, @UpdateDate | GroupWisePromoQtyDAL.Update___Info | 1 (PAGE-122) | M04 | NOT IN REPO | sp-convention |
| sp_Update_PromoMIOTagMaster | @EmpInfoId, @EntryBy, @MIOId, @MIOTagId, @PromoGroupId | PromoMITagDAL.SaveBonusCampaign | 1 (PAGE-125) | M04 | NOT IN REPO | sp-convention |
| sp_Update_QuotedPriceMaster | @ActiveFromDate, @ActiveToDate, @AreaId, @CustomerMasterId, @Description, @EntryBy, @GroupId, @IsCustomerWise, @IsMarketWise, @MarketId, @Po | ExpenseDal.SaveMasterDetals; QuotedPriceSetupDAL.SaveMasterDetals | 1 (PAGE-118) | M04 | NOT IN REPO | sp-convention |
| sp_Update_RouterMaster | @EntryBy, @RouterMasterId, @RouterName, @UpdateBy, @id | RouterSetup.Save_RouterSetup | 1 (PAGE-052) | M03 | NOT IN REPO | sp-convention |
| sp_Update_RSMActiveStatus | @InactiveBy, @RSMId | RSMSetupDal.Inactive_RSMInfoById | 1 (PAGE-574) | M15 | NOT IN REPO | sp-convention |
| sp_Update_SubMarketData | @Name, @acInAcDate, @createdBy, @id, @isActive, @marketId, @remarks | Setup2DAL.SaveSubMarket | 0 | — | NOT IN REPO | sp-convention |
| sp_Update_SubTerritoryData | @MasterId, @Name, @PageName, @TerritoryId, @acInAcDate, @createdBy, @id, @isActive | Setup2DAL.SaveSubTerritory | 2 (PAGE-057, PAGE-679) | M03,M17 | NOT IN REPO | sp-convention |
| sp_Update_TadaClaimMaster | @AreaId, @DAAmount, @EmpInfoId, @GroupId, @HotelName, @HotelPhone, @MarketId, @RegionId, @Remarks, @SubTerritoryId, @TadaDate, @TadaID, @Ter | Setup2DAL.SaveTADADA | 1 (PAGE-587) | M15 | NOT IN REPO | sp-convention |
| sp_Update_TADAMarketRulesConfig | @AreaId, @DAAmount, @EntryBy, @GroupId, @IsActive, @IsBoth, @IsMarketWise, @IsRoleWise, @MarketId, @TAAmount, @TADAMarketRuleConfigId, @Terr | Setup2DAL.SaveTADAMarketRuleConfiguration | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Update_TargetInfo | @EmpId, @FYId, @MonthName, @SL, @UpdateBy, @Value, @YearValue | BonusCampaignNewDAL.SaveTargetEdit | 1 (PAGE-555) | M14 | NOT IN REPO | sp-convention |
| sp_Update_TerritoryData | @CodeStr, @MasterId, @Name, @PageName, @acInAcDate, @areaId, @createdBy, @id, @isActive, @remarks | Setup2DAL.SaveTerritory | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_Update_TourPurpose | @AMAmount, @Activedate, @DZSMAmount, @EntryBy, @IsActive, @IsMarketVisit, @IsOtherVisit, @MIOAmount, @TPId, @TPName, @UpdateBy | DoctorVisitDAL.SaveTourPurpose; TourTypeDal.SaveTourPurpose | 2 (PAGE-597, PAGE-598) | M15 | NOT IN REPO | sp-convention |
| sp_Update_TourSetupEmployee | @CountNo, @EmpInfoId, @EntryBy, @EntryDate, @IsEmployeeWise, @IsRoleWise, @RoleTypeId, @StationTypeId, @TourSetupEmployeeId, @UpdateBy, @Upd | TourSetupForEmployeeDAL.SaveDAInfo | 2 (PAGE-616, PAGE-617) | M15 | NOT IN REPO | sp-convention |
| sp_Update_TourType | @Activedate, @EntryBy, @IsActive, @TourTypeId, @TourTypeName, @UpdateBy | DoctorVisitDAL.SaveTourType; TourTypeDal.SaveTourType | 1 (PAGE-600) | M15 | NOT IN REPO | sp-convention |
| sp_Update_Trainning | @AreaId, @Description, @EntryBy, @FromDate, @GroupId, @IsActive, @MarketId, @RegionId, @SubTerritoryId, @TerritoryId, @Title, @ToDate, @Trai | Setup2DAL.Save_Trainning | 1 (PAGE-601) | M15 | NOT IN REPO | sp-convention |
| sp_Update_Transport | @AllowedMilagePerKM, @EntryBy, @IsActive, @MasterId, @PageName, @TransportId, @TransportName, @UpdateBy | Setup2DAL.SaveTransport; SetupDAL.SaveTransport | 1 (PAGE-605) | M15 | NOT IN REPO | sp-convention |
| sp_Update_UserMaster_new | @ActiveInActiveDate, @AreaId, @ComUnitId, @EmpInfoId, @EntryBy, @GroupId, @IMEI_One, @IMEI_Two, @IsAppsUser, @IsDepotDashboard, @IsMainDashb | UserInfoDAL.SaveUserInfo | 4 (PAGE-015, PAGE-016, PAGE-613, PAGE-615) | M02,M15 | NOT IN REPO | sp-convention |
| sp_UpDate_UserSettingPanel | @FromDate, @Todate, @UserSettingPanelId | UserSettingPanelDal.SaveUserPanelSettting | 1 (PAGE-025) | M02 | NOT IN REPO | sp-convention |
| sp_Update_ZoneInfo | @CodeStr, @GroupId, @MasterId, @Name, @PageName, @acInAcDate, @createdBy, @id, @isActive, @remarks, @zoneId, @zoneName | ZoneSetupDAL.SaveZoneInfo | 1 (PAGE-679) | M17 | NOT IN REPO | sp-convention |
| sp_UpdateAndInsertInvoiceDetailSalesReturn | @BonusQuantity, @DeliveryStatus, @DiscountAmount, @DiscountPercentage, @InvoiceDetailId, @InvoiceId, @NetAmount, @PreviousQuantity, @Quantit | SalesReturnDAL.InsertInvoiceDetailReturn | 1 (PAGE-298) | M08 | ['SP_SalesReturn_Related.sql:173'] | sp-convention |
| sp_UpdateBacktoReturnPage | @LoginName, @OrdID | CustomerMasterDAL.UpdateBacktoReturnPage | 1 (PAGE-284) | M08 | NOT IN REPO | sp-convention |
| sp_Upsertdate_EmpInfo | @ActionStatus, @RoleType, @UpdateBy, @employee_code, @employee_id | SAP_IntrigationPointDAL.SaveEmpInfoDAL | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_Upsertdate_ProductInfo | @UpdateBy, @product_code, @product_id, @status | SAP_IntrigationPointDAL.SaveProductInfoDAL | 2 (PAGE-680, PAGE-681) | M17 | NOT IN REPO | sp-convention |
| sp_VerifyCustomer | @CustomerMasterExcelFileMasterID | ExcelUpForMIGODAL.VerifyCustomerDAL | 1 (PAGE-193) | M05 | NOT IN REPO | sp-convention |
| sp_VerifyCustomerTagList | @CustomerTagChangeExcelFileMasterID | ExcelUpForCustTagChangeDAL.VerifyCustomerDAL | 1 (PAGE-198) | M05 | NOT IN REPO | sp-convention |
| sp_web_SaveExpanseAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveExpenseClaim_ApplogDAL | 1 (PAGE-701) | M18 | NOT IN REPO | sp-convention |
| sp_web_SaveMileageAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveMileageClaim_Applog_DAL | 1 (PAGE-703) | M18 | NOT IN REPO | sp-convention |
| sp_web_SaveTADAAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveTADAClaim_ApplogDAL | 1 (PAGE-696) | M18 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_CustomerApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | MarketStructureTransferDAL.GetCustomer_ApplogDAL; OrderTrackingDAL.GetCustomer_ApplogDAL | 0 | — | NOT IN REPO | sp-convention |
| sp_Webapi_Get_DayName | — | CommonDataLoad.Get_DayName | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_DoctorClaimApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | DoctorDAL.GetDoctorApprovalList | 0 | — | NOT IN REPO | sp-convention |
| sp_Webapi_Get_EmpAllawance_MonthYear | @EmpInfoId, @Month, @year | ExpenseDal.Get_EmpAllawance | 1 (PAGE-628) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_ImagePath | @type | AppPrimaryDAL.GetImagePath | 4 (PAGE-570, PAGE-581, PAGE-621, PAGE-653) | M15,M16 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_Leave_AppLog | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | MarketStructureTransferDAL.GetLeave_AppLogDAL | 0 | — | NOT IN REPO | sp-convention |
| sp_Webapi_Get_LeaveApproveById | @id | LeaveApplicationDal.IsEditable | 1 (PAGE-608) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_LeaveConType | — | CommonDataLoad.Get_LeaveConType | 1 (PAGE-578) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_OrderApp | @AppStatus, @EmpId, @FromDt, @Role, @ToDt, @param | CustomerInfoDAL.GetOrderList_ApplogDAL; MarketStructureTransferDAL.GetOrderList_ApplogDAL | 1 (PAGE-704) | M18 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_TourPlanBalance | @EmpInfoId, @Month, @year | TourTypeDal.Get_TourPlanBalanceDAL | 1 (PAGE-591) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_TourPlanBalanceEmp | @EmpInfoId, @Month, @year | ExpenseDal.GetGet_TourPlanBalanceMasterById | 1 (PAGE-628) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Get_TourPlanBalanceWithEmpInfo | @EmpInfoId, @Month, @Year | EmployeeInformationDaL.GetTourPlanReportBal | 2 (PAGE-626, PAGE-629) | M15 | NOT IN REPO | sp-convention |
| sp_WebApi_GetVersion | — | AppPrimaryDAL.GetActiveVersion | 0 | — | NOT IN REPO | sp-convention |
| sp_Webapi_LeaveReport_Details | @Parm, @Parm2, @Year | CmnCrystaltoView.GetEmployee_YearlyLeaveBalanceRptList | 1 (PAGE-622) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_LeaveReport_Summary | @Parm, @Parm2, @Year | CmnCrystaltoView.GetEmployee_YearlySummaryList | 1 (PAGE-622) | M15 | NOT IN REPO | sp-convention |
| sp_Webapi_Save_LeaveInfo | @empId, @endDate, @leaveAppId, @reason, @startDate, @typeId | LeaveApplicationDal.SaveLeaveApplication | 1 (PAGE-608) | M15 | NOT IN REPO | sp-convention |
| sp_webapi_SaveAppLog | @ApprovalId, @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryBy | CustomerInfoDAL.SaveAttandaceAppLog | 1 (PAGE-564) | M15 | NOT IN REPO | sp-convention |
| sp_webapi_SaveCustomerAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @CustomerApprovalId, @Date,  | CustomerInfoDAL.SaveCustomer_ApplogDAL; MarketStructureTransferDAL.SaveCustomer_ApplogDAL; OrderTrackingDAL.SaveCustomer_ApplogDAL | 1 (PAGE-695) | M18 | NOT IN REPO | sp-convention |
| sp_webapi_SaveDCRAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @DCRApprovalId, @Date, @Entr | CustomerInfoDAL.SaveDCR_Applog_DAL | 2 (PAGE-658, PAGE-698) | M16,M18 | NOT IN REPO | sp-convention |
| sp_webapi_SaveDoctorAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @DoctorApprovalId, @E | DoctorDAL.SaveDoctor_ApplogDAL | 1 (PAGE-699) | M18 | NOT IN REPO | sp-convention |
| sp_webapi_SaveLeaveAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | MarketStructureTransferDAL.SaveLeave_ApplogDAL | 0 | — | NOT IN REPO | sp-convention |
| sp_webapi_SaveOrderAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveOrder_ApplogDAL; MarketStructureTransferDAL.SaveOrder_ApplogDAL | 1 (PAGE-704) | M18 | NOT IN REPO | sp-convention |
| sp_webapi_SavePrescriptionAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveRX_Applog_DAL | 1 (PAGE-705) | M18 | NOT IN REPO | sp-convention |
| sp_webapi_SaveTourPlanAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveTourPlan_ApplogDAL | 1 (PAGE-706) | M18 | NOT IN REPO | sp-convention |
| sp_webapi_SaveVisitPlanAppLog | @ApproveByApp, @ApproveByS, @ApproveDateApp, @ApproveDateS, @ApproveTimeApp, @ApproveTimeS, @AreaId, @Comments, @Date, @EntryByApp, @EntryBy | CustomerInfoDAL.SaveVisitTourPlan_ApplogDAL | 1 (PAGE-697) | M18 | NOT IN REPO | sp-convention |
| sp_Webapi_Update_LeaveData | @approvalType, @empId, @id | LeaveApplicationDal.ApproveLeaveApplication | 1 (PAGE-608) | M15 | NOT IN REPO | sp-convention |
| sp_WHBeenCard | @fromDate, @toDate | CWMonthlyInventoryReportDal.GetCWMonthlyInventoryReport | 1 (PAGE-346) | M10 | NOT IN REPO | sp-convention |
| spDiscDataTable | — | SInventory_RPTVIEW_CompanySalesReportViewer.Page_Init | 1 (PAGE-126) | M04 | NOT IN REPO | sp-convention |
| spInsertReturnInvoice_new | @AreaCode, @ComUnitId, @Createdate, @CustomerMasterId, @DZSMName, @DeductionCharge, @DisCode, @DpMob, @DpNAme, @FEName, @FixedCustomer, @Inv | BonusCampaignNewDAL.SaveReturnSalesMaster | 1 (PAGE-294) | M08 | ['Live_Deployment_Script_All_Updates.sql:857', 'SP_SalesReturn_Related.sql:18'] | sp-convention |
| spInsertTourPurposeOtherSetup | @AreaId, @EntryBy, @GroupId, @IsActive, @RegionId, @RoleName, @TerritoryId, @TourPurposeId, @TourPurposeOtherSetupId, @TourTypeId, @VisitTyp | SetupDAL.SaveTPOtherMaster | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| spInsertTourPurposeOtherSetupDtl | @AreaId, @EntryBy, @GroupId, @IsActive, @RegionId, @RoleName, @TerritoryId, @TourPurposeId, @TourPurposeOtherSetupId, @TourTypeId, @VisitTyp | SetupDAL.SaveTPOtherMaster | 1 (PAGE-595) | M15 | NOT IN REPO | sp-convention |
| tbl_INV_CentralStoreStoIssue | @StockInTransfarId | RequisitionDAL.ProcessWeightedAvg | 1 (PAGE-259) | M07 | ['SP_Fix_CentralStoreStoIssue.sql:11', 'SP_Fix_CentralStoreStoIssue_Corrected.sql:15'] | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| UD_CustomerMaster | @Addrees2, @Address, @AreaCode, @CategoryId, @CellNo, @City, @ComUnitCode, @ComUnitName, @ConPerson, @CustomerCode, @CustomerMasterId, @Cust | CustomerMasterInfoDal.UpdateCustomerMasterInfo | 1 (PAGE-185) | M05 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| UD_CustomerMaster2 | @Addrees2, @Address, @AreaCode, @CategoryId, @CellNo, @City, @ComUnitCode, @ComUnitName, @CustomerCode, @CustomerMasterId, @CustomerName, @D | CustomerMasterDAL.UpdateCustomerMasterInfo | 1 (PAGE-194) | M05 | NOT IN REPO | NON-CONVENTIONAL name (verify: may be view/function/DB name or false positive) |
| usp_CheckCampaignEntryDate | — | BonusCampaignNewDAL.GetMobileSyncStatus | 2 (PAGE-116, PAGE-173) | M04,M05 | NOT IN REPO | sp-convention |
| usp_InsertTerritoryData | @EntryBy, @FinYearValue, @MonthName, @TerritoryCode, @Value, @YearValue | CompanywisebranchDal.SaveTargetValueInfo | 1 (PAGE-554) | M14 | NOT IN REPO | sp-convention |

<!-- GENERATED-INDEX-END: sp -->

