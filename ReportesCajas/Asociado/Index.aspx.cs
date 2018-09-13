using Newtonsoft.Json;
using System.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using System.Reflection;


public partial class _Default : System.Web.UI.Page 
{
    public string Datax = "";
    public string Semana = "";
    public string Variety = "";
    public string Cajas = "";
    DataTable dtx;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        {
            Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
            parameters2.Add("@funcion", 4);
            DataAccess daccess = new DataAccess();
            DataTable dt5 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parameters2);
            DropDrownPlanta.DataSource = dt5;
            DropDrownPlanta.DataTextField = "Name";
            DropDrownPlanta.DataValueField = "farm";
            DropDrownPlanta.DataBind();

            fecha.Text = DateTime.Today.ToString("yyyy-MM-dd");

        }////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
       
           // DropDownListInvernadero.SelectedValue =
                string x = (string)Session["invernadero"];
       
      
    }
    protected void DropDrownPlanta_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion", 5);
        parameters2.Add("@idFarm", DropDrownPlanta.SelectedValue);
        parameters2.Add("@fecha", fecha.Text);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parameters2);
        
        DropDownListInvernadero.DataSource = dt3;
        DropDownListInvernadero.DataTextField = "Greenhouse";
        DropDownListInvernadero.DataValueField = "idGreenHouse";
        DropDownListInvernadero.DataBind();
     ////////////////////////////////////////////////////////////
        Dictionary<string, object> parametersx2 = new System.Collections.Generic.Dictionary<string, object>();
        parametersx2.Add("@funcion", 6);
        parametersx2.Add("@idgreenHouse", DropDownListInvernadero.SelectedValue);

        DataTable dtx3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parametersx2);
        DropDownListLider.DataSource = dtx3;
        DropDownListLider.DataTextField = "Lider";
        DropDownListLider.DataValueField = "idGreenHouse";
        DropDownListLider.DataBind();

       // TablaPivot.Src = "";
        SemanaLabel.Text = "";
        variedad.Text = "";
        tCajas.Text = "";
        Button1.Visible = false;
    }

     
    protected void Button2_Click(object sender, EventArgs e)
    {
        string currentDirName = System.IO.Directory.GetCurrentDirectory();
        //string dirFisico = "C:\\Users\\camaya\\Desktop\\Natural Sweet\\ReportesCajas\\Fuentes\\ReportesCajas\\Asociado\\";
        string dirFisico = "F:\\wmp\\WMP2\\ReportesCajas\\Asociado\\";
        var workbook = new XLWorkbook(dirFisico + "template.xlsx");
        var worksheet = workbook.Worksheets.First();//"Sheet 1");
        string lider = "";
        if (DropDownListLider.SelectedIndex<0)
        {
            lider = "";
        }
        else {
            lider = DropDownListLider.SelectedItem.Text;
        }
        worksheet.Cell(3, "J").Value = DropDrownPlanta.SelectedItem.Text;
        worksheet.Cell(3, "Q").Value = fecha.Text;
        worksheet.Cell(3, "Y").Value = DropDownListInvernadero.SelectedItem.Text;
        worksheet.Cell(5, "J").Value = lider;
        worksheet.Cell(5, "Q").Value = SemanaLabel.Text;
        worksheet.Cell(5, "Y").Value = variedad.Text;

        DataTable x = (DataTable)Session["data"];

        var distinctIds = x.AsEnumerable()
                   .Select(s => new
                   {
                       codigo = s.Field<int>("Codigo"),
                   })
                   .Distinct().ToList();   

        int MaxLavel = Convert.ToInt32(x.Compute("Max(RowAsociado)", string.Empty));
        int asociado = 0;
        foreach(var Codigo in distinctIds){
            int totales = 0;
            for (int row = 1; row <= MaxLavel; row++)
            {
                worksheet.Cell(12, 4+(row*2)).Value = row;
                int row2 = 0;

                
                worksheet.Column(5 + (row * 2)).Width = 6.43;
                worksheet.Column(6 + (row * 2)).Width = 6.43;
                worksheet.Range(12, 4 + (row * 2), 12, 5 + (row * 2)).Merge();
                worksheet.Range(12, 4 + (row * 2), 12, 5 + (row * 2)).Style.Border.SetTopBorder(XLBorderStyleValues.Medium);
                //worksheet.Range(12, 4 + (row * 2), 12, 5 + (row * 2)).Style.Border.RightBorder(XLBorderStyleValues.Medium);

                foreach (DataRow xx in x.Select("Codigo = '"+Codigo.codigo+"' and RowAsociado='"+row+"'"))
                {
                    totales = totales + int.Parse(xx["cajas"].ToString());
                     worksheet.Cell(14 + asociado, 4).Value = xx["Codigo"].ToString();
                     worksheet.Cell(14 + asociado, 5).Value = xx["Asociado"].ToString();
                     worksheet.Cell(14+asociado, 4+(row*2)).Value = xx["surco"].ToString();
                     worksheet.Cell(14+asociado, 5+(row*2)).Value = xx["cajas"].ToString();
                     
                     worksheet.Cell(13, 4 + (row * 2)).Value = "Pasillo";
                     worksheet.Cell(13, 5 + (row * 2)).Value = "Cajas";

                     row2++;
                }
            }
            asociado++;
            
            worksheet.Cell(13 + asociado, 3).Value = asociado;
            worksheet.Cell(13 + asociado, 6+(MaxLavel*2)).Value = totales;

            worksheet.Range(13, 6 + (MaxLavel * 2), 13, 6 + (MaxLavel * 2)).Merge();
            worksheet.Cell(13, 6 + (MaxLavel * 2)).Value = "Cajas Totales";

            worksheet.Cell(13 + asociado, 6 + (MaxLavel * 2)).Style.Fill.BackgroundColor = XLColor.Gray;
            worksheet.Cell(13 + asociado, 5 + (MaxLavel * 2)).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
            worksheet.Cell(13 + asociado, 6 + (MaxLavel * 2)).Style.Border.SetRightBorder(XLBorderStyleValues.Medium);
        }

        Response.Clear();
        Response.ContentType =
             "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.AddHeader("content-disposition", "attachment;filename=\"Asociado_Captura.xlsx\"");

        using (var memoryStream = new MemoryStream())
        {
            workbook.SaveAs(memoryStream);
            memoryStream.WriteTo(Response.OutputStream);
        }
        Response.End();
    }
   
    protected void DropDownListInvernadero_SelectedIndexChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion", 6);
        parameters2.Add("@idgreenHouse", DropDownListInvernadero.SelectedValue);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parameters2);
        DropDownListLider.DataSource = dt3;
        DropDownListLider.DataTextField = "Lider";
        DropDownListLider.DataValueField = "idGreenHouse";
        DropDownListLider.DataBind();

        Session["invernadero"] = DropDownListInvernadero.SelectedValue;
       // TablaPivot.Src = "";
        SemanaLabel.Text = "";
        variedad.Text = "";
        tCajas.Text = "";
        Button1.Visible = false;
    }

    protected void fecha_TextChanged(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
        parameters2.Add("@funcion", 5);
        parameters2.Add("@idFarm", DropDrownPlanta.SelectedValue);
        parameters2.Add("@fecha", fecha.Text);
        DataAccess daccess = new DataAccess();
        DataTable dt3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parameters2);
        DropDownListInvernadero.DataSource = dt3;
        DropDownListInvernadero.DataTextField = "Greenhouse";
        DropDownListInvernadero.DataValueField = "idGreenHouse";
        DropDownListInvernadero.DataBind();
        ////////////////////////////////////////////////////////////
        Dictionary<string, object> parametersx2 = new System.Collections.Generic.Dictionary<string, object>();
        parametersx2.Add("@funcion", 6);
        parametersx2.Add("@idgreenHouse", DropDownListInvernadero.SelectedValue);
       
        DataTable dtx3 = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parametersx2);
        DropDownListLider.DataSource = dtx3;
        DropDownListLider.DataTextField = "Lider";
        DropDownListLider.DataValueField = "idGreenHouse";
        DropDownListLider.DataBind();

        //TablaPivot.Src = "";
        SemanaLabel.Text = "";
        variedad.Text = "";
        tCajas.Text = "";
        Button1.Visible = false;
        
    }
    protected void Generar_Click(object sender, EventArgs e)
    {
        string fechaFolio = fecha.Text;
        if ( DropDownListInvernadero.SelectedIndex>= 0)
        {
            Dictionary<string, object> parameters3 = new System.Collections.Generic.Dictionary<string, object>();
            parameters3.Add("@funcion", 8);
            parameters3.Add("@fecha", fechaFolio);
            parameters3.Add("@idgreenHouse", DropDownListInvernadero.SelectedValue);

            DataAccess daccess = new DataAccess();
            DataTable dt = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parameters3);
            SemanaLabel.Text = dt.Rows[0]["WeekShort"].ToString();
            variedad.Text = dt.Rows[0]["Variety"].ToString();
            tCajas.Text = dt.Rows[0]["cajas"].ToString();


            //Session["fecha"] = fechaFolio;
            //Session["idgreenhouse"] = DropDownListInvernadero.SelectedValue;
            //string fecha = (string)Session["fecha"];
            //string idgreenHouse = (string)Session["idgreenhouse"];

            Dictionary<string, object> parameters2 = new System.Collections.Generic.Dictionary<string, object>();
            parameters2.Add("@funcion", 7);
            parameters2.Add("@fecha", fechaFolio);
            parameters2.Add("@idgreenHouse", DropDownListInvernadero.SelectedValue);

            
            dtx = daccess.executeStoreProcedureDataTable("spr_rpt_DevExtreme", parameters2);
            Datax = JsonConvert.SerializeObject(dtx);
            Session["data"] = dtx;

            Button1.Visible = false;
        }
         
        
    }
}
