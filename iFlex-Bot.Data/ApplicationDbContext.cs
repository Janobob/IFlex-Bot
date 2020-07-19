using iFlex_Bot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace iFlex_Bot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ChannelUpdateLog> ChannelUpdateLogs { get; set; }
        public DbSet<IFlexDiscordUser> IFlexDiscordUsers { get; set; }
        public DbSet<ActivityLevel> ActivityLevels { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationStatus>().HasData(
                new ApplicationStatus
                {
                    Id = 1,
                    IssueTime = DateTime.Now,
                    Type = ApplicationStatusType.Stopped
                });

            modelBuilder.Entity<ActivityLevel>().HasData(
                new ActivityLevel
                {
                    Id = 1,
                    Order = 1,
                    SecondsToAchieve = TimeSpan.FromMinutes(5).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 2,
                    Order = 2,
                    SecondsToAchieve = TimeSpan.FromHours(2).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 3,
                    Order = 3,
                    SecondsToAchieve = TimeSpan.FromHours(12).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 4,
                    Order = 4,
                    SecondsToAchieve = TimeSpan.FromDays(1).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 5,
                    Order = 5,
                    SecondsToAchieve = TimeSpan.FromDays(7).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 6,
                    Order = 6,
                    SecondsToAchieve = TimeSpan.FromDays(28).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 7,
                    Order = 7,
                    SecondsToAchieve = TimeSpan.FromDays(31).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 8,
                    Order = 8,
                    SecondsToAchieve = TimeSpan.FromDays(182).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 9,
                    Order = 1,
                    SecondsToAchieve = TimeSpan.FromDays(365).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 10,
                    Order = 10,
                    SecondsToAchieve = TimeSpan.FromDays(1000).TotalSeconds
                }
            );
        }
    }
}
