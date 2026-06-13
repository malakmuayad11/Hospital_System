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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace HospitalSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IConfiguration AddAzureKeyVaultIfConfigured(this IConfiguration config)
        {
            var keyVaultUrl = config["KeyVault:Url"];
            if (!string.IsNullOrWhiteSpace(keyVaultUrl))
            {
                if (config is ConfigurationManager mgr)
                {
                    mgr.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
                    return mgr;
                }

                var builder = new ConfigurationBuilder()
                    .AddConfiguration(config)
                    .AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());
                return builder.Build();
            }
            return config;
        }

        public static IServiceCollection AddHospitalPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connSecret = configuration["ConnectionString"];
            var connectionString = !string.IsNullOrWhiteSpace(connSecret)
                ? connSecret
                : configuration.GetConnectionString("HospitalDB");

            services.AddDbContext<HospitalSystemContext>(opt =>
                opt.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddHospitalRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();
            services.AddScoped<IConsultationRepository, ConsultationRepository>();
            services.AddScoped<IBillingRepository, BillingRepository>();
            services.AddScoped<IUsersTokensRepository, UsersTokensRepository>();
            return services;
        }

        public static IServiceCollection AddHospitalServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IMedicalRecordService, MedicalRecordService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IConsultationService, ConsultationService>();
            services.AddScoped<IBillingService, BillingService>();
            services.AddScoped<IPasswordHasherService, BcryptHasherService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUsersTokensService, UsersTokensService>();
            services.AddScoped<ILoggerService, NotepadLoggerService>();
            return services;
        }

        public static IServiceCollection AddHospitalAuthAndPolicies(this IServiceCollection services, IConfiguration config)
        {
            // JWT
            var secretKey = config["JwtSigningKey"];
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new Exception("JWT Signing Key is not found in configuration.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "HospitalSystemApi",
                        ValidAudience = "HospitalSystemApiUsers",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });

            // Authorization handlers and policies
            services.AddSingleton<IAuthorizationHandler, UserOwnerOrAdminHandler>();
            services.AddSingleton<IAuthorizationHandler, HasUserPermissionsHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("UserOwnerOrAdmin", p => p.Requirements.Add(new UserOwnerOrAdminRequirement()));
                options.AddPolicy("AddEditDoctors", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eAddEditDoctors)));
                options.AddPolicy("ManagePatients", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManagePatients)));
                options.AddPolicy("ManageAppointments", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManageAppointments)));
                options.AddPolicy("ManagePayments", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManagePayments)));
                options.AddPolicy("ShowMedicalRecords", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eShowMedicalRecords)));
                options.AddPolicy("AddEditMedicalRecords", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eAddEditMedicalRecords)));
                options.AddPolicy("ManageUsers", p => p.Requirements.Add(new HasUserPermissionsRequirement((int)IUserService.enPermissions.eManageUsers)));
            });

            return services;
        }

        public static IServiceCollection AddHospitalSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        new string[] { }
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddHospitalRateLimiting(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddPolicy("AuthLimiter", ctx =>
                {
                    var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions { PermitLimit = 5, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 });
                });

                options.AddPolicy("CriticalOpsLimiter", ctx =>
                {
                    var uid = ctx.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(uid, _ => new FixedWindowRateLimiterOptions { PermitLimit = 30, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 });
                });

                options.AddPolicy("LightOpsLimiter", ctx =>
                {
                    var uid = ctx.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(uid, _ => new FixedWindowRateLimiterOptions { PermitLimit = 100, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 });
                });
            });
            return services;
        }

        public static IServiceCollection AddHospitalCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("HospitalSystemApiCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins("http://127.0.0.1:5500", "http://localhost:5109")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
