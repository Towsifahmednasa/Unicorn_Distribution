using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.InternalCls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

namespace Library.BLL.SInventory_BLL
{
    public class CentralStoreBLL
    {
        ClsPrimaryKeyFind aClsPrimaryKeyFind = new ClsPrimaryKeyFind();
        CentralStoreDAL aReceiveDal = new CentralStoreDAL();
        public string SaveStockReceive(CurrentStock aCurrentStock,List<CentralStore> aStockReceiveList  )
        {
            if (!aReceiveDal.HasProductName(aCurrentStock))
            {
                aCurrentStock.StockId = aClsPrimaryKeyFind.PrimaryKeyMax("StockId", "tblCurrentStock");
                if (aReceiveDal.SaveDataForCurrentStock(aCurrentStock))
                {

                    foreach (var aStockRecieve in aStockReceiveList)
                    {
                        aStockRecieve.ReceiveId = aClsPrimaryKeyFind.PrimaryKeyMax("ReceiveId", "tblCentralStore");
                        aStockRecieve.ReceiveId = aCurrentStock.StockId;
                        if (!aReceiveDal.SaveDataForStockReceive(aStockRecieve))
                        {
                            return "Data Save successfully";
                        }
                    }
                }
                else
                {
                    return "Data not Save";
                }
            }
            else
            {
                DataTable dataTableStockQty = new DataTable();
                dataTableStockQty = aReceiveDal.CurrentStockQty(aCurrentStock);
                decimal currentQty = Convert.ToDecimal(dataTableStockQty.Rows[0]["Quantity"].ToString());
                decimal newQty = Convert.ToDecimal(aCurrentStock.Quantity);
                aReceiveDal.UpdateCurrentStockQuantity(aCurrentStock.ProductCode, Convert.ToString(currentQty + newQty));
                foreach (var aStockRecieve in aStockReceiveList)
                {
                    aStockRecieve.ReceiveId = aClsPrimaryKeyFind.PrimaryKeyMax("ReceiveId", "tblCentralStore");
                    aStockRecieve.ReceiveId = aCurrentStock.StockId;
                    if (!aReceiveDal.SaveDataForStockReceive(aStockRecieve))
                    {
                        return "Data Save successfully";
                    }
                }
            }

            return "Data  save Successfully";
        }
       

        public DataTable CurrentStockReportBll()
        {

            return aReceiveDal.CurrentStockReport();
        }
        public bool UpdateDataForstockReceive(CentralStore astockReceive)
        {
            return aReceiveDal.UpdateStockReceive(astockReceive);
        }

        public DataTable LoadstockReceive()
        {
            return aReceiveDal.LoadStockReceiveView();
        }

        public CentralStore StockRecieve(string ReceiveId)
        {
            return aReceiveDal.StockReceiveEditLoad(ReceiveId);
        }
        public CentralStore CurrentStock(string StockId)
        {
            return aReceiveDal.StockReceiveEditLoad(StockId);
        }

        public DataTable ProductInfo(string productId)
        {
            return aReceiveDal.LoadProduct(productId);
        }

        public void LoadCategoryDropdown(DropDownList ddl)
        {
            aReceiveDal.LoadCategoryDropdown(ddl);
        }

        public void LoadProductDropdown(DropDownList ddl)
        {
            aReceiveDal.LoadProductDropdown(ddl);
        }

        public DataTable GetStockValuationLedger(string fromDate, string toDate, int? categoryId, int? productId)
        {
            return aReceiveDal.GetStockValuationLedger(fromDate, toDate, categoryId, productId);
        }

        public DataTable GetNationalStockValuation(string fromDate, string toDate, int? categoryId, int? productId)
        {
            return aReceiveDal.GetNationalStockValuation(fromDate, toDate, categoryId, productId);
        }

        public DataTable GetNationalStockValuationBatchDetails(string productCode, string locationName, string fromDate, string toDate)
        {
            return aReceiveDal.GetNationalStockValuationBatchDetails(productCode, locationName, fromDate, toDate);
        }
    }
}
