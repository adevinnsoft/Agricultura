using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Class
{
    public class Notificaciones
    {
        private int intIdNotificacion = 0;
        private int intIdDepartamento = 0;
        private int intIdRol = 0;
        private int intIdUsuario = 0;
        private bool blnEsParaTodos = false;
        private string strMensaje = "";
        private bool blnEsEnEspanol = false;
        private int intErrorNumber = 0;
        private string strErrorMessage = "";

        public int IdNotificacion
        {
            get { return this.intIdNotificacion; }
            set { this.intIdNotificacion = value; }
        }
        public int IdDepartamento
        {
            get { return this.intIdDepartamento; }
            set { this.intIdDepartamento = value; }
        }
        public int IdRol
        {
            get { return this.intIdRol; }
            set { this.intIdRol = value; }
        }
        public int IdUsuario
        {
            get { return this.intIdUsuario; }
            set { this.intIdUsuario = value; }
        }
        public bool EsParaTodos
        {
            get { return this.blnEsParaTodos; }
            set { this.blnEsParaTodos = value; }
        }
        public string Mensaje
        {
            get { return this.strMensaje; }
            set { this.strMensaje = value; }
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

        public int ObtenerNumeroDeNotifcaciones()
        {
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlDataAdapter daAdapter = new SqlDataAdapter();
            SqlParameter prmGeneric = null;
            DataTable tblTable = null;
            int intNumeroNotificaciones = 0;

            try
            {

                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerNumeroDeNotificaciones", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@idUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuario;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //OUTPUT Parameters
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
                    if (tblTable.Rows.Count > 0)
                    {
                        intNumeroNotificaciones = Convert.ToInt32(tblTable.Rows[0][0].ToString());
                    }
                    else
                    {
                        intNumeroNotificaciones = 0;
                    }
                }
                else
                {
                    intNumeroNotificaciones = 0;
                }

            }
            catch (SqlException exError)
            {
                strErrorMessage = exError.Message;
                intErrorNumber = exError.ErrorCode;
                intNumeroNotificaciones = 0;
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
            return intNumeroNotificaciones;
        }

        public DataTable ObtenerNotificaciones()
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

                cmdQuery = new SqlCommand("spr_ObtenerListaDeNotifcaciones", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@idUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuario;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.EsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //OUTPUT Parameters
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
                strErrorMessage = exError.Message;
                intErrorNumber = exError.ErrorCode;
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

        public bool AltaNotificacionLeida()
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

                cmdQuery = new SqlCommand("spr_AltaDeNotificacionLeida", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new SqlParameter("@idNotificacion", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdNotificacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new SqlParameter("@idUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuario;
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

                intErrorNumber = Convert.ToInt32(cmdQuery.Parameters["@NumeroDeError"].Value);
                strErrorMessage = Convert.ToString(cmdQuery.Parameters["@MensajeDeError"].Value);

                if (intErrorNumber == 0)
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
                strErrorMessage = exError.Message;
                intErrorNumber = exError.ErrorCode;
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

        public bool Mantenimiento(int intMovimiento)
        {
            //intMovement Description:
            //1: Insert
            //2: Update
            //3: Delete
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

                cmdQuery = new SqlCommand("spr_MantenimientoNotificaciones", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idNotificacion", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdNotificacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdDepartamento", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdDepartamento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdRol", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdRol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdUsuario;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Mensaje", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Mensaje;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsParaTodos", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsParaTodos;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new System.Data.SqlClient.SqlParameter("@MensajeDeError", System.Data.SqlDbType.NVarChar, 255);
                prmGeneric.Value = "";
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NumeroDeError", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

                daAdapter = new SqlDataAdapter(cmdQuery);
                tblTable = new DataTable();
                daAdapter.Fill(tblTable);

                this.strErrorMessage = daAdapter.SelectCommand.Parameters["@MensajeDeError"].Value.ToString();
                this.intErrorNumber = Convert.ToInt32(daAdapter.SelectCommand.Parameters["@NumeroDeError"].Value.ToString());

                if (intErrorNumber == 0)
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
                blnResult = false;
                this.strErrorMessage = exError.Message;
                this.intErrorNumber = exError.Number;
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