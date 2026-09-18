<%@ page title="Batch Lifecycle & Traceability Statement" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_BatchLifecycleReport, App_Web_v1vsrjbt" %>

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
        
        /* Professional Excel Grid Styling */
        .excel-table {
            border-collapse: collapse !important;
            width: 100%;
            font-size: 12px;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #ffffff;
            border: 1px solid #bcbec0 !important;
        }
        
        .excel-table th {
            background-color: #2b3e50 !important;
            color: #ffffff !important;
            font-weight: 600;
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            padding: 8px 6px !important;
            border: 1px solid #1a252f !important;
            vertical-align: middle;
            text-align: center;
            white-space: nowrap;
        }
        
        .excel-table td {
            padding: 6px 8px !important;
            border: 1px solid #dcdfe3 !important;
            vertical-align: middle;
            color: #212529;
            white-space: nowrap;
        }
        
        .excel-table tr:nth-child(even) {
            background-color: #f8fafc;
        }
        
        .excel-table tr:hover {
            background-color: #eaf2fb !important;
        }
        
        /* Column Specific Alignments & Colors */
        .col-center {
            text-align: center !important;
        }
        .col-left {
            text-align: left !important;
        }
        .col-right {
            text-align: right !important;
            font-variant-numeric: tabular-nums;
        }
        
        .qty-receive {
            background-color: #f0f9f1 !important;
            color: #1b5e20 !important;
            font-weight: bold;
            text-align: right !important;
        }
        
        .qty-issue {
            background-color: #fdf2f2 !important;
            color: #b71c1c !important;
            font-weight: bold;
            text-align: right !important;
        }
        
        .qty-balance {
            background-color: #eff6ff !important;
            color: #0d47a1 !important;
            font-weight: bold;
            text-align: right !important;
        }

        .excel-footer td {
            background-color: #e2e8f0 !important;
            font-weight: bold !important;
            border-top: 2px solid #64748b !important;
            border-bottom: 3px double #1e293b !important;
            padding: 8px 6px !important;
        }
        
        /* Opening balance row */
        .row-opening {
            background-color: #f1f5f9 !important;
            font-weight: 600;
        }

        /* Clean Professional Flat Status Indicators */
        .status-dot {
            display: inline-block;
            width: 8px;
            height: 8px;
            border-radius: 50%;
            margin-right: 5px;
        }
        .status-opening { color: #334155; font-weight: 700; }
        .status-opening .status-dot { background-color: #475569; }

        .status-receive { color: #15803d; font-weight: 600; }
        .status-receive .status-dot { background-color: #16a34a; }
        
        .status-sales { color: #1d4ed8; font-weight: 600; }
        .status-sales .status-dot { background-color: #2563eb; }
        
        .status-return { color: #b45309; font-weight: 600; }
        .status-return .status-dot { background-color: #d97706; }
        
        .status-b2b { color: #7e22ce; font-weight: 600; }
        .status-b2b .status-dot { background-color: #9333ea; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-history"></i> Batch Lifecycle & Traceability Report</div>
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
                                        <label class="col-sm-2 col-form-label fw-bold">Distribution Center:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="dcDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Distribution Center" AutoPostBack="True" OnSelectedIndexChanged="dcDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label fw-bold">From Date:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="fromDateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select From Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row mb-2">
                                        <label class="col-sm-2 col-form-label fw-bold">Product: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="productDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Product" AutoPostBack="True" OnSelectedIndexChanged="productDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label fw-bold">To Date:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="toDateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select To Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group row mb-3">
                                        <label class="col-sm-2 col-form-label fw-bold">Batch No: <span style="color:red">*</span></label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="batchDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2" data-placeholder="Select Batch"></asp:DropDownList>
                                        </div>
                                        <label class="col-sm-2 col-form-label fw-bold">Transaction Type:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="txnTypeDropDownList" runat="server" class="form-select form-select-sm mb-3 mySelect2">
                                                <asp:ListItem Value="All" Text="All Movements" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="Receive from WH" Text="Receive from WH"></asp:ListItem>
                                                <asp:ListItem Value="Sales" Text="Sales"></asp:ListItem>
                                                <asp:ListItem Value="Sales Return" Text="Sales Return"></asp:ListItem>
                                                <asp:ListItem Value="B2B Out" Text="B2B Out"></asp:ListItem>
                                                <asp:ListItem Value="B2B In" Text="B2B In"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <!-- Buttons -->
                                    <div class="row mb-4">
                                        <div class="col-12 text-center">
                                            <asp:LinkButton ID="searchButton" runat="server" OnClick="searchButton_Click" class="btn btn-sm btn-primary shadow-sm" style="background-color: #0288d1; color: #fff; border: none; font-weight: 600; padding: 6px 20px;">
                                                <i class="fa fa-search"></i>&nbsp; Search
                                            </asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="resetButton" runat="server" OnClick="resetButton_Click" class="btn btn-sm btn-warning shadow-sm" style="background-color: #e65100; color: #fff; border: none; font-weight: 600; padding: 6px 20px;">
                                                <i class="fa fa-retweet"></i>&nbsp; Reset
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <!-- Report Data Grid -->
                                    <div class="row">
                                        <div class="col-12">
                                            <div class="d-flex justify-content-between align-items-center mb-2">
                                                <h6 class="mb-0 fw-bold text-secondary"><i class="fa fa-table"></i> Batch Movement & Traceability Ledger</h6>
                                                <asp:LinkButton ID="exportButton" runat="server" OnClick="exportButton_Click" class="btn btn-sm btn-success shadow-sm" style="background-color: #2e7d32; color: #fff; border: none; font-weight: 600;">
                                                    <i class="fa fa-file-excel-o"></i>&nbsp; Export to Excel
                                                </asp:LinkButton>
                                            </div>
                                            <div class="table-responsive" style="max-height: 600px; overflow-y: auto; border: 1px solid #bcbec0;">
                                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                                    CssClass="excel-table table-bordered" ShowFooter="true" 
                                                    OnRowDataBound="loadGridView_RowDataBound" OnPreRender="loadGridView_PreRender">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SL No">
                                                            <ItemTemplate>
                                                                <%# Eval("TransactionType").ToString().StartsWith("Opening Balance") ? "-" : (Container.DataItemIndex + 1).ToString() %>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="45px" CssClass="col-center" />
                                                            <ItemStyle CssClass="col-center" Width="45px" />
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField HeaderText="Txn ID">
                                                            <HeaderStyle Width="65px" CssClass="col-center" />
                                                            <ItemStyle CssClass="col-center fw-bold text-secondary" Width="65px" />
                                                            <ItemTemplate>
                                                                <%# Eval("TransactionID") == DBNull.Value || Eval("TransactionID") == null ? "-" : Eval("TransactionID") %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:BoundField DataField="TransactionDate" HeaderText="Posting Date & Time">
                                                            <HeaderStyle Width="145px" CssClass="col-center" />
                                                            <ItemStyle CssClass="col-center" Width="145px" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:TemplateField HeaderText="Transaction Type">
                                                            <HeaderStyle Width="160px" CssClass="col-left" />
                                                            <ItemStyle CssClass="col-left" Width="160px" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("TransactionType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="Reference" HeaderText="Reference Document">
                                                            <HeaderStyle Width="230px" CssClass="col-left" />
                                                            <ItemStyle CssClass="col-left" Width="230px" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField DataField="Source" HeaderText="Source / Origin">
                                                            <HeaderStyle Width="260px" CssClass="col-left" />
                                                            <ItemStyle CssClass="col-left" Width="260px" />
                                                        </asp:BoundField>
                                                        
                                                        <asp:BoundField DataField="Destination" HeaderText="Destination / Party Details">
                                                            <HeaderStyle CssClass="col-left" />
                                                            <ItemStyle CssClass="col-left" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="ReceiveQty" HeaderText="Receive (+)" DataFormatString="{0:N0}">
                                                            <HeaderStyle Width="90px" CssClass="col-right" BackColor="#1e3d59" />
                                                            <ItemStyle CssClass="qty-receive" Width="90px" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="IssueQty" HeaderText="Issue (-)" DataFormatString="{0:N0}">
                                                            <HeaderStyle Width="90px" CssClass="col-right" BackColor="#1e3d59" />
                                                            <ItemStyle CssClass="qty-issue" Width="90px" />
                                                        </asp:BoundField>

                                                        <asp:BoundField DataField="CumulativeBatchBalance" HeaderText="Batch Balance" DataFormatString="{0:N0}">
                                                            <HeaderStyle Width="110px" CssClass="col-right" BackColor="#1e3d59" />
                                                            <ItemStyle CssClass="qty-balance" Width="110px" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle CssClass="excel-footer" />
                                                    <EmptyDataTemplate>
                                                        <div style="text-align:center; padding:25px; font-weight:bold; color:#d32f2f; background-color:#fff;">
                                                            <i class="fa fa-info-circle"></i> No batch movement records found for the selected criteria.
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
