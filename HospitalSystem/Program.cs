using Azure.Identity;
using HospitalSystem.Data.Data;
using HospitalSystem.Infrastructure.Authorization.Handlers;
using HospitalSystem.Infrastructure.Authorization.Requirements;
using HospitalSystem.Repository.Classes;
using HospitalSystem.Repository.Interfaces;
using HospitalSystem.Service.Classes;
using HospitalSystem.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load Azure Key Vault

var keyVaultUrl = builder.Configuration["KeyVault:Url"];

if (!string.IsNullOrWhiteSpace(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Register Swagger generator and customize its behavior.
builder.Services.AddSwaggerGen(options =>
{
    // Define the JWT Bearer security scheme
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    // Require the Bearer scheme for secured endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
                // Reference the previously defined "Bearer" security scheme.
                Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
            new string[] {}
    }
  });
});

// DbContext
builder.Services.AddDbContext<HospitalSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HospitalDB")));

// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IBillingRepository, BillingRepository>();

// Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IConsultationService, ConsultationService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IPasswordHasher, BcryptHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("HospitalSystemApiCorsPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://127.0.0.1:5500",
                "http://localhost:5109"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 🔹 JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration["JwtSigningKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new Exception("JWT Signing Key is not found in Azure Key Vault.");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "HospitalSystemApi",
            ValidAudience = "HospitalSystemApiUsers",

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            ),
            RoleClaimType = ClaimTypes.Role
        };
    });

// 🔹 Adding Policies
builder.Services.AddSingleton<IAuthorizationHandler, UserOwnerOrAdminHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HasUserPermissionsHandler>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOwnerOrAdmin", policy =>
        policy.Requirements.Add(new UserOwnerOrAdminRequirement()));

    options.AddPolicy("AddEditDoctors", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eAddEditDoctors)));

    options.AddPolicy("ManagePatients", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManagePatients)));

    options.AddPolicy("ManageAppointments", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManageAppointments)));

    options.AddPolicy("ManagePayments", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManagePayments)));

    options.AddPolicy("ShowMedicalRecords", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eShowMedicalRecords)));

    options.AddPolicy("AddEditMedicalRecords", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eAddEditMedicalRecords)));
    
    options.AddPolicy("ManageUsers", policy =>
        policy.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManageUsers)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("HospitalSystemApiCorsPolicy");


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
