using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class FeedBackConfiguration : IEntityTypeConfiguration<FeedBack>
    {
        public void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            builder.Property(x => x.Comment).HasMaxLength(500);
            
        }
    }
}