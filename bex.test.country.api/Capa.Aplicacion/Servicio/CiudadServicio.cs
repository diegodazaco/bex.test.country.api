using bex.test.country.api.Capa.Aplicacion.Common;
using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Dominio.Entidades;
using bex.test.country.api.Capa.Dominio.Interfaces;
using Serilog.Context;

namespace bex.test.country.api.Capa.Aplicacion.Servicio
{
    public class CiudadServicio : ICiudadServicio
    {
        private readonly ICiudadRepositorio _ciudadRepositorio;
        private readonly ILogger<CiudadServicio> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;

        public CiudadServicio(ICiudadRepositorio ciudadRepositorio, ILogger<CiudadServicio> logger, IHttpContextAccessor httpContextAccessor, IDepartamentoRepositorio departamentoRepositorio)
        {
            _ciudadRepositorio = ciudadRepositorio;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _departamentoRepositorio = departamentoRepositorio;
            LogContext.PushProperty("Application", "bex.test.country.api - CiudadServicio");
        }

        public async Task<Ciudad> Create(Ciudad ciudad)
        {
            try
            {
                string mensajeValidacion = IsValid(ciudad);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);

                if (await _ciudadRepositorio.GetByNombre(ciudad.NombreCiudad) != null)
                    throw new ValidacionException("La ciudad ya existe.");
                if (await _departamentoRepositorio.GetById(ciudad.DepartamentoId) == null)
                    throw new ValidacionException("El departamento no existe.");

                Ciudad ciudadResult = await _ciudadRepositorio.Create(ciudad);
                _logger.LogInformation("Ciudad creada con éxito: {NombreCiudad}", ciudad.NombreCiudad);
                return ciudadResult;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al crear la ciudad: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la ciudad: {Nombre}. CorrelacionId {CorrelationId}. Mensaje {Message}", ciudad.NombreCiudad, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al crear la ciudad: " + ciudad.NombreCiudad);
            }
        }

        public async Task<bool> Delete(int ciudadId)
        {
            try
            {
                string mensajeValidacion = string.Empty;
                if (ciudadId <= 0)
                    mensajeValidacion += "El ID de la ciudad es inválido ";

                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);

                bool resultado = await _ciudadRepositorio.Delete(ciudadId);
                if (!resultado)
                {
                    _logger.LogWarning("No se encontró la ciudad con ID: {CiudadId}", ciudadId);
                    return false;
                }

                _logger.LogInformation("Ciudad eliminada con éxito: {CiudadId}", ciudadId);
                return true;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al eliminar la ciudad: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la ciudad con ID: {CiudadId}. CorrelacionId {CorrelationId}. Mensaje {Message}", ciudadId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al eliminar la ciudad con ID: " + ciudadId);
            }
        }

        public async Task<List<Ciudad>> GetAll()
        {
            try
            {
                List<Ciudad> ciudades = await _ciudadRepositorio.GetAll();
                if (ciudades == null || ciudades.Count == 0)
                {
                    _logger.LogWarning("No se encontraron ciudades.");
                    throw new ValidacionException("No se encontraron ciudades.");
                }
                _logger.LogInformation("Se encontraron {Count} ciudades.", ciudades.Count);
                return ciudades;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las ciudades. CorrelacionId {CorrelationId}. Mensaje {Message}", GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener todas las ciudades.");
            }
        }

        public async Task<List<Ciudad>> GetByDepartamentoId(int departamentoId)
        {
            try
            {
                List<Ciudad> ciudades = await _ciudadRepositorio.GetByDepartamentoId(departamentoId);
                if (ciudades == null || ciudades.Count == 0)
                {
                    _logger.LogWarning("No se encontraron ciudades para el departamento con ID: {DepartamentoId}", departamentoId);
                    throw new ValidacionException("No se encontraron ciudades para el departamento con ID: " + departamentoId);
                }
                _logger.LogInformation("Se encontraron {Count} ciudades para el departamento con ID: {DepartamentoId}", ciudades.Count, departamentoId);
                return ciudades;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener ciudades para el departamento con ID: {DepartamentoId}. CorrelacionId {CorrelationId}. Mensaje {Message}", departamentoId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener ciudades para el departamento con ID: " + departamentoId);
            }
        }

        public async Task<Ciudad> GetById(int ciudadId)
        {
            try
            {
                Ciudad ciudad = await _ciudadRepositorio.GetById(ciudadId);
                if (ciudad == null)
                {
                    _logger.LogWarning("No se encontró la ciudad con ID: {CiudadId}", ciudadId);
                    throw new ValidacionException("No se encontró la ciudad con ID: " + ciudadId);
                }
                _logger.LogInformation("Ciudad encontrada: {NombreCiudad}", ciudad.NombreCiudad);
                return ciudad;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la ciudad con ID: {CiudadId}. CorrelacionId {CorrelationId}. Mensaje {Message}", ciudadId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener la ciudad con ID: " + ciudadId);
            }
        }

        public async Task<Ciudad> Update(Ciudad ciudad)
        {
            try
            {
                string mensajeValidacion = IsValid(ciudad);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);
                if(ciudad.CiudadId <= 0)
                    throw new ValidacionException("El ID de la ciudad es inválido.");

                Ciudad ciudadResult = await _ciudadRepositorio.Update(ciudad);
                if (ciudadResult == null)
                {
                    _logger.LogWarning("No se pudo actualizar la ciudad: {NombreCiudad}", ciudad.NombreCiudad);
                    throw new ValidacionException("No se pudo actualizar la ciudad: " + ciudad.NombreCiudad);
                }
                _logger.LogInformation("Ciudad actualizada con éxito: {NombreCiudad}", ciudad.NombreCiudad);
                return ciudadResult;
            }
            catch(ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al actualizar la ciudad: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la ciudad: {NombreCiudad}. CorrelacionId {CorrelationId}. Mensaje {Message}", ciudad.NombreCiudad, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al actualizar la ciudad: " + ciudad.NombreCiudad);
            }
        }

        private string GetCorrelacionId()
        {
            return _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A";
        }

        private string IsValid(Ciudad ciudad)
        {
            string mensajeValidacion = string.Empty;
            if (string.IsNullOrEmpty(ciudad.NombreCiudad))
                mensajeValidacion += "El nombre de la ciudad es inválido. ";
            if (ciudad.DepartamentoId <= 0)
                mensajeValidacion += "El ID del departamento es inválido. ";
            if (ciudad.NombreCiudad.Length > 100)
                mensajeValidacion += "El nombre de la ciudad no puede tener más de 100 caracteres. ";
            if (ciudad.NombreCiudad.Length < 0)
                mensajeValidacion += "El nombre de la ciudad no puede ser menor a 100 caracteres ";
            if (ciudad.NombreCiudad == "string")
                mensajeValidacion += "El nombre de la ciudad no puede ser 'string' ";

            return mensajeValidacion;
        }
    }
}
