using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.BLL.SInventory_BLL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_ManualOrderApproval : System.Web.UI.Page
{
    private readonly OrderInfoBLL orderInfoBll = new OrderInfoBLL();
    private readonly OrderListBLL orderListBll = new OrderListBLL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ManualOrderInformation();
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        ManualOrderInformation();
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if ((gv.ShowHeader == true && gv.Rows.Count > 0)
            || (gv.ShowHeaderWhenEmpty == true))
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void dueGridView_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if ((gv.ShowHeader == true && gv.Rows.Count > 0)
            || (gv.ShowHeaderWhenEmpty == true))
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    private void ManualOrderInformation()
    {
        string fileName = Path.GetFileName(Request.Path);
        string userName = Convert.ToString(Session["LoginName"]);
        string actionStatus = orderListBll.LoadForApprovalConditionBLL(fileName, userName);

        orderListBll.LoadApprovalControlBLL(statusRadioButtonList, fileName, userName);

        DataTable dataTable = orderListBll.GetSpecialApproveOrder(actionStatus);
        loadGridView.DataSource = dataTable;
        loadGridView.DataBind();
    }

    private void ShowMessageBox(string message)
    {
        string sScript;
        message = message.Replace("'", "\\'");
        sScript = String.Format("alert('{0}');", message);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sScript, true);
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox headerCheckBox = (CheckBox)loadGridView.HeaderRow.FindControl("chkSelectAll");

        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox rowCheckBox = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");
            if (rowCheckBox != null)
            {
                rowCheckBox.Checked = headerCheckBox.Checked;
            }
        }
    }

    protected void btnSubmit0_Click(object sender, EventArgs e)
    {
        if (Validation())
        {
            SubmitApproval();
        }
        else
        {
            ShowMessageBox("Please select at least one row!!!");
        }

        ManualOrderInformation();
    }

    private bool Validation()
    {
        bool status = false;
        int count = 0;

        if (statusRadioButtonList.SelectedValue == "")
        {
            ShowMessageBox("Please select an operation!!");
        }
        else
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                if (cb != null && cb.Checked)
                {
                    count++;
                }

                if (count > 0)
                {
                    status = true;
                    break;
                }
            }
        }

        return status;
    }

    private void SubmitApproval()
    {
        for (int i = 0; i < loadGridView.Rows.Count; i++)
        {
            CheckBox cb = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

            if (cb == null || !cb.Checked)
            {
                continue;
            }

            OrderInfoMaster orderInfoMaster = new OrderInfoMaster
            {
                OrderId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                ActionStatus = statusRadioButtonList.SelectedItem.Text,
                ApprovedBy = Convert.ToString(Session["LoginName"]),
                ApprovedDate = DateTime.Today
            };

            orderListBll.ApprovalUpdateDAL(orderInfoMaster);
        }

        ShowMessageBox("Your " + statusRadioButtonList.SelectedItem.Text + " is Successfully Done !!");
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "PreviewDueHistory")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            string customerMasterId = loadGridView.DataKeys[rowIndex][1].ToString();

            LoadDueHistory(customerMasterId);
            ModalPopupExtender10.Show();
        }
    }

    private void LoadDueHistory(string customerMasterId)
    {
        DataTable dataTable = orderInfoBll.LoadCustomerDueHistory(customerMasterId);
        dueGridView.DataSource = dataTable;
        dueGridView.DataBind();
    }

    protected void Button41_OnClick(object sender, EventArgs e)
    {
        ModalPopupExtender10.Hide();
    }
}
