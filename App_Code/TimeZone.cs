using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Reflection;


public static class TimeZone
{

    //TimeZoneInfo localZone = TimeZoneInfo.Local;

    public enum PLANTA
    {
        [Description("Central Standard Time (Mexico)")]
        AEROPUERTO,
        [Description("Central Standard Time (Mexico)")]
        HABILLAS,
        [Description("Central Standard Time (Mexico)")]
        BARBOSA,
        [Description("Central Standard Time (Mexico)")]
        SAISTE,
        [Description("Central Standard Time (Mexico)")]
        RANCHOT
    }

    public static DateTime obtenerHoraDeLaCuenta(DateTime hwTime, PLANTA utc)
    {
       //DateTime hwTime = new DateTime();
        try
        {
            TimeZoneInfo hwZone = TimeZoneInfo.FindSystemTimeZoneById(GetEnumDescription(utc));
            /*return String.Format ("{0} {1} is {2} local time.",
                    hwTime,
                    hwZone.IsDaylightSavingTime(hwTime) ? hwZone.DaylightName : hwZone.StandardName,
                    TimeZoneInfo.ConvertTime(hwTime, hwZone, TimeZoneInfo.Local));*/
            return TimeZoneInfo.ConvertTime(hwTime, TimeZoneInfo.Local, TimeZoneInfo.Local);
        }
        catch (TimeZoneNotFoundException)
        {
            Console.WriteLine("The registry does not define the Hawaiian Standard Time zone.");
        }
        catch (InvalidTimeZoneException)
        {
            Console.WriteLine("Registry data on the Hawaiian STandard Time zone has been corrupted.");
        }
        return DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
    }

    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return value.ToString();
    }

    public static PLANTA obtenerZonaID(int id)
    {
        switch (id)
        {
            case 1:
                return PLANTA.AEROPUERTO;
                break;
            case 2:
                return PLANTA.AEROPUERTO;
                break;
            case 5:
                return PLANTA.HABILLAS;
                break;
            case 6:
                return PLANTA.BARBOSA;
                break;
            default:
                return PLANTA.SAISTE;
                break;
        }
    }
}