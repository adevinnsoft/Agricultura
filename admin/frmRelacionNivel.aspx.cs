using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;
using Incidencias;


/*using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.IO;*/



public partial class frmRelacionNivel : BasePage
{
    Incidencias.IncidenciasWS ws = new Incidencias.IncidenciasWS();
    
    //#region variables del sentido de ordenacion en la tabla
    //protected string SortExpression
    //{
    //    get
    //    {
    //        return ViewState["SortExpression"] as string;
    //    }
    //    set
    //    {
    //        ViewState["SortExpression"] = value;
    //    }
    //}

    //protected SortDirection SortDirection
    //{
    //    get
    //    {
    //        object o = ViewState["SortDirection"];

    //        if(o == null)
    //            return SortDirection.Ascending;
    //        else
    //            return (SortDirection)o;
    //    }
    //    set
    //    {
    //        ViewState["SortDirection"] = value;
    //    }
    //}
    //#endregion variables del sentido de ordenacion en la tabla

	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			if(Session["usernameInj"] == null)
				Response.Redirect("~/frmLogin.aspx", false);

			cargarDatos();
			//cargar_ddlTipoInventario();
		}
	}

	#region metodos
	protected void cargarDatos()
	{
		//Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
		//parameters.Add("@lengua", CultureInfo.CurrentCulture.Name);

		try
		{
            //DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_TemporalesObtener", parameters);

            //gvTemporales.DataSource = ViewState["gvTemporal"] = dt;
            //gvTemporales.DataBind();
            divGridView.InnerHtml = gvRelacionAsociados();
            divRadiosFamilias.InnerHtml = dibujaRadiosFamilias();
            divTablaAsociados.InnerHtml += tablaAsociados(Convert.ToInt32(HttpContext.Current.Session["idLider"].ToString()));

		}
		catch(Exception e)
		{
			//popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "loaderrordb").ToString(), Comun.MESSAGE_TYPE.Info);
			Log.Error(e.ToString());
		}

		Limpiar();
	}

    [WebMethod(EnableSession = true)]
    public static string dibujaRadiosFamilias()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable familias = dataaccess.executeStoreProcedureDataTable("spr_FamiliasObtiene", new Dictionary<string, object>() {{ "@lengua", CultureInfo.CurrentCulture.Name }});

            response += "<table><tr>";
            foreach (DataRow item in familias.Rows)
            {
                response += "<td style='display:" + (item["bActivo"].ToString() == "True" ? "inline-block" : "none") + "; height:25px;'><input class='check-with-label selFamilia' type='radio' id='F" + item["IdFamilia"] + "' value='" + item["IdFamilia"] + "|" + item["Nombre"] + "' " + (item["bActivo"].ToString() == "True" ? "" : "disabled") + " name='familias'/>"
                    + "<label class='label-for-check' for='F" + item["IdFamilia"] + "'><span></span>" + item["Nombre"] + "</label></td>";
            }
            response += "</tr></table>";

            //Incidencias.IncidenciasWS ws = new Incidencias.IncidenciasWS();
            //Asociados[] query = ws.swAsociados("idPlanta", 0, 0, 0, 0, 0, 0, true);

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }


    [WebMethod(EnableSession = true)]
    public static string dibujaRadiosNiveles(int idFamilia)
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable familias = dataaccess.executeStoreProcedureDataTable("spr_NivelesObtiene", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@idFamilia", idFamilia} });

            foreach (DataRow item in familias.Rows)
            {
                response += "<td style='display:" + (item["bActivo"].ToString() == "True" ? "inline-block" : "none") + "; height:25px;'><input class='check-with-label selNivel' type='radio' id='N" + item["Nivel"] + "' value='" + item["idNivel"] + "' " + (item["bActivo"].ToString() == "True" ? "" : "disabled") + " name='niveles'/>"
                    + "<label class='label-for-check' for='N" + item["Nivel"] + "'><span></span>" + item["Nivel"] + "</label></td>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string gvRelacionAsociados()
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_RelacionAsociadosGv", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@idLider", HttpContext.Current.Session["idLider"] } });

            if (asociados.Rows.Count == 0)
            {
                response += "<table id='gvAsociados' class='gridView'><thead><tr><td>No hay asociados relacionados para un nivel.</td></tr></thead>";
                //response += "<tbody><tr><td>No hay asociados relacionados para un nivel.</td></tr><tbody>";
            }
            else
            {
                response += "<table id='gvAsociados' class='gridView'><thead><tr><th>" + "Familia" + "</th><th>" + "Nivel" + "</th><th>" + "Cantidad" + "</th></tr></thead><tbody>";
                foreach (DataRow item in asociados.Rows)
                {
                    response += "<tr onClick='showGrupo(" + item["idFamilia"] + "," + item["idNivel"] + ")'>"
                        + "<td>" + item["Familia"] + "</td>"
                        + "<td>" + item["Nivel"] + "</td>"
                        + "<td>" + item["cantidad"] + "</td>"
                        + "</tr>";
                }
                response += "</tbody></table>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string tablaAsociados(int idLider)
    {
        var response = "";

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_AsociadosObtener", new Dictionary<string, object>() { { "@idLider", idLider } });

            response += "<table id='tablaAsociados' class='gridView'><thead><tr><th>" + "Nombre" + "</th><th>" + "Familia/Nivel" + "</th></tr></thead><tbody>";
            foreach (DataRow item in asociados.Rows)
            {
                response += "<tr>"
                    + "<td class='left'>"
                    + "<input class='check-with-label selAsociados' type='checkbox' id='" + item["IDEmployee"] + "' value='" + item["IDEmployee"] + "|" + item["FullName"] + "' name='asociados' />" //" + (item["Grupo"].ToString() != "---" ? "disabled='disabled'": "") + "
                    + "<label class='label-for-check' for='" + item["IDEmployee"] + "'><span></span>" + item["IDEmployee"] + " - " + item["FullName"] + "</label>"
                    + "</td>"
                    + "<td id='label" + item["IDEmployee"] + "'>" + item["Grupo"] + "</td>"
                    + "</tr>";
            }
            response += "</tbody></table>";

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    
    [WebMethod(EnableSession = true)]
    public static string[] addRows(int idFamilia, int idNivel)
    {
        var response = new string[2];
        response[0] = "" + idFamilia + "" + idNivel;

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable asociados = dataaccess.executeStoreProcedureDataTable("spr_RelacionAsociadosObtener", new Dictionary<string, object>() { { "@idFamilia", idFamilia }, { "@idNivel", idNivel }, { "@idLider", HttpContext.Current.Session["idLider"]} });

            foreach (DataRow item in asociados.Rows)
            {
                response[1] += "<tr><td><div class='imgGuardado' id='img" + item["IDEmployee"] + "' title='Este asociado ya esta guardado.'/></td><td>" + item["IDEmployee"] + " - " + item["FullName"] + "</td><td  style='text-align:center;'><img src='../comun/img/remove-icon.png' alt='eliminar' title='Eliminar' width='20' height='20' onClick='regresarAsociado(\"" + item["IDEmployee"] + "\", this);' /></td></tr>";
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[1] = "<tr><td></td><td colspan='8'>" + ex.Message + "</td></tr>";
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string[] borrarAsociado(string idAsociado)
    {
        var response = new string[3];
        response[2] = idAsociado;
        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_RelacionAsociadosEliminar", new Dictionary<string, object>() { { "@idAsociado", idAsociado } });

            response[0] = result.Rows[0]["msg"].ToString();
            response[1] = result.Rows[0]["detalle"].ToString();

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[0] = "error";
            response[1] = ex.Message;
            return response;
        }

        return response;
    }

    [WebMethod(EnableSession = true)]
    public static string[] guardaRelacion(string niveles, string asociados)
    {
        var response = new string[2];

        try
        {
            DataAccess dataaccess = new DataAccess();
            DataTable result = dataaccess.executeStoreProcedureDataTable("spr_RealacionAsociadosNivelInsertar", new Dictionary<string, object>() { { "@idAsociados", asociados }, { "@idLider", HttpContext.Current.Session["idLider"]}, { "@idNiveles", niveles }, { "@idUser", HttpContext.Current.Session["idUsuario"].ToString() }, { "@lengua", CultureInfo.CurrentCulture.Name } });

            response[0] = result.Rows[0]["msg"].ToString();
            response[1] = result.Rows[0]["detalle"].ToString();
            HttpContext.Current.Session["Asociados"] = GetDataTableToJson(dataaccess.executeStoreProcedureDataTable("spr_AsociadosPorNivelLider", new Dictionary<string, object>() { { "@idUsuario", HttpContext.Current.Session["idUsuario"].ToString() } }));

        }
        catch (Exception ex)
        {
            Log.Error(ex);
            response[0] = "error";
            response[1] = ex.Message;
            return response;
        }

        return response;
    }

	protected void Limpiar()
	{
        //idTemporal.Value = "";
        //txtTemporal.Text = String.Empty;
        //txtTemporal_EN.Text = String.Empty;
        //txtDescripcion_ES.Text = String.Empty;
        //txtDescripcion_EN.Text = String.Empty;
        //txtEficiencia.Text = String.Empty;
        //txtColorP.Text = "ffffff";
        //txtColorP.BackColor = System.Drawing.Color.White;
        //chkActivo.Checked = true;
        //chkRepetir.Checked = true;
        //chkBorrar.Checked = false;
        //txtFechaEnd.Text = String.Empty;
        //txtFechaStart.Text = String.Empty;
        //btnCancel.Text = GetGlobalResourceObject("Commun", "Limpiar").ToString();
        //btnSave.Text = GetGlobalResourceObject("Commun", "Guardar").ToString();
    }
	#endregion

	#region botones

    //protected void Guardar_Actualizar(object sender, EventArgs e)
    //{
    //    Dictionary<string, object> parameters = new Dictionary<string, object>();
    //    DataTable result;

    //    try
    //    {
    //        //parameters.Add("@lengua", CultureInfo.CurrentCulture.Name);
    //        //parameters.Add("@idTemporal", idTemporal.Value == "" ? "0" : idTemporal.Value);
    //        //parameters.Add("@Temporal", txtTemporal.Text);
    //        //parameters.Add("@Temporal_EN", txtTemporal_EN.Text);
    //        //parameters.Add("@Activo", chkActivo.Checked ? "True" : "False");
    //        //parameters.Add("@RepetirAnual", chkRepetir.Checked ? "True" : "False");
    //        //parameters.Add("@Borrar", chkBorrar.Checked ? "true" : "False");
    //        //parameters.Add("@Descripcion", txtDescripcion_ES.Text);
    //        //parameters.Add("@Descripcion_EN", txtDescripcion_EN.Text);
    //        //parameters.Add("@FechaStart", DateTime.ParseExact(txtFechaStart.Text, "dd-MM-yyyy", null).ToString("yyyyMMdd"));
    //        //parameters.Add("@FechaEnd", DateTime.ParseExact(txtFechaEnd.Text, "dd-MM-yyyy", null).ToString("yyyyMMdd"));
    //        //parameters.Add("@Eficiencia", txtEficiencia.Text);
    //        //parameters.Add("@Color", txtColorP.Text);
    //        //parameters.Add("@Usuario", Session["idUsuario"].ToString());


    //        result = dataaccess.executeStoreProcedureDataTable("spr_TemporalesInsertarxxx", parameters);


    //        switch (result.Rows[0]["msg"].ToString())
    //        {
    //            case "ok":
    //                popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Success);
    //                break;
    //            case "info":
    //                popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Info);
    //                break;
    //            default:
    //                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoGuardado").ToString() + "<br /> " + result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Error);
    //                break;
    //        }

    //        cargarDatos();
    //        /*if (result.Rows[0]["msg"].ToString() == "ok")
    //        {
    //            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Guardado").ToString(), Comun.MESSAGE_TYPE.Info);
    //            cargarDatos();
    //        }

    //        else
    //        {
    //            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoGuardado").ToString() + result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Error);
    //            cargarDatos();
    //            Limpiar();
    //        }*/
    //    }
    //    catch(Exception ex)
    //    {
    //        popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Info);
    //        Log.Error(ex.ToString());
    //    }
    //}

	protected void Cancelar_Limpiar(object sender, EventArgs e)
	{
		try
		{
			Limpiar();
		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}
	#endregion

	#region eventos grid
    //protected void gvTemporales_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if(Session["usernameInj"] == null)
    //        Response.Redirect("~/frmLogin.aspx", false);

    //    try
    //    {
    //        idTemporal.Value = gvTemporales.DataKeys[gvTemporales.SelectedIndex].Value.ToString();

    //        DataTable dt = (DataTable)ViewState["gvTemporal"];
    //        DataRow[] dr = dt.Select("idTemporal = " + idTemporal.Value);
    //        if(dr.Length > 0)
    //        {
    //            txtTemporal.Text = dr[0]["Temporal"].ToString();
    //            txtTemporal_EN.Text = dr[0]["Temporal_EN"].ToString();
    //            txtDescripcion_ES.Text = dr[0]["Descripcion"].ToString();
    //            txtDescripcion_EN.Text = dr[0]["Descripcion_EN"].ToString();
    //            txtEficiencia.Text = dr[0]["Eficiencia"].ToString();
    //            txtColorP.Text = dr[0]["Color"].ToString();
    //            txtFechaStart.Text = DateTime.ParseExact(dr[0]["FechaStart"].ToString().Split(' ')[0], (CultureInfo.CurrentCulture.ToString() == "es-MX" ? "dd/MM/yyyy" : "M/d/yyyy"), CultureInfo.CurrentCulture).ToString("dd-MM-yyyy"); //Convert.ToDateTime(dr[0]["FechaStart"].ToString()).ToShortDateString();
    //            txtFechaEnd.Text = DateTime.ParseExact(dr[0]["FechaEnd"].ToString().Split(' ')[0], (CultureInfo.CurrentCulture.ToString() == "es-MX" ? "dd/MM/yyyy" : "M/d/yyyy"), CultureInfo.CurrentUICulture).ToString("dd-MM-yyyy"); //Convert.ToDateTime(dr[0]["FechaEnd"].ToString()).ToShortDateString(); 

    //            chkActivo.Checked = dr[0]["Activo"].ToString().Equals("True") ? true : false;
    //            chkRepetir.Checked = dr[0]["RepetirAnual"].ToString().Equals("True") ? true : false;

    //            btnSave.Text = GetGlobalResourceObject("Commun", "Actualizar").ToString();
    //            btnCancel.Text = GetGlobalResourceObject("Commun", "Cancelar").ToString();
    //        }
    //    }
    //    catch(Exception ex)
    //    {
    //        popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "InnerError").ToString(), Comun.MESSAGE_TYPE.Info);
    //        Log.Error(ex.ToString());
    //    }
    //}

    //protected void Ordenartabla()
    //{
    //    bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
    //    DataTable dt = ViewState["gvTemporal"] != null ? ViewState["gvTemporal"] as DataTable : null;
    //    DataView dataView = dt.DefaultView;

    //    if(dataView != null)
    //    {
    //        dataView.Sort = this.SortExpression + " " + (sortAscending ? "ASC" : "DESC");
    //        gvTemporales.DataSource = dataView;
    //        gvTemporales.DataBind();
    //    }
    //}

    //protected void gvTemporales_Sorted(object sender, EventArgs e)
    //{
    //    Ordenartabla();
    //}

    //protected void gvTemporales_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if(this.SortExpression == e.SortExpression)
    //    {
    //        this.SortDirection = this.SortDirection == SortDirection.Ascending ?
    //             SortDirection.Descending : SortDirection.Ascending;
    //    }
    //    else
    //    {
    //        this.SortDirection = SortDirection.Ascending;
    //    }

    //    this.SortExpression = e.SortExpression;
    //    gvTemporales.EditIndex = -1;
    //    gvTemporales.SelectedIndex = -1;
    //}

    //protected void gvTemporales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    if(String.IsNullOrEmpty(this.SortExpression))
    //        cargarDatos();
    //    else
    //        Ordenartabla();
    //}

    //protected void gvTemporales_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    switch(e.Row.RowType)
    //    {
    //        case DataControlRowType.DataRow:
    //            e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvTemporales, "Select$" + e.Row.RowIndex);
    //            break;
    //    }
    //}

    //protected void gvTemporales_PreRender(object sender, EventArgs e)
    //{
    //    if(gvTemporales.HeaderRow != null)
    //        gvTemporales.HeaderRow.TableSection = TableRowSection.TableHeader;
    //}

    //protected override void Render(HtmlTextWriter writer)
    //{
    //    try
    //    {
    //        for(int i = 0; i < gvTemporales.Rows.Count; i++)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(new System.Web.UI.PostBackOptions(gvTemporales, "Select$" + i.ToString()));
    //        }
    //        base.Render(writer);

    //    }
    //    catch(Exception ex)
    //    {
    //        Log.Error(ex.ToString());
    //    }
    //}
	#endregion

}