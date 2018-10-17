$(function () {
    $("#reporte-general").DataTable({
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
            var sumColumn = $("#columnaTotal").attr("colspan");

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
            $("#MontoTotal").html(total.toLocaleString("es-CL", { minimumFractionDigits: 2 }));
        }
    });

    $("#reporte-mensual").DataTable({
        searching: false,
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

            /*** Columna Total En Bs. ***/
            var sumColumn = $("#columnaTotal").attr("colspan");

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
            $("#MontoTotal").html(total.toLocaleString("es-CL", { minimumFractionDigits: 2 }));


            /*** Columna Total Cobertura ***/
            if ($("#TieneCobertura").val() == "True") {

                var sumColumnCobertura = new Number($("#columnaTotal").attr("colspan")) + 1;

                // Remove the formatting to get integer data for summation
                var intValCobertura = function (i) {
                    return typeof i === "string" ?
                        i.replace(/\./g, "").replace(",", ".") * 1 :
                        typeof i === "number" ?
                        i : 0;
                };

                // Total over all pages
                var totalCobertura = api
                    .column(sumColumnCobertura)
                    .data()
                    .reduce(function(a, b) {
                        return intValCobertura(a) + intValCobertura(b);
                    }, 0);

                // Total over this page
                var pageTotalCobertura = api
                    .column(sumColumnCobertura, { page: "current" })
                    .data()
                    .reduce(function(a, b) {
                        return intValCobertura(a) + intValCobertura(b);
                    }, 0);

                // Update footer
                $(api.column(sumColumnCobertura).footer()).html(pageTotalCobertura.toLocaleString("es-CL"));
                $("#CoberturaTotal").html(totalCobertura.toLocaleString("es-CL"));
            }
        }
    });

    /* ´Módulo de Planillas */
    $("#planilla-sueldos").DataTable({
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
            var sumColumn = $("#columnaTotal").attr("colspan");

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
            $("#MontoTotal").html(total.toLocaleString("es-CL", { minimumFractionDigits: 2 }));
        }
    });

});