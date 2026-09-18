<%@ page title="COGS-based GP Calculation Report" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_COGSBasedGPReport, App_Web_oqu23fcy" %>

<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register Src="~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx" TagPrefix="uc1" TagName="IVMarketStructure" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ssss {
            font-size: 13px;
            font-weight: bold;
        }
        .button-padding-right {
            margin-right: 5px;
        }
        .SelectchkChoice label {
            padding-left: 4px;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>COGS-based GP Calculation Report</div>
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
                                                selectYears: true
                                            })
                                        }
                                    </script>

                                    <div class="row">
                                        <div class="col-4">
                                            <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">Sales Center: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="dcDropDownList1" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">From Date:  <span style="color: red">*</span></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="InvoiceDateTextBox" AutoPostBack="true" OnTextChanged="fromDateTextBox_TextChanged" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select From Date"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">To Date:  <span style="color: red">*</span></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="todateTextBox" runat="server" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select To Date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                             <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">Customer: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2">
                                                    </asp:DropDownList>
                                                </div>
                                             </div>
                                             <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">Customer Type: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCustomerType" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2">
                                                    </asp:DropDownList>
                                                </div>
                                             </div>
                                        </div>

                                        <div class="col-md-4">
                                             <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">Product Name: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2">
                                                    </asp:DropDownList>
                                                </div>
                                             </div>
                                             <div class="form-group row" runat="server">
                                                <label class="col-sm-5 col-form-label">Sales Executive: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlOrderSender" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2">
                                                    </asp:DropDownList>
                                                </div>
                                             </div>
                                        </div>

                                        <div style="display: none;">
                                            <uc1:IVMarketStructure runat="server" ID="IVMarketStructure" />
                                        </div>
                                    </div>

                                    <br />
                                    <div class="row">
                                        <div class="col-2">&nbsp;</div>
                                        <div class="col-8">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label"></label>
                                                <div class="col-sm-8">
                                                    <asp:LinkButton OnClick="SearchButton_Click" runat="server" ID="submitButton" class="btn btnMyDesignSearch btn-sm">
                                                        <i class="fa fa-print" aria-hidden="true"></i>&nbsp; View Report
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" OnClick="cancelButton_Click" class="btn btnMyDesignReset btn-sm"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-4">
                                        <div class="col-4">
                                            <h3>Report Details</h3>
                                        </div>
                                        <div class="col-6">
                                            <asp:Label ID="lblCount" runat="server" CssClass="ssss btn bg-info pull-right" Text="Total Realization Amount : 0.00"></asp:Label>
                                        </div>
                                        <div class="col-2">
                                            <div class="form-group row pull-right">
                                                <asp:LinkButton ID="btnExport" class="btn btn-sm mb-2" Style="background-color: #1A7343; color: #fff;" runat="server" OnClick="btnExport_Click"><i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                    <hr />

                                    <div class="table-responsive" id="MainGradeDiv" style="height: 600px">
                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender" AllowPaging="True" PageIndex="0" OnPageIndexChanging="loadGridView_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="Month" HeaderText="Month" />
                                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                                <asp:BoundField DataField="ComUnitName" HeaderText="Sales Center" />
                                                <asp:BoundField DataField="BUWise" HeaderText="BU Wise" />
                                                <asp:BoundField DataField="Particulars" HeaderText="Particulars" />
                                                <asp:BoundField DataField="SKU" HeaderText="SKU" />
                                                <asp:BoundField DataField="Qty" HeaderText="Qty" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="OrderSenderName" HeaderText="Name of Sales Executive" />
                                                <asp:BoundField DataField="RealizationAmount" HeaderText="Realization Amount" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="VATAmount" HeaderText="VAT" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="TaxAmount" HeaderText="Tax" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="COGSAmount" HeaderText="COGS Amount" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="GP" HeaderText="GP" DataFormatString="{0:N2}" />
                                                <asp:TemplateField HeaderText="GP %">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGPPercent" runat="server" Text='<%# Eval("GPPercent") + "%" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExport" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function exportTableToExcel(tableID, filename) {
            var downloadLink;
            var dataType = 'application/vnd.ms-excel';
            var tableSelect = document.getElementById(tableID);
            var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

            filename = filename ? filename + '.xls' : 'excel_data.xls';
            downloadLink = document.createElement("a");
            document.body.appendChild(downloadLink);

            if (navigator.msSaveOrOpenBlob) {
                var blob = new Blob(['\ufeff', tableHTML], {
                    type: dataType
                });
                navigator.msSaveOrOpenBlob(blob, filename);
            } else {
                downloadLink.href = 'data:' + dataType + ', ' + tableHTML;
                downloadLink.download = filename;
                downloadLink.click();
            }
        }
    </script>
</asp:Content>
