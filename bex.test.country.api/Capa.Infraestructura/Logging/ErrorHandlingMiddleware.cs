using Serilog.Context;

namespace bex.test.country.api.Capa.Infraestructura.Logging
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlacionId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? Guid.NewGuid().ToString();
            context.Items["CorrelationId"] = correlacionId;

            var usuario = context.User.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anónimo";

            context.Items["UserName"] = usuario;
            context.Response.Headers.Add("X-Correlation-ID", correlacionId);

            try
            {
                LogContext.PushProperty("CorrelationId", correlacionId);
                LogContext.PushProperty("UserName", usuario);
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado: {Message}", ex.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Error inesperado",
                    correlacionId
                });
            }
        }
    }
}
