using DanaWarga.Application;
using DanaWarga.Infrastructure;
using DanaWarga.Infrastructure.Persistence;
using DanaWarga.Infrastructure.Security;
using DanaWarga.WebApi.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DanaWargaDbContext>();
    var hasher = scope.ServiceProvider.GetRequiredService<PasswordHasher>();
    await DataSeeder.SeedAsync(dbContext, hasher, CancellationToken.None);
}

app.MapControllers();
app.Run();
