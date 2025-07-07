//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Infrastructure.Data.EntitiesConfig
//{
//    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
//    {
//        public void Configure(EntityTypeBuilder<Notification> builder)
//        {
//            builder.Property(x => x.TitleAR).HasMaxLength(50);
//            builder.Property(x => x.TitleEN).HasMaxLength(50);
//            builder.Property(x => x.ContentAR).HasMaxLength(100);
//            builder.Property(x => x.ContentEN).HasMaxLength(100);
//        }
//    }
//}