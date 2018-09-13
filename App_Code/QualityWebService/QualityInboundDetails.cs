using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de QualityInboundDetails
/// </summary>
public class QualityInboundDetails
{
	public QualityInboundDetails()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public int IDQualityHeader { get; set; }
    public int IDProductClass { get; set; }
    public String vFolio { get; set; }
    public String txtCasesTotales { get; set; }
    public GeneralDetailsQuality[] DTGeneralDetails { get; set; }

    

    public Dictionary<String,object> toParam(){

        Dictionary<String, object> param = new Dictionary<string, object>();

        DataTable dt = new GeneralDetailsQuality().toDataTable();

        foreach(GeneralDetailsQuality qa in DTGeneralDetails){
            dt.Rows.Add(qa.toDataRow());
        }

        param.Add("@DTGeneralDetails", dt);
        param.Add("@IDQualityHeader", this.IDQualityHeader);
        param.Add("@IDProductClass", this.IDProductClass);
        param.Add("@vFolio", this.vFolio);
        param.Add("@txtCasesTotales", this.txtCasesTotales);

        return param;
    }

}