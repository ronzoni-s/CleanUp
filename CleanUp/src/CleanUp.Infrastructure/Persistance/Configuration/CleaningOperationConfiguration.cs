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
    public class CleaningOperationConfiguration : IEntityTypeConfiguration<CleaningOperation>
    {
        public void Configure(EntityTypeBuilder<CleaningOperation> builder)
        {
            builder.ToTable(nameof(CleanUpDbContext.CleaningOperations), "dbo");

        }
    }
}
