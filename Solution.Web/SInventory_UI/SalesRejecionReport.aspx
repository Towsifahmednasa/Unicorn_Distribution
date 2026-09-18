<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="SalesRejecionReport.aspx.cs" Inherits="SInventory_UI_SalesRejecionReport" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
     <style type="text/css"> 
        .excel-button{
            margin-left: 5px;
        }
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


    
    
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i>  Sales Rejection Report </div>
                
                <div class="ms-auto">
                    <div class="btn-group">

                      
                    </div>
                </div>
            </div>
            <!--end breadcrumb-->
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

                       <script type="text/javascript">
                           function pageLoad() {
                               $('.mySelect2').select2({
                                   theme: 'bootstrap4',
                                   width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                                   placeholder: $(this).data('placeholder'),
                                   allowClear: Boolean($(this).data('allow-clear')),
                               });
                               $('.datepicker').pickadate({
                                   selectMonths: true,
                                   selectYears: true
                               })

                           }
                       </script>
                    <div class="row">
                          <div class="col-md-4">
                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end"></label>
                                   <div class="col-sm-8">
                                       <asp:CheckBox ID="CheckBox1" Text="National Report" CssClass="SelectchkChoice" runat="server" AutoPostBack="True" oncheckedchanged="CheckBox1_CheckedChanged1" />
                                   </div>
                               </div>

                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">Sales Center: <span class="text-danger">*</span></label>
                                   <div class="col-sm-8">
                                       <asp:DropDownList ID="dcDropDownList1" runat="server" CssClass="form-select form-select-sm mySelect2" 
                                           AutoPostBack="True" onselectedindexchanged="dcDropDownList1_SelectedIndexChanged"></asp:DropDownList>
                                   </div>
                               </div>
                          </div>

                          <div class="col-md-4">
                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">From Date: <span class="text-danger">*</span></label>
                                   <div class="col-sm-8">
                                       <asp:TextBox ID="InvoiceDateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Invoice From Date"></asp:TextBox>
                                   </div>
                                </div>
                          </div>

                          <div class="col-md-4">
                               <div class="row mb-3 align-items-center" runat="server">
                                   <label class="col-sm-4 col-form-label text-end">To Date: <span class="text-danger">*</span></label>
                                   <div class="col-sm-8">
                                       <asp:TextBox ID="todateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Invoice To Date"></asp:TextBox>
                                   </div>
                               </div>
                          </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-12 text-center">
                            <asp:LinkButton OnClick="SearchButton_Click" Visible="false" runat="server" id="SearchButton" class="btn btnMyDesignSearch btn-sm" style="margin-right: 8px;">
                                <i class="fa fa-print" aria-hidden="true"></i>&nbsp; View Report
                            </asp:LinkButton>
                            <asp:LinkButton OnClick="excelButton_OnClick" runat="server" id="excelButton" class="btn btnMyDesignSearch btn-sm" style="margin-right: 8px;">
                                <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" OnClick="Unnamed_Click" class="btn btnMyDesignReset btn-sm">
                                <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset
                            </asp:LinkButton>
                        </div>
                    </div></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                            </div>
                            </div>
                            </div>
                            </div>
                            </div>
                            </div>


 
</asp:Content>



