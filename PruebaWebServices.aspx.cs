using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PruebaWebServices : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SimpleServiceReference.AndroidSync simpleService = new SimpleServiceReference.AndroidSync();
        //simpleService.SyncAllV5("[]","[]","[]","[]","[]","[]","[]","[]","[]","[]",1336,5990043,"[]","[]","[]","[]","[]","[]","[]","[]","[]","[]","[]","[{"UUID":"f3cb98c3-aedb-449b-89a8-172e83ca43f7","calidad":9.0,"cantidadSurcos":1,"comentarios":"","estatus":1,"fechaCaptura":"2018-01-31 10:37:13","fechaFin":"2018-01-29 13:59:00","fechaInicio":"2018-01-29 09:30:00","fechaModifico":"2018-01-31 10:37:13","idActividadPrograma":232296,"idActividadProgramaLocal":2,"idAsociado":201863,"idCapturaHeaderHistoria":0,"idCapturaHeaderHistoriaLocal":1,"idPeriodo":409819,"idPeriodoLocal":7,"idUsuario":0,"surcoFin":1,"surcoInicio":1},{"UUID":"ab2cf523-a57e-4e68-9b68-50b32e3af475","calidad":9.0,"cantidadSurcos":1,"comentarios":"","estatus":1,"fechaCaptura":"2018-01-31 10:37:55","fechaFin":"2018-01-29 13:59:00","fechaInicio":"2018-01-29 09:30:00","fechaModifico":"2018-01-31 10:37:54","idActividadPrograma":232296,"idActividadProgramaLocal":2,"idAsociado":201863,"idCapturaHeaderHistoria":0,"idCapturaHeaderHistoriaLocal":2,"idPeriodo":409819,"idPeriodoLocal":7,"idUsuario":0,"surcoFin":1,"surcoInicio":1},{"UUID":"f7117e47-de89-41db-9487-0a424a334efc","calidad":9.0,"cantidadSurcos":1,"comentarios":"","estatus":1,"fechaCaptura":"2018-01-31 11:10:25","fechaFin":"2018-01-29 13:59:00","fechaInicio":"2018-01-29 09:30:00","fechaModifico":"2018-01-31 11:10:25","idActividadPrograma":232296,"idActividadProgramaLocal":2,"idAsociado":201863,"idCapturaHeaderHistoria":0,"idCapturaHeaderHistoriaLocal":3,"idPeriodo":409819,"idPeriodoLocal":7,"idUsuario":0,"surcoFin":1,"surcoInicio":1},{"UUID":"aac96671-75a0-4b93-95ce-eed5d0a8e931","calidad":9.0,"cantidadSurcos":1,"comentarios":"","estatus":1,"fechaCaptura":"2018-01-31 11:17:30","fechaFin":"2018-01-29 13:59:00","fechaInicio":"2018-01-29 09:30:00","fechaModifico":"2018-01-31 11:17:30","idActividadPrograma":232296,"idActividadProgramaLocal":2,"idAsociado":201863,"idCapturaHeaderHistoria":0,"idCapturaHeaderHistoriaLocal":4,"idPeriodo":409819,"idPeriodoLocal":7,"idUsuario":0,"surcoFin":1,"surcoInicio":1},{"UUID":"ee681127-2aff-4d3f-bdc5-b3eec5769313","calidad":9.0,"cantidadSurcos":2,"comentarios":"","estatus":1,"fechaCaptura":"2018-01-31 11:17:47","fechaFin":"2018-01-29 13:59:00","fechaInicio":"2018-01-29 09:30:00","fechaModifico":"2018-01-31 11:17:47","idActividadPrograma":232296,"idActividadProgramaLocal":2,"idAsociado":219188,"idCapturaHeaderHistoria":0,"idCapturaHeaderHistoriaLocal":5,"idPeriodo":409819,"idPeriodoLocal":7,"idUsuario":0,"surcoFin":6,"surcoInicio":4}]",);
    }
}