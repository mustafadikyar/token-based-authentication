using token_based_authentication.ActionFilters;
using token_based_authentication.HHandlers;
using token_based_authentication.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(PublicTokenAuthOptions.DefaultSchemeName)
    .AddScheme<PublicTokenAuthOptions, PublicTokenAuthHandler>(PublicTokenAuthOptions.DefaultSchemeName, opts => { });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Token Based Authentication",
        Description = $"<b>Env:</b> {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}<br/>" +
                      $"<b>Upd:</b> {File.GetLastWriteTimeUtc(Assembly.GetExecutingAssembly().Location):dd.MM.yyyy HH:mm:ss}"
    });
    c.OperationFilter<CustomHeaderSwagger>();
    c.CustomSchemaIds(type => type.FullName);
});


builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
