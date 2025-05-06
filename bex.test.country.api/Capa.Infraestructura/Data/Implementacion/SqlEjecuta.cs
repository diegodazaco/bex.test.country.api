using bex.test.country.api.Capa.Infraestructura.Data.Interface;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System;
using bex.test.country.api.Capa.Infraestructura.Data.Queries;
using bex.test.country.api.Capa.Dominio.Enums;
using Newtonsoft.Json;

namespace bex.test.country.api.Capa.Infraestructura.Data.Implementacion
{
    public class SqlEjecuta : ISqlEjecuta
    {
        private const string DefaultConnection = "Server=DIEGO_DAZA\\LOCALDATABASE; Database=TestBexT;User ID=test;Password=test321*;Trusted_Connection=false; MultipleActiveResultSets=true; TrustServerCertificate=True;";

        public async Task<T> EjecutaGenericoPSAsync<T>(SqlSPEjecuta sqlSPExecute)
        {
            SqlConnection con;
            using (con = new SqlConnection(DefaultConnection))
            {
                con.Open();

                List<SqlParametros> parameters = new List<SqlParametros>();
                string procedureName = sqlSPExecute.NombreProcedimientoAlmacenado;
                if (sqlSPExecute.Parametros != null && sqlSPExecute.Parametros.Count > 0)
                {
                    parameters = sqlSPExecute.Parametros.Select(e => new SqlParametros()
                    {
                        TipoDato = e.TipoDato,
                        Nombre = e.Llave,
                        Valor = e.Valor,
                        Direccion = e.Direccion,
                        TipoNombre = e.TipoNombre
                    }).ToList();
                }

                SqlCommand cmd = GetStoredProcedureCmd(procedureName, parameters, con);
                int rowsAffected = 0;

                switch (sqlSPExecute.AccionSP)
                {
                    case AccionSP.Insert:
                        rowsAffected = await cmd.ExecuteNonQueryAsync();

                        ResultadoGenerico response = new ResultadoGenerico() { EsExitoso = rowsAffected == 0 ? false : true };

                        if (parameters.Any(e => e.Direccion == AccionDireccionParametro.Output))
                            response.Id = cmd.Parameters["@Id"].Value.ToString();

                        return (T)(object)response;

                    case AccionSP.Select:
                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        List<dynamic> resultList = new List<dynamic>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            dynamic objectResult = new ExpandoObject();
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                var res = dt.Columns[i];
                                AddProperty(objectResult, res.ColumnName, dr[res.ColumnName].ToString());
                            }
                            resultList.Add(objectResult);
                        }

                        string resulJsonList = JsonConvert.SerializeObject(resultList);
                        return (T)(object)JsonConvert.DeserializeObject<T>(resulJsonList)!;
                    case AccionSP.Update:
                        rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return (T)(object)new ResultadoGenerico() { EsExitoso = rowsAffected == 0 ? false : true };
                    case AccionSP.Delete:
                        rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return (T)(object)new ResultadoGenerico() { EsExitoso = rowsAffected == 0 ? false : true };
                    default:
                        return (T)(object)new ResultadoGenerico() { EsExitoso = false, Mensaje = "No fue posible ejecutar la consulta", TipoMensaje = TipoMensaje.Warning };
                }
            }
        }

        #region methods private
        private SqlCommand GetStoredProcedureCmd(string nameSP, List<SqlParametros> Parameters, SqlConnection con, SqlTransaction? trans = null)
        {
            DataTable dt = new DataTable();
            SqlCommand? cmd = null;

            if (trans != null)
                cmd = new SqlCommand(nameSP, con, trans);
            else
                cmd = new SqlCommand(nameSP, con);

            cmd.CommandType = CommandType.StoredProcedure;

            if (Parameters != null && Parameters.Count > 0)
            {
                foreach (var para in Parameters)
                {
                    SqlDbType type = GetType(para.TipoDato);

                    if (para.Direccion == AccionDireccionParametro.Output)
                    {
                        cmd.Parameters.AddWithValue(para.Nombre, type).Value = para.Valor;
                        cmd.Parameters[para.Nombre].Direction = ParameterDirection.Output;
                    }
                    else
                    {
                        if (para.Valor != null)
                        {
                            if (para.TipoDato.ToLower().Contains("datetime"))
                                cmd.Parameters.AddWithValue(para.Nombre, type).Value = DateTime.Parse(para.Valor.ToString() ?? "");
                            else
                                cmd.Parameters.AddWithValue(para.Nombre, type).Value = para.Valor;
                        }
                        else
                            cmd.Parameters.AddWithValue(para.Nombre, type).Value = DBNull.Value;
                    }
                }
            }

            return cmd;
        }

        private bool FindDefaultDelete(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                return bool.Parse(dr["IsDeleted"].ToString() ?? "");
            }

            return false;
        }

        private static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        private SqlDbType GetType(string value)
        {
            value = value.Trim();
            value = value.ToLower();

            switch (value.ToString())
            {
                case "smallint":
                    return SqlDbType.SmallInt;
                case "int":
                    return SqlDbType.Int;
                case "int32":
                    return SqlDbType.Int;
                case "int64":
                    return SqlDbType.BigInt;
                case "long":
                    return SqlDbType.BigInt;
                case "short":
                    return SqlDbType.SmallInt;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "varchar":
                    return SqlDbType.VarChar;
                case "string":
                    return SqlDbType.VarChar;
                case "datetime":
                    return SqlDbType.DateTime;
                case "bit":
                    return SqlDbType.Bit;
                case "bool":
                    return SqlDbType.Bit;
                case "datetime2":
                    return SqlDbType.DateTime2;
                case "bigint":
                    return SqlDbType.BigInt;
                case "decimal":
                    return SqlDbType.Decimal;
                case "guid":
                    return SqlDbType.UniqueIdentifier;
                case "structured":
                    return SqlDbType.Structured;
                case "char":
                    return SqlDbType.Char;
                case "money":
                    return SqlDbType.Money;
                case "date":
                    return SqlDbType.Date;
                case "nchar":
                    return SqlDbType.NChar;
                case "smalldatetime":
                    return SqlDbType.SmallDateTime;
                default:
                    throw new Exception(string.Format("Method exception. Date: {0}"));
            }
        }
        #endregion
    }
}
