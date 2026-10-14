<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="SalesReturn.aspx.cs" Inherits="SInventory_UI_SalesReturn" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        /* ==========================================================================
           AjaxControlToolkit AutoComplete Extender Styling
           ========================================================================== */
        .autocomplete_completionListElement {
            margin: 2px 0 0 0 !important;
            padding: 4px 0 !important;
            background-color: #ffffff !important;
            color: #2b303a !important;
            border: 1px solid #ced4da !important;
            border-radius: 6px !important;
            cursor: default !important;
            overflow-y: auto !important;
            overflow-x: hidden !important;
            max-height: 260px !important;
            text-align: left !important;
            list-style-type: none !important;
            z-index: 105000 !important;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.18) !important;
            min-width: 320px !important;
        }

        .autocomplete_completionListElement li,
        .autocomplete_listItem {
            background-color: #ffffff !important;
            color: #2b303a !important;
            padding: 8px 14px !important;
            border-bottom: 1px solid #f1f3f5 !important;
            font-size: 13px !important;
            line-height: 1.4 !important;
            list-style-type: none !important;
            cursor: pointer !important;
            transition: background-color 0.15s ease-in-out, color 0.15s ease-in-out !important;
        }

        .autocomplete_completionListElement li:hover,
        .autocomplete_highlightedListItem {
            background-color: #0d6efd !important;
            color: #ffffff !important;
            padding: 8px 14px !important;
            cursor: pointer !important;
            font-size: 13px !important;
            line-height: 1.4 !important;
            list-style-type: none !important;
            border-bottom: 1px solid #0d6efd !important;
            font-weight: 500 !important;
        }

        .autocomplete_completionListElement::-webkit-scrollbar {
            width: 6px;
        }
        .autocomplete_completionListElement::-webkit-scrollbar-thumb {
            background: #c1c9d2;
            border-radius: 4px;
        }
        .autocomplete_completionListElement::-webkit-scrollbar-thumb:hover {
            background: #9aa5b1;
        }

        /* ==========================================================================
           Card & Form Controls Styling
           ========================================================================== */
        .form-card {
            border-radius: 8px;
            box-shadow: 0 2px 12px rgba(0, 0, 0, 0.06);
            border: 1px solid #e9ecef;
            background-color: #ffffff;
        }

        .section-badge {
            font-size: 12px;
            font-weight: 500;
            padding: 4px 10px;
            border-radius: 20px;
        }

        .table-custom th {
            font-size: 12px;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            font-weight: 600;
            vertical-align: middle;
            padding: 9px 8px;
        }

        .table-custom td {
            font-size: 13px;
            vertical-align: middle;
            padding: 6px 8px;
        }

        .radioChoice label {
            padding-left: 4px;
            padding-right: 15px;
            font-size: 13px;
            font-weight: bold;
            cursor: pointer;
        }
        .radioChoice input[type="radio"] {
            margin-right: 2px;
            vertical-align: middle;
        }

        /* Anchor table cells for datepicker dropdowns */
        #ssada td, #ssada th {
            position: relative !important;
        }
        #ssada {
            overflow: visible !important;
        }

        /* Grid Datepicker alignment: drop right-aligned below the input */
        #ssada .picker {
            position: absolute !important;
            top: 100% !important;
            right: 0 !important;
            left: auto !important;
            width: auto !important;
            z-index: 10500 !important;
        }
        #ssada .picker__holder {
            position: absolute !important;
            top: 0 !important;
            right: 0 !important;
            left: auto !important;
            min-width: 250px !important;
            max-width: 260px !important;
            box-shadow: 0 6px 20px rgba(0,0,0,0.18) !important;
            border-radius: 6px !important;
            border: 1px solid #ced4da !important;
        }
        #ssada .picker__box {
            font-size: 12px !important;
            padding: 6px 8px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!-- Breadcrumb Header -->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize text-success"></i> Sales Return</div>
                <div class="ps-3 text-secondary" style="font-size: 13px;">Manage customer product returns and credit calculations</div>
                <div class="ms-auto">
                    <div class="btn-group">
                        <asp:LinkButton ID="viewLinkButton" class="btn btn-sm btn-outline-secondary shadow-sm"
                            OnClick="ListImageButton_Click" runat="server"><i class="fa fa-arrow-left"></i>&nbsp; Back to Return List</asp:LinkButton>
                    </div>
                </div>
            </div>
            <!-- End Breadcrumb -->

            <div class="row">
                <div class="col-12">
                    <div class="card form-card border-top border-0 border-4 border-success">
                        <script type="text/javascript">
                            function pageLoad() {
                                $('.datepicker').pickadate({
                                    selectMonths: true,
                                    selectYears: true,
                                    format: 'dd-mmm-yyyy',
                                    formatSubmit: 'dd-mmm-yyyy'
                                });
                                $('.mySelect2').select2({
                                    theme: 'bootstrap4',
                                    width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                    placeholder: $(this).data('placeholder'),
                                    allowClear: Boolean($(this).data('allow-clear'))
                                });
                            }
                        </script>

                        <div class="card-body p-4">
                            <!-- Customer & Order Information Header -->
                            <div class="d-flex align-items-center mb-3 pb-2 border-bottom">
                                <h6 class="text-success font-weight-bold mb-0"><i class="fa fa-user-circle"></i> Return & Customer Information</h6>
                            </div>

                            <div class="row g-3">
                                <!-- Left Column -->
                                <div class="col-lg-6">
                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Distribution Center <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:DropDownList ID="salesCenterDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2"
                                                AutoPostBack="True" OnSelectedIndexChanged="salesCenterDropDownList_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Customer Code <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="custCodeTextBox" runat="server" AutoPostBack="True" CssClass="form-control form-control-sm font-weight-bold"
                                                placeholder="Enter Customer Code" OnTextChanged="custCodeTextBox_TextChanged"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="custCodeTextBox_AutoCompleteExtender" runat="server"
                                                DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                CompletionSetCount="15" ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx"
                                                TargetControlID="custCodeTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                ShowOnlyCurrentWordInCompletionListItem="true">
                                            </asp:AutoCompleteExtender>

                                            <!-- Hidden Fields -->
                                            <asp:HiddenField ID="hdCustomerMasterId" runat="server" />
                                            <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                                            <asp:HiddenField ID="orderHiddenField" runat="server" />
                                            <asp:HiddenField ID="hdComUnitId" runat="server" />
                                            <asp:HiddenField ID="hdMiaId" runat="server" />
                                            <asp:HiddenField ID="hfMIOId" runat="server" />
                                            <asp:HiddenField ID="hfMioEmpId" runat="server" />
                                            <asp:HiddenField ID="hfTerriId" runat="server" />
                                            <asp:HiddenField ID="hfSapCode" runat="server" />
                                        </div>
                                    </div>

                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Customer Name <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="custNameTextBox" runat="server" CssClass="form-control form-control-sm font-weight-bold"
                                                placeholder="Type to search customer..." AutoPostBack="True" OnTextChanged="custNameTextBox_TextChanged"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server"
                                                DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1"
                                                CompletionSetCount="15" ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx"
                                                TargetControlID="custNameTextBox" UseContextKey="True" CompletionListCssClass="autocomplete_completionListElement"
                                                CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                ShowOnlyCurrentWordInCompletionListItem="true">
                                            </asp:AutoCompleteExtender>
                                        </div>
                                    </div>

                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Customer Address <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="custAddressTextBox" runat="server" CssClass="form-control form-control-sm bg-light" ReadOnly="True" placeholder="Address"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- Hidden / Legacy Controls preserved for code-behind compatibility -->
                                    <div class="row" runat="server" visible="false">
                                        <asp:TextBox ID="comUnitNameTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="miaCodeTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="miaNameTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:CheckBox ID="fixedcustomerCheckBox1" runat="server" />
                                    </div>
                                </div>

                                <!-- Right Column -->
                                <div class="col-lg-6">
                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Sales Person Name <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:Label ID="lblMioInfo" runat="server" CssClass="form-control form-control-sm bg-light text-dark font-weight-bold" Style="min-height: 31px; display: flex; align-items: center;"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Return Date <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <div class="input-group input-group-sm">
                                                <span class="input-group-text bg-white"><i class="fa fa-calendar text-secondary"></i></span>
                                                <asp:TextBox ID="orderDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker" ReadOnly="True" placeholder="DD-MMM-YYYY"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Remarks <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox ID="orderNoTextBox" runat="server" CssClass="form-control form-control-sm" placeholder="Return From Market"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mb-2 align-items-center">
                                        <label class="col-sm-4 col-form-label font-weight-bold text-secondary text-sm-end">
                                            Invoice No <span class="text-danger">*</span> :
                                        </label>
                                        <div class="col-sm-8">
                                            <asp:DropDownList ID="ddlInvoiceNo" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <!-- Hidden fields preserved for code-behind compatibility -->
                                    <div class="row" runat="server" visible="false">
                                        <asp:TextBox ID="districtNameTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="areaNameTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="marketNameTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:TextBox ID="custCategoryTextBox" runat="server" ReadOnly="True"></asp:TextBox>
                                        <asp:RadioButtonList RepeatLayout="Flow" ID="rbTypePrice" CssClass="radioChoice" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Old Price">Old Price</asp:ListItem>
                                            <asp:ListItem Value="New Price" Selected="True">New Price</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                            </div>

                            <hr class="my-4 text-muted opacity-25" />

                            <!-- Available Products Section Header -->
                            <div class="row mb-2">
                                <div class="col-12 d-flex justify-content-between align-items-center">
                                    <h6 class="text-primary font-weight-bold mb-0">
                                        <i class="fa fa-boxes"></i> Available Products For Return
                                    </h6>
                                    <span class="badge bg-light text-primary border section-badge">
                                        <i class="fa fa-check-square-o"></i> Select products and enter return quantities
                                    </span>
                                </div>
                            </div>

                            <!-- Available Products Grid -->
                            <div class="row mb-3">
                                <div class="col-12">
                                    <div class="table-responsive border rounded shadow-sm" id="MainGradeDiv" style="max-height: 350px; overflow-y: auto;">
                                        <asp:GridView ID="productGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-hover table-striped table-bordered table-sm table-custom mb-0 align-middle text-center"
                                            DataKeyNames="ProductId,UnitPrice,CostPrice">
                                            <HeaderStyle CssClass="bg-dark text-white font-weight-bold" Height="36px" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="45px" ItemStyle-Width="45px" ItemStyle-VerticalAlign="Middle">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" ToolTip="Select / Deselect All" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ProductCode" HeaderText="Product Code" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-CssClass="font-weight-bold text-primary" ItemStyle-VerticalAlign="Middle" />
                                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="text-dark px-3 font-weight-bold" ItemStyle-VerticalAlign="Middle" />
                                                <asp:BoundField DataField="CostPrice" HeaderText="Cost Price" DataFormatString="{0:N2}" HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="UnitPrice" HeaderText="Sales Price" DataFormatString="{0:N2}" HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Right" />
                                                <asp:TemplateField HeaderText="Return Qty" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="transferQtyTextBox" runat="server" CssClass="form-control form-control-sm text-center font-weight-bold mx-auto border-primary" Width="110px" placeholder="Qty"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderconvRate" runat="server"
                                                            Enabled="True" TargetControlID="transferQtyTextBox" FilterType="Custom" ValidChars="0123456789">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                            <!-- Add To List Action Row -->
                            <div class="row my-3">
                                <div class="col-12 text-center">
                                    <asp:LinkButton ID="LinkButton5" CssClass="btn btn-primary px-4 py-2 shadow-sm font-weight-bold" runat="server" OnClick="addButton_Click">
                                        <i class="fa fa-plus-circle"></i> &nbsp;Add Selected Products To Return List
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <!-- Return Line Items Section Header -->
                            <div class="row mt-4 mb-2">
                                <div class="col-12 d-flex justify-content-between align-items-center">
                                    <h6 class="text-success font-weight-bold mb-0">
                                        <i class="fa fa-shopping-basket"></i> Return Line Items List
                                    </h6>
                                    <span class="badge bg-light text-success border section-badge">
                                        <i class="fa fa-pencil-square-o"></i> Review quantities, batch, manufacturing & expiry dates
                                    </span>
                                </div>
                            </div>

                            <!-- Added Line Items Grid -->
                            <div class="row mb-4">
                                <div class="col-12">
                                    <div class="table-responsive border rounded shadow-sm" id="ssada">
                                        <asp:GridView ID="gridLineItemGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-hover table-striped table-bordered table-sm table-custom mb-0 align-middle text-center">
                                            <HeaderStyle CssClass="bg-dark text-white font-weight-bold" Height="36px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="slLabel" runat="server" Text='<%# Eval("SL")%>'></asp:Label>
                                                        <asp:ImageButton ID="addImageButton" runat="server" ImageUrl="~/images/lineAdd.png"
                                                            OnClick="addImageButton_Click" />
                                                        <asp:ImageButton ID="removeImageButton" runat="server" ImageUrl="~/images/lineDelete.png"
                                                            OnClick="removeImageButton_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Code" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="codeTextBox" runat="server" Text='<%# Eval("ProductCode")%>' CssClass="form-control form-control-sm text-center font-weight-bold text-primary"
                                                            AutoPostBack="True" OnTextChanged="codeTextBox_TextChanged" ReadOnly="True"></asp:TextBox>
                                                        <asp:HiddenField ID="orderdetailIdHiddenField" runat="server" Value='<%# Eval("OrderDetailsId")%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="nameTextBox" runat="server" CssClass="form-control form-control-sm font-weight-bold text-dark" Text='<%# Eval("ProductName")%>'
                                                            AutoPostBack="True" OnTextChanged="nameTextBox_TextChanged" ReadOnly="True"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CStock" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="currentStockTextBox" runat="server" CssClass="form-control form-control-sm text-center font-weight-bold text-danger"
                                                            Text='<%# Eval("StockQty")%>' ReadOnly="True" Style="background-color: #f8f9fa;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fcurrentStockTextBox" runat="server" TargetControlID="currentStockTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit Price" HeaderStyle-Width="105px" ItemStyle-Width="105px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="unitPriceTextBox" runat="server" CssClass="form-control form-control-sm text-end font-weight-bold border-primary" Text='<%# Eval("UnitPrice")%>'
                                                            AutoPostBack="True" OnTextChanged="unitPriceTextBox_TextChanged" placeholder="0.00"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="funitPriceTextBox" runat="server" TargetControlID="unitPriceTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cost Price" HeaderStyle-Width="105px" ItemStyle-Width="105px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="cogsTextBox" runat="server" CssClass="form-control form-control-sm text-end font-weight-bold border-primary" Text='<%# Eval("CostPrice")%>'
                                                            placeholder="0.00"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fcogsTextBox" runat="server" TargetControlID="cogsTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Unit VAT" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="upVatTextBox" runat="server" CssClass="form-control form-control-sm text-end" Text='<%# Eval("UnitVAT")%>'
                                                            ReadOnly="True" Style="background-color: #f8f9fa;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fupVatTextBox" runat="server" TargetControlID="upVatTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Return Qty" HeaderStyle-Width="85px" ItemStyle-Width="85px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="qtyTextBox" runat="server" CssClass="form-control form-control-sm text-center font-weight-bold border-primary"
                                                            Text='<%# Eval("Quantity")%>' AutoPostBack="True" OnTextChanged="qtyTextBox_TextChanged" placeholder="0"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fqtyTextBox" runat="server" TargetControlID="qtyTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total TP" HeaderStyle-Width="95px" ItemStyle-Width="95px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tpTextBox" runat="server" CssClass="form-control form-control-sm text-end font-weight-bold" Text='<%# Eval("TotalPrice")%>'
                                                            ReadOnly="True" Style="background-color: #f8f9fa;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftpTextBox" runat="server" TargetControlID="tpTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DP" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="dpTextBox" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("DiscountPercentage")%>'
                                                            ReadOnly="True"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fdpTextBox" runat="server" TargetControlID="dpTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DAmt" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="dpAmtTextBox" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("DiscountAmount")%>'
                                                            ReadOnly="True"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fdpAmtTextBox" runat="server" TargetControlID="dpAmtTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SD" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sdTextBox" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("SpecialAmount")%>'
                                                            ReadOnly="True"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fsdTextBox" runat="server" TargetControlID="sdTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total VAT" HeaderStyle-Width="85px" ItemStyle-Width="85px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tpVatTextBox" runat="server" CssClass="form-control form-control-sm text-end" Text='<%# Eval("VAT")%>'
                                                            ReadOnly="True" Style="background-color: #f8f9fa;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftpVatTextBox" runat="server" TargetControlID="tpVatTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Net Price" HeaderStyle-Width="95px" ItemStyle-Width="95px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="npTextBox" runat="server" CssClass="form-control form-control-sm text-end font-weight-bold" Text='<%# Eval("NetPrice")%>'
                                                            ReadOnly="True" Style="background-color: #f8f9fa;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fnpTextBox" runat="server" TargetControlID="npTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="BQty" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="bQtyTextBox" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("BonusQty")%>'
                                                            AutoPostBack="True" OnTextChanged="bQtyTextBox_TextChanged" ReadOnly="True"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="fbQtyTextBox" runat="server" TargetControlID="bQtyTextBox"
                                                            FilterType="Custom, Numbers" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Qty" HeaderStyle-Width="75px" ItemStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tQtyTextBox" runat="server" CssClass="form-control form-control-sm text-center font-weight-bold" Text='<%# Eval("TotalQty")%>'
                                                            ReadOnly="True" Style="background-color: #f8f9fa;"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mfg Date" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="mfgDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker text-center" placeholder="DD-MMM-YYYY"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="expDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker text-center" placeholder="DD-MMM-YYYY"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Batch No" HeaderStyle-Width="110px" ItemStyle-Width="110px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="batchNoTextBox" runat="server" CssClass="form-control form-control-sm text-center" placeholder="Batch No"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="add" runat="server" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="addImageButton2" runat="server" ImageUrl="~/images/lineAdd.png"
                                                            OnClick="addImageButton_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                            <!-- Bottom Section: Summary & Print Voucher Cards -->
                            <div class="row mt-4">
                                <!-- Left: Print Voucher Card -->
                                <div class="col-lg-6 mb-3">
                                    <div class="card border border-info shadow-none h-100" style="background-color: #f8fafc; border-radius: 8px;">
                                        <div class="card-header bg-transparent py-2 border-bottom d-flex align-items-center">
                                            <h6 class="mb-0 text-info font-weight-bold"><i class="fa fa-print"></i> Print Return Voucher</h6>
                                        </div>
                                        <div class="card-body py-3 d-flex flex-column justify-content-center">
                                            <div class="row align-items-center">
                                                <label class="col-sm-4 col-form-label font-weight-bold text-secondary">Return Invoice No :</label>
                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="invTextBox" runat="server" CssClass="form-control form-control-sm font-weight-bold" placeholder="Enter or Generated RINV No"></asp:TextBox>
                                                        <asp:Button ID="Button1" runat="server" OnClick="printButton_Click" CssClass="btn btn-sm btn-info text-white font-weight-bold px-3" Text="Print" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <!-- Right: Amount Calculation Summary Card -->
                                <div class="col-lg-6 mb-3">
                                    <div class="card border border-success shadow-none h-100" style="border-radius: 8px;">
                                        <div class="card-header bg-transparent py-2 border-bottom d-flex align-items-center">
                                            <h6 class="mb-0 text-success font-weight-bold"><i class="fa fa-calculator"></i> Return Amount Summary</h6>
                                        </div>
                                        <div class="card-body py-2">
                                            <!-- Hidden textboxes for code-behind compatibility -->
                                            <asp:TextBox ID="disTotalTextBox" runat="server" Visible="false" Text="0.00"></asp:TextBox>
                                            <asp:TextBox ID="pdTextBox" runat="server" Visible="false" Text="0.00"></asp:TextBox>
                                            <asp:TextBox ID="TextBox5" runat="server" Visible="false"></asp:TextBox>

                                            <div class="row mb-2 align-items-center">
                                                <label class="col-sm-5 col-form-label font-weight-bold text-secondary text-sm-end">TP Total :</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="tpTptalTextBox" runat="server" AutoPostBack="True" CssClass="form-control form-control-sm font-weight-bold" ReadOnly="True" Style="background-color: #f8f9fa; text-align: right;"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row mb-2 align-items-center">
                                                <label class="col-sm-5 col-form-label font-weight-bold text-secondary text-sm-end">VAT Total :</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="vatTotalTextBox" runat="server" CssClass="form-control form-control-sm font-weight-bold" ReadOnly="True" Style="background-color: #f8f9fa; text-align: right;"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row mb-2 align-items-center">
                                                <label class="col-sm-5 col-form-label font-weight-bold text-primary text-sm-end">Return Total :</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="grandTotalTextBox" runat="server" CssClass="form-control form-control-sm font-weight-bold text-primary" ReadOnly="True" Style="background-color: #f8f9fa; text-align: right;"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row mb-2 align-items-center">
                                                <label class="col-sm-5 col-form-label font-weight-bold text-danger text-sm-end"><i class="fa fa-minus-circle"></i> Deduction Charge :</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtDeductionCharge" runat="server" AutoPostBack="True" OnTextChanged="txtDeductionCharge_TextChanged" oninput="calcNetReturn();" onkeyup="calcNetReturn();" CssClass="form-control form-control-sm font-weight-bold text-danger" Text="0.00" Style="text-align: right;"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="ftbeDeduction" runat="server" Enabled="True" TargetControlID="txtDeductionCharge" FilterType="Custom" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>

                                            <div class="row mb-1 align-items-center" style="border-top: 1px dashed #ced4da; padding-top: 6px;">
                                                <label class="col-sm-5 col-form-label font-weight-bold text-success text-sm-end" style="font-size: 15px;"><i class="fa fa-check-circle"></i> Net Return Amount :</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtNetReturnAmount" runat="server" CssClass="form-control form-control-sm font-weight-bold text-success border-success" ReadOnly="True" Style="font-size: 16px; background-color: #e8f5e9; text-align: right;" Text="0.00"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <script type="text/javascript">
                                function calcNetReturn() {
                                    var txtGrand = document.querySelector('[id$="grandTotalTextBox"]');
                                    var txtDed = document.querySelector('[id$="txtDeductionCharge"]');
                                    var txtNet = document.querySelector('[id$="txtNetReturnAmount"]');
                                    if (txtGrand && txtDed && txtNet) {
                                        var grand = parseFloat(txtGrand.value.replace(/,/g, '')) || 0;
                                        var ded = parseFloat(txtDed.value.replace(/,/g, '')) || 0;
                                        var net = grand - ded;
                                        if (net < 0) net = 0;
                                        txtNet.value = net.toFixed(2);
                                    }
                                }
                            </script>

                            <!-- Action Buttons Section -->
                            <div class="row mt-4 mb-3">
                                <div class="col-12 text-center">
                                    <asp:LinkButton ID="LinkButton3" CssClass="btn btn-success px-5 py-2 shadow-sm font-weight-bold me-3" runat="server" OnClick="submitButton_Click"
                                        OnClientClick="if (this.dataset.busy === '1') return false; this.dataset.busy = '1'; this.classList.add('disabled'); this.innerHTML = '<i class=&quot;fa fa-spinner fa-spin&quot;></i> &amp;nbsp;Saving...';">
                                        <i class="fa fa-check-circle"></i> &nbsp;Submit Return Information
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton4" CssClass="btn btn-outline-danger px-4 py-2 shadow-sm font-weight-bold" runat="server" OnClick="cancelButton_Click">
                                        <i class="fa fa-refresh"></i> &nbsp;Reset Information
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div> <!-- End card-body -->
                    </div> <!-- End card -->
                </div> <!-- End col -->
            </div> <!-- End row -->
        </div> <!-- End page-content -->
    </div> <!-- End page-wrapper -->
</asp:Content>

