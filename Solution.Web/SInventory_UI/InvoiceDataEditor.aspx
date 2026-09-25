<%@ Page Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="InvoiceDataEditor.aspx.cs" Inherits="SInventory_UI_InvoiceDataEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .idegrid td, .idegrid th { white-space: nowrap; font-size: 12px; padding: 3px 6px; }
        .idegrid input[type=text] { min-width: 90px; font-size: 12px; }
        .gridwrap { overflow-x: auto; }
        .idegrid th { background: #f2f2f2; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Invoice Data Editor</div>
                    </div>

                    <div class="card mb-3">
                        <div class="card-body">
                            <div class="row g-2 align-items-center">
                                <div class="col-auto"><b>Invoice No / Invoice Id:</b></div>
                                <div class="col-auto">
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" Width="260px"></asp:TextBox>
                                </div>
                                <div class="col-auto">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                                </div>
                                <div class="col-auto">
                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card mb-3">
                        <div class="card-header"><b>Sales &mdash; tblInvoice</b></div>
                        <div class="card-body gridwrap">
                            <asp:GridView ID="gvInvoice" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                        </div>
                    </div>

                    <div class="card mb-3">
                        <div class="card-header"><b>Sales &mdash; tblInvoiceDetail</b></div>
                        <div class="card-body gridwrap">
                            <asp:GridView ID="gvInvoiceDetail" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                        </div>
                    </div>

                    <div class="card mb-3">
                        <div class="card-header"><b>Sales Voucher (TableID 2014) &mdash; tblDebitCreditVoucherMaster + Detail</b></div>
                        <div class="card-body gridwrap">
                            <asp:GridView ID="gvSalesVchM" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                            <asp:GridView ID="gvSalesVchD" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                        </div>
                    </div>

                    <div class="card mb-3">
                        <div class="card-header"><b>Payment &mdash; tblCustomerPay + tblCustPayDetail</b></div>
                        <div class="card-body gridwrap">
                            <asp:GridView ID="gvCustPay" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                            <asp:GridView ID="gvCustPayDetail" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                        </div>
                    </div>

                    <div class="card mb-3">
                        <div class="card-header"><b>Payment Voucher (TableID 2017) &mdash; tblDebitCreditVoucherMaster + Detail</b></div>
                        <div class="card-body gridwrap">
                            <asp:GridView ID="gvPayVchM" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                            <asp:GridView ID="gvPayVchD" runat="server" CssClass="table table-bordered table-striped idegrid" AutoGenerateEditButton="true" OnRowDataBound="Gv_RowDataBound" OnRowEditing="Gv_RowEditing" OnRowCancelingEdit="Gv_RowCancelingEdit" OnRowUpdating="Gv_RowUpdating" EmptyDataText="No rows found."></asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
