using System;

namespace SalesReport.Models
{
    public class CoverageReportModel
    {
        public string NodeID { get; set; }
        public string ParentNodeID { get; set; }
        public string Code { get; set; }                // Maps to SP's "ID / Code"
        public string Name { get; set; }                // Maps to SP's "Salesman / Customer Name"
        public string TerritoryRegion { get; set; }      // Maps to SP's "Territory / Region"
        public string TotalAssigned { get; set; }        // Maps to SP's "Total Assigned"
        public string Visited { get; set; }                // Maps to SP's "Visited (DCR Done)"
        public string Missed { get; set; }                 // Maps to SP's "Missed / No DCR"
        public string CoverageStatus { get; set; }       // Maps to SP's "Coverage % / Status"
        public decimal TotalSalesAmount { get; set; }    // Maps to SP's "Total Sales Amount (BDT)"
        public int ReportLevel { get; set; }             // 1 for Parent/Salesman, 2 for Child/Customer
        public int IsGroup { get; set; }                 // 1 for Parent, 0 for Child
    }
}
