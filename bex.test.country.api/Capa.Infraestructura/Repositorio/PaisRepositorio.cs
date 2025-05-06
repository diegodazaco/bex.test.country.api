using bex.test.country.api.Capa.Dominio.Entidades;
using bex.test.country.api.Capa.Dominio.Interfaces;
using bex.test.country.api.Capa.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace bex.test.country.api.Capa.Infraestructura.Repositorio
{
    public class PaisRepositorio : IPaisRepositorio
    {
        private readonly DataContext _context;

        public PaisRepositorio(DataContext context)
        {
            _context = context;
        }

        public async Task<Pais> Create(Pais pais)
        {
            _context.Paises.Add(pais);
            await _context.SaveChangesAsync();
            return pais;
        }

        public async Task<bool> Delete(int paisId)
        {
            Pais? pais = await _context.Paises.AsNoTracking().FirstOrDefaultAsync(p => p.PaisId == paisId);
            if (pais == null)
                return false;

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Pais>> GetAll()
        {
            List<Pais> paises = await _context.Paises.ToListAsync();
            if (!paises.Any())
                return null!;

            return paises;
        }

        public async Task<Pais> GetById(int paisId)
        {
            Pais? pais = await _context.Paises.FindAsync(paisId);
            if (pais == null)
                return null!;

            return pais;
        }

        public async Task<Pais> GetByNombre(string nombrePais)
        {
            Pais? pais = await _context.Paises.AsNoTracking().FirstOrDefaultAsync(p => p.NombrePais == nombrePais);
            if (pais == null)
                return null!;
            return pais;
        }

        public async Task<Pais> Update(Pais pais)
        {
            Pais? paisExistente = await _context.Paises.AsNoTracking().FirstOrDefaultAsync(p => p.PaisId == pais.PaisId);
            if (paisExistente == null)
                return null!;

            paisExistente.PaisId = pais.PaisId;
            paisExistente.NombrePais = pais.NombrePais;
            paisExistente.FechaModificacion = DateTime.Now;

            _context.Paises.Update(paisExistente);
            await _context.SaveChangesAsync();
            return pais;
        }
    }
}
