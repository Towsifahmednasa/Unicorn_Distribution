<%@ page title="" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ManualOrderApproval, App_Web_yureiwkh" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .align-middle {
            margin-left: 37px;
        }

        .pd-left {
            padding-left: 5px;
        }

        .radioButtonList {
            list-style: none;
            margin: 0;
            padding: 0;
        }

        .radioButtonList.horizontal li {
            display: inline;
        }

        .radioButtonList label {
            display: inline;
        }

        .manual-approval-modal {
            background-color: rgba(15, 23, 42, 0.45);
        }

        .manual-approval-panel {
            width: 1000px;
            max-width: 96vw;
            max-height: 85vh;
            overflow: auto;
            border-radius: 10px;
            background: #fff;
            box-shadow: 0 24px 60px rgba(15, 23, 42, 0.25);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Manual Order Approval </div>

                        <div class="ms-auto">
                            <div class="btn-group">
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-2">&nbsp;</div>
                                            <div class="col-8">
                                                <div class="form-group row">
                                                    <div class="col-sm-5"></div>
                                                    <div class="col-sm-5">
                                                        <span style="font-size: 15px; font-weight: bold; text-align: left; padding-right: 10px; line-height: 35px;">Take Action:</span>
                                                        <asp:RadioButtonList ID="statusRadioButtonList" CssClass="radioButtonList" runat="server">
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <br />
                                        <br />
                                        <br />

                                        <div class="row">
                                            <div class="col-2">&nbsp;</div>
                                            <div class="col-8">
                                                <div class="form-group row">
                                                    <div class="col-md-4"></div>
                                                    <div class="col-sm-8">
                                                        <asp:LinkButton ID="submitButton" CssClass="btn btn-sm btn-primary mb-2" runat="server" OnClick="btnSubmit0_Click" style="background-color: #00bcd4;color: #fff;">
                                                            <i class="fa fa-check-square"></i>&nbsp; Submit
                                                        </asp:LinkButton>
                                                        <asp:LinkButton ID="cancelButton" class="btn btn-sm btn-warning mb-2" style="background-color: orangered; color: #fff;" runat="server" OnClick="cancelButton_Click">
                                                            <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset Information
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-2">&nbsp;</div>
                                        </div>

                                        <br />

                                        <div class="row">
                                            <div class="table-responsive" id="MainGradeDiv">
                                                <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False" CssClass="table blueTable"
                                                    OnPreRender="gv_DocumentUpload_PreRender"
                                                    DataKeyNames="OrderId,CustomerMasterId"
                                                    OnRowCommand="loadGridView_RowCommand">
                                                    <Columns>
                                                        <asp:BoundField DataField="OrderCode" HeaderText="Order Code" />
                                                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                                        <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                                        <asp:BoundField DataField="SubmissionDate" HeaderText="Order Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                        <asp:BoundField DataField="MIOName" HeaderText="MIO Name" />
                                                        <asp:BoundField DataField="MarketName" HeaderText="Market Name" />
                                                        <asp:BoundField DataField="TerritoryName" HeaderText="Territory Name" />
                                                        <asp:BoundField DataField="RegionName" HeaderText="Region Name" />
                                                        <asp:BoundField DataField="AreaName" HeaderText="Area Name" />
                                                        <asp:BoundField DataField="QTY" HeaderText="QTY" />
                                                        <asp:BoundField DataField="GrossValue" HeaderText="Gross Value" />
                                                        <asp:BoundField DataField="Due" HeaderText="Total Due" />
                                                        <asp:TemplateField HeaderText="Preview">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="invoiceButton" runat="server" CssClass="btn btn-outline-info btn-sm"
                                                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="PreviewDueHistory">
                                                                    Due History
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="HiddenField10" runat="server"></asp:HiddenField>
            <asp:ModalPopupExtender ID="ModalPopupExtender10" runat="server" TargetControlID="HiddenField10"
                PopupControlID="Panel10" BackgroundCssClass="manual-approval-modal">
            </asp:ModalPopupExtender>

            <asp:Panel ID="Panel10" runat="server" CssClass="manual-approval-panel" Style="display: none;">
                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <div style="text-align: center; padding: 15px;">
                            <table width="100%">
                                <tr>
                                    <td colspan="6">
                                        <h3>Due History</h3>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td width="20%" class="TDRight" colspan="6">
                                        <div style="height: auto; overflow: auto; width: 97%; margin: 0 auto;">
                                            <asp:GridView ID="dueGridView" runat="server" AutoGenerateColumns="False" CssClass="table blueTable"
                                                DataKeyNames="CustomerMasterId" OnPreRender="dueGridView_PreRender">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SL">
                                                        <ItemTemplate>
                                                            <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                                    <asp:BoundField DataField="ComUnitName" HeaderText="Unit Name" />
                                                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice No" />
                                                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                    <asp:BoundField DataField="DueAmount" HeaderText="Due Amount" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" class="text-center" style="padding-top: 15px;">
                                        <asp:LinkButton ID="Button41" runat="server" CssClass="btn btn-danger btn-sm" OnClick="Button41_OnClick">
                                            <i class="fa fa-times"></i>&nbsp;Close
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
