using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using DanaWarga.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using BillingPeriodEntity = DanaWarga.Domain.Entities.Period;

namespace DanaWarga.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(DanaWargaDbContext dbContext, PasswordHasher passwordHasher, CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (!await dbContext.Users.AnyAsync(cancellationToken))
        {
            var treasurer = new AppUser("treasurer", passwordHasher.Hash("Pass123$"), "System Treasurer");
            treasurer.Roles.Add(new AppUserRole(treasurer.Id, "Treasurer"));
            treasurer.Roles.Add(new AppUserRole(treasurer.Id, "Committee"));

            var residentUser = new AppUser("resident", passwordHasher.Hash("Pass123$"), "Default Resident");
            residentUser.Roles.Add(new AppUserRole(residentUser.Id, "Resident"));

            await dbContext.Users.AddRangeAsync([treasurer, residentUser], cancellationToken);
        }

        if (!await dbContext.Residents.AnyAsync(cancellationToken))
        {
            var resident = new Resident("Default Resident", "resident@danawarga.local", "08123456789");
            await dbContext.Residents.AddAsync(resident, cancellationToken);

            var house = new House("A-01", "Cluster Mawar", resident.Id);
            await dbContext.Houses.AddAsync(house, cancellationToken);

            for (var month = 1; month <= 12; month++)
            {
                await dbContext.Periods.AddAsync(new BillingPeriodEntity(DateTime.UtcNow.Year, month), cancellationToken);
            }

            await dbContext.IplPriceConfigurations.AddRangeAsync(
            [
                new IplPriceConfiguration(house.Id, new Money(150000m), new DateTime(DateTime.UtcNow.Year, 1, 1), new DateTime(DateTime.UtcNow.Year, 3, 31)),
                new IplPriceConfiguration(house.Id, new Money(180000m), new DateTime(DateTime.UtcNow.Year, 4, 1), null)
            ], cancellationToken);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}