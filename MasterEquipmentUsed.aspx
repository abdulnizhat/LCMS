<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MasterEquipmentUsed.aspx.cs" Inherits="MasterEquipmentUsed" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#<%= txtCalibDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtValidDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <h3><%: Title %>Master Equipment Used</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:UpdatePanel ID="updaction" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAddEqp" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="btnAddEqp" Text="Add New" OnClick="btnAddEqp_Click" CssClass="btn btn-primary" Visible="false" />
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
                        <div class="col-md-10">
                            <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdEqp" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="10" OnPageIndexChanging="grdEqp_PageIndexChanging">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="description" HeaderText="Description" />
                                        <asp:BoundField DataField="master_sr" HeaderText="Master Sr.No" />
                                        <asp:BoundField DataField="calibration_date" HeaderText="Calibration Date" />
                                        <asp:BoundField DataField="valid_till_date" HeaderText="Valid Till Date " />
                                       <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="updpaEdit" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnEditEqp" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnEditEqp" runat="server" OnClick="btnEditEqp_Click"
                                                            CommandArgument='<%# Eval("id")%>' class="btn btn-sm btn-info" title="Update" Enabled="false"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>
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
            <div class="col-md-offset-2 col-md-12">
                <div class="form-horizontal">
                    <%-- <h4>Branch Master</h4>--%>
                    <br />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                       <asp:Label runat="server" CssClass="col-md-2 control-label">Description<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtDesc" Style="height: 50px" CssClass="form-control" MaxLength="500" TextMode="MultiLine" TabIndex="2" />
                             <asp:Label ID="lblMEqpId" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                     <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Master Sr.No.<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtMasterSrNo" CssClass="form-control" TabIndex="2" MaxLength="100" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMasterSrNo" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Master Sr.No. field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Calibration Date<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtCalibDate" placeHolder="DD/MM/YYYY" CssClass="form-control" TabIndex="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCalibDate" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Calibration Date field is required." ValidationGroup="a" />
                        </div>
                    </div>
                   <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Valid Till Date<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtValidDate" placeHolder="DD/MM/YYYY" CssClass="form-control" TabIndex="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtValidDate" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Valid Till Date field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    
                    <div class="col-md-offset-2 col-md-6" style="margin-bottom:50px">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveEqp" OnClick="btnSaveEqp_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="3" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnCloseEqp" Text="Close" OnClick="btnCloseEqp_Click" CssClass="btn btn-primary" TabIndex="4" />
                        </div>
                    </div>
                </div>
            </div>

        </asp:View>
    </asp:MultiView>
</asp:Content>

