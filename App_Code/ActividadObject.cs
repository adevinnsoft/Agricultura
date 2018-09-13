using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de ActividadObject
/// </summary>
public class ActividadObject
{
    private DataAccess dataAccess = new DataAccess();

    private int id;
    private int idPlanta;
    private int idInvernadero;
    private int idLider;
    private int idActividadEtapa;
    private string nombreActividad;
    
    private DateTime inicio;
    private DateTime fin;
    private DateTime finJornales;
    private DateTime finLider;

    private int surcosCapturados;
    private int surcosInvernadero;

    private double plantulasProgramadas;

    private int target;

    private double tiempoEstimado;
    private double tiempoProgramado;

    private int jornalesSugeridos;
    private int JornalesUsados;

    private Boolean directriz;

    private Dictionary<string, string> jornalesMostrados= new Dictionary<string,string>();
    private Dictionary<string, string> jornalesSeleccionados = new Dictionary<string, string>();
    private Dictionary<string, string> etapas = new Dictionary<string, string>();

    private Boolean aprobada;
    private Boolean rechazada;
    private Boolean soloEjecutable;

    private double eficienciaAsociados;

    private string color;

    private int contador;

    //Plantilla para crear el html
    public string html = "";

    public ActividadObject()
    {

    }


	public ActividadObject(int id)
	{
		
	}

    public ActividadObject( string idPlanta, int idInvernadero, string idLider, int idActividadEtapa, DateTime Inicio, DateTime fin, int surcos, bool directriz, int id)
    {
        this.idPlanta = int.Parse(idPlanta);
        this.idInvernadero = idInvernadero;
        this.idLider = int.Parse(idLider);
        this.idActividadEtapa = idActividadEtapa;
        this.inicio = Inicio;
        this.fin = Inicio.AddHours(2);
        this.surcosInvernadero = surcos;
        this.directriz = directriz;
        this.id = id;

        var dt = dataAccess.executeStoreProcedureDataSet("spr_ActividadEjemplo", new Dictionary<string, object>() { { "idHabilidad", idActividadEtapa } });

        this.nombreActividad = dt.Tables[0].Rows[0]["Habilidad"].ToString();
        if (dt.Tables[1].Rows.Count > 0)
        {
            foreach(DataRow d in dt.Tables[1].Rows){
                this.jornalesMostrados.Add(d["ID_EMPLEADO"].ToString(), d["NOMBRE"].ToString());
            }
           
        }
        this.jornalesSugeridos = dt.Tables[1].Rows.Count;

        this.soloEjecutable = Convert.ToBoolean(dt.Tables[0].Rows[0]["Ejecutable"]);

        this.ToHtml();

        try
        {
            
        }
        catch (FormatException fe)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public string ToHtml(){
        
        string result = "<div class=\"e"+this.id+"  col-md-3 panel panel-default grid\">" +
                "<div class=\"panel-heading\" style=\"cursor: pointer;\" onclick=\"var opt = $(this).find('input:checkbox'); if(opt.prop('checked')==true) opt.prop('checked',false); else opt.prop('checked',true); \">" +
                    "<span class=\"NombreHabilidad\">" + this.nombreActividad + "</span><span class=\"text-right\">" +
                        "<input type=\"checkbox\" name=\"ActividadSelect\" class=\"ActividadSelect\" />" +
                    "</span>" +
                "</div>" +
                "<div class=\"panel-body\">" +
                    "<span class=\"idHabilidad\"></span>" +
                    "<table>" +
                        "<tbody>" +
                            "<tr>" +
                                "<td>" +
                                        Resources.Commun.Inicio.ToString() +
                                "</td>" +
                                "<td>" +
                                    "<span class=\"FechaHoraInicio\">"+this.inicio+"</span>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td>" +
                                    Resources.Commun.Fin.ToString()+
                                "</td>"+
                                "<td>"+
                                    "<span class=\"FechaHoraFin\">"+this.fin+"</span>"+
                                "</td>"+
                            "</tr>";
                            if(this.etapas.Count > 0 && !this.soloEjecutable){
                                result += "<tr>" +
                                "<td>" +
                                        Resources.Commun.Etapa.ToString() +
                                    "</td>" +
                                    "<td>";
                                foreach (var item in etapas)
                                {

                                    result += "<input type=\"radio\" value=\""+item.Key+"\" name=\"Etapa\" />"+item.Value;
                                    
                                }

                                result+="</td>"+
                                    "</tr>";
                            }
                            if (!this.soloEjecutable)
                            {
                                result += "<tr>" +
                                    "<td>" +
                                            Resources.Commun.Surcos.ToString() +
                                   "</td>" +
                                    "<td>" +
                                        "<input type=\"text\" class=\"Surcos\" style=\"width: 30px;\" /><span class=\"SurcosTotales\">" + this.surcosInvernadero + "</span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        Resources.Commun.Target.ToString() +
                                    "</td>" +
                                    "<td>" +
                                        "<select class=\"Target\">" +
                                            "<option value=\"1\">1</option>" +
                                            "<option value=\"2\">2</option>" +
                                            "<option value=\"3\">3</option>" +
                                        "</select>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        Resources.Commun.TiempoEstimado.ToString() +
                                    "</td>" +
                                    "<td>" +
                                        "<span class=\"TiempoEstimado\">"+(this.fin - this.inicio).TotalMinutes+"</span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        Resources.Commun.JornalesEstimados.ToString() +
                                    "</td>" +
                                    "<td>" +
                                        "<span class=\"Jornales\">"+this.jornalesSugeridos+"</span>" +
                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td>" +
                                        Resources.Commun.JornalesUsados.ToString() +
                                    "</td>" +
                                    "<td>" +
                                        "<input type=\"text\" class=\"txtCantidadJornales\" disabled style=\"width: 30px;\" />" +
                                    "</td>" +
                                "</tr>";
                           }
                            result += "<tr>" +
                                 "<td colspan=\"2\" style=\" overflow-y:auto; height:150px !important;\">";
                                foreach (var item in this.jornalesMostrados){
                                    result += "<input type=\"checkbox\" value=\""+item.Key+"\"/>"+item.Value+"<br/>";
                                }
                                   result+="</td>"+
                            "</tr>"+
                        "</tbody>"+
                    "</table>"+
                "</div>" +
            "</div>";

        return this.html = result;
    }



}