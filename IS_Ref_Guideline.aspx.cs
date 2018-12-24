using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class IS_Ref_Guideline : System.Web.UI.Page
{
    #region declear variable
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    string stISRef = "";
    string stISRevision = "";
    string stISReleason = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                MultiView1.ActiveViewIndex = 0;
                bindISRefGrd();
                btnAddISRef.Focus();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
    private void bindISRefGrd()
    {
        try
        {
            DataTable dtIsref = q.getISRef();
            grdISRef.DataSource = dtIsref;
            grdISRef.DataBind();
            checkAuthority();
        }
        catch (Exception ex)
        {
           g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddISRef_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveISRef.Text = "Save";
        txtISRef.Focus();
    }
    protected void grdISRef_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdISRef.PageIndex = e.NewPageIndex;
        bindISRefGrd();
    }
    protected void btnEditISRef_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton Lnk = (LinkButton)sender;
            lblISRefId.Text = Lnk.CommandArgument;
            MultiView1.ActiveViewIndex = 1;
            txtISRef.Focus();
            DataTable dtedit = g.ReturnData("Select id,is_number,description,revesion, DATE_FORMAT(rev_date, '%d/%m/%Y') as rev_date, release_number, DATE_FORMAT(release_date, '%d/%m/%Y') as release_date, status from is_referance_guideline_tb where id=" + Convert.ToInt32(lblISRefId.Text) + "");
            txtISRef.Text = dtedit.Rows[0]["is_number"].ToString(); 
            txtDesc.Text = dtedit.Rows[0]["description"].ToString();
            txtRevise.Text = dtedit.Rows[0]["revesion"].ToString();
            txtReviseDate.Text = dtedit.Rows[0]["rev_date"].ToString();
            txtReleaseNo.Text = dtedit.Rows[0]["release_number"].ToString();
            txtReleaseDate.Text = dtedit.Rows[0]["release_date"].ToString();
            btnSaveISRef.Text = "Update";
        }
        catch (Exception ex)
        {
           g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnSaveISRef_Click(object sender, EventArgs e)
    {
        try
        {
            stISRef = txtISRef.Text.Trim();
            stISRef = Regex.Replace(stISRef, @"\s+", " ");
            stISRevision = txtRevise.Text.Trim();
            stISRevision = Regex.Replace(stISRevision, @"\s+", " ");
            stISReleason = txtReleaseNo.Text.Trim();
            stISReleason = Regex.Replace(stISReleason, @"\s+", " ");
            DateTime reviceDate = DateTime.ParseExact(txtReviseDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string  reviceDate1 = reviceDate.ToString("yyyy-MM-dd H:mm:ss");
            DateTime releaseDate = DateTime.ParseExact(txtReleaseDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string releaseDate1 = releaseDate.ToString("yyyy-MM-dd H:mm:ss"); 
            if (btnSaveISRef.Text == "Save")
            {
                DataTable dtedit = g.ReturnData("Select id,is_number,description,revesion,rev_date,release_number,release_date,status from is_referance_guideline_tb where is_number='" + stISRef + "'");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "I.S Number is already exist.");
                    return;
                }
                else
                {
                    DataTable dtsave = g.ReturnData("Insert into is_referance_guideline_tb (is_number,description,revesion,rev_date,release_number,release_date,status) values('" + stISRef + "',  '" + txtDesc.Text + "',  '" + stISRevision + "', '" + reviceDate1 + "', '" + stISReleason + "', '" + releaseDate1 + "',True)");

                    g.ShowMessage(this.Page, "I.S Refrence Guidline is saved successfully.");
                }
            }
            else
            {
                DataTable dtedit = g.ReturnData("Select id,is_number,description,revesion,rev_date,release_number,release_date,status from is_referance_guideline_tb where is_number='" + stISRef + "' and id<>" + Convert.ToInt32(lblISRefId.Text) + "");

                if (dtedit.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "I.S Number is already exist.");

                    return;
                }
                else
                {
                    DataTable dtupdate = g.ReturnData("Update is_referance_guideline_tb set is_number='" + stISRef + "',  description='" + txtDesc.Text + "', revesion='" + stISRevision + "', rev_date='" + reviceDate1 + "', release_number='" + stISReleason + "', release_date='" + releaseDate1 + "'   where id=" + Convert.ToInt32(lblISRefId.Text) + "");

                    g.ShowMessage(this.Page, "I.S Refrence Guidline is updated successfully.");
                }
            }

            bindISRefGrd();
            MultiView1.ActiveViewIndex = 0;
            txtISRef.Text = "";
            txtDesc.Text = "";
            txtReleaseDate.Text = "";
            txtReleaseNo.Text = "";
            txtRevise.Text = "";
            txtReviseDate.Text = "";
            btnAddISRef.Focus();

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        } 
    }
    protected void btnCloseISRef_Click(object sender, EventArgs e)
    {
        txtISRef.Text = "";
        txtDesc.Text = "";
        txtReleaseNo.Text = "";
        txtReleaseDate.Text = "";
        txtReleaseNo.Text = "";
        txtReviseDate.Text = "";
        MultiView1.ActiveViewIndex = 0;
        btnAddISRef.Focus();
    }
    private void checkAuthority()
    {

        try
        {
            int childId = 0;
            string stallauthority = "";
            childId = g.GetChildId("IS_Ref_Guideline.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');
                if (staustatus[0].ToString() == "True")
                {
                    btnAddISRef.Visible = true;
                }
                else
                {
                    btnAddISRef.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdISRef.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdISRef.Rows[i].FindControl("btnEditISRef");
                        lnk.Enabled = true;
                    }
                }
                else
                {
                    for (int i = 0; i < grdISRef.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdISRef.Rows[i].FindControl("btnEditISRef");
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