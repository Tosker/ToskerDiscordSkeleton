using Discord.Commands;
using System;

namespace ToskersDiscordSkeleton.Modules
{
    public class GenericModule : ModuleBase
    {
        private static Random random = new Random();

        [Command("random", RunMode = RunMode.Async)]
        public async void Random(params int[] range)
        {
            var randomNumber = random.Next(range[0], range[1]).ToString();
            await ReplyAsync($"I think I'll pick... {randomNumber}", false);
        }

        [Command("hello", RunMode = RunMode.Async)]
        public async void Greeting()
        {
            var username = Context.Message.Author.Username;
            await ReplyAsync($"Hello to you as well, {username}", false);
        }
    }
}