/*!
 * Start Bootstrap - SB Admin 2 v3.3.7+1 (http://startbootstrap.com/template-overviews/sb-admin-2)
 * Copyright 2013-2016 Start Bootstrap
 * Licensed under MIT (https://github.com/BlackrockDigital/startbootstrap/blob/gh-pages/LICENSE)
 */
$(function () {
    // Dependent Dropdowns
    var $select1 = $('#CiudadId'),
		$select2 = $('#CodigoFacility'),
    $options = $select2.find('option');

    $select1.on('change', function () {
        $select2.html($options.filter('[value="' + this.value + '"]'));
    }).trigger('change');


    /* Desahabilitación manual */
    $("#CuentaContableId option[value=1]").attr("disabled", "disabled");
    $("#CuentaContableId option[value=731]").attr("disabled", "disabled");
    $("#CuentaContableId option[value=743]").attr("disabled", "disabled");
    $("#CuentaContableId option[value=750]").attr("disabled", "disabled");
    $("#CuentaContableId option[value=753]").attr("disabled", "disabled");

    $('#side-menu').metisMenu();

    $('#datetimepicker').datetimepicker({
        locale: 'es',
        format: 'L'
    });

    $('#datetimepicker-comprobante').datetimepicker({
        locale: 'es',
        format: 'L'
    });

    $('#datetimepicker-inicio').datetimepicker({
        locale: 'es',
        format: 'L'
    });
    $('#datetimepicker-ingreso').datetimepicker({
        locale: 'es',
        format: 'L',
        showClear: true
    });
    $('#datetimepicker-retiro').datetimepicker({
        locale: 'es',
        format: 'L',
        showClear: true
    });
    $('#datetimepicker-bajaretiro').datetimepicker({
        locale: 'es',
        format: 'L',
        showClear: true
    });
    $('#datetimepicker-fin').datetimepicker({
        locale: 'es',
        format: 'L',
        showClear: true
    });

    /* Formato de número con separador de miles*/
    $(".decimal-text").on({
        "focus": function (event) {
            $(event.target).select();
        },
        "keyup": function (event) {
            $(event.target).val(function (index, value) {
                return value.replace(/\D/g, "")
                    .replace(/([0-9])([0-9]{2})$/, '$1,$2')
                    .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, " ");
            });
        }
    });

    /* Plugin para el formato de números */
    $('.input-mask').inputmask("numeric", {
        radixPoint: ".",
        groupSeparator: " ",
        digits: 2,
        autoGroup: true,
        rightAlign: true
    });

    //$("#codigoTipoContratacion").change(function() {
    //    if ($(this).val() === "100001") {
    //        $("#fechaRetiro").removeAttr("readonly");
    //    } else {
    //        $("#fechaRetiro").val("");
    //        $("#fechaRetiro").attr("readonly", "readonly");
    //    }
    //});


    $('#datetimepicker-de').datetimepicker({
        locale: 'es',
        format: 'L',
        showClear: true
    });
    $('#datetimepicker-a').datetimepicker({
        locale: 'es',
        format: 'L',
        showClear: true
    });

    $('select#NumeroCuenta').multiselect({
        enableFiltering: true,
        maxHeight: 300,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'Buscar...'
    });

    $('select#CuentaContableId').multiselect({
        enableFiltering: true,
        maxHeight: 300,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'Buscar...'
    });

    $('select#PlanProgramaticoId').multiselect({
        enableFiltering: true,
        maxHeight: 300,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'Buscar...'
    });

    $('select#codigoEsEvaluado').multiselect({
        enableFiltering: true,
        maxHeight: 300,
        enableCaseInsensitiveFiltering: true,
        filterPlaceholder: 'Buscar...'
    });

    $("#FacturaGasolina").change(function () {
        var modal = $("#facturaModal");
        if ($(this).is(":checked")) {
            $("#FacturaGasolinaRow").removeClass("hidden");
            var importeCF = modal.find("#Importe").val() * 0.7;
            $("#ImporteCF").val(importeCF);
        } else {
            $("#FacturaGasolinaRow").addClass("hidden");
            $("#Placa").val("");
            $("#ImporteCF").val("0");
        }
    });

    $("#FacturaVentas").change(function () {
        var modal = $("#facturaModal");
        if ($(this).is(":checked")) {
            $("#FacturaVentasRow").removeClass("hidden");
        } else {
            $("#FacturaVentasRow").addClass("hidden");

        }
    });

    $(".TransferenciaRow").change(function () {
        var modal = $("#tansferenciaManutencionModal");
        var row = $(this).parent().parent();
        if ($(this).is(":checked")) {
            $(".TransferenciaRow").removeAttr("checked");
            var valorDebe = new Number(row.find(".TrDebe").text().replace(/,/g, ""));
            var valorHaber = new Number(row.find(".TrHaber").text().replace(/,/g, ""));
            if (valorDebe > 0) {
                modal.find("#Monto").val(valorDebe);
                modal.find("#AnPrograma").val("false");
            }

            if (valorHaber > 0) {
                modal.find("#Monto").val(valorHaber);
                modal.find("#AnPrograma").val("true");
            }
        } else {
            modal.find("#Monto").val("");
            modal.find("#AnPrograma").val("");
        }
    });

    /* Custom functions */
    var totalDebe = $("td.total-debe").text();
    var totalHaber = $("td.total-haber").text();
    var tableFooter = $("table.table tfoot");
    if (totalDebe == "0,00" && totalHaber == "0,00") {
        tableFooter.addClass("bg-info");
    } else {
        if (totalDebe == totalHaber) {
            tableFooter.removeClass();
            tableFooter.addClass("bg-success");
        } else {
            tableFooter.removeClass();
            tableFooter.addClass("bg-danger");
        }
    }

    $("#finalizarComprobante").click(function() {
        var finalizar = confirm('Esta seguro de finalizar el Comprobante? Posteriormente para modificar datos del comprobante,\n deberá realizar una solicitud al Administrador del Sistema.');
        if (finalizar) {
            $("#registrarFactura").hide();
            $("#adjuntarRespaldos").hide();
            $("#finalizarComprobante").hide();
            $("#panelNuevaLinea").hide();
            $("#asientosTipo").hide();
            $("#panelAsiento").removeClass("panel-success").addClass("panel-primary");
        }
    });

    /* Revisión de presupuestos */
    $(".check-recurso").change(function() {
        SetFilasSeleccionadas();
    });

    if ($("#FilasObservadas").length > 0) CheckFilasSeleccionadas();

    if ($("#tiposCambioModal").length > 0) {
        $("#tiposCambioModal").modal('show');
    }

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function () {
    $(window).bind("load resize", function () {
        var topOffset = 50;
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        var height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    // var element = $('ul.nav a').filter(function() {
    //     return this.href == url;
    // }).addClass('active').parent().parent().addClass('in').parent();
    var element = $('ul.nav a').filter(function () {
        return this.href == url;
    }).addClass('active').parent();

    while (true) {
        if (element.is('li')) {
            element = element.parent().addClass('in').parent();
        } else {
            break;
        }
    }
});

/**
 Jquery & bootstrap validation fix
 */
$(function() {
    var defaultOptions = {
        validClass: 'has-success',
        errorClass: 'has-error',
        highlight: function (element, errorClass, validClass) {
            $(element).closest(".form-group")
                .removeClass(validClass)
                .addClass('has-error');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).closest(".form-group")
            .removeClass('has-error')
            .addClass(validClass);
        }
    };
 
    $.validator.setDefaults(defaultOptions);
 
    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass,
    };
});

/* Funciones módulo de Pressupuesto */
function SetFilasSeleccionadas() {
    var filasSeleccionadas = Array();
    $("input.check-recurso:checked").each(function () {
        filasSeleccionadas.push($(this).attr("id"));
    });

    $("#FilasObservadas").val(filasSeleccionadas.toString());
}

function CheckFilasSeleccionadas() {
    var filasObservadas = $("#FilasObservadas").val();
    var filasArray = filasObservadas.split(',');

    $("#recursos tr").each(function() {
        var id = $(this).attr("id");
        if (filasArray.indexOf(id) != -1) {
            $(this).addClass("danger");
        }
    });
}
