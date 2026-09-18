<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master" AutoEventWireup="true" CodeFile="DeliveryReturnReport_new.aspx.cs" Inherits="SInventory_UI_DeliveryReturnReport_new" %>
<%@ Register TagPrefix="cc1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.20820.28364, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%--<%@ Register Src="../SInventory_UI/IVMarketStructureInvoSearch.ascx" TagPrefix="uc1" TagName="IVMarketStructure" %> 
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
     <style type="text/css">

            .ssss {
            font-size: 13px;
            font-weight: bold;
        }
        .button-padding-right {
            margin-right: 5px;
        }  
          .SelectchkChoice label {
            padding-left: 4px;
            font-weight: bold;
        }
    </style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    
    
    <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Delivery Return Report</div>
                
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
                                       <asp:TextBox AutoPostBack="true" OnTextChanged="fromDateTextBox_TextChanged" ID="InvoiceDateTextBox" runat="server" class="form-control form-control-sm datepicker" autocomplete="off" placeholder="Select Invoice From Date"></asp:TextBox>
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
                    
                    <br />

                    <div class="row mb-3">
                        <div class="col-12 text-center">
                            <asp:LinkButton OnClick="SearchButton_Click" runat="server" id="submitButton" class="btn btnMyDesignSearch btn-sm" style="margin-right: 8px;">
                                <i class="fa fa-print" aria-hidden="true"></i>&nbsp; View Report
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cancelButton" OnClick="cancelButton_Click" class="btn btnMyDesignReset btn-sm">
                                <i class="fa fa-retweet" aria-hidden="true"></i>&nbsp; Reset
                            </asp:LinkButton>
                        </div>
                    </div>

                    <hr class="my-4" />

                    <div class="row align-items-center mb-3">
                        <div class="col-md-3">
                            <h3 class="mb-0">Details List</h3>
                        </div>
                        <div class="col-md-9">
                            <div class="d-flex flex-wrap justify-content-md-end justify-content-start gap-2 align-items-center">
                                <asp:Label ID="lblCount" runat="server" CssClass="ssss btn bg-info text-dark" Text="Total Net Amount : 0"></asp:Label>
                                <asp:LinkButton ID="btnExport" class="btn btn-sm text-white" style="background-color: #1A7343; border-color: #1A7343;" runat="server" OnClick="btnExport_Click">
                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp; Export to Excel
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                                        </div>
                    <hr />

             <div class="table-responsive" id="MainGradeDiv"  style="height:600px">

                          
                                    <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                 
                                CssClass="table table-striped table-bordered" OnPreRender="gv_DocumentUpload_PreRender"   AllowPaging="True" PageIndex="0" OnPageIndexChanging="loadGridView_PageIndexChanging">
                                <Columns>
                                      <%--  <asp:TemplateField HeaderText="SL">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                         
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="ComUnitCode" HeaderText="Sales Center Code" />
                                    <asp:BoundField DataField="ComUnitName" HeaderText="Sales Center Name" />
                                    
                                    <asp:BoundField DataField="CustomerCode" HeaderText="Customer ID" />
                                    <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" />


                                    <asp:BoundField DataField="CustomerType" HeaderText=" Customer Type" />
                                    <asp:BoundField DataField="OrderNo" HeaderText="Order Code" />
                                    <asp:BoundField DataField="OrderDate" HeaderText="Order / Submission Date" />
                                    <asp:BoundField DataField="InvoiceNo" HeaderText="Invoice Number" />
                                    <asp:BoundField DataField="InvoiceDate" HeaderText="Invoice Date" />
                                    <asp:BoundField DataField="DelivaryInvoiceNo" HeaderText="Delivary Invoice  Number" />

                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                    <asp:BoundField DataField="BatchNo" HeaderText="Batch No" />
                                    <asp:BoundField DataField="ExpDate" HeaderText="Exp Date" />
                                    <asp:BoundField DataField="ReturnQuantity" HeaderText="Return Qty" />
                                    <asp:BoundField DataField="Amount" HeaderText="Net Amount" />
                                    <asp:BoundField DataField="VatAmount" HeaderText="VAT" />
                                    <asp:BoundField DataField="MiaCode" HeaderText="MIO EMP Code" />
                                    <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" />
                                    <asp:BoundField DataField="ReturnReason" HeaderText="Return Reason" />


                         
                           


                                    
                                </Columns>
                                          <PagerStyle HorizontalAlign="Left" CssClass="GridPager" />
                            </asp:GridView>
                                            </div>

                    </ContentTemplate>
                     <Triggers>
                 
                 <asp:PostBackTrigger ControlID="btnExport"/>
             </Triggers>
                </asp:UpdatePanel>
                            </div>

                            </div>
                            </div>
                            </div>
                            </div>
                            </div> 


     <script>

         function exportToExcel() {

             var file = new Blob([$('#MainGradeDiv').html()], { type: "application/vnd.ms-excel" });
             var url = URL.createObjectURL(file);
             var a = $("<a />", {
                 href: url,
                 download: "Invoice Report.xls"
             }).appendTo("body").get(0).click();
             e.preventDefault();

         }

         function exportTableToExcel(tableID, filename) {
             var downloadLink;
             var dataType = 'application/vnd.ms-excel';
             var tableSelect = document.getElementById(tableID);
             var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

             // Specify file name
             filename = filename ? filename + '.xls' : 'excel_data.xls';

             // Create download link element
             downloadLink = document.createElement("a");

             document.body.appendChild(downloadLink);

             if (navigator.msSaveOrOpenBlob) {
                 var blob = new Blob(['\ufeff', tableHTML], {
                     type: dataType
                 });
                 navigator.msSaveOrOpenBlob(blob, filename);
             } else {
                 // Create a link to the file
                 downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

                 // Setting the file name
                 downloadLink.download = filename;

                 //triggering the function
                 downloadLink.click();
             }
         }
     </script>
     
</asp:Content>

