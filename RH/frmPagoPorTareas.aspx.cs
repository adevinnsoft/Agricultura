using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Web.Script.Serialization;
using System.IO;
using System.Configuration;
using ClosedXML.Excel;

public partial class Reportes_frmPagoPorTareas : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            ObtieneRanchosPorUsuario(Session["idUsuario"].ToString());
            ObtieneInvernaderosPorUsuario(ddlRancho.SelectedValue, Session["idUsuario"].ToString());


        }
    }
  
    protected void btnConsulta_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();


        if (cbxCosecha.Checked)
        {
            parameters.Add("@fechaInicio", txtFechaInicio.Text);
            parameters.Add("@fechaFin", txtFechaFin.Text);
            parameters.Add("@idInvernadero", ddlInvernadero.SelectedValue);


            DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneCosechaPorEmpleado", parameters);
            GridView1.DataSource = result;
            GridView1.DataBind();
        }
        else
        {
            parameters.Add("@idPlanta", ddlRancho.SelectedValue);
            parameters.Add("@fechaInicio", txtFechaInicio.Text);
            parameters.Add("@fechaFin", txtFechaFin.Text);
            parameters.Add("@idInvernadero", ddlInvernadero.SelectedValue);
            parameters.Add("@PorCantidad", cbxCantidad.Checked);


            DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneActividadesApagar", parameters);
            GridView1.DataSource = result;
            GridView1.DataBind();
        }
        
    }
    public void ObtieneRanchosPorUsuario(string idUsuario)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idUsuario", idUsuario);

            ddlRancho.DataSource = dataaccess.executeStoreProcedureDataTableFill("spr_ObtenerPlantaPorUsuario", parameters);
            ddlRancho.DataTextField = "NombrePlanta";
            ddlRancho.DataValueField = "idPlanta";
            ddlRancho.DataBind();

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }
      public void ObtieneInvernaderosPorUsuario(string idRancho, string idUsuario)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@idPlanta", idRancho);
            parameters.Add("@idUsuario", idUsuario);

            ddlInvernadero.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtieneInvernaderosPorUsuario", parameters);
            ddlInvernadero.DataTextField = "GreenHouse";
            ddlInvernadero.DataValueField = "idGreenHouse";
            ddlInvernadero.DataBind();
        }
        catch (Exception es)
        {
            Log.Error(es.Message);
        }
    }

    
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");

            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");

            var ddl = e.Row.FindControl("ddlRazonRechazo") as DropDownList;

            if (ddl != null)
            {
                if (HttpContext.Current.Session["ddlRazonRechazoPago"] == null)
                {
                    ddl.DataSource = dataaccess.executeStoreProcedureDataTableFill("procObtieneRazonRechazosPagos", null);
                    ddl.DataTextField = "nombre";
                    ddl.DataValueField = "idRazonRechazo";
                  
                    ddl.DataBind(); 
                    Session["ddlRazonRechazoPago"] = ddl;
                }
                else
                {
                    ddl.DataSource = ((DropDownList)Session["ddlRazonRechazoPago"]).DataSource;
                    ddl.DataTextField = "nombre";
                    ddl.DataValueField = "idRazonRechazo";
                    ddl.DataBind(); ;
                }
            }
        }

    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {

    }
    protected void btnProcesar_Click(object sender, EventArgs e)
    {
        DataTable dtRegistrosApagar = dtRegistrisPagos();
        int contador = 1;
        for (int i = 0; i < GridView1.Rows.Count - 1; i++)
        {
            if (((CheckBox)GridView1.Rows[i].Cells[14].FindControl("CheckBox1")).Checked)
            {
                DataRow drP = dtRegistrosApagar.NewRow();
                drP["c_codigo_lug"] = "0";
                drP["d_fecha_cpn"] = Convert.ToDateTime(GridView1.Rows[i].Cells[5].Text).ToString("MM/dd/yyyy");
                drP["c_terminal_hoj"] = "1";
                drP["c_codigo_emp"] = ((LinkButton)GridView1.Rows[i].Cells[0].FindControl("lnkEmpleado")).Text;
                drP["c_codigo_lot"] = GridView1.Rows[i].Cells[4].Text;
                drP["c_codigo_tab"] = ((HiddenField)GridView1.Rows[i].Cells[0].FindControl("hdfActividad")).Value;
                drP["n_destajo_hoj"] = GridView1.Rows[i].Cells[7].Text; ;
                drP["c_numhoja_cpn"] = "1";
                drP["c_referencia_cpn"] = "51" + RegresaDigitos(contador.ToString().Length) + contador.ToString();
                dtRegistrosApagar.Rows.Add(drP);
                contador++;
            }
          
        }

        //calling create Excel File Method and ing dataTable   
        CreateExcelFile(dtRegistrosApagar);  
    }
    public string RegresaDigitos(int numero)
    {
        switch (numero)
        {
            case 1:
                return "000";

            case 2:
                return "00";
            case 3:
                return "0";
            case 4:
                return "";
            case 5:
                return "";
            default:
                return "";
        }
    }
    private static DataTable dtRegistrisPagos()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("c_codigo_lug");
        dt.Columns.Add("d_fecha_cpn");
        dt.Columns.Add("c_terminal_hoj");
        dt.Columns.Add("c_codigo_emp");
        dt.Columns.Add("c_codigo_lot");
        dt.Columns.Add("c_codigo_tab");
        dt.Columns.Add("n_destajo_hoj");
        dt.Columns.Add("c_numhoja_cpn");
        dt.Columns.Add("c_referencia_cpn");
        return dt;
    }

    public void CreateExcelFile(DataTable Excel)
    {

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Excel, "Pago");

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Pago.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }  

    [System.Web.Services.WebMethod]
    public static string GetCurrentTime(string name)
    {
        return "Hello " + name + Environment.NewLine + "The Current Time is: "
            + DateTime.Now.ToString();
    }
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string noEmpleado,nombre, actividad, horaInicio, horaFin, idActividad;
        
        //lblName.Text = GridView1.SelectedRow.Cells[1].Text;
        noEmpleado = (GridView1.SelectedRow.FindControl("lnkEmpleado") as LinkButton).Text;
        idActividad = (GridView1.SelectedRow.FindControl("hdfActividad") as HiddenField).Value;
        lblNoEmpleado.Text = noEmpleado;
        lblNombreEmpleado.Text = GridView1.SelectedRow.Cells[1].Text;
        lblActividad.Text = GridView1.SelectedRow.Cells[2].Text;
        lblFechaInicio.Text = GridView1.SelectedRow.Cells[5].Text;
        lblFechaFin.Text = GridView1.SelectedRow.Cells[6].Text;
        lblCantidadSurcosTrabajados.Text = (GridView1.SelectedRow.FindControl("hdfCantidad") as HiddenField).Value;

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        DataTable result;
        if (cbxCosecha.Checked)
        {
            parameters.Add("@idAsociado", noEmpleado);
            parameters.Add("@fechaInicio", Convert.ToDateTime(lblFechaInicio.Text).ToString("yyyy-MM-dd"));
            parameters.Add("@fechaFin", Convert.ToDateTime(lblFechaFin.Text).ToString("yyyy-MM-dd"));
            parameters.Add("@idInvernadero", ddlInvernadero.SelectedValue);

            DataAccess dataaccess = new DataAccess();
            result = dataaccess.executeStoreProcedureDataTable("procObtieneCosechaPorEmpleadoDetalle", parameters);
        }
        else
        {
            parameters.Add("@NoEmpleado", noEmpleado);
            parameters.Add("@fechaInicio", Convert.ToDateTime(lblFechaInicio.Text).ToString("yyyy-MM-dd"));
            parameters.Add("@fechaFin", Convert.ToDateTime(lblFechaFin.Text).ToString("yyyy-MM-dd"));
            parameters.Add("@idActividad", idActividad);

            DataAccess dataaccess = new DataAccess();
            result = dataaccess.executeStoreProcedureDataTable("procObtieneDetalleActividadApagar", parameters);
        }
        GvDetalle.DataSource = result;
        GvDetalle.DataBind();

        mpe.Show();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            //Determine the RowIndex of the Row whose LinkButton was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GridView1.Rows[rowIndex];

            //Fetch value of Name.
            string name = (row.FindControl("lnkEmpleado") as TextBox).Text;

            //Fetch value of Country
            string country = row.Cells[1].Text;

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Name: " + name + "\\nCountry: " + country + "');", true);
        }
    }


    [System.Web.Services.WebMethod]
    public static string SaveData(string noEmpleado, string horaInicio, string horaFin, string idActividad)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@NoEmpleado", noEmpleado);
            parameters.Add("@fechaInicio", Convert.ToDateTime(horaInicio).ToString("yyyy-MM-dd"));
            parameters.Add("@fechaFin", Convert.ToDateTime(horaFin).ToString("yyyy-MM-dd"));
            parameters.Add("@idActividad", idActividad);

            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("procObtieneDetalleActividadApagar", parameters);
            GridView gvDetalle = new GridView();
            gvDetalle.DataSource = result;
            gvDetalle.DataBind();
            
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally { }
    }
    public void BindGridView(DataTable dtResult)
    {
        GvDetalle.DataSource = dtResult;
        GvDetalle.DataBind();
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        ddlRancho.SelectedIndex = 0;
        txtFechaInicio.Text = "";
        txtFechaFin.Text = "";
    }
    protected void ddlRancho_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneInvernaderosPorUsuario(ddlRancho.SelectedValue, Session["idUsuario"].ToString());
    }
}