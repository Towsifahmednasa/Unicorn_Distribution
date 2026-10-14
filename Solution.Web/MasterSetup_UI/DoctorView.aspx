<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="DoctorView.aspx.cs" EnableEventValidation="false" Inherits="MasterSetup_UI_DoctorView" %>

<%@ Register Src="~/MasterSetup_UI/IVMarketStforAll.ascx" TagPrefix="uc1" TagName="IVMarketStructure" %> 
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        /*AutoComplete flyout */
        .autocomplete_completionListElement {
            margin: 0px !important;
            background-color: White !important;
            color: windowtext !important;
            border: buttonshadow !important;
            border-width: 1px !important;
            border-style: solid !important;
            cursor: 'default' !important;
            overflow: auto !important;
            font-family: Calibri !important;
            font-size: 12px !important;
            text-align: left !important;
            list-style-type: none !important;
            margin-left: 0px !important;
            padding-left: 0px !important;
            max-height: 200px !important;
            width: 300px !important;
            overflow: auto !important;
            box-shadow: 0 0 3px 1px rgba(0,0,0,.35) !important;
        }


        .autocomplete_completionListElement222 {
            margin: 0px !important;
            background-color: White !important;
            color: windowtext !important;
            border: buttonshadow !important;
            border-width: 1px !important;
            border-style: solid !important;
            cursor: 'default' !important;
            overflow: auto !important;
            font-family: Calibri !important;
            font-size: 12px !important;
            text-align: left !important;
            list-style-type: none !important;
            margin-left: 0px !important;
            padding-left: 0px !important;
            max-height: 200px !important;
            width: 600px !important;
            overflow: auto !important;
            box-shadow: 0 0 3px 1px rgba(0,0,0,.35) !important;
        }
        /* AutoComplete highlighted item */

        .autocomplete_highlightedListItem {
            background-color: #17A2B8 !important;
            color: white !important;
            padding: 6px !important;
            font-weight: bold !important;
        }

        /* AutoComplete item */

        .autocomplete_listItem {
            padding: 6px !important;
            cursor: pointer !important;
            font-weight: bold !important;
            background-color: #fff !important;
            border-bottom: 1px solid #d4d4d4 !important;
            box-shadow: 0 1px 1px rgba(0, 0, 0, 0.075) inset !important;
        }

        .ssss {
            font-size: 13px;
            font-weight: bold;
        }

        /* Freeze first 4 columns (SL, Action, Doctor Code, Doctor Name) on horizontal scroll.
           Widths are fixed so the left offsets line up: 60 + 110 + 120 = 290. */
        [id$="loadGridView"] > thead > tr > th:nth-child(-n+4),
        [id$="loadGridView"] > tbody > tr:not(.GridPager) > td:nth-child(-n+4) {
            position: sticky;
            z-index: 1;
            background-color: #fff;
        }

        [id$="loadGridView"] > thead > tr > th:nth-child(1),
        [id$="loadGridView"] > tbody > tr:not(.GridPager) > td:nth-child(1) {
            left: 0;
            min-width: 60px;
            max-width: 60px;
        }

        [id$="loadGridView"] > thead > tr > th:nth-child(2),
        [id$="loadGridView"] > tbody > tr:not(.GridPager) > td:nth-child(2) {
            left: 60px;
            min-width: 110px;
            max-width: 110px;
        }

        [id$="loadGridView"] > thead > tr > th:nth-child(3),
        [id$="loadGridView"] > tbody > tr:not(.GridPager) > td:nth-child(3) {
            left: 170px;
            min-width: 120px;
            max-width: 120px;
        }

        [id$="loadGridView"] > thead > tr > th:nth-child(4),
        [id$="loadGridView"] > tbody > tr:not(.GridPager) > td:nth-child(4) {
            left: 290px;
            min-width: 220px;
            max-width: 220px;
            white-space: normal;
        }

        /* Keep "Show entries" / "Search" / pager in view while the wide table scrolls sideways. */
        #MainGradeDiv .dataTables_length,
        #MainGradeDiv .dataTables_filter,
        #MainGradeDiv .dataTables_info,
        #MainGradeDiv .dataTables_paginate {
            position: sticky;
            float: left;
            left: 0;
        }

        #MainGradeDiv .dataTables_filter,
        #MainGradeDiv .dataTables_paginate {
            left: 260px;
            margin-left: 40px;
        }
    </style>
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Doctor  List</div>

                <div class="ms-auto">
                    <div class="btn-group">

                        <asp:LinkButton ID="EmpCetegoryAddImageButton" CssClass="btn btn-sm btn-outline-info " runat="server" OnClick="EmpCetegoryAddImageButton_Click"><i class="fa fa-plus" aria-hidden="true"></i> New Entry </asp:LinkButton>


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

                                            // "Show N entries" + "Search" on the doctor list. The grid is re-rendered on
                                            // every UpdatePanel postback, so this runs from pageLoad, not document.ready.
                                            var $grid = $('#ContentPlaceHolder1_loadGridView');
                                            if ($grid.find('thead').length && !$.fn.DataTable.isDataTable($grid)) {
                                                $grid.DataTable({
                                                    lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                                                    pageLength: 10,
                                                    ordering: false,
                                                    dom: 'lfrtip'
                                                });
                                            }
                                        }
                                    </script>


                                    <div style="padding: 2px!important"></div>

                                    <div class="row">

                                        <div class="col-6">
                                            <div id="divMarketStructure" style="display:none !important;">
    <uc1:IVMarketStructure runat="server" ID="IVMarketStructure" />
                                                
                                                

</div>
                                            
                                            
                                            <div class="form-group row" >
                                                <label for="ddlSalesPerson" class="col-sm-3 col-form-label">Sales Person: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlSalesPerson"></asp:DropDownList>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Doctor Type: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList runat="server" ID="DoctorTypeSelect" name="DoctorTypeSelect" class="form-select form-select-sm mb-3 mySelect2 "></asp:DropDownList>



                                                    </div>
                                                </div>

                                            </div>

                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Degree: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList runat="server" ID="degreeSelect" class="form-select form-select-sm mb-3 mySelect2 "></asp:DropDownList>



                                                    </div>
                                                </div>

                                            </div>
                                            
                                            
                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Doctor: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="custNameTextBox" runat="server" CssClass="form-control form-control-sm mb-3 "
                                                            AutoPostBack="True" OnTextChanged="custNameTextBox_TextChanged"></asp:TextBox>
                                                        <asp:AutoCompleteExtender
                                                            ID="at_txt_JobCirculation"
                                                            TargetControlID="custNameTextBox"
                                                            runat="server"
                                                            ServiceMethod="GetDoctor_ALL"
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


                                        <div class="col-6">

                                            <div class="form-group row" style="display: none !important;">
                                                <label for="mainName" class="col-sm-3 col-form-label">Provider Type: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlProgramType"></asp:DropDownList>
                                                        </span>
 

                                                    </div>
                                                </div>

                                            </div>


                                              <div class="form-group row" style="display: none !important;">
                                                <label for="mainName" class="col-sm-3 col-form-label">Pharma Platform: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlPharmaPlatform"></asp:DropDownList>
                                                       
 

                                                    </div>
                                                </div>
                                                  
                                                  
                                            </div>
                                            
                                            
                                            <div class="form-group row">
                                                <label for="mainName" class="col-sm-3 col-form-label">Active Status: </label>

                                                <div class="col-sm-8">
                                                    <div class="input-group">
                                                        <asp:DropDownList class="form-select form-select-sm mb-3 mySelect2 " runat="server" ID="ddlStatus">
                                                            <asp:ListItem Value="">Select Status</asp:ListItem>
                                                            <asp:ListItem  Selected="True"  Value="1">Active</asp:ListItem>
                                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                        </asp:DropDownList>
                                                    
 

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

                                            
                                        </div>
                                    </div>

                                    <div style="padding-top: 16px;"></div>
                                    <div class="row">
                                        <div class="col-md-5">
                                        </div>
                                        <div class="col-md-4" style="align-content: center">
                                            <asp:LinkButton runat="server" ID="btnSearch" class="btn btnMyDesignSearch   btn-sm " OnClick="btnSearch_Click">  <i class="fa fa-search-plus"></i>&nbsp; Search</asp:LinkButton>
                                            <asp:LinkButton runat="server" class="btn btnMyDesignReset   btn-sm" ID="resetBtn" OnClick="resetBtn_Click"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label></label>
                                        </div>


                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-2">
                                        </div>
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-1">
                                        </div>

                                        <div class="col-md-2" style="margin-top: 5px;">
                                            <asp:Label ID="lblCount" runat="server" CssClass="ssss btn bg-info pull-right" Text="Total : 0"></asp:Label>


                                        </div>


                                        <div class="col-md-2 ">
                                            <asp:LinkButton ID="btnExportToExcel" runat="server" CssClass="btn btn-success pull-right" OnClick="btnExportToExcel_Click"><span aria-hidden="true" class="fa fa-file-excel-o" ></span> &nbsp;Export To Excel</asp:LinkButton>




                                        </div>
                                    </div>

                                    <br />



                                    <div class="table-responsive" id="MainGradeDiv">



                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            DataKeyNames="DoctorId,DoctorCode" OnRowCommand="loadGridView_RowCommand" OnRowCreated="loadGridView_RowCreated" OnRowDataBound="loadGridView_RowDataBound"
                                            CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender" AllowPaging="False">
                                            <Columns>

                                                <%-- No TemplateFields in this grid: the dynamic detail columns make the Columns collection
                                                     reload from ViewState on postback, and TemplateFields come back without their template. --%>
                                                <asp:BoundField DataField="RowSL" HeaderText="SL" />
                                                <asp:ButtonField ButtonType="Button" CommandName="ToggleActive" DataTextField="ToggleText" HeaderText="Action" />
                                                <asp:BoundField DataField="DoctorCode" HeaderText="Doctor Code" />
                                                <asp:BoundField DataField="DoctorName" HeaderText="Doctor Name" />
                                                <asp:BoundField DataField="StreetAddress" Visible="False" HeaderText="Street Address" />
                                                <asp:BoundField DataField="ContactName" Visible="False" HeaderText="Mobile NO" />
                                                <asp:BoundField DataField="Gender" HeaderText="Gender" />
                                                 <asp:BoundField DataField="SalesPersonName" HeaderText="Sales Person Name" />
                                                <asp:BoundField DataField="RegionCode" Visible="False" HeaderText="Zone Code" />
                                                <asp:BoundField DataField="AreaCode" Visible="False" HeaderText="Area Code" />
                                                <asp:BoundField DataField="TerritoryCode" Visible="False" HeaderText="Territory Code" />
                                                <asp:BoundField DataField="MarketCode" Visible="False" HeaderText="Market Code" />
                                                <asp:BoundField DataField="MarketName" Visible="False" HeaderText="Market Name" />
                                                <asp:BoundField DataField="DesigName" HeaderText="Designation" />
                                                <asp:BoundField DataField="DegreeName" HeaderText="Degree" />

                                                <asp:BoundField DataField="DoctorTypeName" HeaderText="Doctor Type" />
                                                <asp:BoundField DataField="ProgramTypeName" Visible="False" HeaderText="Provider Type" />

                                                  <asp:BoundField DataField="SMCType" Visible="False" HeaderText="Pharma Platform" />
                                                <asp:BoundField DataField="DoctorSpeciality" HeaderText="Doctor Speciality" />
                                                <asp:BoundField DataField="ApprovalStatusWeb" HeaderText="Approval Status" />

                                                  <asp:BoundField DataField="StationTypeName" Visible="False" HeaderText="MIO Tour Type" />
                                                  <asp:BoundField DataField="Dept" Visible="False" HeaderText="Dept Tagging" />
                                                <asp:BoundField DataField="IsActiveText" HeaderText="Is Active" />
                                                <asp:BoundField DataField="EntryByText" HeaderText="Entry By" />
                                                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" />
                                                <asp:BoundField DataField="UpdateByText" HeaderText="Update By" />
                                                <asp:BoundField DataField="UpdateDate" HeaderText="Update Date" DataFormatString="{0:dd MMM yyyy}" />



                                                <asp:HyperLinkField HeaderText="Edit" DataNavigateUrlFields="DoctorId" DataNavigateUrlFormatString="DoctorEntry.aspx?mid={0}"
                                                    Target="_blank" Text="&lt;span class='btn-warning btn-sm mb-1 mb-md-0'&gt;&lt;i class='bx bxs-edit' aria-hidden='true'&gt;&lt;/i&gt;&lt;/span&gt;" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="left" CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>






                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnExportToExcel" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>

        //$(document).ready(function () {

        //    var table = $('#ContentPlaceHolder1_loadGridView').DataTable(
        //        {
        //            "bInfo": true,
        //            "bFilter": true,
        //            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        //            pageLength: 10,
        //            dom: 'lBfrtip',


        //            buttons: ['copy', 'excel', 'pdf', 'print']
        //        }
        //    );

        //    var prm = Sys.WebForms.PageRequestManager.getInstance();
        //    if (prm != null) {
        //        prm.add_endRequest(function (sender, e) {
        //            if (sender._postBackSettings.panelsToUpdate != null) {
        //                table = $('#ContentPlaceHolder1_loadGridView').DataTable(
        //                    {
        //                        "bInfo": true,
        //                        "bFilter": true,
        //                        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
        //                        pageLength: 10,
        //                        dom: 'lBfrtip',


        //                        buttons: ['copy', 'excel', 'pdf', 'print']


        //                    }
        //                );
        //            }
        //        });
        //    };


        //    table.columns().every(function () {
        //        var that = this;


        //    });
        //});


    </script>
</asp:Content>

