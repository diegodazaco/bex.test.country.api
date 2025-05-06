using bex.test.country.api.Capa.Dominio.Entidades;

namespace bex.test.country.api.Capa.Dominio.Interfaces
{
    public interface IPaisRepositorio
    {
        public Task<List<Pais>> GetAll();
        public Task<Pais> GetById(int paisId);
        public Task<Pais> GetByNombre(string nombrePais);
        public Task<Pais> Create(Pais pais);
        public Task<Pais> Update(Pais pais);
        public Task<bool> Delete(int paisId);
    }
}
