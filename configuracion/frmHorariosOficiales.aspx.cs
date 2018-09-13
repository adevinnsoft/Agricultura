using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class configuracion_frmHorariosOficiales : BasePage
{


    private string strIdHorario = "idHorarioOficial";
    private string strIdDepto = "idDepartamento";
    private string strIdLider = "idLider";
    private string strIdPais = "idPais";
    private string strIdPlanta = "idPlanta";
    private string strNombre = "Nombre";
    private string strDepartamento = "Departamento";
    private string strNombreLider = "NombreLider";
    private string strNombrePlanta = "NombrePlanta";
    private string strIdInvernadero = "IdInvernadero";
    private string strNombreInvernadero = "ClaveInvernadero";

    private string strEvento = "Evento";
    private string strEventoEn = "Evento_EN";
    private string strFecha = "fecha";
    private string strFechaMod = "fechaModifico";
    private string strVNombre = "vNombre";
    private string strActivo = "Activo";


    private String strHoraInicio = "HoraInicio";
    private String strMinutoInicio = "minutoInicio";
    private String strHoraFin = "HoraFin";
    private String strMinutoFin = "minutoFin";

    private string strTipoRepeticion = "TipoRepeticion";
    private string strSeTrabaja = "SeTrabaja";
    private string strKey = "Key";
    private string strValue = "Value";
    private List<string> paises = new List<string>();
    private List<string> plantas = new List<string>();
    private List<string> deptos = new List<string>();
    private List<string> lideres = new List<string>();
    private List<string> invernaderos = new List<string>();

    private List<ListItem> plantasRem = new List<ListItem>();
    private List<ListItem> deptosRem = new List<ListItem>();
    private List<ListItem> lideresRem = new List<ListItem>();
    private List<ListItem> invernaderosRem = new List<ListItem>();
    string[] repetir = { "porDia", "porSemana", "porMes", "porAnio" };

    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if (!IsPostBack)
            {
                limpiarCampos();
                llenaPaises();
                llenaDiasSemana();
                llenaRepetir();
                llenaGrid();
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.ToString());
        }
    }

    protected void gvHorarioOficial_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvHorarioOficial, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());

            }
        }
    }

    protected void gvHorarioOficial_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            limpiarCampos();

            Session["IdModuloCookie"] = gvHorarioOficial.DataKeys[gvHorarioOficial.SelectedIndex].Value;
            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
            parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);

            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneHorarioOficial", parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                
                txtHoraInicio.Text = generaStringHora(row[strHoraInicio].ToString(), row[strMinutoInicio].ToString());
                txtHoraFin.Text = generaStringHora(row[strHoraFin].ToString(), row[strMinutoFin].ToString());
                
                hiddenIdHorarioOficial.Value = dt.Rows[0][strIdHorario].ToString();
                txtEvento.Text = row["Evento"].ToString();
                txtEvento_EN.Text = row["Evento_EN"].ToString();
                setFecha(DateTime.Parse(row["Fecha"].ToString()));

                btnSave.Text = GetLocalResourceObject("Modificar").ToString();
                btnClear.Text = GetLocalResourceObject("Cancelar").ToString();

                if (dt.Rows[0]["Activo"].ToString().Equals("True"))
                    chkActivo.Checked = true;
                else
                    chkActivo.Checked = false;
                if (dt.Rows[0][strSeTrabaja].ToString().Equals("True"))
                    chkSeTrabaja.Checked = true;
                else
                    chkSeTrabaja.Checked = false;
                if (row[strTipoRepeticion] != null)
                {
                    rblRepetir.SelectedValue = row[strTipoRepeticion].ToString();
                    if (rblRepetir.SelectedValue == "2")
                    {
                        ckDias.Visible = true;
                    }
                }

                parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);
                dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneDiasPorHorarioOficial", parameters);
                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (ListItem item in ckDias.Items)
                        {
                            if (item.Value == dr["dia"].ToString())
                                item.Selected = true;
                        }
                    }
                }


                parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);
                dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePaisesPorHorarioOficial", parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListItem pais = lbPais.Items.FindByValue(dr[strIdPais].ToString());
                        if (pais != null)
                        {
                            pais.Selected = true;
                            if (!paises.Contains(pais.Value))
                            {
                                paises.Add(pais.Value);
                                llenaPlantas(pais.Value);
                            }
                        }
                    }

                    ViewState["lPais"] = paises;

                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                    parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);
                    dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasPorHorarioOficial", parameters);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            foreach (ListItem item in lbPlantas.Items)
                            {
                                if (item.Value.EndsWith(dr[strIdPlanta].ToString()))
                                {
                                    item.Selected = true;
                                    plantas.Add(item.Value);
                                    break;
                                }
                            }
                        }
                        ViewState["lPlanta"] = plantas;

                        llenaDeptos();
                        if (lbDepto.Items.Count > 0)
                        {
                            parameters = new System.Collections.Generic.Dictionary<string, object>();
                            parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);
                            dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneDepartamentosPorHorarioOficial", parameters);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    foreach (ListItem depto in lbDepto.Items)
                                    {
                                        if (depto.Value.Equals(dr[strIdDepto].ToString()))
                                        {
                                            depto.Selected = true;
                                            deptos.Add(depto.Value);
                                            foreach (ListItem planta in lbPlantas.Items)
                                            {
                                                if (planta.Selected)
                                                {
                                                    llenaLider(depto.Value, planta.Value.Split('-')[1]);
                                                }
                                            }
                                        }
                                    }
                                }

                                ViewState["lDepto"] = deptos;

                                if (lbLider.Items.Count > 0)
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneLideresPorHorarioOficial", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        //lbLider.ClearSelection();
                                        //lideres.Clear();
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            foreach (ListItem lider in lbLider.Items)
                                            {
                                                if (lider.Value.EndsWith(dr[strIdLider].ToString()))
                                                {
                                                    lider.Selected = true;
                                                    lideres.Add(lider.Value);
                                                    string[] l = lider.Value.Split('-');
                                                    llenaInvernadero(l[0], l[1], l[2]);
                                                    //break;
                                                }
                                            }
                                        }

                                        ViewState["lLider"] = lideres;

                                        // consultar invernaderos

                                        if (lbInvernadero.Items.Count > 0)
                                        {
                                            parameters = new System.Collections.Generic.Dictionary<string, object>();
                                            parameters.Add("@idHorarioOficial", Session["IdModuloCookie"]);
                                            dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneInvernaderosPorHorarioOficial", parameters);
                                            if (dt != null && dt.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dt.Rows)
                                                {
                                                    foreach (ListItem inv in lbInvernadero.Items)
                                                    {
                                                        if (inv.Value.EndsWith(dr[strIdInvernadero].ToString()))
                                                        {
                                                            invernaderos.Add(inv.Value);
                                                            inv.Selected = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                ViewState["lInvernadero"] = invernaderos;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    protected void gvHorarioOficial_PreRender(object sender, EventArgs e)
    {
        if (gvHorarioOficial.HeaderRow != null)
            gvHorarioOficial.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiarCampos();
    }


    private string getStringSeleccion(int[] indices, ListItemCollection coleccion)
    {
        string str = "";
        if (indices == null)
        {

            foreach (ListItem item in coleccion)
            {
                if (item.Selected)
                    str += (str.Length > 0 ? "," : "") + item.Value;
            }
        }
        else
        {
            foreach (int index in indices)
                str += (str.Length > 0 ? "," : "") + coleccion[index].Value;
        }
        return str;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try{
            txtFecha.ReadOnly = false;
            txtHoraInicio.ReadOnly = false;
            txtHoraFin.ReadOnly = false;
            DateTime fecha = new DateTime();
            string[] horainicio = txtHoraInicio.Text.Split(':');
            string[] horainfin = txtHoraFin.Text.Split(':');
            bool saveOk = false;
            if (txtEvento.Text.Length > 0 && txtEvento_EN.Text.Length > 0 && txtFecha.Text.Length > 0 && txtHoraInicio.Text.Length > 0 && txtHoraFin.Text.Length > 0 && DateTime.TryParse(txtFecha.Text + " 00:00:00", out fecha) && horainfin.Length == 2 && horainfin.Length == 2 )
            {
                
                string dias = getStringSeleccion(null, ckDias.Items);
                string pais = getStringSeleccion(lbPais.GetSelectedIndices(), lbPais.Items);
                string planta = getStringSeleccion(lbPlantas.GetSelectedIndices(), lbPlantas.Items);
                string depto = getStringSeleccion(lbDepto.GetSelectedIndices(), lbPlantas.Items);
                string lider = getStringSeleccion(lbLider.GetSelectedIndices(), lbLider.Items);
                string inver = getStringSeleccion(lbInvernadero.GetSelectedIndices(), lbInvernadero.Items);


                int idHorario = 0;
                Int32.TryParse(hiddenIdHorarioOficial.Value, out idHorario);
                

                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idHorarioOficial",idHorario);
	            parameters.Add("@Evento", txtEvento.Text);
	            parameters.Add("@Evento_EN",txtEvento_EN.Text);
	            parameters.Add("@Fecha", fecha);
	            parameters.Add("@HoraInicio", Int32.Parse(horainicio[0]));
                parameters.Add("@MinutoInicio", Int32.Parse(horainicio[1]));
	            parameters.Add("@HoraFin", Int32.Parse(horainfin[0])); 
	            parameters.Add("@MinutoFin", Int32.Parse(horainfin[1]));
	            parameters.Add("@TipoRepeticion", (rblRepetir.SelectedItem != null)? rblRepetir.SelectedItem.Value.ToString(): "0");
                parameters.Add("@dias", dias);
                parameters.Add("@idPais", pais);
                parameters.Add("@idPlanta", planta);
                parameters.Add("@idDepto", depto);
                parameters.Add("@idLider", lider);
                parameters.Add("@idInvernadero", inver);


                DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_HorarioOficialValidador", parameters);

                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    int valido = -1;
                    Int32.TryParse(dt.Rows[0][0].ToString(), out valido);
                    if (valido < 0)
                    {
                        //Error
                        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("compruebeDatos").ToString(), Comun.MESSAGE_TYPE.Error);
                        return;
                    }
                    else if (valido > 0)
                    {
                        string etiqueta = dt.Rows[0].ItemArray.Length > 1 ? dt.Rows[0][1].ToString() : "";
                        // registros existentes
                        if (etiqueta.Equals("EVENTO"))
                            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("eventoExistente").ToString(), Comun.MESSAGE_TYPE.Error);
                        else if (etiqueta.Equals("CAMPOS"))
                            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("registrosExistentes").ToString(), Comun.MESSAGE_TYPE.Error);
                        else
                            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("compruebeDatos").ToString(), Comun.MESSAGE_TYPE.Error);
                        return;

                    }
                    // else  validación OK / continua con proceso


                }
                else
                {
                    //Error
                    popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("compruebeDatos").ToString(), Comun.MESSAGE_TYPE.Error);
                    return;

                }
                

                
                parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idHorarioOficial", idHorario);
                parameters.Add("@Evento", txtEvento.Text);
                parameters.Add("@Evento_EN", txtEvento_EN.Text);
                parameters.Add("@Fecha", fecha);
                parameters.Add("@HoraInicio", horainicio[0]);
                parameters.Add("@MinutoInicio", horainicio[1]);
                parameters.Add("@HoraFin", horainfin[0]);
                parameters.Add("@MinutoFin", horainfin[1]);
                parameters.Add("@TipoRepeticion", (rblRepetir.SelectedItem != null) ? rblRepetir.SelectedItem.Value.ToString() : "0");
                parameters.Add("@SeTrabaja", chkSeTrabaja.Checked);
                parameters.Add("@Activo", chkActivo.Checked);
                parameters.Add("@usuarioModifico", Int32.Parse(Session["idUsuario"].ToString()));

                dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficial", parameters);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int hiddenId = 0;
                    Int32.TryParse(dt.Rows[0][0].ToString(), out idHorario);
                    
                    if (idHorario > 0)
                    {
                        Int32.TryParse(hiddenIdHorarioOficial.Value, out hiddenId);
                        if (hiddenId > 0)
                        {
                            parameters = new Dictionary<string, object>();
                            parameters.Add("@idHorarioOficial", idHorario);
                            dt = dataaccess.executeStoreProcedureDataTable("spr_HorarioOficialLimpiaReferenciasParaUpdate", parameters);
                            int status = 0;
                            if (dt != null && dt.Rows.Count > 0)
                                Int32.TryParse(dt.Rows[0][0].ToString(), out status);

                        }
                        if (lbPais.GetSelectedIndices().Length > 0) 
                        {
                            string id = "";
                            foreach (ListItem item in ckDias.Items)
                            {
                                if (item.Selected)
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", idHorario);
                                    parameters.Add("@dia", item.Value);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficialDia", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                        id = dt.Rows[0][0].ToString();
                                }
                            }
                            foreach (ListItem item in lbPais.Items)
                            {
                                if (item.Selected && !item.Value.Equals("0"))
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", idHorario);
                                    parameters.Add("@idPais", item.Value);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficialPais", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                        id = dt.Rows[0][0].ToString();
                                }
                            }
                            foreach (ListItem item in lbPlantas.Items)
                            {
                                if (item.Selected && !item.Value.Equals("0"))
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", idHorario);
                                    parameters.Add("@idPlanta", item.Value.Split('-')[1]);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficialPlanta", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                        id = dt.Rows[0][0].ToString();
                                }
                            }
                            foreach (ListItem item in lbDepto.Items)
                            {
                                if (item.Selected && !item.Value.Equals("0"))
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", idHorario);
                                    parameters.Add("@idDepartamento", item.Value);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficialDepartamento", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                        id = dt.Rows[0][0].ToString();
                                }
                            }
                            foreach (ListItem item in lbLider.Items)
                            {
                                if (item.Selected && !item.Value.Equals("0"))
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", idHorario);
                                    parameters.Add("@idLider", item.Value.Split('-')[2]);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficialLider", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                        id = dt.Rows[0][0].ToString();
                                }
                            }
                            foreach (ListItem item in lbInvernadero.Items)
                            {
                                if (item.Selected && !item.Value.Equals("0"))
                                {
                                    parameters = new System.Collections.Generic.Dictionary<string, object>();
                                    parameters.Add("@idHorarioOficial", idHorario);
                                    parameters.Add("@idInvernadero", item.Value.Split('-')[3]);
                                    dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioOficialInvernadero", parameters);
                                    if (dt != null && dt.Rows.Count > 0)
                                        id = dt.Rows[0][0].ToString();
                                }
                            }
                        }


                        saveOk = true;
                    }
                    else
                    {
                        saveOk = false;
                    }
                }
            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("compruebeDatos").ToString(), Comun.MESSAGE_TYPE.Error);
                return;
            }

            if (saveOk)
            {
                limpiarCampos();
                llenaGrid();
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("guardadoOk").ToString(), Comun.MESSAGE_TYPE.Info);
                return;

            }
            else
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("errorGuardar").ToString(), Comun.MESSAGE_TYPE.Error);
                return;
            }



        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("errorGuardar").ToString(), Comun.MESSAGE_TYPE.Error);
            return;
        }
    }

    protected void lbPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        if (!IsPostBack && ViewState["enUso"] != null && Boolean.Parse(ViewState["enUso"].ToString()))
            return;
        ViewState["enUso"] = true;
        paises = ViewState["lPais"] != null ? ViewState["lPais"] as List<string> : new List<string>();
        
        foreach (ListItem item in lbPais.Items)
        {

            if (item.Selected)
            {
                if (item.Value.Equals("0"))
                {
                    //no selecciona index 0
                    if (lbPais.GetSelectedIndices().Length == 1)
                    {
                        limpiaListaPais();
                        limpiaListaDepto(true);
                        limpiaListaLider(true);
                        ViewState["enUso"] = false;
                        return;
                    }
                    else
                    {
                        item.Selected = false;
                        continue;
                    }
                }
                else if (!paises.Contains(item.Value))
                {
                    paises.Add(item.Value);
                    llenaPlantas(item.Value);
                }
            }
            else
            {
                if (paises.Count > 0 && paises.Contains(item.Value))
                {
                    paises.Remove(item.Value);
                    plantasRem = new List<ListItem>();
                    // buscar plantas a borrar
                    foreach (ListItem planta in lbPlantas.Items)
                    {
                        if (planta.Value.StartsWith(item.Value + "-"))
                        {
                            plantasRem.Add(planta);
                        }
                    }
                    //borrando plantas de pais
                    if (plantasRem.Count > 0)
                    {
                        DataTable dtPlanta = ViewState["dtPlanta"] as DataTable;
                                            
                        foreach (ListItem planta in plantasRem)
                        {
                            if (planta.Value.StartsWith(item.Value + "-"))
                            {
                                if (ViewState["dtPlanta"] != null)
                                {
                                    lbPlantas.Items.Remove(planta);
                                    List<DataRow> plantasABorrar = new List<DataRow>();
                                    foreach (DataRow drow in dtPlanta.Rows)
                                    {
                                        if (drow[strIdPlanta].ToString() == planta.Value)
                                            plantasABorrar.Add(drow);
                                    }
                                    if (plantasABorrar != null && plantasABorrar.Count > 0)
                                    {
                                        foreach (DataRow drow in plantasABorrar)
                                        {
                                            
                                            foreach (ListItem depto in lbDepto.Items)
                                            {
                                                if (depto.Selected)
                                                {
                                                    removerLideres(drow.ItemArray[0].ToString().Split('-')[1], depto.Value.ToString());
                                                }
                                            }
                                            dtPlanta.Rows.Remove(drow);
                                        }
                                    }
                                }
                            }
                        }
                        ViewState["dtPlanta"] = dtPlanta;
                    }
                }
            }
        }
        
                    if(lbDepto.Items.Count == 0)
                        llenaDeptos();
        ViewState["lPais"] = paises;

        ViewState["enUso"] = false;
    }


    private string buscaNuevo(List<string> lista, ListItemCollection items ) {
        foreach(ListItem item in items){
            if (!lista.Contains(item.Value))
            {
               
                return item.Value;
            }
        }

        return string.Empty;
    }



    protected void lbPlantas_SelectedIndexChanged(object sender, EventArgs e)
    {
        

        if (!IsPostBack && ViewState["enUso"] != null && Boolean.Parse(ViewState["enUso"].ToString()))
            return;
        ViewState["enUso"] = true;

        if (ViewState["lPlanta"] != null)
            plantas = ViewState["lPlanta"] as List<string>;
        else
            plantas = new List<string>();

        var nuevo = buscaNuevo(plantas, ((ListControl)sender).Items);
        if (nuevo.Length == 0 )
        {
            ViewState["enUso"] = false;
            return;
        }
         foreach (ListItem item in lbPlantas.Items)
         {

             if (item.Selected)
             {
                 if (item.Value.Equals("0"))
                 {
                     //no selecciona index 0
                     if (lbPlantas.GetSelectedIndices().Length == 1)
                     {
                         limpiaListaPlanta(false);
                         ViewState["enUso"] = false;
                         return;
                     }
                     else
                     {
                         item.Selected = false;
                         continue;
                     }
                 }
                 else if (!plantas.Contains(item.Value))
                 {
                     deptos = ViewState["lDepto"] != null? ViewState["lDepto"] as List<string>: new List<string>();
                     plantas.Add(item.Value);
                     foreach(string depto in deptos)
                     {
                         llenaLider(depto, item.Value.Split('-')[1]);
                     }
                 }
             }
             else
             {
                 if (plantas.Contains(item.Value))
                 {
                     plantas.Remove(item.Value);
                     ViewState["lPlanta"] = plantas;
                     foreach (ListItem dep in lbDepto.Items)
                     {
                         if (lbLider.Items.Count == 0)
                             break;
                         if (!dep.Value.Equals("0") && lbLider.Items.Count > 0)
                         {
                             removerLideres(item.Value.Split('-')[1], dep.Value);
                         }
                     }
                     
                 }
             }
         }
         ViewState["lPlanta"] = plantas;

         ViewState["enUso"] = false;
    }

    protected void lbDepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!IsPostBack && ViewState["enUso"] != null && Boolean.Parse(ViewState["enUso"].ToString()))
            return;
        ViewState["enUso"] = true;

        if (ViewState["lDepto"] != null)
            deptos = ViewState["lDepto"] as List<string>;
        else
            deptos = new List<string>();


        var nuevo = buscaNuevo(deptos, ((ListControl)sender).Items);
        if (nuevo.Length == 0 )
        {
            ViewState["enUso"] = false;
            return;
        }

        plantas = (ViewState["lPlanta"] != null) ? ViewState["lPlanta"] as List<string> : new List<string>();
        foreach (ListItem item in lbDepto.Items)
        {
           
            if (item.Selected)
             {
                 if (item.Value.Equals("0"))
                 {
                     //no selecciona index 0
                     if (lbDepto.GetSelectedIndices().Length == 1)
                     {
                         limpiaListaDepto(false);
                         ViewState["enUso"] = false;
                         return;
                     }
                     else
                     {
                         item.Selected = false;
                         continue;
                     }
                 }
                 else if (!deptos.Contains(item.Value))
                {
                    deptos.Add(item.Value);
                    if (ViewState["lPlanta"] != null)
                    {
                        foreach (string pl in plantas)
                            llenaLider(item.Value, pl.Split('-')[1]);
                    }
                }
            }
            else
            {

                // buscar lideres a borrar
                if (deptos.Contains(item.Value))
                {
                    deptos.Remove(item.Value);
                    foreach (ListItem pl in lbPlantas.Items)
                    {
                        if (lbLider.Items.Count == 0)
                            break;
                        if (!pl.Value.Equals("0") && lbLider.Items.Count > 0)
                            removerLideres(pl.Value.Split('-')[1], item.Value);
                    }
                }
            }
            

        }
        ViewState["lDepto"] = deptos;

        ViewState["enUso"] = false;

    }

    protected void lbLider_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (!IsPostBack && ViewState["enUso"] != null && Boolean.Parse(ViewState["enUso"].ToString()))
            return;
        ViewState["enUso"] = true;

       lideres = (ViewState["lLider"] != null) ? ViewState["lLider"] as List<string> : new List<string>();

       var nuevo = buscaNuevo(lideres, ((ListControl)sender).Items);
       if (nuevo.Length == 0)
       {
           ViewState["enUso"] = false;
           return;
       }
       foreach (ListItem item in lbLider.Items)
        {

             if (item.Selected)
             {
                 if (item.Value.Equals("0"))
                 {
                     //no selecciona index 0
                     if (lbLider.GetSelectedIndices().Length == 1)
                     {
                         limpiaListaLider(false);
                           ViewState["enUso"] = false;
                         return;
                     }
                     else
                     {
                         item.Selected = false;
                         continue;
                     }
                 }
                 else if (!lideres.Contains(item.Value))
                {
                    lideres.Add(item.Value);
                    string[] valores = item.Value.Split('-');
                    llenaInvernadero(valores[0], valores[1], valores[2]);
                }
            }
            else
            {
                 if (lideres.Contains(item.Value))
                 {
                     lideres.Remove(item.Value);
                     removerInvernadero(item.Value);
                 }
             }
        }

       ViewState["lLider"] = lideres;
       ViewState["enUso"] = false;
    }


    private void llenaInvernadero(string idPlanta, string idDepartamento, string idLider)
    {
        invernaderos = (ViewState["lInvernadero"] != null) ? ViewState["lInvernadero"] as List<string> : new List<string>();
        DataTable dtTemporal = (ViewState["dtInvernadero"] != null) ? ViewState["dtInvernadero"] as DataTable : buildDTInvernadero();
        DataTable dt = dataaccess.executeStoreProcedureDataTable(
            "spr_ObtieneInvernaderosPorLiderDepartamentoPlanta",
            new Dictionary<string, object>() { { "@idPlanta", idPlanta }, { "@idDepartamento", idDepartamento }, { "@idLider", idLider } });

        if (dt != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                DataRow tRow = dtTemporal.NewRow();
                tRow[strIdInvernadero] = idPlanta + "-" + idDepartamento + "-" + idLider + "-" + row[strIdInvernadero];
                tRow[strNombreInvernadero] = row[strNombreInvernadero].ToString();
                dtTemporal.Rows.Add(tRow);
            }

            ViewState["dtInvernadero"] = dtTemporal;
        }

        if (dtTemporal != null && dtTemporal.Rows.Count > 0)
        {
            lbInvernadero.DataSource = dtTemporal;
            lbInvernadero.DataTextField = strNombreInvernadero;
            lbInvernadero.DataValueField = strIdInvernadero;
            lbInvernadero.DataBind();
            lbInvernadero.Items.Insert(0, new ListItem((GetLocalResourceObject("General") != null ? GetLocalResourceObject("General").ToString().ToUpper() : "GENERAL"), "0"));
            foreach (string value in invernaderos)
            {
                ListItem item = lbInvernadero.Items.FindByValue(value);
                if (item != null)
                    item.Selected = true;
            }
        }
    }

    private void llenaLider(string idDepartamento, string idPlanta)
    {
        try
        {
            lideres= (ViewState["lLider"] != null) ? ViewState["lLider"] as List<string> : new List<string>();
            DataTable dtTemporal = (ViewState["dtLider"] != null) ? ViewState["dtLider"] as DataTable : buildDTLider();
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneLideresPorPlantaYDepto", new Dictionary<string, object>() { { "@idPlanta", idPlanta }, { "@idDepartamento", idDepartamento }, { "@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0 } });

            if (dt != null && dt.Rows.Count > 0)
            {
                 foreach (DataRow row in dt.Rows)
                {
                    DataRow tRow = dtTemporal.NewRow();
                    tRow[strIdLider] = idPlanta + "-" + idDepartamento + "-" + row[strIdLider];
                    tRow[strNombreLider] = row[strNombreLider].ToString();
                    dtTemporal.Rows.Add(tRow);
                    //  lideres.Add(tRow[strIdLider].ToString());
                }

                ViewState["dtLider"] = dtTemporal;
                //   ViewState["lLider"] = lideres;
                lbLider.DataSource = dtTemporal;
                lbLider.DataTextField = strNombreLider;
                lbLider.DataValueField = strIdLider;
                lbLider.DataBind();
                lbLider.Items.Insert(0, new ListItem((GetLocalResourceObject("General") != null ? GetLocalResourceObject("General").ToString().ToUpper() : "GENERAL"), "0"));
                 foreach (string value in lideres)
                {
                    ListItem item = lbLider.Items.FindByValue(value);
                    if (item != null)
                        item.Selected = true;
                }
            }
            else
            {
                return;
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    private void llenaPlantas(string idPais)
    {
        try
        {
            DataTable dtTemporal = null;
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePlantasPorPais", new Dictionary<string, object>() { { "@idPais", idPais } });
            if (ViewState["dtPlanta"] != null)
            {
                dtTemporal = ViewState["dtPlanta"] as DataTable;

            }
            else
            {
                dtTemporal = buildDTPlanta();

            }
            if (dtTemporal != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DataRow tRow = dtTemporal.NewRow();
                    tRow[strIdPlanta] = idPais + "-" + row[strIdPlanta];
                    tRow[strNombrePlanta] = row[strNombrePlanta].ToString();
                    dtTemporal.Rows.Add(tRow);
                }

                ViewState["dtPlanta"] = dtTemporal;
                lbPlantas.DataSource = dtTemporal;
                lbPlantas.DataTextField = strNombrePlanta;
                lbPlantas.DataValueField = strIdPlanta;
                lbPlantas.DataBind();

                lbPlantas.Items.Insert(0, new ListItem((GetLocalResourceObject("General") != null ? GetLocalResourceObject("General").ToString().ToUpper() : "GENERAL"), "0"));

            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    private void llenaDeptos()
    {
        try
        {
            DataTable dtTemporal = null;
            if (ViewState["dtDepto"] != null)
            {
                dtTemporal = ViewState["dtDepto"] as DataTable;

            }
            else
            {
                dtTemporal = dataaccess.executeStoreProcedureDataTable("spr_DepartamentosObtener", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0 } });
                DataRow aBorrar = null;
                foreach (DataRow row in dtTemporal.Rows)
                {
                    if (row[strIdDepto].ToString() == "0")
                    {
                        aBorrar = row;
                        break;
                    }
                }
                if (aBorrar != null)
                    dtTemporal.Rows.Remove(aBorrar);
                ViewState["dtDepto"] = dtTemporal;

            }
            if (dtTemporal != null)
            {

                lbDepto.DataSource = dtTemporal;
                lbDepto.DataTextField = strDepartamento;
                lbDepto.DataValueField = strIdDepto;
                lbDepto.DataBind();
                lbDepto.Items.Insert(0, new ListItem((GetLocalResourceObject("General") != null ? GetLocalResourceObject("General").ToString().ToUpper() : "GENERAL"), "0"));

            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    private void llenaPaises()
    {
        try
        {
            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtienePaises", new Dictionary<string, object>() { { "@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0 } });
            ViewState["dtPais"] = dt;
            lbPais.DataSource = dt;
            lbPais.DataTextField = strNombre;
            lbPais.DataValueField = strIdPais;
            lbPais.DataBind();
            lbPais.Items.Insert(0, new ListItem((GetLocalResourceObject("General") != null ? GetLocalResourceObject("General").ToString().ToUpper() : "GENERAL"), "0"));
        }
        catch (Exception e)
        {
            Log.Error(e.Message);

        }

    }

    private DataTable buildDTInvernadero()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.String");
        column.ColumnName = strIdInvernadero;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strNombreInvernadero;
        data.Columns.Add(column);

        return data;
    }
    
    private DataTable buildDTLider()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.String");
        column.ColumnName = strIdLider;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strNombreLider;
        data.Columns.Add(column);

        return data;
    }

    private DataTable buildDTPlanta()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.String");
        column.ColumnName = strIdPlanta;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strNombrePlanta;
        data.Columns.Add(column);

        return data;
    }

    private DataTable buildDTDeptos()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.String");
        column.ColumnName = strIdDepto;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strNombrePlanta;
        data.Columns.Add(column);

        return data;
    }
    
    private DataTable getdataKeyValue()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.String");
        column.ColumnName = strKey;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strValue;
        data.Columns.Add(column);

        return data;
    }

    private void limpiarCampos()
    {

        btnSave.Text = GetLocalResourceObject("Guardar").ToString();
        btnClear.Text = GetLocalResourceObject("Limpiar").ToString();
        hiddenIdHorarioOficial.Value = "0";
        txtEvento.Text = string.Empty;
        txtEvento_EN.Text = string.Empty;
        txtFecha.ReadOnly = false;
        txtFecha.Text = string.Empty;
        txtHoraInicio.ReadOnly = false;
        txtHoraInicio.Text = "00:00";
        txtHoraFin.ReadOnly = false;
        txtHoraFin.Text = "00:00";
        lbInvernadero.Items.Clear();
        lbLider.Items.Clear();
        lbDepto.Items.Clear();
        lbPlantas.Items.Clear();
        lbPais.ClearSelection();
        rblRepetir.SelectedIndex = rblRepetir.Items.Count - 1;
        ckDias.ClearSelection();
        chkActivo.Checked = true;
        chkSeTrabaja.Checked = true;
        ViewState["lPais"] = null;
        ViewState["lPlanta"] = null;
        ViewState["lDepto"] = null;
        ViewState["lLider"] = null;
        ViewState["lInvernadero"] = null;
        ViewState["dtPais"] = null;
        ViewState["dtPlanta"] = null;
        ViewState["dtDepto"] = null;
        ViewState["dtLider"] = null;
        ViewState["dtInvernadero"] = null;
        limpiaListaPais();
    }
    
    private void llenaRepetir()
    {
        
        DataTable data = getdataKeyValue();
        DataRow row = null;
        for (int index = 0; index < repetir.Length ; index++)
        {
            row = data.NewRow();

            row[strKey] = (index +1).ToString();
            row[strValue] = GetLocalResourceObject(repetir[index]).ToString();
            
            data.Rows.Add(row);
        }

        rblRepetir.DataSource = data;
        rblRepetir.DataTextField = strValue;
        rblRepetir.DataValueField = strKey;
        rblRepetir.DataBind();
        rblRepetir.SelectedIndex = repetir.Length - 1;
    }

    private void llenaDiasSemana() 
    {
        DataTable data = getdataKeyValue();
        DataRow row = null;

        for (int index = 0; index < CultureInfo.CurrentCulture.DateTimeFormat.DayNames.Length; index++)
        {
            row = data.NewRow();

            row[strKey] = index.ToString();
            row[strValue] = CultureInfo.CurrentCulture.DateTimeFormat.DayNames[index];
            
            data.Rows.Add(row);
        }


        ckDias.DataSource = data;
        ckDias.DataTextField = strValue;
        ckDias.DataValueField = strKey;
        ckDias.DataBind();

    }

    private void llenaGrid()
    {
        Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
        parameters.Add("@lengua", CultureInfo.CurrentCulture.Name == "es-MX" ? 1 : 0);
        DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneReporteHorariosOficiales", parameters);

        if (dt != null && dt.Rows.Count > 0)
        {

            DataTable data = createGvHorarioOficial();
            foreach (DataRow dr in dt.Rows)
            {
                DateTime date =  DateTime.Parse(dr[strFecha].ToString());
                int index = 0;
                Int32.TryParse(dr[strTipoRepeticion].ToString(), out index);
                DataRow row = data.NewRow();
                row[strIdHorario] = dr[strIdHorario];
                row[strEvento] = dr[strEvento];
                row[strSeTrabaja] = dr[strSeTrabaja];
                row[strFecha] = date.ToString("yyyy-MM-dd");
                row[strHoraInicio] = generaStringHora(dr[strHoraInicio].ToString(), dr[strMinutoInicio].ToString());
                row[strHoraFin] = generaStringHora(dr[strHoraFin].ToString(), dr[strMinutoFin].ToString());
                row[strTipoRepeticion] = index > 0 ? GetLocalResourceObject(repetir[index - 1]).ToString() : "";
                row[strVNombre] = dr[strVNombre];
                row[strFechaMod] = dr[strFechaMod];
                row[strActivo] = dr[strActivo];

                data.Rows.Add(row);
            }
            ViewState["dtHorario"] = data;
            gvHorarioOficial.DataSource = data;
            gvHorarioOficial.DataBind();
        }
    }


    private void removerLideres(string idPlanta, string idDepto)
    {
        lideresRem = new List<ListItem>();
        // buscar lideres a borrar
        foreach (ListItem lider in lbLider.Items)
        {
            if (lider.Value.StartsWith(idPlanta + "-" + idDepto + "-"))
            {
                lideresRem.Add(lider);
            }
        }


        //borrando lideres
        if (lideresRem.Count > 0)
        {
            lideres = ViewState["lLider"] != null ? ViewState["lLider"] as List<string> : new List<string>();
            DataTable dtLider = ViewState["dtLider"] as DataTable;
                foreach (ListItem lider in lideresRem)
                {
                    if (ViewState["dtLider"] != null)
                    {
                        lbLider.Items.Remove(lider);
                        List<DataRow> lideresABorrar = new List<DataRow>();
                        foreach (DataRow drow in dtLider.Rows)
                        {
                            if (drow[strIdLider].ToString() == lider.Value)
                               lideresABorrar.Add(drow);
                        }
                        if (lideresABorrar != null && lideresABorrar.Count > 0)
                        {
                            foreach (DataRow drow in lideresABorrar)
                            {
                                dtLider.Rows.Remove(drow);
                                lideres.Remove(lider.Value);
                                removerInvernadero(lider.Value);
                            }
                        }
                    }
            }
            ViewState["dtLider"] = dtLider;
            ViewState["lLider"] = lideres;
            
        }

        if (lbLider.Items.Count == 1 && lbLider.Items[0].Value.Equals("0"))
            limpiaListaLider(true);
        
    }

    private void removerInvernadero(string idLider)
    {
        invernaderos = ViewState["lInvernadero"] != null ? ViewState["lInvernadero"] as List<string> : new List<string>();

        invernaderosRem = new List<ListItem>();
        foreach(ListItem invernadero in lbInvernadero.Items) 
        {
            if (invernadero.Value.StartsWith(idLider + "-"))
                invernaderosRem.Add(invernadero);
        }

        if (invernaderosRem.Count > 0)
        {
            DataTable dt = ViewState["dtInvernadero"] as DataTable;

            List<DataRow> invABorrar = new List<DataRow>();
            foreach (ListItem inv in invernaderosRem)
            {
                invernaderos.Remove(inv.Value);
                lbInvernadero.Items.Remove(inv);
                foreach (DataRow row in dt.Rows)
                {
                    if (row[strIdInvernadero].ToString().Equals(inv.Value))
                    {
                        invABorrar.Add(row);
                        break;
                    }
                }
            }
            if (invABorrar.Count > 0)
            {
                foreach (DataRow row in invABorrar)
                {
                    dt.Rows.Remove(row);
                }
            }
            ViewState["dtInvernadero"] = dt;
            ViewState["lInvernadero"] = invernaderos;
            
        }
        if (lbInvernadero.Items.Count == 1 && lbInvernadero.Items[0].Value.Equals("0"))
            limpiaListaInvernadero(true);
        
    }

    private void limpiaListaInvernadero(bool vaciar)
    {
        // limpiar departamento
        lbInvernadero.ClearSelection();
        if (ViewState["lInvernadero"] != null)
        {
            ViewState["lInvernadero"] = null;
        }

        if (vaciar)
        {
            ViewState["dtInvernadero"] = buildDTInvernadero();
            lbInvernadero.DataSource = buildDTInvernadero();
            lbInvernadero.DataTextField = strNombrePlanta;
            lbInvernadero.DataValueField = strIdPlanta;
            lbInvernadero.DataBind();
        }
        
    }

    private void limpiaListaLider(bool vaciar)
    {

        // limpiar departamento
        lbLider.ClearSelection();
        if (ViewState["lLider"] != null)
        {
            ViewState["lLider"] = null;
        }
        if (vaciar)
        {
            ViewState["dtLider"] = buildDTLider();
            lbLider.DataSource = buildDTLider();
            lbLider.DataTextField = strNombrePlanta;
            lbLider.DataValueField = strIdPlanta;
            lbLider.DataBind();

        }

        limpiaListaInvernadero(true);

    }

    private void limpiaListaDepto(bool vaciar)
    {

        // limpiar departamento
        lbDepto.ClearSelection();
        if (ViewState["lDepto"] != null)
        {
            ViewState["lDepto"] = null;
        }
        //if (vaciar)
        //{
        //    lbDepto.DataSource = buildDTDeptos();
        //    lbDepto.DataTextField = strNombrePlanta;
        //    lbDepto.DataValueField = strIdPlanta;
        //    lbDepto.DataBind();
        //}
        limpiaListaLider(true);
    }

    private void limpiaListaPlanta(bool vaciar)
    {

        // limpiar departamento
        lbPlantas.ClearSelection();
        if (ViewState["lPlanta"] != null)
        {
            ViewState["lPlanta"] = null;
        }
        if (vaciar)
        {
            ViewState["dtPlanta"] = buildDTPlanta();
            lbPlantas.DataSource = buildDTPlanta();
            lbPlantas.DataTextField = strNombrePlanta;
            lbPlantas.DataValueField = strIdPlanta;
            lbPlantas.DataBind();

        }
        limpiaListaDepto(true);
    }

    private void limpiaListaPais()
    {

        // limpiar departamento
        lbPais.ClearSelection();
        if (ViewState["lPais"] != null)
        {
            ViewState["lPais"] = null;
        }

        limpiaListaPlanta(true);
    }

    private DataTable createGvHorarioOficial()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.Int32");
        column.ColumnName = strIdHorario;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strEvento;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.Boolean");
        column.ColumnName = strSeTrabaja;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strFecha;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strHoraInicio;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strHoraFin;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strTipoRepeticion;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strVNombre;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strFechaMod;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.Boolean");
        column.ColumnName = strActivo;
        data.Columns.Add(column);
        


        return data;
    }

    protected void gvHorarioOficial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (null != ViewState["dtHorario"])
            {
                DataSet ds = ViewState["dtHorario"] as DataSet;

                if (ds != null)
                {
                    gvHorarioOficial.DataSource = ds;
                    gvHorarioOficial.DataBind();
                }
            }
            ((GridView)sender).PageIndex = e.NewPageIndex;
            ((GridView)sender).DataBind();
        }
        catch (Exception)
        {
        }

    }

    private string generaStringHora(string hora, string minuto)
    {
        Int32 h = 0, m = 0;
        Int32.TryParse(hora, out h);
        Int32.TryParse(minuto, out m);
        return ((h <= 9) ? "0" + h : "" + h ) + ":" + ((m <= 9) ? "0" + m : "" + m);

    }

    private void setFecha(DateTime date)
    {
     //   txtFecha.ReadOnly = false;
        txtFecha.Text = date.ToString("yyyy-MM-dd") ;
     //   txtFecha.ReadOnly = true;
    }
}