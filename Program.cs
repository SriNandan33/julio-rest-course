
using Asp.Versioning;
using GameStore.Data;
using GameStore.Endpoints;
using GameStore.ErrorHandling;
using GameStore.Middlewares;
using GameStore.Repositories;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new QueryStringApiVersionReader("v");
    options.ReportApiVersions = true;
});

builder.Services.AddScoped<IGamesRepository, GamesRepository>();

var connString = builder.Configuration.GetConnectionString("GameStoreDatabase");
builder.Services.AddSqlServer<GameStoreContext>(connString);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GamesRead", policy => policy.RequireClaim("scope", "games:read"));
    options.AddPolicy("GamesWrite", policy => policy.RequireClaim("scope", "games:write"));
});

var app = builder.Build();

app.UseExceptionHandler(exceptionHandler => exceptionHandler.ConfigureExceptionHandler());

app.UseMiddleware<RequestTimingMiddleWare>();

app.UseHttpLogging();

app.Services.InitializeDb();

app.MapGamesEndpoints();

app.Run();
