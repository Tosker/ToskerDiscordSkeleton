using System;
using System.Threading.Tasks;
using ToskersDiscordSkeleton.Services;

namespace ToskersDiscordSkeleton
{
    internal class Program
    {
        private static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            //TODO: Cleaner way of establishing which config.json to be used.
            string configPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\resources\\ExampleBot.json"; //Visit this file in 'resources' and enter bot token
            var config = ConfigurationService.Load(configPath);

            BaseBot bot = new BaseBot();
            await bot.ConnectAsync(config);
        }
    }
}