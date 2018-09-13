<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ejecucion.aspx.cs" Inherits="TablaPivot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
        <!--[if lt IE 9]>
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
        <![endif]-->
    <!--[if gte IE 9]><!-->
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
    <!--<![endif]-->
    <script src="http://cdnjs.cloudflare.com/ajax/libs/knockout/3.4.0/knockout-min.js"></script>
    <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/globalize/0.1.1/globalize.min.js"></script>
    <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.chartjs.js"></script>
    <script type="text/javascript" src="http://cdn3.devexpress.com/jslib/15.2.10/js/dx.webappjs.js"></script>
    <script type="text/javascript" src="Scripts/jszip.js"></script>
    <script type="text/javascript" src="Scripts/jszip.min.js"></script>

    <link rel="stylesheet" type="text/css" href="http://cdn3.devexpress.com/jslib/15.2.10/css/dx.common.css" />
    <link rel="stylesheet" type="text/css" href="http://cdn3.devexpress.com/jslib/15.2.10/css/dx.light.css" />
    <title></title>
     
    <script type="text/javascript">
            
        var Aceptable = parent.document.getElementById("Aceptable").value;
         
        var minimo = (100 - Aceptable);///100;
        var maximo = ((100*1)+(Aceptable*1));///100;
       // alert(minimo+'---'+maximo)
        /////---------------------------------------------------------------------------------------------------------------
        $(function(){
           

            $("#gridContainer").dxDataGrid({
                dataSource: <%=Datax%>,
                showRowLines: true,
                rowAlternationEnabled: true,
                allowColumnReordering: true,
                selection: {
                    mode: "multiple"
                },
                paging: {
                    pageSize: 100
                },
                filterRow: {
                    visible: true,
                    applyFilter: "auto"
                },
                headerFilter: {
                    visible: true
                },
                "export": {
                    enabled: true,
                    fileName: "EjecucionYCumplimiento",
                    allowExportSelectedData: true
                },
                groupPanel: {
                    visible: true
                },
                columnAutoWidth:true,
                grouping: {
                    autoExpandAll: false
                },
                onCellPrepared: function (info) {
                    if(info.column.dataField==='Cumplimiento'){
                        if(info.value>=minimo && info.value<=maximo){
                            info.cellElement.addClass('siPasa');
                        }
                        else{
                            info.cellElement.addClass('noPasa');
                        }
                    } 
                    if(info.column.dataField==='E_Surcos'){
                        if(info.value>=minimo && info.value<=maximo){
                            info.cellElement.addClass('siPasa');
                        }else{
                            info.cellElement.addClass('noPasa');
                        }
                    }
                    if(info.column.dataField==='E_Asociados'){
                        if(info.value>=minimo && info.value<=maximo){
                            info.cellElement.addClass('siPasa');
                        }else{
                            info.cellElement.addClass('noPasa');
                        }
                    }
                    if(info.column.dataField==='E_Tiempo'){
                        if(info.value>=minimo && info.value<=maximo){
                            info.cellElement.addClass('siPasa');
                        }else{
                            info.cellElement.addClass('noPasa');
                        }
                    }
                    if(info.column.dataField==='Ejecucion'){
                        if(info.value>=minimo && info.value<=maximo){
                            info.cellElement.addClass('siPasa');
                        }else{
                            info.cellElement.addClass('noPasa');
                        }
                    }
                },
                columns: [
                     { dataField: 'Pais', groupIndex: 0 }
                    ,{ dataField: 'Planta', groupIndex: 1 }
                    ,{ dataField: 'Gerente'     , groupIndex: 2 }
                    ,{ dataField: "Lider"       , groupIndex: 3 }
                    ,{ dataField: "Zona"        , groupIndex: 4 }
                    ,{ dataField: "Invernadero" , groupIndex: 5 }
                    ,{ dataField: "Habilidad"   , Datatype:"string"}
                    //,{ caption: "PROGRAMACION", columns: [
                    ,{ caption:"Tiempo", dataField: "MinutosProg",  visible: false }
                    ,{ caption:"Surcos Prog"
                        , dataField: "SurcosProgramados"
                        , visible: true
                        ,alignment: "right"}
                    ,{ dataField: "TotalSurcoActividad", caption:"Surcos Captura", Datatype:"number"}
                    ,
                      ////////
                      //{ columns: [
                        { caption:"Surcos",//format: { type: 'percent', precision: 1 } ,
                        calculateCellValue: function(data){
                            var res = Math.abs((data.TotalSurcoActividad*100)/data.SurcosProgramados).toFixed(2);
                            if (res > 100){
                                res = res - (res - 100);
                            }
                            return res;
                        }
                            ,alignment: "right"
                        ,dataField: "E_Surcos"
                    }
                    ,{ caption:"Asociados Prog"
                        , dataField: "AsociadosProg"
                        , visible: true
                        , alignment: "right"}
                //]
                  //  }
                    ,{ dataField: "MinutosCap", caption:"Tiempo Captura", Datatype:"number", visible: false }
                    
                    ,{ dataField: "AsociadosReal", caption:"Asociados Captura", Datatype:"number"}
                    //]}
                    
                    ,{ caption:"Asociados"
                        , 
                        calculateCellValue: function(data){
                            var res = Math.abs((data.AsociadosReal*100)/data.AsociadosProg).toFixed(2);
                            if(res > 100){
                                res = 100 - (res-100);
                            }
                            return res;
                        }
                        ,alignment: "right"
                        ,dataField: "E_Asociados"
                    }
                    ,{ caption:"Tiempo",dataField: "E_Tiempo"
                        ,
                        calculateCellValue: function(data){
                            return Math.abs((data.MinutosCap*100)/data.MinutosProg).toFixed(2);
                        }
                        ,alignment: "right"
                   // }],caption: "EJECUCION"
            }
                        
                     ///
                    ,
                    { dataField: 'idInvernadero', visible: false }
                     ,{ dataField: "Cumplimiento",
                         calculateCellValue: function(data){
                             if(data.TotalSurcoActividad==0) 
                             {
                                 return 0;
                             }
                             else if(data.TotalSurcoActividad>=data.SurcosProgramados){
                                 return 100;
                             }else{
                                 var res = Math.abs((data.TotalSurcoActividad*100)/data.SurcosProgramados).toFixed(2);
                                if (res > 100){
                                    res = 100;
                                }
                                return res;
                                 //return Math.abs((data.TotalSurcoActividad*100)/data.SurcosProgramados).toFixed(2);
                             }
                         }
                         ,alignment: "right"
                     }
                    ,{ dataField: "Ejecucion"
                       ,alignment: "right"
                           , calculateCellValue: function(data){
                                var ESurcos = (data.TotalSurcoActividad*100)/data.SurcosProgramados;
                                if (ESurcos > 100){
                                    ESurcos = 100 - (ESurcos-100);
                                }
                                var EAsociados = (data.AsociadosReal*100)/data.AsociadosProg;
                                if (EAsociados > 100){
                                    EAsociados = 100 - (EAsociados-100);
                                }
                                var EMinutos = (data.MinutosCap*100)/data.MinutosProg;
                                if (EMinutos > 100){
                                    EMinutos = 100 - (EMinutos-100);
                                }
                                var correcto =0;
                                if(ESurcos >= minimo  && ESurcos <= maximo){
                                    correcto++;
                                }
                                if(EAsociados >= minimo  && EAsociados <= maximo){
                                    correcto++;
                                }
                                if(EMinutos >= minimo  && EMinutos <= maximo){
                                    correcto++;
                                }
                                return   Math.abs((correcto*100/3)).toFixed(2);
                            }
                    }
                ],
                summary: {
                    groupItems: [
                        {
                        column: "Cumplimiento",
                        summaryType: "avg",
                        valueFormat: 'decimal',
                        precision: 2,
                        displayFormat: "{0}% Cumplimiento"
                        },
                        {
                            column: "Ejecucion",
                            summaryType: "avg",
                            valueFormat: 'decimal',
                            precision: 2,
                            displayFormat: "{0}% Ejecucion"
                        }
                    ]}
            });
            
        });
            
        function calcularTiempoDosFechas(date1, date2){
            start_actual_time = new Date(date1);
            end_actual_time = new Date(date2);

            var diff = end_actual_time - start_actual_time;

            var diffSeconds = diff/1000;
            //var HH = Math.floor(diffSeconds/3600);
            //var MM = Math.floor(diffSeconds%3600)/60;

           // var formatted = ((HH < 10)?("0" + HH):HH) + ":" + ((MM < 10)?("0" + MM):MM)
            return diffSeconds;
        }
        </script>
    <style>
           .pivotgrid-demo .plantacss {
            color: #bfae6a;
            font-weight: bold;
        }

        #pivotgrid-demo > .dx-button {
            margin: 10px 0;
        }

        #pivotgrid-demo .desc-container a {
            color: #f05b41;
            text-decoration: underline;
            cursor: pointer;
        }

            #pivotgrid-demo .desc-container a:hover {
                text-decoration: none;
            }
            .noPasa {
    color: #FF4500;
}
            .siPasa {
    color: #32CD32;
}
    </style>
</head>
<body>
    <br />
    <div id="applyCustomFilter"></div>
    <div id="gridContainer"></div>
    
    <br />
</body>
</html>
