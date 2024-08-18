using Microsoft.Extensions.Options;
using Minio;
using Minio.AspNetCore;
using PetProject.API.Middlewares;
using PetProject.API.Providers;
using PetProject.Application;
using PetProject.Application.Abstractions;
using PetProject.Infrastructure.Postgres;
using MinioOptions = PetProject.API.Providers.MinioOptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMinio(options =>
{
    var minioOptions = builder.Configuration.GetSection("Minio").Get<MinioOptions>();
    options.AccessKey = minioOptions!.AccessKey;
    options.Endpoint = minioOptions.Endpoint;
    options.SecretKey = minioOptions.SecretKey;
});


builder.Services.AddApplication();
builder.Services.AddPostgresInfrastructure(builder.Configuration);
builder.Services.AddScoped<IMinioProvider, MinioProvider>();


var app = builder.Build();

//app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();