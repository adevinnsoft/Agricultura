using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;

/// <summary>
/// Autor: F Cazares
/// </summary>
public class Json
{
	public Json()
	{
	}

    /// <summary>
    /// Convierte un string XML en una cadena Json
    /// 
    /// </summary>
    /// <param name="xmlString"></param>
    /// <returns></returns>
    public static string XmlToJson(string xmlString)
    {
        return new JavaScriptSerializer().Serialize(GetXmlValues(XElement.Parse(xmlString)));
    }

    public static Dictionary<string, object> GetXmlValues(XElement xml) {
        Dictionary<string, object> attr = xml.Attributes().ToDictionary(d => d.Name.LocalName, d => (object)d.Value);
        if (xml.HasElements)
            foreach(XNode node in xml.Nodes()) {
                attr.Add(((XElement)node).Name.ToString(), ((XElement)node).Elements().Select(e => GetXmlValues(e)));
            }
            
            
        else if (!xml.IsEmpty)
            attr.Add("_value", xml.Value);

        return new Dictionary<string, object> { { xml.Name.LocalName, attr } };
    }


}