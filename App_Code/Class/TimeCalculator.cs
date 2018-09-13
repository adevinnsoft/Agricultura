using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data;

/// <summary>
/// Summary description for TimeCalculator
/// </summary>
public class TimeCalculator
{
    DataAccess da = new DataAccess();
    public TimeCalculator()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int getWeeksFromYear(int year)
    {
        
        return da.executeStoreProcedureGetInt("spr_getMaxWeekFromYear", new Dictionary<string, object>() { 
            {"@anio",year}
        });
        //DateTime SemanaInicial = new DateTime(year, 1, 1);
        //DateTime SemanaFinal = new DateTime(year, 12, 31);
        //int w = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(SemanaInicial, CalendarWeekRule.FirstFourDayWeek, SemanaInicial.DayOfWeek);
        //if (SemanaFinal.DayOfWeek.Equals(DayOfWeek.Thursday) || (isBisiesto(SemanaFinal.Year) && (SemanaFinal.DayOfWeek.Equals(DayOfWeek.Thursday) || SemanaFinal.DayOfWeek.Equals(DayOfWeek.Friday))))
        //    return 53;
        //else 
        //    return 52;
    }
    public int getActualWeek()
    {
        DataTable dt = da.executeStoreProcedureDataTable("spr_getNSDate", new Dictionary<string, object>() { 
            {"@fecha", DateTime.Now}
        });
        if (dt.Rows.Count > 0)
            return int.Parse(dt.Rows[0]["iShortWeek"].ToString());
        else
        {
            return 0;
        }
        //DateTime SemanaInicial = new DateTime(DateTime.Now.Year, 1, 1);
        //DateTime SemanaFinal = new DateTime(DateTime.Now.Year, 12, 31);
        //int w = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, SemanaInicial.DayOfWeek);
        //return w;
    }

    public int getWeekFromDate(DateTime dtime)
    {
        //DateTime SemanaInicial = new DateTime(DateTime.Now.Year, 1, 1);
        //DateTime SemanaFinal = new DateTime(DateTime.Now.Year, 12, 31);
        //int w = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(dtime, CalendarWeekRule.FirstFourDayWeek, SemanaInicial.DayOfWeek);
        //return w;
        DataTable dt = da.executeStoreProcedureDataTable("spr_getNSDate", new Dictionary<string, object>() { 
            {"@fecha", dtime}
        });
        if (dt.Rows.Count > 0)
            return int.Parse(dt.Rows[0]["iShortWeek"].ToString());
        else
        {
            return 0;
        }
    }
    public int getYearFromDate(DateTime dtime)
    {
        //DateTime SemanaInicial = new DateTime(DateTime.Now.Year, 1, 1);
        //DateTime SemanaFinal = new DateTime(DateTime.Now.Year, 12, 31);
        //int w = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(dtime, CalendarWeekRule.FirstFourDayWeek, SemanaInicial.DayOfWeek);
        //return w;
        DataTable dt = da.executeStoreProcedureDataTable("spr_getNSDate", new Dictionary<string, object>() { 
            {"@fecha", dtime}
        });
        if (dt.Rows.Count > 0)
            return int.Parse(dt.Rows[0]["iYear"].ToString());
        else
        {
            return 0;
        }
    }

    public DateTime getDate(int semana, int anio)
    {
        DataTable dt = da.executeStoreProcedureDataTable("spr_getNSDate", new Dictionary<string, object>() { 
            {"@anio", anio},
            {"@semana", semana}
        });
        if (dt.Rows.Count > 0)
            return DateTime.ParseExact(dt.Rows[0]["StartDate"].ToString(), "dd/MM/yyyy", new CultureInfo("es-MX"));
        else
        {
            return new DateTime();
        }
    }

    private bool isBisiesto(int año)
    {
        if (año % 4 == 0 && año % 100 != 0 || año % 400 == 0)
            return true;
        return false;
    }
}