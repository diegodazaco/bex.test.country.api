using bex.test.country.api.Capa.Aplicacion.Interfaces;
using bex.test.country.api.Capa.Aplicacion.Servicio;
using bex.test.country.api.Capa.Dominio.Interfaces;
using bex.test.country.api.Capa.Infraestructura.Data;
using bex.test.country.api.Capa.Infraestructura.Data.Implementacion;
using bex.test.country.api.Capa.Infraestructura.Data.Interface;
using bex.test.country.api.Capa.Infraestructura.Logging;
using bex.test.country.api.Capa.Infraestructura.Repositorio;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;

namespace bex.test.country.api.Configuracion
{
    public static class Config
    {
        private const string DefaultConnectionString = "Server=DIEGO_DAZA\\LOCALDATABASE;Database=TestBexT;User Id=test;Password=test321*;TrustServerCertificate=false;MultipleActiveResultSets=true;TrustServerCertificate=true;";

        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region DataContext EntityFramework
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(DefaultConnectionString));
            #endregion DataContext EntityFramework

            #region Configuración del Logger con Serilog
            var columnOptions = new ColumnOptions();
            columnOptions.Store.Clear();
            columnOptions.Store.Add(StandardColumn.Message);
            columnOptions.Store.Add(StandardColumn.MessageTemplate);
            columnOptions.Store.Add(StandardColumn.Level);
            columnOptions.Store.Add(StandardColumn.TimeStamp);
            columnOptions.Store.Add(StandardColumn.Exception);
            columnOptions.Store.Add(StandardColumn.Properties);
            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn { ColumnName = "Application", DataType = SqlDbType.NVarChar, DataLength = 100 },
                new SqlColumn { ColumnName = "UserName", DataType = SqlDbType.NVarChar, DataLength = 30 },
                new SqlColumn { ColumnName = "CorrelationId", DataType = SqlDbType.NVarChar, DataLength = 100 },
            };


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(
                    connectionString: DefaultConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", SchemaName = "dbo", BatchPostingLimit = 1, AutoCreateSqlTable = false },
                    columnOptions: columnOptions)
                .CreateLogger();

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            Log.Information("Inicia la API Country correctamente");
            #endregion Configuración del Logger con Serilog

            #region Register Dependency Injection - Services
            builder.Services.AddScoped<IPaisServicio, PaisServicio>();
            builder.Services.AddScoped<IDepartamentoServicio, DepartamentoServicio>();
            builder.Services.AddScoped<ICiudadServicio, CiudadServicio>();
            #endregion Register Dependency Injection - Services

            #region Register Dependency Injection - Repositories
            builder.Services.AddScoped<IPaisRepositorio, PaisRepositorio>();
            builder.Services.AddScoped<IDepartamentoRepositorio, DepartamentoRepositorio>();
            builder.Services.AddScoped<ICiudadRepositorio, CiudadRepositorio>();
            #endregion Register Dependency Injection - Repositories

            #region Register Dependency Injection - Data
            builder.Services.AddScoped<ISqlEjecuta, SqlEjecuta>();
            #endregion Register Dependency Injection - Data

            #region Policies Angular
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            #endregion Policies Angular



            builder.Services.AddHttpContextAccessor();

        }

        public static void RegisterMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
