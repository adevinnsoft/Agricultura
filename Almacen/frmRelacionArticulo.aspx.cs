using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Globalization;
using log4net;
using System.Text;
using System.IO;
using System.Web.Script.Serialization;

public partial class Almacen_frmRelacionArticulo : BasePage
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Almacen_frmRelacionArticulo));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //obtieneArt();
                ObtieneCategoria();
                obtieneUnidad();


            }
            else
            {
                //Sin Actividad
            }
        }
        catch (Exception Exception)
        { log.Error(Exception.ToString()); }
    }

    private void obtieneUnidad()
    {
        ////ddlPlantas.Items.Clear();
        //var parameters = new Dictionary<string, object>();

        ////parameters.Add("@ACTIVO", true);
        //parameters.Add("@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        //ddlUnidad.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "Select") as String, string.Empty));

        //try
        //{
        //    //parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        //    ddlUnidad.DataSource = dataaccess.executeStoreProcedureDataSet("spr_UnidadObtener", parameters);
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(ex.ToString());

        //    //return false;     //Esta variable se puede utilizar como un retorno de si existió un error y efectuar una acción
        //}

        //ddlUnidad.DataTextField = "Nombre";
        //ddlUnidad.DataValueField = "IdUnidadMedida";
        //ddlUnidad.DataBind();

        ////return true;            //Esta variable se puede utilizar como un retorno de si existió un error y efectuar una acción
        ////throw new NotImplementedException();
    }

    private void ObtieneCategoria()
    {
        //ddlCategoria.Items.Clear();
        //var parameters = new Dictionary<string, object>();

        ////parameters.Add("@ACTIVO", true);
        //parameters.Add("@idioma", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        //ddlCategoria.Items.Add(new ListItem(GetGlobalResourceObject("Commun", "Select") as String, string.Empty));

        //try
        //{
        //    //parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        //    ddlCategoria.DataSource = dataaccess.executeStoreProcedureDataSet("spr_CategoriaObtener", parameters);
        //}
        //catch (Exception ex)
        //{
        //    Log.Error(ex.ToString());

        //    //return false;     //Esta variable se puede utilizar como un retorno de si existió un error y efectuar una acción
        //}

        //ddlCategoria.DataTextField = "NombreCategoria";
        //ddlCategoria.DataValueField = "IdCategoriaArticulo";
        //ddlCategoria.DataBind();
        ////throw new NotImplementedException();
    }
    private void obtieneArt()
    {
        //DataTable dt1 = dataaccess.executeStoreProcedureDataTable("spr_PlantaObtener",null); // Retorna todas las sucursales

        ////////////////////////////////////////////Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        ////////////////////////////////////////////parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        ////////////////////////////////////////////DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ArticuloObtener", parameters);
        ////////////////////////////////////////////gv_Articulo.DataSource = dt;
        ////////////////////////////////////////////ViewState["dsArticulo"] = dt;
        ////////////////////////////////////////////gv_Articulo.DataBind();
        //////////////////////////////////////////////throw new NotImplementedException();
    }
    ///////////
    //Métodos del grid
    public int cant_niveles, cadena_Niveles;
    protected void gv_Articulo_SelectedIndexChanged(object sender, EventArgs e)
    {

        
    }

    private String cadenaNiveles2 = "";
    public String cadenaNiveles = "";
    //public string Client(){return cadenaNiveles; }
    public string Clientdd { get { return cadenaNiveles2; } }

    protected void gv_Articulo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //try
        //{
        //    if (null != ViewState["dsFamilia"])
        //    {
        //        DataSet ds = ViewState["dsFamilia"] as DataSet;

        //        if (ds != null)
        //        {
        //            gv_Familia.DataSource = ds;
        //            gv_Familia.DataBind();
        //        }
        //    }
        //    ((GridView)sender).PageIndex = e.NewPageIndex;
        //    ((GridView)sender).DataBind();
        //}
        //catch (Exception)
        //{
        //}

    }
    protected void gv_Articulo_PreRender(object sender, EventArgs e)
    {
        //if (gv_Familia.HeaderRow != null)
        //    gv_Familia.HeaderRow.TableSection = TableRowSection.TableHeader;
    }
    protected void gv_Articulo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //switch (e.Row.RowType)
        //{
        //    case DataControlRowType.DataRow:
        //        e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gv_Familia, ("Select$" + e.Row.RowIndex.ToString()));
        //        break;
        //}
    }


 
    
    protected void btn_Cancelar_Click(object sender, EventArgs e)
    {
       
    }
    public static int contadorTablas = 0;
    [WebMethod]
    public static string tablaStockPlantas()
    {
        DataAccess da = new DataAccess();
        DataSet ds = da.executeStoreProcedureDataSet("spr_PlantasDepartamentos", new Dictionary<string, object>() { });
        DataTable dt1 = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];
        DataTable dt3 = ds.Tables[2];
        DataTable dt4 = ds.Tables[3];
        StringBuilder sb=new StringBuilder();
        StringBuilder sbini=new StringBuilder();
        sb.AppendLine("<div class=\"articulo\"><table class=\"articuloCabecera\"><tr></tr><tr><th rowspan=\"2\" style=\"text-align: center;\"><label class=\"lblArtES\">*Nombre:</label>" +
                                "</th><td><input type=\"text\"  class=\"txtArtES\" MaxLength=\"150\"/> </td><td><label class=\"lblActivo\">Activo</label></td>"+
                                "<td><input type=\"checkbox\" class=\"Activo\" value=\"Activo\" checked/></td><td ><img onclick=\"EliminarFormularioArticulo($(this))\" alt=\"Eliminar Artículo\" class=\"style2 EliminarArt\" src=\"../comun/img/error.png\" /></td></tr>" +
                                "<tr><td><input type=\"text\"  class=\"txtArtENs\" MaxLength=\"150\"/></td><td align=\"right\"><label class=\"ltCategoria\">*Categoría:</label>"+
                                "</td><td align=\"left\"><select  Class=\"ddlCategoria\">");//onclick=\"return Select1_onclick()\"
        sb.AppendLine("<option value=\"default\">--Seleccione--</option>");
                                foreach (DataRow Itemcat in dt3.Select())
                                {
                                    sb.AppendLine("<option value=\"" + Itemcat["IdCategoriaArticulo"] + "\">" + Itemcat["NombreCategoria"] + "</option>");
                                }
                                sb.AppendLine("</select></td></tr>");
                                sb.AppendLine("<tr><td>&nbsp;</td></tr>" +
                                "<tr><th rowspan=\"2\" style=\"text-align: center;\"><label class=\"lblArtES\">*Descripción:</label></th>"+
                                "<td><textarea  Class=\"txtDescripcion_ES\" MaxLength=\"200\"></textarea></td><td align=\"right\"><label class=\"litUnidad\">*Unidad:</label></td>" +
                                "<td><select  Class=\"ddlUnidad\">");
                                sb.AppendLine("<option value=\"0\">--Seleccione--</option>");
                                foreach (DataRow Itemuni in dt4.Select())
                                {
                                    sb.AppendLine("<option value=\"" + Itemuni["IdUnidadMedida"] + "\">" + Itemuni["Nombre"] + "</option>");
                                }
                                sb.AppendLine("</select></td></tr>");
                                //<option value=\"1\">Unidad</option></select></td>"+
                                //sb.AppendLine("<td>&nbsp;</td></tr>"+
                                sb.AppendLine("<tr><td class=\"style1\"><textarea  Class=\"txtDescripcion_EN\" MaxLength=\"200\"></textarea></td></tr></table>");
        //
        //
        sb.AppendLine("<h3 class=\"headerAcordion\">Stock por Planta</h3><table class=\"articuloStock\" style='display:none;'><thead><tr ID=" + contadorTablas + "><th ID=Omar>Planta</th><th>Departamento</th><th>Stock Actual</th><th>Asignación</th><th>Final</th><th>Motivo</th></tr></thead><tbody>");///required intValidate
        foreach (DataRow P in dt1.Rows)
        {
            sb.AppendLine("<tr><td idPlanta=\"" + P["idPlanta"] + "\">" + P["NombrePlanta"] + "</td><td idDepartamento=\"" + P["idDepartamento"] + "\">" + P["NombreDepartamento"] + "</td><td class=\"stockActual\">0</td><td><input type=\"text\" maxlength=\"50\" class=\"required int312\" onchange=\"ValidarNumeroNegativo($(this))\" /></td><td><label class=\"Final\"></label></td><td><select  Class=\"Motivo\"><option value=\"0\">--Seleccione--</option>");    //onclick=\"return Select1_onclick()\"
             foreach (DataRow Item in dt2.Select())
             {
                 sb.AppendLine("<option  style='display:none;' operacion=\"" + Item["TipoOperacion"] + "\" value=\"" + Item["idMotivo"] + "\">" + Item["Motivo"] + "</option>");
             }
             sb.AppendLine("</select></td></tr>");
        }
        sb.AppendLine("</tbody></table></div>");
        return sb.ToString();
    }
    [WebMethod]
    public static string[] AlmacenarArticulo(Articulo[] articulo)
    {
        string[] mensaje = new string[2];
        

            if (articulo.Length == 0)
            {
                mensaje[0] = "No se realizaron cambios.";
                mensaje[1] = "warning";
                return mensaje;
            }
            else
            {
                try
                {
                    Dictionary<string, object> prm = new Dictionary<string, object>();

                    DataTable articulos = new DataTable();
                    articulos.Columns.Add("ArticuloES");
                    articulos.Columns.Add("ArticuloEN");
                    articulos.Columns.Add("DescripcionES");
                    articulos.Columns.Add("DescripcionEN");
                    articulos.Columns.Add("idCategoria");
                    articulos.Columns.Add("idUnidad");
                    articulos.Columns.Add("Activo");
                    articulos.Columns.Add("Indice");
                    articulos.Columns.Add("idarticulo");
                    articulos.Columns.Add("stocksplantas");

                    DataTable StocksPlantas = new DataTable();
                    StocksPlantas.Columns.Add("idPlanta");
                    StocksPlantas.Columns.Add("idDpto");
                    StocksPlantas.Columns.Add("Asignacion");
                    StocksPlantas.Columns.Add("Padre");
                    StocksPlantas.Columns.Add("idMotivo");
                    StocksPlantas.Columns.Add("IndiceMov");
                    StocksPlantas.Columns.Add("IdArticuloPlanta");

                    //DataTable ArticuloMotivo = new DataTable();
                    //ArticuloMotivo.Columns.Add("idMotivo");

                    int ContadorDeArticulos = 0;
                    int ContadorPorPlanta = 0;
                    foreach (var art in articulo)
                    {
                        DataRow dr = articulos.NewRow();
                        if (art.idarticulo == null)
                        { //Inserción de un registro nuevo
                        }
                        else
                        {
                            dr["idarticulo"] = art.idarticulo;
                        }

                        dr["ArticuloES"] = art.ArticuloES;
                        dr["ArticuloEN"] = art.ArticuloEN;
                        dr["DescripcionES"] = art.DescripcionES;
                        dr["DescripcionEN"] = art.DescripcionEN;
                        dr["idCategoria"] = art.idCategoria;
                        dr["idUnidad"] = art.idUnidad;
                        dr["Activo"] = art.Activo;
                        dr["Indice"] = ++ContadorDeArticulos;
                        articulos.Rows.Add(dr);
                        foreach (var sto in art.stocksplantas)
                        {
                            DataRow dr2 = StocksPlantas.NewRow();
                            //DataRow dr3 = ArticuloMotivo.NewRow();
                            dr2["idPlanta"] = sto.idPlanta;
                            dr2["idDpto"] = sto.idDpto;
                            dr2["Asignacion"] = sto.Asignacion;
                            //dr2["Motivo"] = sto.Motivo;
                            dr2["Padre"] = ContadorDeArticulos;
                            dr2["idMotivo"] = sto.idMotivo;
                            dr2["IndiceMov"] = ++ContadorPorPlanta;
                            dr2["IdArticuloPlanta"] = sto.IdArticuloPlanta;


                            StocksPlantas.Rows.Add(dr2);
                            //ArticuloMotivo.Rows.Add(dr3);
                        }
                    }

                    prm.Add("@idUsuario", HttpContext.Current.Session["idUsuario"]);
                    prm.Add("@articulos", articulos);
                    prm.Add("@stockPlantas", StocksPlantas);

                    DataAccess da = new DataAccess();
                    DataTable dt = da.executeStoreProcedureDataTable("spr_ArticulosInsertar", prm);
                    switch (int.Parse(dt.Rows[0]["Estado"].ToString()))
                    {
                        case 0:
                            mensaje[0] = "No se efectuarán cambios en los artículos, debido a un error interno";
                            mensaje[1] = "error";
                            //return mensaje;
                            break;
                        case 1:
                            mensaje[0] = "Los datos se almacenaron correctamente";
                            mensaje[1] = "ok";
                            //return mensaje;
                            break;
                        case 2:
                            mensaje[0] = "No se pueden registrar datos con el mismo nombre";
                            mensaje[1] = "error";
                            break;
                        case 3:
                            mensaje[0] = "Existen artículos con el mismo nombre, favor de verificar la información";
                            mensaje[1] = "error";
                            break;
                        case 4:
                            mensaje[0] = "Registros repetidos dentro de la captura de artículos";
                            mensaje[1] = "error";
                            break;
                        default:
                            break;
                    }
                    return mensaje;
                    //if (dt.Rows[0]["Estado"].ToString().Equals("0"))
                    //{
                    //    mensaje[0] = "No se efectuarán cambios en los artículos, debido a un error interno";
                    //    mensaje[1] = "error";
                    //    return mensaje;
                    //}
                    //else
                    //{
                    //    mensaje[0] = "Los datos se almacenaron correctamente";
                    //    mensaje[1] = "ok";
                    //    return mensaje;
                    //}
                    //return mensaje;
                }
                catch (Exception)
                {

                    mensaje[0] = "No se Almacenaron los datos registrados";
                    mensaje[1] = "error";
                    return mensaje;
                }
            }
        

    }
    [WebMethod]
    public static string ObtenerArticulosAlmacenados()
    { 
        StringBuilder sb = new StringBuilder();    
        DataAccess da = new DataAccess();
        DataTable ds = da.executeStoreProcedureDataTable("spr_ArticuloObtener", new Dictionary<string, object>() { });
        foreach (DataRow A in ds.Rows)
        {
            sb.AppendLine("<tr onclick=CargarArticuloPorID(" + A["IdArticulo"] + ")>" +
                "<td>"+A["NombreArticulo"]+"</td>" +
                "<td>" + A["Descripcion"] + "</td>" +
                "<td>"+A["Categoria"]+"</td>" +
                "<td>" + A["Simbolo"] + "</td>" +
                "<td>" + (A["Activo"].ToString().ToLower().Equals("true") ? "Si" : "No") + "</td></tr>");
        }
        return sb.ToString();
    }
    [WebMethod]
    public static string CargarArticuloPorId(int IdArticulo)
    {
        StringBuilder sb = new StringBuilder();
        DataAccess da = new DataAccess();
        DataTable result = da.executeStoreProcedureDataTable("spr_CargarArticuloStockXMLPorId", new Dictionary<string, object>() { 
            { "@IdArticulo", IdArticulo }
        });
        var stringBuilder = new StringBuilder();
        foreach (DataRow row in result.Rows)
        {
            stringBuilder.Append(row[0]);
        }
        var xml = stringBuilder.ToString();
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Articulo));
        var reader = new StringReader(xml);
        var objeto = (Articulo)serializer.Deserialize(reader);
        return new JavaScriptSerializer().Serialize(objeto);
    }
}