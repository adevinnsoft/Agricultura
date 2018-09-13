using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Configuration;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strScript = "";
        bool blnDebugSession = false;
        EstatusGrowing objEstatus = new EstatusGrowing();
        Planta objPlanta = new Planta();
        CalendarioNS objCalendario = new CalendarioNS();
        bool blnEspanol = false;

        try
        {
            if (!this.IsPostBack)
            {
                // Seteo por default el valor para la variable de session de las imagenes y variables de session:
                this.Session["UserTemp"] = "";
                Session["ImagesUpload"] = null;
                blnDebugSession = Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]);
                if (blnDebugSession)
                {
                    Session["userIDInj"] = "0";
                    Session["usernameInj"] = "Admin";
                }

                if (Session["Locale"].ToString() == "es-MX")
                {
                    blnEspanol = true;
                }
                else
                {
                    blnEspanol = false;
                }
                // Obtengo el catalogo de estatus:
                this.ddlEstatus.Items.Clear();
                this.ddlEstatus.DataSource = objEstatus.ObtenerListaDeEstatus(blnEspanol);
                this.ddlEstatus.DataValueField = "idEstatusGrowing";
                this.ddlEstatus.DataTextField = "Nombre";
                this.ddlEstatus.DataBind();
                this.ddlEstatus.Items.Insert(0, "");
                // Obtengo el catalogo de plantas:
                this.ddlPlanta.Items.Clear();
                this.ddlPlanta.DataSource = objPlanta.ObtenerLista();
                this.ddlPlanta.DataValueField = "idPlanta";
                this.ddlPlanta.DataTextField = "NombrePlanta";
                this.ddlPlanta.DataBind();
                this.ddlPlanta.Items.Insert(0, "");
                // Obtengo el Calendario NS:
                this.ddlSemana.Items.Clear();
                this.ddlSemana.DataSource = objCalendario.ObtenerCalendarioNS(blnEspanol);
                this.ddlSemana.DataValueField = "vWeek";
                this.ddlSemana.DataTextField = "vWeek";
                this.ddlSemana.DataBind();
                this.ddlSemana.Items.Insert(0, "");

                this.ddlPlanta.Focus();
            }
        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            objCalendario = null;
            objEstatus = null;
            objPlanta = null;
            if (strScript != "")
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
        }
    }

    protected void ddlPlanta_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strScript = "";
        Invernadero objInvernadero = new Invernadero();

        try
        {
            if (this.ddlPlanta.SelectedItem.Text != "")
            {
                objInvernadero.idPlanta = Convert.ToInt32(this.ddlPlanta.SelectedItem.Value);
                this.ddlInvernadero.Items.Clear();
                this.ddlInvernadero.DataSource = objInvernadero.ObtenerLista();
                this.ddlInvernadero.DataValueField = "IdInvernadero";
                this.ddlInvernadero.DataTextField = "ClaveInvernadero";
                this.ddlInvernadero.DataBind();
                this.ddlInvernadero.Items.Insert(0, "");
            }
            else
            {
                this.ddlInvernadero.Items.Clear();
                this.ddlInvernadero.DataSource = null;
                this.ddlInvernadero.DataBind();
            }
        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    public bool ValidarValoresParaBusqueda()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:\\n";
        string strMensajeEN = "Please specify the following fields:\\n";

        if (this.ddlPlanta.Text == "")
        {
            strMensaje = strMensaje + "* Planta\\n";
            strMensajeEN = strMensajeEN + "* Plant\\n";
            blnResult = false;
        }

        if (this.ddlInvernadero.Text == "")
        {
            strMensaje = strMensaje + "* Invernadero\\n";
            strMensajeEN = strMensajeEN + "* Greenhouse\\n";
            blnResult = false;
        }

        if (this.ddlSemana.Text == "")
        {
            strMensaje = strMensaje + "* Semana\\n";
            strMensajeEN = strMensajeEN + "* Week\\n";
            blnResult = false;
        }
       
        if (blnResult == false)
        {
            strMensaje = "<script language='javascript'>popUpAlert('" + strMensaje + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
        }

        return blnResult;
    }

    public bool ValidarValoresParaGuardar()
    {
        bool blnResult = true;
        string strMensaje = "Favor de especificar los siguientes campos:\\n";
        string strMensajeEN = "Please specify the following fields:\\n";

        if (this.ddlPlanta.Text == "")
        {
            strMensaje = strMensaje + "* Planta\\n";
            strMensajeEN = strMensajeEN + "* Plant\\n";
            blnResult = false;
        }

        if (this.ddlInvernadero.Text == "")
        {
            strMensaje = strMensaje + "* Invernadero\\n";
            strMensajeEN = strMensajeEN + "* Greenhouse\\n";
            blnResult = false;
        }


        if (this.ddlEstatus.Text == "")
        {
            strMensaje = strMensaje + "* Estatus\\n";
            strMensajeEN = strMensajeEN + "* Status\\n";
            blnResult = false;
        }

        if (this.rdbPlantacion.Text == "")
        {
            strMensaje = strMensaje + "* Plantación o No Plantación\\n";
            strMensajeEN = strMensajeEN + "* Plantation or not plantation\\n";
            blnResult = false;
        }

        if (this.ddlSemana.Text == "")
        {
            strMensaje = strMensaje + "* Semana\\n";
            strMensajeEN = strMensajeEN + "* Week\\n";
            blnResult = false;
        }

        if (blnResult == false)
        {
            strMensaje = "<script language='javascript'> popUpAlert('" + strMensaje + "','error');</script>";
            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strMensaje, false);
        }

        return blnResult;
    }

    protected void btnCargarDatos_Click(object sender, EventArgs e)
    {
        if (ValidarValoresParaBusqueda() == true)
        {
            string strScript = "";

            try
            {
                this.hidPKGrower.Value = "1";
                this.hidPKGteZona.Value = "1";
                this.hidPKLider.Value = "1";

                if (!this.CargaDatosGrid(ref this.grdPlantacion, this.ddlSemana.SelectedItem.Value, "Plantacion"))
                {
                    strScript = "Error al cargar los datos de la sección de plantación.";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdControlClima, this.ddlSemana.SelectedItem.Value, "Control de Clima"))
                {
                    strScript = "Error al cargar los datos de la sección de Control de Clima.";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdTrabajoCultural, this.ddlSemana.SelectedItem.Value, "Trabajos Culturales"))
                {
                    strScript = "Error al cargar los datos de la sección de Trabajos Culturales.";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdPlagas, this.ddlSemana.SelectedItem.Value, "Plagas y Enfermedades"))
                {
                    strScript = "Error al cargar los datos de la sección de Plagas y Enfermedades.";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdTrampeo, this.ddlSemana.SelectedItem.Value, "Trampeo"))
                {
                    strScript = "Error al cargar los datos de la sección de Trampeo.";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdLimpieza, this.ddlSemana.SelectedItem.Value, "Limpieza de Invernaderos"))
                {
                    strScript = "Error al cargar los datos de la sección de Limpieza de invernaderos.";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdFertirriego, this.ddlSemana.SelectedItem.Value, "Fertirriego"))
                {
                    strScript = "Error al cargar los datos de la sección de Fertirriego .";
                    return;
                }
                if (!this.CargaDatosGrid(ref this.grdEstadoDelFruto, this.ddlSemana.SelectedItem.Value, "Estado del Fruto"))
                {
                    strScript = "Error al cargar los datos de la sección de Estado del Fruto .";
                    return;
                }

                if (!this.CargaDatosGrid(ref this.grdPolinizacion, this.ddlSemana.SelectedItem.Value, "Polinizacion"))
                {
                    strScript = "Error al cargar los datos de la sección de plantación.";
                    return;
                }


            }
            catch (Exception ex)
            {
                strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
            }
            finally
            {


                if (strScript != "")
                {
                    strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
                }
                else
                {
                    //strScript = "<script language='javascript'> CargaAcordion();</script>";
                    //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
                }
            }
        }
    }

    public bool CargaDatosGrid(ref GridView gvData, string strSemana, string strGrupo)
    {

        string strScript = "";
        ParametrosGrowing objParametros = new ParametrosGrowing();

        try
        {
            objParametros.Grupo = strGrupo;
            gvData.DataSource = objParametros.ObtenerListaPorGrupo();
            gvData.DataBind();
        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }



        return true;
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        if (ValidarValoresParaGuardar() == true)
        {
        Int32 intRow = 0;
        Growing objGrowing = new Growing();
        decimal decPuntaje = 0;
        bool blnContinue = true;

        string strScript = "";
        try
        {
            objGrowing.idPlanta = Convert.ToInt32(this.ddlPlanta.SelectedValue);
            objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedValue);
            objGrowing.idEstatusGrowing = Convert.ToInt32(this.ddlEstatus.SelectedValue);
            //objGrowing.idGrower = Convert.ToInt32(this.hidPKGrower.Value);
            //objGrowing.idGerente = Convert.ToInt32(this.hidPKGteZona.Value);
            //objGrowing.idLider = Convert.ToInt32(this.hidPKLider.Value);

            objGrowing.Grower = this.txtGrower.Text.ToString();
            objGrowing.Gerente = this.txtGteZona.Text.ToString();
            objGrowing.Lider = this.txtLider.Text.ToString();

            objGrowing.idLider = Convert.ToInt32(this.hidPKLider.Value);
            objGrowing.IdUsuario = Convert.ToInt32(Session["userIDInj"]);
            objGrowing.Anio = Convert.ToInt32(this.ddlSemana.SelectedValue.ToString().Substring(0, 4).ToString());
            objGrowing.NoSemana = Convert.ToInt32(this.ddlSemana.SelectedValue.ToString().Substring(5, 2).ToString());
            objGrowing.Semana = this.ddlSemana.SelectedValue.ToString();
            if (this.rdbPlantacion.SelectedValue.ToString() == "1")
            {
                objGrowing.EsPlantacion = true;
            }
            else
            {
                objGrowing.EsPlantacion = false;
            }
            objGrowing.MantenimientoGrowing(1);

            blnContinue = GuardarDatosGrid(ref this.grdPlantacion, "Plantacion", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdControlClima, "Control de Clima", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdTrabajoCultural, "Trabajos Culturales", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdPlagas, "Plagas y Enfermedades", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdTrampeo, "Trampeo", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdLimpieza, "Limpieza de Invernaderos", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdFertirriego, "Fertirriego ", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdEstadoDelFruto, "Estado del Fruto", objGrowing.idMastGrowing);
            blnContinue = GuardarDatosGrid(ref this.grdPolinizacion, "Polinizacion", objGrowing.idMastGrowing);

            if (blnContinue == true)
            {
                strScript = "Registro guardado";
            }
            else
            {
                strScript = objGrowing.ErrorMessage;
            }

        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            if (strScript != "")
            {
                if (blnContinue == true)
                {
                    strScript = "<script language='javascript'> popUpAlert('" + strScript + "','ok');</script>";
                }
                else
                {
                    strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
                }
              
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    }

    public bool GuardarDatosGrid(ref GridView gvData, string strGrupo, int MastGrowing)
    {
        int intRow;
        int intRowListas;
        decimal decPuntaje = 0;
        string strScript = "";
        Growing objGrowing = new Growing();

        try
        {
            intRow = 0;
            while (intRow <= gvData.Rows.Count - 1)
            {
                try
                {
                     if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[1]).SelectedItem.Text == "Ok")
                                    {
                                        decPuntaje = decPuntaje + Convert.ToDecimal(gvData.Rows[intRow].Cells[9].Text);
                                    }
                }
                catch
                {
                }
               
                intRow = intRow + 1;
            }
            objGrowing.idMastGrowing = MastGrowing;
            objGrowing.IdUsuario = Convert.ToInt32(Session["userIDInj"]);
            objGrowing.PuntajeObtenido = decPuntaje;
            objGrowing.Grupo = strGrupo;
            if (this.rdbPlantacion.SelectedValue.ToString() == "1")
            {
                objGrowing.EsPlantacion = true;
            }
            else
            {
                objGrowing.EsPlantacion = false;
            }
            objGrowing.MantenimientoPuntajePorGrupo(1);

            intRow = 0;
            while (intRow <= gvData.Rows.Count - 1)
            {
                objGrowing.IdParametroPorGrupoGrowing = Convert.ToInt32(gvData.Rows[intRow].Cells[0].Text);
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[1]).SelectedItem.Text == "Ok")
                {
                    objGrowing.PuntajeObtenido = Convert.ToDecimal(gvData.Rows[intRow].Cells[9].Text);
                }
                // Se barre el radiobuton NA_OK_X
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[1]).SelectedValue == "N/A")
                {
                    objGrowing.NA = true;
                }
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[1]).SelectedValue == "Ok")
                {
                    objGrowing.OK = true;
                }
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[1]).SelectedValue == "X")
                {
                    objGrowing.X = true;
                }
                // Se barre el radiobuton S_A_G_N
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[7]).SelectedValue == "S")
                {
                    objGrowing.S = true;
                }
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[7]).SelectedValue == "A")
                {
                    objGrowing.A = true;
                }
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[7]).SelectedValue == "G")
                {
                    objGrowing.G = true;
                }
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[7]).SelectedValue == "N/A")
                {
                    objGrowing.N = true;
                }

                objGrowing.MantenimientoDetGrowing(1);
                objGrowing.NA = false;
                objGrowing.OK = false;
                objGrowing.X = false;
                objGrowing.S = false;
                objGrowing.A = false;
                objGrowing.G = false;
                objGrowing.N = false;

                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[3]).Items.Count > 0)
                {
                    objGrowing.IdCatalogoListaNA_OK_X_PorParametro = Convert.ToInt32(((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[3]).SelectedValue);
                    objGrowing.MantenimientoDetNA_OK_X(1);
                }
                if (((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[9]).Items.Count > 0)
                {
                    objGrowing.IdCatalogoListaS_A_G_N_PorParametro = Convert.ToInt32(((RadioButtonList)gvData.Rows[intRow].Cells[10].Controls[9]).SelectedValue);
                    objGrowing.MantenimientoDetS_A_G_N(1);
                }


                intRowListas = 0;
                while (intRowListas <= ((CheckBoxList)gvData.Rows[intRow].Cells[10].Controls[5]).Items.Count - 1)
                {
                    if (((CheckBoxList)gvData.Rows[intRow].Cells[10].Controls[5]).Items[intRowListas].Selected == true)
                    {
                        objGrowing.IdCatalogoListaNA_OK_X_PorParametro = Convert.ToInt32(((CheckBoxList)gvData.Rows[intRow].Cells[10].Controls[5]).Items[intRowListas].Value);
                        objGrowing.MantenimientoDetNA_OK_X(1);
                    }
                    intRowListas = intRowListas + 1;
                }

                intRowListas = 0;
                while (intRowListas <= ((CheckBoxList)gvData.Rows[intRow].Cells[10].Controls[11]).Items.Count - 1)
                {
                    if (((CheckBoxList)gvData.Rows[intRow].Cells[10].Controls[11]).Items[intRowListas].Selected == true)
                    {
                        objGrowing.IdCatalogoListaS_A_G_N_PorParametro = Convert.ToInt32(((CheckBoxList)gvData.Rows[intRow].Cells[10].Controls[11]).Items[intRowListas].Value);
                        objGrowing.MantenimientoDetS_A_G_N(1);
                    }
                    intRowListas = intRowListas + 1;
                }


                intRow = intRow + 1;
            }
            return true;
        }

        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
            return false;
        }
        //finally
        //{
        //    if (strScript != "")
        //    {
        //        strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
        //    }
        //    else
        //    {
        //        strScript = "<script language='javascript'> CargaAcordion();</script>";
        //        System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
        //    }
        //}

    }


    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        string strScript = "";
        try
        {
            Response.Redirect("~/Pages/frmGrowingConditions.aspx", false);
        }
        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");
        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerPlantacion_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdPlantacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[11]).ClearSelection();



                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void rdbOpcionMasivaPlantacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intRow = 0;

        if (this.rdbOpcionMasivaPlantacion.SelectedValue == "1")
        {
            while (intRow <= this.grdPlantacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaPlantacion.SelectedValue == "2")
        {
            while (intRow <= this.grdPlantacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaPlantacion.SelectedValue == "3")
        {
            while (intRow <= this.grdPlantacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlantacion.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
    }
    protected void btnRestablecerControlClima_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdControlClima.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdControlClima.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdControlClima.Rows[intRow].Cells[10].Controls[11]).ClearSelection();
                                
                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'> popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerTrabajoCultural_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdTrabajoCultural.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[11]).ClearSelection();
                
                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerPlagas_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdPlagas.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdPlagas.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdPlagas.Rows[intRow].Cells[10].Controls[11]).ClearSelection();
                
                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerTrampeo_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdTrampeo.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[11]).ClearSelection();



                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerLimpieza_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdLimpieza.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[11]).ClearSelection();
                
                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerFertirriego_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdFertirriego.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[11]).ClearSelection();
                
                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void btnRestablecerEstadoDelFruto_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdEstadoDelFruto.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[11]).ClearSelection();



                intRow = intRow + 1;
            }

        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
  
    protected void grdPolinizacion_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }

    protected void grdPolinizacion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString()==chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }                    

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void btnRestablecerPolinizacion_Click(object sender, EventArgs e)
    {
        string strScript = "";
        Int32 intRow = 0;
        try
        {

            while (intRow <= this.grdPolinizacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[1]).SelectedIndex = -1;
                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[3]).SelectedIndex = -1;
                ((CheckBoxList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[5]).ClearSelection();

                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[7]).SelectedIndex = -1;
                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[9]).SelectedIndex = -1;
                ((CheckBoxList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[11]).ClearSelection();



                intRow = intRow + 1;
            }
           
        }


        catch (Exception ex)
        {
            strScript = "Error: " + ex.Message.ToString().Replace("'", "").Replace("\n", "\\n").Replace("\r", "\\n").Replace(Convert.ToString((char)10), "\\n").Replace(Convert.ToString((char)13), "\\n");

        }
        finally
        {
            if (strScript != "")
            {
                strScript = "<script language='javascript'>popUpAlert('" + strScript + "','error');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);
            }
            else
            {
                //strScript = "<script language='javascript'> CargaAcordion();</script>";
                //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "CargaAcordion", strScript, false);
            }
        }
    }
    protected void rdbOpcionMasivaPolinizacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intRow = 0;

        if (this.rdbOpcionMasivaPolinizacion.SelectedValue == "1")
        {
            while (intRow <= this.grdPolinizacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaPolinizacion.SelectedValue == "2")
        {
            while (intRow <= this.grdPolinizacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaPolinizacion.SelectedValue == "3")
        {
            while (intRow <= this.grdPolinizacion.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPolinizacion.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
    }
    protected void grdPlantacion_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdControlClima_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdTrabajoCultural_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdPlagas_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdTrampeo_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdLimpieza_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdFertirriego_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdEstadoDelFruto_RowCreated(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[0].Visible = false;
        e.Row.Cells[1].Visible = false;
        e.Row.Cells[3].Visible = false;
        e.Row.Cells[4].Visible = false;
        e.Row.Cells[5].Visible = false;
        e.Row.Cells[6].Visible = false;
        e.Row.Cells[7].Visible = false;
        e.Row.Cells[8].Visible = false;
        e.Row.Cells[9].Visible = false;
    }
    protected void grdPlantacion_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdControlClima_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }     

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdTrabajoCultural_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdPlagas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdTrampeo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdLimpieza_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdFertirriego_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void grdEstadoDelFruto_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int id;
        string strNumerico;
        ParametrosGrowing objParametros = new ParametrosGrowing();
        Growing objGrowing = new Growing();
        DataTable tblTable = null;
        int intRow = 0;

        try
        {
            strNumerico = e.Row.Cells[0].Text;
            Int32.TryParse(strNumerico, out id);
            if (id > 0)
            {

                objParametros.ParametroPorGrupoGrowing = Convert.ToInt16(e.Row.Cells[0].Text);

                //------------------------------INICIO    NA OK X------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[4].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[5].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataTextField = "SubParametroNAOKX";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("NAOKX");
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataTextField = "SubParametroNAOKX";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataValueField = "idCatalogoListaNA_OK_X_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[3]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN     NA OK X------------------------------------

                //------------------------------INICIO    S A G NA------------------------------------
                if (Convert.ToBoolean(e.Row.Cells[6].Text) == true)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).Visible = true;          //NA OK X  "Visible"

                    if (Convert.ToBoolean(e.Row.Cells[7].Text) == true)
                    {
                        if (Convert.ToBoolean(e.Row.Cells[8].Text) == true)    // Si es Multiselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataSource = tblTable;
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataTextField = "SubParametroSAGN";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).DataBind();
                            ((CheckBoxList)e.Row.Cells[10].Controls[11]).Visible = true;
                        }
                        else                                              //Si es uniselección
                        {
                            tblTable = objParametros.ObtenerDetalleListaPorGrupo("SAGN");
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataSource = tblTable;
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataTextField = "SubParametroSAGN";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataValueField = "idCatalogoListaS_A_G_N_PorParametro";
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).DataBind();
                            ((RadioButtonList)e.Row.Cells[10].Controls[9]).Visible = true;
                        }
                    }
                    else
                    {
                        ((CheckBoxList)e.Row.Cells[10].Controls[5]).Visible = false;
                    }
                }
                else
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).Visible = false;       //NA OK X  "NO Visible"
                }

                //------------------------------FIN      S A G NA-----------------------------------
                //-----------------------------Se llenan los campos de los RadiobutonListPrincipales
                objGrowing.IdParametroPorGrupoGrowing = objParametros.ParametroPorGrupoGrowing;
                objGrowing.idInvernadero = Convert.ToInt32(this.ddlInvernadero.SelectedItem.Value);
                objGrowing.Semana = this.ddlSemana.SelectedValue;
                tblTable = objGrowing.ObtenerNAOK_SAGN();
                if (tblTable.Rows.Count > 0)
                {
                    ((RadioButtonList)e.Row.Cells[10].Controls[1]).SelectedValue = tblTable.Rows[0][0].ToString();
                    ((RadioButtonList)e.Row.Cells[10].Controls[7]).SelectedValue = tblTable.Rows[0][1].ToString();
                }

                tblTable = objGrowing.ObtenerListaNAOK();
                if (tblTable.Rows.Count > 0)
                {
                    intRow = 0;
                    while (intRow <= tblTable.Rows.Count - 1)
                    {

                        foreach (ListItem chkitem in ((CheckBoxList)e.Row.Cells[10].Controls[5]).Items)
                        {
                            if (tblTable.Rows[intRow][0].ToString() == chkitem.Value)
                            {
                                chkitem.Selected = true;
                            }
                        }

                        intRow = intRow + 1;
                    }

                }
            }

        }
        catch
        {


        }
        finally
        {
            objGrowing = null;
            objParametros = null;
        }
    }
    protected void rdbOpcionMasivaTrabajoCultural_SelectedIndexChanged(object sender, EventArgs e)
    {
               int intRow = 0;

        if (this.rdbOpcionMasivaTrabajoCultural.SelectedValue == "1")
        {
            while (intRow <= this.grdTrabajoCultural.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaTrabajoCultural.SelectedValue == "2")
        {
            while (intRow <= this.grdTrabajoCultural.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaTrabajoCultural.SelectedValue == "3")
        {
            while (intRow <= this.grdTrabajoCultural.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrabajoCultural.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
    }
    protected void rdbOpcionMasivaControlClima_SelectedIndexChanged(object sender, EventArgs e)
    {
        int intRow = 0;

        if (this.rdbOpcionMasivaControlClima.SelectedValue == "1")
        {
            while (intRow <= this.grdControlClima.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaControlClima.SelectedValue == "2")
        {
            while (intRow <= this.grdControlClima.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaControlClima.SelectedValue == "3")
        {
            while (intRow <= this.grdControlClima.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdControlClima.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
    }
    protected void  rdbOpcionMasivaPlagas_SelectedIndexChanged(object sender, EventArgs e)
{
                int intRow = 0;

        if (this.rdbOpcionMasivaPlagas.SelectedValue == "1")
        {
            while (intRow <= this.grdPlagas.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaPlagas.SelectedValue == "2")
        {
            while (intRow <= this.grdPlagas.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaPlagas.SelectedValue == "3")
        {
            while (intRow <= this.grdPlagas.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdPlagas.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
}
protected void  rdbOpcionMasivaTrampeo_SelectedIndexChanged(object sender, EventArgs e)
{
            int intRow = 0;

        if (this.rdbOpcionMasivaTrampeo.SelectedValue == "1")
        {
            while (intRow <= this.grdTrampeo.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaTrampeo.SelectedValue == "2")
        {
            while (intRow <= this.grdTrampeo.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaTrampeo.SelectedValue == "3")
        {
            while (intRow <= this.grdTrampeo.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdTrampeo.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
}
protected void  rdbOpcionMasivaLimpieza_SelectedIndexChanged(object sender, EventArgs e)
{
            int intRow = 0;

        if (this.rdbOpcionMasivaLimpieza.SelectedValue == "1")
        {
            while (intRow <= this.grdLimpieza.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaLimpieza.SelectedValue == "2")
        {
            while (intRow <= this.grdLimpieza.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaLimpieza.SelectedValue == "3")
        {
            while (intRow <= this.grdLimpieza.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdLimpieza.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
}
protected void  rdbOpcionMasivaFertirriego_SelectedIndexChanged(object sender, EventArgs e)
{
            int intRow = 0;

        if (this.rdbOpcionMasivaFertirriego.SelectedValue == "1")
        {
            while (intRow <= this.grdFertirriego.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaFertirriego.SelectedValue == "2")
        {
            while (intRow <= this.grdFertirriego.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaFertirriego.SelectedValue == "3")
        {
            while (intRow <= this.grdFertirriego.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdFertirriego.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
}
protected void  rdbOpcionMasivaEstadoDelFruto_SelectedIndexChanged(object sender, EventArgs e)
{
            int intRow = 0;

        if (this.rdbOpcionMasivaEstadoDelFruto.SelectedValue == "1")
        {
            while (intRow <= this.grdEstadoDelFruto.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "N/A";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaEstadoDelFruto.SelectedValue == "2")
        {
            while (intRow <= this.grdEstadoDelFruto.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "Ok";
                intRow = intRow + 1;
            }
        }
        if (this.rdbOpcionMasivaEstadoDelFruto.SelectedValue == "3")
        {
            while (intRow <= this.grdEstadoDelFruto.Rows.Count - 1)
            {
                ((RadioButtonList)this.grdEstadoDelFruto.Rows[intRow].Cells[10].Controls[1]).SelectedValue = "X";
                intRow = intRow + 1;
            }
        }
}
}

