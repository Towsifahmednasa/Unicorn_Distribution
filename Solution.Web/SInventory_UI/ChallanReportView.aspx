<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="ChallanReportView.aspx.cs" Inherits="SInventory_UI_ChallanReportView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .filter-label { font-weight: 600; font-size: 13px; color: #334155; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Challan View</div>
                <div class="ms-auto">
                    <div class="btn-group">
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-sm btn-outline-info"
                            NavigateUrl="~/SInventory_UI/CreatePickingOnWareHouse.aspx"><i class="fa fa-backward"></i>&nbsp;Back to List</asp:HyperLink>
                    </div>
                </div>
            </div>
            <!--end breadcrumb-->
            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <script type="text/javascript">
                                        function pageLoad() {
                                            $('.datepicker').pickadate({
                                                selectMonths: true,
                                                selectYears: true,
                                                format: 'dd-mmm-yyyy'
                                            });
                                        }
                                    </script>

                                    <asp:Label ID="MessageLabel" runat="server" ForeColor="#009900"></asp:Label>

                                    <div class="row mb-3 align-items-center">
                                        <label class="col-sm-2 col-form-label filter-label">Sales Center:</label>
                                        <div class="col-sm-4">
                                            <asp:DropDownList ID="salesCenterDropDownList" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="National Report" Visible="False"
                                                OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="True" />
                                        </div>
                                    </div>
                                    <div class="row mb-3 align-items-center">
                                        <label class="col-sm-2 col-form-label filter-label">Challan From Date:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="dateTextBox" runat="server" CssClass="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select From Date"></asp:TextBox>
                                        </div>
                                        <label class="col-sm-2 col-form-label filter-label">Challan To Date:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select To Date"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <div class="col-sm-12">
                                            <asp:Button ID="searchButton" runat="server" Text="View List" CssClass="btn btn-sm btn-primary" OnClick="searchButton_Click" />
                                            <asp:Button ID="btnReload" runat="server" Text="Reload" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnReload_Click" />
                                            <asp:Button ID="detailsButton" runat="server" Visible="false" Text="Details Report" CssClass="btn btn-sm btn-success" OnClick="detailsButton_Click" />
                                            <asp:Button ID="summaryButton" runat="server" Visible="false" Text="Summary Report" CssClass="btn btn-sm btn-dark" OnClick="summaryButton_Click" />
                                            <asp:Button ID="excelButton1" runat="server" Visible="false" Text="Export to Excel" CssClass="btn btn-sm btn-outline-success" OnClick="btnExportToExcel_Click" />
                                        </div>
                                    </div>

                                    <div class="table-responsive" id="divForGrid">
                                        <asp:GridView ID="reportListGridView" runat="server" CssClass="table blueTable" 
                                AutoGenerateColumns="False" DataKeyNames="ReqId"   onrowcommand="loadGridView_RowCommand" >
                                <Columns>
                                    <asp:BoundField DataField="ComUnitCode" HeaderText="D.C.Code" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="D.C.Name" />
                                    <asp:BoundField DataField="ReqNo" HeaderText="Req.No" />
                                    <asp:BoundField DataField="ReqDate" DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderText="Req.Date" />
                                    <asp:BoundField DataField="IssueChalanNo" HeaderText="DeliveryClnNo" />
                                    <asp:BoundField DataField="IssuChalanDate" DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderText="DeliveryClnDate" />

                                    <asp:TemplateField HeaderText="Print">
                                        <ItemTemplate >
                                            <asp:Button ID="printButton" runat="server" Font-Bold="False" CssClass="btn btn-sm btn-success"
                                                Font-Italic="False" Text="PRINT" onclick="printButton_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>




                                </Columns>
                            </asp:GridView>
                               <asp:GridView ID="summaryGridView" runat="server" CssClass="table blueTable" 
                                AutoGenerateColumns="False" >
                                <Columns>
                                      <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    
                                      <asp:BoundField DataField="TotalQuantity" HeaderText="Total Quantity" />
                                    
                                      <asp:BoundField DataField="TotalPriceAmount" HeaderText="Total Price Amount" />
                                    
                                      <asp:BoundField DataField="TotalVATAmount" HeaderText="Total VAT Amount" />
                                    <asp:BoundField DataField="TotalPriceAmountwithVat" HeaderText="Total Amount with Vat" />
                                 
                                </Columns>
                            </asp:GridView>
                               <asp:GridView ID="detailsGridView" runat="server" CssClass="table blueTable" 
                                AutoGenerateColumns="False" >
                                <Columns>
                                    <asp:BoundField DataField="ComUnitCode" HeaderText="D.C.Code" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="D.C.Name" />
                                    
                                    
                                      <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    
                                      <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                                    
                                    
                                      <asp:BoundField DataField="TotalQuantity" HeaderText="Total Quantity" />
                                    <asp:BoundField DataField="UnitPrice" HeaderText="TP Price" />
                                    
                                    
                                    
                                      <asp:BoundField DataField="TotalPriceAmount" HeaderText="Total Price Amount" />
                                    <asp:BoundField DataField="VATAmountPerUnit" HeaderText="VAT Amount Per Unit" />
                                    

                                    
                                      <asp:BoundField DataField="TotalVATAmount" HeaderText="Total VAT Amount" />
                                    <asp:BoundField DataField="TotalPriceAmountwithVat" HeaderText="Total Amount with Vat" />
                                    
                                    
<%--
                                    <asp:BoundField DataField="ReqNo" HeaderText="Req.No" />
                                    <asp:BoundField DataField="ReqDate" DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderText="Req.Date" />--%>
                                    <asp:BoundField DataField="IssueChalanNo" HeaderText="DeliveryClnNo" />
                                    <asp:BoundField DataField="SubmitDate" DataFormatString="{0:dd/MM/yyyy}" 
                                        HeaderText="DeliveryClnDate" />
                                 
                             <%--       <asp:TemplateField HeaderText="Print">
                                        <ItemTemplate>
                                            <asp:Button ID="printButton" runat="server" Font-Bold="False" 
                                                Font-Italic="False" Text="PRINT" onclick="printButton_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="excelButton1" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
