using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de QualityHeader
/// </summary>
public class QualityHeader
{
    public QualityHeader()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public Int32 ACCION { get; set; }
    public String vVaariety { get; set; }
    public decimal nNetWeight { get; set; }
    public DateTime dReceiptDate { get; set; }
    public DateTime dHarvestStart { get; set; }
    public DateTime dHarvestEnd { get; set; }
    public Int32 iCutNumber { get; set; }
    public decimal nGrossWeight { get; set; }
    public Int32 IdProduct { get; set; }
    public Int32 IdProductClass { get; set; }
    public String txtPlanta { get; set; }
    //public String txtCorteNumero { get; set; }
    public Int32 txtSemana { get; set; }
    public String txtSecciones { get; set; }
    //public String txtLibrasTotales { get; set; }
    public String txtInvernadero { get; set; }
    //public String txtHoraCosechaInicio { get; set; }
    //public String txtHoraRecepcion { get; set; }
    //public String txtHoraCosechaFin { get; set; }
    public Int32 txtDiasCorte { get; set; }
    public String txtApuntador { get; set; }
    public String txtPuntoRocio { get; set; }
    public String txtHumedadRelativa { get; set; }
    public Decimal txtTempAmbiente { get; set; }
    public Decimal txtPesoMuestra { get; set; }
    public String txtVFolio { get; set; }
    public String ddlAnalistas { get; set; }
    public String txtComments { get; set; }
    public Int32 SelectedQty { get; set; }
    public Int32 SelectedPolvo { get; set; }
    public Int32 SelectedManchado { get; set; }
    public String txtCasesTotales { get; set; }
    public String txtStatus { get; set; }
    public String txtQtySugerida { get; set; }
    public String bExpected { get; set; }
    public String bPesticide { get; set; }
    public String bCondensed { get; set; }
    public String ddlDisposition { get; set; }
    public Decimal txtTempPulp { get; set; }
    public Int32 txtFlavor { get; set; }
    public GeneralDetailsQuality[] DTGeneralDetails { get; set; }

    public Dictionary<String, object> toParam()
    {
        Dictionary<String, object> param = new Dictionary<string, object>();
        DataTable dt = new GeneralDetailsQuality().toDataTable();

        foreach (GeneralDetailsQuality qa in DTGeneralDetails)
        {
            dt.Rows.Add(qa.toDataRow().ItemArray);
        }

        param.Add("@ACCION", 1);
        param.Add("@vVariety", this.vVaariety);
        //param.Add("@nNetWeight", this.nNetWeight);
        //param.Add("@dReceiptDate", this.dReceiptDate);
        //param.Add("@dHarvestStart", this.dHarvestStart);
        //param.Add("@dHarvestEnd", this.dHarvestEnd);
        //param.Add("@iCutNumber", this.iCutNumber);
        param.Add("@nGrossWeight", this.nGrossWeight);
        param.Add("@IdProduct", this.IdProduct);
        param.Add("@IdProductClass", this.IdProductClass);
        param.Add("@txtPlanta", this.txtPlanta);
        param.Add("@txtCorteNumero", this.iCutNumber);
        param.Add("@txtSemana", this.txtSemana);
        param.Add("@txtSecciones", this.txtSecciones);
        param.Add("@txtLibrasTotales", this.nNetWeight);
        param.Add("@txtInvernadero", this.txtInvernadero);
        param.Add("@txtHoraCosechaInicio", this.dHarvestStart.ToString("yyyy-MM-dd HH:mm"));
        param.Add("@txtHoraRecepcion", this.dReceiptDate.ToString("yyyy-MM-dd HH:mm"));
        param.Add("@txtHoraCosechaFin", this.dHarvestEnd.ToString("yyyy-MM-dd HH:mm"));
        param.Add("@txtDiasCorte", this.txtDiasCorte);
        param.Add("@txtApuntador", this.txtApuntador);
        param.Add("@txtPuntoRocio", this.txtPuntoRocio);
        param.Add("@txtHumedadRelativa", this.txtHumedadRelativa);
        param.Add("@txtTempAmbiente", this.txtTempAmbiente);
        param.Add("@txtPesoMuestra", this.txtPesoMuestra);
        param.Add("@txtVFolio", this.txtVFolio);
        param.Add("@ddlAnalistas", this.ddlAnalistas);
        param.Add("@txtComments", this.txtComments);
        param.Add("@SelectedQty", this.SelectedQty);
        param.Add("@SelectedPolvo", this.SelectedPolvo);
        param.Add("@SelectedManchado", this.SelectedManchado);
        param.Add("@txtCasesTotales", this.txtCasesTotales);
        param.Add("@txtStatus", this.txtStatus);
        param.Add("@txtQtySugerida", this.txtQtySugerida);
        param.Add("@bExpected", this.bExpected);
        param.Add("@bPesticide", this.bPesticide);
        param.Add("@bCondensed", this.bCondensed);
        param.Add("@ddlDisposition", this.ddlDisposition);
        param.Add("@txtTempPulp", this.txtTempPulp);
        param.Add("@txtFlavor", this.txtFlavor);
        param.Add("@DTGeneralDetails", dt);

        return param;
    }
}