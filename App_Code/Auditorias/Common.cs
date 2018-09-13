using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Hosting;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

/// <summary>
/// Descripción breve de Common
/// </summary>
///

public static class CommonReportAudit
{

    public static string paintHtmlTable(int xlength, int ylength)
    {
        if (xlength < 1 && ylength < 1)
        {
            return string.Empty;
        }

        StringBuilder sbTable = new StringBuilder();
        string tblOpen = "<table class=\"tableWareHouseClass\">";
        string tblClose = "</table>";
        string trClose = "</tr>";
        string trOpen = "<tr>";
        string tdClose = "</td>";
        string tdOpen = "<td>";
        string spanOpen = "<span>";
        string spanClose = "</span>";

        sbTable.Append(tblOpen);
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);

        for (int i = 0; i < xlength; i++)
        {
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, Convert.ToChar(Convert.ToInt32(('A' + i).ToString())), spanClose, tdClose);
        }

        sbTable.Append(trClose);

        for (int y = 0; y < ylength; y++)
        {
            sbTable.Append(trOpen);
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, (y + 1).ToString(), spanClose, tdClose);
            for (int x = 0; x < xlength; x++)
            {
                sbTable.AppendFormat("{0} class=\"tdBorder\" >{1}", tdOpen.Substring(0, tdOpen.Length - 1), tdClose);
            }
            sbTable.Append(trClose);
        }

        sbTable.Append(tblClose);

        return sbTable.ToString();
    }

    public static string paintHtmlTable(int IDWareHouse, int xlength, int ylength, DataTable dtInformation, string WareHouseName)
    {
        return paintHtmlTable(IDWareHouse, xlength, ylength, dtInformation, string.Empty, WareHouseName, string.Empty);
    }

    public static string paintHtmlTable(int IDWareHouse, int xlength, int ylength, DataTable dtInformation)
    {
        return paintHtmlTable(IDWareHouse, xlength, ylength, dtInformation, string.Empty, string.Empty, string.Empty);
    }

    public static string paintHtmlTable(int IDWareHouse, int xlength, int ylength, DataTable dtInformation, string sLiClass, string WareHouseName, string sDivClass)
    {

        if (xlength < 1 && ylength < 1)
        {
            return string.Empty;
        }

        if (dtInformation == null || dtInformation.Rows.Count < 1)
        {
            return paintHtmlTableNoTable(IDWareHouse, xlength, ylength, WareHouseName, sDivClass);
        }

        StringBuilder sbTable = new StringBuilder();
        string tblOpen = "<table class=\"tableWareHouseClass\">";
        string tblClose = "</table>";
        string trClose = "</tr>";
        string trOpen = "<tr>";
        string tdClose = "</td>";
        string tdOpen = "<td>";
        string spanOpen = "<span>";
        string spanClose = "</span>";

        int iCount = 0;

        sbTable.Append(tblOpen);
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);

        for (int i = 0; i < xlength; i++)
        {
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, Convert.ToChar(Convert.ToInt32(('A' + i).ToString())), spanClose, tdClose);
        }

        sbTable.Append(trClose);

        DataRow[] drResult = null;

        for (int y = 1; y <= ylength; y++)
        {
            //Open new Row ( TR )
            sbTable.Append(trOpen);
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, (y).ToString(), spanClose, tdClose);
            for (int x = 1; x <= xlength; x++)
            {
                sbTable.AppendFormat("{0} class=\"tdBorderBig\" >", tdOpen.Substring(0, tdOpen.Length - 1));

                //Si ya no hay filas no buscar y asignar nulo
                drResult = iCount < dtInformation.Rows.Count ? dtInformation.Select(string.Format("XPosition = {0} AND YPosition = {1}", x, y)) : null;

                //Paint Div Container Dropable
                sbTable.AppendFormat("<div id=\"IDDrop-{0}-{1}-{2}\" class=\"classGray {3}\">", IDWareHouse, x, y, sDivClass);
                if (drResult != null && drResult.Length > 0)
                {
                    //Paint li Dragable
                    sbTable.Append("<ul class='ui-helper-reset'>");

                    foreach (DataRow item in drResult)
                    {
                        //sbTable.Append(liPreformated(item["Folio"].ToString(), item["TimeIn"].ToString(), item["SKU"].ToString()
                        //        , item["GreenHouse"].ToString(), item["internalID"].ToString(), item["internalQty"].ToString(), sLiClass, item["internalProductClass"].ToString()));
                        sbTable.Append(liPreformated(item, sLiClass));

                        iCount++;
                    }
                    sbTable.Append("</ul>");
                }
                //Close Div Container Droppable
                sbTable.Append("</div>");
                sbTable.Append(tdClose);
            }
            sbTable.Append(trClose);
        }
        //Paint The Name, Set it down the table
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);
        sbTable.AppendFormat("<td class=\"tdClassNameWareHouse\" colspan=\"{0}\">{1}{2}", xlength, WareHouseName, tdClose);
        sbTable.Append(trClose);

        sbTable.Append(tblClose);

        return sbTable.ToString();
    }

    public static string paintHtmlTable(int IDWareHouse, int xlength, int ylength, DataTable dtInformation, string sLiClass, string WareHouseName, string sDivClass, DataTable dtSemaforo, DataTable dtExtraHours, string identificador)
    {

        if (xlength < 1 && ylength < 1)
        {
            return string.Empty;
        }

        if (dtInformation == null || dtInformation.Rows.Count < 1)
        {
            return paintHtmlTableNoTable(IDWareHouse, xlength, ylength, WareHouseName, sDivClass);
        }

        StringBuilder sbTable = new StringBuilder();
        string tblOpen = "<table class=\"tableWareHouseClass\">";
        string tblClose = "</table>";
        string trClose = "</tr>";
        string trOpen = "<tr>";
        string tdClose = "</td>";
        string tdOpen = "<td>";
        string spanOpen = "<span>";
        string spanClose = "</span>";

        int iCount = 0;

        sbTable.Append(tblOpen);
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);

        for (int i = 0; i < xlength; i++)
        {
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, Convert.ToChar(Convert.ToInt32(('A' + i).ToString())), spanClose, tdClose);
        }

        sbTable.Append(trClose);

        DataRow[] drResult = null;

        for (int y = 1; y <= ylength; y++)
        {
            //Open new Row ( TR )
            sbTable.Append(trOpen);
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, (y).ToString(), spanClose, tdClose);
            for (int x = 1; x <= xlength; x++)
            {
                sbTable.AppendFormat("{0} class=\"tdBorderBig\" >", tdOpen.Substring(0, tdOpen.Length - 1));

                //Si ya no hay filas no buscar y asignar nulo
                drResult = iCount < dtInformation.Rows.Count ? dtInformation.Select(string.Format("XPosition = {0} AND YPosition = {1}", x, y)) : null;

                //Paint Div Container Dropable
                sbTable.AppendFormat("<div id=\"IDDrop-{0}-{1}-{2}\" class=\"classGray {3}\">", IDWareHouse, x, y, sDivClass);
                if (drResult != null && drResult.Length > 0)
                {
                    //Paint li Dragable
                    sbTable.Append("<ul class='ui-helper-reset'>");

                    foreach (DataRow item in drResult)
                    {
                        //sbTable.Append(liPreformated(item["Folio"].ToString(), item["TimeIn"].ToString(), item["SKU"].ToString()
                        //        , item["GreenHouse"].ToString(), item["internalID"].ToString(), item["internalQty"].ToString(), sLiClass, item["internalProductClass"].ToString()));
                        /*Obtener TimeIn para compararlo con semáforo*/
                        //TODO ALEX Poner Maximo tiempo
                        sbTable.Append(liPreformated(item, sLiClass, CommonReportAudit.getHexColorFromSemaphore(dtSemaforo, dtExtraHours, item["GreenHouse"].ToString(), item["Folio"].ToString(), (int)item["TimeIn"]), CommonReportAudit.getMaxTimeToRed(dtSemaforo, dtExtraHours, item["GreenHouse"].ToString(), item["Folio"].ToString()), identificador));
                        //sbTable.Append(liPreformated(item, sLiClass));
                        iCount++;
                    }
                    sbTable.Append("</ul>");
                }
                //Close Div Container Droppable
                sbTable.Append("</div>");
                sbTable.Append(tdClose);
            }
            sbTable.Append(trClose);
        }
        //Paint The Name, Set it down the table
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);
        sbTable.AppendFormat("<td class=\"tdClassNameWareHouse\" colspan=\"{0}\">{1}{2}", xlength, WareHouseName, tdClose);
        sbTable.Append(trClose);

        sbTable.Append(tblClose);

        return sbTable.ToString();
    }

    private static string paintHtmlTableNoTable(int IDWareHouse, int xlength, int ylength, string WareHouseName, string sDivClass)
    {

        StringBuilder sbTable = new StringBuilder();
        string tblOpen = "<table class=\"tableWareHouseClass\">";
        string tblClose = "</table>";
        string trClose = "</tr>";
        string trOpen = "<tr>";
        string tdClose = "</td>";
        string tdOpen = "<td>";
        string spanOpen = "<span>";
        string spanClose = "</span>";

        sbTable.Append(tblOpen);
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);

        for (int i = 0; i < xlength; i++)
        {
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, Convert.ToChar(Convert.ToInt32(('A' + i).ToString())), spanClose, tdClose);
        }

        sbTable.Append(trClose);

        for (int y = 1; y <= ylength; y++)
        {
            sbTable.Append(trOpen);
            sbTable.AppendFormat("{0}{1}{2}{3}{4}", tdOpen, spanOpen, (y).ToString(), spanClose, tdClose);
            for (int x = 1; x <= xlength; x++)
            {
                sbTable.AppendFormat("{0} class=\"tdBorderBig\" >", tdOpen.Substring(0, tdOpen.Length - 1));
                sbTable.AppendFormat("<div id=\"IDDrop-{0}-{1}-{2}\" class=\"classGray {3}\">", IDWareHouse, x, y, sDivClass);
                sbTable.Append(tdClose);

            }
            sbTable.Append(trClose);
        }

        //Paint The Name, Set it down the table
        sbTable.Append(trOpen);
        sbTable.Append(tdOpen);
        sbTable.Append(tdClose);
        sbTable.AppendFormat("<td class=\"tdClassNameWareHouse\" colspan=\"{0}\">{1}{2}", xlength, WareHouseName, tdClose);
        sbTable.Append(trClose);

        sbTable.Append(tblClose);

        return sbTable.ToString();
    }

    private static string liPreformated(string folio, string time, string SKU)
    {
        StringBuilder sbLiPre = new StringBuilder();

        sbLiPre.Append("<li><table class=\"tblInternalFa\"><tr><td colspan=\"2\" style=\"vertical-align:top;\">");
        sbLiPre.AppendFormat("<span>{0}</span></td></tr><tr><td style=\"text-align:left;\">", folio);
        sbLiPre.AppendFormat("<span>{0}</span></td><td style=\"text-align:right;\">", time);
        sbLiPre.AppendFormat("<span>{0}</span></td></tr></table></li>", "7700");

        return sbLiPre.ToString();
    }

    //******************************************************
    //**********************************************************
    public static string liPreformatedTest(string folio, string time, string SKU, string GreenHouse, string Boxes, string internalID, string internalQty, string internalProductClass, string colorHx, string maxTime)
    {
        StringBuilder sbLiPre = new StringBuilder();
        sbLiPre.AppendFormat("<li id=\"{0}\" style=\"background-color: {1}\" >", folio, colorHx);
        //sbLiPre.AppendFormat("<li id=\"{0}\">", folio);

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", internalProductClass);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", internalID);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", internalQty);

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", folio);

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0}-{1} hrs</span></td>", time, maxTime);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", internalProductClass), internalProductClass == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", internalQty, internalQty == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", SKU);
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}</span>/", GreenHouse);
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></tr></table></li>", Boxes);
        return sbLiPre.ToString();
    }
    //****************PRECOOLER
    public static string liPreformatedTest(string folio, string time, string SKU, string GreenHouse, string Boxes, string internalID, string internalQty, string internalProductClass, string colorHx, string maxTime, string sclass)
    {
        StringBuilder sbLiPre = new StringBuilder();
        sbLiPre.AppendFormat("<li id=\"{0}\" style=\"background-color: {1}\" class=\"{2}\" >", folio, colorHx, sclass);
        //sbLiPre.AppendFormat("<li id=\"{0}\">", folio);

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", internalProductClass);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", internalID);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", internalQty);

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", folio);

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0}-{1} hrs</span></td>", time, maxTime);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", internalProductClass), internalProductClass == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", internalQty, internalQty == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", SKU);
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}</span>/", GreenHouse);
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></tr></table></li>", Boxes);
        return sbLiPre.ToString();
    }

    public static string liPreformated(string folio, string time, string SKU, string GreenHouse, string Boxes, string internalID, string internalQty, string internalProductClass)
    {
        StringBuilder sbLiPre = new StringBuilder();
        //sbLiPre.AppendFormat("<li id=\"{0}\" style=\"background-color: #000000\">", folio);
        sbLiPre.AppendFormat("<li id=\"{0}\">", folio);

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", internalProductClass);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", internalID);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", internalQty);

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", folio);

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0} hrs</span></td>", time);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", internalProductClass), internalProductClass == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", internalQty, internalQty == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", SKU);
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}</span>/", GreenHouse);
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></tr></table></li>", Boxes);
        return sbLiPre.ToString();
    }

    public static string liPreformated(string folio, string time, string SKU, string GreenHouse, string Boxes, string internalID, string internalQty, string sClass, string internalProductClass)
    {
        StringBuilder sbLiPre = new StringBuilder();

        sbLiPre.AppendFormat("<li id=\"{0}\" {1}>", folio, string.IsNullOrEmpty(sClass) ? string.Empty : string.Format("class=\"{0}\"", sClass));

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", internalProductClass);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", internalID);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", internalQty);

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", folio);

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0} hrs</span></td>", time);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", internalProductClass), internalProductClass == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", internalQty, internalQty == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", SKU);
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}</span>", GreenHouse);
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></tr></table></li>", Boxes);


        return sbLiPre.ToString();
    }

    public static string liPreformated(string folio, string time, string SKU, string GreenHouse, string Boxes, string internalID, string internalQty, string sClass, string internalProductClass, string TimeMoved, string colorHx, string maxTime)
    {
        StringBuilder sbLiPre = new StringBuilder();

        sbLiPre.AppendFormat("<li id=\"{0}\" {1} style=\"background-color: {2}\">", folio, string.IsNullOrEmpty(sClass) ? string.Empty : string.Format("class=\"{0}\"", sClass), colorHx);

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", internalProductClass);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", internalID);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", internalQty);

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", folio);

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0}-{1} hrs</span></td>", time, maxTime);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", internalProductClass), internalProductClass == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", internalQty, internalQty == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", SKU);
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}</span>/", GreenHouse);
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></tr></table></li>", Boxes);
        sbLiPre.AppendFormat("<label name=\"TimeMoved\" class=\"TimeMoved\">{0}</label></li>", TimeMoved);

        return sbLiPre.ToString();
    }

    //AGREGUE 23/01/2014
    public static string liPreformated(string folio, string time, string SKU, string GreenHouse, string Boxes, string internalID, string internalIDLog, string IdLine, string internalQty, string sClass, string internalProductClass, string TimeMoved, string colorHx, string maxTime)
    {
        StringBuilder sbLiPre = new StringBuilder();

        sbLiPre.AppendFormat("<li id=\"{0}\" {1} style=\"background-color: {2}\">", folio, string.IsNullOrEmpty(sClass) ? string.Empty : string.Format("class=\"{0}\"", sClass), colorHx);

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", internalProductClass);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", internalID);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalIDLog\" name=\"{0}\" value=\"{0}\">", internalIDLog);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalIdLine\" name=\"{0}\" value=\"{0}\">", IdLine);
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", internalQty);

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", folio);

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0}-{1} hrs</span></td>", time, maxTime);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", internalProductClass), internalProductClass == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", internalQty, internalQty == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", SKU);
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}</span>/", GreenHouse);
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></tr></table></li>", Boxes);
        sbLiPre.AppendFormat("<label name=\"TimeMoved\" class=\"TimeMoved\">{0}</label></li>", TimeMoved);

        return sbLiPre.ToString();
    }

    //FIN AGREGUE
    public static string liPreformated(DataRow dr, string sClass)
    {
        StringBuilder sbLiPre = new StringBuilder();

        sbLiPre.AppendFormat("<li id=\"{0}\" {1}>", dr["folio"].ToString(), string.IsNullOrEmpty(sClass) ? string.Empty : string.Format("class=\"{0}\"", sClass));

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", dr["internalProductClass"].ToString());
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", dr["internalID"].ToString());
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", dr["internalQty"].ToString());

        if (dr.Table.Columns.Contains("iStatusQty"))
        {
            sbLiPre.AppendFormat("<input type=\"hidden\" id=\"iStatusQty\" name=\"{0}\" value=\"{0}\">", dr["iStatusQty"].ToString());
        }

        if (dr.Table.Columns.Contains("internalEmbarque"))
        {
            sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalEmbarque\" name=\"{0}\" value=\"{0}\">", dr["internalEmbarque"].ToString());
        }

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", dr["folio"].ToString());

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0} hrs</span></td>", dr["TimeIn"].ToString());
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", dr["internalProductClass"].ToString()), dr["internalProductClass"].ToString() == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", dr["internalQty"].ToString(), dr["internalQty"].ToString() == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", dr["SKU"].ToString());
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}/ </span>", dr["GreenHouse"].ToString());
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></td></tr></table>", dr["Boxes"].ToString());

        if (dr.Table.Columns.Contains("TimeMoved"))
        {
            sbLiPre.AppendFormat("<label name=\"TimeMoved\" class=\"TimeMoved\">{0}</label></li>", dr["TimeMoved"].ToString());
        }
        return sbLiPre.ToString();
    }

    public static string liPreformated(DataRow dr, string sClass, string sColor, string sMaxTime, string almacen)
    {
        StringBuilder sbLiPre = new StringBuilder();

        sbLiPre.AppendFormat("<li id=\"{0}\" {1} style=\"background-color: {2}\">", dr["folio"].ToString(), string.IsNullOrEmpty(sClass) ? string.Empty : string.Format("class=\"{0}\"", sClass), sColor);

        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalProductClass\" name=\"{0}\" value=\"{0}\">", dr["internalProductClass"].ToString());
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalID\" name=\"{0}\" value=\"{0}\">", dr["internalID"].ToString());
        if (almacen == "a")
        {
            sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalIDLOG\" name=\"{0}\" value=\"{0}\">", dr["internalIDLOG"].ToString());
        }
        sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalQty\" name=\"{0}\" value=\"{0}\">", dr["internalQty"].ToString());

        if (dr.Table.Columns.Contains("iStatusQty"))
        {
            sbLiPre.AppendFormat("<input type=\"hidden\" id=\"iStatusQty\" name=\"{0}\" value=\"{0}\">", dr["iStatusQty"].ToString());
        }

        if (dr.Table.Columns.Contains("internalEmbarque"))
        {
            sbLiPre.AppendFormat("<input type=\"hidden\" id=\"internalEmbarque\" name=\"{0}\" value=\"{0}\">", dr["internalEmbarque"].ToString());
        }

        sbLiPre.AppendFormat("<table class=\"tblInternalFa\"><tr><td colspan=\"3\"><span>{0}</span></td></tr>", dr["folio"].ToString());

        sbLiPre.AppendFormat("<tr><td><span id=\"timeHrs\">{0}-{1} hrs</span></td>", dr["TimeIn"].ToString(), sMaxTime);
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/{1}.png\"></td>", "/Common/img", string.Format("productImgID{0}", dr["internalProductClass"].ToString()), dr["internalProductClass"].ToString() == "0" ? "none" : "block");
        sbLiPre.AppendFormat("<td style=\"vertical-align:top;\"><img style =\"display:{2};\" width=\"16\" height=\"16\"  src=\"{0}/QualityImgID{1}.png\"></td></tr>", "/Common/img", dr["internalQty"].ToString(), dr["internalQty"].ToString() == "0" ? "none" : "block");

        sbLiPre.AppendFormat("<tr><td><span id=\"IDSKU\">{0}</span></td>", dr["SKU"].ToString());
        sbLiPre.AppendFormat("<td colspan=\"2\" style=\"text-align:right;\"><span id=\"GreenHouse\">{0}/ </span>", dr["GreenHouse"].ToString());
        sbLiPre.AppendFormat("<span id=\"Boxes\">{0}</span></td></td></tr></table>", dr["Boxes"].ToString());

        if (dr.Table.Columns.Contains("TimeMoved"))
        {
            sbLiPre.AppendFormat("<label name=\"TimeMoved\" class=\"TimeMoved\">{0}</label></li>", dr["TimeMoved"].ToString());
        }
        return sbLiPre.ToString();
    }

    public static HtmlGenericControl paintLine(string IDLine, string sName, string sDivClass, string SKUs)
    {
        Literal ltReturn = new Literal();

        StringBuilder sbLine = new StringBuilder();
        HtmlGenericControl htDiv = new HtmlGenericControl("div");
        HtmlGenericControl divInside = new HtmlGenericControl("div");
        //HtmlGenericControl tr = new HtmlGenericControl("tr");
        HtmlGenericControl span = new HtmlGenericControl("span");
        HtmlGenericControl p = new HtmlGenericControl("p");

        divInside.Attributes["class"] = "calendartitleelement ";
        divInside.Attributes["class"] += sDivClass;
        divInside.Attributes["id"] = string.Format("IDDrop-{0}-0-0", IDLine);
        p.Attributes["class"] = "SubInfo";

        span.InnerText = sName;
        p.InnerText = SKUs;
        divInside.Controls.Add(span);
        divInside.Controls.Add(p);
        htDiv.Controls.Add(divInside);

        return divInside;
    }

    public static HtmlGenericControl paintLine(string IDLine, string IDLineEmpaque, string sName, string sDivClass, string SKUs)
    {
        Literal ltReturn = new Literal();

        StringBuilder sbLine = new StringBuilder();
        HtmlGenericControl htDiv = new HtmlGenericControl("div");
        HtmlGenericControl divInside = new HtmlGenericControl("div");
        //HtmlGenericControl tr = new HtmlGenericControl("tr");
        HtmlGenericControl span = new HtmlGenericControl("span");
        HtmlGenericControl detail = new HtmlGenericControl("span");
        HtmlGenericControl p = new HtmlGenericControl("p");

        divInside.Attributes["class"] = "calendartitleelement ";
        divInside.Attributes["class"] += sDivClass;
        divInside.Attributes["id"] = string.Format("IDDrop-{0}-{1}-0-0-0", IDLine, IDLineEmpaque);
        p.Attributes["class"] = "SubInfo";


        span.InnerText = sName;
        detail.InnerHtml = "<br><label id=" + IDLine + " class='LineIdGP" + IDLineEmpaque + "'></label> <label id=" + IDLineEmpaque + " class='LineDetail' style='cursor:Pointer; color:DarkBlue'>Ver detalles </label><br>";
        ///////////////////////////////////////////
        //MODIFIQUEEEEEEEEEEEEEEEEEEEE
        char[] delimiterChars = { ',' };

        string text = SKUs;
        string[] words = text.Split(delimiterChars);

        string SKUFinal;
        int c, c2;

        SKUFinal = "";
        c = 0;
        c2 = 0;
        foreach (string s in words)
        {
            c++;
            c2++;
            if (c2 == 5)
            {
                c2 = 0;
                SKUFinal = SKUFinal + " ";
            }

            if (c == words.Length)
            {
                SKUFinal = SKUFinal + s;
            }
            else
            {
                SKUFinal = SKUFinal + s + ",";
            }
        }


        p.InnerText = SKUFinal;
        /////////////////////////////////////
        ///////////////////////////////////////

        //p.InnerText = SKUs;


        divInside.Controls.Add(span);
        divInside.Controls.Add(detail);
        divInside.Controls.Add(p);
        htDiv.Controls.Add(divInside);

        return divInside;
    }

    public static HtmlGenericControl paintLine(string IDLine, string IDLineEmpaque, string sName, string sDivClass, DataTable dtInfo, int MaxRows, string SKUs, DataTable dtSemaphore, DataTable dtExtraHours)
    {
        if (dtInfo == null)
        {
            return paintLine(IDLine, IDLineEmpaque, sName, sDivClass, SKUs);
        }

        Literal ltReturn = new Literal();

        HtmlGenericControl divInside = new HtmlGenericControl("div");
        HtmlGenericControl span = new HtmlGenericControl("span");

        divInside.Attributes["class"] = "calendartitleelement ";
        divInside.Attributes["class"] += sDivClass;
        divInside.Attributes["id"] = string.Format("IDDrop-{0}-{1}-0-0-0", IDLine, IDLineEmpaque);

        span.InnerText = sName;
        divInside.Controls.Add(span);

        System.Text.StringBuilder sbList = new StringBuilder();

        sbList.Append(" <br><label id=" + IDLine + " class='LineIdGP" + IDLineEmpaque + "'><label  id=" + IDLineEmpaque + " class='LineDetail' style='cursor:Pointer; color:DarkBlue'>Ver detalles</label><br>");
        sbList.Append("<ul class='ui-helper-reset'>");

        Literal lt = new Literal();
        foreach (DataRow item in dtInfo.Rows)
        {
            sbList.Append(liPreformated(
                        item["Folio"].ToString(),
                        item["TimeIn"].ToString(),
                        item["SKU"].ToString(),
                        item["GreenHouse"].ToString(),
                        item["Boxes"].ToString(),
                        item["internalID"].ToString(),
                        //agregue 23/01/2014
                        item["internalIDLog"].ToString(),
                        //agregue 23/01/2014
                        item["IDLine"].ToString(),
                        item["internalQty"].ToString(),
                        "",
                        item["internalProductClass"].ToString(),
                        item["TimeMoved"].ToString(),
                        getHexColorFromSemaphore(dtSemaphore, dtExtraHours, item["GreenHouse"].ToString(), item["Folio"].ToString(), (int)item["TimeIn"]),
                        getMaxTimeToRed(dtSemaphore, dtExtraHours, item["GreenHouse"].ToString(), item["Folio"].ToString())));
            if (--MaxRows <= 0) break;
        }
        sbList.Append("</ul>");
        lt.Text = sbList.ToString();

        divInside.Controls.Add(lt);

        return divInside;
    }

    public static void FillDropDownList(ref DropDownList ddl, DataTable dt)
    {
        ddl.DataSource = dt;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "Description";
        ddl.DataBind();
        if (ddl.Items.Count > 0)
        {
            ddl.Items.Insert(0, "-- Selecciona uno --");
            ddl.SelectedIndex = 0;
        }
        dt.Dispose();
    }

    public static void FillDropDownListClean(ref DropDownList ddl, DataTable dt)
    {
        ddl.DataSource = dt;
        ddl.DataValueField = "ID";
        ddl.DataTextField = "Description";
        ddl.DataBind();
        //if (ddl.Items.Count > 0)
        //{
        //    ddl.Items.Insert(0, "-- Selecciona uno --");
        //    ddl.SelectedIndex = 0;
        //}
        dt.Dispose();
    }

    public static void FillCheckBoxList(ref CheckBoxList cbl, DataTable dt)
    {
        cbl.DataSource = dt;
        cbl.DataValueField = "ID";
        cbl.DataTextField = "Description";
        cbl.DataBind();
        dt.Dispose();
    }

    public static void FillRadioButtonList(ref RadioButtonList rdl, DataTable dt)
    {
        rdl.DataSource = dt;
        rdl.DataValueField = "ID";
        rdl.DataTextField = "Description";
        rdl.DataBind();
        dt.Dispose();
    }
    /// <summary>
    /// Fill by foreach in dt, including columns: ID, Description, Active
    /// </summary>
    /// <param name="cbl">By ref pass de CheckBoxList wanted</param>
    /// <param name="dtItems">columns: ID, Description, Active</param>
    public static void FillCheckBoxListCycle(ref CheckBoxList cbl, DataTable dtItems)
    {
        int itemCount = 0;
        if (dtItems != null && dtItems.Rows.Count > 0)
        {
            foreach (DataRow item in dtItems.Rows)
            {
                cbl.Items.Add(new ListItem(item["Description"].ToString(), item["ID"].ToString()));
                cbl.Items[itemCount++].Selected = Convert.ToBoolean(item["Active"]);
            }
        }
        dtItems.Dispose();
    }

    //public static string getHexColorFromSemaphore(DataTable dtSemaphore, DataTable dtExtraHours, string greenhouse, string forma, int currentHours)
    //{
    //    /*Buscamos si tiene extension de horas*/
    //    StringBuilder strBuilderFilter = new StringBuilder();
    //    strBuilderFilter.Append("GreenHouse = '");
    //    strBuilderFilter.Append(greenhouse);
    //    strBuilderFilter.Append("' and FormaA is null");
    //    var extraInv = dtExtraHours.Select(strBuilderFilter.ToString());

    //    StringBuilder strBuilderFilterFA = new StringBuilder();
    //    strBuilderFilterFA.Append("FormaA = '");
    //    strBuilderFilterFA.Append(forma);
    //    strBuilderFilterFA.Append("'");
    //    var extraFA = dtExtraHours.Select(strBuilderFilterFA.ToString());
    //    int extraTime = 0;
    //    if (extraFA.Length > 0)
    //    {
    //        extraTime = (int) extraFA[0]["ExtraHours"];
    //    }
    //    else
    //    {
    //        if (extraInv.Length > 0)
    //        {
    //            extraTime = (int) extraInv[0]["ExtraHours"];
    //        }
    //    }

    //    StringBuilder strBuilder = new StringBuilder();
    //    //if (currentHours < 0)
    //    //{
    //    //    currentHours = 1;
    //    //}


    //    strBuilder.Append(currentHours > 0 ? currentHours + 1 : currentHours);
    //    strBuilder.Append("> from and ");
    //    strBuilder.Append(currentHours);
    //    strBuilder.Append("<= to ");
    //    /*DataRow[] rowY, rowR;
    //    rowY = dtSemaphore.Select("ColorName = 'Amarillo'");
    //    rowR = dtSemaphore.Select("ColorName = 'Rojo'");
    //    if (extraTime > 0)
    //    {
    //        if (rowY.Length > 0)
    //            rowY[0]["to"] = (int)rowY[0]["to"] + extraTime;
    //        if (rowR.Length > 0)
    //            rowR[0]["from"] = (int)rowR[0]["from"] + extraTime;                        
    //    }*/

    //    var x = dtSemaphore.Select(strBuilder.ToString());

    //    /*if(extraTime > 0)
    //    {
    //        if (rowY.Length > 0)
    //            rowY[0]["to"] = (int)rowY[0]["to"] - extraTime;
    //        if (rowR.Length > 0)
    //            rowR[0]["from"] = (int)rowR[0]["from"] - extraTime;                        
    //    }*/

    //    return extraTime > 0 ? "#33CCFF" : x[0]["hex"].ToString();
    //    //return "#33CCFF";
    //}

    public static string getHexColorFromSemaphore(DataTable dtSemaphore, DataTable dtExtraHours, string greenhouse, string forma, int currentHours)
    {
        /*Buscamos si tiene extension de horas*/
        StringBuilder strBuilderFilter = new StringBuilder();
        strBuilderFilter.Append("GreenHouse = '");
        strBuilderFilter.Append(greenhouse);
        strBuilderFilter.Append("' and FormaA is null");
        var extraInv = dtExtraHours.Select(strBuilderFilter.ToString());

        StringBuilder strBuilderFilterFA = new StringBuilder();
        strBuilderFilterFA.Append("FormaA = '");
        strBuilderFilterFA.Append(forma);
        strBuilderFilterFA.Append("'");
        var extraFA = dtExtraHours.Select(strBuilderFilterFA.ToString());
        int extraTime = 0;
        if (extraFA.Length > 0)
        {
            extraTime = (int)extraFA[0]["ExtraHours"];
        }
        else
        {
            if (extraInv.Length > 0)
            {
                extraTime = (int)extraInv[0]["ExtraHours"];
            }
        }


        StringBuilder strBuilder = new StringBuilder();
        strBuilder.Append(currentHours <= 0 ? 1 : currentHours);
        strBuilder.Append("> from and ");
        strBuilder.Append(currentHours <= 0 ? 1 : currentHours);
        strBuilder.Append("<= to ");

        var x = dtSemaphore.Select(strBuilder.ToString());

        if (x.Length > 0)
        {
            return extraTime > 0 ? "#3399CC" : x[0]["hex"].ToString();
        }
        else
        {
            return "#3399CC";
        }
    }







    public static string getMaxTimeToRed(DataTable dtSemaphore, DataTable dtExtraHours, string greenhouse, string forma)
    {
        /*Buscamos si tiene extension de horas*/
        StringBuilder strBuilderFilter = new StringBuilder();
        strBuilderFilter.Append("GreenHouse = '");
        strBuilderFilter.Append(greenhouse);
        strBuilderFilter.Append("' and FormaA is null");
        var extraInv = dtExtraHours.Select(strBuilderFilter.ToString());

        StringBuilder strBuilderFilterFA = new StringBuilder();
        strBuilderFilterFA.Append("FormaA = '");
        strBuilderFilterFA.Append(forma);
        strBuilderFilterFA.Append("'");
        var extraFA = dtExtraHours.Select(strBuilderFilterFA.ToString());
        int extraTime = 0;
        if (extraFA.Length > 0)
        {
            extraTime = (int)extraFA[0]["ExtraHours"];
        }
        else
        {
            if (extraInv.Length > 0)
            {
                extraTime = (int)extraInv[0]["ExtraHours"];
            }
        }
        DataRow[] rowY = dtSemaphore.Select("ColorName = 'Amarillo'");

        if (extraTime > 0)
        {
            if (rowY.Length > 0)
                rowY[0]["to"] = (int)rowY[0]["to"] + extraTime;
        }
        var maxTime = rowY[0]["to"].ToString();
        if (extraTime > 0)
        {
            if (rowY.Length > 0)
                rowY[0]["to"] = (int)rowY[0]["to"] - extraTime;
        }

        return maxTime;
    }

    #region ENUM
    public enum STATUS
    {
        Arribo,
        Almacen,
        Precooler,
        Linea
    }

    public enum MESSAGE_TYPE
    {
        Error,
        Info,
        Warning,
        Success
    }
    #endregion

    public static int getStatutsNumber(STATUS status)
    {
        switch (status)
        {
            case STATUS.Arribo:
                return 1;
            case STATUS.Almacen:
                return 2;
            case STATUS.Precooler:
                return 3;
            case STATUS.Linea:
                return 4;
            default:
                return 0;
        }
    }

    public static string FormatEmails(DataTable dt)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        foreach (DataRow item in dt.Rows)
        {
            sb.AppendFormat("{0};", item[0].ToString());
        }
        return sb.ToString();
    }



}
