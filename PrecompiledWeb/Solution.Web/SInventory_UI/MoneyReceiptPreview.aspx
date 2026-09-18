<%@ page language="C#" autoeventwireup="true" inherits="SInventory_UI_MoneyReceiptPreview, App_Web_yureiwkh" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Money Receipt Preview</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
        * {
            box-sizing: border-box;
            -webkit-print-color-adjust: exact !important;
            print-color-adjust: exact !important;
        }

        body {
            font-family: Arial, Helvetica, sans-serif;
            margin: 0;
            padding: 15px;
            background-color: #f4f6f9;
            color: #222;
        }

        .action-bar {
            max-width: 800px;
            margin: 0 auto 12px auto;
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .btn {
            padding: 6px 16px;
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

        .btn-primary {
            background-color: #0d6efd;
            color: #fff;
        }

        .btn-primary:hover {
            background-color: #0b5ed7;
        }

        .btn-secondary {
            background-color: #6c757d;
            color: #fff;
        }

        .btn-secondary:hover {
            background-color: #5c636a;
        }

        /* Receipt Container */
        .receipt-container {
            max-width: 800px;
            margin: 0 auto;
            background: #ffffff;
            padding: 25px 35px 20px 35px;
            border: 1px solid #dcdcdc;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
            position: relative;
        }

        /* Header Section */
        .header-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 8px;
        }

        .logo-col {
            width: 18%;
            vertical-align: middle;
        }

        .logo-img {
            max-width: 130px;
            height: auto;
        }

        .company-col {
            width: 64%;
            text-align: center;
            vertical-align: middle;
            padding: 0 10px;
        }

        .company-name {
            font-size: 20px;
            font-weight: bold;
            color: #1a1a1a;
            margin: 0 0 4px 0;
        }

        .company-address {
            font-size: 12.5px;
            font-weight: 600;
            color: #333;
            margin: 0;
            line-height: 1.35;
        }

        .copy-col {
            width: 18%;
            text-align: right;
            vertical-align: top;
        }

        .copy-badge {
            display: inline-block;
            border: 1.5px solid #a0a0a0;
            padding: 4px 10px;
            font-size: 12px;
            font-weight: 600;
            color: #333;
            border-radius: 3px;
        }

        /* Title Bar */
        .title-bar {
            background-color: #f2f3f5;
            text-align: center;
            padding: 5px 0;
            margin: 8px 0 12px 0;
        }

        .title-text {
            font-size: 18px;
            font-weight: bold;
            color: #111;
            margin: 0;
            letter-spacing: 0.3px;
        }

        /* Metadata Row (Receipt No & Date) */
        .meta-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 12px;
        }

        .meta-right {
            text-align: right;
            font-size: 13.5px;
        }

        .meta-label {
            font-weight: bold;
            display: inline-block;
            min-width: 90px;
            text-align: right;
        }

        .meta-value {
            display: inline-block;
            min-width: 100px;
            text-align: right;
            padding-left: 8px;
            font-weight: 500;
        }

        /* Body Details */
        .details-table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 15px;
            font-size: 13.5px;
            line-height: 1.6;
        }

        .details-table td {
            padding: 3px 0;
            vertical-align: top;
        }

        .label-cell {
            width: 140px;
            font-weight: bold;
            color: #222;
        }

        .separator-cell {
            width: 15px;
            font-weight: bold;
            color: #222;
            text-align: center;
        }

        .value-cell {
            color: #111;
        }

        .amount-bold {
            font-weight: bold;
            font-size: 14px;
        }

        .in-words-row {
            margin-top: 4px;
            font-size: 13px;
            font-style: italic;
            font-weight: normal;
        }

        /* Signatures Section */
        .signature-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 45px;
            margin-bottom: 18px;
        }

        .sig-col-left {
            width: 35%;
            text-align: center;
        }

        .sig-col-space {
            width: 30%;
        }

        .sig-col-right {
            width: 35%;
            text-align: center;
        }

        .sig-line {
            border-top: 1.5px solid #222;
            padding-top: 4px;
            font-size: 13px;
            font-weight: bold;
            color: #111;
        }

        /* Footer Note */
        .note-text {
            font-size: 11px;
            color: #111;
            font-weight: bold;
            margin: 0;
            padding-top: 4px;
        }

        /* Print Settings */
        @media print {
            body {
                background: #fff !important;
                padding: 0 !important;
                margin: 0 !important;
            }

            .action-bar {
                display: none !important;
            }

            .receipt-container {
                border: none !important;
                box-shadow: none !important;
                padding: 10px 20px !important;
                max-width: 100% !important;
                page-break-inside: avoid;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="action-bar">
            <button type="button" class="btn btn-primary" onclick="window.print();">Print Receipt</button>
            <button type="button" class="btn btn-secondary" onclick="window.close();">Close</button>
        </div>

        <div class="receipt-container">
            <!-- Header -->
            <table class="header-table">
                <tr>
                    <td class="logo-col">
                        <img src="../images/unicorn_logo.png" alt="Unicorn Healthcare" class="logo-img" onerror="this.style.display='none';" />
                    </td>
                    <td class="company-col">
                        <div class="company-name">
                            <asp:Literal ID="litCompanyName" runat="server">Unicorn Healthcare Solution Ltd</asp:Literal>
                        </div>
                        <div class="company-address">
                            <asp:Literal ID="litCompanyAddress" runat="server">Floor – 8/A, Dynesty Wahid Tower 56/2 Panthapath, Dhaka-1205, Bangladesh .</asp:Literal>
                        </div>
                    </td>
                    <td class="copy-col">
                        <div class="copy-badge">
                            <asp:Literal ID="litCopyType" runat="server">Client Copy</asp:Literal>
                        </div>
                    </td>
                </tr>
            </table>

            <!-- Title Bar -->
            <div class="title-bar">
                <div class="title-text">Money Receipt</div>
            </div>

            <!-- Metadata (Receipt No & Date) -->
            <table class="meta-table">
                <tr>
                    <td></td>
                    <td class="meta-right">
                        <div>
                            <span class="meta-label">Receipt No :</span>
                            <span class="meta-value">
                                <asp:Literal ID="litReceiptNo" runat="server"></asp:Literal>
                            </span>
                        </div>
                        <div style="margin-top: 4px;">
                            <span class="meta-label">Date :</span>
                            <span class="meta-value">
                                <asp:Literal ID="litReceiptDate" runat="server"></asp:Literal>
                            </span>
                        </div>
                    </td>
                </tr>
            </table>

            <!-- Receipt Details -->
            <table class="details-table">
                <tr>
                    <td class="label-cell">Customer</td>
                    <td class="separator-cell">:</td>
                    <td class="value-cell">
                        <asp:Literal ID="litCustomer" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell">Cheque No</td>
                    <td class="separator-cell">:</td>
                    <td class="value-cell">
                        <asp:Literal ID="litChequeNo" runat="server">Cash</asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell">Cheque Date</td>
                    <td class="separator-cell">:</td>
                    <td class="value-cell">
                        <asp:Literal ID="litChequeDate" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell">Customer Bank</td>
                    <td class="separator-cell">:</td>
                    <td class="value-cell">
                        <asp:Literal ID="litCustomerBank" runat="server">NONE</asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell">Received Against</td>
                    <td class="separator-cell">:</td>
                    <td class="value-cell">
                        <asp:Literal ID="litReceivedAgainst" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="label-cell">Amount</td>
                    <td class="separator-cell">:</td>
                    <td class="value-cell amount-bold">
                        <asp:Literal ID="litAmount" runat="server"></asp:Literal>
                        <div class="in-words-row">
                            IN WORD - ( <asp:Literal ID="litAmountInWords" runat="server"></asp:Literal> )
                        </div>
                    </td>
                </tr>
            </table>

            <!-- Signatures -->
            <table class="signature-table">
                <tr>
                    <td class="sig-col-left">
                        <div class="sig-line">Customer</div>
                    </td>
                    <td class="sig-col-space">&nbsp;</td>
                    <td class="sig-col-right">
                        <div class="sig-line">Received By</div>
                    </td>
                </tr>
            </table>

            <!-- Disclaimer Note -->
            <p class="note-text">
                N.B. This money receipt against Cheque/PO is valid subject to the clearance of the same.
            </p>
        </div>
    </form>
</body>
</html>
