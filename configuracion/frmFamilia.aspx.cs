using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using log4net;
using System.Globalization;
using System.Web.Services;

public partial class configuracion_frmFamilia : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(configuracion_frmFamilia));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                obtieneFamilia();
                Actualizar_Nivel();
                //ObtienePlantas();
                //ObtieneDepartamentos();
               
            }
            else
            {
                //Sin Actividad
            }
        }
        catch (Exception Exception)
        { log.Error(Exception.ToString()); }
    }

    private void obtieneFamilia()
    {
        //DataTable dt1 = dataaccess.executeStoreProcedureDataTable("spr_PlantaObtener",null); // Retorna todas las sucursales
        
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_FamiliaObtener", parameters);
        gv_Familia.DataSource = dt;
        ViewState["dsMerma"] = dt;
        gv_Familia.DataBind();
        //throw new NotImplementedException();
    }
  
   
    private void Actualizar_Nivel()
    {
        
    }

    //Métodos del grid
    public int cant_niveles,cadena_Niveles;
    protected void gv_Familia_SelectedIndexChanged(object sender, EventArgs e)
    {

        Session["IdModuloCookie"] = gv_Familia.DataKeys[gv_Familia.SelectedIndex].Value.ToString();
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@IdFamilia", Session["IdModuloCookie"]);
        //DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_HabilidadEtapaObtener", parameters);
        DataSet ds = dataaccess.executeStoreProcedureDataSet("spr_SelectFromFamiliaId", parameters);
        DataTable dt1 = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];
        if (dt1.Rows.Count > 0)
        {
            txtFam.Text = dt1.Rows[0]["Vfamilia"].ToString().Trim();
            txtFam_EN.Text = dt1.Rows[0]["vFamilia_EN"].ToString().Trim();
            txtNivel.Text = dt1.Rows[0]["Num_Niveles"].ToString().Trim();
            cant_niveles = int.Parse(txtNivel.Text);

            foreach (DataRow Item in dt2.Select())
            {
                //idNivel,idFamilia,UsuarioModifico,FechaModifico,bActivo
               
                cadenaNiveles += Item["Nivel"].ToString();
                cadenaNiveles += "|";
                cadenaNiveles += Item["idFamilia"].ToString();
                cadenaNiveles += "|";
                cadenaNiveles += Item["NombreNivel"].ToString();
                cadenaNiveles += "|";
                cadenaNiveles += Item["bActivo"].ToString();
                cadenaNiveles += "]";
                //Client();
            }
            ClientScript.RegisterStartupScript(this.GetType(), "myScript", "crearTabla();", true);
            Actualizar_Nivel();
            //txtRuta.Text = dt.Rows[0]["ruta"].ToString().Trim();
            if (dt1.Rows[0]["bActivo"].ToString().Equals("True"))
                idActivo.Checked = true;
            else
                idActivo.Checked = false;

            Accion.Value = "Guardar Cambios";
            //btn_Enviar.Visible = true;
            //btnCancelar.Visible = true;
            btnAgregar_nivel.Value = "Actualizar";//GetLocalResourceObject("Editar").ToString();
            btnCancelar.Text = "Cancelar";// GetLocalResourceObject("Cancelar").ToString();
        }
        else
        {
            //No se encontró el registro
        }
    }
    public string firstName = "Manas"; 
    public string lastName = "Bhardwaj";
    private String cadenaNiveles2 = "";
    public String cadenaNiveles = "";
    //public string Client(){return cadenaNiveles; }
    public string Clientdd { get { return cadenaNiveles2; } }

    protected void gv_Familia_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsFamilia"])
            {
                DataSet ds = ViewState["dsFamilia"] as DataSet;

                if (ds != null)
                {
                    gv_Familia.DataSource = ds;
                    gv_Familia.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception)
        {
        }

    }
    protected void gv_Familia_PreRender(object sender, EventArgs e)
    {
        if (gv_Familia.HeaderRow != null)
            gv_Familia.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void gv_Familia_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gv_Familia, ("Select$" + e.Row.RowIndex.ToString()));
                break;
        }
    }






    protected void idtxtDepto_TextChanged(object sender, EventArgs e)
    {

    }
    protected void lista_plantas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //ObtienePlantas();
        }
        catch (Exception exception)
        {
            Log.Error(exception.ToString());
        }
    }

    public int cant_tablas = 0;
    protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    //public static string[] mensaje = new string[2];
    //public static string[] mensaje = {"",""};
    [WebMethod(EnableSession = true)]
    public static string[] web_met(string txtFam, string txtFam_EN, string txtNivel, Boolean idActivo, string hddAccion,Boolean agregados,string cadnewNivels, string NivelsSelec)
    {
        string[] mensaje = new string[2];
        try
        {
            //Creamos dataTable de los niveles
            DataTable dtNiveles = new DataTable();
            DataRow row;
            dtNiveles.Columns.Add("Nivel", typeof(int));
            dtNiveles.Columns.Add("Activo", typeof(int));
            dtNiveles.Columns.Add("Nombre", typeof(string));

            string[] niveles = NivelsSelec.Split(']');
            foreach (string n in niveles)
            {
                if (n != "")
                {
                    string[] campos = n.Split('|');
                    row = dtNiveles.NewRow();
                    row["Nivel"] = campos[0];
                    row["Activo"] = campos[1];
                    row["Nombre"] = campos[2];
                    dtNiveles.Rows.Add(row);
                }
            }

            //mensaje[mensaje.Length + 1] = "hola";
            DataAccess dataaccess = new DataAccess();
            if (agregados == true)
            {
                //Creamos dataTable para los niveles agregados
                DataTable dtAgregados = new DataTable();
                DataRow rowA;
                dtAgregados.Columns.Add("Nivel", typeof(int));
                dtAgregados.Columns.Add("Activo", typeof(int));
                dtAgregados.Columns.Add("Nombre", typeof(string));

                string[] nagregados = cadnewNivels.Split(']');
                foreach (string na in nagregados)
                {
                    if (na != "")
                    {
                        string[] cagregados = na.Split('|');
                        rowA = dtAgregados.NewRow();
                        rowA["Nivel"] = cagregados[0];
                        rowA["Activo"] = cagregados[1];
                        rowA["Nombre"] = cagregados[2];
                        dtAgregados.Rows.Add(rowA);
                    }
                }

                Dictionary<string, object> parnivelsnew = new System.Collections.Generic.Dictionary<string, object>();
                parnivelsnew.Add("@idsnews", dtAgregados);
                //parnivelsnew.Add("@IdFamilia", Session["IdModuloCookie"]);
                parnivelsnew.Add("@IdFamilia", HttpContext.Current.Session["IdModuloCookie"].ToString());
                parnivelsnew.Add("@UsuarioModifico", HttpContext.Current.Session["idUsuario"]);
                String Res = dataaccess.executeStoreProcedureString("spr_NuevosNiveles", parnivelsnew);
                if (Res.Equals("error"))
                {
                    mensaje[0] = "No pudo agregar los nuevos niveles";
                    mensaje[1] = "info";
                    return mensaje;
                }
            }
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@Familia", txtFam);
            parameters.Add("@Familia_EN", txtFam_EN);
            parameters.Add("@NumNiveles", txtNivel);
            parameters.Add("@UsuarioModifico", HttpContext.Current.Session["idUsuario"]);
            parameters.Add("@Activo", idActivo);
            parameters.Add("@NivelesSeleccionados", dtNiveles);

            if (hddAccion == "Añadir")
            {
                //Verificar que el valor "Razón" a insertar no estan anteriormente agregados
                Dictionary<string, object> find = new System.Collections.Generic.Dictionary<string, object>();
                find.Add("@Familia", txtFam);
                find.Add("@Familia_EN", txtFam_EN);
                find.Add("@UsuarioModifico", HttpContext.Current.Session["idUsuario"]);
                if (dataaccess.executeStoreProcedureGetInt("spr_ExisteFamilia", find) > 0)
                {
                    //popUpMessageControl1.setAndShowInfoMessage("No se efectuarán cambios debido a que la Familia ya existe.", Comun.MESSAGE_TYPE.Info);
                    mensaje[0] = "No se efectuarán cambios debido a que la Familia ya existe";
                    mensaje[1] = "info";
                    return mensaje;
                }
                else
                {
                    String Rs = dataaccess.executeStoreProcedureString("spr_FamiliaInsertar", parameters);
                    if (Rs.Equals("repetido"))
                    {
                        //popUpMessageControl1.setAndShowInfoMessage("Ese registro ya había sido capturado.", Comun.MESSAGE_TYPE.Error);
                        mensaje[0] = "Ese registro ya había sido capturado.";
                        mensaje[1] = "info";
                        return mensaje;
                    }
                    else if (Rs.Equals("ok"))
                    {
                        //popUpMessageControl1.setAndShowInfoMessage("La Familia  \"" + txtFam + "\" se guardó exitosamente.", Comun.MESSAGE_TYPE.Success);
                        mensaje[0] = "La Familia  \"" + txtFam + "\" se guardó exitosamente.";
                        mensaje[1] = "ok";
                        return mensaje;
                    }
                    else
                    {
                        mensaje[0] = "La Familia  \"" + txtFam + "\" no se guardó exitosamente.";
                        mensaje[1] = "error";
                        return mensaje;
                    }
                }
            }
            else
            {
                if (HttpContext.Current.Session["IdModuloCookie"] == null || HttpContext.Current.Session["IdModuloCookie"].ToString() == "")
                {
                    //popUpMessageControl1.setAndShowInfoMessage("Error #RGRL02: Se perdió la información del ID actual", Comun.MESSAGE_TYPE.Error);
                    mensaje[0] = "La Familia  \"" + txtFam + "\" no se guardó exitosamente.";
                    mensaje[1] = "error";
                    return mensaje;
                }
                else
                {
                    parameters.Add("@IdFamilia", HttpContext.Current.Session["IdModuloCookie"].ToString());
                    //parameters.Add("@NivelesSeleccionados", NivelsSelec);
                    //ClientScript.RegisterStartupScript(this.GetType(), "myScript", "NivSel();", true);
                    //Response.Write("<script type='text/javascript'>NivSel();</script>");
                    //Response.End();
                    String cad1 = NivelsSelec;
                    String Rs = dataaccess.executeStoreProcedureString("spr_UpdateFamilia", parameters);
                    if (Rs.Equals("error"))
                    {
                        //popUpMessageControl1.setAndShowInfoMessage("No existieron cambios en la Familia, por registro similar.", Comun.MESSAGE_TYPE.Info);
                        mensaje[0] = "No se pudo modificar la familia.";
                        mensaje[1] = "error";
                        return mensaje;
                    }
                    else
                    {
                        //popUpMessageControl1.setAndShowInfoMessage("La Familia fue modificada.", Comun.MESSAGE_TYPE.Success);
                        mensaje[0] = "La Familia fue modificada.";
                        mensaje[1] = "ok";
                        return mensaje;

                    }

                }
            }
            //obtieneFamilia();

            //VolverAlPanelInicial();
            //}
        }
        catch (Exception x)
        {
            log.Error(x);
            mensaje[0] = "Error al guardar la familia.";
            mensaje[1] = "error";
            return mensaje;
        }
    }
    [WebMethod(EnableSession = true)]
    public static string wmcadenaNivel(string txtFam)
    {
        return "";
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        idActivo.Checked = true;
        btnAgregar_nivel.Value = "Guardar";
        if (Accion.Value == "Añadir")
        {
            txtFam.Text = "";
            txtFam_EN.Text = "";
            txtNivel.Text = "";

        }
        else
        {
            Accion.Value = "Añadir";
            //btn_Enviar.Text = "Guardar";
            btnCancelar.Text = "Limpiar";
            txtFam.Text = "";
            txtFam_EN.Text = "";
            txtNivel.Text = "";
        } 
        btnAgregar_nivel.Value = "Guardar";
        
    }
}
 