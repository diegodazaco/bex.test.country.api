namespace bex.test.country.api.Capa.Dominio.Entidades
{
    public class Ciudad
    {
        public int CiudadId { get; set; }
        public int DepartamentoId { get; set; }
        public string NombreCiudad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
