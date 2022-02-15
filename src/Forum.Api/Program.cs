using Forum.Data;
using Forum.Data.Interfaces;
using Forum.Data.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IDatabase, Database>();
builder.Services.AddScoped<ITopics, Topics>();
var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();