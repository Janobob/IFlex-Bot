using iFlex_Bot.Data.Repositories;
using iFlex_Bot.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace iFlex_Bot.Data.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
        {
            return services.AddSingleton(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options));
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IChannelUpdateLogRepository, ChannelUpdateLogRepository>();
            services.AddSingleton<IIFlexDiscordUserRepository, IFlexDiscordUserRepository>();
            services.AddSingleton<IActivityLevelRepository, ActivityLevelRepository>();
            services.AddSingleton<IApplicationStatusRepository, ApplicationStatusRepository>();

            return services;
        }
    }
}
