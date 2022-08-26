using CleanUp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure.Persistance.Configuration
{
    public class CleanUpUserConfiguration : IEntityTypeConfiguration<CleanUpUser>
    {
        public void Configure(EntityTypeBuilder<CleanUpUser> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired()
                .HasDefaultValue(string.Empty)
                .HasMaxLength(100);

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasDefaultValue(string.Empty)
                .HasMaxLength(100);

            builder.Property(x => x.FullName)
                .HasComputedColumnSql("(concat([LastName],case when [LastName]<>'' AND [FirstName]<>'' then ' ' else '' end,[FirstName]))");

        }
    }
}
