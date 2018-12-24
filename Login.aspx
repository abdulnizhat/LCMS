<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html>
<head>


    <meta charset="utf-8">
    <title>LCMS Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <!-- Main CSS -->
    <link href="CustomDesignTemplates/css/style.css" rel="stylesheet" />
    <!-- Font Awesome -->
    <link href="~/CustomDesignTemplates/css/font-awesome.min.css" rel="stylesheet" />
    <link href="~/CustomDesignTemplates/css/uts_styles.css" rel="stylesheet" />
    <script>history.go(1)</script>
</head>
<body>
    <form id="frmlogin" runat="server">
        <center>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <div class="col-md-12">
                               <a href="http://www.aarushqualityservices.com/"> 
                                    <h1 style="color:white">LAB CERTIFICATE MANAGEMENT SYSTEM</h1>
                                </a>&nbsp;<a href="http://www.aarushqualityservices.com/"></a>
                                <br />
                               </div>
                        </div>
                        <div style="margin-top:80px" class="col-md-12">
                            <div class="form-group">
                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-6 form-horizontal">
                                        <div class="col-md-3">
                                           <asp:Label runat="server" CssClass="col-md-offset-3 col-md-2 control-label" style="color:white; font-size:large"> Customer<b style="color: Red; font-size:small">&nbsp;*</b></asp:Label>
                                         </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control dropdown" 
                                                TabIndex="1" placeholder="Select Customer"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-6 form-horizontal">
                                        <div class="col-md-3">
                                          <asp:Label runat="server" CssClass="col-md-offset-3 col-md-2 control-label" style="color:white; font-size:large">Username<b style="color: Red; font-size:small">&nbsp;*</b></asp:Label>
                                          </div>
                                        <div class="col-md-6">
                                            <asp:TextBox  ID="txtusername" MaxLength="20" TabIndex="2" runat="server" placeholder="Username" CssClass="form-control"
                                            ></asp:TextBox>

                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-6 form-horizontal">
                                        <div class="col-md-3">
                                           <asp:Label runat="server" CssClass="col-md-offset-3 col-md-2 control-label" style="color:white; font-size:large">Password<b style="color: Red; font-size:small">&nbsp;*</b></asp:Label>
                                          </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtpassword" MaxLength="20" TabIndex="3" runat="server" CssClass="form-control"
                                            TextMode="Password" placeholder="Password"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-offset-5 col-md-10">
                                    <div class="col-md-1">
                                        <asp:Button ID="btnlogin" runat="server" CssClass="btn btn-primary my-2 my-sm-0" style="font-size:large"  Text="Login" OnClick="btnlogin_Click1" TabIndex="4" />
                                    </div>
                                    <div class="col-md-2">
                                    <asp:Label ID="lblerror" runat="server" CssClass="control-label text-danger font-weight-bold bg-white" Text="Label"  Visible="false"></asp:Label>
                                  </div>
                                </div>
                                 
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </center>
    </form>
    <center>
        <footer class="footer">
            <div class="footer-bottom" style="margin-top:200px">
                <p class="text-center"><a href="https://google.com/">System By ABDUL RAHMAN</a>.</p>
            </div>
        </footer>
    </center>

</body>
</html>