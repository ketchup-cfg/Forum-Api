using System.Reflection;
using Somewhere.Services.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", 
        new OpenApiInfo
        {
            Title = "Some API",
            Version = "v1",
            Description = "An API for a forum somewhere."
        });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddSomeDataServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();