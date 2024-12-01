using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfig
{
    public class NotificationUserConfiguration : IEntityTypeConfiguration<NotificationUser>
    {
        public void Configure(EntityTypeBuilder<NotificationUser> builder)
        {
            builder.HasKey(x => new { x.NotificationId, x.UserId });

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.NotificationUsers)
                .HasForeignKey(x => x.NotificationId)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.NotificationUsers)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

        }
    }
}