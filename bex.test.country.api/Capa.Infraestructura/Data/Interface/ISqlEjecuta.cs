using bex.test.country.api.Capa.Infraestructura.Data.Queries;

namespace bex.test.country.api.Capa.Infraestructura.Data.Interface
{
    public interface ISqlEjecuta
    {
        public Task<T> EjecutaGenericoPSAsync<T>(SqlSPEjecuta sqlSPExecute);
    }
}
