<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/NewMasterPage.master"
    AutoEventWireup="true" CodeFile="MaterialConversionReceive.aspx.cs" Inherits="SInventory_UI_MaterialConversionReceive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../assets/vendors/sweetalert2/sweetalert2.min.css" rel="stylesheet" />
    <style type="text/css">
        :root {
            --material-accent: #5a8dee;
            --material-accent-hover: #4b7fe0;
        }

        .radio-inline-list label {
            margin-right: 12px;
        }

        .converted-material-title {
            display: inline-flex;
            align-items: center;
            gap: 10px;
            margin: 0 0 14px;
            padding: 8px 18px 8px 14px;
            border-radius: 999px;
            background: linear-gradient(135deg, #eef4ff 0%, #f9fbff 100%);
            border: 1px solid #d9e5ff;
            box-shadow: 0 8px 22px rgba(90, 141, 238, 0.14);
            font-size: 18px;
            font-weight: 700;
            color: var(--material-accent);
            letter-spacing: 0.01em;
        }

        .converted-material-title:before {
            content: "\f0ad";
            font-family: FontAwesome;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 28px;
            height: 28px;
            border-radius: 50%;
            background-color: rgba(90, 141, 238, 0.12);
            color: var(--material-accent);
            font-size: 14px;
        }

        .material-page-title {
            display: inline-flex;
            align-items: center;
            gap: 10px;
            margin: 0;
            padding: 8px 18px 8px 14px;
            border-radius: 999px;
            background: linear-gradient(135deg, #eef4ff 0%, #f9fbff 100%);
            border: 1px solid #d9e5ff;
            box-shadow: 0 8px 22px rgba(90, 141, 238, 0.14);
            color: var(--material-accent);
            font-size: 18px;
            font-weight: 700;
            letter-spacing: 0.01em;
        }

        .material-page-title i {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 28px;
            height: 28px;
            border-radius: 50%;
            background-color: rgba(90, 141, 238, 0.12);
            color: var(--material-accent);
            font-size: 16px;
        }

        .material-form-card {
            border: 1px solid #e4ebf8;
            border-radius: 18px;
            padding: 14px;
            margin-bottom: 18px;
            background: linear-gradient(180deg, #ffffff 0%, #fbfcff 100%);
            box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.9);
        }

        #MainGridDiv {
            border: 1px solid #d9dfef;
            border-radius: 8px;
            overflow: hidden;
            background-color: #ffffff;
        }

        .converted-material-grid {
            margin-bottom: 0;
            border: 0;
        }

        .converted-material-grid > tbody > tr > th,
        .converted-material-grid > thead > tr > th {
            padding: 10px 12px;
            border: 1px solid var(--material-accent);
            background: var(--material-accent);
            color: #ffffff;
            font-size: 14px;
            font-weight: 700;
            vertical-align: middle;
            white-space: nowrap;
        }

        .converted-material-grid > tbody > tr > td {
            padding: 6px;
            border: 1px solid #dfe5f1;
            background-color: #ffffff;
            vertical-align: middle;
        }

        .converted-material-grid > tbody > tr:nth-child(even) > td {
            background-color: #fbfcff;
        }

        .converted-material-grid input[type="checkbox"] {
            width: 16px;
            height: 16px;
        }

        .converted-material-grid .form-control {
            min-height: 38px;
            border: 1px solid #ccd5e4;
            border-radius: 4px;
            background-color: #f7faff;
            color: #14213d;
            box-shadow: none;
        }

        .converted-material-grid .form-control[readonly] {
            background-color: #f3f6fb;
        }

        .converted-material-grid .form-control:focus {
            border-color: #8a97e6;
            box-shadow: 0 0 0 0.15rem rgba(98, 113, 216, 0.12);
        }

        .material-select {
            width: 100%;
        }

        .material-select-label {
            margin-bottom: 6px;
            color: #526587;
            font-size: 13px;
            font-weight: 600;
            letter-spacing: 0.01em;
        }

        .select2-container {
            width: 100% !important;
        }

        .select2-container .select2-selection--single {
            height: 36px;
            border: 1px solid #d5def0;
            border-radius: 10px;
            background: linear-gradient(180deg, #ffffff 0%, #fbfcff 100%);
            box-shadow: none;
        }

        .select2-container--default.select2-container--focus .select2-selection--single,
        .select2-container--default.select2-container--open .select2-selection--single {
            border-color: #9dbaf5;
            box-shadow: 0 0 0 0.15rem rgba(90, 141, 238, 0.12);
        }

        .select2-container--default .select2-results__option--highlighted[aria-selected],
        .select2-container--default .select2-results__option[aria-selected="true"] {
            background-color: var(--material-accent);
            color: #ffffff;
        }

        .select2-container .select2-selection--single .select2-selection__rendered {
            line-height: 34px;
            padding-left: 0.75rem;
            padding-right: 2rem;
            font-size: 0.875rem;
            color: #23314f;
        }

        .select2-container .select2-selection--single .select2-selection__placeholder {
            color: #6c757d;
        }

        .select2-container .select2-selection--single .select2-selection__arrow {
            height: 34px;
            right: 6px;
            width: 20px;
        }

        .select2-dropdown {
            border: 1px solid #ced4da;
            border-radius: 0.25rem;
        }

        .material-form-card .form-control:not(.converted-material-grid .form-control) {
            min-height: 36px;
            border: 1px solid #d5def0;
            border-radius: 10px;
            background: linear-gradient(180deg, #ffffff 0%, #fbfcff 100%);
            box-shadow: none;
            color: #23314f;
        }

        .material-form-card .form-control:not(.converted-material-grid .form-control):focus {
            border-color: #9dbaf5;
            box-shadow: 0 0 0 0.15rem rgba(90, 141, 238, 0.12);
        }

        .material-action-bar {
            margin-top: 20px;
            padding: 14px;
            border: 1px solid #e6ebf5;
            border-radius: 10px;
            background-color: #ffffff;
        }

        .material-action-buttons {
            display: flex;
            gap: 12px;
        }

        .material-action-buttons .btn {
            width: 50%;
            min-height: 42px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            border-radius: 6px;
            font-size: 15px;
            font-weight: 500;
        }

        .material-reset-btn {
            border: 1px solid #dfe7f5;
            background-color: #edf3ff;
            color: #667085;
        }

        .material-reset-btn:hover,
        .material-reset-btn:focus {
            border-color: #d4deef;
            background-color: #e7eefc;
            color: #5d697d;
        }

        .material-submit-btn {
            border: 1px solid var(--material-accent);
            background-color: var(--material-accent);
            color: #ffffff;
        }

        .material-submit-btn:hover,
        .material-submit-btn:focus {
            border-color: var(--material-accent-hover);
            background-color: var(--material-accent-hover);
            color: #ffffff;
        }

        .swal-material-card {
            border-radius: 24px;
            padding: 1.25rem;
        }

        .swal-material-heading {
            font-size: 1.4rem;
            font-weight: 700;
        }

        .swal-material-body {
            font-size: 0.98rem;
            line-height: 1.6;
        }
    </style>
    <script src="../assets/vendors/sweetalert2/sweetalert2.min.js"></script>
    <script type="text/javascript">
        (function (w) {
            function buildMaterialSwalOptions(type, title, text) {
                var iconColorMap = {
                    success: "#22c55e",
                    error: "#f97373",
                    warning: "#f97316",
                    info: "#5a8dee",
                    question: "#4f46e5"
                };

                return {
                    icon: type || "info",
                    iconColor: iconColorMap[type] || "#5a8dee",
                    title: title || "",
                    text: text || "",
                    buttonsStyling: false,
                    confirmButtonText: "OK",
                    customClass: {
                        popup: "swal-material-card",
                        title: "swal-material-heading",
                        htmlContainer: "swal-material-body",
                        confirmButton: "btn btn-primary rounded-pill px-4"
                    }
                };
            }

            w.showMaterialAlert = function (type, title, text) {
                if (typeof Swal === "undefined") {
                    alert(text || title || "");
                    return;
                }

                Swal.fire(buildMaterialSwalOptions(type, title, text));
            };

            w.showMaterialSuccess = function (title, text) {
                if (typeof Swal === "undefined") {
                    alert(text || title || "");
                    return;
                }

                var options = buildMaterialSwalOptions("success", title, text);
                options.showConfirmButton = false;
                options.timer = 1400;
                options.timerProgressBar = true;
                Swal.fire(options);
            };

            w.confirmMaterialSubmit = function (btn) {
                if (btn && btn.dataset && btn.dataset.confirmed === "true") {
                    btn.dataset.confirmed = "false";
                    return true;
                }

                if (w.event && typeof w.event.preventDefault === "function") {
                    w.event.preventDefault();
                }

                var triggerConfirmedClick = function () {
                    if (!btn) {
                        return;
                    }

                    btn.dataset.confirmed = "true";
                    btn.click();
                };

                if (typeof Swal !== "undefined" && Swal.fire) {
                    Swal.fire({
                        icon: "question",
                        iconColor: "#4f46e5",
                        title: "Confirm submission",
                        text: "Do you want to save this stock in information?",
                        showCancelButton: true,
                        confirmButtonText: "Yes, Save",
                        cancelButtonText: "Cancel",
                        reverseButtons: true,
                        buttonsStyling: false,
                        customClass: {
                            popup: "swal-material-card",
                            title: "swal-material-heading",
                            htmlContainer: "swal-material-body",
                            confirmButton: "btn btn-primary rounded-pill px-4 me-2",
                            cancelButton: "btn btn-light rounded-pill px-4"
                        }
                    }).then(function (result) {
                        if (result && result.isConfirmed) {
                            triggerConfirmedClick();
                        }
                    });

                    return false;
                }

                if (typeof swal !== "undefined") {
                    swal({
                        title: "Are You Sure ?",
                        text: "You are about to submit the data!",
                        type: "info",
                        icon: "warning",
                        buttons: {
                            yes: {
                                text: "Confirm",
                                value: "yes"
                            },
                            no: {
                                text: "Cancel",
                                value: "no",
                                className: "",
                                closeModal: true
                            }
                        }
                    }).then(function (value) {
                        if (value === "yes") {
                            triggerConfirmedClick();
                        }
                    });

                    return false;
                }

                return confirm("Are you sure you want to Save ?");
            };

            function showMaterialLoading() {
                if (typeof Swal === "undefined") {
                    return;
                }

                Swal.fire({
                    title: "Processing",
                    text: "Please wait...",
                    allowEscapeKey: false,
                    allowOutsideClick: false,
                    showConfirmButton: false,
                    didOpen: function () {
                        Swal.showLoading();
                    },
                    customClass: {
                        popup: "swal-material-card",
                        title: "swal-material-heading",
                        htmlContainer: "swal-material-body"
                    }
                });
            }

            function closeMaterialLoading() {
                if (typeof Swal === "undefined") {
                    return;
                }

                if (Swal.isVisible()) {
                    Swal.close();
                }
            }

            function initMaterialConversionDatePicker() {
                if (typeof $ === "undefined" || typeof $.fn.pickadate === "undefined") {
                    return;
                }

                $(".datepicker").each(function () {
                    if (!$(this).data("pickadate")) {
                        $(this).pickadate({
                            selectMonths: true,
                            selectYears: true
                        });
                    }
                });
            }

            function initMaterialConversionSelect2() {
                if (typeof $ === "undefined" || typeof $.fn.select2 === "undefined") {
                    return;
                }

                $(".mySelect2").each(function () {
                    var $select = $(this);

                    if ($select.data("select2")) {
                        $select.select2("destroy");
                    }

                    $select.select2({
                        width: "100%",
                        placeholder: $select.attr("data-placeholder") || "Select an option"
                    });
                });
            }

            function hookUpdatePanelEndRequest() {
                if (typeof Sys === "undefined" || !Sys.WebForms || !Sys.WebForms.PageRequestManager) {
                    return;
                }

                if (w.__materialConversionHooksApplied) {
                    return;
                }

                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_initializeRequest(function () {
                    showMaterialLoading();
                });

                prm.add_endRequest(function () {
                    closeMaterialLoading();
                    initMaterialConversionDatePicker();
                    initMaterialConversionSelect2();
                });

                w.__materialConversionHooksApplied = true;
            }

            $(function () {
                initMaterialConversionDatePicker();
                initMaterialConversionSelect2();
                hookUpdatePanelEndRequest();
            });
        })(window);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="page-wrapper">
                <div class="page-content">
                    <div class="page-breadcrumb d-none d-sm-flex align-items-center mb-3">
                        <div class="breadcrumb-title pe-3 border-0">
                            <h1 class="material-page-title">
                                <i class="bx bx-customize"></i>
                                <span>Converted Material Stock In (WH)</span>
                            </h1>
                        </div>
                        <div class="ms-auto">
                            <asp:LinkButton ID="viewLinkButton" class="btn btn-sm btn-outline-info" runat="server"
                                OnClick="viewLinkButton_OnClick"><i class="fa fa-backward"></i>&nbsp;Back to List</asp:LinkButton>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col">
                            <div class="card border-top border-0 border-4 border-success">
                                <div class="card-body">
                                    <div class="material-form-card">
                                        <div class="row g-3">
                                        <div class="col-md-4">
                                            <label class="form-label material-select-label">Company <span style="color: red">*</span></label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-select form-select-sm mySelect2 material-select"
                                                data-placeholder="Select company"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_OnSelectedIndexChanged" />
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label material-select-label">Warehouse <span style="color: red">*</span></label>
                                            <asp:DropDownList ID="warehouseDropDownList" runat="server" CssClass="form-select form-select-sm mySelect2 material-select"
                                                data-placeholder="Select warehouse" />
                                            <asp:HiddenField ID="stockInIdHiddenField" runat="server" />
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label material-select-label">Production Unit <span style="color: red">*</span></label>
                                            <asp:DropDownList ID="ddlProductionUnit" runat="server" CssClass="form-select form-select-sm mySelect2 material-select"
                                                data-placeholder="Select production unit"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddlProductionUnit_OnSelectedIndexChanged" />
                                        </div>

                                        <div class="col-md-4">
                                            <label class="form-label">Stock In Date <span style="color: red">*</span></label>
                                            <asp:TextBox ID="stockInDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"
                                                AutoPostBack="True" OnTextChanged="stockInDateTextBox_OnTextChanged" />
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label">Challan No <span style="color: red">*</span></label>
                                            <asp:TextBox ID="challanNoTextBox" runat="server" CssClass="form-control form-control-sm" />
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label">Challan Date <span style="color: red">*</span></label>
                                            <asp:TextBox ID="challanDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"
                                                AutoPostBack="True" OnTextChanged="challanDateTextBox_OnTextChanged" />
                                        </div>

                                        <div class="col-md-4">
                                            <label class="form-label">Reference No</label>
                                            <asp:TextBox ID="referenceNoTextBox" runat="server" CssClass="form-control form-control-sm" />
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label">Reference Date</label>
                                            <asp:TextBox ID="referenceDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"
                                                AutoPostBack="True" OnTextChanged="referenceDateTextBox_OnTextChanged" />
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label">Remarks</label>
                                            <asp:TextBox ID="remarksTextBox" runat="server" CssClass="form-control form-control-sm" />
                                        </div>
                                        </div>
                                    </div>

                                    <asp:DropDownList ID="ddlSupplier" runat="server" Visible="False" CssClass="form-select form-select-sm mySelect2 material-select"
                                        data-placeholder="Select supplier" />

                                    <hr />

                                    <div class="converted-material-title">Product List</div>

                                    <div class="table-responsive" id="MainGridDiv">
                                        <asp:GridView ID="loadGridView" runat="server" AutoGenerateColumns="False"
                                            CssClass="table table-sm align-middle converted-material-grid"
                                            DataKeyNames="MaterialConversionMasterId,MaterialConversionDetailId"
                                            OnPreRender="gv_DocumentUpload_PreRender"
                                            OnRowCommand="loadGridView_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="SL">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LabelSL" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelect_OnCheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Conversion Code">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="conversionCodeTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("MaterialConversionCode") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Code">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="productCodeTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("ProductCode") %>' />
                                                        <asp:HiddenField ID="productidHiddenField" Value='<%# Eval("ProductId") %>' runat="server" />
                                                        <asp:HiddenField ID="hdfMaterialConversionMasterId" Value='<%# Eval("MaterialConversionMasterId") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product Name">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="productNameTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("ProductName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pack Size">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="packSizeTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("PackSize") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Batch">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="batchTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            Text='<%# Eval("Batch_No") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mfg. Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="mfgDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"
                                                            Text='<%# Eval("Mfg_Date", "{0:dd-MMM-yyyy}") %>'
                                                            AutoPostBack="True" OnTextChanged="mfgDateTextBox_OnTextChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Exp. Date">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="expDateDateTextBox" runat="server" CssClass="form-control form-control-sm datepicker"
                                                            Text='<%# Eval("Exp_Date", "{0:dd-MMM-yyyy}") %>'
                                                            AutoPostBack="True" OnTextChanged="expDateDateTextBox_OnTextChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="reqQtyTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("FinalOutputQuantityInt") %>'
                                                            AutoPostBack="True" OnTextChanged="reqQtyTextBox_OnTextChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="costPriceTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("CostPerUnit") %>'
                                                            AutoPostBack="True" OnTextChanged="costPriceTextBox_OnTextChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Vat" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="vatTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("Vat") %>'
                                                            AutoPostBack="True" OnTextChanged="vatTextBox_OnTextChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total Amt">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="totalValueTextBox" runat="server" CssClass="form-control form-control-sm"
                                                            ReadOnly="True" Text='<%# Eval("TotalCostOfThisBatch") %>'
                                                            AutoPostBack="True" OnTextChanged="totalValueTextBox_OnTextChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                    <div class="row mt-3">
                                        <div class="col-md-4 ms-auto">
                                            <div class="input-group input-group-sm mb-2">
                                                <span class="input-group-text">Total Qty</span>
                                                <asp:TextBox ID="totalQtyTextBox" runat="server" CssClass="form-control" ReadOnly="True" />
                                            </div>
                                            <div class="input-group input-group-sm mb-2">
                                                <span class="input-group-text">Total Vat</span>
                                                <asp:TextBox ID="totalVatTextBox" runat="server" CssClass="form-control" ReadOnly="True" />
                                            </div>
                                            <div class="input-group input-group-sm mb-2">
                                                <span class="input-group-text">Grand Total</span>
                                                <asp:TextBox ID="grandTotalTextBox" runat="server" CssClass="form-control" ReadOnly="True" />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="material-action-bar">
                                        <div class="material-action-buttons">
                                            <asp:LinkButton ID="cancelButton" CssClass="btn material-reset-btn" runat="server"
                                                OnClick="cancelButton_Click">
                                                <i class="fa fa-undo" aria-hidden="true"></i>&nbsp;Reset
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="submitButton" CssClass="btn material-submit-btn" runat="server"
                                                OnClick="submitButton_Click" OnClientClick="return confirmMaterialSubmit(this);">
                                                <i class="fa fa-floppy-o" aria-hidden="true"></i>&nbsp;Submit Material Stock In
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="submitButton" />
        </Triggers>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="2147483647" DynamicLayout="true">
        <ProgressTemplate>
            <div style="display: none;"></div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
