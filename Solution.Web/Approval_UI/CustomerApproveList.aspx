<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="CustomerApproveList.aspx.cs" Inherits="Approval_UI_CustomerApproveList" %>

<%@ Register Src="../SInventory_UI/IVMarketStructureInvoSearch.ascx" TagPrefix="uc1" TagName="IVMarketStructure" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Customer Approval List</div>

                <div class="ms-auto">
                    <div class="btn-group">

                        <asp:LinkButton ID="EmpCetegoryAddImageButton" Visible="false" CssClass="btn btn-sm btn-outline-info " runat="server" OnClick="EmpCetegoryAddImageButton_Click"><i class="fa fa-plus" aria-hidden="true"></i> New Entry </asp:LinkButton>


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
                                    <script type="text/javascript">


                                        function pageLoad() {


                                            $('.datepicker').pickadate({
                                                selectMonths: true,
                                                selectYears: true
                                            })
                                            $('.multiple-select').select2({
                                                includeSelectAllOption: true,
                                                theme: 'bootstrap4',
                                                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                                placeholder: $(this).data('placeholder'),
                                                allowClear: Boolean($(this).data('allow-clear')),
                                            });
                                            $('.mySelect2').select2({
                                                theme: 'bootstrap4',
                                                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                                placeholder: $(this).data('placeholder'),
                                                allowClear: Boolean($(this).data('allow-clear')),
                                            });
                                        }
                                    </script>


                                    <div style="padding: 2px!important"></div>

                                    <div class="row" runat="server" visible="false">

                                        <div class="col-6">

                                            <div class="form-group row">
                                                <label for="GroupSelect" class="col-sm-4 col-form-label">Distribution Center:  </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList CssClass="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlDistributionCenter"></asp:DropDownList>


                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>



                                    <asp:HiddenField ID="hfEmpTerrId" runat="server" />
                                    <asp:HiddenField ID="hfEmpAreaId" runat="server" />
                                    <asp:HiddenField ID="hfEmpRegionId" runat="server" />
                                    <asp:HiddenField ID="hfEmpGroupId" runat="server" />


                                    <div style="padding: 2px!important"></div>

                                    <div class="row" runat="server" visible="false">

                                        <div class="col-6">



                                            <uc1:IVMarketStructure runat="server" ID="IVMarketStructure" />



                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Status: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlStatus">
                                                            <asp:ListItem Value="">Select Status</asp:ListItem>
                                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                        </asp:DropDownList>
                                                        </span>
 

                                                    </div>
                                                </div>

                                            </div>
                                        </div>


                                        <div class="col-6">

                                            <div class="form-group row" runat="server">
                                                <label for="GroupSelect" class="col-sm-3 col-form-label">DC:  </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList CssClass="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="DropDownList1"></asp:DropDownList>


                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Provider Type: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlProgramType"></asp:DropDownList>
                                                        </span>
 

                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Customer  Type: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlChemisType"></asp:DropDownList>
                                                        </span>
 

                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Approval Status: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlApprovalStatus"></asp:DropDownList>
                                                        </span>
 

                                                    </div>
                                                </div>

                                            </div>


                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Create From Date: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="frmDate" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>
                                                        <span id="v-frmDate" class="invalid-tooltip fade hide" data-delay="2000"></span>



                                                    </div>
                                                </div>

                                            </div>
                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Create To Date: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox runat="server" ID="toDate" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>
                                                        <span id="v-toDate" class="invalid-tooltip fade hide" data-delay="2000"></span>


                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Customer: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="custNameTextBox" runat="server" CssClass="form-control form-control-sm mb-3 "
                                                            AutoPostBack="True" OnTextChanged="custNameTextBox_TextChanged"></asp:TextBox>
                                                        <asp:AutoCompleteExtender
                                                            ID="at_txt_JobCirculation"
                                                            TargetControlID="custNameTextBox"
                                                            runat="server"
                                                            ServiceMethod="GetCustomer_ALL_new"
                                                            ServicePath="SInventoryWebService.asmx"
                                                            MinimumPrefixLength="1"
                                                            CompletionInterval="10"
                                                            EnableCaching="false"
                                                            CompletionSetCount="1"
                                                            FirstRowSelected="false" CompletionListCssClass="autocomplete_completionListElement"
                                                            CompletionListItemCssClass="autocomplete_listItem"
                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            ShowOnlyCurrentWordInCompletionListItem="true">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:HiddenField ID="hfCustomerId" runat="server" />


                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>



                                    <div style="padding-top: 16px;"></div>
                                    <div class="row" runat="server" visible="false">
                                        <div class="col-md-5">
                                        </div>
                                        <div class="col-md-4" style="align-content: center">
                                            <asp:LinkButton runat="server" ID="btnSearch" class="btn btnMyDesignSearch   btn-sm " OnClick="btnSearch_Click">  <i class="fa fa-search-plus"></i>&nbsp; Search</asp:LinkButton>


                                            <asp:LinkButton runat="server" class="btn btnMyDesignReset   btn-sm" ID="resetBtn" OnClick="resetBtn_Click"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </asp:LinkButton>
                                        </div>
                                    </div>

                                    <hr class="my-3" />
                                    <div class="d-flex justify-content-between align-items-center mb-3">
                                        <h5 class="mb-0"><i class="bx bx-list-ul"></i> Customer List</h5>
                                        <div>
                                            <asp:LinkButton ID="btnBulkApprove" runat="server" CssClass="btn btn-sm btn-success" Style="margin-right: 15px !important;" OnClick="btnBulkApprove_Click" OnClientClick="return confirm('Are you sure you want to approve all selected customers?');">
                                                <i class="fa fa-check"></i> Approve Selected
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnBulkReject" runat="server" CssClass="btn btn-sm btn-danger" OnClick="btnBulkReject_Click" OnClientClick="return confirm('Are you sure you want to reject all selected customers?');">
                                                <i class="bx bx-x"></i> Reject Selected
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <hr class="my-3" />

                                    <div class="table-responsive" id="MainGradeDiv" style="height: 600px;">
                                        <style>
                                            .bg-color-red {
                                                background-color: red;
                                            }
                                        </style>


                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            DataKeyNames="CustomerMasterId,CustomerApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep,ExistenceStatus" OnRowDataBound="loadGridView_RowDataBound" OnRowCommand="loadGridView_RowCommand"
                                            CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="SelectAllRows(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CustomerCode" HeaderText="Customer Code" />
                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person Name" />

                                                <asp:BoundField DataField="RegionCode" HeaderText="Zone" Visible="False" />

                                                <asp:BoundField DataField="AreaCode" HeaderText="Area" Visible="False"/>


                                                <asp:BoundField DataField="TerritoryCode" HeaderText="Territory" Visible="False" />
                                                <asp:BoundField DataField="MarketCode" HeaderText="Market Code" Visible="False" />
                                                <asp:BoundField DataField="MarketName" HeaderText="Market Name" Visible="False" />



                                                <asp:BoundField DataField="CustomerType" HeaderText="Customer Type" />

                                                <asp:BoundField DataField="SMCType" HeaderText="Pharma Platform" Visible="False" />

                                                <asp:BoundField DataField="ProgramTypeName" HeaderText="Provider Type" Visible="False" />
                                                <asp:BoundField DataField="CellNo" HeaderText="Mobile NO" />
                                                <asp:BoundField DataField="Address" HeaderText="Address" />

                                                <asp:BoundField DataField="DistributionRouteName" HeaderText="Distribution RouteName" Visible="False"/>

                                                <asp:BoundField DataField="ApprovalStatusWeb" HeaderText="Approval Status" />
                                                <asp:BoundField DataField="WaitingForRole" HeaderText="Waiting For" />
                                                <asp:BoundField DataField="EmpMasterCode" HeaderText="Entry By" />
                                                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" />


                                                <%--<asp:BoundField DataField="WaitingForRole" HeaderText="Waiting For" />--%>

                                                <%--  <asp:BoundField DataField="DesigName" HeaderText="Designation" />
                                    <asp:BoundField DataField="DegreeName" HeaderText="Degree" />
                                    <asp:BoundField DataField="DoctorSpeciality" HeaderText="Doctor Speciality" />--%>

                                                <asp:TemplateField HeaderText="Actions">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hfCustomerMasterId" Value='<%#Eval("CustomerMasterId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfFromEmpId" Value='<%#Eval("FromEmpId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfToEmpId" Value='<%#Eval("ToEmpId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfStep" Value='<%#Eval("Step")%>' />
                                                        <asp:HiddenField runat="server" ID="hfRoleTypeId" Value='<%#Eval("RoleTypeId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfCustomerApprovalId" Value='<%#Eval("CustomerApprovalId")%>' />

                                                        <asp:HiddenField runat="server" ID="hfToRoleTypeId" Value='<%#Eval("ToRoleTypeId")%>' />

                                                        <asp:Label runat="server" ID="lbMsg" />
                                                        <asp:LinkButton ID="lbEdit" runat="server" class="btn-warning  btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData"><i class='bx bxs-edit' aria-hidden='true'></i></asp:LinkButton>

                                                        <asp:LinkButton ID="lbApprove" runat="server" class="btn-info  btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="ApproveData"><i class='fa fa-check' aria-hidden='true'></i> </asp:LinkButton>


                                                        <asp:LinkButton ID="lbReject" runat="server" class="btn-danger  btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="RejectData">  <i class='fadeIn animated bx bx-x' aria-hidden='true'></i> </asp:LinkButton>

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


    <script>
        function SelectAllRows(headerChk) {
            var table = $('#ContentPlaceHolder1_loadGridView').DataTable();
            var rows = table.rows({ 'search': 'applied' }).nodes();
            $('input[type="checkbox"]', rows).prop('checked', headerChk.checked);
        }

        $(document).ready(function () {

            var table = $('#ContentPlaceHolder1_loadGridView').DataTable(
                {
                    "bInfo": true,
                    "bFilter": true,
                    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    pageLength: 10,
                    dom: 'lBfrtip',


                    buttons: ['copy', 'excel', 'pdf', 'print']
                }
            );

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        table = $('#ContentPlaceHolder1_loadGridView').DataTable(
                            {
                                "bInfo": true,
                                "bFilter": true,
                                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                                pageLength: 10,
                                dom: 'lBfrtip',


                                buttons: ['copy', 'excel', 'pdf', 'print']


                            }
                        );
                    }
                });
            };


            table.columns().every(function () {
                var that = this;


            });
        });


    </script>
</asp:Content>

