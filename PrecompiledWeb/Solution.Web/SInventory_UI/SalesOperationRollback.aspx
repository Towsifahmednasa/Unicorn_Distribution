<%@ page title="Sales Operation Rollback" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_SalesOperationRollback, App_Web_yureiwkh" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Sales Operation Rollback</div>
                    </div>

                    <div class="row">
                        <div class="col">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-2">&nbsp;</div>
                                        <div class="col-8">
                                            
                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Operation Type</label>
                                                <div class="col-sm-7 pt-2">
                                                    <div class="form-check form-check-inline d-inline-block me-3">
                                                        <asp:RadioButton ID="rdoProforma" runat="server" GroupName="RollbackType" Text="Proforma Invoice" AutoPostBack="true" OnCheckedChanged="RollbackType_CheckedChanged" Checked="true" />
                                                    </div>
                                                    <div class="form-check form-check-inline d-inline-block me-3">
                                                        <asp:RadioButton ID="rdoDelivery" runat="server" GroupName="RollbackType" Text="Delivery Invoice" AutoPostBack="true" OnCheckedChanged="RollbackType_CheckedChanged" />
                                                    </div>
                                                    <div class="form-check form-check-inline d-inline-block">
                                                        <asp:RadioButton ID="rdoPayment" runat="server" GroupName="RollbackType" Text="Customer Payment" AutoPostBack="true" OnCheckedChanged="RollbackType_CheckedChanged" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <asp:Label ID="lblCodeLabel" runat="server" Text="Proforma Invoice No" CssClass="col-sm-3 col-form-label"></asp:Label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtCode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                            <div class="form-group row mb-3">
                                                <label class="col-sm-3 col-form-label">Remarks / Reason</label>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </div>
                                                <span class="text-sm-left text-c-red">*</span>
                                            </div>

                                        </div>
                                        <div class="col-2">&nbsp;</div>
                                    </div>

                                    <div class="row mt-4">
                                        <div class="col-2">&nbsp;</div>
                                        <div class="col-8">
                                            <div class="form-group row">
                                                <label class="col-sm-3 col-form-label"></label>
                                                <div class="col-sm-8">
                                                    <asp:LinkButton ID="btnRollback" runat="server" CssClass="btn btnMyDesignSearch btn-sm" OnClick="btnRollback_Click" OnClientClick="return confirm('Are you sure you want to perform this rollback/delete operation?');">
                                                        <i class="fa fa-trash"></i>&nbsp;Rollback / Delete
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-2">&nbsp;</div>
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
