using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;

namespace Library.DAL.SInventory_DAL
{
    public class MaterialConversionReceiveDal
    {
        private readonly ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        private readonly DB_Manager aDbManager = new DB_Manager();

        public void CreateConnection_DAL()
        {
            aDbManager.CreateConnection("SalesDisDB_New3");
        }

        public void CloseAllConnection_DAL()
        {
            aDbManager.CloseConnection();
        }

        public DataTable LoadMaterialConversionList(string param)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@Param", param));

            string queryStr = @"SELECT 
                CD.MaterialConversionDetailId,
                CM.MaterialConversionMasterId,
                CM.MaterialConversionCode,
                ISNULL(NULLIF(LTRIM(RTRIM(FG.ProductCode)), ''), 'FG-' + CAST(CM.FGItemId AS VARCHAR(50))) AS ProductCode,
                ISNULL(NULLIF(FG.ProductId, 0), CAST(CM.FGItemId AS INT)) AS ProductId,
                ISNULL(NULLIF(LTRIM(RTRIM(FG.ProductName)), ''), 'FG Item ' + CAST(CM.FGItemId AS VARCHAR(50))) AS ProductName,
                ISNULL(NULLIF(LTRIM(RTRIM(FG.PackSize)), ''), 'N/A') AS PackSize,
                CM.CostPerUnit, 
                0 AS Vat,
                CAST(SUM(ABS(ISNULL(BS.Trans_Qty, CM.FinalOutputQuantity))) AS INT) AS FinalOutputQuantityInt,
                CAST(ROUND(SUM(ABS(ISNULL(BS.Trans_Qty, CM.FinalOutputQuantity))) * CM.CostPerUnit, 2) AS DECIMAL(18,2)) AS TotalCostOfThisBatch,
                BS.Batch_No,
                BS.Mfg_Date, 
                BS.Exp_Date
            FROM CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionMaster AS CM
            LEFT JOIN CSTL_ERPDB_Unicorn..tbl_MC_ProductionUnit AS PUNT ON CM.ProductionUnitId = PUNT.ProductionUnitId
            INNER JOIN CSTL_ERPDB_Unicorn..tbl_MC_RecipeMaster AS RCM ON CM.RecipeSetupMasterId = RCM.RecipeSetupMasterId
            LEFT JOIN CSTL_ERPDB_Unicorn..tblCompanyInfo AS CI ON CM.BusinessUnitId = CI.CompanyId
            INNER JOIN CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionDetails AS CD ON CM.MaterialConversionMasterId = CD.MaterialConversionMasterId
            INNER JOIN CSTL_ERPDB_Unicorn..tbl_INV_BatchWiseStock AS BS ON CD.MaterialConversionDetailId = BS.MaterialConversionDetailId
            LEFT JOIN (SELECT DISTINCT WD.MaterialConversionDetailId FROM tblWHStockInMaster AS WM
                INNER JOIN tblWHStockInDetail AS WD ON WM.WHStockInMasterID = WD.WHStockInMasterID
                WHERE ISNULL(WD.MaterialConversionDetailId,0) <> 0
                AND LTRIM(RTRIM(Upper(WM.Status))) != LTRIM(RTRIM(Upper('Reject')))) AS CS
                ON CD.MaterialConversionDetailId = CS.MaterialConversionDetailId
            LEFT JOIN tblProduct AS FG ON CM.FGItemId = FG.ProductId
            LEFT JOIN tblPackSize AS PKS ON FG.PackSizeId = PKS.PackSizeId
            WHERE CS.MaterialConversionDetailId IS NULL AND ISNULL(CM.FinalOutputQuantity,0) > 0  AND CM.MaterialConversionMasterId IS NOT NULL
            AND EXISTS (SELECT 1 FROM tbl_MC_ProductRawMaterialDetails AS MAP WHERE MAP.ProductId = CM.FGItemId) "
            + param + @"
            GROUP BY 
                CD.MaterialConversionDetailId,
                CM.MaterialConversionMasterId,
                CM.MaterialConversionCode,
                FG.ProductCode,
                FG.ProductId,
                CM.FGItemId,
                FG.ProductName,
                FG.PackSize,
                CM.CostPerUnit,
                BS.Batch_No,
                BS.Mfg_Date,
                BS.Exp_Date
            ORDER BY CM.MaterialConversionMasterId DESC, BS.Batch_No";

            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable LoadMaterialConversionListForEdit(string param, int stockInMasterId)
        {
            string queryStr = @"SELECT 
                CD.MaterialConversionDetailId,
                CM.MaterialConversionMasterId,
                CM.MaterialConversionCode,
                ISNULL(NULLIF(LTRIM(RTRIM(FG.ProductCode)), ''), 'FG-' + CAST(CM.FGItemId AS VARCHAR(50))) AS ProductCode,
                ISNULL(NULLIF(FG.ProductId, 0), CAST(CM.FGItemId AS INT)) AS ProductId,
                ISNULL(NULLIF(LTRIM(RTRIM(FG.ProductName)), ''), 'FG Item ' + CAST(CM.FGItemId AS VARCHAR(50))) AS ProductName,
                ISNULL(NULLIF(LTRIM(RTRIM(FG.PackSize)), ''), 'N/A') AS PackSize,
                CM.CostPerUnit, 
                0 AS Vat,
                CAST(SUM(ABS(ISNULL(BS.Trans_Qty, CM.FinalOutputQuantity))) AS INT) AS FinalOutputQuantityInt,
                CAST(ROUND(SUM(ABS(ISNULL(BS.Trans_Qty, CM.FinalOutputQuantity))) * CM.CostPerUnit, 2) AS DECIMAL(18,2)) AS TotalCostOfThisBatch,
                BS.Batch_No,
                BS.Mfg_Date, 
                BS.Exp_Date
            FROM CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionMaster AS CM
            LEFT JOIN CSTL_ERPDB_Unicorn..tbl_MC_ProductionUnit AS PUNT ON CM.ProductionUnitId = PUNT.ProductionUnitId
            INNER JOIN CSTL_ERPDB_Unicorn..tbl_MC_RecipeMaster AS RCM ON CM.RecipeSetupMasterId = RCM.RecipeSetupMasterId
            LEFT JOIN CSTL_ERPDB_Unicorn..tblCompanyInfo AS CI ON CM.BusinessUnitId = CI.CompanyId
            INNER JOIN CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionDetails AS CD ON CM.MaterialConversionMasterId = CD.MaterialConversionMasterId
            INNER JOIN CSTL_ERPDB_Unicorn..tbl_INV_BatchWiseStock AS BS ON CD.MaterialConversionDetailId = BS.MaterialConversionDetailId
            LEFT JOIN (SELECT DISTINCT WD.MaterialConversionDetailId FROM tblWHStockInMaster AS WM
                INNER JOIN tblWHStockInDetail AS WD ON WM.WHStockInMasterID = WD.WHStockInMasterID
                WHERE ISNULL(WD.MaterialConversionDetailId,0) <> 0
                AND LTRIM(RTRIM(Upper(WM.Status))) != LTRIM(RTRIM(Upper('Reject')))
                AND WM.WHStockInMasterID <> " + stockInMasterId + @") AS CS
                ON CD.MaterialConversionDetailId = CS.MaterialConversionDetailId
            LEFT JOIN tblProduct AS FG ON CM.FGItemId = FG.ProductId
            LEFT JOIN tblPackSize AS PKS ON FG.PackSizeId = PKS.PackSizeId
            WHERE CS.MaterialConversionDetailId IS NULL AND ISNULL(CM.FinalOutputQuantity,0) > 0  AND CM.MaterialConversionMasterId IS NOT NULL
            AND EXISTS (SELECT 1 FROM tbl_MC_ProductRawMaterialDetails AS MAP WHERE MAP.ProductId = CM.FGItemId) "
            + param + @"
            GROUP BY 
                CD.MaterialConversionDetailId,
                CM.MaterialConversionMasterId,
                CM.MaterialConversionCode,
                FG.ProductCode,
                FG.ProductId,
                CM.FGItemId,
                FG.ProductName,
                FG.PackSize,
                CM.CostPerUnit,
                BS.Batch_No,
                BS.Mfg_Date,
                BS.Exp_Date
            ORDER BY CM.MaterialConversionMasterId DESC, BS.Batch_No";

            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable LoadStockInMasterForEdit(int stockInMasterId)
        {
            string queryStr = @"SELECT TOP 1
            WM.WHStockInMasterID,
            WM.WarehouseId,
            WH.CompanyId,
            WM.WHStockInDate,
            WM.ChallanNo,
            WM.ChallanDate,
            WM.ReferenceNo,
            WM.ReferenceDate,
            WM.Remarks,
            WM.TotalQuantity,
            WM.TotalVat,
            WM.TotalValue,
            ISNULL(PU.ProductionUnitId, 0) AS ProductionUnitId
            FROM dbo.tblWHStockInMaster AS WM
            LEFT JOIN dbo.tblWearhouse AS WH ON WM.WarehouseId = WH.WearhouseId
            OUTER APPLY
            (
                SELECT TOP 1 CM.ProductionUnitId
                FROM dbo.tblWHStockInDetail AS WD
                LEFT JOIN CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionDetails AS CD ON WD.MaterialConversionDetailId = CD.MaterialConversionDetailId
                LEFT JOIN CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionMaster AS CM ON CD.MaterialConversionMasterId = CM.MaterialConversionMasterId
                WHERE WD.WHStockInMasterID = WM.WHStockInMasterID
                ORDER BY WD.WHStockInDetailID
            ) AS PU
            WHERE WM.WHStockInMasterID = '" + stockInMasterId + "'";

            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public DataTable LoadStockInDetailForEdit(int stockInMasterId)
        {
            string queryStr = @"SELECT
            ISNULL(CM.MaterialConversionMasterId, ISNULL(WM.MaterialConversionMasterId, 0)) AS MaterialConversionMasterId,
            ISNULL(CD.MaterialConversionDetailId, ISNULL(WD.MaterialConversionDetailId, 0)) AS MaterialConversionDetailId,
            ISNULL(CM.MaterialConversionCode, 'N/A') AS MaterialConversionCode,
            ISNULL(PD.ProductCode, '') AS ProductCode,
            WD.ProductId,
            ISNULL(PD.ProductName, '') AS ProductName,
            ISNULL(NULLIF(LTRIM(RTRIM(PD.PackSize)), ''), 'N/A') AS PackSize,
            ISNULL(WD.Price, 0) AS CostPerUnit,
            ISNULL(WD.VAT, 0) AS Vat,
            CAST(ISNULL(WD.Qty, 0) AS INT) AS FinalOutputQuantityInt,
            ISNULL(WD.TotalAmount, 0) AS TotalCostOfThisBatch,
            0 AS RawMaterialId,
            WD.Batch AS Batch_No,
            WD.MfgDate AS Mfg_Date,
            WD.ExpDate AS Exp_Date
            FROM dbo.tblWHStockInDetail AS WD
            LEFT JOIN dbo.tblWHStockInMaster AS WM ON WD.WHStockInMasterID = WM.WHStockInMasterID
            LEFT JOIN CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionDetails AS CD ON WD.MaterialConversionDetailId = CD.MaterialConversionDetailId
            LEFT JOIN CSTL_ERPDB_Unicorn..tbl_MC_MaterialConversionMaster AS CM ON ISNULL(CD.MaterialConversionMasterId, WM.MaterialConversionMasterId) = CM.MaterialConversionMasterId
            LEFT JOIN dbo.tblProduct AS PD ON WD.ProductId = PD.ProductId
            WHERE WD.WHStockInMasterID = '" + stockInMasterId + "' ORDER BY WD.WHStockInDetailID";

            return aCommonInternalDal.DataContainerDataTable(queryStr, "SSIDB");
        }

        public void LoadCompany(DropDownList ddl)
        {
            string queryStr = "SELECT * FROM dbo.tblCompanyInfo";
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "CompanyName", "CompanyId", queryStr);
        }

        public void GetProductionUnit(DropDownList ddl, string businessUnitId)
        {
            string queryStr = "SELECT ProductionUnitId,ProductionUnitName FROM CSTL_ERPDB_Unicorn..tbl_MC_ProductionUnit WHERE BusinessUnitId = " + businessUnitId;
            aCommonInternalDal.LoadDropDownValueWithoutDataBase(ddl, "ProductionUnitName", "ProductionUnitId", queryStr);
        }

        public DataTable GetMaterialConversionReceiveData()
        {
            string query = @"SELECT * FROM dbo.tblWHStockInMaster AS WSHM 
            WHERE (WSHM.Status = 'Posted' OR WSHM.Status = 'Reject') 
            AND WSHM.MaterialConversionMasterId IS NOT NULL 
            AND WSHM.MaterialConversionMasterId <> 0 
            ORDER BY WSHM.WHStockInMasterID DESC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }
}
