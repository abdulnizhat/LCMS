<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AllReports.aspx.cs" Inherits="AllReports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function Confirm() {
            var chk = '<%= Session["isWithHeader"].ToString() %>';
        var form = document.forms[0];
        // Remove the previous element added
        var oldInput = document.getElementById('myInput');
        if (oldInput !== null) form.removeChild(oldInput);
        // Add a new element
        var confirm_value = document.createElement("INPUT");
        confirm_value.setAttribute('id', 'myInput');
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";

        if (confirm("Is this report download with header ?")) {
            confirm_value.value = "Yes";
        } else {
            confirm_value.value = "No";
        }
        form.appendChild(confirm_value);
    }
    </script>
    <h3><%: Title %>ALL Reports Of Certificate</h3>
    <div class="col-md-12">
        <div class="form-horizontal">
            <div class="form-group">
                <div class="col-md-12">
                    <asp:Panel ID="pnlgag" runat="server" Width="100%" ScrollBars="Auto">
                        <asp:GridView ID="grdData" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                            PageSize="50" OnPageIndexChanging="grdData_PageIndexChanging" HeaderStyle-Wrap="false">
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
                                <asp:TemplateField HeaderText="Action" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <asp:UpdatePanel ID="updpaEdit" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnPrintBlankData" />
                                                <asp:PostBackTrigger ControlID="btnPrintWithData" />
                                                <asp:PostBackTrigger ControlID="btnPrintFullReport" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:LinkButton ID="btnPrintBlankData" runat="server" OnClick="btnPrintBlankData_Click"
                                                    CommandArgument='<%# Eval("certificatedataId") + "," + Eval("certificateId")  + "," + Eval("type_of_gauge")%>' class="btn btn-sm btn-info" ToolTip="Print Blank Data">
                                                    <i class="glyphicon glyphicon-print"></i></asp:LinkButton>
                                                &nbsp;
                                                        <asp:LinkButton ID="btnPrintWithData" runat="server" OnClick="btnPrintWithData_Click"
                                                            CommandArgument='<%# Eval("certificatedataId") + "," + Eval("certificateId")  + "," + Eval("type_of_gauge")%>' class="btn btn-sm btn-info"
                                                            ToolTip="Print With Data"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
                                                &nbsp;
                                                        <asp:LinkButton ID="btnPrintFullReport" runat="server" OnClientClick="Confirm();" OnClick="btnPrintFullReport_Click"
                                                            CommandArgument='<%# Eval("certificatedataId") + "," + Eval("certificateId")  + "," + Eval("type_of_gauge")%>' class="btn btn-sm btn-info"
                                                            ToolTip="Print Report"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
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

</asp:Content>

