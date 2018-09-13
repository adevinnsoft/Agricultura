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

public partial class configuracion_Habilidad : BasePage
{
    static int idioma;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            llenaDdl();
           // llenaTabla();
            llenaPlantas();
            idioma = getIdioma();
        }
    }

    private void llenaPlantas()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasDeUsuario", new Dictionary<string, object>() { 
                { "@iduser", Session["idUsuario"] } 
            });
            ddlPlanta.DataSource = dt;
            foreach (DataRow D in dt.Rows)
            {
                ListItem L = new ListItem(D["Planta"].ToString(), D["idPlanta"].ToString());
                L.Attributes.Add("idPlanta", D["idPlanta"].ToString());
                ddlPlanta.Items.Add(L);
            }
            if (dt.Rows.Count == 0)
                ddlPlanta.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "NoDdlItems").ToString(), ""));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }

    public void llenaDdl()
    {
        try
        {
            DataTable dtdepartamento = dataaccess.executeStoreProcedureDataTable("spr_DepartamentosObtener", new Dictionary<string, object>() { { "@lengua", getIdioma() } });
            ddlDepartment.DataSource = dtdepartamento;
            foreach (DataRow D in dtdepartamento.Rows)
            {
                ListItem L = new ListItem(D["Departamento"].ToString(), D["idDepartamento"].ToString());
                L.Attributes.Add("idDepartamento", D["idDepartamento"].ToString());
                ddlDepartment.Items.Add(L);
            }
            if (dtdepartamento.Rows.Count == 0)
                ddlDepartment.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "NoDdlItems").ToString(), ""));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }
    }
    public void LimpiaCampos()
    {
        hdnIdHabilidad.Value = "";
        txtNameHabilidad.Text = "";
        hdnNombreCorto.Value = "";
        hdnNombreCorto_EN.Value = "";
        txtColorP.Text = "";
        chkActive.Checked = true;
        chkEjecutable.Checked = false;
        ddlDepartment.SelectedIndex = 0;
        txtNameHabilidad_EN.Text = "";
        txtColorP.CssClass = "required color {pickerFaceColor:'transparent',pickerFace:3,pickerBorder:0,pickerInsetColor:'black'}";

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        DataTable result;
        try
        {

            var planta = hdnPlanta.Value;

            parameters.Add("@idHabilidad", hdnIdHabilidad.Value.ToString());
            parameters.Add("@NombreHabilidad", txtNameHabilidad.Text);
            parameters.Add("@NombreCorto", hdnNombreCorto.Value.ToString().Trim());
            parameters.Add("@NombreCorto_EN", hdnNombreCorto_EN.Value.ToString().Trim());
            parameters.Add("@Color", txtColorP.Text);
            parameters.Add("@Activo", chkActive.Checked ? "True" : "False");
            parameters.Add("@Ejecutable", chkEjecutable.Checked ? "True" : "False");
            parameters.Add("@Usuario", Session["idUsuario"].ToString());
            parameters.Add("@idDepartamento", ddlDepartment.SelectedValue);
            parameters.Add("@NombreHabilidad_EN", txtNameHabilidad_EN.Text);
            parameters.Add("@idPlanta", planta);

            result = dataaccess.executeStoreProcedureDataTable("spr_GuardaHabilidad", parameters);

            if (result.Rows[0]["m1"].ToString() == "ok")
            {
                if (hdnIdHabilidad.Value.ToString().Length > 0 && !hdnIdHabilidad.Value.ToString().Equals("0"))
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Modificado").ToString(), Comun.MESSAGE_TYPE.Success);
                }
                else
                {
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Guardado").ToString(), Comun.MESSAGE_TYPE.Success);
                }
                LimpiaCampos();
               // llenaTabla();
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoGuardado").ToString() + result.Rows[0]["m2"].ToString(), Comun.MESSAGE_TYPE.Error);
            }

        }
        catch (Exception es)
        {
            Log.Error(es.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoGuardado").ToString(), Comun.MESSAGE_TYPE.Error);
        }
        finally
        {
            LimpiaCampos();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        LimpiaCampos();
    }

    [WebMethod]
    public static string targetPorProductos()
    {
        DataAccess dataaccess = new DataAccess();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_Productos", new Dictionary<string, object>());
        StringBuilder sb = new StringBuilder();
        foreach (DataRow P in dt.Rows)
        {
            sb.AppendLine("<div class=\"targetPorProducto\"><h3 class=\"lblNombreProducto\" idProducto=\"" + P["idProducto"] + "\">" + P["Descripcion"] + "</h3>");
            sb.AppendLine("<label class=\"lblTarget\" >* Target:</label><input type=\"text\"  class=\"txtTarget intValidate required\" />");
            sb.AppendLine("<h5 class=\"invisible\">Porcentaje de incremento al target por número de elementos.</h5> ");
            sb.AppendLine("<span class=\"porcenajesDeIncremento \">");
            //sb.AppendLine("    <span>1</span><input type=\"text\" />");
            sb.AppendLine("</span></div>");
        }
        return sb.ToString();
    }

    [WebMethod]
    public static string[] familiasYNiveles()
    {
        DataAccess dataaccess = new DataAccess();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_FamiliasYNiveles", new Dictionary<string, object>());
        StringBuilder sbFamilias = new StringBuilder();
        StringBuilder sbNiveles = new StringBuilder();
        DataView view = new DataView(dt);
        DataTable dtFamilias = view.ToTable(true, "idFamilia", "Familia");
        sbFamilias.AppendLine("<option value=\"0\">-- Seleccione --</option>");
        sbNiveles.AppendLine("<option value=\"0\">-- Seleccione --</option>");
        foreach (DataRow F in dtFamilias.Rows)
        {
            var idFamilia = F["idFamilia"];
            var Familia = F["Familia"];
            sbFamilias.AppendLine("<option value=\"" + idFamilia + "\">" + Familia + "</option>");
            foreach (DataRow N in dt.Select("idFamilia=" + idFamilia))
            {
                var idNivel = N["idNivel"];
                var nivel = N["Nivel"];
                sbNiveles.AppendLine("<option style=\"display:none;\" idFamilia=\"" + idFamilia + "\" value=\"" + idNivel + "\">" + nivel + "</option>");
            }
        }
        return new string[] { sbFamilias.ToString(), sbNiveles.ToString() };
    }
    [WebMethod]
    public static string HerramientasYMateriales()
    {
        DataAccess dataaccess = new DataAccess();
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_HerramientasYMateriales", new Dictionary<string, object>());
        StringBuilder sb = new StringBuilder();
        foreach (DataRow F in dt.Rows)
        {
            var idCategoria = F["idCategoria"];
            var Categoria = F["NombreCategoria"];
            var idMaterial = F["idArticulo"];
            var Material = F["NombreArticulo"];
            var UnidadDeMedida = F["Simbolo"];
            sb.AppendLine("<tr><td>" + Categoria + "</td><td>" + Material + "</td><td><input type=\"text\" idMaterial=\"" + idMaterial + "\" class=\"intValidate \" value=\"\" ></input></td><td>" + UnidadDeMedida + "</td><td class=\"invisible\"><img src=\"../comun/img/remove-icon.png\" onclick=\"$(this).parent().parent().remove();\"></td></tr>");

        }
        return sb.ToString();
    }
    [WebMethod]
    public static string AlmacenarHabilidad(Habilidad habilidad)
    {
        try
        {
            Dictionary<string, object> prm = new Dictionary<string, object>();
            prm.Add("@idHabilidad", habilidad.idHabilidad);            
            prm.Add("@activo", habilidad.activo ? 1 : 0);
            prm.Add("@nombre", habilidad.nombreActividad);
            prm.Add("@nombreEN", habilidad.nombreActividadEN);
            prm.Add("@codigo", habilidad.codigo);
            prm.Add("@codigoEN", habilidad.codigoEN);
            prm.Add("@color", habilidad.color);
            prm.Add("@ejecutable", habilidad.ejecutable);

            Type habilidadNivelType = typeof(HabilidadNivel);
            DataTable dtHabilidadNivel = getDataTableByTypeClass(habilidadNivelType);
            Type MaterialesType = typeof(HerramientasMateriales);
            DataTable dtMateriales = getDataTableByTypeClass(MaterialesType);
            Type TargetType = typeof(Target);
            DataTable dtTarget = getDataTableByTypeClass(TargetType);
            Type PorcentajeType = typeof(PorcentajePorElemento);
            DataTable dtPorcentaje = getDataTableByTypeClass(PorcentajeType);


            DataTable dtPlantas = new DataTable();
            dtPlantas.Columns.Add("idPlanta");
            dtPlantas.Columns.Add("descripcion");
            dtPlantas.Columns.Add("padre");
            //dtPlantas.Columns.Add("indice");
            foreach (int planta in habilidad.plantas)
            {
                DataRow r = dtPlantas.NewRow();
                r["idPlanta"] = planta;
                dtPlantas.Rows.Add(r);
            }

            DataTable dtDepartamentos = new DataTable();
            dtDepartamentos.Columns.Add("idDepartamento");
            dtDepartamentos.Columns.Add("descripcion");
            dtDepartamentos.Columns.Add("padre");
            //dtPlantas.Columns.Add("indice");
            foreach (int Departamento in habilidad.departamentos)
            {
                DataRow r = dtDepartamentos.NewRow();
                r["idDepartamento"] = Departamento;
                dtDepartamentos.Rows.Add(r);
            }
            int nivelCount = 0;
            int targetPadre = 0;
            foreach (HabilidadNivel Nivel in habilidad.niveles)
            {
                DataRow rNivel = dtHabilidadNivel.NewRow();
                rNivel["nombre"] = Nivel.nombre;
                rNivel["nombreEN"] = Nivel.nombreEN;
                rNivel["activo"] = Nivel.activo ? 1 : 0;
                rNivel["elemento"] = Nivel.elemento;
                rNivel["elementoEN"] = Nivel.elementoEN;
                rNivel["idNivel"] = Nivel.idNivel;
                rNivel["padre"] = null;
                rNivel["indice"] = ++nivelCount;
                dtHabilidadNivel.Rows.Add(rNivel);
                foreach (HerramientasMateriales Material in Nivel.herramientasYMateriales)
                {
                    DataRow rMaterial = dtMateriales.NewRow();
                    rMaterial["idMaterial"] = Material.idMaterial;
                    rMaterial["cantidad"] = Material.cantidad;
                    rMaterial["padre"] = nivelCount;
                    rMaterial["indice"] = null;
                    dtMateriales.Rows.Add(rMaterial);
                }
                foreach (Target TargetXProducto in Nivel.targetXproducto)
                {
                    DataRow rTarget = dtTarget.NewRow();
                    rTarget["idProducto"] = TargetXProducto.idProducto;
                    rTarget["target"] = TargetXProducto.target;
                    rTarget["padre"] = nivelCount;
                    rTarget["indice"] = ++targetPadre;
                    dtTarget.Rows.Add(rTarget);
                    foreach (PorcentajePorElemento PorcentajeXelemento in TargetXProducto.porcentajesPorElemento)
                    {
                        DataRow rPorcentaje = dtPorcentaje.NewRow();
                        rPorcentaje["cantidadDeElementos"] = PorcentajeXelemento.cantidadDeElementos;
                        rPorcentaje["porcentaje"] = PorcentajeXelemento.porcentaje;
                        rPorcentaje["padre"] = targetPadre;
                        rPorcentaje["indice"] = null;
                        dtPorcentaje.Rows.Add(rPorcentaje);
                    }
                }
            }
            prm.Add("@niveles", dtHabilidadNivel);
            prm.Add("@materiales", dtMateriales);
            prm.Add("@target", dtTarget);
            prm.Add("@porcentajes", dtPorcentaje);
            prm.Add("@plantas", dtPlantas);
            prm.Add("@departamentos", dtDepartamentos);
            prm.Add("@idUsuario", HttpContext.Current.Session["idUsuario"]);

            DataAccess da = new DataAccess();
            DataTable dt = da.executeStoreProcedureDataTable("spr_HabilidadInsertar", prm);
            if (dt.Rows[0]["Estado"].ToString().Equals("1"))
            {
                if (habilidad.idHabilidad > 0)
                    return "Se modificó correctamente la habilidad y sus etapas.|ok";
                else
                return "Se almacenó correctamente la habilidad y sus etapas.|ok";
            }
            else if (dt.Rows[0]["Estado"].ToString().Equals("0"))
            {
                return "Ya existe una habilidad con el mismo nombre o código.|error";
            }
            else
            {
                return "Se ocasionó un error en el guardado de información. Consulte con el administrador del sistema.|error";
            }
            //return string.Empty;

        }
        catch (Exception x)
        {
            return "Se ocasionó un error en el procesamiento de información. Consulte con el administrador del sistema.|error";
        }
    }


    [WebMethod]
    public static string HabilidadesGuardadas()
    {
        DataAccess da = new DataAccess();
        DataTable dt= da.executeStoreProcedureDataTable("spr_ObtieneHabilidadesGridView", new Dictionary<string, object>() { { "@idioma", idioma}, {"@idUsuario", HttpContext.Current.Session["idUsuario"]} });
        StringBuilder sb = new StringBuilder();
        //sb.AppendLine("<table class=\"gridView tablesorter tablesorter-default hasFilters\" cellspacing=\"0\" rules=\"all\" border=\"1\" >");
        //sb.AppendLine("<thead><tr><th>Activo</th><th>Habilidad</th><th>Clave</th><th>Color</th><th>Ejecutable</th><th>Planta</th><th>Departamento</th></thead><tbody>");
        foreach (DataRow R in dt.Rows)
        {
            sb.AppendLine(string.Format("<tr onClick=\"cargarFormularioDeHabilidad({0});\"><td>{1}</td><td>{2}</td><td>{3}</td><td><div style=\"width: 16px; height: 16px; margin:0 auto; background-color: #{4}\"></div></td><td>{5}</td><td>{6}</td><td>{7}</td></tr>",
                R["IdHabilidad"],
                R["Activo"].ToString().ToLower().Equals("true") ? "Si":"No", 
                R["NombreHabilidad"], 
                R["NombreCorto"], 
                R["Color"],
                R["SoloEjecutable"].ToString().ToLower().Equals("true") ? "Si" : "No", 
                R["NombrePlanta"], 
                R["NombreDepartamento"] ));
        }
        //sb.AppendLine("</tbody>");
        return sb.ToString();
    }
    
    public static DataTable getDataTableByTypeClass(Type myType)
    {
        DataTable dt = new DataTable();
        foreach (FieldInfo item in myType.GetFields())
        {
            if (!item.FieldType.Name.ToString().Contains("[]"))
                dt.Columns.Add(item.Name.ToString());
        }
        dt.Columns.Add("padre");
        dt.Columns.Add("indice");
        return dt;
    }
    [WebMethod]
    public static string obtenerHabilidadPorId(int idHabilidad)
    {
        DataAccess dataaccess = new DataAccess();
        var result = dataaccess.executeStoreProcedureDataTable("spr_HabilidadXMLPorId", new Dictionary<string, object>() { { "@idHabilidad", idHabilidad } });
        var stringBuilder = new StringBuilder();

        foreach (DataRow row in result.Rows)
        {
            stringBuilder.Append(row[0]);
        }

        var xml = stringBuilder.ToString();
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Habilidad));
        var reader = new StringReader(xml);

        var objeto = (Habilidad)serializer.Deserialize(reader);
        return new JavaScriptSerializer().Serialize(objeto);
    }
    #region obsoleto
    /*
    public void llenaTabla()
    {
        try
        {
            gvHabilidad.DataSource = dataaccess.executeStoreProcedureDataTable("spr_ObtieneHabilidadesGridView", new Dictionary<string, object>() { { "@idioma", getIdioma() } });
            gvHabilidad.DataBind();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
    
    protected void gvHabilidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id;
            DataTable dt;
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (null != gvHabilidad.SelectedPersistedDataKey)
            {
                Int32.TryParse(gvHabilidad.SelectedPersistedDataKey["IdHabilidad"].ToString(), out id);
            }
            else
            {
                Int32.TryParse(gvHabilidad.SelectedDataKey["IdHabilidad"].ToString(), out id);
            }

            parameters.Add("@idHabilidad", id);

            dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneHabilidadEditar", parameters);

            hdnIdHabilidad.Value = dt.Rows[0]["idHabilidad"].ToString();
            ddlDepartment.SelectedValue = dt.Rows[0]["IdDepartamento"].ToString();
            txtNameHabilidad.Text = dt.Rows[0]["NombreHabilidad"].ToString();
            txtColorP.Text = dt.Rows[0]["Color"].ToString();
            chkActive.Checked = dt.Rows[0]["Activo"].ToString() == "True" ? true : false;
            hdnNombreCorto.Value = dt.Rows[0]["NombreCorto"].ToString();
            chkEjecutable.Checked = dt.Rows[0]["SoloEjecutable"].ToString() == "True" ? true : false;
            hdnNombreCorto_EN.Value = dt.Rows[0]["NombreCorto_EN"].ToString();
            txtNameHabilidad_EN.Text = dt.Rows[0]["NombreHabilidad_EN"].ToString();

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

    }
    
    protected void gvHabilidad_PreRender(object sender, EventArgs e)
    {
        if (gvHabilidad.HeaderRow != null)
            gvHabilidad.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void gvHabilidad_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvHabilidad, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
    */
    #endregion 
    
}