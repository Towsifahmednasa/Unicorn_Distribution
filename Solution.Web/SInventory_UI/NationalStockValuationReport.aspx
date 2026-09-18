<%@ Page Title="National Stock Valuation Report" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master"
    AutoEventWireup="true" CodeFile="NationalStockValuationReport.aspx.cs" Inherits="SInventory_UI_NationalStockValuationReport" %>

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
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> National Stock Valuation Report</div>
            </div>
            <!--end breadcrumb-->

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="exportButton" />
                                </Triggers>
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
                                        <label class="col-sm-2 col-form-label">Item Group:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="categoryDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Item Group"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label">From Date: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="fromDateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select From Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row mb-3">
                                        <label class="col-sm-2 col-form-label">Product:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="productDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Product"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label">To Date: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="toDateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select To Date"></asp:TextBox>
                                        </div>
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
                                                <h5 class="mb-0">National Stock Details</h5>
                                                <asp:LinkButton ID="exportButton" runat="server" OnClick="exportButton_Click" class="btn btn-sm btn-success" style="background-color: forestgreen; color: #fff; border: none;">
                                                    <i class="fa fa-file-excel-o"></i>&nbsp; Export to Excel
                                                </asp:LinkButton>
                                            </div>
                                            <div class="table-responsive">
                                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="True" 
                                                    CssClass="table table-striped table-bordered blueTable" ShowFooter="true" 
                                                    OnRowDataBound="loadGridView_RowDataBound" OnPreRender="loadGridView_PreRender">
                                                    <EmptyDataTemplate>
                                                        <div style="text-align:center; padding:20px; font-weight:bold; color:red;">
                                                            No Data Found!!
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Batch Details Modal Popup -->
                                    <div class="modal fade" id="batchModal" tabindex="-1" aria-labelledby="batchModalLabel" aria-hidden="true">
                                        <div class="modal-dialog modal-lg modal-dialog-scrollable">
                                            <div class="modal-content">
                                                <div class="modal-header" style="background-color: #00bcd4; color: #fff;">
                                                    <h5 class="modal-title" id="batchModalLabel">
                                                        <i class="fa fa-cubes"></i> Batch Wise Details
                                                    </h5>
                                                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="mb-3">
                                                        <strong>Product: </strong> <asp:Label ID="lblModalProduct" runat="server" CssClass="text-primary"></asp:Label><br />
                                                        <strong>Location: </strong> <asp:Label ID="lblModalLocation" runat="server" CssClass="text-success"></asp:Label>
                                                    </div>
                                                    <div class="table-responsive">
                                                        <asp:GridView ID="gvBatchDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered" ShowFooter="true" OnRowDataBound="gvBatchDetails_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="SL">
                                                                    <ItemTemplate>
                                                                        <%# Container.DataItemIndex + 1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Batch No" HeaderText="Batch No" />
                                                                <asp:BoundField DataField="MFG Date" HeaderText="MFG Date" />
                                                                <asp:BoundField DataField="Exp Date" HeaderText="Exp Date" />
                                                                <asp:BoundField DataField="Qty" HeaderText="Qty" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <div style="text-align:center; padding:10px; color:red; font-weight:bold;">
                                                                    No Batch details found for this stock quantity.
                                                                </div>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                                 <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">Close</button>
                                                 </div>
                                             </div>
                                         </div>
                                     </div>

                                     <!-- Hidden Controls for Popup Triggers -->
                                     <asp:HiddenField ID="hfProductCode" runat="server" />
                                     <asp:HiddenField ID="hfProductName" runat="server" />
                                     <asp:HiddenField ID="hfLocationName" runat="server" />
                                     <asp:LinkButton ID="btnHidden" runat="server" OnClick="btnHidden_Click" Style="display:none;" />

                                     <script type="text/javascript">
                                         function showBatchDetails(productCode, productName, locationName) {
                                             var hfProd = document.getElementById('<%= hfProductCode.ClientID %>');
                                             var hfName = document.getElementById('<%= hfProductName.ClientID %>');
                                             var hfLoc = document.getElementById('<%= hfLocationName.ClientID %>');
                                             if (hfProd && hfName && hfLoc) {
                                                 hfProd.value = productCode;
                                                 hfName.value = productName;
                                                 hfLoc.value = locationName;
                                                 __doPostBack('<%= btnHidden.UniqueID %>', '');
                                             }
                                         }
                                     </script>
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
