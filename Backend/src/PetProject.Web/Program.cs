using DotNetEnv;
using PetProject.Accounts.Application;
using PetProject.Accounts.Infrastructure;
using PetProject.Accounts.Presentation;
using PetProject.SpeciesManagement.Application;
using PetProject.SpeciesManagement.Infrastructure;
using PetProject.VolunteerManagement.Application;
using PetProject.VolunteerManagement.Infrastructure;
using PetProject.Web.Extensions;
using PetProject.Web.Middlewares;
using Serilog;

Env.Load(); 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);
builder.Services.AddHttpLogging(options => { options.CombineLogs = true; });
builder.Services.AddSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services
    .AddVolunteerManagementApplication()
    .AddVolunteerPostgresInfrastructure(builder.Configuration);

builder.Services
    .AddSpeciesManagementApplication()
    .AddSpeciesManagementInfrastructure();


builder.Services
    .AddAccountsManagementApplication()
    .AddAccountsInfrastructure(builder.Configuration)
    .AddAccountsPresentation();


var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()
    || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger");
            return;
        }

        await next();
    });
    
    await app.ApplyMigrations();
    await app.SeedDatabases();
}

app.UseCors(config =>
{
    config
        .WithOrigins("http://localhost:5173")
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();