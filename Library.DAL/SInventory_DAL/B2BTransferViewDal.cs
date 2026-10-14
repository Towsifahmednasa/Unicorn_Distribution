using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    public class B2BTransferViewDal
    {

        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();

       

        public DataTable GetB2bTransferInfo(string prameter)
        {
            string query = @"SELECT ChalanId,ChalanNo,ChalanDate,FromComUnitName,ToComUnitName,TotalValue,TotalVat,GrandTotal,
                             CASE WHEN IsDeliver = 'True' THEN 'Received' ELSE 'Pending' END AS Status
                             FROM dbo.tblChalanInfo WHERE ChalanId IS NOT NULL " + prameter + " ORDER BY Status";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable GetChalanPrintHeader(string chalanNo)
        {
            string query = @"SELECT ChalanNo,ChalanDate,TrackNo,DriverName,FromComUnitName,FromComUnitAddress,ToComUnitName,ToComUnitAddress,
                             TotalValue,TotalVat,GrandTotal,IsDeliver
                             FROM dbo.tblChalanInfo WHERE ChalanNo = @ChalanNo";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(chalanNo))
            });
        }

        public DataTable GetChalanPrintLines(string chalanNo)
        {
            string query = @"SELECT D.ProductCode, ISNULL(P.ProductName, D.ProductName) AS ProductName, D.BatchNo, DS.MfgDate, DS.ExpDate,
                             D.Quantity, D.UnitPrice, D.Value, D.Vat, D.ValueWVat
                             FROM dbo.tblChalanInfo C
                             INNER JOIN dbo.tblChalanDetail D ON C.ChalanId = D.ChalanId
                             LEFT JOIN dbo.tblDCStore DS ON D.DCStoreId = DS.DCStoreId
                             LEFT JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode
                             WHERE C.ChalanNo = @ChalanNo ORDER BY D.ChalanDetailsId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ChalanNo", SInventorySql.DbValue(chalanNo))
            });
        }

        public bool DeleteChallanMasterById(int chalanId)
        {
            string query = @"DELETE FROM dbo.tblChalanInfo WHERE ChalanId = @ChalanId";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ChalanId", chalanId)
            };
            return SInventorySql.Execute(query, parameters);
        }

        //public bool DeleteChallanDetailById(int chalanId)
        //{
        //    string query = @"DELETE FROM dbo.tblChalanDetail WHERE ChalanId = " + chalanId;
        //    return aCommonInternalDal.DeleteDataByDeleteCommand(query, "SSIDB");
        //}


        public bool DeleteChallanDetailById(int chalanId)
        {
            var aSqlParameters = new List<SqlParameter>();

            aSqlParameters.Add(new SqlParameter("@chalanId", chalanId));

            return aCommonInternalDal.DeleteAction("sp_Del_B2BDelete", aSqlParameters);
        }
    }
}
