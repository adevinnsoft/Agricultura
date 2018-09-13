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
using System.Text.RegularExpressions;
/// <summary>
/// Descripción breve de EnviarCorreo
/// </summary>
public class EnviarCorreo
{
	public EnviarCorreo()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public string Enviar(string RecipientesSeparadosPorPuntoYComa, string Asunto, string Mensaje)
    {
        return string.Empty;
    }

    public string Enviar(string RecipientesSeparadosPorPuntoYComa, string Asunto, string Mensaje, string PerfilDeCorreo)
    {
        return string.Empty;
    }

    public string Enviar(int idListaDistribucion, string Asunto, string Mensaje)
    {
        return string.Empty;
    }

    public string Enviar(int idListaDistribucion, string Asunto, string Mensaje, string PerfilDeCorreo)
    {
        return string.Empty;
    }

    public string Enviar(List<String> To,string Subject,string Message)
    {
        //int IdCorreo = 0;
        try
        {
            DataAccess da = new DataAccess();
            Dictionary<string, object> prm = new Dictionary<string, object>();
            DataTable dtCorreos = new DataTable();

            //dtCorreos.Columns.Add("IdCorreo");
            dtCorreos.Columns.Add("Correo");
            DataRow dr= null;
            foreach (String email in To)
            {
                dr = dtCorreos.NewRow();
               // dr["IdCorreo"] = ++IdCorreo;
                dr["Correo"] = email;
                dtCorreos.Rows.Add(dr);
            }

            prm.Add("@To",dtCorreos);
            prm.Add("@Subject",Subject);
            prm.Add("@Message", Message);

            DataTable  dtResult = da.executeStoreProcedureDataTable("spr_EnvioListaDeDistribucionCorreos", prm);

            if (dtResult.Rows.Count > 0)
            {
                if (dtResult.Rows[0]["Estado"].ToString().Equals("1"))
                {
                    return "1 OK";
                }
                else
                {
                    return "0 ERROR";
                }
            }
            else
            {
                return "0 NO EXISTEN DATOS";
            }
        }
        catch (Exception ex)
        {
            return  "0" + ex.ToString();
        }

    }

    public bool validateMail(string email)
    {
        string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

        if (Regex.IsMatch(email, expression))
        {
            if (Regex.Replace(email, expression, string.Empty).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
}