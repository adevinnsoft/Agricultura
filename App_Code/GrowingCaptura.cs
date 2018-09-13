using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GrowingCaptura
{
    public int idCaptura;
    public string Etiqueta;
    public string Comentarios;
    public string FechaCaptura;
    public int idInvernadero;
    public decimal CalificacionCalculada;
    //public decimal Calificacion;
    public int Plantacion;
    public String UUID { get; set; }
    public int Activo { get; set; }
    public int idLider { get; set; }
    public int idGrower { get; set; }
    public int idGerente { get; set; }
    public int idStatus { get; set; }
    public GrowingGrupo[] grupo;
    public GrowingParametro[] parametro;
    public GrowingPropiedad[] propiedad;
}
