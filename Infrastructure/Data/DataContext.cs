using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class DataContext : IdentityDbContext<AppUser,
                                                    AppRole,
                                                    string,
                                                    IdentityUserClaim<string>,
                                                    AppUserRole,
                                                    IdentityUserLogin<string>,
                                                    IdentityRoleClaim<string>,
                                                    IdentityUserToken<string>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<MobileUser> MobileUsers { get; set; }
        public DbSet<EmergencyReport> EmergencyReports { get; set; }
        public DbSet<EmergencyService> EmergencyServices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<IssueCategory> IssueCategories { get; set; }
        public DbSet<IssueReport> IssueReports { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }


        public DbSet<IssueReportStatusHistory> ReportStatusHistories { get; set; }

        public DbSet<SocialMediaReport> SocialMediaReports { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);

            //builder.Entity<IssueReport>()
            //    .HasDiscriminator<EReportType>("ReportType")
            //    .HasValue<IssueReport>(EReportType.None)
            //    .HasValue<SocialMediaReport>(EReportType.SocialMedia);
            //.HasValue<EmergencyReport>(EReportType.Emergency);
            

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

            builder.Entity<FeedBack>()
                  .HasOne(f => f.MobileUser)
                  .WithMany(u => u.FeedBacks)
                  .HasForeignKey(f => f.MobileUserId)
                  .IsRequired(false);

            builder.Entity<UserCategory>()
                 .HasKey(uc => new { uc.UserId, uc.CategoryId });

            builder.Entity<UserCategory>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCategories)
                .HasForeignKey(uc => uc.UserId);

            builder.Entity<UserCategory>()
                .HasOne(uc => uc.Category)
                .WithMany(c => c.UserCategories)
                .HasForeignKey(uc => uc.CategoryId);


        }
    }
}