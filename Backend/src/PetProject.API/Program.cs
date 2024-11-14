using Microsoft.AspNetCore.HttpLogging;
using PetProject.API.Extensions;
using PetProject.API.Middlewares;
using PetProject.API.Validation;
using PetProject.Application;
using PetProject.Infrastructure.Postgres;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

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

builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.OverrideDefaultResultFactoryWith<CustomResultFactory>();
});

builder.Services.AddApplication();
builder.Services
    .AddMinioInfrastructure(builder.Configuration)
    .AddPostgresInfrastructure();




var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();