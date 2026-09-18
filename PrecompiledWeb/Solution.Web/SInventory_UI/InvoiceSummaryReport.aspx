<%@ page title="Invoice Summary Report" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_InvoiceSummaryReport, App_Web_02kl0opo" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ssss {
            font-size: 13px;
            font-weight: bold;
        }
        .print-btn {
            padding: 2px 6px;
            font-size: 11px;
            margin-right: 2px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Invoice Summary Report</div>
                <div class="ms-auto">
                    <div class="btn-group"></div>
                </div>
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

                                        function openReportWindow(url) {
                                            var Mleft = (screen.width/2)-(950/2);
                                            var Mtop = (screen.height/2)-(700/2);
                                            window.open(url, null, 'height=700,width=950,status=yes,toolbar=no,addressbar=no,scrollbars=yes,menubar=no,location=no,top='+Mtop+',left='+Mleft);
                                        }
                                    </script>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group row mb-2">
                                                <label class="col-sm-5 col-form-label">Sales Center: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="dcDropDownList1" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group row mb-2">
                                                <label class="col-sm-5 col-form-label">From Date: <span class="text-danger">*</span></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="InvoiceDateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select From Date"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row mb-2">
                                                <label class="col-sm-5 col-form-label">To Date: <span class="text-danger">*</span></label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="todateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select To Date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group row mb-2">
                                                <label class="col-sm-5 col-form-label">Customer: </label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="form-group row mb-2">
                                                <label class="col-sm-5 col-form-label">Proforma Invoice No: </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtProformaNo" runat="server" class="form-control form-control-sm" placeholder="Proforma No"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group row mb-2">
                                                <label class="col-sm-5 col-form-label">Delivery Invoice No: </label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtDeliveryNo" runat="server" class="form-control form-control-sm" placeholder="Delivery No"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mt-3">
                                        <div class="col-12 text-center">
                                            <asp:LinkButton OnClick="SearchButton_Click" runat="server" ID="submitButton" class="btn btnMyDesignSearch btn-sm" Style="margin-right: 8px;">
                                                <i class="fa fa-print" aria-hidden="true"></i>&nbsp; View Report
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="btnReset" OnClick="cancelButton_Click" class="btn btnMyDesignReset btn-sm">
                                                <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset
                                            </asp:LinkButton>
                                        </div>
                                    </div>

                                    <hr class="my-4" />

                                    <div class="row align-items-center mb-3">
                                        <div class="col-md-3">
                                            <h3 class="mb-0">Details List</h3>
                                        </div>
                                        <div class="col-md-9">
                                            <div class="d-flex flex-wrap justify-content-md-end justify-content-start gap-2 align-items-center">
                                                <asp:Label ID="lblTotalReceivable" runat="server" CssClass="ssss btn bg-info text-dark" Text="Total Receivable: 0.00"></asp:Label>
                                                <asp:Label ID="lblTotalPaid" runat="server" CssClass="ssss btn bg-info text-dark" Text="Total Paid: 0.00"></asp:Label>
                                                <asp:Label ID="lblTotalDue" runat="server" CssClass="ssss btn bg-info text-dark" Text="Total Due: 0.00"></asp:Label>
                                                <asp:LinkButton ID="btnExport" class="btn btn-sm text-white" Style="background-color: #1A7343; border-color: #1A7343;" runat="server" OnClick="btnExport_Click">
                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="table-responsive" id="MainGradeDiv" style="height: 600px">
                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender" 
                                            AllowPaging="True" PageIndex="0" OnPageIndexChanging="loadGridView_PageIndexChanging" 
                                            DataKeyNames="InvoiceId" OnRowCommand="loadGridView_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Business Unit" HeaderText="Business Unit" />
                                                <asp:BoundField DataField="Customer Code" HeaderText="Customer Code" />
                                                <asp:BoundField DataField="Customer Name" HeaderText="Customer Name" />
                                                <asp:BoundField DataField="Proforma Invoice No" HeaderText="Proforma Invoice No" />
                                                <asp:BoundField DataField="Proforma Invoice Date" HeaderText="Proforma Date" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="Delivery Invoice No" HeaderText="Delivery Invoice No" />
                                                <asp:BoundField DataField="Delivery Invoice Date" HeaderText="Delivery Date" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="Receiveable Amount" HeaderText="Receivable Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="Total Payment" HeaderText="Total Payment" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="Last Payment Date" HeaderText="Last Payment Date" HtmlEncode="false" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="Due Amount" HeaderText="Due Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" />
                                                
                                                <asp:TemplateField HeaderText="Print Actions">
                                                    <ItemStyle Width="250px" HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkPrintProforma" runat="server" CssClass="btn btn-outline-primary print-btn btn-xs" 
                                                            CommandName="PrintProforma" CommandArgument='<%# Eval("Proforma Invoice No") %>'>
                                                            <i class="fa fa-print"></i> PI
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkPrintChallan" runat="server" CssClass="btn btn-outline-success print-btn btn-xs" 
                                                            CommandName="PrintChallan" CommandArgument='<%# Eval("Delivery Invoice No") %>'>
                                                            <i class="fa fa-print"></i> Challan
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="lnkPrintReceipt" runat="server" CssClass="btn btn-outline-warning print-btn btn-xs" 
                                                            CommandName="PrintReceipt" CommandArgument='<%# Eval("InvoiceId") %>'>
                                                            <i class="fa fa-print"></i> Receipt
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />
                                            <EmptyDataTemplate>
                                                <div style="text-align:center; padding:20px; font-weight:bold; color:red;">
                                                    No Data Found!!
                                                </div>
                                            </EmptyDataTemplate>
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
</asp:Content>
