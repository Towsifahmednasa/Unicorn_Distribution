<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReturnInvoiceReportViewer2.aspx.cs" Inherits="SInventory_RPTVIEW_ReturnInvoiceReportViewer2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Return Invoice</title>
    <meta charset="utf-8" />
    <style type="text/css">
        * { box-sizing: border-box; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }
        body { font-family: "Times New Roman", Times, serif; margin: 0; padding: 15px; background: #f4f6f9; color: #000; font-size: 14px; }
        .action-bar { max-width: 900px; margin: 0 auto 12px; display: flex; justify-content: flex-end; gap: 10px; font-family: Arial, sans-serif; }
        .btn { padding: 6px 16px; font-size: 13px; font-weight: 600; border: none; border-radius: 4px; cursor: pointer; color: #fff; }
        .btn-primary { background: #0d6efd; }
        .btn-secondary { background: #6c757d; }
        .page { max-width: 900px; margin: 0 auto; background: #fff; padding: 25px 30px; border: 1px solid #dcdcdc; box-shadow: 0 2px 8px rgba(0,0,0,.08); }
        .gov { border: 1px solid #000; text-align: center; padding: 6px 10px; line-height: 1.3; }
        .gov h2 { margin: 6px 0 2px; font-size: 22px; text-decoration: underline; }
        table { width: 100%; border-collapse: collapse; }
        .info td { vertical-align: top; padding: 2px 3px; }
        .info .l { text-align: left; width: 130px; white-space: nowrap; }
        .info .k { width: 110px; font-weight: bold; white-space: nowrap; }
        .info .h { font-weight: bold; text-decoration: underline; }
        .grid { margin-top: 18px; font-size: 13px; }
        .grid th, .grid td { border: 1px solid #000; padding: 4px 6px; }
        .grid th { text-align: center; font-family: Arial, sans-serif; font-size: 12px; }
        .grid td.c { text-align: center; }
        .grid td.r { text-align: right; font-family: Arial, sans-serif; font-size: 12px; }
        .grid tfoot td { font-weight: bold; }
        .code { font-family: Arial, sans-serif; font-size: 11px; font-weight: bold; }
        .sign { margin-top: 70px; }
        .sign td { width: 33%; text-align: center; }
        .sign div { border-top: 1.5px solid #000; display: inline-block; min-width: 150px; padding-top: 4px; font-weight: bold; }
        .msg { text-align: center; color: #dc2626; font-weight: bold; padding: 30px; font-family: Arial, sans-serif; }
        thead { display: table-header-group; }
        tr { page-break-inside: avoid; }
        @media print {
            body { background: #fff !important; padding: 0 !important; }
            .action-bar { display: none !important; }
            .page { border: none !important; box-shadow: none !important; max-width: 100% !important; padding: 0 !important; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="action-bar">
            <button type="button" class="btn btn-primary" onclick="window.print();">Print</button>
            <button type="button" class="btn btn-secondary" onclick="window.close();">Close</button>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="msg" Visible="false"></asp:Label>

        <asp:Panel ID="pnlReport" runat="server" CssClass="page">
            <div class="gov">
                <h2>Return Invoice</h2>
                <div>Government of the People's Republic of Bangladesh, NBR, Dhaka</div>
                <div>Tax Challanpatra</div>
                <div>Rule-40, Sub Rule(1), Clause (GA &amp; CHA)</div>
            </div>

            <table class="info" style="margin-top: 8px;">
                <tr>
                    <td class="l">Name of the Seller :</td>
                    <td><b><asp:Literal ID="litCompanyName" runat="server"></asp:Literal></b></td>
                </tr>
                <tr>
                    <td class="l">Address :</td>
                    <td><asp:Literal ID="litCompanyAddress" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <table class="info" style="margin-top: 16px;">
                <tr>
                    <td class="h">Sales Center</td>
                    <td></td><td></td>
                </tr>
                <tr>
                    <td><asp:Literal ID="litUnitCode" runat="server"></asp:Literal></td>
                    <td class="k">Invoice No</td>
                    <td style="width: 220px;"><asp:Literal ID="litInvoiceNo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td><asp:Literal ID="litUnitName" runat="server"></asp:Literal></td>
                    <td class="k">Invoice Date</td>
                    <td><asp:Literal ID="litInvoiceDate" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td><asp:Literal ID="litUnitAddress" runat="server"></asp:Literal></td>
                    <td class="k">Order No</td>
                    <td><asp:Literal ID="litOrderNo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td></td>
                    <td class="k">Order Date</td>
                    <td><asp:Literal ID="litOrderDate" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <table class="info" style="margin-top: 14px;">
                <tr>
                    <td class="h">Sold To</td>
                    <td class="k">Payment Type</td>
                    <td style="width: 220px;"><asp:Literal ID="litPaymentType" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td><asp:Literal ID="litCustomerCode" runat="server"></asp:Literal></td>
                    <td class="k">MIA</td>
                    <td><asp:Literal ID="litMia" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td><asp:Literal ID="litCustomerName" runat="server"></asp:Literal></td>
                    <td class="k">Print On</td>
                    <td><asp:Literal ID="litPrintDate" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td colspan="3"><asp:Literal ID="litCustomerAddress" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <table class="grid">
                <thead>
                    <tr>
                        <th>SL</th><th>Code</th><th>Product Name</th><th>Batch #</th><th>Pack Size</th>
                        <th>Quantity</th><th>Per Pack</th><th>Total</th><th>Discount</th><th>VAT</th><th>Net Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptLines" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="c"><%# Container.ItemIndex + 1 %></td>
                                <td class="code"><%# Server.HtmlEncode(Convert.ToString(Eval("ProductCode"))) %></td>
                                <td><%# Server.HtmlEncode(Convert.ToString(Eval("Product"))) %></td>
                                <td class="c"><%# Server.HtmlEncode(Convert.ToString(Eval("BatchNo"))) %></td>
                                <td class="c"><%# Server.HtmlEncode(Convert.ToString(Eval("BonusQuantity"))) %></td>
                                <td class="c"><%# Eval("Quantity", "{0:#,##0.##}") %></td>
                                <td class="r"><%# Eval("UnitPrice", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("TotalPrice", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("DiscountAmount", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("TotalPriceVatAmount", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("NetAmount", "{0:#,##0.00}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5" class="r" style="font-family: 'Times New Roman', serif; font-size: 14px;">Total</td>
                        <td class="c"><asp:Literal ID="litTotalQty" runat="server"></asp:Literal></td>
                        <td></td>
                        <td class="r"><asp:Literal ID="litTpTotal" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTpDiscount" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTpVat" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTpGrandTotal" runat="server"></asp:Literal></td>
                    </tr>
                </tfoot>
            </table>

            <table class="sign">
                <tr>
                    <td><div>Prepared By</div></td>
                    <td><div>Checked By</div></td>
                    <td><div>Customer Signature</div></td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
