<%@ Page Language="C#" AutoEventWireup="true" CodeFile="B2BTransferPrint.aspx.cs" Inherits="SInventory_UI_B2BTransferPrint" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Transfer Challan</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <style type="text/css">
        * { box-sizing: border-box; -webkit-print-color-adjust: exact !important; print-color-adjust: exact !important; }
        body { font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 15px; background: #f4f6f9; color: #222; font-size: 13px; }
        .action-bar { max-width: 900px; margin: 0 auto 12px; display: flex; justify-content: flex-end; gap: 10px; }
        .btn { padding: 6px 16px; font-size: 13px; font-weight: 600; border: none; border-radius: 4px; cursor: pointer; color: #fff; }
        .btn-primary { background: #0d6efd; }
        .btn-secondary { background: #6c757d; }
        .page { max-width: 900px; margin: 0 auto; background: #fff; padding: 25px 30px; border: 1px solid #dcdcdc; box-shadow: 0 2px 8px rgba(0,0,0,.08); }
        .company { text-align: center; margin-bottom: 6px; }
        .company h2 { margin: 0 0 4px; font-size: 20px; }
        .company div { font-size: 12.5px; font-weight: 600; line-height: 1.4; }
        .title { background: #f2f3f5; text-align: center; padding: 5px 0; margin: 10px 0 12px; font-size: 17px; font-weight: bold; }
        table { width: 100%; border-collapse: collapse; }
        .info td { padding: 3px 4px; vertical-align: top; }
        .info .lbl { width: 110px; font-weight: bold; }
        .info .sep { width: 12px; font-weight: bold; }
        .grid { margin-top: 12px; }
        .grid th, .grid td { border: 1px solid #555; padding: 4px 6px; }
        .grid th { background: #e9ecef; text-align: center; }
        .grid td.c { text-align: center; }
        .grid td.r { text-align: right; }
        .grid tfoot td { font-weight: bold; }
        .sign { margin-top: 60px; }
        .sign td { width: 33%; text-align: center; }
        .sign div { border-top: 1.5px solid #222; display: inline-block; min-width: 150px; padding-top: 4px; font-weight: bold; }
        .msg { text-align: center; color: #dc2626; font-weight: bold; padding: 30px; }
        @media print {
            body { background: #fff !important; padding: 0 !important; }
            .action-bar { display: none !important; }
            .page { border: none !important; box-shadow: none !important; max-width: 100% !important; padding: 10px 15px !important; }
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
            <div class="company">
                <h2><asp:Literal ID="litCompanyName" runat="server"></asp:Literal></h2>
                <div><asp:Literal ID="litCompanyAddress" runat="server"></asp:Literal></div>
                <div><asp:Literal ID="litCompanyContact" runat="server"></asp:Literal></div>
            </div>

            <div class="title">Stock Transfer Challan (DC to DC)</div>

            <table class="info">
                <tr>
                    <td class="lbl">Chalan No</td><td class="sep">:</td><td><asp:Literal ID="litChalanNo" runat="server"></asp:Literal></td>
                    <td class="lbl">Chalan Date</td><td class="sep">:</td><td><asp:Literal ID="litChalanDate" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="lbl">From (DC)</td><td class="sep">:</td><td><asp:Literal ID="litFrom" runat="server"></asp:Literal></td>
                    <td class="lbl">To (DC)</td><td class="sep">:</td><td><asp:Literal ID="litTo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="lbl">Vehicle / Track</td><td class="sep">:</td><td><asp:Literal ID="litTrack" runat="server"></asp:Literal></td>
                    <td class="lbl">Driver</td><td class="sep">:</td><td><asp:Literal ID="litDriver" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td class="lbl">Status</td><td class="sep">:</td><td colspan="4"><asp:Literal ID="litStatus" runat="server"></asp:Literal></td>
                </tr>
            </table>

            <table class="grid">
                <thead>
                    <tr>
                        <th>#SL</th><th>Product Code</th><th>Product Name</th><th>Batch No</th><th>Mfg Date</th><th>Exp Date</th>
                        <th>Qty</th><th>Unit Price</th><th>Value</th><th>Vat</th><th>Total</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptLines" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="c"><%# Container.ItemIndex + 1 %></td>
                                <td><%# Eval("ProductCode") %></td>
                                <td><%# Eval("ProductName") %></td>
                                <td><%# Eval("BatchNo") %></td>
                                <td class="c"><%# Date(Eval("MfgDate")) %></td>
                                <td class="c"><%# Date(Eval("ExpDate")) %></td>
                                <td class="r"><%# Eval("Quantity", "{0:#,##0.##}") %></td>
                                <td class="r"><%# Eval("UnitPrice", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("Value", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("Vat", "{0:#,##0.00}") %></td>
                                <td class="r"><%# Eval("ValueWVat", "{0:#,##0.00}") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="6" class="r">Total</td>
                        <td class="r"><asp:Literal ID="litTotalQty" runat="server"></asp:Literal></td>
                        <td></td>
                        <td class="r"><asp:Literal ID="litTotalValue" runat="server"></asp:Literal></td>
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
