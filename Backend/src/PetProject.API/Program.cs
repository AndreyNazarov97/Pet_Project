using Microsoft.AspNetCore.HttpLogging;
using Minio.AspNetCore;
using PetProject.API.Middlewares;
using PetProject.API.Providers;
using PetProject.API.Validation;
using PetProject.Application;
using PetProject.Application.Abstractions;
using PetProject.Infrastructure.Postgres;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using MinioOptions = PetProject.API.Providers.MinioOptions;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = options.LoggingFields |
                            HttpLoggingFields.RequestBody |
                            HttpLoggingFields.RequestHeaders |
                            HttpLoggingFields.ResponseBody |
                            HttpLoggingFields.ResponseHeaders;
});



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

builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

builder.Services.AddApplication();
builder.Services.AddPostgresInfrastructure();

builder.Services.AddScoped<IMinioProvider, MinioProvider>();


var app = builder.Build();

app.UseSerilogRequestLogging(); 


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