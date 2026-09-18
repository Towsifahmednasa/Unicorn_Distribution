<%@ Page Title="Manual Order Creation" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master"
    AutoEventWireup="true" CodeFile="ManualOrderCreation.aspx.cs" Inherits="SInventory_UI_ManualOrderCreation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .autocomplete_completionListElement {
            margin: 0px !important;
            background-color: #ffffff !important;
            color: #333333 !important;
            border: 1px solid #ced4da !important;
            cursor: pointer !important;
            overflow: auto !important;
            font-family: inherit !important;
            font-size: 13px !important;
            text-align: left !important;
            list-style-type: none !important;
            margin-left: 0px !important;
            padding-left: 0px !important;
            max-height: 220px !important;
            width: 450px !important;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15) !important;
            border-radius: 6px !important;
            z-index: 99999 !important;
        }

        .autocomplete_highlightedListItem {
            background-color: #0d6efd !important;
            color: white !important;
            padding: 8px 12px !important;
            font-weight: 600 !important;
        }

        .autocomplete_listItem {
            padding: 8px 12px !important;
            cursor: pointer !important;
            font-weight: normal !important;
            background-color: #fff !important;
            border-bottom: 1px solid #f0f0f0 !important;
        }

        .customer-info-box {
            background: #f8f9fa;
            border-left: 4px solid #0d6efd;
            border-radius: 4px;
            padding: 10px 15px;
        }

        .table-custom-header th {
            background-color: #212529 !important;
            color: #ffffff !important;
            font-weight: 600;
            vertical-align: middle;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="divWaiting">
                        <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="page-wrapper">
                <div class="page-content">
                    <!--breadcrumb-->
                    <div class="page-breadcrumb d-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3">
                            <i class="bx bx-cart-alt"></i> Manual Order Creation
                        </div>
                        <div class="ms-auto">
                            <a href="OrderListView.aspx" class="btn btn-sm btn-outline-primary px-3 shadow-sm">
                                <i class="bx bx-list-ul me-1"></i> View Order List
                            </a>
                        </div>
                    </div>
                    <!--end breadcrumb-->

                    <!-- Order & Customer Information Card -->
                    <div class="card border-top border-0 border-4 border-primary shadow-sm mb-3">
                        <div class="card-header bg-transparent py-2">
                            <h6 class="mb-0 text-primary fw-bold"><i class="bx bx-info-circle me-1"></i> Customer & Order Information</h6>
                        </div>
                        <div class="card-body">
                            <div class="row g-3">
                                <!-- Customer Search -->
                                <div class="col-md-6">
                                    <label class="form-label fw-bold text-dark mb-1">
                                        <i class="bx bx-user me-1 text-primary"></i> Customer Name / Code: <span class="text-danger">*</span>
                                    </label>
                                    <div class="input-group">
                                        <asp:TextBox ID="custCodeTextBox" runat="server" CssClass="form-control form-control-sm"
                                            AutoPostBack="True" OnTextChanged="custCodeTextBox_TextChanged" placeholder="Type Customer Name, Code or Mobile No..."></asp:TextBox>
                                        <asp:AutoCompleteExtender
                                            ID="at_txt_JobCirculation"
                                            TargetControlID="custCodeTextBox"
                                            runat="server"
                                            ServiceMethod="GetCustomer_New"
                                            ServicePath="SInventoryWebService.asmx"
                                            MinimumPrefixLength="1"
                                            CompletionInterval="10"
                                            EnableCaching="false"
                                            CompletionSetCount="1"
                                            FirstRowSelected="false"
                                            CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                        </asp:AutoCompleteExtender>
                                        <span class="input-group-text bg-light text-primary"><i class="bx bx-search"></i></span>
                                    </div>
                                    <asp:HiddenField ID="hfCustomerId" runat="server" />
                                </div>

                                <!-- Sales Person (MIO) -->
                                <div class="col-md-6">
                                    <label class="form-label fw-bold text-dark mb-1">
                                        <i class="bx bx-user-pin me-1 text-primary"></i> Sales Person (MIO): <span class="text-danger">*</span>
                                    </label>
                                    <asp:DropDownList ID="ddlSalesPerson" runat="server" CssClass="form-select form-select-sm mySelect2"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlSalesPerson_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <!-- Customer Details Display Banner (Address & Phone) -->
                                <div class="col-12" id="pnlCustomerDetails" runat="server" visible="false">
                                    <div class="customer-info-box shadow-sm">
                                        <div class="row align-items-center">
                                            <div class="col-md-8">
                                                <i class="bx bx-map-pin text-danger me-1"></i>
                                                <strong class="text-muted">Address:</strong>
                                                <asp:Label ID="lblAddress" runat="server" CssClass="text-dark fw-bold ms-1" />
                                            </div>
                                            <div class="col-md-4 text-md-end">
                                                <i class="bx bx-phone-call text-success me-1"></i>
                                                <strong class="text-muted">Contact:</strong>
                                                <asp:Label ID="lblCellNo" runat="server" CssClass="text-dark fw-bold ms-1" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Order Details Row (2 Columns) -->
                                <div class="col-md-6">
                                    <label class="form-label fw-bold text-dark small mb-1">
                                        <i class="bx bx-calendar me-1 text-primary"></i> Order Date: <span class="text-danger">*</span>
                                    </label>
                                    <asp:TextBox ID="orderDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker" placeholder="Select Date"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label fw-bold text-dark small mb-1">
                                        <i class="bx bx-store me-1 text-primary"></i> Distribution Center: <span class="text-danger">*</span>
                                    </label>
                                    <asp:DropDownList ID="dcDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2">
                                    </asp:DropDownList>
                                </div>

                                <!-- Remarks / Note Field -->
                                <div class="col-12">
                                    <label class="form-label fw-bold text-dark small mb-1">
                                        <i class="bx bx-comment-detail me-1 text-primary"></i> Remarks / Special Note:
                                    </label>
                                    <asp:TextBox ID="remarksTextBox" runat="server" TextMode="MultiLine" Rows="2"
                                        CssClass="form-control form-control-sm" placeholder="Enter order remarks or notes (optional)..."></asp:TextBox>
                                </div>
                            </div>

                            <!-- Hidden storage for required background values -->
                            <div style="display:none;">
                                <asp:HiddenField ID="hfOrderId" runat="server" />
                                <asp:TextBox ID="orderNoTextBox" runat="server"></asp:TextBox>
                                <asp:Label ID="custNameLabel" runat="server"></asp:Label>
                                <asp:Label ID="mioCodeLabel" runat="server"></asp:Label>
                                <asp:Label ID="mioNameLabel" runat="server"></asp:Label>
                                <asp:Label ID="marketNameLabel" runat="server"></asp:Label>
                                <asp:Label ID="teritory" runat="server"></asp:Label>
                                <asp:Label ID="FCBLabel3" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>

                    <!-- Ordered Products Grid Card -->
                    <div class="card border-top border-0 border-4 border-success shadow-sm mb-3">
                        <div class="card-header bg-transparent py-2 d-flex align-items-center justify-content-between">
                            <h6 class="mb-0 text-success fw-bold"><i class="bx bx-package me-1"></i> Order Items / Products</h6>
                            <span class="badge bg-light-success text-success border border-success fw-normal">Add products and quantity below</span>
                        </div>
                        <div class="card-body p-0">
                            <div class="table-responsive">
                                <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover align-middle text-center mb-0"
                                    HeaderStyle-CssClass="table-custom-header">
                                    <Columns>
                                        <asp:BoundField DataField="SL" HeaderText="#" ItemStyle-Width="3%" />
                                        <asp:TemplateField HeaderText="Product Code" ItemStyle-Width="12%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="form-control form-control-sm text-center" AutoPostBack="True"
                                                    OnTextChanged="productCodeTextBox_TextChanged" Text='<%# Eval("ProductCode")%>' placeholder="Code"></asp:TextBox>
                                                <asp:HiddenField ID="unitpriceHiddenField" Value='<%# Eval("UnitPrice")%>' runat="server" />
                                                <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product Name" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="productNameTextBox" runat="server" CssClass="form-control form-control-sm text-start" Text='<%# Eval("ProductName")%>'
                                                    AutoPostBack="True" OnTextChanged="productNameTextBox_TextChanged" placeholder="Search product..."></asp:TextBox>
                                                <asp:AutoCompleteExtender
                                                    ID="at_txt_JobCirculassstion"
                                                    TargetControlID="productNameTextBox"
                                                    runat="server"
                                                    ServiceMethod="GetProductList"
                                                    ServicePath="SInventoryWebService.asmx"
                                                    MinimumPrefixLength="1"
                                                    CompletionInterval="10"
                                                    EnableCaching="false"
                                                    CompletionSetCount="1"
                                                    FirstRowSelected="false"
                                                    CompletionListCssClass="autocomplete_completionListElement"
                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    ShowOnlyCurrentWordInCompletionListItem="true">
                                                </asp:AutoCompleteExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pack Size" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="packSizeTextBox" runat="server" CssClass="form-control form-control-sm bg-light text-center" ReadOnly="True"
                                                    Text='<%# Eval("PackSize")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Req. Qty" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="reqQtyTextBox" runat="server" CssClass="form-control form-control-sm text-center fw-bold text-primary req-qty-input"
                                                    oninput="calculateRow(this);" onkeyup="calculateRow(this);" Text='<%# Eval("Quantity")%>' placeholder="0"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit TP (Tk)" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="tpTextBox" runat="server" CssClass="form-control form-control-sm text-end fw-bold text-dark unit-tp-input"
                                                    oninput="calculateRow(this);" onkeyup="calculateRow(this);" Text='<%# Eval("UnitPrice")%>' placeholder="0.00"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit VAT (Tk)" ItemStyle-Width="9%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="vatTextBox" runat="server" CssClass="form-control form-control-sm bg-light text-end unit-vat-input" ReadOnly="True" Text='<%# Eval("VAT")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total TP (Tk)" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TotaltpTextBox" runat="server" CssClass="form-control form-control-sm bg-light text-end fw-bold row-total-tp" ReadOnly="True" Text='<%# Eval("Totaltp")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total VAT (Tk)" ItemStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="TotaltpVatTextBox" runat="server" CssClass="form-control form-control-sm bg-light text-end row-total-vat" ReadOnly="True" Text='<%# Eval("TotaltpVat")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Add" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnRowAdd" runat="server" CssClass="btn btn-sm btn-outline-success p-1 px-2" OnClick="ImageButton1_Click" ToolTip="Add Row">
                                                    <i class="bx bx-plus"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Del" ItemStyle-Width="4%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnRowDelete" runat="server" CssClass="btn btn-sm btn-outline-danger p-1 px-2" OnClick="ImageButton2_Click" ToolTip="Remove Row">
                                                    <i class="bx bx-trash"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IsGiftProduct" Visible="false">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="IsGiftProductDropDownList" runat="server">
                                                    <asp:ListItem Value="True">True</asp:ListItem>
                                                    <asp:ListItem Value="False" Selected="True">False</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IsCampaignProduct" Visible="false">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="IsCampaignProductDropDownList" runat="server">
                                                    <asp:ListItem Value="True">True</asp:ListItem>
                                                    <asp:ListItem Value="False" Selected="True">False</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!-- Order Total Financial Summary Banner -->
                    <div class="card border-0 shadow-sm mb-3" style="background: #f8f9fc; border: 1px solid #e3e6f0 !important; border-radius: 8px;">
                        <div class="card-body py-3">
                            <div class="row align-items-center text-center">
                                <div class="col-md-3 border-end">
                                    <span class="text-muted small fw-bold d-block text-uppercase mb-1">Total Quantity</span>
                                    <span id="lblSumQty" class="fs-5 fw-bold text-dark">0</span>
                                </div>
                                <div class="col-md-3 border-end">
                                    <span class="text-muted small fw-bold d-block text-uppercase mb-1">Total TP (Gross)</span>
                                    <span class="fs-5 fw-bold text-primary">Tk <span id="lblSumTP">0.00</span></span>
                                </div>
                                <div class="col-md-3 border-end">
                                    <span class="text-muted small fw-bold d-block text-uppercase mb-1">Total VAT</span>
                                    <span class="fs-5 fw-bold text-warning">Tk <span id="lblSumVAT">0.00</span></span>
                                </div>
                                <div class="col-md-3">
                                    <span class="text-muted small fw-bold d-block text-uppercase mb-1">Net Payable Amount</span>
                                    <span class="fs-4 fw-bold text-success">Tk <span id="lblSumNetPayable">0.00</span></span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Action & Submit Card -->
                    <div class="card shadow-sm border-0 bg-light">
                        <div class="card-body d-flex align-items-center justify-content-end gap-2">
                            <asp:LinkButton ID="submitButton" runat="server" OnClick="submitButton_Click"
                                OnClientClick="return validateOrderForm();"
                                CssClass="btn btn-primary px-4 shadow-sm fw-bold">
                                <i class="bx bx-check-circle me-1"></i> Submit Order
                            </asp:LinkButton>
                            <asp:LinkButton ID="resetbtn" runat="server" OnClick="resetbtn_Click"
                                CssClass="btn btn-secondary px-4 shadow-sm">
                                <i class="bx bx-undo me-1"></i> Reset
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <script type="text/javascript">
                function validateOrderForm() {
                    var custCode = $('#<%= custCodeTextBox.ClientID %>').val();
                    if (!custCode || custCode.trim() === '') {
                        alert('Please Select a valid Customer!');
                        $('#<%= custCodeTextBox.ClientID %>').focus();
                        return false;
                    }

                    var spVal = $('#<%= ddlSalesPerson.ClientID %>').val();
                    if (!spVal || spVal === '' || spVal === '0') {
                        alert('Please Select Sales Person (MIO)!');
                        try {
                            $('#<%= ddlSalesPerson.ClientID %>').select2('open');
                        } catch (e) {
                            $('#<%= ddlSalesPerson.ClientID %>').focus();
                        }
                        return false;
                    }

                    var dcVal = $('#<%= dcDropDownList.ClientID %>').val();
                    if (!dcVal || dcVal === '' || dcVal === '0') {
                        alert('Please Select Distribution Center!');
                        $('#<%= dcDropDownList.ClientID %>').focus();
                        return false;
                    }

                    var hasValidProduct = false;
                    $('#<%= productGridView.ClientID %> tr').each(function () {
                        var qtyInput = $(this).find('.req-qty-input');
                        if (qtyInput.length > 0) {
                            var qty = parseFloat(qtyInput.val()) || 0;
                            if (qty > 0) {
                                hasValidProduct = true;
                            }
                        }
                    });

                    if (!hasValidProduct) {
                        alert('Please Add at least one Product item with valid quantity!');
                        return false;
                    }

                    return true;
                }

                function calculateRow(input) {
                    var row = $(input).closest('tr');
                    var qty = parseFloat(row.find('.req-qty-input').val()) || 0;
                    var unitTP = parseFloat(row.find('.unit-tp-input').val()) || 0;
                    var unitVAT = parseFloat(row.find('.unit-vat-input').val()) || 0;

                    var totalTP = (qty * unitTP);
                    var totalVAT = (qty * unitVAT);

                    row.find('.row-total-tp').val(totalTP.toFixed(2));
                    row.find('.row-total-vat').val(totalVAT.toFixed(2));

                    calculateGrandTotals();
                }

                function calculateGrandTotals() {
                    var grandQty = 0;
                    var grandTP = 0;
                    var grandVAT = 0;

                    $('#<%= productGridView.ClientID %> tr').each(function () {
                        var row = $(this);
                        var qtyInput = row.find('.req-qty-input');
                        if (qtyInput.length > 0) {
                            var qty = parseFloat(qtyInput.val()) || 0;
                            var unitTP = parseFloat(row.find('.unit-tp-input').val()) || 0;
                            var unitVAT = parseFloat(row.find('.unit-vat-input').val()) || 0;

                            var totalTP = qty * unitTP;
                            var totalVAT = qty * unitVAT;

                            if (qty > 0) {
                                row.find('.row-total-tp').val(totalTP.toFixed(2));
                                row.find('.row-total-vat').val(totalVAT.toFixed(2));
                            }

                            grandQty += qty;
                            grandTP += totalTP;
                            grandVAT += totalVAT;
                        }
                    });

                    var grandNet = grandTP + grandVAT;

                    $('#lblSumQty').text(grandQty);
                    $('#lblSumTP').text(grandTP.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
                    $('#lblSumVAT').text(grandVAT.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
                    $('#lblSumNetPayable').text(grandNet.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }));
                }

                function pageLoad() {
                    $('.datepicker').pickadate({
                        selectMonths: true,
                        selectYears: true
                    });

                    $('.mySelect2').select2({
                        theme: 'bootstrap4',
                        width: '100%'
                    });

                    calculateGrandTotals();
                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
