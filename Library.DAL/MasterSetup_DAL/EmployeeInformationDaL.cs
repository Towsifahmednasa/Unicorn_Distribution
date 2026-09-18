using Library.DAL.DataManager;
using SalesSolution.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Library.DAL.MasterSetup_DAL
{
   public class EmployeeInformationDaL
    {
        private DataAccessManager  accessManager = new DataAccessManager ();



        public int SaveEmployeeInformation(EmployeeInformation employee, string MonthlyArray, string sessionUser, int MyPK, IEnumerable<int> shiftIds = null, IEnumerable<int> productLineIds = null)
        {
            int pk = 0;
            ResultInfo aInformation = new ResultInfo();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;

                gSqlParameterList.Add(new SqlParameter("@EmpInfoId", employee.EmpInfoId ));
                gSqlParameterList.Add(new SqlParameter("@CompanyId", employee.CompanyId ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@CompanyUnitId", employee.CompanyUnitId ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@EmpName", employee.EmpName ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@EmpMasterCode", employee.EmpMasterCode ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@FatherName", employee.FatherName ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@MotherName", employee.MotherName ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@Religion", employee.Religion ?? (object)DBNull.Value));

                gSqlParameterList.Add(new SqlParameter("@Nationality", employee.Nationality ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@NationalIdNo", employee.NationalIdNo ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@BloodGroup", employee.BloodGroup ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@Gender", employee.Gender ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@AddressPresent", employee.AddressPresent ?? (object)DBNull.Value));

                gSqlParameterList.Add(new SqlParameter("@AddressPermanent", employee.AddressPermanent ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@CellNumber", employee.CellNumber ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@Email", employee.Email ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@EmrgContactNo", employee.EmrgContactNo ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@MaritalStatus", employee.MaritalStatus ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@RefName", employee.RefName ?? (object)DBNull.Value));

                gSqlParameterList.Add(new SqlParameter("@RefContactNo", employee.RefContactNo ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@DesignationId", employee.DesignationId ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@DepartmentId", employee.DepartmentId ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@ShiftId", employee.ShiftId ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@JoiningDate", employee.JoiningDate ?? (object)DBNull.Value));

                gSqlParameterList.Add(new SqlParameter("@JobLeftDate", employee.JobLeftDate ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@LastCompanyName", employee.LastCompanyName ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@LastJobLocation", employee.LastJobLocation ?? (object)DBNull.Value));

                gSqlParameterList.Add(new SqlParameter("@EmployeeStatus", employee.EmployeeStatus ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@EmrgContactNoRelaton", employee.EmrgContactNoRelaton ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@IsProbition", employee.IsProbition ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@IsTempEmployeeCode", employee.IsTempEmployeeCode ?? (object)DBNull.Value));
                gSqlParameterList.Add(new SqlParameter("@ProbitionEndDate", employee.ProbitionEndDate ?? (object)DBNull.Value));

                if (employee.EmpInfoId > 0)
                {
                    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();

                    aSqlParameterlist.Add(new SqlParameter("@id", employee.EmpInfoId));
                    aSqlParameterlist.Add(new SqlParameter("@Name", employee.EmpMasterCode));
                    DataTable dt = accessManager.GetDataTable("sp_check_EmployeeInfo", aSqlParameterlist);
                    if (dt.Rows.Count == 0)
                    {
                        gSqlParameterList.Add(new SqlParameter("@UpdateBy", sessionUser));



                        bool result = accessManager.UpdateData("sp_Update_EmployeeInformation", gSqlParameterList);
                        aInformation.isSuccess = result;
                        pk = employee.EmpInfoId;
                        MyPK= pk ;
                    }
                }
                else
                {
                    gSqlParameterList.Add(new SqlParameter("@EntryBy", sessionUser));
                    pk = accessManager.SaveDataReturnPrimaryKey("sp_Save_EmployeeInformation", gSqlParameterList);
                    if (pk > 0)
                    {
                        MyPK = pk;


                        aInformation.isSuccess = true;
                    }
                }


                if (aInformation.isSuccess)
                {
                    if (pk > 0)
                    {
                        IEnumerable<int> employeeShiftIds = shiftIds
                            ?? (employee.ShiftId.HasValue
                                ? new[] { employee.ShiftId.Value }
                                : Enumerable.Empty<int>());

                        aInformation.isSuccess = SyncEmployeeShiftDeclareInternal(pk, employeeShiftIds);
                        aInformation.isSuccess = aInformation.isSuccess && SyncEmployeeProductLineDeclareInternal(pk, productLineIds ?? Enumerable.Empty<int>());

                        if (MonthlyArray != "")
                        {
                            string[] degreeList = MonthlyArray.Split(',');

                            foreach (String item in degreeList)
                            {
                                if (item != "")
                                {
                                    List<SqlParameter> aSQL = new List<SqlParameter>();
                                    aSQL.Add(new SqlParameter("@EmpInfoId", pk));
                                    aSQL.Add(new SqlParameter("@AllowanceId", item ?? (object)DBNull.Value));
                                    aInformation.isSuccess = accessManager.SaveData("sp_Save_EmployeeAllowanceDetail", aSQL);
                                }
                            }



                        }
                    }
                }

               

            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                aInformation.isSuccess = false;
                aInformation.ErrorMessage = exception.Message;

                throw exception;
            }
            finally
            {

                accessManager.SqlConnectionClose();
            }

            return pk;
        }

        public List<int> GetEmployeeShiftIds(int empInfoId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@EmpInfoId", empInfoId)
                };

                DataTable dt = accessManager.GetDataTableByText(
                    "SELECT ShiftId FROM dbo.tblEmpwiseShiftDeclare WHERE EmpInfoId = @EmpInfoId",
                    parameters);

                return dt.AsEnumerable()
                    .Select(row => row.Field<int>("ShiftId"))
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public List<int> GetEmployeeProductLineIds(int empInfoId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@EmpInfoId", empInfoId)
                };

                DataTable dt = accessManager.GetDataTableByText(
                    "SELECT ProductLineID FROM dbo.tblEmpwiseProductLineDeclare WHERE EmpInfoId = @EmpInfoId",
                    parameters);

                return dt.AsEnumerable()
                    .Select(row => row.Field<int>("ProductLineID"))
                    .Distinct()
                    .ToList();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        private bool SyncEmployeeShiftDeclareInternal(int empInfoId, IEnumerable<int> shiftIds)
        {
            var targetShiftIds = new HashSet<int>((shiftIds ?? Enumerable.Empty<int>()).Distinct());
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmpInfoId", empInfoId)
            };

            DataTable dt = accessManager.GetDataTableByText(
                "SELECT ShiftId FROM dbo.tblEmpwiseShiftDeclare WHERE EmpInfoId = @EmpInfoId",
                parameters);

            var existingShiftIds = new HashSet<int>(
                dt.AsEnumerable().Select(row => row.Field<int>("ShiftId")));

            foreach (int shiftId in existingShiftIds.Except(targetShiftIds))
            {
                accessManager.ExecuteNonQueryByText(
                    "DELETE FROM dbo.tblEmpwiseShiftDeclare WHERE EmpInfoId = @EmpInfoId AND ShiftId = @ShiftId",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@EmpInfoId", empInfoId),
                        new SqlParameter("@ShiftId", shiftId)
                    });
            }

            foreach (int shiftId in targetShiftIds.Except(existingShiftIds))
            {
                accessManager.ExecuteNonQueryByText(
                    "INSERT INTO dbo.tblEmpwiseShiftDeclare (EmpInfoId, ShiftId) VALUES (@EmpInfoId, @ShiftId)",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@EmpInfoId", empInfoId),
                        new SqlParameter("@ShiftId", shiftId)
                    });
            }

            return true;
        }

        private bool SyncEmployeeProductLineDeclareInternal(int empInfoId, IEnumerable<int> productLineIds)
        {
            var targetProductLineIds = new HashSet<int>((productLineIds ?? Enumerable.Empty<int>()).Distinct());
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@EmpInfoId", empInfoId)
            };

            DataTable dt = accessManager.GetDataTableByText(
                "SELECT ProductLineID FROM dbo.tblEmpwiseProductLineDeclare WHERE EmpInfoId = @EmpInfoId",
                parameters);

            var existingProductLineIds = new HashSet<int>(
                dt.AsEnumerable().Select(row => row.Field<int>("ProductLineID")));

            foreach (int productLineId in existingProductLineIds.Except(targetProductLineIds))
            {
                accessManager.ExecuteNonQueryByText(
                    "DELETE FROM dbo.tblEmpwiseProductLineDeclare WHERE EmpInfoId = @EmpInfoId AND ProductLineID = @ProductLineID",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@EmpInfoId", empInfoId),
                        new SqlParameter("@ProductLineID", productLineId)
                    });
            }

            foreach (int productLineId in targetProductLineIds.Except(existingProductLineIds))
            {
                accessManager.ExecuteNonQueryByText(
                    "INSERT INTO dbo.tblEmpwiseProductLineDeclare (EmpInfoId, ProductLineID) VALUES (@EmpInfoId, @ProductLineID)",
                    new List<SqlParameter>
                    {
                        new SqlParameter("@EmpInfoId", empInfoId),
                        new SqlParameter("@ProductLineID", productLineId)
                    });
            }

            return true;
        }

        public ResultInfo SaveNoticeImage(string path, string name)
        {
            int pk = 0;
            ResultInfo aInformation = new ResultInfo();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;

                //gSqlParameterList.Add(new SqlParameter("@NoticeImageId", master.NoticeId));
                //gSqlParameterList.Add(new SqlParameter("@ImageName", master.NoticeTitle));
                //gSqlParameterList.Add(new SqlParameter("@ImagePath", master.Announcement));

                //if (master.NoticeId > 0)
                //{
                //    gSqlParameterList.Add(new SqlParameter("@UpdateBy", sessionUser));
                //    bool result = accessManager.UpdateData("sp_Update_NoticeMaster", gSqlParameterList);
                //    aInformation.isSuccess = result;
                //    pk = master.NoticeId;
                //}
                //else
                //{
                //    gSqlParameterList.Add(new SqlParameter("@EntryBy", sessionUser));
                //    pk = accessManager.SaveDataReturnPrimaryKey("sp_Save_NoticeMaster", gSqlParameterList);
                //    if (pk > 0)
                //    {
                //        aInformation.isSuccess = true;
                //    }
                //}
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                aInformation.isSuccess = false;
                aInformation.ErrorMessage = exception.Message;

                throw exception;
            }
            finally
            {

                accessManager.SqlConnectionClose();
            }

            return aInformation;
        }





        public DataTable GetEmployeeInformationList(string param)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@Parm", param));

                DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationList_Prm", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public DataTable GetEmployeeInformationListForReport(string Param, string Month, string Year)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@Parm", Param));
                aSqlParameterlist.Add(new SqlParameter("@Month", Month));
                aSqlParameterlist.Add(new SqlParameter("@Year", Year));
                //DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListRpt_Final", aSqlParameterlist);

                DataTable dt = accessManager.GetDataTable("sp_RPT_MonthlyExpense", aSqlParameterlist,true);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public DataTable GetTourPlanSummaryReportReport(  string Month, string Year, string EmpInfoId, string UserRoleId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@EmpInfoId", EmpInfoId));
                aSqlParameterlist.Add(new SqlParameter("@UserRoleId", UserRoleId));
                aSqlParameterlist.Add(new SqlParameter("@Month", Month));
                aSqlParameterlist.Add(new SqlParameter("@Year", Year));
                //DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListRpt_Final", aSqlParameterlist);

                DataTable dt = accessManager.GetDataTable("sp_Get_TourPlanSummaryReport", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public DataTable GetEmployeeInfoOrdPermission(string Param)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@Parm", Param));
             
                //DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListRpt_Final", aSqlParameterlist);

                DataTable dt = accessManager.GetDataTable("sp_GET_EmployeeInfoOrdPermission", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public DataTable GetTourPlanReportNew(string Param, string Month, string Year)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@param", Param));
                //aSqlParameterlist.Add(new SqlParameter("@Month", Month));
                //aSqlParameterlist.Add(new SqlParameter("@Year", Year));
                //DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListRpt_Final", aSqlParameterlist);

                DataTable dt = accessManager.GetDataTable("sp_Get_TourPlanReportList", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }public DataTable GetTourPlanReport__(string EmpInfoId, string Month, string Year)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@EmpInfoId", EmpInfoId));
                aSqlParameterlist.Add(new SqlParameter("@Month", Month));
                aSqlParameterlist.Add(new SqlParameter("@Year", Year));
                //DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListRpt_Final", aSqlParameterlist);

                DataTable dt = accessManager.GetDataTable("sp_Get_TourPlanReportList__n", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }public DataTable GetTourPlanReportBal(string EmpInfoId, string Month, string Year)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@EmpInfoId", EmpInfoId));
                aSqlParameterlist.Add(new SqlParameter("@Month", Month));
                aSqlParameterlist.Add(new SqlParameter("@Year", Year));
                //DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListRpt_Final", aSqlParameterlist);

                DataTable dt = accessManager.GetDataTable("sp_Webapi_Get_TourPlanBalanceWithEmpInfo", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public DataTable GetTerritoryCodeByRoleTypeEmpId(string EmpID, string RoleType)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@EmpId", EmpID));
                aSqlParameterlist.Add(new SqlParameter("@RoleType", RoleType));
                DataTable dt = accessManager.GetDataTable("sp_Get_TerritoryCodeByRoleTypeEmpId", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        
        public DataTable GetAMDZSMListByTerritoryId(string TerritoryId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@TerritoryId", TerritoryId));
                
                DataTable dt = accessManager.GetDataTable("sp_Get_AMDZSMListByTerritoryId", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }



        public DataTable GetTerritoryCodeByRoleTypeEmpIdActive(string EmpID, string RoleType)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@EmpId", EmpID));
                aSqlParameterlist.Add(new SqlParameter("@RoleType", RoleType));
                DataTable dt = accessManager.GetDataTable("sp_Get_TerritoryCodeByRoleTypeEmpId_Active", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }


        public DataTable GetEmployeeInformationListForActive(string Param)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@Parm", Param));
                DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformationListActive", aSqlParameterlist);
                return dt;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }
        public ResultInfo Delete_EmployeeInformation(Int32 DeleteId)
        {
            int pk = 0;

            ResultInfo aInformation = new ResultInfo();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> gSqlParameterList = new List<SqlParameter>();
                DateTime entryDtae = DateTime.Now;
                gSqlParameterList.Add(new SqlParameter("@EmpInfoId", DeleteId));

                bool result = accessManager.DeleteData("sp_Delete_EmployeeInformation", gSqlParameterList);
                pk = DeleteId;
            }
            catch (Exception exception)
            {
                accessManager.SqlConnectionClose(true);
                aInformation.isSuccess = false;
                aInformation.ErrorMessage = exception.Message;

                throw exception;
            }
            finally
            {
                aInformation.isSuccess = true;
                accessManager.SqlConnectionClose();
            }

            return aInformation;
        }


        public DataTable GetEmployeeInfoForEdit(int id)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                EmployeeInformation master = new EmployeeInformation();
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@id", id));
                DataTable dt = accessManager.GetDataTable("sp_Get_EmployeeInformation_ById", aSqlParameters);
                //while (dr.Read())
                //{
                //    master.EmpInfoId = (int)dr["EmpInfoId"];
                //    master.CompanyId = (int)dr["CompanyId"];
                //    master.EmpName = dr["EmpName"].ToString();
                //    master.FatherName = dr["FatherName"].ToString();
                //    master.MotherName = dr["MotherName"].ToString();
                //    master.DateOfBirth = (DateTime)dr["DateOfBirth"];
                //    master.AddressPermanent = dr["AddressPermanent"].ToString();
                //    master.AddressPresent = dr["AddressPresent"].ToString();
                //    master.Gender = dr["Gender"].ToString();
                //    master.Religion = dr["Religion"].ToString();
                //    master.Nationality = dr["Nationality"].ToString();

                //    master.Gender = dr["Gender"].ToString();
                //    master.Religion = dr["Religion"].ToString();
                //    master.Nationality = dr["Nationality"].ToString();



                //    master.BloodGroup = dr["BloodGroup"].ToString();
                //    master.MaritalStatus = dr["MaritalStatus"].ToString();
                //    master.NationalIdNo = dr["NationalIdNo"].ToString();
                //    master.JoiningDate = (DateTime)dr["JoiningDate"];


                //    master.DepartmentId = (int)dr["DepartmentId"];
                //    master.DesignationId = (int)dr["DesignationId"];
                //    master.ShiftId = (int)dr["ShiftId"];

                //    master.CellNumber = dr["CellNumber"].ToString();
                //    master.Email = dr["Email"].ToString();
                //    master.RefName = dr["RefName"].ToString();
                //    master.RefContactNo = dr["RefContactNo"].ToString();
                //    master.EmrgContactNo = dr["EmrgContactNo"].ToString();

                //}
                return dt;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }


    }
}
