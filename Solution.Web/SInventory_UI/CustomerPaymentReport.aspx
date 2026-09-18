<%@ Page Title="Customer Payment Report" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master"
    AutoEventWireup="true" CodeFile="CustomerPaymentReport.aspx.cs" Inherits="SInventory_UI_CustomerPaymentReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- FontAwesome & DataTables Styles -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../VerticalAsset/MyDt/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="../VerticalAsset/MyDt/buttons.dataTables.min.css" rel="stylesheet" />

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

        /* Filter Section Styling */
        .filter-card {
            background-color: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 6px;
            padding: 16px 20px;
            margin-bottom: 20px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.05);
        }
        .filter-label {
            font-weight: 600;
            font-size: 13.5px;
            color: #334155;
            margin-bottom: 4px;
            display: block;
        }
        .form-control-custom {
            height: 36px;
            font-size: 13px;
            border-radius: 4px;
            border: 1px solid #cbd5e1;
            padding: 6px 12px;
            color: #1e293b;
            background-color: #fff;
            width: 100%;
        }
        .form-control-custom:focus {
            border-color: #3b82f6;
            box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.15);
        }

        /* Buttons */
        .btn-action-search {
            background-color: #0ea5e9;
            color: #ffffff !important;
            border: none;
            font-size: 13.5px;
            font-weight: 600;
            padding: 7px 24px;
            border-radius: 4px;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: all 0.2s;
            text-decoration: none;
            cursor: pointer;
        }
        .btn-action-search:hover {
            background-color: #0284c7;
            box-shadow: 0 2px 4px rgba(14, 165, 233, 0.3);
        }
        .btn-action-reset {
            background-color: #f97316;
            color: #ffffff !important;
            border: none;
            font-size: 13.5px;
            font-weight: 600;
            padding: 7px 24px;
            border-radius: 4px;
            display: inline-flex;
            align-items: center;
            gap: 6px;
            transition: all 0.2s;
            text-decoration: none;
            cursor: pointer;
        }
        .btn-action-reset:hover {
            background-color: #ea580c;
            box-shadow: 0 2px 4px rgba(249, 115, 22, 0.3);
        }
        .btn-export-excel {
            background-color: #10b981;
            color: #ffffff !important;
            border: none;
            font-size: 12.5px;
            font-weight: 600;
            padding: 5px 14px;
            border-radius: 4px;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            text-decoration: none;
            cursor: pointer;
        }
        .btn-export-excel:hover {
            background-color: #059669;
        }
        .btn-export-pdf {
            background-color: #ef4444;
            color: #ffffff !important;
            border: none;
            font-size: 12.5px;
            font-weight: 600;
            padding: 5px 14px;
            border-radius: 4px;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            text-decoration: none;
            cursor: pointer;
        }
        .btn-export-pdf:hover {
            background-color: #dc2626;
        }

        /* DataTable Custom Styles */
        .table-custom {
            width: 100% !important;
            border-collapse: separate !important;
            border-spacing: 0 !important;
            border: 1px solid #e2e8f0 !important;
            border-radius: 6px;
            font-size: 13px;
            color: #1e293b;
        }
        .table-custom thead th {
            background-color: #f1f5f9 !important;
            color: #0f172a !important;
            font-weight: 700 !important;
            font-size: 12.5px !important;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            padding: 10px 12px !important;
            border-bottom: 2px solid #cbd5e1 !important;
            border-right: 1px solid #e2e8f0 !important;
            vertical-align: middle;
            white-space: nowrap;
        }
        .table-custom tbody td {
            padding: 8px 12px !important;
            border-bottom: 1px solid #f1f5f9 !important;
            border-right: 1px solid #f1f5f9 !important;
            vertical-align: middle;
        }
        .table-custom tbody tr:hover {
            background-color: #eff6ff !important;
        }
        .table-custom tfoot td {
            background-color: #f8fafc !important;
            font-weight: 700 !important;
            font-size: 13px !important;
            padding: 10px 12px !important;
            border-top: 2px solid #cbd5e1 !important;
            color: #0f172a !important;
        }

        /* Custom Badge / Pill */
        .badge-voucher {
            background-color: #e0f2fe;
            color: #0369a1;
            font-weight: 600;
            padding: 3px 8px;
            border-radius: 4px;
            font-family: Consolas, monospace;
            font-size: 12px;
            border: 1px solid #bae6fd;
            display: inline-block;
        }
        .badge-mode {
            background-color: #f1f5f9;
            color: #475569;
            font-weight: 600;
            padding: 2px 7px;
            border-radius: 3px;
            font-size: 11.5px;
            border: 1px solid #e2e8f0;
            display: inline-block;
        }

        /* Single Preview Button */
        .btn-preview-receipt {
            font-size: 12px;
            font-weight: 600;
            padding: 4px 10px;
            border-radius: 4px;
            border: 1px solid #2563eb;
            background-color: #eff6ff;
            color: #1d4ed8;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            cursor: pointer;
            transition: all 0.15s ease-in-out;
        }
        .btn-preview-receipt:hover {
            background-color: #2563eb;
            color: #ffffff;
            box-shadow: 0 2px 4px rgba(37, 99, 235, 0.25);
        }

        /* DataTables Controls styling */
        .dataTables_wrapper .dataTables_filter input {
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            padding: 4px 8px;
            margin-left: 6px;
        }
        .dataTables_wrapper .dataTables_length select {
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            padding: 3px 6px;
        }
    </style>

    <script type="text/javascript">
        function openReceiptPreview(receiptNo) {
            var url = 'MoneyReceiptPreview.aspx?ReceiptNo=' + encodeURIComponent(receiptNo) + '&CopyType=Client%20Copy';
            var win = window.open(url, '_blank', 'width=880,height=750,scrollbars=yes,resizable=yes');
            if (win) {
                win.focus();
            }
            return false;
        }

        function openPdfReport() {
            var fromDate = $('#<%= fromDateTextBox.ClientID %>').val();
            var toDate = $('#<%= toDateTextBox.ClientID %>').val();
            var url = 'CustomerPaymentReportPDF.aspx?FromDate=' + encodeURIComponent(fromDate) + '&ToDate=' + encodeURIComponent(toDate);
            window.open(url, '_blank');
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content" style="padding-top: 10px;">
            
            <!-- Breadcrumb / Header Title -->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3" style="font-size: 18px; font-weight: 700; color: #0f172a;">
                    <i class="fa fa-file-text-o text-success me-2"></i> Customer Payment & Contra Voucher Report
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card border-0 shadow-sm">
                        <div class="card-body p-4">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="divWaiting">
                                                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="160px" Height="160px" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <script type="text/javascript">
                                        function initUIPlugins() {
                                            $('.datepicker').pickadate({
                                                selectMonths: true,
                                                selectYears: true,
                                                format: 'dd-mmm-yyyy'
                                            });

                                            // Initialize jQuery DataTable
                                            if ($.fn.DataTable) {
                                                var tableId = '#<%= loadGridView.ClientID %>';
                                                if ($.fn.DataTable.isDataTable(tableId)) {
                                                    $(tableId).DataTable().destroy();
                                                }
                                                if ($(tableId + ' tbody tr').length > 0 && !$(tableId + ' tbody tr td').hasClass('dataTables_empty')) {
                                                    $(tableId).DataTable({
                                                        responsive: true,
                                                        paging: true,
                                                        pageLength: 25,
                                                        lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                                                        order: [[0, 'asc']],
                                                        language: {
                                                            search: "_INPUT_",
                                                            searchPlaceholder: "Search records..."
                                                        }
                                                    });
                                                }
                                            }
                                        }

                                        function pageLoad() {
                                            initUIPlugins();
                                        }
                                    </script>

                                    <!-- Filter Panel: Only From Date and To Date -->
                                    <div class="filter-card">
                                        <div class="row g-3 justify-content-center align-items-end">
                                            <div class="col-md-4">
                                                <label class="filter-label">From Date <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="fromDateTextBox" runat="server" class="form-control form-control-custom datepicker" autocomplete="off" placeholder="From Date"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <label class="filter-label">To Date <span class="text-danger">*</span></label>
                                                <asp:TextBox ID="toDateTextBox" runat="server" class="form-control form-control-custom datepicker" autocomplete="off" placeholder="To Date"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4 d-flex gap-2">
                                                <asp:LinkButton ID="searchButton" runat="server" OnClick="searchButton_Click" class="btn-action-search">
                                                    <i class="fa fa-search"></i> Search
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="resetButton" runat="server" OnClick="resetButton_Click" class="btn-action-reset">
                                                    <i class="fa fa-refresh"></i> Reset
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Report Grid Section -->
                                    <div class="d-flex justify-content-between align-items-center mb-2">
                                        <div>
                                            <h6 class="mb-0 fw-bold text-dark">
                                                <i class="fa fa-list-alt text-primary me-1"></i> Payment Transaction Records
                                            </h6>
                                        </div>
                                        <div class="d-flex gap-2">
                                            <asp:LinkButton ID="exportButton" runat="server" OnClick="exportButton_Click" class="btn-export-excel" ToolTip="Export to Excel">
                                                <i class="fa fa-file-excel-o"></i> Export to Excel
                                            </asp:LinkButton>
                                            <a href="javascript:void(0);" onclick="openPdfReport();" class="btn-export-pdf" title="Open PDF in New Tab">
                                                <i class="fa fa-file-pdf-o"></i> Export to PDF
                                            </a>
                                        </div>
                                    </div>

                                    <!-- Table with DataTable format -->
                                    <div class="table-responsive">
                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                            CssClass="table-custom display" ShowFooter="true" GridLines="None"
                                            OnRowDataBound="loadGridView_RowDataBound" OnPreRender="loadGridView_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" HeaderStyle-Width="45px" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Voucher No" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <span class="badge-voucher"><%# Eval("VoucherNo") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="PaymentDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" 
                                                    HeaderStyle-Width="105px" ItemStyle-HorizontalAlign="Center" />

                                                <asp:BoundField DataField="BankCash" HeaderText="Account Head" HeaderStyle-Width="130px" />

                                                <asp:TemplateField HeaderText="Payment Mode" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <span class="badge-mode"><%# Eval("ChequeNo") %></span>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Narration" HeaderText="Narration / Particulars" />

                                                <asp:BoundField DataField="Amount" HeaderText="Amount (Tk)" DataFormatString="{0:N2}" 
                                                    HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" 
                                                    FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true" />

                                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" class="btn-preview-receipt" title="Print/Preview Money Receipt"
                                                           onclick="openReceiptPreview('<%# Eval("VoucherNo") %>');">
                                                            <i class="fa fa-print"></i> Preview
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="UserName" HeaderText="Created By" HeaderStyle-Width="100px" 
                                                    ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <div class="text-center py-4 text-danger fw-bold">
                                                    <i class="fa fa-info-circle me-1"></i> No Payment Transactions Found for the Selected Date Range.
                                                </div>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
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
