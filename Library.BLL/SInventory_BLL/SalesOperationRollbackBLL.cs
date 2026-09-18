using System;
using Library.DAL.SInventory_DAL;

namespace Library.BLL.SInventory_BLL
{
    public class SalesOperationRollbackBLL
    {
        private SalesOperationRollbackDAL aRollbackDal = new SalesOperationRollbackDAL();

        public string RollbackProforma(string proformaCode, string loginUser, string remarks)
        {
            try
            {
                return aRollbackDal.RollbackProforma(proformaCode, loginUser, remarks);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string RollbackDeliveryInvoice(string deliveryInvoiceNo, string loginUser, string remarks)
        {
            try
            {
                return aRollbackDal.RollbackDeliveryInvoice(deliveryInvoiceNo, loginUser, remarks);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string RollbackCustomerPayment(string moneyReceiptNo, string loginUser, string remarks)
        {
            try
            {
                return aRollbackDal.RollbackCustomerPayment(moneyReceiptNo, loginUser, remarks);
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
