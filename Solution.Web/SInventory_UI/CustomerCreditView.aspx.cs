using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;

public partial class SInventory_UI_CustomerCreditView : Page
{
    private readonly CreditLimitDAL _creditLimitDal = new CreditLimitDAL();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();
            LoadGrid();
        }
    }

    private void LoadDropdowns()
    {
        _creditLimitDal.GetCompanyDropdownList(companyDropDownList);
    }

    private void LoadGrid()
    {
        //DataTable table = _creditLimitDal.GetCustomerCreditInfo(
        //    companyDropDownList.SelectedValue,
        //    customerIdHiddenField.Value);


        DataTable table = _creditLimitDal.GetCustomerCreditInfo("");

        loadGridView.DataSource = table;
        loadGridView.DataBind();
    }

    private void ShowFail(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + message.Replace("'", "\\'") + "','Faild');", true);
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerCreditView.aspx");
    }

    protected void newEntryLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerCreditLimitEntry.aspx");
    }

    protected void customerTextBox_TextChanged(object sender, EventArgs e)
    {
        if (customerTextBox.Text.Contains(":"))
        {
            string[] parts = customerTextBox.Text.Split(':');
            if (parts.Length > 3)
            {
                customerTextBox.Text = parts[1].Trim();
                customerIdHiddenField.Value = parts[3].Trim();
                return;
            }
        }

        customerTextBox.Text = string.Empty;
        customerIdHiddenField.Value = string.Empty;
    }

    protected void loadGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int rowIndex;
        if (!int.TryParse(e.CommandArgument.ToString(), out rowIndex))
        {
            return;
        }

        string creditLimitId = loadGridView.DataKeys[rowIndex]["CreditLimitId"].ToString();

        if (e.CommandName == "EditData")
        {
            Response.Redirect("CustomerCreditLimitEntry.aspx?ID=" + creditLimitId);
            return;
        }

        if (e.CommandName == "DeleteData")
        {
            bool deleted = _creditLimitDal.DeleteCreditLimit(Convert.ToInt32(creditLimitId));

            if (deleted)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "ShowSuccesalert('Operation successful!','Success');", true);
                LoadGrid();
            }
            else
            {
                ShowFail("Operation failed.");
            }
        }
    }

    protected void gv_DocumentUpload_PreRender(object sender, EventArgs e)
    {
        GridView gv = (GridView)sender;

        if ((gv.ShowHeader && gv.Rows.Count > 0) || gv.ShowHeaderWhenEmpty)
        {
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}
