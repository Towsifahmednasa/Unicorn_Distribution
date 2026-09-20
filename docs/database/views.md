# Database Views

**No `CREATE VIEW` definition exists in the repository.** Views are known only by name from inline SQL. Definitions/columns: UNKNOWN (must be scripted from the live database `SalesDisDB_Unicorn_DB`).

| View | Read by (class.method) | Modules of pages using it | Columns referenced (from SELECT lists: not extracted) |
|---|---|---|---|
| VIew_SubDepotCurrentStock | Sub_InvoiceDAL.ProductInfoDAL | — | UNKNOWN |
| View_CentralStoreCurrentStock | ChallanReportViewDAL.GetPickingDataForIssueDAL; CountryStockReportDAL.CountryReportWithoutProductCodeDetailDataDAL | M10 | UNKNOWN |
| View_CustomerForModification | CustomerMasterInfoDal.LoadCustomerMasterViewM | M05 | UNKNOWN |
| View_CustomerMaster | CompanySalesReportDAL.CustomerPaymentDAl; CompanySalesReportDAL.DeliveryReturnReportDAl; CompanySalesReportDAL.SalesReectionReportDAl; CompanySalesReportDAL.SalesReportDAl; CompanySalesReportDAL.SalesReportDAlParameter; CustPaymentDAL.LoadCustomerMaster | M05, M06, M07, M08, M09, M10, M11, M12, M13 | UNKNOWN |
| View_DCStoreCurrentStock | InvoiceDAL.ProductInfoDAL; SalesReturnDAL.ProductInfoDAL | M06, M07, M08, M09 | UNKNOWN |
| View_MIAWiseSalesReport | ComUnitSalesReportDAL.ComUnitReportDetailDataDAL; CompanySalesReportDAL.CompanyReportDetailDataDAL; CustomerSalesReportDAL.CustomerReportDetailDataDAL; DistrictSalesReportDAL.DistrictReportDetailDataDAL; MiaSalesReportDAL.MiaWiseReportDetailDataDAL; RegionSalesReportDAL.RegionReportDetailDataDAL | M13 | UNKNOWN |
| View_OrderCustomerInfo | InvoiceDAL.LoadCustomerMaster; SalesReturnDAL.LoadCustomerMaster | M06, M07, M08, M09 | UNKNOWN |
| View_ProDCStock | DCStockReportDAL.DCWiseCountryReportDetailDataDAL | M10 | UNKNOWN |
| View_TotalCurrentStockofCompanyWithStockInTransfar | CountryStockReportDAL.CountryReportDetailDataDAL | M10 | UNKNOWN |

## Notes

- `View_CustomerMaster` is the most-used object (216 textual references) and is the read model for customer lookups; `View_Webapi_EmployeeFieldForceInfo` (84 refs) feeds mobile/API endpoints (INFERRED from name).
- `View_*CurrentStock` views (`View_CentralStoreCurrentStock`, `View_DCStoreCurrentStock`, `VIew_SubDepotCurrentStock`, `View_TotalCurrentStockofCompanyWithStockInTransfar`, `View_ProDCStock`) are the stock read models for warehouse / DC / sub-depot / national stock (INFERRED from names).

