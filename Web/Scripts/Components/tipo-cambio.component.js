/*!
 * AJAX CRUD Operations Recursos
 */
$(function () {

});


/* Funciones AJAX CRUD Operations */
function enableRowDataModal(idRow) {
    var fila = $("#" + idRow);

    fila.find(".Valor").addClass("hidden");
    fila.find(".EditarTipoCambio").addClass("hidden");

    fila.find(".ValorInput").removeClass("hidden");
    fila.find("#Valor").val(fila.find(".Valor").text());
    fila.find(".GuardarTipoCambio").removeClass("hidden");
    fila.find(".CancelarTipoCambio").removeClass("hidden");
}

function cancelEditModalData(idRow) {
    var fila = $("#" + idRow);

    fila.find(".Valor").removeClass("hidden");
    fila.find(".EditarTipoCambio").removeClass("hidden");

    fila.find(".ValorInput").addClass("hidden");
    fila.find(".GuardarTipoCambio").addClass("hidden");
    fila.find(".CancelarTipoCambio").addClass("hidden");
}


function editModalData(idRow) {
    var fila = $("#" + idRow);

    var modal = $("#tiposCambioModal");
    var tipoCambio = {
        Id: fila.find(".Id").text(),
        Valor: new Number(fila.find("#Valor").val())
    };

    $.ajax({

        url: "/Home/EditTipoCambio",
        data: JSON.stringify(tipoCambio),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateRowData(tipoCambio);
            $("#editRecursoMesModal").modal("hide");
            alert("Los cambios se guardaron correctamente");
        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateRowData(tipoCambio) {
    var id = tipoCambio.Id;
    var fila = $("#tc-" + id);

    fila.find(".Valor").text(tipoCambio.Valor);
    cancelEditModalData("tc-" + id);
}

