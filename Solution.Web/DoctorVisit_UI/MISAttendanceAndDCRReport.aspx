<%@ Page Title="MIS Attendance & DCR Report" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="MISAttendanceAndDCRReport.aspx.cs" Inherits="DoctorVisit_UI_MISAttendanceAndDCRReport" %>

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
            text-align: center;
            vertical-align: middle;
            color: #000000;
        }
        
        .excel-style-table th {
            background-color: #2F5597 !important;
            color: #ffffff !important;
            font-weight: bold;
            border: 1px solid #BFBFBF !important;
        }
        
        .excel-style-table thead tr.header-row-primary th {
            font-size: 15px;
            height: 30px;
        }
        
        .excel-style-table thead tr.header-row-secondary th {
            font-size: 13px;
            background-color: #2F5597 !important;
            height: 25px;
        }
        
        .weekly-off {
            background-color: #F8D7DA !important; /* Premium light pink background */
            color: #721C24 !important; /* Bold red text */
            font-weight: bold;
        }
        
        .weekly-off td {
            background-color: #F8D7DA !important;
            color: #721C24 !important;
        }
        
        .total-row {
            background-color: #D9E1F2 !important; /* Light blue/gray background matching the image */
            font-weight: bold;
        }
        
        .total-row td {
            background-color: #D9E1F2 !important;
            font-weight: bold;
            border: 1px solid #BFBFBF !important;
        }

        /* Employee Info Grid Table */
        .report-info-table {
            border-collapse: collapse;
            width: 100%;
            margin-top: 15px;
            margin-bottom: 20px;
            font-family: 'Calibri', 'Arial', sans-serif;
            font-size: 14px;
        }
        
        .report-info-table td {
            padding: 6px 12px;
            border: 1px solid #BFBFBF !important;
            vertical-align: middle;
            color: #000000;
        }
        
        .report-info-table td.info-label {
            font-weight: bold;
            background-color: #F2F2F2;
            width: 15%;
        }
        
        .report-info-table td.info-value {
            width: 35%;
            background-color: #ffffff;
        }

        /* Header Info styling */
        .report-header-title {
            font-family: 'Calibri', 'Arial', sans-serif;
            color: #2F5597;
            font-weight: bold;
            font-size: 24px;
            margin: 0;
            padding: 0;
            text-align: left;
        }
        
        .report-header-subtitle {
            font-family: 'Calibri', 'Arial', sans-serif;
            color: #333333;
            font-weight: bold;
            font-size: 18px;
            margin: 5px 0 15px 0;
            padding: 0;
            text-align: left;
        }

        .text-right {
            text-align: right !important;
        }
        .text-center {
            text-align: center !important;
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
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> MIS Attendance & DCR Report</div>
                <div class="ms-auto">
                    <div class="btn-group"></div>
                </div>
            </div>
            <!--end breadcrumb-->

            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <!-- Filter Section -->
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group row">
                                        <label for="FromDate" class="col-sm-4 col-form-label">From Date: </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="FromDate" ClientIDMode="Static" type="text" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group row">
                                        <label for="ToDate" class="col-sm-4 col-form-label">To Date: </label>
                                        <div class="col-sm-8">
                                            <asp:TextBox runat="server" ID="ToDate" ClientIDMode="Static" type="text" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Date"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="form-group row">
                                        <label for="EmployeeIdSelect" class="col-sm-4 col-form-label">Employee: </label>
                                        <div class="col-sm-8">
                                            <asp:DropDownList runat="server" ID="EmployeeIdSelect" ClientIDMode="Static" class="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>
                                        </div>
                                    </div>
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

                            <div style="padding-top: 10px;"></div>

                            <!-- Report Output Wrapper -->
                            <div id="MainGradeDiv" style="display:none;" class="report-card-container">
                                <div class="row align-items-center mb-3">
                                    <div class="col-md-9">
                                        <!-- Company and Report Titles -->
                                        <div class="report-header-title">Unicorn Healthcare Solution Ltd.</div>
                                        <div id="lblReportSubtitle" class="report-header-subtitle" style="margin-bottom:0;">Monthly Expense Claim of June, 2026</div>
                                    </div>
                                    <div class="col-md-3 text-end">
                                        <button type="button" id="btnExport" class="btn btn-excel-green btn-sm" style="background-color: #28a745; border-color: #28a745; color: #fff; font-weight: bold;" onclick="exportToExcel()">
                                            Export to Excel
                                        </button>
                                    </div>
                                </div>

                                <!-- Employee Details Info Table -->
                                <table class="report-info-table">
                                    <tr>
                                        <td class="info-label">Name:</td>
                                        <td class="info-value" id="lblEmpName"></td>
                                        <td class="info-label">ID:</td>
                                        <td class="info-value" id="lblEmpID"></td>
                                    </tr>
                                    <tr>
                                        <td class="info-label">Designation:</td>
                                        <td class="info-value" id="lblDesignation"></td>
                                        <td class="info-label">Customer Coverage:</td>
                                        <td class="info-value" id="lblCustomerCoverage"></td>
                                    </tr>
                                </table>

                                <!-- Grid Data Table -->
                                <div class="table-responsive">
                                    <table id="dtTble" class="excel-style-table">
                                        <thead>
                                            <tr class="header-row-primary">
                                                <th rowspan="2" style="width: 12%;">Date & Day</th>
                                                <th colspan="2" style="width: 24%;">Morning Working Time</th>
                                                <th colspan="2" style="width: 24%;">Evening Working Time</th>
                                                <th rowspan="2" style="width: 10%;">DCR Count</th>
                                                <th rowspan="2" style="width: 18%;">Visit with</th>
                                                <th rowspan="2" style="width: 12%;">Total Working Hour</th>
                                            </tr>
                                            <tr class="header-row-secondary">
                                                <th style="width: 12%;">Start</th>
                                                <th style="width: 12%;">End</th>
                                                <th style="width: 12%;">Start</th>
                                                <th style="width: 12%;">End</th>
                                            </tr>
                                        </thead>
                                        <tbody id="dtTableBody"></tbody>
                                        <tfoot id="dtTableFooter"></tfoot>
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
            $('#btnExport').hide();
        }

        function formatSeconds(totalSeconds) {
            var hours = Math.floor(totalSeconds / 3600);
            var minutes = Math.floor((totalSeconds % 3600) / 60);
            var seconds = totalSeconds % 60;
            
            return (hours < 10 ? "0" + hours : hours) + ":" + 
                   (minutes < 10 ? "0" + minutes : minutes) + ":" + 
                   (seconds < 10 ? "0" + seconds : seconds);
        }

        function getMonthName(monthNumber) {
            var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            return months[parseInt(monthNumber) - 1];
        }

        function parseDateRobust(dateStr) {
            if (!dateStr) return new Date(NaN);
            
            dateStr = dateStr.trim();
            
            var parsed = new Date(dateStr);
            if (!isNaN(parsed.getTime())) return parsed;
            
            var parts = dateStr.split(/[\s,\-]+/);
            if (parts.length >= 3) {
                var day = parseInt(parts[0]);
                var monthStr = parts[1].toLowerCase();
                var year = parseInt(parts[2]);
                
                var months = ["jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"];
                var fullMonths = ["january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december"];
                
                var month = -1;
                for (var i = 0; i < 12; i++) {
                    if (monthStr.indexOf(months[i]) === 0 || monthStr.indexOf(fullMonths[i]) === 0) {
                        month = i;
                        break;
                    }
                }
                
                if (month !== -1 && !isNaN(day) && !isNaN(year)) {
                    return new Date(year, month, day);
                }
            }
            
            var slashParts = dateStr.split('/');
            if (slashParts.length === 3) {
                var d = parseInt(slashParts[0]);
                var m = parseInt(slashParts[1]) - 1;
                var y = parseInt(slashParts[2]);
                if (!isNaN(d) && !isNaN(m) && !isNaN(y)) {
                    return new Date(y, m, d);
                }
            }
            
            return new Date(NaN);
        }

        function getSubtitle(startDateStr, endDateStr) {
            var startD = parseDateRobust(startDateStr);
            var endD = parseDateRobust(endDateStr);
            
            if (isNaN(startD.getTime()) || isNaN(endD.getTime())) {
                return "MIS Attendance & DCR Report";
            }
            
            if (startD.getMonth() === endD.getMonth() && startD.getFullYear() === endD.getFullYear()) {
                return "Monthly Expense Claim of " + getMonthName(startD.getMonth() + 1) + ", " + startD.getFullYear();
            } else {
                return "MIS Attendance & DCR Report from " + startDateStr + " to " + endDateStr;
            }
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
            if (empInfoId === "" || empInfoId === "0") {
                alert("Please select an Employee.");
                return;
            }

            var urlpath = 'MISAttendanceAndDCRReport.aspx/GetMISAttendanceAndDCRReportData';
            $.ajax({
                url: urlpath,
                dataType: 'json',
                data: JSON.stringify({
                    "startDate": startDate,
                    "endDate": endDate,
                    "empInfoId": parseInt(empInfoId)
                }),
                type: "POST",
                contentType: "application/json;charset=utf-8",
                async: true,
                beforeSend: function () {
                    $("#coverScreen").show();
                    $('#MainGradeDiv').hide();
                    $('#btnExport').hide();
                },
                success: function (data) {
                    var result = JSON.parse(data.d);
                    
                    if (result.length === 0) {
                        // Clear employee details
                        $('#lblEmpName').text('');
                        $('#lblEmpID').text('');
                        $('#lblBaseHQ').text('');
                        $('#lblDesignation').text('');
                        $('#lblCustomerCoverage').text('0%');
                        $('#lblZoneCode').text('');

                        // Update Subtitle dynamically
                        $('#lblReportSubtitle').text(getSubtitle(startDate, endDate));

                        // Render empty row in body and clear footer
                        var emptyRowHtml = "<tr><td colspan='8' style='text-align:center;font-weight:bold;color:#721c24;background-color:#f8d7da;'>No data available for the selected criteria</td></tr>";
                        $('#dtTableBody').html(emptyRowHtml);
                        $('#dtTableFooter').html('');

                        // Show table but hide export button
                        $('#MainGradeDiv').show();
                        $('#btnExport').hide();
                        return;
                    }

                    // Populate Employee general details from first row
                    $('#lblEmpName').text(result[0].EmpName || '');
                    $('#lblEmpID').text(result[0].EmpID || '');
                    $('#lblBaseHQ').text(result[0].BaseHQ || '');
                    $('#lblDesignation').text(result[0].Designation || '');
                    $('#lblCustomerCoverage').text(result[0].CustomerCoverage || '0%');
                    $('#lblZoneCode').text(result[0].ZoneCode || '');

                    // Update Subtitle dynamically
                    $('#lblReportSubtitle').text(getSubtitle(startDate, endDate));

                    var rowHtml = "";
                    var totalDcrCount = 0;
                    var totalSecondsSum = 0;

                    for (var i = 0; i < result.length; i++) {
                        var row = result[i];
                        var isWeeklyOff = (row.Morning_Start === "Weekly Off");
                        var rowClass = isWeeklyOff ? "class='weekly-off'" : "";

                        rowHtml += "<tr " + rowClass + ">";
                        rowHtml += "<td>" + (row["Date & Day"] || '') + "</td>";

                        if (isWeeklyOff) {
                            rowHtml += "<td>Weekly Off</td>";
                            rowHtml += "<td>Weekly Off</td>";
                            rowHtml += "<td>Weekly Off</td>";
                            rowHtml += "<td>Weekly Off</td>";
                            rowHtml += "<td>Weekly Off</td>";
                            rowHtml += "<td>Weekly Off</td>";
                            rowHtml += "<td>Weekly Off</td>";
                        } else {
                            rowHtml += "<td>" + (row.Morning_Start || '') + "</td>";
                            rowHtml += "<td>" + (row.Morning_End || '') + "</td>";
                            rowHtml += "<td>" + (row.Evening_Start || '') + "</td>";
                            rowHtml += "<td>" + (row.Evening_End || '') + "</td>";
                            
                            var dcrCount = parseInt(row["DCR Count"]) || 0;
                            totalDcrCount += dcrCount;
                            rowHtml += "<td>" + dcrCount + "</td>";
                            
                            rowHtml += "<td>" + (row["Visit with"] || 'Self') + "</td>";
                            rowHtml += "<td>" + (row["Total Working Hour"] || '00:00:00') + "</td>";

                            totalSecondsSum += parseInt(row.TotalSeconds) || 0;
                        }
                        
                        rowHtml += "</tr>";
                    }

                    $('#dtTableBody').html(rowHtml);

                    // Add summary row in footer
                    var footerHtml = "<tr class='total-row'>";
                    footerHtml += "<td colspan='5' class='text-center'>Total</td>";
                    footerHtml += "<td>" + totalDcrCount + "</td>";
                    footerHtml += "<td></td>";
                    footerHtml += "<td class='text-right'>" + formatSeconds(totalSecondsSum) + "</td>";
                    footerHtml += "</tr>";

                    $('#dtTableFooter').html(footerHtml);

                    // Display the report area and export button
                    $('#MainGradeDiv').show();
                    $('#btnExport').show();
                },
                error: function(err) {
                    alert("Error loading report data. Please try again.");
                },
                complete: function () {
                    $("#coverScreen").hide();
                }
            });
        }

        function exportToExcel() {
            var htmlContent = $('#MainGradeDiv').html();
            if ($('#MainGradeDiv').css('display') === 'none' || $('#dtTableBody tr').length === 0) {
                alert("Please load the report data first before exporting.");
                return;
            }
            var file = new Blob([htmlContent], { type: "application/vnd.ms-excel" });
            var url = URL.createObjectURL(file);
            var empId = $('#lblEmpID').text() || 'Employee';
            var startDate = $('#FromDate').val();
            var endDate = $('#ToDate').val();
            var filename = "MIS_Attendance_And_DCR_Report_" + empId + "_" + startDate.replace(/,?\s+/g, '_') + "_to_" + endDate.replace(/,?\s+/g, '_') + ".xls";
            
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
