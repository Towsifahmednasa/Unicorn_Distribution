<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChallanReportViewer.aspx.cs" Inherits="SInventory_RPTVIEW_ChallanReportViewer" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delivery Challan</title>
    <meta charset="utf-8" />
    <style type="text/css">
        * { box-sizing: border-box; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }
        body { font-family: "Times New Roman", Times, serif; margin: 0; padding: 15px; background: #f4f6f9; color: #000; font-size: 14px; }
        .action-bar { max-width: 900px; margin: 0 auto 12px; display: flex; justify-content: flex-end; gap: 10px; font-family: Arial, sans-serif; }
        .btn { padding: 6px 16px; font-size: 13px; font-weight: 600; border: none; border-radius: 4px; cursor: pointer; color: #fff; }
        .btn-primary { background: #0d6efd; }
        .btn-secondary { background: #6c757d; }
        .page { max-width: 900px; margin: 0 auto; background: #fff; padding: 25px 30px; border: 1px solid #dcdcdc; box-shadow: 0 2px 8px rgba(0,0,0,.08); }
        .gov { border: 1px solid #000; text-align: center; padding: 6px 10px; position: relative; line-height: 1.3; }
        .gov .mushak { position: absolute; right: 12px; top: 28px; font-size: 14px; }
        .gov h2 { margin: 6px 0 2px; font-size: 22px; text-decoration: underline; }
        table { width: 100%; border-collapse: collapse; }
        .info td { vertical-align: top; padding: 2px 3px; }
        .info .l { text-align: right; width: 150px; white-space: nowrap; }
        .info .r { text-align: right; font-size: 11px; font-family: Arial, sans-serif; }
        .big { font-size: 16px; }
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
                <div>Government of the People's Republic of Bangladesh, NBR, Dhaka</div>
                <div>TAX Challan</div>
                <div>[Clause ga &amp; cha of sub-rules 1 of rules 40]</div>
                <span class="mushak">Mushak-6.3</span>
                <h2>Delivery Challan</h2>
            </div>

            <table class="info" style="margin-top: 6px;">
                <tr>
                    <td class="l">Registered Organisation :</td>
                    <td><asp:Literal ID="litCompanyName" runat="server"></asp:Literal></td>
                    <td class="r">Form No: ADM/Form/003</td>
                </tr>
                <tr>
                    <td class="l">Address :</td>
                    <td><asp:Literal ID="litCompanyAddress" runat="server"></asp:Literal></td>
                    <td class="r">Version No: 00</td>
                </tr>
                <tr>
                    <td class="l">BIN :</td>
                    <td>002213238-0208</td>
                    <td class="r big" style="font-family: 'Times New Roman', serif; font-size: 14px;">Effective Date : 01.08.2015</td>
                </tr>
            </table>

            <table class="info" style="margin-top: 16px;">
                <tr>
                    <td class="l">Issuing Office :</td>
                    <td><asp:Literal ID="litIssueOffice" runat="server"></asp:Literal></td>
                    <td class="big" style="width: 280px;">Challan No : <asp:Literal ID="litChalanNo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="l">Address :</td>
                    <td><asp:Literal ID="litIssueAddress" runat="server"></asp:Literal></td>
                    <td class="big">Date : <asp:Literal ID="litChalanDate" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="l">Destination :</td>
                    <td><asp:Literal ID="litDestination" runat="server"></asp:Literal></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="l">Address :</td>
                    <td><asp:Literal ID="litDestAddress" runat="server"></asp:Literal></td>
                    <td class="r">Print Date: &nbsp;<asp:Literal ID="litPrintDate" runat="server"></asp:Literal></td>
                </tr>
                <tr runat="server" id="trTruck">
                    <td class="l">Truck / Driver :</td>
                    <td colspan="2"><asp:Literal ID="litTruck" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <table class="grid">
                <thead>
                    <tr>
                        <th>SL</th><th>Code</th><th>Product Name</th><th>Batch #</th><th>Exp.Date</th>
                        <th>Unit</th><th>Quantity</th><th>TP Price</th><th>VAT</th><th>Total Price</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptLines" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="c"><%# Container.ItemIndex + 1 %></td>
                                <td class="code"><%# Eval("ProductCode") %></td>
                                <td><%# Eval("ProductName") %></td>
                                <td class="c"><%# Eval("BatchNo") %></td>
                                <td class="c"><%# Date(Eval("ExpDate")) %></td>
                                <td class="c"><%# Eval("StockUOMName") %></td>
                                <td class="c"><%# Eval("Quantity", "{0:#,##0.##}") %></td>
                                <td class="r"><%# Eval("PriceAmount", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("VATAmount", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("TotalPriceAmount", "{0:#,##0.00}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6" class="r" style="font-family: 'Times New Roman', serif; font-size: 14px;">Total</td>
                        <td class="c"><asp:Literal ID="litTotalQty" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTotalPrice" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litTotalVat" runat="server"></asp:Literal></td>
                        <td class="r"><asp:Literal ID="litGrandTotal" runat="server"></asp:Literal></td>
                    </tr>
                </tfoot>
            </table>

            <table class="sign">
                <tr>
                    <td><div>Prepared By</div></td>
                    <td><div>Delivered By</div></td>
                    <td><div>Received By</div></td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
