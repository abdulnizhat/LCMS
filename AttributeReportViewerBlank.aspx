<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AttributeReportViewerBlank.aspx.cs" Inherits="AttributeReportViewerBlank" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
     <form id="form1" runat="server">
    <div style="margin-left:250px">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" Height="650px" Width="750px" ShowRefreshButton="False">
            <LocalReport ReportPath="AttributeReportBlank.rdlc">
               <%-- <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet2" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet3" />
                </DataSources>--%>
            </LocalReport>
        </rsweb:ReportViewer>
        <%--<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="AttributeDataSetTableAdapters."></asp:ObjectDataSource>--%>
    </div>
    </form>
</body>
</html>
