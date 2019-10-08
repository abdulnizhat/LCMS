using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FeelerReportViewerBlank : System.Web.UI.Page
{
    Genreal g = new Genreal();
    DataTable dtMasterEqp = new DataTable();
    DataTable dtNominalSize = new DataTable();
     DataTable dtNominalSize2 = new DataTable();
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
                        DataSet ds4 = new DataSet();
                        ViewState["dtMasterEqp"] = null;
                        //dtMasterEqp = null;
                        ViewState["dtNominalSize"] = null;
                        //dtNominalSize = null;
                        string strQueryGaugeDetails = g.GetQueryForReportPart1ForBlank(Convert.ToInt32(certId));
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
                            ReportViewer1.LocalReport.ReportPath = MapPath("~/feelerReportBlank.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds1.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);
                            if (dtMasterEqp.Rows.Count > 0)
                            {
                                ReportDataSource mastereqp = new ReportDataSource("DataSet3", dtMasterEqp);
                                ReportViewer1.LocalReport.DataSources.Add(mastereqp);
                            }
                        }
                        // ds2 = g.ReturnData1("Select id, nominal_size,exterroravgtop, exterroravgbottom, interroravgtop, interrorbottom, calculated_ex_error_top, calculated_ex_error_bottom, calculated_in_error_top, calculated_in_error_bottom from vernier_result_tb where certification_id='" + certId + "' order by id asc");
                        //ds2 = g.ReturnData1("Select id, nominal_size, observed, variation from feeler_result_tb where certification_id='" + certId + "' and nominal_size < 0.110 order by id asc");
                        //if (ds2.Tables[0].Rows.Count > 0)
                        //{
                        //    ReportDataSource src2 = new ReportDataSource("DataSet2", ds2.Tables[0]);
                        //    ReportViewer1.LocalReport.DataSources.Add(src2);
                        //}
                        //ds4 = g.ReturnData1("Select id, nominal_size, observed, variation from feeler_result_tb where certification_id='" + certId + "' and nominal_size > 0.110 order by id asc");
                        //if (ds4.Tables[0].Rows.Count > 0)
                        //{
                        //    ReportDataSource src4 = new ReportDataSource("DataSet4", ds4.Tables[0]);
                        //    ReportViewer1.LocalReport.DataSources.Add(src4);
                        //}
                        DataTable dtgetNominalSize = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + gaugeType + "' and size < 0.110 order by id asc");
            string strnominalSize = "";
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
                }
            }
            if (dtNominalSize.Rows.Count > 0)
            {
                ReportDataSource src2 = new ReportDataSource("DataSet2", dtNominalSize);
                ReportViewer1.LocalReport.DataSources.Add(src2);
            }
            DataTable dtgetNominalSize2 = g.ReturnData("Select id, size, gauge_type from nominal_size_tb where nominal_size='" + sizeRange + "' and gauge_type='" + gaugeType + "' and size > 0.110  order by id asc");
            string strnominalSize2 = "";
            if (dtgetNominalSize2.Rows.Count > 0)
            {
                for (int j = 0; j < dtgetNominalSize2.Rows.Count; j++)
                {
                    if (ViewState["dtNominalSize2"] != null)
                    {
                        dtNominalSize2 = (DataTable)ViewState["dtNominalSize2"];
                    }
                    else
                    {
                        DataColumn nominal_size = dtNominalSize2.Columns.Add("nominal_size");
                        DataColumn observed = dtNominalSize2.Columns.Add("observed");
                        DataColumn variation = dtNominalSize2.Columns.Add("variation");
                    }
                    DataRow dr = dtNominalSize2.NewRow();
                    strnominalSize2 = dtgetNominalSize2.Rows[j]["size"].ToString();
                    dr[0] = strnominalSize2;
                    dr[1] = "";
                    dr[2] = "";
                    dtNominalSize2.Rows.Add(dr);
                    ViewState["dtNominalSize2"] = dtNominalSize2;
                }
            }
            if (dtNominalSize2.Rows.Count > 0)
            {
                ReportDataSource src4 = new ReportDataSource("DataSet4", dtNominalSize2);
                ReportViewer1.LocalReport.DataSources.Add(src4);
            }


                         //DataTable dtcust = new DataTable();
                        //dtcust = g.GetCustomerDetails(Convert.ToInt32(Session["Customer_ID"]));
                        //ReportDataSource repcust = new ReportDataSource("DataSetcust", dtcust);
                        //ReportViewer1.LocalReport.DataSources.Add(repcust);
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