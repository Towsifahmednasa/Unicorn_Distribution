<%@ page title="" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_DeliverySalesReturn, App_Web_02kl0opo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <!--breadcrumb-->
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Delivery Sales Return</div>
                    </div>
                    <!--end breadcrumb-->

                    <div class="row">
                        <div class="col-12">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">
                                    <!-- Search Section -->
                                    <div class="row mb-3 align-items-center">
                                        <label class="col-sm-2 col-form-label text-end fw-bold">Delivery Invoice No:</label>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="delInvNoTextBox0" runat="server" AutoPostBack="True" OnTextChanged="Button2_Click" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-2">
                                            <asp:Button ID="Button2" runat="server" CssClass="btn btn-sm btn-primary w-100" Text="Search" OnClick="Button2_Click" />
                                        </div>
                                    </div>

                                    <div class="row mb-3 align-items-center" style="display: none;">
                                        <label class="col-sm-2 col-form-label text-end fw-bold">Payment Type:</label>
                                        <div class="col-sm-4">
                                             <asp:RadioButtonList ID="paymentTypeRadioButtonList" RepeatDirection="Horizontal" runat="server" CssClass="d-inline-flex gap-3">
                                                <asp:ListItem Text="Cash" Value="Cash" Selected="True" class="me-3" />
                                                <asp:ListItem Text="Credit" Value="Credit" />
                                             </asp:RadioButtonList>
                                        </div>
                                    </div>

                                    <hr />

                                    <!-- Details Section -->
                                    <div id="uiel" runat="server" visible="True" class="mt-4 border p-4 bg-light rounded shadow-sm">
                                        <!-- Customer Section -->
                                        <div class="row mb-3 border-bottom pb-2">
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Customer Code:</span>
                                                <asp:Label ID="custCodeTextBox" runat="server" CssClass="ms-2 text-dark fw-bold"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Customer Name:</span>
                                                <asp:Label ID="custNameTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Customer Address:</span>
                                                <asp:Label ID="custAddressTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                        </div>

                                        <!-- Order Section -->
                                        <div class="row mb-3 border-bottom pb-2">
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Order No:</span>
                                                <asp:Label ID="orderNoTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Order Date:</span>
                                                <asp:Label ID="orderDateTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Order Send by:</span>
                                                <asp:Label ID="salesPersonNameLabel" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                        </div>

                                        <!-- Invoice & Payment Section -->
                                        <div class="row mb-3 border-bottom pb-2">
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Invoice No:</span>
                                                <asp:Label ID="invoiceNoLabel" runat="server" CssClass="ms-2 text-dark fw-bold text-primary"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Invoice Date:</span>
                                                <asp:Label ID="invDateTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Payment Amount:</span>
                                                <asp:Label ID="payamntTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                        </div>

                                        <!-- Delivery Info Section -->
                                        <div class="row mb-3 border-bottom pb-2">
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Delivery Invoice No:</span>
                                                <asp:Label ID="deliveryInvoiceNoLabel" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">Update Date:</span>
                                                <asp:Label ID="updateDateLabel" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                            <div class="col-md-4">
                                                <span class="fw-bold text-secondary">DA Name:</span>
                                                <asp:Label ID="daNameLabel" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <div class="col-md-12">
                                                <span class="fw-bold text-secondary">Sales Center:</span>
                                                <asp:Label ID="comUnitNameTextBox" runat="server" CssClass="ms-2 text-dark"></asp:Label>
                                            </div>
                                        </div>

                                        <!-- Hidden controls kept for code-behind references -->
                                        <asp:Label ID="deliverypersonMobileTextBox" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="deliverypersonNameTextBox" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="remarksTextBox" runat="server" Visible="false"></asp:Label>

                                        <asp:HiddenField ID="invoiceNoHiddenField" runat="server" />
                                        <asp:HiddenField ID="invoiceHiddenField" runat="server" />
                                        <asp:HiddenField ID="orderIdHiddenField" runat="server" />
                                        <asp:HiddenField ID="hdCustomerMasterId" runat="server" />
                                        <asp:HiddenField ID="hdComUnitId" runat="server" />
                                        <asp:HiddenField ID="hdMiaId" runat="server" />

                                        <div class="my-4">
                                            <div class="table-responsive">
                                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" 
                                                    CssClass="table table-striped table-bordered table-sm align-middle"
                                                    DataKeyNames="InvoiceId,InvoiceDetailId,UnitPrice,UnitVatAmount">
                                                    <Columns>
                                                        <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                                        <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                                        <asp:BoundField DataField="AvialableQty" HeaderText="Available Qty" />
                                                        <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" />
                                                        <asp:TemplateField HeaderText="Return Qty">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="retQtyTextBox" runat="server" AutoPostBack="True" OnTextChanged="retQtyTextBox_OnTextChanged" CssClass="form-control form-control-sm" style="width: 100px;"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Return Amount">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="retamountTextBox" runat="server" AutoPostBack="True" Enabled="False" CssClass="form-control form-control-sm" style="width: 120px;"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <!-- Return Calculation & Deduction Section -->
                                        <div class="card my-3" style="background-color: #f8fbf9; border: 1px solid #b8dfd8; border-left: 4px solid #28a745; border-radius: 6px;">
                                            <div class="card-body p-3">
                                                <div class="row align-items-center">
                                                    <div class="col-md-4 col-sm-12 mb-2">
                                                        <label class="form-label font-weight-bold text-dark" style="font-size: 13px; margin-bottom: 2px;">
                                                            Total Product Return Amount:
                                                        </label>
                                                        <asp:TextBox ID="txtTotalReturnAmount" runat="server" ReadOnly="True" CssClass="form-control form-control-sm font-weight-bold text-primary" Text="0.00" style="background-color: #e9ecef; font-size: 14px;"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-4 col-sm-12 mb-2">
                                                        <label class="form-label font-weight-bold text-danger" style="font-size: 13px; margin-bottom: 2px;">
                                                            <i class="fa fa-minus-circle"></i> Return Delivery / Deduction Charge:
                                                        </label>
                                                        <asp:TextBox ID="txtDeductionCharge" runat="server" AutoPostBack="True" OnTextChanged="txtDeductionCharge_TextChanged" 
                                                            oninput="calcNetReturn();" onkeyup="calcNetReturn();"
                                                            CssClass="form-control form-control-sm font-weight-bold text-danger" Text="0.00" style="font-size: 14px;"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="ftbeDeduction" runat="server"
                                                            Enabled="True" TargetControlID="txtDeductionCharge" FilterType="Custom" ValidChars="0123456789.">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>

                                                    <div class="col-md-4 col-sm-12 mb-2">
                                                        <label class="form-label font-weight-bold text-success" style="font-size: 13px; margin-bottom: 2px;">
                                                            <i class="fa fa-check-circle"></i> Net Customer Return / Credit:
                                                        </label>
                                                        <asp:TextBox ID="txtNetReturnAmount" runat="server" ReadOnly="True" CssClass="form-control form-control-sm font-weight-bold text-success" Text="0.00" style="background-color: #e9ecef; font-size: 14px;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <script type="text/javascript">
                                            function calcNetReturn() {
                                                var txtTotal = document.querySelector('[id$="txtTotalReturnAmount"]');
                                                var txtDed = document.querySelector('[id$="txtDeductionCharge"]');
                                                var txtNet = document.querySelector('[id$="txtNetReturnAmount"]');
                                                if (txtTotal && txtDed && txtNet) {
                                                    var total = parseFloat(txtTotal.value.replace(/,/g, '')) || 0;
                                                    var ded = parseFloat(txtDed.value.replace(/,/g, '')) || 0;
                                                    var net = total - ded;
                                                    if (net < 0) net = 0;
                                                    txtNet.value = net.toFixed(2);
                                                }
                                            }
                                        </script>

                                        <div class="row mt-3">
                                            <div class="col-12 text-center">
                                                <asp:Button ID="Button1" runat="server" Text="Save Return" OnClick="saveButton_Click" CssClass="btn btn-sm btn-success px-4 me-2"
                                                    OnClientClick="return confirm('Are you sure you want to Return Invoice ?');" /> 
                                                <asp:Button ID="clearButton" runat="server" OnClick="clearButton_OnClick" CssClass="btn btn-sm btn-danger px-4"
                                                    Text="Clear" />
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
