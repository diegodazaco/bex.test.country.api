using bex.test.country.api.Capa.Dominio.Entidades;

namespace bex.test.country.api.Capa.Aplicacion.Interfaces
{
    public interface IDepartamentoServicio
    {
        public Task<List<Departamento>> GetAll();
        public Task<Departamento> GetById(int departamentoId);
        public Task<List<Departamento>> GetByPaisId(int paisId);
        public Task<Departamento> Create(Departamento departamento);
        public Task<Departamento> Update(Departamento departamento);
        public Task<bool> Delete(int departamentoId);
    }
}
