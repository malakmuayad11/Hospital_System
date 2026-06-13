using HospitalSystem.Extensions;
using HospitalSystem.Service.Interfaces;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Centralized configuration moved to extension methods
builder.Configuration.AddAzureKeyVaultIfConfigured();

builder.Services
    .AddControllers()
    .AddJsonOptions(o => {});

builder.Services.AddEndpointsApiExplorer();

// Compose feature groups
builder.Services.AddHospitalPersistence(builder.Configuration);
builder.Services.AddHospitalRepositories();
builder.Services.AddHospitalServices();
builder.Services.AddHospitalAuthAndPolicies(builder.Configuration);
builder.Services.AddHospitalSwagger();
builder.Services.AddHospitalRateLimiting();
builder.Services.AddHospitalCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("HospitalSystemApiCorsPolicy");

app.UseRateLimiter();

// Safe Message for rate limiter
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many attempts. Please try again later.");
    }
});

// Logging Forbidden Access
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        var loggerService = context.RequestServices.GetRequiredService<ILoggerService>();

        var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var userIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        int userId = int.TryParse(userIdClaim, out var id) ? id : 0;

        loggerService.Log(ip, userIdClaim, "Forbidden Access");
    }
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();