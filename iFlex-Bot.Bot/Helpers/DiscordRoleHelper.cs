using Discord.WebSocket;
using iFlex_Bot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Helpers
{
    public static class DiscordRoleHelper
    {
        public static async Task GenerateActivityRoleForDiscordServerAsync(DiscordSocketClient discord, IEnumerable<ActivityLevel> activityLevels)
        {
            var iflexServer = discord.Guilds.FirstOrDefault(x => x.Name == "iFlex Esports");

            foreach (var activityLevel in activityLevels)
            {
                await iflexServer.CreateRoleAsync($"Level {activityLevel.Order} - {activityLevel.Name}", null, Discord.Color.DarkRed, false, false, null);
            }
        }

        public static async Task CleanupActivityRolesAsync(DiscordSocketClient discord)
        {
            foreach(var role in discord.Guilds.FirstOrDefault(x => x.Name == "iFlex Esports").Roles.Where(x => x.Name.StartsWith("Level")))
            {
                await role.DeleteAsync();
            }
        }
    }
}
