using System.Reflection;
using HeavyMetalMachine.Core.Extensions;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHeavyMetalDataServices();
builder.Services.AddFluentValidation(config => 
{ 
    config.RegisterValidatorsFromAssembly(Assembly.Load("HeavyMetalMachine.Core"));
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();