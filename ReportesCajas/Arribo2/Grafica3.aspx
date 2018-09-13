<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Grafica3.aspx.cs" Inherits="Grafica3" %>

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
    <script type="text/javascript">
        /////---------------------------------------------------------------------------------------------------------------
        var dataSource = <%=convertido%>;
     
        $(function () {//////////////////////////////////////////////////
            $("#Container").dxDataGrid({
                dataSource: dataSource,
                selection: {
                    mode: "multiple"
                },
                filterRow: { 
                    visible: true 
                },
                "export": {
                    enabled: true,
                    fileName: "Arribo",
                    allowExportSelectedData: true
                },
                groupPanel: {
                    visible: true
                },
                columns: [
                         "folio"
                        ,"Planta"
                        ,"fechaInicio"
	                    ,"claveInvernadero"
	                    ,"Cajas_Cosecha"
	                    ,"Cajas_Entrada_Empaque"
	                    ,"P_total"
	                    ,"P_pack"
                ]
            });
            ///////////////////////////////////////////////////////
        });
    </script>
      <style>
        
        .ajuste {
            float: left;
            margin: 10px;
            padding: 10px;
            width: 500px;
            height: 600px;
            border: 1px solid black;
        }</style>
</head>
<body>
     <div id="Container"></div>
</body>
</html>
