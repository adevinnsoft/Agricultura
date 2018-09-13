using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

    public class Growing
    {

        private int intidMastGrowing = 0;
        private int intidPlanta = 0;
        private int intidInvernadero = 0;
        private int intidEstatusGrowing = 0;
        private int intidGrower = 0;
        private int intidGerente = 0;
        private int intidLider = 0;
        private int intIdUsuario = 0;
        private int intAnio = 0;
        private int intNoSemana = 0;
        private string strSemana = "";
        private bool blnEsPlantacion = true;

        private string strGrower = "";
        private string strGerente = "";
        private string strLider = "";

        private int intidGrowingPuntajePorGrupo = 0;
        private int intidGrupoGrowing = 0;
        private decimal decPuntajeObtenido = 0;
        private string strGrupo = "";

        private int intIdParametroPorGrupoGrowing = 0;

        private int intIdDetGrowing = 0;
        private bool blnNA = false;
        private bool blnOK = false;
        private bool blnX = false;
        private bool blnS = false;
        private bool blnA = false;
        private bool blnG = false;
        private bool blnN = false;

        private int intIdDetNA_OK_X_Growing = 0;
        private int intIdCatalogoListaNA_OK_X_PorParametro = 0;

        private int intIdDetS_A_G_N_Growing = 0;
        private int intIdCatalogoListaS_A_G_N_PorParametro = 0;

        private bool blnEsEnEspanol = true;
        private int intErrorNumber = 0;
        private string strErrorMessage = "";

                   
        public int idMastGrowing
        {
            get { return this.intidMastGrowing; }
            set { this.intidMastGrowing = value; }
        }   
        public int idPlanta
        {
            get { return this.intidPlanta; }
            set { this.intidPlanta = value; }
        }
        public int idInvernadero
        {
            get { return this.intidInvernadero; }
            set { this.intidInvernadero = value; }
        }
        public int IdDetGrowing
        {
            get { return this.intIdDetGrowing; }
            set { this.intIdDetGrowing = value; }
        }
        public int idEstatusGrowing
        {
            get { return this.intidEstatusGrowing; }
            set { this.intidEstatusGrowing = value; }
        }

        public int IdParametroPorGrupoGrowing
        {
            get { return this.intIdParametroPorGrupoGrowing; }
            set { this.intIdParametroPorGrupoGrowing = value; }
        }
        public int idGrower
        {
            get { return this.intidGrower; }
            set { this.intidGrower = value; }
        }
        public int idGerente
        {
            get { return this.intidGerente; }
            set { this.intidGerente = value; }
        }

        public int idLider
        {
            get { return this.intidLider; }
            set { this.intidLider = value; }
        }

        public int idGrowingPuntajePorGrupo
        {
            get { return this.intidGrowingPuntajePorGrupo; }
            set { this.intidGrowingPuntajePorGrupo = value; }
        }

        public int idGrupoGrowing
        {
            get { return this.intidGrupoGrowing; }
            set { this.intidGrupoGrowing = value; }
        }

        public decimal PuntajeObtenido
        {
            get { return this.decPuntajeObtenido; }
            set { this.decPuntajeObtenido = value; }
        }

   

        public int IdUsuario
        {
            get { return this.intIdUsuario; }
            set { this.intIdUsuario = value; }
        }
        public int Anio
        {
            get { return this.intAnio; }
            set { this.intAnio = value; }
        }
        public int NoSemana
        {
            get { return this.intNoSemana; }
            set { this.intNoSemana = value; }
        }
        public string Semana
        {
            get { return this.strSemana; }
            set { this.strSemana = value; }
        }

        public string Grower
        {
            get { return this.strGrower; }
            set { this.strGrower = value; }
        }

        public string Gerente
        {
            get { return this.strGerente; }
            set { this.strGerente = value; }
        }


        public string Lider
        {
            get { return this.strLider; }
            set { this.strLider = value; }
        }

        public string Grupo
        {
            get { return this.strGrupo; }
            set { this.strGrupo = value; }
        }

        public bool EsPlantacion
        {
            get { return this.blnEsPlantacion; }
            set { this.blnEsPlantacion = value; }
        }

        public bool NA
        {
            get { return this.blnNA; }
            set { this.blnNA = value; }
        }
        public bool OK
        {
            get { return this.blnOK; }
            set { this.blnOK = value; }
        }
        public bool X
        {
            get { return this.blnX; }
            set { this.blnX = value; }
        }
        public bool S
        {
            get { return this.blnS; }
            set { this.blnS = value; }
        }
        public bool A
        {
            get { return this.blnA; }
            set { this.blnA = value; }
        }
        public bool G
        {
            get { return this.blnG; }
            set { this.blnG = value; }
        }
        public bool N
        {
            get { return this.blnN; }
            set { this.blnN = value; }
        }


        public int IdDetNA_OK_X_Growing
        {
            get { return this.intIdDetNA_OK_X_Growing; }
            set { this.intIdDetNA_OK_X_Growing = value; }
        }
        public int IdCatalogoListaNA_OK_X_PorParametro
        {
            get { return this.intIdCatalogoListaNA_OK_X_PorParametro; }
            set { this.intIdCatalogoListaNA_OK_X_PorParametro = value; }
        }

        public int IdDetS_A_G_N_Growing
        {
            get { return this.intIdDetS_A_G_N_Growing; }
            set { this.intIdDetS_A_G_N_Growing = value; }
        }
        public int IdCatalogoListaS_A_G_N_PorParametro
        {
            get { return this.intIdCatalogoListaS_A_G_N_PorParametro; }
            set { this.intIdCatalogoListaS_A_G_N_PorParametro = value; }
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


        public DataTable ObtenerCatalogo()
        {
            DataTable functionReturnValue = null;
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerGrupoGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                //prmGeneric = new System.Data.SqlClient.SqlParameter("@idGrupoGrowing", System.Data.SqlDbType.Int);
                //prmGeneric.Value = IdGrupoGrowing;
                //prmGeneric.Direction = ParameterDirection.Input;
                //cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = EsEnEspanol;
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
                    functionReturnValue = tblTable;
                }
                else
                {
                    functionReturnValue = null;
                }

            }
            catch (SqlException exError)
            {
                functionReturnValue = null;
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
                if (cnnMain != null)
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return functionReturnValue;
        }


        public bool MantenimientoGrowing(int intMovimiento)
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

                cmdQuery = new SqlCommand("spr_MantenimientoGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idMastGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idMastGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idPlanta", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idPlanta;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idInvernadero", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idInvernadero;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idEstatusGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idEstatusGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //prmGeneric = new System.Data.SqlClient.SqlParameter("@idGrower", System.Data.SqlDbType.Int);
                //prmGeneric.Value = this.idMastGrowing;
                //prmGeneric.Direction = ParameterDirection.Input;
                //cmdQuery.Parameters.Add(prmGeneric);

                //prmGeneric = new System.Data.SqlClient.SqlParameter("@idGerente", System.Data.SqlDbType.Int);
                //prmGeneric.Value = this.idGerente;
                //prmGeneric.Direction = ParameterDirection.Input;
                //cmdQuery.Parameters.Add(prmGeneric);

                //prmGeneric = new System.Data.SqlClient.SqlParameter("@idLider", System.Data.SqlDbType.Int);
                //prmGeneric.Value = this.idLider;
                //prmGeneric.Direction = ParameterDirection.Input;
                //cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Grower", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Grower;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Gerente", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Gerente;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Lider", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Lider;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);


                prmGeneric = new System.Data.SqlClient.SqlParameter("@IdUsuario", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.intIdUsuario;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Anio", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.Anio;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NoSemana", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.NoSemana;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Semana", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Semana;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsPlantacion", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsPlantacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);
                                
                //output parameters
                prmGeneric = new System.Data.SqlClient.SqlParameter("@idMastGrowingOUT", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

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

                if (intMovimiento == 1)
                {
                    this.idMastGrowing =Convert.ToInt32(daAdapter.SelectCommand.Parameters["@idMastGrowingOUT"].Value.ToString());
                }

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



        public bool MantenimientoPuntajePorGrupo(int intMovimiento)
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

                cmdQuery = new SqlCommand("spr_MantenimientoPuntajePorGrupoGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idGrowingPuntajePorGrupo", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idGrowingPuntajePorGrupo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idMastGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idMastGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Grupo", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Grupo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@PuntajeObtenido", System.Data.SqlDbType.Float);
                prmGeneric.Value = this.PuntajeObtenido;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@ValidoParaPlantacion", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsPlantacion;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new System.Data.SqlClient.SqlParameter("@idGrowingPuntajePorGrupoOUT", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

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

                if (intMovimiento == 1)
                {
                    this.idGrowingPuntajePorGrupo = Convert.ToInt32(daAdapter.SelectCommand.Parameters["@idGrowingPuntajePorGrupoOUT"].Value.ToString());
                }

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


        public bool MantenimientoDetGrowing(int intMovimiento)
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

                cmdQuery = new SqlCommand("spr_MantenimientoDetGrowing", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idDetGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.intIdDetGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idGrowingPuntajePorGrupo", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.idGrowingPuntajePorGrupo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idParametroPorGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdParametroPorGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Grupo", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = this.Grupo;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@PuntajeObtenido", System.Data.SqlDbType.Float);
                prmGeneric.Value = this.PuntajeObtenido;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@NA", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.NA;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@OK", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.OK;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@X", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.X;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@S", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.S;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@A", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.A;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@G", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.G;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@N", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.N;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                //output parameters
                prmGeneric = new System.Data.SqlClient.SqlParameter("@idDetGrowingOUT", System.Data.SqlDbType.Int);
                prmGeneric.Value = 0;
                prmGeneric.Direction = ParameterDirection.Output;
                cmdQuery.Parameters.Add(prmGeneric);

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

                if (intMovimiento == 1)
                {
                    this.IdDetGrowing = Convert.ToInt32(daAdapter.SelectCommand.Parameters["@idDetGrowingOUT"].Value.ToString());
                }

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

        public bool MantenimientoDetNA_OK_X(int intMovimiento)
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

                cmdQuery = new SqlCommand("spr_MantenimientoDet_NA_OK_X", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idDetNA_OK_X_Growing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdDetNA_OK_X_Growing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idDetGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdDetGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idCatalogoListaNA_OK_X_PorParametro", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdCatalogoListaNA_OK_X_PorParametro;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);
                              
                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
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

        public bool MantenimientoDetS_A_G_N(int intMovimiento)
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

                cmdQuery = new SqlCommand("spr_MantenimientoDet_S_A_G_N", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Movimiento", System.Data.SqlDbType.Int);
                prmGeneric.Value = intMovimiento;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idDetS_A_G_N_Growing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdDetNA_OK_X_Growing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idDetGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdDetGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idCatalogoListaS_A_G_N_PorParametro", System.Data.SqlDbType.Int);
                prmGeneric.Value = this.IdCatalogoListaNA_OK_X_PorParametro;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);
                              
                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = this.blnEsEnEspanol;
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

        public DataTable ObtenerNAOK_SAGN()
        {
            DataTable functionReturnValue = null;
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerNAOK_SAGN", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idParametroPorGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = IdParametroPorGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idInvernadero", System.Data.SqlDbType.Int);
                prmGeneric.Value = idInvernadero;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Semana", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = Semana;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = EsEnEspanol;
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
                    functionReturnValue = tblTable;
                }
                else
                {
                    functionReturnValue = null;
                }

            }
            catch (SqlException exError)
            {
                functionReturnValue = null;
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
                if (cnnMain != null)
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return functionReturnValue;
        }

        public DataTable ObtenerListaNAOK()
        {
            DataTable functionReturnValue = null;
            SqlConnection cnnMain = new SqlConnection();
            SqlCommand cmdQuery = null;
            SqlParameter prmGeneric = null;
            SqlDataAdapter daAdapter = default(SqlDataAdapter);
            DataTable tblTable = null;

            try
            {
                cnnMain.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["dbConn"].ToString();
                cnnMain.Open();

                cmdQuery = new SqlCommand("spr_ObtenerListaNAOK", cnnMain);
                cmdQuery.CommandType = CommandType.StoredProcedure;
                cmdQuery.Parameters.Clear();

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idParametroPorGrupoGrowing", System.Data.SqlDbType.Int);
                prmGeneric.Value = IdParametroPorGrupoGrowing;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@idInvernadero", System.Data.SqlDbType.Int);
                prmGeneric.Value = idInvernadero;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@Semana", System.Data.SqlDbType.NVarChar);
                prmGeneric.Value = Semana;
                prmGeneric.Direction = ParameterDirection.Input;
                cmdQuery.Parameters.Add(prmGeneric);

                prmGeneric = new System.Data.SqlClient.SqlParameter("@EsEnEspanol", System.Data.SqlDbType.Bit);
                prmGeneric.Value = EsEnEspanol;
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
                    functionReturnValue = tblTable;
                }
                else
                {
                    functionReturnValue = null;
                }

            }
            catch (SqlException exError)
            {
                functionReturnValue = null;
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
                if (cnnMain != null)
                {
                    cnnMain.Dispose();
                    cnnMain = null;
                }
            }
            return functionReturnValue;
        }


    }
