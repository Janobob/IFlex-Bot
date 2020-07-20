using Discord.Commands;
using iFlex_Bot.Data.Repositories;
using iFlex_Bot.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Modules
{
    [Group("playtime")]
    [Alias("time", "t")]
    public class PlayTimeModule : ModuleBase<SocketCommandContext>
    {
        public IIFlexDiscordUserRepository IFlexDiscordUserRepository { get; set; }
        public IActivityLevelRepository ActivityLevelRepository { get; set; }

        [Command("")]
        public async Task GetPlayTime()
        {
            var user = await IFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(Context.User.Id);

            await ReplyAsync($"Du bist Level {user.Level} mit {user.PlayTimeInSeconds} Sekunden akiver Zeit auf dem Server.");
        }

        [Command("tonextLevel")]
        [Alias("next", "n")]
        public async Task GetTimeToNextLevel()
        {
            var user = await IFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(Context.User.Id);
            var nextActivityLevel = (await ActivityLevelRepository.GetActivityLevelsAsync()).FirstOrDefault(x => x.Order == user.Level + 1);

            if(nextActivityLevel == null)
            {
                await ReplyAsync("Du hast schon das höchste Level erreich!");
                return;
            }

            await ReplyAsync($"Du brauchst noch {nextActivityLevel.SecondsToAchieve - user.PlayTimeInSeconds} Sekunden bis zum nächsten Level.");
        }
    }
}
