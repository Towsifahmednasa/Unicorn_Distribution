<%@ page title="Customer Invoice Aging & Outstanding Report" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CustomerInvoiceAgingReport, App_Web_jn5nt2is" %>

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
        .text-left {
            text-align: left !important;
        }
        .text-center {
            text-align: center !important;
        }
        .text-right {
            text-align: right !important;
        }

        /* Filter Form Styling */
        .filter-label {
            font-weight: 600;
            font-size: 13px;
            color: #334155;
        }

        /* Report Grid Styling */
        .custom-report-table {
            border-collapse: collapse !important;
            width: 100% !important;
            font-size: 13px;
            background-color: #ffffff;
            border: 1px solid #c5d0dc !important;
            margin-bottom: 0 !important;
        }
        .custom-report-table thead th {
            background: #23527c !important;
            background: linear-gradient(180deg, #2b669a 0%, #1e466b 100%) !important;
            color: #ffffff !important;
            font-weight: 600 !important;
            font-size: 13px !important;
            padding: 9px 8px !important;
            border: 1px solid #163654 !important;
            vertical-align: middle !important;
            text-shadow: 0 1px 1px rgba(0,0,0,0.25);
            white-space: nowrap;
        }
        .custom-report-table tbody tr {
            transition: background-color 0.15s ease;
        }
        .custom-report-table tbody tr:nth-child(even) {
            background-color: #f8fafc;
        }
        .custom-report-table tbody tr:hover {
            background-color: #e0f2fe !important;
        }
        .custom-report-table tbody td {
            padding: 7px 8px !important;
            border: 1px solid #e2e8f0 !important;
            vertical-align: middle !important;
            color: #1e293b;
            font-size: 12.5px;
        }
        .custom-report-table tfoot tr {
            background-color: #f1f5f9 !important;
            border-top: 2px solid #1e466b !important;
        }
        .custom-report-table tfoot td {
            padding: 9px 8px !important;
            font-size: 13px !important;
            font-weight: bold !important;
            color: #0f172a !important;
            border: 1px solid #cbd5e1 !important;
            vertical-align: middle !important;
        }

        /* Column Specific Styling */
        .cell-sl {
            font-weight: 500;
            color: #64748b;
        }
        .cell-cust-name {
            font-weight: 600;
            color: #0f172a;
        }
        .cell-invoice-no {
            font-weight: 500;
            color: #1d4ed8;
        }
        .cell-credit-days {
            font-weight: bold;
            color: #0369a1;
        }
        .cell-outstanding {
            font-weight: bold;
            color: #dc2626 !important;
        }

        /* DataTables Controls styling */
        .dataTables_wrapper {
            font-size: 13px;
            margin-top: 5px;
        }
        .dataTables_wrapper .dataTables_length {
            margin-bottom: 12px;
        }
        .dataTables_wrapper .dataTables_length label {
            font-weight: 500;
            color: #475569;
            display: inline-flex;
            align-items: center;
        }
        .dataTables_wrapper .dataTables_length select {
            margin: 0 6px;
            padding: 4px 10px;
            font-size: 13px;
            border-radius: 4px;
            border: 1px solid #cbd5e1;
            background-color: #fff;
            width: auto;
            display: inline-block;
        }
        .dataTables_wrapper .dataTables_filter {
            margin-bottom: 12px;
            text-align: right;
        }
        .dataTables_wrapper .dataTables_filter label {
            font-weight: 500;
            color: #475569;
        }
        .dataTables_wrapper .dataTables_filter input {
            margin-left: 8px;
            padding: 5px 12px;
            font-size: 13px;
            border-radius: 4px;
            border: 1px solid #cbd5e1;
            width: 220px;
            display: inline-block;
            outline: none;
            transition: border-color 0.2s, box-shadow 0.2s;
        }
        .dataTables_wrapper .dataTables_filter input:focus {
            border-color: #2b669a;
            box-shadow: 0 0 0 2px rgba(43, 102, 154, 0.2);
        }
        .dataTables_wrapper .dataTables_info {
            padding-top: 12px;
            font-size: 12.5px;
            color: #64748b;
        }
        .dataTables_wrapper .dataTables_paginate {
            padding-top: 10px;
            text-align: right;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-receipt"></i> Customer Invoice Aging & Outstanding Report</div>
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
                                        function initDataTable() {
                                            var $table = $('#<%= loadGridView.ClientID %>');
                                            if ($table.find('tbody tr').length > 0 && !$table.find('tbody tr td').hasClass('dataTables_empty') && $table.find('tbody tr td').length > 1) {
                                                if ($.fn.DataTable.isDataTable($table)) {
                                                    $table.DataTable().destroy();
                                                }
                                                $table.DataTable({
                                                    "bInfo": true,
                                                    "bFilter": true,
                                                    "bAutoWidth": false,
                                                    "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                                                    "pageLength": 25,
                                                    "order": [],
                                                    "dom": '<"row mb-2 align-items-center"<"col-sm-6"l><"col-sm-6 text-end"f>>rt<"row mt-3 align-items-center"<"col-sm-6"i><"col-sm-6 text-end"p>>',
                                                    "language": {
                                                        "search": "Search:",
                                                        "emptyTable": "No data available"
                                                    }
                                                });
                                            }
                                        }

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
                                            initDataTable();
                                        }
                                    </script>

                                    <!-- Filters Section with Clean Alignment -->
                                    <div class="row mb-3 align-items-center">
                                        <label class="col-sm-2 col-form-label filter-label">Distribution Center (DC):</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="comUnitDropDownList" runat="server" class="form-select form-select-sm mySelect2" data-placeholder="Select DC" AutoPostBack="true" OnSelectedIndexChanged="comUnitDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label filter-label">As On Date: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="asOnDateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select As On Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mb-3 align-items-center">
                                        <label class="col-sm-2 col-form-label filter-label">Customer / Party:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="customerDropDownList" runat="server" class="form-select form-select-sm mySelect2" data-placeholder="Select Customer"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label filter-label">Invoice From Date:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="fromDateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select From Date (Optional)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mb-3 align-items-center">
                                        <label class="col-sm-2 col-form-label filter-label">Sales Person / MIO:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="salesPersonDropDownList" runat="server" class="form-select form-select-sm mySelect2" data-placeholder="Select Sales Person"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label filter-label">Invoice To Date:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="toDateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select To Date (Optional)"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- Action Buttons -->
                                    <div class="row mb-4 mt-2">
                                        <div class="col-12 text-center">
                                            <asp:LinkButton ID="searchButton" runat="server" OnClick="searchButton_Click" class="btn btn-sm btn-primary px-4 py-1" style="background-color: #00bcd4; color: #fff; border: none; font-weight: 500;">
                                                <i class="fa fa-search"></i>&nbsp; Search
                                            </asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="resetButton" runat="server" OnClick="resetButton_Click" class="btn btn-sm btn-warning px-4 py-1" style="background-color: orangered; color: #fff; border: none; font-weight: 500;">
                                                <i class="fa fa-refresh"></i>&nbsp; Reset
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <!-- Report Data Grid Section -->
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex justify-content-between align-items-center mb-3 pb-2 border-bottom">
                                                <h5 class="mb-0 font-weight-bold" style="color: #1e466b; font-size: 16px;">
                                                    <i class="bx bx-list-check"></i> Customer Outstanding Details
                                                </h5>
                                                <asp:LinkButton ID="exportButton" runat="server" OnClick="exportButton_Click" class="btn btn-sm btn-success px-3 py-1" style="background-color: #2e7d32; color: #fff; border: none; font-weight: 500;">
                                                    <i class="fa fa-file-excel-o"></i>&nbsp; Export to Excel
                                                </asp:LinkButton>
                                            </div>

                                            <div class="table-responsive">
                                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                                    CssClass="table table-bordered custom-report-table" ShowFooter="true" 
                                                    OnRowDataBound="loadGridView_RowDataBound" OnPreRender="loadGridView_PreRender">
                                                    <Columns>
                                                        <asp:BoundField DataField="SL" HeaderText="SL" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center cell-sl" ItemStyle-Width="40px" />
                                                        <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left cell-cust-name" />
                                                        <asp:BoundField DataField="Invoice No" HeaderText="Invoice No" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center cell-invoice-no" ItemStyle-Width="110px" />
                                                        <asp:BoundField DataField="Invoice Date" HeaderText="Invoice Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" ItemStyle-Width="100px" />
                                                        <asp:BoundField DataField="Invoice Amount" HeaderText="Invoice Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" ItemStyle-CssClass="text-right font-weight-bold" ItemStyle-Width="120px" />
                                                        <asp:BoundField DataField="Paid Amount" HeaderText="Paid Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" ItemStyle-CssClass="text-right" ItemStyle-Width="110px" />
                                                        <asp:BoundField DataField="Credit Days" HeaderText="Credit Days" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center cell-credit-days" ItemStyle-Width="85px" />
                                                        <asp:BoundField DataField="Outstanding" HeaderText="Outstanding" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right" ItemStyle-CssClass="text-right cell-outstanding" ItemStyle-Width="125px" />
                                                        <asp:BoundField DataField="Responsible Person" HeaderText="Responsible Person" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left" ItemStyle-CssClass="text-left" ItemStyle-Width="180px" />
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <div style="text-align:center; padding:25px; font-weight:bold; color:#dc2626; font-size:14px;">
                                                            <i class="fa fa-info-circle"></i> No Data Found!
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
