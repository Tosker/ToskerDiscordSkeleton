using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToskersDiscordSkeleton.Embeds
{
    public class GuildTemplate
    {
        private IGuild guild { get; set; }

        public int TotalUsers { get; private set; }
        public int OnlineUsers { get; private set; }

        public IGuildUser Owner { get; private set; }
        public IReadOnlyCollection<IGuildUser> Users { get; private set; }
        public IReadOnlyCollection<ITextChannel> TextChannels { get; private set; }
        public IReadOnlyCollection<IVoiceChannel> VoiceChannels { get; private set; }

        public async void LoadGuild(IGuild guild)
        {
            this.guild = guild;
            await CopyGuildObject();
        }

        private async Task CopyGuildObject()
        {
            await GetAsyncCollections();
            TotalUsers = Users.Count;
            OnlineUsers = Users.Where(x => x.Status == UserStatus.Online).Count();
        }

        private async Task GetAsyncCollections()
        {
            Owner = await guild.GetOwnerAsync();
            Users = await guild.GetUsersAsync();

            TextChannels = await guild.GetTextChannelsAsync();
            VoiceChannels = await guild.GetVoiceChannelsAsync();
        }

        public EmbedBuilder GetEmbedBuilder()
        {
            var builder = new EmbedBuilder();
            builder.AddField(guild.Name, "Welcome!");
            builder.ThumbnailUrl = guild.IconUrl;

            builder.AddInlineField("Region", guild.VoiceRegionId);
            builder.AddInlineField("Members", $"{OnlineUsers}/{TotalUsers}");
            builder.AddInlineField("Roles", guild.Roles.Count);
            builder.AddInlineField("Owner", Owner.Username);
            builder.AddInlineField("Text Channels", TextChannels.Count);
            builder.AddInlineField("Voice Channels", VoiceChannels.Count);

            return builder;
        }
    }
}