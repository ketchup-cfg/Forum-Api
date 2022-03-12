using System.Reflection;
using Forum.Services.Extensions;
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
            Title = "Forum API",
            Version = "v1"
        });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddForumDataServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();