<%@ page title="" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_SalesCollectionReport, App_Web_ua4dmbbd" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style type="text/css">
         .ssss {
             font-size: 13px;
             font-weight: bold;
         }
         .card-highlight {
             border-radius: 8px;
             box-shadow: 0 4px 6px rgba(0,0,0,0.1);
             padding: 15px;
             margin-bottom: 20px;
         }
     </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Sales Collection Report</div>
            </div>
            <!--end breadcrumb-->

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <script type="text/javascript">
                                function pageLoad() {
                                    $('.mySelect2').select2({
                                        theme: 'bootstrap4',
                                        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                        placeholder: $(this).data('placeholder'),
                                        allowClear: Boolean($(this).data('allow-clear')),
                                    });
                                    $('.datepicker').pickadate({
                                        selectMonths: true,
                                        selectYears: true
                                    });
                                }
                            </script>

                            <!-- SEARCH & FILTER CONTROL PANEL -->
                            <div class="row mb-3">
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Date From:</label>
                                    <asp:TextBox ID="txtDateFrom" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Start Date"></asp:TextBox>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Date To:</label>
                                    <asp:TextBox ID="txtDateTo" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select End Date"></asp:TextBox>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Delivery Agent (DA):</label>
                                    <asp:DropDownList ID="ddlDA" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Customer:</label>
                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Sales Person:</label>
                                    <asp:DropDownList ID="ddlSalesPerson" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Payment Channel:</label>
                                    <asp:DropDownList ID="ddlPaymentChannel" runat="server" CssClass="form-select form-select-sm mySelect2">
                                        <asp:ListItem Text="ALL" Value="" />
                                        <asp:ListItem Text="Bank" Value="Bank" />
                                        <asp:ListItem Text="bKash" Value="bKash" />
                                        <asp:ListItem Text="Hand Cash" Value="Hand Cash" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Region:</label>
                                    <asp:DropDownList ID="ddlRegion" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                </div>
                                <div class="col-md-3 mb-2">
                                    <label class="form-label">Product Name:</label>
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-12 text-center">
                                    <asp:LinkButton OnClick="SearchButton_Click" runat="server" id="submitButton" class="btn btnMyDesignSearch btn-sm" style="margin-right: 8px;">
                                        <i class="fa fa-search" aria-hidden="true"></i>&nbsp; Search
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="cancelButton" OnClick="cancelButton_Click" class="btn btnMyDesignReset btn-sm">
                                        <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <hr class="my-4" />

                            <!-- DETAILS LIST HEADER -->
                            <div class="row align-items-center mb-3">
                                <div class="col-md-6">
                                    <h3 class="mb-0">Details List</h3>
                                </div>
                                <div class="col-md-6 text-end">
                                    <asp:LinkButton ID="btnExport" class="btn btn-sm text-white" style="background-color: #1A7343; border-color: #1A7343;" runat="server" OnClick="btnExport_Click">
                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <!-- GRIDVIEW -->
                            <div class="table-responsive" id="MainGradeDiv">
                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-striped table-bordered" AllowPaging="True" PageSize="50" PageIndex="0" 
                                    OnPageIndexChanging="loadGridView_PageIndexChanging" OnPreRender="gv_PreRender" ShowFooter="True">
                                    <Columns>
                                        <asp:BoundField DataField="Month" HeaderText="Month" />
                                        <asp:BoundField DataField="Date" HeaderText="Date" />
                                        <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                        <asp:BoundField DataField="Narration" HeaderText="Narration" />
                                        <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" />
                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                        <asp:BoundField DataField="CustomerAddress" HeaderText="Customer Address" />
                                        <asp:BoundField DataField="Region" HeaderText="Region" />
                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                        
                                        <asp:BoundField DataField="Quantity" HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N0}" />
                                        <asp:BoundField DataField="ProductRate" HeaderText="Product Rate" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                        <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                        
                                        <asp:BoundField DataField="BankAmount" HeaderText="Bank Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                        <asp:BoundField DataField="bKashAmount" HeaderText="bKash Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                        <asp:BoundField DataField="HandCash" HeaderText="Hand Cash" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" />
                                        
                                        <asp:BoundField DataField="DeliveryAgent" HeaderText="Delivery Agent (DA)" />
                                        <asp:BoundField DataField="SalesPerson" HeaderText="Sales Person" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />
                                </asp:GridView>
                            </div>

                            <!-- SUMMARY HIGHLIGHTS (AUTOCALCULATED) -->
                            <div class="card mt-4 border-top border-0 border-4 border-primary">
                                <div class="card-body">
                                    <h5 class="card-title text-primary"><i class="fa fa-calculator"></i> SUMMARY HIGHLIGHTS (AUTOCALCULATED)</h5>
                                    <hr />
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="card bg-light text-center card-highlight">
                                                <div class="text-secondary font-weight-bold">Total Bank Payment</div>
                                                <h4 class="text-primary mt-2">TK <asp:Label ID="lblTotalBank" runat="server" Text="0.00"></asp:Label></h4>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="card bg-light text-center card-highlight">
                                                <div class="text-secondary font-weight-bold">Total bKash Payment</div>
                                                <h4 class="text-success mt-2">TK <asp:Label ID="lblTotalbKash" runat="server" Text="0.00"></asp:Label></h4>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="card bg-light text-center card-highlight">
                                                <div class="text-secondary font-weight-bold">Total Hand Cash</div>
                                                <h4 class="text-warning mt-2">TK <asp:Label ID="lblTotalHandCash" runat="server" Text="0.00"></asp:Label></h4>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="card bg-light text-center card-highlight">
                                                <div class="text-secondary font-weight-bold">Grand Total Amount</div>
                                                <h4 class="text-info mt-2">TK <asp:Label ID="lblGrandTotal" runat="server" Text="0.00"></asp:Label></h4>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
