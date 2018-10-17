/*!
 * AJAX CRUD Operations Recursos
 */
$(function () {
    $("#EditRecursoMes").click(function () {
        if (validateData()) {
            editModalData();
        } else {
            alert("La suma de los meses debe ser igual al Monto Anual");
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
    $(".mes-presupuesto").change(function() {
        calcularSubtotalMonto();
    });
});

function distribuirRecursos(idRow) {
    var modal = $("#editRecursoMesModal");
    var coberturaTotal = new Number(modal.find("#Monto").text().replace(/\./g, "").replace(",", "."));
    var promedio = coberturaTotal / 12;

    modal.find("#Enero").val(promedio.toFixed(2));
    modal.find("#Febrero").val(promedio.toFixed(2));
    modal.find("#Marzo").val(promedio.toFixed(2));
    modal.find("#Abril").val(promedio.toFixed(2));
    modal.find("#Mayo").val(promedio.toFixed(2));
    modal.find("#Junio").val(promedio.toFixed(2));
    modal.find("#Julio").val(promedio.toFixed(2));
    modal.find("#Agosto").val(promedio.toFixed(2));
    modal.find("#Septiembre").val(promedio.toFixed(2));
    modal.find("#Octubre").val(promedio.toFixed(2));
    modal.find("#Noviembre").val(promedio.toFixed(2));
    modal.find("#Diciembre").val(promedio.toFixed(2));

    calcularSubtotalMonto();
}

function calcularSubtotalMonto() {
    var suma = 0;
    $(".mes-presupuesto").each(function () {
        suma += new Number($(this).val());
    });

    $("#Sumatoria").val(suma.toFixed(2));
}

/* Funciones AJAX CRUD Operations */
function loadRowDataIntoModal(idRow) {
    var modal = $("#editRecursoMesModal");
    var fila = $("#" + idRow);

    modal.find("#Id").val(fila.find(".Id").text());

    modal.find("#Enero").val(fila.find(".Enero").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Febrero").val(fila.find(".Febrero").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Marzo").val(fila.find(".Marzo").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Abril").val(fila.find(".Abril").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Mayo").val(fila.find(".Mayo").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Junio").val(fila.find(".Junio").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Julio").val(fila.find(".Julio").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Agosto").val(fila.find(".Agosto").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Septiembre").val(fila.find(".Septiembre").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Octubre").val(fila.find(".Octubre").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Noviembre").val(fila.find(".Noviembre").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Diciembre").val(fila.find(".Diciembre").text().replace(/\./g, "").replace(",", "."));

    modal.find("#Monto").text(fila.find(".Monto").text());
    modal.find("#Cobertura").text(fila.find(".Cobertura").text());
    modal.find("#CodigoProgramatico").text(fila.find(".CodigoProgramatico").text());
}

function validateData() {
    var modal = $("#editRecursoMesModal");
    var total = new Number(modal.find("#Monto").text().replace(/\./g, "").replace(",", "."));

    var enero = new Number(modal.find("#Enero").val());
    var febrero = new Number(modal.find("#Febrero").val());
    var marzo = new Number(modal.find("#Marzo").val());
    var abril = new Number(modal.find("#Abril").val());
    var mayo = new Number(modal.find("#Mayo").val());
    var junio = new Number(modal.find("#Junio").val());
    var julio = new Number(modal.find("#Julio").val());
    var agosto = new Number(modal.find("#Agosto").val());
    var septiembre = new Number(modal.find("#Septiembre").val());
    var octubre = new Number(modal.find("#Octubre").val());
    var noviembre = new Number(modal.find("#Noviembre").val());
    var diciembre = new Number(modal.find("#Diciembre").val());
    var suma = (enero + febrero + marzo + abril + mayo + junio + julio + agosto + septiembre + octubre + noviembre + diciembre).toFixed(2);
    return suma == total;
}

function editModalData() {
    var modal = $("#editRecursoMesModal");
    var recursoMes = {
        Id: modal.find("#Id").val(),
        Enero: new Number(modal.find("#Enero").val()),
        Febrero: new Number(modal.find("#Febrero").val()),
        Marzo: new Number(modal.find("#Marzo").val()),
        Abril: new Number(modal.find("#Abril").val()),
        Mayo: new Number(modal.find("#Mayo").val()),
        Junio: new Number(modal.find("#Junio").val()),
        Julio: new Number(modal.find("#Julio").val()),
        Agosto: new Number(modal.find("#Agosto").val()),
        Septiembre: new Number(modal.find("#Septiembre").val()),
        Octubre: new Number(modal.find("#Octubre").val()),
        Noviembre: new Number(modal.find("#Noviembre").val()),
        Diciembre: new Number(modal.find("#Diciembre").val()),
    };

    $.ajax({

        url: "/RecursosMeses/Edit",
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

    fila.find(".Enero").text(recursoMes.Enero.toFixed(2).replace(".", ","));
    fila.find(".Febrero").text(recursoMes.Febrero.toFixed(2).replace(".", ","));
    fila.find(".Marzo").text(recursoMes.Marzo.toFixed(2).replace(".", ","));
    fila.find(".Abril").text(recursoMes.Abril.toFixed(2).replace(".", ","));
    fila.find(".Mayo").text(recursoMes.Mayo.toFixed(2).replace(".", ","));
    fila.find(".Junio").text(recursoMes.Junio.toFixed(2).replace(".", ","));
    fila.find(".Julio").text(recursoMes.Julio.toFixed(2).replace(".", ","));
    fila.find(".Agosto").text(recursoMes.Agosto.toFixed(2).replace(".", ","));
    fila.find(".Septiembre").text(recursoMes.Septiembre.toFixed(2).replace(".", ","));
    fila.find(".Octubre").text(recursoMes.Octubre.toFixed(2).replace(".", ","));
    fila.find(".Noviembre").text(recursoMes.Noviembre.toFixed(2).replace(".", ","));
    fila.find(".Diciembre").text(recursoMes.Diciembre.toFixed(2).replace(".", ","));

}

