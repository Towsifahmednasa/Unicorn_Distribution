using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustomerCreditLimitEntry : Page
{
    private readonly CreditLimitDAL _creditLimitDal = new CreditLimitDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();

            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                creditHiddenField.Value = Request.QueryString["ID"];
                LoadCreditInfo(creditHiddenField.Value);
                updateButton.Visible = true;
                saveButton.Visible = false;
            }
            else
            {
                saveButton.Visible = true;
                updateButton.Visible = false;
                isActiveCheckBox.Checked = true;
            }
        }
    }

    private void LoadDropdowns()
    {
        _creditLimitDal.GetCompanyDropdownList(companyDropDownList);
    }

    private void LoadCreditInfo(string creditIdValue)
    {
        int creditId;

        if (!int.TryParse(creditIdValue, out creditId))
        {
            return;
        }

        DataTable table = _creditLimitDal.GetCustomerCreditInfoById(creditId);
        if (table.Rows.Count == 0)
        {
            return;
        }

        DataRow row = table.Rows[0];
        companyDropDownList.SelectedValue = row["CompanyId"].ToString();
        customerTextBox.Text = row["CustomerName"].ToString();
        customeridHiddenField.Value = row["CustomerMasterId"].ToString();
        limitAmntTextBox.Text = Convert.ToDecimal(row["LimitAmount"]).ToString(CultureInfo.InvariantCulture);
        limitDaysTextBox.Text = row["DayLimit"].ToString();
        isActiveCheckBox.Checked = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
    }

    private void ClearForm()
    {
        creditHiddenField.Value = string.Empty;
        customerTextBox.Text = string.Empty;
        customeridHiddenField.Value = string.Empty;
        companyDropDownList.SelectedIndex = 0;
        limitAmntTextBox.Text = string.Empty;
        limitDaysTextBox.Text = string.Empty;
        isActiveCheckBox.Checked = true;
        saveButton.Visible = true;
        updateButton.Visible = false;
    }

    private bool ValidateForm()
    {
        if (companyDropDownList.SelectedIndex <= 0)
        {
            ShowFail("Please select a company.");
            companyDropDownList.Focus();
            return false;
        }

        if (string.IsNullOrEmpty(customeridHiddenField.Value))
        {
            ShowFail("Please select a valid customer.");
            customerTextBox.Focus();
            return false;
        }

        decimal limitAmount;
        if (!decimal.TryParse(limitAmntTextBox.Text.Trim(), out limitAmount) || limitAmount <= 0)
        {
            ShowFail("Please input a valid credit limit amount.");
            limitAmntTextBox.Focus();
            return false;
        }

        int dayLimit;
        if (!int.TryParse(limitDaysTextBox.Text.Trim(), out dayLimit) || dayLimit <= 0)
        {
            ShowFail("Please input a valid day limit.");
            limitDaysTextBox.Focus();
            return false;
        }

        return true;
    }

    private CreditLimitDAO BuildModel(bool isUpdate)
    {
        var model = new CreditLimitDAO
        {
            CompanyId = Convert.ToInt32(companyDropDownList.SelectedValue),
            CustomerMasterId = Convert.ToInt32(customeridHiddenField.Value),
            LimitAmount = Convert.ToDecimal(limitAmntTextBox.Text.Trim()),
            DayLimit = Convert.ToInt32(limitDaysTextBox.Text.Trim()),
            IsActive = isActiveCheckBox.Checked,
            ActionStatus = "Posted"
        };

        if (isUpdate)
        {
            model.CreditLimitId = Convert.ToInt32(creditHiddenField.Value);
            model.UpdateBy = Session["LoginName"] == null ? string.Empty : Session["LoginName"].ToString();
            model.UpdateDate = DateTime.Now;
        }
        else
        {
            model.EntryBy = Session["LoginName"] == null ? string.Empty : Session["LoginName"].ToString();
            model.EntryDate = DateTime.Now;
        }

        return model;
    }

    private bool IsDuplicate(bool isUpdate)
    {
        int companyId = Convert.ToInt32(companyDropDownList.SelectedValue);
        int customerId = Convert.ToInt32(customeridHiddenField.Value);

        if (isUpdate)
        {
            int creditLimitId = Convert.ToInt32(creditHiddenField.Value);
            using (DataTable dt = _creditLimitDal.CheckExistorNotUpdate(companyId, customerId, creditLimitId))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
            }
        }
        else
        {
            using (DataTable dt = _creditLimitDal.CheckExistorNot(companyId, customerId))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void ShowFail(string message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "faildalert('" + message.Replace("'", "\\'") + "','Faild');", true);
    }

    protected void saveButton_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
        {
            return;
        }

        if (IsDuplicate(false))
        {
            ShowFail("Data already exists.");
            return;
        }

        if (_creditLimitDal.SaveCreditLimit(BuildModel(false)))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Operation successful!','Success','CustomerCreditView.aspx');", true);
        }
        else
        {
            ShowFail("Operation failed.");
        }
    }

    protected void updateButton_Click(object sender, EventArgs e)
    {
        if (!ValidateForm())
        {
            return;
        }

        if (IsDuplicate(true))
        {
            ShowFail("Data already exists.");
            return;
        }

        if (_creditLimitDal.UpdateCreditLimit(BuildModel(true)))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert('Operation successful!','Success','CustomerCreditView.aspx');", true);
        }
        else
        {
            ShowFail("Operation failed.");
        }
    }

    protected void customerTextBox_TextChanged(object sender, EventArgs e)
    {
        if (customerTextBox.Text.Contains(":"))
        {
            string[] parts = customerTextBox.Text.Split(':');
            if (parts.Length >= 2)
            {
                customerTextBox.Text = parts[0].Trim();
                customeridHiddenField.Value = parts[1].Trim();
                return;
            }
        }

        customeridHiddenField.Value = string.Empty;
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
        {
            Response.Redirect("CustomerCreditLimitEntry.aspx");
            return;
        }

        ClearForm();
    }

    protected void viewLinkButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("CustomerCreditView.aspx");
    }
}
