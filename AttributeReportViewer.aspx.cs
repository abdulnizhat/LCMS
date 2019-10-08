using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AttributeReportViewer : System.Web.UI.Page
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
                        int certificateDataId = 0;
                        string certId = "0";
                        //string sizeRange = "";
                        //string gaugeType = "";
                        //int gaugeId = 0;
                        string getIds = Request.QueryString["queryStringVal"].ToString();
                        //if (getIds.Contains("notBlank"))
                        //{
                            string[] str = getIds.Split(',');
                            certificateDataId = Convert.ToInt32(str[0].ToString());
                            certId = str[1].ToString();
                            string isWithHeader = str[2].ToString();
                        //}
                        //else if (getIds.Contains("blank"))
                        //{
                        //    string[] str = getIds.Split(',');
                        //    gaugeId = Convert.ToInt32(str[0].ToString());
                        //    gaugeType = str[1].ToString();
                        //    sizeRange = str[2].ToString();
                        //    certId = str[3].ToString();
                        //}
                        //else
                        //{
                        //    string[] str = getIds.Split(',');
                        //    certificateDataId = Convert.ToInt32(str[0].ToString());
                        //    certId = str[1].ToString();
                        //}
                        DataTable dt2 = new DataTable();
                        DataTable dt1 = new DataTable();
                        DataSet ds1 = new DataSet();
                        DataSet ds2 = new DataSet();
                        ViewState["dtMasterEqp"] = null;
                        //dtMasterEqp = null;
                        ViewState["dtNominalSize"] = null;
                        //dtNominalSize = null;
                        string strQueryGaugeDetails = g.GetQueryForReportPart1(Convert.ToInt32(certId));
                        string gauge_name = "";
                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType = string.Empty;
                        string encoding = string.Empty;
                        string extension = string.Empty;
                        ds1 = g.ReturnData1(strQueryGaugeDetails);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            gauge_name= ds1.Tables[0].Rows[0]["gauge_name"].ToString();
                            gauge_name = gauge_name.Replace(" ", "-");
                            gauge_name = gauge_name + "_CertID_" + certId+".pdf";
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
                           
                            //ReportViewer1.Reset();
                            //ReportViewer1.LocalReport.Refresh();
                            ReportViewer1.ProcessingMode = ProcessingMode.Local;
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/AttributeReport.rdlc");
                            ReportDataSource rep = new ReportDataSource("DataSet1", ds1.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(rep);

                            if (dtMasterEqp.Rows.Count > 0)
                            {
                                ReportDataSource mastereqp = new ReportDataSource("DataSet3", dtMasterEqp);
                                ReportViewer1.LocalReport.DataSources.Add(mastereqp);
                            }
                        }
                        ds2 = g.ReturnData1("Select id, certification_id, make, lowersize, highersize, gominus, goplus, nogominus, nogoplus, werelimit, observedgo, observednogo from attribute_result_tb where certification_id='" + certId + "' order by id asc");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            ReportDataSource src2 = new ReportDataSource("DataSet2", ds2.Tables[0]);
                            ReportViewer1.LocalReport.DataSources.Add(src2);
                        }
                        ReportParameter rp = new ReportParameter("isWithHeader", isWithHeader);
                        ReportViewer1.LocalReport.SetParameters(rp);
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