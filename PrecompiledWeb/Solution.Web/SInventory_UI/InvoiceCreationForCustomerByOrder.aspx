<%@ page title="" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="True" inherits="SInventory_UI_InvoiceCreationForCustomer, App_Web_ua4dmbbd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        /* Head Section Wrapper */
        .head-section-wrapper {
            background: linear-gradient(to bottom, #ffffff, #fdfdfd);
            border: 1px solid #e3e8ec;
            border-radius: 12px;
            padding: 24px 20px 10px 20px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.02);
            margin-bottom: 25px;
        }

        /* Form Labels Styling */
        .head-section-wrapper .col-form-label {
            font-size: 0.85rem;
            font-weight: 600;
            color: #344054;
            display: flex;
            align-items: center;
            padding-top: calc(0.375rem + 1px);
            padding-bottom: calc(0.375rem + 1px);
        }

        /* Textbox & Dropdown Styling */
        .head-section-wrapper .form-control, 
        .head-section-wrapper .form-select {
            border-radius: 6px;
            border: 1px solid #d0d5dd;
            padding: 0.375rem 0.75rem;
            font-size: 0.875rem;
            transition: all 0.2s ease;
            box-shadow: 0 1px 2px rgba(16, 24, 40, 0.04);
        }

        /* Focus State */
        .head-section-wrapper .form-control:focus, 
        .head-section-wrapper .form-select:focus {
            border-color: #84c9fb;
            box-shadow: 0 0 0 4px #e1f5fe;
            outline: 0;
        }

        /* ReadOnly Fields Styling */
        .head-section-wrapper .form-control[readonly] {
            background-color: #f2f4f7;
            border-color: #eaecf0;
            color: #475467;
            cursor: not-allowed;
            box-shadow: none;
        }

        /* Form Group Row spacing */
        .head-section-wrapper .form-group.row {
            margin-bottom: 1.25rem;
        }

        /* Adjust textarea heights */
        .head-section-wrapper textarea.form-control {
            resize: none;
            line-height: 1.4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">




    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Invoice Creation</div>

                <div class="ms-auto">
                    <div class="btn-group">

                        <asp:LinkButton ID="viewLinkButton" class="btn btn-sm btn-sm btn-outline-info"
                            OnClick="backLinkButton_Click" runat="server"><i class="fa fa-backward"></i>&nbsp Back To List</asp:LinkButton>



                    </div>
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
                                    <div class="row">




                                        <div class="card-body">
                                            <br />
                                            <div class="head-section-wrapper">



                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-2 col-form-label">Customer Code:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="custCodeTextBox" runat="server" AutoPostBack="True" ReadOnly="True"
                                                        CssClass="form-control form-control-sm " OnTextChanged="custCodeTextBox_TextChanged"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Customer Name	:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="custNameTextBox" runat="server" CssClass="form-control form-control-sm " ReadOnly="True"
                                                        AutoPostBack="True" OnTextChanged="custNameTextBox_TextChanged"></asp:TextBox>

                                                    <asp:AutoCompleteExtender ID="custNameTextBox_AutoCompleteExtender" runat="server"
                                                        DelimiterCharacters="" EnableCaching="True"
                                                        Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                                        ServiceMethod="GetCustomer" ServicePath="SInventoryWebService.asmx" TargetControlID="custNameTextBox"
                                                        UseContextKey="True"
                                                        CompletionListCssClass="autocomplete_completionListElement"
                                                        CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        ShowOnlyCurrentWordInCompletionListItem="True">
                                                    </asp:AutoCompleteExtender>


                                                </div>


                                                <label for="mainName" class="col-sm-2 col-form-label">Customer Address:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="custAddressTextBox" runat="server" CssClass="form-control "
                                                        ReadOnly="True" TextMode="MultiLine" Rows="2"></asp:TextBox>




                                                </div>

                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-2 col-form-label">Salse Center:</label>

                                                <div class="col-sm-2">

                                                    <asp:TextBox ID="comUnitNameTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">DA Name: <span class="text-c-red">*</span></label>

                                                <div class="col-sm-6">
                                                    <asp:DropDownList runat="server" class="form-select form-select-sm mb-3 mySelect2 " ID="ddlDAName"></asp:DropDownList>
                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label" style="display:none;">MIO Code:</label>

                                                <div class="col-sm-2" style="display:none;">
                                                    <asp:TextBox ID="miaCodeTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label" style="display:none;">MIO Name:</label>

                                                <div class="col-sm-2" style="display:none;">
                                                    <asp:TextBox ID="miaNameTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>

                                                </div>



                                            </div>

                                            <div class="form-group row" style="display:none;">
                                                <label for="mainName" class="col-sm-2 col-form-label">FE Name:</label>

                                                <div class="col-sm-2">

                                                    <asp:TextBox ID="districtNameTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Territory Name:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="areaNameTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Merket:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="marketNameTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>

                                                </div>



                                            </div>



                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-2 col-form-label">Invoice Date:</label>

                                                <div class="col-sm-2">

                                                    <asp:TextBox ID="invDateTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>

                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Order No:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="orderNoTextBox" runat="server" CssClass="form-control form-control-sm " ReadOnly="True"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Order Date:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="orderDateTextBox" runat="server" CssClass="form-control form-control-sm " ReadOnly="True"></asp:TextBox>

                                                </div>



                                            </div>


                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-2 col-form-label" style="display:none;">Customer Category:</label>

                                                <div class="col-sm-2" style="display:none;">

                                                    <asp:TextBox ID="custCategoryTextBox" runat="server" CssClass="form-control form-control-sm "
                                                        ReadOnly="True"></asp:TextBox>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label" style="display:none;">Payment Type:</label>

                                                <div class="col-sm-2" style="display:none;">
                                                    <asp:DropDownList ID="payTypeDDL" runat="server" CssClass="form-control form-control-sm ">
                                                    </asp:DropDownList>


                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Remarks:</label>

                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="remarksTextBox" runat="server" CssClass="form-control form-control-sm " TextMode="MultiLine" Rows="2"></asp:TextBox>

                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Customer Delivery Address:</label>

                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="custDeliveryAddressTextBox" runat="server" CssClass="form-control form-control-sm " TextMode="MultiLine" Rows="2"></asp:TextBox>

                                                </div>

                                            </div>


                                            <div class="form-group row" style="display:none;">
                                                <label for="mainName" class="col-sm-2 col-form-label" style="display:none;">Provider Type:</label>

                                                <div class="col-sm-2" style="display:none;">

                                                    <asp:TextBox ID="cusTypeTextBox" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>

                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Delivery Person Name:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="deliverypersonNameTextBox" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>

                                                </div>

                                                <label for="mainName" class="col-sm-2 col-form-label">Delivery Person Mob. No:</label>

                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="deliverypersonMobileTextBox" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>

                                                </div>



                                            </div>
                                            </div>

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


                                            <div id="divOrderRemarksAlert" runat="server" class="alert alert-warning border-0 shadow-sm p-3 mb-3" visible="false" style="background-color: #fff3cd; border-left: 5px solid #ffc107 !important;">
                                                <span style="font-weight: bold; color: #856404; font-size: 15px;"><i class="fa fa-exclamation-circle text-warning font-weight-bold"></i> Special Order Remarks:</span>
                                                <asp:Label ID="lblOrderRemarksSpecial" runat="server" Font-Bold="true" ForeColor="#d9534f" style="font-size: 15px; margin-left: 8px;"></asp:Label>
                                            </div>

                                            <div class="row">
                                                <div class="table-responsive" id="MainGradeDiv">
                                                    <asp:GridView ID="gridLineItemGridView" runat="server"
                                                        AutoGenerateColumns="False" CssClass="table  blueTable" OnPreRender="gv_DocumentUpload_PreRender" DataKeyNames="ISGiftProduct">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="SL" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="slLabel" runat="server" Text='<%# Eval("SL")%>'></asp:Label>
                                                                    <asp:ImageButton ID="addImageButton" runat="server"
                                                                        ImageUrl="~/images/lineAdd.png" OnClick="addImageButton_Click" />
                                                                    <asp:ImageButton ID="removeImageButton" runat="server"
                                                                        ImageUrl="~/images/lineDelete.png" OnClick="removeImageButton_Click" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="codeTextBox" runat="server" Text='<%# Eval("ProductCode")%>'
                                                                        CssClass="form-control form-control-sm " AutoPostBack="True"
                                                                        OnTextChanged="codeTextBox_TextChanged"></asp:TextBox>
                                                                    <asp:HiddenField ID="orderdetailIdHiddenField" runat="server" Value='<%# Eval("OrderDetailsId")%>' />
                                                                    <asp:HiddenField ID="CampaignTypeHiddenField" runat="server" Value='<%# Eval("CampaignType")%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Name">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="nameTextBox" runat="server" ReadOnly="true" TextMode="MultiLine" CssClass="form-control-sm "
                                                                        Text='<%# Eval("ProductName")%>' AutoPostBack="True"
                                                                        OnTextChanged="nameTextBox_TextChanged"></asp:TextBox>
                                                                    <asp:AutoCompleteExtender ID="nameTextBox_AutoCompleteExtender" runat="server"
                                                                        DelimiterCharacters="" EnableCaching="True"
                                                                        Enabled="True" MinimumPrefixLength="1" CompletionSetCount="10"
                                                                        ServiceMethod="GetProduct2" ServicePath="SInventoryWebService.asmx" TargetControlID="nameTextBox"
                                                                        UseContextKey="True"
                                                                        CompletionListCssClass="autocomplete_completionListElement"
                                                                        CompletionListItemCssClass="autocomplete_listItem"
                                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                        ShowOnlyCurrentWordInCompletionListItem="True">
                                                                    </asp:AutoCompleteExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CStock">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="currentStockTextBox" runat="server"
                                                                        CssClass="form-control form-control-sm " Text='<%# Eval("StockQty")%>' ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="fcurrentStockTextBox" runat="server"
                                                                        TargetControlID="currentStockTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="UP">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="unitPriceTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("UnitPrice")%>' ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="funitPriceTextBox" runat="server"
                                                                        TargetControlID="unitPriceTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="UVAT">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="upVatTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("UnitVAT")%>' ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="fupVatTextBox" runat="server"
                                                                        TargetControlID="upVatTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="qtyTextBox" runat="server" CssClass="form-control form-control-sm " ReadOnly="True"
                                                                        Text='<%# Eval("Quantity")%>' AutoPostBack="True"
                                                                        OnTextChanged="qtyTextBox_TextChanged"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="fqtyTextBox" runat="server"
                                                                        TargetControlID="qtyTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TP">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="tpTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("TotalPrice")%>' ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="ftpTextBox" runat="server"
                                                                        TargetControlID="tpTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DP">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="dpTextBox" runat="server" CssClass="form-control form-control-sm text-end"
                                                                        Text='<%# Eval("DiscountPercentage")%>' AutoPostBack="True" OnTextChanged="dpTextBox_TextChanged"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="fdpTextBox" runat="server"
                                                                        TargetControlID="dpTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="DAmt">
                                                                 <ItemTemplate>
                                                                     <asp:TextBox ID="dpAmtTextBox" runat="server" CssClass="form-control form-control-sm text-end"
                                                                         Text='<%# Eval("DiscountAmount")%>' AutoPostBack="True" OnTextChanged="dpAmtTextBox_TextChanged"></asp:TextBox>
                                                                     <asp:FilteredTextBoxExtender ID="fdpAmtTextBox" runat="server"
                                                                         TargetControlID="dpAmtTextBox"
                                                                         FilterType="Custom, Numbers"
                                                                         ValidChars="." />
                                                                 </ItemTemplate>
                                                             </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SD">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sdTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("IsCampaignProduct")%>' ReadOnly="True"></asp:TextBox>
                                                                    <%-- <asp:FilteredTextBoxExtender ID="fsdTextBox" runat="server"
                                                        TargetControlID="sdTextBox"         
                                                        FilterType="Custom, Numbers"
                                                        ValidChars="." />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TPVAT">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="tpVatTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("VAT")%>' ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="ftpVatTextBox" runat="server"
                                                                        TargetControlID="tpVatTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />

                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="NP">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="npTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("NetPrice")%>' ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="fnpTextBox" runat="server"
                                                                        TargetControlID="npTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="BQty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="bQtyTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("ISGiftProduct")%>' AutoPostBack="True"
                                                                        OnTextChanged="bQtyTextBox_TextChanged" ReadOnly="True"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="fbQtyTextBox" runat="server"
                                                                        TargetControlID="bQtyTextBox"
                                                                        FilterType="Custom, Numbers"
                                                                        ValidChars="." />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="TQty">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="tQtyTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                        Text='<%# Eval("TotalQty")%>' ReadOnly="True"></asp:TextBox>
                                                                    <%--   <asp:FilteredTextBoxExtender ID="ftQtyTextBox" runat="server"
                                                        TargetControlID="tQtyTextBox"         
                                                        FilterType="Custom, Numbers"  ReadOnly="True"
                                                        ValidChars="." />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="A.M">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="amTextBox" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>
                                                                    <%--   <asp:FilteredTextBoxExtender ID="ftQtyTextBox" runat="server"
                                                        TargetControlID="tQtyTextBox"         
                                                        FilterType="Custom, Numbers"  ReadOnly="True"
                                                        ValidChars="." />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--  <asp:TemplateField HeaderText="Campaign Type">
                                        <ItemTemplate>
                                            <asp:TextBox ID="CampaignTypeHiddenField" runat="server" CssClass="form-control form-control-sm " 
                                                Text= <%# Eval("CampaignType")%> ></asp:TextBox>
                                      
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="add" runat="server" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="addImageButton2" runat="server"
                                                                        ImageUrl="~/images/lineAdd.png" OnClick="addImageButton_Click" />
                                                                    <%-- <asp:ImageButton ID="removeImageButton" runat="server" 
                                                ImageUrl="~/images/lineDelete.png" onclick="removeImageButton_Click" />--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>

                                                </div>
                                            </div>

                                            <br />
                                            <div class="row">
                                                <div class="col-4">&nbsp;</div>
                                                <div class="col-6">
                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">TP Total:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="tpTptalTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>



                                                        </div>

                                                    </div>


                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Discount Total:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="disTotalTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>


                                                        </div>

                                                    </div>


                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Special Discount:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="pdTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>


                                                        </div>

                                                    </div>


                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">VAT Total:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="vatTotalTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>


                                                        </div>

                                                    </div>

                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Delivery Charge:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="deliveryChargeTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                AutoPostBack="True" OnTextChanged="deliveryChargeTextBox_TextChanged"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="fdeliveryChargeTextBox" runat="server"
                                                                TargetControlID="deliveryChargeTextBox" FilterType="Custom, Numbers"
                                                                ValidChars="." />

                                                        </div>

                                                    </div>

                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Grand Total:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="grandTotalTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>



                                                        </div>

                                                    </div>


                                                    <div class="form-group row" >
                                                        <label for="mainName" class="col-sm-3 col-form-label">Credit Note Amount:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="crAmountTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>



                                                        </div>

                                                    </div>



                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Receivable Amount:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="rcvAmountTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="True"></asp:TextBox>



                                                        </div>

                                                    </div>



                                                    <div class="form-group row" runat="server" Visible="False">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Adjust Return Invoice No:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="adjustInvoiceNoTextBox" runat="server" CssClass="form-control form-control-sm "
                                                                ReadOnly="false"></asp:TextBox>



                                                        </div>

                                                    </div>

                                                </div>


                                            </div>



                                            <br />
                                            <div class="row">
                                                <div class="col-4">&nbsp;</div>
                                                <div class="col-6">

                                                    <div class="form-group row">
                                                        <label for="exampleInputUsername2" class="col-sm-3 col-form-label"></label>
                                                        <div class="col-sm-8">

                                                            <asp:LinkButton ID="submitButton" CssClass="btn btn-sm btn-primary mb-2" runat="server" OnClientClick="return sweetAlertConfirm_Submit(this);" OnClick="saveButton_Click" Style="background-color: #00bcd4; color: #fff;"> <i class="fa fa-check-square"></i>&nbsp; Invoice Generate</asp:LinkButton>

                                                            <%--OnClientClick="return sweetAlertConfirm_Submit(this);"--%>
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="col-2">&nbsp;</div>
                                            </div>

                                            <br />

                                            <div class="row">
                                                <div class="col-3">&nbsp;</div>
                                                <div class="col-8">
                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Print Inv No:</label>

                                                        <div class="col-sm-5">

                                                            <asp:TextBox ID="invTextBox" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>



                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:LinkButton ID="printButton" runat="server" class="btn btn-sm btn-info" OnClick="printButton_Click"><i class="fa fa-print"></i>&nbsp; Print</asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>


                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="False">
                                <ContentTemplate>
                                    <div>
                                        <table width="100%" class="TableWorkArea">
                                            <tr>
                                                <td colspan="6" class="TableHeading">Proforma Invoice Creation</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">
                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="InvoiceCreationByOrder.aspx">Back to List</asp:HyperLink>
                                                </td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td width="13%" class="TDLeft">Customer Code</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">CustomerName</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Customer Address</td>
                                                <td width="20%" class="TDRight"></td>
                                            </tr>
                                            <tr>
                                                <td width="13%" class="TDLeft">Salse Center</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">MIO Code</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">MIO Name</td>
                                                <td width="20%" class="TDRight"></td>
                                            </tr>
                                            <tr>
                                                <td width="13%" class="TDLeft">FE Name
                                                </td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Territory Name</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Merket</td>
                                                <td width="20%" class="TDRight"></td>
                                            </tr>

                                            <tr>
                                                <td width="13%" class="TDLeft">Invoice Date</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Order No</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Order Date</td>
                                                <td width="20%" class="TDRight">

                                                    <%-- <asp:ImageButton runat="server" AlternateText="Click to show calendar" ImageUrl="~/Images/Calendar_scheduleHS.png"
                                TabIndex="4" ID="imgorderDate"></asp:ImageButton>
                            <asp:CalendarExtender ID="orderDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="orderDateTextBox"
                                PopupButtonID="imgorderDate">
                            </asp:CalendarExtender>--%>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td width="13%" class="TDLeft">Customer Category
                                                </td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Payment Type</td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft">Remarks</td>
                                                <td width="20%" class="TDRight"></td>
                                            </tr>
                                            <tr>


                                                <td width="13%" class="TDLeft">Customer Type
                                                </td>
                                                <td width="20%" class="TDRight"></td>

                                                <td width="13%" class="TDLeft">Delivery Person Name  
                                                </td>
                                                <td width="20%" class="TDRight"></td>


                                                <td width="13%" class="TDLeft">Delivery Person Mob.No   </td>
                                                <td width="20%" class="TDRight"></td>
                                                <td width="13%" class="TDLeft"></td>
                                                <td width="20%" class="TDRight"></td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">
                                                    <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                                                </td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%" colspan="6">

                                                    <br />

                                                </td>

                                            </tr>

                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">TP Total</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Discount Total</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Special Discount</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">VAT Total</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>

                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Grand Total</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Credit Amount</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Receivable Amount</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Adjust Return Invoice No</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">
                                                    <asp:Label ID="warningLabel" runat="server" Font-Bold="True" Font-Italic="True"
                                                        Font-Size="Small" ForeColor="#FF3300"></asp:Label>
                                                </td>
                                                <td class="TDRight" width="20%">&nbsp;
                                                </td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                        <ContentTemplate>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
                                                        DisplayAfter="0" DynamicLayout="True">
                                                        <ProgressTemplate>
                                                            <center>
                                                                <asp:Image ID="Img2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                                            </center>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>

                                                </td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">Print InvNo</td>
                                                <td class="TDRight" width="20%"></td>
                                                <td class="TDLeft" width="13%"></td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td class="TDLeft" width="13%">
                                                    <asp:HiddenField ID="hdCustomerMasterId" runat="server" />
                                                    <asp:HiddenField ID="hdComUnitId" runat="server" />

                                                </td>
                                                <td class="TDRight" width="20%">
                                                    <asp:HiddenField ID="hdMiaId" runat="server" />
                                                    <asp:HiddenField ID="orderHiddenField" runat="server" />
                                                </td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                                <td class="TDLeft" width="13%">&nbsp;</td>
                                                <td class="TDRight" width="20%">&nbsp;</td>
                                            </tr>
                                        </table>
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


