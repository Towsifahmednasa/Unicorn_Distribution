using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Library.DAL.SInventory_DAL;

namespace Library.DAL.SInventory_DAL
{
    public class StockTransferReportDAL
    {
        public DataTable LoadStockTransferReport(string fromDate, string toDate, string fromLocationId, string toLocationId)
        {
            string query = @"
                SELECT * FROM (
                    -- Source 1: Inter-Depot / DC-to-DC Challans (tblChalanInfo & tblChalanDetail)
                    SELECT
                        ci.ChalanDate                               AS TransferDate,
                        cd.ProductCode                             AS ProductCode,
                        cd.ProductName                             AS ProductName,
                        cd.BatchNo                                 AS BatchNo,
                        SUM(cd.Quantity)                            AS Quantity,
                        MAX(cd.UnitPrice)                          AS UnitPrice,
                        SUM(ISNULL(cd.Vat, 0))                     AS VATAmount,
                        SUM(ISNULL(cd.ValueWVat, cd.Value))        AS Amount,
                        ci.ChalanNo                                 AS RefNo,
                        ci.FromComUnitName                          AS FromLocation,
                        ci.ToComUnitName                            AS ToLocation,
                        ISNULL(ci.TrackNo, ci.ChalanNo)             AS STONo,
                        ci.ChalanId                                 AS ReqId,
                        ISNULL(ci.DriverName, '')                   AS UserName,
                        cuFrom.ComUnitId                            AS FromLocationId,
                        cuTo.ComUnitId                              AS ToLocationId
                    FROM dbo.tblChalanDetail cd
                    INNER JOIN dbo.tblChalanInfo ci ON cd.ChalanId = ci.ChalanId
                    LEFT JOIN dbo.tblCompanyUnit cuFrom ON (ci.FromComUnitId = cuFrom.ComUnitId OR ci.FromComUnitCode = cuFrom.ComUnitCode)
                    LEFT JOIN dbo.tblCompanyUnit cuTo ON (ci.ToComUnitCode = cuTo.ComUnitCode OR ci.ToComUnitName = cuTo.ComUnitName)
                    GROUP BY 
                        ci.ChalanDate, ci.ChalanNo, ci.FromComUnitName, ci.ToComUnitName, 
                        ci.TrackNo, ci.ChalanId, ci.DriverName, cuFrom.ComUnitId, cuTo.ComUnitId,
                        cd.ProductCode, cd.ProductName, cd.BatchNo

                    UNION ALL

                    -- Source 2: Warehouse to DC STO Transfers (tblRequisition & tblStockInTransfar)
                    SELECT
                        ci.ChalanDate                               AS TransferDate,
                        sit.ProductCode                             AS ProductCode,
                        (sit.ProductName + ' ' + ISNULL(sit.PackSize,'')) AS ProductName,
                        sit.BatchNo                                 AS BatchNo,
                        SUM(sit.Quantity)                           AS Quantity,
                        MAX(sit.UnitPrice)                          AS UnitPrice,
                        SUM(ISNULL(sit.VATAmount, 0))               AS VATAmount,
                        SUM(ISNULL(sit.TotalPriceAmount, 0))        AS Amount,
                        ci.ChalanNo                                 AS RefNo,
                        ci.FromComUnitName                          AS FromLocation,
                        ci.ToComUnitName                            AS ToLocation,
                        r.ReqNo                                     AS STONo,
                        r.ReqId                                     AS ReqId,
                        ISNULL(u.UserName, r.EntryBy)               AS UserName,
                        ci.FromComUnitId                            AS FromLocationId,
                        cuTo.ComUnitId                              AS ToLocationId
                    FROM dbo.tblStockInTransfar sit
                    INNER JOIN dbo.tblRequisition r      ON sit.ReqId = r.ReqId
                    INNER JOIN dbo.tblChalanInfo ci      ON r.IssueChalanNo = ci.ChalanNo
                    LEFT  JOIN dbo.tblCompanyUnit cuTo   ON (ci.ToComUnitCode = cuTo.ComUnitCode OR ci.ToComUnitName = cuTo.ComUnitName)
                    LEFT  JOIN dbo.tblUser u              ON r.EntryBy = u.LoginName
                    GROUP BY
                        ci.ChalanDate, sit.ProductCode, sit.ProductName, sit.PackSize, sit.BatchNo,
                        ci.ChalanNo, ci.FromComUnitName, ci.ToComUnitName, r.ReqNo, r.ReqId, u.UserName, r.EntryBy, ci.FromComUnitId, cuTo.ComUnitId
                ) AS T
                WHERE 1=1";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(fromDate))
            {
                query += " AND CONVERT(date, TransferDate) >= CONVERT(date, @FromDate)";
                parameters.Add(new SqlParameter("@FromDate", SInventorySql.DbValue(fromDate)));
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                query += " AND CONVERT(date, TransferDate) <= CONVERT(date, @ToDate)";
                parameters.Add(new SqlParameter("@ToDate", SInventorySql.DbValue(toDate)));
            }
            if (!string.IsNullOrEmpty(fromLocationId) && fromLocationId != "0")
            {
                query += " AND FromLocationId = @FromLocationId";
                parameters.Add(new SqlParameter("@FromLocationId", SInventorySql.DbValue(fromLocationId)));
            }
            if (!string.IsNullOrEmpty(toLocationId) && toLocationId != "0")
            {
                query += " AND ToLocationId = @ToLocationId";
                parameters.Add(new SqlParameter("@ToLocationId", SInventorySql.DbValue(toLocationId)));
            }

            query += " ORDER BY TransferDate DESC, RefNo, ProductName";

            return SInventorySql.GetDataTable(query, parameters);
        }

        public DataTable LoadCompanyUnits()
        {
            string query = @"
                SELECT ComUnitId, ComUnitName 
                FROM dbo.tblCompanyUnit 
                ORDER BY ComUnitName";
            return SInventorySql.GetDataTable(query, new List<SqlParameter>());
        }
    }
}
