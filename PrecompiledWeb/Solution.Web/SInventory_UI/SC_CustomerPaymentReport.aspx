<%@ page title="" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_SC_CustomerPaymentReport, App_Web_4colmdem" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
 <%@ Register Src="~/SInventory_UI/IVMarketStructureInvoSearchReport.ascx" TagPrefix="uc1" TagName="IVMarketStructure" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <style type="text/css">
        .button-padding-right {
            margin-right: 5px; 
        }  
        .SelectchkChoice label {
            padding-left: 4px;
            font-weight: bold;
        }
        .ssss {
            font-size: 13px;
            font-weight: bold;
        }

        /* Scrollable Freeze Container */
        .table-responsive-custom {
            width: 100%;
            max-height: 600px;
            overflow: auto;
            position: relative;
            border: 1px solid #dee2e6;
            margin-top: 10px;
            background-color: #fff;
        }

        #ContentPlaceHolder1_loadGridView,
        #ContentPlaceHolder1_summaryGridView {
            border-collapse: separate !important;
            border-spacing: 0;
            width: 100%;
            margin-bottom: 0;
        }

        /* General Cell Styling & Prevent Text Overflow Bleed */
        #ContentPlaceHolder1_loadGridView th,
        #ContentPlaceHolder1_loadGridView td,
        #ContentPlaceHolder1_summaryGridView th,
        #ContentPlaceHolder1_summaryGridView td {
            box-sizing: border-box !important;
            white-space: nowrap !important;
            vertical-align: middle !important;
            padding: 6px 8px;
            overflow: hidden !important;
            text-overflow: ellipsis !important;
        }

        /* 1. Sticky Header */
        #ContentPlaceHolder1_loadGridView thead th,
        #ContentPlaceHolder1_summaryGridView thead th {
            position: sticky;
            top: 0;
            background-color: #1A7343 !important;
            color: #ffffff !important;
            text-align: center;
            padding: 8px 10px;
            z-index: 20;
            border-bottom: 2px solid #145934;
        }

        /* 2. Sticky Footer */
        #ContentPlaceHolder1_loadGridView tfoot td,
        #ContentPlaceHolder1_summaryGridView tfoot td {
            position: sticky;
            bottom: 0;
            background-color: #E9ECEF !important;
            color: #000000 !important;
            font-weight: bold;
            padding: 8px 10px;
            border-top: 2px solid #6c757d;
            z-index: 20;
        }

        /* Default Body Row Backgrounds */
        #ContentPlaceHolder1_loadGridView tbody td,
        #ContentPlaceHolder1_summaryGridView tbody td {
            background-color: #ffffff;
            z-index: 1;
        }
        #ContentPlaceHolder1_loadGridView tbody tr:nth-child(even) td,
        #ContentPlaceHolder1_summaryGridView tbody tr:nth-child(even) td {
            background-color: #f8f9fa;
        }

        /* ==================== DETAILS GRID FROZEN COLUMNS (1 to 5) ==================== */
        /* Col 1: SL */
        #ContentPlaceHolder1_loadGridView th:nth-child(1),
        #ContentPlaceHolder1_loadGridView td:nth-child(1) {
            position: sticky;
            left: 0px;
            width: 50px !important;
            min-width: 50px !important;
            max-width: 50px !important;
            text-align: center;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 2: Sales Center */
        #ContentPlaceHolder1_loadGridView th:nth-child(2),
        #ContentPlaceHolder1_loadGridView td:nth-child(2) {
            position: sticky;
            left: 50px;
            width: 90px !important;
            min-width: 90px !important;
            max-width: 90px !important;
            text-align: center;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 3: Sales Center Name */
        #ContentPlaceHolder1_loadGridView th:nth-child(3),
        #ContentPlaceHolder1_loadGridView td:nth-child(3) {
            position: sticky;
            left: 140px;
            width: 180px !important;
            min-width: 180px !important;
            max-width: 180px !important;
            white-space: normal !important;
            word-wrap: break-word !important;
            overflow-wrap: break-word !important;
            line-height: 1.3 !important;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 4: Customer ID */
        #ContentPlaceHolder1_loadGridView th:nth-child(4),
        #ContentPlaceHolder1_loadGridView td:nth-child(4) {
            position: sticky;
            left: 320px;
            width: 100px !important;
            min-width: 100px !important;
            max-width: 100px !important;
            text-align: center;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 5: Customer Name */
        #ContentPlaceHolder1_loadGridView th:nth-child(5),
        #ContentPlaceHolder1_loadGridView td:nth-child(5) {
            position: sticky;
            left: 420px;
            width: 260px !important;
            min-width: 260px !important;
            max-width: 260px !important;
            white-space: normal !important;
            word-wrap: break-word !important;
            overflow-wrap: break-word !important;
            line-height: 1.3 !important;
            box-shadow: 3px 0 5px -1px rgba(0,0,0,0.18);
        }

        /* Opaque Background for Details Frozen Body Rows */
        #ContentPlaceHolder1_loadGridView tbody td:nth-child(1),
        #ContentPlaceHolder1_loadGridView tbody td:nth-child(2),
        #ContentPlaceHolder1_loadGridView tbody td:nth-child(3),
        #ContentPlaceHolder1_loadGridView tbody td:nth-child(4),
        #ContentPlaceHolder1_loadGridView tbody td:nth-child(5) {
            background-color: #ffffff !important;
            z-index: 10 !important;
        }
        #ContentPlaceHolder1_loadGridView tbody tr:nth-child(even) td:nth-child(1),
        #ContentPlaceHolder1_loadGridView tbody tr:nth-child(even) td:nth-child(2),
        #ContentPlaceHolder1_loadGridView tbody tr:nth-child(even) td:nth-child(3),
        #ContentPlaceHolder1_loadGridView tbody tr:nth-child(even) td:nth-child(4),
        #ContentPlaceHolder1_loadGridView tbody tr:nth-child(even) td:nth-child(5) {
            background-color: #f8f9fa !important;
            z-index: 10 !important;
        }

        /* Intersecting Sticky Corners for Details Header & Footer */
        #ContentPlaceHolder1_loadGridView thead th:nth-child(1),
        #ContentPlaceHolder1_loadGridView thead th:nth-child(2),
        #ContentPlaceHolder1_loadGridView thead th:nth-child(3),
        #ContentPlaceHolder1_loadGridView thead th:nth-child(4),
        #ContentPlaceHolder1_loadGridView thead th:nth-child(5) {
            background-color: #1A7343 !important;
            color: #ffffff !important;
            z-index: 30 !important;
        }
        #ContentPlaceHolder1_loadGridView tfoot td:nth-child(1),
        #ContentPlaceHolder1_loadGridView tfoot td:nth-child(2),
        #ContentPlaceHolder1_loadGridView tfoot td:nth-child(3),
        #ContentPlaceHolder1_loadGridView tfoot td:nth-child(4),
        #ContentPlaceHolder1_loadGridView tfoot td:nth-child(5) {
            background-color: #E9ECEF !important;
            z-index: 30 !important;
        }

        /* ==================== SUMMARY GRID FROZEN COLUMNS (1 to 4) ==================== */
        /* Col 1: SL */
        #ContentPlaceHolder1_summaryGridView th:nth-child(1),
        #ContentPlaceHolder1_summaryGridView td:nth-child(1) {
            position: sticky;
            left: 0px;
            width: 50px !important;
            min-width: 50px !important;
            max-width: 50px !important;
            text-align: center;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 2: Payment Date */
        #ContentPlaceHolder1_summaryGridView th:nth-child(2),
        #ContentPlaceHolder1_summaryGridView td:nth-child(2) {
            position: sticky;
            left: 50px;
            width: 110px !important;
            min-width: 110px !important;
            max-width: 110px !important;
            text-align: center;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 3: Customer ID */
        #ContentPlaceHolder1_summaryGridView th:nth-child(3),
        #ContentPlaceHolder1_summaryGridView td:nth-child(3) {
            position: sticky;
            left: 160px;
            width: 100px !important;
            min-width: 100px !important;
            max-width: 100px !important;
            text-align: center;
            box-shadow: 1px 0 0 #dee2e6;
        }

        /* Col 4: Customer Name */
        #ContentPlaceHolder1_summaryGridView th:nth-child(4),
        #ContentPlaceHolder1_summaryGridView td:nth-child(4) {
            position: sticky;
            left: 260px;
            width: 280px !important;
            min-width: 280px !important;
            max-width: 280px !important;
            white-space: normal !important;
            word-wrap: break-word !important;
            overflow-wrap: break-word !important;
            line-height: 1.3 !important;
            box-shadow: 3px 0 5px -1px rgba(0,0,0,0.18);
        }

        /* Opaque Background for Summary Frozen Body Rows */
        #ContentPlaceHolder1_summaryGridView tbody td:nth-child(1),
        #ContentPlaceHolder1_summaryGridView tbody td:nth-child(2),
        #ContentPlaceHolder1_summaryGridView tbody td:nth-child(3),
        #ContentPlaceHolder1_summaryGridView tbody td:nth-child(4) {
            background-color: #ffffff !important;
            z-index: 10 !important;
        }
        #ContentPlaceHolder1_summaryGridView tbody tr:nth-child(even) td:nth-child(1),
        #ContentPlaceHolder1_summaryGridView tbody tr:nth-child(even) td:nth-child(2),
        #ContentPlaceHolder1_summaryGridView tbody tr:nth-child(even) td:nth-child(3),
        #ContentPlaceHolder1_summaryGridView tbody tr:nth-child(even) td:nth-child(4) {
            background-color: #f8f9fa !important;
            z-index: 10 !important;
        }

        /* Intersecting Sticky Corners for Summary Header & Footer */
        #ContentPlaceHolder1_summaryGridView thead th:nth-child(1),
        #ContentPlaceHolder1_summaryGridView thead th:nth-child(2),
        #ContentPlaceHolder1_summaryGridView thead th:nth-child(3),
        #ContentPlaceHolder1_summaryGridView thead th:nth-child(4) {
            background-color: #1A7343 !important;
            color: #ffffff !important;
            z-index: 30 !important;
        }
        #ContentPlaceHolder1_summaryGridView tfoot td:nth-child(1),
        #ContentPlaceHolder1_summaryGridView tfoot td:nth-child(2),
        #ContentPlaceHolder1_summaryGridView tfoot td:nth-child(3),
        #ContentPlaceHolder1_summaryGridView tfoot td:nth-child(4) {
            background-color: #E9ECEF !important;
            z-index: 30 !important;
        }

        /* DataTables Controls Layout */
        .dataTables_wrapper .dataTables_length {
            margin-bottom: 10px;
            font-weight: 500;
        }
        .dataTables_wrapper .dataTables_filter {
            margin-bottom: 10px;
        }
        .dataTables_wrapper .dataTables_filter input {
            border: 1px solid #ced4da;
            border-radius: 4px;
            padding: 4px 8px;
            margin-left: 5px;
        }
        .dataTables_wrapper .dataTables_info,
        .dataTables_wrapper .dataTables_paginate {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    
    
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Customer Payment  Report</div>
                
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
         <%--   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>--%>
                       <%-- <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                    <ProgressTemplate>
                       
                        <div class="divWaiting">
                            <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>--%>

                       <script type="text/javascript">
                           function initUIPlugins() {
                               $('.mySelect2').select2({
                                   theme: 'bootstrap4',
                                   width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                   placeholder: $(this).data('placeholder'),
                                   allowClear: Boolean($(this).data('allow-clear')),
                               });
                               $('.datepicker').pickadate({
                                   selectMonths: true,
                                   selectYears: true
                               });

                               // Helper to parse numbers
                               var numVal = function (i) {
                                   return typeof i === 'string' ?
                                       i.replace(/[\$,]/g, '') * 1 :
                                       typeof i === 'number' ? i : 0;
                               };

                               var formatNumber = function (n) {
                                   return n.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                               };

                               // 1. Initialize Details DataTable
                               var detailsTableId = '#<%= loadGridView.ClientID %>';
                               if ($.fn.DataTable && $(detailsTableId).length > 0) {
                                   if ($.fn.DataTable.isDataTable(detailsTableId)) {
                                       $(detailsTableId).DataTable().destroy();
                                   }

                                   if ($(detailsTableId + ' tbody tr').length > 0 && !$(detailsTableId + ' tbody tr td').hasClass('dataTables_empty')) {
                                       $(detailsTableId).DataTable({
                                           paging: true,
                                           pageLength: 25,
                                           lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                                           order: [],
                                           autoWidth: false,
                                           language: {
                                               search: "_INPUT_",
                                               searchPlaceholder: "Search in details...",
                                               lengthMenu: "Show _MENU_ entries"
                                           },
                                           drawCallback: function (settings) {
                                               var api = this.api();
                                               var totalInv = api.column(15, { page: 'current' }).data().reduce(function (a, b) { return numVal(a) + numVal(b); }, 0);
                                               var totalPaid = api.column(16, { page: 'current' }).data().reduce(function (a, b) { return numVal(a) + numVal(b); }, 0);
                                               var totalDisc = api.column(17, { page: 'current' }).data().reduce(function (a, b) { return numVal(a) + numVal(b); }, 0);
                                               var totalAit = api.column(18, { page: 'current' }).data().reduce(function (a, b) { return numVal(a) + numVal(b); }, 0);

                                               $(api.column(0).footer()).html('Total');
                                               $(api.column(15).footer()).html(formatNumber(totalInv)).css('text-align', 'right');
                                               $(api.column(16).footer()).html(formatNumber(totalPaid)).css('text-align', 'right');
                                               $(api.column(17).footer()).html(formatNumber(totalDisc)).css('text-align', 'right');
                                               $(api.column(18).footer()).html(formatNumber(totalAit)).css('text-align', 'right');

                                               $(this).find('tbody td').each(function () {
                                                   var text = $(this).text().trim();
                                                   if (text && text !== '&nbsp;' && !$(this).attr('title')) {
                                                       $(this).attr('title', text);
                                                   }
                                               });
                                           }
                                       });
                                   }
                               }

                               // 2. Initialize Summary DataTable
                               var summaryTableId = '#<%= summaryGridView.ClientID %>';
                               if ($.fn.DataTable && $(summaryTableId).length > 0) {
                                   if ($.fn.DataTable.isDataTable(summaryTableId)) {
                                       $(summaryTableId).DataTable().destroy();
                                   }

                                   if ($(summaryTableId + ' tbody tr').length > 0 && !$(summaryTableId + ' tbody tr td').hasClass('dataTables_empty')) {
                                       $(summaryTableId).DataTable({
                                           paging: true,
                                           pageLength: 25,
                                           lengthMenu: [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]],
                                           order: [],
                                           autoWidth: false,
                                           language: {
                                               search: "_INPUT_",
                                               searchPlaceholder: "Search in summary...",
                                               lengthMenu: "Show _MENU_ entries"
                                           },
                                           drawCallback: function (settings) {
                                               var api = this.api();
                                               var totalPaid = api.column(6, { page: 'current' }).data().reduce(function (a, b) { return numVal(a) + numVal(b); }, 0);

                                               $(api.column(0).footer()).html('Total');
                                               $(api.column(6).footer()).html(formatNumber(totalPaid)).css('text-align', 'right');

                                               $(this).find('tbody td').each(function () {
                                                   var text = $(this).text().trim();
                                                   if (text && text !== '&nbsp;' && !$(this).attr('title')) {
                                                       $(this).attr('title', text);
                                                   }
                                               });
                                           }
                                       });
                                   }
                               }
                           }

                           function pageLoad() {
                               initUIPlugins();
                           }

                           $(document).ready(function () {
                               initUIPlugins();
                           });

                             // Print / Save to PDF function
                             function printReportDiv(tableOrDivId, reportTitle) {
                                 var containerElem = document.getElementById(tableOrDivId);
                                 if (!containerElem) return;

                                 var tableElem = containerElem.tagName === 'TABLE' ? containerElem : containerElem.querySelector('table');
                                 if (!tableElem) return;

                                 var tableId = tableElem.id;
                                 var isSummary = (reportTitle.indexOf('Summary') >= 0 || tableId.indexOf('summary') >= 0);

                                 // 1. Handle DataTable pagination: temporarily show all rows for printing
                                 var dt = ($.fn.DataTable && $.fn.DataTable.isDataTable('#' + tableId)) ? $('#' + tableId).DataTable() : null;
                                 var prevPageLen = dt ? dt.page.len() : 25;
                                 if (dt) {
                                     dt.page.len(-1).draw(false);
                                 }

                                 // 2. Extract Filter values from UI controls
                                 var scText = $('#<%= dcDropDownList1.ClientID %> option:selected').text();
                                 if (!scText || scText.indexOf('Select') >= 0) scText = 'All Sales Centers';

                                 var fromDate = $('#<%= InvoiceDateTextBox.ClientID %>').val();
                                 var toDate = $('#<%= todateTextBox.ClientID %>').val();
                                 var dateRange = (fromDate && toDate) ? (fromDate + ' to ' + toDate) : 'All Dates';

                                 var customer = $('#<%= ddlCustomer.ClientID %> option:selected').text();
                                 if (!customer || customer.indexOf('Select') >= 0) customer = 'All Customers';

                                 var custType = $('#<%= ddlChemisType.ClientID %> option:selected').text();
                                 if (!custType || custType.indexOf('Select') >= 0) custType = 'All';

                                 var paymentBy = $('#<%= ddlPaymentBy.ClientID %> option:selected').text();
                                 if (!paymentBy || paymentBy.indexOf('Select') >= 0) paymentBy = 'All';

                                 var now = new Date();
                                 var printTime = now.toLocaleDateString('en-GB', { day: '2-digit', month: 'short', year: 'numeric' }) + ', ' + now.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: true });

                                 // 3. Build Fresh Clean Table HTML
                                 var tableHtml = '<table class="report-table">';
                                 
                                 // THEAD
                                 var headerRow = tableElem.querySelector('thead tr');
                                 if (headerRow) {
                                     tableHtml += '<thead><tr>';
                                     var ths = headerRow.querySelectorAll('th');
                                     for (var i = 0; i < ths.length; i++) {
                                         var cloneTh = ths[i].cloneNode(true);
                                         $(cloneTh).find('span, i, .dataTables_sizing').remove();
                                         var hText = cloneTh.innerText.trim();
                                         
                                         var alignClass = 'col-left';
                                         if (isSummary) {
                                             if (i === 0 || i === 1 || i === 2 || i === 4 || i === 7) alignClass = 'col-center';
                                             else if (i === 6) alignClass = 'col-right';
                                         } else {
                                             if (i === 0 || i === 1 || i === 3 || i === 6 || i === 7 || i === 8 || i === 9 || i === 10 || i === 11 || i === 12 || i === 13 || i === 19) alignClass = 'col-center';
                                             else if (i === 15 || i === 16 || i === 17 || i === 18) alignClass = 'col-right';
                                         }
                                         tableHtml += '<th class="' + alignClass + '">' + hText + '</th>';
                                     }
                                     tableHtml += '</tr></thead>';
                                 }

                                 // TBODY
                                 var bodyRows = tableElem.querySelectorAll('tbody tr');
                                 tableHtml += '<tbody>';
                                 var sl = 1;
                                 for (var r = 0; r < bodyRows.length; r++) {
                                     var row = bodyRows[r];
                                     if (row.classList.contains('dataTables_empty')) continue;

                                     tableHtml += '<tr>';
                                     var tds = row.querySelectorAll('td');
                                     for (var c = 0; c < tds.length; c++) {
                                         var cellText = tds[c].innerText.trim();
                                         if (c === 0) cellText = sl.toString();

                                         var alignClass = 'col-left';
                                         if (isSummary) {
                                             if (c === 0 || c === 1 || c === 2 || c === 4 || c === 7) alignClass = 'col-center';
                                             else if (c === 6) alignClass = 'col-right';
                                         } else {
                                             if (c === 0 || c === 1 || c === 3 || c === 6 || c === 7 || c === 8 || c === 9 || c === 10 || c === 11 || c === 12 || c === 13 || c === 19) alignClass = 'col-center';
                                             else if (c === 15 || c === 16 || c === 17 || c === 18) alignClass = 'col-right';
                                         }
                                         tableHtml += '<td class="' + alignClass + '">' + cellText + '</td>';
                                     }
                                     tableHtml += '</tr>';
                                     sl++;
                                 }
                                 tableHtml += '</tbody>';

                                 // TFOOT
                                 var footRow = tableElem.querySelector('tfoot tr');
                                 if (footRow) {
                                     tableHtml += '<tfoot><tr>';
                                     var fds = footRow.querySelectorAll('td');
                                     for (var f = 0; f < fds.length; f++) {
                                         var footText = fds[f].innerText.trim();
                                         var alignClass = 'col-left';
                                         if (isSummary) {
                                             if (f === 6) alignClass = 'col-right bold-cell';
                                             else if (f === 0) alignClass = 'col-center bold-cell';
                                             else alignClass = 'col-left bold-cell';
                                         } else {
                                             if (f === 15 || f === 16 || f === 17 || f === 18) alignClass = 'col-right bold-cell';
                                             else if (f === 0) alignClass = 'col-center bold-cell';
                                             else alignClass = 'col-left bold-cell';
                                         }
                                         tableHtml += '<td class="' + alignClass + '">' + footText + '</td>';
                                     }
                                     tableHtml += '</tr></tfoot>';
                                 }

                                 tableHtml += '</table>';

                                 // Restore DataTable page length on active screen
                                 if (dt) {
                                     dt.page.len(prevPageLen).draw(false);
                                 }

                                 // 4. Open Print Window
                                 var newWin = window.open('', '_blank', 'width=1100,height=750,scrollbars=yes');
                                 newWin.document.open();

                                 var html = '<!DOCTYPE html><html><head><meta charset="utf-8"/><title>' + reportTitle + '</title>';
                                 html += '<style>';
                                 html += '@page { size: landscape; margin: 10mm 8mm 12mm 8mm; }';
                                 html += '* { box-sizing: border-box; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }';
                                 html += 'body { font-family: "Segoe UI", Arial, Helvetica, sans-serif; font-size: 10px; color: #111; margin: 0; padding: 10px 15px; background: #fff; }';
                                 
                                 // Header Styles
                                 html += '.header-container { text-align: center; margin-bottom: 12px; }';
                                 html += '.company-title { font-size: 20px; font-weight: 800; color: #1A7343; text-transform: uppercase; margin: 0 0 2px 0; letter-spacing: 0.5px; }';
                                 html += '.report-title { font-size: 13.5px; font-weight: 700; color: #222; text-transform: uppercase; margin: 0 0 8px 0; }';
                                 
                                 // Meta Box
                                 html += '.meta-box { border: 1px solid #ccc; background-color: #fcfcfc; border-radius: 4px; padding: 6px 10px; margin-bottom: 10px; }';
                                 html += '.meta-table { width: 100%; border-collapse: collapse; font-size: 9.5px; }';
                                 html += '.meta-table td { border: none !important; padding: 3px 4px; vertical-align: middle; }';
                                 html += '.m-lbl { font-weight: bold; color: #333; }';
                                 html += '.m-val { color: #111; font-weight: 500; }';
                                 
                                 // Table Styles
                                 html += 'table.report-table { width: 100%; border-collapse: collapse; margin-top: 6px; font-size: 9.5px; table-layout: auto; }';
                                 html += 'table.report-table th, table.report-table td { border: 1px solid #777; padding: 5px 6px; vertical-align: middle; line-height: 1.25; }';
                                 html += 'table.report-table thead th { background-color: #1A7343 !important; color: #ffffff !important; font-weight: bold; border: 1px solid #145934; }';
                                 html += 'table.report-table tbody tr:nth-child(even) td { background-color: #f9fbf9; }';
                                 html += 'table.report-table tfoot td { background-color: #E9ECEF !important; font-weight: bold; border-top: 1.5pt solid #222; border-bottom: 1.5pt solid #222; }';
                                 
                                 // Alignment Helpers
                                 html += '.col-center { text-align: center !important; }';
                                 html += '.col-left { text-align: left !important; }';
                                 html += '.col-right { text-align: right !important; }';
                                 html += '.bold-cell { font-weight: bold !important; }';
                                 
                                 // Signature and Footer
                                 html += '.signature-container { margin-top: 45px; width: 100%; display: flex; justify-content: space-between; page-break-inside: avoid; }';
                                 html += '.sig-block { border-top: 1px solid #333; width: 17%; text-align: center; font-size: 9.5px; font-weight: 600; padding-top: 4px; color: #222; }';
                                 html += '.print-footer { margin-top: 18px; font-size: 9px; color: #666; display: flex; justify-content: space-between; border-top: 0.5pt solid #ccc; padding-top: 4px; }';
                                 html += '</style></head><body>';

                                  // Render Header
                                  var compName = '<%= CompanyName %>';
                                  html += '<div class="header-container">';
                                  html += '<div class="company-title">' + compName + '</div>';
                                  html += '<div class="report-title">' + reportTitle + '</div>';
                                  html += '<div class="meta-box">';
                                  html += '<table class="meta-table">';
                                  html += '<tr>';
                                  html += '<td style="width: 12%;" class="m-lbl">Sales Center:</td><td style="width: 21%;" class="m-val">' + scText + '</td>';
                                  html += '<td style="width: 11%;" class="m-lbl">Date Range:</td><td style="width: 25%;" class="m-val">' + dateRange + '</td>';
                                  html += '<td style="width: 11%;" class="m-lbl">Printed On:</td><td style="width: 20%;" class="m-val">' + printTime + '</td>';
                                  html += '</tr>';
                                  html += '<tr>';
                                  html += '<td class="m-lbl">Customer:</td><td class="m-val">' + customer + '</td>';
                                  html += '<td class="m-lbl">Customer Type:</td><td class="m-val">' + custType + '</td>';
                                  html += '<td class="m-lbl">Payment Mode:</td><td class="m-val">' + paymentBy + '</td>';
                                  html += '</tr>';
                                  html += '</table>';
                                  html += '</div>';
                                  html += '</div>';

                                  // Render Table
                                  html += tableHtml;

                                  // Render Signatures
                                  html += '<div class="signature-container">';
                                  html += '<div class="sig-block">Prepared By</div>';
                                  html += '<div class="sig-block">Checked By</div>';
                                  html += '<div class="sig-block">Accounts Officer</div>';
                                  html += '<div class="sig-block">Manager / In-charge</div>';
                                  html += '<div class="sig-block">Authorized Signatory</div>';
                                  html += '</div>';

                                  // Render Print Footer
                                  html += '<div class="print-footer">';
                                  html += '<div>System Generated Document - ' + compName + '</div>';
                                  html += '<div>Confidential &amp; Proprietary</div>';
                                  html += '</div>';

                                 html += '</body></html>';

                                 newWin.document.write(html);
                                 newWin.document.close();

                                 setTimeout(function () {
                                     newWin.focus();
                                     newWin.print();
                                     newWin.close();
                                 }, 400);
                             }
                       </script>
                    <div class="row">
                          <div class="col-md-4">
                               <div class="row mb-3 align-items-center" runat="server" ID="SalesCenter" Visible="True">
                                   <label class="col-sm-4 col-form-label text-end">Sales Center:</label>
                                   <div class="col-sm-8">
                                       <asp:DropDownList ID="dcDropDownList1" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                   </div>
                               </div>

                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">Payment By:</label>
                                   <div class="col-sm-8">
                                       <asp:DropDownList class="form-select form-select-sm mySelect2" runat="server" id="ddlPaymentBy">
                                           <asp:ListItem Text="Select One" Value="" />
                                           <asp:ListItem Text="DIC" Value="DIC" />
                                           <asp:ListItem Text="MIO" Value="MIO" />
                                           <asp:ListItem Text="X. MIO" Value="X. MIO" />
                                       </asp:DropDownList>
                                   </div>
                               </div>
                          </div>

                          <div class="col-md-4">
                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">From Date: <span class="text-danger">*</span></label>
                                   <div class="col-sm-8">
                                       <asp:TextBox ID="InvoiceDateTextBox" AutoPostBack="true" OnTextChanged="fromDateTextBox_TextChanged" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Invoice From Date"></asp:TextBox>
                                   </div>
                               </div>

                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">To Date: <span class="text-danger">*</span></label>
                                   <div class="col-sm-8">
                                       <asp:TextBox ID="todateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Invoice To Date"></asp:TextBox>
                                   </div>
                               </div>
                          </div>

                          <div class="col-md-4">
                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">Customer:</label>
                                   <div class="col-sm-8">
                                       <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                   </div>
                               </div>

                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">Customer Type:</label>
                                   <div class="col-sm-8">
                                       <asp:DropDownList ID="ddlChemisType" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                   </div>
                               </div>
                          </div>

                          <div style="display: none;">
                               <asp:DropDownList class="form-select form-select-sm mySelect2" runat="server" id="ddlProgramType"></asp:DropDownList>
                               <uc1:IVMarketStructure runat="server" ID="IVMarketStructure" />
                          </div>
                    </div>
                            
                            <br />

                    <div class="row mb-3">
                        <div class="col-12 text-center">
                            <asp:LinkButton OnClick="SearchButton_Click" runat="server" id="submitButton" class="btn btnMyDesignSearch btn-sm" style="margin-right: 8px;">
                                <i class="fa fa-list-alt" aria-hidden="true"></i>&nbsp; Details Report
                            </asp:LinkButton>
                            <asp:LinkButton OnClick="SummaryButton_Click" runat="server" id="btnSummaryReport" class="btn btn-primary btn-sm" style="margin-right: 8px;">
                                <i class="fa fa-table" aria-hidden="true"></i>&nbsp; Summary Report
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cancelButton" OnClick="cancelButton_Click" class="btn btnMyDesignReset btn-sm">
                                <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset
                            </asp:LinkButton>
                        </div>
                    </div>

                    <hr class="my-4" />

                    <!-- Details Panel -->
                    <div id="pnlDetails" runat="server">
                        <div class="row align-items-center mb-3">
                            <div class="col-md-3">
                                <h3 class="mb-0">Details List</h3>
                            </div>
                            <div class="col-md-9">
                                <div class="d-flex flex-wrap justify-content-md-end justify-content-start gap-2 align-items-center">
                                    <asp:Label ID="lblCount" runat="server" CssClass="ssss btn bg-info text-dark" Text="Total Pay Amount (TP+Vat) : 0"></asp:Label>
                                    <asp:LinkButton ID="btnExport" class="btn btn-sm text-white" style="background-color: #1A7343; border-color: #1A7343;" runat="server" OnClick="btnExport_Click">
                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnPrintPdfDetails" class="btn btn-sm text-white" style="background-color: #B30B00; border-color: #B30B00;" runat="server" OnClientClick="printReportDiv('MainGradeDiv', 'Customer Payment Details Report'); return false;">
                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp; Print / PDF
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive-custom" id="MainGradeDiv">
                            <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                CssClass="table table-striped table-bordered" AllowPaging="False" OnPreRender="gv_DocumentUpload_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ComUnitCode" HeaderText="Sales Center" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="Sales Center Name" />
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="CustomerType" HeaderText="Customer Type" />
                                    <asp:BoundField DataField="OrderNo" HeaderText="Order Code" />
                                    <asp:BoundField DataField="OrderDate" HeaderText="Order / Submission Date" />
                                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice Number" />
                                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" />
                                    <asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="Delivary Invoice" />
                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivary Date" />
                                    <asp:BoundField DataField="FinalPaymentNo" HeaderText="Payment Number" />
                                    <asp:BoundField DataField="custPaymentDates" HeaderText="Payment Date" />
                                    <asp:BoundField DataField="CashBankAccount" HeaderText="Cash / Bank" />
                                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Invoice Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="PaidAmount" HeaderText="Paid Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="Discount" HeaderText="Discount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="AitVatAmount" HeaderText="AIT + VAT" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="paymenttype" HeaderText="Payment Mode" />
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <!-- Summary Panel -->
                    <div id="pnlSummary" runat="server" visible="false">
                        <div class="row align-items-center mb-3">
                            <div class="col-md-3">
                                <h3 class="mb-0">Summary List</h3>
                            </div>
                            <div class="col-md-9">
                                <div class="d-flex flex-wrap justify-content-md-end justify-content-start gap-2 align-items-center">
                                    <asp:Label ID="lblSummaryCount" runat="server" CssClass="ssss btn bg-info text-dark" Text="Total Collection Amount : 0"></asp:Label>
                                    <asp:LinkButton ID="btnExportSummary" class="btn btn-sm text-white" style="background-color: #1A7343; border-color: #1A7343;" runat="server" OnClick="btnExportSummary_Click">
                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnPrintPdfSummary" class="btn btn-sm text-white" style="background-color: #B30B00; border-color: #B30B00;" runat="server" OnClientClick="printReportDiv('SummaryGradeDiv', 'Customer Payment Summary Report'); return false;">
                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp; Print / PDF
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive-custom" id="SummaryGradeDiv">
                            <asp:GridView ID="summaryGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                CssClass="table table-striped table-bordered" AllowPaging="False" OnPreRender="gv_DocumentUpload_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="custPaymentDates" HeaderText="Payment Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="FinalPaymentNo" HeaderText="Payment Ref. No." ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="CashBankAccount" HeaderText="Cash / Bank" />
                                    <asp:BoundField DataField="PaidAmount" HeaderText="Collection Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" />
                                    <asp:BoundField DataField="paymenttype" HeaderText="Payment Mode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <%--</ContentTemplate>
                      <Triggers>
                 
                 <asp:PostBackTrigger ControlID="btnExport"/>
             </Triggers>
                </asp:UpdatePanel>--%>
                            </div>

                            </div>
                            </div>
                            </div>
                            </div>
                           


     <script>

         function exportToExcel() {

             var file = new Blob([$('#MainGradeDiv').html()], { type: "application/vnd.ms-excel" });
             var url = URL.createObjectURL(file);
             var a = $("<a />", {
                 href: url,
                 download: "Payment  Report.xls"
             }).appendTo("body").get(0).click();
             e.preventDefault();

         }

         function exportTableToExcel(tableID, filename) {
             var downloadLink;
             var dataType = 'application/vnd.ms-excel';
             var tableSelect = document.getElementById(tableID);
             var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

             // Specify file name
             filename = filename ? filename + '.xls' : 'excel_data.xls';

             // Create download link element
             downloadLink = document.createElement("a");

             document.body.appendChild(downloadLink);

             if (navigator.msSaveOrOpenBlob) {
                 var blob = new Blob(['\ufeff', tableHTML], {
                     type: dataType
                 });
                 navigator.msSaveOrOpenBlob(blob, filename);
             } else {
                 // Create a link to the file
                 downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

                 // Setting the file name
                 downloadLink.download = filename;

                 //triggering the function
                 downloadLink.click();
             }
         }
     </script>
     
</asp:Content>

