<%@ page title="Product Information List" language="C#" masterpagefile="~/MasterPages/NewMasterPage.master" autoeventwireup="true" inherits="SInventory_UI_ProductView, App_Web_02kl0opo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


     <div class="page-wrapper">
        <div class="page-content">
            <!--breadcrumb-->
            <div class="page-breadcrumb d-flex align-items-center mb-3">
                <div class="breadcrumb-title pe-3"><i class="bx bx-customize"></i> Product Information List</div>
                <div class="ms-auto">
                    <a href="ProductEntry.aspx" class="btn btn-sm btn-primary px-3 shadow-sm text-white fw-bold">
                        <i class="bx bx-plus-circle me-1"></i> + New Product Entry
                    </a>
                </div>
            </div>
            <!--end breadcrumb-->
            <div class="row">
                <div class="col">

                    <div class="card border-top border-0 border-4 border-success">
                        <div class="card-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <!-- Search Panel -->
                                    <div class="row mb-3 align-items-end g-3">
                                        <div class="col-md-3">
                                            <label class="form-label fw-bold">Product Line:</label>
                                            <asp:DropDownList ID="productLineDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label fw-bold">Product Category:</label>
                                            <asp:DropDownList ID="categoryDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label fw-bold">Generic Group:</label>
                                            <asp:DropDownList ID="genericGroupDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3 d-flex gap-2">
                                            <asp:Button ID="searchButton" runat="server" CssClass="btn btn-sm btn-primary flex-grow-1" Text="Search" OnClick="searchButton_Click" />
                                            <asp:Button ID="resetButton" runat="server" CssClass="btn btn-sm btn-secondary flex-grow-1" Text="Reset" OnClick="resetButton_Click" />
                                        </div>
                                    </div>
                                    <hr />
                                    



                                            <div class="table-responsive" id="MainGradeDiv">

                                               

                                                 <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="ProductId" 
                                onrowcommand="loadGridView_RowCommand" CssClass="table table-striped table-bordered table-hover" OnPreRender="gv_DocumentUpload_PreRender">
                                <Columns>
                                    <asp:BoundField DataField="GenericGroupName" HeaderText="Generic Group" />
                                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                    <asp:BoundField DataField="ProductCode" HeaderText="Product Code" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="StockUOMName" HeaderText="UOM" />

                                    <asp:BoundField DataField="PackSize" HeaderText="Pack Size" />
                                    <%--<asp:BoundField DataField="CategoryName" HeaderText="Category" />--%>
                                    <asp:BoundField DataField="GroupName" HeaderText="Product Group" />
                                     <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <span class='<%# Eval("CSSStatus")%>'>  <%# Eval("StatusInfo")%>  </span>
                                               </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                              <asp:LinkButton ID="editImageButton" runat="server" CssClass="btn-warning  btn-sm mb-1 mb-md-0"
                                                                    CommandArgument="<%# Container.DataItemIndex %>" CommandName="EditData"> <i class="bx bxs-edit " aria-hidden="true"></i> </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                                            </div>


                                            
                                      


                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function pageLoad() {
            $('.mySelect2').select2({
                theme: 'bootstrap4',
                width: $(this).data('width') ? $(this).data('width') : $(this).hasClass('w-100') ? '100%' : 'style',
                placeholder: $(this).data('placeholder'),
                allowClear: Boolean($(this).data('allow-clear')),
            });
        }

        $(document).ready(function () {

            var table = $('#ContentPlaceHolder1_loadGridView').DataTable(
                {
                    "bInfo": true,
                    "bFilter": true,
                    lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                    pageLength: 10,
                    dom: 'lBfrtip',


                    buttons: ['copy', 'excel', 'pdf', 'print']
                }
            );

            var prm = Sys.WebForms.PageRequestManager.getInstance();
            if (prm != null) {
                prm.add_endRequest(function (sender, e) {
                    if (sender._postBackSettings.panelsToUpdate != null) {
                        table = $('#ContentPlaceHolder1_loadGridView').DataTable(
                            {
                                "bInfo": true,
                                "bFilter": true,
                                lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
                                pageLength: 10,
                                dom: 'lBfrtip',


                                buttons: ['copy', 'excel', 'pdf', 'print']


                            }
                        );
                    }
                });
            };


            table.columns().every(function () {
                var that = this;

               
            });
        });


    </script>

    
            


</asp:Content>

