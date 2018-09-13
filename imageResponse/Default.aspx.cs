using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

public partial class imageResponse_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            string idImage = Request.QueryString["ID"].ToString();
            string ruta = Request.QueryString["ruta"].ToString();
            Byte[] bytes = File.ReadAllBytes(ConfigurationManager.AppSettings[ruta].ToString() + idImage); ;
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "image/" + idImage.Split('.')[1];
            Response.AddHeader("content-disposition", "attachment;filename=" + idImage);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
        catch (Exception x)
        { 
            
        }
    }
}