/*!
 * AJAX CRUD Operations Recursos
 */
$(function () {
    $("#recursos").DataTable({
        searching: true,
        columnDefs: [
            { "orderable": true, "targets": 0 },
            { "searchable": false, "targets": 0 }
        ],
        language: {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Mostrar _MENU_ filas por página",
            "zeroRecords": "No existen datos.",
            "info": "Página _PAGE_ de _PAGES_",
            "infoEmpty": "No existen datos",
            "infoFiltered": "(cantidad de registros: _MAX_ )",
            "search": "Búsqueda rápida:",
            "paginate": {
                "previous": "<<",
                "next": ">>"
            }
        },
        footerCallback: function (row, data, start, end, display) {
            var api = this.api();
            var sumColumn = 8;

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === "string" ?
                    i.replace(/\./g, "").replace(",", ".") * 1 :
                    typeof i === "number" ?
                    i : 0;
            };

            // Total over all pages
            var total = api
                .column(sumColumn)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Total over this page
            var pageTotal = api
                .column(sumColumn, { page: "current" })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            $(api.column(sumColumn).footer()).html(pageTotal.toLocaleString("es-CL", { minimumFractionDigits: 2 }));
            $("#MontoTotalFacility").html(total.toLocaleString("es-CL", { minimumFractionDigits: 2 }));
        }
    });

    $("#addRecursoModal form").validate(addRecursoValidation);
    $("#editRecursoModal form").validate(editRecursoValidation);
    $("#panel-observaciones form").validate(addObservacionValidation);

    // Eventos del formulario
    $("#AddRecurso").click(function () {
        if ($("#addRecursoModal form").valid()) {
            cleanDecimals();
            $(this).attr("disabled", "disabled");

            // Only for Chrome Browser
            $("#addRecursoModal form").submit();
            return true;
        }
    });

    $("#EditRecurso").click(function () {
        if ($("#editRecursoModal form").valid()) {
            if ($("#editRecursoModal").find("#TieneCobertura").is(":checked")) {
                $("#editRecursoModal").find("#Cobertura").trigger("change");
            }
            editModalData();
            return false;
        }
    });

    $("#CloseAddRecursoModal").click(function () {
        $("#addRecursoModal").modal("hide");
    });

    $("#CloseEditRecursoModal").click(function () {
        $("#editRecursoModal").modal("hide");
    });

    $("#AddObservacion").click(function () {
        if ($("#panel-observaciones form").valid()) {
            $(this).attr("disabled", "disabled");

            // Only for Chrome Browser
            $("#panel-observaciones form").submit();
            return true;
        }
    });

    /* Cálculos del modal*/
    $("#addRecursoModal").find("#Cobertura, #IndiceTransferencia").change(function () {
        var modal = $("#addRecursoModal");
        calcularCobertura(modal);
    });

    $("#editRecursoModal").find("#Cobertura, #IndiceTransferencia").change(function () {
        var modal = $("#editRecursoModal");
        calcularCobertura(modal);
    });

    /* Habilitación de coberturas */
    $("#addRecursoModal").find("#TieneCobertura").change(function () {
        var modal = $("#addRecursoModal");
        if ($(this).is(":checked")) {
            modal.find("#Cobertura, #IndiceTransferencia").removeAttr("readonly");
            modal.find("#Monto").attr("readonly", "readonly");
            modal.find("#Monto").val("0");
        } else {
            modal.find("#Monto").removeAttr("readonly");
            modal.find("#Cobertura, #IndiceTransferencia").attr("readonly", "readonly");
            modal.find("#Cobertura, #IndiceTransferencia").val("0");
        }
    });

    $("#editRecursoModal").find("#TieneCobertura").change(function () {
        var modal = $("#editRecursoModal");
        if ($(this).is(":checked")) {
            modal.find("#Cobertura, #IndiceTransferencia").removeAttr("readonly");
            modal.find("#Monto").attr("readonly", "readonly");
            modal.find("#Monto").val("0");
            calcularCobertura(modal);
        } else {
            modal.find("#Monto").removeAttr("readonly");
            modal.find("#Cobertura, #IndiceTransferencia").attr("readonly", "readonly");
            modal.find("#Cobertura, #IndiceTransferencia").val("0");
        }
    });

});

function calcularCobertura(modal) {
    var cobertura = new Number(modal.find("#Cobertura").val());
    var indiceTransferencia = new Number(modal.find("#IndiceTransferencia").val());
    var total = (cobertura != "" && cobertura > 0) && (indiceTransferencia != "" && indiceTransferencia != 0) ? cobertura * indiceTransferencia : 0;
    modal.find("#Monto").val(total.toFixed(2));
}

function cleanDecimals() {
    var modal = $("#addRecursoModal");
    var montoReal = modal.find("#Monto").val().replace(".", ",");
    modal.find("#Monto").val(montoReal);

    var indiceReal = modal.find("#IndiceTransferencia").val().replace(".", ",");
    modal.find("#IndiceTransferencia").val(indiceReal);
    return true;
}

/* Funciones AJAX CRUD Operations */
function loadRowDataIntoModal(idRow) {
    var modal = $("#editRecursoModal");
    var fila = $("#" + idRow);


    modal.find("#PlanProgramaticoId").multiselect("select", fila.find(".PlanProgramaticoId").text());
    modal.find("#CuentaContableId").multiselect("select", fila.find(".CuentaContableId").text());

    modal.find("#TerritorioId").val(fila.find(".TerritorioId").text());
    modal.find("#ContraparteId").val(fila.find(".ContraparteId").text());
    modal.find("#CodigoAuditoriaId").val(fila.find(".CodigoAuditoriaId").text());
    modal.find("#AccionNacionalId").val(fila.find(".AccionNacionalId").text());

    modal.find("#Id").val(fila.find(".Id").text());
    modal.find("#Descripcion").val(fila.find(".Descripcion").text());
    modal.find("#NotasAdicionales").val(fila.find(".NotasAdicionales").text());
    modal.find("#Monto").val(fila.find(".Monto").text().replace(/\./g, "").replace(",", "."));
    modal.find("#IndiceTransferencia").val(fila.find(".IndiceTransferencia").text().replace(/\./g, "").replace(",", "."));
    modal.find("#Cobertura").val(fila.find(".Cobertura").text().replace(/\./g, "").replace(",", "."));

    var cobertura = new Number(fila.find(".Cobertura").text().replace(/\./g, "").replace(",", "."));
    modal.find("#TieneCobertura").prop("checked", (cobertura > 0));
    modal.find("#TieneCobertura").trigger("change");

    modal.find("#CiudadCodigo").val(fila.find(".CiudadCodigo").text());
    modal.find("#PlanProgramaticoCodigo").val(fila.find(".PlanProgramaticoCodigo").text());
    modal.find("#TerritorioCodigo").val(fila.find(".TerritorioCodigo").text());
    modal.find("#ContraparteCodigo").val(fila.find(".ContraparteCodigo").text());
    modal.find("#AccionesNacionaleCodigo").val(fila.find(".AccionesNacionaleCodigo").text());
    modal.find("#MarcoLogicoId").val(fila.find(".MarcoLogicoId").text());
}

function editModalData() {
    var modal = $("#editRecursoModal");
    var recurso = {
        Id: modal.find("#Id").val(),
        CuentaContableId: modal.find("#CuentaContableId").val(),
        PlanProgramaticoId: modal.find("#PlanProgramaticoId").val(),
        TerritorioId: modal.find("#TerritorioId").val(),
        ContraparteId: modal.find("#ContraparteId").val(),
        CodigoAuditoriaId: modal.find("#CodigoAuditoriaId").val(),
        AccionNacionalId: modal.find("#AccionNacionalId").val(),
        MarcoLogicoId: modal.find("#MarcoLogicoId").val(),

        Descripcion: modal.find("#Descripcion").val(),
        NotasAdicionales: modal.find("#NotasAdicionales").val(),
        Monto: new Number(modal.find("#Monto").val()),
        Cobertura: new Number(modal.find("#Cobertura").val()),
        IndiceTransferencia: new Number(modal.find("#IndiceTransferencia").val()),
        CuentaContable: {
            Nombre: modal.find("#CuentaContableId option:selected").text()
        },
        PlanProgramatico: {
            Codigo: modal.find("#PlanProgramaticoId option:selected").text().split(" ")[0],
            Descripcion: modal.find("#PlanProgramaticoId option:selected").text()
        },
        CodigosAuditoria: {
            Descripcion: modal.find("#CodigoAuditoriaId option:selected").val() != "" ? modal.find("#CodigoAuditoriaId option:selected").text() : ""
        },
        Territorio: {
            Codigo: modal.find("#TerritorioId option:selected").text().split(" - ")[0]
        },
        Contraparte: {
            Codigo: modal.find("#ContraparteId option:selected").text().split(" - ")[0]
        },
        AccionesNacionale: {
            Codigo: modal.find("#AccionNacionalId option:selected").val() != "" ? modal.find("#AccionNacionalId option:selected").text().split(" - ")[0] : ""
        },
        CodigoMarcoLogico: {
            Codigo: modal.find("#MarcoLogicoId option:selected").val() != "" ? modal.find("#MarcoLogicoId option:selected").text() : ""
},
        Ciudad: {
            Codigo: modal.find("#CiudadCodigo").val()
        }
    };

    $.ajax({

        url: "/Recursos/Edit",
        data: JSON.stringify(recurso),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            updateRowData(recurso);
            $("#editRecursoModal").modal("hide");
            alert("Los cambios se guardaron correctamente");
        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function updateRowData(recurso) {
    var id = recurso.Id;
    var fila = $("#item-" + id);

    fila.find(".PlanProgramaticoId").text(recurso.PlanProgramaticoId);
    fila.find(".CuentaContableId").text(recurso.CuentaContableId);

    fila.find(".TerritorioId").text(recurso.TerritorioId);
    fila.find(".ContraparteId").text(recurso.ContraparteId);
    fila.find(".CodigoAuditoriaId").text(recurso.CodigoAuditoriaId);
    fila.find(".AccionNacionalId").text(recurso.AccionNacionalId);
    fila.find(".MarcoLogicoId").text(recurso.MarcoLogicoId);

    fila.find(".CuentaContableNombre").text(recurso.CuentaContable.Nombre);
    fila.find(".PlanProgramaticoDescripcion").text(recurso.PlanProgramatico.Descripcion);
    fila.find(".CodigoAuditoriaDescripcion").text(recurso.CodigosAuditoria.Descripcion);
    fila.find(".Descripcion").text(recurso.Descripcion);
    fila.find(".NotasAdicionales").text(recurso.NotasAdicionales);
    fila.find(".Monto").text(recurso.Monto.toFixed(2).replace(".", ","));
    fila.find(".Cobertura").text(recurso.Cobertura);
    fila.find(".IndiceTransferencia").text(recurso.IndiceTransferencia.toFixed(2).replace(".", ","));

    var codigoProgramatico = recurso.Ciudad.Codigo + recurso.PlanProgramatico.Codigo + recurso.Territorio.Codigo + recurso.Contraparte.Codigo + "-/" + recurso.AccionesNacionale.Codigo + "*" + recurso.CodigoMarcoLogico.Codigo + ":" + recurso.NotasAdicionales;
    fila.find(".CodigoProgramatico").text(codigoProgramatico);

    fila.find(".PlanProgramaticoCodigo").text(recurso.PlanProgramatico.Codigo);
    fila.find(".TerritorioCodigo").text(recurso.Territorio.Codigo);
    fila.find(".ContraparteCodigo").text(recurso.Contraparte.Codigo);
    fila.find(".AccionesNacionaleCodigo").text(recurso.AccionesNacionale.Codigo);

    //Actualizar Monto Total
    var total = 0;
    var arrayMontos = $("#recursos").DataTable().column(8).data();
    for (var i = 0; i < arrayMontos.length; i++) {
        var monto = new Number(arrayMontos[i].replace(/\./g, "").replace(",", "."));
        total += monto;
    }

    $("#MontoTotal").text(total.toFixed(2));
}

/*
    Validation Section
*/

// Custom Validation Rules
$(function () {
    // Reglas personalizadas de jQueryValidation
    jQuery.validator.addMethod("validarMonto", function (value, element) {
        var modal = $('#addRecursoModal').is(':visible') ? $('#addRecursoModal') : null;
        if (modal != null) {
            if (modal.find("#Cobertura").val() > 0 || modal.find("#Monto").val() != 0) {
                return true;
            } else {
                return false;
            };
        }

        modal = $('#editRecursoModal').is(':visible') ? $('#editRecursoModal') : null;
        if (modal != null) {
            if (modal.find("#Cobertura").val() > 0 || modal.find("#Monto").val() != 0) {
                return true;
            } else {
                return false;
            };
        }

        return false;
    }, "No debe ser cero.");
});


// Validation Objects Definition
addRecursoValidation = {
    rules: {
        CuentaContableId: {
            required: true
        },
        PlanProgramaticoId: {
            required: true
        },
        Descripcion: {
            required: true
        },
        TerritorioId: {
            required: true
        },
        ContraparteId: {
            required: true
        },
        Cobertura: {
            validarMonto: true
        },
        IndiceTransferencia: {
            validarMonto: true
        },
        Monto: {
            validarMonto: true
        }
    },
    messages: {
        CuentaContableId: "Debe seleccionar una Cuenta",
        PlanProgramaticoId: "Debe seleccionar un Código del Plan Programático",
        Descripcion: "El campo es requerido.",
        TerritorioId: "Debe seleccionar un Territorio.",
        ContraparteId: "Debe seleccionar una Contraparte."
    }

};


editRecursoValidation = {
    rules: {
        CuentaContableId: {
            required: true
        },
        PlanProgramaticoId: {
            required: true
        },
        Descripcion: {
            required: true
        },
        TerritorioId: {
            required: true
        },
        ContraparteId: {
            required: true
        },
        Cobertura: {
            validarMonto: true
        },
        IndiceTransferencia: {
            validarMonto: true
        },
        Monto: {
            validarMonto: true
        }
    },
    messages: {
        CuentaContableId: "Debe seleccionar una Cuenta",
        PlanProgramaticoId: "Debe seleccionar un Código del Plan Programático",
        Descripcion: "El campo es requerido.",
        TerritorioId: "Debe seleccionar un Territorio.",
        ContraparteId: "Debe seleccionar una Contraparte."
    }

};


addObservacionValidation = {
    rules: {
        Descripcion: {
            required: true
        }
    },
    messages: {
        Descripcion: "Ingrese el texto de la Observación"
    }
};