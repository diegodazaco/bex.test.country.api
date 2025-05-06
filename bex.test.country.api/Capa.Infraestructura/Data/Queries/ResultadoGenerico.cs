using bex.test.country.api.Capa.Dominio.Enums;

namespace bex.test.country.api.Capa.Infraestructura.Data.Queries
{
    public class ResultadoGenerico
    {
        public bool EsExitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public dynamic? Id { get; set; }
        public TipoMensaje? TipoMensaje { get; set; }
    }
}
