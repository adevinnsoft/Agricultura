using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Class
{

    public class ConfiguracionCorreo
    {
        // Declaro variables privadas de la clase:
        private int intIdAdministracionCorreo = 0;
        private int intIdCaptura = 0;
        private int intIdUsuario = 0;
        private int intIdUsuarioNoEnviar = 0;
        private int intIdEnviarCorreo = 0;
        private int intDiasParaComenzarEstudio = 0;
        private int intDiasParaEfectuarEstudio = 0;
        private bool blnNotificacionActiva = false;
        private int intDiasParaEnvioDeNotificacion = 0;
        private string strCaptura = "";
        private string strRolCapturista = "";
        private string strCorreo = "";
        private bool blnEsEnEspanol = true;
        private int intErrorNumber = 0;
        private string strErrorMessage = "";

        // Declaro las propiedades de la clase:
        public int IdAdministracionCorreo
        {
            get { return this.intIdAdministracionCorreo; }
            set { this.intIdAdministracionCorreo = value; }
        }
        public int IdCaptura
        {
            get { return this.intIdCaptura; }
            set { this.intIdCaptura = value; }
        }
        public int IdUsuarioCaptura
        {
            get { return this.intIdUsuario; }
            set { this.intIdUsuario = value; }
        }
        public int IdUsuarioNoEnviar
        {
            get { return this.intIdUsuarioNoEnviar; }
            set { this.intIdUsuarioNoEnviar = value; }
        }
        public int IdEnviarCorreo 
        {
            get { return this.intIdEnviarCorreo; }
            set { this.intIdEnviarCorreo = value; }
        }
        public int DiasParaComenzarEstudio
        {
            get { return this.intDiasParaComenzarEstudio; }
            set { this.intDiasParaComenzarEstudio = value; }
        }
        public int DiasParaEfectuarEstudio
        {
            get { return this.intDiasParaEfectuarEstudio; }
            set { this.intDiasParaEfectuarEstudio = value; }
        }
        public bool NotificacionActiva
        {
            get { return this.blnNotificacionActiva; }
            set { this.blnNotificacionActiva = value; }
        }
        public int DiasParaEnvioDeNotificacion
        {
            get { return this.intDiasParaEnvioDeNotificacion; }
            set { this.intDiasParaEnvioDeNotificacion = value; }
        }
        public string Captura
        {
            get { return this.strCaptura; }
            set { this.strCaptura = value; }
        }
        public string RolCapturista
        {
            get { return this.strRolCapturista; }
            set { this.strRolCapturista = value; }
        }
        public string Correo
        {
            get { return this.strCorreo; }
            set { this.strCorreo = value; }
        }
        public bool EsEnEspanol
        {
            get { return this.blnEsEnEspanol; }
            set { this.blnEsEnEspanol = value; }
        }
        public int ErrorNumber
        {
            get { return this.intErrorNumber; }
        }
        public string ErrorMessage
        {
            get { return this.strErrorMessage; }
        }

        // Declaro las funciones de la clase:
        public DataTable ObtenerListaDeDistribucion()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlDataAdapter daAdapter = new SqlDataAdapter();
            SqlParameter prmGeneric = null;
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerListaDeDistribucion", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                //Output Parameters
                prmGeneric = new SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                intErrorNumber = Convert.ToInt32(cmdQuery.Parameters["@NumeroDeError"].Value);
                strErrorMessage = Convert.ToString(cmdQuery.Parameters["@MensajeDeError"].Value);

                if (intErrorNumber == 0)
                {
                    return tblTable;
                }
                else
                {
                    return null;
                }
            }
            catch (SqlException exError)
            {
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.ErrorCode;
                return null;
            }
            finally
            {
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }

                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (prmGeneric != null)
                {
                    prmGeneric = null;
                }
                if (cnnMain != null)
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter = null;
                }
            }
        }

        public bool ObtenerDatosDeCaptura()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlDataAdapter daAdapter = new SqlDataAdapter();
            SqlParameter prmGeneric = null;
            DataTable tblTable = null;
            bool blnResult = false;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerDatosCorreoPorCaptura", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@Captura", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Captura;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //Output Parameters
                prmGeneric = new SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                intErrorNumber = Convert.ToInt32(cmdQuery.Parameters["@NumeroDeError"].Value);
                strErrorMessage = Convert.ToString(cmdQuery.Parameters["@MensajeDeError"].Value);

                if (intErrorNumber == 0)
                {
                    blnResult = true;
                    if (tblTable.Rows.Count > 0)
                    {
                        foreach (DataRow DRow in tblTable.Rows)
                        {
                            this.IdAdministracionCorreo = Convert.ToInt32(DRow["IdAdministracionCorreo"]);
                            this.IdCaptura = Convert.ToInt32(DRow["IdCaptura"]);
                            this.DiasParaComenzarEstudio = Convert.ToInt32(DRow["DiasParaComenzarEstudio"]);
                            this.DiasParaEfectuarEstudio = Convert.ToInt32(DRow["DiasParaEfectuarEstudio"]);
                            this.NotificacionActiva = Convert.ToBoolean(DRow["NotificacionActiva"]);
                            this.DiasParaEnvioDeNotificacion = Convert.ToInt32(DRow["DiasParaEnvioDeNotificacion"]);
                        }
                    }
                    else
                    {
                        this.IdAdministracionCorreo = 0;
                        this.IdCaptura = 0;
                        this.DiasParaComenzarEstudio = 0;
                        this.DiasParaEfectuarEstudio = 0;
                        this.NotificacionActiva = false;
                        this.DiasParaEnvioDeNotificacion = 0;
                    }
                }
                else
                {
                    blnResult = false;
                }
            }
            catch (SqlException exError)
            {
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.ErrorCode;
                blnResult = false;
            }
            finally
            {
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }

                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (prmGeneric != null)
                {
                    prmGeneric = null;
                }
                if (cnnMain != null)
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter = null;
                }
            }
            return blnResult;
        }

        public bool GuardarDatos()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            bool blnResult = false;
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_AlmacenarListaDeDistribucion", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                //prmGeneric = new SqlParameter("@Captura", System.Data.SqlDbType.NVarChar);
                //prmGeneric.Value = this.Captura; 
                //prmGeneric.Direction = ParameterDirection.Input;
                //cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@IdUsuarioCaptura", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuarioCaptura;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);
                blnResult = true;
            }
            catch (SqlException exError)
            {
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.ErrorCode;
                blnResult = false;
            }
            finally
            {
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (prmGeneric != null)
                    prmGeneric = null;
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }
                if ((cnnMain != null))
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return blnResult;
        }

        public bool AltaEnviarCorreo()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            bool blnResult = false;
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_AltaEnviarCorreoPorCaptura", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@idAdministracionCorreo", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdAdministracionCorreo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@idUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuarioCaptura;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@Correo", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Correo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                this.intErrorNumber = Convert.ToInt32(cmdQuery.Parameters["@NumeroDeError"].Value);
                this.strErrorMessage = Convert.ToString(cmdQuery.Parameters["@MensajeDeError"].Value);

                if (this.intErrorNumber == 0)
                {
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }

            }
            catch (SqlException exError)
            {
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.ErrorCode;
                blnResult = false;
            }
            finally
            {
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (prmGeneric != null)
                    prmGeneric = null;
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }
                if ((cnnMain != null))
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return blnResult;
        }

        public bool AltaNoEnviarCorreo()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            bool blnResult = false;
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_AltaNoEnviarCorreoPorCaptura", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@idAdministracionCorreo", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdAdministracionCorreo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@idUsuarioNoEnviar", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuarioNoEnviar;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@idUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuarioCaptura;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                this.intErrorNumber = Convert.ToInt32(cmdQuery.Parameters["@NumeroDeError"].Value);
                this.strErrorMessage = Convert.ToString(cmdQuery.Parameters["@MensajeDeError"].Value);

                if (this.intErrorNumber == 0)
                {
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }

            }
            catch (SqlException exError)
            {
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.ErrorCode;
                blnResult = false;
            }
            finally
            {
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (prmGeneric != null)
                    prmGeneric = null;
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }
                if ((cnnMain != null))
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return blnResult;
        }

        public bool EliminarCorreoDeCaptura()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            bool blnResult = false;
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_EliminarEnviarCorreoPorCaptura", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@idEnviarCorreo", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdEnviarCorreo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@idUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuarioCaptura;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                this.intErrorNumber = Convert.ToInt32(cmdQuery.Parameters["@NumeroDeError"].Value);
                this.strErrorMessage = Convert.ToString(cmdQuery.Parameters["@MensajeDeError"].Value);

                if (this.intErrorNumber == 0)
                {
                    blnResult = true;
                }
                else
                {
                    blnResult = false;
                }

            }
            catch (SqlException exError)
            {
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.ErrorCode;
                blnResult = false;
            }
            finally
            {
                if (tblTable != null)
                {
                    tblTable.Dispose();
                    tblTable = null;
                }
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (prmGeneric != null)
                    prmGeneric = null;
                if (daAdapter != null)
                {
                    daAdapter.Dispose();
                    daAdapter = null;
                }
                if (cmdQuery != null)
                {
                    cmdQuery.Dispose();
                    cmdQuery = null;
                }
                if (cnnMain.State == ConnectionState.Open)
                {
                    cnnMain.Close();
                }
                if ((cnnMain != null))
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return blnResult;
        }
    }

}