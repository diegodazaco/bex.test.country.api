using bex.test.country.api.Capa.Aplicacion.Common;
using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Dominio.Entidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace bex.test.country.api.Capa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly IPaisServicio _paisServicio;
        private readonly ILogger<PaisController> _logger;

        public PaisController(IPaisServicio paisServicio, ILogger<PaisController> logger)
        {
            _paisServicio = paisServicio;
            _logger = logger;
        }

        [HttpGet(nameof(PaisController.GetAll))]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Llamado al metodo GetAll en el PaisController");
            try
            {
                List<Pais> paises = await _paisServicio.GetAll();
                return Ok(new { Message = "Se encontraron " + paises.Count.ToString() + " paises", Resultado = paises, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los paises");
                return StatusCode(500, "Ocurrió un error interno al listar los paises.");
            }
        }

        [HttpGet(nameof(PaisController.GetById))]
        public async Task<IActionResult> GetById(int paisId)
        {
            _logger.LogInformation("Llamado al metodo GetById en el PaisController");
            if (paisId <= 0)
                return BadRequest("El ID del país es inválido.");
            try
            {
                Pais pais = await _paisServicio.GetById(paisId);
                return Ok(new { Mensaje = "País encontrado: " + pais.NombrePais, Resultado = pais, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el país con ID: {PaisId}", paisId);
                return StatusCode(500, "Ocurrió un error interno al obtener el país.");
            }
        }

        [HttpPost(nameof(PaisController.Create))]
        public async Task<IActionResult> Create([FromBody] Pais pais)
        {
            _logger.LogInformation("Llamado al metodo Create en el PaisController");
            if (pais == null)
                return BadRequest("El país no puede ser nulo.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Pais paisCreado = await _paisServicio.Create(pais);
                return Ok(new { Mensaje = "País creado con éxito: " + paisCreado.NombrePais, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el país: {Nombre}. Mensaje {Message}", pais.NombrePais, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al crear el país.");
            }
        }

        [HttpPost(nameof(PaisController.Delete))]
        public async Task<IActionResult> Delete(int paisId)
        {
            _logger.LogInformation("Llamado al metodo Delete en el PaisController");
            if (paisId <= 0)
                return BadRequest("El ID del país es inválido.");
            try
            {
                bool resultado = await _paisServicio.Delete(paisId);
                return Ok(new { Mensaje = "País eliminado con éxito", EsExitoso = resultado });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el país con ID: {PaisId}", paisId);
                return StatusCode(500, "Ocurrió un error interno al eliminar el país.");
            }
        }

        [HttpPost(nameof(PaisController.Update))]
        public async Task<IActionResult> Update([FromBody] Pais pais)
        {
            _logger.LogInformation("Llamado al metodo Update en el PaisController");
            if (pais == null)
                return BadRequest("El país no puede ser nulo.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Pais paisActualizado = await _paisServicio.Update(pais);
                return Ok(new { Mensaje = "País actualizado con éxito: " + paisActualizado.NombrePais, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar el país: {Nombre}. Mensaje {Message}", pais.NombrePais, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al actualizar el país.");
            }
        }
    }
}
