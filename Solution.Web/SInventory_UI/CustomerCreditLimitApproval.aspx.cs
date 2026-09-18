using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Library.DAL.SInventory_DAL;
using Library.DAO.SInventory_Entities;

public partial class SInventory_UI_CustomerCreditLimitApproval : System.Web.UI.Page
{
    private readonly CreditLimitDAL creditLimitDal = new CreditLimitDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CustomerCreditLimitInformation();
        }
    }

    protected void cancelButton_Click(object sender, EventArgs e)
    {
        CustomerCreditLimitInformation();
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

    private void CustomerCreditLimitInformation()
    {
        DataTable dataTable = creditLimitDal.LoadCustomerCreditLimitApproval(string.Empty);
        loadGridView.DataSource = dataTable;
        loadGridView.DataBind();
    }

    private void ShowMessageBox(string message)
    {
        string script = string.Format("alert('{0}');", message.Replace("'", "\\'"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
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

        CustomerCreditLimitInformation();
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
                CheckBox checkBox = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                if (checkBox != null && checkBox.Checked)
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
        if (statusRadioButtonList.SelectedItem.Text != "Reject")
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox checkBox = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                if (checkBox == null || !checkBox.Checked)
                {
                    continue;
                }

                CreditLimitDAO limitDao = new CreditLimitDAO
                {
                    CreditLimitId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                    ActionStatus = "Approved",
                    ApprovedBy = Convert.ToString(Session["LoginName"]),
                    ApprovedDate = DateTime.Today
                };

                creditLimitDal.ApprovalUpdateDAL(limitDao);
            }

            ShowMessageBox("Credit Limit Approved Successfully!!");
        }
        else
        {
            for (int i = 0; i < loadGridView.Rows.Count; i++)
            {
                CheckBox checkBox = (CheckBox)loadGridView.Rows[i].FindControl("chkSelect");

                if (checkBox == null || !checkBox.Checked)
                {
                    continue;
                }

                CreditLimitDAO limitDao = new CreditLimitDAO
                {
                    CreditLimitId = Convert.ToInt32(loadGridView.DataKeys[i][0].ToString()),
                    ActionStatus = "Reject",
                    ApprovedBy = Convert.ToString(Session["LoginName"]),
                    ApprovedDate = DateTime.Today
                };

                creditLimitDal.ApprovalUpdateDAL(limitDao);
            }

            ShowMessageBox("Credit Limit Rejected Successfully!!!");
        }
    }
}
