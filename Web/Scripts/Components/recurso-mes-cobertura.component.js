/*!
 * AJAX CRUD Operations Recursos
 */
$(function () {
    $("#EditRecursoMes").click(function () {
        if (validateData()) {
            editModalData();
        } else {
            alert("La suma de los meses debe ser igual a la Cobertura Anual");
        }
        return false;
    });

    $("#CloseEditRecursoMesModal").click(function () {
        $("#editRecursoMesModal").modal("hide");
    });

    $("#DistribuirRecursoMesModal").click(function () {
        distribuirRecursos();
    });

    //Eventos para sumatoria de meses
    $(".mes-cobertura").change(function () {
        calcularSubtotalCobertura();
    });
});

function distribuirRecursos(idRow) {
    var modal = $("#editRecursoMesModal");
    var coberturaTotal = new Number(modal.find("#Cobertura").text());
    var promedio = parseInt(coberturaTotal / 12);
    var restante = coberturaTotal > (promedio * 12) ? coberturaTotal - (promedio * 12) : 0;

    modal.find("#CoberturaEnero").val(promedio);
    modal.find("#CoberturaFebrero").val(promedio);
    modal.find("#CoberturaMarzo").val(promedio);
    modal.find("#CoberturaAbril").val(promedio);
    modal.find("#CoberturaMayo").val(promedio);
    modal.find("#CoberturaJunio").val(promedio);
    modal.find("#CoberturaJulio").val(promedio);
    modal.find("#CoberturaAgosto").val(promedio);
    modal.find("#CoberturaSeptiembre").val(promedio);
    modal.find("#CoberturaOctubre").val(promedio);
    modal.find("#CoberturaNoviembre").val(promedio);
    modal.find("#CoberturaDiciembre").val(promedio + restante);

    calcularSubtotalCobertura();
}

function calcularSubtotalCobertura() {
    var suma = 0;
    $(".mes-cobertura").each(function () {
        suma += new Number($(this).val());
    });

    $("#Sumatoria").val(suma);
}

/* Funciones AJAX CRUD Operations */
function loadRowDataIntoModal(idRow) {
    var modal = $("#editRecursoMesModal");
    var fila = $("#" + idRow);

    modal.find("#Id").val(fila.find(".Id").text());

    modal.find("#CoberturaEnero").val(fila.find(".CoberturaEnero").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaFebrero").val(fila.find(".CoberturaFebrero").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaMarzo").val(fila.find(".CoberturaMarzo").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaAbril").val(fila.find(".CoberturaAbril").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaMayo").val(fila.find(".CoberturaMayo").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaJunio").val(fila.find(".CoberturaJunio").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaJulio").val(fila.find(".CoberturaJulio").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaAgosto").val(fila.find(".CoberturaAgosto").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaSeptiembre").val(fila.find(".CoberturaSeptiembre").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaOctubre").val(fila.find(".CoberturaOctubre").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaNoviembre").val(fila.find(".CoberturaNoviembre").text().replace(/\./g, "").replace(",", "."));
    modal.find("#CoberturaDiciembre").val(fila.find(".CoberturaDiciembre").text().replace(/\./g, "").replace(",", "."));

    modal.find("#Monto").text(fila.find(".Monto").text());
    modal.find("#Cobertura").text(fila.find(".Cobertura").text());
    modal.find("#CodigoProgramatico").text(fila.find(".CodigoProgramatico").text());
}

function validateData() {
    var modal = $("#editRecursoMesModal");
    var total = new Number(modal.find("#Cobertura").text().replace(/\./g, "").replace(",", "."));

    var enero = new Number(modal.find("#CoberturaEnero").val());
    var febrero = new Number(modal.find("#CoberturaFebrero").val());
    var marzo = new Number(modal.find("#CoberturaMarzo").val());
    var abril = new Number(modal.find("#CoberturaAbril").val());
    var mayo = new Number(modal.find("#CoberturaMayo").val());
    var junio = new Number(modal.find("#CoberturaJunio").val());
    var julio = new Number(modal.find("#CoberturaJulio").val());
    var agosto = new Number(modal.find("#CoberturaAgosto").val());
    var septiembre = new Number(modal.find("#CoberturaSeptiembre").val());
    var octubre = new Number(modal.find("#CoberturaOctubre").val());
    var noviembre = new Number(modal.find("#CoberturaNoviembre").val());
    var diciembre = new Number(modal.find("#CoberturaDiciembre").val());
    var suma = enero + febrero + marzo + abril + mayo + junio + julio + agosto + septiembre + octubre + noviembre + diciembre;
    return suma == total;
}

function editModalData() {
    var modal = $("#editRecursoMesModal");
    var recursoMes = {
        Id: modal.find("#Id").val(),
        CoberturaEnero: new Number(modal.find("#CoberturaEnero").val()),
        CoberturaFebrero: new Number(modal.find("#CoberturaFebrero").val()),
        CoberturaMarzo: new Number(modal.find("#CoberturaMarzo").val()),
        CoberturaAbril: new Number(modal.find("#CoberturaAbril").val()),
        CoberturaMayo: new Number(modal.find("#CoberturaMayo").val()),
        CoberturaJunio: new Number(modal.find("#CoberturaJunio").val()),
        CoberturaJulio: new Number(modal.find("#CoberturaJulio").val()),
        CoberturaAgosto: new Number(modal.find("#CoberturaAgosto").val()),
        CoberturaSeptiembre: new Number(modal.find("#CoberturaSeptiembre").val()),
        CoberturaOctubre: new Number(modal.find("#CoberturaOctubre").val()),
        CoberturaNoviembre: new Number(modal.find("#CoberturaNoviembre").val()),
        CoberturaDiciembre: new Number(modal.find("#CoberturaDiciembre").val()),
    };

    $.ajax({

        url: "/RecursosMeses/EditCobertura",
        data: JSON.stringify(recursoMes),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateRowData(recursoMes);
            $("#editRecursoMesModal").modal("hide");
            alert("Los cambios se guardaron correctamente");
        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateRowData(recursoMes) {
    var id = recursoMes.Id;
    var fila = $("#recurso-" + id);

    fila.find(".CoberturaEnero").text(recursoMes.CoberturaEnero);
    fila.find(".CoberturaFebrero").text(recursoMes.CoberturaFebrero);
    fila.find(".CoberturaMarzo").text(recursoMes.CoberturaMarzo);
    fila.find(".CoberturaAbril").text(recursoMes.CoberturaAbril);
    fila.find(".CoberturaMayo").text(recursoMes.CoberturaMayo);
    fila.find(".CoberturaJunio").text(recursoMes.CoberturaJunio);
    fila.find(".CoberturaJulio").text(recursoMes.CoberturaJulio);
    fila.find(".CoberturaAgosto").text(recursoMes.CoberturaAgosto);
    fila.find(".CoberturaSeptiembre").text(recursoMes.CoberturaSeptiembre);
    fila.find(".CoberturaOctubre").text(recursoMes.CoberturaOctubre);
    fila.find(".CoberturaNoviembre").text(recursoMes.CoberturaNoviembre);
    fila.find(".CoberturaDiciembre").text(recursoMes.CoberturaDiciembre);

}

