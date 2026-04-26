using System.Text;
using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Abstractions.Security;
using DanaWarga.Infrastructure.Persistence;
using DanaWarga.Infrastructure.Repositories;
using DanaWarga.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DanaWarga.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Missing DefaultConnection string.");

        services.AddDbContext<DanaWargaDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        var jwt = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
            ?? throw new InvalidOperationException("Missing JWT configuration.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret))
                };
            });

        services.AddAuthorization();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IResidentRepository, ResidentRepository>();
        services.AddScoped<IHouseRepository, HouseRepository>();
        services.AddScoped<IPeriodRepository, PeriodRepository>();
        services.AddScoped<IIplPriceConfigurationRepository, IplPriceConfigurationRepository>();
        services.AddScoped<IIplPaymentRepository, IplPaymentRepository>();
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<PasswordHasher>();
        services.AddScoped<IPasswordHasher>(sp => sp.GetRequiredService<PasswordHasher>());
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}