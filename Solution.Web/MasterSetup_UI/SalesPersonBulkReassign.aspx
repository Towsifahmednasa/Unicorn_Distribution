<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="SalesPersonBulkReassign.aspx.cs" Inherits="MasterSetup_UI_SalesPersonBulkReassign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .radioChoice label {
            padding-left: 5px;
            padding-right: 30px;
            font-size: 20px;
            font-weight: bold;
        }
    </style>

    <div class="page-wrapper">
        <div class="page-content">
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>Sales Person Bulk Reassign</div>
            </div>
            <div class="row">
                <div class="col">
                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="progress" runat="server" ClientIDMode="Static" DisplayAfter="0" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="divWaiting">
                                                <asp:Image ID="imgWait" CssClass="position-set" runat="server" ImageAlign="Middle" ImageUrl="../images/Spinner.gif" Width="180px" Height="180px" />
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>

                                    <div class="row">
                                        <div class="col-md-12" style="text-align: center">
                                            <asp:RadioButtonList runat="server" ID="rbType" CssClass="radioChoice" AutoPostBack="True" OnSelectedIndexChanged="rbType_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                <asp:ListItem Selected="True" Value="0">Customer</asp:ListItem>
                                                <asp:ListItem Value="1">Doctor</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <script type="text/javascript">
                                                function pageLoad() {
                                                    $('.multiple-select').select2({
                                                        theme: 'bootstrap4',
                                                        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                                        placeholder: $(this).data('placeholder'),
                                                        allowClear: Boolean($(this).data('allow-clear'))
                                                    });
                                                    $('.mySelect2').select2({
                                                        theme: 'bootstrap4',
                                                        width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                                        placeholder: $(this).data('placeholder'),
                                                        allowClear: Boolean($(this).data('allow-clear'))
                                                    });
                                                }
                                            </script>
                                        </div>
                                    </div>
                                    <br />

                                    <div class="row">
                                        <div class="col-md-6">
                                            <fieldset class="for-panel">
                                                <legend>From (remove from)</legend>
                                                <div class="input-group">
                                                    <asp:ListBox runat="server" ID="lbFrom" SelectionMode="Multiple" CssClass="form-select form-select-sm mb-3 multiple-select" data-placeholder="Select Sales Person(s)"></asp:ListBox>
                                                </div>
                                                <asp:LinkButton runat="server" ID="btnSearch" class="btn btnMyDesignAddtoList btn-sm" OnClick="btnSearch_Click">
                                                    <i class="fa fa-search-plus"></i>Search &nbsp;
                                                </asp:LinkButton>
                                            </fieldset>
                                        </div>
                                        <div class="col-md-6">
                                            <fieldset class="for-panel">
                                                <legend>To (assign to)</legend>
                                                <div class="input-group">
                                                    <asp:DropDownList runat="server" ID="ddlTo" CssClass="form-select form-select-sm mb-3 mySelect2"></asp:DropDownList>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>

                                    <br />
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvList" runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                            CssClass="table table-striped table-bordered" OnPreRender="gvList_PreRender">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL#">
                                                    <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" CssClass="form-control-sm" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" CssClass="form-control-sm" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Code" HeaderText="Code" />
                                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                                <asp:BoundField DataField="TaggedIds" HeaderText="Tagged Sales Person Ids" />
                                                <asp:BoundField DataField="TaggedSp" HeaderText="Tagged Sales Person Codes" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <br />
                                    <asp:LinkButton OnClick="btnSave_Click" runat="server" ID="btnSave" class="btn btnMyDesignSearch btn-sm" OnClientClick="return sweetAlertConfirm_Submit(this);">
                                        <i class="fa fa-check"></i>Reassign
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnReset" OnClick="btnReset_Click" class="btn btnMyDesignReset btn-sm"><i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset </asp:LinkButton>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
