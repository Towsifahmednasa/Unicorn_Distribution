using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Library.DAL.DataManager;
using Library.DAL.InternalCls;
using Library.DAO.SInventory_Entities;

namespace Library.DAL.SInventory_DAL
{
    public class ProductUnitPriceDAL
    {
        private ClsCommonInternalDAL aCommonInternalDal = new ClsCommonInternalDAL();
        public bool SaveProductUnitPrice(ProductUnitPrice aProductUnitPrice)
        {
            string insertQuery = @"insert into tblUnitPrice (UnitPriceId,ProductId,ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,IsActive,ActiveDate,MRPPrice) 
            values (@UnitPriceId,@ProductId,@ProductCode,@ProductName,@PackSize,@CostPrice,@UnitPrice,@VATPercentage,@VATAmountPerUnit,@IsActive,@ActiveDate,@MRPPrice)";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                new SqlParameter("@UnitPriceId", aProductUnitPrice.UnitPriceId),
                new SqlParameter("@ProductId", aProductUnitPrice.ProductId),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aProductUnitPrice.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aProductUnitPrice.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aProductUnitPrice.PackSize)),
                new SqlParameter("@CostPrice", aProductUnitPrice.CostPrice),
                new SqlParameter("@UnitPrice", aProductUnitPrice.UnitPrice),
                new SqlParameter("@VATPercentage", aProductUnitPrice.VATPercentage),
                new SqlParameter("@VATAmountPerUnit", aProductUnitPrice.VATAmountPerUnit),
                new SqlParameter("@IsActive", aProductUnitPrice.IsActive),
                new SqlParameter("@ActiveDate", aProductUnitPrice.ActiveDate),
                new SqlParameter("@MRPPrice", aProductUnitPrice.MRPPrice)
            });
        }
        public bool SaveProductUnitPriceUpdate(ProductUnitPrice aProductUnitPrice)
        {
            string insertQuery = @"insert into tblUnitPriceUpdate (UnitPriceUpdateId,UnitPriceId,ProductId,ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,ActiveDate) 
            values (@UnitPriceUpdateId,@UnitPriceId,@ProductId,@ProductCode,@ProductName,@PackSize,@CostPrice,@UnitPrice,@VATPercentage,@VATAmountPerUnit,@ActiveDate)";
            return SInventorySql.Execute(insertQuery, new List<SqlParameter>
            {
                new SqlParameter("@UnitPriceUpdateId", aProductUnitPrice.UnitPriceUpdateId),
                new SqlParameter("@UnitPriceId", aProductUnitPrice.UnitPriceId),
                new SqlParameter("@ProductId", aProductUnitPrice.ProductId),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aProductUnitPrice.ProductCode)),
                new SqlParameter("@ProductName", SInventorySql.DbValue(aProductUnitPrice.ProductName)),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aProductUnitPrice.PackSize)),
                new SqlParameter("@CostPrice", aProductUnitPrice.CostPrice),
                new SqlParameter("@UnitPrice", aProductUnitPrice.UnitPrice),
                new SqlParameter("@VATPercentage", aProductUnitPrice.VATPercentage),
                new SqlParameter("@VATAmountPerUnit", aProductUnitPrice.VATAmountPerUnit),
                new SqlParameter("@ActiveDate", aProductUnitPrice.ActiveDate)
            });
        }

        public bool HasProductName(ProductUnitPrice aCategory)
        {
            string query = "select * from tblUnitPrice where ProductId = @ProductId";
            return SInventorySql.Exists(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductId", aCategory.ProductId)
            });
        }
        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        public DataTable LoadUnitPriceView()
        {
            string query = @"SELECT (UnitPrice+VATAmountPerUnit)TPVat ,(UnitPrice+VATAmountPerUnit)TPVat ,tblProduct.ProductCode,tblProduct.Description ProductName,UnitPrice,VATAmountPerUnit,MRPPrice,UnitPriceId from tblUnitPrice 
                LEFT JOIN dbo.tblProduct ON dbo.tblUnitPrice.ProductId = dbo.tblProduct.ProductId
                LEFT JOIN dbo.tblProductSQ ON dbo.tblProduct.ProductBrandId = dbo.tblProductSQ.ProductBrandId
                LEFT JOIN dbo.tblPackSize ON dbo.tblProduct.PackSizeId=dbo.tblPackSize.PackSizeId
                LEFT JOIN dbo.tblProType ON dbo.tblProType.ProTypeId=dbo.tblProduct.ProTypeId
                LEFT JOIN dbo.tblProCategory ON dbo.tblProduct.CategoryId = dbo.tblProCategory.CategoryId
                LEFT JOIN dbo.tblManufacturer ON dbo.tblProduct.ManufacId=dbo.tblManufacturer.ManufacId
                LEFT JOIN dbo.tblStockUOM ON dbo.tblProduct.StockUOMId=dbo.tblStockUOM.StockUOMId
                LEFT JOIN dbo.tblProductCase ON dbo.tblProductCase.CaseId=dbo.tblProduct.CaseId  where tblUnitPrice.IsActive='1'";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }
        public DataTable GetProductPriceReportInfo()
        {
            string query = @"SELECT [ProductCode],[ProductName],[PackSize],[CostPrice],[UnitPrice],[VATPercentage],[VATAmountPerUnit],[IsActive],[ActiveDate],[InActiveDate] from tblUnitPrice WITH(NOLOCK) ORDER BY IsActive DESC";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }



        /// /////////////////////////////////////////////////////////////////////////
        
        public ProductUnitPrice ProductUnitPriceEditLoad(string UnitPriceId)
        {
            string query = "select * from tblUnitPrice where UnitPriceId = @UnitPriceId";
            DataTable priceTable = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@UnitPriceId", SInventorySql.DbValue(UnitPriceId))
            });
            ProductUnitPrice aCategory = new ProductUnitPrice();
            if (priceTable.Rows.Count > 0)
            {
                DataRow priceRow = priceTable.Rows[0];
                int unitPriceId = 0, productId = 0;
                int.TryParse(priceRow["UnitPriceId"] != DBNull.Value ? priceRow["UnitPriceId"].ToString() : "0", out unitPriceId);
                int.TryParse(priceRow["ProductId"] != DBNull.Value ? priceRow["ProductId"].ToString() : "0", out productId);

                aCategory.UnitPriceId = unitPriceId;
                aCategory.ProductId = productId;
                aCategory.ProductCode = priceRow["ProductCode"] != DBNull.Value ? priceRow["ProductCode"].ToString() : "";
                aCategory.ProductName = priceRow["ProductName"] != DBNull.Value ? priceRow["ProductName"].ToString() : "";

                decimal vatAmount = 0, vatPer = 0, costPrice = 0, unitPrice = 0, mrpPrice = 0;
                decimal.TryParse(priceRow["VATAmountPerUnit"] != DBNull.Value ? priceRow["VATAmountPerUnit"].ToString() : "0", out vatAmount);
                decimal.TryParse(priceRow["VATPercentage"] != DBNull.Value ? priceRow["VATPercentage"].ToString() : "0", out vatPer);
                decimal.TryParse(priceRow["CostPrice"] != DBNull.Value ? priceRow["CostPrice"].ToString() : "0", out costPrice);
                decimal.TryParse(priceRow["UnitPrice"] != DBNull.Value ? priceRow["UnitPrice"].ToString() : "0", out unitPrice);
                decimal.TryParse(priceRow["MRPPrice"] != DBNull.Value ? priceRow["MRPPrice"].ToString() : "0", out mrpPrice);

                aCategory.VATAmountPerUnit = vatAmount;
                aCategory.VATPercentage = vatPer;
                aCategory.PackSize = priceRow["PackSize"] != DBNull.Value ? priceRow["PackSize"].ToString() : "";
                aCategory.CostPrice = costPrice;
                aCategory.UnitPrice = unitPrice;
                aCategory.MRPPrice = mrpPrice;

                DateTime activeDate = DateTime.Now;
                if (priceRow["ActiveDate"] != DBNull.Value)
                {
                    DateTime.TryParse(priceRow["ActiveDate"].ToString(), out activeDate);
                }
                aCategory.ActiveDate = activeDate;

                bool isActive = false;
                if (priceRow["IsActive"] != DBNull.Value)
                {
                    bool.TryParse(priceRow["IsActive"].ToString(), out isActive);
                }
                aCategory.IsActive = isActive;
            }
            return aCategory;
        }

        public ProductUnitPrice ProductUnitPriceEditLoadProduct(string productId)
        {
            string query = "select * from tblUnitPrice where ProductId = @ProductId";
            DataTable priceTable = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductId", SInventorySql.DbValue(productId))
            });
            ProductUnitPrice aCategory = new ProductUnitPrice();
            if (priceTable.Rows.Count > 0)
            {
                DataRow priceRow = priceTable.Rows[0];
                int unitPriceId = 0, pId = 0;
                int.TryParse(priceRow["UnitPriceId"] != DBNull.Value ? priceRow["UnitPriceId"].ToString() : "0", out unitPriceId);
                int.TryParse(priceRow["ProductId"] != DBNull.Value ? priceRow["ProductId"].ToString() : "0", out pId);

                aCategory.UnitPriceId = unitPriceId;
                aCategory.ProductId = pId;
                aCategory.ProductCode = priceRow["ProductCode"] != DBNull.Value ? priceRow["ProductCode"].ToString() : "";
                aCategory.ProductName = priceRow["ProductName"] != DBNull.Value ? priceRow["ProductName"].ToString() : "";

                decimal vatAmount = 0, vatPer = 0, costPrice = 0, unitPrice = 0, mrpPrice = 0;
                decimal.TryParse(priceRow["VATAmountPerUnit"] != DBNull.Value ? priceRow["VATAmountPerUnit"].ToString() : "0", out vatAmount);
                decimal.TryParse(priceRow["VATPercentage"] != DBNull.Value ? priceRow["VATPercentage"].ToString() : "0", out vatPer);
                decimal.TryParse(priceRow["CostPrice"] != DBNull.Value ? priceRow["CostPrice"].ToString() : "0", out costPrice);
                decimal.TryParse(priceRow["UnitPrice"] != DBNull.Value ? priceRow["UnitPrice"].ToString() : "0", out unitPrice);
                decimal.TryParse(priceRow["MRPPrice"] != DBNull.Value ? priceRow["MRPPrice"].ToString() : "0", out mrpPrice);

                aCategory.VATAmountPerUnit = vatAmount;
                aCategory.VATPercentage = vatPer;
                aCategory.PackSize = priceRow["PackSize"] != DBNull.Value ? priceRow["PackSize"].ToString() : "";
                aCategory.CostPrice = costPrice;
                aCategory.UnitPrice = unitPrice;
                aCategory.MRPPrice = mrpPrice;

                DateTime activeDate = DateTime.Now;
                if (priceRow["ActiveDate"] != DBNull.Value)
                {
                    DateTime.TryParse(priceRow["ActiveDate"].ToString(), out activeDate);
                }
                aCategory.ActiveDate = activeDate;

                bool isActive = false;
                if (priceRow["IsActive"] != DBNull.Value)
                {
                    bool.TryParse(priceRow["IsActive"].ToString(), out isActive);
                }
                aCategory.IsActive = isActive;
            }
            return aCategory;
        }
        
        public bool UpdateCustCategoryInfo(ProductUnitPrice aCategory)
        {
            string query = @"UPDATE tblUnitPrice SET ProductName=@ProductName,CostPrice=@CostPrice,UnitPrice=@UnitPrice,PackSize=@PackSize,ProductCode=@ProductCode,VATPercentage=@VATPercentage,VATAmountPerUnit=@VATAmountPerUnit WHERE UnitPriceId=@UnitPriceId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductName", SInventorySql.DbValue(aCategory.ProductName)),
                new SqlParameter("@CostPrice", aCategory.CostPrice),
                new SqlParameter("@UnitPrice", aCategory.UnitPrice),
                new SqlParameter("@PackSize", SInventorySql.DbValue(aCategory.PackSize)),
                new SqlParameter("@ProductCode", SInventorySql.DbValue(aCategory.ProductCode)),
                new SqlParameter("@VATPercentage", aCategory.VATPercentage),
                new SqlParameter("@VATAmountPerUnit", aCategory.VATAmountPerUnit),
                new SqlParameter("@UnitPriceId", aCategory.UnitPriceId)
            });
        }
        public bool UpdateActive(DateTime inactivedate,string unitpriceId)
        {
            string query = @"UPDATE tblUnitPrice SET IsActive=@IsActive,InActiveDate=@InActiveDate WHERE UnitPriceId=@UnitPriceId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@IsActive", false),
                new SqlParameter("@InActiveDate", inactivedate),
                new SqlParameter("@UnitPriceId", SInventorySql.DbValue(unitpriceId))
            });
        }
        // Inactive: copy the row to tblUnitPriceRollback, then delete it from tblUnitPrice (single transaction)
        public bool MoveUnitPriceToRollback(DateTime inactivedate, string unitpriceId, string loginUser)
        {
            string connStr = "Data Source=" + SqlUserAccess.DataSource +
                             ";Initial Catalog=" + DataBase.SalesDB +
                             ";Integrated Security=false;User ID=" + SqlUserAccess.UserName +
                             ";Password=" + SqlUserAccess.PassWord +
                             ";Encrypt=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    string insertQuery = @"INSERT INTO tblUnitPriceRollback
                        (UnitPriceId,ProductId,ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,MRPPrice,IsActive,ActiveDate,InActiveDate,RollbackBy,RollbackDate)
                        SELECT UnitPriceId,ProductId,ProductCode,ProductName,PackSize,CostPrice,UnitPrice,VATPercentage,VATAmountPerUnit,MRPPrice,0,ActiveDate,@InActiveDate,@RollbackBy,GETDATE()
                        FROM tblUnitPrice WHERE UnitPriceId=@UnitPriceId";
                    SqlCommand cmdInsert = new SqlCommand(insertQuery, conn, trans);
                    cmdInsert.Parameters.AddWithValue("@InActiveDate", inactivedate);
                    cmdInsert.Parameters.AddWithValue("@RollbackBy", SInventorySql.DbValue(loginUser));
                    cmdInsert.Parameters.AddWithValue("@UnitPriceId", SInventorySql.DbValue(unitpriceId));
                    if (cmdInsert.ExecuteNonQuery() == 0)
                    {
                        throw new Exception("Unit price not found!");
                    }

                    SqlCommand cmdDelete = new SqlCommand("DELETE FROM tblUnitPrice WHERE UnitPriceId=@UnitPriceId", conn, trans);
                    cmdDelete.Parameters.AddWithValue("@UnitPriceId", SInventorySql.DbValue(unitpriceId));
                    cmdDelete.ExecuteNonQuery();

                    trans.Commit();
                    return true;
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }
        public bool DeleteProduct(string productId)
        {
            string query = @"DELETE FROM dbo.tblUnitPrice WHERE ProductId=@ProductId";
            return SInventorySql.Execute(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductId", SInventorySql.DbValue(productId))
            });
        }
        public DataTable LoadProduct(string productId)
        {
            DataTable aDataTableEmpInfo = new DataTable();
            string query = @"SELECT * FROM tblProduct where ProductId=@ProductId ";
            aDataTableEmpInfo = SInventorySql.GetDataTable(query, new List<SqlParameter>
            {
                new SqlParameter("@ProductId", SInventorySql.DbValue(productId.Trim()))
            });
            return aDataTableEmpInfo;
        }

        public DataTable TotalUnitPriceReport()
        {
            string query = @"select * from tblUnitPrice";
            return aCommonInternalDal.DataContainerDataTable(query, "SSIDB");
        }

    }
}
