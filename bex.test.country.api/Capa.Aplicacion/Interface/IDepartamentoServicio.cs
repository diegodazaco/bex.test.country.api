using bex.test.country.api.Capa.Aplicacion.DTOs;
using bex.test.country.api.Capa.Dominio.Entidades;

namespace bex.test.country.api.Capa.Aplicacion.Interfaces
{
    public interface IDepartamentoServicio
    {
        public Task<List<DepartamentoDTO>> GetAll();
        public Task<DepartamentoDTO> GetById(int departamentoId);
        public Task<List<DepartamentoDTO>> GetByPaisId(int paisId);
        public Task<Departamento> GetByNombre(string nombreDepartamento);
        public Task<Departamento> Create(Departamento departamento);
        public Task<Departamento> Update(Departamento departamento);
        public Task<bool> Delete(int departamentoId);
    }
}
