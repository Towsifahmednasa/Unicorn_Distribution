using System;
using System.Collections.Generic;

namespace Library.DAO.SInventory_Entities
{
    // tblInvoiceBadDebtsMaster
    public class InvoiceAdjustment
    {
        public int BadDebtsId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int CustomerMasterId { get; set; }
        public int ComUnitId { get; set; }
        public string AdjustmentType { get; set; }   // 'Bad Debts' | 'Promotional Expense'
        public DateTime AdjustmentDate { get; set; }
        public decimal TotalAdjustment { get; set; }
        public string Remarks { get; set; }
        public string EntryBy { get; set; }
        public List<InvoiceAdjustmentDetail> Details = new List<InvoiceAdjustmentDetail>();
    }

    // tblInvoiceBadDebtsDetails
    public class InvoiceAdjustmentDetail
    {
        public int InvoiceDetailId { get; set; }
        public string ProductCode { get; set; }
        public decimal DeliveryNetAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
    }
}
