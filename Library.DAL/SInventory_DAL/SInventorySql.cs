using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Library.DAL.DataManager;
using Library.DAL.InternalCls;

namespace Library.DAL.SInventory_DAL
{
    internal static class SInventorySql
    {
        internal static object DbValue(object value)
        {
            return value ?? DBNull.Value;
        }

        internal static DataTable GetDataTable(string query, List<SqlParameter> parameters)
        {
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                return accessManager.GetDataTableByText(query, parameters);
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        internal static bool Exists(string query, List<SqlParameter> parameters)
        {
            return GetDataTable(query, parameters).Rows.Count > 0;
        }

        internal static bool Execute(string query, List<SqlParameter> parameters)
        {
            ClsCommonInternalDAL commonInternalDal = new ClsCommonInternalDAL();
            return commonInternalDal.UpdateDataByUpdateCommandNew(query, parameters);
        }
    }
}
