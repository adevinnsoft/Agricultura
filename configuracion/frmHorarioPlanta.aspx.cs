using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

public partial class configuracion_frmHorarioPlanta : BasePage
{
        private String strIdHorarioPlanta = "idHorarioPlanta";
        private String strIdPlanta = "idPlanta";
        private String strNombrePlanta = "NombrePlanta";
        private String strDiaInicioSemana = "diaInicioSemana";
        private String strHoraInicio = "HoraInicio";
        private String strMinutoInicio = "minutoInicio";
        private String strHoraFin = "HoraFin";
        private String strMinutoFin = "minutoFin";
        private String strNombre = "vNombre";
        private String strFechaModifico = "fechaModifico";
        private String myCultureInfo = "";
        private Dictionary<Int32, String> listDay = null;
        private List<string> plantasSeleccionadas = null;
        private String diaSeleccionado = null;
        private int idPlanta, horaIni, horaFin, minutoIni, minutoFin, idHorario;
        private String[] hora1, hora2;
        private bool saveOk = false;
        private bool colmena = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        llenaPlantas();
        llenaGvHorarioPlanta();
        llenaDias();

    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        limpiaCampos();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            int contador = 0;
            saveOk = false;
            foreach (ListItem item in cblPlanta.Items)
            {

                if (item.Selected == true)
                {
                    idHorario = 0;
                    idPlanta = 0;
                    // Int32.TryParse(hiddenIdHorarioPlanta.Value.ToString(), out idHorario);
                    Int32.TryParse(item.Value.ToString(), out idPlanta);
                    hora1 = txtHoraInicio.Text.Trim().Split(':');
                    hora2 = txtHoraFin.Text.Trim().Split(':');
                    horaIni = 0;
                    horaFin = 0;
                    minutoIni = 0;
                    minutoFin = 0;

                    if (idPlanta > 0 && hora1.Length > 1 && hora2.Length > 1)
                    {
                        Int32.TryParse(hora1[0], out horaIni);
                        Int32.TryParse(hora1[1], out minutoIni);
                        Int32.TryParse(hora2[0], out horaFin);
                        Int32.TryParse(hora2[1], out minutoFin);


                        if (horaIni > 0 && horaFin >= horaIni && minutoIni < 60 && minutoFin < 60 && ((horaIni * 100) + minutoIni) < ((horaFin * 100) + minutoFin))
                        {
                            Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                            parameters.Add("@idHorarioPlanta", idHorario);
                            parameters.Add("@idPlanta", idPlanta);
                            parameters.Add("@diaInicioSemana", dplDiaInicio.SelectedValue);
                            parameters.Add("@horaInicio", horaIni);
                            parameters.Add("@minutoInicio", minutoIni);
                            parameters.Add("@horaFin", horaFin);
                            parameters.Add("@minutoFin", minutoFin);
                            parameters.Add("@colmena", chkColmena.Checked?1:0);
                            parameters.Add("@usuarioModifico", Int32.Parse(Session["idUsuario"].ToString()));
                            idHorario = 0;

                            DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_GuardaHorarioPlanta", parameters);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Int32.TryParse(dt.Rows[0][0].ToString(), out idHorario);
                                if (idHorario > 0)
                                {
                                    saveOk = true;
                                }
                                else
                                {
                                    saveOk = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("compruebeHora").ToString(), Comun.MESSAGE_TYPE.Warning);
                            return;

                        }

                    }
                    else
                    {
                        popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("compruebeDatos").ToString(), Comun.MESSAGE_TYPE.Warning);
                        return;
                    }
                    contador++;
                }

            }

            if (saveOk)
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("guardadoOk").ToString(), Comun.MESSAGE_TYPE.Info);
                limpiaCampos();
                llenaGvHorarioPlanta();
            }
            else if (contador == 0)
            {
                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("errorNoPlantas").ToString(), Comun.MESSAGE_TYPE.Error);
                return;
            }
            else
            {

                popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("errorGuardar").ToString(), Comun.MESSAGE_TYPE.Error);
                return;
            }
        }
        catch (Exception ex1)
        {
            Log.Error(ex1.Message);
            popUpMessageControl1.setAndShowInfoMessage(GetLocalResourceObject("errorGuardar").ToString(), Comun.MESSAGE_TYPE.Error);
            return;
        }
       
    }
    protected void gvHorarioPlanta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }
    protected void gvHorarioPlanta_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            limpiaCampos();
            DataRow row = null;
            Session["IdModuloCookie"] = gvHorarioPlanta.DataKeys[gvHorarioPlanta.SelectedIndex].Value;
            if (Session["IdModuloCookie"].ToString().Trim().Length > 0)
            {
                Dictionary<string, object> parameters = new System.Collections.Generic.Dictionary<string, object>();
                parameters.Add("@idHoraPlanta", Session["IdModuloCookie"]);
                DataTable dt = dataaccess.executeStoreProcedureDataTable("spr_ObtieneHorarioPlanta", parameters);
                if (dt.Rows.Count > 0)
                {
                    Int32 hora = 0, min = 0;
                    row = dt.Rows[0];
                    hiddenIdHorarioPlanta.Value = dt.Rows[0][strIdHorarioPlanta].ToString();
                    cblPlanta.ClearSelection();
                    foreach (ListItem item in cblPlanta.Items)
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            if (item.Value.Equals(drow["idPlanta"].ToString()))
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }

                    dplDiaInicio.ClearSelection();
                    dplDiaInicio.SelectedValue = row[strDiaInicioSemana].ToString();
                    Int32.TryParse(row[strHoraInicio].ToString(), out hora);
                    Int32.TryParse(row[strMinutoInicio].ToString(), out min);
                    txtHoraInicio.Text = hora + ":" + ((min <= 9) ? "0" + min : "" + min);
                
                    hora = 0;
                    min = 0;
                    Int32.TryParse(row[strHoraFin].ToString(), out hora);
                    Int32.TryParse(row[strMinutoFin].ToString(), out min);
                    txtHoraFin.Text = hora + ":" + ((min <= 9) ? "0" + min : "" + min);
                    chkColmena.Checked = row["aceptacolmena"].ToString() == "True";

                }
            }
            btnClear.Text = GetLocalResourceObject("Cancelar").ToString();
            btnSave.Text = GetLocalResourceObject("Actualizar").ToString();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    protected void gvHorarioPlanta_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                e.Row.Attributes["OnClick"] = Page.ClientScript.GetPostBackClientHyperlink(gvHorarioPlanta, ("Select$" + e.Row.RowIndex.ToString()));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
    protected void gvHorarioPlanta_PreRender(object sender, EventArgs e)
    {
        if (gvHorarioPlanta.HeaderRow != null)
            gvHorarioPlanta.HeaderRow.TableSection = TableRowSection.TableHeader;
    }

    
    private void llenaDias()
    {
        diaSeleccionado = "";
        diaSeleccionado = (dplDiaInicio.SelectedItem != null?dplDiaInicio.SelectedItem.Value: "0");
        dplDiaInicio.DataSource = getListDias();
        dplDiaInicio.DataTextField = "Value";
        dplDiaInicio.DataValueField = "Key";
        dplDiaInicio.SelectedValue = diaSeleccionado;
        dplDiaInicio.DataBind();
    }

    private void llenaGvHorarioPlanta()
    {
        try
        {
            Dictionary<string, object> par = new Dictionary<string, object>();
            par.Add("@idUsuario", Int32.Parse(Session["idUsuario"].ToString()));
            DataTable data = dataaccess.executeStoreProcedureDataTable("spr_ObtieneHorarioPlantas", par);
            DataTable newTable = createDataTableHorarioPlanta();
            Dictionary<Int32, String> listDias = getListDias();
            String dia = null;
            if (data != null && data.Rows.Count > 0)
            {
                DataRow newRow = null;
                foreach (DataRow row in data.Rows)
                {
                    newRow = newTable.NewRow();
                    Int32 hora = 0, min = 0;
                    newRow[strIdHorarioPlanta] = row[strIdHorarioPlanta];
                    newRow[strNombrePlanta] = row[strNombrePlanta];
                    dia = "";
                    listDias.TryGetValue((int)row[strDiaInicioSemana], out dia);
                    newRow[strDiaInicioSemana] = dia;
                    Int32.TryParse(row[strHoraInicio].ToString(), out hora);
                    Int32.TryParse(row[strMinutoInicio].ToString(), out min);
                    newRow[strHoraInicio] = hora + ":" + ((min <= 9) ? "0" + min : "" + min);

                    hora = 0;
                    min = 0;
                    Int32.TryParse(row[strHoraFin].ToString(), out hora);
                    Int32.TryParse(row[strMinutoFin].ToString(), out min);
                    newRow[strHoraFin] = hora + ":" + ((min <= 9) ? "0" + min : "" + min);

                    //newRow[strNombre] = row[strNombre];
                    //newRow[strFechaModifico] = row[strFechaModifico];
                    newRow["aceptacolmena"] = row["aceptacolmena"];

                    newTable.Rows.Add(newRow);
                }

            }
            gvHorarioPlanta.DataSource = newTable;
            gvHorarioPlanta.DataBind();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }

    private DataTable createDataTableHorarioPlanta()
    {
        DataTable data = new DataTable();
        DataColumn column = new DataColumn();

        column.DataType = Type.GetType("System.Int32");
        column.ColumnName = strIdHorarioPlanta;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strNombrePlanta;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strDiaInicioSemana;
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
        column.ColumnName = strNombre;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = strFechaModifico;
        data.Columns.Add(column);

        column = new DataColumn();
        column.DataType = Type.GetType("System.String");
        column.ColumnName = "aceptacolmena";
        data.Columns.Add(column);


        return data;
    }


    private Dictionary<Int32, String> getListDias()
    {
        if (!CultureInfo.CurrentCulture.Name.Equals(myCultureInfo))
        {
            listDay = new Dictionary<int, string>();
            for (int i = 0; i < 7; i++)
            {
                listDay.Add(i, CultureInfo.CurrentCulture.DateTimeFormat.DayNames[i]);
            }
            myCultureInfo = CultureInfo.CurrentCulture.Name;
        }
       
        return listDay;
    }


    public void llenaPlantas()
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


    private void limpiaCampos()
    {
        hiddenIdHorarioPlanta.Value = "";
        hiddenIdPlanta.Value = "";
        txtHoraInicio.Text = "";
        txtHoraFin.Text = "";
        cblPlanta.ClearSelection();
        dplDiaInicio.ClearSelection();
        btnClear.Text = GetLocalResourceObject("Limpiar").ToString();
        btnSave.Text = GetLocalResourceObject("Guardar").ToString();
        chkColmena.Checked = false;
    }
}