using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DataSheet : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    DisplaySerialNumber displaySerialNumber = new DisplaySerialNumber();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {

                MultiView1.ActiveViewIndex = 0;
                fillCustomer();
                BindCertificateData();
                //fillCertificateNumber();

            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void fillCertificateNumber()
    {
        try
        {
            ddlCertificateNo.Items.Clear();
            DataTable dt = displaySerialNumber.GetSerialNumber();
            ddlCertificateNo.DataSource = dt;
            ddlCertificateNo.DataTextField = "Certificate_No_List";
            ddlCertificateNo.DataValueField = "Certificate_No_List";
            ddlCertificateNo.DataBind();
            ddlCertificateNo.Items.Insert(0, "--Select--");
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fillGauge(int customerId)
    {
        try
        {
            DataTable dtFetchGauge = g.ReturnData("SELECT concat_WS(':', gauge_name, gauge_sr_no, gauge_Manufature_Id) gauge_name, gauge_id FROM gaugemaster_tb where customer_id=" + Convert.ToInt32(customerId) + " and status=True");
            ddlgauge.DataSource = dtFetchGauge;
            ddlgauge.DataTextField = "gauge_name";
            ddlgauge.DataValueField = "gauge_id";
            ddlgauge.DataBind();
            ddlgauge.Items.Insert(0, "--Select--");
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void fillCustomer()
    {
        try
        {
            DataTable dtFetchGaugeType = g.ReturnData("SELECT customer_id, customer_name  FROM customer_tb where status=True");
            ddlCustomer.DataSource = dtFetchGaugeType;
            ddlCustomer.DataTextField = "customer_name";
            ddlCustomer.DataValueField = "customer_id";
            ddlCustomer.DataBind();
            ddlCustomer.Items.Insert(0, "--Select--");
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    private void BindCertificateData()
    {
        try
        {
            string query = @"Select ct.id, ct.certificate_no, ct.gauge_id, gt.gauge_name, cus.customer_name, ct.customer_id,
tm.type_of_gauge,su.size_range, cgr.satifactory,ct.identification_mark_by, (case when ct.is_certification_done='False' then 'No' else 'Yes' End) as CertificationDone

from certificate_data_tb as ct
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
 ON ct.customer_id=cus.customer_id 
 where ct.is_certification_done='False' and ct.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " order by  ct.id desc ";
            DataTable dtBindGrid = g.ReturnData(query);
            grdCertificateData.DataSource = dtBindGrid;
            grdCertificateData.DataBind();
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
            childId = g.GetChildId("TypeMaster.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');

                if (staustatus[0].ToString() == "True")
                {
                    btnAddCerticateData.Visible = true;
                }
                else
                {
                    btnAddCerticateData.Visible = false;
                }
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCertificateData.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCertificateData.Rows[i].FindControl("btnEditCerData");
                        // LinkButton lnkdlet = (LinkButton)grdTypeMaster.Rows[i].FindControl("lnkDelete");
                        //lnkdlet.Enabled = true;
                        lnk.Enabled = true;
                        //Session["dleteGagePartLink"] = "YES";
                    }
                }
                else
                {
                    for (int i = 0; i < grdCertificateData.Rows.Count; i++)
                    {
                        LinkButton lnk = (LinkButton)grdCertificateData.Rows[i].FindControl("btnEditCerData");
                        lnk.Enabled = false;
                        //LinkButton lnkdlet = (LinkButton)grdTypeMaster.Rows[i].FindControl("lnkDelete");
                        //lnkdlet.Enabled = false;
                        //Session["dleteGagePartLink"] = "NO";
                    }
                }
            }


        }
        catch (Exception ex)
        {

            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void grdCertificateData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCertificateData.PageIndex = e.NewPageIndex;
        BindCertificateData();
    }
    protected void btnAddCerticateData_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
        btnSaveCertificateData.Text = "Save";
        txtRefrenceDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtDateOfReceipt.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtDateOfCalib.Text = DateTime.Now.ToString("dd/MM/yyyy");
        ddlCertificateNo.Enabled = true;
        fillCertificateNumber();
    }
    protected void btnEditCerData_Click(object sender, EventArgs e)
    {
        try
        {

            LinkButton lnk = (LinkButton)sender;
            lblCertificateDataId.Text = lnk.CommandArgument;
            string query = @"Select ct.id, ct.certificate_no, ct.gauge_id, gt.gauge_name, gt.make, cus.customer_name, ct.customer_id,su.size_range as sizeRang, 
ct.identification_mark_by, (case when ct.is_certification_done='False' then 'No' else 'Yes' End) as CertificationDone,ct.calibration_carriedout,ct.calib_frequency,
ct.next_clib_date, ct.frequency_type, ct.cretificate_date,ct.refrence_dc_no,ct.receipt_date,ct.calibration_date,ct.identification_mark_by,ct.refrence_dc_date,

gt.gauge_sr_no, gt.gauge_Manufature_Id, gt.size_range, tm.type_of_gauge, cgr.satifactory as conditionatreceipt,
tp.check_accuracy as testpurpose, rf.description as isrefrence, cm.description as calibmethod,  un.gauge_type as uncertinity,
 tm.master_equipment

from certificate_data_tb as ct
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
 ON ct.customer_id=cus.customer_id
 Left Outer join uncertainty_measurement_tb as un 
 ON tm.uncertinity=un.id
 Where ct.id=" + Convert.ToInt32(lblCertificateDataId.Text) + " ";
            txtMasterEquipmentUsed.Text = "";
            DataTable dtFetchRecord = g.ReturnData(query);
            if (dtFetchRecord.Rows.Count > 0)
            {
                #region Fettch Record
                fillCustomer();
                ddlCustomer.SelectedValue = dtFetchRecord.Rows[0]["customer_id"].ToString();
                fillGauge(Convert.ToInt32(dtFetchRecord.Rows[0]["customer_id"].ToString()));
                ddlgauge.SelectedValue = dtFetchRecord.Rows[0]["gauge_id"].ToString();
                txtCalibCarriedOutat.Text = dtFetchRecord.Rows[0]["calibration_carriedout"].ToString();
                txtCalibrationFrequency.Text = dtFetchRecord.Rows[0]["calib_frequency"].ToString();
                txtCalibrationMethodNumber.Text = dtFetchRecord.Rows[0]["calibmethod"].ToString();
                txtConditionOfReceipt.Text = dtFetchRecord.Rows[0]["calibration_carriedout"].ToString();
                DateTime dtDateOfCalib = Convert.ToDateTime(dtFetchRecord.Rows[0]["calibration_date"].ToString());
                txtDateOfCalib.Text = dtDateOfCalib.ToString("dd/MM/yyyy");
                DateTime dtDateOfReceipt = Convert.ToDateTime(dtFetchRecord.Rows[0]["receipt_date"].ToString());
                txtDateOfReceipt.Text = dtDateOfReceipt.ToString("dd/MM/yyyy");
                txtdentificationMarkedByRML.Text = dtFetchRecord.Rows[0]["identification_mark_by"].ToString();
                txtIdentification.Text = dtFetchRecord.Rows[0]["gauge_sr_no"].ToString();
                txtIsrefGuideLine.Text = dtFetchRecord.Rows[0]["isrefrence"].ToString();
                txtMake.Text = dtFetchRecord.Rows[0]["make"].ToString();
                txtMfgNo.Text = dtFetchRecord.Rows[0]["gauge_Manufature_Id"].ToString();
                DateTime dtNextCalibDate = Convert.ToDateTime(dtFetchRecord.Rows[0]["next_clib_date"].ToString());
                txtNextCalibDate.Text = dtNextCalibDate.ToString("dd/MM/yyyy");
                DateTime dtRefrenceDate = Convert.ToDateTime(dtFetchRecord.Rows[0]["refrence_dc_date"].ToString());
                txtRefrenceDate.Text = dtRefrenceDate.ToString("dd/MM/yyyy");
                txtRefrenceDcno.Text = dtFetchRecord.Rows[0]["refrence_dc_no"].ToString();
                txtSize.Text = dtFetchRecord.Rows[0]["size_range"].ToString();
                txtTestPurpose.Text = dtFetchRecord.Rows[0]["testpurpose"].ToString();
                txtTypeOfgauge.Text = dtFetchRecord.Rows[0]["type_of_gauge"].ToString();
                txtuncertintiy.Text = dtFetchRecord.Rows[0]["uncertinity"].ToString();
                string masterEquip = dtFetchRecord.Rows[0]["master_equipment"].ToString();
                if (dtFetchRecord.Rows[0]["frequency_type"].ToString() == "DAYS")
                {
                    ddlFrequencyType.SelectedIndex = 1;
                }
                else if (dtFetchRecord.Rows[0]["frequency_type"].ToString() == "MONTH")
                {
                    ddlFrequencyType.SelectedIndex = 2;
                }
                else if (dtFetchRecord.Rows[0]["frequency_type"].ToString() == "YEAR")
                {
                    ddlFrequencyType.SelectedIndex = 3;
                }
                else
                {
                    ddlFrequencyType.SelectedIndex = 0;
                }
                ddlCertificateNo.Items.Insert(0, dtFetchRecord.Rows[0]["certificate_no"].ToString());
                ddlCertificateNo.SelectedIndex = 0;
                ddlCertificateNo.Enabled = false;
                string[] masterEquipArray = masterEquip.Split(',');
                int cnt = 1;
                for (int j = 0; j < masterEquipArray.Count(); j++)
                {
                    int masterEquipId = Convert.ToInt32(masterEquipArray[j].ToString());
                    DataTable dtFetchMasterEquip = g.ReturnData("Select description from master_equipment_used_tb where id=" + Convert.ToInt32(masterEquipId) + "");

                    if (!string.IsNullOrEmpty(txtMasterEquipmentUsed.Text))
                    {
                        txtMasterEquipmentUsed.Text = txtMasterEquipmentUsed.Text + " " + cnt.ToString() + " . " + dtFetchMasterEquip.Rows[0]["description"].ToString();
                        cnt++;
                    }
                    else
                    {
                        txtMasterEquipmentUsed.Text = cnt.ToString() + " . " + dtFetchMasterEquip.Rows[0]["description"].ToString();
                        cnt++;
                    }
                }
                MultiView1.ActiveViewIndex = 1;
                btnSaveCertificateData.Text = "Update";
                #endregion
            }
            else
            {
                clearFields();
            }


        }
        catch (Exception ex)
        {
            Logger.Error("Edit On certificate Data Sheet: " + ex.Message);
        }
    }
    protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCustomer.SelectedIndex > 0)
        {
            fillGauge(Convert.ToInt32(ddlCustomer.SelectedValue));
        }
        else
        {
            ddlCustomer.Items.Clear();
        }

    }
    protected void btnSaveCertificateData_Click(object sender, EventArgs e)
    {
        try
        {


            DateTime refDate = DateTime.ParseExact(txtRefrenceDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strRefDate = refDate.ToString("yyyy-MM-dd H:mm:ss");

            DateTime dateReceipt = DateTime.ParseExact(txtDateOfReceipt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strDateReceipt = dateReceipt.ToString("yyyy-MM-dd H:mm:ss");

            DateTime dateOfCalib = DateTime.ParseExact(txtDateOfCalib.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strDateOfCalib = dateOfCalib.ToString("yyyy-MM-dd H:mm:ss");

            DateTime nextdateOfCalib = DateTime.ParseExact(txtNextCalibDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string strnextdateOfCalib = nextdateOfCalib.ToString("yyyy-MM-dd H:mm:ss");

            string frequency = txtCalibrationFrequency.Text;
            string frequencytype = ddlFrequencyType.SelectedItem.Text;

            string strCurentDatae = "";
            if (btnSaveCertificateData.Text == "Save")
            {
                DataTable dtExistData = g.ReturnData("select id from certificate_data_tb where gauge_id=" + Convert.ToInt32(ddlgauge.SelectedValue) + " and is_certification_done=False order by id desc");
                if (dtExistData.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "This gauge already exist.");
                    return;
                }

                string query = ("Insert into certificate_data_tb (certificate_no, cretificate_date, gauge_id, customer_id, refrence_dc_no, refrence_dc_date, receipt_date, calibration_date, identification_mark_by, created_date, created_by_id, is_certification_done,status, calibration_carriedout,next_clib_date, calib_frequency,frequency_type) values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13,?param14,?param15,?param16,?param17)");
                DateTime date = DateTime.Now;
                strCurentDatae = date.ToString("yyyy-MM-dd H:mm:ss");
                long id = g.SaveCertificateData(query, ddlCertificateNo.SelectedValue, strCurentDatae, Convert.ToInt32(ddlgauge.SelectedValue), Convert.ToInt32(ddlCustomer.SelectedValue), txtRefrenceDcno.Text, strRefDate, strDateReceipt, strDateOfCalib, txtdentificationMarkedByRML.Text, strCurentDatae, Convert.ToInt32(Session["User_ID"].ToString()), false, true, txtCalibCarriedOutat.Text, strnextdateOfCalib, frequency, frequencytype);
                if (id != 0)
                {
                    // Below comented code written in General class
                    ////string strCertificateNumber = "CC00" + Convert.ToString(id);
                    ////DataTable dtUpdate = g.ReturnData("Update certificate_data_tb set certificate_no='" + strCertificateNumber + "' where id=" + Convert.ToInt32(id) + "");
                    g.ShowMessage(this.Page, "Certification Data Sheet is saved successfully.");
                    BindCertificateData();
                    MultiView1.ActiveViewIndex = 0;
                    clearFields();
                    ddlgauge.Items.Clear();
                    ddlCustomer.SelectedIndex = 0;
                }
                else
                {
                    g.ShowMessage(this.Page, "Data is not saved. Some error founded.");
                }

            }
            if (btnSaveCertificateData.Text == "Update")
            {
                DataTable dtExistData = g.ReturnData("select id from certificate_data_tb where gauge_id=" + Convert.ToInt32(ddlgauge.SelectedValue) + " and is_certification_done=False and id<>" + Convert.ToInt32(lblCertificateDataId.Text) + " ");
                if (dtExistData.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "This gauge already exist.");
                    return;
                }

                DateTime date = DateTime.Now;
                strCurentDatae = date.ToString("yyyy-MM-dd H:mm:ss");

                string query = "Update certificate_data_tb set gauge_id= " + Convert.ToInt32(ddlgauge.SelectedValue) + ", refrence_dc_no='" + txtRefrenceDcno.Text + "', refrence_dc_date='" + strRefDate + "', receipt_date='" + strDateReceipt + "', calibration_date='" + strDateOfCalib + "', identification_mark_by='" + txtdentificationMarkedByRML.Text + "', created_date='" + strCurentDatae + "', created_by_id=" + Convert.ToInt32(Session["User_ID"].ToString()) + ", calibration_carriedout='" + txtCalibCarriedOutat.Text + "', next_clib_date='" + strnextdateOfCalib + "', calib_frequency='" + frequency + "', frequency_type='" + frequencytype + "' where id=" + Convert.ToInt32(lblCertificateDataId.Text) + " ";
                DataTable dtUpdate = g.ReturnData(query);
                g.ShowMessage(this.Page, "Certification Data Sheet is updated successfully.");
                BindCertificateData();
                MultiView1.ActiveViewIndex = 0;
                clearFields();
                ddlgauge.Items.Clear();
                ddlCustomer.SelectedIndex = 0;

            }
        }
        catch (Exception ex)
        {
            Logger.Error("Save Certificate Data Sheet: " + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
        ddlgauge.Items.Clear();
        ddlCustomer.SelectedIndex = 0;
    }
    protected void ddlgauge_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlgauge.SelectedIndex > 0)
        {
            fillGaugeDetails(Convert.ToInt32(ddlgauge.SelectedValue));
        }
        else
        {
            clearFields();
        }
    }

    private void clearFields()
    {
        try
        {
            txtDateOfCalib.Text = txtCalibCarriedOutat.Text = txtCalibrationMethodNumber.Text = txtConditionOfReceipt.Text = txtDateOfReceipt.Text =
            txtdentificationMarkedByRML.Text = txtIdentification.Text = txtIsrefGuideLine.Text = txtMake.Text = txtMasterEquipmentUsed.Text =
            txtMfgNo.Text = txtRefrenceDate.Text = txtRefrenceDcno.Text = txtSize.Text = txtTestPurpose.Text = txtTypeOfgauge.Text = txtuncertintiy.Text =
             txtNextCalibDate.Text = txtCalibrationFrequency.Text = string.Empty;

        }
        catch (Exception)
        {

            throw;
        }
    }

    private void fillGaugeDetails(int gaugeId)
    {
        try
        {
            string strQuery = @"SELECT gt.gauge_id,gt.gauge_name, gt.make, gt.gauge_sr_no, gt.gauge_Manufature_Id, gt.size_range, tm.type_of_gauge, cgr.satifactory as conditionatreceipt,
      tp.check_accuracy as testpurpose, rf.description as isrefrence, cm.description as calibmethod,  un.gauge_type as uncertinity,

 tm.master_equipment from lcms_db.gaugemaster_tb as gt Left Outer join typemaster_tb as tm  ON gt.gauge_type_master_id=tm.id  Left Outer join condition_gauge_receipt_tb as cgr 
 ON tm.condition_of_receipt=cgr.satifactory_id  Left Outer join test_purpose_tb as tp  ON tm.test_purpose=tp.id  Left Outer join calibration_methodno_tb as cm 
 ON tm.calibration_method=cm.id  Left Outer join master_equipment_used_tb as meq  ON tm.master_equipment=meq.id  Left Outer join subtypemaster_tb as su 
 ON tm.sub_type_id=su.id  Left Outer join is_referance_guideline_tb as rf  ON tm.is_ref_guidline=rf.id  Left Outer join uncertainty_measurement_tb as un 
 ON tm.uncertinity=un.id  where gt.gauge_id=" + Convert.ToInt32(gaugeId) + "  order by gt.gauge_id desc";
            txtMasterEquipmentUsed.Text = "";
            DataTable dtFetchGaugedetails = g.ReturnData(strQuery);
            if (dtFetchGaugedetails.Rows.Count > 0)
            {
                txtTypeOfgauge.Text = dtFetchGaugedetails.Rows[0]["type_of_gauge"].ToString();

                txtIdentification.Text = dtFetchGaugedetails.Rows[0]["gauge_sr_no"].ToString();// IdentificationNo
                txtMfgNo.Text = dtFetchGaugedetails.Rows[0]["gauge_Manufature_Id"].ToString();
                txtMake.Text = dtFetchGaugedetails.Rows[0]["make"].ToString();
                txtSize.Text = dtFetchGaugedetails.Rows[0]["size_range"].ToString();
                txtConditionOfReceipt.Text = dtFetchGaugedetails.Rows[0]["conditionatreceipt"].ToString();
                txtTestPurpose.Text = dtFetchGaugedetails.Rows[0]["testpurpose"].ToString();
                txtIsrefGuideLine.Text = dtFetchGaugedetails.Rows[0]["isrefrence"].ToString();
                txtCalibrationMethodNumber.Text = dtFetchGaugedetails.Rows[0]["calibmethod"].ToString();
                txtuncertintiy.Text = dtFetchGaugedetails.Rows[0]["uncertinity"].ToString();
                txtTypeOfgauge.Text = dtFetchGaugedetails.Rows[0]["type_of_gauge"].ToString();

                string masterEquip = dtFetchGaugedetails.Rows[0]["master_equipment"].ToString();
                string[] masterEquipArray = masterEquip.Split(',');
                int cnt = 1;
                for (int j = 0; j < masterEquipArray.Count(); j++)
                {
                    int masterEquipId = Convert.ToInt32(masterEquipArray[j].ToString());
                    DataTable dtFetchMasterEquip = g.ReturnData("Select description from master_equipment_used_tb where id=" + Convert.ToInt32(masterEquipId) + "");

                    if (!string.IsNullOrEmpty(txtMasterEquipmentUsed.Text))
                    {
                        txtMasterEquipmentUsed.Text = txtMasterEquipmentUsed.Text + " " + cnt.ToString() + " . " + dtFetchMasterEquip.Rows[0]["description"].ToString();
                        cnt++;
                    }
                    else
                    {
                        txtMasterEquipmentUsed.Text = cnt.ToString() + " . " + dtFetchMasterEquip.Rows[0]["description"].ToString();
                        cnt++;
                    }
                }
            }
            else
            {
                clearFields();
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Fill Gauge Details On Certificate Data Sheet :" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);

        }
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnk = (LinkButton)sender;
            string id = lnk.CommandArgument;
            string strQuery = @"Select ct.id, ct.gauge_id, su.gauge_type, su.size_range
  from certificate_data_tb as ct Left Outer join gaugemaster_tb as gt  ON ct.gauge_id=gt.gauge_id
 Left Outer join typemaster_tb as tm  ON gt.gauge_type_master_id=tm.id  Left Outer join subtypemaster_tb as su
 ON tm.sub_type_id=su.id  where ct.id=" + Convert.ToInt32(id) + "";
            DataTable dtgetgaugeId = g.ReturnData(strQuery);
            string gaugeType = "";
            string sizerange = "";
            string gaugeId = "";
            string certId = "";
            string queryStringVal = "";
            string reportType = "blank";
            if (dtgetgaugeId.Rows.Count > 0)
            {
                gaugeType = dtgetgaugeId.Rows[0]["gauge_type"].ToString();
                sizerange = dtgetgaugeId.Rows[0]["size_range"].ToString();
                gaugeId = dtgetgaugeId.Rows[0]["gauge_id"].ToString();
                certId = dtgetgaugeId.Rows[0]["id"].ToString();
                queryStringVal = gaugeId + "," + gaugeType + "," + sizerange + "," + certId + "," + reportType;
            }
            else
            {
                gaugeType = "";
                sizerange = "";
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
                    Response.Redirect("CertificationDataBlankReportViewer.aspx?queryStringVal=" + queryStringVal);
                }
                else if (gaugeType == "For Attribute")
                {
                    Response.Redirect("AttributeReportViewerBlank.aspx?queryStringVal=" + queryStringVal);
                }
                else if (gaugeType == "Pressure")
                {
                    Response.Redirect("pressurreeportViewerBlank.aspx?queryStringVal=" + queryStringVal);
                }
                else if (gaugeType == "Feeler")
                {
                    Response.Redirect("FeelerReportViewerBlank.aspx?queryStringVal=" + queryStringVal);
                }
                else if (gaugeType == "Lever")
                {
                    Response.Redirect("leverBoreReportViewerBlank.aspx?queryStringVal=" + queryStringVal);
                }
                else if (gaugeType == "Plunger")
                {
                    Response.Redirect("plungerReportViewerBlank.aspx?queryStringVal=" + queryStringVal);
                }
                else if (gaugeType == "Vernier")
                {
                    Response.Redirect("vernierReportViewerBlank.aspx?queryStringVal=" + queryStringVal);
                }
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void ddlFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFrequencyType.Focus();
        fillScheduleDate();
    }
    protected void txtCalibrationFrequency_TextChanged(object sender, EventArgs e)
    {
        txtDateOfCalib.Focus();
        fillScheduleDate();
    }
    private void fillScheduleDate()
    {
        try
        {
            if (String.IsNullOrEmpty(txtDateOfCalib.Text))
            {

                txtNextCalibDate.Text = string.Empty;
                return;
            }
            int fereq = 0;
            string strdate = txtDateOfCalib.Text;
            DateTime dt = DateTime.ParseExact(strdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime revDate = dt;
            if (String.IsNullOrEmpty(txtCalibrationFrequency.Text))
            {
                //g.ShowMessage(this.Page, "Enter the value of calibration frequency.");
                txtNextCalibDate.Text = string.Empty;
                return;
            }
            else
            {
                fereq = Convert.ToInt32(txtCalibrationFrequency.Text);
            }
            if (ddlFrequencyType.SelectedIndex > 0)
            {
                if (ddlFrequencyType.SelectedIndex == 1)
                {
                    DateTime nextDuedate = revDate.AddDays(fereq);
                    txtNextCalibDate.Text = nextDuedate.ToString("dd/MM/yyyy");
                }
                if (ddlFrequencyType.SelectedIndex == 2)
                {
                    DateTime nextDuedate = revDate.AddMonths(fereq);
                    txtNextCalibDate.Text = nextDuedate.ToString("dd/MM/yyyy");
                }
                if (ddlFrequencyType.SelectedIndex == 3)
                {
                    DateTime nextDuedateYear = revDate.AddYears(fereq);
                    txtNextCalibDate.Text = nextDuedateYear.ToString("dd/MM/yyyy");

                }
            }
            else
            {
                //g.ShowMessage(this.Page, "Select frequency type.");
                txtNextCalibDate.Text = string.Empty;
                return;
            }
        }
        catch (Exception ex)
        {
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    protected void txtDateOfCalib_TextChanged(object sender, EventArgs e)
    {
        fillScheduleDate();
    }
}