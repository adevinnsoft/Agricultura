using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web;

public partial class catalogos_frmTemporales : BasePage
{

	#region variables del sentido de ordenacion en la tabla
	protected string SortExpression
	{
		get
		{
			return ViewState["SortExpression"] as string;
		}
		set
		{
			ViewState["SortExpression"] = value;
		}
	}

	protected SortDirection SortDirection
	{
		get
		{
			object o = ViewState["SortDirection"];

			if(o == null)
				return SortDirection.Ascending;
			else
				return (SortDirection)o;
		}
		set
		{
			ViewState["SortDirection"] = value;
		}
	}
	#endregion variables del sentido de ordenacion en la tabla

	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			if(Session["usernameInj"] == null)
				Response.Redirect("~/frmLogin.aspx", false);

			cargarDatos();
		}
	}

	#region metodos
	protected void cargarDatos()
	{
		Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
		parameters.Add("@lengua", CultureInfo.CurrentCulture.Name);

		try
		{
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_TemporalesObtener", parameters);

            gvTemporales.DataSource = ViewState["gvTemporal"] = dt;
            gvTemporales.DataBind();



            //DataSet lTiempo = dataaccess.executeStoreProcedureDataSet("spr_LineaTiempoObtener", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@año", null } });
            //DataTable años = lTiempo.Tables[0];
            //DataTable linea = lTiempo.Tables[1];
            

            /*ddlAño.DataSource = años;
            ddlAño.DataTextField = "year";
            ddlAño.DataValueField = "year";

            ddlAño.DataBind();
            //ddlAño.Items.Insert(0, new ListItem(GetGlobalResourceObject("Commun", "Select").ToString(), ""));*/
            //sacar año actual
            DateTime anio = DateTime.Now;
            
            for (int a = 0; a <= 10; a++) 
            {
                ddlAño.Items.Insert(a, new ListItem((anio.Year + a).ToString(), "" + (anio.Year + a)));
            }
            /*ddlAño.Items.Insert(0, new ListItem(anio.Year.ToString(), "" + anio.Year));
            ddlAño.Items.Insert(1, new ListItem((anio.Year + 1).ToString(), "" + (anio.Year + 1)));
            ddlAño.Items.Insert(2, new ListItem((anio.Year + 2).ToString(), "" + (anio.Year +2)));*/
            ddlAño.DataBind();

            dibujaEscala(Convert.ToInt32(ddlAño.SelectedValue));
            //if (años.Rows.Count == 0)
            //{
            //    ddlAño.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "NoDdlItems").ToString(), ""));
            //}
            //else { 
                ddlAño.SelectedValue = DateTime.Now.Year.ToString(); 
            //}
            //lbDisplay.Text += ddlAño.SelectedValue;

		}
		catch(Exception e)
		{
			popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "loaderrordb").ToString(), Comun.MESSAGE_TYPE.Info);
			Log.Error(e.ToString());
		}

		Limpiar();
	}

    [WebMethod(EnableSession = true)]
    public static string dibujaLinea(int año)
    {
        var response = "";

        try
        {

            var dias = 0f;

            for (int m = 1; m <= 12; m++)
            {
                dias += DateTime.DaysInMonth(año, m);
            }

            DataAccess dataaccess = new DataAccess();
            DataSet lTiempo = dataaccess.executeStoreProcedureDataSet("spr_LineaTiempoObtener", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name }, { "@año", año } });
            DataTable linea = lTiempo.Tables[1];

            foreach (DataRow item in linea.Rows)
            {
                var color2 = setColor(item["Color"].ToString());
                response += "<div class='ltemporales tooltip' title='"
                    + "<b>" + HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Url.AbsolutePath.Replace("/dibujaLinea", ""), "BoundFieldResource4.HeaderText") + ":</b> " + item["NombreGv"] + "<br />"
                    + "<b>" + HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Url.AbsolutePath.Replace("/dibujaLinea", ""), "BoundFieldResource1.HeaderText") + ":</b> " + String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(item["FechaStart"].ToString())) + "<br />"
                    + "<b>" + HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Url.AbsolutePath.Replace("/dibujaLinea", ""), "BoundFieldResource2.HeaderText") + ":</b> " + String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(item["FechaEnd"].ToString())) + "<br />"
                    + "' style='margin-left:" + ((Convert.ToDouble(item["margin-left"].ToString()) * 100f) / dias) + "%; width:" + ((((Convert.ToDouble(item["width"].ToString()) + Convert.ToDouble(item["margin-left"].ToString()) > dias ? (Convert.ToDouble(item["width"].ToString()) - (Convert.ToDouble(item["width"].ToString()) + Convert.ToDouble(item["margin-left"].ToString()) - dias)) + 1f : Convert.ToDouble(item["width"].ToString()) + 1f)) * 100f) / dias) + "%; "
                    + "background-image:" + (item["RepetirAnual"].ToString() == "True" ? "url(../comun/img/repeat.png)," : "") + " -webkit-gradient(linear, left top, left bottom, from(#" + item["Color"] + "), to(#" + color2 + "));  "
                    + "background-image:" + (item["RepetirAnual"].ToString() == "True" ? "url(../comun/img/repeat.png)," : "") + " -webkit-linear-gradient(top, #" + item["Color"] + " 0%, #" + color2 + " 100%);"
                    + "background-image:" + (item["RepetirAnual"].ToString() == "True" ? "url(../comun/img/repeat.png)," : "") + " -o-linear-gradient(top, #" + item["Color"] + " 0%, #" + color2 + " 100%);"
                    + "background-image:" + (item["RepetirAnual"].ToString() == "True" ? "url(../comun/img/repeat.png)," : "") + " linear-gradient(to bottom, #" + item["Color"] + " 0%, #" + color2 + " 100%);"
                    + "background-color:#" + item["Color"] + "; position:absolute;'></div>";
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


    private void dibujaEscala(int año)
    {
        string meses = "";
        string[] mes;
        var left = 0f;
        var dias = 0f;

        for (int m = 1; m <= 12; m++)
        {
            dias += DateTime.DaysInMonth(año, m);
        }

        if (CultureInfo.CurrentCulture.Name == "es-MX")
        {
            mes = new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
        }
        else
        {
            mes = new string[] { "January", "February", "March", "April ", "May", "June", "July", "August ", "September ", "October", "November ", "December " };
        }

        /*foreach (DataRow item in linea.Rows)
        {
            var color2 = setColor(item["Color"].ToString());
            lineas += "<div class='ltemporales tooltip' title='"
                + "<b>" + GetLocalResourceObject("BoundFieldResource4.HeaderText") + ":</b> " + item["NombreGv"] + "<br />"
                + "<b>" + GetLocalResourceObject("BoundFieldResource1.HeaderText") + ":</b> " + String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(item["FechaStart"].ToString())) + "<br />"
                + "<b>" + GetLocalResourceObject("BoundFieldResource2.HeaderText") + ":</b> " + String.Format("{0:dd/MMM/yyyy}", DateTime.Parse(item["FechaEnd"].ToString())) + "<br />"
                    + "' style='margin-left:" + ((Convert.ToDouble(item["margin-left"].ToString()) * 100f) / dias) + "%; width:" + ((((Convert.ToDouble(item["width"].ToString()) + Convert.ToDouble(item["margin-left"].ToString()) > dias ? (Convert.ToDouble(item["width"].ToString()) - (Convert.ToDouble(item["width"].ToString()) + Convert.ToDouble(item["margin-left"].ToString()) - dias)) + 1f : Convert.ToDouble(item["width"].ToString()) + 1f)) * 100f) / dias) + "%; "
                + "background-image: -webkit-gradient(linear, left top, left bottom, from(#" + item["Color"] + "), to(#" + color2 + "));  "
                + "background-image: -webkit-linear-gradient(top, #" + item["Color"] + " 0%, #" + color2 + " 100%);"
                + "background-image: -o-linear-gradient(top, #" + item["Color"] + " 0%, #" + color2 + " 100%);"
                + "background-image: linear-gradient(to bottom, #" + item["Color"] + " 0%, #" + color2 + " 100%);"
                + "background-color:#" + item["Color"] + "; background-repeat: repeat-x; position:absolute;'></div>";
        }*/
        
        divTiempo.InnerHtml = dibujaLinea(año);

        for(int m=0; m<=11; m++){
            meses += "<div class='lmeses' style='margin-left:" + left + "%; width:" + ((DateTime.DaysInMonth(2015, m + 1)) * 100f / dias) + "%; /*background-color:#ffffff;*/ position:absolute;'>" + mes[m].ToString() + "</div>";
            left += ((DateTime.DaysInMonth(2015, m + 1)) * 100f / dias);
        }
        divMeses.InnerHtml = meses;

    }

    [WebMethod(EnableSession = true)]
    public static string setColor(string color)
    {
            var bcolor1 = Convert.ToInt32(color.Substring(0, 2), 16);
            var bcolor2 = Convert.ToInt32(color.Substring(2, 2), 16);
            var bcolor3 = Convert.ToInt32(color.Substring(4, 2), 16);

            bcolor1 = (bcolor1 - 30) > 0 ? (bcolor1 - 30) : 0;
            bcolor2 = (bcolor2 - 30) > 0 ? (bcolor2 - 30) : 0;
            bcolor3 = (bcolor3 - 30) > 0 ? (bcolor3 - 30) : 0;

            return (String.Format("{0:x}", bcolor1).ToString() =="0" ? "00" : String.Format("{0:x}", bcolor1)) + (String.Format("{0:x}", bcolor2).ToString() =="0" ? "00" : String.Format("{0:x}", bcolor2)) + (String.Format("{0:x}", bcolor3).ToString() =="0" ? "00" : String.Format("{0:x}", bcolor3));//getHexValue(bcolor1) + getHexValue(bcolor2) + getHexValue(bcolor3) + ';');
        }

	protected void Limpiar()
	{
        idTemporal.Value = "";
		txtTemporal.Text = String.Empty;
		txtTemporal_EN.Text = String.Empty;
        txtDescripcion_ES.Text = String.Empty;
        txtDescripcion_EN.Text = String.Empty;
        txtEficiencia.Text = String.Empty;
        txtColorP.Text = "ffffff";
        txtColorP.BackColor = System.Drawing.Color.White;
        chkActivo.Checked = true;
        chkRepetir.Checked = true;
        chkBorrar.Checked = false;
        txtFechaEnd.Text = String.Empty;
        txtFechaStart.Text = String.Empty;
        btnCancel.Text = GetGlobalResourceObject("Commun", "Limpiar").ToString();
        btnSave.Text = GetGlobalResourceObject("Commun", "Guardar").ToString();
    }
	#endregion

	#region botones

	protected void Guardar_Actualizar(object sender, EventArgs e)
	{
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        DataTable result;

        if (!new System.Text.RegularExpressions.Regex("^[0-9][0-9]{0,2}([\\.][0-9]+)?$").IsMatch(txtEficiencia.Text))
        {
            popUpMessageControl1.setAndShowInfoMessage("El valor de eficiencia es incorrecto.", Comun.MESSAGE_TYPE.Error);
            return;
        
        }

		    try
		    {
                parameters.Add("@lengua", CultureInfo.CurrentCulture.Name);
                parameters.Add("@idTemporal", idTemporal.Value == "" ? "0" : idTemporal.Value);
                parameters.Add("@Temporal", txtTemporal.Text);
                parameters.Add("@Temporal_EN", txtTemporal_EN.Text);
                parameters.Add("@Activo", chkActivo.Checked ? "True" : "False");
                parameters.Add("@RepetirAnual", chkRepetir.Checked ? "True" : "False");
                parameters.Add("@Borrar", chkBorrar.Checked ? "true" : "False");
		        parameters.Add("@Descripcion", txtDescripcion_ES.Text);
		        parameters.Add("@Descripcion_EN", txtDescripcion_EN.Text);
                parameters.Add("@FechaStart", DateTime.ParseExact(txtFechaStart.Text, "dd-MM-yyyy", null).ToString("yyyyMMdd"));
		        parameters.Add("@FechaEnd", DateTime.ParseExact(txtFechaEnd.Text, "dd-MM-yyyy", null).ToString("yyyyMMdd"));
                parameters.Add("@Eficiencia", txtEficiencia.Text);
                parameters.Add("@Color", txtColorP.Text);
                parameters.Add("@Usuario", Session["idUsuario"].ToString());


                result = dataaccess.executeStoreProcedureDataTable("spr_TemporalesInsertar", parameters);


                switch (result.Rows[0]["msg"].ToString())
                {
                    case "ok":
                        popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Success);
                        break;
                    case "info":
                        popUpMessageControl1.setAndShowInfoMessage(result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Info);
                        break;
                    default:
                        popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "NoGuardado").ToString() + ": " + result.Rows[0]["detalle"].ToString(), Comun.MESSAGE_TYPE.Error);
                        break;
                }

                cargarDatos();

		    }
		    catch(Exception ex)
		    {
                popUpMessageControl1.setAndShowInfoMessage(ex.Message, Comun.MESSAGE_TYPE.Error);
			    Log.Error(ex.ToString());
		    }
        
	}

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
	protected void gvTemporales_SelectedIndexChanged(object sender, EventArgs e)
	{
		if(Session["usernameInj"] == null)
			Response.Redirect("~/frmLogin.aspx", false);

		try
		{
            idTemporal.Value = gvTemporales.DataKeys[gvTemporales.SelectedIndex].Value.ToString();

            DataTable dt = (DataTable)ViewState["gvTemporal"];
            DataRow[] dr = dt.Select("idTemporal = " + idTemporal.Value);
			if(dr.Length > 0)
			{
                txtTemporal.Text = dr[0]["Temporal"].ToString();
                txtTemporal_EN.Text = dr[0]["Temporal_EN"].ToString();
                txtDescripcion_ES.Text = dr[0]["Descripcion"].ToString();
                txtDescripcion_EN.Text = dr[0]["Descripcion_EN"].ToString();
                txtEficiencia.Text = dr[0]["Eficiencia"].ToString();
                txtColorP.Text = dr[0]["Color"].ToString();
                txtFechaStart.Text = DateTime.ParseExact(dr[0]["FechaStart"].ToString().Split(' ')[0], (CultureInfo.CurrentCulture.ToString() == "es-MX" ? "dd/MM/yyyy" : "M/d/yyyy"), CultureInfo.CurrentCulture).ToString("dd-MM-yyyy"); //Convert.ToDateTime(dr[0]["FechaStart"].ToString()).ToShortDateString();
                txtFechaEnd.Text = DateTime.ParseExact(dr[0]["FechaEnd"].ToString().Split(' ')[0], (CultureInfo.CurrentCulture.ToString() == "es-MX" ? "dd/MM/yyyy" : "M/d/yyyy"), CultureInfo.CurrentUICulture).ToString("dd-MM-yyyy"); //Convert.ToDateTime(dr[0]["FechaEnd"].ToString()).ToShortDateString(); 

                chkActivo.Checked = dr[0]["Activo"].ToString().Equals("True") ? true : false;
                chkRepetir.Checked = dr[0]["RepetirAnual"].ToString().Equals("True") ? true : false;

                btnSave.Text = GetGlobalResourceObject("Commun", "Actualizar").ToString();
                btnCancel.Text = GetGlobalResourceObject("Commun", "Cancelar").ToString();
			}
		}
		catch(Exception ex)
		{
            popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "InnerError").ToString(), Comun.MESSAGE_TYPE.Info);
			Log.Error(ex.ToString());
		}
	}

	protected void Ordenartabla()
	{
		bool sortAscending = this.SortDirection == SortDirection.Ascending ? true : false;
        DataTable dt = ViewState["gvTemporal"] != null ? ViewState["gvTemporal"] as DataTable : null;
		DataView dataView = dt.DefaultView;

		if(dataView != null)
		{
			dataView.Sort = this.SortExpression + " " + (sortAscending ? "ASC" : "DESC");
			gvTemporales.DataSource = dataView;
			gvTemporales.DataBind();
		}
	}

	protected void gvTemporales_Sorted(object sender, EventArgs e)
	{
		Ordenartabla();
	}

	protected void gvTemporales_Sorting(object sender, GridViewSortEventArgs e)
	{
		if(this.SortExpression == e.SortExpression)
		{
			this.SortDirection = this.SortDirection == SortDirection.Ascending ?
				 SortDirection.Descending : SortDirection.Ascending;
		}
		else
		{
			this.SortDirection = SortDirection.Ascending;
		}

		this.SortExpression = e.SortExpression;
		gvTemporales.EditIndex = -1;
		gvTemporales.SelectedIndex = -1;
	}

	protected void gvTemporales_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		if(String.IsNullOrEmpty(this.SortExpression))
			cargarDatos();
		else
			Ordenartabla();
	}

	protected void gvTemporales_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		switch(e.Row.RowType)
		{
			case DataControlRowType.DataRow:
				e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvTemporales, "Select$" + e.Row.RowIndex);
				break;
		}
	}

	protected void gvTemporales_PreRender(object sender, EventArgs e)
	{
		if(gvTemporales.HeaderRow != null)
			gvTemporales.HeaderRow.TableSection = TableRowSection.TableHeader;
	}

	protected override void Render(HtmlTextWriter writer)
	{
		try
		{
			for(int i = 0; i < gvTemporales.Rows.Count; i++)
			{
				Page.ClientScript.RegisterForEventValidation(new System.Web.UI.PostBackOptions(gvTemporales, "Select$" + i.ToString()));
			}
			base.Render(writer);

		}
		catch(Exception ex)
		{
			Log.Error(ex.ToString());
		}
	}
	#endregion

}