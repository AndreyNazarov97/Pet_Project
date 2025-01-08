using Amazon.S3;
using FileService.Endpoints;
using FileService.Extensions;
using FileService.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);
builder.Services.AddHttpLogging(options => { options.CombineLogs = true; });
builder.Services.AddSerilog();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 2000 * 1024 * 1024; // 2000 MB
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddEndpoints();
builder.Services.AddCors();

builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddHangfirePostgres(builder.Configuration);
builder.Services.AddMinio(builder.Configuration);
builder.Services.AddRepositories();

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

app.UseSerilogRequestLogging();
app.UseExceptionHandling();

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