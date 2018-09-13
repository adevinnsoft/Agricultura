using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

public partial class configuracion_frmZonificacion : BasePage
{
    #region Eventos Pagina
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session["usernameInj"] == null)
                    Response.Redirect("~/frmLogin.aspx", false);

                ObtieneRanchos();
                ObtieneInvernaderosPorRancho(Convert.ToInt32(ddlRancho.SelectedValue));
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.ToString());
        }
    }



    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["usernameInj"] == null)
                Response.Redirect("~/frmLogin.aspx", false);


            if (ddlInvernadero.SelectedValue == "" || ddlRancho.SelectedValue == "")
            {

                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("camposRequeridos") as String, Comun.MESSAGE_TYPE.Warning);
                return;
            }
            else
            {

                int idRol, idModulo;
                String pathList = String.Empty;
                ListItem item = listaSecciones.Items[0];

                if (item.Selected)
                {
                    pathList += "|";
                }

                for (int i = 1; i < listaSecciones.Items.Count; i++)
                {
                    item = listaSecciones.Items[i];

                    if (item.Selected)
                    {
                        pathList += item.Value + '|';
                    }
                }

                int.TryParse(ddlRancho.SelectedValue, out idRol);
                int.TryParse(ddlInvernadero.SelectedValue, out idModulo);
                guardaAsignacionPermisos(idRol, idModulo, pathList);
            }
        }
        catch (Exception es)
        {
            Log.Error(es);
        }
    }

   

    protected void listaSecciones_DataBound(object sender, EventArgs e)
    {
        foreach (ListItem item in listaSecciones.Items)
        {
            item.Text = HttpUtility.HtmlEncode(item.Text);
        }
    }
    #endregion

    #region Auxiliares
    private void ObtieneRanchos()
    {
       
        ddlRancho.DataSource = dataaccess.executeStoreProcedureDataSet("[procObtienePlantasActivas]", null);
        ddlRancho.DataTextField = "NombrePlanta";
        ddlRancho.DataValueField = "idPlanta";
        ddlRancho.DataBind();
    }

    private void ObtieneInvernaderosPorRancho(int idPlanta)
    {
        try
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            

            parameters.Add("@idPlanta", idPlanta);
            ddlInvernadero.DataSource = dataaccess.executeStoreProcedureDataTableFill("[procObtieneInvernaderosActivosPorRancho]", parameters);
            ddlInvernadero.DataTextField = "invernadero";
            ddlInvernadero.DataValueField = "idInvernadero";
            ddlInvernadero.DataBind();

           


        }
        catch (Exception e)
        {
            return;
        }

        //ddlModulo.DataSource = dataaccess.executeStoreProcedureDataSet("spr_SelectAllModulo", parameters);
        //ddlModulo.DataTextField = "vModulo";
        //ddlModulo.DataValueField = "idModulo";
        //ddlModulo.DataBind();
    }

    private void ObtieneSecciones()
    {
        listaSecciones.Items.Clear();

        if (ddlRancho.SelectedValue.Length == 0 || ddlInvernadero.SelectedValue.Length == 0)
        {
            return;
        }

        var parameters = new Dictionary<string, object>();

        parameters.Add("@idInvernadero", ddlInvernadero.SelectedValue);
        //parameters.Add("@Android", 0);

        var ds = dataaccess.executeStoreProcedureDataTable("procObtieneSeccionesYSurcosPorIdInvernadero", parameters);

        listaSecciones.Items.Add(new ListItem(ddlInvernadero.SelectedItem.Text, ddlInvernadero.SelectedValue));
        listaSecciones.Items[0].Selected = true;
        addSubmodules(ds, "idSeccion IS NOT NULL ", 1);
        listaSecciones.DataTextField = "numeroSeccion";
        listaSecciones.DataValueField = "idSeccion";
        listaSecciones.DataBind();


    }

    private void addSubmodules(DataTable table, String filter, int level)
    {
        ListItem listItem;
        int contado = 10;
        foreach (DataRow childItem in table.Select(filter))
        {
            
                listItem = new ListItem();
                listItem.Attributes.CssStyle.Add("padding-left", 20 * level + "px");
                listItem.Text = childItem["numeroSeccion"].ToString();
                listItem.Value = childItem["idSeccion"].ToString();
                //listItem.Selected = Convert.ToBoolean(childItem["tienePermiso"]);
                listaSecciones.Items.Add(listItem);
                //addSubmodules(table, "idSeccion = " + (int)childItem["idSeccion"], level + 1);

               
        }
    }

    private void guardaAsignacionPermisos(int idRol, int idModulo, string idSubModuloList)
    {
        if (Session["usernameInj"] == null)
            Response.Redirect("~/frmLogin.aspx", false);

        string msj = "";
        if (!string.IsNullOrEmpty(idSubModuloList))
        {
            idSubModuloList = "0" + idSubModuloList;
        }

        try
        {
            if (idRol <= 0)
            {
                msj += GetLocalResourceObject("SelectOneRole") as String;
            }

            if (idModulo <= 0)
            {
                msj += GetGlobalResourceObject("Commun", "SelectOneModule") as String;
            }

            if (msj.Trim().Length == 0)
            {
                //Error free, hace el insert...
                var parameters = new Dictionary<string, object>();

                parameters.Add("@idRol", idRol);
                parameters.Add("@idSubModuloList", idSubModuloList);
                parameters.Add("@idModulo", idModulo);
                dataaccess.executeStoreProcedureDataTable("spr_GUARDAR_AsignacionPermisos", parameters);
                popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "RecordSaved") as String, Comun.MESSAGE_TYPE.Success);
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(msj, Comun.MESSAGE_TYPE.Error);
            }
        }
        catch (Exception ex)
        {
            popUpMessageControl1.setAndShowInfoMessage(GetGlobalResourceObject("Commun", "DataDidNotSave") as String, Comun.MESSAGE_TYPE.Error);
            Log.Error(ex.ToString());
        }
    }
    #endregion
    protected void ddlRancho_SelectedIndexChanged(object sender, EventArgs e)
    {
        ObtieneInvernaderosPorRancho(Convert.ToInt32(ddlRancho.SelectedValue));
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        ObtieneSecciones();
    }
}