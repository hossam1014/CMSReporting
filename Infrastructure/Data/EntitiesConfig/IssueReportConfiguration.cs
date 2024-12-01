using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class IssueReportConfiguration : IEntityTypeConfiguration<IssueReport>
    {
        public void Configure(EntityTypeBuilder<IssueReport> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.Address).HasMaxLength(100);
        }
    }
}