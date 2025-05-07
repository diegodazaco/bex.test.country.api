namespace bex.test.country.api.Capa.Aplicacion.DTOs
{
    public class CiudadDTO
    {
        public int CiudadId { get; set; }
        public int DepartamentoId { get; set; }
        public string NombreCiudad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? NombreDepartamento { get; set; }
        public string? NombrePais { get; set; }
    }
}
