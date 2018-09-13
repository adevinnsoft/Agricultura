using System;
using System.IO;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

public class NpoiExport : IDisposable
{
    const int MaximumNumberOfRowsPerSheet = 65500;
    const int MaximumSheetNameLength = 25;
    protected IWorkbook Workbook { get; set; }

    public NpoiExport()
    {
        this.Workbook = new HSSFWorkbook();
    }

    protected string EscapeSheetName(string sheetName)
    {
        var escapedSheetName = sheetName
                                    .Replace("/", "-")
                                    .Replace("\\", " ")
                                    .Replace("?", string.Empty)
                                    .Replace("*", string.Empty)
                                    .Replace("[", string.Empty)
                                    .Replace("]", string.Empty)
                                    .Replace(":", string.Empty);

        if (escapedSheetName.Length > MaximumSheetNameLength)
            escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

        return escapedSheetName;
    }

    protected ISheet createExportDataTableSheetAndHeaderRow(DataTable exportData, string sheetName, ICellStyle headerRowStyle)
    {
        var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));

        // Create the header row
        var row = sheet.CreateRow(0);

        for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
        {
            var cell = row.CreateCell(colIndex);
            cell.SetCellValue(exportData.Columns[colIndex].ColumnName);

            if (headerRowStyle != null)
                cell.CellStyle = headerRowStyle;
        }

        return sheet;
    }

    public void ExportDataTableToWorkbook(DataTable exportData, string sheetName)
    {
        // Create the header row cell style
        var headerLabelCellStyle = this.Workbook.CreateCellStyle();
        //headerLabelCellStyle.BorderBottom = HSSFCellBorderType.THIN;
        var headerLabelFont = this.Workbook.CreateFont();
        headerLabelFont.Boldweight = (short)FontBoldWeight.BOLD;
        headerLabelCellStyle.SetFont(headerLabelFont);

        var sheet = createExportDataTableSheetAndHeaderRow(exportData, sheetName, headerLabelCellStyle);
        var currentNPOIRowIndex = 1;
        var sheetCount = 1;

        for (var rowIndex = 0; rowIndex < exportData.Rows.Count; rowIndex++)
        {
            if (currentNPOIRowIndex >= MaximumNumberOfRowsPerSheet)
            {
                sheetCount++;
                currentNPOIRowIndex = 1;

                sheet = createExportDataTableSheetAndHeaderRow(exportData, 
                                                               sheetName + " - " + sheetCount, 
                                                               headerLabelCellStyle);
            }

            var row = sheet.CreateRow(currentNPOIRowIndex++);

            for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
            {
                var cell = row.CreateCell(colIndex);
                cell.SetCellValue(exportData.Rows[rowIndex][colIndex].ToString());
            }
        }
    }

    public byte[] GetBytes()
    {
        using (var buffer = new MemoryStream())
        {
            this.Workbook.Write(buffer);
            return buffer.GetBuffer();
        }
    }

    public bool WriteToFile(string _FileName)
    {
        try
        {
            // Open file for reading
            System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

            // Writes a block of bytes to this stream using data from a byte array.
            _FileStream.Write(this.GetBytes(), 0, this.GetBytes().Length);

            // close file stream
            _FileStream.Close();

            return true;
        }
        catch (Exception _Exception)
        {
            // Error
            Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
        }

        // error occured, return false
        return false;
    }


    public void Dispose()
    {
        
    }
}