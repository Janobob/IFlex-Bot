using Discord;
using Discord.Commands;
using iFlex_Bot.Bot.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iFlex_Bot.Bot.Modules
{
    [Group("help")]
    [Summary("Rufe die Hilfe über die Kommands auf")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly BotConfiguration _config;

        public HelpModule(CommandService service, BotConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [Command("")]
        [Summary("Liste allen verfügbaren Kommandos")]
        public async Task HelpAsync()
        {
            string prefix = _config.Prefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = "Diese Befehle kannst du benutzen:"
            };

            foreach (var module in _service.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += $"{prefix}{string.Join(", ", cmd.Aliases)} \"{string.Join(", ", cmd.Parameters)}\" - {cmd.Summary}\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    builder.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("")]
        [Summary("Detailierte Infos über Kommandos")]
        public async Task HelpAsync([Remainder]string command)
        {
            var result = _service.Search(Context, command);

            if (!result.IsSuccess)
            {
                await ReplyAsync($"Entschuldige, ich konnte kein Kommand mit dem Namen **{command}** finden.");
                return;
            }

            string prefix = _config.Prefix;
            var builder = new EmbedBuilder()
            {
                Color = new Color(114, 137, 218),
                Description = $"Hier sind Kommandos wie **{command}**"
            };

            foreach (var match in result.Commands)
            {
                var cmd = match.Command;

                builder.AddField(x =>
                {
                    x.Name = string.Join(", ", cmd.Aliases);
                    x.Value = $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name))}\n" +
                              $"Summary: {cmd.Summary}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync("", false, builder.Build());
        }
    }
}
