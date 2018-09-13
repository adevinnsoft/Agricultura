<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="frmCorteMedioDia.aspx.cs" Inherits="Reportes_frmCorteMedioDia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../comun/Scripts/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../comun/scripts/jquery.ui.datepicker.js"></script>
    <script src="../comun/scripts/jsPopUp.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            semanaActual = parseInt($('[id*="ltSemana"]').text());
            $('#txtSemanaCalendario').val(semanaActual);
            $('#txtAnioCalendario').val(new Date().getFullYear());

            var today = new Date();
            var lastYear = new Date(today.getFullYear() - 1, today.getMonth(), today.getDate());
            $('.datePicker').datepicker('destroy').datepicker({
                dateFormat: 'yy/mm/dd',
                minDate: lastYear, //'2015/11/28',
                maxDate: today
            });
            //ObtenerReportePreharvest();
        });

        function semanaAnterior() {
            var semana = parseInt($('#txtSemanaCalendario').val());
            var anio = parseInt($('#txtAnioCalendario').val());
            maxAnioAnterior = 52;
            if (semana == 1) {
                semana = maxAnioAnterior;
                anio = parseInt($('#txtAnioCalendario').val()) - 1;
                $('#txtSemanaCalendario').val(semana);
                $('#txtAnioCalendario').val(anio);
            }
            else {
                semana = parseInt(semana) - 1;
                $('#txtSemanaCalendario').val(semana);
            }

            $('#calendar').fullCalendar('prev');
            ObtenerReportePreharvest();
        }

        function semanaSiguiente() {
            var semana = $('#txtSemanaCalendario').val();
            var anio = $('#txtAnioCalendario').val();
            maxAnioActual = 53;
            maxAnioSiguiente = 52;
            if (semana == maxAnioActual) {
                semana = 1;
                anio = parseInt($('#txtAnioCalendario').val()) + 1;
                $('#txtSemanaCalendario').val(1);
                $('#txtAnioCalendario').val(anio);
            }
            else {
                semana = parseInt(semana) + 1;
                $('#txtSemanaCalendario').val(semana);
            }

            $('#calendar').fullCalendar('next');
            ObtenerReportePreharvest();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <h1>Corte 1/2 D&iacutea</h1>

        <table id="SelectorSemana" class="index">
            <tr>
                <td align="center" style="text-align: center;">
                    <img src="../comun/img/prev.png" style="float: none;" onclick="semanaAnterior();" />
                    <input id="txtSemanaCalendario" type="text" style="float: none; width: 60px; text-align: center;"
                        readonly />
                    <input id="txtAnioCalendario" type="text" style="float: none; width: 60px; text-align: center;"
                        readonly />
                    <img src="../comun/img/next.png" style="float: none;" onclick="semanaSiguiente();" />
                </td>
                <td>
                    <input type="text" id="txtDia" style="text-align: center;" class="datePicker" />
                </td>
            </tr>
        </table><br />

        <table id="tbl_CorteMedioDia" class="gridView">
            <thead>
                <tr>
                    <th>Invernadero </th>
                    <th>Proyectado</th>
                    <th>Estimado</th>
                    <th>Cierre</th>
                    <th>Merma</th>
                    <th>Dif. Estimado <br /> VS Cierre</th>
                    <th>Dif. Proyectado <br /> VS Cierre</th>
                    <th>Causas Merma</th>
                </tr>
            </thead>
        </table>
    </div>
</asp:Content>

