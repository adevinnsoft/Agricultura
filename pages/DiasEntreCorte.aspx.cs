using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;


    public partial class pages_DiasEntreCorte : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strScript = "";
            bool blnDebugSession = false;

            try
            {
                // Seteo por default el valor para la variable de session de las imagenes y variables de session:
                blnDebugSession = Convert.ToBoolean(ConfigurationManager.AppSettings["SessionDebug"]);
                if (blnDebugSession)
                {
                    Session["userIDInj"] = "1";
                    Session["usernameInj"] = "Admin";
                }

                if (UICulture.ToString().ToUpper().IndexOf("Ñ") > 0)
                {
                    this.hidEsEnEspanol.Value = "true";
                }
                else
                {
                    this.hidEsEnEspanol.Value = "false";
                }
                // Cargo el grid:
                if (!IsPostBack)
                {
                    this.dstDiasEntreCortes.DataBind();
                    this.gvDias.DataBind();
                }
                
            }
            catch (Exception exError)
            {
                strScript = "Error al inicializar la pagina:\\n" + exError.ToString();
            }

            if (strScript != "")
            {
                strScript = "<script language='javascript'> alert('" + strScript + "');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", strScript, false);

            }
            
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Vuelvo a generar visible el header para el tema del sorting:
            if (this.gvDias.Rows.Count > 0)
            {
                this.gvDias.UseAccessibleHeader = true;
                this.gvDias.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            this.dstDiasEntreCortes.DataBind();
            this.gvDias.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // Vuelvo a generar visible el header para el tema del sorting:
            if (this.gvDias.Rows.Count > 0)
            {
                this.gvDias.UseAccessibleHeader = true;
                this.gvDias.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            DiasEntreCortes objDiasEntreCorte = new DiasEntreCortes();

            string strScript = "";
            bool blnResultado = false;
            string strDias = "";
            int id;
                        
            try
                {
                   foreach (GridViewRow objRows in gvDias.Rows) 
                {
                       
                           strDias =  Convert.ToString(((TextBox)objRows.Cells[4].Controls[1]).Text.Trim());
                           Int32.TryParse(strDias, out id);
                       if (id>0)
                          {
                        objDiasEntreCorte.Dias = Convert.ToInt32(((TextBox)objRows.Cells[4].Controls[1]).Text.Trim());
                    objDiasEntreCorte.Activo = true;
                    objDiasEntreCorte.EsEnEspanol =Convert.ToBoolean(this.hidEsEnEspanol.Value);
                    objDiasEntreCorte.idCiclo = Convert.ToString(objRows.Cells[0].Text);
                    objDiasEntreCorte.IdUsuario = Convert.ToInt32(Session["userIDInj"]);
                  blnResultado=  objDiasEntreCorte.MantenimientoCatalogo(1);
                          }

                      
                }
                
                        if (blnResultado == true)
                        {

                            if (this.hidEsEnEspanol.Value == "true")
                            {
                                strScript = "Registro guardado satisfactoriamente.\\n";
                            }
                            else
                            {
                                strScript = "Record successfully saved.\\n";
                            }
                                                        
                        this.dstDiasEntreCortes.DataBind();
                        this.gvDias.DataBind();
                    }

                }
                catch (Exception exError)
                {
                    if (this.hidEsEnEspanol.Value == "true")
                    {
                        strScript = "Error al guardar el registro en la clase DiasEntreCortes:MantenimientoCatalogo(1).\\n" + exError.Message;
                    }
                    else
                    {
                        strScript = "Failed to save the record to the class DiasEntreCortes:MantenimientoCatalogo(1).\\n" + exError.Message;
                    }

                }
                finally
                {
                    objDiasEntreCorte = null;
                   
                }
            

            if (!string.IsNullOrEmpty(strScript))
            {
                strScript = "<script language='javascript'> alert('" + strScript + "');</script>";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Error", strScript, false);
           
            }
           
        }

        protected void gvDias_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (this.gvDias.Rows.Count > 0)
            {
                this.gvDias.UseAccessibleHeader = true;
                this.gvDias.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gvDias_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[0].Visible = false;
        }
    }
