<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IS_Ref_Guideline.aspx.cs" Inherits="IS_Ref_Guideline" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link rel="stylesheet" href="css/ui.theme.css" type="text/css" media="all" />
    <script src="Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#<%= txtReviseDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtReleaseDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <h3 style="margin-bottom:0px !important"><%: Title %>IS Ref. (Guideline) Master</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="col-md-12">
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-10">
                            <asp:UpdatePanel ID="updaction" runat="server">
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnAddISRef" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="btnAddISRef" Text="Add New" OnClick="btnAddISRef_Click" CssClass="btn btn-primary" Visible="false" />
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
                                <asp:GridView ID="grdISRef" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="10" OnPageIndexChanging="grdISRef_PageIndexChanging">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="id" HeaderText="IS Ref. Id" />
                                        <asp:BoundField DataField="is_number" HeaderText="I.S Number" />
                                        <asp:BoundField DataField="description" HeaderText="Description" />
                                        <asp:BoundField DataField="revesion" HeaderText="Revision" />
                                        <asp:BoundField DataField="rev_date" HeaderText="Revise Date" />
                                        <asp:BoundField DataField="release_number" HeaderText="Release Number" />
                                        <asp:BoundField DataField="release_date" HeaderText="Release Date" />
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="updpaEdit" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnEditISRef" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnEditISRef" runat="server" OnClick="btnEditISRef_Click"
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
                    <div class="form-group" style="margin-top:0px !important">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">I.S Number<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtISRef" MaxLength="45" CssClass="form-control" TabIndex="1" />
                            <asp:Label ID="lblISRefId" runat="server" Visible="false"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtISRef" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="I.S Number field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Revise<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtRevise" CssClass="form-control" TabIndex="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRevise" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Revise field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Revise Date<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtReviseDate" placeHolder="DD/MM/YYYY" CssClass="form-control" TabIndex="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReviseDate" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Revise Date field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Release Number<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtReleaseNo" CssClass="form-control" TabIndex="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReleaseNo" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Release Number field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Release Date<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtReleaseDate" placeHolder="DD/MM/YYYY" CssClass="form-control" TabIndex="2" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtReleaseDate" SetFocusOnError="true"
                                CssClass="text-danger" ErrorMessage="Release Date field is required." ValidationGroup="a" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="col-md-2 control-label">Description<b style="color: Red">*</b></asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="txtDesc" Style="height: 50px" CssClass="form-control" MaxLength="500" TextMode="MultiLine" TabIndex="2" />
                        </div>
                    </div>
                    <div class="col-md-offset-2 col-md-6" style="margin-bottom:50px">
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="btnSaveISRef" OnClick="btnSaveISRef_Click" Text="Save" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="3" />
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" ID="btnCloseISRef" Text="Close" OnClick="btnCloseISRef_Click" CssClass="btn btn-primary" TabIndex="4" />
                        </div>
                    </div>
                </div>
            </div>

        </asp:View>
    </asp:MultiView>
</asp:Content>
