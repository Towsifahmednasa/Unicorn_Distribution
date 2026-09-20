# INT-13 - Excel file imports acting as integration points

| Import | Page | Target | Notes |
|---|---|---|---|
| Territory monthly target | `SInventory_UI/TargetExcelUpload.aspx` (PAGE-554, M14) | `usp_InsertTerritoryData` per row | validates mapped employee + duplicate month; non-atomic (BR-M14-021/022) |
| MIGO (SAP goods movement) | `MIGO_Upload.aspx` (PAGE-688) | `tblMIGOMaster/Detail` -> `sp_StockInMIGOtoCentralStore` | see INT-07 |
| Customer master | `CustomerExcelUpload` family (`ExcelUpForMIGODAL.CustomerXLDataGridToDbByRow` -> `sp_I_Customer`, `sp_VerifyCustomer`, `sp_CustomerTransfer`) | staging `tblCustomerMasterExcelFileMaster/Detail` | not in M14/M17 page set |
| Deposit slips / customer payments | `DepositSlipExcelUpload*.aspx`, `CustomerPaymentExcelUpload.aspx` | company deposit tables | map territory -> SAP employee code (`sp_Get_SAP_EmpSApCodebyTerritory`), bank -> SAP account (`sp_Get_BankSAPMappingAccounTNo`, `sp_GET_BAnkInfoById_ByIdExcel`), DIC deposit code (`sp_Get_DepositCodeByDIC`) - feeds SAP bank-deposit posting (INT-04) |

Common mechanics (CONFIRMED in TargetExcelUpload/MIGO_Upload): file saved to `~/ExcelFiles/<client file name>` without extension whitelist; OLEDB Jet 4.0 (`.xls`) / ACE 12.0 (`.xlsx`); first sheet only; `HDR=YES`; rows with empty first cell dropped; grid shown before commit; per-row inserts on separate commands. Auth: Session login only.
