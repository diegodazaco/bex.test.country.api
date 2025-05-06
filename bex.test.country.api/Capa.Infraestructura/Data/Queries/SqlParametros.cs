using bex.test.country.api.Capa.Dominio.Enums;

namespace bex.test.country.api.Capa.Infraestructura.Data.Queries
{
    public class SqlParametros
    {
        public string Nombre { get; set; }
        public string TipoDato { get; set; }
        public object Valor { get; set; }
        public string? TipoNombre { get; set; }
        public AccionDireccionParametro Direccion { get; set; }
    }
}
