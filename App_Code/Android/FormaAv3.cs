﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de FormaAv3
/// </summary>
public class FormaAv3
{
	public FormaAv3()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}


    public int idFormaA { get; set; }
    public int idFormaALocal { get; set; }
    public int idProgramaLocal { get; set; }
    public string fechaFin { get; set; }
    public string fechaInicio { get; set; }
    public string prefijo { get; set; }
    public string dmcCalidad { get; set; }
    public string dmcMercado { get; set; }
    public string comentarios { get; set; }
    public string folio { get; set; }
    public int idPrograma { get; set; }
    public int cerrada { get; set; }
    public int estatus { get; set; }
    public string fechaFinTractorista { get; set; }
    public string fechaInicioTractorista { get; set; }
    public string storage { get; set; }
    public String UUID { get; set; }
    public int Preharvest { get; set; }
    public int idTipoCaja { get; set; }
    public int idTipoCosecha { get; set; }
}