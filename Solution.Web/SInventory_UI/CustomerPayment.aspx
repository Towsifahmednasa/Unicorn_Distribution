<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="CustomerPayment.aspx.cs" Inherits="SInventory_UI_CustomerPayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        /* AutoComplete flyout style */
        .autocomplete_completionListElement {
            margin: 0px !important;
            background-color: #ffffff !important;
            color: #333333 !important;
            border: 1px solid #ced4da !important;
            cursor: default;
            overflow: auto;
            max-height: 250px;
            text-align: left;
            list-style-type: none !important;
            padding: 0px !important;
            z-index: 1050 !important; /* ensure it stays on top of buttons/grid */
            box-shadow: 0px 8px 16px rgba(0, 0, 0, 0.15) !important;
            border-radius: 4px;
            width: auto !important;
            min-width: 400px;
        }

        /* AutoComplete item style */
        .autocomplete_listItem {
            background-color: #ffffff !important;
            color: #333333 !important;
            padding: 8px 12px !important;
            border-bottom: 1px solid #f1f1f1;
            font-size: 13px;
            line-height: 1.5;
        }

        /* AutoComplete highlighted item style */
        .autocomplete_highlightedListItem {
            background-color: #0d6efd !important;
            color: #ffffff !important;
            padding: 8px 12px !important;
            cursor: pointer;
            font-size: 13px;
            line-height: 1.5;
            border-bottom: 1px solid #0d6efd;
        }

        /* AjaxControlToolkit CalendarExtender popup styling & layout fix */
        .ajax__calendar {
            position: absolute !important;
            z-index: 999999 !important;
        }
        .ajax__calendar .ajax__calendar_container {
            width: 220px !important;
            height: 210px !important;
            background-color: #ffffff !important;
            border: 1px solid #ced4da !important;
            border-radius: 6px !important;
            box-shadow: 0px 8px 24px rgba(0, 0, 0, 0.2) !important;
            padding: 6px !important;
            font-family: inherit !important;
            box-sizing: content-box !important;
        }
        .ajax__calendar .ajax__calendar_header {
            height: 26px !important;
            line-height: 26px !important;
            width: 100% !important;
            text-align: center !important;
            margin-bottom: 4px !important;
            background-color: #f8f9fa !important;
            border-bottom: 1px solid #dee2e6 !important;
            border-radius: 4px 4px 0 0 !important;
            position: relative !important;
        }
        .ajax__calendar .ajax__calendar_prev {
            width: 20px !important;
            height: 20px !important;
            position: absolute !important;
            left: 4px !important;
            top: 3px !important;
            cursor: pointer !important;
            background-repeat: no-repeat !important;
        }
        .ajax__calendar .ajax__calendar_next {
            width: 20px !important;
            height: 20px !important;
            position: absolute !important;
            right: 4px !important;
            top: 3px !important;
            cursor: pointer !important;
            background-repeat: no-repeat !important;
        }
        .ajax__calendar .ajax__calendar_title {
            cursor: pointer !important;
            font-weight: 600 !important;
            font-size: 13px !important;
            color: #0d6efd !important;
            display: inline-block !important;
            line-height: 26px !important;
        }
        .ajax__calendar .ajax__calendar_body {
            width: 100% !important;
            height: 145px !important;
            position: relative !important;
            overflow: hidden !important;
            background-color: #ffffff !important;
            margin: 0 !important;
            padding: 0 !important;
        }
        .ajax__calendar .ajax__calendar_days,
        .ajax__calendar .ajax__calendar_months,
        .ajax__calendar .ajax__calendar_years {
            top: 0px !important;
            left: 0px !important;
            width: 100% !important;
            height: 145px !important;
            position: absolute !important;
            text-align: center !important;
            margin: 0 !important;
            padding: 0 !important;
        }
        .ajax__calendar .ajax__calendar_days table,
        .ajax__calendar .ajax__calendar_months table,
        .ajax__calendar .ajax__calendar_years table {
            width: 100% !important;
            height: 140px !important;
            margin: 0 !important;
            border-collapse: separate !important;
            border-spacing: 1px !important;
            background-color: #ffffff !important;
        }
        .ajax__calendar tr {
            border: none !important;
        }
        .ajax__calendar td, .ajax__calendar th {
            background-color: transparent !important;
            padding: 1px !important;
            text-align: center !important;
            border: none !important;
        }
        .ajax__calendar .ajax__calendar_dayname {
            font-size: 11px !important;
            font-weight: 700 !important;
            color: #6c757d !important;
            height: 20px !important;
            line-height: 20px !important;
        }
        .ajax__calendar .ajax__calendar_day {
            width: 26px !important;
            height: 20px !important;
            line-height: 20px !important;
            font-size: 12px !important;
            border-radius: 4px !important;
            cursor: pointer !important;
            color: #212529 !important;
        }
        .ajax__calendar .ajax__calendar_day:hover {
            background-color: #e9ecef !important;
            color: #0d6efd !important;
        }
        .ajax__calendar .ajax__calendar_today .ajax__calendar_day {
            border: 1px solid #0d6efd !important;
            font-weight: bold !important;
            color: #0d6efd !important;
        }
        .ajax__calendar .ajax__calendar_active .ajax__calendar_day {
            background-color: #0d6efd !important;
            color: #ffffff !important;
            font-weight: bold !important;
        }
        .ajax__calendar .ajax__calendar_other .ajax__calendar_day {
            color: #ced4da !important;
        }
        .ajax__calendar .ajax__calendar_month,
        .ajax__calendar .ajax__calendar_year {
            height: 34px !important;
            line-height: 34px !important;
            cursor: pointer !important;
            border-radius: 4px !important;
            font-size: 12px !important;
        }
        .ajax__calendar .ajax__calendar_month:hover,
        .ajax__calendar .ajax__calendar_year:hover {
            background-color: #e9ecef !important;
        }
        .ajax__calendar .ajax__calendar_footer {
            height: 24px !important;
            line-height: 24px !important;
            background-color: #f8f9fa !important;
            border-top: 1px solid #dee2e6 !important;
            font-size: 11px !important;
            text-align: center !important;
            cursor: pointer !important;
            border-radius: 0 0 4px 4px !important;
        }

        /* ==================== DUE INVOICE GRID FROZEN / STICKY COLUMNS ==================== */
        #MainGradeDiv {
            overflow-x: auto !important;
            max-width: 100% !important;
            position: relative;
            border: 1px solid #dee2e6;
            border-radius: 6px;
        }

        /* Base table settings for sticky columns */
        table[id$="orderGridView"] {
            border-collapse: separate !important;
            border-spacing: 0 !important;
            margin-bottom: 0 !important;
        }

        /* Default Headers */
        table[id$="orderGridView"] thead th,
        table[id$="orderGridView"] th {
            background-color: #212529 !important;
            color: #ffffff !important;
            position: sticky;
            top: 0;
            z-index: 10;
            border-top: none !important;
            vertical-align: middle;
            font-size: 13px;
            white-space: nowrap;
        }

        /* Default Cells */
        table[id$="orderGridView"] td {
            background-color: #ffffff;
            vertical-align: middle;
            font-size: 13px;
            white-space: nowrap;
        }

        table[id$="orderGridView"] tr:nth-child(even) td {
            background-color: #f8f9fa;
        }

        table[id$="orderGridView"] tr:hover td {
            background-color: #f1f3f5;
        }

        /* Col 1: Checkbox */
        table[id$="orderGridView"] th:nth-child(1),
        table[id$="orderGridView"] td:nth-child(1) {
            position: sticky !important;
            left: 0px !important;
            width: 45px !important;
            min-width: 45px !important;
            max-width: 45px !important;
            text-align: center;
            border-right: 1px solid #dee2e6 !important;
        }

        /* Col 2: Customer ID */
        table[id$="orderGridView"] th:nth-child(2),
        table[id$="orderGridView"] td:nth-child(2) {
            position: sticky !important;
            left: 45px !important;
            width: 105px !important;
            min-width: 105px !important;
            max-width: 105px !important;
            text-align: center;
            border-right: 1px solid #dee2e6 !important;
        }

        /* Col 3: Customer Name */
        table[id$="orderGridView"] th:nth-child(3),
        table[id$="orderGridView"] td:nth-child(3) {
            position: sticky !important;
            left: 150px !important;
            width: 220px !important;
            min-width: 220px !important;
            max-width: 220px !important;
            text-align: left;
            border-right: 1px solid #dee2e6 !important;
            white-space: normal !important;
            word-wrap: break-word !important;
        }

        /* Col 4: Invoice No */
        table[id$="orderGridView"] th:nth-child(4),
        table[id$="orderGridView"] td:nth-child(4) {
            position: sticky !important;
            left: 370px !important;
            width: 150px !important;
            min-width: 150px !important;
            max-width: 150px !important;
            text-align: center;
            border-right: 1px solid #dee2e6 !important;
            white-space: normal !important;
            word-wrap: break-word !important;
        }

        /* Col 5: Invoice Date */
        table[id$="orderGridView"] th:nth-child(5),
        table[id$="orderGridView"] td:nth-child(5) {
            position: sticky !important;
            left: 520px !important;
            width: 120px !important;
            min-width: 120px !important;
            max-width: 120px !important;
            text-align: center;
            border-right: 2px solid #adb5bd !important;
            box-shadow: 3px 0 6px -2px rgba(0, 0, 0, 0.25) !important;
        }

        /* Z-Index & Backgrounds for Frozen Headers */
        table[id$="orderGridView"] th:nth-child(1),
        table[id$="orderGridView"] th:nth-child(2),
        table[id$="orderGridView"] th:nth-child(3),
        table[id$="orderGridView"] th:nth-child(4),
        table[id$="orderGridView"] th:nth-child(5) {
            background-color: #212529 !important;
            color: #ffffff !important;
            z-index: 30 !important;
        }

        /* Z-Index & Backgrounds for Frozen Body Cells */
        table[id$="orderGridView"] td:nth-child(1),
        table[id$="orderGridView"] td:nth-child(2),
        table[id$="orderGridView"] td:nth-child(3),
        table[id$="orderGridView"] td:nth-child(4),
        table[id$="orderGridView"] td:nth-child(5) {
            background-color: #ffffff !important;
            z-index: 15 !important;
        }

        table[id$="orderGridView"] tr:nth-child(even) td:nth-child(1),
        table[id$="orderGridView"] tr:nth-child(even) td:nth-child(2),
        table[id$="orderGridView"] tr:nth-child(even) td:nth-child(3),
        table[id$="orderGridView"] tr:nth-child(even) td:nth-child(4),
        table[id$="orderGridView"] tr:nth-child(even) td:nth-child(5) {
            background-color: #f8f9fa !important;
        }

        table[id$="orderGridView"] tr:hover td:nth-child(1),
        table[id$="orderGridView"] tr:hover td:nth-child(2),
        table[id$="orderGridView"] tr:hover td:nth-child(3),
        table[id$="orderGridView"] tr:hover td:nth-child(4),
        table[id$="orderGridView"] tr:hover td:nth-child(5) {
            background-color: #e9ecef !important;
        }
    </style>
    <script type="text/javascript">
        // Ensure modal backdrop is always removed when modal closes
        $(document).ready(function () {
            $(document).on('hidden.bs.modal', '#moneyModal', function () {
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <div id="popDiv">
            </div>

            <div class="page-wrapper">
                <div class="page-content">
                    <!--breadcrumb-->
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Customer Payment </div>

                        <div class="ms-auto" runat="server" visible="False">
                            <div class="btn-group" />


                            <a href="CustomerPaymentSnd.aspx" class="btn btn-sm btn-sm btn-outline-danger" target="_blank">
                                <i class="fa fa-undo"></i>&nbsp; Customer payment for second return
                            </a>&nbsp; &nbsp; 

                             <a href="CustomerPaymentExcelUpload.aspx" class="btn btn-sm btn-sm btn-outline-info"><i class="fa fa-file-excel-o"></i>&nbsp; Excel Upload</a>

                        </div>
                    </div>
                    <!--end breadcrumb-->
                    <div class="row">
                        <div class="col">

                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                                <ProgressTemplate>

                                                    <div class="divWaiting">
                                                        <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>


                                            <div class="row">
                                                <div class="col-2">&nbsp;</div>
                                                <div class="col-8">
                                                    <div class="form-group row">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Sales Center:</label>

                                                        <div class="col-sm-5">


                                                            <asp:DropDownList ID="salesCenterDropDownList" runat="server" CssClass="form-select form-select-sm mb-3 mySelect2"
                                                                AutoPostBack="True" OnSelectedIndexChanged="salesCenterDropDownList_SelectedIndexChanged">
                                                            </asp:DropDownList>

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


                                                        </div>
                                                        <span class="text-sm-left text-c-red">*</span>
                                                    </div>

                                                    <div class="form-group row">
                                                         <label for="mainName" class="col-sm-3 col-form-label">Customer:</label>

                                                         <div class="col-sm-5">
                                                             <asp:TextBox ID="txtCustomerSearch" runat="server" CssClass="form-control form-control-sm mb-3"
                                                                 AutoPostBack="True" OnTextChanged="txtCustomerSearch_TextChanged" placeholder="Type Customer Name or Code"></asp:TextBox>
                                                             <asp:AutoCompleteExtender ID="txtCustomerSearch_AutoCompleteExtender" runat="server"
                                                                 TargetControlID="txtCustomerSearch"
                                                                 ServiceMethod="GetCustomerPaymentSearch"
                                                                 ServicePath="SInventoryWebService.asmx"
                                                                 MinimumPrefixLength="1"
                                                                 CompletionInterval="10"
                                                                 EnableCaching="false"
                                                                 CompletionSetCount="10"
                                                                 FirstRowSelected="false"
                                                                 CompletionListCssClass="autocomplete_completionListElement"
                                                                 CompletionListItemCssClass="autocomplete_listItem"
                                                                 CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                 UseContextKey="True">
                                                             </asp:AutoCompleteExtender>
                                                             <asp:HiddenField ID="hfCustomerSearchId" runat="server" />
                                                         </div>
                                                         <span class="text-sm-left text-c-red">&nbsp;</span>
                                                     </div>



                                                    <asp:HiddenField runat="server" ID="hfCustPayId" />



                                                    <div class="form-group row" runat="server" visible="False">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Route:</label>

                                                        <div class="col-sm-5">


                                                            <asp:DropDownList ID="rootDropDownList" AutoPostBack="true" OnSelectedIndexChanged="rootDropDownList_SelectedIndexChanged" runat="server" CssClass="form-control form-control-sm mySelect2 ">
                                                            </asp:DropDownList>


                                                        </div>
                                                        <span class="text-sm-left text-c-red">*</span>
                                                    </div>


                                                    <div class="form-group row" runat="server" visible="False">
                                                        <label for="mainName" class="col-sm-3 col-form-label">Territory Name:</label>

                                                        <div class="col-sm-5">


                                                            <asp:DropDownList runat="server" class="form-select form-select-sm mb-3 mySelect2 " ID="ddlTerritoryName"></asp:DropDownList>


                                                        </div>
                                                        <span class="text-sm-left text-c-red">*</span>
                                                    </div>



                                                    <div class="form-group row" runat="server" visible="False">
                                                        <label for="mainName" class="col-sm-3 col-form-label">DA Name:</label>

                                                        <div class="col-sm-5">


                                                            <asp:DropDownList runat="server" class="form-select form-select-sm mb-3 mySelect2 " ID="ddlDAName" AutoPostBack="true" OnSelectedIndexChanged="ddlDAName_SelectedIndexChanged"></asp:DropDownList>


                                                        </div>
                                                        <span class="text-sm-left text-c-red">*</span>
                                                    </div>


                                                </div>
                                            </div>


                                            <br />
                                            <div class="row">
                                                <div class="col-2">&nbsp;</div>
                                                <div class="col-8">

                                                    <div class="form-group row">
                                                        <label for="exampleInputUsername2" class="col-sm-3 col-form-label"></label>
                                                        <div class="col-sm-8">

                                                            <asp:LinkButton OnClick="submitButton_Click" runat="server" ID="submitButton" class="btn btnMyDesignSearch   btn-sm">
                 <i class="fa fa-search"></i> Search
                                                            </asp:LinkButton>
                                                            <asp:LinkButton runat="server" OnClick="cancelButton_Click" class="btn btnMyDesignReset   btn-sm"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </asp:LinkButton>



                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="col-2">&nbsp;</div>
                                            </div>


                                            <div class="row">

                                                <div class="form-group row" style="display: none">
                                                    <div class="col-4">
                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-4 col-form-label">Sales Center:</label>

                                                            <div class="col-sm-7">
                                                            </div>
                                                            <span class="text-sm-left text-c-red">*</span>
                                                        </div>

                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-4 col-form-label">Payment Type:</label>

                                                            <div class="col-sm-7">

                                                                <asp:DropDownList ID="payTypeDDL" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>


                                                            </div>
                                                            <span class="text-sm-left text-c-red">*</span>
                                                        </div>
                                                    </div>
                                                    <div class="col-4">

                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-4 col-form-label">Payment Date:</label>

                                                            <div class="col-sm-7">
                                                            </div>
                                                            <span class="text-sm-left text-c-red">*</span>
                                                        </div>

                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-4 col-form-label">Customer:</label>

                                                            <div class="col-sm-7">
                                                            </div>
                                                            <span class="text-sm-left text-c-red">*</span>
                                                        </div>



                                                    </div>
                                                    <div class="col-4">

                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-4 col-form-label">Reference No:</label>

                                                            <div class="col-sm-7">

                                                                <div class="input-group">
                                                                    <asp:TextBox ID="refNameTextBox" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>
                                                                    <asp:TextBox ID="refDtTextBox" Visible="False" runat="server" AutoPostBack="True" CssClass="form-control form-control-sm mb-3"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="paymentDtTextBox_CalendarExtender0" runat="server"
                                                                        Format="dd-MMM-yyyy" PopupButtonID="ImageButton0"
                                                                        TargetControlID="refDtTextBox">
                                                                    </asp:CalendarExtender>
                                                                    <asp:ImageButton Visible="False" ID="ImageButton0" runat="server"
                                                                        AlternateText="Click to show calendar"
                                                                        ImageUrl="~/Images/Calendar_scheduleHS.png" TabIndex="4" />

                                                                </div>


                                                            </div>
                                                            <span class="text-sm-left text-c-red">*</span>
                                                        </div>


                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-4 col-form-label">Payment Amount:</label>

                                                            <div class="col-sm-7">
                                                            </div>
                                                            <span class="text-sm-left text-c-red">*</span>
                                                        </div>

                                                    </div>
                                                </div>


                                                <div runat="server" visible="False">


                                                    <div class="col-2">&nbsp;</div>
                                                    <div class="col-8">



                                                        <div class="form-group row">
                                                            <label for="txtNID" class="col-sm-3 col-form-label">Sales Center:</label>

                                                            <div class="col-sm-7">
                                                                <div class="input-group">
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-3 col-form-label">Market: </label>

                                                            <div class="col-sm-7">
                                                                <div class="input-group">
                                                                    <asp:DropDownList ID="marketDropDownList" runat="server" AutoPostBack="True"
                                                                        CssClass="form-control form-control-sm "
                                                                        OnSelectedIndexChanged="marketDropDownList_SelectedIndexChanged">
                                                                    </asp:DropDownList>

                                                                </div>

                                                            </div>
                                                        </div>




                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-3 col-form-label">Customer: </label>

                                                            <div class="col-sm-7">
                                                                <div class="input-group">

                                                                    <asp:DropDownList ID="customerDropDownList" Visible="False" runat="server" AutoPostBack="True" CssClass="DropDown" OnSelectedIndexChanged="customerDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-3 col-form-label">Payment Date: </label>

                                                            <div class="col-sm-7">
                                                                <div class="input-group">
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-3 col-form-label">Payment Amount: </label>

                                                            <div class="col-sm-7">
                                                                <div class="input-group">
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-3 col-form-label">Payment Type: </label>

                                                            <div class="col-sm-7">
                                                                <div class="input-group">
                                                                </div>

                                                            </div>
                                                        </div>


                                                        <div class="form-group row">
                                                            <label for="mainName" class="col-sm-3 col-form-label">Reference No: </label>

                                                            <div class="col-sm-7">
                                                            </div>
                                                        </div>




                                                        <br />



                                                    </div>

                                                </div>

                                                <br />

                                                <!-- Payment Details & Bulk Disburse Section -->
                                                <div class="card my-3" style="background-color: #f8fbf9; border: 1px solid #b8dfd8; border-left: 4px solid #17a2b8; border-radius: 6px;">
                                                    <div class="card-body p-3">
                                                        <div class="row">
                                                            <div class="col-md-3 col-sm-6 mb-2">
                                                                <label class="form-label font-weight-bold text-dark" style="font-size: 12px; margin-bottom: 2px;">
                                                                    Payment Date: <span class="text-danger">*</span>
                                                                </label>
                                                                <div class="input-group input-group-sm">
                                                                    <asp:TextBox ID="paymentDateTextBox" runat="server" CssClass="form-control form-control-sm" placeholder="dd-MMM-yyyy" style="height: 32px; font-size: 12px;"></asp:TextBox>
                                                                    <span class="input-group-text bg-light text-primary" onclick="$('#<%= paymentDateTextBox.ClientID %>').focus();" style="cursor: pointer;"><i class="fa fa-calendar"></i></span>
                                                                </div>
                                                                <asp:CalendarExtender ID="cePaymentDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="paymentDateTextBox" PopupButtonID="paymentDateTextBox"></asp:CalendarExtender>
                                                            </div>

                                                            <div class="col-md-3 col-sm-6 mb-2">
                                                                <label class="form-label font-weight-bold text-dark" style="font-size: 12px; margin-bottom: 2px;">
                                                                    Cheque No:
                                                                </label>
                                                                <asp:TextBox ID="checkNoTextBox" runat="server" CssClass="form-control form-control-sm" placeholder="Cheque / Instrument No" style="height: 32px; font-size: 12px;"></asp:TextBox>
                                                            </div>

                                                            <div class="col-md-3 col-sm-6 mb-2">
                                                                <label class="form-label font-weight-bold text-dark" style="font-size: 12px; margin-bottom: 2px;">
                                                                    Cheque Date:
                                                                </label>
                                                                <div class="input-group input-group-sm">
                                                                    <asp:TextBox ID="checkDateTextBox" runat="server" CssClass="form-control form-control-sm" placeholder="dd-MMM-yyyy" style="height: 32px; font-size: 12px;"></asp:TextBox>
                                                                    <span class="input-group-text bg-light text-primary" onclick="$('#<%= checkDateTextBox.ClientID %>').focus();" style="cursor: pointer;"><i class="fa fa-calendar"></i></span>
                                                                </div>
                                                                <asp:CalendarExtender ID="ceCheckDate" runat="server" Format="dd-MMM-yyyy" TargetControlID="checkDateTextBox" PopupButtonID="checkDateTextBox"></asp:CalendarExtender>
                                                            </div>

                                                            <div class="col-md-3 col-sm-6 mb-2">
                                                                <label class="form-label font-weight-bold text-dark" style="font-size: 12px; margin-bottom: 2px;">
                                                                    Remarks:
                                                                </label>
                                                                <asp:TextBox ID="remarksTextBox" runat="server" CssClass="form-control form-control-sm" placeholder="Payment remarks / notes"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <!-- Bulk Disbursement Amount Row -->
                                                        <div class="row mt-2 pt-2" style="border-top: 1px dashed #d1d5db;">
                                                            <div class="col-md-4 col-sm-6 mb-1">
                                                                <label class="form-label font-weight-bold text-primary" style="font-size: 12px; margin-bottom: 2px;">
                                                                    <i class="fa fa-money"></i> Collection / Disburse Amount:
                                                                </label>
                                                                <asp:TextBox ID="txtDisburseAmount" runat="server" CssClass="form-control form-control-sm font-weight-bold" 
                                                                             placeholder="Enter collection amount" oninput="disbursePaymentAmount();" onkeyup="disbursePaymentAmount();"></asp:TextBox>
                                                                <asp:FilteredTextBoxExtender ID="ftbeDisburse" runat="server" Enabled="True" 
                                                                    TargetControlID="txtDisburseAmount" FilterType="Custom" ValidChars="0123456789.">
                                                                </asp:FilteredTextBoxExtender>
                                                            </div>
                                                            <div class="col-md-5 col-sm-6 mb-1 d-flex align-items-end">
                                                                <small class="text-muted" style="font-size: 11px; line-height: 1.3;">
                                                                    <i class="fa fa-info-circle text-info"></i> Enter total amount above; it will auto-allocate top to bottom across checked invoices based on Due Amount.
                                                                </small>
                                                            </div>
                                                            <div class="col-md-3 col-sm-12 mb-1 text-right d-flex align-items-end justify-content-end">
                                                                <asp:Label ID="lblCount" runat="server" CssClass="ssss btn bg-info text-white font-weight-bold" Text="Total Pay Amount : 0"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <script type="text/javascript">
                                                    function disbursePaymentAmount() {
                                                        var txtDisburse = document.querySelector('[id$="txtDisburseAmount"]');
                                                        if (!txtDisburse) return;
                                                        var rawVal = txtDisburse.value.replace(/,/g, '').trim();
                                                        if (rawVal === '' || isNaN(rawVal)) {
                                                            return;
                                                        }
                                                        var remaining = parseFloat(rawVal);
                                                        if (remaining < 0) remaining = 0;

                                                        var grid = document.querySelector('[id$="orderGridView"]');
                                                        if (!grid) return;

                                                        var rows = grid.getElementsByTagName('tr');
                                                        var totalPay = 0;

                                                        for (var i = 1; i < rows.length; i++) {
                                                            var row = rows[i];
                                                            var chk = row.querySelector('input[type="checkbox"][id*="chkSelect"]');
                                                            var txtPay = row.querySelector('input[type="text"][id*="payAmountTextBox"]');
                                                            var lblDue = row.querySelector('span[id*="lblDue"]');

                                                            if (chk && txtPay && lblDue) {
                                                                if (chk.checked) {
                                                                    var due = parseFloat(lblDue.innerText.replace(/,/g, '').trim()) || 0;
                                                                    if (remaining > 0) {
                                                                        if (remaining >= due) {
                                                                            txtPay.value = due.toFixed(2);
                                                                            remaining -= due;
                                                                            totalPay += due;
                                                                        } else {
                                                                            txtPay.value = remaining.toFixed(2);
                                                                            totalPay += remaining;
                                                                            remaining = 0;
                                                                        }
                                                                    } else {
                                                                        txtPay.value = "0.00";
                                                                    }
                                                                } else {
                                                                    txtPay.value = "0.00";
                                                                }
                                                            }
                                                        }

                                                        var lblTotal = document.querySelector('[id$="lblCount"]');
                                                        if (lblTotal) {
                                                            lblTotal.innerText = "Total Pay Amount : " + totalPay.toFixed(2);
                                                        }
                                                    }
                                                </script>

                                                <div class="row mt-2">
                                                    <div class="col-6">
                                                        <h5><i class="fa fa-list" aria-hidden="true"></i> Due Invoice List </h5>
                                                    </div>
                                                    <div class="col-6 text-right">
                                                    </div>
                                                </div>


                                                <div class="row" style="margin-top: 10px;">
                                                    <div class="table-responsive" id="MainGradeDiv">
                                                        <asp:GridView ID="orderGridView" runat="server"
                                                            AutoGenerateColumns="False" CssClass="table table-bordered  text-center thead-dark" DataKeyNames="InvoiceId,CompanyId,PaymentTypeName" OnRowDataBound="orderGridView_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True"
                                                                            OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField runat="server" ID="hfCustomerMasterId" Value='<%#Eval("CustomerMasterId")%>' />
                                                                        <asp:HiddenField runat="server" ID="hfMarketId" Value='<%#Eval("MarketId")%>' />
                                                                        <asp:HiddenField runat="server" ID="hfDistributionRouteId" Value='<%#Eval("DistributionRouteId")%>' />
                                                                        <asp:HiddenField runat="server" ID="hfInvoiceId" Value='<%#Eval("InvoiceId")%>' />


                                                                        <asp:HiddenField runat="server" ID="hfTP_Pay" Value='<%#Eval("TP_Pay")%>' />
                                                                        <asp:HiddenField runat="server" ID="hfVat_Pay" Value='<%#Eval("Vat_Pay")%>' />


                                                                        <asp:CheckBox OnCheckedChanged="chkSelect_CheckedChanged" ID="chkSelect" AutoPostBack="True" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" />
                                                                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                                <asp:BoundField DataField="TerritoryName_Ord" HeaderText="Territory Name" Visible="False" />

                                                                <%--       <asp:BoundField DataField="OrderNo" HeaderText="Order Code" />
  <asp:BoundField DataField="OrderDate" HeaderText="Order / Submission Date" DataFormatString="{0:dd-MMM-yyyy}" />--%>
                                                                <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                                                <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                <%--    <asp:BoundField DataField="RtnInvoiceNo" HeaderText="Confirm Invoice No" />
                                                                <asp:BoundField DataField="RtnInvoiceDate" HeaderText="Confirm Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                --%>


                                                                <asp:TemplateField HeaderText="Confirm  Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblTotalDelivery" Text='<%#Eval("TotalDelivery")%>' />

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Previous Pay">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblPaymentAmount" Text='<%#Eval("PaymentAmount")%>' />

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField HeaderText="Due Amount">
                                                                    <ItemTemplate>
                                                                        <asp:Label runat="server" ID="lblDue" Text='<%#Eval("Due")%>' />

                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Pay Amount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="payAmountTextBox" Width="120px" runat="server" Text='<%#Eval("Due")%>' AutoPostBack="True"
                                                                            CssClass="form-control form-control-sm" OnTextChanged="payAmountTextBox_TextChanged"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderunitValue" runat="server"
                                                                            Enabled="True" TargetControlID="payAmountTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Discount">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="discountTextBox" Width="80px" runat="server" Text="0.00" AutoPostBack="True"
                                                                            CssClass="form-control form-control-sm" OnTextChanged="discountTextBox_TextChanged"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderDiscount" runat="server"
                                                                            Enabled="True" TargetControlID="discountTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="AIT / VAT">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="aitTextBox" Width="80px" runat="server" Text="0.00" AutoPostBack="True"
                                                                            CssClass="form-control form-control-sm" OnTextChanged="aitTextBox_TextChanged"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtenderAit" runat="server"
                                                                            Enabled="True" TargetControlID="aitTextBox" FilterType="Custom" ValidChars="0123456789.">
                                                                        </asp:FilteredTextBoxExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Is Bank">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="isBank" AutoPostBack="True" runat="server" OnCheckedChanged="isBank_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Bank Account">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlBank" Width="150px" CssClass="form-control form-control-sm" runat="server" AutoPostBack="True"></asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Is Cash">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="isCash" AutoPostBack="True" runat="server" OnCheckedChanged="isCash_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Cash Account">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlCash" Width="150px" CssClass="form-control form-control-sm" runat="server"></asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Collection By">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlCollectionBy" Width="110px" CssClass="form-control form-control-sm" runat="server">
                                                                            <asp:ListItem Text="DIC" Value="DIC"></asp:ListItem>
                                                                            <asp:ListItem Text="MIO" Value="MIO"></asp:ListItem>
                                                                            <asp:ListItem Text="X. MIO" Value="X. MIO"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>


                                                                <asp:TemplateField Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkAdjust" OnCheckedChanged="chkAdjust_OnCheckedChanged" AutoPostBack="True" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField Visible="false" DataField="AdjustableAmount" HeaderText="AdjustableAmount" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>

                                              

                                                <div class="row">
                                                    <div class="col-2">&nbsp;</div>
                                                    <div class="col-8">
                                                        
                                                          <br />
                                                <br />


                                                        <div class="form-group row">
                                                            <label for="exampleInputUsername2" class="col-sm-3 col-form-label"></label>
                                                            <div class="col-sm-8">

                                                                <asp:LinkButton OnClick="saveButton_Click" runat="server" ID="masterButton" OnClientClick="return sweetAlertConfirm_Submit(this);" class="btnMyyNew btnMyyNew-6">  <i class="fa fa-check"></i> ALL Submit</asp:LinkButton>


                                                            </div>
                                                        </div>
                                                        
                                                          <br />


                                                    </div>
                                                    <div class="col-2">
                                                    </div>
                                                </div>




                                                <div class="col-2">&nbsp;</div>
                                            </div>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <!-- Bootstrap Modal -->



        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="modal fade" id="moneyModal" tabindex="-1" aria-labelledby="moneyModalLabel" aria-hidden="true" data-backdrop="static" data-bs-backdrop="static">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content shadow-lg border-0" style="border-radius: 8px;">
                        <div class="modal-header bg-success text-white py-2" style="border-radius: 8px 8px 0 0;">
                            <h5 class="modal-title fs-6 text-white" id="moneyModalLabel"><i class="fa fa-check-circle me-1"></i> Payment Saved Successfully</h5>
                            <button type="button" class="btn-close btn-close-white close text-white" data-dismiss="modal" data-bs-dismiss="modal" aria-label="Close" 
                                    onclick="$('#moneyModal').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();"
                                    style="background: transparent; border: none; font-size: 20px; cursor: pointer;">&times;</button>
                        </div>
                        <div class="modal-body text-center py-4">
                            <div class="mb-3">
                                <i class="fa fa-check-circle text-success" style="font-size: 48px;"></i>
                            </div>
                            <h5 class="text-dark font-weight-bold mb-2">Payment Saved Successfully!</h5>
                            <p class="text-muted mb-0 font-14">Do you want to print the Money Receipt now?</p>
                        </div>
                        <div class="modal-footer bg-light py-2 justify-content-center" style="border-radius: 0 0 8px 8px;">
                            <asp:Button ID="btnMoneyReceiptPrint" runat="server" CssClass="btn btn-primary btn-sm px-3"
                                Text="Print Money Receipt" OnClick="btnMoneyReceiptPrint_Click" 
                                OnClientClick="$('#moneyModal').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();" />
                            <asp:Button ID="btnSavePayment" runat="server" CssClass="btn btn-secondary btn-sm px-3"
                                Text="Close (Don't Print)" OnClick="btnSavePayment_Click" 
                                OnClientClick="$('#moneyModal').modal('hide'); $('body').removeClass('modal-open'); $('.modal-backdrop').remove();" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

