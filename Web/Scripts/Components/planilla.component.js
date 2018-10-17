/*!
 * AJAX CRUD Operations Planillas
 */
$(function () {
    /* Planillas */
    $("#EditPlanilla").click(function () {
        editModalData();
        return false;
    });

    $("#CloseEditModal").click(function () {
        $("#planillaModal").modal("hide");
    });

    $("CloseImportarModal").click(function () {
        $("#importarModal").modal("hide");
    });
});


/* Funciones AJAX CRUD Operations */
function loadRowDataIntoModal(idRow) {
    var modal = $("#planillaModal");
    var fila = $("#" + idRow);

    var item = fila.find(".ITEM").text();
    var nombre = fila.find(".NOMBRE").text();
    modal.find(".modal-title").text("Modificar Planilla para el Item " + item + " - " + nombre);
    modal.find("#ITEM").val(item);

    modal.find("#MOD_RC_IVA").val(fila.find(".MOD_RC_IVA").text().replace(",", "."));
    modal.find("#MOD_COMISION").val(fila.find(".MOD_COMISION").text().replace(",", "."));
    modal.find("#MOD_AMIGOSOS").val(fila.find(".MOD_AMIGOSOS").text().replace(",", "."));

    modal.find("#MOD_PRESTAMOS").val(fila.find(".MOD_PRESTAMOS").text().replace(",", "."));
    modal.find("#MOD_OTRS_DESCUENTOS").val(fila.find(".MOD_OTRS_DESCUENTOS").text().replace(",", "."));
    modal.find("#MOD_OTROS_INGRESOS").val(fila.find(".MOD_OTROS_INGRESOS").text().replace(",", "."));
}

function editModalData() {
    var modal = $("#planillaModal");
    var planilla = {
        ITEM: modal.find("#ITEM").val(),
        MES: modal.find("#MES").val(),
        GESTION: modal.find("#GESTION").val(),
        MOD_RC_IVA: new Number(modal.find("#MOD_RC_IVA").val()),
        MOD_COMISION: new Number(modal.find("#MOD_COMISION").val()),
        MOD_AMIGOSOS: modal.find("#MOD_AMIGOSOS").val(),

        MOD_PRESTAMOS: modal.find("#MOD_PRESTAMOS").val(),
        MOD_OTRS_DESCUENTOS: modal.find("#MOD_OTRS_DESCUENTOS").val(),
        MOD_OTROS_INGRESOS: modal.find("#MOD_OTROS_INGRESOS").val()
    };

    $.ajax({

        url: "/Planillas/Edit",
        data: JSON.stringify(planilla),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateRowData(planilla);
            $("#planillaModal").modal("hide");
            //alert("Los cambios se guardaron correctamente");
        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateRowData(planilla) {
    var item = planilla.ITEM;
    var fila = $("#item-" + item);

    fila.find(".MOD_RC_IVA").text(planilla.MOD_RC_IVA.toFixed(2));
    fila.find(".MOD_COMISION").text(planilla.MOD_COMISION.toFixed(2));
    fila.find(".MOD_AMIGOSOS").text(planilla.MOD_AMIGOSOS);

    fila.find(".MOD_PRESTAMOS").text(planilla.MOD_PRESTAMOS);
    fila.find(".MOD_OTRS_DESCUENTOS").text(planilla.MOD_OTRS_DESCUENTOS);
    fila.find(".MOD_OTROS_INGRESOS").text(planilla.MOD_OTROS_INGRESOS);
}

