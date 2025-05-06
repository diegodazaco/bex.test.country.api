using bex.test.country.api.Capa.Aplicacion.Common;
using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Dominio.Entidades;
using bex.test.country.api.Capa.Dominio.Interfaces;
using Serilog.Context;

namespace bex.test.country.api.Capa.Aplicacion.Servicio
{
    public class PaisServicio : IPaisServicio
    {
        private readonly IPaisRepositorio _paisRepositorio;
        private readonly ILogger<PaisServicio> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaisServicio(IPaisRepositorio paisRepositorio, ILogger<PaisServicio> logger, IHttpContextAccessor httpContextAccessor)
        {
            _paisRepositorio = paisRepositorio;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            LogContext.PushProperty("Application", "bex.test.country.api - PaisServicio");
        }

        public async Task<Pais> Create(Pais pais)
        {
            try
            {
                string mensajeValidacion = IsValid(pais);

                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);

                if (await _paisRepositorio.GetByNombre(pais.NombrePais) != null)
                    throw new ValidacionException("El país ya existe.");

                Pais paisResult = await _paisRepositorio.Create(pais);
                _logger.LogInformation("País creado con éxito: {NombrePais}", pais.NombrePais);
                return paisResult;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al crear el país: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el país: {Nombre}. CorrelacionId {CorrelationId}. Mensaje {Message}", pais.NombrePais, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al crear el país: " + pais.NombrePais);
            }
        }

        public async Task<bool> Delete(int paisId)
        {
            try
            {
                string mensajeValidacion = string.Empty;
                if (paisId <= 0)
                    mensajeValidacion += "El ID del país es inválido ";

                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);

                bool resultado = await _paisRepositorio.Delete(paisId);
                if (!resultado)
                {
                    _logger.LogWarning("No se encontró el país con ID: {PaisId}", paisId);
                    return false;
                }
                
                _logger.LogInformation("País eliminado con éxito: {PaisId}", paisId);
                return true;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al eliminar el país: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al eliminar el país con ID: {PaisId}. CorrelacionId {CorrelationId}. Mensaje {Message}", paisId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al eliminar el país con ID: " + paisId);
            }
        }

        public async Task<List<Pais>> GetAll()
        {
            try
            {
                List<Pais> paises = await _paisRepositorio.GetAll();
                if (paises == null || !paises.Any())
                {
                    _logger.LogWarning("No se encontraron países.");
                    throw new ValidacionException("No se encontraron países.");
                }
                _logger.LogInformation("Se encontraron {Count} países.", paises.Count);
                return paises;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de países. CorrelacionId {CorrelationId}. Mensaje {Message}", GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener la lista de países");
            }
        }

        public async Task<Pais> GetById(int paisId)
        {
            try
            {
                Pais? pais = await _paisRepositorio.GetById(paisId);
                if (pais == null)
                {
                    _logger.LogWarning("No se encontró el país con ID: {PaisId}", paisId);
                    throw new ValidacionException("No se encontró el país con ID: " + paisId);
                }
                _logger.LogInformation("Se encontró el país con ID: {PaisId}", paisId);
                return pais;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el país con ID: {PaisId}. CorrelacionId {CorrelationId}. Mensaje {Message}", paisId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener el país con ID: " + paisId);
            }
        }

        public async Task<Pais> Update(Pais pais)
        {
            try
            {
                string mensajeValidacion = IsValid(pais);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);
                if (pais.PaisId <= 0)
                    throw new ValidacionException("El ID del país es inválido.");

                Pais paisResult = await _paisRepositorio.Update(pais);
                if (paisResult == null)
                {
                    _logger.LogWarning("No se encontró el país con ID: {PaisId}", pais.PaisId);
                    throw new ValidacionException("No se encontró el país con ID: " + pais.PaisId);
                }
                _logger.LogInformation("País actualizado con éxito: {NombrePais}", pais.NombrePais);
                return paisResult;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al actualizar el país: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el país: {Nombre}. CorrelacionId {CorrelationId}. Mensaje {Message}", pais.NombrePais, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al actualizar el país: " + pais.NombrePais);
            }
        }

        private string GetCorrelacionId()
        {
            return _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A";
        }

        private string IsValid(Pais pais)
        {
            string mensajeValidacion = string.Empty;
            if (string.IsNullOrEmpty(pais.NombrePais))
                mensajeValidacion += "El nombre del país es obligatorio ";
            if (pais.NombrePais.Length > 100)
                mensajeValidacion += "El nombre del país no puede tener más de 100 caracteres ";
            if (pais.NombrePais.Length < 0)
                mensajeValidacion += "El nombre del país no puede ser menor a 100 caracteres ";
            if (pais.NombrePais == "string")
                mensajeValidacion += "El nombre del país no puede ser 'string' ";

            return mensajeValidacion;
        }
    }
}
