<%@ Page Title="Stock Transfer Report" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="StockTransferReport.aspx.cs" Inherits="SInventory_UI_StockTransferReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="page-wrapper">
        <div class="page-content">

            <%-- Breadcrumb --%>
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-transfer"></i> Stock Transfer Report</div>
            </div>
            <%-- End Breadcrumb --%>

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-primary">
                        <div class="card-body">

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                    <%-- Search / Filter Panel --%>
                                    <div class="row mb-3 align-items-end g-3">

                                        <div class="col-md-2">
                                            <label class="form-label fw-bold">From Date:</label>
                                            <asp:TextBox ID="fromDateTextBox" runat="server"
                                                CssClass="form-control form-control-sm datepicker"
                                                placeholder="dd-mmm-yyyy"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <label class="form-label fw-bold">To Date:</label>
                                            <asp:TextBox ID="toDateTextBox" runat="server"
                                                CssClass="form-control form-control-sm datepicker"
                                                placeholder="dd-mmm-yyyy"></asp:TextBox>
                                        </div>

                                        <div class="col-md-3">
                                            <label class="form-label fw-bold">From Location:</label>
                                            <asp:DropDownList ID="fromLocationDropDownList" runat="server"
                                                CssClass="form-select form-select-sm mySelect2">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-3">
                                            <label class="form-label fw-bold">To Location:</label>
                                            <asp:DropDownList ID="toLocationDropDownList" runat="server"
                                                CssClass="form-select form-select-sm mySelect2">
                                            </asp:DropDownList>
                                        </div>

                                        <div class="col-md-2 d-flex gap-2">
                                            <asp:Button ID="searchButton" runat="server" Text="Search"
                                                CssClass="btn btn-sm btn-primary flex-grow-1"
                                                OnClick="searchButton_Click" />
                                            <asp:Button ID="resetButton" runat="server" Text="Reset"
                                                CssClass="btn btn-sm btn-secondary flex-grow-1"
                                                OnClick="resetButton_Click" />
                                        </div>

                                    </div>
                                    <hr />

                                    <%-- Summary info row --%>
                                    <div class="row mb-2">
                                        <div class="col-md-12">
                                            <span class="badge bg-info text-dark me-2">
                                                Total Records: <asp:Label ID="totalRecordsLabel" runat="server" Text="0"></asp:Label>
                                            </span>
                                            <span class="badge bg-success me-2">
                                                Total Amount: ৳ <asp:Label ID="totalAmountLabel" runat="server" Text="0.00"></asp:Label>
                                            </span>
                                        </div>
                                    </div>

                                    <%-- Grid --%>
                                    <div class="table-responsive" id="MainGradeDiv">
                                        <asp:GridView ID="loadGridView" runat="server"
                                            AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered table-hover"
                                            OnPreRender="gv_PreRender">
                                            <Columns>
                                                <asp:BoundField DataField="TransferDate"   HeaderText="Date"          DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="RefNo"          HeaderText="Ref #" />
                                                <asp:BoundField DataField="STONo"          HeaderText="STO No" />
                                                <asp:BoundField DataField="ProductCode"    HeaderText="Product Code" />
                                                <asp:BoundField DataField="ProductName"    HeaderText="Product Name" />
                                                <asp:BoundField DataField="BatchNo"        HeaderText="Batch No" />
                                                <asp:BoundField DataField="Quantity"       HeaderText="Qty" DataFormatString="{0:N0}" />
                                                <asp:BoundField DataField="UnitPrice"      HeaderText="Unit Price" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="VATAmount"      HeaderText="VAT Amt" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="Amount"         HeaderText="Amount" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="FromLocation"   HeaderText="From" />
                                                <asp:BoundField DataField="ToLocation"     HeaderText="To" />
                                                <asp:BoundField DataField="UserName"       HeaderText="User" />
                                                <asp:TemplateField HeaderText="STO Preview" HeaderStyle-CssClass="no-export text-center" ItemStyle-CssClass="no-export text-center text-nowrap">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" onclick="openReport('../SInventory_RPTVIEW/StockTransfarOrderReportViewer.aspx?reqId=<%# Eval("ReqId") %>');" 
                                                           class="btn btn-sm btn-outline-primary py-0 px-2" title="STO Voucher Preview">
                                                            <i class="bx bx-printer"></i> STO
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Challan Preview" HeaderStyle-CssClass="no-export text-center" ItemStyle-CssClass="no-export text-center text-nowrap">
                                                    <ItemTemplate>
                                                        <a href="javascript:void(0);" onclick="openReport('../SInventory_RPTVIEW/ChallanReportViewer.aspx?reqId=<%# Eval("ReqId") %>');" 
                                                           class="btn btn-sm btn-outline-success py-0 px-2" title="Challan Voucher Preview">
                                                            <i class="bx bx-file"></i> Challan
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        function openReport(url) {
            var Mleft = (screen.width / 2) - (950 / 2);
            var Mtop = (screen.height / 2) - (700 / 2);
            window.open(url, null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no,scrollbars=yes,menubar=no,location=no,top=' + Mtop + ',left=' + Mleft);
        }

        // Re-init Select2 and datepicker after UpdatePanel postback
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
                format: 'dd mmm yyyy',
                formatSubmit: 'dd-mmm-yyyy'
            });
        }

        var exportOptionsConfig = {
            columns: ':not(.no-export)'
        };

        function initDataTable() {
            return $('#ContentPlaceHolder1_loadGridView').DataTable({
                "bInfo": true,
                "bFilter": true,
                "order": [],
                lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                pageLength: 25,
                dom: 'lBfrtip',
                buttons: [
                    {
                        extend: 'excel',
                        text: '<i class="bx bxs-file-export"></i> Excel',
                        className: 'btn btn-sm btn-success',
                        title: 'Stock Transfer Report',
                        exportOptions: exportOptionsConfig
                    },
                    {
                        extend: 'pdf',
                        text: '<i class="bx bxs-file-pdf"></i> PDF',
                        className: 'btn btn-sm btn-danger',
                        title: 'Stock Transfer Report',
                        orientation: 'landscape',
                        pageSize: 'A4',
                        exportOptions: exportOptionsConfig
                    },
                    {
                        extend: 'print',
                        text: '<i class="bx bx-printer"></i> Print',
                        className: 'btn btn-sm btn-secondary',
                        exportOptions: exportOptionsConfig
                    },
                    {
                        extend: 'copy',
                        text: '<i class="bx bx-copy"></i> Copy',
                        className: 'btn btn-sm btn-info',
                        exportOptions: exportOptionsConfig
                    }
                ],
                language: {
                    search: "Global Search:"
                }
            });
        }

        $(document).ready(function () {
            var table = initDataTable();

            // Re-init DataTable after UpdatePanel async postback
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        table = initDataTable();
                    }
                });
            }
        });

    </script>

</asp:Content>
