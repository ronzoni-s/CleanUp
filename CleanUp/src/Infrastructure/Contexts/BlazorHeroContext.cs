using CleanUp.Application.Interfaces.Services;
using CleanUp.Infrastructure.Models.Identity;
using CleanUp.Domain.Contracts;
using CleanUp.Domain.Entities.Catalog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanUp.Domain.Entities;
using System.Reflection;

namespace CleanUp.Infrastructure.Contexts
{
    public class BlazorHeroContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public BlazorHeroContext(DbContextOptions<BlazorHeroContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }


        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresss { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderMenu> OrderMenus { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<ReceiptEod> ReceiptEods { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<EmailConfig> EmailConfigs { get; set; }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.LastUpdated = _dateTimeService.NowUtc;
                        entry.Entity.LastUpdatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastUpdated = _dateTimeService.NowUtc;
                        entry.Entity.LastUpdatedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //Assembly assemblyWithConfigurations = GetType().Assembly;
            //builder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);

            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
            
            builder.Entity<BlazorHeroUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<BlazorHeroRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<BlazorHeroRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });


            //
            builder.Entity<CustomerAddress>()
                .HasOne<Customer>(ca => ca.Customer)
                .WithMany(c => c.CustomerAddresss)
                .HasForeignKey(ca => ca.CustomerId);

            builder.Entity<OrderMenu>()
                .HasOne<Order>(op => op.Order)
                .WithMany(o => o.OrderMenus)
                .HasForeignKey(op => op.OrderId);

            builder.Entity<OrderProduct>()
                .HasOne<Order>(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .HasForeignKey(op => op.OrderId);

            builder.Entity<OrderProduct>()
                .HasOne<Product>(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .HasForeignKey(op => op.ProductId);

            builder.Entity<Order>(entity =>
            {
                entity.Property(x => x.ReceiptTime)
                    .HasColumnType("datetime2(0)");

                entity.Property(x => x.ReceiptErrorDescription)
                    .HasMaxLength(200);

                entity.Property(x => x.VoidReceiptTime)
                    .HasColumnType("datetime2(0)");

                entity.Property(x => x.VoidReceiptErrorDescription)
                    .HasMaxLength(200);

                entity.Property(x => x.OrderDate).HasColumnType("date");

                entity
                    .HasOne<CustomerAddress>(o => o.CustomerAddress)
                    .WithMany(ca => ca.Orders)
                    .HasForeignKey(o => o.CustomerAddressId);
            });

            builder.Entity<Product>(entity =>
            {
                entity.Property(x => x.Code)
                    .HasMaxLength(20);

                entity.Property(x => x.Name)
                    .HasMaxLength(100);

                entity.Property(x => x.Place)
                    .HasMaxLength(100);
            });

            builder.Entity<ReceiptEod>(entity =>
            {
                entity.Property(x => x.Id)
                    .IsFixedLength()
                    .HasMaxLength(36);
            });
        }
    }
}