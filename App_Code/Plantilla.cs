using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HPSF;
using NPOI.HSSF.Record.Crypto;
using NPOI.POIFS.FileSystem;

/// <summary>
/// Summary description for Plantilla
/// </summary>
public class Plantilla
{
    public string nombreDelArchivo;
    public HttpResponse Response;
    public HSSFWorkbook hssfworkbook;

    /*CONSTRUCTOR*/
    //Genera un documento vacio
    public Plantilla(HttpResponse Response, String PreName)
    {
        this.Response = Response;
        string filename = PreName + (nombreDelArchivo == null ? DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") : nombreDelArchivo);
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename + ".xls"));
        Response.Clear();
        InitializeWorkbook();
    }

    //Crea una tabla a partir de los parametros indicados
    public void CrearTabla(List<string> Columns, int Rows, int X, int Y, string headerText, string tableName, string hoja)
    {
        ISheet Sheet1 = hssfworkbook.GetSheet(hoja) != null ? hssfworkbook.GetSheet(hoja) : hssfworkbook.CreateSheet(hoja);
        ICell Title = Sheet1.GetRow(Y) != null ? (Sheet1.GetRow(Y).GetCell(X) != null ? Sheet1.GetRow(Y).GetCell(X) : Sheet1.GetRow(Y).CreateCell(X)) : Sheet1.CreateRow(Y).CreateCell(X);

        Title.CellStyle = EstTitulos;
        Title.SetCellValue(headerText);
        Sheet1.AddMergedRegion(new CellRangeAddress(Y, Y + 1, X, X + Columns.Count - 1));
        /* CAMPOS */

        IRow RowColumns = Sheet1.GetRow(Y + 2) != null ? Sheet1.GetRow(Y + 2) : Sheet1.CreateRow(Y + 2);
        for (int c = 0; c < Columns.Count; c++)
        {
            ICell CellColumn = RowColumns.GetCell(X + c) != null ? RowColumns.GetCell(X + c) : RowColumns.CreateCell(X + c);
            CellColumn.CellStyle = EstSubtitulos;
            CellColumn.SetCellValue(Columns[c]);
            Sheet1.AutoSizeColumn(CellColumn.ColumnIndex);
        }
        for (int i = 0; i < Rows; i++)
        {
            IRow CurrentRow = Sheet1.GetRow(Y + i + 3) != null ? Sheet1.GetRow(Y + i + 3) : Sheet1.CreateRow(Y + i + 3);
            for (int j = 0; j < Columns.Count; j++)
            {
                ICell currentCell = CurrentRow.GetCell(X + j) != null ? CurrentRow.GetCell(X + j) : CurrentRow.CreateCell(X + j);
                currentCell.CellStyle = EstCeldas;
            }
        }
        IName name = hssfworkbook.CreateName();
        if (hssfworkbook.GetName(tableName) == null)
            name.NameName = tableName;
        else
            throw new Exception("No se pueden crear dos espacios de nombre iguales.");
        name.RefersToFormula = "'" + hoja + "'" + "!$" + getChar(X + 1) + "$" + (X + 3) + ":$" + getChar(Y + Columns.Count) + "$" + (X + 3 + Rows);

    }

    public void CrearTabla(List<string> Columns, List<string> Rows, int X, int Y, string headerText, string tableName, string hoja)
    {
        ISheet Sheet1 = hssfworkbook.GetSheet(hoja) != null ? hssfworkbook.GetSheet(hoja) : hssfworkbook.CreateSheet(hoja);
        ICell Title = Sheet1.GetRow(Y) != null ? (Sheet1.GetRow(Y).GetCell(X) != null ? Sheet1.GetRow(Y).GetCell(X) : Sheet1.GetRow(Y).CreateCell(X)) : Sheet1.CreateRow(Y).CreateCell(X);

        Title.CellStyle = EstTitulos;
        Title.SetCellValue(headerText);
        Sheet1.AddMergedRegion(new CellRangeAddress(Y, Y + 1, X, X + Columns.Count - 1));
        /* CAMPOS */

        IRow RowColumns = Sheet1.GetRow(Y + 2) != null ? Sheet1.GetRow(Y + 2) : Sheet1.CreateRow(Y + 2);
        for (int c = 0; c < Columns.Count; c++)
        {
            ICell CellColumn = RowColumns.GetCell(X + c) != null ? RowColumns.GetCell(X + c) : RowColumns.CreateCell(X + c);
            CellColumn.CellStyle = EstSubtitulos;
            CellColumn.SetCellValue(Columns[c]);
            Sheet1.AutoSizeColumn(CellColumn.ColumnIndex);
            Sheet1.SetColumnWidth(CellColumn.ColumnIndex, int.Parse((Sheet1.GetColumnWidth(CellColumn.ColumnIndex) * 2).ToString()));
        }
        for (int i = 0; i < Rows.Count; i++)
        {
            IRow CurrentRow = Sheet1.GetRow(Y + i + 3) != null ? Sheet1.GetRow(Y + i + 3) : Sheet1.CreateRow(Y + i + 3);
            for (int j = 0; j < Columns.Count; j++)
            {
                ICell currentCell = CurrentRow.GetCell(X + j) != null ? CurrentRow.GetCell(X + j) : CurrentRow.CreateCell(X + j);
                currentCell.CellStyle = EstCeldas;
                if (j == 0)
                {
                    currentCell.CellStyle = EstSubtitulos;
                    currentCell.SetCellValue(Rows[i].ToString());
                }
            }
        }
        IName name = hssfworkbook.CreateName();
        name.NameName = tableName;
        //name.RefersToFormula = "'" + hoja + "'" + "!$" + getChar(X + 1) + "$" + (Y + 3) + ":$" + getChar(X + Columns.Count) + "$" + (Y + 3 + Rows.Count);
        name.RefersToFormula = "'" + hoja + "'" + "!$" + ColumnIndexToColumnLetter(X + 1) + "$" + (Y + 3) + ":$" + ColumnIndexToColumnLetter(X + Columns.Count) + "$" + (Y + 3 + Rows.Count);

    }

    public void CrearTablaCustom(List<string> Columns, List<string> Rows, int X, int Y, string headerText, string tableName, string hoja, List<string> Formato)
    {
        ISheet Sheet1 = hssfworkbook.GetSheet(hoja) != null ? hssfworkbook.GetSheet(hoja) : hssfworkbook.CreateSheet(hoja);
        ICell Title = Sheet1.GetRow(Y) != null ? (Sheet1.GetRow(Y).GetCell(X) != null ? Sheet1.GetRow(Y).GetCell(X) : Sheet1.GetRow(Y).CreateCell(X)) : Sheet1.CreateRow(Y).CreateCell(X);

        Title.CellStyle = EstTitulos;
        Title.SetCellValue(headerText);
        Sheet1.AddMergedRegion(new CellRangeAddress(Y, Y + 1, X, X + Columns.Count - 1));
        /* CAMPOS */

        IRow RowColumns = Sheet1.GetRow(Y + 2) != null ? Sheet1.GetRow(Y + 2) : Sheet1.CreateRow(Y + 2);
        for (int c = 0; c < Columns.Count; c++)
        {
            ICell CellColumn = RowColumns.GetCell(X + c) != null ? RowColumns.GetCell(X + c) : RowColumns.CreateCell(X + c);
            CellColumn.CellStyle = EstSubtitulos;
            CellColumn.SetCellValue(Columns[c]);
            Sheet1.AutoSizeColumn(CellColumn.ColumnIndex);
            Sheet1.SetColumnWidth(CellColumn.ColumnIndex, int.Parse((Sheet1.GetColumnWidth(CellColumn.ColumnIndex) * 2).ToString()));
        }
        for (int i = 0; i < Rows.Count; i++)
        {
            IRow CurrentRow = Sheet1.GetRow(Y + i + 3) != null ? Sheet1.GetRow(Y + i + 3) : Sheet1.CreateRow(Y + i + 3);
            for (int j = 0; j < Columns.Count; j++)
            {
                ICell currentCell = CurrentRow.GetCell(X + j) != null ? CurrentRow.GetCell(X + j) : CurrentRow.CreateCell(X + j);
                currentCell.CellStyle = ColumnSetTipe(Formato[j].ToString().Split('.')[1]);//EstText;
                if (j == 0)
                {
                    currentCell.CellStyle = EstSubtitulos;
                    currentCell.SetCellValue(Rows[i].ToString());
                }
            }
        }
        IName name = hssfworkbook.CreateName();
        name.NameName = tableName;
        //name.RefersToFormula = "'" + hoja + "'" + "!$" + getChar(X + 1) + "$" + (Y + 3) + ":$" + getChar(X + Columns.Count) + "$" + (Y + 3 + Rows.Count);
        name.RefersToFormula = "'" + hoja + "'" + "!$" + ColumnIndexToColumnLetter(X + 1) + "$" + (Y + 3) + ":$" + ColumnIndexToColumnLetter(X + Columns.Count) + "$" + (Y + 3 + Rows.Count);

    }

    public ICellStyle ColumnSetTipe(string type)
    {
        switch (type)
        {
            case "Int32":
                return EstCeldas;
                break;
            case "String":
                return EsString;
                break;
            case "Single":
                return EsString;
                break;
            case "DateTime":
                return EsDateTime;
                break;
            case "Boolean":
                return EstCeldas;
                break;
            default:
                return EsString;
                break;
        }
    }

    static string ColumnIndexToColumnLetter(int colIndex)
    {
        int div = colIndex;
        string colLetter = String.Empty;
        int mod = 0;

        while (div > 0)
        {
            mod = (div - 1) % 26;
            colLetter = (char)(65 + mod) + colLetter;
            div = (int)((div - mod) / 26);
        }
        return colLetter;
    }

    public void CrearTabla(DataTable dt, int X, int Y, string headerText, string tableName, string hoja)
    {

        ISheet Sheet1 = hssfworkbook.GetSheet(hoja) != null ? hssfworkbook.GetSheet(hoja) : hssfworkbook.CreateSheet(hoja);
        ICell Title = Sheet1.GetRow(Y) != null ? (Sheet1.GetRow(Y).GetCell(X) != null ? Sheet1.GetRow(Y).GetCell(X) : Sheet1.GetRow(Y).CreateCell(X)) : Sheet1.CreateRow(Y).CreateCell(X);

        Title.CellStyle = EstTitulos;
        Title.SetCellValue(headerText);
        Sheet1.AddMergedRegion(new CellRangeAddress(Y, Y + 1, X, X + dt.Columns.Count - 1));
        /* CAMPOS */

        IRow RowColumns = Sheet1.GetRow(Y + 2) != null ? Sheet1.GetRow(Y + 2) : Sheet1.CreateRow(Y + 2);
        for (int c = 0; c < dt.Columns.Count; c++)
        {
            ICell CellColumn = RowColumns.GetCell(X + c) != null ? RowColumns.GetCell(X + c) : RowColumns.CreateCell(X + c);
            CellColumn.CellStyle = EstSubtitulos;
            CellColumn.SetCellValue(dt.Columns[c].ColumnName);
            Sheet1.AutoSizeColumn(CellColumn.ColumnIndex);
            Sheet1.SetColumnWidth(CellColumn.ColumnIndex, int.Parse((Sheet1.GetColumnWidth(CellColumn.ColumnIndex) * 2).ToString()));
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            IRow CurrentRow = Sheet1.GetRow(Y + i + 3) != null ? Sheet1.GetRow(Y + i + 3) : Sheet1.CreateRow(Y + i + 3);
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ICell currentCell = CurrentRow.GetCell(X + j) != null ? CurrentRow.GetCell(X + j) : CurrentRow.CreateCell(X + j);
                currentCell.CellStyle = EstCeldas;
                if (j == 0)
                {
                    currentCell.CellStyle = EstSubtitulos;
                    currentCell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
        }
        IName name = hssfworkbook.CreateName();
        name.NameName = tableName;
        name.RefersToFormula = "'" + hoja + "'" + "!$" + ColumnIndexToColumnLetter(X + 1) + "$" + (Y + 3) + ":$" + ColumnIndexToColumnLetter(X + dt.Columns.Count) + "$" + (Y + 3 + dt.Rows.Count);

    }

    public void CrearTablaExport(DataTable dt, int X, int Y, string headerText, string tableName, string hoja)
    {

        ISheet Sheet1 = hssfworkbook.GetSheet(hoja) != null ? hssfworkbook.GetSheet(hoja) : hssfworkbook.CreateSheet(hoja);
        ICell Title = Sheet1.GetRow(Y) != null ? (Sheet1.GetRow(Y).GetCell(X) != null ? Sheet1.GetRow(Y).GetCell(X) : Sheet1.GetRow(Y).CreateCell(X)) : Sheet1.CreateRow(Y).CreateCell(X);

        Title.CellStyle = EstTitulos;
        Title.SetCellValue(headerText);
        Sheet1.AddMergedRegion(new CellRangeAddress(Y, Y + 1, X, X + dt.Columns.Count - 1));
        /* CAMPOS */

        IRow RowColumns = Sheet1.GetRow(Y + 2) != null ? Sheet1.GetRow(Y + 2) : Sheet1.CreateRow(Y + 2);
        for (int c = 0; c < dt.Columns.Count; c++)
        {
            ICell CellColumn = RowColumns.GetCell(X + c) != null ? RowColumns.GetCell(X + c) : RowColumns.CreateCell(X + c);
            CellColumn.CellStyle = EstSubtitulos;
            CellColumn.SetCellValue(dt.Columns[c].ColumnName);
            Sheet1.AutoSizeColumn(CellColumn.ColumnIndex);
            Sheet1.SetColumnWidth(CellColumn.ColumnIndex, int.Parse((Sheet1.GetColumnWidth(CellColumn.ColumnIndex) * 2).ToString()));
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            IRow CurrentRow = Sheet1.GetRow(Y + i + 3) != null ? Sheet1.GetRow(Y + i + 3) : Sheet1.CreateRow(Y + i + 3);
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                ICell currentCell = CurrentRow.GetCell(X + j) != null ? CurrentRow.GetCell(X + j) : CurrentRow.CreateCell(X + j);
                currentCell.CellStyle = EstCeldas;
                if (j == 0)
                {
                    currentCell.CellStyle = EstSubtitulos;
                }
                currentCell.SetCellValue(dt.Rows[i][j].ToString() == "0" ? "" : dt.Rows[i][j].ToString());
            }
        }
        IName name = hssfworkbook.CreateName();
        name.NameName = tableName;
        name.RefersToFormula = "'" + hoja + "'" + "!$" + ColumnIndexToColumnLetter(X + 1) + "$" + (Y + 3) + ":$" + ColumnIndexToColumnLetter(X + dt.Columns.Count) + "$" + (Y + 3 + dt.Rows.Count);

    }


    //Descarga del documento creado
    public void GuardarPlantilla()
    {

        if (hssfworkbook.NumberOfSheets > 0)
        {
            MemoryStream file = new MemoryStream();
            hssfworkbook.Write(file = new MemoryStream());
            Response.BinaryWrite(file.GetBuffer());
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        else
        {
            evitarDescargaDeArchivo();
        }
    }

    public void evitarDescargaDeArchivo()
    {
        HttpContext.Current.ApplicationInstance.Dispose();
        Response.ClearHeaders();
    }

    public void ProtegerArchivo(string contrasena)
    {
        for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
        {
            ISheet Sheet = hssfworkbook.GetSheetAt(i);
            Sheet.ProtectSheet(contrasena);
            Sheet.SetActiveCell(0, 0);
        }
    }
    public void ProtegerArchivo()
    {
        for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
        {
            ISheet Sheet = hssfworkbook.GetSheetAt(i);
            Sheet.ProtectSheet("<P@ssw0rd>");
            Sheet.SetActiveCell(0, 0);
        }
    }



    #region Estilos


    ICellStyle EstTitulos;
    ICellStyle EstSubtitulos;
    ICellStyle EsString;
    ICellStyle EstCeldas;
    ICellStyle EstCeldasDate;
    ICellStyle EsDateTime;
    ICellStyle EstCeldasP;
    ICellStyle EstCeldasPercentLocked;
    ICellStyle EstInvernadero;
    ICellStyle RenglonAmarillo;
    ICellStyle RenglonBlanco;



    private void GenerarFormatos()
    {
        HSSFPalette Colores = hssfworkbook.GetCustomPalette();

        Colores.SetColorAtIndex(HSSFColor.GREEN.index, (byte)83, (byte)147, (byte)91); //Verde
        Colores.SetColorAtIndex(HSSFColor.DARK_YELLOW.index, (byte)231, (byte)204, (byte)45); //Amarillo Fuerte
        Colores.SetColorAtIndex(HSSFColor.LIGHT_YELLOW.index, (byte)248, (byte)241, (byte)196); //Amarillo Suave
        Colores.SetColorAtIndex(HSSFColor.WHITE.index, (byte)255, (byte)255, (byte)255); //Blanco
        Colores.SetColorAtIndex(HSSFColor.BLACK.index, (byte)0, (byte)0, (byte)0); //Negro

        Colores.SetColorAtIndex(HSSFColor.GREY_25_PERCENT.index, (byte)216, (byte)228, (byte)188); //Verde Suave

        IFont FBlanca12 = hssfworkbook.CreateFont();
        FBlanca12.Color = HSSFColor.WHITE.index;
        FBlanca12.IsItalic = false;
        FBlanca12.FontHeightInPoints = 12;

        IFont FBlanca10 = hssfworkbook.CreateFont();
        FBlanca10.Color = HSSFColor.WHITE.index;
        FBlanca10.IsItalic = false;
        FBlanca10.FontHeightInPoints = 10;

        IFont FNegra = hssfworkbook.CreateFont();
        FNegra.Color = HSSFColor.BLACK.index;
        FNegra.IsItalic = false;
        FNegra.FontHeightInPoints = 10;

        EstTitulos = hssfworkbook.CreateCellStyle();
        EstTitulos.Alignment = HorizontalAlignment.CENTER;
        EstTitulos.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstTitulos.FillForegroundColor = HSSFColor.GREEN.index;
        EstTitulos.VerticalAlignment = VerticalAlignment.CENTER;
        EstTitulos.SetFont(FBlanca10);
        EstTitulos.IsLocked = true;

        EstInvernadero = hssfworkbook.CreateCellStyle();
        EstInvernadero.Alignment = HorizontalAlignment.CENTER;
        EstInvernadero.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstInvernadero.FillForegroundColor = HSSFColor.GREY_25_PERCENT.index;
        EstInvernadero.VerticalAlignment = VerticalAlignment.CENTER;
        EstInvernadero.SetFont(FNegra);
        EstInvernadero.IsLocked = true;

        EstSubtitulos = hssfworkbook.CreateCellStyle();
        EstSubtitulos.Alignment = HorizontalAlignment.CENTER;
        EstSubtitulos.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstSubtitulos.FillForegroundColor = HSSFColor.DARK_YELLOW.index;
        EstSubtitulos.SetFont(FBlanca10);
        EstSubtitulos.IsLocked = true;


        EstCeldas = hssfworkbook.CreateCellStyle();
        EstCeldas.Alignment = HorizontalAlignment.CENTER;
        EstCeldas.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstCeldas.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        EstCeldas.SetFont(FNegra);
        EstCeldas.IsLocked = false;

        EsString = hssfworkbook.CreateCellStyle();
        EsString.Alignment = HorizontalAlignment.CENTER;
        EsString.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EsString.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        EsString.SetFont(FNegra);
        EsString.IsLocked = false;
        var formatId = HSSFDataFormat.GetBuiltinFormat("@");
        if (formatId == -1)
        {
            var newDataFormat = hssfworkbook.CreateDataFormat();
            EsString.DataFormat = newDataFormat.GetFormat("@");
        }
        else
            EsString.DataFormat = formatId;

        RenglonAmarillo = hssfworkbook.CreateCellStyle();
        RenglonAmarillo.Alignment = HorizontalAlignment.CENTER;
        RenglonAmarillo.FillPattern = FillPatternType.SOLID_FOREGROUND;
        RenglonAmarillo.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        RenglonAmarillo.SetFont(FNegra);
        RenglonAmarillo.IsLocked = false;

        RenglonBlanco = hssfworkbook.CreateCellStyle();
        RenglonBlanco.Alignment = HorizontalAlignment.CENTER;
        RenglonBlanco.FillPattern = FillPatternType.SOLID_FOREGROUND;
        RenglonBlanco.FillForegroundColor = HSSFColor.WHITE.index;
        RenglonBlanco.SetFont(FNegra);
        RenglonBlanco.IsLocked = false;

        formatId = HSSFDataFormat.GetBuiltinFormat("#,###,##0;-#,###,##0");
        if (formatId == -1)
        {
            var newDataFormat = hssfworkbook.CreateDataFormat();
            EstCeldas.DataFormat = newDataFormat.GetFormat("#,###,##0;-#,###,##0");
        }
        else
            EstCeldas.DataFormat = formatId;

        EstCeldasDate = hssfworkbook.CreateCellStyle();
        EstCeldasDate.Alignment = HorizontalAlignment.CENTER;
        EstCeldasDate.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstCeldasDate.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        EstCeldasDate.SetFont(FNegra);
        EstCeldasDate.IsLocked = false;
        formatId = HSSFDataFormat.GetBuiltinFormat("m/d/yy");
        if (formatId == -1)
        {
            var newDataFormat = hssfworkbook.CreateDataFormat();
            EstCeldasDate.DataFormat = newDataFormat.GetFormat("m/d/yy");
        }
        else
            EstCeldasDate.DataFormat = formatId;

        EsDateTime = hssfworkbook.CreateCellStyle();
        EsDateTime.Alignment = HorizontalAlignment.CENTER;
        EsDateTime.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EsDateTime.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        EsDateTime.SetFont(FNegra);
        EsDateTime.IsLocked = false;
        formatId = HSSFDataFormat.GetBuiltinFormat("YYYY-MM-DD");
        if (formatId == -1)
        {
            var newDataFormat = hssfworkbook.CreateDataFormat();
            EsDateTime.DataFormat = newDataFormat.GetFormat("YYYY-MM-DD");
        }
        else
            EsDateTime.DataFormat = formatId;

        EstCeldasP = hssfworkbook.CreateCellStyle();
        EstCeldasP.Alignment = HorizontalAlignment.CENTER;
        EstCeldasP.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstCeldasP.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        EstCeldasP.SetFont(FNegra);
        EstCeldasP.IsLocked = false;
        formatId = HSSFDataFormat.GetBuiltinFormat("0%");
        if (formatId == -1)
        {
            var newDataFormat = hssfworkbook.CreateDataFormat();
            EstCeldasP.DataFormat = newDataFormat.GetFormat("0%");
        }
        else
            EstCeldasP.DataFormat = formatId;
        EstCeldasPercentLocked = hssfworkbook.CreateCellStyle();
        EstCeldasPercentLocked.Alignment = HorizontalAlignment.CENTER;
        EstCeldasPercentLocked.FillPattern = FillPatternType.SOLID_FOREGROUND;
        EstCeldasPercentLocked.FillForegroundColor = HSSFColor.LIGHT_YELLOW.index;
        EstCeldasPercentLocked.SetFont(FNegra);
        EstCeldasPercentLocked.IsLocked = true;

        formatId = HSSFDataFormat.GetBuiltinFormat("0%");
        if (formatId == -1)
        {
            var newDataFormat = hssfworkbook.CreateDataFormat();
            EstCeldasPercentLocked.DataFormat = newDataFormat.GetFormat("0%");
        }

        else
            EstCeldasPercentLocked.DataFormat = formatId;





    }
    #endregion

    #region Auxiliares
    private string getChar(int n)
    {
        switch (n)
        {
            case 1: return "A";
            case 2: return "B";
            case 3: return "C";
            case 4: return "D";
            case 5: return "E";
            case 6: return "F";
            case 7: return "G";
            case 8: return "H";
            case 9: return "I";
            case 10: return "J";
            case 11: return "K";
            case 12: return "L";
            case 13: return "M";
            case 14: return "N";
            case 15: return "O";
            case 16: return "P";
            case 17: return "Q";
            case 18: return "R";
            case 19: return "S";
            case 20: return "T";
            case 21: return "U";
            case 22: return "V";
            case 23: return "W";
            case 24: return "X";
            case 25: return "Y";
            case 26: return "Z";
        }
        if (n > 702)
            return getChar(n / 702) + getChar(n - (702 * (n / 702)));
        if (n > 26)
            return getChar(n / 26) + getChar((n - 26) - 26);


        return "0";
    }
    private void InitializeWorkbook()
    {
        hssfworkbook = new HSSFWorkbook();

        //////create a entry of DocumentSummaryInformation
        //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
        //dsi.Company = creador;
        //hssfworkbook.DocumentSummaryInformation = dsi;

        //////create a entry of SummaryInformation
        //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
        //si.Subject = asunto;
        //hssfworkbook.SummaryInformation = si;
        GenerarFormatos();
    }
    public bool protectSheet(string url, string sheetName, string contrasena)
    {
        HSSFWorkbook hssfwb;
        using (FileStream file = new FileStream(@url, FileMode.Open, FileAccess.ReadWrite))
        {
            hssfwb = new HSSFWorkbook(file);
        }
        ISheet sheet = hssfwb.GetSheet(sheetName);
        sheet.ProtectSheet(contrasena);
        if (sheet.Protect)
            return true;
        else
            return false;
    }
    public bool protectSheet(string url, int sheetZeroIndex, string contrasena)
    {
        FileStream file = new FileStream(@url, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        hssfworkbook = new HSSFWorkbook(file);
        ISheet sheet = hssfworkbook.GetSheetAt(sheetZeroIndex);
        sheet.ProtectSheet(contrasena);
        sheet.SetActiveCell(0, 0);
        file = new FileStream(@url, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        hssfworkbook.Write(file);
        if (sheet.Protect)
        {
            file.Close();
            return true;
        }
        else
        {
            file.Close();
            return false;
        }
    }
    #endregion

}