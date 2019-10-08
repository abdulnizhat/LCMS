<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GaugeMaster.aspx.cs" Inherits="GaugeMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
        <link type="text/css" href="js/searchabledropdown/3.3.7-css-bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="js/searchabledropdown/1.12.2-css-select.min.css" rel="stylesheet" />

    <style>
        .dropdownCustom {
            width: 280px !important;
        }

        .panel_popup {
            border-style: double;
            border-width: 2px;
            border-radius: 15px;
            -moz-border-radius: 15px;
            padding-left: 20px;
            background-color: #DDDDDD;
            padding-right: 20px;
            box-shadow: 0px 1px 15px #092137;
        }
    </style>

    <script>
        function Confirm() {
            $('#confirmationModal').modal('show');
            return true;
        };
        function isNumberKey(txt, evt) {

            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode == 46) {
                //Check if the text already contains the . character
                if (txt.value.indexOf('.') === -1) {
                    return true;
                } else {
                    return false;
                }
            } else {
                if (charCode > 31
                     && (charCode < 48 || charCode > 57))
                    return false;
            }
            return true;
        }
        function Confirm() {
            <%--var chk = '<%= Session["dleteGage"].ToString() %>';--%>
            if (chk == "YES") {
                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("Do you want to delete Gauge record?")) {
                    confirm_value.value = "Yes";
                } else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            }
        }
        function ShowPopup() {
            $("#btnShowPopup").click();
        }

        $(document).ready(function () {
            $('.isSelected').click(function (o) {
                var $eleId = $(this).attr("id");
                var splitId = $eleId;
                var sp = splitId.split('_');
                var spId = sp[3];
                //grdPlugTolerance_
                var texEtopR1 = document.getElementById('MainContent_grdPlugTolerance_rdGrd6_' + spId).value;
                alert(texEtopR1);
            });
        });

        function onchangeradio(o) {
            var gradeVal = o.innerText;
            var $eleId = o.children[0].id;
            var splitId = $eleId;
            var sp = splitId.split('_');
            var grade = sp[2];
            var spId = sp[3];
            var toleranceId = document.getElementById('MainContent_grdPlugTolerance_hfToleranceId_' + spId).value;
            var id = document.getElementById('MainContent_grdPlugTolerance_hfId_' + spId).value;
            var rangeVal = document.getElementById('MainContent_grdPlugTolerance_hfRangeVal_' + spId).value;

            $("#<%= hfGradValue.ClientID  %>").val(gradeVal);
            $("#<%= hftoleranceId.ClientID  %>").val(toleranceId);
            $("#<%= hfId.ClientID  %>").val(id);
            $("#<%=hfRange.ClientID %>").val(rangeVal);
            $("#<%=hfGrade.ClientID %>").val(grade); 
           
        }

        function onchangeradioOnSnapTolerance(o) {
            var gradeVal = o.innerText;
            var $eleId = o.children[0].id;
            var splitId = $eleId;
            var sp = splitId.split('_');
            var grade = sp[2];
            var spId = sp[3];
            var toleranceId = document.getElementById('MainContent_grdSnapTolerance_hfToleranceId_' + spId).value;
            var id = document.getElementById('MainContent_grdSnapTolerance_hfId_' + spId).value;
            var rangeVal = document.getElementById('MainContent_grdSnapTolerance_hfRangeVal_' + spId).value;

            $("#<%= hfGradValue.ClientID  %>").val(gradeVal);
            $("#<%= hftoleranceId.ClientID  %>").val(toleranceId);
            $("#<%= hfId.ClientID  %>").val(id);
            $("#<%=hfRange.ClientID %>").val(rangeVal);
            $("#<%=hfGrade.ClientID %>").val(grade);

        }


    </script>
    <%--   <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
  <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>--%>
    <h3><%: Title %>Gauge Master</h3>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="updaction" runat="server">
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnAddGauge" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="col-md-1">
                                            <asp:Button runat="server" ID="btnAddGauge" Text="Add New" OnClick="btnAddGauge_Click" CssClass="btn btn-primary" />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Search By<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlsortby" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsortby_SelectedIndexChanged" CssClass="form-control" TabIndex="2">
                                                    <asp:ListItem>--Select--</asp:ListItem>
                                                    <asp:ListItem>Gauge Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Name-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Sr.No.-Wise</asp:ListItem>
                                                    <asp:ListItem>Manufacture Id-Wise</asp:ListItem>
                                                    <asp:ListItem>Size/Range-Wise</asp:ListItem>
                                                    <asp:ListItem>Gauge Type-Wise</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>

                                        </div>

                                        <div class="col-md-5" runat="server" id="divSearchBy">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="lblName" Visible="false"></asp:Label>
                                            <asp:Label runat="server" CssClass="col-md-4 control-label" ID="searchBy" Visible="false"></asp:Label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtsearchValue" Visible="false" runat="server" TabIndex="3" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtsearchValue"
                                                    CssClass="text-danger" ErrorMessage="Required search value." />
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" ValidationGroup="a" Visible="false" CssClass="btn btn-primary" />
                                        </div>


                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                    <asp:GridView ID="grdGauge" runat="server" Width="100%" AutoGenerateColumns="false"
                                        CssClass="table table-bordered table-responsive" PageSize="10" AllowPaging="true" OnPageIndexChanging="grdGauge_PageIndexChanging">
                                        <HeaderStyle CssClass="header" />
                                        <Columns>
                                            <asp:BoundField DataField="gauge_id" HeaderText="Gauge Id" />
                                            <asp:BoundField DataField="gauge_name" HeaderText="Gauge Name" />
                                            <asp:BoundField DataField="gauge_sr_no" HeaderText="Gauge Sr.No." />
                                            <asp:BoundField DataField="gauge_Manufature_Id" HeaderText="Manufacture Id" />
                                            <asp:BoundField DataField="gauge_type" HeaderText="Gauge Type" />
                                            <asp:BoundField DataField="subgaugetype" HeaderText="Gauge Sub Type" />
                                            <asp:BoundField DataField="make" HeaderText="Make" />
                                            <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                            <asp:BoundField DataField="employee_name" HeaderText="Created By" />

                                            <asp:TemplateField HeaderText="Action" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:UpdatePanel ID="updaction" runat="server" UpdateMode="Always">
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="btnEditGauge" />
                                                            <asp:PostBackTrigger ControlID="LnkDownLoadDocument" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:LinkButton ID="btnEditGauge" runat="server" CommandName="Upd" OnClick="btnEditGauge_Click"
                                                                CommandArgument='<%# Eval("gauge_id")%>' class="btn btn-sm btn-info"
                                                                title="Update"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="LnkDownLoadDocument" Visible="false" runat="server" OnClick="LnkDownLoadDocument_Click"
                                                                CommandArgument='<%# Eval("gauge_id")%>' class="btn btn-sm btn-info"
                                                                title="Download Drawing Document"><i class="glyphicon glyphicon-download"></i></asp:LinkButton>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="confirmationModal" class="modal fade" style="display: none;">
                <div role="document" class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4>Confirmation</h4>
                        </div>
                        <div>
                            <label id="lblMsg" style="display: none; color: red; font-family: 'Courier New'; width: 100%; text-align: center;"></label>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 panel-body">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <h5>Gauge name is already exist! Do you want create duplicate gauge name?</h5>
                                        </div>
                                    </div>
                                    <div class="row section-gap">
                                        <div class="col-md-4 col-md-offset-4">
                                            <button aria-hidden="true" data-dismiss="modal" class="btn btn-default">No</button>
                                            <a class="btn btn-success">Yes</a>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:View>

        <asp:View ID="View2" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <br />
                    <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                        <div class="col-md-6">
                            <div class="form-horizontal">
                                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                    <p class="text-danger">
                                        <asp:Literal runat="server" ID="FailureText" />
                                    </p>
                                </asp:PlaceHolder>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Id</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" ID="txtGaugeId" ReadOnly="true" CssClass="form-control" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtGaugeId"
                                            CssClass="text-danger" ErrorMessage="Gauge Id field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Type<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="ddlType" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlType" TabIndex="3" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control">
                                                    <asp:ListItem Value="0">Select Type</asp:ListItem>
                                                    <asp:ListItem Value="1">ATTRIBUTE</asp:ListItem>
                                                    <asp:ListItem Value="2">VARIABLE</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" ControlToValidate="ddlType" CssClass="text-danger" ErrorMessage="Type field is required." />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Name<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="1" ID="txtGaugeName" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtGaugeName"
                                            CssClass="text-danger" ErrorMessage="Gauge Name field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Sr.No.</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="2" ID="txtGaugeSrNo" MaxLength="50" CssClass="form-control" />
                                        <asp:RequiredFieldValidator runat="server" SetFocusOnError="true" ControlToValidate="txtGaugeSrNo"
                                            CssClass="text-danger" ValidationGroup="g" ErrorMessage="Gauge Sr. No. field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Manufacture Id</asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="2" ID="txtManufactureId" CssClass="form-control" />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group" runat="server" id="divGoWereLimit">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Go Were Limit<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="4" ID="txtGoWereLimit" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" runat="server" ControlToValidate="txtGoWereLimit"
                                            CssClass="text-danger" ErrorMessage="Go Were Limit field is required." SetFocusOnError="true" />
                                        <asp:FilteredTextBoxExtender ID="txtGoWereLimit_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="txtGoWereLimit"
                                            ValidChars="0123456789."></asp:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Make<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="12" ID="txtmake" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" runat="server" SetFocusOnError="true" ControlToValidate="txtmake"
                                            CssClass="text-danger" ErrorMessage="Make field is required." />
                                    </div>
                                </div>

                                <div class="form-group" runat="server" id="divLeastCount">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Least Count<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="4" ID="txtLeastCount" CssClass="form-control" MaxLength="6" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" runat="server" SetFocusOnError="true" ControlToValidate="txtLeastCount"
                                            CssClass="text-danger" ErrorMessage="Least Count field is required." />
                                        <asp:FilteredTextBoxExtender ID="txtLeastCount_FilteredTextBoxExtender1"
                                            runat="server" Enabled="True" TargetControlID="txtLeastCount"
                                            ValidChars="0123456789."></asp:FilteredTextBoxExtender>

                                    </div>
                                </div>


                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-horizontal">
                                <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                                    <p class="text-danger">
                                        <asp:Literal runat="server" ID="Literal1" />
                                    </p>
                                </asp:PlaceHolder>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Gauge Type<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:DropDownList ID="ddlgaugeType" runat="server" class="selectpicker dropdownCustom" data-live-search-style="begins"
                                            data-live-search="true" TabIndex="2">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="ddlgaugeType" CssClass="text-danger" Display="Dynamic"
                                            ErrorMessage="Select Gauge Type" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="g" ValueToCompare="--Select--"></asp:CompareValidator>
                                        <asp:Label ID="lblTypeMasterId" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>

                                <div runat="server" id="divGoandNoGoPlus">

                                    <div class="form-group" runat="server" id="divSize">

                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Lower Size<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" TabIndex="5" ID="txtSize" CssClass="form-control" onkeypress="return isNumberKey(this, event);" />
                                                <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtSize"
                                                    CssClass="text-danger" ErrorMessage="Lower Size field is required." />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Higher Size<b style="color: Red">*</b></asp:Label>
                                            <div class="col-md-8">
                                                <asp:TextBox runat="server" TabIndex="5" ID="txtSizeMax" CssClass="form-control" onkeypress="return isNumberKey(this, event);" />
                                                <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtSizeMax"
                                                    CssClass="text-danger" ErrorMessage="Heigher Size field is required." />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Tolerance Type</asp:Label>
                                            <div class="col-md-8">
                                                <asp:DropDownList ID="ddlTolerance" runat="server" TabIndex="1" CssClass="form-control">
                                                    <asp:ListItem Value="--Select--" Text="--Select--" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Value="Plug Tolerance" Text="Plug Tolerance"></asp:ListItem>
                                                    <asp:ListItem Value="Snap Tolerance" Text="Snap Tolerance"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" CssClass="col-md-4 control-label">Tolerance Range</asp:Label>
                                            <div class="col-md-8">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnShowPopup" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Button runat="server" ID="btnShowPopup" OnClick="btnShowPopup_Click" CssClass="col-md-4 btn btn-primary btn-sm" Width="100px" Text="View And Select"></asp:Button>
                                                         <asp:TextBox runat="server" TabIndex="6" ID="txtToleranceRange" ReadOnly="true" CssClass="col-md-4 form-control" Width="170px" style="margin-left:5px" />
                                                      <%--  <asp:RequiredFieldValidator ValidationGroup="g" runat="server" SetFocusOnError="true" ControlToValidate="txtToleranceRange"
                                                            CssClass="text-danger" ErrorMessage="Select Tolerance Range" />--%>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>

                                        </div>


                                    </div>


                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Go Tollerance (+)<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" TabIndex="6" ID="txtGoTollerancePlus" CssClass="form-control" MaxLength="6" />
                                            <asp:RequiredFieldValidator ValidationGroup="g" runat="server" SetFocusOnError="true" ControlToValidate="txtGoTollerancePlus"
                                                CssClass="text-danger" ErrorMessage="Go Tollerance (+) field is required." />
                                            <asp:FilteredTextBoxExtender ID="txtGoTollerancePlus_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="txtGoTollerancePlus"
                                                ValidChars="0123456789."></asp:FilteredTextBoxExtender>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">Go Tollerance(-)<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" TabIndex="7" ID="txtGoTolleranceMinus" CssClass="form-control" MaxLength="6" />
                                            <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtGoTolleranceMinus"
                                                CssClass="text-danger" ErrorMessage="Go Tollerance(-) field is required." />
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                                runat="server" Enabled="True" TargetControlID="txtGoTolleranceMinus"
                                                ValidChars="0123456789.-"></asp:FilteredTextBoxExtender>

                                        </div>
                                    </div>
                                </div>
                                <div runat="server" id="divGoandNoGoTolleranceminus">

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">No Go Tollerance (+)<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" ID="txtNoGoTollerancePlus" TabIndex="8" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtNoGoTollerancePlus"
                                                CssClass="text-danger" ErrorMessage="No Go Tollerance (+) field is required." />
                                            <asp:FilteredTextBoxExtender ID="txtNoGoTollerancePlus_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="txtNoGoTollerancePlus"
                                                ValidChars="0123456789."></asp:FilteredTextBoxExtender>


                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-4 control-label">No Go Tollerance(-)<b style="color: Red">*</b></asp:Label>
                                        <div class="col-md-8">
                                            <asp:TextBox runat="server" TabIndex="9" ID="txtNoGoTolleranceMinus" CssClass="form-control" />
                                            <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtNoGoTolleranceMinus"
                                                CssClass="text-danger" ErrorMessage="No Go Tollerance(-) field is required." />
                                            <asp:FilteredTextBoxExtender ID="txtNoGoTolleranceMinus_FilteredTextBoxExtender"
                                                runat="server" Enabled="True" TargetControlID="txtNoGoTolleranceMinus"
                                                ValidChars="0123456789.-"></asp:FilteredTextBoxExtender>

                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" runat="server" id="divResolution">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Resolution<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="5" ID="txtResolution" CssClass="form-control" MaxLength="6" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" runat="server" ControlToValidate="txtResolution"
                                            CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Resolution field is required." />
                                        <asp:FilteredTextBoxExtender ID="txtRe3js3W69CrMinNGtLdWyYrnnKzHR26vu4"
                                            runat="server" Enabled="True" TargetControlID="txtResolution"
                                            ValidChars="0123456789."></asp:FilteredTextBoxExtender>

                                    </div>
                                </div>
                                <div class="form-group" runat="server" id="divPermisable1">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Permissable Error1<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="6" ID="txtPermisableError1" CssClass="form-control" MaxLength="6" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPermisableError1"
                                            ValidationGroup="g" CssClass="text-danger" ErrorMessage="Permissable Error1 field is required." />
                                        <asp:FilteredTextBoxExtender ID="txtPermisableError1_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="txtPermisableError1"
                                            ValidChars="0123456789.-"></asp:FilteredTextBoxExtender>

                                    </div>
                                </div>
                                <div class="form-group" runat="server" id="divPermisable2">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Permissable Error2<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="7" ID="txtPermisableError2" CssClass="form-control" MaxLength="6" />
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPermisableError2"
                                            CssClass="text-danger" ValidationGroup="g" ErrorMessage="Permissable Error2 field is required." />
                                        <asp:FilteredTextBoxExtender ID="txtPermisableError2_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="txtPermisableError2"
                                            ValidChars="0123456789.-"></asp:FilteredTextBoxExtender>


                                    </div>
                                </div>


                                <asp:Label ID="lblGaugeId" runat="server" Visible="false"></asp:Label>
                                <%-- <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Current Location<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="13" ID="txtCurrentLocation" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtCurrentLocation"
                                            CssClass="text-danger" ErrorMessage="Current Location field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Purchase Cost<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="14" ID="txtPurchaseCost" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtPurchaseCost"
                                            CssClass="text-danger" ErrorMessage="Purchase Cost field is required." />
                                        <asp:FilteredTextBoxExtender ID="txtPurchaseCost_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="txtPurchaseCost"
                                            ValidChars="0123456789."></asp:FilteredTextBoxExtender>

                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Purchase Date<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="15" placeHolder="DD/MM/YYYY" ID="txtPurchaseDate" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtPurchaseDate"
                                            CssClass="text-danger" ErrorMessage="Purchase Date field is required." />
                                    </div>
                                </div>--%>

                                <div class="form-group" runat="server" id="divRange" visible="false">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Range<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="16" ID="txtRange" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtRange"
                                            CssClass="text-danger" ErrorMessage="Range field is required." />
                                        <asp:FilteredTextBoxExtender ID="txtRange_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" TargetControlID="txtRange"
                                            ValidChars="0123456789.-MMmm "></asp:FilteredTextBoxExtender>

                                    </div>
                                </div>
                                <%-- <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Service Date<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="16" placeHolder="DD/MM/YYYY" ID="txtServiceDate" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtServiceDate"
                                            CssClass="text-danger" ErrorMessage="Service Date field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Retairment Date<b style="color: Red">*</b></asp:Label>
                                    <div class="col-md-8">
                                        <asp:TextBox runat="server" TabIndex="17" placeHolder="DD/MM/YYYY" ID="txtRetairementDate" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ValidationGroup="g" SetFocusOnError="true" runat="server" ControlToValidate="txtRetairementDate"
                                            CssClass="text-danger" ErrorMessage="Retairment Date field is required." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:Label runat="server" CssClass="col-md-4 control-label">Upload Drawing File</asp:Label>
                                    <div class="col-md-8">
                                        <asp:FileUpload ID="UploadDrwainfFile" runat="server" TabIndex="15" CssClass="form-control" />
                                        <asp:Label ID="txtImageName" runat="server"></asp:Label>
                                        <br />
                                        <asp:Label ID="Label1" Text="jpeg, png, gif, bmp, jpg and pdf only" ForeColor="#cc3300" runat="server"></asp:Label>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                        <div class="col-md-offset-5 col-md-6" style="margin-bottom: 10px">
                            <asp:UpdatePanel ID="updd" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSaveGauge" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button runat="server" TabIndex="20" ID="btnSaveGauge" ValidationGroup="g" OnClick="btnSaveGauge_Click" Text="Save" CssClass="btn btn-primary" />
                                    &nbsp;
                                            <asp:Button runat="server" TabIndex="21" ID="btnClloseGauge" CausesValidation="false" Text="Close" OnClick="btnClloseGauge_Click" CssClass="btn btn-primary" />
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </asp:Panel>

                </div>
            </div>
        </asp:View>
    </asp:MultiView>




    <table id="tblleaddetails" runat="server" style="display: none; margin-top: 20px" height="650px"
        width="1050px">
        <tr>
            <td>
                <asp:Panel ID="Panel3" runat="server" CssClass="panel_popup" Height="650px"
                    Width="1050px">
                    <table style="width: 100%">
                        <tr style="background-color: #FFFFFF">
                            <td style="color: #800000; font-size: 18px; font-weight: bold; background-color: #FFFFFF;"
                                align="center">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tolerance Details
                            </td>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ImageButton4" />
                                </Triggers>
                                <ContentTemplate>
                                    <td align="right" style="float: right; background-color: #FFFFFF;">
                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/Close.jpg"
                                            Width="23px" OnClick="ImageButton4_Click" AutoPostBack="True" />
                                    </td>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </tr>
                    </table>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-md-12">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <asp:Panel ID="Panel2" runat="server" Width="100%" Height="600px" ScrollBars="Auto">
                                                <asp:GridView ID="grdPlugTolerance" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive"
                                                    HeaderStyle-Wrap="false">
                                                    <HeaderStyle CssClass="header" />
                                                    <Columns>
                                                        <asp:BoundField DataField="range_type" HeaderText="Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                                                       <asp:BoundField DataField="SYM" HeaderText="SYM" HeaderStyle-Width="30px" ItemStyle-Font-Bold="true" />
                                                        <asp:TemplateField HeaderText="GRD.6" HeaderStyle-Width="50px" ItemStyle-Font-Size="Medium" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                               <asp:RadioButton runat="server" CssClass="radio" Font-Bold="true" ID="grd6" GroupName="rdBtnGrp" Text='<%# Eval("GRD6")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="GRD.7" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" Font-Bold="true" ID="grd7" GroupName="rdBtnGrp" Text='<%# Eval("GRD7")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.8" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd8" GroupName="rdBtnGrp" Text='<%# Eval("GRD8")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.9" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd9" GroupName="rdBtnGrp" Text='<%# Eval("GRD9")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="GRD.10" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd10" GroupName="rdBtnGrp" Text='<%# Eval("GRD10")%>' onchange="onchangeradio(this);" />
                                                               </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="GRD.11" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd11" GroupName="rdBtnGrp" Text='<%# Eval("GRD11")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.12" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd12" GroupName="rdBtnGrp" Text='<%# Eval("GRD12")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.13" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd13" GroupName="rdBtnGrp" Text='<%# Eval("GRD13")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.14" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd14" GroupName="rdBtnGrp" Text='<%# Eval("GRD14")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.15" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd15" GroupName="rdBtnGrp" Text='<%# Eval("GRD15")%>' onchange="onchangeradio(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.16" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd16" GroupName="rdBtnGrp" Text='<%# Eval("GRD16")%>' onchange="onchangeradio(this);" />
                                                                <asp:HiddenField ID="hfId" Value='<%# Eval("id")%>' runat="server"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfToleranceId" Value='<%# Eval("tolerance_id")%>' runat="server"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfRangeVal" Value='<%# Eval("range_type")%>' runat="server"></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                                </asp:GridView>

                                                <asp:GridView ID="grdSnapTolerance" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="table table-bordered table-responsive"
                                                    HeaderStyle-Wrap="false">
                                                    <HeaderStyle CssClass="header" />
                                                    <Columns>
                                                        <asp:BoundField DataField="range_type" HeaderText="Range" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" />
                                                       <asp:BoundField DataField="SYM" HeaderText="SYM" HeaderStyle-Width="30px" ItemStyle-Font-Bold="true" />
                                                         <asp:TemplateField HeaderText="GRD.5" HeaderStyle-Width="50px" ItemStyle-Font-Size="Medium" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                               <asp:RadioButton runat="server" CssClass="radio" Font-Bold="true" ID="grd5" GroupName="rdBtnGrp" Text='<%# Eval("GRD5")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                          <asp:TemplateField HeaderText="GRD.6" HeaderStyle-Width="50px" ItemStyle-Font-Size="Medium" ItemStyle-Font-Bold="true">
                                                            <ItemTemplate>
                                                               <asp:RadioButton runat="server" CssClass="radio" Font-Bold="true" ID="grd6" GroupName="rdBtnGrp" Text='<%# Eval("GRD6")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="GRD.7" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" Font-Bold="true" ID="grd7" GroupName="rdBtnGrp" Text='<%# Eval("GRD7")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.8" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd8" GroupName="rdBtnGrp" Text='<%# Eval("GRD8")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.9" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd9" GroupName="rdBtnGrp" Text='<%# Eval("GRD9")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="GRD.10" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd10" GroupName="rdBtnGrp" Text='<%# Eval("GRD10")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                               </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="GRD.11" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd11" GroupName="rdBtnGrp" Text='<%# Eval("GRD11")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.12" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd12" GroupName="rdBtnGrp" Text='<%# Eval("GRD12")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.13" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd13" GroupName="rdBtnGrp" Text='<%# Eval("GRD13")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.14" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd14" GroupName="rdBtnGrp" Text='<%# Eval("GRD14")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.15" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd15" GroupName="rdBtnGrp" Text='<%# Eval("GRD15")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="GRD.16" HeaderStyle-Width="50px">
                                                            <ItemTemplate>
                                                                <asp:RadioButton runat="server" CssClass="radio" ID="grd16" GroupName="rdBtnGrp" Text='<%# Eval("GRD16")%>' onchange="onchangeradioOnSnapTolerance(this);" />
                                                                <asp:HiddenField ID="hfId" Value='<%# Eval("id")%>' runat="server"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfToleranceId" Value='<%# Eval("tolerance_id")%>' runat="server"></asp:HiddenField>
                                                                <asp:HiddenField ID="hfRangeVal" Value='<%# Eval("range_type")%>' runat="server"></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfGradValue" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hftoleranceId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfId" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfRange" runat="server"></asp:HiddenField>
    <asp:HiddenField ID="hfGrade" runat="server"></asp:HiddenField>
    <asp:Label ID="Labelpanb" runat="server" Text=""></asp:Label>
    <asp:ModalPopupExtender ID="ModalPopupExtenderTolerance" runat="server" TargetControlID="Labelpanb"
        PopupControlID="tblleaddetails">
    </asp:ModalPopupExtender>

</asp:Content>

