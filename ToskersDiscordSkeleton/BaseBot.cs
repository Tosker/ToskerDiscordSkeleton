using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using ToskersDiscordSkeleton.Services;

namespace ToskersDiscordSkeleton
{
    public class BaseBot
    {
        public IServiceProvider Service;
        public DiscordSocketClient Client;
        private CommandHandler commandHandler;
        private ConfigurationService configService;

        public virtual async Task ConnectAsync(ConfigurationService config)
        {
            configService = config;

            //Setup requirements
            SetupClient();
            await SetupServices();

            commandHandler = new CommandHandler(Service);

            //Login and start the client
            await ClientLogin(config.Token);

            await Task.Delay(-1);
        }

        private void SetupClient()
        {
            var config = GetSocketConfiguration();
            Client = new DiscordSocketClient(config);
            Client.Log += Log;
        }

        private DiscordSocketConfig GetSocketConfiguration()
        {
            var config = new DiscordSocketConfig();
            config.LogLevel = LogSeverity.Debug;
            config.AlwaysDownloadUsers = true;
            return config;
        }

        private async Task SetupServices()
        {
            var commands = new CommandService();
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());

            Service = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(commands)
                .AddSingleton(configService)
                .AddSingleton<AudioService>()
                .AddSingleton<FunService>()
            .BuildServiceProvider();
        }

        private async Task ClientLogin(string token)
        {
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            Client.Ready += () =>
            {
                return Task.CompletedTask;
            };
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}