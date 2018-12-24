using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterEquipmentUsed : System.Web.UI.Page
{
     #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stMasterSrNo = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindMEqpUsed();
                btnAddEqp.Focus();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
        
    }

    private void bindMEqpUsed()
    {
        try
        {
            DataTable dtMeqpused = q.getMeqpused();
            grdEqp.DataSource = dtMeqpused;
            grdEqp.DataBind();
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
            childId = g.GetChildId("MasterEquipmentUsed.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');
                if (staustatus[0].ToString() == "True")
                {
                    btnAddEqp.Visible = true;
                }
                else
                {
                    btnAddEqp.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdEqp.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdEqp.Rows[i].FindControl("btnEditEqp");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdEqp.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdEqp.Rows[i].FindControl("btnEditEqp");
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
    protected void btnAddEqp_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveEqp.Text = "Save";
        txtDesc.Focus();
    }
    protected void btnEditEqp_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblMEqpId.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtDesc.Focus();
            DataTable dtedit = g.ReturnData("Select id,description,master_sr, DATE_FORMAT(calibration_date, '%d/%m/%Y') as calibration_date, DATE_FORMAT(valid_till_date, '%d/%m/%Y') as valid_till_date, status from master_equipment_used_tb where id=" + Convert.ToInt32(lblMEqpId.Text) + "");
            txtDesc.Text = dtedit.Rows[0]["description"].ToString();
            txtMasterSrNo.Text = dtedit.Rows[0]["master_sr"].ToString();
            txtCalibDate.Text = dtedit.Rows[0]["calibration_date"].ToString();
            txtValidDate.Text = dtedit.Rows[0]["valid_till_date"].ToString();
            btnSaveEqp.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveEqp_Click(object sender, EventArgs e)
    {
        try
        {
            stMasterSrNo = txtMasterSrNo.Text.Trim();
            stMasterSrNo = Regex.Replace(stMasterSrNo, @"\s+", " ");
            DateTime calibDate = DateTime.ParseExact(txtCalibDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string calibDate1 = calibDate.ToString("yyyy-MM-dd H:mm:ss");
            DateTime validDate = DateTime.ParseExact(txtValidDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string validDate1 = validDate.ToString("yyyy-MM-dd H:mm:ss");
            if (btnSaveEqp.Text == "Save")
            {
                DataTable dtedit = g.ReturnData("Select id from master_equipment_used_tb where master_sr='" + stMasterSrNo + "'");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Master serial number is already exist.");
                    return;
                }
                else
                {
                    DataTable dtsave = g.ReturnData("Insert into master_equipment_used_tb (description,master_sr, calibration_date, valid_till_date, status) values('" + txtDesc.Text + "',  '" + stMasterSrNo + "', '" + calibDate1 + "', '" + validDate1 + "',True)");

                    g.ShowMessage(this.Page, "Master Equipment Used is saved successfully.");
                }
            }
            else
            {
                DataTable dtedit = g.ReturnData("Select id from master_equipment_used_tb where master_sr='" + stMasterSrNo + "' and  id<>" + Convert.ToInt32(lblMEqpId.Text) + "");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Master serial number is already exist.");

                    return;
                }
                else
                {
                    DataTable dtupdate = g.ReturnData("Update master_equipment_used_tb set description='" + txtDesc.Text + "', master_sr='" + stMasterSrNo + "', calibration_date='" + calibDate1 + "', valid_till_date='" + validDate1 + "'   where id=" + Convert.ToInt32(lblMEqpId.Text) + "");

                    g.ShowMessage(this.Page, "Master Equipment Used is updated successfully.");
                }
            }

            bindMEqpUsed();
            MultiView1.ActiveViewIndex = 0;
            txtDesc.Text = "";
            txtMasterSrNo.Text = "";
            txtCalibDate.Text = "";
            txtValidDate.Text = "";
            btnAddEqp.Focus();

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        } 
    }
    protected void btnCloseEqp_Click(object sender, EventArgs e)
    {
        txtDesc.Text = "";
        txtMasterSrNo.Text = "";
        txtCalibDate.Text = "";
        txtValidDate.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddEqp.Focus();
    }
    protected void grdEqp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdEqp.PageIndex = e.NewPageIndex;
        bindMEqpUsed();
    }
}