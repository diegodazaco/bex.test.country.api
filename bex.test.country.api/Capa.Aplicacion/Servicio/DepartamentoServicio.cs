using bex.test.country.api.Capa.Aplicacion.Common;
using bex.test.country.api.Capa.Aplicacion.DTOs;
using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Dominio.Entidades;
using bex.test.country.api.Capa.Dominio.Interfaces;
using Serilog.Context;

namespace bex.test.country.api.Capa.Aplicacion.Servicio
{
    public class DepartamentoServicio : IDepartamentoServicio
    {
        private readonly IDepartamentoRepositorio _departamentoRepositorio;
        private readonly ILogger<DepartamentoServicio> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaisRepositorio _paisRepositorio;

        public DepartamentoServicio(IDepartamentoRepositorio departamentoRepositorio, ILogger<DepartamentoServicio> logger, IHttpContextAccessor httpContextAccessor, IPaisRepositorio paisRepositorio)
        {
            _departamentoRepositorio = departamentoRepositorio;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _paisRepositorio = paisRepositorio;
            LogContext.PushProperty("Application", "bex.test.country.api - DepartamentoServicio");
        }

        public async Task<Departamento> Create(Departamento departamento)
        {
            try
            {
                string mensajeValidacion = IsValid(departamento);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);

                if (await _departamentoRepositorio.GetByNombre(departamento.NombreDepartamento) != null)
                    throw new ValidacionException("El departamento ya existe.");

                if (await _paisRepositorio.GetById(departamento.PaisId) == null)
                    throw new ValidacionException("El país no existe.");

                Departamento departamentoResult = await _departamentoRepositorio.Create(departamento);
                _logger.LogInformation("Departamento creado con éxito: {NombreDepartamento}", departamento.NombreDepartamento);
                return departamentoResult;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al crear el departamento: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el departamento: {Nombre}. CorrelacionId {CorrelationId}. Mensaje {Message}", departamento.NombreDepartamento, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al crear el departamento: " + departamento.NombreDepartamento);
            }
        }

        public async Task<bool> Delete(int departamentoId)
        {
            try
            {
                string mensajeValidacion = string.Empty;
                if (departamentoId <= 0)
                    mensajeValidacion += "El ID del departamento es inválido ";

                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);

                bool resultado = await _departamentoRepositorio.Delete(departamentoId);
                if (!resultado)
                {
                    _logger.LogWarning("No se pudo eliminar el departamento: {DepartamentoId}", departamentoId);
                    return false;
                }

                _logger.LogInformation("Departamento eliminado con éxito: {DepartamentoId}", departamentoId);
                return true;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al eliminar el departamento: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el departamento: {DepartamentoId}. CorrelacionId {CorrelationId}. Mensaje {Message}", departamentoId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al eliminar el departamento: " + departamentoId);
            }
        }

        public async Task<List<DepartamentoDTO>> GetAll()
        {
            try
            {
                List<DepartamentoDTO> departamentos = await _departamentoRepositorio.GetAll();
                if (departamentos == null || departamentos.Count == 0)
                {
                    _logger.LogWarning("No se encontraron departamentos.");
                    throw new ValidacionException("No se encontraron departamentos.");
                }
                _logger.LogInformation("Se encontraron {Count} departamentos.", departamentos.Count);
                return departamentos;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los departamentos. CorrelacionId {CorrelationId}. Mensaje {Message}", GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener todos los departamentos.");
            }
        }

        public async Task<DepartamentoDTO> GetById(int departamentoId)
        {
            try
            {
                DepartamentoDTO? departamento = await _departamentoRepositorio.GetById(departamentoId);
                if (departamento == null)
                {
                    _logger.LogWarning("No se encontró el departamento con ID: {DepartamentoId}", departamentoId);
                    throw new ValidacionException("No se encontró el departamento con ID: " + departamentoId);
                }
                _logger.LogInformation("Departamento encontrado: {NombreDepartamento}", departamento.NombreDepartamento);
                return departamento;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el departamento por ID: {DepartamentoId}. CorrelacionId {CorrelationId}. Mensaje {Message}", departamentoId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener el departamento por ID: " + departamentoId);
            }
        }

        public async Task<Departamento> GetByNombre(string nombreDepartamento)
        {
            try
            {
                Departamento? departamento = await _departamentoRepositorio.GetByNombre(nombreDepartamento);
                if (departamento == null)
                {
                    _logger.LogWarning("No se encontró el departamento con nombre: {NombreDepartamento}", nombreDepartamento);
                    throw new ValidacionException("No se encontró el departamento con nombre: " + nombreDepartamento);
                }
                _logger.LogInformation("Departamento encontrado: {NombreDepartamento}", departamento.NombreDepartamento);
                return departamento;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el departamento por nombre: {NombreDepartamento}. CorrelacionId {CorrelationId}. Mensaje {Message}", nombreDepartamento, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener el departamento por nombre: " + nombreDepartamento);
            }
        }

        public async Task<List<DepartamentoDTO>> GetByPaisId(int paisId)
        {
            try
            {
                List<DepartamentoDTO>? departamentos = await _departamentoRepositorio.GetByPaisId(paisId);
                if (departamentos == null || departamentos.Count == 0)
                {
                    _logger.LogWarning("No se encontró el departamentos con ID de país: {PaisId}", paisId);
                    throw new ValidacionException("No se encontró el departamentos con ID de país: " + paisId);
                }
                _logger.LogInformation("Se encontraron {Count} departamentos.", departamentos.Count);
                return departamentos;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("{Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el departamentos por ID de país: {PaisId}. CorrelacionId {CorrelationId}. Mensaje {Message}", paisId, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al obtener el departamentos por ID de país: " + paisId);
            }
        }

        public async Task<Departamento> Update(Departamento departamento)
        {
            try
            {
                string mensajeValidacion = IsValid(departamento);
                if (!string.IsNullOrEmpty(mensajeValidacion))
                    throw new ValidacionException(mensajeValidacion);
                if(departamento.DepartamentoId <= 0)
                    throw new ValidacionException("El ID del departamento es inválido.");

                Departamento departamentoResult = await _departamentoRepositorio.Update(departamento);
                if (departamentoResult == null)
                {
                    _logger.LogWarning("No se pudo actualizar el departamento: {NombreDepartamento}", departamento.NombreDepartamento);
                    throw new ValidacionException("No se pudo actualizar el departamento: " + departamento.NombreDepartamento);
                }
                _logger.LogInformation("Departamento actualizado con éxito: {NombreDepartamento}", departamento.NombreDepartamento);
                return departamentoResult;
            }
            catch (ValidacionException vex)
            {
                _logger.LogWarning("Validación fallida al actualizar el departamento: {Message}", vex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el departamento: {Nombre}. CorrelacionId {CorrelationId}. Mensaje {Message}", departamento.NombreDepartamento, GetCorrelacionId(), ex.Message);
                throw new Exception("Error al actualizar el departamento: " + departamento.NombreDepartamento);
            }
        }

        private string GetCorrelacionId()
        {
            return _httpContextAccessor.HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A";
        }

        private string IsValid(Departamento departamento)
        {
            string mensajeValidacion = string.Empty;
            if (departamento == null)
                mensajeValidacion += "El departamento no puede ser nulo. ";
            if (string.IsNullOrEmpty(departamento.NombreDepartamento))
                mensajeValidacion += "El nombre del departamento no puede estar vacío. ";
            if (departamento.PaisId <= 0)
                mensajeValidacion += "El ID del país es inválido. ";
            if (departamento.NombreDepartamento.Length > 100)
                mensajeValidacion += "El nombre del departamento no puede tener más de 100 caracteres. ";
            if (departamento.NombreDepartamento.Length < 0)
                mensajeValidacion += "El nombre del departamento no puede ser menor a 100 caracteres ";
            if (departamento.NombreDepartamento == "string")
                mensajeValidacion += "El nombre del país no puede ser 'string' ";

            return mensajeValidacion;
        }
    }
}
