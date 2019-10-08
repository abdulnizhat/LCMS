<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DataSheet.aspx.cs" Inherits="DataSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <link type="text/css" href="js/searchabledropdown/3.3.7-css-bootstrap.min.css" rel="stylesheet" />
    <link type="text/css" href="js/searchabledropdown/1.12.2-css-select.min.css" rel="stylesheet" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <style>
        .dropdownCustom {
            width: 280px !important;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#<%= txtRefrenceDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtDateOfReceipt.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtDateOfCalib.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        function onlyNos(e, t) {
            try {
                if (window.event) {
                    var charCode = window.event.keyCode;
                }
                else if (e) {
                    var charCode = e.which;
                }
                else { return true; }
                if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                    return false;
                }
                return true;
            }
            catch (err) {
                alert(err.Description);
            }
        }

    </script>
    <h3><%: Title %>Certificate Data Sheet</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:UpdatePanel ID="updaction" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAddCerticateData" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="btnAddCerticateData" Text="Add New" OnClick="btnAddCerticateData_Click" CssClass="btn btn-primary" Visible="false" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-10" style="text-align: right" runat="server" visible="false">
                            <b>
                                <asp:Label ID="lbl1" runat="server" Text="No. of Count :">
                                </asp:Label>
                                <asp:Label ID="lblcnt" runat="server">
                                </asp:Label></b>

                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdCertificateData" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="50" OnPageIndexChanging="grdCertificateData_PageIndexChanging" HeaderStyle-Wrap="false">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="gauge_name" HeaderText="Gauge" />
                                        <asp:BoundField DataField="certificate_no" HeaderText="Certificate No" />
                                        <asp:BoundField DataField="type_of_gauge" HeaderText="Gauge Type" />
                                        <asp:BoundField DataField="size_range" HeaderText="Size Range" />
                                        <asp:BoundField DataField="satifactory" HeaderText="Condition Of Receipt" />
                                        <asp:BoundField DataField="identification_mark_by" HeaderText="Identification Mark By" />
                                        <asp:BoundField DataField="CertificationDone" HeaderText="Certification Done" />
                                        <asp:BoundField DataField="customer_name" HeaderText="Customer" />
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="updpaEdit" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnEditCerData" />
                                                        <asp:PostBackTrigger ControlID="btnPrint" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnEditCerData" runat="server" OnClick="btnEditCerData_Click"
                                                            CommandArgument='<%# Eval("id")%>' class="btn btn-sm btn-info" title="Update" Enabled="false"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
                                                        &nbsp;
                                                        <asp:LinkButton ID="btnPrint" runat="server" OnClick="btnPrint_Click"
                                                            CommandArgument='<%# Eval("id")%>' class="btn btn-sm btn-info"
                                                            ToolTip="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
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
        </asp:View>
        <asp:View ID="View2" runat="server">
            <div class="row">
                <div class="col-md-12">
                    <br />

                    <div class="col-md-6">

                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="FailureText" />
                                </p>
                            </asp:PlaceHolder>
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddlCustomer" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Customer<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:DropDownList ID="ddlCustomer" runat="server" class="selectpicker dropdownCustom" data-live-search-style="begins"
                                                data-live-search="true" TabIndex="1" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToValidate="ddlCustomer" CssClass="text-danger" Display="Dynamic"
                                                ErrorMessage="Select Customer" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="a" ValueToCompare="--Select--"></asp:CompareValidator>
                                            <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder1" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal1" />
                                </p>
                            </asp:PlaceHolder>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="ddlgauge" />
                                    <asp:PostBackTrigger ControlID="ddlCustomer" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Gauge<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:DropDownList ID="ddlgauge" runat="server" class="selectpicker dropdownCustom" data-live-search-style=""
                                                data-live-search="true" TabIndex="1" AutoPostBack="true" OnSelectedIndexChanged="ddlgauge_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToValidate="ddlgauge" CssClass="text-danger" Display="Dynamic"
                                                ErrorMessage="Select Gauge" SetFocusOnError="true" Operator="NotEqual" ValidationGroup="a" ValueToCompare="--Select--"></asp:CompareValidator>
                                            <asp:Label ID="lblCertificateDataId" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                </div>
                <div class="col-md-12" runat="server" id="divHiddenFields">
                    <div class="col-md-6">

                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder2" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal2" />
                                </p>
                            </asp:PlaceHolder>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Type Of Gauge</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtTypeOfgauge" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtTypeOfgauge"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Type Of Gauge field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Identification</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtIdentification" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Mfg.No</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtMfgNo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>

                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Make</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtMake" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Size</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtSize" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtSize"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Size field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Condition Of Gauge at Receipt</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtConditionOfReceipt" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtConditionOfReceipt"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Condition Of Gauge at Receipt field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Test Purpose</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtTestPurpose" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtTestPurpose"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Test Purpose Receipt field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">IS Ref.(Guiedline)</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtIsrefGuideLine" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtIsrefGuideLine"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="IS Ref.(Guiedline) field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Calibration Method Number</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtCalibrationMethodNumber" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtCalibrationMethodNumber"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Calibration Method Number field is required." />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Calibration Carried Out at<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtCalibCarriedOutat" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtCalibCarriedOutat"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Calibration Carried Out at is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Uncertinity Of Measurment</asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtuncertintiy" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtuncertintiy"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Uncertinity Of Measurment is required." />
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-horizontal">
                            <asp:PlaceHolder runat="server" ID="PlaceHolder3" Visible="false">
                                <p class="text-danger">
                                    <asp:Literal runat="server" ID="Literal3" />
                                </p>
                            </asp:PlaceHolder>

                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Refrence/Dc.No</asp:Label>
                                        <div class="col-md-9">

                                            <asp:TextBox ID="txtRefrenceDcno" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Date Of Refrence<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtRefrenceDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtRefrenceDate"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Date Of Refrence field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Date Of Receipt<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtDateOfReceipt" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtDateOfReceipt"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Date Of Receipt field is required." />
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="ddlFrequencyType" />
                                            <asp:PostBackTrigger ControlID="txtCalibrationFrequency" />
                                            <asp:PostBackTrigger ControlID="txtDateOfCalib" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Frequency Type<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:DropDownList ID="ddlFrequencyType" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ddlFrequencyType_SelectedIndexChanged" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                                        <asp:ListItem Value="1">DAYS</asp:ListItem>
                                                        <asp:ListItem Value="2" Selected="True">MONTH</asp:ListItem>
                                                        <asp:ListItem Value="3">YEAR</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator ID="CompareValidator2" SetFocusOnError="true" runat="server" ControlToValidate="ddlFrequencyType" Display="Dynamic" ForeColor="Maroon"
                                                        ErrorMessage="Select Frequency Type" Operator="NotEqual" ValidationGroup="a" ValueToCompare="0"></asp:CompareValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Calibration Frequency<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox runat="server" onkeypress="return onlyNos(event,this);" TabIndex="6" MaxLength="2" OnTextChanged="txtCalibrationFrequency_TextChanged" AutoPostBack="true" ID="txtCalibrationFrequency" CssClass="form-control" />
                                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCalibrationFrequency"
                                                        CssClass="text-danger" SetFocusOnError="true" ValidationGroup="c" Display="Dynamic" ErrorMessage="Calibration Frequency field is required." />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Date Of Calibration<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="txtDateOfCalib" runat="server" OnTextChanged="txtDateOfCalib_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtDateOfCalib"
                                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Date Of Calibration field is required." />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label runat="server" CssClass="col-md-3 control-label">Next Calibration Date<b style="color: Red"> *</b></asp:Label>
                                                <div class="col-md-9">
                                                    <asp:TextBox ID="txtNextCalibDate" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtNextCalibDate"
                                                        CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Next Calibration Date field is required." />
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Identification Marked By RML<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtdentificationMarkedByRML" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtdentificationMarkedByRML"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Identification Marked By RML field is required." />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Certificate No.<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:DropDownList ID="ddlCertificateNo" runat="server" CssClass="form-control" TabIndex="21">
                                            </asp:DropDownList>
                                            <br />
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="ddlCertificateNo" CssClass="text-danger" Display="Dynamic"
                                                ErrorMessage="Select Certificate No." SetFocusOnError="true" Operator="NotEqual" ValidationGroup="a" ValueToCompare="--Select--"></asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" CssClass="col-md-3 control-label">Master Equipment Used<b style="color: Red"> *</b></asp:Label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtMasterEquipmentUsed" runat="server" Height="120px" TextMode="MultiLine" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtMasterEquipmentUsed"
                                                CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Master Equipment Used field is required." />
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSaveCertificateData" />
                        <asp:PostBackTrigger ControlID="btnClose" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="col-md-offset-5 col-md-6" style="margin-bottom: 50px">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnSaveCertificateData" OnClick="btnSaveCertificateData_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="22" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button runat="server" ID="btnClose" Text="Close" OnClick="btnClose_Click" CssClass="btn btn-primary" TabIndex="23" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </asp:View>
    </asp:MultiView>
</asp:Content>

