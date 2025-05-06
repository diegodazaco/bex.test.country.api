using bex.test.country.api.Capa.Aplicacion.Common;
using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Dominio.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace bex.test.country.api.Capa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoServicio _departamentoServicio;
        private readonly ILogger<DepartamentoController> _logger;

        public DepartamentoController(IDepartamentoServicio departamentoServicio, ILogger<DepartamentoController> logger)
        {
            _departamentoServicio = departamentoServicio;
            _logger = logger;
        }

        [HttpGet(nameof(DepartamentoController.GetAll))]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Llamado al metodo GetAll en el DepartamentoController");
            try
            {
                List<Departamento> departamentos = await _departamentoServicio.GetAll();
                return Ok(new { Message = "Se encontraron " + departamentos.Count.ToString() + " departamentos", Resultado = departamentos, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los departamentos");
                return StatusCode(500, "Ocurrió un error interno al listar los departamentos.");
            }
        }

        [HttpGet(nameof(DepartamentoController.GetById))]
        public async Task<IActionResult> GetById(int departamentoId)
        {
            _logger.LogInformation("Llamado al metodo GetById en el DepartamentoController");
            if (departamentoId <= 0)
                return BadRequest("El ID del departamento es inválido.");
            try
            {
                Departamento departamento = await _departamentoServicio.GetById(departamentoId);
                return Ok(new { Mensaje = "Departamento encontrado: " + departamento.NombreDepartamento, Resultado = departamento, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el departamento con ID: {DepartamentoId}", departamentoId);
                return StatusCode(500, "Ocurrió un error interno al obtener el departamento.");
            }
        }

        [HttpGet(nameof(DepartamentoController.GetByPaisId))]
        public async Task<IActionResult> GetByPaisId(int paisId)
        {
            _logger.LogInformation("Llamado al metodo GetByPaisId en el DepartamentoController");
            if (paisId <= 0)
                return BadRequest("El ID del país es inválido.");
            try
            {
                List<Departamento> departamentos = await _departamentoServicio.GetByPaisId(paisId);
                return Ok(new { Mensaje = "Se encontraron " + departamentos.Count.ToString() + " departamentos para el país con ID: " + paisId, Resultado = departamentos, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los departamentos para el país con ID: {PaisId}", paisId);
                return StatusCode(500, "Ocurrió un error interno al obtener los departamentos.");
            }
        }

        [HttpPost(nameof(DepartamentoController.Create))]
        public async Task<IActionResult> Create([FromBody] Departamento departamento)
        {
            _logger.LogInformation("Llamado al metodo Create en el DepartamentoController");
            if (departamento == null)
                return BadRequest("El departamento no puede ser nulo.");
            try
            {
                Departamento nuevoDepartamento = await _departamentoServicio.Create(departamento);
                return Ok(new { Mensaje = "Departamento creado con éxito: " + nuevoDepartamento.NombreDepartamento, Resultado = nuevoDepartamento, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el departamento: {Nombre}. Mensaje {Message}", departamento.NombreDepartamento, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al crear el departamento.");
            }
        }

        [HttpPost(nameof(DepartamentoController.Delete))]
        public async Task<IActionResult> Delete(int departamentoId)
        {
            _logger.LogInformation("Llamado al metodo Delete en el DepartamentoController");
            if (departamentoId <= 0)
                return BadRequest("El ID del departamento es inválido.");
            try
            {
                bool resultado = await _departamentoServicio.Delete(departamentoId);
                return Ok(new { Mensaje = "Departamento eliminado con éxito", EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el departamento con ID: {DepartamentoId}", departamentoId);
                return StatusCode(500, "Ocurrió un error interno al eliminar el departamento.");
            }
        }

        [HttpPost(nameof(DepartamentoController.Update))]
        public async Task<IActionResult> Update([FromBody] Departamento departamento)
        {
            _logger.LogInformation("Llamado al metodo Update en el DepartamentoController");
            if (departamento == null)
                return BadRequest("El departamento no puede ser nulo.");
            try
            {
                Departamento departamentoActualizado = await _departamentoServicio.Update(departamento);
                return Ok(new { Mensaje = "Departamento actualizado con éxito: " + departamentoActualizado.NombreDepartamento, Resultado = departamentoActualizado, EsExitoso = true });
            }
            catch (ValidacionException vex)
            {
                return BadRequest(new { Mensaje = vex.Message, EsExitoso = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el departamento: {Nombre}. Mensaje {Message}", departamento.NombreDepartamento, ex.Message);
                return StatusCode(500, "Ocurrió un error interno al actualizar el departamento.");
            }
        }
    }
}
