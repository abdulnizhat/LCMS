﻿using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AttributeReportViewerBlank : System.Web.UI.Page
{
    Genreal g = new Genreal();
    DataTable dtMasterEqp = new DataTable();

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
                        string withdata = str[4].ToString();
                        DataTable dt2 = new DataTable();
                        DataTable dt1 = new DataTable();
                        DataSet ds1 = new DataSet();
                        DataSet ds2 = new DataSet();
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
                        string calibrationTbID = string.Empty;
                        ds1 = g.ReturnData1(strQueryGaugeDetails);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            calibrationTbID = ds1.Tables[0].Rows[0]["calitbid"].ToString();
                            gauge_name= ds1.Tables[0].Rows[0]["gauge_name"].ToString();
                            gauge_name = gauge_name.Replace(" ", "-");
                            gauge_name = gauge_name + "_CertID_" + certId+".pdf";
                            string masterEquip = ds1.Tables[0].Rows[0]["master_equipment"].ToString();
                            string[] strMasterEqpArray = masterEquip.Split(',');
                            for (int i = 0; i < strMasterEqpArray.Count(); i++)
                            {
                                int masterEquipId = Convert.ToInt32(strMasterEqpArray[i].ToString());
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
                           
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/AttributeReportBlank.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds1.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);

                            if (dtMasterEqp.Rows.Count > 0)
                            {
                                ReportDataSource mastereqp = new ReportDataSource("DataSet3", dtMasterEqp);
                                ReportViewer1.LocalReport.DataSources.Add(mastereqp);
                            }
                        }
                        if (withdata == "withdata")
                        {
                            ds2 = g.ReturnData1("Select  make, lowersize, highersize, gominus, goplus, nogominus, nogoplus, werelimit, observedgo, observednogo from attribute_result_tb where certification_id='" + calibrationTbID + "' order by id asc");
                        }
                        else
                        {
                            ds2 = g.ReturnData1("Select  make, size_range as lowersize, size2 as highersize, go_were_limit as werelimit, go_tollerance_minus as gominus, go_tollerance_plus as goplus, no_go_tollerance_minus as nogominus, no_go_tollerance_plus as nogoplus,  'observedgo', 'observednogo' from gaugemaster_tb where gauge_id='" + gaugeId + "' ");
                        }
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            ReportDataSource src2 = new ReportDataSource("DataSet2", ds2.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(src2);
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