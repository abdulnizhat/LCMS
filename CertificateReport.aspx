<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CertificateReport.aspx.cs" Inherits="CertificateReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        $(function () {
           <%-- $("#<%= txtRefrenceDate.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
             $("#<%= txtDateOfReceipt.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });
             $("#<%= txtDateOfCalib.ClientID  %>").datepicker({ dateFormat: 'dd/mm/yy' });--%>
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
         //function Confirm() {
         //    $('#confirmationModal').modal('show');
         //    return true;
         //};
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
    <h3><%: Title %>Certificate Report</h3>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div class="col-md-12">
                <div class="form-horizontal">
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
                                <asp:GridView ID="grdCertificate" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="50" OnPageIndexChanging="grdCertificate_PageIndexChanging" HeaderStyle-Wrap="false">
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
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="130px">
                                            <ItemTemplate>
                                                <asp:UpdatePanel ID="updpaEdit" runat="server">
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnPrintCertificateReport" />
                                                         <%--<asp:PostBackTrigger ControlID="btnEditCertificateResult" />--%>
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:LinkButton ID="btnPrintCertificateReport" runat="server" OnClientClick="Confirm();" OnClick="btnPrintCertificateReport_Click"
                                                        CommandArgument='<%# Eval("certificateId")  + "," + Eval("type_of_gauge")%>' class="btn btn-sm btn-info"
                                                        ToolTip="Print"><i class="glyphicon glyphicon-print"></i></asp:LinkButton>
                                                   <%-- &nbsp;
                                                   <asp:LinkButton ID="btnEditCertificateResult" Visible="false" runat="server" OnClick="btnEditCertificateResult_Click"
                                                       CommandArgument='<%# Eval("certificateId")%>' class="btn btn-sm btn-info"
                                                       title="Edit"><i class="glyphicon glyphicon-pencil"></i></asp:LinkButton>--%>
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
                                            <h5>Is this report download with header?</h5>
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
              <div class="col-md-8">
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
                                <asp:GridView ID="grdCalibResult" runat="server" Width="90%" AutoGenerateColumns="false" AllowPaging="true" CssClass="table table-bordered table-responsive"
                                    PageSize="100" HeaderStyle-Wrap="false">
                                    <HeaderStyle CssClass="header" />
                                    <Columns>
                                        <asp:BoundField DataField="nominal_size" HeaderText="Nominal Size in mm" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Larger" />
                                        <asp:TemplateField HeaderText="Observed in mm" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtObserved" Text='<%# Eval("obsereved")%>' onkeypress="return isNumberKey(this, event);" MaxLength="15" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtObserved"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Field is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error in mm" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtError" Text='<%# Eval("error")%>' onkeypress="return isNumberKey(this, event);" MaxLength="15" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ValidationGroup="a" Display="Dynamic" runat="server" ControlToValidate="txtError"
                                                    CssClass="text-danger" SetFocusOnError="true" ErrorMessage="Field is required." />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>No data exist.</EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </div>
                    </div>
                </div>



                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnUpdateCertificate" />
                        <asp:PostBackTrigger ControlID="btnClose" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="col-md-offset-5 col-md-6" style="margin-bottom: 50px">
                            <div class="col-md-2">
                                <asp:Button runat="server" ID="btnUpdateCertificate" OnClick="btnUpdateCertificate_Click" Text="Updtae" CssClass="btn btn-primary" ValidationGroup="a" TabIndex="22" />
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

