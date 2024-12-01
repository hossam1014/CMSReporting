using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class IssueCategoryConfiguration : IEntityTypeConfiguration<IssueCategory>
    {
        public void Configure(EntityTypeBuilder<IssueCategory> builder)
        {
            builder.Property(x => x.NameAR).HasMaxLength(100);
            builder.Property(x => x.NameEN).HasMaxLength(100);
            builder.Property(x => x.Key).HasMaxLength(50);
        }
    }
}