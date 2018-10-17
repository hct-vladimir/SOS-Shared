using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cerberus.Sos.Accounting.BusinessLogic.Entities.Aldeas;

namespace Cerberus.Sos.Accounting.BusinessLogic.Entities
{
    public class Personal
    {
        public string item { get; set; }
        public string apellidoPaterno { get; set; }
        public string apellidoMaterno { get; set; }
        public string apellidoCasada { get; set; }
        public string nombres { get; set; }

        public string NombreCompleto => string.Format("{0} {1} {2}", nombres, apellidoPaterno, apellidoMaterno);
        public Nullable<System.DateTime> fechaNacimiento { get; set; }
        public string sexo { get; set; }
        public string estadoCivil { get; set; }
        public string codigoEstudio { get; set; }
        public string codigoProfesion { get; set; }
        public string codigoTipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string codigoDepartamento { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string usuario_correo { get; set; }
        public string correo { get; set; }
        public string codigoFilial { get; set; }
        public string codigoPrograma { get; set; }
        public string filial { get; set; }
        public Nullable<System.DateTime> fechaIngreso { get; set; }
        public Nullable<System.DateTime> fechaInicioCargo { get; set; }
        public string codigoTipoContratacion { get; set; }
        public string codigoSeccion { get; set; }
        public string codigoCargoActual { get; set; }
        public ListaCargo ListaCargo { get; set; }
        public string cargo_act { get; set; }
        public string codigoEscalaSalarial { get; set; }
        public Nullable<decimal> salario { get; set; }
        public string contratoIndefinido { get; set; }
        public string tiempoCompleto { get; set; }
        public string numeroCuenta { get; set; }
        public string numeroSeguro { get; set; }
        public string codigoRecibeInstruccion { get; set; }
        public string tieneSubAlternos { get; set; }
        public string codigoEsEvaluado { get; set; }
        public string propositoCargo { get; set; }
        public string funcion1 { get; set; }
        public string funcion2 { get; set; }
        public string funcion3 { get; set; }
        public string codigoAFP { get; set; }
        public string nua { get; set; }
        public Nullable<System.DateTime> fechaRetiro { get; set; }
        public string estado { get; set; }
        public string estadoADM { get; set; }
        public string observaciones { get; set; }
        public string password { get; set; }
        public System.DateTime fechaSistema { get; set; }
        public string login { get; set; }
        public string zona { get; set; }
        public string telefonoContacto { get; set; }
        public Nullable<int> codigoFacility { get; set; }
        public string tipoSeguro { get; set; }
        public string tipoInicio { get; set; }
        public Nullable<int> area { get; set; }
        public string item2 { get; set; }
        public Nullable<int> cotizante { get; set; }
        public string nacionalidad { get; set; }
        public Nullable<System.DateTime> fechaFinContrato { get; set; }
        public Nullable<int> tipoGrupo { get; set; }
        public Nullable<int> tipoGrupoRetroactivo { get; set; }
        public Nullable<int> tipoAasignacion { get; set; }
        public string excepciones { get; set; }
    }
}
