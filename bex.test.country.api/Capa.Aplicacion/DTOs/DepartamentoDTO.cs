namespace bex.test.country.api.Capa.Aplicacion.DTOs
{
    public class DepartamentoDTO
    {
        public int DepartamentoId { get; set; }
        public int PaisId { get; set; }
        public string NombreDepartamento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? NombrePais { get; set; }
    }
}
