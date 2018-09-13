using System;
using System.Web;
using System.Xml;


/// <summary>
/// Descripción breve de clsXmlReader
/// </summary>
public static class lablesXML
{

    public static string getNameSpanish(string tagName)
    {
        if (tagName == "")
        {
            return tagName;
        }
        string returnText;
        XmlTextReader xtr = new XmlTextReader(HttpContext.Current.Server.MapPath("~/Common/Language/spanish.xml"));
        xtr.WhitespaceHandling = WhitespaceHandling.None;
        XmlDocument xd = new XmlDocument();
        xd.Load(xtr);
        xtr.Close();
        try
        {
            returnText = xd.DocumentElement.SelectSingleNode(string.Format("/Resource/item[./@name='{0}']", tagName)).InnerText.ToString();
        }
        catch (Exception)
        {
            return tagName;
        }
        return returnText == "" ? tagName : returnText;
    }

    public static string getNameSpanish(string tagName, string lenguage)
    {
        if (tagName == "")
        {
            return tagName;
        }
        if (lenguage == "EN")
        {
            return tagName;
        }
        string returnText;
        XmlTextReader xtr = new XmlTextReader(HttpContext.Current.Server.MapPath("~/common2/Language/spanish.xml"));
        //XmlTextReader xtr = new XmlTextReader(HttpContext.Current.Server.MapPath("~/Common/Language/spanish.xml"));
        xtr.WhitespaceHandling = WhitespaceHandling.None;
        XmlDocument xd = new XmlDocument();
        xd.Load(xtr);
        xtr.Close();
        try
        {
            returnText = xd.DocumentElement.SelectSingleNode(string.Format("/Resource/item[./@name='{0}']", tagName)).InnerText.ToString();
        }
        catch (Exception)
        {
            return tagName;
        }
        return returnText == "" ? tagName : returnText;
    }
}
