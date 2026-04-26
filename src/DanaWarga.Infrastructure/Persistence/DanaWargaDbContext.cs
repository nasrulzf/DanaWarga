using DanaWarga.Domain.Entities;
using DanaWarga.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using BillingPeriodEntity = DanaWarga.Domain.Entities.Period;

namespace DanaWarga.Infrastructure.Persistence;

public sealed class DanaWargaDbContext(DbContextOptions<DanaWargaDbContext> options) : DbContext(options)
{
    public DbSet<Resident> Residents => Set<Resident>();
    public DbSet<House> Houses => Set<House>();
    public DbSet<BillingPeriodEntity> Periods => Set<BillingPeriodEntity>();
    public DbSet<IplPriceConfiguration> IplPriceConfigurations => Set<IplPriceConfiguration>();
    public DbSet<IplPayment> IplPayments => Set<IplPayment>();
    public DbSet<IplPaymentAllocation> IplPaymentAllocations => Set<IplPaymentAllocation>();
    public DbSet<Income> Incomes => Set<Income>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<FinancialPeriod> FinancialPeriods => Set<FinancialPeriod>();
    public DbSet<FinancialPeriodSnapshot> FinancialPeriodSnapshots => Set<FinancialPeriodSnapshot>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<AppUserRole> UserRoles => Set<AppUserRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureMoney(modelBuilder.Entity<IplPriceConfiguration>().OwnsOne(x => x.Amount));
        ConfigureMoney(modelBuilder.Entity<IplPayment>().OwnsOne(x => x.TotalAmount));
        ConfigureMoney(modelBuilder.Entity<IplPaymentAllocation>().OwnsOne(x => x.AllocatedAmount));
        ConfigureMoney(modelBuilder.Entity<Income>().OwnsOne(x => x.Amount));
        ConfigureMoney(modelBuilder.Entity<Expense>().OwnsOne(x => x.Amount));
        ConfigureMoney(modelBuilder.Entity<FinancialPeriodSnapshot>().OwnsOne(x => x.TotalIncome));
        ConfigureMoney(modelBuilder.Entity<FinancialPeriodSnapshot>().OwnsOne(x => x.TotalExpense));
        ConfigureMoney(modelBuilder.Entity<FinancialPeriodSnapshot>().OwnsOne(x => x.EndingBalance));

        modelBuilder.Entity<BillingPeriodEntity>()
            .HasIndex(x => new { x.Year, x.Month })
            .IsUnique();

        modelBuilder.Entity<FinancialPeriod>()
            .HasIndex(x => new { x.Year, x.Month })
            .IsUnique();

        modelBuilder.Entity<FinancialPeriodSnapshot>()
            .HasIndex(x => new { x.Year, x.Month })
            .IsUnique();

        modelBuilder.Entity<AppUserRole>()
            .HasKey(x => new { x.AppUserId, x.RoleName });

        modelBuilder.Entity<AppUserRole>()
            .HasOne(x => x.AppUser)
            .WithMany(x => x.Roles)
            .HasForeignKey(x => x.AppUserId);

        modelBuilder.Entity<House>()
            .HasOne(x => x.Resident)
            .WithMany(x => x.Houses)
            .HasForeignKey(x => x.ResidentId);

        modelBuilder.Entity<IplPriceConfiguration>()
            .HasOne(x => x.House)
            .WithMany(x => x.PriceConfigurations)
            .HasForeignKey(x => x.HouseId);

        modelBuilder.Entity<IplPaymentAllocation>()
            .HasOne(x => x.Payment)
            .WithMany(x => x.Allocations)
            .HasForeignKey(x => x.PaymentId);
    }

    private static void ConfigureMoney<T>(Microsoft.EntityFrameworkCore.Metadata.Builders.OwnedNavigationBuilder<T, Money> builder)
        where T : class
    {
        builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Currency).HasMaxLength(3);
    }
}