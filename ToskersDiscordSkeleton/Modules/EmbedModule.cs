using Discord.Commands;
using ToskersDiscordSkeleton.Embeds;

namespace ToskersDiscordSkeleton.Modules
{
    public class EmbedModule : ModuleBase
    {
        [Command("serverinfo", RunMode = RunMode.Async)]
        [RequireContext(ContextType.Guild)]
        public async void TestEmbed()
        {
            var guildTemplate = new GuildTemplate();
            guildTemplate.LoadGuild(Context.Guild);

            var embed = guildTemplate.GetEmbedBuilder();

            await ReplyAsync(string.Empty, false, embed);
        }
    }
}