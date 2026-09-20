# Database Inventory

## What is (and is not) in the repository

- There is **no `database/` directory** and **no schema dump**. The only SQL in the repository is: 17 ad-hoc deployment/fix scripts at the repo root and 3 scripts in `Library.DAL/SAP_IntegrationDAL/sql/` — see below.
- Database objects were therefore reverse-engineered from the **C# data-access code**: 1034 distinct stored-procedure names are referenced (1005 follow the `sp_*` convention), 237 table/view names appear in inline SQL.
- Stored procedures whose bodies are present in the repo: 41 (see `docs/database/stored-procedures.md`). For every other SP only the call interface (name, SqlParameter names, callers, pages) is knowable.
- No `CREATE TABLE` (other than `#temp` tables), `CREATE VIEW`, `CREATE FUNCTION`, `CREATE TRIGGER`, PK/FK/constraint or index definitions exist in the repository except: `CREATE INDEX IX_tblUnitPriceRollback_ProductId` (SP_Create_tblUnitPriceRollback.sql). Counts for functions and triggers are therefore **0 verified / UNKNOWN actual**.
- Databases named in code: `SalesDisDB_Unicorn_DB` (main; `DataBase.SalesDB`), cross-database references `CSTL_ACCDB_Unicorn` (accounts), `CSTL_ERPDB_Unicorn`, `SalesDisDB_SMC_NEWDB` (legacy name), and an archive DB configured at runtime via `tblArcDBConnect` (`ArchiveDbConnectRepository.cs`).

## SQL scripts present

| Script | Bytes | Stored procedures created/altered | Tables altered |
|---|---|---|---|
| Create_Order_ODR-20260901134.sql | 4592 | — | — |
| LIVE_DB_UPDATE_2026-08-30_TO_2026-08-31.sql | 62713 | sp_Get_CustMasterList_Approve, sp_Get_CustomerApp, sp_Get_DoctorList, sp_LoadOrderListForOrderCreationbyTerri, sp_Report_DCStore_StockValuationLedger, sp_Save_CustomerMaster, sp_Save_DoctorMaster, sp_Save_ManualOrderDetail, sp_Save_ManualOrderMaster, sp_UD_CustomerMaster, sp_Update_CustomerMaster, sp_Update_DoctorMaster, sp_Update_ManualOrderMaster | tblCustMaster, tblDoctorMaster, tblSalesReturnDetail, tblTempSalesReturnOrder |
| Library.DAL/SAP_IntegrationDAL/sql/alter_tblProviderDropoutIntrigration_AddApprovalColumns.sql | 599 | — | tblProviderDropoutIntrigration |
| Library.DAL/SAP_IntegrationDAL/sql/sp_Get_ProviderDropoutIntrigrationList.sql | 733 | sp_Get_ProviderDropoutIntrigrationList | — |
| Library.DAL/SAP_IntegrationDAL/sql/sp_Up_ProviderDropoutIntrigrationApprove.sql | 630 | sp_Up_ProviderDropoutIntrigrationApprove | — |
| Live_Deployment_Script_All_Updates.sql | 55035 | spInsertReturnInvoice_new, sp_DeliverySalesReturnPosting, sp_GET_PaymentInvSP, sp_GET_PaymentInvSPPaymentAmount, sp_GET_PaymentInvSPTPVATAmt, sp_Report_CustomerInvoiceAgingOutstanding, sp_SalesReturnPosting, sp_Sales_CustomerPaymentPosting | tblCustomerReturnMaster, tblDCStore, tblReturnInvoice |
| SP_Adjust_DCStore_Stock_Only.sql | 6007 | sp_Adjust_DCStore_Stock_Only | — |
| SP_Audit_Sales_Accounts_Voucher_Posting.sql | 21341 | sp_Audit_Sales_Accounts_Voucher_Posting | — |
| SP_Create_sp_Get_MoneyReceiptReportAfterPaymentList.sql | 2785 | sp_Get_MoneyReceiptReportAfterPaymentList | — |
| SP_Create_tblUnitPriceRollback.sql | 1022 | — | tblUnitPriceRollback |
| SP_Fix_CentralStoreStoIssue.sql | 6954 | tbl_INV_CentralStoreStoIssue | — |
| SP_Fix_CentralStoreStoIssue_Corrected.sql | 7540 | tbl_INV_CentralStoreStoIssue | — |
| SP_Fix_CustomerPayment_Live.sql | 769 | — | tblCustomerPay |
| SP_Fix_Invoice_Anomaly_And_Sync.sql | 2590 | sp_Check_anomalyInvoiceDetails | — |
| SP_Fix_STO_Chalan_Process.sql | 2407 | sp_Del_B2BDelete | — |
| SP_NewStockTransferDcToDc_Related.sql | 15882 | sp_Del_B2BDelete, sp_DeleteDwpotToWhChalan, sp_SAP_B2BChallanDetailInsert, sp_SAP_B2BChallanMasterInsert, sp_SAP_B2BStockReceiveByDc, sp_SAP_ChallanSendDetailByChalanId, sp_SAP_Up_SAP_ChallanConfirmByChalanId, sp_SAP_Up_SAP_ChallanSendByChalanId | — |
| SP_SalesReturn_Related.sql | 6665 | spInsertReturnInvoice_new, sp_UpdateAndInsertInvoiceDetailSalesReturn | — |
| SP_Update_StockValuation_TransactionDate_Logic.sql | 22402 | sp_Report_DCStore_StockValuationLedger, sp_Report_NationalStockValuation_BatchDetails, sp_Report_NationalStockValuation_DateRange | — |
| sp_Check_DCStore_StockAnomalies.sql | 4367 | sp_Check_DCStore_StockAnomalies | — |
| sp_Delete_ProformaInvoice_Update.sql | 9221 | sp_Delete_ProformaInvoice | — |

## Tables referenced by inline SQL

| Table | Read by (DAL/page methods) | Written by | Columns observed (INSERT/UPDATE) | Pages (inline-SQL reach) |
|---|---|---|---|---|
| CSTL_ACCDB_Unicorn | 11 | 2 | 0 | 10 |
| CSTL_ERPDB_Unicorn | 5 | 0 | 0 | 1 |
| GetmonthlycustomerSaleDate | 1 | 0 | 0 | 1 |
| InboundLots | 2 | 0 | 0 | 1 |
| OutboundSum | 2 | 0 | 0 | 1 |
| SalesDisDB_SMC_NEWDB | 1 | 0 | 0 | 1 |
| SystemTest_Testing | 1 | 0 | 0 | 1 |
| tblAction | 2 | 0 | 0 | 1 |
| tblActionPageWiseStep | 3 | 0 | 0 | 1 |
| tblActionStepDetail | 3 | 0 | 0 | 1 |
| tblActionSteps | 3 | 0 | 0 | 1 |
| tblActionUserWiseApproval | 3 | 0 | 0 | 1 |
| tblAddressDivision | 1 | 0 | 0 | 2 |
| tblAdjustmentType | 6 | 1 | 1 | 5 |
| tblAppSetup | 5 | 3 | 5 | 6 |
| tblArcDBConnect | 2 | 1 | 2 | 1 |
| tblArea | 49 | 2 | 3 | 47 |
| tblBankInfo | 2 | 0 | 0 | 2 |
| tblBankInfoNew | 1 | 0 | 0 | 1 |
| tblBoardUniversity | 1 | 0 | 0 | 0 |
| tblBranchInfo | 1 | 0 | 0 | 0 |
| tblCentralStore | 46 | 14 | 24 | 30 |
| tblCentralStore_OpeninigBalance | 3 | 0 | 0 | 3 |
| tblChalanDetail | 19 | 2 | 11 | 9 |
| tblChalanInfo | 21 | 10 | 18 | 10 |
| tblCompanyInfo | 32 | 2 | 7 | 76 |
| tblCompanyUnit | 267 | 2 | 7 | 232 |
| tblCompanyWiseDeposit | 1 | 1 | 3 | 1 |
| tblCountry | 1 | 0 | 0 | 0 |
| tblCurrentStock | 7 | 4 | 7 | 2 |
| tblCustCategory | 30 | 2 | 3 | 15 |
| tblCustMaster | 146 | 4 | 25 | 78 |
| tblCustPayDetail | 29 | 5 | 15 | 27 |
| tblCustomerCreditLimit | 7 | 4 | 10 | 6 |
| tblCustomerMasterExcelFileDetail | 3 | 1 | 0 | 2 |
| tblCustomerMasterExcelFileMaster | 3 | 2 | 8 | 2 |
| tblCustomerMasterTagChangeExcelFileDetail | 3 | 2 | 22 | 3 |
| tblCustomerMasterTagChangeExcelFileMaster | 3 | 2 | 8 | 2 |
| tblCustomerPay | 17 | 3 | 12 | 24 |
| tblCustomerReturnDetail | 3 | 1 | 4 | 1 |
| tblCustomerReturnMaster | 3 | 2 | 10 | 1 |
| tblCustomerType | 3 | 0 | 0 | 3 |
| tblCustomertype | 3 | 0 | 0 | 3 |
| tblDAInfo | 11 | 0 | 0 | 11 |
| tblDCPicking | 16 | 4 | 5 | 3 |
| tblDCPickingDetail | 8 | 4 | 3 | 2 |
| tblDCRApprovalLog | 1 | 0 | 0 | 1 |
| tblDCStock | 12 | 0 | 0 | 0 |
| tblDCStore | 83 | 26 | 32 | 49 |
| tblDCStoreFreeze | 10 | 17 | 26 | 17 |
| tblDCStoreTransaction | 1 | 1 | 5 | 1 |
| tblDCStore_OpeningBalance | 2 | 0 | 0 | 1 |
| tblDeStockOutDetails | 4 | 2 | 10 | 3 |
| tblDeStockOutMaster | 6 | 4 | 11 | 7 |
| tblDeliveryManInfo | 3 | 0 | 0 | 2 |
| tblDepartment | 5 | 2 | 3 | 7 |
| tblDepotToWHChalanDetail | 3 | 1 | 13 | 4 |
| tblDepotToWHChalanInfo | 6 | 3 | 17 | 5 |
| tblDesignation | 5 | 0 | 0 | 5 |
| tblDesignationType | 1 | 0 | 0 | 2 |
| tblDistributionRoute | 3 | 0 | 0 | 0 |
| tblDistrict | 22 | 2 | 3 | 26 |
| tblDivision | 1 | 0 | 0 | 2 |
| tblDivisionWing | 1 | 0 | 0 | 2 |
| tblDoctorDegree | 1 | 0 | 0 | 0 |
| tblDoctorDegreeDetail | 2 | 0 | 0 | 1 |
| tblDoctorMaster | 2 | 0 | 0 | 0 |
| tblDoctorProgramType | 1 | 0 | 0 | 0 |
| tblDoctorProgramTypeDetail | 1 | 0 | 0 | 0 |
| tblDoctorSpeciality | 1 | 0 | 0 | 0 |
| tblDoctorSpecialityDetail | 1 | 0 | 0 | 0 |
| tblEducationName | 2 | 0 | 0 | 2 |
| tblEducationSubjectGroup | 1 | 0 | 0 | 0 |
| tblEducationalInstitution | 1 | 0 | 0 | 0 |
| tblEmpCategory | 1 | 0 | 0 | 2 |
| tblEmpChildren | 1 | 0 | 0 | 0 |
| tblEmpEducation | 1 | 0 | 0 | 0 |
| tblEmpExperience | 1 | 0 | 0 | 0 |
| tblEmpGeneralInfo | 35 | 2 | 30 | 30 |
| tblEmpInfoTrainingType | 1 | 0 | 0 | 0 |
| tblEmpReference | 1 | 0 | 0 | 0 |
| tblEmpTraining | 1 | 0 | 0 | 0 |
| tblEmpTrainingInstitution | 1 | 0 | 0 | 0 |
| tblEmployeeType | 1 | 0 | 0 | 2 |
| tblEmpwiseProductLineDeclare | 2 | 1 | 2 | 3 |
| tblEmpwiseShiftDeclare | 2 | 1 | 2 | 3 |
| tblEvaluateTrainingMaster | 1 | 0 | 0 | 0 |
| tblFinancialYear | 1 | 0 | 0 | 1 |
| tblFiscalYearInfos | 1 | 0 | 0 | 1 |
| tblFocDetails | 4 | 0 | 0 | 2 |
| tblFocMaster | 4 | 0 | 0 | 2 |
| tblGenericGroup | 3 | 0 | 0 | 2 |
| tblGroupWisePromoQty | 2 | 2 | 8 | 2 |
| tblIngridents | 6 | 2 | 3 | 6 |
| tblInvoice | 263 | 14 | 70 | 101 |
| tblInvoiceBatch | 14 | 2 | 3 | 6 |
| tblInvoiceDeleteLog | 2 | 2 | 7 | 2 |
| tblInvoiceDetail | 149 | 10 | 46 | 55 |
| tblInvoiceDetailReturn | 1 | 0 | 0 | 1 |
| tblInvoiceDetail_DeleterRecord | 2 | 1 | 8 | 2 |
| tblJobLocation | 1 | 0 | 0 | 2 |
| tblLoginLog | 0 | 1 | 3 | 0 |
| tblMIAInfo | 52 | 2 | 4 | 27 |
| tblMIATarget | 3 | 2 | 6 | 3 |
| tblMIGODetail | 19 | 4 | 24 | 12 |
| tblMIGOMaster | 10 | 4 | 7 | 6 |
| tblMIOInfo | 6 | 0 | 0 | 4 |
| tblMainMenu | 23 | 1 | 4 | 9 |
| tblManufacturer | 42 | 2 | 4 | 62 |
| tblMarket | 83 | 2 | 3 | 46 |
| tblMenuDistribution | 8 | 2 | 4 | 1 |
| tblMonthlyTarget | 4 | 2 | 7 | 2 |
| tblNationality | 1 | 0 | 0 | 2 |
| tblNonTranscationalInvoiceDetail | 1 | 0 | 0 | 0 |
| tblNonTranscationalInvoiceMaster | 2 | 0 | 0 | 0 |
| tblOccupation | 3 | 0 | 0 | 2 |
| tblOfficeBranch | 1 | 0 | 0 | 0 |
| tblOpeningBalanceFinancialYearLog | 1 | 0 | 0 | 1 |
| tblOrder | 104 | 6 | 7 | 44 |
| tblOrderDel | 4 | 0 | 0 | 2 |
| tblOrderDetail | 44 | 5 | 1 | 20 |
| tblOrderDetailDel | 2 | 0 | 0 | 1 |
| tblOrderListDetail | 6 | 2 | 16 | 5 |
| tblOrderListMaster | 6 | 2 | 6 | 4 |
| tblOrder_Doctorrequirement | 3 | 0 | 0 | 1 |
| tblPackSize | 11 | 2 | 1 | 10 |
| tblPaymentType | 33 | 0 | 0 | 25 |
| tblProCategory | 9 | 2 | 3 | 12 |
| tblProType | 6 | 2 | 2 | 8 |
| tblProduct | 194 | 2 | 18 | 110 |
| tblProductCase | 11 | 4 | 4 | 11 |
| tblProductDCDetails | 1 | 0 | 0 | 2 |
| tblProductDiscount | 31 | 2 | 7 | 19 |
| tblProductGroup | 3 | 0 | 0 | 3 |
| tblProductSQ | 11 | 3 | 3 | 17 |
| tblProductWiseSalesTarget | 2 | 2 | 11 | 2 |
| tblProgramType | 3 | 0 | 0 | 3 |
| tblProjectSetup | 1 | 0 | 0 | 2 |
| tblPromoGroup | 6 | 2 | 4 | 6 |
| tblPromoMIOTagDetail | 3 | 2 | 2 | 2 |
| tblPromoMIOTagMaster | 3 | 1 | 3 | 2 |
| tblPurpose | 2 | 1 | 2 | 3 |
| tblRegion | 37 | 2 | 3 | 54 |
| tblRequisition | 40 | 15 | 28 | 27 |
| tblRequsitionChild | 13 | 14 | 17 | 14 |
| tblReturnAmount | 5 | 5 | 4 | 6 |
| tblReturnInvoice | 36 | 7 | 50 | 11 |
| tblReturnInvoiceDetail | 22 | 7 | 36 | 9 |
| tblReturnReason | 8 | 0 | 0 | 4 |
| tblRoleType | 1 | 0 | 0 | 1 |
| tblRouteInformationMarketDetail | 1 | 0 | 0 | 0 |
| tblRouteInformationMaster | 20 | 0 | 0 | 11 |
| tblSMCTrainingVenue | 1 | 0 | 0 | 0 |
| tblSalaryGrade | 1 | 0 | 0 | 2 |
| tblSalaryLocation | 1 | 0 | 0 | 2 |
| tblSalaryStep | 1 | 0 | 0 | 2 |
| tblSalesReturnDetail | 1 | 1 | 7 | 1 |
| tblSampleStockForDcDetails | 2 | 2 | 9 | 2 |
| tblSampleStockForDcMaster | 2 | 2 | 8 | 2 |
| tblSampleStockForWareHouseDetails | 2 | 2 | 9 | 2 |
| tblSampleStockForWareHouseMaster | 2 | 2 | 8 | 2 |
| tblSection | 1 | 0 | 0 | 2 |
| tblSpecialization | 1 | 0 | 0 | 0 |
| tblStockCondition | 4 | 0 | 0 | 4 |
| tblStockConditionFreeze | 0 | 2 | 7 | 1 |
| tblStockConditionPermission | 5 | 2 | 3 | 3 |
| tblStockInTransfar | 42 | 7 | 19 | 22 |
| tblStockUOM | 11 | 2 | 2 | 16 |
| tblSubDepoStockAdjustDetail | 0 | 1 | 3 | 1 |
| tblSubDepoStockAdjustment | 0 | 1 | 4 | 1 |
| tblSubDepot | 20 | 2 | 7 | 13 |
| tblSubDepotChalanDetail | 6 | 2 | 11 | 6 |
| tblSubDepotChalanInfo | 9 | 4 | 16 | 6 |
| tblSubDepotChalanRetuenDetail | 2 | 1 | 11 | 2 |
| tblSubDepotChalanReturnInfo | 5 | 3 | 16 | 3 |
| tblSubDepotStockOutDetails | 2 | 2 | 10 | 3 |
| tblSubDepotStockOutMaster | 4 | 2 | 9 | 5 |
| tblSubDepotStore | 19 | 7 | 1 | 13 |
| tblSubDepotStoreFreeze | 4 | 6 | 23 | 3 |
| tblSubInvoiceDetail | 62 | 2 | 1 | 28 |
| tblSubInvoiceMaster | 101 | 4 | 3 | 40 |
| tblSubInvoiceMasterBatch | 4 | 0 | 0 | 1 |
| tblSubSection | 1 | 0 | 0 | 2 |
| tblSubTerritory | 4 | 0 | 0 | 2 |
| tblSupplierInformation | 6 | 2 | 8 | 4 |
| tblTempSalesReturnOrder | 2 | 1 | 14 | 1 |
| tblTerritory | 9 | 0 | 0 | 7 |
| tblThana | 1 | 0 | 0 | 2 |
| tblTherapeuticGroup | 1 | 0 | 0 | 1 |
| tblTradePolicy | 3 | 0 | 0 | 4 |
| tblTradePolicyNew | 3 | 0 | 0 | 7 |
| tblTradePolicyNew2 | 3 | 0 | 0 | 7 |
| tblTrainingOrgInfo | 1 | 0 | 0 | 0 |
| tblTrainingRecordMaster | 1 | 0 | 0 | 0 |
| tblTrainingType | 1 | 0 | 0 | 0 |
| tblUnitPrice | 61 | 5 | 13 | 44 |
| tblUnitPriceRollback | 0 | 1 | 15 | 1 |
| tblUnitPriceUpdate | 0 | 1 | 11 | 0 |
| tblUnitPrice_Old | 1 | 0 | 0 | 1 |
| tblUser | 53 | 5 | 12 | 29 |
| tblUserCompanyUnit | 16 | 3 | 5 | 129 |
| tblUserSessionTracking | 0 | 4 | 13 | 1 |
| tblUserType | 3 | 0 | 0 | 5 |
| tblWHAdjustmentDetail | 3 | 2 | 3 | 3 |
| tblWHStockAdjustment | 6 | 2 | 10 | 3 |
| tblWHStockInDetail | 20 | 4 | 10 | 12 |
| tblWHStockInMaster | 15 | 6 | 19 | 10 |
| tblWHStockOutDetail | 0 | 1 | 5 | 1 |
| tblWHStockOutMaster | 1 | 1 | 8 | 1 |
| tblWearhouse | 15 | 0 | 0 | 20 |
| tblWhStockConditionFreeze | 0 | 1 | 6 | 2 |
| tblWhStoreFreeze | 4 | 2 | 14 | 5 |
| tblZone | 6 | 2 | 5 | 4 |
| tbl_BonusCampaignNewDetail | 3 | 0 | 0 | 0 |
| tbl_BonusCampaignNewMaster | 3 | 0 | 0 | 0 |
| tbl_DCRInfo | 1 | 0 | 0 | 1 |
| tbl_Group | 4 | 0 | 0 | 2 |
| tbl_INV_CentralStore_WeightedAvg | 7 | 1 | 0 | 8 |
| tbl_INV_Central_BatchWiseStock | 3 | 1 | 0 | 2 |
| tbl_MC_ProductRawMaterialDetails | 2 | 0 | 0 | 1 |
| tbl_PrescriptionType | 1 | 0 | 0 | 0 |
| tbl_UserRoleInfo | 3 | 0 | 0 | 5 |
| tbl_trainingRecordDetailsEmployee | 1 | 0 | 0 | 0 |
| tblarea | 1 | 0 | 0 | 1 |
| tblmarket | 2 | 0 | 0 | 1 |
| tblregion | 3 | 0 | 0 | 1 |
| tbltempCustMaster | 4 | 1 | 1 | 2 |
| tblunitprice | 1 | 0 | 0 | 1 |

## Views referenced

| View | Read by (methods) |
|---|---|
| VIew_SubDepotCurrentStock | 1 |
| View_CentralStoreCurrentStock | 2 |
| View_CustomerForModification | 1 |
| View_CustomerMaster | 117 |
| View_DCStoreCurrentStock | 2 |
| View_MIAWiseSalesReport | 7 |
| View_OrderCustomerInfo | 2 |
| View_ProDCStock | 1 |
| View_TotalCurrentStockofCompanyWithStockInTransfar | 1 |

## Report datasets (typed DataSets, `Library.CrystalReports/SInventory_DS/*.xsd`)

| XSD | DataTable | # columns | Columns |
|---|---|---|---|
| ActiveUnitPriceDetailDataTable.xsd | ActiveUnitPriceDetailDataTable | 10 | ProductCode, ProductName, PackSize, CostPrice, UnitPrice, VATPercentage, VATAmountPerUnit, IsActive, ActiveDate, InActiveDate |
| ActiveUnitPriceDetailDataTable.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| AreaWiseMonthlyInventoryReport.xsd | AreaWiseMonthlyInventoryReportDataTable | 21 | ProductCode, ProductName, BaseUnit, OpeningStock, ReceiveFromCentralWarehouse, ReceiveFromAreaOfficeInterTransfer, TotalReceived, IssuedToSales, IssuedToProductBonus, IssuedToAreaOfficeInterTransfer, IssuedToCentralWareh |
| BranchWiseDetailSales.xsd | Bsales1 | 7 | ComUnitCode, ComUnitName, UpdateDate, NumberofInvoiceSold, SumofNetSalesAmount, DataColumn1, DataColumn2 |
| CWMonthlyInventoryReport.xsd | CWMonthlyInventoryReportDataTable | 25 | ProductCode, ProductName, BaseUnit, OpeningStock, ReceiveFromProduction, Barishal, Bogura, Chottogram, Cumilla, Dhaka_West, Khulna, Kushtia, Mymensingh, Rajshahi, Rangpur, Sylhet, OtherIsueSample, OtherIsueDamage, TotalI |
| DayWiseBusinessDataSet.xsd | DayWiseBusinessDataTable | 34 | SalesManCode, SalesManName, Designation, _x0031_, _x0032_, _x0033_, _x0034_, _x0035_, _x0036_, _x0037_, _x0038_, _x0039_, _x0031_0, _x0031_1, _x0031_2, _x0031_3, _x0031_4, _x0031_5, _x0031_6, _x0031_7, _x0031_8, _x0031_9 |
| DcOpeningStockReportDataTable.xsd | dcOpeningStockReportDataTable | 9 | ComUnitCode, ComUnitName, ProductCode, ProductName, PackSize, BatchNo, MfgDate, ExpDate, StockQty |
| DcOpeningStockReportDataTable.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| DcStockMonitoringDataSet.xsd | DCStockMonitoringDataTable | 30 | TotalSale, TotalValue, DistributionCenterCode, DistributionCenter, Total, Quantity, Actifast_x0020_Tablet_x007E_10mg, Dolwin_x0020_Tablet_x007E_500mg, Esomium_x0020_20_x0020_Capsule, Esomium_x0020_40_x0020_Capsule, Ezeve |
| DeleteOrderDataTable.xsd | DeleteOrderDataTable | 17 | OrderCode, SalesCenterCode, SalesCenterName, MIOCode, MIOName, TeritoryName, TeritoryCode, FECode, FEName, DZSMCode, DZSMName, CustomerCode, CustomerName, GrossValue, SubmissionDate, DeleteBy, DeleteDate |
| DeleteOrderDataTable.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| FixedCustomerSalesReportDataSet.xsd | FixedCustomerSalesReport | 24 | January, February, March, April, May, June, July, August, September, October, November, December, CustomerMasterId, CustomerCode, CustomerName, Address, TerritoryCode, FECode, MIOCode, MIOnAme, SalesMonth, Salesyear, TG, |
| HistoricalReportDataSet.xsd | HistoricalReportDataTable | 13 | MIOName, OpeningReceivableInHand, OpeningReceivableMarketOutStanding, OpeningReceivableFromAO, SalesOnTP, SalesVAT, TotalSales, CollectionFromSales, DepositCashAtBank, DepositOthers, ClosingReceivableInHand, ClosingRecei |
| MarketwiseSummary.xsd | MarketwiseSummaryDS | 13 | ComUnitCode, ComUnitName, ProformaDate, NumberofInvoiceSold, ProformaAmountTP, ProformaAmountVAt, MarketName, MarketCode, MIACode, MIAName, AreaCode, RegionCode, DisCode |
| ProductWiseNationalSalesReportDataSet.xsd | ProductWiseNationalSalesReportDataTable | 12 | ProductCode, ProductName, PackSize, DeliveryTotal3, PreviousMonthSaleValue3, DeliveryTotal2, PreviousMonthSaleValue2, DeliveryTotal1, PreviousMonthSaleValue1, DeliveryTotal, CurrentMonthSaleValue, ContributionPercent |
| ProductWiseNationalSalesZoneWiseDataSet.xsd | ProductWiseNationalSalesReportZoneWiseDataTable | 16 | ProductCode, ProductName, PackSize, DeliveryTotal-KL-100, SaleValue-KL-100, DeliveryTotal-BG-100, SaleValue-BG-100, DeliveryTotal-SL-100, SaleValue-SL-100, DeliveryTotal-CM-100, SaleValue-CM-100, DeliveryTotal-DK-100, Sa |
| SalesTrendDataSet.xsd | SalesTrendDataTable | 16 | SalesManCode, SalesManName, Designation, January, February, March, April, May, June, July, August, September, October, November, December, Total |
| WHStockInReportData.xsd | mainDataTable | 22 | WHStockInMasterID, WHStockInCode, ManufacId, WHStockInDate, TotalQuantity, TotalVat, TotalValue, ChallanNo, ChallanDate, ReferenceNo, ReferenceDate, Remarks, Status, EntryBy, EntryDate, UpdateBy, UpdateDate, ApproveBy, A |
| WHStockInReportData.xsd | detailDataTable | 13 | WHStockInDetailID, WHStockInMasterID, ProductCode, ProductName, PackSize, Batch, ExpDate, MfgDate, Qty, Price, VAT, TotalAmount, StockUOMName |
| WHStockInReportData.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| WhStockMonitoringReportDataTable.xsd | WhStockMonitoringDataTable | 10 | ProductCode, ProductName, StockInQty, OpeningStockQty, OpAmount, IssueQty, ClosingBal, saleQty, saleAmount, ClosingAmount |
| dsAllSalesReport.xsd | AllsalesDataTable | 45 | ComUnitCode, ComUnitName, CustomerCode, CustomerName, OrderNo, OrderDate, InvoiceNo, InvoiceDate, DelivaryInvoiceNo, UpdateDate, ProductCode, ProductName, PackSize, BatchNo, ExpDate, DeliveryQuantity, DeliveryNetAmount,  |
| dsAuditReportOne.xsd | AuditReportOneDataTable | 22 | ProductCode, ProductName, Quantity, TradePrice, TotalTradePrice, OrderCode, SalesCenterCode, MIOCode, MIOName, TeritoryName, TeritoryCode, FECode, FEName, DZSMCode, DZSMName, CustomerCode, CustomerName, GrossValue, Submi |
| dsBatchWiseCollection.xsd | CollectionTable | 11 | InvoiceNo, InvoiceDate, BatchNo, BatchCreationDate, PayType, CustomerName, PaidAmount, MRNo, PaymentDate, PaymentRemarks, EmpName |
| dsBusinessSummary.xsd | BsummaryDataTable | 12 | ComUnitCode, ComUnitName, NumberofProformaInvoice, SumofNetProformaAmount, NumberofInvoiceSold, SumofNetSalesAmount, NumberofReturnInvoice, SumofNetReturnAmount, ShortName, ProTpVat, DelTpVat, DelReTpVat |
| dsComUnitSales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsComUnitSales.xsd | comUnitDetailDataTable | 4 | ProductCode, Product, TotalQty, TotalAmount |
| dsComUnitSales.xsd | comUnitMainDataTable | 3 | ComUnitDetail, FromDate, ToDate |
| dsCompanySales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsCompanySales.xsd | comSalesDetailDataTable | 4 | ProductCode, Product, TotalQty, TotalAmount |
| dsCompanySales.xsd | comSalesMainDataTable | 2 | FromDate, ToDate |
| dsCountryStock.xsd | countryDetailDataTable | 4 | ProductCode, ProductName, PackSize, TotalCurrentStockQty |
| dsCountryStock.xsd | countryDetailDataTable2 | 4 | ProductCode, ProductName, PackSize, TotalQty |
| dsCountryStock.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsCountryStock.xsd | countryMainDataTable | 1 | ProductCode |
| dsCustomerLedger.xsd | DataTabledsCustomerLedger | 17 | CustomerCode, CustomerName, OrderNo, OrderDate, InvoiceNo, InvoiceDate, ComUnitCode, ComUnitName, MarketCode, MarketName, TpGrandTotal, DeliveryTpGrandTotal, DelivaryInvoiceNo, UpdateDate, PaymentAmount, PaymentStatus, D |
| dsCustomerMaster.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsCustomerMaster.xsd | custMasterDetailTable | 31 | CustomerCode, CustomerName, Address, Addrees2, City, ConPerson, CellNo, MarketName, AreaName, DZSMName, ComUnitName, RegionName, FEName, DistrictName, CategoryName, MiaName, PaymentType, ShippingCond, MarketCode, MiaCode |
| dsCustomerPaymentReport.xsd | custPaymentDataTable | 25 | CustomerCode, CustomerName, InvoiceNo, InvoiceDate, DelivaryInvoiceNo, DelivaryInvoiceDate, DeliveryTpGrandTotal, PaymentAmount, PaymentStatus, RefNo, PaymentDate, MarketCode, MarketName, AreaCode, MiaCode, DistrictCode, |
| dsCustomerSales.xsd | custMainDataTable | 8 | CustDetail, Address, MarketName, AreaName, DistrictName, ComUnitName, FromDate, ToDate |
| dsCustomerSales.xsd | custDetailDataTable | 4 | ProductCode, Product, Total, TotalPrice |
| dsCustomerSales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsDCPicking.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsDCPicking.xsd | mainDataTable | 7 | DCPicNo, DCPicDate, ComUnitCode, ComUnitName, Address, AreaCode, AreaName |
| dsDCPicking.xsd | detailDataTable | 4 | ProductCode, ProductName, BatchNo, TotalPickQty |
| dsDCStock.xsd | comUnitDetailDataTable | 14 | ComUnitId, ComUnitCode, ComUnitName, ProductCode, ProductName, PackSize, BatchNo, MfgDate, ExpDate, AQty, BookFDel, Tqty, RQty, BQty |
| dsDCwiseCountryStock.xsd | comUnitDetailDataTable | 5 | ProductCode, ProductName, PackSize, TotalQuantity, ComUnitCode |
| dsDCwiseCountryStock.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsDcStockOutReport.xsd | DcStockOutDT | 14 | ComUnitName, InvoiceNo, Reason, StockOutDate, ProductCode, ProductName, BatchNo, StackOutQty, Total, VATAmountPerUnit, UnitPrice, CustomerCode, CustomerName, Address |
| dsDeleteInvoiceReportDataTable.xsd | dsDeleteInvoiceReportDataTable | 32 | InvoiceNo, DelInvoiceNo, DeleteDateTime, DeleteBy, ProductCode, ProductName, PackSize, ReceiveDate, ExpDate, CostPrice, UnitPrice, UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, DataColumn19, DataColu |
| dsDeliveryReturn.xsd | deliveryDataTable | 28 | ComUnitCode, ComUnitName, CustomerCode, CustomerName, OrderNo, OrderDate, InvoiceNo, InvoiceDate, DelivaryInvoiceNo, ProductCode, ProductName, PackSize, BatchNo, ExpDate, Quantity, Amount, VatAmount, AreaCode, MiaCode, D |
| dsDistrictSales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsDistrictSales.xsd | districtMainDataTable | 5 | District, ComUnitName, ZoneName, Fromdate, Todate |
| dsDistrictSales.xsd | districtDetailDataTable | 4 | ProductCode, Product, TotalQty, TotalAmount |
| dsEmployeewiseProductSales.xsd | EmployeewiseProductSalesDataTable | 31 | SalesManCode, SalesManName, SaleAmount, Designation, Discount, vat, NetSales, TDqty, ProductName, Actifast_x0020_Tablet_x007E_10mg, Dolwin_x0020_Tablet_x007E_500mg, Esomium_x0020_20_x0020_Capsule, Esomium_x0020_40_x0020_ |
| dsInvoiceForCustomer.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsInvoiceForCustomer.xsd | mainDataTable | 38 | InvoiceId, InvoiceNo, InvoiceDate, OrderNo, OrderDate, TpTotal, TpVat, TpDiscount, TpGrandTotal, ComUnitCode, CompanyName, CUAddress, CustomerCode, CustomerName, CMAddress, CategoryName, AreaName, PaymentTypeName, MiaCod |
| dsInvoiceForCustomer.xsd | detailDataTable | 15 | ProductCode, Product, BatchNo, BonusQuantity, Quantity, UnitPrice, UnitVatAmount, TotalPrice, TotalPriceVatAmount, DiscountPercentage, DiscountAmount, NetAmount, InvoiceId, ReturnInvoiceId, ExpDate |
| dsInvoiceForCustomer.xsd | dtReceiveable | 8 | InvoiceNo, InvoiceDate, ReturnAmount, CustomerPaymentAmount, ReceivableTotalAmnt, CustomerMasterIdNew, IntransitDay, NetAmountForRecv |
| dsLoadingReport.xsd | LoadingReportdt | 18 | LOADINGREPORTNo, TitleDate, DeliveryPerson, VeichleNo, MarketName, CashCollection, DEPOTName, MarketDues, ItemNo, InvoiceNo, TerritoryCode, InvoiceValue, Collection, PartialRejection, Remarks, UndeliverdInvoice, FullReje |
| dsMIASales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsMIASales.xsd | miaDetailDataTable | 6 | InvoiceDate, MiaId, ProductCode, Product, TotalQuantity, Price |
| dsMIASales.xsd | miaMainDataTable | 6 | MiaId, MIA, District, CompanyUnit, FromDate, ToDate |
| dsMIAWiseSales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsMIAWiseSales.xsd | miaDetailDataTable | 6 | InvoiceDate, MiaId, ProductCode, Product, TotalQuantity, Price |
| dsMIAWiseSales.xsd | miaMainDataTable | 6 | MiaId, MIA, District, CompanyUnit, FromDate, ToDate |
| dsMarketwisePicking.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsMarketwisePicking.xsd | marketwisepickingDataTable | 7 | MarketName, InvoiceDate, ProductCode, ProductName, BatchNo, PackSize, Quantity |
| dsMigo.xsd | migoDataTable | 23 | ShipToParty, PONo, PODate, ItemNo, OrderDocNo, OrderDocDate, DeliveryDocNo, DeliveryDocDate, LMID, LMIDDescription, Batch, ExpDate, MfgDate, Qty, VATChallan, BilltoParty, InvoiceNo, InvoiceDate, CaseNoofShipper, VAT, Amo |
| dsMoneyReceipt.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsMoneyReceipt.xsd | mainDataTable | 14 | OrderNo, CustomerCode, CustomerName, InvoiceNo, InvoiceDate, PaymentNo, PaymentDate, PaymentAmount, ReceiptNo, ComUnitName, Address, OrderBy, TPAmount, VatAmount |
| dsMonthlyExpense.xsd | dtExpense | 16 | EmpMasterCode, EmpName, DesigName, BaseHQ, MarketCode, ZoneCode, MonthYear, DAdate, TourType, daAmount, AllowedMileageInKM, mileageAmount, expenseAmount, totalAmount, MarketName, EmpInfoId |
| dsMonthlyExpense.xsd | dtAllowance | 4 | MonthlyAllowance, MonthlyAllowanceName, EmpInfoId, EmpMasterCode |
| dsMonthlyExpense.xsd | dtStationType | 4 | StationTypeName, Balance, EmpInfoId, EmpMasterCode |
| dsMonthlyExpense.xsd | dtTotal | 2 | EmpMasterCode, totalAmount |
| dsOrderInformation.xsd | orderDetailsDataTable | 15 | SalesCentre, SalesCentreName, TerritoryCode, MIOCode, MIOName, FECode, DataColumn1, OrderCode, SubmissionDate, CustomerID, CustomerName, ProductCode, ProductName, OrderQty, GrossValue |
| dsOrderInformation.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsProductCostPrice.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsProductCostPrice.xsd | unitPriceDetailDataTable | 6 | ProductCode, ProductName, PackSize, CostPrice, VATAmountPerUnit, UnitPrice |
| dsProductTreadPrice.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsProductTreadPrice.xsd | unitPriceDetailDataTable | 5 | UnitPriceId, ProductCode, ProductName, PackSize, UnitPrice |
| dsProforma.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsProforma.xsd | proformaDataTable | 34 | ComUnitCode, ComUnitName, CustomerCode, CustomerName, OrderNo, OrderDate, InvoiceNo, InvoiceDate, ProductCode, ProductName, PackSize, BatchNo, ExpDate, Quantity, NetAmount, TotalPriceVatAmount, DiscountAmount, SpecialAmo |
| dsRegionSales.xsd | regionDetailDataTable | 4 | ProductCode, Product, TotalQty, TotalAmount |
| dsRegionSales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsRegionSales.xsd | regionMainDataTable | 3 | RegionDetail, FromDate, ToDate |
| dsSTO.xsd | StoDataTable | 27 | ComUnitId, ComUnitCode, ComUnitName, ProductCode, ProductName, MfgDate, ExpDate, PackSize, BatchNo, TotalQuantity, UnitPrice, TotalPriceAmount, VATAmountPerUnit, TotalVATAmount, TotalPriceAmountwithVat, ItemNo, OrderDocN |
| dsSalesComparision.xsd | SalesComparisionDataTable | 7 | DistrictCode, DistrictName, PreviousMonthSaleValue, SaleValue, DiffInAmount, DiffInPercent, CurrentMonth |
| dsSalesRejection.xsd | salesRejectionDataTable | 25 | ComUnitCode, ComUnitName, MarketCode, MarketName, CustomerCode, CustomerName, OrderCode, SubmissionDate, ProductCode, ProductName, InvoiceNo, InvoiceDate, RejectQuantity, NetRejectionAmount, DateofRejection, RejectionDes |
| dsStockTransfarOrder.xsd | mainDataTable | 15 | ReqId, ReqNo, ReqDate, IssueChalanNo, IssuChalanDate, WearhouseCode, WearhouseName, WHAddress, ComUnitCode, ComUnitName, CUAddress, TruckNo, DriverName, PickingNo, PickingDate |
| dsStockTransfarOrder.xsd | detailDataTable | 12 | ReqId, ProductCode, ProductName, CaseQty, BatchNo, Quantity, PriceAmount, VATAmount, TotalPriceAmount, PickingQty, ExpDate, StockUOMName |
| dsStockTransfarOrder.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsStockTransfarOrder.xsd | migoDataTable | 23 | ShipToParty, PONo, PODate, ItemNo, OrderDocNo, OrderDocDate, DeliveryDocNo, DeliveryDocDate, LMID, LMIDDescription, Batch, MfgDate, ExpDate, Qty, VATChallan, BilltoParty, InvoiceNo, InvoiceDate, CaseNoofShipper, Amount,  |
| dsStockTransfarOrder.xsd | dcToDcDataTable | 19 | ProductCode, ProductName, Quantity, BatchNo, Value, Vat, ValueWVat, ChalanNo, ChalanDate, FromComUnitCode, FromComUnitName, FromComUnitAddress, ToComUnitName, ToComUnitAddress, ToComUnitCode, TrackNo, DriverName, MfgDate |
| dsStockTransfarOrder.xsd | dcstockDataTable | 18 | ProductCode, ProductName, PackSize, BatchNo, TotalQuantity, ExpDate, ReceiveDate, ChalanNo, ChalanDate, StockQty, DamageQty, StockRcvDate, ComUnitCode, ComUnitName, UnitPrice, VATAmountPerUnit, MfgDate, DataColumn1 |
| dsTopSheet.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsTopSheet.xsd | detailDataTable | 10 | CustomerCode, CustomerName, InvoiceNo, InvoiceDate, TpGrandTotal, MarketName, Address, CellNo, CustomerType, TopSheetGenCode |
| dsTopSheet.xsd | delInvoiceDataTable | 8 | CustomerCode, CustomerName, DelivaryInvoiceNo, InvoiceNo, InvoiceDate, UpdateDate, DeliveryTpGrandTotal, MarketName |
| dsTopSheet.xsd | marketwisepickingDataTable | 7 | MarketName, InvoiceDate, ProductCode, ProductName, BatchNo, PackSize, Quantity |
| dsTourPlan.xsd | dtTPDetail | 1 | ContactNo |
| dsTourPlan.xsd | dtBal | 1 | OrderNo |
| dsTourPlanMonthy.xsd | dtTPDetail | 26 | EmpName, EmpMasterCode, MonthYear, RoleName, PostingPlace, PostingPlaceCode, Zone, Date, Day, MorningStartingPlaceTime, MorningEndingPlaceTime, WorkingTypesM, WorkWithMIOM, MorningWorkingMarkets, EveningStartingPlaceTime |
| dsTourPlanMonthy.xsd | dtBal | 5 | HQ, ExHQ, OS, OSDCC, Total |
| dsZoneSales.xsd | companyInfoDataTable | 4 | CompanyName, Address, ContactNo, FaxNo |
| dsZoneSales.xsd | zoneDetailDataTable | 4 | ProductCode, Product, TotalQty, TotalAmount |
| dsZoneSales.xsd | zoneMainDataTable | 4 | ZoneDetail, ComUnitName, FromDate, ToDate |
| dtDCToWhDataTable.xsd | DCToWhDataTable | 19 | ChalanNo, ChalanDate, TrackNo, DriverName, DCCode, DCName, DCAddress, WHCode, WHName, WHAddress, ProductCode, ProductName, BatchNo, MfgDate, ExpDate, Quantity, Value, Vat, ValueWVat |
| whOpeningStockReportDataTable.xsd | whOpeningStockReportDataTable | 7 | ProductCode, ProductName, PackSize, BatchNo, MfgDate, ExpDate, Quantity |

## Crystal Report files

| Report file | Location | Referenced by page(s) |
|---|---|---|
| crpAllSalesReport.rpt | Library.CrystalReports/SInventory_RPT/crpAllSalesReport.rpt | PAGE-249, PAGE-466, PAGE-481, PAGE-485 |
| crpCWMonthlyInventoryReport.rpt | Library.CrystalReports/SInventory_RPT/crpCWMonthlyInventoryReport.rpt | PAGE-346 |
| crpCustomerMaster.rpt | Library.CrystalReports/SInventory_RPT/crpCustomerMaster.rpt | PAGE-180, PAGE-181 |
| crpDeliveryReturnReport.rpt | Library.CrystalReports/SInventory_RPT/crpDeliveryReturnReport.rpt | PAGE-280 |
| crpHistoricalReport.rpt | Library.CrystalReports/SInventory_RPT/crpHistoricalReport.rpt | PAGE-483 |
| crpMigoDetailsReport.rpt | Library.CrystalReports/SInventory_RPT/crpMigoDetailsReport.rpt | PAGE-684 |
| crpOrderDetailsReport.rpt | Library.CrystalReports/SInventory_RPT/crpOrderDetailsReport.rpt | PAGE-684 |
| rptAllProductCountryStock.rpt | Library.CrystalReports/SInventory_RPT/rptAllProductCountryStock.rpt | none found (unused or loaded dynamically) |
| rptAreaWiseMonthlyInventoryReport.rpt | Library.CrystalReports/SInventory_RPT/rptAreaWiseMonthlyInventoryReport.rpt | PAGE-467 |
| rptChalanDCToDC.rpt | Library.CrystalReports/SInventory_RPT/rptChalanDCToDC.rpt | none found (unused or loaded dynamically) |
| rptChalanForWarehouse.rpt | Library.CrystalReports/SInventory_RPT/rptChalanForWarehouse.rpt | none found (unused or loaded dynamically) |
| rptChalanReceive.rpt | Library.CrystalReports/SInventory_RPT/rptChalanReceive.rpt | none found (unused or loaded dynamically) |
| rptComUnitSales.rpt | Library.CrystalReports/SInventory_RPT/rptComUnitSales.rpt | none found (unused or loaded dynamically) |
| rptCompanySales.rpt | Library.CrystalReports/SInventory_RPT/rptCompanySales.rpt | none found (unused or loaded dynamically) |
| rptCountryStock.rpt | Library.CrystalReports/SInventory_RPT/rptCountryStock.rpt | none found (unused or loaded dynamically) |
| rptCustMaster.rpt | Library.CrystalReports/SInventory_RPT/rptCustMaster.rpt | none found (unused or loaded dynamically) |
| rptCustomerSales.rpt | Library.CrystalReports/SInventory_RPT/rptCustomerSales.rpt | none found (unused or loaded dynamically) |
| rptDCPicking.rpt | Library.CrystalReports/SInventory_RPT/rptDCPicking.rpt | none found (unused or loaded dynamically) |
| rptDCPickingListReport.rpt | Library.CrystalReports/SInventory_RPT/rptDCPickingListReport.rpt | none found (unused or loaded dynamically) |
| rptDCStock.rpt | Library.CrystalReports/SInventory_RPT/rptDCStock.rpt | PAGE-349, PAGE-433 |
| rptDCwiseCountryStock.rpt | Library.CrystalReports/SInventory_RPT/rptDCwiseCountryStock.rpt | none found (unused or loaded dynamically) |
| rptDcPickingSlip.rpt | Library.CrystalReports/SInventory_RPT/rptDcPickingSlip.rpt | none found (unused or loaded dynamically) |
| rptDcToDcChalan.rpt | Library.CrystalReports/SInventory_RPT/rptDcToDcChalan.rpt | none found (unused or loaded dynamically) |
| rptDistrictWiseSales.rpt | Library.CrystalReports/SInventory_RPT/rptDistrictWiseSales.rpt | none found (unused or loaded dynamically) |
| rptInvoiceForCustomer.rpt | Library.CrystalReports/SInventory_RPT/rptInvoiceForCustomer.rpt | PAGE-250 |
| rptMarketwiseBusinessSummary.rpt | Library.CrystalReports/SInventory_RPT/rptMarketwiseBusinessSummary.rpt | PAGE-472, PAGE-473 |
| rptMarketwisePicking.rpt | Library.CrystalReports/SInventory_RPT/rptMarketwisePicking.rpt | PAGE-248, PAGE-432 |
| rptMiaSales.rpt | Library.CrystalReports/SInventory_RPT/rptMiaSales.rpt | none found (unused or loaded dynamically) |
| rptMiaWiseSales.rpt | Library.CrystalReports/SInventory_RPT/rptMiaWiseSales.rpt | none found (unused or loaded dynamically) |
| rptMigoDetail.rpt | Library.CrystalReports/SInventory_RPT/rptMigoDetail.rpt | none found (unused or loaded dynamically) |
| rptOrderDetails.rpt | Library.CrystalReports/SInventory_RPT/rptOrderDetails.rpt | none found (unused or loaded dynamically) |
| rptPickingForWarehouse.rpt | Library.CrystalReports/SInventory_RPT/rptPickingForWarehouse.rpt | PAGE-255 |
| rptProductCostPrice.rpt | Library.CrystalReports/SInventory_RPT/rptProductCostPrice.rpt | none found (unused or loaded dynamically) |
| rptProductTradePrice.rpt | Library.CrystalReports/SInventory_RPT/rptProductTradePrice.rpt | PAGE-128 |
| rptProformaReport.rpt | Library.CrystalReports/SInventory_RPT/rptProformaReport.rpt | PAGE-246, PAGE-251 |
| rptRegionSales.rpt | Library.CrystalReports/SInventory_RPT/rptRegionSales.rpt | none found (unused or loaded dynamically) |
| rptSpecialDiscount.rpt | Library.CrystalReports/SInventory_RPT/rptSpecialDiscount.rpt | none found (unused or loaded dynamically) |
| rptStockTransfarOrder.rpt | Library.CrystalReports/SInventory_RPT/rptStockTransfarOrder.rpt | PAGE-404 |
| rptTopSheet.rpt | Library.CrystalReports/SInventory_RPT/rptTopSheet.rpt | PAGE-254, PAGE-434 |
| rptZoneSales.rpt | Library.CrystalReports/SInventory_RPT/rptZoneSales.rpt | none found (unused or loaded dynamically) |
| Copy of crpChallan.rpt | Solution.Web/Reports/CrystalReports/Copy of crpChallan.rpt | none found (unused or loaded dynamically) |
| Copy of crpWHStockIn.rpt | Solution.Web/Reports/CrystalReports/Copy of crpWHStockIn.rpt | none found (unused or loaded dynamically) |
| INTransitReport.rpt | Solution.Web/Reports/CrystalReports/INTransitReport.rpt | PAGE-398, PAGE-401, PAGE-402 |
| MIOWIseReceivable.rpt | Solution.Web/Reports/CrystalReports/MIOWIseReceivable.rpt | PAGE-402 |
| ProformaandsalesReport.rpt | Solution.Web/Reports/CrystalReports/ProformaandsalesReport.rpt | PAGE-249 |
| SubdeportAreaWiseMonthlyInventoryReport.rpt | Solution.Web/Reports/CrystalReports/SubdeportAreaWiseMonthlyInventoryReport.rpt | PAGE-427 |
| crpAllSalesReport.rpt | Solution.Web/Reports/CrystalReports/crpAllSalesReport.rpt | PAGE-249, PAGE-466, PAGE-481, PAGE-485 |
| crpAllSalesReturnReport.rpt | Solution.Web/Reports/CrystalReports/crpAllSalesReturnReport.rpt | PAGE-279, PAGE-282 |
| crpAuditReportOne.rpt | Solution.Web/Reports/CrystalReports/crpAuditReportOne.rpt | PAGE-468 |
| crpBatchWiseCollectionReport.rpt | Solution.Web/Reports/CrystalReports/crpBatchWiseCollectionReport.rpt | PAGE-300 |
| crpBusinessSummary.rpt | Solution.Web/Reports/CrystalReports/crpBusinessSummary.rpt | PAGE-469 |
| crpCWMonthlyInventoryReport.rpt | Solution.Web/Reports/CrystalReports/crpCWMonthlyInventoryReport.rpt | PAGE-346 |
| crpChallan.rpt | Solution.Web/Reports/CrystalReports/crpChallan.rpt | PAGE-238 |
| crpCustomerLedger.rpt | Solution.Web/Reports/CrystalReports/crpCustomerLedger.rpt | PAGE-301 |
| crpCustomerMaster.rpt | Solution.Web/Reports/CrystalReports/crpCustomerMaster.rpt | PAGE-180, PAGE-181 |
| crpDCStockMonitoringReport.rpt | Solution.Web/Reports/CrystalReports/crpDCStockMonitoringReport.rpt | PAGE-351 |
| crpDCToWhChalan.rpt | Solution.Web/Reports/CrystalReports/crpDCToWhChalan.rpt | PAGE-400 |
| crpDayWiseBusinessReport.rpt | Solution.Web/Reports/CrystalReports/crpDayWiseBusinessReport.rpt | PAGE-477 |
| crpDcOpeningStockReport.rpt | Solution.Web/Reports/CrystalReports/crpDcOpeningStockReport.rpt | PAGE-347 |
| crpDcStockOut.rpt | Solution.Web/Reports/CrystalReports/crpDcStockOut.rpt | PAGE-348 |
| crpDelOrder.rpt | Solution.Web/Reports/CrystalReports/crpDelOrder.rpt | PAGE-211, PAGE-240, PAGE-468 |
| crpDelOrderReport.rpt | Solution.Web/Reports/CrystalReports/crpDelOrderReport.rpt | PAGE-211 |
| crpDeleteInvoiceReport.rpt | Solution.Web/Reports/CrystalReports/crpDeleteInvoiceReport.rpt | PAGE-240 |
| crpDeleteOrder.rpt | Solution.Web/Reports/CrystalReports/crpDeleteOrder.rpt | none found (unused or loaded dynamically) |
| crpDeliveryReturnReport.rpt | Solution.Web/Reports/CrystalReports/crpDeliveryReturnReport.rpt | PAGE-280 |
| crpEmpExpense.rpt | Solution.Web/Reports/CrystalReports/crpEmpExpense.rpt | PAGE-628 |
| crpEmployeewiseProductSales.rpt | Solution.Web/Reports/CrystalReports/crpEmployeewiseProductSales.rpt | PAGE-479 |
| crpFixedCustomerSalesReport.rpt | Solution.Web/Reports/CrystalReports/crpFixedCustomerSalesReport.rpt | none found (unused or loaded dynamically) |
| crpHistoricalReport.rpt | Solution.Web/Reports/CrystalReports/crpHistoricalReport.rpt | PAGE-483 |
| crpINTransitReport.rpt | Solution.Web/Reports/CrystalReports/crpINTransitReport.rpt | PAGE-398, PAGE-401 |
| crpMainMigoReport.rpt | Solution.Web/Reports/CrystalReports/crpMainMigoReport.rpt | PAGE-684 |
| crpMigoDetailsReport.rpt | Solution.Web/Reports/CrystalReports/crpMigoDetailsReport.rpt | PAGE-684 |
| crpOrderDetailsReport.rpt | Solution.Web/Reports/CrystalReports/crpOrderDetailsReport.rpt | PAGE-684 |
| crpProductWiseNationalSalesReport.rpt | Solution.Web/Reports/CrystalReports/crpProductWiseNationalSalesReport.rpt | PAGE-486 |
| crpProductWiseNationalSalesReportZoneWise.rpt | Solution.Web/Reports/CrystalReports/crpProductWiseNationalSalesReportZoneWise.rpt | PAGE-486 |
| crpSalesRejection.rpt | Solution.Web/Reports/CrystalReports/crpSalesRejection.rpt | PAGE-281 |
| crpSalesTrendReport.rpt | Solution.Web/Reports/CrystalReports/crpSalesTrendReport.rpt | PAGE-489 |
| crpSubDepotStockOut.rpt | Solution.Web/Reports/CrystalReports/crpSubDepotStockOut.rpt | PAGE-436 |
| crpWHStockIn.rpt | Solution.Web/Reports/CrystalReports/crpWHStockIn.rpt | PAGE-354 |
| crpWhOpeningStockReport.rpt | Solution.Web/Reports/CrystalReports/crpWhOpeningStockReport.rpt | none found (unused or loaded dynamically) |
| crpWhStockAdjustmentListInfo.rpt | Solution.Web/Reports/CrystalReports/crpWhStockAdjustmentListInfo.rpt | PAGE-353 |
| crpWhStockMonitoring.rpt | Solution.Web/Reports/CrystalReports/crpWhStockMonitoring.rpt | PAGE-355 |
| crptoutype.rpt | Solution.Web/Reports/CrystalReports/crptoutype.rpt | none found (unused or loaded dynamically) |
| rptAreaWiseMonthlyInventoryReport.rpt | Solution.Web/Reports/CrystalReports/rptAreaWiseMonthlyInventoryReport.rpt | PAGE-467 |
| rptBranchSales.rpt | Solution.Web/Reports/CrystalReports/rptBranchSales.rpt | PAGE-470, PAGE-471 |
| rptCustomerPayment.rpt | Solution.Web/Reports/CrystalReports/rptCustomerPayment.rpt | PAGE-302 |
| rptCustomerPaymentDue.rpt | Solution.Web/Reports/CrystalReports/rptCustomerPaymentDue.rpt | PAGE-302 |
| rptDCStock.rpt | Solution.Web/Reports/CrystalReports/rptDCStock.rpt | PAGE-349, PAGE-433 |
| rptDelivaryTopSheet.rpt | Solution.Web/Reports/CrystalReports/rptDelivaryTopSheet.rpt | PAGE-242, PAGE-429 |
| rptFixedSales.rpt | Solution.Web/Reports/CrystalReports/rptFixedSales.rpt | PAGE-482 |
| rptInvoiceForCustomer.rpt | Solution.Web/Reports/CrystalReports/rptInvoiceForCustomer.rpt | PAGE-250 |
| rptInvoiceForCustomerDelivary.rpt | Solution.Web/Reports/CrystalReports/rptInvoiceForCustomerDelivary.rpt | PAGE-241, PAGE-428 |
| rptInvoiceForCustomerDue.rpt | Solution.Web/Reports/CrystalReports/rptInvoiceForCustomerDue.rpt | none found (unused or loaded dynamically) |
| rptInvoiceForCustomer_Past.rpt | Solution.Web/Reports/CrystalReports/rptInvoiceForCustomer_Past.rpt | PAGE-430, PAGE-431 |
| rptInvoiceForCustomer_Uncr.rpt | Solution.Web/Reports/CrystalReports/rptInvoiceForCustomer_Uncr.rpt | PAGE-244, PAGE-250 |
| rptLoadingReport.rpt | Solution.Web/Reports/CrystalReports/rptLoadingReport.rpt | PAGE-247 |
| rptMarketwiseBusinessSummary.rpt | Solution.Web/Reports/CrystalReports/rptMarketwiseBusinessSummary.rpt | PAGE-472, PAGE-473 |
| rptMarketwisePicking.rpt | Solution.Web/Reports/CrystalReports/rptMarketwisePicking.rpt | PAGE-248, PAGE-432 |
| rptMoneyReceipt.rpt | Solution.Web/Reports/CrystalReports/rptMoneyReceipt.rpt | PAGE-303, PAGE-304 |
| rptPickingForWarehouse.rpt | Solution.Web/Reports/CrystalReports/rptPickingForWarehouse.rpt | PAGE-255 |
| rptProductPriceReport.rpt | Solution.Web/Reports/CrystalReports/rptProductPriceReport.rpt | PAGE-128 |
| rptProductTradePrice.rpt | Solution.Web/Reports/CrystalReports/rptProductTradePrice.rpt | PAGE-128 |
| rptProformaReport.rpt | Solution.Web/Reports/CrystalReports/rptProformaReport.rpt | PAGE-246, PAGE-251 |
| rptReturnInvoiceForCustomer.rpt | Solution.Web/Reports/CrystalReports/rptReturnInvoiceForCustomer.rpt | PAGE-252, PAGE-253 |
| rptSalesComparisionReport.rpt | Solution.Web/Reports/CrystalReports/rptSalesComparisionReport.rpt | PAGE-488 |
| rptSalesRejection.rpt | Solution.Web/Reports/CrystalReports/rptSalesRejection.rpt | PAGE-281 |
| rptStockTransfarOrder.rpt | Solution.Web/Reports/CrystalReports/rptStockTransfarOrder.rpt | PAGE-404 |
| rptTopSheet.rpt | Solution.Web/Reports/CrystalReports/rptTopSheet.rpt | PAGE-254, PAGE-434 |
| rptTourPlan.rpt | Solution.Web/Reports/CrystalReports/rptTourPlan.rpt | PAGE-629 |
| rptWHDCStock.rpt | Solution.Web/Reports/CrystalReports/rptWHDCStock.rpt | PAGE-349, PAGE-354 |
| rptWHDCStockExcel.rpt | Solution.Web/Reports/CrystalReports/rptWHDCStockExcel.rpt | PAGE-354 |
| rptfixedcamistsalesreport.rpt | Solution.Web/Reports/CrystalReports/rptfixedcamistsalesreport.rpt | PAGE-480 |
