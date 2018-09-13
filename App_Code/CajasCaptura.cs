using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CajasCapturaWeb
{
    public int idInvernadero;
    public int idLider;
    public int idCosecha;
    public int idCosechaLocal;
    public int Semana;
    public int Instrucciones;
    public int UsuarioCaptura;
    public int UsuarioModifica;
    public string FechaCaptura;
    public string FechaModifica;
    public int Estatus;
    public CajasDetalleWeb[] Cajas;
}

public class CajasDetalleWeb
{
    public int idEstimadoCajasCaptura;
    public int idEstimadoCajas;
    public int Surco;
    public decimal Caja;
    public string FechaCaptura;
    public string FechaModifica;
    public int Instrucciones;
    public int Estatus;
}