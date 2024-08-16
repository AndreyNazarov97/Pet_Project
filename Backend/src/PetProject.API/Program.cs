using Microsoft.AspNetCore.HttpLogging;
using PetProject.Application;
using PetProject.Infrastructure.Postgres;
using Serilog;

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

builder.Services.AddApplication();
builder.Services.AddPostgresInfrastructure(builder.Configuration);


var app = builder.Build();

app.UseSerilogRequestLogging(); 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();