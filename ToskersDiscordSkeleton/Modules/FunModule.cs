using Discord.Commands;
using System.Threading.Tasks;
using ToskersDiscordSkeleton.Services;

namespace ToskersDiscordSkeleton.Modules
{
    public class FunModule : ModuleBase
    {
        private FunService service;

        public FunModule(FunService funService)
        {
            service = funService;
        }

        [Command("rateme", RunMode = RunMode.Async)]
        public async void RateMe()
        {
            var response = await Task.Run(() => service.RateMe());
            await ReplyAsync(response, false);
        }

        [Command("mypenis", RunMode = RunMode.Async)]
        public async void MyPenis()
        {
            var response = await Task.Run(() => service.MyPenis());
            await ReplyAsync(response, false);
        }
    }
}