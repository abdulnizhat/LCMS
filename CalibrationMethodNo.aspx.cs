using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CalibrationMethodNo : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stRevision = "";
    string stdesc = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindCalibMethodGrd();
                btnAddCalibMetho.Focus();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void bindCalibMethodGrd()
    {
        try
        {
            DataTable dtIsref = q.getCallMethodGrd();
            grdCalibMeth.DataSource = dtIsref;
            grdCalibMeth.DataBind();
            checkAuthority();
        }
        catch (Exception ex)
        {
           g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdCalibMeth_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCalibMeth.PageIndex = e.NewPageIndex;
        bindCalibMethodGrd();
    }
    protected void btnEditgrdCalibMeth_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblCallmethodId.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtDesc.Focus();
            DataTable dtedit = g.ReturnData("Select id,description,revesion,DATE_FORMAT(revesion_date, '%d/%m/%Y') as revesion_date, status from calibration_methodno_tb where id=" + Convert.ToInt32(lblCallmethodId.Text) + "");
            txtDesc.Text = dtedit.Rows[0]["description"].ToString();
            txtRevise.Text = dtedit.Rows[0]["revesion"].ToString();
            txtReviseDate.Text = dtedit.Rows[0]["revesion_date"].ToString();
            btnSaveCalibMethod.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveCalibMethod_Click(object sender, EventArgs e)
    {
        try
        {
            stdesc = txtDesc.Text.Trim();
            stdesc = Regex.Replace(stdesc, @"\s+", " ");
            stRevision = txtRevise.Text.Trim();
            stRevision = Regex.Replace(stRevision, @"\s+", " ");
            DateTime reviseDate = DateTime.ParseExact(txtReviseDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string reviseDate1 = reviseDate.ToString("yyyy-MM-dd H:mm:ss");
            
            if (btnSaveCalibMethod.Text == "Save")
            {
                DataTable dtedit = g.ReturnData("Select id,description,revesion,DATE_FORMAT(revesion_date, '%d/%m/%Y') as revesion_date, status from calibration_methodno_tb where revesion='" + stRevision + "'");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "This Revise is already exist.");
                    return;
                }
                else
                {
                    DataTable dtsave = g.ReturnData("Insert into calibration_methodno_tb (description,revesion,revesion_date, status) values('" + stdesc + "',  '" + stRevision + "', '" + reviseDate1 + "' ,True)");

                    g.ShowMessage(this.Page, "Calibration Method Number is saved successfully.");
                }
            }
            else
            {
                DataTable dtedit = g.ReturnData("Select id,description,revesion,DATE_FORMAT(revesion_date, '%d/%m/%Y') as revesion_date, status from calibration_methodno_tb where revesion='" + stRevision + "' and id<>" + Convert.ToInt32(lblCallmethodId.Text) + "");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "This Revise is already exist.");

                    return;
                }
                else
                {
                    DataTable dtupdate = g.ReturnData("Update calibration_methodno_tb set description='" + stdesc + "',  revesion='" + stRevision + "', revesion_date='" + reviseDate1 + "'   where id=" + Convert.ToInt32(lblCallmethodId.Text) + "");

                    g.ShowMessage(this.Page, "Calibration Method Number is updated successfully.");
                }
            }

            bindCalibMethodGrd();
            MultiView1.ActiveViewIndex = 0;
            txtDesc.Text = "";
            txtRevise.Text = "";
            txtReviseDate.Text = "";
            btnAddCalibMetho.Focus();

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        } 
    }
    protected void btnCloseCalibMethod_Click(object sender, EventArgs e)
    {
        txtDesc.Text = "";
        txtReviseDate.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddCalibMetho.Focus();
    }
    protected void btnAddCalibMetho_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveCalibMethod.Text = "Save";
        txtDesc.Focus();
    }
    private void checkAuthority()
    {
        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("CalibrationMethodNo.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');
                if (staustatus[0].ToString() == "True")
                {
                    btnAddCalibMetho.Visible = true;
                }
                else
                {
                    btnAddCalibMetho.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCalibMeth.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibMeth.Rows[i].FindControl("btnEditgrdCalibMeth");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdCalibMeth.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCalibMeth.Rows[i].FindControl("btnEditgrdCalibMeth");
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