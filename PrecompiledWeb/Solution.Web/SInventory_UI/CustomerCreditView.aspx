<%@ page title="Customer Credit List" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_CustomerCreditView, App_Web_jn5nt2is" %>

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

        .disabled, .aspNetDisabled {
            cursor: not-allowed !important;
            pointer-events: auto !important;
            opacity: 0.65;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Customer Credit List</div>
                <div class="ms-auto">
                    <div class="btn-group">
                        <asp:LinkButton ID="newEntryLinkButton" CssClass="btn btn-sm btn-outline-info" runat="server" OnClick="newEntryLinkButton_Click">
                            <i class="fa fa-plus" aria-hidden="true"></i>&nbsp;New Entry
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="divWaiting">
                                                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <div class="row">
                                        <div class="col-1"></div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Company Name</label>
                                                <div class="col-sm-8">
                                                    <asp:DropDownList ID="companyDropDownList" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label class="col-sm-4 col-form-label">Customer Code</label>
                                                <div class="col-sm-8">
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
                                                    <asp:HiddenField ID="customerIdHiddenField" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-1"></div>
                                    </div>

                                    <div class="row mb-3">
                                        <div class="col-4"></div>
                                        <div class="col-4 text-center">
                                            <asp:LinkButton ID="searchButton" runat="server" CssClass="btn btnMyDesignSearch btn-sm" OnClick="searchButton_Click">
                                                <i class="fa fa-search"></i>&nbsp;Search
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="resetButton" runat="server" CssClass="btn btnMyDesignReset btn-sm" OnClick="resetButton_Click">
                                                <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp;Reset
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-4"></div>
                                    </div>

                                    <div class="table-responsive" id="MainGradeDiv">
                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            DataKeyNames="CreditLimitId"
                                            CssClass="table table-striped table-bordered"
                                            OnRowCommand="loadGridView_RowCommand"
                                            OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>
                                                <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                                                <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" />
                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                <asp:BoundField DataField="LimitAmount" HeaderText="Limit Amount" DataFormatString="{0:N2}" />
                                                <asp:BoundField DataField="Validity" HeaderText="Validity (Day)" />
                                                <asp:BoundField DataField="ActionStatus" HeaderText="Status" />
                                                <asp:BoundField DataField="DaysRemaining" HeaderText="Overdue Since (Days)" />
                                                <asp:BoundField DataField="InvoiceNo" HeaderText="Oldest Due Invoice No" />
                                                <asp:BoundField DataField="InvoiceDate" HeaderText="Oldest Due Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:BoundField DataField="ActiveStatus" HeaderText="Active Status" />
                                                <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" />
                                                <asp:BoundField DataField="ApprovedDate" HeaderText="Approved Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="editButton" runat="server" CssClass="btn-warning btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData">
                                                            <i class='bx bxs-edit' aria-hidden='true'></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="deleteButton" runat="server" 
                                                            CssClass='<%# Eval("ActiveStatus").ToString() == "Active" ? "btn-danger btn-sm mb-1 mb-md-0" : "btn-danger btn-sm mb-1 mb-md-0 disabled" %>'
                                                            Enabled='<%# Eval("ActiveStatus").ToString() == "Active" %>'
                                                            title='<%# Eval("ActiveStatus").ToString() == "Active" ? "Deactivate" : "Already Inactive" %>'
                                                            OnClientClick="return sweetAlertConfirm_Delete(this);"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="DeleteData">
                                                            <i class='bx bxs-trash' aria-hidden='true'></i>
                                                        </asp:LinkButton>
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
        function pageLoad() {
            $('.mySelect2').select2({
                theme: 'bootstrap4',
                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                placeholder: $(this).data('placeholder'),
                allowClear: Boolean($(this).data('allow-clear'))
            });
        }

        $(document).ready(function () {
            var table = $('#ContentPlaceHolder1_loadGridView').DataTable({
                "bInfo": true,
                "bFilter": true,
                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                pageLength: 10,
                dom: 'lBfrtip',
                buttons: ['copy', 'excel', 'pdf', 'print']
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        if ($.fn.DataTable.isDataTable('#ContentPlaceHolder1_loadGridView')) {
                            $('#ContentPlaceHolder1_loadGridView').DataTable().destroy();
                        }
                        $('#ContentPlaceHolder1_loadGridView').DataTable({
                            "bInfo": true,
                            "bFilter": true,
                            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                            pageLength: 10,
                            dom: 'lBfrtip',
                            buttons: ['copy', 'excel', 'pdf', 'print']
                        });
                    }
                });
            }
        });
    </script>
</asp:Content>
