using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CertificationDataBlankReportViewer : System.Web.UI.Page
{
    Genreal g = new Genreal();
    DataTable dtMasterEqp = new DataTable();
    DataTable dtNominalSize = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_ID"] != null && Session["Customer_ID"] != null)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["queryStringVal"] != null)
                    {
                        string getIds = Request.QueryString["queryStringVal"].ToString();
                        string[] str = getIds.Split(',');
                        int gaugeId = Convert.ToInt32(str[0].ToString());
                        string gaugeType = str[1].ToString();
                        string sizeRange = str[2].ToString();
                        string certId = str[3].ToString();
                        DataTable dt2 = new DataTable();
                        DataTable dt1 = new DataTable();
                        DataSet ds1 = new DataSet();
                        DataSet ds2 = new DataSet();
                        ViewState["dtMasterEqp"] = null;
                        //dtMasterEqp = null;
                        ViewState["dtNominalSize"] = null;
                        //dtNominalSize = null;
                        string strQueryGaugeDetails = @"SELECT ct.gauge_id,gt.gauge_name, gt.gauge_sr_no, gt.gauge_Manufature_Id,
                            gt.size_range, tm.type_of_gauge, cgr.satifactory as conditionatreceipt,
      tp.check_accuracy as testpurpose, rf.description as isrefrence, cm.description as calibmethod,  un.gauge_type as uncertinity,
 tm.master_equipment, ct.certificate_no, ct.identification_mark_by, ct.calibration_carriedout, cus.customer_name from certificate_data_tb as ct
 Left Outer join gaugemaster_tb as gt  ON ct.gauge_id=gt.gauge_id
 Left Outer join typemaster_tb as tm  ON gt.gauge_type_master_id=tm.id  
Left Outer join condition_gauge_receipt_tb as cgr  ON tm.condition_of_receipt=cgr.satifactory_id 
Left Outer join test_purpose_tb as tp  ON tm.test_purpose=tp.id  
Left Outer join calibration_methodno_tb as cm  ON tm.calibration_method=cm.id  
Left Outer join master_equipment_used_tb as meq  ON tm.master_equipment=meq.id  
Left Outer join subtypemaster_tb as su  ON tm.sub_type_id=su.id  
Left Outer join is_referance_guideline_tb as rf  ON tm.is_ref_guidline=rf.id  
Left Outer join uncertainty_measurement_tb as un  ON tm.uncertinity=un.id  
Left Outer join customer_tb as cus  ON ct.customer_id=cus.customer_id   
where ct.id=" + Convert.ToInt32(certId) + " ";
                        string gauge_name = "";
                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType = string.Empty;
                        string encoding = string.Empty;
                        string extension = string.Empty;
                        ds1 = g.ReturnData1(strQueryGaugeDetails);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            gauge_name = ds1.Tables[0].Rows[0]["gauge_name"].ToString();
                            gauge_name = gauge_name.Replace(" ", "-");
                            gauge_name = gauge_name + "_CertID_" + certId + ".pdf";
                            string masterEquip = ds1.Tables[0].Rows[0]["master_equipment"].ToString();
                            string[] strMasterEqpArray = masterEquip.Split(',');
                            for (int i = 0; i < strMasterEqpArray.Count(); i++)
                            {
                                int masterEquipId = Convert.ToInt32(strMasterEqpArray[i].ToString());
                                //DataTable dtMasterEquipMentUsed = g.ReturnData("Select description from master_equipment_used_tb where id=" + Convert.ToInt32(masterEquipId) + "");
                                DataTable dtMasterEquipMentUsed = g.GetMasterEquipmentDetails(masterEquipId);
                                if (dtMasterEquipMentUsed.Rows.Count > 0)
                                {
                                    if (ViewState["dtMasterEqp"] != null)
                                    {
                                        dtMasterEqp = (DataTable)ViewState["dtMasterEqp"];
                                    }
                                    else
                                    {
                                       DataColumn masterequipmentused = dtMasterEqp.Columns.Add("masterequipmentused");
                                    }
                                    DataRow dr = dtMasterEqp.NewRow();
                                    string strdesc = dtMasterEquipMentUsed.Rows[0]["description"].ToString();
                                    dr[0] = strdesc;
                                    dtMasterEqp.Rows.Add(dr);
                                    ViewState["dtMasterEqp"] = dtMasterEqp;
                                }
                            }
                            ReportViewer1.Reset();
                            ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/CertificateDataBlankReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds1.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);
                        }
                        if (dtMasterEqp.Rows.Count > 0)
                        {
                            ReportDataSource mastereqp = new ReportDataSource("DataSet2", dtMasterEqp);
                            ReportViewer1.LocalReport.DataSources.Add(mastereqp);
                        }
                        ds2 = g.ReturnData1("Select id, size from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + gaugeType + "' order by id asc");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
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
                                    DataColumn meanreading = dtNominalSize.Columns.Add("meanreading");
                                }
                                DataRow dr = dtNominalSize.NewRow();
                                string strnominalSize = ds2.Tables[0].Rows[j]["size"].ToString();
                                dr[0] = strnominalSize;
                                dr[1] = "";
                                dr[2] = "";
                                dr[3] = "";
                                dr[4] = "";
                                dtNominalSize.Rows.Add(dr);
                                ViewState["dtNominalSize"] = dtNominalSize;
                            }
                            ReportDataSource src2 = new ReportDataSource("DataSet3", dtNominalSize);
                            ReportViewer1.LocalReport.DataSources.Add(src2);
                        }
                        DataTable dtcust = new DataTable();
                        dtcust = g.GetCustomerDetails(Convert.ToInt32(Session["Customer_ID"]));
                        ReportDataSource repcust = new ReportDataSource("DataSetcust", dtcust);
                        ReportViewer1.LocalReport.DataSources.Add(repcust);
                        //Code For Download Direct PDF    
                        byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.    
                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = mimeType;
                        Response.AddHeader("content-disposition", "attachment; filename=" + gauge_name + "");
                        Response.BinaryWrite(bytes); // create the file    
                        Response.Flush();

                    }
                }
                catch (Exception ex)
                {
                    g.ShowMessage(this.Page, ex.Message);
                }
            }
        }

        else
        {
            Response.Redirect("Login.aspx");
        }
    }


}