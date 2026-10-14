<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProformaInvoiceHtmlViewer.aspx.cs" Inherits="SInventory_RPTVIEW_ProformaInvoiceHtmlViewer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Proforma Invoice</title>
    <meta charset="utf-8" />
    <style type="text/css">
        * { box-sizing: border-box; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }
        body { font-family: "Times New Roman", Times, serif; margin: 0; padding: 15px; background: #f4f6f9; color: #000; font-size: 14px; }
        .action-bar { max-width: 900px; margin: 0 auto 12px; display: flex; justify-content: flex-end; gap: 10px; font-family: Arial, sans-serif; }
        .btn { padding: 6px 16px; font-size: 13px; font-weight: 600; border: none; border-radius: 4px; cursor: pointer; color: #fff; }
        .btn-primary { background: #0d6efd; }
        .btn-secondary { background: #6c757d; }
        .page { max-width: 900px; margin: 0 auto; background: #fff; padding: 25px 30px; border: 1px solid #dcdcdc; box-shadow: 0 2px 8px rgba(0,0,0,.08); }
        table { width: 100%; border-collapse: collapse; }
        .head td { vertical-align: top; }
        .head img { height: 48px; }
        .title { text-align: center; line-height: 1.35; margin: 4px 0 10px; }
        .title h2 { margin: 0; font-size: 22px; text-decoration: underline; }
        .meta { border-top: 1.5px solid #000; border-bottom: 1.5px solid #000; padding: 6px 0; }
        .meta td { vertical-align: top; padding: 3px 4px; }
        .meta .k { font-weight: bold; white-space: nowrap; width: 1%; }
        .grid { margin-top: 14px; font-size: 13px; }
        .grid th, .grid td { border: 1px solid #000; padding: 4px 6px; }
        .grid th { text-align: center; font-size: 12px; }
        .grid td.c { text-align: center; }
        .grid td.r { text-align: right; }
        .grid tfoot td { font-weight: bold; }
        .sum { width: 330px; margin: 10px 0 0 auto; font-size: 13px; }
        .sum td { padding: 3px 6px; }
        .sum td.r { text-align: right; }
        .sum tr.g td { border-top: 1.5px solid #000; font-weight: bold; }
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
            <table class="head">
                <tr>
                    <td style="width: 150px;"><img src="../images/unicorn_logo.png" alt="" /></td>
                    <td>
                        <b><asp:Literal ID="litCompanyName" runat="server"></asp:Literal></b><br />
                        <asp:Literal ID="litCompanyAddress" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>

            <div class="title">
                <h2>Proforma Invoice</h2>
                <div>(Tax Challanpatra, Mushak- 6.3)</div>
                <div>BIN: 002213238-0208</div>
                <div style="text-align: right; font-size: 12px;">Print: <asp:Literal ID="litPrintDate" runat="server"></asp:Literal></div>
            </div>

            <table class="meta">
                <tr>
                    <td class="k">Customer Code:</td><td><asp:Literal ID="litCustomerCode" runat="server"></asp:Literal></td>
                    <td class="k">Branch:</td><td><asp:Literal ID="litBranch" runat="server"></asp:Literal></td>
                    <td class="k">Order No:</td><td><asp:Literal ID="litOrderNo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="k">Customer Name:</td><td><asp:Literal ID="litCustomerName" runat="server"></asp:Literal></td>
                    <td class="k">Sales Person:</td><td><asp:Literal ID="litSalesPerson" runat="server"></asp:Literal></td>
                    <td class="k">Invoice No:</td><td><asp:Literal ID="litInvoiceNo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="k">Billing Address:</td><td><asp:Literal ID="litBillingAddress" runat="server"></asp:Literal></td>
                    <td class="k">Contact:</td><td><asp:Literal ID="litContact" runat="server"></asp:Literal></td>
                    <td class="k">Delivery Date:</td><td><asp:Literal ID="litDeliveryDate" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="k">Delivery Address:</td><td><asp:Literal ID="litDeliveryAddress" runat="server"></asp:Literal></td>
                    <td class="k">Credit Days:</td><td><asp:Literal ID="litCreditDays" runat="server"></asp:Literal></td>
                    <td class="k">Payment:</td><td><asp:Literal ID="litPayment" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <table class="grid">
                <thead>
                    <tr>
                        <th rowspan="2">SL</th><th rowspan="2">Code</th><th rowspan="2">Description &amp; Pack Size</th>
                        <th rowspan="2">Batch</th><th rowspan="2">Exp. Date</th><th rowspan="2">Qty</th>
                        <th colspan="2">Per Pack</th><th colspan="2">Total</th><th colspan="2">Discount</th>
                        <th rowspan="2">Net Value (BDT)</th>
                    </tr>
                    <tr>
                        <th>TP</th><th>VAT</th><th>TP</th><th>VAT</th><th>%</th><th>Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptLines" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="c"><%# Container.ItemIndex + 1 %></td>
                                <td><%# Server.HtmlEncode(Convert.ToString(Eval("ProductCode"))) %></td>
                                <td><%# Server.HtmlEncode((Convert.ToString(Eval("Product")) + " " + Convert.ToString(Eval("BonusQuantity"))).Trim()) %></td>
                                <td class="c"><%# Server.HtmlEncode(Convert.ToString(Eval("BatchNo"))) %></td>
                                <td class="c"><%# Date(Eval("ExpDate")) %></td>
                                <td class="c"><%# Eval("Quantity", "{0:#,##0.##}") %></td>
                                <td class="r"><%# Eval("UnitPrice", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("UnitVatAmount", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("TotalPrice", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("TotalPriceVatAmount", "{0:#,##0.00}") %></td>
                                <td class="c"><%# Eval("DiscountPercentage", "{0:#,##0.##}") %></td>
                                <td class="r"><%# Eval("DiscountAmount", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("NetAmount", "{0:#,##0.00}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="5" class="r">Total Quantity :</td>
                        <td class="c"><asp:Literal ID="litTotalQty" runat="server"></asp:Literal></td>
                        <td></td><td></td>
                        <td class="r"><asp:Literal ID="litTotalTp" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTotalVat" runat="server"></asp:Literal></td>
                        <td></td>
                        <td class="r"><asp:Literal ID="litTotalDiscount" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTotalNet" runat="server"></asp:Literal></td>
                    </tr>
                </tfoot>
            </table>

            <table class="sum">
                <tr runat="server" id="trDelivery"><td>Delivery Charge</td><td class="r"><asp:Literal ID="litDelivery" runat="server"></asp:Literal></td></tr>
                <tr class="g"><td>Grand Total (BDT)</td><td class="r"><asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></td></tr>
                <tr><td>Total Due (Customer)</td><td class="r"><asp:Literal ID="litTotalDue" runat="server"></asp:Literal></td></tr>
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
