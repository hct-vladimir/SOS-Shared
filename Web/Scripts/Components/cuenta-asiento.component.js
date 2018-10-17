/*!
 * AJAX CRUD Operations CuentaAsientos
 */
$(function () {
    $("#AddCuentaAsiento").click(function () {
        cleanDecimals();
    });

    $("#EditCuentaAsiento").click(function () {
        editModalData();
        return false;
    });

    $("#CloseAddCuentaAsientoModal").click(function () {
        $("#addCuentaAsientoModal").modal("hide");
    });

    $("#CloseEditCuentaAsientoModal").click(function () {
        $("#editCuentaAsientoModal").modal("hide");
    });

    $("#CloseAddPlantillaAsientoModal").click(function () {
        $("#addPlantillaAsientoModal").modal("hide");
    });
});

function cleanDecimals() {
    var modal = $("#addCuentaAsientoModal");
    var montoReal = modal.find("#Monto").val().replace(".", ",");
    modal.find("#Monto").val(montoReal);
}

/* Funciones AJAX CRUD Operations */
function loadRowDataIntoModal(idRow) {
    var modal = $("#editCuentaAsientoModal");
    var fila = $("#" + idRow);


    modal.find("#PlanProgramaticoId").multiselect("select", fila.find(".PlanProgramaticoId").text());
    modal.find("#CuentaContableId").multiselect("select", fila.find(".CuentaContableId").text());

    modal.find("#CiudadCodigo").val(fila.find(".CiudadCodigo").text());
    modal.find("#TerritorioId").val(fila.find(".TerritorioId").text());
    modal.find("#ContraparteId").val(fila.find(".ContraparteId").text());
    modal.find("#CodigoAuditoriaId").val(fila.find(".CodigoAuditoriaId").text());
    modal.find("#AccionNacionalId").val(fila.find(".AccionNacionalId").text());
    modal.find("#AnexoTributarioId").val(fila.find(".AnexoTributarioId").text());
    modal.find("#MarcoLogicoId").val(fila.find(".MarcoLogicoId").text());

    modal.find("#Id").val(fila.find(".Id").text());
    modal.find("#Descripcion").val(fila.find(".Descripcion").text());
    modal.find("#NotasAdicionales").val(fila.find(".NotasAdicionales").text());
    modal.find("#Monto").val(fila.find(".Monto").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Glosa").val(fila.find(".Glosa").text());
    modal.find("#Debe").val(fila.find(".Debe").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Haber").val(fila.find(".Haber").text().replace(/\./g, "").replace(",", "."));

    // Códigos para generar el Código Programático
    modal.find("#PlanProgramaticoCodigo").val(fila.find(".PlanProgramaticoCodigo").text());
    modal.find("#TerritorioCodigo").val(fila.find(".TerritorioCodigo").text());
    modal.find("#ContraparteCodigo").val(fila.find(".ContraparteCodigo").text());
    modal.find("#AccionesNacionaleCodigo").val(fila.find(".AccionesNacionaleCodigo").text());
}

function editModalData() {
    var modal = $("#editCuentaAsientoModal");
    var cuentaAsiento = {
        Id: modal.find("#Id").val(),
        CuentaContableId: modal.find("#CuentaContableId").val(),
        PlanProgramaticoId: modal.find("#PlanProgramaticoId").val(),
        TerritorioId: modal.find("#TerritorioId").val(),
        ContraparteId: new Number(modal.find("#ContraparteId").val()),
        CodigoAuditoriaId: modal.find("#CodigoAuditoriaId").val(),
        AccionNacionalId: modal.find("#AccionNacionalId").val(),
        AnexoTributarioId: modal.find("#AnexoTributarioId").val(),
        MarcoLogicoId: modal.find("#MarcoLogicoId").val(),

        Glosa: modal.find("#Glosa").val(),
        NotasAdicionales: modal.find("#NotasAdicionales").val(),
        Debe: new Number(modal.find("#Debe").val()),
        Haber: new Number(modal.find("#Haber").val()),
        CuentaContable: {
            Nombre: modal.find("#CuentaContableId option:selected").text()
        },
        PlanProgramatico: {
            Codigo: modal.find("#PlanProgramaticoId option:selected").text().split(" ")[0],
            Descripcion: modal.find("#PlanProgramaticoId option:selected").text()
        },
        Territorio: {
            Codigo: modal.find("#TerritorioCodigo").val()
        },
        Contraparte: {
            Codigo: modal.find("#ContraparteCodigo").val()
        },
        AccionesNacionale: {
            Codigo: modal.find("#AccionNacionalId option:selected").val() != "" ? modal.find("#AccionNacionalId option:selected").text().split(" - ")[0] : ""
        },
        CodigoMarcoLogico: {
            Codigo: modal.find("#MarcoLogicoId option:selected").val() != "" ? modal.find("#MarcoLogicoId option:selected").text() : ""
        },
        CodigosAuditoria: {
            Descripcion: modal.find("#CodigoAuditoriaId option:selected").val() != "" ? modal.find("#CodigoAuditoriaId option:selected").text() : ""
        },
        Ciudad: {
            Codigo: modal.find("#CiudadCodigo").val()
        }
    };

    $.ajax({

        url: "/CuentasAsientos/Edit",
        data: JSON.stringify(cuentaAsiento),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateRowData(cuentaAsiento);
            $("#editCuentaAsientoModal").modal("hide");
            alert("Los cambios se guardaron correctamente");
        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateRowData(cuentaAsiento) {
    var id = cuentaAsiento.Id;
    var fila = $("#item-" + id);

    fila.find(".PlanProgramaticoId").text(cuentaAsiento.PlanProgramaticoId);
    fila.find(".CuentaContableId").text(cuentaAsiento.CuentaContableId);

    fila.find(".TerritorioId").text(cuentaAsiento.TerritorioId);
    fila.find(".ContraparteId").text(cuentaAsiento.ContraparteId);
    fila.find(".CodigoAuditoriaId").text(cuentaAsiento.CodigoAuditoriaId);
    fila.find(".AccionNacionalId").text(cuentaAsiento.AccionNacionalId);
    fila.find(".MarcoLogicoId").text(cuentaAsiento.MarcoLogicoId);

    fila.find(".CuentaContableNombre").text(cuentaAsiento.CuentaContable.Nombre);
    fila.find(".Glosa").text(cuentaAsiento.Glosa);
    fila.find(".NotasAdicionales").text(cuentaAsiento.NotasAdicionales);
    fila.find(".Debe").text(cuentaAsiento.Debe > 0 ? cuentaAsiento.Debe.toFixed(2).replace(".", ",") : "");
    fila.find(".Haber").text(cuentaAsiento.Haber > 0 ? cuentaAsiento.Haber.toFixed(2).replace(".", ",") : "");

    var codigoProgramatico = cuentaAsiento.Ciudad.Codigo + cuentaAsiento.PlanProgramatico.Codigo + cuentaAsiento.Territorio.Codigo + cuentaAsiento.Contraparte.Codigo + "-/" + cuentaAsiento.AccionesNacionale.Codigo + "*" + cuentaAsiento.CodigoMarcoLogico.Codigo + ":" + cuentaAsiento.NotasAdicionales;
    fila.find(".CodigoProgramatico").text(codigoProgramatico);

    fila.find(".PlanProgramaticoCodigo").text(cuentaAsiento.PlanProgramatico.Codigo);
    fila.find(".TerritorioCodigo").text(cuentaAsiento.Territorio.Codigo);
    fila.find(".ContraparteCodigo").text(cuentaAsiento.Contraparte.Codigo);
    fila.find(".AccionesNacionaleCodigo").text(cuentaAsiento.AccionesNacionale.Codigo);

}

