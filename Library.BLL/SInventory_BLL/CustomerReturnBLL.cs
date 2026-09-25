using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CustomerReturnBLL
    {
        CustomerReturnDAL aCustomerReturnDal = new CustomerReturnDAL();

        public int SaveCustomerReturn(CustomerReturnMasterDAO aCustomerReturnMasterDao)
        {
            return aCustomerReturnDal.SaveCustomerReturn(aCustomerReturnMasterDao);
        }
        public int SaveCustomerReturnDetail(CustomerReturnDetailDAO aCustomerReturnDetailDao)
        {
            return aCustomerReturnDal.SaveCustomerReturnDetail(aCustomerReturnDetailDao);
        }
        public DataTable LoadInvoiceNew(string delinvno)
        {
            return aCustomerReturnDal.LoadInvoiceNew(delinvno);
        }

        public bool UpdateDCStoreQty(string dcstoreId, int qty)
        {
            return aCustomerReturnDal.UpdateDCStoreQty(dcstoreId, qty);
        }

        public DataTable LoadInvoiceNewConsumer(string delinvno)
        {
            return aCustomerReturnDal.LoadInvoiceNewConsumer(delinvno);
        }

        public bool ApprovalUpdateDAL(CustomerReturnMasterDAO aCustomerReturnMasterDao)
        {
            return aCustomerReturnDal.ApprovalUpdateDAL(aCustomerReturnMasterDao);
        }
        public DataTable LoadCustomerReturnApproval(string actionstatus)
        {
            return aCustomerReturnDal.LoadCustomerReturnApproval(actionstatus);
        }

        public DataTable LoadInvoiceDataApproval(string id)
        {
            return aCustomerReturnDal.LoadInvoiceDataApproval(id);
        }

        public bool PostDeliverySalesReturn(int custReturnId)
        {
            return aCustomerReturnDal.PostDeliverySalesReturn(custReturnId);
        }

        public DataTable LoadCustomerInfoByCode(string custCode)
        {
            return aCustomerReturnDal.LoadCustomerInfoByCode(custCode);
        }
        public DataTable GetCompanyUnitById(string comUnitId)
        {
            return aCustomerReturnDal.GetCompanyUnitById(comUnitId);
        }
        public DataTable GetDANameById(string daId)
        {
            return aCustomerReturnDal.GetDANameById(daId);
        }
        public DataTable GetOrderSenderNameByOrderId(string orderId)
        {
            return aCustomerReturnDal.GetOrderSenderNameByOrderId(orderId);
        }
        public bool LockInvoiceForReturn(int invoiceId)
        {
            return aCustomerReturnDal.LockInvoiceForReturn(invoiceId);
        }
        public bool HasOverReturn(int invoiceId)
        {
            return aCustomerReturnDal.HasOverReturn(invoiceId);
        }
        public bool IsDeliverySalesReturnPosted(int custReturnId)
        {
            return aCustomerReturnDal.IsDeliverySalesReturnPosted(custReturnId);
        }
        public bool InsertDCStoreStockReturn(int invoiceDetailId, int returnQty, int salesReturnDetailId = 0)
        {
            return aCustomerReturnDal.InsertDCStoreStockReturn(invoiceDetailId, returnQty, salesReturnDetailId);
        }
    }
}
