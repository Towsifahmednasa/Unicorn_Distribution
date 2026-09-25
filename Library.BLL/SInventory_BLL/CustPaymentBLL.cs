using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CustPaymentBLL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        CustPaymentDAL aCustPaymentDal=new CustPaymentDAL();

        public bool UpdateAdjustment(int invoiceId)
        {
            return aCustPaymentDal.UpdateAdjustment(invoiceId);
        }
        public bool UpdateInvoiceFinalPayment(int invoiceId,decimal PaymentAmount,string ptStatus, string PaymentBy)
        {
            return aCustPaymentDal.UpdateInvoiceFinalPayment(invoiceId, PaymentAmount, ptStatus, PaymentBy);
        }

        public bool PaymentJournalPosting(int custPayId, int invoiceId)
        {
            return aCustPaymentDal.PaymentJournalPosting(custPayId,invoiceId);
        }

        public bool IsPaymentAlreadyProcessed(int invoiceId, decimal paymentAmount)
        {
            return aCustPaymentDal.IsPaymentAlreadyProcessed(invoiceId, paymentAmount);
        }

        public bool UpdateInvoiceFinalPaymentTx(int invoiceId, decimal paymentAmount, string ptStatus, string paymentBy)
        {
            return aCustPaymentDal.UpdateInvoiceFinalPaymentTx(invoiceId, paymentAmount, ptStatus, paymentBy);
        }

        public bool IsPaymentJournalPosted(int custPayId)
        {
            return aCustPaymentDal.IsPaymentJournalPosted(custPayId);
        }

        public bool SaveCustPaymentTx(CustPayment aCustPayment, CustPaymentDetail aCustPaymentDetail)
        {
            // Transaction-aware save: one header + its own detail, with the DAL row-count
            // results actually checked (SaveCustPayment/SaveCustDetail return false when the
            // insert was skipped, e.g. by the detail duplicate guard).
            ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
            aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");
            if (!aCustPaymentDal.SaveCustPayment(aCustPayment))
            {
                return false;
            }
            aCustPaymentDetail.CustPayId = aCustPayment.CustPayId;
            aCustPaymentDetail.CustPayDetailId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayDetailId", "tblCustPayDetail");
            return aCustPaymentDal.SaveCustDetail(aCustPaymentDetail);
        }
        public bool SaveCustPayment(CustPayment aCustPayment,List<CustPaymentDetail> aPaymentDetailList )
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");
                aCustPaymentDal.SaveCustPayment(aCustPayment);
                foreach (var custPaymentDetail in aPaymentDetailList)
                {
                    custPaymentDetail.CustPayId = aCustPayment.CustPayId;
                    SaveCustDetail(custPaymentDetail);
                }
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public bool SaveCustPayment(CustPayment aCustPayment, CustPaymentDetail custPaymentDetail)
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");


                DataTable dt = aCustPaymentDal.Existence(custPaymentDetail.InvoiceId.ToString(), custPaymentDetail.PaymentAmount.ToString());


                if( dt.Rows.Count > 0 )
                {

                  

                }
                else
                {
                      aCustPaymentDal.SaveCustPayment(aCustPayment);
                    custPaymentDetail.CustPayId = aCustPayment.CustPayId;
                    SaveCustDetail(custPaymentDetail);
                }

                
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public bool SubdeportSaveCustPayment(CustPayment aCustPayment, CustPaymentDetail custPaymentDetail)
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
                aCustPayment.CustPayId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayId", "tblCustomerPay");
                aCustPaymentDal.SaveCustPayment(aCustPayment);

                custPaymentDetail.CustPayId = aCustPayment.CustPayId;
                SubdeportSaveCustDetail(custPaymentDetail);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public bool UpdateInvoicePaymentAmount(string amount, string status, string id)
        {
            return aCustPaymentDal.UpdateInvoicePaymentAmount(amount, status, id);
        }
        public bool SubdeportUpdateInvoicePaymentAmount(string amount, string status, string id)
        {
            return aCustPaymentDal.SubdeportUpdateInvoicePaymentAmount(amount, status, id);
        }
      
        public DataTable GetPrevAmount(string invoiceId)
        {
            return aCustPaymentDal.GetPrevAmount(invoiceId);
        }
        public bool SaveCustDetail(CustPaymentDetail aCustPaymentDetail)
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                aCustPaymentDetail.CustPayDetailId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayDetailId", "tblCustPayDetail");
                aCustPaymentDal.SaveCustDetail(aCustPaymentDetail);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public bool SubdeportSaveCustDetail(CustPaymentDetail aCustPaymentDetail)
        {
            try
            {

                ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();

                aCustPaymentDetail.CustPayDetailId = aClsPrimaryKeyFind.PrimaryKeyMax("CustPayDetailId", "tblCustPayDetail");
                aCustPaymentDal.SubdeportSaveCustDetail(aCustPaymentDetail);

                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }
        public void LoadSC(DropDownList ddl)
        {
            aCustPaymentDal.LoadSC(ddl);
        }
        public void LoadSC(DropDownList ddl, string userId)
        {
            aCustPaymentDal.LoadSC(ddl,userId);
        }
        public void LoadManufac(DropDownList ddl)
        {
            aCustPaymentDal.LoadManufac(ddl);
            
        }
        
        public void LoadCustomerMaster(DropDownList ddl, string marketId)
        {
            aCustPaymentDal.LoadCustomerMaster(ddl,marketId);
        }
        public void LoadMarket(DropDownList ddl, string comunitId)
        {
            aCustPaymentDal.LoadMarket(ddl,comunitId);
        }
        public void PaymentTypeLoadBLL(DropDownList aDropDownList)
        {
            aCustPaymentDal.PaymentTypeLoad(aDropDownList);
        }
        public DataTable LoadInvoice(string comUnitId, string customerId, string marketId)
        {
            return aCustPaymentDal.LoadInvoice(comUnitId, customerId, marketId);
        }

        public DataTable LoadInvoice(string comUnitId, string marketId)
        {
            return aCustPaymentDal.LoadInvoice(comUnitId, marketId);
        }
        public DataTable LoadSubDeportInvoice(string comUnitId, string marketId)
        {
            return aCustPaymentDal.LoadSubDeportInvoice(comUnitId, marketId);
        }
        public CustomerMaster CustomerLoad(string aCustomerMaster)
        {
            return aCustPaymentDal.CustomerLoad(aCustomerMaster);
        }
        public CustomerMaster DetailCustomerLoad(string aCustomerMaster)
        {
            return aCustPaymentDal.DetailCustomerLoad(aCustomerMaster);
        }
        public Product DetailProductLoad(string aProduct)
        {
            return aCustPaymentDal.DetailProductLoad(aProduct);
        }

        public void LoadBankAccount(DropDownList ddlBank, string companyId)
        {
            aCustPaymentDal.LoadBankAccount(ddlBank, companyId);
        }

        public void LoadCashAccount(DropDownList ddlCash, string companyId)
        {
            aCustPaymentDal.LoadCashAccount(ddlCash, companyId);
        }
    }
}
