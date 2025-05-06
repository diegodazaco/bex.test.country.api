using bex.test.country.api.Capa.Dominio.Enums;
using System;

namespace bex.test.country.api.Capa.Infraestructura.Data.Queries
{
    public class SqlSPEjecuta
    {
        public string NombreProcedimientoAlmacenado { get; set; } = string.Empty;
        public List<ParametroGenerico> Parametros { get; set; }
        public AccionSP AccionSP { get; set; }
    }
}
