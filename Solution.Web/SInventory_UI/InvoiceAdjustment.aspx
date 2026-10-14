<%@ Page Title="Invoice Adjustment" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="InvoiceAdjustment.aspx.cs" Inherits="SInventory_UI_InvoiceAdjustment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .autocomplete_completionListElement { margin: 0 !important; background: #fff !important; color: #333 !important; border: 1px solid #ced4da !important; overflow: auto; max-height: 250px; text-align: left; list-style-type: none !important; padding: 0 !important; z-index: 1050 !important; box-shadow: 0 8px 16px rgba(0,0,0,.15) !important; border-radius: 4px; min-width: 300px; }
        .autocomplete_listItem { padding: 6px 12px !important; border-bottom: 1px solid #f1f1f1; font-size: 13px; }
        .autocomplete_highlightedListItem { background: #0d6efd !important; color: #fff !important; padding: 6px 12px !important; cursor: pointer; font-size: 13px; }
        .divWaiting { position: fixed; background: #FAFAFA; z-index: 2147483647 !important; opacity: .8; text-align: center; top: 0; left: 0; height: 100%; width: 100%; padding-top: 20%; }
        table[id$="detailGridView"] th, table[id$="detailGridView"] td { white-space: nowrap; font-size: 12px; padding: 4px 6px !important; }
        table[id$="detailGridView"] th { background: #2b3e50 !important; color: #fff !important; }
        .adjAmt { width: 110px; text-align: right; }
        .info-box span { display: inline-block; margin-right: 18px; }
    </style>
    <script type="text/javascript">
        // Clamp each adjustment to its max (DeliveryNetAmount - previously adjusted) and keep the running total.
        function recalcAdjustment() {
            var total = 0;
            var inputs = document.querySelectorAll('.adjAmt');
            for (var i = 0; i < inputs.length; i++) {
                var el = inputs[i];
                if (el.disabled) continue;
                var v = parseFloat(el.value) || 0;
                var max = parseFloat(el.getAttribute('data-max')) || 0;
                if (v < 0) v = 0;
                if (v > max) { v = max; el.value = max.toFixed(2); }
                total += v;
            }
            var lbl = document.getElementById('totalAdjustmentSpan');
            if (lbl) lbl.innerHTML = total.toFixed(2);
        }
        document.addEventListener('input', function (e) {
            if (e.target && e.target.className.indexOf('adjAmt') >= 0) recalcAdjustment();
        });
        function pageLoad() { recalcAdjustment(); }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="0">
                <ProgressTemplate>
                    <div class="divWaiting">
                        <asp:Image ID="imgWait" runat="server" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Invoice Adjustment (Bad Debts / Promotional Expense)</div>
                        <div class="ms-auto">
                            <div class="btn-group">
                                <a href="InvoiceAdjustmentList.aspx" class="btn btn-sm btn-outline-info"><i class="fa fa-backward"></i>&nbsp;Back to List</a>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">

                                    <div class="row">
                                        <div class="col-md-2">&nbsp;</div>
                                        <div class="col-md-8">
                                            <div class="form-group row mb-2">
                                                <label class="col-sm-3 col-form-label">Invoice Adjustment Type:</label>
                                                <div class="col-sm-7">
                                                    <asp:RadioButtonList ID="adjustmentTypeRadioButtonList" runat="server" RepeatDirection="Horizontal" CssClass="mt-1">
                                                        <asp:ListItem Text="&nbsp;Bad Debts&nbsp;&nbsp;&nbsp;" Value="Bad Debts" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="&nbsp;Promotional Expense" Value="Promotional Expense"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <div class="form-group row mb-2">
                                                <label class="col-sm-3 col-form-label">Invoice No:</label>
                                                <div class="col-sm-5">
                                                    <asp:TextBox ID="invoiceNoTextBox" runat="server" CssClass="form-control form-control-sm" placeholder="Type invoice no" autocomplete="off"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="invoiceNoAutoCompleteExtender" runat="server"
                                                        TargetControlID="invoiceNoTextBox" ServiceMethod="GetProformaInvoiceNo" ServicePath="SInventoryWebService.asmx"
                                                        MinimumPrefixLength="1" CompletionInterval="10" EnableCaching="false" CompletionSetCount="15"
                                                        CompletionListCssClass="autocomplete_completionListElement"
                                                        CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                    </asp:AutoCompleteExtender>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="searchButton" runat="server" Text="Search" CssClass="btn btn-sm btn-primary px-4" OnClick="searchButton_Click" />
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <asp:HiddenField ID="hfInvoiceId" runat="server" />
                                            <asp:HiddenField ID="hfCustomerMasterId" runat="server" />
                                            <asp:HiddenField ID="hfComUnitId" runat="server" />
                                        </div>
                                    </div>

                                    <asp:Panel ID="detailPanel" runat="server" Visible="false">
                                        <hr class="my-3" />
                                        <div class="alert alert-light border info-box py-2 mb-2">
                                            <span><b>Customer:</b> <asp:Label ID="customerLabel" runat="server" /></span>
                                            <span><b>Invoice Date:</b> <asp:Label ID="invoiceDateLabel" runat="server" /></span>
                                            <span><b>Delivery Invoice:</b> <asp:Label ID="deliveryInvoiceLabel" runat="server" /></span>
                                            <span><b>Delivered Amount:</b> <asp:Label ID="deliveredAmountLabel" runat="server" /></span>
                                            <span><b>Paid:</b> <asp:Label ID="paidAmountLabel" runat="server" /></span>
                                            <span><b>Previous Adjustment:</b> <asp:Label ID="prevAdjustmentLabel" runat="server" /></span>
                                        </div>
                                        <div class="small text-muted mb-2">
                                            Adjustment Amount cannot exceed the line's Delivery Net Amount (less any earlier adjustment). Rejected lines cannot be adjusted.
                                        </div>

                                        <div class="table-responsive" style="max-height: 480px;">
                                            <asp:GridView ID="detailGridView" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-sm text-center"
                                                DataKeyNames="InvoiceDetailId" OnRowDataBound="detailGridView_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="ProductCode" HeaderText="ProductCode" />
                                                    <asp:BoundField DataField="ProductName" HeaderText="ProductName" ItemStyle-HorizontalAlign="Left" />
                                                    <asp:BoundField DataField="CostPrice" HeaderText="CostPrice" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="UnitVatAmount" HeaderText="UnitVatAmount" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                                                    <asp:BoundField DataField="BonusQuantity" HeaderText="BonusQuantity" />
                                                    <asp:BoundField DataField="TotalQuantity" HeaderText="TotalQuantity" />
                                                    <asp:BoundField DataField="TotalPrice" HeaderText="TotalPrice" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="TotalPriceVatAmount" HeaderText="TotalPriceVatAmount" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="DiscountPercentage" HeaderText="DiscountPercentage" />
                                                    <asp:BoundField DataField="DiscountAmount" HeaderText="DiscountAmount" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="NetAmount" HeaderText="NetAmount" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="InvoiceId" HeaderText="InvoiceId" />
                                                    <asp:BoundField DataField="DCStoreId" HeaderText="DCStoreId" />
                                                    <asp:BoundField DataField="DeliveryQuantity" HeaderText="DeliveryQuantity" />
                                                    <asp:BoundField DataField="DeliveryBonusQuantity" HeaderText="DeliveryBonusQuantity" />
                                                    <asp:BoundField DataField="DeliveryTotalQuantity" HeaderText="DeliveryTotalQuantity" />
                                                    <asp:BoundField DataField="DeliveryTotalPrice" HeaderText="DeliveryTotalPrice" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="DeliveryTotalPriceVatAmount" HeaderText="DeliveryTotalPriceVatAmount" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="DeliveryDiscountPercentage" HeaderText="DeliveryDiscountPercentage" />
                                                    <asp:BoundField DataField="DeliveryNetAmount" HeaderText="DeliveryNetAmount" DataFormatString="{0:N2}" />
                                                    <asp:BoundField DataField="DeliveryStatus" HeaderText="DeliveryStatus" />
                                                    <asp:BoundField DataField="PrevAdjustment" HeaderText="Prev. Adjustment" DataFormatString="{0:N2}" />
                                                    <asp:TemplateField HeaderText="Adjustment Amount">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="adjustmentTextBox" runat="server" CssClass="form-control form-control-sm adjAmt" Text="" placeholder="0.00"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="adjustmentFilteredTextBoxExtender" runat="server" TargetControlID="adjustmentTextBox" FilterType="Custom" ValidChars="0123456789." />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                        <div class="row mt-3 align-items-center">
                                            <div class="col-md-6">
                                                <div class="form-group row">
                                                    <label class="col-sm-2 col-form-label">Remarks:</label>
                                                    <div class="col-sm-10">
                                                        <asp:TextBox ID="remarksTextBox" runat="server" CssClass="form-control form-control-sm" MaxLength="500"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 text-end">
                                                <span class="btn bg-info text-dark me-3">Total Adjustment: <b id="totalAdjustmentSpan">0.00</b></span>
                                                <asp:Button ID="saveButton" runat="server" Text="Save Adjustment" CssClass="btn btn-success px-4" OnClick="saveButton_Click"
                                                    OnClientClick="return confirm('Save this invoice adjustment and post to accounts?');" />
                                            </div>
                                        </div>
                                    </asp:Panel>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
