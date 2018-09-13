<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Default" EnableEventValidation = "false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <meta charset="utf-8" />
    <title>Asociados</title>
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
    <link rel="stylesheet" href="StyleSheet.css">
    <script type="text/javascript">
        function fecha_TextChanged() {
            var date = document.getElementById("fecha").textContent;
            alert(date);
        }

        $(function(){
            $("#dateInicio").dxDateBox({
                format: "date",
                onValueChanged: function(data) {
                    var week = new Date(data.value).getWeek();
                    alert(week);
                }
            });
        });

        ///////////////////////////////////////////////
        //function write_to_excel() {
        //    str = "";
        //    var mytable = document.getElementById("tbExport");
        //    var rowCount = mytable.rows.length;
        //    var colCount = mytable.getElementsByTagName("tr")[0].getElementsByTagName("th").length;
        //    var ExcelApp = new ActiveXObject("Excel.Application");
        //    var ExcelSheet = new ActiveXObject("Excel.Sheet");
        //    //ExcelSheet.Application.Visible = true;
        //    for (var i = 0; i < rowCount; i++) {
        //        for (var j = 0; j < colCount; j++) {
        //            if (i == 0) {
        //                str = mytable.getElementsByTagName("tr")[i].getElementsByTagName("th")[j].innerText;
        //            }
        //            else {
        //                str = mytable.getElementsByTagName("tr")[i].getElementsByTagName("td")[j].innerText;
        //            }
        //            ExcelSheet.ActiveSheet.Cells(i + 1, j + 1).Value = str;
        //        }
        //    }
        //    ExcelSheet.autofit;
        //    ExcelSheet.Application.Visible = true;
        //    DisplayAlerts = true;
        //    CollectGarbage();
        //}
    </script>
       <script type="text/javascript">

           /////---------------------------------------------------------------------------------------------------------------
           var sales = <%=Datax%>;
            
           $(function () {
               var pivotgrid = $("#pivotgrid").dxPivotGrid({
                   allowSortingBySummary: true,
                   allowSorting: true,
                   allowFiltering: true,
                   allowExpandAll: true,
                   showBorders: true,
                   height: 900,
                   showRowTotals:false,
                   showRowGrandTotals: false,
                   showGrandTotals: false,
                   fieldChooser: {enabled: false},
                   "export": {
                       enabled: false,
                       fileName: "Asociados_Actividad"
                   },
                   dataSource: {
                       fields: [ 
                           {
                               dataField: "Codigo",
                               area: "row",
                               width:60,
                               expanded: true
                           },
                        {
                            dataField: "Asociado",
                            area: "row",
                            width:300
                        },
                         {
                             dataField: "RowAsociado",
                             area: "column",
                         },
                         {
                             caption: "Pasillo",
                             dataField: "surco",
                             dataType: "number",
                             summaryType: "sum",
                             format: "number",
                             area: "data",
                             //showTotals: false
                             showRowGrandTotals: false,
                             showGrandTotals: false,
                         },
                        {
                            caption: "Cajas",
                            dataField: "cajas",
                            dataType: "number",
                            summaryType: "sum",
                            format: "number",
                            area: "data",
                         
                        }],
                       store: sales
                   }
               }).dxPivotGrid("instance");

               $("#reset").dxButton({
                   text: "Reset Tabla",
                   onClick: function() {
                       pivotgrid.getDataSource().state({});
                   }
               });

           });


           //$(function(){
           //    var tablaPivot = document.getElementsByClassName('dx-pivotgrid-border');
           //    var rowCount = tablaPivot.rows.length;
           //    alert(rowCount);
           //}
           //);
    </script>
 
</head>
<body style="width:1310px;   margin:0px auto;  ">
    <div id="contenido" runat="server"  style=" " >
    <form id="form1" runat="server" style=" height:50%;"  >
        <div style="width:200px; position:relative; float:left;">
            <img alt="" src="../../comun/img/bonanza.png" width="200px"  style="align-self:flex-start" align="left" />
           
        </div>
        
        <div style="width:1100px; position:relative; float:left;" >
            <div style="width:100%; background-color:white; text-align:center; float:left; ">
                <div id="parametros"  style="width:85%;  position:relative;  float:left;" runat="server">

                    <div style="width:100%; float:left; margin-top:30px;">

                        <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                            Rancho:
                                </label>
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
                   
                             <asp:DropDownList
                                             ID="DropDrownPlanta" runat="server" 
                                             OnSelectedIndexChanged="DropDrownPlanta_SelectedIndexChanged"
                                             AutoPostBack="true">
                            </asp:DropDownList>
                        
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                            Dia:
                                </label>
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
                            <asp:TextBox ID="fecha" TextMode="Date" runat="server" Width="100%" OnTextChanged="fecha_TextChanged" AutoPostBack="true"></asp:TextBox>
                             
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                            Invernadero:
                                </label>
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
                     
                            <asp:DropDownList ID="DropDownListInvernadero" OnSelectedIndexChanged="DropDownListInvernadero_SelectedIndexChanged" runat="server" AutoPostBack="true"></asp:DropDownList>
                         
                        </div>


                    </div>

                     <div style="width:100%; float:left; margin-top:20px;">

                        <div  style="width:25%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                            Lider de cosecha:
                                </label>
                        </div>
                        <div  style="width:40%;  position:relative;  float:left;">
                   
                             <asp:DropDownList
                                             ID="DropDownListLider" runat="server" 
                                             
                                             Width="100%">
                            </asp:DropDownList>
                        
                        </div>
                        
                    </div>

                     <div style="width:100%; float:left; margin-top:20px;">

                        <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                            Coordinador:
                                </label>
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
      
                        </div>
                         <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                             Semana:
                                </label>
                              
                        </div>
                        <div  style="width:5%;  position:relative;  float:left; border-bottom:medium;">
                          <asp:Label ID="SemanaLabel" runat="server"></asp:Label>
                        </div>
                        <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none;" >
                            Variedad:
                                </label>
                        </div>
                        <div  style="width:10%;  position:relative;  float:left; border-bottom:medium;">
                            <asp:Label ID="variedad" runat="server"></asp:Label>
                        </div>
                         <div  style="width:15%;  position:relative;  float:left;">
                            <label class="label dx-field-label" style="position:relative;  float:none; font-style:oblique;" >
                            Total Cajas:
                                </label>
                        </div>
                        <div  style="width:15%;  position:relative;  float:left; border-bottom:medium;">
                            <asp:Label ID="tCajas" runat="server" Font-Bold="true"></asp:Label>
                        </div>
                    </div>
                    
                </div>
               <div id="datosx"    style="width:15%;  position:relative;  float:left;">
                   
                </div>
            </div>
        </div>
        <div>

        </div>
         <div style="width:200px; position:relative; float:left;">
             <asp:Button ID="Button1" Text="Descargar" runat="server"  CssClass="button-0" OnClick="Button2_Click" Enabled="true" Visible="false" />
             <asp:Button ID="Generar" Text="Generar" runat="server" CssClass="button-0" OnClick="Generar_Click" Enabled="true" />
            </div>
        <div>
            <asp:Label ID="msj" runat="server"></asp:Label>
        </div>
        <div style="width:100%; height:auto; position:relative; float:left; overflow:hidden; ">
             <div id="pivotgrid-demo">
            <div id="pivotgrid" runat="server">
            </div>
            </div>
            <%--<iframe src="" name="TablaPivot" id="TablaPivot"  height="100%" width="100%" runat="server" frameborder="0"></iframe>--%>
 
        </div>
        
    </form></div>
</body>
</html>

