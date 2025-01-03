using Amazon.S3;
using FileService.Endpoints;
using FileService.Extensions;
using FileService.Infrastructure;
using FileService.Infrastructure.Repositories;
using Hangfire;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints();

builder.Services.AddCors();

builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddHangfirePostgres(builder.Configuration);
builder.Services.AddMinio(builder.Configuration);

builder.Services.AddSingleton<IAmazonS3>(_ =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = "http://localhost:9000",
        ForcePathStyle = true,
        UseHttp = true
    };

    return new AmazonS3Client("minio_admin", "minio_password", config);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHangfireServer();
app.UseHangfireDashboard();


app.MapEndpoints();

app.Run();