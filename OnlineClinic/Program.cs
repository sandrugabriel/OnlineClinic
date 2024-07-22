using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineClinic.Appointments.Repositoy.interfaces;
using OnlineClinic.Appointments.Repositoy;
using OnlineClinic.Appointments.Services.interfaces;
using OnlineClinic.Appointments.Services;
using OnlineClinic.Customers.Models;
using OnlineClinic.Customers.Repository.interfaces;
using OnlineClinic.Customers.Repository;
using OnlineClinic.Data;
using OnlineClinic.Services.Repository.interfaces;
using OnlineClinic.Services.Repository;
using OnlineClinic.Services.Services.interfaces;
using OnlineClinic.Services.Services;
using System;
using System.Text;
using OnlineClinic.Customers.Services.interfaces;
using OnlineClinic.Customers.Services;
using OnlineClinic.Doctors.Service.interfaces;
using OnlineClinic.Doctors.Repository.interfaces;
using OnlineClinic.Doctors.Service;
using OnlineClinic.Doctors.Repository;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        builder.Services.AddScoped<IRepositoryCustomer, RepositoryCustomer>();
        builder.Services.AddScoped<ICustomerCommandService, CustomerCommandService>();
        builder.Services.AddScoped<ICustomerQueryService, CustomerQueryService>();

        builder.Services.AddScoped<IRepositoryDoctor, RepositoryDoctor>();
        builder.Services.AddScoped<IDoctorCommandService, DoctorCommandService>();
        builder.Services.AddScoped<IDoctorQueryService, DoctorQueryService>();

        builder.Services.AddScoped<IRepositoryService, RepositoryService>();
        builder.Services.AddScoped<IServiceQueryService, ServiceQueryService>();
        builder.Services.AddScoped<IServiceCommandService, ServiceCommandService>();

        builder.Services.AddScoped<IRepositoryAppointment, RepositoryAppointment>();
        builder.Services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();


        builder.Services.AddDbContext<AppDbContext>(op => op.UseMySql(builder.Configuration.GetConnectionString("Default")!,
         new MySqlServerVersion(new Version(8, 0, 21))), ServiceLifetime.Scoped);

        builder.Services.AddIdentity<Customer, IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.AddMySql5().WithGlobalConnectionString(builder.Configuration.GetConnectionString("Default"))
            .ScanIn(typeof(Program).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
        });
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Api", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference{
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        }
                    },
                new string[] { }
                }
            });

        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }

        app.Run();
    }
}