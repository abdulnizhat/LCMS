using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Condition_Gauge_Reciept : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stSatifactory = "";
    string stNotSatifactory = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindConditionGaugeReciptgrd();
                btnAddConditionGauge.Focus();
            }

        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void bindConditionGaugeReciptgrd()
    {
        try
        {
            DataTable dtCondGauge = q.getConditionGaugeReceiptDetails();
            grdConditionGauge.DataSource = dtCondGauge;
            grdConditionGauge.DataBind();
            checkAuthority();
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnCloseConditionGauge_Click(object sender, EventArgs e)
    {
        txtSatifactory.Text = "";
        //txtNotSatifactory.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddConditionGauge.Focus();
    }
    protected void btnSaveConditionGauge_Click(object sender, EventArgs e)
    {
        try
        {
          
           stSatifactory = txtSatifactory.Text.Trim();
           stSatifactory = Regex.Replace(stSatifactory, @"\s+", " ");
           //stNotSatifactory = txtNotSatifactory.Text.Trim();
           stNotSatifactory = Regex.Replace(stNotSatifactory, @"\s+", " ");
           if (btnSaveConditionGauge.Text == "Save")
                {
                    
                    DataTable dtedit = g.ReturnData("Select satifactory  from condition_gauge_receipt_tb where satifactory='" + stSatifactory + "'");

                    if (dtedit.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "This condition is already exist.");
                        return;
                    }
                    else
                    {
                        DataTable dtsave = g.ReturnData("Insert into condition_gauge_receipt_tb (satifactory,status) values('" + stSatifactory + "',True)");
                        g.ShowMessage(this.Page, "Condition of Gauge at reciept is saved successfully.");
                    }
                }
                else
                {
                    
                    DataTable dtedit = g.ReturnData("Select satifactory  from condition_gauge_receipt_tb where satifactory='" + stSatifactory + "' and country_Id<>" + Convert.ToInt32(lblSatifactoryId.Text) + "");
                    if (dtedit.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "This condition is already exist.");
                        return;
                    }
                    else
                    {
                        DataTable dtupdate = g.ReturnData("Update condition_gauge_receipt_tb set satifactory='" + stSatifactory + "'  where satifactory_id=" + Convert.ToInt32(lblSatifactoryId.Text) + "");
                        g.ShowMessage(this.Page, "Condition of Gauge at reciept is updated successfully.");
                    }
                }

                bindConditionGaugeReciptgrd();
                MultiView1.ActiveViewIndex = 0;
                txtSatifactory.Text = "";
                //txtNotSatifactory.Text = "";
                btnAddConditionGauge.Focus();
            
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnEditConditionGauge_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblSatifactoryId.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtSatifactory.Focus();
            DataTable dtedit = g.ReturnData("Select satifactory_id,satifactory, status from condition_gauge_receipt_tb where satifactory_id=" + Convert.ToInt32(lblSatifactoryId.Text) + "");

            txtSatifactory.Text = dtedit.Rows[0]["satifactory"].ToString();
            //txtNotSatifactory.Text = dtedit.Rows[0]["not_satifactory"].ToString();
            btnSaveConditionGauge.Text = "Update";
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddConditionGauge_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveConditionGauge.Text = "Save";
        txtSatifactory.Focus();
    }
    protected void grdConditionGauge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdConditionGauge.PageIndex = e.NewPageIndex;
        bindConditionGaugeReciptgrd();
    }
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("Condition_Gauge_Reciept.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddConditionGauge.Visible = true;
                }
                else
                {
                    btnAddConditionGauge.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdConditionGauge.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdConditionGauge.Rows[i].FindControl("btnEditConditionGauge");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdConditionGauge.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdConditionGauge.Rows[i].FindControl("btnEditConditionGauge");
                        lnk.Enabled = false;
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
}