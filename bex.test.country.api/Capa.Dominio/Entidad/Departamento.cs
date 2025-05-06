namespace bex.test.country.api.Capa.Dominio.Entidades
{
    public class Departamento
    {
        public int DepartamentoId { get; set; }
        public int PaisId { get; set; }
        public string NombreDepartamento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
