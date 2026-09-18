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
    public class OrderListBLL
    {
        private readonly OrderListDAL aOrderListDal = new OrderListDAL();

        public bool SaveOrderMaster(OrderInfoMaster aListMasterDao, out int maxorderId)
        {
            return aOrderListDal.SaveOrderMaster(aListMasterDao, out maxorderId);
        }

        public bool UpdateOrderMaster(OrderInfoMaster aListMasterDao)
        {
            return aOrderListDal.UpdateOrderMaster(aListMasterDao);
        }
        public string OrdNo()
        {
            string ordNo = string.Empty;

            ordNo = OrdnNoGenerator(aOrderListDal.OrderManualId());

            return ordNo;
        }
        public bool ApprovalUpdateDAL(OrderInfoMaster aOrderInfoMaster)
        {
            return aOrderListDal.ApprovalUpdateDAL(aOrderInfoMaster);
        }
        public DataTable GetSpecialApproveOrder(string actionstatus)
        {
            return aOrderListDal.GetSpecialApproveOrder(actionstatus);
        }
        public void LoadApprovalControlBLL(DropDownList ddl, string pageName, string userName)
        {
            aOrderListDal.LoadApprovalControlDAL(ddl, pageName, userName);
        }
        public void LoadApprovalControlBLL(RadioButtonList rdl, string pageName, string userName)
        {
            aOrderListDal.LoadApprovalControlDAL(rdl, pageName, userName);
        }
        public string LoadForApprovalConditionBLL(string pageName, string userName)
        {
            return aOrderListDal.LoadForApprovalConditionDAL(pageName, userName);
        }

        public void LoadSalesPerson(DropDownList ddl)
        {
            aOrderListDal.LoadSalesPerson(ddl);
        }

        public DataTable GetDCBySalesPerson(int empInfoId)
        {
            return aOrderListDal.GetDCBySalesPerson(empInfoId);
        }
        private string OrdnNoGenerator(int id)
        {
            string code = string.Empty;
            string Id = id.ToString();
            if (Id.Length == 1)
            {
                Id = "000000" + Id;
            }
            if (Id.Length == 2)
            {
                Id = "00000" + Id;
            }
            if (Id.Length == 3)
            {
                Id = "0000" + Id;
            }
            if (Id.Length == 4)
            {
                Id = "000" + Id;
            }
            if (Id.Length == 5)
            {
                Id = "00" + Id;
            }
            if (Id.Length == 6)
            {
                Id = "0" + Id;
            }
            code = "ORD-" + Id;
            return code;
        }
        public DataTable CustomerInfo(string custCode)
        {
            return aOrderListDal.CustomerInfo(custCode);
        }
        public string SaveOrderDetail(List<OrderInfoDetail> aOrderListDetailDaoList)
        {
            foreach (var aOrderListDetailDao in aOrderListDetailDaoList)
            {
                aOrderListDal.SaveOrderDetail(aOrderListDetailDao);
            }
            return "Data Saved Successfully";
        }
        public void LoadmanufacturerName(DropDownList ddl)
        {
            aOrderListDal.LoadmanufacturerName(ddl);
        }

        public void DCLoad(DropDownList aDownList)
        {
            aOrderListDal.DCLoad(aDownList);

        }
        public bool UpdateDataForCompanyInfo(OrderInfoMaster aOrderInfoMaster)
        {
            return aOrderListDal.UpdateCompanyInfo(aOrderInfoMaster);
        }

    }
}
