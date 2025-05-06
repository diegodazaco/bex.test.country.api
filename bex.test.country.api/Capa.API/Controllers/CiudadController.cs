using bex.test.country.api.Capa.Aplicacion.Common;
using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Aplicacion.Servicio;
using bex.test.country.api.Capa.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace bex.test.country.api.Capa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CiudadController : ControllerBase
    {
        private readonly ICiudadServicio _ciudadServicio;
        private readonly ILogger<CiudadController> _logger;
        public CiudadController(ICiudadServicio ciudadServicio, ILogger<CiudadController> logger)
        {
            _ciudadServicio = ciudadServicio;
            _logger = logger;
        }

        [HttpGet(nameof(CiudadController.GetAll))]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Llamado al metodo GetAll en el CiudadController");
            try
            {
                List<Ciudad> ciudades = await _ciudadServicio.GetAll();
                return Ok(new { Message = "Se encontraron " + ciudades.Count.ToString() + " ciudades", Resultado = ciudades, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ciudades");
                return StatusCode(500, "Ocurrió un error interno al listar las ciudades.");
            }
        }

        [HttpGet(nameof(CiudadController.GetById))]
        public async Task<IActionResult> GetById(int ciudadId)
        {
            _logger.LogInformation("Llamado al metodo GetById en el CiudadController");
            if (ciudadId <= 0)
                return BadRequest("El ID de la ciudad es inválido.");
            try
            {
                Ciudad ciudad = await _ciudadServicio.GetById(ciudadId);
                return Ok(new { Mensaje = "Ciudad encontrada: " + ciudad.NombreCiudad, Resultado = ciudad, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la ciudad con ID: {CiudadId}", ciudadId);
                return StatusCode(500, "Ocurrió un error interno al obtener la ciudad.");
            }
        }

        [HttpGet(nameof(CiudadController.GetByDepartamentoId))]
        public async Task<IActionResult> GetByDepartamentoId(int departamentoId)
        {
            _logger.LogInformation("Llamado al metodo GetByDepartamentoId en el CiudadController");
            if (departamentoId <= 0)
                return BadRequest("El ID del departamento es inválido.");
            try
            {
                List<Ciudad> ciudades = await _ciudadServicio.GetByDepartamentoId(departamentoId);
                return Ok(new { Mensaje = "Se encontraron " + ciudades.Count.ToString() + " ciudades para el departamento con ID: " + departamentoId, Resultado = ciudades, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las ciudades para el departamento con ID: {DepartamentoId}", departamentoId);
                return StatusCode(500, "Ocurrió un error interno al obtener las ciudades.");
            }
        }

        [HttpPost(nameof(CiudadController.Create))]
        public async Task<IActionResult> Create([FromBody] Ciudad ciudad)
        {
            _logger.LogInformation("Llamado al metodo Create en el CiudadController");
            if (ciudad == null)
                return BadRequest("El objeto ciudad no puede ser nulo.");
            try
            {
                Ciudad ciudadCreada = await _ciudadServicio.Create(ciudad);
                return Ok(new { Mensaje = "Ciudad creada con éxito: " + ciudadCreada.NombreCiudad, Resultado = ciudadCreada, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la ciudad: {Nombre}. Mensaje {Message}", ciudad.NombreCiudad, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al crear la ciudad.");
            }
        }

        [HttpPost(nameof(CiudadController.Delete))]
        public async Task<IActionResult> Delete(int ciudadId)
        {
            _logger.LogInformation("Llamado al metodo Delete en el CiudadController");
            if (ciudadId <= 0)
                return BadRequest("El ID de la ciudad es inválido.");
            try
            {
                bool resultado = await _ciudadServicio.Delete(ciudadId);
                if (resultado)
                    return Ok(new { Mensaje = "Ciudad eliminada con éxito", EsExitoso = true });
                else
                    return NotFound(new { Mensaje = "No se encontró la ciudad con ID: " + ciudadId, EsExitoso = false });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la ciudad con ID: {CiudadId}. Mensaje {Message}", ciudadId, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al eliminar la ciudad.");
            }
        }

        [HttpPost(nameof(CiudadController.Update))]
        public async Task<IActionResult> Update([FromBody] Ciudad ciudad)
        {
            _logger.LogInformation("Llamado al metodo Update en el CiudadController");
            if (ciudad == null)
                return BadRequest("El objeto ciudad no puede ser nulo.");
            try
            {
                Ciudad ciudadActualizada = await _ciudadServicio.Update(ciudad);
                return Ok(new { Mensaje = "Ciudad actualizada con éxito: " + ciudadActualizada.NombreCiudad, Resultado = ciudadActualizada, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la ciudad: {Nombre}. Mensaje {Message}", ciudad.NombreCiudad, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al actualizar la ciudad.");
            }
        }
    }
}
