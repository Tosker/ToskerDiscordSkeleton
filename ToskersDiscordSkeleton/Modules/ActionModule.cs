using Discord;
using Discord.Commands;
using System.Collections.Generic;

namespace ToskersDiscordSkeleton.Modules
{
    public class ActionModule : ModuleBase
    {
        [Command("prune", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async void Prune(IUser user, int amount = 100)
        {
            var messages = await Context.Channel.GetMessagesAsync(amount).Flatten();
            List<IMessage> deleteCollection = new List<IMessage>();
            foreach (var message in messages)
            {
                if (message.Author == user)
                    deleteCollection.Add(message);
            }
            await Context.Channel.DeleteMessagesAsync(deleteCollection);
        }
    }
}