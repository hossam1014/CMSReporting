using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class EmergencyServiceConfiguration : IEntityTypeConfiguration<EmergencyService>
    {
        public void Configure(EntityTypeBuilder<EmergencyService> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100);
            builder.Property(x => x.ContactNumber).HasMaxLength(20);
            builder.Property(x => x.Address).HasMaxLength(100);
        }
    }
}