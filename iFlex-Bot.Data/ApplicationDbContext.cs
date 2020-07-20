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
                    Name = "Neuankömmling",
                    GuestMessage = "Schon 5 Minuten ist es her, dass du auf dem Server gejoinet bis. Vermutlich ein gutes Zeichen dass wir dich noch nicht vertrieben haben. Wir freuen uns auf eine tolle Zeit mit dir!",
                    MemberMessage = "Deine aktuelle Zeit wird nun gemessen, desto länger du auf dem Server bist, umso höher wird dein Level. Was bringt dir das fragst du dich? - Naja, nichts ...",
                    Order = 1,
                    SecondsToAchieve = TimeSpan.FromMinutes(5).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 2,
                    Name = "Fremder",
                    GuestMessage = "Zwei Stunden sind vergangen, aber vermutlich kennst du noch nicht alle - wird Zeit dies zu ändern!",
                    MemberMessage = "Zwei Stunden ist eine lange Zeit, schon mal überlegt eine Pause einzulegen?",
                    Order = 2,
                    SecondsToAchieve = TimeSpan.FromHours(2).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 3,
                    Name = "Bekanntes Gesicht",
                    GuestMessage = "Du scheinst schon lange auf diesem Server zu spielen, schon mal über einen Beitritt nachgedacht? Schreibe mir doch ==> !flex invite oder gehe auf die Iflex Website (https://iflexesports.ch)",
                    MemberMessage = "Ein langer Sprint du hinter dir hast, aber noch ein weiter Weg vor dir junger Padawan.",
                    Order = 3,
                    SecondsToAchieve = TimeSpan.FromHours(12).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 4,
                    Name = "3 Tage Bart",
                    GuestMessage = "Dir stehe schon die ersten Haare im Bart und immer noch kein Mitglied?",
                    MemberMessage = "Noch etwas länger und du hast eine schöne Bartpracht",
                    Order = 4,
                    SecondsToAchieve = TimeSpan.FromDays(3).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 5,
                    Name = "Eine Woche im Koma",
                    GuestMessage = "Sonntags bis Freitags durchgeschlafen, was für einen angenehmen Schlaf das war.",
                    MemberMessage = "Sonntags bis Freitags durchgeschlafen, was für einen angenehmen Schlaf das war.",
                    Order = 5,
                    SecondsToAchieve = TimeSpan.FromDays(7).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 6,
                    Name = "Februarkind",
                    GuestMessage = "Du hast schon 28 Tage Spielzeit - so viele Tage wie Februar Tage hat.",
                    MemberMessage = "Du hast schon 28 Tage Spielzeit - so viele Tage wie Februar Tage hat.",
                    Order = 6,
                    SecondsToAchieve = TimeSpan.FromDays(28).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 7,
                    Name = "Lange Sommerferien",
                    GuestMessage = "Einen ganzen Monat ist es her, da habe ich angefangen zu zählen wie lange du bei uns bist - immer noch nicht überzeugt?",
                    MemberMessage = "Die Sommerferien sind vorbei - ran an die Arbeit!",
                    Order = 7,
                    SecondsToAchieve = TimeSpan.FromDays(31).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 8,
                    Name = "Harte Zeiten",
                    GuestMessage = "Ein halbes Jahr - ist doch nicht so viel oder?",
                    MemberMessage = "Ein halbes Jahr - ist doch nicht so viel oder?",
                    Order = 8,
                    SecondsToAchieve = TimeSpan.FromDays(182).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 9,
                    Name = "Hardcore Gamer",
                    GuestMessage = "Nur die Starken halten durch - du hast ein ganzes Jahr hier auf dem Server verbracht und dies ist nur die aktive Zeit ...",
                    MemberMessage = "Nur die Starken halten durch - du hast ein ganzes Jahr hier auf dem Server verbracht und dies ist nur die aktive Zeit ...",
                    Order = 9,
                    SecondsToAchieve = TimeSpan.FromDays(365).TotalSeconds
                },
                new ActivityLevel
                {
                    Id = 10,
                    Name = "Suchthuufe",
                    GuestMessage = "Mir fehlen die Worte, 1000 Tage - wenn du dies erreichst solltest du mal mit mir ein ernstes Gespräch führen ...",
                    MemberMessage = "Mir fehlen die Worte, 1000 Tage - wenn du dies erreichst solltest du mal mit mir ein ernstes Gespräch führen ...",
                    Order = 10,
                    SecondsToAchieve = TimeSpan.FromDays(1000).TotalSeconds
                }
            );
        }
    }
}
