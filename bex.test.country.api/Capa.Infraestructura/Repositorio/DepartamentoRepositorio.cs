using bex.test.country.api.Capa.Dominio.Entidades;
using bex.test.country.api.Capa.Dominio.Enums;
using bex.test.country.api.Capa.Dominio.Interfaces;
using bex.test.country.api.Capa.Infraestructura.Data.Interface;
using bex.test.country.api.Capa.Infraestructura.Data.Queries;
using System;

namespace bex.test.country.api.Capa.Infraestructura.Repositorio
{
    public class DepartamentoRepositorio : IDepartamentoRepositorio
    {
        private readonly ISqlEjecuta _sqlExecute;

        public DepartamentoRepositorio(ISqlEjecuta sqlExecute)
        {
            _sqlExecute = sqlExecute;
        }

        public async Task<Departamento> Create(Departamento departamento)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "string", Llave = "@NombreDepartamento", Valor = departamento.NombreDepartamento, Direccion = AccionDireccionParametro.Input },
                new ParametroGenerico { TipoDato = "int", Llave = "@PaisId", Valor = departamento.PaisId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_Create",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Insert
            };
            ResultadoGenerico resultadoGenerico = await _sqlExecute.EjecutaGenericoPSAsync<ResultadoGenerico>(sqlSPExecute);
            return departamento;
        }

        public async Task<bool> Delete(int departamentoId)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@DepartamentoId", Valor = departamentoId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_Delete",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Delete
            };
            ResultadoGenerico resultadoGenerico = await _sqlExecute.EjecutaGenericoPSAsync<ResultadoGenerico>(sqlSPExecute);
            return resultadoGenerico.EsExitoso;
        }

        public async Task<List<Departamento>> GetAll()
        {
            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_GetAll",
                Parametros = new(),
                AccionSP = AccionSP.Select
            };
            return await _sqlExecute.EjecutaGenericoPSAsync<List<Departamento>>(sqlSPExecute);
        }

        public async Task<Departamento> GetById(int departamentoId)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@DepartamentoId", Valor = departamentoId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_GetById",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Select
            };
            List<Departamento> departamentos = await _sqlExecute.EjecutaGenericoPSAsync<List<Departamento>>(sqlSPExecute);
            return departamentos.FirstOrDefault();
        }

        public async Task<Departamento> GetByNombre(string nombreDepartamento)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "string", Llave = "@NombreDepartamento", Valor = nombreDepartamento, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_GetByNombre",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Select
            };
            List<Departamento> departamentos = await _sqlExecute.EjecutaGenericoPSAsync<List<Departamento>>(sqlSPExecute);
            return departamentos.FirstOrDefault();
        }

        public async Task<List<Departamento>> GetByPaisId(int paisId)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@PaisId", Valor = paisId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_GetByPaisId",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Select
            };
            return await _sqlExecute.EjecutaGenericoPSAsync<List<Departamento>>(sqlSPExecute);
        }

        public async Task<Departamento> Update(Departamento departamento)
        {
            List<ParametroGenerico> paramGenerics = new List<ParametroGenerico>
            {
                new ParametroGenerico { TipoDato = "int", Llave = "@DepartamentoId", Valor = departamento.DepartamentoId, Direccion = AccionDireccionParametro.Input },
                new ParametroGenerico { TipoDato = "string", Llave = "@NombreDepartamento", Valor = departamento.NombreDepartamento, Direccion = AccionDireccionParametro.Input },
                new ParametroGenerico { TipoDato = "int", Llave = "@PaisId", Valor = departamento.PaisId, Direccion = AccionDireccionParametro.Input }
            };

            SqlSPEjecuta sqlSPExecute = new()
            {
                NombreProcedimientoAlmacenado = "Departamento_Update",
                Parametros = paramGenerics,
                AccionSP = AccionSP.Update
            };
            ResultadoGenerico resultadoGenerico = await _sqlExecute.EjecutaGenericoPSAsync<ResultadoGenerico>(sqlSPExecute);
            return resultadoGenerico.EsExitoso ? departamento : null! ;
        }
    }
}
