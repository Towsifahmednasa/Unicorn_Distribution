<%@ page title="DCR List" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="DoctorVisit_UI_DCRList, App_Web_3mg5hlwf" %>

<%@ Register Src="~/DoctorVisit_UI/IVMasterStructureForDCR.ascx" TagPrefix="uc1" TagName="IVMarketStructure" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #MainGradeDivs {
            overflow-x: auto;
        }

        #ContentPlaceHolder1_loadGridView {
            min-width: 1600px;
        }

        #ContentPlaceHolder1_loadGridView th,
        #ContentPlaceHolder1_loadGridView td {
            white-space: nowrap;
        }

        #ContentPlaceHolder1_loadGridView thead th {
            position: sticky;
            top: 0;
            z-index: 2;
            background: #ffffff;
        }

        #ContentPlaceHolder1_loadGridView th:nth-child(1),
        #ContentPlaceHolder1_loadGridView td:nth-child(1) {
            position: sticky;
            left: 0;
            z-index: 3;
            background: #f2f7ff;
            min-width: 45px;
            width: 45px;
            box-shadow: inset -1px 0 0 #d6e4ff;
        }

        #ContentPlaceHolder1_loadGridView th:nth-child(2),
        #ContentPlaceHolder1_loadGridView td:nth-child(2) {
            position: sticky;
            left: 45px;
            z-index: 3;
            background: #f2f7ff;
            min-width: 120px;
            box-shadow: inset -1px 0 0 #d6e4ff;
        }

        #ContentPlaceHolder1_loadGridView th:nth-child(3),
        #ContentPlaceHolder1_loadGridView td:nth-child(3) {
            position: sticky;
            left: 190px;
            z-index: 3;
            background: #f2f7ff;
            min-width: 220px;
            box-shadow: inset -1px 0 0 #d6e4ff;
        }

        #ContentPlaceHolder1_loadGridView thead th:nth-child(1),
        #ContentPlaceHolder1_loadGridView thead th:nth-child(2),
        #ContentPlaceHolder1_loadGridView thead th:nth-child(3) {
            z-index: 4;
            background: #dfeaff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>DCR List</div>

                <div class="ms-auto">
                    <div class="btn-group">
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

                                        <div class="col-4">
                                            <div class="form-group row">
                                                <label for="FromDate" class="col-sm-5 col-form-label">From Date:  </label>

                                                <div class="col-sm-7">
                                                    <asp:TextBox runat="server" ID="FromDate" type="text" class="form-control form-control-sm  datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>

                                                    <script type="text/javascript">
                                                        function pageLoad() {

                                                            $('.multiple-select').select2({
                                                                includeSelectAllOption: true,
                                                                theme: 'bootstrap4',
                                                                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                                                placeholder: $(this).data('placeholder'),
                                                                allowClear: Boolean($(this).data('allow-clear')),
                                                            });
                                                            $('.datepicker').pickadate({
                                                                selectMonths: true,
                                                                selectYears: true
                                                            });
                                                            $('.mySelect2').select2({
                                                                theme: 'bootstrap4',
                                                                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                                                placeholder: $(this).data('placeholder'),
                                                                allowClear: Boolean($(this).data('allow-clear')),
                                                            });

                                                            $(".fancybox").fancybox({
                                                                openEffect: "none",
                                                                closeEffect: "none"
                                                            });

                                                            $(".zoom").hover(function () {

                                                                $(this).addClass('transition');
                                                            }, function () {

                                                                $(this).removeClass('transition');
                                                            });
                                                        }

                                                    </script>
                                                </div>

                                            </div>


                                            <div class="form-group row">
                                                <label for="ToDate" class="col-sm-5 col-form-label">To Date:  </label>

                                                <div class="col-sm-7">
                                                    <asp:TextBox runat="server" ID="ToDate" type="text" class="form-control form-control-sm   datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>

                                                </div>

                                            </div>



                                            


                                        </div>
                                        <div class="col-4">
                                            
                                            <div class="form-group row">
                                                <label for="UserRoleSelect" class="col-sm-5 col-form-label">Approval Status:  </label>

                                                <div class="col-sm-7">


                                                    <asp:DropDownList runat="server" ID="ApprovalStatusSelect" name="ApprovalStatusSelect" class="form-select form-select-sm   mySelect2"></asp:DropDownList>
                                                </div>

                                            </div>
                                            

                                            <div class="form-group row">
                                                <label for="EmployeeIdSelect" class="col-sm-5 col-form-label">Employee:  </label>

                                                <div class="col-sm-7">


                                                    <asp:DropDownList runat="server" ID="EmployeeIdSelect" name="EmployeeIdSelect" class="form-select form-select-sm   mySelect2"></asp:DropDownList>

                                                </div>

                                            </div>
                                            <div class="form-group row" style="display: none !important;">
                                                <label for="UserRoleSelect" class="col-sm-5 col-form-label">User Role:  </label>

                                                <div class="col-sm-7">


                                                    <asp:DropDownList runat="server" ID="UserRoleSelect" name="UserRoleSelect" class="form-select form-select-sm   mySelect2"></asp:DropDownList>
                                                </div>

                                            </div>



                                            <div class="form-group row" style="display: none !important;">
                                                <label for="mainName" class="col-sm-5 col-form-label">Pharma Platform: </label>

                                                <div class="col-sm-7">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlPharmaPlatform"></asp:DropDownList>



                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <uc1:IVMarketStructure runat="server" ID="IVMarketStructure" />
                                        </div>
                                    </div>



                                    <br />

                                    <div class="row">
                                        <div class="col-md-5">
                                        </div>
                                        <div class="col-md-4" style="align-content: center">

                                            <asp:LinkButton runat="server" ID="btnSearch" class="btn btnMyDesignSearch   btn-sm " OnClick="btnSearch_Click">  <i class="fa fa-search-plus"></i>&nbsp; Search</asp:LinkButton>


                                            <asp:LinkButton runat="server" class="btn btnMyDesignReset   btn-sm" ID="resetBtn" OnClick="resetBtn_Click"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </asp:LinkButton>

                                        </div>
                                    </div>
                                    <div style="padding-top: 10px;"></div>


                                    <div class="table-responsive" id="MainGradeDivs">



                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            DataKeyNames="DcrId"
                                            OnRowCommand="loadGridView_RowCommand" CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>

                                                <asp:TemplateField HeaderText="SL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DcrDate" HeaderText="DCR Date" />
                                                <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />
                                                <asp:BoundField DataField="DCRSenderName" HeaderText="Sales Person Name" />
                                                <asp:BoundField DataField="DoctorAddress" HeaderText="Doctor Address" />
                                                <asp:BoundField DataField="TourTypeName" HeaderText="Visit Type" />
                                                <asp:BoundField DataField="VisitedWith" HeaderText="Visited With" />
                                                <asp:BoundField DataField="ChamberName" HeaderText="Chamber" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Comments" />
                                                <asp:BoundField DataField="RoleName" HeaderText="Role" />
                                                <asp:TemplateField HeaderText="Location">
                                                    <ItemTemplate>


                                                        <a data-toggle='tooltip' title='Show in map' target='_blank' style='font-size: 20px' href='<%# "http://maps.google.com/?q=" +Eval("POutLoc")%>  ' class='bx bx-location-plus'></a>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="StreetAddress" HeaderText="Visited Address" />
                                                <asp:BoundField DataField="DoctorStreet" HeaderText="Doctor Street Address" />

                                                <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" />



                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-success btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData"><i class='fa fa-eye' aria-hidden='true'></i></asp:LinkButton>
                                                        <asp:LinkButton ID="lbApprove" runat="server" class="btn btn-info btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="ApproveData" 
                                                            Visible='<%# Eval("ApprovalStatus").ToString() == "Rejected" %>'><i class='fa fa-check' aria-hidden='true'></i></asp:LinkButton>
                                                        <asp:LinkButton ID="lbReject" runat="server" class="btn btn-danger btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="RejectData" 
                                                            Visible='<%# Eval("ApprovalStatus").ToString() == "Approved" %>'><i class='fadeIn animated bx bx-x' aria-hidden='true'></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>




                                    </div>
                                    </div>
                                               
                                                </div>
                                                </div>
                                                </div>
                                                </div>
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

        $(document).ready(function () {

            function initDataTable() {
                var $table = $('#ContentPlaceHolder1_loadGridView');

                // destroy existing instance safely if present
                if ($.fn.DataTable.isDataTable($table)) {
                    $table.DataTable().destroy();
                }

                $table.DataTable({
                    destroy: true,
                    bInfo: true,
                    bFilter: true,
                    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    pageLength: 10,
                    dom: 'lBfrtip',
                    buttons: ['copy', 'excel', 'pdf', 'print'],
                    language: {
                        emptyTable: "No data available"
                    }
                });
            }

            initDataTable();

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        initDataTable();
                    }
                });
            }
        });


    </script>

























</asp:Content>

