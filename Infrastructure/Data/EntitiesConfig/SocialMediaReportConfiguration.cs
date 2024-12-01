using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class SocialMediaReportConfiguration : IEntityTypeConfiguration<SocialMediaReport>
    {
        public void Configure(EntityTypeBuilder<SocialMediaReport> builder)
        {
            builder.Property(x => x.Content).HasMaxLength(1000);
        }
    }
}