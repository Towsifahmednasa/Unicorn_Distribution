# Inferred Table Relationships

**No foreign-key constraints are available** (no DDL in repo). The relationships below are **INFERRED** from `JOIN ... ON a.col = b.col` predicates in inline SQL of the C# code: each row shows how many distinct methods use the join. Cardinality and enforcement are UNKNOWN. Master/detail pairs are inferred from naming (`tblX` / `tblXDetail`).

| Table.Column A | Table.Column B | Methods using join | Example source | Confidence |
|---|---|---|---|---|
| tblSubInvoiceDetail.InvoiceId | tblSubInvoiceMaster.InvoiceId | 90 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:68 | INFERRED (≥3 uses) |
| tblInvoice.InvoiceId | tblInvoiceDetail.InvoiceId | 83 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:285 | INFERRED (≥3 uses) |
| View_CustomerMaster.CustomerMasterId | tblSubInvoiceMaster.CustomerMasterId | 44 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:68 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblSubInvoiceMaster.ComUnitId | 42 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:68 | INFERRED (≥3 uses) |
| tblCustMaster.CustomerMasterId | tblInvoice.CustomerMasterId | 40 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:24 | INFERRED (≥3 uses) |
| tblInvoiceDetail.ProductCode | tblProduct.ProductCode | 31 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1012 | INFERRED (≥3 uses) |
| View_CustomerMaster.CustomerMasterId | tblInvoice.CustomerMasterId | 30 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:285 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblInvoice.ComUnitId | 27 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:106 | INFERRED (≥3 uses) |
| tblInvoice.InvoiceId | tblSubInvoiceDetail.InvoiceId | 26 | Library.DAL/SInventory_DAL/MIOTotalSummaryDAL.cs:246 | INFERRED (≥3 uses) |
| tblInvoice.OrderId | tblOrder.OrderId | 24 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | INFERRED (≥3 uses) |
| tblCustCategory.CategoryId | tblCustMaster.CategoryId | 23 | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:144 | INFERRED (≥3 uses) |
| tblCentralStore.MigoDetailID | tblWHStockInDetail.WHStockInDetailID | 18 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:447 | INFERRED (≥3 uses) |
| tblOrder.InvoiceId | tblSubInvoiceDetail.InvoiceId | 16 | Library.DAL/SInventory_DAL/MIOTotalSummaryDAL.cs:31 | INFERRED (≥3 uses) |
| tblOrder.DistributionRouteId | tblRouteInformationMaster.RouteInformationMasterId | 15 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:2079 | INFERRED (≥3 uses) |
| tblCentralStore.ReceiveId | tblStockInTransfar.ReceiveId | 14 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:447 | INFERRED (≥3 uses) |
| tblCustMaster.CustomerMasterId | tblReturnInvoice.CustomerMasterId | 14 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED (≥3 uses) |
| tblProduct.ProductCode | tblSubInvoiceDetail.ProductCode | 14 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:68 | INFERRED (≥3 uses) |
| tblInvoice.PaymentTypeId | tblPaymentType.PaymentTypeId | 14 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1239 | INFERRED (≥3 uses) |
| tblInvoice.UserId | tblUser.UserId | 14 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1239 | INFERRED (≥3 uses) |
| tblCustPayDetail.InvoiceId | tblInvoice.InvoiceId | 13 | Solution.Web/SInventory_UI/MoneyReceiptPreview.aspx.cs:51 | INFERRED (≥3 uses) |
| tblOrder.OrderId | tblOrderDetail.OrderId | 13 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:245 | INFERRED (≥3 uses) |
| View_CustomerMaster.MiaId | tblMIAInfo.MiaId | 13 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:285 | INFERRED (≥3 uses) |
| tblInvoice.CustomerMasterId | tblProductDiscount.CustomerMasterId | 13 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1256 | INFERRED (≥3 uses) |
| tblReturnInvoice.ReturnInvoiceId | tblReturnInvoiceDetail.ReturnInvoiceId | 12 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblReturnInvoice.ComUnitId | 12 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED (≥3 uses) |
| tblCustPayDetail.CustPayId | tblCustomerPay.CustPayId | 10 | Solution.Web/SInventory_UI/MoneyReceiptPreview.aspx.cs:51 | INFERRED (≥3 uses) |
| tblSubDepotStore.DCStoreId | tblSubInvoiceDetail.DCStoreId | 10 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:68 | INFERRED (≥3 uses) |
| tblSubDepotStore.SubDCStoreId | tblSubInvoiceDetail.SubDCStoreId | 10 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:68 | INFERRED (≥3 uses) |
| tblPaymentType.PaymentTypeId | tblReturnInvoice.PaymentTypeId | 10 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1055 | INFERRED (≥3 uses) |
| tblMIAInfo.MiaId | tblReturnInvoice.MiaId | 10 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1055 | INFERRED (≥3 uses) |
| tblReturnInvoice.UserId | tblUser.UserId | 10 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1055 | INFERRED (≥3 uses) |
| tblRequisition.ReqId | tblStockInTransfar.ReqId | 9 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:447 | INFERRED (≥3 uses) |
| tblProduct.ProductCode | tblStockInTransfar.ProductCode | 8 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:447 | INFERRED (≥3 uses) |
| tblStockInTransfar.ProductCode | tblUnitPrice.ProductCode | 8 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:447 | INFERRED (≥3 uses) |
| tblDCStore.DCStoreId | tblInvoiceDetail.DCStoreId | 8 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:285 | INFERRED (≥3 uses) |
| tblInvoice.MiaId | tblMIAInfo.MiaId | 8 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1239 | INFERRED (≥3 uses) |
| tblInvoice.BatchId | tblInvoiceBatch.BatchId | 7 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:36 | INFERRED (≥3 uses) |
| tblProduct.ProductCode | tblUnitPrice.ProductCode | 7 | Library.DAL/SInventory_DAL/DCStoreDAL.cs:80 | INFERRED (≥3 uses) |
| tblCompanyInfo.CompanyId | tblCompanyUnit.CompanyId | 6 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:106 | INFERRED (≥3 uses) |
| tblProduct.ProductBrandId | tblProductSQ.ProductBrandId | 6 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED (≥3 uses) |
| tblMIGOMaster.ManufacId | tblManufacturer.ManufacId | 6 | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:116 | INFERRED (≥3 uses) |
| tblProductDiscount.CustomerMasterId | tblReturnInvoice.CustomerMasterId | 6 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1659 | INFERRED (≥3 uses) |
| tblProduct.StockUOMId | tblStockUOM.StockUOMId | 6 | Library.DAL/SInventory_DAL/ProductDAL.cs:81 | INFERRED (≥3 uses) |
| tblPackSize.PackSizeId | tblProduct.PackSizeId | 5 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:191 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblOrder.ComUnitId | 5 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:245 | INFERRED (≥3 uses) |
| tblCompanyInfo.CompanyId | tblCustomerCreditLimit.CompanyId | 5 | Library.DAL/SInventory_DAL/CreditLimitDAL.cs:49 | INFERRED (≥3 uses) |
| tblCustMaster.CustomerMasterId | tblCustomerCreditLimit.CustomerMasterId | 5 | Library.DAL/SInventory_DAL/CreditLimitDAL.cs:49 | INFERRED (≥3 uses) |
| tblCustMaster.CustomerMasterId | tblSubInvoiceMaster.CustomerMasterId | 5 | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:303 | INFERRED (≥3 uses) |
| tblDAInfo.DAId | tblInvoice.Inv_DANameId | 5 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1303 | INFERRED (≥3 uses) |
| tblInvoice.InvoiceId | tblInvoiceBatch.InvoiceId | 4 | Solution.Web/SInventory_UI/DoctorInvoiceCreationByOrder.aspx.cs:30 | INFERRED (≥3 uses) |
| tblEmpGeneralInfo.EmpInfoId | tblMIOInfo.EmployeeId | 4 | Library.DAL/PromoAllocDAL/PromoMITagDAL.cs:168 | INFERRED (≥3 uses) |
| tblManufacturer.ManufacId | tblRequisition.ManufacId | 4 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:103 | INFERRED (≥3 uses) |
| View_CustomerMaster.ComUnitCode | tblCompanyUnit.ComUnitCode | 4 | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:186 | INFERRED (≥3 uses) |
| View_CustomerMaster.MarketCode | tblMarket.MarketCode | 4 | Library.DAL/SInventory_DAL/CustPaymentDAL.cs:186 | INFERRED (≥3 uses) |
| tblDCStoreFreeze.ProductCode | tblProduct.ProductCode | 4 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:37 | INFERRED (≥3 uses) |
| tblCentralStore.ReceiveId | tblWhStoreFreeze.ReceiveId | 4 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:311 | INFERRED (≥3 uses) |
| tblProduct.ProductCode | tblSubDepotStoreFreeze.ProductCode | 4 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED (≥3 uses) |
| tblFocDetails.FocId | tblFocMaster.FocId | 4 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:606 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblDCPicking.ComUnitId | 4 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1167 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblDistrict.ComUnitId | 4 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1190 | INFERRED (≥3 uses) |
| tblWHStockInDetail.WHStockInMasterID | tblWHStockInMaster.WHStockInMasterID | 4 | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:26 | INFERRED (≥3 uses) |
| tblProduct.ProductId | tblWHStockInDetail.ProductId | 4 | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:168 | INFERRED (≥3 uses) |
| tblInvoiceDetail.InvoiceId | tblSubInvoiceMaster.InvoiceId | 4 | Library.DAL/SInventory_DAL/MIOTotalSummaryDAL.cs:31 | INFERRED (≥3 uses) |
| tblCentralStore.ProductCode | tblUnitPrice.ProductCode | 4 | Library.DAL/SInventory_DAL/StockConditionFreezeDAL.cs:48 | INFERRED (≥3 uses) |
| tblAdjustmentType.AdjustmentTypeId | tblWHStockAdjustment.AdjustmentType | 4 | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:17 | INFERRED (≥3 uses) |
| tblPaymentType.PaymentTypeId | tblSubInvoiceMaster.PaymentTypeId | 4 | Library.DAL/SubDepot_DAL/Sub_InvoiceDAL.cs:749 | INFERRED (≥3 uses) |
| tblMIAInfo.MiaId | tblSubInvoiceMaster.MiaId | 4 | Library.DAL/SubDepot_DAL/Sub_InvoiceDAL.cs:749 | INFERRED (≥3 uses) |
| tblSubInvoiceMaster.UserId | tblUser.UserId | 4 | Library.DAL/SubDepot_DAL/Sub_InvoiceDAL.cs:749 | INFERRED (≥3 uses) |
| tblCustMaster.MarketId | tblMarket.MarketId | 3 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED (≥3 uses) |
| tblCustMaster.CustomerMasterId | tblCustomerPay.CustomerMasterId | 3 | Solution.Web/SInventory_UI/CustomerPaymentReport.aspx.cs:175 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblCustomerPay.ComUnitId | 3 | Solution.Web/SInventory_UI/CustomerPaymentReport.aspx.cs:175 | INFERRED (≥3 uses) |
| tblUser.UserRoleID | tbl_UserRoleInfo.UserRoleID | 3 | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:273 | INFERRED (≥3 uses) |
| tblEmpGeneralInfo.EmpInfoId | tblUser.EmpInfoId | 3 | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:273 | INFERRED (≥3 uses) |
| tblActionPageWiseStep.ManuSL | tblActionUserWiseApproval.ManuSL | 3 | Library.DAL/InternalCls/ClsApprovalAction.cs:14 | INFERRED (≥3 uses) |
| tblActionPageWiseStep.ASId | tblActionSteps.ASId | 3 | Library.DAL/InternalCls/ClsApprovalAction.cs:14 | INFERRED (≥3 uses) |
| tblActionStepDetail.ASId | tblActionSteps.ASId | 3 | Library.DAL/InternalCls/ClsApprovalAction.cs:14 | INFERRED (≥3 uses) |
| tblActionUserWiseApproval.ManuSL | tblMainMenu.SL | 3 | Library.DAL/InternalCls/ClsApprovalAction.cs:14 | INFERRED (≥3 uses) |
| tblOrder.ProgramTypeId | tblProgramType.ProgramTypeId | 3 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | INFERRED (≥3 uses) |
| tblCustomertype.CustomerTypeId | tblOrder.CusttypeId | 3 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | INFERRED (≥3 uses) |
| tblDesignation.DesignationId | tblEmpGeneralInfo.DesignationId | 3 | Library.DAL/PanalCls/PanalClsDAL.cs:16 | INFERRED (≥3 uses) |
| tblMainMenu.SL | tblMenuDistribution.MenuSL | 3 | Library.DAL/Panal_DAL/PanalDAL.cs:118 | INFERRED (≥3 uses) |
| tblPromoMIOTagDetail.MIOTagMasterId | tblPromoMIOTagMaster.MIOTagId | 3 | Library.DAL/PromoAllocDAL/PromoMITagDAL.cs:168 | INFERRED (≥3 uses) |
| tblDeliveryManInfo.EmpInfoId | tblEmpGeneralInfo.EmpInfoId | 3 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:17 | INFERRED (≥3 uses) |
| tblDCStore.ProductCode | tblUnitPrice.ProductCode | 3 | Library.DAL/SInventory_DAL/ChalanDAL.cs:441 | INFERRED (≥3 uses) |
| tblDCStore.StockInTransfarId | tblStockInTransfar.StockInTransfarId | 3 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:37 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblDeStockOutMaster.ComUnitId | 3 | Library.DAL/SInventory_DAL/DeStockOutDal.cs:84 | INFERRED (≥3 uses) |
| tblDeStockOutMaster.InvoiceId | tblInvoice.InvoiceId | 3 | Library.DAL/SInventory_DAL/DeStockOutDal.cs:84 | INFERRED (≥3 uses) |
| tblDeStockOutDetails.DcStockOutMasterId | tblDeStockOutMaster.DcStockOutMasterId | 3 | Library.DAL/SInventory_DAL/DeStockOutDal.cs:164 | INFERRED (≥3 uses) |
| tblManufacturer.ManufacId | tblOrderListMaster.ManufacId | 3 | Library.DAL/SInventory_DAL/ExcelUpForOrderListDAL.cs:56 | INFERRED (≥3 uses) |
| tblDistributionRoute.DistributionRouteId | tblOrder.DistributionRouteId | 3 | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1009 | INFERRED (≥3 uses) |
| tbl_BonusCampaignNewDetail.CampaignMasterId | tbl_BonusCampaignNewMaster.CampgainMasterId | 3 | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1543 | INFERRED (≥3 uses) |
| tblInvoiceDetail.OrderDetailsId | tblOrderDetail.OrderDetailId | 3 | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1727 | INFERRED (≥3 uses) |
| tblCustMaster.CustomerTypeId | tblOrder.CustTypeId | 3 | Library.DAL/SInventory_DAL/OrderInfoDAL.cs:1833 | INFERRED (≥3 uses) |
| tblDCStore.DCStoreId | tblDepotToWHChalanDetail.DCStoreId | 3 | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:169 | INFERRED (≥3 uses) |
| tblRequsitionChild.ReqChildId | tblStockInTransfar.ReqChildId | 3 | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs:43 | INFERRED (≥3 uses) |
| tblWHStockAdjustment.FromStore | tblWearhouse.WearhouseId | 3 | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:17 | INFERRED (≥3 uses) |
| tblCompanyUnit.ComUnitId | tblSubDepotStockOutMaster.ComUnitId | 3 | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:62 | INFERRED (≥3 uses) |
| tblProductDiscount.CustomerMasterId | tblSubInvoiceMaster.CustomerMasterId | 3 | Library.DAL/SubDepot_DAL/Sub_InvoiceDAL.cs:820 | INFERRED (≥3 uses) |
| tblArea.AreaId | tblTerritory.AreaId | 2 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblDCStore.ProductCode | tblProduct.ProductCode | 2 | Solution.Web/SInventory_UI/BatchLifecycleReport.aspx.cs:59 | INFERRED |
| tblUser.UserTypeId | tblUserType.UserTypeId | 2 | Library.DAL/DoctorModule_DAL/RSMSetupDal.cs:273 | INFERRED |
| tblUser.UserId | tblUserCompanyUnit.UserId | 2 | Library.DAL/PanalCls/PanalClsDAL.cs:16 | INFERRED |
| tblCustMaster.CustomerMasterId | tblOrderDel.CustomerMasterId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.DelBy | tblUser.UserId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblEmpGeneralInfo.EmpInfoId | tblOrderDel.RSMId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblEmpGeneralInfo.EmpInfoId | tblOrderDel.ASMId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblEmpGeneralInfo.EmpInfoId | tblOrderDel.MIOId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.MarketId | tblmarket.MarketId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.SubTerritoryId | tblSubTerritory.SubTerritoryId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.TerritoryId | tblTerritory.TerritoryId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblArea.AreaId | tblOrderDel.AreaId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.RegionId | tblRegion.RegionId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.GroupId | tbl_Group.GroupId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblOrderDel.OrderId | tblOrderDetailDel.OrderId | 2 | Library.DAL/SInventory_DAL/AuditReportOneDal.cs:28 | INFERRED |
| tblDeliveryManInfo.DeliveryManId | tblInvoice.DeliveryManId | 2 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:71 | INFERRED |
| tblInvoice.InvoiceId | tblNonTranscationalInvoiceMaster.InvoiceId | 2 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:178 | INFERRED |
| tblCompanyUnit.ComUnitId | tblNonTranscationalInvoiceMaster.ComUnitId | 2 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:178 | INFERRED |
| tblCustMaster.CustomerMasterId | tblNonTranscationalInvoiceMaster.CustomerMasterId | 2 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:178 | INFERRED |
| tblChalanDetail.DCStoreId | tblDCStore.DCStoreId | 2 | Library.DAL/SInventory_DAL/ChalanDAL.cs:123 | INFERRED |
| tblChalanDetail.ChalanId | tblChalanInfo.ChalanId | 2 | Library.DAL/SInventory_DAL/ChalanDAL.cs:514 | INFERRED |
| tblDCStore.DCStoreId | tblReturnInvoiceDetail.DCStoreId | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED |
| tblProduct.ProductCode | tblReturnInvoiceDetail.ProductCode | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED |
| tblInvoice.AdjustInvoiceNo_ReturnInvoiceNo | tblReturnInvoice.ReturnInvoiceNo | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:27 | INFERRED |
| tblOrderDetail.ProductCode | tblProduct.ProductCode | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:245 | INFERRED |
| View_CustomerMaster.CustomerCode | tblOrder.CustomerCode | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:245 | INFERRED |
| tblInvoice.OrderNo | tblOrder.OrderCode | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:245 | INFERRED |
| tblOrderDetail.ProductCode | tblUnitPrice.ProductCode | 2 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:245 | INFERRED |
| tblCustMaster.SalesPersonId | tblEmpGeneralInfo.EmpInfoId | 2 | Library.DAL/SInventory_DAL/CustomerInvoiceAgingDAL.cs:58 | INFERRED |
| tblCompanyUnit.ComUnitId | tblDCStore_OpeningBalance.ComUnitId | 2 | Library.DAL/SInventory_DAL/DcOpeningStockReportDal.cs:23 | INFERRED |
| tblSubDepotStore.SubDCStoreId | tblSubDepotStoreFreeze.SubDCStoreId | 2 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED |
| tblInvoiceDeleteLog.InvoiceId | tblInvoiceDetail_DeleterRecord.InvoiceId | 2 | Library.DAL/SInventory_DAL/DeleteInvoiceReportDAL.cs:29 | INFERRED |
| tblCompanyUnit.ComUnitId | tblInvoiceDeleteLog.ComUnitId | 2 | Library.DAL/SInventory_DAL/DeleteInvoiceReportDAL.cs:29 | INFERRED |
| View_CustomerMaster.CustomerCode | tblOrderDel.CustomerCode | 2 | Library.DAL/SInventory_DAL/DeleteOrderReportDal.cs:29 | INFERRED |
| tblMIGODetail.MigoMasterID | tblMIGOMaster.MigoMasterID | 2 | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:154 | INFERRED |
| tblCustMaster.CustomerMasterId | tblOrder.CustomerMasterId | 2 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:1303 | INFERRED |
| tblCustomerType.CustomerTypeId | tblOrder.CustTypeId | 2 | Library.DAL/SInventory_DAL/InvoiceDAL.cs:2079 | INFERRED |
| tblDCStoreFreeze.ProductCode | tblUnitPrice.ProductCode | 2 | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:18 | INFERRED |
| tblProCategory.CategoryId | tblProduct.CategoryId | 2 | Library.DAL/SInventory_DAL/ProductDAL.cs:81 | INFERRED |
| tblProduct.ProductCode | tblProductCase.ProductCode | 2 | Library.DAL/SInventory_DAL/ProductDAL.cs:81 | INFERRED |
| tblGenericGroup.GenericGroupId | tblProduct.GenericGroupId | 2 | Library.DAL/SInventory_DAL/ProductDAL.cs:81 | INFERRED |
| tblProduct.ProductGroupId | tblProductGroup.GroupId | 2 | Library.DAL/SInventory_DAL/ProductDAL.cs:81 | INFERRED |
| tbl_INV_CentralStore_WeightedAvg.Weighted_Avg_Id | tbl_INV_Central_BatchWiseStock.Weighted_Avg_Id | 2 | Library.DAL/SInventory_DAL/RequisitionDAL.cs:306 | INFERRED |
| InboundLots.Inbound_WAvg_Id | OutboundSum.Inbound_WAvg_Id | 2 | Library.DAL/SInventory_DAL/RequisitionDAL.cs:306 | INFERRED |
| tblChalanDetail.ChalanDetailsId | tblDCStore.ChalanDetailsId | 2 | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:205 | INFERRED |
| tblDepotToWHChalanDetail.SChalanId | tblDepotToWHChalanInfo.SChalanId | 2 | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:237 | INFERRED |
| tblChalanInfo.ToComUnitCode | tblCompanyUnit.ComUnitCode | 2 | Library.DAL/SInventory_DAL/StockTransferReportDAL.cs:12 | INFERRED |
| tblManufacturer.ManufacId | tblWHStockInMaster.ManufacId | 2 | Library.DAL/SInventory_DAL/WarehouseStockInApprovalDal.cs:28 | INFERRED |
| tblUnitPrice.ProductId | tblWhStoreFreeze.ProductId | 2 | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:18 | INFERRED |
| tblProduct.ProductId | tblWhStoreFreeze.ProductId | 2 | Library.DAL/SInventory_DAL/WhFreezeStockReleaseDal.cs:18 | INFERRED |
| tblWHAdjustmentDetail.WHStockAdjId | tblWHStockAdjustment.WHStockAdjId | 2 | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:29 | INFERRED |
| tblCentralStore.ReceiveId | tblWHAdjustmentDetail.ReceiveId | 2 | Library.DAL/SInventory_DAL/WHStockAdjDAL.cs:29 | INFERRED |
| tblRequisition.ReqId | tblRequsitionChild.ReqId | 2 | Library.DAL/SInventory_DAL/WhStockMonitoringReportDal.cs:16 | INFERRED |
| tblSubDepotStockOutMaster.InvoiceId | tblSubInvoiceMaster.InvoiceId | 2 | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:62 | INFERRED |
| tblMarket.SubTerritoryId | tblSubTerritory.SubTerritoryId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblSubTerritory.TerritoryId | tblTerritory.TerritoryId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblArea.RegionId | tblRegion.RegionId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblRegion.GroupId | tbl_Group.GroupId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblCustMaster.MarketId | tblRouteInformationMarketDetail.MarketId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblRouteInformationMarketDetail.RouteInformationMasterId | tblRouteInformationMaster.RouteInformationMasterId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblCompanyUnit.ComUnitId | tblRouteInformationMaster.DCId | 1 | Solution.Web/App_Code/SInventoryWebService.cs:398 | INFERRED |
| tblDCRApprovalLog.TableId | tbl_DCRInfo.DcrId | 1 | Solution.Web/DoctorVisit_UI/DCRList.aspx.cs:134 | INFERRED |
| tblBankInfo.BankId | tblCustPayDetail.BankAccId | 1 | Solution.Web/SInventory_UI/MoneyReceiptPreview.aspx.cs:51 | INFERRED |
| tblSubInvoiceMaster.InvoiceId | tblSubInvoiceMasterBatch.InvoiceId | 1 | Solution.Web/SubDepot_UI/SubDepotInvoiceCreation.aspx.cs:89 | INFERRED |
| tblDoctorProgramType.DegreeId | tblDoctorProgramTypeDetail.DegId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblDoctorProgramType.SpecialityId | tblDoctorProgramTypeDetail.SpecialityId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblDoctorProgramType.ProgramTypeId | tblDoctorProgramTypeDetail.ProgramTypeId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblDesignation.DesignationId | tblDoctorMaster.DesignationId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblDoctorMaster.ProgramType | tblDoctorProgramType.ProgramTypeId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblDoctorMaster.EntryBy | tblUser.UserId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblDoctorMaster.UpdateBy | tblUser.UserId | 1 | Library.DAL/DoctorModule_DAL/SetupDAL.cs:66 | INFERRED |
| tblCustPayDetail.DANameId | tblDAInfo.DAId | 1 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | INFERRED |
| tblCustomerPay.CreateBy | tblEmpGeneralInfo.EmpMasterCode | 1 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | INFERRED |
| tblCustomerPay.CreateBy | tblUser.LoginName | 1 | Library.DAL/MasterSetup_DAL/CmnCrystaltoView.cs:550 | INFERRED |
| tblDAInfo.DAId | tblUser.daInfoId | 1 | Library.DAL/PanalCls/PanalClsDAL.cs:16 | INFERRED |
| tblRoleType.RoleTypeId | tbl_UserRoleInfo.RoleTypeId | 1 | Library.DAL/PanalCls/PanalClsDAL.cs:16 | INFERRED |
| tblGroupWisePromoQty.ProductId | tblProduct.ProductId | 1 | Library.DAL/PromoAllocDAL/GroupWisePromoQtyDAL.cs:182 | INFERRED |
| tblEmpGeneralInfo.EmpInfoId | tblPromoMIOTagDetail.EmpInfoId | 1 | Library.DAL/PromoAllocDAL/PromoMITagDAL.cs:189 | INFERRED |
| tblInvoice.MiaId | tblMIOInfo.MIOId | 1 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:106 | INFERRED |
| tblNonTranscationalInvoiceDetail.InvReturnMasterID | tblNonTranscationalInvoiceMaster.InvReturnMasterID | 1 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:191 | INFERRED |
| tblNonTranscationalInvoiceDetail.ProductId | tblProduct.ProductId | 1 | Library.DAL/SInventory_DAL/BatchWiseCollectionReportDal.cs:191 | INFERRED |
| tblCompanyUnit.ComUnitId | tblDCStore.ComUnitId | 1 | Library.DAL/SInventory_DAL/ChalanDAL.cs:441 | INFERRED |
| tblCentralStore.ProductId | tblUnitPrice.ProductId | 1 | Library.DAL/SInventory_DAL/ChalanDAL.cs:620 | INFERRED |
| View_CentralStoreCurrentStock.ProductCode | tblRequsitionChild.ProductCode | 1 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:168 | INFERRED |
| tblRequsitionChild.ProductCode | tblUnitPrice.ProductCode | 1 | Library.DAL/SInventory_DAL/ChallanReportViewDAL.cs:168 | INFERRED |
| View_CustomerMaster.DistrictId | tblDistrict.DistrictId | 1 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:285 | INFERRED |
| View_CustomerMaster.RegionId | tblRegion.RegionId | 1 | Library.DAL/SInventory_DAL/CompanySalesReportDAL.cs:285 | INFERRED |
| tblCompanyUnit.ComUnitId | tblCompanyWiseDeposit.CompanyId | 1 | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:83 | INFERRED |
| tblCompanyWiseDeposit.EmpMasterCode | tblEmpGeneralInfo.EmpMasterCode | 1 | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:83 | INFERRED |
| tblBankInfo.BankId | tblCompanyWiseDeposit.BankId | 1 | Library.DAL/SInventory_DAL/CompanywisebranchDal.cs:83 | INFERRED |
| View_CustomerMaster.CustomerMasterId | tblCustMaster.CustomerMasterId | 1 | Library.DAL/SInventory_DAL/CustomerMasterInfoDal.cs:192 | INFERRED |
| tblCustomerReturnMaster.InvoiceId | tblInvoice.InvoiceId | 1 | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:118 | INFERRED |
| tblCustomerReturnDetail.CustReturnId | tblCustomerReturnMaster.CustReturnId | 1 | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:126 | INFERRED |
| tblCustomerReturnDetail.InvoiceDetailId | tblInvoiceDetail.InvoiceDetailId | 1 | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:126 | INFERRED |
| tblMarket.TerritoryId | tblTerritory.TerritoryId | 1 | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:144 | INFERRED |
| tblMIOInfo.TerritoryId | tblTerritory.TerritoryId | 1 | Library.DAL/SInventory_DAL/CustomerReturnDAL.cs:144 | INFERRED |
| tblDCStoreFreeze.StockInTransfarId | tblStockInTransfar.StockInTransfarId | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:37 | INFERRED |
| tblCentralStore.MigoDetailID | tblWHStockInDetail.MigoDetailID | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:37 | INFERRED |
| tblSubDepotStoreFreeze.SubDCStoreId | tblSubInvoiceDetail.SubDCStoreId | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED |
| tblSubDepotChalanDetail.SChalanId | tblSubDepotChalanInfo.SChalanId | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED |
| tblDCStore.DCStoreId | tblSubDepotChalanDetail.DCStoreId | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED |
| tblSubDepot.SubDepotCode | tblSubDepotChalanInfo.SubDepotCode | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED |
| tblProduct.ProductCode | tblSubDepotChalanDetail.ProductCode | 1 | Library.DAL/SInventory_DAL/DCStockReportDAL.cs:463 | INFERRED |
| tblCustMaster.CustomerCode | tblDeStockOutMaster.CustomerCode | 1 | Library.DAL/SInventory_DAL/DeStockOutDal.cs:164 | INFERRED |
| tblDeStockOutDetails.ProductCode | tblunitprice.ProductCode | 1 | Library.DAL/SInventory_DAL/DeStockOutDal.cs:164 | INFERRED |
| tblCentralStore.MigoDetailID | tblMIGODetail.MigoDetailID | 1 | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:137 | INFERRED |
| tblCustomerMasterTagChangeExcelFileMaster.ManufacId | tblManufacturer.ManufacId | 1 | Library.DAL/SInventory_DAL/ExcelUpForCustTagChangeDAL.cs:224 | INFERRED |
| tblCustomerMasterExcelFileMaster.ManufacId | tblManufacturer.ManufacId | 1 | Library.DAL/SInventory_DAL/ExcelUpForMIGODAL.cs:210 | INFERRED |
| tblWHStockInMaster.WarehouseId | tblWearhouse.WearhouseId | 1 | Library.DAL/SInventory_DAL/MaterialConversionReceiveDal.cs:136 | INFERRED |
| tblCompanyUnit.ComUnitId | tblUserCompanyUnit.CompanyUnitId | 1 | Library.DAL/SInventory_DAL/OrderListDAL.cs:258 | INFERRED |
| tblOrder.RegionId | tblRegion.RegionId | 1 | Library.DAL/SInventory_DAL/OrderListDAL.cs:283 | INFERRED |
| tblMarket.MarketId | tblOrder.MarketId | 1 | Library.DAL/SInventory_DAL/OrderListDAL.cs:283 | INFERRED |
| tblOrder.TerritoryId | tblTerritory.TerritoryId | 1 | Library.DAL/SInventory_DAL/OrderListDAL.cs:283 | INFERRED |
| tblSubDepotStoreFreeze.ProductCode | tblUnitPrice.ProductCode | 1 | Library.DAL/SInventory_DAL/OtherStockActionDAL.cs:138 | INFERRED |
| tblOrder.OrderId | tblSubInvoiceMaster.OrderId | 1 | Library.DAL/SInventory_DAL/PaymentReverseDal.cs:27 | INFERRED |
| tblCompanyUnit.ComUnitCode | tblOrderListDetail.SalesCentre | 1 | Library.DAL/SInventory_DAL/PendingOrderDal.cs:16 | INFERRED |
| tblProduct.CaseId | tblProductCase.CaseId | 1 | Library.DAL/SInventory_DAL/ProductDAL.cs:278 | INFERRED |
| tblProduct.ProductCode | tblUnitPrice_Old.ProductCode | 1 | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:430 | INFERRED |
| tblCompanyUnit.ComUnitId | tblCustMaster.ComUnitId | 1 | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:516 | INFERRED |
| tblCustMaster.TerritoryId | tblMIOInfo.TerritoryId | 1 | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:516 | INFERRED |
| tblSalesReturnDetail.OrderId | tblTempSalesReturnOrder.OrderId | 1 | Library.DAL/SInventory_DAL/ReturnInvoiceDAL.cs:1362 | INFERRED |
| tblMIOInfo.EmployeeId | tblOrder.MIOId | 1 | Library.DAL/SInventory_DAL/SalesReturnDAL.cs:1158 | INFERRED |
| tblEmpGeneralInfo.EmpInfoId | tblOrder.MIOId | 1 | Library.DAL/SInventory_DAL/SalesReturnDAL.cs:1158 | INFERRED |
| tblSampleStockForWareHouseMaster.WareHouseId | tblWearhouse.WearhouseId | 1 | Library.DAL/SInventory_DAL/SampleStockForWHDal.cs:106 | INFERRED |
| tblCompanyUnit.ComUnitId | tblSampleStockForDcMaster.ComUnitId | 1 | Library.DAL/SInventory_DAL/SampleTypeConventionDal.cs:87 | INFERRED |
| tblCompanyUnit.ComUnitId | tblDCStoreFreeze.ComUnitId | 1 | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:87 | INFERRED |
| tblDCStoreFreeze.DCStoreFreezeId | tblDepotToWHChalanDetail.DCStoreFreezeId | 1 | Library.DAL/SInventory_DAL/SCtoWHTransferDal.cs:169 | INFERRED |
| tblDepotToWHChalanDetail.ProductCode | tblProduct.ProductCode | 1 | Library.DAL/SInventory_DAL/StockRcvByWhDal.cs:28 | INFERRED |
| tblDepotToWHChalanDetail.PurposeId | tblPurpose.PurposeId | 1 | Library.DAL/SInventory_DAL/StockRcvByWhDal.cs:28 | INFERRED |
| tblChalanInfo.FromComUnitId | tblCompanyUnit.ComUnitId | 1 | Library.DAL/SInventory_DAL/StockTransferReportDAL.cs:12 | INFERRED |
| tblChalanInfo.ChalanNo | tblRequisition.IssueChalanNo | 1 | Library.DAL/SInventory_DAL/StockTransferReportDAL.cs:12 | INFERRED |
| tblRequisition.EntryBy | tblUser.LoginName | 1 | Library.DAL/SInventory_DAL/StockTransferReportDAL.cs:12 | INFERRED |
| tblRequisition.WarehouseId | tblWearhouse.WearhouseId | 1 | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs:20 | INFERRED |
| tblCompanyUnit.ComUnitId | tblRequisition.ComUnitId | 1 | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs:20 | INFERRED |
| tblProduct.ProductCode | tblRequsitionChild.ProductCode | 1 | Library.DAL/SInventory_DAL/StockTransportOrderReportDAL.cs:43 | INFERRED |
| tblProduct.ProductId | tblUnitPrice.ProductId | 1 | Library.DAL/SInventory_DAL/WarehouseStockInDal.cs:32 | INFERRED |
| tblSupplierInformation.SupplierId | tblWHStockInMaster.SupplierId | 1 | Library.DAL/SInventory_DAL/WHStockInReportDal.cs:22 | INFERRED |
| tblSubDepot.SubDepotId | tblSubDepotStore.SubDepotId | 1 | Library.DAL/SubDepot_DAL/SubDepoAdjustDAL.cs:349 | INFERRED |
| tblCompanyUnit.ComUnitId | tblSubDepot.ComUnitId | 1 | Library.DAL/SubDepot_DAL/SubDepotDAL.cs:57 | INFERRED |
| tblInvoice.InvoiceId | tblSubDepotStockOutMaster.InvoiceId | 1 | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:114 | INFERRED |
| tblSubDepotStockOutDetails.SubDcStockOutMasterId | tblSubDepotStockOutMaster.SubDcStockOutMasterId | 1 | Library.DAL/SubDepot_DAL/SubDepotStockAdjustmentsVoucherDal.cs:126 | INFERRED |
| tblProductWiseSalesTarget.GroupId | tbl_Group.GroupId | 1 | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:64 | INFERRED |
| tblProductWiseSalesTarget.RegionId | tblRegion.RegionId | 1 | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:64 | INFERRED |
| tblArea.AreaId | tblProductWiseSalesTarget.AreaId | 1 | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:64 | INFERRED |
| tblProductWiseSalesTarget.TerritoryId | tblTerritory.TerritoryId | 1 | Library.DAL/TargetDAL/ProuctWiseSalesDAL.cs:64 | INFERRED |
| tblCompanyInfo.CompanyId | tblEmpGeneralInfo.CompanyId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblDivision.DivisionId | tblEmpGeneralInfo.DivisionId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblDivisionWing.DivisionWId | tblEmpGeneralInfo.DivisionWId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblDepartment.DepartmentId | tblEmpGeneralInfo.DepartmentId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEmpGeneralInfo.SectionId | tblSection.SectionId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEmpGeneralInfo.SubSectionId | tblSubSection.SubSectionId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEmpCategory.EmpCategoryId | tblEmpGeneralInfo.EmpCategoryId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEmpGeneralInfo.SalaryGradeId | tblSalaryGrade.SalaryGradeId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEmpGeneralInfo.SalaryStepId | tblSalaryStep.SalaryStepId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblDesignationType.DesignationTypeId | tblEmpGeneralInfo.DesignationTypeId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEmpGeneralInfo.SalaryLoationId | tblSalaryLocation.SalaryLoationId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:17 | INFERRED |
| tblEducationName.EducationNameID | tblEmpEducation.EducationNameId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:126 | INFERRED |
| tblBoardUniversity.BoardUniversityID | tblEmpEducation.BoardUniversityId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:126 | INFERRED |
| tblEducationSubjectGroup.EducationSubjectGroupID | tblEmpEducation.SubjectGroupId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:126 | INFERRED |
| tblEducationalInstitution.InstitutionID | tblEmpEducation.EducationalInstituteId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:126 | INFERRED |
| tblEmpEducation.FieldOfSpecializationId | tblSpecialization.SpecializationID | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:126 | INFERRED |
| tblEmpInfoTrainingType.TrainingTypeID | tblEmpTraining.TrainingType | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblEmpTraining.TrainingInstitution | tblEmpTrainingInstitution.InstitutionID | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblCountry.CountryID | tblEmpTraining.TrainingCountry | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblTrainingRecordMaster.TrainingRecordMasterId | tbl_trainingRecordDetailsEmployee.TrainingRecordMasterId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblTrainingRecordMaster.TrainingTypeId | tblTrainingType.TrainingTypeID | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblTrainingOrgInfo.TrainingOrgId | tblTrainingRecordMaster.TrainingOrgId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblSMCTrainingVenue.SMCVenueID | tblTrainingRecordMaster.TrainingVenue | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |
| tblOfficeBranch.TrainingOrgId | tblTrainingRecordMaster.TrainingOrgId | 1 | Library.DAL/UserProfileDAL/ChangePasswordDAL.cs:166 | INFERRED |

## Naming-based master/detail pairs (INFERRED)

| Master | Detail | Basis |
|---|---|---|
| tblDCPicking | tblDCPickingDetail | naming |
| tblDoctorDegree | tblDoctorDegreeDetail | naming |
| tblDoctorProgramType | tblDoctorProgramTypeDetail | naming |
| tblDoctorSpeciality | tblDoctorSpecialityDetail | naming |
| tblInvoice | tblInvoiceDetail | naming |
| tblOrder | tblOrderDetail | naming |
| tblReturnInvoice | tblReturnInvoiceDetail | naming |
