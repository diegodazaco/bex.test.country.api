using System.ComponentModel.DataAnnotations;

namespace bex.test.country.api.Capa.Dominio.Entidades
{
    public class Pais
    {
        [Required]
        [Key]
        public int PaisId { get; set; }
        [Required(ErrorMessage = "El nombre del país es obligatorio y tener un máximo de 100 caracteres")]
        public string NombrePais { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

    }
}
