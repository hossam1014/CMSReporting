using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class DataContext : IdentityDbContext<AppUser,
                                                    AppRole,
                                                    int,
                                                    IdentityUserClaim<int>,
                                                    AppUserRole,
                                                    IdentityUserLogin<int>,
                                                    IdentityRoleClaim<int>,
                                                    IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<DashboardUser> DashboardUsers { get; set; }
        public DbSet<MobileUser> MobileUsers { get; set; }
        public DbSet<EmergencyReport> EmergencyReports { get; set; }
        public DbSet<EmergencyService> EmergencyServices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<IssueCategory> IssueCategories { get; set; }
        public DbSet<IssueReport> IssueReports { get; set; }
        public DbSet<IssueReportStatusHistory> ReportStatusHistories { get; set; }
        public DbSet<SocialMediaReport> SocialMediaReports { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);


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


        }
    }
}