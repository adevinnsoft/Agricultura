using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class configuracion_frmParametroEntrega : BasePage
{
    
    private string strIdCriterio =     "idCriterioParametroEntrega";
    private string strActivo =         "activo";
    private string strDescripcion =    "descripcion";
    private string strDescripcionAux = "descripcion_aux";
    private string strDescripcionEn =  "descripcion_EN";
    private string strUsuario =        "vUsuario";
    private Int32 contadorCriterio = 0, 
                    idCriterio = 0;

    List<string> plantasSeleccionadas;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                llenaComboBoxList();
                llenaGridView();
                llenaRadioButtonTipo();
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.ToString());
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable result;
        Dictionary<string, object> parameters = null; 
        Int32 idParametro = 0;
        if (txtNameParametro.Text.Trim().Length <= 0 ||
            txtNameParametro_EN.Text.Trim().Length <= 0 ||
            rlTipo.SelectedValue.ToString() == null ||
            rlTipo.SelectedValue.ToString().Length <= 0 ||
            rlTipo.SelectedValue.ToString().Equals("0")
            )
        {
            // no guarda
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("CamposPorLlenar").ToString(), Comun.MESSAGE_TYPE.Warning);

        }
        else
        {

            try
            {

                if (hiddenIdParametroEntrega.Value.Trim().Length > 0)
                    Int32.TryParse(hiddenIdParametroEntrega.Value.Trim() , out idParametro);
                //guardando o modificando
                parameters = new Dictionary<string, object>();
                parameters.Add("@idParametroEntrega", idParametro);
                parameters.Add("@idTipoParametroEntrega", Int32.Parse(rlTipo.SelectedValue));
                parameters.Add("@usuarioModifica", Int32.Parse(Session["idUsuario"].ToString()));
                parameters.Add("@nombre", txtNameParametro.Text.ToString());
                parameters.Add("@nombre_EN", txtNameParametro_EN.Text.ToString());
                parameters.Add("@activo", ckActiveParametro.Checked);

                result = dataaccess.executeStoreProcedureDataTable("spr_GuardaParametroEntrega", parameters);

                if (result != null && result.Rows.Count > 0)
                    Int32.TryParse(result.Rows[0][0].ToString(), out idParametro);

                if (idParametro > 0)
                {
                    foreach (ListItem item in cblPlanta.Items)
                    {
                        parameters = new Dictionary<string, object>();
                        parameters.Add("@idParametroEntrega", idParametro);
                        parameters.Add("@idPlanta", item.Value);
                        parameters.Add("@activo", item.Selected);
                        parameters.Add("@usuarioModifica", Int32.Parse(Session["idUsuario"].ToString()));

                        result = dataaccess.executeStoreProcedureDataTable("spr_GuardaParametroEntregaPlanta", parameters);
                    }

                    DataTable table = recuperaDataTableDeCriterios();
                    foreach (DataRow row in table.Rows)
                    {
                        if (Server.HtmlDecode(row[strDescripcionAux].ToString()).Trim().Length > 0)
                        {

                            parameters = new Dictionary<string, object>();
                            Int32 idCriterio = 0;
                            Int32.TryParse(row[strIdCriterio].ToString(), out idCriterio);
                            idCriterio = (idCriterio > 0) ? idCriterio : 0;
                            parameters.Add("@idCriterioParametroEntrega", idCriterio);
                            parameters.Add("@idParametroEntrega", idParametro);
                            parameters.Add("@descripcion", Server.HtmlDecode(CultureInfo.CurrentCulture.Name == "es-MX" ? row[strDescripcion].ToString() : row[strDescripcionAux].ToString()));
                            parameters.Add("@descripcion_EN", Server.HtmlDecode(CultureInfo.CurrentCulture.Name == "es-MX" ? row[strDescripcionAux].ToString() : row[strDescripcion].ToString()));
                            parameters.Add("@usuarioModifica", Int32.Parse(Session["idUsuario"].ToString()));
                            parameters.Add("@activo", Boolean.Parse(row[strActivo].ToString()));
                            result = dataaccess.executeStoreProcedureDataTable("spr_GuardaCriterioParametroEntrega", parameters);
                        }
                    }

                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("Guardado").ToString(), Comun.MESSAGE_TYPE.Success);
                    limpiarCampos();
                    llenaGridView();
                }
                else 
                {
                    if (idParametro < 0 && result.Rows != null && result.Rows.Count > 0 && result.Rows[0].ItemArray.Length > 1 && result.Rows[0][1].ToString().StartsWith("EXISTE_NOMBRE"))
                        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ErrorNombreExiste").ToString(), Comun.MESSAGE_TYPE.Error);
                    else
                        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ErrorAlGuardar").ToString(), Comun.MESSAGE_TYPE.Error);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);


                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("ErrorAlGuardar").ToString(), Comun.MESSAGE_TYPE.Error);
            }

        }

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiarCampos();
    }

    protected void gvParametro_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dsParametro"])
            {
                DataSet ds = ViewState["dsParametro"] as DataSet;

                if (ds != null)
                {
                    gvParametro.DataSource = ds;
                    gvParametro.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception)
        {
        }

    }

    protected void gvParametro_PreRender(object sender, EventArgs e)
    {

        if (gvParametro.HeaderRow != null)
            gvParametro.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void gvParametro_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvParametro, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

            }
        }
    }

    protected void gvParametro_SelectedIndexChanged(object sender, EventArgs e)
    {
        limpiarCampos();

        Session["IdModuloCookie"] = gvParametro.DataKeys[gvParametro.SelectedIndex].Value;
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@idParametroEntrega", Session["IdModuloCookie"]);

        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneParametroEntrega", parameters);
        if (dt.Rows.Count > 0)
        {
            hiddenIdParametroEntrega.Value = dt.Rows[0]["idParametroEntrega"].ToString();
            txtNameParametro.Text = dt.Rows[0]["nombre"].ToString();
            txtNameParametro_EN.Text = dt.Rows[0]["nombre_EN"].ToString();
            rlTipo.SelectedValue = dt.Rows[0]["idTipoParametroEntrega"].ToString();
            rlTipo.Enabled = false;
            if (dt.Rows[0][strActivo].ToString().Equals("True"))
                ckActiveParametro.Checked = true;
            else
                ckActiveParametro.Checked = false;

            llenaCriterios(Session["IdModuloCookie"]);

            Dictionary<string, object> PlantasParam = new System.Collections.Generic.Dictionary<string, object>();
            PlantasParam.Add("@idParametroEntrega", Session["IdModuloCookie"]);
            PlantasParam.Add("@idUsuario", Session["idUsuario"]);
            PlantasParam.Add("@soloActivos", 1);

            DataTable dt2 = dataaccess.executeStoreProcedureDataTable("spr_ObtieneParametroEntregaPlantas", PlantasParam);
            if (dt2 == null || dt2.Rows == null)
                return;
            if (dt2.Rows.Count > 0)
            {
                foreach (ListItem item in cblPlanta.Items)
                {
                      item.Selected = false;
                }
                foreach (ListItem item in cblPlanta.Items) 
                {
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (item.Value.Equals(row["idPlanta"].ToString()))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
                /*
            else
            {
                // No se encontró el registro.
            //    limpiarCampos();
            //    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoEncontrado").ToString(), Comun.MESSAGE_TYPE.Success);
                return;
            }
            */

            btnSave.Text = GetLocalResourceObject("Modificar").ToString();
            btnClear.Text = GetLocalResourceObject("Cancelar").ToString();
        }
        else
        {
            //No se encontró el registro
            limpiarCampos();
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("NoEncontrado").ToString(), Comun.MESSAGE_TYPE.Success);
            return;
        }

    }




    protected void btnCancelaCriterio_Click(object sender, EventArgs e)
    {
       
        limpiarCriterio();
        cblPlanta.DataBind();
        rlTipo.DataBind();
    }
   
    protected void btnAgregaCriterio_Click(object sender, EventArgs e)
    {

        String descripcion = CultureInfo.CurrentCulture.Name == "es-MX" ? txtDescripcion.Text : txtDescripcion_EN.Text;
        String descripcion_aux = CultureInfo.CurrentCulture.Name == "es-MX" ? txtDescripcion_EN.Text : txtDescripcion.Text;
        DataTable table = null;
        contadorCriterio = 0;
        DataRow row = null;
        if (descripcion.Trim().Length < 1 || descripcion_aux.Trim().Length < 1 )
        {
            // no guarda
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("CamposPorLlenarCriterio").ToString(), Comun.MESSAGE_TYPE.Warning);
            return;
        }
        else if (hiddenIdCriterio.Value.Length > 0)
        {
            // modifica criterio
            idCriterio = 0;
            Int32.TryParse(hiddenIdCriterio.Value, out idCriterio);
            if (!idCriterio.Equals(0))
            {
                table = recuperaDataTableDeCriterios();
                foreach (DataRow drow in table.Rows)
                {
                    if (drow[strIdCriterio].ToString().Equals(idCriterio.ToString()))
                    {
                        row = drow;
                        break;
                        
                    }
                }
            }
        }
        else
        {
           // nuevo criterio a guardar
            if (hiddenCountCriterioNuevo.Value.Trim().Length > 0)
                Int32.TryParse(hiddenCountCriterioNuevo.Value,  out contadorCriterio);
            contadorCriterio -= 1;
            hiddenCountCriterioNuevo.Value = contadorCriterio.ToString();
            table = recuperaDataTableDeCriterios();
            row = table.NewRow();
        }

        if (row != null)
        {
            row[strActivo] = ckActiveCriterio.Checked;
            row[strDescripcion] = descripcion;
        //    row[strUsuario] = Session["usernameInj"].ToString();
            row[strDescripcionAux] = descripcion_aux;

            if (contadorCriterio < 0)
            {
                row[strIdCriterio] = contadorCriterio;
                table.Rows.Add(row);
            }
            gvCriterio.DataSource = table;
            gvCriterio.DataBind();
        }

        limpiarCriterio();
    }
    
    protected void gvCriterio_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            limpiarCriterio();
            DataRow row = null;
            Session["IdModuloCookie"] = gvCriterio.DataKeys[gvCriterio.SelectedIndex].Value;
            DataTable table = recuperaDataTableDeCriterios();
            row = getRowCriterio(Int32.Parse(Session["IdModuloCookie"].ToString().Trim()), table);
            if (Session["IdModuloCookie"].ToString().Trim().Length > 0)
            {
                if (Int32.Parse(Session["IdModuloCookie"].ToString().Trim()) > 0 && Server.HtmlDecode(row[strDescripcionAux].ToString()).Trim().Length <= 0 )
                {
                    Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                    parameters.Add("@idCriterioParametroEntrega", Session["IdModuloCookie"]);
                    DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneCriterioParametroEntrega", parameters);
                    if (dt.Rows.Count > 0)
                    {
                        row = dt.Rows[0];
                        txtDescripcion.Text = dt.Rows[0][strDescripcion].ToString();
                        txtDescripcion_EN.Text = dt.Rows[0][strDescripcionEn].ToString();
                        hiddenIdCriterio.Value = dt.Rows[0][strIdCriterio].ToString();
                        ckActiveCriterio.Checked = Boolean.Parse(dt.Rows[0][strActivo].ToString());
                    }
                }
                else
                {
                    // si id es negativo, se busca row en gridview(es criterio nuevo)
                    String descripcion = CultureInfo.CurrentCulture.Name == "es-MX" ? txtDescripcion.Text : txtDescripcion_EN.Text;
                    String descripcion_aux = CultureInfo.CurrentCulture.Name == "es-MX" ? txtDescripcion_EN.Text : txtDescripcion.Text;

                    txtDescripcion.Text = CultureInfo.CurrentCulture.Name == "es-MX" ? row[strDescripcion].ToString() : row[strDescripcionAux].ToString();
                    txtDescripcion_EN.Text = CultureInfo.CurrentCulture.Name == "es-MX" ? row[strDescripcionAux].ToString() : row[strDescripcion].ToString();
                    ckActiveCriterio.Checked = Boolean.Parse(row[strActivo].ToString());
                    hiddenIdCriterio.Value = row[strIdCriterio].ToString();
                }
            }
           
        
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    protected void gvCriterio_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvCriterio, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }

    protected void gvCriterio_PreRender(object sender, EventArgs e)
    {
        if (gvCriterio.HeaderRow != null)
            gvCriterio.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    //END eventos Form

    DataTable recuperaDataTableDeCriterios()
    {
        DataTable data = createDataTableCriterio();
        if (gvCriterio.Rows.Count > 0)
        {
            foreach (GridViewRow row in gvCriterio.Rows)
            {
                DataRow dataRow = data.NewRow();
                dataRow[strIdCriterio] = Convert.ToInt32(row.Cells[1].Text == "" ? "0" : row.Cells[1].Text);
                dataRow[strActivo] = Convert.ToBoolean( row.Cells[2].Text);
                dataRow[strDescripcion] = Server.HtmlDecode(row.Cells[0].Text);
              //  dataRow[strUsuario] = Server.HtmlDecode(row.Cells[1].Text);
                dataRow[strDescripcionAux] = Server.HtmlDecode(row.Cells[3].Text);
                
                data.Rows.Add(dataRow);
            }

        }
        return data;
    }

    protected DataRow getRowCriterio(Int32 idCriterio, DataTable table)
    {
        DataRow row = null;
        foreach (DataRow drow in table.Rows)
        {
            if (drow[strIdCriterio].ToString().Equals(idCriterio.ToString()))
            {
                row = drow;
                break;

            }
        }
        return row;
    }

    private DataTable createDataTableCriterio()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.Int32");
        column.ColumnName = strIdCriterio;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.Boolean");
        column.ColumnName = strActivo;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strDescripcion;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType( "System.String");
        column.ColumnName = strDescripcionAux;
        data.Columns.Add(column);

        //column = new DataColumn();
        //column.DataType = Type.GetType("System.String");
        //column.ColumnName = strUsuario;
        //data.Columns.Add(column);

        return data;
    }

    protected void limpiarCampos()
    {
        limpiarCriterio();


        DataTable dt1 = null, dt2 = null;

        dt1 = (DataTable)gvCriterio.DataSource;
        if (dt1 != null)
        {
            dt2 = dt1.Clone();
            gvCriterio.DataSource = dt2;

        }
        gvCriterio.DataBind();


        btnSave.Text = GetLocalResourceObject("Guardar").ToString();
        btnClear.Text = GetLocalResourceObject("Limpiar").ToString();


        hiddenIdCriterio.Value = string.Empty;
        hiddenIdParametroEntrega.Value = "";
        hiddenCountCriterioNuevo.Value = "";

        txtNameParametro.Text = "";
        txtNameParametro_EN.Text = "";
        ckActiveParametro.Checked = true;
        rlTipo.Enabled = true;
        if (rlTipo.Items.Count > 0)
            rlTipo.Items[0].Selected = true;
        foreach (ListItem item in cblPlanta.Items)
        {
            item.Selected = false;
        }

        
    }

    protected void limpiarCriterio()
    {
        txtDescripcion.Text = "";
        txtDescripcion_EN.Text = "";
        ckActiveCriterio.Checked = true;
        hiddenIdCriterio.Value = "";
    }

    protected void llenaRadioButtonTipo()
    {

        try
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("@activo", 1);
            par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            rlTipo.DataSource = dataaccess.executeStoreProcedureDataTable("spr_ObtieneTiposParametroEntrega", par);
            rlTipo.DataTextField = "nombre";
            rlTipo.DataValueField = "idTipoParametroEntrega";
            rlTipo.DataBind();
            if (rlTipo.Items.Count > 0)
                rlTipo.Items[0].Selected = true;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
        }
    }


    public void llenaComboBoxList()
    {
        try
        {
            plantasSeleccionadas = new List<string>();
            foreach (ListItem item in cblPlanta.Items)
            {
                if (item.Selected == true)
                {
                    plantasSeleccionadas.Add(item.Value);
                }
            }

            DataTable dtplanta = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasDdl", new Dictionary<string, object>() { { "@idUsuario", Session["idUsuario"].ToString() } });
            cblPlanta.DataSource = dtplanta;
            cblPlanta.DataTextField = "NombrePlanta";
            cblPlanta.DataValueField = "idPlanta";
            cblPlanta.DataBind();

            if (plantasSeleccionadas.Count > 0)
            {
                foreach (ListItem item in cblPlanta.Items)
                {
                    foreach (string row in plantasSeleccionadas)
                    {
                        if (item.Value.Equals(row))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }

        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }

    }

    public void llenaGridView()
    {
        try
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            par.Add("@soloActivos", 1);
            DataTable dataTable = dataaccess.executeStoreProcedureDataTable("spr_ObtieneParametrosEntrega", par);
            gvParametro.DataSource = dataTable;

            ViewState["dsParametro"] = dataTable;
            gvParametro.DataBind();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }


    protected void llenaCriterios(object idParametroentrada)
    {
        try
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@idParametroEntrega", idParametroentrada);
            param.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
            param.Add("@soloActivos", 0);

            DataTable data = dataaccess.executeStoreProcedureDataTable("spr_ObtieneCriteriosParametroEntrega", param);
            data.Columns.Add(new DataColumn(strDescripcionAux));

            gvCriterio.DataSource = data;
            gvCriterio.DataBind();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
        }
    }

}
