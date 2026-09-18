<%@ Page Title="DC Stock Valuation Report" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master"
    AutoEventWireup="true" CodeFile="DCStockValuationReport.aspx.cs" Inherits="SInventory_UI_DCStockValuationReport" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .divWaiting {
            position: fixed;
            background-color: #FAFAFA;
            z-index: 2147483647 !important;
            opacity: 0.8;
            overflow: hidden;
            text-align: center;
            top: 0;
            left: 0;
            height: 100%;
            width: 100%;
            padding-top: 20%;
        }
        .position-set {
            margin: 0 auto;
        }
        .text-right {
            text-align: right;
        }
        .footer-total {
            font-weight: bold;
            background-color: #f1f1f1;
            text-align: right;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> DC Stock Valuation Report</div>
            </div>
            <!--end breadcrumb-->

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="divWaiting">
                                                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

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
                                                selectYears: true,
                                                format: 'dd mmmm, yyyy'
                                            });
                                        }
                                    </script>

                                    <!-- Filters -->
                                    <div class="form-group row mb-2">
                                        <label class="col-sm-2 col-form-label">Distribution Center:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="dcDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Distribution Center"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label">From Date: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="fromDateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select From Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row mb-2">
                                        <label class="col-sm-2 col-form-label">Item Group:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="categoryDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Item Group"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label">To Date: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="toDateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select To Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row mb-3">
                                        <label class="col-sm-2 col-form-label">Product:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="productDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Product"></asp:DropDownList>
                                        </div>
                                        <div class="col-sm-6">&nbsp;</div>
                                    </div>

                                    <!-- Buttons -->
                                    <div class="row mb-4">
                                        <div class="col-12 text-center">
                                            <asp:LinkButton ID="searchButton" runat="server" OnClick="searchButton_Click" class="btn btn-sm btn-primary" style="background-color: #00bcd4; color: #fff; border: none;">
                                                <i class="fa fa-search"></i>&nbsp; Search
                                            </asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="resetButton" runat="server" OnClick="resetButton_Click" class="btn btn-sm btn-warning" style="background-color: orangered; color: #fff; border: none;">
                                                <i class="fa fa-retweet"></i>&nbsp; Reset
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <!-- Report Data Grid -->
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex justify-content-between align-items-center mb-3">
                                                <h5 class="mb-0">Item Wise Stock</h5>
                                                <asp:LinkButton ID="exportButton" runat="server" OnClick="exportButton_Click" class="btn btn-sm btn-success" style="background-color: forestgreen; color: #fff; border: none;">
                                                    <i class="fa fa-file-excel-o"></i>&nbsp; Export to Excel
                                                </asp:LinkButton>
                                            </div>
                                             <div class="table-responsive">
                                                 <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                                     CssClass="table table-striped table-bordered blueTable" ShowFooter="true" 
                                                     OnRowDataBound="loadGridView_RowDataBound" OnPreRender="loadGridView_PreRender">
                                                     <Columns>
                                                         <asp:TemplateField HeaderText="SL No">
                                                             <ItemTemplate>
                                                                 <%# Container.DataItemIndex + 1 %>
                                                             </ItemTemplate>
                                                             <HeaderStyle Width="50px" />
                                                             <ItemStyle HorizontalAlign="Center" />
                                                         </asp:TemplateField>
                                                         <asp:BoundField DataField="DC Code" HeaderText="DC Code" />
                                                         <asp:BoundField DataField="Distribution Center" HeaderText="Distribution Center" />
                                                         <asp:BoundField DataField="Category Name" HeaderText="Category Name" />
                                                         <asp:BoundField DataField="Product Code" HeaderText="Product Code" />
                                                         <asp:BoundField DataField="Product Name" HeaderText="Product Name" />
                                                         <asp:BoundField DataField="Pack Size" HeaderText="Pack Size" />
                                                         
                                                         <asp:BoundField DataField="Opening Qty" HeaderText="Opening Qty" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#e0f7fa" ForeColor="#006064" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Receive From WH" HeaderText="[Receive] From WH" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#e8f5e9" ForeColor="#1b5e20" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="B2B Receive" HeaderText="[Receive] B2B Transfer" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#e8f5e9" ForeColor="#1b5e20" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Return Rcv" HeaderText="[Receive] Sales Return" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#e8f5e9" ForeColor="#1b5e20" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Sales" HeaderText="[Issue] Sales" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#ffebee" ForeColor="#b71c1c" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="B2B Issue" HeaderText="[Issue] B2B Transfer" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#ffebee" ForeColor="#b71c1c" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Closing Qty" HeaderText="Closing Qty" DataFormatString="{0:N2}">
                                                             <ItemStyle HorizontalAlign="Right" />
                                                             <HeaderStyle BackColor="#e3f2fd" ForeColor="#0d47a1" />
                                                             <FooterStyle HorizontalAlign="Right" CssClass="footer-total" />
                                                         </asp:BoundField>
                                                     </Columns>
                                                    <EmptyDataTemplate>
                                                        <div style="text-align:center; padding:20px; font-weight:bold; color:red;">
                                                            No Data Found!!
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="exportButton" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
