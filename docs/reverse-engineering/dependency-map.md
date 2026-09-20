# Dependency Map

## Project / assembly dependencies (from `Solution.sln` and `*.csproj`)

| Project | Target framework | Project refs | Assembly refs |
|---|---|---|---|
| Library.DAL/Library.DAL.csproj | v4.0 | Library.DAO.csproj | Microsoft.CSharp, Microsoft.Practices.EnterpriseLibrary.Common, Microsoft.Practices.EnterpriseLibrary.Data, Microsoft.Practices.ObjectBuilder2, System, System.Core, System.Data, System.Data.DataSetExtensions, System.Web, System.Web.Http.Common, System.Xml, System.Xml.Linq |
| Library.BLL/Library.BLL.csproj | v4.0 | Library.DAL.csproj, Library.DAO.csproj | Microsoft.CSharp, System, System.Core, System.Data, System.Data.DataSetExtensions, System.Web, System.Web.Http.Common, System.Xml, System.Xml.Linq |
| Library.DAO/Library.DAO.csproj | v4.0 | — | Microsoft.CSharp, System, System.Core, System.Data, System.Data.DataSetExtensions, System.Web, System.Web.Http.Common, System.Xml, System.Xml.Linq |
| Library.CrystalReports/Library.CrystalReports.csproj | v4.0 | — | CrystalDecisions.CrystalReports.Engine, CrystalDecisions.ReportSource, CrystalDecisions.Shared, Microsoft.CSharp, System, System.Core, System.Data, System.Data.DataSetExtensions, System.Drawing, System.Web.Http.Common, System.Xml, System.Xml.Linq |
| Solution.Web (website project, `Solution.sln` ProjectReferences) | v4.0 | Library.DAL.dll; Library.DAO.dll; Library.BLL.dll | CrystalDecisions 13.0.2000.0, log4net 1.2.10, Newtonsoft.Json 13, EPPlus 6.0.3, Enterprise Library Data (`DLL/`) |

Layering: **Solution.Web → Library.BLL → Library.DAL → SQL Server**, with `Library.DAO` entities shared by all and `Library.CrystalReports` holding `.rpt` + typed datasets. Pages also call DAL classes directly (see `direct_dal` gap flag in page specs).

## BLL → DAL class dependencies

| BLL class | DAL classes used |
|---|---|
| AccountSettingsBll | AccountSettingsDal |
| AreaBLL | AreaDAL, ClsPrimaryKeyFind |
| AreaWiseMonthlyInventoryReportBll | AreaWiseMonthlyInventoryReportDal |
| AuditReportOneBll | AuditReportOneDal |
| CWMonthlyInventoryReportBll | CWMonthlyInventoryReportDal |
| CentralStoreBLL | CentralStoreDAL, ClsPrimaryKeyFind |
| ChalanBLL | ChalanDAL, ClsPrimaryKeyFind |
| ChallanReportViewBLL | ChallanReportViewDAL, ClsPrimaryKeyFind |
| ComUnitSalesReportBLL | ComUnitSalesReportDAL |
| CompanyInfoBLL | ClsPrimaryKeyFind, CompanyInfoDAL |
| CompanySalesReportBLL | CompanySalesReportDAL |
| CompanyUnitBLL | ClsPrimaryKeyFind, CompanyUnitDAL |
| CountryReportBLL | CountryStockReportDAL |
| CustPaymentBLL | ClsCommonInternalDAL, ClsPrimaryKeyFind, CustPaymentDAL |
| CustomerCategoryBLL | ClsPrimaryKeyFind, CustomerCategoryDAL |
| CustomerInvoiceAgingBLL | CustomerInvoiceAgingDAL |
| CustomerLedgerBLL | CustomerLedgerDAL |
| CustomerMasterBLL | ClsPrimaryKeyFind, CustomerMasterDAL |
| CustomerMasterInfoBll | ClsPrimaryKeyFind, CustomerMasterInfoDal |
| CustomerPaymentDeleteBLL | CustomerPaymentDeleteDAL |
| CustomerReturnBLL | CustomerReturnDAL |
| CustomerSalesReportBLL | CustomerSalesReportDAL |
| DCStockReportBLL | DCStockReportDAL |
| DCStoreBLL | ClsPrimaryKeyFind, DCStoreDAL |
| DCUserPermissionBll | DCUserPermissionDal |
| DayWiseBusinessReportBll | DayWiseBusinessReportDal |
| DcOpeningStockReportBll | DcOpeningStockReportDal |
| DeStockOutBLL | ClsPrimaryKeyFind, DeStockOutDal |
| DeleteInvoiceReportBLL | DeleteInvoiceReportDAL |
| DeleteOrderReportBll | DeleteOrderReportDal |
| DepartmentBLL | ClsPrimaryKeyFind, DepartmentDAL |
| DistributionCenterStockMonitoringBll | DistributionCenterStockMonitoringDal |
| DistrictInfoBLL | ClsPrimaryKeyFind, DistrictDAL |
| DistrictSalesReportBLL | DistrictSalesReportDAL |
| EmpGeneralInfoBLL | ClsPrimaryKeyFind, EmpGeneralInfoDAL |
| ExcelUpForCustTagChangeBLL | ClsPrimaryKeyFind, ExcelUpForCustTagChangeDAL |
| ExcelUpForMIGOBLL | ClsPrimaryKeyFind, ExcelUpForMIGODAL |
| ExcelUpForOrderListBLL | ClsPrimaryKeyFind, ExcelUpForOrderListDAL |
| HistoricalReportBll | HistoricalReportDal |
| ImportedApiCustomerBll | ClsPrimaryKeyFind, ImportedApiCustomerDal |
| IngridentsBLL | ClsPrimaryKeyFind, IngridentsDAL |
| InvoiceBLL | ClsPrimaryKeyFind, InvoiceDAL |
| InvoicewisedetailssalesBLL | AreaDAL, ClsPrimaryKeyFind |
| MIAInformationBLL | ClsPrimaryKeyFind, MiaInformationDAL |
| MIOTotalSummaryBLL | MIOTotalSummaryDAL |
| ManufacturerBLL | ClsPrimaryKeyFind, ManufacturerDAL |
| MarketInfoBLL | ClsPrimaryKeyFind, MarketInfoDAL |
| MiaSalesReportBLL | MiaSalesReportDAL |
| MiaTargetBLL | ClsPrimaryKeyFind, MiaTargetDAL |
| MultiCustomerEditBLL | MultiCustomerEditDAL |
| OrderInfoBLL | OrderInfoDAL |
| OrderInfoBLLSalesReturn | OrderInfoDALSalesReturn |
| OrderListBLL | OrderListDAL |
| OrderStatusBll | OrderStatusDal |
| OtherStockActionBLL | OtherStockActionDAL |
| PackSizeBLL | ClsPrimaryKeyFind, PackSizeDAL |
| PanalBLL | ClsPrimaryKeyFind, PanalClsDAL, PanalDAL |
| PendingOrderBll | PendingOrderDal |
| ProTypeBLL | ClsPrimaryKeyFind, ProTypeDAL |
| ProductBLL | ClsPrimaryKeyFind, ProductDAL, SeedDataDAL |
| ProductCategoriesBLL | ClsPrimaryKeyFind, ProductCategoryDAL |
| ProductDiscountBLL | ClsPrimaryKeyFind, ProductDiscountDAL |
| ProductSQBLL | ClsPrimaryKeyFind, ProductSQDAL |
| ProductWiseNationalSalesReportBll | ProductWiseNationalSalesReportDal |
| ProformaOrInvoiceReturnBLL | ProformaOrInvoiceReturn |
| RegionInfoBLL | ClsPrimaryKeyFind, RegionDAL |
| RegionSalesReportBLL | RegionSalesReportDAL |
| RequisitionBLL | ClsPrimaryKeyFind, RequisitionDAL |
| ReturnInvoiceBLL | ClsPrimaryKeyFind, ReturnInvoiceDAL |
| RollBackBLL | RollBackDAL |
| SalesComparisoneReportBll | SalesComparisoneReportDal |
| SalesOperationRollbackBLL | SalesOperationRollbackDAL |
| SalesReturnBLL | ClsPrimaryKeyFind, SalesReturnDAL |
| SalesTrendReportBll | SalesTrendReportDal |
| SampleStockForWHBll | ClsPrimaryKeyFind, SampleStockForWHDal |
| SampleTypeConventionBLL | ClsPrimaryKeyFind, SampleTypeConventionDal |
| ShippingCartonSizeBLL | ClsPrimaryKeyFind |
| StockConditionFreezeBLL | ClsPrimaryKeyFind, StockConditionFreezeDAL |
| StockConditionPermissionBll | StockConditionPermissionDal |
| StockTransferReportBLL | StockTransferReportDAL |
| StockTransportOrderReportBLL | StockTransportOrderReportDAL |
| StockUOMBLL | ClsPrimaryKeyFind, StockUOMDAL |
| SubDepoAdjustBLL | ClsPrimaryKeyFind, SubDepoAdjustDAL |
| SubDepotBLL | SubDepotDAL |
| SubDepotChalanBLL | ClsPrimaryKeyFind, SubDepotChalanDAL |
| SubDepotChalanReturnBLL | ClsPrimaryKeyFind, SubDepotChalanReturnDAL |
| SubDepotStockAdjustmentsVoucherBll | ClsPrimaryKeyFind, SubDepotStockAdjustmentsVoucherDal |
| Sub_InvoiceBLL | ClsPrimaryKeyFind, Sub_InvoiceDAL |
| TotalSummaryBLL | TotalSummaryDAL |
| UnitPriceBLL | ClsPrimaryKeyFind, ProductUnitPriceDAL |
| UserBLL | ClsPrimaryKeyFind, UserDAL |
| WHStockInReportBll | WHStockInReportDal |
| WHStockListBLL | ClsPrimaryKeyFind, WHStockListDAL |
| WarehousePickingBLL | WarehousePickingDAL |
| WarehouseStockInApprovalBll | WarehouseStockInApprovalDal |
| WarehouseStockInBll | ClsPrimaryKeyFind, WarehouseStockInDal |
| WhFreezeStockReleaseBll | WhFreezeStockReleaseDal |
| WhOpeningStockReportBll | WhOpeningStockReportDal |
| WhStockConditionFreezeBll | ClsPrimaryKeyFind, WhStockConditionFreezeDal |
| WhStockMonitoringReportBll | WhStockMonitoringReportDal |
| ZoneInfoBLL | ClsPrimaryKeyFind, ZoneInfoDAL |
| ZoneSalesReportBLL | ZoneSalesReportDAL |

## Pages per DAL class (top 40)

| DAL class | Pages reaching it | File |
|---|---|---|
| CommonDataLoad | 102 | Library.DAL/DoctorModule_DAL/CommonDataLoad.cs |
| SeedDataDAL | 83 | Library.DAL/DoctorModule_DAL/SeedDataDAL.cs |
| OrderInfoDAL | 76 | Library.DAL/SInventory_DAL/OrderInfoDAL.cs |
| StockTransportOrderReportDAL | 47 | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs |
| OtherStockActionDAL | 45 | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs |
| RequisitionDAL | 40 | Library.DAL/SInventory_DAL/RequisitionDAL.cs |
| Setup2DAL | 39 | Library.DAL/DoctorModule_DAL/Setup2DAL.cs |
| InvoiceDAL | 39 | Library.DAL/SInventory_DAL/InvoiceDAL.cs |
| CmnCrystaltoView | 36 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs |
| TotalSummaryDAL | 30 | Library.DAL/SInventory_DAL/TotalSummaryDAL.cs |
| CustomerMasterDAL | 20 | Library.DAL/SInventory_DAL/CustomerMasterDAL.cs |
| BonusCampaignNewDAL | 19 | Library.DAL/MasterSetup_DAL/BonusCampaignNewDAL.cs |
| CustomerInfoDAL | 19 | Library.DAL/MasterSetup_DAL/CustomerInfoDAL.cs |
| CustPaymentDAL | 16 | Library.DAL/SInventory_DAL/CustPaymentDAL.cs |
| SetupDAL | 15 | Library.DAL/DoctorModule_DAL/SetupDAL.cs |
| RouteInformationDAL | 12 | Library.DAL/MasterSetup_DAL/RouteInformationDAL.cs |
| RegionDAL | 12 | Library.DAL/SInventory_DAL/RegionDAL.cs |
| ChalanDAL | 12 | Library.DAL/SInventory_DAL/ChalanDAL.cs |
| ExcelUpForMIGODAL | 11 | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs |
| SAP_IntrigationPointDAL | 11 | Library.DAL/SAP_IntegrationDAL/SAP_IntrigationPointDAL.cs |
| DoctorVisitDAL | 11 | Library.DAL/DoctorVisit_DAL/DoctorVisitDAL.cs |
| TourTypeDal | 11 | Library.DAL/DoctorModule_DAL/TourTypeDal.cs |
| EmployeeInformationDaL | 10 | Library.DAL/MasterSetup_DAL/EmployeeInformationDaL.cs |
| OrderTrackingDAL | 10 | Library.DAL/MasterSetup_DAL/OrderTrackingDAL.cs |
| ProductDAL | 10 | Library.DAL/SInventory_DAL/ProductDAL.cs |
| CompanySalesReportDAL | 9 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs |
| DCStockReportDAL | 9 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs |
| QuotedPriceSetupDAL | 8 | Library.DAL/MasterSetup_DAL/QuotedPriceSetupDAL.cs |
| DCStoreDAL | 8 | Library.DAL/SInventory_DAL/DCStoreDAL.cs |
| CustomerMasterInfoDal | 7 | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs |
| ZoneWiseTargetSetupDal | 7 | Library.DAL/DWSP_DAL/ZoneWiseTargetSetupDal.cs |
| UserInfoDAL | 6 | Library.DAL/DoctorModule_DAL/UserInfoDAL.cs |
| CompanywisebranchDal | 6 | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs |
| MarketStructureTransferDAL | 6 | Library.DAL/Transfer_DAL/MarketStructureTransferDAL.cs |
| WarehouseStockInDal | 6 | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs |
| DoctorDAL | 6 | Library.DAL/MasterSetup_DAL/DoctorDAL.cs |
| AreaWiseTargetSetupDal | 6 | Library.DAL/DWSP_DAL/AreaWiseTargetSetupDal.cs |
| CommonDal | 6 | Library.DAL/DWSP_DAL/CommonDal.cs |
| RSMSetupDal | 5 | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs |
| ThanaDal | 5 | Library.DAL/Thana_DAL/ThanaDal.cs |

## Cross-module table usage (tables reached from pages of ≥ 3 modules)

| Table | Modules |
|---|---|
| tblCompanyUnit | M02, M03, M05, M06, M07, M08, M09, M10, M11, M12, M13, M15, M17, M18 |
| tblUserCompanyUnit | M01, M02, M03, M05, M06, M07, M08, M09, M10, M11, M12, M13, M17 |
| tblCompanyInfo | M01, M03, M04, M05, M06, M07, M09, M10, M11, M12, M13, M15, M17 |
| tblManufacturer | M04, M05, M06, M07, M08, M09, M10, M11, M12, M13, M14, M15, M17 |
| tblProduct | M04, M05, M06, M07, M08, M09, M10, M11, M12, M13, M14, M17 |
| tblInvoice | M05, M06, M07, M08, M09, M10, M11, M12, M13, M17, M18 |
| tblEmpGeneralInfo | M01, M02, M04, M06, M07, M08, M09, M13, M15, M16 |
| tblUser | M01, M02, M06, M07, M09, M10, M11, M12, M13, M15 |
| tblRegion | M03, M05, M06, M07, M09, M12, M13, M14, M15, M17 |
| tblArea | M03, M05, M06, M07, M08, M09, M13, M14, M15, M17 |
| tblCustMaster | M04, M05, M06, M07, M08, M09, M10, M12, M13, M17 |
| tblUnitPrice | M04, M05, M06, M07, M08, M09, M10, M11, M12, M17 |
| tblMarket | M03, M05, M06, M07, M08, M09, M12, M13, M17 |
| View_CustomerMaster | M05, M06, M07, M08, M09, M10, M11, M12, M13 |
| tblMIAInfo | M03, M05, M07, M12, M13, M14, M15, M17 |
| tblInvoiceDetail | M06, M07, M08, M09, M10, M11, M12, M13 |
| tblDCStore | M06, M07, M08, M09, M10, M11, M12, M13 |
| tblDistrict | M01, M03, M05, M07, M13, M15, M17 |
| tblCentralStore | M04, M07, M10, M11, M12, M13, M17 |
| tblOrder | M05, M06, M07, M08, M09, M12, M13 |
| tblRequisition | M05, M06, M07, M10, M11, M13, M17 |
| tblStockInTransfar | M06, M07, M10, M11, M12, M13, M17 |
| tblSubInvoiceMaster | M06, M07, M08, M09, M11, M12, M13 |
| tblDAInfo | M01, M02, M07, M08, M09, M15 |
| tblMainMenu | M01, M02, M06, M10, M12, M18 |
| tblWearhouse | M02, M06, M07, M10, M11, M12 |
| tblTerritory | M04, M06, M08, M09, M13, M14 |
| tblProductDiscount | M04, M06, M07, M08, M09, M12 |
| tblRequsitionChild | M05, M06, M07, M10, M11, M13 |
| tblCustPayDetail | M06, M07, M08, M09, M12, M13 |
| tblCustomerPay | M06, M07, M08, M09, M12, M13 |
| tblProductSQ | M04, M07, M08, M13, M16 |
| tbl_INV_CentralStore_WeightedAvg | M05, M06, M07, M11, M13 |
| tblOrderDetail | M06, M07, M08, M09, M13 |
| tblPaymentType | M06, M07, M08, M09, M12 |
| tblRouteInformationMaster | M06, M07, M08, M09, M12 |
| tblSubInvoiceDetail | M06, M07, M08, M12, M13 |
| tblDCStoreFreeze | M08, M10, M11, M12, M13 |
| tblAppSetup | M02, M10, M12, M18 |
| tblStockUOM | M04, M07, M10, M11 |
| tblCustCategory | M05, M07, M08, M12 |
| tblMIGODetail | M05, M11, M12, M17 |
| View_DCStoreCurrentStock | M06, M07, M08, M09 |
| View_OrderCustomerInfo | M06, M07, M08, M09 |
| tblReturnAmount | M06, M08, M12, M13 |
| CSTL_ACCDB_Unicorn | M06, M07, M09, M13 |
| tblReturnInvoice | M07, M08, M09, M12 |
| tblSubDepotStore | M07, M08, M12, M13 |
| tbl_UserRoleInfo | M01, M02, M15 |
| tblUserType | M01, M02, M15 |
| tblDepartment | M01, M03, M15 |
| tblProductCase | M04, M06, M07 |
| tblProCategory | M04, M09, M10 |
| tblReturnReason | M06, M08, M09 |
| tblTradePolicy | M06, M08, M09 |
| tblTradePolicyNew2 | M06, M08, M09 |
| tblTradePolicyNew | M06, M08, M09 |
| tblInvoiceBatch | M06, M07, M09 |
| tblReturnInvoiceDetail | M07, M08, M12 |
| tblWHStockInDetail | M07, M10, M17 |

## Stored procedures shared by ≥ 3 pages (change-impact hot-spots)

| Stored procedure | Pages | Modules |
|---|---|---|
| sp_CS_GetEmpGeneralInfo_All | 41 | M15,M16,M18 |
| sp_GET_ActionStatusList | 33 | M16,M18,M15 |
| sp_Get_UserRoleInfo | 31 | M15,M18,M02 |
| sp_GET_DistributionCenter | 26 | M18,M05,M06 |
| sp_Get_EmployeeFieldForceInfo_EmpId | 18 | M18,M15,M06 |
| sp_GET_MainPermissionByUserRoleandPageUrl | 17 | M18,M15,M02 |
| sp_check_Vali_MarketStructure | 16 | M15,M03,M04 |
| sp_Get_MileageClaimList | 12 | M15,M16,M02 |
| sp_GET_SMCTypeListParm | 12 | M16,M05,M06 |
| sp_CS_GetArea_ByZoneId_Active | 11 | M14,M15,M03 |
| sp_GET_ProductLine | 11 | M04,M15,M05 |
| sp_GET_CustomerType | 10 | M04,M05,M17 |
| sp_NTS_Group_Active | 10 | M14 |
| sp_HigharchyInfoByEmployeeId | 9 | M14,M13,M03 |
| sp_GET_ProgramTypeListParm | 9 | M16,M05 |
| sp_GET_ProgramTypeListAll | 9 | M06,M09,M13 |
| sp_GET_CustomerTypeAll | 9 | M06,M09,M13 |
| sp_CS_GetTerritory_ByAreaId_All | 9 | M13,M14,M15 |
| sp_Get_Zone_All_Active | 8 | M15,M17,M03 |
| sp_GET_CampaignType | 8 | M04,M17 |
| sp_CS_GetTerritory_ByAreaId_Active | 7 | M15,M03,M13 |
| sp_Get_OfferTypeInfo | 7 | M04,M17 |
| sp_CS_Group_All | 7 | M15,M13,M14 |
| sp_Get_DoctorType_Active | 7 | M16,M14,M17 |
| sp_Get_BonusCampaigndtlList | 6 | M04 |
| sp_Save_BonusCampaignNewDetail | 6 | M04 |
| sp_Get_Product_All | 6 | M04 |
| sp_GET_CampaignDetailMarket_ById | 6 | M04 |
| sp_Get_Product_ForTargetSetup_DDL | 6 | M04 |
| sp_Save_BonusCampaignNewDetail_Up | 6 | M04 |
| sp_Save_BonusCampaignCustomerDetail | 6 | M04 |
| sp_GET_CampaignDetail_ById | 6 | M04 |
| sp_Save_BonusCampaignMarketDetail | 6 | M04 |
| sp_GET_CampaignMaster_ById | 6 | M04 |
| sp_GET_CampaignDetailCustomer_ById | 6 | M04 |
| sp_GET_btnShow | 6 | M10,M11,M06 |
| sp_Get_Zone_ForDSM | 6 | M14,M13 |
| sp_Get_Zone_AllByGroup | 6 | M13,M14,M15 |
| sp_Get_Zone_All_Active_ByGroup | 6 | M14 |
| sp_CS_Designation_Active | 6 | M15,M17,M14 |
| sp_GET_StationTypeList | 5 | M03,M05,M15 |
| sp_Update_MarketStructure_Transfer | 5 | M03 |
| sp_Save_BonusCampaignNewMaster | 5 | M04 |
| sp_Update_BonusCampaignpkCampaignSetupId | 5 | M04 |
| sp_Del_BonusCampaignNewMaster | 5 | M04 |
| sp_Update_BonusCampaignNewMaster | 5 | M04 |
| sp_I_Customer | 5 | M09,M05,M14 |
| sp_Get_NewReceiveableList | 5 | M06,M07,M09 |
| sp_GET_RouteInformationDADetailDDL | 5 | M09,M06 |
| sp_Check_Duplicate_InvoiceFinalPayment | 5 | M09 |
| sp_Get_RouteInfoforCustPayment | 5 | M09 |
| sp_Update_InvoiceFinalPayment | 5 | M09 |
| sp_CS_Department_Active | 5 | M15,M14,M17 |
| sp_CS_Shift_Active | 5 | M15,M14,M17 |
| sp_Save_UserMarketDetail | 4 | M02,M15 |
| sp_check_da_UserInfo | 4 | M02,M15 |
| sp_check_UserInfo_Save | 4 | M02,M15 |
| sp_Save_UserCompanyUnit | 4 | M02,M15 |
| sp_Get_EmployeeList | 4 | M02,M17 |
| sp_check_da_UserInfo_Save | 4 | M02,M15 |
