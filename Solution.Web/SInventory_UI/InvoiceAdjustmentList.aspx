<%@ Page Title="Invoice Adjustment List" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="InvoiceAdjustmentList.aspx.cs" Inherits="SInventory_UI_InvoiceAdjustmentList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .divWaiting { position: fixed; background: #FAFAFA; z-index: 2147483647 !important; opacity: .8; text-align: center; top: 0; left: 0; height: 100%; width: 100%; padding-top: 20%; }
        table[id$="GridView"] th, table[id$="GridView"] td { white-space: nowrap; font-size: 12px; padding: 4px 6px !important; }
        table[id$="GridView"] th { background: #2b3e50 !important; color: #fff !important; }
        .info-box span { display: inline-block; margin-right: 18px; }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            $('.datepicker').pickadate({ selectMonths: true, selectYears: true });
        }
        function closeDetailModal() {
            $('#detailModal').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                <ProgressTemplate>
                    <div class="divWaiting">
                        <asp:Image ID="imgWait" runat="server" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Invoice Adjustment List</div>
                        <div class="ms-auto">
                            <div class="btn-group">
                                <a href="InvoiceAdjustment.aspx" class="btn btn-sm btn-outline-info"><i class="fa fa-plus" aria-hidden="true"></i>&nbsp;New Entry</a>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">

                                    <div class="row align-items-end">
                                        <div class="col-md-3">
                                            <label class="col-form-label">From Date:</label>
                                            <asp:TextBox ID="fromDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="col-form-label">To Date:</label>
                                            <asp:TextBox ID="toDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Button ID="searchButton" runat="server" Text="Search" CssClass="btn btn-sm btn-primary px-4" OnClick="searchButton_Click" />
                                        </div>
                                        <div class="col-md-3 text-end">
                                            <span class="btn bg-info text-dark">Total: <asp:Label ID="totalLabel" runat="server" Text="0.00" /></span>
                                        </div>
                                    </div>

                                    <div class="table-responsive mt-3">
                                        <asp:GridView ID="listGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-sm text-center"
                                            DataKeyNames="BadDebtsId" OnRowCommand="listGridView_RowCommand" EmptyDataText="No adjustment found.">
                                            <Columns>
                                                <asp:BoundField DataField="BadDebtsId" HeaderText="ID" />
                                                <asp:BoundField DataField="AdjustmentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="AdjustmentType" HeaderText="Type" />
                                                <asp:BoundField DataField="ComUnitName" HeaderText="Unit" />
                                                <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                                <asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="Delivery Invoice" />
                                                <asp:BoundField DataField="TotalAdjustment" HeaderText="Adjustment" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="VoucherCode" HeaderText="Voucher" />
                                                <asp:BoundField DataField="EntryBy" HeaderText="Entry By" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderText="View">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="viewLinkButton" runat="server" CssClass="btn btn-sm btn-outline-info py-0" CommandName="ViewDetail"
                                                            CommandArgument='<%# Eval("BadDebtsId") %>'><i class="fa fa-eye"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Adjustment detail popup -->
            <div class="modal fade" id="detailModal" tabindex="-1" aria-labelledby="detailModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-xl modal-dialog-centered">
                    <div class="modal-content shadow-lg border-0">
                        <div class="modal-header bg-info py-2">
                            <h5 class="modal-title fs-6 mb-0" id="detailModalLabel"><i class="fa fa-list me-1"></i> Adjustment # <asp:Label ID="detailIdLabel" runat="server" /></h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" onclick="closeDetailModal();"></button>
                        </div>
                        <div class="modal-body">
                            <div class="alert alert-light border info-box py-2 mb-2">
                                <span><b>Invoice:</b> <asp:Label ID="detailInvoiceLabel" runat="server" /></span>
                                <span><b>Customer:</b> <asp:Label ID="detailCustomerLabel" runat="server" /></span>
                                <span><b>Type:</b> <asp:Label ID="detailTypeLabel" runat="server" /></span>
                                <span><b>Total:</b> <asp:Label ID="detailTotalLabel" runat="server" /></span>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="detailGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-sm text-center">
                                    <Columns>
                                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DeliveryNetAmount" HeaderText="Delivery Net Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="AdjustmentAmount" HeaderText="Adjustment Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="modal-footer py-2">
                            <button type="button" class="btn btn-sm btn-secondary" data-bs-dismiss="modal" onclick="closeDetailModal();">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
