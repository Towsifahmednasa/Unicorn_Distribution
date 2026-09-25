using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;

// Admin/maintenance page: search by InvoiceNo, view + edit every related row
// (invoice, invoice detail, customer payment, payment detail, and the
// debit/credit voucher master+detail rows in CSTL_ACCDB_Unicorn) as raw grids.
public partial class SInventory_UI_InvoiceDataEditor : System.Web.UI.Page
{
    private const string AccDb = "CSTL_ACCDB_Unicorn";

    private class TableCfg
    {
        public string Db;    // null = default catalog (SalesDisDB_Unicorn_DB)
        public string Table;
        public string Where; // {0} = InvoiceId
        public string[] Hide;     // columns not shown (and not updated) in the grid
        public string[] Editable; // null = all editable; empty = read-only grid; else whitelist
    }

    private static readonly string[] AmountBDTOnly = new string[] { "DebitAmountBDT", "CreditAmountBDT" };
    private static readonly string[] PaymentAmountOnly = new string[] { "PaymentAmount" };
    private static readonly string[] VchMasterEditable = new string[] { "Narration", "TotalAmount", "TotalAmountBDT" };
    private static readonly string[] InvoiceEditable = new string[]
    {
        "TpTotal", "TpGrandTotal", "DeliveryTpTotal", "DeliveryTpGrandTotal", "PaymentAmount"
    };
    private static readonly string[] InvoiceDetailEditable = new string[]
    {
        "UnitPrice", "TotalPrice", "NetAmount", "DeliveryTotalPrice", "DeliveryNetAmount"
    };

    private static readonly string[] VchDetailHide = new string[]
    {
        "VoucherCategory", "CostCenterId", "RefInfoInd", "SubsidoryLedgerId", "Remarks", "LCChargesPaymentDetailsId",
        "DebitAmount", "CreditAmount"
    };

    private static readonly string[] VchMasterHide = new string[]
    {
        "IsCostCenter", "IsBuyerCostCenter", "IsOtherCostCenter", "ReceiveFromIsBuyer", "ByerId",
        "PaytoAndReceiveFromIsOther", "Other", "OtherAdress", "PaytoIsSupplier", "SupplierId", "IsInvoice",
        "BankCheckNo", "BankCheckDate", "CashRefNo", "CashRefDate", "JournalType", "Comments",
        "IsExportInvoice", "IsImportInvoice", "IsReconciliation", "ReconciliationDate", "ReconciliationComments",
        "isManualVCRNo", "ProformaId", "DcFreezeId",
        "CompanyInfoId", "FinancialYearId", "PostingDate", "VoucherNo", "VoucherCategoryID", "CurrencyId", "CurrencyCovRate",
        "Remarks", "EntryBy", "EntryDate", "UpdateBy", "UpdateDate", "VoucherType", "DestroyId", "ActionStatus",
        "SalesReturnId", "CustPayDetailId", "WHStockInMasterId", "PurchaseId", "SampleInvoiceId", "WholeSaleId",
        "IsAdjusted", "Note", "IsFormERP", "PurchaseOrderLocalReceiveId", "InvoiceId", "PaymentMasterId",
        "CommonPK_ID", "TableID", "SpotPurchaseId", "LandedCostActualDifference", "CommercialInvoiceActualDifference",
        "IsInventoryValuation",
        "BankAccountId", "ChequeNo", "ChequeDate", "CashAccountId", "RefNo", "RefDate", "ApprovedBy", "ApprovedDate",
        "ReconcileDate", "IsReconcile", "ToCompanyInfoId", "ToFinancialYearId"
    };

    private static readonly string[] CustPayHide = new string[]
    {
        "MarketId", "CustomerMasterId", "RefNo", "RefDate", "CreateBy", "CreateDate", "UpdateBy", "UpdateDate",
        "ComUnitId", "Remarks"
    };

    private static readonly string[] CustPayDetailHide = new string[]
    {
        "SubDeportInvoiceId", "Discount", "CashAccId", "BankAccId", "AIT", "IsPosting", "TransctionDetailId",
        "custPaymentDate", "TPAmount", "VATAmount", "fristRow", "SecondRow",
        "CustPayId", "42workingRow", "CollectionBy", "DANameId", "PreviousDANameId", "TestIDnew", "PreCollDate"
    };

    private static readonly string[] InvoiceHide = new string[]
    {
        "OrderId", "OrderNo", "OrderDate", "CustomerMasterId", "ComUnitId", "MiaId", "PaymentTypeId",
        "CreateBy", "CreateDate", "UpdateBy", "UpdateDate", "TotalSpecialAmount", "DelivarySpecialAmount",
        "ProductOffer", "OldTradePolicy", "Remarks", "FixedCustomer", "MIACode", "MIAName",
        "RejectionSts", "Inv_DANameId", "SndReturnInvoiceNo", "SndReturnPaymentBy", "SndReturnPaymentDate",
        "sndReturnTpTotal", "sndReturnTpDiscount", "sndReturnTpVat", "sndReturnTpGrandTotal", "sndReturnInvoiceStatus",
        "MarketCode", "MarketName", "AreaCode", "DisCode", "FEName", "RegionCode", "DZSMName",
        "DeliveryPersonName", "DeliveryPersonPhNo", "Types", "GreenStarBlueStarID", "AdjustAmount", "IsAdjustInvoice",
        "CampaignName", "OrderSenderType", "OrderSenderCode", "OrderSenderName", "CustomerType",
        "AdjustInvoiceNo_ReturnInvoiceNo", "DeliveryManId", "AIT", "DiscountOnPayment", "IsPosting",
        "MIOId", "IsAuto", "LoadingSummaryStatus",
        "IsSalesTransfer", "TransferInvoiceDate", "UpdateDatetime", "LoadingSummaryUpdateBy", "LoadingSummaryUpdateDate",
        "loadingsummaryFinalStatus", "loadingsummaryFinalStatusUpdateBy", "loadingsummaryFinalStatusUpdateDatetime",
        "PaymentTpTotal", "PaymentTpDiscount", "PaymentTpVat", "PaymentTpGrandTotal", "PaymentInvoiceStatus",
        "PaymentInvoiceNo", "PaymentBy", "PaymentDate", "FinalPaymentNo", "FinalPaymentBy", "FinalPaymentDate",
        "ReceivableAmount", "UpdateDateDate", "IS_Date"
    };

    private static readonly string[] InvoiceDetailHide = new string[]
    {
        "PackSize", "BatchNo", "ReceiveDate", "ExpDate",
        "ReturnReason", "Campaign", "ISGiftProduct", "CampaignType", "IsCampaignProduct", "AdjustmentAmount",
        "OrderDetailsId", "SpecialAmount", "DelivarySpecialAmount", "PaymentQuantity", "PaymentBonusQuantity",
        "PaymentTotalQuantity", "PaymentTotalPrice", "PaymentTotalPriceVatAmount", "PaymentDiscountPercentage",
        "PaymentDiscountAmount",
        "DeliveryDiscountAmount", "PaymentNetAmount", "PaymentReturnReason", "PaymentStatus", "PaymentTP"
    };

    private static readonly Dictionary<string, TableCfg> Cfg = new Dictionary<string, TableCfg>
    {
        { "gvInvoice",       new TableCfg { Table = "tblInvoice", Hide = InvoiceHide, Editable = InvoiceEditable, Where = "InvoiceId = {0}" } },
        { "gvInvoiceDetail", new TableCfg { Table = "tblInvoiceDetail", Hide = InvoiceDetailHide, Editable = InvoiceDetailEditable, Where = "InvoiceId = {0}" } },
        { "gvSalesVchM",     new TableCfg { Db = AccDb, Table = "tblDebitCreditVoucherMaster", Hide = VchMasterHide, Editable = VchMasterEditable,
            Where = "TableID = 2014 AND CommonPK_ID = {0} AND VoucherMasterId IN (SELECT VoucherMasterId FROM tblDebitCreditVoucherDetail WHERE ISNULL(Remarks, '') <> 'Cost of Goods Sold')" } },
        { "gvSalesVchD",     new TableCfg { Db = AccDb, Table = "tblDebitCreditVoucherDetail", Hide = VchDetailHide, Editable = AmountBDTOnly,
            Where = "VoucherMasterId IN (SELECT VoucherMasterId FROM tblDebitCreditVoucherMaster WHERE TableID = 2014 AND CommonPK_ID = {0}) AND ISNULL(Remarks, '') <> 'Cost of Goods Sold'" } },
        { "gvCustPay",       new TableCfg { Table = "tblCustomerPay", Hide = CustPayHide, Editable = PaymentAmountOnly,
            Where = "CustPayId IN (SELECT CustPayId FROM tblCustPayDetail WHERE InvoiceId = {0})" } },
        { "gvCustPayDetail", new TableCfg { Table = "tblCustPayDetail", Hide = CustPayDetailHide, Editable = PaymentAmountOnly, Where = "InvoiceId = {0}" } },
        { "gvPayVchM",       new TableCfg { Db = AccDb, Table = "tblDebitCreditVoucherMaster", Hide = VchMasterHide, Editable = VchMasterEditable,
            Where = "TableID = 2017 AND CommonPK_ID IN (SELECT CustPayId FROM SalesDisDB_Unicorn_DB..tblCustPayDetail WHERE InvoiceId = {0})" } },
        { "gvPayVchD",       new TableCfg { Db = AccDb, Table = "tblDebitCreditVoucherDetail", Hide = VchDetailHide, Editable = AmountBDTOnly,
            Where = "VoucherMasterId IN (SELECT VoucherMasterId FROM tblDebitCreditVoucherMaster WHERE TableID = 2017 AND CommonPK_ID IN (SELECT CustPayId FROM SalesDisDB_Unicorn_DB..tblCustPayDetail WHERE InvoiceId = {0}))" } },
    };

    private string ConnStr
    {
        get { return ConfigurationManager.ConnectionStrings["SolutionConnectionStringSSIDB"].ConnectionString; }
    }

    private GridView[] AllGrids()
    {
        return new GridView[] { gvInvoice, gvInvoiceDetail, gvSalesVchM, gvSalesVchD, gvCustPay, gvCustPayDetail, gvPayVchM, gvPayVchD };
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Grids are bound only on search / after edit events; ViewState carries them across postbacks.
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string no = txtInvoiceNo.Text.Trim();
        if (no.Length == 0) { ShowMsg("Enter an Invoice No.", true); return; }

        object found = null;
        using (SqlConnection conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 InvoiceId FROM tblInvoice WHERE InvoiceNo = @no", conn);
            cmd.Parameters.AddWithValue("@no", no);
            found = cmd.ExecuteScalar();

            int typedId;
            if (found == null && int.TryParse(no, out typedId))
            {
                cmd = new SqlCommand("SELECT TOP 1 InvoiceId FROM tblInvoice WHERE InvoiceId = @id", conn);
                cmd.Parameters.AddWithValue("@id", typedId);
                found = cmd.ExecuteScalar();
            }
        }

        if (found == null)
        {
            ViewState["InvId"] = null;
            foreach (GridView gv in AllGrids()) { gv.DataSource = null; gv.DataBind(); }
            ShowMsg("Invoice not found: " + no, true);
            return;
        }

        int invId = Convert.ToInt32(found);
        ViewState["InvId"] = invId;
        foreach (GridView gv in AllGrids()) gv.EditIndex = -1;
        ShowMsg("Loaded InvoiceId " + invId + ".", false);
        BindAll();
    }

    private void BindAll()
    {
        object o = ViewState["InvId"];
        if (o == null) return;
        int invId = (int)o;

        foreach (GridView gv in AllGrids())
        {
            TableCfg c = Cfg[gv.ID];
            using (SqlConnection conn = OpenConn(c))
            {
                string sql = "SELECT * FROM " + c.Table + " WHERE " + string.Format(c.Where, invId);
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.FillSchema(dt, SchemaType.Source);
                da.Fill(dt);
                StripHidden(dt, c);

                string[] pks;
                if (dt.PrimaryKey.Length > 0)
                {
                    pks = new string[dt.PrimaryKey.Length];
                    for (int i = 0; i < pks.Length; i++) pks[i] = dt.PrimaryKey[i].ColumnName;
                }
                else
                {
                    pks = new string[] { dt.Columns[0].ColumnName }; // ponytail: no-PK fallback assumes first column is the id
                }

                ViewState["pk_" + gv.ID] = string.Join(",", pks);
                gv.AutoGenerateEditButton = c.Editable == null || c.Editable.Length > 0;
                gv.DataKeyNames = pks;
                gv.DataSource = dt;
                gv.DataBind();
            }
        }
    }

    protected void Gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView gv = (GridView)sender;
        TableCfg c = Cfg[gv.ID];
        if (c.Editable == null) return;
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        if ((e.Row.RowState & DataControlRowState.Edit) == 0) return;

        DataRowView drv = (DataRowView)e.Row.DataItem;
        for (int i = 0; i < drv.Row.Table.Columns.Count; i++)
        {
            if (NameIn(c.Editable, drv.Row.Table.Columns[i].ColumnName)) continue;
            TableCell cell = e.Row.Cells[i + 1]; // cell 0 = command cell
            if (cell.Controls.Count == 0) continue;
            WebControl ctl = cell.Controls[0] as WebControl;
            if (ctl != null) ctl.Enabled = false;
        }
    }

    protected void Gv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ((GridView)sender).EditIndex = e.NewEditIndex;
        BindAll();
    }

    protected void Gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ((GridView)sender).EditIndex = -1;
        BindAll();
    }

    protected void Gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView gv = (GridView)sender;
        TableCfg c = Cfg[gv.ID];
        string[] pks = ((string)ViewState["pk_" + gv.ID]).Split(',');

        try
        {
            using (SqlConnection conn = OpenConn(c))
            {
                // Reload just this row, push edited cell values into it, let
                // SqlCommandBuilder generate the typed UPDATE.
                List<string> wh = new List<string>();
                SqlCommand sel = new SqlCommand();
                sel.Connection = conn;
                for (int i = 0; i < pks.Length; i++)
                {
                    wh.Add("[" + pks[i] + "] = @pk" + i);
                    sel.Parameters.AddWithValue("@pk" + i, gv.DataKeys[e.RowIndex].Values[pks[i]]);
                }
                sel.CommandText = "SELECT * FROM " + c.Table + " WHERE " + string.Join(" AND ", wh);

                SqlDataAdapter da = new SqlDataAdapter(sel);
                DataTable dt = new DataTable();
                da.FillSchema(dt, SchemaType.Source);
                da.Fill(dt);
                if (dt.Rows.Count == 0) { ShowMsg("Row no longer exists in " + c.Table + ".", true); return; }

                // Grid was bound without the hidden columns, so map its cells
                // against the visible columns only; the DataTable stays complete
                // for SqlCommandBuilder.
                List<DataColumn> visible = new List<DataColumn>();
                foreach (DataColumn dc in dt.Columns)
                    if (c.Hide == null || !NameIn(c.Hide, dc.ColumnName)) visible.Add(dc);

                DataRow r = dt.Rows[0];
                GridViewRow row = gv.Rows[e.RowIndex];
                List<DataColumn> changed = new List<DataColumn>();
                List<string> logParts = new List<string>();
                for (int i = 0; i < visible.Count; i++)
                {
                    DataColumn col = visible[i];
                    if (col.ReadOnly || col.AutoIncrement || NameIn(pks, col.ColumnName)) continue;
                    if (c.Editable != null && !NameIn(c.Editable, col.ColumnName)) continue;

                    TableCell cell = row.Cells[i + 1]; // cell 0 = Edit/Update command cell
                    if (cell.Controls.Count == 0) continue;
                    Control ctl = cell.Controls[0];

                    object oldVal = r[col];
                    if (ctl is CheckBox)
                    {
                        r[col] = ((CheckBox)ctl).Checked;
                    }
                    else if (ctl is TextBox)
                    {
                        string t = ((TextBox)ctl).Text.Trim();
                        if (t.Length == 0)
                        {
                            if (col.AllowDBNull) r[col] = DBNull.Value;
                            else if (col.DataType == typeof(string)) r[col] = "";
                            // non-nullable non-string left as-is when blank
                        }
                        else
                        {
                            r[col] = t; // DataColumn converts string -> column type
                        }
                    }

                    if (!ValEq(oldVal, r[col]))
                    {
                        changed.Add(col);
                        logParts.Add(col.ColumnName + ": '" + Fmt(oldVal) + "' -> '" + Fmt(r[col]) + "'");
                    }
                }

                if (changed.Count == 0)
                {
                    gv.EditIndex = -1;
                    ShowMsg("No changes in " + c.Table + ".", false);
                    BindAll();
                    return;
                }

                // Manual UPDATE: works even when the table has no primary key
                // (SqlCommandBuilder refuses those). WHERE uses the detected
                // key / first-column fallback from bind time.
                SqlCommand upd = new SqlCommand();
                upd.Connection = conn;
                List<string> sets = new List<string>();
                for (int i = 0; i < changed.Count; i++)
                {
                    sets.Add("[" + changed[i].ColumnName + "] = @v" + i);
                    upd.Parameters.AddWithValue("@v" + i, r[changed[i]]);
                }
                for (int i = 0; i < pks.Length; i++)
                    upd.Parameters.AddWithValue("@w" + i, gv.DataKeys[e.RowIndex].Values[pks[i]]);
                List<string> updWh = new List<string>();
                for (int i = 0; i < pks.Length; i++) updWh.Add("[" + pks[i] + "] = @w" + i);
                upd.CommandText = "UPDATE " + c.Table + " SET " + string.Join(", ", sets) +
                                  " WHERE " + string.Join(" AND ", updWh);
                upd.ExecuteNonQuery();

                List<string> keyParts = new List<string>();
                for (int i = 0; i < pks.Length; i++)
                    keyParts.Add(pks[i] + "=" + gv.DataKeys[e.RowIndex].Values[pks[i]]);
                LogChange(c.Table, string.Join(", ", keyParts), logParts);
            }

            gv.EditIndex = -1;
            ShowMsg("Row updated in " + c.Table + ".", false);
            BindAll();
        }
        catch (Exception ex)
        {
            // No rebind on failure so the typed values stay in the edit row.
            ShowMsg("Update failed (" + c.Table + "): " + ex.Message, true);
        }
    }

    private static void StripHidden(DataTable dt, TableCfg c)
    {
        if (c.Hide == null) return;
        for (int i = 0; i < c.Hide.Length; i++)
            if (dt.Columns.Contains(c.Hide[i])) dt.Columns.Remove(c.Hide[i]);
    }

    // Audit trail: every change made through this page is appended to
    // tblChangeManagement (sales DB) against the loaded InvoiceId.
    private void LogChange(string table, string keyDesc, List<string> parts)
    {
        string info = table + " [" + keyDesc + "] " + string.Join("; ", parts);
        string by = Session["LoginName"] == null ? "" : Session["LoginName"].ToString();
        using (SqlConnection conn = new SqlConnection(ConnStr))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(@"
IF OBJECT_ID('dbo.tblChangeManagement') IS NULL
CREATE TABLE dbo.tblChangeManagement (
    ChangeId INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceId INT NOT NULL,
    ChangeInformation NVARCHAR(MAX) NOT NULL,
    ChangeBy NVARCHAR(100) NULL,
    ChangeDate DATETIME NOT NULL DEFAULT GETDATE()
);
INSERT INTO dbo.tblChangeManagement (InvoiceId, ChangeInformation, ChangeBy) VALUES (@inv, @info, @by);", conn);
            cmd.Parameters.AddWithValue("@inv", (int)ViewState["InvId"]);
            cmd.Parameters.AddWithValue("@info", info);
            cmd.Parameters.AddWithValue("@by", by);
            cmd.ExecuteNonQuery();
        }
    }

    private static bool ValEq(object a, object b)
    {
        if (a == null || b == null) return a == b;
        return a.Equals(b);
    }

    private static string Fmt(object v)
    {
        if (v == null || v == DBNull.Value) return "NULL";
        return Convert.ToString(v);
    }

    private static bool NameIn(string[] names, string colName)
    {
        for (int i = 0; i < names.Length; i++)
            if (string.Equals(names[i], colName, StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }

    private SqlConnection OpenConn(TableCfg c)
    {
        SqlConnection conn = new SqlConnection(ConnStr);
        conn.Open();
        if (c.Db != null) conn.ChangeDatabase(c.Db); // keeps SqlCommandBuilder's unqualified UPDATE on the right DB
        return conn;
    }

    private void ShowMsg(string text, bool error)
    {
        lblMsg.Text = text;
        lblMsg.ForeColor = error ? Color.Red : Color.Green;
    }
}
