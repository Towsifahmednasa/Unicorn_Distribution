using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.DataManager;
using Library.DAL.InternalCls;
using Library.DAL.MAIN_FUNCTION;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class InvoiceDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        private DataAccessManager  accessManager = new DataAccessManager ();

        DB_Manager aDbManager = new DB_Manager();

        private static SqlParameter Param(string name, object value)
        {
            return new SqlParameter(name, SInventorySql.DbValue(value));
        }

        private static string BuildInClause(string parameterPrefix, string commaSeparatedValues, List<SqlParameter> parameters)
        {
            List<string> parameterNames = new List<string>();
            string[] values = (commaSeparatedValues ?? string.Empty).Split(',');

            for (int i = 0; i < values.Length; i++)
            {
                string value = values[i].Trim().Trim('\'');
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                string parameterName = parameterPrefix + i;
                parameterNames.Add(parameterName);
                parameters.Add(Param(parameterName, value));
            }

            return parameterNames.Count == 0 ? "(NULL)" : "(" + string.Join(",", parameterNames) + ")";
        }

        public DataTable MoneyReceiptInfo(string pram)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@Parm", pram));

                DataTable dt = accessManager.GetDataTable("sp_Get_MoneyReceiptReportList", aSqlParameters);
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
         

        public DataTable MoneyReceiptAfterPaymentInfo(string pram)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
                aSqlParameters.Add(new SqlParameter("@Parm", pram));

                DataTable dt = accessManager.GetDataTable("sp_Get_MoneyReceiptReportAfterPaymentList", aSqlParameters);
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
        public DataTable GetNewReceiveableDAl(string districtId, string fromDate, string toDate)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@districtId", districtId));
                aSqlParameterlist.Add(new SqlParameter("@fromDate", fromDate));
                aSqlParameterlist.Add(new SqlParameter("@toDate", toDate));

                DataTable dt = accessManager.GetDataTable("sp_Get_NewReceiveableList", aSqlParameterlist);
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
        public DataTable GetNewReceiveableDAlWeb(string districtId, string fromDate, string toDate)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@districtId", districtId));
                aSqlParameterlist.Add(new SqlParameter("@fromDate", fromDate));
                aSqlParameterlist.Add(new SqlParameter("@toDate", toDate));

                DataTable dt = accessManager.GetDataTable("sp_Get_NewReceiveableListWeb", aSqlParameterlist);
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
        public DataTable GetNewReceiveableforInvoiceDAl(string districtId, string fromDate, string toDate)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@districtId", districtId));
                aSqlParameterlist.Add(new SqlParameter("@fromDate", fromDate));
                aSqlParameterlist.Add(new SqlParameter("@toDate", toDate));

                DataTable dt = accessManager.GetDataTable("sp_Get_NewReceiveableListforInvoice", aSqlParameterlist);
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
        public DataTable GetMonthlyInventoryReportDAl(string districtId, string fromDate, string toDate, string ProTypId)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameters = new List<SqlParameter>();
               

                aSqlParameters.Add(new SqlParameter("@fromDate", fromDate));
                aSqlParameters.Add(new SqlParameter("@toDate", toDate));
                aSqlParameters.Add(new SqlParameter("@CiD", districtId));
                aSqlParameters.Add(new SqlParameter("@ProTypId", ProTypId));

                DataTable dt = accessManager.GetDataTable("sp_Get_MonthlyInventoryReport", aSqlParameters);
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
        public DataTable GetPendingSalesConfirmationReportDAl(string districtId, string fromDate, string toDate)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@districtId", districtId));
                //aSqlParameterlist.Add(new SqlParameter("@fromDate", fromDate));
                //aSqlParameterlist.Add(new SqlParameter("@toDate", toDate));

                DataTable dt = accessManager.GetDataTable("sp_GET_PendingSalesConfirmationReport", aSqlParameterlist);
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
        public void CreateConnection_DAL()
        {
            aDbManager.CreateConnection("SalesDisDB_New3");
        }
        public void CloseAllConnection_DAL()
        {
            aDbManager.CloseConnection();
        }
        public int SaveFullInvoice(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationFull_New", aSqlParameterList, "SSIDB");
        }

        public bool PostDeliveryConfFullJournal(int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            //return aCommonInternalDal.UpdateAction("sp_DeliveryConfirmationFullPosting", aSqlParameterList);
            return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", aSqlParameterList);
        }

        public int SavePaymentConformationFull(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_PaymentConformationFull", aSqlParameterList, "SSIDB");
        }


        public int UP_LoadingSummaryInvoice(string InvoiceNo, string updateby, string status)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@LoadingSummaryStatus", status));
            return aCommonInternalDal.RunStoreProcedure("sp_UP_LoadingSummary", aSqlParameterList, "SSIDB");
        }


        public int UP_LoadingSummaryInvoice_Complete(string InvoiceNo, string updateby, string status)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceId", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@LoadingSummaryStatus", status));
            return aCommonInternalDal.RunStoreProcedure("sp_UP_LoadingSummaryFinal", aSqlParameterList, "SSIDB");
        }

        public int SaveFullInvoiceOldData(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationFull_OldData", aSqlParameterList, "SSIDB");
        }
        public int SubdeportSaveFullInvoice(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_SubdeportDeliveryConformationFull", aSqlParameterList, "SSIDB");
        }

        public int SaveFullProformaInvoice(string InvoiceNo, string updateby, string updatedate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationFull", aSqlParameterList, "SSIDB");
        }
        public int SaveRejectInvoice(string InvoiceNo, string updateby, string updatedate, string reason)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            aSqlParameterList.Add(new SqlParameter("@ReturnReason", reason));
            return aCommonInternalDal.RunStoreProcedure("sp_DeliveryConformationReject", aSqlParameterList, "SSIDB");
        }

        public int SaveProformaInvoice(string InvoiceNo, string updateby)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@OrderCodeInPut", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UserId", updateby));

            return aCommonInternalDal.RunStoreProcedure("sp_AutoInvoiceGeneration", aSqlParameterList, "SSIDB");
        }

        public int SubSaveRejectInvoice(string InvoiceNo, string updateby, string updatedate, string reason)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@InvoiceNo", InvoiceNo));
            aSqlParameterList.Add(new SqlParameter("@UpdateBy", updateby));
            aSqlParameterList.Add(new SqlParameter("@UpdateDate", updatedate));
            aSqlParameterList.Add(new SqlParameter("@ReturnReason", reason));
            return aCommonInternalDal.RunStoreProcedure("sp_SubDeportDeliveryConformationReject", aSqlParameterList, "SSIDB");
        }
        //public Int32 SaveDataForInvoice(Invoice aInvoice)
        //{
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aInvoice.InvoiceId));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceNo", aInvoice.InvoiceNo));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceDate", aInvoice.InvoiceDate));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderNo", aInvoice.OrderNo));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderDate", aInvoice.OrderDate));
        //    aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aInvoice.CustomerMasterId));
        //    aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aInvoice.ComUnitId));
        //    aSqlParameterlist.Add(new SqlParameter("@MiaId", aInvoice.MiaId));
        //    aSqlParameterlist.Add(new SqlParameter("@PaymentTypeId", aInvoice.PaymentTypeId));
        //    aSqlParameterlist.Add(new SqlParameter("@TpTotal", aInvoice.TpTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@TpDiscount", aInvoice.TpDiscount));
        //    aSqlParameterlist.Add(new SqlParameter("@TpVat", aInvoice.TpVat));
        //    aSqlParameterlist.Add(new SqlParameter("@TpGrandTotal", aInvoice.TpGrandTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@UserId", aInvoice.UserId));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderId", aInvoice.OrderId));
        //    aSqlParameterlist.Add(new SqlParameter("@TotalSpecialAmount", aInvoice.TotalSpecialAmount));
        //    aSqlParameterlist.Add(new SqlParameter("@ProductOffer", aInvoice.ProductOffer));

        //    return aDbManager.SaveAction("sp_I_InvoiceMaster", aSqlParameterlist, "@InvoiceId");
        //}
        //public Int32 SaveDataForInvoiceDetails(InvoiceDetail aInvoiceDetail)
        //{
        //    List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceId", aInvoice.InvoiceId));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceNo", aInvoice.InvoiceNo));
        //    aSqlParameterlist.Add(new SqlParameter("@InvoiceDate", aInvoice.InvoiceDate));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderNo", aInvoice.OrderNo));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderDate", aInvoice.OrderDate));
        //    aSqlParameterlist.Add(new SqlParameter("@CustomerMasterId", aInvoice.CustomerMasterId));
        //    aSqlParameterlist.Add(new SqlParameter("@ComUnitId", aInvoice.ComUnitId));
        //    aSqlParameterlist.Add(new SqlParameter("@MiaId", aInvoice.MiaId));
        //    aSqlParameterlist.Add(new SqlParameter("@PaymentTypeId", aInvoice.PaymentTypeId));
        //    aSqlParameterlist.Add(new SqlParameter("@TpTotal", aInvoice.TpTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@TpDiscount", aInvoice.TpDiscount));
        //    aSqlParameterlist.Add(new SqlParameter("@TpVat", aInvoice.TpVat));
        //    aSqlParameterlist.Add(new SqlParameter("@TpGrandTotal", aInvoice.TpGrandTotal));
        //    aSqlParameterlist.Add(new SqlParameter("@UserId", aInvoice.UserId));
        //    aSqlParameterlist.Add(new SqlParameter("@OrderId", aInvoice.OrderId));
        //    aSqlParameterlist.Add(new SqlParameter("@TotalSpecialAmount", aInvoice.TotalSpecialAmount));
        //    aSqlParameterlist.Add(new SqlParameter("@ProductOffer", aInvoice.ProductOffer));

        //    return aDbManager.SaveAction("sp_I_InvoiceMaster", aSqlParameterlist, "@InvoiceId");
        //}

        public bool SaveDataForInvoice(Invoice aInvoice)
        {
            string insertQuery = @"INSERT INTO dbo.tblInvoice
                (InvoiceNo, CustomerType, AdjustInvoiceNo_ReturnInvoiceNo, CreateDate, AdjustAmount,
                 IsAdjustInvoice, ReceivableAmount, InvoiceDate, OrderNo, OrderDate, CustomerMasterId,
                 ComUnitId, MiaId, PaymentTypeId, TpTotal, TpDiscount, Types, TpVat, TpGrandTotal,
                 UserId, OrderId, TotalSpecialAmount, OldTradePolicy, ProductOffer, Inv_DANameId,
                 Remarks, MIACode, MIAName, MarketCode, MarketName, AreaCode, DisCode, FEName,
                 RegionCode, DZSMName, FixedCustomer, DeliveryPersonName, DeliveryPersonPhNo, CustomerDeliveryAddress, DeliveryCharge)
                VALUES
                (@InvoiceNo, @CustomerType, @AdjustInvoiceNo_ReturnInvoiceNo, @CreateDate, @AdjustAmount,
                 @IsAdjustInvoice, @ReceivableAmount, @InvoiceDate, @OrderNo, @OrderDate, @CustomerMasterId,
                 @ComUnitId, @MiaId, @PaymentTypeId, @TpTotal, @TpDiscount, @Types, @TpVat, @TpGrandTotal,
                 @UserId, @OrderId, @TotalSpecialAmount, @OldTradePolicy, @ProductOffer, @Inv_DANameId,
                 @Remarks, @MIACode, @MIAName, @MarketCode, @MarketName, @AreaCode, @DisCode, @FEName,
                 @RegionCode, @DZSMName, @FixedCustomer, @DeliveryPersonName, @DeliveryPersonPhNo, @CustomerDeliveryAddress, @DeliveryCharge)";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                Param("@InvoiceNo", aInvoice.InvoiceNo),
                Param("@CustomerType", aInvoice.cusType),
                Param("@AdjustInvoiceNo_ReturnInvoiceNo", aInvoice.AdjustInvoiceNo_ReturnInvoiceNo),
                Param("@CreateDate", aInvoice.Createdate),
                Param("@AdjustAmount", aInvoice.AdjustAmount),
                Param("@IsAdjustInvoice", aInvoice.IsAdjustInvoice),
                Param("@ReceivableAmount", aInvoice.ReceivableAmount),
                Param("@InvoiceDate", aInvoice.InvoiceDate),
                Param("@OrderNo", aInvoice.OrderNo),
                Param("@OrderDate", aInvoice.OrderDate),
                Param("@CustomerMasterId", aInvoice.CustomerMasterId),
                Param("@ComUnitId", aInvoice.ComUnitId),
                Param("@MiaId", aInvoice.MiaId),
                Param("@PaymentTypeId", aInvoice.PaymentTypeId),
                Param("@TpTotal", aInvoice.TpTotal),
                Param("@TpDiscount", aInvoice.TpDiscount),
                Param("@Types", aInvoice.Type),
                Param("@TpVat", aInvoice.TpVat),
                Param("@TpGrandTotal", aInvoice.TpGrandTotal),
                Param("@UserId", aInvoice.UserId),
                Param("@OrderId", aInvoice.OrderId),
                Param("@TotalSpecialAmount", aInvoice.TotalSpecialAmount),
                Param("@OldTradePolicy", aInvoice.OldTradePolicy),
                Param("@ProductOffer", aInvoice.ProductOffer),
                Param("@Inv_DANameId", aInvoice.Inv_DANameId),
                Param("@Remarks", aInvoice.Remarks),
                Param("@MIACode", aInvoice.MIACode),
                Param("@MIAName", aInvoice.MIAName),
                Param("@MarketCode", aInvoice.MarketCode),
                Param("@MarketName", aInvoice.MarketName),
                Param("@AreaCode", aInvoice.AreaCode),
                Param("@DisCode", aInvoice.DisCode),
                Param("@FEName", aInvoice.FEName),
                Param("@RegionCode", aInvoice.RegionCode),
                Param("@DZSMName", aInvoice.DZSMName),
                Param("@FixedCustomer", aInvoice.FixedCustomer),
                Param("@DeliveryPersonName", aInvoice.DpNAme),
                Param("@DeliveryPersonPhNo", aInvoice.DpMob),
                Param("@CustomerDeliveryAddress", aInvoice.CustomerDeliveryAddress),
                Param("@DeliveryCharge", aInvoice.DeliveryCharge)
            };
            return SInventorySql.Execute(insertQuery, parameters);
        }

        public bool SaveDataForInvoiceBatch(string invoiceid,string batchno)
        {
            string insertQuery = @"INSERT INTO dbo.tblInvoiceBatch
	(
	    BatchNo,
	    Date,
	    InvoiceId
	)
	VALUES
	(@BatchNo,GETDATE(),@InvoiceId) ";

            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                Param("@BatchNo", batchno),
                Param("@InvoiceId", invoiceid)
            });
        }

        public bool SaveDataForReturnInvoice(Invoice aInvoice)
        {
            string insertQuery = @"INSERT INTO dbo.tblReturnInvoice
                (ReturnInvoiceId, ReturnInvoiceNo, ReturnInvoiceDate, OrderNo, OrderDate, CustomerMasterId,
                 ComUnitId, MiaId, PaymentTypeId, TpTotal, TpDiscount, TpVat, TpGrandTotal, UserId,
                 OrderId, InvoiceId, TotalSpecialAmount)
                VALUES
                (@ReturnInvoiceId, @ReturnInvoiceNo, @ReturnInvoiceDate, @OrderNo, @OrderDate, @CustomerMasterId,
                 @ComUnitId, @MiaId, @PaymentTypeId, @TpTotal, @TpDiscount, @TpVat, @TpGrandTotal, @UserId,
                 @OrderId, @InvoiceId, @TotalSpecialAmount)";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                Param("@ReturnInvoiceId", aInvoice.InvoiceId),
                Param("@ReturnInvoiceNo", aInvoice.InvoiceNo),
                Param("@ReturnInvoiceDate", aInvoice.InvoiceDate),
                Param("@OrderNo", aInvoice.OrderNo),
                Param("@OrderDate", aInvoice.OrderDate),
                Param("@CustomerMasterId", aInvoice.CustomerMasterId),
                Param("@ComUnitId", aInvoice.ComUnitId),
                Param("@MiaId", aInvoice.MiaId),
                Param("@PaymentTypeId", aInvoice.PaymentTypeId),
                Param("@TpTotal", aInvoice.TpTotal),
                Param("@TpDiscount", aInvoice.TpDiscount),
                Param("@TpVat", aInvoice.TpVat),
                Param("@TpGrandTotal", aInvoice.TpGrandTotal),
                Param("@UserId", aInvoice.UserId),
                Param("@OrderId", aInvoice.OrderId),
                Param("@InvoiceId", aInvoice.ReturnInvoiceid),
                Param("@TotalSpecialAmount", aInvoice.TotalSpecialAmount)
            });
        }

        public bool SaveDataForInvoiceDetails(InvoiceDetail aInvoiceDetail)
        {
            string insertQuery = @"INSERT INTO dbo.tblInvoiceDetail
                (ProductCode, ProductName, PackSize, BatchNo, ReceiveDate, ExpDate, CostPrice, UnitPrice,
                 UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice, TotalPriceVatAmount,
                 DiscountPercentage, DiscountAmount, NetAmount, InvoiceId, DCStoreId, OrderDetailsId,
                 Campaign, ISGiftProduct, IsCampaignProduct, SpecialAmount, AdjustmentAmount)
                VALUES
                (@ProductCode, @ProductName, @PackSize, @BatchNo, @ReceiveDate, @ExpDate, @CostPrice, @UnitPrice,
                 @UnitVatAmount, @Quantity, @BonusQuantity, @TotalQuantity, @TotalPrice, @TotalPriceVatAmount,
                 @DiscountPercentage, @DiscountAmount, @NetAmount, @InvoiceId, @DCStoreId, @OrderDetailsId,
                 @Campaign, @ISGiftProduct, @IsCampaignProduct, @SpecialAmount, @AdjustmentAmount)";
            return SInventorySql.Execute(insertQuery, InvoiceDetailParameters(aInvoiceDetail, false));
        }

        public bool SaveDataForReturnInvoiceDetails(InvoiceDetail aInvoiceDetail)
        {
            string insertQuery = @"INSERT INTO dbo.tblReturnInvoiceDetail
                (ReuturnInvoiceDetailId, ProductCode, ProductName, PackSize, BatchNo, ReceiveDate, ExpDate,
                 CostPrice, UnitPrice, UnitVatAmount, Quantity, BonusQuantity, TotalQuantity, TotalPrice,
                 TotalPriceVatAmount, DiscountPercentage, DiscountAmount, NetAmount, ReturnInvoiceId,
                 DCStoreId, OrderDetailsId, InvoiceDetailId, SpecialAmount)
                VALUES
                (@ReuturnInvoiceDetailId, @ProductCode, @ProductName, @PackSize, @BatchNo, @ReceiveDate, @ExpDate,
                 @CostPrice, @UnitPrice, @UnitVatAmount, @Quantity, @BonusQuantity, @TotalQuantity, @TotalPrice,
                 @TotalPriceVatAmount, @DiscountPercentage, @DiscountAmount, @NetAmount, @ReturnInvoiceId,
                 @DCStoreId, @OrderDetailsId, @InvoiceDetailId, @SpecialAmount)";
            List<SqlParameter> parameters = InvoiceDetailParameters(aInvoiceDetail, true);
            parameters.Add(Param("@ReuturnInvoiceDetailId", aInvoiceDetail.InvoiceDetailId));
            parameters.Add(Param("@ReturnInvoiceId", aInvoiceDetail.InvoiceId));
            parameters.Add(Param("@InvoiceDetailId", aInvoiceDetail.ReturnDetailsId));
            return SInventorySql.Execute(insertQuery, parameters);
        }

        public bool UpdateInvoiceHeaderTotals(int invoiceId)
        {
            string query = @"
                UPDATE tblInvoice 
                SET 
                    TpTotal = d.TotPrice,
                    TpDiscount = d.TotDiscount,
                    TpVat = d.TotVat,
                    TpGrandTotal = (d.TotNet + ISNULL(tblInvoice.DeliveryCharge, 0)),
                    ReceivableAmount = (d.TotNet + ISNULL(tblInvoice.DeliveryCharge, 0) - ISNULL(tblInvoice.AdjustAmount, 0))
                FROM tblInvoice
                INNER JOIN (
                    SELECT 
                        InvoiceId,
                        ISNULL(SUM(TotalPrice), 0) AS TotPrice,
                        ISNULL(SUM(DiscountAmount), 0) AS TotDiscount,
                        ISNULL(SUM(TotalPriceVatAmount), 0) AS TotVat,
                        ISNULL(SUM(NetAmount), 0) AS TotNet
                    FROM tblInvoiceDetail
                    WHERE InvoiceId = @InvoiceId
                    GROUP BY InvoiceId
                ) d ON tblInvoice.InvoiceId = d.InvoiceId
                WHERE tblInvoice.InvoiceId = @InvoiceId";

            return SInventorySql.Execute(query, new List<SqlParameter> { Param("@InvoiceId", invoiceId) });
        }

        private List<SqlParameter> InvoiceDetailParameters(InvoiceDetail aInvoiceDetail, bool forReturnInvoice)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                Param("@ProductCode", aInvoiceDetail.ProductCode),
                Param("@ProductName", aInvoiceDetail.ProductName),
                Param("@PackSize", aInvoiceDetail.PackSize),
                Param("@BatchNo", aInvoiceDetail.BatchNo),
                Param("@ReceiveDate", aInvoiceDetail.ReceiveDate),
                Param("@ExpDate", aInvoiceDetail.ExpDate),
                Param("@CostPrice", aInvoiceDetail.CostPrice),
                Param("@UnitPrice", aInvoiceDetail.UnitPrice),
                Param("@UnitVatAmount", aInvoiceDetail.UnitVatAmount),
                Param("@Quantity", aInvoiceDetail.Quantity),
                Param("@BonusQuantity", aInvoiceDetail.BonusQuantity),
                Param("@TotalQuantity", aInvoiceDetail.TotalQuantity),
                Param("@TotalPrice", aInvoiceDetail.TotalPrice),
                Param("@TotalPriceVatAmount", aInvoiceDetail.TotalPriceVatAmount),
                Param("@DiscountPercentage", aInvoiceDetail.DiscountPercentage),
                Param("@DiscountAmount", aInvoiceDetail.DiscountAmount),
                Param("@NetAmount", aInvoiceDetail.NetAmount),
                Param("@DCStoreId", aInvoiceDetail.DCStoreId),
                Param("@OrderDetailsId", aInvoiceDetail.OrderDetailsId),
                Param("@SpecialAmount", aInvoiceDetail.SpecialAmount)
            };

            if (!forReturnInvoice)
            {
                parameters.Add(Param("@InvoiceId", aInvoiceDetail.InvoiceId));
                parameters.Add(Param("@Campaign", "N"));
                parameters.Add(Param("@ISGiftProduct", aInvoiceDetail.ISGiftProductforInv));
                parameters.Add(Param("@IsCampaignProduct", aInvoiceDetail.IsCampaignProductforInv));
                parameters.Add(Param("@AdjustmentAmount", aInvoiceDetail.AdjustmentAmount));
            }

            return parameters;
        }

        public bool HasProductcode(DCStore aReceive)
        {
            string query = "select top 1 1 from tblDCStock where ProductCode = @ProductCode and BatchNo = @BatchNo";
            return SInventorySql.Exists(query, new List<SqlParameter>
            {
                Param("@ProductCode", aReceive.ProductCode),
                Param("@BatchNo", aReceive.BatchNo)
            });
        }

        public DataTable LoadInvoiceView()
        {
            string query = @"SELECT *  FROM tblInvoice ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable ProductInfoDAL(string comUnitId, string productCode)
        {

             string query = @"   SELECT P.ProductCode,(P.ProductName+':'+P.PackSize) as  ProductName,P.PackSize, " +
            " ISNULL(UP.UnitPrice,0) AS UnitPrice,ISNULL(VCS.TotalQty,0) AS StockQty, " +
            " (UP.VATAmountPerUnit) AS VAT, ISNULL(UP.CostPrice,0) AS CostPrice, " +
            " ISNULL(UP.VATPercentage,0)VATPercentage  FROM " +
            " dbo.tblProduct P  " +
            " LEFT JOIN dbo.tblUnitPrice UP ON P.ProductCode = UP.ProductCode  " +
            " LEFT JOIN (select ComUnitId,ProductCode, TotalQty from View_DCStoreCurrentStock WHERE ComUnitId=@ComUnitId AND ProductCode=@ProductCode) VCS  " +
            " ON P.ProductCode=VCS.ProductCode   where P.ProductCode=@ProductCode ";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", comUnitId == null ? null : comUnitId.Trim()),
                Param("@ProductCode", productCode == null ? null : productCode.Trim())
            });
        }
        public DataTable ProductFocBonusQtyDAL(string invoiceDate, string productCode,int Qty)
        {

            string query = @"select * from [dbo].[tblFocMaster] M " +
                           " inner join [dbo].[tblFocDetails] D on M.FocId=D.FocId " +
                           " where ProductCode=@ProductCode and (@InvoiceDate between [FocFromDate] and [FocToDate]) " +
                           " and IsActive=1 and (@Qty between [RangeFrom] and RangeTo) ";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ProductCode", productCode == null ? null : productCode.Trim()),
                Param("@InvoiceDate", invoiceDate == null ? null : invoiceDate.Trim()),
                Param("@Qty", Qty)
            });
        }
        public DataTable LoadProductQty(string orderid, string productCode)
        {
            string query = @"SELECT SUM(Quantity)Qty FROM dbo.tblOrder
LEFT JOIN dbo.tblOrderDetail ON dbo.tblOrder.OrderId = dbo.tblOrderDetail.OrderId WHERE dbo.tblOrder.OrderId=@OrderId AND ProductCode=@ProductCode";

            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@OrderId", orderid),
                Param("@ProductCode", productCode)
            });
        }
       
        public DataTable LoadProduct(string productId)
        {
            string query = @"SELECT * FROM tblProduct where ProductCode=@ProductCode";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ProductCode", productId == null ? null : productId.Trim())
            });
        }
        public DataTable LoadCustomerMaster(string OrderNO)
        {
            string query = @"SELECT   * FROM dbo.View_OrderCustomerInfo ord 
WHERE ord.OrderCode=@OrderCode";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@OrderCode", OrderNO == null ? null : OrderNO.Trim())
            });
        }
        public DataTable DCStockQuantity(DCStore aReceive)
        {
            string query = "select * from tblDCStock where ProductCode = @ProductCode and BatchNo = @BatchNo";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ProductCode", aReceive.ProductCode),
                Param("@BatchNo", aReceive.BatchNo)
            });
        }
        public DataTable DCInfoWithDCId(string dcstoreId)
        {
            string query = "SELECT * FROM dbo.tblDCStore WHERE DCStoreId=@DCStoreId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@DCStoreId", dcstoreId)
            });
        }

        public void UpdateDCStoreQuantity(string dCStoreId, decimal issueQty)
        {
            string updateQuery = @"
    INSERT INTO dbo.tblDCStore 
    (
        DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
        TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
        ComUnitId, StockQty, DamageQty, StockRcvDate, ReqId, ReqChildId,
        StockCondition, StockInTransfarId,
        Trans_Type, CostPerUnit, TotalCostAmount,
        ReferenceDCStoreId, ReferenceTransactionDate
    )
    SELECT
        (SELECT ISNULL(MAX(DCStoreId), 0) + 1 FROM tblDCStore),
        StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
        0,
        ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
        ComUnitId, @IssueQty * -1,
        0, StockRcvDate, ReqId, ReqChildId,
        'Issue', StockInTransfarId,
        'Issue',
        CostPerUnit,
        @IssueQty * -1 * CostPerUnit,
        @DCStoreId,
        GETDATE()
    FROM tblDCStore
    WHERE DCStoreId = @DCStoreId
      AND Trans_Type = 'Receive'";

            SInventorySql.Execute(updateQuery, new List<SqlParameter>
    {
        Param("@IssueQty", issueQty),
        Param("@DCStoreId", dCStoreId == null ? null : dCStoreId.Trim())
    });
        }


        //public void UpdateDCStoreQuantity(string dCStoreId, decimal Quantity)
        //{
        //    string updateQuery = @"UPDATE tblDCStore SET StockQty=@StockQty WHERE DCStoreId=@DCStoreId";
        //    SInventorySql.Execute(updateQuery, new List<SqlParameter>
        //    {
        //        Param("@StockQty", Quantity),
        //        Param("@DCStoreId", dCStoreId == null ? null : dCStoreId.Trim())
        //    });
        //}

        public DataTable Isgift(int dcstoreId)
        {
            string query = "select ISGiftProduct from tblOrderDetail where OrderDetailId=@OrderDetailId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@OrderDetailId", dcstoreId)
            });
        }
      
      //  public bool DCStockInDAL(DCStockNew aDcStockNew)
      //  {
      //      string query = @"INSERT INTO dbo.tblDCStoreFreeze " +
      // "  ( DCStoreId , " +
      //   "    DCStoreFreezeId, " +
      //   "    InvoiceDetailId, " +
      //   "    StorageLocation , " +
      //   "    ProductCode , " +
      //   "    ProductName , " +
      //   "    PackSize , " +
      //   "    BatchNo , " +
      //   "    TotalQuantity , " +
      //   "    ExpDate , " +
      //   "    ReceiveDate , " +
      //   "    ChalanNo , " +
      //   "    ChalanDate , " +
      //    "   ComUnitId , " +
      //   "    StockQty , " +
      //   "    DamageQty , " +
      //   "    StockRcvDate , " +
      //   "    ReqId , " +
      //   "    ReqChildId , " +
      //   "    StockInTransfarId, " +
      //   "    StockCondition " +
      //  "   ) " +
      //  "   VALUES  ( '" + aDcStockNew.DCStoreId + "' , " +
      //  "     '" + aDcStockNew.DCStoreFreezeId + "' , " +
      //  "     '" + aDcStockNew.InvoiceDetailId + "' , " +
      //  "     '" + aDcStockNew.StorageLocation + "' , " +
      //  "     '" + aDcStockNew.ProductCode + "' ,  " +
      //  "    '" + aDcStockNew.ProductName + "' , " +
      //  "    '" + aDcStockNew.PackSize + "' , " +
      //  "    '" + aDcStockNew.BatchNo + "' , " +
      //   "    '" + aDcStockNew.TotalQuantity + "' ,  " +
      //   "    '" + aDcStockNew.ExpDate + "' ,  " +
      //  "     '" + aDcStockNew.ReceiveDate + "' , " +
      //  "    '" + aDcStockNew.ChalanNo + "' , " +
      //  "    '" + aDcStockNew.ChalanDate + "', " +
      //  "    '" + aDcStockNew.ComUnitId + "' , " +
      //   "    '" + aDcStockNew.StockQty + "', " +
      //   "    '" + aDcStockNew.DamageQty + "' , " +
      //   "   '" + aDcStockNew.StockRcvDate + "' , " +
      //   "   '" + aDcStockNew.ReqId + "' , " +
      //  "    '" + aDcStockNew.ReqChildId + "', " +
      // "      '" + aDcStockNew.StockInTransfarId + "',  " +
      // "      'ReturnStock'  " +
      //"     )";
      //      return aCommonInternalDal.SaveDataByInsertCommand(query, "SSIDB");
      //  }

        public void UpdateInvoice(Invoice aInvoice)
        {
            string updateQuery = @"UPDATE tblInvoice SET DeliveryTpTotal=@DeliveryTpTotal,DeliveryTpDiscount=@DeliveryTpDiscount,DeliveryTpVat=@DeliveryTpVat,UpdateDatetime=@UpdateDatetime,
                                 DeliveryTpGrandTotal=@DeliveryTpGrandTotal,DeliveryInvoiceStatus=@DeliveryInvoiceStatus,DelivaryInvoiceNo=@DelivaryInvoiceNo,DelivarySpecialAmount=@DelivarySpecialAmount,UpdateBy=@UpdateBy,UpdateDate=@UpdateDate WHERE InvoiceId=@InvoiceId";
            SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                Param("@DeliveryTpTotal", aInvoice.TpTotal),
                Param("@DeliveryTpDiscount", aInvoice.TpDiscount),
                Param("@DeliveryTpVat", aInvoice.TpVat),
                Param("@UpdateDatetime", aInvoice.updatetime),
                Param("@DeliveryTpGrandTotal", aInvoice.TpGrandTotal),
                Param("@DeliveryInvoiceStatus", aInvoice.DeliveryInvoiceStatus),
                Param("@DelivaryInvoiceNo", "DEL-" + aInvoice.DelivaryInvoiceNo),
                Param("@DelivarySpecialAmount", aInvoice.TotalSpecialAmount),
                Param("@UpdateBy", aInvoice.UpdateBy),
                Param("@UpdateDate", aInvoice.InvoiceDate),
                Param("@InvoiceId", aInvoice.InvoiceId)
            });
        }


        public void PaymentUpdateInvoice(Invoice aInvoice)
        {
            string updateQuery = @"UPDATE tblInvoice SET PaymentTpTotal=@PaymentTpTotal,PaymentTpDiscount=@PaymentTpDiscount,PaymentTpVat=@PaymentTpVat,
                                 PaymentTpGrandTotal=@PaymentTpGrandTotal,PaymentInvoiceStatus=@PaymentInvoiceStatus,PaymentInvoiceNo=@PaymentInvoiceNo,PaymentBy=@PaymentBy,PaymentDate=@PaymentDate WHERE InvoiceId=@InvoiceId";
            SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                Param("@PaymentTpTotal", aInvoice.TpTotal),
                Param("@PaymentTpDiscount", aInvoice.TpDiscount),
                Param("@PaymentTpVat", aInvoice.TpVat),
                Param("@PaymentTpGrandTotal", aInvoice.TpGrandTotal),
                Param("@PaymentInvoiceStatus", aInvoice.DeliveryInvoiceStatus),
                Param("@PaymentInvoiceNo", "RTN-" + aInvoice.DelivaryInvoiceNo),
                Param("@PaymentBy", aInvoice.UpdateBy),
                Param("@PaymentDate", aInvoice.InvoiceDate),
                Param("@InvoiceId", aInvoice.InvoiceId)
            });
        }
        public void UpdateInvoiceDetail(InvoiceDetail  aInvoiceDetail)
        {
            string updateQuery = @"UPDATE tblInvoiceDetail SET DeliveryQuantity=@DeliveryQuantity,DeliveryBonusQuantity=@DeliveryBonusQuantity,DeliveryTotalQuantity=@DeliveryTotalQuantity,
                                 DeliveryTotalPrice=@DeliveryTotalPrice,DeliveryTotalPriceVatAmount=@DeliveryTotalPriceVatAmount,DeliveryDiscountPercentage=@DeliveryDiscountPercentage,
                                 DeliveryDiscountAmount=@DeliveryDiscountAmount,DeliveryNetAmount=@DeliveryNetAmount,DeliveryStatus=@DeliveryStatus,DelivarySpecialAmount=@DelivarySpecialAmount,
                                 ReturnReason=@ReturnReason WHERE InvoiceDetailId=@InvoiceDetailId";
            SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                Param("@DeliveryQuantity", aInvoiceDetail.Quantity),
                Param("@DeliveryBonusQuantity", aInvoiceDetail.BonusQuantity),
                Param("@DeliveryTotalQuantity", aInvoiceDetail.TotalQuantity),
                Param("@DeliveryTotalPrice", aInvoiceDetail.TotalPrice),
                Param("@DeliveryTotalPriceVatAmount", aInvoiceDetail.TotalPriceVatAmount),
                Param("@DeliveryDiscountPercentage", aInvoiceDetail.DiscountPercentage),
                Param("@DeliveryDiscountAmount", aInvoiceDetail.DiscountAmount),
                Param("@DeliveryNetAmount", aInvoiceDetail.NetAmount),
                Param("@DeliveryStatus", aInvoiceDetail.DeliveryStatus),
                Param("@DelivarySpecialAmount", aInvoiceDetail.SpecialAmount),
                Param("@ReturnReason", aInvoiceDetail.ReturnReason),
                Param("@InvoiceDetailId", aInvoiceDetail.InvoiceDetailId)
            });
        }

        public bool PostDeliveryConfPartialJournal(int invoiceId)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@InvoiceId", invoiceId));

            //return aCommonInternalDal.UpdateAction("sp_DeliveryConfirmationPartiallPosting", aSqlParameterList);
            return aCommonInternalDal.UpdateAction("sp_Sales_DeliverySalesPosting", aSqlParameterList);
        }


        public void PaymentUpdateInvoiceDetail(InvoiceDetail aInvoiceDetail)
        {
            string updateQuery = @"UPDATE tblInvoiceDetail SET PaymentQuantity=@PaymentQuantity,PaymentBonusQuantity=@PaymentBonusQuantity,PaymentTotalQuantity=@PaymentTotalQuantity,
                                 PaymentTotalPrice=@PaymentTotalPrice,PaymentTotalPriceVatAmount=@PaymentTotalPriceVatAmount,PaymentDiscountPercentage=@PaymentDiscountPercentage,
                                 PaymentDiscountAmount=@PaymentDiscountAmount,PaymentNetAmount=@PaymentNetAmount,PaymentStatus=@PaymentStatus,PaymentReturnReason=@PaymentReturnReason
                                 WHERE InvoiceDetailId=@InvoiceDetailId";
            SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                Param("@PaymentQuantity", aInvoiceDetail.Quantity),
                Param("@PaymentBonusQuantity", aInvoiceDetail.BonusQuantity),
                Param("@PaymentTotalQuantity", aInvoiceDetail.TotalQuantity),
                Param("@PaymentTotalPrice", aInvoiceDetail.TotalPrice),
                Param("@PaymentTotalPriceVatAmount", aInvoiceDetail.TotalPriceVatAmount),
                Param("@PaymentDiscountPercentage", aInvoiceDetail.DiscountPercentage),
                Param("@PaymentDiscountAmount", aInvoiceDetail.DiscountAmount),
                Param("@PaymentNetAmount", aInvoiceDetail.NetAmount),
                Param("@PaymentStatus", aInvoiceDetail.DeliveryStatus),
                Param("@PaymentReturnReason", aInvoiceDetail.ReturnReason),
                Param("@InvoiceDetailId", aInvoiceDetail.InvoiceDetailId)
            });
        }


        public void PaymentTypeLoad(DropDownList aDropDownList)
        {
            string query = @"select * from tblPaymentType";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "PaymentTypeName", "PaymentTypeId", query, "SSIDB");
        }
        public void ReturnReason(DropDownList aDropDownList)
        {
            string query = @"SELECT * FROM tblReturnReason";
            aCommonInternalDal.LoadDropDownValue(aDropDownList, "ReaturnReason", "ReturndReasonId", query, "SSIDB");
        }
        public DataTable GetReason()
        {
            string query = @"SELECT ReaturnReason,ReturndReasonId FROM tblReturnReason";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        //public DataTable GetWarning(string CustID, string CustCode)
        //{
        //    string query = @"Select TOP 1  InvoiceNo + '- Market Name: ' + MarketName+ '- Amount: ' + CONVERT(varchar, TpGrandTotal)  as Details,  DATEDIFF(DAY, UpdateDate, GETDATE()   ),* from tblInvoice with (nolock)
        //    where CustomerMasterId='" + CustID.Trim() + "' and  	InvoiceDate between '1-july-2021' and getdate() and   DelivaryInvoiceNo is null  and DATEDIFF(DAY, InvoiceDate, GETDATE()   ) >=30  and (InvoiceNo is not null)";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}


        public DataTable GetWarning(string CustID, string CustCode)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@CustID", CustID));
                aSqlParameterlist.Add(new SqlParameter("@CustCode", CustCode));
                DataTable dt = accessManager.GetDataTable("sp_GetWarningForCustomerPayment_new", aSqlParameterlist);
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

        public DataTable DiscountofProduct(string productCode,string qty)
        {
            string query = @"SELECT TOP 1 * FROM dbo.tblProductDiscount WHERE ProductCode=@ProductCode AND (Status='Active' OR Status='1' OR Status IS NULL)";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ProductCode", productCode == null ? null : productCode.Trim())
            });
        }

        public DataTable BatchWiseProductQty(string productCode, string comUnitId)
        {
            string query = @"
    SELECT 
        RCV.DCStoreId,
        RCV.StorageLocation,
        RCV.ProductCode,
        RCV.ProductName,
        RCV.PackSize,
        RCV.BatchNo,
        RCV.TotalQuantity,
        RCV.ExpDate,
        RCV.MfgDate,
        RCV.ReceiveDate,
        RCV.ChalanNo,
        RCV.ChalanDate,
        RCV.ComUnitId,
        RCV.CostPerUnit,
        RCV.StockInTransfarId,
        RCV.ReqId,
        RCV.ReqChildId,
        (CASE WHEN ISNULL(RCV.TotalQuantity, 0) > 0 THEN RCV.TotalQuantity ELSE RCV.StockQty END) + ISNULL(ISS.TotalIssued, 0) AS StockQty
    FROM tblDCStore RCV
    LEFT JOIN (
        SELECT ReferenceDCStoreId, SUM(StockQty) AS TotalIssued
        FROM tblDCStore
        WHERE Trans_Type = 'Issue'
          AND ProductCode = @ProductCode
          AND ComUnitId   = @ComUnitId
        GROUP BY ReferenceDCStoreId
    ) ISS ON ISS.ReferenceDCStoreId = RCV.DCStoreId
    WHERE RCV.Trans_Type  = 'Receive'
      AND RCV.ProductCode = @ProductCode
      AND RCV.ComUnitId   = @ComUnitId
      AND ((CASE WHEN ISNULL(RCV.TotalQuantity, 0) > 0 THEN RCV.TotalQuantity ELSE RCV.StockQty END) + ISNULL(ISS.TotalIssued, 0)) > 0
    ORDER BY RCV.ExpDate ASC, RCV.BatchNo ASC";

            return SInventorySql.GetDataTable(query, new List<SqlParameter>
    {
        Param("@ProductCode", productCode == null ? null : productCode.Trim()),
        Param("@ComUnitId", comUnitId == null ? null : comUnitId.Trim())
    });
        }

        //public DataTable BatchWiseProductQty(string productCode, string comUnitId)
        //{
        //    string query = @"SELECT * FROM tblDCStore WHERE ProductCode=@ProductCode AND ComUnitId=@ComUnitId AND StockQty>0 ORDER BY ExpDate ASC,BatchNo ASC";
        //    return SInventorySql.GetDataTable(query, new List<SqlParameter>
        //    {
        //        Param("@ProductCode", productCode == null ? null : productCode.Trim()),
        //        Param("@ComUnitId", comUnitId == null ? null : comUnitId.Trim())
        //    });
        //}

        //public DataTable InvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName , " +
        //                 " (CU.Address +' ' +',M-'+CU.MobileNo+',P-'+CU.PhoneNo+',F-'+CU.FaxNo) AS CUAddress, "+
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address+' '+CM.CellNo) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, "+
        //                " MIA.MiaCode,MIA.MiaName,U.UserName "+
        //                " FROM tblInvoice IV "+
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId "+
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId "+
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId "+
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId "+
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId "+
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        //public DataTable InvoiceDetailDataForReport(string invNo)
        //{
        //    //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
        //    //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
        //    //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //    //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //    //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

        //    string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
        //                    "  IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
        //                    "  (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.InvoiceId  " +
        //                    "   FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //             "  INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId " +
        //            "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //              " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        public DataTable InvoiceDetailDataForReport(int Dcid, int ManufId, int marketid, DateTime invDate)
        {
            string query = @"    SELECT  I.CustomerType, CellNo,(Addrees2 + '[' +  Address +']')		 as Address	,tblMarket.MarketName + ', Territory Name : ' + A.AreaName as MarketName ,* 				
FROM tblInvoice I
INNER JOIN (SELECT DISTINCT D.InvoiceId, ManufacId FROM dbo.tblInvoice I
            INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId
            INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode
            ) as tblD ON I.InvoiceId = tblD.InvoiceId  
 INNER JOIN dbo.View_CustomerMaster C ON I.CustomerMasterId = C.CustomerMasterId
 INNER JOIN dbo.tblMarket ON C.MarketCode=dbo.tblMarket.MarketCode  
  INNER JOIN tblArea A ON A.AreaCode = I.AreaCode   
where I.ComUnitId=@ComUnitId and tblD.ManufacId=@ManufacId and tblMarket.MarketId=@MarketId and InvoiceDate=@InvoiceDate order by OrderNo";


            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", Dcid),
                Param("@ManufacId", ManufId),
                Param("@MarketId", marketid),
                Param("@InvoiceDate", invDate)
            });
        }
        public DataTable InvoiceDetailDataForReport(int Dcid, int ManufId, string tr, DateTime invDate)
        {
            string query = @"    SELECT  I.CustomerType, CellNo,(Addrees2 + '[' +  Address +']')		 as Address	,* 				
FROM tblInvoice I
INNER JOIN (SELECT DISTINCT D.InvoiceId, ManufacId FROM dbo.tblInvoice I
            INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId
            INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode
            ) as tblD ON I.InvoiceId = tblD.InvoiceId  
 INNER JOIN dbo.View_CustomerMaster C ON I.CustomerMasterId = C.CustomerMasterId
 INNER JOIN dbo.tblMarket ON C.MarketCode=dbo.tblMarket.MarketCode     
where I.ComUnitId=@ComUnitId and tblD.ManufacId=@ManufacId and I.AreaCode=@AreaCode and InvoiceDate=@InvoiceDate order by OrderNo";


            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", Dcid),
                Param("@ManufacId", ManufId),
                Param("@AreaCode", tr),
                Param("@InvoiceDate", invDate)
            });
        }
        public DataTable ReturnInvoiceMainDataForReport(string invNo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string invNoClause = BuildInClause("@ReturnInvoiceNo", invNo, parameters);
            string query = @"SELECT IV.InvoiceId,IV.ReturnInvoiceNo as InvoiceNo,ReturnInvoiceId as InvoiceId ,IV.ReturnInvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,IV.TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName , " +
                         " (CU.Address +' ' +',M-'+CU.MobileNo+',P-'+CU.PhoneNo+',F-'+CU.FaxNo) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address+' '+CM.CellNo) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,U.UserName " +
                        " FROM tblReturnInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in " + invNoClause;
            return SInventorySql.GetDataTable(query, parameters);
        }
        public DataTable ReturnInvoiceDetailDataForReport(string invNo)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string invNoClause = BuildInClause("@ReturnInvoiceNo", invNo, parameters);
            string query = @"SELECT IV.ReturnInvoiceNo as InvoiceNo,IV.ReturnInvoiceId as InvoiceId,IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
                            " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.ReturnInvoiceId " +
                             " FROM dbo.tblReturnInvoiceDetail IVD LEFT JOIN dbo.tblReturnInvoice IV ON IVD.ReturnInvoiceId = IV.ReturnInvoiceId " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in " + invNoClause;
            return SInventorySql.GetDataTable(query, parameters);
        }


        public DataTable AllInvoiceForPrintingDAL(string ComUnitId, DateTime InvoiceDate)
        {
            string query = @"SELECT * FROM dbo.tblInvoice I LEFT JOIN dbo.tblCustMaster C ON I.CustomerMasterId = C.CustomerMasterId "+
                            " WHERE I.ComUnitId=@ComUnitId AND I.InvoiceDate=@InvoiceDate ORDER BY I.InvoiceId DESC" ;

            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", ComUnitId == null ? null : ComUnitId.Trim()),
                Param("@InvoiceDate", InvoiceDate)
            });
        }

        public DataTable InvoiceForDCPickingDAL(string ComUnitId, DateTime InvoiceDate)
        {
            string query = @"SELECT * FROM dbo.tblInvoice I LEFT JOIN dbo.tblCustMaster C ON I.CustomerMasterId = C.CustomerMasterId " +
                            " WHERE I.ComUnitId=@ComUnitId AND I.InvoiceDate=@InvoiceDate AND I.InvoiceNo NOT IN (SELECT InvoiceNo FROM dbo.tblDCPickingDetail)  ORDER BY I.InvoiceId DESC";

            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", ComUnitId == null ? null : ComUnitId.Trim()),
                Param("@InvoiceDate", InvoiceDate)
            });
        }


        public bool DcPickingSaveDAL(DCPicking aDcPicking)
        {
            string insertQuery = @" INSERT INTO dbo.tblDCPicking "+
                          "  ( DCPicId , "+
                           "    DCPicNo , "+
                          "     DCPicDate , "+
                           "    ComUnitId ,"+
                           "    AreaId " +
                          "   ) "+
                   "  VALUES  (@DCPicId, @DCPicNo, @DCPicDate, @ComUnitId, @AreaId)";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                Param("@DCPicId", aDcPicking.DCPicId),
                Param("@DCPicNo", aDcPicking.DCPicNo),
                Param("@DCPicDate", aDcPicking.DCPicDate),
                Param("@ComUnitId", aDcPicking.ComUnitId),
                Param("@AreaId", aDcPicking.AreaId)
            });
        }
        public bool UpdateOrder(string  status,string id)
        {
            string insertQuery = @"UPDATE dbo.tblOrderDetail SET Status=@Status WHERE OrderDetailId=@OrderDetailId";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                Param("@Status", status),
                Param("@OrderDetailId", id)
            });
        }

        public bool DcPickingDetailSaveDAL(DCPickingDetail aDcPickingDetail)
        {
            string insertQuery = @"INSERT INTO dbo.tblDCPickingDetail "+
                         "   ( DCPicDetailId, InvoiceNo, DCPicId ) "+
                   "    VALUES  (@DCPicDetailId, @InvoiceNo, @DCPicId)";

            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                Param("@DCPicDetailId", aDcPickingDetail.DCPicDetailId),
                Param("@InvoiceNo", aDcPickingDetail.InvoiceNo),
                Param("@DCPicId", aDcPickingDetail.DCPicId)
            });
        }


        public DataTable AllPickingForReportList(string comUnitId,DateTime pickDate)
        {
            string query = @"select * from tblDCPicking where DCPicDate=@DCPicDate and ComUnitId=@ComUnitId order by DCPicId desc";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@DCPicDate", pickDate),
                Param("@ComUnitId", comUnitId == null ? null : comUnitId.Trim())
            });
        }


        public DataTable DCPickingReportMainDataDAL(string dcPickingNo)
        {
            string query = @"SELECT P.DCPicNo,P.DCPicDate,CU.ComUnitCode,CU.ComUnitName,CU.Address,A.AreaCode,A.AreaName FROM tblDCPicking P LEFT JOIN dbo.tblCompanyUnit CU ON P.ComUnitId = CU.ComUnitId LEFT JOIN dbo.tblArea A ON P.AreaId=A.AreaId " +
                            " WHERE P.DCPicNo=@DCPicNo";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@DCPicNo", dcPickingNo == null ? null : dcPickingNo.Trim())
            });
        }

        public DataTable  DCPickingReportDetailDataDAL(string dcPickingNo)
        {

            string query = @"SELECT IND.ProductCode,IND.ProductName,IND.BatchNo, SUM(TotalQuantity) AS TotalPickQty FROM dbo.tblInvoiceDetail IND LEFT JOIN dbo.tblInvoice I ON IND.InvoiceId = I.InvoiceId  "+
                            " WHERE I.InvoiceNo IN (SELECT InvoiceNo FROM dbo.tblDCPickingDetail LEFT JOIN dbo.tblDCPicking "+
                            " ON dbo.tblDCPickingDetail.DCPicId = dbo.tblDCPicking.DCPicId WHERE tblDCPicking.DCPicNo=@DCPicNo)  " +
                            " GROUP BY IND.ProductCode,IND.ProductName,IND.BatchNo ";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@DCPicNo", dcPickingNo == null ? null : dcPickingNo.Trim())
            });
        }

        public void AreaDropDownLoad(DropDownList dropDownList, string comUnitId)
        {
            string query = @"SELECT A.* FROM dbo.tblArea A LEFT JOIN dbo.tblDistrict D ON A.DistrictId = D.DistrictId "+
                                " LEFT JOIN dbo.tblCompanyUnit C ON D.ComUnitId = C.ComUnitId "+
                                " WHERE C.ComUnitId='" + comUnitId.Trim() + "'";
            aCommonInternalDal.LoadDropDownValue(dropDownList, "AreaName", "AreaId", query, "SSIDB");

        }
        public DataTable InvoiceNoCount(string comUnitId)
        {
            string query = @"SELECT (isnull(MAX(InvoiceId),0)+1)CountNo FROM dbo.tblInvoice ";

            //string query = @"SELECT  (ISNULL(MAX(CAST((SUBSTRING(InvoiceNo,10,11)) AS INT)),0)+1) CountNo FROM dbo.tblInvoice WHERE ComUnitId ='" + comUnitId.Trim() + "'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ReturnInvoiceNoCount(string comUnitId)
        {
            string query = @"SELECT count(ReturnInvoiceNo) CountNo FROM dbo.tblReturnInvoice WHERE ComUnitId=@ComUnitId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", comUnitId == null ? null : comUnitId.Trim())
            });
        }
        public DataTable DcPickingNoCount(string comUnitId)
        {
            string query = @"SELECT count(DCPicNo) CountNo FROM dbo.tblDCPicking WHERE ComUnitId=@ComUnitId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", comUnitId == null ? null : comUnitId.Trim())
            });
        }

        public DataTable MarketPickinReport(string SC, int MarketID, int ManufacID, DateTime InvDate)
        {
            string query = @" SELECT tblMarket.MarketName,M.MiaName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity " +
                        " FROM dbo.tblInvoice I " +
                           " INNER JOIN View_CustomerMaster C  ON I.CustomerMasterId = C.CustomerMasterId " +
                        " INNER JOIN dbo.tblMIAInfo M ON C.MiaId = M.MiaId " +
                        " INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId " +
                        " INNER JOIN dbo.tblMarket ON C.MarketId=dbo.tblMarket.MarketId  " +
                        " INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode  " +
                       " WHERE  I.ComUnitId='" + SC + "' and p.ManufacId='" + ManufacID + "' and InvoiceDate='" + InvDate + "' and tblMarket.MarketId='" + MarketID + "' GROUP BY tblMarket.MarketName,M.MiaName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize ";
            //  " I.ComUnitId= '2' AND p.ManufacId='1' AND tblMarket.MarketId='9' AND InvoiceDate='7/31/2017 12:00:00 AM'  ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }




        public DataTable DeliveryInvoiceMainDataForReportDAL(string invNo)
        {
            string query = @"SELECT IV.InvoiceId,IV.DelivaryInvoiceNo AS InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal AS TpTotal,IV.DeliveryTpVat AS TpVat,(IV.DeliveryTpDiscount  +isnull(IV.DelivarySpecialAmount ,0))TpDiscount,IV.DeliveryTpGrandTotal AS TpGrandTotal,CU.ComUnitCode,CU.ComUnitName , " +
                         "        (CU.Address +' ' +',M-'+CU.MobileNo+',P-'+CU.PhoneNo+',F-'+CU.FaxNo) AS CUAddress,  " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address+' '+CM.CellNo) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,U.UserName  " +
                        " FROM tblInvoice IV  " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
               
                       " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DeliveryInvoiceDetailDataForReportDAL(string invNo)
        {
            string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.DeliveryQuantity AS Quantity,IVD.UnitPrice, IVD.UnitVatAmount,IVD.DeliveryTotalPrice,IVD.DeliveryTotalPriceVatAmount,(IVD.DeliveryDiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, (IVD.DeliveryDiscountAmount+IVD.DelivarySpecialAmount)DiscountAmount,IVD.DeliveryNetAmount AS NetAmount,IV.InvoiceId   " +
                            " LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                            " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProformaReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {

            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,I.FixedCustomer,Campaign AS ProductOffer,
CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,
CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount,I.AreaCode
,I.RegionCode as MiaCode,I.DisCode as DistrictCode,I.MarketCode,I.Types as IntransitDay
,I.MarketName,I.CustomerType as Type
FROM dbo.tblInvoice I with(nolock)
INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
        where CU.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "' 		UNION ALL SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,I.FixedCustomer,CampaignType AS ProductOffer,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode as DistrictCode,I.MarketCode,C.Type as IntransitDay ,I.MarketName,I.CustomerType as Type FROM dbo.tblSubInvoiceMaster I with(nolock) INNER JOIN dbo.tblSubInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId INNER JOIN dbo.tblSubDepotStore DS ON DS.SubDCStoreId = ID.SubDCStoreId where CU.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";

            
            
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable ProformaReportDAl( DateTime fromDate, DateTime toDate)
        {

            string query =
                       @"SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,I.FixedCustomer,Campaign AS ProductOffer,
CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,
CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount,I.AreaCode
,I.RegionCode as MiaCode,I.DisCode as DistrictCode,I.MarketCode,I.Types as IntransitDay
,I.MarketName,I.CustomerType as Type
FROM dbo.tblInvoice I with(nolock)
INNER JOIN dbo.tblInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId
INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId
INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId
INNER JOIN dbo.tblDCStore DS ON DS.DCStoreId = ID.DCStoreId
       where I.InvoiceDate between '" + fromDate + "' and '" + toDate + "' 	UNION ALL SELECT CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,I.FixedCustomer,CampaignType  AS ProductOffer, CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,ID.ProductCode,ID.ProductName,ID.PackSize,ID.BatchNo,CONVERT(VARCHAR,DS.ExpDate,103) ExpDate,ID.Quantity,NetAmount,TotalPriceVatAmount,DiscountAmount,ID.SpecialAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode as DistrictCode,I.MarketCode,C.Type as IntransitDay ,I.MarketName,I.CustomerType as Type FROM dbo.tblSubInvoiceMaster I with(nolock) INNER JOIN dbo.tblSubInvoiceDetail ID ON ID.InvoiceId = I.InvoiceId INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId INNER JOIN dbo.tblSubDepotStore DS ON DS.SubDCStoreId = ID.SubDCStoreId where I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable InvoiceMainDataForReport(string invNo, string Code)
        {
            string query = @"SELECT '" + Code + @"' AS TopSheetGenCode, 
    ISNULL(NULLIF(ord.OrderSenderName, ''), ISNULL(NULLIF(ord.MIOName, ''), ISNULL(NULLIF(IV.OrderSenderName, ''), ISNULL(NULLIF(IV.MIAName, ''), emp.EmpName)))) AS ReturnInvoiceId,
    DeliveryDate, 
    ord.CustomerMasterId,
    IV.AdjustAmount,
    IV.ReceivableAmount,
    IV.FixedCustomer,
    da.Name AS DeliveryPersonName,
    ISNULL(CM.CellNo, da.PhoneNo) AS DeliveryPersonPhNo,
    IV.InvoiceId,
    IV.InvoiceNo,
    IV.InvoiceDate,
    IV.OrderNo,
    IV.OrderDate,
    ct.CustomerType AS TpTotal,
    IV.TpVat,
    (IV.TpDiscount + ISNULL(IV.TotalSpecialAmount, 0)) AS TpDiscount, 
    (ISNULL(tbIn.NetAmount, 0) + ISNULL(IV.DeliveryCharge, 0)) AS TpGrandTotal,
    ISNULL(IV.DeliveryCharge, 0) AS DeliveryCharge,
    CU.ComUnitCode,
    CU.ComUnitName AS CompanyName,  
    (CU.Address) AS CUAddress,  
    CM.CustomerCode,
    CM.CustomerName, 
    (CM.Address) AS CMAddress, 
    ISNULL(NULLIF(IV.CustomerDeliveryAddress, ''), ISNULL(NULLIF(CM.Addrees2, ''), CM.Address)) AS Addrees2,
    ptt.ProgramTypeName AS CategoryName,
    ISNULL(PT.PaymentTypeName, ISNULL(ord.PaymentType, CM.TermOfPayment)) AS PaymentTypeName,  
    ISNULL(NULLIF(ord.OrderSenderCode, ''), ISNULL(NULLIF(ord.MIOCode, ''), ISNULL(NULLIF(IV.OrderSenderCode, ''), ISNULL(NULLIF(IV.MIACode, ''), emp.EmpMasterCode)))) AS MiaCode,
    ISNULL(NULLIF(ord.OrderSenderName, ''), ISNULL(NULLIF(ord.MIOName, ''), ISNULL(NULLIF(IV.OrderSenderName, ''), ISNULL(NULLIF(IV.MIAName, ''), emp.EmpName)))) AS MiaName,
    ISNULL(NULLIF(U.UserName, ''), ISNULL(NULLIF(IV.CreateBy, ''), '')) AS UserName,

    -- Remarks Handling (NULL ba Empty space thakle 'N/A' dekhabe)
    CASE 
        WHEN LTRIM(RTRIM(ISNULL(ord.Remarks, ''))) = '' THEN 'N/A'
        ELSE ord.Remarks 
    END AS Remarks,

    -- Dynamic Fields from Stored Procedure
    ISNULL(TS.Due, 0) AS TotalDue,
    CASE 
        WHEN ISNULL(LM.DayLimit, 0) - ISNULL(DATEDIFF(DAY, LD.InvoiceDate, GETDATE()), 0) < 0 THEN 0
        ELSE ISNULL(LM.DayLimit, 0) - ISNULL(DATEDIFF(DAY, LD.InvoiceDate, GETDATE()), 0)
    END AS RemainingCreditDays

FROM tblInvoice IV
LEFT JOIN (
    SELECT InvoiceId, SUM(tblInvoiceDetail.NetAmount) AS NetAmount 
    FROM tblInvoiceDetail 
    GROUP BY InvoiceId 
) tbIn ON tbIn.InvoiceId = IV.InvoiceId
INNER JOIN dbo.tblOrder ord ON IV.OrderId = ord.OrderId
LEFT JOIN tblProgramType ptt ON ord.ProgramTypeId = ptt.ProgramTypeId
LEFT JOIN tblCustomertype ct ON ord.CusttypeId = ct.CustomerTypeId
LEFT JOIN tblCompanyUnit CU ON ord.ComUnitId = CU.ComUnitId
LEFT JOIN tblDAInfo da ON IV.Inv_DANameId = da.DAId
LEFT JOIN tblCustMaster CM ON ord.CustomerMasterId = CM.CustomerMasterId
LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId = PT.PaymentTypeId
LEFT JOIN dbo.tblEmpGeneralInfo emp ON (ord.MIOId IS NOT NULL AND ord.MIOId = emp.EmpInfoId) OR (NULLIF(ord.OrderSenderCode, '') IS NOT NULL AND ord.OrderSenderCode = emp.EmpMasterCode)
LEFT JOIN tblUser U ON IV.UserId = U.UserId

-- Total Due Subquery (TS)
LEFT JOIN (
    SELECT 
        INV.CustomerMasterId, 
        INV.ComUnitId,
        SUM(ISNULL(DeliveryTpGrandTotal, 0)) - (
            SUM(ISNULL(PaymentAmount, 0)) + 
            SUM(ISNULL(AIT, 0)) + 
            SUM(ISNULL(DiscountOnPayment, 0)) +
            SUM(ISNULL(RTN.ReturnAmount, 0))
        ) AS Due
    FROM tblInvoice AS INV
    LEFT JOIN (
        SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount 
        FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS M
        LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
        WHERE ISNULL(M.IsInventoryValuation, 0) = 0 AND M.TableID = 2018 
        GROUP BY M.CommonPK_ID
    ) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
    GROUP BY INV.CustomerMasterId, INV.ComUnitId
    HAVING SUM(ISNULL(DeliveryTpGrandTotal, 0)) > 0
) AS TS ON ord.CustomerMasterId = TS.CustomerMasterId AND CU.ComUnitId = TS.ComUnitId

-- Credit Limit Subquery (LM)
LEFT JOIN (
    SELECT 
        CompanyId, 
        CustomerMasterId, 
        SUM(ISNULL(LimitAmount, 0)) AS LimitAmount, 
        SUM(ISNULL(DayLimit, 0)) AS DayLimit 
    FROM tblCustomerCreditLimit 
    WHERE IsActive = 1 AND LTRIM(RTRIM(UPPER(ActionStatus))) = LTRIM(RTRIM(UPPER('Approved'))) 
    GROUP BY CompanyId, CustomerMasterId
) AS LM ON ord.CustomerMasterId = LM.CustomerMasterId AND CU.CompanyId = LM.CompanyId

-- Oldest Unpaid Invoice Subquery (LD)
LEFT JOIN (
    SELECT 
        INV.CustomerMasterId, 
        CI.CompanyId,
        MIN(InvoiceDate) AS InvoiceDate 
    FROM tblInvoice AS INV
    LEFT JOIN tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
    LEFT JOIN (
        SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount 
        FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS M
        LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
        WHERE ISNULL(M.IsInventoryValuation, 0) = 0 AND M.TableID = 2018 
        GROUP BY M.CommonPK_ID
    ) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
    WHERE ISNULL(DeliveryTpGrandTotal, 0) > 0 
      AND (ISNULL(DeliveryTpGrandTotal, 0) - (
            ISNULL(PaymentAmount, 0) + ISNULL(AIT, 0) + ISNULL(DiscountOnPayment, 0) + ISNULL(RTN.ReturnAmount, 0)
          )) > 1
    GROUP BY INV.CustomerMasterId, CI.CompanyId
) AS LD ON ord.CustomerMasterId = LD.CustomerMasterId AND CU.CompanyId = LD.CompanyId

WHERE IV.InvoiceNo IN (" + invNo.Trim() + ") ORDER BY IV.InvoiceNo";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable InvoiceMainDataForReport2(string invNo)
        {
            string query = @"SELECT IV.CustomerMasterId, 
    ISNULL(NULLIF(tblOrder.OrderSenderName, ''), ISNULL(NULLIF(tblOrder.MIOName, ''), ISNULL(NULLIF(IV.OrderSenderName, ''), ISNULL(NULLIF(IV.MIAName, ''), MIA.EmpName)))) as ReturnInvoiceId,
    IV.AdjustAmount,IV.ReceivableAmount,IV.FixedCustomer,
    da.Name DeliveryPersonName,
    ISNULL(CM.CellNo, da.PhoneNo) as DeliveryPersonPhNo,
    IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,
    IV.CustomerType as TpTotal,IV.TpVat,
    (IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,
    (ISNULL(tbIn.NetAmount, 0) + ISNULL(IV.DeliveryCharge, 0)) AS TpGrandTotal,
    ISNULL(IV.DeliveryCharge, 0) AS DeliveryCharge,
    CU.ComUnitCode,CU.ComUnitName as CompanyName, (CU.Address) AS CUAddress, 
    tblOrder.CustomerCode,tblOrder.CustomerName, (CM.Address) AS CMAddress,
    ISNULL(NULLIF(IV.CustomerDeliveryAddress, ''), ISNULL(NULLIF(CM.Addrees2, ''), CM.Address)) AS Addrees2,
    IV.Types as CategoryName, 
    ISNULL(PT.PaymentTypeName, ISNULL(tblOrder.PaymentType, CM.TermOfPayment)) as PaymentTypeName, 
    ISNULL(NULLIF(tblOrder.OrderSenderCode, ''), ISNULL(NULLIF(tblOrder.MIOCode, ''), ISNULL(NULLIF(IV.OrderSenderCode, ''), ISNULL(NULLIF(IV.MIACode, ''), MIA.EmpMasterCode)))) as MiaCode,
    ISNULL(NULLIF(tblOrder.OrderSenderName, ''), ISNULL(NULLIF(tblOrder.MIOName, ''), ISNULL(NULLIF(IV.OrderSenderName, ''), ISNULL(NULLIF(IV.MIAName, ''), MIA.EmpName)))) as MiaName,
    ISNULL(NULLIF(U.UserName, ''), ISNULL(NULLIF(IV.CreateBy, ''), '')) as UserName 
FROM tblInvoice IV with (nolock)
left join (select InvoiceId, sum(tblInvoiceDetail.NetAmount) NetAmount from tblInvoiceDetail group by InvoiceId )tbIn on tbIn.InvoiceId=IV.InvoiceId
LEFT JOIN tblCompanyUnit CU with (nolock) ON IV.ComUnitId = CU.ComUnitId 
LEFT JOIN tblCustMaster CM with (nolock) ON IV.CustomerMasterId=CM.CustomerMasterId 
LEFT JOIN tblPaymentType PT with (nolock) ON IV.PaymentTypeId=PT.PaymentTypeId 
LEFT JOIN tblUser U with (nolock) ON IV.UserId=U.UserId 
LEFT JOIN tblDAInfo da ON IV.Inv_DANameId = da.DAId
LEFT JOIN tblOrder with (nolock) on tblOrder.OrderId =IV.OrderId 
LEFT JOIN dbo.tblEmpGeneralInfo MIA with (nolock) ON (tblOrder.MIOId IS NOT NULL AND tblOrder.MIOId = MIA.EmpInfoId) OR (NULLIF(tblOrder.OrderSenderCode, '') IS NOT NULL AND tblOrder.OrderSenderCode = MIA.EmpMasterCode)
" +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " " + invNo.Trim() + " order by IV.InvoiceNo asc ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }


        public DataTable InvoiceMainDatainvNo(string invNo)
        {
            string query = @"SELECT IV.CustomerMasterId, 
    ISNULL(NULLIF(tblOrder.OrderSenderName, ''), ISNULL(NULLIF(tblOrder.MIOName, ''), ISNULL(NULLIF(IV.OrderSenderName, ''), ISNULL(NULLIF(IV.MIAName, ''), MIA.EmpName)))) as ReturnInvoiceId,
    IV.AdjustAmount,IV.ReceivableAmount,IV.FixedCustomer ,IV.DeliveryPersonName,
    ISNULL(CM.CellNo, IV.DeliveryPersonPhNo) as DeliveryPersonPhNo,
    IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,
    IV.CustomerType as TpTotal,IV.TpVat,
    (IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,
    (ISNULL(tbIn.NetAmount, 0) + ISNULL(IV.DeliveryCharge, 0)) AS TpGrandTotal,
    ISNULL(IV.DeliveryCharge, 0) AS DeliveryCharge,
    CU.ComUnitCode,CU.ComUnitName as CompanyName, (CU.Address) AS CUAddress, 
    tblOrder.CustomerCode,tblOrder.CustomerName, (CM.Address) AS CMAddress,
    ISNULL(NULLIF(IV.CustomerDeliveryAddress, ''), ISNULL(NULLIF(CM.Addrees2, ''), CM.Address)) AS Addrees2,
    IV.Types as CategoryName,
    ISNULL(PT.PaymentTypeName, ISNULL(tblOrder.PaymentType, CM.TermOfPayment)) as PaymentTypeName, 
    ISNULL(NULLIF(tblOrder.OrderSenderCode, ''), ISNULL(NULLIF(tblOrder.MIOCode, ''), ISNULL(NULLIF(IV.OrderSenderCode, ''), ISNULL(NULLIF(IV.MIACode, ''), MIA.EmpMasterCode)))) as MiaCode,
    ISNULL(NULLIF(tblOrder.OrderSenderName, ''), ISNULL(NULLIF(tblOrder.MIOName, ''), ISNULL(NULLIF(IV.OrderSenderName, ''), ISNULL(NULLIF(IV.MIAName, ''), MIA.EmpName)))) as MiaName,
    ISNULL(NULLIF(U.UserName, ''), ISNULL(NULLIF(IV.CreateBy, ''), '')) as UserName,
    DeliveryDate, 
    CASE 
        WHEN LTRIM(RTRIM(ISNULL(tblOrder.Remarks, ''))) = '' THEN 'N/A'
        ELSE tblOrder.Remarks 
    END AS Remarks,
    ISNULL(TS.Due, 0) AS TotalDue,
    CASE 
        WHEN ISNULL(LM.DayLimit, 0) - ISNULL(DATEDIFF(DAY, LD.InvoiceDate, GETDATE()), 0) < 0 THEN 0
        ELSE ISNULL(LM.DayLimit, 0) - ISNULL(DATEDIFF(DAY, LD.InvoiceDate, GETDATE()), 0)
    END AS RemainingCreditDays
FROM tblInvoice IV   with (nolock)
left join (select InvoiceId, sum(tblInvoiceDetail.NetAmount) NetAmount from tblInvoiceDetail group by InvoiceId )tbIn on tbIn.InvoiceId=IV.InvoiceId
 LEFT JOIN tblCompanyUnit CU   with (nolock) ON IV.ComUnitId = CU.ComUnitId 
  LEFT JOIN tblCustMaster CM  with (nolock) ON IV.CustomerMasterId=CM.CustomerMasterId 
   LEFT JOIN tblPaymentType PT  with (nolock) ON IV.PaymentTypeId=PT.PaymentTypeId 
    LEFT JOIN tblUser U  with (nolock) ON IV.UserId=U.UserId  
	 LEFT JOIN tblOrder  with (nolock) on tblOrder.OrderId =IV.OrderId 
    LEFT JOIN dbo.tblEmpGeneralInfo MIA  with (nolock) ON (tblOrder.MIOId IS NOT NULL AND tblOrder.MIOId = MIA.EmpInfoId) OR (NULLIF(tblOrder.OrderSenderCode, '') IS NOT NULL AND tblOrder.OrderSenderCode = MIA.EmpMasterCode)  

-- Total Due Subquery (TS)
LEFT JOIN (
    SELECT 
        INV.CustomerMasterId, 
        INV.ComUnitId,
        SUM(ISNULL(DeliveryTpGrandTotal, 0)) - (
            SUM(ISNULL(PaymentAmount, 0)) + 
            SUM(ISNULL(AIT, 0)) + 
            SUM(ISNULL(DiscountOnPayment, 0)) +
            SUM(ISNULL(RTN.ReturnAmount, 0))
        ) AS Due
    FROM tblInvoice AS INV
    LEFT JOIN (
        SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount 
        FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS M
        LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
        WHERE ISNULL(M.IsInventoryValuation, 0) = 0 AND M.TableID = 2018 
        GROUP BY M.CommonPK_ID
    ) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
    GROUP BY INV.CustomerMasterId, INV.ComUnitId
    HAVING SUM(ISNULL(DeliveryTpGrandTotal, 0)) > 0
) AS TS ON tblOrder.CustomerMasterId = TS.CustomerMasterId AND CU.ComUnitId = TS.ComUnitId

-- Credit Limit Subquery (LM)
LEFT JOIN (
    SELECT 
        CompanyId, 
        CustomerMasterId, 
        SUM(ISNULL(LimitAmount, 0)) AS LimitAmount, 
        SUM(ISNULL(DayLimit, 0)) AS DayLimit 
    FROM tblCustomerCreditLimit 
    WHERE IsActive = 1 AND LTRIM(RTRIM(UPPER(ActionStatus))) = LTRIM(RTRIM(UPPER('Approved'))) 
    GROUP BY CompanyId, CustomerMasterId
) AS LM ON tblOrder.CustomerMasterId = LM.CustomerMasterId AND CU.CompanyId = LM.CompanyId

-- Oldest Unpaid Invoice Subquery (LD)
LEFT JOIN (
    SELECT 
        INV.CustomerMasterId, 
        CI.CompanyId,
        MIN(InvoiceDate) AS InvoiceDate 
    FROM tblInvoice AS INV
    LEFT JOIN tblCompanyUnit AS CI ON INV.ComUnitId = CI.ComUnitId
    LEFT JOIN (
        SELECT M.CommonPK_ID, SUM(DebitAmountBDT) AS ReturnAmount 
        FROM CSTL_ACCDB_Unicorn..tblDebitCreditVoucherMaster AS M
        LEFT JOIN CSTL_ACCDB_Unicorn..tblDebitCreditVoucherDetail AS D ON M.VoucherMasterId = D.VoucherMasterId
        WHERE ISNULL(M.IsInventoryValuation, 0) = 0 AND M.TableID = 2018 
        GROUP BY M.CommonPK_ID
    ) AS RTN ON INV.InvoiceId = RTN.CommonPK_ID
    WHERE ISNULL(DeliveryTpGrandTotal, 0) > 0 
      AND (ISNULL(DeliveryTpGrandTotal, 0) - (
            ISNULL(PaymentAmount, 0) + ISNULL(AIT, 0) + ISNULL(DiscountOnPayment, 0) + ISNULL(RTN.ReturnAmount, 0)
          )) > 1
    GROUP BY INV.CustomerMasterId, CI.CompanyId
) AS LD ON tblOrder.CustomerMasterId = LD.CustomerMasterId AND CU.CompanyId = LD.CompanyId
where IV.InvoiceNo in ('" + invNo.Trim() + "')  order by IV.InvoiceNo";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable RInvoiceMainDataForReport(string invNo)
        {
            string query = @"SELECT IV.FixedCustomer ,IV.DeliveryPersonName,IV.DeliveryPersonPhNo,IV.InvoiceId,IV.ReturnInvoiceNo as InvoiceNo,IV.CreateDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
                         " (CU.Address) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CM.Addrees2,IV.Types as CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,CM.MarketName as UserName " +
                        " FROM tblReturnInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable FindInvoiceCode(string invNo)
        {
            string query = @"select ReturnInvoiceNo from tblReturnInvoice where ReturnInvoiceId in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        //public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.InvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
        //                 " (CU.Address) AS CUAddress, " +
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
        //                " MIA.MiaCode,MIA.MiaName,U.UserName " +
        //                " FROM tblInvoice IV " +
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable InvoiceDetailDataForReport(string invNo)
        {
            //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
            //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
            //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
            //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
            //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,IVD.BatchNo,ISNULL(IVD.ExpDate, dc.ExpDate) AS ExpDate,IVD.PackSize as BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
                            "  IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
                            "  (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.InvoiceId  " +
                            "   FROM dbo.tblInvoiceDetail IVD " +
                            "   LEFT JOIN dbo.tblDCStore dc ON dc.DCStoreId = IVD.DCStoreId " +
                            "   LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
                     "  INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId " +
                    "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode   WHERE IV.InvoiceNo in ('" + invNo.Trim() + "') ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable InvoiceDetailDataForReport2(string invNo)
        {
            //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
            //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
            //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
            //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
            //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,dc.BatchNo,ISNULL(dc.ExpDate, IVD.ExpDate) AS ExpDate,IVD.PackSize as BonusQuantity,IVD.Quantity,IVD.UnitPrice,   IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage,   (IVD.DiscountAmount+IVD.SpecialAmount) DiscountAmount,IVD.NetAmount,IV.InvoiceId     FROM dbo.tblInvoiceDetail IVD
   LEFT JOIN dbo.tblDCStore dc  ON dc.DCStoreId = IVD.DCStoreId  
   LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId 
   INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId  
   
     left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                      " " + invNo.Trim() + "  order by IVD.ProductName asc";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }



        public DataTable InvoiceDetailDataInvoID(string invNo)
        {
            //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
            //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
            //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
            //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
            //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,dc.BatchNo,ISNULL(dc.ExpDate, IVD.ExpDate) AS ExpDate,IVD.PackSize as BonusQuantity,IVD.Quantity,IVD.UnitPrice,   IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage,   (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.InvoiceId     FROM dbo.tblInvoiceDetail IVD
 LEFT JOIN dbo.tblDCStore dc  ON dc.DCStoreId = IVD.DCStoreId  
 LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId 
 LEFT JOIN (
     SELECT CustomerMasterId, ProductCode, MAX(DiscountPercentage) AS DiscountPercentage
     FROM dbo.tblProductDiscount
     GROUP BY CustomerMasterId, ProductCode
 ) PD ON PD.CustomerMasterId = IV.CustomerMasterId AND PD.ProductCode = IVD.ProductCode   WHERE IV.InvoiceNo in ('" + invNo.Trim() + "')  order by IVD.ProductName asc";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable RInvoiceDetailDataForReport(string invNo)
        {
            //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
            //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
            //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
            //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
            //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,IVD.BatchNo,IVD.PackSize as BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
                            "  IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
                            "  (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.InvoiceId  " +
                            "   FROM dbo.tblReturnInvoiceDetail IVD  LEFT JOIN dbo.tblReturnInvoice IV ON IVD.ReturnInvoiceId = IV.ReturnInvoiceId  " +
                     " INNER JOIN dbo.tblReturnInvoice I ON I.ReturnInvoiceId = IV.ReturnInvoiceId  " +
                    "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
                      " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable ReturnReturnInvoiceMainDataForReport(string invNo)
        {
            string query = @"SELECT IV.ReturnInvoiceId as InvoiceId,IV.ReturnInvoiceNo as InvoiceNo,IV.ReturnInvoiceDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.TpTotal,IV.TpVat,(IV.TpDiscount+isnull(IV.TotalSpecialAmount,0))TpDiscount,IV.TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName , " +
                         " (CU.Address ) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,U.UserName " +
                        " FROM tblReturnInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                //" WHERE IV.ReturnInvoiceNo='" + invNo.Trim() + "'";
                       " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        public DataTable ReturnReturnInvoiceDetailDataForReport(string invNo)
        {
            string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, " +
                            "  IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
                            "  (IVD.DiscountAmount+IVD.SpecialAmount)DiscountAmount,IVD.NetAmount,IV.ReturnInvoiceId as  InvoiceId  " +
                            "   FROM dbo.tblReturnInvoiceDetail IVD LEFT JOIN dbo.tblReturnInvoice IV ON IVD.ReturnInvoiceId = IV.ReturnInvoiceId " +
                     "  INNER JOIN dbo.tblReturnInvoice I ON I.ReturnInvoiceId = IV.ReturnInvoiceId " +
                    "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                //" WHERE IV.ReturnInvoiceNo='" + invNo.Trim() + "'";
                      " WHERE IV.ReturnInvoiceNo in (" + invNo.Trim() + ") ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        ///////////////////////////////////////////////////////////////////////////////
        //public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.DelivaryInvoiceNo,IV.InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal,IV.DeliveryTpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
        //                 " (CU.Address) AS CUAddress, " +
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
        //                " MIA.MiaCode,MIA.MiaName,U.UserName " +
        //                " FROM tblInvoice IV " +
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        //public DataTable DelivaryInvoiceDetailDataForReport(string invNo)
        //{
        //    //string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.Quantity,IVD.UnitPrice, "+
        //    //                " IVD.UnitVatAmount,IVD.TotalPrice,IVD.TotalPriceVatAmount,IVD.DiscountPercentage,IVD.DiscountAmount,IVD.NetAmount,IV.InvoiceId " +
        //    //                 " FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //    //           //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //    //           " WHERE IV.InvoiceNo in (" + invNo.Trim() + ") ";

        //    string query = @"SELECT IVD.ProductCode,(IVD.ProductName+':'+IVD.PackSize) AS Product,IVD.BatchNo,IVD.BonusQuantity,IVD.DeliveryQuantity,IVD.UnitPrice, " +
        //                    "  IVD.UnitVatAmount,IVD.DeliveryTotalPrice,IVD.DeliveryTotalPriceVatAmount,(IVD.DiscountPercentage+ISNULL(PD.DeliveryDiscountPercentage,0))DiscountPercentage, " +
        //                    "  (IVD.DeliveryDiscountAmount+IVD.DelivarySpecialAmount)DiscountAmount,IVD.DeliveryNetAmount,IV.InvoiceId  " +
        //                    "   FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
        //             "  INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId " +
        //            "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //              " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") ";

        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        //public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        //{
        //    string query = @"SELECT IV.InvoiceId,IV.DelivaryInvoiceNo as InvoiceNo,IV.UpdateDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal,IV.DeliveryTpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
        //                 " (CU.Address) AS CUAddress, " +
        //                " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CC.CategoryName,PT.PaymentTypeName, " +
        //                " MIA.MiaCode,MIA.MiaName,U.UserName " +
        //                " FROM tblInvoice IV " +
        //                " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
        //                " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
        //                " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
        //                " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
        //                " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
        //                " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
        //        //" WHERE IV.InvoiceNo='" + invNo.Trim() + "'";
        //               " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ")   ";
        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}
        public DataTable MarketPickinReport(string SC, int MarketID, int ManufacID, DateTime InvDate, string parameter)
        {
            string query = @" SELECT tblMarket.MarketName + ', Territory Name : ' + A.AreaName as MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity " +
                        " FROM dbo.tblInvoice I " +
                           " INNER JOIN View_CustomerMaster C  ON I.CustomerMasterId = C.CustomerMasterId " +
                        " INNER JOIN dbo.tblMIAInfo M ON C.MiaId = M.MiaId " +
                        " INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId " +
                        " INNER JOIN dbo.tblMarket ON C.MarketId=dbo.tblMarket.MarketId   INNER JOIN tblArea A ON A.AreaCode = I.AreaCode  " +
                        " INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode  " +
                       " WHERE  I.ComUnitId='" + SC + "' and p.ManufacId='" + ManufacID + "' and InvoiceDate='" + InvDate + "' and tblMarket.MarketId='" + MarketID + "' " + parameter + " GROUP BY tblMarket.MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,A.AreaName order by ProductName";
            //  " I.ComUnitId= '2' AND p.ManufacId='1' AND tblMarket.MarketId='9' AND InvoiceDate='7/31/2017 12:00:00 AM'  ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable MarketPickinReport(string SC, string t, int ManufacID, DateTime InvDate, string parameter)
        {
            string query = @" SELECT tblMarket.MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity " +
                        " FROM dbo.tblInvoice I " +
                           " INNER JOIN View_CustomerMaster C  ON I.CustomerMasterId = C.CustomerMasterId " +
                        " INNER JOIN dbo.tblMIAInfo M ON C.MiaId = M.MiaId " +
                        " INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId " +
                        " INNER JOIN dbo.tblMarket ON C.MarketId=dbo.tblMarket.MarketId  " +
                        " INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode  " +
                       " WHERE  I.ComUnitId='" + SC + "' and p.ManufacId='" + ManufacID + "' and InvoiceDate='" + InvDate + "' and I.AreaCode='" + t + "' " + parameter + " GROUP BY tblMarket.MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize order by ProductName";
            //  " I.ComUnitId= '2' AND p.ManufacId='1' AND tblMarket.MarketId='9' AND InvoiceDate='7/31/2017 12:00:00 AM'  ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DelivaryInvoiceDetailDataForReport(string invNo)
        {

            string query = @"SELECT IVD.ProductCode,(IVD.ProductName) AS Product,IVD.BatchNo,IVD.PackSize as BonusQuantity,IVD.DeliveryQuantity as Quantity,IVD.UnitPrice, " +
                            "  IVD.UnitVatAmount,IVD.DeliveryTotalPrice as TotalPrice,IVD.DeliveryTotalPriceVatAmount as TotalPriceVatAmount,(IVD.DeliveryDiscountPercentage+ISNULL(PD.DiscountPercentage,0))DiscountPercentage, " +
                            "  (IVD.DeliveryDiscountAmount+IVD.DelivarySpecialAmount)DiscountAmount,IVD.DeliveryNetAmount as NetAmount,IV.InvoiceId  " +
                            "   FROM dbo.tblInvoiceDetail IVD LEFT JOIN dbo.tblInvoice IV ON IVD.InvoiceId = IV.InvoiceId " +
                     "  INNER JOIN dbo.tblInvoice I ON I.InvoiceId = IV.InvoiceId " +
                    "   left JOIN dbo.tblProductDiscount PD ON PD.CustomerMasterId = I.CustomerMasterId AND PD.ProductCode = IVD.ProductCode " +
                      " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ") and IVD.DeliveryStatus IN ('Full','Partial') ";

            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable DelivaryInvoiceMainDataForReport(string invNo)
        {
            string query = @"SELECT IV.DeliveryPersonName,IV.DeliveryPersonPhNo,IV.InvoiceId,IV.DelivaryInvoiceNo as InvoiceNo,IV.UpdateDate as InvoiceDate,IV.OrderNo,IV.OrderDate,IV.DeliveryTpTotal as TpTotal,IV.DeliveryTpVat as TpVat,(IV.DeliveryTpDiscount+isnull(IV.DelivarySpecialAmount,0))TpDiscount,IV.DeliveryTpGrandTotal as TpGrandTotal,CU.ComUnitCode,CU.ComUnitName as CompanyName, " +
                         " (CU.Address) AS CUAddress, " +
                        " CM.CustomerCode,CM.CustomerName, (CM.Address) AS CMAddress,CM.Addrees2,CC.CategoryName,PT.PaymentTypeName, " +
                        " MIA.MiaCode,MIA.MiaName,CM.MarketName as UserName " +
                        " FROM tblInvoice IV " +
                        " LEFT JOIN tblCompanyUnit CU ON IV.ComUnitId = CU.ComUnitId " +
                        " LEFT JOIN tblCustMaster CM ON IV.CustomerMasterId=CM.CustomerMasterId " +
                        " LEFT JOIN tblPaymentType PT ON IV.PaymentTypeId=PT.PaymentTypeId " +
                        " LEFT JOIN tblMIAInfo MIA ON IV.MiaId=MIA.MiaId " +
                        " LEFT JOIN tblUser U ON IV.UserId=U.UserId " +
                        " LEFT JOIN dbo.tblCustCategory CC ON CM.CategoryId=CC.CategoryId " +
                        " WHERE IV.DelivaryInvoiceNo in (" + invNo.Trim() + ")   ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public bool UpdateDCStock(decimal StockQty, int DCStoreId)
        {
            string query = @"UPDATE tblDCStore SET StockQty=StockQty + @StockQty WHERE DCStoreId=@DCStoreId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                Param("@StockQty", StockQty),
                Param("@DCStoreId", DCStoreId)
            });
        }

        public bool ReturnToDCStore(decimal dCStoreId, decimal returnQty, int invoiceDetailId)
        {
            string updateQuery = @"INSERT INTO dbo.tblDCStore 
(
    DCStoreId, StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
    TotalQuantity, ExpDate, MfgDate, ReceiveDate, ChalanNo, ChalanDate,
    ComUnitId, StockQty, DamageQty, StockRcvDate, ReqId, ReqChildId,
    StockCondition, StockInTransfarId,
    Trans_Type, CostPerUnit, TotalCostAmount,
    ReferenceDCStoreId, ReferenceTransactionDate,
    ReturnFromInvoiceDetailId
)
SELECT
    (SELECT ISNULL(MAX(DCStoreId), 0) + 1 FROM tblDCStore),
    StorageLocation, ProductCode, ProductName, PackSize, BatchNo,
    0,
    ExpDate, MfgDate, GETDATE(), ChalanNo, ChalanDate,
    ComUnitId, @ReturnQty,
    0, GETDATE(), ReqId, ReqChildId,
    'DeliveryReturn', StockInTransfarId,
    'Receive',
    CostPerUnit,
    @ReturnQty * CostPerUnit,
    CASE WHEN Trans_Type = 'Receive' THEN DCStoreId ELSE ReferenceDCStoreId END,
    GETDATE(),
    @InvoiceDetailId
FROM tblDCStore
WHERE DCStoreId = @DCStoreId";

            return SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                Param("@ReturnQty", returnQty),
                Param("@DCStoreId", dCStoreId),        
                Param("@InvoiceDetailId", invoiceDetailId)
            });
        }

        public bool DCStockInDAL(DCStockNew aDcStockNew)
        {
            string query = @"INSERT INTO dbo.tblDCStoreFreeze
                (DCStoreId, DCStoreFreezeId, InvoiceDetailId, StorageLocation, ProductCode, ProductName,
                 PackSize, BatchNo, TotalQuantity, ExpDate, ReceiveDate, ChalanNo, ChalanDate, ComUnitId,
                 StockQty, DamageQty, StockRcvDate, ReqId, ReqChildId, StockInTransfarId, StockCondition,
                 ChalanDetailsId)
                VALUES
                (@DCStoreId, @DCStoreFreezeId, @InvoiceDetailId, @StorageLocation, @ProductCode, @ProductName,
                 @PackSize, @BatchNo, @TotalQuantity, @ExpDate, @ReceiveDate, @ChalanNo, @ChalanDate, @ComUnitId,
                 @StockQty, @DamageQty, @StockRcvDate, @ReqId, @ReqChildId, @StockInTransfarId, @StockCondition,
                 @ChalanDetailsId)";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                Param("@DCStoreId", aDcStockNew.DCStoreId),
                Param("@DCStoreFreezeId", aDcStockNew.DCStoreFreezeId),
                Param("@InvoiceDetailId", aDcStockNew.InvoiceDetailId),
                Param("@StorageLocation", aDcStockNew.StorageLocation),
                Param("@ProductCode", aDcStockNew.ProductCode),
                Param("@ProductName", aDcStockNew.ProductName),
                Param("@PackSize", aDcStockNew.PackSize),
                Param("@BatchNo", aDcStockNew.BatchNo),
                Param("@TotalQuantity", aDcStockNew.TotalQuantity),
                Param("@ExpDate", aDcStockNew.ExpDate),
                Param("@ReceiveDate", aDcStockNew.ReceiveDate),
                Param("@ChalanNo", aDcStockNew.ChalanNo),
                Param("@ChalanDate", aDcStockNew.ChalanDate),
                Param("@ComUnitId", aDcStockNew.ComUnitId),
                Param("@StockQty", aDcStockNew.StockQty),
                Param("@DamageQty", aDcStockNew.DamageQty),
                Param("@StockRcvDate", aDcStockNew.StockRcvDate),
                Param("@ReqId", aDcStockNew.ReqId),
                Param("@ReqChildId", aDcStockNew.ReqChildId),
                Param("@StockInTransfarId", aDcStockNew.StockInTransfarId),
                Param("@StockCondition", "ReturnStock"),
                Param("@ChalanDetailsId", aDcStockNew.ChalanDetailsId)
            });
        }
        public DataTable MArketwiseIntransitReportDAl(string districtId, DateTime fromDate, DateTime toDate, string market,string miacode)
        {
            string miacodea = string.IsNullOrEmpty(miacode) ? "NULL" : "'" + miacode + "'";
            string marketa = string.IsNullOrEmpty(market) ? "NULL" : "'" + market + "'";

            string query =
                       @"SELECT '" + fromDate + "' as fromdate ,'" + toDate + "' as todate ,C.Address as ComUnitCode,CU.ComUnitName," +
                       "C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo, CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate, " +
                       "tblDetails.NetAmount AS NetAmount,tblDetails.UnitVatAmount AS TotalPriceVatAmount,tblDetails.TotalPriceVatAmount  AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode," +
                       "I.DisCode AS DistrictCode ,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE," +
                       "I.MIAName as MainMIONAME,C.Type as SpecialAmount FROM dbo.tblInvoice I WITH(nolock) " +
                       "INNER JOIN ( select InvoiceId,((Sum(TotalPrice)+Sum(TotalPriceVatAmount))-Sum(DiscountAmount))NetAmount,Sum(TotalPriceVatAmount)UnitVatAmount,(Sum(DiscountAmount))TotalPriceVatAmount " +
                       "from dbo.tblInvoiceDetail group by  InvoiceId) tblDetails ON I.InvoiceId = tblDetails.InvoiceId INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId " +
                       "where I.TpTotal>0 AND I.DelivaryInvoiceNo IS NULL and I.AreaCode=ISNULL(" + marketa + ",I.AreaCode)  and I.MiaCode=ISNULL(" + miacodea + ",I.MiaCode)  and CU.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "' " +
                      
                       "UNION ALL SELECT '" + fromDate + "' as fromdate ,'" + toDate + "' as todate ," +
                       "C.Address as ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo," +
                       "CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo, CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate, I.TpGrandTotal " +
                       "AS NetAmount,I.TpVat AS TotalPriceVatAmount,I.TpDiscount AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode," +
                       "I.DisCode AS DistrictCode ,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) " +
                       "IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME,C.Type as SpecialAmount " +
                       "FROM dbo.tblSubInvoiceMaster I WITH(nolock) " +
                       "INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId " +
                       "INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId where I.TpTotal>0 " +
                       "AND I.DelivaryInvoiceNo IS NULL and  I.AreaCode=ISNULL(" + marketa + ",I.AreaCode) AND I.MiaCode=ISNULL(" + miacodea + ",I.MiaCode)  and CU.ComUnitId='" + districtId.Trim() + "'  " +
                       "and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "' ORDER BY DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) DESC";



            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        //public DataTable IntransitReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        //{
        //    string query =
        //               @"SELECT '" + fromDate + "' as fromdate ,'" + toDate + "' as todate ,tblDetails.DeliveryNetAmount AS  ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate,tblDetails.NetAmount AS NetAmount,tblDetails.UnitVatAmount AS TotalPriceVatAmount,tblDetails.TotalPriceVatAmount  AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode AS DistrictCode,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME,I.CustomerType as SpecialAmount FROM dbo.tblInvoice I WITH(nolock) INNER JOIN ( select InvoiceId,((Sum(TotalPrice)+Sum(TotalPriceVatAmount))-Sum(DiscountAmount))NetAmount,((Sum(DeliveryTotalPrice)+Sum(DeliveryTotalPriceVatAmount))-Sum(DeliveryDiscountAmount))DeliveryNetAmount,Sum(TotalPriceVatAmount)UnitVatAmount,(Sum(DiscountAmount))TotalPriceVatAmount from dbo.tblInvoiceDetail group by  InvoiceId) tblDetails ON I.InvoiceId = tblDetails.InvoiceId INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN tblCustMaster C ON C.CustomerMasterId = I.CustomerMasterId where I.TpTotal>0   and CU.ComUnitId='" + districtId.Trim() + "' and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'" +
        //               " UNION ALL SELECT '" + fromDate + "' as fromdate ,'" + toDate + "' as todate ,CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo, CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate, I.TpGrandTotal AS NetAmount,I.TpVat AS TotalPriceVatAmount,I.TpDiscount AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode AS DistrictCode ,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME,I.CustomerType as SpecialAmount FROM dbo.tblSubInvoiceMaster I WITH(nolock) INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN tblCustMaster C ON C.CustomerMasterId = I.CustomerMasterId where I.TpTotal>0   and CU.ComUnitId='" + districtId.Trim() + "'  and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";
            
         

        //    return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        //}



        public DataTable IntransitReportDAl(string districtId, string fromDate, string toDate)
        {
            try
            {
                accessManager.SqlConnectionOpen(DataBase.SalesDB);
                List<SqlParameter> aSqlParameterlist = new List<SqlParameter>();
                aSqlParameterlist.Add(new SqlParameter("@districtId", districtId));
                aSqlParameterlist.Add(new SqlParameter("@fromDate", fromDate));
                aSqlParameterlist.Add(new SqlParameter("@toDate", toDate));

                DataTable dt = accessManager.GetDataTable("sp_Get_IntransitReportList", aSqlParameterlist);
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

        public DataTable AgingIntransitReportDAl(string districtId, DateTime fromDate, DateTime toDate)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();

            aSqlParameterList.Add(new SqlParameter("@districtId", districtId));
            aSqlParameterList.Add(new SqlParameter("@fromDate", fromDate));
            aSqlParameterList.Add(new SqlParameter("@toDate", toDate));


            return aCommonInternalDal.GetDataTableAction("sp_Get_AgingReceivableReport", aSqlParameterList, "SSIDB");
        }
        public DataTable IntransitReportDAl(DateTime fromDate, DateTime toDate)
        {
            string query =
                       @"SELECT '" + fromDate + "' as fromdate ,'" + toDate + "' as todate ,CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo,CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate, tblDetails.NetAmount AS NetAmount,tblDetails.UnitVatAmount AS TotalPriceVatAmount,tblDetails.TotalPriceVatAmount  AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode AS DistrictCode ,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME,I.CustomerType as SpecialAmount FROM dbo.tblInvoice I WITH(nolock) INNER JOIN ( select InvoiceId,((Sum(TotalPrice)+Sum(TotalPriceVatAmount))-Sum(DiscountAmount))NetAmount,Sum(TotalPriceVatAmount)UnitVatAmount,(Sum(DiscountAmount))TotalPriceVatAmount from dbo.tblInvoiceDetail group by  InvoiceId) tblDetails ON I.InvoiceId = tblDetails.InvoiceId INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId where I.TpTotal>0 AND I.DelivaryInvoiceNo IS NULL    and I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'UNION ALL SELECT '" + fromDate + "' as fromdate ,'" + toDate + "' as todate ,CU.ComUnitCode,CU.ComUnitName,C.CustomerCode,C.CustomerName,I.OrderNo,CONVERT(VARCHAR,I.OrderDate,103) OrderDate,I.InvoiceNo, CONVERT(VARCHAR,I.InvoiceDate,103) InvoiceDate, I.TpGrandTotal AS NetAmount,I.TpVat AS TotalPriceVatAmount,I.TpDiscount AS DiscountAmount,I.AreaCode,I.RegionCode as MiaCode,I.DisCode AS DistrictCode ,I.MarketCode,I.MarketName,DATEDIFF(DAY,DATEADD(day, -1, InvoiceDate), GETDATE()) IntransitDay,I.MIACode as MainMIOCODE,I.MIAName as MainMIONAME,I.CustomerType as SpecialAmount FROM dbo.tblSubInvoiceMaster I WITH(nolock) INNER JOIN dbo.tblCompanyUnit CU ON CU.ComUnitId = I.ComUnitId INNER JOIN View_CustomerMaster C ON C.CustomerMasterId = I.CustomerMasterId where I.TpTotal>0 AND I.DelivaryInvoiceNo IS NULL  and  I.InvoiceDate between '" + fromDate + "' and '" + toDate + "'";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public int DeleteInvoice(string Invoice)
        {
            List<SqlParameter> aSqlParameterList = new List<SqlParameter>();
            aSqlParameterList.Add(new SqlParameter("@OrderID", Invoice));
            return aCommonInternalDal.RunStoreProcedure("sp_Deletenvoice", aSqlParameterList, "SSIDB");
        }
        public bool SaveDataForReturnAmount(ReturnAmountDAO amountDao)
        {
            string insertQuery = @" INSERT INTO dbo.tblReturnAmount " +
     "   ( CustomerId , " +
                "     InvoiceId , " +
     //"        ReturnInvoiceId , " +
      "       Amount  " +
       "    ) " +
 "  VALUES  (@CustomerId, @InvoiceId, @Amount)";

            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                Param("@CustomerId", amountDao.CustomerId),
                Param("@InvoiceId", amountDao.InvoiceId),
                Param("@Amount", amountDao.Amount)
            });
        }

        //Pulak

        public DataTable LoadInvoiceWithDetail(string invoiceId)
        {
            string query = @"SELECT '0'SL,ProductCode,OrderDetailsId,ProductName,'0'StockQty,UnitPrice,UnitVatAmount as UnitVAT,Quantity,TotalPrice,DiscountPercentage,DiscountAmount,''IsCampaignProduct,TpVat as VAT,NetAmount as NetPrice,''ISGiftProduct,TotalQuantity as TotalQty,*  FROM tblInvoice
            left join tblInvoiceDetail on tblInvoice.InvoiceId=tblInvoiceDetail.InvoiceId
            where tblInvoiceDetail.InvoiceId=@InvoiceId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@InvoiceId", invoiceId)
            });
        }
        public DataTable LoadInvoice(string invoicenNo)
        {
            string query = @"select * from tblInvoice where InvoiceNo=@InvoiceNo";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@InvoiceNo", invoicenNo)
            });
        }
        public DataTable LoadInvoiceDetailData(string invoiceId)
        {
            string query = "select * from tblInvoiceDetail where InvoiceId=@InvoiceId";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@InvoiceId", invoiceId)
            });
        }
        public void DeleteInvoice(string invoiceId, string invoicedetailId)
        {
            string updateQuery = @"delete from tblInvoice where InvoiceId=@InvoiceId; delete from tblInvoiceDetail where InvoiceDetailId=@InvoiceDetailId";
            SInventorySql.Execute(updateQuery, new List<SqlParameter>
            {
                Param("@InvoiceId", invoiceId),
                Param("@InvoiceDetailId", invoicedetailId)
            });
        }
        //public void UpdateDCStoreQuantity(string dCStoreId, decimal Quantity)
        //{
        //    string updateQuery = @"UPDATE tblDCStore SET StockQty='" + Quantity + "' WHERE DCStoreId='" + dCStoreId.Trim() + "'  ";
        //    aCommonInternalDal.UpdateDataByUpdateCommand(updateQuery, "SSIDB");
        //}
        public DataTable InvoiceDetailDataForReportNew(int Dcid, int ManufId, int market,string tr, DateTime invDate)
        {
            string query = @"    SELECT  I.CustomerType, CellNo,(Addrees2 + '[' +  Address +']')		 as Address	,tblMarket.MarketName + ', Territory Name : ' + A.AreaName as MarketName ,* 				
FROM tblInvoice I
INNER JOIN (SELECT DISTINCT D.InvoiceId, ManufacId FROM dbo.tblInvoice I
            INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId
            INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode
            ) as tblD ON I.InvoiceId = tblD.InvoiceId  
 INNER JOIN dbo.View_CustomerMaster C ON I.CustomerMasterId = C.CustomerMasterId
 INNER JOIN dbo.tblMarket ON C.MarketCode=dbo.tblMarket.MarketCode    INNER JOIN tblArea A ON A.AreaCode = I.AreaCode   
where I.ComUnitId=@ComUnitId and tblD.ManufacId=@ManufacId and tblMarket.MarketId=@MarketId and I.AreaCode=@AreaCode and InvoiceDate=@InvoiceDate order by OrderNo";


            return SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                Param("@ComUnitId", Dcid),
                Param("@ManufacId", ManufId),
                Param("@MarketId", market),
                Param("@AreaCode", tr),
                Param("@InvoiceDate", invDate)
            });
        }


        public DataTable InvoiceDetailDataForReportNew_(string invNoid, string Route, string invDat, string Code)
        {
            string query = @"     SELECT  '" + Code + @"' TopSheetGenCode,Ct.CustomerType CustomerType, CellNo,(C.Address + '[' +  C.Address +']')		 as Address	,DACode+' : '+ Name as MarketName ,tblD.ManufacId as TpGrandTotal,* 				
FROM tblInvoice I  with (nolock)
INNER JOIN (SELECT DISTINCT D.InvoiceId,sum(NetAmount)ManufacId  FROM dbo.tblInvoice I  with (nolock)
            INNER JOIN dbo.tblInvoiceDetail D  with (nolock) ON I.InvoiceId = D.InvoiceId
            INNER JOIN dbo.tblProduct P  with (nolock) ON D.ProductCode = P.ProductCode
             group by  D.InvoiceId  ) as tblD ON I.InvoiceId = tblD.InvoiceId  
 INNER JOIN dbo.tblCustMaster C  with (nolock) ON I.CustomerMasterId = C.CustomerMasterId
   INNER JOIN dbo.tblOrder ord  with (nolock) ON I.OrderId = ord.OrderId  
  left JOIN dbo.tblCustomerType Ct  with (nolock) ON ord.CustTypeId = Ct.CustomerTypeId
  LEFT JOIN tblDAInfo AS DL ON I.Inv_DANameId = DL.DAId
  
   LEFT JOIN dbo.tblRouteInformationMaster rt  with (nolock) ON rt.RouteInformationMasterId = ord.DistributionRouteId 
where I.InvoiceId in (" + invNoid + ")   order by I.InvoiceNo asc";


            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable MarketPickinReportNew(string SC,int Mrk, string t, int ManufacID, DateTime InvDate, string parameter)
        {
            string query = @" SELECT tblMarket.MarketName + ', Territory Name : ' + A.AreaName as MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity " +
                        " FROM dbo.tblInvoice I " +
                           " INNER JOIN View_CustomerMaster C  ON I.CustomerMasterId = C.CustomerMasterId " +
                        " INNER JOIN dbo.tblMIAInfo M ON C.MiaId = M.MiaId " +
                        " INNER JOIN dbo.tblInvoiceDetail D ON I.InvoiceId = D.InvoiceId " +
                        " INNER JOIN dbo.tblMarket ON C.MarketId=dbo.tblMarket.MarketId  " +
                        " INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode   INNER JOIN tblArea A ON A.AreaCode = I.AreaCode " +
                       " WHERE  I.ComUnitId='" + SC + "' and p.ManufacId='" + ManufacID + "' and  tblMarket.MarketId='" + Mrk + "' and InvoiceDate='" + InvDate + "' and I.AreaCode='" + t + "' " + parameter + " GROUP BY tblMarket.MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo,D.PackSize,A.AreaName ";
            //  " I.ComUnitId= '2' AND p.ManufacId='1' AND tblMarket.MarketId='9' AND InvoiceDate='7/31/2017 12:00:00 AM'  ";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

        public DataTable MarketPickinReportNew_(string invNoid, string Route, string InvDate)
        {
            string query = @"   select distinct * from (SELECT   'DA Name : ' +  DACode+' : '+ Name as MarketName,I.InvoiceDate,D.ProductCode,D.ProductName,D.BatchNo as BatchNo,D.PackSize,SUM(D.Quantity) AS Quantity  FROM dbo.tblInvoice I   with (nolock)
  INNER JOIN tblCustMaster C  with (nolock)  ON I.CustomerMasterId = C.CustomerMasterId  
						   
						 INNER JOIN dbo.tblInvoiceDetail D  with (nolock) ON I.InvoiceId = D.InvoiceId  
						INNER JOIN dbo.tblOrder ord  with (nolock) ON I.OrderId = ord.OrderId   
      INNER JOIN dbo.tblProduct P ON D.ProductCode = P.ProductCode   
						  LEFT JOIN tblDAInfo AS DL ON I.Inv_DANameId = DL.DAId
   where I.InvoiceId in (" + invNoid + @")   

                         GROUP BY DACode,I.InvoiceDate,D.ProductCode,D.ProductName , D.BatchNo , D.PackSize ,DL.Name  ) tbl order by  ProductName asc";
        
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
    }

}
