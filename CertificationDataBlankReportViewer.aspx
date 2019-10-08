<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CertificationDataBlankReportViewer.aspx.cs" Inherits="CertificationDataBlankReportViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div style="margin-left:250px">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" Height="650px" Width="750px" ShowRefreshButton="False">
            <LocalReport ReportPath="CertificateDataBlankReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet2" />
                     <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet3" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        </div>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" TypeName="CalibrationHistoryReportDataSetTableAdapters."></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
