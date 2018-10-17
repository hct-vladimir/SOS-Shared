/*!
 * AJAX CRUD Operations EstadosCuentas
 */
$(function () {
    $(".chkEstadoCuenta").change(function () {
        var idRow = $(this).parent().parent().attr("id");
        var fila = $("#" + idRow);
        if ($(this).is(":checked")) {
            $("#CuentaContableEstadoId").val(fila.find(".CuentaContableId").text());
            $("#Glosa").val(fila.find(".Glosa").text());
            $("#Debe").val(fila.find(".Haber").text().replace(/\.| /g, "").replace(",", "."));
            $("#Haber").val(fila.find(".Debe").text().replace(/\.| /g, "").replace(",", "."));

            $("#EstadoCuentaRelacionado").val("true");
            $("#EstadoCuentaRelacionadoId").val(fila.find(".Id").text());
        } else {
            $("#CuentaContableEstadoId").val("");
            $("#Glosa").val("");
            $("#Debe").val("");
            $("#Haber").val("");

            $("#EstadoCuentaRelacionado").val("false");
            $("#EstadoCuentaRelacionadoId").val("");
        }
    });
});

function cleanDecimals() {
    var debe = $("#Debe");
    var debeReal = debe.val().replace(".", ",");
    debe.val(debeReal);

    var haber = $("#Haber");
    var haberReal = haber.val().replace(".", ",");
    haber.val(haberReal);
}