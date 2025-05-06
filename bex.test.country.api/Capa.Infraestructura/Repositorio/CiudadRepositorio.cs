using bex.test.country.api.Capa.Dominio.Entidades;
using bex.test.country.api.Capa.Dominio.Enums;
using bex.test.country.api.Capa.Dominio.Interfaces;
using bex.test.country.api.Capa.Infraestructura.Data.Interface;
using bex.test.country.api.Capa.Infraestructura.Data.Queries;

namespace bex.test.country.api.Capa.Infraestructura.Repositorio
{
    public class CiudadRepositorio : ICiudadRepositorio
    {
        private readonly ISqlEjecuta _sqlExecute;

        public CiudadRepositorio(ISqlEjecuta sqlExecute)
        {
            _sqlExecute = sqlExecute;
        }

        public async Task<Ciudad> Create(Ciudad ciudad)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@DepartamentoId", Valor = ciudad.DepartamentoId, Direccion = AccionDireccionParametro.Input },
                new ParametroGenerico { TipoDato = "string", Llave = "@NombreCiudad", Valor = ciudad.NombreCiudad, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_Create",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Insert
            };
            ResultadoGenerico resultadoGenerico = await _sqlExecute.EjecutaGenericoPSAsync<ResultadoGenerico>(sqlSPExecute);
            return ciudad;
        }

        public async Task<bool> Delete(int ciudadId)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@CiudadId", Valor = ciudadId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_Delete",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Delete
            };
            ResultadoGenerico resultadoGenerico = await _sqlExecute.EjecutaGenericoPSAsync<ResultadoGenerico>(sqlSPExecute);
            return resultadoGenerico.EsExitoso;
        }

        public async Task<List<Ciudad>> GetAll()
        {
            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_GetAll",
                Parametros = new(),
                AccionSP = AccionSP.Select
            };
            return await _sqlExecute.EjecutaGenericoPSAsync<List<Ciudad>>(sqlSPExecute);
        }

        public async Task<List<Ciudad>> GetByDepartamentoId(int departamentoId)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@DepartamentoId", Valor = departamentoId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_GetByDepartamentoId",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Select
            };
            return await _sqlExecute.EjecutaGenericoPSAsync<List<Ciudad>>(sqlSPExecute);
        }

        public async Task<Ciudad> GetById(int ciudadId)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@CiudadId", Valor = ciudadId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_GetById",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Select
            };
            List<Ciudad> ciudades = await _sqlExecute.EjecutaGenericoPSAsync<List<Ciudad>>(sqlSPExecute);
            return ciudades.FirstOrDefault();
        }

        public async Task<Ciudad> GetByNombre(string nombreCiudad)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "string", Llave = "@NombreCiudad", Valor = nombreCiudad, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_GetByNombre",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Select
            };
            List<Ciudad> ciudades = await _sqlExecute.EjecutaGenericoPSAsync<List<Ciudad>>(sqlSPExecute);
            return ciudades.FirstOrDefault();
        }

        public async Task<Ciudad> Update(Ciudad ciudad)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@CiudadId", Valor = ciudad.CiudadId, Direccion = AccionDireccionParametro.Input },
                new ParametroGenerico { TipoDato = "int", Llave = "@DepartamentoId", Valor = ciudad.DepartamentoId, Direccion = AccionDireccionParametro.Input },
                new ParametroGenerico { TipoDato = "string", Llave = "@NombreCiudad", Valor = ciudad.NombreCiudad, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Ciudad_Update",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Update
            };
            ResultadoGenerico resultadoGenerico = await _sqlExecute.EjecutaGenericoPSAsync<ResultadoGenerico>(sqlSPExecute);
            return resultadoGenerico.EsExitoso ? ciudad : null!;
        }
    }
}
