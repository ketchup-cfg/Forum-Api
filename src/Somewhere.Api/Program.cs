using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Somewhere.Core.Extensions;
using Microsoft.OpenApi.Models;
using Somewhere.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSomewhereFrontendApp", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidation();
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
    options.EnableAnnotations();
});

builder.Services.AddSomeDataServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseReDoc();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseCors("AllowSomewhereFrontendApp");
app.UseAuthorization();
app.MapControllers();

app.Run();