using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GaugeMaster : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    //string blob = "";
    string gaugeName = "";
    string gaugeSrNo = "";
    //byte[] imgByte = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {

            if (!Page.IsPostBack)
            {
                Session["dleteGage"] = "NO";
                btnAddGauge.Focus();
                MultiView1.ActiveViewIndex = 0;
                bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
                fillGaugeType();
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }

    }
    private void fillGaugeType()
    {
        try
        {
            DataTable dtFetchGaugeType = g.ReturnData("SELECT tm.id, concat_WS(':',sb.size_range,tm.type_of_gauge, sb.least_count) as gaugetype  FROM typemaster_tb tm Left Outer join subtypemaster_tb as sb ON tm.sub_type_id=sb.id");
            ddlgaugeType.DataSource = dtFetchGaugeType;
            ddlgaugeType.DataTextField = "gaugetype";
            ddlgaugeType.DataValueField = "id";
            ddlgaugeType.DataBind();
            ddlgaugeType.Items.Insert(0, "--Select--");
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
            childId = g.GetChildId("GaugeMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');
                if (staustatus[0].ToString() == "True")
                {
                    btnAddGauge.Visible = true;
                }
                else
                {
                    btnAddGauge.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdGauge.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGauge.Rows[i].FindControl("btnEditGauge");
                        LinkButton lnkdown = (LinkButton)grdGauge.Rows[i].FindControl("LnkDownLoadDocument");
                        lnkdown.Enabled = true;
                        lnk.Enabled = true;
                        //Session["dleteGage"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdGauge.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdGauge.Rows[i].FindControl("btnEditGauge");
                        lnk.Enabled = false;
                        LinkButton lnkdown = (LinkButton)grdGauge.Rows[i].FindControl("LnkDownLoadDocument");
                        lnkdown.Enabled = false;
                        // Session["dleteGage"] = "NO";
                    }
                }
            }
        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void bindGaugeGrid(int custId)
    {
        try
        {
            string searchValue = txtsearchValue.Text.Trim();
            searchValue = Regex.Replace(searchValue, @"\s+", " ");
            string stprocedure = "spGaugeDetails";
            DataTable dt = new DataTable();
            if (ddlsortby.SelectedItem.Text == "--Select--")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 6, custId, 0, "", "", "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
            {
                try
                {
                    int gaugeId = Convert.ToInt32(searchValue);
                    DataSet ds = q.ProcdureWith8Param(stprocedure, 2, custId, gaugeId, "", "", "", "", "");
                    dt = ds.Tables[0];
                }
                catch (Exception ex)
                {

                    g.ShowMessage(this.Page, "Gauge Id is accept only numeric value. " + ex.Message);
                }

            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
            {

                DataSet ds = q.ProcdureWith8Param(stprocedure, 7, custId, 0, searchValue, "", "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 8, custId, 0, "", searchValue, "", "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 9, custId, 0, "", "", searchValue, "", "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Size/Range-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 10, custId, 0, "", "", "", searchValue, "");
                dt = ds.Tables[0];
            }
            else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
            {
                DataSet ds = q.ProcdureWith8Param(stprocedure, 3, custId, 0, "", "", "", "", searchValue);
                dt = ds.Tables[0];
            }

            grdGauge.DataSource = dt;
            grdGauge.DataBind();

            checkAuthority();
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnAddGauge_Click(object sender, EventArgs e)
    {
        ddlType.Focus();
        MultiView1.ActiveViewIndex = 1;
        try
        {
            DataTable dtmax = g.ReturnData("Select MAX(gauge_id) from gaugeMaster_TB");
            int maxId = Convert.ToInt32(dtmax.Rows[0][0].ToString());

            txtGaugeId.Text = (maxId + 1).ToString();
        }
        catch (Exception)
        {
            txtGaugeId.Text = "1";
        }

    }
    protected void btnSaveGauge_Click(object sender, EventArgs e)
    {
        // For remove white space
        gaugeName = txtGaugeName.Text.Trim();
        gaugeName = Regex.Replace(gaugeName, @"\s+", " ");
        gaugeSrNo = txtGaugeSrNo.Text.Trim();
        gaugeSrNo = Regex.Replace(gaugeSrNo, @"\s+", " ");
        MultiView1.ActiveViewIndex = 1;
        try
        {
            #region File Upload
            //FileUpload img = (FileUpload)UploadDrwainfFile;
            //if (img.HasFile && img.PostedFile != null)
            //{
            //    imgByte = null;
            //    //To create a PostedFile
            //    HttpPostedFile File = UploadDrwainfFile.PostedFile;
            //    txtImageName.Text = File.FileName;
            //    string filePath = UploadDrwainfFile.PostedFile.FileName;
            //    string filename = Path.GetFileName(filePath);
            //    string ext = Path.GetExtension(filename);
            //    long fileSize = UploadDrwainfFile.FileContent.Length;
            //    if (fileSize <= 1024000) // Check in byte 1024000 Byte =1.024 MB . 1 MB.
            //    {
            //        string contenttype = String.Empty;
            //        switch (ext)
            //        {
            //            case ".jpeg":
            //                contenttype = "image/jpeg";
            //                break;
            //            case ".jpg":
            //                contenttype = "image/jpg";
            //                break;
            //            case ".png":
            //                contenttype = "image/png";
            //                break;
            //            case ".gif":
            //                contenttype = "image/gif";
            //                break;
            //            case ".pdf":
            //                contenttype = "application/pdf";
            //                break;
            //            case ".bmp":
            //                contenttype = "image/bmp";
            //                break;
            //        }
            //        if (contenttype != String.Empty)
            //        {
            //            Stream stream = File.InputStream;
            //            BinaryReader bReader = new BinaryReader(stream);
            //            imgByte = bReader.ReadBytes((int)stream.Length);
            //            //Create byte Array with file len
            //            // imgByte = new Byte[File.ContentLength];
            //            //force the control to load data in array
            //            // File.InputStream.Read(imgByte, 0, File.ContentLength);
            //        }
            //        else
            //        {
            //            g.ShowMessage(this.Page, "File type is not valid.");
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        g.ShowMessage(this.Page, "Selected file size is exceeds the size limit 1 MB only.");
            //        return;
            //    }

            //}
            #endregion


            if (btnSaveGauge.Text == "Save")
            {
                DataTable dtexist = g.ReturnData("Select gauge_sr_no from gaugeMaster_TB where gauge_sr_no='" + gaugeSrNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + "");
                if (dtexist.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Gauge serial number is already exist.");
                    return;
                }
                else
                {
                    saveUpdateGauge(0);
                }
            }
            else
            {
                int updateGaugeId = Convert.ToInt32(lblGaugeId.Text);
                DataTable dtexist = g.ReturnData("Select gauge_sr_no from gaugeMaster_TB where gauge_sr_no='" + gaugeSrNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_id=" + updateGaugeId + "");
                if (dtexist.Rows.Count > 0)
                {
                    saveUpdateGauge(updateGaugeId);
                }
                else
                {
                    DataTable dtexist1 = g.ReturnData("Select gauge_sr_no from gaugeMaster_TB where gauge_sr_no='" + gaugeSrNo + "' and customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " and gauge_id<>" + updateGaugeId + "");
                    if (dtexist1.Rows.Count > 0)
                    {
                        g.ShowMessage(this.Page, "Gauge serial number is already exist.");
                        return;
                    }
                    else
                    {
                        saveUpdateGauge(updateGaugeId);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
        clearFields();
        MultiView1.ActiveViewIndex = 0;
        bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
    }
    private void saveUpdateGauge(int updateGaugeId)
    {
        try
        {
            long id = 0;
            string stsize = "";
            string toleranceType = "";
            int tolId = 0;
            if (!string.IsNullOrEmpty(hftoleranceId.Value.ToString()))
            {
                tolId = Convert.ToInt32(hftoleranceId.Value);
            }
            string strGrade = "";
            if (!string.IsNullOrEmpty(hfGrade.Value.ToString()))
            {
                strGrade = hfGrade.Value;
                strGrade = strGrade.Replace("grd", "grd_");
            }
            if (ddlTolerance.SelectedIndex > 0)
            {
                toleranceType = ddlTolerance.SelectedItem.Text;
            }
            if (ddlType.SelectedIndex == 1)
            {
                stsize = txtSize.Text;
            }
            else if (ddlType.SelectedIndex == 2)
            {
                stsize = txtRange.Text;
            }

            if (btnSaveGauge.Text == "Save")
            {
                string stquery = "Insert into gaugeMaster_TB (customer_id,cycles,status,created_by_id,gauge_name,gauge_sr_no,gauge_Manufature_Id,gauge_type, go_tollerance_plus,go_tollerance_minus,go_were_limit,least_count,no_go_tollerance_plus,no_go_tollerance_minus,permisable_error1,permisable_error2,resolution,size_range,make,gauge_type_master_id,size2,tolerance_type,tolerance_id,tolerance_grd,tolerance_desc) VALUES(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17,?param18,?param19,?param20,?param21,?param22,?param23,?param24,?param25)";
                id = g.saveGaugeMasterwith18param(stquery, Convert.ToInt32(Session["Customer_ID"]), 0, "1", Convert.ToInt32(Session["User_ID"]), gaugeName, gaugeSrNo, txtManufactureId.Text, ddlType.SelectedItem.Text, txtGoTollerancePlus.Text, txtGoTolleranceMinus.Text, txtGoWereLimit.Text, txtLeastCount.Text, txtNoGoTollerancePlus.Text, txtNoGoTolleranceMinus.Text, txtPermisableError1.Text, txtPermisableError2.Text, txtResolution.Text, stsize, txtmake.Text, Convert.ToInt32(ddlgaugeType.SelectedValue), txtSizeMax.Text, toleranceType, tolId, strGrade, txtToleranceRange.Text);
                if (id != 0)
                {
                    g.ShowMessage(this.Page, "Gauge data is saved successfully.");
                }
            }
            else
            {
                string stquery = "Update gaugeMaster_TB set customer_id=?param1 ,cycles=?param2,status=?param3,created_by_id=?param4,gauge_name=?param5,gauge_sr_no=?param6,gauge_Manufature_Id=?param7,gauge_type=?param8,go_tollerance_plus=?param9,go_tollerance_minus=?param10, go_were_limit=?param11,least_count=?param12,no_go_tollerance_plus=?param13, no_go_tollerance_minus=?param14,permisable_error1=?param15,permisable_error2=?param16, resolution=?param17,size_range=?param18, make=?param19, gauge_type_master_id=?param20, size2=?param21, tolerance_type=?param22, tolerance_id=?param23, tolerance_grd=?param24, tolerance_desc=?param25  where gauge_id=" + updateGaugeId + "";
                id = g.saveGaugeMasterwith18param(stquery, Convert.ToInt32(Session["Customer_ID"]), 0, "1", Convert.ToInt32(Session["User_ID"]), gaugeName, gaugeSrNo, txtManufactureId.Text, ddlType.SelectedItem.Text, txtGoTollerancePlus.Text, txtGoTolleranceMinus.Text, txtGoWereLimit.Text, txtLeastCount.Text, txtNoGoTollerancePlus.Text, txtNoGoTolleranceMinus.Text, txtPermisableError1.Text, txtPermisableError2.Text, txtResolution.Text, stsize, txtmake.Text, Convert.ToInt32(ddlgaugeType.SelectedValue), txtSizeMax.Text, toleranceType, tolId, strGrade,txtToleranceRange.Text);
                //if (id != 0)
                //{
                    g.ShowMessage(this.Page, "Gauge data is updated successfully.");
                //}
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnClloseGauge_Click(object sender, EventArgs e)
    {
        clearFields();
        MultiView1.ActiveViewIndex = 0;
    }
    private void clearFields()
    {
        txtGaugeSrNo.Text = "";
        btnAddGauge.Focus();
        txtGaugeId.Text = "";
        txtGaugeName.Text = "";
        txtManufactureId.Text = "";
        ddlType.SelectedIndex = 0;
        txtGoWereLimit.Text = "";
        txtSize.Text = "";
        txtRange.Text = "";
        txtGoTollerancePlus.Text = "";
        txtGoTolleranceMinus.Text = "";
        txtNoGoTollerancePlus.Text = "";
        txtNoGoTolleranceMinus.Text = "";
        txtLeastCount.Text = "";
        txtResolution.Text = "";
        txtPermisableError1.Text = "";
        txtPermisableError2.Text = "";
        txtmake.Text = "";
        divSize.Visible = true;
        divRange.Visible = false;
        divLeastCount.Visible = true;
        divResolution.Visible = true;
        divGoandNoGoPlus.Visible = true;
        divGoandNoGoTolleranceminus.Visible = true;
        divPermisable1.Visible = true;
        divPermisable2.Visible = true;
        divGoWereLimit.Visible = true;
        btnSaveGauge.Text = "Save";
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedIndex == 1)
        {
            divSize.Visible = true;
            divRange.Visible = false;
            divLeastCount.Visible = false;
            divResolution.Visible = false;
            divGoandNoGoPlus.Visible = true;
            divGoandNoGoTolleranceminus.Visible = true;
            divPermisable1.Visible = false;
            divPermisable2.Visible = false;
            divGoWereLimit.Visible = true;
            ddlType.Focus();
        }
        else if (ddlType.SelectedIndex == 2)
        {
            divSize.Visible = false;
            divRange.Visible = true;
            divLeastCount.Visible = true;
            divResolution.Visible = true;
            divGoandNoGoPlus.Visible = false;
            divGoandNoGoTolleranceminus.Visible = false;
            divPermisable1.Visible = true;
            divPermisable2.Visible = true;
            divGoWereLimit.Visible = false;
            ddlType.Focus();


        }
        else
        {
            divSize.Visible = true;
            divRange.Visible = false;
            divLeastCount.Visible = true;
            divResolution.Visible = true;
            divGoandNoGoPlus.Visible = true;
            divGoandNoGoTolleranceminus.Visible = true;
            divPermisable1.Visible = true;
            divPermisable2.Visible = true;
            divGoWereLimit.Visible = true;

        }
    }
    protected void btnEditGauge_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            lblGaugeId.Text = lnk.CommandArgument;
            string stprocedure = "spGaugeDetailsReport";
            DataSet ds = q.ProcdureWith3Param(stprocedure, 2, 0, Convert.ToInt32(lblGaugeId.Text));
            DataTable dtgauge = ds.Tables[0];
            txtGaugeId.Text = dtgauge.Rows[0]["gauge_id"].ToString();
            txtGaugeName.Text = dtgauge.Rows[0]["gauge_name"].ToString();
            txtGaugeSrNo.Text = dtgauge.Rows[0]["gauge_sr_no"].ToString();
            txtManufactureId.Text = dtgauge.Rows[0]["gauge_Manufature_Id"].ToString();
            txtmake.Text = dtgauge.Rows[0]["make"].ToString();
            ddlgaugeType.SelectedValue = dtgauge.Rows[0]["gauge_type_master_id"].ToString();
            if (dtgauge.Rows[0]["gauge_type"].ToString() == "ATTRIBUTE")
            {
                ddlType.SelectedIndex = 1;
                divSize.Visible = true;
                divRange.Visible = false;
                divLeastCount.Visible = false;
                divResolution.Visible = false;
                divGoandNoGoPlus.Visible = true;
                divGoandNoGoTolleranceminus.Visible = true;
                divPermisable1.Visible = false;
                divPermisable2.Visible = false;
                divGoWereLimit.Visible = true;
                txtSize.Text = dtgauge.Rows[0]["size_range"].ToString();
                string tolType = dtgauge.Rows[0]["tolerance_type"].ToString();
                if (tolType=="Plug Tolerance")
                {
                    ddlTolerance.SelectedIndex = 1;
                }
                else if (tolType=="Snap Tolerance")
                {
                    ddlTolerance.SelectedIndex = 2;
                }
                else
                {
                    ddlTolerance.SelectedIndex = 0;
                }
                txtSizeMax.Text = dtgauge.Rows[0]["size2"].ToString();
                txtToleranceRange.Text = dtgauge.Rows[0]["tolerance_desc"].ToString();
                hftoleranceId.Value = dtgauge.Rows[0]["tolerance_id"].ToString();
                string grade = dtgauge.Rows[0]["tolerance_grd"].ToString();
                grade=grade.Replace("grd_","grd");
                hfGrade.Value = grade;


            }
            else if (dtgauge.Rows[0]["gauge_type"].ToString() == "VARIABLE")
            {
                ddlType.SelectedIndex = 2;
                divSize.Visible = false;
                divRange.Visible = true;
                divLeastCount.Visible = true;
                divResolution.Visible = true;
                divGoandNoGoPlus.Visible = false;
                divGoandNoGoTolleranceminus.Visible = false;
                divPermisable1.Visible = true;
                divPermisable2.Visible = true;
                divGoWereLimit.Visible = false;
                txtRange.Text = dtgauge.Rows[0]["size_range"].ToString();
            }
            txtGoWereLimit.Text = dtgauge.Rows[0]["go_were_limit"].ToString();
            txtGoTollerancePlus.Text = dtgauge.Rows[0]["go_tollerance_plus"].ToString();
            txtGoTolleranceMinus.Text = dtgauge.Rows[0]["go_tollerance_minus"].ToString();
            txtNoGoTollerancePlus.Text = dtgauge.Rows[0]["no_go_tollerance_plus"].ToString();
            txtNoGoTolleranceMinus.Text = dtgauge.Rows[0]["no_go_tollerance_minus"].ToString();
            txtLeastCount.Text = dtgauge.Rows[0]["least_count"].ToString();
            txtResolution.Text = dtgauge.Rows[0]["resolution"].ToString();
            txtPermisableError1.Text = dtgauge.Rows[0]["permisable_error1"].ToString();
            txtPermisableError2.Text = dtgauge.Rows[0]["permisable_error2"].ToString();
            MultiView1.ActiveViewIndex = 1;
            btnSaveGauge.Text = "Update";
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdGauge_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdGauge.PageIndex = e.NewPageIndex;
        bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));
    }
    protected void LnkDownLoadDocument_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            int gaugeId = Convert.ToInt32(lnk.CommandArgument);
            string contentType = "";

            DataTable dt = g.ReturnData("Select drawing_file, drawing_name from gaugeMaster_TB where gauge_id='" + gaugeId + "'");

            if (dt.Rows.Count > 0)
            {
                string fileExt = dt.Rows[0]["drawing_name"].ToString();
                if (String.IsNullOrEmpty(fileExt))
                {
                    g.ShowMessage(this.Page, "There is no file.");
                    return;
                }
                byte[] bytes = (byte[])(dt.Rows[0]["drawing_file"]);
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                contentType = MimeMapping.GetMimeMapping(fileExt);
                Response.ContentType = contentType;
                Response.AddHeader("content-disposition", "attachment;filename=" + dt.Rows[0]["drawing_name"].ToString());
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.SuppressContent = true;
            }
            else
            {
                g.ShowMessage(this.Page, "There is no file.");
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }

    }
    protected void ddlsortby_SelectedIndexChanged(object sender, EventArgs e)
    {
        searchBy.Text = "";
        txtsearchValue.Text = "";
        if (ddlsortby.SelectedIndex > 0)
        {
            btnSearch.Visible = true;
            lblName.Visible = true;
            txtsearchValue.Visible = true;
        }
        else
        {
            btnSearch.Visible = false;
            txtsearchValue.Visible = false;
            lblName.Text = "";
            txtsearchValue.Text = "";
            lblName.Visible = false;

        }
        if (ddlsortby.SelectedItem.Text == "Gauge Id-Wise")
        {
            lblName.Text = "Gauge Id";
            searchBy.Text = "gt.gauge_id";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Name-Wise")
        {
            lblName.Text = "Gauge Name";
            searchBy.Text = "gt.gauge_name";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Sr.No.-Wise")
        {
            lblName.Text = "Gauge Sr.No.";
            searchBy.Text = "gt.gauge_sr_no";
        }
        else if (ddlsortby.SelectedItem.Text == "Manufacture Id-Wise")
        {
            lblName.Text = "Manufacture Id";
            searchBy.Text = "gt.gauge_Manufature_Id";
        }
        else if (ddlsortby.SelectedItem.Text == "Gauge Type-Wise")
        {
            lblName.Text = "Gauge Type";
            searchBy.Text = "gt.gauge_type";
        }
        else if (ddlsortby.SelectedItem.Text == "Size/Range-Wise")
        {
            lblName.Text = "Size/Range";
            searchBy.Text = "gt.size_range";
        }
        else if (ddlsortby.SelectedItem.Text == "--Select--")
        {
            bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindGaugeGrid(Convert.ToInt32(Session["Customer_ID"]));

        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, "Some error found. " + ex.Message);
        }
    }
    protected void btnShowPopup_Click(object sender, EventArgs e)
    {
        try
        {
            string lSize = txtSize.Text;
            string hSize = txtSizeMax.Text;
            if (ddlTolerance.SelectedIndex > 0 && !string.IsNullOrEmpty(lSize) && !string.IsNullOrEmpty(hSize))
            {
                fetchToleranceData(ddlTolerance.SelectedItem.Text);
                tblleaddetails.Visible = true;
                ModalPopupExtenderTolerance.Show();
            }
            else
            {
                g.ShowMessage(this.Page, "Before Select Tolerance Type, Lower Size and Heigher Size");
                ModalPopupExtenderTolerance.Hide();
                tblleaddetails.Visible = false;
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fetchToleranceData(string toleranceType)
    {
        try
        {
            DataTable dtgetRange = new DataTable();
            dtgetRange = g.ReturnData("Select tolerance_grd_details.id, tm.range_type, tolerance_id, tolerance_grd_details.tolerance_type, sym_value as SYM,  grd_5 as GRD5, grd_6 as GRD6, grd_7 as GRD7, grd_8 as GRD8, grd_9 as GRD9, grd_10 as GRD10, grd_11 as GRD11, grd_12 as GRD12, grd_13 as GRD13, grd_14 as GRD14, grd_15 as GRD15, grd_16 as GRD16  from tolerance_grd_details  Left Outer Join tolerance_master_tb as tm  ON tm.id=tolerance_grd_details.tolerance_id where tolerance_grd_details.tolerance_type='" + toleranceType + "' group by tolerance_id");
            if (dtgetRange.Rows.Count > 0)
            {
                if (toleranceType == "Plug Tolerance")
                {
                    grdPlugTolerance.DataSource = dtgetRange;
                    grdPlugTolerance.DataBind();
                }
                else if (toleranceType == "Snap Tolerance")
                {
                    grdSnapTolerance.DataSource = dtgetRange;
                    grdSnapTolerance.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        string strgradeval = hfGradValue.Value;
        string strid = hfId.Value;
        string strtolId = hftoleranceId.Value;
        string strGrade = hfGrade.Value;
        strGrade = strGrade.Replace("grd", "grd_");
        string toleranceType = ddlTolerance.SelectedItem.Text;
        txtToleranceRange.Text = hfRange.Value + "-" + hfGrade.Value.ToUpper() + " T=" + hfGradValue.Value;

        if (!string.IsNullOrEmpty(strgradeval) && !string.IsNullOrEmpty(strtolId) && ddlTolerance.SelectedIndex > 0)
        {
            DataTable dtGettoleranceVal = g.ReturnData("Select id, tolerance_id, tolerance_type, sym_value as SYM,  " + strGrade + "  from tolerance_grd_details  where tolerance_type='" + toleranceType + "' and tolerance_id=" + Convert.ToInt32(strtolId) + "  order By id asc");
            if (dtGettoleranceVal.Rows.Count > 0)
            {
                decimal T = Convert.ToDecimal(dtGettoleranceVal.Rows[0][strGrade].ToString());
                decimal H2 = Convert.ToDecimal(dtGettoleranceVal.Rows[1][strGrade].ToString());
                decimal Y = Convert.ToDecimal(dtGettoleranceVal.Rows[2][strGrade].ToString());
                decimal Z = Convert.ToDecimal(dtGettoleranceVal.Rows[3][strGrade].ToString());
                decimal A = Convert.ToDecimal(dtGettoleranceVal.Rows[4][strGrade].ToString());

                decimal gotoleranceMinus = 0;
                decimal gotolerancePlus = 0;
                decimal goWereLimit = 0;
                decimal noGotoleranceMinus = 0;
                decimal noGotolerancePlus = 0;
                decimal lowerSize = Convert.ToDecimal(txtSize.Text);
                decimal heigherSize = Convert.ToDecimal(txtSizeMax.Text);

                if (toleranceType == "Plug Tolerance")
                {
                    gotoleranceMinus = lowerSize + Z;
                    gotoleranceMinus = gotoleranceMinus - H2;
                    gotolerancePlus = lowerSize + Z;
                    gotolerancePlus = gotolerancePlus + H2;
                    goWereLimit = lowerSize - Y;
                    goWereLimit = goWereLimit - A;
                    noGotoleranceMinus = heigherSize - H2;
                    noGotolerancePlus = heigherSize + H2;
                }
                else if (toleranceType == "Snap Tolerance")
                {
                    gotoleranceMinus = heigherSize - Z;
                    gotoleranceMinus = gotoleranceMinus - H2;

                    gotolerancePlus = heigherSize - Z;
                    gotolerancePlus = gotolerancePlus + H2;

                    goWereLimit = heigherSize + Y;
                    goWereLimit = goWereLimit - A;

                    noGotoleranceMinus = lowerSize - H2;

                    noGotolerancePlus = lowerSize + H2;
                }

                txtGoTolleranceMinus.Text = Convert.ToString(gotoleranceMinus);
                txtGoTollerancePlus.Text = Convert.ToString(gotolerancePlus);
                txtGoWereLimit.Text = Convert.ToString(goWereLimit);
                txtNoGoTolleranceMinus.Text = Convert.ToString(noGotoleranceMinus);
                txtNoGoTollerancePlus.Text = Convert.ToString(noGotolerancePlus);
            }
        }
    }
}