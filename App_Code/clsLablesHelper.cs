using System;
using System.Data;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;

/// <summary>
/// Descripción breve de clsLablesHelper
/// </summary>
public static class LabelsHelper
{
    public static void setLenguageOnControls(ControlCollection cc, string leng)
    {
        
        foreach (Control c in cc)
        {
            if (c.HasControls())
            {
                findControlsRec(c.Controls, leng);
            }

           
            Type controlType = c.GetType();

            PropertyInfo[] controlPropertiesArray = controlType.GetProperties();

            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if ((controlProperty.Name == "Text" || controlProperty.Name == "HeaderText") && controlProperty.PropertyType == typeof(String))
                {
                    string propertyValue = GetPropertyValue(controlProperty.Name, c);
                    if (propertyValue != "" && !propertyValue.Contains("\r") && !propertyValue.Contains("\n"))
                    {
                        setPropertyValue(controlProperty, controlProperty.PropertyType, c, lablesXML.getNameSpanish(propertyValue, leng));
                    }

                    break;
                }

            }

        }
    }

    private static void findControlsRec(ControlCollection cc, string leng)
    {
        //traemos todos los controles de la página
        foreach (Control c in cc)
        {

            //a cada control se le extrae el tipo
            Type controlType = c.GetType();

            //si el control tiene controles dentro revisar cada uno recursivamente
            if (c.HasControls())
            {
                findControlsRec(c.Controls, leng);
            }
            //si no tiene controles se le sacan sus propiedades
            PropertyInfo[] controlPropertiesArray = controlType.GetProperties();
            foreach (PropertyInfo controlProperty in controlPropertiesArray)
            {
                if ((controlProperty.Name == "Text" || controlProperty.Name == "HeaderText" || controlProperty.Name == "EmptyDataText") && controlProperty.PropertyType == typeof(String))
                {
                    string propertyValue = GetPropertyValue(controlProperty.Name, c);
                    if (propertyValue != "" && !propertyValue.Contains("\r") && !propertyValue.Contains("\n"))
                    {
                        setPropertyValue(controlProperty, controlProperty.PropertyType, c, lablesXML.getNameSpanish(propertyValue, leng));
                    }
                    break;
                }                
            }   
        }
    }

    ///// <summary>
    ///// Loop in controls to change lenguage
    ///// </summary>
    ///// <param name="cc">ControlCollection</param>
    ///// <param name="leng">EN = English</param>
    ///// <param name="Attr">Attribute to change, default "Text"</param>
    //public static void setLenguageOnControls(ControlCollection cc, string leng, string Attr)
    //{
    //    foreach (Control c in cc)
    //    {
    //        if (c.HasControls())
    //        {
    //            findControlsRec(c.Controls, leng, Attr);
    //        }

    //        Type controlType = c.GetType();
    //        PropertyInfo[] controlPropertiesArray =
    //             controlType.GetProperties();

    //        foreach (PropertyInfo controlProperty in controlPropertiesArray)
    //        {
    //            if (controlProperty.Name == Attr &&
    //                    controlProperty.PropertyType == typeof(String))
    //            {
    //                string propertyValue = GetPropertyValue(controlProperty.Name, c);
    //                if (propertyValue != "" && !propertyValue.Contains("\r") && !propertyValue.Contains("\n"))
    //                {
    //                    setPropertyValue(controlProperty, controlProperty.PropertyType, c, lablesXML.getNameSpanish(propertyValue, leng));
    //                }
    //                break;
    //            }
    //        }
    //    }
    //}


    //private static void findControlsRec(ControlCollection cc, string leng, string Attr)
    //{

    //    foreach (Control c in cc)
    //    {
    //        Type controlType = c.GetType();
    //        PropertyInfo[] controlPropertiesArray =
    //             controlType.GetProperties();
    //        if (c.HasControls())
    //        {
    //            findControlsRec(c.Controls, leng, Attr);
    //        }
    //        foreach (PropertyInfo controlProperty in controlPropertiesArray)
    //        {
    //            if (controlProperty.Name == Attr &&
    //                    controlProperty.PropertyType == typeof(String))
    //            {
    //                string propertyValue = GetPropertyValue(controlProperty.Name, c);
    //                if (propertyValue != "" && !propertyValue.Contains("\r") && !propertyValue.Contains("\n"))
    //                {
    //                    setPropertyValue(controlProperty, controlProperty.PropertyType, c, lablesXML.getNameSpanish(propertyValue, leng));
    //                }
    //                break;
    //            }

    //        }
    //    }
    //}

    private static string GetPropertyValue(string pName, Control control)
    {

        Type type = control.GetType();

        string propertyName = pName;

        BindingFlags flags = BindingFlags.GetProperty;

        Binder binder = null;

        object[] args = null;

        object value = type.InvokeMember(
        propertyName,
        flags,
        binder,
        control,
        args
        );

        return value.ToString();
    }

    private static void setPropertyValue(PropertyInfo pi, Type propertyType, object oObj, string value)
    {
        if (propertyType == typeof(string))
        {
            pi.SetValue(oObj, value, null);
            return;
        }
        object[] attributes = propertyType.GetCustomAttributes(typeof(System.ComponentModel.TypeConverterAttribute), false);
        foreach (System.ComponentModel.TypeConverterAttribute converterAttribute in attributes)
        {
            System.ComponentModel.TypeConverter converter = (System.ComponentModel.TypeConverter)Activator.CreateInstance(Type.GetType(converterAttribute.ConverterTypeName));
            if (converter.CanConvertFrom(value.GetType()))
            {
                // good - use the converter to restore the value
                pi.SetValue(oObj, converter.ConvertFromString(value), null);
                break;
            }
        }
    }

    public static DataTable changeToSpanishNames(DataTable dt)
    {
        foreach (DataColumn item in dt.Columns)
        {
            //Nota de intercambio
            item.ColumnName = lablesXML.getNameSpanish(item.ColumnName, "ES");
        }
        dt.AcceptChanges();
        return dt;
    }

    public static DataTable changeToSpanishNames(DataTable dt, string lang)
    {
        if (lang == "ES")
        {
            foreach (DataColumn item in dt.Columns)
            {
                //Nota de intercambio
                item.ColumnName = lablesXML.getNameSpanish(item.ColumnName, "ES");
            }
            dt.AcceptChanges();
        }
        return dt;
    }

    public static DataView changeToSpanishNames(DataView dv)
    {
        DataTable dt = dv.ToTable();
        foreach (DataColumn item in dt.Columns)
        {
            //Nota de intercambio
            item.ColumnName = lablesXML.getNameSpanish(item.ColumnName, "ES");
        }
        dt.AcceptChanges();
        return dt.DefaultView;
    }

    public static void changeToSpanishNames(ref GridView gr)
    {
        foreach (DataControlField item in gr.Columns)
        {
            //Nota de intercambio
            item.HeaderText = lablesXML.getNameSpanish(item.HeaderText, "ES");
        }
    }
}
