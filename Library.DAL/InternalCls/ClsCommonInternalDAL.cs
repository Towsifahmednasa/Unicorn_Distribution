using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Library.DAL.InternalCls
{
    public class ClsCommonInternalDAL
    {


        internal void LoadDropDownValueWithoutDataBase(DropDownList ddl, string displayField, string valueField, string queryString)
        {
            try
            {
                DataTable dataDDL = new DataTable();
                Database db;
                DbCommand dbCommand;
                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                dataDDL = db.ExecuteDataSet(dbCommand).Tables[0];
                ddl.DataTextField = displayField;
                ddl.DataValueField = valueField;
                ddl.DataSource = dataDDL;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--------Select---------", String.Empty));
                ddl.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        internal void LoadDropDownValueWithoutDataBase(DropDownList ddl, string displayField, string valueField, string queryString, List<SqlParameter> parameters)
        {
            DataTable dataDDL = new DataTable();
            Database db = null;
            DbCommand dbCommand = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand.CommandTimeout = 99000000;
                if (parameters != null && parameters.Count > 0)
                    dbCommand.Parameters.AddRange(parameters.ToArray());

                dataDDL = db.ExecuteDataSet(dbCommand).Tables[0];
                ddl.DataTextField = displayField;
                ddl.DataValueField = valueField;
                ddl.DataSource = dataDDL;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--------Select---------", String.Empty));
                ddl.SelectedIndex = 0;
            }
            finally
            {
                if (dbCommand != null)
                {
                    dbCommand.Parameters.Clear();
                    dbCommand.Dispose();
                }
            }
        }
        internal bool DeleteAction(string StoreProcedureName, List<SqlParameter> SqlParameterlist)
        {
            try
            {
                ActionStatus =
                    Convert.ToBoolean(ExecuteNonQueryAction(StoreProcedureName, SqlParameterlist, string.Empty, false));
            }
            catch (Exception ex)
            {
                ActionStatus = false;
            }
            return ActionStatus;
        }
        internal DataTable GetDataTableAction(string StoreProcedureName, string dataBaseName)
        {
            DataTable dt = new DataTable();

            try
            {
                Database db;
                DbCommand dbCommand;

                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                dbCommand = db.GetStoredProcCommand(StoreProcedureName);
                dbCommand.CommandTimeout = 99000000;
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// WARNING: Caller MUST close the returned IDataReader (Dispose) to avoid leaks.
        internal IDataReader DataContainerDataReader(string queryString)
        {
            Database db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
            DbCommand cmd = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
            cmd.CommandTimeout = 60;
            db.AddInParameter(cmd, "Query", DbType.String, queryString);

            // ExecuteReader থেকে reader close করলে cmd/connection-ও বন্ধ হবে।
            // Caller must: using (var r = dal.DataContainerDataReader(sql)) { ... }
            return db.ExecuteReader(cmd);
        }

        internal DataTable DataContainerDataTable(string queryString)
        {
            DataTable table = new DataTable();
            Database db = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                cmd = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                cmd.CommandTimeout = 90; // <= keep sane
                db.AddInParameter(cmd, "Query", DbType.String, queryString);

                ds = db.ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0)
                    table = ds.Tables[0];
            }
            finally
            {
                if (ds != null) ds.Dispose();
                if (cmd != null) cmd.Dispose();
                // Enterprise Library Database dispose দরকার হয় না
            }

            return table;
        }

        internal int SaveDataByInsertCommandWithIdentity(string queryString, List<SqlParameter> aSqlParameterlist, string dataBaseName)
        {
            int pk = 0;
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                cmd = db.GetSqlStringCommand(queryString + "; SELECT @ID = SCOPE_IDENTITY();");
                cmd.CommandTimeout = 60;

                cmd.Parameters.Clear();
                if (aSqlParameterlist != null && aSqlParameterlist.Count > 0)
                    cmd.Parameters.AddRange(aSqlParameterlist.ToArray());

                db.AddOutParameter(cmd, "@ID", DbType.Int32, 20);

                if (db.ExecuteNonQuery(cmd) > 0)
                    pk = Convert.ToInt32(cmd.Parameters["@ID"].Value);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }

            return pk;
        }


        internal bool SaveDataByInsertCommand(string queryString)
        {
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                cmd = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                cmd.CommandTimeout = 60;
                db.AddInParameter(cmd, "Query", DbType.String, queryString);

                return db.ExecuteNonQuery(cmd) > 0;
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
            }
        }


        internal bool UpdateDataByUpdateCommandNew(string queryString, List<SqlParameter> aSqlParameterlist)
        {
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                cmd = db.GetSqlStringCommand(queryString);
                cmd.CommandTimeout = 60;

                cmd.Parameters.Clear();
                if (aSqlParameterlist != null && aSqlParameterlist.Count > 0)
                    cmd.Parameters.AddRange(aSqlParameterlist.ToArray());

                return db.ExecuteNonQuery(cmd) > 0;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
        }

        internal bool UpdateDataByUpdateCommand(string queryString)
        {
            try
            {
                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                if (db.ExecuteNonQuery(dbCommand) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal bool UpdateDataByUpdateCommand(string queryString, List<SqlParameter> aSqlParameterlist, string dataBaseName)
        {
            try
            {
                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand.Parameters.Clear();
                dbCommand.Parameters.AddRange(aSqlParameterlist.ToArray());
                if (db.ExecuteNonQuery(dbCommand) > 0)
                {
                    dbCommand.Parameters.Clear();
                    return true;
                }
                else
                {
                    dbCommand.Parameters.Clear();
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal void LoadDropDownValue(DropDownList ddl, string displayField, string valueField, string queryString, string dataBaseName)
        {
            Database db = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                cmd = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                cmd.CommandTimeout = 60;
                db.AddInParameter(cmd, "Query", DbType.String, queryString);

                ds = db.ExecuteDataSet(cmd);
                DataTable dt = (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : new DataTable();

                ddl.DataTextField = displayField;
                ddl.DataValueField = valueField;
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("--------Select---------", string.Empty));
                ddl.SelectedIndex = 0;
            }
            finally
            {
                if (ds != null) ds.Dispose();
                if (cmd != null) cmd.Dispose();
            }
        }

        internal IDataReader DataContainerDataReader(string queryString, string dataBaseName)
        {
            try
            {
                IDataReader dataReader;
                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                dataReader = db.ExecuteReader(dbCommand);
                return dataReader;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal DataTable DataContainerDataTable(string queryString, string dataBaseName)
        {
            DataTable table = new DataTable();
            Database db = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                cmd = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                cmd.CommandTimeout = 90;
                db.AddInParameter(cmd, "Query", DbType.String, queryString);

                ds = db.ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0)
                    table = ds.Tables[0];
            }
            finally
            {
                if (ds != null) ds.Dispose();
                if (cmd != null) cmd.Dispose();
            }

            return table;
        }

        internal DataTable DataContainerDataTable(string queryString, List<SqlParameter> parameters, string dataBaseName)
        {
            DataTable table = new DataTable();
            Database db = null;
            DbCommand cmd = null;
            DataSet ds = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                cmd = db.GetSqlStringCommand(queryString);
                cmd.CommandTimeout = 90;

                if (parameters != null && parameters.Count > 0)
                    cmd.Parameters.AddRange(parameters.ToArray());

                ds = db.ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0)
                    table = ds.Tables[0];
            }
            finally
            {
                if (ds != null) ds.Dispose();
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }

            return table;
        }

        internal bool SaveDataByInsertCommand(string queryString, string dataBaseName)
        {
            try
            {
                Database db;
                DbCommand dbCommand;
                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                if (db.ExecuteNonQuery(dbCommand) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal bool SaveDataByInsertCommand(string queryString, List<SqlParameter> parameters, string dataBaseName)
        {
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                cmd = db.GetSqlStringCommand(queryString);
                cmd.CommandTimeout = 60;

                if (parameters != null && parameters.Count > 0)
                    cmd.Parameters.AddRange(parameters.ToArray());

                return db.ExecuteNonQuery(cmd) > 0;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }
        }

        internal bool UpdateAction(string StoreProcedureName, List<SqlParameter> SqlParameterlist)
        {
            try
            {
                ActionStatus =
                    Convert.ToBoolean(ExecuteNonQueryAction(StoreProcedureName, SqlParameterlist, string.Empty, false));
            }
            catch (Exception ex)
            {

                ActionStatus = false;
            }

            return ActionStatus;
        }
        internal bool UpdateDataByUpdateCommand(string queryString, string dataBaseName)
        {
            try
            {
                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                if (db.ExecuteNonQuery(dbCommand) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
               // throw ex;
            }
        }

        internal bool DeleteDataByDeleteCommand(string queryString)
        {
            try
            {
                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                if (db.ExecuteNonQuery(dbCommand) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal bool DeleteDataByDeleteCommand(string queryString, string dataBaseName)
        {
            try
            {
                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                //dbCommand = db.GetSqlStringCommand(queryString);
                dbCommand = db.GetStoredProcCommand("ExecuteAllSqlQueryByStoreProcedure");
                dbCommand.CommandTimeout = 99000000;
                db.AddInParameter(dbCommand, "Query", DbType.String, queryString);
                if (db.ExecuteNonQuery(dbCommand) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal int RunStoreProcedure(string sp, List<SqlParameter> aSqlParameterlist, string dataBaseName)
        {
            int pk = 0;
            try
            {
                DataTable dataContain = new DataTable();

                Database db;
                DbCommand dbCommand;

                //Prepare Database Call
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                using (dbCommand = db.GetStoredProcCommand(sp))
                {
                    dbCommand.CommandTimeout = 99000000;
                    dbCommand.Parameters.Clear();
                    dbCommand.Parameters.AddRange(aSqlParameterlist.ToArray());

                    if (db.ExecuteNonQuery(dbCommand) > 0)
                    {
                        pk = 1;
                        return pk;
                    }
                    else
                    {
                        dbCommand.Parameters.Clear();
                        return pk;
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // SP Insert Method
        bool ActionStatus;
        internal int SaveAction(string StoreProcedureName, List<SqlParameter> SqlParameterlist,
            string PrimaryKeyParameter)
        {
            int pk = 0;
            try
            {
                pk = ExecuteNonQueryAction(StoreProcedureName, SqlParameterlist, PrimaryKeyParameter, true);
            }
            catch (Exception ex)
            {
                pk = 0;
            }
            return pk;
        }
        private int ExecuteNonQueryAction(string storeProcedureName, List<SqlParameter> sqlParameterList,
                                     string primaryKeyParameter, bool isPrimaryKey)
        {
            int pk = 0;
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionStringSSIDB");
                cmd = db.GetStoredProcCommand(storeProcedureName);
                cmd.CommandTimeout = 60;

                cmd.Parameters.Clear();
                if (sqlParameterList != null && sqlParameterList.Count > 0)
                    cmd.Parameters.AddRange(sqlParameterList.ToArray());

                if (isPrimaryKey && !string.IsNullOrEmpty(primaryKeyParameter))
                    db.AddOutParameter(cmd, primaryKeyParameter, DbType.Int32, 10);

                if (db.ExecuteNonQuery(cmd) > 0)
                {
                    if (isPrimaryKey && !string.IsNullOrEmpty(primaryKeyParameter))
                        pk = Convert.ToInt32(cmd.Parameters[primaryKeyParameter].Value);
                    else
                        pk = 1;
                }
                else
                {
                    pk = 0;
                }
            }
            catch
            {
                // Preserve original stack trace for .NET 4
                throw;
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                }
            }

            return pk;
        }


        internal DataTable GetDataTableAction(string StoreProcedureName, List<SqlParameter> SqlParameterlist, string dataBaseName)
        {
            DataTable dt = new DataTable();
            Database db;
            DbCommand dbCommand = null;

            try
            {

                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                dbCommand = db.GetStoredProcCommand(StoreProcedureName);
                dbCommand.CommandTimeout = 99000000;
                dbCommand.Parameters.AddRange(SqlParameterlist.ToArray());
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
                dbCommand.Parameters.Clear();
            }
            catch (Exception ex)
            {
                if (dbCommand != null) dbCommand.Parameters.Clear();
                throw;
            }
            return dt;
        }
        internal DataTable GetDataTableUsingReader(string storeProcedureName, List<SqlParameter> sqlParameterList, string dataBaseName)
        {
            DataTable dt = new DataTable();
            Database db = null;
            DbCommand cmd = null;
            IDataReader reader = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("SolutionConnectionString" + dataBaseName);
                cmd = db.GetStoredProcCommand(storeProcedureName);
                cmd.CommandTimeout = 60;

                if (sqlParameterList != null && sqlParameterList.Count > 0)
                    cmd.Parameters.AddRange(sqlParameterList.ToArray());

                reader = db.ExecuteReader(cmd);
                dt.Load(reader);
            }
            finally
            {
                if (reader != null) reader.Dispose();
                if (cmd != null) cmd.Parameters.Clear();
                if (cmd != null) cmd.Dispose();
            }

            return dt;
        }


    }
}
