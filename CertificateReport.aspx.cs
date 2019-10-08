using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CertificateReport : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["isWithHeader"] = "NO";
                MultiView1.ActiveViewIndex = 0;
                BindCertificateReport();


            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void BindCertificateReport()
    {
        try
        {
            string query = @"Select ctr.id as certificateId, ct.id as certificatedataId, ct.certificate_no, ct.gauge_id, gt.gauge_name, cus.customer_name, ct.customer_id,
tm.type_of_gauge,su.size_range, cgr.satifactory,ct.identification_mark_by, (case when ct.is_certification_done='False' then 'No' else 'Yes' End) as CertificationDone
from certification_tb as ctr
Left Outer join certificate_data_tb as ct
ON ctr.certification_data_id=ct.id
Left Outer join gaugemaster_tb as gt
 ON ct.gauge_id=gt.gauge_id
 Left Outer join typemaster_tb as tm
 ON gt.gauge_type_master_id=tm.id
 Left Outer join condition_gauge_receipt_tb as cgr
 ON tm.condition_of_receipt=cgr.satifactory_id
 Left Outer join test_purpose_tb as tp
 ON tm.test_purpose=tp.id
 Left Outer join calibration_methodno_tb as cm
 ON tm.calibration_method=cm.id
 Left Outer join subtypemaster_tb as su
 ON tm.sub_type_id=su.id
 Left Outer join is_referance_guideline_tb as rf
 ON tm.is_ref_guidline=rf.id
 Left Outer join customer_tb as cus
 ON ct.customer_id=cus.customer_id Where ct.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " order by ctr.id desc ";
            DataTable dtBindGrid = g.ReturnData(query);
            grdCertificate.DataSource = dtBindGrid;
            grdCertificate.DataBind();
            checkAuthority();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("CertificateReport.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCertificate.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCertificate.Rows[i].FindControl("btnPrintCertificateReport");
                        //LinkButton lnkEdit = (LinkButton)grdCalibResult.Rows[i].FindControl("btnEditCertificateResult");
                        //lnkEdit.Enabled = true;
                        lnk.Enabled = true;
                        Session["isWithHeader"] = "YES";
                        //Session["dleteGagePartLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdCertificate.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCertificate.Rows[i].FindControl("btnPrintCertificateReport");
                        lnk.Enabled = false;
                        //LinkButton lnkEdit = (LinkButton)grdCalibResult.Rows[i].FindControl("btnEditCertificateResult");
                        //lnkEdit.Enabled = false;
                        Session["isWithHeader"] = "NO";
                    }
                }

            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {

    }
    protected void btnUpdateCertificate_Click(object sender, EventArgs e)
    {

    }
    protected void btnPrintCertificateReport_Click(object sender, EventArgs e)
    {
        try
        {
            string confirmValue = Request.Form["confirm_value"];
            string[] arg = new string[2];
            LinkButton lnk = (LinkButton)sender;
            arg = lnk.CommandArgument.ToString().Split(',');
            string id = arg[0];
            string gaugeType = arg[1];
            string strQuery = @"Select id, certification_data_id from certification_tb  where id=" + Convert.ToInt32(id) + "";
            DataTable dtgetgaugeId = g.ReturnData(strQuery);
            string certDataId = "";
            string certId = "";
            string queryStringVal = "";
            
            //string reportType = "notBlank";
            if (dtgetgaugeId.Rows.Count > 0)
            {
                certDataId = dtgetgaugeId.Rows[0]["certification_data_id"].ToString();
                certId = dtgetgaugeId.Rows[0]["id"].ToString();
                queryStringVal = certDataId + "," + certId +","+ confirmValue;  // +"," + reportType;
            }
            else
            {
                certDataId = "";
                certId = "";
                queryStringVal = "";
            }
            if (String.IsNullOrEmpty(id))
            {
                return;
            }
            else
            {

                if (gaugeType == "Micrometer")
                {
                    CallMicrometerReportViewer(queryStringVal);
                }
                else if (gaugeType == "Vernier")
                {
                    CallVernierReportViewer(queryStringVal);
                }
                else if (gaugeType == "Plunger")
                {
                    CallPlungerReportViewer(queryStringVal);
                }
                else if (gaugeType == "Lever")
                {
                    CallLeverBoreReportViewer(queryStringVal);
                }
                else if (gaugeType == "Feeler")
                {
                    CallFeelerReportViewer(queryStringVal);
                }
                else if (gaugeType == "Pressure")
                {
                    CallPressureReportViewer(queryStringVal);
                }
                else if (gaugeType == "For Attribute")
                {
                    CallAttributeReportViewer(queryStringVal);
                }
            }
          
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void CallAttributeReportViewer(string queryStringVal)
    {
        try
        {
            Response.Redirect("AttributeReportViewer.aspx?queryStringVal=" + queryStringVal);
            //string ss = "window.open('AttributeReportViewer.aspx?queryStringVal=" + queryStringVal + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "", script, false);
        }
        catch (Exception ex)
        {
            Logger.Error("Call Attribute reportViewer" + ex.Message);
        }

    }
    private void CallLeverBoreReportViewer(string strQuery)
    {
        try
        {
            Response.Redirect("leverBoreReportViewer.aspx?queryStringVal=" + strQuery);
            //string ss = "window.open('leverBoreReportViewer.aspx?queryStringVal=" + strQuery + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);

        }
        catch (Exception ex)
        {
            Logger.Error("Call Plunger reportViewer" + ex.Message);
        }
    }
    private void CallPlungerReportViewer(string queryStringVal)
    {
        try
        {
            Response.Redirect("plungerReportViewer.aspx?queryStringVal=" + queryStringVal);
            //string ss = "window.open('plungerReportViewer.aspx?queryStringVal=" + queryStringVal + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);

        }
        catch (Exception ex)
        {
            Logger.Error("Call Plunger reportViewer" + ex.Message);
        }
    }
    private void CallVernierReportViewer(string queryStringVal)
    {
        try
        {
            Response.Redirect("vernierReportViewer.aspx?queryStringVal=" + queryStringVal);
            //string ss = "window.open('vernierReportViewer.aspx?queryStringVal=" + queryStringVal + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);

        }
        catch (Exception ex)
        {
            Logger.Error("Call Vernier reportViewer" + ex.Message);
        }
    }
    private void CallMicrometerReportViewer(string queryStringVal)
    {
        try
        {
            Response.Redirect("MicrometerReportViewer.aspx?queryStringVal=" + queryStringVal);
            //string ss = "window.open('MicrometerReportViewer.aspx?queryStringVal=" + queryStringVal + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);

        }
        catch (Exception ex)
        {
            Logger.Error("Call Micrometer reportViewer" + ex.Message);
        }
    }
    private void CallFeelerReportViewer(string queryStringVal)
    {
        try
        {
            Response.Redirect("FeelerReportViewer.aspx?queryStringVal=" + queryStringVal);
            //string ss = "window.open('FeelerReportViewer.aspx?queryStringVal=" + queryStringVal + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);

        }
        catch (Exception ex)
        {
            Logger.Error("Call Micrometer reportViewer" + ex.Message);
        }
    }
    private void CallPressureReportViewer(string queryStringVal)
    {
        try
        {
            Response.Redirect("pressurreeportViewer.aspx?queryStringVal=" + queryStringVal);
            //string ss = "window.open('pressurreeportViewer.aspx?queryStringVal=" + queryStringVal + "," + "Type=All','mywindow','width=1000,height=700,left=200,top=1,screenX=100,screenY=100,toolbar=no,location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=yes,resizable=no')";
            //string script = "<script language='javascript'>" + ss + "</script>";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUpWindow", script, false);

        }
        catch (Exception ex)
        {
            Logger.Error("Call Plunger reportViewer" + ex.Message);
        }
    }
    protected void grdCertificate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCertificate.PageIndex = e.NewPageIndex;
        BindCertificateReport();
    }
}