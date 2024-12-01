using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>

    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(100);
            builder.Property(x => x.DefaultLanguage).HasMaxLength(2);
        }
    }
}