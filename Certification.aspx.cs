using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Certification : System.Web.UI.Page
{
    Genreal g = new Genreal();
    QueryClass q = new QueryClass();
    DataTable dtNominalSize = new DataTable();
    DataTable dtGetResult = new DataTable();
    DataTable dtGetResultPlunger2Grd = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!Page.IsPostBack)
            {

                MultiView1.ActiveViewIndex = 0;
                BindCertificateData();
                grdCalibResult.Visible = false;
                grdVernier.Visible = false;
                divstanderd.Visible = false;
                divVernier.Visible = false;
                grdPlunger2.Visible = false;
                grdPlunger1.Visible = false;
                grdLeverBore.Visible = false;
                grdFeeler.Visible = false;
                grdPressureFowrding.Visible = false;
                //grdPressureReverse.Visible = false;
                tblforAttribute.Visible = false;

            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    private void BindCertificateData()
    {
        try
        {
            string query = @"Select ct.id, certi.id as certificate_id, ct.certificate_no, ct.gauge_id, gt.gauge_name, cus.customer_name, ct.customer_id,
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
 Left Outer join certification_tb as certi 
 ON ct.id=certi.certification_data_id
 where ct.customer_id=" + Convert.ToInt32(Session["Customer_ID"]) + " Order by  ct.id DESC";
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
            childId = g.GetChildId("Certification.aspx");
            if (childId != 0)
            {
                stallauthority = g.GetAuthorityStatus(Convert.ToInt32(Session["Customer_ID"]), Convert.ToInt32(Session["User_ID"]), childId);
                string[] staustatus = stallauthority.Split(',');
                if (staustatus[1].ToString() == "True")
                {
                    for (int i = 0; i < grdCertificate.Rows.Count; i++)
                    {
                        Button btn = (Button)grdCertificate.Rows[i].FindControl("btnCerateCertificate");
                        //LinkButton lnkdlet = (LinkButton)grdTypeMaster.Rows[i].FindControl("lnkDelete");
                        //lnkdlet.Enabled = true;
                        string isCertificateDone = grdCertificate.Rows[i].Cells[6].Text;
                        if (isCertificateDone == "Yes")
                        {
                            btn.Enabled = true;
                            btn.Text = "Edit Certificate";
                        }
                        else
                        {
                            btn.Enabled = true;
                            btn.Text = "Create Certificate";

                        }

                        //Session["dleteGagePartLink"] = "YES";

                    }
                }
                else
                {
                    for (int i = 0; i < grdCertificate.Rows.Count; i++)
                    {
                        Button btn = (Button)grdCertificate.Rows[i].FindControl("btnCerateCertificate");
                        string isCertificateDone = grdCertificate.Rows[i].Cells[6].Text;
                        if (isCertificateDone == "Yes")
                        {
                            btn.Text = "Edit Certificate";
                            //lnk.Enabled = false;
                        }
                        else
                        {
                            btn.Text = "Create Certificate";

                        }
                        btn.Enabled = false;
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
    protected void grdCertificate_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCertificate.PageIndex = e.NewPageIndex;
        BindCertificateData();
    }
    protected void btnCerateCertificate_Click(object sender, EventArgs e)
    {
        try
        {
            txttotalerror.Text = "0.0";
            txtvisual.Text = "OK";
            txtfacesm.Text = "0.0";
            Button lnk = (Button)sender;
            string[] parameter = lnk.CommandArgument.ToString().Split(new char[] { ',' });
            lblCertificateId.Text = parameter[0];
            string certificateIdForEdit = parameter[1];
            lblcertificateIdForEdit.Text = parameter[1];
            int certIdForEdit = 0;
            //lblCertificateId.Text = lnk.CommandArgument;
            if (!String.IsNullOrEmpty(certificateIdForEdit))
            {
                certIdForEdit = Convert.ToInt32(certificateIdForEdit);
            }
            string query = @"Select ct.id, certi.id as certificate_id, certi.permissible_error_for, certi.description as certdescription, certi.standerd,
certi.total_error, certi.face_sm, certi.visual, certi.ext, certi.intval, certi.depth,
ct.certificate_no, ct.gauge_id, gt.gauge_name, gt.make, gt.go_tollerance_plus, gt.go_tollerance_minus, gt.no_go_tollerance_plus,
 gt.no_go_tollerance_minus,gt.go_were_limit, gt.size2, cus.customer_name, ct.customer_id,su.size_range as sizeRang, 
ct.identification_mark_by, (case when ct.is_certification_done='False' then 'No' else 'Yes' End) as CertificationDone,ct.calibration_carriedout,ct.calib_frequency,
ct.next_clib_date, ct.frequency_type, ct.cretificate_date,ct.refrence_dc_no,ct.receipt_date,ct.calibration_date,ct.identification_mark_by,ct.refrence_dc_date,
gt.gauge_sr_no, gt.gauge_Manufature_Id, gt.size_range, tm.type_of_gauge, cgr.satifactory as conditionatreceipt, su.size_range as gaugerange,
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
 Left Outer join certification_tb as certi 
 ON ct.id=certi.certification_data_id
 Where ct.id=" + Convert.ToInt32(lblCertificateId.Text) + " ";
            txtMasterEquipmentUsed.Text = "";
            DataTable dtFetchRecord = g.ReturnData(query);
            if (dtFetchRecord.Rows.Count > 0)
            {
                #region Fettch Record
                txtGauge.Text = dtFetchRecord.Rows[0]["gauge_name"].ToString();
                txtCalibCarriedOutat.Text = dtFetchRecord.Rows[0]["calibration_carriedout"].ToString();
                txtCalibrationFrequency.Text = dtFetchRecord.Rows[0]["calib_frequency"].ToString();
                txtCalibrationMethodNumber.Text = dtFetchRecord.Rows[0]["calibmethod"].ToString();
                txtConditionOfReceipt.Text = dtFetchRecord.Rows[0]["conditionatreceipt"].ToString();
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
                txtFrequencyType.Text = dtFetchRecord.Rows[0]["frequency_type"].ToString();

                lblPermissableErrorFor.Text = dtFetchRecord.Rows[0]["permissible_error_for"].ToString();
                txtstanderd.Text = dtFetchRecord.Rows[0]["standerd"].ToString();
                txtdesc.Text = dtFetchRecord.Rows[0]["certdescription"].ToString();
                txtfacesm.Text = dtFetchRecord.Rows[0]["face_sm"].ToString();
                txttotalerror.Text = dtFetchRecord.Rows[0]["total_error"].ToString();
                txtvisual.Text = dtFetchRecord.Rows[0]["visual"].ToString();
                txtExt.Text = dtFetchRecord.Rows[0]["ext"].ToString();
                txtDepth.Text = dtFetchRecord.Rows[0]["depth"].ToString();
                txtInt.Text = dtFetchRecord.Rows[0]["intval"].ToString();
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
                string typeofGauge = dtFetchRecord.Rows[0]["type_of_gauge"].ToString();
                string sizeRange = dtFetchRecord.Rows[0]["gaugerange"].ToString();
                ViewState["dtNominalSize"] = null;
                if (typeofGauge == "Micrometer")
                {
                    displayMicrometer(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = true;
                    grdVernier.Visible = false;
                    divstanderd.Visible = true;
                    divVernier.Visible = false;
                    grdLeverBore.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    //grdPressureReverse.Visible = false;
                    tblforAttribute.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    tblThredPlugGauge.Visible = false;
                }
                else if (typeofGauge == "Vernier")
                {
                    displayVernier(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = true;
                    divstanderd.Visible = false;
                    grdLeverBore.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    //grdPressureReverse.Visible = false;
                    tblforAttribute.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    tblThredPlugGauge.Visible = false;
                    divVernier.Visible = true;
                }
                else if (typeofGauge == "Plunger")
                {
                    displayPlunger(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = false;
                    divstanderd.Visible = false;
                    divVernier.Visible = false;
                    grdLeverBore.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    //grdPressureReverse.Visible = false;
                    tblforAttribute.Visible = false;
                    tblThredPlugGauge.Visible = false;
                    grdPlunger1.Visible = true;
                    grdPlunger2.Visible = true;
                }
                else if (typeofGauge == "Lever")
                {
                    displayLeverBore(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = false;
                    divstanderd.Visible = false;
                    divVernier.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    //grdPressureReverse.Visible = false;
                    tblforAttribute.Visible = false;
                    tblThredPlugGauge.Visible = false;
                    grdLeverBore.Visible = true;
                }
                else if (typeofGauge == "Feeler")
                {
                    displayFeeler(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = false;
                    divstanderd.Visible = false;
                    divVernier.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    //grdPressureReverse.Visible = false;
                    tblforAttribute.Visible = false;
                    grdLeverBore.Visible = false;
                    tblThredPlugGauge.Visible = false;
                    grdFeeler.Visible = true;
                }
                else if (typeofGauge == "Pressure")
                {
                    displayPressure(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = false;
                    divstanderd.Visible = false;
                    divVernier.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    grdLeverBore.Visible = false;
                    grdFeeler.Visible = false;
                    tblforAttribute.Visible = false;
                    tblThredPlugGauge.Visible = false;
                    grdPressureFowrding.Visible = true;
                }
                else if (typeofGauge == "For Attribute")
                {
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = false;
                    divstanderd.Visible = false;
                    divVernier.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    grdLeverBore.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    tblThredPlugGauge.Visible = false;
                    if (certIdForEdit != 0)
                    {
                        string strQuery = @"SELECT id, make, lowersize, highersize, gominus, goplus, nogominus, nogoplus, werelimit, observedgo, observednogo FROM attribute_result_tb
                             where certification_id=" + certIdForEdit + " order by id ASC ";
                        DataTable dtFetchExistAttribute = g.ReturnData(strQuery);
                        if (dtFetchExistAttribute.Rows.Count > 0)
                        {
                            lblmakeforAttribute.Text = dtFetchExistAttribute.Rows[0]["make"].ToString();
                            lblGOLowerSize.Text = dtFetchExistAttribute.Rows[0]["lowersize"].ToString();// Lower Size
                            lblNOGOHigherSize.Text = dtFetchExistAttribute.Rows[0]["highersize"].ToString();// Higher Size
                            lblGOMinus.Text = dtFetchExistAttribute.Rows[0]["gominus"].ToString();
                            lblGoPlus.Text = dtFetchExistAttribute.Rows[0]["goplus"].ToString();
                            lblNOGOMinus.Text = dtFetchExistAttribute.Rows[0]["nogominus"].ToString();
                            lblNOGOPlus.Text = dtFetchExistAttribute.Rows[0]["nogoplus"].ToString();
                            lblWereLimit.Text = dtFetchExistAttribute.Rows[0]["werelimit"].ToString();
                            txtGoObservedSize.Text = dtFetchExistAttribute.Rows[0]["observedgo"].ToString();
                            txtNoGoObservedSize.Text = dtFetchExistAttribute.Rows[0]["observednogo"].ToString();
                        }
                        tblforAttribute.Visible = true;
                    }
                    else
                    {
                        lblmakeforAttribute.Text = dtFetchRecord.Rows[0]["make"].ToString();
                        lblGOLowerSize.Text = dtFetchRecord.Rows[0]["size_range"].ToString();// Lower Size
                        lblNOGOHigherSize.Text = dtFetchRecord.Rows[0]["size2"].ToString();// Higher Size
                        lblGOMinus.Text = dtFetchRecord.Rows[0]["go_tollerance_minus"].ToString();
                        lblGoPlus.Text = dtFetchRecord.Rows[0]["go_tollerance_plus"].ToString();
                        lblNOGOMinus.Text = dtFetchRecord.Rows[0]["no_go_tollerance_minus"].ToString();
                        lblNOGOPlus.Text = dtFetchRecord.Rows[0]["no_go_tollerance_plus"].ToString();
                        lblWereLimit.Text = dtFetchRecord.Rows[0]["go_were_limit"].ToString();
                        tblforAttribute.Visible = true;
                    }
                }
                else if (typeofGauge == "Thread Plug Gauge")
                {
                    tblThredPlugGauge.Visible = true;
                    displayVernier(typeofGauge, sizeRange, certIdForEdit);
                    grdCalibResult.Visible = false;
                    grdVernier.Visible = false;
                    divstanderd.Visible = false;
                    grdLeverBore.Visible = false;
                    grdFeeler.Visible = false;
                    grdPressureFowrding.Visible = false;
                    //grdPressureReverse.Visible = false;
                    tblforAttribute.Visible = false;
                    grdPlunger1.Visible = false;
                    grdPlunger2.Visible = false;
                    divVernier.Visible = false;
                    if (certIdForEdit != 0)
                    {
                        string strQuery = @"SELECT id, make, lowersize, highersize, gominus, goplus, nogominus, nogoplus, werelimit, observedgo, observednogo FROM attribute_result_tb
                             where certification_id=" + certIdForEdit + " order by id ASC ";
                        DataTable dtFetchExistAttribute = g.ReturnData(strQuery);
                        if (dtFetchExistAttribute.Rows.Count > 0)
                        {
                            lblmakeforTPG.Text = dtFetchExistAttribute.Rows[0]["make"].ToString();
                            lblGOLowerSizeTPG.Text = dtFetchExistAttribute.Rows[0]["lowersize"].ToString();// Lower Size
                            lblNOGOHigherSizeTPG.Text = dtFetchExistAttribute.Rows[0]["highersize"].ToString();// Higher Size
                            lblGOMinusTPG.Text = dtFetchExistAttribute.Rows[0]["gominus"].ToString();
                            lblGoPlusTPG.Text = dtFetchExistAttribute.Rows[0]["goplus"].ToString();
                            lblNOGOMinusTPG.Text = dtFetchExistAttribute.Rows[0]["nogominus"].ToString();
                            lblNOGOPlusTPG.Text = dtFetchExistAttribute.Rows[0]["nogoplus"].ToString();
                            lblWereLimitTPG.Text = dtFetchExistAttribute.Rows[0]["werelimit"].ToString();
                            txtGoObservedSizeTPG.Text = dtFetchExistAttribute.Rows[0]["observedgo"].ToString();
                            txtNoGoObservedSizeTPG.Text = dtFetchExistAttribute.Rows[0]["observednogo"].ToString();
                        }
                    }
                    else
                    {
                        lblmakeforTPG.Text = dtFetchRecord.Rows[0]["make"].ToString();
                        lblGOLowerSizeTPG.Text = dtFetchRecord.Rows[0]["size_range"].ToString();// Lower Size
                        lblNOGOHigherSizeTPG.Text = dtFetchRecord.Rows[0]["size2"].ToString();// Higher Size
                        lblGOMinusTPG.Text = dtFetchRecord.Rows[0]["go_tollerance_minus"].ToString();
                        lblGoPlusTPG.Text = dtFetchRecord.Rows[0]["go_tollerance_plus"].ToString();
                        lblNOGOMinusTPG.Text = dtFetchRecord.Rows[0]["no_go_tollerance_minus"].ToString();
                        lblNOGOPlusTPG.Text = dtFetchRecord.Rows[0]["no_go_tollerance_plus"].ToString();
                        lblWereLimitTPG.Text = dtFetchRecord.Rows[0]["go_were_limit"].ToString();
                    }
                }


                MultiView1.ActiveViewIndex = 1;
                #endregion
            }
            else
            {
                clearFields();
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Create On certificate: " + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    #region Display All Format
    private void displayVernier(string gaugeType, string sizeRange, int isEditId)
    {
        try
        {

            if (isEditId != 0)
            {
                string strQuery = @"SELECT id, nominal_size,exterrortopr1 as ExternalErrorTopR1 ,exterrortbotomr1 as ExternalErrorBottomR1, exterrortopr2 as ExternalErrorTopR2,  
                             exterrortbotomr2 as ExternalErrorBottomR2, exterroravgtop as ExternalAvgErrorTop , exterroravgbottom as ExternalAvgErrorBottom, 
                            interrortopr1 as InternalErrorTopR1, interrorbottmr1 as InternalErrorBottomR1, interrortopr2 as InternalErrorTopR2, interrorbottomr2 as InternalErrorBottomR2, 
                            interroravgtop as InternalAvgErrorTop, interrorbottom as InternalAvgErrorBottom ,
                            calculated_ex_error_top, calculated_ex_error_bottom, calculated_in_error_top, calculated_in_error_bottom FROM vernier_result_tb
                             where certification_id=" + isEditId + " order by id ASC ";
                DataTable dtFetchExistRecord = g.ReturnData(strQuery);
                grdVernier.DataSource = dtFetchExistRecord;
                grdVernier.DataBind();
                return;
            }

            DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + gaugeType + "' order by id asc");
            string perErrFor = "";
            string strnominalSize = "";
            int cntforperError = 0;
            if (dtgetNominalSize.Rows.Count > 0)
            {
                for (int j = 0; j < dtgetNominalSize.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize"] != null)
                    {
                        dtNominalSize = (DataTable)ViewState["dtNominalSize"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize.Columns.Add("nominal_size");
                        //External
                        DataColumn ExternalErrorTopR1 = dtNominalSize.Columns.Add("ExternalErrorTopR1");
                        DataColumn ExternalErrorBottomR1 = dtNominalSize.Columns.Add("ExternalErrorBottomR1");
                        DataColumn ExternalErrorTopR2 = dtNominalSize.Columns.Add("ExternalErrorTopR2");
                        DataColumn ExternalErrorBottomR2 = dtNominalSize.Columns.Add("ExternalErrorBottomR2");
                        DataColumn ExternalAvgErrorTop = dtNominalSize.Columns.Add("ExternalAvgErrorTop");
                        DataColumn ExternalAvgErrorBottom = dtNominalSize.Columns.Add("ExternalAvgErrorBottom");
                        //Internal
                        DataColumn InternalErrorTopR1 = dtNominalSize.Columns.Add("InternalErrorTopR1");
                        DataColumn InternalErrorBottomR1 = dtNominalSize.Columns.Add("InternalErrorBottomR1");
                        DataColumn InternalErrorTopR2 = dtNominalSize.Columns.Add("InternalErrorTopR2");
                        DataColumn InternalErrorBottomR2 = dtNominalSize.Columns.Add("InternalErrorBottomR2");
                        DataColumn InternalAvgErrorTop = dtNominalSize.Columns.Add("InternalAvgErrorTop");
                        DataColumn InternalAvgErrorBottom = dtNominalSize.Columns.Add("InternalAvgErrorBottom");

                        DataColumn CalExErrorTopR1 = dtNominalSize.Columns.Add("calculated_ex_error_top");
                        DataColumn CalExErrorBottomR1 = dtNominalSize.Columns.Add("calculated_ex_error_bottom");
                        DataColumn CalInAvgErrorTop2 = dtNominalSize.Columns.Add("calculated_in_error_top");
                        DataColumn CalInAvgErrorBottom2 = dtNominalSize.Columns.Add("calculated_in_error_bottom");
                    }
                    DataRow dr = dtNominalSize.NewRow();
                    strnominalSize = dtgetNominalSize.Rows[j]["size"].ToString();
                    dr[0] = strnominalSize;
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = "";
                    dr[5] = "";
                    dr[6] = "";
                    dr[7] = "";
                    dr[8] = "";
                    dr[9] = "";
                    dr[10] = "";
                    dr[11] = "";
                    dr[12] = "";
                    dr[13] = "";
                    dr[14] = "";
                    dr[15] = "";


                    dtNominalSize.Rows.Add(dr);
                    ViewState["dtNominalSize"] = dtNominalSize;
                    grdVernier.DataSource = dtNominalSize;
                    grdVernier.DataBind();
                    if (cntforperError == 0)
                    {
                        perErrFor = "0";
                        cntforperError++;
                    }
                }
                perErrFor = perErrFor + "-" + strnominalSize;
                lblPermissableErrorFor.Text = perErrFor;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("On Fetch Vernier Data in Certificate Form" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void displayMicrometer(string gaugeType, string sizeRange, int isEditId)
    {
        try
        {
            if (isEditId != 0)
            {
                DataTable dtFetchExistRecord = g.ReturnData("SELECT id, nominal_size,r1,r2, r3, observed_mm as obsereved, error_mm as error FROM calibration_results_tb where certification_id=" + isEditId + " order by id ASC ");
                grdCalibResult.DataSource = dtFetchExistRecord;
                grdCalibResult.DataBind();
                return;
            }

            DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + gaugeType + "' order by id asc");
            string perErrFor = "";
            string strnominalSize = "";
            int cntforperError = 0;
            if (dtgetNominalSize.Rows.Count > 0)
            {
                for (int j = 0; j < dtgetNominalSize.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize"] != null)
                    {
                        dtNominalSize = (DataTable)ViewState["dtNominalSize"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize.Columns.Add("nominal_size");
                        DataColumn r1 = dtNominalSize.Columns.Add("r1");
                        DataColumn r2 = dtNominalSize.Columns.Add("r2");
                        DataColumn r3 = dtNominalSize.Columns.Add("r3");
                        DataColumn obsereved = dtNominalSize.Columns.Add("obsereved");
                        DataColumn error = dtNominalSize.Columns.Add("error");
                    }
                    DataRow dr = dtNominalSize.NewRow();
                    strnominalSize = dtgetNominalSize.Rows[j]["size"].ToString();
                    dr[0] = strnominalSize;
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = "";
                    dr[5] = "";
                    dtNominalSize.Rows.Add(dr);
                    ViewState["dtNominalSize"] = dtNominalSize;
                    grdCalibResult.DataSource = dtNominalSize;
                    grdCalibResult.DataBind();
                    if (cntforperError == 0)
                    {
                        perErrFor = "0";
                        cntforperError++;
                    }
                }
                perErrFor = perErrFor + "-" + strnominalSize;
                lblPermissableErrorFor.Text = perErrFor;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("On Fetch Micrometer Data in Certificate Form" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void displayPlunger(string gaugeType, string sizeRange, int isEditId)
    {
        try
        {
            if (isEditId != 0)
            {
                DataTable dtFetchExistRecord1 = g.ReturnData("SELECT id, nominal_size, r1 as PlungerR1,r2 as PlungerR2, observed_mm as PlungerMeanReding, error_mm as PlungerError FROM calibration_results_tb where certification_id=" + isEditId + " and nominal_size < 190 order by id ASC ");
                grdPlunger1.DataSource = dtFetchExistRecord1;
                grdPlunger1.DataBind();

                DataTable dtFetchExistRecord2 = g.ReturnData("SELECT id, nominal_size, r1 as PlungerR1,r2 as PlungerR2, observed_mm as PlungerMeanReding, error_mm as PlungerError FROM calibration_results_tb where certification_id=" + isEditId + " and nominal_size > 180 order by id ASC ");
                grdPlunger2.DataSource = dtFetchExistRecord2;
                grdPlunger2.DataBind();


                return;
            }
            DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + gaugeType + "' order by id asc");
            string perErrFor = "";
            string strnominalSize = "";
            int cntforperError = 0;
            if (dtgetNominalSize.Rows.Count > 0)
            {
                int nominalSize = 0;
                ViewState["dtGetResultPlunger2Grd"] = null;
                for (int j = 0; j < dtgetNominalSize.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize"] != null)
                    {
                        dtNominalSize = (DataTable)ViewState["dtNominalSize"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize.Columns.Add("nominal_size");
                        DataColumn PlungerR1 = dtNominalSize.Columns.Add("PlungerR1");
                        DataColumn PlungerR2 = dtNominalSize.Columns.Add("PlungerR2");
                        DataColumn PlungerMeanReding = dtNominalSize.Columns.Add("PlungerMeanReding");
                        DataColumn PlungerError = dtNominalSize.Columns.Add("PlungerError");
                    }

                    strnominalSize = dtgetNominalSize.Rows[j]["size"].ToString();
                    nominalSize = Convert.ToInt32(strnominalSize);

                    if (nominalSize > 180)
                    {

                        if (ViewState["dtGetResultPlunger2Grd"] != null)
                        {
                            dtGetResultPlunger2Grd = (DataTable)ViewState["dtGetResultPlunger2Grd"];
                        }
                        else
                        {
                            DataColumn nominal_size = dtGetResultPlunger2Grd.Columns.Add("nominal_size");
                            DataColumn PlungerR1 = dtGetResultPlunger2Grd.Columns.Add("PlungerR1");
                            DataColumn PlungerR2 = dtGetResultPlunger2Grd.Columns.Add("PlungerR2");
                            DataColumn PlungerMeanReding = dtGetResultPlunger2Grd.Columns.Add("PlungerMeanReding");
                            DataColumn PlungerError = dtGetResultPlunger2Grd.Columns.Add("PlungerError");
                        }
                        DataRow dr2 = dtGetResultPlunger2Grd.NewRow();
                        dr2[0] = strnominalSize;
                        dr2[1] = "";
                        dr2[2] = "";
                        dr2[3] = "";
                        dr2[4] = "";
                        dtGetResultPlunger2Grd.Rows.Add(dr2);
                        ViewState["dtGetResultPlunger2Grd"] = dtGetResultPlunger2Grd;
                        grdPlunger2.DataSource = dtGetResultPlunger2Grd;
                        grdPlunger2.DataBind();
                    }
                    else
                    {
                        DataRow dr = dtNominalSize.NewRow();
                        dr[0] = strnominalSize;
                        dr[1] = "";
                        dr[2] = "";
                        dr[3] = "";
                        dr[4] = "";
                        dtNominalSize.Rows.Add(dr);
                        ViewState["dtNominalSize"] = dtNominalSize;
                        grdPlunger1.DataSource = dtNominalSize;
                        grdPlunger1.DataBind();

                    }
                    if (cntforperError == 0)
                    {
                        perErrFor = "0";
                        cntforperError++;
                    }
                }
                perErrFor = perErrFor + "-" + strnominalSize;
                lblPermissableErrorFor.Text = perErrFor;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("On Fetch Micrometer Data in Certificate Form" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void displayLeverBore(string typeofGauge, string sizeRange, int isEditId)
    {
        try
        {
            if (isEditId != 0)
            {
                DataTable dtFetchExistRecord = g.ReturnData("SELECT id, nominal_size,upr1 as UpErrorpR1, upr2 as UpErrorpR2, downr1 as DownErrorR1, downr2 as DownErrorR2, meanreadingup as Upward, meanreadingdown as Downward, error_up, error_down FROM borelever_result_tb where certification_id=" + isEditId + " order by id ASC ");
                grdLeverBore.DataSource = dtFetchExistRecord;
                grdLeverBore.DataBind();
                return;
            }

            DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + typeofGauge + "' order by id asc");
            string perErrFor = "";
            string strnominalSize = "";
            int cntforperError = 0;
            if (dtgetNominalSize.Rows.Count > 0)
            {
                for (int j = 0; j < dtgetNominalSize.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize"] != null)
                    {
                        dtNominalSize = (DataTable)ViewState["dtNominalSize"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize.Columns.Add("nominal_size");
                        DataColumn UpErrorpR1 = dtNominalSize.Columns.Add("UpErrorpR1");
                        DataColumn UpErrorpR2 = dtNominalSize.Columns.Add("UpErrorpR2");
                        DataColumn DownErrorR1 = dtNominalSize.Columns.Add("DownErrorR1");
                        DataColumn DownErrorR2 = dtNominalSize.Columns.Add("DownErrorR2");
                        DataColumn Upward = dtNominalSize.Columns.Add("Upward");
                        DataColumn Downward = dtNominalSize.Columns.Add("Downward");
                        DataColumn ErrorUp = dtNominalSize.Columns.Add("error_up");
                        DataColumn ErrorDown = dtNominalSize.Columns.Add("error_down");
                    }
                    DataRow dr = dtNominalSize.NewRow();
                    strnominalSize = dtgetNominalSize.Rows[j]["size"].ToString();
                    dr[0] = strnominalSize;
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = "";
                    dr[5] = "";
                    dr[5] = "";
                    dr[6] = "";
                    dr[7] = "";
                    dtNominalSize.Rows.Add(dr);
                    ViewState["dtNominalSize"] = dtNominalSize;
                    grdLeverBore.DataSource = dtNominalSize;
                    grdLeverBore.DataBind();
                    if (cntforperError == 0)
                    {
                        perErrFor = "0";
                        cntforperError++;
                    }
                }
                perErrFor = perErrFor + "-" + strnominalSize;
                lblPermissableErrorFor.Text = perErrFor;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("On Fetch Lever Bore Data in Certificate Form" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void displayFeeler(string typeofGauge, string sizeRange, int isEditId)
    {
        try
        {
            if (isEditId != 0)
            {
                DataTable dtFetchExistRecord = g.ReturnData("SELECT id, nominal_size, observed, variation FROM feeler_result_tb where certification_id=" + isEditId + " order by id ASC ");
                grdFeeler.DataSource = dtFetchExistRecord;
                grdFeeler.DataBind();
                return;
            }
            DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + typeofGauge + "' order by id asc");
            string perErrFor = "";
            string strnominalSize = "";
            int cntforperError = 0;
            if (dtgetNominalSize.Rows.Count > 0)
            {
                for (int j = 0; j < dtgetNominalSize.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize"] != null)
                    {
                        dtNominalSize = (DataTable)ViewState["dtNominalSize"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize.Columns.Add("nominal_size");
                        DataColumn observed = dtNominalSize.Columns.Add("observed");
                        DataColumn variation = dtNominalSize.Columns.Add("variation");
                    }
                    DataRow dr = dtNominalSize.NewRow();
                    strnominalSize = dtgetNominalSize.Rows[j]["size"].ToString();
                    dr[0] = strnominalSize;
                    dr[1] = "";
                    dr[2] = "";
                    dtNominalSize.Rows.Add(dr);
                    ViewState["dtNominalSize"] = dtNominalSize;
                    grdFeeler.DataSource = dtNominalSize;
                    grdFeeler.DataBind();
                    if (cntforperError == 0)
                    {
                        perErrFor = "0";
                        cntforperError++;
                    }
                }
                perErrFor = perErrFor + "-" + strnominalSize;
                lblPermissableErrorFor.Text = perErrFor;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("On Fetch Feeler Data in Certificate Form" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }
    private void displayPressure(string typeofGauge, string sizeRange, int isEditId)
    {
        try
        {
            if (isEditId != 0)
            {
                DataTable dtFetchExistRecord = g.ReturnData(@"SELECT id, nominal_size,forwardr1 as ForwardR1, forwardr2 as ForwardR2, forwardr3 as ForwardR3, 
                forwardmeanreading as ForwardMeanReding, forwarderror as ForwardError, reverser1 as ReverseR1, reverserr2 as ReverseR2, 
                reverserr3 as ReverseR3, reversermeanreading as ReverseMeanReding, reversererror as ReverseError FROM pressure_results_tb where certification_id=" + isEditId + " order by id ASC ");
                grdPressureFowrding.DataSource = dtFetchExistRecord;
                grdPressureFowrding.DataBind();
                return;
            }
            DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + typeofGauge + "' order by id asc");
            string perErrFor = "";
            string strnominalSize = "";
            int cntforperError = 0;
            if (dtgetNominalSize.Rows.Count > 0)
            {
                for (int j = 0; j < dtgetNominalSize.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize"] != null)
                    {
                        dtNominalSize = (DataTable)ViewState["dtNominalSize"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize.Columns.Add("nominal_size");
                        DataColumn ForwardR1 = dtNominalSize.Columns.Add("ForwardR1");
                        DataColumn ForwardR2 = dtNominalSize.Columns.Add("ForwardR2");
                        DataColumn ForwardR3 = dtNominalSize.Columns.Add("ForwardR3");
                        DataColumn ForwardMeanReding = dtNominalSize.Columns.Add("ForwardMeanReding");
                        DataColumn ForwardError = dtNominalSize.Columns.Add("ForwardError");
                        //Reversed
                        DataColumn ReverseR1 = dtNominalSize.Columns.Add("ReverseR1");
                        DataColumn ReverseR2 = dtNominalSize.Columns.Add("ReverseR2");
                        DataColumn ReverseR3 = dtNominalSize.Columns.Add("ReverseR3");
                        DataColumn ReverseMeanReding = dtNominalSize.Columns.Add("ReverseMeanReding");
                        DataColumn ReverseError = dtNominalSize.Columns.Add("ReverseError");


                    }
                    DataRow dr = dtNominalSize.NewRow();
                    strnominalSize = dtgetNominalSize.Rows[j]["size"].ToString();
                    dr[0] = strnominalSize;
                    dr[1] = "";
                    dr[2] = "";
                    dr[3] = "";
                    dr[4] = "";
                    dr[5] = "";
                    dr[6] = "";
                    dr[7] = "";
                    dr[8] = "";
                    dr[9] = "";
                    dr[10] = "";
                    dtNominalSize.Rows.Add(dr);
                    ViewState["dtNominalSize"] = dtNominalSize;
                    grdPressureFowrding.DataSource = dtNominalSize;
                    grdPressureFowrding.DataBind();
                    if (cntforperError == 0)
                    {
                        perErrFor = "0";
                        cntforperError++;
                    }
                }
                perErrFor = perErrFor + "-" + strnominalSize;
                lblPermissableErrorFor.Text = perErrFor;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("On Fetch Pressure Gauge Data in Certificate Form" + ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    #endregion

    protected void btnSaveCertificate_Click(object sender, EventArgs e)
    {
        try
        {
            int EditId = 0;
            if (lblcertificateIdForEdit.Text == "")
            {
                DataTable dtExistRecord = g.ReturnData("Select id from certification_tb where certification_data_id=" + Convert.ToInt32(lblCertificateId.Text) + "");
                if (dtExistRecord.Rows.Count > 0)
                {
                    g.ShowMessage(this.Page, "Certification is already done of this gauge.");
                    return;
                }
            }
            else
            {
                EditId = Convert.ToInt32(lblcertificateIdForEdit.Text);
            }
            string gaugeType = txtTypeOfgauge.Text;
            if (gaugeType == "Micrometer")
            {
                getMicrometerResult();
            }
            else if (gaugeType == "Vernier")
            {
                getVernierResult();
            }
            else if (gaugeType == "Plunger")
            {
                getPlungerResult();
            }
            else if (gaugeType == "Lever")
            {
                getLeverBoreResult();
            }
            else if (gaugeType == "Feeler")
            {
                getFeelerResult();
            }
            else if (gaugeType == "Pressure")
            {
                getPressureResult();
            }
            else if (gaugeType == "For Attribute")
            {
                getAttributeResult();
            }
            if (dtGetResult.Rows.Count == 0)
            {
                g.ShowMessage(this.Page, "Certification result is not found.");
                return;
            }
            string strCurentDatae = "";

            string query = "";
            if (EditId == 0)
            {
                query = ("Insert into certification_tb (total_error, face_sm, visual, certification_data_id, permissible_error_for, description, created_date, created_by_id, status,standerd, ext, intval, depth) values(?param1,?param2,?param3,?param4,?param5,?param6,?param7,?param8,?param9,?param10,?param11,?param12,?param13)");
            }
            else
            {
                query = ("update  certification_tb set total_error=?param1, face_sm=?param2, visual=?param3, certification_data_id=?param4, permissible_error_for=?param5, description=?param6, created_date=?param7, created_by_id=?param8, status=?param9,standerd=?param10, ext=?param11, intval=?param12, depth=?param13 where id=" + EditId + "");
            }

            DateTime date = DateTime.Now;
            strCurentDatae = date.ToString("yyyy-MM-dd H:mm:ss");
            long id = g.SaveCertificate(query, txttotalerror.Text, txtfacesm.Text, txtvisual.Text, Convert.ToInt32(lblCertificateId.Text), lblPermissableErrorFor.Text, txtdesc.Text, strCurentDatae, Convert.ToInt32(Session["User_ID"].ToString()), false, txtstanderd.Text, txtExt.Text, txtInt.Text, txtDepth.Text, dtGetResult, gaugeType, EditId);
            if (id != 0)
            {
                g.ShowMessage(this.Page, "Certificate is saved successfully.");
                BindCertificateData();
                MultiView1.ActiveViewIndex = 0;
                clearFields();
            }
            else
            {
                g.ShowMessage(this.Page, "Data is not saved. Some error founded.");
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex.Message);
            g.ShowMessage(this.Page, ex.Message);
        }
    }

    #region Get Results from iniput formats

    private void getVernierResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (grdVernier.Rows.Count > 0)
            {
                for (int j = 0; j < grdVernier.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataTable dt = new DataTable();
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");
                        //External
                        DataColumn ExternalErrorTopR1 = dtGetResult.Columns.Add("ExternalErrorTopR1");
                        DataColumn ExternalErrorBottomR1 = dtGetResult.Columns.Add("ExternalErrorBottomR1");
                        DataColumn ExternalErrorTopR2 = dtGetResult.Columns.Add("ExternalErrorTopR2");
                        DataColumn ExternalErrorBottomR2 = dtGetResult.Columns.Add("ExternalErrorBottomR2");
                        DataColumn ExternalAvgErrorTop = dtGetResult.Columns.Add("ExternalAvgErrorTop");
                        DataColumn ExternalAvgErrorBottom = dtGetResult.Columns.Add("ExternalAvgErrorBottom");
                        //Internal
                        DataColumn InternalErrorTopR1 = dtGetResult.Columns.Add("InternalErrorTopR1");
                        DataColumn InternalErrorBottomR1 = dtGetResult.Columns.Add("InternalErrorBottomR1");
                        DataColumn InternalErrorTopR2 = dtGetResult.Columns.Add("InternalErrorTopR2");
                        DataColumn InternalErrorBottomR2 = dtGetResult.Columns.Add("InternalErrorBottomR2");
                        DataColumn InternalAvgErrorTop = dtGetResult.Columns.Add("InternalAvgErrorTop");
                        DataColumn InternalAvgErrorBottom = dtGetResult.Columns.Add("InternalAvgErrorBottom");
                        //Calculate Error
                        DataColumn CalExErrorTopR1 = dtGetResult.Columns.Add("CalExErrorTopR1");
                        DataColumn CalExErrorBottomR1 = dtGetResult.Columns.Add("CalExErrorBottomR1");
                        DataColumn CalInAvgErrorTop2 = dtGetResult.Columns.Add("CalInAvgErrorTop2");
                        DataColumn CalInAvgErrorBottom2 = dtGetResult.Columns.Add("CalInAvgErrorBottom2");
                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox tnominalRangeVernier = (TextBox)grdVernier.Rows[j].FindControl("txtnominalsizeVernier");
                    TextBox tExternalTopR1 = (TextBox)grdVernier.Rows[j].FindControl("txtexternalErrorTopR1");
                    TextBox tExternalBottomR1 = (TextBox)grdVernier.Rows[j].FindControl("txtexternalErrorBottomR1");
                    TextBox tExternalTopR2 = (TextBox)grdVernier.Rows[j].FindControl("txtexternalErrorTopR2");
                    TextBox tExternalBottomR2 = (TextBox)grdVernier.Rows[j].FindControl("txtexternalErrorBottomR2");
                    TextBox tExternalEvgTop = (TextBox)grdVernier.Rows[j].FindControl("txtExternalAvgErrorTop");
                    TextBox tExternalEvgBottom = (TextBox)grdVernier.Rows[j].FindControl("txtExternalAvgErrorBottom");
                    //Internal
                    TextBox tInternalErrorTopR1 = (TextBox)grdVernier.Rows[j].FindControl("txtInternalErrorTopR1");
                    TextBox tInternalErrorBottomR1 = (TextBox)grdVernier.Rows[j].FindControl("txtInternalErrorBottomR1");
                    TextBox tInternalErrorTopR2 = (TextBox)grdVernier.Rows[j].FindControl("txtInternalErrorTopR2");
                    TextBox tInternalErrorBottomR2 = (TextBox)grdVernier.Rows[j].FindControl("txtInternalErrorBottomR2");
                    TextBox tInternalAvgErrorTop = (TextBox)grdVernier.Rows[j].FindControl("txtInternalAvgErrorTop");
                    TextBox tInternalAvgErrorBottom = (TextBox)grdVernier.Rows[j].FindControl("txtInternalAvgErrorBottom");
                    HiddenField hfErrorTop1 = (HiddenField)grdVernier.Rows[j].FindControl("hfExtError1Top");
                    HiddenField hfErrorBottom1 = (HiddenField)grdVernier.Rows[j].FindControl("hfExtError1Bottom");
                    HiddenField hfErrorTop2 = (HiddenField)grdVernier.Rows[j].FindControl("hfIntError1Top");
                    HiddenField hfErrorBottom2 = (HiddenField)grdVernier.Rows[j].FindControl("hfIntError1Bottom");
                    dr[0] = tnominalRangeVernier.Text;
                    dr[1] = tExternalTopR1.Text;
                    dr[2] = tExternalBottomR1.Text;
                    dr[3] = tExternalTopR2.Text;
                    dr[4] = tExternalBottomR2.Text;
                    dr[5] = tExternalEvgTop.Text;
                    dr[6] = tExternalEvgBottom.Text;
                    dr[7] = tInternalErrorTopR1.Text;
                    dr[8] = tInternalErrorBottomR1.Text;
                    dr[9] = tInternalErrorTopR2.Text;
                    dr[10] = tInternalErrorBottomR2.Text;
                    dr[11] = tInternalAvgErrorTop.Text;
                    dr[12] = tInternalAvgErrorBottom.Text;
                    dr[13] = hfErrorTop1.Value.ToString();
                    dr[14] = hfErrorBottom1.Value.ToString();
                    dr[15] = hfErrorTop2.Value.ToString();
                    dr[16] = hfErrorBottom2.Value.ToString();
                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("GetVernierResult on Certificate: " + ex.Message);
        }
    }

    private void getMicrometerResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (grdCalibResult.Rows.Count > 0)
            {
                for (int j = 0; j < grdCalibResult.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");
                        DataColumn r1 = dtGetResult.Columns.Add("r1");
                        DataColumn r2 = dtGetResult.Columns.Add("r2");
                        DataColumn r3 = dtGetResult.Columns.Add("r3");
                        DataColumn obsereved = dtGetResult.Columns.Add("obsereved");
                        DataColumn error = dtGetResult.Columns.Add("error");
                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox txtnominalRange = (TextBox)grdCalibResult.Rows[j].FindControl("txtnominalsize");
                    //string strnominalSize = grdCalibResult.Rows[j].Cells[0].Text;
                    TextBox txtr11 = (TextBox)grdCalibResult.Rows[j].FindControl("txtr1");
                    TextBox txt22 = (TextBox)grdCalibResult.Rows[j].FindControl("txtr2");
                    TextBox txt33 = (TextBox)grdCalibResult.Rows[j].FindControl("txtr3");
                    TextBox txtObser = (TextBox)grdCalibResult.Rows[j].FindControl("txtObserved");
                    TextBox txtErrors = (TextBox)grdCalibResult.Rows[j].FindControl("txtError");
                    dr[0] = txtnominalRange.Text;
                    dr[1] = txtr11.Text;
                    dr[2] = txt22.Text;
                    dr[3] = txt33.Text;
                    dr[4] = txtObser.Text;
                    dr[5] = txtErrors.Text;

                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("GetMicrometerResult on Certificate: " + ex.Message);
        }
    }

    private void getPlungerResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (grdPlunger1.Rows.Count > 0)
            {
                for (int j = 0; j < grdPlunger1.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");
                        DataColumn PlungerR1 = dtGetResult.Columns.Add("PlungerR1");
                        DataColumn PlungerR2 = dtGetResult.Columns.Add("PlungerR2");
                        DataColumn PlungerMeanReding = dtGetResult.Columns.Add("PlungerMeanReding");
                        DataColumn PlungerError = dtGetResult.Columns.Add("PlungerError");
                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox txtnominalRange = (TextBox)grdPlunger1.Rows[j].FindControl("txtnominalsizePlunger");
                    TextBox tPlungerR1 = (TextBox)grdPlunger1.Rows[j].FindControl("txtPlungerR1");
                    TextBox tPlungerR2 = (TextBox)grdPlunger1.Rows[j].FindControl("txtPlungerR2");
                    TextBox tMeanReading = (TextBox)grdPlunger1.Rows[j].FindControl("txtMeanReading");
                    TextBox terror = (TextBox)grdPlunger1.Rows[j].FindControl("txterror");
                    dr[0] = txtnominalRange.Text;
                    dr[1] = tPlungerR1.Text;
                    dr[2] = tPlungerR2.Text;
                    dr[3] = tMeanReading.Text;
                    dr[4] = terror.Text;
                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }

            if (grdPlunger2.Rows.Count > 0)
            {
                for (int j = 0; j < grdPlunger2.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");
                        DataColumn PlungerR1 = dtGetResult.Columns.Add("PlungerR1");
                        DataColumn PlungerR2 = dtGetResult.Columns.Add("PlungerR2");
                        DataColumn PlungerMeanReding = dtGetResult.Columns.Add("PlungerMeanReding");
                        DataColumn PlungerError = dtGetResult.Columns.Add("PlungerError");
                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox txtnominalRange = (TextBox)grdPlunger2.Rows[j].FindControl("txtnominalsizePlunger");
                    TextBox tPlungerR1 = (TextBox)grdPlunger2.Rows[j].FindControl("txtPlungerR1");
                    TextBox tPlungerR2 = (TextBox)grdPlunger2.Rows[j].FindControl("txtPlungerR2");
                    TextBox tMeanReading = (TextBox)grdPlunger2.Rows[j].FindControl("txtMeanReading");
                    TextBox terror = (TextBox)grdPlunger2.Rows[j].FindControl("txterror");
                    dr[0] = txtnominalRange.Text;
                    dr[1] = tPlungerR1.Text;
                    dr[2] = tPlungerR2.Text;
                    dr[3] = tMeanReading.Text;
                    dr[4] = terror.Text;
                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }

        }
        catch (Exception ex)
        {
            Logger.Error("GetMicrometerResult on Certificate: " + ex.Message);
        }
    }

    private void getLeverBoreResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (grdLeverBore.Rows.Count > 0)
            {
                for (int j = 0; j < grdLeverBore.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");

                        DataColumn UpErrorpR1 = dtGetResult.Columns.Add("UpErrorpR1");
                        DataColumn UpErrorpR2 = dtGetResult.Columns.Add("UpErrorpR2");
                        DataColumn DownErrorR1 = dtGetResult.Columns.Add("DownErrorR1");
                        DataColumn DownErrorR2 = dtGetResult.Columns.Add("DownErrorR2");

                        DataColumn Upward = dtGetResult.Columns.Add("Upward");
                        DataColumn Downward = dtGetResult.Columns.Add("Downward");
                        DataColumn UpwardError = dtGetResult.Columns.Add("UpwardError");
                        DataColumn DownwardError = dtGetResult.Columns.Add("DownwardError");

                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox txtnominalRange = (TextBox)grdLeverBore.Rows[j].FindControl("txtnominalsizeLeverBore");
                    TextBox txtr11 = (TextBox)grdLeverBore.Rows[j].FindControl("txtUpErrorpR1");
                    TextBox txt22 = (TextBox)grdLeverBore.Rows[j].FindControl("txtUpErrorpR2");
                    TextBox txt33 = (TextBox)grdLeverBore.Rows[j].FindControl("txtDownErrorR1");
                    TextBox txt44 = (TextBox)grdLeverBore.Rows[j].FindControl("txtDownErrorR2");
                    TextBox txtUp = (TextBox)grdLeverBore.Rows[j].FindControl("txtUpward");
                    TextBox txtdown = (TextBox)grdLeverBore.Rows[j].FindControl("txtDownward");
                    HiddenField hfUpErrors = (HiddenField)grdLeverBore.Rows[j].FindControl("hfUpError");
                    HiddenField hfDownErrors = (HiddenField)grdLeverBore.Rows[j].FindControl("hfDownError");
                    dr[0] = txtnominalRange.Text;
                    dr[1] = txtr11.Text;
                    dr[2] = txt22.Text;
                    dr[3] = txt33.Text;
                    dr[4] = txt44.Text;
                    dr[5] = txtUp.Text;
                    dr[6] = txtdown.Text;
                    dr[7] = hfUpErrors.Value;
                    dr[8] = hfDownErrors.Value;

                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("GetLeverBoreResult on Certificate: " + ex.Message);
        }
    }

    private void getFeelerResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (grdFeeler.Rows.Count > 0)
            {
                for (int j = 0; j < grdFeeler.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");
                        DataColumn observed = dtGetResult.Columns.Add("observed");
                        DataColumn variation = dtGetResult.Columns.Add("variation");


                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox txtnominalFeelerRange = (TextBox)grdFeeler.Rows[j].FindControl("txtnominalsizeFeeler");
                    TextBox txtObserveds = (TextBox)grdFeeler.Rows[j].FindControl("txtObserved");
                    TextBox txtVaritaions = (TextBox)grdFeeler.Rows[j].FindControl("txtvariation");

                    dr[0] = txtnominalFeelerRange.Text;
                    dr[1] = txtObserveds.Text;
                    dr[2] = txtVaritaions.Text;
                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("GetFeelerResult on Certificate: " + ex.Message);
        }
    }

    private void getPressureResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (grdPressureFowrding.Rows.Count > 0)
            {
                for (int j = 0; j < grdPressureFowrding.Rows.Count; j++)
                {
                    if (ViewState["dtGetResult"] != null)
                    {
                        dtGetResult = (DataTable)ViewState["dtGetResult"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtGetResult.Columns.Add("nominal_size");
                        DataColumn ForwardR1 = dtGetResult.Columns.Add("ForwardR1");
                        DataColumn ForwardR2 = dtGetResult.Columns.Add("ForwardR2");
                        DataColumn ForwardR3 = dtGetResult.Columns.Add("ForwardR3");
                        DataColumn ForwardMeanReding = dtGetResult.Columns.Add("ForwardMeanReding");
                        DataColumn ForwardError = dtGetResult.Columns.Add("ForwardError");
                        //Reversed
                        DataColumn ReverseR1 = dtGetResult.Columns.Add("ReverseR1");
                        DataColumn ReverseR2 = dtGetResult.Columns.Add("ReverseR2");
                        DataColumn ReverseR3 = dtGetResult.Columns.Add("ReverseR3");
                        DataColumn ReverseMeanReding = dtGetResult.Columns.Add("ReverseMeanReding");
                        DataColumn ReverseError = dtGetResult.Columns.Add("ReverseError");


                    }
                    DataRow dr = dtGetResult.NewRow();
                    TextBox txtnominalRange = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtnominalsizeForward");
                    TextBox tForwardR1 = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtForwardR1");
                    TextBox tForwardR2 = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtForwardR2");
                    TextBox tForwardR3 = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtForwardR3");
                    TextBox tForwardMeanReading = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtForwardMeanReading");
                    TextBox tForwarderror = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtForwardError");

                    TextBox tReverseR1 = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtReverseR1");
                    TextBox tReverseR2 = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtReverseR2");
                    TextBox tReverseR3 = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtReverseR3");
                    TextBox tReverseMeanReading = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtReverseMeanReading");
                    TextBox tReverseerror = (TextBox)grdPressureFowrding.Rows[j].FindControl("txtReverseError");


                    dr[0] = txtnominalRange.Text;
                    dr[1] = tForwardR1.Text;
                    dr[2] = tForwardR2.Text;
                    dr[3] = tForwardR3.Text;
                    dr[4] = tForwardMeanReading.Text;
                    dr[5] = tForwarderror.Text;
                    //Reverse
                    dr[6] = tReverseR1.Text;
                    dr[7] = tReverseR2.Text;
                    dr[8] = tReverseR3.Text;
                    dr[9] = tReverseMeanReading.Text;
                    dr[10] = tReverseerror.Text;
                    dtGetResult.Rows.Add(dr);
                    ViewState["dtGetResult"] = dtGetResult;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("GetPressureResult on Certificate: " + ex.Message);
        }
    }

    private void getAttributeResult()
    {
        try
        {
            ViewState["dtGetResult"] = null;
            if (ViewState["dtGetResult"] != null)
            {
                dtGetResult = (DataTable)ViewState["dtGetResult"];
            }
            else
            {
                DataColumn make = dtGetResult.Columns.Add("make");
                DataColumn lowersize = dtGetResult.Columns.Add("lowersize");
                DataColumn highersize = dtGetResult.Columns.Add("highersize");
                DataColumn gominus = dtGetResult.Columns.Add("gominus");
                DataColumn goplus = dtGetResult.Columns.Add("goplus");
                DataColumn nogominus = dtGetResult.Columns.Add("nogominus");
                DataColumn nogoplus = dtGetResult.Columns.Add("nogoplus");
                DataColumn werelimit = dtGetResult.Columns.Add("werelimit");
                DataColumn observedgo = dtGetResult.Columns.Add("observedgo");
                DataColumn observednogo = dtGetResult.Columns.Add("observednogo");
            }
            DataRow dr = dtGetResult.NewRow();
            dr[0] = lblmakeforAttribute.Text;
            dr[1] = lblGOLowerSize.Text;
            dr[2] = lblNOGOHigherSize.Text;
            dr[3] = lblGOMinus.Text;
            dr[4] = lblGoPlus.Text;
            dr[5] = lblNOGOMinus.Text;
            dr[6] = lblNOGOPlus.Text;
            dr[7] = lblWereLimit.Text;
            dr[8] = txtGoObservedSize.Text;
            dr[9] = txtNoGoObservedSize.Text;
            dtGetResult.Rows.Add(dr);
            ViewState["dtGetResult"] = dtGetResult;
        }
        catch (Exception ex)
        {
            Logger.Error("Get Attribute on Certificate: " + ex.Message);
        }
    }
    #endregion

    protected void btnClose_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
        clearFields();
    }
    private void clearFields()
    {
        try
        {
            txtDateOfCalib.Text = txtCalibCarriedOutat.Text = txtCalibrationMethodNumber.Text = txtConditionOfReceipt.Text = txtDateOfReceipt.Text =
            txtdentificationMarkedByRML.Text = txtIdentification.Text = txtIsrefGuideLine.Text = txtMake.Text = txtMasterEquipmentUsed.Text =
            txtMfgNo.Text = txtRefrenceDate.Text = txtRefrenceDcno.Text = txtSize.Text = txtTestPurpose.Text = txtTypeOfgauge.Text = txtuncertintiy.Text =
            txtNextCalibDate.Text = txtCalibrationFrequency.Text = txtFrequencyType.Text = lblCertificateId.Text =
            txtNoGoObservedSize.Text = txtGoObservedSize.Text = lblmakeforAttribute.Text = lblGOMinus.Text = lblGoPlus.Text = lblWereLimit.Text =
            lblNOGOMinus.Text = lblNOGOPlus.Text = lblGOLowerSize.Text = lblNOGOHigherSize.Text = txtstanderd.Text = txtdesc.Text = txtExt.Text =
            txtfacesm.Text = txtDepth.Text = txtvisual.Text = txttotalerror.Text = string.Empty;
            ViewState["dtNominalSize"] = null;
            ViewState["dtGetResult"] = null;
            ViewState["dtGetResultPlunger2Grd"] = null;
            grdVernier.DataSource = null;
            grdVernier.DataBind();
            grdCalibResult.DataSource = null;
            grdCalibResult.DataBind();
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void grdVernier_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "External Measurements";
                HeaderCell.ColumnSpan = 7;
                HeaderCell.Style.Add("Font-Size", "Larger");
                HeaderCell.Style.Add("font-weight", "bold");
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Internal Measurements";
                HeaderCell.ColumnSpan = 6;
                HeaderCell.Style.Add("Font-Size", "Larger");
                HeaderCell.Style.Add("font-weight", "bold");
                HeaderGridRow.Cells.Add(HeaderCell);

                grdVernier.Controls[0].Controls.AddAt(0, HeaderGridRow);


                GridViewRow HeaderGridRow1 = new GridViewRow(0, 1, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Specified";
                //HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Error R1";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Error R2";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Avg. Error";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Error R1";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Error R2";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Avg. Error";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);
                HeaderGridRow.Attributes.Add("class", "header");
                HeaderGridRow1.Attributes.Add("class", "header");

                grdVernier.Controls[0].Controls.AddAt(1, HeaderGridRow1);

            }
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void grdLeverBore_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.Header)
            {
                //GridView HeaderGrid = (GridView)sender;
                //GridViewRow HeaderGridRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //TableCell HeaderCell = new TableCell();
                //HeaderCell.Text = "External Measurements";
                //HeaderCell.ColumnSpan = 7;
                //HeaderCell.Style.Add("Font-Size", "Larger");
                //HeaderCell.Style.Add("font-weight", "bold");
                //HeaderGridRow.Cells.Add(HeaderCell);

                //HeaderCell = new TableCell();
                //HeaderCell.Text = "Internal Measurements";
                //HeaderCell.ColumnSpan = 6;
                //HeaderCell.Style.Add("Font-Size", "Larger");
                //HeaderCell.Style.Add("font-weight", "bold");
                //HeaderGridRow.Cells.Add(HeaderCell);

                //grdVernier.Controls[0].Controls.AddAt(0, HeaderGridRow);


                GridViewRow HeaderGridRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Nominal Size(mm/inch)";
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Upward";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Downward";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);

                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "Mean Reading";
                HeaderCell1.ColumnSpan = 2;
                HeaderCell1.Style.Add("font-weight", "bold");
                HeaderGridRow1.Cells.Add(HeaderCell1);
                HeaderGridRow1.Attributes.Add("class", "header");
                grdLeverBore.Controls[0].Controls.AddAt(0, HeaderGridRow1);

            }
        }
        catch (Exception)
        {
            throw;
        }

    }
}