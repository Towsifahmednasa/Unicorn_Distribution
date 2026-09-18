<%@ page title="Pending Order List" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_AreaView, App_Web_yureiwkh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3">
                    <i class="bx bx-list-check"></i> Pending Order List
                </div>
                <div class="ms-auto d-flex gap-2">
                    <asp:Button ID="excelButton1" runat="server" Text="Export to Excel" CssClass="btn btn-sm btn-success px-3 shadow-sm text-white fw-bold" OnClick="btnExportToExcel_Click" />
                    <a href="ManualOrderCreation.aspx" class="btn btn-sm btn-primary px-3 shadow-sm text-white fw-bold">
                        <i class="bx bx-plus-circle me-1"></i> + New Manual Order
                    </a>
                </div>
            </div>
            <!--end breadcrumb-->

            <div class="row">
                <div class="col-12">
                    <div class="card border-top border-0 border-4 border-primary shadow-sm">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive" id="MainGradeDiv">
                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered table-hover w-100" DataKeyNames="OrderId"
                                            OnRowCommand="loadGridView_RowCommand" OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>
                                                <asp:BoundField DataField="OrderCode" HeaderText="Order Code" />
                                                <asp:BoundField DataField="ComUnitName" HeaderText="Sales Center / DC" />
                                                <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                <asp:BoundField DataField="GrossValue" HeaderText="Gross Value (TP)" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="SubmissionDate" HeaderText="Submission Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" NullDisplayText="-" />
                                                <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                                                    <ItemTemplate>
                                                        <div class="d-flex justify-content-center gap-1">
                                                            <asp:LinkButton ID="btnPreview" runat="server" CssClass="btn btn-sm btn-outline-primary px-2 py-1 shadow-sm"
                                                                CommandName="PreviewOrder" CommandArgument='<%# Eval("OrderId") %>' ToolTip="View Order Preview">
                                                                <i class="bx bx-show me-1"></i> Preview
                                                            </asp:LinkButton>
                                                            <a href='ManualOrderCreation.aspx?OrderId=<%# Eval("OrderId") %>' class="btn btn-sm btn-outline-warning px-2 py-1 shadow-sm text-dark" title="Edit Order">
                                                                <i class="bx bx-edit me-1"></i> Edit
                                                            </a>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <!-- Order Preview Modal -->
                                    <div class="modal fade" id="orderPreviewModal" tabindex="-1" aria-labelledby="orderPreviewModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
                                            <div class="modal-content border-0 shadow-lg">
                                                <div class="modal-header bg-primary text-white py-2">
                                                    <h6 class="modal-title fw-bold" id="orderPreviewModalLabel">
                                                        <i class="bx bx-receipt me-1"></i> Order Details Preview
                                                    </h6>
                                                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                                </div>
                                                <div class="modal-body p-4">
                                                    <!-- Order Header Details -->
                                                    <div class="row g-3 p-3 bg-light rounded border mb-3">
                                                        <div class="col-md-3">
                                                            <small class="text-muted d-block fw-bold">Order Code</small>
                                                            <asp:Label ID="lblModalOrderCode" runat="server" CssClass="fw-bold text-primary fs-6"></asp:Label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <small class="text-muted d-block fw-bold">Order Date</small>
                                                            <asp:Label ID="lblModalOrderDate" runat="server" CssClass="fw-bold text-dark"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <small class="text-muted d-block fw-bold">Distribution Center</small>
                                                            <asp:Label ID="lblModalDC" runat="server" CssClass="fw-bold text-dark"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <small class="text-muted d-block fw-bold">Customer</small>
                                                            <asp:Label ID="lblModalCustomer" runat="server" CssClass="fw-bold text-dark"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <small class="text-muted d-block fw-bold">Sales Person (MIO)</small>
                                                            <asp:Label ID="lblModalSalesPerson" runat="server" CssClass="fw-bold text-dark"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <small class="text-muted d-block fw-bold">Customer Address</small>
                                                            <asp:Label ID="lblModalAddress" runat="server" CssClass="text-dark"></asp:Label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <small class="text-muted d-block fw-bold">Contact / Cell No</small>
                                                            <asp:Label ID="lblModalCellNo" runat="server" CssClass="text-dark"></asp:Label>
                                                        </div>
                                                        <div class="col-12" id="pnlModalRemarks" runat="server">
                                                            <small class="text-muted d-block fw-bold">Remarks / Special Note</small>
                                                            <asp:Label ID="lblModalRemarks" runat="server" CssClass="text-dark fst-italic"></asp:Label>
                                                        </div>
                                                    </div>

                                                    <!-- Products Table -->
                                                    <div class="table-responsive border rounded mb-3">
                                                        <asp:GridView ID="gvModalItems" runat="server" AutoGenerateColumns="False"
                                                            CssClass="table table-bordered table-striped table-hover mb-0">
                                                            <HeaderStyle CssClass="table-dark" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" ItemStyle-Width="130px" />
                                                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                                                <asp:BoundField DataField="PackSize" HeaderText="Pack Size" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="Quantity" HeaderText="Qty" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:N0}" />
                                                                <asp:BoundField DataField="TradePrice" HeaderText="Unit TP (Tk)" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                                <asp:BoundField DataField="TotalTradePrice" HeaderText="Total TP (Tk)" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                                <asp:BoundField DataField="UnitVatAmount" HeaderText="Unit VAT" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                                <asp:BoundField DataField="TotalVatAmount" HeaderText="Total VAT" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount (Tk)" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div class="text-center py-3 text-muted">No items found for this order.</div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>

                                                    <!-- Order Totals Summary Card -->
                                                    <div class="row g-3 text-center">
                                                        <div class="col-md-3">
                                                            <div class="p-2 border rounded bg-light">
                                                                <small class="text-muted fw-bold d-block">TOTAL QUANTITY</small>
                                                                <asp:Label ID="lblModalTotalQty" runat="server" CssClass="fs-6 fw-bold text-dark">0</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="p-2 border rounded bg-light">
                                                                <small class="text-muted fw-bold d-block">TOTAL TP (GROSS)</small>
                                                                <asp:Label ID="lblModalTotalTP" runat="server" CssClass="fs-6 fw-bold text-primary">Tk 0.00</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="p-2 border rounded bg-light">
                                                                <small class="text-muted fw-bold d-block">TOTAL VAT</small>
                                                                <asp:Label ID="lblModalTotalVAT" runat="server" CssClass="fs-6 fw-bold text-warning">Tk 0.00</asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="p-2 border rounded bg-light border-success">
                                                                <small class="text-success fw-bold d-block">NET PAYABLE</small>
                                                                <asp:Label ID="lblModalNetPayable" runat="server" CssClass="fs-6 fw-bold text-success">Tk 0.00</asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer bg-light py-2">
                                                    <button type="button" class="btn btn-sm btn-secondary px-3" data-bs-dismiss="modal">
                                                        <i class="bx bx-x me-1"></i> Close
                                                    </button>
                                                    <asp:HyperLink ID="lnkModalEdit" runat="server" CssClass="btn btn-sm btn-warning text-dark px-3 fw-bold">
                                                        <i class="bx bx-edit me-1"></i> Edit Order
                                                    </asp:HyperLink>
                                                    <button type="button" class="btn btn-sm btn-outline-primary px-3" onclick="window.print();">
                                                        <i class="bx bx-printer me-1"></i> Print
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="excelButton1" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            initializeDataTable();

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        initializeDataTable();
                    }
                });
            }
        });

        function initializeDataTable() {
            var gridId = '#<%= loadGridView.ClientID %>';
            if ($.fn.DataTable.isDataTable(gridId)) {
                $(gridId).DataTable().destroy();
            }
            $(gridId).DataTable({
                "bInfo": true,
                "bFilter": true,
                "responsive": true,
                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                pageLength: 10,
                dom: 'lBfrtip',
                buttons: ['copy', 'excel', 'pdf', 'print']
            });
        }
    </script>

</asp:Content>

