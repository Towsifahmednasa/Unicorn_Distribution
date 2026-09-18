<%@ page title="" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="Approval_UI_DCRApprovalList, App_Web_ipkvamod" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>DCR Approval List</div>

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
                                    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                        <ProgressTemplate>

                                            <div class="divWaiting">
                                                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
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


                                    <div class="row">
                                        <div class="col-1">
                                        </div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label for="FromDate" class="col-sm-4 col-form-label">From Date:  </label>

                                                <div class="col-sm-8">
                                                    <asp:TextBox runat="server" ID="FromDate" type="text" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>

                                                </div>

                                            </div>

                                        </div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label for="EmployeeIdSelect" class="col-sm-4 col-form-label">Employee:  </label>

                                                <div class="col-sm-8">


                                                    <asp:DropDownList runat="server" ID="EmployeeIdSelect" name="EmployeeIdSelect" class="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>

                                                </div>

                                            </div>

                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-1">
                                        </div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label for="ToDate" class="col-sm-4 col-form-label">To Date:  </label>

                                                <div class="col-sm-8">
                                                    <asp:TextBox runat="server" ID="ToDate" type="text" class="form-control form-control-sm mb-3 datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>

                                                </div>

                                            </div>

                                        </div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label for="UserRoleSelect" class="col-sm-4 col-form-label">User Role:  </label>

                                                <div class="col-sm-8">


                                                    <asp:DropDownList runat="server" ID="UserRoleSelect" name="UserRoleSelect" class="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>
                                                </div>

                                            </div>

                                        </div>
                                    </div>


                                    <div class="row">

                                        <div class="col-1">
                                        </div>
                                        <div class="col-5">
                                            <div class="form-group row">
                                                <label for="UserRoleSelect" class="col-sm-4 col-form-label">Approval Status:  </label>

                                                <div class="col-sm-8">


                                                    <asp:DropDownList runat="server" ID="ApprovalStatusSelect" name="ApprovalStatusSelect" class="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>
                                                </div>

                                            </div>

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

                                    <hr class="my-3" />
                                    <div class="d-flex justify-content-between align-items-center mb-3">
                                        <h5 class="mb-0"><i class="bx bx-list-ul"></i> DCR List</h5>
                                        <div>
                                            <asp:LinkButton ID="btnBulkApprove" runat="server" CssClass="btn btn-sm btn-success" Style="margin-right: 15px !important;" OnClick="btnBulkApprove_Click" OnClientClick="return confirm('Are you sure you want to approve all selected DCR records?');">
                                                <i class="fa fa-check"></i> Approve Selected
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnBulkReject" runat="server" CssClass="btn btn-sm btn-danger" OnClick="btnBulkReject_Click" OnClientClick="return confirm('Are you sure you want to reject all selected DCR records?');">
                                                <i class="bx bx-x"></i> Reject Selected
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <hr class="my-3" />

                                    <div class="table-responsive" id="MainGradeDiv">



                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            DataKeyNames="TableId,DCRApprovalId,FromEmpId,ToEmpId,Step,RoleTypeId,ToRoleTypeId,MaxStep" OnRowCommand="loadGridView_RowCommand"
                                            CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender">
                                            <Columns>

                                                <asp:TemplateField HeaderText="SL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" onclick="SelectAllRows(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />

                                                <asp:BoundField DataField="EntryDate" HeaderText="DCR Date" />
                                                <asp:BoundField DataField="DCRSenderName" HeaderText="Sales Person Name" />
                                                

                                                <asp:BoundField DataField="TourTypeName" HeaderText="Visit Type" />

                                                <asp:BoundField DataField="ChamberName" HeaderText="Chamber" />
                                                <asp:BoundField DataField="Comments" HeaderText="Comments" />

                                                <asp:TemplateField HeaderText="Location">
                                                    <ItemTemplate>


                                                        <a data-toggle='tooltip' title='Show in map' target='_blank' style='font-size: 20px' href='<%# "http://maps.google.com/?q=" +Eval("POutLoc")%>  ' class='bx bx-location-plus'></a>

                                                    </ItemTemplate>
                                                </asp:TemplateField>



                                                <asp:BoundField DataField="ApprovalStatusWeb" HeaderText="Approval Status" />

                                                <asp:BoundField DataField="WaitingForRole" HeaderText="Waiting For"  Visible="False"/>




                                                <asp:TemplateField HeaderText="Actions">
                                                    <ItemTemplate>
                                                        <asp:HiddenField runat="server" ID="hfCustomerMasterId" Value='<%#Eval("TableId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfFromEmpId" Value='<%#Eval("FromEmpId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfToEmpId" Value='<%#Eval("ToEmpId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfStep" Value='<%#Eval("Step")%>' />
                                                        <asp:HiddenField runat="server" ID="hfRoleTypeId" Value='<%#Eval("RoleTypeId")%>' />
                                                        <asp:HiddenField runat="server" ID="hfCustomerApprovalId" Value='<%#Eval("DCRApprovalId")%>' />

                                                        <asp:HiddenField runat="server" ID="hfToRoleTypeId" Value='<%#Eval("ToRoleTypeId")%>' />

                                                        <asp:Label runat="server" ID="lbMsg" />
                                                        <asp:LinkButton ID="lbEdit" runat="server" Visible="false" class="btn-success  btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData"><i class='fa fa-eye' aria-hidden='true'></i></asp:LinkButton>

                                                        <asp:LinkButton ID="lbApprove" runat="server" class="btn-info  btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="ApproveData"><i class='fa fa-check' aria-hidden='true'></i> </asp:LinkButton>


                                                        <asp:LinkButton ID="lbReject" runat="server" class="btn-danger  btn-sm mb-1 mb-md-0"
                                                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="RejectData"> </i><i class='fadeIn animated bx bx-x' aria-hidden='true'></i> </asp:LinkButton>

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
                    // prevents "Incorrect column count" error when GridView returns 0 rows
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

