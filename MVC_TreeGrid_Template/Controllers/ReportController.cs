using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalesReport.Models;

namespace SalesReport.Controllers
{
    public class ReportController : Controller
    {
        private readonly string _connectionString;

        public ReportController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public IActionResult GetSalesmanCustomerVisitReportData(DateTime startDate, DateTime endDate, int? empInfoId, string filterStatus)
        {
            List<CoverageReportModel> reportList = new List<CoverageReportModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_MIS_GetSalesmanCustomerVisitReport", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date) { Value = startDate });
                        cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.Date) { Value = endDate });
                        
                        cmd.Parameters.Add(new SqlParameter("@SalesPersonMasterCode", SqlDbType.NVarChar, 50) 
                        { 
                            Value = empInfoId == null || empInfoId == 0 ? (object)DBNull.Value : empInfoId.ToString()
                        });
                        
                        cmd.Parameters.Add(new SqlParameter("@FilterStatus", SqlDbType.VarChar, 20) 
                        { 
                            Value = string.IsNullOrEmpty(filterStatus) || filterStatus == "All" ? (object)DBNull.Value : filterStatus 
                        });

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            foreach (DataRow row in dt.Rows)
                            {
                                CoverageReportModel item = new CoverageReportModel
                                {
                                    NodeID = row["NodeID"] != DBNull.Value ? row["NodeID"].ToString() : "",
                                    ParentNodeID = row["ParentNodeID"] != DBNull.Value ? row["ParentNodeID"].ToString() : null,
                                    Code = row["ID / Code"] != DBNull.Value ? row["ID / Code"].ToString() : "",
                                    Name = row["Salesman / Customer Name"] != DBNull.Value ? row["Salesman / Customer Name"].ToString() : "",
                                    TerritoryRegion = row["Territory / Region"] != DBNull.Value ? row["Territory / Region"].ToString() : "",
                                    TotalAssigned = row["Total Assigned"] != DBNull.Value ? row["Total Assigned"].ToString() : "",
                                    Visited = row["Visited (DCR Done)"] != DBNull.Value ? row["Visited (DCR Done)"].ToString() : "",
                                    Missed = row["Missed / No DCR"] != DBNull.Value ? row["Missed / No DCR"].ToString() : "",
                                    CoverageStatus = row["Coverage % / Status"] != DBNull.Value ? row["Coverage % / Status"].ToString() : "",
                                    TotalSalesAmount = row["Total Sales Amount (BDT)"] != DBNull.Value ? Convert.ToDecimal(row["Total Sales Amount (BDT)"]) : 0,
                                    ReportLevel = row["ReportLevel"] != DBNull.Value ? Convert.ToInt32(row["ReportLevel"]) : 2,
                                    IsGroup = row["IsGroup"] != DBNull.Value ? Convert.ToInt32(row["IsGroup"]) : 0
                                };
                                reportList.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Json(reportList);
        }
    }
}
