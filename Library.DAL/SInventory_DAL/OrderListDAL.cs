using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.DataManager;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class OrderListDAL
    {
        private readonly ClsApprovalAction approvalAction = new ClsApprovalAction();

        private static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, SInventorySql.DbValue(value));
        }

        private static void BindDropDown(DropDownList ddl, DataTable dataTable, string textField, string valueField)
        {
            ddl.DataSource = dataTable;
            ddl.DataTextField = textField;
            ddl.DataValueField = valueField;
            ddl.DataBind();
        }

        public void LoadApprovalControlDAL(DropDownList ddl, string pageName, string userName)
        {
            string actionConditionQuery = @"SELECT AD.ActionId,
                                                   AD.ActionCondition
                                            FROM dbo.tblActionUserWiseApproval UA
                                            INNER JOIN dbo.tblActionPageWiseStep PS ON UA.ManuSL = PS.ManuSL
                                            INNER JOIN dbo.tblActionSteps SA ON PS.ASId = SA.ASId
                                            INNER JOIN dbo.tblActionStepDetail AD ON SA.ASId = AD.ASId AND UA.ActionId = AD.ActionId
                                            INNER JOIN dbo.tblMainMenu MM ON UA.ManuSL = MM.SL
                                            WHERE UA.LoginName = @LoginName
                                              AND MM.URL LIKE '%' + @PageName + '%'";

            DataTable dtActionCondition = SInventorySql.GetDataTable(actionConditionQuery, new List<SqlParameter>
            {
                Param("@LoginName", userName),
                Param("@PageName", pageName)
            });

            DataTable dtAction = SInventorySql.GetDataTable(
                "SELECT ActionId, ActionText FROM dbo.tblAction WHERE IsShow = 1",
                new List<SqlParameter>());

            BindDropDown(ddl, dtAction, "ActionText", "ActionId");

            for (int i = 0; i < ddl.Items.Count; i++)
            {
                ddl.Items[i].Enabled = false;
            }

            if (dtActionCondition.Rows.Count == 0)
            {
                return;
            }

            string actionId = Convert.ToString(dtActionCondition.Rows[0]["ActionId"]);
            string actionCondition = Convert.ToString(dtActionCondition.Rows[0]["ActionCondition"]);

            for (int i = 0; i < ddl.Items.Count; i++)
            {
                bool isSelectedAction = string.Equals(ddl.Items[i].Value, actionId, StringComparison.OrdinalIgnoreCase);
                bool isCancel = string.Equals(ddl.Items[i].Text, "Cancel", StringComparison.OrdinalIgnoreCase);

                ddl.Items[i].Enabled = isSelectedAction || isCancel;

                if (string.Equals(ddl.Items[i].Text, actionCondition, StringComparison.OrdinalIgnoreCase))
                {
                    ddl.Items[i].Enabled = false;
                }
            }
        }

        public void LoadApprovalControlDAL(RadioButtonList rdl, string pageName, string userName)
        {
            approvalAction.LoadActionControlByUser(rdl, pageName, userName);
        }

        public string LoadForApprovalConditionDAL(string pageName, string userName)
        {
            return approvalAction.LoadForApprovalByUserCondition(pageName, userName);
        }

        public bool SaveOrderMaster(OrderInfoMaster aListMasterDao, out int orderId)
        {
            orderId = 0;
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);

                List<SqlParameter> paramList = new List<SqlParameter>
                {
                    new SqlParameter("@OrderCode", SqlDbType.NVarChar, 50) { Value = aListMasterDao.OrderCode ?? "", Direction = ParameterDirection.InputOutput },
                    new SqlParameter("@TerritoryCode", aListMasterDao.teritory ?? (object)DBNull.Value),
                    new SqlParameter("@ComUnitId", aListMasterDao.ComUnitId),
                    new SqlParameter("@ComUnitCode", aListMasterDao.ComUnitCode ?? (object)DBNull.Value),
                    new SqlParameter("@ComUnitName", aListMasterDao.ComUnitName ?? (object)DBNull.Value),
                    new SqlParameter("@MIOCode", aListMasterDao.MIOCode ?? (object)DBNull.Value),
                    new SqlParameter("@MIOName", aListMasterDao.MIOName ?? (object)DBNull.Value),
                    new SqlParameter("@MIOId", aListMasterDao.MIOId > 0 ? (object)aListMasterDao.MIOId : DBNull.Value),
                    new SqlParameter("@ManufacId", aListMasterDao.ManufacId),
                    new SqlParameter("@CustomerCode", aListMasterDao.CustomerCode ?? (object)DBNull.Value),
                    new SqlParameter("@CustomerName", aListMasterDao.CustomerName ?? (object)DBNull.Value),
                    new SqlParameter("@GrossValue", aListMasterDao.GrossValue),
                    new SqlParameter("@TotalVat", aListMasterDao.TotalVat),
                    new SqlParameter("@TotalDiscount", aListMasterDao.TotalDiscount),
                    new SqlParameter("@TotalNetPayable", aListMasterDao.TotalNetPayable > 0 ? aListMasterDao.TotalNetPayable : aListMasterDao.GrossValue),
                    new SqlParameter("@SubmissionDate", aListMasterDao.SubmissionDate),
                    new SqlParameter("@FixedCustomer", aListMasterDao.FCB),
                    new SqlParameter("@Remarks", aListMasterDao.Remarks ?? (object)DBNull.Value),
                    new SqlParameter("@OrderId", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                bool result = accessManager.SaveData("sp_Save_ManualOrderMaster", paramList);
                var orderIdParam = paramList.Find(p => p.ParameterName == "@OrderId");
                if (orderIdParam != null && orderIdParam.Value != DBNull.Value)
                {
                    orderId = Convert.ToInt32(orderIdParam.Value);
                    aListMasterDao.OrderId = orderId;
                }
                var orderCodeParam = paramList.Find(p => p.ParameterName == "@OrderCode");
                if (orderCodeParam != null && orderCodeParam.Value != DBNull.Value)
                {
                    aListMasterDao.OrderCode = orderCodeParam.Value.ToString();
                }
                return orderId > 0;
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public bool SaveOrderMaster(OrderInfoMaster aListMasterDao)
        {
            int orderId;
            return SaveOrderMaster(aListMasterDao, out orderId);
        }

        public bool UpdateOrderMaster(OrderInfoMaster aListMasterDao)
        {
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);

                List<SqlParameter> paramList = new List<SqlParameter>
                {
                    new SqlParameter("@OrderId", aListMasterDao.OrderId),
                    new SqlParameter("@TerritoryCode", aListMasterDao.teritory ?? (object)DBNull.Value),
                    new SqlParameter("@ComUnitId", aListMasterDao.ComUnitId),
                    new SqlParameter("@ComUnitCode", aListMasterDao.ComUnitCode ?? (object)DBNull.Value),
                    new SqlParameter("@ComUnitName", aListMasterDao.ComUnitName ?? (object)DBNull.Value),
                    new SqlParameter("@MIOCode", aListMasterDao.MIOCode ?? (object)DBNull.Value),
                    new SqlParameter("@MIOName", aListMasterDao.MIOName ?? (object)DBNull.Value),
                    new SqlParameter("@MIOId", aListMasterDao.MIOId > 0 ? (object)aListMasterDao.MIOId : DBNull.Value),
                    new SqlParameter("@CustomerCode", aListMasterDao.CustomerCode ?? (object)DBNull.Value),
                    new SqlParameter("@CustomerName", aListMasterDao.CustomerName ?? (object)DBNull.Value),
                    new SqlParameter("@GrossValue", aListMasterDao.GrossValue),
                    new SqlParameter("@TotalVat", aListMasterDao.TotalVat),
                    new SqlParameter("@TotalDiscount", aListMasterDao.TotalDiscount),
                    new SqlParameter("@TotalNetPayable", aListMasterDao.TotalNetPayable > 0 ? aListMasterDao.TotalNetPayable : aListMasterDao.GrossValue),
                    new SqlParameter("@SubmissionDate", aListMasterDao.SubmissionDate),
                    new SqlParameter("@FixedCustomer", aListMasterDao.FCB),
                    new SqlParameter("@Remarks", aListMasterDao.Remarks ?? (object)DBNull.Value)
                };

                return accessManager.SaveData("sp_Update_ManualOrderMaster", paramList);
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public bool SaveOrderDetail(OrderInfoDetail aOrderListDetailDao)
        {
            DataAccessManager accessManager = new DataAccessManager();
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);

                bool isGift = false;
                bool isCamp = false;
                bool.TryParse(aOrderListDetailDao.IsgiftProduct, out isGift);
                bool.TryParse(aOrderListDetailDao.IsCampaignProduct, out isCamp);

                List<SqlParameter> paramList = new List<SqlParameter>
                {
                    new SqlParameter("@ProductId", aOrderListDetailDao.ProductId),
                    new SqlParameter("@ProductCode", aOrderListDetailDao.ProductCode ?? (object)DBNull.Value),
                    new SqlParameter("@ProductName", aOrderListDetailDao.ProductName ?? (object)DBNull.Value),
                    new SqlParameter("@Quantity", aOrderListDetailDao.Quantity),
                    new SqlParameter("@TradePrice", aOrderListDetailDao.TradePrice),
                    new SqlParameter("@TotalTradePrice", aOrderListDetailDao.TotalTradePrice),
                    new SqlParameter("@ISGiftProduct", isGift),
                    new SqlParameter("@IsCampaignProduct", isCamp),
                    new SqlParameter("@OrderId", aOrderListDetailDao.OrderId),
                    new SqlParameter("@UnitVatAmount", aOrderListDetailDao.VatAmountPerUnit),
                    new SqlParameter("@TotalVatAmount", aOrderListDetailDao.TotalVatAmount),
                    new SqlParameter("@NetAmount", aOrderListDetailDao.NetAmount > 0 ? aOrderListDetailDao.NetAmount : aOrderListDetailDao.TotalTradePrice)
                };

                return accessManager.SaveData("sp_Save_ManualOrderDetail", paramList);
            }
            finally
            {
                accessManager.SqlConnectionClose();
            }
        }

        public void LoadSalesPerson(DropDownList ddl)
        {
            string queryStr = @"SELECT emp.EmpInfoId, (emp.EmpMasterCode + ' : ' + emp.EmpName) AS EmpDisplayName 
                                FROM dbo.tblEmpGeneralInfo emp WITH (NOLOCK)
                                ORDER BY emp.EmpName ASC";
            BindDropDown(ddl, SInventorySql.GetDataTable(queryStr, new List<SqlParameter>()), "EmpDisplayName", "EmpInfoId");
            ddl.Items.Insert(0, new ListItem("-- Select Sales Person --", ""));
        }

        public void LoadmanufacturerName(DropDownList ddl)
        {
            string queryStr = "select * from tblManufacturer";
            BindDropDown(ddl, SInventorySql.GetDataTable(queryStr, new List<SqlParameter>()), "ManufacName", "ManufacId");
            ddl.Items.Insert(0, new ListItem("-- Select Manufacturer --", ""));
        }

        public DataTable CustomerInfo(string custCode)
        {
            string query = @"SELECT * FROM dbo.View_CustomerMaster WHERE CustomerCode=@CustomerCode";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@CustomerCode", custCode)
            });
        }

        public int OrderManualId()
        {
            string query = @"SELECT (isnull(MAX(SUBSTRING(OrderCode,5,15)),0)+1) as PKMaxNo FROM dbo.tblOrder WHERE IsManual='True'";
            return Convert.ToInt32(SInventorySql.GetDataTable(query, new List<SqlParameter>()).Rows[0][0].ToString());
        }

        public void DCLoad(DropDownList aDownList)
        {
            string dc = "select ComUnitId, (ComUnitCode+':'+ComUnitName) as Com from dbo.tblCompanyUnit";
            BindDropDown(aDownList, SInventorySql.GetDataTable(dc, new List<SqlParameter>()), "Com", "ComUnitId");
            aDownList.Items.Insert(0, new ListItem("-- Select Distribution Center --", ""));
        }

        public DataTable GetDCBySalesPerson(int empInfoId)
        {
            string query = @"SELECT DISTINCT cu.ComUnitId, (cu.ComUnitCode + ' : ' + cu.ComUnitName) AS ComDisplayName
                             FROM dbo.tblUserCompanyUnit ucu WITH (NOLOCK)
                             INNER JOIN dbo.tblUser u WITH (NOLOCK) ON ucu.UserId = u.UserId
                             INNER JOIN dbo.tblCompanyUnit cu WITH (NOLOCK) ON ucu.CompanyUnitId = cu.ComUnitId
                             WHERE u.EmpInfoId = @EmpInfoId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@EmpInfoId", empInfoId)
            });
        }

        public bool UpdateCompanyInfo(OrderInfoMaster aOrderInfoMaster)
        {
            string query = @"UPDATE tblOrder SET ComUnitId=@ComUnitId,ComUnitCode=@ComUnitCode,ComUnitName=@ComUnitName WHERE OrderId=@OrderId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                Param("@ComUnitId", aOrderInfoMaster.ComUnitId),
                Param("@ComUnitCode", aOrderInfoMaster.ComUnitCode),
                Param("@ComUnitName", aOrderInfoMaster.ComUnitName),
                Param("@OrderId", aOrderInfoMaster.OrderId)
            });
        }

        public DataTable GetSpecialApproveOrder(string actionstatus)
        {
            string query = @"SELECT O.OrderCode,
                                    O.CustomerMasterId,
                                    O.SubmissionDate,
                                    O.CustomerName,
                                    O.MIOName,
                                    M.MarketName,
                                    T.TerritoryName,
                                    R.RegionName,
                                    A.AreaName,
                                    O.GrossValue,
                                    SUM(OD.Quantity) AS QTY,
                                    O.OrderId,
                                    D.Due,
                                    CI.CompanyName,
                                    CU.ComUnitName
                             FROM dbo.tblOrder O
                             LEFT JOIN dbo.tblOrderDetail OD ON OD.OrderId = O.OrderId
                             LEFT JOIN dbo.tblRegion R ON R.RegionId = O.RegionId
                             LEFT JOIN dbo.tblArea A ON A.AreaId = O.AreaId
                             LEFT JOIN dbo.tblMarket M ON M.MarketId = O.MarketId
                             LEFT JOIN dbo.tblTerritory T ON T.TerritoryId = O.TerritoryId
                             LEFT JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = O.ComUnitId
                             LEFT JOIN dbo.tblCompanyInfo CI ON CI.CompanyId = CU.CompanyId
                             LEFT JOIN (
                                 SELECT CustomerMasterId,
                                        ISNULL(SUM(DeliveryTpGrandTotal - (ISNULL(PaymentAmount, 0) + ISNULL(DiscountOnPayment, 0) + ISNULL(AIT, 0))), 0) AS Due
                                 FROM dbo.tblInvoice
                                 WHERE DeliveryInvoiceStatus IS NOT NULL
                                   AND DeliveryInvoiceStatus <> 'Reject'
                                 GROUP BY CustomerMasterId
                             ) D ON D.CustomerMasterId = O.CustomerMasterId
                             WHERE (O.IsInvoice IS NULL OR O.IsInvoice = 0)
                               AND O.ActionStatus = @ActionStatus
                               AND O.IsSpecialApproval = 1
                             GROUP BY O.OrderCode,
                                      O.CustomerMasterId,
                                      O.SubmissionDate,
                                      O.CustomerName,
                                      O.MIOName,
                                      M.MarketName,
                                      T.TerritoryName,
                                      R.RegionName,
                                      A.AreaName,
                                      O.GrossValue,
                                      O.OrderId,
                                      D.Due,
                                      CI.CompanyName,
                                      CU.ComUnitName";

            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ActionStatus", actionstatus)
            });
        }

        public bool ApprovalUpdateDAL(OrderInfoMaster aOrderInfoMaster)
        {
            string query = @"UPDATE dbo.tblOrder
                             SET ActionStatus = @ActionStatus,
                                 ApprovedBy = @ApprovedBy,
                                 ApprovedDate = @ApprovedDate
                             WHERE OrderId = @OrderId";

            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                Param("@ActionStatus", aOrderInfoMaster.ActionStatus),
                Param("@ApprovedBy", aOrderInfoMaster.ApprovedBy),
                Param("@ApprovedDate", aOrderInfoMaster.ApprovedDate),
                Param("@OrderId", aOrderInfoMaster.OrderId)
            });
        }
    }
}
