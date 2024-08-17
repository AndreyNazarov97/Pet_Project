using Minio.AspNetCore;
using PetProject.API.Middlewares;
using PetProject.API.Providers;
using PetProject.Application;
using PetProject.Application.Abstractions;
using PetProject.Infrastructure.Postgres;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMinio(options =>
{
    options.Endpoint = builder.Configuration.GetValue<string>("Minio:Endpoint");
    options.AccessKey = builder.Configuration.GetValue<string>("Minio:AccessKey");
    options.SecretKey = builder.Configuration.GetValue<string>("Minio:SecretKey");
});

builder.Services.AddApplication();
builder.Services.AddPostgresInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IMinioProvider, MinioProvider>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();