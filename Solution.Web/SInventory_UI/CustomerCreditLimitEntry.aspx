<%@ Page Title="Customer Credit Entry" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="CustomerCreditLimitEntry.aspx.cs" Inherits="SInventory_UI_CustomerCreditLimitEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .autocomplete_completionListElement {
            margin: 0 !important;
            background-color: #fff;
            color: windowtext;
            border: 1px solid buttonshadow;
            cursor: default;
            overflow: auto;
            font-family: Calibri;
            font-size: 12px;
            text-align: left;
            list-style-type: none;
            padding-left: 0;
            max-height: 350px;
            width: 40% !important;
        }

        .autocomplete_highlightedListItem {
            background-color: #ffe082;
            color: #000;
            padding: 1px;
        }

        .autocomplete_listItem {
            background-color: #fff;
            color: #0d6efd;
            padding: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Customer Credit Entry</div>
                        <div class="ms-auto">
                            <div class="btn-group">
                                <asp:LinkButton ID="viewLinkButton" runat="server" CssClass="btn btn-sm btn-outline-info" OnClick="viewLinkButton_Click">
                                    <i class="fa fa-list"></i>&nbsp;Back to List
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="divWaiting">
                                                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <asp:HiddenField ID="creditHiddenField" runat="server" />
                                    <asp:HiddenField ID="customeridHiddenField" runat="server" />

                                    <div class="row">
                                        <div class="col-2">&nbsp;</div>
                                        <div class="col-8">
                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Company Name</label>
                                                <div class="col-sm-7">
                                                    <asp:DropDownList ID="companyDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Customer Name</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="customerTextBox" runat="server" CssClass="form-control form-control-sm" AutoPostBack="True" OnTextChanged="customerTextBox_TextChanged"></asp:TextBox>
                                                    <asp:AutoCompleteExtender ID="customerTextBox_AutoCompleteExtender" runat="server"
                                                        CompletionListCssClass="autocomplete_completionListElement"
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        CompletionListItemCssClass="autocomplete_listItem"
                                                        CompletionSetCount="10"
                                                        DelimiterCharacters=""
                                                        EnableCaching="true"
                                                        Enabled="True"
                                                        MinimumPrefixLength="1"
                                                        ServiceMethod="GetCustomernew"
                                                        ServicePath="SInventoryWebService.asmx"
                                                        ShowOnlyCurrentWordInCompletionListItem="true"
                                                        TargetControlID="customerTextBox"
                                                        UseContextKey="True">
                                                    </asp:AutoCompleteExtender>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Limit Amount</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="limitAmntTextBox" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="limitAmountFilteredTextBoxExtender" runat="server"
                                                        TargetControlID="limitAmntTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Validity (Days)</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="limitDaysTextBox" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="limitDaysFilteredTextBoxExtender" runat="server"
                                                        TargetControlID="limitDaysTextBox" FilterType="Numbers">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Is Active</label>
                                                <div class="col-sm-7 pt-2">
                                                    <asp:CheckBox ID="isActiveCheckBox" runat="server" Checked="true" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">&nbsp;</div>
                                    </div>

                                    <div class="row mt-4">
                                        <div class="col-2">&nbsp;</div>
                                        <div class="col-8">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label"></label>
                                                <div class="col-sm-8">
                                                    <asp:LinkButton ID="saveButton" runat="server" CssClass="btn btnMyDesignSearch btn-sm" OnClick="saveButton_Click">
                                                        <i class="fa fa-check"></i>&nbsp;Save
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="updateButton" runat="server" CssClass="btn btnMyDesignSearch btn-sm" Visible="false" OnClick="updateButton_Click">
                                                        <i class="fa fa-check"></i>&nbsp;Update
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="resetButton" runat="server" CssClass="btn btnMyDesignReset btn-sm" OnClick="resetButton_Click">
                                                        <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp;Reset
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">&nbsp;</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function pageLoad() {
            $('.mySelect2').select2({
                theme: 'bootstrap4',
                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                placeholder: $(this).data('placeholder'),
                allowClear: Boolean($(this).data('allow-clear'))
            });
        }
    </script>
</asp:Content>
