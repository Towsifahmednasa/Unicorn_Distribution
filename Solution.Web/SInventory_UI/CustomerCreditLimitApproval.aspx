<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master"
    AutoEventWireup="true" CodeFile="CustomerCreditLimitApproval.aspx.cs" Inherits="SInventory_UI_CustomerCreditLimitApproval" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Customer Credit Limit Approval </div>

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
                                                            <asp:ListItem Value="1" Text="Approve"></asp:ListItem>
                                                            <asp:ListItem Value="0" Text="Reject"></asp:ListItem>
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
                                                    DataKeyNames="CreditLimitId" OnPreRender="gv_DocumentUpload_PreRender">
                                                    <Columns>
                                                        <asp:BoundField DataField="CompanyName" HeaderText="Company Name" />
                                                        <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />
                                                        <asp:BoundField DataField="LimitAmount" HeaderText="Limit Amount" />
                                                        <asp:BoundField DataField="EntryBy" HeaderText="Entry User" />
                                                        <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" DataFormatString="{0:dd-MMM-yyyy}" />
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
        <ProgressTemplate>
            <div class="divWaiting">
                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="~/images/Spinner.gif"
                    Width="180px" Height="180px" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
