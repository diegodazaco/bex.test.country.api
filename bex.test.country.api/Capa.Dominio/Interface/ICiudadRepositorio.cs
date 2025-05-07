using bex.test.country.api.Capa.Aplicacion.DTOs;
using bex.test.country.api.Capa.Dominio.Entidades;

namespace bex.test.country.api.Capa.Dominio.Interfaces
{
    public interface ICiudadRepositorio
    {
        public Task<List<CiudadDTO>> GetAll();
        public Task<CiudadDTO> GetById(int ciudadId);
        public Task<List<CiudadDTO>> GetByDepartamentoId(int departamentoId);
        public Task<Ciudad> GetByNombre(string nombreCiudad);
        public Task<Ciudad> Create(Ciudad ciudad);
        public Task<Ciudad> Update(Ciudad ciudad);
        public Task<bool> Delete(int ciudadId);
    }
}
