<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerPaymentReportPDF.aspx.cs" Inherits="SInventory_UI_CustomerPaymentReportPDF" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Payment Report - PDF Preview</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    
    <style type="text/css">
        * {
            box-sizing: border-box;
            -webkit-print-color-adjust: exact !important;
            print-color-adjust: exact !important;
        }

        body {
            font-family: Arial, Helvetica, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f1f5f9;
            color: #1e293b;
        }

        .action-bar {
            max-width: 950px;
            margin: 0 auto 15px auto;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .btn {
            padding: 8px 18px;
            font-size: 13px;
            font-weight: 600;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

        .btn-pdf {
            background-color: #ef4444;
            color: #fff;
        }
        .btn-pdf:hover {
            background-color: #dc2626;
        }

        .btn-close {
            background-color: #64748b;
            color: #fff;
        }
        .btn-close:hover {
            background-color: #475569;
        }

        /* Document Container */
        .doc-container {
            max-width: 950px;
            margin: 0 auto;
            background: #ffffff;
            padding: 30px 40px;
            border: 1px solid #cbd5e1;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
            border-radius: 4px;
        }

        /* Header */
        .report-header {
            text-align: center;
            margin-bottom: 20px;
            border-bottom: 2px solid #0f172a;
            padding-bottom: 12px;
        }
        .company-name {
            font-size: 22px;
            font-weight: bold;
            color: #0f172a;
            margin: 0 0 4px 0;
        }
        .company-address {
            font-size: 12px;
            color: #475569;
            margin: 0 0 10px 0;
        }
        .report-title {
            font-size: 16px;
            font-weight: bold;
            color: #1e293b;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            background-color: #f1f5f9;
            display: inline-block;
            padding: 4px 16px;
            border-radius: 3px;
            margin-bottom: 6px;
        }
        .report-date-range {
            font-size: 12.5px;
            font-weight: 600;
            color: #334155;
            margin: 0;
        }

        /* Table */
        .pdf-table {
            width: 100%;
            border-collapse: collapse;
            font-size: 12px;
            margin-top: 15px;
        }
        .pdf-table th {
            background-color: #f1f5f9;
            color: #0f172a;
            font-weight: bold;
            text-align: left;
            padding: 8px 10px;
            border: 1px solid #cbd5e1;
            font-size: 11.5px;
            text-transform: uppercase;
        }
        .pdf-table td {
            padding: 7px 10px;
            border: 1px solid #e2e8f0;
            vertical-align: middle;
        }
        .pdf-table tr:nth-child(even) {
            background-color: #f8fafc;
        }
        .text-center {
            text-align: center;
        }
        .text-right {
            text-align: right;
        }
        .pdf-table tfoot td {
            font-weight: bold;
            background-color: #f1f5f9;
            border-top: 2px solid #0f172a;
            padding: 8px 10px;
        }

        .voucher-code {
            font-family: Consolas, monospace;
            font-weight: 600;
        }

        /* Footer Signatures */
        .footer-sig {
            margin-top: 60px;
            width: 100%;
            border-collapse: collapse;
        }
        .footer-sig td {
            width: 33.33%;
            text-align: center;
            font-size: 12px;
            font-weight: bold;
            padding-top: 6px;
            border: none;
        }
        .sig-line {
            border-top: 1px solid #0f172a;
            width: 70%;
            margin: 0 auto;
            padding-top: 4px;
        }

        @media print {
            body {
                background: #fff !important;
                padding: 0 !important;
                margin: 0 !important;
            }
            .action-bar {
                display: none !important;
            }
            .doc-container {
                border: none !important;
                box-shadow: none !important;
                padding: 0 !important;
                max-width: 100% !important;
            }
            @page {
                size: A4 landscape;
                margin: 10mm;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="action-bar">
            <button type="button" class="btn btn-pdf" onclick="window.print();">
                <i class="fa fa-print"></i> Print / Save as PDF
            </button>
            <button type="button" class="btn btn-close" onclick="window.close();">
                <i class="fa fa-times"></i> Close
            </button>
        </div>

        <div class="doc-container">
            <!-- Header -->
            <div class="report-header">
                <div class="company-name">Unicorn Healthcare Solution Ltd</div>
                <div class="company-address">Floor – 8/A, Dynesty Wahid Tower 56/2 Panthapath, Dhaka-1205, Bangladesh.</div>
                <div class="report-title">Customer Payment & Contra Voucher Report</div>
                <div class="report-date-range">
                    Period: <asp:Literal ID="litPeriod" runat="server"></asp:Literal>
                </div>
            </div>

            <!-- Table -->
            <table class="pdf-table">
                <thead>
                    <tr>
                        <th style="width: 40px;" class="text-center">SL</th>
                        <th style="width: 140px;" class="text-center">Voucher No</th>
                        <th style="width: 95px;" class="text-center">Date</th>
                        <th style="width: 120px;">Account Head</th>
                        <th style="width: 100px;" class="text-center">Payment Mode</th>
                        <th>Narration / Particulars</th>
                        <th style="width: 110px;" class="text-right">Amount (Tk)</th>
                        <th style="width: 85px;" class="text-center">User</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptPayments" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="text-center"><%# Container.ItemIndex + 1 %></td>
                                <td class="text-center voucher-code"><%# Eval("VoucherNo") %></td>
                                <td class="text-center"><%# Convert.ToDateTime(Eval("PaymentDate")).ToString("dd-MMM-yyyy") %></td>
                                <td><%# Eval("BankCash") %></td>
                                <td class="text-center"><%# Eval("ChequeNo") %></td>
                                <td><%# Eval("Narration") %></td>
                                <td class="text-right" style="font-weight: bold;"><%# Convert.ToDecimal(Eval("Amount")).ToString("#,##0.00") %></td>
                                <td class="text-center"><%# Eval("UserName") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6" class="text-right">Grand Total:</td>
                        <td class="text-right">
                            <asp:Literal ID="litGrandTotal" runat="server">0.00</asp:Literal>
                        </td>
                        <td></td>
                    </tr>
                </tfoot>
            </table>

            <!-- Signatures -->
            <table class="footer-sig">
                <tr>
                    <td>
                        <div class="sig-line">Prepared By</div>
                    </td>
                    <td>
                        <div class="sig-line">Verified By</div>
                    </td>
                    <td>
                        <div class="sig-line">Authorized Signatory</div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
