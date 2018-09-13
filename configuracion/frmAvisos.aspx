<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmAvisos.aspx.cs" Inherits="configuracion_frmAvisos" MasterPageFile="~/MasterPage.master" 
    ValidateRequest="false" EnableEventValidation="false"  MaintainScrollPositionOnPostback="true"
%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/controls/popUpMessageControl.ascx" TagName="popUpMessageControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- PAginador con busqueda rapida--%>
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <link href="../Styles/jquery.dataTables.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery.dataTables.js" type="text/javascript"></script>
   <%-- <script type="text/javascript">
        var j = jQuery.noConflict();
        j(function () {
            var lang = '<%=this.Session["Locale"]%>';
            if (lang != 'es-MX' && lang != 'en-US')
                lang = 'es-MX';
            j("#<%=grViewPendings.ClientID%>").dataTable({
                language: {
                    url: '../Scripts/' + lang + '.json'
                }
            });
        });
    </script>--%>
    <link href="../comun/css/chosen.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../comun/scripts/jquery.blockUI.js"></script>
	<style type="text/css">
        .style1
        {
            width: 120px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        
    </div>
</asp:Content>
