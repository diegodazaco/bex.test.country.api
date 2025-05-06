using bex.test.country.api.Configuracion;

var builder = WebApplication.CreateBuilder(args);
builder.RegisterServices();
var app = builder.Build();

app.RegisterMiddlewares();
app.Run();