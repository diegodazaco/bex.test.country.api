using bex.test.country.api.Capa.Dominio.Entidades;

namespace bex.test.country.api.Capa.Aplicacion.Interfaces
{
    public interface ICiudadServicio
    {
        public Task<List<Ciudad>> GetAll();
        public Task<Ciudad> GetById(int ciudadId);
        public Task<List<Ciudad>> GetByDepartamentoId(int departamentoId);
        public Task<Ciudad> Create(Ciudad ciudad);
        public Task<Ciudad> Update(Ciudad ciudad);
        public Task<bool> Delete(int ciudadId);
    }
}
