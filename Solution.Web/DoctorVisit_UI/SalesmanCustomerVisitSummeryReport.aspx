<%@ Page Title="Salesman Wise Customer Coverage & DCR Summary Sheet" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="SalesmanCustomerVisitSummeryReport.aspx.cs" Inherits="DoctorVisit_UI_SalesmanCustomerVisitSummeryReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        /* Report Layout and Styling */
        .report-card-container {
            background: #ffffff;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
            margin-top: 15px;
        }

        .excel-style-table {
            border-collapse: collapse;
            width: 100%;
            font-family: 'Calibri', 'Arial', sans-serif;
            font-size: 14px;
            background-color: #ffffff;
            margin-top: 10px;
        }
        
        .excel-style-table th, .excel-style-table td {
            border: 1px solid #BFBFBF !important;
            padding: 8px 12px;
            vertical-align: middle;
            color: #000000;
        }
        
        .excel-style-table th {
            background-color: #005A5B !important; /* Dark teal background matching GL headers */
            color: #ffffff !important;
            font-weight: bold;
            text-align: center;
            border: 1px solid #BFBFBF !important;
            font-size: 14px;
            height: 35px;
        }
        
        .total-row {
            background-color: #ffffff !important;
            font-weight: bold;
        }
        
        .total-row td {
            border-top: 2px solid #000000 !important; /* Thick top border */
            border-bottom: 2px double #000000 !important; /* Double bottom border for accounting style */
            font-weight: bold;
            background-color: #ffffff !important;
        }

        /* Header Info styling */
        .report-header-title {
            font-family: 'Calibri', 'Arial', sans-serif;
            color: #005A5B;
            font-weight: bold;
            font-size: 24px;
            margin: 0;
            padding: 0;
            text-align: center;
            margin-bottom: 20px;
        }

        .text-right {
            text-align: right !important;
        }
        .text-center {
            text-align: center !important;
        }
        .text-left {
            text-align: left !important;
        }

        .divWaitingJquery {
            position: fixed;
            z-index: 99999;
            height: 100%;
            width: 100%;
            top: 0;
            left: 0;
            background-color: rgba(0, 0, 0, 0.3);
            filter: alpha(opacity=60);
            opacity: 0.6;
            text-align: center;
        }
        
        .position-set {
            position: absolute;
            top: 40%;
            left: 45%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="coverScreen" class="divWaitingJquery" style="display:none;">
        <img src="../images/Spinner.gif" style="width:180px" class="position-set" />
    </div>

    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Salesman Wise Customer Coverage & DCR Summary Sheet</div>
                <div class="ms-auto">
                    <div class="btn-group"></div>
                </div>
            </div>
            <!--end breadcrumb-->

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <!-- Filter Section (Centered 2x2 Layout) -->
                            <div class="row justify-content-center">
                                <div class="col-md-5">
                                    <div class="form-group row">
                                        <label for="FromDate" class="col-sm-4 col-form-label text-end">From Date: </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="FromDate" ClientIDMode="Static" type="text" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-5">
                                    <div class="form-group row">
                                        <label for="ToDate" class="col-sm-4 col-form-label text-end">To Date: </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="ToDate" ClientIDMode="Static" type="text" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div style="padding-top: 10px;"></div>

                            <div class="row justify-content-center">
                                <div class="col-md-5">
                                    <div class="form-group row">
                                        <label for="EmployeeIdSelect" class="col-sm-4 col-form-label text-end">Employee: </label>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" ID="EmployeeIdSelect" ClientIDMode="Static" class="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <!-- Empty column to keep the 2x2 alignment balanced -->
                                </div>
                            </div>

                            <div class="row mt-2">
                                <div class="col-md-12 text-center">
                                    <button type="button" id="btnSearch" class="btn btnMyDesignSearch btn-sm" onclick="GetReportData()">
                                        <i class="fa fa-search-plus"></i>&nbsp; Search
                                    </button>
                                    <button type="button" class="btn btnMyDesignReset btn-sm" onclick="ResetFilters()">
                                        <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset
                                    </button>
                                </div>
                            </div>

                            <div style="padding-top:15px;"></div>

                            <!-- Report Output Wrapper -->
                            <div id="MainGradeDiv" style="display:none;" class="report-card-container">
                                <div class="row align-items-center mb-3">
                                    <div class="col-md-12 text-end">
                                        <!-- Export Button positioned at top right -->
                                        <button type="button" id="btnExport" class="btn btn-success btn-sm" onclick="exportToExcel()">
                                            <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                                        </button>
                                    </div>
                                </div>

                                <!-- Grid Data Table -->
                                <div class="table-responsive">
                                    <table id="dtTble" class="excel-style-table">
                                        <thead>
                                            <!-- Title Row matching report name -->
                                            <tr style="background-color: #ffffff !important; font-size: 16px; font-weight: bold;">
                                                <th colspan="8" style="color: #0000ff !important; background-color: #ffffff !important; text-align: center; border: 1px solid #BFBFBF !important;">Salesman Wise Customer Coverage & DCR Summary Sheet</th>
                                            </tr>
                                            <tr>
                                                <th style="width: 5%;">#SL No</th>
                                                <th style="width: 10%;">Salesman ID</th>
                                                <th style="width: 20%;">Salesman Name</th>
                                                <th style="width: 15%;">Total Assigned Customers</th>
                                                <th style="width: 15%;">Customers Visited (DCR Done)</th>
                                                <th style="width: 15%;">Customers Missed (No DCR)</th>
                                                <th style="width: 10%;">Coverage %</th>
                                                <th style="width: 15%;">Total Sales Amount (BDT)</th>
                                            </tr>
                                        </thead>
                                        <tbody id="treeGridBody"></tbody>
                                        <tfoot id="treeGridFooter"></tfoot>
                                    </table>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Client-side Scripts -->
    <script type="text/javascript">
        $(function () {
            // Initialize select2
            $('.mySelect2').select2({
                theme: 'bootstrap4',
                width: '100%'
            });

            // Initialize pickadate datepicker
            $('.datepicker').pickadate({
                selectMonths: true,
                selectYears: true
            });
        });

        // ASP.NET AJAX client lifecycle pageLoad (runs on initial load and async postbacks)
        function pageLoad() {
            $('.datepicker').pickadate({
                selectMonths: true,
                selectYears: true
            });
            $('.mySelect2').select2({
                theme: 'bootstrap4',
                width: '100%'
            });
        }

        function formatDisplayDate(date) {
            var day = date.getDate();
            var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var month = months[date.getMonth()];
            var year = date.getFullYear();
            return (day < 10 ? "0" + day : day) + " " + month + ", " + year;
        }

        function getDefaultFromDate() {
            var d = new Date();
            var firstDay = new Date(d.getFullYear(), d.getMonth(), 1);
            return formatDisplayDate(firstDay);
        }

        function getDefaultToDate() {
            return formatDisplayDate(new Date());
        }

        function ResetFilters() {
            $('#FromDate').val(getDefaultFromDate());
            $('#ToDate').val(getDefaultToDate());
            $('#EmployeeIdSelect').val('0').trigger('change');
            
            $('#MainGradeDiv').hide();
        }

        function formatNumber(num) {
            return parseFloat(num).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
        }

        function GetReportData() {
            var startDate = $('#FromDate').val();
            var endDate = $('#ToDate').val();
            var empInfoId = $('#EmployeeIdSelect').val();

            if (startDate === "") {
                alert("Please select a From Date.");
                return;
            }
            if (endDate === "") {
                alert("Please select a To Date.");
                return;
            }

            var urlpath = 'SalesmanCustomerVisitSummeryReport.aspx/GetSalesmanCustomerCoverageSummaryData';
            $.ajax({
                url: urlpath,
                dataType: 'json',
                data: JSON.stringify({
                    "startDate": startDate,
                    "endDate": endDate,
                    "empInfoId": parseInt(empInfoId) || 0
                }),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                async: true,
                beforeSend: function () {
                    $("#coverScreen").show();
                    $('#MainGradeDiv').hide();
                },
                success: function (data) {
                    var result = JSON.parse(data.d);
                    
                    if (result.length === 0) {
                        var emptyRowHtml = "<tr><td colspan='8' style='text-align:center;font-weight:bold;color:#721c24;background-color:#f8d7da;'>No data available for the selected criteria</td></tr>";
                        $('#treeGridBody').html(emptyRowHtml);
                        $('#treeGridFooter').html('');
                        $('#MainGradeDiv').show();
                        return;
                    }

                    var rowHtml = "";
                    var totalAssigned = 0;
                    var totalVisited = 0;
                    var totalMissed = 0;
                    var totalCoverageSum = 0;
                    var totalSalesAmt = 0;

                    for (var i = 0; i < result.length; i++) {
                        var row = result[i];
                        
                        var slNo = i + 1;
                        var salesmanId = row["Salesman ID"] || '';
                        var salesmanName = row["Salesman Name"] || '';
                        var assigned = parseInt(row["Total Assigned Customers"]) || 0;
                        var visited = parseInt(row["Customers Visited (DCR Done)"]) || 0;
                        var missed = parseInt(row["Customers Missed (No DCR)"]) || 0;
                        var coverage = parseFloat(row["Coverage %"]) || 0.0;
                        var salesAmt = parseFloat(row["Total Sales Amount (BDT)"]) || 0.0;

                        totalAssigned += assigned;
                        totalVisited += visited;
                        totalMissed += missed;
                        totalCoverageSum += coverage;
                        totalSalesAmt += salesAmt;

                        rowHtml += "<tr>";
                        rowHtml += "<td class='text-center'>" + slNo + "</td>";
                        rowHtml += "<td class='text-center'>" + salesmanId + "</td>";
                        rowHtml += "<td class='text-left'>" + salesmanName + "</td>";
                        rowHtml += "<td class='text-center'>" + assigned + "</td>";
                        rowHtml += "<td class='text-center'>" + visited + "</td>";
                        rowHtml += "<td class='text-center'>" + missed + "</td>";
                        rowHtml += "<td class='text-right'>" + coverage.toFixed(1) + "%</td>";
                        rowHtml += "<td class='text-right'>" + formatNumber(salesAmt) + "</td>";
                        rowHtml += "</tr>";
                    }

                    $('#treeGridBody').html(rowHtml);

                    var avgCoverage = result.length > 0 ? (totalCoverageSum / result.length) : 0.0;

                    // Add summary row in footer (Total / Average)
                    var footerHtml = "<tr class='total-row'>";
                    footerHtml += "<td colspan='3' class='text-left'>Total / Average</td>";
                    footerHtml += "<td class='text-center'>" + totalAssigned + "</td>";
                    footerHtml += "<td class='text-center'>" + totalVisited + "</td>";
                    footerHtml += "<td class='text-center'>" + totalMissed + "</td>";
                    footerHtml += "<td class='text-right'>" + avgCoverage.toFixed(1) + "%</td>";
                    footerHtml += "<td class='text-right'>" + formatNumber(totalSalesAmt) + "</td>";
                    footerHtml += "</tr>";

                    $('#treeGridFooter').html(footerHtml);

                    // Display the report area
                    $('#MainGradeDiv').show();
                },
                error: function(err) {
                    alert("Error loading report data. Please try again.");
                },
                complete: function () {
                    $("#coverScreen").hide();
                }
            });
        }

        // Export To Excel Method
        function exportToExcel() {
            var htmlContent = $('#MainGradeDiv').html();
            if ($('#MainGradeDiv').css('display') === 'none' || $('#treeGridBody tr').length === 0) {
                alert("Please load the report data first before exporting.");
                return;
            }
            var file = new Blob([htmlContent], { type: "application/vnd.ms-excel" });
            var url = URL.createObjectURL(file);
            var startDate = $('#FromDate').val();
            var endDate = $('#ToDate').val();
            var filename = "Salesman_Wise_Customer_Coverage_DCR_Summary_Sheet_" + startDate.replace(/,?\s+/g, '_') + "_to_" + endDate.replace(/,?\s+/g, '_') + ".xls";
            
            var a = $("<a />", {
                href: url,
                download: filename
            }).appendTo("body");
            
            a.get(0).click();
            
            setTimeout(function() {
                a.remove();
                URL.revokeObjectURL(url);
            }, 100);
        }
    </script>
</asp:Content>
