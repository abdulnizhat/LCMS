using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                displayDueStatus();
                displayMsaDueStatus();
                displayIssueReturnPending();
                displayGraph();
                sendMail();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void sendMail()
    {
        try
        {
            string senderMailId = null;
            string password = null;
            string sendTo = null;
            int port = 0;

            DateTime date = new DateTime();
            date = DateTime.Now;
            stDate = date.ToString("yyyy-MM-dd");
            DataTable dtcheckSendMailOrNotToday = g.ReturnData("Select customer_id from sendMailHistoryTB where customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and user_id = "+Convert.ToInt32(Session["User_ID"])+" and DATE_FORMAT(send_Date, '%m/%d/%Y')='"+ Convert.ToDateTime(date.ToString("MM/dd/yyyy"))+"'");
         
            if (dtcheckSendMailOrNotToday.Rows.Count > 0)
            {
                return;
            }
            else
            {
                DataTable dtresultgetMailId = g.ReturnData("Select email_id_from, credential, port, specified_emai_to_send from mail_Setting_TB where customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and status = True");
              
                if (dtresultgetMailId.Rows.Count>0)
                {
                   
                    senderMailId = dtresultgetMailId.Rows[0]["email_id_from"].ToString(); 
                    password = dtresultgetMailId.Rows[0]["credential"].ToString(); 
                        port = Convert.ToInt32( dtresultgetMailId.Rows[0]["port"].ToString());
                        sendTo =dtresultgetMailId.Rows[0]["specified_emai_to_send"].ToString(); 
                   
                        DataTable dtresultUserEmailId = g.ReturnData("Select email from employee_TB where employee_id ="+ Convert.ToInt32(Session["User_ID"])+"");
                   
                        if (dtresultUserEmailId.Rows.Count>0)
                    {
                        string empmailid = null;
                       
                        empmailid = dtresultUserEmailId.Rows[0]["email"].ToString(); 
                    
                        if (!String.IsNullOrEmpty(empmailid))
                        {
                            if (!String.IsNullOrEmpty(senderMailId) && !String.IsNullOrEmpty(password) && port != 0)
                            {
                                SendMailFunction(senderMailId, password, port, empmailid, "EMP");
                            }
                        }
                    }
                        DataTable dtcheckSendMail = g.ReturnData("Select customer_id from sendMailHistoryTB where customer_id = " + Convert.ToInt32(Session["Customer_ID"]) + " and DATE_FORMAT(send_Date, '%m/%d/%Y')='" + Convert.ToDateTime(date.ToString("MM/dd/yyyy")) + "'");
                   
                        if (dtcheckSendMail.Rows.Count > 0)
                    {
                        sendTo = null;
                    }
                    else
                    {
                        DataTable dtresultCustEmailId = g.ReturnData("Select email from customer_TB where customer_id = "+Convert.ToInt32(Session["Customer_ID"])+"");
                      
                        string cusmailid = null;
                       
                        cusmailid = dtresultCustEmailId.Rows[0]["email"].ToString(); 
                     
                        if (!String.IsNullOrEmpty(cusmailid))
                        {
                            if (!String.IsNullOrEmpty(cusmailid))
                            {
                                if (!String.IsNullOrEmpty(sendTo))
                                {
                                    sendTo = sendTo + "," + cusmailid;
                                }
                                else
                                {
                                    sendTo = cusmailid;
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(senderMailId) && !String.IsNullOrEmpty(password) && port != 0)
                        {
                            SendMailFunction(senderMailId, password, port, sendTo, "ALL");
                        }
                    }
                }
                else
                {
                    //g.ShowMessage(this.Page, "Your mail setting is not set or incorrect.");
                    return;
                }
            }

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            //  g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void SendMailFunction(string senderMailId, string password, int port, string sendTo, string who)
    {
        try
        {
            if (String.IsNullOrEmpty(sendTo))
            {
                return;
            }
            grdsendCustomerGaugeStatus.DataSource = null;
            grdsendCustomerGaugeStatus.DataBind();
            string sendQuery = null;
            if (who == "EMP")
            {
                sendQuery = @"Select cs.calibration_schedule_id, cs.gauge_id, gm.gauge_sr_no, gm.gauge_name, cs.next_due_date from calibration_schedule_TB as cs
                Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id
                where cs.status=1 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and cs.created_by_id=" + Convert.ToInt32(Session["User_ID"]) + " and cs.next_due_date <= (now()+15)";
                DataTable getnextDueStatus = g.ReturnData(sendQuery);
                grdsendCustomerGaugeStatus.DataSource = getnextDueStatus;
                grdsendCustomerGaugeStatus.DataBind();
            }
            else
            {
                try
                {
                    string stprocedure = "spDashBoardQuery";
                    DataSet ds = q.ProcdureWithTwoParam(stprocedure, 1, Convert.ToInt32(Session["Customer_ID"]));
                    
                    grdsendCustomerGaugeStatus.DataSource = ds.Tables[0];
                    grdsendCustomerGaugeStatus.DataBind();

                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                    //g.ShowMessage(this.Page, ex.Message);
                }
            }
            string[] strarryEmailId = sendTo.Split(',');
            string[] strsmtpget = senderMailId.Split('@');
            string a = strsmtpget[1].ToString();
            string b = "smtp.";
            b = b + a;
            for (int i = 0; i < strarryEmailId.Count(); i++)
            {
                MailMessage objMail = new MailMessage();
                //string str1 = "manasi.charatkar@excellenceit.in";
                string strSendTo = strarryEmailId[i].ToString();
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                mail.To.Add(strSendTo);
                mail.From = new MailAddress(senderMailId);
                mail.Subject = "Gauge Due Status";
                mail.Body = "<br><br>Hi Dear ..!!<br><br><br> ";
                mail.Body += "Your gauge next due status date as below list:";
                mail.Body += GetGridviewData(grdsendCustomerGaugeStatus);
                mail.Body += "<br><br><br><b>Gauge Management System!!</b><br><br><br>";
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Host = b;// "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(senderMailId, password);
                smtp.EnableSsl = true;
                smtp.Port = port;
                smtp.Send(mail);
            }
            DateTime date = new DateTime();
            date = DateTime.Now;
            string strDate = date.ToString("yyyy-MM-dd H:mm:ss");

            DataTable dtsavemailhist = g.ReturnData("Insert into sendMailHistoryTB (customer_id,user_id,send_Date) values(" + Convert.ToInt32(Session["Customer_ID"]) + "," + Convert.ToInt32(Session["User_ID"]) + ",'" + strDate + "')");
            //using (TTMSDataClassesDataContext ds = new TTMSDataClassesDataContext())
            // {
            //     sendMailHistoryTB sm = new sendMailHistoryTB();
            //     sm.customer_id = Convert.ToInt32(Session["Customer_ID"]);
            //     sm.user_id = Convert.ToInt32(Session["User_ID"]);
            //     sm.send_Date = DateTime.Now;
            //     ds.sendMailHistoryTBs.InsertOnSubmit(sm);
            //     ds.SubmitChanges();
            // }

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            // g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void displayIssueReturnPending()
    {
        try
        {
            string query = @"Select st.issued_id, st.issue_type, st.issued_to_type, st.issued_date, st.date_of_return,
            st.gauge_id, gt.gauge_name, gt.gauge_sr_no, gt.gauge_Manufature_Id, gt.gauge_type,
            case when st.issued_to_type='Employee' then em.employee_name
            else sp.supplier_name end as Name,
            case when st.issued_status='OPEN' then 'PENDING'
            else st.issued_status end as ReturnStatus
            from issued_status_TB as st
            Left Outer Join gaugeMaster_TB as gt
            ON st.gauge_id=gt.gauge_id
            Left Outer Join supplier_TB as sp
            ON st.issued_to_supplier_id=sp.supplier_id
            Left outer Join employee_TB as em
            ON st.issued_to_employee_id=em.employee_id
            where st.status=1 and st.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and st.issued_status='OPEN'";
            DataTable dtPendingIssuedStatus = g.ReturnData(query);
            grdIssue.DataSource = dtPendingIssuedStatus;
            grdIssue.DataBind();

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            //g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void displayMsaDueStatus()
    {
        try
        {
            //02/09/2017 By ABdul Due Seprate Table made of this report. DataTable dt = g.ReturnData("Select cs.calibration_schedule_id, cs.gauge_id, gm.gauge_sr_no,gm.gauge_name, cs.next_due_date,(CASE when (cs.frequency_type='YEAR') then DATEADD(YEAR,CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)else DATEADD(MONTH, CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)end) as MSADate from calibration_schedule_TB as cs Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id where cs.status=1 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and (CASE when (cs.frequency_type='YEAR') then DATEADD(YEAR,CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date) else DATEADD(MONTH, CONVERT(INT, CONVERT(nvarchar,cs.bias)), cs.last_calibration_date)end) <= (Getdate()+15)");
            string strQuery = " Select cs.msa_schedule_id, cs.gauge_id, gm.gauge_sr_no, gm.gauge_name, cs.next_due_date " +
                          " from msa_schedule_TB as cs " +
                           " Left Outer Join gaugeMaster_TB as gm ON cs.gauge_id=gm.gauge_id " +
                            " where cs.status=1 and cs.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " " +
                            " and cs.next_due_date <= (now()+15) " +
                " and cs.gauge_id NOT IN (Select mt.gauge_id from msa_transaction_TB mt where mt.calibration_schedule_id=cs.msa_schedule_id)";
            DataTable dt = g.ReturnData(strQuery);
            grdMsaDue.DataSource = dt;
            grdMsaDue.DataBind();
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    private void displayDueStatus()
    {
        try
        {
            string stprocedure = "spDashBoardQuery";
            DataSet ds = q.ProcdureWithTwoParam(stprocedure, 1, Convert.ToInt32(Session["Customer_ID"]));
               // var result = ds.spDashBoardQuery(Convert.ToInt32(Session["Customer_ID"]), 1).ToList();
                grdDueStatus.DataSource = ds.Tables[0];
                grdDueStatus.DataBind();
           
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
        }
    }

    private void displayGraph()
    {
        try
        {
            DateTime tdate1 = DateTime.Now;
            string tdate2 = tdate1.ToString("yyyy-MM-dd");
            DateTime fdate1 = tdate1.AddYears(-1);
            string fdate2 = fdate1.ToString("yyyy-MM-dd");
            DataTable dtdisp = new DataTable();
            int custid = Convert.ToInt32(Session["Customer_ID"]);

           // DataTable dtsch = g.ReturnData("Select (DATENAME(YY, last_calibration_date)+'-'+ Left(DATENAME(MM, last_calibration_date),3) )as Month, (Select  Count(calibration_schedule_id) from calibration_schedule_TB where last_calibration_date >='" + fdate1 + "' and last_calibration_date <='" + tdate1 + "' and DATENAME(MONTH, last_calibration_date)=DATENAME(MONTH, cas.last_calibration_date) and DATENAME(YY, last_calibration_date)=DATENAME(YY,cas.last_calibration_date) and customer_id='" + custid + "')as Scount from calibration_schedule_TB as cas where cas.last_calibration_date >='" + fdate1 + "' and cas.last_calibration_date <='" + tdate1 + "' and cas.customer_id='" + custid + "' order by last_calibration_date");
            DataTable dtsch=g.ReturnData("Select concat_WS('-',EXTRACT(YEAR FROM cas.last_calibration_date), DATE_FORMAT(cas.last_calibration_date,'%b') ) as Month, (Select  Count(calibration_schedule_id) from calibration_schedule_TB where DATE_FORMAT(last_calibration_date, '%Y-%m-%d') >='" + fdate2 + "' and DATE_FORMAT(last_calibration_date, '%Y-%m-%d') <='" + tdate2 + "' and  MONTHNAME(last_calibration_date) =MONTHNAME(cas.last_calibration_date) and EXTRACT(YEAR FROM last_calibration_date)=EXTRACT(YEAR FROM cas.last_calibration_date) and customer_id='" + custid + "') as Scount  from calibration_schedule_TB as cas where DATE_FORMAT(cas.last_calibration_date, '%Y-%m-%d')  >='" + fdate2 + "' and DATE_FORMAT(cas.last_calibration_date, '%Y-%m-%d') <='" + tdate2 + "' and cas.customer_id='" + custid + "' order by last_calibration_date");

            for (int i = 0; i < dtsch.Rows.Count; i++)
            {
                int cnt = 0;
                if (ViewState["dtdisp"] != null)
                {
                    dtdisp = (DataTable)ViewState["dtdisp"];
                }
                else
                {
                    DataColumn Month = dtdisp.Columns.Add("Month");
                    DataColumn Scount = dtdisp.Columns.Add("Scount");
                    DataColumn Tcount = dtdisp.Columns.Add("Tcount");
                }
                DataRow dr = dtdisp.NewRow();
                dr[0] = dtsch.Rows[i]["Month"].ToString();
                dr[1] = dtsch.Rows[i]["Scount"].ToString();
                dr[2] = 0;
                for (int j = 0; j < dtdisp.Rows.Count; j++)
                {
                    if (dtdisp.Rows[j]["Month"].ToString() == dtsch.Rows[i]["Month"].ToString())
                    {
                        cnt++;
                    }
                }
                if (cnt == 0)
                {
                    dtdisp.Rows.Add(dr);
                }

                ViewState["dtdisp"] = dtdisp;
            }
           // DataTable dttch = g.ReturnData("Select (DATENAME(YY, calibration_date)+'-'+  Left(DATENAME(MM, calibration_date),3) )as Month, (Select  Count(calibration_transaction_id) from calibration_transaction_TB where calibration_date >= '" + fdate1 + "' and calibration_date <='" + tdate1 + "' and DATENAME(MONTH, calibration_date)=DATENAME(MONTH,cts.calibration_date) and DATENAME(YY, calibration_date)=DATENAME(YY,cts.calibration_date) and customer_id='" + custid + "')as Tcount from calibration_transaction_TB cts where  cts.calibration_date >='" + fdate1 + "'  and cts.calibration_date <='" + tdate1 + "' and cts.customer_id='" + custid + "' order by calibration_date");
            DataTable dttch = g.ReturnData("Select concat_WS('-',EXTRACT(YEAR FROM cts.calibration_date), DATE_FORMAT(cts.calibration_date,'%b') ) as Month, (Select  Count(calibration_transaction_id) from calibration_transaction_TB where DATE_FORMAT(calibration_date, '%Y-%m-%d') >= '" + fdate2 + "' and DATE_FORMAT(calibration_date, '%Y-%m-%d') <='" + tdate2 + "' and  MONTHNAME(calibration_date)= MONTHNAME(cts.calibration_date) and EXTRACT(YEAR FROM calibration_date)=EXTRACT(YEAR FROM cts.calibration_date)  and customer_id=" + custid + ") as Tcount  from calibration_transaction_TB cts where DATE_FORMAT(cts.calibration_date, '%Y-%m-%d') >='" + fdate2 + "'  and DATE_FORMAT(cts.calibration_date, '%Y-%m-%d') <='" + tdate2 + "'  and cts.customer_id=" + custid + " order by calibration_date");
            for (int j = 0; j < dttch.Rows.Count; j++)
            {
                int cnt = 0;
                int cnt1 = 0;
                if (ViewState["dtdisp"] != null)
                {
                    dtdisp = (DataTable)ViewState["dtdisp"];
                }

                for (int k = 0; k < dtdisp.Rows.Count; k++)
                {
                    if (dtdisp.Rows[k]["Month"].ToString() == dttch.Rows[j]["Month"].ToString())
                    {
                        dtdisp.Rows[k]["Tcount"] = dttch.Rows[j]["Tcount"];
                        cnt++;
                    }

                }
                if (cnt == 0)
                {

                    if (ViewState["dtdisp"] != null)
                    {
                        dtdisp = (DataTable)ViewState["dtdisp"];
                    }
                    else
                    {
                        DataColumn Month = dtdisp.Columns.Add("Month");
                        DataColumn Scount = dtdisp.Columns.Add("Scount");
                        DataColumn Tcount = dtdisp.Columns.Add("Tcount");
                    }
                    DataRow dr = dtdisp.NewRow();
                    dr[0] = dttch.Rows[j]["Month"].ToString();
                    dr[1] = 0;
                    dr[2] = dttch.Rows[j]["Tcount"].ToString();
                    for (int l = 0; l < dtdisp.Rows.Count; l++)
                    {
                        if (dtdisp.Rows[l]["Month"].ToString() == dttch.Rows[j]["Month"].ToString())
                        {
                            cnt1++;
                        }
                    }
                    if (cnt1 == 0)
                    {
                        dtdisp.Rows.Add(dr);
                    }
                    ViewState["dtdisp"] = dtdisp;
                }
            }
            if (dtdisp.Rows.Count > 0)
            {
                Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                Chart1.Series["Series1"].XValueMember = "Month";
                Chart1.Series["Series1"].YValueMembers = "SCount";
                Chart1.Series["Series2"].XValueMember = "Month";
                Chart1.Series["Series2"].YValueMembers = "TCount";
                Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                //Chart1.Series["Series1"].ToolTip = #VALX;
                Chart1.Series["Series1"].IsValueShownAsLabel = true;

                // Chart1.ChartAreas[0].AxisX.IntervalAutoMode = System.Windows.Charting.IntervalAutoMode.VariableCount;

                Chart1.DataSource = dtdisp;
                Chart1.DataBind();
            }
            Session["dtdisp1"] = dtdisp;
            dtdisp = null;
            ViewState["dtdisp"] = null;

        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            //   g.ShowMessage(this.Page, ex.Message);
        }
    }
    // This Method is used to render gridview control
    public string GetGridviewData(GridView gv)
    {
        StringBuilder strBuilder = new StringBuilder();
        StringWriter strWriter = new StringWriter(strBuilder);
        HtmlTextWriter htw = new HtmlTextWriter(strWriter);
        gv.RenderControl(htw);
        return strBuilder.ToString();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void grdDueStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDueStatus.PageIndex = e.NewPageIndex;
        displayDueStatus();
    }
    protected void grdMsaDue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMsaDue.PageIndex = e.NewPageIndex;
        displayMsaDueStatus();
    }
    protected void grdIssue_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdIssue.PageIndex = e.NewPageIndex;
        displayIssueReturnPending();
    }
}