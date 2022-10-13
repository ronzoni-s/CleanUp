﻿using CleanUp.Domain.Entities;
using fbognini.Core.Interfaces;
using fbognini.Infrastructure.Identity.Persistence;
using fbognini.Infrastructure.Persistence;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanUp.Infrastructure.Persistance
{
    public class CleanUpDbContext : AuditableContext<CleanUpDbContext, CleanUpUser, CleanUpRole, string>, IDataProtectionKeyContext
    {
        public CleanUpDbContext(
            DbContextOptions<CleanUpDbContext> options,
            ITenantInfo tenantInfo,
            ICurrentUserService currentUserService)
            : base(options, tenantInfo, currentUserService)
        {

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<CleaningOperation> CleaningOperations { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder
            //    .HasDbFunction(() => ApplicationDbContextSFExtensions.ValidateValidation(default(int), default(int), default(int?), default(int?), default(int?)));
            //builder.HasDefaultSchema("dbo");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
