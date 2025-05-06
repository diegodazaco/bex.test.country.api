using bex.test.country.api.Capa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace bex.test.country.api.Capa.Infraestructura.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Pais> Paises { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pais>().ToTable("Pais");
            modelBuilder.Entity<Pais>().HasKey(p => p.PaisId);
            modelBuilder.Entity<Pais>().Property(p => p.NombrePais).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Pais>().Property(p => p.FechaCreacion).IsRequired().HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Pais>().Property(p => p.FechaModificacion).IsRequired(false);
        }
    }
}
