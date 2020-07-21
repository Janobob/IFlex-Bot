using Discord.Commands;
using iFlex_Bot.Data.Repositories.Contracts;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Modules
{
    [Group("settings")]
    [Summary("Ändere deine persönlichen Einstellungen vom Bot.")]
    public class SettingsModule : ModuleBase<SocketCommandContext>
    {
        public IIFlexDiscordUserRepository IFlexDiscordUserRepository { get; set; }
        
        [Command("mute")]
        [Alias("m")]
        [Summary("Mute den Bot, damit du keine persönlichen Nachrichten mehr erhälst.")]
        public async Task Mute()
        {
            var user = await IFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(Context.User.Id);

            user.AllowMessages = false;

            await IFlexDiscordUserRepository.SaveChangesAsync();

            await ReplyAsync("Du erhälst nun keine Benachrichtungen mehr von mir.");
        }

        [Command("unmute")]
        [Alias("um")]
        [Summary("Unmutte den Bot, damit du wieder persönlichen Nachrichten erhälst.")]
        public async Task Unmute()
        {
            var user = await IFlexDiscordUserRepository.GetIFlexDiscordUserByDiscordIdAsync(Context.User.Id);

            user.AllowMessages = true;

            await IFlexDiscordUserRepository.SaveChangesAsync();

            await ReplyAsync("Du erhälst nun wieder Benachrichtungen von mir.");
        }
    }
}
